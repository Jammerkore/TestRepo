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
	public class frmForecastSpreadMethod : MIDRetail.Windows.WorkflowMethodFormBase
	{
		#region Fields

		private Bitmap _picInclude;
		private Bitmap _picExclude;
		private Image _dynamicToPlanImage;
		private Image _dynamicToCurrentImage;
		//private ProfileList _versionProfList;
		private MIDRetail.Business.OTSForecastSpreadMethod _OTSForecastSpreadMethod = null;
		private HierarchyNodeSecurityProfile _hierNodeSecurity;
		private System.Data.DataSet _dsForecastSpread;
        //private System.Data.DataTable _dtForecastVersions;
		private System.Data.DataTable _dtBasis;
        //private System.Data.DataTable _dtGroupLevel;
        private FunctionSecurityProfile _filterUserSecurity;
        private FunctionSecurityProfile _filterGlobalSecurity;
		private ArrayList _userRIDList;
		//private StoreFilterData _storeFilterDL;
		private ProfileList _variables;
		private int _nodeRID = Include.NoRID;
        //private int _prevAttributeValue;
        //private int _prevSetValue;
        //private ePlanType _planType;
//		private eMethodType _methodType;
        //private bool _attributeReset = false;
        //private bool _attributeChanged = false;
        //private bool _setReset = false;
		private bool _skipAfterCellUpdate = false;
		private bool _basisNodeRequired = false;
		private string _thisTitle;
        //private bool _needsValidated = false;
		private int _highLevelWeekCount = 0;		// MID Track #5149 - error when basis weeks > high level weeks

		#endregion

		private System.Windows.Forms.TabPage tabMethod;
		private System.Windows.Forms.Label lblTimePeriod;
        private System.Windows.Forms.TabPage tabProperties;
		private System.Windows.Forms.TabControl tabSpreadMethod;
		private System.Windows.Forms.GroupBox grpSpread;
		private System.Windows.Forms.Label lblVersion;
		private System.Windows.Forms.Label lblMerchandise;
        private System.Windows.Forms.TextBox txtSpreadNode;
		private MIDRetail.Windows.Controls.MIDDateRangeSelector mdsPlanDateRange;
		private System.Windows.Forms.GroupBox grpBasis;
		private Infragistics.Win.UltraWinGrid.UltraGrid grdBasis;
		private System.Windows.Forms.ContextMenu mnuBasisGrid;
		private System.Windows.Forms.ToolTip toolTip1;
		private System.Windows.Forms.ImageList Icons;
		private System.Windows.Forms.GroupBox grpOptions;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.RadioButton rbBasis;
		private System.Windows.Forms.RadioButton rbPlan;
		private System.Windows.Forms.CheckBox cbLocks;
        private HierarchyNodeList _hierChildList;
        private DataTable _dtLowerLevels;
		private int _parentVersion;
		private System.Windows.Forms.Label lblEqualizeWgt;
		private System.Windows.Forms.RadioButton rbEWNo;
		private System.Windows.Forms.RadioButton rbEWYes;
        private Button btnOverride;
        private Label lblFromLevel;
        private Label lblToLevel;
        private CheckBox cbMultiLevel;
        private GroupBox grpLastProcessed;
        private Label lblLastProcessed;
        private Label lblByUserValue;
        private Label lblByUser;
        private Label lblDateTimeValue;
        private Label lblDateTime;
        private UltraGrid ugWorkflows;

//BEGIN TT#7 - RBeck - Dynamic dropdowns 
        private MIDComboBoxEnh cboSpreadVersion;
        private MIDComboBoxEnh cboFromLevel;
        private MIDComboBoxEnh cboToLevel;
        private MIDComboBoxEnh cboOverride;
//END   TT#7 - RBeck - Dynamic dropdowns

		private System.ComponentModel.IContainer components = null;

		public frmForecastSpreadMethod(SessionAddressBlock SAB, ExplorerAddressBlock aEAB) : base (SAB, aEAB, eMIDTextCode.frm_ForecastChainSpread, eWorkflowMethodType.Method)
		{
			try
			{
				InitializeComponent();

                UserSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ForecastMethodsUserOTSSpread);
                GlobalSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ForecastMethodsGlobalOTSSpread);
			
			}
			catch(Exception ex)
			{
				HandleException(ex, "NewOTSForecastSpreadMethod");
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
                this.cboFromLevel.SelectionChangeCommitted -= new System.EventHandler(this.cboFromLevel_SelectionChangeCommitted);
                this.cboToLevel.SelectionChangeCommitted -= new System.EventHandler(this.cboToLevel_SelectionChangeCommitted);
                //Begin TT#316 -jsobek -Replace all Windows Combobox controls with new enhanced control
                this.cboOverride.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler(cboOverride_MIDComboBoxPropertiesChangedEvent);
                this.cboToLevel.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler(cboToLevel_MIDComboBoxPropertiesChangedEvent);
                this.cboFromLevel.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler(cboFromLevel_MIDComboBoxPropertiesChangedEvent);
                this.cboSpreadVersion.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler(cboSpreadVersion_MIDComboBoxPropertiesChangedEvent);
                //End TT#316
                this.grdBasis.InitializeRow -= new Infragistics.Win.UltraWinGrid.InitializeRowEventHandler(this.grdBasis_InitializeRow);
				this.grdBasis.MouseEnterElement -= new Infragistics.Win.UIElementEventHandler(this.grdBasis_MouseEnterElement);
				this.grdBasis.DragDrop -= new System.Windows.Forms.DragEventHandler(this.grdBasis_DragDrop);
				this.grdBasis.AfterRowInsert -= new Infragistics.Win.UltraWinGrid.RowEventHandler(this.grdBasis_AfterRowInsert);
				this.grdBasis.DragOver -= new System.Windows.Forms.DragEventHandler(this.grdBasis_DragOver);
				this.grdBasis.ClickCellButton -= new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdBasis_ClickCellButton);
				this.grdBasis.AfterCellUpdate -= new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdBasis_AfterCellUpdate);
				this.grdBasis.InitializeLayout -= new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.grdBasis_InitializeLayout);
                //Begin TT#169 - JSmith - enter max of 1000 and receive 'must be integer'
                MIDRetail.Windows.Controls.UltraGridLayoutDefaults ugld = new MIDRetail.Windows.Controls.UltraGridLayoutDefaults(ErrorImage);
                ugld.DetachGridEventHandlers(grdBasis);
                //End TT#169
				this.mdsPlanDateRange.Click -= new System.EventHandler(this.mdsPlanDateRange_Click);
				this.mdsPlanDateRange.OnSelection -= new MIDRetail.Windows.Controls.MIDDateRangeSelector.SelectionEventHandler(this.mdsPlanDateRange_OnSelection);
                this.cbMultiLevel.CheckedChanged -= new System.EventHandler(this.cbMultiLevel_CheckedChanged);
                this.btnOverride.Click -= new System.EventHandler(this.btnOverride_Click);
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
            this.grpBasis = new System.Windows.Forms.GroupBox();
            this.rbEWNo = new System.Windows.Forms.RadioButton();
            this.rbEWYes = new System.Windows.Forms.RadioButton();
            this.lblEqualizeWgt = new System.Windows.Forms.Label();
            this.grdBasis = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.mnuBasisGrid = new System.Windows.Forms.ContextMenu();
            this.grpOptions = new System.Windows.Forms.GroupBox();
            this.cbLocks = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.rbPlan = new System.Windows.Forms.RadioButton();
            this.rbBasis = new System.Windows.Forms.RadioButton();
            this.grpSpread = new System.Windows.Forms.GroupBox();
            this.cboOverride = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this.cboToLevel = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this.cboFromLevel = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this.cboSpreadVersion = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this.cbMultiLevel = new System.Windows.Forms.CheckBox();
            this.btnOverride = new System.Windows.Forms.Button();
            this.mdsPlanDateRange = new MIDRetail.Windows.Controls.MIDDateRangeSelector();
            this.lblFromLevel = new System.Windows.Forms.Label();
            this.lblToLevel = new System.Windows.Forms.Label();
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
            this.grpBasis.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdBasis)).BeginInit();
            this.grpOptions.SuspendLayout();
            this.grpSpread.SuspendLayout();
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
            this.tabMethod.Controls.Add(this.grpBasis);
            this.tabMethod.Controls.Add(this.grpOptions);
            this.tabMethod.Controls.Add(this.grpSpread);
            this.tabMethod.Location = new System.Drawing.Point(4, 22);
            this.tabMethod.Name = "tabMethod";
            this.tabMethod.Size = new System.Drawing.Size(680, 448);
            this.tabMethod.TabIndex = 1;
            this.tabMethod.Text = "Method";
            // 
            // grpBasis
            // 
            this.grpBasis.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpBasis.Controls.Add(this.rbEWNo);
            this.grpBasis.Controls.Add(this.rbEWYes);
            this.grpBasis.Controls.Add(this.lblEqualizeWgt);
            this.grpBasis.Controls.Add(this.grdBasis);
            this.grpBasis.Location = new System.Drawing.Point(8, 201);
            this.grpBasis.Name = "grpBasis";
            this.grpBasis.Size = new System.Drawing.Size(664, 240);
            this.grpBasis.TabIndex = 11;
            this.grpBasis.TabStop = false;
            this.grpBasis.Text = "Basis";
            // 
            // rbEWNo
            // 
            this.rbEWNo.Location = new System.Drawing.Point(540, 12);
            this.rbEWNo.Name = "rbEWNo";
            this.rbEWNo.Size = new System.Drawing.Size(48, 16);
            this.rbEWNo.TabIndex = 3;
            this.rbEWNo.Text = "No";
            // 
            // rbEWYes
            // 
            this.rbEWYes.Checked = true;
            this.rbEWYes.Location = new System.Drawing.Point(490, 12);
            this.rbEWYes.Name = "rbEWYes";
            this.rbEWYes.Size = new System.Drawing.Size(48, 16);
            this.rbEWYes.TabIndex = 2;
            this.rbEWYes.TabStop = true;
            this.rbEWYes.Text = "Yes";
            // 
            // lblEqualizeWgt
            // 
            this.lblEqualizeWgt.Location = new System.Drawing.Point(376, 12);
            this.lblEqualizeWgt.Name = "lblEqualizeWgt";
            this.lblEqualizeWgt.Size = new System.Drawing.Size(104, 16);
            this.lblEqualizeWgt.TabIndex = 1;
            this.lblEqualizeWgt.Text = "Equalize Weighting:";
            // 
            // grdBasis
            // 
            this.grdBasis.AllowDrop = true;
            this.grdBasis.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grdBasis.ContextMenu = this.mnuBasisGrid;
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
            this.grdBasis.Location = new System.Drawing.Point(16, 34);
            this.grdBasis.Name = "grdBasis";
            this.grdBasis.Size = new System.Drawing.Size(632, 195);
            this.grdBasis.TabIndex = 0;
            this.grdBasis.AfterCellUpdate += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdBasis_AfterCellUpdate);
            this.grdBasis.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.grdBasis_InitializeLayout);
            this.grdBasis.InitializeRow += new Infragistics.Win.UltraWinGrid.InitializeRowEventHandler(this.grdBasis_InitializeRow);
            this.grdBasis.AfterRowInsert += new Infragistics.Win.UltraWinGrid.RowEventHandler(this.grdBasis_AfterRowInsert);
            this.grdBasis.CellChange += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdBasis_CellChange);
            this.grdBasis.ClickCellButton += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdBasis_ClickCellButton);
            this.grdBasis.MouseEnterElement += new Infragistics.Win.UIElementEventHandler(this.grdBasis_MouseEnterElement);
            this.grdBasis.DragDrop += new System.Windows.Forms.DragEventHandler(this.grdBasis_DragDrop);
            this.grdBasis.DragOver += new System.Windows.Forms.DragEventHandler(this.grdBasis_DragOver);
            // 
            // grpOptions
            // 
            this.grpOptions.Controls.Add(this.cbLocks);
            this.grpOptions.Controls.Add(this.label1);
            this.grpOptions.Controls.Add(this.rbPlan);
            this.grpOptions.Controls.Add(this.rbBasis);
            this.grpOptions.Location = new System.Drawing.Point(8, 145);
            this.grpOptions.Name = "grpOptions";
            this.grpOptions.Size = new System.Drawing.Size(664, 50);
            this.grpOptions.TabIndex = 10;
            this.grpOptions.TabStop = false;
            this.grpOptions.Text = "Options";
            // 
            // cbLocks
            // 
            this.cbLocks.Location = new System.Drawing.Point(356, 16);
            this.cbLocks.Name = "cbLocks";
            this.cbLocks.Size = new System.Drawing.Size(104, 24);
            this.cbLocks.TabIndex = 23;
            this.cbLocks.Text = "Ignore Locks";
            this.cbLocks.CheckedChanged += new System.EventHandler(this.cbLocks_CheckedChanged);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(23, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 16);
            this.label1.TabIndex = 20;
            this.label1.Text = "Spread Option:";
            // 
            // rbPlan
            // 
            this.rbPlan.Location = new System.Drawing.Point(191, 21);
            this.rbPlan.Name = "rbPlan";
            this.rbPlan.Size = new System.Drawing.Size(48, 16);
            this.rbPlan.TabIndex = 22;
            this.rbPlan.Text = "Plan";
            this.rbPlan.CheckedChanged += new System.EventHandler(this.rbPlan_CheckedChanged);
            // 
            // rbBasis
            // 
            this.rbBasis.Location = new System.Drawing.Point(119, 21);
            this.rbBasis.Name = "rbBasis";
            this.rbBasis.Size = new System.Drawing.Size(56, 16);
            this.rbBasis.TabIndex = 21;
            this.rbBasis.Text = "Basis";
            this.rbBasis.CheckedChanged += new System.EventHandler(this.rbBasis_CheckedChanged);
            // 
            // grpSpread
            // 
            this.grpSpread.Controls.Add(this.cboOverride);
            this.grpSpread.Controls.Add(this.cboToLevel);
            this.grpSpread.Controls.Add(this.cboFromLevel);
            this.grpSpread.Controls.Add(this.cboSpreadVersion);
            this.grpSpread.Controls.Add(this.cbMultiLevel);
            this.grpSpread.Controls.Add(this.btnOverride);
            this.grpSpread.Controls.Add(this.mdsPlanDateRange);
            this.grpSpread.Controls.Add(this.lblFromLevel);
            this.grpSpread.Controls.Add(this.lblToLevel);
            this.grpSpread.Controls.Add(this.lblMerchandise);
            this.grpSpread.Controls.Add(this.txtSpreadNode);
            this.grpSpread.Controls.Add(this.lblVersion);
            this.grpSpread.Controls.Add(this.lblTimePeriod);
            this.grpSpread.Location = new System.Drawing.Point(8, 8);
            this.grpSpread.Name = "grpSpread";
            this.grpSpread.Size = new System.Drawing.Size(664, 131);
            this.grpSpread.TabIndex = 4;
            this.grpSpread.TabStop = false;
            this.grpSpread.Text = "Spread Parent";
            // 
            // cboOverride
            // 
            this.cboOverride.AutoAdjust = true;
            this.cboOverride.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboOverride.AutoScroll = true;
            this.cboOverride.DataSource = null;
            this.cboOverride.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboOverride.DropDownWidth = 175;
            this.cboOverride.Location = new System.Drawing.Point(475, 95);
            this.cboOverride.Margin = new System.Windows.Forms.Padding(0);
            this.cboOverride.Name = "cboOverride";
            this.cboOverride.Size = new System.Drawing.Size(175, 21);
            this.cboOverride.TabIndex = 33;
            this.cboOverride.Tag = null;
            this.cboOverride.SelectionChangeCommitted += new System.EventHandler(this.cboOverride_SelectionChangeCommitted);
            this.cboOverride.MIDComboBoxPropertiesChangedEvent += new MIDComboBoxPropertiesChangedEventHandler(cboOverride_MIDComboBoxPropertiesChangedEvent);
            // 
            // cboToLevel
            // 
            this.cboToLevel.AutoAdjust = true;
            this.cboToLevel.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboToLevel.AutoScroll = true;
            this.cboToLevel.DataSource = null;
            this.cboToLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboToLevel.DropDownWidth = 223;
            this.cboToLevel.Location = new System.Drawing.Point(427, 68);
            this.cboToLevel.Margin = new System.Windows.Forms.Padding(0);
            this.cboToLevel.Name = "cboToLevel";
            this.cboToLevel.Size = new System.Drawing.Size(223, 21);
            this.cboToLevel.TabIndex = 30;
            this.cboToLevel.Tag = null;
            this.cboToLevel.SelectionChangeCommitted += new System.EventHandler(this.cboToLevel_SelectionChangeCommitted);
            // 
            // cboFromLevel
            // 
            this.cboFromLevel.AutoAdjust = true;
            this.cboFromLevel.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboFromLevel.AutoScroll = true;
            this.cboFromLevel.DataSource = null;
            this.cboFromLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboFromLevel.DropDownWidth = 223;
            this.cboFromLevel.Location = new System.Drawing.Point(427, 29);
            this.cboFromLevel.Margin = new System.Windows.Forms.Padding(0);
            this.cboFromLevel.Name = "cboFromLevel";
            this.cboFromLevel.Size = new System.Drawing.Size(223, 21);
            this.cboFromLevel.TabIndex = 28;
            this.cboFromLevel.Tag = null;
            this.cboFromLevel.SelectionChangeCommitted += new System.EventHandler(this.cboFromLevel_SelectionChangeCommitted);
            // 
            // cboSpreadVersion
            // 
            this.cboSpreadVersion.AutoAdjust = true;
            this.cboSpreadVersion.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboSpreadVersion.AutoScroll = true;
            this.cboSpreadVersion.DataSource = null;
            this.cboSpreadVersion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSpreadVersion.DropDownWidth = 224;
            this.cboSpreadVersion.Location = new System.Drawing.Point(97, 62);
            this.cboSpreadVersion.Margin = new System.Windows.Forms.Padding(0);
            this.cboSpreadVersion.Name = "cboSpreadVersion";
            this.cboSpreadVersion.Size = new System.Drawing.Size(224, 21);
            this.cboSpreadVersion.TabIndex = 7;
            this.cboSpreadVersion.Tag = null;
            this.cboSpreadVersion.SelectionChangeCommitted += new System.EventHandler(this.cboSpreadVersion_SelectionChangeCommitted);
            // 
            // cbMultiLevel
            // 
            this.cbMultiLevel.Location = new System.Drawing.Point(350, 13);
            this.cbMultiLevel.Name = "cbMultiLevel";
            this.cbMultiLevel.Size = new System.Drawing.Size(95, 15);
            this.cbMultiLevel.TabIndex = 30;
            this.cbMultiLevel.Text = "Multi Level";
            this.cbMultiLevel.CheckedChanged += new System.EventHandler(this.cbMultiLevel_CheckedChanged);
            // 
            // btnOverride
            // 
            this.btnOverride.Location = new System.Drawing.Point(352, 95);
            this.btnOverride.Name = "btnOverride";
            this.btnOverride.Size = new System.Drawing.Size(117, 23);
            this.btnOverride.TabIndex = 32;
            this.btnOverride.Text = "Override";
            this.btnOverride.Click += new System.EventHandler(this.btnOverride_Click);
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
            // lblFromLevel
            // 
            this.lblFromLevel.Location = new System.Drawing.Point(356, 37);
            this.lblFromLevel.Name = "lblFromLevel";
            this.lblFromLevel.Size = new System.Drawing.Size(65, 17);
            this.lblFromLevel.TabIndex = 29;
            this.lblFromLevel.Text = "From Level:";
            // 
            // lblToLevel
            // 
            this.lblToLevel.Location = new System.Drawing.Point(356, 68);
            this.lblToLevel.Name = "lblToLevel";
            this.lblToLevel.Size = new System.Drawing.Size(65, 18);
            this.lblToLevel.TabIndex = 26;
            this.lblToLevel.Text = "To Level:";
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
            this.ugWorkflows.TabIndex = 16;
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
            this.grpLastProcessed.TabIndex = 15;
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
            // frmForecastSpreadMethod
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(718, 572);
            this.Controls.Add(this.tabSpreadMethod);
            this.Name = "frmForecastSpreadMethod";
            this.Text = "Multi Level Spread";
            this.Controls.SetChildIndex(this.tabSpreadMethod, 0);
            this.Controls.SetChildIndex(this.pnlGlobalUser, 0);
            this.Controls.SetChildIndex(this.lblName, 0);
            this.Controls.SetChildIndex(this.txtName, 0);
            this.Controls.SetChildIndex(this.txtDesc, 0);
            this.Controls.SetChildIndex(this.btnSave, 0);
            this.Controls.SetChildIndex(this.btnClose, 0);
            this.Controls.SetChildIndex(this.btnProcess, 0);
            this.pnlGlobalUser.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.utmMain)).EndInit();
            this.tabSpreadMethod.ResumeLayout(false);
            this.tabMethod.ResumeLayout(false);
            this.grpBasis.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdBasis)).EndInit();
            this.grpOptions.ResumeLayout(false);
            this.grpSpread.ResumeLayout(false);
            this.grpSpread.PerformLayout();
            this.tabProperties.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ugWorkflows)).EndInit();
            this.grpLastProcessed.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		/// <summary>
		/// Create a new Multi Level Spread Method
		/// </summary>
		override public void NewWorkflowMethod(MIDWorkflowMethodTreeNode aParentNode)
		{
			try
			{
				_OTSForecastSpreadMethod = new OTSForecastSpreadMethod(SAB, Include.NoRID);
				ABM = _OTSForecastSpreadMethod;
				base.NewWorkflowMethod(aParentNode, eSecurityFunctions.ForecastMethodsUserOTSPlan, eSecurityFunctions.ForecastMethodsGlobalOTSPlan);

				_parentVersion = Include.NoRID;  // Issue 3801 - stodd
				
				Common_Load();

			}
			catch(Exception ex)
			{
				HandleException(ex, "NewOTSForecastSpreadMethod");
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
				_OTSForecastSpreadMethod = new OTSForecastSpreadMethod(SAB, aMethodRID);
				base.UpdateWorkflowMethod(aLockStatus, eSecurityFunctions.ForecastMethodsUserOTSPlan, eSecurityFunctions.ForecastMethodsGlobalOTSPlan);

				_parentVersion = _OTSForecastSpreadMethod.VersionRID;  // Issue 4008 - stodd

				Common_Load();
			}
			catch(Exception ex)
			{
				HandleException(ex, "InitializeOTSForecastSpreadMethod");
				FormLoadError = true;
			}
		}

		/// <summary>
        /// Deletes a Multi Level Spread Method.
		/// </summary>
		/// <param name="aMethodRID">The record ID of the method</param>
		override public bool DeleteWorkflowMethod(int aMethodRID)
		{
			try
			{
				_OTSForecastSpreadMethod = new OTSForecastSpreadMethod(SAB, aMethodRID);
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
        /// Renames an Multi Level Spread Method.
		/// </summary>
		/// <param name="aMethodRID">The record ID of the method</param>
		/// <param name="aNewName">The new name of the workflow or method</param>
		override public bool RenameWorkflowMethod(int aMethodRID, string aNewName)
		{
			try
			{
				_OTSForecastSpreadMethod = new OTSForecastSpreadMethod(SAB, aMethodRID);
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
				this.grpSpread.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_SpreadFrom);
                this.cbMultiLevel.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_cbMultiLevel);
                this.lblFromLevel.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_FromLevel) + ":";
                this.lblToLevel.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_ToLevel) + ":";
				this.btnOverride.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_OverrideLowVersion);
				this.lblMerchandise.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Merchandise) + ":";
				this.lblVersion.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Version) + ":";
				this.lblTimePeriod.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_PlanTimePeriod) + ":";	
				this.grpBasis.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Basis); 
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
				//_versionProfList = SAB.ClientServerSession.GetUserForecastVersions();  // Issue 4858
				_variables = SAB.ApplicationServerSession.DefaultPlanComputations.PlanVariables.VariableProfileList;

				SetText();

				eMIDTextCode textCode =  eMIDTextCode.frm_ForecastChainSpread;
	
				_thisTitle = MIDText.GetTextOnly((int)textCode);
				if (_OTSForecastSpreadMethod.Method_Change_Type == eChangeType.add)
				{
 					Format_Title(eDataState.New, textCode, null);
				}
				else
				{
					if (FunctionSecurity.AllowUpdate)
					{
						Format_Title(eDataState.Updatable,textCode, _OTSForecastSpreadMethod.Name);
					}
					else
					{
						Format_Title(eDataState.ReadOnly, textCode, _OTSForecastSpreadMethod.Name);
					}

                    lblDateTimeValue.Text = _OTSForecastSpreadMethod.LastProcessedDateTime;
                    lblByUserValue.Text = _OTSForecastSpreadMethod.LastProcessedUser;

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

				if (this._OTSForecastSpreadMethod.OverrideLowLevelRid != Include.NoRID)
				{
					ModelsData modelData = new ModelsData();
                    cboOverride.SelectedValue = _OTSForecastSpreadMethod.OverrideLowLevelRid;
				}

				BindVersionComboBoxes();				
			
				CreateBasisComboLists();

                LoadOverrideModelComboBox(this.cboOverride.ComboBox, _OTSForecastSpreadMethod.OverrideLowLevelRid, _OTSForecastSpreadMethod.CustomOLL_RID);//TT#7 - RBeck - Dynamic dropdowns
			
				LoadMethodData();

				// Begin issue 3716 - stodd 02/15/06
				LoadWorkflows();
				// End issue 3716

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

        private void cboFromLevel_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (FormLoaded)
            {
                ChangePending = true;

                if (txtSpreadNode.Tag != null)
                {
                    HierarchyNodeProfile hnp = SAB.HierarchyServerSession.GetNodeData(_OTSForecastSpreadMethod.HierNodeRID);
                    PopulateFromToLevels(hnp, cboToLevel.ComboBox, cboFromLevel.SelectedIndex);//TT#7 - RBeck - Dynamic dropdowns
					ApplySecurity();
                }
            }
        }

        private void cboToLevel_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (FormLoaded)
            {
                ChangePending = true;

                if (txtSpreadNode.Tag != null)
                {
                    //--->Put Code Here<----
					_OTSForecastSpreadMethod.ToLevelType = ((ToLevelCombo)cboToLevel.SelectedItem).ToLevelType;
					_OTSForecastSpreadMethod.ToLevelOffset = ((ToLevelCombo)cboToLevel.SelectedItem).ToLevelOffset;
					_OTSForecastSpreadMethod.ToLevelSequence = ((ToLevelCombo)cboToLevel.SelectedItem).ToLevelSequence;
					ApplySecurity();
                }
            }
        }
        //Begin TT#316 -jsobek -Replace all Windows Combobox controls with new enhanced control
        void cboOverride_MIDComboBoxPropertiesChangedEvent(object source, MIDComboBoxPropertiesChangedEventArgs args)
        {
            this.cboOverride_SelectionChangeCommitted(source, new EventArgs());
        }
        void cboToLevel_MIDComboBoxPropertiesChangedEvent(object source, MIDComboBoxPropertiesChangedEventArgs args)
        {
            this.cboToLevel_SelectionChangeCommitted(source, new EventArgs());
        }
        void cboFromLevel_MIDComboBoxPropertiesChangedEvent(object source, MIDComboBoxPropertiesChangedEventArgs args)
        {
            this.cboFromLevel_SelectionChangeCommitted(source, new EventArgs());
        }
        void cboSpreadVersion_MIDComboBoxPropertiesChangedEvent(object source, MIDComboBoxPropertiesChangedEventArgs args)
        {
            this.cboSpreadVersion_SelectionChangeCommitted(source, new EventArgs());
        }
        //End TT#316

		private void BindVersionComboBoxes()
		{
			try
			{
				// BEGIN Issue 4858 stodd 11.7.2007 method security
//				_dtMultiLevelVersions = new DataTable("Versions");
//				_dtMultiLevelVersions.Columns.Add("Description", typeof(string));
//				_dtMultiLevelVersions.Columns.Add("Key", typeof(int));
//
//				_dtMultiLevelVersions.Rows.Add(new object[] {string.Empty, Include.NoRID});
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
//						_dtMultiLevelVersions.Rows.Add(new object[] {verProf.Description, verProf.Key});
//					}
//					// End Issue 4562
//				}
//				
				ProfileList versionProfList = null;
				if (_OTSForecastSpreadMethod.Method_Change_Type == eChangeType.add)
				{
					versionProfList = base.GetForecastVersionList(eSecuritySelectType.Update, eSecurityTypes.Chain, true, this._OTSForecastSpreadMethod.VersionRID, true);	// Track #5871
				}
				else
				{
					versionProfList = base.GetForecastVersionList(eSecuritySelectType.Update, eSecurityTypes.Chain, false, this._OTSForecastSpreadMethod.VersionRID, true);	// Track #5871
				}

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
				_OTSForecastSpreadMethod = new OTSForecastSpreadMethod(SAB, aMethodRID);
				ProcessAction(eMethodType.ForecastSpread, true);
                // Begin TT#2281 - JSmith - Error when updating Low Level in Matrix Balance OTS method
                if (_OTSForecastSpreadMethod.FoundDuplicate)
                {
                    MessageBox.Show(MIDText.GetText(eMIDTextCode.msg_DuplicateDescendantInMethodError, _OTSForecastSpreadMethod.DuplicateMessage),
                    _OTSForecastSpreadMethod.Name, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                // End TT#2281
			}
			catch (Exception ex)
			{
				HandleException(ex);
				FormLoadError = true;
			}
		}
		// End Issue 4323 - stodd 2.21.07 - fix process from workflow explorer

        //private void SaveBasisForSetValues(int aSetValue)
        //{
        //    DataRow setRow = null;
        //    foreach (DataRow row in _dtGroupLevel.Rows)
        //    {
        //        if ((int)row["SGL_RID"] == aSetValue)
        //        {
        //            setRow = row;
        //            break;
        //        }				
        //    }
        //    if (setRow == null)
        //    {
        //        if (_dtBasis.DefaultView.Count > 0)
        //        {
        //            setRow = _dtGroupLevel.NewRow();
        //            _dtGroupLevel.Rows.Add(setRow);
        //            setRow["SGL_RID"] = aSetValue;
        //            _dtGroupLevel.AcceptChanges();
        //        }
        //    }
        //    else if (_dtBasis.DefaultView.Count == 0)
        //    {
        //        _dtGroupLevel.Rows.Remove(setRow);
        //        _dtGroupLevel.AcceptChanges();
        //    }
        //}

		private void LoadBasisForSetValues(int aSetValue)
		{
		
			_dtBasis.DefaultView.RowFilter = "SGL_RID = " +  aSetValue.ToString();
			foreach (UltraGridRow row in grdBasis.Rows)
			{
				DateRangeProfile drp = SAB.ClientServerSession.Calendar.GetDateRange(Convert.ToInt32(row.Cells["CDR_RID"].Value, CultureInfo.CurrentUICulture));
				row.Cells["DateRange"].Value = drp.DisplayDate;
				if (drp.DateRangeType == eCalendarRangeType.Dynamic)
				{
					switch (drp.RelativeTo)
					{
						case eDateRangeRelativeTo.Current:
							row.Cells["DateRange"].Appearance.Image = this.DynamicToCurrentImage;
							break;
						case eDateRangeRelativeTo.Plan:
							row.Cells["DateRange"].Appearance.Image = this.DynamicToPlanImage;
							break;
						default:
							row.Cells["DateRange"].Appearance.Image = null;
							break;
					}
				}
			}
		}	
		/// <summary>
		/// Creates a list for use on the "Version" column, which is a dropdown.
		/// </summary>
		
		private void CreateBasisComboLists()
		{
			int i;
			Infragistics.Win.ValueList vl;
			//Infragistics.Win.ValueList vl2; <---DKJ
			Infragistics.Win.ValueListItem vli;

			try
			{
				// BEGIN Issue 4858
				vl = grdBasis.DisplayLayout.ValueLists.Add("Version");
				ProfileList versionProfList = base.GetForecastVersionList(eSecuritySelectType.View | eSecuritySelectType.Update, eSecurityTypes.Chain, true, Include.NoRID);
				for (i = 0; i < versionProfList.Count; i++)
				{
					vli = new Infragistics.Win.ValueListItem();
					vli.DataValue= versionProfList[i].Key;
					vli.DisplayText = ((VersionProfile)versionProfList[i]).Description;
					vl.ValueListItems.Add(vli);
				}
				// END Issue 4858
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
				// Inititalize Fields
				_dsForecastSpread = _OTSForecastSpreadMethod.DSForecastSpread;

				mdsPlanDateRange.DateRangeRID = Include.UndefinedCalendarDateRange;
				mdsPlanDateRange.SetImage(null);

                //Begin Track #5858 - KJohnson - Validating store security only
                txtSpreadNode.Tag = new MIDMerchandiseTextBoxTag(SAB, txtSpreadNode, eMIDControlCode.form_ForecastChainSpread, eSecurityTypes.Store, eSecuritySelectType.View | eSecuritySelectType.Update);
                //End Track #5858
				if (_OTSForecastSpreadMethod.Method_Change_Type != eChangeType.add)
				{

					this.txtName.Text = _OTSForecastSpreadMethod.Name;
                    this.txtDesc.Text = _OTSForecastSpreadMethod.Method_Description;

					if (_OTSForecastSpreadMethod.User_RID == Include.GetGlobalUserRID())
						radGlobal.Checked = true;
					else
						radUser.Checked = true;
					LoadWorkflows();
				}

                if (_OTSForecastSpreadMethod.MultiLevel)
                {
                    this.cbMultiLevel.Checked = true;
                    this.cboFromLevel.Enabled = true;
                }
                else
                {
                    this.cbMultiLevel.Checked = false;
                    this.cboFromLevel.Enabled = false;
                }

				if (_OTSForecastSpreadMethod.HierNodeRID > 0)
				{
                    HierarchyNodeProfile hnp = SAB.HierarchyServerSession.GetNodeData(_OTSForecastSpreadMethod.HierNodeRID);
                    //Begin Track #5858 - KJohnson - Validating store security only
                    txtSpreadNode.Text = hnp.Text;
                    ((MIDTag)(txtSpreadNode.Tag)).MIDTagData = hnp;
                    //End Track #5858
                    // Begin TT#56 - JSmith - not holding to level after save or update
                    //PopulateFromToLevels(hnp, cboFromLevel, 0);
                    //PopulateFromToLevels(hnp, cboToLevel, 0);
                    PopulateFromToLevels(hnp, cboFromLevel.ComboBox, 0, false);//TT#7 - RBeck - Dynamic dropdowns
                    PopulateFromToLevels(hnp, cboToLevel.ComboBox, 0, false);//TT#7 - RBeck - Dynamic dropdowns
                    // End TT#56
                    cboFromLevel.SelectedIndex = cboFromLevel.Items.IndexOf(new FromLevelCombo(_OTSForecastSpreadMethod.FromLevelType, _OTSForecastSpreadMethod.FromLevelOffset, _OTSForecastSpreadMethod.FromLevelSequence, ""));
                    cboToLevel.SelectedIndex = cboToLevel.Items.IndexOf(new ToLevelCombo(_OTSForecastSpreadMethod.ToLevelType, _OTSForecastSpreadMethod.ToLevelOffset, _OTSForecastSpreadMethod.ToLevelSequence, ""));
                    if (cboFromLevel.SelectedIndex == -1 && cboToLevel.SelectedIndex > -1) 
                    {
                        cboFromLevel.SelectedIndex = 0;
                    }
				}

				// BEGIN Issue 4858
                // Begin Track #5858 - JSmith - Validating store security only
                //base.ValidatePlanNodeSecurity(txtSpreadNode, true);
                base.ValidatePlanNodeSecurity(txtSpreadNode, true, eSecurityTypes.Chain);
                // End Track #5858
				// END Issue 4858

				if (_OTSForecastSpreadMethod.VersionRID > 0)
				{
					cboSpreadVersion.SelectedValue = _OTSForecastSpreadMethod.VersionRID;
					_parentVersion = _OTSForecastSpreadMethod.VersionRID;
				}
				else
				{
					_parentVersion = Include.NoRID;
				}
				// BEGIN Issue 4858
                // Begin Track #5858 - JSmith - Validating store security only
                //base.ValidatePlanVersionSecurity(cboSpreadVersion, true);
                base.ValidatePlanVersionSecurity(cboSpreadVersion.ComboBox, true, ePlanSelectType.Chain);//TT#7 - RBeck - Dynamic dropdowns
                // End Track #5858
				// END Issue 4858

				if (_OTSForecastSpreadMethod.DateRangeRID > 0 && _OTSForecastSpreadMethod.DateRangeRID != Include.UndefinedCalendarDateRange)
				{
					DateRangeProfile drp = SAB.ClientServerSession.Calendar.GetDateRange(_OTSForecastSpreadMethod.DateRangeRID);
					LoadDateRangeSelector(mdsPlanDateRange, drp);
				}
 			 
				// Begin Issue 4328 - stodd 2.28.07
				// Moved ahead of radio buttons so basis grid is defined and RB checked change code
				// can function correctly.
				LoadBasis();
				// End issue 4328

				if (_OTSForecastSpreadMethod.SpreadOption == eSpreadOption.Basis)
					this.rbBasis.Checked = true;
				else
					this.rbPlan.Checked = true;

				if (_OTSForecastSpreadMethod.IgnoreLocks)
					this.cbLocks.Checked = true;
				else
					this.cbLocks.Checked = false;

				// BEGIN ANF - Weighting Multiple Basis
				if (_OTSForecastSpreadMethod.EqualizeWeighting)
				{
					this.rbEWYes.Checked = true;
				}
				else
				{
					this.rbEWNo.Checked = true;
				}
				// END ANF - Weighting Multiple Basis

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
			
				_dtBasis.Columns.Add("DETAIL_SEQ",		System.Type.GetType("System.Int32")); //this column will be hidden.
				_dtBasis.Columns.Add("FV_RID",			System.Type.GetType("System.Int32")); //this column will be hidden.
				_dtBasis.Columns.Add("DateRange",		System.Type.GetType("System.String"));
				_dtBasis.Columns.Add("CDR_RID",			System.Type.GetType("System.Int32")); //this column will be hidden.
				_dtBasis.Columns.Add("WEIGHT",			System.Type.GetType("System.Decimal"));

				if (_dsForecastSpread != null)
				{
					_dtBasis = _dsForecastSpread.Tables["Basis"];
				}
				else
				{
					_dsForecastSpread = new DataSet();
					_dsForecastSpread.Tables.Add(_dtBasis);
				}

				_dtBasis.Columns["WEIGHT"].DefaultValue = 1;

				foreach (DataRow dr in _dtBasis.Rows)
				{
					DateRangeProfile drp = SAB.ClientServerSession.Calendar.GetDateRange(Convert.ToInt32(dr["CDR_RID"], CultureInfo.CurrentUICulture));
					dr["DateRange"] = drp.DisplayDate;
				}	

				grdBasis.DataSource = _dtBasis.DefaultView;
			}
			catch(Exception ex)
			{
				HandleException(ex, "LoadBasis");
			}
		}


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

				// BEGIN MID Track #3792 - replace obsolete method 
				//e.Layout.AutoFitColumns = true;
				e.Layout.AutoFitStyle = AutoFitStyle.ResizeAllColumns;
				// END MID Track #3792

				//hide the key columns.
				//e.Layout.Bands[0].Columns["SGL_RID"].Hidden = true;
				e.Layout.Bands[0].Columns["DETAIL_SEQ"].Hidden = true;
				//e.Layout.Bands[0].Columns["HN_RID"].Hidden = true;
				e.Layout.Bands[0].Columns["CDR_RID"].Hidden = true;
				//e.Layout.Bands[0].Columns["INCLUDE_EXCLUDE"].Hidden = true;
								
				//Prevent the user from re-arranging columns.
				grdBasis.DisplayLayout.Override.AllowColMoving = AllowColMoving.NotAllowed;

				if (FunctionSecurity.AllowUpdate)
				{
					grdBasis.DisplayLayout.Override.AllowAddNew = AllowAddNew.Yes;
					grdBasis.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.True;
					BuildBasisContextMenu();
					grdBasis.ContextMenu = mnuBasisGrid;
				}
				else
				{
					grdBasis.DisplayLayout.Override.AllowAddNew = AllowAddNew.No;
					grdBasis.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
				}

				//Set the header captions.
				//grdBasis.DisplayLayout.Bands[0].Columns["Merchandise"].Header.VisiblePosition = 1;
				//grdBasis.DisplayLayout.Bands[0].Columns["Merchandise"].Header.Caption = "Merchandise";
				grdBasis.DisplayLayout.Bands[0].Columns["FV_RID"].Header.VisiblePosition = 2;
				grdBasis.DisplayLayout.Bands[0].Columns["FV_RID"].Header.Caption = "Version";
				grdBasis.DisplayLayout.Bands[0].Columns["DateRange"].Header.VisiblePosition = 3;
				grdBasis.DisplayLayout.Bands[0].Columns["DateRange"].Header.Caption = "Date Range";
				grdBasis.DisplayLayout.Bands[0].Columns["WEIGHT"].Header.VisiblePosition = 5;
				grdBasis.DisplayLayout.Bands[0].Columns["WEIGHT"].Header.Caption = "Weight";
				//grdBasis.DisplayLayout.Bands[0].Columns["IncludeButton"].Header.VisiblePosition = 6;
				//grdBasis.DisplayLayout.Bands[0].Columns["IncludeButton"].Header.Caption = " ";

				//make the "Version" column a drop down list.
				grdBasis.DisplayLayout.Bands[0].Columns["FV_RID"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
				grdBasis.DisplayLayout.Bands[0].Columns["FV_RID"].ValueList = grdBasis.DisplayLayout.ValueLists["Version"];
		
				//grdBasis.DisplayLayout.Bands[0].Columns["IncludeButton"].CellAppearance.ImageHAlign = Infragistics.Win.HAlign.Center;
				//grdBasis.DisplayLayout.Bands[0].Columns["IncludeButton"].CellAppearance.ImageVAlign = Infragistics.Win.VAlign.Middle;
				//grdBasis.DisplayLayout.Bands[0].Columns["IncludeButton"].Style  = Infragistics.Win.UltraWinGrid.ColumnStyle.Button;
				//grdBasis.DisplayLayout.Bands[0].Columns["IncludeButton"].Width = 20;
				//grdBasis.DisplayLayout.Bands[0].Columns["IncludeButton"].CellActivation = Activation.NoEdit;


				//Make the "INCLUDE_EXCLUDE" column a checkbox column.
				//grdBasis.DisplayLayout.Bands[0].Columns["INCLUDE_EXCLUDE"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox;
		
				//Set the width of the "DateRange" column so that the DateRangeSelector can fit.
				grdBasis.DisplayLayout.Bands[0].Columns["DateRange"].Width = 160;
				// BEGIN Issue 4640 stodd 09.21.2007
				if (FunctionSecurity.AllowUpdate)
				{
					grdBasis.DisplayLayout.Bands[0].Columns["DateRange"].Style  = Infragistics.Win.UltraWinGrid.ColumnStyle.EditButton;
					grdBasis.DisplayLayout.Bands[0].Columns["DateRange"].CellActivation = Activation.NoEdit;
				}
				// End Issue 4640 stodd 09.21.2007

				grdBasis.DisplayLayout.Bands[0].AddButtonCaption = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_AddBasisDetails);
				grdBasis.DisplayLayout.Bands[0].AddButtonToolTipText = MIDText.GetTextOnly(eMIDTextCode.tt_Button_AddBasisDetails);
				grdBasis.DisplayLayout.AddNewBox.Hidden = false;
				grdBasis.DisplayLayout.AddNewBox.Style = AddNewBoxStyle.Compact;
			}
			catch(Exception err)
			{
				HandleException(err);
			}
		}
	
		private void grdBasis_InitializeRow(object sender, Infragistics.Win.UltraWinGrid.InitializeRowEventArgs e)
		{
			if (e.Row.Cells["CDR_RID"].Value.ToString() != "")
			{
				DateRangeProfile dr;
				if (mdsPlanDateRange.DateRangeRID != Include.NoRID)
					dr = SAB.ClientServerSession.Calendar.GetDateRange(Convert.ToInt32(e.Row.Cells["CDR_RID"].Value, CultureInfo.CurrentUICulture),mdsPlanDateRange.DateRangeRID);
				else
					dr = SAB.ClientServerSession.Calendar.GetDateRange(Convert.ToInt32(e.Row.Cells["CDR_RID"].Value, CultureInfo.CurrentUICulture));

				e.Row.Cells["DateRange"].Value = dr.DisplayDate;
				e.Row.Cells["CDR_RID"].Value = dr.Key;

				if (dr.DateRangeType == eCalendarRangeType.Dynamic)
				{
					switch (dr.RelativeTo)
					{
						case eDateRangeRelativeTo.Current:
							e.Row.Cells["DateRange"].Appearance.Image = this.DynamicToCurrentImage;
							break;
						case eDateRangeRelativeTo.Plan:
							e.Row.Cells["DateRange"].Appearance.Image = this.DynamicToPlanImage;
							break;
						default:
							e.Row.Cells["DateRange"].Appearance.Image = null;
							break;
					}
				}
			}
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

		private void grdBasis_CellChange(object sender, Infragistics.Win.UltraWinGrid.CellEventArgs e)
		{
			if (FormLoaded)
			{
				ChangePending = true;
			}
		
		}

		private void grdBasis_AfterCellUpdate(object sender, Infragistics.Win.UltraWinGrid.CellEventArgs e)
		{
			bool errorFound = false;
			string errorMessage = string.Empty;
			try
			{
				if (_skipAfterCellUpdate)
				{
					_skipAfterCellUpdate = false;
					return;
				}		

//				switch (e.Cell.Column.Key)
//				{
//					case "Merchandise":
//						if (e.Cell.Value.ToString().Trim().Length == 0)
//						{
//							e.Cell.Row.Cells["HN_RID"].Value = Include.NoRID;
//						}
//						else
//						{
//							productID = e.Cell.Value.ToString().Trim();
//							HierarchyNodeProfile hnp = SAB.HierarchyServerSession.GetNodeData(productID);
//							if (hnp.Key == Include.NoRID)
//							{
//								errorMessage = string.Format(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_InvalidTreeNode),
//									productID );	
//								errorFound = true;
//							}
//							else 
//							{
//								e.Cell.Row.Cells["HN_RID"].Value = hnp.Key;
//								_skipAfterCellUpdate = true;
//								e.Cell.Value = hnp.Text;
//							}
//						}
//						break;
//					default:
//						break;
//				}
				if (errorFound)
				{
					e.Cell.Appearance.Image = ErrorImage;
					e.Cell.Tag = errorMessage;
				}
				else
				{
					e.Cell.Appearance.Image = null;
					e.Cell.Tag = null;
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
		
		private void grdBasis_AfterRowInsert(object sender, Infragistics.Win.UltraWinGrid.RowEventArgs e)
		{
			try
			{
				//The "IsIncluded" column is a checkbox column. Right after we inserted a
				//new row, we want to default this column to be checked (true). If we
				//don't, it will default to a grayed-out check, like the 3rd state in a 
				//tri-state checkbox, even if we explicitly set this column to be a normal
				//checkbox.
				
				if (e.Row.Band == grdBasis.DisplayLayout.Bands[0])
				{
					//e.Row.Cells["SGL_RID"].Value = Convert.ToInt32(cboAttributeSet.SelectedValue,CultureInfo.CurrentUICulture);
					//e.Row.Cells["HN_RID"].Value = Include.NoRID;
					e.Row.Cells["CDR_RID"].Value = Include.UndefinedCalendarDateRange;
					//e.Row.Cells["INCLUDE_EXCLUDE"].Value = (int)eBasisIncludeExclude.Include;
					//e.Row.Cells["IncludeButton"].Appearance.Image = _picInclude;
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

		private void grdBasis_DragOver(object sender, System.Windows.Forms.DragEventArgs e)
		{
			try
			{
				Infragistics.Win.UIElement aUIElement;
				aUIElement = grdBasis.DisplayLayout.UIElement.ElementFromPoint(grdBasis.PointToClient(new Point(e.X, e.Y)));

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
		private void grdBasis_DragDrop(object sender, System.Windows.Forms.DragEventArgs e)
		{
            TreeNodeClipboardList cbList;
			HierarchyNodeProfile hnp;
			try
			{
				Infragistics.Win.UIElement aUIElement;

				aUIElement = grdBasis.DisplayLayout.UIElement.ElementFromPoint(grdBasis.PointToClient(new Point(e.X, e.Y)));

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
							
									//Begin Track #5378 - color and size not qualified
//									hnp = SAB.HierarchyServerSession.GetNodeData(cbp.Key);
                                hnp = SAB.HierarchyServerSession.GetNodeData(cbList.ClipboardProfile.Key, true, true);
									//End Track #5378
									
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
									grdBasis.UpdateData();
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

						frmCalDtSelector = (CalendarDateSelector)CreateControl(typeof(CalendarDateSelector), new object[]{SAB});

						if (e.Cell.Row.Cells["DateRange"].Value != null &&
							e.Cell.Row.Cells["DateRange"].Value != System.DBNull.Value &&
							e.Cell.Row.Cells["DateRange"].Text.Length > 0)
						{
							frmCalDtSelector.DateRangeRID = Convert.ToInt32(e.Cell.Row.Cells["CDR_RID"].Value, CultureInfo.CurrentUICulture);
						}

						if (mdsPlanDateRange.DateRangeRID != Include.UndefinedCalendarDateRange)
						{
							frmCalDtSelector.AnchorDateRangeRID = mdsPlanDateRange.DateRangeRID;
						}
						else
						{
							frmCalDtSelector.AnchorDate = SAB.ClientServerSession.Calendar.CurrentDate;
						}

						frmCalDtSelector.AnchorDateRelativeTo = eDateRangeRelativeTo.Current;
						frmCalDtSelector.AllowDynamicToStoreOpen = false;
						// BEGIN Issue 5232 stodd 4.25.2008
						frmCalDtSelector.AllowDynamicToPlan = true;
						// END Issue 5242

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
						}

						e.Cell.CancelUpdate();
						grdBasis.PerformAction(UltraGridAction.DeactivateCell);


						break;

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
//						grdBasis.PerformAction(UltraGridAction.DeactivateCell);
//
//						break;
				}
			}
			catch(Exception exception)
			{
				HandleException(exception);
			}
		}
		private void grdBasis_MouseEnterElement(object sender, Infragistics.Win.UIElementEventArgs e)
		{
			try
			{
				ShowUltraGridToolTip(grdBasis, e);
			}
			catch(Exception ex)
			{
				HandleException(ex);
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
				if (grdBasis.Rows.Count > 0)
				{
					if (grdBasis.ActiveRow == null) return;
					rowPosition = Convert.ToInt32(grdBasis.ActiveRow.Cells["DETAIL_SEQ"].Value, CultureInfo.CurrentUICulture);
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
				//addedRow["HN_RID"] = Include.NoRID;
				addedRow["CDR_RID"] = Include.UndefinedCalendarDateRange;
				//addedRow["INCLUDE_EXCLUDE"] = (int)eBasisIncludeExclude.Include;

				_dtBasis.Rows.Add(addedRow);
				_dtBasis.AcceptChanges();
				grdBasis.DisplayLayout.Bands[0].SortedColumns.Clear();
				grdBasis.DisplayLayout.Bands[0].SortedColumns.Add("DETAIL_SEQ", false);
				ChangePending = true;
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}	
		private void mnuBasisGridItemDelete_Click(object sender, System.EventArgs e)
		{
			if (grdBasis.Selected.Rows.Count > 0)
			{
				grdBasis.DeleteSelectedRows();
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
                //Begin Track #5858 - KJohnson - Validating store security only
                HierarchyNodeProfile hnp = (HierarchyNodeProfile)((MIDTag)txtSpreadNode.Tag).MIDTagData;
                _OTSForecastSpreadMethod.HierNodeRID = hnp.Key;
                //End Track #5858
                _OTSForecastSpreadMethod.VersionRID = Convert.ToInt32(cboSpreadVersion.SelectedValue, CultureInfo.CurrentUICulture);

                _OTSForecastSpreadMethod.FromLevelType = ((FromLevelCombo)cboFromLevel.SelectedItem).FromLevelType;
                _OTSForecastSpreadMethod.FromLevelOffset = ((FromLevelCombo)cboFromLevel.SelectedItem).FromLevelOffset;
                _OTSForecastSpreadMethod.FromLevelSequence = ((FromLevelCombo)cboFromLevel.SelectedItem).FromLevelSequence;
                _OTSForecastSpreadMethod.ToLevelType = ((ToLevelCombo)cboToLevel.SelectedItem).ToLevelType;
                _OTSForecastSpreadMethod.ToLevelOffset = ((ToLevelCombo)cboToLevel.SelectedItem).ToLevelOffset;
                _OTSForecastSpreadMethod.ToLevelSequence = ((ToLevelCombo)cboToLevel.SelectedItem).ToLevelSequence;

                if (this.cbMultiLevel.Checked)
                {
                    _OTSForecastSpreadMethod.MultiLevel = true;
                }
                else
                {
                    _OTSForecastSpreadMethod.MultiLevel = false;
                }

                _OTSForecastSpreadMethod.DateRangeRID = mdsPlanDateRange.DateRangeRID;
				if (this.cbLocks.Checked)
                    _OTSForecastSpreadMethod.IgnoreLocks = true;
				else
                    _OTSForecastSpreadMethod.IgnoreLocks = false;
				if (this.rbPlan.Checked)
                    _OTSForecastSpreadMethod.SpreadOption = eSpreadOption.Plan;
				else
                    _OTSForecastSpreadMethod.SpreadOption = eSpreadOption.Basis;
				
				grdBasis.UpdateData();

                _OTSForecastSpreadMethod.EqualizeWeighting = rbEWYes.Checked; // ANF - Weighting Multiple Basis

                _OTSForecastSpreadMethod.DSForecastSpread = _dsForecastSpread;
                if (_OTSForecastSpreadMethod.SpreadOption == eSpreadOption.Plan)
				{
					_OTSForecastSpreadMethod.DSForecastSpread.Tables["Basis"].Rows.Clear();
				}
                //_OTSForecastSpreadMethod.DTLowerLevels = _dtLowerLevels; <---DKJ
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
                //if (Convert.ToInt32(cboSpreadVersion.SelectedValue, CultureInfo.CurrentUICulture) == Include.NoRID)  //TT#741 - MD - ComboBox NullReference Exception red icon not displayed - RBeck 
                if (cboSpreadVersion.SelectedValue == null || Convert.ToInt32(cboSpreadVersion.SelectedValue, CultureInfo.CurrentUICulture) == Include.NoRID)
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

                if (cboFromLevel.Items.Count == 0)
                {
                    methodFieldsValid = false;
                    ErrorProvider.SetError(cboFromLevel, SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_pl_FromLevelsNotDefined));
                }
                else
                {
                    ErrorProvider.SetError(cboFromLevel, string.Empty);
                    // BEGIN MID Track #5149 - error when basis weeks > high level weeks
                    DateRangeProfile drp = SAB.ClientServerSession.Calendar.GetDateRange(mdsPlanDateRange.DateRangeRID, null);
                    _highLevelWeekCount = SAB.ClientServerSession.Calendar.GetDateRangeWeeks(drp, null).Count;
                    // END MID Track #5149  
                }
            //TT#736 - Begin - MD - ComboBox causes a NullReferenceException - RBeck
                if (cboFromLevel.SelectedIndex == -1)
                {
                    methodFieldsValid = false;
                    ErrorProvider.SetError(cboFromLevel, SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_FieldIsRequired));
                }
                else
                {
                    ErrorProvider.SetError(cboFromLevel, string.Empty);
                }
            //TT#736 - End - MD - ComboBox causes a NullReferenceException - RBeck
                if (cboToLevel.Items.Count == 0)
                {
                    methodFieldsValid = false;
                    ErrorProvider.SetError(cboToLevel, SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_pl_ToLevelsNotDefined));
                }
                else
                {
                    ErrorProvider.SetError(cboToLevel, string.Empty);
                    // BEGIN MID Track #5149 - error when basis weeks > high level weeks
                    DateRangeProfile drp = SAB.ClientServerSession.Calendar.GetDateRange(mdsPlanDateRange.DateRangeRID, null);
                    _highLevelWeekCount = SAB.ClientServerSession.Calendar.GetDateRangeWeeks(drp, null).Count;
                    // END MID Track #5149  
                }
            //TT#736 - Begin - MD - ComboBox causes a NullReferenceException - RBeck
                if (cboToLevel.SelectedIndex == -1)
                {
                    methodFieldsValid = false;
                    ErrorProvider.SetError(cboToLevel, SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_FieldIsRequired));
                }
                else
                {
                    ErrorProvider.SetError(cboToLevel, string.Empty);
                }
            //TT#736 - End - MD - ComboBox causes a NullReferenceException - RBeck

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
				ABM = _OTSForecastSpreadMethod;
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
			ErrorProvider.SetError (grdBasis,string.Empty);
			try
			{
				if (rbBasis.Checked) //  ANF - Weighting Multiple Basis
				{
					double totWeight = 0; 
					// BEGIN Issue 4233 stodd 9.21.2007
					if (grdBasis.Rows.Count == 0 && this.rbBasis.Checked == true)
					{
						errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_pl_BasisRequired);
						grdBasis.Tag = errorMessage;
						ErrorProvider.SetError (grdBasis,errorMessage);
						errorFound = true;
					}
					// END Issue 4233 stodd 9.21.2007

					if (grdBasis.Rows.Count > 0)
					{
						foreach (UltraGridRow gridRow in grdBasis.Rows)
						{
							if (!ValidVersion(gridRow.Cells["FV_RID"]))
							{
								errorFound = true;
							}
							if (!DateRangeValid(gridRow.Cells["DateRange"]))
							{
								errorFound = true;
							}
							if (!WeightValid(gridRow.Cells["WEIGHT"]))
							{
								errorFound = true;
							}
							// BEGIN ANF - Weighting Multiple Basis
							else if (rbEWYes.Checked)
							{
								totWeight += Convert.ToDouble(gridRow.Cells["WEIGHT"].Value, CultureInfo.CurrentUICulture);
							}
						}
						if (rbEWYes.Checked && totWeight != 1)
						{
							errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_pl_BasisWeightTotalInvalid);
							grdBasis.Tag = errorMessage;
							ErrorProvider.SetError (grdBasis,errorMessage);
							errorFound = true;
						}
						// END ANF - Weighting Multiple Basis
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
				return false;
			else
				return true;
		}	

		private bool ValidVersion(UltraGridCell gridCell)
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
				gridCell.Appearance.Image = null;
				gridCell.Tag = null;
				return true;
			}
		}	

		private bool DateRangeValid(UltraGridCell gridCell)
		{
			bool errorFound = false;
			string errorMessage = string.Empty;
			try
			{
				if (gridCell.Text.Length == 0)	
				{
					errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_FieldIsRequired);
					errorFound = true;
				}
				// BEGIN MID Track #5149 - error when basis weeks > high level weeks
				else  
				{
					int cdrRID = Convert.ToInt32(gridCell.Row.Cells["CDR_RID"].Value, CultureInfo.CurrentUICulture);
					DateRangeProfile drp = SAB.ClientServerSession.Calendar.GetDateRange(cdrRID, null);
					int basisWeekCount = SAB.ClientServerSession.Calendar.GetDateRangeWeeks(drp,null).Count;
					// BEGIN Issue 5578/5681 stodd
					// This edit is not needed.
					//if (basisWeekCount > _highLevelWeekCount)
					//{
					//    errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_pl_BasisWeeksExceedHighLevelWeeks);
					//    errorFound = true;
					//}
					// END Issue 5578/5681
				}	
			}	// END MID Track #5149
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
				if (gridCell.Appearance.Image == ErrorImage)
				{
					gridCell.Appearance.Image = null;
					gridCell.Tag = null;
				}
				return true;
			}
		}	
		private bool WeightValid(UltraGridCell gridCell)
		{
			bool errorFound = false;
			string errorMessage = string.Empty;
			double dblValue;
			try
			{
				if (gridCell.Text.Length == 0)	
				{
					errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_FieldIsRequired);
					errorFound = true;
				}
				else
				{
					dblValue = Convert.ToDouble(gridCell.Text, CultureInfo.CurrentUICulture);
					// BEGIN ANF - Weighting Multiple Basis
					//if (dblValue < 1)
					//{
					//	errorMessage = string.Format
					//		(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_al_MinimumValueExceeded),dblValue, "1");
					//	errorFound = true;
					//}
					if (dblValue <= 0)
					{
						errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_pl_BasisWeightInvalid);
						errorFound = true;
					}
					else if (dblValue > 9999)
					{
						errorMessage = string.Format
							(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_al_MaximumValueExceeded),dblValue, "9999");
						errorFound = true;
					}
					
					if (rbEWYes.Checked)
					{
						if (dblValue > 1)
						{
							errorMessage = string.Format
								(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_al_MaximumValueExceeded),dblValue, "1");
							errorFound = true;
						}
					}	
					// END ANF - Weighting Multiple Basis
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
		#endregion Validate Basis Grid		
	
		#region Button Events
		// Begin MID Track 4858 - JSmith - Security changes
//		private void btnProcess_Click(object sender, System.EventArgs e)
//		{
//			try
//			{
//				_basisNodeRequired = true;
//				ProcessAction(eMethodType.ForcastSpread);
//				// as part of the  processing we saved the info, so it should be changed to update.
//				if (!ErrorFound)
//				{
//					_OTSForcastSpreadMethod.Method_Change_Type = eChangeType.update;
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
//					_OTSForcastSpreadMethod.Method_Change_Type = eChangeType.update;
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
				ProcessAction(eMethodType.ForecastSpread);
				// as part of the  processing we saved the info, so it should be changed to update.

                // Begin TT#2281 - JSmith - Error when updating Low Level in Matrix Balance OTS method
                if (_OTSForecastSpreadMethod.FoundDuplicate)
                {
                    MessageBox.Show(MIDText.GetText(eMIDTextCode.msg_DuplicateDescendantInMethodError, _OTSForecastSpreadMethod.DuplicateMessage),
                    _OTSForecastSpreadMethod.Name, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    ErrorFound = true;
                }
                // End TT#2281

				if (!ErrorFound)
				{
					_OTSForecastSpreadMethod.Method_Change_Type = eChangeType.update;
					btnSave.Text = "&Update";

                    lblDateTimeValue.Text = _OTSForecastSpreadMethod.LastProcessedDateTime;
                    lblByUserValue.Text = _OTSForecastSpreadMethod.LastProcessedUser;
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

        private void cboOverride_SelectionChangeCommitted(object sender, System.EventArgs e)
        {
			if (FormLoaded)
			{
				_OTSForecastSpreadMethod.OverrideLowLevelRid = ((ComboObject)cboOverride.SelectedItem).Key;
				ChangePending = true;
			}

        }

		private void txtSpreadNode_TextChanged(object sender, System.EventArgs e)
		{
			if (FormLoaded)
			{
				ChangePending = true;
			}
		}
		
		private void txtSpreadNode_DragEnter(object sender, System.Windows.Forms.DragEventArgs e)
		{
            Image_DragEnter(sender, e);
		}


        private void txtSpreadNode_DragLeave(object sender, EventArgs e)
        {
            Image_DragLeave(sender, e);
        }

		private void txtSpreadNode_DragDrop(object sender, System.Windows.Forms.DragEventArgs e)
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

                    _OTSForecastSpreadMethod.HierNodeRID = hnp.Key;

                    PopulateFromToLevels(hnp, cboFromLevel.ComboBox, 0);//TT#7 - RBeck - Dynamic dropdowns
                    PopulateFromToLevels(hnp, cboToLevel.ComboBox, 0);//TT#7 - RBeck - Dynamic dropdowns

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
//                            _OTSForecastSpreadMethod.HierNodeRID = cbp.Key;

//                            //Begin Track #5858 - KJohnson - Validating store security only
//                            ((TextBox)sender).Text = hnp.Text;
//                            ((MIDTag)(((TextBox)sender).Tag)).MIDTagData = hnp;
//                            //End Track #5858

//                            // BEGIN Issue 4858 stodd
//                            //Begin Track #5858 - JSmith - Validating store security only
//                            //base.ValidatePlanNodeSecurity(txtSpreadNode, true);
//                            //base.ValidatePlanNodeSecurity(txtSpreadNode, true, eSecurityTypes.Store);
//                            //End Track #5858

//                            PopulateLowLevels(hnp, cboLowerLevels);
//                            LoadLowerLevel(cbp.Key);

//                            ApplySecurity();  

//                            //this.ValidLowerLevelGrid();
//                            //bool canUpdate = ABM.AuthorizedToUpdate(this.SAB.ClientServerSession, this.SAB.ClientServerSession.UserRID);
//                            //base.ApplyCanUpdate(canUpdate);
//                            // END Issue 4858
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

		//private void txtSpreadNode_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		//{
		//	e.Handled = true;
		//}

		private void txtSpreadNode_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
            //_needsValidated = true;
		}
		
		private void txtSpreadNode_Validating(object sender, System.ComponentModel.CancelEventArgs e)
		{
//            string productID; 
//            try
//            {
//                if (txtSpreadNode.Modified)
//                {
//                    if (txtSpreadNode.Text.Trim().Length > 0)
//                    {
//                        productID = txtSpreadNode.Text.Trim();
//                        _nodeRID = GetNodeText(ref productID);
//                        if (_nodeRID == Include.NoRID)
//                            MessageBox.Show(productID + " is not valid; please enter or drag and drop a node from the tree");
//                        else 
//                        {
//                            //Begin Track #5858 - KJohnson - Validating store security only
//                            txtSpreadNode.Text = productID;
//                            ((MIDTag)(txtSpreadNode.Tag)).MIDTagData = _nodeRID;
//                            //End Track #5858

//                            _OTSForecastSpreadMethod.HierNodeRID = _nodeRID;

//                            // Begin Issue 3800 - stodd
//                            //Begin Track #5378 - color and size not qualified
////							HierarchyNodeProfile hnp = SAB.HierarchyServerSession.GetNodeData(_nodeRID);
//                            HierarchyNodeProfile hnp = SAB.HierarchyServerSession.GetNodeData(_nodeRID, true, true);
//                            //End Track #5378
//                            PopulateLowLevels(hnp, cboLowerLevels);
//                            // End Issue 3800

//                            LoadLowerLevel(_nodeRID);

//                            // BEGIN Issue 4858 stodd
//                            //Begin Track #5858 - JSmith - Validating store security only
//                            //base.ValidatePlanNodeSecurity(txtSpreadNode, true);
//                            //base.ValidatePlanNodeSecurity(txtSpreadNode, true, eSecurityTypes.Store);
//                            //End Track #5858

//                            ApplySecurity();

//                            //this.ValidLowerLevelGrid();
//                            //bool canUpdate = ABM.AuthorizedToUpdate(this.SAB.ClientServerSession, this.SAB.ClientServerSession.UserRID);
//                            //base.ApplyCanUpdate(canUpdate);
//                            // END Issue 4858
//                        }
//                    }
//                    else
//                    {
//                        //Begin Track #5858 - KJohnson - Validating store security only
//                        ((MIDTag)(txtSpreadNode.Tag)).MIDTagData = null;
//                        //End Track #5858
//                    }
//                }
//            }
//            catch
//            {
//                throw;
//            }
		}

        private void txtSpreadNode_Validated(object sender, EventArgs e)
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

                    _OTSForecastSpreadMethod.HierNodeRID = _nodeRID;

                    PopulateFromToLevels(hnp, cboFromLevel.ComboBox, 0);//TT#7 - RBeck - Dynamic dropdowns
                    PopulateFromToLevels(hnp, cboToLevel.ComboBox, 0);//TT#7 - RBeck - Dynamic dropdowns

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
				this._OTSForecastSpreadMethod.VersionRID = _parentVersion;
				ApplySecurity();
				//bool canUpdate = ABM.AuthorizedToUpdate(this.SAB.ClientServerSession, this.SAB.ClientServerSession.UserRID);
				//base.ApplyCanUpdate(canUpdate);
				// END Issue 4858
			}
			// BEGIN Issue 4858 jsmith
			else if (_OTSForecastSpreadMethod.Method_Change_Type == eChangeType.add)
			{
				_OTSForecastSpreadMethod.VersionRID = Convert.ToInt32(cboSpreadVersion.SelectedValue, CultureInfo.CurrentUICulture);
			}
			// END Issue 4858
			//base.ValidatePlanVersionSecurity(cboSpreadVersion, true);
		}

		override protected bool ApplySecurity()	// track 5871 stodd
		{
			bool securityOk = true;
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

            // Begin Track #5858 - JSmith - Validating store security only
            //SecurityOk = base.ValidatePlanNodeSecurity(txtSpreadNode, true);
            securityOk = base.ValidateChainPlanVersionSecurity(cboSpreadVersion.ComboBox, true);//TT#7 - RBeck - Dynamic dropdowns

            if (securityOk)
               securityOk = (((MIDControlTag)(txtSpreadNode.Tag)).IsAuthorized(eSecurityTypes.Chain, eSecuritySelectType.Update));


			bool canUpdate = ABM.AuthorizedToUpdate(this.SAB.ClientServerSession, this.SAB.ClientServerSession.UserRID);
			base.ApplyCanUpdate(canUpdate);
			ErrorProvider.SetError(btnOverride, string.Empty);
			if (!canUpdate)
			{
				if (FunctionSecurity.IsReadOnly
                    || txtSpreadNode.Text.Trim().Length == 0
					|| (cboSpreadVersion.SelectedValue == null
						|| Convert.ToInt32(cboSpreadVersion.SelectedValue, CultureInfo.CurrentUICulture) == Include.NoRID))
				{
					// Skip
				}
				else
				{
					securityOk = false;
					string errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_NotAuthorizedToPlanLowLevel);
					ErrorProvider.SetError(btnOverride, errorMessage);
				}
			}
			return securityOk;	// track 5871 stodd
		}

		#endregion

        // Begin TT#56 - JSmith - not holding to level after save or update
        //private void PopulateFromToLevels(HierarchyNodeProfile aHierarchyNodeProfile, ComboBox aComboBox, int toOffset)
        private void PopulateFromToLevels(HierarchyNodeProfile aHierarchyNodeProfile, ComboBox aComboBox, int toOffset)
        {
            PopulateFromToLevels(aHierarchyNodeProfile, aComboBox, toOffset, true);
        }

        private void PopulateFromToLevels(HierarchyNodeProfile aHierarchyNodeProfile, ComboBox aComboBox, int toOffset, bool aSetClassValues)
        // End TT#56 - JSmith
        {
            try
            {
                HierarchyProfile hierProf;
                object oldSelectedItem = aComboBox.SelectedItem;
                aComboBox.Items.Clear();
                //if (aHierarchyNodeProfile == null)
                //{
                //	aComboBox.Enabled = false;
                //}
                //else
                //{
                //	aComboBox.Enabled = true;
                //}

                int offset = 0;
                int fromLimit = 0;
                if (aComboBox.Name == "cboFromLevel")
                {
                    offset = 0;
                    fromLimit = -1;
                }
                else
                {
                    offset = 1;
                    fromLimit = 0;
                }

                if (aHierarchyNodeProfile != null)
                {
                    hierProf = SAB.HierarchyServerSession.GetHierarchyData(aHierarchyNodeProfile.HierarchyRID);
                    if (hierProf.HierarchyType == eHierarchyType.organizational)
                    {
                        for (int i = aHierarchyNodeProfile.HomeHierarchyLevel + offset; i <= hierProf.HierarchyLevels.Count + fromLimit; i++)
                        {
                            if (i == 0) // hierarchy
                            {
                                if (aComboBox.Name == "cboFromLevel")
                                {
                                    aComboBox.Items.Add(
                                        new FromLevelCombo(eFromLevelsType.HierarchyLevel,
                                        0,
                                        0,
                                        hierProf.HierarchyID));
                                }
                                else
                                {
                                    if (cboFromLevel.Items.Count > 0)
                                    {
                                        aComboBox.Items.Add(
                                            new ToLevelCombo(eToLevelsType.HierarchyLevel,
                                            0,
                                            0,
                                            hierProf.HierarchyID));
                                    }
                                }
                            }
                            else
                            {
                                HierarchyLevelProfile hlp = (HierarchyLevelProfile)hierProf.HierarchyLevels[i];
                                if (hlp != null)
                                {
                                    if (aComboBox.Name == "cboFromLevel")
                                    {
                                        aComboBox.Items.Add(
                                            new FromLevelCombo(eFromLevelsType.HierarchyLevel,
                                            0,
                                            hlp.Key,
                                            hlp.LevelID));
                                    }
                                    else
                                    {
                                        if (cboFromLevel.Items.Count > 0)
                                        {
                                            aComboBox.Items.Add(
                                                new ToLevelCombo(eToLevelsType.HierarchyLevel,
                                                0,
                                                hlp.Key,
                                                hlp.LevelID));
                                        }
                                    }
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                    }
                    else
                    {
                        HierarchyProfile mainHierProf = SAB.HierarchyServerSession.GetMainHierarchyData();

                        int highestGuestLevel = SAB.HierarchyServerSession.GetHighestGuestLevel(aHierarchyNodeProfile.Key);

                        // add guest levels to comboBox
                        if (highestGuestLevel != int.MaxValue)
                        {
                            //for (int i = highestGuestLevel; i <= mainHierProf.HierarchyLevels.Count; i++)
                            //{
                            //    if (i == 0)
                            //    {
                            //        if (aComboBox.Name == "cboFromLevel")
                            //        {
                            //            aComboBox.Items.Add(
                            //                new FromLevelCombo(eFromLevelsType.HierarchyLevel,
                            //                0,
                            //                0,
                            //                "Root"));
                            //        }
                            //        else 
                            //        {
                            //            aComboBox.Items.Add(
                            //                new ToLevelCombo(eToLevelsType.HierarchyLevel,
                            //                0,
                            //                0,
                            //                "Root"));
                            //        }
                            //    }
                            //    else
                            //    {
                            //        HierarchyLevelProfile hlp = (HierarchyLevelProfile)mainHierProf.HierarchyLevels[i];
                            //        if (aComboBox.Name == "cboFromLevel")
                            //        {
                            //            aComboBox.Items.Add(
                            //            new FromLevelCombo(eFromLevelsType.HierarchyLevel,
                            //            0,
                            //            hlp.Key,
                            //            hlp.LevelID));
                            //        }
                            //        else 
                            //        {
                            //            aComboBox.Items.Add(
                            //            new ToLevelCombo(eToLevelsType.HierarchyLevel,
                            //            0,
                            //            hlp.Key,
                            //            hlp.LevelID));
                            //        }
                            //    }
                            //}
                        }

                        // add offsets to comboBox
                        //BEGIN TT#4689 - DOConnell - OTS Forecast - Multi-Level Low Levels not being populated correctly
                        //int longestBranchCount = SAB.HierarchyServerSession.GetLongestBranch(aHierarchyNodeProfile.Key);
                        DataTable hierarchyLevels = SAB.HierarchyServerSession.GetHierarchyDescendantLevels(aHierarchyNodeProfile.Key);
                        int longestBranchCount = hierarchyLevels.Rows.Count;
                        //END TT#4689 - DOConnell - OTS Forecast - Multi-Level Low Levels not being populated correctly

                        if (aComboBox.Name == "cboFromLevel")
                        {
                            offset = -1;
                            longestBranchCount = longestBranchCount + 1;
                        }
                        else
                        {
                            offset = 0;
                        }

                        for (int i = 0; i < longestBranchCount + fromLimit; i++)
                        {
                            ++offset;
                            if (aComboBox.Name == "cboFromLevel")
                            {
                                aComboBox.Items.Add(
                                new FromLevelCombo(eFromLevelsType.LevelOffset,
                                offset,
                                0,
                                null));
                            }
                            else
                            {
                                aComboBox.Items.Add(
                                new ToLevelCombo(eToLevelsType.LevelOffset,
                                offset,
                                0,
                                null));
                            }
                        }
                    }

                    if (aComboBox.Items.Count > 0)
                    {
                        if (toOffset > 0)
                        {
                            int count = aComboBox.Items.Count;
                            for (int i = 0; i < toOffset; i++)
                            {
                                aComboBox.Items.RemoveAt(0);
                            }

                            if (oldSelectedItem != null && aComboBox.Items.IndexOf(oldSelectedItem) > -1)
                            {
                                aComboBox.SelectedIndex = aComboBox.Items.IndexOf(oldSelectedItem);
                            }
                            else
                            {
                                aComboBox.SelectedIndex = 0;
                            }

                        }
                        else 
                        {
                            if (oldSelectedItem != null && aComboBox.Items.IndexOf(oldSelectedItem) > -1 && aComboBox.Name == "cboToLevel")
                            {
                                aComboBox.SelectedIndex = aComboBox.Items.IndexOf(oldSelectedItem);
                            }
                            else
                            {
                                aComboBox.SelectedIndex = 0;
                            }
                        }

                    }
                }

                // BEGIN Issue 5871 stodd
                // Begin TT#56 - JSmith - not holding to level after save or update
                if (aSetClassValues)
                {
                    // End TT#56
                    if (aComboBox.Name == "cboFromLevel")
                    {
                        _OTSForecastSpreadMethod.FromLevelType = ((FromLevelCombo)aComboBox.SelectedItem).FromLevelType;
                        _OTSForecastSpreadMethod.FromLevelOffset = ((FromLevelCombo)aComboBox.SelectedItem).FromLevelOffset;
                        _OTSForecastSpreadMethod.FromLevelSequence = ((FromLevelCombo)aComboBox.SelectedItem).FromLevelSequence;
                    }
                    else
                    {
                        _OTSForecastSpreadMethod.ToLevelType = ((ToLevelCombo)aComboBox.SelectedItem).ToLevelType;
                        _OTSForecastSpreadMethod.ToLevelOffset = ((ToLevelCombo)aComboBox.SelectedItem).ToLevelOffset;
                        _OTSForecastSpreadMethod.ToLevelSequence = ((ToLevelCombo)aComboBox.SelectedItem).ToLevelSequence;
                    }
                // Begin TT#56 - JSmith - not holding to level after save or update
                }
                // End TT#56
                // END Issue 5871 stodd
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

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
                GetOTSPLANWorkflows(_OTSForecastSpreadMethod.Key, ugWorkflows);
            }
            catch (Exception ex)
            {
                HandleException(ex, "LoadWorkflows");
            }
		}
		#endregion

        private void btnOverride_Click(object sender, EventArgs e)
        {
            if (FormLoaded)
            {
                Cursor.Current = Cursors.WaitCursor;
                try
                {
					string lowLevelText = string.Empty;
					if (cboToLevel.SelectedIndex != -1)
						lowLevelText = cboToLevel.Items[cboToLevel.SelectedIndex].ToString();
					

                    System.Windows.Forms.Form parentForm;
                    parentForm = this.MdiParent;

                    object[] args = null;

                    //Begin tt#700 - APicchetti - forecast methods show the method has changed when user copens Override Low Level model and then clicks close
                    //System.Windows.Forms.Form frm;
                    //End tt#700

					// Begin Track #5909 - stodd
					FunctionSecurityProfile methodSecurity;
					if (radGlobal.Checked)
						methodSecurity = GlobalSecurity;
					else
						methodSecurity = UserSecurity;
					args = new object[] { SAB, _OTSForecastSpreadMethod.OverrideLowLevelRid, _OTSForecastSpreadMethod.HierNodeRID, _OTSForecastSpreadMethod.VersionRID, lowLevelText, _OTSForecastSpreadMethod.CustomOLL_RID, methodSecurity };
					// End Track #5909 - stodd

                    //Begin tt#700 - APicchetti - forecast methods show the method has changed when user copens Override Low Level model and then clicks close
                    _overrideLowLevelfrm = GetForm(typeof(frmOverrideLowLevelModel), args, false);
                    parentForm = this.MdiParent;
                    _overrideLowLevelfrm.MdiParent = parentForm;
                    _overrideLowLevelfrm.Anchor = AnchorStyles.Left | AnchorStyles.Top;
                    _overrideLowLevelfrm.Show();
                    _overrideLowLevelfrm.BringToFront();
                    ((frmOverrideLowLevelModel)_overrideLowLevelfrm).OnOverrideLowLevelCloseHandler += new frmOverrideLowLevelModel.OverrideLowLevelCloseEventHandler(OnOverrideLowLevelCloseHandler);

                    //frm = GetForm(typeof(frmOverrideLowLevelModel), args, false);
                    //parentForm = this.MdiParent;
                    //frm.MdiParent = parentForm;
                    //frm.Anchor = AnchorStyles.Left | AnchorStyles.Top;
                    //frm.Show();
                    //frm.BringToFront();
                    //((frmOverrideLowLevelModel)frm).OnOverrideLowLevelCloseHandler += new frmOverrideLowLevelModel.OverrideLowLevelCloseEventHandler(OnOverrideLowLevelCloseHandler);
                    //End tt#700

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
					if (_OTSForecastSpreadMethod.OverrideLowLevelRid != e.aOllRid)
						ChangePending = true;
					_OTSForecastSpreadMethod.OverrideLowLevelRid = e.aOllRid;
					if (_OTSForecastSpreadMethod.CustomOLL_RID != e.aCustomOllRid)
					{
						_OTSForecastSpreadMethod.CustomOLL_RID = e.aCustomOllRid;
						UpdateMethodCustomOLLRid(_OTSForecastSpreadMethod.Key, _OTSForecastSpreadMethod.CustomOLL_RID);
					}

                    //Begin tt#700 - APicchetti - forecast methods show the method has changed when user copens Override Low Level model and then clicks close
                    if (_overrideLowLevelfrm.DialogResult != DialogResult.Cancel)
                    {
                        LoadOverrideModelComboBox(cboOverride.ComboBox, e.aOllRid, _OTSForecastSpreadMethod.CustomOLL_RID);//TT#7 - RBeck - Dynamic dropdowns
                    }

                    _overrideLowLevelfrm = null;
                    // End tt#700


					//LoadOverrideModelComboBox(cboOverride, e.aOllRid, _OTSForecastSpreadMethod.CustomOLL_RID);
				}
			}
			catch
			{
				throw;
			}

		}

		//private System.Windows.Forms.Form GetForm(System.Type aType, object[] args, bool aAlwaysCreateNewForm)
		//{
		//    try
		//    {
		//        bool foundForm = false;
		//        System.Windows.Forms.Form frm = null;

		//        if (!aAlwaysCreateNewForm)
		//        {
		//            foreach (Form childForm in this.MdiParent.MdiChildren)
		//            {
		//                if (childForm.GetType().Equals(aType))
		//                {
		//                    frm = childForm;
		//                    foundForm = true;
		//                    break;
		//                }
		//                else
		//                {
		//                    childForm.Enabled = false;
		//                }
		//            }
		//        }

		//        if (aAlwaysCreateNewForm ||
		//            !foundForm)
		//        {
		//            frm = (System.Windows.Forms.Form)Activator.CreateInstance(aType, args);
		//        }

		//        return frm;
		//    }
		//    catch (Exception exception)
		//    {
		//        MessageBox.Show(exception.ToString(), this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
		//        throw;
		//    }
		//}

        private void cbMultiLevel_CheckedChanged(object sender, System.EventArgs e)
        {
            if (FormLoaded)
            {
                ChangePending = true;

                if (this.cbMultiLevel.Checked)
                {
                    this.cboFromLevel.Enabled = true;
                    // BEGIN MID Track #5115 - KJohnson - Basis Merchandise is not disabled from selection
                    //this.grdBasis.Enabled = false;  // TT#57 RMatelic -  Forecast spread method with multi checked and basis-> basis does not open up
                    // END MID Track #5115
                    //this.cboToLevel.Enabled = true;
                    //this.txtOverride.Enabled = true;
                    //this.btnOverride.Enabled = true;
                }
                else
                {
                    this.cboFromLevel.Enabled = false;
                    // BEGIN MID Track #5115 - KJohnson - Basis Merchandise is not disabled from selection
                    //this.grdBasis.Enabled = true;  // TT#57 RMatelic -  Forecast spread method with multi checked and basis-> basis does not open up
                    // END MID Track #5115
                    //this.cboToLevel.Enabled = false;
                    //this.txtOverride.Enabled = false;
                    //this.btnOverride.Enabled = false;
                }
                // Begin TT#56 - JSmith - not holding to level after save or update
                //}
                // End TT#56

                if (_OTSForecastSpreadMethod.HierNodeRID > 0)
                {
                    HierarchyNodeProfile hnp = SAB.HierarchyServerSession.GetNodeData(_OTSForecastSpreadMethod.HierNodeRID);
                    txtSpreadNode.Text = hnp.Text;
                    //Begin TT#53 - JSmith - Open forecast spread method and check multi level box-> application abends
                    //txtSpreadNode.Tag = hnp.Key;
                    ((MIDTag)(txtSpreadNode.Tag)).MIDTagData = hnp;
                    //End TT#53
                    PopulateFromToLevels(hnp, cboFromLevel.ComboBox, 0);//TT#7 - RBeck - Dynamic dropdowns
                    PopulateFromToLevels(hnp, cboToLevel.ComboBox, 0);//TT#7 - RBeck - Dynamic dropdowns
                }
            // Begin TT#56 - JSmith - not holding to level after save or update
            }
            // End TT#56
        }

		private void rbBasis_CheckedChanged(object sender, System.EventArgs e)
		{
			if (FormLoaded)
				ChangePending = true;

			if (rbBasis.Checked == true)
			{
				grdBasis.DisplayLayout.Bands[0].Override.AllowAddNew = AllowAddNew.Yes;
				foreach (MenuItem mi in grdBasis.ContextMenu.MenuItems)
				{
					mi.Enabled = true;

				}
				foreach (UltraGridColumn col in grdBasis.DisplayLayout.Bands[0].Columns)
				{
					col.CellActivation = Activation.AllowEdit;
					if (col.Header.Caption == "Date Range")
						col.CellActivation = Activation.NoEdit;
				}

				// BEGIN ANF - Weighting Multiple Basis
				SetEqualizeWeightingEnabled(true);
				// END ANF - Weighting Multiple Basis
			}
		}

		private void rbPlan_CheckedChanged(object sender, System.EventArgs e)
		{
			if (FormLoaded)
				ChangePending = true;

			if (rbPlan.Checked == true)
			{
				grdBasis.DisplayLayout.Bands[0].Override.AllowAddNew = AllowAddNew.No;
				foreach (MenuItem mi in grdBasis.ContextMenu.MenuItems)
				{
					mi.Enabled = false;

				}
				foreach (UltraGridColumn col in grdBasis.DisplayLayout.Bands[0].Columns)
				{
					col.CellActivation = Activation.Disabled;
				}

				// BEGIN ANF - Weighting Multiple Basis
				rbEWNo.Checked = true;
				SetEqualizeWeightingEnabled(false);
				// END ANF - Weighting Multiple Basis
			}
		}

		// BEGIN ANF - Weighting Multiple Basis
		private void SetEqualizeWeightingEnabled(bool aEqualize)
		{
			lblEqualizeWgt.Enabled = aEqualize;
			rbEWNo.Enabled = aEqualize;
			rbEWYes.Enabled = aEqualize;
		}
		// END ANF - Weighting Multiple Basis

		private void cbLocks_CheckedChanged(object sender, System.EventArgs e)
		{
			if (FormLoaded)
				ChangePending = true;
        }

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

        private void txtOverride_DragOver(object sender, DragEventArgs e)
        {
            Image_DragOver(sender, e);
        }

















	}
}

