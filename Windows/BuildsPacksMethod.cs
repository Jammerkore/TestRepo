using System;
using System.Drawing;
using System.Collections;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Globalization;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Data.SqlTypes;
using System.IO;
using System.Configuration;

using Infragistics.Win.UltraWinGrid;
using Infragistics.Win;
using Infragistics.Win.UltraWinListBar;
using Infragistics.Win.UltraWinTree;
using Infragistics.Win.UltraWinMaskedEdit;

using MIDRetail.Business;
using MIDRetail.Business.Allocation;
using MIDRetail.Common;
using MIDRetail.Data;
using MIDRetail.DataCommon;
using MIDRetail.Windows.Controls;

namespace MIDRetail.Windows
{
    /// <summary>
    /// Summary description for BuildPacksMethod.
    /// </summary>
    public class frmBuildPacksMethod : SizeMethodsFormBase
    {
        private TabControl tabRuleMethod;
        private TabPage tabMethod;
        private TabControl tabBuildPacks;
        private TabPage tabCriteria;
        private TabPage tabConstraints;
        private TabPage tabEvaluation;
        private GroupBox grpConstraints;
        private Label lblPercentToReservePacks;
        private Label lblPercentToReserveBulk;
        private Label lblPercentToReserve;
        private TextBox txtReserveBulk;
        private TextBox txtReservePacks;
        private TextBox txtReserve;
        private GroupBox grpPackErrorOptions;
        private TextBox txtAvgPackDeviationTolerance;
        private Label lblAvgPackDeviationTolerance;
        private TextBox txtMaxPackAllocationNeedTolerance;
        private Label lblMaxPackAllocationNeedTolerance;
        private Button btnApply;
        private Label lblEvaluationPackOptions;
        private GroupBox grpCriteria;
        private TextBox txtVendorPackOrderMin; // TT#787 Vendor Min Order applies only to packs
        private Label lblVendorPackOrderMin; // TT#787 Vendor Min Order applies only to packs
        private Label lblVendor;
        private TextBox txtSizeMultiple;
        private Label lblSizeMultiple;
        private Label lblCandidatePacks;
        private UltraGrid grdCandidatePacks;
        private Panel panel1;
        private RadioButton rdoReserveUnits;
        private RadioButton rdoReservePercent;
        private Panel panel2;
        private RadioButton rdoReserveBulkUnits;
        private RadioButton rdoReserveBulkPercent;
        private Panel panel3;
        private RadioButton rdoReservePacksUnits;
        private RadioButton rdoReservePacksPercent;
        private SplitContainer splEvaluation;
        private DataGridView grdResults;
        private DataGridView grdBPSummary;
        private ContextMenuStrip mnuPackCombination;
        private IContainer components;
        private ToolStripMenuItem mnuPackCombinationAdd;
        private ToolStripMenuItem mnuPackCombinationAddPack;
        private ToolStripMenuItem mnuPackCombinationDelete;
        private Button btnGetHeader;
        private DataGridViewTextBoxColumn PropertyNames;
        private MIDRetail.Windows.Controls.MIDComboBoxEnh cmbEvalOptions;
        private DataGridViewTextBoxColumn evalpropertynames;
        private UltraGrid grdSummary;
        private UltraGrid ugWorkflows;
        private TabPage tabProperties;
        private GroupBox gbxVarianceOptions;
        private CheckBox chkDepleteReserve;
        private TextBox txtIncreaseBuyQty;
        private CheckBox chkIncreaseBuyQty;
        private Label lblPercent;
        private TextBox txtBasisHeader;
        private Label lblHeader;
        private GroupBox groupBox1;
        private TabPage tabBPSummary;
        private MIDComboBoxEnh cmbVendor;
		private CheckBox chkBoxRemoveBulk;

        //constructor
        public frmBuildPacksMethod(SessionAddressBlock SAB, ExplorerAddressBlock aEAB) :
            base(SAB, aEAB, eMIDTextCode.frm_BuildPacksMethod, eWorkflowMethodType.Method)
        {
            try
            {
                //
                // Required for Windows Form Designer support
                //
                InitializeComponent();

                //
                // TODO: Add any constructor code after InitializeComponent call
                //
                UserSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AllocationMethodsUserBuildPacks);
                GlobalSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AllocationMethodsGlobalBuildPacks);
            }
            catch (Exception ex)
            {
                string exceptionMessage = ex.Message;
                this.FormLoadError = true;
            }
        }

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmBuildPacksMethod));
            this.grdBPSummary = new System.Windows.Forms.DataGridView();
            this.PropertyNames = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabRuleMethod = new System.Windows.Forms.TabControl();
            this.tabMethod = new System.Windows.Forms.TabPage();
            this.tabBuildPacks = new System.Windows.Forms.TabControl();
            this.tabCriteria = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnGetHeader = new System.Windows.Forms.Button();
            this.grdCandidatePacks = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.mnuPackCombination = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mnuPackCombinationAdd = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuPackCombinationAddPack = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuPackCombinationDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.grpCriteria = new System.Windows.Forms.GroupBox();
            this.cmbVendor = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this.txtBasisHeader = new System.Windows.Forms.TextBox();
            this.lblHeader = new System.Windows.Forms.Label();
            this.txtVendorPackOrderMin = new System.Windows.Forms.TextBox();
            this.lblVendorPackOrderMin = new System.Windows.Forms.Label();
            this.lblVendor = new System.Windows.Forms.Label();
            this.txtSizeMultiple = new System.Windows.Forms.TextBox();
            this.lblSizeMultiple = new System.Windows.Forms.Label();
            this.lblCandidatePacks = new System.Windows.Forms.Label();
            this.tabConstraints = new System.Windows.Forms.TabPage();
            this.gbxVarianceOptions = new System.Windows.Forms.GroupBox();
            this.lblPercent = new System.Windows.Forms.Label();
            this.txtIncreaseBuyQty = new System.Windows.Forms.TextBox();
            this.chkIncreaseBuyQty = new System.Windows.Forms.CheckBox();
            this.chkDepleteReserve = new System.Windows.Forms.CheckBox();
            this.grpPackErrorOptions = new System.Windows.Forms.GroupBox();
            this.txtMaxPackAllocationNeedTolerance = new System.Windows.Forms.TextBox();
            this.lblMaxPackAllocationNeedTolerance = new System.Windows.Forms.Label();
            this.txtAvgPackDeviationTolerance = new System.Windows.Forms.TextBox();
            this.lblAvgPackDeviationTolerance = new System.Windows.Forms.Label();
            this.grpConstraints = new System.Windows.Forms.GroupBox();
            this.chkBoxRemoveBulk = new System.Windows.Forms.CheckBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.rdoReservePacksUnits = new System.Windows.Forms.RadioButton();
            this.rdoReservePacksPercent = new System.Windows.Forms.RadioButton();
            this.panel2 = new System.Windows.Forms.Panel();
            this.rdoReserveBulkUnits = new System.Windows.Forms.RadioButton();
            this.rdoReserveBulkPercent = new System.Windows.Forms.RadioButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.rdoReserveUnits = new System.Windows.Forms.RadioButton();
            this.rdoReservePercent = new System.Windows.Forms.RadioButton();
            this.txtReserveBulk = new System.Windows.Forms.TextBox();
            this.txtReservePacks = new System.Windows.Forms.TextBox();
            this.txtReserve = new System.Windows.Forms.TextBox();
            this.lblPercentToReservePacks = new System.Windows.Forms.Label();
            this.lblPercentToReserveBulk = new System.Windows.Forms.Label();
            this.lblPercentToReserve = new System.Windows.Forms.Label();
            this.tabProperties = new System.Windows.Forms.TabPage();
            this.ugWorkflows = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.tabEvaluation = new System.Windows.Forms.TabPage();
            this.cmbEvalOptions = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this.splEvaluation = new System.Windows.Forms.SplitContainer();
            this.grdResults = new System.Windows.Forms.DataGridView();
            this.evalpropertynames = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.grdSummary = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.lblEvaluationPackOptions = new System.Windows.Forms.Label();
            this.btnApply = new System.Windows.Forms.Button();
            this.tabBPSummary = new System.Windows.Forms.TabPage();
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
            ((System.ComponentModel.ISupportInitialize)(this.grdBPSummary)).BeginInit();
            this.tabRuleMethod.SuspendLayout();
            this.tabMethod.SuspendLayout();
            this.tabBuildPacks.SuspendLayout();
            this.tabCriteria.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdCandidatePacks)).BeginInit();
            this.mnuPackCombination.SuspendLayout();
            this.grpCriteria.SuspendLayout();
            this.tabConstraints.SuspendLayout();
            this.gbxVarianceOptions.SuspendLayout();
            this.grpPackErrorOptions.SuspendLayout();
            this.grpConstraints.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tabProperties.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ugWorkflows)).BeginInit();
            this.tabEvaluation.SuspendLayout();
            this.splEvaluation.Panel1.SuspendLayout();
            this.splEvaluation.Panel2.SuspendLayout();
            this.splEvaluation.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdResults)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdSummary)).BeginInit();
            this.tabBPSummary.SuspendLayout();
            this.SuspendLayout();
            // 
            // cboSizeGroup
            // 
            this.cboSizeGroup.Size = new System.Drawing.Size(169, 21);
            this.cboSizeGroup.TabIndex = 2;
            this.cboSizeGroup.DropDown += new System.EventHandler(this.cboSizeGroup_DropDown);
            this.cboSizeGroup.DropDownClosed += new System.EventHandler(this.cboSizeGroup_DropDownClosed);
            this.cboSizeGroup.Click += new System.EventHandler(this.cboSizeGroup_Click);
            // 
            // lblStoreAttribute
            // 
            this.lblStoreAttribute.Visible = false;
            // 
            // cboStoreAttribute
            // 
            this.cboStoreAttribute.Enabled = false;
            this.cboStoreAttribute.Visible = false;
            // 
            // cboFilter
            // 
            this.cboFilter.Enabled = false;
            // 
            // lblFilter
            // 
            this.lblFilter.Visible = false;
            // 
            // cboSizeCurve
            // 
            this.cboSizeCurve.Location = new System.Drawing.Point(48, 20);
            this.cboSizeCurve.Size = new System.Drawing.Size(169, 21);
            this.cboSizeCurve.TabIndex = 3;
            this.cboSizeCurve.DropDown += new System.EventHandler(this.cboSizeCurve_DropDown);
            this.cboSizeCurve.DropDownClosed += new System.EventHandler(this.cboSizeCurve_DropDownClosed);
            this.cboSizeCurve.Click += new System.EventHandler(this.cboSizeCurve_Click);
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
            this.gbSizeCurve.Location = new System.Drawing.Point(242, 92);
            this.gbSizeCurve.Size = new System.Drawing.Size(225, 56);
            // 
            // gbSizeConstraints
            // 
            this.gbSizeConstraints.Visible = false;
            // 
            // picBoxCurve
            // 
            this.picBoxCurve.Location = new System.Drawing.Point(19, 20);
            // 
            // gbSizeGroup
            // 
            this.gbSizeGroup.Location = new System.Drawing.Point(8, 92);
            this.gbSizeGroup.Size = new System.Drawing.Size(225, 56);
            // 
            // gbSizeAlternate
            // 
            this.gbSizeAlternate.Visible = false;
            // 
            // gbxNormalizeSizeCurves
            // 
            this.gbxNormalizeSizeCurves.Visible = false;
            // 
            // cbxUseDefaultcurve
            // 
            this.cbxUseDefaultcurve.Location = new System.Drawing.Point(105, 61);
            // 
            // cbxApplyRulesOnly
            // 
            this.cbxApplyRulesOnly.AutoCheck = false;
            this.cbxApplyRulesOnly.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.cbxApplyRulesOnly.CausesValidation = false;
            this.cbxApplyRulesOnly.Enabled = false;
            this.cbxApplyRulesOnly.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.cbxApplyRulesOnly.Location = new System.Drawing.Point(227, 140);
            this.cbxApplyRulesOnly.Visible = false;
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(656, 556);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(575, 556);
            // 
            // btnProcess
            // 
            this.btnProcess.Location = new System.Drawing.Point(13, 556);
            // 
            // radGlobal
            // 
            this.radGlobal.Checked = true;
            this.radGlobal.TabStop = true;
            // 
            // utmMain
            // 
            this.utmMain.MenuSettings.ForceSerialization = true;
            this.utmMain.ToolbarSettings.ForceSerialization = true;
            // 
            // grdBPSummary
            // 
            this.grdBPSummary.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdBPSummary.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdBPSummary.Location = new System.Drawing.Point(3, 3);
            this.grdBPSummary.Name = "grdBPSummary";
            this.grdBPSummary.ReadOnly = true;
            this.grdBPSummary.RowHeadersVisible = false;
            this.grdBPSummary.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.grdBPSummary.Size = new System.Drawing.Size(674, 432);
            this.grdBPSummary.TabIndex = 31;
            // 
            // PropertyNames
            // 
            this.PropertyNames.HeaderText = "Property Names";
            this.PropertyNames.Name = "PropertyNames";
            this.PropertyNames.ReadOnly = true;
            this.PropertyNames.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // tabRuleMethod
            // 
            this.tabRuleMethod.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabRuleMethod.Controls.Add(this.tabMethod);
            this.tabRuleMethod.Controls.Add(this.tabProperties);
            this.tabRuleMethod.Location = new System.Drawing.Point(15, 41);
            this.tabRuleMethod.Name = "tabRuleMethod";
            this.tabRuleMethod.SelectedIndex = 0;
            this.tabRuleMethod.Size = new System.Drawing.Size(718, 496);
            this.tabRuleMethod.TabIndex = 18;
            // 
            // tabMethod
            // 
            this.tabMethod.BackColor = System.Drawing.SystemColors.Control;
            this.tabMethod.Controls.Add(this.tabBuildPacks);
            this.tabMethod.Location = new System.Drawing.Point(4, 22);
            this.tabMethod.Name = "tabMethod";
            this.tabMethod.Padding = new System.Windows.Forms.Padding(3);
            this.tabMethod.Size = new System.Drawing.Size(710, 470);
            this.tabMethod.TabIndex = 0;
            this.tabMethod.Text = "Method";
            // 
            // tabBuildPacks
            // 
            this.tabBuildPacks.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabBuildPacks.Controls.Add(this.tabCriteria);
            this.tabBuildPacks.Controls.Add(this.tabConstraints);
            this.tabBuildPacks.Location = new System.Drawing.Point(3, 3);
            this.tabBuildPacks.Name = "tabBuildPacks";
            this.tabBuildPacks.SelectedIndex = 0;
            this.tabBuildPacks.Size = new System.Drawing.Size(704, 464);
            this.tabBuildPacks.TabIndex = 0;
            // 
            // tabCriteria
            // 
            this.tabCriteria.BackColor = System.Drawing.Color.Transparent;
            this.tabCriteria.Controls.Add(this.groupBox1);
            this.tabCriteria.Controls.Add(this.grdCandidatePacks);
            this.tabCriteria.Controls.Add(this.gbSizeGroup);
            this.tabCriteria.Controls.Add(this.gbSizeCurve);
            this.tabCriteria.Controls.Add(this.grpCriteria);
            this.tabCriteria.Controls.Add(this.lblCandidatePacks);
            this.tabCriteria.Location = new System.Drawing.Point(4, 22);
            this.tabCriteria.Name = "tabCriteria";
            this.tabCriteria.Padding = new System.Windows.Forms.Padding(3);
            this.tabCriteria.Size = new System.Drawing.Size(696, 438);
            this.tabCriteria.TabIndex = 0;
            this.tabCriteria.Text = "Criteria";
            this.tabCriteria.UseVisualStyleBackColor = true;
            this.tabCriteria.Controls.SetChildIndex(this.lblCandidatePacks, 0);
            this.tabCriteria.Controls.SetChildIndex(this.grpCriteria, 0);
            this.tabCriteria.Controls.SetChildIndex(this.gbSizeCurve, 0);
            this.tabCriteria.Controls.SetChildIndex(this.gbSizeGroup, 0);
            this.tabCriteria.Controls.SetChildIndex(this.grdCandidatePacks, 0);
            this.tabCriteria.Controls.SetChildIndex(this.groupBox1, 0);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnGetHeader);
            this.groupBox1.Location = new System.Drawing.Point(478, 92);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(196, 56);
            this.groupBox1.TabIndex = 56;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Header Sizes";
            // 
            // btnGetHeader
            // 
            this.btnGetHeader.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnGetHeader.Location = new System.Drawing.Point(35, 20);
            this.btnGetHeader.Name = "btnGetHeader";
            this.btnGetHeader.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.btnGetHeader.Size = new System.Drawing.Size(125, 23);
            this.btnGetHeader.TabIndex = 29;
            this.btnGetHeader.Text = "Get Header Sizes";
            this.btnGetHeader.Click += new System.EventHandler(this.btnGetHeader_Click);
            // 
            // grdCandidatePacks
            // 
            this.grdCandidatePacks.AllowDrop = true;
            this.grdCandidatePacks.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grdCandidatePacks.ContextMenuStrip = this.mnuPackCombination;
            appearance7.BackColor = System.Drawing.SystemColors.ControlDark;
            appearance7.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance7.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            this.grdCandidatePacks.DisplayLayout.Appearance = appearance7;
            this.grdCandidatePacks.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ExtendLastColumn;
            this.grdCandidatePacks.DisplayLayout.InterBandSpacing = 10;
            appearance8.BackColor = System.Drawing.Color.Transparent;
            this.grdCandidatePacks.DisplayLayout.Override.CardAreaAppearance = appearance8;
            appearance9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance9.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance9.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance9.ForeColor = System.Drawing.Color.Black;
            appearance9.TextHAlignAsString = "Left";
            appearance9.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            this.grdCandidatePacks.DisplayLayout.Override.HeaderAppearance = appearance9;
            this.grdCandidatePacks.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance10.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.grdCandidatePacks.DisplayLayout.Override.RowAppearance = appearance10;
            appearance11.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance11.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance11.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            this.grdCandidatePacks.DisplayLayout.Override.RowSelectorAppearance = appearance11;
            this.grdCandidatePacks.DisplayLayout.Override.RowSelectorWidth = 12;
            this.grdCandidatePacks.DisplayLayout.Override.RowSpacingBefore = 2;
            appearance12.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance12.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance12.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance12.ForeColor = System.Drawing.Color.Black;
            this.grdCandidatePacks.DisplayLayout.Override.SelectedRowAppearance = appearance12;
            this.grdCandidatePacks.DisplayLayout.RowConnectorColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.grdCandidatePacks.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.Solid;
            this.grdCandidatePacks.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.grdCandidatePacks.Location = new System.Drawing.Point(8, 188);
            this.grdCandidatePacks.Name = "grdCandidatePacks";
            this.grdCandidatePacks.Size = new System.Drawing.Size(667, 244);
            this.grdCandidatePacks.TabIndex = 5;
            this.grdCandidatePacks.UseOsThemes = Infragistics.Win.DefaultableBoolean.True;
            // 
            // mnuPackCombination
            // 
            this.mnuPackCombination.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuPackCombinationAdd,
            this.mnuPackCombinationAddPack,
            this.mnuPackCombinationDelete});
            this.mnuPackCombination.Name = "mnuPackCombination";
            this.mnuPackCombination.Size = new System.Drawing.Size(198, 70);
            this.mnuPackCombination.Opening += new System.ComponentModel.CancelEventHandler(this.mnuPackCombination_Opening);
            // 
            // mnuPackCombinationAdd
            // 
            this.mnuPackCombinationAdd.Name = "mnuPackCombinationAdd";
            this.mnuPackCombinationAdd.Size = new System.Drawing.Size(197, 22);
            this.mnuPackCombinationAdd.Text = "&Add Pack Combination";
            this.mnuPackCombinationAdd.Click += new System.EventHandler(this.mnuPackCombinationAdd_Click);
            // 
            // mnuPackCombinationAddPack
            // 
            this.mnuPackCombinationAddPack.Enabled = false;
            this.mnuPackCombinationAddPack.Name = "mnuPackCombinationAddPack";
            this.mnuPackCombinationAddPack.Size = new System.Drawing.Size(197, 22);
            this.mnuPackCombinationAddPack.Text = "Add &Pack";
            this.mnuPackCombinationAddPack.Click += new System.EventHandler(this.mnuPackCombinationAddPack_Click);
            // 
            // mnuPackCombinationDelete
            // 
            this.mnuPackCombinationDelete.Enabled = false;
            this.mnuPackCombinationDelete.Name = "mnuPackCombinationDelete";
            this.mnuPackCombinationDelete.Size = new System.Drawing.Size(197, 22);
            this.mnuPackCombinationDelete.Text = "&Delete";
            this.mnuPackCombinationDelete.Click += new System.EventHandler(this.mnuPackCombinationDelete_Click);
            // 
            // grpCriteria
            // 
            this.grpCriteria.Controls.Add(this.cmbVendor);
            this.grpCriteria.Controls.Add(this.txtBasisHeader);
            this.grpCriteria.Controls.Add(this.lblHeader);
            this.grpCriteria.Controls.Add(this.txtVendorPackOrderMin);
            this.grpCriteria.Controls.Add(this.lblVendorPackOrderMin);
            this.grpCriteria.Controls.Add(this.lblVendor);
            this.grpCriteria.Controls.Add(this.txtSizeMultiple);
            this.grpCriteria.Controls.Add(this.lblSizeMultiple);
            this.grpCriteria.Location = new System.Drawing.Point(8, 6);
            this.grpCriteria.Name = "grpCriteria";
            this.grpCriteria.Size = new System.Drawing.Size(666, 84);
            this.grpCriteria.TabIndex = 31;
            this.grpCriteria.TabStop = false;
            this.grpCriteria.Text = "Criteria";
            // 
            // cmbVendor
            // 
            this.cmbVendor.AutoAdjust = true;
            this.cmbVendor.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmbVendor.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbVendor.DataSource = null;
            this.cmbVendor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbVendor.DropDownWidth = 314;
            this.cmbVendor.FormattingEnabled = false;
            this.cmbVendor.IgnoreFocusLost = false;
            this.cmbVendor.ItemHeight = 13;
            this.cmbVendor.Location = new System.Drawing.Point(66, 17);
            this.cmbVendor.Margin = new System.Windows.Forms.Padding(0);
            this.cmbVendor.MaxDropDownItems = 25;
            this.cmbVendor.Name = "cmbVendor";
            this.cmbVendor.SetToolTip = "";
            this.cmbVendor.Size = new System.Drawing.Size(314, 21);
            this.cmbVendor.TabIndex = 21;
            this.cmbVendor.Tag = null;
            this.cmbVendor.MIDComboBoxPropertiesChangedEvent += new MIDRetail.Windows.Controls.MIDComboBoxPropertiesChangedEventHandler(this.cmbVendor_MIDComboBoxPropertiesChangedEvent);
            this.cmbVendor.SelectionChangeCommitted += new System.EventHandler(this.cmbVendor_SelectionChangeCommitted);
            // 
            // txtBasisHeader
            // 
            this.txtBasisHeader.AllowDrop = true;
            this.txtBasisHeader.Location = new System.Drawing.Point(350, 53);
            this.txtBasisHeader.Name = "txtBasisHeader";
            this.txtBasisHeader.Size = new System.Drawing.Size(29, 20);
            this.txtBasisHeader.TabIndex = 0;
            this.txtBasisHeader.Visible = false;
            // 
            // lblHeader
            // 
            this.lblHeader.Location = new System.Drawing.Point(231, 52);
            this.lblHeader.Name = "lblHeader";
            this.lblHeader.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblHeader.Size = new System.Drawing.Size(116, 15);
            this.lblHeader.TabIndex = 20;
            this.lblHeader.Text = "Base Sizes on Header:";
            this.lblHeader.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.lblHeader.Visible = false;
            // 
            // txtVendorPackOrderMin
            // 
            this.txtVendorPackOrderMin.Enabled = false;
            this.txtVendorPackOrderMin.Location = new System.Drawing.Point(546, 17);
            this.txtVendorPackOrderMin.Name = "txtVendorPackOrderMin";
            this.txtVendorPackOrderMin.ReadOnly = true;
            this.txtVendorPackOrderMin.Size = new System.Drawing.Size(100, 20);
            this.txtVendorPackOrderMin.TabIndex = 19;
            // 
            // lblVendorPackOrderMin
            // 
            this.lblVendorPackOrderMin.AutoSize = true;
            this.lblVendorPackOrderMin.Location = new System.Drawing.Point(395, 21);
            this.lblVendorPackOrderMin.Name = "lblVendorPackOrderMin";
            this.lblVendorPackOrderMin.Size = new System.Drawing.Size(145, 13);
            this.lblVendorPackOrderMin.TabIndex = 18;
            this.lblVendorPackOrderMin.Text = "Vendor Pack Order Minimum:";
            this.lblVendorPackOrderMin.Click += new System.EventHandler(this.lblVendorPackOrderMin_Click);
            // 
            // lblVendor
            // 
            this.lblVendor.AutoSize = true;
            this.lblVendor.Location = new System.Drawing.Point(14, 21);
            this.lblVendor.Name = "lblVendor";
            this.lblVendor.Size = new System.Drawing.Size(44, 13);
            this.lblVendor.TabIndex = 17;
            this.lblVendor.Text = "Vendor:";
            // 
            // txtSizeMultiple
            // 
            this.txtSizeMultiple.Enabled = false;
            this.txtSizeMultiple.Location = new System.Drawing.Point(546, 50);
            this.txtSizeMultiple.Name = "txtSizeMultiple";
            this.txtSizeMultiple.ReadOnly = true;
            this.txtSizeMultiple.Size = new System.Drawing.Size(100, 20);
            this.txtSizeMultiple.TabIndex = 19;
            // 
            // lblSizeMultiple
            // 
            this.lblSizeMultiple.AutoSize = true;
            this.lblSizeMultiple.Location = new System.Drawing.Point(395, 54);
            this.lblSizeMultiple.Name = "lblSizeMultiple";
            this.lblSizeMultiple.Size = new System.Drawing.Size(69, 13);
            this.lblSizeMultiple.TabIndex = 18;
            this.lblSizeMultiple.Text = "Size Multiple:";
            // 
            // lblCandidatePacks
            // 
            this.lblCandidatePacks.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblCandidatePacks.BackColor = System.Drawing.SystemColors.ControlDark;
            this.lblCandidatePacks.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCandidatePacks.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.lblCandidatePacks.Location = new System.Drawing.Point(8, 156);
            this.lblCandidatePacks.Name = "lblCandidatePacks";
            this.lblCandidatePacks.Size = new System.Drawing.Size(667, 27);
            this.lblCandidatePacks.TabIndex = 34;
            this.lblCandidatePacks.Text = "Pack Combinations";
            this.lblCandidatePacks.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tabConstraints
            // 
            this.tabConstraints.BackColor = System.Drawing.Color.Transparent;
            this.tabConstraints.Controls.Add(this.gbxVarianceOptions);
            this.tabConstraints.Controls.Add(this.grpPackErrorOptions);
            this.tabConstraints.Controls.Add(this.grpConstraints);
            this.tabConstraints.Location = new System.Drawing.Point(4, 22);
            this.tabConstraints.Name = "tabConstraints";
            this.tabConstraints.Padding = new System.Windows.Forms.Padding(3);
            this.tabConstraints.Size = new System.Drawing.Size(696, 438);
            this.tabConstraints.TabIndex = 1;
            this.tabConstraints.Text = "Constraints";
            this.tabConstraints.UseVisualStyleBackColor = true;
            // 
            // gbxVarianceOptions
            // 
            this.gbxVarianceOptions.Controls.Add(this.lblPercent);
            this.gbxVarianceOptions.Controls.Add(this.txtIncreaseBuyQty);
            this.gbxVarianceOptions.Controls.Add(this.chkIncreaseBuyQty);
            this.gbxVarianceOptions.Controls.Add(this.chkDepleteReserve);
            this.gbxVarianceOptions.Location = new System.Drawing.Point(8, 205);
            this.gbxVarianceOptions.Name = "gbxVarianceOptions";
            this.gbxVarianceOptions.Size = new System.Drawing.Size(665, 88);
            this.gbxVarianceOptions.TabIndex = 2;
            this.gbxVarianceOptions.TabStop = false;
            this.gbxVarianceOptions.Text = "Variance Options:";
            // 
            // lblPercent
            // 
            this.lblPercent.AutoSize = true;
            this.lblPercent.Location = new System.Drawing.Point(595, 41);
            this.lblPercent.Name = "lblPercent";
            this.lblPercent.Size = new System.Drawing.Size(18, 13);
            this.lblPercent.TabIndex = 3;
            this.lblPercent.Text = "%:";
            // 
            // txtIncreaseBuyQty
            // 
            this.txtIncreaseBuyQty.Location = new System.Drawing.Point(489, 37);
            this.txtIncreaseBuyQty.Name = "txtIncreaseBuyQty";
            this.txtIncreaseBuyQty.Size = new System.Drawing.Size(100, 20);
            this.txtIncreaseBuyQty.TabIndex = 2;
            // 
            // chkIncreaseBuyQty
            // 
            this.chkIncreaseBuyQty.AutoSize = true;
            this.chkIncreaseBuyQty.Checked = true;
            this.chkIncreaseBuyQty.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkIncreaseBuyQty.Location = new System.Drawing.Point(349, 37);
            this.chkIncreaseBuyQty.Name = "chkIncreaseBuyQty";
            this.chkIncreaseBuyQty.Size = new System.Drawing.Size(133, 17);
            this.chkIncreaseBuyQty.TabIndex = 1;
            this.chkIncreaseBuyQty.Text = "Increase Buy Quantity:";
            this.chkIncreaseBuyQty.UseVisualStyleBackColor = true;
            this.chkIncreaseBuyQty.CheckedChanged += new System.EventHandler(this.chkIncreaseBuyQty_CheckedChanged);
            // 
            // chkDepleteReserve
            // 
            this.chkDepleteReserve.AutoSize = true;
            this.chkDepleteReserve.Checked = true;
            this.chkDepleteReserve.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkDepleteReserve.Location = new System.Drawing.Point(14, 37);
            this.chkDepleteReserve.Name = "chkDepleteReserve";
            this.chkDepleteReserve.Size = new System.Drawing.Size(109, 17);
            this.chkDepleteReserve.TabIndex = 0;
            this.chkDepleteReserve.Text = "Deplete Reserve:";
            this.chkDepleteReserve.UseVisualStyleBackColor = true;
            // 
            // grpPackErrorOptions
            // 
            this.grpPackErrorOptions.Controls.Add(this.txtMaxPackAllocationNeedTolerance);
            this.grpPackErrorOptions.Controls.Add(this.lblMaxPackAllocationNeedTolerance);
            this.grpPackErrorOptions.Controls.Add(this.txtAvgPackDeviationTolerance);
            this.grpPackErrorOptions.Controls.Add(this.lblAvgPackDeviationTolerance);
            this.grpPackErrorOptions.Location = new System.Drawing.Point(8, 118);
            this.grpPackErrorOptions.Name = "grpPackErrorOptions";
            this.grpPackErrorOptions.Size = new System.Drawing.Size(666, 73);
            this.grpPackErrorOptions.TabIndex = 1;
            this.grpPackErrorOptions.TabStop = false;
            this.grpPackErrorOptions.Text = "Pack Error Options";
            // 
            // txtMaxPackAllocationNeedTolerance
            // 
            this.txtMaxPackAllocationNeedTolerance.Location = new System.Drawing.Point(428, 30);
            this.txtMaxPackAllocationNeedTolerance.Name = "txtMaxPackAllocationNeedTolerance";
            this.txtMaxPackAllocationNeedTolerance.Size = new System.Drawing.Size(100, 20);
            this.txtMaxPackAllocationNeedTolerance.TabIndex = 4;
            this.txtMaxPackAllocationNeedTolerance.Text = "0";
            // 
            // lblMaxPackAllocationNeedTolerance
            // 
            this.lblMaxPackAllocationNeedTolerance.Location = new System.Drawing.Point(342, 34);
            this.lblMaxPackAllocationNeedTolerance.Name = "lblMaxPackAllocationNeedTolerance";
            this.lblMaxPackAllocationNeedTolerance.Size = new System.Drawing.Size(80, 13);
            this.lblMaxPackAllocationNeedTolerance.TabIndex = 6;
            this.lblMaxPackAllocationNeedTolerance.Text = "Ship Variance";
            // 
            // txtAvgPackDeviationTolerance
            // 
            this.txtAvgPackDeviationTolerance.Location = new System.Drawing.Point(144, 30);
            this.txtAvgPackDeviationTolerance.Name = "txtAvgPackDeviationTolerance";
            this.txtAvgPackDeviationTolerance.Size = new System.Drawing.Size(100, 20);
            this.txtAvgPackDeviationTolerance.TabIndex = 3;
            // 
            // lblAvgPackDeviationTolerance
            // 
            this.lblAvgPackDeviationTolerance.Location = new System.Drawing.Point(8, 24);
            this.lblAvgPackDeviationTolerance.Name = "lblAvgPackDeviationTolerance";
            this.lblAvgPackDeviationTolerance.Size = new System.Drawing.Size(117, 34);
            this.lblAvgPackDeviationTolerance.TabIndex = 4;
            this.lblAvgPackDeviationTolerance.Text = "Average Pack Deviation Tolerance";
            // 
            // grpConstraints
            // 
            this.grpConstraints.Controls.Add(this.chkBoxRemoveBulk);
            this.grpConstraints.Controls.Add(this.panel3);
            this.grpConstraints.Controls.Add(this.panel2);
            this.grpConstraints.Controls.Add(this.panel1);
            this.grpConstraints.Controls.Add(this.txtReserveBulk);
            this.grpConstraints.Controls.Add(this.txtReservePacks);
            this.grpConstraints.Controls.Add(this.txtReserve);
            this.grpConstraints.Controls.Add(this.lblPercentToReservePacks);
            this.grpConstraints.Controls.Add(this.lblPercentToReserveBulk);
            this.grpConstraints.Controls.Add(this.lblPercentToReserve);
            this.grpConstraints.Location = new System.Drawing.Point(8, 8);
            this.grpConstraints.Name = "grpConstraints";
            this.grpConstraints.Size = new System.Drawing.Size(666, 106);
            this.grpConstraints.TabIndex = 0;
            this.grpConstraints.TabStop = false;
            this.grpConstraints.Text = "Constraints";
            // 
            // chkBoxRemoveBulk
            // 
            this.chkBoxRemoveBulk.AutoSize = true;
            this.chkBoxRemoveBulk.Location = new System.Drawing.Point(383, 49);
            this.chkBoxRemoveBulk.Name = "chkBoxRemoveBulk";
            this.chkBoxRemoveBulk.Size = new System.Drawing.Size(157, 17);
            this.chkBoxRemoveBulk.TabIndex = 12;
            this.chkBoxRemoveBulk.Text = "Remove Bulk From Header:";
            this.chkBoxRemoveBulk.UseVisualStyleBackColor = true;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.rdoReservePacksUnits);
            this.panel3.Controls.Add(this.rdoReservePacksPercent);
            this.panel3.Location = new System.Drawing.Point(259, 74);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(105, 20);
            this.panel3.TabIndex = 11;
            // 
            // rdoReservePacksUnits
            // 
            this.rdoReservePacksUnits.AutoSize = true;
            this.rdoReservePacksUnits.Location = new System.Drawing.Point(52, 2);
            this.rdoReservePacksUnits.Name = "rdoReservePacksUnits";
            this.rdoReservePacksUnits.Size = new System.Drawing.Size(49, 17);
            this.rdoReservePacksUnits.TabIndex = 9;
            this.rdoReservePacksUnits.TabStop = true;
            this.rdoReservePacksUnits.Text = "Units";
            this.rdoReservePacksUnits.UseVisualStyleBackColor = true;
            this.rdoReservePacksUnits.CheckedChanged += new System.EventHandler(this.rdoReservePacksUnits_CheckedChanged);
            // 
            // rdoReservePacksPercent
            // 
            this.rdoReservePacksPercent.AutoSize = true;
            this.rdoReservePacksPercent.Checked = true;
            this.rdoReservePacksPercent.Location = new System.Drawing.Point(5, 2);
            this.rdoReservePacksPercent.Name = "rdoReservePacksPercent";
            this.rdoReservePacksPercent.Size = new System.Drawing.Size(33, 17);
            this.rdoReservePacksPercent.TabIndex = 8;
            this.rdoReservePacksPercent.TabStop = true;
            this.rdoReservePacksPercent.Text = "%";
            this.rdoReservePacksPercent.UseVisualStyleBackColor = true;
            this.rdoReservePacksPercent.CheckedChanged += new System.EventHandler(this.rdoReservePacksPercent_CheckedChanged);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.rdoReserveBulkUnits);
            this.panel2.Controls.Add(this.rdoReserveBulkPercent);
            this.panel2.Location = new System.Drawing.Point(259, 46);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(105, 20);
            this.panel2.TabIndex = 10;
            // 
            // rdoReserveBulkUnits
            // 
            this.rdoReserveBulkUnits.AutoSize = true;
            this.rdoReserveBulkUnits.Location = new System.Drawing.Point(52, 2);
            this.rdoReserveBulkUnits.Name = "rdoReserveBulkUnits";
            this.rdoReserveBulkUnits.Size = new System.Drawing.Size(49, 17);
            this.rdoReserveBulkUnits.TabIndex = 9;
            this.rdoReserveBulkUnits.TabStop = true;
            this.rdoReserveBulkUnits.Text = "Units";
            this.rdoReserveBulkUnits.UseVisualStyleBackColor = true;
            this.rdoReserveBulkUnits.CheckedChanged += new System.EventHandler(this.rdoReserveBulkUnits_CheckedChanged);
            // 
            // rdoReserveBulkPercent
            // 
            this.rdoReserveBulkPercent.AutoSize = true;
            this.rdoReserveBulkPercent.Checked = true;
            this.rdoReserveBulkPercent.Location = new System.Drawing.Point(5, 2);
            this.rdoReserveBulkPercent.Name = "rdoReserveBulkPercent";
            this.rdoReserveBulkPercent.Size = new System.Drawing.Size(33, 17);
            this.rdoReserveBulkPercent.TabIndex = 8;
            this.rdoReserveBulkPercent.TabStop = true;
            this.rdoReserveBulkPercent.Text = "%";
            this.rdoReserveBulkPercent.UseVisualStyleBackColor = true;
            this.rdoReserveBulkPercent.CheckedChanged += new System.EventHandler(this.rdoReserveBulkPercent_CheckedChanged);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.rdoReserveUnits);
            this.panel1.Controls.Add(this.rdoReservePercent);
            this.panel1.Location = new System.Drawing.Point(259, 18);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(105, 20);
            this.panel1.TabIndex = 8;
            // 
            // rdoReserveUnits
            // 
            this.rdoReserveUnits.AutoSize = true;
            this.rdoReserveUnits.Location = new System.Drawing.Point(52, 2);
            this.rdoReserveUnits.Name = "rdoReserveUnits";
            this.rdoReserveUnits.Size = new System.Drawing.Size(49, 17);
            this.rdoReserveUnits.TabIndex = 9;
            this.rdoReserveUnits.TabStop = true;
            this.rdoReserveUnits.Text = "Units";
            this.rdoReserveUnits.UseVisualStyleBackColor = true;
            this.rdoReserveUnits.CheckedChanged += new System.EventHandler(this.rdoReserveUnits_CheckedChanged);
            // 
            // rdoReservePercent
            // 
            this.rdoReservePercent.AutoSize = true;
            this.rdoReservePercent.Checked = true;
            this.rdoReservePercent.Location = new System.Drawing.Point(5, 2);
            this.rdoReservePercent.Name = "rdoReservePercent";
            this.rdoReservePercent.Size = new System.Drawing.Size(33, 17);
            this.rdoReservePercent.TabIndex = 8;
            this.rdoReservePercent.TabStop = true;
            this.rdoReservePercent.Text = "%";
            this.rdoReservePercent.UseVisualStyleBackColor = true;
            this.rdoReservePercent.CheckedChanged += new System.EventHandler(this.rdoReservePercent_CheckedChanged);
            // 
            // txtReserveBulk
            // 
            this.txtReserveBulk.Location = new System.Drawing.Point(144, 46);
            this.txtReserveBulk.Name = "txtReserveBulk";
            this.txtReserveBulk.Size = new System.Drawing.Size(100, 20);
            this.txtReserveBulk.TabIndex = 1;
            this.txtReserveBulk.Text = "0";
            // 
            // txtReservePacks
            // 
            this.txtReservePacks.Location = new System.Drawing.Point(144, 74);
            this.txtReservePacks.Name = "txtReservePacks";
            this.txtReservePacks.Size = new System.Drawing.Size(100, 20);
            this.txtReservePacks.TabIndex = 2;
            this.txtReservePacks.Text = "0";
            // 
            // txtReserve
            // 
            this.txtReserve.Location = new System.Drawing.Point(144, 18);
            this.txtReserve.Name = "txtReserve";
            this.txtReserve.Size = new System.Drawing.Size(100, 20);
            this.txtReserve.TabIndex = 0;
            this.txtReserve.Text = "0";
            // 
            // lblPercentToReservePacks
            // 
            this.lblPercentToReservePacks.AutoSize = true;
            this.lblPercentToReservePacks.Location = new System.Drawing.Point(8, 77);
            this.lblPercentToReservePacks.Name = "lblPercentToReservePacks";
            this.lblPercentToReservePacks.Size = new System.Drawing.Size(94, 13);
            this.lblPercentToReservePacks.TabIndex = 2;
            this.lblPercentToReservePacks.Text = "Reserve as Packs";
            // 
            // lblPercentToReserveBulk
            // 
            this.lblPercentToReserveBulk.AutoSize = true;
            this.lblPercentToReserveBulk.Location = new System.Drawing.Point(8, 49);
            this.lblPercentToReserveBulk.Name = "lblPercentToReserveBulk";
            this.lblPercentToReserveBulk.Size = new System.Drawing.Size(85, 13);
            this.lblPercentToReserveBulk.TabIndex = 1;
            this.lblPercentToReserveBulk.Text = "Reserve as Bulk";
            // 
            // lblPercentToReserve
            // 
            this.lblPercentToReserve.AutoSize = true;
            this.lblPercentToReserve.Location = new System.Drawing.Point(8, 21);
            this.lblPercentToReserve.Name = "lblPercentToReserve";
            this.lblPercentToReserve.Size = new System.Drawing.Size(47, 13);
            this.lblPercentToReserve.TabIndex = 0;
            this.lblPercentToReserve.Text = "Reserve";
            // 
            // tabProperties
            // 
            this.tabProperties.BackColor = System.Drawing.SystemColors.Control;
            this.tabProperties.Controls.Add(this.ugWorkflows);
            this.tabProperties.Location = new System.Drawing.Point(4, 22);
            this.tabProperties.Name = "tabProperties";
            this.tabProperties.Padding = new System.Windows.Forms.Padding(3);
            this.tabProperties.Size = new System.Drawing.Size(710, 470);
            this.tabProperties.TabIndex = 1;
            this.tabProperties.Text = "Properties";
            this.tabProperties.Visible = false;
            // 
            // ugWorkflows
            // 
            this.ugWorkflows.AllowDrop = true;
            this.ugWorkflows.ContextMenuStrip = this.mnuPackCombination;
            appearance13.BackColor = System.Drawing.SystemColors.ControlDark;
            appearance13.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance13.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            this.ugWorkflows.DisplayLayout.Appearance = appearance13;
            this.ugWorkflows.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns;
            this.ugWorkflows.DisplayLayout.InterBandSpacing = 10;
            appearance14.BackColor = System.Drawing.Color.Transparent;
            this.ugWorkflows.DisplayLayout.Override.CardAreaAppearance = appearance14;
            appearance15.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance15.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance15.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance15.ForeColor = System.Drawing.Color.Black;
            appearance15.TextHAlignAsString = "Left";
            appearance15.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            this.ugWorkflows.DisplayLayout.Override.HeaderAppearance = appearance15;
            this.ugWorkflows.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance16.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.ugWorkflows.DisplayLayout.Override.RowAppearance = appearance16;
            appearance17.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance17.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance17.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            this.ugWorkflows.DisplayLayout.Override.RowSelectorAppearance = appearance17;
            this.ugWorkflows.DisplayLayout.Override.RowSelectorWidth = 12;
            this.ugWorkflows.DisplayLayout.Override.RowSpacingBefore = 2;
            appearance18.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance18.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance18.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance18.ForeColor = System.Drawing.Color.Black;
            this.ugWorkflows.DisplayLayout.Override.SelectedRowAppearance = appearance18;
            this.ugWorkflows.DisplayLayout.RowConnectorColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.ugWorkflows.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.Solid;
            this.ugWorkflows.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.ugWorkflows.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ugWorkflows.Location = new System.Drawing.Point(3, 3);
            this.ugWorkflows.Name = "ugWorkflows";
            this.ugWorkflows.Size = new System.Drawing.Size(703, 464);
            this.ugWorkflows.TabIndex = 34;
            this.ugWorkflows.UseOsThemes = Infragistics.Win.DefaultableBoolean.True;
            // 
            // tabEvaluation
            // 
            this.tabEvaluation.BackColor = System.Drawing.Color.Transparent;
            this.tabEvaluation.Controls.Add(this.cmbEvalOptions);
            this.tabEvaluation.Controls.Add(this.splEvaluation);
            this.tabEvaluation.Controls.Add(this.lblEvaluationPackOptions);
            this.tabEvaluation.Location = new System.Drawing.Point(4, 22);
            this.tabEvaluation.Name = "tabEvaluation";
            this.tabEvaluation.Size = new System.Drawing.Size(680, 438);
            this.tabEvaluation.TabIndex = 2;
            this.tabEvaluation.Text = "Evaluation";
            this.tabEvaluation.UseVisualStyleBackColor = true;
            // 
            // cmbEvalOptions
            // 
            this.cmbEvalOptions.AutoAdjust = true;
            this.cmbEvalOptions.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmbEvalOptions.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbEvalOptions.DataSource = null;
            this.cmbEvalOptions.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbEvalOptions.DropDownWidth = 587;
            this.cmbEvalOptions.FormattingEnabled = true;
            this.cmbEvalOptions.IgnoreFocusLost = false;
            this.cmbEvalOptions.ItemHeight = 13;
            this.cmbEvalOptions.Location = new System.Drawing.Point(87, 8);
            this.cmbEvalOptions.Margin = new System.Windows.Forms.Padding(0);
            this.cmbEvalOptions.MaxDropDownItems = 25;
            this.cmbEvalOptions.Name = "cmbEvalOptions";
            this.cmbEvalOptions.SetToolTip = "";
            this.cmbEvalOptions.Size = new System.Drawing.Size(587, 21);
            this.cmbEvalOptions.TabIndex = 32;
            this.cmbEvalOptions.Tag = null;
            this.cmbEvalOptions.MIDComboBoxPropertiesChangedEvent += new MIDRetail.Windows.Controls.MIDComboBoxPropertiesChangedEventHandler(this.cmbEvalOptions_MIDComboBoxPropertiesChangedEvent);
            this.cmbEvalOptions.SelectionChangeCommitted += new System.EventHandler(this.cmbEvalOptions_SelectionChangeCommitted);
            // 
            // splEvaluation
            // 
            this.splEvaluation.Location = new System.Drawing.Point(8, 35);
            this.splEvaluation.Name = "splEvaluation";
            this.splEvaluation.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splEvaluation.Panel1
            // 
            this.splEvaluation.Panel1.Controls.Add(this.grdResults);
            // 
            // splEvaluation.Panel2
            // 
            this.splEvaluation.Panel2.Controls.Add(this.grdSummary);
            this.splEvaluation.Size = new System.Drawing.Size(666, 396);
            this.splEvaluation.SplitterDistance = 198;
            this.splEvaluation.TabIndex = 31;
            // Begin TT#5095 - JSmith - Evaluation tab does not resize correctly
            this.splEvaluation.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            // End TT#5095 - JSmith - Evaluation tab does not resize correctly
            // 
            // grdResults
            // 
            this.grdResults.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdResults.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.evalpropertynames});
            this.grdResults.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdResults.Location = new System.Drawing.Point(0, 0);
            this.grdResults.Name = "grdResults";
            this.grdResults.ReadOnly = true;
            this.grdResults.RowHeadersVisible = false;
            this.grdResults.Size = new System.Drawing.Size(666, 198);
            this.grdResults.TabIndex = 30;
            // 
            // evalpropertynames
            // 
            this.evalpropertynames.HeaderText = "Property Names";
            this.evalpropertynames.Name = "evalpropertynames";
            this.evalpropertynames.ReadOnly = true;
            this.evalpropertynames.Visible = false;
            // 
            // grdSummary
            // 
            this.grdSummary.AllowDrop = true;
            this.grdSummary.ContextMenuStrip = this.mnuPackCombination;
            appearance19.BackColor = System.Drawing.SystemColors.AppWorkspace;
            appearance19.BackColor2 = System.Drawing.SystemColors.AppWorkspace;
            appearance19.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            this.grdSummary.DisplayLayout.Appearance = appearance19;
            this.grdSummary.DisplayLayout.InterBandSpacing = 10;
            appearance20.BackColor = System.Drawing.Color.Transparent;
            this.grdSummary.DisplayLayout.Override.CardAreaAppearance = appearance20;
            appearance21.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance21.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance21.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance21.ForeColor = System.Drawing.Color.Black;
            appearance21.TextHAlignAsString = "Left";
            appearance21.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            this.grdSummary.DisplayLayout.Override.HeaderAppearance = appearance21;
            this.grdSummary.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance22.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.grdSummary.DisplayLayout.Override.RowAppearance = appearance22;
            appearance23.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance23.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance23.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            this.grdSummary.DisplayLayout.Override.RowSelectorAppearance = appearance23;
            this.grdSummary.DisplayLayout.Override.RowSelectorWidth = 12;
            this.grdSummary.DisplayLayout.Override.RowSpacingBefore = 2;
            appearance24.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance24.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance24.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance24.ForeColor = System.Drawing.Color.Black;
            this.grdSummary.DisplayLayout.Override.SelectedRowAppearance = appearance24;
            this.grdSummary.DisplayLayout.RowConnectorColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.grdSummary.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.Solid;
            this.grdSummary.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.grdSummary.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdSummary.Location = new System.Drawing.Point(0, 0);
            this.grdSummary.Name = "grdSummary";
            this.grdSummary.Size = new System.Drawing.Size(666, 194);
            this.grdSummary.TabIndex = 34;
            this.grdSummary.UseOsThemes = Infragistics.Win.DefaultableBoolean.True;
            // 
            // lblEvaluationPackOptions
            // 
            this.lblEvaluationPackOptions.AutoSize = true;
            this.lblEvaluationPackOptions.Location = new System.Drawing.Point(4, 11);
            this.lblEvaluationPackOptions.Name = "lblEvaluationPackOptions";
            this.lblEvaluationPackOptions.Size = new System.Drawing.Size(74, 13);
            this.lblEvaluationPackOptions.TabIndex = 30;
            this.lblEvaluationPackOptions.Text = "Pack Options:";
            // 
            // btnApply
            // 
            this.btnApply.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnApply.Enabled = false;
            this.btnApply.Location = new System.Drawing.Point(94, 556);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(75, 23);
            this.btnApply.TabIndex = 20;
            this.btnApply.Text = "Apply";
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // tabBPSummary
            // 
            this.tabBPSummary.Controls.Add(this.grdBPSummary);
            this.tabBPSummary.Location = new System.Drawing.Point(4, 22);
            this.tabBPSummary.Name = "tabBPSummary";
            this.tabBPSummary.Padding = new System.Windows.Forms.Padding(3);
            this.tabBPSummary.Size = new System.Drawing.Size(680, 438);
            this.tabBPSummary.TabIndex = 3;
            this.tabBPSummary.Text = "Summary";
            this.tabBPSummary.UseVisualStyleBackColor = true;
            // 
            // frmBuildPacksMethod
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(745, 588);
            this.Controls.Add(this.tabRuleMethod);
            this.Controls.Add(this.btnApply);
            //this.FunctionSecurity = ((MIDRetail.DataCommon.FunctionSecurityProfile)(resources.GetObject("$this.FunctionSecurity")));
            this.Name = "frmBuildPacksMethod";
            //this.StartPosition = System.Windows.Forms.FormStartPosition.WindowsDefaultBounds;
            this.Text = "Build Packs Method";
            this.Load += new System.EventHandler(this.frmBuildPacksMethod_Load);
            this.Disposed += new System.EventHandler(this.frmBuildPackMethod_Dispose);
            this.Controls.SetChildIndex(this.btnApply, 0);
            this.Controls.SetChildIndex(this.ugRules, 0);
            this.Controls.SetChildIndex(this.cboStoreAttribute, 0);
            this.Controls.SetChildIndex(this.cboFilter, 0);
            this.Controls.SetChildIndex(this.gbxNormalizeSizeCurves, 0);
            this.Controls.SetChildIndex(this.lblFilter, 0);
            this.Controls.SetChildIndex(this.btnSave, 0);
            this.Controls.SetChildIndex(this.lblStoreAttribute, 0);
            this.Controls.SetChildIndex(this.cbExpandAll, 0);
            this.Controls.SetChildIndex(this.gbSizeConstraints, 0);
            this.Controls.SetChildIndex(this.gbSizeAlternate, 0);
            this.Controls.SetChildIndex(this.tabRuleMethod, 0);
            this.Controls.SetChildIndex(this.lblName, 0);
            this.Controls.SetChildIndex(this.btnProcess, 0);
            this.Controls.SetChildIndex(this.btnClose, 0);
            this.Controls.SetChildIndex(this.txtName, 0);
            this.Controls.SetChildIndex(this.txtDesc, 0);
            this.Controls.SetChildIndex(this.pnlGlobalUser, 0);
            ((System.ComponentModel.ISupportInitialize)(this.ugRules)).EndInit();
            this.gbGenericSizeCurve.ResumeLayout(false);
            this.gbSizeCurve.ResumeLayout(false);
            this.gbSizeCurve.PerformLayout();
            this.gbSizeConstraints.ResumeLayout(false);
            this.gbSizeConstraints.PerformLayout();
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
            ((System.ComponentModel.ISupportInitialize)(this.grdBPSummary)).EndInit();
            this.tabRuleMethod.ResumeLayout(false);
            this.tabMethod.ResumeLayout(false);
            this.tabBuildPacks.ResumeLayout(false);
            this.tabCriteria.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdCandidatePacks)).EndInit();
            this.mnuPackCombination.ResumeLayout(false);
            this.grpCriteria.ResumeLayout(false);
            this.grpCriteria.PerformLayout();
            this.tabConstraints.ResumeLayout(false);
            this.gbxVarianceOptions.ResumeLayout(false);
            this.gbxVarianceOptions.PerformLayout();
            this.grpPackErrorOptions.ResumeLayout(false);
            this.grpPackErrorOptions.PerformLayout();
            this.grpConstraints.ResumeLayout(false);
            this.grpConstraints.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tabProperties.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ugWorkflows)).EndInit();
            this.tabEvaluation.ResumeLayout(false);
            this.tabEvaluation.PerformLayout();
            this.splEvaluation.Panel1.ResumeLayout(false);
            this.splEvaluation.Panel2.ResumeLayout(false);
            this.splEvaluation.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdResults)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdSummary)).EndInit();
            this.tabBPSummary.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        //member variables
        private int _nodeRID = -1;

        //Pack combination datatable
        private DataTable dtPackCombo;

        //Pack datatable
        private DataTable dtPack;

        //Pack combo and pack dataset
        private DataSet dsPackCombo;

        //initialize the business object
        private MIDRetail.Business.Allocation.BuildPacksMethod _BuildPacksMethod = null;

        //initialize the header list
        SelectedHeaderList selectedHeaderList;

        //list of primary sizes
        //List<string> lstSizes = new List<string>();  // TT#615 Addendum

        //list of SizeUnits
        List<int> lstSizeRIDs = new List<int>();

        //hashtable of size info
        //Hashtable htSizes = new Hashtable();  // TT#615 Addendum
        Dictionary<int, string> htSizes = new Dictionary<int, string>();  // TT#615 Addendum

        //required objects
        AllocationSizeBaseMethod asbm;
        AllocationHeaderProfile ahp;
        Profile prf;

        //list of vendor pack combinations
        List<PackPatternCombo> lstVendorPackCombos;

        #region Common Load and Overrides
        private void Common_Load(eGlobalUserType aGlobalUserType)
        {
            Name = MIDText.GetTextOnly((int)eMethodType.BuildPacks);

            SetText();

            selectedHeaderList = (SelectedHeaderList)SAB.ClientServerSession.GetSelectedHeaderList();

            int prfKey = Include.NoRID;
            if (selectedHeaderList.Count > 0)
            {
                prf = selectedHeaderList[0];

                prfKey = prf.Key;
            }

            ahp = new AllocationHeaderProfile(prfKey);

            //Begin TT#523 - JScott - Duplicate folder when new folder added
            //asbm = new AllocationSizeBaseMethod(SAB, ahp.MethodRID, eMethodType.BuildPacks);
            asbm = new AllocationSizeBaseMethod(SAB, ahp.MethodRID, eMethodType.BuildPacks, eProfileType.MethodBuildPacks);
            //End TT#523 - JScott - Duplicate folder when new folder added

            //if (FunctionSecurity.AllowExecute)
            //{
            //    btnProcess.Enabled = true;
            //}
            //else
            //{
            txtSizeMultiple.ReadOnly = true;
            txtVendorPackOrderMin.ReadOnly = true; // TT#787 Vendor Min Order applies only to packs
            //btnProcess.Enabled = false;
            btnApply.Enabled = false;
            BindSizeGroupComboBox(true, Include.NoRID);
            BindSizeCurveComboBox(true, Include.NoRID);

            // begin TT#615 Addendum Size Group, Size Curve and Size Run Issues
            _selectedSizeGroupRID = _BuildPacksMethod.SizeGroupRID; // TT#615 Size Group, Size Curve and Size Run Issues
            if (_selectedSizeGroupRID != Include.NoRID)
            {
                SizeGroupProfile sgp = new SizeGroupProfile(_selectedSizeGroupRID);
                if (sgp.Key == Include.UndefinedSizeGroupRID)
                {
                    _selectedSizeGroupRID = Include.NoRID;
                }
                else
                {
                    cboSizeGroup.Text = sgp.SizeGroupName;
                }
            }
            _selectedSizeGroupIndex = cboSizeGroup.SelectedIndex;

            _selectedSizeCurveRID = _BuildPacksMethod.SizeCurveGroupRID; // TT#615 Size Group, Size Curve and Size Run Issues
            if (_selectedSizeCurveRID != Include.NoRID)
            {
                SizeCurveGroupProfile scp = new SizeCurveGroupProfile(_selectedSizeCurveRID);
                if (scp.Key == Include.NoRID)
                {
                    _selectedSizeCurveRID = Include.NoRID;
                }
                else
                {
                    cboSizeCurve.Text = scp.SizeCurveGroupName;
                }
            }
            _selectedSizeCurveIndex = cboSizeCurve.SelectedIndex; // TT#615 Size Group, Size Curve and Size Run Issues
            // end TT#615 Addendum Size Group, Size Curve and Size Run Issues

            if (SAB.ClientServerSession.GlobalOptions.PackSizeErrorPercent == Double.MaxValue)
            {
                txtAvgPackDeviationTolerance.Text = "";
            }
            else
            {
                txtAvgPackDeviationTolerance.Text = Convert.ToInt32(SAB.ClientServerSession.GlobalOptions.PackSizeErrorPercent).ToString().Trim();
            }

            if (SAB.ClientServerSession.GlobalOptions.MaxSizeErrorPercent == Double.MaxValue)
            {
                txtMaxPackAllocationNeedTolerance.Text = "0";
            }
            else
            {
                txtMaxPackAllocationNeedTolerance.Text = Convert.ToInt32(SAB.ClientServerSession.GlobalOptions.MaxSizeErrorPercent).ToString().Trim();
            }
            //}

            if (selectedHeaderList != null)
            {
                if (selectedHeaderList.Count > 1)
                {
                    MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_InteractiveMultipeHeadersSelected), 
                        "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }
        // begin change by Jim
        /// <summary>
        /// Renames a Build Packs Method.
        /// </summary>
        /// <param name="aMethodRID">The record ID of the method</param>
        /// <param name="aNewName">The new name of the workflow or method</param>
        override public bool RenameWorkflowMethod(int aMethodRID, string aNewName)
        {
            try
            {
                _BuildPacksMethod = new BuildPacksMethod(SAB, aMethodRID);
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
                _BuildPacksMethod = new BuildPacksMethod(SAB, aMethodRID);
                ProcessAction(eMethodType.BuildPacks, true);
            }
            catch (Exception ex)
            {
                HandleException(ex);
                FormLoadError = true;
            }
        }
        // end change by Jim
        override public void ISave()
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                SaveChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        protected override void Call_btnSave_Click()
        {
            try
            {
                // Begin TT#733 - JSmith - Messaging incorrect
                //if (FieldValidations() == true)
                //{
                //    base.btnSave_Click();
                //}
                base.btnSave_Click();
                // End TT#733
            }
            catch (Exception exception)
            {
                HandleException(exception);
            }
        }

        protected override void SetCommonFields()
        {
            WorkflowMethodName = txtName.Text.Trim();
            WorkflowMethodDescription = txtDesc.Text.Trim();
            GlobalRadioButton = radGlobal;
            UserRadioButton = radUser;
        }

        protected override bool IsGridValid()
        {
            //return base.IsGridValid();

            return true;
        }

        /// <summary>
        /// Use to validate the fields that are specific to this method type
        /// </summary>
        override protected bool ValidateSpecificFields()
        {
            bool isFormValid = true;

            try
            {
                // Begin TT#733 - JSmith - Messaging incorrect
                if (FieldValidations() == false)
                {
                    isFormValid = false;
                }
                // End TT#733

                ErrorMessages.Clear();
                AttachErrors(cboSizeCurve);

                if (!IsGridValid())
                {
                    isFormValid = false;
                }
            }
            catch
            {
                throw;
            }

            return isFormValid;
        }

        // Begin TT#733 - JSmith - Messaging incorrect
        /// <summary>
        /// Use to set the errors to the screen
        /// </summary>

        override protected void HandleErrors()
        {
            try
            {
                if (!WorkflowMethodNameValid)
                {
                    ErrorProvider.SetError(txtName, WorkflowMethodNameMessage);
                }
                else
                {
                    ErrorProvider.SetError(txtName, string.Empty);
                }
                if (!WorkflowMethodDescriptionValid)
                {
                    ErrorProvider.SetError(txtDesc, WorkflowMethodDescriptionMessage);
                }
                else
                {
                    ErrorProvider.SetError(txtDesc, string.Empty);
                }
                if (!UserGlobalValid)
                {
                    ErrorProvider.SetError(pnlGlobalUser, UserGlobalMessage);
                }
                else
                {
                    ErrorProvider.SetError(pnlGlobalUser, string.Empty);
                }
            }
            catch (Exception err)
            {
                HandleException(err, "HandleErrors Method");
            }
        }
        // End TT#733

        protected override bool ApplySecurity()
        {
            return true;
        }

        private void ReplacePackCombinations()
        {
            MIDException aStatusReason;

            // begin TT#607 Delete Combo not working
            //_BuildPacksMethod.RemoveCombinations(out aStatusReason);
            //if (aStatusReason != null)
            //{
            //    throw new MIDException(aStatusReason.ErrorLevel, aStatusReason.ErrorNumber,
            //        MIDText.GetTextOnly(aStatusReason.ErrorNumber), aStatusReason.InnerException);
            //}
            //else
            //{
            // end TT#607 Delete Combo not working 
            Infragistics.Win.UltraWinGrid.UltraGridChildBand ultraGridPackBand;
            List<PackPatternCombo> lstPackCombos = new List<PackPatternCombo>();
            foreach (UltraGridRow packComboBand in grdCandidatePacks.Rows)
            {
                if (packComboBand.Cells["DefinitionType"].Value.ToString().Trim() != "user")
                {
                    if (!_BuildPacksMethod.SetCombinationSelected(
                          ePackPatternType.Vendor,
                        //Convert.ToInt32(packComboBand.Cells["PackCombinationID"].Value, CultureInfo.CurrentCulture),  // change by Jim
                          Convert.ToInt32(packComboBand.Cells["PackCombinationRID"].Value, CultureInfo.CurrentCulture),   // change by Jim
                          Convert.ToBoolean(packComboBand.Cells["Choice"].Value, CultureInfo.CurrentCulture),
                          out aStatusReason))
                    {
                        throw aStatusReason;
                    }
                }
                else
                {
                    //if ((bool)packComboBand.Cells[0].Value == true)   // Temp Change (cannot save size run when this code is here
                    //{                                                 // Temp Change (cannot save size run when this code is here 

                    List<PackPattern> lstPacks = new List<PackPattern>();
                    ultraGridPackBand = packComboBand.ChildBands[0];	// get "PACKS" band
                    foreach (UltraGridRow packRow in ultraGridPackBand.Rows)
                    {
                        List<SizeUnits> lstSizeUnits = new List<SizeUnits>();
                        //int sizeCtr = 0;                    // TT#615 Addendum

                        //foreach (string size in lstSizes)   // TT#615 Addendum
                        string size;                          // TT#615 Addendum
                        foreach (int sizeRID in lstSizeRIDs)  // TT#615 Addendum
                        {
                            size = htSizes[sizeRID];          // TT#615 Addendum
                            foreach (UltraGridColumn packCol in ultraGridPackBand.Band.Columns)
                            {
                                if (size == packCol.Key)
                                {
                                    //int intSizeRID = Convert.ToInt32(lstSizeRIDs[sizeCtr]); // TT#615 Addendum
                                    if (packRow.Cells[size].Value.ToString().Trim() != "")
                                    {
                                        int intSizeUnits = Convert.ToInt32(packRow.Cells[size].Value);

                                        //SizeUnits su = new SizeUnits(intSizeRID, intSizeUnits);  // TT#615 Addendum
                                        SizeUnits su = new SizeUnits(sizeRID, intSizeUnits);       // TT#615 Addendum

                                        lstSizeUnits.Add(su);
                                    }
                                }
                            }

                            //sizeCtr++;  // TT#615 Addendum
                        }

                        BuildPacksMethod_PackPattern pp = null;
                        if (lstSizeUnits.Count == 0)
                        {
                            pp = new BuildPacksMethod_PackPattern(
                                _BuildPacksMethod.Key,
                                //Convert.ToInt32(packRow.Cells["PackDetailID"].Value) * -1,  // change by Jim
                                Convert.ToInt32(packRow.Cells["PackPatternRID"].Value),       // change by Jim
                                packRow.Cells["PackName"].Value.ToString().Trim(),
                                Convert.ToInt32(packRow.Cells["PackQuantity"].Value),
                                Convert.ToInt32(packRow.Cells["Maximum Packs"].Value));

                        }
                        else
                        {
                            pp = new BuildPacksMethod_PackPattern(
                               _BuildPacksMethod.Key,
                                //Convert.ToInt32(packRow.Cells["PackDetailID"].Value) * -1,  // change by Jim
                                Convert.ToInt32(packRow.Cells["PackPatternRID"].Value),       // change by Jim
                               packRow.Cells["PackName"].Value.ToString().Trim(),
                               lstSizeUnits);
                        }
                        lstPacks.Add(pp);

                    }

                    BuildPacksMethod_Combo pc = new BuildPacksMethod_Combo(
                        ahp.MethodRID,
                        //Convert.ToInt32(packComboBand.Cells["PackCombinationID"].Value) * -1,  // change by Jim
                        Convert.ToInt32(packComboBand.Cells["PackCombinationRID"].Value),        // change by Jim   
                        packComboBand.Cells["PackCombinationName"].Value.ToString().Trim(),
                        Convert.ToBoolean(packComboBand.Cells[0].Value),
                        lstPacks);

                    lstPackCombos.Add(pc);

                    //}  // Temp Change (cannot save size run when this code is here
                }
            }  

            //_BuildPacksMethod.AddCombinations(lstPackCombos, out aStatusReason);  // Temp Fix
            _BuildPacksMethod.ReplaceCombinations(lstPackCombos, out aStatusReason); // Temp Fix
            if (aStatusReason != null)
            {
                throw new MIDException(aStatusReason.ErrorLevel, aStatusReason.ErrorNumber,
                    MIDText.GetTextOnly(aStatusReason.ErrorNumber), aStatusReason.InnerException);
            }
            //} TT#607 Delete Pack Combo Pack not working
        }

        /// <summary>
        /// Use to set the specific fields in method object before updating
        /// </summary>
        override protected void SetSpecificFields()
        {

            //Profile prf = selectedHeaderList[0];
            //int prfKey = prf.Key;

            //AllocationHeaderProfile ahp = new AllocationHeaderProfile(prfKey);
            int key = _BuildPacksMethod.Key;

            try
            {
                //hand off control data to the business class
                MIDException aStatusReason;

                ReplacePackCombinations();

                ////_BuildPacksMethod.SetVendorName(cmbVendor.Text, out aStatusReason);
                ////if (aStatusReason != null)
                ////{
                ////    throw new MIDException(aStatusReason.ErrorLevel, aStatusReason.ErrorNumber, 
                ////        MIDText.GetTextOnly(aStatusReason.ErrorNumber), aStatusReason.InnerException);
                //////}

                //Infragistics.Win.UltraWinGrid.UltraGridChildBand ultraGridPackBand;
                //List<PackPatternCombo> lstPackCombos = new List<PackPatternCombo>();
                //foreach (UltraGridRow packComboBand in grdCandidatePacks.Rows)
                //{
                //    if (packComboBand.Cells["DefinitionType"].Value.ToString().Trim() != "user")
                //    {
                //        if (!_BuildPacksMethod.SetCombinationSelected(
                //              ePackPatternType.Vendor,
                //              //Convert.ToInt32(packComboBand.Cells["PackCombinationID"].Value, CultureInfo.CurrentCulture),  // change by Jim
                //              Convert.ToInt32(packComboBand.Cells["PackCombinationRID"].Value, CultureInfo.CurrentCulture),   // change by Jim
                //              Convert.ToBoolean(packComboBand.Cells["Choice"].Value, CultureInfo.CurrentCulture),
                //              out aStatusReason))
                //        {
                //            throw aStatusReason;
                //        }
                //    }
                //    else
                //    {
                //        //if ((bool)packComboBand.Cells[0].Value == true)   // Temp Change (cannot save size run when this code is here
                //        //{                                                 // Temp Change (cannot save size run when this code is here 

                //            List<PackPattern> lstPacks = new List<PackPattern>();
                //            ultraGridPackBand = packComboBand.ChildBands[0];	// get "PACKS" band
                //            foreach (UltraGridRow packRow in ultraGridPackBand.Rows)
                //            {
                //                List<SizeUnits> lstSizeUnits = new List<SizeUnits>();
                //                int sizeCtr = 0;

                //                foreach (string size in lstSizes)
                //                {
                //                    foreach (UltraGridColumn packCol in ultraGridPackBand.Band.Columns)
                //                    {
                //                        if (size == packCol.Key)
                //                        {
                //                            int intSizeRID = Convert.ToInt32(lstSizeRIDs[sizeCtr]);
                //                            if (packRow.Cells[size].Value.ToString().Trim() != "")
                //                            {
                //                                int intSizeUnits = Convert.ToInt32(packRow.Cells[size].Value);

                //                                SizeUnits su = new SizeUnits(intSizeRID, intSizeUnits);

                //                                lstSizeUnits.Add(su);
                //                            }
                //                        }
                //                    }

                //                    sizeCtr++;
                //                }

                //                BuildPacksMethod_PackPattern pp = null;
                //                if (lstSizeUnits.Count == 0)
                //                {
                //                    pp = new BuildPacksMethod_PackPattern(
                //                        _BuildPacksMethod.Key,
                //                        //Convert.ToInt32(packRow.Cells["PackDetailID"].Value) * -1,  // change by Jim
                //                        Convert.ToInt32(packRow.Cells["PackPatternRID"].Value),       // change by Jim
                //                        packRow.Cells["PackName"].Value.ToString().Trim(),
                //                        Convert.ToInt32(packRow.Cells["PackQuantity"].Value),
                //                        Convert.ToInt32(packRow.Cells["Maximum Packs"].Value));

                //                }
                //                else
                //                {
                //                    pp = new BuildPacksMethod_PackPattern(
                //                       _BuildPacksMethod.Key,
                //                        //Convert.ToInt32(packRow.Cells["PackDetailID"].Value) * -1,  // change by Jim
                //                        Convert.ToInt32(packRow.Cells["PackPatternRID"].Value),       // change by Jim
                //                       packRow.Cells["PackName"].Value.ToString().Trim(),
                //                       lstSizeUnits);
                //                }
                //                lstPacks.Add(pp);

                //            }

                //            BuildPacksMethod_Combo pc = new BuildPacksMethod_Combo(
                //                ahp.MethodRID,
                //                //Convert.ToInt32(packComboBand.Cells["PackCombinationID"].Value) * -1,  // change by Jim
                //                Convert.ToInt32(packComboBand.Cells["PackCombinationRID"].Value),        // change by Jim   
                //                packComboBand.Cells["PackCombinationName"].Value.ToString().Trim(),
                //                Convert.ToBoolean(packComboBand.Cells[0].Value),
                //                lstPacks);

                //            lstPackCombos.Add(pc);

                //        //}  // Temp Change (cannot save size run when this code is here
                //    }
                //}

                ////_BuildPacksMethod.AddCombinations(lstPackCombos, out aStatusReason);  // Temp Fix
                //_BuildPacksMethod.ReplaceCombinations(lstPackCombos, out aStatusReason); // Temp Fix
                //if (aStatusReason != null)
                //{
                //    throw new MIDException(aStatusReason.ErrorLevel, aStatusReason.ErrorNumber,
                //        MIDText.GetTextOnly(aStatusReason.ErrorNumber), aStatusReason.InnerException);
                //}

                // begin TT#615 Size Group, Size Curve and Size Run Issues
                if (!_BuildPacksMethod.SetSizeCurveGroupRID(Include.NoRID, out aStatusReason))
                {
                    // must clear size curve group before updating either (so both are not on method at same time)
                    throw new MIDException(aStatusReason.ErrorLevel, aStatusReason.ErrorNumber,
                        MIDText.GetTextOnly(aStatusReason.ErrorNumber), aStatusReason.InnerException);
                }
                if (!_BuildPacksMethod.SetSizeGroupRID(_selectedSizeGroupRID, out aStatusReason))
                {
                    throw new MIDException(aStatusReason.ErrorLevel, aStatusReason.ErrorNumber,
                        MIDText.GetTextOnly(aStatusReason.ErrorNumber), aStatusReason.InnerException);
                }
                if (!_BuildPacksMethod.SetSizeCurveGroupRID(_selectedSizeCurveRID, out aStatusReason))
                {
                    throw new MIDException(aStatusReason.ErrorLevel, aStatusReason.ErrorNumber,
                        MIDText.GetTextOnly(aStatusReason.ErrorNumber), aStatusReason.InnerException);    
                }
                //if (cboSizeGroup.Text.Trim() != "" && cboSizeGroup.Text != null)
                //{
                //    SizeGroupList sgl = new SizeGroupList(eProfileType.SizeGroup);
                //    sgl.LoadAll(true);
                //    SizeGroupProfile sgp = sgl.FindGroupName(cboSizeGroup.Text);
                //    _BuildPacksMethod.SetSizeGroupRID(sgp.Key, out aStatusReason);
                //    if (aStatusReason != null)
                //    {
                //        throw new MIDException(aStatusReason.ErrorLevel, aStatusReason.ErrorNumber,
                //            MIDText.GetTextOnly(aStatusReason.ErrorNumber), aStatusReason.InnerException);
                //    }
                //}

                //if (cboSizeCurve.Text.Trim() != "" && cboSizeCurve.Text != null)
                //{
                //    _BuildPacksMethod.SetSizeCurveGroupRID(Convert.ToInt32(cboSizeCurve.SelectedValue), out aStatusReason);
                //    if (aStatusReason != null)
                //    {
                //        throw new MIDException(aStatusReason.ErrorLevel, aStatusReason.ErrorNumber,
                //            MIDText.GetTextOnly(aStatusReason.ErrorNumber), aStatusReason.InnerException);
                //    }
                //}
                // end TT#615 Size Group, Size Curve and Size Run Issues

                //_BuildPacksMethod.SetSizeGroupRID(Include.NoRID, out aStatusReason);
                //if (aStatusReason != null)
                //{
                //    throw new MIDException(aStatusReason.ErrorLevel, aStatusReason.ErrorNumber,
                //        MIDText.GetTextOnly(aStatusReason.ErrorNumber), aStatusReason.InnerException);
                //}

                //_BuildPacksMethod.SetSizeCurveGroupRID(Include.NoRID, out aStatusReason);
                //if (aStatusReason != null)
                //{
                //    throw new MIDException(aStatusReason.ErrorLevel, aStatusReason.ErrorNumber,
                //        MIDText.GetTextOnly(aStatusReason.ErrorNumber), aStatusReason.InnerException);
                //}

                _BuildPacksMethod.SetReserveTotal(Convert.ToDouble(txtReserve.Text.Trim()), out aStatusReason);
                if (aStatusReason != null)
                {
                    throw new MIDException(aStatusReason.ErrorLevel, aStatusReason.ErrorNumber,
                        MIDText.GetTextOnly(aStatusReason.ErrorNumber), aStatusReason.InnerException);
                }

                if (rdoReservePercent.Checked == true && rdoReserveUnits.Checked == false)
                {
                    _BuildPacksMethod.ReserveTotalIsPercent = true;
                }
                else if (rdoReservePercent.Checked == false && rdoReserveUnits.Checked == true)
                {
                    _BuildPacksMethod.ReserveTotalIsPercent = false;
                }

                _BuildPacksMethod.SetReserveBulk(Convert.ToDouble(txtReserveBulk.Text.Trim()), out aStatusReason);
                if (aStatusReason != null)
                {
                    throw new MIDException(aStatusReason.ErrorLevel, aStatusReason.ErrorNumber,
                           MIDText.GetTextOnly(aStatusReason.ErrorNumber), aStatusReason.InnerException);
                }

                if (rdoReserveBulkPercent.Checked == true && rdoReserveBulkUnits.Checked == false)
                {
                    _BuildPacksMethod.ReserveBulkIsPercent = true;
                }
                else if (rdoReserveBulkPercent.Checked == false && rdoReserveBulkUnits.Checked == true)
                {
                    _BuildPacksMethod.ReserveBulkIsPercent = false;
                }

                _BuildPacksMethod.SetReservePacks(Convert.ToDouble(txtReservePacks.Text.Trim()), out aStatusReason);
                if (aStatusReason != null)
                {
                    throw new MIDException(aStatusReason.ErrorLevel, aStatusReason.ErrorNumber,
                           MIDText.GetTextOnly(aStatusReason.ErrorNumber), aStatusReason.InnerException);
                }

                if (rdoReservePacksPercent.Checked == true && rdoReservePacksUnits.Checked == false)
                {
                    _BuildPacksMethod.ReservePacksIsPercent = true;
                }
                else if (rdoReservePacksPercent.Checked == false && rdoReservePacksUnits.Checked == true)
                {
                    _BuildPacksMethod.ReservePacksIsPercent = false;
                }
                // begin TT#744 - Use Orig Pack Fitting Logic; Remove Bulk From Header
                if (chkBoxRemoveBulk.Checked)
                {
                    _BuildPacksMethod.RemoveBulkAfterFittingPacks = true;
                }
                else
                {
                    _BuildPacksMethod.RemoveBulkAfterFittingPacks = false;
                }
                // end TT#744 - Use Orig Pack Fitting Logic; Remove Bulk From Header
                // BEGIN TT#669 - AGallagher – Build Pack Method – Variance Options
                if (chkDepleteReserve.Checked == true)
                {
                    _BuildPacksMethod.DepleteReserveSelected = true;
                }
                else
                {
                    _BuildPacksMethod.DepleteReserveSelected = false;
                }
                string increaseBuyQty;  // TT#669 Build Packs Variance Enhancement Data Layer (JEllis)
                
                if (chkIncreaseBuyQty.Checked == true)
                {
                    _BuildPacksMethod.IncreaseBuySelected = true;
                    this.txtIncreaseBuyQty.Enabled = true;
                    // begin TT#669 Build Packs Variance Data Layer (JEllis)
                    //_BuildPacksMethod.IncreaseBuyPct = Convert.ToDouble(txtIncreaseBuyQty.Text.Trim());                     
                    increaseBuyQty = txtIncreaseBuyQty.Text.Trim();
                    // end TT#669 Build Packs Variance Data Layer (JEllis)
                    // BEGIN TT#669 - AGallagher – Build Pack Method – Variance Options
                    //if (txtIncreaseBuyQty.Text.Trim().Length != 0)
                    //    {
                    //       double wkincreaseBuyQty = Double.Parse(txtIncreaseBuyQty.Text);
                    //       if (wkincreaseBuyQty < 0)
                    //           ErrorProvider.SetError(txtIncreaseBuyQty, SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MustBeNonNegative));
                    //    }
                    //    else
                    //    {
                    //        ErrorProvider.SetError(txtIncreaseBuyQty, string.Empty);
                    //    }
                    // END TT#669 - AGallagher – Build Pack Method – Variance Options
                }
                else
                {
                    _BuildPacksMethod.IncreaseBuySelected = false;
                    //_BuildPacksMethod.IncreaseBuyPct = double.MaxValue;   // TT#669 Build Packs Variance Enhancement Data Layer (JEllis)
                    increaseBuyQty = string.Empty; // TT#669 Build Packs Variance Enhancement Data Layer (JEllis)
                    this.txtIncreaseBuyQty.Enabled = false;
                }
                // begin TT#669 Build Packs Variance Data Layer (JEllis)
                if (increaseBuyQty == string.Empty)
                {
                    _BuildPacksMethod.SetIncreaseBuyPct(double.MaxValue, out aStatusReason);
                }
                else
                {
                    _BuildPacksMethod.SetIncreaseBuyPct(Convert.ToDouble(txtIncreaseBuyQty.Text.Trim()), out aStatusReason);
                }
                // end TT#669 Build Packs Variance Data Layer (JEllis)
                if (aStatusReason != null)
                {
                    throw new MIDException(aStatusReason.ErrorLevel, aStatusReason.ErrorNumber,
                        MIDText.GetTextOnly(aStatusReason.ErrorNumber), aStatusReason.InnerException);
                }
                // END TT#669 - AGallagher – Build Pack Method – Variance Options
                // begin Change by Jim
                //_BuildPacksMethod.AvgPackErrorDevTolerance = (uint)Convert.ToInt32(txtAvgPackDeviationTolerance.Text.Trim());
                string avgPackDeviationTolerance = txtAvgPackDeviationTolerance.Text.Trim();
                if (avgPackDeviationTolerance == string.Empty)
                {
                    _BuildPacksMethod.AvgPackErrorDevTolerance = Include.DefaultMaxSizeErrorPercent;
                }
                else
                {
                    _BuildPacksMethod.AvgPackErrorDevTolerance = Convert.ToDouble(avgPackDeviationTolerance);
                }
                // end Change by Jim

                _BuildPacksMethod.ShipVariance = (uint)Convert.ToInt32(txtMaxPackAllocationNeedTolerance.Text);

            }
            catch
            {
                throw;
            }
        }

        protected override bool SaveChanges()
        {
            // begin TT#747 - JEllis - Cannot modify Custom Pack Combinations
            //return base.SaveChanges();
            if (base.SaveChanges())
            {
                LoadPackCombinations();
                return true;
            }
            return false;
            // end TT#747 - JEllis - Cannot modify Custom Pack Combinations
        }

        public override void UpdateWorkflowMethod(int aWorkflowMethodRID, int aNodeRID, MIDWorkflowMethodTreeNode aNode, eLockStatus aLockStatus)
        {
            try
            {
                _nodeRID = aNodeRID;
                _BuildPacksMethod = new BuildPacksMethod(SAB, aWorkflowMethodRID);
                base.UpdateWorkflowMethod(aLockStatus, eSecurityFunctions.AllocationMethodsUserBuildPacks, eSecurityFunctions.AllocationMethodsGlobalBuildPacks);

                Common_Load(aNode.GlobalUserType);

                LoadMethod();

                _BuildPacksMethod.Method_Change_Type = eChangeType.update;
            }
            catch (Exception ex)
            {
                HandleException(ex, "UpdateWorkflowMethod");
                FormLoadError = true;
            }

        }

        override public void NewWorkflowMethod(MIDWorkflowMethodTreeNode aParentNode)
        {
            try
            {
                _BuildPacksMethod = new BuildPacksMethod(SAB, Include.NoRID);
                ABM = _BuildPacksMethod;
                base.NewWorkflowMethod(aParentNode, eSecurityFunctions.AllocationMethodsUserBuildPacks, eSecurityFunctions.AllocationMethodsGlobalBuildPacks);

                Common_Load(aParentNode.GlobalUserType);

            }
            catch (Exception ex)
            {
                string exceptionMessage = ex.Message;
            }
        }


        /// <summary>
        /// Gets if workflow or method.
        /// </summary>
        override protected eWorkflowMethodIND WorkflowMethodInd()
        {
            return eWorkflowMethodIND.Methods;
        }

        /// <summary>
        /// Deletes a Multi Level Spread Method.
        /// </summary>
        /// <param name="aMethodRID">The record ID of the method</param>
        override public bool DeleteWorkflowMethod(int aMethodRID)
        {
            try
            {
                _BuildPacksMethod = new BuildPacksMethod(SAB, aMethodRID);

                _BuildPacksMethod.Method_Change_Type = eChangeType.delete;

                return Delete();
            }
            catch (DatabaseForeignKeyViolation)
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
        /// Use to set the specific method object before updating
        /// </summary>
        override protected void SetObject()
        {
            try
            {
                ABM = _BuildPacksMethod;
            }
            catch (Exception exception)
            {
                HandleException(exception);
            }
        }
        #endregion

        #region Properties Tab - Workflows
        /// <summary>
        /// Fill the _dtWorkflows DataTable and add TableStyle
        /// </summary>
        private void LoadWorkflows()
        {
            try
            {
                GetWorkflows(_BuildPacksMethod.Key, ugWorkflows);
            }
            catch (Exception ex)
            {
                HandleException(ex, "LoadWorkflows");
            }
        }
        #endregion


        //private void tabCriteria_Resize(object sender, EventArgs e)
        //{
        //    //resize controls with tab
        //    // Begin TT#735 - JSmith - Build Packs Information Icon on the on the criteria tab is too far to the right and hardly visable to the user.
        //    //this.lblCandidatePacks.Width = this.tabCriteria.Width - 12;
        //    //this.grdCandidatePacks.Width = this.tabCriteria.Width - 12;
        //    //this.grdCandidatePacks.Height = this.tabCriteria.Height - 194;
        //    //this.grpCriteria.Width = this.tabCriteria.Width - 12;
        //    this.lblCandidatePacks.Width = this.tabCriteria.Width - 24;
        //    this.grdCandidatePacks.Width = this.tabCriteria.Width - 24;
        //    this.grdCandidatePacks.Height = this.tabCriteria.Height - 194;
        //    this.grpCriteria.Width = this.tabCriteria.Width - 24;
        //    // End TT#735
        //}

        //private void tabConstraints_Resize(object sender, EventArgs e)
        //{
        //    //resize controls with tab
        //    grpConstraints.Width = this.tabConstraints.Width - 14;
        //    grpPackErrorOptions.Width = this.tabConstraints.Width - 14;
        //}

        //private void tabEvaluation_Resize(object sender, EventArgs e)
        //{
        //    //resize controls with tab
        //    splEvaluation.Width = this.tabEvaluation.Width - 14;
        //    splEvaluation.Height = this.tabEvaluation.Height - 48;
        //}

        //private void frmBuildPacksMethod_Resize(object sender, EventArgs e)
        //{
        //    if (this.WindowState == FormWindowState.Normal)
        //    {
        //        MessageBox.Show("normal");
        //    }

        //    //resize controls with form
        //    this.tabRuleMethod.Width = this.Width - 26;
        //    this.tabRuleMethod.Height = this.Height - 120;
        //    this.Refresh();


        //}

        private void frmBuildPacksMethod_Load(object sender, EventArgs e)
        {
            //formatting
            // Begin TT#TT#668-MD - JSmith - Windows 8
            //this.Width = 765;  // something is resizing the grid before here; not sure what
            // End TT#TT#668-MD - JSmith - 
            //this.Height = 615;

            DataTable dtBPCNames = _BuildPacksMethod.BPCNames;

            //Begin TT#746 - Vendor list need to be ordered in the dropdown - APicchetti - 6/21/2010
            DataView dvBPCNames = new DataView(dtBPCNames);
            dvBPCNames.Sort = "BPC_NAME asc";
            DataTable dtBPCNamesSorted = dvBPCNames.ToTable();

            for (int iBPCName = 0; iBPCName < dtBPCNames.Rows.Count; iBPCName++)
            {
                cmbVendor.Items.Add(dtBPCNamesSorted.Rows[iBPCName]["BPC_NAME"].ToString());
            }
            //End TT#746

            // Begin TT#5087 - JSmith - Vendor is blank
            cmbVendor.Text = _BuildPacksMethod.VendorName.Trim();
            // End TT#5087 - JSmith - Vendor is blank

            _BuildPacksMethod.IsInteractive = true;

            //BEGIN TT#110-MD-VStuart - In Use Tool
            tabRuleMethod.Controls.Remove(tabProperties);
            //END TT#110-MD-VStuart - In Use Tool

        }

        private int _newPackCombinationRID = 0;  // change by Jim
        private void mnuPackCombinationAdd_Click(object sender, EventArgs e)
        {
            //add a pack combination
            // begin change by Jim
            //AddPackCombination(true, true, Include.NoRID, null);
            _newPackCombinationRID--;
            AddPackCombination(true, true, _newPackCombinationRID, null);
            _newPackPatternRID--;   // TT#800-Argument Exception when adding packs
            AddPack(true, _newPackPatternRID, 1, 1, false, "", null, false); // TT#800-Argument Exception when adding packs
            //AddPack(true, Include.NoRID, 1, 1, false, "", null, false);  // TT#800-Argument Exception when adding packs
            // end change by Jim

            FormatGrids(grdCandidatePacks);

            grdCandidatePacks.Rows[grdCandidatePacks.ActiveRow.Index].Expanded = true;

        }

        private void CreateDataTables(bool UserAdded)
        {
            //initialize the pack combo datatable
            dtPackCombo = new DataTable("PACK_COMBOS");

            //create the primary key
            DataColumn[] dtKeys = new DataColumn[1];

            //add the needed fields and keys
            DataColumn pcCol = new DataColumn("Choice");
            pcCol.Caption = "";
            pcCol.DataType = System.Type.GetType("System.Boolean");
            dtPackCombo.Columns.Add(pcCol);

            pcCol = new DataColumn("PackCombinationID");
            pcCol.DataType = System.Type.GetType("System.Int32");
            dtPackCombo.Columns.Add(pcCol);
            dtKeys[0] = pcCol;

            pcCol = new DataColumn("PackCombinationName");
            pcCol.DataType = System.Type.GetType("System.String");
            dtPackCombo.Columns.Add(pcCol);

            pcCol = new DataColumn("DefinitionType");
            pcCol.DataType = System.Type.GetType("System.String");
            dtPackCombo.Columns.Add(pcCol);

            // begin changes by Jim
            pcCol = new DataColumn("PackCombinationRID");
            pcCol.DataType = System.Type.GetType("System.Int32");
            dtPackCombo.Columns.Add(pcCol);
            // end changes by Jim

            //define the primary key
            dtPackCombo.PrimaryKey = dtKeys;

            //initialize the pack datatable
            dtPack = new DataTable("PACKS");

            //create the primary key
            DataColumn[] dtKeys3 = new DataColumn[2];

            //add the needed fields and keys
            DataColumn pCol = new DataColumn("PackCombinationID");
            pCol.ExtendedProperties.Add("Type", "Pack");
            pCol.DataType = System.Type.GetType("System.Int32");
            dtPack.Columns.Add(pCol);
            dtKeys3[0] = pCol;

            pCol = new DataColumn("PackDetailID");
            pCol.ExtendedProperties.Add("Type", "Pack");
            pCol.DataType = System.Type.GetType("System.Int32");
            dtPack.Columns.Add(pCol);
            dtKeys3[1] = pCol;

 
            pCol = new DataColumn("PackName");
            pCol.ExtendedProperties.Add("Type", "Pack");
            pCol.DataType = System.Type.GetType("System.String");
            dtPack.Columns.Add(pCol);

            pCol = new DataColumn("PackQuantity");
            pCol.ExtendedProperties.Add("Type", "Pack");
            pCol.DataType = System.Type.GetType("System.Int32");
            dtPack.Columns.Add(pCol);

            pCol = new DataColumn("Maximum Packs");
            pCol.ExtendedProperties.Add("Type", "Pack");
            pCol.DataType = System.Type.GetType("System.String");
            dtPack.Columns.Add(pCol);

            // begin changes by Jim
            pCol = new DataColumn("PackPatternRID");
            pCol.ExtendedProperties.Add("Type", "Pack");
            pCol.DataType = System.Type.GetType("System.Int32");
            dtPack.Columns.Add(pCol);
            // end changes by Jim

            //lstSizeRIDs.Sort();  // TT#615 Addendum

            foreach (int intSize in lstSizeRIDs)
            {
                string sizeName = htSizes[intSize].ToString();
                pCol = new DataColumn(sizeName);
                pCol.ExtendedProperties.Add("Type", "Size");
                pCol.DataType = System.Type.GetType("System.Int32");
                dtPack.Columns.Add(pCol);
            }
            //foreach (string strSize in lstSizes)
            //{
            //    pCol = new DataColumn(strSize);
            //    pCol.ExtendedProperties.Add("Type", "Size");
            //    pCol.DataType = System.Type.GetType("System.Int32");
            //    dtPack.Columns.Add(pCol);

            //}

            //define the primary key
            dtPack.PrimaryKey = dtKeys3;

            //initialize the dataset and add tables
            dsPackCombo = new DataSet();
            dsPackCombo.Tables.Add(dtPackCombo);
            dsPackCombo.Tables.Add(dtPack);

            //define the relationship
            DataRelation drPackCombo = new DataRelation("PackCriteriaBreakdown",
                dsPackCombo.Tables[0].Columns["PackCombinationID"],
                dsPackCombo.Tables[1].Columns["PackCombinationID"]);
            dsPackCombo.Relations.Add(drPackCombo);

            //define the grid's datasource
            grdCandidatePacks.DataSource = dsPackCombo;

        }

        // begin TT#747 - JEllis - Cannot modify custom Pack Combinations
        private void LoadPackCombinations()
        {
            List<PackPatternCombo> lstPackCombos = _BuildPacksMethod.PackCombination;
            if (dtPackCombo != null)
            {
                dtPack.Clear();
                dtPackCombo.Clear();
            }
            foreach (PackPatternCombo packCombo in lstPackCombos)
            {
                if (packCombo.PackPatternType == ePackPatternType.Vendor)
                {
                    AddPackCombination(false, packCombo.ComboSelected, packCombo.ComboRID, packCombo.ComboName);

                    foreach (PackPattern pack in packCombo.PackPatternList)
                    {
                        // begin change by Jim
                        //AddPack(false, packCombo.ComboRID, pack.PackPatternRID, pack.PackMultiple, pack.MaxPatternPacks,
                        //    pack.PatternIncludesSizeRun, pack.PatternName, pack.SizeRun, false);
                        AddPack(false, pack.PackPatternRID, pack.PackMultiple, pack.MaxPatternPacks,
                            pack.PatternIncludesSizeRun, pack.PatternName, pack.SizeRun, false);
                        // end change by Jim
                    }
                }
                else
                {
                    AddPackCombination(true, packCombo.ComboSelected, packCombo.ComboRID, packCombo.ComboName);

                    foreach (PackPattern pack in packCombo.PackPatternList)
                    {
                        // begin change by Jim
                        //AddPack(true, packCombo.ComboRID, pack.PackPatternRID, pack.PackMultiple, pack.MaxPatternPacks,
                        //   pack.PatternIncludesSizeRun, pack.PatternName, pack.SizeRun, false);
                        AddPack(true, pack.PackPatternRID, pack.PackMultiple, pack.MaxPatternPacks,
                           pack.PatternIncludesSizeRun, pack.PatternName, pack.SizeRun, false);
                        // end change by Jim

                    }
                }
            }
        }
        // end TT#747 - JEllis - Cannot modify custom Pack Combinations

        // begin changes by Jim
        //private void AddPackCombination(bool UserAdded, bool Choice, int PackCombinationID, string PackCombinationName)
        /// <summary>
        /// Adds a pack combination to the form
        /// </summary>
        /// <param name="UserAdded">True: user added combination; False: vendor combination</param>
        /// <param name="Choice">True: combination is selected; False: combination not selected</param>
        /// <param name="PackCombinationRID">RID of the combination on the database; use uniquely generated negative numbers when "new"</param>
        /// <param name="PackCombinationName">Name of the combination</param>
        private void AddPackCombination(bool UserAdded, bool Choice, int PackCombinationRID, string PackCombinationName)
            // end changes by Jim
        {
            //create the pack combo/pack tables if needed
            if (dtPackCombo == null)
            {
                CreateDataTables(UserAdded);
            }

            //create the internal pack combination id
            int intPackComboIdx = 1;
            if (dtPackCombo.Rows.Count > 0)
            {
                intPackComboIdx = Convert.ToInt32(dtPackCombo.Rows[dtPackCombo.Rows.Count - 1]["PackCombinationID"]) + 1;
            }

            //add the new row
            if (UserAdded == true)
            {
                DataRow pcRow = dtPackCombo.NewRow();
                pcRow["Choice"] = Choice;
                pcRow["PackCombinationID"] = intPackComboIdx;
                pcRow["PackCombinationRID"] = PackCombinationRID;  // change by Jim
                if (PackCombinationName == null || PackCombinationName.Trim() == "")
                {
                    pcRow["PackCombinationName"] = "Pack Combination " + intPackComboIdx.ToString().Trim();
                }
                else
                {
                    pcRow["PackCombinationName"] = PackCombinationName;
                }
                pcRow["DefinitionType"] = "user";
                dtPackCombo.Rows.Add(pcRow);
            }
            else
            {
                DataRow pcRow = dtPackCombo.NewRow();
                //pcRow["Choice"] = true;  // Temp Change   // without this change, non-selected vendors come back as selected
                pcRow["Choice"] = Choice;  // Temp Change   // without this change, non-selected vendors come back as selected
                //pcRow["PackCombinationID"] = PackCombinationID;  // change by Jim 
                pcRow["PackCombinationID"] = intPackComboIdx;      // change by Jim 
                pcRow["PackCombinationRID"] = PackCombinationRID;  // change by Jim
                pcRow["PackCombinationName"] = PackCombinationName;
                pcRow["DefinitionType"] = "Vendor";
                dtPackCombo.Rows.Add(pcRow);
            }

            //enable the add pack & delete functionality if pack combos exist
            if (dtPackCombo.Rows.Count > 0)
            {
                mnuPackCombinationAddPack.Enabled = true;
                mnuPackCombinationDelete.Enabled = true;
            }

            //Activate last row that was added to the grid
            grdCandidatePacks.ActiveRow = grdCandidatePacks.Rows[grdCandidatePacks.Rows.Count - 1];

        }

        public void FormatGrids(Infragistics.Win.UltraWinGrid.UltraGrid grid)
        {
            grid.DataBind();

            //format the pack candidates grid
            foreach (Infragistics.Win.UltraWinGrid.UltraGridBand band in grid.DisplayLayout.Bands)
            {
                band.Override.HeaderAppearance.BackColor = btnApply.BackColor;
                band.Override.HeaderAppearance.BackGradientStyle = GradientStyle.None;
                band.Override.RowSelectorAppearance.BackColor = btnApply.BackColor;
                band.Override.RowSelectorAppearance.BackGradientStyle = GradientStyle.None;
                band.Override.ActiveRowAppearance.BackColor = grdResults.DefaultCellStyle.SelectionBackColor;
                band.Override.ActiveRowAppearance.ForeColor = grdResults.DefaultCellStyle.SelectionForeColor;
                band.Override.ActiveRowAppearance.BackGradientStyle = GradientStyle.None;
                band.Override.ActiveRowAppearance.BorderColor = grdResults.GridColor;
            }

            if(grid == grdCandidatePacks)
            {
                grid.DisplayLayout.Bands[0].Columns[1].Header.Caption = "Combination ID";
                grid.DisplayLayout.Bands[0].Columns[1].Hidden = true;
                grid.DisplayLayout.Bands[0].Columns[0].Header.Caption = "";
                grid.DisplayLayout.Bands[0].Columns[2].Header.Caption = "Combination Name";
                grid.DisplayLayout.Bands[1].Columns[0].Hidden = true;
                grid.DisplayLayout.Bands[1].Columns[1].Hidden = true;
                grid.DisplayLayout.Bands[1].Columns[2].Hidden = true;
                grid.DisplayLayout.Bands[0].Columns[3].Hidden = true;
                grid.DisplayLayout.Bands[0].Columns[4].Hidden = true; // change by Jim
                grid.DisplayLayout.Bands[1].Columns[5].Hidden = true; // change by Jim
            }

            if (grid == grdSummary)
            {
                grid.DisplayLayout.Override.AllowColSizing = AllowColSizing.Synchronized;  // TT#886 - Distinct Packs have same size runs
                grid.DisplayLayout.Bands[0].Columns[0].Hidden = true;
                grid.DisplayLayout.Bands[1].Columns[0].Hidden = true;
                // begin TT#886 - Distinct Packs have same size runs
                grid.DisplayLayout.Bands[0].Columns[2].Hidden = true;
                grid.DisplayLayout.Bands[0].Columns[3].Hidden = true;
                grid.DisplayLayout.Bands[0].Columns[1].ColSpan = 3;
                // end TT#886 - Distinct Packs have same size runs 
            }

            grid.DisplayLayout.Appearance.BackColor = grdResults.BackgroundColor;
            grid.DisplayLayout.Appearance.BackColor2 = grdResults.BackgroundColor;
            grid.DisplayLayout.Appearance.BackGradientStyle = GradientStyle.None;

            if (grid == grdCandidatePacks)
            {
                for (int gRow = 0; gRow < grdCandidatePacks.Rows.Count; gRow++)
                {
                    string strType = "";
                    if (grdCandidatePacks.Rows[gRow].Band.Index == 0)
                    {
                        strType = grdCandidatePacks.Rows[gRow].Cells["DefinitionType"].Value.ToString().ToLower().Trim();
                    }
                    for (int gCell = 0; gCell < grdCandidatePacks.Rows[gRow].Cells.Count; gCell++)
                    {
                        if (grdCandidatePacks.Rows[gRow].Cells[gCell].Value.GetType() != System.Type.GetType("System.Boolean"))
                        {
                            if (strType == "vendor")
                            {
                                grdCandidatePacks.Rows[gRow].Cells[gCell].Activation = Activation.Disabled;

                                UltraGridRow aParentRow = grdCandidatePacks.Rows[gRow];


                                if (aParentRow.HasChild() == true)
                                {
                                    UltraGridRow cRow = aParentRow.GetChild(ChildRow.First);
                                    while (cRow != null)
                                    {
                                        cRow.Activation = Activation.Disabled;
                                        cRow = cRow.GetSibling(SiblingRow.Next, false, false);
                                    }

                                }
                            }
                        }
                    }
                }
            }


            //format the evaluation summary grid
            if (grid == grdSummary)  // TT#886 - Distinct Packs have same size runs
            {                        // TT#886 - Distinct Packs have same size runs
                foreach (Infragistics.Win.UltraWinGrid.UltraGridBand band in grdSummary.DisplayLayout.Bands)
                {

                    band.Override.HeaderAppearance.BackColor = btnApply.BackColor;
                    band.Override.HeaderAppearance.BackGradientStyle = GradientStyle.None;
                    band.Override.RowSelectorAppearance.BackColor = btnApply.BackColor;
                    band.Override.RowSelectorAppearance.BackGradientStyle = GradientStyle.None;
                    band.Override.ActiveRowAppearance.BackColor = grdResults.DefaultCellStyle.SelectionBackColor;
                    band.Override.ActiveRowAppearance.ForeColor = grdResults.DefaultCellStyle.SelectionForeColor;
                    band.Override.ActiveRowAppearance.BackGradientStyle = GradientStyle.None;
                    band.Override.ActiveRowAppearance.BorderColor = grdResults.GridColor;
                    // begin TT#886 - Distinct Packs have same size runs
                    for (int dgvCol = 0; dgvCol < band.Columns.Count; dgvCol++)
                    {
                        if (dgvCol > 1)
                        {
                            band.Columns[dgvCol].CellAppearance.TextHAlign = HAlign.Right;
                        }
                    }
                    // end TT#886 - Distinct Packs have same size runs
                }
                grdSummary.DisplayLayout.Appearance.BackColor = grdResults.BackgroundColor;
                grdSummary.DisplayLayout.Appearance.BackColor2 = grdResults.BackgroundColor;
                grdSummary.DisplayLayout.Appearance.BackGradientStyle = GradientStyle.None;
                grdSummary.DisplayLayout.Appearance.BackColor = grdResults.BackgroundColor;
                grdSummary.DisplayLayout.Appearance.BackColor2 = grdResults.BackgroundColor;
                grdSummary.DisplayLayout.Appearance.BackGradientStyle = GradientStyle.None;
            }   // TT#886 - Distinct Packs have same size runs
            //align the text of the resulting grids
            for (int dgvCol = 0; dgvCol < grdBPSummary.Columns.Count; dgvCol++)
            {
                if (dgvCol > 0)
                {
                    grdBPSummary.Columns[dgvCol].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                }

            }

            for (int dgvCol = 0; dgvCol < grdResults.Columns.Count; dgvCol++)
            {
                if (dgvCol > 0)
                {
                    grdResults.Columns[dgvCol].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                }
            }

        }

        private int _newPackPatternRID = 0;   // change by Jim
        private void mnuPackCombinationAddPack_Click(object sender, EventArgs e)
        {
            //add pack
            // begin change by Jim
            //AddPack(true, Include.NoRID, Include.NoRID, Include.NoRID, Include.NoRID, false, null, null, true);
            _newPackPatternRID--;
            AddPack(true, _newPackPatternRID, 1, 1, false, null, null, true);
            // end change by Jim
        }

        // begin change by Jim
        //private void AddPack(bool UserAdded, int PackPatternRID, int ParentIdx, int PackIdx, int PackMultiple,
        //  int MaxPatternPacks, bool PatternIncludesSizeRun, string PatternName, SizeUnitRun SizeRun, bool add)
        /// <summary>
        /// Adds Pack to Combination on the form
        /// </summary>
        /// <param name="UserAdded">True: pack added by user; False: vendor pack</param>
        /// <param name="PackPatternRID">RID of the pack pattern on the database; use an uniquely assigned negative number for new pack patterns</param>
        /// <param name="PackMultiple">Number of units in this pack</param>
        /// <param name="MaxPatternPacks">Maximum number of packs with this pattern that may be placed on a header</param>
        /// <param name="PatternIncludesSizeRun">True: Pattern contains a specific size run; False: Size run will be generated</param>
        /// <param name="PatternName">Identifying name for this pack pattern</param>
        /// <param name="SizeRun">Size run; ignored when PatternIncludesSizeRun is false.</param>
        /// <param name="add">True: first time pack added; False: pack pattern is being updated</param>
        private void AddPack(bool UserAdded, int PackPatternRID, int PackMultiple,
            int MaxPatternPacks, bool PatternIncludesSizeRun, string PatternName, SizeUnitRun SizeRun, bool add)
            // end change by Jim
        {
            //get the pack combination id to add to
            UltraGridRow row = grdCandidatePacks.ActiveRow;
            int PackComboIdx = Convert.ToInt32(row.Cells["PackCombinationID"].Value);
            string PackComboName = "";
            // begin TT#589 Build Packs added size curve, name holds but sizes do not
            //if (row.ParentRow == null)
            //{
            //    PackComboName = row.Cells["PackCombinationName"].Value.ToString().Trim();
            //}
            //else
            //{
            //    PackComboName = row.ParentRow.Cells["PackCombinationName"].Value.ToString().Trim();
            //}
            UltraGridRow parentRow;
            if (row.ParentRow == null)
            {
                parentRow = row;
            }
            else
            {
                parentRow = row.ParentRow;
            }
            PackComboName = parentRow.Cells["PackCombinationName"].Value.ToString().Trim();
            //get the packcombinationid
            int intPackCombo = Convert.ToInt32(parentRow.Cells["PackCombinationID"].Value);
            bool error = false;
            if (UserAdded) // TT#589 addendum conditional code to only limit adding a pack when user is doing it
            {
                for (int packRow = dtPack.Rows.Count - 1; packRow >= 0; packRow--)
                {
                    DataRow rowPack = dtPack.Rows[packRow];
                    DataRow rowPackCombo = rowPack.GetParentRow("PackCriteriaBreakdown");
                    if ((int)rowPack["PackCombinationID"] == intPackCombo)
                    {
                        if (rowPackCombo["DefinitionType"].ToString().ToLower().Trim() == "vendor")
                        {
                            error = true;
                            break;
                        }
                    }
                }
            }  // TT#589 addendum
            if (error)
            {
                //warn the user about inability to modify vendor defined pack combos

                //Begin TT#684 - APicchetti - Build Packs Soft Text
                //MessageBox.Show("You cannot add packs to a vendor defined pack combination.",
                //    "Add Pack Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_VendorAddAttempt),"", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //End TT#684 - APicchetti - Build Packs Soft Text
            }
            else
            {
                // end TT#589 Build Packs added size curve, name holds but sizes do not

                //add the new pack
                int packIdx = 0;

                //get pack index for naming
                if (grdCandidatePacks.ActiveRow == null)
                {
                    packIdx = 1;
                    grdCandidatePacks.Rows[0].Activate();
                }
                else if (grdCandidatePacks.ActiveRow.ParentRow == null)
                {
                    if (grdCandidatePacks.ActiveRow.ChildBands[0].Rows.Count == 0)
                    {
                        packIdx = 1;
                    }
                    else
                    {
                        int rowIdx = grdCandidatePacks.ActiveRow.ChildBands[0].Rows.Count - 1;
                        string lastPackName = grdCandidatePacks.ActiveRow.ChildBands[0].Rows[rowIdx].Cells[2].Value.ToString().Trim();
                        char[] delim = ".".ToCharArray();
                        string[] lastPackNames = lastPackName.Split(delim);
                        packIdx = Convert.ToInt32(lastPackNames[1]) + 1;
                    }
                }
                else
                {
                    //UltraGridRow parentRow = grdCandidatePacks.ActiveRow.ParentRow; // TT#589 Build Packs added size curve, name holds but sizes do not
                    int rowIdx = parentRow.ChildBands[0].Rows.Count - 1;
                    string lastPackName = parentRow.ChildBands[0].Rows[rowIdx].Cells[2].Value.ToString().Trim();
                    char[] delim = ".".ToCharArray();
                    string[] lastPackNames = lastPackName.Split(delim);
                    packIdx = Convert.ToInt32(lastPackNames[1]) + 1;
                }

                //set grid values
                if (UserAdded == true)
                {
                    DataRow pRow = dtPack.NewRow();
                    pRow["PackCombinationID"] = PackComboIdx;
                    pRow["PackDetailID"] = packIdx;
                    pRow["PackPatternRID"] = PackPatternRID; // change by Jim
                    pRow["PackName"] = PackComboName + "." + packIdx.ToString().Trim();
                    pRow["Maximum Packs"] = "1";
                    if (add == false)
                    {
                        pRow["PackQuantity"] = PackMultiple;
                        pRow["Maximum Packs"] = MaxPatternPacks;
                    }
                    // begin Temp Fix
                    if (PatternIncludesSizeRun == true)
                    {
                        //Parse the size run and enter the values
                        AddSizeRunToPack(pRow, SizeRun);
                    }
                    // end Temp Fix
                    dtPack.Rows.Add(pRow);
                }
                else
                {
                    DataRow pRow = dtPack.NewRow();
                    pRow["PackCombinationID"] = PackComboIdx;
                    //pRow["PackDetailID"] = PackIdx;        // change by Jim
                    pRow["PackDetailID"] = packIdx;          // change by Jim
                    pRow["PackPatternRID"] = PackPatternRID; // change by Jim
                    pRow["PackName"] = PackComboName + "." + packIdx.ToString().Trim();
                    pRow["PackQuantity"] = PackMultiple;
                    pRow["Maximum Packs"] = MaxPatternPacks;

                    if (PatternIncludesSizeRun == true)
                    {
                        //Parse the size run and enter the values
                        // begin emp Fix
                        //Infragistics.Win.UltraWinGrid.UltraGridChildBand ultraGridPackBand;
                        //foreach (UltraGridRow packComboBand in grdCandidatePacks.Rows)
                        //{
                        //    ultraGridPackBand = packComboBand.ChildBands[0];	// get "PACKS" band
                        //    foreach (UltraGridRow packRow in ultraGridPackBand.Rows)
                        //    {
                        //        for(int intSize = 0; intSize > SizeRun.Count; intSize++)
                        //        {
                        //            SizeUnits su = (SizeUnits)SizeRun[intSize];

                        //            packRow.Cells[intSize + 5].Value = su.Units;

                        //        }
                        //    }
                        //}
                        AddSizeRunToPack(pRow, SizeRun);
                        // end Temp Fix
                    }

                    dtPack.Rows.Add(pRow);
                    grdCandidatePacks.DataSource = dsPackCombo;
                    grdCandidatePacks.ActiveRow = grdCandidatePacks.Rows[grdCandidatePacks.Rows.Count - 1];
                }

                //make vendor added cells read only
                if (UserAdded != true)
                {
                    for (int gRow = 0; gRow < grdCandidatePacks.Rows.Count; gRow++)
                    {
                        for (int gCRow = 0; gCRow < grdCandidatePacks.Rows[gRow].ChildBands[0].Rows.Count; gCRow++)
                        {
                            for (int gCell = 0; gCell < grdCandidatePacks.Rows[gRow].ChildBands[0].Rows[gCRow].Cells.Count; gCell++)
                            {
                                if (grdCandidatePacks.Rows[gRow].ChildBands[0].Rows[gCRow].Cells[gCell].Value.GetType() !=
                                    System.Type.GetType("System.Boolean"))
                                {
                                    grdCandidatePacks.Rows[gRow].ChildBands[0].Rows[gCRow].Cells[gCell].Activation = Activation.Disabled;
                                }
                            }
                        }
                    }
                }

                //expand the grid to show off new rows
                //grdCandidatePacks.Rows.ExpandAll(true);
            } // TT#589 Build Packs Size Curve added, name holds but sizes do not

        }
        // begin Temp
        private void AddSizeRunToPack(DataRow aPackRow, SizeUnitRun aSizeUnitRun)
        {
            int sizeCodeRID;
            SizeUnits suUnits;
            // begin TT#615 Addendum
            //for (int i = 0; i < lstSizes.Count; i++)
            for (int i=0; i<lstSizeRIDs.Count; i++)
                // end TT#615 Addendum
            {
                sizeCodeRID = lstSizeRIDs[i];

                if(aSizeUnitRun.TryGetValue(sizeCodeRID, out suUnits))
                {
                    //aPackRow[lstSizes[i]] = suUnits.Units;   // TT#615 Addendum
                    aPackRow[htSizes[sizeCodeRID]] = suUnits.Units; // TT#615 Addendum
                }
            }
        }
        // end Temp
        private void mnuPackCombinationDelete_Click(object sender, EventArgs e)
        {
            //Delete Pack or pack combination
            DeleteFromPackComboGrid();
        }

        private void DeleteFromPackComboGrid()
        {
            //determine if active row is pack or pack combination level
            UltraGridRow ParentRow = grdCandidatePacks.ActiveRow.ParentRow;

            //if pack combination level, delete pack combination and all child packs

            if (ParentRow == null)
            {

                //Begin TT#684 - APicchetti - Build Packs Soft Text

                //if (MessageBox.Show("Are you sure that you would like delete this pack combination?",
                //    "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                
                if(MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_DeletePackCombo), "", 
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)

                //End TT#684 - APicchetti - Build Packs Soft Text
                {
                    //get active row
                    UltraGridRow row = grdCandidatePacks.ActiveRow;

                    //get the packcombinationid
                    int intPackCombo = Convert.ToInt32(row.Cells["PackCombinationID"].Value);

                    //delete from pack table first
                    bool isVendorRow = false;
                    for (int packRow = dtPack.Rows.Count - 1; packRow >= 0; packRow--)
                    {
                        DataRow rowPack = dtPack.Rows[packRow];
                        DataRow rowPackCombo = rowPack.GetParentRow("PackCriteriaBreakdown");
                        if ((int)rowPack["PackCombinationID"] == intPackCombo)
                        {
                            if (rowPackCombo["DefinitionType"].ToString().ToLower().Trim() == "vendor")
                            {
                                //Begin TT#684 - APicchetti - Build Packs Soft Text

                                //warn the user about inability to delete vendor defined pack combos
                                //MessageBox.Show("You cannot delete a vendor defined pack combination.",
                                //    "Delete Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                                MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_VerdorDeleteAttempt), "", 
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                                //End TT#684 - APicchetti - Build Packs Soft Text

                                isVendorRow = true;

                                break;
                            }
                            else
                            {
                                //remove the row
                                dtPack.Rows.RemoveAt(packRow);
                            }
                        }
                    }

                    //remove the parent row
                    if (isVendorRow == false)
                    {
                        row.Delete(false);
                    }
                }
            }
            else
            {
                //Begin TT#684 - APicchetti - Build Packs Soft Text
                //if (MessageBox.Show("Are you sure that you would like delete this pack?",
                //    "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)

                if(MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_DeletePackCombo), "", 
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning) ==DialogResult.Yes)
                //End TT#684 - APicchetti - Build Packs Soft Text


                {
                    //delete active pack
                    UltraGridRow row = grdCandidatePacks.ActiveRow;
                    row.Delete(false);
                }
            }

            ReplacePackCombinations();
        }

        private void cmbVendor_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (cmbVendor.SelectedIndex <= 0)
            {
                return;
            }

            //clear grid source if it is not null
            bool blSizeGroup = false;
            bool blSizeCurve = false;
            if (dsPackCombo != null)
            {
                if (dtPack.Rows.Count != 0 || dtPackCombo.Rows.Count != 0)
                {
                    this.grdCandidatePacks.BeginUpdate();
                    this.grdCandidatePacks.SuspendRowSynchronization();

                    grdCandidatePacks.DataSource = null;

                    dtPack.Rows.Clear();
                    dtPackCombo.Rows.Clear();

                    grdCandidatePacks.DataSource = dsPackCombo;

                    this.grdCandidatePacks.ResumeRowSynchronization();
                    this.grdCandidatePacks.EndUpdate();

                    //Begin TT#943 - Receive unhandled exception when changing the vendor from blank to a vendor on an existing Build Pack method. - apicchetti - 10/20/2010
                    if (this.cboSizeGroup.Text != "")
                    {
                        blSizeGroup = true;
                    }

                    if (this.cboSizeCurve.Text != "")
                    {
                        blSizeCurve = true;
                    }
                    //End TT#943 - Receive unhandled exception when changing the vendor from blank to a vendor on an existing Build Pack method. - apicchetti - 10/20/2010
               }
            }

            //set the vendor properties
            MIDException aStatusReason;
            _BuildPacksMethod.SetVendorName(cmbVendor.Text, out aStatusReason);
            if (aStatusReason != null)
            {
                throw new MIDException(aStatusReason.ErrorLevel, aStatusReason.ErrorNumber,
                    MIDText.GetTextOnly(aStatusReason.ErrorNumber), aStatusReason.InnerException);
            }

            //get the sizes to display
            if ((selectedHeaderList == null && _BuildPacksMethod.IsInteractive == false) || 
                (txtBasisHeader.Text.Trim() == "" && _BuildPacksMethod.IsInteractive == true))
            {
                //if (cboSizeCurve.Text.Trim() == "" && cboSizeGroup.Text.Trim() == "")
                //{
                //    MessageBox.Show("There is no header attached and you have not selected a Size Group or Size Curve to use.",
                //        "Build Packs Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //}
                //else
                //{
                if ((cboSizeCurve.Text != "" && cboSizeCurve.Text != null) && blSizeCurve == false)  //TT#943 - Receive unhandled exception when changing the vendor from blank to a vendor on an existing Build Pack method. - apicchetti - 10/20/2010
                {
                    // begin Temp Fix
                    //SizeGroupProfile sgp = new SizeGroupProfile(asbm.SizeCurveGroupRid); 
                    //
                    //SizeCodeList scl = sgp.SizeCodeList;
                    SizeCurveGroupProfile scgp = new SizeCurveGroupProfile(asbm.SizeCurveGroupRid); 

                    SizeCodeList scl = (SizeCodeList)scgp.SizeCodeList;

                    //foreach (SizeCodeProfile sci in scl)
                    //{
                    //    lstSizeRIDs.Add(sci.SizeCodePrimaryRID);

                    //    lstSizes.Add(sci.SizeCodePrimary);
                    //    string sizeName = sci.SizeCodePrimary;

                    //}
                    Build_lstSizes(scl);
                    // end Temp Fix
                }
                else
                {
                    SizeGroupProfile sgp = new SizeGroupProfile(asbm.SizeGroupRid);

                    SizeCodeList scl = sgp.SizeCodeList;

                    Build_lstSizes(scl);

                }
                //}
            }
            else if ((selectedHeaderList.Count > 0 && _BuildPacksMethod.IsInteractive == false) || 
                (txtBasisHeader.Text.Trim() == "" && _BuildPacksMethod.IsInteractive == true))
            {
                if (selectedHeaderList.Count > 1)
                {
                    //Begin TT#684 - APicchetti - Build Packs Soft Text

                    //MessageBox.Show("The interactive mode of the Build Packs method \n" +
                    //    "only supports one header at a time. Only the first \n" +
                    //    "chosen header will be processed.", "Build Packs warning",
                    //    MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_InteractiveOneHeader), "", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    //End TT#684 - APicchetti - Build Packs Soft Text
                }
                Profile prf = selectedHeaderList[0];
                int prfKey = prf.Key;

                AllocationHeaderProfile ahp = new AllocationHeaderProfile(prfKey);

                AllocationProfile alHeader = new AllocationProfile(SAB, null, ahp.Key, SAB.ApplicationServerSession);

                // begin temp fix
                //SizeGroupProfile sgp = new SizeGroupProfile(alHeader.SizeGroupRID);
                //
                //SizeCodeList scl = sgp.SizeCodeList;
                SizeCodeList scl = alHeader.GetSizeCodeList();
                // end Temp Fix
                //foreach (SizeCodeProfile sci in scl)
                //{
                //    lstSizeRIDs.Add(sci.SizeCodePrimaryRID);
                //    lstSizes.Add(sci.SizeCodePrimary);
                //}

                Build_lstSizes(scl);
            }

            //display the other vendor information
            txtVendorPackOrderMin.Text = _BuildPacksMethod.PackMinOrder.ToString().Trim(); // TT#787 Vendor Min Order applies only to packs
            txtSizeMultiple.Text = _BuildPacksMethod.SizeMultiple.ToString().Trim();
            lstVendorPackCombos = _BuildPacksMethod.PackCombination;

            foreach (PackPatternCombo packCombo in lstVendorPackCombos)
            {
                if (packCombo.PackPatternType == ePackPatternType.Vendor)
                {
                    AddPackCombination(false, packCombo.ComboSelected, packCombo.ComboRID, packCombo.ComboName);
                    //int parentIdx = grdCandidatePacks.Rows.Count - 1;   // change by Jim

                    foreach (PackPattern pack in packCombo.PackPatternList)
                    {
                        // begin change by Jim
                        //AddPack(false, parentIdx, pack.PackPatternRID, pack.PackMultiple, pack.MaxPatternPacks,
                        //    pack.PatternIncludesSizeRun, pack.PatternName, pack.SizeRun, false);
                        AddPack(false, pack.PackPatternRID, pack.PackMultiple, pack.MaxPatternPacks,
                              pack.PatternIncludesSizeRun, pack.PatternName, pack.SizeRun, false);
                        // end change by Jim
                    }
                }
                else
                {
                    AddPackCombination(true, packCombo.ComboSelected, packCombo.ComboRID, packCombo.ComboName);

                    foreach (PackPattern pack in packCombo.PackPatternList)
                    {
                        AddPack(true, pack.PackPatternRID, pack.PackMultiple, pack.MaxPatternPacks,
                              pack.PatternIncludesSizeRun, pack.PatternName, pack.SizeRun, false);
                    }
                }
            }

            FormatGrids(grdCandidatePacks);
        }

        //Begin TT#316 - MD - DOConnell - Replace all Windows Combobox controls with new enhanced control
        void cmbEvalOptions_MIDComboBoxPropertiesChangedEvent(object source, MIDComboBoxPropertiesChangedEventArgs args)
        {
            this.cmbEvalOptions_SelectionChangeCommitted(source, new EventArgs());
        }

        void cmbVendor_MIDComboBoxPropertiesChangedEvent(object source, MIDComboBoxPropertiesChangedEventArgs args)
        {
            this.cmbVendor_SelectionChangeCommitted(source, new EventArgs());
        }
        //End TT#316 - MD - DOConnell - Replace all Windows Combobox controls with new enhanced control

        private void Build_lstSizes(ProfileList aSizeCodeList) // TT#615 Size Group, Size Curve and Size Run Issues
        {
            //lstSizes.Clear();  // TT#615 Addendum Size Group, Size Curve and Size Run issues
            lstSizeRIDs.Clear();
            htSizes.Clear();

            foreach (SizeCodeProfile scp in aSizeCodeList)
            {
                //lstSizes.Add(Include.GetSizeName(scp.SizeCodePrimary, scp.SizeCodeSecondary, scp.SizeCodeID));  // TT#615 Addendum Size Group, Size Curve and Size Run Issues
                lstSizeRIDs.Add(scp.Key);
                htSizes.Add(scp.Key, Include.GetSizeName(scp.SizeCodePrimary, scp.SizeCodeSecondary, scp.SizeCodeID));
            }
            lstSizeRIDs.Sort();  // TT#615 Addendum  // NOTE: moved this sort from another location but think it will NOT put sizes in correct order
                                                     //       will address this sort problem later; for now want sizes listed in consistent order.

        }


        bool sg_selectionInitiated = false;
        private void cboSizeGroup_DropDown(object sender, EventArgs e)
        {
            sg_selectionInitiated = true;
            //cboSizeGroup_CurrentValue = cboSizeGroup.Text.Trim();  // TT#615 Issues with Size Group, Size Curve and Size Runs
        }

        bool SizeGroupSelectionMade = false;
        private void cboSizeGroup_DropDownClosed(object sender, EventArgs e)
        {
            if (cboSizeGroup.Text.ToString().Trim() != "")
            {
                SizeGroupSelectionMade = true;
            }
        }

        //string cboSizeGroup_CurrentValue = "";  // TT#615 Issues with Size Group, Size Curve and Size Runs
        int _selectedSizeGroupIndex;
        int _selectedSizeGroupRID;
        // Begin TT#301-MD - JSmith - Controls are not functioning properly
        //private void cboSizeGroup_SelectionChangeCommitted(object sender, EventArgs e)
        override protected void cboSizeGroup_SelectionChangeCommitted(object sender, EventArgs e)
        // End TT#301-MD - JSmith - Controls are not functioning properly
        {
            // Begin TT#4868 - JSmith - select header and an existing method receive mssg
            if (!FormLoaded)
            {
                return;
            }
            // End TT#4868 - JSmith - select header and an existing method receive mssg

            if (cboSizeGroup.SelectedIndex <= 0)
            {
                return;
            }
            //processing flags
            bool headerAttachedOverwrite = false;
            bool sizeCurveOverwrite = false;
            bool messageAnswered = false;

            if (txtBasisHeader.Text.Trim() != "" && headerSizesApplied == true)
            {
                //Begin TT#684 - APicchetti - Build Packs Soft Text

                //if (MessageBox.Show("A header is selected to use with this method.\n" +
                //    "Do you want to override the size data with a size group?", "Size data warning",
                //    MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)

                if (MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_OverrideSizeGroup), "",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                
                //End TT#684 - APicchetti - Build Packs Soft Text

                {
                    headerAttachedOverwrite = true;
                }
                else
                {
                    messageAnswered = true;
                }

            }

            //if (cboSizeCurve.Text.Trim() != "") // TT#615 Issues with Size Group, SIze Curve and SIze Runs
            if (cboSizeCurve.SelectedIndex > 0)   // TT#615 Issues with SIze Group, Size Curve and Size Runs
            {
                //Begin TT#684 - APicchetti - Build Packs Soft Text

                //if (MessageBox.Show("Sizes are selected to use with this method. \n" +
                //    "Do you want to override the size data with a size group?", "Size data warning",
                //    MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)

                if (MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_SelectedOverrideSizeGroup), "",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                //End TT#684 - APicchetti - Build Packs Soft Text

                {
                    // begin TT#615 Issues with Size Group, Size Curve and Size Runs
                    //MIDException aStatusReason;
                    //_BuildPacksMethod.SetSizeCurveGroupRID(Include.NoRID, out aStatusReason);
                    //if (aStatusReason != null)
                    //{
                    //    throw new MIDException(aStatusReason.ErrorLevel, aStatusReason.ErrorNumber,
                    //        MIDText.GetTextOnly(aStatusReason.ErrorNumber), aStatusReason.InnerException);
                    //}
                    //cboSizeCurve.Text = "";
                    // end TT#615 Issues with Size Group, Size Curve and Size Runs
                    sizeCurveOverwrite = true;                    
                }
                else
                {
                    messageAnswered = true;
                }
            }

            bool overwriteSizes = false;
            if (headerAttachedOverwrite == false && sizeCurveOverwrite == false)
            {
                if (messageAnswered == true)
                {
                    overwriteSizes = false;
                }
                else
                {
                    //Begin TT#684 - APicchetti - Build Packs Soft Text

                    //if (MessageBox.Show("Do you want to apply the size information to the pack combinations?",
                    //    "Size data warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)

                    if (MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ApplySizesToPackCombo), "", 
                        MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)

                    //End TT#684 - APicchetti - Build Packs Soft Text

                    {
                        overwriteSizes = true;
                    }
                }
            }

            if (headerAttachedOverwrite == true || sizeCurveOverwrite == true || overwriteSizes == true)
            {
                // begin TT#615 Issues with Size Group, Size Curve and Size Run
                cboSizeCurve.SelectedIndex = 0;
                _selectedSizeCurveIndex = 0;
                _selectedSizeCurveRID = Include.NoRID;
                _selectedSizeGroupIndex = cboSizeGroup.SelectedIndex;
                _selectedSizeGroupRID = Convert.ToInt32(((DataRowView)cboSizeGroup.Items[_selectedSizeGroupIndex])["Key"]);
                SizeGroupProfile sgp = new SizeGroupProfile(_selectedSizeGroupRID);
                SizeCodeList scl = sgp.SizeCodeList;
                Build_lstSizes(scl);
                otherSizesApplied = true;
                if (SizeGroupSelectionMade == true || sg_selectionInitiated == true || sizeCurveOverwrite == true)
                {
                    BuildPackExtendedSzProperties();
                    sg_selectionInitiated = false;
                }
                headerSizesApplied = false;

                //otherSizesApplied = true;
                //if(cboSizeGroup.Text.Trim() == "")
                //{
                //    MIDException aStatus = null;
                //    _BuildPacksMethod.SetSizeGroupRID(Include.NoRID, out aStatus);
                //}
                //if (SizeGroupSelectionMade == true || sg_selectionInitiated == true || sizeCurveOverwrite == true)
                //{
                //    this.grdCandidatePacks.BeginUpdate();
                //    this.grdCandidatePacks.SuspendRowSynchronization();

                //    DataSet dsCandidatePacks = (DataSet)grdCandidatePacks.DataSource;
                //    DataTable dtCandidatePacks = (DataTable)dsCandidatePacks.Tables["PACKS"];
                //    for (int iCol = grdCandidatePacks.DisplayLayout.Bands[1].Columns.Count - 1; iCol > -1; iCol--)
                //    {


                //        if (dtCandidatePacks.Columns[iCol].ExtendedProperties.ContainsValue("Size"))
                //        {
                //            dtCandidatePacks.Columns.RemoveAt(iCol);
                //        }
                //    }

                //    SizeGroup sg = new SizeGroup();
                //    DataTable dtSg = sg.GetSizeGroup(cboSizeGroup.Text.Trim());

                //    if (dtSg.Rows.Count > 0)
                //    {
                //        //initialize the size lists
                //        lstSizeRIDs.Clear();
                //        lstSizes.Clear();
                //        htSizes.Clear();

                //        foreach (DataRow iRow in dtSg.Rows)
                //        {
                //            string sizeName = iRow["SIZE_CODE_PRIMARY"].ToString().Trim();
                //            lstSizes.Add(sizeName);
                //            lstSizeRIDs.Add(Convert.ToInt32(iRow["SIZE_CODE_RID"]));
                //            htSizes.Add(Convert.ToInt32(iRow["SIZE_CODE_RID"]), sizeName);

                //            if (iRow["SIZE_CODE_SECONDARY"].ToString().Trim() != "No Secondary Size" && iRow["SIZE_CODE_SECONDARY"].ToString().Trim() != "None")
                //            {
                //                sizeName += " (" + iRow["SIZE_CODE_SECONDARY"].ToString().Trim() + ")";
                //            }

                //            dtCandidatePacks.Columns.Add(sizeName);
                //            dtCandidatePacks.Columns[dtCandidatePacks.Columns.Count - 1].ExtendedProperties.Add("Type", "Size");

                //            SizeCodeInfo sci = new SizeCodeInfo();

                //        }

                //        sg_selectionInitiated = false;
                //    }

                //    this.grdCandidatePacks.ResumeRowSynchronization();
                //    this.grdCandidatePacks.EndUpdate();

                //    if (sizeCurveOverwrite == true)
                //    {
                //        cboSizeCurve.Text = "";
                //    }

                //    headerSizesApplied = false;
                //}
                // end TT#615 Issues with Size Group, SIze Curve and Size Run

            }
            else
            {
                //cboSizeGroup.Text = cboSizeGroup_CurrentValue;  // TT#615 Issues with Size Group, Size Curve and Size Runs
                cboSizeGroup.SelectedIndex = _selectedSizeGroupIndex; // TT#615 Issues with Size Group, Size Curve and Size Runs
            }
        }

        private void cboSizeGroup_Click(object sender, EventArgs e)
        {
            if (cboSizeGroup.Items.Count == 0)
            {
                //Begin TT#684 - APicchetti - Build Packs Soft Text

                //MessageBox.Show("There are no search results to show.", "Size Group Warning",
                //    MessageBoxButtons.OK, MessageBoxIcon.Warning);

                MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_NoSearchResults), "", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);

                //End TT#684 - APicchetti - Build Packs Soft Text

            }
        }

        bool sc_selectionInitiated = false;
        private void cboSizeCurve_DropDown(object sender, EventArgs e)
        {
            //cboSizeCurve_CurrentValue = cboSizeCurve.Text.Trim(); // TT#615 Issues with Size Group, Size Curve and Size Runs
            sc_selectionInitiated = true;
        }

        bool SizeCurveSelectionMade = false;
        private void cboSizeCurve_DropDownClosed(object sender, EventArgs e)
        {
            if (cboSizeCurve.Text.ToString().Trim() != "")
            {
                SizeCurveSelectionMade = true;
            }
        }

        //string cboSizeCurve_CurrentValue = ""; // TT#615 Issues with SIze Group, SIze Curve and Size Runs
        int _selectedSizeCurveIndex;
        int _selectedSizeCurveRID;
        override protected void cboSizeCurve_SelectionChangeCommitted(object sender, EventArgs e)
        {
            // Begin TT#4868 - JSmith - select header and an existing method receive mssg
            if (!FormLoaded)
            {
                return;
            }
            // End TT#4868 - JSmith - select header and an existing method receive mssg

            if (cboSizeCurve.SelectedIndex <= 0)
            {
                return;
            }
            //processing flags
            bool headerAttachedOverwrite = false;
            bool sizeGroupOverwrite = false;
            bool messageAnswered = false;

            if (txtBasisHeader.Text.Trim() != "" && headerSizesApplied == true)
            {
                //Begin TT#684 - APicchetti - Build Packs Soft Text

                //if (MessageBox.Show("A header is selected to use with this method.\n" +
                //    "Do you want to override the size data with a size curve?", "Size data warning",
                //    MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)

                if (MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_OverrideSizeCurve), "",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)

                //End TT#684 - APicchetti - Build Packs Soft Text

                {
                    headerAttachedOverwrite = true;
                }
                else
                {
                    messageAnswered = true;
                }
            }

            //if (cboSizeGroup.Text.Trim() != "")  // TT#615 Issues with SIze Group, Size Curve and Size Runs
            if (_selectedSizeGroupRID != Include.NoRID)    // TT#615 Issues with Size Group, SIze Curve and Size Runs
            {
                //Begin TT#684 - APicchetti - Build Packs Soft Text

                //if (MessageBox.Show("Sizes are selected to use with this method. \n" +
                //    "Do you want to override the size data with a size curve?", "Size data warning",
                //    MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)

                if (MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_SelectedOverrideSizeCurve), "",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)

                //End TT#684 - APicchetti - Build Packs Soft Text

                {
                    // begin TT#615 Issues with Size Group, Size Curve and Size Runs
                    //MIDException aStatusReason;
                    //_BuildPacksMethod.SetSizeGroupRID(Include.NoRID, out aStatusReason);
                    //if (aStatusReason != null)
                    //{
                    //    throw new MIDException(aStatusReason.ErrorLevel, aStatusReason.ErrorNumber,
                    //        MIDText.GetTextOnly(aStatusReason.ErrorNumber), aStatusReason.InnerException);
                    //}
                    //cboSizeGroup.Text = "";
                    // end TT#615 Issues with Size Group, Size Curve and Size Runs
                    sizeGroupOverwrite = true;
                }
                else
                {
                    messageAnswered = true;
                }
            }

            bool overwriteSizes = false;
            if (headerAttachedOverwrite == false && sizeGroupOverwrite == false)
            {
                if (messageAnswered == true)
                {
                    overwriteSizes = false;
                }
                else
                {
                    //Begin TT#684 - APicchetti - Build Packs Soft Text

                    //if (MessageBox.Show("Do you want to apply the size information to the pack combinations?",
                    //    "Size data warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)

                    if (MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ApplySizesToPackCombo), "", 
                        MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)

                    //End TT#684 - APicchetti - Build Packs Soft Text

                    {
                        overwriteSizes = true;
                    }
                }
            }

            if (headerAttachedOverwrite == true || sizeGroupOverwrite == true || overwriteSizes == true)
            {
                cboSizeGroup.SelectedIndex = 0;
                _selectedSizeGroupIndex = 0;
                _selectedSizeGroupRID = Include.NoRID;
                _selectedSizeCurveIndex = cboSizeCurve.SelectedIndex;
                _selectedSizeCurveRID = Convert.ToInt32(cboSizeCurve.SelectedValue);
                SizeCurveGroupProfile scgp = new SizeCurveGroupProfile(_selectedSizeCurveRID); 
                ProfileList scl = (ProfileList)scgp.SizeCodeList;
                Build_lstSizes(scl);
                otherSizesApplied = true;
                if (SizeCurveSelectionMade == true || sc_selectionInitiated == true || sizeGroupOverwrite == true)
                {
                    BuildPackExtendedSzProperties();
                    sc_selectionInitiated = false;
                }
                headerSizesApplied = false;


                //otherSizesApplied = true;
                //if (cboSizeCurve.Text.Trim() == "")
                //{
                //    MIDException aStatus = null;
                //    _BuildPacksMethod.SetSizeCurveGroupRID(Include.NoRID, out aStatus);
                //}
                //if (SizeCurveSelectionMade == true || sc_selectionInitiated == true || sizeGroupOverwrite == true)
                //{
                //    DataSet dsCandidatePacks = (DataSet)grdCandidatePacks.DataSource;
                //    DataTable dtCandidatePacks = (DataTable)dsCandidatePacks.Tables["PACKS"];
                //    for (int iCol = grdCandidatePacks.DisplayLayout.Bands[1].Columns.Count - 1; iCol > -1; iCol--)
                //    {
                //        this.grdCandidatePacks.BeginUpdate();
                //        this.grdCandidatePacks.SuspendRowSynchronization();

                //        if (dtCandidatePacks.Columns[iCol].ExtendedProperties.ContainsValue("Size"))
                //        {
                //            dtCandidatePacks.Columns.RemoveAt(iCol);
                //        }
                //    }

                //    SizeCurve sc = new SizeCurve();

                //    DataTable dtSc = sc.GetSizesInSizeCurveGroup(sc.GetSizeCurveGroupKey(cboSizeCurve.Text.Trim()));


                //    if (dtSc.Rows.Count > 0)
                //    {
                //        //initialize the size lists
                //        lstSizeRIDs.Clear();
                //        lstSizes.Clear();
                //        htSizes.Clear();

                //        foreach (DataRow iRow in dtSc.Rows)
                //        {
                //            string sizeName = iRow["SIZE_CODE_PRIMARY"].ToString().Trim();
                //            lstSizes.Add(sizeName);
                //            lstSizeRIDs.Add(Convert.ToInt32(iRow["SIZE_CODE_RID"]));
                //            htSizes.Add(Convert.ToInt32(iRow["SIZE_CODE_RID"]), sizeName);

                //            if (iRow["SIZE_CODE_SECONDARY"].ToString().Trim() != "No Secondary Size" && 
                //                iRow["SIZE_CODE_SECONDARY"].ToString().Trim() != "None")
                //            {
                //                sizeName += " (" + iRow["SIZE_CODE_SECONDARY"].ToString().Trim() + ")";
                //            }

                //            dtCandidatePacks.Columns.Add(sizeName);
                //            dtCandidatePacks.Columns[dtCandidatePacks.Columns.Count - 1].ExtendedProperties.Add("Type", "Size");
                //        }

                //        sg_selectionInitiated = false;
                //    }

                //    this.grdCandidatePacks.ResumeRowSynchronization();
                //    this.grdCandidatePacks.EndUpdate();

                //    if (sizeGroupOverwrite == true)
                //    {
                //        cboSizeGroup.Text = "";
                //    }

                //    headerSizesApplied = false;
                //}
            }
            else
            {
                //cboSizeCurve.Text = cboSizeCurve_CurrentValue;  // TT#615 Issues with Size Group, Size Curve and Size Runs
                cboSizeCurve.SelectedIndex = _selectedSizeCurveIndex; // TT#615 Issues with Size Group, Size Curve and Size Runs
            }
        }

        // begin TT#615 Issues with Size Group, Size Curve and Size Runs
        /// <summary>
        /// Builds the Pack extended size properties
        /// </summary>
        private void BuildPackExtendedSzProperties()
        {
            grdCandidatePacks.BeginUpdate();
            grdCandidatePacks.SuspendRowSynchronization();
            if (grdCandidatePacks.DataSource == null)
            {
                CreateDataTables(true);
            }
            DataSet dsCandidatePacks = (DataSet)grdCandidatePacks.DataSource;
            DataTable dtCandidatePacks = (DataTable)dsCandidatePacks.Tables["PACKS"];
            for (int iCol = grdCandidatePacks.DisplayLayout.Bands[1].Columns.Count - 1; iCol > -1; iCol--)
            {
                if (dtCandidatePacks.Columns[iCol].ExtendedProperties.ContainsValue("Size"))
                {
                    dtCandidatePacks.Columns.RemoveAt(iCol);
                }
            }
            //foreach (string sizeName in lstSizes) // TT#615 Addendum
            foreach (int sizeRID in lstSizeRIDs)    // TT#615 Addendum
            {
                dtCandidatePacks.Columns.Add(htSizes[sizeRID]);
                dtCandidatePacks.Columns[dtCandidatePacks.Columns.Count - 1].ExtendedProperties.Add("Type", "Size");
            }
            this.grdCandidatePacks.ResumeRowSynchronization();
            this.grdCandidatePacks.EndUpdate();

        }
        // end TT#615 Issues with Size Group, Size Curve and Size Runs

        private void LoadMethod()
        {
            //criteria tab
            txtName.Text = _BuildPacksMethod.BuildPacksMethodName.Trim();
            txtDesc.Text = _BuildPacksMethod.BuildPacksMethodDescription.Trim();

            // Begin TT#5087 - JSmith - Vendor is blank
            //cmbVendor.Text = _BuildPacksMethod.VendorName.Trim();
            // End TT#5087 - JSmith - Vendor is blank
            txtVendorPackOrderMin.Text = _BuildPacksMethod.PackMinOrder.ToString().Trim(); // TT#787 Vendor Min Order applies only to packs
            txtSizeMultiple.Text = _BuildPacksMethod.SizeMultiple.ToString().Trim();

            // begin TT#615 Addendum
            //// begin TT#615 Size Group, Size Curve and Size Run Issues
            //_selectedSizeGroupRID = _BuildPacksMethod.SizeGroupRID; // TT#615 Size Group, Size Curve and Size Run Issues
            //if (_selectedSizeGroupRID != Include.NoRID)
            //{
            //    SizeGroupProfile sgp = new SizeGroupProfile(_selectedSizeGroupRID);
            //    if (sgp.Key == Include.UndefinedSizeGroupRID)
            //    {
            //        _selectedSizeGroupRID = Include.NoRID;
            //    }
            //    else
            //    {
            //        cboSizeGroup.Text = sgp.SizeGroupName;
            //    }
            //}
            //_selectedSizeGroupIndex = cboSizeGroup.SelectedIndex;

            //_selectedSizeCurveRID = _BuildPacksMethod.SizeCurveGroupRID; // TT#615 Size Group, Size Curve and Size Run Issues
            //if (_selectedSizeCurveRID != Include.NoRID)
            //{
            //    SizeCurveGroupProfile scp = new SizeCurveGroupProfile(_selectedSizeCurveRID);
            //    if (scp.Key == Include.NoRID)
            //    {
            //        _selectedSizeCurveRID = Include.NoRID;
            //    }
            //    else
            //    {
            //        cboSizeCurve.Text = scp.SizeCurveGroupName;
            //    }
            //}
            //_selectedSizeCurveIndex = cboSizeCurve.SelectedIndex; // TT#615 Size Group, Size Curve and Size Run Issues
            //// end TT#615 Size Group, Size Curve and Size Run Issues
            // end TT#615 Addendum

            // begin temp fix
            SizeCodeList scl = _BuildPacksMethod.SizeCodeList;
            if (scl.Count > 0)
            {
                Build_lstSizes(_BuildPacksMethod.SizeCodeList);
                BuildPackExtendedSzProperties();  // TT#615 Size Group, Size Curve and Size Run Issues
            }
            // end temp fix
            // begin TT#747 - JEllis - Cannot modify Custom Pack Combinations
            //List<PackPatternCombo> lstPackCombos = _BuildPacksMethod.PackCombination;

            //foreach (PackPatternCombo packCombo in lstPackCombos)
            //{
            //    if (packCombo.PackPatternType == ePackPatternType.Vendor)
            //    {
            //        AddPackCombination(false, packCombo.ComboSelected, packCombo.ComboRID, packCombo.ComboName);

            //        foreach (PackPattern pack in packCombo.PackPatternList)
            //        {
            //            // begin change by Jim
            //            //AddPack(false, packCombo.ComboRID, pack.PackPatternRID, pack.PackMultiple, pack.MaxPatternPacks,
            //            //    pack.PatternIncludesSizeRun, pack.PatternName, pack.SizeRun, false);
            //            AddPack(false, pack.PackPatternRID, pack.PackMultiple, pack.MaxPatternPacks,
            //                pack.PatternIncludesSizeRun, pack.PatternName, pack.SizeRun, false);
            //            // end change by Jim
            //        }
            //    }
            //    else
            //    {
            //        AddPackCombination(true, packCombo.ComboSelected, packCombo.ComboRID, packCombo.ComboName);

            //        foreach (PackPattern pack in packCombo.PackPatternList)
            //        {
            //            // begin change by Jim
            //            //AddPack(true, packCombo.ComboRID, pack.PackPatternRID, pack.PackMultiple, pack.MaxPatternPacks,
            //            //   pack.PatternIncludesSizeRun, pack.PatternName, pack.SizeRun, false);
            //            AddPack(true, pack.PackPatternRID, pack.PackMultiple, pack.MaxPatternPacks,
            //               pack.PatternIncludesSizeRun, pack.PatternName, pack.SizeRun, false);
            //            // end change by Jim

            //        }
            //    }
            //}
            LoadPackCombinations();
            // end TT#747 - JEllis - Cannot modify Custom Pack Combinations

            // begin TT#615 Size Group, Size Curve and Size Run Issues
            //SizeGroupProfile sgp = new SizeGroupProfile(_BuildPacksMethod.SizeGroupRID);

            //_selectedSizeGroupRID = Include.NoRID; // TT#615 Size Group, Size Curve and Size Run Issues
            //if (sgp != null)
            //{
            //    cboSizeGroup.Text = sgp.SizeGroupName;
            //    _selectedSizeGroupRID = sgp.Key;  // TT#615 Size Group, Size Curve and Size Run issues
            //}
            //_selectedSizeGroupIndex = cboSizeGroup.SelectedIndex;

            //SizeCurveGroupProfile scp = new SizeCurveGroupProfile(_BuildPacksMethod.SizeCurveGroupRID);

            //_selectedSizeCurveRID = Include.NoRID; // TT#615 Size Group, Size Curve and Size Run Issues
            //if (sgp != null)
            //{
            //    cboSizeCurve.Text = scp.SizeCurveGroupName;
            //    _selectedSizeCurveRID = Include.NoRID;  // TT#615 Size Group, Size Curve and Size Run Issues
            //}
            //_selectedSizeCurveIndex = cboSizeCurve.SelectedIndex; // TT#615 Size Group, Size Curve and Size Run Issues
            // end TT#615 Size Group, Size Curve and Size Run Issues

            FormatGrids(grdCandidatePacks);

            //constraints tab
            txtReserve.Text = _BuildPacksMethod.ReserveTotal.ToString().Trim();
            if (_BuildPacksMethod.ReserveTotalIsPercent == true)
            {
                rdoReservePercent.Checked = true;
                rdoReserveUnits.Checked = false;
            }
            else
            {
                rdoReservePercent.Checked = false;
                rdoReserveUnits.Checked = true;
            }
            txtReserveBulk.Text = _BuildPacksMethod.ReserveBulk.ToString().Trim();
            if (_BuildPacksMethod.ReserveBulkIsPercent == true)
            {
                rdoReserveBulkPercent.Checked = true;
                rdoReserveBulkUnits.Checked = false;
            }
            else
            {
                rdoReserveBulkPercent.Checked = false;
                rdoReserveBulkUnits.Checked = true;
            }
            txtReservePacks.Text = _BuildPacksMethod.ReservePacks.ToString().Trim();
            if (_BuildPacksMethod.ReservePacksIsPercent == true)
            {
                rdoReservePacksPercent.Checked = true;
                rdoReservePacksUnits.Checked = false;
            }
            else
            {
                rdoReservePacksPercent.Checked = false;
                rdoReservePacksUnits.Checked = true;
            }
            // begin TT#744 - JEllis - Use Orig Pack Fitting Logic; Remove Bulk
            if (_BuildPacksMethod.RemoveBulkAfterFittingPacks == true)
            {
                chkBoxRemoveBulk.Checked = true;
            }
            else
            {
                chkBoxRemoveBulk.Checked = false;
            }
            // end TT#744 - JEllis - Use Orig Pack Fitting Logic; Remove Bulk
            // BEGIN TT#669 - AGallagher – Build Pack Method – Variance Options
            if (_BuildPacksMethod.DepleteReserveSelected == true)
            {
                chkDepleteReserve.Checked = true;
            }
            else
            {
                chkDepleteReserve.Checked = false;
            }
            if (_BuildPacksMethod.IncreaseBuySelected == true)
            {
                chkIncreaseBuyQty.Checked = true;
                this.txtIncreaseBuyQty.Enabled = true;
            }
            else
            {
                chkIncreaseBuyQty.Checked = false;
                this.txtIncreaseBuyQty.Enabled = false;
            }
            if (_BuildPacksMethod.IncreaseBuyPct != double.MaxValue)
                { txtIncreaseBuyQty.Text = _BuildPacksMethod.IncreaseBuyPct.ToString().Trim(); }
            // END TT#669 - AGallagher – Build Pack Method – Variance Options
            // begin Change by Jim
            //txtAvgPackDeviationTolerance.Text = _BuildPacksMethod.AvgPackErrorDevTolerance.ToString().Trim();
            if (_BuildPacksMethod.AvgPackErrorDevTolerance < Include.DefaultMaxSizeErrorPercent)
            {
                txtAvgPackDeviationTolerance.Text = _BuildPacksMethod.AvgPackErrorDevTolerance.ToString().Trim();
            }
            else
            {
                txtAvgPackDeviationTolerance.Text = string.Empty;
            }
            // end Change by Jim
            txtMaxPackAllocationNeedTolerance.Text = _BuildPacksMethod.ShipVariance.ToString().Trim();

            txtSizeMultiple.ReadOnly = true;
            txtVendorPackOrderMin.ReadOnly = true; // TT#787 Vendor Min Order applies only to packs
            //btnProcess.Enabled = false;
            btnApply.Enabled = false;

            //Begin TT#542 - JSmith - Build Packs -  When double clicking on the method that was saved, to re-open receive a System Argument Exception
            //BindSizeGroupComboBox(true, Include.NoRID);
            //BindSizeCurveComboBox(true, Include.NoRID);
            //End TT#542
        }

        //SelectedHeaderProfile InteractiveHeader = null;
        bool headerSizesApplied = false;
        bool otherSizesApplied = false;
        private void btnGetHeader_Click(object sender, System.EventArgs e)
        {
            try
            {
                SelectedHeaderList selectedHeaderList = (SelectedHeaderList)SAB.ClientServerSession.GetSelectedHeaderList();
                if (selectedHeaderList.Count == 0)
                {
                    MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_NoHeaderSelectedOnWorkspace));
                    return;
                }
                if (selectedHeaderList.Count > 1)
                {
                    MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MultHeadersSelectedOnWorkspace));
                    return;
                }
                if (selectedHeaderList.Count == 1)
                {
                    //Begin TT#684 - APicchetti - Build Packs Soft Text

                    //DialogResult drMsgBox = MessageBox.Show("Apply Sizes from Header?",
                    //    "Header change warning", MessageBoxButtons.YesNoCancel,
                    //    MessageBoxIcon.Question);

                    DialogResult drMsgBox = MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ApplyHeaderSizes),
                        "", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                    //End TT#684 - APicchetti - Build Packs Soft Text

                    if (drMsgBox == DialogResult.Yes)
                    {
                        headerSizesApplied = true;

                        // begin TT#615 Size Group, Size Curve and Size Run Issues
                        //this.grdCandidatePacks.BeginUpdate();
                        //this.grdCandidatePacks.SuspendRowSynchronization();

                        //if (grdCandidatePacks.DataSource == null)
                        //{
                        //    CreateDataTables(true);
                        //}

                        //DataSet dsCandidatePacks = (DataSet)grdCandidatePacks.DataSource;
                        //DataTable dtCandidatePacks = (DataTable)dsCandidatePacks.Tables["PACKS"];
                        //for (int iCol = grdCandidatePacks.DisplayLayout.Bands[1].Columns.Count - 1; iCol > -1; iCol--)
                        //{
                        //    if (dtCandidatePacks.Columns[iCol].ExtendedProperties.ContainsValue("Size"))
                        //    {
                        //        dtCandidatePacks.Columns.RemoveAt(iCol);
                        //    }
                        //}
                        // end TT#615 Size Group, Size Curve, and Size Run issues

                        SelectedHeaderProfile shp = (SelectedHeaderProfile)selectedHeaderList[0];
                        txtBasisHeader.Text = shp.HeaderID;
                        int _headerRID = shp.Key;

                        Profile prf = selectedHeaderList[0];
                        int prfKey = prf.Key;

                        AllocationHeaderProfile ahp = new AllocationHeaderProfile(prfKey);

                        AllocationProfile alHeader = new AllocationProfile(SAB, null, ahp.Key, SAB.ApplicationServerSession);

                        // begin Temp List
                        //SizeGroupProfile sgp = new SizeGroupProfile(alHeader.SizeGroupRID);
                        //
                        //SizeCodeList scl = sgp.SizeCodeList;
                        SizeCodeList scl = alHeader.GetSizeCodeList();
                        // end Temp List


                        if (scl.Count > 0)
                        {
                            //build size lists
                            //lstSizes = new List<string>(); // TT#615 Addendum
                            Build_lstSizes(scl);

                            // begin TT#615 Size Group, Size Curve and Size Run Issues
                            BuildPackExtendedSzProperties();
                            //foreach (string sizeName in lstSizes)
                            //{
                            //    dtCandidatePacks.Columns.Add(sizeName);
                            //    dtCandidatePacks.Columns[dtCandidatePacks.Columns.Count - 1].ExtendedProperties.Add("Type", "Size");
                            //}
                            // end TT#615 Size Group, Size Curve and Size Run Issues

                            sg_selectionInitiated = false;
                        }

                        // begin TT#615 Size Curve, Size Group and Size Run Issues
                        //this.grdCandidatePacks.ResumeRowSynchronization();
                        //this.grdCandidatePacks.EndUpdate();

                        cboSizeGroup.SelectedIndex = 0;
                        _selectedSizeGroupIndex = 0;
                        _selectedSizeGroupRID = Include.NoRID;
                        cboSizeCurve.SelectedIndex = 0;
                        _selectedSizeCurveIndex = 0;
                        _selectedSizeCurveRID = Include.NoRID;

                        //cboSizeCurve.Text = "";
                        //MIDException aStatusReason;
                        //_BuildPacksMethod.SetSizeCurveGroupRID(Include.NoRID, out aStatusReason);
                        //if(aStatusReason != null)
                        //{
                        //    throw new MIDException(aStatusReason.ErrorLevel, aStatusReason.ErrorNumber,
                        //        MIDText.GetTextOnly(aStatusReason.ErrorNumber), aStatusReason.InnerException);
                        //}

                        //cboSizeGroup.Text = "";
                        //_BuildPacksMethod.SetSizeGroupRID(Include.NoRID, out aStatusReason);
                        //if (aStatusReason != null)
                        //{
                        //    throw new MIDException(aStatusReason.ErrorLevel, aStatusReason.ErrorNumber,
                        //        MIDText.GetTextOnly(aStatusReason.ErrorNumber), aStatusReason.InnerException);
                        //}
                        // end TT#615 Size Group, Size Curve, and Size Run issues
                    }
                    else if (drMsgBox == DialogResult.No)
                    {
                        SelectedHeaderProfile shp = (SelectedHeaderProfile)selectedHeaderList[0];
                        txtBasisHeader.Text = shp.HeaderID;

                        if (dsPackCombo == null)
                        {
                            CreateDataTables(false);
                        }
                    }
                }

                //InteractiveHeader = (SelectedHeaderProfile) selectedHeaderList[0];
                //if (FunctionSecurity.AllowExecute == true)
                //{
                //    btnProcess.Enabled = true;
                //}

                FormatGrids(grdCandidatePacks);

            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void cboSizeCurve_Click(object sender, EventArgs e)
        {
            if (cboSizeCurve.Items.Count == 0)
            {
                //Begin TT#684 - APicchetti - Build Packs Soft Text

                //MessageBox.Show("There are no search results to show.", "Size Group Warning",
                //    MessageBoxButtons.OK, MessageBoxIcon.Warning);

                MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_NoSearchResults), "",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);

                //End TT#684 - APicchetti - Build Packs Soft Text
            }
        }

        private void SetText()
        {
            try
            {
                // BEGIN TT#3830 - AGallagher - Size Curve Method label has double colons
                //WorkflowMethodNameLabel = "Build Packs Name:";
                WorkflowMethodNameLabel = "Method";
                // END TT#3830 - AGallagher - Size Curve Method label has double colons
                this.tabProperties.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Properties);
                this.tabBuildPacks.Text = MIDText.GetTextOnly(eMIDTextCode.tab_BuildPacks);
                this.tabCriteria.Text = MIDText.GetTextOnly(eMIDTextCode.tab_Criteria);
                this.tabConstraints.Text = MIDText.GetTextOnly(eMIDTextCode.tab_Constraints);
                this.tabBPSummary.Text = MIDText.GetTextOnly(eMIDTextCode.tab_Summary);
                this.tabEvaluation.Text = MIDText.GetTextOnly(eMIDTextCode.tab_Evaluation);
                this.grpCriteria.Text = MIDText.GetTextOnly(eMIDTextCode.grp_Criteria);
                this.lblHeader.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_BuildPackHeader);
                this.btnGetHeader.Text = MIDText.GetTextOnly(eMIDTextCode.btn_GetHeader);
                this.lblVendorPackOrderMin.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_VendorPackOrderMin); // TT#787 Vendor Min Order applies only to packs
                this.lblVendor.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_BuildPackVendor);
                this.lblSizeMultiple.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_BuildPackSizeMultiple);
                this.gbSizeGroup.Text = MIDText.GetTextOnly(eMIDTextCode.gb_SizeGroup);
                this.gbSizeCurve.Text = MIDText.GetTextOnly(eMIDTextCode.gb_SizeCurve);
                this.lblCandidatePacks.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_CandidatePacks);
                this.lblPercentToReserve.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_PercentToReserve);
                this.lblPercentToReserveBulk.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_PercentToReserveBulk);
                this.lblPercentToReservePacks.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_PercentToReservePacks);
                this.grpPackErrorOptions.Text = MIDText.GetTextOnly(eMIDTextCode.grp_PackErrorOptions);
                this.lblAvgPackDeviationTolerance.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_AvgPackDeviationTolerance);
                this.lblMaxPackAllocationNeedTolerance.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_MaxPackAllocationNeedTolerance);
                this.lblEvaluationPackOptions.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_EvaluationPackOptions);
                //BEGIN TT#669 - AGallagher – Build Pack Method – Variance Options
                this.gbxVarianceOptions.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_VarianceOptions);
                this.chkDepleteReserve.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_DepleteReserve);
                this.chkIncreaseBuyQty.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_IncreaseBuyQty);
                this.lblPercent.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_PercentSign);
                //END TT#669 - AGallagher – Build Pack Method – Variance Options
                this.chkBoxRemoveBulk.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_BP_RemoveBulkFromHeader); // TT#744 - JEllis - Use Orig Pack Fitting Logic; Remove Bulk from Header
                this.btnApply.Text = MIDText.GetTextOnly(eMIDTextCode.btn_Apply);
                this.mnuPackCombinationAdd.Text = MIDText.GetTextOnly(eMIDTextCode.mnu_PackCombinationAdd);
                this.mnuPackCombinationAddPack.Text = MIDText.GetTextOnly(eMIDTextCode.mnu_PackCombinationAddPack);
                this.mnuPackCombinationDelete.Text = MIDText.GetTextOnly(eMIDTextCode.mnu_PackCombinationDelete);
                this.rdoReserveBulkPercent.Text = MIDText.GetTextOnly(eMIDTextCode.rdo_ReserveBulkPercent);
                this.rdoReserveBulkUnits.Text = MIDText.GetTextOnly(eMIDTextCode.rdo_ReserveBulkUnits);
                this.rdoReservePacksPercent.Text = MIDText.GetTextOnly(eMIDTextCode.rdo_ReservePacksPercent);
                this.rdoReservePacksUnits.Text = MIDText.GetTextOnly(eMIDTextCode.rdo_ReservePacksUnits);
                this.rdoReservePercent.Text = MIDText.GetTextOnly(eMIDTextCode.rdo_ReservePercent);
                this.rdoReserveUnits.Text = MIDText.GetTextOnly(eMIDTextCode.rdo_ReserveUnits);
                this.tabMethod.Text = MIDText.GetTextOnly(eMIDTextCode.tab_Method);
                this.PropertyNames.HeaderText = MIDText.GetTextOnly(eMIDTextCode.Property_Names);

            }
            catch (Exception ex)
            {
                string message = ex.ToString();
                throw;
            }
        }

        protected override void Call_btnProcess_Click()
        {
            try
            {
                eAllocationActionStatus actionStatus = new eAllocationActionStatus();

                // Begin TT#696-MD - stodd - needed no selection msg
                //====================================================
                // Checks to be sure there are valid selected headers
                //====================================================
                if (!OkToProcess(this, eMethodType.BuildPacks))
                {
                    return;
                }
                // End TT#696-MD - stodd - msg

                if (this.tabBuildPacks.Controls.Contains(this.tabBPSummary) == true)
                {
                    hEvalOptionRIDs = new Hashtable();
                    grdBPSummary.Rows.Clear();
                    grdBPSummary.Columns.Clear();
                    this.tabBuildPacks.Controls.Remove(this.tabBPSummary);
                }

                if (this.tabBuildPacks.Controls.Contains(this.tabEvaluation) == true)
                {
                    cmbEvalOptions.Items.Clear();
                    this.tabBuildPacks.Controls.Remove(this.tabEvaluation);
                }

                if (FieldValidations() == true)
                {
                    // begin changes by Jim
                    if (_BuildPacksMethod.IsInteractive)
                    {
                        try
                        {
                            this.Save_Click(false);
                            if (!ErrorFound)
                            {
                                //SAB.ClientServerSession.ClearSelectedHeaderList();
								// Begin TT#2 - stodd - assortment
                                //SAB.ClientServerSession.AddSelectedHeaderList(InteractiveHeader.Key, InteractiveHeader.HeaderID, eHeaderType.WorkupTotalBuy);
								// End TT#2 - stodd - assortment
                                _BuildPacksMethod.Method_Change_Type = eChangeType.update;
                                btnSave.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_Update);
                                //ApplicationTransaction.BuildPacks = _BuildPacksMethod; //TT#795 -  MD - DOConnell - Build Packs not working on a Placeholder in an assortment.
                                // Begin TT#289-MD - JSmith - Build Pack method-> processes but fails to apply the pack to the header
                                string enqMessage;
								//BEGIN TT#795 -  MD - DOConnell - Build Packs not working on a Placeholder in an assortment.
                                if (AssortmentActiveProcessToolbarHelper.ActiveProcess.screenID != Include.NoRID)	// Use assortment
                                {
                                    ApplicationSessionTransaction assortTrans = SAB.AssortmentTransactionEvent.GetAssortmentTransaction(this, AssortmentActiveProcessToolbarHelper.ActiveProcess.screenID);

                                    assortTrans.BuildPacks = _BuildPacksMethod;
                                    // End TT#289-MD - JSmith - Build Pack method-> processes but fails to apply the pack to the header
                                    assortTrans.ProcessInteractiveBuildPacks();
                                    actionStatus = assortTrans.AllocationActionAllHeaderStatus;
                                    if (actionStatus == eAllocationActionStatus.ActionCompletedSuccessfully)
                                    {
                                        List<OptionPackProfile> lstOPP = _BuildPacksMethod.OptionPackBestToLeastOrder;
                                        DisplaySummaryOptions(lstOPP);

                                        this.tabBuildPacks.Controls.Add(this.tabBPSummary);
                                        this.tabBuildPacks.Controls.Add(this.tabEvaluation);
                                        tabBuildPacks.SelectedTab = tabBPSummary;
                                        cmbEvalOptions.SelectedIndex = 0;
                                    }
                                    else
                                    {
                                        ErrorFound = true;
                                        string message = MIDText.GetTextOnly((int)actionStatus);
                                        MessageBox.Show(message, Name, MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        // begin TT#241 - MD - JEllis - Header Enqueue Process
                                        if (actionStatus == eAllocationActionStatus.HeaderEnqueueFailed
                                            || actionStatus == eAllocationActionStatus.NoHeaderResourceLocks)
                                        {
                                            ErrorFound = true;
                                            Close();
                                        }
                                        // end TT#241 - MD - JEllis - Header Enqueue Process
                                        // Begin TT#289-MD - JSmith - Build Pack method-> processes but fails to apply the pack to the header
                                    }
                                }
                                else
                                {
                                    ApplicationTransaction.BuildPacks = _BuildPacksMethod;
                                    if (ApplicationTransaction.EnqueueSelectedHeaders(out enqMessage))
                                    {
                                        // End TT#289-MD - JSmith - Build Pack method-> processes but fails to apply the pack to the header
										// Begin TT#696-MD - Build Packs Method runs against wrong selected header list - 
										// Moved to here from CreateMasterAllocationProfileListFromSelectedHeaders()
										// Do not want to do this for the call about when it's against an assortment.
                                        ApplicationTransaction.CreateMasterAllocationProfileListFromSelectedHeaders();
										// End TT#696-MD - Build Packs Method runs against wrong selected header list - 
                                        ApplicationTransaction.ProcessInteractiveBuildPacks();
                                        actionStatus = ApplicationTransaction.AllocationActionAllHeaderStatus;
                                        if (actionStatus == eAllocationActionStatus.ActionCompletedSuccessfully)
                                        {
                                            List<OptionPackProfile> lstOPP = _BuildPacksMethod.OptionPackBestToLeastOrder;
                                            DisplaySummaryOptions(lstOPP);

                                            this.tabBuildPacks.Controls.Add(this.tabBPSummary);
                                            this.tabBuildPacks.Controls.Add(this.tabEvaluation);
                                            tabBuildPacks.SelectedTab = tabBPSummary;
                                            cmbEvalOptions.SelectedIndex = 0;
                                        }
                                        else
                                        {
                                            ErrorFound = true;
                                            string message = MIDText.GetTextOnly((int)actionStatus);
                                            MessageBox.Show(message, Name, MessageBoxButtons.OK, MessageBoxIcon.Information);
                                            // begin TT#241 - MD - JEllis - Header Enqueue Process
                                            if (actionStatus == eAllocationActionStatus.HeaderEnqueueFailed
                                                || actionStatus == eAllocationActionStatus.NoHeaderResourceLocks)
                                            {
                                                ErrorFound = true;
                                                Close();
                                            }
                                            // end TT#241 - MD - JEllis - Header Enqueue Process
                                            // Begin TT#289-MD - JSmith - Build Pack method-> processes but fails to apply the pack to the header
                                        }
                                    }
                                    else
                                    {
                                        ApplicationTransaction.SAB.MessageCallback.HandleMessage(
                                        enqMessage,
                                        "Header Lock Conflict",
                                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Asterisk);
                                    }
                                    // End TT#289-MD - JSmith - Build Pack method-> processes but fails to apply the pack to the header
                                }
								//END TT#795 -  MD - DOConnell - Build Packs not working on a Placeholder in an assortment.
                            }
                        }
                        catch (Exception exception)
                        {
                            HandleException(exception);
                        }
                    }
                    else
                    {
                        //=======================================
                        // Below is the usual way to process...
                        //=======================================

                        ProcessAction(eMethodType.BuildPacks);

                        if (!ErrorFound)
                        {
                            _BuildPacksMethod.Method_Change_Type = eChangeType.update;
                            btnSave.Text = "&Update";
                        }
                    }
                }
                //List<OptionPackProfile> lstOPP = _BuildPacksMethod.OptionPackBestToLeastOrder;
                // end changes by Jim

                // Begin TT#224 MD - JSmith - Index out of range error
                //if (actionStatus != eAllocationActionStatus.ActionFailed && actionStatus != 0)
                if (actionStatus == eAllocationActionStatus.ActionCompletedSuccessfully)
                // End TT#224 MD
                {
                    grdResults.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                    grdBPSummary.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        Hashtable hEvalOptionRIDs = new Hashtable();
        string strRowOmissionList = "OptionPackKey;OptionPackID;FromPackPatternComboRID;Key;Version;Maximum Pack Need Tolerance;";

        private void DisplaySummaryOptions(List<OptionPackProfile> oppl)
        {
            try
            {
                OptionPackProfile opp_name = (OptionPackProfile)oppl[0];
                PropertyDescriptorCollection pdc_name = TypeDescriptor.GetProperties(opp_name);


                grdBPSummary.Columns.Add("PropertyNames", "Property Names");

                //add blank rows to the grid so they can be filled-in in the correct sort order
                foreach (PropertyDescriptor pd in pdc_name)
                {
                    string strPropName = SetPropertyNameText(pd.Name);

                    char[] delim = ";".ToCharArray();

                    string[] fieldsToOmit = strRowOmissionList.Split(delim);

                    bool omitField = false;

                    foreach (string fieldToOmit in fieldsToOmit)
                    {
                        if (strPropName == fieldToOmit)
                        {
                            omitField = true;
                            break;
                        }
                    }

                    if (omitField == false)
                    {
                        grdBPSummary.Rows.Add();
                    }
                }

                int property_cnt = 0;
                int evalPropCnt = 0;
                foreach (PropertyDescriptor pd in pdc_name)
                {
                    string strPropName = SetPropertyNameText(pd.Name);

                    char[] delim = ";".ToCharArray();

                    string[] fieldsToOmit = strRowOmissionList.Split(delim);

                    bool omitField = false;

                    foreach (string fieldToOmit in fieldsToOmit)
                    {
                        if (strPropName == fieldToOmit)
                        {
                            omitField = true;
                            break;
                        }
                    }

                    if (omitField == false)
                    {
                        
                        switch (pd.Name)
                        {
                            case "AllStoreTotalNumberOfPacks":
                            case "AllStoreTotalPackUnits":
                            case "AverageErrorPerSizeWithUnits":
                            case "AverageErrorPerStoreWithPacks":
                            case "CountOfAllStoresWithPacks":
                            case "CountOfNonReserveStoresWithPacks":
                            case "NonReserveTotalNumberOfPacks":
                            case "NonReserveTotalPackUnits":
                            case "ReserveTotalNumberOfPacks":
                            case "ReserveTotalPackUnits":
                                grdResults.Rows.Add();
                                grdResults.Rows[evalPropCnt].Cells[0].Value = strPropName;
                                //grdBPSummary.Rows.Add();
                                grdBPSummary.Rows[GetSummary_EvalSortNbr(strPropName)].Cells[0].Value = strPropName;
                                evalPropCnt++;
                                break;
                            default:
                                //grdBPSummary.Rows.Add();
                                grdBPSummary.Rows[GetSummary_EvalSortNbr(strPropName)].Cells[0].Value = strPropName;
                                break;

                        }
                        
                        property_cnt++;
                    }
                }


                for (int intPackCol = 1; intPackCol <= oppl.Count; intPackCol++)
                {
                    OptionPackProfile opp = (OptionPackProfile)oppl[intPackCol -1];

                    grdBPSummary.Columns.Add(opp.OptionPackID.ToString().Trim(), opp.OptionPackID.ToString().Trim());
                    grdBPSummary.Columns[intPackCol].SortMode = DataGridViewColumnSortMode.NotSortable;
                    cmbEvalOptions.Items.Add("Option: " + opp.OptionPackID + "; " +
                        "Ship Variance: " + opp.ShipVariance + "; " +
                        "Percent Of Units In Pack: " + Math.Round(opp.PercentAllStoreUnitsInPacks, 3).ToString().Trim());
                    hEvalOptionRIDs.Add(opp.OptionPackID, opp.OptionPackKey);

                    PropertyDescriptorCollection pdc = TypeDescriptor.GetProperties(opp);

                    //property_cnt = 0;
                    foreach (PropertyDescriptor pd in pdc)
                    {
                        string tc = pd.Converter.ToString().Trim();

                        string strPropName = SetPropertyNameText(pd.Name);

                        System.Diagnostics.Debug.Print(strPropName);

                        char[] delim = ";".ToCharArray();

                        string[] fieldsToOmit = strRowOmissionList.Split(delim);

                        bool omitField = false;

                        switch (tc)
                        {
                            case "System.ComponentModel.DoubleConverter":

                                foreach (string fieldToOmit in fieldsToOmit)
                                {
                                    if (strPropName == fieldToOmit)
                                    {
                                        omitField = true;
                                        break;
                                    }
                                }

                                if (omitField == false)
                                {
									// Begin TT#585 - stodd - data not matching row desc
                                    if (strPropName == "Average Pack Deviation Tolerance" && Convert.ToDouble(pd.GetValue(opp)) == Double.MaxValue)
                                    {
                                        // begin TT#689 Pack Coverage too small (unrelated issue: Avg Pack Dev shows as "0" on TAB 3 when it should be blank)
                                        grdBPSummary.Rows[GetSummary_EvalSortNbr(strPropName)].Cells[intPackCol].Value = "";
                                        //grdBPSummary.Rows[GetSummary_EvalSortNbr(strPropName)].Cells[intPackCol].Value = "0";
                                        // end TT#689 Pack Coverage too small (unrelated issue: Avg Pack Dev shows as "0" on TAB 3 when it should be blank)
                                    }
                                    else if (strPropName == "Maximum Pack Need Tolerance" && Convert.ToDouble(pd.GetValue(opp)) == Double.MaxValue)
                                    {
										grdBPSummary.Rows[GetSummary_EvalSortNbr(strPropName)].Cells[intPackCol].Value = "";
                                    }
                                    else
                                    {
                                        double dValue = Math.Round(Convert.ToDouble(pd.GetValue(opp)),3);
                                        string strValue = dValue.ToString("0.000").Trim();
										grdBPSummary.Rows[GetSummary_EvalSortNbr(strPropName)].Cells[intPackCol].Value = strValue;
                                    }
                                    //property_cnt++;
									// End TT#585 - Build Pack - variable order for tab 3 = Summary Tab
                                }
                                break;

                            case "System.ComponentModel.StringConverter":
                            case "System.ComponentModel.Int16Converter":
                            case "System.ComponentModel.Int32Converter":
                            case "System.ComponentModel.Int64Converter":
                            case "System.ComponentModel.BooleanConverter":
                            case "System.ComponentModel.ByteConverter":
                            case "System.ComponentModel.CharConverter":
                            case "System.ComponentModel.DateTimeConverter":
                            case "System.ComponentModel.DateTimeOffsetConverter":
                            case "System.ComponentModel.DecimalConverter":
                            case "System.ComponentModel.MultilineStringConverter":
                            case "System.ComponentModel.NullableConverter":
                            case "System.ComponentModel.SByteConverter":
                            case "System.ComponentModel.SingleConverter":
                            case "System.ComponentModel.TimeSpanConverter":
                            case "System.ComponentModel.UInt16Converter":
                            case "System.ComponentModel.UInt32Converter":
                            case "System.ComponentModel.UInt64Converter":

                                foreach (string fieldToOmit in fieldsToOmit)
                                {
                                    if (strPropName == fieldToOmit)
                                    {
                                        omitField = true;
                                        break;
                                    }
                                }

								property_cnt = 0;
                                if (omitField == false)
                                {
									// Begin TT#585 - stodd - data not matching row desc
                                    if (strPropName == "Average Pack Deviation Tolerance" && Convert.ToDouble(pd.GetValue(opp)) == Double.MaxValue)
                                    {
										grdBPSummary.Rows[GetSummary_EvalSortNbr(strPropName)].Cells[intPackCol].Value = "0";
                                    }
                                    else if (strPropName == "Maximum Pack Need Tolerance" && Convert.ToDouble(pd.GetValue(opp)) == Double.MaxValue)
                                    {
										grdBPSummary.Rows[GetSummary_EvalSortNbr(strPropName)].Cells[intPackCol].Value = "";
                                    }
                                    else
                                    {
                                        string strValue = pd.GetValue(opp).ToString().Trim();
										grdBPSummary.Rows[GetSummary_EvalSortNbr(strPropName)].Cells[intPackCol].Value = strValue;
                                    }
                                    //property_cnt++;
									// Begin TT#585 - stodd - data not matching row desc
                                }

                                break;
                        }

                    }

                }


                if (FunctionSecurity.AllowExecute == true)
                {
                    btnApply.Enabled = true;
                }
            }
            catch (Exception err)
            {
                HandleException(err);
            }
        }

        private string SetPropertyNameText(string PropertyName)
        {
            string strReturn = "";
            switch (PropertyName)
            {
                case "FromPackPatternComboName":
                    strReturn = MIDText.GetTextOnly(eMIDTextCode.pnFromPackPatternComboName);
                    break;
                case "ShipVariance":
                    strReturn = MIDText.GetTextOnly(eMIDTextCode.pnShipVariance);
                    break;
                case "MaxPackNeedTolerance":
                    strReturn = MIDText.GetTextOnly(eMIDTextCode.pnMaxPackNeedTolerance);
                    break;
                case "AvgPackDevTolerance":
                    strReturn = MIDText.GetTextOnly(eMIDTextCode.pnAvgPackDevTolerance);
                    break;
                case "AllStoreTotalBuy":
                    strReturn = MIDText.GetTextOnly(eMIDTextCode.pnAllStoreTotalBuy);
                    break;
                case "AllStoreBulkBuy":
                    strReturn = MIDText.GetTextOnly(eMIDTextCode.pnAllStoreBulkBuy);
                    break;
                case "NonReserveTotalBuy":
                    strReturn = MIDText.GetTextOnly(eMIDTextCode.pnNonReserveTotalBuy);
                    break;
                case "NonReserveBulkBuy":
                    strReturn = MIDText.GetTextOnly(eMIDTextCode.pnNonReserveBulkBuy);
                    break;
                case "ReserveTotalBuy":
                    strReturn = MIDText.GetTextOnly(eMIDTextCode.pnReserveTotalBuy);
                    break;
                case "ReserveBulkBuy":
                    strReturn = MIDText.GetTextOnly(eMIDTextCode.pnReserveBulkBuy);
                    break;
                case "AllStoreTotalPackUnits":
                    strReturn = MIDText.GetTextOnly(eMIDTextCode.pnAllStoreTotalPackUnits);
                    break;
                case "NonReserveTotalPackUnits":
                    strReturn = MIDText.GetTextOnly(eMIDTextCode.pnNonReserveTotalPackUnits);
                    break;
                case "ReserveTotalPackUnits":
                    strReturn = MIDText.GetTextOnly(eMIDTextCode.pnReserveTotalPackUnits);
                    break;
                case "AllStoreTotalNumberOfPacks":
                    strReturn = MIDText.GetTextOnly(eMIDTextCode.pnAllStoreTotalNumberOfPacks);
                    break;
                case "NonReserveTotalNumberOfPacks":
                    strReturn = MIDText.GetTextOnly(eMIDTextCode.pnNonReserveTotalNumberOfPacks);
                    break;
                case "ReserveTotalNumberOfPacks":
                    strReturn = MIDText.GetTextOnly(eMIDTextCode.pnReserveTotalNumberOfPacks);
                    break;
                case "CountSizesWithUnits":
                    strReturn = MIDText.GetTextOnly(eMIDTextCode.pnCountSizesWithUnits);
                    break;
                case "CountSizesWithAtLeast1Error":
                    strReturn = MIDText.GetTextOnly(eMIDTextCode.pnCountSizesWithAtLeast1Error);
                    break;
                case "TotalSizeUnitError":
                    strReturn = MIDText.GetTextOnly(eMIDTextCode.pnTotalSizeUnitError);
                    break;
                case "AvgErrorPerSizeWithUnits":
                    strReturn = MIDText.GetTextOnly(eMIDTextCode.pnAvgErrorPerSizeWithUnits);
                    break;
                case "AvgErrorPerSizeInError":
                    strReturn = MIDText.GetTextOnly(eMIDTextCode.pnAvgErrorPerSizeInError);
                    break;
                case "AvgErrorPerPack":
                    strReturn = MIDText.GetTextOnly(eMIDTextCode.pnAvgErrorPerPack);
                    break;
                case "AvgErrorPerPackWithError":
                    strReturn = MIDText.GetTextOnly(eMIDTextCode.pnAvgErrorPerPackWithError);
                    break;
                case "AvgErrorPerStoreWithError":
                    strReturn = MIDText.GetTextOnly(eMIDTextCode.pnAvgErrorPerStoreWithError);
                    break;
                case "AvgErrorPerStoreWithPacks":
                    strReturn = MIDText.GetTextOnly(eMIDTextCode.pnAvgErrorPerStoreWithPacks);
                    break;
                case "AvgErrorPerStoreWithUnits":
                    strReturn = MIDText.GetTextOnly(eMIDTextCode.pnAvgErrorPerStoreWithUnits);
                    break;
                case "PercentAllStoreUnitsInPacks":
                    strReturn = MIDText.GetTextOnly(eMIDTextCode.pnPercentAllStoreUnitsInPacks);
                    break;
                case "PercentNonReserveUnitsInPacks":
                    strReturn = MIDText.GetTextOnly(eMIDTextCode.pnPercentNonReserveUnitsInPacks);
                    break;
                case "PercentReserveUnitsInPacks":
                    strReturn = MIDText.GetTextOnly(eMIDTextCode.pnPercentReserveUnitsInPacks);
                    break;
                case "PercentReserveInTotal":
                    strReturn = MIDText.GetTextOnly(eMIDTextCode.pnPercentReserveInTotal);
                    break;
                case "PercentBulkToTotal":
                    strReturn = MIDText.GetTextOnly(eMIDTextCode.pnPercentBulkToTotal);
                    break;
                case "PercentBulkInReserve":
                    strReturn = MIDText.GetTextOnly(eMIDTextCode.pnPercentBulkInReserve);
                    break;
                case "PercentBulkTotalInReserve":
                    strReturn = MIDText.GetTextOnly(eMIDTextCode.pnPercentBulkTotalInReserve);
                    break;
                case "PercentReserveInBulk":
                    strReturn = MIDText.GetTextOnly(eMIDTextCode.pnPercentReserveInBulk);
                    break;
                case "CountOfAllStoresWithPacks":
                    strReturn = MIDText.GetTextOnly(eMIDTextCode.pnCountOfAllStoresWithPacks);
                    break;
                case "CountOfNonReserveStoresWithPacks":
                    strReturn = MIDText.GetTextOnly(eMIDTextCode.pnCountOfNonReserveStoresWithPacks);
                    break;
                case "CountOfAllStoresWithUnits":
                    strReturn = MIDText.GetTextOnly(eMIDTextCode.pnCountOfAllStoresWithUnits);
                    break;
                case "CountOfNonReserveStoresWithUnits":
                    strReturn = MIDText.GetTextOnly(eMIDTextCode.pnCountOfNonReserveStoresWithUnits);
                    break;
                case "CountOfAllStoresWithBulk":
                    strReturn = MIDText.GetTextOnly(eMIDTextCode.pnCountOfAllStoresWithBulk);
                    break;
                case "CountOfNonReserveStoresWithBulk":
                    strReturn = MIDText.GetTextOnly(eMIDTextCode.pnCountOfNonReserveStoresWithBulk);
                    break;
                case "StoresWithErrorCount":
                    strReturn = MIDText.GetTextOnly(eMIDTextCode.pnStoresWithErrorCount);
                    break;
                case "PercentNonReserveWithUnitsInError":
                    strReturn = MIDText.GetTextOnly(eMIDTextCode.pnPercentNonReserveWithUnitsInError);
                    break;
                case "Key":
                    strReturn = MIDText.GetTextOnly(eMIDTextCode.pnKey);
                    break;
                    // begin TT#801 - BP Need additional pack select criteria
                case "PercentOriginalBuyPackaged":
                    strReturn = MIDText.GetTextOnly(eMIDTextCode.pnPercentOriginalBuyPackaged);
                    break;
                case "OriginalBuyPackUnits":
                    strReturn = MIDText.GetTextOnly(eMIDTextCode.pnOriginalBuyPackUnits);
                    break;
                    // end TT#801 - BP Need additional pack select criteria
            }
            return strReturn;
        }

        int _BuildPackChosenOption = 0;
        private void cmbEvalOptions_SelectionChangeCommitted(object sender, EventArgs e)
        {
            // Begin TT#634-MD - JSmith - WUB Build Pack Evaluation Tab on the 1st combination is blank.
            //if (cmbEvalOptions.SelectedIndex <= 0)
            if (cmbEvalOptions.SelectedIndex < 0)
            // End TT#634-MD - JSmith - WUB Build Pack Evaluation Tab on the 1st combination is blank.
            {
                return;
            }

            grdResults.Columns[0].Visible = true;

            if (grdResults.Columns.Count > 1)
            {
                for (int resultsCol = grdResults.Columns.Count - 1; resultsCol >= 1; resultsCol--)
                {
                    grdResults.Columns.RemoveAt(resultsCol);
                }
            }

            char[] delim = ";".ToCharArray();

            string[] OptionDescriptors = cmbEvalOptions.Text.Split(delim);

            string OptionID = OptionDescriptors[0].Substring(8).Trim();

            int OptionKey = Convert.ToInt32(hEvalOptionRIDs[OptionID]);
            _BuildPackChosenOption = OptionKey;

            OptionPackProfile opp = _BuildPacksMethod.GetOptionPackProfile(OptionKey);
            OptionPack_Combo oppc = opp.OptionPacks;
            PackPatternList ppl_oppc = oppc.PackPatternList;
            SizeUnits[] suBulk = opp.NonReserveBulkSizeBuy;
            SizeUnits[] suReserve = opp.ReserveBulkSizeBuy;
            SizeUnits[] suTotal = opp.AllStoreBulkSizeBuy;

            PropertyDescriptorCollection pdc = TypeDescriptor.GetProperties(opp);

            grdResults.Columns.Add(opp.OptionPackID.ToString().Trim(), "Total");
            grdResults.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;

            int prop_row = 0;
            foreach (PropertyDescriptor pd in pdc)
            {

                string tc = pd.Converter.ToString().Trim();

                string thing = pd.Name;

                string strPropName = pd.Name;

                string[] fieldsToOmit = strRowOmissionList.Split(delim);

                bool omitField = false;


                #region WriteProperties
                switch (tc)
                {
                    case "System.ComponentModel.DoubleConverter":

                        foreach (string fieldToOmit in fieldsToOmit)
                        {
                            if (strPropName == fieldToOmit)
                            {
                                omitField = true;
                                break;
                            }
                        }

                        if (omitField == false)
                        {
                            double dValue = 0;
                            string strValue = "";
                            switch(strPropName)
                            {
                                case "AllStoreTotalNumberOfPacks":
                                case "AllStoreTotalPackUnits":
                                case "AverageErrorPerSizeWithUnits":
                                case "AverageErrorPerStoreWithPacks":
                                case "CountOfAllStoresWithPacks":
                                case "CountOfNonReserveStoresWithPacks":
                                case "NonReserveTotalNumberOfPacks":
                                case "NonReserveTotalPackUnits":
                                case "ReserveTotalNumberOfPacks":
                                case "ReserveTotalPackUnits":
                                    dValue = Math.Round(Convert.ToDouble(pd.GetValue(opp)), 2);
                                    strValue = dValue.ToString().Trim();
                                    grdResults.Rows[prop_row].Cells[1].Value = strValue;
                                    int intCell = 2;
                                    foreach (OptionPack_PackPattern pp in opp.OptionPacks.PackPatternList)
                                    {

                                        char[] patternDelim = ".".ToCharArray();
                                        string[] patternNameParts = pp.PatternName.Split(patternDelim);
                                        switch (strPropName)
                                        {
                                            case "AllStoreTotalNumberOfPacks":
                                                if (prop_row == 0)
                                                {
                                                    grdResults.Columns.Add(pp.PatternName.ToString().Trim(), pp.PackName);
                                                }
                                                grdResults.Rows[prop_row].Cells[intCell].Value = opp.GetPackProperty(eBuildPackProperty.AllStoreTotalNumberOfPacks, pp.PackName);
                                                intCell++;
                                                break;
                                            case "AllStoreTotalPackUnits":
                                                if (prop_row == 0)
                                                {
                                                    grdResults.Columns.Add(pp.PatternName.ToString().Trim(), pp.PackName);
                                                }
                                                grdResults.Rows[prop_row].Cells[intCell].Value = opp.GetPackProperty(eBuildPackProperty.AllStoreTotalPackUnits, pp.PackName);
                                                intCell++;
                                                break;
                                            case "AverageErrorPerSizeWithUnits":
                                                if (prop_row == 0)
                                                {
                                                    grdResults.Columns.Add(pp.PatternName.ToString().Trim(), pp.PackName);
                                                }
                                                grdResults.Rows[prop_row].Cells[intCell].Value = opp.GetPackProperty(eBuildPackProperty.AverageErrorPerSizeWithUnits, pp.PackName);
                                                intCell++;
                                                break;
                                            case "AverageErrorPerStoreWithPacks":
                                                if (prop_row == 0)
                                                {
                                                    grdResults.Columns.Add(pp.PatternName.ToString().Trim(), pp.PackName);
                                                }
                                                grdResults.Rows[prop_row].Cells[intCell].Value = opp.GetPackProperty(eBuildPackProperty.AverageErrorPerStoreWithPacks, pp.PackName);
                                                intCell++;
                                                break;
                                            case "CountOfAllStoresWithPacks":
                                                if (prop_row == 0)
                                                {
                                                    grdResults.Columns.Add(pp.PatternName.ToString().Trim(), pp.PackName);
                                                }
                                                grdResults.Rows[prop_row].Cells[intCell].Value = opp.GetPackProperty(eBuildPackProperty.CountOfAllStoresWithPacks, pp.PackName);
                                                intCell++;
                                                break;
                                            case "CountOfNonReserveStoresWithPacks":
                                                if (prop_row == 0)
                                                {
                                                    grdResults.Columns.Add(pp.PatternName.ToString().Trim(), pp.PackName);
                                                }
                                                grdResults.Rows[prop_row].Cells[intCell].Value = opp.GetPackProperty(eBuildPackProperty.CountOfNonReserveStoresWithPacks, pp.PackName);
                                                intCell++;
                                                break;
                                            case "NonReserveTotalNumberOfPacks":
                                                if (prop_row == 0)
                                                {
                                                    grdResults.Columns.Add(pp.PatternName.ToString().Trim(), pp.PackName);
                                                }
                                                grdResults.Rows[prop_row].Cells[intCell].Value = opp.GetPackProperty(eBuildPackProperty.NonReserveTotalNumberOfPacks, pp.PackName);
                                                intCell++;
                                                break;
                                            case "NonReserveTotalPackUnits":
                                                if (prop_row == 0)
                                                {
                                                    grdResults.Columns.Add(pp.PatternName.ToString().Trim(), pp.PackName);
                                                }
                                                grdResults.Rows[prop_row].Cells[intCell].Value = opp.GetPackProperty(eBuildPackProperty.NonReserveTotalPackUnits, pp.PackName);
                                                intCell++;
                                                break;
                                            case "ReserveTotalNumberOfPacks":
                                                if (prop_row == 0)
                                                {
                                                    grdResults.Columns.Add(pp.PatternName.ToString().Trim(), pp.PackName);
                                                }
                                                grdResults.Rows[prop_row].Cells[intCell].Value = opp.GetPackProperty(eBuildPackProperty.ReserveTotalNumberOfPacks, pp.PackName);
                                                intCell++;
                                                break;
                                            case "ReserveTotalPackUnits":
                                                if (prop_row == 0)
                                                {
                                                    grdResults.Columns.Add(pp.PatternName.ToString().Trim(), pp.PackName);
                                                }
                                                // Begin TT#732 - JSmith - Build Pack on the Evaluation tab the total bulk units is not correct.
                                                //grdResults.Rows[prop_row].Cells[intCell].Value = opp.GetPackProperty(eBuildPackProperty.ReserveTotalNumberOfPacks, pp.PackName);
                                                grdResults.Rows[prop_row].Cells[intCell].Value = opp.GetPackProperty(eBuildPackProperty.ReserveTotalPackUnits, pp.PackName);
                                                // End TT#732
                                                intCell++;
                                                break;
                                        }

                                    }
                                    prop_row++;
                                    break;

                            }
                            
                        }
                        break;

                    case "System.ComponentModel.StringConverter":
                    case "System.ComponentModel.Int16Converter":
                    case "System.ComponentModel.Int32Converter":
                    case "System.ComponentModel.Int64Converter":
                    case "System.ComponentModel.BooleanConverter":
                    case "System.ComponentModel.ByteConverter":
                    case "System.ComponentModel.CharConverter":
                    case "System.ComponentModel.DateTimeConverter":
                    case "System.ComponentModel.DateTimeOffsetConverter":
                    case "System.ComponentModel.MultilineStringConverter":
                    case "System.ComponentModel.NullableConverter":
                    case "System.ComponentModel.SByteConverter":
                    case "System.ComponentModel.TimeSpanConverter":
                    case "System.ComponentModel.UInt16Converter":
                    case "System.ComponentModel.UInt32Converter":
                    case "System.ComponentModel.UInt64Converter":
                    case "System.ComponentModel.DecimalConverter":
                    case "System.ComponentModel.SingleConverter":

                        foreach (string fieldToOmit in fieldsToOmit)
                        {
                            if (strPropName == fieldToOmit)
                            {
                                omitField = true;
                                break;
                            }
                        }

                        if (omitField == false)
                        {
                            int intValue = 0;
                            string strValue = "";
                            switch (strPropName)
                            {
                                case "AllStoreTotalNumberOfPacks":
                                case "AllStoreTotalPackUnits":
                                case "AverageErrorPerSizeWithUnits":
                                case "AverageErrorPerStoreWithPacks":
                                case "CountOfAllStoresWithPacks":
                                case "CountOfNonReserveStoresWithPacks":
                                case "NonReserveTotalNumberOfPacks":
                                case "NonReserveTotalPackUnits":
                                case "ReserveTotalNumberOfPacks":
                                case "ReserveTotalPackUnits":
                                    intValue = Convert.ToInt32(pd.GetValue(opp).ToString().Trim());
                                    strValue = intValue.ToString().Trim();
                                    grdResults.Rows[prop_row].Cells[1].Value = strValue;

                                    int intCell = 2;
                                    foreach (OptionPack_PackPattern pp in opp.OptionPacks.PackPatternList)
                                    {
                                        char[] patternDelim = ".".ToCharArray();
                                        string[] patternNameParts = pp.PatternName.Split(patternDelim);
                                        switch (strPropName)
                                        {
                                            case "AllStoreTotalNumberOfPacks":
                                                if (prop_row == 0)
                                                {
                                                    grdResults.Columns.Add(pp.PatternName.ToString().Trim(), pp.PackName);
                                                }
                                                grdResults.Rows[prop_row].Cells[intCell].Value = opp.GetPackProperty(eBuildPackProperty.AllStoreTotalNumberOfPacks, pp.PackName);
                                                intCell++;
                                                break;
                                            case "AllStoreTotalPackUnits":
                                                if (prop_row == 0)
                                                {
                                                    grdResults.Columns.Add(pp.PatternName.ToString().Trim(), pp.PackName);
                                                }
                                                grdResults.Rows[prop_row].Cells[intCell].Value = opp.GetPackProperty(eBuildPackProperty.AllStoreTotalPackUnits, pp.PackName);
                                                intCell++;
                                                break;
                                            case "AverageErrorPerSizeWithUnits":
                                                if (prop_row == 0)
                                                {
                                                    grdResults.Columns.Add(pp.PatternName.ToString().Trim(), pp.PackName);
                                                }
                                                grdResults.Rows[prop_row].Cells[intCell].Value = opp.GetPackProperty(eBuildPackProperty.AverageErrorPerSizeWithUnits, pp.PackName);
                                                intCell++;
                                                break;
                                            case "AverageErrorPerStoreWithPacks":
                                                if (prop_row == 0)
                                                {
                                                    grdResults.Columns.Add(pp.PatternName.ToString().Trim(), pp.PackName);
                                                }
                                                grdResults.Rows[prop_row].Cells[intCell].Value = opp.GetPackProperty(eBuildPackProperty.AverageErrorPerStoreWithPacks, pp.PackName);
                                                intCell++;
                                                break;
                                            case "CountOfAllStoresWithPacks":
                                                if (prop_row == 0)
                                                {
                                                    grdResults.Columns.Add(pp.PatternName.ToString().Trim(), pp.PackName);
                                                }
                                                grdResults.Rows[prop_row].Cells[intCell].Value = opp.GetPackProperty(eBuildPackProperty.CountOfAllStoresWithPacks, pp.PackName);
                                                intCell++;
                                                break;
                                            case "CountOfNonReserveStoresWithPacks":
                                                if (prop_row == 0)
                                                {
                                                    grdResults.Columns.Add(pp.PatternName.ToString().Trim(), pp.PackName);
                                                }
                                                grdResults.Rows[prop_row].Cells[intCell].Value = opp.GetPackProperty(eBuildPackProperty.CountOfNonReserveStoresWithPacks, pp.PackName);
                                                intCell++;
                                                break;
                                            case "NonReserveTotalNumberOfPacks":
                                                if (prop_row == 0)
                                                {
                                                    grdResults.Columns.Add(pp.PatternName.ToString().Trim(), pp.PackName);
                                                }
                                                grdResults.Rows[prop_row].Cells[intCell].Value = opp.GetPackProperty(eBuildPackProperty.NonReserveTotalNumberOfPacks, pp.PackName);
                                                intCell++;
                                                break;
                                            case "NonReserveTotalPackUnits":
                                                if (prop_row == 0)
                                                {
                                                    grdResults.Columns.Add(pp.PatternName.ToString().Trim(), pp.PackName);
                                                }
                                                grdResults.Rows[prop_row].Cells[intCell].Value = opp.GetPackProperty(eBuildPackProperty.NonReserveTotalPackUnits, pp.PackName);
                                                intCell++;
                                                break;
                                            case "ReserveTotalNumberOfPacks":
                                                if (prop_row == 0)
                                                {
                                                    grdResults.Columns.Add(pp.PatternName.ToString().Trim(), pp.PackName);
                                                }
                                                grdResults.Rows[prop_row].Cells[intCell].Value = opp.GetPackProperty(eBuildPackProperty.ReserveTotalNumberOfPacks, pp.PackName);
                                                intCell++;
                                                break;
                                            case "ReserveTotalPackUnits":
                                                if (prop_row == 0)
                                                {
                                                    grdResults.Columns.Add(pp.PatternName.ToString().Trim(), pp.PackName);
                                                }
                                                // Begin TT#732 - JSmith - Build Pack on the Evaluation tab the total bulk units is not correct.
                                                //grdResults.Rows[prop_row].Cells[intCell].Value = opp.GetPackProperty(eBuildPackProperty.ReserveTotalNumberOfPacks, pp.PackName);
                                                grdResults.Rows[prop_row].Cells[intCell].Value = opp.GetPackProperty(eBuildPackProperty.ReserveTotalPackUnits, pp.PackName);
                                                // End TT#732
                                                intCell++;
                                                break;
                                        }

                                    }

                                    prop_row++;
                                    break;
                            }

                        }
                        break;
                }

            }
            #endregion 

            SizeUnits[] AllStorePackUnits = opp.NonReserveTotalSizeBuy;
            SizeUnits[] ReserveStorePackUnits = opp.ReserveTotalSizeBuy;
            SizeUnits[] TotalStorePackUnits = opp.AllStoreTotalSizeBuy;
            
            SizeGroup sg = new SizeGroup();

            DataSet dsSummary = new DataSet("DataSummary");
            DataTable dtSummaryLevel1 = new DataTable("SummaryLevel1");
            DataTable dtSummaryLevel2 = new DataTable("SummaryLevel2");
            
            //add the needed fields and keys to level 1
            DataColumn Col = new DataColumn("SummaryID");
            Col.Caption = "";
            Col.DataType = System.Type.GetType("System.Int32");
            Col.ReadOnly = true; // TT#886 - Distinct Packs have same size runs
            dtSummaryLevel1.Columns.Add(Col);
            DataColumn[] dtKeys = new DataColumn[0];

            Col = new DataColumn("Location");
            Col.Caption = "Location";
            Col.DataType = System.Type.GetType("System.String");
            Col.ReadOnly = true; // TT#886 - Distinct Packs have same size runs
            dtSummaryLevel1.Columns.Add(Col);

            // begin TT#886 - Distinct Packs have same size runs - (unrelated) Standardize columns for readability
            Col = new DataColumn("TotalPacks");
            Col.Caption = "";
            Col.DataType = System.Type.GetType("System.String");
            Col.ReadOnly = true; // TT#886 -Distinct Packs have same size runs
            dtSummaryLevel1.Columns.Add(Col);

            Col = new DataColumn("Multiple");
            Col.Caption = "";
            Col.DataType = System.Type.GetType("System.String");
            Col.ReadOnly = true; // TT#886 - Distinct Packs have same size runs
            dtSummaryLevel1.Columns.Add(Col);
            // end TT#886 - Distinct Packs have same size runs - (unrelated) Standardize columns for readability
            
            Col = new DataColumn("Quantity");
            //Col.Caption = "Quantity";   // TT#886 - Distinct Packs have same size runs
            Col.Caption = "Total Units";  // TT#886 - Distinct Packs have same size runs
            Col.DataType = System.Type.GetType("System.Int32");
            Col.ReadOnly = true; // TT#886 - Distinct Packs have same size runs
            dtSummaryLevel1.Columns.Add(Col);

            //// begin TT#886 - Distinct Packs have same size runs - (unrelated) Standardize columns for readability
            //Col = new DataColumn("TotalPacks");
            //Col.Caption = "";
            //Col.DataType = System.Type.GetType("System.String");
            //Col.ReadOnly = true; // TT#886 - Distinct Packs have same size runs
            //dtSummaryLevel1.Columns.Add(Col);

            //Col = new DataColumn("Multiple");
            //Col.Caption = "";
            //Col.DataType = System.Type.GetType("System.String");
            //Col.ReadOnly = true; // TT#886 - Distinct Packs have same size runs
            //dtSummaryLevel1.Columns.Add(Col);
            //// end TT#886 - Distinct Packs have same size runs - (unrelated) Standardize columns for readability

            //add the needed fields and keys to level 2
            DataColumn Col2 = new DataColumn("SummaryID");
            Col2.Caption = "";
            Col2.DataType = System.Type.GetType("System.Int32");
            Col2.ReadOnly = true; // TT#886 - Distinct Packs have same size runs
            dtSummaryLevel2.Columns.Add(Col2);
            DataColumn[] dtKeys1 = new DataColumn[0];

            Col2 = new DataColumn("Location");
            Col2.Caption = "";
            Col2.DataType = System.Type.GetType("System.String");
            Col2.ReadOnly = true; // TT#886 - Distinct Packs have same size runs
            dtSummaryLevel2.Columns.Add(Col2);

            // begin TT#886 - Distinct Packs have same size runs - (unrelated) Standardize columns for readability
            Col2 = new DataColumn("Quantity");
            Col2.Caption = "Multiple";
            Col2.DataType = System.Type.GetType("System.Int32");
            Col2.ReadOnly = true; // TT#886 - Distinct Packs have same size runs
            dtSummaryLevel2.Columns.Add(Col2);
            // end TT#886 - Distinct Packs have same size runs - (unrelated) Standardize columns for readability

            // begin TT#886 - Distinct Packs have same size runs - (unrelated) Standardize columns for readability
            Col2 = new DataColumn("TotalPacks");
            Col2.Caption = "Total Packs";
            Col2.DataType = System.Type.GetType("System.Int32");
            dtSummaryLevel2.Columns.Add(Col2);
            // end TT#886 - Distinct Packs have same size runs - (unrelated) Standardize columns for readability

            Col2 = new DataColumn("Total");
            Col2.Caption = "Total Units";
            Col2.DataType = System.Type.GetType("System.Int32");
            Col2.ReadOnly = true; // TT#886 - Distinct Packs have same size runs
            dtSummaryLevel2.Columns.Add(Col2);

            //// begin TT#886 - Distinct Packs have same size runs - (unrelated) Standardize columns for readability
            //Col2 = new DataColumn("TotalPacks");
            //Col2.Caption = "Total Packs";
            //Col2.DataType = System.Type.GetType("System.Int32");
            //Col2.ReadOnly = true; // TT#886 - Distinct Packs have same size runs
            //dtSummaryLevel2.Columns.Add(Col2);
            //// end TT#886 - Distinct Packs have same size runs - (unrelated) Standardize columns for readability

            //// begin TT#886 - Distinct Packs have same size runs - (unrelated) Standardize columns for readability
            //Col2 = new DataColumn("Quantity");
            //Col2.Caption = "Multiple";
            //Col2.DataType = System.Type.GetType("System.Int32");
            //Col2.ReadOnly = true; // TT#886 - Distinct Packs have same size runs
            //dtSummaryLevel2.Columns.Add(Col2);
            //// end TT#886 - Distinct Packs have same size runs - (unrelated) Standardize columns for readability

            int SummaryRowCtr = 0;
            int SummaryColCtr = 0;
            DataRow storesRow = dtSummaryLevel1.NewRow();
            DataRow reserveRow = dtSummaryLevel1.NewRow();
            DataRow totalRow = dtSummaryLevel1.NewRow();
            Dictionary<int, string> sizeKeyDictionary = new Dictionary<int, string>(); // TT#886 - Distinct Packs have same size runs
           
            foreach (SizeUnits size in AllStorePackUnits)
            {
                string sizeKey = "Size_" + size.RID.ToString(); // TT#886 - Distinct Packs have same size runs
                sizeKeyDictionary.Add(size.RID, sizeKey); // TT#886 - Distinct Packs have same size runs
                if (SummaryColCtr == 0)
                {
                    storesRow["SummaryID"] = SummaryRowCtr;
                    storesRow["Location"] = "Stores";
                    storesRow["Quantity"] = opp.NonReserveTotalBuy;
                }

                string SizeCodePrimary = "";
                string SizeCodeSecondary = "";
                sg.GetSizeCodeID(size.RID, out SizeCodePrimary, out SizeCodeSecondary);

                // begin TT#886 - Distinct Packs have same size runs
                //Col = new DataColumn("Size" + SummaryColCtr.ToString().Trim());
                //Col2 = new DataColumn("Size_" + SummaryColCtr.ToString().Trim());
                Col = new DataColumn(sizeKey);
                Col2 = new DataColumn(sizeKey);
                // end TT#886 - Distinct Packs have same size runs
                Col.DataType = System.Type.GetType("System.Int32");
                Col2.DataType = System.Type.GetType("System.Int32");

                if (SizeCodeSecondary != "No Secondary Size" && SizeCodeSecondary != "None")
                {
                    Col.Caption = SizeCodePrimary + " (" + SizeCodeSecondary + ")";
                    Col2.Caption = SizeCodePrimary + " (" + SizeCodeSecondary + ")";
                }
                else
                {
                    Col.Caption = SizeCodePrimary;
                    Col2.Caption = SizeCodePrimary;
                }
                Col.ReadOnly = true;  // TT#886 - Distinct Packs have same size runs
                Col2.ReadOnly = true; // TT#886 - Distinct Packs have same size runs
                dtSummaryLevel1.Columns.Add(Col);
                dtSummaryLevel2.Columns.Add(Col2);

                // begin TT#886 - Distinct Packs have same size runs
                //storesRow["Size" + SummaryColCtr.ToString().Trim()] = size.Units;
                storesRow[sizeKey] = size.Units;
                // end TT#886 - Distinct Packs have same size runs
                SummaryColCtr++;
            }
            dtSummaryLevel1.Rows.Add(storesRow);
            SummaryColCtr = 0;


            foreach (OptionPack_PackPattern pp in ppl_oppc)
            {
                DataRow sumRow = dtSummaryLevel2.NewRow();

                // begin TT#886 - Distinct Packs have same size runs
                //char[] delim1 = ".".ToCharArray();
                //string[] PackPatterNameParts = pp.PatternName.Split(delim1);
                // end TT#886 - Distinct Packs have same size runs

                sumRow["SummaryID"] = SummaryRowCtr;
                sumRow["Location"] = "Pack " + pp.PackName;
                sumRow["Quantity"] = pp.PackMultiple;
                sumRow["TotalPacks"] = opp.GetPackProperty(eBuildPackProperty.NonReserveTotalNumberOfPacks, pp.PackName);
                sumRow["Total"] = opp.GetPackProperty(eBuildPackProperty.NonReserveTotalPackUnits, pp.PackName);

                SizeUnitRun sur = pp.SizeRun;
                int[] surKey = sur.SizeRIDs;

                // begin TT#886 - Distinct Packs have same size runs
                foreach (int sizeRID in surKey)
                {
                    sumRow[sizeKeyDictionary[sizeRID]] = sur[sizeRID].Units;
                }
                //for (int intSU = 0; intSU < sur.Count; intSU++)
                //{
                //    sumRow["Size_" + intSU] = sur[Convert.ToInt32(surKey[intSU])].Units;
                //}
                // end TT#886 - Distinct Packs have same size runs

                SummaryColCtr++;
                dtSummaryLevel2.Rows.Add(sumRow);
            }
            
            SummaryColCtr = 0;
            DataRow bulkRow = dtSummaryLevel2.NewRow();

            foreach (SizeUnits size in suBulk)
            {
                if (SummaryColCtr == 0)
                {
                    bulkRow["SummaryID"] = SummaryRowCtr;
                    bulkRow["Location"] = "Bulk";
                    bulkRow["Total"] = opp.NonReserveBulkBuy;
                }

                // begin TT#886 - Distinct Packs have same size runs
                //bulkRow["Size_" + SummaryColCtr.ToString().Trim()] = size.Units;
                bulkRow[sizeKeyDictionary[size.RID]] = size.Units;
                // end TT#886 - Distinct Packs have same size runs

                SummaryColCtr++;
            }
            dtSummaryLevel2.Rows.Add(bulkRow);
            SummaryColCtr = 0;
            SummaryRowCtr++;

            foreach (SizeUnits size in ReserveStorePackUnits)
            {

                if (SummaryColCtr == 0)
                {
                    reserveRow["SummaryID"] = SummaryRowCtr;
                    reserveRow["Location"] = "Reserve";
                    reserveRow["Quantity"] = opp.ReserveTotalBuy;
                }

                // begin TT#886 - Distinct Packs have same size runs
                //reserveRow["Size" + SummaryColCtr.ToString().Trim()] = size.Units;
                reserveRow[sizeKeyDictionary[size.RID]] = size.Units;
                // end TT#886 - Distinct Packs have same size runs

                SummaryColCtr++;
            }
            dtSummaryLevel1.Rows.Add(reserveRow);
            SummaryColCtr = 0;

            foreach (OptionPack_PackPattern pp in ppl_oppc)
            {
                DataRow sumRow = dtSummaryLevel2.NewRow();

                // begin TT#886 - Distinct Packs have same size runs
                //char[] delim1 = ".".ToCharArray();
                //string[] PackPatterNameParts = pp.PatternName.Split(delim1);
                // end TT#886 - Distinct Packs have same size runs

                sumRow["SummaryID"] = SummaryRowCtr;
                sumRow["Location"] = "Pack " + pp.PackName;
                sumRow["Quantity"] = pp.PackMultiple;
                sumRow["TotalPacks"] = opp.GetPackProperty(eBuildPackProperty.ReserveTotalNumberOfPacks, pp.PackName);
                sumRow["Total"] = opp.GetPackProperty(eBuildPackProperty.ReserveTotalPackUnits, pp.PackName);

                SizeUnitRun sur = pp.SizeRun;
                int[] surKey = sur.SizeRIDs;

                // begin TT#886 - Distinct Packs have same size runs
                foreach (int sizeRID in surKey)
                {
                    sumRow[sizeKeyDictionary[sizeRID]] = sur[sizeRID].Units;
                }
                //for (int intSU = 0; intSU < sur.Count; intSU++)
                //{
                //    sumRow["Size_" + intSU] = sur[Convert.ToInt32(surKey[intSU])].Units;
                //}
                // end TT#886 - Distinct Packs have same size runs

                SummaryColCtr++;
                dtSummaryLevel2.Rows.Add(sumRow);
            }

            SummaryColCtr = 0;

            DataRow reserveSubRow = dtSummaryLevel2.NewRow();

            foreach (SizeUnits size in suReserve)
            {
                if (SummaryColCtr == 0)
                {
                    reserveSubRow["SummaryID"] = SummaryRowCtr;
                    reserveSubRow["Location"] = "Bulk";
                    // Begin TT#732 - JSmith - Build Pack on the Evaluation tab the total bulk units is not correct.
                    //reserveSubRow["Total"] = opp.ReserveTotalBuy;
                    reserveSubRow["Total"] = opp.ReserveBulkBuy;
                    // End TT#732
                }

                // begin TT#886 -  Distinct Packs have same size runs
                //reserveSubRow["Size_" + SummaryColCtr.ToString().Trim()] = size.Units;
                reserveSubRow[sizeKeyDictionary[size.RID]] = size.Units;
                // end TTE#886 - Distinct Packs have same size runs

                SummaryColCtr++;
            }
            dtSummaryLevel2.Rows.Add(reserveSubRow);
            SummaryColCtr = 0;
            SummaryRowCtr++;

            foreach (SizeUnits size in TotalStorePackUnits)
            {

                if (SummaryColCtr == 0)
                {
                    totalRow["SummaryID"] = SummaryRowCtr;
                    totalRow["Location"] = "Total";
                    totalRow["Quantity"] = opp.AllStoreTotalBuy;
                }

                // begin TT#886 - Distinct Packs have same size runs
                //totalRow["Size" + SummaryColCtr.ToString().Trim()] = size.Units;
                totalRow[sizeKeyDictionary[size.RID]] = size.Units;
                // end TT#886 - Distinct Packs have same size runs

                SummaryColCtr++;
            }
            dtSummaryLevel1.Rows.Add(totalRow);
            SummaryColCtr = 0;

            foreach (OptionPack_PackPattern pp in ppl_oppc)
            {
                DataRow sumRow = dtSummaryLevel2.NewRow();

                // begin TT3886 - Distinct Packs have same size runs
                //char[] delim1 = ".".ToCharArray();
                //string[] PackPatterNameParts = pp.PatternName.Split(delim1);
                // end TT#886 - Distinct Packs have same size runs

                sumRow["SummaryID"] = SummaryRowCtr;
                sumRow["Location"] = "Pack " + pp.PackName;
                sumRow["Quantity"] = pp.PackMultiple;
                sumRow["TotalPacks"] = opp.GetPackProperty(eBuildPackProperty.AllStoreTotalNumberOfPacks, pp.PackName);
                sumRow["Total"] = opp.GetPackProperty(eBuildPackProperty.AllStoreTotalPackUnits, pp.PackName);

                SizeUnitRun sur = pp.SizeRun;
                int[] surKey = sur.SizeRIDs;

                // begin TT#886 - Distinct Packs have same size runs
                foreach (int sizeRID in surKey)
                {
                    sumRow[sizeKeyDictionary[sizeRID]] = sur[sizeRID].Units;
                }
                //for (int intSU = 0; intSU < sur.Count; intSU++)
                //{
                //    sumRow["Size_" + intSU] = sur[Convert.ToInt32(surKey[intSU])].Units;
                //}
                // end TT#886 - Distinct Packs have same size runs

                SummaryColCtr++;
                dtSummaryLevel2.Rows.Add(sumRow);
            }

            SummaryColCtr = 0;
            DataRow totalSubRow = dtSummaryLevel2.NewRow();

            foreach (SizeUnits size in suTotal)
            {
                if (SummaryColCtr == 0)
                {
                    totalSubRow["SummaryID"] = SummaryRowCtr;
                    totalSubRow["Location"] = "Bulk";
                    totalSubRow["Total"] = opp.AllStoreBulkBuy;
                }

                // begin TT#886 - Distinct Packs have same size runs
                //totalSubRow["Size_" + SummaryColCtr.ToString().Trim()] = size.Units;
                totalSubRow[sizeKeyDictionary[size.RID]] = size.Units;
                // end TT#886 - Distinct Packs have same size runs

                SummaryColCtr++;
            }
            dtSummaryLevel2.Rows.Add(totalSubRow);

            dsSummary.Tables.Add(dtSummaryLevel1);
            dsSummary.Tables.Add(dtSummaryLevel2);

            DataRelation drSummary = new DataRelation("SummaryDetails",
                dsSummary.Tables[0].Columns["SummaryID"],
                dsSummary.Tables[1].Columns["SummaryID"]);
            dsSummary.Relations.Add(drSummary);

            grdSummary.DataSource = null;
            grdSummary.DataSource = dsSummary;

            FormatGrids(grdSummary);
     
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            // Begin TT#289-MD - JSmith - Build Pack method-> processes but fails to apply the pack to the header
            try
            {
            // End TT#289-MD - JSmith - Build Pack method-> processes but fails to apply the pack to the header
                //Begin - tt#845 - Process and Apply buttons are confusing - apicchetti - 10/15/2010
                bool blApply = false;

                if (_BuildPacksMethod.IsInteractive == true)
                {
                    if (MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_al_BP_ApplyConfirmation),
                        "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        blApply = true;
                    }
                }
                // BEGIN TT#1061 - #845-Apply Button in Build Pack Method - apicchetti - 1/6/2011
                else
                {
                    blApply = true;
                }
                // END TT#1061 - #845-Apply Button in Build Pack Method - apicchetti - 1/6/2011

                if (blApply == true)
                {
                    //End - tt#845 - Process and Apply buttons are confusing - apicchetti - 10/15/2010

                    MIDException StatusReason = null;
                    _BuildPacksMethod.ApplySelectedOptionPackProfile(_BuildPackChosenOption, out StatusReason);

                    if (StatusReason != null)
                    {
                        if (StatusReason.ErrorLevel == eErrorLevel.fatal || StatusReason.ErrorLevel == eErrorLevel.severe)
                        {
                            // Begin TT#234-MD - JSmith - Receive an Unhandled Exception when using a WUB header with 1 color>running a rule method with a quantity>size need>build packs, when selecting the apply button get the unhandled exception.
                            //throw new MIDException(StatusReason.ErrorLevel, StatusReason.ErrorNumber,
                            //    MIDText.GetTextOnly(StatusReason.ErrorNumber), StatusReason.InnerException);
                            MessageBox.Show(StatusReason.ErrorMessage, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            // End TT#234-MD - JSmith - Receive an Unhandled Exception when using a WUB header with 1 color>running a rule method with a quantity>size need>build packs, when selecting the apply button get the unhandled exception.
                        }
                        else
                        {
                            //Begin TT#684 - APicchetti - Build Packs Soft Text
                            MessageBox.Show(MIDText.GetTextOnly(StatusReason.ErrorNumber) + "\n" +
                                StatusReason.ErrorMessage, "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            //End TT#684 - APicchetti - Build Packs Soft Text
                        }
                    }
                    else
                    {
                        //Begin TT#684 - APicchetti - Build Packs Soft Text

                        //MessageBox.Show("The chosen pack option was successfully applied.", "Apply Selected Option Pack",
                        //        MessageBoxButtons.OK, MessageBoxIcon.Information);

                        MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_PackOptionApplied), "",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);

                        //End TT#684 - APicchetti - Build Packs Soft Text

                    }
                    // begin TT#612 Refresh Build Pack Work Up Buy After Apply on WorkSpace
                    int[] modifiedHeader = { _BuildPacksMethod.WorkUpSizeBuy.HeaderRID };
                    EAB.AllocationWorkspaceExplorer.ReloadUpdatedHeaders(modifiedHeader);
                    // end TT##612 Refresh Build Pack Work Up Buy After Apply on WorkSpace
                }
            // Begin TT#289-MD - JSmith - Build Pack method-> processes but fails to apply the pack to the header
            }
            catch
            {
                throw;
            }
            finally
            {
                ApplicationTransaction.DequeueHeaders();
            }
            // End TT#289-MD - JSmith - Build Pack method-> processes but fails to apply the pack to the header
        }

        // Begin TT#733 - JSmith - Messaging incorrect
        //private bool FieldValidations()
        //{
        //    string validation_msg = "";

        //    if (txtName.Text.Trim() == "")
        //    {
        //        validation_msg += SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_NoMethodName) + " \n";
        //    }

        //    if (txtDesc.Text.Trim() == "")
        //    {
        //        validation_msg += SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_NoMethodDesc) + " \n";
        //    }

        //    if (grdCandidatePacks.Rows.Count < 1)
        //    {
        //        validation_msg += SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_NoPackCombo) + " \n";
        //    }

        //    DataSet dsChosenValidation;
        //    DataTable dtChosenValidation;
        //    bool chosen = false;
        //    if (grdCandidatePacks.DataSource != null)
        //    {
        //        dsChosenValidation = (DataSet)grdCandidatePacks.DataSource;
        //        dtChosenValidation = (DataTable)dsChosenValidation.Tables[0];
        //        for (int intRow = 0; intRow < dtChosenValidation.Rows.Count; intRow++)
        //        {
        //            if (Convert.ToBoolean(dtChosenValidation.Rows[intRow]["Choice"]) == true)
        //            {
        //                chosen = true;
        //                break;
        //            }
        //        }

        //        dtChosenValidation = (DataTable)dsChosenValidation.Tables[1];
        //        for (int intRow = 0; intRow < dtChosenValidation.Rows.Count; intRow++)
        //        {
        //            if (dtChosenValidation.Rows[intRow]["PackQuantity"].ToString().Trim() == "" ||
        //                dtChosenValidation.Rows[intRow]["Maximum Packs"].ToString().Trim() == "")
        //            {
        //                validation_msg += SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_NoPackQtyOrMax) + " \n";
        //                break;
        //            }
        //        }

        //    }

        //    if (chosen == false)
        //    {
        //        validation_msg += SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_NoPackCombo) + " \n";
        //    }

        //    if (txtReserve.Text.Trim() == "")
        //    {
        //        validation_msg += lblPercentToReserve.Text.Trim() +
        //            SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_BlankField) + " \n";
        //    }

        //    if (txtReserveBulk.Text.Trim() == "")
        //    {
        //        validation_msg += lblPercentToReserveBulk.Text.Trim() +
        //            SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_BlankField) + " \n";
        //    }

        //    if (txtReservePacks.Text.Trim() == "")
        //    {
        //        validation_msg += lblPercentToReservePacks.Text.Trim() +
        //            SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_BlankField) + " \n";
        //    }

        //    if (txtAvgPackDeviationTolerance.Text.Trim() != "")
        //    {
        //        if (Convert.ToDouble(txtAvgPackDeviationTolerance.Text) < 0)
        //        {
        //            validation_msg += lblAvgPackDeviationTolerance.Text.Trim() +
        //                SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_NegNumber) + " \n";
        //        }
        //    }

        //    if (txtMaxPackAllocationNeedTolerance.Text.Trim() == "")
        //    {
        //        validation_msg += lblMaxPackAllocationNeedTolerance.Text.Trim() +
        //            SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_BlankField) + " \n";
        //    }

        //    //Begin tt#683 - APicchetti - Catch negative numbers entered into the Ship Variance field
        //    if (txtMaxPackAllocationNeedTolerance.Text.Trim() != "")
        //    {
        //        if (Convert.ToDouble(txtMaxPackAllocationNeedTolerance.Text) < 0)
        //        {
        //            validation_msg += lblMaxPackAllocationNeedTolerance.Text.Trim() +
        //                SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_NegNumber) + " \n";
        //        }
        //    }

        //    if (txtMaxPackAllocationNeedTolerance.Text.IndexOf(".") >= 0)
        //    {
        //        validation_msg += lblMaxPackAllocationNeedTolerance.Text.Trim() +
        //            SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_DecimalNotAllowed) + " \n";
        //    }
        //    //End tt#683

        //    if (txtMaxPackAllocationNeedTolerance.Text.Trim().Length != 0)
        //    {
        //        double wkNeedTolerance = Double.Parse(txtMaxPackAllocationNeedTolerance.Text);
        //        if (wkNeedTolerance > 0)
        //        {
        //            if (chkDepleteReserve.Checked == false
        //            && (chkIncreaseBuyQty.Checked == false))
        //                validation_msg += lblMaxPackAllocationNeedTolerance.Text.Trim() +
        //                    SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_GreaterThanZeroVar) + " \n";
        //        }
        //    }
        //    if (txtIncreaseBuyQty.Text.Trim().Length != 0)
        //    {
        //        double wkincreaseBuyQty = Double.Parse(txtIncreaseBuyQty.Text);
        //        if (wkincreaseBuyQty < 0)
        //            validation_msg += txtIncreaseBuyQty.Text.Trim() + SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_NegNumber) + " \n";
        //    }
        //    // END TT#669 - AGallagher – Build Pack Method – Variance Options
        //    if (rdoReservePercent.Checked == true)
        //    {
        //        if (txtReserve.Text.Trim() != "")
        //        {
        //            if (Convert.ToDouble(txtReserve.Text) < 0 || Convert.ToDouble(txtReserve.Text) > 100)
        //            {
        //                validation_msg += lblPercentToReserve.Text.Trim() +
        //                    SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_NegNbrLessThanZero) + " \n";
        //            }
        //        }
        //    }

        //    if (rdoReservePercent.Checked == true)
        //    {
        //        if (txtReserveBulk.Text.Trim() != "" && txtReservePacks.Text.Trim() != "")
        //        {
        //            double dbReserveBulk = Convert.ToDouble(txtReserveBulk.Text);
        //            double dbReservePacks = Convert.ToDouble(txtReservePacks.Text);

        //            if (dbReserveBulk + dbReservePacks != 100)
        //            {
        //                validation_msg += lblPercentToReserveBulk.Text.Trim() + " and " + lblPercentToReservePacks.Text.Trim() +
        //                    SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_FieldMust100) + " \n";
        //            }
        //        }
        //    }

        //    //begin tt#681 - APicchetti - Sum of the Reserve and Bulk and Reserve as Packs equal the Reserve units
        //    if (rdoReserveUnits.Checked == true)
        //    {
        //        if (txtReserveBulk.Text.Trim() != "" && txtReservePacks.Text.Trim() != "")
        //        {
        //            double dbReserveBulk = Convert.ToDouble(txtReserveBulk.Text);
        //            double dbReservePacks = Convert.ToDouble(txtReservePacks.Text);
        //            double dbReserve = Convert.ToDouble(txtReserve.Text);

        //            if (dbReserveBulk + dbReservePacks != dbReserve)
        //            {
        //                validation_msg += lblPercentToReserveBulk.Text.Trim() + " and " + lblPercentToReservePacks.Text.Trim() +
        //                    SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_FieldMustField) + lblPercentToReserve.Text.Trim() + " \n";
        //            }
        //        }
        //    }
        //    //end tt#681

        //    if (validation_msg != "")
        //    {
        //        MessageBox.Show(validation_msg, "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        return false;
        //    }
        //    else
        //    {
        //        if (FunctionSecurity.AllowExecute == true)
        //        {
        //            btnProcess.Enabled = true;
        //        }
        //        return true;
        //    }
        //}

        private bool FieldValidations()
        {
            bool fieldsAreValid = true;

            ErrorProvider.SetError(grdCandidatePacks, null);
            ErrorProvider.SetError(txtReserve, null);
            ErrorProvider.SetError(txtReserveBulk, null);
            ErrorProvider.SetError(txtReservePacks, null);
            ErrorProvider.SetError(txtAvgPackDeviationTolerance, null);
            ErrorProvider.SetError(txtMaxPackAllocationNeedTolerance, null);

            DataSet dsChosenValidation;
            DataTable dtChosenValidation;
            bool chosen = false;
            if (grdCandidatePacks.DataSource != null)
            {
                dsChosenValidation = (DataSet)grdCandidatePacks.DataSource;
                dtChosenValidation = (DataTable)dsChosenValidation.Tables[0];
                for (int intRow = 0; intRow < dtChosenValidation.Rows.Count; intRow++)
                {
                    if (Convert.ToBoolean(dtChosenValidation.Rows[intRow]["Choice"]) == true)
                    {
                        chosen = true;
                        break;
                    }
                }

                dtChosenValidation = (DataTable)dsChosenValidation.Tables[1];
                for (int intRow = 0; intRow < dtChosenValidation.Rows.Count; intRow++)
                {
                    if (dtChosenValidation.Rows[intRow]["PackQuantity"].ToString().Trim() == "" ||
                        dtChosenValidation.Rows[intRow]["Maximum Packs"].ToString().Trim() == "")
                    {
                        ErrorProvider.SetError(grdCandidatePacks, SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_NoPackQtyOrMax));
                        fieldsAreValid = false;
                        break;
                    }
                }

            }

            if (chosen == false)
            {
                ErrorProvider.SetError(grdCandidatePacks, SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_NoPackCombo));
                fieldsAreValid = false;
            }

            if (txtReserve.Text.Trim() == "")
            {
                ErrorProvider.SetError(txtReserve, SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_BlankField, lblPercentToReserve.Text.Trim()));
                fieldsAreValid = false;
            }

            if (txtReserveBulk.Text.Trim() == "")
            {
                ErrorProvider.SetError(txtReserveBulk, SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_BlankField, lblPercentToReserveBulk.Text.Trim()));
                fieldsAreValid = false;
            }

            if (txtReservePacks.Text.Trim() == "")
            {
                ErrorProvider.SetError(txtReservePacks, SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_BlankField, lblPercentToReservePacks.Text.Trim()));
                fieldsAreValid = false;
            }

            if (txtAvgPackDeviationTolerance.Text.Trim() != "")
            {
                try
                {
                    if (Convert.ToDouble(txtAvgPackDeviationTolerance.Text) < 0)
                    {
                        ErrorProvider.SetError(txtAvgPackDeviationTolerance, SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_NegNumber, lblAvgPackDeviationTolerance.Text.Trim()));
                        fieldsAreValid = false;
                    }
                }
                catch
                {
                    ErrorProvider.SetError(txtAvgPackDeviationTolerance, SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MustBeNumeric));
                    fieldsAreValid = false;
                }
            }

            if (txtMaxPackAllocationNeedTolerance.Text.Trim() == "")
            {
                ErrorProvider.SetError(txtMaxPackAllocationNeedTolerance, SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_BlankField, lblMaxPackAllocationNeedTolerance.Text.Trim()));
                fieldsAreValid = false;
            }

            if (txtMaxPackAllocationNeedTolerance.Text.Trim() != "")
            {
                try
                {
                    if (Convert.ToDouble(txtMaxPackAllocationNeedTolerance.Text) < 0)
                    {
                        ErrorProvider.SetError(txtMaxPackAllocationNeedTolerance, SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_NegNumber, lblMaxPackAllocationNeedTolerance.Text.Trim()));
                        fieldsAreValid = false;
                    }
                }
                catch
                {
                    ErrorProvider.SetError(txtMaxPackAllocationNeedTolerance, SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MustBeNumeric));
                    fieldsAreValid = false;
                }
            }

            if (txtMaxPackAllocationNeedTolerance.Text.IndexOf(".") >= 0)
            {
                ErrorProvider.SetError(txtMaxPackAllocationNeedTolerance, SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_DecimalNotAllowed, lblMaxPackAllocationNeedTolerance.Text.Trim()));
                fieldsAreValid = false;
            }

            if (txtMaxPackAllocationNeedTolerance.Text.Trim().Length != 0)
            {
                try
                {
                    double wkNeedTolerance = Double.Parse(txtMaxPackAllocationNeedTolerance.Text);
                    if (wkNeedTolerance > 0)
                    {
                        if (chkDepleteReserve.Checked == false
                        && (chkIncreaseBuyQty.Checked == false))
                        {
                            ErrorProvider.SetError(txtMaxPackAllocationNeedTolerance, SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_GreaterThanZeroVar, lblMaxPackAllocationNeedTolerance.Text.Trim()));
                            fieldsAreValid = false;
                        }
                    }
                }
                catch
                {
                    ErrorProvider.SetError(txtMaxPackAllocationNeedTolerance, SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MustBeNumeric));
                    fieldsAreValid = false;
                }
            }
            if (txtIncreaseBuyQty.Text.Trim().Length != 0)
            {
                try
                {
                    double wkincreaseBuyQty = Double.Parse(txtIncreaseBuyQty.Text);
                    if (wkincreaseBuyQty < 0)
                    {
                        ErrorProvider.SetError(txtIncreaseBuyQty, SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_NegNumber, txtIncreaseBuyQty.Text.Trim()));
                        fieldsAreValid = false;
                    }
                }
                catch
                {
                    ErrorProvider.SetError(txtIncreaseBuyQty, SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MustBeNumeric));
                    fieldsAreValid = false;
                }
            }
            if (rdoReservePercent.Checked == true)
            {
                if (txtReserve.Text.Trim() != "")
                {
                    try
                    {
                        if (Convert.ToDouble(txtReserve.Text) < 0 || Convert.ToDouble(txtReserve.Text) > 100)
                        {
                            ErrorProvider.SetError(txtReserve, SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_NegNbrLessThanZero, lblPercentToReserve.Text.Trim()));
                            fieldsAreValid = false;
                        }
                    }
                    catch
                    {
                        ErrorProvider.SetError(txtReserve, SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MustBeNumeric));
                        fieldsAreValid = false;
                    }
                }
            }

            // Begin TT#733 - JSmith - Do not show message if reserve percent is zero
            //if (rdoReservePercent.Checked == true)
            if (rdoReservePercent.Checked == true &&
                txtReserve.Text.Trim() != "" &&
                Convert.ToDouble(txtReserve.Text) > 0)
            // End TT#733
            {
                if (txtReserveBulk.Text.Trim() != "" && txtReservePacks.Text.Trim() != "")
                {
                    double dbReserveBulk = 0;
                    double dbReservePacks = 0;
                    try
                    {
                        dbReserveBulk = Convert.ToDouble(txtReserveBulk.Text);
                    }
                    catch
                    {
                        ErrorProvider.SetError(txtReserveBulk, SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MustBeNumeric));
                        fieldsAreValid = false;
                    }
                    try
                    {
                        dbReservePacks = Convert.ToDouble(txtReservePacks.Text);
                    }
                    catch
                    {
                        ErrorProvider.SetError(txtReservePacks, SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MustBeNumeric));
                        fieldsAreValid = false;
                    }

                    if (dbReserveBulk + dbReservePacks != 100)
                    {
                        ErrorProvider.SetError(txtReserveBulk, SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_FieldMust100));
                        ErrorProvider.SetError(txtReservePacks, SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_FieldMust100));
                        fieldsAreValid = false;
                    }
                }
            }

            if (rdoReserveUnits.Checked == true)
            {
                if (txtReserveBulk.Text.Trim() != "" && txtReservePacks.Text.Trim() != "")
                {
                    double dbReserveBulk = Convert.ToDouble(txtReserveBulk.Text);
                    double dbReservePacks = Convert.ToDouble(txtReservePacks.Text);
                    double dbReserve = Convert.ToDouble(txtReserve.Text);

                    if (dbReserveBulk + dbReservePacks != dbReserve)
                    {
                        ErrorProvider.SetError(txtReserve, SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_FieldMustField, lblPercentToReserveBulk.Text.Trim(), lblPercentToReservePacks.Text.Trim(), lblPercentToReserve.Text.Trim()));
                        ErrorProvider.SetError(txtReserveBulk, SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_FieldMustField, lblPercentToReserveBulk.Text.Trim(), lblPercentToReservePacks.Text.Trim(), lblPercentToReserve.Text.Trim()));
                        ErrorProvider.SetError(txtReservePacks, SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_FieldMustField, lblPercentToReserveBulk.Text.Trim(), lblPercentToReservePacks.Text.Trim(), lblPercentToReserve.Text.Trim()));
                        fieldsAreValid = false;
                    }
                }
            }

            if (fieldsAreValid)
            {
                if (FunctionSecurity.AllowExecute != true)
                {
                    btnProcess.Enabled = false;
                }
            }

            return fieldsAreValid;
        }
        // End TT#733

        private void frmBuildPackMethod_Dispose(object sender, EventArgs e)
        {
            selectedHeaderList = null;
        }

        // BEGIN TT#669 - AGallagher – Build Pack Method – Variance Options
        private void chkIncreaseBuyQty_CheckedChanged(object sender, System.EventArgs e)
        {
            if (chkIncreaseBuyQty.Checked == true)
            {
                _BuildPacksMethod.IncreaseBuySelected = true;
                this.txtIncreaseBuyQty.Enabled = true; 
            }
            else
            {
                _BuildPacksMethod.IncreaseBuySelected = false;
                // begin TT#669 Build Packs Variance Enhancement Data Layer (JEllis)
                //_BuildPacksMethod.IncreaseBuyPct = double.MaxValue; 
                MIDException statusReason;
                if (!_BuildPacksMethod.SetIncreaseBuyPct(double.MaxValue, out statusReason)) 
                {
                    throw new MIDException(statusReason.ErrorLevel, statusReason.ErrorNumber, statusReason.ErrorMessage);
                }
                // end TT#669 Build Packs Variance Enhancement Data Layer (JEllis)

                this.txtIncreaseBuyQty.Enabled = false; 
                this.txtIncreaseBuyQty.Text = null;
            }
        }        
        // END TT#669 - AGallagher – Build Pack Method – Variance Options
        
        private int GetSummary_EvalSortNbr(string propertyName)
        {
            int returnValue = -1;

            // Begin TT#852 - JSmith - Build Packs Labels are not "soft"
            //switch (propertyName.ToUpper())
            //{
            //    case "PACK COMBINATION":
            //        returnValue = 0;
            //        break;
            //    case "SHIP VARIANCE":
            //        returnValue = 1;
            //        break;
            //    case "AVERAGE PACK DEVIATION TOLERANCE":
            //        returnValue = 2;
            //        break;
            //    case "ALL STORE TOTAL BUY":
            //        returnValue = 3;
            //        break;
            //    case "ALL STORE TOTAL NUMBER OF PACKS":
            //        returnValue = 4;
            //        break;
            //    case "PERCENT OF RESERVE IN TOTAL":
            //        returnValue = 5;
            //        break;
            //    case "ALL STORE TOTAL PACK UNITS":
            //        returnValue = 6;
            //        break;
            //    case "PERCENT ALL STORE UNITS IN PACKS":
            //        returnValue = 7;
            //        break;
            //    case "ALL STORE BULK BUY":
            //        returnValue = 8;
            //        break;
            //    case "PERCENT OF BULK TO TOTAL":
            //        returnValue = 9;
            //        break;
            //    case "PERCENT OF BULK IN RESERVE":
            //        returnValue = 10;
            //        break;
            //    case "NON-RESERVE TOTAL BUY":
            //        returnValue = 11;
            //        break;
            //    case "NON-RESERVE TOTAL NUMBER OF PACKS":
            //        returnValue = 12;
            //        break;
            //    case "NON-RESERVE BULK BUY":
            //        returnValue = 13;
            //        break;
            //    case "NON-RESERVE TOTAL PACK UNITS":
            //        returnValue = 14;
            //        break;
            //    case "PERCENT OF NON-RESERVE UNITS IN PACKS":
            //        returnValue = 15;
            //        break;
            //    case "RESERVE TOTAL BUY":
            //        returnValue = 16;
            //        break;
            //    case "RESERVE TOTAL NUMBER OF PACKS":
            //        returnValue = 17;
            //        break;
            //    case "RESERVE BULK BUY":
            //        returnValue = 18;
            //        break;
            //    case "PERCENT OF RESERVE IN BULK":
            //        returnValue = 19;
            //        break;
            //    case "RESERVE TOTAL PACK UNITS":
            //        returnValue = 20;
            //        break;
            //    case "PERCENT OF RESERVE UNITS IN PACKS":
            //        returnValue = 21;
            //        break;
            //    case "SIZES WITH UNITS COUNT":
            //        returnValue = 22;
            //        break;
            //    case "SIZES WITH AT LEAST ONE ERROR COUNT":
            //        returnValue = 23;
            //        break;
            //    case "TOTAL SIZE UNIT ERROR":
            //        returnValue = 24;
            //        break;
            //    case "AVERAGE ERROR PER SIZE WITH UNITS":
            //        returnValue = 25;
            //        break;
            //    case "AVERAGE ERROR PER SIZE IN ERROR":
            //        returnValue = 26;
            //        break;
            //    case "AVERAGE ERROR PER PACK":
            //        returnValue = 27;
            //        break;
            //    case "AVERAGE ERROR PER PACK WITH ERROR":
            //        returnValue = 28;
            //        break;
            //    case "AVERAGE ERROR PER STORE WITH ERROR":
            //        returnValue = 29;
            //        break;
            //    case "AVERAGE ERROR PER STORE WITH PACKS":
            //        returnValue = 30;
            //        break;
            //    case "AVERAGE ERROR PER STORE WITH UNITS":
            //        returnValue = 31;
            //        break;
            //    case "PERCENT OF BULK TOTAL IN RESERVE":
            //        returnValue = 32;
            //        break;
            //    case "COUNT OF ALL STORES WITH PACKS":
            //        returnValue = 33;
            //        break;
            //    case "COUNT OF NON-RESERVE STORES WITH PACKS":
            //        returnValue = 34;
            //        break;
            //    case "COUNT OF ALL STORES WITH UNITS":
            //        returnValue = 35;
            //        break;
            //    case "COUNT OF NON-RESERVE STORES WITH UNITS":
            //        returnValue = 36;
            //        break;
            //    case "COUNT OF ALL STORES WITH BULK":
            //        returnValue = 37;
            //        break;
            //    case "COUNT OF NON-RESERVE STORES WITH BULK":
            //        returnValue = 38;
            //        break;
            //    case "STORES WITH ERROR COUNT":
            //        returnValue = 39;
            //        break;
            //    case "PERCENT OF NON-RESERVE WITH UNITS IN ERROR":
            //        returnValue = 40;
            //        break;
            //    // begin TT#801 - Need additional Pack Select Criteria
            //    case "ORIGINAL BUY PACKAGED UNITS":
            //        returnValue = 41;
            //        break;
            //    case "PERCENT ORIGINAL BUY PACKAGED":
            //        returnValue = 42;
            //        break;
            //    // end TT#801 - Need additional Pack Select Criteria
            //}

            if (propertyName == MIDText.GetTextOnly(eMIDTextCode.pnFromPackPatternComboName))
            {
                returnValue = 0;
            }
            else if (propertyName == MIDText.GetTextOnly(eMIDTextCode.pnShipVariance))
            {
                returnValue = 1;
            }
            else if (propertyName == MIDText.GetTextOnly(eMIDTextCode.pnAvgPackDevTolerance))
            {
                returnValue = 2;
            }
            else if (propertyName == MIDText.GetTextOnly(eMIDTextCode.pnAllStoreTotalBuy))
            {
                returnValue = 3;
            }
            else if (propertyName == MIDText.GetTextOnly(eMIDTextCode.pnAllStoreTotalNumberOfPacks))
            {
                returnValue = 4;
            }
            else if (propertyName == MIDText.GetTextOnly(eMIDTextCode.pnPercentReserveInTotal))
            {
                returnValue = 5;
            }
            else if (propertyName == MIDText.GetTextOnly(eMIDTextCode.pnAllStoreTotalPackUnits))
            {
                returnValue = 6;
            }
            else if (propertyName == MIDText.GetTextOnly(eMIDTextCode.pnPercentAllStoreUnitsInPacks))
            {
                returnValue = 7;
            }
            else if (propertyName == MIDText.GetTextOnly(eMIDTextCode.pnAllStoreBulkBuy))
            {
                returnValue = 8;
            }
            else if (propertyName == MIDText.GetTextOnly(eMIDTextCode.pnPercentBulkToTotal))
            {
                returnValue = 9;
            }
            else if (propertyName == MIDText.GetTextOnly(eMIDTextCode.pnPercentBulkInReserve))
            {
                returnValue = 10;
            }
            else if (propertyName == MIDText.GetTextOnly(eMIDTextCode.pnNonReserveTotalBuy))
            {
                returnValue = 11;
            }
            else if (propertyName == MIDText.GetTextOnly(eMIDTextCode.pnNonReserveTotalNumberOfPacks))
            {
                returnValue = 12;
            }
            else if (propertyName == MIDText.GetTextOnly(eMIDTextCode.pnNonReserveBulkBuy))
            {
                returnValue = 13;
            }
            else if (propertyName == MIDText.GetTextOnly(eMIDTextCode.pnNonReserveTotalPackUnits))
            {
                returnValue = 14;
            }
            else if (propertyName == MIDText.GetTextOnly(eMIDTextCode.pnPercentNonReserveUnitsInPacks))
            {
                returnValue = 15;
            }
            else if (propertyName == MIDText.GetTextOnly(eMIDTextCode.pnReserveTotalBuy))
            {
                returnValue = 16;
            }
            else if (propertyName == MIDText.GetTextOnly(eMIDTextCode.pnReserveTotalNumberOfPacks))
            {
                returnValue = 17;
            }
            else if (propertyName == MIDText.GetTextOnly(eMIDTextCode.pnReserveBulkBuy))
            {
                returnValue = 18;
            }
            else if (propertyName == MIDText.GetTextOnly(eMIDTextCode.pnPercentReserveInBulk))
            {
                returnValue = 19;
            }
            else if (propertyName == MIDText.GetTextOnly(eMIDTextCode.pnReserveTotalPackUnits))
            {
                returnValue = 20;
            }
            else if (propertyName == MIDText.GetTextOnly(eMIDTextCode.pnPercentReserveUnitsInPacks))
            {
                returnValue = 21;
            }
            else if (propertyName == MIDText.GetTextOnly(eMIDTextCode.pnCountSizesWithUnits))
            {
                returnValue = 22;
            }
            else if (propertyName == MIDText.GetTextOnly(eMIDTextCode.pnCountSizesWithAtLeast1Error))
            {
                returnValue = 23;
            }
            else if (propertyName == MIDText.GetTextOnly(eMIDTextCode.pnTotalSizeUnitError))
            {
                returnValue = 24;
            }
            else if (propertyName == MIDText.GetTextOnly(eMIDTextCode.pnAvgErrorPerSizeWithUnits))
            {
                returnValue = 25;
            }
            else if (propertyName == MIDText.GetTextOnly(eMIDTextCode.pnAvgErrorPerSizeInError))
            {
                returnValue = 26;
            }
            else if (propertyName == MIDText.GetTextOnly(eMIDTextCode.pnAvgErrorPerPack))
            {
                returnValue = 27;
            }
            else if (propertyName == MIDText.GetTextOnly(eMIDTextCode.pnAvgErrorPerPackWithError))
            {
                returnValue = 28;
            }
            else if (propertyName == MIDText.GetTextOnly(eMIDTextCode.pnAvgErrorPerStoreWithError))
            {
                returnValue = 29;
            }
            else if (propertyName == MIDText.GetTextOnly(eMIDTextCode.pnAvgErrorPerStoreWithPacks))
            {
                returnValue = 30;
            }
            else if (propertyName == MIDText.GetTextOnly(eMIDTextCode.pnAvgErrorPerStoreWithUnits))
            {
                returnValue = 31;
            }
            else if (propertyName == MIDText.GetTextOnly(eMIDTextCode.pnPercentBulkTotalInReserve))
            {
                returnValue = 32;
            }
            else if (propertyName == MIDText.GetTextOnly(eMIDTextCode.pnCountOfAllStoresWithPacks))
            {
                returnValue = 33;
            }
            else if (propertyName == MIDText.GetTextOnly(eMIDTextCode.pnCountOfNonReserveStoresWithPacks))
            {
                returnValue = 34;
            }
            else if (propertyName == MIDText.GetTextOnly(eMIDTextCode.pnCountOfAllStoresWithUnits))
            {
                returnValue = 35;
            }
            else if (propertyName == MIDText.GetTextOnly(eMIDTextCode.pnCountOfNonReserveStoresWithUnits))
            {
                returnValue = 36;
            }
            else if (propertyName == MIDText.GetTextOnly(eMIDTextCode.pnCountOfAllStoresWithBulk))
            {
                returnValue = 37;
            }
            else if (propertyName == MIDText.GetTextOnly(eMIDTextCode.pnCountOfNonReserveStoresWithBulk))
            {
                returnValue = 38;
            }
            else if (propertyName == MIDText.GetTextOnly(eMIDTextCode.pnStoresWithErrorCount))
            {
                returnValue = 39;
            }
            else if (propertyName == MIDText.GetTextOnly(eMIDTextCode.pnPercentNonReserveWithUnitsInError))
            {
                returnValue = 40;
            }
            else if (propertyName == MIDText.GetTextOnly(eMIDTextCode.pnOriginalBuyPackUnits))
            {
                returnValue = 41;
            }
            else if (propertyName == MIDText.GetTextOnly(eMIDTextCode.pnPercentOriginalBuyPackaged))
            {
                returnValue = 42;
            }
            // End TT#852 - JSmith - Build Packs Labels are not "soft"

            return returnValue;
        }

        protected override void Dispose(bool disposing)
        {
            // Begin TT#681 - JSmith - Build Packs Method
            if (disposing && (components != null))
			{
				components.Dispose();
			}

            if (disposing)
            {
                // Begin TT#301-MD - JSmith - Controls are not functioning properly
                //this.cboSizeGroup.SelectionChangeCommitted -= new System.EventHandler(this.cboSizeGroup_SelectionChangeCommitted);
                // End TT#301-MD - JSmith - Controls are not functioning properly
                this.cboSizeGroup.DropDownClosed -= new System.EventHandler(this.cboSizeGroup_DropDownClosed);
                this.cboSizeGroup.DropDown -= new System.EventHandler(this.cboSizeGroup_DropDown);
                this.cboSizeGroup.Click -= new System.EventHandler(this.cboSizeGroup_Click);
                // Begin TT#301-MD - JSmith - Controls are not functioning properly
                //this.cboSizeCurve.SelectionChangeCommitted -= new System.EventHandler(this.cboSizeCurve_SelectionChangeCommitted);
                // End TT#301-MD - JSmith - Controls are not functioning properly
                this.cboSizeCurve.DropDownClosed -= new System.EventHandler(this.cboSizeCurve_DropDownClosed);
                this.cboSizeCurve.DropDown -= new System.EventHandler(this.cboSizeCurve_DropDown);
                this.cboSizeCurve.Click -= new System.EventHandler(this.cboSizeCurve_Click);
                //this.tabCriteria.Resize -= new System.EventHandler(this.tabCriteria_Resize);
                this.mnuPackCombinationAdd.Click -= new System.EventHandler(this.mnuPackCombinationAdd_Click);
                this.mnuPackCombinationAddPack.Click -= new System.EventHandler(this.mnuPackCombinationAddPack_Click);
                this.mnuPackCombinationDelete.Click -= new System.EventHandler(this.mnuPackCombinationDelete_Click);
                this.btnGetHeader.Click -= new System.EventHandler(this.btnGetHeader_Click);
                this.cmbVendor.SelectionChangeCommitted -= new System.EventHandler(this.cmbVendor_SelectionChangeCommitted);
                //Begin TT#316 - MD - DOConnell - Replace all Windows Combobox controls with new enhanced control
                this.cmbVendor.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler(cmbVendor_MIDComboBoxPropertiesChangedEvent);
                //End TT#316 - MD - DOConnell - Replace all Windows Combobox controls with new enhanced control
                //this.tabConstraints.Resize -= new System.EventHandler(this.tabConstraints_Resize);
                //this.tabEvaluation.Resize -= new System.EventHandler(this.tabEvaluation_Resize);
                this.cmbEvalOptions.SelectionChangeCommitted -= new System.EventHandler(this.cmbEvalOptions_SelectionChangeCommitted);
                //Begin TT#316 - MD - DOConnell - Replace all Windows Combobox controls with new enhanced control
                this.cmbEvalOptions.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler(cmbEvalOptions_MIDComboBoxPropertiesChangedEvent);
                //End TT#316 - MD - DOConnell - Replace all Windows Combobox controls with new enhanced control
                this.btnApply.Click -= new System.EventHandler(this.btnApply_Click);
                this.Load -= new System.EventHandler(this.frmBuildPacksMethod_Load);
                this.Disposed -= new System.EventHandler(this.frmBuildPackMethod_Dispose);
                //this.Resize -= new System.EventHandler(this.frmBuildPacksMethod_Resize);
            }

            base.Dispose(disposing);
            // End TT#681
        }

        bool unitsChecked = false;
        bool percentsChecked = false;
        private void CheckUnits()
        {
            rdoReserveUnits.Checked = true;
            rdoReserveBulkUnits.Checked = true;
            rdoReservePacksUnits.Checked = true;
            rdoReservePacksPercent.Checked = false;
            rdoReserveBulkPercent.Checked = false;
            rdoReservePercent.Checked = false;
            unitsChecked = true;
            percentsChecked = false;
        }

        private void CheckPercents()
        {
            rdoReserveUnits.Checked = false;
            rdoReserveBulkUnits.Checked = false;
            rdoReservePacksUnits.Checked = false;
            rdoReservePacksPercent.Checked = true;
            rdoReserveBulkPercent.Checked = true;
            rdoReservePercent.Checked = true;
            unitsChecked = false;
            percentsChecked = true;
        }

        private void rdoReserveUnits_CheckedChanged(object sender, EventArgs e)
        {
            if (unitsChecked == false)
            {
                CheckUnits();
            }
        }

        private void rdoReserveBulkUnits_CheckedChanged(object sender, EventArgs e)
        {
            if (unitsChecked == false)
            {
                CheckUnits();
            }
        }

        private void rdoReservePacksUnits_CheckedChanged(object sender, EventArgs e)
        {
            if (unitsChecked == false)
            {
                CheckUnits();
            }
        }

        private void rdoReservePacksPercent_CheckedChanged(object sender, EventArgs e)
        {
            if (percentsChecked == false)
            {
                CheckPercents();
            }
        }

        private void rdoReserveBulkPercent_CheckedChanged(object sender, EventArgs e)
        {
            if (percentsChecked == false)
            {
                CheckPercents();
            }
        }

        private void rdoReservePercent_CheckedChanged(object sender, EventArgs e)
        {
            if (percentsChecked == false)
            {
                CheckPercents();
            }
        }

        private void lblVendorPackOrderMin_Click(object sender, EventArgs e) // TT#787 Vendor Min Order applies only to packs
        {

        }

        // Begin TT#289-MD - JSmith - Build Pack method-> processes but fails to apply the pack to the header
        override protected void BeforeClosing()
        {
            try
            {
                ApplicationTransaction.DequeueHeaders();
            }
            catch (Exception exception)
            {
                HandleException(exception);
            }
        }
        // End TT#289-MD - JSmith - Build Pack method-> processes but fails to apply the pack to the header

        // Begin TT#3152 - JSmith - Build Pack Method-> Unhandled Exception when delete Pack Combination row and then attempt to add pack row to existing Pack Combination
        private void mnuPackCombination_Opening(object sender, CancelEventArgs e)
        {
            if (grdCandidatePacks.ActiveRow == null)
            {
                mnuPackCombinationAddPack.Enabled = false;
                mnuPackCombinationDelete.Enabled = false;
            }
            else
            {
                mnuPackCombinationAddPack.Enabled = true;
                mnuPackCombinationDelete.Enabled = true;
            }
        }
        // End TT#3152 - JSmith - Build Pack Method-> Unhandled Exception when delete Pack Combination row and then attempt to add pack row to existing Pack Combination
    }
}

