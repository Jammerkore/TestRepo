using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Globalization;
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
	/// Summary description for CopyForecastMethod.
	/// </summary>
	public class frmRollupMethod : MIDRetail.Windows.WorkflowMethodFormBase
	{
		#region Fields

		private Bitmap _picInclude;
		private Bitmap _picExclude;
		private Image _dynamicToPlanImage;
		private Image _dynamicToCurrentImage;
		//private ProfileList _versionProfList;
        private MIDRetail.Business.OTSRollupMethod _OTSRollupMethod = null;
		private HierarchyNodeSecurityProfile _hierNodeSecurity;
        private FunctionSecurityProfile _filterUserSecurity;
        private FunctionSecurityProfile _filterGlobalSecurity;
        private System.Data.DataSet _dsRollup;
        //private System.Data.DataTable _dtRollupVersions;
		private System.Data.DataTable _dtBasis;
		private ArrayList _userRIDList;
		//private StoreFilterData _storeFilterDL;
		private ProfileList _variables;
		private int _nodeRID = Include.NoRID;
		private int _prevAttributeValue;
		private int _prevSetValue;
		private ePlanType _planType;
//		private eMethodType _methodType;
		private bool _attributeReset = false;
		private bool _attributeChanged = false;
		private bool _setReset = false;
		private bool _skipAfterCellUpdate = false;
		private bool _basisNodeRequired = false;
		private string _thisTitle;
		private bool _needsValidated = false;
		private int _highLevelWeekCount = 0;		// MID Track #5149 - error when basis weeks > high level weeks
        private ArrayList _fromLevelList;
        private ArrayList _toLevelList;
        private bool _atLeastOneCheck = false;

		#endregion

		private System.Windows.Forms.TabPage tabMethod;
		private System.Windows.Forms.Label lblTimePeriod;
        private System.Windows.Forms.TabPage tabProperties;
        private System.Windows.Forms.TabControl tabSpreadMethod;
		private System.Windows.Forms.Label lblVersion;
		private System.Windows.Forms.Label lblMerchandise;
        private System.Windows.Forms.TextBox txtSpreadNode;
        private MIDRetail.Windows.Controls.MIDDateRangeSelector mdsPlanDateRange;
		private Infragistics.Win.UltraWinGrid.UltraGrid grdRollup;
		private System.Windows.Forms.ContextMenu mnuBasisGrid;
		private System.Windows.Forms.ToolTip toolTip1;
		private System.Windows.Forms.ImageList Icons;
        private System.Windows.Forms.GroupBox grpOptions;
        private ProfileList _versionProfList;
        //private HierarchyNodeList _hierChildList;
        //private DataTable _dtLowerLevels;
        private int _parentVersion;
        private GroupBox grpRollup;
        private GroupBox grpLastProcessed;
        private Label lblLastProcessed;
        private Label lblByUserValue;
        private Label lblByUser;
        private Label lblDateTimeValue;
        private Label lblDateTime;
        private UltraGrid ugWorkflows;
        private MIDComboBoxEnh cboSpreadVersion;

		private System.ComponentModel.IContainer components = null;

		public frmRollupMethod(SessionAddressBlock SAB, ExplorerAddressBlock aEAB) : base (SAB, aEAB, eMIDTextCode.frm_Rollup, eWorkflowMethodType.Method)
		{
			try
			{
				InitializeComponent();

                UserSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ForecastMethodsUserRollup);
                GlobalSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ForecastMethodsGlobalRollup);

                _fromLevelList = new ArrayList();
                _toLevelList = new ArrayList();
			}
			catch(Exception ex)
			{
                HandleException(ex, "NewOTSRollupMethod");
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
				if (components != null) 
				{
					components.Dispose();
				}
				// Begin MID Track 4858 - JSmith - Security changes
//				this.radGlobal.CheckedChanged -= new System.EventHandler(this.radGlobal_CheckedChanged);
//				this.radUser.CheckedChanged -= new System.EventHandler(this.radUser_CheckedChanged);
//				this.btnProcess.Click -= new System.EventHandler(this.btnProcess_Click);
//				this.btnSave.Click -= new System.EventHandler(this.btnSave_Click);
//				this.btnClose.Click -= new System.EventHandler(this.btnClose_Click);

                this.cboSpreadVersion.SelectionChangeCommitted -= new System.EventHandler(this.cboSpreadVersion_SelectionChangeCommitted); //TT#7 - RBeck - Dynamic dropdowns

				// End MID Track 4858
                this.tabSpreadMethod.SelectedIndexChanged -= new System.EventHandler(this.tabSpreadMethod_SelectedIndexChanged);
				//this.txtSpreadNode.KeyPress -= new System.Windows.Forms.KeyPressEventHandler(this.txtSpreadNode_KeyPress);
//				this.txtName.TextChanged -= new System.EventHandler(this.txtName_TextChanged);
//				this.txtDesc.TextChanged -= new System.EventHandler(this.txtDesc_TextChanged);
				this.txtSpreadNode.TextChanged -= new System.EventHandler(this.txtSpreadNode_TextChanged);
				this.txtSpreadNode.KeyDown -= new System.Windows.Forms.KeyEventHandler(this.txtSpreadNode_KeyDown);
                this.txtSpreadNode.Leave -= new System.EventHandler(this.txtSpreadNode_Leave);
				this.txtSpreadNode.Validating -= new System.ComponentModel.CancelEventHandler(this.txtSpreadNode_Validating);
                this.txtSpreadNode.Validated -= new System.EventHandler(this.txtSpreadNode_Validated);
				this.txtSpreadNode.DragDrop -= new System.Windows.Forms.DragEventHandler(this.txtSpreadNode_DragDrop);
				this.txtSpreadNode.DragEnter -= new System.Windows.Forms.DragEventHandler(this.txtSpreadNode_DragEnter);
                this.cboSpreadVersion.SelectionChangeCommitted -= new System.EventHandler(this.cboSpreadVersion_SelectionChangeCommitted);
                this.cboSpreadVersion.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler(cboSpreadVersion_MIDComboBoxPropertiesChangedEvent);
                this.grdRollup.InitializeRow -= new Infragistics.Win.UltraWinGrid.InitializeRowEventHandler(this.grdRollup_InitializeRow);
				this.grdRollup.MouseEnterElement -= new Infragistics.Win.UIElementEventHandler(this.grdRollup_MouseEnterElement);
				this.grdRollup.DragDrop -= new System.Windows.Forms.DragEventHandler(this.grdRollup_DragDrop);
				this.grdRollup.AfterRowInsert -= new Infragistics.Win.UltraWinGrid.RowEventHandler(this.grdRollup_AfterRowInsert);
				this.grdRollup.DragOver -= new System.Windows.Forms.DragEventHandler(this.grdRollup_DragOver);
				this.grdRollup.ClickCellButton -= new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdRollup_ClickCellButton);
				this.grdRollup.AfterCellUpdate -= new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdRollup_AfterCellUpdate);
				this.grdRollup.InitializeLayout -= new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.grdRollup_InitializeLayout);
                //Begin TT#169 - JSmith - enter max of 1000 and receive 'must be integer'
                MIDRetail.Windows.Controls.UltraGridLayoutDefaults ugld = new MIDRetail.Windows.Controls.UltraGridLayoutDefaults(ErrorImage);
                ugld.DetachGridEventHandlers(grdRollup);
                //End TT#169
                this.grdRollup.AfterCellListCloseUp -= new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdRollup_AfterCellListCloseUp);
				this.mdsPlanDateRange.Click -= new System.EventHandler(this.mdsPlanDateRange_Click);
				this.mdsPlanDateRange.OnSelection -= new MIDRetail.Windows.Controls.MIDDateRangeSelector.SelectionEventHandler(this.mdsPlanDateRange_OnSelection);
                this.txtSpreadNode.DragOver -= new System.Windows.Forms.DragEventHandler(this.txtSpreadNode_DragOver);
            }
			base.Dispose( disposing );
		}

		#region Designer generated code
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
            this.tabSpreadMethod = new System.Windows.Forms.TabControl();
            this.tabMethod = new System.Windows.Forms.TabPage();
            this.grpOptions = new System.Windows.Forms.GroupBox();
            this.grdRollup = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.mnuBasisGrid = new System.Windows.Forms.ContextMenu();
            this.grpRollup = new System.Windows.Forms.GroupBox();
            this.cboSpreadVersion = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this.mdsPlanDateRange = new MIDRetail.Windows.Controls.MIDDateRangeSelector();
            this.lblMerchandise = new System.Windows.Forms.Label();
            this.txtSpreadNode = new System.Windows.Forms.TextBox();
            this.lblVersion = new System.Windows.Forms.Label();
            this.lblTimePeriod = new System.Windows.Forms.Label();
            this.tabProperties = new System.Windows.Forms.TabPage();
            this.ugWorkflows = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.grpLastProcessed = new System.Windows.Forms.GroupBox();
            this.lblLastProcessed = new System.Windows.Forms.Label();
            this.lblByUserValue = new System.Windows.Forms.Label();
            this.lblByUser = new System.Windows.Forms.Label();
            this.lblDateTimeValue = new System.Windows.Forms.Label();
            this.lblDateTime = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.Icons = new System.Windows.Forms.ImageList(this.components);
            this.pnlGlobalUser.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.utmMain)).BeginInit();
            this.tabSpreadMethod.SuspendLayout();
            this.tabMethod.SuspendLayout();
            this.grpOptions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdRollup)).BeginInit();
            this.grpRollup.SuspendLayout();
            this.tabProperties.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ugWorkflows)).BeginInit();
            this.grpLastProcessed.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(629, 540);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(539, 540);
            // 
            // btnProcess
            // 
            this.btnProcess.Location = new System.Drawing.Point(17, 540);
            // 
            // utmMain
            // 
            this.utmMain.MenuSettings.ForceSerialization = true;
            this.utmMain.ToolbarSettings.ForceSerialization = true;
            // 
            // tabSpreadMethod
            // 
            this.tabSpreadMethod.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabSpreadMethod.Controls.Add(this.tabMethod);
            this.tabSpreadMethod.Controls.Add(this.tabProperties);
            this.tabSpreadMethod.Location = new System.Drawing.Point(16, 56);
            this.tabSpreadMethod.Name = "tabSpreadMethod";
            this.tabSpreadMethod.SelectedIndex = 0;
            this.tabSpreadMethod.Size = new System.Drawing.Size(688, 474);
            this.tabSpreadMethod.TabIndex = 18;
            this.tabSpreadMethod.SelectedIndexChanged += new System.EventHandler(this.tabSpreadMethod_SelectedIndexChanged);
            // 
            // tabMethod
            // 
            this.tabMethod.Controls.Add(this.grpOptions);
            this.tabMethod.Controls.Add(this.grpRollup);
            this.tabMethod.Location = new System.Drawing.Point(4, 22);
            this.tabMethod.Name = "tabMethod";
            this.tabMethod.Size = new System.Drawing.Size(680, 448);
            this.tabMethod.TabIndex = 1;
            this.tabMethod.Text = "Method";
            // 
            // grpOptions
            // 
            this.grpOptions.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpOptions.Controls.Add(this.grdRollup);
            this.grpOptions.Location = new System.Drawing.Point(8, 145);
            this.grpOptions.Name = "grpOptions";
            this.grpOptions.Size = new System.Drawing.Size(666, 294);
            this.grpOptions.TabIndex = 10;
            this.grpOptions.TabStop = false;
            this.grpOptions.Text = "Options";
            // 
            // grdRollup
            // 
            this.grdRollup.AllowDrop = true;
            this.grdRollup.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grdRollup.ContextMenu = this.mnuBasisGrid;
            appearance1.BackColor = System.Drawing.Color.White;
            appearance1.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance1.BackGradientStyle = Infragistics.Win.GradientStyle.ForwardDiagonal;
            this.grdRollup.DisplayLayout.Appearance = appearance1;
            this.grdRollup.DisplayLayout.InterBandSpacing = 10;
            appearance2.BackColor = System.Drawing.Color.Transparent;
            this.grdRollup.DisplayLayout.Override.CardAreaAppearance = appearance2;
            appearance3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance3.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance3.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance3.ForeColor = System.Drawing.Color.Black;
            appearance3.TextHAlignAsString = "Left";
            appearance3.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            this.grdRollup.DisplayLayout.Override.HeaderAppearance = appearance3;
            appearance4.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.grdRollup.DisplayLayout.Override.RowAppearance = appearance4;
            appearance5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance5.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance5.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            this.grdRollup.DisplayLayout.Override.RowSelectorAppearance = appearance5;
            this.grdRollup.DisplayLayout.Override.RowSelectorWidth = 12;
            this.grdRollup.DisplayLayout.Override.RowSpacingBefore = 2;
            appearance6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance6.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance6.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance6.ForeColor = System.Drawing.Color.Black;
            this.grdRollup.DisplayLayout.Override.SelectedRowAppearance = appearance6;
            this.grdRollup.DisplayLayout.RowConnectorColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.grdRollup.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.Solid;
            this.grdRollup.Location = new System.Drawing.Point(10, 19);
            this.grdRollup.Name = "grdRollup";
            this.grdRollup.Size = new System.Drawing.Size(644, 263);
            this.grdRollup.TabIndex = 0;
            this.grdRollup.AfterCellUpdate += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdRollup_AfterCellUpdate);
            this.grdRollup.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.grdRollup_InitializeLayout);
            this.grdRollup.InitializeRow += new Infragistics.Win.UltraWinGrid.InitializeRowEventHandler(this.grdRollup_InitializeRow);
            this.grdRollup.AfterRowInsert += new Infragistics.Win.UltraWinGrid.RowEventHandler(this.grdRollup_AfterRowInsert);
            this.grdRollup.CellChange += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdRollup_CellChange);
            this.grdRollup.ClickCellButton += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdRollup_ClickCellButton);
            this.grdRollup.AfterCellListCloseUp += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdRollup_AfterCellListCloseUp);
            this.grdRollup.MouseEnterElement += new Infragistics.Win.UIElementEventHandler(this.grdRollup_MouseEnterElement);
            this.grdRollup.DragDrop += new System.Windows.Forms.DragEventHandler(this.grdRollup_DragDrop);
            this.grdRollup.DragOver += new System.Windows.Forms.DragEventHandler(this.grdRollup_DragOver);
            this.grdRollup.MouseUp += new System.Windows.Forms.MouseEventHandler(this.grdRollup_MouseUp);
            // 
            // grpRollup
            // 
            this.grpRollup.Controls.Add(this.cboSpreadVersion);
            this.grpRollup.Controls.Add(this.mdsPlanDateRange);
            this.grpRollup.Controls.Add(this.lblMerchandise);
            this.grpRollup.Controls.Add(this.txtSpreadNode);
            this.grpRollup.Controls.Add(this.lblVersion);
            this.grpRollup.Controls.Add(this.lblTimePeriod);
            this.grpRollup.Location = new System.Drawing.Point(8, 8);
            this.grpRollup.Name = "grpRollup";
            this.grpRollup.Size = new System.Drawing.Size(666, 131);
            this.grpRollup.TabIndex = 4;
            this.grpRollup.TabStop = false;
            this.grpRollup.Text = "Rollup";
            // 
            // cboSpreadVersion
            // 
            this.cboSpreadVersion.AutoAdjust = true;
            this.cboSpreadVersion.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboSpreadVersion.DataSource = null;
            this.cboSpreadVersion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSpreadVersion.DropDownWidth = 224;
            this.cboSpreadVersion.Location = new System.Drawing.Point(97, 61);
            this.cboSpreadVersion.Margin = new System.Windows.Forms.Padding(0);
            this.cboSpreadVersion.Name = "cboSpreadVersion";
            this.cboSpreadVersion.Size = new System.Drawing.Size(224, 21);
            this.cboSpreadVersion.TabIndex = 7;
            this.cboSpreadVersion.Tag = null;
            this.cboSpreadVersion.SelectionChangeCommitted += new System.EventHandler(this.cboSpreadVersion_SelectionChangeCommitted);
            this.cboSpreadVersion.MIDComboBoxPropertiesChangedEvent += new MIDComboBoxPropertiesChangedEventHandler(cboSpreadVersion_MIDComboBoxPropertiesChangedEvent);
            // 
            // mdsPlanDateRange
            // 
            this.mdsPlanDateRange.DateRangeForm = null;
            this.mdsPlanDateRange.DateRangeRID = 0;
            this.mdsPlanDateRange.Enabled = false;
            this.mdsPlanDateRange.Location = new System.Drawing.Point(97, 95);
            this.mdsPlanDateRange.Name = "mdsPlanDateRange";
            this.mdsPlanDateRange.Size = new System.Drawing.Size(224, 24);
            this.mdsPlanDateRange.TabIndex = 19;
            this.mdsPlanDateRange.OnSelection += new MIDRetail.Windows.Controls.MIDDateRangeSelector.SelectionEventHandler(this.mdsPlanDateRange_OnSelection);
            this.mdsPlanDateRange.Click += new System.EventHandler(this.mdsPlanDateRange_Click);
            // 
            // lblMerchandise
            // 
            this.lblMerchandise.Location = new System.Drawing.Point(14, 35);
            this.lblMerchandise.Name = "lblMerchandise";
            this.lblMerchandise.Size = new System.Drawing.Size(72, 16);
            this.lblMerchandise.TabIndex = 18;
            this.lblMerchandise.Text = "Merchandise";
            this.lblMerchandise.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtSpreadNode
            // 
            this.txtSpreadNode.AllowDrop = true;
            this.txtSpreadNode.Location = new System.Drawing.Point(97, 30);
            this.txtSpreadNode.Name = "txtSpreadNode";
            this.txtSpreadNode.Size = new System.Drawing.Size(224, 20);
            this.txtSpreadNode.TabIndex = 1;
            this.txtSpreadNode.TextChanged += new System.EventHandler(this.txtSpreadNode_TextChanged);
            this.txtSpreadNode.DragDrop += new System.Windows.Forms.DragEventHandler(this.txtSpreadNode_DragDrop);
            this.txtSpreadNode.DragEnter += new System.Windows.Forms.DragEventHandler(this.txtSpreadNode_DragEnter);
            this.txtSpreadNode.DragOver += new System.Windows.Forms.DragEventHandler(this.txtSpreadNode_DragOver);
            this.txtSpreadNode.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSpreadNode_KeyDown);
            this.txtSpreadNode.Leave += new System.EventHandler(this.txtSpreadNode_Leave);
            this.txtSpreadNode.Validating += new System.ComponentModel.CancelEventHandler(this.txtSpreadNode_Validating);
            this.txtSpreadNode.Validated += new System.EventHandler(this.txtSpreadNode_Validated);
            // 
            // lblVersion
            // 
            this.lblVersion.Location = new System.Drawing.Point(14, 67);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(48, 16);
            this.lblVersion.TabIndex = 6;
            this.lblVersion.Text = "Version:";
            this.lblVersion.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblTimePeriod
            // 
            this.lblTimePeriod.Location = new System.Drawing.Point(14, 100);
            this.lblTimePeriod.Name = "lblTimePeriod";
            this.lblTimePeriod.Size = new System.Drawing.Size(72, 16);
            this.lblTimePeriod.TabIndex = 2;
            this.lblTimePeriod.Text = "Time Period:";
            this.lblTimePeriod.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tabProperties
            // 
            this.tabProperties.Controls.Add(this.ugWorkflows);
            this.tabProperties.Controls.Add(this.grpLastProcessed);
            this.tabProperties.Location = new System.Drawing.Point(4, 22);
            this.tabProperties.Name = "tabProperties";
            this.tabProperties.Size = new System.Drawing.Size(680, 448);
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
            this.ugWorkflows.Location = new System.Drawing.Point(32, 9);
            this.ugWorkflows.Name = "ugWorkflows";
            this.ugWorkflows.Size = new System.Drawing.Size(616, 367);
            this.ugWorkflows.TabIndex = 18;
            // 
            // grpLastProcessed
            // 
            this.grpLastProcessed.Controls.Add(this.lblLastProcessed);
            this.grpLastProcessed.Controls.Add(this.lblByUserValue);
            this.grpLastProcessed.Controls.Add(this.lblByUser);
            this.grpLastProcessed.Controls.Add(this.lblDateTimeValue);
            this.grpLastProcessed.Controls.Add(this.lblDateTime);
            this.grpLastProcessed.Location = new System.Drawing.Point(12, 382);
            this.grpLastProcessed.Name = "grpLastProcessed";
            this.grpLastProcessed.Size = new System.Drawing.Size(660, 59);
            this.grpLastProcessed.TabIndex = 17;
            this.grpLastProcessed.TabStop = false;
            // 
            // lblLastProcessed
            // 
            this.lblLastProcessed.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLastProcessed.Location = new System.Drawing.Point(36, 11);
            this.lblLastProcessed.Name = "lblLastProcessed";
            this.lblLastProcessed.Size = new System.Drawing.Size(113, 16);
            this.lblLastProcessed.TabIndex = 17;
            this.lblLastProcessed.Text = "Last Processed:";
            // 
            // lblByUserValue
            // 
            this.lblByUserValue.Location = new System.Drawing.Point(397, 34);
            this.lblByUserValue.Name = "lblByUserValue";
            this.lblByUserValue.Size = new System.Drawing.Size(197, 15);
            this.lblByUserValue.TabIndex = 16;
            this.lblByUserValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblByUser
            // 
            this.lblByUser.Location = new System.Drawing.Point(341, 34);
            this.lblByUser.Name = "lblByUser";
            this.lblByUser.Size = new System.Drawing.Size(50, 15);
            this.lblByUser.TabIndex = 15;
            this.lblByUser.Text = "By User:";
            this.lblByUser.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblDateTimeValue
            // 
            this.lblDateTimeValue.Location = new System.Drawing.Point(104, 34);
            this.lblDateTimeValue.Name = "lblDateTimeValue";
            this.lblDateTimeValue.Size = new System.Drawing.Size(197, 15);
            this.lblDateTimeValue.TabIndex = 14;
            this.lblDateTimeValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblDateTime
            // 
            this.lblDateTime.Location = new System.Drawing.Point(36, 34);
            this.lblDateTime.Name = "lblDateTime";
            this.lblDateTime.Size = new System.Drawing.Size(62, 15);
            this.lblDateTime.TabIndex = 13;
            this.lblDateTime.Text = "Date/Time:";
            this.lblDateTime.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Icons
            // 
            this.Icons.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.Icons.ImageSize = new System.Drawing.Size(16, 16);
            this.Icons.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // frmRollupMethod
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(718, 572);
            this.Controls.Add(this.tabSpreadMethod);
            this.Name = "frmRollupMethod";
            this.Text = "Rollup";
            this.Controls.SetChildIndex(this.pnlGlobalUser, 0);
            this.Controls.SetChildIndex(this.lblName, 0);
            this.Controls.SetChildIndex(this.txtName, 0);
            this.Controls.SetChildIndex(this.txtDesc, 0);
            this.Controls.SetChildIndex(this.tabSpreadMethod, 0);
            this.Controls.SetChildIndex(this.btnSave, 0);
            this.Controls.SetChildIndex(this.btnClose, 0);
            this.Controls.SetChildIndex(this.btnProcess, 0);
            this.pnlGlobalUser.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.utmMain)).EndInit();
            this.tabSpreadMethod.ResumeLayout(false);
            this.tabMethod.ResumeLayout(false);
            this.grpOptions.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdRollup)).EndInit();
            this.grpRollup.ResumeLayout(false);
            this.grpRollup.PerformLayout();
            this.tabProperties.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ugWorkflows)).EndInit();
            this.grpLastProcessed.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		/// <summary>
		/// Create a new Rollup
		/// </summary>
		override public void NewWorkflowMethod(MIDWorkflowMethodTreeNode aParentNode)
		{
			try
			{
                _OTSRollupMethod = new OTSRollupMethod(SAB, Include.NoRID);
                ABM = _OTSRollupMethod;
				base.NewWorkflowMethod(aParentNode, eSecurityFunctions.ForecastMethodsUserRollup, eSecurityFunctions.ForecastMethodsGlobalRollup);

				_parentVersion = Include.NoRID;  // Issue 3801 - stodd
				
				Common_Load();

			}
			catch(Exception ex)
			{
                HandleException(ex, "NewOTSRollupMethod");
				FormLoadError = true;
			}
		}

		/// <summary>
        /// Opens an existing Matrix Method. //Eventually combine with NewOTSMultiLevelCopyMethod method
		/// 		/// Seperate for debugging & initial development
		/// </summary>
		/// <param name="aMethodRID">method_RID</param>
		/// <param name="aLockStatus">The lock status of the data to be displayed</param>
		override public void UpdateWorkflowMethod(int aMethodRID, int aNodeRID, MIDWorkflowMethodTreeNode aNode, eLockStatus aLockStatus)
		{
			try
			{
                _OTSRollupMethod = new OTSRollupMethod(SAB, aMethodRID);
                base.UpdateWorkflowMethod(aLockStatus, eSecurityFunctions.ForecastMethodsUserRollup, eSecurityFunctions.ForecastMethodsGlobalRollup);

                _parentVersion = _OTSRollupMethod.VersionRID;  // Issue 4008 - stodd

				Common_Load();
			}
			catch(Exception ex)
			{
                HandleException(ex, "InitializeOTSRollupMethod");
				FormLoadError = true;
			}
		}

		/// <summary>
        /// Deletes a Rollup Method.
		/// </summary>
		/// <param name="aMethodRID">The record ID of the method</param>
		override public bool DeleteWorkflowMethod(int aMethodRID)
		{
			try
			{
                _OTSRollupMethod = new OTSRollupMethod(SAB, aMethodRID);
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
        /// Renames an Rollup Method.
		/// </summary>
		/// <param name="aMethodRID">The record ID of the method</param>
		/// <param name="aNewName">The new name of the workflow or method</param>
		override public bool RenameWorkflowMethod(int aMethodRID, string aNewName)
		{
			try
			{
                _OTSRollupMethod = new OTSRollupMethod(SAB, aMethodRID);
				return Rename(aNewName);
			}
			catch (Exception err)
			{
				HandleException(err);
				FormLoadError = true;
			}
			return false;
		}
		
		private void SetText()
		{
			try
			{
				this.lblName.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Method) + ":";
				this.btnSave.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_Save);
				this.btnClose.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_Close);
				this.btnProcess.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_Process);
                this.grpRollup.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Rollup);
				this.lblMerchandise.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Merchandise) + ":";
				this.lblVersion.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Version) + ":";
				this.lblTimePeriod.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_PlanTimePeriod) + ":";	
				this.tabMethod.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Method);
				this.tabProperties.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Properties);
                this.grpOptions.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_OptionsForm);
            }
			catch( Exception exception )
			{
				HandleException(exception);
			}
		}
		private void Common_Load()
		{
			try
			{
				//Icon = MIDGraphics.GetIcon(MIDGraphics.CopyImage);
				//_storeFilterDL = new StoreFilterData();
                _versionProfList = SAB.ClientServerSession.GetUserForecastVersions();
				_variables = SAB.ApplicationServerSession.DefaultPlanComputations.PlanVariables.VariableProfileList;

				SetText();

				eMIDTextCode textCode =  eMIDTextCode.frm_Rollup;
	
				_thisTitle = MIDText.GetTextOnly((int)textCode);
                if (_OTSRollupMethod.Method_Change_Type == eChangeType.add)
				{
 					Format_Title(eDataState.New, textCode, null);
				}
				else
				{
					if (FunctionSecurity.AllowUpdate)
					{
                        Format_Title(eDataState.Updatable, textCode, _OTSRollupMethod.Name);
					}
					else
					{
                        Format_Title(eDataState.ReadOnly, textCode, _OTSRollupMethod.Name);
					}

                    lblDateTimeValue.Text = _OTSRollupMethod.LastProcessedDateTime;
                    lblByUserValue.Text = _OTSRollupMethod.LastProcessedUser;

				}

//				if (FunctionSecurity.AllowExecute)
//				{
//					btnProcess.Enabled = true;
//				}
//				else
//				{
//					btnProcess.Enabled = false;
//				}

				//SetReadOnly(FunctionSecurity.AllowUpdate);  //Security changes - 1/24/2005 vg

				_picInclude = new Bitmap(GraphicsDirectory + "\\include.gif");
				_picExclude = new Bitmap(GraphicsDirectory + "\\exclude.gif");
				_dynamicToPlanImage = Image.FromFile(GraphicsDirectory + "\\" + MIDGraphics.DynamicToPlanImage);
				_dynamicToCurrentImage = Image.FromFile(GraphicsDirectory + "\\" + MIDGraphics.DynamicToCurrentImage);

				_filterUserSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ToolsFiltersStoreUser);
				_filterGlobalSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ToolsFiltersStoreGlobal);

				_userRIDList = new ArrayList();
				_userRIDList.Add(-1);

				if (_filterUserSecurity.AllowUpdate || _filterUserSecurity.AllowView)
				{
					_userRIDList.Add(SAB.ClientServerSession.UserRID);
				}

				if (_filterGlobalSecurity.AllowUpdate || _filterGlobalSecurity.AllowView)
				{
					_userRIDList.Add(Include.GlobalUserRID);	// Issue 3806
				}

				BindVersionComboBoxes();				
			
				LoadMethodData();

				// Begin issue 3716 - stodd 02/15/06
				LoadWorkflows();
				// End issue 3716

				// Begin Track #5859 stodd
				//// Begin MID Track 5852 - KJohnson - Security changes
				//bool chainCanUpdate = _OTSRollupMethod.ChainAuthorizedToUpdate();
				//if (chainCanUpdate)
				//{
				//    grdRollup.DisplayLayout.Bands[0].Columns["Chain"].CellActivation = Activation.AllowEdit;
				//    grdRollup.DisplayLayout.Bands[0].Columns["StoreToChain"].CellActivation = Activation.AllowEdit;
				//}
				//else
				//{
				//    grdRollup.DisplayLayout.Bands[0].Columns["Chain"].CellActivation = Activation.Disabled;
				//    grdRollup.DisplayLayout.Bands[0].Columns["StoreToChain"].CellActivation = Activation.Disabled;
				//}

				//bool storeCanUpdate = _OTSRollupMethod.StoreAuthorizedToUpdate();
				//if (storeCanUpdate)
				//{
				//    grdRollup.DisplayLayout.Bands[0].Columns["Store"].CellActivation = Activation.AllowEdit;
				//}
				//else
				//{
				//    grdRollup.DisplayLayout.Bands[0].Columns["Store"].CellActivation = Activation.Disabled;
				//}
				//// End MID Track 5852
				// Begin Track #5858 stodd
                //SetPlanTypeAuthority();
				// End Track #5858
				// End track #5859 stodd
				ApplySecurity();

                //BEGIN TT#110-MD-VStuart - In Use Tool
                tabSpreadMethod.Controls.Remove(tabProperties);
                //END TT#110-MD-VStuart - In Use Tool
            }

			catch( Exception exception )
			{
				HandleException(exception);
			}
		}

		private void BindVersionComboBoxes()
		{
			try
			{
				// BEGIN Issue 4858 stodd 11.7.2007 method security
//				_dtRollupVersions = new DataTable("Versions");
//				_dtRollupVersions.Columns.Add("Description", typeof(string));
//				_dtRollupVersions.Columns.Add("Key", typeof(int));
//
//				_dtRollupVersions.Rows.Add(new object[] {string.Empty, Include.NoRID});
//
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
//						_dtRollupVersions.Rows.Add(new object[] {verProf.Description, verProf.Key});
//					}
//					// End Issue 4562
//				}
//				
                // Begin Track #5858 - JSmith - Validating store security only
                //ProfileList versionProfList = base.GetForecastVersionList(ePlanBasisType.Plan, eSecurityTypes.Chain, false, this._OTSRollupMethod.VersionRID);
                ProfileList versionProfList = base.GetForecastVersionList(eSecuritySelectType.Update, eSecurityTypes.Chain | eSecurityTypes.Store, false, this._OTSRollupMethod.VersionRID);	// Track 5871
                // End Track #5858

				this.cboSpreadVersion.DisplayMember = "Description";
				this.cboSpreadVersion.ValueMember = "Key";
				this.cboSpreadVersion.DataSource = versionProfList.ArrayList;
				// END Issue 4858 stodd 11.7.2007 method security

			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		
		private bool DeleteWarningOK (string aChangedItem)
		{
			DialogResult diagResult;
			string errorMessage = string.Empty;
			bool continueProcess = true;
			try
			{
				if (_dtBasis.Rows.Count == 0) 
					continueProcess = true;
				else
				{
					errorMessage = string.Format(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_BasisDeleteWarning),
						aChangedItem );	
					errorMessage += Environment.NewLine + MIDText.GetTextOnly((int)eMIDTextCode.msg_ContinueQuestion);
					diagResult = MessageBox.Show(errorMessage,_thisTitle,
						MessageBoxButtons.YesNo,  MessageBoxIcon.Warning);
					if (diagResult == System.Windows.Forms.DialogResult.No)
						continueProcess = false;
					else
						continueProcess = true;
				}
			}
			catch(Exception ex)
			{
				HandleException(ex);
				continueProcess = false;
			}
			return continueProcess; 
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
                _OTSRollupMethod = new OTSRollupMethod(SAB, aMethodRID);
                ProcessAction(eMethodType.Rollup, true);
			}
			catch (Exception ex)
			{
				HandleException(ex);
				FormLoadError = true;
			}
		}
		// End Issue 4323 - stodd 2.21.07 - fix process from workflow explorer

		private void LoadMethodData()
		{
			try
			{
				// Inititalize Fields
                _dsRollup = _OTSRollupMethod.DSRollup;

				mdsPlanDateRange.DateRangeRID = Include.UndefinedCalendarDateRange;
				mdsPlanDateRange.SetImage(null);

                if (_OTSRollupMethod.Method_Change_Type != eChangeType.add)
				{

                    this.txtName.Text = _OTSRollupMethod.Name;
                    this.txtDesc.Text = _OTSRollupMethod.Method_Description;

                    if (_OTSRollupMethod.User_RID == Include.GetGlobalUserRID())
						radGlobal.Checked = true;
					else
						radUser.Checked = true;
					LoadWorkflows();
				}

                //Begin Track #5858 - KJohnson - Validating store security only
                txtSpreadNode.Tag = new MIDMerchandiseTextBoxTag(SAB, txtSpreadNode, eMIDControlCode.form_Rollup, eSecurityTypes.Chain | eSecurityTypes.Store, eSecuritySelectType.Update);
                //End Track #5858

                if (_OTSRollupMethod.HierNodeRID > 0)
				{
                    HierarchyNodeProfile hnp = SAB.HierarchyServerSession.GetNodeData(_OTSRollupMethod.HierNodeRID);
					txtSpreadNode.Text = hnp.Text;
                    //txtSpreadNode.Tag = hnp.Key;
                    ((MIDTag)(txtSpreadNode.Tag)).MIDTagData = hnp;
				}

				// BEGIN Issue 4858
                // Begin Track #5858 - JSmith - Validating store security only
                //base.ValidatePlanNodeSecurity(txtSpreadNode, true);
				base.ValidatePlanNodeSecurity(txtSpreadNode, true, eSecurityTypes.Chain | eSecurityTypes.Store);
                // End Track #5858
				// END Issue 4858

                if (_OTSRollupMethod.VersionRID > 0)
				{
                    cboSpreadVersion.SelectedValue = _OTSRollupMethod.VersionRID;
                    _parentVersion = _OTSRollupMethod.VersionRID;
				}
				else
				{
					_parentVersion = Include.NoRID;
				}
				// BEGIN Issue 4858
                // Begin Track #5858 - JSmith - Validating store security only
                //base.ValidatePlanVersionSecurity(cboSpreadVersion, true);
                base.ValidatePlanVersionSecurity(cboSpreadVersion.ComboBox, true, ePlanSelectType.Chain | ePlanSelectType.Store);
                // End Track #5858
				// END Issue 4858

                if (_OTSRollupMethod.DateRangeRID > 0 && _OTSRollupMethod.DateRangeRID != Include.UndefinedCalendarDateRange)
				{
                    DateRangeProfile drp = SAB.ClientServerSession.Calendar.GetDateRange(_OTSRollupMethod.DateRangeRID);
					LoadDateRangeSelector(mdsPlanDateRange, drp);
				}
 			 
				// Begin Issue 4328 - stodd 2.28.07
				// Moved ahead of radio buttons so basis grid is defined and RB checked change code
				// can function correctly.
				LoadBasis();
				// End issue 4328

			}
			catch(Exception ex)
			{
				HandleException(ex, "LoadMethodData");
			}
		}
		
		private void LoadBasis()
		{
			try
			{

                _dtBasis = MIDEnvironment.CreateDataTable("Basis");
			
				_dtBasis.Columns.Add("DETAIL_SEQ",		    System.Type.GetType("System.Int32")); //this column will be hidden.
				//_dtBasis.Columns.Add("FV_RID",		    System.Type.GetType("System.Int32")); //this column will be hidden.
                _dtBasis.Columns.Add("FromLevel",           System.Type.GetType("System.Object"));
                _dtBasis.Columns.Add("FROM_LEVEL_HRID",     System.Type.GetType("System.Int32")); //this column will be hidden.
                _dtBasis.Columns.Add("FROM_LEVEL_TYPE",     System.Type.GetType("System.Int32")); //this column will be hidden.
                _dtBasis.Columns.Add("FROM_LEVEL_SEQ",      System.Type.GetType("System.Int32")); //this column will be hidden.
                _dtBasis.Columns.Add("FROM_LEVEL_OFFSET",   System.Type.GetType("System.Int32")); //this column will be hidden.
                _dtBasis.Columns.Add("ToLevel",             System.Type.GetType("System.Object"));
                _dtBasis.Columns.Add("TO_LEVEL_HRID",       System.Type.GetType("System.Int32")); //this column will be hidden.
                _dtBasis.Columns.Add("TO_LEVEL_TYPE",       System.Type.GetType("System.Int32")); //this column will be hidden.
                _dtBasis.Columns.Add("TO_LEVEL_SEQ",        System.Type.GetType("System.Int32")); //this column will be hidden.
                _dtBasis.Columns.Add("TO_LEVEL_OFFSET",     System.Type.GetType("System.Int32")); //this column will be hidden.
                _dtBasis.Columns.Add("Store",               System.Type.GetType("System.Boolean"));
                _dtBasis.Columns.Add("Chain",               System.Type.GetType("System.Boolean"));
                _dtBasis.Columns.Add("StoreToChain",        System.Type.GetType("System.Boolean"));
                _dtBasis.Columns.Add("STORE_IND",           System.Type.GetType("System.Int32")); //this column will be hidden.
                _dtBasis.Columns.Add("CHAIN_IND",           System.Type.GetType("System.Int32")); //this column will be hidden.
                _dtBasis.Columns.Add("STORE_TO_CHAIN_IND",  System.Type.GetType("System.Int32")); //this column will be hidden.
                //_dtBasis.Columns.Add("DateRange",         System.Type.GetType("System.String"));
				//_dtBasis.Columns.Add("CDR_RID",	 		System.Type.GetType("System.Int32")); //this column will be hidden.
				//_dtBasis.Columns.Add("WEIGHT",			System.Type.GetType("System.Decimal"));

                if (_dsRollup != null)
				{
                    _dtBasis = _dsRollup.Tables["Basis"];
				}
				else
				{
                    _dsRollup = new DataSet();
                    _dsRollup.Tables.Add(_dtBasis);
				}

				//_dtBasis.Columns["WEIGHT"].DefaultValue = 1;

                //foreach (DataRow dr in _dtBasis.Rows)
                //{
                //    DateRangeProfile drp = SAB.ClientServerSession.Calendar.GetDateRange(Convert.ToInt32(dr["CDR_RID"], CultureInfo.CurrentUICulture));
                //    dr["DateRange"] = drp.DisplayDate;
                //}	

                //foreach (DataRow dr in _dtBasis.Rows)
                //{
                //   // dr["FromLevel"] = drp.DisplayDate;
                //   //cboFromLevel.SelectedIndex = cboFromLevel.Items.IndexOf(new FromLevelCombo(_OTSRollupMethod.FromLevelType, _OTSRollupMethod.FromLevelOffset, _OTSRollupMethod.FromLevelSequence, ""));
                //   //cboToLevel.SelectedIndex = cboToLevel.Items.IndexOf(new ToLevelCombo(_OTSRollupMethod.ToLevelType, _OTSRollupMethod.ToLevelOffset, _OTSRollupMethod.ToLevelSequence, ""));

                //   //dr["FromLevel"]

                //   //_dtBasis.Rows["FromLevel"]. 
                //}	



				grdRollup.DataSource = _dtBasis.DefaultView;
			}
			catch(Exception ex)
			{
				HandleException(ex, "LoadBasis");
			}
		}


		#region Grid Events

		private void grdRollup_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
		{
			try
			{
                //Begin TT#169 - JSmith - enter max of 1000 and receive 'must be integer'
                //MIDRetail.Windows.Controls.UltraGridLayoutDefaults ugld = new MIDRetail.Windows.Controls.UltraGridLayoutDefaults();
                //ugld.ApplyDefaults(e);
                MIDRetail.Windows.Controls.UltraGridLayoutDefaults ugld = new MIDRetail.Windows.Controls.UltraGridLayoutDefaults(ErrorImage);
                ugld.ApplyDefaults((Infragistics.Win.UltraWinGrid.UltraGrid)sender, e, true, 2, true);
                //End TT#169

				// BEGIN MID Track #3792 - replace obsolete method 
				//e.Layout.AutoFitColumns = true;
				e.Layout.AutoFitStyle = AutoFitStyle.ResizeAllColumns;
				// END MID Track #3792

				//hide the key columns.
				e.Layout.Bands[0].Columns["DETAIL_SEQ"].Hidden = true;
                e.Layout.Bands[0].Columns["FROM_LEVEL_HRID"].Hidden = true;
                e.Layout.Bands[0].Columns["FROM_LEVEL_TYPE"].Hidden = true;
                e.Layout.Bands[0].Columns["FROM_LEVEL_SEQ"].Hidden = true;
                e.Layout.Bands[0].Columns["FROM_LEVEL_OFFSET"].Hidden = true;
                e.Layout.Bands[0].Columns["TO_LEVEL_HRID"].Hidden = true;
                e.Layout.Bands[0].Columns["TO_LEVEL_TYPE"].Hidden = true;
                e.Layout.Bands[0].Columns["TO_LEVEL_SEQ"].Hidden = true;
                e.Layout.Bands[0].Columns["TO_LEVEL_OFFSET"].Hidden = true;

                e.Layout.Bands[0].Columns["STORE_IND"].Hidden = true;
                e.Layout.Bands[0].Columns["CHAIN_IND"].Hidden = true;
                e.Layout.Bands[0].Columns["STORE_TO_CHAIN_IND"].Hidden = true;

				//Prevent the user from re-arranging columns.
				grdRollup.DisplayLayout.Override.AllowColMoving = AllowColMoving.NotAllowed;
                grdRollup.DisplayLayout.Bands[0].ColHeaderLines = 2;

				if (FunctionSecurity.AllowUpdate)
				{
					grdRollup.DisplayLayout.Override.AllowAddNew = AllowAddNew.Yes;
					grdRollup.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.True;
					BuildBasisContextMenu();
					grdRollup.ContextMenu = mnuBasisGrid;
				}
				else
				{
					grdRollup.DisplayLayout.Override.AllowAddNew = AllowAddNew.No;
					grdRollup.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
				}

				//Set the header captions.
                int visiblePosition = 0;

				//grdRollup.DisplayLayout.Bands[0].Columns["Merchandise"].Header.VisiblePosition = 1;
				//grdRollup.DisplayLayout.Bands[0].Columns["Merchandise"].Header.Caption = "Merchandise";
				//grdRollup.DisplayLayout.Bands[0].Columns["FV_RID"].Header.VisiblePosition = 2;
				//grdRollup.DisplayLayout.Bands[0].Columns["FV_RID"].Header.Caption = "Version";
                //grdRollup.DisplayLayout.Bands[0].Columns["DateRange"].Header.VisiblePosition = 3;
                //grdRollup.DisplayLayout.Bands[0].Columns["DateRange"].Header.Caption = "Date Range";

                grdRollup.DisplayLayout.Bands[0].Columns["FromLevel"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
              //grdRollup.DisplayLayout.Bands[0].Columns["FromLevel"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.;
                grdRollup.DisplayLayout.Bands[0].Columns["FromLevel"].Header.VisiblePosition = visiblePosition;
                grdRollup.DisplayLayout.Bands[0].Columns["FromLevel"].Header.Caption = "From Level";
                grdRollup.DisplayLayout.Bands[0].Columns["FromLevel"].MinWidth = 100;
                grdRollup.DisplayLayout.Bands[0].Columns["FromLevel"].Width = 300;

                grdRollup.DisplayLayout.Bands[0].Columns["ToLevel"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                grdRollup.DisplayLayout.Bands[0].Columns["ToLevel"].Header.VisiblePosition = ++visiblePosition;
                grdRollup.DisplayLayout.Bands[0].Columns["ToLevel"].Header.Caption = "To Level";
                grdRollup.DisplayLayout.Bands[0].Columns["ToLevel"].MinWidth = 100;
                grdRollup.DisplayLayout.Bands[0].Columns["ToLevel"].Width = 300;

                grdRollup.DisplayLayout.Bands[0].Columns["Store"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox;
                //grdRollup.DisplayLayout.Bands[0].Columns["Store"].CellAppearance.ImageHAlign = Infragistics.Win.HAlign.Center;
                //grdRollup.DisplayLayout.Bands[0].Columns["Store"].CellAppearance.ImageVAlign = Infragistics.Win.VAlign.Middle;
                //grdRollup.DisplayLayout.Bands[0].Columns["Store"].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right;
                grdRollup.DisplayLayout.Bands[0].Columns["Store"].Header.VisiblePosition = ++visiblePosition;
                grdRollup.DisplayLayout.Bands[0].Columns["Store"].Header.Caption = "Store";
                grdRollup.DisplayLayout.Bands[0].Columns["Store"].MinWidth = 55;

                grdRollup.DisplayLayout.Bands[0].Columns["Chain"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox;
                //grdRollup.DisplayLayout.Bands[0].Columns["Chain"].CellAppearance.ImageHAlign = Infragistics.Win.HAlign.Center;
                //grdRollup.DisplayLayout.Bands[0].Columns["Chain"].CellAppearance.ImageVAlign = Infragistics.Win.VAlign.Middle;
                //grdRollup.DisplayLayout.Bands[0].Columns["Chain"].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right;
                grdRollup.DisplayLayout.Bands[0].Columns["Chain"].Header.VisiblePosition = ++visiblePosition;
                grdRollup.DisplayLayout.Bands[0].Columns["Chain"].Header.Caption = "Chain";
                grdRollup.DisplayLayout.Bands[0].Columns["Chain"].MinWidth = 55;

                grdRollup.DisplayLayout.Bands[0].Columns["StoreToChain"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox;
                grdRollup.DisplayLayout.Bands[0].Columns["StoreToChain"].Header.VisiblePosition = ++visiblePosition;
                grdRollup.DisplayLayout.Bands[0].Columns["StoreToChain"].Header.Caption = "Store To\nChain";
                grdRollup.DisplayLayout.Bands[0].Columns["StoreToChain"].MinWidth = 55;

				//grdRollup.DisplayLayout.Bands[0].Columns["WEIGHT"].Header.VisiblePosition = 4;
				//grdRollup.DisplayLayout.Bands[0].Columns["WEIGHT"].Header.Caption = "Weight";
				//grdRollup.DisplayLayout.Bands[0].Columns["IncludeButton"].Header.VisiblePosition = 6;
				//grdRollup.DisplayLayout.Bands[0].Columns["IncludeButton"].Header.Caption = " ";

				//make the "Version" column a drop down list.
				//grdRollup.DisplayLayout.Bands[0].Columns["FV_RID"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
				//grdRollup.DisplayLayout.Bands[0].Columns["FV_RID"].ValueList = grdRollup.DisplayLayout.ValueLists["Version"];

				//grdRollup.DisplayLayout.Bands[0].Columns["IncludeButton"].CellAppearance.ImageHAlign = Infragistics.Win.HAlign.Center;
				//grdRollup.DisplayLayout.Bands[0].Columns["IncludeButton"].CellAppearance.ImageVAlign = Infragistics.Win.VAlign.Middle;
				//grdRollup.DisplayLayout.Bands[0].Columns["IncludeButton"].Style  = Infragistics.Win.UltraWinGrid.ColumnStyle.Button;
				//grdRollup.DisplayLayout.Bands[0].Columns["IncludeButton"].Width = 20;
				//grdRollup.DisplayLayout.Bands[0].Columns["IncludeButton"].CellActivation = Activation.NoEdit;


				//Make the "INCLUDE_EXCLUDE" column a checkbox column.
				//grdRollup.DisplayLayout.Bands[0].Columns["INCLUDE_EXCLUDE"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox;
		
				//Set the width of the "DateRange" column so that the DateRangeSelector can fit.
				//grdRollup.DisplayLayout.Bands[0].Columns["DateRange"].Width = 160;

				// BEGIN Issue 4640 stodd 09.21.2007
				//if (FunctionSecurity.AllowUpdate)
				//{
				//	grdRollup.DisplayLayout.Bands[0].Columns["DateRange"].Style  = Infragistics.Win.UltraWinGrid.ColumnStyle.EditButton;
				//	grdRollup.DisplayLayout.Bands[0].Columns["DateRange"].CellActivation = Activation.NoEdit;
				//}
				// End Issue 4640 stodd 09.21.2007

                grdRollup.DisplayLayout.Bands[0].AddButtonCaption = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_AddNewRollup);
                grdRollup.DisplayLayout.Bands[0].AddButtonToolTipText = MIDText.GetTextOnly(eMIDTextCode.tt_Button_AddNewRollup);
				grdRollup.DisplayLayout.AddNewBox.Hidden = false;
				grdRollup.DisplayLayout.AddNewBox.Style = AddNewBoxStyle.Compact;

                int verKey;
                VersionProfile verProf;

                foreach (UltraGridRow row in grdRollup.Rows)
                {
                    FillLevelValueLists(row);

					// Begin Track #5859 stodd
					//if (_OTSRollupMethod.VersionRID != Include.NoRID)
					//{
					//    verKey = Convert.ToInt32(_OTSRollupMethod.VersionRID);
					//    verProf = (VersionProfile)_versionProfList.FindKey(verKey);

					//    if (verProf != null)
					//    {
					//        if (verProf.ChainSecurity.AllowUpdate)
					//        {
					//            row.Cells["Chain"].Activation = Infragistics.Win.UltraWinGrid.Activation.AllowEdit;
					//            row.Cells["StoreToChain"].Activation = Infragistics.Win.UltraWinGrid.Activation.AllowEdit;
					//        }
					//        else
					//        {
					//            row.Cells["Chain"].Activation = Infragistics.Win.UltraWinGrid.Activation.Disabled;
					//            row.Cells["StoreToChain"].Activation = Infragistics.Win.UltraWinGrid.Activation.Disabled;
					//        }

					//        if (verProf.StoreSecurity.AllowUpdate)
					//        {
					//            row.Cells["Store"].Activation = Infragistics.Win.UltraWinGrid.Activation.AllowEdit;
					//        }
					//        else
					//        {
					//            row.Cells["Store"].Activation = Infragistics.Win.UltraWinGrid.Activation.Disabled;
					//        }
					//    }
					//    else
					//    {
					//        row.Cells["Chain"].Activation = Infragistics.Win.UltraWinGrid.Activation.Disabled;
					//        row.Cells["StoreToChain"].Activation = Infragistics.Win.UltraWinGrid.Activation.Disabled;
					//        row.Cells["Store"].Activation = Infragistics.Win.UltraWinGrid.Activation.Disabled;
					//    }
					//}
					// End track #5859 stodd
                }

			}
			catch(Exception err)
			{
				HandleException(err);
			}
		}

        private void grdRollup_AfterCellListCloseUp(object sender, Infragistics.Win.UltraWinGrid.CellEventArgs e)
        {
            HierarchyLevelValueItem valItem = null;
            // BEGIN MID Track #2704 - Security updates
            VersionProfile verProf;

            // END MID Track #2704 - Security updates
            try
            {
                int newFromIndex = -1;
                int newToIndex = -1;

                int oldFromIndex = -1;
                if (e.Cell.Row.Cells["FromLevel"].Value != System.DBNull.Value)
                {
                    oldFromIndex = Convert.ToInt32(e.Cell.Row.Cells["FromLevel"].Value);
                }

                int oldToIndex = -1;
                if (e.Cell.Row.Cells["ToLevel"].Value != System.DBNull.Value)
                {
                    oldToIndex = Convert.ToInt32(e.Cell.Row.Cells["ToLevel"].Value);
                }

                bool oldChainCheck = Convert.ToBoolean(e.Cell.Row.Cells["Chain"].Value);
                bool oldStoreCheck = Convert.ToBoolean(e.Cell.Row.Cells["Store"].Value);
                bool oldStoreToChainCheck = Convert.ToBoolean(e.Cell.Row.Cells["StoreToChain"].Value);

                if (grdRollup.ActiveCell != null)
                {
                    switch (e.Cell.Column.Key)
                    {
                        case "FromLevel":
                            grdRollup.PerformAction(UltraGridAction.ExitEditMode);

                            if (e.Cell.Row.Cells["FromLevel"].Value != System.DBNull.Value)
                            {
                                newFromIndex = Convert.ToInt32(e.Cell.Row.Cells["FromLevel"].Value);
                            }

                            if (e.Cell.Row.Cells["ToLevel"].Value != System.DBNull.Value)
                            {
                                newToIndex = Convert.ToInt32(e.Cell.Row.Cells["ToLevel"].Value);
                            }

                            if (e.Cell.Value == System.DBNull.Value)
                            {
                                e.Cell.Row.Cells["FROM_LEVEL_TYPE"].Value = System.DBNull.Value;
                                e.Cell.Row.Cells["FROM_LEVEL_HRID"].Value = System.DBNull.Value;
                                e.Cell.Row.Cells["FROM_LEVEL_SEQ"].Value = System.DBNull.Value;
                                e.Cell.Row.Cells["FROM_LEVEL_OFFSET"].Value = System.DBNull.Value;
                            }
                            else
                            {
                                if ((newFromIndex != -1) && (newToIndex != -1))
                                {
                                    if (newFromIndex <= newToIndex)
                                    {
                                        if (oldStoreCheck || oldChainCheck)
                                        {
                                            e.Cell.Row.Cells["FromLevel"].Value = oldToIndex + 1;
                                        }
                                        else
                                        {
                                            e.Cell.Row.Cells["FromLevel"].Value = oldToIndex;
                                        }
                                    }
                                }

                                valItem = (HierarchyLevelValueItem)_fromLevelList[Convert.ToInt32(e.Cell.Value)];
                                e.Cell.Row.Cells["FROM_LEVEL_TYPE"].Value = valItem.LevelType;

                                if (valItem.LevelType == eHierarchyDescendantType.levelType)
                                {
                                    e.Cell.Row.Cells["FROM_LEVEL_HRID"].Value = valItem.HierarchyRID;
                                    e.Cell.Row.Cells["FROM_LEVEL_SEQ"].Value = valItem.LevelRID;
                                    e.Cell.Row.Cells["FROM_LEVEL_OFFSET"].Value = System.DBNull.Value;
                                }
                                else
                                {
                                    e.Cell.Row.Cells["FROM_LEVEL_HRID"].Value = System.DBNull.Value;
                                    e.Cell.Row.Cells["FROM_LEVEL_SEQ"].Value = System.DBNull.Value;
                                    e.Cell.Row.Cells["FROM_LEVEL_OFFSET"].Value = valItem.Offset;
                                }
                            }
                            break;
                        case "ToLevel":
                            grdRollup.PerformAction(UltraGridAction.ExitEditMode);

                            if (e.Cell.Row.Cells["FromLevel"].Value != System.DBNull.Value)
                            {
                                newFromIndex = Convert.ToInt32(e.Cell.Row.Cells["FromLevel"].Value);
                            }

                            if (e.Cell.Row.Cells["ToLevel"].Value != System.DBNull.Value)
                            {
                                newToIndex = Convert.ToInt32(e.Cell.Row.Cells["ToLevel"].Value);
                            }

                            if (e.Cell.Value == System.DBNull.Value)
                            {
                                e.Cell.Row.Cells["TO_LEVEL_TYPE"].Value = System.DBNull.Value;
                                e.Cell.Row.Cells["TO_LEVEL_HRID"].Value = System.DBNull.Value;
                                e.Cell.Row.Cells["TO_LEVEL_SEQ"].Value = System.DBNull.Value;
                                e.Cell.Row.Cells["TO_LEVEL_OFFSET"].Value = System.DBNull.Value;
                            }
                            else
                            {
                                if ((newFromIndex != -1) && (newToIndex != -1))
                                {
                                    if (newToIndex >= newFromIndex)
                                    {
                                        if (oldStoreCheck || oldChainCheck)
                                        {
                                            e.Cell.Row.Cells["ToLevel"].Value = oldFromIndex - 1;
                                        }
                                        else
                                        {
                                            e.Cell.Row.Cells["ToLevel"].Value = oldFromIndex;
                                        }
                                    }
                                }

                                valItem = (HierarchyLevelValueItem)_toLevelList[Convert.ToInt32(e.Cell.Value)];
                                e.Cell.Row.Cells["TO_LEVEL_TYPE"].Value = valItem.LevelType;

                                if (valItem.LevelType == eHierarchyDescendantType.levelType)
                                {
                                    e.Cell.Row.Cells["TO_LEVEL_HRID"].Value = valItem.HierarchyRID;
                                    e.Cell.Row.Cells["TO_LEVEL_SEQ"].Value = valItem.LevelRID;
                                    e.Cell.Row.Cells["TO_LEVEL_OFFSET"].Value = System.DBNull.Value;
                                }
                                else
                                {
                                    e.Cell.Row.Cells["TO_LEVEL_HRID"].Value = System.DBNull.Value;
                                    e.Cell.Row.Cells["TO_LEVEL_SEQ"].Value = System.DBNull.Value;
                                    e.Cell.Row.Cells["TO_LEVEL_OFFSET"].Value = valItem.Offset;
                                }
                            }
                            break;
                    }
                }

                //e.Cell.Column.PerformAutoResize(PerformAutoSizeType.VisibleRows);

                ChangePending = true;
            }
            catch (Exception exc)
            {
                HandleException(exc);
            }
        }

		private void grdRollup_InitializeRow(object sender, Infragistics.Win.UltraWinGrid.InitializeRowEventArgs e)
		{
            //if (e.Row.Cells["CDR_RID"].Value.ToString() != "")
            //{
            //    DateRangeProfile dr;
            //    if (mdsPlanDateRange.DateRangeRID != Include.NoRID)
            //        dr = SAB.ClientServerSession.Calendar.GetDateRange(Convert.ToInt32(e.Row.Cells["CDR_RID"].Value, CultureInfo.CurrentUICulture),mdsPlanDateRange.DateRangeRID);
            //    else
            //        dr = SAB.ClientServerSession.Calendar.GetDateRange(Convert.ToInt32(e.Row.Cells["CDR_RID"].Value, CultureInfo.CurrentUICulture));

            //    //e.Row.Cells["DateRange"].Value = dr.DisplayDate;
            //    e.Row.Cells["CDR_RID"].Value = dr.Key;

            //    if (dr.DateRangeType == eCalendarRangeType.Dynamic)
            //    {
            //        switch (dr.RelativeTo)
            //        {
            //            case eDateRangeRelativeTo.Current:
            //                e.Row.Cells["DateRange"].Appearance.Image = this.DynamicToCurrentImage;
            //                break;
            //            case eDateRangeRelativeTo.Plan:
            //                e.Row.Cells["DateRange"].Appearance.Image = this.DynamicToPlanImage;
            //                break;
            //            default:
            //                e.Row.Cells["DateRange"].Appearance.Image = null;
            //                break;
            //        }
            //    }
            //}

//			try
//			{
//				if (e.Row.Cells["INCLUDE_EXCLUDE"].Value != DBNull.Value)
//				{
//					if (Convert.ToInt32(e.Row.Cells["INCLUDE_EXCLUDE"].Value,CultureInfo.CurrentUICulture) == (int)eBasisIncludeExclude.Include)
//					{
//						e.Row.Cells["IncludeButton"].Appearance.Image = _picInclude;
//					}
//					else
//					{
//						e.Row.Cells["INCLUDE_EXCLUDE"].Value = (int)eBasisIncludeExclude.Exclude;
//						e.Row.Cells["IncludeButton"].Appearance.Image = _picExclude;
//					}
//				}
//			}
//			catch (Exception exc)
//			{
//				HandleException(exc);
//			}

		}

		private void grdRollup_CellChange(object sender, Infragistics.Win.UltraWinGrid.CellEventArgs e)
		{
			if (FormLoaded)
			{
				ChangePending = true;

				// Begin Track #5859 stodd
				// This was added so _OTSRollupMethod.AuthorizedToUpdate() would get the most current info.
                //_dsRollup.AcceptChanges();
                //_OTSRollupMethod.DSRollup = _dsRollup;
				// End Track #5859

				//switch (e.Cell.Column.Key)
				//{
				//    case "Chain":
				//        e.Cell.Row.Cells["CHAIN_IND"].Value = (Convert.ToBoolean(e.Cell.Row.Cells["Chain"].Value)) ? 1 : 0;
				//        ApplySecurity();	// Track #5858 stodd
				//        break;
				//    case "Store":
				//        e.Cell.Row.Cells["STORE_IND"].Value = (Convert.ToBoolean(e.Cell.Row.Cells["Store"].Value)) ? 1 : 0;
				//        ApplySecurity();	// Track #5858 stodd	
				//        break;
				//    case "StoreToChain":
				//        e.Cell.Row.Cells["STORE_TO_CHAIN_IND"].Value = (Convert.ToBoolean(e.Cell.Row.Cells["StoreToChain"].Value)) ? 1 : 0;
				//        ApplySecurity();	// Track #5858 stodd
				//        grdRollup.PerformAction(UltraGridAction.PrevCell);
				//        break;
				//    //					case "Merchandise":
				//    //						if (e.Cell.Value.ToString().Trim().Length == 0)
				//    //						{
				//    //							e.Cell.Row.Cells["HN_RID"].Value = Include.NoRID;
				//    //						}
				//    //						else
				//    //						{
				//    //							productID = e.Cell.Value.ToString().Trim();
				//    //							HierarchyNodeProfile hnp = SAB.HierarchyServerSession.GetNodeData(productID);
				//    //							if (hnp.Key == Include.NoRID)
				//    //							{
				//    //								errorMessage = string.Format(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_InvalidTreeNode),
				//    //									productID );	
				//    //								errorFound = true;
				//    //							}
				//    //							else 
				//    //							{
				//    //								e.Cell.Row.Cells["HN_RID"].Value = hnp.Key;
				//    //								_skipAfterCellUpdate = true;
				//    //								e.Cell.Value = hnp.Text;
				//    //							}
				//    //						}
				//    //						break;
				//    default:
				//        break;
				//}

			}
		
		}

		private void grdRollup_AfterCellUpdate(object sender, Infragistics.Win.UltraWinGrid.CellEventArgs e)
		{
			bool errorFound = false;
			string errorMessage = string.Empty, productID;
			try
			{
				if (_skipAfterCellUpdate)
				{
					_skipAfterCellUpdate = false;
					return;
				}

				// BEGIN Track #5852 (point 3) - chain/store/storeToChain checkbox error
				//switch (e.Cell.Column.Key)
				//{
				//    case "Chain":
				//        e.Cell.Row.Cells["CHAIN_IND"].Value = (Convert.ToBoolean(e.Cell.Row.Cells["Chain"].Value)) ? 1 : 0;
				//        ApplySecurity();	// Track #5858 stodd
				//        grdRollup.PerformAction(UltraGridAction.PrevCell);
				//        break;
				//    case "Store":
				//        e.Cell.Row.Cells["STORE_IND"].Value = (Convert.ToBoolean(e.Cell.Row.Cells["Store"].Value)) ? 1 : 0;
				//        ApplySecurity();	// Track #5858 stodd	
				//        grdRollup.PerformAction(UltraGridAction.PrevCell);
				//        break;
				//    case "StoreToChain":
				//        e.Cell.Row.Cells["STORE_TO_CHAIN_IND"].Value = (Convert.ToBoolean(e.Cell.Row.Cells["StoreToChain"].Value)) ? 1 : 0;
				//        ApplySecurity();	// Track #5858 stodd
				//        grdRollup.PerformAction(UltraGridAction.PrevCell);
				//        break;
				//    //					case "Merchandise":
				//    //						if (e.Cell.Value.ToString().Trim().Length == 0)
				//    //						{
				//    //							e.Cell.Row.Cells["HN_RID"].Value = Include.NoRID;
				//    //						}
				//    //						else
				//    //						{
				//    //							productID = e.Cell.Value.ToString().Trim();
				//    //							HierarchyNodeProfile hnp = SAB.HierarchyServerSession.GetNodeData(productID);
				//    //							if (hnp.Key == Include.NoRID)
				//    //							{
				//    //								errorMessage = string.Format(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_InvalidTreeNode),
				//    //									productID );	
				//    //								errorFound = true;
				//    //							}
				//    //							else 
				//    //							{
				//    //								e.Cell.Row.Cells["HN_RID"].Value = hnp.Key;
				//    //								_skipAfterCellUpdate = true;
				//    //								e.Cell.Value = hnp.Text;
				//    //							}
				//    //						}
				//    //						break;
				//    default:
				//        break;
				//}
				// END Track #5852 (point 3) - chain/store/storeToChain checkbox error

				//if (errorFound)
				//{
				//    e.Cell.Appearance.Image = ErrorImage;
				//    e.Cell.Tag = errorMessage;
				//}
				//else
				//{
				//    e.Cell.Appearance.Image = null;
				//    e.Cell.Tag = null;
				//}

				if (FormLoaded)
				{
					ChangePending = true;
				}


			}
			catch (Exception exc)
			{
				HandleException(exc);
			}
		}
		
		private void grdRollup_AfterRowInsert(object sender, Infragistics.Win.UltraWinGrid.RowEventArgs e)
		{
			try
			{
				//The "IsIncluded" column is a checkbox column. Right after we inserted a
				//new row, we want to default this column to be checked (true). If we
				//don't, it will default to a grayed-out check, like the 3rd state in a 
				//tri-state checkbox, even if we explicitly set this column to be a normal
				//checkbox.

				if (e.Row.Band == grdRollup.DisplayLayout.Bands[0])
				{
					//e.Row.Cells["SGL_RID"].Value = Convert.ToInt32(cboAttributeSet.SelectedValue,CultureInfo.CurrentUICulture);
					//e.Row.Cells["HN_RID"].Value = Include.NoRID;
					//e.Row.Cells["CDR_RID"].Value = Include.UndefinedCalendarDateRange;
					//e.Row.Cells["INCLUDE_EXCLUDE"].Value = (int)eBasisIncludeExclude.Include;
					//e.Row.Cells["IncludeButton"].Appearance.Image = _picInclude;

                    //e.Row.Cells["Store"].Value;

                    e.Row.Cells["Store"].Value = false;
                    e.Row.Cells["STORE_IND"].Value = 0;
                    e.Row.Cells["Chain"].Value = false;
                    e.Row.Cells["CHAIN_IND"].Value = 0;
                    e.Row.Cells["StoreToChain"].Value = false;
                    e.Row.Cells["STORE_TO_CHAIN_IND"].Value = 0;
                    FillLevelValueLists(e.Row);
                    int rowCount = _dtBasis.Rows.Count;
					e.Row.Cells["DETAIL_SEQ"].Value = rowCount++;
				}

				if (FormLoaded)
				{
					ChangePending = true;
				}
			}
			catch (Exception exc)
			{
				HandleException(exc);
			}
		}

		private void grdRollup_DragOver(object sender, System.Windows.Forms.DragEventArgs e)
		{
			try
			{
				Infragistics.Win.UIElement aUIElement;
				aUIElement = grdRollup.DisplayLayout.UIElement.ElementFromPoint(grdRollup.PointToClient(new Point(e.X, e.Y)));

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
				
//				if (aCell == aRow.Cells["Merchandise"])
				if (aCell.Column.Key == "Merchandise")
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
		private void grdRollup_DragDrop(object sender, System.Windows.Forms.DragEventArgs e)
		{
			HierarchyNodeProfile hnp;
            TreeNodeClipboardList cbList;
			try
			{
				Infragistics.Win.UIElement aUIElement;

				aUIElement = grdRollup.DisplayLayout.UIElement.ElementFromPoint(grdRollup.PointToClient(new Point(e.X, e.Y)));

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
                        //IDataObject data = Clipboard.GetDataObject();

						//If the data is ClipboardProfile, then retrieve the data
                        //ClipboardProfile cbp;
                        //HierarchyClipboardData MIDTreeNode_cbd;
						//object cellValue = null;	
                        if (e.Data.GetDataPresent(typeof(TreeNodeClipboardList)))
						{
                            cbList = (TreeNodeClipboardList)e.Data.GetData(typeof(TreeNodeClipboardList));
                            //if (cbp.ClipboardDataType == eClipboardDataType.HierarchyNode)
                            if (cbList.ClipboardDataType == eProfileType.HierarchyNode)
							{
                                //if (cbp.ClipboardData.GetType() == typeof(HierarchyClipboardData))
                                //{
                                    //MIDTreeNode_cbd = (HierarchyClipboardData)cbp.ClipboardData;

                                hnp = SAB.HierarchyServerSession.GetNodeData(cbList.ClipboardProfile.Key);
									


									//aRow.Cells["HN_RID"].Value = cbp.Key;
									_skipAfterCellUpdate = true;
									//aRow.Cells["Merchandise"].Value = hnp.Text;
//									AddNodeToMerchandiseCombo2 (hnp);
//									if (!_basisNodeInList)
//									{
//										Infragistics.Win.ValueListItem vli = new Infragistics.Win.ValueListItem();
//										vli.DataValue = ugBasisNodeVersion.DisplayLayout.ValueLists["Merchandise"].ValueListItems.Count;
//										vli.DisplayText = hnp.Text;;
//										vli.Tag = Convert.ToString(hnp.Key, CultureInfo.CurrentUICulture);
//										ugBasisNodeVersion.DisplayLayout.ValueLists["Merchandise"].ValueListItems.Add(vli);
//										cellValue = vli.DataValue;	
//									}
//									else
//									{
//										foreach (Infragistics.Win.ValueListItem vli in ugBasisNodeVersion.DisplayLayout.ValueLists["Merchandise"].ValueListItems)
//										{
//											if (vli.DisplayText == hnp.Text)
//											{
//												cellValue = vli.DataValue;
//												break;
//											}
//										}	
//									}
//									_skipAfterCellUpdate = true;
//									aCell.Value = cellValue;
//									_skipAfterCellUpdate = false;
									grdRollup.UpdateData();
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
		private void grdRollup_ClickCellButton(object sender, Infragistics.Win.UltraWinGrid.CellEventArgs e)
		{
			CalendarDateSelector frmCalDtSelector;
			DialogResult dateRangeResult;
			DateRangeProfile selectedDateRange;

			try
			{
				switch (e.Cell.Column.Key)
				{
                    //case "DateRange":

                    //    frmCalDtSelector = (CalendarDateSelector)CreateControl(typeof(CalendarDateSelector), new object[]{SAB});

                    //    if (e.Cell.Row.Cells["DateRange"].Value != null &&
                    //        e.Cell.Row.Cells["DateRange"].Value != System.DBNull.Value &&
                    //        e.Cell.Row.Cells["DateRange"].Text.Length > 0)
                    //    {
                    //        frmCalDtSelector.DateRangeRID = Convert.ToInt32(e.Cell.Row.Cells["CDR_RID"].Value, CultureInfo.CurrentUICulture);
                    //    }

                    //    if (mdsPlanDateRange.DateRangeRID != Include.UndefinedCalendarDateRange)
                    //    {
                    //        frmCalDtSelector.AnchorDateRangeRID = mdsPlanDateRange.DateRangeRID;
                    //    }
                    //    else
                    //    {
                    //        frmCalDtSelector.AnchorDate = SAB.ClientServerSession.Calendar.CurrentDate;
                    //    }

                    //    frmCalDtSelector.AnchorDateRelativeTo = eDateRangeRelativeTo.Current;
                    //    frmCalDtSelector.AllowDynamicToStoreOpen = false;
                    //    frmCalDtSelector.AllowDynamicToPlan = false;

                    //    dateRangeResult = frmCalDtSelector.ShowDialog();

                    //    if (dateRangeResult == DialogResult.OK)
                    //    {
                    //        selectedDateRange = (DateRangeProfile)frmCalDtSelector.Tag;

                    //        e.Cell.Value = selectedDateRange.DisplayDate;
                    //        e.Cell.Row.Cells["CDR_RID"].Value = selectedDateRange.Key;

                    //        if (selectedDateRange.DateRangeType == eCalendarRangeType.Dynamic)
                    //        {
                    //            if (selectedDateRange.RelativeTo == eDateRangeRelativeTo.Plan)
                    //            {
                    //                e.Cell.Appearance.Image = _dynamicToPlanImage;
                    //            }
                    //            else
                    //            {
                    //                e.Cell.Appearance.Image = _dynamicToCurrentImage;
                    //            }
                    //        }
                    //        else
                    //        {
                    //            e.Cell.Appearance.Image = null;
                    //        }
                    //    }

                    //    e.Cell.CancelUpdate();
                    //    grdRollup.PerformAction(UltraGridAction.DeactivateCell);


                    //    break;

//					case "IncludeButton":
//
//						if (Convert.ToInt32(e.Cell.Row.Cells["INCLUDE_EXCLUDE"].Value, CultureInfo.CurrentUICulture) == (int)eBasisIncludeExclude.Include)
//						{
//							e.Cell.Row.Cells["INCLUDE_EXCLUDE"].Value = (int)eBasisIncludeExclude.Exclude;
//							e.Cell.Appearance.Image = _picExclude;
//						}
//						else
//						{
//							e.Cell.Row.Cells["INCLUDE_EXCLUDE"].Value = (int)eBasisIncludeExclude.Include;
//							e.Cell.Appearance.Image = _picInclude;
//						}
//
//						e.Cell.CancelUpdate();
//						grdRollup.PerformAction(UltraGridAction.DeactivateCell);
//
//						break;
				}
			}
			catch(Exception exception)
			{
				HandleException(exception);
			}
		}
		private void grdRollup_MouseEnterElement(object sender, Infragistics.Win.UIElementEventArgs e)
		{
			try
			{
				ShowUltraGridToolTip(grdRollup, e);
			}
			catch(Exception ex)
			{
				HandleException(ex);
			}
		}

		// Begin Track #5852 (3) stodd - Showing error immediately upon clicking checkbox
		private void grdRollup_MouseUp(object sender, MouseEventArgs e)
		{
			try
			{
				Infragistics.Win.UIElement aUIElement;
				Point point = new Point(e.X, e.Y);
				aUIElement = grdRollup.DisplayLayout.UIElement.ElementFromPoint(point);

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

				switch (aCell.Column.Key)
				{
					case "Chain":
						aCell.Row.Cells["CHAIN_IND"].Value = (Convert.ToBoolean(aCell.Row.Cells["Chain"].Text)) ? 1 : 0;
						ApplySecurity();
						grdRollup.PerformAction(UltraGridAction.DeactivateCell);
						break;
					case "Store":
                        aCell.Row.Cells["STORE_IND"].Value = (Convert.ToBoolean(aCell.Row.Cells["Store"].Text)) ? 1 : 0;
						ApplySecurity();
						grdRollup.PerformAction(UltraGridAction.DeactivateCell);
						break;
					case "StoreToChain":
                        aCell.Row.Cells["STORE_TO_CHAIN_IND"].Value = (Convert.ToBoolean(aCell.Row.Cells["StoreToChain"].Text)) ? 1 : 0;
						ApplySecurity();
						grdRollup.PerformAction(UltraGridAction.DeactivateCell);

						break;

					default:
						break;
				}
				// End Track #5852 (3) stodd

				//if (aCell == aRow.Cells["Store"]
				//    || aCell == aRow.Cells["Chain"]
				//    || aCell == aRow.Cells["StoreToChain"])
				//{
				//    ApplySecurity();
				//}
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}
		// End Track 5852

        private void FillLevelValueLists(UltraGridRow aRow)
        {
            Infragistics.Win.ValueList fromValList;
            Infragistics.Win.ValueList toValList;
            HierarchyNodeProfile nodeProf;
            HierarchyProfile hierProf;
            int startLevel;
            int i;
            HierarchyProfile mainHierProf;
            // Begin Track #5960 - JSmith - Get "No Low Levels Found" when using hierarchy level name
            int highestGuestLevel;
            ArrayList guestLevels;
            HierarchyLevelProfile hlp;
            // End Track #5960
            int longestBranchCount;
            int offset;
            HierarchyLevelValueItem valItem;

            try
            {
                if (_OTSRollupMethod.HierNodeRID != Include.NoRID)
                {
                    nodeProf = SAB.HierarchyServerSession.GetNodeData(_OTSRollupMethod.HierNodeRID);
                    hierProf = SAB.HierarchyServerSession.GetHierarchyData(nodeProf.HomeHierarchyRID);

                    // Load Level arrays

                    _fromLevelList.Clear();
                    _toLevelList.Clear();

                    if (hierProf.HierarchyType == eHierarchyType.organizational)
                    {
                        if (nodeProf.HomeHierarchyLevel == 0)
                        {
                            _toLevelList.Add(new HierarchyLevelValueItem(hierProf.Key, 0, hierProf.HierarchyID));
                            _fromLevelList.Add(new HierarchyLevelValueItem(hierProf.Key, 0, hierProf.HierarchyID));
                            startLevel = 1;
                        }
                        else
                        {
                            _toLevelList.Add(new HierarchyLevelValueItem(hierProf.Key, ((HierarchyLevelProfile)hierProf.HierarchyLevels[nodeProf.HomeHierarchyLevel]).Key, ((HierarchyLevelProfile)hierProf.HierarchyLevels[nodeProf.HomeHierarchyLevel]).LevelID));
                            _fromLevelList.Add(new HierarchyLevelValueItem(hierProf.Key, ((HierarchyLevelProfile)hierProf.HierarchyLevels[nodeProf.HomeHierarchyLevel]).Key, ((HierarchyLevelProfile)hierProf.HierarchyLevels[nodeProf.HomeHierarchyLevel]).LevelID));
                            startLevel = nodeProf.HomeHierarchyLevel + 1;
                        }

                        for (i = startLevel; i <= hierProf.HierarchyLevels.Count; i++)
                        {
                            _fromLevelList.Add(new HierarchyLevelValueItem(hierProf.Key, ((HierarchyLevelProfile)hierProf.HierarchyLevels[i]).Key, ((HierarchyLevelProfile)hierProf.HierarchyLevels[i]).LevelID));
                            _toLevelList.Add(new HierarchyLevelValueItem(hierProf.Key, ((HierarchyLevelProfile)hierProf.HierarchyLevels[i]).Key, ((HierarchyLevelProfile)hierProf.HierarchyLevels[i]).LevelID));
                        }
                    }
                    else
                    {
                        mainHierProf = SAB.HierarchyServerSession.GetMainHierarchyData();
                        // Begin Track #5960 - JSmith - Get "No Low Levels Found" when using hierarchy level name
                        highestGuestLevel = SAB.HierarchyServerSession.GetHighestGuestLevel(nodeProf.Key);
                        guestLevels = SAB.HierarchyServerSession.GetAllGuestLevels(nodeProf.Key);
                        // End Track #5960

                        _toLevelList.Add(new HierarchyLevelValueItem(0, nodeProf.NodeID));
                        _fromLevelList.Add(new HierarchyLevelValueItem(0, nodeProf.NodeID));
                        startLevel = 1;

                        // Begin Track #5960 - JSmith - Get "No Low Levels Found" when using hierarchy level name
                        //if (highestGuestLevel != int.MaxValue)
                        //{
                        //    for (i = highestGuestLevel; i <= mainHierProf.HierarchyLevels.Count; i++)
                        //    {
                        //        _fromLevelList.Add(new HierarchyLevelValueItem(mainHierProf.Key, ((HierarchyLevelProfile)mainHierProf.HierarchyLevels[i]).Key, ((HierarchyLevelProfile)mainHierProf.HierarchyLevels[i]).LevelID));
                        //        _toLevelList.Add(new HierarchyLevelValueItem(mainHierProf.Key, ((HierarchyLevelProfile)mainHierProf.HierarchyLevels[i]).Key, ((HierarchyLevelProfile)mainHierProf.HierarchyLevels[i]).LevelID));
                        //    }
                        //}

                        // only show level of main hierarchy if all guests are from the same level
                        if (guestLevels.Count == 1)
                        {
                            hlp = (HierarchyLevelProfile)guestLevels[0];
                            _fromLevelList.Add(new HierarchyLevelValueItem(mainHierProf.Key, hlp.Key, hlp.LevelID));
                            _toLevelList.Add(new HierarchyLevelValueItem(mainHierProf.Key, hlp.Key, hlp.LevelID));
                        }
                        // End Track #5960

                        // Begin Track #5960 - JSmith - Get "No Low Levels Found" when using hierarchy level name
                        //longestBranchCount = SAB.HierarchyServerSession.GetLongestBranch(nodeProf.Key);

                        //BEGIN TT#4689 - DOConnell - OTS Forecast - Multi-Level Low Levels not being populated correctly
                        //longestBranchCount = SAB.HierarchyServerSession.GetLongestBranch(nodeProf.Key, true);
                        DataTable hierarchyLevels = SAB.HierarchyServerSession.GetHierarchyDescendantLevels(nodeProf.Key);
                        longestBranchCount = hierarchyLevels.Rows.Count - 1;
                        //END TT#4689 - DOConnell - OTS Forecast - Multi-Level Low Levels not being populated correctly
                        // End Track #5960
                        offset = 0;

                        for (i = 0; i < longestBranchCount; i++)
                        {
                            offset++;
                            _fromLevelList.Add(new HierarchyLevelValueItem(offset, "+" + offset.ToString()));
                            _toLevelList.Add(new HierarchyLevelValueItem(offset, "+" + offset.ToString()));
                        }
                    }

                    // Setup Level ValueLists

                    if (aRow.Cells["FromLevel"].ValueList == null)
                    {
                        fromValList = new Infragistics.Win.ValueList();
                        aRow.Cells["FromLevel"].ValueList = fromValList;
                    }
                    else
                    {
                        fromValList = (Infragistics.Win.ValueList)aRow.Cells["FromLevel"].ValueList;
                        fromValList.ValueListItems.Clear();
                    }

                    if (aRow.Cells["ToLevel"].ValueList == null)
                    {
                        toValList = new Infragistics.Win.ValueList();
                        aRow.Cells["ToLevel"].ValueList = toValList;
                    }
                    else
                    {
                        toValList = (Infragistics.Win.ValueList)aRow.Cells["ToLevel"].ValueList;
                        toValList.ValueListItems.Clear();
                    }

                    for (i = 0; i < _fromLevelList.Count; i++)
                    {
                        valItem = (HierarchyLevelValueItem)_fromLevelList[i];
                        fromValList.ValueListItems.Add(i, valItem.LevelName);
                    }

                    for (i = 0; i < _toLevelList.Count; i++)
                    {
                        valItem = (HierarchyLevelValueItem)_toLevelList[i];
                        toValList.ValueListItems.Add(i, valItem.LevelName);
                    }

                    if (aRow.Cells["FROM_LEVEL_SEQ"].Value == System.DBNull.Value &&
                        aRow.Cells["FROM_LEVEL_OFFSET"].Value == System.DBNull.Value)
                    {
                        //aRow.Cells["FromLevel"].Value = System.DBNull.Value;
                        if (_fromLevelList.Count >= 2)
                        {
                            aRow.Cells["FromLevel"].Value = 1;

                            HierarchyLevelValueItem valItem1 = (HierarchyLevelValueItem)_fromLevelList[Convert.ToInt32(aRow.Cells["FromLevel"].Value)];
                            aRow.Cells["FROM_LEVEL_TYPE"].Value = valItem1.LevelType;

                            if (valItem1.LevelType == eHierarchyDescendantType.levelType)
                            {
                                aRow.Cells["FROM_LEVEL_HRID"].Value = valItem1.HierarchyRID;
                                aRow.Cells["FROM_LEVEL_SEQ"].Value = valItem1.LevelRID;
                                aRow.Cells["FROM_LEVEL_OFFSET"].Value = System.DBNull.Value;
                            }
                            else
                            {
                                aRow.Cells["FROM_LEVEL_HRID"].Value = System.DBNull.Value;
                                aRow.Cells["FROM_LEVEL_SEQ"].Value = System.DBNull.Value;
                                aRow.Cells["FROM_LEVEL_OFFSET"].Value = valItem1.Offset;
                            }
                        }
                        else if (_fromLevelList.Count >= 1)
                        {
                            aRow.Cells["FromLevel"].Value = 0;

                            HierarchyLevelValueItem valItem1 = (HierarchyLevelValueItem)_fromLevelList[Convert.ToInt32(aRow.Cells["FromLevel"].Value)];
                            aRow.Cells["FROM_LEVEL_TYPE"].Value = valItem1.LevelType;

                            if (valItem1.LevelType == eHierarchyDescendantType.levelType)
                            {
                                aRow.Cells["FROM_LEVEL_HRID"].Value = valItem1.HierarchyRID;
                                aRow.Cells["FROM_LEVEL_SEQ"].Value = valItem1.LevelRID;
                                aRow.Cells["FROM_LEVEL_OFFSET"].Value = System.DBNull.Value;
                            }
                            else
                            {
                                aRow.Cells["FROM_LEVEL_HRID"].Value = System.DBNull.Value;
                                aRow.Cells["FROM_LEVEL_SEQ"].Value = System.DBNull.Value;
                                aRow.Cells["FROM_LEVEL_OFFSET"].Value = valItem1.Offset;
                            }
                        }
                        else
                        {
                            aRow.Cells["FromLevel"].Value = System.DBNull.Value;
                        }
                    }
                    else
                    {
                        for (i = 0; i < _fromLevelList.Count; i++)
                        {
                            valItem = (HierarchyLevelValueItem)_fromLevelList[i];

                            if (valItem.LevelType == (eHierarchyDescendantType)Convert.ToInt32(aRow.Cells["FROM_LEVEL_TYPE"].Value) &&
                               ((valItem.LevelType == eHierarchyDescendantType.levelType && valItem.HierarchyRID == Convert.ToInt32(aRow.Cells["FROM_LEVEL_HRID"].Value) && valItem.LevelRID == Convert.ToInt32(aRow.Cells["FROM_LEVEL_SEQ"].Value)) ||
                               (valItem.LevelType == eHierarchyDescendantType.offset && valItem.Offset == Convert.ToInt32(aRow.Cells["FROM_LEVEL_OFFSET"].Value))))
                            {
                                aRow.Cells["FromLevel"].Value = i;
                                break;
                            }
                        }

                        if (i == _fromLevelList.Count)
                        {
                          //aRow.Cells["FromLevel"].Value = System.DBNull.Value;
                            if (_fromLevelList.Count >= 2)
                            {
                                aRow.Cells["FromLevel"].Value = 1;

                                HierarchyLevelValueItem valItem1 = (HierarchyLevelValueItem)_fromLevelList[Convert.ToInt32(aRow.Cells["FromLevel"].Value)];
                                aRow.Cells["FROM_LEVEL_TYPE"].Value = valItem1.LevelType;

                                if (valItem1.LevelType == eHierarchyDescendantType.levelType)
                                {
                                    aRow.Cells["FROM_LEVEL_HRID"].Value = valItem1.HierarchyRID;
                                    aRow.Cells["FROM_LEVEL_SEQ"].Value = valItem1.LevelRID;
                                    aRow.Cells["FROM_LEVEL_OFFSET"].Value = System.DBNull.Value;
                                }
                                else
                                {
                                    aRow.Cells["FROM_LEVEL_HRID"].Value = System.DBNull.Value;
                                    aRow.Cells["FROM_LEVEL_SEQ"].Value = System.DBNull.Value;
                                    aRow.Cells["FROM_LEVEL_OFFSET"].Value = valItem1.Offset;
                                }
                            }
                            else if (_fromLevelList.Count >= 1) 
                            {
                                aRow.Cells["FromLevel"].Value = 0;

                                HierarchyLevelValueItem valItem1 = (HierarchyLevelValueItem)_fromLevelList[Convert.ToInt32(aRow.Cells["FromLevel"].Value)];
                                aRow.Cells["FROM_LEVEL_TYPE"].Value = valItem1.LevelType;

                                if (valItem1.LevelType == eHierarchyDescendantType.levelType)
                                {
                                    aRow.Cells["FROM_LEVEL_HRID"].Value = valItem1.HierarchyRID;
                                    aRow.Cells["FROM_LEVEL_SEQ"].Value = valItem1.LevelRID;
                                    aRow.Cells["FROM_LEVEL_OFFSET"].Value = System.DBNull.Value;
                                }
                                else
                                {
                                    aRow.Cells["FROM_LEVEL_HRID"].Value = System.DBNull.Value;
                                    aRow.Cells["FROM_LEVEL_SEQ"].Value = System.DBNull.Value;
                                    aRow.Cells["FROM_LEVEL_OFFSET"].Value = valItem1.Offset;
                                }
                            }
                            else
                            {
                                aRow.Cells["FromLevel"].Value = System.DBNull.Value;
                            }
                        }
                    }

                    if (aRow.Cells["TO_LEVEL_SEQ"].Value == System.DBNull.Value &&
                        aRow.Cells["TO_LEVEL_OFFSET"].Value == System.DBNull.Value)
                    {
                        //aRow.Cells["ToLevel"].Value = System.DBNull.Value;
                        if (_toLevelList.Count >= 1)
                        {
                            aRow.Cells["ToLevel"].Value = 0;

                            HierarchyLevelValueItem valItem1 = (HierarchyLevelValueItem)_toLevelList[Convert.ToInt32(aRow.Cells["ToLevel"].Value)];
                            aRow.Cells["TO_LEVEL_TYPE"].Value = valItem1.LevelType;

                            if (valItem1.LevelType == eHierarchyDescendantType.levelType)
                            {
                                aRow.Cells["TO_LEVEL_HRID"].Value = valItem1.HierarchyRID;
                                aRow.Cells["TO_LEVEL_SEQ"].Value = valItem1.LevelRID;
                                aRow.Cells["TO_LEVEL_OFFSET"].Value = System.DBNull.Value;
                            }
                            else
                            {
                                aRow.Cells["TO_LEVEL_HRID"].Value = System.DBNull.Value;
                                aRow.Cells["TO_LEVEL_SEQ"].Value = System.DBNull.Value;
                                aRow.Cells["TO_LEVEL_OFFSET"].Value = valItem1.Offset;
                            }
                        }
                        else
                        {
                            aRow.Cells["ToLevel"].Value = System.DBNull.Value;
                        }
                    }
                    else
                    {
                        for (i = 0; i < _toLevelList.Count; i++)
                        {
                            valItem = (HierarchyLevelValueItem)_toLevelList[i];

                            if (valItem.LevelType == (eHierarchyDescendantType)Convert.ToInt32(aRow.Cells["TO_LEVEL_TYPE"].Value) &&
                               ((valItem.LevelType == eHierarchyDescendantType.levelType && valItem.HierarchyRID == Convert.ToInt32(aRow.Cells["TO_LEVEL_HRID"].Value) && valItem.LevelRID == Convert.ToInt32(aRow.Cells["TO_LEVEL_SEQ"].Value)) ||
                               (valItem.LevelType == eHierarchyDescendantType.offset && valItem.Offset == Convert.ToInt32(aRow.Cells["TO_LEVEL_OFFSET"].Value))))
                            {
                                aRow.Cells["ToLevel"].Value = i;
                                break;
                            }
                        }

                        if (i == _toLevelList.Count)
                        {
                          //aRow.Cells["ToLevel"].Value = System.DBNull.Value;
                            if (_toLevelList.Count >= 1)
                            {
                                aRow.Cells["ToLevel"].Value = 0;

                                HierarchyLevelValueItem valItem1 = (HierarchyLevelValueItem)_toLevelList[Convert.ToInt32(aRow.Cells["ToLevel"].Value)];
                                aRow.Cells["TO_LEVEL_TYPE"].Value = valItem1.LevelType;

                                if (valItem1.LevelType == eHierarchyDescendantType.levelType)
                                {
                                    aRow.Cells["TO_LEVEL_HRID"].Value = valItem1.HierarchyRID;
                                    aRow.Cells["TO_LEVEL_SEQ"].Value = valItem1.LevelRID;
                                    aRow.Cells["TO_LEVEL_OFFSET"].Value = System.DBNull.Value;
                                }
                                else
                                {
                                    aRow.Cells["TO_LEVEL_HRID"].Value = System.DBNull.Value;
                                    aRow.Cells["TO_LEVEL_SEQ"].Value = System.DBNull.Value;
                                    aRow.Cells["TO_LEVEL_OFFSET"].Value = valItem1.Offset;
                                }
                            }
                            else 
                            {
                                aRow.Cells["ToLevel"].Value = System.DBNull.Value;
                            }
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

		private void BuildBasisContextMenu()
		{
			try
			{
				MenuItem mnuItemInsert = new MenuItem(MIDText.GetTextOnly(eMIDTextCode.lbl_Insert));
				MenuItem mnuItemInsertBefore = new MenuItem(MIDText.GetTextOnly(eMIDTextCode.lbl_Insert_Before));
				MenuItem mnuItemInsertAfter = new MenuItem(MIDText.GetTextOnly(eMIDTextCode.lbl_Insert_after));
				MenuItem mnuItemDelete = new MenuItem(MIDText.GetTextOnly(eMIDTextCode.lbl_Delete));
				MenuItem mnuItemDeleteAll = new MenuItem(MIDText.GetTextOnly(eMIDTextCode.lbl_DeleteAll));
				mnuBasisGrid.MenuItems.Add(mnuItemInsert);
				mnuItemInsert.MenuItems.Add(mnuItemInsertBefore);
				mnuItemInsert.MenuItems.Add(mnuItemInsertAfter);
				mnuBasisGrid.MenuItems.Add(mnuItemDelete);
				mnuBasisGrid.MenuItems.Add(mnuItemDeleteAll);
				mnuItemInsert.Click += new System.EventHandler(this.mnuBasisGridItemInsert_Click);
				mnuItemInsertBefore.Click += new System.EventHandler(this.mnuBasisGridItemInsertBefore_Click);
				mnuItemInsertAfter.Click += new System.EventHandler(this.mnuBasisGridItemInsertAfter_Click);
				mnuItemDelete.Click += new System.EventHandler(this.mnuBasisGridItemDelete_Click);
				mnuItemDeleteAll.Click += new System.EventHandler(this.mnuBasisGridItemDeleteAll_Click);
			}
			catch(Exception ex)
			{
				HandleException(ex);
			}
		}
	
		private void mnuBasisGridItemInsert_Click(object sender, System.EventArgs e)
		{
		}
		private void mnuBasisGridItemInsertBefore_Click(object sender, System.EventArgs e)
		{
			InsertBasis(true);
		}
		private void mnuBasisGridItemInsertAfter_Click(object sender, System.EventArgs e)
		{
			InsertBasis(false);
		}

		private void InsertBasis (bool InsertBeforeRow)
		{
			int rowPosition = 0;
			try
			{
				if (grdRollup.Rows.Count > 0)
				{
					if (grdRollup.ActiveRow == null) return;
					rowPosition = Convert.ToInt32(grdRollup.ActiveRow.Cells["DETAIL_SEQ"].Value, CultureInfo.CurrentUICulture);
					int seq;
					foreach (DataRow row in _dtBasis.Rows)
					{
						seq = (int)row["DETAIL_SEQ"];
						if (InsertBeforeRow)
						{
							if (seq >= rowPosition)
								row["DETAIL_SEQ"] = seq + 1;
						}
						else
						{
							if (seq > rowPosition)
								row["DETAIL_SEQ"] = seq + 1;
						}
					}
					if (!InsertBeforeRow)
						rowPosition++;
				}
				DataRow addedRow = _dtBasis.NewRow();
				//addedRow["SGL_RID"] = Convert.ToInt32(cboAttributeSet.SelectedValue,CultureInfo.CurrentUICulture);
				addedRow["DETAIL_SEQ"] = rowPosition;
                addedRow["FROM_LEVEL_HRID"] = 0;
                addedRow["FROM_LEVEL_TYPE"] = 0;
                addedRow["FROM_LEVEL_SEQ"] = 0;
                addedRow["FROM_LEVEL_OFFSET"] = 0;
                addedRow["TO_LEVEL_HRID"] = 0;
                addedRow["TO_LEVEL_TYPE"] = 0;
                addedRow["TO_LEVEL_SEQ"] = 0; 
                addedRow["TO_LEVEL_OFFSET"] = 0;
                addedRow["STORE_IND"] = 0;
                addedRow["CHAIN_IND"] = 0;
                addedRow["STORE_TO_CHAIN_IND"] = 0;
                //addedRow["HN_RID"] = Include.NoRID;
				//addedRow["CDR_RID"] = Include.UndefinedCalendarDateRange;
				//addedRow["INCLUDE_EXCLUDE"] = (int)eBasisIncludeExclude.Include;

                _dtBasis.Rows.Add(addedRow);
				_dtBasis.AcceptChanges();
				grdRollup.DisplayLayout.Bands[0].SortedColumns.Clear();
				grdRollup.DisplayLayout.Bands[0].SortedColumns.Add("DETAIL_SEQ", false);
                grdRollup.Rows[rowPosition].Cells["Store"].Value = false;
                grdRollup.Rows[rowPosition].Cells["STORE_IND"].Value = 0;
                grdRollup.Rows[rowPosition].Cells["Chain"].Value = false;
                grdRollup.Rows[rowPosition].Cells["CHAIN_IND"].Value = 0;
                grdRollup.Rows[rowPosition].Cells["StoreToChain"].Value = false;
                grdRollup.Rows[rowPosition].Cells["STORE_TO_CHAIN_IND"].Value = 0;
                FillLevelValueLists(grdRollup.Rows[rowPosition]);
                ChangePending = true;
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}	
		private void mnuBasisGridItemDelete_Click(object sender, System.EventArgs e)
		{
			if (grdRollup.Selected.Rows.Count > 0)
			{
				grdRollup.DeleteSelectedRows();
				_dtBasis.AcceptChanges();
				ChangePending = true;
			}
		}

		private void mnuBasisGridItemDeleteAll_Click(object sender, System.EventArgs e)
		{
			_dtBasis.Rows.Clear();
			_dtBasis.AcceptChanges();
			ChangePending = true;
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
			//int storeFilterRID;
			try
			{
                //_OTSRollupMethod.HierNodeRID = Convert.ToInt32(txtSpreadNode.Tag, CultureInfo.CurrentUICulture);
                _OTSRollupMethod.HierNodeRID = ((HierarchyNodeProfile)((MIDTag)(txtSpreadNode.Tag)).MIDTagData).Key;
                _OTSRollupMethod.VersionRID = Convert.ToInt32(cboSpreadVersion.SelectedValue, CultureInfo.CurrentUICulture);

                //_OTSRollupMethod.FromLevelType = ((FromLevelCombo)cboFromLevel.SelectedItem).FromLevelType;
                //_OTSRollupMethod.FromLevelOffset = ((FromLevelCombo)cboFromLevel.SelectedItem).FromLevelOffset;
                //_OTSRollupMethod.FromLevelSequence = ((FromLevelCombo)cboFromLevel.SelectedItem).FromLevelSequence;
                //_OTSRollupMethod.ToLevelType = ((ToLevelCombo)cboToLevel.SelectedItem).ToLevelType;
                //_OTSRollupMethod.ToLevelOffset = ((ToLevelCombo)cboToLevel.SelectedItem).ToLevelOffset;
                //_OTSRollupMethod.ToLevelSequence = ((ToLevelCombo)cboToLevel.SelectedItem).ToLevelSequence;

                _OTSRollupMethod.DateRangeRID = mdsPlanDateRange.DateRangeRID;

				grdRollup.UpdateData();

                _OTSRollupMethod.DSRollup = _dsRollup;
                //if (_OTSRollupMethod.SpreadOption == eSpreadOption.Plan)
                //{
                //    _OTSRollupMethod.DSRollup.Tables["Basis"].Rows.Clear();
                //}
                //_OTSRollupMethod.DTLowerLevels = _dtLowerLevels; <---DKJ
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
				if (txtSpreadNode.Text.Trim().Length == 0)
				{
					methodFieldsValid = false;
					ErrorProvider.SetError (txtSpreadNode,SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_FieldIsRequired));
				}
				else
				{
					ErrorProvider.SetError (txtSpreadNode,string.Empty);
				}
				
				if (Convert.ToInt32(cboSpreadVersion.SelectedValue, CultureInfo.CurrentUICulture) == Include.NoRID)
				{
					methodFieldsValid = false;
					ErrorProvider.SetError (cboSpreadVersion,SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_FieldIsRequired));
				}
				else
				{
					ErrorProvider.SetError (cboSpreadVersion,string.Empty);
				}

				if (mdsPlanDateRange.DateRangeRID == Include.UndefinedCalendarDateRange)
				{
					methodFieldsValid = false;
					ErrorProvider.SetError (mdsPlanDateRange,SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_FieldIsRequired));
				}
				// BEGIN MID Track #5149 - error when basis weeks > high level weeks
				else
				{
					ErrorProvider.SetError (mdsPlanDateRange,string.Empty);
					DateRangeProfile drp = SAB.ClientServerSession.Calendar.GetDateRange(mdsPlanDateRange.DateRangeRID, null);
					_highLevelWeekCount = SAB.ClientServerSession.Calendar.GetDateRangeWeeks(drp,null).Count;
				}
				// END MID Track #5149 

                //if (cboFromLevel.Items.Count == 0)
                //{
                //    methodFieldsValid = false;
                //    ErrorProvider.SetError(cboFromLevel, SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_pl_FromLevelsNotDefined));
                //}
                //else
                //{
                //    ErrorProvider.SetError(cboFromLevel, string.Empty);
                //    // BEGIN MID Track #5149 - error when basis weeks > high level weeks
                //    DateRangeProfile drp = SAB.ClientServerSession.Calendar.GetDateRange(mdsPlanDateRange.DateRangeRID, null);
                //    _highLevelWeekCount = SAB.ClientServerSession.Calendar.GetDateRangeWeeks(drp, null).Count;
                //    // END MID Track #5149  
                //}

                //if (cboToLevel.Items.Count == 0)
                //{
                //    methodFieldsValid = false;
                //    ErrorProvider.SetError(cboToLevel, SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_pl_ToLevelsNotDefined));
                //}
                //else
                //{
                //    ErrorProvider.SetError(cboToLevel, string.Empty);
                //    // BEGIN MID Track #5149 - error when basis weeks > high level weeks
                //    DateRangeProfile drp = SAB.ClientServerSession.Calendar.GetDateRange(mdsPlanDateRange.DateRangeRID, null);
                //    _highLevelWeekCount = SAB.ClientServerSession.Calendar.GetDateRangeWeeks(drp, null).Count;
                //    // END MID Track #5149  
                //}

                if (methodFieldsValid) 
				{
					if (!ValidBasisGrid())
					{
						methodFieldsValid = false;
						this.tabSpreadMethod.SelectedTab = this.tabMethod;
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
		
		override public bool VerifySecurity()
		{
			return true;
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
				ErrorProvider.SetError (pnlGlobalUser,UserGlobalMessage);
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
                ABM = _OTSRollupMethod;
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

		#region Validate Basis Grid
		private bool ValidBasisGrid()			
		{
			bool errorFound = false;
			string errorMessage = string.Empty;
			ErrorProvider.SetError (grdRollup,string.Empty);
			try
			{
				//if (rbBasis.Checked) //  ANF - Weighting Multiple Basis
				//{
					////double totWeight = 0; 
					// BEGIN Issue 4233 stodd 9.21.2007
					//if (grdRollup.Rows.Count == 0 && this.rbBasis.Checked == true)
					//{
					//	errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_pl_BasisRequired);
					//	grdRollup.Tag = errorMessage;
					//	ErrorProvider.SetError (grdRollup,errorMessage);
					//	errorFound = true;
					//}
					// END Issue 4233 stodd 9.21.2007

					if (grdRollup.Rows.Count > 0)
					{
                        _atLeastOneCheck = false;
						foreach (UltraGridRow gridRow in grdRollup.Rows)
						{
							InitGridRowForValidation(gridRow);
                            if (!ValidateForAtLeastOneCheck(gridRow.Cells["Store"]))
                            {
                                errorFound = true;
                            }
							// Begin Track #5858 stodd
							// The method above takes care of all of the cells in the row.
							//if (!ValidateForAtLeastOneCheck(gridRow.Cells["Chain"]))
							//{
							//    errorFound = true;
							//}
							//if (!ValidateForAtLeastOneCheck(gridRow.Cells["StoreToChain"]))
							//{
							//    errorFound = true;
							//}
							// ENd Track #5858
                            if (!ValidateFrom(gridRow.Cells["FromLevel"]))
                            {
                                errorFound = true;
                            }
                            if (!ValidateTo(gridRow.Cells["ToLevel"]))
                            {
                                errorFound = true;
                            }
                            if (!ValidateCheckAgainstFromTo(gridRow.Cells["Store"]))
                            {
                                errorFound = true;
                            }
                            if (!ValidateCheckAgainstFromTo(gridRow.Cells["Chain"]))
                            {
                                errorFound = true;
                            }
                            //if (!ValidVersion(gridRow.Cells["FV_RID"]))
                            //{
                            //    errorFound = true;
                            //}
                            //if (!DateRangeValid(gridRow.Cells["DateRange"]))
                            //{
                            //    errorFound = true;
                            //}
                            //if (!WeightValid(gridRow.Cells["WEIGHT"]))
                            //{
                            //    errorFound = true;
                            //}
                            // BEGIN ANF - Weighting Multiple Basis
                            //else if (rbEWYes.Checked)
                            //{
                            //    totWeight += Convert.ToDouble(gridRow.Cells["WEIGHT"].Value, CultureInfo.CurrentUICulture);
                            //}
						}

						// Begin Track #5858 stodd
						//if (_atLeastOneCheck)
						//{
						//    System.Windows.Forms.MessageBox.Show(this,
						//        "Either Store, Chain or StoreToChain must be checked for processing to work.",
						//        this.Text.Replace("*", ""),
						//        MessageBoxButtons.OK,
						//        MessageBoxIcon.Warning);
						//}
						// End track #5858

                        //if (rbEWYes.Checked && totWeight != 1)
                        //{
                        //    errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_pl_BasisWeightTotalInvalid);
                        //    grdRollup.Tag = errorMessage;
                        //    ErrorProvider.SetError(grdRollup, errorMessage);
                        //    errorFound = true;
                        //}
						// END ANF - Weighting Multiple Basis
					}
					// Begin Track #5858
					else
					{
						errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_EntriesLessThanMinimum);
						errorMessage = errorMessage.Replace("{0}", "one");
						errorMessage = errorMessage.Replace("{1}", "rollup item");
						ErrorProvider.SetError(grdRollup, errorMessage);
						errorFound = true;
					}
					// End Track #5858
				//}
			}
			catch( Exception error)
			{
				string exceptionMessage = error.Message;
				errorMessage = error.Message;
				errorFound = true;
			}
			if (errorFound)
				return false;
			else
				return true;
		}

		// Begin Track #5858 stodd
		private void InitGridRowForValidation(UltraGridRow gridRow)
		{
			gridRow.Cells["FromLevel"].Appearance.Image = null;
			gridRow.Cells["FromLevel"].Tag = string.Empty;
			gridRow.Cells["ToLevel"].Appearance.Image = null;
			gridRow.Cells["ToLevel"].Tag = string.Empty;
			gridRow.Cells["Store"].Appearance.Image = null;
			gridRow.Cells["Store"].Tag = string.Empty;
			gridRow.Cells["Chain"].Appearance.Image = null;
			gridRow.Cells["Chain"].Tag = string.Empty;
			gridRow.Cells["StoreToChain"].Appearance.Image = null;
			gridRow.Cells["StoreToChain"].Tag = string.Empty;
		}
		// End Track #5858

        private bool ValidateForAtLeastOneCheck(UltraGridCell gridCell)
        {
            bool errorFound = false;
            string errorMessage = string.Empty;
            bool warningFound = false;
            string warningMessage = string.Empty;
            try
            {
                bool storeCheck = Convert.ToBoolean(gridCell.Row.Cells["Store"].Value);
                bool chainCheck = Convert.ToBoolean(gridCell.Row.Cells["Chain"].Value);
                bool storeToChainCheck = Convert.ToBoolean(gridCell.Row.Cells["StoreToChain"].Value);
                if (!storeCheck && !chainCheck && !storeToChainCheck)
                {
					// Begin Track #5858 stodd
					errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_AtLeastOneCheck);
					errorFound = true;
					// End Track #5858
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
				// Begin Track #5858 stodd
				gridCell.Row.Cells["Store"].Appearance.Image = ErrorImage;
				gridCell.Row.Cells["Store"].Tag = errorMessage;
				gridCell.Row.Cells["Chain"].Appearance.Image = ErrorImage;
				gridCell.Row.Cells["Chain"].Tag = errorMessage;
				gridCell.Row.Cells["StoreToChain"].Appearance.Image = ErrorImage;
				gridCell.Row.Cells["StoreToChain"].Tag = errorMessage;
                //gridCell.Appearance.Image = ErrorImage;
                //gridCell.Tag = errorMessage;
				// End Track #5858
                return false;
            }
            else if (warningFound)
            {
                gridCell.Appearance.Image = WarningImage;
                gridCell.Tag = warningMessage;
                _atLeastOneCheck = true;
                return true;
            }
            else
            {
                //gridCell.Appearance.Image = null;
                //gridCell.Tag = null;
                return true;
            }
        }

        private bool ValidateFrom(UltraGridCell gridCell)
		{
			bool errorFound = false;
			string errorMessage = string.Empty;
			try
			{
				string text = gridCell.Text.TrimEnd(' '); // Issue 3800 - stodd
				if (text.Length == 0)	
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
				//gridCell.Tag = null;
				return true;
			}
		}

        private bool ValidateTo(UltraGridCell gridCell)
        {
            bool errorFound = false;
            string errorMessage = string.Empty;
            try
            {
                string text = gridCell.Text.TrimEnd(' '); // Issue 3800 - stodd
                if (text.Length == 0)
                {
                    errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_FieldIsRequired);
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
                //gridCell.Appearance.Image = null;
                //gridCell.Tag = null;
                return true;
            }
        }

        private bool ValidateCheckAgainstFromTo(UltraGridCell gridCell)
        {
            bool errorFound = false;
            string errorMessage = string.Empty;
            try
            {
                if (Convert.ToBoolean(gridCell.Value) == true)
                {
                    int oldFromIndex = -1;
                    if (gridCell.Row.Cells["FromLevel"].Value != System.DBNull.Value)
                    {
                        oldFromIndex = Convert.ToInt32(gridCell.Row.Cells["FromLevel"].Value);
                    }

                    int oldToIndex = -1;
                    if (gridCell.Row.Cells["ToLevel"].Value != System.DBNull.Value)
                    {
                        oldToIndex = Convert.ToInt32(gridCell.Row.Cells["ToLevel"].Value);
                    }

                    if (oldFromIndex == oldToIndex && oldFromIndex >= 0)
                    {
                        errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_FromToAreEqual);
                        errorFound = true;
                    }
                }
            }	// END MID Track #5149
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
				//if (gridCell.Appearance.Image == ErrorImage)
				//{
				//    gridCell.Appearance.Image = null;
				//    gridCell.Tag = null;
				//}
                return true;
            }
        }

        //private bool ValidVersion(UltraGridCell gridCell)
        //{
        //    bool errorFound = false;
        //    string errorMessage = string.Empty;
        //    try
        //    {
        //        string text = gridCell.Text.TrimEnd(' '); // Issue 3800 - stodd
        //        if (text.Length == 0)
        //        {
        //            errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_FieldIsRequired);
        //            errorFound = true;
        //        }
        //    }
        //    catch (Exception error)
        //    {
        //        string exceptionMessage = error.Message;
        //        errorMessage = error.Message;
        //        errorFound = true;
        //    }

        //    if (errorFound)
        //    {
        //        gridCell.Appearance.Image = ErrorImage;
        //        gridCell.Tag = errorMessage;
        //        return false;
        //    }
        //    else
        //    {
        //        gridCell.Appearance.Image = null;
        //        gridCell.Tag = null;
        //        return true;
        //    }
        //}

        //private bool DateRangeValid(UltraGridCell gridCell)
        //{
        //    bool errorFound = false;
        //    string errorMessage = string.Empty;
        //    try
        //    {
        //        if (gridCell.Text.Length == 0)
        //        {
        //            errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_FieldIsRequired);
        //            errorFound = true;
        //        }
        //        // BEGIN MID Track #5149 - error when basis weeks > high level weeks
        //        else
        //        {
        //            int cdrRID = Convert.ToInt32(gridCell.Row.Cells["CDR_RID"].Value, CultureInfo.CurrentUICulture);
        //            DateRangeProfile drp = SAB.ClientServerSession.Calendar.GetDateRange(cdrRID, null);
        //            int basisWeekCount = SAB.ClientServerSession.Calendar.GetDateRangeWeeks(drp, null).Count;
        //            if (basisWeekCount > _highLevelWeekCount)
        //            {
        //                errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_pl_BasisWeeksExceedHighLevelWeeks);
        //                errorFound = true;
        //            }
        //        }
        //    }	// END MID Track #5149
        //    catch (Exception error)
        //    {
        //        string exceptionMessage = error.Message;
        //        errorMessage = error.Message;
        //        errorFound = true;
        //    }

        //    if (errorFound)
        //    {
        //        gridCell.Appearance.Image = ErrorImage;
        //        gridCell.Tag = errorMessage;
        //        return false;
        //    }
        //    else
        //    {
        //        if (gridCell.Appearance.Image == ErrorImage)
        //        {
        //            gridCell.Appearance.Image = null;
        //            gridCell.Tag = null;
        //        }
        //        return true;
        //    }
        //}


        //private bool WeightValid(UltraGridCell gridCell)
        //{
        //    bool errorFound = false;
        //    string errorMessage = string.Empty;
        //    double dblValue;
        //    try
        //    {
        //        if (gridCell.Text.Length == 0)
        //        {
        //            errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_FieldIsRequired);
        //            errorFound = true;
        //        }
        //        else
        //        {
        //            dblValue = Convert.ToDouble(gridCell.Text, CultureInfo.CurrentUICulture);
        //            // BEGIN ANF - Weighting Multiple Basis
        //            //if (dblValue < 1)
        //            //{
        //            //	errorMessage = string.Format
        //            //		(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_al_MinimumValueExceeded),dblValue, "1");
        //            //	errorFound = true;
        //            //}
        //            if (dblValue <= 0)
        //            {
        //                errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_pl_BasisWeightInvalid);
        //                errorFound = true;
        //            }
        //            else if (dblValue > 9999)
        //            {
        //                errorMessage = string.Format
        //                    (SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_al_MaximumValueExceeded), dblValue, "9999");
        //                errorFound = true;
        //            }

        //            //if (rbEWYes.Checked)
        //            //{
        //            if (dblValue > 1)
        //            {
        //                errorMessage = string.Format
        //                    (SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_al_MaximumValueExceeded), dblValue, "1");
        //                errorFound = true;
        //            }
        //            //}	
        //            // END ANF - Weighting Multiple Basis
        //        }
        //    }
        //    catch (Exception error)
        //    {
        //        string exceptionMessage = error.Message;
        //        errorMessage = error.Message;
        //        errorFound = true;
        //    }

        //    if (errorFound)
        //    {
        //        gridCell.Appearance.Image = ErrorImage;
        //        gridCell.Tag = errorMessage;
        //        return false;
        //    }
        //    else
        //    {
        //        gridCell.Appearance.Image = null;
        //        gridCell.Tag = null;
        //        return true;
        //    }
        //}	

		#endregion Validate Basis Grid		
	
		#region Button Events
		// Begin MID Track 4858 - JSmith - Security changes
//		private void btnProcess_Click(object sender, System.EventArgs e)
//		{
//			try
//			{
//				_basisNodeRequired = true;
//				ProcessAction(eMethodType.Rollup);
//				// as part of the  processing we saved the info, so it should be changed to update.
//				if (!ErrorFound)
//				{
//					_OTSRollupMethod.Method_Change_Type = eChangeType.update;
//					btnSave.Text = "&Update";
//				}
//			}
//			catch(Exception ex)
//			{
//				HandleException(ex);
//			}
//		}
//
//
//		private void btnSave_Click(object sender, System.EventArgs e)
//		{
//			try
//			{
//				_basisNodeRequired = false;
//				this.Save_Click(false);
//
//				if (!ErrorFound)
//				{
//					// Now that this one has been saved, it should be changed to update.
//					_OTSRollupMethod.Method_Change_Type = eChangeType.update;
//					btnSave.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_Update);
//				}
//			}
//			catch(Exception err)
//			{
//				HandleException(err);
//			}
//		
//		}
//
//		private void btnClose_Click(object sender, System.EventArgs e)
//		{
//			try
//			{
//				_basisNodeRequired = false;
//				Cancel_Click();
//			}
//			catch(Exception err)
//			{
//				HandleException(err);
//			}
//		}

		protected override void Call_btnProcess_Click()
		{
			try
			{
				_basisNodeRequired = true;
                ProcessAction(eMethodType.Rollup);
				// as part of the  processing we saved the info, so it should be changed to update.
				if (!ErrorFound)
				{
                    _OTSRollupMethod.Method_Change_Type = eChangeType.update;
					btnSave.Text = "&Update";

                    lblDateTimeValue.Text = _OTSRollupMethod.LastProcessedDateTime;
                    lblByUserValue.Text = _OTSRollupMethod.LastProcessedUser;
				}
			}
			catch(Exception ex)
			{
				HandleException(ex);
			}
		}

		protected override void Call_btnSave_Click()
		{
			try
			{
				_basisNodeRequired = false;
				base.btnSave_Click();
			}
			catch( Exception exception )
			{
				HandleException(exception);
			}	
		}
		// End MID Track 4858

		#endregion Button Events

        private void tabSpreadMethod_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			
		}
		
		#region TextBox Events

		private void txtSpreadNode_TextChanged(object sender, System.EventArgs e)
		{
			if (FormLoaded)
			{
				ChangePending = true;
			}
		}
		
		private void txtSpreadNode_DragEnter(object sender, System.Windows.Forms.DragEventArgs e)
		{
            Merchandise_DragEnter(sender, e);
        }

		private void txtSpreadNode_DragDrop(object sender, System.Windows.Forms.DragEventArgs e)
		{
            //IDataObject data;
            //ClipboardProfile cbp;
            //HierarchyClipboardData MIDTreeNode_cbd;
            HierarchyNodeProfile hnp;

			try
			{

                //Begin Track #5858 - Kjohnson - Validating store security only
                bool isSuccessfull = ((MIDTextBoxTag)(((TextBox)sender).Tag)).TextBox_DragDrop(sender, e);

                if (isSuccessfull)
                {
                    hnp = (HierarchyNodeProfile)((MIDTag)(((TextBox)sender).Tag)).MIDTagData;

                    CheckReplaceNode((TextBox)sender, hnp.Key);

                    ChangePending = true;
                }
                //End Track #5858 - Kjohnson

                //// Create a new instance of the DataObject interface.

                //data = Clipboard.GetDataObject();

                ////If the data is ClipboardProfile, then retrieve the data
				
                //if (data.GetDataPresent(ClipboardProfile.Format.Name))
                //{
                //    cbp = (ClipboardProfile)data.GetData(ClipboardProfile.Format.Name);

                //    //if (cbp.ClipboardDataType == eClipboardDataType.HierarchyNode)
                //    if (cbp.ClipboardDataType == eProfileType.HierarchyNode)
                //    {
                //        if (cbp.ClipboardData.GetType() == typeof(HierarchyClipboardData))
                //        {
                //            // Begin Track #5744 - JSmith - Rollup Method
                //            //bool processDragDrop = false;

                //            //if (this.grdRollup.Rows.Count > 0)
                //            //{
                //            //    DialogResult dr = DialogResult.Yes;
                //            //    dr = MessageBox.Show("This will cause items in the base item table below to be deleted.  Are you sure you want to continue?",
                //            //        MIDText.GetTextOnly(eMIDTextCode.frm_Rollup), MessageBoxButtons.YesNo);

                //            //    if (dr == DialogResult.Yes)
                //            //    {
                //            //      //---Clear Out All Items From Base Grid-------
                //            //        _dtBasis.Rows.Clear();
                //            //        _dtBasis.AcceptChanges();
                //            //        ChangePending = true;

                //            //        processDragDrop = true;
                //            //    }
                //            //}
                //            //else
                //            //{
                //            //    processDragDrop = true;
                //            //}

                //            //if (processDragDrop)
                //            //{
                //            //    //---Do Drag Drop Processing------------------
                //            //    MIDTreeNode_cbd = (HierarchyClipboardData)cbp.ClipboardData;
                //            //    HierarchyNodeProfile hnp = SAB.HierarchyServerSession.GetNodeData(cbp.Key);
                //            //    _OTSRollupMethod.HierNodeRID = cbp.Key;
                //            //    ((TextBox)sender).Text = hnp.Text;
                //            //    ((TextBox)sender).Tag = cbp.Key;

                //            //    txtSpreadNode.Text = hnp.Text;
                //            //    txtSpreadNode.Tag = cbp.Key;
                //            //    // BEGIN Issue 4858 stodd
                //            //    base.ValidatePlanNodeSecurity(txtSpreadNode, true);

                //            //    //ApplySecurity();  

                //            //    bool canUpdate = ABM.AuthorizedToUpdate(this.SAB.ClientServerSession, this.SAB.ClientServerSession.UserRID);
                //            //    base.ApplyCanUpdate(canUpdate);
                //            //    // END Issue 4858
                //            //}

                //            MIDTreeNode_cbd = (HierarchyClipboardData)cbp.ClipboardData;
                //            CheckReplaceNode((TextBox)sender, cbp.Key);
                //            // End Track #5744
                //        }
                //        else
                //        {
                //            MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_BadDataInClipboard));
                //        }
                //    }
                //    else
                //    {
                //        MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MustBeNodeToDrop));
                //    }
                //}
                //else
                //{
                //    MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MustBeNodeToDrop));
                //}
			}
			catch (Exception exc)
			{
				HandleException(exc);
			}
		}

        private void CheckReplaceNode(TextBox aTextBox, int aNodeRID)
        {
            bool replaceNode;
            HierarchyNodeProfile currHnp;
            HierarchyNodeProfile newHnp;
            DialogResult dr;
            bool clearTable;
            try
            {
                replaceNode = false;
                clearTable = true;

                newHnp = SAB.HierarchyServerSession.GetNodeData(aNodeRID, false);
                if (_OTSRollupMethod.HierNodeRID != Include.NoRID)
                {
                    currHnp = SAB.HierarchyServerSession.GetNodeData(_OTSRollupMethod.HierNodeRID, false);
                }
                else
                {
                    currHnp = new HierarchyNodeProfile(Include.NoRID);
                }

                if (this.grdRollup.Rows.Count > 0)
                {
                    dr = DialogResult.Yes;
                    if (currHnp.HomeHierarchyLevel != newHnp.HomeHierarchyLevel)
                    {
                        dr = MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_BasisItemsDeleteWarning),
                            MIDText.GetTextOnly(eMIDTextCode.frm_Rollup), MessageBoxButtons.YesNo);
                        if (dr == DialogResult.Yes)
                        {
                            replaceNode = true;
                        }
                        else
                        {
                            clearTable = false;
                        }
                    }
                    else
                    {
                        dr = MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_KeepBasisItems),
                            MIDText.GetTextOnly(eMIDTextCode.frm_Rollup), MessageBoxButtons.YesNo);
                        if (dr == DialogResult.Yes)
                        {
                            clearTable = false;
                            replaceNode = true;
                        }
                    }

                    if (clearTable)
                    {
                        //---Clear Out All Items From Base Grid-------
                        _dtBasis.Rows.Clear();
                        _dtBasis.AcceptChanges();
                        ChangePending = true;

                        replaceNode = true;
                    }
                }
                else
                {
                    replaceNode = true;
                }

                if (replaceNode)
                {
                    _OTSRollupMethod.HierNodeRID = aNodeRID;
                    aTextBox.Text = newHnp.Text;
                    //aTextBox.Tag = aNodeRID;
                    ((MIDTag)(aTextBox.Tag)).MIDTagData = newHnp;
					// Begin track #5859 stodd
					//// Begin Track #5858 - JSmith - Validating store security only
					////base.ValidatePlanNodeSecurity(aTextBox, true);
					//base.ValidatePlanNodeSecurity(aTextBox, true, eSecurityTypes.Chain | eSecurityTypes.Store);
					//// End Track #5858
					//bool canUpdate = ABM.AuthorizedToUpdate(this.SAB.ClientServerSession, this.SAB.ClientServerSession.UserRID);
					//base.ApplyCanUpdate(canUpdate);
					ApplySecurity();
					// End track #5859 stodd
                }
                else // put the prior values back
                {
                    aTextBox.Text = currHnp.Text;
                    //aTextBox.Tag = _OTSRollupMethod.HierNodeRID;
                    ((MIDTag)(aTextBox.Tag)).MIDTagData = SAB.HierarchyServerSession.GetNodeData(_OTSRollupMethod.HierNodeRID);
                }

                // Begin Track #6398 - JSmith - Node ID not refreshing on drag/drop
                aTextBox.Invalidate();
                // End Track #6398
            }
            catch
            {
                throw;
            }
        }

		//private void txtSpreadNode_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		//{
		//	e.Handled = true;
		//}

		private void txtSpreadNode_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			_needsValidated = true;
		}

		
		
		private void txtSpreadNode_Validating(object sender, System.ComponentModel.CancelEventArgs e)
		{
            //string productID; 
            //try
            //{
            //    if (txtSpreadNode.Modified)
            //    {
            //        if (txtSpreadNode.Text.Trim().Length > 0)
            //        {
            //            productID = txtSpreadNode.Text.Trim();
            //            _nodeRID = GetNodeText(ref productID);
            //            if (_nodeRID == Include.NoRID)
            //                MessageBox.Show(productID + " is not valid; please enter or drag and drop a node from the tree");
            //            else 
            //            {
            //                // Begin Track #5744 - JSmith - Rollup Method
            //                //txtSpreadNode.Text = productID;
            //                //txtSpreadNode.Tag = _nodeRID;
            //                //_OTSRollupMethod.HierNodeRID = _nodeRID;

            //                //HierarchyNodeProfile hnp = SAB.HierarchyServerSession.GetNodeData(_nodeRID);

            //                //// BEGIN Issue 5744 KJohnson
            //                //if (this.grdRollup.Rows.Count > 0)
            //                //{
            //                //    DialogResult dr = DialogResult.Yes;
            //                //    dr = MessageBox.Show("This will cause items in the base item table below to be deleted.  Are you sure you want to continue?",
            //                //        MIDText.GetTextOnly(eMIDTextCode.frm_Rollup), MessageBoxButtons.YesNo);

            //                //    if (dr == DialogResult.Yes)
            //                //    {
            //                //      //---Clear Out All Items From Base Grid-------
            //                //        _dtBasis.Rows.Clear();
            //                //        _dtBasis.AcceptChanges();
            //                //    }
            //                //}
            //                //// END Issue 5744 KJohnson

            //                //// BEGIN Issue 4858 stodd
            //                //base.ValidatePlanNodeSecurity(txtSpreadNode, true);

            //                ////ApplySecurity();

            //                //bool canUpdate = ABM.AuthorizedToUpdate(this.SAB.ClientServerSession, this.SAB.ClientServerSession.UserRID);
            //                //base.ApplyCanUpdate(canUpdate);
            //                //// END Issue 4858
            //                CheckReplaceNode((TextBox)sender, _nodeRID);
            //                // End Track #5744
            //            }
            //        }
            //        else
            //        {
            //            //txtSpreadNode.Tag = null;
            //            ((MIDTag)(txtSpreadNode.Tag)).MIDTagData = null;
            //        }
            //    }
            //}
            //catch
            //{
            //    throw;
            //}
		}

        private void txtSpreadNode_Validated(object sender, EventArgs e)
        {
            if (((MIDTag)(txtSpreadNode.Tag)).MIDTagData != null)
            {
                _nodeRID = ((HierarchyNodeProfile)((MIDTag)(txtSpreadNode.Tag)).MIDTagData).Key;
                CheckReplaceNode((TextBox)sender, _nodeRID);
            }
        }

		private void txtSpreadNode_Leave(object sender, System.EventArgs e)
		{
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
		
		private void cboSpreadVersion_SelectionChangeCommitted(object sender, System.EventArgs e)
		{
			if (FormLoaded)
			{
				ChangePending = true;

				_parentVersion = Convert.ToInt32(cboSpreadVersion.SelectedValue, CultureInfo.CurrentUICulture);
				// BEGIN Issue 4858 stodd
                this._OTSRollupMethod.VersionRID = _parentVersion;
                // Begin Track #5863 - JSmith - Security issues
                //bool canUpdate = ABM.AuthorizedToUpdate(this.SAB.ClientServerSession, this.SAB.ClientServerSession.UserRID);
                //base.ApplyCanUpdate(canUpdate);
                if (_OTSRollupMethod.HierNodeRID > Include.NoRID)
                {
					// BEGIN Track #5859 stodd
					//bool canUpdate = ABM.AuthorizedToUpdate(this.SAB.ClientServerSession, this.SAB.ClientServerSession.UserRID);
					//base.ApplyCanUpdate(canUpdate);
					// END Track #5859 stodd
                }
                else
                {
                    return;
                }
                // End Track #5863
				// END Issue 4858
			}
			// BEGIN Issue 4858 jsmith
            else if (_OTSRollupMethod.Method_Change_Type == eChangeType.add)
			{
                _OTSRollupMethod.VersionRID = Convert.ToInt32(cboSpreadVersion.SelectedValue, CultureInfo.CurrentUICulture);
			}
			// END Issue 4858
			// BEGIN Track #5859 stodd
			//// Begin Track #5858 - JSmith - Validating store security only
			////base.ValidatePlanVersionSecurity(cboSpreadVersion, true);
			//base.ValidatePlanVersionSecurity(cboSpreadVersion, true, ePlanType.Chain | ePlanType.Store);
			//// End Track #5858
			ApplySecurity();
			// END Track #5859 stodd

		}
        // Begin TT#316-MD - RMatelic - Replace all Windows Combobox controls with new enhanced control 
        void cboSpreadVersion_MIDComboBoxPropertiesChangedEvent(object source, MIDComboBoxPropertiesChangedEventArgs args)
        {
            this.cboSpreadVersion_SelectionChangeCommitted(source, new EventArgs());
        }
        // End TT#316
        private void SetPlanTypeAuthority()
        {
            bool chainCanUpdate = _OTSRollupMethod.ChainAuthorizedToUpdate();
            if (chainCanUpdate)
            {
                grdRollup.DisplayLayout.Bands[0].Columns["Chain"].CellActivation = Activation.AllowEdit;
                grdRollup.DisplayLayout.Bands[0].Columns["StoreToChain"].CellActivation = Activation.AllowEdit;
            }
            else
            {
                grdRollup.DisplayLayout.Bands[0].Columns["Chain"].CellActivation = Activation.Disabled;
                grdRollup.DisplayLayout.Bands[0].Columns["StoreToChain"].CellActivation = Activation.Disabled;
            }

            bool storeCanUpdate = _OTSRollupMethod.StoreAuthorizedToUpdate();
            if (storeCanUpdate)
            {
                grdRollup.DisplayLayout.Bands[0].Columns["Store"].CellActivation = Activation.AllowEdit;
            }
            else
            {
                grdRollup.DisplayLayout.Bands[0].Columns["Store"].CellActivation = Activation.Disabled;
            }
        }

		override protected bool ApplySecurity()	// track 5871 stodd
		{
			bool securityOk = true;	// track 5871
			// BEgin track #5859 stodd
			//if (FunctionSecurity.AllowUpdate)
			//{
			//    btnSave.Enabled = true;
			//}
			//else
			//{
			//    btnSave.Enabled = false;
			//}

			//if (FunctionSecurity.AllowExecute)
			//{
			//    btnProcess.Enabled = true;
			//}
			//else
			//{
			//    btnProcess.Enabled = false;
			//}

			//if (_nodeRID != Include.NoRID)
			//{
			//    _hierNodeSecurity = SAB.ClientServerSession.GetMyUserNodeSecurityAssignment(_nodeRID, (int)eSecurityTypes.Store);
			//    if (!_hierNodeSecurity.AllowUpdate)
			//    {
			//        btnProcess.Enabled = false;
			//    }
			//}

			securityOk = base.ValidatePlanNodeSecurity(txtSpreadNode, true, eSecurityTypes.Chain | eSecurityTypes.Store);
			if (securityOk)
				securityOk = base.ValidatePlanVersionSecurity(cboSpreadVersion.ComboBox, true, ePlanSelectType.Chain | ePlanSelectType.Store);

			// Begin Track #5858 stodd
			//if (_dsRollup != null)
			//{
			//    _dsRollup.AcceptChanges();
			//    _OTSRollupMethod.DSRollup = _dsRollup;
			//    SetPlanTypeAuthority();
			//}
			// End Track #5858

			bool canUpdate = ABM.AuthorizedToUpdate(this.SAB.ClientServerSession, this.SAB.ClientServerSession.UserRID);
			// Begin Track #5858
			canUpdate = ApplyGridSecurity(canUpdate);
			// End track #5858
			base.ApplyCanUpdate(canUpdate);
			// End Track #5859 stodd
			return securityOk;	// track 5871
		}

		// Begin Track #5858
		private bool ApplyGridSecurity(bool canUpdate)
		{
			bool storeOk = false;
			bool chainOk = false;
			string errorMessage = string.Empty;

			if (grdRollup.Rows.Count > 0)
			{
				HierarchyNodeSecurityProfile storeNodeSecurity = SAB.ClientServerSession.GetMyUserNodeSecurityAssignment(_OTSRollupMethod.HierNodeRID, (int)eSecurityTypes.Store);
				VersionSecurityProfile storeVersionSecurity = SAB.ClientServerSession.GetMyVersionSecurityAssignment(_OTSRollupMethod.VersionRID, (int)eSecurityTypes.Store);
				if (storeNodeSecurity.AllowUpdate && storeVersionSecurity.AllowUpdate)
				{
					storeOk = true;
				}
				HierarchyNodeSecurityProfile chainNodeSecurity = SAB.ClientServerSession.GetMyUserNodeSecurityAssignment(_OTSRollupMethod.HierNodeRID, (int)eSecurityTypes.Chain);
				VersionSecurityProfile chainVersionSecurity = SAB.ClientServerSession.GetMyVersionSecurityAssignment(_OTSRollupMethod.VersionRID, (int)eSecurityTypes.Chain);
				if (chainNodeSecurity.AllowUpdate && chainVersionSecurity.AllowUpdate)
				{
					chainOk = true;
				}

				foreach (UltraGridRow gridRow in grdRollup.Rows)
				{
					if (gridRow.Cells["Store"].Value != DBNull.Value && gridRow.Cells["Chain"].Value != DBNull.Value &&
							gridRow.Cells["StoreToChain"].Value != DBNull.Value)	// Check if this is a brand new row
					{
						// BEGIN Track #5852 (point 3) - chain/store/storeToChain checkbox error
						bool storeChecked = Convert.ToBoolean(gridRow.Cells["Store"].Text);
						bool chainChecked = Convert.ToBoolean(gridRow.Cells["Chain"].Text);
						bool storeToChainChecked = Convert.ToBoolean(gridRow.Cells["StoreToChain"].Text);
						// END Track #5852 (point 3) - chain/store/storeToChain checkbox error
                        // Begin Track #6346 - JSmith - Duplicate rows after Rollup
                        //errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_NotAuthorizedToPlan);
                        errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_NotAuthorizedToPlan, false);
                        // End Track #6346

						if (!storeOk && storeChecked)
						{
							gridRow.Cells["Store"].Appearance.Image = ErrorImage;
							gridRow.Cells["Store"].Tag = errorMessage;

							canUpdate = false;
						}
						else if (gridRow.Cells["Store"].Tag != null)
						{
							if (gridRow.Cells["Store"].Tag.ToString() == errorMessage)
							{
								gridRow.Cells["Store"].Appearance.Image = null;
								gridRow.Cells["Store"].Tag = string.Empty;
							}
						}
						if (!chainOk && chainChecked)
						{
							gridRow.Cells["Chain"].Appearance.Image = ErrorImage;
							gridRow.Cells["Chain"].Tag = errorMessage;

							canUpdate = false;
						}
						else if (gridRow.Cells["Chain"].Tag != null)
						{
							if (gridRow.Cells["Chain"].Tag.ToString() == errorMessage)
							{
								gridRow.Cells["Chain"].Appearance.Image = null;
								gridRow.Cells["Chain"].Tag = string.Empty;
							}
						}
						if (!chainOk && storeToChainChecked)
						{
							gridRow.Cells["StoreToChain"].Appearance.Image = ErrorImage;
							gridRow.Cells["StoreToChain"].Tag = errorMessage;

							canUpdate = false;
						}
						else if (gridRow.Cells["StoreToChain"].Tag != null)
						{
							if (gridRow.Cells["StoreToChain"].Tag.ToString() == errorMessage)
							{
								gridRow.Cells["StoreToChain"].Appearance.Image = null;
								gridRow.Cells["StoreToChain"].Tag = string.Empty;
							}
						}
					}
				}
			}
			return canUpdate;
		}
		// End Track #5858
		#endregion


        #region MIDDateRangeSelector Events

		private void mdsPlanDateRange_Click(object sender, System.EventArgs e)
		{
			try
			{
				CalendarDateSelector frm = (CalendarDateSelector)CreateControl(typeof(CalendarDateSelector), new object[]{SAB});
				mdsPlanDateRange.DateRangeForm = frm;
				frm.DateRangeRID = mdsPlanDateRange.DateRangeRID;
				frm.AllowDynamicToPlan = false;
				frm.AllowDynamicToStoreOpen = false;
				frm.AllowDynamicSwitch = true;
				mdsPlanDateRange.ShowSelector();
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
					LoadDateRangeSelector(mdsPlanDateRange, e.SelectedDateRange);
                    ErrorProvider.SetError(mdsPlanDateRange, string.Empty);
					ChangePending = true;
 				}
			}
			catch (Exception exc)
			{
				HandleException(exc);
			}
		}
		
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
				else
				{
					aMIDDRS.SetImage(null);
				}
				//=========================================================
				// Override the image if this is a dynamic switch date.
				//=========================================================
				if (aDateRangeProf.IsDynamicSwitch)
					aMIDDRS.SetImage(this.DynamicSwitchImage);

			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
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
                GetOTSPLANWorkflows(_OTSRollupMethod.Key, ugWorkflows);
            }
            catch (Exception ex)
            {
                HandleException(ex, "LoadWorkflows");
            }
		}
		#endregion

        private void txtSpreadNode_DragOver(object sender, DragEventArgs e)
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

	
	}
}

