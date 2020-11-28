using System;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Globalization;
using System.Configuration;

using MIDRetail.Business;
using MIDRetail.Common;
using MIDRetail.DataCommon;
using MIDRetail.Data;
using MIDRetail.Windows.Controls;

namespace MIDRetail.Windows
{
	/// <summary>
	/// Summary description for HierarchyProperties.
	/// </summary>
	/// 
	
	public class frmHierarchyProperties : MIDFormBase
	{
		// add event to update explorer when hierarchy is changed
		public delegate void HierPropertyChangeEventHandler(object source, HierPropertyChangeEventArgs e);
		public event HierPropertyChangeEventHandler OnHierPropertyChangeHandler;

		private DataTable _OTSTypeDataTable = null;
		private DataTable _displayOptionDataTable = null;
		private DataTable _IDFormatDataTable = null;
//		private bool _newHierarchy = false;
//		private HierarchyNodeSecurityProfile _nodeSecurity;
//		private bool ChangePending = false;
		private bool _nameChanged = false;
		private bool _levelDisplayOptionChanged = false;
		private bool _levelChangePending = false;
		private bool _folderColorChanged = false;
//		private bool _cancelPressed = false;
		private bool _colorDefined = false;
		private bool _sizeDefined = false;
		private bool _loadingLevel = true;
		private bool _loadingWindow = false;
//		private bool FormLoaded = false;
		private bool _changeEventThrown = false;
		private bool _purgeDailyHistoryChanged = false;
		private bool _purgeWeeklyHistoryChanged = false;
		private bool _purgePlansChanged = false;
        //BEGIN TT#400-MD-VStuart-Add Header Purge Criteria by Header Type
        //private bool _purgeHeadersChanged = false;
        private bool _htASNChanged = false;
        private bool _htDropShipChanged = false;
        private bool _htDummyChanged = false;
        private bool _htPurchaseChanged = false;
        private bool _htReceiptChanged = false;
        private bool _htReserveChanged = false;
        private bool _htVSWChanged = false;
        private bool _htWorkupTotByChanged = false;
        //END TT#400-MD-VStuart-Add Header Purge Criteria by Header Type
		private MIDHierarchyNode _node;
		private string _errors = null;
		private string _lblMIDLevels = null;
		private string _lblNewHierarchy = null;
		private string _lblNewPersonalHierarchy = null;
		private string _lblNewNode = null;
		private string _lblNewLevelName = null;
		private string _lblCodeColor = null;
		private HierarchyProfile _hp;
		private int _buttonWidth = 25, _buttonHeight = 25;
		private int _arrowWidth = 20, _arrowHeight = 20;
		private HierarchyListViewItem _selectedItem = null;
		private int _currentLevelIndex = -1;
		private int _selImageIndex;
		private string _selLevelColor;
		private eHierarchyDisplayOptions _selDisplayOption;
		private int _currentDisplayOptionIndex = -1;
		private eHierarchyIDFormat _selIDFormat;
		private int _currentIDFormatIndex = -1;
		private string _selColorFile;
		private bool _upArrowClick = false;
		private bool _downArrowClick = false;
		private int _defaultImageIndex;
		private string _defaultImageFileName;
		private eHierarchyLevelType _currentLevelType;
		private int _currentOTSIndex = -1;
//Begin Track #3863 - JScott - OTS Forecast Level Defaults
		private bool _checkBoxChanging;
//End Track #3863 - JScott - OTS Forecast Level Defaults

		private System.Windows.Forms.TextBox txtHierarchyPropertyName;
		private System.Windows.Forms.PictureBox picHierarchyPropertyColor;
		private System.Windows.Forms.GroupBox gbxHierarchyType;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnHelp;
		private System.Windows.Forms.Label lblName;
		private System.Windows.Forms.GroupBox gbxLevels;
		private System.Windows.Forms.ListView lstHierarchyLevels;
		private System.Windows.Forms.Button btnNewLevel;
		private System.Windows.Forms.Button btnDeleteLevel;
		private System.Windows.Forms.PictureBox picUpArrow;
		private System.Windows.Forms.PictureBox picDownArrow;
		private System.Windows.Forms.GroupBox gbxLevelProperties;
        private System.Windows.Forms.TextBox txtLevelName;
		private System.Windows.Forms.PictureBox picLevelColor;
		private System.Windows.Forms.Button btnUpdate;
		private System.Windows.Forms.RadioButton radOpen;
		private System.Windows.Forms.RadioButton radOrganizational;
        private System.Windows.Forms.Label _lblNewLevelNameName;
        private System.Windows.Forms.Label lblPostingDate;
		private System.Windows.Forms.RadioButton rdoHierarchyOTSTypeTotal;
		private System.Windows.Forms.RadioButton rdoHierarchyOTSTypeRegular;
		private System.Windows.Forms.GroupBox gbxHierarchyOTSType;
		private System.Windows.Forms.GroupBox gbxHierarchyRollupOption;
		private System.Windows.Forms.RadioButton rdoHierarchyRollupOptionAPI;
		private System.Windows.Forms.RadioButton rdoHierarchyRollupOptionRealTime;
        private Button btnApply;
        private TabControl tabControl1;
        private TabPage tabProfile;
        private GroupBox gbxOTSType;
        private CheckBox chkOTSTypeTotal;
        private CheckBox chkOTSTypeRegular;
        private MIDComboBoxEnh cboIDFormat;
        private Label lblIDOption;
        private GroupBox gbxLevelNameLength;
        private NumericUpDown numRangeTo;
        private Label txtRangeTo;
        private NumericUpDown numRangeFrom;
        private RadioButton radRange;
        private NumericUpDown numRequiredSize;
        private RadioButton radRequiredSize;
        private RadioButton radUnrestricted;
        private MIDComboBoxEnh cboDisplayOption;
        private Label lblDisplayOption;
        private Label lblOTSType;
        private MIDComboBoxEnh cboOTSType;
        private TabPage tabPurgeCriteria;
        private Label lblHtReceipt;
        private Label lblHtPurchaseOrder;
        private Label lblHtDummy;
        private Label lblHtDropShip;
        private Label lblHtASN;
        private TextBox txtPurgePlans;
        private TextBox txtPurgeWeeklyHistory;
        private TextBox txtPurgeDailyHistory;
        private Label lblPurgePlansTimeframe;
        private Label lblPurgeWeeklyHistoryTimeframe;
        private Label lblPurgeDailyHistoryTimeframe;
        private Label lblPurgeWeeklyHistory;
        private Label lblPurgePlans;
        private Label lblPurgeDailyHistory;
        private Label lblHtWorkUpTot;
        private Label lblHtVSW;
        private Label lblHtReserve;
        private Label lblHtWorkUpTotTimeframe;
        private Label lblHtVSWTimeframe;
        private Label lblHtReserveTimeframe;
        private Label lblHtPurchaseOrderTimeframe;
        private Label lblHtReceiptTimeframe;
        private Label lblHtDummyTimeframe;
        private Label lblHtDropShipTimeframe;
        private Label lblHtASNTimeframe;
        private TextBox txtHtVSW;
        private TextBox txtHtReserve;
        private TextBox txtHtPurchase;
        private TextBox txtHtReceipt;
        private TextBox txtHtDummy;
        private TextBox txtHtDropShip;
        private TextBox txtHtASN;
        private TextBox txtHtWorkupTotBy;
        private System.ComponentModel.IContainer components;

		public int HierarchyRID 
		{
			get { return _hp.Key; }
		}

		public frmHierarchyProperties(SessionAddressBlock aSAB) : base (aSAB)
		{
//Begin Track #3863 - JScott - OTS Forecast Level Defaults
			DataTable dtTempType;
			DataRow newRow;
//End Track #3863 - JScott - OTS Forecast Level Defaults
			try
			{
			    //
			    // Required for Windows Form Designer support
			    //
			    InitializeComponent();

			    this.lstHierarchyLevels.View = View.Details;
			    // Allow the user to edit item text.
			    this.lstHierarchyLevels.LabelEdit = false;
			    // Allow the user to rearrange columns.
			    this.lstHierarchyLevels.AllowColumnReorder = false;
			    // Display check boxes.
			    this.lstHierarchyLevels.CheckBoxes = false;
			    // Select the item and subitems when selection is made.
			    this.lstHierarchyLevels.FullRowSelect = false;
			    // Display grid lines.
			    this.lstHierarchyLevels.GridLines = false;
			    // Do not allow sorting
			    this.lstHierarchyLevels.Sorting = SortOrder.None;
			    // Not allow multi selection
			    this.lstHierarchyLevels.MultiSelect = false;
			    // Set the name of list
			    _defaultImageFileName = GraphicsDirectory + "\\" + MIDGraphics.DefaultClosedFolder;
			    _defaultImageIndex = MIDGraphics.ImageIndex(MIDGraphics.DefaultClosedFolder);
			    displayUpArrow(GraphicsDirectory + "\\" + MIDGraphics.UpArrow);
			    displayDownArrow(GraphicsDirectory + "\\" + MIDGraphics.DownArrow);
//Begin Track #3863 - JScott - OTS Forecast Level Defaults
//				_OTSTypeDataTable = MIDText.GetTextType(eMIDTextType.eHierarchyLevelType, eMIDTextOrderBy.TextValue);
			    dtTempType = MIDText.GetTextType(eMIDTextType.eHierarchyLevelType, eMIDTextOrderBy.TextValue);
			    _OTSTypeDataTable = dtTempType.Clone();

			    foreach (DataRow row in dtTempType.Rows)
			    {
			        switch ((eHierarchyLevelMasterType) Convert.ToInt32(row["TEXT_CODE"], CultureInfo.CurrentUICulture))
			        {
			            case eHierarchyLevelMasterType.Color:
			            case eHierarchyLevelMasterType.Size:
			            case eHierarchyLevelMasterType.Style:
			            case eHierarchyLevelMasterType.Undefined:
			                newRow = _OTSTypeDataTable.NewRow();
			                newRow.ItemArray = row.ItemArray;
			                _OTSTypeDataTable.Rows.Add(newRow);
			                break;
			        }
			    }
//End Track #3863 - JScott - OTS Forecast Level Defaults

				_displayOptionDataTable = MIDText.GetTextType(eMIDTextType.eHierarchyDisplayOptions, eMIDTextOrderBy.TextCode);
				_IDFormatDataTable = MIDText.GetTextType(eMIDTextType.eHierarchyIDFormat, eMIDTextOrderBy.TextValue);
				SetText();
				btnApply.Enabled = false;
			}
			catch( Exception exception )
			{
				HandleException(exception);
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

				this.txtHierarchyPropertyName.TextChanged -= new System.EventHandler(this.txtHierarchyPropertyName_TextChanged);
				this.txtHierarchyPropertyName.Leave -= new System.EventHandler(this.txtHierarchyPropertyName_Leave);
				this.radOpen.CheckedChanged -= new System.EventHandler(this.radOpen_CheckedChanged);
				this.radOrganizational.CheckedChanged -= new System.EventHandler(this.radOrganizational_CheckedChanged);
				this.picHierarchyPropertyColor.MouseHover -= new System.EventHandler(this.picFolderColor_MouseHover);
				this.picHierarchyPropertyColor.DoubleClick -= new System.EventHandler(this.picHierarchyPropertyColor_DoubleClick);
				this.btnOK.Click -= new System.EventHandler(this.btnOK_Click);
				this.btnCancel.Click -= new System.EventHandler(this.btnCancel_Click);
				this.btnUpdate.Click -= new System.EventHandler(this.btnUpdate_Click);
				this.picLevelColor.DoubleClick -= new System.EventHandler(this.picLevelColor_DoubleClick);
				this.picLevelColor.MouseHover -= new System.EventHandler(this.picFolderColor_MouseHover);
				this.txtLevelName.TextChanged -= new System.EventHandler(this.txtLevelName_TextChanged);
				this.cboIDFormat.SelectionChangeCommitted -= new System.EventHandler(this.cboIDFormat_SelectionChangeCommitted);
				this.radRange.CheckedChanged -= new System.EventHandler(this.radRange_CheckedChanged);
				this.radRequiredSize.CheckedChanged -= new System.EventHandler(this.radRequiredSize_CheckedChanged);
				this.radUnrestricted.CheckedChanged -= new System.EventHandler(this.radUnrestricted_CheckedChanged);
				this.cboDisplayOption.SelectionChangeCommitted -= new System.EventHandler(this.cboDisplayOption_SelectionChangeCommitted);
//Begin Track #3863 - JScott - OTS Forecast Level Defaults
//				this.radOTSPlvlRegular.CheckedChanged -= new System.EventHandler(this.radOTSPlvlRegular_CheckedChanged);
//				this.radOTSPlvlTotal.CheckedChanged -= new System.EventHandler(this.radOTSPlvlTotal_CheckedChanged);
				this.chkOTSTypeTotal.CheckedChanged -= new System.EventHandler(this.chkOTSTypeTotal_CheckedChanged);
				this.chkOTSTypeRegular.CheckedChanged -= new System.EventHandler(this.chkOTSTypeRegular_CheckedChanged);
				this.rdoHierarchyOTSTypeTotal.CheckedChanged -= new System.EventHandler(this.rdoHierarchyOTSTypeTotal_CheckedChanged);
				this.rdoHierarchyOTSTypeRegular.CheckedChanged -= new System.EventHandler(this.rdoHierarchyOTSTypeRegular_CheckedChanged);
				this.numRangeTo.ValueChanged -= new System.EventHandler(this.numRangeTo_ValueChanged);
				this.numRangeFrom.ValueChanged -= new System.EventHandler(this.numRangeFrom_ValueChanged);
				this.numRequiredSize.ValueChanged -= new System.EventHandler(this.numRequiredSize_ValueChanged);
//End Track #3863 - JScott - OTS Forecast Level Defaults
//Begin - JScott - Add Rollup Type to Hierarchy Properties - Part 2
				this.rdoHierarchyRollupOptionRealTime.CheckedChanged -= new System.EventHandler(this.rdoHierarchyRollupOptionRealTime_CheckedChanged);
				this.rdoHierarchyRollupOptionAPI.CheckedChanged -= new System.EventHandler(this.rdoHierarchyRollupOptionAPI_CheckedChanged);
//End - JScott - Add Rollup Type to Hierarchy Properties - Part 2
				this.cboOTSType.SelectionChangeCommitted -= new System.EventHandler(this.cboOTSType_SelectionChangeCommitted);
                //BEGIN TT#400-MD-VStuart-Add Header Purge Criteria by Header Type
                //this.txtPurgeHeaders.TextChanged -= new System.EventHandler(this.txtPurgeHeaders_TextChanged);
                //END TT#400-MD-VStuart-Add Header Purge Criteria by Header Type
				this.txtPurgePlans.TextChanged -= new System.EventHandler(this.txtPurgePlans_TextChanged);
				this.txtPurgeWeeklyHistory.TextChanged -= new System.EventHandler(this.txtPurgeWeeklyHistory_TextChanged);
				this.txtPurgeDailyHistory.TextChanged -= new System.EventHandler(this.txtPurgeDailyHistory_TextChanged);
				this.picDownArrow.Click -= new System.EventHandler(this.picDownArrow_Click);
				this.picUpArrow.Click -= new System.EventHandler(this.picUpArrow_Click);
				this.btnDeleteLevel.Click -= new System.EventHandler(this.btnDeleteLevel_Click);
				this.btnNewLevel.Click -= new System.EventHandler(this.btnNewLevel_Click);
				this.lstHierarchyLevels.SelectedIndexChanged -= new System.EventHandler(this.lstHierarchyLevels_SelectedIndexChanged);
                //BEGIN TT#400-MD-VStuart-Add Header Purge Criteria by Header Type
                txtHtASN.TextChanged -= txtHtASN_TextChanged;
                txtHtDropShip.TextChanged -= txtHtDropShip_TextChanged;
                txtHtDummy.TextChanged -= txtHtDummy_TextChanged;
                txtHtPurchase.TextChanged -= txtHtPurchase_TextChanged;
                txtHtReceipt.TextChanged -= txtHtReceipt_TextChanged;
                txtHtReserve.TextChanged -= txtHtReserve_TextChanged;
                txtHtVSW.TextChanged -= txtHtVSW_TextChanged;
                txtHtWorkupTotBy.TextChanged -= txtHtWorkupTotBy_TextChanged;
                //END TT#400-MD-VStuart-Add Header Purge Criteria by Header Type
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
            this.lblName = new System.Windows.Forms.Label();
            this.txtHierarchyPropertyName = new System.Windows.Forms.TextBox();
            this.gbxHierarchyType = new System.Windows.Forms.GroupBox();
            this.radOpen = new System.Windows.Forms.RadioButton();
            this.radOrganizational = new System.Windows.Forms.RadioButton();
            this.picHierarchyPropertyColor = new System.Windows.Forms.PictureBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnHelp = new System.Windows.Forms.Button();
            this.gbxLevels = new System.Windows.Forms.GroupBox();
            this.gbxLevelProperties = new System.Windows.Forms.GroupBox();
            this._lblNewLevelNameName = new System.Windows.Forms.Label();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.picLevelColor = new System.Windows.Forms.PictureBox();
            this.txtLevelName = new System.Windows.Forms.TextBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabProfile = new System.Windows.Forms.TabPage();
            this.gbxOTSType = new System.Windows.Forms.GroupBox();
            this.chkOTSTypeTotal = new System.Windows.Forms.CheckBox();
            this.chkOTSTypeRegular = new System.Windows.Forms.CheckBox();
            this.cboIDFormat = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this.lblIDOption = new System.Windows.Forms.Label();
            this.gbxLevelNameLength = new System.Windows.Forms.GroupBox();
            this.numRangeTo = new System.Windows.Forms.NumericUpDown();
            this.txtRangeTo = new System.Windows.Forms.Label();
            this.numRangeFrom = new System.Windows.Forms.NumericUpDown();
            this.radRange = new System.Windows.Forms.RadioButton();
            this.numRequiredSize = new System.Windows.Forms.NumericUpDown();
            this.radRequiredSize = new System.Windows.Forms.RadioButton();
            this.radUnrestricted = new System.Windows.Forms.RadioButton();
            this.cboDisplayOption = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this.lblDisplayOption = new System.Windows.Forms.Label();
            this.lblOTSType = new System.Windows.Forms.Label();
            this.cboOTSType = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this.tabPurgeCriteria = new System.Windows.Forms.TabPage();
            this.lblHtWorkUpTotTimeframe = new System.Windows.Forms.Label();
            this.txtHtWorkupTotBy = new System.Windows.Forms.TextBox();
            this.lblHtVSWTimeframe = new System.Windows.Forms.Label();
            this.lblHtReserveTimeframe = new System.Windows.Forms.Label();
            this.lblHtPurchaseOrderTimeframe = new System.Windows.Forms.Label();
            this.lblHtReceiptTimeframe = new System.Windows.Forms.Label();
            this.lblHtDummyTimeframe = new System.Windows.Forms.Label();
            this.lblHtDropShipTimeframe = new System.Windows.Forms.Label();
            this.lblHtASNTimeframe = new System.Windows.Forms.Label();
            this.txtHtVSW = new System.Windows.Forms.TextBox();
            this.txtHtReserve = new System.Windows.Forms.TextBox();
            this.txtHtPurchase = new System.Windows.Forms.TextBox();
            this.txtHtReceipt = new System.Windows.Forms.TextBox();
            this.txtHtDummy = new System.Windows.Forms.TextBox();
            this.txtHtDropShip = new System.Windows.Forms.TextBox();
            this.txtHtASN = new System.Windows.Forms.TextBox();
            this.lblHtWorkUpTot = new System.Windows.Forms.Label();
            this.lblHtVSW = new System.Windows.Forms.Label();
            this.lblHtReserve = new System.Windows.Forms.Label();
            this.lblHtReceipt = new System.Windows.Forms.Label();
            this.lblHtPurchaseOrder = new System.Windows.Forms.Label();
            this.lblHtDummy = new System.Windows.Forms.Label();
            this.lblHtDropShip = new System.Windows.Forms.Label();
            this.lblHtASN = new System.Windows.Forms.Label();
            this.txtPurgePlans = new System.Windows.Forms.TextBox();
            this.txtPurgeWeeklyHistory = new System.Windows.Forms.TextBox();
            this.txtPurgeDailyHistory = new System.Windows.Forms.TextBox();
            this.lblPurgePlansTimeframe = new System.Windows.Forms.Label();
            this.lblPurgeWeeklyHistoryTimeframe = new System.Windows.Forms.Label();
            this.lblPurgeDailyHistoryTimeframe = new System.Windows.Forms.Label();
            this.lblPurgeWeeklyHistory = new System.Windows.Forms.Label();
            this.lblPurgePlans = new System.Windows.Forms.Label();
            this.lblPurgeDailyHistory = new System.Windows.Forms.Label();
            this.picDownArrow = new System.Windows.Forms.PictureBox();
            this.picUpArrow = new System.Windows.Forms.PictureBox();
            this.btnDeleteLevel = new System.Windows.Forms.Button();
            this.btnNewLevel = new System.Windows.Forms.Button();
            this.lstHierarchyLevels = new System.Windows.Forms.ListView();
            this.lblPostingDate = new System.Windows.Forms.Label();
            this.gbxHierarchyOTSType = new System.Windows.Forms.GroupBox();
            this.rdoHierarchyOTSTypeTotal = new System.Windows.Forms.RadioButton();
            this.rdoHierarchyOTSTypeRegular = new System.Windows.Forms.RadioButton();
            this.gbxHierarchyRollupOption = new System.Windows.Forms.GroupBox();
            this.rdoHierarchyRollupOptionAPI = new System.Windows.Forms.RadioButton();
            this.rdoHierarchyRollupOptionRealTime = new System.Windows.Forms.RadioButton();
            this.btnApply = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.utmMain)).BeginInit();
            this.gbxHierarchyType.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picHierarchyPropertyColor)).BeginInit();
            this.gbxLevels.SuspendLayout();
            this.gbxLevelProperties.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picLevelColor)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabProfile.SuspendLayout();
            this.gbxOTSType.SuspendLayout();
            this.gbxLevelNameLength.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numRangeTo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numRangeFrom)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numRequiredSize)).BeginInit();
            this.tabPurgeCriteria.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picDownArrow)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picUpArrow)).BeginInit();
            this.gbxHierarchyOTSType.SuspendLayout();
            this.gbxHierarchyRollupOption.SuspendLayout();
            this.SuspendLayout();
            // 
            // utmMain
            // 
            this.utmMain.MenuSettings.ForceSerialization = true;
            this.utmMain.ToolbarSettings.ForceSerialization = true;
            // 
            // lblName
            // 
            this.lblName.Location = new System.Drawing.Point(104, 15);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(48, 23);
            this.lblName.TabIndex = 0;
            this.lblName.Text = "Name:";
            this.lblName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtHierarchyPropertyName
            // 
            this.txtHierarchyPropertyName.Location = new System.Drawing.Point(160, 16);
            this.txtHierarchyPropertyName.Name = "txtHierarchyPropertyName";
            this.txtHierarchyPropertyName.Size = new System.Drawing.Size(216, 20);
            this.txtHierarchyPropertyName.TabIndex = 1;
            this.txtHierarchyPropertyName.TextChanged += new System.EventHandler(this.txtHierarchyPropertyName_TextChanged);
            this.txtHierarchyPropertyName.Leave += new System.EventHandler(this.txtHierarchyPropertyName_Leave);
            // 
            // gbxHierarchyType
            // 
            this.gbxHierarchyType.Controls.Add(this.radOpen);
            this.gbxHierarchyType.Controls.Add(this.radOrganizational);
            this.gbxHierarchyType.Location = new System.Drawing.Point(32, 48);
            this.gbxHierarchyType.Name = "gbxHierarchyType";
            this.gbxHierarchyType.Size = new System.Drawing.Size(224, 48);
            this.gbxHierarchyType.TabIndex = 2;
            this.gbxHierarchyType.TabStop = false;
            this.gbxHierarchyType.Text = "Type";
            // 
            // radOpen
            // 
            this.radOpen.Location = new System.Drawing.Point(120, 16);
            this.radOpen.Name = "radOpen";
            this.radOpen.Size = new System.Drawing.Size(96, 24);
            this.radOpen.TabIndex = 3;
            this.radOpen.Text = "Alternate";
            this.radOpen.CheckedChanged += new System.EventHandler(this.radOpen_CheckedChanged);
            // 
            // radOrganizational
            // 
            this.radOrganizational.Location = new System.Drawing.Point(8, 16);
            this.radOrganizational.Name = "radOrganizational";
            this.radOrganizational.Size = new System.Drawing.Size(112, 24);
            this.radOrganizational.TabIndex = 2;
            this.radOrganizational.Text = "Organizational";
            this.radOrganizational.CheckedChanged += new System.EventHandler(this.radOrganizational_CheckedChanged);
            // 
            // picHierarchyPropertyColor
            // 
            this.picHierarchyPropertyColor.Location = new System.Drawing.Point(72, 15);
            this.picHierarchyPropertyColor.Name = "picHierarchyPropertyColor";
            this.picHierarchyPropertyColor.Size = new System.Drawing.Size(24, 23);
            this.picHierarchyPropertyColor.TabIndex = 6;
            this.picHierarchyPropertyColor.TabStop = false;
            this.picHierarchyPropertyColor.DoubleClick += new System.EventHandler(this.picHierarchyPropertyColor_DoubleClick);
            this.picHierarchyPropertyColor.MouseHover += new System.EventHandler(this.picFolderColor_MouseHover);
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(377, 529);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 21;
            this.btnOK.Text = "OK";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(463, 529);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 22;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnHelp
            // 
            this.btnHelp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnHelp.Location = new System.Drawing.Point(16, 529);
            this.btnHelp.Name = "btnHelp";
            this.btnHelp.Size = new System.Drawing.Size(24, 23);
            this.btnHelp.TabIndex = 9;
            this.btnHelp.Text = "?";
            // 
            // gbxLevels
            // 
            this.gbxLevels.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbxLevels.Controls.Add(this.gbxLevelProperties);
            this.gbxLevels.Controls.Add(this.picDownArrow);
            this.gbxLevels.Controls.Add(this.picUpArrow);
            this.gbxLevels.Controls.Add(this.btnDeleteLevel);
            this.gbxLevels.Controls.Add(this.btnNewLevel);
            this.gbxLevels.Controls.Add(this.lstHierarchyLevels);
            this.gbxLevels.Location = new System.Drawing.Point(32, 102);
            this.gbxLevels.Name = "gbxLevels";
            this.gbxLevels.Size = new System.Drawing.Size(592, 417);
            this.gbxLevels.TabIndex = 4;
            this.gbxLevels.TabStop = false;
            this.gbxLevels.Text = "Level Information";
            // 
            // gbxLevelProperties
            // 
            this.gbxLevelProperties.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.gbxLevelProperties.Controls.Add(this._lblNewLevelNameName);
            this.gbxLevelProperties.Controls.Add(this.btnUpdate);
            this.gbxLevelProperties.Controls.Add(this.picLevelColor);
            this.gbxLevelProperties.Controls.Add(this.txtLevelName);
            this.gbxLevelProperties.Controls.Add(this.tabControl1);
            this.gbxLevelProperties.Location = new System.Drawing.Point(224, 24);
            this.gbxLevelProperties.Name = "gbxLevelProperties";
            this.gbxLevelProperties.Size = new System.Drawing.Size(352, 384);
            this.gbxLevelProperties.TabIndex = 7;
            this.gbxLevelProperties.TabStop = false;
            this.gbxLevelProperties.Text = "Level Properties";
            // 
            // _lblNewLevelNameName
            // 
            this._lblNewLevelNameName.Location = new System.Drawing.Point(64, 24);
            this._lblNewLevelNameName.Name = "_lblNewLevelNameName";
            this._lblNewLevelNameName.Size = new System.Drawing.Size(48, 23);
            this._lblNewLevelNameName.TabIndex = 4;
            this._lblNewLevelNameName.Text = "Name:";
            this._lblNewLevelNameName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnUpdate
            // 
            this.btnUpdate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnUpdate.Location = new System.Drawing.Point(136, 352);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(75, 23);
            this.btnUpdate.TabIndex = 20;
            this.btnUpdate.Text = "Update";
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // picLevelColor
            // 
            this.picLevelColor.Location = new System.Drawing.Point(24, 24);
            this.picLevelColor.Name = "picLevelColor";
            this.picLevelColor.Size = new System.Drawing.Size(32, 24);
            this.picLevelColor.TabIndex = 1;
            this.picLevelColor.TabStop = false;
            this.picLevelColor.DoubleClick += new System.EventHandler(this.picLevelColor_DoubleClick);
            this.picLevelColor.MouseHover += new System.EventHandler(this.picFolderColor_MouseHover);
            // 
            // txtLevelName
            // 
            this.txtLevelName.Location = new System.Drawing.Point(120, 24);
            this.txtLevelName.Name = "txtLevelName";
            this.txtLevelName.Size = new System.Drawing.Size(184, 20);
            this.txtLevelName.TabIndex = 7;
            this.txtLevelName.TextChanged += new System.EventHandler(this.txtLevelName_TextChanged);
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.tabControl1.Controls.Add(this.tabProfile);
            this.tabControl1.Controls.Add(this.tabPurgeCriteria);
            this.tabControl1.Location = new System.Drawing.Point(24, 56);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(304, 288);
            this.tabControl1.TabIndex = 8;
            // 
            // tabProfile
            // 
            this.tabProfile.Controls.Add(this.gbxOTSType);
            this.tabProfile.Controls.Add(this.cboIDFormat);
            this.tabProfile.Controls.Add(this.lblIDOption);
            this.tabProfile.Controls.Add(this.gbxLevelNameLength);
            this.tabProfile.Controls.Add(this.cboDisplayOption);
            this.tabProfile.Controls.Add(this.lblDisplayOption);
            this.tabProfile.Controls.Add(this.lblOTSType);
            this.tabProfile.Controls.Add(this.cboOTSType);
            this.tabProfile.Location = new System.Drawing.Point(4, 22);
            this.tabProfile.Name = "tabProfile";
            this.tabProfile.Size = new System.Drawing.Size(296, 262);
            this.tabProfile.TabIndex = 0;
            this.tabProfile.Text = "Profile";
            // 
            // gbxOTSType
            // 
            this.gbxOTSType.Controls.Add(this.chkOTSTypeTotal);
            this.gbxOTSType.Controls.Add(this.chkOTSTypeRegular);
            this.gbxOTSType.Location = new System.Drawing.Point(16, 104);
            this.gbxOTSType.Name = "gbxOTSType";
            this.gbxOTSType.Size = new System.Drawing.Size(256, 40);
            this.gbxOTSType.TabIndex = 11;
            this.gbxOTSType.TabStop = false;
            this.gbxOTSType.Text = "OTS Type";
            // 
            // chkOTSTypeTotal
            // 
            this.chkOTSTypeTotal.Location = new System.Drawing.Point(176, 8);
            this.chkOTSTypeTotal.Name = "chkOTSTypeTotal";
            this.chkOTSTypeTotal.Size = new System.Drawing.Size(56, 24);
            this.chkOTSTypeTotal.TabIndex = 1;
            this.chkOTSTypeTotal.Text = "Total";
            this.chkOTSTypeTotal.CheckedChanged += new System.EventHandler(this.chkOTSTypeTotal_CheckedChanged);
            // 
            // chkOTSTypeRegular
            // 
            this.chkOTSTypeRegular.Location = new System.Drawing.Point(88, 8);
            this.chkOTSTypeRegular.Name = "chkOTSTypeRegular";
            this.chkOTSTypeRegular.Size = new System.Drawing.Size(64, 24);
            this.chkOTSTypeRegular.TabIndex = 0;
            this.chkOTSTypeRegular.Text = "Regular";
            this.chkOTSTypeRegular.CheckedChanged += new System.EventHandler(this.chkOTSTypeRegular_CheckedChanged);
            // 
            // cboIDFormat
            // 
            this.cboIDFormat.AutoAdjust = true;
            this.cboIDFormat.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboIDFormat.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboIDFormat.DataSource = null;
            this.cboIDFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboIDFormat.DropDownWidth = 152;
            this.cboIDFormat.FormattingEnabled = false;
            this.cboIDFormat.IgnoreFocusLost = false;
            this.cboIDFormat.ItemHeight = 13;
            this.cboIDFormat.Location = new System.Drawing.Point(120, 40);
            this.cboIDFormat.Margin = new System.Windows.Forms.Padding(0);
            this.cboIDFormat.MaxDropDownItems = 25;
            this.cboIDFormat.Name = "cboIDFormat";
            this.cboIDFormat.SetToolTip = "";
            this.cboIDFormat.Size = new System.Drawing.Size(152, 21);
            this.cboIDFormat.TabIndex = 10;
            this.cboIDFormat.Tag = null;
            this.cboIDFormat.Visible = false;
            this.cboIDFormat.SelectionChangeCommitted += new System.EventHandler(this.cboIDFormat_SelectionChangeCommitted);
            // 
            // lblIDOption
            // 
            this.lblIDOption.Location = new System.Drawing.Point(8, 40);
            this.lblIDOption.Name = "lblIDOption";
            this.lblIDOption.Size = new System.Drawing.Size(104, 23);
            this.lblIDOption.TabIndex = 9;
            this.lblIDOption.Text = "ID Format:";
            this.lblIDOption.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblIDOption.Visible = false;
            // 
            // gbxLevelNameLength
            // 
            this.gbxLevelNameLength.Controls.Add(this.numRangeTo);
            this.gbxLevelNameLength.Controls.Add(this.txtRangeTo);
            this.gbxLevelNameLength.Controls.Add(this.numRangeFrom);
            this.gbxLevelNameLength.Controls.Add(this.radRange);
            this.gbxLevelNameLength.Controls.Add(this.numRequiredSize);
            this.gbxLevelNameLength.Controls.Add(this.radRequiredSize);
            this.gbxLevelNameLength.Controls.Add(this.radUnrestricted);
            this.gbxLevelNameLength.Location = new System.Drawing.Point(16, 144);
            this.gbxLevelNameLength.Name = "gbxLevelNameLength";
            this.gbxLevelNameLength.Size = new System.Drawing.Size(256, 112);
            this.gbxLevelNameLength.TabIndex = 14;
            this.gbxLevelNameLength.TabStop = false;
            this.gbxLevelNameLength.Text = "Length";
            // 
            // numRangeTo
            // 
            this.numRangeTo.Location = new System.Drawing.Point(184, 80);
            this.numRangeTo.Name = "numRangeTo";
            this.numRangeTo.Size = new System.Drawing.Size(48, 20);
            this.numRangeTo.TabIndex = 19;
            this.numRangeTo.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numRangeTo.ValueChanged += new System.EventHandler(this.numRangeTo_ValueChanged);
            // 
            // txtRangeTo
            // 
            this.txtRangeTo.Location = new System.Drawing.Point(160, 80);
            this.txtRangeTo.Name = "txtRangeTo";
            this.txtRangeTo.Size = new System.Drawing.Size(24, 23);
            this.txtRangeTo.TabIndex = 6;
            this.txtRangeTo.Text = "to";
            this.txtRangeTo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // numRangeFrom
            // 
            this.numRangeFrom.Location = new System.Drawing.Point(112, 80);
            this.numRangeFrom.Name = "numRangeFrom";
            this.numRangeFrom.Size = new System.Drawing.Size(48, 20);
            this.numRangeFrom.TabIndex = 18;
            this.numRangeFrom.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numRangeFrom.ValueChanged += new System.EventHandler(this.numRangeFrom_ValueChanged);
            // 
            // radRange
            // 
            this.radRange.Location = new System.Drawing.Point(24, 80);
            this.radRange.Name = "radRange";
            this.radRange.Size = new System.Drawing.Size(104, 24);
            this.radRange.TabIndex = 17;
            this.radRange.Text = "Range from";
            this.radRange.CheckedChanged += new System.EventHandler(this.radRange_CheckedChanged);
            // 
            // numRequiredSize
            // 
            this.numRequiredSize.Location = new System.Drawing.Point(128, 48);
            this.numRequiredSize.Name = "numRequiredSize";
            this.numRequiredSize.Size = new System.Drawing.Size(48, 20);
            this.numRequiredSize.TabIndex = 16;
            this.numRequiredSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numRequiredSize.ValueChanged += new System.EventHandler(this.numRequiredSize_ValueChanged);
            // 
            // radRequiredSize
            // 
            this.radRequiredSize.Location = new System.Drawing.Point(24, 48);
            this.radRequiredSize.Name = "radRequiredSize";
            this.radRequiredSize.Size = new System.Drawing.Size(104, 24);
            this.radRequiredSize.TabIndex = 15;
            this.radRequiredSize.Text = "Required Size";
            this.radRequiredSize.CheckedChanged += new System.EventHandler(this.radRequiredSize_CheckedChanged);
            // 
            // radUnrestricted
            // 
            this.radUnrestricted.Location = new System.Drawing.Point(24, 16);
            this.radUnrestricted.Name = "radUnrestricted";
            this.radUnrestricted.Size = new System.Drawing.Size(104, 24);
            this.radUnrestricted.TabIndex = 14;
            this.radUnrestricted.Text = "Unrestricted";
            this.radUnrestricted.CheckedChanged += new System.EventHandler(this.radUnrestricted_CheckedChanged);
            // 
            // cboDisplayOption
            // 
            this.cboDisplayOption.AutoAdjust = true;
            this.cboDisplayOption.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboDisplayOption.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboDisplayOption.DataSource = null;
            this.cboDisplayOption.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboDisplayOption.DropDownWidth = 152;
            this.cboDisplayOption.FormattingEnabled = false;
            this.cboDisplayOption.IgnoreFocusLost = false;
            this.cboDisplayOption.ItemHeight = 13;
            this.cboDisplayOption.Location = new System.Drawing.Point(120, 8);
            this.cboDisplayOption.Margin = new System.Windows.Forms.Padding(0);
            this.cboDisplayOption.MaxDropDownItems = 25;
            this.cboDisplayOption.Name = "cboDisplayOption";
            this.cboDisplayOption.SetToolTip = "";
            this.cboDisplayOption.Size = new System.Drawing.Size(152, 21);
            this.cboDisplayOption.TabIndex = 9;
            this.cboDisplayOption.Tag = null;
            this.cboDisplayOption.SelectionChangeCommitted += new System.EventHandler(this.cboDisplayOption_SelectionChangeCommitted);
            // 
            // lblDisplayOption
            // 
            this.lblDisplayOption.Location = new System.Drawing.Point(8, 8);
            this.lblDisplayOption.Name = "lblDisplayOption";
            this.lblDisplayOption.Size = new System.Drawing.Size(104, 23);
            this.lblDisplayOption.TabIndex = 6;
            this.lblDisplayOption.Text = "Display:";
            this.lblDisplayOption.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblOTSType
            // 
            this.lblOTSType.Location = new System.Drawing.Point(8, 72);
            this.lblOTSType.Name = "lblOTSType";
            this.lblOTSType.Size = new System.Drawing.Size(104, 23);
            this.lblOTSType.TabIndex = 1;
            this.lblOTSType.Text = "Level Type:";
            this.lblOTSType.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cboOTSType
            // 
            this.cboOTSType.AutoAdjust = true;
            this.cboOTSType.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboOTSType.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboOTSType.DataSource = null;
            this.cboOTSType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboOTSType.DropDownWidth = 152;
            this.cboOTSType.FormattingEnabled = false;
            this.cboOTSType.IgnoreFocusLost = false;
            this.cboOTSType.ItemHeight = 13;
            this.cboOTSType.Location = new System.Drawing.Point(120, 72);
            this.cboOTSType.Margin = new System.Windows.Forms.Padding(0);
            this.cboOTSType.MaxDropDownItems = 25;
            this.cboOTSType.Name = "cboOTSType";
            this.cboOTSType.SetToolTip = "";
            this.cboOTSType.Size = new System.Drawing.Size(152, 21);
            this.cboOTSType.TabIndex = 11;
            this.cboOTSType.Tag = null;
            this.cboOTSType.SelectionChangeCommitted += new System.EventHandler(this.cboOTSType_SelectionChangeCommitted);
            // 
            // tabPurgeCriteria
            // 
            this.tabPurgeCriteria.AutoScroll = true;
            this.tabPurgeCriteria.Controls.Add(this.lblHtWorkUpTotTimeframe);
            this.tabPurgeCriteria.Controls.Add(this.txtHtWorkupTotBy);
            this.tabPurgeCriteria.Controls.Add(this.lblHtVSWTimeframe);
            this.tabPurgeCriteria.Controls.Add(this.lblHtReserveTimeframe);
            this.tabPurgeCriteria.Controls.Add(this.lblHtPurchaseOrderTimeframe);
            this.tabPurgeCriteria.Controls.Add(this.lblHtReceiptTimeframe);
            this.tabPurgeCriteria.Controls.Add(this.lblHtDummyTimeframe);
            this.tabPurgeCriteria.Controls.Add(this.lblHtDropShipTimeframe);
            this.tabPurgeCriteria.Controls.Add(this.lblHtASNTimeframe);
            this.tabPurgeCriteria.Controls.Add(this.txtHtVSW);
            this.tabPurgeCriteria.Controls.Add(this.txtHtReserve);
            this.tabPurgeCriteria.Controls.Add(this.txtHtPurchase);
            this.tabPurgeCriteria.Controls.Add(this.txtHtReceipt);
            this.tabPurgeCriteria.Controls.Add(this.txtHtDummy);
            this.tabPurgeCriteria.Controls.Add(this.txtHtDropShip);
            this.tabPurgeCriteria.Controls.Add(this.txtHtASN);
            this.tabPurgeCriteria.Controls.Add(this.lblHtWorkUpTot);
            this.tabPurgeCriteria.Controls.Add(this.lblHtVSW);
            this.tabPurgeCriteria.Controls.Add(this.lblHtReserve);
            this.tabPurgeCriteria.Controls.Add(this.lblHtReceipt);
            this.tabPurgeCriteria.Controls.Add(this.lblHtPurchaseOrder);
            this.tabPurgeCriteria.Controls.Add(this.lblHtDummy);
            this.tabPurgeCriteria.Controls.Add(this.lblHtDropShip);
            this.tabPurgeCriteria.Controls.Add(this.lblHtASN);
            this.tabPurgeCriteria.Controls.Add(this.txtPurgePlans);
            this.tabPurgeCriteria.Controls.Add(this.txtPurgeWeeklyHistory);
            this.tabPurgeCriteria.Controls.Add(this.txtPurgeDailyHistory);
            this.tabPurgeCriteria.Controls.Add(this.lblPurgePlansTimeframe);
            this.tabPurgeCriteria.Controls.Add(this.lblPurgeWeeklyHistoryTimeframe);
            this.tabPurgeCriteria.Controls.Add(this.lblPurgeDailyHistoryTimeframe);
            this.tabPurgeCriteria.Controls.Add(this.lblPurgeWeeklyHistory);
            this.tabPurgeCriteria.Controls.Add(this.lblPurgePlans);
            this.tabPurgeCriteria.Controls.Add(this.lblPurgeDailyHistory);
            this.tabPurgeCriteria.Location = new System.Drawing.Point(4, 22);
            this.tabPurgeCriteria.Name = "tabPurgeCriteria";
            this.tabPurgeCriteria.Size = new System.Drawing.Size(296, 262);
            this.tabPurgeCriteria.TabIndex = 1;
            this.tabPurgeCriteria.Text = "Purge Criteria";
            // 
            // lblHtWorkUpTotTimeframe
            // 
            this.lblHtWorkUpTotTimeframe.Location = new System.Drawing.Point(228, 232);
            this.lblHtWorkUpTotTimeframe.Name = "lblHtWorkUpTotTimeframe";
            this.lblHtWorkUpTotTimeframe.Size = new System.Drawing.Size(40, 23);
            this.lblHtWorkUpTotTimeframe.TabIndex = 77;
            this.lblHtWorkUpTotTimeframe.Text = "weeks";
            this.lblHtWorkUpTotTimeframe.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtHtWorkupTotBy
            // 
            this.txtHtWorkupTotBy.Location = new System.Drawing.Point(173, 233);
            this.txtHtWorkupTotBy.Name = "txtHtWorkupTotBy";
            this.txtHtWorkupTotBy.Size = new System.Drawing.Size(48, 20);
            this.txtHtWorkupTotBy.TabIndex = 19;
            this.txtHtWorkupTotBy.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtHtWorkupTotBy.TextChanged += new System.EventHandler(this.txtHtWorkupTotBy_TextChanged);
            // 
            // lblHtVSWTimeframe
            // 
            this.lblHtVSWTimeframe.Location = new System.Drawing.Point(228, 210);
            this.lblHtVSWTimeframe.Name = "lblHtVSWTimeframe";
            this.lblHtVSWTimeframe.Size = new System.Drawing.Size(40, 23);
            this.lblHtVSWTimeframe.TabIndex = 75;
            this.lblHtVSWTimeframe.Text = "weeks";
            this.lblHtVSWTimeframe.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblHtReserveTimeframe
            // 
            this.lblHtReserveTimeframe.Location = new System.Drawing.Point(228, 188);
            this.lblHtReserveTimeframe.Name = "lblHtReserveTimeframe";
            this.lblHtReserveTimeframe.Size = new System.Drawing.Size(40, 23);
            this.lblHtReserveTimeframe.TabIndex = 74;
            this.lblHtReserveTimeframe.Text = "weeks";
            this.lblHtReserveTimeframe.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblHtPurchaseOrderTimeframe
            // 
            this.lblHtPurchaseOrderTimeframe.Location = new System.Drawing.Point(228, 144);
            this.lblHtPurchaseOrderTimeframe.Name = "lblHtPurchaseOrderTimeframe";
            this.lblHtPurchaseOrderTimeframe.Size = new System.Drawing.Size(40, 23);
            this.lblHtPurchaseOrderTimeframe.TabIndex = 73;
            this.lblHtPurchaseOrderTimeframe.Text = "weeks";
            this.lblHtPurchaseOrderTimeframe.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblHtReceiptTimeframe
            // 
            this.lblHtReceiptTimeframe.Location = new System.Drawing.Point(228, 166);
            this.lblHtReceiptTimeframe.Name = "lblHtReceiptTimeframe";
            this.lblHtReceiptTimeframe.Size = new System.Drawing.Size(40, 23);
            this.lblHtReceiptTimeframe.TabIndex = 72;
            this.lblHtReceiptTimeframe.Text = "weeks";
            this.lblHtReceiptTimeframe.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblHtDummyTimeframe
            // 
            this.lblHtDummyTimeframe.Location = new System.Drawing.Point(228, 122);
            this.lblHtDummyTimeframe.Name = "lblHtDummyTimeframe";
            this.lblHtDummyTimeframe.Size = new System.Drawing.Size(40, 23);
            this.lblHtDummyTimeframe.TabIndex = 69;
            this.lblHtDummyTimeframe.Text = "weeks";
            this.lblHtDummyTimeframe.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblHtDropShipTimeframe
            // 
            this.lblHtDropShipTimeframe.Location = new System.Drawing.Point(228, 99);
            this.lblHtDropShipTimeframe.Name = "lblHtDropShipTimeframe";
            this.lblHtDropShipTimeframe.Size = new System.Drawing.Size(40, 23);
            this.lblHtDropShipTimeframe.TabIndex = 68;
            this.lblHtDropShipTimeframe.Text = "weeks";
            this.lblHtDropShipTimeframe.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblHtASNTimeframe
            // 
            this.lblHtASNTimeframe.Location = new System.Drawing.Point(228, 76);
            this.lblHtASNTimeframe.Name = "lblHtASNTimeframe";
            this.lblHtASNTimeframe.Size = new System.Drawing.Size(40, 23);
            this.lblHtASNTimeframe.TabIndex = 66;
            this.lblHtASNTimeframe.Text = "weeks";
            this.lblHtASNTimeframe.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtHtVSW
            // 
            this.txtHtVSW.Location = new System.Drawing.Point(173, 211);
            this.txtHtVSW.Name = "txtHtVSW";
            this.txtHtVSW.Size = new System.Drawing.Size(48, 20);
            this.txtHtVSW.TabIndex = 18;
            this.txtHtVSW.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtHtVSW.TextChanged += new System.EventHandler(this.txtHtVSW_TextChanged);
            // 
            // txtHtReserve
            // 
            this.txtHtReserve.Location = new System.Drawing.Point(173, 189);
            this.txtHtReserve.Name = "txtHtReserve";
            this.txtHtReserve.Size = new System.Drawing.Size(48, 20);
            this.txtHtReserve.TabIndex = 17;
            this.txtHtReserve.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtHtReserve.TextChanged += new System.EventHandler(this.txtHtReserve_TextChanged);
            // 
            // txtHtPurchase
            // 
            this.txtHtPurchase.Location = new System.Drawing.Point(173, 145);
            this.txtHtPurchase.Name = "txtHtPurchase";
            this.txtHtPurchase.Size = new System.Drawing.Size(48, 20);
            this.txtHtPurchase.TabIndex = 15;
            this.txtHtPurchase.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtHtPurchase.TextChanged += new System.EventHandler(this.txtHtPurchase_TextChanged);
            // 
            // txtHtReceipt
            // 
            this.txtHtReceipt.Location = new System.Drawing.Point(173, 167);
            this.txtHtReceipt.Name = "txtHtReceipt";
            this.txtHtReceipt.Size = new System.Drawing.Size(48, 20);
            this.txtHtReceipt.TabIndex = 16;
            this.txtHtReceipt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtHtReceipt.TextChanged += new System.EventHandler(this.txtHtReceipt_TextChanged);
            // 
            // txtHtDummy
            // 
            this.txtHtDummy.Location = new System.Drawing.Point(173, 123);
            this.txtHtDummy.Name = "txtHtDummy";
            this.txtHtDummy.Size = new System.Drawing.Size(48, 20);
            this.txtHtDummy.TabIndex = 14;
            this.txtHtDummy.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtHtDummy.TextChanged += new System.EventHandler(this.txtHtDummy_TextChanged);
            // 
            // txtHtDropShip
            // 
            this.txtHtDropShip.Location = new System.Drawing.Point(173, 100);
            this.txtHtDropShip.Name = "txtHtDropShip";
            this.txtHtDropShip.Size = new System.Drawing.Size(48, 20);
            this.txtHtDropShip.TabIndex = 13;
            this.txtHtDropShip.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtHtDropShip.TextChanged += new System.EventHandler(this.txtHtDropShip_TextChanged);
            // 
            // txtHtASN
            // 
            this.txtHtASN.Location = new System.Drawing.Point(173, 77);
            this.txtHtASN.Name = "txtHtASN";
            this.txtHtASN.Size = new System.Drawing.Size(48, 20);
            this.txtHtASN.TabIndex = 12;
            this.txtHtASN.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtHtASN.TextChanged += new System.EventHandler(this.txtHtASN_TextChanged);
            // 
            // lblHtWorkUpTot
            // 
            this.lblHtWorkUpTot.Location = new System.Drawing.Point(8, 235);
            this.lblHtWorkUpTot.Name = "lblHtWorkUpTot";
            this.lblHtWorkUpTot.Size = new System.Drawing.Size(161, 16);
            this.lblHtWorkUpTot.TabIndex = 55;
            this.lblHtWorkUpTot.Text = "Header Type: Workup Total Buy";
            this.lblHtWorkUpTot.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblHtVSW
            // 
            this.lblHtVSW.Location = new System.Drawing.Point(28, 213);
            this.lblHtVSW.Name = "lblHtVSW";
            this.lblHtVSW.Size = new System.Drawing.Size(141, 16);
            this.lblHtVSW.TabIndex = 54;
            this.lblHtVSW.Text = "Header Type: VSW";
            this.lblHtVSW.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblHtReserve
            // 
            this.lblHtReserve.Location = new System.Drawing.Point(28, 191);
            this.lblHtReserve.Name = "lblHtReserve";
            this.lblHtReserve.Size = new System.Drawing.Size(141, 16);
            this.lblHtReserve.TabIndex = 53;
            this.lblHtReserve.Text = "Header Type: Reserve";
            this.lblHtReserve.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblHtReceipt
            // 
            this.lblHtReceipt.Location = new System.Drawing.Point(28, 169);
            this.lblHtReceipt.Name = "lblHtReceipt";
            this.lblHtReceipt.Size = new System.Drawing.Size(141, 16);
            this.lblHtReceipt.TabIndex = 52;
            this.lblHtReceipt.Text = "Header Type: Receipt";
            this.lblHtReceipt.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblHtPurchaseOrder
            // 
            this.lblHtPurchaseOrder.Location = new System.Drawing.Point(11, 145);
            this.lblHtPurchaseOrder.Name = "lblHtPurchaseOrder";
            this.lblHtPurchaseOrder.Size = new System.Drawing.Size(158, 18);
            this.lblHtPurchaseOrder.TabIndex = 51;
            this.lblHtPurchaseOrder.Text = "Header Type: Purchase Order";
            this.lblHtPurchaseOrder.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblHtDummy
            // 
            this.lblHtDummy.Location = new System.Drawing.Point(28, 125);
            this.lblHtDummy.Name = "lblHtDummy";
            this.lblHtDummy.Size = new System.Drawing.Size(141, 16);
            this.lblHtDummy.TabIndex = 48;
            this.lblHtDummy.Text = "Header Type: Dummy";
            this.lblHtDummy.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblHtDropShip
            // 
            this.lblHtDropShip.Location = new System.Drawing.Point(28, 102);
            this.lblHtDropShip.Name = "lblHtDropShip";
            this.lblHtDropShip.Size = new System.Drawing.Size(141, 16);
            this.lblHtDropShip.TabIndex = 47;
            this.lblHtDropShip.Text = "Header Type: Drop Ship";
            this.lblHtDropShip.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblHtASN
            // 
            this.lblHtASN.Location = new System.Drawing.Point(28, 79);
            this.lblHtASN.Name = "lblHtASN";
            this.lblHtASN.Size = new System.Drawing.Size(141, 16);
            this.lblHtASN.TabIndex = 45;
            this.lblHtASN.Text = "Header Type: ASN";
            this.lblHtASN.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtPurgePlans
            // 
            this.txtPurgePlans.Location = new System.Drawing.Point(173, 54);
            this.txtPurgePlans.Name = "txtPurgePlans";
            this.txtPurgePlans.Size = new System.Drawing.Size(48, 20);
            this.txtPurgePlans.TabIndex = 11;
            this.txtPurgePlans.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtPurgePlans.TextChanged += new System.EventHandler(this.txtPurgePlans_TextChanged);
            // 
            // txtPurgeWeeklyHistory
            // 
            this.txtPurgeWeeklyHistory.Location = new System.Drawing.Point(173, 31);
            this.txtPurgeWeeklyHistory.Name = "txtPurgeWeeklyHistory";
            this.txtPurgeWeeklyHistory.Size = new System.Drawing.Size(48, 20);
            this.txtPurgeWeeklyHistory.TabIndex = 10;
            this.txtPurgeWeeklyHistory.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtPurgeWeeklyHistory.TextChanged += new System.EventHandler(this.txtPurgeWeeklyHistory_TextChanged);
            // 
            // txtPurgeDailyHistory
            // 
            this.txtPurgeDailyHistory.Location = new System.Drawing.Point(173, 8);
            this.txtPurgeDailyHistory.Name = "txtPurgeDailyHistory";
            this.txtPurgeDailyHistory.Size = new System.Drawing.Size(48, 20);
            this.txtPurgeDailyHistory.TabIndex = 9;
            this.txtPurgeDailyHistory.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtPurgeDailyHistory.TextChanged += new System.EventHandler(this.txtPurgeDailyHistory_TextChanged);
            // 
            // lblPurgePlansTimeframe
            // 
            this.lblPurgePlansTimeframe.Location = new System.Drawing.Point(228, 55);
            this.lblPurgePlansTimeframe.Name = "lblPurgePlansTimeframe";
            this.lblPurgePlansTimeframe.Size = new System.Drawing.Size(40, 23);
            this.lblPurgePlansTimeframe.TabIndex = 35;
            this.lblPurgePlansTimeframe.Text = "weeks";
            this.lblPurgePlansTimeframe.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblPurgeWeeklyHistoryTimeframe
            // 
            this.lblPurgeWeeklyHistoryTimeframe.Location = new System.Drawing.Point(228, 31);
            this.lblPurgeWeeklyHistoryTimeframe.Name = "lblPurgeWeeklyHistoryTimeframe";
            this.lblPurgeWeeklyHistoryTimeframe.Size = new System.Drawing.Size(40, 23);
            this.lblPurgeWeeklyHistoryTimeframe.TabIndex = 34;
            this.lblPurgeWeeklyHistoryTimeframe.Text = "weeks";
            this.lblPurgeWeeklyHistoryTimeframe.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblPurgeDailyHistoryTimeframe
            // 
            this.lblPurgeDailyHistoryTimeframe.Location = new System.Drawing.Point(228, 7);
            this.lblPurgeDailyHistoryTimeframe.Name = "lblPurgeDailyHistoryTimeframe";
            this.lblPurgeDailyHistoryTimeframe.Size = new System.Drawing.Size(40, 23);
            this.lblPurgeDailyHistoryTimeframe.TabIndex = 33;
            this.lblPurgeDailyHistoryTimeframe.Text = "days";
            this.lblPurgeDailyHistoryTimeframe.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblPurgeWeeklyHistory
            // 
            this.lblPurgeWeeklyHistory.Location = new System.Drawing.Point(28, 34);
            this.lblPurgeWeeklyHistory.Name = "lblPurgeWeeklyHistory";
            this.lblPurgeWeeklyHistory.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblPurgeWeeklyHistory.Size = new System.Drawing.Size(141, 16);
            this.lblPurgeWeeklyHistory.TabIndex = 29;
            this.lblPurgeWeeklyHistory.Text = "Purge weekly history after";
            this.lblPurgeWeeklyHistory.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblPurgePlans
            // 
            this.lblPurgePlans.Location = new System.Drawing.Point(28, 58);
            this.lblPurgePlans.Name = "lblPurgePlans";
            this.lblPurgePlans.Size = new System.Drawing.Size(141, 16);
            this.lblPurgePlans.TabIndex = 23;
            this.lblPurgePlans.Text = "Purge OTS plans after";
            this.lblPurgePlans.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblPurgeDailyHistory
            // 
            this.lblPurgeDailyHistory.Location = new System.Drawing.Point(28, 10);
            this.lblPurgeDailyHistory.Name = "lblPurgeDailyHistory";
            this.lblPurgeDailyHistory.Size = new System.Drawing.Size(141, 16);
            this.lblPurgeDailyHistory.TabIndex = 21;
            this.lblPurgeDailyHistory.Text = "Purge daily history after";
            this.lblPurgeDailyHistory.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // picDownArrow
            // 
            this.picDownArrow.Location = new System.Drawing.Point(184, 200);
            this.picDownArrow.Name = "picDownArrow";
            this.picDownArrow.Size = new System.Drawing.Size(24, 24);
            this.picDownArrow.TabIndex = 4;
            this.picDownArrow.TabStop = false;
            this.picDownArrow.Click += new System.EventHandler(this.picDownArrow_Click);
            // 
            // picUpArrow
            // 
            this.picUpArrow.Location = new System.Drawing.Point(184, 144);
            this.picUpArrow.Name = "picUpArrow";
            this.picUpArrow.Size = new System.Drawing.Size(24, 24);
            this.picUpArrow.TabIndex = 3;
            this.picUpArrow.TabStop = false;
            this.picUpArrow.Click += new System.EventHandler(this.picUpArrow_Click);
            // 
            // btnDeleteLevel
            // 
            this.btnDeleteLevel.Location = new System.Drawing.Point(120, 352);
            this.btnDeleteLevel.Name = "btnDeleteLevel";
            this.btnDeleteLevel.Size = new System.Drawing.Size(75, 23);
            this.btnDeleteLevel.TabIndex = 6;
            this.btnDeleteLevel.Text = "Delete";
            this.btnDeleteLevel.Click += new System.EventHandler(this.btnDeleteLevel_Click);
            // 
            // btnNewLevel
            // 
            this.btnNewLevel.Location = new System.Drawing.Point(24, 352);
            this.btnNewLevel.Name = "btnNewLevel";
            this.btnNewLevel.Size = new System.Drawing.Size(75, 23);
            this.btnNewLevel.TabIndex = 5;
            this.btnNewLevel.Text = "New";
            this.btnNewLevel.Click += new System.EventHandler(this.btnNewLevel_Click);
            // 
            // lstHierarchyLevels
            // 
            this.lstHierarchyLevels.Location = new System.Drawing.Point(32, 32);
            this.lstHierarchyLevels.Name = "lstHierarchyLevels";
            this.lstHierarchyLevels.Size = new System.Drawing.Size(136, 304);
            this.lstHierarchyLevels.TabIndex = 4;
            this.lstHierarchyLevels.UseCompatibleStateImageBehavior = false;
            this.lstHierarchyLevels.SelectedIndexChanged += new System.EventHandler(this.lstHierarchyLevels_SelectedIndexChanged);
            // 
            // lblPostingDate
            // 
            this.lblPostingDate.Location = new System.Drawing.Point(400, 16);
            this.lblPostingDate.Name = "lblPostingDate";
            this.lblPostingDate.Size = new System.Drawing.Size(216, 23);
            this.lblPostingDate.TabIndex = 11;
            this.lblPostingDate.Text = "Posting Date:";
            this.lblPostingDate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // gbxHierarchyOTSType
            // 
            this.gbxHierarchyOTSType.Controls.Add(this.rdoHierarchyOTSTypeTotal);
            this.gbxHierarchyOTSType.Controls.Add(this.rdoHierarchyOTSTypeRegular);
            this.gbxHierarchyOTSType.Location = new System.Drawing.Point(264, 48);
            this.gbxHierarchyOTSType.Name = "gbxHierarchyOTSType";
            this.gbxHierarchyOTSType.Size = new System.Drawing.Size(176, 48);
            this.gbxHierarchyOTSType.TabIndex = 23;
            this.gbxHierarchyOTSType.TabStop = false;
            this.gbxHierarchyOTSType.Text = "OTS Type";
            // 
            // rdoHierarchyOTSTypeTotal
            // 
            this.rdoHierarchyOTSTypeTotal.Location = new System.Drawing.Point(96, 16);
            this.rdoHierarchyOTSTypeTotal.Name = "rdoHierarchyOTSTypeTotal";
            this.rdoHierarchyOTSTypeTotal.Size = new System.Drawing.Size(72, 24);
            this.rdoHierarchyOTSTypeTotal.TabIndex = 3;
            this.rdoHierarchyOTSTypeTotal.Text = "Total";
            this.rdoHierarchyOTSTypeTotal.CheckedChanged += new System.EventHandler(this.rdoHierarchyOTSTypeTotal_CheckedChanged);
            // 
            // rdoHierarchyOTSTypeRegular
            // 
            this.rdoHierarchyOTSTypeRegular.Location = new System.Drawing.Point(8, 16);
            this.rdoHierarchyOTSTypeRegular.Name = "rdoHierarchyOTSTypeRegular";
            this.rdoHierarchyOTSTypeRegular.Size = new System.Drawing.Size(88, 24);
            this.rdoHierarchyOTSTypeRegular.TabIndex = 2;
            this.rdoHierarchyOTSTypeRegular.Text = "Regular";
            this.rdoHierarchyOTSTypeRegular.CheckedChanged += new System.EventHandler(this.rdoHierarchyOTSTypeRegular_CheckedChanged);
            // 
            // gbxHierarchyRollupOption
            // 
            this.gbxHierarchyRollupOption.Controls.Add(this.rdoHierarchyRollupOptionAPI);
            this.gbxHierarchyRollupOption.Controls.Add(this.rdoHierarchyRollupOptionRealTime);
            this.gbxHierarchyRollupOption.Location = new System.Drawing.Point(448, 48);
            this.gbxHierarchyRollupOption.Name = "gbxHierarchyRollupOption";
            this.gbxHierarchyRollupOption.Size = new System.Drawing.Size(176, 48);
            this.gbxHierarchyRollupOption.TabIndex = 24;
            this.gbxHierarchyRollupOption.TabStop = false;
            this.gbxHierarchyRollupOption.Text = "Rollup Option";
            // 
            // rdoHierarchyRollupOptionAPI
            // 
            this.rdoHierarchyRollupOptionAPI.Location = new System.Drawing.Point(104, 16);
            this.rdoHierarchyRollupOptionAPI.Name = "rdoHierarchyRollupOptionAPI";
            this.rdoHierarchyRollupOptionAPI.Size = new System.Drawing.Size(64, 24);
            this.rdoHierarchyRollupOptionAPI.TabIndex = 3;
            this.rdoHierarchyRollupOptionAPI.Text = "API";
            this.rdoHierarchyRollupOptionAPI.CheckedChanged += new System.EventHandler(this.rdoHierarchyRollupOptionAPI_CheckedChanged);
            // 
            // rdoHierarchyRollupOptionRealTime
            // 
            this.rdoHierarchyRollupOptionRealTime.Location = new System.Drawing.Point(8, 16);
            this.rdoHierarchyRollupOptionRealTime.Name = "rdoHierarchyRollupOptionRealTime";
            this.rdoHierarchyRollupOptionRealTime.Size = new System.Drawing.Size(96, 24);
            this.rdoHierarchyRollupOptionRealTime.TabIndex = 2;
            this.rdoHierarchyRollupOptionRealTime.Text = "Real Time";
            this.rdoHierarchyRollupOptionRealTime.CheckedChanged += new System.EventHandler(this.rdoHierarchyRollupOptionRealTime_CheckedChanged);
            // 
            // btnApply
            // 
            this.btnApply.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnApply.Location = new System.Drawing.Point(549, 529);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(75, 23);
            this.btnApply.TabIndex = 25;
            this.btnApply.Text = "Apply";
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // frmHierarchyProperties
            // 
            this.AcceptButton = this.btnOK;
            this.AllowDragDrop = true;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(632, 559);
            this.Controls.Add(this.gbxHierarchyRollupOption);
            this.Controls.Add(this.gbxHierarchyOTSType);
            this.Controls.Add(this.lblPostingDate);
            this.Controls.Add(this.gbxLevels);
            this.Controls.Add(this.btnHelp);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.picHierarchyPropertyColor);
            this.Controls.Add(this.btnApply);
            this.Controls.Add(this.txtHierarchyPropertyName);
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.gbxHierarchyType);
            this.Name = "frmHierarchyProperties";
            this.Text = "Hierarchy Properties";
            this.Controls.SetChildIndex(this.gbxHierarchyType, 0);
            this.Controls.SetChildIndex(this.lblName, 0);
            this.Controls.SetChildIndex(this.txtHierarchyPropertyName, 0);
            this.Controls.SetChildIndex(this.btnApply, 0);
            this.Controls.SetChildIndex(this.picHierarchyPropertyColor, 0);
            this.Controls.SetChildIndex(this.btnOK, 0);
            this.Controls.SetChildIndex(this.btnCancel, 0);
            this.Controls.SetChildIndex(this.btnHelp, 0);
            this.Controls.SetChildIndex(this.gbxLevels, 0);
            this.Controls.SetChildIndex(this.lblPostingDate, 0);
            this.Controls.SetChildIndex(this.gbxHierarchyOTSType, 0);
            this.Controls.SetChildIndex(this.gbxHierarchyRollupOption, 0);
            ((System.ComponentModel.ISupportInitialize)(this.utmMain)).EndInit();
            this.gbxHierarchyType.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picHierarchyPropertyColor)).EndInit();
            this.gbxLevels.ResumeLayout(false);
            this.gbxLevelProperties.ResumeLayout(false);
            this.gbxLevelProperties.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picLevelColor)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabProfile.ResumeLayout(false);
            this.gbxOTSType.ResumeLayout(false);
            this.gbxLevelNameLength.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numRangeTo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numRangeFrom)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numRequiredSize)).EndInit();
            this.tabPurgeCriteria.ResumeLayout(false);
            this.tabPurgeCriteria.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picDownArrow)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picUpArrow)).EndInit();
            this.gbxHierarchyOTSType.ResumeLayout(false);
            this.gbxHierarchyRollupOption.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		private void SetText()
		{
			try
			{
				this.lblName.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Hier_Name);
				this.gbxHierarchyType.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Hier_Type);
				this.radOpen.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Hier_Type_Alternate);
				this.radOrganizational.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Hier_Type_Organizational);
				this.btnOK.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_OK);
				this.btnApply.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_Apply);
				this.btnCancel.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_Cancel);
				this.gbxLevels.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Hier_Lvl_Info_gbx);
				this.gbxLevelProperties.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Hier_Lvl_Prop_gbx);
				this._lblNewLevelNameName.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Hier_Lvl_Name);
				this.btnUpdate.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_Update);
				this.gbxLevelNameLength.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Hier_Lvl_Len);
				this.txtRangeTo.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Hier_Lvl_Len_Range_to);
				this.radRange.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Hier_Lvl_Len_Range_From);
				this.radRequiredSize.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Hier_Lvl_Len_Req_Size);
				this.radUnrestricted.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Hier_Lvl_Len_Unrestricted);
				this.btnDeleteLevel.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_Delete);
				this.btnNewLevel.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Insert);
//Begin Track #3863 - JScott - OTS Forecast Level Defaults
//				this.gbxOTSInformation.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_OTS_Info);
//				this.lblOTSType.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_OTS_Type);
//				this.radOTSPlvlRegular.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_OTS_Regular);
//				this.radOTSPlvlTotal.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_OTS_Total);
				this.gbxOTSType.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_OTS_Type);
				this.lblOTSType.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_OTS_Level_Type);
				this.chkOTSTypeRegular.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_OTS_Regular);
				this.chkOTSTypeTotal.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_OTS_Total);
				this.gbxHierarchyOTSType.Text = this.gbxOTSType.Text;
				this.rdoHierarchyOTSTypeRegular.Text = this.chkOTSTypeRegular.Text;
				this.rdoHierarchyOTSTypeTotal.Text = this.chkOTSTypeTotal.Text;
//End Track #3863 - JScott - OTS Forecast Level Defaults
				this.cboDisplayOption.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Display_Option);
				this.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Hier_Properties_Form);
				this.lblIDOption.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_ID_Option);
				this.lblPurgeDailyHistory.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Purge_Daily_History);
				this.lblPurgeDailyHistoryTimeframe.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Weeks);
				this.lblPurgeWeeklyHistory.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Purge_Weekly_History);
				this.lblPurgeWeeklyHistoryTimeframe.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Weeks);
				this.lblPurgePlans.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Purge_Plans);
				this.lblPurgePlansTimeframe.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Weeks);
                //BEGIN TT#400-MD-VStuart-Add Header Purge Criteria by Header Type
                //this.lblPurgeHeaders.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Purge_Headers);
                //this.lblPurgeHeadersTimeframe.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Weeks);
                //END TT#400-MD-VStuart-Add Header Purge Criteria by Header Type
                _lblMIDLevels = MIDText.GetTextOnly(eMIDTextCode.lbl_Levels);
				_lblNewHierarchy = MIDText.GetTextOnly(eMIDTextCode.lbl_NewHierarchy);
				_lblNewPersonalHierarchy = MIDText.GetTextOnly(eMIDTextCode.lbl_NewPersonalHierarchy);
				_lblNewLevelName = MIDText.GetTextOnly(eMIDTextCode.lbl_NewLevel);
				_lblNewNode = MIDText.GetTextOnly(eMIDTextCode.lbl_NewNode);
				_lblCodeColor = MIDText.GetTextOnly(eMIDTextCode.lbl_ColorCode);
                /* BEGIN TT#400-MD-VStuart-Add Header Purge Criteria by Header Type */
                lblHtASNTimeframe.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Weeks);
                lblHtDropShipTimeframe.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Weeks);
                lblHtDummyTimeframe.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Weeks);
                lblHtReceiptTimeframe.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Weeks);
                lblHtPurchaseOrderTimeframe.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Weeks);
                lblHtReserveTimeframe.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Weeks);
                lblHtVSWTimeframe.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Weeks);
                lblHtWorkUpTotTimeframe.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Weeks);
                /* END TT#400-MD-VStuart-Add Header Purge Criteria by Header Type */
			}
			catch( Exception exception )
			{
				HandleException(exception);
			}
		}
		
		public void InitializeForm(MIDHierarchyNode node, int nodeLevel)
		{
			try
			{
//Begin Track #3901 - John Smith - pending level change when switch hierarchies
				if (_levelChangePending)
				{
					ChangePending = true;
					btnApply.Enabled = true;
				}
//End Track #3901

				CheckForPendingChanges();
				_loadingWindow = true;
				if (node.HierarchyType == eHierarchyType.organizational)
				{
					FunctionSecurity = SAB.ClientServerSession.GetMyUserNodeFunctionSecurityAssignment(node.Profile.Key,eSecurityFunctions.AdminHierarchiesOrgProperty, (int)eSecurityTypes.All);
				}
				else if (node.Profile.ProfileType == eProfileType.MerchandiseMainUserFolder)
				{
					FunctionSecurity = SAB.ClientServerSession.GetMyUserNodeFunctionSecurityAssignment(node.Profile.Key, eSecurityFunctions.AdminHierarchiesAltUser, (int)eSecurityTypes.All);
				}
				else
				{
					FunctionSecurity = SAB.ClientServerSession.GetMyUserNodeFunctionSecurityAssignment(node.Profile.Key, eSecurityFunctions.AdminHierarchiesAltGlobalProperty, (int)eSecurityTypes.All);
				}
				//ChangePending = false;
				// if redisplaying different hierarchy with existing form, dequeue prior hierarchy
				if (_hp != null && 
					_hp.HierarchyLockStatus == eLockStatus.Locked && 
					node.HomeHierarchyRID != _hp.Key)
				{
					SAB.HierarchyServerSession.DequeueHierarchy(_hp.Key);
				}
//				LoadCommon();
				node.NodeChangeType = eChangeType.none;
				_node = node;
				this.txtHierarchyPropertyName.Text = node.Text;
				this.lstHierarchyLevels.Items.Clear();
				this.lstHierarchyLevels.View = View.Details;
				this.lstHierarchyLevels.Columns.Clear();
				this.lstHierarchyLevels.Columns.Add(_lblMIDLevels, (int) lstHierarchyLevels.Width - 4,
					HorizontalAlignment.Left);
				lstHierarchyLevels.SmallImageList = MIDGraphics.ImageList;
				if (node.Profile.ProfileType == eProfileType.MerchandiseMainUserFolder)
				{
					_hp = new HierarchyProfile(-1);   // create as holder for fields
					this.gbxHierarchyType.Enabled = false;
					this.gbxLevels.Enabled = false;
					displayHierarchyFolderColor(GraphicsDirectory + "\\" + SAB.ClientServerSession.MyHierarchyColor + MIDGraphics.ClosedFolder);
					LoadCommon();
				}
				else
				{
					// enqueue the hierarchy
					if (FunctionSecurity.AllowUpdate)
					{
						// don't lock hierarchy if already locked
						if (_hp == null || 
							node.HomeHierarchyRID != _hp.Key ||
							_hp.HierarchyLockStatus != eLockStatus.Locked)
						{
							_hp = SAB.HierarchyServerSession.GetHierarchyDataForUpdate(node.HomeHierarchyRID, true);
							if (_hp.HierarchyLockStatus == eLockStatus.ReadOnly)
							{
								FunctionSecurity.SetReadOnly();  
							}
							else
								if (_hp.HierarchyLockStatus == eLockStatus.Cancel)
							{
								throw new HierarchyConflictException();
							}
						}
					}
					else
					{
						_hp = SAB.HierarchyServerSession.GetHierarchyData(node.HomeHierarchyRID);
					}
					LoadCommon();
					this.txtHierarchyPropertyName.Text = _hp.HierarchyID;
					switch (_hp.HierarchyType)
					{
						case eHierarchyType.organizational: 
						{
							this.radOrganizational.Checked = true;
							//						if (node.NodeType == eProfileType.OrganizationalHierarchyRoot)
//Begin - JScott - Add Rollup Type to Hierarchy Properties - Part 2
							this.gbxHierarchyRollupOption.Enabled = false;
//End - JScott - Add Rollup Type to Hierarchy Properties - Part 2
							if (_hp.HierarchyLevels.Count > 0)
							{
								if (nodeLevel == 0)  // hierarchy selected
								{
									_currentLevelIndex = 0;  // display the first level
								}
								else
								{
									_currentLevelIndex = nodeLevel - 1;  // subtract one since hierarchy does not occupy a level
								}
								lstHierarchyLevels_Load(_hp.HierarchyLevels);
							}
							break;
						}
						case eHierarchyType.alternate: 
						{
							this.radOpen.Checked = true;
							this.gbxLevels.Enabled = false;
//Begin - JScott - Add Rollup Type to Hierarchy Properties - Part 2
							this.gbxHierarchyRollupOption.Enabled = true;
//End - JScott - Add Rollup Type to Hierarchy Properties - Part 2
							Level_Information_Load(-1);  // clear level information
							break;
						}
					}
					this.radOrganizational.Enabled = false;
					this.radOpen.Enabled = false;
//Begin Track #3863 - JScott - OTS Forecast Level Defaults
					switch (_hp.OTSPlanLevelType)
					{
						case eOTSPlanLevelType.Regular: 
						{
							this.rdoHierarchyOTSTypeRegular.Checked = true;
							break;
						}
						case eOTSPlanLevelType.Total: 
						{
							this.rdoHierarchyOTSTypeTotal.Checked = true;
							break;
						}
                        default:
                        {
							this.rdoHierarchyOTSTypeTotal.Checked = true;
							break;
						}
					}
//End Track #3863 - JScott - OTS Forecast Level Defaults
//Begin - JScott - Add Rollup Type to Hierarchy Properties - Part 2
					switch (_hp.HierarchyRollupOption)
					{
						case eHierarchyRollupOption.RealTime: 
						{
							this.rdoHierarchyRollupOptionRealTime.Checked = true;
							break;
						}
						case eHierarchyRollupOption.API: 
						{
							this.rdoHierarchyRollupOptionAPI.Checked = true;
							break;
						}
					}
//End - JScott - Add Rollup Type to Hierarchy Properties - Part 2
					displayHierarchyFolderColor(GraphicsDirectory + "\\" + _hp.HierarchyColor + MIDGraphics.ClosedFolder);
				}
				
				if (_hp.HierarchyType == eHierarchyType.organizational)
				{
					if (_hp.PostingDate == Include.UndefinedDate)
					{
						this.lblPostingDate.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Posting_Date) + " " + MIDText.GetTextOnly(eMIDTextCode.lbl_Date_Not_Set);
					}
					else
					{
						this.lblPostingDate.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Posting_Date) + " " + _hp.PostingDate.ToShortDateString();
					}
				}
				else
				{
					this.lblPostingDate.Text = "";
				}

				eDataState dataState;
				if (FunctionSecurity.IsReadOnly)
				{
//					AllowUpdate = false;  Security changes 1/24/05 vg
					dataState = eDataState.ReadOnly;
					this.AcceptButton = this.btnCancel;
				}
				else
				{
//					AllowUpdate = true;  Security changes 1/24/05 vg
					dataState = eDataState.Updatable;
					this.AcceptButton = this.btnOK;
				}
				Format_Title(dataState, eMIDTextCode.frm_HierarchyProperties, _hp.HierarchyID);
				_loadingWindow = false;
			}
			catch (HierarchyConflictException)
			{
				throw;
			}
			catch( Exception exception )
			{
				HandleException(exception);
			}
		}

		public void NewHierarchy(int userRID, eHierarchyType hierarchyType)
		{
			try
			{
				_loadingWindow = true;
				_nameChanged = true;
				if (hierarchyType == eHierarchyType.organizational)
				{
					FunctionSecurity = new FunctionSecurityProfile (Convert.ToInt32(eSecurityFunctions.AdminHierarchiesOrg, CultureInfo.CurrentCulture));
				}
				else
				{
					FunctionSecurity = new FunctionSecurityProfile (Convert.ToInt32(eSecurityFunctions.AdminHierarchiesAlt, CultureInfo.CurrentCulture));
				}
				FunctionSecurity.SetFullControl();
				LoadCommon();
				ChangePending = true;
                // Begin Track #6348 - JSmith - Create Node under My Hier - get err message
                //btnApply.Enabled = true;
                btnApply.Enabled = false;
                // End Track #6348
				_hp = new HierarchyProfile(-1);
				if (userRID > 1)
				{
					txtHierarchyPropertyName.Text = _lblNewPersonalHierarchy;
				}
				else
				{
					txtHierarchyPropertyName.Text = _lblNewHierarchy;
				}
//Begin Track #4387 - JSmith - Merchandise Explorer performance
				//_node = new MIDHierarchyNode(txtHierarchyPropertyName.Text,_defaultImageIndex,_defaultImageIndex, SAB);
                _node = new MIDHierarchyNode(SAB, eTreeNodeType.ObjectNode, new HierarchyNodeProfile(Include.NoRID), txtHierarchyPropertyName.Text, Include.NoRID, _hp.Owner, FunctionSecurity, _defaultImageIndex, _defaultImageIndex, _defaultImageIndex, _defaultImageIndex, _hp.Owner);    
//End Track #4387
				_node.NodeChangeType = eChangeType.add;
                _node.isHierarchyRoot = true;
				gbxLevelProperties.Enabled = false;
				displayHierarchyFolderColor(GraphicsDirectory + "\\" + MIDGraphics.DefaultClosedFolder);
				lstHierarchyLevels.View = View.Details;
				lstHierarchyLevels.Columns.Add(_lblMIDLevels, (int) lstHierarchyLevels.Width - 4, 
					HorizontalAlignment.Left);
				lstHierarchyLevels.SmallImageList = MIDGraphics.ImageList;
				if (hierarchyType == eHierarchyType.organizational)  // user selected Organizational folder
				{
					this.radOrganizational.Checked = true;
					this.gbxHierarchyType.Enabled = false;
					_hp.HierarchyType = eHierarchyType.organizational;
//Begin - JScott - Add Rollup Type to Hierarchy Properties - Part 2
					this.rdoHierarchyRollupOptionAPI.Checked = true;
					this.gbxHierarchyRollupOption.Enabled = false;
					_hp.HierarchyRollupOption = eHierarchyRollupOption.API;
//End - JScott - Add Rollup Type to Hierarchy Properties - Part 2
				}
				else
				{
					this.gbxHierarchyType.Enabled = false;
					this.radOpen.Checked = true;
					this.gbxLevels.Enabled = false;
					_hp.HierarchyType = eHierarchyType.alternate;
//Begin - JScott - Add Rollup Type to Hierarchy Properties - Part 2
					this.rdoHierarchyRollupOptionRealTime.Checked = true;
					this.gbxHierarchyRollupOption.Enabled = true;
					_hp.HierarchyRollupOption = eHierarchyRollupOption.RealTime;
//End - JScott - Add Rollup Type to Hierarchy Properties - Part 2
				}
//Begin Track #3863 - JScott - OTS Forecast Level Defaults
				this.rdoHierarchyOTSTypeTotal.Checked = true;
				_hp.OTSPlanLevelType = eOTSPlanLevelType.Total;
//End Track #3863 - JScott - OTS Forecast Level Defaults
				_hp.HierarchyChangeType = eChangeType.add;
				_hp.HierarchyColor = Include.MIDDefault;
				_hp.Owner = userRID;
				//			this.Text = txtHierarchyPropertyName.Text + " " + MIDText.GetTextOnly(eMIDTextCode.lbl_Properties);
				Format_Title(eDataState.New, eMIDTextCode.frm_HierarchyProperties,null);

                _loadingWindow = false;
			}
			catch( Exception exception )
			{
				HandleException(exception);
			}
		}

		private void LoadCommon()
		{
			try
			{
				this.cboOTSType.DataSource = _OTSTypeDataTable;
				this.cboOTSType.DisplayMember = "TEXT_VALUE";
				this.cboOTSType.ValueMember = "TEXT_CODE";

				this.cboDisplayOption.DataSource = _displayOptionDataTable;
				this.cboDisplayOption.DisplayMember = "TEXT_VALUE";
				this.cboDisplayOption.ValueMember = "TEXT_CODE";

				this.cboIDFormat.DataSource = _IDFormatDataTable;
				this.cboIDFormat.DisplayMember = "TEXT_VALUE";
				this.cboIDFormat.ValueMember = "TEXT_CODE";

				SetReadOnly(FunctionSecurity.AllowUpdate);  //Security changes - 1/24/2005 vg
			}
			catch( Exception exception )
			{
				HandleException(exception);
			}
		}

		private void lstHierarchyLevels_Load(Hashtable hierarchyLevels )
		{
			try
			{
// Begin Track #3490 - Jeff Scott - Invalid folder color causing errors
//				string closedFolder;
// End Track #3490 - Jeff Scott - Invalid folder color causing errors
				int closedFolderIndex;
				int levelIndex = 0;
			
				for (levelIndex = 1; levelIndex <= hierarchyLevels.Count; levelIndex++)
				{
					HierarchyLevelProfile hlp = (HierarchyLevelProfile)hierarchyLevels[levelIndex];
// Begin Track #3490 - Jeff Scott - Invalid folder color causing errors
//					closedFolder = hlp.LevelColor + MIDGraphics.ClosedFolder;
//					closedFolderIndex = MIDGraphics.ImageIndex(closedFolder);
					closedFolderIndex = MIDGraphics.ImageIndexWithDefault(hlp.LevelColor, MIDGraphics.ClosedFolder);
// End Track #3490 - Jeff Scott - Invalid folder color causing errors
					if (hlp.LevelType == eHierarchyLevelType.Color)
					{
						_colorDefined = true;
					}
					else
						if (hlp.LevelType == eHierarchyLevelType.Size)
					{
						_sizeDefined = true;
					}
					// process size internally, do not display
//					if (hlp.LevelType != eHierarchyLevelType.Size)
//					{
						addItem(hlp, closedFolderIndex);
//					}
				}
				if (_hp.HierarchyLevels.Count > 0)
				{
					Level_Information_Load(_currentLevelIndex);
				}
			}
			catch( Exception exception )
			{
				HandleException(exception);
			}
		}

		public void Level_Information_Load(int selectedIndex)
		{
			try
			{
				if (_hp.HierarchyType == eHierarchyType.organizational)
				{
					if (selectedIndex < 0)
					{
						selectedIndex = 0;
					}
					_loadingLevel = true;
					_currentLevelIndex = selectedIndex;
					_levelDisplayOptionChanged = false;
					HierarchyListViewItem hlvi = (HierarchyListViewItem)lstHierarchyLevels.Items[selectedIndex];
					txtLevelName.Text = hlvi.Text;
					string imageFileName = GraphicsDirectory + "\\" + MIDGraphics.ImageFileName(hlvi.ImageIndex);
					displayLevelFolderColor(imageFileName);
					_selImageIndex = hlvi.ImageIndex;
					_selColorFile = hlvi.LevelColorFile;
					_selLevelColor = hlvi.LevelColor;
					_selDisplayOption = hlvi.LevelDisplayOption;
					_selIDFormat = hlvi.LevelIDFormat;
					this.numRequiredSize.Value = hlvi.LevelRequiredSize;
					this.numRangeFrom.Value = hlvi.LevelSizeRangeFrom;
					this.numRangeTo.Value = hlvi.LevelSizeRangeTo;
					switch (hlvi.LevelType)
					{
						case eHierarchyLevelType.Color:
						case eHierarchyLevelType.Size: 
						{
							this.numRequiredSize.Maximum = 50;
							this.numRangeTo.Maximum = 50;
							break;
						}
						default:
						{
							this.numRequiredSize.Maximum = 50;
							this.numRangeTo.Maximum = 50;
							break;
						}
					}
				
					this.cboOTSType.SelectedValue = (int) hlvi.LevelType;
					this.cboDisplayOption.SelectedValue = (int) hlvi.LevelDisplayOption;
					this.cboIDFormat.SelectedValue = (int) hlvi.LevelIDFormat;

					switch (hlvi.LevelLengthType)
					{
						case eLevelLengthType.unrestricted: 
						{
							this.radUnrestricted.Checked = true;
							this.numRequiredSize.Enabled = false;
							this.numRangeFrom.Enabled = false;
							this.numRangeTo.Enabled = false;
							break;
						}
						case eLevelLengthType.required: 
						{
							this.radRequiredSize.Checked = true;
							this.numRequiredSize.Enabled = true;
							this.numRangeFrom.Enabled = false;
							this.numRangeTo.Enabled = false;
							break;
						}
						case eLevelLengthType.range: 
						{
							this.radRange.Checked = true;
							this.numRequiredSize.Enabled = false;
							this.numRangeFrom.Enabled = true;
							this.numRangeTo.Enabled = true;
							break;
						}
					}

					if (hlvi.LevelType == eHierarchyLevelType.Color ||	// color and size must be a required size < 10
						hlvi.LevelType == eHierarchyLevelType.Size)		// or a range from 1-10
					{
						if (this.radUnrestricted.Enabled)
						{
							this.radUnrestricted.Enabled = false;
							this.radRange.Enabled = true;
							this.radRange.Checked = true;
						}
						if (this.numRangeFrom.Value == 0)
						{
							this.numRangeFrom.Value = 1;
						}
						if (this.numRangeTo.Value == 0)
						{
							this.numRangeTo.Value = 50;
						}
					}
					else
					{
						this.radUnrestricted.Enabled = true;
						this.radRequiredSize.Enabled = true;
					}

					_currentLevelType = hlvi.LevelType;
					//Begin Track #3863 - JScott - OTS Forecast Level Defaults
					//					if (hlvi.LevelType != eHierarchyLevelType.Planlevel)
					//					{
					//						pnlOTSPlvl.Enabled = false;
					//						this.radOTSPlvlRegular.Checked = false;
					//						this.radOTSPlvlTotal.Checked = false;
					//						if (hlvi.LevelType == eHierarchyLevelType.Color ||
					//							hlvi.LevelType == eHierarchyLevelType.Size)
					//						{
					//							cboOTSType.Enabled = false;
					//						}
					//						else
					//						{
					//							cboOTSType.Enabled = true;
					//						}
					//					}
					//					else
					//					{
					//						pnlOTSPlvl.Enabled = true;
					//						cboOTSType.Enabled = true;
					//						if (hlvi.LevelOTSPlanLevelType == eOTSPlanLevelType.Regular)
					//						{
					//							this.radOTSPlvlRegular.Checked = true;
					//						}
					//						else
					//							if (hlvi.LevelOTSPlanLevelType == eOTSPlanLevelType.Total)
					//						{
					//							this.radOTSPlvlTotal.Checked = true;
					//						}
					//						else
					//						{
					//							this.radOTSPlvlRegular.Checked = false;
					//							this.radOTSPlvlTotal.Checked = false;
					//						}
					//					}
					cboOTSType.Enabled = true;
					if (hlvi.LevelOTSPlanLevelType == eOTSPlanLevelType.Regular)
					{
						this.chkOTSTypeRegular.Checked = true;
					}
					else
						if (hlvi.LevelOTSPlanLevelType == eOTSPlanLevelType.Total)
					{
						this.chkOTSTypeTotal.Checked = true;
					}
					else
					{
						this.chkOTSTypeRegular.Checked = false;
						this.chkOTSTypeTotal.Checked = false;
					}
					//End Track #3863 - JScott - OTS Forecast Level Defaults

					if (hlvi.LevelType == eHierarchyLevelType.Style)
					{
						this.cboIDFormat.Enabled = true;
					}
					else
					{
						this.cboIDFormat.Enabled = false;
					}

					if (hlvi.PurgeDailyHistory != Include.Undefined)
					{
						this.txtPurgeDailyHistory.Text = Convert.ToString(hlvi.PurgeDailyHistory,CultureInfo.CurrentCulture);
					}
					else
					{
						this.txtPurgeDailyHistory.Text = string.Empty;
					}

					if (hlvi.PurgeWeeklyHistory != Include.Undefined)
					{
						this.txtPurgeWeeklyHistory.Text = Convert.ToString(hlvi.PurgeWeeklyHistory,CultureInfo.CurrentCulture);
					}
					else
					{
						this.txtPurgeWeeklyHistory.Text = string.Empty;
					}

					if (hlvi.PurgePlans != Include.Undefined)
					{
						this.txtPurgePlans.Text = Convert.ToString(hlvi.PurgePlans,CultureInfo.CurrentCulture);
					}
					else
					{
						this.txtPurgePlans.Text = string.Empty;
					}

                    //BEGIN TT#400-MD-VStuart-Add Header Purge Criteria by Header Type
                    //if (hlvi.PurgeHeaders != Include.Undefined)
                    //{
                    //    this.txtPurgeHeaders.Text = Convert.ToString(hlvi.PurgeHeaders,CultureInfo.CurrentCulture);
                    //}
                    //else
                    //{
                    //    this.txtPurgeHeaders.Text = string.Empty;
                    //}

                    if (hlvi.HtASN != Include.Undefined)
                    {
                        this.txtHtASN.Text = Convert.ToString(hlvi.HtASN, CultureInfo.CurrentCulture);
                    }
                    else
                    {
                        this.txtHtASN.Text = string.Empty;
                    }
                    if (hlvi.HtDropShip != Include.Undefined)
                    {
                        this.txtHtDropShip.Text = Convert.ToString(hlvi.HtDropShip, CultureInfo.CurrentCulture);
                    }
                    else
                    {
                        this.txtHtDropShip.Text = string.Empty;
                    }
                    if (hlvi.HtDummy != Include.Undefined)
                    {
                        this.txtHtDummy.Text = Convert.ToString(hlvi.HtDummy, CultureInfo.CurrentCulture);
                    }
                    else
                    {
                        this.txtHtDummy.Text = string.Empty;
                    }
                    if (hlvi.HtPurchase != Include.Undefined)
                    {
                        this.txtHtPurchase.Text = Convert.ToString(hlvi.HtPurchase, CultureInfo.CurrentCulture);
                    }
                    else
                    {
                        this.txtHtPurchase.Text = string.Empty;
                    }
                    if (hlvi.HtReceipt != Include.Undefined)
                    {
                        this.txtHtReceipt.Text = Convert.ToString(hlvi.HtReceipt, CultureInfo.CurrentCulture);
                    }
                    else
                    {
                        this.txtHtReceipt.Text = string.Empty;
                    }
                    if (hlvi.HtReserve != Include.Undefined)
                    {
                        this.txtHtReserve.Text = Convert.ToString(hlvi.HtReserve, CultureInfo.CurrentCulture);
                    }
                    else
                    {
                        this.txtHtReserve.Text = string.Empty;
                    }
                    if (hlvi.HtVSW != Include.Undefined)
                    {
                        this.txtHtVSW.Text = Convert.ToString(hlvi.HtVSW, CultureInfo.CurrentCulture);
                    }
                    else
                    {
                        this.txtHtVSW.Text = string.Empty;
                    }
                    if (hlvi.HtWorkupTotBy != Include.Undefined)
                    {
                        this.txtHtWorkupTotBy.Text = Convert.ToString(hlvi.HtWorkupTotBy, CultureInfo.CurrentCulture);
                    }
                    else
                    {
                        this.txtHtWorkupTotBy.Text = string.Empty;
                    }
                    //END TT#400-MD-VStuart-Add Header Purge Criteria by Header Type

					_purgeDailyHistoryChanged = false;
					_purgeWeeklyHistoryChanged = false;
					_purgePlansChanged = false;
                    //BEGIN TT#400-MD-VStuart-Add Header Purge Criteria by Header Type
                    //_purgeHeadersChanged = false;
                    _htASNChanged = false;
                    _htDropShipChanged = false;
                    _htDummyChanged = false;
                    _htPurchaseChanged = false;
                    _htReceiptChanged = false;
                    _htReserveChanged = false;
                    _htVSWChanged = false;
                    _htWorkupTotByChanged = false;
                    //END TT#400-MD-VStuart-Add Header Purge Criteria by Header Type
//Begin - JScott - Add Rollup Type to Hierarchy Properties - Part 2
//					_loadingLevel = false;
//End - JScott - Add Rollup Type to Hierarchy Properties - Part 2

                    // Begin TT#3985 - JSmith - Up and down arrows allow user to move levels after nodes are defined
                    picUpArrow.Enabled = true;
                    picDownArrow.Enabled = true;
                    btnDeleteLevel.Enabled = true;
                    btnNewLevel.Enabled = true;
                    // Cannot move or delete level if node exists
                    if (hlvi.LevelNodesExist)
                    {
                        picUpArrow.Enabled = false;
                        picDownArrow.Enabled = false;
                        btnDeleteLevel.Enabled = false;
                    }

                    // Cannot add level below color or size
                    if (hlvi.LevelType == eHierarchyLevelType.Color ||
                        hlvi.LevelType == eHierarchyLevelType.Size)
                    {
                        btnNewLevel.Enabled = false;
                    }

                    // Cannot add level if nodes exist at the next level
                    if (_currentLevelIndex + 1 < lstHierarchyLevels.Items.Count)
                    {
                        hlvi = (HierarchyListViewItem)lstHierarchyLevels.Items[_currentLevelIndex + 1];
                        if (hlvi.LevelNodesExist)
                        {
                            btnNewLevel.Enabled = false;
                        }
                    }
                    // End TT#3985 - JSmith - Up and down arrows allow user to move levels after nodes are defined
				}
				else
				{
					txtLevelName.Text = " ";
//Begin Track #3863 - JScott - OTS Forecast Level Defaults
//					this.radOTSPlvlRegular.Checked = false;
//					this.radOTSPlvlTotal.Checked = false;
					this.chkOTSTypeRegular.Checked = false;
					this.chkOTSTypeTotal.Checked = false;
//End Track #3863 - JScott - OTS Forecast Level Defaults
					this.radUnrestricted.Checked = false;
					this.radRequiredSize.Checked = false;
					this.radRange.Checked = false;
					this.numRequiredSize.Value = 0;
					this.numRangeFrom.Value = 0;
					this.numRangeTo.Value = 0;
					this.cboIDFormat.Enabled = false;
					this.txtPurgeDailyHistory.Text = string.Empty;
					this.txtPurgeWeeklyHistory.Text = string.Empty;
					this.txtPurgePlans.Text = string.Empty;
                    //BEGIN TT#400-MD-VStuart-Add Header Purge Criteria by Header Type
                    //this.txtPurgeHeaders.Text = string.Empty;
                    txtHtASN.Text = string.Empty;
                    txtHtDropShip.Text = string.Empty;
                    txtHtDummy.Text = string.Empty;
                    txtHtPurchase.Text = string.Empty;
                    txtHtReceipt.Text = string.Empty;
                    txtHtReserve.Text = string.Empty;
                    txtHtVSW.Text = string.Empty;
                    txtHtWorkupTotBy.Text = string.Empty;
                    //END TT#400-MD-VStuart-Add Header Purge Criteria by Header Type                                                        

				}
                //Begin - JScott - Add Rollup Type to Hierarchy Properties - Part 2
				_loadingLevel = false;
                //End - JScott - Add Rollup Type to Hierarchy Properties - Part 2

                if (FunctionSecurity.IsReadOnly)
				{
					SetReadOnly(false);  
				}
			}
			catch( Exception exception )
			{
				HandleException(exception);
			}
		}

		private void addItem(HierarchyLevelProfile hlp, int imageIndex)
		{
			try
			{
				HierarchyListViewItem item = new HierarchyListViewItem(hlp.LevelID, imageIndex);
				item.LevelID = hlp.LevelID;
				item.Level = hlp.Level;
				item.LevelColor = hlp.LevelColor;
				item.LevelLengthType = hlp.LevelLengthType;
				item.LevelRequiredSize = hlp.LevelRequiredSize;
				item.LevelSizeRangeFrom = hlp.LevelSizeRangeFrom;
				item.LevelSizeRangeTo = hlp.LevelSizeRangeTo;
				//Begin TT#988 - JScott - Add Active Only indicator to Override Low Level Model
				//item.LevelNodeCount = hlp.LevelNodeCount;
				item.LevelNodesExist = hlp.LevelNodesExist;
				//End TT#988 - JScott - Add Active Only indicator to Override Low Level Model
				item.LevelType = hlp.LevelType;
				item.LevelOTSPlanLevelType = hlp.LevelOTSPlanLevelType;
				item.LevelDisplayOption = hlp.LevelDisplayOption;
				item.LevelIDFormat = hlp.LevelIDFormat;
				item.PurgeDailyHistoryTimeframe = hlp.PurgeDailyHistoryTimeframe;
				item.PurgeDailyHistory = hlp.PurgeDailyHistory;
				item.PurgeWeeklyHistoryTimeframe = hlp.PurgeWeeklyHistoryTimeframe;
				item.PurgeWeeklyHistory = hlp.PurgeWeeklyHistory;
				item.PurgePlansTimeframe = hlp.PurgePlansTimeframe;
				item.PurgePlans = hlp.PurgePlans;
                //BEGIN TT#400-MD-VStuart-Add Header Purge Criteria by Header Type
                //item.PurgeHeadersTimeframe = hlp.PurgeHeadersTimeframe;
                //item.PurgeHeaders = hlp.PurgeHeaders;
                item.HtASNTimeframe = hlp.PurgeHtASNTimeframe;
                item.HtASN = hlp.PurgeHtASN;
                item.HtDropShipTimeframe = hlp.PurgeHtDropShipTimeframe;
                item.HtDropShip = hlp.PurgeHtDropShip;
                item.HtDummyTimeframe = hlp.PurgeHtDummyTimeframe;
                item.HtDummy = hlp.PurgeHtDummy;
                item.HtPurchaseTimeframe = hlp.PurgeHtPurchaseOrderTimeframe;
                item.HtPurchase = hlp.PurgeHtPurchaseOrder;
                item.HtReceiptTimeframe = hlp.PurgeHtReceiptTimeframe;
                item.HtReceipt = hlp.PurgeHtReceipt;
                item.HtReserveTimeframe = hlp.PurgeHtReserveTimeframe;
                item.HtReserve = hlp.PurgeHtReserve;
                item.HtVSWTimeframe = hlp.PurgeHtVSWTimeframe;
                item.HtVSW = hlp.PurgeHtVSW;
                item.HtWorkupTotByTimeframe = hlp.PurgeHtWorkUpTotTimeframe;
                item.HtWorkupTotBy = hlp.PurgeHtWorkUpTot;
                //END TT#400-MD-VStuart-Add Header Purge Criteria by Header Type

				this.lstHierarchyLevels.Items.Add(item);
			}
			catch( Exception exception )
			{
				HandleException(exception);
			}
		}

		private void addNewLevel(string levelID, int imageIndex, string levelColor, eHierarchyLevelType levelType)
		{
			try
			{
				gbxLevelProperties.Enabled = true;
				HierarchyListViewItem item = new HierarchyListViewItem(levelID, imageIndex);
				item.LevelChangeType = eChangeType.add;
				item.LevelID = levelID;
				item.Level = _hp.HierarchyLevels.Count + 1;
				item.LevelColor = levelColor;
				item.LevelLengthType = eLevelLengthType.unrestricted;
				item.LevelRequiredSize = 0;
				item.LevelSizeRangeFrom = 0;
				item.LevelSizeRangeTo = 0;
				//Begin TT#988 - JScott - Add Active Only indicator to Override Low Level Model
				//item.LevelNodeCount = 0;
				item.LevelNodesExist = false;
				//End TT#988 - JScott - Add Active Only indicator to Override Low Level Model
				item.LevelType = levelType;
				item.LevelOTSPlanLevelType = eOTSPlanLevelType.Undefined;
				item.LevelDisplayOption = eHierarchyDisplayOptions.NameOnly;
				item.LevelIDFormat = eHierarchyIDFormat.Unique;
				item.NewLevel = true;

				this.lstHierarchyLevels.Items.Insert(_currentLevelIndex+1, item);

				//			// instantiate new HierarchyLevelProfile object to pass information through interface
				//			HierarchyLevelProfile hlp = new HierarchyLevelProfile();
				//			_hp.HierarchyLevels.Add(item.Level, hlp);
			}
			catch( Exception exception )
			{
				HandleException(exception);
			}
		}

		private void displayHierarchyFolderColor(string name)
		{
			try
			{
				Image image;
				image = Image.FromFile(name);
				int imageWidth = image.Width;
				int imageHeight = image.Height;

				SizeF sizef = new SizeF(imageHeight / image.HorizontalResolution,
					imageWidth / image.VerticalResolution);
				float fScale = Math.Min(_buttonWidth / sizef.Width,
					_buttonHeight / sizef.Height);
				sizef.Width *= fScale;
				sizef.Height *= fScale;
				Size size = Size.Ceiling(sizef);
				Bitmap bitmap = new Bitmap(image, size);
				picHierarchyPropertyColor.Image = bitmap;
				picHierarchyPropertyColor.Size = new Size(_buttonWidth, _buttonHeight);
			}
			catch( Exception exception )
			{
				HandleException(exception);
			}
		}

		private void displayLevelFolderColor(string name)
		{
			Image image;
			try
			{
				image = Image.FromFile(name);
				int imageWidth = image.Width;
				int imageHeight = image.Height;

				SizeF sizef = new SizeF(imageHeight / image.HorizontalResolution,
					imageWidth / image.VerticalResolution);
				float fScale = Math.Min(_buttonWidth / sizef.Width,
					_buttonHeight / sizef.Height);
				sizef.Width *= fScale;
				sizef.Height *= fScale;
				Size size = Size.Ceiling(sizef);
				Bitmap bitmap = new Bitmap(image, size);
				picLevelColor.Image = bitmap;
				picLevelColor.Size = new Size(_buttonWidth, _buttonHeight);
			}

			catch( Exception exception )
			{
				HandleException(exception);
			}
		}

		private void displayUpArrow(string name)
		{
			try
			{
				Image image;
				image = Image.FromFile(name);
				int imageWidth = image.Width;
				int imageHeight = image.Height;

				SizeF sizef = new SizeF(imageHeight / image.HorizontalResolution,
					imageWidth / image.VerticalResolution);
				float fScale = Math.Min(_arrowWidth / sizef.Width,
					_arrowHeight / sizef.Height);
				sizef.Width *= fScale;
				sizef.Height *= fScale;
				Size size = Size.Ceiling(sizef);
				Bitmap bitmap = new Bitmap(image, size);
				picUpArrow.Image = bitmap;
				picUpArrow.Size = new Size(_arrowWidth, _arrowHeight);
			}
			catch( Exception exception )
			{
				HandleException(exception);
			}
		}

		private void displayDownArrow(string name)
		{
			try
			{
				Image image;
				image = Image.FromFile(name);
				int imageWidth = image.Width;
				int imageHeight = image.Height;

				SizeF sizef = new SizeF(imageHeight / image.HorizontalResolution,
					imageWidth / image.VerticalResolution);
				float fScale = Math.Min(_arrowWidth / sizef.Width,
					_arrowHeight / sizef.Height);
				sizef.Width *= fScale;
				sizef.Height *= fScale;
				Size size = Size.Ceiling(sizef);
				Bitmap bitmap = new Bitmap(image, size);
				picDownArrow.Image = bitmap;
				picDownArrow.Size = new Size(_arrowWidth, _arrowHeight);
			}
			catch( Exception exception )
			{
				HandleException(exception);
			}
		}

		private void picFolderColor_MouseHover(object sender, System.EventArgs e)
		{
			try
			{
				string message = MIDText.GetText(eMIDTextCode.tt_DoubleClickToChangeColor);
				ToolTip.Active = true; 
				ToolTip.SetToolTip((System.Windows.Forms.Control)sender, message);
			}
			catch( Exception exception )
			{
				HandleException(exception);
			}
		}

		private void picHierarchyPropertyColor_DoubleClick(object sender, System.EventArgs e)
		{
			try
			{
				string selColorFile;
				string selColor;
				frmSelectFolderColor frmSelectColor = new frmSelectFolderColor(SAB);
				frmSelectColor.initializeForm();
				frmSelectColor.ShowDialog(this);
				selColorFile = frmSelectColor.ColorSelectedFile;
				selColor = frmSelectColor.ColorSelected;
				if (selColorFile != null)
				{
					ChangePending = true;
					btnApply.Enabled = true;
					displayHierarchyFolderColor(selColorFile);
					_hp.HierarchyColor = selColor;
					_folderColorChanged = true;
					if (_hp.HierarchyChangeType == eChangeType.none)
					{
						_hp.HierarchyChangeType = eChangeType.update;
					}
				}
			}
			catch( Exception exception )
			{
				HandleException(exception);
			}
		}

		private void btnCancel_Click(object sender, System.EventArgs e)
		{
			try
			{
//				HierPropertyChangeEventArgs ea = new HierPropertyChangeEventArgs(_node);
//				if (OnHierPropertyChangeHandler != null)
//				{
//					OnHierPropertyChangeHandler(this, ea);
//				}
//				_cancelPressed = true;
				_node.NodeChangeType = eChangeType.none;
				//			this.Close();
				Cancel_Click();
			}
			catch( Exception exception )
			{
				HandleException(exception);
			}
		}

		private void btnOK_Click(object sender, System.EventArgs e)
		{
			try
			{
				OK_Click();
			}
			catch( Exception exception )
			{
				HandleException(exception);
			}
		}

		private void btnApply_Click(object sender, EventArgs e)
		{
			try
			{
				SaveChanges();
			}
			catch (Exception exception)
			{
				HandleException(exception);
			}
		}

		override protected bool SaveChanges()
		{
			try
			{
				ErrorFound = false;
				if (_levelChangePending) // update pending level changes before continuing
				{
					SaveLevelChanges();
				}
				else
				{
					//Begin TT#474 - JScott - Create My Hier w dash - errors
					//_errors = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_FollowingErrors);
					_errors = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_FollowingErrors, false);
					//End TT#474 - JScott - Create My Hier w dash - errors
				}

				if (ChangePending || _hp.HierarchyChangeType == eChangeType.add)
				{
					SaveHierarchyChanges();
				}
			}
			catch( Exception exception )
			{
				HandleException(exception);
			}

			return true;
		}

		private void SaveHierarchyChanges()
		{
			HierarchyLevelProfile hlp;
			try
			{
				if (_hp.HierarchyChangeType == eChangeType.none)
				{
					_hp.HierarchyChangeType = eChangeType.update;
				}

				//Begin TT#195 - JScott - When I go in to the application the 1st time I  created a MY Hierarchy, and the name was the same as an Alternate Hierarchy, I received a Database Unique Constraint Violation. 
				txtHierarchyPropertyName.Text = txtHierarchyPropertyName.Text.Trim();

				//End TT#195 - JScott - When I go in to the application the 1st time I  created a MY Hierarchy, and the name was the same as an Alternate Hierarchy, I received a Database Unique Constraint Violation. 
				if (DataIsValid())
				{
					HierPropertyChangeEventArgs ea;
					
					_hp.HierarchyID = txtHierarchyPropertyName.Text;
					//Begin Track #6201 - JScott - Store Count removed from attr sets
					//_node.Text = txtHierarchyPropertyName.Text;
					_node.InternalText = txtHierarchyPropertyName.Text;
					//End Track #6201 - JScott - Store Count removed from attr sets
					((HierarchyNodeProfile)_node.Profile).NodeColor = _hp.HierarchyColor;
                   _node.NodeChangeType = _hp.HierarchyChangeType;
					ea = new HierPropertyChangeEventArgs(_node);
					if (_node.isMainUserFolder)
					{
						SAB.ClientServerSession.UpdateMyHierarchy(txtHierarchyPropertyName.Text, _hp.HierarchyColor);
					}
					else
					{
                        if (_hp.Owner != Include.GlobalUserRID)  // Issue 3806 
						{
							ea.IsMyHierarchy = true;
						}
						int level = 1;
                        int addedLevel = int.MaxValue;  // TT#3985 - JSmith - Up and down arrows allow user to move levels after nodes are defined
                        HierarchyProfile origHP = SAB.HierarchyServerSession.GetHierarchyData(_hp.Key);  // TT#1911-MD - JSmith - Database Error on Update
						_hp.HierarchyLevels.Clear();
						foreach(HierarchyListViewItem hlvi in lstHierarchyLevels.Items)
						{
							// do not add new levels that are deleted
							if (hlvi.LevelChangeType == eChangeType.delete &&
								hlvi.NewLevel)
							{
								continue;
							}
							hlp = new HierarchyLevelProfile(level);
							hlp.Level = level;
                            // Begin TT#1911-MD - JSmith - Database Error on Update
                            //// Begin TT#3985 - JSmith - Up and down arrows allow user to move levels after nodes are defined
                            ////hlp.LevelChangeType = hlvi.LevelChangeType;
                            //if (hlvi.LevelChangeType == eChangeType.add)
                            //{
                            //    addedLevel = hlp.Level;
                            //}
                            //if (hlvi.LevelChangeType != eChangeType.delete &&
                            //    (hlvi.Level > addedLevel ||
                            //    !hlvi.LevelNodesExist))
                            //{
                            //    hlp.LevelChangeType = eChangeType.add;
                            //}
                            
                            if ((hlvi.LevelChangeType == eChangeType.add
                                && origHP.HierarchyLevels.ContainsKey(hlp.Level))
                                || hlvi.LevelChangeType == eChangeType.none)
                            {
                                hlp.LevelChangeType = eChangeType.update;
                            }
                            // End TT#1911-MD - JSmith - Database Error on Update
                            else 
                            {
                                hlp.LevelChangeType = hlvi.LevelChangeType;
                            }
                            // End TT#3985 - JSmith - Up and down arrows allow user to move levels after nodes are defined
                            
							hlp.LevelColor = hlvi.LevelColor;
							hlp.LevelID = hlvi.LevelID;
							hlp.LevelLengthType = hlvi.LevelLengthType;
							hlp.LevelRequiredSize = hlvi.LevelRequiredSize;
							hlp.LevelSizeRangeFrom = hlvi.LevelSizeRangeFrom;
							hlp.LevelSizeRangeTo = hlvi.LevelSizeRangeTo;
							hlp.LevelType = hlvi.LevelType;
							hlp.LevelOTSPlanLevelType = hlvi.LevelOTSPlanLevelType;
							hlp.LevelDisplayOption = hlvi.LevelDisplayOption;
							if (hlp.LevelType == eHierarchyLevelType.Style)  // only allow option to be set on style
							{
								hlp.LevelIDFormat = hlvi.LevelIDFormat;
							}
							else
							{
								hlp.LevelIDFormat = eHierarchyIDFormat.Unique;
							}
							hlp.PurgeDailyHistoryTimeframe = hlvi.PurgeDailyHistoryTimeframe;
							hlp.PurgeDailyHistory = hlvi.PurgeDailyHistory;
							hlp.PurgeWeeklyHistoryTimeframe = hlvi.PurgeWeeklyHistoryTimeframe;
							hlp.PurgeWeeklyHistory = hlvi.PurgeWeeklyHistory;
							hlp.PurgePlansTimeframe = hlvi.PurgePlansTimeframe;
							hlp.PurgePlans = hlvi.PurgePlans;
                            //BEGIN TT#400-MD-VStuart-Add Header Purge Criteria by Header Type
                            //hlp.PurgeHeadersTimeframe = hlvi.PurgeHeadersTimeframe;
                            //hlp.PurgeHeaders = hlvi.PurgeHeaders;
                            hlp.PurgeHtASNTimeframe = hlvi.HtASNTimeframe;
                            hlp.PurgeHtASN = hlvi.HtASN;
                            hlp.PurgeHtDropShipTimeframe = hlvi.HtDropShipTimeframe;
                            hlp.PurgeHtDropShip = hlvi.HtDropShip;
                            hlp.PurgeHtDummyTimeframe = hlvi.HtDummyTimeframe;
                            hlp.PurgeHtDummy = hlvi.HtDummy;
                            hlp.PurgeHtPurchaseOrderTimeframe = hlvi.HtPurchaseTimeframe;
                            hlp.PurgeHtPurchaseOrder = hlvi.HtPurchase;
                            hlp.PurgeHtReceiptTimeframe = hlvi.HtReceiptTimeframe;
                            hlp.PurgeHtReceipt = hlvi.HtReceipt;
                            hlp.PurgeHtReserveTimeframe = hlvi.HtReserveTimeframe;
                            hlp.PurgeHtReserve = hlvi.HtReserve;
                            hlp.PurgeHtVSWTimeframe = hlvi.HtVSWTimeframe;
                            hlp.PurgeHtVSW = hlvi.HtVSW;
                            hlp.PurgeHtWorkUpTotTimeframe = hlvi.HtWorkupTotByTimeframe;
                            hlp.PurgeHtWorkUpTot = hlvi.HtWorkupTotBy;                                         
                            //END TT#400-MD-VStuart-Add Header Purge Criteria by Header Type                                                        
                            hlp.LevelNodesExist = hlvi.LevelNodesExist;  // TT#4571 - JSmith - Received the Database Foreign Key Violation when changing Display option on Hierarchy Level                       
                            _hp.HierarchyLevels.Add(hlp.Level, hlp);

							if (hlvi.LevelColorChanged || hlvi.LevelDisplayOptionChanged)
							{
								LevelChange lcc = new LevelChange();
								lcc.Level = hlvi.Level;
								if (hlvi.LevelColorChanged)
								{
									lcc.LevelColorChanged = true;
									lcc.LevelColor = hlvi.LevelColor;
								}
								if (hlvi.LevelDisplayOptionChanged)
								{
									lcc.LevelDisplayOptionChanged = true;
									lcc.LevelDisplayOption = hlvi.LevelDisplayOption;
								}
								ea.LevelChanges.Add(lcc);
							}

							level++;
						}

						_hp = SAB.HierarchyServerSession.HierarchyUpdate(_hp);
						if (_node.NodeChangeType == eChangeType.add)
						{
                            ea.Node = (MIDHierarchyNode)_node.CloneNode(SAB.HierarchyServerSession.GetNodeData(_hp.HierarchyRootNodeRID));
                            ea.Node.HierarchyRID = _hp.Key;
                            // Begin Track #6312 - JSmith - Cut/paste issues
                            ea.Node.HierarchyType = _hp.HierarchyType;
                            // End Track #6312
							//ea.Node.NodeRID = _hp.HierarchyRootNodeRID;
                            AddSecurityToHierarchy(false, Include.NoRID, ea.Node.Profile.Key, eSecurityLevel.Allow);
						}
					}
					if (OnHierPropertyChangeHandler != null && (_nameChanged || _levelDisplayOptionChanged || _folderColorChanged))
					{
                        //ea.Node = (MIDHierarchyNode)ea.Node.CloneNode(SAB.HierarchyServerSession.GetNodeData(_hp.HierarchyRootNodeRID));
						OnHierPropertyChangeHandler(this, ea);
						_changeEventThrown = true;
						_nameChanged = false;
						_levelDisplayOptionChanged = false;
						_folderColorChanged = false;
					}

					ChangePending = false;
					btnApply.Enabled = false;
				}
				else
				{
					ErrorFound = true;
					MessageBox.Show (_errors,  this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}
			catch( Exception exception )
			{
				HandleException(exception, true);
			}
		}

		private void AddSecurityToHierarchy(bool isForGroup, int aGroupRID, int aNodeRID, eSecurityLevel aSecurityLevel)
		{
			SecurityAdmin security = new SecurityAdmin();
			try
			{
				security.OpenUpdateConnection();
				if (isForGroup)
				{
					security.AssignGroupNode(aGroupRID, aNodeRID, eSecurityActions.FullControl, eDatabaseSecurityTypes.Allocation, aSecurityLevel);
					security.AssignGroupNode(aGroupRID, aNodeRID, eSecurityActions.FullControl, eDatabaseSecurityTypes.Chain, aSecurityLevel);
					security.AssignGroupNode(aGroupRID, aNodeRID, eSecurityActions.FullControl, eDatabaseSecurityTypes.Store, aSecurityLevel);
				}
				else
				{
					security.AssignUserNode(SAB.ClientServerSession.UserRID, aNodeRID, eSecurityActions.FullControl, eDatabaseSecurityTypes.Allocation, aSecurityLevel);
					security.AssignUserNode(SAB.ClientServerSession.UserRID, aNodeRID, eSecurityActions.FullControl, eDatabaseSecurityTypes.Chain, aSecurityLevel);
					security.AssignUserNode(SAB.ClientServerSession.UserRID, aNodeRID, eSecurityActions.FullControl, eDatabaseSecurityTypes.Store, aSecurityLevel);
				}
				security.CommitData();
				SAB.HierarchyServerSession.AddToSecurityNodeList(aNodeRID);
			}
			catch( Exception exception )
			{
				HandleException(exception, true);
			}
			finally
			{
				security.CloseUpdateConnection();
			}
		}

		private bool DataIsValid()
		{
			try
			{
				string error = null;
				bool dataIsValid = true;
				if (this.txtHierarchyPropertyName.Text == "")
				{
					error = AddErrorMessage(eMIDTextCode.msg_NameRequired);
					dataIsValid = false;
					ErrorProvider.SetError (this.txtHierarchyPropertyName,error);
				}
				else
				{
					ErrorProvider.SetError (this.txtHierarchyPropertyName,"");
//Begin Track #4182 - JScott - Error when renaming an Alternate Hierarchy
//					if ( _hp.HierarchyChangeType == eChangeType.add)  // make sure ID is unique
					if (_hp.HierarchyChangeType == eChangeType.add ||
						(_hp.HierarchyChangeType == eChangeType.update && txtHierarchyPropertyName.Text != _hp.HierarchyID))  // make sure ID is unique
//End Track #4182 - JScott - Error when renaming an Alternate Hierarchy
					{
						HierarchyProfile hp = SAB.HierarchyServerSession.GetHierarchyData(txtHierarchyPropertyName.Text);
                        if (hp.Key != -1)
                        {
                            error = AddErrorMessage(eMIDTextCode.msg_HierarchyAlreadyExists);
                            dataIsValid = false;
                            ErrorProvider.SetError(this.txtHierarchyPropertyName, error);
                        }
                        //Begin Track #5465 - JSmith - Error creating hierarchy
                        else
                        {
                            HierarchyNodeProfile hnp = SAB.HierarchyServerSession.GetNodeData(txtHierarchyPropertyName.Text);
                            if (hnp.Key != -1)
                            {
                                error = AddErrorMessage(eMIDTextCode.msg_DuplicateProductID);
                                dataIsValid = false;
                                ErrorProvider.SetError(this.txtHierarchyPropertyName, error);
                            }
                        }
                        //End Track #5465
					}
				}
			
				return dataIsValid;
			}
			catch( Exception exception )
			{
				HandleException(exception);
				return false;
			}
		}

		private string AddErrorMessage(eMIDTextCode textCode)
		{
			try
			{
				//Begin TT#474 - JScott - Create My Hier w dash - errors
				//string error = SAB.ClientServerSession.Audit.GetText(textCode);
				string error = SAB.ClientServerSession.Audit.GetText(textCode, false);
				//End TT#474 - JScott - Create My Hier w dash - errors
				_errors += Environment.NewLine + "     " + error;
				return error;
			}
			catch( Exception exception )
			{
				HandleException(exception);
				return null;
			}
		}

		private void picLevelColor_DoubleClick(object sender, System.EventArgs e)
		{
			try
			{
                frmSelectFolderColor frmSelectColor = new frmSelectFolderColor(SAB);
				frmSelectColor.initializeForm();
				frmSelectColor.ShowDialog(this);
				if (frmSelectColor.ColorSelected != null)
				{
					_selColorFile = frmSelectColor.ColorSelectedFile;
					_selLevelColor = frmSelectColor.ColorSelected;
// Begin Track #3490 - Jeff Scott - Invalid folder color causing errors
//					string closedFolder = _selLevelColor + MIDGraphics.ClosedFolder;
//					_selImageIndex = MIDGraphics.ImageIndex(closedFolder);
					_selImageIndex = MIDGraphics.ImageIndexWithDefault(_selLevelColor, MIDGraphics.ClosedFolder);
// End Track #3490 - Jeff Scott - Invalid folder color causing errors
					displayLevelFolderColor(_selColorFile);
					_levelChangePending = true;
                    // Begin TT#21 - JSmith - Folder colors do not change when updated in Hierarchy Properties
                    _folderColorChanged = true;
                    // End TT#21
				}
			}
			catch( Exception exception )
			{
				HandleException(exception);
			}
		}

		private void radOrganizational_CheckedChanged(object sender, System.EventArgs e)
		{
			try
			{
				if (this.radOrganizational.Checked == true)
				{
					this.gbxLevels.Enabled = true;
					_hp.HierarchyType = eHierarchyType.organizational;
					if (!_loadingLevel && !_loadingWindow)
					{
						ChangePending = true;
//Begin - JScott - Add Rollup Type to Hierarchy Properties - Part 2
						this.rdoHierarchyRollupOptionAPI.Checked = true;
						this.gbxHierarchyRollupOption.Enabled = false;
//End - JScott - Add Rollup Type to Hierarchy Properties - Part 2
						btnApply.Enabled = true;
					}
				}

			}
			catch( Exception exception )
			{
				HandleException(exception);
			}
		}

		private void radOpen_CheckedChanged(object sender, System.EventArgs e)
		{
			try
			{
				if (this.radOpen.Checked == true)
				{
					if (!_loadingLevel && !_loadingWindow)
					{
						if (this.lstHierarchyLevels.Items.Count == 0)
						{
							ChangePending = true;
							btnApply.Enabled = true;
							_hp.HierarchyType = eHierarchyType.alternate;
							this.gbxLevels.Enabled = false;
						}
						else
							if (_hp.HierarchyType == eHierarchyType.organizational &&
							this.lstHierarchyLevels.Items.Count > 0)
						{
							if (MessageBox.Show (SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_LevelsWillBeDeleted),  this.Text,
								MessageBoxButtons.OKCancel, MessageBoxIcon.Information)
								== DialogResult.OK) 
							{
								ChangePending = true;
								btnApply.Enabled = true;
								_hp.HierarchyType = eHierarchyType.alternate;
								this.lstHierarchyLevels.Items.Clear();
								_hp.HierarchyLevels.Clear();
								Level_Information_Load(-1);  // clear level information
								this.gbxLevels.Enabled = false;
								_levelChangePending = false;
								_currentLevelIndex = -1;
							}
							else  // if won't delete levels, put back to organizational
							{
								this.radOrganizational.Checked = true;
							}
						}
//Begin - JScott - Add Rollup Type to Hierarchy Properties - Part 2
						if (this.radOpen.Checked == true)
						{
							this.rdoHierarchyRollupOptionRealTime.Checked = true;
							this.gbxHierarchyRollupOption.Enabled = true;
							_hp.HierarchyRollupOption = eHierarchyRollupOption.RealTime;
						}
//End - JScott - Add Rollup Type to Hierarchy Properties - Part 2
					}
				}
			}
			catch( Exception exception )
			{
				HandleException(exception);
			}
		}

//Begin Track #3863 - JScott - OTS Forecast Level Defaults
		private void rdoHierarchyOTSTypeRegular_CheckedChanged(object sender, System.EventArgs e)
		{
			try
			{
				if (this.rdoHierarchyOTSTypeRegular.Checked == true)
				{
					_hp.OTSPlanLevelType = eOTSPlanLevelType.Regular;
					if (!_loadingLevel && !_loadingWindow)
					{
						ChangePending = true;
						btnApply.Enabled = true;
					}
				}
			}
			catch( Exception exception )
			{
				HandleException(exception);
			}
		}

		private void rdoHierarchyOTSTypeTotal_CheckedChanged(object sender, System.EventArgs e)
		{
			try
			{
				if (this.rdoHierarchyOTSTypeTotal.Checked == true)
				{
					_hp.OTSPlanLevelType = eOTSPlanLevelType.Total;
					if (!_loadingLevel && !_loadingWindow)
					{
						ChangePending = true;
						btnApply.Enabled = true;
					}
				}
			}
			catch( Exception exception )
			{
				HandleException(exception);
			}
		}

//End Track #3863 - JScott - OTS Forecast Level Defaults
//Begin - JScott - Add Rollup Type to Hierarchy Properties - Part 2
		private void rdoHierarchyRollupOptionRealTime_CheckedChanged(object sender, System.EventArgs e)
		{
			try
			{
				if (this.rdoHierarchyRollupOptionRealTime.Checked == true)
				{
					_hp.HierarchyRollupOption = eHierarchyRollupOption.RealTime;
					if (!_loadingLevel && !_loadingWindow)
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

		private void rdoHierarchyRollupOptionAPI_CheckedChanged(object sender, System.EventArgs e)
		{
			try
			{
				if (this.rdoHierarchyRollupOptionAPI.Checked == true)
				{
					_hp.HierarchyRollupOption = eHierarchyRollupOption.API;
					if (!_loadingLevel && !_loadingWindow)
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

//End - JScott - Add Rollup Type to Hierarchy Properties - Part 2
		private void lstHierarchyLevels_SelectedIndexChanged(object sender, EventArgs e)
		{
			try
			{
				if (lstHierarchyLevels.Items.Count != 0)
				{
					if (lstHierarchyLevels.SelectedItems.Count !=0)
					{
						if (_levelChangePending)
						{
							if (MessageBox.Show (SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_SavePendingChanges),  "Save Changes",
								MessageBoxButtons.YesNo, MessageBoxIcon.Question)
								== DialogResult.Yes) 
							{
								SaveLevelChanges();
							}
							_levelChangePending = false;
						}
						_currentLevelIndex = lstHierarchyLevels.SelectedItems[0].Index;
						Level_Information_Load(_currentLevelIndex);
					}
					else
					{
						_selectedItem = (HierarchyListViewItem)this.lstHierarchyLevels.FocusedItem;
						if (_selectedItem != null)
						{
							if(_upArrowClick || _downArrowClick) // item is removed, do not change index
							{
							}
							else
							{
								if (lstHierarchyLevels.FocusedItem.Index != -1)
								{
									if (_levelChangePending)
									{
										if (MessageBox.Show (SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_SavePendingChanges),  "Save Changes",
											MessageBoxButtons.YesNo, MessageBoxIcon.Question)
											== DialogResult.Yes) 
										{
											SaveLevelChanges();
										}
										_levelChangePending = false;
									}
									_currentLevelIndex = lstHierarchyLevels.FocusedItem.Index;
									Level_Information_Load(_currentLevelIndex);
								}
							}
						}
						else
						{
							if (_levelChangePending)
							{
								if (MessageBox.Show (SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_SavePendingChanges),  "Save Changes",
									MessageBoxButtons.YesNo, MessageBoxIcon.Question)
									== DialogResult.Yes) 
								{
									SaveLevelChanges();
								}
								_levelChangePending = false;
							}
							_currentLevelIndex = 0;
							Level_Information_Load(_currentLevelIndex);
						}
					}
				}
			}
			catch( Exception exception )
			{
				HandleException(exception);
			}
		}

		private void picUpArrow_Click(object sender, System.EventArgs e)
		{
			try
			{
				if (_currentLevelIndex == 0)
				{
					MessageBox.Show (SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_FirstLevelNotUp), this.Text,
						MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				}
				else
				{
					_upArrowClick = true;
					int holdEditItemIndex = _currentLevelIndex;
					HierarchyListViewItem hlvi = (HierarchyListViewItem)lstHierarchyLevels.Items[_currentLevelIndex];
					//Begin TT#988 - JScott - Add Active Only indicator to Override Low Level Model
					//if (hlvi.LevelNodeCount > 0)
					if (hlvi.LevelNodesExist)
					//End TT#988 - JScott - Add Active Only indicator to Override Low Level Model
					{
						MessageBox.Show (SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_CanNotMoveLevel), this.Text,
							MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					}
					else
						if (hlvi.LevelType == eHierarchyLevelType.Style ||
						hlvi.LevelType == eHierarchyLevelType.Color ||
						hlvi.LevelType == eHierarchyLevelType.Size)
					{
						MessageBox.Show (SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_CanNotMoveLevel), this.Text,
							MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					}
					else
					{
						lstHierarchyLevels.Items.RemoveAt(_currentLevelIndex);
						lstHierarchyLevels.Items.Insert(_currentLevelIndex - 1, hlvi);
						_currentLevelIndex = holdEditItemIndex - 1;
						lstHierarchyLevels.Items[_currentLevelIndex].Selected = true;
						lstHierarchyLevels.Items[_currentLevelIndex].Focused = true;
						ChangePending = true;
						btnApply.Enabled = true;
					}
					_upArrowClick = false;
				}
			}
			catch( Exception exception )
			{
				HandleException(exception);
			}
		}

		private void picDownArrow_Click(object sender, System.EventArgs e)
		{
			try
			{
				if (_currentLevelIndex + 1 == lstHierarchyLevels.Items.Count)
				{
					MessageBox.Show (SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_LastLevelNotDown), this.Text,
						MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				}
				else
				{
					_downArrowClick = true;
					int holdEditItemIndex = _currentLevelIndex;
					HierarchyListViewItem hlvi = (HierarchyListViewItem)lstHierarchyLevels.Items[_currentLevelIndex];
					HierarchyListViewItem tohlvi = (HierarchyListViewItem)lstHierarchyLevels.Items[_currentLevelIndex + 1];
					//Begin TT#988 - JScott - Add Active Only indicator to Override Low Level Model
					//if (tohlvi.LevelNodeCount > 0)
					if (tohlvi.LevelNodesExist)
					//End TT#988 - JScott - Add Active Only indicator to Override Low Level Model
					{
						MessageBox.Show (SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_CanNotMoveLevel), this.Text,
							MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					}
					else
						if (hlvi.LevelType == eHierarchyLevelType.Style ||
						hlvi.LevelType == eHierarchyLevelType.Color ||
						hlvi.LevelType == eHierarchyLevelType.Size)
					{
						MessageBox.Show (SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_CanNotMoveLevel), this.Text,
							MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					}
					else
						if (tohlvi.LevelType == eHierarchyLevelType.Style ||
						tohlvi.LevelType == eHierarchyLevelType.Color ||
						tohlvi.LevelType == eHierarchyLevelType.Size)
					{
						MessageBox.Show (SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_CanNotMoveLevelBelowStyle), this.Text,
							MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					}
					else
					{
						lstHierarchyLevels.Items.RemoveAt(_currentLevelIndex);
						lstHierarchyLevels.Items.Insert(_currentLevelIndex + 1, hlvi);
						_currentLevelIndex = holdEditItemIndex + 1;
						lstHierarchyLevels.Items[_currentLevelIndex].Selected = true;
						lstHierarchyLevels.Items[_currentLevelIndex].Focused = true;
						ChangePending = true;
						btnApply.Enabled = true;
					}
					_downArrowClick = false;
				}
			}
			catch( Exception exception )
			{
				HandleException(exception);
			}
		}

		//		private void tabHierarchyProperties_SelectionChangeCommitted(object sender, System.EventArgs e)
		//		{
		//			TabPage tpg = this.tabHierarchyProperties.SelectedTab;
		//			if (tpg.Text == _lblMIDLevels)
		//			{
		//				this.ActiveControl = lstHierarchyLevels;
		//			}
		//		}

		private void btnDeleteLevel_Click(object sender, System.EventArgs e)
		{
			try
			{
				if (lstHierarchyLevels.Items.Count != 0)
				{
					_selectedItem = (HierarchyListViewItem)lstHierarchyLevels.SelectedItems[0];
					if (_selectedItem != null)
					{
						if (_selectedItem.Level < lstHierarchyLevels.Items.Count)
						{
							MessageBox.Show (SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_DeleteLowerLevels), this.Text,
								MessageBoxButtons.OK, MessageBoxIcon.Error);
						}
						//Begin TT#988 - JScott - Add Active Only indicator to Override Low Level Model
						//else if (_selectedItem.LevelNodeCount != 0)
						else if (_selectedItem.LevelNodesExist)
						//End TT#988 - JScott - Add Active Only indicator to Override Low Level Model
						{
							MessageBox.Show (SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_LevelHasNodesCanNotDelete), this.Text,
								MessageBoxButtons.OK, MessageBoxIcon.Error);
						}
						else
						{
							string message = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ConfirmDelete);
							message = message.Replace("{0}", _selectedItem.Text);
							if (MessageBox.Show (message,  "Confirm Delete",
								MessageBoxButtons.YesNo, MessageBoxIcon.Question)
								== DialogResult.Yes) 
							{
								ChangePending = true;
								btnApply.Enabled = true;
								_selectedItem.LevelChangeType = eChangeType.delete;
								_selectedItem.BackColor = System.Drawing.Color.LightGray;
								if (lstHierarchyLevels.Items.Count > 0)
								{
									if (_currentLevelIndex > 0)
									{
										_currentLevelIndex--;
									}
									lstHierarchyLevels.Items[_currentLevelIndex].Selected = true;
								}
							}
						}
					}
					if (lstHierarchyLevels.Items.Count == 0)
					{
						txtLevelName.Text = " ";
					}
				}
			}
			catch( Exception exception )
			{
				HandleException(exception);
			}
		}

		private void btnNewLevel_Click(object sender, System.EventArgs e)
		{
			try
			{
				bool addLevel = true;
				if (_levelChangePending)
				{
					if (MessageBox.Show (SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_SavePendingChanges),  "Save Changes",
						MessageBoxButtons.YesNo, MessageBoxIcon.Question)
						== DialogResult.Yes) 
					{
						SaveLevelChanges();
					}
					_levelChangePending = false;
				}

				if (_currentLevelIndex > -1)
				{
					HierarchyListViewItem hlvi = null;
					if (_currentLevelIndex + 1 <= lstHierarchyLevels.Items.Count)
					{
						hlvi = (HierarchyListViewItem)lstHierarchyLevels.Items[_currentLevelIndex];
						if (hlvi.LevelType == eHierarchyLevelType.Style ||
							hlvi.LevelType == eHierarchyLevelType.Color ||
							hlvi.LevelType == eHierarchyLevelType.Size)
						{
							MessageBox.Show (SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_NoLevelBelowStyle), this.Text,
								MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
							addLevel = false;
						}
					}

					if (_currentLevelIndex + 1 < lstHierarchyLevels.Items.Count)
					{
						hlvi = (HierarchyListViewItem)lstHierarchyLevels.Items[_currentLevelIndex + 1];
						//Begin TT#988 - JScott - Add Active Only indicator to Override Low Level Model
						//if (hlvi.LevelNodeCount != 0)
						if (hlvi.LevelNodesExist)
						//End TT#988 - JScott - Add Active Only indicator to Override Low Level Model
						{
							MessageBox.Show (SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_NextLevelHasNodes), this.Text,
								MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
							addLevel = false;
						}
					}
				}

				if (addLevel)
				{
					if (lstHierarchyLevels.SelectedItems.Count > 0)
					{
						_selectedItem = (HierarchyListViewItem)lstHierarchyLevels.SelectedItems[0];
						string message = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ConfirmInsertLevel);
						message = message.Replace("{0}", _selectedItem.Text);
						if (MessageBox.Show (message,  "Confirm Insert",
							MessageBoxButtons.YesNo, MessageBoxIcon.Question)
							== DialogResult.No) 
						{
							addLevel = false;
						}
					}
				}

				if (addLevel)
				{
					addNewLevel(_lblNewLevelName, _defaultImageIndex, Include.MIDDefault, eHierarchyLevelType.Undefined);
					++_currentLevelIndex;
					lstHierarchyLevels.Items[_currentLevelIndex].Selected = true;
					lstHierarchyLevels.Items[_currentLevelIndex].Focused = true;
					this.ActiveControl = txtLevelName;
					txtLevelName.Focus();
					txtLevelName.Text = _lblNewLevelName;
					txtLevelName.Select(0, _lblNewLevelName.Length);
					displayLevelFolderColor(_defaultImageFileName);
					ChangePending = true;
					btnApply.Enabled = true;
				}
			}
			catch( Exception exception )
			{
				HandleException(exception);
			}
		}

		private void btnUpdate_Click(object sender, System.EventArgs e)
		{
			try
			{
				ChangePending = true;
				btnApply.Enabled = true;
				SaveLevelChanges();
			}
			catch( Exception exception )
			{
				HandleException(exception);
			}
		}

		private void SaveLevelChanges()
		{
			try
			{
				//Begin TT#474 - JScott - Create My Hier w dash - errors
				//_errors = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_FollowingErrors);
				_errors = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_FollowingErrors, false);
				//End TT#474 - JScott - Create My Hier w dash - errors
				if (LevelDataIsValid())
				{
					lstHierarchyLevels.Items[_currentLevelIndex].Text = txtLevelName.Text;
					lstHierarchyLevels.Items[_currentLevelIndex].ImageIndex = _selImageIndex;
					HierarchyListViewItem hlvi = (HierarchyListViewItem) lstHierarchyLevels.Items[_currentLevelIndex];
					if (hlvi.LevelChangeType != eChangeType.add)
					{
						hlvi.LevelChangeType = eChangeType.update;
					}
					hlvi.LevelID = txtLevelName.Text;
					if (hlvi.LevelColor != _selLevelColor)
					{
						hlvi.LevelColor = _selLevelColor;
						hlvi.LevelColorFile = _selColorFile;
						hlvi.LevelColorChanged = true;
					}
					if (hlvi.LevelDisplayOption != _selDisplayOption)
					{
						hlvi.LevelDisplayOption = _selDisplayOption;
						hlvi.LevelDisplayOptionChanged = true;
					}
			
					hlvi.LevelIDFormat = _selIDFormat;
					hlvi.LevelRequiredSize = (int)numRequiredSize.Value;
					hlvi.LevelSizeRangeFrom = (int)numRangeFrom.Value;
					hlvi.LevelSizeRangeTo = (int)numRangeTo.Value;
					if (this.radUnrestricted.Checked == true)
					{
						hlvi.LevelLengthType = eLevelLengthType.unrestricted;
					}
					else 
						if (this.radRequiredSize.Checked == true)
					{
						hlvi.LevelLengthType = eLevelLengthType.required;
					}
					else
						if (this.radRange.Checked == true)
					{
						hlvi.LevelLengthType = eLevelLengthType.range;
					}

					if (_purgeDailyHistoryChanged)
					{
						hlvi.PurgeDailyHistoryTimeframe = ePurgeTimeframe.Weeks;
						if (txtPurgeDailyHistory.Text.Length > 0)
						{
							hlvi.PurgeDailyHistory = Convert.ToInt32(txtPurgeDailyHistory.Text, CultureInfo.CurrentCulture);
						}
						else
						{
							hlvi.PurgeDailyHistory = Include.Undefined;
						}
					}

					if (_purgeWeeklyHistoryChanged)
					{
						hlvi.PurgeWeeklyHistoryTimeframe = ePurgeTimeframe.Weeks;
						if (txtPurgeWeeklyHistory.Text.Length > 0)
						{
							hlvi.PurgeWeeklyHistory = Convert.ToInt32(txtPurgeWeeklyHistory.Text, CultureInfo.CurrentCulture);
						}
						else
						{
							hlvi.PurgeWeeklyHistory = Include.Undefined;
						}
					}

					if (_purgePlansChanged)
					{
						hlvi.PurgePlansTimeframe = ePurgeTimeframe.Weeks;
						if (txtPurgePlans.Text.Length > 0)
						{
							hlvi.PurgePlans = Convert.ToInt32(txtPurgePlans.Text, CultureInfo.CurrentCulture);
						}
						else
						{
							hlvi.PurgePlans = Include.Undefined;
						}
					}

                    //BEGIN TT#400-MD-VStuart-Add Header Purge Criteria by Header Type
                    //if (_purgeHeadersChanged)
                    //{
                    //    hlvi.PurgeHeadersTimeframe = ePurgeTimeframe.Weeks;
                    //    if (txtPurgeHeaders.Text.Length > 0)
                    //    {
                    //        hlvi.PurgeHeaders = Convert.ToInt32(txtPurgeHeaders.Text, CultureInfo.CurrentCulture);
                    //    }
                    //    else
                    //    {
                    //        hlvi.PurgeHeaders = Include.Undefined;
                    //    }
                    //}

                    if (_htASNChanged)
                    {
                        hlvi.HtASNTimeframe = ePurgeTimeframe.Weeks;
                        if (txtHtASN.Text.Length > 0)
                        {
                            hlvi.HtASN = Convert.ToInt32(txtHtASN.Text, CultureInfo.CurrentCulture);
                        }
                        else
                        {
                            hlvi.HtASN = Include.Undefined;
                        }
                    }
                    if (_htDropShipChanged)
                    {
                        hlvi.HtDropShipTimeframe = ePurgeTimeframe.Weeks;
                        if (txtHtDropShip.Text.Length > 0)
                        {
                            hlvi.HtDropShip = Convert.ToInt32(txtHtDropShip.Text, CultureInfo.CurrentCulture);
                        }
                        else
                        {
                            hlvi.HtDropShip = Include.Undefined;
                        }
                    }
                    if (_htDummyChanged)
                    {
                        hlvi.HtDummyTimeframe = ePurgeTimeframe.Weeks;
                        if (txtHtDummy.Text.Length > 0)
                        {
                            hlvi.HtDummy = Convert.ToInt32(txtHtDummy.Text, CultureInfo.CurrentCulture);
                        }
                        else
                        {
                            hlvi.HtDummy = Include.Undefined;
                        }
                    }
                    if (_htPurchaseChanged)
                    {
                        hlvi.HtPurchaseTimeframe = ePurgeTimeframe.Weeks;
                        if (txtHtPurchase.Text.Length > 0)
                        {
                            hlvi.HtPurchase = Convert.ToInt32(txtHtPurchase.Text, CultureInfo.CurrentCulture);
                        }
                        else
                        {
                            hlvi.HtPurchase = Include.Undefined;
                        }
                    }
                    if (_htReceiptChanged)
                    {
                        hlvi.HtReceiptTimeframe = ePurgeTimeframe.Weeks;
                        if (txtHtReceipt.Text.Length > 0)
                        {
                            hlvi.HtReceipt = Convert.ToInt32(txtHtReceipt.Text, CultureInfo.CurrentCulture);
                        }
                        else
                        {
                            hlvi.HtReceipt = Include.Undefined;
                        }
                    }
                    if (_htReserveChanged)
                    {
                        hlvi.HtReserveTimeframe = ePurgeTimeframe.Weeks;
                        if (txtHtReserve.Text.Length > 0)
                        {
                            hlvi.HtReserve = Convert.ToInt32(txtHtReserve.Text, CultureInfo.CurrentCulture);
                        }
                        else
                        {
                            hlvi.HtReserve = Include.Undefined;
                        }
                    }
                    if (_htVSWChanged)
                    {
                        hlvi.HtVSWTimeframe = ePurgeTimeframe.Weeks;
                        if (txtHtVSW.Text.Length > 0)
                        {
                            hlvi.HtVSW = Convert.ToInt32(txtHtVSW.Text, CultureInfo.CurrentCulture);
                        }
                        else
                        {
                            hlvi.HtVSW = Include.Undefined;
                        }
                    }
                    if (_htWorkupTotByChanged)
                    {
                        hlvi.HtWorkupTotByTimeframe = ePurgeTimeframe.Weeks;
                        if (txtHtWorkupTotBy.Text.Length > 0)
                        {
                            hlvi.HtWorkupTotBy = Convert.ToInt32(txtHtWorkupTotBy.Text, CultureInfo.CurrentCulture);
                        }
                        else
                        {
                            hlvi.HtWorkupTotBy = Include.Undefined;
                        }
                    }
                   //END TT#400-MD-VStuart-Add Header Purge Criteria by Header Type

					_purgeDailyHistoryChanged = false;
					_purgeWeeklyHistoryChanged = false;
					_purgePlansChanged = false;
                    //BEGIN TT#400-MD-VStuart-Add Header Purge Criteria by Header Type
                    //_purgeHeadersChanged = false;
                    _htASNChanged = false;
                    _htDropShipChanged = false;
                    _htDummyChanged = false;
                    _htPurchaseChanged = false;
                    _htReceiptChanged = false;
                    _htReserveChanged = false;
                    _htVSWChanged = false;
                    _htWorkupTotByChanged = false;
                    //END TT#400-MD-VStuart-Add Header Purge Criteria by Header Type

					DataRow dr = _OTSTypeDataTable.Rows[cboOTSType.SelectedIndex];
					hlvi.LevelType = (eHierarchyLevelType)(Convert.ToInt32(dr["TEXT_CODE"], CultureInfo.CurrentUICulture));
//Begin Track #3863 - JScott - OTS Forecast Level Defaults
//					if (hlvi.LevelType == eHierarchyLevelType.Planlevel)
//					{
//						if (this.radOTSPlvlRegular.Checked == true)
//						{
//							hlvi.LevelOTSPlanLevelType = eOTSPlanLevelType.Regular;
//						}
//						else
//							if (this.radOTSPlvlTotal.Checked == true)
//						{
//							hlvi.LevelOTSPlanLevelType = eOTSPlanLevelType.Total;
//						}
//						else
//						{
//							hlvi.LevelOTSPlanLevelType = eOTSPlanLevelType.Undefined;
//						}
//					}
//					else
//					{
//						hlvi.LevelOTSPlanLevelType = eOTSPlanLevelType.Undefined;
//Begin Track #4102 - JSmith - Ask to add color and size levels
						if (hlvi.LevelType == eHierarchyLevelType.Style && !_colorDefined)
						{
							if (MessageBox.Show (SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_AddColorLevel),  "Add Color",
								MessageBoxButtons.YesNo, MessageBoxIcon.Question)
								== DialogResult.Yes) 
							{
								addNewLevel("Color", _defaultImageIndex, Include.MIDDefault, eHierarchyLevelType.Color);
								_colorDefined = true;
							}
						}
						else if (hlvi.LevelType == eHierarchyLevelType.Color && !_sizeDefined)
						{
							if (MessageBox.Show (SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_AddSizeLevel),  "Add Size",
								MessageBoxButtons.YesNo, MessageBoxIcon.Question)
								== DialogResult.Yes) 
							{
								addNewLevel("Size", _defaultImageIndex, Include.MIDDefault, eHierarchyLevelType.Size);
								_sizeDefined = true;
							}
						}
//End Track #4102
//					}

					if (this.chkOTSTypeRegular.Checked == true)
					{
						hlvi.LevelOTSPlanLevelType = eOTSPlanLevelType.Regular;
					}
					else if (this.chkOTSTypeTotal.Checked == true)
					{
						hlvi.LevelOTSPlanLevelType = eOTSPlanLevelType.Total;
					}
					else
					{
						hlvi.LevelOTSPlanLevelType = eOTSPlanLevelType.Undefined;
					}
//End Track #3863 - JScott - OTS Forecast Level Defaults

//					if (_colorDefined)
//					{
//						if (hlvi.LevelType != eHierarchyLevelType.Style && _currentLevelType == eHierarchyLevelType.Style) 
//						{
//							// remove color and size
//							foreach(HierarchyListViewItem wrk_hlvi in lstHierarchyLevels.Items)
//							{
//								if (wrk_hlvi.LevelType == eHierarchyLevelType.Color ||
//									wrk_hlvi.LevelType == eHierarchyLevelType.Size)
//								{
//									lstHierarchyLevels.Items.Remove(wrk_hlvi);
//								}
//							}
//							_colorDefined = false;
//						}
//					}


					ChangePending = true;
					btnApply.Enabled = true;
					_levelChangePending = false;
				}
				else
				{
					ErrorFound = true;
					MessageBox.Show (_errors,  this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}
			catch( Exception exception )
			{
				HandleException(exception);
			}
		}

		private bool LevelDataIsValid()
		{
			try
			{
				string error = null;
				bool dataIsValid = true;
				if (this.txtLevelName.Text == "")
				{
					error = AddErrorMessage(eMIDTextCode.msg_NameRequired);
					dataIsValid = false;
					ErrorProvider.SetError (this.txtLevelName,error);
				}
				else
				{
					ErrorProvider.SetError (this.txtLevelName,"");
				}

//Begin Track #3863 - JScott - OTS Forecast Level Defaults
//				ErrorProvider.SetError (this.pnlOTSPlvl,"");
//				if (cboOTSType.SelectedIndex > -1)
//				{
//					DataRow dr = _OTSTypeDataTable.Rows[cboOTSType.SelectedIndex];
//					eHierarchyLevelType hlt = (eHierarchyLevelType)(Convert.ToInt32(dr["TEXT_CODE"], CultureInfo.CurrentUICulture));
//					if (hlt == eHierarchyLevelType.Planlevel &&
//						this.radOTSPlvlRegular.Checked == false &&
//						this.radOTSPlvlTotal.Checked == false)
//					{
//						error = AddErrorMessage(eMIDTextCode.msg_FieldIsRequired);
//						dataIsValid = false;
//						ErrorProvider.SetError (this.pnlOTSPlvl,error);
//					}
//
//				}
//
//End Track #3863 - JScott - OTS Forecast Level Defaults
				if (numRequiredSize.Value > numRequiredSize.Maximum)
				{
					error = AddErrorMessage(eMIDTextCode.msg_ValueExceedsMaximum);
					dataIsValid = false;
					ErrorProvider.SetError (this.numRequiredSize,error);
				}
				else
				{
					ErrorProvider.SetError (this.numRequiredSize,"");
				}

				if (this.numRangeTo.Value > numRangeTo.Maximum)
				{
					error = AddErrorMessage(eMIDTextCode.msg_ValueExceedsMaximum);
					dataIsValid = false;
					ErrorProvider.SetError (this.numRangeTo,error);
				}
				else
				{
					ErrorProvider.SetError (this.numRangeTo,"");
				}

//Begin Track #3704 - JScott - To Length can be less than From Length on Hierarchy Properties
				if (this.numRangeTo.Value < numRangeFrom.Value)
				{
					error = AddErrorMessage(eMIDTextCode.msg_ToMustIsLessThanFrom);
					dataIsValid = false;
					ErrorProvider.SetError (this.numRangeTo,error);
				}
				else
				{
					ErrorProvider.SetError (this.numRangeTo,"");
				}

//End Track #3704 - JScott - To Length can be less than From Length on Hierarchy Properties
				if (!ValidatePurgeCriteria(this.txtPurgeDailyHistory))
				{
					dataIsValid = false;
				}
				if (!ValidatePurgeCriteria(this.txtPurgeWeeklyHistory))
				{
					dataIsValid = false;
				}
				if (!ValidatePurgeCriteria(this.txtPurgePlans))
				{
					dataIsValid = false;
				}
                //BEGIN TT#400-MD-VStuart-Add Header Purge Criteria by Header Type
                //if (!ValidatePurgeCriteria(this.txtPurgeHeaders))
                //{
                //    dataIsValid = false;
                //}
                if (!ValidatePurgeCriteria(this.txtHtASN))
                {
                    dataIsValid = false;
                }
                if (!ValidatePurgeCriteria(this.txtHtDropShip))
                {
                    dataIsValid = false;
                }
                if (!ValidatePurgeCriteria(this.txtHtDummy))
                {
                    dataIsValid = false;
                }
                if (!ValidatePurgeCriteria(this.txtHtPurchase))
                {
                    dataIsValid = false;
                }
                if (!ValidatePurgeCriteria(this.txtHtReceipt))
                {
                    dataIsValid = false;
                }
                if (!ValidatePurgeCriteria(this.txtHtReserve))
                {
                    dataIsValid = false;
                }
                if (!ValidatePurgeCriteria(this.txtHtVSW))
                {
                    dataIsValid = false;
                }
                if (!ValidatePurgeCriteria(this.txtHtWorkupTotBy))
                {
                    dataIsValid = false;
                }
                //END TT#400-MD-VStuart-Add Header Purge Criteria by Header Type

				return dataIsValid;
			}
			catch( Exception exception )
			{
				HandleException(exception);
				return false;
			}
		}

		private bool ValidatePurgeCriteria(TextBox aPurgeField)
		{
			try
			{
				if (aPurgeField.Text.Length > 0)
				{
					try
					{
						int purgeValue = Convert.ToInt32(aPurgeField.Text, CultureInfo.CurrentUICulture);
						if (purgeValue < 0)
						{
							string error = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MustBeNonNegative);
							ErrorProvider.SetError (aPurgeField,error);
							return false;
						}
						else
						{
							ErrorProvider.SetError (aPurgeField,string.Empty);
							return true;
						}
					}
					catch
					{
						string error = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MustBeNonNegative);
						ErrorProvider.SetError (aPurgeField,error);
						return false;
					}
				}
				else
				{
					return true;
				}
			}
			catch( Exception exception )
			{
				HandleException(exception);
			}
			return false;
		}

		private void txtHierarchyPropertyName_TextChanged(object sender, System.EventArgs e)
		{
			try
			{
				if (!_loadingWindow)
				{
					ChangePending = true;
                    // Begin Track #6348 - JSmith - Create Node under My Hier - get err message
                    //btnApply.Enabled = true;
                    if (_node.NodeChangeType != eChangeType.add)
                    {
                        btnApply.Enabled = true;
                    }
                    // End Track #6348
					_nameChanged = true;
				}
			}
			catch( Exception exception )
			{
				HandleException(exception);
			}
		}

		private void radUnrestricted_CheckedChanged(object sender, System.EventArgs e)
		{
			try
			{
//				this.numRequiredSize.Enabled = false;
				this.numRangeFrom.Enabled = false;
				this.numRangeTo.Enabled = false;
				this.numRequiredSize.Enabled = false;
				if (!_loadingLevel && !_loadingWindow)
				{
					_levelChangePending = true;
				}

			}
			catch( Exception exception )
			{
				HandleException(exception);
			}
		}

		private void radRequiredSize_CheckedChanged(object sender, System.EventArgs e)
		{
			try
			{
				this.numRequiredSize.Enabled = true;
				this.numRangeFrom.Enabled = false;
				this.numRangeTo.Enabled = false;
				if (!_loadingLevel && !_loadingWindow)
				{
					_levelChangePending = true;
				}
			}
			catch( Exception exception )
			{
				HandleException(exception);
			}
		}

		private void radRange_CheckedChanged(object sender, System.EventArgs e)
		{
			try
			{
				this.numRequiredSize.Enabled = false;
				this.numRangeFrom.Enabled = true;
				this.numRangeTo.Enabled = true;
				if (!_loadingLevel && !_loadingWindow)
				{
					_levelChangePending = true;
				}
			}
			catch( Exception exception )
			{
				HandleException(exception);
			}
		}

		override protected void BeforeClosing()
		{
			if (_levelChangePending)
			{
				ChangePending = true;
				btnApply.Enabled = true;
			}
		}

		override protected void AfterClosing()
		{
			try
			{
				if (FunctionSecurity.AllowUpdate)
				{
					SAB.HierarchyServerSession.DequeueHierarchy(_node.HomeHierarchyRID);
				}
				// throw event to remove node from displayed list
				if (SaveOnClose && !_changeEventThrown)
				{
					HierPropertyChangeEventArgs ea = new HierPropertyChangeEventArgs(_node);
					ea.FormClosing = true;
					if (OnHierPropertyChangeHandler != null && (_nameChanged || _levelDisplayOptionChanged || _folderColorChanged))
					{
						OnHierPropertyChangeHandler(this, ea);
						_nameChanged = false;
						_levelDisplayOptionChanged = false;
						_folderColorChanged = false;
					}
				}
			}
			catch( Exception exception )
			{
				HandleException(exception);
			}
		}

		private void cboOTSType_SelectionChangeCommitted(object sender, System.EventArgs e)
		{
			try
			{
				DataRow dr = _OTSTypeDataTable.Rows[cboOTSType.SelectedIndex];
				eHierarchyLevelType hlt = (eHierarchyLevelType)(Convert.ToInt32(dr["TEXT_CODE"], CultureInfo.CurrentUICulture));
				this.cboIDFormat.Enabled = false;
				switch (hlt)
				{
//Begin Track #3863 - JScott - OTS Forecast Level Defaults
//					case eHierarchyLevelType.Planlevel:
//						if (!_loadingLevel && !_loadingWindow)
//						{
//							if (_currentLevelType != eHierarchyLevelType.Planlevel)  //  no need to check if same level
//							{
//								bool planLevelFound = false;
//								foreach(HierarchyListViewItem wrk_hlvi in lstHierarchyLevels.Items)
//								{
//									if (wrk_hlvi.LevelType == eHierarchyLevelType.Planlevel)
//									{
//										planLevelFound = true;
//										break;
//									}
//								}
//								if (planLevelFound)  // plan level can only be defined once
//								{
//									MessageBox.Show (SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_LevelOfTypeAlreadyDefined), this.Text,
//										MessageBoxButtons.OK, MessageBoxIcon.Error);
//									cboOTSType.SelectedIndex = _currentOTSIndex;
//								}
//								else
//								{
//									pnlOTSPlvl.Enabled = true;
//									if (!radOTSPlvlRegular.Checked &&
//										!radOTSPlvlTotal.Checked)
//									{
//										radOTSPlvlTotal.Checked = true;
//									}
//								}
//							}
//						}
//						break;
//End Track #3863 - JScott - OTS Forecast Level Defaults
					case eHierarchyLevelType.Color:
						if (!_loadingLevel && !_loadingWindow)
						{
							HierarchyListViewItem hlvi = (HierarchyListViewItem)lstHierarchyLevels.Items[_currentLevelIndex - 1];
//							if (_currentLevelIndex != lstHierarchyLevels.Items.Count - 1)
							if (hlvi.LevelType != eHierarchyLevelType.Style)
							{
								MessageBox.Show (SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ColorAfterStyle), this.Text,
									MessageBoxButtons.OK, MessageBoxIcon.Error);
								cboOTSType.SelectedIndex = _currentOTSIndex;
							}
						}
						break;
					case eHierarchyLevelType.Size:
						if (!_loadingLevel && !_loadingWindow)
						{
							if (_currentLevelIndex + 1 != lstHierarchyLevels.Items.Count)
							{
								MessageBox.Show (SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_SizeMustBeLowestLevel), this.Text,
									MessageBoxButtons.OK, MessageBoxIcon.Error);
								cboOTSType.SelectedIndex = _currentOTSIndex;
							}
						}
						break;
					case eHierarchyLevelType.Style:
						if (!_loadingLevel && !_loadingWindow)
						{
							if (_currentLevelType != eHierarchyLevelType.Style)  //  no need to check if same level
							{
								bool styleLevelFound = false;
								foreach(HierarchyListViewItem wrk_hlvi in lstHierarchyLevels.Items)
								{
									if (wrk_hlvi.LevelType == eHierarchyLevelType.Style)
									{
										styleLevelFound = true;
										break;
									}
								}
								if (styleLevelFound)  // style can only be defined once
								{
									MessageBox.Show (SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_LevelOfTypeAlreadyDefined), this.Text,
										MessageBoxButtons.OK, MessageBoxIcon.Error);
									cboOTSType.SelectedIndex = _currentOTSIndex;
								}
								else
								{
									this.cboIDFormat.Enabled = true;
								}
							}
						}
						break;
//Begin Track #3863 - JScott - OTS Forecast Level Defaults
//					default:
//						pnlOTSPlvl.Enabled = false;
//						break;
//End Track #3863 - JScott - OTS Forecast Level Defaults
				}
				//			if (hlt != eHierarchyLevelType.Planlevel)
				//			{
				//				pnlOTSPlvl.Enabled = false;
				//			}
				//			else
				//			{
				//				pnlOTSPlvl.Enabled = true;
				//			}

				if (!_loadingLevel && !_loadingWindow)
				{
					_levelChangePending = true;
				}
				_currentOTSIndex = cboOTSType.SelectedIndex;
			}
			catch( Exception exception )
			{
				HandleException(exception);
			}
		}

//		private void frmHierarchyProperties_Load(object sender, System.EventArgs e)
//		{
//			FormLoaded = true;
//		}

		private void cboDisplayOption_SelectionChangeCommitted(object sender, System.EventArgs e)
		{
			try
			{
				if (!_loadingLevel && !_loadingWindow)
				{
					_levelChangePending = true;
					_levelDisplayOptionChanged = true;
					_currentDisplayOptionIndex = cboDisplayOption.SelectedIndex;
					DataRow dr = _displayOptionDataTable.Rows[cboDisplayOption.SelectedIndex];
					_selDisplayOption = (eHierarchyDisplayOptions)(Convert.ToInt32(dr["TEXT_CODE"], CultureInfo.CurrentUICulture));
				}
			}
			catch( Exception exception )
			{
				HandleException(exception);
			}
		}

		private void cboIDFormat_SelectionChangeCommitted(object sender, System.EventArgs e)
		{
			try
			{
				if (!_loadingLevel && !_loadingWindow)
				{
					_levelChangePending = true;
					_currentIDFormatIndex = cboIDFormat.SelectedIndex;
					DataRow dr = _IDFormatDataTable.Rows[cboIDFormat.SelectedIndex];
					_selIDFormat = (eHierarchyIDFormat)(Convert.ToInt32(dr["TEXT_CODE"], CultureInfo.CurrentUICulture));
				}
			}
			catch( Exception exception )
			{
				HandleException(exception);
			}
		}

		private void txtLevelName_TextChanged(object sender, System.EventArgs e)
		{
			try
			{
				if (!_loadingLevel && !_loadingWindow)
				{
					_levelChangePending = true;
				}
			}
			catch( Exception exception )
			{
				HandleException(exception);
			}
		}

//Begin Track #3863 - JScott - OTS Forecast Level Defaults
//		private void radOTSPlvlRegular_CheckedChanged(object sender, System.EventArgs e)
//		{
//			try
//			{
//				if (!_loadingLevel && !_loadingWindow)
//				{
//					_levelChangePending = true;
//				}
//			}
//			catch( Exception exception )
//			{
//				HandleException(exception);
//			}
//		}
//
//		private void radOTSPlvlTotal_CheckedChanged(object sender, System.EventArgs e)
//		{
//			try
//			{
//				if (!_loadingLevel && !_loadingWindow)
//				{
//					_levelChangePending = true;
//				}
//			}
//			catch( Exception exception )
//			{
//				HandleException(exception);
//			}
//		}
		private void chkOTSTypeRegular_CheckedChanged(object sender, System.EventArgs e)
		{
			try
			{
				if (!_checkBoxChanging)
				{
					_checkBoxChanging = true;

					if (!_loadingLevel && !_loadingWindow)
					{
						_levelChangePending = true;
					}

					if (chkOTSTypeRegular.Checked)
					{
						chkOTSTypeTotal.Checked = false;
					}

					_checkBoxChanging = false;
				}
			}
			catch( Exception exception )
			{
				HandleException(exception);
			}
		}

		private void chkOTSTypeTotal_CheckedChanged(object sender, System.EventArgs e)
		{
			try
			{
				if (!_checkBoxChanging)
				{
					_checkBoxChanging = true;

					if (!_loadingLevel && !_loadingWindow)
					{
						_levelChangePending = true;
					}

					if (chkOTSTypeTotal.Checked)
					{
						chkOTSTypeRegular.Checked = false;
					}

					_checkBoxChanging = false;
				}
			}
			catch( Exception exception )
			{
				HandleException(exception);
			}
		}
//End Track #3863 - JScott - OTS Forecast Level Defaults

//Begin Track #3704 - JScott - To Length can be less than From Length on Hierarchy Properties
		private void numRequiredSize_ValueChanged(object sender, System.EventArgs e)
		{
			try
			{
				if (!_loadingLevel && !_loadingWindow)
				{
					_levelChangePending = true;
				}
			}
			catch( Exception exception )
			{
				HandleException(exception);
			}
		}

		private void numRangeFrom_ValueChanged(object sender, System.EventArgs e)
		{
			try
			{
				if (!_loadingLevel && !_loadingWindow)
				{
					_levelChangePending = true;
				}
			}
			catch( Exception exception )
			{
				HandleException(exception);
			}
		}

		private void numRangeTo_ValueChanged(object sender, System.EventArgs e)
		{
			try
			{
				if (!_loadingLevel && !_loadingWindow)
				{
					_levelChangePending = true;
				}
			}
			catch( Exception exception )
			{
				HandleException(exception);
			}
		}

//Begin Track #3704 - JScott - To Length can be less than From Length on Hierarchy Properties
		private void txtHierarchyPropertyName_Leave(object sender, System.EventArgs e)
		{
			try
			{
//				if (_newHierarchy || txtHierarchyPropertyName.Text != _hp.HierarchyID)  // make sure ID is unique
//				{
//					HierarchyProfile hp = SAB.HierarchyServerSession.GetHierarchyData(txtHierarchyPropertyName.Text);
//					if (hp.Key != -1)
//					{
//						MessageBox.Show (SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_HierarchyAlreadyExists), this.Text,
//							MessageBoxButtons.OK, MessageBoxIcon.Error);
//					}
//				}
			}
			catch( Exception exception )
			{
				HandleException(exception);
			}
		}

		private void txtPurgeDailyHistory_TextChanged(object sender, System.EventArgs e)
		{
			try
			{
				if (!_loadingLevel && !_loadingWindow)
				{
					this.txtPurgeDailyHistory.ForeColor = this.ForeColor;
					_levelChangePending = true;
					_purgeDailyHistoryChanged = true;
				}
			}
			catch( Exception exception )
			{
				HandleException(exception);
			}
		}

		private void txtPurgeWeeklyHistory_TextChanged(object sender, System.EventArgs e)
		{
			try
			{
				if (!_loadingLevel && !_loadingWindow)
				{
					this.txtPurgeWeeklyHistory.ForeColor = this.ForeColor;
					_levelChangePending = true;
					_purgeWeeklyHistoryChanged = true;
				}
			}
			catch( Exception exception )
			{
				HandleException(exception);
			}
		}

		private void txtPurgePlans_TextChanged(object sender, System.EventArgs e)
		{
			try
			{
				if (!_loadingLevel && !_loadingWindow)
				{
					this.txtPurgePlans.ForeColor = this.ForeColor;
					_levelChangePending = true;
					_purgePlansChanged = true;
				}
			}
			catch( Exception exception )
			{
				HandleException(exception);
			}
		}

        //BEGIN TT#400-MD-VStuart-Add Header Purge Criteria by Header Type
        //private void txtPurgeHeaders_TextChanged(object sender, System.EventArgs e)
        //{
        //    try
        //    {
        //        if (!_loadingLevel && !_loadingWindow)
        //        {
        //            this.txtPurgeHeaders.ForeColor = this.ForeColor;
        //            _levelChangePending = true;
        //            _purgeHeadersChanged = true;
        //        }
        //    }
        //    catch( Exception exception )
        //    {
        //        HandleException(exception);
        //    }
        //}

        private void txtHtASN_TextChanged(object sender, System.EventArgs e)
        {
            try
            {
                if (!_loadingLevel && !_loadingWindow)
                {
                    this.txtHtASN.ForeColor = this.ForeColor;
                    _levelChangePending = true;
                    _htASNChanged = true;
                }
            }
            catch (Exception exception)
            {
                HandleException(exception);
            }
        }

        private void txtHtDropShip_TextChanged(object sender, System.EventArgs e)
        {
            try
            {
                if (!_loadingLevel && !_loadingWindow)
                {
                    this.txtHtDropShip.ForeColor = this.ForeColor;
                    _levelChangePending = true;
                    _htDropShipChanged = true;
                }
            }
            catch (Exception exception)
            {
                HandleException(exception);
            }
        }

        private void txtHtDummy_TextChanged(object sender, System.EventArgs e)
        {
            try
            {
                if (!_loadingLevel && !_loadingWindow)
                {
                    this.txtHtDummy.ForeColor = this.ForeColor;
                    _levelChangePending = true;
                    _htDummyChanged = true;
                }
            }
            catch (Exception exception)
            {
                HandleException(exception);
            }
        }

        private void txtHtPurchase_TextChanged(object sender, System.EventArgs e)
        {
            try
            {
                if (!_loadingLevel && !_loadingWindow)
                {
                    this.txtHtPurchase.ForeColor = this.ForeColor;
                    _levelChangePending = true;
                    _htPurchaseChanged = true;
                }
            }
            catch (Exception exception)
            {
                HandleException(exception);
            }
        }

        private void txtHtReceipt_TextChanged(object sender, System.EventArgs e)
        {
            try
            {
                if (!_loadingLevel && !_loadingWindow)
                {
                    this.txtHtReceipt.ForeColor = this.ForeColor;
                    _levelChangePending = true;
                    _htReceiptChanged = true;
                }
            }
            catch (Exception exception)
            {
                HandleException(exception);
            }
        }

        private void txtHtReserve_TextChanged(object sender, System.EventArgs e)
        {
            try
            {
                if (!_loadingLevel && !_loadingWindow)
                {
                    this.txtHtReserve.ForeColor = this.ForeColor;
                    _levelChangePending = true;
                    _htReserveChanged = true;
                }
            }
            catch (Exception exception)
            {
                HandleException(exception);
            }
        }

        private void txtHtVSW_TextChanged(object sender, System.EventArgs e)
        {
            try
            {
                if (!_loadingLevel && !_loadingWindow)
                {
                    this.txtHtVSW.ForeColor = this.ForeColor;
                    _levelChangePending = true;
                    _htVSWChanged = true;
                }
            }
            catch (Exception exception)
            {
                HandleException(exception);
            }
        }

        private void txtHtWorkupTotBy_TextChanged(object sender, System.EventArgs e)
        {
            try
            {
                if (!_loadingLevel && !_loadingWindow)
                {
                    this.txtHtWorkupTotBy.ForeColor = this.ForeColor;
                    _levelChangePending = true;
                    _htWorkupTotByChanged = true;
                }
            }
            catch (Exception exception)
            {
                HandleException(exception);
            }
        }
        //END TT#400-MD-VStuart-Add Header Purge Criteria by Header Type
		#region IFormBase Members
		override public void ICut()
		{
			
		}

		override public void ICopy()
		{
			
		}

		override public void IPaste()
		{
			
		}	

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

		override public void ISaveAs()
		{
			
		}

		override public void IDelete()
		{
			
		}

		override public void IRefresh()
		{
			
		}
		
		#endregion

    }

	public class HierPropertyChangeEventArgs : EventArgs
	{
        MIDHierarchyNode _node;
		ArrayList _levelChangePendings = new ArrayList();
		bool _isMyHierarchy;
		bool _formClosing;

        public HierPropertyChangeEventArgs(MIDHierarchyNode node)
		{
			_node = node;
			_isMyHierarchy = false;
			_formClosing = false;
		}
		public bool IsMyHierarchy 
		{
			get { return _isMyHierarchy ; }
			set { _isMyHierarchy = value; }
		}
		public bool FormClosing 
		{
			get { return _formClosing ; }
			set { _formClosing = value; }
		}
        public MIDHierarchyNode Node 
		{
			get { return _node ; }
			set { _node = value; }
		}
		public ArrayList LevelChanges 
		{
			get { return _levelChangePendings ; }
			set { _levelChangePendings = value; }
		}
	}

	public class LevelChange
	{
		int							_level;
		bool						_levelColorChanged;
		string						_levelColor;
		bool						_levelDisplayOptionChanged;
		eHierarchyDisplayOptions	_levelDisplayOption;

		public int Level
		{
			get { return _level ; }
			set { _level = value; }
		}
		public bool LevelColorChanged 
		{
			get { return _levelColorChanged ; }
			set { _levelColorChanged = value; }
		}
		public string LevelColor 
		{
			get { return _levelColor ; }
			set { _levelColor = value; }
		}
		public bool LevelDisplayOptionChanged 
		{
			get { return _levelDisplayOptionChanged ; }
			set { _levelDisplayOptionChanged = value; }
		}
		public eHierarchyDisplayOptions LevelDisplayOption 
		{
			get { return _levelDisplayOption ; }
			set { _levelDisplayOption = value; }
		}

		public LevelChange()
		{
			_levelColorChanged = false;
			_levelDisplayOptionChanged = false;
		}		
	}

	public class HierarchyConflictException : Exception
	{
	}
}
