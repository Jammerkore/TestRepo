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
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;

namespace MIDRetail.Windows
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class frmSizeNeedMethod : SizeMethodsFormBase
	{

		#region Member Variables
			private int _nodeRID = -1;
//			private string _strMethodType;
			//private DataTable _dtSizes;
			private SizeNeedMethod _sizeNeedMethod;
			//private bool _usingFringe = false; // MID Track 3619 Remove Fringe
	//		private AllocationProfile _basisHeaderProfile = null;
	//		private AllocationProfile _activeHeaderProfile = null;
	//		private DataTable _ColorDataTable = null;
	//		private DataTable _RulesDataTable = null;
		private bool _textChanged = false;
		private bool _priorError = false;
		private int _lastMerchIndex = -1;
		#endregion

		#region Form Fields

			private System.Windows.Forms.TabControl tabControl1;
			private System.Windows.Forms.TabPage tabMethods;
			private System.Windows.Forms.TabPage tabProperties;
            private System.Windows.Forms.Label lblMerchandise;
            private System.Windows.Forms.TextBox txtAvgPackDevTolerance;
			private System.Windows.Forms.TextBox txtMaxPackAllocNeedTolerance;
			private System.Windows.Forms.GroupBox grpSizedPacksOnly;
		#endregion
		private Infragistics.Win.UltraWinGrid.UltraGrid ugWorkflows;
		private System.Windows.Forms.TabControl tabControl2;
		private System.Windows.Forms.TabPage tabPageGeneral;
		private System.Windows.Forms.TabPage tabPageRule;
        private GroupBox gbAvgPackDevTolerance;
        private GroupBox gbMaxPackAllocNeedTolerance;
        private CheckBox cbxOverrideMaxPackAllocNeed;
        private CheckBox cbxOverrideAvgPackDev;
        private CheckBox cbxNoMaxStep;
        private CheckBox cbxStepped;
        private MIDComboBoxEnh cboMerchandise;
        // Begin TT#41-MD -- GTaylor - UC #2
        DataTable MerchandiseDataTable3;
        private bool _textChangedIB = false;
        private bool _priorErrorIB = false;
        private int _lastMerchIndexIB = -1;
        private GroupBox grpVSWSizeConstraints;
        private MIDComboBoxEnh cboSizeConstraints;
        private CheckBox chkVSWSizeConstraintsOverride;
        // End TT#41-MD -- GTaylor - UC #2
        private eVSWSizeConstraints _vswSizeConstraints;  // TT#246-MD - AGallagher - VSW Size - Add In-store Size Constraint options

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		#region Constructor/Dispose
			public frmSizeNeedMethod(SessionAddressBlock SAB, ExplorerAddressBlock aEAB) : base (SAB, aEAB, eMIDTextCode.frm_SizeNeedMethod, eWorkflowMethodType.Method)
			{
				try
				{
					AllowDragDrop = true;
					//
					// Required for Windows Form Designer support
					//
					InitializeComponent();

					UserSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AllocationMethodsUserSizeNeed);
					GlobalSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AllocationMethodsGlobalSizeNeed);
				}
				catch
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

					if (_sizeNeedMethod != null)
					{
						_sizeNeedMethod = null;
					}

                    // Begin TT#301-MD - JSmith - Controls are not functioning properly
                    //this.cboSizeGroup.SelectionChangeCommitted -= new System.EventHandler(this.cboSizeGroup_SelectionChangeCommitted);
                    //this.cboSizeCurve.SelectionChangeCommitted -= new System.EventHandler(this.cboSizeCurve_SelectionChangeCommitted);
                    //this.cboStoreAttribute.SelectionChangeCommitted -= new System.EventHandler(this.cboStoreAttribute_SelectionChangeCommitted);
                    //this.cboSizeGroup.SelectionChangeCommitted -= new System.EventHandler(this.cboSizeGroup_SelectionChangeCommitted);
                    //this.cboSizeCurve.SelectionChangeCommitted -= new System.EventHandler(this.cboSizeCurve_SelectionChangeCommitted);
                    //this.cboStoreAttribute.SelectionChangeCommitted -= new System.EventHandler(this.cboStoreAttribute_SelectionChangeCommitted);
                    // End TT#301-MD - JSmith - Controls are not functioning properly
                    // begin TT#2155 - JEllis - Fill Size Holes Null Reference
                    this.cbxUseDefaultcurve.CheckedChanged -= new System.EventHandler(this.cbxUseDefaultCurve_CheckChanged);
                    // Begin TT#301-MD - JSmith - Controls are not functioning properly
                    //this.cboHierarchyLevel.SelectionChangeCommitted -= new System.EventHandler(this.cboHierarchyLevel_SelectionChangeCommitted);
                    //this.cboHeaderChar.SelectionChangeCommitted -= new System.EventHandler(this.cboHeaderChar_SelectionChangeCommitted);
                    //this.cboNameExtension.SelectionChangeCommitted -= new System.EventHandler(this.cboNameExtension_SelectionChangeCommitted);
                    //this.cboHierarchyLevel.SelectionChangeCommitted -= new System.EventHandler(this.cboHierarchyLevel_SelectionChangeCommitted);
                    //this.cboHeaderChar.SelectionChangeCommitted -= new System.EventHandler(this.cboHeaderChar_SelectionChangeCommitted);
                    //this.cboNameExtension.SelectionChangeCommitted -= new System.EventHandler(this.cboNameExtension_SelectionChangeCommitted);
                    // End TT#301-MD - JSmith - Controls are not functioning properly
                    // end TT#2155 - JEllis - Fill Size Holes Null Reference
                    //this.cboStoreAttribute.DragDrop -= new System.Windows.Forms.DragEventHandler(this.cboStoreAttribute_DragDrop);
                    //this.cboStoreAttribute.DragEnter -= new System.Windows.Forms.DragEventHandler(this.cboStoreAttribute_DragEnter);
					// Begin MID Track 4858 - JSmith - Security changes
//					this.txtName.TextChanged -= new System.EventHandler(this.txtName_TextChanged);
//					this.txtDesc.TextChanged -= new System.EventHandler(this.txtDesc_TextChanged);
//					this.radUser.CheckedChanged -= new System.EventHandler(this.radUser_CheckedChanged);
//					this.radGlobal.CheckedChanged -= new System.EventHandler(this.radGlobal_CheckedChanged);
					// End MID Track 4858
                    this.txtAvgPackDevTolerance.TextChanged -= new System.EventHandler(this.txtAvgPackDevTolerance_TextChanged);
                    this.txtMaxPackAllocNeedTolerance.TextChanged -= new System.EventHandler(this.txtMaxPackAllocNeedTolerance_TextChanged);
					this.cboMerchandise.DragDrop -= new System.Windows.Forms.DragEventHandler(this.cboMerchandise_DragDrop);
                    // Begin TT#301-MD - JSmith - Controls are not functioning properly
                    //this.cboMerchandise.SelectionChangeCommitted -= new System.EventHandler(this.cboMerchandise_SelectionChangeCommitted);
                    this.cboMerchandise.SelectionChangeCommitted -= new System.EventHandler(this.cboMerchandise_SelectionChangeCommitted);
                    // End TT#301-MD - JSmith - Controls are not functioning properly
					this.cboMerchandise.DragEnter -= new System.Windows.Forms.DragEventHandler(this.cboMerchandise_DragEnter);
					this.cboMerchandise.DragOver -= new System.Windows.Forms.DragEventHandler(this.cboMerchandise_DragOver);
					this.cboMerchandise.Validating -=new CancelEventHandler(cboMerchandise_Validating);
					this.cboMerchandise.Validated -= new EventHandler(cboMerchandise_Validated);
					this.cboMerchandise.KeyDown -= new KeyEventHandler(cboMerchandise_KeyDown);
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
					//this.cboFringe.SelectionChangeCommitted -= new System.EventHandler(this.cboFringe_SelectionChangeCommitted); // MID Track 3619 Remove Fringe
                    // Begin TT#301-MD - JSmith - Controls are not functioning properly
                    //this.cboConstraints.SelectionChangeCommitted -= new System.EventHandler(this.cboConstraints_SelectionChangeCommitted);
                    //this.cboAlternates.SelectionChangeCommitted -= new System.EventHandler(this.cboAlternatives_SelectionChangeCommitted);
                    //this.cboConstraints.SelectionChangeCommitted -= new System.EventHandler(this.cboConstraints_SelectionChangeCommitted);
                    //this.cboAlternates.SelectionChangeCommitted -= new System.EventHandler(this.cboAlternatives_SelectionChangeCommitted);
                    //this.ugRules.InitializeLayout -= new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.ugRules_InitializeLayout);
                    ////Begin TT#169 - JSmith - enter max of 1000 and receive 'must be integer'
                    MIDRetail.Windows.Controls.UltraGridLayoutDefaults ugld = new MIDRetail.Windows.Controls.UltraGridLayoutDefaults(ErrorImage);
                    //ugld.DetachGridEventHandlers(ugRules);
                    ////End TT#169
                    // End TT#301-MD - JSmith - Controls are not functioning properly
                    this.ugWorkflows.InitializeLayout -= new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.ugWorkflows_InitializeLayout);
                    //Begin TT#169 - JSmith - enter max of 1000 and receive 'must be integer'
                    ugld.DetachGridEventHandlers(ugWorkflows);
                    //End TT#169
                    // begin TT#1365 - JEllis - FL Detail Pack Size Need Enhancement
                    cbxNoMaxStep.CheckedChanged -= new EventHandler(cbxNoMaxStep_CheckedChanged);
                    cbxStepped.CheckedChanged -= new EventHandler(cbxStepped_CheckedChanged);
                    // end TT#1365 - JEllis - FL Detail Pack Size Need Enhancement
                    // BEGIN TT#246-MD - AGallagher - VSW Size - Add In-store Size Constraint options 
                    this.chkVSWSizeConstraintsOverride.CheckedChanged -= new System.EventHandler(this.chkVSWSizeConstraintsOverride_CheckedChanged);
                    // END TT#246-MD - AGallagher - VSW Size - Add In-store Size Constraint options 
					// Begin MID Track 4858 - JSmith - Security changes
//					this.btnProcess.Click -= new System.EventHandler(this.btnProcess_Click);
//					this.btnSave.Click -= new System.EventHandler(this.btnSave_Click);
//					this.btnClose.Click -= new System.EventHandler(this.btnClose_Click);
					// End MID Track 4858

                    this.cboHierarchyLevel.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler(cboHierarchyLevel_MIDComboBoxPropertiesChangedEvent);
                    this.cboMerchandise.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler(cboMerchandise_MIDComboBoxPropertiesChangedEvent);
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabMethods = new System.Windows.Forms.TabPage();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tabPageGeneral = new System.Windows.Forms.TabPage();
            this.grpVSWSizeConstraints = new System.Windows.Forms.GroupBox();
            this.cboSizeConstraints = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this.chkVSWSizeConstraintsOverride = new System.Windows.Forms.CheckBox();
            this.cboMerchandise = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this.lblMerchandise = new System.Windows.Forms.Label();
            this.grpSizedPacksOnly = new System.Windows.Forms.GroupBox();
            this.gbMaxPackAllocNeedTolerance = new System.Windows.Forms.GroupBox();
            this.cbxStepped = new System.Windows.Forms.CheckBox();
            this.cbxOverrideMaxPackAllocNeed = new System.Windows.Forms.CheckBox();
            this.txtMaxPackAllocNeedTolerance = new System.Windows.Forms.TextBox();
            this.cbxNoMaxStep = new System.Windows.Forms.CheckBox();
            this.gbAvgPackDevTolerance = new System.Windows.Forms.GroupBox();
            this.cbxOverrideAvgPackDev = new System.Windows.Forms.CheckBox();
            this.txtAvgPackDevTolerance = new System.Windows.Forms.TextBox();
            this.tabPageRule = new System.Windows.Forms.TabPage();
            this.tabProperties = new System.Windows.Forms.TabPage();
            this.ugWorkflows = new Infragistics.Win.UltraWinGrid.UltraGrid();
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
            this.tabControl1.SuspendLayout();
            this.tabMethods.SuspendLayout();
            this.tabControl2.SuspendLayout();
            this.tabPageGeneral.SuspendLayout();
            this.grpVSWSizeConstraints.SuspendLayout();
            this.grpSizedPacksOnly.SuspendLayout();
            this.gbMaxPackAllocNeedTolerance.SuspendLayout();
            this.gbAvgPackDevTolerance.SuspendLayout();
            this.tabPageRule.SuspendLayout();
            this.tabProperties.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ugWorkflows)).BeginInit();
            this.SuspendLayout();
            // 
            // cboSizeGroup
            // 
            this.cboSizeGroup.TabIndex = 6;
            // Begin TT#301-MD - JSmith - Controls are not functioning properly
            //this.cboSizeGroup.SelectionChangeCommitted += new System.EventHandler(this.cboSizeGroup_SelectionChangeCommitted);
            //this.cboSizeGroup.SelectionChangeCommitted += new System.EventHandler(this.cboSizeGroup_SelectionChangeCommitted);
            // End TT#301-MD - JSmith - Controls are not functioning properly
            // 
            // lblStoreAttribute
            // 
            this.lblStoreAttribute.Location = new System.Drawing.Point(118, 27);
            // 
            // cboStoreAttribute
            // 
            this.cboStoreAttribute.Location = new System.Drawing.Point(211, 26);
            this.cboStoreAttribute.Size = new System.Drawing.Size(251, 21);
            // Begin TT#301-MD - JSmith - Controls are not functioning properly
            //this.cboStoreAttribute.SelectionChangeCommitted += new System.EventHandler(this.cboStoreAttribute_SelectionChangeCommitted);
            //this.cboStoreAttribute.SelectionChangeCommitted += new System.EventHandler(this.cboStoreAttribute_SelectionChangeCommitted);
            // End TT#301-MD - JSmith - Controls are not functioning properly
            //this.cboStoreAttribute.DragDrop += new System.Windows.Forms.DragEventHandler(this.cboStoreAttribute_DragDrop);
            //this.cboStoreAttribute.DragEnter += new System.Windows.Forms.DragEventHandler(this.cboStoreAttribute_DragEnter);
            // 
            // cboFilter
            // 
            this.cboFilter.DropDownWidth = 102;
            this.cboFilter.Enabled = false;
            this.cboFilter.Location = new System.Drawing.Point(589, 19);
            this.cboFilter.Size = new System.Drawing.Size(102, 21);
            this.cboFilter.Visible = false;
            // 
            // lblFilter
            // 
            this.lblFilter.Enabled = false;
            this.lblFilter.Location = new System.Drawing.Point(586, 3);
            this.lblFilter.Visible = false;
            // 
            // cboSizeCurve
            // 
            this.cboSizeCurve.TabIndex = 10;
            // Begin TT#301-MD - JSmith - Controls are not functioning properly
            //this.cboSizeCurve.SelectionChangeCommitted += new System.EventHandler(this.cboSizeCurve_SelectionChangeCommitted);
            //this.cboSizeCurve.SelectionChangeCommitted += new System.EventHandler(this.cboSizeCurve_SelectionChangeCommitted);
            // End TT#301-MD - JSmith - Controls are not functioning properly
            // 
            // cboConstraints
            // 
            this.cboConstraints.TabIndex = 21;
            // Begin TT#301-MD - JSmith - Controls are not functioning properly
            //this.cboConstraints.SelectionChangeCommitted += new System.EventHandler(this.cboConstraints_SelectionChangeCommitted);
            //this.cboConstraints.SelectionChangeCommitted += new System.EventHandler(this.cboConstraints_SelectionChangeCommitted);
            // End TT#301-MD - JSmith - Controls are not functioning properly
            // 
            // cboAlternates
            // 
            this.cboAlternates.TabIndex = 8;
            // Begin TT#301-MD - JSmith - Controls are not functioning properly
            //this.cboAlternates.SelectionChangeCommitted += new System.EventHandler(this.cboAlternatives_SelectionChangeCommitted);
            //this.cboAlternates.SelectionChangeCommitted += new System.EventHandler(this.cboAlternatives_SelectionChangeCommitted);
            // End TT#301-MD - JSmith - Controls are not functioning properly
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
            this.ugRules.Location = new System.Drawing.Point(8, 55);
            this.ugRules.Size = new System.Drawing.Size(677, 337);
            // Begin TT#301-MD - JSmith - Controls are not functioning properly
            //this.ugRules.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.ugRules_InitializeLayout);
            //this.ugRules.AfterRowActivate += new System.EventHandler(this.ugRules_AfterRowActivate);
            // End TT#301-MD - JSmith - Controls are not functioning properly
            // 
            // cbExpandAll
            // 
            this.cbExpandAll.Location = new System.Drawing.Point(11, 35);
            this.cbExpandAll.Size = new System.Drawing.Size(104, 18);
            this.cbExpandAll.CheckedChanged += new System.EventHandler(this.cbExpandAll_CheckedChanged);
            // 
            // cboHeaderChar
            // 
            this.cboHeaderChar.TabIndex = 14;
            // Begin TT#301-MD - JSmith - Controls are not functioning properly
            //this.cboHeaderChar.SelectionChangeCommitted += new System.EventHandler(this.cboHeaderChar_SelectionChangeCommitted);
            //this.cboHeaderChar.SelectionChangeCommitted += new System.EventHandler(this.cboHeaderChar_SelectionChangeCommitted);
            // End TT#301-MD - JSmith - Controls are not functioning properly
            // 
            // cboHierarchyLevel
            // 
            this.cboHierarchyLevel.TabIndex = 15;
            // Begin TT#301-MD - JSmith - Controls are not functioning properly
            //this.cboHierarchyLevel.SelectionChangeCommitted += new System.EventHandler(this.cboHierarchyLevel_SelectionChangeCommitted);
            this.cboHierarchyLevel.SelectionChangeCommitted += new System.EventHandler(this.cboHierarchyLevel_SelectionChangeCommitted);
            // End TT#301-MD - JSmith - Controls are not functioning properly
            this.cboHierarchyLevel.MIDComboBoxPropertiesChangedEvent += new MIDComboBoxPropertiesChangedEventHandler(cboHierarchyLevel_MIDComboBoxPropertiesChangedEvent);
            // 
            // cbColor
            // 
            this.cbColor.TabIndex = 16;
            // 
            // gbGenericSizeCurve
            // 
            this.gbGenericSizeCurve.Location = new System.Drawing.Point(16, 62);
            this.gbGenericSizeCurve.Size = new System.Drawing.Size(261, 100);
            this.gbGenericSizeCurve.TabIndex = 11;
            // 
            // gbSizeCurve
            // 
            this.gbSizeCurve.Location = new System.Drawing.Point(18, 117);
            this.gbSizeCurve.TabIndex = 9;
            // 
            // gbSizeConstraints
            // 
            this.gbSizeConstraints.Location = new System.Drawing.Point(346, 118);
            this.gbSizeConstraints.TabIndex = 20;
            // 
            // gbGenericConstraint
            // 
            this.gbGenericConstraint.TabIndex = 22;
            // 
            // cboConstrHeaderChar
            // 
            this.cboConstrHeaderChar.TabIndex = 23;
            // 
            // cboConstrHierLevel
            // 
            this.cboConstrHierLevel.TabIndex = 24;
            // 
            // cbConstrColor
            // 
            this.cbConstrColor.TabIndex = 25;
            // 
            // gbSizeGroup
            // 
            this.gbSizeGroup.Location = new System.Drawing.Point(18, 52);
            this.gbSizeGroup.TabIndex = 6;
            // 
            // gbSizeAlternate
            // 
            this.gbSizeAlternate.Location = new System.Drawing.Point(346, 53);
            this.gbSizeAlternate.TabIndex = 7;
            // 
            // gbxNormalizeSizeCurves
            // 
            this.gbxNormalizeSizeCurves.Location = new System.Drawing.Point(348, 5);
            this.gbxNormalizeSizeCurves.TabIndex = 5;
            // 
            // cbxOverrideNormalizeDefault
            // 
            this.cbxOverrideNormalizeDefault.TabIndex = 5;
            // 
            // cbxUseDefaultcurve
            // 
            this.cbxUseDefaultcurve.CheckedChanged += new System.EventHandler(this.cbxUseDefaultCurve_CheckChanged);
            // 
            // cboNameExtension
            // 
            // Begin TT#301-MD - JSmith - Controls are not functioning properly
            //this.cboNameExtension.SelectionChangeCommitted += new System.EventHandler(this.cboNameExtension_SelectionChangeCommitted);
            //this.cboNameExtension.SelectionChangeCommitted += new System.EventHandler(this.cboNameExtension_SelectionChangeCommitted);
            // End TT#301-MD - JSmith - Controls are not functioning properly
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(636, 545);
            this.btnClose.TabIndex = 34;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(547, 545);
            this.btnSave.TabIndex = 32;
            // 
            // btnProcess
            // 
            this.btnProcess.Location = new System.Drawing.Point(12, 545);
            this.btnProcess.TabIndex = 30;
            // 
            // radGlobal
            // 
            this.radGlobal.TabIndex = 3;
            // 
            // txtDesc
            // 
            this.txtDesc.TabIndex = 2;
            // 
            // txtName
            // 
            this.txtName.TabIndex = 1;
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
            this.tabControl1.Controls.Add(this.tabMethods);
            this.tabControl1.Controls.Add(this.tabProperties);
            this.tabControl1.Location = new System.Drawing.Point(16, 48);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(704, 483);
            this.tabControl1.TabIndex = 0;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // tabMethods
            // 
            this.tabMethods.Controls.Add(this.tabControl2);
            this.tabMethods.Location = new System.Drawing.Point(4, 22);
            this.tabMethods.Name = "tabMethods";
            this.tabMethods.Size = new System.Drawing.Size(696, 457);
            this.tabMethods.TabIndex = 0;
            this.tabMethods.Text = "Methods";
            // 
            // tabControl2
            // 
            this.tabControl2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl2.Controls.Add(this.tabPageGeneral);
            this.tabControl2.Controls.Add(this.tabPageRule);
            this.tabControl2.Location = new System.Drawing.Point(-3, 3);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(699, 454);
            this.tabControl2.TabIndex = 2;
            this.tabControl2.SelectedIndexChanged += new System.EventHandler(this.tabControl2_SelectedIndexChanged);
            // 
            // tabPageGeneral
            // 
            this.tabPageGeneral.Controls.Add(this.grpVSWSizeConstraints);
            this.tabPageGeneral.Controls.Add(this.gbSizeConstraints);
            this.tabPageGeneral.Controls.Add(this.gbSizeCurve);
            this.tabPageGeneral.Controls.Add(this.cboMerchandise);
            this.tabPageGeneral.Controls.Add(this.lblMerchandise);
            this.tabPageGeneral.Controls.Add(this.grpSizedPacksOnly);
            this.tabPageGeneral.Controls.Add(this.lblFilter);
            this.tabPageGeneral.Controls.Add(this.cboFilter);
            this.tabPageGeneral.Controls.Add(this.gbSizeAlternate);
            this.tabPageGeneral.Controls.Add(this.gbSizeGroup);
            this.tabPageGeneral.Controls.Add(this.gbxNormalizeSizeCurves);
            this.tabPageGeneral.Location = new System.Drawing.Point(4, 22);
            this.tabPageGeneral.Name = "tabPageGeneral";
            this.tabPageGeneral.Size = new System.Drawing.Size(691, 428);
            this.tabPageGeneral.TabIndex = 0;
            this.tabPageGeneral.Text = "General";
            this.tabPageGeneral.Controls.SetChildIndex(this.gbxNormalizeSizeCurves, 0);
            this.tabPageGeneral.Controls.SetChildIndex(this.gbSizeGroup, 0);
            this.tabPageGeneral.Controls.SetChildIndex(this.gbSizeAlternate, 0);
            this.tabPageGeneral.Controls.SetChildIndex(this.cboFilter, 0);
            this.tabPageGeneral.Controls.SetChildIndex(this.lblFilter, 0);
            this.tabPageGeneral.Controls.SetChildIndex(this.grpSizedPacksOnly, 0);
            this.tabPageGeneral.Controls.SetChildIndex(this.lblMerchandise, 0);
            this.tabPageGeneral.Controls.SetChildIndex(this.cboMerchandise, 0);
            this.tabPageGeneral.Controls.SetChildIndex(this.gbSizeCurve, 0);
            this.tabPageGeneral.Controls.SetChildIndex(this.gbSizeConstraints, 0);
            this.tabPageGeneral.Controls.SetChildIndex(this.grpVSWSizeConstraints, 0);
            // 
            // grpVSWSizeConstraints
            // 
            this.grpVSWSizeConstraints.Controls.Add(this.cboSizeConstraints);
            this.grpVSWSizeConstraints.Controls.Add(this.chkVSWSizeConstraintsOverride);
            this.grpVSWSizeConstraints.Location = new System.Drawing.Point(18, 287);
            this.grpVSWSizeConstraints.Name = "grpVSWSizeConstraints";
            this.grpVSWSizeConstraints.Size = new System.Drawing.Size(294, 37);
            this.grpVSWSizeConstraints.TabIndex = 31;
            this.grpVSWSizeConstraints.TabStop = false;
            this.grpVSWSizeConstraints.Text = "VSW Size Constraints:";
            // 
            // cboSizeConstraints
            // 
            this.cboSizeConstraints.AutoAdjust = true;
            this.cboSizeConstraints.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboSizeConstraints.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboSizeConstraints.DataSource = null;
            this.cboSizeConstraints.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSizeConstraints.DropDownWidth = 164;
            this.cboSizeConstraints.FormattingEnabled = false;
            this.cboSizeConstraints.IgnoreFocusLost = false;
            this.cboSizeConstraints.ItemHeight = 13;
            this.cboSizeConstraints.Location = new System.Drawing.Point(123, 9);
            this.cboSizeConstraints.Margin = new System.Windows.Forms.Padding(0);
            this.cboSizeConstraints.MaxDropDownItems = 8;
            this.cboSizeConstraints.Name = "cboSizeConstraints";
            this.cboSizeConstraints.Size = new System.Drawing.Size(164, 23);
            this.cboSizeConstraints.TabIndex = 1;
            this.cboSizeConstraints.Tag = null;
            // 
            // chkVSWSizeConstraintsOverride
            // 
            this.chkVSWSizeConstraintsOverride.AutoSize = true;
            this.chkVSWSizeConstraintsOverride.Location = new System.Drawing.Point(11, 14);
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
            this.cboMerchandise.IgnoreFocusLost = true;
            this.cboMerchandise.AutoAdjust = true;
            this.cboMerchandise.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.cboMerchandise.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboMerchandise.DataSource = null;
            this.cboMerchandise.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown;
            this.cboMerchandise.DropDownWidth = 217;
            this.cboMerchandise.FormattingEnabled = false;
            this.cboMerchandise.Location = new System.Drawing.Point(126, 12);
            this.cboMerchandise.Margin = new System.Windows.Forms.Padding(0);
            this.cboMerchandise.Name = "cboMerchandise";
            this.cboMerchandise.Size = new System.Drawing.Size(216, 21);
            this.cboMerchandise.TabIndex = 4;
            this.cboMerchandise.Tag = null;
            // Begin TT#301-MD - JSmith - Controls are not functioning properly
            //this.cboMerchandise.SelectionChangeCommitted += new System.EventHandler(this.cboMerchandise_SelectionChangeCommitted);
            this.cboMerchandise.SelectionChangeCommitted += new System.EventHandler(this.cboMerchandise_SelectionChangeCommitted);
            // End TT#301-MD - JSmith - Controls are not functioning properly
            this.cboMerchandise.MIDComboBoxPropertiesChangedEvent += new MIDComboBoxPropertiesChangedEventHandler(cboMerchandise_MIDComboBoxPropertiesChangedEvent);
            this.cboMerchandise.DragDrop += new System.Windows.Forms.DragEventHandler(this.cboMerchandise_DragDrop);
            this.cboMerchandise.DragEnter += new System.Windows.Forms.DragEventHandler(this.cboMerchandise_DragEnter);
            this.cboMerchandise.DragOver += new System.Windows.Forms.DragEventHandler(this.cboMerchandise_DragOver);
            this.cboMerchandise.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cboMerchandise_KeyDown);
            this.cboMerchandise.Validating += new System.ComponentModel.CancelEventHandler(this.cboMerchandise_Validating);
            this.cboMerchandise.Validated += new System.EventHandler(this.cboMerchandise_Validated);
            //
            // cboInventoryBasis
            //
            this.cboInventoryBasis.AllowDrop = true;
            // Begin TT#301-MD - JSmith - Controls are not functioning properly
            //this.cboInventoryBasis.SelectionChangeCommitted += new System.EventHandler(cboInventoryBasis_SelectionChangeCommitted);
            //this.cboInventoryBasis.SelectionChangeCommitted += new System.EventHandler(cboInventoryBasis_SelectionChangeCommitted);
            //this.cboInventoryBasis.KeyDown += new System.Windows.Forms.KeyEventHandler(cboInventoryBasis_KeyDown);
            //this.cboInventoryBasis.Validating += new System.ComponentModel.CancelEventHandler(cboInventoryBasis_Validating);
            //this.cboInventoryBasis.Validated += new System.EventHandler(cboInventoryBasis_Validated);
            //this.cboInventoryBasis.DragDrop += new System.Windows.Forms.DragEventHandler(cboInventoryBasis_DragDrop);
            //this.cboInventoryBasis.DragEnter += new System.Windows.Forms.DragEventHandler(cboInventoryBasis_DragEnter);
            //this.cboInventoryBasis.DragOver += new System.Windows.Forms.DragEventHandler(cboInventoryBasis_DragOver);
            // End TT#301-MD - JSmith - Controls are not functioning properly
            // 
            // lblMerchandise
            // 
            this.lblMerchandise.AutoSize = true;
            this.lblMerchandise.Location = new System.Drawing.Point(18, 16);
            this.lblMerchandise.Name = "lblMerchandise";
            this.lblMerchandise.Size = new System.Drawing.Size(96, 13);
            this.lblMerchandise.TabIndex = 30;
            this.lblMerchandise.Text = "Merchandise Basis";
            // 
            // grpSizedPacksOnly
            // 
            this.grpSizedPacksOnly.Controls.Add(this.gbMaxPackAllocNeedTolerance);
            this.grpSizedPacksOnly.Controls.Add(this.gbAvgPackDevTolerance);
            this.grpSizedPacksOnly.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpSizedPacksOnly.Location = new System.Drawing.Point(18, 330);
            this.grpSizedPacksOnly.Name = "grpSizedPacksOnly";
            this.grpSizedPacksOnly.Size = new System.Drawing.Size(657, 85);
            this.grpSizedPacksOnly.TabIndex = 26;
            this.grpSizedPacksOnly.TabStop = false;
            this.grpSizedPacksOnly.Text = "Size Detail Packs Only";
            // 
            // gbMaxPackAllocNeedTolerance
            // 
            this.gbMaxPackAllocNeedTolerance.Controls.Add(this.cbxStepped);
            this.gbMaxPackAllocNeedTolerance.Controls.Add(this.cbxOverrideMaxPackAllocNeed);
            this.gbMaxPackAllocNeedTolerance.Controls.Add(this.txtMaxPackAllocNeedTolerance);
            this.gbMaxPackAllocNeedTolerance.Controls.Add(this.cbxNoMaxStep);
            this.gbMaxPackAllocNeedTolerance.Location = new System.Drawing.Point(304, 19);
            this.gbMaxPackAllocNeedTolerance.Name = "gbMaxPackAllocNeedTolerance";
            this.gbMaxPackAllocNeedTolerance.Size = new System.Drawing.Size(347, 53);
            this.gbMaxPackAllocNeedTolerance.TabIndex = 30;
            this.gbMaxPackAllocNeedTolerance.TabStop = false;
            this.gbMaxPackAllocNeedTolerance.Text = "Maximum Pack Allocation Need Tolerance";
            this.gbMaxPackAllocNeedTolerance.Enter += new System.EventHandler(this.gbMaxPackAllocNeedTolerance_Enter);
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
            this.gbAvgPackDevTolerance.Enter += new System.EventHandler(this.gbAvgPackDevTolerance_Enter);
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
            // tabPageRule
            // 
            this.tabPageRule.Controls.Add(this.cbExpandAll);
            this.tabPageRule.Controls.Add(this.ugRules);
            this.tabPageRule.Controls.Add(this.lblStoreAttribute);
            this.tabPageRule.Controls.Add(this.cboStoreAttribute);
            this.tabPageRule.Location = new System.Drawing.Point(4, 22);
            this.tabPageRule.Name = "tabPageRule";
            this.tabPageRule.Size = new System.Drawing.Size(691, 428);
            this.tabPageRule.TabIndex = 1;
            this.tabPageRule.Text = "Rules";
            // 
            // tabProperties
            // 
            this.tabProperties.Controls.Add(this.ugWorkflows);
            this.tabProperties.Location = new System.Drawing.Point(4, 22);
            this.tabProperties.Name = "tabProperties";
            this.tabProperties.Size = new System.Drawing.Size(696, 457);
            this.tabProperties.TabIndex = 1;
            this.tabProperties.Text = "Properties";
            // 
            // ugWorkflows
            // 
            this.ugWorkflows.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ugWorkflows.Cursor = System.Windows.Forms.Cursors.Default;
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
            this.ugWorkflows.Size = new System.Drawing.Size(648, 379);
            this.ugWorkflows.TabIndex = 0;
            // 
            // frmSizeNeedMethod
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(728, 577);
            this.Controls.Add(this.tabControl1);
            this.Name = "frmSizeNeedMethod";
            this.Text = "Size Need Method";
            this.Load += new System.EventHandler(this.frmSizeNeedMethod_Load);
            this.Controls.SetChildIndex(this.btnSave, 0);
            this.Controls.SetChildIndex(this.btnClose, 0);
            this.Controls.SetChildIndex(this.btnProcess, 0);
            this.Controls.SetChildIndex(this.pnlGlobalUser, 0);
            this.Controls.SetChildIndex(this.tabControl1, 0);
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
            this.tabControl1.ResumeLayout(false);
            this.tabMethods.ResumeLayout(false);
            this.tabControl2.ResumeLayout(false);
            this.tabPageGeneral.ResumeLayout(false);
            this.tabPageGeneral.PerformLayout();
            this.grpVSWSizeConstraints.ResumeLayout(false);
            this.grpVSWSizeConstraints.PerformLayout();
            this.grpSizedPacksOnly.ResumeLayout(false);
            this.gbMaxPackAllocNeedTolerance.ResumeLayout(false);
            this.gbMaxPackAllocNeedTolerance.PerformLayout();
            this.gbAvgPackDevTolerance.ResumeLayout(false);
            this.gbAvgPackDevTolerance.PerformLayout();
            this.tabPageRule.ResumeLayout(false);
            this.tabPageRule.PerformLayout();
            this.tabProperties.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ugWorkflows)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		#region Control Event Handlers

            //  BEGIN TT#41-MD - Gtaylor - UC#2
            private HierarchyNodeProfile GetNodeProfile2(string aProductID)
            {
                string productID;
                string[] pArray;

                try
                {
                    productID = aProductID.Trim();
                    pArray = productID.Split(new char[] { '[' });
                    productID = pArray[0].Trim();

                    HierarchyMaintenance hm = new HierarchyMaintenance(SAB);
                    EditMsgs em = new EditMsgs();
                    pArray = null;
                    return hm.NodeLookup(ref em, productID, false);
                }
                catch (Exception)
                {
                    throw;
                }
            }
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
            // End TT#301-MD - JSmith - Controls are not functioning properly

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
                        // JBolles - MID Track #5020 - Prevent errored textbox from resetting its value before error is corrected
                        //					else if (_priorError)
                        //					{
                        //						cboInventoryBasis.SelectedIndex = _lastMerchIndex;
                        //					}
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
            //  END TT#41-MD - Gtaylor - UC#2

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

        private void cboMerchandise_DragOver(object sender, System.Windows.Forms.DragEventArgs e)
		{
            // Begin TT#296-MD - JSmith - Methods and Workflows can be dragged and dropped into Inventory Basis drop down.
            //Image_DragOver(sender, e);
            Merchandise_DragOver(sender, e);
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
                    //					HierarchyNodeProfile hnp = SAB.HierarchyServerSession.GetNodeData(cbp.Key);
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

		private void cboMerchandise_Validating(object sender, CancelEventArgs e)
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
        //private void cboSizeCurve_SelectionChangeCommitted(object sender, System.EventArgs e)
        override protected void cboSizeCurve_SelectionChangeCommitted(object sender, System.EventArgs e)
        // End TT#301-MD - JSmith - Controls are not functioning properly
		{
			// begin MID Track 3781 Size Curve not required
			try
			{
				if (base.FormLoaded)
				{
					if (_sizeNeedMethod.PromptSizeChange)
					{
						// Begin TT#499 - RMatelic - Check Rules and Basis Substitutes to determine if warning message is shown
                        if (RuleExists() || BasisSubstituteExists())
                        {
                            if (ShowWarningPrompt(false) == DialogResult.Yes)
                            {
                                //_sizeNeedMethod.DeleteMethodRules(new TransactionData());
                                //ChangePending = true;
                                //_sizeNeedMethod.SizeCurveGroupRid = Convert.ToInt32(cboSizeCurve.SelectedValue.ToString(), CultureInfo.CurrentUICulture); ;
                                //_sizeNeedMethod.PromptSizeChange = false;
                                //_sizeNeedMethod.SizeGroupRid = Include.NoRID;
                                //cboSizeGroup.SelectedValue = Include.NoRID;
                                //_sizeNeedMethod.GetSizesUsing = eGetSizes.SizeCurveGroupRID;
                                //_sizeNeedMethod.GetDimensionsUsing = eGetDimensions.SizeCurveGroupRID;
                                //_sizeNeedMethod.CreateConstraintData();

                                //BindAllSizeGrid(_sizeNeedMethod.MethodConstraints);
                                //CheckExpandAll();
                                UpdateCurveData();
                            }
                            else
                            {
                                //Shut off the prompt so the combo can be reset to original value.
                                _sizeNeedMethod.PromptSizeChange = false;
                                cboSizeCurve.SelectedValue = _sizeNeedMethod.SizeCurveGroupRid;
                                //this.cboSizeCurve_SelectionChangeCommitted(source, new EventArgs()); // TT#301-MD - JSmith - Controls are not functioning properly
                                _sizeNeedMethod.PromptSizeChange = true; // TT#2155 - JEllis - Fill Size Holes Null Reference
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
						_sizeNeedMethod.PromptSizeChange = true;
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
                _sizeNeedMethod.DeleteMethodRules(new TransactionData());
                ChangePending = true;
                _sizeNeedMethod.SizeCurveGroupRid = Convert.ToInt32(cboSizeCurve.SelectedValue.ToString(), CultureInfo.CurrentUICulture); ;
                _sizeNeedMethod.PromptSizeChange = false;
                _sizeNeedMethod.SizeGroupRid = Include.NoRID;
                cboSizeGroup.SelectedValue = Include.NoRID;
                //this.cboSizeGroup_SelectionChangeCommitted(source, new EventArgs()); // TT#301-MD - JSmith - Controls are not functioning properly
                SetApplyRulesOnly_State(); // TT#2155 - JEllis - Fill Size Holes Null Reference
                _sizeNeedMethod.GetSizesUsing = eGetSizes.SizeCurveGroupRID;
                _sizeNeedMethod.GetDimensionsUsing = eGetDimensions.SizeCurveGroupRID;
                _sizeNeedMethod.CreateConstraintData();
                BindAllSizeGrid(_sizeNeedMethod.MethodConstraints);
                CheckExpandAll();
                _sizeNeedMethod.PromptSizeChange = true;
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
            if (_sizeNeedMethod.SizeGroupRid != Include.NoRID)
            {
                if (!cbxUseDefaultcurve.Checked
                   && Convert.ToInt32(cboHierarchyLevel.SelectedValue, CultureInfo.CurrentUICulture) == -2
                   && Convert.ToInt32(cboHeaderChar.SelectedValue, CultureInfo.CurrentUICulture) == Include.NoRID
                   && Convert.ToInt32(cboNameExtension.SelectedValue, CultureInfo.CurrentUICulture) == Include.NoRID)
                {
                    cbxApplyRulesOnly.Checked = false;
                    _sizeNeedMethod.ApplyRulesOnly = false;
                    cbxApplyRulesOnly.Enabled = false;
                }
                else
                {
                    cbxApplyRulesOnly.Enabled = true;
                }
            }
            else if (_sizeNeedMethod.SizeCurveGroupRid != Include.NoRID)
            {
                cbxApplyRulesOnly.Enabled = true;
            }
            else
            {
                cbxApplyRulesOnly.Checked = false;
                _sizeNeedMethod.ApplyRulesOnly = false;
                cbxApplyRulesOnly.Enabled = false;
            }
        }
        // end TT#2155 - JEllis - Fill Size Holes Null Reference

        // End TT#499
////				if (base.FormLoaded)
////				{
////					ChangePending = true;
////				}
//
//
//				try
//				{
//					if (base.FormLoaded)
//					{
//						if (_sizeNeedMethod.PromptSizeChange)
//						{
//							
//							if (ShowWarningPrompt(false) == DialogResult.Yes)
//							{
//	
//								_sizeNeedMethod.DeleteMethodRules(new TransactionData());
//								ChangePending = true;
//								_sizeNeedMethod.SizeCurveGroupRid = Convert.ToInt32(cboSizeCurve.SelectedValue.ToString(),CultureInfo.CurrentUICulture);;
//								_sizeNeedMethod.CreateConstraintData();
//								BindAllSizeGrid(_sizeNeedMethod.MethodConstraints);
//								CheckExpandAll();
////								if (_sizeNeedMethod.SizeCurveGroupRid == Include.NoRID)
////								{
////									ClearSizeCurveComboBox();
////								}
////								else
////								{
////									BindSizeCurveComboBox(_sizeNeedMethod.SizeCurveGroupRid);
////								}
//							}
//	
//							else
//							{
//								//Shut off the prompt so the combo can be reset to original value.
//								_sizeNeedMethod.PromptSizeChange = false;
//								cboSizeCurve.SelectedValue = _sizeNeedMethod.SizeCurveGroupRid;
//							}
//						}
//						else
//						{
//							//Turn the prompt back on.
//							_sizeNeedMethod.PromptSizeChange = true;
//						}
//					}
//				}
//				catch (Exception ex)
//				{
//					HandleException(ex, "cboSizeCurve_SelectionChangeCommitted");
//				}
//			}
		// end MID Track 3781 Size Curve not required

        // Begin TT#301-MD - JSmith - Controls are not functioning properly
        //private void cboSizeGroup_SelectionChangeCommitted(object sender, System.EventArgs e)
        override protected void cboSizeGroup_SelectionChangeCommitted(object sender, System.EventArgs e)
        // End TT#301-MD - JSmith - Controls are not functioning properly
		{
			// begin MID Track 3781 Size Curve Not required
			try
			{
				if (base.FormLoaded)
				{
					if (_sizeNeedMethod.PromptSizeChange)
					{
						
						if (ShowWarningPrompt(false) == DialogResult.Yes)
						{
							_sizeNeedMethod.DeleteMethodRules(new TransactionData());
                            ChangePending = true;
							_sizeNeedMethod.SizeGroupRid = Convert.ToInt32(cboSizeGroup.SelectedValue.ToString(), CultureInfo.CurrentUICulture); 
							_sizeNeedMethod.PromptSizeChange = false; 
							_sizeNeedMethod.SizeCurveGroupRid = Include.NoRID; 
							cboSizeCurve.SelectedValue = Include.NoRID;
                            //this.cboSizeCurve_SelectionChangeCommitted(source, new EventArgs()); // TT#301-MD - JSmith - Controls are not functioning properly
							_sizeNeedMethod.GetSizesUsing = eGetSizes.SizeGroupRID; 
							_sizeNeedMethod.GetDimensionsUsing = eGetDimensions.SizeGroupRID; 
							_sizeNeedMethod.CreateConstraintData();
							BindAllSizeGrid(_sizeNeedMethod.MethodConstraints);
							CheckExpandAll();
                            _sizeNeedMethod.PromptSizeChange = true; // TT#2155 - JEllis - Fill Size Holes Null Reference
						}

						else
						{
							//Shut off the prompt so the combo can be reset to original value.
							_sizeNeedMethod.PromptSizeChange = false;
							cboSizeGroup.SelectedValue = _sizeNeedMethod.SizeGroupRid; // MID Track 3781 Size Curve not required for Fill Size Holes
                            //this.cboSizeGroup_SelectionChangeCommitted(source, new EventArgs()); // TT#301-MD - JSmith - Controls are not functioning properly
                            _sizeNeedMethod.PromptSizeChange = true; // TT#2155 - JEllis - Fill Size Holes Null Reference
						}
					}
					else
					{
						//Turn the prompt back on.
						_sizeNeedMethod.PromptSizeChange = true;
					}
				}
                // begin TT#2155 - JEllis - Fill Size Holes Null Reference
                SetApplyRulesOnly_State();
                // end TT#2155 - JEllis - Fill Size holes Null Reference

			}
			catch (Exception ex)
			{
                // Begin TT#301-MD - JSmith - Controls are not functioning properly
                //HandleException(ex, "cboSizeGroup_SelectionChangeCommitted");
                HandleException(ex, "cboSizeGroup_SelectionChangeCommitted");
                // End TT#301-MD - JSmith - Controls are not functioning properly
			}
		}
//				try
//				{
//					if (base.FormLoaded)
//					{
//	//					if (_sizeNeedMethod.PromptSizeChange)
//	//					{
//	//						
//	//						if (ShowWarningPrompt() == DialogResult.Yes)
//	//						{
//	//
//	//							_sizeNeedMethod.DeleteMethodConstraints(new TransactionData());
//								ChangePending = true;
//								_sizeNeedMethod.SizeGroupRid = Convert.ToInt32(cboSizeGroup.SelectedValue.ToString(),CultureInfo.CurrentUICulture);;
//								CheckExpandAll();
//	//							_sizeNeedMethod.CreateConstraintData();
//	//							BindAllSizeGrid(_sizeNeedMethod.MethodConstraints);
//	//
////								if (_sizeNeedMethod.SizeGroupRid == Include.NoRID)
////								{
////									ClearSizeCurveComboBox();
////								}
////								else
////								{
////									BindSizeCurveComboBox(_sizeNeedMethod.SizeGroupRid);
////								}
//	//						}
//	//
//	//						else
//	//						{
//	//							//Shut off the prompt so the combo can be reset to original value.
//	//							_sizeNeedMethod.PromptSizeChange = false;
//	//							cboSizeGroup.SelectedValue = _fillSizeHolesMethod.SizeGroupRid;
//	//						}
//	//					}
//	//					else
//	//					{
//	//						//Turn the prompt back on.
//	//						_sizeNeedMethod.PromptSizeChange = true;
//	//					}
//					}
//				}
//				catch (Exception ex)
//				{
//					HandleException(ex, "cboSizeGroup_SelectionChangeCommitted");
//				}
//			}
        // end MID Track 3781 Size Curve not required

        // Begin TT#301-MD - JSmith - Controls are not functioning properly
        //private void cboStoreAttribute_SelectionChangeCommitted(object sender, System.EventArgs e)
        override protected void cboStoreAttribute_SelectionChangeCommitted(object sender, System.EventArgs e)
        // End TT#301-MD - JSmith - Controls are not functioning properly
		{
			try
			{
				if (base.FormLoaded)
				{
					if (_sizeNeedMethod.PromptSizeChange)
					{
							
						if (ShowWarningPrompt(false) == DialogResult.Yes)
						{
	
							_sizeNeedMethod.DeleteMethodRules(new TransactionData());
							ChangePending = true;
							_sizeNeedMethod.SG_RID = Convert.ToInt32(cboStoreAttribute.SelectedValue.ToString(),CultureInfo.CurrentUICulture);;
							_sizeNeedMethod.CreateConstraintData();
							BindAllSizeGrid(_sizeNeedMethod.MethodConstraints);
							CheckExpandAll();
						}
	
						else
						{
							//Shut off the prompt so the combo can be reset to original value.
							_sizeNeedMethod.PromptSizeChange = false;
							cboStoreAttribute.SelectedValue = _sizeNeedMethod.SG_RID;
                            //this.cboStoreAttribute_SelectionChangeCommitted(source, new EventArgs()); // TT#301-MD - JSmith - Controls are not functioning properly
						}
					}
					else
					{
						//Turn the prompt back on.
						_sizeNeedMethod.PromptSizeChange = true;
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
        // End TT#301-MD - JSmith - Controls are not functioning properly

        // Begin TT#301-MD - JSmith - Controls are not functioning properly
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

		private void btnClose_Click(object sender, System.EventArgs e)
		{
			try
			{
				Cancel_Click();
			}
			catch
			{
				MessageBox.Show("Error in btnClose_Click");
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

		private void btnProcess_Click(object sender, System.EventArgs e)
		{
			try
			{	// begin A&F add Generic Size Curve Name 
				// BEGIN TT#696-MD - Stodd - add "active process"
				if (!OkToProcess(this, eMethodType.SizeNeedAllocation))		 
				{
					return;
				}	
				// END TT#696-MD - Stodd - add "active process"					
				// end A&F add Generic Size Curve Name 

				ProcessAction(eMethodType.SizeNeedAllocation);

				// as part of the  processing we saved the info, so it should be changed to update.
				if (!ErrorFound)
				{
					_sizeNeedMethod.Method_Change_Type = eChangeType.update;
					btnSave.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_Update);
				}

			}
			catch(Exception ex)
			{
				HandleException(ex, "btnProcess_Click");
			}
		}

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

		protected override void Call_btnProcess_Click()
		{
			try
			{	// begin A&F add Generic Size Curve Name 
				// BEGIN TT#696-MD - Stodd - add "active process"
				//====================================================
				// Checks to be sure there are valid selected headers
				//====================================================
				if (!OkToProcess(this, eMethodType.SizeNeedAllocation))		 
				{
					return;
				}	
				// END TT#696-MD - Stodd - add "active process"					
				// end A&F add Generic Size Curve Name 

				ProcessAction(eMethodType.SizeNeedAllocation);

				// as part of the  processing we saved the info, so it should be changed to update.
				if (!ErrorFound)
				{
					_sizeNeedMethod.Method_Change_Type = eChangeType.update;
					btnSave.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_Update);
				}

			}
			catch(Exception ex)
			{
				HandleException(ex, "Call_btnProcess_Click");
			}
		}
		// End MID Track 4858

        // Begin TT#356 - RMatelic - Allocation Options->detail pack settings - will not override to blank within the method
        private void cbxOverrideAvgPackDev_CheckedChanged(object sender, EventArgs e)
        {
            if (!cbxOverrideAvgPackDev.Checked)
            {
                double avgPackDevTolerance = SAB.ApplicationServerSession.GlobalOptions.PackSizeErrorPercent;
                txtAvgPackDevTolerance.Text = (avgPackDevTolerance == Double.MaxValue) ? string.Empty : Convert.ToString(avgPackDevTolerance, CultureInfo.CurrentUICulture);
            }
            txtAvgPackDevTolerance.Enabled = cbxOverrideAvgPackDev.Checked;
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
        // End TT#356 

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
                cboSizeConstraints.SelectedValue = _sizeNeedMethod.VSWSizeConstraints;
            }
        }

        // END TT#246-MD - AGallagher - VSW Size - Add In-store Size Constraint options 

        private void txtAvgPackDevTolerance_TextChanged(object sender, EventArgs e)
		{
			if (base.FormLoaded)
			{
				ChangePending = true;
			}
		}


        private void txtMaxPackAllocNeedTolerance_TextChanged(object sender, System.EventArgs e)
		{
			if (base.FormLoaded)
			{
				ChangePending = true;
			}
		}

        // Begin TT#301-MD - JSmith - Controls are not functioning properly
        //private void cboMerchandise_SelectionChangeCommitted(object sender, System.EventArgs e)
        private void cboMerchandise_SelectionChangeCommitted(object sender, System.EventArgs e)
        // End TT#301-MD - JSmith - Controls are not functioning properly
		{
			if (base.FormLoaded)
			{
				ChangePending = true;
			}

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

		// begin A&F Add Generic Size Curve data 
		private void tabControl1_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			 
			if (tabControl1.SelectedTab == tabMethods && tabControl2.SelectedTab == tabPageGeneral)
			{
				//gbGenericSizeCurve.Visible =   true;
				//gbSizeCurve.Visible = true;
				//gbSizeConstraints.Visible = true;
			}
			else
			{
				//gbGenericSizeCurve.Visible =   false;
				//gbSizeCurve.Visible = false;
				//gbSizeConstraints.Visible = false;
			}
		}
		
		// end A&F Add Generic Size Curve data 

        // Begin TT#301-MD - JSmith - Controls are not functioning properly
        //private void cboAlternatives_SelectionChangeCommitted(object sender, System.EventArgs e)
        override protected void cboAlternates_SelectionChangeCommitted(object sender, System.EventArgs e)
        // ENd TT#301-MD - JSmith - Controls are not functioning properly
		{
			try
			{
				if (base.FormLoaded)
				{
					ChangePending = true;
					//_sizeNeedMethod.SizeAlternateRid = Convert.ToInt32(cboAlternatives.SelectedValue.ToString(),CultureInfo.CurrentUICulture);;
				}
			}
			catch (Exception ex)
			{
                // Begin TT#301-MD - JSmith - Controls are not functioning properly
                //HandleException(ex, "cboAlternatives_SelectionChangeCommitted");
                HandleException(ex, "cboAlternatives_SelectionChangeCommitted");
                // End TT#301-MD - JSmith - Controls are not functioning properly
			}
		}

        // Begin TT#301-MD - JSmith - Controls are not functioning properly
        //private void cboConstraints_SelectionChangeCommitted(object sender, System.EventArgs e)
        override protected void cboConstraints_SelectionChangeCommitted(object sender, System.EventArgs e)
        // End TT#301-MD - JSmith - Controls are not functioning properly
		{
			try
			{
				if (base.FormLoaded)
				{
					ChangePending = true;
					//_sizeNeedMethod.SizeConstraintRid = Convert.ToInt32(cboConstraints.SelectedValue.ToString(),CultureInfo.CurrentUICulture);;
				}
			}
			catch (Exception ex)
			{
                // Begin TT#301-MD - JSmith - Controls are not functioning properly
                //HandleException(ex, "cboConstraints_SelectionChangeCommitted");
                HandleException(ex, "cboConstraints_SelectionChangeCommitted");
                // End TT#301-MD - JSmith - Controls are not functioning properly
			}
		}

		#endregion

		#region Methods

			/// <summary>
			/// Private method handles loading data on the form
			/// </summary>
			/// <param name="aGlobalUserType"></param>
			private void Common_Load(eGlobalUserType aGlobalUserType)
			{
				try
				{		
					SetText();
				
					Name = MIDText.GetTextOnly((int)eMethodType.SizeNeedAllocation);			

					LoadMethods();

                    LoadWorkflows();

					// BEGIN MID Track #4826 - JSmith - Normalize Size Curves
					if (_sizeNeedMethod.NormalizeSizeCurvesDefaultIsOverridden)
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
                    cbxUseDefaultcurve.Checked = _sizeNeedMethod.UseDefaultCurve;
                    // End TT#413

                    // begin TT#2155 - JEllis - Fill Size Holes Null Reference
                    cbxApplyRulesOnly.Checked = _sizeNeedMethod.ApplyRulesOnly;
                    // end TT#2155 - JEllis - Fill Size Holes Null Reference
					
					// BEGIN ANF Generic Size Constraint
					SetMaskedComboBoxesEnabled(); 
					// END ANF Generic Size Constraint
                    // Begin TT#44 - JSmith - Drag/Drop User Attributes or Filters in to Global Methods does not react consistantly
                    cboFilter.Tag = new MIDStoreFilterComboBoxTag(SAB, cboFilter.ComboBox, eSecurityTypes.Store, eSecuritySelectType.View | eSecuritySelectType.Update, FunctionSecurity, aGlobalUserType == eGlobalUserType.User);
                    // End TT#44

                    Load_IB_Combo(); // TT#41-MD - GTaylor - UC#2

                    //BEGIN TT#110-MD-VStuart - In Use Tool
                    tabControl1.Controls.Remove(tabProperties);
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
					if (_sizeNeedMethod.Method_Change_Type == eChangeType.update)
						btnSave.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_Update);
					else
						btnSave.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_Save);

					this.btnClose.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_Close);
					this.btnProcess.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_Process);
                    // Begin TT#356 - RMatelic - Allocation Options->detail pack settings - will not override to blank within the method
                    string lblOverrideDefault = MIDText.GetTextOnly(eMIDTextCode.lbl_OverrideDefault);
                    cbxOverrideNormalizeDefault.Text = lblOverrideDefault;
                    cbxOverrideAvgPackDev.Text = lblOverrideDefault;
                    cbxOverrideMaxPackAllocNeed.Text = lblOverrideDefault;
                    gbAvgPackDevTolerance.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_AvgPackDevTolerance);
                    gbMaxPackAllocNeedTolerance.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_MaxPackAllocNeedTolerance);
                    // End TT#356
                    this.lblInventoryBasis.Text = MIDText.GetTextOnly((int)eMIDTextCode.lbl_InventoryBasis); // TT#41-MD - GTaylor - UC#2
                    // BEGIN TT#246-MD - AGallagher - VSW Size - Add In-store Size Constraint options
                    this.grpVSWSizeConstraints.Text = MIDText.GetTextOnly((int)eMIDTextCode.lbl_VSWSizeConstraints);
                    this.chkVSWSizeConstraintsOverride.Text = MIDText.GetTextOnly((int)eMIDTextCode.lbl_VSWOverrideDefault);
                    // END TT#246-MD - AGallagher - VSW Size - Add In-store Size Constraint options
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
					BuildDataTables(); //Inherited from WorkflowMethodFormBase
				 
                    //Begin Track #5858 - KJohnson - Validating store security only
                    // Begin TT#44 - JSmith - Drag/Drop User Attributes or Filters in to Global Methods does not react consistantly
                    //cboStoreAttribute.Tag = new MIDStoreAttributeComboBoxTag(SAB, cboStoreAttribute);
                    cboStoreAttribute.Tag = new MIDStoreAttributeComboBoxTag(SAB, cboStoreAttribute, FunctionSecurity, _sizeNeedMethod.GlobalUserType == eGlobalUserType.User);
                    // End TT#44
                    //End Track #5858
					BindMerchandiseComboBox();
                    // Begin Track #4872 - JSmith - Global/User Attributes
                    //BindStoreAttrComboBox(); //Inherited from SizeMethodsFormBase
                    BindStoreAttrComboBox(_sizeNeedMethod.Method_Change_Type, _sizeNeedMethod.GlobalUserType); //Inherited from SizeMethodsFormBase
                    // End Track #4872
					
					// BEGIN MID Track #4628 - Error on window close; solution is to remove 
					//                         the following 'if...' condition
					//if (FormLoaded)
 					//{
						BindAllSizeGrid(_sizeNeedMethod.MethodConstraints); //Inherited from SizeMethodsFormBase
					//}
					// END MID Track #4628

					// BEGIN ANF Generic Size Constraint
					BindSizeComboBoxes(_sizeNeedMethod.SizeGroupRid, _sizeNeedMethod.SizeAlternateRid, 
									   _sizeNeedMethod.SizeCurveGroupRid,_sizeNeedMethod.SizeConstraintRid);
					// END ANF Generic Size Constraint

					// ANF - add Generic Size Curve
					LoadGenericSizeCurveGroupBox();

					this.txtName.Text = _sizeNeedMethod.Name;
					this.txtDesc.Text = _sizeNeedMethod.Method_Description;
										
					
					
					//LoadWorkflows();

					//OK If New
					LoadSizeNeedValues();
					SetEditableQuantityCells();
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
					for (int levIndex = 0;
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
				}
				catch(Exception ex)
				{
					HandleException(ex, "AddNodeToMerchandiseCombo");
				}
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

                    //var z = new EventArgs();	//TT#301-MD-VStuart-Version 5.0-Controls
					cboMerchandise.DataSource  = MerchandiseDataTable; //Inherited from WorkflowMethodFormBase
                    //this.cboMerchandise_SelectionChangeCommitted(this, z);	//TT#301-MD-VStuart-Version 5.0-Controls
					cboMerchandise.DisplayMember = "text";
					cboMerchandise.ValueMember = "seqno";
                    AdjustTextWidthComboBox_DropDown(cboMerchandise);  // TT#1401 - AGallagher - Reservation Stores

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
                    //this.cboInventoryBasis_SelectionChangeCommitted(this, z);	//TT#301-MD-VStuart-Version 5.0-Controls
                    cboInventoryBasis.DisplayMember = "text";
                    cboInventoryBasis.ValueMember = "seqno";
                    // END TT#41-MD -- GTaylor - UC#2
				}
				catch (Exception ex)
				{
					HandleException(ex, "BindMerchandiseComboBox");
				}
			}


			/// <summary>
			/// Clears the merchandise combo box.
			/// </summary>
			private void ClearMerchandiseComboBox()
			{
                //var z = new EventArgs();	//TT#301-MD-VStuart-Version 5.0-Controls
                cboMerchandise.DataSource = null;//TT#7 - MD - RBeck - Dynamic dropdowns
                //this.cboMerchandise_SelectionChangeCommitted(this, z);	//TT#301-MD-VStuart-Version 5.0-Controls
                cboMerchandise.Items.Clear();//TT#7 - MD - RBeck - Dynamic dropdowns
                // BEGIN TT#41-MD - GTaylor - UC#2
                cboInventoryBasis.DataSource = null;
                //this.cboInventoryBasis_SelectionChangeCommitted(this, z);	//TT#301-MD-VStuart-Version 5.0-Controls
                cboInventoryBasis.Items.Clear();
                // END TT#41-MD - GTaylor - UC#2
			}

            // BEGIN TT#41-MD - GTaylor - UC#2
            private void Load_IB_Combo()
            {
                //MerchandiseDataTable3 = MerchandiseDataTable.Copy();
                try
                {
                    cboInventoryBasis.DataSource = MerchandiseDataTable3;
                    //var z = new EventArgs();	//TT#301-MD-VStuart-Version 5.0-Controls
                    //this.cboInventoryBasis_SelectionChangeCommitted(this, z);	//TT#301-MD-VStuart-Version 5.0-Controls
                    cboInventoryBasis.DisplayMember = "text";
                    cboInventoryBasis.ValueMember = "seqno";
                }
                catch (Exception ex)
                {
                    HandleException(ex);
                }

            }
            // END TT#41-MD - GTaylor - UC#2

			/// <summary>
			/// Private method loads the Fill Size Holes Data associated with the current Fill Size Hole Method.
			/// </summary>
			private void LoadSizeNeedValues()
			{
				try
                {   // Begin TT#356 - RMatelic - Allocation Options->detail pack settings - will not override to blank within the method
                    if (_sizeNeedMethod.MaxPackNeedTolerance != double.MaxValue)
                    {
                        txtMaxPackAllocNeedTolerance.Text = _sizeNeedMethod.MaxPackNeedTolerance.ToString();
                    }
                    if (_sizeNeedMethod.AvgPackDeviationTolerance != double.MaxValue)
                    {
                        txtAvgPackDevTolerance.Text = _sizeNeedMethod.AvgPackDeviationTolerance.ToString();
                    }
                    
                    if (_sizeNeedMethod.OverrideAvgPackDevTolerance)
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
                    if (_sizeNeedMethod.PackToleranceStepped)
                    {
                        cbxStepped.Checked = true;
                    }
                    else
                    {
                        cbxStepped.Checked = false;
                    }
                    if (_sizeNeedMethod.PackToleranceNoMaxStep)
                    {
                        cbxNoMaxStep.Checked = true;
                    }
                    else
                    {
                        cbxNoMaxStep.Checked = false;
                    }
                    // end TT#1365 - JEllis - FL Detail Pack Size Need Enhancement
                    if (_sizeNeedMethod.OverrideMaxPackNeedTolerance)
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
                    // End TT#356  

//					if (_sizeNeedMethod.SizeGroupRid != Include.NoRID)
//					{
//						cboSizeGroup.SelectedValue = _sizeNeedMethod.SizeGroupRid;
//					}
					
					// begin MID track 3781 Size Curve not required 
					if (_sizeNeedMethod.SizeGroupRid != Include.NoRID)
					{
						cboSizeGroup.SelectedValue = _sizeNeedMethod.SizeGroupRid;
                        //this.cboSizeGroup_SelectionChangeCommitted(source, new EventArgs()); // TT#301-MD - JSmith - Controls are not functioning properly
						_sizeNeedMethod.GetSizesUsing = eGetSizes.SizeGroupRID;
						_sizeNeedMethod.GetDimensionsUsing = eGetDimensions.SizeGroupRID;
						cboSizeCurve.SelectedValue = Include.NoRID;
                        //this.cboSizeCurve_SelectionChangeCommitted(source, new EventArgs()); // TT#301-MD - JSmith - Controls are not functioning properly
						_sizeNeedMethod.SizeCurveGroupRid = Include.NoRID;
						if (!_sizeNeedMethod.ConstraintsLoaded)
						{
							_sizeNeedMethod.CreateConstraintData();
						}
						if (FormLoaded)
						{
							BindAllSizeGrid(_sizeNeedMethod.MethodConstraints);
							CheckExpandAll();
						}
					}
					// end MID track 3781 Size Curve not required 
						
					if (_sizeNeedMethod.SizeCurveGroupRid != Include.NoRID)
					{
						cboSizeCurve.SelectedValue = _sizeNeedMethod.SizeCurveGroupRid;
                        //this.cboSizeCurve_SelectionChangeCommitted(source, new EventArgs()); // TT#301-MD - JSmith - Controls are not functioning properly
						// begin MID Track 3781 Size Curve not required 
						_sizeNeedMethod.GetSizesUsing = eGetSizes.SizeCurveGroupRID;
						_sizeNeedMethod.GetDimensionsUsing = eGetDimensions.SizeCurveGroupRID;
						cboSizeGroup.SelectedValue = Include.NoRID;
                        //this.cboSizeGroup_SelectionChangeCommitted(source, new EventArgs()); // TT#301-MD - JSmith - Controls are not functioning properly
						_sizeNeedMethod.SizeGroupRid = Include.NoRID;
						if (!_sizeNeedMethod.ConstraintsLoaded)
						{
							_sizeNeedMethod.CreateConstraintData();
						}
						if (FormLoaded)
						{
							BindAllSizeGrid(_sizeNeedMethod.MethodConstraints);
							CheckExpandAll();
						}
						// end MID Track 3781 Size Curve not required 
					}
					 
					if (_sizeNeedMethod.SizeAlternateRid != Include.NoRID)
					{
						cboAlternates.SelectedValue = _sizeNeedMethod.SizeAlternateRid;
					}
					if (_sizeNeedMethod.SizeConstraintRid != Include.NoRID)
					{
						cboConstraints.SelectedValue = _sizeNeedMethod.SizeConstraintRid;
                        //this.cboConstraints_SelectionChangeCommitted(source, new EventArgs()); // TT#301-MD - JSmith - Controls are not functioning properly
					}
					// begin MID Track 3619 Remove Fringe
					//if (_sizeNeedMethod.SizeFringeRid == Include.NoRID)
					//{
					//	HideQuantityColumn(true);								
					//}
					//else
					//{
					//	cboFringe.SelectedValue = _sizeNeedMethod.SizeFringeRid;
					//	HideQuantityColumn(false);								
					//}
					HideQuantityColumn(false);
					// end MID Track 3619 Remove Fringe

                    if (_sizeNeedMethod.SG_RID != Include.NoRID)
                    {
                        this.cboStoreAttribute.SelectedValue = _sizeNeedMethod.SG_RID;
                        //this.cboStoreAttribute_SelectionChangeCommitted(source, new EventArgs()); // TT#301-MD - JSmith - Controls are not functioning properly
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
                    // Begin TT#1530 - RMatelic - Record locking violations from Store Attribute change
                    else
                    {
                        cboStoreAttribute.Enabled = FunctionSecurity.AllowUpdate;
                    }
                    // End TT#1530

                    AdjustTextWidthComboBox_DropDown(cboStoreAttribute); //TT#7 - MD - RBeck - Dynamic dropdowns

					//Load Merchandise Node or Level Text to combo box
					HierarchyNodeProfile hnp;
					if (_sizeNeedMethod.MerchHnRid != Include.NoRID)
					{
						//Begin Track #5378 - color and size not qualified
//						hnp = SAB.HierarchyServerSession.GetNodeData(_sizeNeedMethod.MerchHnRid);
                        hnp = SAB.HierarchyServerSession.GetNodeData(_sizeNeedMethod.MerchHnRid, true, true);
						//End Track #5378
						AddNodeToMerchandiseCombo ( hnp );
					}
					else
					{ 
						if (_sizeNeedMethod.MerchPhRid != Include.NoRID)
						{
							SetComboToLevel(_sizeNeedMethod.MerchPhlSequence);
						}
						else
						{
                            cboMerchandise.SelectedIndex = 0;//TT#7 - MD - RBeck - Dynamic dropdowns
                            //this.cboMerchandise_SelectionChangeCommitted(source, new EventArgs()); // TT#301-MD - JSmith - Controls are not functioning properly
						}
					}

                    // BEGIN TT#41-MD - GTaylor - UC#2
                    if (_sizeNeedMethod.IB_MERCH_HN_RID != Include.NoRID)
                    {
                        hnp = SAB.HierarchyServerSession.GetNodeData(_sizeNeedMethod.IB_MERCH_HN_RID, true, true);
                        AddNodeToIBCombo(hnp);
                    }
                    else
                    {
                        if (_sizeNeedMethod.IB_MERCH_PH_RID != Include.NoRID)
                        {
                            SetIBComboToLevel(_sizeNeedMethod.IB_MERCH_PHL_SEQ);
                        }
                        else
                        {
                            cboInventoryBasis.SelectedIndex = 0;
                            //this.cboInventoryBasis_SelectionChangeCommitted(source, new EventArgs()); // TT#301-MD - JSmith - Controls are not functioning properly
                        }
                    }
                    // END TT#41-MD - GTaylor - UC#2

					// begin Generic Size Curve data
					if (_sizeNeedMethod.GenCurveCharGroupRID != Include.NoRID)
					{
						this.cboHeaderChar.SelectedValue = _sizeNeedMethod.GenCurveCharGroupRID;
                        //this.cboHeaderChar_SelectionChangeCommitted(source, new EventArgs()); // TT#301-MD - JSmith - Controls are not functioning properly
					}
                    
                    // Begin TT#413 - RMatelic - Add new Generic Size Curve names to correspond with the new Size Curves being generated within Node Properties settings.
                    this.cboNameExtension.SelectedValue = _sizeNeedMethod.GenCurveNsccdRID;
                    //this.cboNameExtension_SelectionChangeCommitted(source, new EventArgs()); // TT#301-MD - JSmith - Controls are not functioning properly
                    // End TT#413  

					switch (_sizeNeedMethod.GenCurveMerchType)
					{
						case eMerchandiseType.Undefined:
							cboHierarchyLevel.SelectedIndex = 0;
                            //this.cboHierarchyLevel_SelectionChangeCommitted(source, new EventArgs()); // TT#301-MD - JSmith - Controls are not functioning properly
							break;
				
						case eMerchandiseType.OTSPlanLevel:
						case eMerchandiseType.HierarchyLevel:
							SetGenSizeCurveComboToLevel(_sizeNeedMethod.GenCurvePhlSequence);
							break;
					
						case eMerchandiseType.Node:
							//Begin Track #5378 - color and size not qualified
//							hnp = SAB.HierarchyServerSession.GetNodeData(_sizeNeedMethod.GenCurveHnRID);
                            hnp = SAB.HierarchyServerSession.GetNodeData(_sizeNeedMethod.GenCurveHnRID, true, true);
							//End Track #5378
							AddNodeToGenSizeCurveCombo(hnp);
							break;
					
					}
					
					cbColor.Checked = _sizeNeedMethod.GenCurveColorInd;
                    // Begin TT#438 - RMatelic - Size Curve generated in node properties- used default in Size Need method but requires a size curve (incorrect)
                    if (SAB.ApplicationServerSession.GlobalOptions.GenericSizeCurveNameType == eGenericSizeCurveNameType.NodePropertiesName && cbColor.Checked)
                    {
                        cbColor.Checked = false;
                    }
                    // End TT#438  
                    // end Generic Size Curve data
                    
					// begin Generic Size Constraint data
					if (_sizeNeedMethod.GenConstraintCharGroupRID != Include.NoRID)
					{
						this.cboConstrHeaderChar.SelectedValue = _sizeNeedMethod.GenConstraintCharGroupRID;
					}
					
					switch (_sizeNeedMethod.GenConstraintMerchType)
					{
						case eMerchandiseType.Undefined:
							cboConstrHierLevel.SelectedIndex = 0;
							break;
				
						case eMerchandiseType.OTSPlanLevel:
						case eMerchandiseType.HierarchyLevel:
							SetGenSizeConstraintComboToLevel(_sizeNeedMethod.GenConstraintPhlSequence);
							break;
					
						case eMerchandiseType.Node:
							hnp = SAB.HierarchyServerSession.GetNodeData(_sizeNeedMethod.GenConstraintHnRID);
							AddNodeToGenSizeConstraintCombo(hnp);
							break;
					
					}
					
					cbConstrColor.Checked = _sizeNeedMethod.GenConstraintColorInd;
					// end Generic Size Constraint data

					// BEGIN MID Track #4826 - JSmith - Normalize Size Curves
					if (_sizeNeedMethod.NormalizeSizeCurves)
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

                    if (_sizeNeedMethod.OverrideVSWSizeConstraints)
                    {
                        cboSizeConstraints.Enabled = true;
                        chkVSWSizeConstraintsOverride.Checked = true;
                        cboSizeConstraints.SelectedValue = _sizeNeedMethod.VSWSizeConstraints; 
                    }
                    else
                    {
                        cboSizeConstraints.Enabled = false;
                        chkVSWSizeConstraintsOverride.Checked = false;
                        cboSizeConstraints.SelectedValue = _sizeNeedMethod.VSWSizeConstraints; 
                    }
                    // END TT#246-MD - AGallagher - VSW Size - Add In-store Size Constraint options 
                    // begin TT#2155 - JEllis - Fill Size Holes Null Reference
                    SetApplyRulesOnly_State();
                    // end TT#2155 - JEllis - Fill Size Holes Null Reference
				}
				catch (Exception ex)
				{
					MessageBox.Show(ex.ToString());
				}

						
			}


			/// <summary>
			/// Binds ugAllSize grid on the Constraint tab to FSHDataSet property from the FillSizeHolesMethod
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


		#endregion

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
				_sizeNeedMethod = new SizeNeedMethod(SAB,aMethodRID);
				base.UpdateWorkflowMethod(aLockStatus, eSecurityFunctions.AllocationMethodsUserSizeNeed, eSecurityFunctions.AllocationMethodsGlobalSizeNeed);

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
				_sizeNeedMethod = new SizeNeedMethod(SAB,method_RID);
				return Delete();
			}
			catch(DatabaseForeignKeyViolation keyVio)
			{
				throw keyVio;
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
			catch
			{
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
                {
                DataRow myDataRow2 = null;
                for (int i = 0; i < MerchandiseDataTable3.Rows.Count; i++)
                {
                    myDataRow2 = MerchandiseDataTable3.Rows[i];
                    if (Convert.ToInt32(myDataRow2["seqno"], CultureInfo.CurrentUICulture) == Convert.ToInt32(cboInventoryBasis.SelectedValue, CultureInfo.CurrentUICulture))
                    {
                        break;
                    }
                }
                    //DataRow myDataRow2 = MerchandiseDataTable3.Rows[cboInventoryBasis.SelectedIndex];
                    // END TT#41-MD -- AGallagher - UC#2
                    eMerchandiseType MerchandiseType2 = (eMerchandiseType)(Convert.ToInt32(myDataRow2["leveltypename"], CultureInfo.CurrentUICulture));

                    _sizeNeedMethod.IB_MerchandiseType = MerchandiseType2;

                    // BEGIN TT#41-MD -- AGallagher - UC#2
                    if (MerchandiseType2 == eMerchandiseType.Undefined)
                    {
                     _sizeNeedMethod.IB_MERCH_HN_RID = Include.NoRID;
                     _sizeNeedMethod.IB_MERCH_PH_RID = Include.NoRID;
                     _sizeNeedMethod.IB_MERCH_PHL_SEQ = 0;
                    }
                    else
                   // END TT#41-MD -- AGallagher - UC#2
                    switch (MerchandiseType2)
                    {
                        case eMerchandiseType.Node:
                            _sizeNeedMethod.IB_MERCH_HN_RID = Convert.ToInt32(myDataRow2["key"], CultureInfo.CurrentUICulture);
                            break;
                        case eMerchandiseType.HierarchyLevel:
                            _sizeNeedMethod.IB_MERCH_PHL_SEQ = Convert.ToInt32(myDataRow2["key"], CultureInfo.CurrentUICulture);
                            _sizeNeedMethod.IB_MERCH_PH_RID = HP.Key;
                            _sizeNeedMethod.IB_MERCH_HN_RID = Include.NoRID;
                            break;
                        case eMerchandiseType.OTSPlanLevel:
                            _sizeNeedMethod.IB_MERCH_HN_RID = Include.NoRID;
                            _sizeNeedMethod.IB_MERCH_PH_RID = Include.NoRID;
                            _sizeNeedMethod.IB_MERCH_PHL_SEQ = 0;
                            break;
                    }
                }
                // BEGIN TT#41-MD -- AGallagher - UC#2
                //else
                //{
                //    _sizeNeedMethod.IB_MERCH_HN_RID = Include.NoRID;
                //    _sizeNeedMethod.IB_MERCH_PH_RID = Include.NoRID;
                //    _sizeNeedMethod.IB_MERCH_PHL_SEQ = 0;
                //}
                // END TT#41-MD -- AGallagher - UC#2
                // END TT#41-MD - GTaylor - UC#2
                // Begin TT#356 - RMatelic - Allocation Options->detail pack settings - will not override to blank within the method
                //_sizeNeedMethod.MaxPackNeedTolerance = (txtMaxPackNeedTolerance.Text.Trim() == string.Empty)
                //                                            ? SAB.ApplicationServerSession.GlobalOptions.MaxSizeErrorPercent
                //                                            : Convert.ToDouble(txtMaxPackNeedTolerance.Text, CultureInfo.CurrentUICulture);

                //_sizeNeedMethod.AvgPackDeviationTolerance = (txtAvgPackDevTolerance.Text.Trim() == string.Empty) 
                //                                                ? SAB.ApplicationServerSession.GlobalOptions.PackSizeErrorPercent
                //                                                : Convert.ToDouble(txtAvgPackDevTolerance.Text, CultureInfo.CurrentUICulture);
                if (cbxOverrideAvgPackDev.Checked)
                {
                    _sizeNeedMethod.OverrideAvgPackDevTolerance = true;
                    if (txtAvgPackDevTolerance.Text.Trim() == string.Empty)
                    {
                        _sizeNeedMethod.AvgPackDeviationTolerance = Double.MaxValue;
                    }
                    else
                    {
                        _sizeNeedMethod.AvgPackDeviationTolerance = Convert.ToDouble(txtAvgPackDevTolerance.Text, CultureInfo.CurrentUICulture);
                    }
                }
                else
                {
                    _sizeNeedMethod.OverrideAvgPackDevTolerance = false;
                    _sizeNeedMethod.AvgPackDeviationTolerance = SAB.ApplicationServerSession.GlobalOptions.PackSizeErrorPercent;
                }
                if (cbxOverrideMaxPackAllocNeed.Checked)
                {
                    _sizeNeedMethod.OverrideMaxPackNeedTolerance = true;
                    if (txtMaxPackAllocNeedTolerance.Text.Trim() == string.Empty)
                    {
                        _sizeNeedMethod.MaxPackNeedTolerance = Double.MaxValue;
                    }
                    else
                    {
                        _sizeNeedMethod.MaxPackNeedTolerance = Convert.ToDouble(txtMaxPackAllocNeedTolerance.Text, CultureInfo.CurrentUICulture);
                    }
                    // begin TT#1365 - JEllis - FL Detail Packs Size Need Enhancement
                    _sizeNeedMethod.PackToleranceNoMaxStep = cbxNoMaxStep.Checked;
                    _sizeNeedMethod.PackToleranceStepped = cbxStepped.Checked;
                    // end TT#1365 - JEllis - FL Detail Packs Size Need Enhancement
                }
                else
                {
                    _sizeNeedMethod.OverrideMaxPackNeedTolerance = false;
                    _sizeNeedMethod.MaxPackNeedTolerance = SAB.ApplicationServerSession.GlobalOptions.MaxSizeErrorPercent;
                    // begin TT#1365 - JEllis - FL Detail Packs Size Need Enhancement
                    _sizeNeedMethod.PackToleranceNoMaxStep = SAB.ApplicationServerSession.GlobalOptions.PackToleranceNoMaxStep;
                    _sizeNeedMethod.PackToleranceStepped = SAB.ApplicationServerSession.GlobalOptions.PackToleranceStepped;
                    // end TT#1365 - JEllis - FL Detail Packs Size Need Enhancement
                }
                // End TT#356  
                
//				_sizeNeedMethod.SizeGroupRid = (cboSizeGroup.Text.Trim() == string.Empty) 
//													? Include.NoRID 
//													: Convert.ToInt32(cboSizeGroup.SelectedValue.ToString(),CultureInfo.CurrentUICulture);

				// begin MID track 3781 Size Curve Not required
				//_sizeNeedMethod.SizeGroupRid = Include.NoRID;
				_sizeNeedMethod.SizeGroupRid = (cboSizeGroup.Text.Trim() == string.Empty) 
				    								? Include.NoRID 
													: Convert.ToInt32(cboSizeGroup.SelectedValue.ToString(),CultureInfo.CurrentUICulture);
                // end MID track 3781 Size Curve Not required


				_sizeNeedMethod.SizeCurveGroupRid = (cboSizeCurve.Text.Trim() == string.Empty) 
														? Include.NoRID 
														: Convert.ToInt32(cboSizeCurve.SelectedValue.ToString(), CultureInfo.CurrentUICulture);


				_sizeNeedMethod.SizeAlternateRid = (cboAlternates.Text.Trim() == string.Empty) 
					? Include.NoRID 
					: Convert.ToInt32(cboAlternates.SelectedValue.ToString(), CultureInfo.CurrentUICulture);
				_sizeNeedMethod.SizeConstraintRid = (cboConstraints.Text.Trim() == string.Empty) 
					? Include.NoRID 
					: Convert.ToInt32(cboConstraints.SelectedValue.ToString(), CultureInfo.CurrentUICulture);
				// begin MID Track 3619 Remove Fringe
				//_sizeNeedMethod.SizeFringeRid = (cboFringe.Text.Trim() == string.Empty) 
				//	? Include.NoRID 
				//	: Convert.ToInt32(cboFringe.SelectedValue.ToString(), CultureInfo.CurrentUICulture);
                // end MID Track 3619 Remove Fringe


				//MERCHANDISE LEVEL
				//*****************************************************************
				DataRow myDataRow = MerchandiseDataTable.Rows[cboMerchandise.SelectedIndex];
				eMerchandiseType MerchandiseType = (eMerchandiseType)(Convert.ToInt32(myDataRow["leveltypename"], CultureInfo.CurrentUICulture)); 
				_sizeNeedMethod.MerchType = MerchandiseType;
		
				switch(MerchandiseType)
				{
					case eMerchandiseType.Node:
						_sizeNeedMethod.MerchHnRid = Convert.ToInt32(myDataRow["key"], CultureInfo.CurrentUICulture);
						break;
					case eMerchandiseType.HierarchyLevel:
						_sizeNeedMethod.MerchPhlSequence = Convert.ToInt32(myDataRow["seqno"], CultureInfo.CurrentUICulture);
						_sizeNeedMethod.MerchPhRid = HP.Key;
						_sizeNeedMethod.MerchHnRid = Include.NoRID;
						break;
					case eMerchandiseType.OTSPlanLevel:
						_sizeNeedMethod.MerchHnRid = Include.NoRID;
						_sizeNeedMethod.MerchPhRid = Include.NoRID;
						_sizeNeedMethod.MerchPhlSequence = 0;
						break;
				}
				//*****************************************************************

				// begin Generic Size Curve data
				_sizeNeedMethod.GenCurveCharGroupRID =  Convert.ToInt32(cboHeaderChar.SelectedValue, CultureInfo.CurrentUICulture);
				_sizeNeedMethod.GenCurveColorInd = cbColor.Checked; 
				
				DataRowView drv = (DataRowView)cboHierarchyLevel.SelectedItem;
				DataRow dRow  = drv.Row;
				 
				_sizeNeedMethod.GenCurveMerchType = (eMerchandiseType)(Convert.ToInt32(dRow["leveltypename"], CultureInfo.CurrentUICulture)); 
				switch(_sizeNeedMethod.GenCurveMerchType)
				{
					case eMerchandiseType.Node:
						_sizeNeedMethod.GenCurveHnRID = Convert.ToInt32(dRow["key"], CultureInfo.CurrentUICulture);
						_sizeNeedMethod.GenCurvePhRID = Include.NoRID;
						_sizeNeedMethod.GenCurvePhlSequence = 0;
						break;
					case eMerchandiseType.HierarchyLevel:
						_sizeNeedMethod.GenCurvePhlSequence  = Convert.ToInt32(dRow["seqno"], CultureInfo.CurrentUICulture);
						_sizeNeedMethod.GenCurvePhRID = HP.Key;
						_sizeNeedMethod.GenCurveHnRID = Include.NoRID;
						break;
					case eMerchandiseType.OTSPlanLevel:
					case eMerchandiseType.Undefined:
						_sizeNeedMethod.GenCurveHnRID = Include.NoRID;
						_sizeNeedMethod.GenCurvePhRID = Include.NoRID;
						_sizeNeedMethod.GenCurvePhlSequence  = 0;
						break;
				}
				// end  Generic Size Curve data

				// begin Generic Size Constraint data
				_sizeNeedMethod.GenConstraintCharGroupRID =  Convert.ToInt32(cboConstrHeaderChar.SelectedValue, CultureInfo.CurrentUICulture);
				_sizeNeedMethod.GenConstraintColorInd = cbConstrColor.Checked; 
				
				drv = (DataRowView)cboConstrHierLevel.SelectedItem;
				dRow  = drv.Row;
				 
				_sizeNeedMethod.GenConstraintMerchType = (eMerchandiseType)(Convert.ToInt32(dRow["leveltypename"], CultureInfo.CurrentUICulture)); 
				switch(_sizeNeedMethod.GenConstraintMerchType)
				{
					case eMerchandiseType.Node:
						_sizeNeedMethod.GenConstraintHnRID = Convert.ToInt32(dRow["key"], CultureInfo.CurrentUICulture);
						_sizeNeedMethod.GenConstraintPhRID = Include.NoRID;
						_sizeNeedMethod.GenConstraintPhlSequence = 0;
						break;
					case eMerchandiseType.HierarchyLevel:
						_sizeNeedMethod.GenConstraintPhlSequence  = Convert.ToInt32(dRow["seqno"], CultureInfo.CurrentUICulture);
						_sizeNeedMethod.GenConstraintPhRID = HP.Key;
						_sizeNeedMethod.GenConstraintHnRID = Include.NoRID;
						break;
					case eMerchandiseType.OTSPlanLevel:
					case eMerchandiseType.Undefined:
						_sizeNeedMethod.GenConstraintHnRID = Include.NoRID;
						_sizeNeedMethod.GenConstraintPhRID = Include.NoRID;
						_sizeNeedMethod.GenConstraintPhlSequence  = 0;
						break;
				}
				// end  Generic Size Constraint data

				// BEGIN MID Track #4826 - JSmith - Normalize Size Curves
				_sizeNeedMethod.NormalizeSizeCurvesDefaultIsOverridden = cbxOverrideNormalizeDefault.Checked;
				_sizeNeedMethod.NormalizeSizeCurves = radNormalizeSizeCurves_Yes.Checked;
				// END MID Track #4826

                // Begin TT#413 - JSmith - Add new Generic Size Curve names to correspond with the new Size Curves being generated within Node Properties settings.
                _sizeNeedMethod.UseDefaultCurve = cbxUseDefaultcurve.Checked;
                // End TT#413

                // begin TT#2155 - JEllis - Fill Size Holes Null Reference
                _sizeNeedMethod.ApplyRulesOnly = cbxApplyRulesOnly.Checked;
                // end TT#2155 - JEllis - Fill Size Holes Null Reference

                // Begin TT#413 - RMatelic - Add new Generic Size Curve names to correspond with the new Size Curves being generated within Node Properties settings.
                if (SAB.ApplicationServerSession.GlobalOptions.GenericSizeCurveNameType == eGenericSizeCurveNameType.NodePropertiesName)
                {
                    _sizeNeedMethod.GenCurveNsccdRID = Convert.ToInt32(cboNameExtension.SelectedValue, CultureInfo.CurrentUICulture);
                    _sizeNeedMethod.GenCurveCharGroupRID = Include.NoRID;
                }
                else
                {
                    _sizeNeedMethod.GenCurveNsccdRID = Include.NoRID;
                }
                // End TT#413

                // BEGIN TT#246-MD - AGallagher - VSW Size - Add In-store Size Constraint options 
                _sizeNeedMethod.OverrideVSWSizeConstraints = chkVSWSizeConstraintsOverride.Checked;
                _sizeNeedMethod.VSWSizeConstraints = (eVSWSizeConstraints) cboSizeConstraints.SelectedValue; 
                // END TT#246-MD - AGallagher - VSW Size - Add In-store Size Constraint options 

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
			bool isFormValid = true;

			try
			{
//				//initialize all fields to not having an error
				ErrorMessages.Clear();
				//AttachErrors(cboSizeGroup);
                // Begin TT#438 - RMatelic - Size Curve generated in node properties- used default in Size Need method but requires a size curve (incorrect)
                //AttachErrors(cboSizeCurve);
                AttachErrors(gbSizeCurve);
                // End TT#438  
				
//			if (cboSizeGroup.Text.Trim() == string.Empty)
//			{
//				isFormValid = false;
//				ErrorMessages.Add(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_SizeGroupRequired));
//				AttachErrors(cboSizeGroup);
//			}
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
			catch
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
			catch
			{
				throw;
			}
		}


		/// <summary>
		/// Use to set the specific method object before updating
		/// </summary>
		override protected void SetObject()
		{
			try
			{
				ABM = _sizeNeedMethod;
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
				_sizeNeedMethod = new SizeNeedMethod(SAB,Include.NoRID);
				SetObject();
				base.NewWorkflowMethod(aParentNode, eSecurityFunctions.AllocationMethodsUserFillSizeHoles, eSecurityFunctions.AllocationMethodsGlobalFillSizeHoles);
				// Store Attribute to allocation default
				GlobalOptionsProfile gop = new GlobalOptionsProfile(-1);
				gop.LoadOptions();
				_sizeNeedMethod.SG_RID = gop.AllocationStoreGroupRID;
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
				_sizeNeedMethod = new SizeNeedMethod(SAB, aMethodRID);
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
        /// <param name="aMethodRID">The record ID of the method</param>
		override public void ProcessWorkflowMethod(int aMethodRID)
		{
			try
			{       
				_sizeNeedMethod = new SizeNeedMethod(SAB, aMethodRID);
				ProcessAction(eMethodType.SizeNeedAllocation, true);
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
		#endregion MIDFormBase Overrides

		#region SizeMethodsFormBase Overrides

		override protected bool IsActiveRowValid(UltraGridRow activeRow)
		{
			try
			{
				bool IsValid = true;

				//COMMON BAND VALIDATIONS
				//==================================================================================

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

					column = ugRules.DisplayLayout.Bands[C_SET].Columns["SIZE_QUANTITY"];
					column.Header.VisiblePosition = 5;
					column.Header.Caption = "Qty";

					ugRules.DisplayLayout.Bands[C_SET].Columns["SGL_RID"].Hidden = true;
					ugRules.DisplayLayout.Bands[C_SET].Columns["METHOD_RID"].Hidden = true;
					ugRules.DisplayLayout.Bands[C_SET].Columns["ROW_TYPE_ID"].Hidden = true;
					ugRules.DisplayLayout.Bands[C_SET].Columns["SIZE_QUANTITY"].Hidden = false;

					ugRules.DisplayLayout.Bands[C_SET].Override.AllowDelete = DefaultableBoolean.False;
					ugRules.DisplayLayout.Bands[C_SET].AddButtonCaption = "Set";
				}
				#endregion


				#region ALL COLOR BAND

				if (ugRules.DisplayLayout.Bands.Exists(C_SET_ALL_CLR))
				{
					ugRules.DisplayLayout.Bands[C_SET_ALL_CLR].Columns["BAND_DSC"].Header.VisiblePosition = 0;
					
					ugRules.DisplayLayout.Bands[C_SET_ALL_CLR].Columns["SIZE_RULE"].Header.VisiblePosition = 1;
					ugRules.DisplayLayout.Bands[C_SET_ALL_CLR].Columns["SIZE_QUANTITY"].Header.VisiblePosition = 2;

					ugRules.DisplayLayout.Bands[C_SET_ALL_CLR].Columns["METHOD_RID"].Hidden = true;
					ugRules.DisplayLayout.Bands[C_SET_ALL_CLR].Columns["SGL_RID"].Hidden = true;
					ugRules.DisplayLayout.Bands[C_SET_ALL_CLR].Columns["ROW_TYPE_ID"].Hidden = true;
					ugRules.DisplayLayout.Bands[C_SET_ALL_CLR].Columns["COLOR_CODE_RID"].Hidden = true;
					ugRules.DisplayLayout.Bands[C_SET_ALL_CLR].Columns["SIZES_RID"].Hidden = true;
					ugRules.DisplayLayout.Bands[C_SET_ALL_CLR].Columns["SIZE_CODE_RID"].Hidden = true;
					ugRules.DisplayLayout.Bands[C_SET_ALL_CLR].Columns["DIMENSIONS_RID"].Hidden = true;
					ugRules.DisplayLayout.Bands[C_SET_ALL_CLR].Columns["SIZE_QUANTITY"].Hidden = false;
					ugRules.DisplayLayout.Bands[C_SET_ALL_CLR].Columns["SIZE_SEQ"].Hidden = true;

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
					ugRules.DisplayLayout.Bands[C_SET_CLR].Columns["SIZE_QUANTITY"].Header.VisiblePosition = 2;

					ugRules.DisplayLayout.Bands[C_SET_CLR].Columns["SGL_RID"].Hidden = true;
					ugRules.DisplayLayout.Bands[C_SET_CLR].Columns["METHOD_RID"].Hidden = true;
					ugRules.DisplayLayout.Bands[C_SET_CLR].Columns["ROW_TYPE_ID"].Hidden = true;
					ugRules.DisplayLayout.Bands[C_SET_CLR].Columns["SIZES_RID"].Hidden = true;
					ugRules.DisplayLayout.Bands[C_SET_CLR].Columns["SIZE_CODE_RID"].Hidden = true;
					ugRules.DisplayLayout.Bands[C_SET_CLR].Columns["DIMENSIONS_RID"].Hidden = true;
					ugRules.DisplayLayout.Bands[C_SET_CLR].Columns["BAND_DSC"].Hidden = true;
					ugRules.DisplayLayout.Bands[C_SET_CLR].Columns["SIZE_QUANTITY"].Hidden = false;
					ugRules.DisplayLayout.Bands[C_SET_CLR].Columns["SIZE_SEQ"].Hidden = true; // issue 3826

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
					ugRules.DisplayLayout.Bands[C_CLR_SZ].Columns["SIZE_QUANTITY"].Header.VisiblePosition = 2;

					ugRules.DisplayLayout.Bands[C_CLR_SZ].Columns["DIMENSIONS_RID"].Hidden = true;
					ugRules.DisplayLayout.Bands[C_CLR_SZ].Columns["SIZES_RID"].Hidden = true;
					ugRules.DisplayLayout.Bands[C_CLR_SZ].Columns["METHOD_RID"].Hidden = true;
					ugRules.DisplayLayout.Bands[C_CLR_SZ].Columns["SGL_RID"].Hidden = true;
					ugRules.DisplayLayout.Bands[C_CLR_SZ].Columns["ROW_TYPE_ID"].Hidden = true;
					ugRules.DisplayLayout.Bands[C_CLR_SZ].Columns["COLOR_CODE_RID"].Hidden = true;
					ugRules.DisplayLayout.Bands[C_CLR_SZ].Columns["BAND_DSC"].Hidden = true;
					ugRules.DisplayLayout.Bands[C_CLR_SZ].Columns["SIZE_QUANTITY"].Hidden = false;
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
					ugRules.DisplayLayout.Bands[C_ALL_CLR_SZ].Columns["SIZE_QUANTITY"].Header.VisiblePosition = 2;

					ugRules.DisplayLayout.Bands[C_ALL_CLR_SZ].Columns["SIZES_RID"].Hidden = true;
					ugRules.DisplayLayout.Bands[C_ALL_CLR_SZ].Columns["METHOD_RID"].Hidden = true;
					ugRules.DisplayLayout.Bands[C_ALL_CLR_SZ].Columns["SGL_RID"].Hidden = true;
					ugRules.DisplayLayout.Bands[C_ALL_CLR_SZ].Columns["DIMENSIONS_RID"].Hidden = true;
					ugRules.DisplayLayout.Bands[C_ALL_CLR_SZ].Columns["ROW_TYPE_ID"].Hidden = true;
					ugRules.DisplayLayout.Bands[C_ALL_CLR_SZ].Columns["COLOR_CODE_RID"].Hidden = true;
					ugRules.DisplayLayout.Bands[C_ALL_CLR_SZ].Columns["BAND_DSC"].Hidden = true;
					ugRules.DisplayLayout.Bands[C_ALL_CLR_SZ].Columns["SIZE_QUANTITY"].Hidden = false;
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
					ugRules.DisplayLayout.Bands[C_ALL_CLR_SZ_DIM].Columns["SIZE_QUANTITY"].Header.VisiblePosition = 2;

					ugRules.DisplayLayout.Bands[C_ALL_CLR_SZ_DIM].Columns["METHOD_RID"].Hidden = true;
					ugRules.DisplayLayout.Bands[C_ALL_CLR_SZ_DIM].Columns["SIZES_RID"].Hidden = true;
					ugRules.DisplayLayout.Bands[C_ALL_CLR_SZ_DIM].Columns["SIZE_CODE_RID"].Hidden = true;
					ugRules.DisplayLayout.Bands[C_ALL_CLR_SZ_DIM].Columns["SGL_RID"].Hidden = true;
					ugRules.DisplayLayout.Bands[C_ALL_CLR_SZ_DIM].Columns["ROW_TYPE_ID"].Hidden = true;
					ugRules.DisplayLayout.Bands[C_ALL_CLR_SZ_DIM].Columns["COLOR_CODE_RID"].Hidden = true;
					ugRules.DisplayLayout.Bands[C_ALL_CLR_SZ_DIM].Columns["BAND_DSC"].Hidden = true;
					ugRules.DisplayLayout.Bands[C_ALL_CLR_SZ_DIM].Columns["SIZE_QUANTITY"].Hidden = false;
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
					ugRules.DisplayLayout.Bands[C_CLR_SZ_DIM].Columns["SIZE_QUANTITY"].Header.VisiblePosition = 2;


					ugRules.DisplayLayout.Bands[C_CLR_SZ_DIM].Columns["METHOD_RID"].Hidden = true;
					ugRules.DisplayLayout.Bands[C_CLR_SZ_DIM].Columns["SIZES_RID"].Hidden = true;
					ugRules.DisplayLayout.Bands[C_CLR_SZ_DIM].Columns["SIZE_CODE_RID"].Hidden = true;
					ugRules.DisplayLayout.Bands[C_CLR_SZ_DIM].Columns["SGL_RID"].Hidden = true;
					ugRules.DisplayLayout.Bands[C_CLR_SZ_DIM].Columns["ROW_TYPE_ID"].Hidden = true;
					ugRules.DisplayLayout.Bands[C_CLR_SZ_DIM].Columns["COLOR_CODE_RID"].Hidden = true;
					ugRules.DisplayLayout.Bands[C_CLR_SZ_DIM].Columns["BAND_DSC"].Hidden = true;
					ugRules.DisplayLayout.Bands[C_CLR_SZ_DIM].Columns["SIZE_QUANTITY"].Hidden = false;
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


		private void HideQuantityColumn(bool isVisible)
		{
			try
			{
				if (ugRules.DisplayLayout.Bands.Exists(C_SET))
				{
					ugRules.DisplayLayout.Bands[C_SET].Columns["SIZE_QUANTITY"].Hidden = isVisible;
				}
				if (ugRules.DisplayLayout.Bands.Exists(C_SET_ALL_CLR))
				{
					ugRules.DisplayLayout.Bands[C_SET_ALL_CLR].Columns["SIZE_QUANTITY"].Hidden = isVisible;
				}
				if (ugRules.DisplayLayout.Bands.Exists(C_SET_CLR))
				{
					ugRules.DisplayLayout.Bands[C_SET_CLR].Columns["SIZE_QUANTITY"].Hidden = isVisible;
				}
				if (ugRules.DisplayLayout.Bands.Exists(C_CLR_SZ))
				{
					ugRules.DisplayLayout.Bands[C_CLR_SZ].Columns["SIZE_QUANTITY"].Hidden = isVisible;
				}
				if (ugRules.DisplayLayout.Bands.Exists(C_ALL_CLR_SZ))
				{
					ugRules.DisplayLayout.Bands[C_ALL_CLR_SZ].Columns["SIZE_QUANTITY"].Hidden = isVisible;
				}
				if (ugRules.DisplayLayout.Bands.Exists(C_ALL_CLR_SZ_DIM))
				{
					ugRules.DisplayLayout.Bands[C_ALL_CLR_SZ_DIM].Columns["SIZE_QUANTITY"].Hidden = isVisible;
				}
				if (ugRules.DisplayLayout.Bands.Exists(C_CLR_SZ_DIM))
				{
					ugRules.DisplayLayout.Bands[C_CLR_SZ_DIM].Columns["SIZE_QUANTITY"].Hidden = isVisible;
				}
			}
			catch (Exception ex)
			{
				HandleException(ex, "HideQuantityColumn");
			}
		}

		/// <summary>
		/// Main validation routine for All Sizes Grid.  Each row will be sent to 
		/// have row validation done on it (IsActiveRowValid)
		/// </summary>
		/// <returns>Boolean value True=Valid|False=InValid</returns>
		/// <remarks>Method must be overridden</remarks>
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

		override protected void InitializeValueLists(UltraGrid myGrid)
		{
			//===============================================================================
			// a note about Sizes vs SizeCell and Dimensions vs DimensionCell.
			// the Sizes/Dimensions lists are only used at start up to get the cells
			// to display the correct value.  After that the "cell" versions are modified
			// and used as the valuelists.
			//===============================================================================
			myGrid.DisplayLayout.ValueLists.Add("Colors");
			myGrid.DisplayLayout.ValueLists.Add("Rules");
			myGrid.DisplayLayout.ValueLists.Add("Sizes");
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

			// begin MID Track 3619 Remove Fringe
			//if (_sizeNeedMethod.SizeFringeRid == Include.NoRID)
			//	base.FillRulesList(myGrid.DisplayLayout.ValueLists["Rules"], false);
			//else
			//	base.FillRulesList(myGrid.DisplayLayout.ValueLists["Rules"], true);
            base.FillRulesList(myGrid.DisplayLayout.ValueLists["Rules"], true);
			// end MID Track 3619 Remove Fringe

//			FillSortList(myGrid.DisplayLayout.ValueLists["SortOrder"],
//				eMIDTextType.eFillSizeHolesSort,
//				eMIDTextOrderBy.TextValue);

			//Fill the size lists if there is a size curve selected.
			if (_sizeNeedMethod.SizeCurveGroupRid != Include.NoRID)
			{
				FillSizesList(myGrid.DisplayLayout.ValueLists["Sizes"],
					_sizeNeedMethod.SizeCurveGroupRid, _sizeNeedMethod.GetSizesUsing);

				FillSizeDimensionList(myGrid.DisplayLayout.ValueLists["Dimensions"],
					_sizeNeedMethod.SizeCurveGroupRid, _sizeNeedMethod.GetDimensionsUsing);
			}

			// begin MID Track 3781 Size curve not required
			//Fill the size lists if there is a size group selected.
			if (_sizeNeedMethod.SizeGroupRid != Include.NoRID)
			{
				FillSizesList(myGrid.DisplayLayout.ValueLists["Sizes"],
					_sizeNeedMethod.SizeGroupRid, _sizeNeedMethod.GetSizesUsing);

				FillSizeDimensionList(myGrid.DisplayLayout.ValueLists["Dimensions"],
					_sizeNeedMethod.SizeGroupRid, _sizeNeedMethod.GetDimensionsUsing);
			}
			// end MID Track 3781 Size curve not required
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
//				//COPY TO ALL COLOR ROW
//				foreach(UltraGridRow allColorRow in activeRow.ChildBands[AllColorBand].Rows)
//				{
//					allColorRow.Cells["SIZE_MIN"].Value = string.Empty;
//					allColorRow.Cells["SIZE_MAX"].Value = string.Empty;
//					allColorRow.Cells["SIZE_MULT"].Value = string.Empty;
//					allColorRow.Cells["SIZE_QUANTITY"].Value = string.Empty;
//					allColorRow.Cells["SIZE_RULE"].Value = string.Empty;
//
//
//					if (AllColorDimBand != null)
//					{
//						foreach (UltraGridRow allColorDimRow in allColorRow.ChildBands[AllColorDimBand].Rows)
//						{
//							allColorDimRow.Cells["SIZE_MIN"].Value = string.Empty;
//							allColorDimRow.Cells["SIZE_MAX"].Value = string.Empty;
//							allColorDimRow.Cells["SIZE_MULT"].Value = string.Empty;
//							allColorDimRow.Cells["SIZE_QUANTITY"].Value = string.Empty;
//							allColorDimRow.Cells["SIZE_RULE"].Value = string.Empty;
//
//							if (AllColorSizeBand != null)
//							{
//								foreach (UltraGridRow allColorSizeRow in allColorDimRow.ChildBands[AllColorSizeBand].Rows)
//								{
//									allColorSizeRow.Cells["SIZE_MIN"].Value = string.Empty;
//									allColorSizeRow.Cells["SIZE_MAX"].Value = string.Empty;
//									allColorSizeRow.Cells["SIZE_MULT"].Value = string.Empty;
//									allColorSizeRow.Cells["SIZE_QUANTITY"].Value = string.Empty;
//									allColorSizeRow.Cells["SIZE_RULE"].Value = string.Empty;
//								}
//							}
//						}
//					}
//				}
//
//				//COPY TO COLOR
//				foreach(UltraGridRow colorRow in activeRow.ChildBands[ColorBand].Rows)
//				{
//					colorRow.Cells["SIZE_MIN"].Value = string.Empty;
//					colorRow.Cells["SIZE_MAX"].Value = string.Empty;
//					colorRow.Cells["SIZE_MULT"].Value = string.Empty;
//					colorRow.Cells["SIZE_QUANTITY"].Value = string.Empty;
//					colorRow.Cells["SIZE_RULE"].Value = string.Empty;
//
//
//					if (ColorDimBand != null)
//					{
//						foreach (UltraGridRow colorDimRow in colorRow.ChildBands[ColorDimBand].Rows)
//						{
//							colorDimRow.Cells["SIZE_MIN"].Value = string.Empty;
//							colorDimRow.Cells["SIZE_MAX"].Value = string.Empty;
//							colorDimRow.Cells["SIZE_MULT"].Value = string.Empty;
//							colorDimRow.Cells["SIZE_QUANTITY"].Value = string.Empty;
//							colorDimRow.Cells["SIZE_RULE"].Value = string.Empty;
//
//							if (ColorSizeBand != null)
//							{
//								foreach (UltraGridRow colorSizeRow in colorDimRow.ChildBands[ColorSizeBand].Rows)
//								{
//									colorSizeRow.Cells["SIZE_MIN"].Value = string.Empty;
//									colorSizeRow.Cells["SIZE_MAX"].Value = string.Empty;
//									colorSizeRow.Cells["SIZE_MULT"].Value = string.Empty;
//									colorSizeRow.Cells["SIZE_QUANTITY"].Value = string.Empty;
//									colorSizeRow.Cells["SIZE_RULE"].Value = string.Empty;
//								}
//							}
//						}
//					}
//				}
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
//				foreach(UltraGridRow setRow in ugAllSize.Rows)
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
//						//ALL COLORS PATH
//						//========================================================================
//						foreach(UltraGridRow allColorRow in setRow.ChildBands[AllColorBand].Rows)
//						{
//							allColorRow.Cells["SIZE_MIN"].Value = activeRow.Cells["SIZE_MIN"].Value;
//							allColorRow.Cells["SIZE_MAX"].Value = activeRow.Cells["SIZE_MAX"].Value;
//							allColorRow.Cells["SIZE_MULT"].Value = activeRow.Cells["SIZE_MULT"].Value;
//							allColorRow.Cells["SIZE_QUANTITY"].Value = activeRow.Cells["SIZE_QUANTITY"].Value;
//							allColorRow.Cells["SIZE_RULE"].Value = activeRow.Cells["SIZE_RULE"].Value;
//
//
//
//							if (AllColorDimBand != null)
//							{
//								foreach (UltraGridRow allColorDimRow in allColorRow.ChildBands[AllColorDimBand].Rows)
//								{
//									allColorDimRow.Cells["SIZE_MIN"].Value = activeRow.Cells["SIZE_MIN"].Value;
//									allColorDimRow.Cells["SIZE_MAX"].Value = activeRow.Cells["SIZE_MAX"].Value;
//									allColorDimRow.Cells["SIZE_MULT"].Value = activeRow.Cells["SIZE_MULT"].Value;
//									allColorDimRow.Cells["SIZE_QUANTITY"].Value = activeRow.Cells["SIZE_QUANTITY"].Value;
//									allColorDimRow.Cells["SIZE_RULE"].Value = activeRow.Cells["SIZE_RULE"].Value;
//
//									if (AllColorSizeBand != null)
//									{
//										foreach (UltraGridRow allColorSizeRow in allColorDimRow.ChildBands[AllColorSizeBand].Rows)
//										{
//											allColorSizeRow.Cells["SIZE_MIN"].Value = activeRow.Cells["SIZE_MIN"].Value;
//											allColorSizeRow.Cells["SIZE_MAX"].Value = activeRow.Cells["SIZE_MAX"].Value;
//											allColorSizeRow.Cells["SIZE_MULT"].Value = activeRow.Cells["SIZE_MULT"].Value;
//											allColorSizeRow.Cells["SIZE_QUANTITY"].Value = activeRow.Cells["SIZE_QUANTITY"].Value;
//											allColorSizeRow.Cells["SIZE_RULE"].Value = activeRow.Cells["SIZE_RULE"].Value;
//										}
//									}
//								}
//							}
//						}
//
//
//
//						foreach(UltraGridRow colorRow in setRow.ChildBands[ColorBand].Rows)
//						{
//							colorRow.Cells["SIZE_MIN"].Value = activeRow.Cells["SIZE_MIN"].Value;
//							colorRow.Cells["SIZE_MAX"].Value = activeRow.Cells["SIZE_MAX"].Value;
//							colorRow.Cells["SIZE_MULT"].Value = activeRow.Cells["SIZE_MULT"].Value;
//							colorRow.Cells["SIZE_QUANTITY"].Value = activeRow.Cells["SIZE_QUANTITY"].Value;
//							colorRow.Cells["SIZE_RULE"].Value = activeRow.Cells["SIZE_RULE"].Value;
//
//							if (ColorDimBand != null)
//							{
//								foreach (UltraGridRow colorDimRow in colorRow.ChildBands[ColorDimBand].Rows)
//								{
//									colorDimRow.Cells["SIZE_MIN"].Value = activeRow.Cells["SIZE_MIN"].Value;
//									colorDimRow.Cells["SIZE_MAX"].Value = activeRow.Cells["SIZE_MAX"].Value;
//									colorDimRow.Cells["SIZE_MULT"].Value = activeRow.Cells["SIZE_MULT"].Value;
//									colorDimRow.Cells["SIZE_QUANTITY"].Value = activeRow.Cells["SIZE_QUANTITY"].Value;
//									colorDimRow.Cells["SIZE_RULE"].Value = activeRow.Cells["SIZE_RULE"].Value;
//
//									if (ColorSizeBand != null)
//									{
//										foreach (UltraGridRow colorSizeRow in colorDimRow.ChildBands[ColorSizeBand].Rows)
//										{
//											colorSizeRow.Cells["SIZE_MIN"].Value = activeRow.Cells["SIZE_MIN"].Value;
//											colorSizeRow.Cells["SIZE_MAX"].Value = activeRow.Cells["SIZE_MAX"].Value;
//											colorSizeRow.Cells["SIZE_MULT"].Value = activeRow.Cells["SIZE_MULT"].Value;
//											colorSizeRow.Cells["SIZE_QUANTITY"].Value = activeRow.Cells["SIZE_QUANTITY"].Value;
//											colorSizeRow.Cells["SIZE_RULE"].Value = activeRow.Cells["SIZE_RULE"].Value;
//										}
//									}
//								}
//							}
//						}
//					}
//				}
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
//				foreach(UltraGridRow setRow in ugAllSize.Rows)
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
//					//ALL COLORS PATH
//					//========================================================================
//					foreach(UltraGridRow allColorRow in setRow.ChildBands[AllColorBand].Rows)
//					{
//						allColorRow.Cells["SIZE_MIN"].Value = string.Empty;
//						allColorRow.Cells["SIZE_MAX"].Value = string.Empty;
//						allColorRow.Cells["SIZE_MULT"].Value = string.Empty;
//						allColorRow.Cells["SIZE_QUANTITY"].Value = string.Empty;
//						allColorRow.Cells["SIZE_RULE"].Value = string.Empty;
//
//						if (AllColorDimBand != null)
//						{
//							foreach (UltraGridRow allColorDimRow in allColorRow.ChildBands[AllColorDimBand].Rows)
//							{
//								allColorDimRow.Cells["SIZE_MIN"].Value = string.Empty;
//								allColorDimRow.Cells["SIZE_MAX"].Value = string.Empty;
//								allColorDimRow.Cells["SIZE_MULT"].Value = string.Empty;
//								allColorDimRow.Cells["SIZE_QUANTITY"].Value = string.Empty;
//								allColorDimRow.Cells["SIZE_RULE"].Value = string.Empty;
//
//								if (AllColorDimBand != null)
//								{
//									foreach (UltraGridRow allColorSizeRow in allColorDimRow.ChildBands[AllColorSizeBand].Rows)
//									{
//										allColorSizeRow.Cells["SIZE_MIN"].Value = string.Empty;
//										allColorSizeRow.Cells["SIZE_MAX"].Value = string.Empty;
//										allColorSizeRow.Cells["SIZE_MULT"].Value = string.Empty;
//										allColorSizeRow.Cells["SIZE_QUANTITY"].Value = string.Empty;
//										allColorSizeRow.Cells["SIZE_RULE"].Value = string.Empty;
//									}
//								}
//							}
//						}
//					}
//
//
//
//					foreach(UltraGridRow colorRow in setRow.ChildBands[ColorBand].Rows)
//					{
//						colorRow.Cells["SIZE_MIN"].Value = string.Empty;
//						colorRow.Cells["SIZE_MAX"].Value = string.Empty;
//						colorRow.Cells["SIZE_MULT"].Value = string.Empty;
//						colorRow.Cells["SIZE_QUANTITY"].Value = string.Empty;
//						colorRow.Cells["SIZE_RULE"].Value = string.Empty;
//
//						if (ColorDimBand != null)
//						{
//							foreach (UltraGridRow colorDimRow in colorRow.ChildBands[ColorDimBand].Rows)
//							{
//								colorDimRow.Cells["SIZE_MIN"].Value = string.Empty;
//								colorDimRow.Cells["SIZE_MAX"].Value = string.Empty;
//								colorDimRow.Cells["SIZE_MULT"].Value = string.Empty;
//								colorDimRow.Cells["SIZE_QUANTITY"].Value = string.Empty;
//								colorDimRow.Cells["SIZE_RULE"].Value = string.Empty;
//
//								if (ColorSizeBand != null)
//								{
//									foreach (UltraGridRow colorSizeRow in colorDimRow.ChildBands[ColorSizeBand].Rows)
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
//			}
//			catch (Exception ex)
//			{
//				HandleException(ex, "ClearAllSizeData");
//			}
//		}

		/// <summary>
		/// Private method, enables or disables the buttons on the AddNewBox on ugAllSize.
		/// Called from ugAllSize_AfterRowActivate
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

                        //Begin TT#302 - JSmith - Size Need method throwing Null Ref Error - selected generic merch level, selected size group, get error
                        //if (this._sizeNeedMethod.SizeCurveGroupRid != Include.NoRID // MID Track 3781 Size Curve not required
                        //    || this._sizeNeedMethod.SizeGroupRid != Include.NoRID)  // MID Track 3781 Size Curve not required
                        if (this._sizeNeedMethod != null &&
                            (this._sizeNeedMethod.SizeCurveGroupRid != Include.NoRID // MID Track 3781 Size Curve not required
                            || this._sizeNeedMethod.SizeGroupRid != Include.NoRID))  // MID Track 3781 Size Curve not required
                        //End TT#302
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
					
						if (this._sizeNeedMethod.SizeCurveGroupRid != Include.NoRID // MID Track 3781 Size Curve not required
							|| this._sizeNeedMethod.SizeGroupRid != Include.NoRID) // MID Track 3781 Size Curve not required
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
		
		//private void cboFringe_SelectionChangeCommitted(object sender, System.EventArgs e)
		//{
			// begin MID Track 3619 Remove Fringe
//			try
//			{
//				if (base.FormLoaded)
//				{
//					if (_sizeNeedMethod.PromptFringeChange)
//					{	
//						ChangePending = true;
//						int fringeRid = Convert.ToInt32(cboFringe.SelectedValue.ToString(),CultureInfo.CurrentUICulture);
//						if (fringeRid == Include.NoRID && _sizeNeedMethod.SizeFringeRid != fringeRid)
//						{
//							if (ShowFringeWarningPrompt() == DialogResult.Yes)
//							{
//								_sizeNeedMethod.SizeFringeRid = fringeRid;
//								if (fringeRid == Include.NoRID)
//								{
//									_usingFringe = false;
//									HideQuantityColumn(true);								
//								}
//								else
//								{
//									_usingFringe = true; 
//									HideQuantityColumn(false);								
//								}
//								ClearInvalidFringeSelections();
//								ugRules.DisplayLayout.ValueLists["Rules"].ValueListItems.Clear();
//								FillRulesList(ugRules.DisplayLayout.ValueLists["Rules"], _usingFringe);
//							}
//							else
//							{
//								//Shut off the prompt so the combo can be reset to original value.
//								_sizeNeedMethod.PromptFringeChange = false;
//								cboSizeCurve.SelectedValue = _sizeNeedMethod.SizeFringeRid;
//							}
//						}
//						else
//						{
//							_sizeNeedMethod.SizeFringeRid = fringeRid;
//							if (fringeRid == Include.NoRID)
//							{
//								_usingFringe = false;
//								HideQuantityColumn(true);								
//							}
//							else
//							{
//								_usingFringe = true; 
//								HideQuantityColumn(false);								
//							}
//							ugRules.DisplayLayout.ValueLists["Rules"].ValueListItems.Clear();
//							FillRulesList(ugRules.DisplayLayout.ValueLists["Rules"], _usingFringe);
//						}	
//					}
//					else
//					{
//						//Turn the prompt back on.
//						_sizeNeedMethod.PromptFringeChange = true;
//					}
//				}
//			}
//			catch (Exception ex)
//			{
//				HandleException(ex, "cboFringe_SelectionChangeCommitted");
//			}
			// end MID Track Remove Fringe
		//}

		// begin MID Track 3619 Remove Fringe
		//protected DialogResult ShowFringeWarningPrompt()
		//{
		//	try
		//	{
		//		DialogResult drResult;
		//		drResult = DialogResult.Yes;
		//		ChangePending = false;
        //
		//		string msg = "Warning:\nChanging this value may remove Rule selections that become invalid after change. ";
		//		msg += 	"\nDo you wish to continue?";
        //
		//		drResult = MessageBox.Show(msg,	"Confirmation", MessageBoxButtons.YesNo);
        //
		//		if (drResult == DialogResult.Yes)
		//		{
		//			ChangePending = true;
		//		}
        //
		//		return drResult;
		//	}
		//	catch (Exception ex)
		//	{
		//		HandleException(ex, "ShowFringeWarningPrompt");
		//		return DialogResult.No;
		//	}	
		//}
        // end MID Track 3619 Remove Fringe 

		private void ClearInvalidRuleSelections()
		{
			try
			{
				
				foreach(UltraGridRow activeRow in ugRules.Rows)
				{
					if (activeRow.Cells["SIZE_RULE"].Value != DBNull.Value)
						if (Convert.ToInt32(activeRow.Cells["SIZE_RULE"].Value) != (int)eSizeRuleType.Exclude)  // MID Track 3619 Remove Fringe
							activeRow.Cells["SIZE_RULE"].Value = DBNull.Value;

					if (activeRow.HasChild())
					{
						//COPY TO ALL COLOR ROW
						foreach(UltraGridRow allColorRow in activeRow.ChildBands[0].Rows)
						{
							if (allColorRow.Cells["SIZE_RULE"].Value != DBNull.Value)
								if ((int)allColorRow.Cells["SIZE_RULE"].Value != (int)eSizeRuleType.Exclude) // MID Track 3619 Remove Fringe
									allColorRow.Cells["SIZE_RULE"].Value = DBNull.Value;

							if (allColorRow.HasChild())
							{
								foreach (UltraGridRow allColorDimRow in allColorRow.ChildBands[0].Rows)
								{
									if (allColorDimRow.Cells["SIZE_RULE"].Value != DBNull.Value)
										if ((int)allColorDimRow.Cells["SIZE_RULE"].Value != (int)eSizeRuleType.Exclude) // MID Track 3619 Remove Fringe
											allColorDimRow.Cells["SIZE_RULE"].Value = DBNull.Value;

									if (allColorDimRow.HasChild())
									{
										foreach (UltraGridRow allColorSizeRow in allColorDimRow.ChildBands[0].Rows)
										{
											if (allColorSizeRow.Cells["SIZE_RULE"].Value != DBNull.Value)
												if ((int)allColorSizeRow.Cells["SIZE_RULE"].Value != (int)eSizeRuleType.Exclude) // MID Track 3619 Remove Fringe
													allColorSizeRow.Cells["SIZE_RULE"].Value = DBNull.Value;
										}
									}
								}
							}
						}

						//COPY TO COLOR
						foreach(UltraGridRow colorRow in activeRow.ChildBands[1].Rows)
						{
							if (colorRow.Cells["SIZE_RULE"].Value != DBNull.Value)
								if ((int)colorRow.Cells["SIZE_RULE"].Value != (int)eSizeRuleType.Exclude) // MID Track 3619 Remove Fringe
									colorRow.Cells["SIZE_RULE"].Value = DBNull.Value;

							if (colorRow.HasChild())
							{
								foreach (UltraGridRow colorDimRow in colorRow.ChildBands[0].Rows)
								{
									if (colorDimRow.Cells["SIZE_RULE"].Value != DBNull.Value)
										if ((int)colorDimRow.Cells["SIZE_RULE"].Value != (int)eSizeRuleType.Exclude) // MID Track 3619 Remove Fringe
											colorDimRow.Cells["SIZE_RULE"].Value = DBNull.Value;

									if (colorDimRow.HasChild())
									{
										foreach (UltraGridRow colorSizeRow in colorDimRow.ChildBands[0].Rows)
										{
											if (colorSizeRow.Cells["SIZE_RULE"].Value != DBNull.Value)
												if ((int)colorSizeRow.Cells["SIZE_RULE"].Value != (int)eSizeRuleType.Exclude) // MID Track 3619 Remove Fringe
													colorSizeRow.Cells["SIZE_RULE"].Value = DBNull.Value;
										}
									}
								}
							}
						}
					}
					activeRow.Update();
				}
			}
			catch (Exception ex)
			{
				HandleException(ex, "ClearInvalidRuleSelections");  // MID Track 3619 Remove Fringe
			}
		}

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

		/// <summary>
		/// Determines if the rule data should be restored for this method.
		/// </summary>
		override protected void AfterClosing()
		{
			TransactionData td = new TransactionData();

			try
			{
				if (ResultSaveChanges != DialogResult.None)
				{
					if (ResultSaveChanges == DialogResult.No)
					{
						//ONLY ROLLBACK IF UPDATING THE METHOD
						if (_sizeNeedMethod.Method_Change_Type == eChangeType.update)
						{
							//CREATE AND OPEN CONNECTION HERE SO THE POSSIBLE ROLLBACKS
							//ARE ON THE SAME TRANSACTION.  THE INSERT UPDATES WILL
							//OPEN AND COMMIT/ROLLBACK DATA UNLESS THE PROVIDED TRANSACTIONDATA
							//OBJECT IS ALREADY OPEN.
							td = new TransactionData();

							if (ConstraintRollback)
							{
								if (!td.ConnectionIsOpen) 
								{
									td.OpenUpdateConnection();
								}								
								_sizeNeedMethod.MethodConstraints = DataSetBackup;
								_sizeNeedMethod.InsertUpdateMethodRules(td);

								if (td.ConnectionIsOpen) 
								{
									td.CommitData();
									td.CloseUpdateConnection();
								}
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				if (td.ConnectionIsOpen) 
				{
					td.Rollback();
					td.CloseUpdateConnection();
				}
				HandleException(ex, "SizeNeedMethod.AfterClosing");
			}
		}

		private void cbExpandAll_CheckedChanged(object sender, System.EventArgs e)
		{
			CheckExpandAll();
		}

        // Begin TT#301-MD - JSmith - Controls are not functioning properly
        //private void ugRules_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        //{
        //    //Begin TT#169 - JSmith - enter max of 1000 and receive 'must be integer'
        //    //MIDRetail.Windows.Controls.UltraGridLayoutDefaults ugld = new MIDRetail.Windows.Controls.UltraGridLayoutDefaults();
        //    //ugld.ApplyDefaults(e);
        //    MIDRetail.Windows.Controls.UltraGridLayoutDefaults ugld = new MIDRetail.Windows.Controls.UltraGridLayoutDefaults(ErrorImage);
        //    // Begin TT#1164 - JSmith - When going in to some methods or Admin features the columns are appearing too wide.
        //    //ugld.ApplyDefaults((Infragistics.Win.UltraWinGrid.UltraGrid)sender, e, true, 2, true);
        //    ugld.ApplyDefaults((Infragistics.Win.UltraWinGrid.UltraGrid)sender, e, true, 2, false);
        //    // End TT#1164
        //    //End TT#169
        //}
        // End TT#301-MD - JSmith - Controls are not functioning properly

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

		private void tabControl2_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			try
			{
				//gbGenericSizeCurve.Visible =  (tabControl2.SelectedTab == tabPageGeneral) ? true : false;
				//gbSizeCurve.Visible =  (tabControl2.SelectedTab == tabPageGeneral) ? true : false;
				//gbSizeConstraints.Visible =  (tabControl2.SelectedTab == tabPageGeneral) ? true : false;

				if (FormLoaded &&
					ugRules.DataSource == null &&
					!FormIsClosing)
				{
					Cursor = Cursors.WaitCursor;
					BindAllSizeGrid(_sizeNeedMethod.MethodConstraints);
					CheckExpandAll();
				}
			}
			catch (Exception ex)
			{
				HandleException(ex, "SizeNeedMethod.tabControl2_SelectedIndexChanged");
			}
			finally
			{
				Cursor = Cursors.Default;
			}
		}

        #region Properties Tab - Workflows
        /// <summary>
        /// Fill the workflow grid
        /// </summary>
        private void LoadWorkflows()//9-17
        {
            try
            {
                GetWorkflows(_sizeNeedMethod.Key, ugWorkflows);
            }
            catch (Exception ex)
            {
                HandleException(ex, "LoadWorkflows");
            }
        }
        #endregion

        // Begin Track #4872 - JSmith - Global/User Attributes
        private void frmSizeNeedMethod_Load(object sender, EventArgs e)
        {
            if (cboStoreAttribute.ReplaceAttribute)
            {
                ChangePending = true;
            }
        }

        private void gbMaxPackAllocNeedTolerance_Enter(object sender, EventArgs e)
        {

        }



        private void gbAvgPackDevTolerance_Enter(object sender, EventArgs e)
        {

        }
        // End Track #4872
        // begin TT#1365 - JEllis - FL Detail Pack Size Need Enhancement
        private void cbxNoMaxStep_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void cbxStepped_CheckedChanged(object sender, EventArgs e)
        {

        }
        // end TT#1365 - JEllis - FL Detail Pack Size Need Enhancement

        private void cboHierarchyLevel_MIDComboBoxPropertiesChangedEvent(object source, MIDComboBoxPropertiesChangedEventArgs args)
        {
            this.cboHierarchyLevel_SelectionChangeCommitted(source, new EventArgs());
        }
        private void cboMerchandise_MIDComboBoxPropertiesChangedEvent(object source, MIDComboBoxPropertiesChangedEventArgs args)
        {
            this.cboMerchandise_SelectionChangeCommitted(source, new EventArgs());
        }

	}
}
