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
    public class frmGlobalUnlockMethod : MIDRetail.Windows.WorkflowMethodFormBase
    {
        #region Fields

        private Bitmap _picInclude;
        private Bitmap _picExclude;
        private Image _dynamicToPlanImage;
        private Image _dynamicToCurrentImage;
        //private ProfileList _versionProfList;
        private MIDRetail.Business.OTSGlobalUnlockMethod _OTSGlobalUnlockMethod = null;
        private MIDRetail.Business.OTSGlobalLockMethod _OTSGlobalLockMethod = null; //TT#43 - MD - DOConnell - Projected Sales Enhancement
        private HierarchyNodeSecurityProfile _hierNodeSecurity;
        private FunctionSecurityProfile _filterUserSecurity;
        private FunctionSecurityProfile _filterGlobalSecurity; 
        private ArrayList _userRIDList;
        //private StoreFilterData _storeFilterDL;
        private FilterData _storeFilterDL;
        private ProfileList _variables;
        private int _nodeRID = Include.NoRID;
        private bool _attributeReset = false;
        private string _thisTitle;
        private bool _needsValidated = false;
        // Begin MID Track 5862 - KJohnson - On an New Method, Attribute Set selection is checked even when Stores are not selected
        private bool _newWorkFlow = false;
        // End MID Track 5862
        // Begin MID Track 5855 - KJohnson - Version list includes all version, should not include View only
        private bool _updatingBindVersionComboBoxes = false;
        private bool _setting_cboSpreadVersion = false;
        // End MID Track 5855
        //BEGIN TT#43 - MD - DOConnell - Projected Sales Enhancement
        private eMethodType aMethodType;
        private MethodBaseData _methodData;
        //END TT#43 - MD - DOConnell - Projected Sales Enhancement
        #endregion

        private System.Windows.Forms.TabPage tabMethod;
        private System.Windows.Forms.Label lblTimePeriod;
        private System.Windows.Forms.TabPage tabProperties;
        private System.Windows.Forms.ContextMenu menuListBox;
        private System.Windows.Forms.MenuItem mniRestoreDefaults;
        private System.Windows.Forms.MenuItem mniSelectAll;
        private System.Windows.Forms.MenuItem mniClearAll;
        private System.Windows.Forms.TabControl tabSpreadMethod;
        private System.Windows.Forms.GroupBox grpUnlock;
        private System.Windows.Forms.Label lblVersion;
        private System.Windows.Forms.Label lblMerchandise;
        private System.Windows.Forms.TextBox txtSpreadNode;
        private MIDRetail.Windows.Controls.MIDDateRangeSelector mdsPlanDateRange;
        private System.Windows.Forms.GroupBox grpOptions;
        private System.Windows.Forms.ContextMenu mnuBasisGrid;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ImageList Icons;
        private int _parentVersion;
        private GroupBox grpLastProcessed;
        private GroupBox grpStoreOptions;
        private Label lblFilter;
        // Begin Track #4872 - JSmith - Global/User Attributes
        //private MIDRetail.Windows.Controls.MIDComboBoxEnh cboAttribute;
        private MIDAttributeComboBox cboAttribute;
        // End Track #4872
        private Label lblAttribute;
        private Label lblAttributeSet;
        private CheckBox cbStores;
        private CheckBox cbChain;
        private CheckBox cbMultiLevel;
        private Label lblFromLevel;
        private Label lblToLevel;
        private Button btnOverride;
        private CheckedListBox chkLstBxAttributeSet;
        private Label lblLastProcessed;
        private Label lblByUserValue;
        private Label lblByUser;
        private Label lblDateTimeValue;
        private Label lblDateTime;
        private UltraGrid ugWorkflows;
        private bool cbStoresCheckedChanging;
        private MIDComboBoxEnh cboFromLevel;
        private MIDComboBoxEnh cboFilter;
        private MIDComboBoxEnh cboSpreadVersion;
        private MIDComboBoxEnh cboOverride;
        private MIDComboBoxEnh cboToLevel;
        private Panel OptionsPanel;


        private System.ComponentModel.IContainer components = null;
		//Begin TT#2576 - DOConnell - Global Lock Create - Title bar name changes
		//public frmGlobalUnlockMethod(SessionAddressBlock SAB, ExplorerAddressBlock aEAB)
        public frmGlobalUnlockMethod(SessionAddressBlock SAB, ExplorerAddressBlock aEAB, eMethodType aMethodType)
            : base(SAB, aEAB, eMIDTextCode.frm_GlobalUnlock, eWorkflowMethodType.Method)
        {
            try
            {
                if (aMethodType == eMethodType.GlobalLock)
                {
                    FormName = eMIDTextCode.frm_GlobalLock;
                }
		//END TT#2576 - DOConnell - Global Lock Create - Title bar name changes		
                InitializeComponent();

                UserSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ForecastMethodsUserGlobalUnlock);
                GlobalSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ForecastMethodsGlobalGlobalUnlock);

            }
            catch (Exception ex)
            {
                HandleException(ex, "NewOTSGlobalUnlockMethod");
                FormLoadError = true;
            }
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
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
                this.cboAttribute.SelectionChangeCommitted -= new System.EventHandler(this.cboAttribute_SelectionChangeCommitted);
                this.cboFilter.SelectionChangeCommitted -= new System.EventHandler(this.cboFilter_SelectionChangeCommitted);
                //Begin TT#316 -jsobek -Replace all Windows Combobox controls with new enhanced control
                this.cboFilter.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler(cboFilter_MIDComboBoxPropertiesChangedEvent);
                this.cboAttribute.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler(cboAttribute_MIDComboBoxPropertiesChangedEvent);
                this.cboSpreadVersion.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler(cboSpreadVersion_MIDComboBoxPropertiesChangedEvent);
                this.cboOverride.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler(cboOverride_MIDComboBoxPropertiesChangedEvent);
                this.cboToLevel.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler(cboToLevel_MIDComboBoxPropertiesChangedEvent);
                this.cboFromLevel.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler(cboFromLevel_MIDComboBoxPropertiesChangedEvent);
                //End TT#316
                this.cboFilter.DragOver -= new System.Windows.Forms.DragEventHandler(this.cboFilter_DragOver);
                this.cboFilter.DragDrop -= new System.Windows.Forms.DragEventHandler(this.cboFilter_DragDrop);
                this.cboFilter.DragEnter -= new System.Windows.Forms.DragEventHandler(this.cboFilter_DragEnter);
                this.mdsPlanDateRange.Click -= new System.EventHandler(this.mdsPlanDateRange_Click);
                this.mdsPlanDateRange.OnSelection -= new MIDRetail.Windows.Controls.MIDDateRangeSelector.SelectionEventHandler(this.mdsPlanDateRange_OnSelection);
                this.mniRestoreDefaults.Click -= new System.EventHandler(this.mniRestoreDefaults_Click);
                this.mniSelectAll.Click -= new System.EventHandler(this.mniSelectAll_Click);
                this.mniClearAll.Click -= new System.EventHandler(this.mniClearAll_Click);
                this.cbChain.CheckedChanged -= new System.EventHandler(this.cbChain_CheckedChanged);
                this.cbStores.CheckedChanged -= new System.EventHandler(this.cbStores_CheckedChanged);
                this.cbMultiLevel.CheckedChanged -= new System.EventHandler(this.cbMultiLevel_CheckedChanged);
                this.btnOverride.Click -= new System.EventHandler(this.btnOverride_Click);
                this.chkLstBxAttributeSet.ItemCheck -= new System.Windows.Forms.ItemCheckEventHandler(this.chkLstBxAttributeSet_ItemCheck);
                this.txtSpreadNode.DragOver -= new System.Windows.Forms.DragEventHandler(this.txtSpreadNode_DragOver);
            }
            base.Dispose(disposing);
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
            this.tabSpreadMethod = new System.Windows.Forms.TabControl();
            this.tabMethod = new System.Windows.Forms.TabPage();
            this.grpLastProcessed = new System.Windows.Forms.GroupBox();
            this.lblLastProcessed = new System.Windows.Forms.Label();
            this.lblByUserValue = new System.Windows.Forms.Label();
            this.lblByUser = new System.Windows.Forms.Label();
            this.lblDateTimeValue = new System.Windows.Forms.Label();
            this.lblDateTime = new System.Windows.Forms.Label();
            this.grpStoreOptions = new System.Windows.Forms.GroupBox();
            this.cboFilter = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this.chkLstBxAttributeSet = new System.Windows.Forms.CheckedListBox();
            this.menuListBox = new System.Windows.Forms.ContextMenu();
            this.mniRestoreDefaults = new System.Windows.Forms.MenuItem();
            this.mniSelectAll = new System.Windows.Forms.MenuItem();
            this.mniClearAll = new System.Windows.Forms.MenuItem();
            this.lblAttributeSet = new System.Windows.Forms.Label();
            this.lblFilter = new System.Windows.Forms.Label();
            this.cboAttribute = new MIDRetail.Windows.Controls.MIDAttributeComboBox();
            this.lblAttribute = new System.Windows.Forms.Label();
            this.grpUnlock = new System.Windows.Forms.GroupBox();
            this.cboSpreadVersion = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this.cboOverride = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this.cboToLevel = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this.cboFromLevel = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this.cbMultiLevel = new System.Windows.Forms.CheckBox();
            this.btnOverride = new System.Windows.Forms.Button();
            this.lblFromLevel = new System.Windows.Forms.Label();
            this.lblToLevel = new System.Windows.Forms.Label();
            this.mdsPlanDateRange = new MIDRetail.Windows.Controls.MIDDateRangeSelector();
            this.lblMerchandise = new System.Windows.Forms.Label();
            this.txtSpreadNode = new System.Windows.Forms.TextBox();
            this.lblVersion = new System.Windows.Forms.Label();
            this.lblTimePeriod = new System.Windows.Forms.Label();
            this.grpOptions = new System.Windows.Forms.GroupBox();
            this.OptionsPanel = new System.Windows.Forms.Panel();
            this.cbStores = new System.Windows.Forms.CheckBox();
            this.cbChain = new System.Windows.Forms.CheckBox();
            this.tabProperties = new System.Windows.Forms.TabPage();
            this.ugWorkflows = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.mnuBasisGrid = new System.Windows.Forms.ContextMenu();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.Icons = new System.Windows.Forms.ImageList(this.components);
            this.pnlGlobalUser.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.utmMain)).BeginInit();
            this.tabSpreadMethod.SuspendLayout();
            this.tabMethod.SuspendLayout();
            this.grpLastProcessed.SuspendLayout();
            this.grpStoreOptions.SuspendLayout();
            this.grpUnlock.SuspendLayout();
            this.grpOptions.SuspendLayout();
            this.OptionsPanel.SuspendLayout();
            this.tabProperties.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ugWorkflows)).BeginInit();
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
            // 
            // tabMethod
            // 
            this.tabMethod.Controls.Add(this.grpLastProcessed);
            this.tabMethod.Controls.Add(this.grpStoreOptions);
            this.tabMethod.Controls.Add(this.grpUnlock);
            this.tabMethod.Controls.Add(this.grpOptions);
            this.tabMethod.Location = new System.Drawing.Point(4, 22);
            this.tabMethod.Name = "tabMethod";
            this.tabMethod.Size = new System.Drawing.Size(680, 448);
            this.tabMethod.TabIndex = 1;
            this.tabMethod.Text = "Method";
            // 
            // grpLastProcessed
            // 
            this.grpLastProcessed.Controls.Add(this.lblLastProcessed);
            this.grpLastProcessed.Controls.Add(this.lblByUserValue);
            this.grpLastProcessed.Controls.Add(this.lblByUser);
            this.grpLastProcessed.Controls.Add(this.lblDateTimeValue);
            this.grpLastProcessed.Controls.Add(this.lblDateTime);
            this.grpLastProcessed.Location = new System.Drawing.Point(8, 304);
            this.grpLastProcessed.Name = "grpLastProcessed";
            this.grpLastProcessed.Size = new System.Drawing.Size(664, 131);
            this.grpLastProcessed.TabIndex = 13;
            this.grpLastProcessed.TabStop = false;
            // 
            // lblLastProcessed
            // 
            this.lblLastProcessed.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLastProcessed.Location = new System.Drawing.Point(36, 80);
            this.lblLastProcessed.Name = "lblLastProcessed";
            this.lblLastProcessed.Size = new System.Drawing.Size(113, 16);
            this.lblLastProcessed.TabIndex = 17;
            this.lblLastProcessed.Text = "Last Processed:";
            // 
            // lblByUserValue
            // 
            this.lblByUserValue.Location = new System.Drawing.Point(397, 98);
            this.lblByUserValue.Name = "lblByUserValue";
            this.lblByUserValue.Size = new System.Drawing.Size(197, 15);
            this.lblByUserValue.TabIndex = 16;
            this.lblByUserValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblByUser
            // 
            this.lblByUser.Location = new System.Drawing.Point(341, 98);
            this.lblByUser.Name = "lblByUser";
            this.lblByUser.Size = new System.Drawing.Size(50, 15);
            this.lblByUser.TabIndex = 15;
            this.lblByUser.Text = "By User:";
            this.lblByUser.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblDateTimeValue
            // 
            this.lblDateTimeValue.Location = new System.Drawing.Point(104, 98);
            this.lblDateTimeValue.Name = "lblDateTimeValue";
            this.lblDateTimeValue.Size = new System.Drawing.Size(197, 15);
            this.lblDateTimeValue.TabIndex = 14;
            this.lblDateTimeValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblDateTime
            // 
            this.lblDateTime.Location = new System.Drawing.Point(36, 98);
            this.lblDateTime.Name = "lblDateTime";
            this.lblDateTime.Size = new System.Drawing.Size(62, 15);
            this.lblDateTime.TabIndex = 13;
            this.lblDateTime.Text = "Date/Time:";
            this.lblDateTime.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // grpStoreOptions
            // 
            this.grpStoreOptions.Controls.Add(this.cboFilter);
            this.grpStoreOptions.Controls.Add(this.chkLstBxAttributeSet);
            this.grpStoreOptions.Controls.Add(this.lblAttributeSet);
            this.grpStoreOptions.Controls.Add(this.lblFilter);
            this.grpStoreOptions.Controls.Add(this.cboAttribute);
            this.grpStoreOptions.Controls.Add(this.lblAttribute);
            this.grpStoreOptions.Location = new System.Drawing.Point(8, 203);
            this.grpStoreOptions.Name = "grpStoreOptions";
            this.grpStoreOptions.Size = new System.Drawing.Size(664, 96);
            this.grpStoreOptions.TabIndex = 12;
            this.grpStoreOptions.TabStop = false;
            this.grpStoreOptions.Text = "Store Options";
            // 
            // cboFilter
            // 
            this.cboFilter.AllowDrop = true;
            this.cboFilter.AutoAdjust = true;
            this.cboFilter.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboFilter.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboFilter.DataSource = null;
            this.cboFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboFilter.DropDownWidth = 224;
            this.cboFilter.FormattingEnabled = false;
            this.cboFilter.Location = new System.Drawing.Point(68, 60);
            this.cboFilter.Margin = new System.Windows.Forms.Padding(0);
            this.cboFilter.Name = "cboFilter";
            this.cboFilter.Size = new System.Drawing.Size(224, 21);
            this.cboFilter.TabIndex = 10;
            this.cboFilter.Tag = null;
            this.cboFilter.SelectionChangeCommitted += new System.EventHandler(this.cboFilter_SelectionChangeCommitted);
            this.cboFilter.MIDComboBoxPropertiesChangedEvent += new MIDComboBoxPropertiesChangedEventHandler(cboFilter_MIDComboBoxPropertiesChangedEvent);
            this.cboFilter.DragDrop += new System.Windows.Forms.DragEventHandler(this.cboFilter_DragDrop);
            this.cboFilter.DragEnter += new System.Windows.Forms.DragEventHandler(this.cboFilter_DragEnter);
            this.cboFilter.DragOver += new System.Windows.Forms.DragEventHandler(this.cboFilter_DragOver);
            // 
            // chkLstBxAttributeSet
            // 
            this.chkLstBxAttributeSet.CheckOnClick = true;
            this.chkLstBxAttributeSet.ContextMenu = this.menuListBox;
            this.chkLstBxAttributeSet.FormattingEnabled = true;
            this.chkLstBxAttributeSet.Location = new System.Drawing.Point(389, 19);
            this.chkLstBxAttributeSet.Name = "chkLstBxAttributeSet";
            this.chkLstBxAttributeSet.Size = new System.Drawing.Size(263, 64);
            this.chkLstBxAttributeSet.TabIndex = 13;
            this.chkLstBxAttributeSet.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.chkLstBxAttributeSet_ItemCheck);
            // 
            // menuListBox
            // 
            this.menuListBox.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mniRestoreDefaults,
            this.mniSelectAll,
            this.mniClearAll});
            // 
            // mniRestoreDefaults
            // 
            this.mniRestoreDefaults.Index = 0;
            this.mniRestoreDefaults.Text = "Restore Defaults";
            this.mniRestoreDefaults.Click += new System.EventHandler(this.mniRestoreDefaults_Click);
            // 
            // mniSelectAll
            // 
            this.mniSelectAll.Index = 1;
            this.mniSelectAll.Text = "Select All";
            this.mniSelectAll.Click += new System.EventHandler(this.mniSelectAll_Click);
            // 
            // mniClearAll
            // 
            this.mniClearAll.Index = 2;
            this.mniClearAll.Text = "Clear All";
            this.mniClearAll.Click += new System.EventHandler(this.mniClearAll_Click);
            // 
            // lblAttributeSet
            // 
            this.lblAttributeSet.Location = new System.Drawing.Point(315, 25);
            this.lblAttributeSet.Name = "lblAttributeSet";
            this.lblAttributeSet.Size = new System.Drawing.Size(72, 16);
            this.lblAttributeSet.TabIndex = 11;
            this.lblAttributeSet.Text = "Attribute Set:";
            // 
            // lblFilter
            // 
            this.lblFilter.Location = new System.Drawing.Point(12, 63);
            this.lblFilter.Name = "lblFilter";
            this.lblFilter.Size = new System.Drawing.Size(52, 16);
            this.lblFilter.TabIndex = 9;
            this.lblFilter.Text = "Filter:";
            // 
            // cboAttribute
            // 
            this.cboAttribute.AllowDrop = true;
            this.cboAttribute.AllowUserAttributes = false;
            this.cboAttribute.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboAttribute.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboAttribute.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboAttribute.Location = new System.Drawing.Point(68, 21);
            this.cboAttribute.Name = "cboAttribute";
            this.cboAttribute.Size = new System.Drawing.Size(224, 21);
            this.cboAttribute.TabIndex = 8;
            this.cboAttribute.SelectionChangeCommitted += new System.EventHandler(this.cboAttribute_SelectionChangeCommitted);
            this.cboAttribute.MIDComboBoxPropertiesChangedEvent += new MIDComboBoxPropertiesChangedEventHandler(cboAttribute_MIDComboBoxPropertiesChangedEvent);
            this.cboAttribute.DragEnter += new System.Windows.Forms.DragEventHandler(this.cboAttribute_DragEnter);
            this.cboAttribute.DragOver += new System.Windows.Forms.DragEventHandler(this.cboAttribute_DragOver);
            // 
            // lblAttribute
            // 
            this.lblAttribute.Location = new System.Drawing.Point(11, 24);
            this.lblAttribute.Name = "lblAttribute";
            this.lblAttribute.Size = new System.Drawing.Size(56, 16);
            this.lblAttribute.TabIndex = 3;
            this.lblAttribute.Text = "Attribute:";
            // 
            // grpUnlock
            // 
            this.grpUnlock.Controls.Add(this.cboSpreadVersion);
            this.grpUnlock.Controls.Add(this.cboOverride);
            this.grpUnlock.Controls.Add(this.cboToLevel);
            this.grpUnlock.Controls.Add(this.cboFromLevel);
            this.grpUnlock.Controls.Add(this.cbMultiLevel);
            this.grpUnlock.Controls.Add(this.btnOverride);
            this.grpUnlock.Controls.Add(this.lblFromLevel);
            this.grpUnlock.Controls.Add(this.lblToLevel);
            this.grpUnlock.Controls.Add(this.mdsPlanDateRange);
            this.grpUnlock.Controls.Add(this.lblMerchandise);
            this.grpUnlock.Controls.Add(this.txtSpreadNode);
            this.grpUnlock.Controls.Add(this.lblVersion);
            this.grpUnlock.Controls.Add(this.lblTimePeriod);
            this.grpUnlock.Location = new System.Drawing.Point(8, 8);
            this.grpUnlock.Name = "grpUnlock";
            this.grpUnlock.Size = new System.Drawing.Size(664, 130);
            this.grpUnlock.TabIndex = 4;
            this.grpUnlock.TabStop = false;
            this.grpUnlock.Text = "Unlock";
            // 
            // cboSpreadVersion
            // 
            this.cboSpreadVersion.AutoAdjust = true;
            this.cboSpreadVersion.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboSpreadVersion.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboSpreadVersion.DataSource = null;
            this.cboSpreadVersion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSpreadVersion.DropDownWidth = 224;
            this.cboSpreadVersion.FormattingEnabled = false;
            this.cboSpreadVersion.Location = new System.Drawing.Point(97, 61);
            this.cboSpreadVersion.Margin = new System.Windows.Forms.Padding(0);
            this.cboSpreadVersion.Name = "cboSpreadVersion";
            this.cboSpreadVersion.Size = new System.Drawing.Size(224, 21);
            this.cboSpreadVersion.TabIndex = 7;
            this.cboSpreadVersion.Tag = null;
            this.cboSpreadVersion.SelectionChangeCommitted += new System.EventHandler(this.cboSpreadVersion_SelectionChangeCommitted);
            this.cboSpreadVersion.MIDComboBoxPropertiesChangedEvent += new MIDComboBoxPropertiesChangedEventHandler(cboSpreadVersion_MIDComboBoxPropertiesChangedEvent);
            // 
            // cboOverride
            // 
            this.cboOverride.AutoAdjust = true;
            this.cboOverride.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboOverride.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboOverride.DataSource = null;
            this.cboOverride.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboOverride.DropDownWidth = 174;
            this.cboOverride.FormattingEnabled = false;
            this.cboOverride.Location = new System.Drawing.Point(476, 95);
            this.cboOverride.Margin = new System.Windows.Forms.Padding(0);
            this.cboOverride.Name = "cboOverride";
            this.cboOverride.Size = new System.Drawing.Size(174, 21);
            this.cboOverride.TabIndex = 33;
            this.cboOverride.Tag = null;
            this.cboOverride.SelectionChangeCommitted += new System.EventHandler(this.cboOverride_SelectionChangeCommitted);
            this.cboOverride.MIDComboBoxPropertiesChangedEvent += new MIDComboBoxPropertiesChangedEventHandler(cboOverride_MIDComboBoxPropertiesChangedEvent);
            // 
            // cboToLevel
            // 
            this.cboToLevel.AutoAdjust = true;
            this.cboToLevel.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboToLevel.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboToLevel.DataSource = null;
            this.cboToLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboToLevel.DropDownWidth = 223;
            this.cboToLevel.FormattingEnabled = false;
            this.cboToLevel.Location = new System.Drawing.Point(427, 61);
            this.cboToLevel.Margin = new System.Windows.Forms.Padding(0);
            this.cboToLevel.Name = "cboToLevel";
            this.cboToLevel.Size = new System.Drawing.Size(223, 21);
            this.cboToLevel.TabIndex = 35;
            this.cboToLevel.Tag = null;
            this.cboToLevel.SelectionChangeCommitted += new System.EventHandler(this.cboToLevel_SelectionChangeCommitted);
            this.cboToLevel.MIDComboBoxPropertiesChangedEvent += new MIDComboBoxPropertiesChangedEventHandler(cboToLevel_MIDComboBoxPropertiesChangedEvent);
            // 
            // cboFromLevel
            // 
            this.cboFromLevel.AutoAdjust = true;
            this.cboFromLevel.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboFromLevel.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboFromLevel.DataSource = null;
            this.cboFromLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboFromLevel.DropDownWidth = 223;
            this.cboFromLevel.FormattingEnabled = false;
            this.cboFromLevel.Location = new System.Drawing.Point(427, 29);
            this.cboFromLevel.Margin = new System.Windows.Forms.Padding(0);
            this.cboFromLevel.Name = "cboFromLevel";
            this.cboFromLevel.Size = new System.Drawing.Size(223, 21);
            this.cboFromLevel.TabIndex = 34;
            this.cboFromLevel.Tag = null;
            this.cboFromLevel.SelectionChangeCommitted += new System.EventHandler(this.cboFromLevel_SelectionChangeCommitted);
            this.cboFromLevel.MIDComboBoxPropertiesChangedEvent += new MIDComboBoxPropertiesChangedEventHandler(cboFromLevel_MIDComboBoxPropertiesChangedEvent);
            // 
            // cbMultiLevel
            // 
            this.cbMultiLevel.Location = new System.Drawing.Point(350, 13);
            this.cbMultiLevel.Name = "cbMultiLevel";
            this.cbMultiLevel.Size = new System.Drawing.Size(95, 15);
            this.cbMultiLevel.TabIndex = 28;
            this.cbMultiLevel.Text = "Multi Level";
            this.cbMultiLevel.CheckedChanged += new System.EventHandler(this.cbMultiLevel_CheckedChanged);
            // 
            // btnOverride
            // 
            this.btnOverride.Location = new System.Drawing.Point(352, 95);
            this.btnOverride.Name = "btnOverride";
            this.btnOverride.Size = new System.Drawing.Size(118, 23);
            this.btnOverride.TabIndex = 32;
            this.btnOverride.Text = "Override";
            this.btnOverride.Click += new System.EventHandler(this.btnOverride_Click);
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
            this.lblToLevel.Size = new System.Drawing.Size(67, 16);
            this.lblToLevel.TabIndex = 26;
            this.lblToLevel.Text = "To Level:";
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
            this.lblMerchandise.Text = "Merchandise:";
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
            // grpOptions
            // 
            this.grpOptions.Controls.Add(this.OptionsPanel);
            this.grpOptions.Location = new System.Drawing.Point(8, 144);
            this.grpOptions.Name = "grpOptions";
            this.grpOptions.Size = new System.Drawing.Size(664, 53);
            this.grpOptions.TabIndex = 11;
            this.grpOptions.TabStop = false;
            this.grpOptions.Text = "Options";
            // 
            // OptionsPanel
            // 
            this.OptionsPanel.Controls.Add(this.cbStores);
            this.OptionsPanel.Controls.Add(this.cbChain);
            this.OptionsPanel.Location = new System.Drawing.Point(6, 13);
            this.OptionsPanel.Name = "OptionsPanel";
            this.OptionsPanel.Size = new System.Drawing.Size(155, 34);
            this.OptionsPanel.TabIndex = 25;
            // 
            // cbStores
            // 
            this.cbStores.Location = new System.Drawing.Point(3, 3);
            this.cbStores.Name = "cbStores";
            this.cbStores.Size = new System.Drawing.Size(56, 25);
            this.cbStores.TabIndex = 24;
            this.cbStores.Text = "Stores";
            this.cbStores.CheckedChanged += new System.EventHandler(this.cbStores_CheckedChanged);
            // 
            // cbChain
            // 
            this.cbChain.Location = new System.Drawing.Point(99, 3);
            this.cbChain.Name = "cbChain";
            this.cbChain.Size = new System.Drawing.Size(53, 24);
            this.cbChain.TabIndex = 25;
            this.cbChain.Text = "Chain";
            this.cbChain.CheckedChanged += new System.EventHandler(this.cbChain_CheckedChanged);
            // 
            // tabProperties
            // 
            this.tabProperties.Controls.Add(this.ugWorkflows);
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
            this.ugWorkflows.Location = new System.Drawing.Point(32, 9);
            this.ugWorkflows.Name = "ugWorkflows";
            this.ugWorkflows.Size = new System.Drawing.Size(616, 430);
            this.ugWorkflows.TabIndex = 3;
            // 
            // Icons
            // 
            this.Icons.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.Icons.ImageSize = new System.Drawing.Size(16, 16);
            this.Icons.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // frmGlobalUnlockMethod
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(718, 572);
            this.Controls.Add(this.tabSpreadMethod);
            this.Name = "frmGlobalUnlockMethod";
            this.Text = "Unlock";
            this.Load += new System.EventHandler(this.frmGlobalUnlockMethod_Load);
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
            this.grpLastProcessed.ResumeLayout(false);
            this.grpStoreOptions.ResumeLayout(false);
            this.grpUnlock.ResumeLayout(false);
            this.grpUnlock.PerformLayout();
            this.grpOptions.ResumeLayout(false);
            this.OptionsPanel.ResumeLayout(false);
            this.tabProperties.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ugWorkflows)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        /// <summary>
        /// Create a new Global Unlock Method
        /// </summary>
        override public void NewWorkflowMethod(MIDWorkflowMethodTreeNode aParentNode)
        {
            try
            {
			//Begin TT#43 - MD - DOConnell - Projected Sales Enhancement
                if (aParentNode.MethodType == eMethodTypeUI.GlobalUnlock)
                {
                    _OTSGlobalUnlockMethod = new OTSGlobalUnlockMethod(SAB, Include.NoRID);
                    ABM = _OTSGlobalUnlockMethod;
                    base.NewWorkflowMethod(aParentNode, eSecurityFunctions.ForecastMethodsUserGlobalUnlock, eSecurityFunctions.ForecastMethodsGlobalGlobalUnlock);

                    _parentVersion = Include.NoRID;  // Issue 3801 - stodd

                    Common_Load();

                    // Begin MID Track 5862 - KJohnson - On an New Method, Attribute Set selection is checked even when Stores are not selected
                    _newWorkFlow = true;
                    // End MID Track 5862 
                }
                else
                {
                    
                    _OTSGlobalLockMethod = new OTSGlobalLockMethod(SAB, Include.NoRID);
                    ABM = _OTSGlobalLockMethod;
                    base.NewWorkflowMethod(aParentNode, eSecurityFunctions.ForecastMethodsUserGlobalLock, eSecurityFunctions.ForecastMethodsGlobalGlobalLock);

                    _parentVersion = Include.NoRID;  // Issue 3801 - stodd

                    Common_Load();

                    // Begin MID Track 5862 - KJohnson - On an New Method, Attribute Set selection is checked even when Stores are not selected
                    _newWorkFlow = true;
                    // End MID Track 5862
                }
			//End TT#43 - MD - DOConnell - Projected Sales Enhancement
            }
            catch (Exception ex)
            {
                HandleException(ex, "NewOTSGlobalUnlockMethod");
                FormLoadError = true;
            }
        }

        /// <summary>
        /// Opens an existing Matrix Method. //Eventually combine with NewOTSForecastCopyMethod method
        /// 		/// Seperate for debugging & initial development
        /// </summary>
        /// <param name="aMethodRID">method_RID</param>
        /// <param name="aLockStatus">The lock status of the data to be displayed</param>
        override public void UpdateWorkflowMethod(int aMethodRID, int aNodeRID, MIDWorkflowMethodTreeNode aNode, eLockStatus aLockStatus)
        {
            try
            {
			//Begin TT#43 - MD - DOConnell - Projected Sales Enhancement
                if (aNode.MethodType == eMethodTypeUI.GlobalUnlock)
                {
                    _OTSGlobalUnlockMethod = new OTSGlobalUnlockMethod(SAB, aMethodRID);
                    base.UpdateWorkflowMethod(aLockStatus, eSecurityFunctions.ForecastMethodsUserGlobalUnlock, eSecurityFunctions.ForecastMethodsGlobalGlobalUnlock);

                    _parentVersion = _OTSGlobalUnlockMethod.VersionRID;  // Issue 4008 - stodd

                    Common_Load();

                    // Begin MID Track 5862 - KJohnson - On an New Method, Attribute Set selection is checked even when Stores are not selected
                    _newWorkFlow = false;
                    // End MID Track 5862 
                }
                else
                {
                    _OTSGlobalLockMethod = new OTSGlobalLockMethod(SAB, aMethodRID);
                    ABM = _OTSGlobalLockMethod;
                    base.UpdateWorkflowMethod(aLockStatus, eSecurityFunctions.ForecastMethodsUserGlobalLock, eSecurityFunctions.ForecastMethodsGlobalGlobalLock);

                    _parentVersion = _OTSGlobalLockMethod.VersionRID;  // Issue 4008 - stodd

                    Common_Load();

                    // Begin MID Track 5862 - KJohnson - On an New Method, Attribute Set selection is checked even when Stores are not selected
                    _newWorkFlow = false;
                    // End MID Track 5862
                }
			//End TT#43 - MD - DOConnell - Projected Sales Enhancement
            }
            catch (Exception ex)
            {
                HandleException(ex, "InitializeOTSGlobalUnlockMethod");
                FormLoadError = true;
            }
        }

        /// <summary>
        /// Deletes a Global Unlock Method.
        /// </summary>
        /// <param name="aMethodRID">The record ID of the method</param>
        override public bool DeleteWorkflowMethod(int aMethodRID)
        {
            _methodData = new MethodBaseData(); //TT#273 - MD - DOConnell - Delete of Lock Method error
            try
            {
                aMethodType = _methodData.GetMethodType(aMethodRID);//TT#273 - MD - DOConnell - Delete of Lock Method error
			//Begin TT#43 - MD - DOConnell - Projected Sales Enhancement
                if (aMethodType == eMethodType.GlobalUnlock)
                {
                    _OTSGlobalUnlockMethod = new OTSGlobalUnlockMethod(SAB, aMethodRID);
                    ABM = _OTSGlobalUnlockMethod;
                }
                else
                {
                    _OTSGlobalLockMethod = new OTSGlobalLockMethod(SAB, aMethodRID);
                    ABM = _OTSGlobalLockMethod;
                }
			//End TT#43 - MD - DOConnell - Projected Sales Enhancement
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
        /// Renames an Forecast Spread Method.
        /// </summary>
        /// <param name="aMethodRID">The record ID of the method</param>
        /// <param name="aNewName">The new name of the workflow or method</param>
        override public bool RenameWorkflowMethod(int aMethodRID, string aNewName)
        {
            try
            {
			//Begin TT#43 - MD - DOConnell - Projected Sales Enhancement
                // Begin TT#2579 - JSmith - Unable to delete or rename forecast methods
                //if (ABM.MethodType == eMethodType.GlobalUnlock)
                _methodData = new MethodBaseData();
                aMethodType = _methodData.GetMethodType(aMethodRID);

                if (aMethodType == eMethodType.GlobalUnlock)
                // End TT#2579 - JSmith - Unable to delete or rename forecast methods
                {
                    _OTSGlobalUnlockMethod = new OTSGlobalUnlockMethod(SAB, aMethodRID);
                }
                else
                {
                    _OTSGlobalLockMethod = new OTSGlobalLockMethod(SAB, aMethodRID);
                }
			//End TT#43 - MD - DOConnell - Projected Sales Enhancement
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
                this.lblMerchandise.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Merchandise) + ":";
                this.lblVersion.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Version) + ":";
                this.lblTimePeriod.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_PlanTimePeriod) + ":";
                this.tabMethod.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Method);
                this.tabProperties.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Properties);

                this.grpUnlock.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_UnlockForm);
                this.cbMultiLevel.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_cbMultiLevel);
                this.lblFromLevel.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_FromLevel) + ":";
                this.lblToLevel.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_ToLevel) + ":";
				this.btnOverride.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_OverrideLowVersion);

                this.grpOptions.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_OptionsForm);
                this.cbStores.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_cbStores);
                this.cbChain.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_cbChain);

                this.grpStoreOptions.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_StoreOptionsForm);
                this.lblAttribute.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Attribute2) + ":";
                this.lblFilter.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Filter2) + ":";
                this.lblAttributeSet.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_AttributeSet2) + ":";

                this.grpLastProcessed.Text = ""; //<--Not Set
                this.lblLastProcessed.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_LastProcessed) + ":";
                this.lblDateTime.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_DateTime) + ":";
                this.lblByUser.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_ByUser) + ":";
            }
            catch (Exception exception)
            {
                HandleException(exception);
            }
        }
        private void Common_Load()
        {
            try
            {
                //Icon = MIDGraphics.GetIcon(MIDGraphics.CopyImage);
                _storeFilterDL = new FilterData();
                eMIDTextCode textCode; //TT#43 - MD - DOConnell - Projected Sales Enhancement
                //_versionProfList = SAB.ClientServerSession.GetUserForecastVersions();  // Issue 4858
				_variables = SAB.ApplicationServerSession.DefaultPlanComputations.PlanVariables.VariableProfileList; 
                SetText();
                //Begin TT#43 - MD - DOConnell - Projected Sales Enhancement
                if (ABM.MethodType == eMethodType.GlobalUnlock)
                {
                     textCode = eMIDTextCode.frm_GlobalUnlock;
                }
                else
                {
                     this.grpUnlock.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_glbLock);
                     textCode = eMIDTextCode.frm_GlobalLock;
                }
                
                _thisTitle = MIDText.GetTextOnly((int)textCode);
                //if (_OTSGlobalUnlockMethod.Method_Change_Type == eChangeType.add)
                if (ABM.Method_Change_Type == eChangeType.add)
                {
                    Format_Title(eDataState.New, textCode, null);
                }
                else
                {
                    if (FunctionSecurity.AllowUpdate)
                    {
                        //Format_Title(eDataState.Updatable, textCode, _OTSGlobalUnlockMethod.Name);
                        Format_Title(eDataState.Updatable, textCode, ABM.Name);
                    }
                    else
                    {
                        //Format_Title(eDataState.ReadOnly, textCode, _OTSGlobalUnlockMethod.Name);
                        Format_Title(eDataState.ReadOnly, textCode, ABM.Name);
                    }

                    if (ABM.MethodType == eMethodType.GlobalUnlock)
                    {
                        lblDateTimeValue.Text = _OTSGlobalUnlockMethod.LastProcessedDateTime;
                        lblByUserValue.Text = _OTSGlobalUnlockMethod.LastProcessedUser;
                    }
                    else
                    {
                        lblDateTimeValue.Text = _OTSGlobalLockMethod.LastProcessedDateTime;
                        lblByUserValue.Text = _OTSGlobalLockMethod.LastProcessedUser;
                        this.grpUnlock.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_glbLock);
                    }
				//End TT#43 - MD - DOConnell - Projected Sales Enhancement
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
				//Begin TT#43 - MD - DOConnell - Projected Sales Enhancement
                if (ABM.MethodType == eMethodType.GlobalUnlock)
                {
				    LoadOverrideModelComboBox(cboOverride.ComboBox, _OTSGlobalUnlockMethod.OverrideLowLevelRid, _OTSGlobalUnlockMethod.CustomOLL_RID);
                }
                else
                {
                    LoadOverrideModelComboBox(cboOverride.ComboBox, _OTSGlobalLockMethod.OverrideLowLevelRid, _OTSGlobalLockMethod.CustomOLL_RID);
                }
				//End TT#43 - MD - DOConnell - Projected Sales Enhancement
                //CreateBasisComboLists();

                PopulateStoreAttributes();
                PopulateFilter();

                LoadMethodData();

                cbStoresCheckedChanging = false;

                // Begin issue 3716 - stodd 02/15/06
                LoadWorkflows();
                // End issue 3716

                // Begin MID Track 5862 - KJohnson - On an New Method, Attribute Set selection is checked even when Stores are not selected
                //if (!_newWorkFlow)
                //{
                    ApplySecurity();
                //}
                // End MID Track 5862

                BindVersionComboBoxes();

                //BEGIN TT#110-MD-VStuart - In Use Tool
                tabSpreadMethod.Controls.Remove(tabProperties);
                //END TT#110-MD-VStuart - In Use Tool
            }

            catch (Exception exception)
            {
                HandleException(exception);
            }
        }

        // Begin MID Track 5852 - KJohnson - Security changes
        private void ApplySToreChainRBSecurity()
        {
            bool chainCanUpdate; //TT#43 - MD - DOConnell - Projected Sales Enhancement
            bool storeCanUpdate; //TT#43 - MD - DOConnell - Projected Sales Enhancement
            if (FunctionSecurity.AllowUpdate)
            {
			//Begin TT#43 - MD - DOConnell - Projected Sales Enhancement
                if (ABM.MethodType == eMethodType.GlobalUnlock)
                {
                    chainCanUpdate = _OTSGlobalUnlockMethod.ChainAuthorizedToPlan();
                }
                else
                {
                    chainCanUpdate = _OTSGlobalLockMethod.ChainAuthorizedToPlan();
                }
			//End TT#43 - MD - DOConnell - Projected Sales Enhancement
				// Begin Track #5859 stodd
				ErrorProvider.SetError(cbChain, string.Empty);
				// End Track #5859
                if (chainCanUpdate)
                {
                    cbChain.Enabled = true;
                }
                else
                {
					// Begin Track #5859 stodd
					if (cbChain.Checked)
					{
						string errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_NotAuthorizedToPlan);
						errorMessage = errorMessage.Replace("this", "chain");
						ErrorProvider.SetError(cbChain, errorMessage);
					}
                    else
                    {
                        cbChain.Enabled = false;
                    }
                    //cbChain.Enabled = false;
					// Begin Track 5852 stodd
                    //cbChain.Checked = false;
					// End Track 5852
                    //_OTSGlobalUnlockMethod.Chain = false;
					// End Track #5859
                }
				//Begin TT#43 - MD - DOConnell - Projected Sales Enhancement
                if (ABM.MethodType == eMethodType.GlobalUnlock)
                {
                    storeCanUpdate = _OTSGlobalUnlockMethod.StoreAuthorizedToPlan();
                }
                else
                {
                    storeCanUpdate = _OTSGlobalLockMethod.StoreAuthorizedToPlan();
                }
				//End TT#43 - MD - DOConnell - Projected Sales Enhancement
				// Begin Track #5859 stodd
				ErrorProvider.SetError(cbStores, string.Empty);
				// End Track #5859

                if (storeCanUpdate)
                {
                    cbStores.Enabled = true;
                    cboAttribute.Enabled = true;
                    cboFilter.Enabled = true;
                    chkLstBxAttributeSet.Enabled = true;

                    grpStoreOptions.Enabled = cbStores.Checked;  //TT#314 - MD - Disable Store Options when store not selected 
                }
                else
                {
					// Begin Track #5859 stodd
					if (cbStores.Checked)
					{
						string errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_NotAuthorizedToPlan);
						errorMessage = errorMessage.Replace("this", "stores");
						ErrorProvider.SetError(cbStores, errorMessage);
					}
                    else
                    {
                        cbStores.Enabled = false;
                        cboAttribute.Enabled = false;
                        cboFilter.Enabled = false;
                        chkLstBxAttributeSet.Enabled = false;
                    }
                    //cbStores.Enabled = false;
                    //cboAttribute.Enabled = false;
                    //cboFilter.Enabled = false;
                    //chkLstBxAttributeSet.Enabled = false;
					// End Track #5859 stodd
                }
            }
        }
        // End MID Track 5852

        private void BindVersionComboBoxes()
        {
			ProfileList versionProfList = new ProfileList(eProfileType.Version);  // Track #5852 stodd
		
            try
            {
                // Begin MID Track 5855 - KJohnson - Version list includes all version, should not include View only
                string previousValue = this.cboSpreadVersion.Text;

                if (!_updatingBindVersionComboBoxes) 
                {
                    _updatingBindVersionComboBoxes = true;
					//Begin TT#43 - MD - DOConnell - Projected Sales Enhancement
                    if (ABM.MethodType == eMethodType.GlobalUnlock)
                    {
                        if (this.cbStores.Checked && this.cbChain.Checked)
                        {
                            versionProfList = base.GetForecastVersionList(eSecuritySelectType.Update, eSecurityTypes.Store | eSecurityTypes.Chain, false, this._OTSGlobalUnlockMethod.VersionRID, true);	// Track #5871
                        }
                        else if (this.cbStores.Checked)
                        {
                            versionProfList = base.GetForecastVersionList(eSecuritySelectType.Update, eSecurityTypes.Store, false, this._OTSGlobalUnlockMethod.VersionRID, true);	// Track #5871
                        }
                        else if (this.cbChain.Checked)
                        {
                            versionProfList = base.GetForecastVersionList(eSecuritySelectType.Update, eSecurityTypes.Chain, false, this._OTSGlobalUnlockMethod.VersionRID, true);	// Track #5871
                        }
                        else
                        {
                            versionProfList = base.GetForecastVersionList(eSecuritySelectType.Update, eSecurityTypes.Store | eSecurityTypes.Chain, false, this._OTSGlobalUnlockMethod.VersionRID, true);	// Track #5871
                        }

                        if (versionProfList.ArrayList.Count > 0)
                        {
                        this.cboSpreadVersion.DisplayMember = "Description";
                        this.cboSpreadVersion.ValueMember = "Key";
                        this.cboSpreadVersion.DataSource = versionProfList.ArrayList;
                            
                            //---Set List Item To Previous Item After Reloading Combo List---
                        for (int i = 0; i < this.cboSpreadVersion.Items.Count; i++)
                            {
                                if (ABM.MethodType == eMethodType.GlobalUnlock)
                                {
                            if ((((VersionProfile)this.cboSpreadVersion.Items[i]).Description == previousValue) ||
                                (((VersionProfile)this.cboSpreadVersion.Items[i]).Key == this._OTSGlobalUnlockMethod.VersionRID))
                                    {
                                        this.cboSpreadVersion.SelectedIndex = i;
                                        break;
                                    }
                                }
                                else
                                {
                                    if ((((VersionProfile)this.cboSpreadVersion.Items[i]).Description == previousValue) ||
                                        (((VersionProfile)this.cboSpreadVersion.Items[i]).Key == this._OTSGlobalLockMethod.VersionRID))
                                    {
                                        this.cboSpreadVersion.SelectedIndex = i;
                                        break;
                                    }
                                }
                            }
                        }
                        else
                        {
                            _setting_cboSpreadVersion = true;
                            this.cboSpreadVersion.DataSource = null;
                            _setting_cboSpreadVersion = false;
                        }

                        _updatingBindVersionComboBoxes = false;
                    }
                    else
                    {
                        if (this.cbStores.Checked && this.cbChain.Checked)
                        {
                            versionProfList = base.GetForecastVersionList(eSecuritySelectType.Update, eSecurityTypes.Store | eSecurityTypes.Chain, false, this._OTSGlobalLockMethod.VersionRID, true);	// Track #5871
                        }
                        else if (this.cbStores.Checked)
                        {
                            versionProfList = base.GetForecastVersionList(eSecuritySelectType.Update, eSecurityTypes.Store, false, this._OTSGlobalLockMethod.VersionRID, true);	// Track #5871
                        }
                        else if (this.cbChain.Checked)
                        {
                            versionProfList = base.GetForecastVersionList(eSecuritySelectType.Update, eSecurityTypes.Chain, false, this._OTSGlobalLockMethod.VersionRID, true);	// Track #5871
                        }
                        else
                        {
                            versionProfList = base.GetForecastVersionList(eSecuritySelectType.Update, eSecurityTypes.Store | eSecurityTypes.Chain, false, this._OTSGlobalLockMethod.VersionRID, true);	// Track #5871
                        }

                        if (versionProfList.ArrayList.Count > 0)
                        {
                            this.cboSpreadVersion.DisplayMember = "Description";
                            this.cboSpreadVersion.ValueMember = "Key";
                            this.cboSpreadVersion.DataSource = versionProfList.ArrayList;

                            //---Set List Item To Previous Item After Reloading Combo List---
                            for (int i = 0; i < this.cboSpreadVersion.Items.Count; i++)
                            {
                                if ((((VersionProfile)this.cboSpreadVersion.Items[i]).Description == previousValue) ||
                                    (((VersionProfile)this.cboSpreadVersion.Items[i]).Key == this._OTSGlobalLockMethod.VersionRID))
                                {
                                this.cboSpreadVersion.SelectedIndex = i;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            _setting_cboSpreadVersion = true;
                        this.cboSpreadVersion.DataSource = null;
                            _setting_cboSpreadVersion = false;
                        }

                        _updatingBindVersionComboBoxes = false;
                    }
					//End TT#43 - MD - DOConnell - Projected Sales Enhancement
                }
                // End MID Track 5855
            }
            catch (Exception exc)
            {
                _updatingBindVersionComboBoxes = false;
                string message = exc.ToString();
                throw;
            }
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
			//Begin TT#43 - MD - DOConnell - Projected Sales Enhancement

                _methodData = new MethodBaseData();
                aMethodType = _methodData.GetMethodType(aMethodRID);

                if (aMethodType == eMethodType.GlobalUnlock)
                {
                    _OTSGlobalUnlockMethod = new OTSGlobalUnlockMethod(SAB, aMethodRID);
                    ProcessAction(eMethodType.GlobalUnlock, true);
                }
                else
                {
                    _OTSGlobalLockMethod = new OTSGlobalLockMethod(SAB, aMethodRID);
                    ProcessAction(eMethodType.GlobalLock, true);
                }
			//End TT#43 - MD - DOConnell - Projected Sales Enhancement
            }
            catch (Exception ex)
            {
                HandleException(ex);
                FormLoadError = true;
            }
        }
        // End Issue 4323 - stodd 2.21.07 - fix process from workflow explorer

        private void PopulateFilter()
        {
            DataTable dtFilter;

            try
            {
                cboFilter.Items.Clear();
                // Begin TT#2669 - JSmith - Unable to remove a filter from a wokflow
                //cboFilter.Items.Add(new FilterNameCombo(Include.UndefinedStoreFilter, Include.GlobalUserRID, "(none)"));
                cboFilter.Items.Add(GetRemoveFilterRow());
                // End TT#2669 - JSmith - Unable to remove a filter from a wokflow

                dtFilter = _storeFilterDL.FilterRead(filterTypes.StoreFilter, eProfileType.FilterStore, _userRIDList);

                foreach (DataRow row in dtFilter.Rows)
                {
                    cboFilter.Items.Add(
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

        private void LoadMethodData()
        {
            try
            {
                // Inititalize Fields
				//Begin TT#43 - MD - DOConnell - Projected Sales Enhancement
                if (ABM.MethodType == eMethodType.GlobalUnlock)
                {
                    cboFilter.SelectedIndex = cboFilter.Items.IndexOf(new FilterNameCombo(_OTSGlobalUnlockMethod.Filter, -1, ""));
                    this.cboAttribute.SelectedValue = _OTSGlobalUnlockMethod.SG_RID;
                }
                else
                {
                    cboFilter.SelectedIndex = cboFilter.Items.IndexOf(new FilterNameCombo(_OTSGlobalLockMethod.Filter, -1, ""));
                    this.cboAttribute.SelectedValue = _OTSGlobalLockMethod.SG_RID;
                }
				//End TT#43 - MD - DOConnell - Projected Sales Enhancement
                // Begin Track #4872 - JSmith - Global/User Attributes
                if (cboAttribute.ContinueReadOnly)
                {
                    SetMethodReadOnly();
                }
                // End Track #4872

                mdsPlanDateRange.DateRangeRID = Include.UndefinedCalendarDateRange;
                mdsPlanDateRange.SetImage(null);
				//Begin TT#43 - MD - DOConnell - Projected Sales Enhancement
                //if (_OTSGlobalUnlockMethod.Method_Change_Type != eChangeType.add)
                if (ABM.Method_Change_Type != eChangeType.add)
                {

                   // this.txtName.Text = _OTSGlobalUnlockMethod.Name;
                    //this.txtDesc.Text = _OTSGlobalUnlockMethod.Method_Description;

                    //if (_OTSGlobalUnlockMethod.User_RID == Include.GetGlobalUserRID())
                    this.txtName.Text = ABM.Name;
                    this.txtDesc.Text = ABM.Method_Description;

                    if (ABM.User_RID == Include.GetGlobalUserRID())
                        radGlobal.Checked = true;
                    else
                        radUser.Checked = true;
                    LoadWorkflows();
                }
				//End TT#43 - MD - DOConnell - Projected Sales Enhancement
                //Begin Track #5858 - JSmith - Validating store security only
                txtSpreadNode.Tag = new MIDMerchandiseTextBoxTag(SAB, txtSpreadNode, eSecurityTypes.Store, eSecuritySelectType.Update); ;
                // Begin TT#44 - JSmith - Drag/Drop User Attributes or Filters in to Global Methods does not react consistantly
                //cboFilter.Tag = new MIDStoreFilterComboBoxTag(SAB, cboFilter, eSecurityTypes.Store, eSecuritySelectType.Update);
                //Begin TT#43 - MD - DOConnell - Projected Sales Enhancement
				if (ABM.MethodType == eMethodType.GlobalUnlock)
                {
                    cboFilter.Tag = new MIDStoreFilterComboBoxTag(SAB, cboFilter.ComboBox, eSecurityTypes.Store, eSecuritySelectType.Update, FunctionSecurity, _OTSGlobalUnlockMethod.GlobalUserType == eGlobalUserType.User);
                }
                else
                {
                    cboFilter.Tag = new MIDStoreFilterComboBoxTag(SAB, cboFilter.ComboBox, eSecurityTypes.Store, eSecuritySelectType.Update, FunctionSecurity, _OTSGlobalLockMethod.GlobalUserType == eGlobalUserType.User);
                }
                //End Track #5858
                if (ABM.MethodType == eMethodType.GlobalUnlock)
                {
                    if (_OTSGlobalUnlockMethod.HierNodeRID > 0)
                    {
                        //BEGIN TT#338 - MD - DOConnell - Global Lock the merchandise only has the color appearing not the style color that was originally dragged in
                        //HierarchyNodeProfile hnp = SAB.HierarchyServerSession.GetNodeData(_OTSGlobalUnlockMethod.HierNodeRID);
                        HierarchyNodeProfile hnp = SAB.HierarchyServerSession.GetNodeData(_OTSGlobalUnlockMethod.HierNodeRID, false, true);
                        //END TT#338 - MD - DOConnell - Global Lock the merchandise only has the color appearing not the style color that was originally dragged in
                        txtSpreadNode.Text = hnp.Text;
                        //Begin Track #5858 - JSmith - Validating store security only
                        //txtSpreadNode.Tag = hnp.Key;
                        ((MIDTag)txtSpreadNode.Tag).MIDTagData = hnp;
                        //End Track #5858
					//BEGIN TT#4650 - DOConnell - Changes do not hold
					//PopulateFromToLevels(hnp, cboFromLevel.ComboBox, 0);
                    //PopulateFromToLevels(hnp, cboToLevel.ComboBox, 0);
                    PopulateFromToLevels(hnp, cboFromLevel, 0);
                    PopulateFromToLevels(hnp, cboToLevel, 0);
					//END TT#4650 - DOConnell - Changes do not hold
                    cboFromLevel.SelectedIndex = cboFromLevel.Items.IndexOf(new FromLevelCombo(_OTSGlobalUnlockMethod.FromLevelType, _OTSGlobalUnlockMethod.FromLevelOffset, _OTSGlobalUnlockMethod.FromLevelSequence, ""));
                    cboToLevel.SelectedIndex = cboToLevel.Items.IndexOf(new ToLevelCombo(_OTSGlobalUnlockMethod.ToLevelType, _OTSGlobalUnlockMethod.ToLevelOffset, _OTSGlobalUnlockMethod.ToLevelSequence, ""));
                    }


                    //// Begin Track #5858 - JSmith - Validating store security only
                    ////base.ValidatePlanNodeSecurity(txtSpreadNode, true);
                    //if (_OTSGlobalUnlockMethod.Stores)
                    //{
                    //    base.ValidatePlanNodeSecurity(txtSpreadNode, true, eSecurityTypes.Store);
                    //}
                    //else if (_OTSGlobalUnlockMethod.Chain)
                    //{
                    //    base.ValidatePlanNodeSecurity(txtSpreadNode, true, eSecurityTypes.Chain);
                    //}
                    //else if (_OTSGlobalUnlockMethod.Stores && _OTSGlobalUnlockMethod.Chain)
                    //{
                    //    base.ValidatePlanNodeSecurity(txtSpreadNode, true, eSecurityTypes.Store | eSecurityTypes.Chain);
                    //}
                    //// End Track #5858

                    if (_OTSGlobalUnlockMethod.VersionRID > 0)
                    {
                    cboSpreadVersion.SelectedValue = _OTSGlobalUnlockMethod.VersionRID;
                        _parentVersion = _OTSGlobalUnlockMethod.VersionRID;
                    }
                    else
                    {
                        _parentVersion = Include.NoRID;
                    }

                    //// BEGIN Issue 5852
                    //if (_OTSGlobalUnlockMethod.Stores)
                    //{
                    //    base.ValidatePlanVersionSecurity(cboSpreadVersion, true, ePlanType.Store);
                    //}
                    //else if (_OTSGlobalUnlockMethod.Chain)
                    //{
                    //    base.ValidatePlanVersionSecurity(cboSpreadVersion, true, ePlanType.Chain);
                    //}
                    //else if (_OTSGlobalUnlockMethod.Stores && _OTSGlobalUnlockMethod.Chain)
                    //{
                    //    base.ValidatePlanVersionSecurity(cboSpreadVersion, true, ePlanType.Chain | ePlanType.Store);
                    //}
                    //// END Issue 5852

                    if (_OTSGlobalUnlockMethod.DateRangeRID > 0 && _OTSGlobalUnlockMethod.DateRangeRID != Include.UndefinedCalendarDateRange)
                    {
                        DateRangeProfile drp = SAB.ClientServerSession.Calendar.GetDateRange(_OTSGlobalUnlockMethod.DateRangeRID);
                        LoadDateRangeSelector(mdsPlanDateRange, drp);
                    }

                    if (_OTSGlobalUnlockMethod.MultiLevel)
                    {
                        this.cbMultiLevel.Checked = true;
                    this.cboFromLevel.Enabled = true;
                    }
                    else
                    {
                        this.cbMultiLevel.Checked = false;
                    this.cboFromLevel.Enabled = false;
                    }

                    if (_OTSGlobalUnlockMethod.Stores)
                    {
                        this.cbStores.Checked = true;
                    }
                    else
                    {
                        this.cbStores.Checked = false;
                    }

                    if (_OTSGlobalUnlockMethod.Chain)
                    {
                        this.cbChain.Checked = true;
                    }
                    else
                    {
                        this.cbChain.Checked = false;
                    }

                    if (_OTSGlobalUnlockMethod.SG_RID > 0)
                    {
                        this.cboAttribute.SelectedValue = _OTSGlobalUnlockMethod.SG_RID;
                        // Begin Track #4872 - JSmith - Global/User Attributes
                        if (cboAttribute.ContinueReadOnly)
                        {
                            SetMethodReadOnly();
                        }
                        // End Track #4872
                    }
                }
                else
                {
                    if (_OTSGlobalLockMethod.HierNodeRID > 0)
                    {
                        //BEGIN TT#338 - MD - DOConnell - Global Lock the merchandise only has the color appearing not the style color that was originally dragged in
                        //HierarchyNodeProfile hnp = SAB.HierarchyServerSession.GetNodeData(_OTSGlobalLockMethod.HierNodeRID);
                        HierarchyNodeProfile hnp = SAB.HierarchyServerSession.GetNodeData(_OTSGlobalLockMethod.HierNodeRID, false, true);
                        //END TT#338 - MD - DOConnell - Global Lock the merchandise only has the color appearing not the style color that was originally dragged in
                        txtSpreadNode.Text = hnp.Text;
                        //Begin Track #5858 - JSmith - Validating store security only
                        //txtSpreadNode.Tag = hnp.Key;
                        ((MIDTag)txtSpreadNode.Tag).MIDTagData = hnp;
                        //End Track #5858
						//END TT#4650 - DOConnell - Changes do not hold
						//PopulateFromToLevels(hnp, cboFromLevel.ComboBox, 0);
                    	//PopulateFromToLevels(hnp, cboToLevel.ComboBox, 0);
                        PopulateFromToLevels(hnp, cboFromLevel, 0);
                        PopulateFromToLevels(hnp, cboToLevel, 0);
						//END TT#4650 - DOConnell - Changes do not hold
                        cboFromLevel.SelectedIndex = cboFromLevel.Items.IndexOf(new FromLevelCombo(_OTSGlobalLockMethod.FromLevelType, _OTSGlobalLockMethod.FromLevelOffset, _OTSGlobalLockMethod.FromLevelSequence, ""));
                        cboToLevel.SelectedIndex = cboToLevel.Items.IndexOf(new ToLevelCombo(_OTSGlobalLockMethod.ToLevelType, _OTSGlobalLockMethod.ToLevelOffset, _OTSGlobalLockMethod.ToLevelSequence, ""));
                    }

                    if (_OTSGlobalLockMethod.VersionRID > 0)
                    {
                        cboSpreadVersion.SelectedValue = _OTSGlobalLockMethod.VersionRID;
                        _parentVersion = _OTSGlobalLockMethod.VersionRID;
                    }
                    else
                    {
                        _parentVersion = Include.NoRID;
                    }

                    if (_OTSGlobalLockMethod.DateRangeRID > 0 && _OTSGlobalLockMethod.DateRangeRID != Include.UndefinedCalendarDateRange)
                    {
                        DateRangeProfile drp = SAB.ClientServerSession.Calendar.GetDateRange(_OTSGlobalLockMethod.DateRangeRID);
                        LoadDateRangeSelector(mdsPlanDateRange, drp);
                    }

                    if (_OTSGlobalLockMethod.MultiLevel)
                    {
                        this.cbMultiLevel.Checked = true;
                        this.cboFromLevel.Enabled = true;
                    }
                    else
                    {
                        this.cbMultiLevel.Checked = false;
                        this.cboFromLevel.Enabled = false;
                    }

                    if (_OTSGlobalLockMethod.Stores)
                    {
                        this.cbStores.Checked = true;
                    }
                    else
                    {
                        this.cbStores.Checked = false;
                    }

                    if (_OTSGlobalLockMethod.Chain)
                    {
                        this.cbChain.Checked = true;
                    }
                    else
                    {
                        this.cbChain.Checked = false;
                    }

                    if (_OTSGlobalLockMethod.SG_RID > 0)
                    {
                        this.cboAttribute.SelectedValue = _OTSGlobalLockMethod.SG_RID;
                        // Begin Track #4872 - JSmith - Global/User Attributes
                        if (cboAttribute.ContinueReadOnly)
                        {
                            SetMethodReadOnly();
                        }
                        // End Track #4872
                    }
				//End TT#43 - MD - DOConnell - Projected Sales Enhancement
                }
 
            }
            catch (Exception ex)
            {
                HandleException(ex, "LoadMethodData");
            }
        }

        private void PopulateStoreAttributes()
        {
            try
            {
                // Begin Track #4872 - JSmith - Global/User Attributes
                //ProfileList pl = SAB.StoreServerSession.GetStoreGroupListViewList();
                BuildAttributeList();

                //this.cboAttribute.ValueMember = "Key";
                //this.cboAttribute.DisplayMember = "Name";
                //this.cboAttribute.DataSource = pl.ArrayList;
                // End Track #4872
            }
            catch (Exception ex)
            {
                HandleException(ex);
                FormLoadError = true;
            }
        }

        private void cboAttribute_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                if (FormLoaded)
                {
                    if (_attributeReset)
                    {
                        _attributeReset = false;
                        return;
                    }
                    //=======================================================================================
                    // The current rules are saved because the warning below needs to know if there is any
                    // data in the grid for the current attribute. If the user continues, it will all be removed.
                    //=======================================================================================
                    //                    this.SetMatrixRules(_currAttributeSet);
                    //idx = this.cboAttribute.SelectedIndex;
                    //                    if (!MatrixWarningOK(this.lblAttribute.Text))
                    //                    {
                    //                        _attributeReset = true;
                    //                        cboAttribute.SelectedValue = _currAttribute;
                    //                        return;
                    //                    }
                    //                    else
                    //                    {
                    //this.cboAttribute.SelectedIndex = idx;
                    PopulateStoreAttributeSet(this.cboAttribute.SelectedValue.ToString());

                    //                        // Build Matrix Grid
                    //                        _dtRules.Clear();
                    //                        BuildMatrix(_currAttributeSet);
                    ChangePending = true;
                    //                    }
                }
                else
                {

                    // Begin Track #4872 - JSmith - Global/User Attributes
                    //PopulateStoreAttributeSet(this.cboAttribute.SelectedValue.ToString());
                    //BEGIN TT#285 - MD - DOConnell - Global Lock Screen issues
                    if (cbStores.Checked == true)
                    {
                        if (cboAttribute.SelectedValue != null)
                        {
                            PopulateStoreAttributeSet(this.cboAttribute.SelectedValue.ToString());
                        }
                    }
                    //END TT#285 - MD - DOConnell - Global Lock Screen issues
                    // End Track #4872
                }

            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }
        //Begin TT#316 -jsobek -Replace all Windows Combobox controls with new enhanced control
        void cboAttribute_MIDComboBoxPropertiesChangedEvent(object source, MIDComboBoxPropertiesChangedEventArgs args)
        {
            this.cboAttribute_SelectionChangeCommitted(source, new EventArgs());
        }
        //End TT#316

        /// <summary>
        /// Populate all values of the Store_Group_Levels (Attribute Sets)
        /// based on the key parameter.
        /// </summary>
        /// <param name="key">SGL_RID</param>
        private void PopulateStoreAttributeSet(string key)
        {
            try
            {
                ProfileList pl = new ProfileList(eProfileType.StoreGroupLevelListView);
                //				StoreGroupLevelListViewProfile sgllvp = new StoreGroupLevelListViewProfile(Include.NoRID);
                //				sgllvp.Name = "(none)";
                //				pl.Add(sgllvp);

                int sgRid = Convert.ToInt32(key, CultureInfo.CurrentUICulture);
                if (sgRid != Include.NoRID)
                {
                    pl = StoreMgmt.StoreGroup_GetLevelListViewList(sgRid); //SAB.StoreServerSession.GetStoreGroupLevelListViewList(sgRid);
                    //					foreach (StoreGroupLevelListViewProfile sgllvp2 in pl2.ArrayList)
                    //					{
                    //						pl.Add(sgllvp2);
                    //					}
                }

                //--- Add Items -----------------------------------------------
                this.chkLstBxAttributeSet.Items.Clear();
                foreach (StoreGroupLevelListViewProfile profile in pl.ArrayList)
                {
                    this.chkLstBxAttributeSet.Items.Add(profile, CheckState.Unchecked);
                }

                //--- Check Items ----------------------------------------------
				//Begin TT#43 - MD - DOConnell - Projected Sales Enhancement
                ArrayList tempArrayList = new ArrayList();
                if (ABM.MethodType == eMethodType.GlobalUnlock)
                {
                    tempArrayList = _OTSGlobalUnlockMethod.SGL_RID_List;
                }
                else
                {
                    tempArrayList = _OTSGlobalLockMethod.SGL_RID_List;
                }
                //if (_OTSGlobalUnlockMethod.SGL_RID_List != null)
                if (tempArrayList != null)
                {
                    for (int i = 0; i < this.chkLstBxAttributeSet.Items.Count; i++)
                    {
                        for (int j = 0; j < tempArrayList.Count; j++)
                        {
                            StoreGroupLevelListViewProfile sglProfileItem = (StoreGroupLevelListViewProfile)this.chkLstBxAttributeSet.Items[i];
                            if (sglProfileItem.Key == Convert.ToInt32(tempArrayList[j]))
                            {
                                this.chkLstBxAttributeSet.SetItemCheckState(i, CheckState.Checked);
                            }
                        }
                    }
                }
				//End TT#43 - MD - DOConnell - Projected Sales Enhancement
                // Begin MID Track 5862 - KJohnson - On an New Method, Attribute Set selection is checked even when Stores are not selected
                if (FormLoaded)
                {
                    //--If No Items Are Checked Then "Check Them All"----
                    if (this.chkLstBxAttributeSet.CheckedItems.Count == 0)
                    {
                        for (int i = 0; i < this.chkLstBxAttributeSet.Items.Count; i++)
                        {
                            this.chkLstBxAttributeSet.SetItemCheckState(i, CheckState.Checked);
                        }
                    }
                }
                // End MID Track 5862

            }
            catch (Exception ex)
            {
                HandleException(ex);
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
			//Begin TT#43 - MD - DOConnell - Projected Sales Enhancement
                if (ABM.MethodType == eMethodType.GlobalUnlock)
                {
                    //Begin Track #5858 - JSmith - Validating store security only
                    //_OTSGlobalUnlockMethod.HierNodeRID = Convert.ToInt32(txtSpreadNode.Tag, CultureInfo.CurrentUICulture);
                    _OTSGlobalUnlockMethod.HierNodeRID = Convert.ToInt32(((HierarchyNodeProfile)((MIDTag)txtSpreadNode.Tag).MIDTagData).Key, CultureInfo.CurrentUICulture);
                    //End Track #5858
                    _OTSGlobalUnlockMethod.VersionRID = Convert.ToInt32(cboSpreadVersion.SelectedValue, CultureInfo.CurrentUICulture);
                    _OTSGlobalUnlockMethod.SG_RID = (int)cboAttribute.SelectedValue;

                    ArrayList SGL_RID_List = new ArrayList();
                    for (int i = 0; i <= this.chkLstBxAttributeSet.CheckedItems.Count - 1; i++)
                    {
                        StoreGroupLevelListViewProfile sglProfileItem = (StoreGroupLevelListViewProfile)this.chkLstBxAttributeSet.CheckedItems[i];
                        SGL_RID_List.Add(sglProfileItem.Key);
                    }
                    _OTSGlobalUnlockMethod.SGL_RID_List = SGL_RID_List;

                    _OTSGlobalUnlockMethod.FromLevelType = ((FromLevelCombo)cboFromLevel.SelectedItem).FromLevelType;
                    _OTSGlobalUnlockMethod.FromLevelOffset = ((FromLevelCombo)cboFromLevel.SelectedItem).FromLevelOffset;
                    _OTSGlobalUnlockMethod.FromLevelSequence = ((FromLevelCombo)cboFromLevel.SelectedItem).FromLevelSequence;
                    _OTSGlobalUnlockMethod.ToLevelType = ((ToLevelCombo)cboToLevel.SelectedItem).ToLevelType;
                    _OTSGlobalUnlockMethod.ToLevelOffset = ((ToLevelCombo)cboToLevel.SelectedItem).ToLevelOffset;
                    _OTSGlobalUnlockMethod.ToLevelSequence = ((ToLevelCombo)cboToLevel.SelectedItem).ToLevelSequence;

                    _OTSGlobalUnlockMethod.DateRangeRID = mdsPlanDateRange.DateRangeRID;

                    if (cboFilter.SelectedItem == null)
                    {
                        // Begin TT#2669 - JSmith - Unable to remove a filter from a wokflow
                        //_OTSGlobalUnlockMethod.Filter = Include.NoRID;
                        _OTSGlobalUnlockMethod.Filter = Include.UndefinedStoreFilter;
                        // End TT#2669 - JSmith - Unable to remove a filter from a wokflow
                    }
                    else
                    {
                    _OTSGlobalUnlockMethod.Filter = ((FilterNameCombo)cboFilter.SelectedItem).FilterRID;
                    }

                    if (this.cbMultiLevel.Checked)
                    {
                        _OTSGlobalUnlockMethod.MultiLevel = true;
                    }
                    else
                    {
                        _OTSGlobalUnlockMethod.MultiLevel = false;
                    }

                    if (this.cbStores.Checked)
                    {
                        _OTSGlobalUnlockMethod.Stores = true;
                    }
                    else
                    {
                        _OTSGlobalUnlockMethod.Stores = false;
                    }

                    if (this.cbChain.Checked)
                    {
                        _OTSGlobalUnlockMethod.Chain = true;
                    }
                    else
                    {
                        _OTSGlobalUnlockMethod.Chain = false;
                    }
                }
                else
                {
                    //Begin Track #5858 - JSmith - Validating store security only
                    _OTSGlobalLockMethod.HierNodeRID = Convert.ToInt32(((HierarchyNodeProfile)((MIDTag)txtSpreadNode.Tag).MIDTagData).Key, CultureInfo.CurrentUICulture);
                    //End Track #5858
                    _OTSGlobalLockMethod.VersionRID = Convert.ToInt32(cboSpreadVersion.SelectedValue, CultureInfo.CurrentUICulture);
                    _OTSGlobalLockMethod.SG_RID = (int)cboAttribute.SelectedValue;

                    ArrayList SGL_RID_List = new ArrayList();
                    for (int i = 0; i <= this.chkLstBxAttributeSet.CheckedItems.Count - 1; i++)
                    {
                        StoreGroupLevelListViewProfile sglProfileItem = (StoreGroupLevelListViewProfile)this.chkLstBxAttributeSet.CheckedItems[i];
                        SGL_RID_List.Add(sglProfileItem.Key);
                    }
                    _OTSGlobalLockMethod.SGL_RID_List = SGL_RID_List;

                    _OTSGlobalLockMethod.FromLevelType = ((FromLevelCombo)cboFromLevel.SelectedItem).FromLevelType;
                    _OTSGlobalLockMethod.FromLevelOffset = ((FromLevelCombo)cboFromLevel.SelectedItem).FromLevelOffset;
                    _OTSGlobalLockMethod.FromLevelSequence = ((FromLevelCombo)cboFromLevel.SelectedItem).FromLevelSequence;
                    _OTSGlobalLockMethod.ToLevelType = ((ToLevelCombo)cboToLevel.SelectedItem).ToLevelType;
                    _OTSGlobalLockMethod.ToLevelOffset = ((ToLevelCombo)cboToLevel.SelectedItem).ToLevelOffset;
                    _OTSGlobalLockMethod.ToLevelSequence = ((ToLevelCombo)cboToLevel.SelectedItem).ToLevelSequence;

                    _OTSGlobalLockMethod.DateRangeRID = mdsPlanDateRange.DateRangeRID;

                    if (cboFilter.SelectedItem == null)
                    {
                        // Begin TT#2669 - JSmith - Unable to remove a filter from a wokflow
                        //_OTSGlobalLockMethod.Filter = Include.NoRID;
                        _OTSGlobalLockMethod.Filter = Include.UndefinedStoreFilter;
                        // End TT#2669 - JSmith - Unable to remove a filter from a wokflow
                    }
                    else
                    {
                        _OTSGlobalLockMethod.Filter = ((FilterNameCombo)cboFilter.SelectedItem).FilterRID;
                    }

                    if (this.cbMultiLevel.Checked)
                    {
                        _OTSGlobalLockMethod.MultiLevel = true;
                    }
                    else
                    {
                        _OTSGlobalLockMethod.MultiLevel = false;
                    }

                    if (this.cbStores.Checked)
                    {
                        _OTSGlobalLockMethod.Stores = true;
                    }
                    else
                    {
                        _OTSGlobalLockMethod.Stores = false;
                    }

                    if (this.cbChain.Checked)
                    {
                        _OTSGlobalLockMethod.Chain = true;
                    }
                    else
                    {
                        _OTSGlobalLockMethod.Chain = false;
                    }
                }
				//End TT#43 - MD - DOConnell - Projected Sales Enhancement
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
                if (this.cbStores.Enabled && !this.cbStores.Checked && !this.cbChain.Enabled)
                {
                    methodFieldsValid = false;
                    //BEGIN TT#285 - MD - DOConnell - Global Lock Screen issues
                    //ErrorProvider.SetError(cbStores, SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_FieldIsRequired));
                    ErrorProvider.SetError(OptionsPanel, SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_FieldIsRequired));
                    //END TT#285 - MD - DOConnell - Global Lock Screen issues
					//BEGIN TT#295 - MD - DOConnell - Error Provider(s) do not get cleared when data is corrected
                    if (chkLstBxAttributeSet.CheckedItems.Count == 0)
                    {
                        methodFieldsValid = false;
                        ErrorProvider.SetError(chkLstBxAttributeSet, SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_FieldIsRequired));
                    }
                    else
                    {
                        ErrorProvider.SetError(chkLstBxAttributeSet, string.Empty);
                    }
                    //End TT#295 - MD - DOConnell - Error Provider(s) do not get cleared when data is corrected

                }
                else if (this.cbChain.Enabled && !this.cbChain.Checked && !this.cbStores.Enabled)
                {
                    methodFieldsValid = false;
                    //BEGIN TT#285 - MD - DOConnell - Global Lock Screen issues
                    //ErrorProvider.SetError(cbChain, SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_FieldIsRequired));
                    ErrorProvider.SetError(OptionsPanel, SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_FieldIsRequired));
                    //END TT#285 - MD - DOConnell - Global Lock Screen issues
                }
                else if ((this.cbChain.Enabled && !this.cbChain.Checked) &&
                         (this.cbStores.Enabled && !this.cbStores.Checked))
                {
                    methodFieldsValid = false;
                    //BEGIN TT#285 - MD - DOConnell - Global Lock Screen issues
                    //ErrorProvider.SetError(this.cbStores, SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_FieldIsRequired));
                    //ErrorProvider.SetError(cbChain, SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_FieldIsRequired));
                    ErrorProvider.SetError(OptionsPanel, SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_FieldIsRequired));
                    //END TT#285 - MD - DOConnell - Global Lock Screen issues

                    //Begin TT#295 - MD - DOConnell - Error Provider(s) do not get cleared when data is corrected
                    if (chkLstBxAttributeSet.CheckedItems.Count == 0)
                    {
                        methodFieldsValid = false;
                        ErrorProvider.SetError(chkLstBxAttributeSet, SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_FieldIsRequired));
                    }
                    else
                    {
                        ErrorProvider.SetError(chkLstBxAttributeSet, string.Empty);
                    }
                }
                else
                {
					//ErrorProvider.SetError(mdsPlanDateRange, string.Empty);
                    ErrorProvider.SetError(OptionsPanel, string.Empty); 
					//End TT#295 - MD - DOConnell - Error Provider(s) do not get cleared when data is corrected
                }

                if (txtSpreadNode.Text.Trim().Length == 0)
                {
                    methodFieldsValid = false;
                    ErrorProvider.SetError(txtSpreadNode, SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_FieldIsRequired));
                }
                else
                {
                    ErrorProvider.SetError(txtSpreadNode, string.Empty);
                }

                if ((Convert.ToInt32(cboSpreadVersion.SelectedValue, CultureInfo.CurrentUICulture) == Include.NoRID) || (cboSpreadVersion.SelectedValue == null))
                {
                    methodFieldsValid = false;
                    ErrorProvider.SetError(cboSpreadVersion, SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_FieldIsRequired));
                }
                else
                {
                    ErrorProvider.SetError(cboSpreadVersion, string.Empty);
                }

                if (mdsPlanDateRange.DateRangeRID == Include.UndefinedCalendarDateRange)
                {
                    methodFieldsValid = false;
                    ErrorProvider.SetError(mdsPlanDateRange, SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_FieldIsRequired));
                }
                else
                {
                    ErrorProvider.SetError(mdsPlanDateRange, string.Empty);
                }

                if (cboFromLevel.SelectedIndex == -1 || cboFromLevel.Items.Count == 0) //TT#736 - MD - Combobox causes NullReference - rbeck
                {
                    methodFieldsValid = false;
                    ErrorProvider.SetError(cboFromLevel, SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_pl_FromLevelsNotDefined));
                }
                else
                {
                    ErrorProvider.SetError(cboFromLevel, string.Empty);
                }

                if (cboToLevel.SelectedIndex == -1 || cboToLevel.Items.Count == 0) //TT#736 - MD - Combobox causes NullReference - rbeck
                {
                    methodFieldsValid = false;
                    ErrorProvider.SetError(cboToLevel, SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_pl_ToLevelsNotDefined));
                }
                else
                {
                    ErrorProvider.SetError(cboToLevel, string.Empty);
                }

                // Begin Track #4872 - JSmith - Global/User Attributes
                if (cboAttribute.SelectedIndex == Include.Undefined)
                {
                    methodFieldsValid = false;
                    ErrorProvider.SetError(cboAttribute, SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_FieldIsRequired));
                }
                else
                {
                    ErrorProvider.SetError(cboAttribute, string.Empty);
                }
                // End Track #4872

                //Begin TT#2589 - DOConnell - global lock not locking with filter selected
                if (cbStores.Checked)
                {
                    if (chkLstBxAttributeSet.CheckedItems.Count == 0)
                    {
                        methodFieldsValid = false;
                        ErrorProvider.SetError(chkLstBxAttributeSet, SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_FieldIsRequired));
                    }
                    else
                    {
                        ErrorProvider.SetError(chkLstBxAttributeSet, string.Empty);
                    }
                }
                //End TT#2589 - DOConnell - global lock not locking with filter selected
                return methodFieldsValid;
            }
            catch (Exception exception)
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
                ErrorProvider.SetError(txtName, WorkflowMethodNameMessage);
            }
            else
            {
                ErrorProvider.SetError(txtName, "");
            }
            if (!WorkflowMethodDescriptionValid)
            {
                ErrorProvider.SetError(txtDesc, WorkflowMethodDescriptionMessage);
            }
            else
            {
                ErrorProvider.SetError(txtDesc, "");
            }
            if (!UserGlobalValid)
            {
                ErrorProvider.SetError(pnlGlobalUser, UserGlobalMessage);
            }
            else
            {
                ErrorProvider.SetError(pnlGlobalUser, "");
            }
        }

        /// <summary>
        /// Use to set the specific method object before updating
        /// </summary>
        override protected void SetObject()
        {
            try
            {
			    //Begin TT#43 - MD - DOConnell - Projected Sales Enhancement

                if (_OTSGlobalUnlockMethod != null)
                {
                    ABM = _OTSGlobalUnlockMethod;
                }
                else
                {
                    ABM = _OTSGlobalLockMethod;
                }

			    //End TT#43 - MD - DOConnell - Projected Sales Enhancement
            }
            catch (Exception ex)
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


        #region Button Events
        // Begin MID Track 4858 - JSmith - Security changes
        //		private void btnProcess_Click(object sender, System.EventArgs e)
        //		{
        //			try
        //			{
        //				_basisNodeRequired = true;
        //				ProcessAction(eMethodType.GlobalUnlock);
        //				// as part of the  processing we saved the info, so it should be changed to update.
        //				if (!ErrorFound)
        //				{
        //					_OTSGlobalUnlockMethod.Method_Change_Type = eChangeType.update;
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
        //					_OTSGlobalUnlockMethod.Method_Change_Type = eChangeType.update;
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
			//Begin TT#43 - MD - DOConnell - Projected Sales Enhancement
                if (ABM.MethodType == eMethodType.GlobalUnlock)
                {
                    ProcessAction(eMethodType.GlobalUnlock);
                    // as part of the  processing we saved the info, so it should be changed to update.
                    if (!ErrorFound)
                    {
                        _OTSGlobalUnlockMethod.Method_Change_Type = eChangeType.update;
                        btnSave.Text = "&Update";

                        _OTSGlobalUnlockMethod.LastProcessedDateTime = Convert.ToString(DateTime.Now);
                        _OTSGlobalUnlockMethod.LastProcessedUser = SAB.ClientServerSession.GetUserName(SAB.ClientServerSession.UserRID);

                        lblDateTimeValue.Text = _OTSGlobalUnlockMethod.LastProcessedDateTime;
                        lblByUserValue.Text = _OTSGlobalUnlockMethod.LastProcessedUser;
                    }
                }
                else
                {
                    ProcessAction(eMethodType.GlobalLock);
                    // as part of the  processing we saved the info, so it should be changed to update.
                    if (!ErrorFound)
                    {
                        _OTSGlobalLockMethod.Method_Change_Type = eChangeType.update;
                        btnSave.Text = "&Update";

                        //BEGIN TT#335 - MD - DOConnell - Last Processed information incorrect on Global Lock and Unlock
                        _OTSGlobalLockMethod.LastProcessedDateTime = Convert.ToString(DateTime.Now);
                        _OTSGlobalLockMethod.LastProcessedUser = SAB.ClientServerSession.GetUserName(SAB.ClientServerSession.UserRID);
                        //END TT#335 - MD - DOConnell - Last Processed information incorrect on Global Lock and Unlock

                        lblDateTimeValue.Text = _OTSGlobalLockMethod.LastProcessedDateTime;
                        lblByUserValue.Text = _OTSGlobalLockMethod.LastProcessedUser;
                    }
                }
			//End TT#43 - MD - DOConnell - Projected Sales Enhancement
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        protected override void Call_btnSave_Click()
        {
            try
            {
                base.btnSave_Click();
            }
            catch (Exception exception)
            {
                HandleException(exception);
            }
        }
        // End MID Track 4858

        #endregion Button Events

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

            try
            {
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
                //            MIDTreeNode_cbd = (HierarchyClipboardData)cbp.ClipboardData;
                //            HierarchyNodeProfile hnp = SAB.HierarchyServerSession.GetNodeData(cbp.Key);
                //            _OTSGlobalUnlockMethod.HierNodeRID = cbp.Key;

                //            txtSpreadNode.Text = hnp.Text;
                //            txtSpreadNode.Tag = cbp.Key;

                //            PopulateFromToLevels(hnp, cboFromLevel, 0);
                //            PopulateFromToLevels(hnp, cboToLevel, 0);

                //            // Begin MID Track 5852 - KJohnson - Security changes
                //            ApplySecurity();
                //            // End MID Track 5852
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

                bool isSuccessfull = ((MIDTextBoxTag)(((TextBox)sender).Tag)).TextBox_DragDrop(sender, e);

                if (isSuccessfull)
                {
                    HierarchyNodeProfile hnp = (HierarchyNodeProfile)((MIDTag)(((TextBox)sender).Tag)).MIDTagData;
					//Begin TT#43 - MD - DOConnell - Projected Sales Enhancement
                    if (ABM.MethodType == eMethodType.GlobalUnlock)
                    {
                        _OTSGlobalUnlockMethod.HierNodeRID = hnp.Key;
                    }
                    else
                    {
                        _OTSGlobalLockMethod.HierNodeRID = hnp.Key;
                    }
					//End TT#43 - MD - DOConnell - Projected Sales Enhancement
                    txtSpreadNode.Text = hnp.Text;
                    //txtSpreadNode.Tag = hnp.Key;
					//BEGIN TT#4650 - DOConnell - Changes do not hold
					//PopulateFromToLevels(hnp, cboFromLevel.ComboBox, 0);
                	//PopulateFromToLevels(hnp, cboToLevel.ComboBox, 0);
                    PopulateFromToLevels(hnp, cboFromLevel, 0);
                    PopulateFromToLevels(hnp, cboToLevel, 0);
					//END TT#4650 - DOConnell - Changes do not hold

                    ApplySecurity();
                }
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
            _needsValidated = true;
        }

        private void txtSpreadNode_Leave(object sender, System.EventArgs e)
        {
        }

        private void txtSpreadNode_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //string productID;
            //string errorMessage;

            //try
            //{
            //    if (txtSpreadNode.Modified)
            //    {
            //        if (txtSpreadNode.Text.Trim().Length > 0)
            //        {
            //            productID = txtSpreadNode.Text.Trim();
            //            _nodeRID = GetNodeText(ref productID);
            //            if (_nodeRID == Include.NoRID)
            //            {
            //                // Begin MID Track 5855 - KJohnson - Type in invalid Merch catches it –but does not go back to the prior merch
            //                errorMessage = string.Format(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_InvalidTreeNode),
            //                    ((TextBox)sender).Text);
            //                ErrorProvider.SetError((TextBox)sender, errorMessage);
            //                MessageBox.Show(errorMessage);
            //                // End MID Track 5855
            //            }
            //            else
            //            {
            //                txtSpreadNode.Text = productID;
            //                txtSpreadNode.Tag = _nodeRID;
            //                _OTSGlobalUnlockMethod.HierNodeRID = _nodeRID;

            //                HierarchyNodeProfile hnp = SAB.HierarchyServerSession.GetNodeData(_nodeRID);
            //                PopulateFromToLevels(hnp, cboFromLevel, 0);
            //                PopulateFromToLevels(hnp, cboToLevel, 0);

            //                // Begin MID Track 5852 - KJohnson - Security changes
            //                ApplySecurity();
            //                // End MID Track 5852
            //            }
            //        }
            //        else
            //        {
            //            txtSpreadNode.Tag = null;
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
					//Begin TT#43 - MD - DOConnell - Projected Sales Enhancement
                    if (ABM.MethodType == eMethodType.GlobalUnlock)
                    {
                        _OTSGlobalUnlockMethod.HierNodeRID = _nodeRID;
                    }
                    else
                    {
                        _OTSGlobalLockMethod.HierNodeRID = _nodeRID;
                    }
					//End TT#43 - MD - DOConnell - Projected Sales Enhancement
					//BEGIN TT#4650 - DOConnell - Changes do not hold
					//PopulateFromToLevels(hnp, cboFromLevel.ComboBox, 0);
                    //PopulateFromToLevels(hnp, cboToLevel.ComboBox, 0);
                    PopulateFromToLevels(hnp, cboFromLevel, 0);
                    PopulateFromToLevels(hnp, cboToLevel, 0);
					//END TT#4650 - DOConnell - Changes do not hold

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

        private void cboOverride_SelectionChangeCommitted(object sender, System.EventArgs e)
        {
            if (FormLoaded)
            {
			//Begin TT#43 - MD - DOConnell - Projected Sales Enhancement
                if (ABM.MethodType == eMethodType.GlobalUnlock)
                {
                _OTSGlobalUnlockMethod.OverrideLowLevelRid = ((ComboObject)cboOverride.SelectedItem).Key;
                }
                else
                {
                    _OTSGlobalLockMethod.OverrideLowLevelRid = ((ComboObject)cboOverride.SelectedItem).Key;
                }
			//End TT#43 - MD - DOConnell - Projected Sales Enhancement
                ChangePending = true;
            }
        }
        //Begin TT#316 -jsobek -Replace all Windows Combobox controls with new enhanced control
        void cboOverride_MIDComboBoxPropertiesChangedEvent(object source, MIDComboBoxPropertiesChangedEventArgs args)
        {
            this.cboOverride_SelectionChangeCommitted(source, new EventArgs());
        }
        //End TT#316

        private int GetNodeText(ref string aProductID)
        {
            string desc = string.Empty;
            try
            {
                string productID = aProductID;
                string[] pArray = productID.Split(new char[] { '[' });
                productID = pArray[0].Trim();
                //				HierarchyNodeProfile hnp = SAB.HierarchyServerSession.GetNodeData(productID);
                HierarchyMaintenance hm = new HierarchyMaintenance(SAB);
                EditMsgs em = new EditMsgs();
                HierarchyNodeProfile hnp = hm.NodeLookup(ref em, productID, false);
                if (hnp.Key == Include.NoRID)
                    return Include.NoRID;
                else
                {
                    aProductID = hnp.Text;
                    return hnp.Key;
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
                return Include.NoRID;
            }
        }

        private void cboSpreadVersion_SelectionChangeCommitted(object sender, System.EventArgs e)
        {
            if (FormLoaded)
            {
                if (!_setting_cboSpreadVersion)
                {
                    ChangePending = true;

                    _parentVersion = Convert.ToInt32(cboSpreadVersion.SelectedValue, CultureInfo.CurrentUICulture);
					//Begin TT#43 - MD - DOConnell - Projected Sales Enhancement
                    if (ABM.MethodType == eMethodType.GlobalUnlock)
                    {
                        this._OTSGlobalUnlockMethod.VersionRID = _parentVersion;
                    }
                    else
                    {
                        this._OTSGlobalLockMethod.VersionRID = _parentVersion;
                    }
					//End TT#43 - MD - DOConnell - Projected Sales Enhancement
                    // Begin MID Track 5852 - KJohnson - Security changes
                    ApplySecurity();
                    // End MID Track 5852
                }
            }
			//Begin TT#43 - MD - DOConnell - Projected Sales Enhancement
            //else if (_OTSGlobalUnlockMethod.Method_Change_Type == eChangeType.add)
            else if (ABM.Method_Change_Type == eChangeType.add)
            {
                if (ABM.MethodType == eMethodType.GlobalUnlock)
                {
                _OTSGlobalUnlockMethod.VersionRID = Convert.ToInt32(cboSpreadVersion.SelectedValue, CultureInfo.CurrentUICulture);
                }
                else
                {
                    _OTSGlobalLockMethod.VersionRID = Convert.ToInt32(cboSpreadVersion.SelectedValue, CultureInfo.CurrentUICulture);
                }
			//End TT#43 - MD - DOConnell - Projected Sales Enhancement
            }
        }
        //Begin TT#316 -jsobek -Replace all Windows Combobox controls with new enhanced control
        void cboSpreadVersion_MIDComboBoxPropertiesChangedEvent(object source, MIDComboBoxPropertiesChangedEventArgs args)
        {
            this.cboSpreadVersion_SelectionChangeCommitted(source, new EventArgs());
        }
        //End TT#316

        // Begin Track #4872 - JSmith - Global/User Attributes
        override protected void BuildAttributeList()
        {
            ProfileList pl;
            int currValue;
            try
            {
                if (cboAttribute.SelectedValue != null &&
                    cboAttribute.SelectedValue.GetType() == typeof(System.Int32))
                {
                    currValue = Convert.ToInt32(cboAttribute.SelectedValue);
                }
                else
                {
                    currValue = Include.NoRID;
                }
				//Begin TT#43 - MD - DOConnell - Projected Sales Enhancement
                if (ABM.MethodType == eMethodType.GlobalUnlock)
                {
                    pl = GetStoreGroupList(_OTSGlobalUnlockMethod.Method_Change_Type, _OTSGlobalUnlockMethod.GlobalUserType, false);
                    cboAttribute.Initialize(SAB, FunctionSecurity, pl.ArrayList, _OTSGlobalUnlockMethod.GlobalUserType == eGlobalUserType.User);
                }
                else
                {
                    pl = GetStoreGroupList(_OTSGlobalLockMethod.Method_Change_Type, _OTSGlobalLockMethod.GlobalUserType, false);
                    cboAttribute.Initialize(SAB, FunctionSecurity, pl.ArrayList, _OTSGlobalLockMethod.GlobalUserType == eGlobalUserType.User);
                }
				//End TT#43 - MD - DOConnell - Projected Sales Enhancement
                if (currValue != Include.NoRID)
                {
                    cboAttribute.SelectedValue = currValue;
                }

                AdjustTextWidthComboBox_DropDown(cboAttribute);     //TT#7 - RBeck - Dynamic dropdowns
            }
            catch
            {
                throw;
            }
        }
        // End Track #4872

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

            // Begin MID Track 5852 - KJohnson - Security changes
            ApplySToreChainRBSecurity();
            // End MID Track 5852

            // Begin Track #5858 - JSmith - Validating security
            if (this.cbStores.Checked && this.cbChain.Checked)
            {
                securityOk = base.ValidatePlanNodeSecurity(txtSpreadNode, true, eSecurityTypes.Store);
                if (securityOk)
                {
                    securityOk = base.ValidatePlanNodeSecurity(txtSpreadNode, true, eSecurityTypes.Chain);
                }

            }
            else if (this.cbChain.Checked)
            {
                securityOk = base.ValidatePlanNodeSecurity(txtSpreadNode, true, eSecurityTypes.Chain);
            }
            else if (this.cbStores.Checked)
            {
                securityOk = base.ValidatePlanNodeSecurity(txtSpreadNode, true, eSecurityTypes.Store);
            }
            else 
            {
                securityOk = base.ValidatePlanNodeSecurity(txtSpreadNode, true, eSecurityTypes.Store | eSecurityTypes.Chain);
            }
            // End Track #5858

            if (securityOk)
            {
                // BEGIN Issue 5852
                if (this.cbStores.Checked && this.cbChain.Checked)
                {
                    securityOk = base.ValidatePlanVersionSecurity(cboSpreadVersion.ComboBox, true, ePlanSelectType.Store);
                    if (securityOk)
                    {
                        securityOk = base.ValidatePlanVersionSecurity(cboSpreadVersion.ComboBox, true, ePlanSelectType.Chain);
                    }

                }
                else if (this.cbChain.Checked)
                {
                    securityOk = base.ValidatePlanVersionSecurity(cboSpreadVersion.ComboBox, true, ePlanSelectType.Chain);
                }
                else if (this.cbStores.Checked)
                {
                    securityOk = base.ValidatePlanVersionSecurity(cboSpreadVersion.ComboBox, true, ePlanSelectType.Store);
                }
                else 
                {
                    securityOk = base.ValidatePlanVersionSecurity(cboSpreadVersion.ComboBox, true, ePlanSelectType.Chain | ePlanSelectType.Store);
                }
                // END Issue 5852
            }

            bool canUpdate = ABM.AuthorizedToUpdate(this.SAB.ClientServerSession, this.SAB.ClientServerSession.UserRID);
            base.ApplyCanUpdate(canUpdate);

            return securityOk;	// Track 5871
        }

        #endregion
		//private void PopulateFromToLevels(HierarchyNodeProfile aHierarchyNodeProfile, ComboBox aComboBox, int toOffset) //TT#4650 
        private void PopulateFromToLevels(HierarchyNodeProfile aHierarchyNodeProfile, MIDComboBoxEnh aComboBox, int toOffset)
        {
            try
            {
                HierarchyProfile hierProf;
				
				//object oldSelectedItem = aComboBox.SelectedItem; //TT#4650 
                //aComboBox.Items.Clear(); //TT#4650 
                object oldSelectedItem = aComboBox.ComboBox.SelectedItem; //TT#4650 
                aComboBox.ComboBox.Items.Clear(); //TT#4650 
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
				//if (aComboBox.Name == "cboFromLevel") //TT#4650 
                if (aComboBox.ComboBox.Name == "cboFromLevel") //TT#4650 
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
							    //if (aComboBox.Name == "cboFromLevel") //TT#4650 
                                if (aComboBox.ComboBox.Name == "cboFromLevel") //TT#4650 
                                {
                                    aComboBox.Items.Add(
                                        new FromLevelCombo(eFromLevelsType.HierarchyLevel,
                                        0,
                                        0,
                                        hierProf.HierarchyID));
                                }
                                else
                                {
									//if (cboFromLevel.Items.Count > 0) //TT#4650 
                                    if (cboFromLevel.ComboBox.Items.Count > 0) //TT#4650 
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
                    //BEGIN TT#4650 - DOConnell - Changes do not hold
                    else if (hierProf.HierarchyType == eHierarchyType.alternate)
                    {
                        HierarchyProfile mainHierProf = SAB.HierarchyServerSession.GetMainHierarchyData();

                        int highestGuestLevel = SAB.HierarchyServerSession.GetHighestGuestLevel(aHierarchyNodeProfile.Key);

                        // add offsets to comboBox
                        DataTable hierarchyLevels = SAB.HierarchyServerSession.GetHierarchyDescendantLevels(aHierarchyNodeProfile.Key);
                        int longestBranchCount = hierarchyLevels.Rows.Count;
                        if (aComboBox.ComboBox.Name == "cboFromLevel")   
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
                            if (aComboBox.ComboBox.Name == "cboFromLevel") 
                            {
                                aComboBox.ComboBox.Items.Add(
                                new FromLevelCombo(eFromLevelsType.LevelOffset,
                                offset,
                                0,
                                null));
                            }
                            else
                            {
                                aComboBox.ComboBox.Items.Add(
                                new ToLevelCombo(eToLevelsType.LevelOffset,
                                offset,
                                0,
                                null));
                            }
                        }
                    }
                    //END TT#4650 - DOConnell - Changes do not hold
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
						//BEGIN TT#4650 - DOConnell - Changes do not hold
                        //int longestBranchCount = SAB.HierarchyServerSession.GetLongestBranch(aHierarchyNodeProfile.Key); 
                        DataTable hierarchyDescendantLevels = SAB.HierarchyServerSession.GetHierarchyDescendantLevels(aHierarchyNodeProfile.Key); //TT#4650 

                        int longestBranchCount = hierarchyDescendantLevels.Rows.Count;
						//END TT#4650 - DOConnell - Changes do not hold
						
                        if (aComboBox.ComboBox.Name == "cboFromLevel")
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
							//if (aComboBox.Name == "cboFromLevel") 
                            if (aComboBox.ComboBox.Name == "cboFromLevel") //TT#4650 
                            {
								//aComboBox.Items.Add( //TT#4650 
                                aComboBox.ComboBox.Items.Add( //TT#4650 
                                new FromLevelCombo(eFromLevelsType.LevelOffset,
                                offset,
                                0,
                                null));
                            }
                            else
                            {
								//aComboBox.Items.Add(
                                aComboBox.ComboBox.Items.Add( //TT#4650 
                                new ToLevelCombo(eToLevelsType.LevelOffset,
                                offset,
                                0,
                                null));
                            }
                        }
                    }
                }
				
				//if (aComboBox.Items.Count > 0)
                if (aComboBox.ComboBox.Items.Count > 0) //TT#4650
                {
                    if (toOffset > 0)
                    {
						//int count = aComboBox.Items.Count;
                        int count = aComboBox.ComboBox.Items.Count; //TT#4650
                        for (int i = 0; i < toOffset; i++)
                        {
							// aComboBox.Items.RemoveAt(0);
                            aComboBox.ComboBox.Items.RemoveAt(0); //TT#4650
                        }
						
						//if (oldSelectedItem != null && aComboBox.Items.IndexOf(oldSelectedItem) > -1)
                        if (oldSelectedItem != null && aComboBox.ComboBox.Items.IndexOf(oldSelectedItem) > -1) //TT#4650
                        {
							//aComboBox.SelectedIndex = aComboBox.Items.IndexOf(oldSelectedItem);
                            aComboBox.ComboBox.SelectedIndex = aComboBox.ComboBox.Items.IndexOf(oldSelectedItem); //TT#4650
                        }
                        else
                        {
							//aComboBox.SelectedIndex = 0;
                            aComboBox.ComboBox.SelectedIndex = 0; //TT#4650
                        }

                    }
                    else
                    {
						//if (oldSelectedItem != null && aComboBox.Items.IndexOf(oldSelectedItem) > -1 && aComboBox.Name == "cboToLevel")
                        if (oldSelectedItem != null && aComboBox.ComboBox.Items.IndexOf(oldSelectedItem) > -1 && aComboBox.ComboBox.Name == "cboToLevel") //TT#4650
                        {
							//aComboBox.SelectedIndex = aComboBox.Items.IndexOf(oldSelectedItem);
                            aComboBox.ComboBox.SelectedIndex = aComboBox.ComboBox.Items.IndexOf(oldSelectedItem); //TT#4650
                        }
                        else
                        {
							//aComboBox.SelectedIndex = 0;
                            aComboBox.ComboBox.SelectedIndex = 0; //TT#4650
                        }
                    }

                }
                

				// BEGIN Issue 5907 stodd
				if (FormLoaded)
				{

					//Begin TT#43 - MD - DOConnell - Projected Sales Enhancement
                    if (ABM.MethodType == eMethodType.GlobalUnlock)
                    {
					// BEGIN Issue 5871 stodd
						//if (aComboBox.Name == "cboFromLevel") 
                        if (aComboBox.ComboBox.Name == "cboFromLevel") //TT#4650
                        {
							//BEGIN TT#4650 - DOConnell - Changes do not hold
							//_OTSGlobalUnlockMethod.FromLevelType = ((FromLevelCombo)aComboBox.SelectedItem).FromLevelType;
                            //_OTSGlobalUnlockMethod.FromLevelOffset = ((FromLevelCombo)aComboBox.SelectedItem).FromLevelOffset;
                            //_OTSGlobalUnlockMethod.FromLevelSequence = ((FromLevelCombo)aComboBox.SelectedItem).FromLevelSequence;
							
                            _OTSGlobalUnlockMethod.FromLevelType = ((FromLevelCombo)aComboBox.ComboBox.SelectedItem).FromLevelType;
                            _OTSGlobalUnlockMethod.FromLevelOffset = ((FromLevelCombo)aComboBox.ComboBox.SelectedItem).FromLevelOffset;
                            _OTSGlobalUnlockMethod.FromLevelSequence = ((FromLevelCombo)aComboBox.ComboBox.SelectedItem).FromLevelSequence;
                        	//END TT#4650 - DOConnell - Changes do not hold
						}
                        else
                        {
							//BEGIN TT#4650 - DOConnell - Changes do not hold
							//_OTSGlobalUnlockMethod.ToLevelType = ((ToLevelCombo)aComboBox.SelectedItem).ToLevelType;
                            //_OTSGlobalUnlockMethod.ToLevelOffset = ((ToLevelCombo)aComboBox.SelectedItem).ToLevelOffset;
                            //_OTSGlobalUnlockMethod.ToLevelSequence = ((ToLevelCombo)aComboBox.SelectedItem).ToLevelSequence;
							
                            _OTSGlobalUnlockMethod.ToLevelType = ((ToLevelCombo)aComboBox.ComboBox.SelectedItem).ToLevelType;
                            _OTSGlobalUnlockMethod.ToLevelOffset = ((ToLevelCombo)aComboBox.ComboBox.SelectedItem).ToLevelOffset;
                            _OTSGlobalUnlockMethod.ToLevelSequence = ((ToLevelCombo)aComboBox.ComboBox.SelectedItem).ToLevelSequence;
							//END TT#4650 - DOConnell - Changes do not hold
                        }
					// END Issue 5871 stodd
                    }
                    else
                    {
						//if (aComboBox.Name == "cboFromLevel")
                        if (aComboBox.ComboBox.Name == "cboFromLevel") //TT#4650
                        {
							//BEGIN TT#4650 - DOConnell - Changes do not hold
							//_OTSGlobalLockMethod.FromLevelType = ((FromLevelCombo)aComboBox.SelectedItem).FromLevelType;
                            //_OTSGlobalLockMethod.FromLevelOffset = ((FromLevelCombo)aComboBox.SelectedItem).FromLevelOffset;
                            //_OTSGlobalLockMethod.FromLevelSequence = ((FromLevelCombo)aComboBox.SelectedItem).FromLevelSequence;
                            
							_OTSGlobalLockMethod.FromLevelType = ((FromLevelCombo)aComboBox.ComboBox.SelectedItem).FromLevelType;
                            _OTSGlobalLockMethod.FromLevelOffset = ((FromLevelCombo)aComboBox.ComboBox.SelectedItem).FromLevelOffset;
                            _OTSGlobalLockMethod.FromLevelSequence = ((FromLevelCombo)aComboBox.ComboBox.SelectedItem).FromLevelSequence;
							//END TT#4650 - DOConnell - Changes do not hold
                        }
                        else
                        {
							//BEGIN TT#4650 - DOConnell - Changes do not hold
							//_OTSGlobalLockMethod.ToLevelType = ((ToLevelCombo)aComboBox.SelectedItem).ToLevelType;
                            //_OTSGlobalLockMethod.ToLevelOffset = ((ToLevelCombo)aComboBox.SelectedItem).ToLevelOffset;
                            //_OTSGlobalLockMethod.ToLevelSequence = ((ToLevelCombo)aComboBox.SelectedItem).ToLevelSequence;
						
                            _OTSGlobalLockMethod.ToLevelType = ((ToLevelCombo)aComboBox.ComboBox.SelectedItem).ToLevelType;
                            _OTSGlobalLockMethod.ToLevelOffset = ((ToLevelCombo)aComboBox.ComboBox.SelectedItem).ToLevelOffset;
                            _OTSGlobalLockMethod.ToLevelSequence = ((ToLevelCombo)aComboBox.ComboBox.SelectedItem).ToLevelSequence;
							//END TT#4650 - DOConnell - Changes do not hold
                        }
                    }
					//End TT#43 - MD - DOConnell - Projected Sales Enhancement
				}
				// END Issue 5907 stodd
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
                CalendarDateSelector frm = (CalendarDateSelector)CreateControl(typeof(CalendarDateSelector), new object[] { SAB });
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
			//Begin TT#43 - MD - DOConnell - Projected Sales Enhancement
                if (ABM.MethodType == eMethodType.GlobalUnlock)
                {
                    GetOTSPLANWorkflows(_OTSGlobalUnlockMethod.Key, ugWorkflows);
                }
                else
                {
                    GetOTSPLANWorkflows(_OTSGlobalLockMethod.Key, ugWorkflows);
                }
			//End TT#43 - MD - DOConnell - Projected Sales Enhancement
            }
            catch (Exception ex)
            {
                HandleException(ex, "LoadWorkflows");
            }
        }
        #endregion

        private void cboFromLevel_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (FormLoaded)
            {
                ChangePending = true;

                if (txtSpreadNode.Tag != null)
                {
				//Begin TT#43 - MD - DOConnell - Projected Sales Enhancement
                    HierarchyNodeProfile hnp;
                    if (ABM.MethodType == eMethodType.GlobalUnlock)
                    {
                         hnp = SAB.HierarchyServerSession.GetNodeData(_OTSGlobalUnlockMethod.HierNodeRID);
                    }
                    else
                    {
                         hnp = SAB.HierarchyServerSession.GetNodeData(_OTSGlobalLockMethod.HierNodeRID);
                    }
				//End TT#43 - MD - DOConnell - Projected Sales Enhancement
					//Begin TT#4649 - DOConnell - Multi Level From Level does not hold after change
					//PopulateFromToLevels(hnp, cboToLevel.ComboBox, cboFromLevel.SelectedIndex);
                    PopulateFromToLevels(hnp, cboToLevel, cboFromLevel.SelectedIndex);
					//End TT#4649 - DOConnell - Multi Level From Level does not hold after change 
                    // BEGIN Issue 5871 stodd
                    ApplySecurity();
                    // END Issue 5871 stodd
                }
            }
        }
        //Begin TT#316 -jsobek -Replace all Windows Combobox controls with new enhanced control
        void cboFromLevel_MIDComboBoxPropertiesChangedEvent(object source, MIDComboBoxPropertiesChangedEventArgs args)
        {
            this.cboFromLevel_SelectionChangeCommitted(source, new EventArgs());
        }
        //End TT#316

        private void cboToLevel_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (FormLoaded)
            {
                ChangePending = true;

                if (txtSpreadNode.Tag != null)
                {
				//Begin TT#43 - MD - DOConnell - Projected Sales Enhancement    
                    if (ABM.MethodType == eMethodType.GlobalUnlock)
                    {
					 // BEGIN Issue 5871 stodd
                    _OTSGlobalUnlockMethod.ToLevelType = ((ToLevelCombo)cboToLevel.SelectedItem).ToLevelType;
                    _OTSGlobalUnlockMethod.ToLevelOffset = ((ToLevelCombo)cboToLevel.SelectedItem).ToLevelOffset;
                    _OTSGlobalUnlockMethod.ToLevelSequence = ((ToLevelCombo)cboToLevel.SelectedItem).ToLevelSequence;
					 // END Issue 5871 stodd
                    }
                    else
                    {
                        _OTSGlobalLockMethod.ToLevelType = ((ToLevelCombo)cboToLevel.SelectedItem).ToLevelType;
                        _OTSGlobalLockMethod.ToLevelOffset = ((ToLevelCombo)cboToLevel.SelectedItem).ToLevelOffset;
                        _OTSGlobalLockMethod.ToLevelSequence = ((ToLevelCombo)cboToLevel.SelectedItem).ToLevelSequence;
                    }
                    ApplySecurity();
				//End TT#43 - MD - DOConnell - Projected Sales Enhancement
                }
            }
        }
        //Begin TT#316 -jsobek -Replace all Windows Combobox controls with new enhanced control
        void cboToLevel_MIDComboBoxPropertiesChangedEvent(object source, MIDComboBoxPropertiesChangedEventArgs args)
        {
            this.cboToLevel_SelectionChangeCommitted(source, new EventArgs());
        }
        //End TT#316

        private void cboFilter_SelectionChangeCommitted(object sender, System.EventArgs e)
        {
            if (FormLoaded)
            {
                ChangePending = true;
            }

            // Begin TT#2669 - JSmith - Unable to remove a filter from a wokflow
            if (cboFilter.SelectedIndex != -1)
            {
                if (((FilterNameCombo)cboFilter.SelectedItem).FilterRID == Include.Undefined)
                {
                    cboFilter.SelectedIndex = -1;
                }
            }
            // End TT#2669 - JSmith - Unable to remove a filter from a wokflow
        }
        //Begin TT#316 -jsobek -Replace all Windows Combobox controls with new enhanced control
        void cboFilter_MIDComboBoxPropertiesChangedEvent(object source, MIDComboBoxPropertiesChangedEventArgs args)
        {
            this.cboFilter_SelectionChangeCommitted(source, new EventArgs());
        }
        //End TT#316

        private void cbMultiLevel_CheckedChanged(object sender, System.EventArgs e)
        {
            if (FormLoaded)
            {
                ChangePending = true;

                if (this.cbMultiLevel.Checked)
                {
                    this.cboFromLevel.Enabled = true;
                    //this.cboToLevel.Enabled = true;
                    //this.txtOverride.Enabled = true;
                    //this.btnOverride.Enabled = true;
                }
                else
                {
                    this.cboFromLevel.Enabled = false;
                    //this.cboToLevel.Enabled = false;
                    //this.txtOverride.Enabled = false;
                    //this.btnOverride.Enabled = false;
                }
                //} TT#4649 - DOConnell - Multi Level From Level does not hold after change

                //Begin TT#43 - MD - DOConnell - Projected Sales Enhancement 
                if (ABM.MethodType == eMethodType.GlobalUnlock)
                {
                    if (_OTSGlobalUnlockMethod.HierNodeRID > 0)
                    {
                        HierarchyNodeProfile hnp = SAB.HierarchyServerSession.GetNodeData(_OTSGlobalUnlockMethod.HierNodeRID);
                        txtSpreadNode.Text = hnp.Text;
                        //Begin Track #5858 - JSmith - Validating store security only
                        //txtSpreadNode.Tag = hnp.Key;
                        ((MIDTag)txtSpreadNode.Tag).MIDTagData = hnp;
                        //End Track #5858
						//BEGIN TT#4650 - DOConnell - Changes do not hold
						//PopulateFromToLevels(hnp, cboFromLevel.ComboBox, 0);
                        //PopulateFromToLevels(hnp, cboToLevel.ComboBox, 0);
                        PopulateFromToLevels(hnp, cboFromLevel, 0);
                        PopulateFromToLevels(hnp, cboToLevel, 0);
						//END TT#4650 - DOConnell - Changes do not hold

                    }
                }
                else
                {
                    if (_OTSGlobalLockMethod.HierNodeRID > 0)
                    {
                        HierarchyNodeProfile hnp = SAB.HierarchyServerSession.GetNodeData(_OTSGlobalLockMethod.HierNodeRID);
                        txtSpreadNode.Text = hnp.Text;
                        //Begin Track #5858 - JSmith - Validating store security only
                        //txtSpreadNode.Tag = hnp.Key;
                        ((MIDTag)txtSpreadNode.Tag).MIDTagData = hnp;
                        //End Track #5858
						//BEGIN TT#4650 - DOConnell - Changes do not hold
                        //PopulateFromToLevels(hnp, cboFromLevel.ComboBox, 0);
                        //PopulateFromToLevels(hnp, cboToLevel.ComboBox, 0);
                        PopulateFromToLevels(hnp, cboFromLevel, 0);
                        PopulateFromToLevels(hnp, cboToLevel, 0);
						//END TT#4650 - DOConnell - Changes do not hold
                    }
                }
                //End TT#43 - MD - DOConnell - Projected Sales Enhancement 
            } //TT#4649 - DOConnell - Multi Level From Level does not hold after change
        }

        private void cbStores_CheckedChanged(object sender, EventArgs e)
        {
            DialogResult dr = DialogResult.Yes;

            if (!cbStoresCheckedChanging)
            {
                ChangePending = true;

                if (((CheckBox)sender).Checked == true && !FunctionSecurity.IsReadOnly)
                {
                    this.cboAttribute.Enabled = true;
                    this.cboFilter.Enabled = true;
                    this.chkLstBxAttributeSet.Enabled = true;
                    // Begin Track #4872 - JSmith - Global/User Attributes
                    //PopulateStoreAttributeSet(this.cboAttribute.SelectedValue.ToString());
                    if (cboAttribute.SelectedValue != null)
                    {
                        PopulateStoreAttributeSet(this.cboAttribute.SelectedValue.ToString());
                    }
                    // End Track #4872
                }
                else
                {
                    this.cboAttribute.Enabled = false;
                    this.cboFilter.Enabled = false;
                    this.chkLstBxAttributeSet.Enabled = false;

                    if ((this.chkLstBxAttributeSet.CheckedItems.Count > 0) && FormLoaded)
                    {
					//Begin TT#43 - MD - DOConnell - Projected Sales Enhancement 
                        if (ABM.MethodType == eMethodType.GlobalUnlock)
                        {
                            dr = MessageBox.Show("This will delete the check's that are curently set in the Multi Value Attributes check boxes below.  Are you sure you want to continue?",
                                MIDText.GetTextOnly(eMIDTextCode.frm_GlobalUnlock), MessageBoxButtons.YesNo);

                            if (dr == DialogResult.Yes)
                            {
                                _OTSGlobalUnlockMethod.SGL_RID_List = null;
                                for (int i = 0; i < this.chkLstBxAttributeSet.Items.Count; i++)
                                {
                                    this.chkLstBxAttributeSet.SetItemCheckState(i, CheckState.Unchecked);
                                }
                            }
                            else
                            {
                                cbStoresCheckedChanging = true;
                                ((CheckBox)sender).Checked = true;
                                this.cboAttribute.Enabled = true;
                            this.cboFilter.Enabled = true;
                                this.chkLstBxAttributeSet.Enabled = true;
                                cbStoresCheckedChanging = false;
                            }
                        }
                        else
                        {
                            dr = MessageBox.Show("This will delete the check's that are curently set in the Multi Value Attributes check boxes below.  Are you sure you want to continue?",
                                MIDText.GetTextOnly(eMIDTextCode.frm_GlobalLock), MessageBoxButtons.YesNo);

                            if (dr == DialogResult.Yes)
                            {
                                _OTSGlobalLockMethod.SGL_RID_List = null;
                                for (int i = 0; i < this.chkLstBxAttributeSet.Items.Count; i++)
                                {
                                    this.chkLstBxAttributeSet.SetItemCheckState(i, CheckState.Unchecked);
                                }
                            }
                            else
                            {
                                cbStoresCheckedChanging = true;
                                ((CheckBox)sender).Checked = true;
                                this.cboAttribute.Enabled = true;
                                this.cboFilter.Enabled = true;
                                this.chkLstBxAttributeSet.Enabled = true;
                                cbStoresCheckedChanging = false;
                            }
                        }
                    }
				//End TT#43 - MD - DOConnell - Projected Sales Enhancement 
                }
				// Begin MID Track 5852 - stodd - Security changes
				if (FormLoaded)
				{
				//Begin TT#43 - MD - DOConnell - Projected Sales Enhancement 
                    if (ABM.MethodType == eMethodType.GlobalUnlock)
                    {
                        _OTSGlobalUnlockMethod.Stores = cbStores.Checked;
                    }
                    else
                    {
                        _OTSGlobalLockMethod.Stores = cbStores.Checked;
                    }
				//End TT#43 - MD - DOConnell - Projected Sales Enhancement 
				}
				// End MID Track 5852 - stodd - Security changes
                // Begin MID Track 5852 - KJohnson - Security changes

                //BEGIN TT#295 - MD - DOConnell - Error Provider(s) do not get cleared when data is corrected
                if (!cbStores.Checked && !cbChain.Checked)
                {
                    ErrorProvider.SetError(OptionsPanel, SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_FieldIsRequired));

                    chkLstBxAttributeSet.Items.Clear();
                    ErrorProvider.SetError(chkLstBxAttributeSet, string.Empty);
                }
                else if (!cbStores.Checked && cbChain.Checked)
                {
                    ErrorProvider.SetError(OptionsPanel, string.Empty);
                    chkLstBxAttributeSet.Items.Clear();
                    ErrorProvider.SetError(chkLstBxAttributeSet, string.Empty);
                }
                else
                {
                    ErrorProvider.SetError(OptionsPanel, string.Empty);
                    if (chkLstBxAttributeSet.Items.Count == 0)
                        PopulateStoreAttributeSet(this.cboAttribute.SelectedValue.ToString());
                    if (this.cbStores.Enabled && this.cbStores.Checked)
                    {
                        if (chkLstBxAttributeSet.CheckedItems.Count == 0)
                        {
                            ErrorProvider.SetError(chkLstBxAttributeSet, SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_FieldIsRequired));
                        }
                        else
                        {
                            ErrorProvider.SetError(chkLstBxAttributeSet, string.Empty);
                        }
                    }
                }
                //END TT#295 - MD - DOConnell - Error Provider(s) do not get cleared when data is corrected

                ApplySecurity();
                BindVersionComboBoxes();
                // End MID Track 5852
            }
        }

        private void cbChain_CheckedChanged(object sender, EventArgs e)
        {
            if (FormLoaded)
            {
                ChangePending = true;
				//Begin TT#43 - MD - DOConnell - Projected Sales Enhancement 
                if (ABM.MethodType == eMethodType.GlobalUnlock)
                {
					// Begin MID Track 5852 - stodd - Security changes
                    _OTSGlobalUnlockMethod.Chain = cbChain.Checked;
					// End MID Track 5852
                }
                else
                {
                    _OTSGlobalLockMethod.Chain = cbChain.Checked;
                }
				//End TT#43 - MD - DOConnell - Projected Sales Enhancement 
				//Begin TT#295 - MD - DOConnell - Error Provider(s) do not get cleared when data is corrected
                if (!cbStores.Checked && !cbChain.Checked)
                {
                    ErrorProvider.SetError(OptionsPanel, SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_FieldIsRequired));

                    chkLstBxAttributeSet.Items.Clear();
                    ErrorProvider.SetError(chkLstBxAttributeSet, string.Empty);
                }
                else if (!cbStores.Checked && cbChain.Checked)
                {
                    ErrorProvider.SetError(OptionsPanel, string.Empty);
                    chkLstBxAttributeSet.Items.Clear();
                    ErrorProvider.SetError(chkLstBxAttributeSet, string.Empty);
                }
                else
                {
                    ErrorProvider.SetError(OptionsPanel, string.Empty);
                    if (chkLstBxAttributeSet.Items.Count == 0)
                        PopulateStoreAttributeSet(this.cboAttribute.SelectedValue.ToString());
                    if (cbStores.Enabled && cbStores.Checked)
                    {
                        if (chkLstBxAttributeSet.CheckedItems.Count == 0)
                        {
                            ErrorProvider.SetError(chkLstBxAttributeSet, SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_FieldIsRequired));
                        }
                        else
                        {
                            ErrorProvider.SetError(chkLstBxAttributeSet, string.Empty);
                        }
                    }
                }
                //END TT#295 - MD - DOConnell - Error Provider(s) do not get cleared when data is corrected

                // Begin MID Track 5852 - KJohnson - Security changes
                ApplySecurity(); 
                BindVersionComboBoxes();
                // End MID Track 5852
            }
        }

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
					//Begin TT#43 - MD - DOConnell - Projected Sales Enhancement
                    if (ABM.MethodType == eMethodType.GlobalUnlock)
                    {
                        args = new object[] { SAB, _OTSGlobalUnlockMethod.OverrideLowLevelRid, _OTSGlobalUnlockMethod.HierNodeRID, _OTSGlobalUnlockMethod.VersionRID, lowLevelText, _OTSGlobalUnlockMethod.CustomOLL_RID, methodSecurity };
                    }
                    else
                    {
                        args = new object[] { SAB, _OTSGlobalLockMethod.OverrideLowLevelRid, _OTSGlobalLockMethod.HierNodeRID, _OTSGlobalLockMethod.VersionRID, lowLevelText, _OTSGlobalLockMethod.CustomOLL_RID, methodSecurity };
                    }
					// End Track #5909 - stodd
					//End TT#43 - MD - DOConnell - Projected Sales Enhancement

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
				//Begin TT#43 - MD - DOConnell - Projected Sales Enhancement
                    if (ABM.MethodType == eMethodType.GlobalUnlock)
                    {
                        if (_OTSGlobalUnlockMethod.OverrideLowLevelRid != e.aOllRid)
                            ChangePending = true;

                        _OTSGlobalUnlockMethod.OverrideLowLevelRid = e.aOllRid;

                        if (_OTSGlobalUnlockMethod.CustomOLL_RID != e.aCustomOllRid)
                        {
                            _OTSGlobalUnlockMethod.CustomOLL_RID = e.aCustomOllRid;
                            UpdateMethodCustomOLLRid(_OTSGlobalUnlockMethod.Key, _OTSGlobalUnlockMethod.CustomOLL_RID);
                        }

                        //Begin tt#700 - APicchetti - forecast methods show the method has changed when user copens Override Low Level model and then clicks close
                        if (_overrideLowLevelfrm.DialogResult != DialogResult.Cancel)
                        {
                        LoadOverrideModelComboBox(cboOverride.ComboBox, e.aOllRid, _OTSGlobalUnlockMethod.CustomOLL_RID);
                        }

                        _overrideLowLevelfrm = null;
                        // End tt#700

                        //LoadOverrideModelComboBox(cboOverride, e.aOllRid, _OTSGlobalUnlockMethod.CustomOLL_RID);
                    }
                    else
                    {
                        if (_OTSGlobalLockMethod.OverrideLowLevelRid != e.aOllRid)
                            ChangePending = true;

                        _OTSGlobalLockMethod.OverrideLowLevelRid = e.aOllRid;

                        if (_OTSGlobalLockMethod.CustomOLL_RID != e.aCustomOllRid)
                        {
                            _OTSGlobalLockMethod.CustomOLL_RID = e.aCustomOllRid;
                            UpdateMethodCustomOLLRid(_OTSGlobalLockMethod.Key, _OTSGlobalLockMethod.CustomOLL_RID);
                        }

                        //Begin tt#700 - APicchetti - forecast methods show the method has changed when user copens Override Low Level model and then clicks close
                        if (_overrideLowLevelfrm.DialogResult != DialogResult.Cancel)
                        {
                            LoadOverrideModelComboBox(cboOverride.ComboBox, e.aOllRid, _OTSGlobalLockMethod.CustomOLL_RID);
                        }

                        _overrideLowLevelfrm = null;
                        // End tt#700
                    }
				//End TT#43 - MD - DOConnell - Projected Sales Enhancement
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

        //        if (aAlwaysCreateNewForm || !foundForm)
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

        private void mniClearAll_Click(object sender, System.EventArgs e)
        {
            int i;
            try
            {
                for (i = 0; i < chkLstBxAttributeSet.Items.Count; i++)
                {
                    chkLstBxAttributeSet.SetItemChecked(i, false);
                }
            }
            catch (Exception exc)
            {
                HandleException(exc);
            }
        }

        private void mniSelectAll_Click(object sender, System.EventArgs e)
        {
            int i;
            try
            {
                for (i = 0; i < chkLstBxAttributeSet.Items.Count; i++)
                {
                    chkLstBxAttributeSet.SetItemChecked(i, true);
                }
            }
            catch (Exception exc)
            {
                HandleException(exc);
            }
        }

        private void mniRestoreDefaults_Click(object sender, System.EventArgs e)
        {
            try
            {
			//Begin TT#43 - MD - DOConnell - Projected Sales Enhancement
                if (ABM.MethodType == eMethodType.GlobalUnlock)
                {
			//End TT#43 - MD - DOConnell - Projected Sales Enhancement
                    if (_OTSGlobalUnlockMethod.SGL_RID_List != null)
                    {
                        for (int i = 0; i < this.chkLstBxAttributeSet.Items.Count; i++)
                        {
                            this.chkLstBxAttributeSet.SetItemCheckState(i, CheckState.Unchecked);
                            for (int j = 0; j < _OTSGlobalUnlockMethod.SGL_RID_List.Count; j++)
                            {
                                StoreGroupLevelListViewProfile sglProfileItem = (StoreGroupLevelListViewProfile)this.chkLstBxAttributeSet.Items[i];
                                if (sglProfileItem.Key == Convert.ToInt32(_OTSGlobalUnlockMethod.SGL_RID_List[j]))
                                {
                                    this.chkLstBxAttributeSet.SetItemCheckState(i, CheckState.Checked);
                                    break;
			//Begin TT#43 - MD - DOConnell - Projected Sales Enhancement
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (_OTSGlobalLockMethod.SGL_RID_List != null)
                    {
                        for (int i = 0; i < this.chkLstBxAttributeSet.Items.Count; i++)
                        {
                            this.chkLstBxAttributeSet.SetItemCheckState(i, CheckState.Unchecked);
                            for (int j = 0; j < _OTSGlobalLockMethod.SGL_RID_List.Count; j++)
                            {
                                StoreGroupLevelListViewProfile sglProfileItem = (StoreGroupLevelListViewProfile)this.chkLstBxAttributeSet.Items[i];
                                if (sglProfileItem.Key == Convert.ToInt32(_OTSGlobalLockMethod.SGL_RID_List[j]))
                                {
                                    this.chkLstBxAttributeSet.SetItemCheckState(i, CheckState.Checked);
                                    break;
                                }
			//End TT#43 - MD - DOConnell - Projected Sales Enhancement
                            }
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                HandleException(exc);
            }
        }

        private void chkLstBxAttributeSet_ItemCheck(object sender, System.Windows.Forms.ItemCheckEventArgs e)
        {
            if (FormLoaded)
            {
                ChangePending = true;
                //Begin TT#295 - MD - DOConnell - Error Provider(s) do not get cleared when data is corrected
                ErrorProvider.SetError(chkLstBxAttributeSet, string.Empty);
                //End TT#295 - MD - DOConnell - Error Provider(s) do not get cleared when data is corrected
            }
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

        // Begin Track #4872 - JSmith - Global/User Attributes
        private void frmGlobalUnlockMethod_Load(object sender, EventArgs e)
        {
            if (cboAttribute.ReplaceAttribute)
            {
                ChangePending = true;
            }
        }

        private void cboAttribute_DragEnter(object sender, DragEventArgs e)
        {
            Image_DragEnter(sender, e);
        }

        private void cboAttribute_DragOver(object sender, DragEventArgs e)
        {
            Image_DragOver(sender, e);
        }

        private void cboFilter_DragEnter(object sender, DragEventArgs e)
        {
            Image_DragEnter(sender, e);
        }

        private void cboFilter_DragOver(object sender, DragEventArgs e)
        {
            Image_DragOver(sender, e);
        }

        private void cboFilter_DragDrop(object sender, DragEventArgs e)
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

        // End Track #4872
    }
}

