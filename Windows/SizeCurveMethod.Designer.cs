using MIDRetail.Windows.Controls;

namespace MIDRetail.Windows
{
	partial class frmSizeCurveMethod
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}

			if (disposing)
			{
                // Begin TT#301-MD - JSmith - Controls are not functioning properly
                //this.cboSizeGroup.SelectionChangeCommitted -= new System.EventHandler(this.cboSizeGroup_SelectionChangeCommitted);
                //this.cboSizeGroup.SelectionChangeCommitted -= new System.EventHandler(this.cboSizeGroup_SelectionChangeCommitted);
                this.cboSizeCurveBySet.SelectionChangeCommitted -= new System.EventHandler(this.cboSizeCurveBySet_SelectionChangeCommitted);
                // End TT#301-MD - JSmith - Controls are not functioning properly
				this.txtName.TextChanged -= new System.EventHandler(this.txtName_TextChanged);
				this.rbMerchBasisEWNo.CheckedChanged -= new System.EventHandler(this.rbMerchBasisEWNo_CheckedChanged);
				this.rbMerchBasisEWYes.CheckedChanged -= new System.EventHandler(this.rbMerchBasisEWYes_CheckedChanged);
				this.grdMerchBasis.ClickCellButton -= new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdMerchBasis_ClickCellButton);
				this.grdMerchBasis.BeforeRowInsert -= new Infragistics.Win.UltraWinGrid.BeforeRowInsertEventHandler(this.grdMerchBasis_BeforeRowInsert);
				this.grdMerchBasis.InitializeLayout -= new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.grdMerchBasis_InitializeLayout);
				this.grdMerchBasis.DragOver -= new System.Windows.Forms.DragEventHandler(this.grdMerchBasis_DragOver);
				this.grdMerchBasis.AfterCellListCloseUp -= new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdMerchBasis_AfterCellListCloseUp);
				this.grdMerchBasis.BeforeCellDeactivate -= new System.ComponentModel.CancelEventHandler(this.grdMerchBasis_BeforeCellDeactivate);
				this.grdMerchBasis.AfterCellUpdate -= new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdMerchBasis_AfterCellUpdate);
				this.grdMerchBasis.InitializeRow -= new Infragistics.Win.UltraWinGrid.InitializeRowEventHandler(this.grdMerchBasis_InitializeRow);
				this.grdMerchBasis.CellChange -= new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdMerchBasis_CellChange);
				this.grdMerchBasis.DragDrop -= new System.Windows.Forms.DragEventHandler(this.grdMerchBasis_DragDrop);
				this.grdMerchBasis.DragEnter -= new System.Windows.Forms.DragEventHandler(this.grdMerchBasis_DragEnter);
				this.grdCurveBasis.ClickCellButton -= new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdCurveBasis_ClickCellButton);
				this.grdCurveBasis.BeforeRowInsert -= new Infragistics.Win.UltraWinGrid.BeforeRowInsertEventHandler(this.grdCurveBasis_BeforeRowInsert);
				this.grdCurveBasis.InitializeLayout -= new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.grdCurveBasis_InitializeLayout);
				this.grdCurveBasis.DragOver -= new System.Windows.Forms.DragEventHandler(this.grdCurveBasis_DragOver);
				this.grdCurveBasis.AfterCellListCloseUp -= new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdCurveBasis_AfterCellListCloseUp);
				this.grdCurveBasis.BeforeCellDeactivate -= new System.ComponentModel.CancelEventHandler(this.grdCurveBasis_BeforeCellDeactivate);
				this.grdCurveBasis.AfterCellUpdate -= new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdCurveBasis_AfterCellUpdate);
				this.grdCurveBasis.InitializeRow -= new Infragistics.Win.UltraWinGrid.InitializeRowEventHandler(this.grdCurveBasis_InitializeRow);
				this.grdCurveBasis.DragDrop -= new System.Windows.Forms.DragEventHandler(this.grdCurveBasis_DragDrop);
				this.grdCurveBasis.DragEnter -= new System.Windows.Forms.DragEventHandler(this.grdCurveBasis_DragEnter);
				this.rbCurveBasisEWNo.CheckedChanged -= new System.EventHandler(this.rbCurveBasisEWNo_CheckedChanged);
				this.rbCurveBasisEWYes.CheckedChanged -= new System.EventHandler(this.rbCurveBasisEWYes_CheckedChanged);
				this.tsmiMerchBasisInsertBefore.Click -= new System.EventHandler(this.tsmiMerchBasisInsertBefore_Click);
				this.tsmiMerchBasisInsertAfter.Click -= new System.EventHandler(this.tsmiMerchBasisInsertAfter_Click);
				this.tsmiMerchBasisDelete.Click -= new System.EventHandler(this.tsmiMerchBasisDelete_Click);
				this.tsmiMerchBasisDeleteAll.Click -= new System.EventHandler(this.tsmiMerchBasisDeleteAll_Click);
				this.tsmiCurveBasisInsertBefore.Click -= new System.EventHandler(this.tsmiCurveBasisInsertBefore_Click);
				this.tsmiCurveBasisInsertAfter.Click -= new System.EventHandler(this.tsmiCurveBasisInsertAfter_Click);
				this.tsmiCurveBasisDelete.Click -= new System.EventHandler(this.tsmiCurveBasisDelete_Click);
				this.tsmiCurveBasisDeleteAll.Click -= new System.EventHandler(this.tsmiCurveBasisDeleteAll_Click);

                this.cboSizeCurveBySet.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler(cboSizeCurveBySet_MIDComboBoxPropertiesChangedEvent);
			}

			base.Dispose(disposing);
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
            this.tabSizeCurveMethod = new System.Windows.Forms.TabControl();
            this.tabMethod = new System.Windows.Forms.TabPage();
            this.grpSizeCurvesBy = new System.Windows.Forms.GroupBox();
            this.cboSizeCurveBySet = new MIDRetail.Windows.Controls.MIDAttributeComboBox();
            this.rdoSizeCurvesBySet = new System.Windows.Forms.RadioButton();
            this.rdoSizeCurvesByStore = new System.Windows.Forms.RadioButton();
            this.tabSizeCurveMethodOptions = new System.Windows.Forms.TabControl();
            this.tabMerchandiseBasis = new System.Windows.Forms.TabPage();
            this.rbMerchBasisEWNo = new System.Windows.Forms.RadioButton();
            this.rbMerchBasisEWYes = new System.Windows.Forms.RadioButton();
            this.lblMerchBasisEqualizeWgt = new System.Windows.Forms.Label();
            this.grdMerchBasis = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.tabTolerance = new System.Windows.Forms.TabPage();
            this.grpMinMaxTolerancePct = new System.Windows.Forms.GroupBox();
            this.cbxApplyMinToZeroTolerance = new System.Windows.Forms.CheckBox();
            this.txtMaximumPct = new System.Windows.Forms.TextBox();
            this.lblMaximumPct = new System.Windows.Forms.Label();
            this.txtMinimumPct = new System.Windows.Forms.TextBox();
            this.lblMinimumPct = new System.Windows.Forms.Label();
            this.grpApplyChainSales = new System.Windows.Forms.GroupBox();
            this.grpIndexUnits = new System.Windows.Forms.GroupBox();
            this.rdoIndexToAverage = new System.Windows.Forms.RadioButton();
            this.rdoUnits = new System.Windows.Forms.RadioButton();
            this.txtSalesTolerance = new System.Windows.Forms.TextBox();
            this.lblSalesTolerance = new System.Windows.Forms.Label();
            this.gHigherLevelSalesTolerance = new System.Windows.Forms.GroupBox();
            this.lblMinimumAvgPerSize = new System.Windows.Forms.Label();
            this.txtMinimumAvgPerSize = new System.Windows.Forms.TextBox();
            this.tabSizeCurveBasis = new System.Windows.Forms.TabPage();
            this.grdCurveBasis = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.rbCurveBasisEWNo = new System.Windows.Forms.RadioButton();
            this.rbCurveBasisEWYes = new System.Windows.Forms.RadioButton();
            this.lblCurveBasisEqualizeWgt = new System.Windows.Forms.Label();
            this.tabProperties = new System.Windows.Forms.TabPage();
            this.ugWorkflows = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.cmsMerchBasisGrid = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmiMerchBasisInsert = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiMerchBasisInsertBefore = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiMerchBasisInsertAfter = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiMerchBasisDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiMerchBasisDeleteAll = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsCurveBasisGrid = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmiCurveBasisInsert = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiCurveBasisInsertBefore = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiCurveBasisInsertAfter = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiCurveBasisDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiCurveBasisDeleteAll = new System.Windows.Forms.ToolStripMenuItem();
            this.cachedAuditReclassReport21 = new MIDRetail.Windows.CrystalReports.CachedAuditReclassReport2();
            ((System.ComponentModel.ISupportInitialize)(this.ugRules)).BeginInit();
            this.gbGenericSizeCurve.SuspendLayout();
            this.gbSizeCurve.SuspendLayout();
            this.gbSizeConstraints.SuspendLayout();
            this.gbGenericConstraint.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxCurve)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxConstraint)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxGroup)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxAlternate)).BeginInit();
            this.gbSizeGroup.SuspendLayout();
            this.gbSizeAlternate.SuspendLayout();
            this.gbxNormalizeSizeCurves.SuspendLayout();
            this.pnlGlobalUser.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.utmMain)).BeginInit();
            this.tabSizeCurveMethod.SuspendLayout();
            this.tabMethod.SuspendLayout();
            this.grpSizeCurvesBy.SuspendLayout();
            this.tabSizeCurveMethodOptions.SuspendLayout();
            this.tabMerchandiseBasis.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdMerchBasis)).BeginInit();
            this.tabTolerance.SuspendLayout();
            this.grpMinMaxTolerancePct.SuspendLayout();
            this.grpApplyChainSales.SuspendLayout();
            this.grpIndexUnits.SuspendLayout();
            this.gHigherLevelSalesTolerance.SuspendLayout();
            this.tabSizeCurveBasis.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdCurveBasis)).BeginInit();
            this.tabProperties.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ugWorkflows)).BeginInit();
            this.cmsMerchBasisGrid.SuspendLayout();
            this.cmsCurveBasisGrid.SuspendLayout();
            this.SuspendLayout();
            // 
            // cboSizeGroup
            // 
            // Begin TT#301-MD - JSmith - Controls are not functioning properly
            //this.cboSizeGroup.SelectionChangeCommitted += new System.EventHandler(this.cboSizeGroup_SelectionChangeCommitted);
            //this.cboSizeGroup.SelectionChangeCommitted += new System.EventHandler(this.cboSizeGroup_SelectionChangeCommitted);
            // End TT#301-MD - JSmith - Controls are not functioning properly
            // 
            // lblStoreAttribute
            // 
            this.lblStoreAttribute.Visible = false;
            // 
            // cboStoreAttribute
            // 
            this.cboStoreAttribute.Visible = false;
            // 
            // cboFilter
            // 
            this.cboFilter.Visible = false;
            // 
            // lblFilter
            // 
            this.lblFilter.Visible = false;
            // 
            // ugRules
            // 
            appearance1.BackColor = System.Drawing.Color.White;
            appearance1.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance1.BackGradientStyle = Infragistics.Win.GradientStyle.ForwardDiagonal;
            this.ugRules.DisplayLayout.Appearance = appearance1;
            this.ugRules.DisplayLayout.InterBandSpacing = 10;
            appearance2.BackColor = System.Drawing.Color.Transparent;
            this.ugRules.DisplayLayout.Override.CardAreaAppearance = appearance2;
            appearance3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance3.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance3.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance3.ForeColor = System.Drawing.Color.Black;
            appearance3.TextHAlignAsString = "Left";
            appearance3.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            this.ugRules.DisplayLayout.Override.HeaderAppearance = appearance3;
            appearance4.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.ugRules.DisplayLayout.Override.RowAppearance = appearance4;
            appearance5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance5.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance5.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            this.ugRules.DisplayLayout.Override.RowSelectorAppearance = appearance5;
            this.ugRules.DisplayLayout.Override.RowSelectorWidth = 12;
            this.ugRules.DisplayLayout.Override.RowSpacingBefore = 2;
            appearance6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance6.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance6.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance6.ForeColor = System.Drawing.Color.Black;
            this.ugRules.DisplayLayout.Override.SelectedRowAppearance = appearance6;
            this.ugRules.DisplayLayout.RowConnectorColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.ugRules.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.Solid;
            this.ugRules.Visible = false;
            // 
            // cbExpandAll
            // 
            this.cbExpandAll.Visible = false;
            // 
            // gbSizeCurve
            // 
            this.gbSizeCurve.Visible = false;
            // 
            // gbSizeConstraints
            // 
            this.gbSizeConstraints.Visible = false;
            // 
            // gbSizeGroup
            // 
            this.gbSizeGroup.Location = new System.Drawing.Point(11, 7);
            // 
            // gbSizeAlternate
            // 
            this.gbSizeAlternate.Visible = false;
            // 
            // gbxNormalizeSizeCurves
            // 
            this.gbxNormalizeSizeCurves.Visible = false;
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(666, 573);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(585, 573);
            // 
            // btnProcess
            // 
            this.btnProcess.Location = new System.Drawing.Point(12, 573);
            // 
            // pnlGlobalUser
            // 
            this.pnlGlobalUser.Location = new System.Drawing.Point(611, 12);
            // 
            // txtName
            // 
            this.txtName.TextChanged += new System.EventHandler(this.txtName_TextChanged);
            // 
            // utmMain
            // 
            this.utmMain.MenuSettings.ForceSerialization = true;
            this.utmMain.ToolbarSettings.ForceSerialization = true;
            // 
            // tabSizeCurveMethod
            // 
            this.tabSizeCurveMethod.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabSizeCurveMethod.Controls.Add(this.tabMethod);
            this.tabSizeCurveMethod.Controls.Add(this.tabProperties);
            this.tabSizeCurveMethod.Location = new System.Drawing.Point(17, 43);
            this.tabSizeCurveMethod.Name = "tabSizeCurveMethod";
            this.tabSizeCurveMethod.SelectedIndex = 0;
            this.tabSizeCurveMethod.Size = new System.Drawing.Size(723, 517);
            this.tabSizeCurveMethod.TabIndex = 60;
            // 
            // tabMethod
            // 
            this.tabMethod.Controls.Add(this.grpSizeCurvesBy);
            this.tabMethod.Controls.Add(this.tabSizeCurveMethodOptions);
            this.tabMethod.Controls.Add(this.gbSizeGroup);
            this.tabMethod.Location = new System.Drawing.Point(4, 22);
            this.tabMethod.Name = "tabMethod";
            this.tabMethod.Size = new System.Drawing.Size(715, 491);
            this.tabMethod.TabIndex = 1;
            this.tabMethod.Text = "Method";
            this.tabMethod.Controls.SetChildIndex(this.gbSizeGroup, 0);
            this.tabMethod.Controls.SetChildIndex(this.tabSizeCurveMethodOptions, 0);
            this.tabMethod.Controls.SetChildIndex(this.grpSizeCurvesBy, 0);
            // 
            // grpSizeCurvesBy
            // 
            this.grpSizeCurvesBy.Controls.Add(this.cboSizeCurveBySet);
            this.grpSizeCurvesBy.Controls.Add(this.rdoSizeCurvesBySet);
            this.grpSizeCurvesBy.Controls.Add(this.rdoSizeCurvesByStore);
            this.grpSizeCurvesBy.Location = new System.Drawing.Point(326, 7);
            this.grpSizeCurvesBy.Name = "grpSizeCurvesBy";
            this.grpSizeCurvesBy.Size = new System.Drawing.Size(373, 68);
            this.grpSizeCurvesBy.TabIndex = 57;
            this.grpSizeCurvesBy.TabStop = false;
            this.grpSizeCurvesBy.Text = "Size Curves By";
            // 
            // cboSizeCurveBySet
            // 
            this.cboSizeCurveBySet.AllowDrop = true;
            this.cboSizeCurveBySet.AllowUserAttributes = false;
            this.cboSizeCurveBySet.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboSizeCurveBySet.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboSizeCurveBySet.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSizeCurveBySet.FormattingEnabled = true;
            this.cboSizeCurveBySet.Location = new System.Drawing.Point(72, 38);
            this.cboSizeCurveBySet.Name = "cboSizeCurveBySet";
            this.cboSizeCurveBySet.Size = new System.Drawing.Size(249, 21);
            this.cboSizeCurveBySet.TabIndex = 3;
            // Begin TT#301-MD - JSmith - Controls are not functioning properly
            //this.cboSizeCurveBySet.SelectionChangeCommitted += new System.EventHandler(this.cboSizeCurveBySet_SelectionChangeCommitted);
            this.cboSizeCurveBySet.SelectionChangeCommitted += new System.EventHandler(this.cboSizeCurveBySet_SelectionChangeCommitted);
            // End TT#301-MD - JSmith - Controls are not functioning properly
            this.cboSizeCurveBySet.MIDComboBoxPropertiesChangedEvent += new MIDComboBoxPropertiesChangedEventHandler(cboSizeCurveBySet_MIDComboBoxPropertiesChangedEvent);
            this.cboSizeCurveBySet.DragDrop += new System.Windows.Forms.DragEventHandler(this.cboSizeCurveBySet_DragDrop);
            this.cboSizeCurveBySet.DragEnter += new System.Windows.Forms.DragEventHandler(this.cboSizeCurveBySet_DragEnter);
            this.cboSizeCurveBySet.DragOver += new System.Windows.Forms.DragEventHandler(this.cboSizeCurveBySet_DragOver);
            // 
            // rdoSizeCurvesBySet
            // 
            this.rdoSizeCurvesBySet.AutoSize = true;
            this.rdoSizeCurvesBySet.Location = new System.Drawing.Point(15, 42);
            this.rdoSizeCurvesBySet.Name = "rdoSizeCurvesBySet";
            this.rdoSizeCurvesBySet.Size = new System.Drawing.Size(41, 17);
            this.rdoSizeCurvesBySet.TabIndex = 1;
            this.rdoSizeCurvesBySet.TabStop = true;
            this.rdoSizeCurvesBySet.Text = "Set";
            this.rdoSizeCurvesBySet.UseVisualStyleBackColor = true;
            this.rdoSizeCurvesBySet.CheckedChanged += new System.EventHandler(this.rdoSizeCurvesBySet_CheckedChanged);
            // 
            // rdoSizeCurvesByStore
            // 
            this.rdoSizeCurvesByStore.AutoSize = true;
            this.rdoSizeCurvesByStore.Location = new System.Drawing.Point(15, 19);
            this.rdoSizeCurvesByStore.Name = "rdoSizeCurvesByStore";
            this.rdoSizeCurvesByStore.Size = new System.Drawing.Size(50, 17);
            this.rdoSizeCurvesByStore.TabIndex = 0;
            this.rdoSizeCurvesByStore.TabStop = true;
            this.rdoSizeCurvesByStore.Text = "Store";
            this.rdoSizeCurvesByStore.UseVisualStyleBackColor = true;
            this.rdoSizeCurvesByStore.CheckedChanged += new System.EventHandler(this.rdoSizeCurvesByStore_CheckedChanged);
            // 
            // tabSizeCurveMethodOptions
            // 
            this.tabSizeCurveMethodOptions.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabSizeCurveMethodOptions.Controls.Add(this.tabMerchandiseBasis);
            this.tabSizeCurveMethodOptions.Controls.Add(this.tabTolerance);
            this.tabSizeCurveMethodOptions.Controls.Add(this.tabSizeCurveBasis);
            this.tabSizeCurveMethodOptions.Location = new System.Drawing.Point(11, 72);
            this.tabSizeCurveMethodOptions.Name = "tabSizeCurveMethodOptions";
            this.tabSizeCurveMethodOptions.SelectedIndex = 0;
            this.tabSizeCurveMethodOptions.Size = new System.Drawing.Size(692, 408);
            this.tabSizeCurveMethodOptions.TabIndex = 56;
            // 
            // tabMerchandiseBasis
            // 
            this.tabMerchandiseBasis.BackColor = System.Drawing.Color.Transparent;
            this.tabMerchandiseBasis.Controls.Add(this.rbMerchBasisEWNo);
            this.tabMerchandiseBasis.Controls.Add(this.rbMerchBasisEWYes);
            this.tabMerchandiseBasis.Controls.Add(this.lblMerchBasisEqualizeWgt);
            this.tabMerchandiseBasis.Controls.Add(this.grdMerchBasis);
            this.tabMerchandiseBasis.Location = new System.Drawing.Point(4, 22);
            this.tabMerchandiseBasis.Name = "tabMerchandiseBasis";
            this.tabMerchandiseBasis.Padding = new System.Windows.Forms.Padding(3);
            this.tabMerchandiseBasis.Size = new System.Drawing.Size(684, 382);
            this.tabMerchandiseBasis.TabIndex = 0;
            this.tabMerchandiseBasis.Text = "Merchandise Basis";
            this.tabMerchandiseBasis.UseVisualStyleBackColor = true;
            // 
            // rbMerchBasisEWNo
            // 
            this.rbMerchBasisEWNo.Location = new System.Drawing.Point(584, 6);
            this.rbMerchBasisEWNo.Name = "rbMerchBasisEWNo";
            this.rbMerchBasisEWNo.Size = new System.Drawing.Size(48, 16);
            this.rbMerchBasisEWNo.TabIndex = 6;
            this.rbMerchBasisEWNo.Text = "No";
            this.rbMerchBasisEWNo.CheckedChanged += new System.EventHandler(this.rbMerchBasisEWNo_CheckedChanged);
            // 
            // rbMerchBasisEWYes
            // 
            this.rbMerchBasisEWYes.Checked = true;
            this.rbMerchBasisEWYes.Location = new System.Drawing.Point(534, 6);
            this.rbMerchBasisEWYes.Name = "rbMerchBasisEWYes";
            this.rbMerchBasisEWYes.Size = new System.Drawing.Size(48, 16);
            this.rbMerchBasisEWYes.TabIndex = 5;
            this.rbMerchBasisEWYes.TabStop = true;
            this.rbMerchBasisEWYes.Text = "Yes";
            this.rbMerchBasisEWYes.CheckedChanged += new System.EventHandler(this.rbMerchBasisEWYes_CheckedChanged);
            // 
            // lblMerchBasisEqualizeWgt
            // 
            this.lblMerchBasisEqualizeWgt.Location = new System.Drawing.Point(424, 8);
            this.lblMerchBasisEqualizeWgt.Name = "lblMerchBasisEqualizeWgt";
            this.lblMerchBasisEqualizeWgt.Size = new System.Drawing.Size(104, 16);
            this.lblMerchBasisEqualizeWgt.TabIndex = 4;
            this.lblMerchBasisEqualizeWgt.Text = "Equalize Weighting:";
            // 
            // grdMerchBasis
            // 
            this.grdMerchBasis.AllowDrop = true;
            this.grdMerchBasis.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grdMerchBasis.Location = new System.Drawing.Point(15, 27);
            this.grdMerchBasis.Name = "grdMerchBasis";
            this.grdMerchBasis.Size = new System.Drawing.Size(652, 340);
            this.grdMerchBasis.TabIndex = 0;
            this.grdMerchBasis.AfterCellUpdate += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdMerchBasis_AfterCellUpdate);
            this.grdMerchBasis.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.grdMerchBasis_InitializeLayout);
            this.grdMerchBasis.InitializeRow += new Infragistics.Win.UltraWinGrid.InitializeRowEventHandler(this.grdMerchBasis_InitializeRow);
            this.grdMerchBasis.CellChange += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdMerchBasis_CellChange);
            this.grdMerchBasis.ClickCellButton += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdMerchBasis_ClickCellButton);
            this.grdMerchBasis.AfterCellListCloseUp += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdMerchBasis_AfterCellListCloseUp);
            this.grdMerchBasis.BeforeCellDeactivate += new System.ComponentModel.CancelEventHandler(this.grdMerchBasis_BeforeCellDeactivate);
            this.grdMerchBasis.BeforeRowInsert += new Infragistics.Win.UltraWinGrid.BeforeRowInsertEventHandler(this.grdMerchBasis_BeforeRowInsert);
            this.grdMerchBasis.DragDrop += new System.Windows.Forms.DragEventHandler(this.grdMerchBasis_DragDrop);
            this.grdMerchBasis.DragEnter += new System.Windows.Forms.DragEventHandler(this.grdMerchBasis_DragEnter);
            this.grdMerchBasis.DragOver += new System.Windows.Forms.DragEventHandler(this.grdMerchBasis_DragOver);
            // 
            // tabTolerance
            // 
            this.tabTolerance.Controls.Add(this.grpMinMaxTolerancePct);
            this.tabTolerance.Controls.Add(this.grpApplyChainSales);
            this.tabTolerance.Controls.Add(this.gHigherLevelSalesTolerance);
            this.tabTolerance.Location = new System.Drawing.Point(4, 22);
            this.tabTolerance.Name = "tabTolerance";
            this.tabTolerance.Size = new System.Drawing.Size(684, 382);
            this.tabTolerance.TabIndex = 2;
            this.tabTolerance.Text = "Tolerance";
            this.tabTolerance.UseVisualStyleBackColor = true;
            // 
            // grpMinMaxTolerancePct
            // 
            this.grpMinMaxTolerancePct.Controls.Add(this.cbxApplyMinToZeroTolerance);
            this.grpMinMaxTolerancePct.Controls.Add(this.txtMaximumPct);
            this.grpMinMaxTolerancePct.Controls.Add(this.lblMaximumPct);
            this.grpMinMaxTolerancePct.Controls.Add(this.txtMinimumPct);
            this.grpMinMaxTolerancePct.Controls.Add(this.lblMinimumPct);
            this.grpMinMaxTolerancePct.Location = new System.Drawing.Point(35, 244);
            this.grpMinMaxTolerancePct.Name = "grpMinMaxTolerancePct";
            this.grpMinMaxTolerancePct.Size = new System.Drawing.Size(578, 88);
            this.grpMinMaxTolerancePct.TabIndex = 9;
            this.grpMinMaxTolerancePct.TabStop = false;
            this.grpMinMaxTolerancePct.Text = "Min/Max Tolerance %";
            // 
            // cbxApplyMinToZeroTolerance
            // 
            this.cbxApplyMinToZeroTolerance.AutoSize = true;
            this.cbxApplyMinToZeroTolerance.Enabled = false;
            this.cbxApplyMinToZeroTolerance.Location = new System.Drawing.Point(56, 65);
            this.cbxApplyMinToZeroTolerance.Name = "cbxApplyMinToZeroTolerance";
            this.cbxApplyMinToZeroTolerance.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.cbxApplyMinToZeroTolerance.Size = new System.Drawing.Size(184, 17);
            this.cbxApplyMinToZeroTolerance.TabIndex = 7;
            this.cbxApplyMinToZeroTolerance.Text = "Apply Minimum to Zero Tolerance";
            this.cbxApplyMinToZeroTolerance.UseVisualStyleBackColor = true;
            this.cbxApplyMinToZeroTolerance.CheckedChanged += new System.EventHandler(this.cbxApplyMinToZeroTolerance_CheckChanged);
            // 
            // txtMaximumPct
            // 
            this.txtMaximumPct.Location = new System.Drawing.Point(368, 28);
            this.txtMaximumPct.Name = "txtMaximumPct";
            this.txtMaximumPct.Size = new System.Drawing.Size(67, 20);
            this.txtMaximumPct.TabIndex = 6;
            this.txtMaximumPct.TextChanged += new System.EventHandler(this.txtMaximumPct_TextChanged);
            // 
            // lblMaximumPct
            // 
            this.lblMaximumPct.AutoSize = true;
            this.lblMaximumPct.Location = new System.Drawing.Point(300, 31);
            this.lblMaximumPct.Name = "lblMaximumPct";
            this.lblMaximumPct.Size = new System.Drawing.Size(62, 13);
            this.lblMaximumPct.TabIndex = 5;
            this.lblMaximumPct.Text = "Maximum %";
            this.lblMaximumPct.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // txtMinimumPct
            // 
            this.txtMinimumPct.Location = new System.Drawing.Point(175, 28);
            this.txtMinimumPct.Name = "txtMinimumPct";
            this.txtMinimumPct.Size = new System.Drawing.Size(67, 20);
            this.txtMinimumPct.TabIndex = 4;
            this.txtMinimumPct.TextChanged += new System.EventHandler(this.txtMinimumPct_TextChanged);
            // 
            // lblMinimumPct
            // 
            this.lblMinimumPct.AutoSize = true;
            this.lblMinimumPct.Location = new System.Drawing.Point(110, 31);
            this.lblMinimumPct.Name = "lblMinimumPct";
            this.lblMinimumPct.Size = new System.Drawing.Size(59, 13);
            this.lblMinimumPct.TabIndex = 3;
            this.lblMinimumPct.Text = "Minimum %";
            this.lblMinimumPct.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // grpApplyChainSales
            // 
            this.grpApplyChainSales.Controls.Add(this.grpIndexUnits);
            this.grpApplyChainSales.Controls.Add(this.txtSalesTolerance);
            this.grpApplyChainSales.Controls.Add(this.lblSalesTolerance);
            this.grpApplyChainSales.Location = new System.Drawing.Point(35, 136);
            this.grpApplyChainSales.Name = "grpApplyChainSales";
            this.grpApplyChainSales.Size = new System.Drawing.Size(578, 73);
            this.grpApplyChainSales.TabIndex = 8;
            this.grpApplyChainSales.TabStop = false;
            this.grpApplyChainSales.Text = "Apply Chain/Set Sales";
            // 
            // grpIndexUnits
            // 
            this.grpIndexUnits.Controls.Add(this.rdoIndexToAverage);
            this.grpIndexUnits.Controls.Add(this.rdoUnits);
            this.grpIndexUnits.Location = new System.Drawing.Point(305, 17);
            this.grpIndexUnits.Name = "grpIndexUnits";
            this.grpIndexUnits.Size = new System.Drawing.Size(237, 39);
            this.grpIndexUnits.TabIndex = 5;
            this.grpIndexUnits.TabStop = false;
            // 
            // rdoIndexToAverage
            // 
            this.rdoIndexToAverage.AutoSize = true;
            this.rdoIndexToAverage.Location = new System.Drawing.Point(23, 13);
            this.rdoIndexToAverage.Name = "rdoIndexToAverage";
            this.rdoIndexToAverage.Size = new System.Drawing.Size(106, 17);
            this.rdoIndexToAverage.TabIndex = 3;
            this.rdoIndexToAverage.TabStop = true;
            this.rdoIndexToAverage.Text = "Index to Average";
            this.rdoIndexToAverage.UseVisualStyleBackColor = true;
            this.rdoIndexToAverage.CheckedChanged += new System.EventHandler(this.rdoIndexToAverage_CheckedChanged);
            // 
            // rdoUnits
            // 
            this.rdoUnits.AutoSize = true;
            this.rdoUnits.Location = new System.Drawing.Point(161, 13);
            this.rdoUnits.Name = "rdoUnits";
            this.rdoUnits.Size = new System.Drawing.Size(49, 17);
            this.rdoUnits.TabIndex = 4;
            this.rdoUnits.TabStop = true;
            this.rdoUnits.Text = "Units";
            this.rdoUnits.UseVisualStyleBackColor = true;
            this.rdoUnits.CheckedChanged += new System.EventHandler(this.rdoUnits_CheckedChanged);
            // 
            // txtSalesTolerance
            // 
            this.txtSalesTolerance.Location = new System.Drawing.Point(175, 29);
            this.txtSalesTolerance.Name = "txtSalesTolerance";
            this.txtSalesTolerance.Size = new System.Drawing.Size(67, 20);
            this.txtSalesTolerance.TabIndex = 2;
            this.txtSalesTolerance.TextChanged += new System.EventHandler(this.txtSalesTolerance_TextChanged);
            // 
            // lblSalesTolerance
            // 
            this.lblSalesTolerance.AutoSize = true;
            this.lblSalesTolerance.Location = new System.Drawing.Point(85, 32);
            this.lblSalesTolerance.Name = "lblSalesTolerance";
            this.lblSalesTolerance.Size = new System.Drawing.Size(84, 13);
            this.lblSalesTolerance.TabIndex = 1;
            this.lblSalesTolerance.Text = "Sales Tolerance";
            this.lblSalesTolerance.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // gHigherLevelSalesTolerance
            // 
            this.gHigherLevelSalesTolerance.Controls.Add(this.lblMinimumAvgPerSize);
            this.gHigherLevelSalesTolerance.Controls.Add(this.txtMinimumAvgPerSize);
            this.gHigherLevelSalesTolerance.Location = new System.Drawing.Point(35, 28);
            this.gHigherLevelSalesTolerance.Name = "gHigherLevelSalesTolerance";
            this.gHigherLevelSalesTolerance.Size = new System.Drawing.Size(578, 73);
            this.gHigherLevelSalesTolerance.TabIndex = 7;
            this.gHigherLevelSalesTolerance.TabStop = false;
            this.gHigherLevelSalesTolerance.Text = "Higher Level Sales Tolerance";
            // 
            // lblMinimumAvgPerSize
            // 
            this.lblMinimumAvgPerSize.AutoSize = true;
            this.lblMinimumAvgPerSize.Location = new System.Drawing.Point(37, 31);
            this.lblMinimumAvgPerSize.Name = "lblMinimumAvgPerSize";
            this.lblMinimumAvgPerSize.Size = new System.Drawing.Size(132, 13);
            this.lblMinimumAvgPerSize.TabIndex = 0;
            this.lblMinimumAvgPerSize.Text = "Minimum Average per Size";
            this.lblMinimumAvgPerSize.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // txtMinimumAvgPerSize
            // 
            this.txtMinimumAvgPerSize.Location = new System.Drawing.Point(175, 28);
            this.txtMinimumAvgPerSize.Name = "txtMinimumAvgPerSize";
            this.txtMinimumAvgPerSize.Size = new System.Drawing.Size(67, 20);
            this.txtMinimumAvgPerSize.TabIndex = 1;
            this.txtMinimumAvgPerSize.TextChanged += new System.EventHandler(this.txtMinimumAvgPerSize_TextChanged);
            // 
            // tabSizeCurveBasis
            // 
            this.tabSizeCurveBasis.BackColor = System.Drawing.Color.Transparent;
            this.tabSizeCurveBasis.Controls.Add(this.grdCurveBasis);
            this.tabSizeCurveBasis.Controls.Add(this.rbCurveBasisEWNo);
            this.tabSizeCurveBasis.Controls.Add(this.rbCurveBasisEWYes);
            this.tabSizeCurveBasis.Controls.Add(this.lblCurveBasisEqualizeWgt);
            this.tabSizeCurveBasis.Location = new System.Drawing.Point(4, 22);
            this.tabSizeCurveBasis.Name = "tabSizeCurveBasis";
            this.tabSizeCurveBasis.Padding = new System.Windows.Forms.Padding(3);
            this.tabSizeCurveBasis.Size = new System.Drawing.Size(684, 382);
            this.tabSizeCurveBasis.TabIndex = 1;
            this.tabSizeCurveBasis.Text = "Size Curve Basis";
            this.tabSizeCurveBasis.UseVisualStyleBackColor = true;
            // 
            // grdCurveBasis
            // 
            this.grdCurveBasis.AllowDrop = true;
            this.grdCurveBasis.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grdCurveBasis.Location = new System.Drawing.Point(15, 27);
            this.grdCurveBasis.Name = "grdCurveBasis";
            this.grdCurveBasis.Size = new System.Drawing.Size(652, 340);
            this.grdCurveBasis.TabIndex = 10;
            this.grdCurveBasis.AfterCellUpdate += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdCurveBasis_AfterCellUpdate);
            this.grdCurveBasis.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.grdCurveBasis_InitializeLayout);
            this.grdCurveBasis.InitializeRow += new Infragistics.Win.UltraWinGrid.InitializeRowEventHandler(this.grdCurveBasis_InitializeRow);
            this.grdCurveBasis.ClickCellButton += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdCurveBasis_ClickCellButton);
            this.grdCurveBasis.AfterCellListCloseUp += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdCurveBasis_AfterCellListCloseUp);
            this.grdCurveBasis.BeforeCellDeactivate += new System.ComponentModel.CancelEventHandler(this.grdCurveBasis_BeforeCellDeactivate);
            this.grdCurveBasis.BeforeRowInsert += new Infragistics.Win.UltraWinGrid.BeforeRowInsertEventHandler(this.grdCurveBasis_BeforeRowInsert);
            this.grdCurveBasis.DragDrop += new System.Windows.Forms.DragEventHandler(this.grdCurveBasis_DragDrop);
            this.grdCurveBasis.DragEnter += new System.Windows.Forms.DragEventHandler(this.grdCurveBasis_DragEnter);
            this.grdCurveBasis.DragOver += new System.Windows.Forms.DragEventHandler(this.grdCurveBasis_DragOver);
            // 
            // rbCurveBasisEWNo
            // 
            this.rbCurveBasisEWNo.Location = new System.Drawing.Point(584, 6);
            this.rbCurveBasisEWNo.Name = "rbCurveBasisEWNo";
            this.rbCurveBasisEWNo.Size = new System.Drawing.Size(48, 16);
            this.rbCurveBasisEWNo.TabIndex = 9;
            this.rbCurveBasisEWNo.Text = "No";
            this.rbCurveBasisEWNo.CheckedChanged += new System.EventHandler(this.rbCurveBasisEWNo_CheckedChanged);
            // 
            // rbCurveBasisEWYes
            // 
            this.rbCurveBasisEWYes.Checked = true;
            this.rbCurveBasisEWYes.Location = new System.Drawing.Point(534, 6);
            this.rbCurveBasisEWYes.Name = "rbCurveBasisEWYes";
            this.rbCurveBasisEWYes.Size = new System.Drawing.Size(48, 16);
            this.rbCurveBasisEWYes.TabIndex = 8;
            this.rbCurveBasisEWYes.TabStop = true;
            this.rbCurveBasisEWYes.Text = "Yes";
            this.rbCurveBasisEWYes.CheckedChanged += new System.EventHandler(this.rbCurveBasisEWYes_CheckedChanged);
            // 
            // lblCurveBasisEqualizeWgt
            // 
            this.lblCurveBasisEqualizeWgt.Location = new System.Drawing.Point(424, 8);
            this.lblCurveBasisEqualizeWgt.Name = "lblCurveBasisEqualizeWgt";
            this.lblCurveBasisEqualizeWgt.Size = new System.Drawing.Size(104, 16);
            this.lblCurveBasisEqualizeWgt.TabIndex = 7;
            this.lblCurveBasisEqualizeWgt.Text = "Equalize Weighting:";
            // 
            // tabProperties
            // 
            this.tabProperties.Controls.Add(this.ugWorkflows);
            this.tabProperties.Location = new System.Drawing.Point(4, 22);
            this.tabProperties.Name = "tabProperties";
            this.tabProperties.Size = new System.Drawing.Size(715, 491);
            this.tabProperties.TabIndex = 0;
            this.tabProperties.Text = "Properties";
            this.tabProperties.Visible = false;
            // 
            // ugWorkflows
            // 
            this.ugWorkflows.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
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
            this.ugWorkflows.Location = new System.Drawing.Point(16, 52);
            this.ugWorkflows.Name = "ugWorkflows";
            this.ugWorkflows.Size = new System.Drawing.Size(683, 424);
            this.ugWorkflows.TabIndex = 1;
            // 
            // cmsMerchBasisGrid
            // 
            this.cmsMerchBasisGrid.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiMerchBasisInsert,
            this.tsmiMerchBasisDelete,
            this.tsmiMerchBasisDeleteAll});
            this.cmsMerchBasisGrid.Name = "cmsBasisGrid";
            this.cmsMerchBasisGrid.Size = new System.Drawing.Size(125, 70);
            // 
            // tsmiMerchBasisInsert
            // 
            this.tsmiMerchBasisInsert.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiMerchBasisInsertBefore,
            this.tsmiMerchBasisInsertAfter});
            this.tsmiMerchBasisInsert.Name = "tsmiMerchBasisInsert";
            this.tsmiMerchBasisInsert.Size = new System.Drawing.Size(124, 22);
            this.tsmiMerchBasisInsert.Text = "Insert";
            // 
            // tsmiMerchBasisInsertBefore
            // 
            this.tsmiMerchBasisInsertBefore.Name = "tsmiMerchBasisInsertBefore";
            this.tsmiMerchBasisInsertBefore.Size = new System.Drawing.Size(108, 22);
            this.tsmiMerchBasisInsertBefore.Text = "Before";
            this.tsmiMerchBasisInsertBefore.Click += new System.EventHandler(this.tsmiMerchBasisInsertBefore_Click);
            // 
            // tsmiMerchBasisInsertAfter
            // 
            this.tsmiMerchBasisInsertAfter.Name = "tsmiMerchBasisInsertAfter";
            this.tsmiMerchBasisInsertAfter.Size = new System.Drawing.Size(108, 22);
            this.tsmiMerchBasisInsertAfter.Text = "After";
            this.tsmiMerchBasisInsertAfter.Click += new System.EventHandler(this.tsmiMerchBasisInsertAfter_Click);
            // 
            // tsmiMerchBasisDelete
            // 
            this.tsmiMerchBasisDelete.Name = "tsmiMerchBasisDelete";
            this.tsmiMerchBasisDelete.Size = new System.Drawing.Size(124, 22);
            this.tsmiMerchBasisDelete.Text = "Delete";
            this.tsmiMerchBasisDelete.Click += new System.EventHandler(this.tsmiMerchBasisDelete_Click);
            // 
            // tsmiMerchBasisDeleteAll
            // 
            this.tsmiMerchBasisDeleteAll.Name = "tsmiMerchBasisDeleteAll";
            this.tsmiMerchBasisDeleteAll.Size = new System.Drawing.Size(124, 22);
            this.tsmiMerchBasisDeleteAll.Text = "Delete All";
            this.tsmiMerchBasisDeleteAll.Click += new System.EventHandler(this.tsmiMerchBasisDeleteAll_Click);
            // 
            // cmsCurveBasisGrid
            // 
            this.cmsCurveBasisGrid.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiCurveBasisInsert,
            this.tsmiCurveBasisDelete,
            this.tsmiCurveBasisDeleteAll});
            this.cmsCurveBasisGrid.Name = "cmsCurveBasisGrid";
            this.cmsCurveBasisGrid.Size = new System.Drawing.Size(125, 70);
            // 
            // tsmiCurveBasisInsert
            // 
            this.tsmiCurveBasisInsert.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiCurveBasisInsertBefore,
            this.tsmiCurveBasisInsertAfter});
            this.tsmiCurveBasisInsert.Name = "tsmiCurveBasisInsert";
            this.tsmiCurveBasisInsert.Size = new System.Drawing.Size(124, 22);
            this.tsmiCurveBasisInsert.Text = "Insert";
            // 
            // tsmiCurveBasisInsertBefore
            // 
            this.tsmiCurveBasisInsertBefore.Name = "tsmiCurveBasisInsertBefore";
            this.tsmiCurveBasisInsertBefore.Size = new System.Drawing.Size(108, 22);
            this.tsmiCurveBasisInsertBefore.Text = "Before";
            this.tsmiCurveBasisInsertBefore.Click += new System.EventHandler(this.tsmiCurveBasisInsertBefore_Click);
            // 
            // tsmiCurveBasisInsertAfter
            // 
            this.tsmiCurveBasisInsertAfter.Name = "tsmiCurveBasisInsertAfter";
            this.tsmiCurveBasisInsertAfter.Size = new System.Drawing.Size(108, 22);
            this.tsmiCurveBasisInsertAfter.Text = "After";
            this.tsmiCurveBasisInsertAfter.Click += new System.EventHandler(this.tsmiCurveBasisInsertAfter_Click);
            // 
            // tsmiCurveBasisDelete
            // 
            this.tsmiCurveBasisDelete.Name = "tsmiCurveBasisDelete";
            this.tsmiCurveBasisDelete.Size = new System.Drawing.Size(124, 22);
            this.tsmiCurveBasisDelete.Text = "Delete";
            this.tsmiCurveBasisDelete.Click += new System.EventHandler(this.tsmiCurveBasisDelete_Click);
            // 
            // tsmiCurveBasisDeleteAll
            // 
            this.tsmiCurveBasisDeleteAll.Name = "tsmiCurveBasisDeleteAll";
            this.tsmiCurveBasisDeleteAll.Size = new System.Drawing.Size(124, 22);
            this.tsmiCurveBasisDeleteAll.Text = "Delete All";
            this.tsmiCurveBasisDeleteAll.Click += new System.EventHandler(this.tsmiCurveBasisDeleteAll_Click);
            // 
            // frmSizeCurveMethod
            // 
            this.ClientSize = new System.Drawing.Size(753, 608);
            this.Controls.Add(this.tabSizeCurveMethod);
            this.Name = "frmSizeCurveMethod";
            this.Load += new System.EventHandler(this.frmSizeCurveMethod_Load);
            this.Controls.SetChildIndex(this.ugRules, 0);
            this.Controls.SetChildIndex(this.cboStoreAttribute, 0);
            this.Controls.SetChildIndex(this.cboFilter, 0);
            this.Controls.SetChildIndex(this.gbxNormalizeSizeCurves, 0);
            this.Controls.SetChildIndex(this.lblFilter, 0);
            this.Controls.SetChildIndex(this.lblStoreAttribute, 0);
            this.Controls.SetChildIndex(this.cbExpandAll, 0);
            this.Controls.SetChildIndex(this.gbSizeCurve, 0);
            this.Controls.SetChildIndex(this.gbSizeConstraints, 0);
            this.Controls.SetChildIndex(this.gbSizeAlternate, 0);
            this.Controls.SetChildIndex(this.btnSave, 0);
            this.Controls.SetChildIndex(this.btnClose, 0);
            this.Controls.SetChildIndex(this.btnProcess, 0);
            this.Controls.SetChildIndex(this.lblName, 0);
            this.Controls.SetChildIndex(this.txtName, 0);
            this.Controls.SetChildIndex(this.txtDesc, 0);
            this.Controls.SetChildIndex(this.pnlGlobalUser, 0);
            this.Controls.SetChildIndex(this.tabSizeCurveMethod, 0);
            ((System.ComponentModel.ISupportInitialize)(this.ugRules)).EndInit();
            this.gbGenericSizeCurve.ResumeLayout(false);
            this.gbSizeCurve.ResumeLayout(false);
            this.gbSizeCurve.PerformLayout();
            this.gbSizeConstraints.ResumeLayout(false);
            this.gbGenericConstraint.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picBoxCurve)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxConstraint)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxGroup)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxAlternate)).EndInit();
            this.gbSizeGroup.ResumeLayout(false);
            this.gbSizeAlternate.ResumeLayout(false);
            this.gbxNormalizeSizeCurves.ResumeLayout(false);
            this.pnlGlobalUser.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.utmMain)).EndInit();
            this.tabSizeCurveMethod.ResumeLayout(false);
            this.tabMethod.ResumeLayout(false);
            this.grpSizeCurvesBy.ResumeLayout(false);
            this.grpSizeCurvesBy.PerformLayout();
            this.tabSizeCurveMethodOptions.ResumeLayout(false);
            this.tabMerchandiseBasis.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdMerchBasis)).EndInit();
            this.tabTolerance.ResumeLayout(false);
            this.grpMinMaxTolerancePct.ResumeLayout(false);
            this.grpMinMaxTolerancePct.PerformLayout();
            this.grpApplyChainSales.ResumeLayout(false);
            this.grpApplyChainSales.PerformLayout();
            this.grpIndexUnits.ResumeLayout(false);
            this.grpIndexUnits.PerformLayout();
            this.gHigherLevelSalesTolerance.ResumeLayout(false);
            this.gHigherLevelSalesTolerance.PerformLayout();
            this.tabSizeCurveBasis.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdCurveBasis)).EndInit();
            this.tabProperties.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ugWorkflows)).EndInit();
            this.cmsMerchBasisGrid.ResumeLayout(false);
            this.cmsCurveBasisGrid.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TabControl tabSizeCurveMethod;
		private System.Windows.Forms.TabPage tabMethod;
		private Infragistics.Win.UltraWinGrid.UltraGrid grdMerchBasis;
		private System.Windows.Forms.TabPage tabProperties;
		private Infragistics.Win.UltraWinGrid.UltraGrid ugWorkflows;
		private System.Windows.Forms.ContextMenuStrip cmsMerchBasisGrid;
		private System.Windows.Forms.ToolStripMenuItem tsmiMerchBasisInsert;
		private System.Windows.Forms.ToolStripMenuItem tsmiMerchBasisInsertBefore;
		private System.Windows.Forms.ToolStripMenuItem tsmiMerchBasisInsertAfter;
		private System.Windows.Forms.ToolStripMenuItem tsmiMerchBasisDelete;
		private System.Windows.Forms.ToolStripMenuItem tsmiMerchBasisDeleteAll;
		private System.Windows.Forms.TabControl tabSizeCurveMethodOptions;
		private System.Windows.Forms.TabPage tabMerchandiseBasis;
		private System.Windows.Forms.TabPage tabSizeCurveBasis;
		private System.Windows.Forms.RadioButton rbMerchBasisEWNo;
		private System.Windows.Forms.RadioButton rbMerchBasisEWYes;
		private System.Windows.Forms.Label lblMerchBasisEqualizeWgt;
		private System.Windows.Forms.RadioButton rbCurveBasisEWNo;
		private System.Windows.Forms.RadioButton rbCurveBasisEWYes;
		private System.Windows.Forms.Label lblCurveBasisEqualizeWgt;
		private Infragistics.Win.UltraWinGrid.UltraGrid grdCurveBasis;
		private System.Windows.Forms.ContextMenuStrip cmsCurveBasisGrid;
		private System.Windows.Forms.ToolStripMenuItem tsmiCurveBasisInsert;
		private System.Windows.Forms.ToolStripMenuItem tsmiCurveBasisInsertBefore;
		private System.Windows.Forms.ToolStripMenuItem tsmiCurveBasisInsertAfter;
		private System.Windows.Forms.ToolStripMenuItem tsmiCurveBasisDelete;
		private System.Windows.Forms.ToolStripMenuItem tsmiCurveBasisDeleteAll;
		private System.Windows.Forms.TabPage tabTolerance;
		private System.Windows.Forms.GroupBox grpMinMaxTolerancePct;
		private System.Windows.Forms.TextBox txtMaximumPct;
		private System.Windows.Forms.Label lblMaximumPct;
		private System.Windows.Forms.TextBox txtMinimumPct;
		private System.Windows.Forms.Label lblMinimumPct;
		private System.Windows.Forms.GroupBox grpApplyChainSales;
		private System.Windows.Forms.GroupBox grpIndexUnits;
		private System.Windows.Forms.RadioButton rdoIndexToAverage;
		private System.Windows.Forms.RadioButton rdoUnits;
		private System.Windows.Forms.TextBox txtSalesTolerance;
		private System.Windows.Forms.Label lblSalesTolerance;
		private System.Windows.Forms.GroupBox gHigherLevelSalesTolerance;
		private System.Windows.Forms.Label lblMinimumAvgPerSize;
		private System.Windows.Forms.TextBox txtMinimumAvgPerSize;
		private System.Windows.Forms.GroupBox grpSizeCurvesBy;
		private System.Windows.Forms.RadioButton rdoSizeCurvesBySet;
		private System.Windows.Forms.RadioButton rdoSizeCurvesByStore;
		private MIDRetail.Windows.Controls.MIDAttributeComboBox cboSizeCurveBySet;
        private System.Windows.Forms.CheckBox cbxApplyMinToZeroTolerance;
        private CrystalReports.CachedAuditReclassReport2 cachedAuditReclassReport21;


	}
}
