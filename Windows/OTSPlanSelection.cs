using System;
using System.Drawing;
using System.Collections;
using System.Globalization;
using System.ComponentModel;
using System.Windows.Forms;
using System.Diagnostics;
using System.Data;
using System.Configuration;

using Infragistics.Win.UltraWinGrid;
using MIDRetail.Business;
using MIDRetail.Common;
using MIDRetail.Data;
using MIDRetail.DataCommon;
using MIDRetail.Windows;
using MIDRetail.Windows.Controls;

namespace MIDRetail.Windows
{
	public class OTSPlanSelection : MIDFormBase
	{
		#region Windows Form Designer generated code

		#region Windows-generated stuff

		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.ToolTip toolTip1;
		private System.Windows.Forms.ContextMenu GridContextMenu;
		private System.Windows.Forms.MenuItem mnuDelete;
		private System.Windows.Forms.GroupBox grpType;
		private System.Windows.Forms.RadioButton radStore;
		private System.Windows.Forms.RadioButton radChain;
        private System.Windows.Forms.RadioButton radChainLadder;
        private System.Windows.Forms.CheckBox chkMultiLevel;
		private System.Windows.Forms.Label lblComputationMode;
		private System.Windows.Forms.Panel pnlMain;
		private Infragistics.Win.UltraWinGrid.UltraGrid grdBasis;
		private System.Windows.Forms.Panel pnlChainSingleLevel;
        private System.Windows.Forms.GroupBox grpCSLDisplay;
		private System.Windows.Forms.Label lblCSLView;
		private System.Windows.Forms.GroupBox grpCSLOTSPlan;
        private MIDRetail.Windows.Controls.MIDDateRangeSelector mdsCSLPlanDateRange;
		private System.Windows.Forms.TextBox txtCSLChainNode;
		private System.Windows.Forms.Label lblCSLChainVersion;
		private System.Windows.Forms.Label lblCSLTimePeriod;
		private System.Windows.Forms.Label lblCSLChainNode;
		private System.Windows.Forms.Panel pnlStoreMultiLevel;
		private System.Windows.Forms.Button btnSMLVersionOverride;
		private System.Windows.Forms.GroupBox grpSMLDisplay;
		private System.Windows.Forms.CheckBox chkSMLSimilarStores;
        private System.Windows.Forms.CheckBox chkSMLIneligibleStores;
        // Begin Track #4872 - JSmith - Global/User Attributes
		// End Track #4872
		private System.Windows.Forms.Label lblSMLFilter;
		private System.Windows.Forms.Label lblSMLView;
		private System.Windows.Forms.Label lblSMLGroupBy;
		private System.Windows.Forms.Label lblSMLStoreAttribute;
        private System.Windows.Forms.GroupBox grpSMLOTSPlan;
        private MIDRetail.Windows.Controls.MIDDateRangeSelector mdsSMLPlanDateRange;
		private System.Windows.Forms.TextBox txtSMLHighLevelNode;
		private System.Windows.Forms.Label lblSMLLowLevelsVersion;
		private System.Windows.Forms.Label lblSMLHighLevelVersion;
		private System.Windows.Forms.Label lblSMLLowLevels;
		private System.Windows.Forms.Label lblSMLTimePeriod;
		private System.Windows.Forms.Label lblSMLHighLevel;
		private System.Windows.Forms.Panel pnlChainMultiLevel;
		private System.Windows.Forms.Button btnCMLVersionOverride;
        private System.Windows.Forms.GroupBox grpCMLDisplay;
		private System.Windows.Forms.Label lblCMLFilter;
		private System.Windows.Forms.Label lblCMLView;
		private System.Windows.Forms.Label lblCMLGroupBy;
        private System.Windows.Forms.GroupBox grpCMLOTSPlan;
        private MIDRetail.Windows.Controls.MIDDateRangeSelector mdsCMLPlanDateRange;
		private System.Windows.Forms.TextBox txtCMLHighLevelNode;
		private System.Windows.Forms.Label lblCMLLowLevelsVersion;
		private System.Windows.Forms.Label lblCMLHighLevelVersion;
		private System.Windows.Forms.Label lblCMLLowLevels;
		private System.Windows.Forms.Label lblCMLTimePeriod;
		private System.Windows.Forms.Label lblCMLHighLevel;
		private System.Windows.Forms.Panel pnlStoreSingleLevel;
		private System.Windows.Forms.GroupBox grpSSLDisplay;
		private System.Windows.Forms.CheckBox chkSSLSimilarStores;
        private System.Windows.Forms.CheckBox chkSSLIneligibleStores;
		// Begin Track #4872 - JSmith - Global/User Attributes
		private MIDAttributeComboBox cboSSLStoreAttribute;
		// End Track #4872
		private System.Windows.Forms.Label lblSSLFilter;
		private System.Windows.Forms.Label lblSSLView;
		private System.Windows.Forms.Label lblSSLGroupBy;
		private System.Windows.Forms.Label lblSSLStoreAttribute;
		private System.Windows.Forms.GroupBox grpSSLOTSPlan;
        private MIDRetail.Windows.Controls.MIDDateRangeSelector mdsSSLPlanDateRange;
		private System.Windows.Forms.TextBox txtSSLChainNode;
		private System.Windows.Forms.TextBox txtSSLStoreNode;
		private System.Windows.Forms.Label lblSSLChainVersion;
		private System.Windows.Forms.Label lblSSLStoreVersion;
		private System.Windows.Forms.Label lblSSLChainNode;
		private System.Windows.Forms.Label lblSSLTimePeriod;
        private System.Windows.Forms.Label lblSSLStoreNode;
		private MIDComboBoxEnh cboStoreNonMulti;
		private Label lblStoreNonMulti;
		private Label lblStore;
		private MIDComboBoxEnh cboStore;
        private MIDComboBoxEnh cboComputationMode;
        private MIDComboBoxEnh cboCSLView;
        private MIDComboBoxEnh cboCSLChainVersion;
        private MIDAttributeComboBox cboSMLStoreAttribute;
        private MIDComboBoxEnh cboSMLGroupBy;
        private MIDComboBoxEnh cboSMLFilter;
        private MIDComboBoxEnh cboSMLView;
        private MIDComboBoxEnh cboSMLHighLevelVersion;
        private MIDComboBoxEnh cboSMLLowLevelsVersion;
        private MIDComboBoxEnh cboSMLOverride;
        private MIDComboBoxEnh cboSMLLowLevels;
        private MIDComboBoxEnh cboCMLGroupBy;
        private MIDComboBoxEnh cboCMLFilter;
        private MIDComboBoxEnh cboCMLView;
        private MIDComboBoxEnh cboCMLHighLevelVersion;
        private MIDComboBoxEnh cboCMLLowLevelsVersion;
        private MIDComboBoxEnh cboCMLOverride;
        private MIDComboBoxEnh cboCMLLowLevels;
        private MIDComboBoxEnh cboSSLGroupBy;
        private MIDComboBoxEnh cboSSLFilter;
        private MIDComboBoxEnh cboSSLView;
        private MIDComboBoxEnh cboSSLStoreVersion;
        private MIDComboBoxEnh cboSSLChainVersion;
        private CheckBox chkRT;
		private System.ComponentModel.IContainer components;

        //TT#622 - Gets error when attempting to open screen in designer  .ComboBox. removed 03/11/2013
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
            this.GridContextMenu = new System.Windows.Forms.ContextMenu();
            this.mnuDelete = new System.Windows.Forms.MenuItem();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.grpType = new System.Windows.Forms.GroupBox();
            this.chkRT = new System.Windows.Forms.CheckBox();
            this.chkMultiLevel = new System.Windows.Forms.CheckBox();
            this.radChainLadder = new System.Windows.Forms.RadioButton();
            this.radChain = new System.Windows.Forms.RadioButton();
            this.radStore = new System.Windows.Forms.RadioButton();
            this.lblComputationMode = new System.Windows.Forms.Label();
            this.pnlMain = new System.Windows.Forms.Panel();
            this.grdBasis = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.pnlStoreSingleLevel = new System.Windows.Forms.Panel();
            this.cboStoreNonMulti = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this.lblStoreNonMulti = new System.Windows.Forms.Label();
            this.grpSSLDisplay = new System.Windows.Forms.GroupBox();
            this.cboSSLView = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this.cboSSLFilter = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this.cboSSLGroupBy = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this.chkSSLSimilarStores = new System.Windows.Forms.CheckBox();
            this.chkSSLIneligibleStores = new System.Windows.Forms.CheckBox();
            this.cboSSLStoreAttribute = new MIDRetail.Windows.Controls.MIDAttributeComboBox();
            this.lblSSLFilter = new System.Windows.Forms.Label();
            this.lblSSLView = new System.Windows.Forms.Label();
            this.lblSSLGroupBy = new System.Windows.Forms.Label();
            this.lblSSLStoreAttribute = new System.Windows.Forms.Label();
            this.grpSSLOTSPlan = new System.Windows.Forms.GroupBox();
            this.cboSSLChainVersion = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this.cboSSLStoreVersion = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this.mdsSSLPlanDateRange = new MIDRetail.Windows.Controls.MIDDateRangeSelector();
            this.txtSSLChainNode = new System.Windows.Forms.TextBox();
            this.txtSSLStoreNode = new System.Windows.Forms.TextBox();
            this.lblSSLChainVersion = new System.Windows.Forms.Label();
            this.lblSSLStoreVersion = new System.Windows.Forms.Label();
            this.lblSSLChainNode = new System.Windows.Forms.Label();
            this.lblSSLTimePeriod = new System.Windows.Forms.Label();
            this.lblSSLStoreNode = new System.Windows.Forms.Label();
            this.pnlChainMultiLevel = new System.Windows.Forms.Panel();
            this.cboCMLOverride = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this.btnCMLVersionOverride = new System.Windows.Forms.Button();
            this.grpCMLDisplay = new System.Windows.Forms.GroupBox();
            this.cboCMLView = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this.cboCMLFilter = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this.cboCMLGroupBy = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this.lblCMLFilter = new System.Windows.Forms.Label();
            this.lblCMLView = new System.Windows.Forms.Label();
            this.lblCMLGroupBy = new System.Windows.Forms.Label();
            this.grpCMLOTSPlan = new System.Windows.Forms.GroupBox();
            this.cboCMLLowLevels = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this.cboCMLLowLevelsVersion = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this.cboCMLHighLevelVersion = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this.mdsCMLPlanDateRange = new MIDRetail.Windows.Controls.MIDDateRangeSelector();
            this.txtCMLHighLevelNode = new System.Windows.Forms.TextBox();
            this.lblCMLLowLevelsVersion = new System.Windows.Forms.Label();
            this.lblCMLHighLevelVersion = new System.Windows.Forms.Label();
            this.lblCMLLowLevels = new System.Windows.Forms.Label();
            this.lblCMLTimePeriod = new System.Windows.Forms.Label();
            this.lblCMLHighLevel = new System.Windows.Forms.Label();
            this.pnlStoreMultiLevel = new System.Windows.Forms.Panel();
            this.cboSMLOverride = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this.lblStore = new System.Windows.Forms.Label();
            this.cboStore = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this.btnSMLVersionOverride = new System.Windows.Forms.Button();
            this.grpSMLDisplay = new System.Windows.Forms.GroupBox();
            this.cboSMLView = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this.cboSMLFilter = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this.cboSMLGroupBy = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this.cboSMLStoreAttribute = new MIDRetail.Windows.Controls.MIDAttributeComboBox();
            this.chkSMLSimilarStores = new System.Windows.Forms.CheckBox();
            this.chkSMLIneligibleStores = new System.Windows.Forms.CheckBox();
            this.lblSMLFilter = new System.Windows.Forms.Label();
            this.lblSMLView = new System.Windows.Forms.Label();
            this.lblSMLGroupBy = new System.Windows.Forms.Label();
            this.lblSMLStoreAttribute = new System.Windows.Forms.Label();
            this.grpSMLOTSPlan = new System.Windows.Forms.GroupBox();
            this.cboSMLLowLevels = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this.cboSMLLowLevelsVersion = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this.cboSMLHighLevelVersion = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this.mdsSMLPlanDateRange = new MIDRetail.Windows.Controls.MIDDateRangeSelector();
            this.txtSMLHighLevelNode = new System.Windows.Forms.TextBox();
            this.lblSMLLowLevelsVersion = new System.Windows.Forms.Label();
            this.lblSMLHighLevelVersion = new System.Windows.Forms.Label();
            this.lblSMLLowLevels = new System.Windows.Forms.Label();
            this.lblSMLTimePeriod = new System.Windows.Forms.Label();
            this.lblSMLHighLevel = new System.Windows.Forms.Label();
            this.pnlChainSingleLevel = new System.Windows.Forms.Panel();
            this.grpCSLDisplay = new System.Windows.Forms.GroupBox();
            this.cboCSLView = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this.lblCSLView = new System.Windows.Forms.Label();
            this.grpCSLOTSPlan = new System.Windows.Forms.GroupBox();
            this.cboCSLChainVersion = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this.mdsCSLPlanDateRange = new MIDRetail.Windows.Controls.MIDDateRangeSelector();
            this.txtCSLChainNode = new System.Windows.Forms.TextBox();
            this.lblCSLChainVersion = new System.Windows.Forms.Label();
            this.lblCSLTimePeriod = new System.Windows.Forms.Label();
            this.lblCSLChainNode = new System.Windows.Forms.Label();
            this.cboComputationMode = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            ((System.ComponentModel.ISupportInitialize)(this.utmMain)).BeginInit();
            this.grpType.SuspendLayout();
            this.pnlMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdBasis)).BeginInit();
            this.pnlStoreSingleLevel.SuspendLayout();
            this.grpSSLDisplay.SuspendLayout();
            this.grpSSLOTSPlan.SuspendLayout();
            this.pnlChainMultiLevel.SuspendLayout();
            this.grpCMLDisplay.SuspendLayout();
            this.grpCMLOTSPlan.SuspendLayout();
            this.pnlStoreMultiLevel.SuspendLayout();
            this.grpSMLDisplay.SuspendLayout();
            this.grpSMLOTSPlan.SuspendLayout();
            this.pnlChainSingleLevel.SuspendLayout();
            this.grpCSLDisplay.SuspendLayout();
            this.grpCSLOTSPlan.SuspendLayout();
            this.SuspendLayout();
            // 
            // utmMain
            // 
            this.utmMain.MenuSettings.ForceSerialization = true;
            this.utmMain.ToolbarSettings.ForceSerialization = true;
            // 
            // GridContextMenu
            // 
            this.GridContextMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuDelete});
            // 
            // mnuDelete
            // 
            this.mnuDelete.Index = 0;
            this.mnuDelete.Text = "Delete";
            this.mnuDelete.Click += new System.EventHandler(this.mnuDelete_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.CausesValidation = false;
            this.btnCancel.Location = new System.Drawing.Point(616, 560);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(536, 560);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 3;
            this.btnOK.Text = "OK";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // grpType
            // 
            this.grpType.Controls.Add(this.chkRT);
            this.grpType.Controls.Add(this.chkMultiLevel);
            this.grpType.Controls.Add(this.radChainLadder);
            this.grpType.Controls.Add(this.radChain);
            this.grpType.Controls.Add(this.radStore);
            this.grpType.Location = new System.Drawing.Point(8, 8);
            this.grpType.Name = "grpType";
            this.grpType.Size = new System.Drawing.Size(413, 48);
            this.grpType.TabIndex = 6;
            this.grpType.TabStop = false;
            this.grpType.Text = "Type";
            // 
            // chkRT
            // 
            this.chkRT.AutoSize = true;
            this.chkRT.Location = new System.Drawing.Point(315, 20);
            this.chkRT.Name = "chkRT";
            this.chkRT.Size = new System.Drawing.Size(83, 17);
            this.chkRT.TabIndex = 4;
            this.chkRT.Text = "Totals Right";
            this.chkRT.UseVisualStyleBackColor = true;
            // 
            // chkMultiLevel
            // 
            this.chkMultiLevel.Location = new System.Drawing.Point(228, 16);
            this.chkMultiLevel.Name = "chkMultiLevel";
            this.chkMultiLevel.Size = new System.Drawing.Size(80, 24);
            this.chkMultiLevel.TabIndex = 3;
            this.chkMultiLevel.Text = "Multi-Level";
            this.chkMultiLevel.CheckedChanged += new System.EventHandler(this.chkMultiLevel_CheckedChanged);
            // 
            // radChainLadder
            // 
            this.radChainLadder.Location = new System.Drawing.Point(120, 16);
            this.radChainLadder.Name = "radChainLadder";
            this.radChainLadder.Size = new System.Drawing.Size(90, 24);
            this.radChainLadder.TabIndex = 2;
            this.radChainLadder.Text = "Chain-Ladder";
            this.radChainLadder.CheckedChanged += new System.EventHandler(this.radChainLadder_CheckedChanged);
            // 
            // radChain
            // 
            this.radChain.Location = new System.Drawing.Point(68, 16);
            this.radChain.Name = "radChain";
            this.radChain.Size = new System.Drawing.Size(80, 24);
            this.radChain.TabIndex = 1;
            this.radChain.Text = "Chain";
            this.radChain.CheckedChanged += new System.EventHandler(this.radChain_CheckedChanged);
            // 
            // radStore
            // 
            this.radStore.Location = new System.Drawing.Point(16, 16);
            this.radStore.Name = "radStore";
            this.radStore.Size = new System.Drawing.Size(72, 24);
            this.radStore.TabIndex = 0;
            this.radStore.Text = "Store";
            this.radStore.CheckedChanged += new System.EventHandler(this.radStore_CheckedChanged);
            // 
            // lblComputationMode
            // 
            this.lblComputationMode.Location = new System.Drawing.Point(421, 24);
            this.lblComputationMode.Name = "lblComputationMode";
            this.lblComputationMode.Size = new System.Drawing.Size(104, 23);
            this.lblComputationMode.TabIndex = 14;
            this.lblComputationMode.Text = "Computation Mode:";
            this.lblComputationMode.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // pnlMain
            // 
            this.pnlMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlMain.Controls.Add(this.grdBasis);
            this.pnlMain.Controls.Add(this.pnlStoreSingleLevel);
            this.pnlMain.Controls.Add(this.pnlChainMultiLevel);
            this.pnlMain.Controls.Add(this.pnlStoreMultiLevel);
            this.pnlMain.Controls.Add(this.pnlChainSingleLevel);
            this.pnlMain.Location = new System.Drawing.Point(8, 56);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Size = new System.Drawing.Size(688, 488);
            this.pnlMain.TabIndex = 16;
            // 
            // grdBasis
            // 
            this.grdBasis.AllowDrop = true;
            this.grdBasis.ContextMenu = this.GridContextMenu;
            appearance1.BackColor = System.Drawing.Color.White;
            appearance1.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance1.BackGradientStyle = Infragistics.Win.GradientStyle.ForwardDiagonal;
            this.grdBasis.DisplayLayout.Appearance = appearance1;
            this.grdBasis.DisplayLayout.InterBandSpacing = 10;
            appearance2.BackColor = System.Drawing.Color.Transparent;
            this.grdBasis.DisplayLayout.Override.CardAreaAppearance = appearance2;
            appearance3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance3.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance3.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance3.ForeColor = System.Drawing.Color.Black;
            appearance3.TextHAlignAsString = "Left";
            appearance3.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            this.grdBasis.DisplayLayout.Override.HeaderAppearance = appearance3;
            appearance4.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.grdBasis.DisplayLayout.Override.RowAppearance = appearance4;
            appearance5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance5.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance5.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            this.grdBasis.DisplayLayout.Override.RowSelectorAppearance = appearance5;
            this.grdBasis.DisplayLayout.Override.RowSelectorWidth = 12;
            this.grdBasis.DisplayLayout.Override.RowSpacingBefore = 2;
            appearance6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance6.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance6.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance6.ForeColor = System.Drawing.Color.Black;
            this.grdBasis.DisplayLayout.Override.SelectedRowAppearance = appearance6;
            this.grdBasis.DisplayLayout.RowConnectorColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.grdBasis.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.Solid;
            this.grdBasis.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdBasis.Location = new System.Drawing.Point(0, 754);
            this.grdBasis.Name = "grdBasis";
            this.grdBasis.Size = new System.Drawing.Size(688, 0);
            this.grdBasis.TabIndex = 1;
            this.grdBasis.AfterCellUpdate += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdBasis_AfterCellUpdate);
            this.grdBasis.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.grdBasis_InitializeLayout);
            this.grdBasis.InitializeRow += new Infragistics.Win.UltraWinGrid.InitializeRowEventHandler(this.grdBasis_InitializeRow);
            this.grdBasis.AfterRowsDeleted += new System.EventHandler(this.grdBasis_AfterRowsDeleted);
            this.grdBasis.AfterRowInsert += new Infragistics.Win.UltraWinGrid.RowEventHandler(this.grdBasis_AfterRowInsert);
            this.grdBasis.ClickCellButton += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdBasis_ClickCellButton);
            this.grdBasis.AfterSelectChange += new Infragistics.Win.UltraWinGrid.AfterSelectChangeEventHandler(this.grdBasis_AfterSelectChange);
            this.grdBasis.BeforeRowInsert += new Infragistics.Win.UltraWinGrid.BeforeRowInsertEventHandler(this.grdBasis_BeforeRowInsert);
            this.grdBasis.BeforeCellUpdate += new Infragistics.Win.UltraWinGrid.BeforeCellUpdateEventHandler(this.grdBasis_BeforeCellUpdate);
            this.grdBasis.BeforeRowsDeleted += new Infragistics.Win.UltraWinGrid.BeforeRowsDeletedEventHandler(this.grdBasis_BeforeRowsDeleted);
            this.grdBasis.DragDrop += new System.Windows.Forms.DragEventHandler(this.grdBasis_DragDrop);
            this.grdBasis.DragEnter += new System.Windows.Forms.DragEventHandler(this.grdBasis_DragEnter);
            this.grdBasis.DragOver += new System.Windows.Forms.DragEventHandler(this.grdBasis_DragOver);
            this.grdBasis.MouseUp += new System.Windows.Forms.MouseEventHandler(this.grdBasis_MouseUp);
            // 
            // pnlStoreSingleLevel
            // 
            this.pnlStoreSingleLevel.Controls.Add(this.cboStoreNonMulti);
            this.pnlStoreSingleLevel.Controls.Add(this.lblStoreNonMulti);
            this.pnlStoreSingleLevel.Controls.Add(this.grpSSLDisplay);
            this.pnlStoreSingleLevel.Controls.Add(this.grpSSLOTSPlan);
            this.pnlStoreSingleLevel.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlStoreSingleLevel.Location = new System.Drawing.Point(0, 554);
            this.pnlStoreSingleLevel.Name = "pnlStoreSingleLevel";
            this.pnlStoreSingleLevel.Size = new System.Drawing.Size(688, 200);
            this.pnlStoreSingleLevel.TabIndex = 13;
            // 
            // cboStoreNonMulti
            // 
            this.cboStoreNonMulti.AutoAdjust = true;
            this.cboStoreNonMulti.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboStoreNonMulti.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboStoreNonMulti.DataSource = null;
            this.cboStoreNonMulti.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboStoreNonMulti.DropDownWidth = 125;
            this.cboStoreNonMulti.FormattingEnabled = false;
            this.cboStoreNonMulti.IgnoreFocusLost = false;
            this.cboStoreNonMulti.ItemHeight = 13;
            this.cboStoreNonMulti.Location = new System.Drawing.Point(550, 102);
            this.cboStoreNonMulti.Margin = new System.Windows.Forms.Padding(0);
            this.cboStoreNonMulti.MaxDropDownItems = 25;
            this.cboStoreNonMulti.Name = "cboStoreNonMulti";
            this.cboStoreNonMulti.SetToolTip = "";
            this.cboStoreNonMulti.Size = new System.Drawing.Size(125, 22);
            this.cboStoreNonMulti.TabIndex = 4;
            this.cboStoreNonMulti.Tag = null;
            this.cboStoreNonMulti.MIDComboBoxPropertiesChangedEvent += new MIDRetail.Windows.Controls.MIDComboBoxPropertiesChangedEventHandler(this.cboStoreNonMulti_MIDComboBoxPropertiesChangedEvent);
            this.cboStoreNonMulti.SelectionChangeCommitted += new System.EventHandler(this.cboStoreNonMulti_SelectionChangeCommitted);
            // 
            // lblStoreNonMulti
            // 
            this.lblStoreNonMulti.AutoSize = true;
            this.lblStoreNonMulti.Location = new System.Drawing.Point(512, 102);
            this.lblStoreNonMulti.Name = "lblStoreNonMulti";
            this.lblStoreNonMulti.Size = new System.Drawing.Size(35, 13);
            this.lblStoreNonMulti.TabIndex = 3;
            this.lblStoreNonMulti.Text = "Store:";
            // 
            // grpSSLDisplay
            // 
            this.grpSSLDisplay.Controls.Add(this.cboSSLView);
            this.grpSSLDisplay.Controls.Add(this.cboSSLFilter);
            this.grpSSLDisplay.Controls.Add(this.cboSSLGroupBy);
            this.grpSSLDisplay.Controls.Add(this.chkSSLSimilarStores);
            this.grpSSLDisplay.Controls.Add(this.chkSSLIneligibleStores);
            this.grpSSLDisplay.Controls.Add(this.cboSSLStoreAttribute);
            this.grpSSLDisplay.Controls.Add(this.lblSSLFilter);
            this.grpSSLDisplay.Controls.Add(this.lblSSLView);
            this.grpSSLDisplay.Controls.Add(this.lblSSLGroupBy);
            this.grpSSLDisplay.Controls.Add(this.lblSSLStoreAttribute);
            this.grpSSLDisplay.Location = new System.Drawing.Point(0, 8);
            this.grpSSLDisplay.Name = "grpSSLDisplay";
            this.grpSSLDisplay.Size = new System.Drawing.Size(672, 72);
            this.grpSSLDisplay.TabIndex = 1;
            this.grpSSLDisplay.TabStop = false;
            this.grpSSLDisplay.Text = "Display";
            // 
            // cboSSLView
            // 
            this.cboSSLView.AutoAdjust = true;
            this.cboSSLView.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboSSLView.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboSSLView.DataSource = null;
            this.cboSSLView.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSSLView.DropDownWidth = 200;
            this.cboSSLView.FormattingEnabled = false;
            this.cboSSLView.IgnoreFocusLost = false;
            this.cboSSLView.ItemHeight = 13;
            this.cboSSLView.Location = new System.Drawing.Point(328, 40);
            this.cboSSLView.Margin = new System.Windows.Forms.Padding(0);
            this.cboSSLView.MaxDropDownItems = 25;
            this.cboSSLView.Name = "cboSSLView";
            this.cboSSLView.SetToolTip = "";
            this.cboSSLView.Size = new System.Drawing.Size(200, 21);
            this.cboSSLView.TabIndex = 7;
            this.cboSSLView.Tag = null;
            this.cboSSLView.MIDComboBoxPropertiesChangedEvent += new MIDRetail.Windows.Controls.MIDComboBoxPropertiesChangedEventHandler(this.cboSSLView_MIDComboBoxPropertiesChangedEvent);
            this.cboSSLView.SelectionChangeCommitted += new System.EventHandler(this.cboView_SelectionChangeCommitted);
            // 
            // cboSSLFilter
            // 
            this.cboSSLFilter.AllowDrop = true;
            this.cboSSLFilter.AutoAdjust = true;
            this.cboSSLFilter.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboSSLFilter.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboSSLFilter.DataSource = null;
            this.cboSSLFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSSLFilter.DropDownWidth = 200;
            this.cboSSLFilter.FormattingEnabled = false;
            this.cboSSLFilter.IgnoreFocusLost = false;
            this.cboSSLFilter.ItemHeight = 13;
            this.cboSSLFilter.Location = new System.Drawing.Point(328, 16);
            this.cboSSLFilter.Margin = new System.Windows.Forms.Padding(0);
            this.cboSSLFilter.MaxDropDownItems = 25;
            this.cboSSLFilter.Name = "cboSSLFilter";
            this.cboSSLFilter.SetToolTip = "";
            this.cboSSLFilter.Size = new System.Drawing.Size(200, 21);
            this.cboSSLFilter.TabIndex = 5;
            this.cboSSLFilter.Tag = null;
            this.cboSSLFilter.MIDComboBoxPropertiesChangedEvent += new MIDRetail.Windows.Controls.MIDComboBoxPropertiesChangedEventHandler(this.cboSSLFilter_MIDComboBoxPropertiesChangedEvent);
            this.cboSSLFilter.SelectionChangeCommitted += new System.EventHandler(this.cboSSLFilter_SelectionChangeCommitted);
            this.cboSSLFilter.DragDrop += new System.Windows.Forms.DragEventHandler(this.cboFilter_DragDrop);
            this.cboSSLFilter.DragEnter += new System.Windows.Forms.DragEventHandler(this.cboFilter_DragEnter);
            this.cboSSLFilter.DragOver += new System.Windows.Forms.DragEventHandler(this.cboFilter_DragOver);
            this.cboSSLFilter.DropDownClosed += new System.EventHandler(this.cboFilter_DropDown);
            // 
            // cboSSLGroupBy
            // 
            this.cboSSLGroupBy.AutoAdjust = true;
            this.cboSSLGroupBy.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboSSLGroupBy.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboSSLGroupBy.DataSource = null;
            this.cboSSLGroupBy.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSSLGroupBy.DropDownWidth = 192;
            this.cboSSLGroupBy.FormattingEnabled = false;
            this.cboSSLGroupBy.IgnoreFocusLost = false;
            this.cboSSLGroupBy.ItemHeight = 13;
            this.cboSSLGroupBy.Location = new System.Drawing.Point(80, 40);
            this.cboSSLGroupBy.Margin = new System.Windows.Forms.Padding(0);
            this.cboSSLGroupBy.MaxDropDownItems = 25;
            this.cboSSLGroupBy.Name = "cboSSLGroupBy";
            this.cboSSLGroupBy.SetToolTip = "";
            this.cboSSLGroupBy.Size = new System.Drawing.Size(192, 21);
            this.cboSSLGroupBy.TabIndex = 3;
            this.cboSSLGroupBy.Tag = null;
            this.cboSSLGroupBy.MIDComboBoxPropertiesChangedEvent += new MIDRetail.Windows.Controls.MIDComboBoxPropertiesChangedEventHandler(this.cboSSLGroupBy_MIDComboBoxPropertiesChangedEvent);
            this.cboSSLGroupBy.SelectionChangeCommitted += new System.EventHandler(this.cboGroupBy_SelectionChangeCommitted);
            // 
            // chkSSLSimilarStores
            // 
            this.chkSSLSimilarStores.Location = new System.Drawing.Point(552, 42);
            this.chkSSLSimilarStores.Name = "chkSSLSimilarStores";
            this.chkSSLSimilarStores.Size = new System.Drawing.Size(104, 16);
            this.chkSSLSimilarStores.TabIndex = 13;
            this.chkSSLSimilarStores.Text = "Similar Stores";
            this.chkSSLSimilarStores.CheckedChanged += new System.EventHandler(this.chkSimilarStores_CheckedChanged);
            // 
            // chkSSLIneligibleStores
            // 
            this.chkSSLIneligibleStores.Location = new System.Drawing.Point(552, 18);
            this.chkSSLIneligibleStores.Name = "chkSSLIneligibleStores";
            this.chkSSLIneligibleStores.Size = new System.Drawing.Size(104, 16);
            this.chkSSLIneligibleStores.TabIndex = 12;
            this.chkSSLIneligibleStores.Text = "Ineligible Stores";
            this.chkSSLIneligibleStores.CheckedChanged += new System.EventHandler(this.chkIneligibleStores_CheckedChanged);
            // 
            // cboSSLStoreAttribute
            // 
            this.cboSSLStoreAttribute.AllowDrop = true;
            this.cboSSLStoreAttribute.AllowUserAttributes = false;
            this.cboSSLStoreAttribute.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboSSLStoreAttribute.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboSSLStoreAttribute.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSSLStoreAttribute.Location = new System.Drawing.Point(80, 16);
            this.cboSSLStoreAttribute.Name = "cboSSLStoreAttribute";
            this.cboSSLStoreAttribute.Size = new System.Drawing.Size(192, 21);
            this.cboSSLStoreAttribute.TabIndex = 1;
            this.cboSSLStoreAttribute.MIDComboBoxPropertiesChangedEvent += new MIDRetail.Windows.Controls.MIDComboBoxPropertiesChangedEventHandler(this.cboSSLStoreAttribute_MIDComboBoxPropertiesChangedEvent);
            this.cboSSLStoreAttribute.SelectionChangeCommitted += new System.EventHandler(this.cboStoreAttribute_SelectionChangeCommitted);
            this.cboSSLStoreAttribute.DragEnter += new System.Windows.Forms.DragEventHandler(this.cboStoreAttribute_DragEnter);
            this.cboSSLStoreAttribute.DragOver += new System.Windows.Forms.DragEventHandler(this.cboStoreAttribute_DragOver);
            // 
            // lblSSLFilter
            // 
            this.lblSSLFilter.Location = new System.Drawing.Point(280, 24);
            this.lblSSLFilter.Name = "lblSSLFilter";
            this.lblSSLFilter.Size = new System.Drawing.Size(40, 16);
            this.lblSSLFilter.TabIndex = 4;
            this.lblSSLFilter.Text = "Filter:";
            // 
            // lblSSLView
            // 
            this.lblSSLView.Location = new System.Drawing.Point(280, 46);
            this.lblSSLView.Name = "lblSSLView";
            this.lblSSLView.Size = new System.Drawing.Size(40, 16);
            this.lblSSLView.TabIndex = 6;
            this.lblSSLView.Text = "View:";
            // 
            // lblSSLGroupBy
            // 
            this.lblSSLGroupBy.Location = new System.Drawing.Point(8, 48);
            this.lblSSLGroupBy.Name = "lblSSLGroupBy";
            this.lblSSLGroupBy.Size = new System.Drawing.Size(80, 16);
            this.lblSSLGroupBy.TabIndex = 2;
            this.lblSSLGroupBy.Text = "Group By:";
            // 
            // lblSSLStoreAttribute
            // 
            this.lblSSLStoreAttribute.Location = new System.Drawing.Point(8, 24);
            this.lblSSLStoreAttribute.Name = "lblSSLStoreAttribute";
            this.lblSSLStoreAttribute.Size = new System.Drawing.Size(80, 16);
            this.lblSSLStoreAttribute.TabIndex = 0;
            this.lblSSLStoreAttribute.Text = "Store Attibute:";
            // 
            // grpSSLOTSPlan
            // 
            this.grpSSLOTSPlan.Controls.Add(this.cboSSLChainVersion);
            this.grpSSLOTSPlan.Controls.Add(this.cboSSLStoreVersion);
            this.grpSSLOTSPlan.Controls.Add(this.mdsSSLPlanDateRange);
            this.grpSSLOTSPlan.Controls.Add(this.txtSSLChainNode);
            this.grpSSLOTSPlan.Controls.Add(this.txtSSLStoreNode);
            this.grpSSLOTSPlan.Controls.Add(this.lblSSLChainVersion);
            this.grpSSLOTSPlan.Controls.Add(this.lblSSLStoreVersion);
            this.grpSSLOTSPlan.Controls.Add(this.lblSSLChainNode);
            this.grpSSLOTSPlan.Controls.Add(this.lblSSLTimePeriod);
            this.grpSSLOTSPlan.Controls.Add(this.lblSSLStoreNode);
            this.grpSSLOTSPlan.Location = new System.Drawing.Point(0, 88);
            this.grpSSLOTSPlan.Name = "grpSSLOTSPlan";
            this.grpSSLOTSPlan.Size = new System.Drawing.Size(504, 96);
            this.grpSSLOTSPlan.TabIndex = 2;
            this.grpSSLOTSPlan.TabStop = false;
            this.grpSSLOTSPlan.Text = "OTS Plan";
            // 
            // cboSSLChainVersion
            // 
            this.cboSSLChainVersion.AutoAdjust = true;
            this.cboSSLChainVersion.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboSSLChainVersion.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboSSLChainVersion.DataSource = null;
            this.cboSSLChainVersion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSSLChainVersion.DropDownWidth = 152;
            this.cboSSLChainVersion.FormattingEnabled = false;
            this.cboSSLChainVersion.IgnoreFocusLost = false;
            this.cboSSLChainVersion.ItemHeight = 13;
            this.cboSSLChainVersion.Location = new System.Drawing.Point(336, 64);
            this.cboSSLChainVersion.Margin = new System.Windows.Forms.Padding(0);
            this.cboSSLChainVersion.MaxDropDownItems = 25;
            this.cboSSLChainVersion.Name = "cboSSLChainVersion";
            this.cboSSLChainVersion.SetToolTip = "";
            this.cboSSLChainVersion.Size = new System.Drawing.Size(152, 21);
            this.cboSSLChainVersion.TabIndex = 9;
            this.cboSSLChainVersion.Tag = null;
            this.cboSSLChainVersion.MIDComboBoxPropertiesChangedEvent += new MIDRetail.Windows.Controls.MIDComboBoxPropertiesChangedEventHandler(this.cboSSLChainVersion_MIDComboBoxPropertiesChangedEvent);
            this.cboSSLChainVersion.SelectionChangeCommitted += new System.EventHandler(this.cboVersion_SelectionChangeCommitted);
            // 
            // cboSSLStoreVersion
            // 
            this.cboSSLStoreVersion.AutoAdjust = true;
            this.cboSSLStoreVersion.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboSSLStoreVersion.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboSSLStoreVersion.DataSource = null;
            this.cboSSLStoreVersion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSSLStoreVersion.DropDownWidth = 152;
            this.cboSSLStoreVersion.FormattingEnabled = false;
            this.cboSSLStoreVersion.IgnoreFocusLost = false;
            this.cboSSLStoreVersion.ItemHeight = 13;
            this.cboSSLStoreVersion.Location = new System.Drawing.Point(336, 16);
            this.cboSSLStoreVersion.Margin = new System.Windows.Forms.Padding(0);
            this.cboSSLStoreVersion.MaxDropDownItems = 25;
            this.cboSSLStoreVersion.Name = "cboSSLStoreVersion";
            this.cboSSLStoreVersion.SetToolTip = "";
            this.cboSSLStoreVersion.Size = new System.Drawing.Size(152, 21);
            this.cboSSLStoreVersion.TabIndex = 7;
            this.cboSSLStoreVersion.Tag = null;
            this.cboSSLStoreVersion.MIDComboBoxPropertiesChangedEvent += new MIDRetail.Windows.Controls.MIDComboBoxPropertiesChangedEventHandler(this.cboSSLStoreVersion_MIDComboBoxPropertiesChangedEvent);
            this.cboSSLStoreVersion.SelectionChangeCommitted += new System.EventHandler(this.cboVersion_SelectionChangeCommitted);
            // 
            // mdsSSLPlanDateRange
            // 
            this.mdsSSLPlanDateRange.DateRangeForm = null;
            this.mdsSSLPlanDateRange.DateRangeRID = 0;
            this.mdsSSLPlanDateRange.Enabled = false;
            this.mdsSSLPlanDateRange.Location = new System.Drawing.Point(80, 40);
            this.mdsSSLPlanDateRange.Name = "mdsSSLPlanDateRange";
            this.mdsSSLPlanDateRange.Size = new System.Drawing.Size(192, 24);
            this.mdsSSLPlanDateRange.TabIndex = 3;
            this.mdsSSLPlanDateRange.OnSelection += new MIDRetail.Windows.Controls.MIDDateRangeSelector.SelectionEventHandler(this.mdsPlanDateRange_OnSelection);
            this.mdsSSLPlanDateRange.Click += new System.EventHandler(this.mdsSSLPlanDateRange_Click);
            // 
            // txtSSLChainNode
            // 
            this.txtSSLChainNode.AllowDrop = true;
            this.txtSSLChainNode.Location = new System.Drawing.Point(80, 64);
            this.txtSSLChainNode.Name = "txtSSLChainNode";
            this.txtSSLChainNode.Size = new System.Drawing.Size(192, 20);
            this.txtSSLChainNode.TabIndex = 5;
            this.txtSSLChainNode.TextChanged += new System.EventHandler(this.txtNode_TextChanged);
            this.txtSSLChainNode.DragDrop += new System.Windows.Forms.DragEventHandler(this.txtNode_DragDrop);
            this.txtSSLChainNode.DragEnter += new System.Windows.Forms.DragEventHandler(this.txtNode_DragEnter);
            this.txtSSLChainNode.DragOver += new System.Windows.Forms.DragEventHandler(this.txtNode_DragOver);
            this.txtSSLChainNode.Validating += new System.ComponentModel.CancelEventHandler(this.txtNode_Validating);
            this.txtSSLChainNode.Validated += new System.EventHandler(this.txtNode_Validated);
            // 
            // txtSSLStoreNode
            // 
            this.txtSSLStoreNode.AllowDrop = true;
            this.txtSSLStoreNode.Location = new System.Drawing.Point(80, 17);
            this.txtSSLStoreNode.Name = "txtSSLStoreNode";
            this.txtSSLStoreNode.Size = new System.Drawing.Size(192, 20);
            this.txtSSLStoreNode.TabIndex = 1;
            this.txtSSLStoreNode.TextChanged += new System.EventHandler(this.txtSSLStoreNode_TextChanged);
            this.txtSSLStoreNode.DragDrop += new System.Windows.Forms.DragEventHandler(this.txtNode_DragDrop);
            this.txtSSLStoreNode.DragEnter += new System.Windows.Forms.DragEventHandler(this.txtNode_DragEnter);
            this.txtSSLStoreNode.DragOver += new System.Windows.Forms.DragEventHandler(this.txtNode_DragOver);
            this.txtSSLStoreNode.Validating += new System.ComponentModel.CancelEventHandler(this.txtNode_Validating);
            this.txtSSLStoreNode.Validated += new System.EventHandler(this.txtNode_Validated);
            // 
            // lblSSLChainVersion
            // 
            this.lblSSLChainVersion.Location = new System.Drawing.Point(280, 68);
            this.lblSSLChainVersion.Name = "lblSSLChainVersion";
            this.lblSSLChainVersion.Size = new System.Drawing.Size(48, 16);
            this.lblSSLChainVersion.TabIndex = 8;
            this.lblSSLChainVersion.Text = "Version:";
            // 
            // lblSSLStoreVersion
            // 
            this.lblSSLStoreVersion.Location = new System.Drawing.Point(280, 22);
            this.lblSSLStoreVersion.Name = "lblSSLStoreVersion";
            this.lblSSLStoreVersion.Size = new System.Drawing.Size(48, 16);
            this.lblSSLStoreVersion.TabIndex = 6;
            this.lblSSLStoreVersion.Text = "Version:";
            // 
            // lblSSLChainNode
            // 
            this.lblSSLChainNode.Location = new System.Drawing.Point(8, 68);
            this.lblSSLChainNode.Name = "lblSSLChainNode";
            this.lblSSLChainNode.Size = new System.Drawing.Size(72, 16);
            this.lblSSLChainNode.TabIndex = 4;
            this.lblSSLChainNode.Text = "Chain:";
            // 
            // lblSSLTimePeriod
            // 
            this.lblSSLTimePeriod.Location = new System.Drawing.Point(8, 46);
            this.lblSSLTimePeriod.Name = "lblSSLTimePeriod";
            this.lblSSLTimePeriod.Size = new System.Drawing.Size(72, 16);
            this.lblSSLTimePeriod.TabIndex = 2;
            this.lblSSLTimePeriod.Text = "Time Period:";
            // 
            // lblSSLStoreNode
            // 
            this.lblSSLStoreNode.Location = new System.Drawing.Point(8, 24);
            this.lblSSLStoreNode.Name = "lblSSLStoreNode";
            this.lblSSLStoreNode.Size = new System.Drawing.Size(72, 16);
            this.lblSSLStoreNode.TabIndex = 0;
            this.lblSSLStoreNode.Text = "Store:";
            // 
            // pnlChainMultiLevel
            // 
            this.pnlChainMultiLevel.Controls.Add(this.cboCMLOverride);
            this.pnlChainMultiLevel.Controls.Add(this.btnCMLVersionOverride);
            this.pnlChainMultiLevel.Controls.Add(this.grpCMLDisplay);
            this.pnlChainMultiLevel.Controls.Add(this.grpCMLOTSPlan);
            this.pnlChainMultiLevel.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlChainMultiLevel.Location = new System.Drawing.Point(0, 352);
            this.pnlChainMultiLevel.Name = "pnlChainMultiLevel";
            this.pnlChainMultiLevel.Size = new System.Drawing.Size(688, 202);
            this.pnlChainMultiLevel.TabIndex = 12;
            // 
            // cboCMLOverride
            // 
            this.cboCMLOverride.AutoAdjust = true;
            this.cboCMLOverride.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboCMLOverride.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboCMLOverride.DataSource = null;
            this.cboCMLOverride.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboCMLOverride.DropDownWidth = 160;
            this.cboCMLOverride.FormattingEnabled = false;
            this.cboCMLOverride.IgnoreFocusLost = false;
            this.cboCMLOverride.ItemHeight = 13;
            this.cboCMLOverride.Location = new System.Drawing.Point(512, 157);
            this.cboCMLOverride.Margin = new System.Windows.Forms.Padding(0);
            this.cboCMLOverride.MaxDropDownItems = 25;
            this.cboCMLOverride.Name = "cboCMLOverride";
            this.cboCMLOverride.SetToolTip = "";
            this.cboCMLOverride.Size = new System.Drawing.Size(160, 21);
            this.cboCMLOverride.TabIndex = 5;
            this.cboCMLOverride.Tag = null;
            this.cboCMLOverride.MIDComboBoxPropertiesChangedEvent += new MIDRetail.Windows.Controls.MIDComboBoxPropertiesChangedEventHandler(this.cboCMLOverride_MIDComboBoxPropertiesChangedEvent);
            this.cboCMLOverride.SelectionChangeCommitted += new System.EventHandler(this.cboCMLOverride_SelectionChangeCommitted);
            // 
            // btnCMLVersionOverride
            // 
            this.btnCMLVersionOverride.Location = new System.Drawing.Point(512, 131);
            this.btnCMLVersionOverride.Name = "btnCMLVersionOverride";
            this.btnCMLVersionOverride.Size = new System.Drawing.Size(160, 23);
            this.btnCMLVersionOverride.TabIndex = 3;
            this.btnCMLVersionOverride.Text = "Override Low Level Versions";
            this.btnCMLVersionOverride.Click += new System.EventHandler(this.btnCMLVersionOverride_Click);
            // 
            // grpCMLDisplay
            // 
            this.grpCMLDisplay.Controls.Add(this.cboCMLView);
            this.grpCMLDisplay.Controls.Add(this.cboCMLFilter);
            this.grpCMLDisplay.Controls.Add(this.cboCMLGroupBy);
            this.grpCMLDisplay.Controls.Add(this.lblCMLFilter);
            this.grpCMLDisplay.Controls.Add(this.lblCMLView);
            this.grpCMLDisplay.Controls.Add(this.lblCMLGroupBy);
            this.grpCMLDisplay.Location = new System.Drawing.Point(0, 8);
            this.grpCMLDisplay.Name = "grpCMLDisplay";
            this.grpCMLDisplay.Size = new System.Drawing.Size(544, 72);
            this.grpCMLDisplay.TabIndex = 1;
            this.grpCMLDisplay.TabStop = false;
            this.grpCMLDisplay.Text = "Display";
            // 
            // cboCMLView
            // 
            this.cboCMLView.AutoAdjust = true;
            this.cboCMLView.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboCMLView.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboCMLView.DataSource = null;
            this.cboCMLView.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboCMLView.DropDownWidth = 200;
            this.cboCMLView.FormattingEnabled = false;
            this.cboCMLView.IgnoreFocusLost = false;
            this.cboCMLView.ItemHeight = 13;
            this.cboCMLView.Location = new System.Drawing.Point(328, 40);
            this.cboCMLView.Margin = new System.Windows.Forms.Padding(0);
            this.cboCMLView.MaxDropDownItems = 25;
            this.cboCMLView.Name = "cboCMLView";
            this.cboCMLView.SetToolTip = "";
            this.cboCMLView.Size = new System.Drawing.Size(200, 21);
            this.cboCMLView.TabIndex = 7;
            this.cboCMLView.Tag = null;
            this.cboCMLView.MIDComboBoxPropertiesChangedEvent += new MIDRetail.Windows.Controls.MIDComboBoxPropertiesChangedEventHandler(this.cboCMLView_MIDComboBoxPropertiesChangedEvent);
            this.cboCMLView.SelectionChangeCommitted += new System.EventHandler(this.cboView_SelectionChangeCommitted);
            // 
            // cboCMLFilter
            // 
            this.cboCMLFilter.AllowDrop = true;
            this.cboCMLFilter.AutoAdjust = true;
            this.cboCMLFilter.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboCMLFilter.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboCMLFilter.DataSource = null;
            this.cboCMLFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboCMLFilter.DropDownWidth = 200;
            this.cboCMLFilter.FormattingEnabled = false;
            this.cboCMLFilter.IgnoreFocusLost = false;
            this.cboCMLFilter.ItemHeight = 13;
            this.cboCMLFilter.Location = new System.Drawing.Point(328, 16);
            this.cboCMLFilter.Margin = new System.Windows.Forms.Padding(0);
            this.cboCMLFilter.MaxDropDownItems = 25;
            this.cboCMLFilter.Name = "cboCMLFilter";
            this.cboCMLFilter.SetToolTip = "";
            this.cboCMLFilter.Size = new System.Drawing.Size(200, 21);
            this.cboCMLFilter.TabIndex = 5;
            this.cboCMLFilter.Tag = null;
            this.cboCMLFilter.MIDComboBoxPropertiesChangedEvent += new MIDRetail.Windows.Controls.MIDComboBoxPropertiesChangedEventHandler(this.cboCMLFilter_MIDComboBoxPropertiesChangedEvent);
            this.cboCMLFilter.SelectionChangeCommitted += new System.EventHandler(this.cboFilter_SelectionChangeCommitted);
            this.cboCMLFilter.DragDrop += new System.Windows.Forms.DragEventHandler(this.cboFilter_DragDrop);
            this.cboCMLFilter.DragEnter += new System.Windows.Forms.DragEventHandler(this.cboFilter_DragEnter);
            this.cboCMLFilter.DragOver += new System.Windows.Forms.DragEventHandler(this.cboFilter_DragOver);
            this.cboCMLFilter.DropDown += new System.EventHandler(this.cboFilter_DropDown);
            // 
            // cboCMLGroupBy
            // 
            this.cboCMLGroupBy.AutoAdjust = true;
            this.cboCMLGroupBy.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboCMLGroupBy.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboCMLGroupBy.DataSource = null;
            this.cboCMLGroupBy.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboCMLGroupBy.DropDownWidth = 192;
            this.cboCMLGroupBy.FormattingEnabled = false;
            this.cboCMLGroupBy.IgnoreFocusLost = false;
            this.cboCMLGroupBy.ItemHeight = 13;
            this.cboCMLGroupBy.Location = new System.Drawing.Point(80, 16);
            this.cboCMLGroupBy.Margin = new System.Windows.Forms.Padding(0);
            this.cboCMLGroupBy.MaxDropDownItems = 25;
            this.cboCMLGroupBy.Name = "cboCMLGroupBy";
            this.cboCMLGroupBy.SetToolTip = "";
            this.cboCMLGroupBy.Size = new System.Drawing.Size(192, 21);
            this.cboCMLGroupBy.TabIndex = 3;
            this.cboCMLGroupBy.Tag = null;
            this.cboCMLGroupBy.MIDComboBoxPropertiesChangedEvent += new MIDRetail.Windows.Controls.MIDComboBoxPropertiesChangedEventHandler(this.cboCMLGroupBy_MIDComboBoxPropertiesChangedEvent);
            this.cboCMLGroupBy.SelectionChangeCommitted += new System.EventHandler(this.cboGroupBy_SelectionChangeCommitted);
            // 
            // lblCMLFilter
            // 
            this.lblCMLFilter.Location = new System.Drawing.Point(280, 22);
            this.lblCMLFilter.Name = "lblCMLFilter";
            this.lblCMLFilter.Size = new System.Drawing.Size(40, 16);
            this.lblCMLFilter.TabIndex = 4;
            this.lblCMLFilter.Text = "Filter:";
            // 
            // lblCMLView
            // 
            this.lblCMLView.Location = new System.Drawing.Point(280, 46);
            this.lblCMLView.Name = "lblCMLView";
            this.lblCMLView.Size = new System.Drawing.Size(40, 16);
            this.lblCMLView.TabIndex = 6;
            this.lblCMLView.Text = "View:";
            // 
            // lblCMLGroupBy
            // 
            this.lblCMLGroupBy.Location = new System.Drawing.Point(8, 24);
            this.lblCMLGroupBy.Name = "lblCMLGroupBy";
            this.lblCMLGroupBy.Size = new System.Drawing.Size(80, 16);
            this.lblCMLGroupBy.TabIndex = 2;
            this.lblCMLGroupBy.Text = "Group By:";
            // 
            // grpCMLOTSPlan
            // 
            this.grpCMLOTSPlan.Controls.Add(this.cboCMLLowLevels);
            this.grpCMLOTSPlan.Controls.Add(this.cboCMLLowLevelsVersion);
            this.grpCMLOTSPlan.Controls.Add(this.cboCMLHighLevelVersion);
            this.grpCMLOTSPlan.Controls.Add(this.mdsCMLPlanDateRange);
            this.grpCMLOTSPlan.Controls.Add(this.txtCMLHighLevelNode);
            this.grpCMLOTSPlan.Controls.Add(this.lblCMLLowLevelsVersion);
            this.grpCMLOTSPlan.Controls.Add(this.lblCMLHighLevelVersion);
            this.grpCMLOTSPlan.Controls.Add(this.lblCMLLowLevels);
            this.grpCMLOTSPlan.Controls.Add(this.lblCMLTimePeriod);
            this.grpCMLOTSPlan.Controls.Add(this.lblCMLHighLevel);
            this.grpCMLOTSPlan.Location = new System.Drawing.Point(0, 88);
            this.grpCMLOTSPlan.Name = "grpCMLOTSPlan";
            this.grpCMLOTSPlan.Size = new System.Drawing.Size(504, 96);
            this.grpCMLOTSPlan.TabIndex = 2;
            this.grpCMLOTSPlan.TabStop = false;
            this.grpCMLOTSPlan.Text = "OTS Plan";
            // 
            // cboCMLLowLevels
            // 
            this.cboCMLLowLevels.AutoAdjust = true;
            this.cboCMLLowLevels.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboCMLLowLevels.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboCMLLowLevels.DataSource = null;
            this.cboCMLLowLevels.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboCMLLowLevels.DropDownWidth = 192;
            this.cboCMLLowLevels.FormattingEnabled = false;
            this.cboCMLLowLevels.IgnoreFocusLost = false;
            this.cboCMLLowLevels.ItemHeight = 13;
            this.cboCMLLowLevels.Location = new System.Drawing.Point(80, 64);
            this.cboCMLLowLevels.Margin = new System.Windows.Forms.Padding(0);
            this.cboCMLLowLevels.MaxDropDownItems = 25;
            this.cboCMLLowLevels.Name = "cboCMLLowLevels";
            this.cboCMLLowLevels.SetToolTip = "";
            this.cboCMLLowLevels.Size = new System.Drawing.Size(192, 21);
            this.cboCMLLowLevels.TabIndex = 10;
            this.cboCMLLowLevels.Tag = null;
            this.cboCMLLowLevels.MIDComboBoxPropertiesChangedEvent += new MIDRetail.Windows.Controls.MIDComboBoxPropertiesChangedEventHandler(this.cboCMLLowLevels_MIDComboBoxPropertiesChangedEvent);
            this.cboCMLLowLevels.SelectionChangeCommitted += new System.EventHandler(this.cboCMLLowLevels_SelectionChangeCommitted);
            // 
            // cboCMLLowLevelsVersion
            // 
            this.cboCMLLowLevelsVersion.AutoAdjust = true;
            this.cboCMLLowLevelsVersion.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboCMLLowLevelsVersion.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboCMLLowLevelsVersion.DataSource = null;
            this.cboCMLLowLevelsVersion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboCMLLowLevelsVersion.DropDownWidth = 152;
            this.cboCMLLowLevelsVersion.FormattingEnabled = false;
            this.cboCMLLowLevelsVersion.IgnoreFocusLost = false;
            this.cboCMLLowLevelsVersion.ItemHeight = 13;
            this.cboCMLLowLevelsVersion.Location = new System.Drawing.Point(336, 64);
            this.cboCMLLowLevelsVersion.Margin = new System.Windows.Forms.Padding(0);
            this.cboCMLLowLevelsVersion.MaxDropDownItems = 25;
            this.cboCMLLowLevelsVersion.Name = "cboCMLLowLevelsVersion";
            this.cboCMLLowLevelsVersion.SetToolTip = "";
            this.cboCMLLowLevelsVersion.Size = new System.Drawing.Size(152, 21);
            this.cboCMLLowLevelsVersion.TabIndex = 9;
            this.cboCMLLowLevelsVersion.Tag = null;
            this.cboCMLLowLevelsVersion.MIDComboBoxPropertiesChangedEvent += new MIDRetail.Windows.Controls.MIDComboBoxPropertiesChangedEventHandler(this.cboCMLLowLevelsVersion_MIDComboBoxPropertiesChangedEvent);
            this.cboCMLLowLevelsVersion.SelectionChangeCommitted += new System.EventHandler(this.cboVersion_SelectionChangeCommitted);
            // 
            // cboCMLHighLevelVersion
            // 
            this.cboCMLHighLevelVersion.AutoAdjust = true;
            this.cboCMLHighLevelVersion.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboCMLHighLevelVersion.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboCMLHighLevelVersion.DataSource = null;
            this.cboCMLHighLevelVersion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboCMLHighLevelVersion.DropDownWidth = 152;
            this.cboCMLHighLevelVersion.FormattingEnabled = false;
            this.cboCMLHighLevelVersion.IgnoreFocusLost = false;
            this.cboCMLHighLevelVersion.ItemHeight = 13;
            this.cboCMLHighLevelVersion.Location = new System.Drawing.Point(336, 16);
            this.cboCMLHighLevelVersion.Margin = new System.Windows.Forms.Padding(0);
            this.cboCMLHighLevelVersion.MaxDropDownItems = 25;
            this.cboCMLHighLevelVersion.Name = "cboCMLHighLevelVersion";
            this.cboCMLHighLevelVersion.SetToolTip = "";
            this.cboCMLHighLevelVersion.Size = new System.Drawing.Size(152, 21);
            this.cboCMLHighLevelVersion.TabIndex = 7;
            this.cboCMLHighLevelVersion.Tag = null;
            this.cboCMLHighLevelVersion.MIDComboBoxPropertiesChangedEvent += new MIDRetail.Windows.Controls.MIDComboBoxPropertiesChangedEventHandler(this.cboCMLHighLevelVersion_MIDComboBoxPropertiesChangedEvent);
            this.cboCMLHighLevelVersion.SelectionChangeCommitted += new System.EventHandler(this.cboVersion_SelectionChangeCommitted);
            // 
            // mdsCMLPlanDateRange
            // 
            this.mdsCMLPlanDateRange.DateRangeForm = null;
            this.mdsCMLPlanDateRange.DateRangeRID = 0;
            this.mdsCMLPlanDateRange.Enabled = false;
            this.mdsCMLPlanDateRange.Location = new System.Drawing.Point(80, 40);
            this.mdsCMLPlanDateRange.Name = "mdsCMLPlanDateRange";
            this.mdsCMLPlanDateRange.Size = new System.Drawing.Size(192, 24);
            this.mdsCMLPlanDateRange.TabIndex = 3;
            this.mdsCMLPlanDateRange.OnSelection += new MIDRetail.Windows.Controls.MIDDateRangeSelector.SelectionEventHandler(this.mdsPlanDateRange_OnSelection);
            this.mdsCMLPlanDateRange.Click += new System.EventHandler(this.mdsCMLPlanDateRange_Click);
            // 
            // txtCMLHighLevelNode
            // 
            this.txtCMLHighLevelNode.AllowDrop = true;
            this.txtCMLHighLevelNode.Location = new System.Drawing.Point(80, 16);
            this.txtCMLHighLevelNode.Name = "txtCMLHighLevelNode";
            this.txtCMLHighLevelNode.Size = new System.Drawing.Size(192, 20);
            this.txtCMLHighLevelNode.TabIndex = 1;
            this.txtCMLHighLevelNode.TextChanged += new System.EventHandler(this.txtNode_TextChanged);
            this.txtCMLHighLevelNode.DragDrop += new System.Windows.Forms.DragEventHandler(this.txtNode_DragDrop);
            this.txtCMLHighLevelNode.DragEnter += new System.Windows.Forms.DragEventHandler(this.txtNode_DragEnter);
            this.txtCMLHighLevelNode.DragOver += new System.Windows.Forms.DragEventHandler(this.txtNode_DragOver);
            this.txtCMLHighLevelNode.Validating += new System.ComponentModel.CancelEventHandler(this.txtNode_Validating);
            this.txtCMLHighLevelNode.Validated += new System.EventHandler(this.txtNode_Validated);
            // 
            // lblCMLLowLevelsVersion
            // 
            this.lblCMLLowLevelsVersion.Location = new System.Drawing.Point(280, 68);
            this.lblCMLLowLevelsVersion.Name = "lblCMLLowLevelsVersion";
            this.lblCMLLowLevelsVersion.Size = new System.Drawing.Size(48, 16);
            this.lblCMLLowLevelsVersion.TabIndex = 8;
            this.lblCMLLowLevelsVersion.Text = "Version:";
            // 
            // lblCMLHighLevelVersion
            // 
            this.lblCMLHighLevelVersion.Location = new System.Drawing.Point(280, 22);
            this.lblCMLHighLevelVersion.Name = "lblCMLHighLevelVersion";
            this.lblCMLHighLevelVersion.Size = new System.Drawing.Size(48, 16);
            this.lblCMLHighLevelVersion.TabIndex = 6;
            this.lblCMLHighLevelVersion.Text = "Version:";
            // 
            // lblCMLLowLevels
            // 
            this.lblCMLLowLevels.Location = new System.Drawing.Point(8, 68);
            this.lblCMLLowLevels.Name = "lblCMLLowLevels";
            this.lblCMLLowLevels.Size = new System.Drawing.Size(72, 16);
            this.lblCMLLowLevels.TabIndex = 4;
            this.lblCMLLowLevels.Text = "Low Levels:";
            // 
            // lblCMLTimePeriod
            // 
            this.lblCMLTimePeriod.Location = new System.Drawing.Point(8, 46);
            this.lblCMLTimePeriod.Name = "lblCMLTimePeriod";
            this.lblCMLTimePeriod.Size = new System.Drawing.Size(72, 16);
            this.lblCMLTimePeriod.TabIndex = 2;
            this.lblCMLTimePeriod.Text = "Time Period:";
            // 
            // lblCMLHighLevel
            // 
            this.lblCMLHighLevel.Location = new System.Drawing.Point(8, 22);
            this.lblCMLHighLevel.Name = "lblCMLHighLevel";
            this.lblCMLHighLevel.Size = new System.Drawing.Size(72, 16);
            this.lblCMLHighLevel.TabIndex = 0;
            this.lblCMLHighLevel.Text = "High Level:";
            // 
            // pnlStoreMultiLevel
            // 
            this.pnlStoreMultiLevel.Controls.Add(this.cboSMLOverride);
            this.pnlStoreMultiLevel.Controls.Add(this.lblStore);
            this.pnlStoreMultiLevel.Controls.Add(this.cboStore);
            this.pnlStoreMultiLevel.Controls.Add(this.btnSMLVersionOverride);
            this.pnlStoreMultiLevel.Controls.Add(this.grpSMLDisplay);
            this.pnlStoreMultiLevel.Controls.Add(this.grpSMLOTSPlan);
            this.pnlStoreMultiLevel.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlStoreMultiLevel.Location = new System.Drawing.Point(0, 152);
            this.pnlStoreMultiLevel.Name = "pnlStoreMultiLevel";
            this.pnlStoreMultiLevel.Size = new System.Drawing.Size(688, 200);
            this.pnlStoreMultiLevel.TabIndex = 11;
            // 
            // cboSMLOverride
            // 
            this.cboSMLOverride.AutoAdjust = true;
            this.cboSMLOverride.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboSMLOverride.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboSMLOverride.DataSource = null;
            this.cboSMLOverride.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSMLOverride.DropDownWidth = 160;
            this.cboSMLOverride.FormattingEnabled = false;
            this.cboSMLOverride.IgnoreFocusLost = false;
            this.cboSMLOverride.ItemHeight = 13;
            this.cboSMLOverride.Location = new System.Drawing.Point(512, 157);
            this.cboSMLOverride.Margin = new System.Windows.Forms.Padding(0);
            this.cboSMLOverride.MaxDropDownItems = 25;
            this.cboSMLOverride.Name = "cboSMLOverride";
            this.cboSMLOverride.SetToolTip = "";
            this.cboSMLOverride.Size = new System.Drawing.Size(160, 21);
            this.cboSMLOverride.TabIndex = 4;
            this.cboSMLOverride.Tag = null;
            this.cboSMLOverride.MIDComboBoxPropertiesChangedEvent += new MIDRetail.Windows.Controls.MIDComboBoxPropertiesChangedEventHandler(this.cboSMLOverride_MIDComboBoxPropertiesChangedEvent);
            this.cboSMLOverride.SelectionChangeCommitted += new System.EventHandler(this.cboSMLOverride_SelectionChangeCommitted);
            // 
            // lblStore
            // 
            this.lblStore.AutoSize = true;
            this.lblStore.Location = new System.Drawing.Point(509, 102);
            this.lblStore.Name = "lblStore";
            this.lblStore.Size = new System.Drawing.Size(35, 13);
            this.lblStore.TabIndex = 6;
            this.lblStore.Text = "Store:";
            // 
            // cboStore
            // 
            this.cboStore.AutoAdjust = true;
            this.cboStore.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboStore.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboStore.DataSource = null;
            this.cboStore.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboStore.DropDownWidth = 125;
            this.cboStore.FormattingEnabled = false;
            this.cboStore.IgnoreFocusLost = false;
            this.cboStore.ItemHeight = 13;
            this.cboStore.Location = new System.Drawing.Point(547, 102);
            this.cboStore.Margin = new System.Windows.Forms.Padding(0);
            this.cboStore.MaxDropDownItems = 25;
            this.cboStore.Name = "cboStore";
            this.cboStore.SetToolTip = "";
            this.cboStore.Size = new System.Drawing.Size(125, 22);
            this.cboStore.TabIndex = 5;
            this.cboStore.Tag = null;
            this.cboStore.MIDComboBoxPropertiesChangedEvent += new MIDRetail.Windows.Controls.MIDComboBoxPropertiesChangedEventHandler(this.cboStore_MIDComboBoxPropertiesChangedEvent);
            this.cboStore.SelectionChangeCommitted += new System.EventHandler(this.cboStore_SelectionChangeCommitted);
            // 
            // btnSMLVersionOverride
            // 
            this.btnSMLVersionOverride.Location = new System.Drawing.Point(512, 131);
            this.btnSMLVersionOverride.Name = "btnSMLVersionOverride";
            this.btnSMLVersionOverride.Size = new System.Drawing.Size(160, 23);
            this.btnSMLVersionOverride.TabIndex = 3;
            this.btnSMLVersionOverride.Text = "Override Low Level Versions";
            this.btnSMLVersionOverride.Click += new System.EventHandler(this.btnSMLVersionOverride_Click);
            // 
            // grpSMLDisplay
            // 
            this.grpSMLDisplay.Controls.Add(this.cboSMLView);
            this.grpSMLDisplay.Controls.Add(this.cboSMLFilter);
            this.grpSMLDisplay.Controls.Add(this.cboSMLGroupBy);
            this.grpSMLDisplay.Controls.Add(this.cboSMLStoreAttribute);
            this.grpSMLDisplay.Controls.Add(this.chkSMLSimilarStores);
            this.grpSMLDisplay.Controls.Add(this.chkSMLIneligibleStores);
            this.grpSMLDisplay.Controls.Add(this.lblSMLFilter);
            this.grpSMLDisplay.Controls.Add(this.lblSMLView);
            this.grpSMLDisplay.Controls.Add(this.lblSMLGroupBy);
            this.grpSMLDisplay.Controls.Add(this.lblSMLStoreAttribute);
            this.grpSMLDisplay.Location = new System.Drawing.Point(0, 8);
            this.grpSMLDisplay.Name = "grpSMLDisplay";
            this.grpSMLDisplay.Size = new System.Drawing.Size(672, 72);
            this.grpSMLDisplay.TabIndex = 1;
            this.grpSMLDisplay.TabStop = false;
            this.grpSMLDisplay.Text = "Display";
            // 
            // cboSMLView
            // 
            this.cboSMLView.AutoAdjust = true;
            this.cboSMLView.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboSMLView.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboSMLView.DataSource = null;
            this.cboSMLView.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSMLView.DropDownWidth = 200;
            this.cboSMLView.FormattingEnabled = false;
            this.cboSMLView.IgnoreFocusLost = false;
            this.cboSMLView.ItemHeight = 13;
            this.cboSMLView.Location = new System.Drawing.Point(328, 40);
            this.cboSMLView.Margin = new System.Windows.Forms.Padding(0);
            this.cboSMLView.MaxDropDownItems = 25;
            this.cboSMLView.Name = "cboSMLView";
            this.cboSMLView.SetToolTip = "";
            this.cboSMLView.Size = new System.Drawing.Size(200, 21);
            this.cboSMLView.TabIndex = 7;
            this.cboSMLView.Tag = null;
            this.cboSMLView.MIDComboBoxPropertiesChangedEvent += new MIDRetail.Windows.Controls.MIDComboBoxPropertiesChangedEventHandler(this.cboSMLView_MIDComboBoxPropertiesChangedEvent);
            this.cboSMLView.SelectionChangeCommitted += new System.EventHandler(this.cboView_SelectionChangeCommitted);
            // 
            // cboSMLFilter
            // 
            this.cboSMLFilter.AllowDrop = true;
            this.cboSMLFilter.AutoAdjust = true;
            this.cboSMLFilter.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboSMLFilter.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboSMLFilter.DataSource = null;
            this.cboSMLFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSMLFilter.DropDownWidth = 200;
            this.cboSMLFilter.FormattingEnabled = false;
            this.cboSMLFilter.IgnoreFocusLost = false;
            this.cboSMLFilter.ItemHeight = 13;
            this.cboSMLFilter.Location = new System.Drawing.Point(328, 16);
            this.cboSMLFilter.Margin = new System.Windows.Forms.Padding(0);
            this.cboSMLFilter.MaxDropDownItems = 25;
            this.cboSMLFilter.Name = "cboSMLFilter";
            this.cboSMLFilter.SetToolTip = "";
            this.cboSMLFilter.Size = new System.Drawing.Size(200, 21);
            this.cboSMLFilter.TabIndex = 5;
            this.cboSMLFilter.Tag = null;
            this.cboSMLFilter.MIDComboBoxPropertiesChangedEvent += new MIDRetail.Windows.Controls.MIDComboBoxPropertiesChangedEventHandler(this.cboSMLFilter_MIDComboBoxPropertiesChangedEvent);
            this.cboSMLFilter.SelectionChangeCommitted += new System.EventHandler(this.cboSMLFilter_SelectionChangeCommitted);
            this.cboSMLFilter.DragDrop += new System.Windows.Forms.DragEventHandler(this.cboFilter_DragDrop);
            this.cboSMLFilter.DragEnter += new System.Windows.Forms.DragEventHandler(this.cboFilter_DragEnter);
            this.cboSMLFilter.DragOver += new System.Windows.Forms.DragEventHandler(this.cboFilter_DragOver);
            this.cboSMLFilter.DropDown += new System.EventHandler(this.cboFilter_DropDown);
            // 
            // cboSMLGroupBy
            // 
            this.cboSMLGroupBy.AutoAdjust = true;
            this.cboSMLGroupBy.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboSMLGroupBy.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboSMLGroupBy.DataSource = null;
            this.cboSMLGroupBy.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSMLGroupBy.DropDownWidth = 192;
            this.cboSMLGroupBy.FormattingEnabled = false;
            this.cboSMLGroupBy.IgnoreFocusLost = false;
            this.cboSMLGroupBy.ItemHeight = 13;
            this.cboSMLGroupBy.Location = new System.Drawing.Point(80, 40);
            this.cboSMLGroupBy.Margin = new System.Windows.Forms.Padding(0);
            this.cboSMLGroupBy.MaxDropDownItems = 25;
            this.cboSMLGroupBy.Name = "cboSMLGroupBy";
            this.cboSMLGroupBy.SetToolTip = "";
            this.cboSMLGroupBy.Size = new System.Drawing.Size(192, 21);
            this.cboSMLGroupBy.TabIndex = 3;
            this.cboSMLGroupBy.Tag = null;
            this.cboSMLGroupBy.MIDComboBoxPropertiesChangedEvent += new MIDRetail.Windows.Controls.MIDComboBoxPropertiesChangedEventHandler(this.cboSMLGroupBy_MIDComboBoxPropertiesChangedEvent);
            this.cboSMLGroupBy.SelectionChangeCommitted += new System.EventHandler(this.cboGroupBy_SelectionChangeCommitted);
            // 
            // cboSMLStoreAttribute
            // 
            this.cboSMLStoreAttribute.AllowDrop = true;
            this.cboSMLStoreAttribute.AllowUserAttributes = false;
            this.cboSMLStoreAttribute.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboSMLStoreAttribute.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboSMLStoreAttribute.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSMLStoreAttribute.FormattingEnabled = true;
            this.cboSMLStoreAttribute.Location = new System.Drawing.Point(80, 16);
            this.cboSMLStoreAttribute.Name = "cboSMLStoreAttribute";
            this.cboSMLStoreAttribute.Size = new System.Drawing.Size(192, 21);
            this.cboSMLStoreAttribute.TabIndex = 1;
            this.cboSMLStoreAttribute.MIDComboBoxPropertiesChangedEvent += new MIDRetail.Windows.Controls.MIDComboBoxPropertiesChangedEventHandler(this.cboSMLStoreAttribute_MIDComboBoxPropertiesChangedEvent);
            this.cboSMLStoreAttribute.SelectionChangeCommitted += new System.EventHandler(this.cboStoreAttribute_SelectionChangeCommitted);
            this.cboSMLStoreAttribute.DragDrop += new System.Windows.Forms.DragEventHandler(this.cboStoreAttribute_DragDrop);
            this.cboSMLStoreAttribute.DragEnter += new System.Windows.Forms.DragEventHandler(this.cboStoreAttribute_DragEnter);
            this.cboSMLStoreAttribute.DragOver += new System.Windows.Forms.DragEventHandler(this.cboStoreAttribute_DragOver);
            // 
            // chkSMLSimilarStores
            // 
            this.chkSMLSimilarStores.Location = new System.Drawing.Point(552, 42);
            this.chkSMLSimilarStores.Name = "chkSMLSimilarStores";
            this.chkSMLSimilarStores.Size = new System.Drawing.Size(104, 16);
            this.chkSMLSimilarStores.TabIndex = 13;
            this.chkSMLSimilarStores.Text = "Similar Stores";
            this.chkSMLSimilarStores.CheckedChanged += new System.EventHandler(this.chkSimilarStores_CheckedChanged);
            // 
            // chkSMLIneligibleStores
            // 
            this.chkSMLIneligibleStores.Location = new System.Drawing.Point(552, 18);
            this.chkSMLIneligibleStores.Name = "chkSMLIneligibleStores";
            this.chkSMLIneligibleStores.Size = new System.Drawing.Size(104, 16);
            this.chkSMLIneligibleStores.TabIndex = 12;
            this.chkSMLIneligibleStores.Text = "Ineligible Stores";
            this.chkSMLIneligibleStores.CheckedChanged += new System.EventHandler(this.chkIneligibleStores_CheckedChanged);
            // 
            // lblSMLFilter
            // 
            this.lblSMLFilter.Location = new System.Drawing.Point(280, 22);
            this.lblSMLFilter.Name = "lblSMLFilter";
            this.lblSMLFilter.Size = new System.Drawing.Size(40, 16);
            this.lblSMLFilter.TabIndex = 4;
            this.lblSMLFilter.Text = "Filter:";
            // 
            // lblSMLView
            // 
            this.lblSMLView.Location = new System.Drawing.Point(280, 46);
            this.lblSMLView.Name = "lblSMLView";
            this.lblSMLView.Size = new System.Drawing.Size(40, 16);
            this.lblSMLView.TabIndex = 6;
            this.lblSMLView.Text = "View:";
            // 
            // lblSMLGroupBy
            // 
            this.lblSMLGroupBy.Location = new System.Drawing.Point(8, 46);
            this.lblSMLGroupBy.Name = "lblSMLGroupBy";
            this.lblSMLGroupBy.Size = new System.Drawing.Size(80, 16);
            this.lblSMLGroupBy.TabIndex = 2;
            this.lblSMLGroupBy.Text = "Group By:";
            // 
            // lblSMLStoreAttribute
            // 
            this.lblSMLStoreAttribute.Location = new System.Drawing.Point(8, 22);
            this.lblSMLStoreAttribute.Name = "lblSMLStoreAttribute";
            this.lblSMLStoreAttribute.Size = new System.Drawing.Size(80, 16);
            this.lblSMLStoreAttribute.TabIndex = 0;
            this.lblSMLStoreAttribute.Text = "Store Attibute:";
            // 
            // grpSMLOTSPlan
            // 
            this.grpSMLOTSPlan.Controls.Add(this.cboSMLLowLevels);
            this.grpSMLOTSPlan.Controls.Add(this.cboSMLLowLevelsVersion);
            this.grpSMLOTSPlan.Controls.Add(this.cboSMLHighLevelVersion);
            this.grpSMLOTSPlan.Controls.Add(this.mdsSMLPlanDateRange);
            this.grpSMLOTSPlan.Controls.Add(this.txtSMLHighLevelNode);
            this.grpSMLOTSPlan.Controls.Add(this.lblSMLLowLevelsVersion);
            this.grpSMLOTSPlan.Controls.Add(this.lblSMLHighLevelVersion);
            this.grpSMLOTSPlan.Controls.Add(this.lblSMLLowLevels);
            this.grpSMLOTSPlan.Controls.Add(this.lblSMLTimePeriod);
            this.grpSMLOTSPlan.Controls.Add(this.lblSMLHighLevel);
            this.grpSMLOTSPlan.Location = new System.Drawing.Point(0, 88);
            this.grpSMLOTSPlan.Name = "grpSMLOTSPlan";
            this.grpSMLOTSPlan.Size = new System.Drawing.Size(504, 96);
            this.grpSMLOTSPlan.TabIndex = 2;
            this.grpSMLOTSPlan.TabStop = false;
            this.grpSMLOTSPlan.Text = "OTS Plan";
            // 
            // cboSMLLowLevels
            // 
            this.cboSMLLowLevels.AutoAdjust = true;
            this.cboSMLLowLevels.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboSMLLowLevels.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboSMLLowLevels.DataSource = null;
            this.cboSMLLowLevels.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSMLLowLevels.DropDownWidth = 192;
            this.cboSMLLowLevels.FormattingEnabled = false;
            this.cboSMLLowLevels.IgnoreFocusLost = false;
            this.cboSMLLowLevels.ItemHeight = 13;
            this.cboSMLLowLevels.Location = new System.Drawing.Point(80, 64);
            this.cboSMLLowLevels.Margin = new System.Windows.Forms.Padding(0);
            this.cboSMLLowLevels.MaxDropDownItems = 25;
            this.cboSMLLowLevels.Name = "cboSMLLowLevels";
            this.cboSMLLowLevels.SetToolTip = "";
            this.cboSMLLowLevels.Size = new System.Drawing.Size(192, 21);
            this.cboSMLLowLevels.TabIndex = 10;
            this.cboSMLLowLevels.Tag = null;
            this.cboSMLLowLevels.MIDComboBoxPropertiesChangedEvent += new MIDRetail.Windows.Controls.MIDComboBoxPropertiesChangedEventHandler(this.cboSMLLowLevels_MIDComboBoxPropertiesChangedEvent);
            this.cboSMLLowLevels.SelectionChangeCommitted += new System.EventHandler(this.cboSMLLowLevels_SelectionChangeCommitted);
            // 
            // cboSMLLowLevelsVersion
            // 
            this.cboSMLLowLevelsVersion.AutoAdjust = false;
            this.cboSMLLowLevelsVersion.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboSMLLowLevelsVersion.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboSMLLowLevelsVersion.DataSource = null;
            this.cboSMLLowLevelsVersion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSMLLowLevelsVersion.DropDownWidth = 152;
            this.cboSMLLowLevelsVersion.FormattingEnabled = false;
            this.cboSMLLowLevelsVersion.IgnoreFocusLost = false;
            this.cboSMLLowLevelsVersion.ItemHeight = 13;
            this.cboSMLLowLevelsVersion.Location = new System.Drawing.Point(328, 64);
            this.cboSMLLowLevelsVersion.Margin = new System.Windows.Forms.Padding(0);
            this.cboSMLLowLevelsVersion.MaxDropDownItems = 25;
            this.cboSMLLowLevelsVersion.Name = "cboSMLLowLevelsVersion";
            this.cboSMLLowLevelsVersion.SetToolTip = "";
            this.cboSMLLowLevelsVersion.Size = new System.Drawing.Size(152, 21);
            this.cboSMLLowLevelsVersion.TabIndex = 9;
            this.cboSMLLowLevelsVersion.Tag = null;
            this.cboSMLLowLevelsVersion.MIDComboBoxPropertiesChangedEvent += new MIDRetail.Windows.Controls.MIDComboBoxPropertiesChangedEventHandler(this.cboSMLLowLevelsVersion_MIDComboBoxPropertiesChangedEvent);
            this.cboSMLLowLevelsVersion.SelectionChangeCommitted += new System.EventHandler(this.cboVersion_SelectionChangeCommitted);
            // 
            // cboSMLHighLevelVersion
            // 
            this.cboSMLHighLevelVersion.AutoAdjust = true;
            this.cboSMLHighLevelVersion.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboSMLHighLevelVersion.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboSMLHighLevelVersion.DataSource = null;
            this.cboSMLHighLevelVersion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSMLHighLevelVersion.DropDownWidth = 152;
            this.cboSMLHighLevelVersion.FormattingEnabled = false;
            this.cboSMLHighLevelVersion.IgnoreFocusLost = false;
            this.cboSMLHighLevelVersion.ItemHeight = 13;
            this.cboSMLHighLevelVersion.Location = new System.Drawing.Point(328, 15);
            this.cboSMLHighLevelVersion.Margin = new System.Windows.Forms.Padding(0);
            this.cboSMLHighLevelVersion.MaxDropDownItems = 25;
            this.cboSMLHighLevelVersion.Name = "cboSMLHighLevelVersion";
            this.cboSMLHighLevelVersion.SetToolTip = "";
            this.cboSMLHighLevelVersion.Size = new System.Drawing.Size(152, 21);
            this.cboSMLHighLevelVersion.TabIndex = 7;
            this.cboSMLHighLevelVersion.Tag = null;
            this.cboSMLHighLevelVersion.MIDComboBoxPropertiesChangedEvent += new MIDRetail.Windows.Controls.MIDComboBoxPropertiesChangedEventHandler(this.cboSMLHighLevelVersion_MIDComboBoxPropertiesChangedEvent);
            this.cboSMLHighLevelVersion.SelectionChangeCommitted += new System.EventHandler(this.cboVersion_SelectionChangeCommitted);
            // 
            // mdsSMLPlanDateRange
            // 
            this.mdsSMLPlanDateRange.DateRangeForm = null;
            this.mdsSMLPlanDateRange.DateRangeRID = 0;
            this.mdsSMLPlanDateRange.Enabled = false;
            this.mdsSMLPlanDateRange.Location = new System.Drawing.Point(80, 40);
            this.mdsSMLPlanDateRange.Name = "mdsSMLPlanDateRange";
            this.mdsSMLPlanDateRange.Size = new System.Drawing.Size(192, 24);
            this.mdsSMLPlanDateRange.TabIndex = 3;
            this.mdsSMLPlanDateRange.OnSelection += new MIDRetail.Windows.Controls.MIDDateRangeSelector.SelectionEventHandler(this.mdsPlanDateRange_OnSelection);
            this.mdsSMLPlanDateRange.Click += new System.EventHandler(this.mdsSMLPlanDateRange_Click);
            // 
            // txtSMLHighLevelNode
            // 
            this.txtSMLHighLevelNode.AllowDrop = true;
            this.txtSMLHighLevelNode.Location = new System.Drawing.Point(80, 16);
            this.txtSMLHighLevelNode.Name = "txtSMLHighLevelNode";
            this.txtSMLHighLevelNode.Size = new System.Drawing.Size(192, 20);
            this.txtSMLHighLevelNode.TabIndex = 1;
            this.txtSMLHighLevelNode.TextChanged += new System.EventHandler(this.txtNode_TextChanged);
            this.txtSMLHighLevelNode.DragDrop += new System.Windows.Forms.DragEventHandler(this.txtNode_DragDrop);
            this.txtSMLHighLevelNode.DragEnter += new System.Windows.Forms.DragEventHandler(this.txtNode_DragEnter);
            this.txtSMLHighLevelNode.DragOver += new System.Windows.Forms.DragEventHandler(this.txtNode_DragOver);
            this.txtSMLHighLevelNode.Validating += new System.ComponentModel.CancelEventHandler(this.txtNode_Validating);
            this.txtSMLHighLevelNode.Validated += new System.EventHandler(this.txtNode_Validated);
            // 
            // lblSMLLowLevelsVersion
            // 
            this.lblSMLLowLevelsVersion.Location = new System.Drawing.Point(280, 68);
            this.lblSMLLowLevelsVersion.Name = "lblSMLLowLevelsVersion";
            this.lblSMLLowLevelsVersion.Size = new System.Drawing.Size(48, 16);
            this.lblSMLLowLevelsVersion.TabIndex = 8;
            this.lblSMLLowLevelsVersion.Text = "Version:";
            // 
            // lblSMLHighLevelVersion
            // 
            this.lblSMLHighLevelVersion.Location = new System.Drawing.Point(280, 22);
            this.lblSMLHighLevelVersion.Name = "lblSMLHighLevelVersion";
            this.lblSMLHighLevelVersion.Size = new System.Drawing.Size(48, 16);
            this.lblSMLHighLevelVersion.TabIndex = 6;
            this.lblSMLHighLevelVersion.Text = "Version:";
            // 
            // lblSMLLowLevels
            // 
            this.lblSMLLowLevels.Location = new System.Drawing.Point(8, 68);
            this.lblSMLLowLevels.Name = "lblSMLLowLevels";
            this.lblSMLLowLevels.Size = new System.Drawing.Size(72, 16);
            this.lblSMLLowLevels.TabIndex = 4;
            this.lblSMLLowLevels.Text = "Low Levels";
            // 
            // lblSMLTimePeriod
            // 
            this.lblSMLTimePeriod.Location = new System.Drawing.Point(8, 48);
            this.lblSMLTimePeriod.Name = "lblSMLTimePeriod";
            this.lblSMLTimePeriod.Size = new System.Drawing.Size(72, 16);
            this.lblSMLTimePeriod.TabIndex = 2;
            this.lblSMLTimePeriod.Text = "Time Period:";
            // 
            // lblSMLHighLevel
            // 
            this.lblSMLHighLevel.Location = new System.Drawing.Point(8, 24);
            this.lblSMLHighLevel.Name = "lblSMLHighLevel";
            this.lblSMLHighLevel.Size = new System.Drawing.Size(72, 16);
            this.lblSMLHighLevel.TabIndex = 0;
            this.lblSMLHighLevel.Text = "High Level:";
            // 
            // pnlChainSingleLevel
            // 
            this.pnlChainSingleLevel.Controls.Add(this.grpCSLDisplay);
            this.pnlChainSingleLevel.Controls.Add(this.grpCSLOTSPlan);
            this.pnlChainSingleLevel.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlChainSingleLevel.Location = new System.Drawing.Point(0, 0);
            this.pnlChainSingleLevel.Name = "pnlChainSingleLevel";
            this.pnlChainSingleLevel.Size = new System.Drawing.Size(688, 152);
            this.pnlChainSingleLevel.TabIndex = 10;
            // 
            // grpCSLDisplay
            // 
            this.grpCSLDisplay.Controls.Add(this.cboCSLView);
            this.grpCSLDisplay.Controls.Add(this.lblCSLView);
            this.grpCSLDisplay.Location = new System.Drawing.Point(0, 8);
            this.grpCSLDisplay.Name = "grpCSLDisplay";
            this.grpCSLDisplay.Size = new System.Drawing.Size(296, 48);
            this.grpCSLDisplay.TabIndex = 1;
            this.grpCSLDisplay.TabStop = false;
            this.grpCSLDisplay.Text = "Display";
            // 
            // cboCSLView
            // 
            this.cboCSLView.AutoAdjust = true;
            this.cboCSLView.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboCSLView.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboCSLView.DataSource = null;
            this.cboCSLView.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboCSLView.DropDownWidth = 200;
            this.cboCSLView.FormattingEnabled = false;
            this.cboCSLView.IgnoreFocusLost = false;
            this.cboCSLView.ItemHeight = 13;
            this.cboCSLView.Location = new System.Drawing.Point(80, 16);
            this.cboCSLView.Margin = new System.Windows.Forms.Padding(0);
            this.cboCSLView.MaxDropDownItems = 25;
            this.cboCSLView.Name = "cboCSLView";
            this.cboCSLView.SetToolTip = "";
            this.cboCSLView.Size = new System.Drawing.Size(200, 21);
            this.cboCSLView.TabIndex = 7;
            this.cboCSLView.Tag = null;
            this.cboCSLView.MIDComboBoxPropertiesChangedEvent += new MIDRetail.Windows.Controls.MIDComboBoxPropertiesChangedEventHandler(this.cboCSLView_MIDComboBoxPropertiesChangedEvent);
            this.cboCSLView.SelectionChangeCommitted += new System.EventHandler(this.cboView_SelectionChangeCommitted);
            // 
            // lblCSLView
            // 
            this.lblCSLView.Location = new System.Drawing.Point(8, 24);
            this.lblCSLView.Name = "lblCSLView";
            this.lblCSLView.Size = new System.Drawing.Size(40, 16);
            this.lblCSLView.TabIndex = 6;
            this.lblCSLView.Text = "View:";
            // 
            // grpCSLOTSPlan
            // 
            this.grpCSLOTSPlan.Controls.Add(this.cboCSLChainVersion);
            this.grpCSLOTSPlan.Controls.Add(this.mdsCSLPlanDateRange);
            this.grpCSLOTSPlan.Controls.Add(this.txtCSLChainNode);
            this.grpCSLOTSPlan.Controls.Add(this.lblCSLChainVersion);
            this.grpCSLOTSPlan.Controls.Add(this.lblCSLTimePeriod);
            this.grpCSLOTSPlan.Controls.Add(this.lblCSLChainNode);
            this.grpCSLOTSPlan.Location = new System.Drawing.Point(0, 64);
            this.grpCSLOTSPlan.Name = "grpCSLOTSPlan";
            this.grpCSLOTSPlan.Size = new System.Drawing.Size(504, 72);
            this.grpCSLOTSPlan.TabIndex = 2;
            this.grpCSLOTSPlan.TabStop = false;
            this.grpCSLOTSPlan.Text = "OTS Plan";
            // 
            // cboCSLChainVersion
            // 
            this.cboCSLChainVersion.AutoAdjust = true;
            this.cboCSLChainVersion.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboCSLChainVersion.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboCSLChainVersion.DataSource = null;
            this.cboCSLChainVersion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboCSLChainVersion.DropDownWidth = 152;
            this.cboCSLChainVersion.FormattingEnabled = false;
            this.cboCSLChainVersion.IgnoreFocusLost = false;
            this.cboCSLChainVersion.ItemHeight = 13;
            this.cboCSLChainVersion.Location = new System.Drawing.Point(328, 19);
            this.cboCSLChainVersion.Margin = new System.Windows.Forms.Padding(0);
            this.cboCSLChainVersion.MaxDropDownItems = 25;
            this.cboCSLChainVersion.Name = "cboCSLChainVersion";
            this.cboCSLChainVersion.SetToolTip = "";
            this.cboCSLChainVersion.Size = new System.Drawing.Size(152, 21);
            this.cboCSLChainVersion.TabIndex = 7;
            this.cboCSLChainVersion.Tag = null;
            this.cboCSLChainVersion.MIDComboBoxPropertiesChangedEvent += new MIDRetail.Windows.Controls.MIDComboBoxPropertiesChangedEventHandler(this.cboCSLChainVersion_MIDComboBoxPropertiesChangedEvent);
            this.cboCSLChainVersion.SelectionChangeCommitted += new System.EventHandler(this.cboVersion_SelectionChangeCommitted);
            // 
            // mdsCSLPlanDateRange
            // 
            this.mdsCSLPlanDateRange.DateRangeForm = null;
            this.mdsCSLPlanDateRange.DateRangeRID = 0;
            this.mdsCSLPlanDateRange.Enabled = false;
            this.mdsCSLPlanDateRange.Location = new System.Drawing.Point(80, 40);
            this.mdsCSLPlanDateRange.Name = "mdsCSLPlanDateRange";
            this.mdsCSLPlanDateRange.Size = new System.Drawing.Size(192, 24);
            this.mdsCSLPlanDateRange.TabIndex = 3;
            this.mdsCSLPlanDateRange.OnSelection += new MIDRetail.Windows.Controls.MIDDateRangeSelector.SelectionEventHandler(this.mdsPlanDateRange_OnSelection);
            this.mdsCSLPlanDateRange.Click += new System.EventHandler(this.mdsCSLPlanDateRange_Click);
            // 
            // txtCSLChainNode
            // 
            this.txtCSLChainNode.AllowDrop = true;
            this.txtCSLChainNode.Location = new System.Drawing.Point(80, 16);
            this.txtCSLChainNode.Name = "txtCSLChainNode";
            this.txtCSLChainNode.Size = new System.Drawing.Size(192, 20);
            this.txtCSLChainNode.TabIndex = 1;
            this.txtCSLChainNode.TextChanged += new System.EventHandler(this.txtNode_TextChanged);
            this.txtCSLChainNode.DragDrop += new System.Windows.Forms.DragEventHandler(this.txtNode_DragDrop);
            this.txtCSLChainNode.DragEnter += new System.Windows.Forms.DragEventHandler(this.txtNode_DragEnter);
            this.txtCSLChainNode.DragOver += new System.Windows.Forms.DragEventHandler(this.txtNode_DragOver);
            this.txtCSLChainNode.Validating += new System.ComponentModel.CancelEventHandler(this.txtNode_Validating);
            this.txtCSLChainNode.Validated += new System.EventHandler(this.txtNode_Validated);
            // 
            // lblCSLChainVersion
            // 
            this.lblCSLChainVersion.Location = new System.Drawing.Point(280, 24);
            this.lblCSLChainVersion.Name = "lblCSLChainVersion";
            this.lblCSLChainVersion.Size = new System.Drawing.Size(48, 16);
            this.lblCSLChainVersion.TabIndex = 6;
            this.lblCSLChainVersion.Text = "Version:";
            // 
            // lblCSLTimePeriod
            // 
            this.lblCSLTimePeriod.Location = new System.Drawing.Point(8, 48);
            this.lblCSLTimePeriod.Name = "lblCSLTimePeriod";
            this.lblCSLTimePeriod.Size = new System.Drawing.Size(72, 16);
            this.lblCSLTimePeriod.TabIndex = 2;
            this.lblCSLTimePeriod.Text = "Time Period:";
            // 
            // lblCSLChainNode
            // 
            this.lblCSLChainNode.Location = new System.Drawing.Point(8, 24);
            this.lblCSLChainNode.Name = "lblCSLChainNode";
            this.lblCSLChainNode.Size = new System.Drawing.Size(72, 16);
            this.lblCSLChainNode.TabIndex = 0;
            this.lblCSLChainNode.Text = "Chain:";
            // 
            // cboComputationMode
            // 
            this.cboComputationMode.AutoAdjust = true;
            this.cboComputationMode.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboComputationMode.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboComputationMode.DataSource = null;
            this.cboComputationMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboComputationMode.DropDownWidth = 160;
            this.cboComputationMode.FormattingEnabled = false;
            this.cboComputationMode.IgnoreFocusLost = false;
            this.cboComputationMode.ItemHeight = 13;
            this.cboComputationMode.Location = new System.Drawing.Point(533, 24);
            this.cboComputationMode.Margin = new System.Windows.Forms.Padding(0);
            this.cboComputationMode.MaxDropDownItems = 25;
            this.cboComputationMode.Name = "cboComputationMode";
            this.cboComputationMode.SetToolTip = "";
            this.cboComputationMode.Size = new System.Drawing.Size(160, 21);
            this.cboComputationMode.TabIndex = 15;
            this.cboComputationMode.Tag = null;
            // 
            // OTSPlanSelection
            // 
            this.AllowDragDrop = true;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(704, 590);
            this.Controls.Add(this.cboComputationMode);
            this.Controls.Add(this.pnlMain);
            this.Controls.Add(this.lblComputationMode);
            this.Controls.Add(this.grpType);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Name = "OTSPlanSelection";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.OTSPlanSelection_Closing);
            this.Closed += new System.EventHandler(this.OTSPlanSelection_Closed);
            this.Load += new System.EventHandler(this.OTSPlanSelection_Load);
            this.Controls.SetChildIndex(this.btnOK, 0);
            this.Controls.SetChildIndex(this.btnCancel, 0);
            this.Controls.SetChildIndex(this.grpType, 0);
            this.Controls.SetChildIndex(this.lblComputationMode, 0);
            this.Controls.SetChildIndex(this.pnlMain, 0);
            this.Controls.SetChildIndex(this.cboComputationMode, 0);
            ((System.ComponentModel.ISupportInitialize)(this.utmMain)).EndInit();
            this.grpType.ResumeLayout(false);
            this.grpType.PerformLayout();
            this.pnlMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdBasis)).EndInit();
            this.pnlStoreSingleLevel.ResumeLayout(false);
            this.pnlStoreSingleLevel.PerformLayout();
            this.grpSSLDisplay.ResumeLayout(false);
            this.grpSSLOTSPlan.ResumeLayout(false);
            this.grpSSLOTSPlan.PerformLayout();
            this.pnlChainMultiLevel.ResumeLayout(false);
            this.grpCMLDisplay.ResumeLayout(false);
            this.grpCMLOTSPlan.ResumeLayout(false);
            this.grpCMLOTSPlan.PerformLayout();
            this.pnlStoreMultiLevel.ResumeLayout(false);
            this.pnlStoreMultiLevel.PerformLayout();
            this.grpSMLDisplay.ResumeLayout(false);
            this.grpSMLOTSPlan.ResumeLayout(false);
            this.grpSMLOTSPlan.PerformLayout();
            this.pnlChainSingleLevel.ResumeLayout(false);
            this.grpCSLDisplay.ResumeLayout(false);
            this.grpCSLOTSPlan.ResumeLayout(false);
            this.grpCSLOTSPlan.PerformLayout();
            this.ResumeLayout(false);

		}

		void grdBasis_DragEnter(object sender, DragEventArgs e)
		{
			Image_DragEnter(sender, e);
		}
		#endregion
		#endregion

		#region Fields

		private Bitmap _picInclude;
		private Bitmap _picExclude;
		private Image _dynamicToPlanImage;
		private Image _dynamicToCurrentImage;
		private Image _dynamicSwitchImage;

		private SessionAddressBlock _sab;
//		private CalendarDateSelector _frm;
		private ProfileList _versionProfList;
		// BEGIN Issue 5708 stodd 8.7.2008
		private ProfileList _chainPlanVersionProfList;
		private ProfileList _storePlanVersionProfList;
		private ProfileList _chainBasisVersionProfList;
		private ProfileList _storeBasisVersionProfList;
		// END Issue 5708
		private ArrayList _chainList;
		private ArrayList _storeList;
		// Begin Track #5960 - JSmith - Get "No Low Levels Found" when using hierarchy level name
		private ArrayList _lowLevels;
		// End Track #5960
		private ProfileList _chainLowLevelVersionList;
		private ProfileList _storeLowLevelVersionList;
		private LowLevelVersionOverrideProfileList _lowlevelVersionOverrideList;  // Override Low Level changes
		private MIDRetail.Data.OTSPlanSelection _planSelectDL;
		private PlanViewData _viewDL;
		//private StoreFilterData _storeFilterDL;
        private FilterData _storeFilterDL;
		private int _passedNodeID;
		private DataSet _dsBasis;
//		private OTSPlanSelectionData _openParms;
		private PlanOpenParms _openParms;
		private DataTable _dtView;
		private ArrayList _userRIDList;
		private FunctionSecurityProfile _singleChainSecurity;
		private FunctionSecurityProfile _multiChainSecurity;
		private FunctionSecurityProfile _singleStoreSecurity;
		private FunctionSecurityProfile _multiStoreSecurity;
		private bool _OTSFormPopulated = false;
		private bool _textChanged = false;
		private bool _priorError = false;
		private bool _skipEdit = false;
		//Begin Track #4358 - JSmith - Performance opening alternate node
		private int _currentLowLevelNode = Include.NoRID;
		private int _longestBranch = Include.NoRID;
		private int _longestHighestGuest = Include.NoRID;
		//End Track #4358
//Begin Modification - JScott - Allow access to Computation Mode with Config Setting
		bool _MIDOnlyFunctions;
//End Modification - JScott - Allow access to Computation Mode with Config Setting

		#endregion

		#region Constructors

		public OTSPlanSelection(SessionAddressBlock SAB)
			: base(SAB)
		{
			InitializeComponent();

			_sab = SAB;
			_passedNodeID = 0;
			CommonConstructorCode();
		}

		public OTSPlanSelection(SessionAddressBlock SAB, int aPassedNodeID)
			: base(SAB)
		{
			InitializeComponent();

			_sab = SAB;
			_passedNodeID = aPassedNodeID;
			CommonConstructorCode();
		}

		private void CommonConstructorCode()
		{
			AllowDragDrop = true;
			_planSelectDL = new MIDRetail.Data.OTSPlanSelection();
			_viewDL = new PlanViewData();
			_storeFilterDL = new FilterData();
			_lowlevelVersionOverrideList = new LowLevelVersionOverrideProfileList(eProfileType.LowLevelVersionOverride);

			this.Text = MIDText.GetTextOnly(eMIDTextCode.frm_OTSPlanSelection);

			this.grpType.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_PlanType);

			this.grpSSLDisplay.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_DisplayOptions);
			this.grpSMLDisplay.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_DisplayOptions);
			this.grpCSLDisplay.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_DisplayOptions);
			this.grpCMLDisplay.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_DisplayOptions);

			this.grpSSLOTSPlan.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_OTSPlan);
			this.grpSMLOTSPlan.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_OTSPlan);
			this.grpCSLOTSPlan.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_OTSPlan);
			this.grpCMLOTSPlan.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_OTSPlan);

			this.grdBasis.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Basis);
			this.radStore.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_PlanTypeStore);
			this.radChain.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_PlanTypeChain);
            this.radChainLadder.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_PlanTypeChainLadder);  // TTT#609-MD
            this.chkRT.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_TotalsRight);  // TT#639-MD -agallagher - OTS Forecast Totals Right
			this.chkMultiLevel.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_PlanTypeMultiLevel);

			this.lblComputationMode.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_ComputationMode) + ":";

			this.lblSSLStoreAttribute.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Attribute) + ":";
			this.lblSMLStoreAttribute.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Attribute) + ":";

			this.lblSSLGroupBy.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_GroupBy) + ":";
			this.lblSMLGroupBy.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_GroupBy) + ":";
			this.lblCMLGroupBy.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_GroupBy) + ":";

			this.lblSSLFilter.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Filter) + ":";
			this.lblSMLFilter.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Filter) + ":";
//			this.lblCSLFilter.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Filter) + ":";
			this.lblCMLFilter.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Filter) + ":";

			this.lblSSLView.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_View) + ":";
			this.lblSMLView.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_View) + ":";
			this.lblCSLView.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_View) + ":";
			this.lblCMLView.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_View) + ":";

			this.chkSSLIneligibleStores.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_IncludeIneligibleStore) + ":";
			this.chkSMLIneligibleStores.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_IncludeIneligibleStore) + ":";

			this.chkSSLSimilarStores.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_IncludeSimilarStores) + ":";
			this.chkSMLSimilarStores.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_IncludeSimilarStores) + ":";

			this.lblSSLStoreNode.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_PlanTypeStore) + ":";
			this.lblSSLChainNode.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_PlanTypeChain) + ":";
			this.lblCSLChainNode.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_PlanTypeChain) + ":";

			this.lblSSLTimePeriod.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_PlanTimePeriod) + ":";
			this.lblSMLTimePeriod.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_PlanTimePeriod) + ":";
			this.lblCSLTimePeriod.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_PlanTimePeriod) + ":";
			this.lblCMLTimePeriod.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_PlanTimePeriod) + ":";

			this.lblSSLStoreVersion.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Version) + ":";
			this.lblSSLChainVersion.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Version) + ":";
			this.lblSMLHighLevelVersion.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Version) + ":";
			this.lblSMLLowLevelsVersion.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Version) + ":";
			this.lblCSLChainVersion.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Version) + ":";
			this.lblCMLHighLevelVersion.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Version) + ":";
			this.lblCMLLowLevelsVersion.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Version) + ":";

			this.lblSMLHighLevel.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_HighLevel) + ":";
			this.lblCMLHighLevel.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_HighLevel) + ":";

			this.lblSMLLowLevels.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_LowLevels) + ":";
			this.lblCMLLowLevels.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_LowLevels) + ":";

			this.btnOK.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_OK);
			this.btnCancel.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_Cancel);
			this.btnCMLVersionOverride.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_OverrideLowVersion);
			this.btnSMLVersionOverride.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_OverrideLowVersion);
		}

		#endregion

		#region Form setup

		public void PopulateForm()
		{
			FunctionSecurityProfile filterUserSecurity;
			FunctionSecurityProfile filterGlobalSecurity;
//Begin Modification - JScott - Allow access to Computation Mode with Config Setting
			string MIDOnlyFunctionsStr;
//End Modification - JScott - Allow access to Computation Mode with Config Setting
			// Begin TT#1125 - JSmith - Global/User should be consistent
            //SecurityAdmin secAdmin; //TT#827-MD -jsobek -Allocation Reviews Performance
			int userRID;
			// End TT#1125

			try
			{
				FormLoaded = false;
				// Begin TT#1125 - JSmith - Global/User should be consistent
                //secAdmin = new SecurityAdmin(); //TT#827-MD -jsobek -Allocation Reviews Performance
				// End TT#1125
				//Begin Track #5858 - JSmith - Validating store security only
				//cboSSLFilter.Tag = "IgnoreMouseWheel";
				// Begin TT#44 - JSmith - Drag/Drop User Attributes or Filters in to Global Methods does not react consistantly
				//cboSSLFilter.Tag = new MIDStoreFilterComboBoxTag(SAB, cboSSLFilter, eMIDControlCode.form_OTSPlanSelection, eSecurityTypes.Store, eSecuritySelectType.View | eSecuritySelectType.Update, true);
				//cboSMLFilter.Tag = new MIDStoreFilterComboBoxTag(SAB, cboSMLFilter, eMIDControlCode.form_OTSPlanSelection, eSecurityTypes.Store, eSecuritySelectType.View | eSecuritySelectType.Update, true);
				//cboCMLFilter.Tag = new MIDStoreFilterComboBoxTag(SAB, cboCMLFilter, eMIDControlCode.form_OTSPlanSelection, eSecurityTypes.Store, eSecuritySelectType.View | eSecuritySelectType.Update, true);
				FunctionSecurity = new FunctionSecurityProfile((int)eSecurityFunctions.ForecastReview);
				FunctionSecurity.SetAllowUpdate();
                cboSSLFilter.Tag = new MIDStoreFilterComboBoxTag(SAB, cboSSLFilter.ComboBox, eMIDControlCode.form_OTSPlanSelection, eSecurityTypes.Store, eSecuritySelectType.View | eSecuritySelectType.Update, true, FunctionSecurity, true);
                cboSMLFilter.Tag = new MIDStoreFilterComboBoxTag(SAB, cboSMLFilter.ComboBox, eMIDControlCode.form_OTSPlanSelection, eSecurityTypes.Store, eSecuritySelectType.View | eSecuritySelectType.Update, true, FunctionSecurity, true);
                cboCMLFilter.Tag = new MIDStoreFilterComboBoxTag(SAB, cboCMLFilter.ComboBox, eMIDControlCode.form_OTSPlanSelection, eSecurityTypes.Store, eSecuritySelectType.View | eSecuritySelectType.Update, true, FunctionSecurity, true);
				// End TT#44
				//End Track #5858

				//setup images to be used in the grid.

				_picInclude = new Bitmap(GraphicsDirectory + "\\include.gif");
				_picExclude = new Bitmap(GraphicsDirectory + "\\exclude.gif");
				_dynamicToPlanImage = Image.FromFile(GraphicsDirectory + "\\" + MIDGraphics.DynamicToPlanImage);
				_dynamicToCurrentImage = Image.FromFile(GraphicsDirectory + "\\" + MIDGraphics.DynamicToCurrentImage);
				_dynamicSwitchImage = Image.FromFile(GraphicsDirectory + "\\" + MIDGraphics.DynamicSwitchImage);

				//Populate the form.

				// Get Versions and split into Store and Chain versions

				//=========================================================================================
				// BEGIN Issue 5807 stodd 8.7.2008
				// The _versionProfList holds ALL versions the user can see and use.
				// The other lists are based upon the paricular security setting.
				// Note that for Plan, we are setting the ePlanBasisType to Basis. This is on purpose, so
				// we pick up the view-only versions.
				// Begin Track #5882 stodd
				_versionProfList = _sab.ClientServerSession.GetUserForecastVersions();
				_chainPlanVersionProfList = _sab.ClientServerSession.GetUserForecastVersions(eSecuritySelectType.View | eSecuritySelectType.Update, eSecurityTypes.Chain);	// Track #5871
				_chainBasisVersionProfList = _sab.ClientServerSession.GetUserForecastVersions(eSecuritySelectType.View | eSecuritySelectType.Update, eSecurityTypes.Chain);	// Track #5871
				_storePlanVersionProfList = _sab.ClientServerSession.GetUserForecastVersions(eSecuritySelectType.View | eSecuritySelectType.Update, eSecurityTypes.Store);	// Track #5871
				_storeBasisVersionProfList = _sab.ClientServerSession.GetUserForecastVersions(eSecuritySelectType.View | eSecuritySelectType.Update, eSecurityTypes.Store);	// Track #5871
				// End track 5882
				// END Issue 5807
				_chainList = new ArrayList();
				_storeList = new ArrayList();
				_chainLowLevelVersionList = new ProfileList(eProfileType.Version);
				_storeLowLevelVersionList = new ProfileList(eProfileType.Version);

				// BEGIN Issue 5807 stodd 8.7.2008
				// Chain / Plan
				foreach (VersionProfile versionProfile in _chainPlanVersionProfList.ArrayList)
				{
						_chainList.Add(versionProfile);
						_chainLowLevelVersionList.Add(versionProfile);
				}

				// Store / Plan
				foreach (VersionProfile versionProfile in _storePlanVersionProfList.ArrayList)
				{
						_storeList.Add(versionProfile);
						_storeLowLevelVersionList.Add(versionProfile);
				}
				// END Issue 5708

				LoadSelectionData(_planSelectDL.GetPlanSelectionMainInfo(_sab.ClientServerSession.UserRID));

				// BEGIN OVerride Low Level Enhancement
				if (_openParms.OverrideLowLevelRid != Include.NoRID)
				{
				ModelsData modelData = new ModelsData();
				cboSMLOverride.SelectedValue = modelData.OverrideLowLevelsModel_GetModelName(_openParms.OverrideLowLevelRid);
                cboCMLOverride.SelectedValue = _openParms.OverrideLowLevelRid;
				}
				// END OVerride Low Level Enhancement

				if (_sab.ClientServerSession.GlobalOptions.AppConfig.PlanningInstalled)
				{
					grpType.Visible = true;
				}
				else
				{
					grpType.Visible = false;
				}

//Begin Track #5197 - JScott - Deny Global View still allows user to use them
//				_userRIDList = new ArrayList();
//				_userRIDList.Add(Include.GlobalUserRID);  // issue 3806
//				_userRIDList.Add(_sab.ClientServerSession.UserRID);
				FunctionSecurityProfile viewUserSecurity = _sab.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ForecastViewsUser);
				FunctionSecurityProfile viewGlobalSecurity = _sab.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ForecastViewsGlobal);

				_userRIDList = new ArrayList();

				_userRIDList.Add(-1);
				
				if (viewUserSecurity.AllowView)
				{
					_userRIDList.Add(_sab.ClientServerSession.UserRID);
				}

				if (viewGlobalSecurity.AllowView)
				{
					_userRIDList.Add(Include.GlobalUserRID);
				}
//End Track #5197 - JScott - Deny Global View still allows user to use them

				// Load the views

				_dtView = _viewDL.PlanView_Read(_userRIDList);

//Begin Track #4804 - JScott - View name being appended with (User) on save
				_dtView.Columns.Add(new DataColumn("DISPLAY_ID", typeof(string)));

//End Track #4804 - JScott - View name being appended with (User) on save
				// Begin TT#1125 - JSmith - Global/User should be consistent
//                foreach (DataRow row in _dtView.Rows)
//                {
//                    // Begin Issue 3806 - stodd
//                    if (Convert.ToInt32(row["USER_RID"], CultureInfo.CurrentUICulture) != Include.GlobalUserRID)
//                    {
////Begin Track #4804 - JScott - View name being appended with (User) on save
////						row["VIEW_ID"] = Convert.ToString(row["VIEW_ID"], CultureInfo.CurrentUICulture) + " (User)";
//                        row["DISPLAY_ID"] = Convert.ToString(row["VIEW_ID"], CultureInfo.CurrentUICulture) + " (User)";
////End Track #4804 - JScott - View name being appended with (User) on save
//                    }
////Begin Track #4804 - JScott - View name being appended with (User) on save
//                    else
//                    {
//                        row["DISPLAY_ID"] = Convert.ToString(row["VIEW_ID"], CultureInfo.CurrentUICulture);
//                    }
////End Track #4804 - JScott - View name being appended with (User) on save
//                    // Edn issue 3806
//                }
				foreach (DataRow row in _dtView.Rows)
				{
					userRID = Convert.ToInt32(row["USER_RID"], CultureInfo.CurrentUICulture);
					if (userRID != Include.GlobalUserRID)
					{
                        //Begin TT#827-MD -jsobek -Allocation Reviews Performance
						//row["DISPLAY_ID"] = Convert.ToString(row["VIEW_ID"], CultureInfo.CurrentUICulture) + " (" + secAdmin.GetUserName(userRID) + ")";
                        row["DISPLAY_ID"] = Convert.ToString(row["VIEW_ID"], CultureInfo.CurrentUICulture) + " (" + UserNameStorage.GetUserName(userRID) + ")";
                        //End TT#827-MD -jsobek -Allocation Reviews Performance
					}
					else
					{
						row["DISPLAY_ID"] = Convert.ToString(row["VIEW_ID"], CultureInfo.CurrentUICulture);
					}
				}
				// End TT#1125 

				// Load Filters

				filterUserSecurity = _sab.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ToolsFiltersStoreUser);
				filterGlobalSecurity = _sab.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ToolsFiltersStoreGlobal);

				_userRIDList = new ArrayList();

				_userRIDList.Add(-1);

//Begin Track #5197 - JScott - Deny Global View still allows user to use them
//				if (filterUserSecurity.AllowUpdate || filterUserSecurity.AllowView)
//				{
//					_userRIDList.Add(_sab.ClientServerSession.UserRID);
//				}
//
//				if (filterGlobalSecurity.AllowUpdate || filterGlobalSecurity.AllowView)
//				{
//					_userRIDList.Add(Include.GlobalUserRID);	// Issue 3806
//				}
				if (filterUserSecurity.AllowView)
				{
					_userRIDList.Add(_sab.ClientServerSession.UserRID);
				}

				if (filterGlobalSecurity.AllowView)
				{
					_userRIDList.Add(Include.GlobalUserRID);
				}
//End Track #5197 - JScott - Deny Global View still allows user to use them

				// Begin Track #4872 - JSmith - Global/User Attributes
				_singleChainSecurity = _sab.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ForecastSingleLevelChain);
				_multiChainSecurity = _sab.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ForecastMultiLevelChain);
				_singleStoreSecurity = _sab.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ForecastSingleLevelStore);
				_multiStoreSecurity = _sab.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ForecastMultiLevelStore);
				// End Track #4872

				//Setups, bindings, etc.
				//Begin Track #5858 - KJohnson - Validating store security only
				// Begin TT#44 - JSmith - Drag/Drop User Attributes or Filters in to Global Methods does not react consistantly
				//cboSSLStoreAttribute.Tag = new MIDStoreAttributeComboBoxTag(SAB, cboSSLStoreAttribute);
				//cboSMLStoreAttribute.Tag = new MIDStoreAttributeComboBoxTag(SAB, cboSMLStoreAttribute);
				cboSSLStoreAttribute.Tag = new MIDStoreAttributeComboBoxTag(SAB, cboSSLStoreAttribute, FunctionSecurity, true);
                cboSMLStoreAttribute.Tag = new MIDStoreAttributeComboBoxTag(SAB, cboSMLStoreAttribute, FunctionSecurity, true);
				// End TT#44
				//End Track #5858 - KJohnson

                AdjustTextWidthComboBox_DropDown(cboSSLStoreAttribute); //TT#7 - MD - RBeck _ Dynamic download
                AdjustTextWidthComboBox_DropDown(cboSMLStoreAttribute); //TT#7 - MD - RBeck _ Dynamic download

				BindFilterComboBox();
				BindGroupByComboBox();
				BindVersionComboBoxes();
				BindStoreAttrComboBoxes();
				BindViewComboBoxes();
				CreateComboLists();
//Begin Modification - JScott - Allow access to Computation Mode with Config Setting
//#if (DEBUG)
//				BindComputationModeComboBox();
//				cboComputationMode.SelectedIndex = cboComputationMode.Items.IndexOf(new ComputationModeCombo(_openParms.ComputationsMode));
//				if (cboComputationMode.SelectedIndex == -1)
//				{
//					cboComputationMode.SelectedIndex = 0;
//				}
//
//				lblComputationMode.Visible = true;
//				cboComputationMode.Visible = true;
//#else
//				lblComputationMode.Visible = false;
//				cboComputationMode.Visible = false;
//#endif
				MIDOnlyFunctionsStr = MIDConfigurationManager.AppSettings["MIDOnlyFunctions"];

				if (MIDOnlyFunctionsStr != null)
				{
					MIDOnlyFunctionsStr = MIDOnlyFunctionsStr.ToLower();

					if (MIDOnlyFunctionsStr == "true" || MIDOnlyFunctionsStr == "yes" || MIDOnlyFunctionsStr == "t" || MIDOnlyFunctionsStr == "y" || MIDOnlyFunctionsStr == "1")
					{
						_MIDOnlyFunctions = true;
					}
					else
					{
						_MIDOnlyFunctions = false;
					}
				}
				else
				{
					_MIDOnlyFunctions = false;
				}

				if (_MIDOnlyFunctions)
				{
					BindComputationModeComboBox();
                    cboComputationMode.SelectedIndex = cboComputationMode.Items.IndexOf(new ComputationModeCombo(_openParms.ComputationsMode));
                    if (cboComputationMode.SelectedIndex == -1)
					{
                        cboComputationMode.SelectedIndex = 0;
					}

					lblComputationMode.Visible = true;
                    cboComputationMode.Visible = true;
				}
				else
				{
					lblComputationMode.Visible = false;
                    cboComputationMode.Visible = false;
				}
//End Modification - JScott - Allow access to Computation Mode with Config Setting

				GetDataSource();
				grdBasis.DataSource = _dsBasis;
				//Begin Track #5864 - JScott - Expand Basis Selection when OTS Forecast Selection Screen Opens
				grdBasis.Rows.ExpandAll(false);

				//End Track #5864 - JScott - Expand Basis Selection when OTS Forecast Selection Screen Opens
				PopulateStoreSingleLevelPanel();
				PopulateStoreMultiLevelPanel();
				PopulateChainSingleLevelPanel();
				PopulateChainMultiLevelPanel();
				PopulateFormBasis();

                //BEGIN TT#6-MD-VStuart - Single Store Select
               // this.cboStoreNonMulti.SelectionChangeCommitted += new System.EventHandler(this.cboStoreNonMulti_SelectionChangeCommitted);// TT#7 - RBeck - Dynamic dropdowns
               // this.cboStore.SelectionChangeCommitted += new System.EventHandler(this.cboStore_SelectionChangeCommitted);   // TT#7 - RBeck - Dynamic dropdowns
                //END TT#6-MD-VStuart - Single Store Select

				// Begin Track #4872 - JSmith - Global/User Attributes
				//_singleChainSecurity = _sab.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ForecastSingleLevelChain);
				//_multiChainSecurity = _sab.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ForecastMultiLevelChain);
				//_singleStoreSecurity = _sab.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ForecastSingleLevelStore);
				//_multiStoreSecurity = _sab.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ForecastMultiLevelStore);
				// End Track #4872

				// Begin TT#44 - JSmith - Drag/Drop User Attributes or Filters in to Global Methods does not react consistantly
				//FunctionSecurity = new FunctionSecurityProfile((int)eSecurityFunctions.ForecastReview);
				//FunctionSecurity.SetAllowUpdate();
				// End TT#44
				SetReadOnly(FunctionSecurity.AllowUpdate);

				if (_singleChainSecurity.AccessDenied && _multiChainSecurity.AccessDenied)
				{
					radChain.Enabled = false;
                    radChainLadder.Enabled = false;
				}

				if (_singleStoreSecurity.AccessDenied && _multiStoreSecurity.AccessDenied)
				{
					radStore.Enabled = false;
				}

                //Begin TT#639-MD -agallagher - OTS Forecast Totals Right
                if (_openParms.IsTotRT)
                {
                    chkRT.Checked = true;
                }
                else
                {
                    chkRT.Checked = false;
                }
                //End TT#639-MD -agallagher - OTS Forecast Totals Right

                //Begin TT#609-MD -jsobek -OTS Forecast Chain Ladder View
                if (_openParms.IsMulti)
                {
                    this.chkMultiLevel.Checked = true;
                }
                else
                {
                    this.chkMultiLevel.Checked = false;
                }

                //Begin Track #3939 - JScott - Single chain/store being allowed with deny security
                //switch (_openParms.PlanSessionType)
                //{
                //    case ePlanSessionType.ChainSingleLevel:
                //    case ePlanSessionType.StoreSingleLevel:
                //        chkMultiLevel.Checked = false;
                //        break;
                //    case ePlanSessionType.ChainMultiLevel:
                //    case ePlanSessionType.StoreMultiLevel:
                //        chkMultiLevel.Checked = true;
                //        break;
                //}

                //End Track #3939 - JScott - Single chain/store being allowed with deny security
                //End TT#609-MD -jsobek -OTS Forecast Chain Ladder View


				switch (_openParms.PlanSessionType)
				{
					case ePlanSessionType.ChainSingleLevel:
					case ePlanSessionType.ChainMultiLevel:
						if (radChain.Enabled)
						{
                            //Begin TT#609-MD -jsobek -OTS Forecast Chain Ladder View
                            if (_openParms.IsLadder)
                            {
                                radChainLadder.Checked = true;
                            }
                            else
                            {
                                radChain.Checked = true;
                            }
                            //End TT#609-MD -jsobek -OTS Forecast Chain Ladder View
						}
						else
						{
							radChain.Checked = false;
                            radChainLadder.Checked = false; //TT#609-MD -jsobek -OTS Forecast Chain Ladder View
							if (radStore.Enabled)
							{
								radStore.Checked = true;
							}
							else
							{
								radStore.Checked = false;
							}
						}
						break;
					case ePlanSessionType.StoreSingleLevel:
					case ePlanSessionType.StoreMultiLevel:
						if (radStore.Enabled)
						{
							radStore.Checked = true;
						}
						else
						{
							radStore.Checked = false;
							if (radChain.Enabled)
							{
                                //Begin TT#609-MD -jsobek -OTS Forecast Chain Ladder View
                                if (_openParms.IsLadder)
                                {
                                    radChainLadder.Checked = true;
                                }
                                else
                                {
                                    radChain.Checked = true;
                                }
                                //End TT#609-MD -jsobek -OTS Forecast Chain Ladder View
							}
							else
							{
								radChain.Checked = false;
                                radChainLadder.Checked = false;  //TT#609-MD -jsobek -OTS Forecast Chain Ladder View
							}
						}
						break;
				}

//Begin Track #3939 - JScott - Single chain/store being allowed with deny security
//				switch (_openParms.PlanSessionType)
//				{
//					case ePlanSessionType.ChainSingleLevel:
//					case ePlanSessionType.StoreSingleLevel:
//						chkMultiLevel.Checked = false;
//						break;
//					case ePlanSessionType.ChainMultiLevel:
//					case ePlanSessionType.StoreMultiLevel:
//						chkMultiLevel.Checked = true;
//						break;
//				}
//
//End Track #3939 - JScott - Single chain/store being allowed with deny security

                //Begin TT#408 - MD - Filter Chain Multi level has drop down for filters -  RBeck
                lblCMLFilter.Visible = false;
                cboCMLFilter.Visible = false;
                lblCMLView.Location = lblCMLFilter.Location;
                cboCMLView.Location = cboCMLFilter.Location;
                //ndd TT#408 - MD - Filter Chain Multi level has drop down for filters -  RBeck
				FormLoaded = true;
				_OTSFormPopulated = true;
				ShowTypePanel();
			}
			catch (Exception exc)
			{
				HandleException(exc);
			}
		}

		private void OTSPlanSelection_Load(object sender, System.EventArgs e)
		{
			if (!_OTSFormPopulated)
			{
				PopulateForm();
			}
//			FunctionSecurityProfile filterUserSecurity;
//			FunctionSecurityProfile filterGlobalSecurity;
//
//			try
//			{
//				FormLoaded = false;
//				cboSSLFilter.Tag = "IgnoreMouseWheel";
//
//				//setup images to be used in the grid.
//
//				_picInclude = new Bitmap(GraphicsDirectory + "\\include.gif");
//				_picExclude = new Bitmap(GraphicsDirectory + "\\exclude.gif");
//				_dynamicToPlanImage = Image.FromFile(GraphicsDirectory + "\\" + MIDGraphics.DynamicToPlanImage);
//				_dynamicToCurrentImage = Image.FromFile(GraphicsDirectory + "\\" + MIDGraphics.DynamicToCurrentImage);
//
//				//Populate the form.
//
//				// Get Versions and split into Store and Chain versions
//
//				_versionProfList = _sab.ClientServerSession.GetUserForecastVersions();
//				_chainList = new ArrayList();
//				_storeList = new ArrayList();
//				_chainLowLevelVersionList = new ProfileList(eProfileType.Version);
//				_storeLowLevelVersionList = new ProfileList(eProfileType.Version);
//
//				foreach (VersionProfile versionProfile in _versionProfList.ArrayList)
//				{
//					if (!versionProfile.ChainSecurity.AccessDenied)
//					{
//						_chainList.Add(versionProfile);
//						_chainLowLevelVersionList.Add(versionProfile);
//					}
//						
//					if (!versionProfile.StoreSecurity.AccessDenied)
//					{
//						_storeList.Add(versionProfile);
//						_storeLowLevelVersionList.Add(versionProfile);
//					}
//				}
//
//				LoadSelectionData(_planSelectDL.GetPlanSelectionMainInfo(_sab.ClientServerSession.UserRID));
//
//				if (_sab.ClientServerSession.GlobalOptions.AppConfig.PlanningInstalled)
//				{
//					grpType.Visible = true;
//				}
//				else
//				{
//					grpType.Visible = false;
//				}
//
//				_userRIDList = new ArrayList();
//				_userRIDList.Add(Include.GlobalUserRID);  // issue 3806
//				_userRIDList.Add(_sab.ClientServerSession.UserRID);
//
//				// Load the views
//
//				_dtView = _viewDL.PlanView_Read(_userRIDList);
//
//				foreach (DataRow row in _dtView.Rows)
//				{
//					// Begin Issue 3806 - stodd
//					if (Convert.ToInt32(row["USER_RID"], CultureInfo.CurrentUICulture) != Include.GlobalUserRID)
//					{
//						row["VIEW_ID"] = Convert.ToString(row["VIEW_ID"], CultureInfo.CurrentUICulture) + " (User)";
//					}
//					// Edn issue 3806
//				}
//
//				// Load Filters
//
//				filterUserSecurity = _sab.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ToolsFiltersUser);
//				filterGlobalSecurity = _sab.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ToolsFiltersGlobal);
//
//				_userRIDList = new ArrayList();
//
//				_userRIDList.Add(-1);
//
//				if (filterUserSecurity.AllowUpdate || filterUserSecurity.AllowView)
//				{
//					_userRIDList.Add(_sab.ClientServerSession.UserRID);
//				}
//
//				if (filterGlobalSecurity.AllowUpdate || filterGlobalSecurity.AllowView)
//				{
//					_userRIDList.Add(Include.GlobalUserRID);	// Issue 3806
//				}
//			
//				//Setups, bindings, etc.
//
//				BindFilterComboBox();
//				BindGroupByComboBox();
//				BindVersionComboBoxes();
//				BindStoreAttrComboBoxes();
//				BindViewComboBoxes();
//				CreateComboLists();
//#if (DEBUG)
//				BindComputationModeComboBox();
//				cboComputationMode.SelectedIndex = cboComputationMode.Items.IndexOf(new ComputationModeCombo(_openParms.ComputationsMode));
//				if (cboComputationMode.SelectedIndex == -1)
//				{
//					cboComputationMode.SelectedIndex = 0;
//				}
//
//				lblComputationMode.Visible = true;
//				cboComputationMode.Visible = true;
//#else
//				lblComputationMode.Visible = false;
//				cboComputationMode.Visible = false;
//#endif
//
//				GetDataSource();
//				grdBasis.DataSource = _dsBasis;
//				PopulateStoreSingleLevelPanel();
//				PopulateStoreMultiLevelPanel();
//				PopulateChainSingleLevelPanel();
//				PopulateChainMultiLevelPanel();
//				PopulateFormBasis();
//				
//				_singleChainSecurity = _sab.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ForecastSingleLevelChain);
//				_multiChainSecurity = _sab.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ForecastMultiLevelChain);
//				_singleStoreSecurity = _sab.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ForecastSingleLevelStore);
//				_multiStoreSecurity = _sab.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ForecastMultiLevelStore);
//
//				FunctionSecurity = new FunctionSecurityProfile((int)eSecurityFunctions.ForecastReview);
//				FunctionSecurity.SetAllowUpdate();
//				SetReadOnly(FunctionSecurity.AllowUpdate);
//
//				if (_singleChainSecurity.AccessDenied && _multiChainSecurity.AccessDenied)
//				{
//					radChain.Enabled = false;
//				}
//
//				if (_singleStoreSecurity.AccessDenied && _multiStoreSecurity.AccessDenied)
//				{
//					radStore.Enabled = false;
//				}
//
////Begin Track #3939 - JScott - Single chain/store being allowed with deny security
//				switch (_openParms.PlanSessionType)
//				{
//					case ePlanSessionType.ChainSingleLevel:
//					case ePlanSessionType.StoreSingleLevel:
//						chkMultiLevel.Checked = false;
//						break;
//					case ePlanSessionType.ChainMultiLevel:
//					case ePlanSessionType.StoreMultiLevel:
//						chkMultiLevel.Checked = true;
//						break;
//				}
//
////End Track #3939 - JScott - Single chain/store being allowed with deny security
//				switch (_openParms.PlanSessionType)
//				{
//					case ePlanSessionType.ChainSingleLevel:
//					case ePlanSessionType.ChainMultiLevel:
//						if (radChain.Enabled)
//						{
//							radChain.Checked = true;
//						}
//						else
//						{
//							radChain.Checked = false;
//							if (radStore.Enabled)
//							{
//								radStore.Checked = true;
//							}
//							else
//							{
//								radStore.Checked = false;
//							}
//						}
//						break;
//					case ePlanSessionType.StoreSingleLevel:
//					case ePlanSessionType.StoreMultiLevel:
//						if (radStore.Enabled)
//						{
//							radStore.Checked = true;
//						}
//						else
//						{
//							radStore.Checked = false;
//							if (radChain.Enabled)
//							{
//								radChain.Checked = true;
//							}
//							else
//							{
//								radChain.Checked = false;
//							}
//						}
//						break;
//				}
//
////Begin Track #3939 - JScott - Single chain/store being allowed with deny security
////				switch (_openParms.PlanSessionType)
////				{
////					case ePlanSessionType.ChainSingleLevel:
////					case ePlanSessionType.StoreSingleLevel:
////						chkMultiLevel.Checked = false;
////						break;
////					case ePlanSessionType.ChainMultiLevel:
////					case ePlanSessionType.StoreMultiLevel:
////						chkMultiLevel.Checked = true;
////						break;
////				}
////
////End Track #3939 - JScott - Single chain/store being allowed with deny security
//				FormLoaded = true;
//				ShowTypePanel();
//			}
//			catch (Exception exc)
//			{
//				HandleException(exc);
//			}
		}

		private void OTSPlanSelection_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			try
			{
				grdBasis.PerformAction(UltraGridAction.ExitEditMode);
			}
			catch (Exception exc)
			{
				HandleException(exc);
			}
		}

		private void OTSPlanSelection_Closed(object sender, System.EventArgs e)
		{
			try
			{
				// Begin TT#856 - JSmith - Out of memory
				((MIDMerchandiseTextBoxTag)txtSSLStoreNode.Tag).Dispose();
				((MIDMerchandiseTextBoxTag)txtSSLChainNode.Tag).Dispose();
				((MIDMerchandiseTextBoxTag)txtSMLHighLevelNode.Tag).Dispose();
				((MIDMerchandiseTextBoxTag)txtCSLChainNode.Tag).Dispose();
				((MIDMerchandiseTextBoxTag)txtCMLHighLevelNode.Tag).Dispose();

                ((MIDStoreFilterComboBoxTag)cboSSLFilter.Tag).Dispose();
                ((MIDStoreFilterComboBoxTag)cboSMLFilter.Tag).Dispose();
                ((MIDStoreFilterComboBoxTag)cboCMLFilter.Tag).Dispose();

				((MIDStoreAttributeComboBoxTag)cboSSLStoreAttribute.Tag).Dispose();
                ((MIDStoreAttributeComboBoxTag)cboSMLStoreAttribute.Tag).Dispose();

				txtSSLStoreNode.Tag = null;
				txtSSLChainNode.Tag = null;
				txtSMLHighLevelNode.Tag = null;
				txtCSLChainNode.Tag = null;
				txtCMLHighLevelNode.Tag = null;
                cboSSLFilter.Tag = null;
                cboSMLFilter.Tag = null;
                cboCMLFilter.Tag = null;
				cboSSLStoreAttribute.Tag = null;
                cboSMLStoreAttribute.Tag = null;
				// End TT#856

//BEGIN TT#7 - RBeck - Dynamic dropdowns
                this.cboSMLFilter.SelectionChangeCommitted -= new System.EventHandler(this.cboSSLFilter_SelectionChangeCommitted);
                this.cboStore.SelectionChangeCommitted -= new System.EventHandler(this.cboStore_SelectionChangeCommitted);
                this.cboSSLFilter.SelectionChangeCommitted -= new System.EventHandler(this.cboSSLFilter_SelectionChangeCommitted);
                this.cboStoreNonMulti.SelectionChangeCommitted -= new System.EventHandler(this.cboStoreNonMulti_SelectionChangeCommitted); 
//END TT#7 - RBeck - Dynamic dropdowns
				this.mnuDelete.Click -= new System.EventHandler(this.mnuDelete_Click);
				this.btnCancel.Click -= new System.EventHandler(this.btnCancel_Click);
				this.btnOK.Click -= new System.EventHandler(this.btnOK_Click);
				this.chkMultiLevel.CheckedChanged -= new System.EventHandler(this.chkMultiLevel_CheckedChanged);
				this.radChain.CheckedChanged -= new System.EventHandler(this.radChain_CheckedChanged);
                this.radChainLadder.CheckedChanged -= new System.EventHandler(this.radChainLadder_CheckedChanged);
				this.radStore.CheckedChanged -= new System.EventHandler(this.radStore_CheckedChanged);
				this.grdBasis.DragDrop -= new System.Windows.Forms.DragEventHandler(this.grdBasis_DragDrop);
				this.grdBasis.DragOver -= new System.Windows.Forms.DragEventHandler(this.grdBasis_DragOver);
				this.grdBasis.BeforeRowsDeleted -= new Infragistics.Win.UltraWinGrid.BeforeRowsDeletedEventHandler(this.grdBasis_BeforeRowsDeleted);
				this.grdBasis.BeforeRowInsert -= new Infragistics.Win.UltraWinGrid.BeforeRowInsertEventHandler(this.grdBasis_BeforeRowInsert);
				this.grdBasis.AfterCellUpdate -= new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdBasis_AfterCellUpdate);
				this.grdBasis.InitializeLayout -= new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.grdBasis_InitializeLayout);
				//Begin TT#169 - JSmith - enter max of 1000 and receive 'must be integer'
				MIDRetail.Windows.Controls.UltraGridLayoutDefaults ugld = new MIDRetail.Windows.Controls.UltraGridLayoutDefaults(ErrorImage);
				ugld.DetachGridEventHandlers(grdBasis);
				//End TT#169
				this.grdBasis.AfterSelectChange -= new Infragistics.Win.UltraWinGrid.AfterSelectChangeEventHandler(this.grdBasis_AfterSelectChange);
//Begin Track #5081 - JScott - Paste into Chain does not work
				this.grdBasis.BeforeCellUpdate -= new Infragistics.Win.UltraWinGrid.BeforeCellUpdateEventHandler(this.grdBasis_BeforeCellUpdate);
//End Track #5081 - JScott - Paste into Chain does not work
				this.grdBasis.AfterRowInsert -= new Infragistics.Win.UltraWinGrid.RowEventHandler(this.grdBasis_AfterRowInsert);
				this.grdBasis.InitializeRow -= new Infragistics.Win.UltraWinGrid.InitializeRowEventHandler(this.grdBasis_InitializeRow);
				this.grdBasis.ClickCellButton -= new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdBasis_ClickCellButton);
				this.grdBasis.AfterRowsDeleted -= new System.EventHandler(this.grdBasis_AfterRowsDeleted);
				this.grdBasis.MouseUp -= new System.Windows.Forms.MouseEventHandler(this.grdBasis_MouseUp);
				// Begin TT#856 - JSmith - Out of memory
				this.grdBasis.DragEnter -= new System.Windows.Forms.DragEventHandler(this.grdBasis_DragEnter);
				// End TT#856
				this.chkSSLSimilarStores.CheckedChanged -= new System.EventHandler(this.chkSimilarStores_CheckedChanged);
				this.chkSSLIneligibleStores.CheckedChanged -= new System.EventHandler(this.chkIneligibleStores_CheckedChanged);
                this.cboSSLView.SelectionChangeCommitted -= new System.EventHandler(this.cboView_SelectionChangeCommitted);
				this.cboSSLFilter.DropDown -= new System.EventHandler(this.cboFilter_DropDown);
                this.cboSSLFilter.DragDrop -= new System.Windows.Forms.DragEventHandler(this.cboFilter_DragDrop);
                this.cboSSLFilter.SelectionChangeCommitted -= new System.EventHandler(this.cboFilter_SelectionChangeCommitted);
				this.cboSSLFilter.DragEnter -= new System.Windows.Forms.DragEventHandler(this.cboFilter_DragEnter);
				this.cboSSLFilter.DragOver -= new System.Windows.Forms.DragEventHandler(this.cboFilter_DragOver);
                this.cboSSLGroupBy.SelectionChangeCommitted -= new System.EventHandler(this.cboGroupBy_SelectionChangeCommitted);
				this.cboSSLStoreAttribute.SelectionChangeCommitted -= new System.EventHandler(this.cboStoreAttribute_SelectionChangeCommitted);
				//this.cboSSLStoreAttribute.DragDrop -= new System.Windows.Forms.DragEventHandler(this.cboStoreAttribute_DragDrop);
				this.cboSSLStoreAttribute.DragEnter -= new System.Windows.Forms.DragEventHandler(this.cboStoreAttribute_DragEnter);
				// Begin TT#856 - JSmith - Out of memory
				this.cboSSLStoreAttribute.DragOver -= new System.Windows.Forms.DragEventHandler(this.cboStoreAttribute_DragOver);
				// End TT#856
				this.mdsSSLPlanDateRange.Click -= new System.EventHandler(this.mdsSSLPlanDateRange_Click);
				this.mdsSSLPlanDateRange.OnSelection -= new MIDRetail.Windows.Controls.MIDDateRangeSelector.SelectionEventHandler(this.mdsPlanDateRange_OnSelection);
                this.cboSSLStoreVersion.SelectionChangeCommitted -= new System.EventHandler(this.cboVersion_SelectionChangeCommitted);
                this.cboSSLChainVersion.SelectionChangeCommitted -= new System.EventHandler(this.cboVersion_SelectionChangeCommitted);
//Begin Track #5081 - JScott - Paste into Chain does not work
//				this.txtSSLChainNode.DragDrop -= new System.Windows.Forms.DragEventHandler(this.txtNode_DragDrop);
//				this.txtSSLChainNode.DragEnter -= new System.Windows.Forms.DragEventHandler(this.txtNode_DragEnter);
//				this.txtSSLChainNode.Validating -= new System.ComponentModel.CancelEventHandler(this.txtNode_Validating);
				this.txtSSLChainNode.Validating -= new System.ComponentModel.CancelEventHandler(this.txtNode_Validating);
				this.txtSSLChainNode.Validated -= new System.EventHandler(this.txtNode_Validated);
				this.txtSSLChainNode.DragDrop -= new System.Windows.Forms.DragEventHandler(this.txtNode_DragDrop);
				this.txtSSLChainNode.TextChanged -= new System.EventHandler(this.txtNode_TextChanged);
				this.txtSSLChainNode.DragEnter -= new System.Windows.Forms.DragEventHandler(this.txtNode_DragEnter);
				// Begin TT#856 - JSmith - Out of memory
				this.txtSSLChainNode.DragOver -= new System.Windows.Forms.DragEventHandler(this.txtNode_DragOver);
				// End TT#856
				this.txtSSLStoreNode.Validating -= new System.ComponentModel.CancelEventHandler(this.txtNode_Validating);
				this.txtSSLStoreNode.Validated -= new System.EventHandler(this.txtNode_Validated);
//End Track #5081 - JScott - Paste into Chain does not work
				this.txtSSLStoreNode.DragDrop -= new System.Windows.Forms.DragEventHandler(this.txtNode_DragDrop);
				this.txtSSLStoreNode.TextChanged -= new System.EventHandler(this.txtSSLStoreNode_TextChanged);
				this.txtSSLStoreNode.DragEnter -= new System.Windows.Forms.DragEventHandler(this.txtNode_DragEnter);
				// Begin TT#856 - JSmith - Out of memory
				this.txtSSLStoreNode.DragOver -= new System.Windows.Forms.DragEventHandler(this.txtNode_DragOver);
				// End TT#856
//Begin Track #5081 - JScott - Paste into Chain does not work
//				this.txtSSLStoreNode.Validating -= new System.ComponentModel.CancelEventHandler(this.txtNode_Validating);
//End Track #5081 - JScott - Paste into Chain does not work
				// Begin TT#856 - JSmith - Out of memory
                this.cboCMLOverride.SelectionChangeCommitted -= new System.EventHandler(this.cboCMLOverride_SelectionChangeCommitted);
				// End TT#856
				this.btnCMLVersionOverride.Click -= new System.EventHandler(this.btnCMLVersionOverride_Click);
                this.cboCMLView.SelectionChangeCommitted -= new System.EventHandler(this.cboView_SelectionChangeCommitted);
				this.cboCMLFilter.DropDown -= new System.EventHandler(this.cboFilter_DropDown);
                this.cboCMLFilter.DragDrop -= new System.Windows.Forms.DragEventHandler(this.cboFilter_DragDrop);
                this.cboCMLFilter.SelectionChangeCommitted -= new System.EventHandler(this.cboFilter_SelectionChangeCommitted);
                this.cboCMLFilter.DragEnter -= new System.Windows.Forms.DragEventHandler(this.cboFilter_DragEnter);
				// Begin TT#856 - JSmith - Out of memory
				// this.cboCMLFilter.DragOver += new System.Windows.Forms.DragEventHandler(this.cboFilter_DragOver);
				this.cboCMLFilter.DragOver -= new System.Windows.Forms.DragEventHandler(this.cboFilter_DragOver);
				// End TT#856
                this.cboCMLGroupBy.SelectionChangeCommitted -= new System.EventHandler(this.cboGroupBy_SelectionChangeCommitted);
                this.cboCMLLowLevels.SelectionChangeCommitted -= new System.EventHandler(this.cboCMLLowLevels_SelectionChangeCommitted);
				this.mdsCMLPlanDateRange.Click -= new System.EventHandler(this.mdsCMLPlanDateRange_Click);
				this.mdsCMLPlanDateRange.OnSelection -= new MIDRetail.Windows.Controls.MIDDateRangeSelector.SelectionEventHandler(this.mdsPlanDateRange_OnSelection);
                this.cboCMLHighLevelVersion.SelectionChangeCommitted -= new System.EventHandler(this.cboVersion_SelectionChangeCommitted);
                this.cboCMLLowLevelsVersion.SelectionChangeCommitted -= new System.EventHandler(this.cboVersion_SelectionChangeCommitted);
//Begin Track #5081 - JScott - Paste into Chain does not work
//				this.txtCMLHighLevelNode.DragDrop -= new System.Windows.Forms.DragEventHandler(this.txtNode_DragDrop);
//				this.txtCMLHighLevelNode.DragEnter -= new System.Windows.Forms.DragEventHandler(this.txtNode_DragEnter);
//				this.txtCMLHighLevelNode.Validating -= new System.ComponentModel.CancelEventHandler(this.txtNode_Validating);
				this.txtCMLHighLevelNode.Validating -= new System.ComponentModel.CancelEventHandler(this.txtNode_Validating);
				this.txtCMLHighLevelNode.Validated -= new System.EventHandler(this.txtNode_Validated);
				this.txtCMLHighLevelNode.DragDrop -= new System.Windows.Forms.DragEventHandler(this.txtNode_DragDrop);
				this.txtCMLHighLevelNode.TextChanged -= new System.EventHandler(this.txtNode_TextChanged);
				this.txtCMLHighLevelNode.DragEnter -= new System.Windows.Forms.DragEventHandler(this.txtNode_DragEnter);
//End Track #5081 - JScott - Paste into Chain does not work
				// Begin TT#856 - JSmith - Out of memoryevelNode.DragEnter -= new System.Windows.Forms.DragEventHandler(this.txtNode_DragEnter);
				this.txtCMLHighLevelNode.DragOver -= new System.Windows.Forms.DragEventHandler(this.txtNode_DragOver);
                this.cboSMLOverride.SelectionChangeCommitted -= new System.EventHandler(this.cboSMLOverride_SelectionChangeCommitted);
				// End TT#856
				this.btnSMLVersionOverride.Click -= new System.EventHandler(this.btnSMLVersionOverride_Click);
				this.chkSMLSimilarStores.CheckedChanged -= new System.EventHandler(this.chkSimilarStores_CheckedChanged);
				this.chkSMLIneligibleStores.CheckedChanged -= new System.EventHandler(this.chkIneligibleStores_CheckedChanged);
                this.cboSMLView.SelectionChangeCommitted -= new System.EventHandler(this.cboView_SelectionChangeCommitted);
				this.cboSMLFilter.DropDown -= new System.EventHandler(this.cboFilter_DropDown);
                this.cboSMLFilter.DragDrop -= new System.Windows.Forms.DragEventHandler(this.cboFilter_DragDrop);
                this.cboSMLFilter.SelectionChangeCommitted -= new System.EventHandler(this.cboFilter_SelectionChangeCommitted);
                this.cboSMLFilter.DragEnter -= new System.Windows.Forms.DragEventHandler(this.cboFilter_DragEnter);
				// Begin TT#856 - JSmith - Out of memory
				// this.cboSMLFilter.DragOver += new System.Windows.Forms.DragEventHandler(this.cboFilter_DragOver);
				this.cboSMLFilter.DragOver -= new System.Windows.Forms.DragEventHandler(this.cboFilter_DragOver);
				// End TT#856
                this.cboSMLGroupBy.SelectionChangeCommitted -= new System.EventHandler(this.cboGroupBy_SelectionChangeCommitted);
				this.cboSMLStoreAttribute.SelectionChangeCommitted -= new System.EventHandler(this.cboStoreAttribute_SelectionChangeCommitted);
				this.cboSMLStoreAttribute.DragDrop -= new System.Windows.Forms.DragEventHandler(this.cboStoreAttribute_DragDrop);
				this.cboSMLStoreAttribute.DragEnter -= new System.Windows.Forms.DragEventHandler(this.cboStoreAttribute_DragEnter);
				// Begin TT#856 - JSmith - Out of memory
				this.cboSMLStoreAttribute.DragOver -= new System.Windows.Forms.DragEventHandler(this.cboStoreAttribute_DragOver);
				// End TT#856
                this.cboSMLLowLevels.SelectionChangeCommitted -= new System.EventHandler(this.cboSMLLowLevels_SelectionChangeCommitted);
				this.mdsSMLPlanDateRange.Click -= new System.EventHandler(this.mdsSMLPlanDateRange_Click);
				this.mdsSMLPlanDateRange.OnSelection -= new MIDRetail.Windows.Controls.MIDDateRangeSelector.SelectionEventHandler(this.mdsPlanDateRange_OnSelection);
                this.cboSMLHighLevelVersion.SelectionChangeCommitted -= new System.EventHandler(this.cboVersion_SelectionChangeCommitted);
                this.cboSMLLowLevelsVersion.SelectionChangeCommitted -= new System.EventHandler(this.cboVersion_SelectionChangeCommitted);
//Begin Track #5081 - JScott - Paste into Chain does not work
//				this.txtSMLHighLevelNode.DragDrop -= new System.Windows.Forms.DragEventHandler(this.txtNode_DragDrop);
//				this.txtSMLHighLevelNode.DragEnter -= new System.Windows.Forms.DragEventHandler(this.txtNode_DragEnter);
//				this.txtSMLHighLevelNode.Validating -=new CancelEventHandler(txtNode_Validating);
				this.txtSMLHighLevelNode.Validating -= new System.ComponentModel.CancelEventHandler(this.txtNode_Validating);
				this.txtSMLHighLevelNode.Validated -= new System.EventHandler(this.txtNode_Validated);
				this.txtSMLHighLevelNode.DragDrop -= new System.Windows.Forms.DragEventHandler(this.txtNode_DragDrop);
				this.txtSMLHighLevelNode.TextChanged -= new System.EventHandler(this.txtNode_TextChanged);
				this.txtSMLHighLevelNode.DragEnter -= new System.Windows.Forms.DragEventHandler(this.txtNode_DragEnter);
				// Begin TT#856 - JSmith - Out of memory
				this.txtSMLHighLevelNode.DragOver -= new System.Windows.Forms.DragEventHandler(this.txtNode_DragOver);
				// End TT#856
//End Track #5081 - JScott - Paste into Chain does not work
                this.cboCSLView.SelectionChangeCommitted -= new System.EventHandler(this.cboView_SelectionChangeCommitted);
				this.mdsCSLPlanDateRange.Click -= new System.EventHandler(this.mdsCSLPlanDateRange_Click);
				this.mdsCSLPlanDateRange.OnSelection -= new MIDRetail.Windows.Controls.MIDDateRangeSelector.SelectionEventHandler(this.mdsPlanDateRange_OnSelection);
                this.cboCSLChainVersion.SelectionChangeCommitted -= new System.EventHandler(this.cboVersion_SelectionChangeCommitted);
				this.txtCSLChainNode.Validating -= new System.ComponentModel.CancelEventHandler(this.txtNode_Validating);
//Begin Track #5081 - JScott - Paste into Chain does not work
				this.txtCSLChainNode.Validated -= new System.EventHandler(this.txtNode_Validated);
//End Track #5081 - JScott - Paste into Chain does not work
				this.txtCSLChainNode.DragDrop -= new System.Windows.Forms.DragEventHandler(this.txtNode_DragDrop);
//Begin Track #5081 - JScott - Paste into Chain does not work
				this.txtCSLChainNode.TextChanged -= new System.EventHandler(this.txtNode_TextChanged);
//End Track #5081 - JScott - Paste into Chain does not work
				this.txtCSLChainNode.DragEnter -= new System.Windows.Forms.DragEventHandler(this.txtNode_DragEnter);
				// Begin TT#856 - JSmith - Out of memory
				this.txtCSLChainNode.DragOver -= new System.Windows.Forms.DragEventHandler(this.txtNode_DragOver);
				// End TT#856

                this.cboStoreNonMulti.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler (cboStoreNonMulti_MIDComboBoxPropertiesChangedEvent);
                this.cboSSLView.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler (cboSSLView_MIDComboBoxPropertiesChangedEvent);
                this.cboSSLFilter.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler (cboSSLFilter_MIDComboBoxPropertiesChangedEvent);
                this.cboSSLGroupBy.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler (cboSSLGroupBy_MIDComboBoxPropertiesChangedEvent);
                this.cboSSLStoreAttribute.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler (cboSSLStoreAttribute_MIDComboBoxPropertiesChangedEvent);
                this.cboSSLChainVersion.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler (cboSSLChainVersion_MIDComboBoxPropertiesChangedEvent);
                this.cboSSLStoreVersion.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler (cboSSLStoreVersion_MIDComboBoxPropertiesChangedEvent);
                this.cboCMLOverride.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler (cboCMLOverride_MIDComboBoxPropertiesChangedEvent);
                this.cboCMLView.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler (cboCMLView_MIDComboBoxPropertiesChangedEvent);
                this.cboCMLFilter.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler (cboCMLFilter_MIDComboBoxPropertiesChangedEvent);
                this.cboCMLGroupBy.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler (cboCMLGroupBy_MIDComboBoxPropertiesChangedEvent);
                this.cboCMLLowLevels.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler (cboCMLLowLevels_MIDComboBoxPropertiesChangedEvent);
                this.cboCMLLowLevelsVersion.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler (cboCMLLowLevelsVersion_MIDComboBoxPropertiesChangedEvent);
                this.cboCMLHighLevelVersion.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler (cboCMLHighLevelVersion_MIDComboBoxPropertiesChangedEvent);
                this.cboSMLOverride.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler (cboSMLOverride_MIDComboBoxPropertiesChangedEvent);
                this.cboStore.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler (cboStore_MIDComboBoxPropertiesChangedEvent);
                this.cboSMLView.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler (cboSMLView_MIDComboBoxPropertiesChangedEvent);
                this.cboSMLFilter.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler (cboSMLFilter_MIDComboBoxPropertiesChangedEvent);
                this.cboSMLGroupBy.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler (cboSMLGroupBy_MIDComboBoxPropertiesChangedEvent);
                this.cboSMLStoreAttribute.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler (cboSMLStoreAttribute_MIDComboBoxPropertiesChangedEvent);
                this.cboSMLLowLevels.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler (cboSMLLowLevels_MIDComboBoxPropertiesChangedEvent);
                this.cboSMLLowLevelsVersion.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler (cboSMLLowLevelsVersion_MIDComboBoxPropertiesChangedEvent);
                this.cboSMLHighLevelVersion.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler (cboSMLHighLevelVersion_MIDComboBoxPropertiesChangedEvent);
                this.cboCSLView.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler (cboCSLView_MIDComboBoxPropertiesChangedEvent);
                this.cboCSLChainVersion.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler (cboCSLChainVersion_MIDComboBoxPropertiesChangedEvent);

				this.Closing -= new System.ComponentModel.CancelEventHandler(this.OTSPlanSelection_Closing);
				this.Load -= new System.EventHandler(this.OTSPlanSelection_Load);
				this.Closed -= new System.EventHandler(this.OTSPlanSelection_Closed);
			}
			catch (Exception exc)
			{
				HandleException(exc);
			}
		}

		private void LoadSelectionData(DataTable aDTSelection)
		{
			try
			{

				if (aDTSelection.Rows.Count == 0)
				{
					_openParms = new PlanOpenParms(ePlanSessionType.StoreSingleLevel, _sab.ApplicationServerSession.GetDefaultComputations());
					_openParms.StoreGroupRID = _sab.ClientServerSession.GlobalOptions.OTSPlanStoreGroupRID;
					_openParms.GroupBy = eStorePlanSelectedGroupBy.ByTimePeriod;
					_openParms.ViewRID = Include.DefaultPlanViewRID;
					_openParms.DisplayTimeBy = eDisplayTimeBy.ByWeek;
					_openParms.IneligibleStores = false;
					_openParms.SimilarStores = true;
					_openParms.LowLevelsType = eLowLevelsType.None;
					// BEGIN Issue 5807 stodd
					//_openParms.ChainHLPlanProfile.VersionProfile = (VersionProfile)_versionProfList.FindKey(Include.FV_ActionRID);
					// Begin Track #5897 - JSmith - New user gets index error
					//_openParms.ChainHLPlanProfile.VersionProfile = (VersionProfile)_chainPlanVersionProfList[0];
					////_openParms.StoreHLPlanProfile.VersionProfile = (VersionProfile)_versionProfList.FindKey(Include.FV_ActionRID);
					//_openParms.StoreHLPlanProfile.VersionProfile = (VersionProfile)_storePlanVersionProfList[0];
					////_openParms.LowLevelVersionDefault = (VersionProfile)_versionProfList.FindKey(Include.FV_ActionRID);
					//_openParms.LowLevelVersionDefault = (VersionProfile)_versionProfList[0];
					if (_chainPlanVersionProfList.Count > 0)
					{
						_openParms.ChainHLPlanProfile.VersionProfile = (VersionProfile)_chainPlanVersionProfList[0];
					}
					else
					{
						_openParms.ChainHLPlanProfile.VersionProfile = new VersionProfile(Include.NoRID);
					}
					//_openParms.StoreHLPlanProfile.VersionProfile = (VersionProfile)_versionProfList.FindKey(Include.FV_ActionRID);
					if (_storePlanVersionProfList.Count > 0)
					{
						_openParms.StoreHLPlanProfile.VersionProfile = (VersionProfile)_storePlanVersionProfList[0];
					}
					else
					{
						_openParms.StoreHLPlanProfile.VersionProfile = new VersionProfile(Include.NoRID);
					}
					//_openParms.LowLevelVersionDefault = (VersionProfile)_versionProfList.FindKey(Include.FV_ActionRID);
					if (_versionProfList.Count > 0)
					{
						_openParms.LowLevelVersionDefault = (VersionProfile)_versionProfList[0];
					}
					else
					{
						_openParms.LowLevelVersionDefault = new VersionProfile(Include.NoRID);
					}
					// Begin Track #5897
					// END Issue 5807
					if (_passedNodeID > 0)
					{
						//Begin Track #5378 - color and size not qualified
//						_openParms.StoreHLPlanProfile.NodeProfile = _sab.HierarchyServerSession.GetNodeData(_passedNodeID);
//						_openParms.ChainHLPlanProfile.NodeProfile = _sab.HierarchyServerSession.GetNodeData(_passedNodeID);
						_openParms.StoreHLPlanProfile.NodeProfile = _sab.HierarchyServerSession.GetNodeData(_passedNodeID, true, true);
						_openParms.ChainHLPlanProfile.NodeProfile = _sab.HierarchyServerSession.GetNodeData(_passedNodeID, true, true);
						//End Track #5378
					}
					else
					{
						_openParms.StoreHLPlanProfile.NodeProfile = new HierarchyNodeProfile(Include.NoRID);
						_openParms.ChainHLPlanProfile.NodeProfile = new HierarchyNodeProfile(Include.NoRID);
					}
					// BEGIN Override Low Level Changes
					_openParms.OverrideLowLevelRid = Include.NoRID;
					_openParms.CustomOverrideLowLevelRid = Include.NoRID;
					// END Override Low Level Changes
                    _openParms.IsLadder = false; //TT#609-MD -jsobek -OTS Forecast Chain Ladder View
                    _openParms.IsMulti = false; //TT#609-MD -jsobek -OTS Forecast Chain Ladder View
                    _openParms.IsTotRT = false; // TT#639-MD -agallagher - OTS Forecast Totals Right
				}
				else
				{
					string computationMode;
					if (aDTSelection.Rows[0]["CALC_MODE"] == System.DBNull.Value)
					{
						computationMode = _sab.ApplicationServerSession.GetDefaultComputations();
					}
					else
					{
						computationMode = Convert.ToString(aDTSelection.Rows[0]["CALC_MODE"], CultureInfo.CurrentUICulture);
					}
					if (aDTSelection.Rows[0]["SESSION_TYPE"] == System.DBNull.Value ||
						!_sab.ClientServerSession.GlobalOptions.AppConfig.PlanningInstalled)
					{
						_openParms = new PlanOpenParms(ePlanSessionType.StoreSingleLevel, computationMode);
					}
					else
					{
						_openParms = new PlanOpenParms((ePlanSessionType)Convert.ToInt32(aDTSelection.Rows[0]["SESSION_TYPE"], CultureInfo.CurrentUICulture), computationMode);
					}
					if (aDTSelection.Rows[0]["SG_RID"] == System.DBNull.Value)
					{
						_openParms.StoreGroupRID = _sab.ClientServerSession.GlobalOptions.OTSPlanStoreGroupRID;
					}
					else
					{
						_openParms.StoreGroupRID = Convert.ToInt32(aDTSelection.Rows[0]["SG_RID"], CultureInfo.CurrentUICulture);
					}
					_openParms.FilterRID = Convert.ToInt32(aDTSelection.Rows[0]["FILTER_RID"], CultureInfo.CurrentUICulture);
					_openParms.GroupBy = (eStorePlanSelectedGroupBy)TypeDescriptor.GetConverter(_openParms.GroupBy).ConvertFrom(aDTSelection.Rows[0]["GROUP_BY_ID"].ToString());
					if (aDTSelection.Rows[0]["VIEW_RID"] == System.DBNull.Value)
					{
						_openParms.ViewRID = Include.DefaultPlanViewRID;
					}
					else
					{
						_openParms.ViewRID = Convert.ToInt32(aDTSelection.Rows[0]["VIEW_RID"], CultureInfo.CurrentUICulture);
					}
					if (_passedNodeID > 0)
					{
						//Begin Track #5378 - color and size not qualified
//						_openParms.StoreHLPlanProfile.NodeProfile = _sab.HierarchyServerSession.GetNodeData(_passedNodeID);
						_openParms.StoreHLPlanProfile.NodeProfile = _sab.HierarchyServerSession.GetNodeData(_passedNodeID, true, true);
						//End Track #5378
					}
					else
					{
						if (aDTSelection.Rows[0]["STORE_HN_RID"] == System.DBNull.Value)
						{
							_openParms.StoreHLPlanProfile.NodeProfile = new HierarchyNodeProfile(Include.NoRID);
						}
						else
						{
							//Begin Track #5378 - color and size not qualified
//							_openParms.StoreHLPlanProfile.NodeProfile = _sab.HierarchyServerSession.GetNodeData(Convert.ToInt32(aDTSelection.Rows[0]["STORE_HN_RID"], CultureInfo.CurrentUICulture));
							_openParms.StoreHLPlanProfile.NodeProfile = _sab.HierarchyServerSession.GetNodeData(Convert.ToInt32(aDTSelection.Rows[0]["STORE_HN_RID"], CultureInfo.CurrentUICulture), true, true);
							//End Track #5378
						}
					}
					if (aDTSelection.Rows[0]["STORE_FV_RID"] == System.DBNull.Value)
					{
						// BEGIN Issue 5807 stodd 
						//_openParms.StoreHLPlanProfile.VersionProfile = (VersionProfile)_versionProfList.FindKey(Include.FV_ActionRID);
						// Begin Track #5897 - JSmith - New user gets index error
						if (_storePlanVersionProfList.Count > 0)
						{
							_openParms.StoreHLPlanProfile.VersionProfile = (VersionProfile)_storePlanVersionProfList[0];
						}
						else
						{
							_openParms.StoreHLPlanProfile.VersionProfile = new VersionProfile(Include.NoRID);
						}
						// End Track #5897
						// END Issue 5807
					}
					else
					{
						_openParms.StoreHLPlanProfile.VersionProfile = (VersionProfile)_versionProfList.FindKey(Convert.ToInt32(aDTSelection.Rows[0]["STORE_FV_RID"], CultureInfo.CurrentUICulture));
					}
					if (aDTSelection.Rows[0]["TIME_PERIOD_CDR_RID"] == System.DBNull.Value)
					{
						_openParms.DateRangeProfile = null;
					}
					else
					{
						_openParms.DateRangeProfile = _sab.ClientServerSession.Calendar.GetDateRange(Convert.ToInt32(aDTSelection.Rows[0]["TIME_PERIOD_CDR_RID"], CultureInfo.CurrentUICulture));
					}
					_openParms.DisplayTimeBy = (eDisplayTimeBy)TypeDescriptor.GetConverter(_openParms.DisplayTimeBy).ConvertFrom(aDTSelection.Rows[0]["DISPLAY_TIME_BY_ID"].ToString());
					if (_passedNodeID > 0)
					{
						//Begin Track #5378 - color and size not qualified
//						_openParms.ChainHLPlanProfile.NodeProfile = _sab.HierarchyServerSession.GetNodeData(_passedNodeID);
						_openParms.ChainHLPlanProfile.NodeProfile = _sab.HierarchyServerSession.GetNodeData(_passedNodeID, true, true);
						//End Track #5378
					}
					else
					{
						if (aDTSelection.Rows[0]["CHAIN_HN_RID"] == System.DBNull.Value)
						{
							_openParms.ChainHLPlanProfile.NodeProfile = new HierarchyNodeProfile(Include.NoRID);
						}
						else
						{
							//Begin Track #5378 - color and size not qualified
//							_openParms.ChainHLPlanProfile.NodeProfile = _sab.HierarchyServerSession.GetNodeData(Convert.ToInt32(aDTSelection.Rows[0]["CHAIN_HN_RID"], CultureInfo.CurrentUICulture));
							_openParms.ChainHLPlanProfile.NodeProfile = _sab.HierarchyServerSession.GetNodeData(Convert.ToInt32(aDTSelection.Rows[0]["CHAIN_HN_RID"], CultureInfo.CurrentUICulture), true, true);
							//End Track #5378
						}
					}
					if (aDTSelection.Rows[0]["CHAIN_FV_RID"] == System.DBNull.Value)
					{
						// BEGIN Issue 5807 stodd 
						//_openParms.ChainHLPlanProfile.VersionProfile = (VersionProfile)_versionProfList.FindKey(Include.FV_ActionRID);
						// Begin Track #5897 - JSmith - New user gets index error
						if (_chainPlanVersionProfList.Count > 0)
						{
							_openParms.ChainHLPlanProfile.VersionProfile = (VersionProfile)_chainPlanVersionProfList[0];
						}
						else
						{
							_openParms.ChainHLPlanProfile.VersionProfile = new VersionProfile(Include.NoRID);
						}
						// End Track #5897
						// END Issue 5807 stodd 
					}
					else
					{
						_openParms.ChainHLPlanProfile.VersionProfile = (VersionProfile)_versionProfList.FindKey(Convert.ToInt32(aDTSelection.Rows[0]["CHAIN_FV_RID"], CultureInfo.CurrentUICulture));
					}
					_openParms.IneligibleStores = Include.ConvertCharToBool(Convert.ToChar(aDTSelection.Rows[0]["INCLUDE_INELIGIBLE_STORES_IND"], CultureInfo.CurrentUICulture));
					_openParms.SimilarStores = Include.ConvertCharToBool(Convert.ToChar(aDTSelection.Rows[0]["INCLUDE_SIMILAR_STORES_IND"], CultureInfo.CurrentUICulture));
				
					if (aDTSelection.Rows[0]["LOW_LEVEL_FV_RID"] == System.DBNull.Value)
					{
						// BEGIN Issue 5807 stodd 
						//_openParms.LowLevelVersionDefault = (VersionProfile)_versionProfList.FindKey(Include.FV_ActionRID);
						// Begin Track #5897 - JSmith - New user gets index error
						if (_versionProfList.Count > 0)
						{
							_openParms.LowLevelVersionDefault = (VersionProfile)_versionProfList[0];
						}
						else
						{
							_openParms.LowLevelVersionDefault = new VersionProfile(Include.NoRID);
						}
						// End Track #5897
						// END Issue 5807 stodd 

					}
					else
					{
						_openParms.LowLevelVersionDefault = (VersionProfile)_versionProfList.FindKey(Convert.ToInt32(aDTSelection.Rows[0]["LOW_LEVEL_FV_RID"], CultureInfo.CurrentUICulture));
					}
					if (aDTSelection.Rows[0]["LOW_LEVEL_TYPE"] == System.DBNull.Value)
					{
						_openParms.LowLevelsType = eLowLevelsType.None;
					}
					else
					{
						_openParms.LowLevelsType = (eLowLevelsType)Convert.ToInt32(aDTSelection.Rows[0]["LOW_LEVEL_TYPE"], CultureInfo.CurrentUICulture);
					}
					if (aDTSelection.Rows[0]["LOW_LEVEL_OFFSET"] != System.DBNull.Value)
					{
						_openParms.LowLevelsOffset = Convert.ToInt32(aDTSelection.Rows[0]["LOW_LEVEL_OFFSET"], CultureInfo.CurrentUICulture);
					}
					if (aDTSelection.Rows[0]["LOW_LEVEL_SEQUENCE"] != System.DBNull.Value)
					{
						_openParms.LowLevelsSequence = Convert.ToInt32(aDTSelection.Rows[0]["LOW_LEVEL_SEQUENCE"], CultureInfo.CurrentUICulture);
					}
					// BEGIN OverrideLowLevel enhancement stodd
					if (aDTSelection.Rows[0]["OLL_RID"] == System.DBNull.Value)
					{
						_openParms.OverrideLowLevelRid = Include.NoRID;
						//Begin Track #6099 - KJohnson - Override low-level has wrong data after keying new node
						_openParms.CustomOverrideLowLevelRid = Include.NoRID;
						//End Track #6099 - KJohnson
					}
					else
					{
						_openParms.OverrideLowLevelRid = Convert.ToInt32(aDTSelection.Rows[0]["OLL_RID"], CultureInfo.CurrentUICulture);
					}
					if (aDTSelection.Rows[0]["CUSTOM_OLL_RID"] == System.DBNull.Value)
					{
						_openParms.CustomOverrideLowLevelRid = Include.NoRID;
					}
					else
					{
						_openParms.CustomOverrideLowLevelRid = Convert.ToInt32(aDTSelection.Rows[0]["CUSTOM_OLL_RID"], CultureInfo.CurrentUICulture);
					}
					// END OverrideLowLevel enhancement stodd
                    //Begin TT#609-MD -jsobek -OTS Forecast Chain Ladder View
                    if (aDTSelection.Rows[0]["IS_LADDER"] == System.DBNull.Value)
                    {
                        _openParms.IsLadder = false;
                    }
                    else
                    {
                        if (Convert.ToInt32(aDTSelection.Rows[0]["IS_LADDER"], CultureInfo.CurrentUICulture) == 0)
                        {
                            _openParms.IsLadder = false;
                        }
                        else
                        {
                            _openParms.IsLadder = true;
                        }
                    }
                    if (aDTSelection.Rows[0]["IS_MULTI"] == System.DBNull.Value)
                    {
                        _openParms.IsMulti = false;
                    }
                    else
                    {
                        if (Convert.ToInt32(aDTSelection.Rows[0]["IS_MULTI"], CultureInfo.CurrentUICulture) == 0)
                        {
                            _openParms.IsMulti = false;
                        }
                        else
                        {
                            _openParms.IsMulti = true;
                        }
                    }
                    //End TT#609-MD -jsobek -OTS Forecast Chain Ladder View

                    //Begin TT#639-MD -agallagher - OTS Forecast Totals Right
                    if (aDTSelection.Rows[0]["TOT_RIGHT"] == System.DBNull.Value)
                    {
                        _openParms.IsTotRT = false;
                    }
                    else
                    {
                        if (Convert.ToInt32(aDTSelection.Rows[0]["TOT_RIGHT"], CultureInfo.CurrentUICulture) == 0)
                        {
                            _openParms.IsTotRT = false;
                        }
                        else
                        {
                            _openParms.IsTotRT = true;
                        }
                    }
                    //End TT#639-MD -agallagher - OTS Forecast Totals Right
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		// START TT#6 - VStuart - Single Store Select
		private void BindStoreComboBox()
		{
			try
			{
				//Begin TT#1517-MD -jsobek -Store Service Optimization
                //FunctionSecurityProfile userAttrSecLvl = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AdminStoreAttributesUser);
                //ProfileList spl = _sab.ApplicationServerSession.GetProfileList(eProfileType.Store, !userAttrSecLvl.AccessDenied);
                ProfileList spl = StoreMgmt.StoreProfiles_GetActiveStoresList(); 
				//End TT#1517-MD -jsobek -Store Service Optimization

                cboStore.Items.Clear();
                cboStore.Items.Add("(None)");

                //This section gives the user a complete list by clicking the dropdown.
                foreach (object obj in spl)
                { cboStore.Items.Add(obj); }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }
		// END TT#6 - VStuart - Single Store Select

		// START TT#6 - VStuart - Single Store Select
		private void BindStoreNonMultiComboBox()
		{
			try
			{
				//FunctionSecurityProfile userAttrSecLvl = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AdminStoreAttributesUser);
				//ProfileList spl = _sab.ApplicationServerSession.GetProfileList(eProfileType.Store, !userAttrSecLvl.AccessDenied);
                ProfileList spl = StoreMgmt.StoreProfiles_GetActiveStoresList(); 

                cboStoreNonMulti.Items.Clear();
                cboStoreNonMulti.Items.Add("(None)");

			    //This section gives the user a complete list by clicking the dropdown.
                foreach (object obj in spl)
                { cboStoreNonMulti.Items.Add(obj); }
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
		// END TT#6 - VStuart - Single Store Select

        private void PopulateStoreSingleLevelPanel()
		{
			try
			{
				// Inititalize Fields

				mdsSSLPlanDateRange.DateRangeRID = Include.UndefinedCalendarDateRange;
				mdsSSLPlanDateRange.SetImage(null);

				// START TT#6 - VStuart - Single Store Select
				BindStoreNonMultiComboBox();
				// END TT#6 - VStuart - Single Store Select

				// Set up "Display" controls

				cboSSLStoreAttribute.SelectedValue = _openParms.StoreGroupRID;
                cboSSLView.SelectedValue = _openParms.ViewRID;
                cboSSLGroupBy.SelectedValue = Convert.ToInt32(_openParms.GroupBy, CultureInfo.CurrentUICulture);
                cboSSLFilter.SelectedIndex = cboSSLFilter.Items.IndexOf(new FilterNameCombo(_openParms.FilterRID, -1, ""));
                ////this.cboSSLFilter_SelectionChangeCommitted(source, new EventArgs()); // TT#301-MD - RBeck - Controls are not functioning properly
                chkSSLIneligibleStores.Checked = _openParms.IneligibleStores;
				chkSSLSimilarStores.Checked = _openParms.SimilarStores;

				// Set up "OTS Plan" controls
				//Begin Track #5858 - JSmith - Validating store security only
				txtSSLStoreNode.Tag = new MIDMerchandiseTextBoxTag(SAB, txtSSLStoreNode, eSecurityTypes.Store, eSecuritySelectType.View | eSecuritySelectType.Update);
				//End Track #5858

				if (_openParms.StoreHLPlanProfile.NodeProfile.Key > 0)
				{
					txtSSLStoreNode.Text = _openParms.StoreHLPlanProfile.NodeProfile.Text;
					//Begin Track #5858 - JSmith - Validating store security only
					//txtSSLStoreNode.Tag = _openParms.StoreHLPlanProfile.NodeProfile;
					((MIDTag)txtSSLStoreNode.Tag).MIDTagData = _openParms.StoreHLPlanProfile.NodeProfile;
					//End Track #5858
				}
				else
				{
					txtSSLStoreNode.Text = "";
				}

//Begin Track #3936 - JScott - Errors encountered with new User w/no security
//				cboSSLStoreVersion.SelectedValue = _openParms.StoreHLPlanProfile.VersionProfile.Key;
				if (_openParms.StoreHLPlanProfile.VersionProfile != null)
				{
                    cboSSLStoreVersion.SelectedValue = _openParms.StoreHLPlanProfile.VersionProfile.Key;
				}
				else
				{
                    cboSSLStoreVersion.SelectedIndex = cboSSLStoreVersion.Items.Count - 1;
				}
//End Track #3936 - JScott - Errors encountered with new User w/no security

				if (_openParms.DateRangeProfile != null &&
					_openParms.DateRangeProfile.Key > 0)
				{
					LoadDateRangeSelector(mdsSSLPlanDateRange, _openParms.DateRangeProfile);
				}
				
				//Begin Track #5858 - JSmith - Validating store security only
				txtSSLChainNode.Tag = new MIDMerchandiseTextBoxTag(SAB, txtSSLChainNode, eMIDControlCode.form_OTSPlanSelection, eSecurityTypes.Chain, eSecuritySelectType.View | eSecuritySelectType.Update);
				//End Track #5858
				if (_openParms.ChainHLPlanProfile.NodeProfile.Key > 0)
				{
					txtSSLChainNode.Text = _openParms.ChainHLPlanProfile.NodeProfile.Text;
					//Begin Track #5858 - JSmith - Validating store security only
					//txtSSLChainNode.Tag = _openParms.ChainHLPlanProfile.NodeProfile;
					((MIDTag)txtSSLChainNode.Tag).MIDTagData = _openParms.ChainHLPlanProfile.NodeProfile;
					//End Track #5858
				}
				else
				{
					txtSSLChainNode.Text = "";
				}

//Begin Track #3936 - JScott - Errors encountered with new User w/no security
//				cboSSLChainVersion.SelectedValue = _openParms.ChainHLPlanProfile.VersionProfile.Key;
				if (_openParms.ChainHLPlanProfile.VersionProfile != null)
				{
                    cboSSLChainVersion.SelectedValue = _openParms.ChainHLPlanProfile.VersionProfile.Key;
				}
				else
				{
                    cboSSLChainVersion.SelectedIndex = cboSSLChainVersion.Items.Count - 1;
				}
//End Track #3936 - JScott - Errors encountered with new User w/no security
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		private void PopulateStoreMultiLevelPanel()
		{
			try
			{
				// Inititalize Fields

				mdsSMLPlanDateRange.DateRangeRID = Include.UndefinedCalendarDateRange;
				mdsSMLPlanDateRange.SetImage(null);

				// START TT#6 - VStuart - Single Store Select
				BindStoreComboBox();
				// END TT#6 - VStuart - Single Store Select

				// Set up "Display" controls

                cboSMLStoreAttribute.SelectedValue = _openParms.StoreGroupRID;
                cboSMLView.SelectedValue = _openParms.ViewRID;
                cboSMLGroupBy.SelectedValue = Convert.ToInt32(_openParms.GroupBy, CultureInfo.CurrentUICulture);
                cboSMLFilter.SelectedIndex = cboSMLFilter.Items.IndexOf(new FilterNameCombo(_openParms.FilterRID, -1, ""));
                ////this.cboSMLFilter_SelectionChangeCommitted(source, new EventArgs()); // TT#301-MD - RBeck - Controls are not functioning properly
				chkSMLIneligibleStores.Checked = _openParms.IneligibleStores;
				chkSMLSimilarStores.Checked = _openParms.SimilarStores;

				// Set up "OTS Plan" controls

				//Begin Track #5858 - JSmith - Validating store security only
				txtSMLHighLevelNode.Tag = new MIDMerchandiseTextBoxTag(SAB, txtSMLHighLevelNode, eMIDControlCode.form_OTSPlanSelection, eSecurityTypes.Store, eSecuritySelectType.View | eSecuritySelectType.Update);
				//End Track #5858
				if (_openParms.StoreHLPlanProfile.NodeProfile.Key > 0)
				{
					txtSMLHighLevelNode.Text = _openParms.StoreHLPlanProfile.NodeProfile.Text;
					//Begin Track #5858 - JSmith - Validating store security only
					//txtSMLHighLevelNode.Tag = _openParms.StoreHLPlanProfile.NodeProfile;
					((MIDTag)txtSMLHighLevelNode.Tag).MIDTagData = _openParms.StoreHLPlanProfile.NodeProfile;
					//End Track #5858
				}
				else
				{
					txtSMLHighLevelNode.Text = "";
				}

//Begin Track #3936 - JScott - Errors encountered with new User w/no security
//				cboSMLHighLevelVersion.SelectedValue = _openParms.StoreHLPlanProfile.VersionProfile.Key;
				if (_openParms.StoreHLPlanProfile.VersionProfile != null)
				{
                    cboSMLHighLevelVersion.SelectedValue = _openParms.StoreHLPlanProfile.VersionProfile.Key;
				}
				else
				{
                    cboSMLHighLevelVersion.SelectedIndex = cboSMLHighLevelVersion.Items.Count - 1;
				}
//End Track #3936 - JScott - Errors encountered with new User w/no security

				if (_openParms.DateRangeProfile != null &&
					_openParms.DateRangeProfile.Key > 0)
				{
					LoadDateRangeSelector(mdsSMLPlanDateRange, _openParms.DateRangeProfile);
				}
				
//Begin Track #3936 - JScott - Errors encountered with new User w/no security
//				cboSMLLowLevelsVersion.SelectedValue = _openParms.LowLevelVersionDefault.Key;
				if (_openParms.LowLevelVersionDefault != null)
				{
                    cboSMLLowLevelsVersion.SelectedValue = _openParms.LowLevelVersionDefault.Key;
				}
				else
				{
                    cboSMLLowLevelsVersion.SelectedIndex = cboSMLLowLevelsVersion.Items.Count - 1;
				}
//End Track #3936 - JScott - Errors encountered with new User w/no security

				if (_openParms.StoreHLPlanProfile.NodeProfile.Key > 0)
				{
                    PopulateLowLevels(_openParms.StoreHLPlanProfile.NodeProfile, cboSMLLowLevels.ComboBox);
				}

                cboSMLLowLevels.SelectedIndex = cboSMLLowLevels.Items.IndexOf(new LowLevelCombo(_openParms.LowLevelsType, _openParms.LowLevelsOffset, _openParms.LowLevelsSequence, ""));
                LoadOverrideModelComboBox(cboSMLOverride.ComboBox, _openParms.OverrideLowLevelRid, _openParms.CustomOverrideLowLevelRid); 

			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		private void PopulateChainSingleLevelPanel()
		{
			try
			{
				// Inititalize Fields

				mdsCSLPlanDateRange.DateRangeRID = Include.UndefinedCalendarDateRange;
				mdsCSLPlanDateRange.SetImage(null);

				// Set up "Display" controls

                cboCSLView.SelectedValue = _openParms.ViewRID;

				// Set up "OTS Plan" controls

				//Begin Track #5858 - JSmith - Validating store security only
				txtCSLChainNode.Tag = new MIDMerchandiseTextBoxTag(SAB, txtCSLChainNode, eMIDControlCode.form_OTSPlanSelection, eSecurityTypes.Chain, eSecuritySelectType.View | eSecuritySelectType.Update);
				//End Track #5858
				if (_openParms.ChainHLPlanProfile.NodeProfile.Key > 0)
				{
					txtCSLChainNode.Text = _openParms.ChainHLPlanProfile.NodeProfile.Text;
					//Begin Track #5858 - JSmith - Validating store security only
					//txtCSLChainNode.Tag = _openParms.ChainHLPlanProfile.NodeProfile;
					((MIDTag)txtCSLChainNode.Tag).MIDTagData = _openParms.ChainHLPlanProfile.NodeProfile;
					//End Track #5858
				}
				else
				{
					txtCSLChainNode.Text = "";
				}

//Begin Track #3936 - JScott - Errors encountered with new User w/no security
//				cboCSLChainVersion.SelectedValue = _openParms.ChainHLPlanProfile.VersionProfile.Key;
				if (_openParms.ChainHLPlanProfile.VersionProfile != null)
				{
                    cboCSLChainVersion.SelectedValue = _openParms.ChainHLPlanProfile.VersionProfile.Key;
				}
				else
				{
                    cboCSLChainVersion.SelectedIndex = cboCSLChainVersion.Items.Count - 1;
				}
//End Track #3936 - JScott - Errors encountered with new User w/no security

				if (_openParms.DateRangeProfile != null &&
					_openParms.DateRangeProfile.Key > 0)
				{
					LoadDateRangeSelector(mdsCSLPlanDateRange, _openParms.DateRangeProfile);
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		private void PopulateChainMultiLevelPanel()
		{
			try
			{
				// Inititalize Fields

				mdsCMLPlanDateRange.DateRangeRID = Include.UndefinedCalendarDateRange;
				mdsCMLPlanDateRange.SetImage(null);

				// Set up "Display" controls

                cboCMLView.SelectedValue = _openParms.ViewRID;
                cboCMLGroupBy.SelectedValue = Convert.ToInt32(_openParms.GroupBy, CultureInfo.CurrentUICulture);
                cboCMLFilter.SelectedIndex = cboCMLFilter.Items.IndexOf(new FilterNameCombo(_openParms.FilterRID, -1, ""));

				// Set up "OTS Plan" controls

				//Begin Track #5858 - JSmith - Validating store security only
				txtCMLHighLevelNode.Tag = new MIDMerchandiseTextBoxTag(SAB, txtCMLHighLevelNode, eMIDControlCode.form_OTSPlanSelection, eSecurityTypes.Chain, eSecuritySelectType.View | eSecuritySelectType.Update);
				//End Track #5858

				if (_openParms.ChainHLPlanProfile.NodeProfile.Key > 0)
				{
					txtCMLHighLevelNode.Text = _openParms.ChainHLPlanProfile.NodeProfile.Text;
					//Begin Track #5858 - JSmith - Validating store security only
					//txtCMLHighLevelNode.Tag = _openParms.ChainHLPlanProfile.NodeProfile;
					((MIDTag)txtCMLHighLevelNode.Tag).MIDTagData = _openParms.ChainHLPlanProfile.NodeProfile;
					//End Track #5858
				}
				else
				{
					txtCMLHighLevelNode.Text = "";
				}

//Begin Track #3936 - JScott - Errors encountered with new User w/no security
//				cboCMLHighLevelVersion.SelectedValue = _openParms.ChainHLPlanProfile.VersionProfile.Key;
				if (_openParms.ChainHLPlanProfile.VersionProfile != null)
				{
                    cboCMLHighLevelVersion.SelectedValue = _openParms.ChainHLPlanProfile.VersionProfile.Key;
				}
				else
				{
                    cboCMLHighLevelVersion.SelectedIndex = cboCMLHighLevelVersion.Items.Count - 1;
				}
//End Track #3936 - JScott - Errors encountered with new User w/no security

				if (_openParms.DateRangeProfile != null &&
					_openParms.DateRangeProfile.Key > 0)
				{
					LoadDateRangeSelector(mdsCMLPlanDateRange, _openParms.DateRangeProfile);
				}

//Begin Track #3936 - JScott - Errors encountered with new User w/no security
//				cboCMLLowLevelsVersion.SelectedValue = _openParms.LowLevelVersionDefault.Key;
				if (_openParms.LowLevelVersionDefault != null)
				{
                    cboCMLLowLevelsVersion.SelectedValue = _openParms.LowLevelVersionDefault.Key;
				}
				else
				{
                    cboCMLLowLevelsVersion.SelectedIndex = cboCMLLowLevelsVersion.Items.Count - 1;
				}
//End Track #3936 - JScott - Errors encountered with new User w/no security

				if (_openParms.ChainHLPlanProfile.NodeProfile.Key > 0)
				{
                    PopulateLowLevels(_openParms.ChainHLPlanProfile.NodeProfile, cboCMLLowLevels.ComboBox);
				}

				cboCMLLowLevels.SelectedIndex = cboCMLLowLevels.Items.IndexOf(new LowLevelCombo(_openParms.LowLevelsType, _openParms.LowLevelsOffset, _openParms.LowLevelsSequence, ""));
                LoadOverrideModelComboBox(cboCMLOverride.ComboBox, _openParms.OverrideLowLevelRid, _openParms.CustomOverrideLowLevelRid); 
			
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}


		private void PopulateFormBasis()
		{
			int i;
			int nodeID;
			string merchandiseText;

			try
			{
				// Set up "Basis" controls

				for (i = 0; i < _dsBasis.Tables["BasisDetails"].Rows.Count; i++)
				{
					//Fill in the Merchandise Description.

					nodeID = Convert.ToInt32(_dsBasis.Tables["BasisDetails"].Rows[i]["MerchandiseID"], CultureInfo.CurrentUICulture);
					//Begin Track #5378 - color and size not qualified
//					merchandiseText = _sab.HierarchyServerSession.GetNodeData(nodeID).Text;
					merchandiseText = _sab.HierarchyServerSession.GetNodeData(nodeID, true, true).Text;
					//End Track #5378

					//Fill in the DateRange selector's display text.

					_dsBasis.Tables["BasisDetails"].Rows[i]["Merchandise"] = merchandiseText;
					_dsBasis.Tables["BasisDetails"].Rows[i]["IsIncluded"] = Include.ConvertCharToBool(Convert.ToChar(_dsBasis.Tables["BasisDetails"].Rows[i]["IsIncluded"], CultureInfo.CurrentUICulture));
				}

				SetBasisDates();
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}


		private void GetDataSource()
		{
			DataRelation drBasis;

			try
			{
				_dsBasis = _planSelectDL.GetPlanSelectionBasis(_sab.ClientServerSession.UserRID);

				//Create a relationship between the two tables and add it to the dataset.
				//So when the grid is bound to the dataset, the parent-child relationship
				//automatically displays nicely in the grid.

				drBasis = new DataRelation("Basis Details", _dsBasis.Tables["Basis"].Columns["BasisID"], _dsBasis.Tables["BasisDetails"].Columns["BasisID"]);
				_dsBasis.Relations.Add(drBasis);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		#endregion

		#region Binding ComboBoxes

		private void BindFilterComboBox()
		{
			DataTable dtFilter;

			try
			{
                cboSSLFilter.Items.Clear();
                // Begin TT#2669 - JSmith - Unable to remove a filter from a wokflow
                //cboSSLFilter.Items.Add(new FilterNameCombo(-1, Include.GlobalUserRID, "(None)"));	// Issue 3806
                cboSSLFilter.Items.Add(GetRemoveFilterRow());
                // End TT#2669 - JSmith - Unable to remove a filter from a wokflow
                cboSMLFilter.Items.Clear();
                // Begin TT#2669 - JSmith - Unable to remove a filter from a wokflow
                //cboSMLFilter.Items.Add(new FilterNameCombo(-1, Include.GlobalUserRID, "(None)"));	// issue 3806
                cboSMLFilter.Items.Add(GetRemoveFilterRow());
                // End TT#2669 - JSmith - Unable to remove a filter from a wokflow
                cboCMLFilter.Items.Clear();
                // Begin TT#2669 - JSmith - Unable to remove a filter from a wokflow
                //cboCMLFilter.Items.Add(new FilterNameCombo(-1, Include.GlobalUserRID, "(None)"));	// issue 3806
                cboCMLFilter.Items.Add(GetRemoveFilterRow());
                // End TT#2669 - JSmith - Unable to remove a filter from a wokflow

                dtFilter = _storeFilterDL.FilterRead(filterTypes.StoreFilter, eProfileType.FilterStore, _userRIDList);

				foreach (DataRow row in dtFilter.Rows)
				{
                    cboSSLFilter.Items.Add(
						new FilterNameCombo(Convert.ToInt32(row["FILTER_RID"], CultureInfo.CurrentUICulture),
						Convert.ToInt32(row["USER_RID"], CultureInfo.CurrentUICulture),
						Convert.ToString(row["FILTER_NAME"], CultureInfo.CurrentUICulture)));
                    cboSMLFilter.Items.Add(
						new FilterNameCombo(Convert.ToInt32(row["FILTER_RID"], CultureInfo.CurrentUICulture),
						Convert.ToInt32(row["USER_RID"], CultureInfo.CurrentUICulture),
						Convert.ToString(row["FILTER_NAME"], CultureInfo.CurrentUICulture)));
                    cboCMLFilter.Items.Add(
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

		private void BindGroupByComboBox()
		{
			DataTable dtGroupBy;

			try
			{
				dtGroupBy = MIDText.GetTextType(eMIDTextType.eStorePlanSelectedGroupBy, eMIDTextOrderBy.TextValue);
                cboSSLGroupBy.DataSource = dtGroupBy;
                cboSSLGroupBy.DisplayMember = "TEXT_VALUE";
                cboSSLGroupBy.ValueMember = "TEXT_CODE";

                cboSMLGroupBy.DataSource = dtGroupBy;
                cboSMLGroupBy.DisplayMember = "TEXT_VALUE";
                cboSMLGroupBy.ValueMember = "TEXT_CODE";

                cboCMLGroupBy.DataSource = dtGroupBy;
                cboCMLGroupBy.DisplayMember = "TEXT_VALUE";
                cboCMLGroupBy.ValueMember = "TEXT_CODE";
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		private void BindVersionComboBoxes()
		{
			try
			{
				//NOTE: the order of the binding is important. We must bind Chain version
				//before we bind Store version, because store version's selected index
				//drives Chain's. We want Chain version to be populated before we
				//manipulate its selected index through the Store Version's index.

				// begin MID Track # 2365 - do not set binging if no versions
				if (_chainList.Count > 0)
				{
				// end MID Track # 2365
                    cboSSLChainVersion.DataSource = _chainList;
                    cboSSLChainVersion.DisplayMember = "Description";
                    cboSSLChainVersion.ValueMember = "Key";
				// begin MID Track # 2365
                    cboCSLChainVersion.DataSource = _chainList;
                    cboCSLChainVersion.DisplayMember = "Description";
                    cboCSLChainVersion.ValueMember = "Key";

                    cboCMLHighLevelVersion.DataSource = _chainList;
                    cboCMLHighLevelVersion.DisplayMember = "Description";
                    cboCMLHighLevelVersion.ValueMember = "Key";
                    cboCMLLowLevelsVersion.DataSource = _chainLowLevelVersionList.ArrayList;
                    cboCMLLowLevelsVersion.DisplayMember = "Description";
                    cboCMLLowLevelsVersion.ValueMember = "Key";
				}
				// end MID Track # 2365
		
				// begin MID Track # 2365 - do not set binging if no versions
				if (_storeList.Count > 0)
				{
				// end MID Track # 2365
                    cboSSLStoreVersion.DataSource = _storeList;
                    cboSSLStoreVersion.DisplayMember = "Description";
                    cboSSLStoreVersion.ValueMember = "Key";
				// begin MID Track # 2365
                    cboSMLHighLevelVersion.DataSource = _storeList;
                    cboSMLHighLevelVersion.DisplayMember = "Description";
                    cboSMLHighLevelVersion.ValueMember = "Key";
                    cboSMLLowLevelsVersion.DataSource = _storeLowLevelVersionList.ArrayList;
                    cboSMLLowLevelsVersion.DisplayMember = "Description";
                    cboSMLLowLevelsVersion.ValueMember = "Key";
				}
				// end MID Track # 2365
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		private void BindStoreAttrComboBoxes()
		{
			ProfileList sgpl;
			StoreGroupListViewProfile sgp;
			// Begin Track #4872 - JSmith - Global/User Attributes
			FunctionSecurityProfile userAttrSecLvl;
			// End Track #4872

			try
			{
//Begin Track #3767 - JScott - Force client to use cached store group lists in application session
//				sgpl = _sab.StoreServerSession.GetStoreGroupListViewList();
				// Begin Track #4872 - JSmith - Global/User Attributes
				userAttrSecLvl = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AdminStoreAttributesUser);
				//Begin TT#1517-MD -jsobek -Store Service Optimization
				//sgpl = _sab.ApplicationServerSession.GetProfileList(eProfileType.StoreGroupListView, !userAttrSecLvl.AccessDenied);
                sgpl = StoreMgmt.StoreGroup_GetListViewList(eStoreGroupSelectType.MyUserAndGlobal, !userAttrSecLvl.AccessDenied);
				//End TT#1517-MD -jsobek -Store Service Optimization
				// End Track #4872
//End Track #3767 - JScott - Force client to use cached store group lists in application session
				sgp = (StoreGroupListViewProfile)sgpl[0];

				// Begin Track #4872 - JSmith - Global/User Attributes
				//this.cboSSLStoreAttribute.ValueMember = "Key";
				//this.cboSSLStoreAttribute.DisplayMember = "Name";
				//this.cboSSLStoreAttribute.DataSource = sgpl.ArrayList;
				userAttrSecLvl = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AdminStoreAttributesUser);
				cboSSLStoreAttribute.Initialize(SAB, _singleStoreSecurity, sgpl.ArrayList, true);
				// End Track #4872
				this.cboSSLStoreAttribute.SelectedValue = sgp.Key;

				// Begin Track #4872 - JSmith - Global/User Attributes
				//this.cboSMLStoreAttribute.ValueMember = "Key";
				//this.cboSMLStoreAttribute.DisplayMember = "Name";
				//this.cboSMLStoreAttribute.DataSource = sgpl.ArrayList;
                cboSMLStoreAttribute.Initialize(SAB, _multiStoreSecurity, sgpl.ArrayList, true);
				// End Track #4872
                this.cboSMLStoreAttribute.SelectedValue = sgp.Key;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
		
		private void BindViewComboBoxes()
		{
			try
			{
                cboSSLView.ValueMember = "VIEW_RID";
//Begin Track #4804 - JScott - View name being appended with (User) on save
//				cboSSLView.DisplayMember = "VIEW_ID";
                cboSSLView.DisplayMember = "DISPLAY_ID";
//End Track #4804 - JScott - View name being appended with (User) on save
                cboSSLView.DataSource = _dtView;

                cboSMLView.ValueMember = "VIEW_RID";
//Begin Track #4804 - JScott - View name being appended with (User) on save
//				cboSMLView.DisplayMember = "VIEW_ID";
                cboSMLView.DisplayMember = "DISPLAY_ID";
//End Track #4804 - JScott - View name being appended with (User) on save
                cboSMLView.DataSource = _dtView;

                cboCSLView.ValueMember = "VIEW_RID";
//Begin Track #4804 - JScott - View name being appended with (User) on save
//				cboCSLView.DisplayMember = "VIEW_ID";
                cboCSLView.DisplayMember = "DISPLAY_ID";
//End Track #4804 - JScott - View name being appended with (User) on save
                cboCSLView.DataSource = _dtView;

                cboCMLView.ValueMember = "VIEW_RID";
//Begin Track #4804 - JScott - View name being appended with (User) on save
//				cboCMLView.DisplayMember = "VIEW_ID";
                cboCMLView.DisplayMember = "DISPLAY_ID";
//End Track #4804 - JScott - View name being appended with (User) on save
                cboCMLView.DataSource = _dtView;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		private void BindComputationModeComboBox()
		{
			try
			{
                cboComputationMode.Items.Clear();
				foreach (string comp in ComputationModes)
				{
                    cboComputationMode.Items.Add(new ComputationModeCombo(comp));
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
		
		/// <summary>
		/// Creates a list for use on the "Version" column, which is a dropdown.
		/// </summary>
		
		private void CreateComboLists()
		{
			int i;
			Infragistics.Win.ValueList vl;
			Infragistics.Win.ValueListItem vli;

			try
			{
				vl = grdBasis.DisplayLayout.ValueLists.Add("Version");
				for (i = 0; i < _versionProfList.Count; i++)
				{
					vli = new Infragistics.Win.ValueListItem();
					vli.DataValue= _versionProfList[i].Key;
					vli.DisplayText = ((VersionProfile)_versionProfList[i]).Description;
					vl.ValueListItems.Add(vli);
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		private void PopulateLowLevels(HierarchyNodeProfile aHierarchyNodeProfile, ComboBox aComboBox)
		{
			try
			{
				HierarchyProfile hierProf;
				aComboBox.Items.Clear();
				if (aHierarchyNodeProfile == null)
				{
					aComboBox.Enabled = false;
				}
				else
				{
					aComboBox.Enabled = true;
				}
				if (aHierarchyNodeProfile != null)
				{
					hierProf = _sab.HierarchyServerSession.GetHierarchyData(aHierarchyNodeProfile.HierarchyRID);
					if (hierProf.HierarchyType == eHierarchyType.organizational)
					{
						for (int i = aHierarchyNodeProfile.HomeHierarchyLevel + 1; i <= hierProf.HierarchyLevels.Count; i++)
						{
							HierarchyLevelProfile hlp = (HierarchyLevelProfile)hierProf.HierarchyLevels[i];
							aComboBox.Items.Add(
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
						HierarchyProfile mainHierProf = _sab.HierarchyServerSession.GetMainHierarchyData();

						//Begin Track #4358 - JSmith - Performance opening alternate node
						if (_currentLowLevelNode != aHierarchyNodeProfile.Key)
						{
							_longestHighestGuest = _sab.HierarchyServerSession.GetHighestGuestLevel(aHierarchyNodeProfile.Key);
						}
						int highestGuestLevel = _longestHighestGuest;
						//End Track #4358

						// add guest levels to comboBox
						if ((highestGuestLevel != int.MaxValue) && (aHierarchyNodeProfile.HomeHierarchyType != eHierarchyType.alternate)) // TT#55 - KJohnson - Override Level option needs to reflect Low level already selected(in all review screens and methods with override level option)
						{
							for (int i = highestGuestLevel; i <= mainHierProf.HierarchyLevels.Count; i++)
							{
//Begin Track #4004 -- Double click on My Alternate
								if (i == 0)
								{
									aComboBox.Items.Add(
										new LowLevelCombo(eLowLevelsType.HierarchyLevel,
										0,
										0,
										mainHierProf.HierarchyID));
								}
								else
								{
//End Track #4004
									HierarchyLevelProfile hlp = (HierarchyLevelProfile)mainHierProf.HierarchyLevels[i];
									aComboBox.Items.Add(
										new LowLevelCombo(eLowLevelsType.HierarchyLevel,
										//Begin Track #5866 - JScott - Matrix Balance does not work
										//0,
										i,
										//End Track #5866 - JScott - Matrix Balance does not work
										hlp.Key,
										hlp.LevelID));
//Begin Track #4004 -- Double click on My Alternate
								}
//End Track #4004
							}
						}

						// add offsets to comboBox
						//Begin Track #4358 - JSmith - Performance opening alternate node
						if (_currentLowLevelNode != aHierarchyNodeProfile.Key)
						{
                            //BEGIN TT#4689 - DOConnell - OTS Forecast - Multi-Level Low Levels not being populated correctly
                            //_longestBranch = _sab.HierarchyServerSession.GetLongestBranch(aHierarchyNodeProfile.Key);
                             DataTable hierarchyLevels = _sab.HierarchyServerSession.GetHierarchyDescendantLevels(aHierarchyNodeProfile.Key);
                            _longestBranch = hierarchyLevels.Rows.Count - 1;
                            //END TT#4689 - DOConnell - OTS Forecast - Multi-Level Low Levels not being populated correctly
						}
						int longestBranchCount = _longestBranch; 
						//End Track #4358
						int offset = 0;
                        for (int i = 0; i < longestBranchCount; i++)
                        {
                            ++offset;
                            aComboBox.Items.Add(
                                new LowLevelCombo(eLowLevelsType.LevelOffset,
                                offset,
                                0,
                                null));
                        }
					}
					if (aComboBox.Items.Count > 0)
					{
						aComboBox.SelectedIndex = 0;
					}
					// BEGIN Override Low Level Enhancement
					//_lowlevelVersionOverrideList.Clear();
					// BEGIN Override Low Level Enhancemen
					//Begin Track #4358 - JSmith - Performance opening alternate node
					_currentLowLevelNode = aHierarchyNodeProfile.Key;
					//End Track #4358

					// Begin Track #5960 - JSmith - Get "No Low Levels Found" when using hierarchy level name
					_lowLevels = new ArrayList();
					foreach (LowLevelCombo llc in aComboBox.Items)
					{
						_lowLevels.Add(llc);
					}
					// End Track #5960
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
		
		#endregion

		#region Events

		#region Button Events

		private void btnOK_Click(object sender, System.EventArgs e)
		{
			Form parentForm;
			
			try
			{
				Cursor.Current = Cursors.WaitCursor;

				_dsBasis.AcceptChanges();

				CheckForm();

				if (_openParms.StoreHLPlanProfile != null && _openParms.StoreHLPlanProfile.NodeProfile != null
					&& _openParms.StoreHLPlanProfile.NodeProfile.Key != Include.NoRID)
				{
                    // Begin TT#2781 - JSmith - Style not listed at top of OTS review screen
                    //_openParms.StoreHLPlanProfile.NodeProfile = _sab.HierarchyServerSession.GetNodeData(_openParms.StoreHLPlanProfile.NodeProfile.Key);
                    // Begin TT#2799 - JSmith - On Hand and Tot Fill blank at all levels
                    //_openParms.StoreHLPlanProfile.NodeProfile = _sab.HierarchyServerSession.GetNodeData(_openParms.StoreHLPlanProfile.NodeProfile.Key, false, true);
                    _openParms.StoreHLPlanProfile.NodeProfile = _sab.HierarchyServerSession.GetNodeData(_openParms.StoreHLPlanProfile.NodeProfile.Key, true, true);
                    // End TT#2799 - JSmith - On Hand and Tot Fill blank at all levels
                    // End TT#2781 - JSmith - Style not listed at top of OTS review screen
				}

				if (_openParms.ChainHLPlanProfile != null && _openParms.ChainHLPlanProfile.NodeProfile != null
					&& _openParms.ChainHLPlanProfile.NodeProfile.Key != Include.NoRID)
				{
                    // Begin TT#2781 - JSmith - Style not listed at top of OTS review screen
                    //_openParms.ChainHLPlanProfile.NodeProfile = _sab.HierarchyServerSession.GetNodeData(_openParms.ChainHLPlanProfile.NodeProfile.Key);
                    // Begin TT#2799 - JSmith - On Hand and Tot Fill blank at all levels
                    //_openParms.ChainHLPlanProfile.NodeProfile = _sab.HierarchyServerSession.GetNodeData(_openParms.ChainHLPlanProfile.NodeProfile.Key, false, true);
                    _openParms.ChainHLPlanProfile.NodeProfile = _sab.HierarchyServerSession.GetNodeData(_openParms.ChainHLPlanProfile.NodeProfile.Key, true, true);
                    // End TT#2799 - JSmith - On Hand and Tot Fill blank at all levels
                    // ENd TT#2781 - JSmith - Style not listed at top of OTS review screen
				}
				// Capture and save all the information on this form

				if (_openParms.ChainHLPlanProfile.NodeProfile.Key == _openParms.StoreHLPlanProfile.NodeProfile.Key)
				{
					if (_openParms.ChainHLPlanProfile.NodeProfile.Key != Include.NoRID)
					{
						_openParms.StoreHLPlanProfile.NodeProfile = _openParms.ChainHLPlanProfile.NodeProfile;
					}
					else
					{
						_openParms.ChainHLPlanProfile.NodeProfile = _openParms.StoreHLPlanProfile.NodeProfile;
					}
				}

//Begin Modification - JScott - Allow access to Computation Mode with Config Setting
//#if (DEBUG)
//				if (cboComputationMode.SelectedIndex != -1)
//				{
//					_openParms.ComputationsMode = ((ComputationModeCombo)cboComputationMode.SelectedItem).ComputationModeName;
//				}
//				else
//				{
//					_openParms.ComputationsMode = _sab.ApplicationServerSession.GetDefaultComputations();
//				}
//#else
//				_openParms.ComputationsMode = _sab.ApplicationServerSession.GetDefaultComputations();
//#endif
				if (_MIDOnlyFunctions)
				{
                    if (cboComputationMode.SelectedIndex != -1)
					{
                        _openParms.ComputationsMode = ((ComputationModeCombo)cboComputationMode.SelectedItem).ComputationModeName;
					}
					else
					{
						_openParms.ComputationsMode = _sab.ApplicationServerSession.GetDefaultComputations();
					}
				}
				else
				{
					_openParms.ComputationsMode = _sab.ApplicationServerSession.GetDefaultComputations();
				}
//End Modification - JScott - Allow access to Computation Mode with Config Setting

				switch (_openParms.PlanSessionType)
				{
					case ePlanSessionType.ChainSingleLevel:
						_openParms.FunctionSecurityProfile = _singleChainSecurity;
						SaveSingleLevelChain();
						_openParms.ChainHLPlanProfile.NodeProfile.ChainSecurityProfile = _sab.ClientServerSession.GetMyUserNodeSecurityAssignment(_openParms.ChainHLPlanProfile.NodeProfile.Key, (int)eSecurityTypes.Chain);
						break;
					case ePlanSessionType.ChainMultiLevel:
						_openParms.FunctionSecurityProfile = _multiChainSecurity;
						SaveMultiLevelChain();
						BuildLowLevelVersionList(ePlanType.Chain);
						_openParms.ChainHLPlanProfile.NodeProfile.ChainSecurityProfile = _sab.ClientServerSession.GetMyUserNodeSecurityAssignment(_openParms.ChainHLPlanProfile.NodeProfile.Key, (int)eSecurityTypes.Chain);
						break;
					case ePlanSessionType.StoreMultiLevel:
						_openParms.FunctionSecurityProfile = _multiStoreSecurity;
						SaveMultiLevelStore();
						BuildLowLevelVersionList(ePlanType.Store);
						_openParms.ChainHLPlanProfile.NodeProfile = _openParms.StoreHLPlanProfile.NodeProfile;
						_openParms.ChainHLPlanProfile.VersionProfile = _openParms.StoreHLPlanProfile.VersionProfile;
						_openParms.ChainHLPlanProfile.NodeProfile.ChainSecurityProfile = _sab.ClientServerSession.GetMyUserNodeSecurityAssignment(_openParms.ChainHLPlanProfile.NodeProfile.Key, (int)eSecurityTypes.Chain);
						_openParms.StoreHLPlanProfile.NodeProfile.StoreSecurityProfile = _sab.ClientServerSession.GetMyUserNodeSecurityAssignment(_openParms.StoreHLPlanProfile.NodeProfile.Key, (int)eSecurityTypes.Store);
						break;
					default:
						_openParms.FunctionSecurityProfile = _singleStoreSecurity;
						SaveSingleLevelStore();
						_openParms.ChainHLPlanProfile.NodeProfile.ChainSecurityProfile = _sab.ClientServerSession.GetMyUserNodeSecurityAssignment(_openParms.ChainHLPlanProfile.NodeProfile.Key, (int)eSecurityTypes.Chain);
						_openParms.StoreHLPlanProfile.NodeProfile.StoreSecurityProfile = _sab.ClientServerSession.GetMyUserNodeSecurityAssignment(_openParms.StoreHLPlanProfile.NodeProfile.Key, (int)eSecurityTypes.Store);
						break;
				}

//Begin Track #4168 - JScott - Error on Store Mulit-level screen when specifying no low-levels

				if ((_openParms.PlanSessionType == ePlanSessionType.ChainMultiLevel || _openParms.PlanSessionType == ePlanSessionType.StoreMultiLevel) &&
					_openParms.LowLevelPlanProfileList.Count == 0)
				{
					throw new EditErrorException(MIDText.GetText(eMIDTextCode.msg_pl_NoLowLevelsExist));
				}
//End Track #4168 - JScott - Error on Store Mulit-level screen when specifying no low-levels

//Begin Track #5091 - JScott - Secuirty Lights don't change when permission changes
//				AddBasisToOpenParms();
				AddBasisToOpenParms(_openParms.StoreHLPlanProfile.NodeProfile, _openParms.ChainHLPlanProfile.NodeProfile);
				
				switch (_openParms.PlanSessionType)
				{
					case ePlanSessionType.ChainSingleLevel:

						if (_openParms.ChainHLPlanProfile.NodeProfile.ChainSecurityProfile.AccessDenied)
						{
							throw new EditErrorException(MIDText.GetText(eMIDTextCode.msg_pl_ChainPlanAccessDenied));
						}

						foreach (BasisProfile basisProf in _openParms.BasisProfileList)
						{
							foreach (BasisDetailProfile basisDetailProf in basisProf.BasisDetailProfileList)
							{
								if (basisDetailProf.HierarchyNodeProfile.ChainSecurityProfile.AccessDenied)
								{
									throw new EditErrorException(MIDText.GetText(eMIDTextCode.msg_pl_ChainBasisAccessDenied));
								}
							}
						}

						break;

					case ePlanSessionType.ChainMultiLevel:

						if (_openParms.ChainHLPlanProfile.NodeProfile.ChainSecurityProfile.AccessDenied)
						{
							throw new EditErrorException(MIDText.GetText(eMIDTextCode.msg_pl_ChainPlanAccessDenied));
						}

						foreach (BasisProfile basisProf in _openParms.BasisProfileList)
						{
							foreach (BasisDetailProfile basisDetailProf in basisProf.BasisDetailProfileList)
							{
								if (basisDetailProf.HierarchyNodeProfile.ChainSecurityProfile.AccessDenied)
								{
									throw new EditErrorException(MIDText.GetText(eMIDTextCode.msg_pl_ChainBasisAccessDenied));
								}
							}
						}

						break;

					case ePlanSessionType.StoreMultiLevel:

						if (_openParms.StoreHLPlanProfile.NodeProfile.StoreSecurityProfile.AccessDenied)
						{
							throw new EditErrorException(MIDText.GetText(eMIDTextCode.msg_pl_StorePlanAccessDenied));
						}

						if (_openParms.ChainHLPlanProfile.NodeProfile.ChainSecurityProfile.AccessDenied)
						{
							throw new EditErrorException(MIDText.GetText(eMIDTextCode.msg_pl_ChainPlanAccessDenied));
						}
						
						foreach (BasisProfile basisProf in _openParms.BasisProfileList)
						{
							foreach (BasisDetailProfile basisDetailProf in basisProf.BasisDetailProfileList)
							{
								if (basisDetailProf.HierarchyNodeProfile.StoreSecurityProfile.AccessDenied)
								{
									throw new EditErrorException(MIDText.GetText(eMIDTextCode.msg_pl_StoreBasisAccessDenied));
								}

								if (basisDetailProf.HierarchyNodeProfile.ChainSecurityProfile.AccessDenied)
								{
									throw new EditErrorException(MIDText.GetText(eMIDTextCode.msg_pl_ChainBasisAccessDenied));
								}
							}
						}

						break;

					default:

						if (_openParms.StoreHLPlanProfile.NodeProfile.StoreSecurityProfile.AccessDenied)
						{
							throw new EditErrorException(MIDText.GetText(eMIDTextCode.msg_pl_StorePlanAccessDenied));
						}

						if (_openParms.ChainHLPlanProfile.NodeProfile.ChainSecurityProfile.AccessDenied)
						{
							throw new EditErrorException(MIDText.GetText(eMIDTextCode.msg_pl_ChainPlanAccessDenied));
						}
						
						foreach (BasisProfile basisProf in _openParms.BasisProfileList)
						{
							foreach (BasisDetailProfile basisDetailProf in basisProf.BasisDetailProfileList)
							{
								if (basisDetailProf.HierarchyNodeProfile.StoreSecurityProfile.AccessDenied)
								{
									throw new EditErrorException(MIDText.GetText(eMIDTextCode.msg_pl_StoreBasisAccessDenied));
								}

								if (basisDetailProf.HierarchyNodeProfile.ChainSecurityProfile.AccessDenied)
								{
									throw new EditErrorException(MIDText.GetText(eMIDTextCode.msg_pl_ChainBasisAccessDenied));
								}
							}
						}

						break;
				}
//End Track #5091 - JScott - Secuirty Lights don't change when permission changes
                //BEGIN TT#6-MD-VStuart - Single Store Select - Variables never used.
                //DataTable viewTable = _dtView.Copy();
                //ArrayList userRIDList = (ArrayList)_userRIDList.Clone();
                //END TT#6-MD-VStuart - Single Store Select - Variables never used.
                _planSelectDL.SavePlanSelection(_sab.ClientServerSession.UserRID, _openParms, _dsBasis);

				// Create new PlanView

				System.Windows.Forms.Form frm = null;
				object[] args = null;
                bool isTotRT = chkRT.Checked;  // TT#639-MD -agallagher - OTS Forecast Totals Right
				switch (_openParms.PlanSessionType)
				{
					case ePlanSessionType.ChainSingleLevel:
                        //Begin TT#609-MD -jsobek -OTS Forecast Chain Ladder View
                        try
						{
							args  =  new object[]{_sab, _openParms};
                            bool isLadder = radChainLadder.Checked;
                            //bool isTotRT = chkRT.Checked;  // TT#639-MD -agallagher - OTS Forecast Totals Right
                            if (isLadder)
                            {
                                frm = GetForm(typeof(PlanChainLadderView), args);
                                ((PlanChainLadderView)frm).Initialize();
                            }
                            else
                            // Begin TT#639-MD -agallagher - OTS Forecast Totals Right
                            if (isTotRT)
                            {
                                frm = GetForm(typeof(PlanViewRT), args);
                                ((PlanViewRT)frm).Initialize();
                            }
                            else
                            // End TT#639-MD -agallagher - OTS Forecast Totals Right
                            {
                                frm = GetForm(typeof(PlanView), args);
                                ((PlanView)frm).Initialize();
                            }
						}
						catch (Exception)
						{
							if (frm != null)
							{
								frm.Dispose();
							}
							throw;
						}
						break;
                        //End TT#609-MD -jsobek -OTS Forecast Chain Ladder View
					case ePlanSessionType.StoreSingleLevel:
                        // Begin TT#639-MD -agallagher - OTS Forecast Totals Right
                        args = new object[] { _sab, _openParms };
                        if (isTotRT)
                        {
                            frm = GetForm(typeof(PlanViewRT), args);
                            ((PlanViewRT)frm).Initialize();
                        }
                        else
                        {
                            frm = GetForm(typeof(PlanView), args);
                            ((PlanView)frm).Initialize();
                        }
                        break;
                        // End TT#639-MD -agallagher - OTS Forecast Totals Right
					case ePlanSessionType.ChainMultiLevel:
                        // Begin TT#639-MD -agallagher - OTS Forecast Totals Right
                        args = new object[] { _sab, _openParms };
                        if (isTotRT)
                        {
                            frm = GetForm(typeof(PlanViewRT), args);
                            ((PlanViewRT)frm).Initialize();
                        }
                        else
                        {
                            frm = GetForm(typeof(PlanView), args);
                            ((PlanView)frm).Initialize();
                        }
                        break;
                    // End TT#639-MD -agallagher - OTS Forecast Totals Right
					case ePlanSessionType.StoreMultiLevel:
						try
						{
							args  =  new object[]{_sab, _openParms};
                            if (isTotRT)
                            {
                                frm = GetForm(typeof(PlanViewRT), args);
                                ((PlanViewRT)frm).Initialize();
                            }
                            else
                            {
                                // End TT#639-MD -agallagher - OTS Forecast Totals Right
                                frm = GetForm(typeof(PlanView), args);
                                ((PlanView)frm).Initialize();
                            } // TT#639-MD -agallagher - OTS Forecast Totals Right
						}
						catch (Exception)
						{
							if (frm != null)
							{
								frm.Dispose();
							}
							throw;
						}
						break;
					default:
						MessageBox.Show("Invalid PlanSessionType in OpenParms");
						break;
				}
				// Close this form

				parentForm = this.MdiParent;
				this.Close();
				this.MdiParent = null;

				// Show the new PlanView

				frm.MdiParent = parentForm;
				
				// Begin VS2010 WindowState Fix - RMatelic - Maximized window state incorrect when window first opened >>> move WindowState to after Show()
				//frm.WindowState = FormWindowState.Maximized;
				frm.Show();
				frm.WindowState = FormWindowState.Maximized;
				// End VS2010 WindowState Fix

				// BEGIN MID Track #4077 - vertical splitters not correct when window first opens. 
				// only happens in Windows Server 2003 SP1, so this is a workaround
                if (frm.GetType() == typeof(PlanView)) //TT#609-MD -jsobek -OTS Forecast Chain Ladder View
                {
                    ((PlanView)frm).ResetSplitters();
                }
				// END MID Track #4077
			}
			catch (EditErrorException exc)
			{
				MessageBox.Show(exc.Message, "Edit error", MessageBoxButtons.OK);
			}
			catch (PlanInUseException)
			{
			}
			catch (Exception exc)
			{
				HandleException(exc);
			}
			finally
			{
				Cursor.Current = Cursors.Default;
			}
		}

		private System.Windows.Forms.Form GetForm( System.Type aType, object[] args)
		{
			try
			{
				System.Windows.Forms.Form frm = null;

				frm = (System.Windows.Forms.Form)Activator.CreateInstance(aType, args);

				return frm;
			}
			catch( Exception exception )
			{
				MessageBox.Show(exception.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
				throw;
			}
		}

//Begin Track #5091 - JScott - Secuirty Lights don't change when permission changes
//		private void AddBasisToOpenParms()
		private void AddBasisToOpenParms(HierarchyNodeProfile aStoreMultiLevel, HierarchyNodeProfile aChainHLNodeProf)
//End Track #5091 - JScott - Secuirty Lights don't change when permission changes
		{
			try
			{
				_openParms.BasisProfileList.Clear();
				BasisProfile basisProfile;
				BasisDetailProfile basisDetailProfile;
				foreach (DataRow dr in _dsBasis.Tables["Basis"].Rows)
				{
					basisProfile = new BasisProfile(Convert.ToInt32(dr["BasisID"], CultureInfo.CurrentUICulture), Convert.ToString(dr["BasisName"], CultureInfo.CurrentUICulture), _openParms);
					DataView dv = new DataView();
					dv.Table = _dsBasis.Tables["BasisDetails"];
					dv.RowFilter = "BasisID = " + dr["BasisID"];

					for (int i = 0; i < dv.Count; i++)
					{
						basisDetailProfile = new BasisDetailProfile(i + 1, _openParms);
						basisDetailProfile.VersionProfile = (VersionProfile)_versionProfList.FindKey((Convert.ToInt32(dv[i]["VersionID"], CultureInfo.CurrentUICulture)));
//Begin Track #5091 - JScott - Secuirty Lights don't change when permission changes
//						basisDetailProfile.HierarchyNodeProfile = _sab.HierarchyServerSession.GetNodeData(Convert.ToInt32(dv[i]["MerchandiseID"], CultureInfo.CurrentUICulture));

						switch (_openParms.PlanSessionType)
						{
							case ePlanSessionType.ChainSingleLevel:
								//Begin Track #5378 - color and size not qualified
//								basisDetailProfile.HierarchyNodeProfile = _sab.HierarchyServerSession.GetNodeData(Convert.ToInt32(dv[i]["MerchandiseID"], CultureInfo.CurrentUICulture));
								basisDetailProfile.HierarchyNodeProfile = _sab.HierarchyServerSession.GetNodeData(Convert.ToInt32(dv[i]["MerchandiseID"], CultureInfo.CurrentUICulture), true, true);
								//End Track #5378
								basisDetailProfile.HierarchyNodeProfile.ChainSecurityProfile = _sab.ClientServerSession.GetMyUserNodeSecurityAssignment(basisDetailProfile.HierarchyNodeProfile.Key, (int)eSecurityTypes.Chain);
								break;
							case ePlanSessionType.ChainMultiLevel:
								basisDetailProfile.HierarchyNodeProfile = aChainHLNodeProf;
								basisDetailProfile.HierarchyNodeProfile.ChainSecurityProfile = _sab.ClientServerSession.GetMyUserNodeSecurityAssignment(basisDetailProfile.HierarchyNodeProfile.Key, (int)eSecurityTypes.Chain);
								break;
							case ePlanSessionType.StoreMultiLevel:
								basisDetailProfile.HierarchyNodeProfile = aStoreMultiLevel;
								basisDetailProfile.HierarchyNodeProfile.StoreSecurityProfile = _sab.ClientServerSession.GetMyUserNodeSecurityAssignment(basisDetailProfile.HierarchyNodeProfile.Key, (int)eSecurityTypes.Store);
								basisDetailProfile.HierarchyNodeProfile.ChainSecurityProfile = _sab.ClientServerSession.GetMyUserNodeSecurityAssignment(basisDetailProfile.HierarchyNodeProfile.Key, (int)eSecurityTypes.Chain);
								break;
							default:
								//Begin Track #5378 - color and size not qualified
//								basisDetailProfile.HierarchyNodeProfile = _sab.HierarchyServerSession.GetNodeData(Convert.ToInt32(dv[i]["MerchandiseID"], CultureInfo.CurrentUICulture));
								basisDetailProfile.HierarchyNodeProfile = _sab.HierarchyServerSession.GetNodeData(Convert.ToInt32(dv[i]["MerchandiseID"], CultureInfo.CurrentUICulture), true, true);
								//End Track #5378
								basisDetailProfile.HierarchyNodeProfile.StoreSecurityProfile = _sab.ClientServerSession.GetMyUserNodeSecurityAssignment(basisDetailProfile.HierarchyNodeProfile.Key, (int)eSecurityTypes.Store);
								basisDetailProfile.HierarchyNodeProfile.ChainSecurityProfile = _sab.ClientServerSession.GetMyUserNodeSecurityAssignment(basisDetailProfile.HierarchyNodeProfile.Key, (int)eSecurityTypes.Chain);
								break;
						}

//End Track #5091 - JScott - Secuirty Lights don't change when permission changes
						basisDetailProfile.DateRangeProfile = _sab.ClientServerSession.Calendar.GetDateRange(Convert.ToInt32(dv[i]["DateRangeID"], CultureInfo.CurrentUICulture));
						basisDetailProfile.DateRangeProfile.DisplayDate = Convert.ToString(dv[i]["DateRange"], CultureInfo.CurrentUICulture);
						basisDetailProfile.DateRangeProfile.Name = "Basis Total";
						if (Convert.ToBoolean(dv[i]["IsIncluded"], CultureInfo.CurrentUICulture) == true)
						{
							basisDetailProfile.IncludeExclude = eBasisIncludeExclude.Include;
						}
						else
						{
							basisDetailProfile.IncludeExclude = eBasisIncludeExclude.Exclude;
						}
						if (dv[i]["Weight"] == DBNull.Value)
						{
							basisDetailProfile.Weight = 1;
						}
						else
						{
							basisDetailProfile.Weight = Convert.ToSingle(dv[i]["Weight"], CultureInfo.CurrentUICulture);
						}

						// BEGIN MID Track #5647 - KJohnson - Matrix Forecast
						DateRangeProfile planDrp = _sab.ApplicationServerSession.Calendar.GetDateRange(_openParms.DateRangeProfile.Key);
						ProfileList weekRange = _sab.ApplicationServerSession.Calendar.GetWeekRange(planDrp, null);
						basisDetailProfile.ForecastingInfo = new BasisDetailForecastInfo();
						basisDetailProfile.ForecastingInfo.PlanWeek = (WeekProfile)weekRange[0]; //Issue 4025
						basisDetailProfile.ForecastingInfo.OrigWeekListCount = basisDetailProfile.GetWeekProfileList(_sab.ApplicationServerSession).Count;
						basisDetailProfile.ForecastingInfo.BasisPeriodList = _sab.ApplicationServerSession.Calendar.GetDateRangePeriods(basisDetailProfile.DateRangeProfile, (WeekProfile)weekRange[0]); //Issue 4025
						// END MID Track #5647

						basisProfile.BasisDetailProfileList.Add(basisDetailProfile);					

					}
					_openParms.BasisProfileList.Add(basisProfile);

				}
			}
			catch
			{
				throw;
			}
		}

		private void SaveSingleLevelChain()
		{
			try
			{
                _openParms.ViewRID = Convert.ToInt32(_dtView.Rows[(int)cboCSLView.SelectedIndex]["VIEW_RID"], CultureInfo.CurrentUICulture);
                _openParms.ViewName = Convert.ToString(_dtView.Rows[(int)cboCSLView.SelectedIndex]["VIEW_ID"], CultureInfo.CurrentUICulture);
                _openParms.ViewUserID = Convert.ToInt32(_dtView.Rows[(int)cboCSLView.SelectedIndex]["USER_RID"], CultureInfo.CurrentUICulture);
//				_openParms.ChainHLPlanProfile.NodeProfile = _chainNodeProfile;
				_openParms.ChainHLPlanProfile.VersionProfile = (VersionProfile)_versionProfList.FindKey(Convert.ToInt32(cboCSLChainVersion.SelectedValue, CultureInfo.CurrentUICulture));
				_openParms.DateRangeProfile = _sab.ClientServerSession.Calendar.GetDateRange(mdsCSLPlanDateRange.DateRangeRID);
                _openParms.IsLadder = radChainLadder.Checked; //TT#609-MD -jsobek -OTS Forecast Chain Ladder View
                _openParms.IsMulti = chkMultiLevel.Checked; //TT#609-MD -jsobek -OTS Forecast Chain Ladder View
                _openParms.IsTotRT = chkRT.Checked; //TT#639-MD -agallagher - OTS Forecast Totals Right
			}
			catch
			{
				throw;
			}
		}

		private void SaveMultiLevelChain()
		{
			try
			{
                _openParms.ViewRID = Convert.ToInt32(_dtView.Rows[(int)cboCMLView.SelectedIndex]["VIEW_RID"], CultureInfo.CurrentUICulture);
                _openParms.ViewName = Convert.ToString(_dtView.Rows[(int)cboCMLView.SelectedIndex]["VIEW_ID"], CultureInfo.CurrentUICulture);
                _openParms.ViewUserID = Convert.ToInt32(_dtView.Rows[(int)cboCMLView.SelectedIndex]["USER_RID"], CultureInfo.CurrentUICulture);
                _openParms.GroupBy = (eStorePlanSelectedGroupBy)Convert.ToInt32(cboCMLGroupBy.SelectedValue, CultureInfo.CurrentUICulture);
				_openParms.DateRangeProfile = _sab.ClientServerSession.Calendar.GetDateRange(mdsCMLPlanDateRange.DateRangeRID);
//				_openParms.ChainHLPlanProfile.NodeProfile = _chainNodeProfile;
                _openParms.ChainHLPlanProfile.VersionProfile = (VersionProfile)_versionProfList.FindKey(Convert.ToInt32(cboCMLHighLevelVersion.SelectedValue, CultureInfo.CurrentUICulture));
                _openParms.LowLevelVersionDefault = (VersionProfile)_versionProfList.FindKey(Convert.ToInt32(cboCMLLowLevelsVersion.SelectedValue, CultureInfo.CurrentUICulture));
                _openParms.IsLadder = false; //TT#609-MD -jsobek -OTS Forecast Chain Ladder View
                _openParms.IsMulti = chkMultiLevel.Checked; //TT#609-MD -jsobek -OTS Forecast Chain Ladder View
                _openParms.IsTotRT = chkRT.Checked; //TT#639-MD -agallagher - OTS Forecast Totals Right

                if (cboCMLFilter.SelectedIndex != -1)
				{
                    _openParms.FilterRID = ((FilterNameCombo)cboCMLFilter.SelectedItem).FilterRID;
				}
				else
				{
					_openParms.FilterRID = -1;
				}

                if (cboCMLLowLevels.SelectedIndex != -1)
				{
                    _openParms.LowLevelsType = ((LowLevelCombo)cboCMLLowLevels.SelectedItem).LowLevelType;
                    _openParms.LowLevelsOffset = ((LowLevelCombo)cboCMLLowLevels.SelectedItem).LowLevelOffset;
                    _openParms.LowLevelsSequence = ((LowLevelCombo)cboCMLLowLevels.SelectedItem).LowLevelSequence;
				}
				else
				{
					_openParms.LowLevelsType = eLowLevelsType.None;
					_openParms.LowLevelsOffset = 0;
					_openParms.LowLevelsSequence = 0;
				}

				// BEGIN OVerride Low Level Enhancement
				//if (_lowlevelVersionOverrideList.Count == 0)
				//{
				//    PopulateVersionOverrideList(ePlanType.Chain);
				//}
				// END OVerride Low Level Enhancement

			}
			catch
			{
				throw;
			}
		}

		private void SaveMultiLevelStore()
		{
			try
			{
                _openParms.ViewRID = Convert.ToInt32(_dtView.Rows[(int)cboSMLView.SelectedIndex]["VIEW_RID"], CultureInfo.CurrentUICulture);
                _openParms.ViewName = Convert.ToString(_dtView.Rows[(int)cboSMLView.SelectedIndex]["VIEW_ID"], CultureInfo.CurrentUICulture);
				_openParms.ViewUserID = Convert.ToInt32(_dtView.Rows[(int)cboSMLView.SelectedIndex]["USER_RID"], CultureInfo.CurrentUICulture);
                _openParms.StoreGroupRID = (int)cboSMLStoreAttribute.SelectedValue;
                _openParms.GroupBy = (eStorePlanSelectedGroupBy)Convert.ToInt32(cboSMLGroupBy.SelectedValue, CultureInfo.CurrentUICulture);
//				_openParms.StoreHLPlanProfile.NodeProfile = _storeNodeProfile;
                _openParms.StoreHLPlanProfile.VersionProfile = (VersionProfile)_versionProfList.FindKey(Convert.ToInt32(cboSMLHighLevelVersion.SelectedValue, CultureInfo.CurrentUICulture));
				_openParms.DateRangeProfile = _sab.ClientServerSession.Calendar.GetDateRange(mdsSMLPlanDateRange.DateRangeRID);
                _openParms.LowLevelVersionDefault = (VersionProfile)_versionProfList.FindKey(Convert.ToInt32(cboSMLLowLevelsVersion.SelectedValue, CultureInfo.CurrentUICulture));
                _openParms.IsLadder = false; //TT#609-MD -jsobek -OTS Forecast Chain Ladder View
                _openParms.IsMulti = chkMultiLevel.Checked; //TT#609-MD -jsobek -OTS Forecast Chain Ladder View
                _openParms.IsTotRT = chkRT.Checked; //TT#639-MD -agallagher - OTS Forecast Totals Right
                //BEGIN TT#6-MD-VStuart - Single Store Select
                if ((cboStore.SelectedIndex != -1)  && (cboStore.SelectedItem.ToString() != "(None)"))
                {
                    _openParms.StoreId = ((StoreProfile)cboStore.SelectedItem).StoreId;
                    _openParms.StoreIdNm = ((StoreProfile)cboStore.SelectedItem).Text;
                }
                //END TT#6-MD-VStuart - Single Store Select

                if (cboSMLFilter.SelectedIndex != -1)
				{
                    _openParms.FilterRID = ((FilterNameCombo)cboSMLFilter.SelectedItem).FilterRID;
				}
				else
				{
					_openParms.FilterRID = -1;
				}

                if (cboSMLLowLevels.SelectedIndex != -1)
				{
                    _openParms.LowLevelsType = ((LowLevelCombo)cboSMLLowLevels.SelectedItem).LowLevelType;
                    _openParms.LowLevelsOffset = ((LowLevelCombo)cboSMLLowLevels.SelectedItem).LowLevelOffset;
                    _openParms.LowLevelsSequence = ((LowLevelCombo)cboSMLLowLevels.SelectedItem).LowLevelSequence;
				}
				else
				{
					_openParms.LowLevelsType = eLowLevelsType.None;
					_openParms.LowLevelsOffset = 0;
					_openParms.LowLevelsSequence = 0;
				}

				_openParms.IneligibleStores = chkSMLIneligibleStores.Checked; //optIneligibleStoresYes.Checked;
				_openParms.SimilarStores = chkSMLSimilarStores.Checked; //optSimilarStoresYes.Checked;

				// BEGIN OVerride Low Level Enhancement
				//if (_lowlevelVersionOverrideList.Count == 0)
				//{
				//    PopulateVersionOverrideList(ePlanType.Store);
				//}
				// END OVerride Low Level Enhancement
			}
			catch
			{
				throw;
			}
		}

		private void SaveSingleLevelStore()
		{
			try
			{
                _openParms.ViewRID = Convert.ToInt32(_dtView.Rows[(int)cboSSLView.SelectedIndex]["VIEW_RID"], CultureInfo.CurrentUICulture);
                _openParms.ViewName = Convert.ToString(_dtView.Rows[(int)cboSSLView.SelectedIndex]["VIEW_ID"], CultureInfo.CurrentUICulture);
                _openParms.ViewUserID = Convert.ToInt32(_dtView.Rows[(int)cboSSLView.SelectedIndex]["USER_RID"], CultureInfo.CurrentUICulture);
			    _openParms.StoreGroupRID = (int) cboSSLStoreAttribute.SelectedValue;
                _openParms.GroupBy = (eStorePlanSelectedGroupBy)Convert.ToInt32(cboSSLGroupBy.SelectedValue, CultureInfo.CurrentUICulture);
//				_openParms.StoreHLPlanProfile.NodeProfile = _storeNodeProfile;
                _openParms.StoreHLPlanProfile.VersionProfile = (VersionProfile)_versionProfList.FindKey(Convert.ToInt32(cboSSLStoreVersion.SelectedValue, CultureInfo.CurrentUICulture));
			    _openParms.DateRangeProfile = _sab.ClientServerSession.Calendar.GetDateRange(mdsSSLPlanDateRange.DateRangeRID);
//				_openParms.ChainHLPlanProfile.NodeProfile = _chainNodeProfile;
                _openParms.ChainHLPlanProfile.VersionProfile = (VersionProfile)_versionProfList.FindKey(Convert.ToInt32(cboSSLChainVersion.SelectedValue, CultureInfo.CurrentUICulture));
                _openParms.IsLadder = false; //TT#609-MD -jsobek -OTS Forecast Chain Ladder View
                _openParms.IsMulti = chkMultiLevel.Checked; //TT#609-MD -jsobek -OTS Forecast Chain Ladder View
                _openParms.IsTotRT = chkRT.Checked; //TT#639-MD -agallagher - OTS Forecast Totals Right
                //BEGIN TT#6-MD-VStuart - Single Store Select
			    if ((cboStoreNonMulti.SelectedIndex != -1) && (cboStoreNonMulti.SelectedItem.ToString() != "(None)"))
			    {
                    _openParms.StoreId = ((StoreProfile)cboStoreNonMulti.SelectedItem).StoreId;
                    _openParms.StoreIdNm = ((StoreProfile)cboStoreNonMulti.SelectedItem).Text;
                }
                //END TT#6-MD-VStuart - Single Store Select

                if (cboSSLFilter.SelectedIndex != -1)
				{
                    _openParms.FilterRID = ((FilterNameCombo)cboSSLFilter.SelectedItem).FilterRID;
				}
				else
				{
					_openParms.FilterRID = -1;
				}

				_openParms.IneligibleStores = chkSSLIneligibleStores.Checked; //optIneligibleStoresYes.Checked;
				_openParms.SimilarStores = chkSSLSimilarStores.Checked; //optSimilarStoresYes.Checked;
			}
			catch
			{
				throw;
			}
		}

		private void BuildLowLevelVersionList(ePlanType aPlanType)
		{
			try
			{
				//Begin Track #4732 - JSmith - No low levels error
				_openParms.ClearLowLevelPlanProfileList();
				//End Track #4732

				// BEGIN Override Low Level enhancement
				PopulateVersionOverrideList(aPlanType);
				// END Override Low Level enhancement

				foreach (LowLevelVersionOverrideProfile lvop in _lowlevelVersionOverrideList)
				{
					if (!lvop.Exclude)
					{
						PlanProfile planProfile = new PlanProfile(lvop.Key);
						planProfile.NodeProfile = lvop.NodeProfile;
						planProfile.NodeProfile.ChainSecurityProfile = _sab.ClientServerSession.GetMyUserNodeSecurityAssignment(lvop.Key, (int)eSecurityTypes.Chain);
						planProfile.NodeProfile.StoreSecurityProfile = _sab.ClientServerSession.GetMyUserNodeSecurityAssignment(lvop.Key, (int)eSecurityTypes.Store);
						if (lvop.VersionIsOverridden)
						{
							planProfile.VersionProfile = lvop.VersionProfile;
						}
						else
						{
							planProfile.VersionProfile = _openParms.LowLevelVersionDefault;
						}
						//Begin Track #3867 -- Low level not sorted on Store Multi view
						//						_openParms.LowLevelPlanProfileList.Add(planProfile);
						_openParms.AddLowLevelPlanProfile(planProfile);
						//End Track #3867 -- Low level not sorted on Store Multi view
					}
				}
			}
			catch
			{
			}
		}

		private void btnCancel_Click(object sender, System.EventArgs e)
		{
			try
			{
				this.Close();
				this.MdiParent = null;
			}
			catch (Exception exc)
			{
				HandleException(exc);
			}
		}

		#endregion

		#region Grid Events

		private void grdBasis_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
		{
			try
			{
				//Begin TT#169 - JSmith - enter max of 1000 and receive 'must be integer'
				//MIDRetail.Windows.Controls.UltraGridLayoutDefaults ugld = new MIDRetail.Windows.Controls.UltraGridLayoutDefaults();
				//ugld.ApplyDefaults(e);
				MIDRetail.Windows.Controls.UltraGridLayoutDefaults ugld = new MIDRetail.Windows.Controls.UltraGridLayoutDefaults(ErrorImage);
				ugld.ApplyDefaults((Infragistics.Win.UltraWinGrid.UltraGrid)sender, e, true, 2, true);
				//End TT#169

				//The following information pertains to the formatting of the grid.

				//NOTE: Bands[0] refers to the "Basis" table.
				//NTOE: Bands[1] refers to the "Details" table.

				// BEGIN MID Track #3792 - replace obsolete method 
				//grdBasis.DisplayLayout.AutoFitColumns = true;
				grdBasis.DisplayLayout.AutoFitStyle = AutoFitStyle.ResizeAllColumns;
				// END MID Track #3792  

				//hide the key columns.

				e.Layout.Bands[0].Columns["BasisID"].Hidden = true;
				e.Layout.Bands[1].Columns["BasisID"].Hidden = true;
				e.Layout.Bands[1].Columns["MerchandiseID"].Hidden = true;
				e.Layout.Bands[1].Columns["VersionID"].Hidden = true;
				e.Layout.Bands[1].Columns["IsIncluded"].Hidden = true;
				e.Layout.Bands[1].Columns["DateRangeID"].Hidden = true;
				e.Layout.Bands[1].Columns["Picture"].Hidden = true;

				//Prevent the user from re-arranging columns.

				grdBasis.DisplayLayout.Override.AllowColMoving = AllowColMoving.NotAllowed;

				//Set the header captions.

				grdBasis.DisplayLayout.Bands[1].Columns["Merchandise"].Header.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_Merchandise);
				grdBasis.DisplayLayout.Bands[1].Columns["Version"].Header.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_Version);
				grdBasis.DisplayLayout.Bands[1].Columns["DateRange"].Header.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_PlanTimePeriod);
				grdBasis.DisplayLayout.Bands[1].Columns["Weight"].Header.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_Weight);
				grdBasis.DisplayLayout.Bands[1].Columns["IncludeButton"].Header.Caption = " ";

				//Set the widths of the columns.

				grdBasis.DisplayLayout.Bands[1].Columns["Merchandise"].Width = 200;
				grdBasis.DisplayLayout.Bands[1].Columns["Version"].Width = 100;
				grdBasis.DisplayLayout.Bands[1].Columns["DateRange"].Width = 200;
				grdBasis.DisplayLayout.Bands[1].Columns["Weight"].Width = 80;
				grdBasis.DisplayLayout.Bands[1].Columns["IncludeButton"].Width = 20;

				//hide the column header.

				grdBasis.DisplayLayout.Bands[0].ColHeadersVisible = false;

				//Make some columns readonly.

				e.Layout.Bands[0].Columns["BasisName"].CellActivation = Activation.NoEdit;
		
				//make the "Version" column a drop down list.

				grdBasis.DisplayLayout.Bands[1].Columns["Version"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
				grdBasis.DisplayLayout.Bands[1].Columns["Version"].ValueList = grdBasis.DisplayLayout.ValueLists["Version"];
		
				//the "IncludeButton" column is the column that contains buttons
				//to include/exclude a basis detail. 

				grdBasis.DisplayLayout.Bands[1].Columns["IncludeButton"].CellAppearance.ImageHAlign = Infragistics.Win.HAlign.Center;
				grdBasis.DisplayLayout.Bands[1].Columns["IncludeButton"].CellAppearance.ImageVAlign = Infragistics.Win.VAlign.Middle;
				grdBasis.DisplayLayout.Bands[1].Columns["IncludeButton"].Style  = Infragistics.Win.UltraWinGrid.ColumnStyle.Button;
				grdBasis.DisplayLayout.Bands[1].Columns["IncludeButton"].CellActivation = Activation.NoEdit;

				//Set the width of the "DateRange" column.

				grdBasis.DisplayLayout.Bands[1].Columns["DateRange"].Style  = Infragistics.Win.UltraWinGrid.ColumnStyle.EditButton;
				grdBasis.DisplayLayout.Bands[1].Columns["DateRange"].CellActivation = Activation.ActivateOnly;

				//the following code tweaks the "Add New" buttons (which come with the grid).

				grdBasis.DisplayLayout.Bands[0].AddButtonCaption = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_AddBasis);
				grdBasis.DisplayLayout.Bands[0].AddButtonToolTipText = MIDText.GetTextOnly(eMIDTextCode.tt_Button_AddBasis);
				grdBasis.DisplayLayout.Bands[1].AddButtonCaption = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_AddBasisDetails);
				grdBasis.DisplayLayout.Bands[1].AddButtonToolTipText = MIDText.GetTextOnly(eMIDTextCode.tt_Button_AddBasisDetails);
				grdBasis.DisplayLayout.AddNewBox.Hidden = false;
				grdBasis.DisplayLayout.AddNewBox.Style = AddNewBoxStyle.Compact;
			}
			catch (Exception exc)
			{
				HandleException(exc);
			}
		}

		private void grdBasis_InitializeRow(object sender, Infragistics.Win.UltraWinGrid.InitializeRowEventArgs e)
		{
			try
			{
				//Set the width of the "DateRange" column.

				if (e.Row.Band.Index == 1)
				{
					if (e.Row.Cells["IsIncluded"].Value != DBNull.Value)
					{
						if (Convert.ToBoolean(e.Row.Cells["IsIncluded"].Value, CultureInfo.CurrentUICulture) == true)
						{
							e.Row.Cells["IncludeButton"].Appearance.Image = _picInclude;
						}
						else
						{
							e.Row.Cells["IncludeButton"].Appearance.Image = _picExclude;
						}
					}
				}
			}
			catch (Exception exc)
			{
				HandleException(exc);
			}
		}
		
		private void grdBasis_BeforeRowInsert(object sender, Infragistics.Win.UltraWinGrid.BeforeRowInsertEventArgs e)
		{
			DataRow basisRow;
			DataRow detailsRow;
			UltraGridRow row;

			try
			{
				this.Cursor = Cursors.WaitCursor;

				CheckInsertCondition(e);

				//If we are inserting a parent row (or "Basis"), we want to set both
				//its ID and Name, instead of letting the user do it. The best way
				//is to directly add the new row in the datatable, because the bound grid
				//will automatically reflect this change.

				if (e.Band == grdBasis.DisplayLayout.Bands[0])
				{
					basisRow = (DataRow)_dsBasis.Tables["Basis"].NewRow();

					if (_dsBasis.Tables["Basis"].Rows.Count == 0)
					{
						basisRow["BasisID"] = 0;
					}
					else
					{
						basisRow["BasisID"] = Convert.ToInt32(_dsBasis.Tables["Basis"].Rows[_dsBasis.Tables["Basis"].Rows.Count-1]["BasisID"], CultureInfo.CurrentUICulture) + 1; //Increase the ID by 1 (based on the last row's ID).
					}

					basisRow["BasisName"] = "Basis " + Convert.ToString(_dsBasis.Tables["Basis"].Rows.Count + 1, CultureInfo.CurrentUICulture);
					detailsRow = (DataRow)_dsBasis.Tables["BasisDetails"].NewRow();
					detailsRow["BasisID"] = basisRow["BasisID"]; 
					detailsRow["IsIncluded"] = true;

					if (_openParms.PlanSessionType == ePlanSessionType.StoreSingleLevel ||
						_openParms.PlanSessionType == ePlanSessionType.StoreMultiLevel)
					{
						if (_openParms.StoreHLPlanProfile != null &&
							_openParms.StoreHLPlanProfile.NodeProfile != null)
						{
							detailsRow["Merchandise"] = _openParms.StoreHLPlanProfile.NodeProfile.Text;
							detailsRow["MerchandiseID"] = _openParms.StoreHLPlanProfile.NodeProfile.Key;
						}
					}
					else
					{
						if (_openParms.ChainHLPlanProfile != null &&
							_openParms.ChainHLPlanProfile.NodeProfile != null)
						{
							detailsRow["Merchandise"] = _openParms.ChainHLPlanProfile.NodeProfile.Text;
							detailsRow["MerchandiseID"] = _openParms.ChainHLPlanProfile.NodeProfile.Key;
						}
					}

					_dsBasis.Tables["Basis"].Rows.Add(basisRow);
					_dsBasis.Tables["BasisDetails"].Rows.Add(detailsRow);

					//Set the active row to this newly added Basis row.

					grdBasis.ActiveRow = grdBasis.Rows[grdBasis.Rows.Count - 1];

					//Since we've already added the necessary information in the underlying
					//datatable, we want to cancel out because if we don't, the grid will
					//add another blank row (in addition to the row we just added to the datatable).

					e.Cancel = true;
				}

				//Expand the parent row (Basis) so the user can see the child row (Details).

				row = this.grdBasis.Rows[this.grdBasis.Rows.Count - 1];

				if (row.IsExpandable)
				{
					row.Expanded = true;
				}
			}
			catch (EditErrorException exc)
			{
				MessageBox.Show(exc.Message, "Edit error", MessageBoxButtons.OK);
			}
			catch (Exception exc)
			{
				HandleException(exc);
			}
			finally
			{
				this.Cursor = Cursors.Default;
			}
		}

		private void grdBasis_BeforeCellUpdate(object sender, Infragistics.Win.UltraWinGrid.BeforeCellUpdateEventArgs e)
		{
			try
			{
				//if the updated cell is the version dropdown column, get the selected
				//item's id and put it in the "VersionID" column.
				
				if (e.Cell.Band == grdBasis.DisplayLayout.Bands[1])
				{
					if (e.Cell.Column.Key == "Merchandise")
					{
						if (_skipEdit) 
						{
							return;
						}
						HierarchyNodeProfile hnp = GetNodeProfile(e.Cell.Text);
						if (hnp.Key == Include.NoRID)
						{
							string errorMessage = string.Format(_sab.ClientServerSession.Audit.GetText(eMIDTextCode.msg_InvalidTreeNode),
								e.Cell.Text );	
							MessageBox.Show( errorMessage);
							e.Cancel = true;
						}
					}
				}
			}
			catch (Exception exc)
			{
				HandleException(exc);
			}
		}

		private void grdBasis_AfterCellUpdate(object sender, Infragistics.Win.UltraWinGrid.CellEventArgs e)
		{
			try
			{
				//if the updated cell is the version dropdown column, get the selected
				//item's id and put it in the "VersionID" column.
				
				if (e.Cell.Band == grdBasis.DisplayLayout.Bands[1])
				{
					if (e.Cell == e.Cell.Row.Cells["Version"])
					{
						int selectedIndex = grdBasis.DisplayLayout.ValueLists["Version"].SelectedIndex;

						e.Cell.Row.Cells["VersionID"].Value = Convert.ToInt32(grdBasis.DisplayLayout.ValueLists["Version"].ValueListItems[selectedIndex].DataValue, CultureInfo.CurrentUICulture);
					}
					else if (e.Cell.Column.Key == "Merchandise")
					{
						if (_skipEdit) 
						{
							_skipEdit = false; 
							return;
						}
						_skipEdit = true;
						HierarchyNodeProfile hnp = GetNodeProfile(e.Cell.Text);
						e.Cell.Row.Cells["MerchandiseID"].Value = hnp.Key;
						e.Cell.Value = hnp.Text;
					}
				}
			}
			catch (Exception exc)
			{
				HandleException(exc);
			}
		}
		
		private void grdBasis_AfterRowInsert(object sender, Infragistics.Win.UltraWinGrid.RowEventArgs e)
		{
			try
			{
				//The "IsIncluded" column is a checkbox column. Right after we inserted a
				//new row, we want to default this column to be checked (true). If we
				//don't, it will default to a grayed-out check, like the 3rd state in a 
				//tri-state checkbox, even if we explicitly set this column to be a normal
				//checkbox.
				
				if (e.Row.Band == grdBasis.DisplayLayout.Bands[1])
				{
					e.Row.Cells["IsIncluded"].Value = true;
					e.Row.Cells["DateRangeID"].Value = Include.UndefinedCalendarDateRange;
					e.Row.Cells["IncludeButton"].Appearance.Image = _picInclude;

					//There is a bug where the grid is not refreshed to display the new
					//cell. We will manually close and expand its parent row to trick the 
					//grid to do a refresh.

					e.Row.ParentRow.Expanded = false;
					e.Row.ParentRow.Expanded = true;
				}
			}
			catch (Exception exc)
			{
				HandleException(exc);
			}
		}

		private void grdBasis_BeforeRowsDeleted(object sender, Infragistics.Win.UltraWinGrid.BeforeRowsDeletedEventArgs e)
		{
			int eMatchingID;
			int i;
			int j;

			try
			{
				grdBasis.UpdateData();
				_dsBasis.AcceptChanges();

				//We want to make sure that after the deletion, there are no "orphaned parent".
				//Every "Basis" row must have at least one "Details" row.

				if (e.Rows[0].Band == grdBasis.DisplayLayout.Bands[1])
				{
					//Infragistics UltraGrid doesn't allow the user to delete a parent-level
					//row and a child-level row at the same time. So by checking the first
					//item in the collection, we can pretty much guarantee that the rest of 
					//the collection is on the same leve.

					//store the number of rows with the same ID (from the delete collection).

					eMatchingID = 0; 

					for (i = 0; i < e.Rows.Length; i += eMatchingID)
					{
						DataView dv = new DataView();
						dv.Table = _dsBasis.Tables["BasisDetails"];
						dv.RowFilter = "BasisID = " + e.Rows[i].Cells["BasisID"].Value.ToString();

						//Count the number of rows in the delete rows collection that 
						//have the same BasisID

						eMatchingID = 0;

						for (j = i; j < e.Rows.Length; j++)
						{
							if (e.Rows[j].Cells["BasisID"].Value.ToString() ==
								e.Rows[i].Cells["BasisID"].Value.ToString())
								eMatchingID ++;
						}

						if (dv.Count == eMatchingID)
						{
							//the user is trying to delete ALL the details rows. Prevent it.

							e.DisplayPromptMsg = false;
							MessageBox.Show("The delete cannot be performed because it makes at least one Basis without Details.\r\n" + 
								"You must leave at least one Basis Detail for each Basis.\r\n\r\n" +
								"If you are sure you want to delete all the details, \r\ndelete the Basis itself.", "Error", MessageBoxButtons.OK);
							e.Cancel = true;
						}
					}
				}
			}
			catch (Exception exc)
			{
				HandleException(exc);
			}
		}

		private void grdBasis_AfterRowsDeleted(object sender, System.EventArgs e)
		{
			int i;

			try
			{
				//update row states so that the deleted row really DOES disappear.

				grdBasis.DataSource = null;

				_dsBasis.AcceptChanges();

				//if the user deleted a BASIS row, we want to re-assign captions and IDs
				//of the remaining Basis so that remaining Basis remain sequenced.
				//For example, say we have Basis 1, Basis 2, Basis 3, and Basis 4. If the
				//user deleted Basis 2, we want to rename Basis 3 and 4 so that after the
				//deletion, we still see Basis 1, 2, and 3 (not 1, 3, 4).

				for (i = 0; i < _dsBasis.Tables["Basis"].Rows.Count; i ++)
				{
					_dsBasis.Tables["Basis"].Rows[i]["BasisID"] = i;
					_dsBasis.Tables["Basis"].Rows[i]["BasisName"] = "Basis " + (i + 1);
				}

				_dsBasis.AcceptChanges();

				grdBasis.DataSource = _dsBasis;
				//Begin TT#1115 - JScott - Merchandise field missing from basis line in multi level view

				FormatBasisGrid();
				//End TT#1115 - JScott - Merchandise field missing from basis line in multi level view
			}
			catch (Exception exc)
			{
				HandleException(exc);
			}
		}
		
		private void grdBasis_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			Infragistics.Win.UIElement element;
			Point point;
			UltraGridRow row;

			try
			{
				//if the user clicked the right mouse, he/she probably wants to see the
				//context menu. We have only one item in the context menu: the "DELETE"
				//command. In order to delete a row, we must select the whole row first.

				if (e.Button == MouseButtons.Left)
				{
					return;
				}

				//get the row the mouse is on.
				//Get the GUI element where the mouse cursor is. (so that later on
				//we can retrieve the row and the cell based on the mouse location.)

				point = new Point(e.X, e.Y);
				element = grdBasis.DisplayLayout.UIElement.ElementFromPoint(point);

				if (element == null) 
				{					
					return;
				}

				//Retrieve the row where the mouse is

				row = (UltraGridRow)element.GetContext(typeof(UltraGridRow)); 

				if (row == null) 
				{
					return;
				}
			
				row.Selected = true;
			}
			catch (Exception exc)
			{
				HandleException(exc);
			}
		}

		private void grdBasis_AfterSelectChange(object sender, Infragistics.Win.UltraWinGrid.AfterSelectChangeEventArgs e)
		{
			bool HasBand0Rows = false;
			bool HasBand1Rows = false;

			try
			{
				//Validate: If the selected rows collection consists of rows from 
				//both the parent table AND the child table, unselect all rows
				//but don't display any error messages (it doesn't appear when 
				//you would expect it).
			
				if (grdBasis.Selected.Rows.Count >= 2)
				{
					//we're only interested if a row is selected. (not cells, not columns)

					foreach (UltraGridRow row in grdBasis.Selected.Rows)
					{
						if (row.Band.Index == 0)
						{
							HasBand0Rows = true;
						}
						else if (row.Band.Index == 1)
						{
							HasBand1Rows = true;
						}
					}

					if (HasBand0Rows == true && HasBand1Rows == true)
					{
						foreach (UltraGridRow row in grdBasis.Selected.Rows)
						{
							row.Selected = false;
						}
					}
				}
			}
			catch (Exception exc)
			{
				HandleException(exc);
			}
		}
		
		/// <summary>
		/// Get the cell where the mouse is. If (and only if) the cell is in 
		/// Band[0] (details grid) and the column is "Description", 
		/// set the effect to ALL.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		
		private void grdBasis_DragOver(object sender, System.Windows.Forms.DragEventArgs e)
		{
			TreeNodeClipboardList cbList;

			try
			{
				Image_DragOver(sender, e);
				Infragistics.Win.UIElement element;

				Point pt = PointToClient(new Point(e.X, e.Y));
				Point realPoint = new Point(pt.X - grdBasis.Location.X - pnlMain.Location.X, pt.Y - grdBasis.Location.Y - pnlMain.Location.Y);
					
				element = grdBasis.DisplayLayout.UIElement.ElementFromPoint(realPoint);

				if (element == null) 
				{
					e.Effect = DragDropEffects.None;
					return;
				}

				UltraGridRow row;
				row = (UltraGridRow)element.GetContext(typeof(UltraGridRow)); 

				if (row == null) 
				{
					e.Effect = DragDropEffects.None;
					return;
				}
				if (row.Band == grdBasis.DisplayLayout.Bands[0])
				{
					e.Effect = DragDropEffects.None;
					return;
				}

				UltraGridCell cell = (UltraGridCell)element.GetContext(typeof(UltraGridCell)); 
				if (cell == null) 
				{
					e.Effect = DragDropEffects.None;
					return;
				}
				if (cell.Band == grdBasis.DisplayLayout.Bands[0])
				{
					e.Effect = DragDropEffects.None;
					return;
				}
				if (cell == row.Cells["Merchandise"])
				{
					//e.Effect = DragDropEffects.All;
					e.Effect = DragDropEffects.None;
					if (e.Data.GetDataPresent(typeof(TreeNodeClipboardList)))
					{
						cbList = (TreeNodeClipboardList)e.Data.GetData(typeof(TreeNodeClipboardList));
						if (cbList.ClipboardDataType == eProfileType.HierarchyNode)
						{
							e.Effect = DragDropEffects.All;
						}
					}
				}
				else
				{
					e.Effect = DragDropEffects.None;
				}
			}
			catch (Exception exc)
			{
				HandleException(exc);
			}
		}

		private void grdBasis_DragDrop(object sender, System.Windows.Forms.DragEventArgs e)
		{
			Infragistics.Win.UIElement element;
			Point point;
			Point realPoint;
			UltraGridRow row;
			UltraGridCell cell;
			//IDataObject data;
			//ClipboardProfile cbp;
			//HierarchyClipboardData MIDTreeNode_cbd;
			TreeNodeClipboardList cbList = null;

			try
			{
				point = PointToClient(new Point(e.X, e.Y));
				realPoint = new Point(point.X - grdBasis.Location.X - pnlMain.Location.X, point.Y - grdBasis.Location.Y - pnlMain.Location.Y);
				element = grdBasis.DisplayLayout.UIElement.ElementFromPoint(realPoint);

				if (element == null) 
				{
					return;
				}

				row = (UltraGridRow)element.GetContext(typeof(UltraGridRow)); 

				if (row == null) 
				{
					return;
				}

				if (row.Band == grdBasis.DisplayLayout.Bands[0])
				{
					return;
				}

				cell = (UltraGridCell)element.GetContext(typeof(UltraGridCell)); 

				if (cell == null) 
				{
					return;
				}

				if (cell.Band == grdBasis.DisplayLayout.Bands[0])
				{
					return;
				}

				if (cell == row.Cells["Merchandise"])
				{
					//// Create a new instance of the DataObject interface.

					//data = Clipboard.GetDataObject();

					//If the data is ClipboardProfile, then retrieve the data

					if (e.Data.GetDataPresent(typeof(TreeNodeClipboardList)))
					{
						cbList = (TreeNodeClipboardList)e.Data.GetData(typeof(TreeNodeClipboardList));
						//if (cbp.ClipboardDataType == eClipboardDataType.HierarchyNode)
						if (cbList.ClipboardDataType == eProfileType.HierarchyNode)
						{
							//if (cbp.ClipboardData.GetType() == typeof(HierarchyClipboardData))
							//{
								_skipEdit = true;
								//MIDTreeNode_cbd = (HierarchyClipboardData)cbp.ClipboardData;
								//Begin Track #5378 - color and size not qualified
//								HierarchyNodeProfile hnp = _sab.HierarchyServerSession.GetNodeData(cbp.Key);
								HierarchyNodeProfile hnp = _sab.HierarchyServerSession.GetNodeData(cbList.ClipboardProfile.Key, true, true);
								//End Track #5378
								cell.Value = hnp.Text;
								row.Cells["MerchandiseID"].Value = hnp.Key;
								_skipEdit = false;
							//}
							//else
							//{
							//    MessageBox.Show(_sab.ClientServerSession.Audit.GetText(eMIDTextCode.msg_BadDataInClipboard));
							//}
						}
						else
						{
							MessageBox.Show(_sab.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MustBeNodeToDrop));
						}
					}
					else
					{
						MessageBox.Show(_sab.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MustBeNodeToDrop));
					}
				}
			}
			catch (Exception exc)
			{
				HandleException(exc);
			}
		}
		
		private void grdBasis_ClickCellButton(object sender, Infragistics.Win.UltraWinGrid.CellEventArgs e)
		{
			CalendarDateSelector frmCalDtSelector;
			DialogResult dateRangeResult;
			DateRangeProfile selectedDateRange;

			try
			{
				switch (e.Cell.Column.Key)
				{
					case "DateRange":

						frmCalDtSelector = (CalendarDateSelector)CreateControl(typeof(CalendarDateSelector), new object[]{_sab});

						if (e.Cell.Row.Cells["DateRange"].Value != null &&
							e.Cell.Row.Cells["DateRange"].Value != System.DBNull.Value &&
							e.Cell.Row.Cells["DateRange"].Text.Length > 0)
						{
							frmCalDtSelector.DateRangeRID = Convert.ToInt32(e.Cell.Row.Cells["DateRangeID"].Value, CultureInfo.CurrentUICulture);
						}

						if (mdsSSLPlanDateRange.DateRangeRID != Include.UndefinedCalendarDateRange)
						{
							frmCalDtSelector.AnchorDateRangeRID = mdsSSLPlanDateRange.DateRangeRID;
						}
						else
						{
							frmCalDtSelector.AnchorDate = _sab.ClientServerSession.Calendar.CurrentDate;
						}

						frmCalDtSelector.AnchorDateRelativeTo = eDateRangeRelativeTo.Plan;
						frmCalDtSelector.AllowDynamicToStoreOpen = false;

						dateRangeResult = frmCalDtSelector.ShowDialog();

						if (dateRangeResult == DialogResult.OK)
						{
							selectedDateRange = (DateRangeProfile)frmCalDtSelector.Tag;

							e.Cell.Value = selectedDateRange.DisplayDate;
							e.Cell.Row.Cells["DateRangeID"].Value = selectedDateRange.Key;

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
						grdBasis.PerformAction(UltraGridAction.DeactivateCell);

//						frmCalDtSelector.Remove();

						break;

					case "IncludeButton":

						if (Convert.ToBoolean(e.Cell.Row.Cells["IsIncluded"].Value, CultureInfo.CurrentUICulture))
						{
							e.Cell.Row.Cells["IsIncluded"].Value = false;
							e.Cell.Appearance.Image = _picExclude;
						}
						else
						{
							e.Cell.Row.Cells["IsIncluded"].Value = true;
							e.Cell.Appearance.Image = _picInclude;
						}

						e.Cell.CancelUpdate();
						grdBasis.PerformAction(UltraGridAction.DeactivateCell);

						break;
				}
			}
			catch(Exception exception)
			{
				HandleException(exception);
			}
		}
		#endregion

		#region TextBox Events

		private void txtSSLStoreNode_TextChanged(object sender, System.EventArgs e)
		{
			try
			{
				this.Text = MIDText.GetTextOnly(eMIDTextCode.frm_OTSPlanSelection) + " - " + txtSSLStoreNode.Text;
//Begin Track #5081 - JScott - Paste into Chain does not work
				_textChanged = true;
//End Track #5081 - JScott - Paste into Chain does not work
			}
			catch (Exception exc)
			{
				HandleException(exc);
			}
		}

		private void txtNode_DragEnter(object sender, System.Windows.Forms.DragEventArgs e)
		{
			Merchandise_DragEnter(sender, e);
		}

		private void txtNode_DragOver(object sender, DragEventArgs e)
		{
			Image_DragOver(sender, e);
		}

		private void txtNode_DragDrop(object sender, System.Windows.Forms.DragEventArgs e)
		{
			//IDataObject data;
			//ClipboardProfile cbp;
			//HierarchyClipboardData MIDTreeNode_cbd;

			try
			{
				//                // Create a new instance of the DataObject interface.

				//                data = Clipboard.GetDataObject();

				//                //If the data is ClipboardProfile, then retrieve the data

				//                if (data.GetDataPresent(ClipboardProfile.Format.Name))
				//                {
				//                    cbp = (ClipboardProfile)data.GetData(ClipboardProfile.Format.Name);

				//                    //if (cbp.ClipboardDataType == eClipboardDataType.HierarchyNode)
				//                    if (cbp.ClipboardDataType == eProfileType.HierarchyNode)
				//                    {
				//                        if (cbp.ClipboardData.GetType() == typeof(HierarchyClipboardData))
				//                        {
				//                            MIDTreeNode_cbd = (HierarchyClipboardData)cbp.ClipboardData;
				//                            //Begin Track #5378 - color and size not qualified
				////							HierarchyNodeProfile hnp = _sab.HierarchyServerSession.GetNodeData(cbp.Key);
				//                            HierarchyNodeProfile hnp = _sab.HierarchyServerSession.GetNodeData(cbp.Key, true, true);
				//Begin Track #5858 - Kjohnson - Validating store security only
				bool isSuccessfull = ((MIDTextBoxTag)(((TextBox)sender).Tag)).TextBox_DragDrop(sender, e);

				if (isSuccessfull)
				{
					HierarchyNodeProfile hnp = (HierarchyNodeProfile)((MIDTag)(((TextBox)sender).Tag)).MIDTagData;
					//End Track #5378
					//Begin Track #5858 - KJohnson - Validating store security only
					((TextBox)sender).Text = hnp.Text;
					//((MIDTag)((TextBox)sender).Tag).MIDTagData = hnp;
					//End Track #5858

					//Begin Track #6099 - JSmith - Override low-level has wrong data after keying new node
                    cboSMLOverride.SelectedItem = null;
                    cboCMLOverride.SelectedItem = null;
					//End Track #6099

					switch (((TextBox)sender).Name)
					{
						case "txtSSLStoreNode":
							_openParms.StoreHLPlanProfile.NodeProfile = hnp;
							txtSMLHighLevelNode.Text = _openParms.StoreHLPlanProfile.NodeProfile.Text;
							////Begin Track #5858 - JSmith - Validating store security only
							////txtSMLHighLevelNode.Tag = hnp;
							//((MIDTag)(txtSMLHighLevelNode.Tag)).MIDTagData = hnp;
							////End Track #5858
                            PopulateLowLevels(_openParms.StoreHLPlanProfile.NodeProfile, cboSMLLowLevels.ComboBox);
							break;
						case "txtSSLChainNode":
							_openParms.ChainHLPlanProfile.NodeProfile = hnp;
							txtCSLChainNode.Text = hnp.Text;
							////Begin Track #5858 - JSmith - Validating store security only
							////txtCSLChainNode.Tag = hnp;
							//((MIDTag)txtCSLChainNode.Tag).MIDTagData = hnp;
							////End Track #5858
							txtCMLHighLevelNode.Text = hnp.Text;
							////Begin Track #5858 - JSmith - Validating store security only
							////txtCMLHighLevelNode.Tag = hnp;
							//((MIDTag)txtCMLHighLevelNode.Tag).MIDTagData = hnp;
							////End Track #5858
							PopulateLowLevels(hnp, cboCMLLowLevels.ComboBox);
							break;
						case "txtSMLHighLevelNode":
							_openParms.StoreHLPlanProfile.NodeProfile = hnp;
							_openParms.ChainHLPlanProfile.NodeProfile = hnp;
							txtSSLStoreNode.Text = hnp.Text;
							////Begin Track #5858 - JSmith - Validating store security only
							////txtSSLStoreNode.Tag = hnp;
							//((MIDTag)txtSSLStoreNode.Tag).MIDTagData = hnp;
							////End Track #5858
                            PopulateLowLevels(hnp, cboSMLLowLevels.ComboBox);
							break;
						case "txtCSLChainNode":
							_openParms.ChainHLPlanProfile.NodeProfile = hnp;
							txtSSLChainNode.Text = hnp.Text;
							////Begin Track #5858 - JSmith - Validating store security only
							////txtSSLChainNode.Tag = hnp;
							//((MIDTag)txtSSLChainNode.Tag).MIDTagData = hnp;
							////End Track #5858
							txtCMLHighLevelNode.Text = hnp.Text;
							////Begin Track #5858 - JSmith - Validating store security only
							////txtCMLHighLevelNode.Tag = hnp;
							//((MIDTag)txtCMLHighLevelNode.Tag).MIDTagData = hnp;
							////End Track #5858
							PopulateLowLevels(hnp, cboCMLLowLevels.ComboBox);
							break;
						case "txtCMLHighLevelNode":
							_openParms.ChainHLPlanProfile.NodeProfile = hnp;
							txtSSLChainNode.Text = hnp.Text;
							////Begin Track #5858 - JSmith - Validating store security only
							////txtSSLChainNode.Tag = hnp;
							//((MIDTag)txtSSLChainNode.Tag).MIDTagData = hnp;
							////End Track #5858
							txtCSLChainNode.Text = hnp.Text;
							////Begin Track #5858 - JSmith - Validating store security only
							////txtCSLChainNode.Tag = hnp;
							//((MIDTag)txtCSLChainNode.Tag).MIDTagData = hnp;
							////End Track #5858
							PopulateLowLevels(hnp, cboCMLLowLevels.ComboBox);
							break;
					}
				}
				else
				{
					MessageBox.Show(_sab.ClientServerSession.Audit.GetText(eMIDTextCode.msg_BadDataInClipboard));
				}
				//    }
				//    else
				//    {
				//        MessageBox.Show(_sab.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MustBeNodeToDrop));
				//    }
				//}
				//else
				//{
				//    MessageBox.Show(_sab.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MustBeNodeToDrop));
				//}
			}
			catch (Exception exc)
			{
				HandleException(exc);
			}
		}

//Begin Track #5081 - JScott - Paste into Chain does not work
//		private void txtNode_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
//		{
//			_textChanged = true;
//		}
		private void txtNode_TextChanged(object sender, System.EventArgs e)
		{
			// Begin Track #5733 - JSmith - Title not correct for chain plan
			if (_openParms.PlanSessionType == ePlanSessionType.ChainSingleLevel)
			{
				this.Text = MIDText.GetTextOnly(eMIDTextCode.frm_OTSPlanSelection) + " - " + this.txtCSLChainNode.Text;
			}
			else if (_openParms.PlanSessionType == ePlanSessionType.ChainMultiLevel)
			{
				this.Text = MIDText.GetTextOnly(eMIDTextCode.frm_OTSPlanSelection) + " - " + this.txtCMLHighLevelNode.Text;
			}
			// End Track #5733
			_textChanged = true;
		}
//End Track #5081 - JScott - Paste into Chain does not work

		private void txtNode_Validating(object sender, System.ComponentModel.CancelEventArgs e)
		{
			//string errorMessage;

			//try
			//{
			//    //Begin Track #5858 - JSmith - Validating store security only
			//    //if (((TextBox)sender).Text == string.Empty && ((TextBox)sender).Tag != null)
			//    if (((TextBox)sender).Text == string.Empty && ((MIDTag)((TextBox)sender).Tag).MIDTagData != null)
			//    //End Track #5858
			//    {
			//        ((TextBox)sender).Text = string.Empty;
			//        //Begin Track #5858 - JSmith - Validating store security only
			//        //((TextBox)sender).Tag = hnp;
			//        ((MIDTag)((TextBox)sender).Tag).MIDTagData = null;
			//        //End Track #5858
			//        switch (((TextBox)sender).Name)
			//        {
			//            case "txtSSLStoreNode":
			//                _openParms.StoreHLPlanProfile.NodeProfile = null;
			//                txtSMLHighLevelNode.Text = string.Empty;
			//                //Begin Track #5858 - JSmith - Validating store security only
			//                //txtSMLHighLevelNode.Tag = hnp;
			//                ((MIDTag)txtSMLHighLevelNode.Tag).MIDTagData = null;
			//                //End Track #5858
			//                PopulateLowLevels(null, cboSMLLowLevels);
			//                break;
			//            case "txtSSLChainNode":
			//                _openParms.ChainHLPlanProfile.NodeProfile = null;
			//                txtCSLChainNode.Text = string.Empty;
			//                //Begin Track #5858 - JSmith - Validating store security only
			//                //txtCSLChainNode.Tag = hnp;
			//                ((MIDTag)txtCSLChainNode.Tag).MIDTagData = null;
			//                //End Track #5858
			//                txtCMLHighLevelNode.Text = string.Empty;
			//                //Begin Track #5858 - JSmith - Validating store security only
			//                //txtCMLHighLevelNode.Tag = hnp;
			//                ((MIDTag)txtCMLHighLevelNode.Tag).MIDTagData = null;
			//                //End Track #5858
			//                PopulateLowLevels(null, cboCMLLowLevels);
			//                break;
			//            case "txtSMLHighLevelNode":
			//                _openParms.StoreHLPlanProfile.NodeProfile = null;
			//                _openParms.ChainHLPlanProfile.NodeProfile = null;
			//                txtSSLStoreNode.Text = string.Empty;
			//                //Begin Track #5858 - JSmith - Validating store security only
			//                //txtSSLStoreNode.Tag = hnp;
			//                ((MIDTag)txtSSLStoreNode.Tag).MIDTagData = null;
			//                //End Track #5858
			//                PopulateLowLevels(null, cboSMLLowLevels);
			//                break;
			//            case "txtCSLChainNode":
			//                _openParms.ChainHLPlanProfile.NodeProfile = null;
			//                txtSSLChainNode.Text = string.Empty;
			//                //Begin Track #5858 - JSmith - Validating store security only
			//                //txtSSLChainNode.Tag = hnp;
			//                ((MIDTag)txtSSLChainNode.Tag).MIDTagData = null;
			//                //End Track #5858
			//                txtCMLHighLevelNode.Text = string.Empty;
			//                //Begin Track #5858 - JSmith - Validating store security only
			//                //txtCMLHighLevelNode.Tag = hnp;
			//                ((MIDTag)txtCMLHighLevelNode.Tag).MIDTagData = null;
			//                //End Track #5858
			//                PopulateLowLevels(null, cboCMLLowLevels);
			//                break;
			//            case "txtCMLHighLevelNode":
			//                _openParms.ChainHLPlanProfile.NodeProfile = null;
			//                txtSSLChainNode.Text = string.Empty;
			//                //Begin Track #5858 - JSmith - Validating store security only
			//                //txtSSLChainNode.Tag = hnp;
			//                ((MIDTag)txtSSLChainNode.Tag).MIDTagData = null;
			//                //End Track #5858
			//                txtCSLChainNode.Text = string.Empty;
			//                //Begin Track #5858 - JSmith - Validating store security only
			//                //txtCSLChainNode.Tag = hnp;
			//                ((MIDTag)txtCSLChainNode.Tag).MIDTagData = null;
			//                //End Track #5858
			//                PopulateLowLevels(null, cboCMLLowLevels);
			//                break;
			//        }
			//    }
			//    else
			//    {
			//        if (_textChanged)
			//        {
			//            _textChanged = false;

			//            HierarchyNodeProfile hnp = GetNodeProfile(((TextBox)sender).Text);
			//            if (hnp.Key == Include.NoRID)
			//            {
			//                _priorError = true;

			//                //Begin Track #5858 - JSmith - Validating store security only
			//                //errorMessage = string.Format(_sab.ClientServerSession.Audit.GetText(eMIDTextCode.msg_InvalidTreeNode),
			//                //    ((TextBox)sender).Text );	
			//                //ErrorProvider.SetError((TextBox)sender,errorMessage);
			//                //MessageBox.Show( errorMessage);
			//                //End Track #5858

			//                e.Cancel = true;
			//            }
			//            else 
			//            {
			//                ((TextBox)sender).Text = hnp.Text;
			//                //Begin Track #5858 - JSmith - Validating store security only
			//                //((TextBox)sender).Tag = hnp;
			//                ((MIDTag)((TextBox)sender).Tag).MIDTagData = hnp;
			//                //End Track #5858
			//                switch (((TextBox)sender).Name)
			//                {
			//                    case "txtSSLStoreNode":
			//                        _openParms.StoreHLPlanProfile.NodeProfile = hnp;
			//                        txtSMLHighLevelNode.Text = _openParms.StoreHLPlanProfile.NodeProfile.Text;
			//                        //Begin Track #5858 - JSmith - Validating store security only
			//                        //txtSMLHighLevelNode.Tag = hnp;
			//                        ((MIDTag)txtSMLHighLevelNode.Tag).MIDTagData = hnp;
			//                        //End Track #5858
			//                        PopulateLowLevels(_openParms.StoreHLPlanProfile.NodeProfile, cboSMLLowLevels);
			//                        break;
			//                    case "txtSSLChainNode":
			//                        _openParms.ChainHLPlanProfile.NodeProfile = hnp;
			//                        txtCSLChainNode.Text = hnp.Text;
			//                        //Begin Track #5858 - JSmith - Validating store security only
			//                        //txtCSLChainNode.Tag = hnp;
			//                        ((MIDTag)txtCSLChainNode.Tag).MIDTagData = hnp;
			//                        //End Track #5858
			//                        txtCMLHighLevelNode.Text = hnp.Text;
			//                        //Begin Track #5858 - JSmith - Validating store security only
			//                        //txtCMLHighLevelNode.Tag = hnp;
			//                        ((MIDTag)txtCMLHighLevelNode.Tag).MIDTagData = hnp;
			//                        //End Track #5858
			//                        PopulateLowLevels(hnp, cboCMLLowLevels);
			//                        break;
			//                    case "txtSMLHighLevelNode":
			//                        _openParms.StoreHLPlanProfile.NodeProfile = hnp;
			//                        _openParms.ChainHLPlanProfile.NodeProfile = hnp;
			//                        txtSSLStoreNode.Text = hnp.Text;
			//                        //Begin Track #5858 - JSmith - Validating store security only
			//                        //txtSSLStoreNode.Tag = hnp;
			//                        ((MIDTag)txtSSLStoreNode.Tag).MIDTagData = hnp;
			//                        //End Track #5858
			//                        PopulateLowLevels(hnp, cboSMLLowLevels);
			//                        break;
			//                    case "txtCSLChainNode":
			//                        _openParms.ChainHLPlanProfile.NodeProfile = hnp;
			//                        txtSSLChainNode.Text = hnp.Text;
			//                        //Begin Track #5858 - JSmith - Validating store security only
			//                        //txtSSLChainNode.Tag = hnp;
			//                        ((MIDTag)txtSSLChainNode.Tag).MIDTagData = hnp;
			//                        //End Track #5858
			//                        txtCMLHighLevelNode.Text = hnp.Text;
			//                        //Begin Track #5858 - JSmith - Validating store security only
			//                        //txtCMLHighLevelNode.Tag = hnp;
			//                        ((MIDTag)txtCMLHighLevelNode.Tag).MIDTagData = hnp;
			//                        //End Track #5858
			//                        PopulateLowLevels(hnp, cboCMLLowLevels);
			//                        break;
			//                    case "txtCMLHighLevelNode":
			//                        _openParms.ChainHLPlanProfile.NodeProfile = hnp;
			//                        txtSSLChainNode.Text = hnp.Text;
			//                        //Begin Track #5858 - JSmith - Validating store security only
			//                        //txtSSLChainNode.Tag = hnp;
			//                        ((MIDTag)txtSSLChainNode.Tag).MIDTagData = hnp;
			//                        //End Track #5858
			//                        txtCSLChainNode.Text = hnp.Text;
			//                        //Begin Track #5858 - JSmith - Validating store security only
			//                        //txtCSLChainNode.Tag = hnp;
			//                        ((MIDTag)txtCSLChainNode.Tag).MIDTagData = hnp;
			//                        //End Track #5858
			//                        PopulateLowLevels(hnp, cboCMLLowLevels);
			//                        break;
			//                }
			//            }	
			//        }
			//        else if (_priorError)
			//        {
			//            //Begin Track #5858 - JSmith - Validating store security only
			//            //if (((TextBox)sender).Tag == null)
			//            if (((MIDTag)((TextBox)sender).Tag).MIDTagData == null)
			//            //End Track #5858
			//            {
			//                ((TextBox)sender).Text = string.Empty;
			//            }
			//            else
			//            {
			//                //Begin Track #5858 - JSmith - Validating store security only
			//                //((TextBox)sender).Text = ((HierarchyNodeProfile)((TextBox)sender).Tag).Text;
			//                ((TextBox)sender).Text = ((HierarchyNodeProfile)((MIDTag)((TextBox)sender).Tag).MIDTagData).Text;
			//                //End Track #5858
			//            }
			//        }
			//    }
			//}
			//catch(Exception ex)
			//{
			//    HandleException(ex);
			//}
		}
	
		private void txtNode_Validated(object sender, System.EventArgs e)
		{
			try
			{
				_textChanged = false;
				_priorError = false;
				//Begin Track #6099 - JSmith - Override low-level has wrong data after keying new node
				if (FormLoaded)
				{
                    cboSMLOverride.SelectedItem = null;
                    cboCMLOverride.SelectedItem = null;
					_openParms.OverrideLowLevelRid = Include.NoRID;
					//Begin Track #6099 - KJohnson - Override low-level has wrong data after keying new node
					_openParms.CustomOverrideLowLevelRid = Include.NoRID;
					//End Track #6099 - KJohnson
				}
				//End Track #6099

				//Begin Track #5858 - KJohnson- Validating store security only
				if ((((TextBox)sender).Text.Trim() == string.Empty) && (((TextBox)sender).Tag != null))
				{
					switch (((TextBox)sender).Name)
					{
						case "txtSSLStoreNode":
							_openParms.StoreHLPlanProfile.NodeProfile = null;
							txtSMLHighLevelNode.Text = string.Empty;
                            PopulateLowLevels(null, cboSMLLowLevels.ComboBox);
							break;
						case "txtSSLChainNode":
							_openParms.ChainHLPlanProfile.NodeProfile = null;
							txtCSLChainNode.Text = string.Empty;
							//Begin Track #5858 - JSmith - Validating store security only
							//txtCSLChainNode.Tag = hnp;
							((MIDTag)txtCSLChainNode.Tag).MIDTagData = null;
							//End Track #5858
							txtCMLHighLevelNode.Text = string.Empty;
							//Begin Track #5858 - JSmith - Validating store security only
							//txtCMLHighLevelNode.Tag = hnp;
							((MIDTag)txtCMLHighLevelNode.Tag).MIDTagData = null;
							//End Track #5858
							PopulateLowLevels(null, cboCMLLowLevels.ComboBox);
							break;
						case "txtSMLHighLevelNode":
							_openParms.StoreHLPlanProfile.NodeProfile = null;
							_openParms.ChainHLPlanProfile.NodeProfile = null;
							txtSSLStoreNode.Text = string.Empty;
							//Begin Track #5858 - JSmith - Validating store security only
							//txtSSLStoreNode.Tag = hnp;
							((MIDTag)txtSSLStoreNode.Tag).MIDTagData = null;
							//End Track #5858
                            PopulateLowLevels(null, cboSMLLowLevels.ComboBox);
							break;
						case "txtCSLChainNode":
							_openParms.ChainHLPlanProfile.NodeProfile = null;
							txtSSLChainNode.Text = string.Empty;
							//Begin Track #5858 - JSmith - Validating store security only
							//txtSSLChainNode.Tag = hnp;
							((MIDTag)txtSSLChainNode.Tag).MIDTagData = null;
							//End Track #5858
							txtCMLHighLevelNode.Text = string.Empty;
							//Begin Track #5858 - JSmith - Validating store security only
							//txtCMLHighLevelNode.Tag = hnp;
							((MIDTag)txtCMLHighLevelNode.Tag).MIDTagData = null;
							//End Track #5858
							PopulateLowLevels(null, cboCMLLowLevels.ComboBox);
							break;
						case "txtCMLHighLevelNode":
							_openParms.ChainHLPlanProfile.NodeProfile = null;
							txtSSLChainNode.Text = string.Empty;
							//Begin Track #5858 - JSmith - Validating store security only
							//txtSSLChainNode.Tag = hnp;
							((MIDTag)txtSSLChainNode.Tag).MIDTagData = null;
							//End Track #5858
							txtCSLChainNode.Text = string.Empty;
							//Begin Track #5858 - JSmith - Validating store security only
							//txtCSLChainNode.Tag = hnp;
							((MIDTag)txtCSLChainNode.Tag).MIDTagData = null;
							//End Track #5858
							PopulateLowLevels(null, cboCMLLowLevels.ComboBox);
							break;
					}
				}
				else 
				{
					HierarchyNodeProfile hnp = (HierarchyNodeProfile)((MIDTag)((TextBox)sender).Tag).MIDTagData;
					switch (((TextBox)sender).Name)
					{
						case "txtSSLStoreNode":
							_openParms.StoreHLPlanProfile.NodeProfile = hnp;
							txtSMLHighLevelNode.Text = _openParms.StoreHLPlanProfile.NodeProfile.Text;
							//Begin Track #5858 - JSmith - Validating store security only
							//txtSMLHighLevelNode.Tag = hnp;
							((MIDTag)txtSMLHighLevelNode.Tag).MIDTagData = hnp;
							//End Track #5858
                            PopulateLowLevels(_openParms.StoreHLPlanProfile.NodeProfile, cboSMLLowLevels.ComboBox);
							break;
						case "txtSSLChainNode":
							_openParms.ChainHLPlanProfile.NodeProfile = hnp;
							txtCSLChainNode.Text = hnp.Text;
							//Begin Track #5858 - JSmith - Validating store security only
							//txtCSLChainNode.Tag = hnp;
							((MIDTag)txtCSLChainNode.Tag).MIDTagData = hnp;
							//End Track #5858
							txtCMLHighLevelNode.Text = hnp.Text;
							//Begin Track #5858 - JSmith - Validating store security only
							//txtCMLHighLevelNode.Tag = hnp;
							((MIDTag)txtCMLHighLevelNode.Tag).MIDTagData = hnp;
							//End Track #5858
							PopulateLowLevels(hnp, cboCMLLowLevels.ComboBox);
							break;
						case "txtSMLHighLevelNode":
							_openParms.StoreHLPlanProfile.NodeProfile = hnp;
							_openParms.ChainHLPlanProfile.NodeProfile = hnp;
							txtSSLStoreNode.Text = hnp.Text;
							//Begin Track #5858 - JSmith - Validating store security only
							//txtSSLStoreNode.Tag = hnp;
							((MIDTag)txtSSLStoreNode.Tag).MIDTagData = hnp;
							//End Track #5858
                            PopulateLowLevels(hnp, cboSMLLowLevels.ComboBox);
							break;
						case "txtCSLChainNode":
							_openParms.ChainHLPlanProfile.NodeProfile = hnp;
							txtSSLChainNode.Text = hnp.Text;
							//Begin Track #5858 - JSmith - Validating store security only
							//txtSSLChainNode.Tag = hnp;
							((MIDTag)txtSSLChainNode.Tag).MIDTagData = hnp;
							//End Track #5858
							txtCMLHighLevelNode.Text = hnp.Text;
							//Begin Track #5858 - JSmith - Validating store security only
							//txtCMLHighLevelNode.Tag = hnp;
							((MIDTag)txtCMLHighLevelNode.Tag).MIDTagData = hnp;
							//End Track #5858
							PopulateLowLevels(hnp, cboCMLLowLevels.ComboBox);
							break;
						case "txtCMLHighLevelNode":
							_openParms.ChainHLPlanProfile.NodeProfile = hnp;
							txtSSLChainNode.Text = hnp.Text;
							//Begin Track #5858 - JSmith - Validating store security only
							//txtSSLChainNode.Tag = hnp;
							((MIDTag)txtSSLChainNode.Tag).MIDTagData = hnp;
							//End Track #5858
							txtCSLChainNode.Text = hnp.Text;
							//Begin Track #5858 - JSmith - Validating store security only
							//txtCSLChainNode.Tag = hnp;
							((MIDTag)txtCSLChainNode.Tag).MIDTagData = hnp;
							//End Track #5858
							PopulateLowLevels(hnp, cboCMLLowLevels.ComboBox);
							break;
					}
				}
				//End Track #5858
			}
			catch (Exception)
			{
				throw;
			}
		}

		private HierarchyNodeProfile GetNodeProfile(string aProductID)
		{
			string desc = string.Empty;
			try
			{
				string productID = aProductID.Trim();
				string[] pArray = productID.Split(new char[] {'['});
				productID = pArray[0].Trim(); 
//				return _sab.HierarchyServerSession.GetNodeData(productID);
				HierarchyMaintenance hm = new HierarchyMaintenance(_sab);
				EditMsgs em = new EditMsgs();
				return hm.NodeLookup(ref em, productID, false);
			}
			catch(Exception ex)
			{
				HandleException(ex);
				throw;
			}
		}

		#endregion

		#region MIDDateRangeSelector Events

		private void mdsSSLPlanDateRange_Click(object sender, System.EventArgs e)
		{
			try
			{
				CalendarDateSelector frm = (CalendarDateSelector)CreateControl(typeof(CalendarDateSelector), new object[]{_sab});
				mdsSSLPlanDateRange.DateRangeForm = frm;
				frm.DateRangeRID = mdsSSLPlanDateRange.DateRangeRID;
				frm.AllowDynamicToPlan = false;
				frm.AllowDynamicToStoreOpen = false;
				frm.DefaultToStatic = true;
				frm.AllowDynamicSwitch = true;
				mdsSSLPlanDateRange.ShowSelector();
			}
			catch (Exception exc)
			{
				HandleException(exc);
			}
		}

		private void mdsSMLPlanDateRange_Click(object sender, System.EventArgs e)
		{
			try
			{
				CalendarDateSelector frm = (CalendarDateSelector)CreateControl(typeof(CalendarDateSelector), new object[]{_sab});
				mdsSMLPlanDateRange.DateRangeForm = frm;
				frm.DateRangeRID = mdsSMLPlanDateRange.DateRangeRID;
				frm.AllowDynamicToPlan = false;
				frm.AllowDynamicToStoreOpen = false;
				frm.DefaultToStatic = true;
				frm.AllowDynamicSwitch = true;
				mdsSMLPlanDateRange.ShowSelector();
			}
			catch (Exception exc)
			{
				HandleException(exc);
			}
		}

		private void mdsCSLPlanDateRange_Click(object sender, System.EventArgs e)
		{
			try
			{
				CalendarDateSelector frm = (CalendarDateSelector)CreateControl(typeof(CalendarDateSelector), new object[]{_sab});
				mdsCSLPlanDateRange.DateRangeForm = frm;
				frm.DateRangeRID = mdsCSLPlanDateRange.DateRangeRID;
				frm.AllowDynamicToPlan = false;
				frm.AllowDynamicToStoreOpen = false;
				frm.DefaultToStatic = true;
				frm.AllowDynamicSwitch = true;
				mdsCSLPlanDateRange.ShowSelector();
			}
			catch (Exception exc)
			{
				HandleException(exc);
			}
		}

		private void mdsCMLPlanDateRange_Click(object sender, System.EventArgs e)
		{
			try
			{
				CalendarDateSelector frm = (CalendarDateSelector)CreateControl(typeof(CalendarDateSelector), new object[]{_sab});
				mdsCMLPlanDateRange.DateRangeForm = frm;
				frm.DateRangeRID = mdsCMLPlanDateRange.DateRangeRID;
				frm.AllowDynamicToPlan = false;
				frm.AllowDynamicToStoreOpen = false;
				frm.DefaultToStatic = true;
				frm.AllowDynamicSwitch = true;
				mdsCMLPlanDateRange.ShowSelector();
			}
			catch (Exception exc)
			{
				HandleException(exc);
			}
		}

		private void mdsPlanDateRange_OnSelection(object source, MIDRetail.Windows.Controls.DateRangeSelectorEventArgs e)
		{
			try
			{
				if (e.SelectedDateRange != null)
				{
					LoadDateRangeSelector(mdsSSLPlanDateRange, e.SelectedDateRange);
					LoadDateRangeSelector(mdsSMLPlanDateRange, e.SelectedDateRange);
					LoadDateRangeSelector(mdsCSLPlanDateRange, e.SelectedDateRange);
					LoadDateRangeSelector(mdsCMLPlanDateRange, e.SelectedDateRange);
					SetBasisDates();
					grdBasis.Refresh();
				}
			}
			catch (Exception exc)
			{
				HandleException(exc);
			}
		}
		
		#endregion

		#region Misc Events

		private void mnuDelete_Click(object sender, System.EventArgs e)
		{
			try
			{
				grdBasis.DeleteSelectedRows();
			}
			catch (Exception exc)
			{
				HandleException(exc);
			}
		}

		private void cboVersion_SelectionChangeCommitted(object sender, System.EventArgs e)
		{
			try
			{
				//When the user selects a different store version, we want the chain
				//version to change to the same value. But the user still has the option
				//to manually change the chain version to something different.
                if (((ComboBox)sender).SelectedItem != null)
				{
                    switch (((ComboBox)sender).Name)
					{
						case "cboSSLStoreVersion":
                            SetStoreVersions(((ComboBox)sender).SelectedItem);
                            SetLowLevelStoreVersions(((ComboBox)sender).SelectedItem);
                            SetChainVersions(((ComboBox)sender).SelectedItem);
                            SetLowLevelChainVersions(((ComboBox)sender).SelectedItem);
							break;
						case "cboSSLChainVersion":
                            SetChainVersions(((ComboBox)sender).SelectedItem);
                            SetLowLevelChainVersions(((ComboBox)sender).SelectedItem);
							break;
						case "cboSMLHighLevelVersion":
                            SetStoreVersions(((ComboBox)sender).SelectedItem);
                            SetLowLevelStoreVersions(((ComboBox)sender).SelectedItem);
                            SetChainVersions(((ComboBox)sender).SelectedItem);
                            SetLowLevelChainVersions(((ComboBox)sender).SelectedItem);
							break;
						case "cboSMLLowLevelsVersion":
							break;
						case "cboCSLChainVersion":
                            SetChainVersions(((ComboBox)sender).SelectedItem);
                            SetLowLevelChainVersions(((ComboBox)sender).SelectedItem);
							break;
						case "cboCMLHighLevelVersion":
                            SetChainVersions(((ComboBox)sender).SelectedItem);
                            SetLowLevelChainVersions(((ComboBox)sender).SelectedItem);
							break;
						case "cboCMLLowLevelsVersion":
							break;
					}
				}
			}
			catch (Exception exc)
			{
				HandleException(exc);
			}
		}

		private void SetStoreVersions(object aSelectedItem)
		{
			try
			{
				int idx;

                idx = cboSSLStoreVersion.Items.IndexOf(aSelectedItem);
				if (idx != -1 &&
                    idx != cboSSLStoreVersion.SelectedIndex)
				{
                    cboSSLStoreVersion.SelectedIndex = idx;             
				}

                idx = cboSMLHighLevelVersion.Items.IndexOf(aSelectedItem);
				if (idx != -1 &&
                    idx != cboSMLHighLevelVersion.SelectedIndex)
				{
                    cboSMLHighLevelVersion.SelectedIndex = idx;
				}
			}
			catch (Exception exc)
			{
				HandleException(exc);
			}
		}

		private void SetLowLevelStoreVersions(object aSelectedItem)
		{
			try
			{
				int idx;

                idx = cboSMLLowLevelsVersion.Items.IndexOf(aSelectedItem);
				if (idx != -1 &&
                    idx != cboSMLLowLevelsVersion.SelectedIndex)
				{
                    cboSMLLowLevelsVersion.SelectedIndex = idx;
				}
			}
			catch (Exception exc)
			{
				HandleException(exc);
			}
		}

		private void SetChainVersions(object aSelectedItem)
		{
			try
			{
				int idx;

                idx = cboCSLChainVersion.Items.IndexOf(aSelectedItem);
				if (idx != -1 &&
                    idx != cboCSLChainVersion.SelectedIndex)
				{
                    cboCSLChainVersion.SelectedIndex = idx;
				}

                idx = cboSSLChainVersion.Items.IndexOf(aSelectedItem);
				if (idx != -1 &&
                    idx != cboSSLChainVersion.SelectedIndex)
				{
                    cboSSLChainVersion.SelectedIndex = idx;
				}

                idx = cboCMLHighLevelVersion.Items.IndexOf(aSelectedItem);
				if (idx != -1 &&
                    idx != cboCMLHighLevelVersion.SelectedIndex)
				{
                    cboCMLHighLevelVersion.SelectedIndex = idx;
				}

                idx = cboCMLLowLevelsVersion.Items.IndexOf(aSelectedItem);
				if (idx != -1 &&
                    idx != cboCMLLowLevelsVersion.SelectedIndex)
				{
                    cboCMLLowLevelsVersion.SelectedIndex = idx;
				}
			}
			catch (Exception exc)
			{
				HandleException(exc);
			}
		}

		private void SetLowLevelChainVersions(object aSelectedItem)
		{
			try
			{
				int idx;

                idx = cboCMLLowLevelsVersion.Items.IndexOf(aSelectedItem);
				if (idx != -1 &&
                    idx != cboCMLLowLevelsVersion.SelectedIndex)
				{
                    cboCMLLowLevelsVersion.SelectedIndex = idx;
				}
			}
			catch (Exception exc)
			{
				HandleException(exc);
			}
		}

		private void cboGroupBy_SelectionChangeCommitted(object sender, System.EventArgs e)
		{
			try
			{
				if (FormLoaded)
				{
					if (((ComboBox)sender).SelectedIndex != -1)
					{
						// set all group by combo boxes to the same index
                        if (cboSSLGroupBy.SelectedIndex != ((ComboBox)sender).SelectedIndex)
						{
                            cboSSLGroupBy.SelectedIndex = ((ComboBox)sender).SelectedIndex;
						}
                        if (cboSMLGroupBy.SelectedIndex != ((ComboBox)sender).SelectedIndex)
						{
                            cboSMLGroupBy.SelectedIndex = ((ComboBox)sender).SelectedIndex;
						}
                        if (cboCMLGroupBy.SelectedIndex != ((ComboBox)sender).SelectedIndex)
						{
                            cboCMLGroupBy.SelectedIndex = ((ComboBox)sender).SelectedIndex;
						}
					}
				}
			}
			catch (Exception exc)
			{
				HandleException(exc);
			}
		}

		private void cboStoreAttribute_SelectionChangeCommitted(object sender, System.EventArgs e)
		{
			try
			{
				if (FormLoaded)
				{
					if (((ComboBox)sender).SelectedIndex != -1)
					{
						// set all store attribute combo boxes to the same index
                        if (cboSSLStoreAttribute.SelectedIndex != ((ComboBox)sender).SelectedIndex)
						{
                            cboSSLStoreAttribute.SelectedIndex = ((ComboBox)sender).SelectedIndex;
						}
                        if (cboSMLStoreAttribute.SelectedIndex != ((ComboBox)sender).SelectedIndex)
						{
                            cboSMLStoreAttribute.SelectedIndex = ((ComboBox)sender).SelectedIndex;
						}
					}
				}
			}
			catch (Exception exc)
			{
				HandleException(exc);
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
					//ChangePending = true;
				}
			}
			catch (Exception exc)
			{
				HandleException(exc);
			}
			//End Track #5858
		}
		private void cboView_SelectionChangeCommitted(object sender, System.EventArgs e)
		{
			try
			{
				if (FormLoaded)
				{
					if (((ComboBox)sender).SelectedIndex != -1)
					{
						// set all view combo boxes to the same index
                        if (cboSSLView.SelectedIndex != ((ComboBox)sender).SelectedIndex)
						{
                            cboSSLView.SelectedIndex = ((ComboBox)sender).SelectedIndex;
						}
                        if (cboSMLView.SelectedIndex != ((ComboBox)sender).SelectedIndex)
						{
                            cboSMLView.SelectedIndex = ((ComboBox)sender).SelectedIndex;
						}
                        if (cboCSLView.SelectedIndex != ((ComboBox)sender).SelectedIndex)
						{
                            cboCSLView.SelectedIndex = ((ComboBox)sender).SelectedIndex;
						}
                        if (cboCMLView.SelectedIndex != ((ComboBox)sender).SelectedIndex)
						{
                            cboCMLView.SelectedIndex = ((ComboBox)sender).SelectedIndex;
						}
					}
				}
			}
			catch (Exception exc)
			{
				HandleException(exc);
			}
		}

		private void chkIneligibleStores_CheckedChanged(object sender, System.EventArgs e)
		{
			try
			{
				if (FormLoaded)
				{
					// set all ineligible check boxes to the same value
					if (chkSSLIneligibleStores.Checked != ((CheckBox)sender).Checked)
					{
						chkSSLIneligibleStores.Checked = ((CheckBox)sender).Checked;
					}
					if (chkSMLIneligibleStores.Checked != ((CheckBox)sender).Checked)
					{
						chkSMLIneligibleStores.Checked = ((CheckBox)sender).Checked;
					}
				}
			}
			catch (Exception exc)
			{
				HandleException(exc);
			}
		}

		private void chkSimilarStores_CheckedChanged(object sender, System.EventArgs e)
		{
			if (FormLoaded)
			{
				// set all ineligible check boxes to the same value
				if (chkSSLSimilarStores.Checked != ((CheckBox)sender).Checked)
				{
					chkSSLSimilarStores.Checked = ((CheckBox)sender).Checked;
				}
				if (chkSMLSimilarStores.Checked != ((CheckBox)sender).Checked)
				{
					chkSMLSimilarStores.Checked = ((CheckBox)sender).Checked;
				}
			}
		}


		private void cboFilter_SelectionChangeCommitted(object sender, System.EventArgs e)
		{
			try
			{
                if (((ComboBox)sender).SelectedIndex != -1)
				{
					if (((FilterNameCombo)((ComboBox)sender).SelectedItem).FilterRID == -1)
					{
						((ComboBox)sender).SelectedIndex = -1;
					}

					// set all filter combo boxes to the same index
                    if (cboSSLFilter.SelectedIndex != ((ComboBox)sender).SelectedIndex)
					{
                        cboSSLFilter.SelectedIndex = ((ComboBox)sender).SelectedIndex;
                        ////this.cboSSLFilter_SelectionChangeCommitted(source, new EventArgs()); // TT#301-MD - RBeck - Controls are not functioning properly                       
					}
                    if (cboSMLFilter.SelectedIndex != ((ComboBox)sender).SelectedIndex)
					{
                        cboSMLFilter.SelectedIndex = ((ComboBox)sender).SelectedIndex;
                        //this.cboSMLFilter_SelectionChangeCommitted(source, new EventArgs()); // TT#301-MD - RBeck - Controls are not functioning properly
					}
                    if (cboCMLFilter.SelectedIndex != ((ComboBox)sender).SelectedIndex)
					{
                        cboCMLFilter.SelectedIndex = ((ComboBox)sender).SelectedIndex;
					}
				}
			}
			catch (Exception exc)
			{
				HandleException(exc);
			}
		}

		private void cboFilter_DropDown(object sender, System.EventArgs e)
		{
			FilterNameCombo holdFilter;

			try
			{
				holdFilter = (FilterNameCombo)((ComboBox)sender).SelectedItem;
				BindFilterComboBox();
                ((ComboBox)sender).SelectedItem = holdFilter;
			}
			catch (Exception exc)
			{
				HandleException(exc);
			}
		}

		private void cboFilter_DragEnter(object sender, System.Windows.Forms.DragEventArgs e)
		{
			Image_DragEnter(sender, e);
		}

		private void cboFilter_DragOver(object sender, DragEventArgs e)
		{
			Image_DragOver(sender, e);
		}

		private void cboFilter_DragDrop(object sender, System.Windows.Forms.DragEventArgs e)
		{
			//Begin Track #5858 - Kjohnson - Validating store security only
			try
			{
                bool isSuccessfull = ((MIDComboBoxTag)(((ComboBox)sender).Tag)).ComboBox_DragDrop(sender, e);

				if (isSuccessfull)
				{
					//ChangePending = true;
                    ((MIDComboBoxEnh)((ComboBox)sender).Parent).FirePropertyChangeEvent();
				}
			}
			catch (Exception exc)
			{
				HandleException(exc);
			}
			//End Track #5858
		}

		#endregion

		#endregion

		#region Methods

		private void LoadDateRangeSelector(Controls.MIDDateRangeSelector aMIDDRS, DateRangeProfile aDateRangeProf)
		{
			try
			{
				aMIDDRS.Text = aDateRangeProf.DisplayDate;
				aMIDDRS.DateRangeRID = aDateRangeProf.Key;

				if (aDateRangeProf.DateRangeType == eCalendarRangeType.Dynamic)
				{
					aMIDDRS.SetImage(_dynamicToCurrentImage);
				}
				else if (aDateRangeProf.DateRangeType == eCalendarRangeType.DynamicSwitch)
				{
					aMIDDRS.SetImage(_dynamicSwitchImage);
				}
				else

				{
					aMIDDRS.SetImage(null);
				}

				//=========================================================
				// Override the image if this is a dynamic switch date.
				//=========================================================
				if (aDateRangeProf.IsDynamicSwitch)
					aMIDDRS.SetImage(this._dynamicSwitchImage);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		private void SetBasisDates()
		{
			int i;
			int drID;
			DateRangeProfile drProf;
			string drText;

			try
			{
				for (i = 0; i < _dsBasis.Tables["BasisDetails"].Rows.Count; i++)
				{
					//Fill in the DateRange selector's display text.

					if (_dsBasis.Tables["BasisDetails"].Rows[i]["DateRangeID"] != System.DBNull.Value)
					{
						drID = Convert.ToInt32(_dsBasis.Tables["BasisDetails"].Rows[i]["DateRangeID"], CultureInfo.CurrentUICulture);

						if (drID != Include.UndefinedCalendarDateRange)
						{
							if (mdsSSLPlanDateRange.DateRangeRID != Include.UndefinedCalendarDateRange)
							{
								drProf = _sab.ClientServerSession.Calendar.GetDateRange(drID, mdsSSLPlanDateRange.DateRangeRID);
							}
							else
							{
								drProf = _sab.ClientServerSession.Calendar.GetDateRange(drID, _sab.ClientServerSession.Calendar.CurrentDate);
							}

							drText = _sab.ClientServerSession.Calendar.GetDisplayDate(drProf);

							_dsBasis.Tables["BasisDetails"].Rows[i]["DateRange"] = drText;
						}
					}
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Validates the entire form
		/// </summary>
		/// <returns></returns>

		private void CheckForm()
		{
			//Check various fields and make sure that they're all filled out.

			try
			{
				switch (_openParms.PlanSessionType)
				{
					case ePlanSessionType.ChainSingleLevel:
						CheckSingleLevelChain();
						break;
					case ePlanSessionType.ChainMultiLevel:
						CheckMultiLevelChain();
						break;
					case ePlanSessionType.StoreMultiLevel:
						CheckMultiLevelStore();
						break;
					default:
						CheckSingleLevelStore();
						break;
				}

				//Check basis fields to make sure that they're all filled out.

				foreach (DataRow dr in _dsBasis.Tables["BasisDetails"].Rows)
				{
					if (dr["MerchandiseID"].ToString() == string.Empty)
					{
						if (_openParms.PlanSessionType == ePlanSessionType.StoreMultiLevel)
						{
							dr["Merchandise"] = _openParms.StoreHLPlanProfile.NodeProfile.Text;
							dr["MerchandiseID"] = _openParms.StoreHLPlanProfile.NodeProfile.Key;
						}
						else if (_openParms.PlanSessionType == ePlanSessionType.ChainMultiLevel)
						{
							dr["Merchandise"] = _openParms.ChainHLPlanProfile.NodeProfile.Text;
							dr["MerchandiseID"] = _openParms.ChainHLPlanProfile.NodeProfile.Key;
						}
						else
						{
							throw new EditErrorException(MIDText.GetText(eMIDTextCode.msg_pl_UnfinishedBasisDetail));
						}
					}

					if (dr["VersionID"].ToString() == string.Empty)
					{
						throw new EditErrorException(MIDText.GetText(eMIDTextCode.msg_pl_UnfinishedBasisDetail));
					}

					if (dr["DateRangeID"].ToString() == string.Empty)
					{
						throw new EditErrorException(MIDText.GetText(eMIDTextCode.msg_pl_UnfinishedBasisDetail));
					}
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		private void CheckSingleLevelChain()
		{
			try
			{
                if (cboCSLView.SelectedIndex == -1)
				{
					throw new EditErrorException(MIDText.GetText(eMIDTextCode.msg_pl_ViewMissing));
				}

				if (txtCSLChainNode.Text.Trim().Length == 0)
				{
					throw new EditErrorException(MIDText.GetText(eMIDTextCode.msg_pl_ChainHierarchyNodeMissing));
				}

                if (cboCSLChainVersion.SelectedIndex == -1)
				{
					throw new EditErrorException(MIDText.GetText(eMIDTextCode.msg_pl_ChainVersionMissing));
				}

				if (mdsCSLPlanDateRange.DateRangeRID == Include.UndefinedCalendarDateRange)
				{
					throw new EditErrorException(MIDText.GetText(eMIDTextCode.msg_pl_PlanDateMissing));
				}

				if (mdsCSLPlanDateRange.Text.Trim() == string.Empty)
				{
					throw new EditErrorException(MIDText.GetText(eMIDTextCode.msg_pl_PlanDateMissing));
				}
			}
			catch
			{
				throw;
			}
		}
		
		private void CheckMultiLevelChain()
		{
			try
			{
                if (cboCMLView.SelectedIndex == -1)
				{
					throw new EditErrorException(MIDText.GetText(eMIDTextCode.msg_pl_ViewMissing));
				}

				if (txtCMLHighLevelNode.Text.Trim().Length == 0)
				{
					throw new EditErrorException(MIDText.GetText(eMIDTextCode.msg_pl_ChainHighLevelHierarchyNodeMissing));
				}

                if (cboCMLHighLevelVersion.SelectedIndex == -1)
				{
					throw new EditErrorException(MIDText.GetText(eMIDTextCode.msg_pl_ChainHighLevelVersionMissing));
				}

				if (mdsCMLPlanDateRange.DateRangeRID == Include.UndefinedCalendarDateRange)
				{
					throw new EditErrorException(MIDText.GetText(eMIDTextCode.msg_pl_PlanDateMissing));
				}

				if (mdsCMLPlanDateRange.Text.Trim() == string.Empty)
				{
					throw new EditErrorException(MIDText.GetText(eMIDTextCode.msg_pl_PlanDateMissing));
				}

                if (cboCMLLowLevelsVersion.SelectedIndex == -1)
				{
					throw new EditErrorException(MIDText.GetText(eMIDTextCode.msg_pl_ChainLowLevelVersionMissing));
				}

                if (cboCMLLowLevels.SelectedIndex == -1)
				{
					throw new EditErrorException(MIDText.GetText(eMIDTextCode.msg_pl_LowLevelsNotDefined));
				}
			}
			catch
			{
				throw;
			}
		}
		
		private void CheckMultiLevelStore()
		{
			try
			{
                if (cboSMLStoreAttribute.SelectedIndex == -1)
				{
					throw new EditErrorException(MIDText.GetText(eMIDTextCode.msg_pl_StoreAttributeMissing));
				}

                if (cboSMLView.SelectedIndex == -1)
				{
					throw new EditErrorException(MIDText.GetText(eMIDTextCode.msg_pl_ViewMissing));
				}

				if (txtSMLHighLevelNode.Text.Trim().Length == 0)
				{
					throw new EditErrorException(MIDText.GetText(eMIDTextCode.msg_pl_StoreHighLevelVersionMissing));
				}

                if (cboSMLHighLevelVersion.SelectedIndex == -1)
				{
					throw new EditErrorException(MIDText.GetText(eMIDTextCode.msg_pl_StoreHighLevelVersionMissing));
				}

				if (mdsSMLPlanDateRange.DateRangeRID == Include.UndefinedCalendarDateRange)
				{
					throw new EditErrorException(MIDText.GetText(eMIDTextCode.msg_pl_PlanDateMissing));
				}

				if (mdsSMLPlanDateRange.Text.Trim() == string.Empty)
				{
					throw new EditErrorException(MIDText.GetText(eMIDTextCode.msg_pl_PlanDateMissing));
				}

                if (cboSMLLowLevelsVersion.SelectedIndex == -1)
				{
					throw new EditErrorException(MIDText.GetText(eMIDTextCode.msg_pl_StoreLowLevelVersionMissing));
				}

                if (cboSMLLowLevels.SelectedIndex == -1)
				{
					throw new EditErrorException(MIDText.GetText(eMIDTextCode.msg_pl_LowLevelsNotDefined));
				}
			}
			catch
			{
				throw;
			}
		}
		
		private void CheckSingleLevelStore()
		{
			try
			{
				if (cboSSLStoreAttribute.SelectedIndex == -1)
				{
					throw new EditErrorException(MIDText.GetText(eMIDTextCode.msg_pl_StoreAttributeMissing));
				}

                if (cboSSLView.SelectedIndex == -1)
				{
					throw new EditErrorException(MIDText.GetText(eMIDTextCode.msg_pl_ViewMissing));
				}

				if (txtSSLStoreNode.Text.Trim().Length == 0)
				{
					throw new EditErrorException(MIDText.GetText(eMIDTextCode.msg_pl_StoreHierarchyNodeMissing));
				}

                if (cboSSLStoreVersion.SelectedIndex == -1)
				{
					throw new EditErrorException(MIDText.GetText(eMIDTextCode.msg_pl_StoreVersionMissing));
				}

				if (mdsSSLPlanDateRange.DateRangeRID == Include.UndefinedCalendarDateRange)
				{
					throw new EditErrorException(MIDText.GetText(eMIDTextCode.msg_pl_PlanDateMissing));
				}

				if (mdsSSLPlanDateRange.Text.Trim() == string.Empty)
				{
					throw new EditErrorException(MIDText.GetText(eMIDTextCode.msg_pl_PlanDateMissing));
				}

				if (txtSSLChainNode.Text == null || txtSSLChainNode.Text.Length == 0)
				{
					throw new EditErrorException(MIDText.GetText(eMIDTextCode.msg_pl_ChainHierarchyNodeMissing));
				}

                if (cboSSLChainVersion.SelectedIndex == -1)
				{
					throw new EditErrorException(MIDText.GetText(eMIDTextCode.msg_pl_ChainVersionMissing));
				}
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// checks to make sure that the previous row, if there is one, is completely
		/// filled out. If any information is missing, return false to the calling
		/// procedure to indicate that it should not proceed adding another row.
		/// </summary>
		/// <returns></returns>
		
		private void CheckInsertCondition(Infragistics.Win.UltraWinGrid.BeforeRowInsertEventArgs e)
		{
			DataView dv;
			DataRowView drv;

			//Check to see if there is already a Details row. It should be impossible to not have one, but we want to check anyway.

			try
			{
				if (e.Band == grdBasis.DisplayLayout.Bands[1])
				{
					//Find all the rows that are children of the Basis row. (Rows in Details table having the save BasisID.)

					dv = new DataView();
					dv.Table = _dsBasis.Tables["BasisDetails"];
					dv.RowFilter = "BasisID = " + e.ParentRow.Cells["BasisID"].Value.ToString();
				
					if (dv.Count != 0) 
					{
						//We are in this block of code because the user is trying to insert a Details row, and there are Details rows already existing.
						//Retrieve the last child row of the Basis row. (The last row in Details table having the same BasisID)

						drv = dv[dv.Count - 1];

						//Check various fields to make sure that they're all filled out.

						if (drv["Merchandise"].ToString() == string.Empty)
						{
							throw new EditErrorException(MIDText.GetText(eMIDTextCode.msg_pl_FillPreviousRow));
						}
						if (drv["Version"].ToString() == string.Empty)
						{
							throw new EditErrorException(MIDText.GetText(eMIDTextCode.msg_pl_FillPreviousRow));
						}
						if (drv["DateRange"].ToString() == string.Empty)
						{
							throw new EditErrorException(MIDText.GetText(eMIDTextCode.msg_pl_FillPreviousRow));
						}
					}
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		#endregion

		private void radChain_CheckedChanged(object sender, System.EventArgs e)
		{
			try
			{
				if (radChain.Checked)
				{
					SetMultiLevelEnabled();
					if (FormLoaded)
					{
						ShowTypePanel();
					}
				}
			}
			catch (Exception exc)
			{
				HandleException(exc);
			}
		}
        //Begin TT#609-MD -jsobek -OTS Forecast Chain Ladder View
        private void radChainLadder_CheckedChanged(object sender, System.EventArgs e)
        {
            try
            {
                if (radChainLadder.Checked)
                {
                    SetMultiLevelEnabled();
                    if (FormLoaded)
                    {
                        ShowTypePanel();
                    }
                }
            }
            catch (Exception exc)
            {
                HandleException(exc);
            }
        }
        //End TT#609-MD -jsobek -OTS Forecast Chain Ladder View

		private void radStore_CheckedChanged(object sender, System.EventArgs e)
		{
			try
			{
				if (radStore.Checked)
				{
					SetMultiLevelEnabled();
					if (FormLoaded)
					{
						ShowTypePanel();
					}
				}
			}
			catch (Exception exc)
			{
				HandleException(exc);
			}
		}

		private void chkMultiLevel_CheckedChanged(object sender, System.EventArgs e)
		{
			try
			{
				if (FormLoaded)
				{
					ShowTypePanel();
				}
			}
			catch (Exception exc)
			{
				HandleException(exc);
			}
		}

		private void SetMultiLevelEnabled()
		{
			try
			{
//Begin Track #3939 - JScott - Single chain/store being allowed with deny security
//				if (radChain.Checked)
//				{
//					if (!_multiChainSecurity.AccessDenied)
//					{
//						chkMultiLevel.Enabled = true;
//					}
//					else
//					{
//						chkMultiLevel.Enabled = false;
//					}
//				}
//				else
//				{
//					if (!_multiStoreSecurity.AccessDenied)
//					{
//						chkMultiLevel.Enabled = true;
//					}
//					else
//					{
//						chkMultiLevel.Enabled = false;
//					}
//				}
				if (radChain.Checked)
				{
                    chkRT.Enabled = true;  // TT#639-MD -jsobek - OTS Forecast Totals Right
					if (!_singleChainSecurity.AccessDenied && !_multiChainSecurity.AccessDenied)
					{
						chkMultiLevel.Enabled = true;
                        //chkRT.Enabled = true;  // TT#639-MD -agallagher - OTS Forecast Totals Right
					}
					else
					{
						if (_singleChainSecurity.AccessDenied)
						{
							chkMultiLevel.Checked = true;
						}
						else
						{
							chkMultiLevel.Checked = false;
						}

						chkMultiLevel.Enabled = false;
					}
				}
                //Begin TT#609-MD -jsobek -OTS Forecast Chain Ladder View
                else if (radChainLadder.Checked)
                {
                    chkMultiLevel.Enabled = false;
                    chkRT.Enabled = false;  // TT#639-MD -agallagher - OTS Forecast Totals Right
                }
                //End TT#609-MD -jsobek -OTS Forecast Chain Ladder View
				else
				{
                    chkRT.Enabled = true;  // TT#639-MD -jsobek - OTS Forecast Totals Right
					if (!_singleStoreSecurity.AccessDenied && !_multiStoreSecurity.AccessDenied)
					{
						chkMultiLevel.Enabled = true;
					}
					else
					{
						if (_singleStoreSecurity.AccessDenied)
						{
							chkMultiLevel.Checked = true;
						}
						else
						{
							chkMultiLevel.Checked = false;
						}

						chkMultiLevel.Enabled = false;
					}
				}
//End Track #3939 - JScott - Single chain/store being allowed with deny security
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		private void ShowTypePanel()
		{
			try
			{
				//Begin TT#1115 - JScott - Merchandise field missing from basis line in multi level view
				//if (radChain.Checked)
				//{
				//    //Begin Track #3939 - JScott - Single chain/store being allowed with deny security
				//    //if (chkMultiLevel.Enabled && chkMultiLevel.Checked)
				//    if (chkMultiLevel.Checked)
				//    //End Track #3939 - JScott - Single chain/store being allowed with deny security
				//    {
				//        _openParms.PlanSessionType = ePlanSessionType.ChainMultiLevel;
				//        grdBasis.DisplayLayout.Bands[1].Columns["Merchandise"].Hidden = true;
				//        grdBasis.DisplayLayout.Bands[1].Columns["Version"].Width = 100;
				//        grdBasis.DisplayLayout.Bands[1].Columns["DateRange"].Width = 200;
				//        grdBasis.DisplayLayout.Bands[1].Columns["Weight"].Width = 80;
				//        grdBasis.DisplayLayout.Bands[1].Columns["IncludeButton"].Width = 20;
				//        pnlChainSingleLevel.Visible = false;
				//        pnlStoreMultiLevel.Visible = false;
				//        pnlStoreSingleLevel.Visible = false;
				//        pnlChainMultiLevel.Visible = true;
				//        // Begin Track #5733 - JSmith - Title not correct for chain plan
				//        if (this.txtCMLHighLevelNode.Text != null && this.txtCMLHighLevelNode.Text.Trim().Length > 0)
				//        {
				//            this.Text = MIDText.GetTextOnly(eMIDTextCode.frm_OTSPlanSelection) + " - " + this.txtCMLHighLevelNode.Text;
				//        }
				//        // End Track #5733
				//    }
				//    else
				//    {
				//        _openParms.PlanSessionType = ePlanSessionType.ChainSingleLevel;
				//        grdBasis.DisplayLayout.Bands[1].Columns["Merchandise"].Hidden = false;
				//        grdBasis.DisplayLayout.Bands[1].Columns["Merchandise"].Width = 200;
				//        grdBasis.DisplayLayout.Bands[1].Columns["Version"].Width = 100;
				//        grdBasis.DisplayLayout.Bands[1].Columns["DateRange"].Width = 200;
				//        grdBasis.DisplayLayout.Bands[1].Columns["Weight"].Width = 80;
				//        grdBasis.DisplayLayout.Bands[1].Columns["IncludeButton"].Width = 20;
				//        pnlChainMultiLevel.Visible = false;
				//        pnlStoreMultiLevel.Visible = false;
				//        pnlStoreSingleLevel.Visible = false;
				//        pnlChainSingleLevel.Visible = true;
				//        // Begin Track #5733 - JSmith - Title not correct for chain plan
				//        if (this.txtCSLChainNode.Text != null && this.txtCSLChainNode.Text.Trim().Length > 0)
				//        {
				//            this.Text = MIDText.GetTextOnly(eMIDTextCode.frm_OTSPlanSelection) + " - " + this.txtCSLChainNode.Text;
				//        }
				//        // End Track #5733
				//    }
				//}
				//else
				//{
				//    //Begin Track #3939 - JScott - Single chain/store being allowed with deny security
				//    //if (chkMultiLevel.Enabled && chkMultiLevel.Checked)
				//    if (chkMultiLevel.Checked)
				//    //End Track #3939 - JScott - Single chain/store being allowed with deny security
				//    {
				//        _openParms.PlanSessionType = ePlanSessionType.StoreMultiLevel;
				//        grdBasis.DisplayLayout.Bands[1].Columns["Merchandise"].Hidden = true;
				//        grdBasis.DisplayLayout.Bands[1].Columns["Version"].Width = 100;
				//        grdBasis.DisplayLayout.Bands[1].Columns["DateRange"].Width = 200;
				//        grdBasis.DisplayLayout.Bands[1].Columns["Weight"].Width = 80;
				//        grdBasis.DisplayLayout.Bands[1].Columns["IncludeButton"].Width = 20;
				//        pnlChainMultiLevel.Visible = false;
				//        pnlChainSingleLevel.Visible = false;
				//        pnlStoreSingleLevel.Visible = false;
				//        pnlStoreMultiLevel.Visible = true;
				//        // Begin Track #5733 - JSmith - Title not correct for chain plan
				//        if (this.txtSSLStoreNode.Text != null && this.txtSSLStoreNode.Text.Trim().Length > 0)
				//        {
				//            this.Text = MIDText.GetTextOnly(eMIDTextCode.frm_OTSPlanSelection) + " - " + txtSSLStoreNode.Text;
				//        }
				//        // End Track #5733
				//    }
				//    else
				//    {
				//        _openParms.PlanSessionType = ePlanSessionType.StoreSingleLevel;
				//        grdBasis.DisplayLayout.Bands[1].Columns["Merchandise"].Hidden = false;
				//        grdBasis.DisplayLayout.Bands[1].Columns["Merchandise"].Width = 200;
				//        grdBasis.DisplayLayout.Bands[1].Columns["Version"].Width = 100;
				//        grdBasis.DisplayLayout.Bands[1].Columns["DateRange"].Width = 200;
				//        grdBasis.DisplayLayout.Bands[1].Columns["Weight"].Width = 80;
				//        grdBasis.DisplayLayout.Bands[1].Columns["IncludeButton"].Width = 20;
				//        pnlChainMultiLevel.Visible = false;
				//        pnlChainSingleLevel.Visible = false;
				//        pnlStoreMultiLevel.Visible = false;
				//        pnlStoreSingleLevel.Visible = true;
				//        // Begin Track #5733 - JSmith - Title not correct for chain plan
				//        if (this.txtSSLStoreNode.Text != null && this.txtSSLStoreNode.Text.Trim().Length > 0)
				//        {
				//            this.Text = MIDText.GetTextOnly(eMIDTextCode.frm_OTSPlanSelection) + " - " + txtSSLStoreNode.Text;
				//        }
				//        // End Track #5733
				//    }
				//}
                if (radChain.Checked || radChainLadder.Checked)   //TT#609-MD -jsobek -OTS Forecast Chain Ladder View
				{
                    if (chkMultiLevel.Checked && radChain.Checked)
					{
						_openParms.PlanSessionType = ePlanSessionType.ChainMultiLevel;
						pnlChainSingleLevel.Visible = false;
						pnlStoreMultiLevel.Visible = false;
						pnlStoreSingleLevel.Visible = false;
						pnlChainMultiLevel.Visible = true;
						if (this.txtCMLHighLevelNode.Text != null && this.txtCMLHighLevelNode.Text.Trim().Length > 0)
						{
							this.Text = MIDText.GetTextOnly(eMIDTextCode.frm_OTSPlanSelection) + " - " + this.txtCMLHighLevelNode.Text;
						}
					}
					else
					{
						_openParms.PlanSessionType = ePlanSessionType.ChainSingleLevel;
						pnlChainMultiLevel.Visible = false;
						pnlStoreMultiLevel.Visible = false;
						pnlStoreSingleLevel.Visible = false;
						pnlChainSingleLevel.Visible = true;
						if (this.txtCSLChainNode.Text != null && this.txtCSLChainNode.Text.Trim().Length > 0)
						{
							this.Text = MIDText.GetTextOnly(eMIDTextCode.frm_OTSPlanSelection) + " - " + this.txtCSLChainNode.Text;
						}
					}
				}
				else
				{
					if (chkMultiLevel.Checked)
					{
						_openParms.PlanSessionType = ePlanSessionType.StoreMultiLevel;
						pnlChainMultiLevel.Visible = false;
						pnlChainSingleLevel.Visible = false;
						pnlStoreSingleLevel.Visible = false;
						pnlStoreMultiLevel.Visible = true;
						if (this.txtSSLStoreNode.Text != null && this.txtSSLStoreNode.Text.Trim().Length > 0)
						{
							this.Text = MIDText.GetTextOnly(eMIDTextCode.frm_OTSPlanSelection) + " - " + txtSSLStoreNode.Text;
						}
					}
					else
					{
						_openParms.PlanSessionType = ePlanSessionType.StoreSingleLevel;

						pnlChainMultiLevel.Visible = false;
						pnlChainSingleLevel.Visible = false;
						pnlStoreMultiLevel.Visible = false;
						pnlStoreSingleLevel.Visible = true;
						if (this.txtSSLStoreNode.Text != null && this.txtSSLStoreNode.Text.Trim().Length > 0)
						{
							this.Text = MIDText.GetTextOnly(eMIDTextCode.frm_OTSPlanSelection) + " - " + txtSSLStoreNode.Text;
						}
					}
				}

				FormatBasisGrid();
				//End TT#1115 - JScott - Merchandise field missing from basis line in multi level view
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		//Begin TT#1115 - JScott - Merchandise field missing from basis line in multi level view
		private void FormatBasisGrid()
		{
			try
			{
                if (radChain.Checked || radChainLadder.Checked)   //TT#609-MD -jsobek -OTS Forecast Chain Ladder View
				{
                    if (chkMultiLevel.Checked && radChain.Checked) //TT#609-MD -jsobek -OTS Forecast Chain Ladder View
					{
						grdBasis.DisplayLayout.Bands[1].Columns["Merchandise"].Hidden = true;
						grdBasis.DisplayLayout.Bands[1].Columns["Version"].Width = 100;
						grdBasis.DisplayLayout.Bands[1].Columns["DateRange"].Width = 200;
						grdBasis.DisplayLayout.Bands[1].Columns["Weight"].Width = 80;
						grdBasis.DisplayLayout.Bands[1].Columns["IncludeButton"].Width = 20;
					}
					else
					{
						grdBasis.DisplayLayout.Bands[1].Columns["Merchandise"].Hidden = false;
						grdBasis.DisplayLayout.Bands[1].Columns["Merchandise"].Width = 200;
						grdBasis.DisplayLayout.Bands[1].Columns["Version"].Width = 100;
						grdBasis.DisplayLayout.Bands[1].Columns["DateRange"].Width = 200;
						grdBasis.DisplayLayout.Bands[1].Columns["Weight"].Width = 80;
						grdBasis.DisplayLayout.Bands[1].Columns["IncludeButton"].Width = 20;
					}
				}
				else
				{
					if (chkMultiLevel.Checked)
					{
						grdBasis.DisplayLayout.Bands[1].Columns["Merchandise"].Hidden = true;
						grdBasis.DisplayLayout.Bands[1].Columns["Version"].Width = 100;
						grdBasis.DisplayLayout.Bands[1].Columns["DateRange"].Width = 200;
						grdBasis.DisplayLayout.Bands[1].Columns["Weight"].Width = 80;
						grdBasis.DisplayLayout.Bands[1].Columns["IncludeButton"].Width = 20;
					}
					else
					{
						grdBasis.DisplayLayout.Bands[1].Columns["Merchandise"].Hidden = false;
						grdBasis.DisplayLayout.Bands[1].Columns["Merchandise"].Width = 200;
						grdBasis.DisplayLayout.Bands[1].Columns["Version"].Width = 100;
						grdBasis.DisplayLayout.Bands[1].Columns["DateRange"].Width = 200;
						grdBasis.DisplayLayout.Bands[1].Columns["Weight"].Width = 80;
						grdBasis.DisplayLayout.Bands[1].Columns["IncludeButton"].Width = 20;
					}
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		//End TT#1115 - JScott - Merchandise field missing from basis line in multi level view
		private void btnCMLVersionOverride_Click(object sender, System.EventArgs e)
		{
			try
			{
                if (this.cboCMLLowLevelsVersion.SelectedIndex == -1)
				{
					MessageBox.Show(MIDText.GetText(eMIDTextCode.msg_pl_LowLevelsNotDefined), this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
					return;
				}
				// Capture and save all the information on this form
//				_openParms.UserID = _sab.ClientServerSession.UserRID;
				SaveMultiLevelChain();

				// BEGIN OverrideLowLevel Enhancements stodd
				//frmOverrideLowLevelVersions frmOverrideLowLevelVersions = new frmOverrideLowLevelVersions(_sab, _lowlevelVersionOverrideList, _chainLowLevelVersionList, _openParms.LowLevelVersionDefault, true, true);
				//frmOverrideLowLevelVersions.ShowDialog();
				ShowOverrideLowLevelform(ePlanType.Chain);
				// END OverrideLowLevel Enhancements stodd
			}
			catch (Exception exc)
			{
				HandleException(exc);
			}
		}

		private void btnSMLVersionOverride_Click(object sender, System.EventArgs e)
		{
			try
			{
                if (this.cboSMLLowLevelsVersion.SelectedIndex == -1)
				{
					MessageBox.Show(MIDText.GetText(eMIDTextCode.msg_pl_LowLevelsNotDefined), this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
					return;
				}
				// Capture and save all the information on this form
//				_openParms.UserID = _sab.ClientServerSession.UserRID;
				SaveMultiLevelStore();
					
				// BEGIN OverrideLowLevel Enhancements stodd
				//frmOverrideLowLevelVersions frmOverrideLowLevelVersions = new frmOverrideLowLevelVersions(_sab, _lowlevelVersionOverrideList, _storeLowLevelVersionList, _openParms.LowLevelVersionDefault, true, true);
				//frmOverrideLowLevelVersions.ShowDialog();
				ShowOverrideLowLevelform(ePlanType.Store);
				// END OverrideLowLevel Enhancements stodd
			}
			catch (Exception exc)
			{
				HandleException(exc);
			}
		}

		// BEGIN OverrideLowLevel Enhancements stodd
		private void ShowOverrideLowLevelform(ePlanType planType)
		{
			if (FormLoaded)
			{
				Cursor.Current = Cursors.WaitCursor;
				try
				{
					string lowLevelText = string.Empty;
					int nodeRid = Include.NoRID;
					if (planType == ePlanType.Store)
					{
                        if (cboSMLLowLevels.SelectedIndex != -1)
						{
                            lowLevelText = cboSMLLowLevels.Items[cboSMLLowLevels.SelectedIndex].ToString();
						}
						nodeRid = _openParms.StoreHLPlanProfile.NodeProfile.Key;
					}
					else
					{
                        if (cboCMLLowLevels.SelectedIndex != -1)
						{
                            lowLevelText = cboCMLLowLevels.Items[cboCMLLowLevels.SelectedIndex].ToString();
						}
						nodeRid = _openParms.ChainHLPlanProfile.NodeProfile.Key;
					}

					System.Windows.Forms.Form parentForm;
					parentForm = this.MdiParent;

					object[] args = null;

					//Begin tt#700 - APicchetti - forecast methods show the method has changed when user copens Override Low Level model and then clicks close
					//System.Windows.Forms.Form frm;
					//End tt#700

					// Begin Track #5909 - stodd
					args = new object[] { SAB, _openParms.OverrideLowLevelRid, nodeRid, _openParms.LowLevelVersionDefault.Key, lowLevelText, _openParms.CustomOverrideLowLevelRid, FunctionSecurity };
					// End Track #5909 - stodd

					//Begin tt#700 - APicchetti - forecast methods show the method has changed when user copens Override Low Level model and then clicks close

					//frm = GetForm(typeof(frmOverrideLowLevelModel), args, false);
					//parentForm = this.MdiParent;
					//frm.MdiParent = parentForm;
					//frm.Anchor = AnchorStyles.Left | AnchorStyles.Top;
					//frm.Show();
					//frm.BringToFront();
					//((frmOverrideLowLevelModel)frm).OnOverrideLowLevelCloseHandler += new frmOverrideLowLevelModel.OverrideLowLevelCloseEventHandler(OnOverrideLowLevelCloseHandler);

					_overrideLowLevelfrm = GetForm(typeof(frmOverrideLowLevelModel), args, false);
					parentForm = this.MdiParent;
					_overrideLowLevelfrm.MdiParent = parentForm;
					_overrideLowLevelfrm.Anchor = AnchorStyles.Left | AnchorStyles.Top;
					_overrideLowLevelfrm.Show();
					_overrideLowLevelfrm.BringToFront();
					((frmOverrideLowLevelModel)_overrideLowLevelfrm).OnOverrideLowLevelCloseHandler += new frmOverrideLowLevelModel.OverrideLowLevelCloseEventHandler(OnOverrideLowLevelCloseHandler);

					//end tt#700

				}
				finally
				{
					Cursor.Current = Cursors.Default;
				}
			}
		}

		//Begin tt#700 - APicchetti - forecast methods show the method has changed when user copens Override Low Level model and then clicks close
		System.Windows.Forms.Form _overrideLowLevelfrm;
		//End tt#700

		private void OnOverrideLowLevelCloseHandler(object source, OverrideLowLevelCloseEventArgs e)
		{
			try
			{
				if (FormLoaded)
				{
					_openParms.OverrideLowLevelRid = e.aOllRid;
					if (_openParms.CustomOverrideLowLevelRid != e.aCustomOllRid)
					{
						_openParms.CustomOverrideLowLevelRid = e.aCustomOllRid;
						UpdateUserPlanCustomOLLRid(SAB.ClientServerSession.UserRID, _openParms.CustomOverrideLowLevelRid);
					} 
  
					//Begin tt#700 - APicchetti - forecast methods show the method has changed when user copens Override Low Level model and then clicks close
					if (_overrideLowLevelfrm.DialogResult != DialogResult.Cancel)
					{
                        LoadOverrideModelComboBox(cboCMLOverride.ComboBox, e.aOllRid, _openParms.CustomOverrideLowLevelRid);
                        LoadOverrideModelComboBox(cboSMLOverride.ComboBox, e.aOllRid, _openParms.CustomOverrideLowLevelRid);
					}

					_overrideLowLevelfrm = null;
					// End tt#700
				}
			}
			catch
			{
				throw;
			}

		}

		private void UpdateUserPlanCustomOLLRid(int userRid, int customOLLRid)
		{
			MIDRetail.Data.OTSPlanSelection otsData = null;
			try
			{
				otsData = new MIDRetail.Data.OTSPlanSelection();
				ClientTransaction.DataAccess.OpenUpdateConnection();
				otsData.UpdateUserPlanCustomOLLRid(ClientTransaction.DataAccess, userRid, customOLLRid);
				ClientTransaction.DataAccess.CommitData();
				ClientTransaction.DataAccess.CloseUpdateConnection();
			}
			catch (Exception)
			{
				throw;
			}
			finally
			{
				if (ClientTransaction.DataAccess.ConnectionIsOpen)
					otsData.CloseUpdateConnection();
			}
		}
		// END OverrideLowLevel Enhancements stodd

		// BEGIN OVerride Low Level Enhancement
		private void PopulateVersionOverrideList(ePlanType aPlanType)
		{
			try
			{
				int hierNode = Include.NoRID;
				if (aPlanType == ePlanType.Chain)
					hierNode = _openParms.ChainHLPlanProfile.NodeProfile.Key;
				else
					hierNode = _openParms.StoreHLPlanProfile.NodeProfile.Key;

				HierarchySessionTransaction hTran = new HierarchySessionTransaction(this.SAB);
				if (_openParms.LowLevelsType == eLowLevelsType.LevelOffset)
				{
					_lowlevelVersionOverrideList = hTran.GetOverrideList(_openParms.OverrideLowLevelRid, hierNode, _openParms.LowLevelVersionDefault.Key,
																			   _openParms.LowLevelsOffset, Include.NoRID, true, false);
				}
				else if (_openParms.LowLevelsType == eLowLevelsType.HierarchyLevel)
				{
					HierarchyNodeProfile hnp = SAB.HierarchyServerSession.GetNodeData(hierNode);

					// Begin Track #5960 - JSmith - Get "No Low Levels Found" when using hierarchy level name
					//int offset = _openParms.LowLevelsSequence - hnp.NodeLevel;
					// BEGIN Track #6107 – John Smith - Cannot view departments in multi-level
					//int offset;
					
					//if (hnp.HomeHierarchyType == eHierarchyType.organizational)
					//{
					//    offset = _openParms.LowLevelsSequence - hnp.NodeLevel;
					//}
					//else
					//{
					//    // search list to find level
					//    offset = 0;
					//    if (_lowLevels != null)
					//    {
					//        foreach (LowLevelCombo llc in _lowLevels)
					//        {
					//            ++offset;
					//            if (llc.LowLevelType == eLowLevelsType.HierarchyLevel &&
					//                llc.LowLevelSequence == _openParms.LowLevelsSequence)
					//            {
					//                break;
					//            }
					//        }
					//    }
					//}
					// END Track #6107
					// End Track #5960
					// BEGIN Track #6107 – John Smith - Cannot view departments in multi-level
					//_lowlevelVersionOverrideList = hTran.GetOverrideList(_openParms.OverrideLowLevelRid, hierNode, _openParms.LowLevelVersionDefault.Key,
					//                                                           offset, Include.NoRID, true, false);
					_lowlevelVersionOverrideList = hTran.GetOverrideList(_openParms.OverrideLowLevelRid, hierNode, _openParms.LowLevelVersionDefault.Key,
																			 eHierarchyDescendantType.levelType, _openParms.LowLevelsSequence, Include.NoRID, true, false);
					// END Track #6107
				}
				else
				{
					_lowlevelVersionOverrideList = new LowLevelVersionOverrideProfileList(eProfileType.LowLevelVersionOverride);
				}

//                HierarchyNodeList hnl = null;
//                if (aPlanType == ePlanType.Store)
//                {
//                    if (_openParms.LowLevelsType == eLowLevelsType.LevelOffset)
//                    {
////Begin Track #4037 - JSmith - Optionally include dummy color in child list
////						hnl = _sab.HierarchyServerSession.GetDescendantData(_openParms.StoreHLPlanProfile.NodeProfile.Key, _openParms.LowLevelsOffset, true);
//                        hnl = _sab.HierarchyServerSession.GetDescendantData(_openParms.StoreHLPlanProfile.NodeProfile.Key, _openParms.LowLevelsOffset, true, eNodeSelectType.NoVirtual);
////End Track #4037
//                    }
//                    else
//                    {
////Begin Track #4037 - JSmith - Optionally include dummy color in child list
////						hnl = _sab.HierarchyServerSession.GetDescendantDataByLevel(_openParms.StoreHLPlanProfile.NodeProfile.Key, _openParms.LowLevelsSequence, true);
//                        hnl = _sab.HierarchyServerSession.GetDescendantDataByLevel(_openParms.StoreHLPlanProfile.NodeProfile.Key, _openParms.LowLevelsSequence, true, eNodeSelectType.NoVirtual);
////End Track #4037
//                    }
//                }
//                else
//                {
//                    if (_openParms.LowLevelsType == eLowLevelsType.LevelOffset)
//                    {
////Begin Track #4037 - JSmith - Optionally include dummy color in child list
////						hnl = _sab.HierarchyServerSession.GetDescendantData(_openParms.ChainHLPlanProfile.NodeProfile.Key, _openParms.LowLevelsOffset, true);
//                        //Begin Track #5378 - color and size not qualified
////						hnl = _sab.HierarchyServerSession.GetDescendantData(_openParms.ChainHLPlanProfile.NodeProfile.Key, _openParms.LowLevelsOffset, true, eNodeSelectType.NoVirtual);
//                        hnl = _sab.HierarchyServerSession.GetDescendantData(_openParms.ChainHLPlanProfile.NodeProfile.Key, _openParms.LowLevelsOffset, true, eNodeSelectType.NoVirtual, true);
//                        //End Track #5378
////End Track #4037
//                    }
//                    else
//                    {
////Begin Track #4037 - JSmith - Optionally include dummy color in child list
////						hnl = _sab.HierarchyServerSession.GetDescendantDataByLevel(_openParms.ChainHLPlanProfile.NodeProfile.Key, _openParms.LowLevelsSequence, true);
//                        //Begin Track #5378 - color and size not qualified
////						hnl = _sab.HierarchyServerSession.GetDescendantDataByLevel(_openParms.ChainHLPlanProfile.NodeProfile.Key, _openParms.LowLevelsSequence, true, eNodeSelectType.NoVirtual);
//                        hnl = _sab.HierarchyServerSession.GetDescendantDataByLevel(_openParms.ChainHLPlanProfile.NodeProfile.Key, _openParms.LowLevelsSequence, true, eNodeSelectType.NoVirtual, true);
//                        //End Track #5378
////End Track #4037
//                    }
//                }

//                foreach (HierarchyNodeProfile hnp in hnl)
//                {
//                    LowLevelVersionOverrideProfile lvop = new LowLevelVersionOverrideProfile(hnp.Key);
//                    lvop.NodeProfile = hnp;
//                    lvop.VersionIsOverridden = false;
//                    lvop.VersionProfile = null;
//                    lvop.Exclude = false;
//                    // BEGIN Issue 4858
//                    lvop.NodeProfile.StoreSecurityProfile = SAB.ClientServerSession.GetMyUserNodeSecurityAssignment(hnp.Key, (int)eSecurityTypes.Store);
//                    lvop.NodeProfile.ChainSecurityProfile = SAB.ClientServerSession.GetMyUserNodeSecurityAssignment(hnp.Key, (int)eSecurityTypes.Chain);
//                    // END Issue 4858
//                    _lowlevelVersionOverrideList.Add(lvop);
//                }
			}
			catch
			{
				throw;
			}
		}
		// END OVerride Low Level Enhancement


		private void cboSMLLowLevels_SelectionChangeCommitted(object sender, System.EventArgs e)
		{
			try
			{
				if (FormLoaded)
				{
                    if (cboSMLLowLevels.SelectedIndex != -1)
					{
                        _openParms.LowLevelsType = ((LowLevelCombo)cboSMLLowLevels.SelectedItem).LowLevelType;
                        _openParms.LowLevelsOffset = ((LowLevelCombo)cboSMLLowLevels.SelectedItem).LowLevelOffset;
                        _openParms.LowLevelsSequence = ((LowLevelCombo)cboSMLLowLevels.SelectedItem).LowLevelSequence;
						_lowlevelVersionOverrideList.Clear();
					}
				}
			}
			catch (Exception exc)
			{
				HandleException(exc);
			}
		}

		private void cboCMLLowLevels_SelectionChangeCommitted(object sender, System.EventArgs e)
		{
			try
			{
				if (FormLoaded)
				{
                    if (cboCMLLowLevels.SelectedIndex != -1)
					{
                        _openParms.LowLevelsType = ((LowLevelCombo)cboCMLLowLevels.SelectedItem).LowLevelType;
                        _openParms.LowLevelsOffset = ((LowLevelCombo)cboCMLLowLevels.SelectedItem).LowLevelOffset;
                        _openParms.LowLevelsSequence = ((LowLevelCombo)cboCMLLowLevels.SelectedItem).LowLevelSequence;
						_lowlevelVersionOverrideList.Clear();
					}
				}
			}
			catch (Exception exc)
			{
				HandleException(exc);
			}
		}

		private void cboCMLOverride_SelectionChangeCommitted(object sender, System.EventArgs e)
		{
			try
			{
				if (FormLoaded)
				{
					//Begin Track #6099 - JSmith - Override low-level has wrong data after keying new node
					//_openParms.OverrideLowLevelRid = ((ComboObject)cboCMLOverride.SelectedItem).Key;
                    if (cboCMLOverride.SelectedItem != null)
					{
                        _openParms.OverrideLowLevelRid = ((ComboObject)cboCMLOverride.SelectedItem).Key;
					}
					else
					{
						_openParms.OverrideLowLevelRid = Include.NoRID;
						//Begin Track #6099 - KJohnson - Override low-level has wrong data after keying new node
						_openParms.CustomOverrideLowLevelRid = Include.NoRID;
						//End Track #6099 - KJohnson
					}
					//End Track #6099
				}
			}
			catch
			{
				throw;
			}
		}

		private void cboSMLOverride_SelectionChangeCommitted(object sender, System.EventArgs e)
		{
			try
			{
				if (FormLoaded)
				{
					//Begin Track #6099 - JSmith - Override low-level has wrong data after keying new node
					//_openParms.OverrideLowLevelRid = ((ComboObject)cboSMLOverride.SelectedItem).Key;
                    if (cboSMLOverride.SelectedItem != null)
					{
                        _openParms.OverrideLowLevelRid = ((ComboObject)cboSMLOverride.SelectedItem).Key;
					}
					else
					{
						_openParms.OverrideLowLevelRid = Include.NoRID;
						//Begin Track #6099 - KJohnson - Override low-level has wrong data after keying new node
						_openParms.CustomOverrideLowLevelRid = Include.NoRID;
						//End Track #6099 - KJohnson
					}
					//End Track #6099
				}
			}
			catch
			{
				throw;
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

        //BEGIN TT#6-MD-VStuart - Single Store Select
        private void cboStore_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if ((cboStore.SelectedIndex != -1) && (cboStore.SelectedItem.ToString() != "(None)"))
            {
                //Single Store Selected
                cboSMLFilter.SelectedIndex = 0;
                //this.cboSMLFilter_SelectionChangeCommitted(source, new EventArgs()); // TT#301-MD - RBeck - Controls are not functioning properly
            }
        }

        private void cboStoreNonMulti_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if ((cboStoreNonMulti.SelectedIndex != -1) && (cboStoreNonMulti.SelectedItem.ToString() != "(None)"))
            {
                //Single Store Selected
                cboSSLFilter.SelectedIndex = 0;
                //this.cboSSLFilter_SelectionChangeCommitted(source, new EventArgs()); // TT#301-MD - RBeck - Controls are not functioning properly
            }
        }

        private void cboSMLFilter_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if ((cboSMLFilter.SelectedIndex != -1) && (cboSMLFilter.SelectedItem.ToString() != "(None)"))
            {
                cboStore.SelectedIndex = 0;
                //this.cboStore_SelectionChangeCommitted(source, new EventArgs()); // TT#301-MD - RBeck - Controls are not functioning properly

                if (cboSMLFilter.SelectedIndex != -1)
                {
                    if (((FilterNameCombo)cboSMLFilter.SelectedItem).FilterRID == Include.Undefined)
                    {
                        cboSMLFilter.SelectedIndex = -1;
                    }
                }
            }
        }

        private void cboSSLFilter_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if ((cboSSLFilter.SelectedIndex != -1) && (cboSSLFilter.SelectedItem.ToString() != "(None)"))
            {
                cboStoreNonMulti.SelectedIndex = 0;
                //this.cboStoreNonMulti_SelectionChangeCommitted(source, new EventArgs()); // TT#301-MD - RBeck - Controls are not functioning properly

                if (cboSSLFilter.SelectedIndex != -1)
                {
                    if (((FilterNameCombo)cboSSLFilter.SelectedItem).FilterRID == Include.Undefined)
                    {
                        cboSSLFilter.SelectedIndex = -1;
                    }
                }
            }
        }
        //END TT#6-MD-VStuart - Single Store Select

        private void cboStoreNonMulti_MIDComboBoxPropertiesChangedEvent(object source, MIDComboBoxPropertiesChangedEventArgs args)
        {
            this.cboStoreNonMulti_SelectionChangeCommitted(((MIDComboBoxEnh)source).ComboBox, new EventArgs());
        }
        private void cboSSLView_MIDComboBoxPropertiesChangedEvent(object source, MIDComboBoxPropertiesChangedEventArgs args)
        {
            this.cboView_SelectionChangeCommitted(((MIDComboBoxEnh)source).ComboBox, new EventArgs());
        }
        private void cboSSLFilter_MIDComboBoxPropertiesChangedEvent(object source, MIDComboBoxPropertiesChangedEventArgs args)
        {
            this.cboSSLFilter_SelectionChangeCommitted(((MIDComboBoxEnh)source).ComboBox, new EventArgs());
        }
        private void cboSSLGroupBy_MIDComboBoxPropertiesChangedEvent(object source, MIDComboBoxPropertiesChangedEventArgs args)
        {
            this.cboGroupBy_SelectionChangeCommitted(((MIDComboBoxEnh)source).ComboBox, new EventArgs());
        }
        private void cboSSLStoreAttribute_MIDComboBoxPropertiesChangedEvent(object source, MIDComboBoxPropertiesChangedEventArgs args)
        {
            this.cboStoreAttribute_SelectionChangeCommitted(source, new EventArgs());
        }
        private void cboSSLChainVersion_MIDComboBoxPropertiesChangedEvent(object source, MIDComboBoxPropertiesChangedEventArgs args)
        {
            this.cboVersion_SelectionChangeCommitted(((MIDComboBoxEnh)source).ComboBox, new EventArgs());
        }
        private void cboSSLStoreVersion_MIDComboBoxPropertiesChangedEvent(object source, MIDComboBoxPropertiesChangedEventArgs args)
        {
            this.cboVersion_SelectionChangeCommitted(((MIDComboBoxEnh)source).ComboBox, new EventArgs());
        }
        private void cboCMLOverride_MIDComboBoxPropertiesChangedEvent(object source, MIDComboBoxPropertiesChangedEventArgs args)
        {
            this.cboCMLOverride_SelectionChangeCommitted(((MIDComboBoxEnh)source).ComboBox, new EventArgs());
        }
        private void cboCMLView_MIDComboBoxPropertiesChangedEvent(object source, MIDComboBoxPropertiesChangedEventArgs args)
        {
            this.cboView_SelectionChangeCommitted(((MIDComboBoxEnh)source).ComboBox, new EventArgs());
        }
        private void cboCMLFilter_MIDComboBoxPropertiesChangedEvent(object source, MIDComboBoxPropertiesChangedEventArgs args)
        {
            this.cboFilter_SelectionChangeCommitted(((MIDComboBoxEnh)source).ComboBox, new EventArgs());
        }
        private void cboCMLGroupBy_MIDComboBoxPropertiesChangedEvent(object source, MIDComboBoxPropertiesChangedEventArgs args)
        {
            this.cboGroupBy_SelectionChangeCommitted(((MIDComboBoxEnh)source).ComboBox, new EventArgs());
        }
        private void cboCMLLowLevels_MIDComboBoxPropertiesChangedEvent(object source, MIDComboBoxPropertiesChangedEventArgs args)
        {
            this.cboCMLLowLevels_SelectionChangeCommitted(((MIDComboBoxEnh)source).ComboBox, new EventArgs());
        }
        private void cboCMLLowLevelsVersion_MIDComboBoxPropertiesChangedEvent(object source, MIDComboBoxPropertiesChangedEventArgs args)
        {
            this.cboVersion_SelectionChangeCommitted(((MIDComboBoxEnh)source).ComboBox, new EventArgs());
        }
        private void cboSMLOverride_MIDComboBoxPropertiesChangedEvent(object source, MIDComboBoxPropertiesChangedEventArgs args)
        {
            this.cboSMLOverride_SelectionChangeCommitted(((MIDComboBoxEnh)source).ComboBox, new EventArgs());
        }
        private void cboStore_MIDComboBoxPropertiesChangedEvent(object source, MIDComboBoxPropertiesChangedEventArgs args)
        {
            this.cboStore_SelectionChangeCommitted(((MIDComboBoxEnh)source).ComboBox, new EventArgs());
        }
        private void cboSMLView_MIDComboBoxPropertiesChangedEvent(object source, MIDComboBoxPropertiesChangedEventArgs args)
        {
            this.cboView_SelectionChangeCommitted(((MIDComboBoxEnh)source).ComboBox, new EventArgs());
        }
        private void cboSMLFilter_MIDComboBoxPropertiesChangedEvent(object source, MIDComboBoxPropertiesChangedEventArgs args)
        {
            this.cboSMLFilter_SelectionChangeCommitted(((MIDComboBoxEnh)source).ComboBox, new EventArgs());
        }
        private void cboSMLGroupBy_MIDComboBoxPropertiesChangedEvent(object source, MIDComboBoxPropertiesChangedEventArgs args)
        {
            this.cboGroupBy_SelectionChangeCommitted(((MIDComboBoxEnh)source).ComboBox, new EventArgs());
        }
        private void cboSMLStoreAttribute_MIDComboBoxPropertiesChangedEvent(object source, MIDComboBoxPropertiesChangedEventArgs args)
        {
            this.cboStoreAttribute_SelectionChangeCommitted(source, new EventArgs());
        }
        private void cboSMLLowLevels_MIDComboBoxPropertiesChangedEvent(object source, MIDComboBoxPropertiesChangedEventArgs args)
        {
            this.cboSMLLowLevels_SelectionChangeCommitted(((MIDComboBoxEnh)source).ComboBox, new EventArgs());
        }
        private void cboSMLLowLevelsVersion_MIDComboBoxPropertiesChangedEvent(object source, MIDComboBoxPropertiesChangedEventArgs args)
        {
            this.cboVersion_SelectionChangeCommitted(((MIDComboBoxEnh)source).ComboBox, new EventArgs());
        }
        private void cboSMLHighLevelVersion_MIDComboBoxPropertiesChangedEvent(object source, MIDComboBoxPropertiesChangedEventArgs args)
        {
            this.cboVersion_SelectionChangeCommitted(((MIDComboBoxEnh)source).ComboBox, new EventArgs());
        }
        private void cboCSLView_MIDComboBoxPropertiesChangedEvent(object source, MIDComboBoxPropertiesChangedEventArgs args)
        {
            this.cboView_SelectionChangeCommitted(((MIDComboBoxEnh)source).ComboBox, new EventArgs());
        }
        private void cboCSLChainVersion_MIDComboBoxPropertiesChangedEvent(object source, MIDComboBoxPropertiesChangedEventArgs args)
        {
            this.cboVersion_SelectionChangeCommitted(((MIDComboBoxEnh)source).ComboBox, new EventArgs());
        }
        private void cboCMLHighLevelVersion_MIDComboBoxPropertiesChangedEvent(object source, MIDComboBoxPropertiesChangedEventArgs args)
        {
            this.cboVersion_SelectionChangeCommitted(((MIDComboBoxEnh)source).ComboBox, new EventArgs());
        }
	}

}
