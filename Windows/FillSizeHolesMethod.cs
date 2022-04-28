using System;
using System.Drawing;
using System.Data;
using System.Collections;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Forms;
using MIDRetail.Common;
using MIDRetail.Business;
using MIDRetail.Data;
using MIDRetail.DataCommon;
using MIDRetail.Business.Allocation;
using MIDRetail.Windows.Controls;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win;
using Infragistics.Shared;


namespace MIDRetail.Windows
{
	/// <summary>
	/// Summary description for frmFillSizeHolesMethod.
	/// </summary>
	public class frmFillSizeHolesMethod : SizeMethodsFormBase
	{

	#region Member Variables
		private int _nodeRID = -1;
//		private string _strMethodType;
//		private DataTable _dtSizes;
		private FillSizeHolesMethod _fillSizeHolesMethod;
		private const int DEFAULT_FILL_ZEROS_QUANTITY = 1;
		private bool _textChanged = false;
		private bool _priorError = false;
		private int _lastMerchIndex = -1;
        // Begin TT#41-MD -- GTaylor - UC #2
        DataTable MerchandiseDataTable3;    
        private bool _textChangedIB = false;
        private bool _priorErrorIB = false;
        private int _lastMerchIndexIB = -1;
        // End TT#41-MD -- GTaylor - UC #2
	#endregion

	#region Form Fields

		private System.Windows.Forms.TabControl tabOTSMethod;
		private System.Windows.Forms.TabPage tabMethod;
		private System.Windows.Forms.TabPage tabProperties;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.Label label5;
		private Infragistics.Win.UltraWinGrid.UltraGrid ugWorkflows;
		private System.Windows.Forms.ListBox listBox1;
		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage tabPageGeneral;
		private System.Windows.Forms.TabPage tabPageConstraints;
        private MIDComboBoxEnh cboMerchandise;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.TextBox txtAvailable;
		private System.Windows.Forms.RadioButton rbPercent;
		private System.Windows.Forms.RadioButton rbUnits;
		private System.Windows.Forms.Panel pnlGridContainer;
	#endregion
		private System.Windows.Forms.GroupBox gboFillSizesTo;
		private System.Windows.Forms.RadioButton radFillSizesTo_SizePlan;
        private System.Windows.Forms.RadioButton radFillSizesTo_Holes;
        private GroupBox grpVSWSizeConstraints;
        private MIDWindowsComboBox cboSizeConstraints;
        private CheckBox chkVSWSizeConstraintsOverride;
        private eVSWSizeConstraints _vswSizeConstraints;
        private RadioButton radFillSizesTo_SizePlanWithSizeMins;  // TT#246-MD - AGallagher - VSW Size - Add In-store Size Constraint options
		//Begin TT#1636-MD -jsobek -Pre-Pack Fill Size
        private GroupBox grpSizedPacksOnly;
        private GroupBox gbMaxPackAllocNeedTolerance;
        private CheckBox cbxStepped;
        private CheckBox cbxOverrideMaxPackAllocNeed;
        private TextBox txtMaxPackAllocNeedTolerance;
        private CheckBox cbxNoMaxStep;
        private GroupBox gbAvgPackDevTolerance;
        private CheckBox cbxOverrideAvgPackDev;
        private TextBox txtAvgPackDevTolerance; 
		//End TT#1636-MD -jsobek -Pre-Pack Fill Size 

	/// <summary>
	/// Required designer variable.
	/// </summary>
	private System.ComponentModel.Container components = null;


	#region Constructor/Dispose
		public frmFillSizeHolesMethod(SessionAddressBlock SAB, ExplorerAddressBlock aEAB) : base (SAB, aEAB, eMIDTextCode.frm_FillSizeHolesMethod, eWorkflowMethodType.Method)
		{
			try
			{
				//
				// Required for Windows Form Designer support
				//
				InitializeComponent();
				UserSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AllocationMethodsUserFillSizeHoles);
				GlobalSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AllocationMethodsGlobalFillSizeHoles);
			}
			catch(Exception)
			{
				FormLoadError = true;
				throw;
			}
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

				if (_fillSizeHolesMethod != null)
				{
					_fillSizeHolesMethod = null;
				}

//				this.ugRules.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.ugRules_MouseDown);
                //this.ugRules.AfterRowActivate -= new System.EventHandler(this.ugRules_AfterRowActivate);        // TT#498 - Unrelated; uncomment to avoid null ref on Update
//				this.ugRules.AfterRowInsert -= new Infragistics.Win.UltraWinGrid.RowEventHandler(this.ugRules_AfterRowInsert);
//				this.ugRules.AfterCellUpdate -= new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ugRules_AfterCellUpdate);
//				this.ugRules.BeforeEnterEditMode -= new System.ComponentModel.CancelEventHandler(this.ugRules_BeforeEnterEditMode);
                // Begin TT#301-MD - JSmith - Controls are not functioning properly
                //this.cboSizeGroup.SelectionChangeCommitted -= new System.EventHandler(this.cboSizeGroup_SelectionChangeCommitted);
                //this.cboStoreAttribute.SelectionChangeCommitted -= new System.EventHandler(this.cboStoreAttribute_SelectionChangeCommitted);
                // End TT#301-MD - JSmith - Controls are not functioning properly
                // Begin TT#301-MD - JSmith - Controls are not functioning properly
                // begin TT#2155 - JEllis - Fill Size Holes Null Reference
                this.cbxUseDefaultcurve.CheckedChanged -= new System.EventHandler(this.cbxUseDefaultCurve_CheckChanged);
                this.txtAvgPackDevTolerance.TextChanged -= new System.EventHandler(this.txtAvgPackDevTolerance_TextChanged); //TT#1636-MD -jsobek -Pre-Pack Fill Size
                this.txtMaxPackAllocNeedTolerance.TextChanged -= new System.EventHandler(this.txtMaxPackAllocNeedTolerance_TextChanged); //TT#1636-MD -jsobek -Pre-Pack Fill Size
                cbxNoMaxStep.CheckedChanged -= new EventHandler(cbxNoMaxStep_CheckedChanged); //TT#1636-MD -jsobek -Pre-Pack Fill Size
                cbxStepped.CheckedChanged -= new EventHandler(cbxStepped_CheckedChanged); //TT#1636-MD -jsobek -Pre-Pack Fill Size
                // Begin TT#301-MD - JSmith - Controls are not functioning properly
                //this.cboHierarchyLevel.SelectionChangeCommitted -= new System.EventHandler(this.cboHierarchyLevel_SelectionChangeCommitted);
                //this.cboHeaderChar.SelectionChangeCommitted -= new System.EventHandler(this.cboHeaderChar_SelectionChangeCommitted);
                //this.cboNameExtension.SelectionChangeCommitted -= new System.EventHandler(this.cboNameExtension_SelectionChangeCommitted);
                //this.cboHierarchyLevel.SelectionChangeCommitted -= new System.EventHandler(this.cboHierarchyLevel_SelectionChangeCommitted);
                //this.cboHeaderChar.SelectionChangeCommitted -= new System.EventHandler(this.cboHeaderChar_SelectionChangeCommitted);
                //this.cboNameExtension.SelectionChangeCommitted -= new System.EventHandler(this.cboNameExtension_SelectionChangeCommitted);
                // End TT#301-MD - JSmith - Controls are not functioning properly
                // end TT#2155 - JEllis - Fill Size Holes Null Reference
                // Begin TT#301-MD - JSmith - Controls are not functioning properly
                //this.cboStoreAttribute.DragDrop -= new System.Windows.Forms.DragEventHandler(this.cboStoreAttribute_DragDrop);
                //this.cboStoreAttribute.DragEnter -= new System.Windows.Forms.DragEventHandler(this.cboStoreAttribute_DragEnter);
                //this.cboSizeCurve.SelectionChangeCommitted -= new System.EventHandler(this.cboSizeCurve_SelectionChangeCommitted);
                //this.cboSizeCurve.SelectionChangeCommitted -= new System.EventHandler(this.cboSizeCurve_SelectionChangeCommitted);
                // End TT#301-MD - JSmith - Controls are not functioning properly
//				this.radGlobal.CheckedChanged -= new System.EventHandler(this.radGlobal_CheckedChanged);
//				this.txtDesc.TextChanged -= new System.EventHandler(this.txtDesc_TextChanged);
//				this.txtName.TextChanged -= new System.EventHandler(this.txtName_TextChanged);
				this.rbPercent.CheckedChanged -= new System.EventHandler(this.rbPercent_CheckedChanged);
				this.rbUnits.CheckedChanged -= new System.EventHandler(this.rbUnits_CheckedChanged);
				this.cboMerchandise.KeyDown -= new System.Windows.Forms.KeyEventHandler(this.cboMerchandise_KeyDown);
				this.cboMerchandise.Validating -= new System.ComponentModel.CancelEventHandler(this.cboMerchandise_Validating);
				this.cboMerchandise.Validated -= new System.EventHandler(this.cboMerchandise_Validated);
				this.cboMerchandise.DragDrop -= new System.Windows.Forms.DragEventHandler(this.cboMerchandise_DragDrop);
                // Begin TT#301-MD - JSmith - Controls are not functioning properly
                //this.cboMerchandise.SelectionChangeCommitted -= new System.EventHandler(this.cboMerchandise_SelectionChangeCommitted);
                this.cboMerchandise.SelectionChangeCommitted -= new System.EventHandler(this.cboMerchandise_SelectionChangeCommitted);
                //Begin TT#316 - MD - DOConnell - Replace all Windows Combobox controls with new enhanced control
                this.cboMerchandise.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler(cboMerchandise_MIDComboBoxPropertiesChangedEvent);
                //End TT#316 - MD - DOConnell - Replace all Windows Combobox controls with new enhanced control

                // End TT#301-MD - JSmith - Controls are not functioning properly
                this.cboMerchandise.DragEnter -= new System.Windows.Forms.DragEventHandler(this.cboMerchandise_DragEnter);
                // Begin TT#296-MD - JSmith - Methods and Workflows can be dragged and dropped into Inventory Basis drop down.
                this.cboMerchandise.DragOver -= new System.Windows.Forms.DragEventHandler(this.cboMerchandise_DragOver);
                // End TT#296-MD - JSmith - Methods and Workflows can be dragged and dropped into Inventory Basis drop down.
				this.txtAvailable.Validating -= new System.ComponentModel.CancelEventHandler(this.txtAvailable_Validating);
				this.txtAvailable.TextChanged -= new System.EventHandler(this.txtAvailable_TextChanged);
				// Begin MID Track 4858 - JSmith - Security changes
//				this.btnSave.Click -= new System.EventHandler(this.btnSave_Click);
//				this.btnClose.Click -= new System.EventHandler(this.btnClose_Click);
//				this.btnProcess.Click -= new System.EventHandler(this.btnProcess_Click);
				// End MID Track 4858
				// BEGIN MID Track #4921 - JSmith - A&F 666 - Size Modification
				this.radFillSizesTo_Holes.CheckedChanged -= new System.EventHandler(this.radFillSizesTo_Holes_CheckedChanged);
				this.radFillSizesTo_SizePlan.CheckedChanged -= new System.EventHandler(this.radFillSizesTo_SizePlan_CheckedChanged);
				// End MID Track #4921
                // BEGIN TT#41-MD - GTaylor - UC#2
                // Begin TT#301-MD - JSmith - Controls are not functioning properly
                //this.cboInventoryBasis.SelectionChangeCommitted -= new EventHandler(cboInventoryBasis_SelectionChangeCommitted);
                //this.cboInventoryBasis.SelectionChangeCommitted -= new EventHandler(cboInventoryBasis_SelectionChangeCommitted);
                //this.cboInventoryBasis.KeyDown -= new KeyEventHandler(cboInventoryBasis_KeyDown);
                //this.cboInventoryBasis.Validating -= new CancelEventHandler(cboInventoryBasis_Validating);
                //this.cboInventoryBasis.Validated -= new EventHandler(cboInventoryBasis_Validated);
                //this.cboInventoryBasis.DragDrop -= new System.Windows.Forms.DragEventHandler(this.cboInventoryBasis_DragDrop);
                //this.cboInventoryBasis.DragEnter -= new System.Windows.Forms.DragEventHandler(this.cboInventoryBasis_DragEnter);
                //this.cboInventoryBasis.DragOver -= new System.Windows.Forms.DragEventHandler(this.cboInventoryBasis_DragOver);
                // End TT#301-MD - JSmith - Controls are not functioning properly
                // END TT#41-MD - GTaylor - UC#2
			}
			base.Dispose( disposing );
		}


	#endregion

	#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
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
            this.tabOTSMethod = new System.Windows.Forms.TabControl();
            this.tabMethod = new System.Windows.Forms.TabPage();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPageGeneral = new System.Windows.Forms.TabPage();
            this.grpSizedPacksOnly = new System.Windows.Forms.GroupBox();
            this.gbMaxPackAllocNeedTolerance = new System.Windows.Forms.GroupBox();
            this.cbxStepped = new System.Windows.Forms.CheckBox();
            this.cbxOverrideMaxPackAllocNeed = new System.Windows.Forms.CheckBox();
            this.txtMaxPackAllocNeedTolerance = new System.Windows.Forms.TextBox();
            this.cbxNoMaxStep = new System.Windows.Forms.CheckBox();
            this.gbAvgPackDevTolerance = new System.Windows.Forms.GroupBox();
            this.cbxOverrideAvgPackDev = new System.Windows.Forms.CheckBox();
            this.txtAvgPackDevTolerance = new System.Windows.Forms.TextBox();
            this.grpVSWSizeConstraints = new System.Windows.Forms.GroupBox();
            this.cboSizeConstraints = new MIDRetail.Windows.Controls.MIDWindowsComboBox();
            this.chkVSWSizeConstraintsOverride = new System.Windows.Forms.CheckBox();
            this.cboMerchandise = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this.gboFillSizesTo = new System.Windows.Forms.GroupBox();
            this.radFillSizesTo_SizePlanWithSizeMins = new System.Windows.Forms.RadioButton();
            this.radFillSizesTo_SizePlan = new System.Windows.Forms.RadioButton();
            this.radFillSizesTo_Holes = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rbUnits = new System.Windows.Forms.RadioButton();
            this.rbPercent = new System.Windows.Forms.RadioButton();
            this.label7 = new System.Windows.Forms.Label();
            this.txtAvailable = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.tabPageConstraints = new System.Windows.Forms.TabPage();
            this.pnlGridContainer = new System.Windows.Forms.Panel();
            this.tabProperties = new System.Windows.Forms.TabPage();
            this.ugWorkflows = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
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
            this.tabOTSMethod.SuspendLayout();
            this.tabMethod.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPageGeneral.SuspendLayout();
            this.grpSizedPacksOnly.SuspendLayout();
            this.gbMaxPackAllocNeedTolerance.SuspendLayout();
            this.gbAvgPackDevTolerance.SuspendLayout();
            this.grpVSWSizeConstraints.SuspendLayout();
            this.gboFillSizesTo.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabPageConstraints.SuspendLayout();
            this.pnlGridContainer.SuspendLayout();
            this.tabProperties.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ugWorkflows)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // lblStoreAttribute
            // 
            this.lblStoreAttribute.Location = new System.Drawing.Point(120, 18);
            // 
            // cboStoreAttribute
            // 
            this.cboStoreAttribute.Location = new System.Drawing.Point(208, 16);
            this.cboStoreAttribute.Size = new System.Drawing.Size(224, 21);
            // 
            // cboFilter
            // 
            this.cboFilter.Location = new System.Drawing.Point(125, 12);
            this.cboFilter.Size = new System.Drawing.Size(296, 21);
            // 
            // lblFilter
            // 
            this.lblFilter.Location = new System.Drawing.Point(12, 12);
            // 
            // cboConstraints
            // 
            this.cboConstraints.Location = new System.Drawing.Point(48, 54);
            // 
            // ugRules
            // 
            this.ugRules.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
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
            this.ugRules.Location = new System.Drawing.Point(6, 9);
            this.ugRules.Size = new System.Drawing.Size(650, 328);
            // 
            // cbExpandAll
            // 
            this.cbExpandAll.Location = new System.Drawing.Point(17, 28);
            this.cbExpandAll.Size = new System.Drawing.Size(80, 18);
            this.cbExpandAll.CheckedChanged += new System.EventHandler(this.cbExpandAll_CheckedChanged);
            // 
            // gbSizeCurve
            // 
            this.gbSizeCurve.Location = new System.Drawing.Point(14, 180);
            // 
            // gbSizeConstraints
            // 
            this.gbSizeConstraints.Location = new System.Drawing.Point(363, 180);
            this.gbSizeConstraints.Size = new System.Drawing.Size(289, 200);
            // 
            // gbGenericConstraint
            // 
            this.gbGenericConstraint.Location = new System.Drawing.Point(15, 81);
            // 
            // picBoxConstraint
            // 
            this.picBoxConstraint.Location = new System.Drawing.Point(19, 54);
            // 
            // gbSizeGroup
            // 
            this.gbSizeGroup.Location = new System.Drawing.Point(14, 108);
            // 
            // gbSizeAlternate
            // 
            this.gbSizeAlternate.Location = new System.Drawing.Point(358, 108);
            // 
            // gbxNormalizeSizeCurves
            // 
            this.gbxNormalizeSizeCurves.Location = new System.Drawing.Point(431, 5);
            // 
            // cbxUseDefaultcurve
            // 
            this.cbxUseDefaultcurve.CheckedChanged += new System.EventHandler(this.cbxUseDefaultCurve_CheckChanged);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(635, 618);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(549, 618);
            // 
            // btnProcess
            // 
            this.btnProcess.Location = new System.Drawing.Point(10, 618);
            // 
            // utmMain
            // 
            this.utmMain.MenuSettings.ForceSerialization = true;
            this.utmMain.ToolbarSettings.ForceSerialization = true;
            // 
            // tabOTSMethod
            // 
            this.tabOTSMethod.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabOTSMethod.Controls.Add(this.tabMethod);
            this.tabOTSMethod.Controls.Add(this.tabProperties);
            this.tabOTSMethod.Location = new System.Drawing.Point(8, 48);
            this.tabOTSMethod.Name = "tabOTSMethod";
            this.tabOTSMethod.SelectedIndex = 0;
            this.tabOTSMethod.Size = new System.Drawing.Size(704, 559);
            this.tabOTSMethod.TabIndex = 5;
            this.tabOTSMethod.SelectedIndexChanged += new System.EventHandler(this.tabOTSMethod_SelectedIndexChanged);
            // 
            // tabMethod
            // 
            this.tabMethod.Controls.Add(this.tabControl1);
            this.tabMethod.Location = new System.Drawing.Point(4, 22);
            this.tabMethod.Name = "tabMethod";
            this.tabMethod.Size = new System.Drawing.Size(696, 533);
            this.tabMethod.TabIndex = 1;
            this.tabMethod.Text = "Method";
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPageGeneral);
            this.tabControl1.Controls.Add(this.tabPageConstraints);
            this.tabControl1.Location = new System.Drawing.Point(8, 8);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(680, 519);
            this.tabControl1.TabIndex = 20;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // tabPageGeneral
            // 
            this.tabPageGeneral.Controls.Add(this.grpSizedPacksOnly);
            this.tabPageGeneral.Controls.Add(this.grpVSWSizeConstraints);
            this.tabPageGeneral.Controls.Add(this.cboMerchandise);
            this.tabPageGeneral.Controls.Add(this.gboFillSizesTo);
            this.tabPageGeneral.Controls.Add(this.gbSizeConstraints);
            this.tabPageGeneral.Controls.Add(this.gbSizeCurve);
            this.tabPageGeneral.Controls.Add(this.gbSizeAlternate);
            this.tabPageGeneral.Controls.Add(this.gbSizeGroup);
            this.tabPageGeneral.Controls.Add(this.groupBox1);
            this.tabPageGeneral.Controls.Add(this.label7);
            this.tabPageGeneral.Controls.Add(this.txtAvailable);
            this.tabPageGeneral.Controls.Add(this.label5);
            this.tabPageGeneral.Controls.Add(this.lblFilter);
            this.tabPageGeneral.Controls.Add(this.cboFilter);
            this.tabPageGeneral.Controls.Add(this.gbxNormalizeSizeCurves);
            this.tabPageGeneral.Location = new System.Drawing.Point(4, 22);
            this.tabPageGeneral.Name = "tabPageGeneral";
            this.tabPageGeneral.Size = new System.Drawing.Size(672, 493);
            this.tabPageGeneral.TabIndex = 0;
            this.tabPageGeneral.Text = "General";
            this.tabPageGeneral.Controls.SetChildIndex(this.gbxNormalizeSizeCurves, 0);
            this.tabPageGeneral.Controls.SetChildIndex(this.cboFilter, 0);
            this.tabPageGeneral.Controls.SetChildIndex(this.lblFilter, 0);
            this.tabPageGeneral.Controls.SetChildIndex(this.label5, 0);
            this.tabPageGeneral.Controls.SetChildIndex(this.txtAvailable, 0);
            this.tabPageGeneral.Controls.SetChildIndex(this.label7, 0);
            this.tabPageGeneral.Controls.SetChildIndex(this.groupBox1, 0);
            this.tabPageGeneral.Controls.SetChildIndex(this.gbSizeGroup, 0);
            this.tabPageGeneral.Controls.SetChildIndex(this.gbSizeAlternate, 0);
            this.tabPageGeneral.Controls.SetChildIndex(this.gbSizeCurve, 0);
            this.tabPageGeneral.Controls.SetChildIndex(this.gbSizeConstraints, 0);
            this.tabPageGeneral.Controls.SetChildIndex(this.gboFillSizesTo, 0);
            this.tabPageGeneral.Controls.SetChildIndex(this.cboMerchandise, 0);
            this.tabPageGeneral.Controls.SetChildIndex(this.grpVSWSizeConstraints, 0);
            this.tabPageGeneral.Controls.SetChildIndex(this.grpSizedPacksOnly, 0);
            // 
            // grpSizedPacksOnly
            // 
            this.grpSizedPacksOnly.Controls.Add(this.gbMaxPackAllocNeedTolerance);
            this.grpSizedPacksOnly.Controls.Add(this.gbAvgPackDevTolerance);
            this.grpSizedPacksOnly.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpSizedPacksOnly.Location = new System.Drawing.Point(14, 400);
            this.grpSizedPacksOnly.Name = "grpSizedPacksOnly";
            this.grpSizedPacksOnly.Size = new System.Drawing.Size(657, 85);
            this.grpSizedPacksOnly.TabIndex = 61;
            this.grpSizedPacksOnly.TabStop = false;
            this.grpSizedPacksOnly.Text = "Size Detail Packs Only";
            // 
            // gbMaxPackAllocNeedTolerance
            // 
            this.gbMaxPackAllocNeedTolerance.Controls.Add(this.cbxNoMaxStep);
            this.gbMaxPackAllocNeedTolerance.Controls.Add(this.cbxStepped);
            this.gbMaxPackAllocNeedTolerance.Controls.Add(this.cbxOverrideMaxPackAllocNeed);
            this.gbMaxPackAllocNeedTolerance.Controls.Add(this.txtMaxPackAllocNeedTolerance);
            this.gbMaxPackAllocNeedTolerance.Location = new System.Drawing.Point(304, 19);
            this.gbMaxPackAllocNeedTolerance.Name = "gbMaxPackAllocNeedTolerance";
            this.gbMaxPackAllocNeedTolerance.Size = new System.Drawing.Size(347, 53);
            this.gbMaxPackAllocNeedTolerance.TabIndex = 30;
            this.gbMaxPackAllocNeedTolerance.TabStop = false;
            this.gbMaxPackAllocNeedTolerance.Text = "Maximum Pack Allocation Need Tolerance";
            // 
            // cbxStepped
            // 
            this.cbxStepped.AutoSize = true;
            this.cbxStepped.Location = new System.Drawing.Point(185, 24);
            this.cbxStepped.Name = "cbxStepped";
            this.cbxStepped.Size = new System.Drawing.Size(66, 17);
            this.cbxStepped.TabIndex = 34;
            this.cbxStepped.Text = "Stepped";
            this.cbxStepped.UseVisualStyleBackColor = true;
            this.cbxStepped.CheckedChanged += new System.EventHandler(this.cbxStepped_CheckedChanged);
            // 
            // cbxOverrideMaxPackAllocNeed
            // 
            this.cbxOverrideMaxPackAllocNeed.AutoSize = true;
            this.cbxOverrideMaxPackAllocNeed.Location = new System.Drawing.Point(6, 24);
            this.cbxOverrideMaxPackAllocNeed.Name = "cbxOverrideMaxPackAllocNeed";
            this.cbxOverrideMaxPackAllocNeed.Size = new System.Drawing.Size(103, 17);
            this.cbxOverrideMaxPackAllocNeed.TabIndex = 31;
            this.cbxOverrideMaxPackAllocNeed.Text = "Override Default";
            this.cbxOverrideMaxPackAllocNeed.UseVisualStyleBackColor = true;
            this.cbxOverrideMaxPackAllocNeed.CheckedChanged += new System.EventHandler(this.cbxOverrideMaxPackAllocNeed_CheckedChanged);
            // 
            // txtMaxPackAllocNeedTolerance
            // 
            this.txtMaxPackAllocNeedTolerance.Location = new System.Drawing.Point(115, 21);
            this.txtMaxPackAllocNeedTolerance.Name = "txtMaxPackAllocNeedTolerance";
            this.txtMaxPackAllocNeedTolerance.Size = new System.Drawing.Size(64, 20);
            this.txtMaxPackAllocNeedTolerance.TabIndex = 32;
            this.txtMaxPackAllocNeedTolerance.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtMaxPackAllocNeedTolerance.TextChanged += new System.EventHandler(this.txtMaxPackAllocNeedTolerance_TextChanged);
            // 
            // cbxNoMaxStep
            // 
            this.cbxNoMaxStep.AutoSize = true;
            this.cbxNoMaxStep.Location = new System.Drawing.Point(253, 24);
            this.cbxNoMaxStep.Name = "cbxNoMaxStep";
            this.cbxNoMaxStep.Size = new System.Drawing.Size(88, 17);
            this.cbxNoMaxStep.TabIndex = 33;
            this.cbxNoMaxStep.Text = "No-Max Step";
            this.cbxNoMaxStep.UseVisualStyleBackColor = true;
            this.cbxNoMaxStep.CheckedChanged += new System.EventHandler(this.cbxNoMaxStep_CheckedChanged);
            // 
            // gbAvgPackDevTolerance
            // 
            this.gbAvgPackDevTolerance.Controls.Add(this.cbxOverrideAvgPackDev);
            this.gbAvgPackDevTolerance.Controls.Add(this.txtAvgPackDevTolerance);
            this.gbAvgPackDevTolerance.Location = new System.Drawing.Point(6, 19);
            this.gbAvgPackDevTolerance.Name = "gbAvgPackDevTolerance";
            this.gbAvgPackDevTolerance.Size = new System.Drawing.Size(288, 53);
            this.gbAvgPackDevTolerance.TabIndex = 27;
            this.gbAvgPackDevTolerance.TabStop = false;
            this.gbAvgPackDevTolerance.Text = "Average Pack Deviation Tolerance";
            // 
            // cbxOverrideAvgPackDev
            // 
            this.cbxOverrideAvgPackDev.AutoSize = true;
            this.cbxOverrideAvgPackDev.Location = new System.Drawing.Point(11, 22);
            this.cbxOverrideAvgPackDev.Name = "cbxOverrideAvgPackDev";
            this.cbxOverrideAvgPackDev.Size = new System.Drawing.Size(103, 17);
            this.cbxOverrideAvgPackDev.TabIndex = 28;
            this.cbxOverrideAvgPackDev.Text = "Override Default";
            this.cbxOverrideAvgPackDev.UseVisualStyleBackColor = true;
            this.cbxOverrideAvgPackDev.CheckedChanged += new System.EventHandler(this.cbxOverrideAvgPackDev_CheckedChanged);
            // 
            // txtAvgPackDevTolerance
            // 
            this.txtAvgPackDevTolerance.Location = new System.Drawing.Point(120, 20);
            this.txtAvgPackDevTolerance.Name = "txtAvgPackDevTolerance";
            this.txtAvgPackDevTolerance.Size = new System.Drawing.Size(64, 20);
            this.txtAvgPackDevTolerance.TabIndex = 29;
            this.txtAvgPackDevTolerance.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtAvgPackDevTolerance.TextChanged += new System.EventHandler(this.txtAvgPackDevTolerance_TextChanged);
            // 
            // grpVSWSizeConstraints
            // 
            this.grpVSWSizeConstraints.Controls.Add(this.cboSizeConstraints);
            this.grpVSWSizeConstraints.Controls.Add(this.chkVSWSizeConstraintsOverride);
            this.grpVSWSizeConstraints.Location = new System.Drawing.Point(14, 361);
            this.grpVSWSizeConstraints.Name = "grpVSWSizeConstraints";
            this.grpVSWSizeConstraints.Size = new System.Drawing.Size(294, 33);
            this.grpVSWSizeConstraints.TabIndex = 60;
            this.grpVSWSizeConstraints.TabStop = false;
            this.grpVSWSizeConstraints.Text = "VSW Size Constraints:";
            // 
            // cboSizeConstraints
            // 
            this.cboSizeConstraints.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboSizeConstraints.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboSizeConstraints.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSizeConstraints.FormattingEnabled = true;
            this.cboSizeConstraints.Location = new System.Drawing.Point(113, 10);
            this.cboSizeConstraints.Name = "cboSizeConstraints";
            this.cboSizeConstraints.Size = new System.Drawing.Size(151, 21);
            this.cboSizeConstraints.TabIndex = 1;
            // 
            // chkVSWSizeConstraintsOverride
            // 
            this.chkVSWSizeConstraintsOverride.AutoSize = true;
            this.chkVSWSizeConstraintsOverride.Location = new System.Drawing.Point(7, 14);
            this.chkVSWSizeConstraintsOverride.Name = "chkVSWSizeConstraintsOverride";
            this.chkVSWSizeConstraintsOverride.Size = new System.Drawing.Size(106, 17);
            this.chkVSWSizeConstraintsOverride.TabIndex = 0;
            this.chkVSWSizeConstraintsOverride.Text = "Override Default:";
            this.chkVSWSizeConstraintsOverride.UseVisualStyleBackColor = true;
            this.chkVSWSizeConstraintsOverride.CheckedChanged += new System.EventHandler(this.chkVSWSizeConstraintsOverride_CheckedChanged);
            // 
            // cboMerchandise
            // 
            this.cboMerchandise.AllowDrop = true;
            this.cboMerchandise.AutoAdjust = true;
            this.cboMerchandise.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.cboMerchandise.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboMerchandise.DataSource = null;
            this.cboMerchandise.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown;
            this.cboMerchandise.DropDownWidth = 217;
            this.cboMerchandise.FormattingEnabled = false;
            this.cboMerchandise.IgnoreFocusLost = true;
            this.cboMerchandise.ItemHeight = 13;
            this.cboMerchandise.Location = new System.Drawing.Point(125, 74);
            this.cboMerchandise.Margin = new System.Windows.Forms.Padding(0);
            this.cboMerchandise.MaxDropDownItems = 25;
            this.cboMerchandise.Name = "cboMerchandise";
            this.cboMerchandise.SetToolTip = "";
            this.cboMerchandise.Size = new System.Drawing.Size(217, 21);
            this.cboMerchandise.TabIndex = 20;
            this.cboMerchandise.Tag = null;
            this.cboMerchandise.MIDComboBoxPropertiesChangedEvent += new MIDRetail.Windows.Controls.MIDComboBoxPropertiesChangedEventHandler(this.cboMerchandise_MIDComboBoxPropertiesChangedEvent);
            this.cboMerchandise.SelectionChangeCommitted += new System.EventHandler(this.cboMerchandise_SelectionChangeCommitted);
            this.cboMerchandise.DragDrop += new System.Windows.Forms.DragEventHandler(this.cboMerchandise_DragDrop);
            this.cboMerchandise.DragEnter += new System.Windows.Forms.DragEventHandler(this.cboMerchandise_DragEnter);
            this.cboMerchandise.DragOver += new System.Windows.Forms.DragEventHandler(this.cboMerchandise_DragOver);
            this.cboMerchandise.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cboMerchandise_KeyDown);
            this.cboMerchandise.Validating += new System.ComponentModel.CancelEventHandler(this.cboMerchandise_Validating);
            this.cboMerchandise.Validated += new System.EventHandler(this.cboMerchandise_Validated);
            // 
            // gboFillSizesTo
            // 
            this.gboFillSizesTo.Controls.Add(this.radFillSizesTo_SizePlanWithSizeMins);
            this.gboFillSizesTo.Controls.Add(this.radFillSizesTo_SizePlan);
            this.gboFillSizesTo.Controls.Add(this.radFillSizesTo_Holes);
            this.gboFillSizesTo.Location = new System.Drawing.Point(386, 59);
            this.gboFillSizesTo.Name = "gboFillSizesTo";
            this.gboFillSizesTo.Size = new System.Drawing.Size(266, 36);
            this.gboFillSizesTo.TabIndex = 57;
            this.gboFillSizesTo.TabStop = false;
            this.gboFillSizesTo.Text = "Fill To:";
            // 
            // radFillSizesTo_SizePlanWithSizeMins
            // 
            this.radFillSizesTo_SizePlanWithSizeMins.Location = new System.Drawing.Point(130, 15);
            this.radFillSizesTo_SizePlanWithSizeMins.Name = "radFillSizesTo_SizePlanWithSizeMins";
            this.radFillSizesTo_SizePlanWithSizeMins.Size = new System.Drawing.Size(132, 16);
            this.radFillSizesTo_SizePlanWithSizeMins.TabIndex = 3;
            this.radFillSizesTo_SizePlanWithSizeMins.Text = "Size Plan + Size Mins";
            this.radFillSizesTo_SizePlanWithSizeMins.CheckedChanged += new System.EventHandler(this.radFillSizesTo_SizePlanWithSizeMin_CheckedChanged);
            // 
            // radFillSizesTo_SizePlan
            // 
            this.radFillSizesTo_SizePlan.Location = new System.Drawing.Point(60, 15);
            this.radFillSizesTo_SizePlan.Name = "radFillSizesTo_SizePlan";
            this.radFillSizesTo_SizePlan.Size = new System.Drawing.Size(72, 16);
            this.radFillSizesTo_SizePlan.TabIndex = 2;
            this.radFillSizesTo_SizePlan.Text = "Size Plan";
            this.radFillSizesTo_SizePlan.CheckedChanged += new System.EventHandler(this.radFillSizesTo_SizePlan_CheckedChanged);
            // 
            // radFillSizesTo_Holes
            // 
            this.radFillSizesTo_Holes.Location = new System.Drawing.Point(6, 14);
            this.radFillSizesTo_Holes.Name = "radFillSizesTo_Holes";
            this.radFillSizesTo_Holes.Size = new System.Drawing.Size(56, 16);
            this.radFillSizesTo_Holes.TabIndex = 1;
            this.radFillSizesTo_Holes.Text = "Holes";
            this.radFillSizesTo_Holes.CheckedChanged += new System.EventHandler(this.radFillSizesTo_Holes_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rbUnits);
            this.groupBox1.Controls.Add(this.rbPercent);
            this.groupBox1.Location = new System.Drawing.Point(230, 34);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(133, 32);
            this.groupBox1.TabIndex = 21;
            this.groupBox1.TabStop = false;
            // 
            // rbUnits
            // 
            this.rbUnits.Location = new System.Drawing.Point(74, 8);
            this.rbUnits.Name = "rbUnits";
            this.rbUnits.Size = new System.Drawing.Size(56, 20);
            this.rbUnits.TabIndex = 2;
            this.rbUnits.Text = "Units";
            this.rbUnits.CheckedChanged += new System.EventHandler(this.rbUnits_CheckedChanged);
            // 
            // rbPercent
            // 
            this.rbPercent.Location = new System.Drawing.Point(8, 8);
            this.rbPercent.Name = "rbPercent";
            this.rbPercent.Size = new System.Drawing.Size(72, 20);
            this.rbPercent.TabIndex = 5;
            this.rbPercent.Text = "Percent";
            this.rbPercent.CheckedChanged += new System.EventHandler(this.rbPercent_CheckedChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(12, 44);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(50, 13);
            this.label7.TabIndex = 6;
            this.label7.Text = "Available";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtAvailable
            // 
            this.txtAvailable.Location = new System.Drawing.Point(125, 44);
            this.txtAvailable.Name = "txtAvailable";
            this.txtAvailable.Size = new System.Drawing.Size(104, 20);
            this.txtAvailable.TabIndex = 1;
            this.txtAvailable.TextChanged += new System.EventHandler(this.txtAvailable_TextChanged);
            this.txtAvailable.Validating += new System.ComponentModel.CancelEventHandler(this.txtAvailable_Validating);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 76);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(96, 13);
            this.label5.TabIndex = 12;
            this.label5.Text = "Merchandise Basis";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tabPageConstraints
            // 
            this.tabPageConstraints.Controls.Add(this.cbExpandAll);
            this.tabPageConstraints.Controls.Add(this.cboStoreAttribute);
            this.tabPageConstraints.Controls.Add(this.lblStoreAttribute);
            this.tabPageConstraints.Controls.Add(this.pnlGridContainer);
            this.tabPageConstraints.Location = new System.Drawing.Point(4, 22);
            this.tabPageConstraints.Name = "tabPageConstraints";
            this.tabPageConstraints.Size = new System.Drawing.Size(672, 493);
            this.tabPageConstraints.TabIndex = 1;
            this.tabPageConstraints.Text = "Rules";
            this.tabPageConstraints.Controls.SetChildIndex(this.pnlGridContainer, 0);
            this.tabPageConstraints.Controls.SetChildIndex(this.lblStoreAttribute, 0);
            this.tabPageConstraints.Controls.SetChildIndex(this.cboStoreAttribute, 0);
            this.tabPageConstraints.Controls.SetChildIndex(this.cbExpandAll, 0);
            // 
            // pnlGridContainer
            // 
            this.pnlGridContainer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlGridContainer.Controls.Add(this.ugRules);
            this.pnlGridContainer.Location = new System.Drawing.Point(8, 40);
            this.pnlGridContainer.Name = "pnlGridContainer";
            this.pnlGridContainer.Size = new System.Drawing.Size(656, 447);
            this.pnlGridContainer.TabIndex = 25;
            // 
            // tabProperties
            // 
            this.tabProperties.Controls.Add(this.ugWorkflows);
            this.tabProperties.Location = new System.Drawing.Point(4, 22);
            this.tabProperties.Name = "tabProperties";
            this.tabProperties.Size = new System.Drawing.Size(696, 533);
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
            this.ugWorkflows.Location = new System.Drawing.Point(16, 16);
            this.ugWorkflows.Name = "ugWorkflows";
            this.ugWorkflows.Size = new System.Drawing.Size(666, 503);
            this.ugWorkflows.TabIndex = 0;
            // 
            // listBox1
            // 
            this.listBox1.Location = new System.Drawing.Point(0, 0);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(120, 95);
            this.listBox1.TabIndex = 0;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(100, 50);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // frmFillSizeHolesMethod
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(720, 652);
            this.Controls.Add(this.tabOTSMethod);
            this.Name = "frmFillSizeHolesMethod";
            this.Text = "Fill Size Holes Method";
            this.Load += new System.EventHandler(this.frmFillSizeHolesMethod_Load);
            this.Controls.SetChildIndex(this.btnSave, 0);
            this.Controls.SetChildIndex(this.btnClose, 0);
            this.Controls.SetChildIndex(this.btnProcess, 0);
            this.Controls.SetChildIndex(this.pnlGlobalUser, 0);
            this.Controls.SetChildIndex(this.tabOTSMethod, 0);
            this.Controls.SetChildIndex(this.lblName, 0);
            this.Controls.SetChildIndex(this.txtName, 0);
            this.Controls.SetChildIndex(this.txtDesc, 0);
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
            this.tabOTSMethod.ResumeLayout(false);
            this.tabMethod.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPageGeneral.ResumeLayout(false);
            this.tabPageGeneral.PerformLayout();
            this.grpSizedPacksOnly.ResumeLayout(false);
            this.gbMaxPackAllocNeedTolerance.ResumeLayout(false);
            this.gbMaxPackAllocNeedTolerance.PerformLayout();
            this.gbAvgPackDevTolerance.ResumeLayout(false);
            this.gbAvgPackDevTolerance.PerformLayout();
            this.grpVSWSizeConstraints.ResumeLayout(false);
            this.grpVSWSizeConstraints.PerformLayout();
            this.gboFillSizesTo.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.tabPageConstraints.ResumeLayout(false);
            this.tabPageConstraints.PerformLayout();
            this.pnlGridContainer.ResumeLayout(false);
            this.tabProperties.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ugWorkflows)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
	#endregion

	#region Control Event Handlers

		// Begin MID Track 4858 - JSmith - Security changes
//		private void btnClose_Click(object sender, System.EventArgs e)
//		{
//			try
//			{
//				Cancel_Click();
//			}
//			catch
//			{
//				MessageBox.Show("Error in btnClose_Click");
//			}
//		}
//
//
//		private void btnSave_Click(object sender, System.EventArgs e)
//		{
//			try
//			{
//				Save_Click(true);
//			}
//			catch( Exception exception )
//			{
//				HandleException(exception);
//			}		
//		}

        //  BEGIN TT#41-MD - GTaylor - UC#2

        // Begin TT#301-MD - JSmith - Controls are not functioning properly
        //private void cboInventoryBasis_DragOver(object sender, DragEventArgs e)
        //{
        //    // Begin TT#296-MD - JSmith - Methods and Workflows can be dragged and dropped into Inventory Basis drop down.
        //    //Image_DragOver(sender, e);
        //    Merchandise_DragOver(sender, e);
        //    // End TT#296-MD - JSmith - Methods and Workflows can be dragged and dropped into Inventory Basis drop down.
        //}
        //private void cboInventoryBasis_DragEnter(object sender, DragEventArgs e)
        //{
        //    // Begin TT#296-MD - JSmith - Methods and Workflows can be dragged and dropped into Inventory Basis drop down.
        //    Merchandise_DragEnter(sender, e);
        //    //try
        //    //{
        //    //    ObjectDragEnter(e);
        //    //    Image_DragEnter(sender, e);
        //    //}
        //    //catch (Exception ex)
        //    //{
        //    //    HandleException(ex);
        //    //}
        //    // End TT#296-MD - JSmith - Methods and Workflows can be dragged and dropped into Inventory Basis drop down.
        //}
        // ENd TT#301-MD - JSmith - Controls are not functioning properly

        private void AddNodeToMerchandiseCombo3(HierarchyNodeProfile hnp)
        {
            try
            {
                DataRow myDataRow;
                bool nodeFound = false;
                int nodeRID = Include.NoRID;
                int levIndex;
                for (levIndex = 0;
                    levIndex < MerchandiseDataTable3.Rows.Count; levIndex++)
                {
                    myDataRow = MerchandiseDataTable3.Rows[levIndex];
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
                    myDataRow = MerchandiseDataTable3.NewRow();
                    myDataRow["seqno"] = MerchandiseDataTable3.Rows.Count;
                    myDataRow["leveltypename"] = eMerchandiseType.Node;
                    myDataRow["text"] = hnp.Text;
                    myDataRow["key"] = hnp.Key;
                    MerchandiseDataTable3.Rows.Add(myDataRow);

                    cboInventoryBasis.SelectedIndex = MerchandiseDataTable3.Rows.Count - 1;
                    //this.cboInventoryBasis_SelectionChangeCommitted(source, new EventArgs()); // TT#301-MD - JSmith - Controls are not functioning properly
                }
                else
                {
                    cboInventoryBasis.SelectedIndex = levIndex;
                    //this.cboInventoryBasis_SelectionChangeCommitted(source, new EventArgs()); // TT#301-MD - JSmith - Controls are not functioning properly
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }
        private HierarchyNodeProfile GetNodeProfile2(string aProductID)
        {
            string productID;
            string[] pArray;

            try
            {
                productID = aProductID.Trim();
                pArray = productID.Split(new char[] { '[' });
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

        // Begin TT#301-MD - JSmith - Controls are not functioning properly
        //private void cboInventoryBasis_DragDrop(object sender, DragEventArgs e)
        override protected void cboInventoryBasis_DragDrop(object sender, DragEventArgs e)
        // End TT#301-MD - JSmith - Controls are not functioning properly
        {
            TreeNodeClipboardList cbList;
            try
            {
                if (e.Data.GetDataPresent(typeof(TreeNodeClipboardList)))
                {
                    cbList = (TreeNodeClipboardList)e.Data.GetData(typeof(TreeNodeClipboardList));
                    HierarchyNodeProfile hnp = SAB.HierarchyServerSession.GetNodeData(cbList.ClipboardProfile.Key, true, true);
                    AddNodeToMerchandiseCombo3(hnp);
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
        // Begin TT#301-MD - JSmith - Controls are not functioning properly
        //private void cboInventoryBasis_Validated(object sender, EventArgs e)
        override protected void cboInventoryBasis_Validated(object sender, EventArgs e)
        // End TT#301-MD - JSmith - Controls are not functioning properly
        {
            try
            {
                // JBolles - MID Track #5020 - Prevent errored textbox from resetting its value before error is corrected
                if (!_priorErrorIB)
                {
                    ErrorProvider.SetError(cboInventoryBasis, string.Empty);
                    _textChangedIB = false;
                    _priorErrorIB = false;
                    _lastMerchIndexIB = -1;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        // Begin TT#301-MD - JSmith - Controls are not functioning properly
        //private void cboInventoryBasis_Validating(object sender, CancelEventArgs e)
        override protected void cboInventoryBasis_Validating(object sender, CancelEventArgs e)
        // End TT#301-MD - JSmith - Controls are not functioning properly
        {
            string errorMessage;

            try
            {
                if (cboInventoryBasis.Text == string.Empty)
                {
                    cboInventoryBasis.SelectedIndex = _lastMerchIndexIB;
                    //this.cboInventoryBasis_SelectionChangeCommitted(source, new EventArgs()); // TT#301-MD - JSmith - Controls are not functioning properly
                    _priorErrorIB = false;
                }
                else
                {
                    if (_textChangedIB)
                    {
                        _textChangedIB = false;

                        HierarchyNodeProfile hnp = GetNodeProfile2(cboInventoryBasis.Text);
                        if (hnp.Key == Include.NoRID)
                        {
                            _priorErrorIB = true;

                            errorMessage = string.Format(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_InvalidTreeNode), cboInventoryBasis.Text);
                            ErrorProvider.SetError(cboInventoryBasis, errorMessage);
                            MessageBox.Show(errorMessage);

                            e.Cancel = true;
                        }
                        else
                        {
                            AddNodeToMerchandiseCombo3(hnp);
                            _priorErrorIB = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        // Begin TT#301-MD - JSmith - Controls are not functioning properly
        //private void cboInventoryBasis_KeyDown(object sender, KeyEventArgs e)
        override protected void cboInventoryBasis_KeyDown(object sender, KeyEventArgs e)
        // End TT#301-MD - JSmith - Controls are not functioning properly
        {
            try
            {
                if (e.KeyCode == Keys.Up ||
                    e.KeyCode == Keys.Down)
                {
                    return;
                }
                _textChangedIB = true;
                if (_lastMerchIndexIB == -1)
                {
                    _lastMerchIndexIB = cboInventoryBasis.SelectedIndex;
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        // Begin TT#301-MD - JSmith - Controls are not functioning properly
        //private void cboInventoryBasis_SelectionChangeCommitted(object sender, EventArgs e)
        override protected void cboInventoryBasis_SelectionChangeCommitted(object sender, EventArgs e)
        // End TT#301-MD - JSmith - Controls are not functioning properly
        {
            if (FormLoaded &&
                !FormIsClosing)
            {
                ErrorProvider.SetError(cboInventoryBasis, string.Empty);
                _lastMerchIndexIB = cboInventoryBasis.SelectedIndex;
                ChangePending = true;
            }
        }
        //  END TT#41-MD - GTaylor - UC#2

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
		

		protected override void Call_btnProcess_Click()
		{
			try
			{	// begin A&F add Generic Size Curve Name 
				// BEGIN TT#696-MD - Stodd - add "active process"
				//====================================================
				// Checks to be sure there are valid selected headers
				//====================================================
				if (!OkToProcess(this, eMethodType.FillSizeHolesAllocation))		 
				{
					return;
				}	
				// END TT#696-MD - Stodd - add "active process"					
				// end A&F add Generic Size Curve Name  			 

				ProcessAction(eMethodType.FillSizeHolesAllocation);

				// as part of the  processing we saved the info, so it should be changed to update.
                // Begin TT#1951 - JSmith - Fill Sizes Error
                //_fillSizeHolesMethod.Method_Change_Type = eChangeType.update;
                //btnSave.Text = "&Update";
                if (!ErrorFound)
                {
                    _fillSizeHolesMethod.Method_Change_Type = eChangeType.update;
                    btnSave.Text = "&Update";
                }
                // End TT#1951
			}
			catch(Exception ex)
			{
				HandleException(ex, "Call_btnProcess_Click");
			}		
		}
		// End MID Track 4858

        // Begin TT#301-MD - JSmith - Controls are not functioning properly
        //private void cboSizeGroup_SelectionChangeCommitted(object sender, System.EventArgs e)
        override protected void cboSizeGroup_SelectionChangeCommitted(object sender, System.EventArgs e)
        // End TT#301-MD - JSmith - Controls are not functioning properly
		{
			try
			{
				if (base.FormLoaded)
				{
					if (_fillSizeHolesMethod.PromptSizeChange)
					{
						if (ShowWarningPrompt(false) == DialogResult.Yes)
						{

							_fillSizeHolesMethod.DeleteMethodRules(new TransactionData());

							//_fillSizeHolesMethod.SizeGroupRid = Convert.ToInt32(cboSizeGroup.SelectedValue.ToString(),CultureInfo.CurrentUICulture);;
							_fillSizeHolesMethod.SizeGroupRid = Convert.ToInt32(cboSizeGroup.SelectedValue.ToString(), CultureInfo.CurrentUICulture); // MID Track 3781 Size Curve not required for Fill Size Holes
							//_fillSizeHolesMethod.SizeCurveGroupRid = Convert.ToInt32(cboSizeCurve.SelectedValue.ToString(),CultureInfo.CurrentUICulture); // MID Track 3781 Size Curve not required for Fill Size Holes
							_fillSizeHolesMethod.PromptSizeChange = false; // MID Track 3781 Size Curve Not Required in Fill Size Holes
							_fillSizeHolesMethod.SizeCurveGroupRid = Include.NoRID; // MID Track 3781 Size Curve Not Required in Fill Size Holes
							cboSizeCurve.SelectedValue = Include.NoRID; // MID Track 3781 Size Curve Not Required in Fill Size Holes
                            //this.cboSizeCurve_SelectionChangeCommitted(source, new EventArgs()); // TT#301-MD - JSmith - Controls are not functioning properly
                            _fillSizeHolesMethod.GetSizesUsing = eGetSizes.SizeGroupRID; // MID Track 3781 Size Curve Not Required in Fill Size Holes
							_fillSizeHolesMethod.GetDimensionsUsing = eGetDimensions.SizeGroupRID; // MID Track 3781 Size Curve Not Required in Fill Size Holes
							_fillSizeHolesMethod.CreateConstraintData();
							BindAllSizeGrid(_fillSizeHolesMethod.MethodConstraints);
							CheckExpandAll();
                            _fillSizeHolesMethod.PromptSizeChange = true; // TT#2155 - JEllis - Fill Size Holes Null Reference
						}

						else
						{
							//Shut off the prompt so the combo can be reset to original value.
							_fillSizeHolesMethod.PromptSizeChange = false;
							//cboSizeGroup.SelectedValue = _fillSizeHolesMethod.SizeGroupRid;
							cboSizeGroup.SelectedValue = _fillSizeHolesMethod.SizeGroupRid; // MID Track 3781 Size Curve not required for Fill Size Holes
                            //this.cboSizeGroup_SelectionChangeCommitted(source, new EventArgs()); // TT#301-MD - JSmith - Controls are not functioning properly
							//cboSizeCurve.SelectedValue = _fillSizeHolesMethod.SizeCurveGroupRid; // MID Track 3781 Size Curve not required for Fill Size Holes
                            _fillSizeHolesMethod.PromptSizeChange = true; // TT#2155 - JEllis - Fill Size Holes Null Reference
						}
					}
					else
					{
						//Turn the prompt back on.
						_fillSizeHolesMethod.PromptSizeChange = true;
					}
                    // begin TT#2155 - JEllis - Fill Size Holes Null Reference
                    SetApplyRulesOnly_State();
                    // end TT#2155 - JEllis - Fill Size holes Null Reference
				}
			}
			catch (Exception ex)
			{
                // Begin TT#301-MD - JSmith - Controls are not functioning properly
                //HandleException(ex, "cboSizeGroup_SelectionChangeCommitted");
                HandleException(ex, "cboSizeGroup_SelectionChangeCommitted");
                // End TT#301-MD - JSmith - Controls are not functioning properly
			}
		}


		private void txtDesc_TextChanged(object sender, System.EventArgs e)
		{
			ChangePending = true;
		}


		private void txtAvailable_TextChanged(object sender, System.EventArgs e)
		{
			try
			{
                if (txtAvailable.Text == null || txtAvailable.Text.Trim() == string.Empty)
                    rbPercent.Enabled = rbUnits.Enabled = false;
                // Begin TT#1530 - RMatelic - Record locking violations from Store Attribute change
                //else
                    //rbPercent.Enabled = rbUnits.Enabled = true;
                else if (FunctionSecurity.AllowUpdate)
                {
                    rbPercent.Enabled = rbUnits.Enabled = true;
                }
                else
                {
                    rbPercent.Enabled = rbUnits.Enabled = false;
                }
                // End TT#1530
			}
			catch (Exception ex)
			{
				HandleException(ex, "txtAvailable_TextChanged");
			}
		}


		private void txtAvailable_Validating(object sender, System.ComponentModel.CancelEventArgs e)
		{
			try
			{
				ChangePending = true;
				AttachErrors(txtAvailable);
				FormatAvailable();
			}
			catch
			{
				ErrorMessages.Add(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MustBeNumeric));
				AttachErrors(txtAvailable);
				((TextBox)sender).Text = string.Empty;
				((TextBox)sender).Focus();
			}
		}

        // Begin TT#301-MD - JSmith - Controls are not functioning properly
        //private void cboStoreAttribute_SelectionChangeCommitted(object sender, System.EventArgs e)
        override protected void cboStoreAttribute_SelectionChangeCommitted(object sender, System.EventArgs e)
        // End TT#301-MD - JSmith - Controls are not functioning properly
		{

			try
			{
				if (base.FormLoaded)
				{
					//if (_fillSizeHolesMethod.SgRid != Convert.ToInt32(cboStoreAttribute.SelectedValue.ToString(),CultureInfo.CurrentUICulture))
					//{
					if (_fillSizeHolesMethod.PromptAttributeChange)
					{
						if (ShowWarningPrompt(false) == DialogResult.Yes)
						{

							_fillSizeHolesMethod.DeleteMethodRules(new TransactionData());

							_fillSizeHolesMethod.SG_RID = Convert.ToInt32(cboStoreAttribute.SelectedValue.ToString(),CultureInfo.CurrentUICulture);
							_fillSizeHolesMethod.CreateConstraintData();
							BindAllSizeGrid(_fillSizeHolesMethod.MethodConstraints);
							CheckExpandAll();
						}
						else
						{
							//Shut off the prompt so the combo can be reset to original value.
							_fillSizeHolesMethod.PromptAttributeChange = false;
							cboStoreAttribute.SelectedValue = _fillSizeHolesMethod.SG_RID;
                            //this.cboStoreAttribute_SelectionChangeCommitted(source, new EventArgs()); // TT#301-MD - JSmith - Controls are not functioning properly
						}
					}
					else
					{
						//Turn the prompt back on.
						_fillSizeHolesMethod.PromptAttributeChange = true;
					}
				}
			}
			catch (Exception ex)
			{
                // Begin TT#301-MD - JSmith - Controls are not functioning properly
                //HandleException(ex, "cboStoreAttribute_SelectionChangeCommitted");
                HandleException(ex, "cboStoreAttribute_SelectionChangeCommitted");
                // End TT#301-MD - JSmith - Controls are not functioning properly
			}
		}

        // Begin TT#301-MD - JSmith - Controls are not functioning properly
        //private void cboStoreAttribute_DragEnter(object sender, DragEventArgs e)
        //{

        //}

        //private void cboStoreAttribute_DragDrop(object sender, DragEventArgs e)
        //{
        //    //Begin Track #5858 - Kjohnson - Validating store security only
        //    try
        //    {
        //        bool isSuccessfull = ((MIDComboBoxTag)(((MIDComboBoxEnh)sender).Tag)).ComboBox_DragDrop(sender, e);

        //        if (isSuccessfull)
        //        {
        //            ChangePending = true;
        //        }
        //    }
        //    catch (Exception exc)
        //    {
        //        HandleException(exc);
        //    }
        //    //End Track #5858
        //}
        // End TT#301-MD - JSmith - Controls are not functioning properly

		private void radUser_CheckedChanged(object sender, System.EventArgs e)
		{
			if (FormLoaded)
			{
				ChangePending = true;
			}

			if (radUser.Checked)
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
			return securityOk;	// Track 5871 stodd
		}

		private void rbPercent_CheckedChanged(object sender, System.EventArgs e)
		{
			try
			{
				ChangePending = true;
				FormatAvailable();
			}
			catch (Exception ex)
			{
				HandleException(ex, "rbPercent_CheckedChanged");
			}
		}


		private void rbUnits_CheckedChanged(object sender, System.EventArgs e)
		{
			ChangePending = true;
		}

        // Begin TT#301-MD - JSmith - Controls are not functioning properly
        //private void cboSizeCurve_SelectionChangeCommitted(object sender, System.EventArgs e)
        override protected void cboSizeCurve_SelectionChangeCommitted(object sender, System.EventArgs e)
        // End TT#301-MD - JSmith - Controls are not functioning properly
		{
			try
			{
				if (base.FormLoaded)
				{
					if (_fillSizeHolesMethod.PromptSizeChange)
					{
						// Begin TT#499 - RMatelic - Check Rules and Basis Substitutes to determine if warning message is shown
                        if (RuleExists() || BasisSubstituteExists())
                        {
                            if (ShowWarningPrompt(false) == DialogResult.Yes)
                            {
                                //_fillSizeHolesMethod.DeleteMethodRules(new TransactionData());
                                //_fillSizeHolesMethod.SizeCurveGroupRid = Convert.ToInt32(cboSizeCurve.SelectedValue.ToString(), CultureInfo.CurrentUICulture); ;
                                //_fillSizeHolesMethod.PromptSizeChange = false; // MID Track 3781 Size Curve not required on Fill Size Holes
                                //_fillSizeHolesMethod.SizeGroupRid = Include.NoRID; // MID Track 3781 Size Curve not required on Fill Size Holes
                                //cboSizeGroup.SelectedValue = Include.NoRID; // MID Track 3781 Size Curve Not Required in Fill Size Holes
                                //_fillSizeHolesMethod.GetSizesUsing = eGetSizes.SizeCurveGroupRID; // MID Track 3781 Size Curve Not Required in Fill Size Holes
                                //_fillSizeHolesMethod.GetDimensionsUsing = eGetDimensions.SizeCurveGroupRID; // MID Track 3781 Size Curve Not Required in Fill Size Holes
                                //_fillSizeHolesMethod.CreateConstraintData();

                                //BindAllSizeGrid(_fillSizeHolesMethod.MethodConstraints);
                                //CheckExpandAll();
                                UpdateCurveData();
                            }

                            else
                            {
                                //Shut off the prompt so the combo can be reset to original value.
                                _fillSizeHolesMethod.PromptSizeChange = false;
                                cboSizeCurve.SelectedValue = _fillSizeHolesMethod.SizeCurveGroupRid;
                                //this.cboSizeCurve_SelectionChangeCommitted(source, new EventArgs()); // TT#301-MD - JSmith - Controls are not functioning properly
                                _fillSizeHolesMethod.PromptSizeChange = true; // TT#2155 - JEllis - Fill Size Holes Null Reference
                            }
                        }
                        else
                        {
                            UpdateCurveData();
                        }
                    }   // End TT#499
					else
					{
						//Turn the prompt back on.
						_fillSizeHolesMethod.PromptSizeChange = true;
					}
				}
			}
			catch (Exception ex)
			{
                // Begin TT#301-MD - JSmith - Controls are not functioning properly
                //HandleException(ex, "cboSizeCurve_SelectionChangeCommitted");
                HandleException(ex, "cboSizeCurve_SelectionChangeCommitted");
                // End TT#301-MD - JSmith - Controls are not functioning properly
			}
		}

         // Begin TT#499 - RMatelic - Size Need Method Default checked on the Rule Tab the Add only goes down to Color - move code for RuleExists Check 
        private void UpdateCurveData()
        {
            try
            {
                _fillSizeHolesMethod.DeleteMethodRules(new TransactionData());
                _fillSizeHolesMethod.SizeCurveGroupRid = Convert.ToInt32(cboSizeCurve.SelectedValue.ToString(), CultureInfo.CurrentUICulture); ;
                _fillSizeHolesMethod.PromptSizeChange = false; // MID Track 3781 Size Curve not required on Fill Size Holes
                _fillSizeHolesMethod.SizeGroupRid = Include.NoRID; // MID Track 3781 Size Curve not required on Fill Size Holes
                cboSizeGroup.SelectedValue = Include.NoRID; // MID Track 3781 Size Curve Not Required in Fill Size Holes
                //this.cboSizeGroup_SelectionChangeCommitted(source, new EventArgs()); // TT#301-MD - JSmith - Controls are not functioning properly
                SetApplyRulesOnly_State(); // TT#2155 - JEllis - Fill Size Holes Null Reference
                _fillSizeHolesMethod.GetSizesUsing = eGetSizes.SizeCurveGroupRID; // MID Track 3781 Size Curve Not Required in Fill Size Holes
                _fillSizeHolesMethod.GetDimensionsUsing = eGetDimensions.SizeCurveGroupRID; // MID Track 3781 Size Curve Not Required in Fill Size Holes
                _fillSizeHolesMethod.CreateConstraintData();
                BindAllSizeGrid(_fillSizeHolesMethod.MethodConstraints);
                CheckExpandAll();
                _fillSizeHolesMethod.PromptSizeChange = true;
                ChangePending = true;
            }
            catch  
            {
                throw;
            }
        }
        // begin TT#2155 - JEllis - Fill Size Holes Null Reference
        private void cbxUseDefaultCurve_CheckChanged(object sender, System.EventArgs e)
        {
            try
            {
                if (base.FormLoaded)
                {
                    SetApplyRulesOnly_State();
                }
 
            }
            catch (Exception ex)
            {
                HandleException(ex, "cbxUseDefaultCurve_CheckChanged");
            }
        }

        // Begin TT#301-MD - JSmith - Controls are not functioning properly
        //private void cboHierarchyLevel_SelectionChangeCommitted(object sender, System.EventArgs e)
        override protected void cboHierarchyLevel_SelectionChangeCommitted(object sender, System.EventArgs e)
        // End TT#301-MD - JSmith - Controls are not functioning properly
        {
            try
            {
                if (base.FormLoaded)
                {
                    SetApplyRulesOnly_State();
                }

            }
            catch (Exception ex)
            {
                // Begin TT#301-MD - JSmith - Controls are not functioning properly
                //HandleException(ex, "cboHierarchyLevel_SelectionChangeCommitted");
                HandleException(ex, "cboHierarchyLevel_SelectionChangeCommitted");
                // End TT#301-MD - JSmith - Controls are not functioning properly
            }
        }

        // Begin TT#301-MD - JSmith - Controls are not functioning properly
        //private void cboHeaderChar_SelectionChangeCommitted(object sender, System.EventArgs e)
        override protected void cboHeaderChar_SelectionChangeCommitted(object sender, System.EventArgs e)
        // End TT#301-MD - JSmith - Controls are not functioning properly
        {
            try
            {
                if (base.FormLoaded)
                {
                    SetApplyRulesOnly_State();
                }

            }
            catch (Exception ex)
            {
                // Begin TT#301-MD - JSmith - Controls are not functioning properly
                //HandleException(ex, "cboHeaderChar_SelectionChangeCommitted");
                HandleException(ex, "cboHeaderChar_SelectionChangeCommitted");
                // End TT#301-MD - JSmith - Controls are not functioning properly
            }
        }

        // Begin TT#301-MD - JSmith - Controls are not functioning properly
        //private void cboNameExtension_SelectionChangeCommitted(object sender, System.EventArgs e)
        override protected void cboNameExtension_SelectionChangeCommitted(object sender, System.EventArgs e)
        // End TT#301-MD - JSmith - Controls are not functioning properly
        {
            try
            {
                if (base.FormLoaded)
                {
                    SetApplyRulesOnly_State();
                }

            }
            catch (Exception ex)
            {
                // Begin TT#301-MD - JSmith - Controls are not functioning properly
                //HandleException(ex, "cboNameExtension_SelectionChangeCommitted");
                HandleException(ex, "cboNameExtension_SelectionChangeCommitted");
                // End TT#301-MD - JSmith - Controls are not functioning properly
            }
        }
        private void SetApplyRulesOnly_State()
        {
            if (_fillSizeHolesMethod.SizeGroupRid != Include.NoRID)
            {
                if (!cbxUseDefaultcurve.Checked
                   && Convert.ToInt32(cboHierarchyLevel.SelectedValue, CultureInfo.CurrentUICulture) == -2
                   && Convert.ToInt32(cboHeaderChar.SelectedValue, CultureInfo.CurrentUICulture) == Include.NoRID
                   && Convert.ToInt32(cboNameExtension.SelectedValue, CultureInfo.CurrentUICulture) == Include.NoRID)
                {
                    cbxApplyRulesOnly.Checked = false;
                    _fillSizeHolesMethod.ApplyRulesOnly = false;
                    cbxApplyRulesOnly.Enabled = false;
                }
                else
                {
                    cbxApplyRulesOnly.Enabled = true;
                }
            }
            else if (_fillSizeHolesMethod.SizeCurveGroupRid != Include.NoRID)
            {
                cbxApplyRulesOnly.Enabled = true;
            }
            else 
            {
                cbxApplyRulesOnly.Checked = false;
                _fillSizeHolesMethod.ApplyRulesOnly = false;
                cbxApplyRulesOnly.Enabled = false;
            }
        }
        // end TT#2155 - JEllis - Fill Size Holes Null Reference

        // Begin TT#301-MD - JSmith - Controls are not functioning properly
        //private void cboMerchandise_SelectionChangeCommitted(object sender, System.EventArgs e)
        private void cboMerchandise_SelectionChangeCommitted(object sender, System.EventArgs e)
        // Begin TT#301-MD - JSmith - Controls are not functioning properly
		{
			if (FormLoaded &&
				!FormIsClosing)
			{
				ErrorProvider.SetError(cboMerchandise, string.Empty);
				_lastMerchIndex = cboMerchandise.SelectedIndex;
				ChangePending = true;
			}
		}
        //Begin TT#316 - MD - DOConnell - Replace all Windows Combobox controls with new enhanced control
        void cboMerchandise_MIDComboBoxPropertiesChangedEvent(object source, MIDComboBoxPropertiesChangedEventArgs args)
        {
            this.cboMerchandise_SelectionChangeCommitted(source, new EventArgs());
        }
        //End TT#316 - MD - DOConnell - Replace all Windows Combobox controls with new enhanced control

		private void cboMerchandise_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
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
					_lastMerchIndex = cboMerchandise.SelectedIndex;
				}
			}
			catch (Exception ex)
			{
				HandleException(ex, "cboMerchandise_KeyDown");
			}
		}

		private void cboMerchandise_Validating(object sender, System.ComponentModel.CancelEventArgs e)
		{
			string errorMessage;

			try
			{
				if (cboMerchandise.Text == string.Empty)
				{
					cboMerchandise.SelectedIndex = _lastMerchIndex;
                    //this.cboMerchandise_SelectionChangeCommitted(source, new EventArgs()); // TT#301-MD - JSmith - Controls are not functioning properly
					_priorError = false;
				}
				else
				{
					if (_textChanged)
					{
						_textChanged = false;

						HierarchyNodeProfile hnp = GetNodeProfile(cboMerchandise.Text);
						if (hnp.Key == Include.NoRID)
						{
							_priorError = true;

							errorMessage = string.Format(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_InvalidTreeNode), cboMerchandise.Text);
							ErrorProvider.SetError(cboMerchandise, errorMessage);
							MessageBox.Show(errorMessage);

							e.Cancel = true;
						}
						else 
						{
							AddNodeToMerchandiseCombo(hnp);
							_priorError = false;
						}	
					}
//					else if (_priorError)
//					{
//						cboMerchandise.SelectedIndex = _lastMerchIndex;
//					}
				}
			}
			catch (Exception ex)
			{
				HandleException(ex, "cboMerchandise_Validating");
			}
		}

		private void cboMerchandise_Validated(object sender, System.EventArgs e)
		{
			try
			{
				if(!_priorError)
				{
					ErrorProvider.SetError(cboMerchandise, string.Empty);
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

		private void cboMerchandise_DragOver(object sender, DragEventArgs e)
		{
            // Begin TT#296-MD - JSmith - Methods and Workflows can be dragged and dropped into Inventory Basis drop down.
            Merchandise_DragOver(sender, e);
            //Image_DragOver(sender, e);
            // End TT#296-MD - JSmith - Methods and Workflows can be dragged and dropped into Inventory Basis drop down.
		}

		private void cboMerchandise_DragDrop(object sender, System.Windows.Forms.DragEventArgs e)
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
                }

			}
			catch (BadDataInClipboardException)
			{
				MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_BadDataInClipboard),
					this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			catch (Exception ex)
			{
				HandleException(ex, "cboMerchandise_DragDrop");
			}
		}

        
        private void cboMerchandise_DragEnter(object sender, System.Windows.Forms.DragEventArgs e)
        {
            // Begin TT#296-MD - JSmith - Methods and Workflows can be dragged and dropped into Inventory Basis drop down.
            Merchandise_DragEnter(sender, e);
            //try
            //{
            //    Image_DragEnter(sender, e);
            //    ObjectDragEnter(e);
            //}
            //catch (Exception ex)
            //{
            //    HandleException(ex, "cboMerchandise_DragEnter");
            //}
            // End TT#296-MD - JSmith - Methods and Workflows can be dragged and dropped into Inventory Basis drop down.
        }
        

		/// <summary>
		/// Toggles the buttons on the AddNewBox to enabled/disabled depending on the current band.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
        // Begin TT#301-MD - JSmith - Controls are not functioning properly
        //private void ugRules_AfterRowActivate(object sender, System.EventArgs e)
        override protected void ugRules_AfterRowActivate(object sender, System.EventArgs e)
        // End TT#301-MD - JSmith - Controls are not functioning properly
		{
			try
			{
				UltraGridRow myRow = ((UltraGrid)sender).ActiveRow;

				switch (myRow.Band.Key.ToUpper())
				{
					case C_SET:
						if ((eSizeMethodRowType)myRow.Cells["ROW_TYPE_ID"].Value == eSizeMethodRowType.AllSize)
						{
							ToggleAddNewBoxButtons(false);
						}
						else
						{
							ToggleAddNewBoxButtons(true);
						}
						break;
					default:
						ToggleAddNewBoxButtons(true);
						break;
				}
			}
			catch (Exception ex)
			{
				HandleException(ex, "ugRules_AfterRowActivate");
			}

		}

		private void cbExpandAll_CheckedChanged(object sender, System.EventArgs e)
		{
			CheckExpandAll();
		}

		
		// begin A&F Add Generic Size Curve data 
		private void tabOTSMethod_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			 
			if (tabOTSMethod.SelectedTab == tabMethod && tabControl1.SelectedTab == tabPageGeneral)
			{
				gbGenericSizeCurve.Visible =   true;
			}
			else
			{
				gbGenericSizeCurve.Visible =   false;
			}
		}
		
		private void tabControl1_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			gbGenericSizeCurve.Visible =  (tabControl1.SelectedTab == tabPageGeneral) ? true : false;
			// MID TRack #6134 - Size Curves group box issue - comment out next line - unnecessary
            //gbxNormalizeSizeCurves.Visible =  (tabControl1.SelectedTab == tabPageGeneral) ? true : false;
		}
		// end A&F Add Generic Size Curve data 


        //Begin TT#1636-MD -jsobek -Pre-Pack Fill Size
        private void cbxOverrideAvgPackDev_CheckedChanged(object sender, EventArgs e)
        {
            if (!cbxOverrideAvgPackDev.Checked)
            {
                double avgPackDevTolerance = SAB.ApplicationServerSession.GlobalOptions.PackSizeErrorPercent;
                txtAvgPackDevTolerance.Text = (avgPackDevTolerance == Double.MaxValue) ? string.Empty : Convert.ToString(avgPackDevTolerance, CultureInfo.CurrentUICulture);
            }
            txtAvgPackDevTolerance.Enabled = cbxOverrideAvgPackDev.Checked;
        }
        private void txtAvgPackDevTolerance_TextChanged(object sender, EventArgs e)
        {
            if (base.FormLoaded)
            {
                ChangePending = true;
            }
        }
        private void cbxOverrideMaxPackAllocNeed_CheckedChanged(object sender, EventArgs e)
        {
            if (!cbxOverrideMaxPackAllocNeed.Checked)
            {
                double maxPackAllocNeedTolerance = SAB.ApplicationServerSession.GlobalOptions.MaxSizeErrorPercent;
                txtMaxPackAllocNeedTolerance.Text = (maxPackAllocNeedTolerance == Double.MaxValue) ? string.Empty : Convert.ToString(maxPackAllocNeedTolerance, CultureInfo.CurrentUICulture);
                // begin TT#1365 - JEllis - FL Detail Pack Size Need Enhancement
                cbxNoMaxStep.Checked = SAB.ApplicationServerSession.GlobalOptions.PackToleranceNoMaxStep;
                cbxStepped.Checked = SAB.ApplicationServerSession.GlobalOptions.PackToleranceStepped;
                // end TT#1365 - Jellis - FL Detail Pack Size Need Enhancement
            }
            txtMaxPackAllocNeedTolerance.Enabled = cbxOverrideMaxPackAllocNeed.Checked;
            // begin TT#1365 - JEllis - FL Detail Pack Size Need Enhancement
            cbxNoMaxStep.Enabled = cbxOverrideMaxPackAllocNeed.Checked;
            cbxStepped.Enabled = cbxOverrideMaxPackAllocNeed.Checked;
            // end TT#1365 - Jellis - FL Detail Pack Size Need Enhancement
        }
        private void txtMaxPackAllocNeedTolerance_TextChanged(object sender, System.EventArgs e)
        {
            if (base.FormLoaded)
            {
                ChangePending = true;
            }
        }
        private void cbxNoMaxStep_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void cbxStepped_CheckedChanged(object sender, EventArgs e)
        {

        }
        //End TT#1636-MD -jsobek -Pre-Pack Fill Size

		
	#endregion Control Event Handlers

	#region Methods

		/// <summary>
		/// Formats the value in txtAvailable appropriately
		/// </summary>
		private void FormatAvailable()
		{
			try
			{
				double dblValue;

				string inStr = txtAvailable.Text;
				if (inStr == null || inStr.Trim() == string.Empty) 
					return;
						
				dblValue = Convert.ToDouble(inStr, CultureInfo.CurrentUICulture);
						
				if (rbPercent.Checked)
				{
					dblValue = Math.Round(dblValue,2);
					txtAvailable.Text = dblValue.ToString(CultureInfo.CurrentUICulture);
				}
				else
				{
					dblValue = Math.Round(dblValue,0);
					txtAvailable.Text = dblValue.ToString(CultureInfo.CurrentUICulture);
				}
			}
			catch (Exception ex)
			{
				HandleException(ex, "FormatAvailable");
			}
		}


		/// <summary>
		/// Private method, enables or disables the buttons on the AddNewBox on ugRules.
		/// Called from ugRules_AfterRowActivate
		/// </summary>
		/// <param name="Defaults"></param>
		private void ToggleAddNewBoxButtons(bool Defaults)
		{
			try
			{
				//THESE TWO BANDS SHOULD NEVER BE ALLOWED TO ADD NEW OR DELETE
				//============================================================
				ugRules.DisplayLayout.Bands[C_SET].Override.AllowAddNew = AllowAddNew.No;
				ugRules.DisplayLayout.Bands[C_SET_ALL_CLR].Override.AllowAddNew = AllowAddNew.No;
				ugRules.DisplayLayout.Bands[C_SET].Override.AllowDelete = DefaultableBoolean.False;
				ugRules.DisplayLayout.Bands[C_SET_ALL_CLR].Override.AllowDelete = DefaultableBoolean.False;
				//============================================================

				if (FunctionSecurity.AllowUpdate)
				{

					if (Defaults)
					{
						//SETUP FOR WHEN THE ACTIVE ROW IS NOT THE ALLSIZE ROW FROM THE SET BAND
						ugRules.DisplayLayout.Bands[C_SET_CLR].Override.AllowAddNew = AllowAddNew.Yes;
					
						//if (_fillSizeHolesMethod.SizeGroupRid != Include.NoRID)
						if (_fillSizeHolesMethod.SizeCurveGroupRid != Include.NoRID  // MID Track 3781 Size Curve Not required for Fill Size Holes
						    || _fillSizeHolesMethod.SizeGroupRid != Include.NoRID) // MID Track 3781 Size Curve Not required for Fill Size Holes
						{
							ugRules.DisplayLayout.Bands[C_ALL_CLR_SZ].Override.AllowAddNew = AllowAddNew.Yes;
							ugRules.DisplayLayout.Bands[C_CLR_SZ].Override.AllowAddNew = AllowAddNew.Yes;
							ugRules.DisplayLayout.Bands[C_CLR_SZ_DIM].Override.AllowAddNew = AllowAddNew.Yes;
							ugRules.DisplayLayout.Bands[C_ALL_CLR_SZ_DIM].Override.AllowAddNew = AllowAddNew.Yes;
						}
					}
					else
					{
						//SETUP FOR WHEN THE ACTIVE ROW IS THE ALLSIZE ROW FROM THE SET BAND
						ugRules.DisplayLayout.Bands[C_SET_CLR].Override.AllowAddNew = AllowAddNew.No;
					
						//if (_fillSizeHolesMethod.SizeGroupRid != Include.NoRID)
						if (_fillSizeHolesMethod.SizeCurveGroupRid != Include.NoRID // MID Track 3781 Size Curve Not required for Fill Size Holes
						    || _fillSizeHolesMethod.SizeGroupRid != Include.NoRID) // MID Track 3781 Size Curve Not required for Fill Size Holes
						{
							ugRules.DisplayLayout.Bands[C_ALL_CLR_SZ].Override.AllowAddNew = AllowAddNew.No;
							ugRules.DisplayLayout.Bands[C_CLR_SZ].Override.AllowAddNew = AllowAddNew.No;
							ugRules.DisplayLayout.Bands[C_CLR_SZ_DIM].Override.AllowAddNew = AllowAddNew.No;
							ugRules.DisplayLayout.Bands[C_ALL_CLR_SZ_DIM].Override.AllowAddNew = AllowAddNew.No;
						}
					}
				}
				else
				{
					foreach (UltraGridBand ugb in ugRules.DisplayLayout.Bands)
					{
						ugb.Override.AllowAddNew = AllowAddNew.No;
						ugb.Override.AllowDelete = DefaultableBoolean.False;
					}
				}
			}
			catch (Exception ex)
			{
				HandleException(ex, "ToggleAddNewBoxButtons");
			}
		
		}


		/// <summary>
		/// Private method handles loading data on the form
		/// </summary>
		/// <param name="aGlobalUserType"></param>
		private void Common_Load(eGlobalUserType aGlobalUserType)
		{
			try
			{		
				SetText();

				Name = MIDText.GetTextOnly((int)eMethodType.FillSizeHolesAllocation);
						
				LoadMethods();

                GetWorkflows(_fillSizeHolesMethod.Key, ugWorkflows);

				// BEGIN MID Track #4826 - JSmith - Normalize Size Curves
				if (_fillSizeHolesMethod.NormalizeSizeCurvesDefaultIsOverridden)
				{
					cbxOverrideNormalizeDefault.Checked = true;
					radNormalizeSizeCurves_Yes.Enabled = true;
					radNormalizeSizeCurves_No.Enabled = true;
				}
				else
				{
					radNormalizeSizeCurves_Yes.Enabled = false;
					radNormalizeSizeCurves_No.Enabled = false;
				}
				// END MID Track #4826

                // Begin TT#413 - JSmith - Add new Generic Size Curve names to correspond with the new Size Curves being generated within Node Properties settings.
                cbxUseDefaultcurve.Checked = _fillSizeHolesMethod.UseDefaultCurve;
                // End TT#413

                // begin TT#2155 - JEllis - Fill Size Holes Null Reference
                cbxApplyRulesOnly.Checked = _fillSizeHolesMethod.ApplyRulesOnly;
                // end TT#2155 - JEllis - Fill Size Holes Null Reference

				// BEGIN MID Track #4921 - JSmith - A&F 666 - Size Modification
				if (_fillSizeHolesMethod.FillSizesToType == eFillSizesToType.SizePlan)
				{
					this.radFillSizesTo_SizePlan.Checked = true;
				}
                else if (_fillSizeHolesMethod.FillSizesToType == eFillSizesToType.SizePlanWithMins) //TT#848-MD -jsobek -Fill to Size Plan Presentation
                {
                    this.radFillSizesTo_SizePlanWithSizeMins.Checked = true;
                }
				else
				{
					this.radFillSizesTo_Holes.Checked = true;
				}
				// END MID Track #4921

//				if (aGlobalUserType == eGlobalUserType.User &&
//					!_sab.ClientServerSession.GetUserFunctionSecurityAssignment(_sab.ClientServerSession.UserRID, eSecurityFunctions.AllocationMethods).AllowUpdate)
//				{
//					radGlobal.Enabled = false;
//				}
				
				// BEGIN ANF Generic Size Constraint
				SetMaskedComboBoxesEnabled(); 
				// END ANF Generic Size Constraint	
                // Begin TT#44 - JSmith - Drag/Drop User Attributes or Filters in to Global Methods does not react consistantly
                cboFilter.Tag = new MIDStoreFilterComboBoxTag(SAB, cboFilter.ComboBox, eSecurityTypes.Store, eSecuritySelectType.View | eSecuritySelectType.Update, FunctionSecurity, aGlobalUserType == eGlobalUserType.User);
                // End TT#44

                Load_IB_Combo(); // TT#41-MD - GTaylor - UC#2

                //BEGIN TT#110-MD-VStuart - In Use Tool
                tabOTSMethod.Controls.Remove(tabProperties);
                //END TT#110-MD-VStuart - In Use Tool
            }
			catch( Exception ex )
			{
				HandleException(ex, "Common_Load");
			}
		}


		/// <summary>
		/// Controls the text for the Process, {Save,Update}, and Close buttons.
		/// </summary>
		private void SetText()
		{
			try
			{
				if (_fillSizeHolesMethod.Method_Change_Type == eChangeType.update)
					btnSave.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_Update);
				else
					btnSave.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_Save);

				this.btnClose.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_Close);
				this.btnProcess.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_Process);
				// BEGIN MID Track #4921 - JSmith - A&F 666 - Size Modification
                //Begin TT#848-MD -jsobek -Fill to Size Plan Presentation
				this.gboFillSizesTo.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Fill_SizesTo) + ":";
                this.radFillSizesTo_SizePlanWithSizeMins.Text = MIDText.GetTextOnly((int)eFillSizesToType.SizePlanWithMins);
                //End TT#848-MD -jsobek -Fill to Size Plan Presentation
				this.radFillSizesTo_Holes.Text = MIDText.GetTextOnly((int)eFillSizesToType.Holes);
				this.radFillSizesTo_SizePlan.Text = MIDText.GetTextOnly((int)eFillSizesToType.SizePlan);
				// END MID Track #4921
                this.lblInventoryBasis.Text = MIDText.GetTextOnly((int)eMIDTextCode.lbl_InventoryBasis); // TT#41-MD - GTaylor - UC#2
                //Begin TT#1636-MD -jsobek -Pre-Pack Fill Size
                string lblOverrideDefault = MIDText.GetTextOnly(eMIDTextCode.lbl_OverrideDefault);
                //cbxOverrideNormalizeDefault.Text = lblOverrideDefault;
                cbxOverrideAvgPackDev.Text = lblOverrideDefault;
                cbxOverrideMaxPackAllocNeed.Text = lblOverrideDefault;
                //End TT#1636-MD -jsobek -Pre-Pack Fill Size
			}
			catch (Exception ex)
			{
				HandleException(ex, "SetText");
			}
		}


		/// <summary>
		/// Private method that initializes controls on the form
		/// </summary>
		private void LoadMethods()
		{
			try
			{			
                //Begin Track #5858 - KJohnson - Validating store security only
                // Begin TT#44 - JSmith - Drag/Drop User Attributes or Filters in to Global Methods does not react consistantly
                //cboStoreAttribute.Tag = new MIDStoreAttributeComboBoxTag(SAB, cboStoreAttribute);
                cboStoreAttribute.Tag = new MIDStoreAttributeComboBoxTag(SAB, cboStoreAttribute, FunctionSecurity, _fillSizeHolesMethod.GlobalUserType == eGlobalUserType.User);
                // End TT#44
                //End Track #5858
				BuildDataTables(); //Inherited from WorkflowMethodFormBase
				BindFilterComboBox(); //Inherited from SizeMethodsFormBase
				BindMerchandiseComboBox();
                // Begin Track #4872 - JSmith - Global/User Attributes
                //BindStoreAttrComboBox(); //Inherited from SizeMethodsFormBase
                BindStoreAttrComboBox(_fillSizeHolesMethod.Method_Change_Type, _fillSizeHolesMethod.GlobalUserType); //Inherited from SizeMethodsFormBase
                // End Track #4872
				BindAllSizeGrid(_fillSizeHolesMethod.MethodConstraints); //Inherited from SizeMethodsFormBase
			
				// BEGIN ANF Generic Size Constraint
				BindSizeComboBoxes(_fillSizeHolesMethod.SizeGroupRid, _fillSizeHolesMethod.SizeAlternateRid, 
				                   _fillSizeHolesMethod.SizeCurveGroupRid, _fillSizeHolesMethod.SizeConstraintRid);
				// END ANF Generic Size Constraint

				// ANF - add Generic Size Curve
				LoadGenericSizeCurveGroupBox();

				this.txtName.Text = _fillSizeHolesMethod.Name;
				this.txtDesc.Text = _fillSizeHolesMethod.Method_Description;
					
				//OK If New
				LoadFillSizeHoleValues();
			}
			catch (Exception ex)
			{
				HandleException(ex, "LoadMethods");
			}
		}

        // BEGIN TT#41-MD - GTaylor - UC#2
        private void AddNodeToIBCombo(HierarchyNodeProfile hnp)
        {
            try
            {
                DataRow myDataRow;
                bool nodeFound = false;
                int nodeRID = Include.NoRID;
                int levIndex;
                for (levIndex = 0;
                    levIndex < MerchandiseDataTable3.Rows.Count; levIndex++)
                {
                    myDataRow = MerchandiseDataTable3.Rows[levIndex];
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
                    myDataRow = MerchandiseDataTable3.NewRow();
                    myDataRow["seqno"] = MerchandiseDataTable3.Rows.Count;
                    myDataRow["leveltypename"] = eMerchandiseType.Node;
                    myDataRow["text"] = hnp.Text;
                    myDataRow["key"] = hnp.Key;
                    MerchandiseDataTable3.Rows.Add(myDataRow);

                    cboInventoryBasis.SelectedIndex = MerchandiseDataTable3.Rows.Count - 1;
                    //this.cboInventoryBasis_SelectionChangeCommitted(source, new EventArgs()); // TT#301-MD - JSmith - Controls are not functioning properly
                }
                else
                {
                    cboInventoryBasis.SelectedIndex = levIndex;
                    //this.cboInventoryBasis_SelectionChangeCommitted(source, new EventArgs()); // TT#301-MD - JSmith - Controls are not functioning properly
                }
            }
            catch (Exception ex)
            {
                HandleException(ex, "AddNodeToMerchandiseCombo");
            }
        }
        // END TT#41-MD - GTaylor - UC#2

		private void AddNodeToMerchandiseCombo (HierarchyNodeProfile hnp )
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

					cboMerchandise.SelectedIndex = MerchandiseDataTable.Rows.Count - 1;
                    //this.cboMerchandise_SelectionChangeCommitted(source, new EventArgs()); // TT#301-MD - JSmith - Controls are not functioning properly
				}
				else
				{
					cboMerchandise.SelectedIndex = levIndex;
                    //this.cboMerchandise_SelectionChangeCommitted(source, new EventArgs()); // TT#301-MD - JSmith - Controls are not functioning properly
				}
			}
			catch(Exception ex)
			{
				HandleException(ex, "AddNodeToMerchandiseCombo");
			}
		}


		/// <summary>
		/// Private method loads the Fill Size Holes Data associated with the current Fill Size Hole Method.
		/// </summary>
		private void LoadFillSizeHoleValues()
		{
			try
			{

				txtAvailable.Text = _fillSizeHolesMethod.Available.ToString();
				if (_fillSizeHolesMethod.PercentInd)
				{
					rbPercent.Checked = true;
				}
				else
				{
					rbUnits.Checked = true;
				}


				if (_fillSizeHolesMethod.StoreFilterRid == Include.NoRID)
				{
					cboFilter.SelectedValue = Include.UndefinedStoreFilter;
				}
				else
				{
					cboFilter.SelectedIndex = cboFilter.Items.IndexOf(new FilterNameCombo(_fillSizeHolesMethod.StoreFilterRid, -1, ""));

				}

                if (_fillSizeHolesMethod.SG_RID == Include.NoRID)
                {
                    cboStoreAttribute.SelectedValue = SAB.ApplicationServerSession.GlobalOptions.AllocationStoreGroupRID;
                    //this.cboStoreAttribute_SelectionChangeCommitted(source, new EventArgs()); // TT#301-MD - JSmith - Controls are not functioning properly
                }
                else
                {
                    cboStoreAttribute.SelectedValue = _fillSizeHolesMethod.SG_RID;
                    if (FunctionSecurity.AllowUpdate)
                    {
                        if (cboStoreAttribute.ContinueReadOnly)
                        {
                            SetMethodReadOnly();
                        }
                    }
                    else
                    {
                        cboStoreAttribute.Enabled = false;
                    }
                    // Begin TT#1530 - RMatelic - Record locking violations from Store Attribute change
                    // Begin Track #4872 - JSmith - Global/User Attributes
                    //if (cboStoreAttribute.ContinueReadOnly)
                    //{
                    //    SetMethodReadOnly();
                    //}
                    // End Track #4872
                    if (FunctionSecurity.AllowUpdate)
                    {
                        if (cboStoreAttribute.ContinueReadOnly)
                        {
                            SetMethodReadOnly();
                        }
                    }
                    else
                    {
                        cboStoreAttribute.Enabled = false;
                    }
                    // End TT#1530
                }

                AdjustTextWidthComboBox_DropDown(cboStoreAttribute); //TT#7 - MD - RBeck - Dynamic dropdowns
 
//				if (_fillSizeHolesMethod.SizeGroupRid != Include.NoRID)
//				{
//					cboSizeGroup.SelectedValue = _fillSizeHolesMethod.SizeGroupRid;
//					//BindSizeCurveComboBox(_fillSizeHolesMethod.SizeGroupRid);
//					//cboSizeCurve.SelectedValue = _fillSizeHolesMethod.SizeCurveGroupRid;
//				}

				// begin MID track 3781 Size Curve not required for Fill Size Holes
                if (_fillSizeHolesMethod.SizeGroupRid != Include.NoRID)
                {
                    cboSizeGroup.SelectedValue = _fillSizeHolesMethod.SizeGroupRid;
                    //this.cboSizeGroup_SelectionChangeCommitted(source, new EventArgs()); // TT#301-MD - JSmith - Controls are not functioning properly
                    _fillSizeHolesMethod.GetSizesUsing = eGetSizes.SizeGroupRID;
                    _fillSizeHolesMethod.GetDimensionsUsing = eGetDimensions.SizeGroupRID;
                    cboSizeCurve.SelectedValue = Include.NoRID;
                    //this.cboSizeCurve_SelectionChangeCommitted(source, new EventArgs()); // TT#301-MD - JSmith - Controls are not functioning properly
                    _fillSizeHolesMethod.SizeCurveGroupRid = Include.NoRID;
                    _fillSizeHolesMethod.CreateConstraintData();
                    BindAllSizeGrid(_fillSizeHolesMethod.MethodConstraints);
                    CheckExpandAll();
                }
				// end MID track 3781 Size Curve not required for Fill Size Holes
				
				if (_fillSizeHolesMethod.SizeCurveGroupRid != Include.NoRID)
				{
					cboSizeCurve.SelectedValue = _fillSizeHolesMethod.SizeCurveGroupRid;
                    //this.cboSizeCurve_SelectionChangeCommitted(source, new EventArgs()); // TT#301-MD - JSmith - Controls are not functioning properly
					// begin MID Track 3781 Size Curve not required for Fill Size holes
					_fillSizeHolesMethod.GetSizesUsing = eGetSizes.SizeCurveGroupRID;
					_fillSizeHolesMethod.GetDimensionsUsing = eGetDimensions.SizeCurveGroupRID;
					cboSizeGroup.SelectedValue = Include.NoRID;
                    //this.cboSizeGroup_SelectionChangeCommitted(source, new EventArgs()); // TT#301-MD - JSmith - Controls are not functioning properly
					_fillSizeHolesMethod.SizeGroupRid = Include.NoRID;
					_fillSizeHolesMethod.CreateConstraintData();
					BindAllSizeGrid(_fillSizeHolesMethod.MethodConstraints);
					CheckExpandAll();
					// end MID Track 3781 Size Curve not required for Fill Size holes
				}

				if (_fillSizeHolesMethod.SizeAlternateRid != Include.NoRID)
				{
					cboAlternates.SelectedValue = _fillSizeHolesMethod.SizeAlternateRid;
				}

				if (_fillSizeHolesMethod.SizeConstraintRid != Include.NoRID)
				{
					cboConstraints.SelectedValue = _fillSizeHolesMethod.SizeConstraintRid;
				}

				txtAvailable.Text = _fillSizeHolesMethod.Available.ToString();
							
				if (_fillSizeHolesMethod.PercentInd)
				{
					rbPercent.Checked = true;
					rbUnits.Checked = false;
				}
				else
				{
					rbPercent.Checked = false;
					rbUnits.Checked = true;
				}


				//Load Merchandise Node or Level Text to combo box
				HierarchyNodeProfile hnp;
				if (_fillSizeHolesMethod.MerchHnRid != Include.NoRID)
				{
					//Begin Track #5378 - color and size not qualified
//					hnp = SAB.HierarchyServerSession.GetNodeData(_fillSizeHolesMethod.MerchHnRid);
                    hnp = SAB.HierarchyServerSession.GetNodeData(_fillSizeHolesMethod.MerchHnRid, true, true);
					//End Track #5378
					AddNodeToMerchandiseCombo ( hnp );
				}
				else
				{ 
					if (_fillSizeHolesMethod.MerchPhRid != Include.NoRID)
					{
						SetComboToLevel(_fillSizeHolesMethod.MerchPhlSequence);
					}
					else
					{
						cboMerchandise.SelectedIndex = 0;
                        //this.cboMerchandise_SelectionChangeCommitted(source, new EventArgs()); // TT#301-MD - JSmith - Controls are not functioning properly
					}
				}

                // BEGIN TT#41-MD - GTaylor - UC#2
                if (_fillSizeHolesMethod.IB_MERCH_HN_RID != Include.NoRID)
                {
                    hnp = SAB.HierarchyServerSession.GetNodeData(_fillSizeHolesMethod.IB_MERCH_HN_RID, true, true);
                    AddNodeToIBCombo(hnp);
                }
                else
                {
                    if (_fillSizeHolesMethod.IB_MERCH_PH_RID != Include.NoRID)
                    {
                        SetIBComboToLevel(_fillSizeHolesMethod.IB_MERCH_PHL_SEQ);
                    }
                    else if (_fillSizeHolesMethod.IB_MerchandiseType == eMerchandiseType.OTSPlanLevel)
                    {
                        cboInventoryBasis.SelectedIndex = 1;
                    }
                    else
                    {
                        cboInventoryBasis.SelectedIndex = 0;
                        //this.cboInventoryBasis_SelectionChangeCommitted(source, new EventArgs()); // TT#301-MD - JSmith - Controls are not functioning properly
                    }
                }
                // END TT#41-MD - GTaylor - UC#2

				// begin Generic Size Curve data
				if (_fillSizeHolesMethod.GenCurveCharGroupRID != Include.NoRID)
				{
					this.cboHeaderChar.SelectedValue = _fillSizeHolesMethod.GenCurveCharGroupRID;
                    //this.cboHeaderChar_SelectionChangeCommitted(source, new EventArgs()); // TT#301-MD - JSmith - Controls are not functioning properly
				}

                // Begin TT#413 - RMatelic - Add new Generic Size Curve names to correspond with the new Size Curves being generated within Node Properties settings.
                this.cboNameExtension.SelectedValue = _fillSizeHolesMethod.GenCurveNsccdRID;
                //this.cboNameExtension_SelectionChangeCommitted(source, new EventArgs()); // TT#301-MD - JSmith - Controls are not functioning properly
                // End TT#413 

				switch (_fillSizeHolesMethod.GenCurveMerchType)
				{
					case eMerchandiseType.Undefined:
						cboHierarchyLevel.SelectedIndex = 0;
                        //this.cboHierarchyLevel_SelectionChangeCommitted(source, new EventArgs()); // TT#301-MD - JSmith - Controls are not functioning properly
						break;
				
					case eMerchandiseType.OTSPlanLevel:
					case eMerchandiseType.HierarchyLevel:
						SetGenSizeCurveComboToLevel(_fillSizeHolesMethod.GenCurvePhlSequence);
						break;
					
					case eMerchandiseType.Node:
						//Begin Track #5378 - color and size not qualified
//						hnp = SAB.HierarchyServerSession.GetNodeData(_fillSizeHolesMethod.GenCurveHnRID);
                        hnp = SAB.HierarchyServerSession.GetNodeData(_fillSizeHolesMethod.GenCurveHnRID, true, true);
						//End Track #5378
						AddNodeToGenSizeCurveCombo(hnp);
						break;
					
				}
					
				cbColor.Checked = _fillSizeHolesMethod.GenCurveColorInd;
                // Begin TT#438 - RMatelic - Size Curve generated in node properties- used default in Size Need method but requires a size curve (incorrect)
                if (SAB.ApplicationServerSession.GlobalOptions.GenericSizeCurveNameType == eGenericSizeCurveNameType.NodePropertiesName && cbColor.Checked)
                {
                    cbColor.Checked = false;
                }
                // End TT#438  
				// end Generic Size Curve data

				// begin Generic Size Constraint data
				if (_fillSizeHolesMethod.GenConstraintCharGroupRID != Include.NoRID)
				{
					this.cboConstrHeaderChar.SelectedValue = _fillSizeHolesMethod.GenConstraintCharGroupRID;
				}
					
				switch (_fillSizeHolesMethod.GenConstraintMerchType)
				{
					case eMerchandiseType.Undefined:
						cboConstrHierLevel.SelectedIndex = 0;
						break;
				
					case eMerchandiseType.OTSPlanLevel:
					case eMerchandiseType.HierarchyLevel:
						SetGenSizeConstraintComboToLevel(_fillSizeHolesMethod.GenConstraintPhlSequence);
						break;
					
					case eMerchandiseType.Node:
						hnp = SAB.HierarchyServerSession.GetNodeData(_fillSizeHolesMethod.GenConstraintHnRID);
						AddNodeToGenSizeConstraintCombo(hnp);
						break;
					
				}
					
				cbConstrColor.Checked = _fillSizeHolesMethod.GenConstraintColorInd;
				// end Generic Size Constraint data

				// BEGIN MID Track #4826 - JSmith - Normalize Size Curves
				if (_fillSizeHolesMethod.NormalizeSizeCurves)
				{
					radNormalizeSizeCurves_Yes.Checked = true;
				}
				else
				{
					radNormalizeSizeCurves_No.Checked = true;
				}
				// END MID Track #4826

                // BEGIN TT#246-MD - AGallagher - VSW Size - Add In-store Size Constraint options 
                // Begin TT#5124 - JSmith - Performance
                //GlobalOptions opts = new GlobalOptions();
                //DataTable dt = opts.GetGlobalOptions();
                //DataRow dr = dt.Rows[0];
                // End TT#5124 - JSmith - Performance
                DataTable dtVSWSizeConstrains = MIDText.GetTextType(eMIDTextType.eVSWSizeConstraints, eMIDTextOrderBy.TextCode);
                cboSizeConstraints.DataSource = dtVSWSizeConstrains;
                cboSizeConstraints.DisplayMember = "TEXT_VALUE";
                cboSizeConstraints.ValueMember = "TEXT_CODE";
                // Begin TT#5124 - JSmith - Performance
                //_vswSizeConstraints = (eVSWSizeConstraints)(Convert.ToInt32(dr["VSW_SIZE_CONSTRAINTS"]));
                _vswSizeConstraints = SAB.ClientServerSession.GlobalOptions.VSWSizeConstraints;
                // End TT#5124 - JSmith - Performance
                cboSizeConstraints.SelectedValue = _vswSizeConstraints;

                if (_fillSizeHolesMethod.OverrideVSWSizeConstraints)
                {
                    cboSizeConstraints.Enabled = true;
                    chkVSWSizeConstraintsOverride.Checked = true;
                    cboSizeConstraints.SelectedValue = _fillSizeHolesMethod.VSWSizeConstraints;
                }
                else
                {
                    cboSizeConstraints.Enabled = false;
                    chkVSWSizeConstraintsOverride.Checked = false;
                    cboSizeConstraints.SelectedValue = _fillSizeHolesMethod.VSWSizeConstraints;
                }
                // END TT#246-MD - AGallagher - VSW Size - Add In-store Size Constraint options 



                //Begin TT#1636-MD -jsobek -Pre-Pack Fill Size

                if (_fillSizeHolesMethod.MaxPackNeedTolerance != double.MaxValue)
                {
                    txtMaxPackAllocNeedTolerance.Text = _fillSizeHolesMethod.MaxPackNeedTolerance.ToString();
                }
                if (_fillSizeHolesMethod.AvgPackDeviationTolerance != double.MaxValue)
                {
                    txtAvgPackDevTolerance.Text = _fillSizeHolesMethod.AvgPackDeviationTolerance.ToString();
                }

                if (_fillSizeHolesMethod.OverrideAvgPackDevTolerance)
                {
                    cbxOverrideAvgPackDev.Checked = true;
                    txtAvgPackDevTolerance.Enabled = true;
                }
                else
                {
                    cbxOverrideAvgPackDev.Checked = false;
                    txtAvgPackDevTolerance.Enabled = false;
                }
                // begin TT#1365 - JEllis - FL Detail Pack Size Need Enhancement
                if (_fillSizeHolesMethod.PackToleranceStepped)
                {
                    cbxStepped.Checked = true;
                }
                else
                {
                    cbxStepped.Checked = false;
                }
                if (_fillSizeHolesMethod.PackToleranceNoMaxStep)
                {
                    cbxNoMaxStep.Checked = true;
                }
                else
                {
                    cbxNoMaxStep.Checked = false;
                }
                // end TT#1365 - JEllis - FL Detail Pack Size Need Enhancement
                if (_fillSizeHolesMethod.OverrideMaxPackNeedTolerance)
                {
                    cbxOverrideMaxPackAllocNeed.Checked = true;
                    txtMaxPackAllocNeedTolerance.Enabled = true;
                    // begin TT#1365 - JEllis - FL Detail Packs Size Need Enhancement
                    cbxStepped.Enabled = true;
                    cbxNoMaxStep.Enabled = true;
                    // end TT#1365 - JEllis - FL Detail Packs Size Need Enhancement

                }
                else
                {
                    cbxOverrideMaxPackAllocNeed.Checked = false;
                    txtMaxPackAllocNeedTolerance.Enabled = false;
                    // begin TT#1365 - JEllis - FL Detail Packs Size Need Enhancement
                    cbxStepped.Enabled = false;
                    cbxNoMaxStep.Enabled = false;
                    // end TT#1365 - JEllis - FL Detail Packs Size Need Enhancement
                }

                //End TT#1636-MD -jsobek -Pre-Pack Fill Size

                // begin TT#2155 - JEllis - Fill Size Holes Null Reference
                SetApplyRulesOnly_State();
                // end TT#2155 - JEllis - Fill Size Holes Null Reference
			}
			catch
			{
				MessageBox.Show("Error in Load Fill Size Hole Values");
			}

					
		}


		/// <summary>
		/// Binds ugRules grid on the Constraint tab to FSHDataSet property from the FillSizeHolesMethod
		/// class.
		/// </summary>
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
						cboMerchandise.SelectedIndex = levIndex;
                        //this.cboMerchandise_SelectionChangeCommitted(source, new EventArgs()); // TT#301-MD - JSmith - Controls are not functioning properly
						break;
					}
				}
			}
			catch (Exception ex)
			{
				HandleException(ex, "SetComboToLevel");
			}

		}
        // BEGIN TT#41-MD - GTaylor - UC#2
        private void SetIBComboToLevel(int seq)
        {
            try
            {
                DataRow myDataRow;

                for (int levIndex = 0;

                    levIndex < MerchandiseDataTable3.Rows.Count; levIndex++)
                {
                    myDataRow = MerchandiseDataTable3.Rows[levIndex];
                    if (Convert.ToInt32(myDataRow["seqno"], CultureInfo.CurrentUICulture) == seq)
                    {
                        // BEGIN TT#41-MD -- AGallagher - UC#2
                        // cboInventoryBasis.SelectedIndex = levIndex;
                        cboInventoryBasis.SelectedIndex = levIndex + 1;
                        //this.cboInventoryBasis_SelectionChangeCommitted(source, new EventArgs()); // TT#301-MD - JSmith - Controls are not functioning properly
                        // END TT#41-MD -- AGallagher - UC#2
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                HandleException(ex, "SetIBComboToLevel");
            }
        }
        // END TT#41-MD - GTaylor - UC#2

		// BEGIN MID Track #4921 - JSmith - A&F 666 - Size Modification
		private void radFillSizesTo_Holes_CheckedChanged(object sender, System.EventArgs e)
		{
			if (FormLoaded)
			{
				ChangePending = true;
			}
		}

		private void radFillSizesTo_SizePlan_CheckedChanged(object sender, System.EventArgs e)
		{
			if (FormLoaded)
			{
				ChangePending = true;
			}
		}
		// End MID Track #4921
        //Begin TT#848-MD -jsobek -Fill to Size Plan Presentation
        private void radFillSizesTo_SizePlanWithSizeMin_CheckedChanged(object sender, EventArgs e)
        {
            if (FormLoaded)
            {
                ChangePending = true;
            }
        }
        //End TT#848-MD -jsobek -Fill to Size Plan Presentation


	#endregion Custom Methods

	#region Bind and Clear Comboboxes

		/// <summary>
		/// Clears the merchandise combo box.
		/// </summary>
		private void ClearMerchandiseComboBox()
		{
			cboMerchandise.DataSource = null;
			cboMerchandise.Items.Clear();
            // BEGIN TT#41-MD - GTaylor - UC#2
            cboInventoryBasis.DataSource = null;
            cboInventoryBasis.Items.Clear();
            // END TT#41-MD - GTaylor - UC#2
		}


		/// <summary>
		/// Populates the merchandise combo box.
		/// </summary>
		private void BindMerchandiseComboBox()
		{
			try
			{
				ClearMerchandiseComboBox();
				// begin MID Track 3749 Implement Merchandise Basis "color"
				string sizeLabel = MIDText.GetTextOnly((int)eHierarchyLevelType.Size);
				foreach (System.Data.DataRow dr in MerchandiseDataTable.Rows)
				{
					bool removeRow = false;
					foreach (object obj in dr.ItemArray)
					{
						if (obj.ToString() == sizeLabel)
						{
							removeRow = true;
							break;
						}
					}
					if (removeRow)
					{
						MerchandiseDataTable.Rows.Remove(dr);
						break;
					}
				}
				// end MID Track 3749 Implement Merchandise Basis "color"
				cboMerchandise.DataSource  = MerchandiseDataTable; //Inherited from WorkflowMethodFormBase
				cboMerchandise.DisplayMember = "text";
				cboMerchandise.ValueMember = "seqno";

                // BEGIN TT#41-MD -- GTaylor - UC#2
                MerchandiseDataTable3 = MerchandiseDataTable.Copy();
                // BEGIN TT#41-MD -- AGallagher - UC#2
                DataRow dRow = MerchandiseDataTable3.NewRow();
                dRow["seqno"] = -1;
                dRow["leveltypename"] = eMerchandiseType.Undefined;
                dRow["text"] = string.Empty;
                dRow["key"] = -2;
                MerchandiseDataTable3.Rows.Add(dRow);
                MerchandiseDataTable3.DefaultView.Sort = "seqno";
                // END TT#41-MD -- AGallagher - UC#2
                cboInventoryBasis.DataSource = MerchandiseDataTable3;
                cboInventoryBasis.DisplayMember = "text";
                cboInventoryBasis.ValueMember = "seqno";
                // END TT#41-MD -- GTaylor - UC#2

			}
			catch (Exception ex)
			{
				HandleException(ex, "BindMerchandiseComboBox");
			}
		}

        // BEGIN TT#41-MD - GTaylor - UC#2
        private void Load_IB_Combo()
        {
            //MerchandiseDataTable3 = MerchandiseDataTable.Copy();
            try
            {
                cboInventoryBasis.DataSource = MerchandiseDataTable3;
                cboInventoryBasis.DisplayMember = "text";
                cboInventoryBasis.ValueMember = "seqno";
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

        }
        // END TT#41-MD - GTaylor - UC#2
	#endregion Bind and Clear Comboboxes

	#region WorkflowMethodFormBase Overrides

		/// <summary>
		/// Opens an existing Fill Size Holes Method. 
		/// </summary>
		/// <param name="aMethodRID">method_RID</param>
		/// <param name="aNodeRID"></param>
		/// <param name="aLockStatus">The lock status of the data to be displayed</param>
		override public void UpdateWorkflowMethod(int aMethodRID, int aNodeRID, MIDWorkflowMethodTreeNode aNode, eLockStatus aLockStatus)
		{
			try
			{
				_nodeRID = aNodeRID;
				_fillSizeHolesMethod = new FillSizeHolesMethod(SAB,aMethodRID);
				base.UpdateWorkflowMethod(aLockStatus, eSecurityFunctions.AllocationMethodsUserFillSizeHoles, eSecurityFunctions.AllocationMethodsGlobalFillSizeHoles);

				Common_Load(aNode.GlobalUserType);
			}
			catch(Exception ex)
			{
				HandleException(ex, "UpdateWorkflowMethod");
				FormLoadError = true;
			}
		}


		/// <summary>
		/// Deletes a Fill Size Holes Method.
		/// </summary>
		/// <param name="method_RID">The record ID of the method</param>
		override public bool DeleteWorkflowMethod(int method_RID)
		{
			try
			{       
				_fillSizeHolesMethod = new FillSizeHolesMethod(SAB,method_RID);
				return Delete();
			}
			catch(DatabaseForeignKeyViolation)
			{
				throw;
			}
			catch (Exception ex)
			{
				HandleException(ex, "DeleteWorkflowMethod");
			}

			return true;
		}


		/// <summary>
		/// Gets if workflow or method.
		/// </summary>
		override protected eWorkflowMethodIND WorkflowMethodInd()
		{
			return eWorkflowMethodIND.SizeMethods;	
		}


		/// <summary>
		/// Use to set the method name, description, user and global radio buttons
		/// </summary>
		override protected void SetCommonFields()
		{
			try
			{
				WorkflowMethodName = txtName.Text;
				WorkflowMethodDescription = txtDesc.Text;
				GlobalRadioButton = radGlobal;
				UserRadioButton = radUser;
			}
			catch (Exception ex)
			{
				string message = ex.ToString();
				throw;
			}

		}


		/// <summary>
		/// Use to set the specific fields in method object before updating
		/// </summary>
		override protected void SetSpecificFields()
		{
			try
			{
                // BEGIN TT#41-MD - GTaylor - UC#2
                //Merchandise Level
                // BEGIN TT#41-MD -- AGallagher - UC#2
                //if (_InventoryInd == 'I')
                DataRow myDataRow2 = null;
                for (int i = 0; i < MerchandiseDataTable3.Rows.Count; i++)
                {
                    myDataRow2 = MerchandiseDataTable3.Rows[i];
                    if (Convert.ToInt32(myDataRow2["seqno"], CultureInfo.CurrentUICulture) == Convert.ToInt32(cboInventoryBasis.SelectedValue, CultureInfo.CurrentUICulture))
                    {
                        break;
                    }
                }
                {
                    //DataRow myDataRow2 = MerchandiseDataTable3.Rows[cboInventoryBasis.SelectedIndex];
                    // END TT#41-MD -- AGallagher - UC#2
                    eMerchandiseType MerchandiseType2 = (eMerchandiseType)(Convert.ToInt32(myDataRow2["leveltypename"], CultureInfo.CurrentUICulture));
                   
                    _fillSizeHolesMethod.IB_MerchandiseType = MerchandiseType2;

                    // BEGIN TT#41-MD -- AGallagher - UC#2
                    if (MerchandiseType2 == eMerchandiseType.Undefined)
                    {
                    _fillSizeHolesMethod.IB_MERCH_HN_RID = Include.NoRID;
                    _fillSizeHolesMethod.IB_MERCH_PH_RID = Include.NoRID;
                    _fillSizeHolesMethod.IB_MERCH_PHL_SEQ = 0;
                    }
                    else
                   // END TT#41-MD -- AGallagher - UC#2
                    switch (MerchandiseType2)
                    {
                        case eMerchandiseType.Node:
                            _fillSizeHolesMethod.IB_MERCH_HN_RID = Convert.ToInt32(myDataRow2["key"], CultureInfo.CurrentUICulture);
                            break;
                        case eMerchandiseType.HierarchyLevel:
                            _fillSizeHolesMethod.IB_MERCH_PHL_SEQ = Convert.ToInt32(myDataRow2["key"], CultureInfo.CurrentUICulture);
                            _fillSizeHolesMethod.IB_MERCH_PH_RID = HP.Key;
                            _fillSizeHolesMethod.IB_MERCH_HN_RID = Include.NoRID;
                            break;
                        case eMerchandiseType.OTSPlanLevel:
                            _fillSizeHolesMethod.IB_MERCH_HN_RID = Include.NoRID;
                            _fillSizeHolesMethod.IB_MERCH_PH_RID = Include.NoRID;
                            _fillSizeHolesMethod.IB_MERCH_PHL_SEQ = 0;
                            break;
                    }
                }
                // BEGIN TT#41-MD -- AGallagher - UC#2
                //else
                //{
                //    _fillSizeHolesMethod.IB_MERCH_HN_RID = Include.NoRID;
                //    _fillSizeHolesMethod.IB_MERCH_PH_RID = Include.NoRID;
                //    _fillSizeHolesMethod.IB_MERCH_PHL_SEQ = 0;
                //}
                // END TT#41-MD -- AGallagher - UC#2
                // END TT#41-MD - GTaylor - UC#2

				if (cboFilter.Text.Trim() != string.Empty)
				{
					_fillSizeHolesMethod.StoreFilterRid = ((FilterNameCombo)cboFilter.SelectedItem).FilterRID;
				}
				else
				{
					_fillSizeHolesMethod.StoreFilterRid = Include.NoRID;
				}

//				if (cboSizeGroup.Text.Trim() != string.Empty)
//				{
//					_fillSizeHolesMethod.SizeGroupRid = Convert.ToInt32(cboSizeGroup.SelectedValue.ToString(),CultureInfo.CurrentUICulture);
//				}
//				else
//				{
					//_fillSizeHolesMethod.SizeGroupRid = Include.NoRID; // MID Track 3781 Size Curve not required for Fill Size Holes
//				}
				// begin MID Track 3781 Size Curve not required for Fill Size Holes
				if (cboSizeGroup.Text.Trim() !=string.Empty)
				{
					_fillSizeHolesMethod.SizeGroupRid = Convert.ToInt32(cboSizeGroup.SelectedValue.ToString(), CultureInfo.CurrentUICulture);
				}
				else
				{
					_fillSizeHolesMethod.SizeGroupRid = Include.NoRID;
				}
				// end MID Track 3781 Size Curve not required for Fill Size Holes

				if (cboSizeCurve.Text.Trim() != string.Empty)
				{
					_fillSizeHolesMethod.SizeCurveGroupRid = Convert.ToInt32(cboSizeCurve.SelectedValue.ToString(), CultureInfo.CurrentUICulture);
				}
				else
				{
					_fillSizeHolesMethod.SizeCurveGroupRid = Include.NoRID;
				}

				_fillSizeHolesMethod.Available = Convert.ToDouble(txtAvailable.Text.ToString(),CultureInfo.CurrentUICulture);
				_fillSizeHolesMethod.PercentInd = rbPercent.Checked;

				if (cboStoreAttribute.Text.Trim() != string.Empty)
				{
					_fillSizeHolesMethod.SG_RID = Convert.ToInt32(cboStoreAttribute.SelectedValue.ToString(),CultureInfo.CurrentUICulture);
				}
				else
				{
					_fillSizeHolesMethod.SG_RID = Include.NoRID;
				}

				if (cboAlternates.Text.Trim() != string.Empty)
				{
					_fillSizeHolesMethod.SizeAlternateRid = Convert.ToInt32(cboAlternates.SelectedValue.ToString(),CultureInfo.CurrentUICulture);
				}
				else
				{
					_fillSizeHolesMethod.SizeAlternateRid = Include.NoRID;
				}

				if (cboConstraints.Text.Trim() != string.Empty)
				{
					_fillSizeHolesMethod.SizeConstraintRid = Convert.ToInt32(cboConstraints.SelectedValue.ToString(),CultureInfo.CurrentUICulture);
				}
				else
				{
					_fillSizeHolesMethod.SizeConstraintRid = Include.NoRID;
				}


				//Merchandise Level
				DataRow myDataRow = MerchandiseDataTable.Rows[cboMerchandise.SelectedIndex];
				eMerchandiseType MerchandiseType = (eMerchandiseType)(Convert.ToInt32(myDataRow["leveltypename"], CultureInfo.CurrentUICulture)); 
				_fillSizeHolesMethod.MerchandiseType = MerchandiseType;
				
				switch(MerchandiseType)
				{
					case eMerchandiseType.Node:
						_fillSizeHolesMethod.MerchHnRid = Convert.ToInt32(myDataRow["key"], CultureInfo.CurrentUICulture);
						break;
					case eMerchandiseType.HierarchyLevel:
						_fillSizeHolesMethod.MerchPhlSequence = Convert.ToInt32(myDataRow["key"], CultureInfo.CurrentUICulture);
						_fillSizeHolesMethod.MerchPhRid = HP.Key;
						_fillSizeHolesMethod.MerchHnRid = Include.NoRID;
						break;
					case eMerchandiseType.OTSPlanLevel:
						_fillSizeHolesMethod.MerchHnRid = Include.NoRID;
						_fillSizeHolesMethod.MerchPhRid = Include.NoRID;
						_fillSizeHolesMethod.MerchPhlSequence = 0;
						break;
				}

				// begin Generic Size Curve data
				_fillSizeHolesMethod.GenCurveCharGroupRID =  Convert.ToInt32(cboHeaderChar.SelectedValue, CultureInfo.CurrentUICulture);
				_fillSizeHolesMethod.GenCurveColorInd = cbColor.Checked; 
				DataRowView drv = (DataRowView)cboHierarchyLevel.SelectedItem;
				DataRow dRow  = drv.Row;
				 
				_fillSizeHolesMethod.GenCurveMerchType = (eMerchandiseType)(Convert.ToInt32(dRow["leveltypename"], CultureInfo.CurrentUICulture)); 
				switch(_fillSizeHolesMethod.GenCurveMerchType)
				{
					case eMerchandiseType.Node:
						_fillSizeHolesMethod.GenCurveHnRID = Convert.ToInt32(dRow["key"], CultureInfo.CurrentUICulture);
						_fillSizeHolesMethod.GenCurvePhRID = Include.NoRID;
						_fillSizeHolesMethod.GenCurvePhlSequence = 0;
						break;
					case eMerchandiseType.HierarchyLevel:
						_fillSizeHolesMethod.GenCurvePhlSequence  = Convert.ToInt32(dRow["seqno"], CultureInfo.CurrentUICulture);
						_fillSizeHolesMethod.GenCurvePhRID = HP.Key;
						_fillSizeHolesMethod.GenCurveHnRID = Include.NoRID;
						break;
					case eMerchandiseType.OTSPlanLevel:
					case eMerchandiseType.Undefined:
						_fillSizeHolesMethod.GenCurveHnRID = Include.NoRID;
						_fillSizeHolesMethod.GenCurvePhRID = Include.NoRID;
						_fillSizeHolesMethod.GenCurvePhlSequence  = 0;
						break;
				}
				// end  Generic Size Curve data

				// begin Generic Size Constraint data
				_fillSizeHolesMethod.GenConstraintCharGroupRID =  Convert.ToInt32(cboConstrHeaderChar.SelectedValue, CultureInfo.CurrentUICulture);
				_fillSizeHolesMethod.GenConstraintColorInd = cbConstrColor.Checked; 
				
				drv = (DataRowView)cboConstrHierLevel.SelectedItem;
				dRow  = drv.Row;
				 
				_fillSizeHolesMethod.GenConstraintMerchType = (eMerchandiseType)(Convert.ToInt32(dRow["leveltypename"], CultureInfo.CurrentUICulture)); 
				switch(_fillSizeHolesMethod.GenConstraintMerchType)
				{
					case eMerchandiseType.Node:
						_fillSizeHolesMethod.GenConstraintHnRID = Convert.ToInt32(dRow["key"], CultureInfo.CurrentUICulture);
						_fillSizeHolesMethod.GenConstraintPhRID = Include.NoRID;
						_fillSizeHolesMethod.GenConstraintPhlSequence = 0;
						break;
					case eMerchandiseType.HierarchyLevel:
						_fillSizeHolesMethod.GenConstraintPhlSequence  = Convert.ToInt32(dRow["seqno"], CultureInfo.CurrentUICulture);
						_fillSizeHolesMethod.GenConstraintPhRID = HP.Key;
						_fillSizeHolesMethod.GenConstraintHnRID = Include.NoRID;
						break;
					case eMerchandiseType.OTSPlanLevel:
					case eMerchandiseType.Undefined:
						_fillSizeHolesMethod.GenConstraintHnRID = Include.NoRID;
						_fillSizeHolesMethod.GenConstraintPhRID = Include.NoRID;
						_fillSizeHolesMethod.GenConstraintPhlSequence  = 0;
						break;
				}
				// end  Generic Size Constraint data

				// BEGIN MID Track #4826 - JSmith - Normalize Size Curves
				_fillSizeHolesMethod.NormalizeSizeCurvesDefaultIsOverridden = cbxOverrideNormalizeDefault.Checked;
				_fillSizeHolesMethod.NormalizeSizeCurves = radNormalizeSizeCurves_Yes.Checked;
				// END MID Track #4826
				// BEGIN MID Track #4921 - JSmith - A&F 666 - Size Modification
				if (this.radFillSizesTo_SizePlan.Checked)
				{
					_fillSizeHolesMethod.FillSizesToType = eFillSizesToType.SizePlan;
				}
                else if (this.radFillSizesTo_SizePlanWithSizeMins.Checked) //TT#848-MD -jsobek -Fill to Size Plan Presentation
                {
                    _fillSizeHolesMethod.FillSizesToType = eFillSizesToType.SizePlanWithMins;
                }
				else
				{
					_fillSizeHolesMethod.FillSizesToType = eFillSizesToType.Holes;
				}
				// End MID Track #4921

                // Begin TT#413 - JSmith - Add new Generic Size Curve names to correspond with the new Size Curves being generated within Node Properties settings.
                _fillSizeHolesMethod.UseDefaultCurve = cbxUseDefaultcurve.Checked;
                // End TT#413

                // begin TT#2155 - JEllis - Fill Size Holes Null Reference
                _fillSizeHolesMethod.ApplyRulesOnly = cbxApplyRulesOnly.Checked;
                // end TT#2155 - JEllis - Fill Size Holes Null Reference

                // Begin TT#413 - RMatelic - Add new Generic Size Curve names to correspond with the new Size Curves being generated within Node Properties settings.
                if (SAB.ApplicationServerSession.GlobalOptions.GenericSizeCurveNameType == eGenericSizeCurveNameType.NodePropertiesName)
                {
                    _fillSizeHolesMethod.GenCurveNsccdRID = Convert.ToInt32(cboNameExtension.SelectedValue, CultureInfo.CurrentUICulture);
                    _fillSizeHolesMethod.GenCurveCharGroupRID = Include.NoRID;
                }
                else
                {
                    _fillSizeHolesMethod.GenCurveNsccdRID = Include.NoRID;
                }

                // BEGIN TT#246-MD - AGallagher - VSW Size - Add In-store Size Constraint options 
                _fillSizeHolesMethod.OverrideVSWSizeConstraints = chkVSWSizeConstraintsOverride.Checked;
                _fillSizeHolesMethod.VSWSizeConstraints = (eVSWSizeConstraints)cboSizeConstraints.SelectedValue;
                // END TT#246-MD - AGallagher - VSW Size - Add In-store Size Constraint options 

                // End TT#413


                //Begin TT#1636-MD -jsobek -Pre-Pack Fill Size
                if (cbxOverrideAvgPackDev.Checked)
                {
                    _fillSizeHolesMethod.OverrideAvgPackDevTolerance = true;
                    if (txtAvgPackDevTolerance.Text.Trim() == string.Empty)
                    {
                        _fillSizeHolesMethod.AvgPackDeviationTolerance = Double.MaxValue;
                    }
                    else
                    {
                        _fillSizeHolesMethod.AvgPackDeviationTolerance = Convert.ToDouble(txtAvgPackDevTolerance.Text, CultureInfo.CurrentUICulture);
                    }
                }
                else
                {
                    _fillSizeHolesMethod.OverrideAvgPackDevTolerance = false;
                    _fillSizeHolesMethod.AvgPackDeviationTolerance = SAB.ApplicationServerSession.GlobalOptions.PackSizeErrorPercent;
                }
                
                if (cbxOverrideMaxPackAllocNeed.Checked)
                {
                    _fillSizeHolesMethod.OverrideMaxPackNeedTolerance = true;
                    if (txtMaxPackAllocNeedTolerance.Text.Trim() == string.Empty)
                    {
                        _fillSizeHolesMethod.MaxPackNeedTolerance = Double.MaxValue;
                    }
                    else
                    {
                        _fillSizeHolesMethod.MaxPackNeedTolerance = Convert.ToDouble(txtMaxPackAllocNeedTolerance.Text, CultureInfo.CurrentUICulture);
                    }
                    // begin TT#1365 - JEllis - FL Detail Packs Size Need Enhancement
                    _fillSizeHolesMethod.PackToleranceNoMaxStep = cbxNoMaxStep.Checked;
                    _fillSizeHolesMethod.PackToleranceStepped = cbxStepped.Checked;
                    // end TT#1365 - JEllis - FL Detail Packs Size Need Enhancement
                }
                else
                {
                    _fillSizeHolesMethod.OverrideMaxPackNeedTolerance = false;
                    _fillSizeHolesMethod.MaxPackNeedTolerance = SAB.ApplicationServerSession.GlobalOptions.MaxSizeErrorPercent;
                    // begin TT#1365 - JEllis - FL Detail Packs Size Need Enhancement
                    _fillSizeHolesMethod.PackToleranceNoMaxStep = SAB.ApplicationServerSession.GlobalOptions.PackToleranceNoMaxStep;
                    _fillSizeHolesMethod.PackToleranceStepped = SAB.ApplicationServerSession.GlobalOptions.PackToleranceStepped;
                    // end TT#1365 - JEllis - FL Detail Packs Size Need Enhancement
                }
                //End TT#1636-MD -jsobek -Pre-Pack Fill Size

			}
			catch (Exception ex)
			{
				string message = ex.ToString();
				throw;
			}

		}


		/// <summary>
		/// Use to validate the fields that are specific to this method type
		/// </summary>
		override protected bool ValidateSpecificFields()
		{				
			bool isFormValid = true;

			try
			{
				//initialize all fields to not having an error
				//ErrorMessages.Clear();
				AttachErrors(txtAvailable);
				AttachErrors(rbUnits);
                // Begin TT#438 - RMatelic - Size Curve generated in node properties- used default in Size Need method but requires a size curve (incorrect)
                //AttachErrors(cboSizeCurve);
                AttachErrors(gbSizeCurve);
                // End TT#438  

				if (txtAvailable.Text.Trim() == string.Empty)
				{
					//Available should be populated.
					isFormValid = false;
					ErrorMessages.Add(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_FieldIsRequired));
					AttachErrors(txtAvailable);
					//ErrorMessages.Clear();
				}
				else
				{
					//Percent or units should be selected if available is populated.
					if (!rbPercent.Checked && !rbUnits.Checked)
					{
						isFormValid = false;
						ErrorMessages.Add(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MissingUnitsPercent));
						AttachErrors(rbUnits);
						//ErrorMessages.Clear();

					}

					//When percent is selected, available should not be > 100 or < 0
					if (rbPercent.Checked)
					{
						if (Convert.ToDouble(txtAvailable.Text,CultureInfo.CurrentUICulture) > 100 || Convert.ToDouble(txtAvailable.Text,CultureInfo.CurrentUICulture) < 0)
						{
							isFormValid = false;
							ErrorMessages.Add(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MustBeBetween0And100));
							AttachErrors(txtAvailable);
							//ErrorMessages.Clear();
						}
					}
				}

                // Begin TT#438 - RMatelic - Size Curve generated in node properties- used default in Size Need method but requires a size curve (incorrect)
                //if (cboSizeCurve.Text.Trim() == string.Empty && cboSizeGroup.Text.Trim() == string.Empty) // MID Track 3781 Size curve  not required
                //{
                //    isFormValid = false;
                //    ErrorMessages.Add(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_FieldIsRequired));
                //    AttachErrors(cboSizeCurve);
                //    AttachErrors(cboSizeGroup); // MID Track 3781 Size Curve not required
                //}
                if (cboSizeCurve.Text.Trim() == string.Empty && cboSizeGroup.Text.Trim() == string.Empty && !cbxUseDefaultcurve.Checked &&
                    Convert.ToInt32(cboHierarchyLevel.SelectedValue, CultureInfo.CurrentUICulture) == -2 &&
                    Convert.ToInt32(cboHeaderChar.SelectedValue, CultureInfo.CurrentUICulture) == Include.NoRID &&
                    Convert.ToInt32(cboNameExtension.SelectedValue, CultureInfo.CurrentUICulture) == Include.NoRID && !cbColor.Checked)  
                {
                    isFormValid = false;
                    ErrorMessages.Add(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_FieldIsRequired));
                    AttachErrors(gbSizeCurve);
                }
                // End TT#438 
                // begin TT#1365 - JEllis - FL Detail Pack Size Need Enhancement
                ErrorMessages.Clear();
                AttachErrors(txtMaxPackAllocNeedTolerance);
                if (cbxStepped.Checked)
                {
                    if (txtMaxPackAllocNeedTolerance.Text.Trim() == string.Empty
                        || Convert.ToDouble(txtMaxPackAllocNeedTolerance.Text, CultureInfo.CurrentUICulture) > Include.MaxPackNeedTolerance)
                    {
                        isFormValid = false;
                        ErrorMessages.Add(string.Format(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_al_ValueTooLargeWhenSteppedActive), Include.MaxPackNeedTolerance, cbxStepped.Text));
                        AttachErrors(txtMaxPackAllocNeedTolerance);
                    }
                }
                // end TT#1365 - JEllis - FL Detail Pack Size Need Enhancement
				if (!IsGridValid())
				{
					isFormValid = false;
				}
			}
			catch (Exception)
			{
				throw;
			}

			return isFormValid;
		}


		/// <summary>
		/// Use to set the errors to the screen
		/// </summary>
		override protected void HandleErrors()
		{
			//ErrorMessages.Clear();
			try
			{
				if (!WorkflowMethodNameValid)
				{
					ErrorMessages.Add(WorkflowMethodNameMessage);
					AttachErrors(txtName);
				}
				else
				{
					AttachErrors(txtName);
				}

				//ErrorMessages.Clear();
				if (!WorkflowMethodDescriptionValid)
				{
					ErrorMessages.Add(WorkflowMethodDescriptionMessage);
					AttachErrors(txtDesc);
				}
				else
				{	
					AttachErrors(txtDesc);
				}

				//ErrorMessages.Clear();
				if (!UserGlobalValid)
				{
					ErrorMessages.Add(UserGlobalMessage);
					AttachErrors(pnlGlobalUser);
				}
				else
				{
					AttachErrors(pnlGlobalUser);
				}

			}
			catch (Exception ex)
			{
				string message = ex.ToString();
				throw;
			}
		}

		override protected void InitializeValueLists(UltraGrid myGrid)
		{
			myGrid.DisplayLayout.ValueLists.Add("Colors");
			myGrid.DisplayLayout.ValueLists.Add("Rules");
			//myGrid.DisplayLayout.ValueLists.Add("SortOrder");
			myGrid.DisplayLayout.ValueLists.Add("Sizes");
			//myGrid.DisplayLayout.ValueLists.Add("DimensionMaster");
			myGrid.DisplayLayout.ValueLists.Add("Dimensions");
			myGrid.DisplayLayout.ValueLists.Add("DimensionCell");
			myGrid.DisplayLayout.ValueLists.Add("SizeCell");
			//Begin MID Track # 2936 - stodd
			myGrid.DisplayLayout.ValueLists.Add("ColorCell");
			//End MID track # 2936
			//Begin MID Track # 3685 - stodd
			//myGrid.DisplayLayout.ValueLists["SizeCell"].SortStyle = ValueListSortStyle.Ascending;
			//End MID Track # 3685 - stodd

			FillColorList(myGrid.DisplayLayout.ValueLists["Colors"]);


			base.FillRulesList(myGrid.DisplayLayout.ValueLists["Rules"], true);  // MID Track 3620 Change Way Size rules work 
			//
			//			FillSortList(myGrid.DisplayLayout.ValueLists["SortOrder"],
			//				eMIDTextType.eFillSizeHolesSort,
			//				eMIDTextOrderBy.TextValue);

			//Fill the size lists if there is a size selected.
			if (_fillSizeHolesMethod.SizeCurveGroupRid != Include.NoRID)
			{
				FillSizesList(myGrid.DisplayLayout.ValueLists["Sizes"],
					_fillSizeHolesMethod.SizeCurveGroupRid, _fillSizeHolesMethod.GetSizesUsing);

				FillSizeDimensionList(myGrid.DisplayLayout.ValueLists["Dimensions"],
					_fillSizeHolesMethod.SizeCurveGroupRid, _fillSizeHolesMethod.GetDimensionsUsing);
			}
			// begin MID Track 3781 Size Curve not required for Fill Size Holes
			if (_fillSizeHolesMethod.SizeGroupRid != Include.NoRID)
			{
				FillSizesList(myGrid.DisplayLayout.ValueLists["Sizes"],
					_fillSizeHolesMethod.SizeGroupRid, _fillSizeHolesMethod.GetSizesUsing);
				FillSizeDimensionList(myGrid.DisplayLayout.ValueLists["Dimensions"],
					_fillSizeHolesMethod.SizeGroupRid, _fillSizeHolesMethod.GetDimensionsUsing);
			}
			// end MID Track 3781 Size Curve not required for Fill Size Holes
		} 


		/// <summary>
		/// Use to set the specific method object before updating
		/// </summary>
		override protected void SetObject()
		{
			try
			{
				ABM = _fillSizeHolesMethod;
			}
			catch
			{
				throw;
			}
		}


		/// <summary>
		/// Use to return the explorer node selected when form was opened
		/// </summary>
		override protected MIDWorkflowMethodTreeNode GetExplorerNode()
		{
			return ExplorerNode;
		}


		/// <summary>
		/// Opens a new Fill Size Holes Method. 
		/// </summary>
		override public void NewWorkflowMethod(MIDWorkflowMethodTreeNode aParentNode)
		{
			try
			{
				_fillSizeHolesMethod = new FillSizeHolesMethod(SAB,Include.NoRID);
				SetObject();
				base.NewWorkflowMethod(aParentNode, eSecurityFunctions.AllocationMethodsUserFillSizeHoles, eSecurityFunctions.AllocationMethodsGlobalFillSizeHoles);
				// Store Attribute to allocation default
				GlobalOptionsProfile gop = new GlobalOptionsProfile(-1);
				gop.LoadOptions();
				_fillSizeHolesMethod.SG_RID = gop.AllocationStoreGroupRID;
				gop = null;
				
				// Begin issue 3779
				Common_Load(aParentNode.GlobalUserType);
				// End issue 3779

			}
			catch(Exception ex)
			{
				HandleException(ex, "NewWorkflowMethod");
				FormLoadError = true;
			}
		}


		/// <summary>
		/// Renames a Fill Size Holes Method.
		/// </summary>
		/// <param name="aMethodRID">The record ID of the method</param>
		/// <param name="aNewName">The new name of the workflow or method</param>
		override public bool RenameWorkflowMethod(int aMethodRID, string aNewName)
		{
			try
			{       
				_fillSizeHolesMethod = new FillSizeHolesMethod(SAB,aMethodRID);
				return Rename(aNewName);
			}
			catch (Exception ex)
			{
				HandleException(ex, "RenameWorkflowMethod");
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
				_fillSizeHolesMethod = new FillSizeHolesMethod(SAB,aMethodRID);

				ProcessAction(eMethodType.FillSizeHolesAllocation, true);
			}
			catch (Exception ex)
			{
				HandleException(ex);
				FormLoadError = true;
			}
		}


	#endregion WorkflowMethodFormBase Overrides

	#region MIDFormBase Overrides
		/// <summary>
		/// Determines if the contraint data should be restored for this method.
		/// </summary>
		override protected void AfterClosing()
		{
			try
			{
				if (ResultSaveChanges != DialogResult.None)
				{
					if (ResultSaveChanges == DialogResult.No)
					{
						//ONLY ROLLBACK IF UPDATING THE METHOD
						if (_fillSizeHolesMethod.Method_Change_Type == eChangeType.update)
						{
							if (ConstraintRollback)
							{
								_fillSizeHolesMethod.MethodConstraints = DataSetBackup;
								_fillSizeHolesMethod.InsertUpdateMethodRules(new TransactionData());
							}	
						}
					}
				}
			}
			catch (Exception ex)
			{
				HandleException(ex, "frmFillSizeHolesMethod.AfterClosing");
			}
		}

	#endregion MIDFormBase Overrides

	#region SizeMethodsFormBase Overrides


		override protected bool IsActiveRowValid(UltraGridRow activeRow)
		{
			try
			{
				bool IsValid = true;
				
				if (!IsSizeRuleValid(activeRow))
				{
					IsValid = false;
				}
				//ADD ADDITIONAL COMMON VALIDATIONS HERE

				//=================================================================================


				//BAND SPECIFIC VALIDATIONS
				//=================================================================================
				switch (activeRow.Band.Key.ToUpper())
				{
					case C_SET:
						break;
					case C_SET_CLR:
						if (!IsColorCodeValid(activeRow))
						{
							IsValid = false;
						}
						break;
					case C_CLR_SZ:
					case C_ALL_CLR_SZ:
						if (!IsSizeValid(activeRow))
						{
							IsValid = false;
						}
						break;
					case C_CLR_SZ_DIM:
					case C_ALL_CLR_SZ_DIM:
						if (!IsDimensionValid(activeRow))
						{
							IsValid = false;
						}
						break;
				}	

				return IsValid;
			}
			catch (Exception ex)
			{
				HandleException(ex, "IsActiveRowValid");
				return false;
			}
		}

		override protected void PositionColumns()
		{
			try
			{
				UltraGridColumn column;

				#region SET BAND

				if (ugRules.DisplayLayout.Bands.Exists(C_SET))
				{
					column = ugRules.DisplayLayout.Bands[C_SET].Columns["BAND_DSC"];
					column.Header.VisiblePosition = 0;
					column.Header.Caption = "Store Sets";
					column.Width = 200;

					

					column = ugRules.DisplayLayout.Bands[C_SET].Columns["SIZE_RULE"];
					column.Header.VisiblePosition = 4;
					column.Header.Caption = "Rule";

					// begin MID Track 3620 Change way size rules work
					column = ugRules.DisplayLayout.Bands[C_SET].Columns["SIZE_QUANTITY"];
					column.Header.VisiblePosition = 5;
					column.Header.Caption = "Qty";
					// end MID Track 3620 Change way size rules work

					ugRules.DisplayLayout.Bands[C_SET].Columns["SGL_RID"].Hidden = true;
					ugRules.DisplayLayout.Bands[C_SET].Columns["METHOD_RID"].Hidden = true;
					ugRules.DisplayLayout.Bands[C_SET].Columns["ROW_TYPE_ID"].Hidden = true;
					//ugRules.DisplayLayout.Bands[C_SET].Columns["SIZE_QUANTITY"].Hidden = true;  // MID Track 3620 Change way size rules work

					ugRules.DisplayLayout.Bands[C_SET].Override.AllowDelete = DefaultableBoolean.False;
					ugRules.DisplayLayout.Bands[C_SET].AddButtonCaption = "Set";
				}
				#endregion


				#region ALL COLOR BAND

				if (ugRules.DisplayLayout.Bands.Exists(C_SET_ALL_CLR))
				{
					ugRules.DisplayLayout.Bands[C_SET_ALL_CLR].Columns["BAND_DSC"].Header.VisiblePosition = 0;
					
					ugRules.DisplayLayout.Bands[C_SET_ALL_CLR].Columns["SIZE_RULE"].Header.VisiblePosition = 1;
					ugRules.DisplayLayout.Bands[C_SET_ALL_CLR].Columns["SIZE_QUANTITY"].Header.VisiblePosition = 2; // MID Track 3620 Change way size rules work

					ugRules.DisplayLayout.Bands[C_SET_ALL_CLR].Columns["METHOD_RID"].Hidden = true;
					ugRules.DisplayLayout.Bands[C_SET_ALL_CLR].Columns["SGL_RID"].Hidden = true;
					ugRules.DisplayLayout.Bands[C_SET_ALL_CLR].Columns["ROW_TYPE_ID"].Hidden = true;
					ugRules.DisplayLayout.Bands[C_SET_ALL_CLR].Columns["COLOR_CODE_RID"].Hidden = true;
					ugRules.DisplayLayout.Bands[C_SET_ALL_CLR].Columns["SIZES_RID"].Hidden = true;
					ugRules.DisplayLayout.Bands[C_SET_ALL_CLR].Columns["SIZE_CODE_RID"].Hidden = true;
					ugRules.DisplayLayout.Bands[C_SET_ALL_CLR].Columns["DIMENSIONS_RID"].Hidden = true;
					// Begin Issue 3826 - stodd
					//ugRules.DisplayLayout.Bands[C_SET_ALL_CLR].Columns["SIZE_QUANTITY"].Hidden = true;
					// End issue 3826
					ugRules.DisplayLayout.Bands[C_SET_ALL_CLR].Columns["SIZE_SEQ"].Hidden = true;
					
					//ugRules.DisplayLayout.Bands[C_SET_ALL_CLR].Columns["SIZE_QUANTITY"].Hidden = true; // MID Track 3620 Change way size rules work

					ugRules.DisplayLayout.Bands[C_SET_ALL_CLR].ColHeadersVisible = false;
					ugRules.DisplayLayout.Bands[C_SET_ALL_CLR].Override.AllowDelete = DefaultableBoolean.False;
					ugRules.DisplayLayout.Bands[C_SET_ALL_CLR].AddButtonCaption = "All Color";
				}
				#endregion


				#region COLOR BAND
				if (ugRules.DisplayLayout.Bands.Exists(C_SET_CLR))
				{
					ugRules.DisplayLayout.Bands[C_SET_CLR].Columns["COLOR_CODE_RID"].Header.VisiblePosition = 0;
					ugRules.DisplayLayout.Bands[C_SET_CLR].Columns["SIZE_RULE"].Header.VisiblePosition = 1;
					ugRules.DisplayLayout.Bands[C_SET_CLR].Columns["SIZE_QUANTITY"].Header.VisiblePosition = 2; // MID track 3620 change way size rules work

					ugRules.DisplayLayout.Bands[C_SET_CLR].Columns["SGL_RID"].Hidden = true;
					ugRules.DisplayLayout.Bands[C_SET_CLR].Columns["METHOD_RID"].Hidden = true;
					ugRules.DisplayLayout.Bands[C_SET_CLR].Columns["ROW_TYPE_ID"].Hidden = true;
					ugRules.DisplayLayout.Bands[C_SET_CLR].Columns["SIZES_RID"].Hidden = true;
					ugRules.DisplayLayout.Bands[C_SET_CLR].Columns["SIZE_CODE_RID"].Hidden = true;
					ugRules.DisplayLayout.Bands[C_SET_CLR].Columns["DIMENSIONS_RID"].Hidden = true;
					ugRules.DisplayLayout.Bands[C_SET_CLR].Columns["BAND_DSC"].Hidden = true;
					//ugRules.DisplayLayout.Bands[C_SET_CLR].Columns["SIZE_QUANTITY"].Hidden = true; // MID Track 3620 Change way size rules work
					ugRules.DisplayLayout.Bands[C_SET_CLR].Columns["SIZE_SEQ"].Hidden = true;

					ugRules.DisplayLayout.Bands[C_SET_CLR].ColHeadersVisible = false;
					ugRules.DisplayLayout.Bands[C_SET_CLR].AddButtonCaption = "Color";
				}
				#endregion


				#region COLOR SIZE BAND
				if (ugRules.DisplayLayout.Bands.Exists(C_CLR_SZ))
				{

					column = ugRules.DisplayLayout.Bands[C_CLR_SZ].Columns["SIZE_CODE_RID"];
					column.Header.VisiblePosition = 0;

					column = ugRules.DisplayLayout.Bands[C_CLR_SZ].Columns["SIZE_SEQ"];
					column.SortIndicator = SortIndicator.Ascending;

					ugRules.DisplayLayout.Bands[C_CLR_SZ].Columns["SIZE_RULE"].Header.VisiblePosition = 1;
					ugRules.DisplayLayout.Bands[C_CLR_SZ].Columns["SIZE_QUANTITY"].Header.VisiblePosition = 2; // MID Track 3620 Change way size rules work

					ugRules.DisplayLayout.Bands[C_CLR_SZ].Columns["DIMENSIONS_RID"].Hidden = true;
					ugRules.DisplayLayout.Bands[C_CLR_SZ].Columns["SIZES_RID"].Hidden = true;
					ugRules.DisplayLayout.Bands[C_CLR_SZ].Columns["METHOD_RID"].Hidden = true;
					ugRules.DisplayLayout.Bands[C_CLR_SZ].Columns["SGL_RID"].Hidden = true;
					ugRules.DisplayLayout.Bands[C_CLR_SZ].Columns["ROW_TYPE_ID"].Hidden = true;
					ugRules.DisplayLayout.Bands[C_CLR_SZ].Columns["COLOR_CODE_RID"].Hidden = true;
					ugRules.DisplayLayout.Bands[C_CLR_SZ].Columns["BAND_DSC"].Hidden = true;
					//ugRules.DisplayLayout.Bands[C_CLR_SZ].Columns["SIZE_QUANTITY"].Hidden = true; // MID Track 3620 Change way Size rule works
					ugRules.DisplayLayout.Bands[C_CLR_SZ].Columns["SIZE_SEQ"].Hidden = true;

					ugRules.DisplayLayout.Bands[C_CLR_SZ].ColHeadersVisible = false;
					ugRules.DisplayLayout.Bands[C_CLR_SZ].AddButtonCaption = "Size";
				}
				#endregion


				#region ALL COLOR SIZE BAND
				if (ugRules.DisplayLayout.Bands.Exists(C_ALL_CLR_SZ))
				{
					column = ugRules.DisplayLayout.Bands[C_ALL_CLR_SZ].Columns["SIZE_CODE_RID"];
					column.Header.VisiblePosition = 0;
					 
					column = ugRules.DisplayLayout.Bands[C_ALL_CLR_SZ].Columns["SIZE_SEQ"];
					column.SortIndicator = SortIndicator.Ascending;

					ugRules.DisplayLayout.Bands[C_ALL_CLR_SZ].Columns["SIZE_RULE"].Header.VisiblePosition = 1;
					ugRules.DisplayLayout.Bands[C_ALL_CLR_SZ].Columns["SIZE_QUANTITY"].Header.VisiblePosition = 2; // MID Track 3620 Change way size rules work

					ugRules.DisplayLayout.Bands[C_ALL_CLR_SZ].Columns["SIZES_RID"].Hidden = true;
					ugRules.DisplayLayout.Bands[C_ALL_CLR_SZ].Columns["METHOD_RID"].Hidden = true;
					ugRules.DisplayLayout.Bands[C_ALL_CLR_SZ].Columns["SGL_RID"].Hidden = true;
					ugRules.DisplayLayout.Bands[C_ALL_CLR_SZ].Columns["DIMENSIONS_RID"].Hidden = true;
					ugRules.DisplayLayout.Bands[C_ALL_CLR_SZ].Columns["ROW_TYPE_ID"].Hidden = true;
					ugRules.DisplayLayout.Bands[C_ALL_CLR_SZ].Columns["COLOR_CODE_RID"].Hidden = true;
					ugRules.DisplayLayout.Bands[C_ALL_CLR_SZ].Columns["BAND_DSC"].Hidden = true;
					//ugRules.DisplayLayout.Bands[C_ALL_CLR_SZ].Columns["SIZE_QUANTITY"].Hidden = true; // MID Track 3620 Change way size rules work
					ugRules.DisplayLayout.Bands[C_ALL_CLR_SZ].Columns["SIZE_SEQ"].Hidden = true;

					ugRules.DisplayLayout.Bands[C_ALL_CLR_SZ].ColHeadersVisible = false;
					ugRules.DisplayLayout.Bands[C_ALL_CLR_SZ].AddButtonCaption = "Size";
				}
				#endregion


				#region ALL COLOR DIMENSION BAND
				if (ugRules.DisplayLayout.Bands.Exists(C_ALL_CLR_SZ_DIM))
				{
					column = ugRules.DisplayLayout.Bands[C_ALL_CLR_SZ_DIM].Columns["DIMENSIONS_RID"];
					column.Header.VisiblePosition = 0;

					column = ugRules.DisplayLayout.Bands[C_ALL_CLR_SZ_DIM].Columns["SIZE_SEQ"];
					column.SortIndicator = SortIndicator.Ascending;

					ugRules.DisplayLayout.Bands[C_ALL_CLR_SZ_DIM].Columns["SIZE_RULE"].Header.VisiblePosition = 1;
					ugRules.DisplayLayout.Bands[C_ALL_CLR_SZ_DIM].Columns["SIZE_QUANTITY"].Header.VisiblePosition = 2; // MID Track 3620 Change way size rules work

					ugRules.DisplayLayout.Bands[C_ALL_CLR_SZ_DIM].Columns["METHOD_RID"].Hidden = true;
					ugRules.DisplayLayout.Bands[C_ALL_CLR_SZ_DIM].Columns["SIZES_RID"].Hidden = true;
					ugRules.DisplayLayout.Bands[C_ALL_CLR_SZ_DIM].Columns["SIZE_CODE_RID"].Hidden = true;
					ugRules.DisplayLayout.Bands[C_ALL_CLR_SZ_DIM].Columns["SGL_RID"].Hidden = true;
					ugRules.DisplayLayout.Bands[C_ALL_CLR_SZ_DIM].Columns["ROW_TYPE_ID"].Hidden = true;
					ugRules.DisplayLayout.Bands[C_ALL_CLR_SZ_DIM].Columns["COLOR_CODE_RID"].Hidden = true;
					ugRules.DisplayLayout.Bands[C_ALL_CLR_SZ_DIM].Columns["BAND_DSC"].Hidden = true;
					//ugRules.DisplayLayout.Bands[C_ALL_CLR_SZ_DIM].Columns["SIZE_QUANTITY"].Hidden = true; // MID Track 3620 Change way size rules work
					ugRules.DisplayLayout.Bands[C_ALL_CLR_SZ_DIM].Columns["SIZE_SEQ"].Hidden = true;

					ugRules.DisplayLayout.Bands[C_ALL_CLR_SZ_DIM].ColHeadersVisible = false;
					ugRules.DisplayLayout.Bands[C_ALL_CLR_SZ_DIM].AddButtonCaption = "Size Dimension";
					
				}
				#endregion


				#region COLOR DIMENSION BAND
				if (ugRules.DisplayLayout.Bands.Exists(C_CLR_SZ_DIM))
				{
					column = ugRules.DisplayLayout.Bands[C_CLR_SZ_DIM].Columns["DIMENSIONS_RID"];
					column.Header.VisiblePosition = 0;

					column = ugRules.DisplayLayout.Bands[C_CLR_SZ_DIM].Columns["SIZE_SEQ"];
					column.SortIndicator = SortIndicator.Ascending;

					ugRules.DisplayLayout.Bands[C_CLR_SZ_DIM].Columns["SIZE_RULE"].Header.VisiblePosition = 1;
					ugRules.DisplayLayout.Bands[C_CLR_SZ_DIM].Columns["SIZE_QUANTITY"].Header.VisiblePosition = 2; // MID Track 3620 Change way size rules work


					ugRules.DisplayLayout.Bands[C_CLR_SZ_DIM].Columns["METHOD_RID"].Hidden = true;
					ugRules.DisplayLayout.Bands[C_CLR_SZ_DIM].Columns["SIZES_RID"].Hidden = true;
					ugRules.DisplayLayout.Bands[C_CLR_SZ_DIM].Columns["SIZE_CODE_RID"].Hidden = true;
					ugRules.DisplayLayout.Bands[C_CLR_SZ_DIM].Columns["SGL_RID"].Hidden = true;
					ugRules.DisplayLayout.Bands[C_CLR_SZ_DIM].Columns["ROW_TYPE_ID"].Hidden = true;
					ugRules.DisplayLayout.Bands[C_CLR_SZ_DIM].Columns["COLOR_CODE_RID"].Hidden = true;
					ugRules.DisplayLayout.Bands[C_CLR_SZ_DIM].Columns["BAND_DSC"].Hidden = true;
					//ugRules.DisplayLayout.Bands[C_CLR_SZ_DIM].Columns["SIZE_QUANTITY"].Hidden = true; // MID Track 3620 Change way size rules work
					ugRules.DisplayLayout.Bands[C_CLR_SZ_DIM].Columns["SIZE_SEQ"].Hidden = true;

					ugRules.DisplayLayout.Bands[C_CLR_SZ_DIM].ColHeadersVisible = false;
					ugRules.DisplayLayout.Bands[C_CLR_SZ_DIM].AddButtonCaption = "Size Dimension";
				}
				#endregion
			}
			catch (Exception ex)
			{
				HandleException(ex, "SizeMethodFormBase.PositionColumns");
			}
		}

		override protected bool IsGridValid()
		{
			try
			{
				bool IsValid = true;

				//================================================================================
				//WALK THE GRID - VALIDATING EACH ROW
				//================================================================================
				foreach(UltraGridRow setRow in ugRules.Rows)
				{
					if (!IsActiveRowValid(setRow))
					{
						IsValid = false;
					}	

					if (setRow.HasChild())
					{
						//ALL COLORS ROW
						//===============
						foreach(UltraGridRow allColorRow in setRow.ChildBands[C_SET_ALL_CLR].Rows)
						{
							if (!IsActiveRowValid(allColorRow))
							{
								IsValid = false;
							}

							if (allColorRow.HasChild())
							{

								//ALL COLOR DIMENSION ROWS
								foreach (UltraGridRow allColorDimRow in allColorRow.ChildBands[C_ALL_CLR_SZ_DIM].Rows)
								{

									if (!IsActiveRowValid(allColorDimRow))
									{
										IsValid = false;
									}

									//ALL COLOR DIMENSION/SIZE ROWS
									if (allColorDimRow.HasChild())
									{
										foreach (UltraGridRow allColorSizeRow in allColorDimRow.ChildBands[C_ALL_CLR_SZ].Rows)
										{
											if (!IsActiveRowValid(allColorSizeRow))
											{
												IsValid = false;
											}
										}	
									}
								}
							}
						}
						//========================================================================


						//COLOR ROWS 
						//===========
						foreach(UltraGridRow colorRow in setRow.ChildBands[C_SET_CLR].Rows)
						{
							if (!IsActiveRowValid(colorRow))
							{
								IsValid = false;
							}

							if (colorRow.HasChild())
							{
								//COLOR SIZE
								//=============
								foreach (UltraGridRow colorDimRow in colorRow.ChildBands[C_CLR_SZ_DIM].Rows)
								{
									if (!IsActiveRowValid(colorDimRow))
									{
										IsValid = false;
									}

									if (colorDimRow.HasChild())
									{
										//COLOR SIZE DIMENSION
										//======================
										foreach (UltraGridRow colorSizeRow in colorDimRow.ChildBands[C_CLR_SZ].Rows)
										{
											if (!IsActiveRowValid(colorSizeRow))
											{
												IsValid = false;
											}
										}
									}
								}
							}
						}
					}
				}

				if (!IsValid) 
				{ 
					ugRules.Rows.ExpandAll(true);
					EditByCell = true;
				}

				return IsValid;
			}
			catch (Exception ex)
			{
				HandleException(ex, "IsGridValid");
				return false;
			}
		}

//
//
//		override protected void ClearSetData(UltraGridRow activeRow)
//		{
//			try
//			{
//				activeRow.Cells["SIZE_MIN"].Value = string.Empty;
//				activeRow.Cells["SIZE_MAX"].Value = string.Empty;
//				activeRow.Cells["SIZE_MULT"].Value = string.Empty;
//				activeRow.Cells["SIZE_QUANTITY"].Value = string.Empty;
//				activeRow.Cells["SIZE_RULE"].Value = string.Empty;
//				activeRow.Cells["FZ_IND"].Value = false;
//				activeRow.Cells["FSH_IND"].Value = false;
//				activeRow.Cells["FILL_ZEROS_QUANTITY"].Value = string.Empty;
//				activeRow.Cells["FILL_SEQUENCE"].Value = Convert.ToInt32(eFillSizeHolesSort.Ascending, CultureInfo.CurrentUICulture);
//				
//				if (activeRow.HasChild())
//				{
//					//COPY TO ALL COLOR ROW
//					foreach(UltraGridRow allColorRow in activeRow.ChildBands[0].Rows)
//					{
//						allColorRow.Cells["SIZE_MIN"].Value = string.Empty;
//						allColorRow.Cells["SIZE_MAX"].Value = string.Empty;
//						allColorRow.Cells["SIZE_MULT"].Value = string.Empty;
//						allColorRow.Cells["SIZE_QUANTITY"].Value = string.Empty;
//						allColorRow.Cells["SIZE_RULE"].Value = string.Empty;
//
//						if (allColorRow.HasChild())
//						{
//							foreach (UltraGridRow allColorDimRow in allColorRow.ChildBands[0].Rows)
//							{
//								allColorDimRow.Cells["SIZE_MIN"].Value = string.Empty;
//								allColorDimRow.Cells["SIZE_MAX"].Value = string.Empty;
//								allColorDimRow.Cells["SIZE_MULT"].Value = string.Empty;
//								allColorDimRow.Cells["SIZE_QUANTITY"].Value = string.Empty;
//								allColorDimRow.Cells["SIZE_RULE"].Value = string.Empty;
//
//								if (allColorDimRow.HasChild())
//								{
//									foreach (UltraGridRow allColorSizeRow in allColorDimRow.ChildBands[0].Rows)
//									{
//										allColorSizeRow.Cells["SIZE_MIN"].Value = string.Empty;
//										allColorSizeRow.Cells["SIZE_MAX"].Value = string.Empty;
//										allColorSizeRow.Cells["SIZE_MULT"].Value = string.Empty;
//										allColorSizeRow.Cells["SIZE_QUANTITY"].Value = string.Empty;
//										allColorSizeRow.Cells["SIZE_RULE"].Value = string.Empty;									}
//								}
//							}
//						}
//					}
//
//					//COPY TO COLOR
//					foreach(UltraGridRow colorRow in activeRow.ChildBands[1].Rows)
//					{
//						colorRow.Cells["SIZE_MIN"].Value = string.Empty;
//						colorRow.Cells["SIZE_MAX"].Value = string.Empty;
//						colorRow.Cells["SIZE_MULT"].Value = string.Empty;
//						colorRow.Cells["SIZE_QUANTITY"].Value = string.Empty;
//						colorRow.Cells["SIZE_RULE"].Value = string.Empty;
//
//
//						if (colorRow.HasChild())
//						{
//							foreach (UltraGridRow colorDimRow in colorRow.ChildBands[0].Rows)
//							{
//								colorDimRow.Cells["SIZE_MIN"].Value = string.Empty;
//								colorDimRow.Cells["SIZE_MAX"].Value = string.Empty;
//								colorDimRow.Cells["SIZE_MULT"].Value = string.Empty;
//								colorDimRow.Cells["SIZE_QUANTITY"].Value = string.Empty;
//								colorDimRow.Cells["SIZE_RULE"].Value = string.Empty;
//
//								if (colorDimRow.HasChild())
//								{
//									foreach (UltraGridRow colorSizeRow in colorDimRow.ChildBands[0].Rows)
//									{
//										colorSizeRow.Cells["SIZE_MIN"].Value = string.Empty;
//										colorSizeRow.Cells["SIZE_MAX"].Value = string.Empty;
//										colorSizeRow.Cells["SIZE_MULT"].Value = string.Empty;
//										colorSizeRow.Cells["SIZE_QUANTITY"].Value = string.Empty;
//										colorSizeRow.Cells["SIZE_RULE"].Value = string.Empty;
//									}
//								}
//							}
//						}
//					}
//				}
//				activeRow.Update();
//			}
//			catch (Exception ex)
//			{
//				HandleException(ex, "ClearSetData");
//			}
//
//		}
//
//
//		override protected void CopyAllSizeData(UltraGridRow activeRow)
//		{
//			try
//			{
//				foreach(UltraGridRow setRow in ugRules.Rows)
//				{
//					if ((eSizeMethodRowType)Convert.ToInt32(setRow.Cells["ROW_TYPE_ID"].Value,CultureInfo.CurrentUICulture) != eSizeMethodRowType.AllSize)
//					{
//						setRow.Cells["SIZE_MIN"].Value = activeRow.Cells["SIZE_MIN"].Value;
//						setRow.Cells["SIZE_MAX"].Value = activeRow.Cells["SIZE_MAX"].Value;
//						setRow.Cells["SIZE_MULT"].Value = activeRow.Cells["SIZE_MULT"].Value;
//						setRow.Cells["SIZE_QUANTITY"].Value = activeRow.Cells["SIZE_QUANTITY"].Value;
//						setRow.Cells["SIZE_RULE"].Value = activeRow.Cells["SIZE_RULE"].Value;
//						setRow.Cells["FZ_IND"].Value = activeRow.Cells["FZ_IND"].Value;
//						setRow.Cells["FSH_IND"].Value = activeRow.Cells["FSH_IND"].Value;
//						setRow.Cells["FILL_ZEROS_QUANTITY"].Value = activeRow.Cells["FILL_ZEROS_QUANTITY"].Value;
//						setRow.Cells["FILL_SEQUENCE"].Value = activeRow.Cells["FILL_SEQUENCE"].Value;
//
//						if (setRow.HasChild())
//						{
//							//ALL COLORS PATH
//							//========================================================================
//							foreach(UltraGridRow allColorRow in setRow.ChildBands[0].Rows)
//							{
//								allColorRow.Cells["SIZE_MIN"].Value = activeRow.Cells["SIZE_MIN"].Value;
//								allColorRow.Cells["SIZE_MAX"].Value = activeRow.Cells["SIZE_MAX"].Value;
//								allColorRow.Cells["SIZE_MULT"].Value = activeRow.Cells["SIZE_MULT"].Value;
//								allColorRow.Cells["SIZE_QUANTITY"].Value = activeRow.Cells["SIZE_QUANTITY"].Value;
//								allColorRow.Cells["SIZE_RULE"].Value = activeRow.Cells["SIZE_RULE"].Value;
//
//								if (allColorRow.HasChild())
//								{
//									foreach (UltraGridRow allColorDimRow in allColorRow.ChildBands[0].Rows)
//									{
//										allColorDimRow.Cells["SIZE_MIN"].Value = activeRow.Cells["SIZE_MIN"].Value;
//										allColorDimRow.Cells["SIZE_MAX"].Value = activeRow.Cells["SIZE_MAX"].Value;
//										allColorDimRow.Cells["SIZE_MULT"].Value = activeRow.Cells["SIZE_MULT"].Value;
//										allColorDimRow.Cells["SIZE_QUANTITY"].Value = activeRow.Cells["SIZE_QUANTITY"].Value;
//										allColorDimRow.Cells["SIZE_RULE"].Value = activeRow.Cells["SIZE_RULE"].Value;
//
//										if (allColorDimRow.HasChild())
//										{
//											foreach (UltraGridRow allColorSizeRow in allColorDimRow.ChildBands[0].Rows)
//											{
//												allColorSizeRow.Cells["SIZE_MIN"].Value = activeRow.Cells["SIZE_MIN"].Value;
//												allColorSizeRow.Cells["SIZE_MAX"].Value = activeRow.Cells["SIZE_MAX"].Value;
//												allColorSizeRow.Cells["SIZE_MULT"].Value = activeRow.Cells["SIZE_MULT"].Value;
//												allColorSizeRow.Cells["SIZE_QUANTITY"].Value = activeRow.Cells["SIZE_QUANTITY"].Value;
//												allColorSizeRow.Cells["SIZE_RULE"].Value = activeRow.Cells["SIZE_RULE"].Value;
//											}
//										}
//									}
//								}
//							}
//
//
//
//							foreach(UltraGridRow colorRow in setRow.ChildBands[1].Rows)
//							{
//								colorRow.Cells["SIZE_MIN"].Value = activeRow.Cells["SIZE_MIN"].Value;
//								colorRow.Cells["SIZE_MAX"].Value = activeRow.Cells["SIZE_MAX"].Value;
//								colorRow.Cells["SIZE_MULT"].Value = activeRow.Cells["SIZE_MULT"].Value;
//								colorRow.Cells["SIZE_QUANTITY"].Value = activeRow.Cells["SIZE_QUANTITY"].Value;
//								colorRow.Cells["SIZE_RULE"].Value = activeRow.Cells["SIZE_RULE"].Value;
//
//								if (colorRow.HasChild())
//								{
//									foreach (UltraGridRow colorDimRow in colorRow.ChildBands[0].Rows)
//									{
//										colorDimRow.Cells["SIZE_MIN"].Value = activeRow.Cells["SIZE_MIN"].Value;
//										colorDimRow.Cells["SIZE_MAX"].Value = activeRow.Cells["SIZE_MAX"].Value;
//										colorDimRow.Cells["SIZE_MULT"].Value = activeRow.Cells["SIZE_MULT"].Value;
//										colorDimRow.Cells["SIZE_QUANTITY"].Value = activeRow.Cells["SIZE_QUANTITY"].Value;
//										colorDimRow.Cells["SIZE_RULE"].Value = activeRow.Cells["SIZE_RULE"].Value;
//
//										if (colorDimRow.HasChild())
//										{
//											foreach (UltraGridRow colorSizeRow in colorDimRow.ChildBands[0].Rows)
//											{
//												colorSizeRow.Cells["SIZE_MIN"].Value = activeRow.Cells["SIZE_MIN"].Value;
//												colorSizeRow.Cells["SIZE_MAX"].Value = activeRow.Cells["SIZE_MAX"].Value;
//												colorSizeRow.Cells["SIZE_MULT"].Value = activeRow.Cells["SIZE_MULT"].Value;
//												colorSizeRow.Cells["SIZE_QUANTITY"].Value = activeRow.Cells["SIZE_QUANTITY"].Value;
//												colorSizeRow.Cells["SIZE_RULE"].Value = activeRow.Cells["SIZE_RULE"].Value;
//											}
//										}
//									}
//								}
//							}
//						}
//					}
//				}
//				ugRules.UpdateData();
//			}
//			catch (Exception ex)
//			{
//				HandleException(ex, "CopyAllSizeData");
//			}
//		}
//
//
//		override protected void ClearAllSizeData(UltraGridRow activeRow)
//		{
//			try
//			{
//				foreach(UltraGridRow setRow in ugRules.Rows)
//				{
//
//					setRow.Cells["SIZE_MIN"].Value = string.Empty;
//					setRow.Cells["SIZE_MAX"].Value = string.Empty;
//					setRow.Cells["SIZE_MULT"].Value = string.Empty;
//					setRow.Cells["SIZE_QUANTITY"].Value = string.Empty;
//					setRow.Cells["SIZE_RULE"].Value = string.Empty;
//					setRow.Cells["FZ_IND"].Value = false;
//					setRow.Cells["FSH_IND"].Value = false;
//					setRow.Cells["FILL_ZEROS_QUANTITY"].Value = string.Empty;
//					setRow.Cells["FILL_SEQUENCE"].Value = Convert.ToInt32(eFillSizeHolesSort.Ascending, CultureInfo.CurrentUICulture);
//
//					if (setRow.HasChild())
//					{
//						//ALL COLORS PATH
//						//========================================================================
//						foreach(UltraGridRow allColorRow in setRow.ChildBands[0].Rows)
//						{
//							allColorRow.Cells["SIZE_MIN"].Value = string.Empty;
//							allColorRow.Cells["SIZE_MAX"].Value = string.Empty;
//							allColorRow.Cells["SIZE_MULT"].Value = string.Empty;
//							allColorRow.Cells["SIZE_QUANTITY"].Value = string.Empty;
//							allColorRow.Cells["SIZE_RULE"].Value = string.Empty;
//
//							if (allColorRow.HasChild())
//							{
//								foreach (UltraGridRow allColorDimRow in allColorRow.ChildBands[0].Rows)
//								{
//									allColorDimRow.Cells["SIZE_MIN"].Value = string.Empty;
//									allColorDimRow.Cells["SIZE_MAX"].Value = string.Empty;
//									allColorDimRow.Cells["SIZE_MULT"].Value = string.Empty;
//									allColorDimRow.Cells["SIZE_QUANTITY"].Value = string.Empty;
//									allColorDimRow.Cells["SIZE_RULE"].Value = string.Empty;
//
//									if (allColorDimRow.HasChild())
//									{
//										foreach (UltraGridRow allColorSizeRow in allColorDimRow.ChildBands[0].Rows)
//										{
//											allColorSizeRow.Cells["SIZE_MIN"].Value = string.Empty;
//											allColorSizeRow.Cells["SIZE_MAX"].Value = string.Empty;
//											allColorSizeRow.Cells["SIZE_MULT"].Value = string.Empty;
//											allColorSizeRow.Cells["SIZE_QUANTITY"].Value = string.Empty;
//											allColorSizeRow.Cells["SIZE_RULE"].Value = string.Empty;
//										}
//									}
//								}
//							}
//						}
//
//
//						foreach(UltraGridRow colorRow in setRow.ChildBands[1].Rows)
//						{
//							colorRow.Cells["SIZE_MIN"].Value = string.Empty;
//							colorRow.Cells["SIZE_MAX"].Value = string.Empty;
//							colorRow.Cells["SIZE_MULT"].Value = string.Empty;
//							colorRow.Cells["SIZE_QUANTITY"].Value = string.Empty;
//							colorRow.Cells["SIZE_RULE"].Value = string.Empty;
//
//							if (colorRow.HasChild())
//							{
//								foreach (UltraGridRow colorDimRow in colorRow.ChildBands[0].Rows)
//								{
//									colorDimRow.Cells["SIZE_MIN"].Value = string.Empty;
//									colorDimRow.Cells["SIZE_MAX"].Value = string.Empty;
//									colorDimRow.Cells["SIZE_MULT"].Value = string.Empty;
//									colorDimRow.Cells["SIZE_QUANTITY"].Value = string.Empty;
//									colorDimRow.Cells["SIZE_RULE"].Value = string.Empty;
//
//									if (colorDimRow.HasChild())
//									{
//										foreach (UltraGridRow colorSizeRow in colorDimRow.ChildBands[0].Rows)
//										{
//											colorSizeRow.Cells["SIZE_MIN"].Value = string.Empty;
//											colorSizeRow.Cells["SIZE_MAX"].Value = string.Empty;
//											colorSizeRow.Cells["SIZE_MULT"].Value = string.Empty;
//											colorSizeRow.Cells["SIZE_QUANTITY"].Value = string.Empty;
//											colorSizeRow.Cells["SIZE_RULE"].Value = string.Empty;
//										}
//									}
//								}
//							}
//						}
//					}
//				}
//				ugRules.UpdateData();
//			}
//			catch (Exception ex)
//			{
//				HandleException(ex, "ClearAllSizeData");
//			}
//		}


	#endregion

	#region IFormBase Overrides
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
		//			catch(Exception ex)
		//			{
		//				MessageBox.Show(ex.Message);
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
			catch(Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
			finally
			{
				this.Cursor = Cursors.Default;
			}
		}

        // Begin Track #4872 - JSmith - Global/User Attributes
        private void frmFillSizeHolesMethod_Load(object sender, EventArgs e)
        {
            if (cboStoreAttribute.ReplaceAttribute)
            {
                ChangePending = true;
            }
        }
        // End Track #4872

        // BEGIN TT#246-MD - AGallagher - VSW Size - Add In-store Size Constraint options 
        private void chkVSWSizeConstraintsOverride_CheckedChanged(object sender, EventArgs e)
        {
            if (!chkVSWSizeConstraintsOverride.Checked)
            {
                cboSizeConstraints.Enabled = false;
                cboSizeConstraints.SelectedValue = SAB.ApplicationServerSession.GlobalOptions.VSWSizeConstraints;
            }
            else
            {
                cboSizeConstraints.Enabled = true;
                cboSizeConstraints.SelectedValue = _fillSizeHolesMethod.VSWSizeConstraints;
            }
        }

      

        // END TT#246-MD - AGallagher - VSW Size - Add In-store Size Constraint options 
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
		
		
	}
}
