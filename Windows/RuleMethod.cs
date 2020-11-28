using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Diagnostics;
using System.Globalization;
using System.Data;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using MIDRetail.Business;
using MIDRetail.Data;
using MIDRetail.DataCommon;
using MIDRetail.Common;
using MIDRetail.Business.Allocation;
using MIDRetail.Windows.Controls;

namespace MIDRetail.Windows
{
	/// <summary>
	/// Summary description for RuleMethod.
	/// </summary>
	public class frmRuleMethod : WorkflowMethodFormBase
	{
		private RuleMethod _ruleMethod;
		private DataTable _IncludeRuleDataTable = null;
		private DataTable _ExcludeRuleDataTable = null;
		private DataTable _ComponentDataTable = null;
		private DataTable _PackDataTable = null;
		private DataTable _ColorDataTable = null;

		private int _nodeRID = Include.NoRID;
		private int _headerRID = Include.NoRID;
		private System.Windows.Forms.TabPage tabMethod;
		private System.Windows.Forms.TabPage tabProperties;
		private System.Windows.Forms.TabControl tabRuleMethod;
        private System.Windows.Forms.Label lblFilter;
		private System.Windows.Forms.GroupBox gbxExcluded;
		private System.Windows.Forms.GroupBox gbxIncluded;
		private System.Windows.Forms.GroupBox gbxBasis;
		private System.Windows.Forms.Label lblHeader;
		private System.Windows.Forms.TextBox txtExclQuantity;
		private System.Windows.Forms.Label lblExclQuantity;
		private System.Windows.Forms.Label lblInclQuantity;
		private System.Windows.Forms.TextBox txtInclQuantity;
		private System.Windows.Forms.RadioButton radAscend;
		private System.Windows.Forms.RadioButton radDescend;
		private System.Windows.Forms.TextBox txtBasisHeader;

		private System.Windows.Forms.Panel pnlStoreOrder;
		private Infragistics.Win.UltraWinGrid.UltraGrid ugWorkflows;
		private System.Windows.Forms.Label lblPack;
		private System.Windows.Forms.Label lblColor;
        private System.Windows.Forms.Label lblBasisComponent;
        private System.Windows.Forms.Label lblSet;
        // Begin Track #4872 - JSmith - Global/User Attributes
        //private MIDRetail.Windows.Controls.MIDComboBoxEnh cboStoreAttribute;
        private MIDAttributeComboBox cboStoreAttribute;
        // End Track #4872
		private System.Windows.Forms.Button btnGetHeader;

		//private StoreFilterData _storeFilterData;
        private FilterData _storeFilterData;
		private DataTable _dtStoreFilter;
		private System.Windows.Forms.CheckBox cbxIsHeaderMaster;
        private MIDComboBoxEnh cboRuleFilter;
        private MIDComboBoxEnh cboAttributeSet;
        private MIDComboBoxEnh cboExclRule;
        private MIDComboBoxEnh cboInclRule;
        private MIDComboBoxEnh cboBasisColor;
        private MIDComboBoxEnh cboBasisPack;
        private MIDComboBoxEnh cboBasisComponent;
        private CheckBox cbxIncludeReserve;

		// /sjd

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

 
		/// <summary>
		/// Creates an instance of frmRuleMethod
		/// </summary>
		/// <param name="SAB">SessionAddressBlock</param>
		public frmRuleMethod(SessionAddressBlock SAB, ExplorerAddressBlock aEAB) : base (SAB, aEAB, eMIDTextCode.frm_RuleMethod, eWorkflowMethodType.Method)
		{
			try
			{
				//
				// Required for Windows Form Designer support
				//
				InitializeComponent();

				// Begin MID Track 4858 - JSmith - Security changes
				// begin MID Track 3623 Port Master Allocation
                // BEGIN TT#2133-MD - AGallagher - Master option is no longer valid and should not be visable or selectable 
                //if (!SAB.ClientServerSession.GlobalOptions.AppConfig.MasterAllocationInstalled)
                //{
                // END TT#2133-MD - AGallagher - Master option is no longer valid and should not be visable or selectable 
					this.cbxIsHeaderMaster.Hide();
				//}  // TT#2133-MD - AGallagher - Master option is no longer valid and should not be visable or selectable 
                    
				// end MID Track 3623 Port Master Allocation
				// End MID Track 4858

				UserSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AllocationMethodsUserRule);
				GlobalSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AllocationMethodsGlobalRule);
				
			}
			catch(Exception ex)
			{
				string exceptionMessage = ex.Message;
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
				this.cboStoreAttribute.SelectionChangeCommitted -= new System.EventHandler(this.cboStoreAttribute_SelectionChangeCommitted);
                this.cboStoreAttribute.DragDrop -= new System.Windows.Forms.DragEventHandler(this.cboStoreAttribute_DragDrop);
                this.cboStoreAttribute.DragEnter -= new System.Windows.Forms.DragEventHandler(this.cboStoreAttribute_DragEnter);
				this.cboAttributeSet.SelectionChangeCommitted -= new System.EventHandler(this.cboAttributeSet_SelectionChangeCommitted);
				this.cboExclRule.SelectionChangeCommitted -= new System.EventHandler(this.cboExclRule_SelectionChangeCommitted);
				this.cboRuleFilter.SelectionChangeCommitted -= new System.EventHandler(this.cboRuleFilter_SelectionChangeCommitted);
                this.cboRuleFilter.DragEnter -= new System.Windows.Forms.DragEventHandler(this.cboRuleFilter_DragEnter);
                this.cboRuleFilter.DragDrop -= new System.Windows.Forms.DragEventHandler(this.cboRuleFilter_DragDrop);
				this.cboInclRule.SelectionChangeCommitted -= new System.EventHandler(this.cboInclRule_SelectionChangeCommitted);
                //this.cboBasisComponent.SelectionChangeCommitted -= new System.EventHandler(this.cboBasisComponent_SelectionChangeCommitted);
				this.txtBasisHeader.Validating -= new System.ComponentModel.CancelEventHandler(this.txtBasisHeader_Validating);
				this.txtBasisHeader.DragDrop -= new System.Windows.Forms.DragEventHandler(this.txtBasisHeader_DragDrop);
				this.txtBasisHeader.TextChanged -= new System.EventHandler(this.txtBasisHeader_TextChanged);
				this.txtBasisHeader.DragEnter -= new System.Windows.Forms.DragEventHandler(this.txtBasisHeader_DragEnter);
				this.txtBasisHeader.Leave -= new System.EventHandler(this.txtBasisHeader_Leave);
				// Begin MID Track 4858 - JSmith - Security changes
//				this.btnSave.Click -= new System.EventHandler(this.btnSave_Click);
//				this.btnClose.Click -= new System.EventHandler(this.btnClose_Click);
//				this.btnProcess.Click -= new System.EventHandler(this.btnProcess_Click);
				// End MID Track 4858
				this.btnGetHeader.Click -= new System.EventHandler(this.btnGetHeader_Click);
// (CSMITH) - BEG MID Track #3219: PO / Master Release Enhancement
				this.cbxIsHeaderMaster.CheckedChanged -= new System.EventHandler(this.cbxIsHeaderMaster_CheckedChanged);
// (CSMITH) - END MID Track #3219
               
                // Begin TT#316-MD - RMatelic - Replace all Windows Combobox controls with new enhanced control 
                this.cboStoreAttribute.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler(cboStoreAttribute_MIDComboBoxPropertiesChangedEvent);
                this.cboAttributeSet.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler(cboAttributeSet_MIDComboBoxPropertiesChangedEvent);
                this.cboRuleFilter.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler(cboRuleFilter_MIDComboBoxPropertiesChangedEvent);
                this.cboExclRule.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler(cboExclRule_MIDComboBoxPropertiesChangedEvent);
                this.cboInclRule.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler(cboInclRule_MIDComboBoxPropertiesChangedEvent);
                // End TT#316-MD

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
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            this.tabRuleMethod = new System.Windows.Forms.TabControl();
            this.tabMethod = new System.Windows.Forms.TabPage();
            this.cboRuleFilter = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this.cboAttributeSet = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this.cboStoreAttribute = new MIDRetail.Windows.Controls.MIDAttributeComboBox();
            this.lblSet = new System.Windows.Forms.Label();
            this.gbxExcluded = new System.Windows.Forms.GroupBox();
            this.cboExclRule = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this.txtExclQuantity = new System.Windows.Forms.TextBox();
            this.lblExclQuantity = new System.Windows.Forms.Label();
            this.lblFilter = new System.Windows.Forms.Label();
            this.gbxIncluded = new System.Windows.Forms.GroupBox();
            this.cboInclRule = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this.lblInclQuantity = new System.Windows.Forms.Label();
            this.txtInclQuantity = new System.Windows.Forms.TextBox();
            this.gbxBasis = new System.Windows.Forms.GroupBox();
            this.cboBasisColor = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this.cboBasisPack = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this.cboBasisComponent = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this.cbxIsHeaderMaster = new System.Windows.Forms.CheckBox();
            this.lblBasisComponent = new System.Windows.Forms.Label();
            this.lblColor = new System.Windows.Forms.Label();
            this.lblPack = new System.Windows.Forms.Label();
            this.pnlStoreOrder = new System.Windows.Forms.Panel();
            this.radDescend = new System.Windows.Forms.RadioButton();
            this.radAscend = new System.Windows.Forms.RadioButton();
            this.txtBasisHeader = new System.Windows.Forms.TextBox();
            this.lblHeader = new System.Windows.Forms.Label();
            this.btnGetHeader = new System.Windows.Forms.Button();
            this.tabProperties = new System.Windows.Forms.TabPage();
            this.ugWorkflows = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.cbxIncludeReserve = new System.Windows.Forms.CheckBox();
            this.pnlGlobalUser.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.utmMain)).BeginInit();
            this.tabRuleMethod.SuspendLayout();
            this.tabMethod.SuspendLayout();
            this.gbxExcluded.SuspendLayout();
            this.gbxIncluded.SuspendLayout();
            this.gbxBasis.SuspendLayout();
            this.pnlStoreOrder.SuspendLayout();
            this.tabProperties.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ugWorkflows)).BeginInit();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(624, 480);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(536, 480);
            // 
            // btnProcess
            // 
            this.btnProcess.Location = new System.Drawing.Point(16, 480);
            // 
            // utmMain
            // 
            this.utmMain.MenuSettings.ForceSerialization = true;
            this.utmMain.ToolbarSettings.ForceSerialization = true;
            // 
            // tabRuleMethod
            // 
            this.tabRuleMethod.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabRuleMethod.Controls.Add(this.tabMethod);
            this.tabRuleMethod.Controls.Add(this.tabProperties);
            this.tabRuleMethod.Location = new System.Drawing.Point(16, 80);
            this.tabRuleMethod.Name = "tabRuleMethod";
            this.tabRuleMethod.SelectedIndex = 0;
            this.tabRuleMethod.Size = new System.Drawing.Size(680, 385);
            this.tabRuleMethod.TabIndex = 19;
            // 
            // tabMethod
            // 
            this.tabMethod.Controls.Add(this.cboRuleFilter);
            this.tabMethod.Controls.Add(this.cboAttributeSet);
            this.tabMethod.Controls.Add(this.cboStoreAttribute);
            this.tabMethod.Controls.Add(this.lblSet);
            this.tabMethod.Controls.Add(this.gbxExcluded);
            this.tabMethod.Controls.Add(this.lblFilter);
            this.tabMethod.Controls.Add(this.gbxIncluded);
            this.tabMethod.Controls.Add(this.gbxBasis);
            this.tabMethod.Location = new System.Drawing.Point(4, 22);
            this.tabMethod.Name = "tabMethod";
            this.tabMethod.Size = new System.Drawing.Size(672, 359);
            this.tabMethod.TabIndex = 1;
            this.tabMethod.Text = "Method";
            // 
            // cboRuleFilter
            // 
            this.cboRuleFilter.AllowDrop = true;
            this.cboRuleFilter.AutoAdjust = true;
            this.cboRuleFilter.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboRuleFilter.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboRuleFilter.DataSource = null;
            this.cboRuleFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboRuleFilter.DropDownWidth = 176;
            this.cboRuleFilter.FormattingEnabled = false;
            this.cboRuleFilter.IgnoreFocusLost = false;
            this.cboRuleFilter.ItemHeight = 13;
            this.cboRuleFilter.Location = new System.Drawing.Point(100, 24);
            this.cboRuleFilter.Margin = new System.Windows.Forms.Padding(0);
            this.cboRuleFilter.MaxDropDownItems = 25;
            this.cboRuleFilter.Name = "cboRuleFilter";
            this.cboRuleFilter.SetToolTip = "";
            this.cboRuleFilter.Size = new System.Drawing.Size(176, 21);
            this.cboRuleFilter.TabIndex = 4;
            this.cboRuleFilter.Tag = null;
            this.cboRuleFilter.MIDComboBoxPropertiesChangedEvent += new MIDRetail.Windows.Controls.MIDComboBoxPropertiesChangedEventHandler(this.cboRuleFilter_MIDComboBoxPropertiesChangedEvent);
            this.cboRuleFilter.SelectionChangeCommitted += new System.EventHandler(this.cboRuleFilter_SelectionChangeCommitted);
            this.cboRuleFilter.DragDrop += new System.Windows.Forms.DragEventHandler(this.cboRuleFilter_DragDrop);
            this.cboRuleFilter.DragEnter += new System.Windows.Forms.DragEventHandler(this.cboRuleFilter_DragEnter);
            this.cboRuleFilter.DragOver += new System.Windows.Forms.DragEventHandler(this.cboRuleFilter_DragOver);
            // 
            // cboAttributeSet
            // 
            this.cboAttributeSet.AutoAdjust = true;
            this.cboAttributeSet.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboAttributeSet.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboAttributeSet.DataSource = null;
            this.cboAttributeSet.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboAttributeSet.DropDownWidth = 176;
            this.cboAttributeSet.FormattingEnabled = false;
            this.cboAttributeSet.IgnoreFocusLost = false;
            this.cboAttributeSet.ItemHeight = 13;
            this.cboAttributeSet.Location = new System.Drawing.Point(344, 35);
            this.cboAttributeSet.Margin = new System.Windows.Forms.Padding(0);
            this.cboAttributeSet.MaxDropDownItems = 25;
            this.cboAttributeSet.Name = "cboAttributeSet";
            this.cboAttributeSet.SetToolTip = "";
            this.cboAttributeSet.Size = new System.Drawing.Size(176, 21);
            this.cboAttributeSet.TabIndex = 7;
            this.cboAttributeSet.Tag = null;
            this.cboAttributeSet.MIDComboBoxPropertiesChangedEvent += new MIDRetail.Windows.Controls.MIDComboBoxPropertiesChangedEventHandler(this.cboAttributeSet_MIDComboBoxPropertiesChangedEvent);
            this.cboAttributeSet.SelectionChangeCommitted += new System.EventHandler(this.cboAttributeSet_SelectionChangeCommitted);
            // 
            // cboStoreAttribute
            // 
            this.cboStoreAttribute.AllowDrop = true;
            this.cboStoreAttribute.AllowUserAttributes = false;
            this.cboStoreAttribute.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboStoreAttribute.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboStoreAttribute.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboStoreAttribute.Location = new System.Drawing.Point(344, 8);
            this.cboStoreAttribute.Name = "cboStoreAttribute";
            this.cboStoreAttribute.Size = new System.Drawing.Size(176, 21);
            this.cboStoreAttribute.TabIndex = 9;
            this.cboStoreAttribute.MIDComboBoxPropertiesChangedEvent += new MIDRetail.Windows.Controls.MIDComboBoxPropertiesChangedEventHandler(this.cboStoreAttribute_MIDComboBoxPropertiesChangedEvent);
            this.cboStoreAttribute.SelectionChangeCommitted += new System.EventHandler(this.cboStoreAttribute_SelectionChangeCommitted);
            this.cboStoreAttribute.DragDrop += new System.Windows.Forms.DragEventHandler(this.cboStoreAttribute_DragDrop);
            this.cboStoreAttribute.DragEnter += new System.Windows.Forms.DragEventHandler(this.cboStoreAttribute_DragEnter);
            this.cboStoreAttribute.DragOver += new System.Windows.Forms.DragEventHandler(this.cboStoreAttribute_DragOver);
            // 
            // lblSet
            // 
            this.lblSet.Location = new System.Drawing.Point(304, 29);
            this.lblSet.Name = "lblSet";
            this.lblSet.Size = new System.Drawing.Size(24, 16);
            this.lblSet.TabIndex = 8;
            this.lblSet.Text = "Set:";
            // 
            // gbxExcluded
            // 
            this.gbxExcluded.Controls.Add(this.cboExclRule);
            this.gbxExcluded.Controls.Add(this.txtExclQuantity);
            this.gbxExcluded.Controls.Add(this.lblExclQuantity);
            this.gbxExcluded.Location = new System.Drawing.Point(304, 232);
            this.gbxExcluded.Name = "gbxExcluded";
            this.gbxExcluded.Size = new System.Drawing.Size(232, 112);
            this.gbxExcluded.TabIndex = 6;
            this.gbxExcluded.TabStop = false;
            this.gbxExcluded.Text = "Excluded Stores";
            // 
            // cboExclRule
            // 
            this.cboExclRule.AutoAdjust = true;
            this.cboExclRule.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboExclRule.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboExclRule.DataSource = null;
            this.cboExclRule.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboExclRule.DropDownWidth = 200;
            this.cboExclRule.FormattingEnabled = false;
            this.cboExclRule.IgnoreFocusLost = false;
            this.cboExclRule.ItemHeight = 13;
            this.cboExclRule.Location = new System.Drawing.Point(16, 32);
            this.cboExclRule.Margin = new System.Windows.Forms.Padding(0);
            this.cboExclRule.MaxDropDownItems = 25;
            this.cboExclRule.Name = "cboExclRule";
            this.cboExclRule.SetToolTip = "";
            this.cboExclRule.Size = new System.Drawing.Size(200, 21);
            this.cboExclRule.TabIndex = 0;
            this.cboExclRule.Tag = null;
            this.cboExclRule.MIDComboBoxPropertiesChangedEvent += new MIDRetail.Windows.Controls.MIDComboBoxPropertiesChangedEventHandler(this.cboExclRule_MIDComboBoxPropertiesChangedEvent);
            this.cboExclRule.SelectionChangeCommitted += new System.EventHandler(this.cboExclRule_SelectionChangeCommitted);
            // 
            // txtExclQuantity
            // 
            this.txtExclQuantity.Location = new System.Drawing.Point(72, 72);
            this.txtExclQuantity.Name = "txtExclQuantity";
            this.txtExclQuantity.Size = new System.Drawing.Size(56, 20);
            this.txtExclQuantity.TabIndex = 2;
            this.txtExclQuantity.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // lblExclQuantity
            // 
            this.lblExclQuantity.Location = new System.Drawing.Point(16, 72);
            this.lblExclQuantity.Name = "lblExclQuantity";
            this.lblExclQuantity.Size = new System.Drawing.Size(56, 16);
            this.lblExclQuantity.TabIndex = 1;
            this.lblExclQuantity.Text = "Quantity";
            // 
            // lblFilter
            // 
            this.lblFilter.Location = new System.Drawing.Point(40, 29);
            this.lblFilter.Name = "lblFilter";
            this.lblFilter.Size = new System.Drawing.Size(40, 16);
            this.lblFilter.TabIndex = 5;
            this.lblFilter.Text = "Filter:";
            // 
            // gbxIncluded
            // 
            this.gbxIncluded.Controls.Add(this.cboInclRule);
            this.gbxIncluded.Controls.Add(this.lblInclQuantity);
            this.gbxIncluded.Controls.Add(this.txtInclQuantity);
            this.gbxIncluded.Location = new System.Drawing.Point(24, 232);
            this.gbxIncluded.Name = "gbxIncluded";
            this.gbxIncluded.Size = new System.Drawing.Size(248, 112);
            this.gbxIncluded.TabIndex = 3;
            this.gbxIncluded.TabStop = false;
            this.gbxIncluded.Text = "Included Stores";
            // 
            // cboInclRule
            // 
            this.cboInclRule.AutoAdjust = true;
            this.cboInclRule.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboInclRule.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboInclRule.DataSource = null;
            this.cboInclRule.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboInclRule.DropDownWidth = 200;
            this.cboInclRule.FormattingEnabled = false;
            this.cboInclRule.IgnoreFocusLost = false;
            this.cboInclRule.ItemHeight = 13;
            this.cboInclRule.Location = new System.Drawing.Point(16, 32);
            this.cboInclRule.Margin = new System.Windows.Forms.Padding(0);
            this.cboInclRule.MaxDropDownItems = 25;
            this.cboInclRule.Name = "cboInclRule";
            this.cboInclRule.SetToolTip = "";
            this.cboInclRule.Size = new System.Drawing.Size(200, 21);
            this.cboInclRule.TabIndex = 15;
            this.cboInclRule.Tag = null;
            this.cboInclRule.MIDComboBoxPropertiesChangedEvent += new MIDRetail.Windows.Controls.MIDComboBoxPropertiesChangedEventHandler(this.cboInclRule_MIDComboBoxPropertiesChangedEvent);
            this.cboInclRule.SelectionChangeCommitted += new System.EventHandler(this.cboInclRule_SelectionChangeCommitted);
            // 
            // lblInclQuantity
            // 
            this.lblInclQuantity.Location = new System.Drawing.Point(16, 72);
            this.lblInclQuantity.Name = "lblInclQuantity";
            this.lblInclQuantity.Size = new System.Drawing.Size(56, 16);
            this.lblInclQuantity.TabIndex = 17;
            this.lblInclQuantity.Text = "Quantity";
            // 
            // txtInclQuantity
            // 
            this.txtInclQuantity.Location = new System.Drawing.Point(72, 72);
            this.txtInclQuantity.Name = "txtInclQuantity";
            this.txtInclQuantity.Size = new System.Drawing.Size(56, 20);
            this.txtInclQuantity.TabIndex = 16;
            this.txtInclQuantity.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // gbxBasis
            // 
            this.gbxBasis.Controls.Add(this.cbxIncludeReserve);
            this.gbxBasis.Controls.Add(this.cboBasisColor);
            this.gbxBasis.Controls.Add(this.cboBasisPack);
            this.gbxBasis.Controls.Add(this.cboBasisComponent);
            this.gbxBasis.Controls.Add(this.cbxIsHeaderMaster);
            this.gbxBasis.Controls.Add(this.lblBasisComponent);
            this.gbxBasis.Controls.Add(this.lblColor);
            this.gbxBasis.Controls.Add(this.lblPack);
            this.gbxBasis.Controls.Add(this.pnlStoreOrder);
            this.gbxBasis.Controls.Add(this.txtBasisHeader);
            this.gbxBasis.Controls.Add(this.lblHeader);
            this.gbxBasis.Controls.Add(this.btnGetHeader);
            this.gbxBasis.Location = new System.Drawing.Point(24, 64);
            this.gbxBasis.Name = "gbxBasis";
            this.gbxBasis.Size = new System.Drawing.Size(512, 152);
            this.gbxBasis.TabIndex = 1;
            this.gbxBasis.TabStop = false;
            this.gbxBasis.Text = "Prior";
            // 
            // cboBasisColor
            // 
            this.cboBasisColor.AutoAdjust = true;
            this.cboBasisColor.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboBasisColor.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboBasisColor.DataSource = null;
            this.cboBasisColor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboBasisColor.DropDownWidth = 136;
            this.cboBasisColor.FormattingEnabled = false;
            this.cboBasisColor.IgnoreFocusLost = false;
            this.cboBasisColor.ItemHeight = 13;
            this.cboBasisColor.Location = new System.Drawing.Point(360, 112);
            this.cboBasisColor.Margin = new System.Windows.Forms.Padding(0);
            this.cboBasisColor.MaxDropDownItems = 25;
            this.cboBasisColor.Name = "cboBasisColor";
            this.cboBasisColor.SetToolTip = "";
            this.cboBasisColor.Size = new System.Drawing.Size(136, 21);
            this.cboBasisColor.TabIndex = 22;
            this.cboBasisColor.Tag = null;
            // 
            // cboBasisPack
            // 
            this.cboBasisPack.AutoAdjust = true;
            this.cboBasisPack.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboBasisPack.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboBasisPack.DataSource = null;
            this.cboBasisPack.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboBasisPack.DropDownWidth = 136;
            this.cboBasisPack.FormattingEnabled = false;
            this.cboBasisPack.IgnoreFocusLost = false;
            this.cboBasisPack.ItemHeight = 13;
            this.cboBasisPack.Location = new System.Drawing.Point(360, 72);
            this.cboBasisPack.Margin = new System.Windows.Forms.Padding(0);
            this.cboBasisPack.MaxDropDownItems = 25;
            this.cboBasisPack.Name = "cboBasisPack";
            this.cboBasisPack.SetToolTip = "";
            this.cboBasisPack.Size = new System.Drawing.Size(136, 21);
            this.cboBasisPack.TabIndex = 30;
            this.cboBasisPack.Tag = null;
            // 
            // cboBasisComponent
            // 
            this.cboBasisComponent.AutoAdjust = true;
            this.cboBasisComponent.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboBasisComponent.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboBasisComponent.DataSource = null;
            this.cboBasisComponent.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboBasisComponent.DropDownWidth = 136;
            this.cboBasisComponent.FormattingEnabled = false;
            this.cboBasisComponent.IgnoreFocusLost = false;
            this.cboBasisComponent.ItemHeight = 13;
            this.cboBasisComponent.Location = new System.Drawing.Point(360, 32);
            this.cboBasisComponent.Margin = new System.Windows.Forms.Padding(0);
            this.cboBasisComponent.MaxDropDownItems = 25;
            this.cboBasisComponent.Name = "cboBasisComponent";
            this.cboBasisComponent.SetToolTip = "";
            this.cboBasisComponent.Size = new System.Drawing.Size(136, 21);
            this.cboBasisComponent.TabIndex = 26;
            this.cboBasisComponent.Tag = null;
            // 
            // cbxIsHeaderMaster
            // 
            this.cbxIsHeaderMaster.Location = new System.Drawing.Point(176, 64);
            this.cbxIsHeaderMaster.Name = "cbxIsHeaderMaster";
            this.cbxIsHeaderMaster.Size = new System.Drawing.Size(72, 24);
            this.cbxIsHeaderMaster.TabIndex = 29;
            this.cbxIsHeaderMaster.Text = "Master";
            this.cbxIsHeaderMaster.CheckedChanged += new System.EventHandler(this.cbxIsHeaderMaster_CheckedChanged);
            // 
            // lblBasisComponent
            // 
            this.lblBasisComponent.Location = new System.Drawing.Point(280, 32);
            this.lblBasisComponent.Name = "lblBasisComponent";
            this.lblBasisComponent.Size = new System.Drawing.Size(80, 23);
            this.lblBasisComponent.TabIndex = 27;
            this.lblBasisComponent.Text = "Component";
            this.lblBasisComponent.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblColor
            // 
            this.lblColor.Location = new System.Drawing.Point(288, 112);
            this.lblColor.Name = "lblColor";
            this.lblColor.Size = new System.Drawing.Size(64, 23);
            this.lblColor.TabIndex = 25;
            this.lblColor.Text = "Color";
            this.lblColor.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblPack
            // 
            this.lblPack.Location = new System.Drawing.Point(288, 72);
            this.lblPack.Name = "lblPack";
            this.lblPack.Size = new System.Drawing.Size(64, 23);
            this.lblPack.TabIndex = 24;
            this.lblPack.Text = "Pack";
            this.lblPack.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // pnlStoreOrder
            // 
            this.pnlStoreOrder.Controls.Add(this.radDescend);
            this.pnlStoreOrder.Controls.Add(this.radAscend);
            this.pnlStoreOrder.Location = new System.Drawing.Point(48, 88);
            this.pnlStoreOrder.Name = "pnlStoreOrder";
            this.pnlStoreOrder.Size = new System.Drawing.Size(216, 40);
            this.pnlStoreOrder.TabIndex = 23;
            // 
            // radDescend
            // 
            this.radDescend.Location = new System.Drawing.Point(30, 8);
            this.radDescend.Name = "radDescend";
            this.radDescend.Size = new System.Drawing.Size(88, 24);
            this.radDescend.TabIndex = 7;
            this.radDescend.Text = "Descending";
            // 
            // radAscend
            // 
            this.radAscend.Location = new System.Drawing.Point(124, 8);
            this.radAscend.Name = "radAscend";
            this.radAscend.Size = new System.Drawing.Size(80, 24);
            this.radAscend.TabIndex = 8;
            this.radAscend.Text = "Ascending";
            // 
            // txtBasisHeader
            // 
            this.txtBasisHeader.AllowDrop = true;
            this.txtBasisHeader.Location = new System.Drawing.Point(72, 32);
            this.txtBasisHeader.Name = "txtBasisHeader";
            this.txtBasisHeader.Size = new System.Drawing.Size(176, 20);
            this.txtBasisHeader.TabIndex = 15;
            this.txtBasisHeader.TextChanged += new System.EventHandler(this.txtBasisHeader_TextChanged);
            this.txtBasisHeader.DragDrop += new System.Windows.Forms.DragEventHandler(this.txtBasisHeader_DragDrop);
            this.txtBasisHeader.DragEnter += new System.Windows.Forms.DragEventHandler(this.txtBasisHeader_DragEnter);
            this.txtBasisHeader.Leave += new System.EventHandler(this.txtBasisHeader_Leave);
            this.txtBasisHeader.Validating += new System.ComponentModel.CancelEventHandler(this.txtBasisHeader_Validating);
            // 
            // lblHeader
            // 
            this.lblHeader.Location = new System.Drawing.Point(16, 32);
            this.lblHeader.Name = "lblHeader";
            this.lblHeader.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblHeader.Size = new System.Drawing.Size(48, 16);
            this.lblHeader.TabIndex = 6;
            this.lblHeader.Text = "Header:";
            this.lblHeader.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // btnGetHeader
            // 
            this.btnGetHeader.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnGetHeader.Location = new System.Drawing.Point(72, 64);
            this.btnGetHeader.Name = "btnGetHeader";
            this.btnGetHeader.Size = new System.Drawing.Size(75, 23);
            this.btnGetHeader.TabIndex = 28;
            this.btnGetHeader.Text = "Get Header";
            this.btnGetHeader.Click += new System.EventHandler(this.btnGetHeader_Click);
            // 
            // tabProperties
            // 
            this.tabProperties.Controls.Add(this.ugWorkflows);
            this.tabProperties.Location = new System.Drawing.Point(4, 22);
            this.tabProperties.Name = "tabProperties";
            this.tabProperties.Size = new System.Drawing.Size(672, 359);
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
            this.ugWorkflows.Size = new System.Drawing.Size(608, 336);
            this.ugWorkflows.TabIndex = 0;
            this.ugWorkflows.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.ugWorkflows_InitializeLayout);
            // 
            // cbxIncludeReserve
            // 
            this.cbxIncludeReserve.AutoSize = true;
            this.cbxIncludeReserve.Location = new System.Drawing.Point(72, 129);
            this.cbxIncludeReserve.Name = "cbxIncludeReserve";
            this.cbxIncludeReserve.Size = new System.Drawing.Size(107, 17);
            this.cbxIncludeReserve.TabIndex = 31;
            this.cbxIncludeReserve.Text = "Include Reserve:";
            this.cbxIncludeReserve.UseVisualStyleBackColor = true;
            this.cbxIncludeReserve.CheckedChanged += new System.EventHandler(this.cbxIncludeReserve_CheckedChanged);
            // 
            // frmRuleMethod
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(720, 510);
            this.Controls.Add(this.tabRuleMethod);
            this.Name = "frmRuleMethod";
            this.Text = "Rule Method";
            this.Load += new System.EventHandler(this.frmRuleMethod_Load);
            this.Controls.SetChildIndex(this.pnlGlobalUser, 0);
            this.Controls.SetChildIndex(this.lblName, 0);
            this.Controls.SetChildIndex(this.txtName, 0);
            this.Controls.SetChildIndex(this.txtDesc, 0);
            this.Controls.SetChildIndex(this.tabRuleMethod, 0);
            this.Controls.SetChildIndex(this.btnSave, 0);
            this.Controls.SetChildIndex(this.btnClose, 0);
            this.Controls.SetChildIndex(this.btnProcess, 0);
            this.pnlGlobalUser.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.utmMain)).EndInit();
            this.tabRuleMethod.ResumeLayout(false);
            this.tabMethod.ResumeLayout(false);
            this.gbxExcluded.ResumeLayout(false);
            this.gbxExcluded.PerformLayout();
            this.gbxIncluded.ResumeLayout(false);
            this.gbxIncluded.PerformLayout();
            this.gbxBasis.ResumeLayout(false);
            this.gbxBasis.PerformLayout();
            this.pnlStoreOrder.ResumeLayout(false);
            this.tabProperties.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ugWorkflows)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion


		override public void NewWorkflowMethod(MIDWorkflowMethodTreeNode aParentNode)
		{
			try
			{
				_ruleMethod = new RuleMethod(SAB,Include.NoRID);
				ABM = _ruleMethod;
				base.NewWorkflowMethod(aParentNode, eSecurityFunctions.AllocationMethodsUserRule, eSecurityFunctions.AllocationMethodsGlobalRule);
				
				// Nomally on a new the base method pulls the store attribute from the global options.
				// store attribute is not required for this method, so we begin with a NoRid valud.
				_ruleMethod.SG_RID = Include.NoRID;

				Common_Load(aParentNode.GlobalUserType);
				cboBasisComponent.SelectedValue = (int) eRuleMethodComponentType.Total;
				SetIncludeCombo();	
				cboInclRule.SelectedValue = (int) eRuleMethod.None;
				cboExclRule.SelectedValue = (int) eRuleMethod.None;
				
				cboBasisComponent.Enabled = false;
				pnlStoreOrder.Enabled = false;
                cbxIncludeReserve.Enabled = false; //TT#1608-MD - SRisch - Prior Header - Include Reserve 06/02/15
// (CSMITH) - BEG MID Track #3219: PO / Master Release Enhancement
				this.cbxIsHeaderMaster.Enabled = false;
// (CSMITH) - END MID Track #3219

			}
			catch (Exception exception)
			{
				HandleException(exception);
			}
		}
		/// <summary>
		/// Opens an existing Rule Method. 
		/// </summary>
		/// <param name="aSecurityLevel">The security level of the user for the data selected</param>
		/// <param name="aMethodRID">method_RID</param>
		/// <param name="aNodeRID"></param>
		/// <param name="aNode"></param>
		/// <param name="aLockStatus">The lock status of the data to be displayed</param>
		override public void UpdateWorkflowMethod(int aMethodRID, int aNodeRID, MIDWorkflowMethodTreeNode aNode, eLockStatus aLockStatus)
		{
			try
			{
				_nodeRID = aNodeRID;

				_ruleMethod = new RuleMethod(SAB,aMethodRID);
				base.UpdateWorkflowMethod(aLockStatus, eSecurityFunctions.AllocationMethodsUserRule, eSecurityFunctions.AllocationMethodsGlobalRule);

				// reset include rule for invalid rule method
                // Begin TT#212 - JSmith - Clears value when header deleted when should not
                //if (_ruleMethod.MethodStatus == eMethodStatus.InvalidMethod)
                //{
                //    _ruleMethod.IncludeQuantity = 0;
                //    _ruleMethod.IncludeRuleMethod = eRuleMethod.None;
                //}
                if (_ruleMethod.MethodStatus == eMethodStatus.InvalidMethod &&
                    (_ruleMethod.IncludeRuleMethod == eRuleMethod.Proportional ||
                    _ruleMethod.IncludeRuleMethod == eRuleMethod.Exact ||
                    _ruleMethod.IncludeRuleMethod == eRuleMethod.Fill))
                {
                    _ruleMethod.IncludeQuantity = 0;
                    _ruleMethod.IncludeRuleMethod = eRuleMethod.None;
                }
                // End TT#212

				Common_Load(aNode.GlobalUserType);

				if (_ruleMethod.HeaderID == null || _ruleMethod.HeaderID == string.Empty)
				{
					//this.btnProcess.Enabled = false;
					this.cboBasisComponent.Enabled = false;
					this.pnlStoreOrder.Enabled = false;
                    this.cbxIncludeReserve.Enabled = false; //TT#1608-MD - SRisch - Prior Header - Include Reserve 06/02/15
// (CSMITH) - BEG MID Track #3219: PO / Master Release Enhancement
					this.cbxIsHeaderMaster.Enabled = false;
// (CSMITH) - END MID Track #3219
				}
				else
				{
					txtBasisHeader.Text = _ruleMethod.HeaderID;
					// BEGIN MID Track #3219
					_headerRID = _ruleMethod.HeaderRID; 
					// END MID Track #3219
// (CSMITH) - BEG MID Track #3219: PO / Master Release Enhancement
					this.cbxIsHeaderMaster.Checked = _ruleMethod.IsHeaderMaster;
// (CSMITH) - END MID Track #3219
					// This fixes the text showing bolder/differently from the other text
					if (!FunctionSecurity.AllowUpdate)
					{
						this.txtBasisHeader.Enabled = false;
// (CSMITH) - BEG MID Track #3219: PO / Master Release Enhancement
						this.cbxIsHeaderMaster.Enabled = false;
// (CSMITH) - END MID Track #3219
					}
					GetPacksAndColors(_ruleMethod.HeaderRID);
					if (FunctionSecurity.AllowUpdate)
					{
						this.cboBasisComponent.Enabled = true;
						this.pnlStoreOrder.Enabled = true;
                        this.cbxIncludeReserve.Enabled = true; //TT#1608-MD - SRisch - Prior Header - Include Reserve 06/02/15
						//this.btnProcess.Enabled = true;
					}
				}
				
				SetIncludeCombo();

				if (_ruleMethod.PackRID != Include.NoRID)
					cboBasisPack.SelectedValue = (int) _ruleMethod.PackRID;
				else
					cboBasisPack.SelectedIndex = -1;

				if (_ruleMethod.ColorCodeRID != Include.NoRID)
					cboBasisColor.SelectedValue = (int) _ruleMethod.ColorCodeRID;
				else
					cboBasisColor.SelectedIndex = -1;
				
				cboBasisComponent.SelectedValue = (int) _ruleMethod.ComponentType;

				cboInclRule.SelectedValue = (int) _ruleMethod.IncludeRuleMethod;
				if (_ruleMethod.IncludeQuantity > 0)
					txtInclQuantity.Text = Convert.ToString(_ruleMethod.IncludeQuantity, CultureInfo.CurrentUICulture);
			
				cboExclRule.SelectedValue = (int) _ruleMethod.ExcludeRuleMethod;
				if (_ruleMethod.ExcludeQuantity > 0) 
					txtExclQuantity.Text = Convert.ToString(_ruleMethod.ExcludeQuantity, CultureInfo.CurrentUICulture);
							
				if (_ruleMethod.SortDirection == eSortDirection.Descending)
				{
					radDescend.Checked = true;
				}
				else
				{
					radAscend.Checked = true;
				}

                

				//if (_ruleMethod.FilterRID == Include.NoRID)
				//{
				//	this.cboBasisComponent.Enabled = true;
				//	this.pnlStoreOrder.Enabled = true;
				//}
				//else
				//{
				//	this.cboBasisComponent.Enabled = false;
				//	this.pnlStoreOrder.Enabled = false;
				//}
			}
			catch(Exception exception)
			{
				HandleException(exception, "InitializeRuleMethod");
				FormLoadError = true;
			}
		}

		/// <summary>
		/// Deletes a Rule Method.
		/// </summary>
		/// <param name="method_RID">The record ID of the method</param>
		override public bool DeleteWorkflowMethod(int method_RID)
		{
			try
			{  
				_ruleMethod = new RuleMethod(SAB,method_RID);
				return Delete();
			}
			catch(DatabaseForeignKeyViolation keyVio)
			{
				throw keyVio;
			}
			catch (Exception exception)
			{
				HandleException(exception);
				FormLoadError = true;
			}

			return true;
		}

		

		/// <summary>
		/// Renames a Rule Method.
		/// </summary>
		/// <param name="aMethodRID">The record ID of the method</param>
		/// <param name="aNewName">The new name of the workflow or method</param>
		override public bool RenameWorkflowMethod(int aMethodRID, string aNewName)
		{
			try
			{       
				_ruleMethod = new RuleMethod(SAB,aMethodRID);
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
				_ruleMethod = new RuleMethod(SAB,aMethodRID);
				ProcessAction(eMethodType.Rule, true);
			}
			catch (Exception ex)
			{
				HandleException(ex);
				FormLoadError = true;
			}
		}

		private void SetText()
		{
			try
			{
				if (_ruleMethod.Method_Change_Type == eChangeType.update)
					this.btnSave.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_Update);
				else
					this.btnSave.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_Save);

				this.btnClose.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_Close);
				this.btnProcess.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_Process);
				this.btnGetHeader.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_GetHeader);

				this.lblFilter.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Filter);
				this.lblSet.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Set);
				this.gbxBasis.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Prior);
				this.lblHeader.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Header);
				this.lblBasisComponent.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Component);
				this.lblPack.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Pack);
				this.lblColor.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Color);
				this.radDescend.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Descending);
				this.radAscend.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Ascending);
				this.gbxIncluded.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_IncludedStores);
				this.gbxExcluded.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_ExcludedStores);
				this.lblInclQuantity.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Quantity);
				this.lblExclQuantity.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Quantity);
// (CSMITH) - BEG MID Track #3219: PO / Master Release Enhancement
				this.cbxIsHeaderMaster.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Master);
                this.cbxIncludeReserve.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_IncludeReserve); //TT#1608-MD - srisch - Working 5.4 - Prior Header 05/27/2015 
// (CSMITH) - END MID Track #3219
			}
			catch( Exception exception )
			{
				HandleException(exception);
			}
		}

		//		private void frmRuleMethod_Load(object sender, System.EventArgs e)
		//		{
		//			
		//			FormLoaded = true;
		//		}

		private void Common_Load(eGlobalUserType aGlobalUserType)
		{
			string filterString;
			int numRule;
			try
			{
				SetText();
				if (_ruleMethod.Method_Change_Type == eChangeType.add)
				{
					Format_Title(eDataState.New, eMIDTextCode.frm_RuleMethod, null);
				}
				else if (FunctionSecurity.AllowUpdate)
				{
					Format_Title(eDataState.Updatable, eMIDTextCode.frm_RuleMethod, _ruleMethod.Name);
				}
				else
				{
					Format_Title(eDataState.ReadOnly, eMIDTextCode.frm_RuleMethod, _ruleMethod.Name);
				}

                GetWorkflows(_ruleMethod.Key, ugWorkflows);

				if (FunctionSecurity.AllowExecute)
				{
					btnProcess.Enabled = true;
				}
				else
				{
					btnProcess.Enabled = false;
				}

				BindStoreFilterComboBox();
				if (_ruleMethod.FilterRID == Include.UndefinedStoreFilter)
				{
					cboRuleFilter.SelectedValue = Include.UndefinedStoreFilter;
				}
				else
				{
					cboRuleFilter.SelectedIndex = cboRuleFilter.Items.IndexOf(new FilterNameCombo(_ruleMethod.FilterRID, -1, ""));

				}

                //Begin Track #5858 - KJohnson - Validating store security only
                // Begin TT#44 - JSmith - Drag/Drop User Attributes or Filters in to Global Methods does not react consistantly
                //cboStoreAttribute.Tag = new MIDStoreAttributeComboBoxTag(SAB, cboStoreAttribute);
                //cboRuleFilter.Tag = new MIDStoreFilterComboBoxTag(SAB, cboRuleFilter, eSecurityTypes.Store, eSecuritySelectType.View | eSecuritySelectType.Update);
                cboStoreAttribute.Tag = new MIDStoreAttributeComboBoxTag(SAB, cboStoreAttribute, FunctionSecurity, _ruleMethod.GlobalUserType == eGlobalUserType.User);
                cboRuleFilter.Tag = new MIDStoreFilterComboBoxTag(SAB, cboRuleFilter.ComboBox, eSecurityTypes.Store, eSecuritySelectType.View | eSecuritySelectType.Update, FunctionSecurity, _ruleMethod.GlobalUserType == eGlobalUserType.User);
                // End TT#44
                //End Track #5858
				BindStoreAttrComboBox();
				if (_ruleMethod.SG_RID == Include.NoRID)
				{
					//cboStoreAttribute.SelectedValue = SAB.ApplicationServerSession.GlobalOptions.AllocationStoreGroupRID;
					cboStoreAttribute.SelectedValue = Include.NoRID;
                    // Begin TT#1530 - RMatelic - Record locking violations from Store Attribute change
                    cboStoreAttribute.Enabled = FunctionSecurity.AllowUpdate;
                    // End TT#1530
				}
				else
				{
                    //StoreGroupLevelProfile sglp = StoreMgmt.GetStoreGroupLevel(_ruleMethod.StoreGroupLevelRID); //SAB.StoreServerSession.GetStoreGroupLevel(_ruleMethod.StoreGroupLevelRID);
					cboStoreAttribute.SelectedValue = _ruleMethod.SG_RID;
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
					//cboStoreAttribute.SelectedValue = sglp.GroupRid;
					cboAttributeSet.SelectedValue = _ruleMethod.StoreGroupLevelRID;
				}

				_ComponentDataTable = MIDText.GetTextType(eMIDTextType.eComponentType, eMIDTextOrderBy.TextValue);
				foreach(DataRow dr in _ComponentDataTable.Rows)
				{
					eRuleMethodComponentType rmct = (eRuleMethodComponentType)(Convert.ToInt32(dr["TEXT_CODE"], CultureInfo.CurrentUICulture));
					if (!Enum.IsDefined(typeof(eRuleMethodComponentType),rmct))
					{
						dr.Delete();
					}
				}

                //BEGIN TT#1608-MD - srisch - Working 5.4 - Prior Header 05/27/2015
                if (_ruleMethod.IncludeReserve == true )
                {
                    cbxIncludeReserve.Checked = true;
                }
                else 
                {
                    cbxIncludeReserve.Checked = false;
                }
                //END TT#1608-MD - srisch - Working 5.4 - Prior Header 05/27/2015 
				_ComponentDataTable.AcceptChanges();
				this.cboBasisComponent.DataSource = _ComponentDataTable;
				this.cboBasisComponent.DisplayMember = "TEXT_VALUE";
				this.cboBasisComponent.ValueMember = "TEXT_CODE";

				_IncludeRuleDataTable = MIDText.GetTextType(eMIDTextType.eRuleMethod, eMIDTextOrderBy.TextValue);
				this.cboInclRule.DataSource = _IncludeRuleDataTable.DefaultView;
				this.cboInclRule.DisplayMember = "TEXT_VALUE";
				this.cboInclRule.ValueMember = "TEXT_CODE";

				numRule = (int)eRuleMethod.Exact;
				filterString = "TEXT_CODE < " + Convert.ToString(numRule,CultureInfo.CurrentUICulture);
				_IncludeRuleDataTable.DefaultView.RowFilter = filterString;

				_ExcludeRuleDataTable = MIDText.GetTextType(eMIDTextType.eRuleMethod, eMIDTextOrderBy.TextValue);
				this.cboExclRule.DataSource = _ExcludeRuleDataTable.DefaultView;
				this.cboExclRule.DisplayMember = "TEXT_VALUE";
				this.cboExclRule.ValueMember = "TEXT_CODE";
				numRule = (int)eRuleMethod.Exact;
				filterString = "TEXT_CODE < " + Convert.ToString(numRule,CultureInfo.CurrentUICulture);
				_ExcludeRuleDataTable.DefaultView.RowFilter = filterString;

				_PackDataTable = MIDEnvironment.CreateDataTable();
				_PackDataTable.Columns.Add("PackRID");
				_PackDataTable.Columns.Add("PackName");
				this.cboBasisPack.DataSource = _PackDataTable;
				this.cboBasisPack.DisplayMember = "PackName";
				this.cboBasisPack.ValueMember = "PackRID";
				
				_ColorDataTable = MIDEnvironment.CreateDataTable();
				_ColorDataTable.Columns.Add("ColorRID");
				_ColorDataTable.Columns.Add("ColorName");
                _ColorDataTable.Columns.Add("HDR_BC_RID");
				this.cboBasisColor.DataSource = _ColorDataTable;
				this.cboBasisColor.DisplayMember = "ColorName";
				this.cboBasisColor.ValueMember = "ColorRID";
				
				SetReadOnly(FunctionSecurity.AllowUpdate);  //Security changes - 1/24/2005 vg

				if (!FunctionSecurity.AllowUpdate)
				{
					foreach (UltraGridBand ugb in this.ugWorkflows.DisplayLayout.Bands)
					{
						ugb.Override.AllowDelete = DefaultableBoolean.False;
					}
				}

//				if (aGlobalUserType == eGlobalUserType.User &&
//					!SAB.ClientServerSession.GetUserFunctionSecurityAssignment(SAB.ClientServerSession.UserRID, eSecurityFunctions.AllocationMethodsGlobalRule).AllowUpdate)
//				{
//					radGlobal.Enabled = false;
//				}

                //BEGIN TT#110-MD-VStuart - In Use Tool
                tabRuleMethod.Controls.Remove(tabProperties);
                //END TT#110-MD-VStuart - In Use Tool
                //cbxIncludeReserve.Checked = _ruleMethod.IncludeReserve; //TT#1608-MD - srisch - Working 5.4 - Prior Header 05/27/2015 
            }
			catch( Exception exception )
			{
				HandleException(exception);
			}
		}
		/// <summary>
		/// Populate all Store_Groups (Attributes); 1st sel if new else selection made
		/// in load
		/// </summary>
		private void BindStoreAttrComboBox()
		{
            try
			{
				// Begin Track #4872 - JSmith - Global/User Attributes
                //ProfileList al = SAB.StoreServerSession.GetStoreGroupListViewList();

                //StoreGroupListViewProfile sgp = new StoreGroupListViewProfile(Include.NoRID);
                //sgp.Name=string.Empty;
                //al.Add(sgp);

                BuildAttributeList();

                AdjustTextWidthComboBox_DropDown(cboStoreAttribute);    //TT#7 - RBeck - Dynamic dropdowns

                //this.cboStoreAttribute.ValueMember = "Key";
                //this.cboStoreAttribute.DisplayMember = "Name";
                //this.cboStoreAttribute.DataSource = al.ArrayList;
                // End Track #4872

				this.cboStoreAttribute.SelectedValue = Include.NoRID;
        
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}

		private void BindStoreFilterComboBox()
		{

			//			if (_filterSecurity != eSecurityLevel.NoAccess && _filterSecurity != eSecurityLevel.NotSpecified)
			//			{
			_storeFilterData = new FilterData();

			ArrayList userRIDList = new ArrayList();
			userRIDList.Add(Include.GlobalUserRID);	// Issue 3806
			userRIDList.Add(SAB.ClientServerSession.UserRID);
            _dtStoreFilter = _storeFilterData.FilterRead(filterTypes.StoreFilter, eProfileType.FilterStore, userRIDList);

			// Add 'empty' or 'none' row
            // Begin TT#2669 - JSmith - Unable to remove a filter from a wokflow
            //cboRuleFilter.Items.Add(new FilterNameCombo(Include.NoRID, Include.GlobalUserRID," "));	// Issue 3806
            cboRuleFilter.Items.Add(GetRemoveFilterRow());
            // End TT#2669 - JSmith - Unable to remove a filter from a wokflow
				
			foreach (DataRow row in _dtStoreFilter.Rows)
			{
				cboRuleFilter.Items.Add(
					new FilterNameCombo(Convert.ToInt32(row["FILTER_RID"], CultureInfo.CurrentUICulture),
					Convert.ToInt32(row["USER_RID"], CultureInfo.CurrentUICulture),
					Convert.ToString(row["FILTER_NAME"], CultureInfo.CurrentUICulture)));
			}
			//			}

		}


		private void cboInclRule_SelectionChangeCommitted(object sender, System.EventArgs e)
		{
			try
			{
				if (cboInclRule.SelectedIndex > -1)
				{
					DataRowView drv = _IncludeRuleDataTable.DefaultView[cboInclRule.SelectedIndex];
					DataRow dr = drv.Row;

					eRuleMethodRequiresQuantity rqm = (eRuleMethodRequiresQuantity)(Convert.ToInt32(dr["TEXT_CODE"], CultureInfo.CurrentUICulture));
					if (Enum.IsDefined(typeof(eRuleMethodRequiresQuantity),rqm))
					{
						txtInclQuantity.Enabled = true;
					}
					else
					{
						txtInclQuantity.Text = string.Empty;
						txtInclQuantity.Enabled = false;
					}

					if (FormLoaded)
					{
						ChangePending = true;
					}
				}
			}
			catch( Exception exception )
			{
				HandleException(exception);
			}
		}

		private void cboExclRule_SelectionChangeCommitted(object sender, System.EventArgs e)
		{
			try
			{
				if (cboExclRule.SelectedIndex > -1)
				{
					DataRowView drv = _ExcludeRuleDataTable.DefaultView[cboExclRule.SelectedIndex];
					DataRow dr = drv.Row;
					eRuleMethodRequiresQuantity rqm = (eRuleMethodRequiresQuantity)(Convert.ToInt32(dr["TEXT_CODE"], CultureInfo.CurrentUICulture));
					if (Enum.IsDefined(typeof(eRuleMethodRequiresQuantity),rqm))
					{
						txtExclQuantity.Enabled = true;
					}
					else
					{
						txtExclQuantity.Enabled = false;
					}

					if (FormLoaded)
					{
						ChangePending = true;
					}
				}
			}
			catch( Exception exception )
			{
				HandleException(exception);
			}
		}

		private void cboBasisComponent_SelectionChangeCommitted(object sender, System.EventArgs e)
		{
			try
			{
				if (cboBasisComponent.SelectedIndex > -1)
				{
					DataRow dr = _ComponentDataTable.Rows[cboBasisComponent.SelectedIndex];
					eRuleMethodComponentType rmct = (eRuleMethodComponentType)(Convert.ToInt32(dr["TEXT_CODE"], CultureInfo.CurrentUICulture));

					if (rmct == eRuleMethodComponentType.SpecificPack)
					{
						if (FunctionSecurity.AllowUpdate)
						{
							cboBasisPack.Enabled = true;
						}
						if (_PackDataTable.Rows.Count > 0)
							cboBasisPack.SelectedIndex = 0;
					}
					else
					{
						cboBasisPack.SelectedIndex = -1;
						cboBasisPack.Enabled = false;
					}

					if (rmct == eRuleMethodComponentType.SpecificColor)
					{
						if (FunctionSecurity.AllowUpdate)
						{
							cboBasisColor.Enabled = true;
						}
						if (_ColorDataTable.Rows.Count > 0)
							cboBasisColor.SelectedIndex = 0;
					}
					else
					{
						cboBasisColor.SelectedIndex = -1;
						cboBasisColor.Enabled = false;
					}
					if (FormLoaded)
					{
						ChangePending = true;
					}
				}
				
			}
			catch( Exception exception )
			{
				HandleException(exception);
			}
		}

		// Begin MID Track 4858 - JSmith - Security changes
//		private void btnSave_Click(object sender, System.EventArgs e)
//		{
//			try
//			{
//				Save_Click(true);
//				// change everything to 'update'
//				if (!ErrorFound)
//				{
//					_ruleMethod.Method_Change_Type = eChangeType.update;
//					btnSave.Text = "&Update";
//				}
//			}
//			catch( Exception exception )
//			{
//				HandleException(exception);
//			}
//		}
//
//		private void btnClose_Click(object sender, System.EventArgs e)
//		{
//			try
//			{
//				Cancel_Click();
//			}
//			catch( Exception exception )
//			{
//				HandleException(exception);
//			}
//		}
//
//		private void btnProcess_Click(object sender, System.EventArgs e)
//		{
//// (CSMITH) - BEG MID Track #3219: PO / Master Release Enhancement
//			Header header = null;
//
//			string msgText = null;
//
//			int subordRID = Include.NoRID;
//
//			SelectedHeaderList selectedHeaderList = null;
//
//			try
//			{
//				selectedHeaderList = (SelectedHeaderList)SAB.ClientServerSession.GetSelectedHeaderList();
//
//				if (selectedHeaderList.Count == 0)
//				{
//					MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_NoHeaderSelectedOnWorkspace));
//
//					return;
//				}
//				else
//				{
//					if (this.cbxIsHeaderMaster.Checked)
//					{
//						if (selectedHeaderList.Count > 1)
//						{
//							MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MultHeadersSelectedOnWorkspace));
//
//							return;
//						}
//						else
//						{
//							header = new Header();
//
//							subordRID = header.GetSubordForMaster(this._headerRID);
//
//							if (subordRID != Include.NoRID)
//							{
//								SelectedHeaderProfile shp = (SelectedHeaderProfile)selectedHeaderList[0];
//
//								if (subordRID != shp.Key)
//								{
//									msgText = MIDText.GetTextOnly(eMIDTextCode.msg_al_MasterAlreadyAssigned);
//									msgText = msgText.Replace("{0}", this.txtBasisHeader.Text.Trim());
//									//msgText = msgText.Replace("{1}", shp.HeaderID);
//									string subHeaderID = header.GetSubordinateID(subordRID);
//									msgText = msgText.Replace("{1}", subHeaderID);
//									MessageBox.Show(msgText);
//
//									return;
//								}
//							}
//						}
//					}
//				}
//// (CSMITH) - END MID Track #3219
//				ProcessAction(eMethodType.Rule);
//
//				// as part of the  processing we saved the info, so it should be changed to update.
//				if (!ErrorFound)
//				{
//					_ruleMethod.Method_Change_Type = eChangeType.update;
//					btnSave.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_Update);
//					// BEGIN MID Track #3219
//					if (this.cbxIsHeaderMaster.Checked)
//					{
//						int[] hdrIdList;
//						hdrIdList = new int[1];
//						hdrIdList[0] = _ruleMethod.HeaderRID;
//						_EAB.AllocationWorkspaceExplorer.ReloadUpdatedHeaders(hdrIdList);
//					}
//					// END MID Track #3219
//				}
//			}
//			catch(Exception ex)
//			{
//				HandleException(ex, "btnProcess_Click");
//			}
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
		protected override void Call_btnProcess_Click()
		{
			Header header = null;

			string msgText = null;

			int subordRID = Include.NoRID;
            // Begin TT#1746-MD - RMatelic - Rule Method with master header receives null reference when Process button is clicked 
            //SelectedHeaderList selectedHeaderList = null;
            SelectedHeaderList selectedHeaderList = (SelectedHeaderList)SAB.ClientServerSession.GetSelectedHeaderList();
            // End TT#1746-MD
			try
			{
				// BEGIN TT#696-MD - Stodd - add "active process"
				//// BEGIN TT#497-MD - stodd -  Methods will not process with Method open
				////bool isProcessingInAssortment = false;
				//bool useAssortmentHeaders = false;

				//useAssortmentHeaders = UseAssortmentSelectedHeaders();
				//if (useAssortmentHeaders)
				//{
				//    selectedHeaderList = SAB.AssortmentSelectedHeaderEvent.GetSelectedHeaders(this, eMethodType.Rule);
				//}
				//else
				//{
				//    // BEGIN MID Track #6022 - DB error trying to process new unsaved header
				//    selectedHeaderList = (SelectedHeaderList)SAB.ClientServerSession.GetSelectedHeaderList();
				//}
				////selectedHeaderList = (SelectedHeaderList)SAB.ClientServerSession.GetSelectedHeaderList();
				//// END TT#497-MD - stodd -  Methods will not process with Method open

				//if (selectedHeaderList.Count == 0)
				//{
				//    MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_NoHeaderSelectedOnWorkspace));

				//    return;
				//}
				//else

				//====================================================
				// Checks to be sure there are valid selected headers
				//====================================================
				if (!OkToProcess(this, eMethodType.Rule))
				{
					return;
				}


				//{
				// END TT#696-MD - Stodd - add "active process"
				if (this.cbxIsHeaderMaster.Checked)
				{
					if (selectedHeaderList.Count > 1)
					{
						MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MultHeadersSelectedOnWorkspace));

						return;
					}
					else
					{
						header = new Header();

						subordRID = header.GetSubordForMaster(this._headerRID);

						if (subordRID != Include.NoRID)
						{
							SelectedHeaderProfile shp = (SelectedHeaderProfile)selectedHeaderList[0];

							if (subordRID != shp.Key)
							{
								msgText = MIDText.GetTextOnly(eMIDTextCode.msg_al_MasterAlreadyAssigned);
								msgText = msgText.Replace("{0}", this.txtBasisHeader.Text.Trim());
								//msgText = msgText.Replace("{1}", shp.HeaderID);
								string subHeaderID = header.GetSubordinateID(subordRID);
								msgText = msgText.Replace("{1}", subHeaderID);
								MessageBox.Show(msgText);

								return;
							}
						}
					}
				}
				//}
				ProcessAction(eMethodType.Rule);

				// as part of the  processing we saved the info, so it should be changed to update.
				if (!ErrorFound)
				{
					_ruleMethod.Method_Change_Type = eChangeType.update;
					btnSave.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_Update);
					if (this.cbxIsHeaderMaster.Checked)
					{
						int[] hdrIdList;
						hdrIdList = new int[1];
						hdrIdList[0] = _ruleMethod.HeaderRID;
						EAB.AllocationWorkspaceExplorer.ReloadUpdatedHeaders(hdrIdList);
					}
				}
			}
			catch(Exception ex)
			{
				HandleException(ex, "Call_btnProcess_Click");
			}
		}
		// End MID Track 4858

		private void txtBasisHeader_Leave(object sender, System.EventArgs e)
		{
			try
			{
				if (this.txtBasisHeader.Text.Length > 0)
				{
					this.cboBasisComponent.Enabled = true;
					this.pnlStoreOrder.Enabled = true;
                    this.cbxIncludeReserve.Enabled = true; //TT#1608-MD - SRisch - Prior Header - Include Reserve 06/02/15
// (CSMITH) - BEG MID Track #3219: PO / Master Release Enhancement
					// begin MID Track 3623 Port Master Allocation
					//this.cbxIsHeaderMaster.Enabled = true;
                    // BEGIN TT#2133-MD - AGallagher - Master option is no longer valid and should not be visable or selectable 
                    //if (SAB.ClientServerSession.GlobalOptions.AppConfig.MasterAllocationInstalled)
                    //{
                    //    this.cbxIsHeaderMaster.Enabled = true;
                    //}
                    //else
                    //{
                    // END TT#2133-MD - AGallagher - Master option is no longer valid and should not be visable or selectable 
						this.cbxIsHeaderMaster.Hide();
                    //}  // TT#2133-MD - AGallagher - Master option is no longer valid and should not be visable or selectable 
					// end MID Track 3623 Port Master Allocation
// (CSMITH) - END MID Track #3219
				}
			}
			catch( Exception exception )
			{
				HandleException(exception);
			}
		}

		private void txtBasisHeader_TextChanged(object sender, System.EventArgs e)
		{
			if (FormLoaded)
			{
				ChangePending = true;
				if (this.txtBasisHeader.Text.Trim().Length > 0)
				{
					this.cboBasisComponent.Enabled = true;
					this.pnlStoreOrder.Enabled = true;
                    this.cbxIncludeReserve.Enabled = true; //TT#1608-MD - SRisch - Prior Header - Include Reserve 06/02/15
// (CSMITH) - BEG MID Track #3219: PO / Master Release Enhancement
					// begin MID Track 3623 Port Master Allocation
					//this.cbxIsHeaderMaster.Enabled = true;
                    // BEGIN TT#2133-MD - AGallagher - Master option is no longer valid and should not be visable or selectable 
                    //if (SAB.ClientServerSession.GlobalOptions.AppConfig.MasterAllocationInstalled)
                    //{
                    //    this.cbxIsHeaderMaster.Enabled = true;
                    //}
                    //else
                    //{
                    // END TT#2133-MD - AGallagher - Master option is no longer valid and should not be visable or selectable 
						this.cbxIsHeaderMaster.Hide();
                    //}  // TT#2133-MD - AGallagher - Master option is no longer valid and should not be visable or selectable 
					// end MID Track 3623 Port Master Allocation
// (CSMITH) - END MID Track #3219
				
					if (!radDescend.Checked && !radAscend.Checked)
						radDescend.Checked = true;
				}
				else
				{
					cboBasisComponent.SelectedValue = (int) eRuleMethodComponentType.Total;
			    	this.cboBasisComponent.Enabled = false;
					this.pnlStoreOrder.Enabled = false;
                    this.cbxIncludeReserve.Enabled = false; //TT#1608-MD - SRisch - Prior Header - Include Reserve 06/02/15
// (CSMITH) - BEG MID Track #3219: PO / Master Release Enhancement
					this.cbxIsHeaderMaster.Checked = false;
					this.cbxIsHeaderMaster.Enabled = false;
// (CSMITH) - END MID Track #3219
					// Begin Issue 4039 - stodd
					_headerRID = Include.NoRID;
					// End issue 4039
				}
			}
		}

		private void txtBasisHeader_DragDrop(object sender, System.Windows.Forms.DragEventArgs e)
		{
            HeaderClipboardList cbList;
            TreeNodeClipboardProfile cbProf;
            try
            {
                //ClipboardProfile cbp = GetClipboardData(eClipboardDataType.Header);
                if (e.Data.GetDataPresent(typeof(HeaderClipboardList)))
                {
                    cbList = (HeaderClipboardList)e.Data.GetData(typeof(HeaderClipboardList));
                    //HeaderClipboardData header_cbd = (HeaderClipboardData)cbp.ClipboardData;
                    //txtBasisHeader.Text = header_cbd.HeaderName;
                    txtBasisHeader.Text = cbList.ClipboardProfile.HeaderName;
                    SetIncludeCombo();
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

		private void txtBasisHeader_DragEnter(object sender, System.Windows.Forms.DragEventArgs e)
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
			try
			{
				WorkflowMethodName = txtName.Text;
				WorkflowMethodDescription = txtDesc.Text;
				GlobalRadioButton = radGlobal;
				UserRadioButton = radUser;
			}
			catch (Exception exception)
			{
				HandleException(exception);
			}
		}

		/// <summary>
		/// Use to set the specific fields in method object before updating
		/// </summary>
		override protected void SetSpecificFields()
		{
			try
			{
				//_ruleMethod.SG_RID = Include.AllStoreFilterRID;
				if (radAscend.Checked)
				{
					_ruleMethod.SortDirection = eSortDirection.Ascending;
				}
				else
				{
					_ruleMethod.SortDirection = eSortDirection.Descending;
				}

				FilterNameCombo aFilter = (FilterNameCombo)cboRuleFilter.SelectedItem;
				if (aFilter == null)
					_ruleMethod.FilterRID = Include.UndefinedStoreFilter;
				else
					_ruleMethod.FilterRID = aFilter.FilterRID;
				
				if (cboStoreAttribute.SelectedValue != null)
				{
					_ruleMethod.SG_RID = Convert.ToInt32(cboStoreAttribute.SelectedValue, CultureInfo.CurrentUICulture);
				}

				if (cboAttributeSet.SelectedValue != null)
				{
					int sgl_rid = Convert.ToInt32(cboAttributeSet.SelectedValue, CultureInfo.CurrentUICulture);
					if (sgl_rid == Include.NoRID)
						_ruleMethod.StoreGroupLevelRID = Include.AllStoreFilterRID;
					else
						_ruleMethod.StoreGroupLevelRID = Convert.ToInt32(cboAttributeSet.SelectedValue, CultureInfo.CurrentUICulture);
				}
				_ruleMethod.HeaderID = txtBasisHeader.Text;
				_ruleMethod.HeaderRID = _headerRID;
// (CSMITH) - BEG MID Track #3219: PO / Master Release Enhancement
				if (this.cbxIsHeaderMaster.Checked      			// MID Track 3623 Port Master Allocation
					&& this.SAB.ClientServerSession.GlobalOptions.AppConfig.MasterAllocationInstalled) 	// MID Track 3623 Port Master Allocation
				{
					_ruleMethod.IsHeaderMaster = true;
				}
				else
				{
					_ruleMethod.IsHeaderMaster = false;
				}
// (CSMITH) - END MID Track #3219

				_ruleMethod.ComponentType = (eComponentType)Convert.ToInt32(cboBasisComponent.SelectedValue, CultureInfo.CurrentUICulture);

				if (_ruleMethod.ComponentType == eComponentType.Total ||
					_ruleMethod.ComponentType == eComponentType.Bulk)
				{
					_ruleMethod.PackRID = Include.NoRID;
					_ruleMethod.PackName = string.Empty;
				}
				else if (cboBasisPack.SelectedValue != null)
				{
					_ruleMethod.PackRID = Convert.ToInt32(cboBasisPack.SelectedValue, CultureInfo.CurrentUICulture);
					_ruleMethod.PackName = cboBasisPack.Text;
				}
				else
				{
					_ruleMethod.PackRID = Include.NoRID;
					_ruleMethod.PackName = string.Empty;
				}
                // Assortment BEGIN
                //if (_ruleMethod.ComponentType == eComponentType.Total ||
                //    _ruleMethod.ComponentType == eComponentType.Bulk)
                //{
                //    _ruleMethod.ColorCodeRID = Include.NoRID;
                //}
                //else if (cboBasisColor.SelectedValue != null)
                if (_ruleMethod.ComponentType == eComponentType.SpecificColor)    
				{
					_ruleMethod.ColorCodeRID = Convert.ToInt32(cboBasisColor.SelectedValue, CultureInfo.CurrentUICulture);
                    if (_ColorDataTable.Rows.Count > 0)
                    {
                        for (int i = 0; i < _ColorDataTable.Rows.Count; i++)
                        {
                            DataRow dRow = _ColorDataTable.Rows[i];
                            int cRID = Convert.ToInt32(dRow["ColorRID"], CultureInfo.CurrentUICulture);
                            if (cRID == _ruleMethod.ColorCodeRID)
                            {
                                _ruleMethod.HdrBCRID = Convert.ToInt32(dRow["HDR_BC_RID"], CultureInfo.CurrentUICulture);
                                break;
                            }
                        }
                    }	
                }
				else
				{
					_ruleMethod.ColorCodeRID = Include.NoRID;
                    _ruleMethod.HdrBCRID = Include.NoRID;
				}
	            // Assortment END
	
				_ruleMethod.IncludeRuleMethod = (eRuleMethod)Convert.ToInt32(cboInclRule.SelectedValue, CultureInfo.CurrentUICulture);
				if (_ruleMethod.IncludeRuleMethod == eRuleMethod.Quantity)
				{
					_ruleMethod.IncludeQuantity = Convert.ToInt32(txtInclQuantity.Text, CultureInfo.CurrentUICulture);
				}
				else
				{
					_ruleMethod.IncludeQuantity = 0;
				}

				_ruleMethod.ExcludeRuleMethod = (eRuleMethod)Convert.ToInt32(cboExclRule.SelectedValue, CultureInfo.CurrentUICulture);
				if (_ruleMethod.ExcludeRuleMethod == eRuleMethod.Quantity)
				{
					_ruleMethod.ExcludeQuantity = Convert.ToInt32(txtExclQuantity.Text, CultureInfo.CurrentUICulture);
				}
				else
				{
					_ruleMethod.ExcludeQuantity = 0;
				}
			}
			catch(Exception exception)
			{
				HandleException(exception);
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

				Header header;
				ErrorProvider.SetError (txtBasisHeader,string.Empty);
				if (txtBasisHeader.Text.Trim() != string.Empty)
				{
					header = new Header();
					if (header.HeaderExists(txtBasisHeader.Text.Trim()))
					{
						_headerRID = header.GetHeaderRID(txtBasisHeader.Text.Trim());
					}
					else
					{
						methodFieldsValid = false;
						ErrorProvider.SetError (txtBasisHeader,SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_HeaderNotFound));
					}
				}
				// if selected component is Pack or Color, require a corresponding selection  
				ErrorProvider.SetError (cboBasisPack,string.Empty);
				ErrorProvider.SetError (cboBasisColor,string.Empty);
				if ((eRuleMethodComponentType)cboBasisComponent.SelectedValue == eRuleMethodComponentType.SpecificPack
					&& cboBasisPack.SelectedIndex == -1)
				{
					methodFieldsValid = false;
					ErrorProvider.SetError (cboBasisPack,SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ValidPackIsRequired));
				}
				else if ((eRuleMethodComponentType)cboBasisComponent.SelectedValue == eRuleMethodComponentType.SpecificColor 
					&& cboBasisColor.SelectedIndex == -1)
				{
					methodFieldsValid = false;
					ErrorProvider.SetError (cboBasisColor,SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ValidColorIsRequired));
				}
				ErrorProvider.SetError (txtInclQuantity,string.Empty);
				if ((eRuleMethod)Convert.ToInt32(cboInclRule.SelectedValue, CultureInfo.CurrentUICulture) == eRuleMethod.Quantity)
				{
					try
					{
						int quantity = Convert.ToInt32(txtInclQuantity.Text, CultureInfo.CurrentUICulture);
						if (quantity < 1)
						{
							methodFieldsValid = false;
							ErrorProvider.SetError (txtInclQuantity, SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MustBePositiveInteger));
						}
					}
					catch (Exception ex)
					{
						string exceptionMessage = ex.Message;
						methodFieldsValid = false;
						ErrorProvider.SetError (txtInclQuantity, SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MustBePositiveInteger));
					}
				}

				ErrorProvider.SetError (txtExclQuantity,string.Empty);
				if ((eRuleMethod)Convert.ToInt32(cboExclRule.SelectedValue, CultureInfo.CurrentUICulture) == eRuleMethod.Quantity)
				{
					try
					{
						int quantity = Convert.ToInt32(txtExclQuantity.Text, CultureInfo.CurrentUICulture);
						if (quantity < 1)
						{
							methodFieldsValid = false;
							ErrorProvider.SetError (txtExclQuantity, SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MustBePositiveInteger));
						}
					}
					catch (Exception ex)
					{
						string exceptionMessage = ex.Message;
						methodFieldsValid = false;
						ErrorProvider.SetError (txtExclQuantity, SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MustBePositiveInteger));
					}
				}
				
				if (!ValidRuleAndComponent())
				{
					methodFieldsValid = false;
				}

				return methodFieldsValid;
			}
			catch(Exception exception)
			{
				HandleException(exception);
				return methodFieldsValid;
			}
		}

		private bool ValidRuleAndComponent()
		{
			bool isValid = true;
			eRuleMethod inclRule, exclRule;;
//			eRuleMethodComponentType compType;
			try
			{
				ErrorProvider.SetError (cboBasisComponent,string.Empty);
				ErrorProvider.SetError (cboInclRule,string.Empty);
				ErrorProvider.SetError (cboExclRule,string.Empty);
				ErrorProvider.SetError (cboStoreAttribute,string.Empty);
                
				inclRule = (eRuleMethod)Convert.ToInt32(cboInclRule.SelectedValue);
				exclRule = (eRuleMethod)Convert.ToInt32(cboExclRule.SelectedValue);
				
				if (inclRule == eRuleMethod.None && exclRule == eRuleMethod.None)
				{
					ErrorProvider.SetError (cboInclRule,SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_InclExclInstructionNotAllowed));
					ErrorProvider.SetError (cboExclRule,SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_InclExclInstructionNotAllowed));
					isValid = false;
				}

				if(cboStoreAttribute.SelectedIndex == Include.NoRID && cboStoreAttribute.Text.Length > 0)
				{
					ErrorProvider.SetError (cboStoreAttribute, String.Format(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ValueRequired), "Attribute"));
					isValid = false;
				}

				if(cboStoreAttribute.SelectedIndex != Include.NoRID && cboAttributeSet.SelectedIndex == Include.NoRID)
				{
					ErrorProvider.SetError (cboAttributeSet, String.Format(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ValueRequired), "Set"));
					isValid = false;
				}
				
//				else
//				{
//					compType = (eRuleMethodComponentType)cboBasisComponent.SelectedValue; 
//					switch (inclRule)
//					{
//						case eRuleMethod.StockMinimum:
//						case eRuleMethod.StockMaximum:
//						case eRuleMethod.AdMinimum:
//							if (compType != eRuleMethodComponentType.Total)
//							{
//								ErrorProvider.SetError (cboBasisComponent,SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ComponentInvalidForInstruction));
//								ErrorProvider.SetError (cboInclRule,SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_TotalInstructionComponentMismatch));
//								isValid = false;	
//							}
//							break;
//
//						case eRuleMethod.ColorMinimum:
//						case eRuleMethod.ColorMaximum:
//							if (	compType != eRuleMethodComponentType.Bulk
//								&&	compType != eRuleMethodComponentType.MatchingColor 
//								&&	compType != eRuleMethodComponentType.SpecificColor 
//							   )
//							{
//								ErrorProvider.SetError (cboBasisComponent,SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ComponentInvalidForInstruction));
//								ErrorProvider.SetError (cboInclRule,SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ColorInstructionComponentMismatch));
//								isValid = false;
//							}
//							break;
//					}
//					
//					switch (exclRule)
//					{
//						case eRuleMethod.StockMinimum:
//						case eRuleMethod.StockMaximum:
//						case eRuleMethod.AdMinimum:
//							if (compType != eRuleMethodComponentType.Total)
//							{
//								ErrorProvider.SetError (cboBasisComponent,SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ComponentInvalidForInstruction));
//								ErrorProvider.SetError (cboExclRule,SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_TotalInstructionComponentMismatch));
//								isValid = false;	
//							}
//							break;
//
//						case eRuleMethod.ColorMinimum:
//						case eRuleMethod.ColorMaximum:
//							if (	compType != eRuleMethodComponentType.Bulk
//								&&	compType != eRuleMethodComponentType.MatchingColor 
//								&&	compType != eRuleMethodComponentType.SpecificColor 
//								)
//							{
//								ErrorProvider.SetError (cboBasisComponent,SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ComponentInvalidForInstruction ));
//								ErrorProvider.SetError (cboExclRule,SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ColorInstructionComponentMismatch));
//								isValid = false;
//							}
//							break;
//					}
//				 }		

			}
			catch(Exception exception)
			{
				HandleException(exception);
			}
			return isValid;
		}	

		/// <summary>
		/// Use to set the errors to the screen
		/// </summary>
		override protected void HandleErrors()
		{
			try
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
			catch(Exception exception)
			{
				HandleException(exception);
			}
		}

		/// <summary>
		/// Use to set the specific method object before updating
		/// </summary>
		override protected void SetObject()
		{
			try
			{
				ABM = _ruleMethod;
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
//			catch(Exception ex)
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
			catch(Exception ex)
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

		private void cboAttributeSet_SelectionChangeCommitted(object sender, System.EventArgs e)
		{
			if (FormLoaded)
			{
				ChangePending = true;
			}
		}

		private void cboStoreAttribute_SelectionChangeCommitted(object sender, System.EventArgs e)
		{
			try
			{ 
				if (this.cboStoreAttribute.SelectedValue != null)
					PopulateStoreAttributeSet(this.cboStoreAttribute.SelectedValue.ToString());
			}
			catch (Exception ex)
			{
				HandleException(ex);
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
                    ChangePending = true;
                }
            }
            catch (Exception exc)
            {
                HandleException(exc);
            }
            //End Track #5858
        }
		/// <summary>
		/// Populate all values of the Store_Group_Levels (Attribute Sets)
		/// based on the key parameter.
		/// </summary>
		/// <param name="key">SGL_RID</param>
private void PopulateStoreAttributeSet(string key)
{
	ProfileList attrSetList;
			
	try
	{
		ProfileList pl = null;
		int attrKey = Convert.ToInt32(key, CultureInfo.CurrentUICulture);
		if (attrKey != Include.NoRID)
			pl = StoreMgmt.StoreGroup_GetLevelListViewList(Convert.ToInt32(key, CultureInfo.CurrentUICulture)); //SAB.StoreServerSession.GetStoreGroupLevelListViewList(Convert.ToInt32(key, CultureInfo.CurrentUICulture));
		else
			pl = null;
			
		attrSetList = new ProfileList(eProfileType.StoreGroupLevelListView);
		// if attribute is empty, then attribute set needs to have empty entry too
		if (pl == null)
		{
			StoreGroupLevelListViewProfile sglp = new StoreGroupLevelListViewProfile(Include.NoRID);
			sglp.Name = string.Empty;
			attrSetList.Add(sglp);
		}
		else
		{
			foreach (StoreGroupLevelListViewProfile sglProf in pl)
			{
				attrSetList.Add(sglProf);
			}
		}

		cboAttributeSet.ValueMember = "Key";
		cboAttributeSet.DisplayMember = "Name";
		cboAttributeSet.DataSource = attrSetList.ArrayList;
		if (this.cboAttributeSet.Items.Count > 0)	
		{
			//					if (_loading && _trans.AllocationCriteriaExists)
			//						this.cboAttributeSet.SelectedValue = _trans.AllocationStoreGroupLevel;
			//					else
			this.cboAttributeSet.SelectedIndex = 0;
		}
		
	}
	catch (Exception ex)
	{
		HandleException(ex);
	}
}

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
					SelectedHeaderProfile shp = (SelectedHeaderProfile)selectedHeaderList[0];
					txtBasisHeader.Text = shp.HeaderID;
					//BEGIN MID Track #3219
					_headerRID = shp.Key;
					//END MID Track #3219
					GetPacksAndColors(shp.Key);
					cboBasisComponent.SelectedValue = (int) eRuleMethodComponentType.Total;
					SetIncludeCombo();	
// (CSMITH) - BEG MID Track #3219: PO / Master Release Enhancement
					// begin MID Track 3623 Port Master Allocation
					//this.cbxIsHeaderMaster.Enabled = true;
                    // BEGIN TT#2133-MD - AGallagher - Master option is no longer valid and should not be visable or selectable 
                    //if (SAB.ClientServerSession.GlobalOptions.AppConfig.MasterAllocationInstalled)
                    //{
                    //    this.cbxIsHeaderMaster.Enabled = true;
                    //}
                    //else
                    //{
                    // END TT#2133-MD - AGallagher - Master option is no longer valid and should not be visable or selectable 
						this.cbxIsHeaderMaster.Hide();
                    //}  // TT#2133-MD - AGallagher - Master option is no longer valid and should not be visable or selectable 
					// end MID Track 3623 Port Master Allocation
// (CSMITH) - END MID Track #3219
				}
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}

	
		private void cboRuleFilter_SelectionChangeCommitted(object sender, System.EventArgs e)
		{
			if (this.cboRuleFilter.SelectedValue != null)
			{
			}

            // Begin TT#2669 - JSmith - Unable to remove a filter from a wokflow
            if (cboRuleFilter.SelectedIndex != -1)
            {
                if (((FilterNameCombo)cboRuleFilter.SelectedItem).FilterRID == Include.Undefined)
                {
                    cboRuleFilter.SelectedIndex = -1;
                }
            }
            // End TT#2669 - JSmith - Unable to remove a filter from a wokflow
		}

        private void cboRuleFilter_DragEnter(object sender, System.Windows.Forms.DragEventArgs e)
        {
            Image_DragEnter(sender, e);
        }

        private void cboRuleFilter_DragDrop(object sender, System.Windows.Forms.DragEventArgs e)
        {
            //Begin Track #5858 - Kjohnson - Validating store security only
            try
            {
                bool isSuccessfull = ((MIDComboBoxTag)(((ComboBox)sender).Tag)).ComboBox_DragDrop(sender, e);

                if (isSuccessfull)
                {
                    ChangePending = true;
                    ((MIDComboBoxEnh)((ComboBox)sender).Parent).FirePropertyChangeEvent();
                }
            }
            catch (Exception exc)
            {
                HandleException(exc);
            }
            //End Track #5858
        }
		private void txtBasisHeader_Validating(object sender, System.ComponentModel.CancelEventArgs e)
		{
			Header header;
			try
			{
				ErrorProvider.SetError (txtBasisHeader,string.Empty);
				if (txtBasisHeader.Text.Trim() != string.Empty)
				{
					header = new Header();
					if (header.HeaderExists(txtBasisHeader.Text.Trim()))
					{
						_headerRID = header.GetHeaderRID(txtBasisHeader.Text.Trim());
						GetPacksAndColors(_headerRID);
						cboBasisComponent.SelectedValue = (int) eRuleMethodComponentType.Total;
					}
					else
					{
						ErrorProvider.SetError (txtBasisHeader,SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_HeaderNotFound));
					}
				}
				SetIncludeCombo();
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}
		private void GetPacksAndColors(int aHeaderRID)
		{
			Header header;
			try
			{
				header = new Header();
				_PackDataTable.Clear();
				_ColorDataTable.Clear();
				DataTable dtPacks = header.GetPacks(aHeaderRID);
				DataTable dtBulkColors = header.GetBulkColors(aHeaderRID);
				if (dtPacks.Rows.Count > 0)
				{	
					foreach (DataRow pRow in dtPacks.Rows)
					{
						_PackDataTable.Rows.Add( new object[] { (int) pRow["HDR_PACK_RID"], pRow["HDR_PACK_NAME"]} ) ;
					}
				}
				
				if (dtBulkColors.Rows.Count > 0)
				{	
					// BEGIN MID Track #3231 - get color description from hierarchy
					//foreach (DataRow cRow in dtBulkColors.Rows)
					//{
					//	int colorKey = Convert.ToInt32(cRow["COLOR_CODE_RID"],CultureInfo.CurrentUICulture);
					//	ColorCodeProfile ccp = SAB.HierarchyServerSession.GetColorCodeProfile(colorKey);
					//	_ColorDataTable.Rows.Add( new object[] {colorKey, ccp.ColorCodeName} ) ;
					//}

					DataTable dtHeader = header.GetHeader(aHeaderRID);
					if (dtHeader.Rows.Count == 0)
						return;
					DataRow row = dtHeader.Rows[0];
					int styleHnRID = Convert.ToInt32(row["STYLE_HNRID"],CultureInfo.CurrentUICulture);
					HierarchyNodeProfile hnp_style = SAB.HierarchyServerSession.GetNodeData(styleHnRID);
					
					foreach (DataRow cRow in dtBulkColors.Rows)
					{
						string colorDescription;
						int colorKey = Convert.ToInt32(cRow["COLOR_CODE_RID"],CultureInfo.CurrentUICulture);
					
                        // Assortment BEGIN
                        int hdrBCRID = Convert.ToInt32(cRow["HDR_BC_RID"], CultureInfo.CurrentUICulture);
                        // Assortment END

						ColorCodeProfile ccp = SAB.HierarchyServerSession.GetColorCodeProfile(colorKey);
						int colorHnRID = Include.NoRID;
						if(SAB.HierarchyServerSession.ColorExistsForStyle(hnp_style.HomeHierarchyRID, hnp_style.Key, ccp.ColorCodeID, ref colorHnRID))
						{
							HierarchyNodeProfile hnp_color = SAB.HierarchyServerSession.GetNodeData(colorHnRID);
							colorDescription = hnp_color.NodeDescription;
						}
						else
						{
							colorDescription = ccp.ColorCodeName;
						}
                        // Assortment BEGIN = add hdrBCRID
                        _ColorDataTable.Rows.Add(new object[] { colorKey, colorDescription, hdrBCRID });
                        // Assortment BEGIN
					}
					// END MID Track #3231 
				}
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
			 
		}

		private void SetIncludeCombo()		
		{
			string filterString;
			int numRule;
			bool resetRule = false;
			try
			{

				if (txtBasisHeader.Text.Trim() == string.Empty)
				{
					if (   (int)cboInclRule.SelectedValue == (int)eRuleMethod.Exact
						|| (int)cboInclRule.SelectedValue == (int)eRuleMethod.Fill
						|| (int)cboInclRule.SelectedValue == (int)eRuleMethod.Proportional )
						resetRule = true;
						
					numRule = (int)eRuleMethod.Exact;
					filterString = "TEXT_CODE < " + Convert.ToString(numRule,CultureInfo.CurrentUICulture);
					_IncludeRuleDataTable.DefaultView.RowFilter = filterString;
					if (resetRule)
						cboInclRule.SelectedValue = (int)eRuleMethod.None;
				}
				else
				{
					// Since we are going to change the row filter, we need to first save the current selection so 
					// we can reset it.
					object selectedValue = cboInclRule.SelectedValue;
					_IncludeRuleDataTable.DefaultView.RowFilter = string.Empty;
					cboInclRule.SelectedValue = selectedValue;
				}
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}

        // Begin Track #4872 - JSmith - Global/User Attributes
        override protected void BuildAttributeList()
        {
            ProfileList al;
            int currValue;
            try
            {
                if (cboStoreAttribute.SelectedValue != null &&
                    cboStoreAttribute.SelectedValue.GetType() == typeof(System.Int32))
                {
                    currValue = Convert.ToInt32(cboStoreAttribute.SelectedValue);
                }
                else
                {
                    currValue = Include.NoRID;
                }
                al = GetStoreGroupList(_ruleMethod.Method_Change_Type, _ruleMethod.GlobalUserType, true);
                cboStoreAttribute.Initialize(SAB, FunctionSecurity, al.ArrayList, _ruleMethod.GlobalUserType == eGlobalUserType.User);
                if (currValue != Include.NoRID)
                {
                    cboStoreAttribute.SelectedValue = currValue;
                }

                AdjustTextWidthComboBox_DropDown(cboStoreAttribute);     //TT#7 - RBeck - Dynamic dropdowns
            }
            catch
            {
                throw;
            }
        }
        // End Track #4872

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
			return securityOk; // Track 5871 stodd
		}

// (CSMITH) - BEG MID Track #3219: PO / Master Release Enhancement
		private void cbxIsHeaderMaster_CheckedChanged(object sender, System.EventArgs e)
		{
			if (this.cbxIsHeaderMaster.Checked)
			{
				AllocationProfile ap = new AllocationProfile(SAB, this.txtBasisHeader.Text.Trim(), this._headerRID, SAB.ClientServerSession);

				if (!ap.AllocationStarted)
				{
					string msgText = MIDText.GetTextOnly(eMIDTextCode.msg_al_MasterAlocNotStarted);
					msgText = msgText.Replace("{0}", this.txtBasisHeader.Text.Trim());

					MessageBox.Show(msgText);

					return;
				}
			}
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

        // (CSMITH) - END MID Track #3219

        // Begin Track #4872 - JSmith - Global/User Attributes
        private void frmRuleMethod_Load(object sender, EventArgs e)
        {
            if (cboStoreAttribute.ReplaceAttribute)
            {
                ChangePending = true;
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

        private void cboRuleFilter_DragOver(object sender, DragEventArgs e)
        {
            Image_DragOver(sender, e);
        }
        // End Track #4872
       
        // Begin TT#316-MD - RMatelic - Replace all Windows Combobox controls with new enhanced control 
        void cboStoreAttribute_MIDComboBoxPropertiesChangedEvent(object source, MIDComboBoxPropertiesChangedEventArgs args)
        {
            this.cboStoreAttribute_SelectionChangeCommitted(source, new EventArgs());
        }

        void cboAttributeSet_MIDComboBoxPropertiesChangedEvent(object source, MIDComboBoxPropertiesChangedEventArgs args)
        {
            this.cboAttributeSet_SelectionChangeCommitted(source, new EventArgs());
        }
        void cboRuleFilter_MIDComboBoxPropertiesChangedEvent(object source, MIDComboBoxPropertiesChangedEventArgs args)
        {
            this.cboRuleFilter_SelectionChangeCommitted(source, new EventArgs());
        }
        void cboExclRule_MIDComboBoxPropertiesChangedEvent(object source, MIDComboBoxPropertiesChangedEventArgs args)
        {
            this.cboExclRule_SelectionChangeCommitted(source, new EventArgs());
        }
        void cboInclRule_MIDComboBoxPropertiesChangedEvent(object source, MIDComboBoxPropertiesChangedEventArgs args)
        {
            this.cboInclRule_SelectionChangeCommitted(source, new EventArgs());
        }

        //BEGIN TT#1608-MD - srisch - Working 5.4 - Prior Header 05/27/2015 
        private void cbxIncludeReserve_CheckedChanged(object sender, EventArgs e)
        {
            _ruleMethod.IncludeReserve = cbxIncludeReserve.Checked; 
        }
        //END TT#1608-MD - srisch - Working 5.4 - Prior Header 05/27/2015 
        // End TT#316

	}

}
