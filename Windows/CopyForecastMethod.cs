// Begin Track #4872 - JSmith - Global/User Attributes
// Renamed cboAttributeSet to cbxAttributeSet so it would not get protected in read only mode.
// End Track #4872
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
	public class frmCopyForecastMethod : MIDRetail.Windows.WorkflowMethodFormBase
	{
		#region Fields

		private Bitmap _picInclude;
		private Bitmap _picExclude;
		private Image _dynamicToPlanImage;
		private Image _dynamicToCurrentImage;
        //private ProfileList _versionProfList;
		private MIDRetail.Business.OTSForecastCopyMethod _OTSForecastCopyMethod = null;
		private HierarchyNodeSecurityProfile _hierNodeSecurity;
		private System.Data.DataSet _dsForecastCopy;
        //private System.Data.DataTable _dtForecastVersions;
		private System.Data.DataTable _dtBasis;
		private System.Data.DataTable _dtGroupLevel;
		private ArrayList _userRIDList;
		//private StoreFilterData _storeFilterDL;
        private FilterData _storeFilterDL;
		private ProfileList _variables;
		private int _nodeRID = Include.NoRID;
		private int _prevAttributeValue;
		private int _prevSetValue;
		private ePlanType _planType;
		private eMethodType _methodType;
		//Begin TT#523 - JScott - Duplicate folder when new folder added
		private eProfileType _profileType;
		//End TT#523 - JScott - Duplicate folder when new folder added
		private bool _attributeReset = false;
		private bool _attributeChanged = false;
		private bool _setReset = false;
		private bool _skipAfterCellUpdate = false;
		private bool _basisNodeRequired = false;
        // Begin MID Track #5699 - KJohnson - Null Error
        private bool _btnSaveClick = false;
        private bool _btnProcessClick = false;
        // End MID Track #5699
		#endregion

		private System.Windows.Forms.TabPage tabMethod;
		private System.Windows.Forms.GroupBox grpOptions;
        private System.Windows.Forms.Label lblVariable;
		private System.Windows.Forms.Label lblFilter;
		private System.Windows.Forms.Label lblTimePeriod;
        private System.Windows.Forms.TabPage tabProperties;
		private System.Windows.Forms.TabControl tabCopyMethod;
		private System.Windows.Forms.GroupBox grpCopyTo;
		private System.Windows.Forms.Label lblVersion;
		private System.Windows.Forms.Label lblMerchandise;
        private System.Windows.Forms.TextBox txtCopyToNode;
		private System.Windows.Forms.Label lblAttribute;
        // Begin Track #4872 - JSmith - Global/User Attributes
        //private MIDRetail.Windows.Controls.MIDComboBoxEnh cboStoreAttribute;
        private MIDAttributeComboBox cboStoreAttribute;
        // End Track #4872
        private System.Windows.Forms.Label lblSet;
		private MIDRetail.Windows.Controls.MIDDateRangeSelector mdsPlanDateRange;
		private System.Windows.Forms.GroupBox grpBasis;
		private Infragistics.Win.UltraWinGrid.UltraGrid grdBasis;
		private System.Windows.Forms.ContextMenu mnuBasisGrid;
		private System.Windows.Forms.ToolTip toolTip1;
		private System.Windows.Forms.ImageList Icons;
        private GroupBox gbxMultiLevel;
		private Label label2;
        private Label label3;
		private CheckBox chkMultiLevel;
		private Button btnOverrideLowerLevels;
        private UltraGrid ugWorkflows;
        private CheckBox chxCopyPreInitValues;
//BEGIN TT#7 - RBeck - Dynamic dropdowns
        private MIDComboBoxEnh cboCopyToVersion;
        private MIDComboBoxEnh cboFromLevel;
        private MIDComboBoxEnh cboToLevel;
        private MIDComboBoxEnh cboOverride;
        private MIDComboBoxEnh cboFilter;
        private MIDComboBoxEnh cbxAttributeSet;
        private MIDComboBoxEnh cboVariable;
//END   TT#7 - RBeck - Dynamic dropdowns

		private System.ComponentModel.IContainer components = null;

		//Begin TT#523 - JScott - Duplicate folder when new folder added
		//public frmCopyForecastMethod(SessionAddressBlock SAB, ExplorerAddressBlock aEAB, eMethodType aMethodType)
		public frmCopyForecastMethod(SessionAddressBlock SAB, ExplorerAddressBlock aEAB, eMethodType aMethodType, eProfileType aProfileType)
		//End TT#523 - JScott - Duplicate folder when new folder added
			: base(SAB, aEAB, eMIDTextCode.frm_CopyChainForecast, eWorkflowMethodType.Method)
		{
			try
			{
				if (aMethodType == eMethodType.CopyStoreForecast)
				{
					FormName = eMIDTextCode.frm_CopyStoreForecast;
				}
				_methodType = aMethodType;
				//Begin TT#523 - JScott - Duplicate folder when new folder added
				_profileType = aProfileType;
				//End TT#523 - JScott - Duplicate folder when new folder added
				_planType = (_methodType == eMethodType.CopyChainForecast) ? ePlanType.Chain : ePlanType.Store;
				InitializeComponent();

                if (_methodType == eMethodType.CopyChainForecast)
				{
					UserSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ForecastMethodsUserCopyChain);
					GlobalSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ForecastMethodsGlobalCopyChain);
					// Begin Issue # 3663 stodd 2/3/06
					// This moves the basis group up where the Options group used to be. It also exspands the size of the
					// Basis group to fill the window.
                    // Begin Track #6347 - JSmith - Copy Store Forecasting seems to be running extremely long
                    //grpOptions.Visible = false;
                    //System.Drawing.Point aPoint = this.grpCopyTo.Location;
                    //System.Drawing.Size aSize = grpCopyTo.Size;
                    //System.Drawing.Point aNewPoint = new Point(aPoint.X, aPoint.Y);
                    //aNewPoint.Y = aPoint.Y + aSize.Height + 10;
                    //this.grpBasis.Location = aNewPoint;
                    //System.Drawing.Size aNewSize = new Size(grpBasis.Size.Width, grpBasis.Size.Height);
                    //aNewSize.Height = aNewSize.Height + grpOptions.Size.Height;
                    //grpBasis.Size = aNewSize;
					// End Issue # 3663
                    grpOptions.Enabled = true;
                    grpOptions.Visible = true;
                    lblAttribute.Visible = false;
                    cboStoreAttribute.Visible = false;
                    lblSet.Visible = false;
                    cbxAttributeSet.Visible = false;
                    lblFilter.Visible = false;
                    cboFilter.Visible = false;
                    lblVariable.Visible = false;
                    cboVariable.Visible = false;

                    chxCopyPreInitValues.Location = lblAttribute.Location;
                    System.Drawing.Size newSize = new Size(grpOptions.Size.Width, grpOptions.Size.Height);
                    newSize.Height = chxCopyPreInitValues.Location.Y + chxCopyPreInitValues.Size.Height + 10;
                    grpOptions.Size = newSize;

                    System.Drawing.Point point = this.grpOptions.Location;
                    System.Drawing.Size size = grpOptions.Size;
                    System.Drawing.Point newPoint = new Point(point.X, point.Y);
                    newPoint.Y = point.Y + size.Height + 10;
                    this.grpBasis.Location = newPoint;
                    newSize = new Size(grpBasis.Size.Width, grpBasis.Size.Height);
                    newSize.Height = newSize.Height + grpOptions.Size.Height;
                    grpBasis.Size = newSize;

                    grdBasis.Height = grpBasis.Height - grdBasis.Location.Y - 10;
                    // End Track #6347
				}
				else
				{
					UserSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ForecastMethodsUserCopyStore);
					GlobalSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ForecastMethodsGlobalCopyStore);
				}
			}
			catch (Exception ex)
			{
				HandleException(ex, "NewOTSForecastCopyMethod");
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
				// Begin MID Track 4858 - JSmith - Security changes
				//				this.radGlobal.CheckedChanged -= new System.EventHandler(this.radGlobal_CheckedChanged);
				//				this.radUser.CheckedChanged -= new System.EventHandler(this.radUser_CheckedChanged);
				//				this.btnProcess.Click -= new System.EventHandler(this.btnProcess_Click);
				//				this.btnSave.Click -= new System.EventHandler(this.btnSave_Click);
				//				this.btnClose.Click -= new System.EventHandler(this.btnClose_Click);
				// End MID Track 4858
				this.tabCopyMethod.SelectedIndexChanged -= new System.EventHandler(this.tabCopyMethod_SelectedIndexChanged);
				//this.txtCopyToNode.KeyPress -= new System.Windows.Forms.KeyPressEventHandler(this.txtCopyToNode_KeyPress);
				//				this.txtName.TextChanged -= new System.EventHandler(this.txtName_TextChanged);
				//				this.txtDesc.TextChanged -= new System.EventHandler(this.txtDesc_TextChanged);
				this.txtCopyToNode.TextChanged -= new System.EventHandler(this.txtCopyToNode_TextChanged);
				this.txtCopyToNode.Validating -= new System.ComponentModel.CancelEventHandler(this.txtCopyToNode_Validating);
                this.txtCopyToNode.Validated -= new System.EventHandler(this.txtCopyToNode_Validated);
				this.txtCopyToNode.DragDrop -= new System.Windows.Forms.DragEventHandler(this.txtCopyToNode_DragDrop);
				this.txtCopyToNode.DragEnter -= new System.Windows.Forms.DragEventHandler(this.txtCopyToNode_DragEnter);
				this.cboCopyToVersion.SelectionChangeCommitted -= new System.EventHandler(this.cboCopyToVersion_SelectionChangeCommitted);
                //Begin TT#316 - MD - DOConnell - Replace all Windows Combobox controls with new enhanced control
                this.cboCopyToVersion.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler(cboCopyToVersion_MIDComboBoxPropertiesChangedEvent);
                //End TT#316 - MD - DOConnell - Replace all Windows Combobox controls with new enhanced control
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
				this.cboStoreAttribute.SelectionChangeCommitted -= new System.EventHandler(this.cboStoreAttribute_SelectionChangeCommitted);
                //Begin TT#316 - MD - DOConnell - Replace all Windows Combobox controls with new enhanced control
                this.cboStoreAttribute.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler(cboStoreAttribute_MIDComboBoxPropertiesChangedEvent);
                //End TT#316 - MD - DOConnell - Replace all Windows Combobox controls with new enhanced control
                this.cboStoreAttribute.DragDrop -= new System.Windows.Forms.DragEventHandler(this.cboStoreAttribute_DragDrop);
                this.cboStoreAttribute.DragEnter -= new System.Windows.Forms.DragEventHandler(this.cboStoreAttribute_DragEnter);
                this.cbxAttributeSet.SelectionChangeCommitted -= new System.EventHandler(this.cbxAttributeSet_SelectionChangeCommitted);
                //Begin TT#316 - MD - DOConnell - Replace all Windows Combobox controls with new enhanced control
                this.cbxAttributeSet.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler(cbxAttributeSet_MIDComboBoxPropertiesChangedEvent);
                //End TT#316 - MD - DOConnell - Replace all Windows Combobox controls with new enhanced control
				this.cboFilter.SelectionChangeCommitted -= new System.EventHandler(this.cboFilter_SelectionChangeCommitted);
                //Begin TT#316 - MD - DOConnell - Replace all Windows Combobox controls with new enhanced control
                this.cboFilter.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler(cboFilter_MIDComboBoxPropertiesChangedEvent);
                //End TT#316 - MD - DOConnell - Replace all Windows Combobox controls with new enhanced control
                this.cboFilter.DragEnter -= new System.Windows.Forms.DragEventHandler(this.cboFilter_DragEnter);
                this.cboFilter.DragDrop -= new System.Windows.Forms.DragEventHandler(this.cboFilter_DragDrop);
				this.mdsPlanDateRange.Click -= new System.EventHandler(this.mdsPlanDateRange_Click);
				this.mdsPlanDateRange.OnSelection -= new MIDRetail.Windows.Controls.MIDDateRangeSelector.SelectionEventHandler(this.mdsPlanDateRange_OnSelection);
                //Begin TT#316 - MD - DOConnell - Replace all Windows Combobox controls with new enhanced control
                this.cboOverride.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler(cboOverride_MIDComboBoxPropertiesChangedEvent);
                this.cboToLevel.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler(cboToLevel_MIDComboBoxPropertiesChangedEvent);
                this.cboFromLevel.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler(cboFromLevel_MIDComboBoxPropertiesChangedEvent);
                //End TT#316 - MD - DOConnell - Replace all Windows Combobox controls with new enhanced control
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
            this.tabCopyMethod = new System.Windows.Forms.TabControl();
            this.tabMethod = new System.Windows.Forms.TabPage();
            this.gbxMultiLevel = new System.Windows.Forms.GroupBox();
            this.cboOverride = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this.cboToLevel = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this.cboFromLevel = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this.btnOverrideLowerLevels = new System.Windows.Forms.Button();
            this.chkMultiLevel = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.grpBasis = new System.Windows.Forms.GroupBox();
            this.grdBasis = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.mnuBasisGrid = new System.Windows.Forms.ContextMenu();
            this.grpOptions = new System.Windows.Forms.GroupBox();
            this.cboVariable = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this.cbxAttributeSet = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this.cboFilter = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this.chxCopyPreInitValues = new System.Windows.Forms.CheckBox();
            this.lblAttribute = new System.Windows.Forms.Label();
            this.cboStoreAttribute = new MIDRetail.Windows.Controls.MIDAttributeComboBox();
            this.lblSet = new System.Windows.Forms.Label();
            this.lblVariable = new System.Windows.Forms.Label();
            this.lblFilter = new System.Windows.Forms.Label();
            this.grpCopyTo = new System.Windows.Forms.GroupBox();
            this.cboCopyToVersion = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this.mdsPlanDateRange = new MIDRetail.Windows.Controls.MIDDateRangeSelector();
            this.lblMerchandise = new System.Windows.Forms.Label();
            this.txtCopyToNode = new System.Windows.Forms.TextBox();
            this.lblVersion = new System.Windows.Forms.Label();
            this.lblTimePeriod = new System.Windows.Forms.Label();
            this.tabProperties = new System.Windows.Forms.TabPage();
            this.ugWorkflows = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.Icons = new System.Windows.Forms.ImageList(this.components);
            this.pnlGlobalUser.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.utmMain)).BeginInit();
            this.tabCopyMethod.SuspendLayout();
            this.tabMethod.SuspendLayout();
            this.gbxMultiLevel.SuspendLayout();
            this.grpBasis.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdBasis)).BeginInit();
            this.grpOptions.SuspendLayout();
            this.grpCopyTo.SuspendLayout();
            this.tabProperties.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ugWorkflows)).BeginInit();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(624, 544);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(536, 544);
            // 
            // btnProcess
            // 
            this.btnProcess.Location = new System.Drawing.Point(16, 544);
            // 
            // utmMain
            // 
            this.utmMain.MenuSettings.ForceSerialization = true;
            this.utmMain.ToolbarSettings.ForceSerialization = true;
            // 
            // tabCopyMethod
            // 
            this.tabCopyMethod.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabCopyMethod.Controls.Add(this.tabMethod);
            this.tabCopyMethod.Controls.Add(this.tabProperties);
            this.tabCopyMethod.Location = new System.Drawing.Point(16, 56);
            this.tabCopyMethod.Name = "tabCopyMethod";
            this.tabCopyMethod.SelectedIndex = 0;
            this.tabCopyMethod.Size = new System.Drawing.Size(688, 474);
            this.tabCopyMethod.TabIndex = 18;
            this.tabCopyMethod.SelectedIndexChanged += new System.EventHandler(this.tabCopyMethod_SelectedIndexChanged);
            // 
            // tabMethod
            // 
            this.tabMethod.Controls.Add(this.gbxMultiLevel);
            this.tabMethod.Controls.Add(this.grpBasis);
            this.tabMethod.Controls.Add(this.grpOptions);
            this.tabMethod.Controls.Add(this.grpCopyTo);
            this.tabMethod.Location = new System.Drawing.Point(4, 22);
            this.tabMethod.Name = "tabMethod";
            this.tabMethod.Size = new System.Drawing.Size(680, 448);
            this.tabMethod.TabIndex = 1;
            this.tabMethod.Text = "Method";
            // 
            // gbxMultiLevel
            // 
            this.gbxMultiLevel.Controls.Add(this.cboOverride);
            this.gbxMultiLevel.Controls.Add(this.cboToLevel);
            this.gbxMultiLevel.Controls.Add(this.cboFromLevel);
            this.gbxMultiLevel.Controls.Add(this.btnOverrideLowerLevels);
            this.gbxMultiLevel.Controls.Add(this.chkMultiLevel);
            this.gbxMultiLevel.Controls.Add(this.label2);
            this.gbxMultiLevel.Controls.Add(this.label3);
            this.gbxMultiLevel.Location = new System.Drawing.Point(346, 11);
            this.gbxMultiLevel.Name = "gbxMultiLevel";
            this.gbxMultiLevel.Size = new System.Drawing.Size(326, 119);
            this.gbxMultiLevel.TabIndex = 20;
            this.gbxMultiLevel.TabStop = false;
            this.gbxMultiLevel.Text = "Multi-Level Copy";
            // 
            // cboOverride
            // 
            this.cboOverride.AutoAdjust = true;
            this.cboOverride.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboOverride.DataSource = null;
            this.cboOverride.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboOverride.DropDownWidth = 168;
            this.cboOverride.Location = new System.Drawing.Point(148, 94);
            this.cboOverride.Margin = new System.Windows.Forms.Padding(0);
            this.cboOverride.Name = "cboOverride";
            this.cboOverride.Size = new System.Drawing.Size(168, 21);
            this.cboOverride.TabIndex = 28;
            this.cboOverride.Tag = null;
            this.cboOverride.SelectionChangeCommitted += new System.EventHandler(this.cboOverride_SelectionChangeCommitted);
            //Begin TT#316 - MD - DOConnell - Replace all Windows Combobox controls with new enhanced control
            this.cboOverride.MIDComboBoxPropertiesChangedEvent += new MIDComboBoxPropertiesChangedEventHandler(cboOverride_MIDComboBoxPropertiesChangedEvent);
            //End TT#316 - MD - DOConnell - Replace all Windows Combobox controls with new enhanced control
            // 
            // cboToLevel
            // 
            this.cboToLevel.AutoAdjust = true;
            this.cboToLevel.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboToLevel.DataSource = null;
            this.cboToLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboToLevel.DropDownWidth = 223;
            this.cboToLevel.Location = new System.Drawing.Point(92, 65);
            this.cboToLevel.Margin = new System.Windows.Forms.Padding(0);
            this.cboToLevel.Name = "cboToLevel";
            this.cboToLevel.Size = new System.Drawing.Size(223, 21);
            this.cboToLevel.TabIndex = 30;
            this.cboToLevel.Tag = null;
            this.cboToLevel.SelectionChangeCommitted += new System.EventHandler(this.cboToLevel_SelectionChangeCommitted);
            //Begin TT#316 - MD - DOConnell - Replace all Windows Combobox controls with new enhanced control
            this.cboToLevel.MIDComboBoxPropertiesChangedEvent += new MIDComboBoxPropertiesChangedEventHandler(cboToLevel_MIDComboBoxPropertiesChangedEvent);
            //End TT#316 - MD - DOConnell - Replace all Windows Combobox controls with new enhanced control
            // 
            // cboFromLevel
            // 
            this.cboFromLevel.AutoAdjust = true;
            this.cboFromLevel.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboFromLevel.DataSource = null;
            this.cboFromLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboFromLevel.DropDownWidth = 223;
            this.cboFromLevel.Location = new System.Drawing.Point(92, 38);
            this.cboFromLevel.Margin = new System.Windows.Forms.Padding(0);
            this.cboFromLevel.Name = "cboFromLevel";
            this.cboFromLevel.Size = new System.Drawing.Size(223, 21);
            this.cboFromLevel.TabIndex = 29;
            this.cboFromLevel.Tag = null;
            this.cboFromLevel.SelectionChangeCommitted += new System.EventHandler(this.cboFromLevel_SelectionChangeCommitted);
            //Begin TT#316 - MD - DOConnell - Replace all Windows Combobox controls with new enhanced control
            this.cboFromLevel.MIDComboBoxPropertiesChangedEvent += new MIDComboBoxPropertiesChangedEventHandler(cboFromLevel_MIDComboBoxPropertiesChangedEvent);
            //End TT#316 - MD - DOConnell - Replace all Windows Combobox controls with new enhanced control
            // 
            // btnOverrideLowerLevels
            // 
            this.btnOverrideLowerLevels.Enabled = false;
            this.btnOverrideLowerLevels.Location = new System.Drawing.Point(14, 92);
            this.btnOverrideLowerLevels.Name = "btnOverrideLowerLevels";
            this.btnOverrideLowerLevels.Size = new System.Drawing.Size(120, 23);
            this.btnOverrideLowerLevels.TabIndex = 21;
            this.btnOverrideLowerLevels.Text = "Override Lower Levels";
            this.btnOverrideLowerLevels.UseVisualStyleBackColor = true;
            this.btnOverrideLowerLevels.Click += new System.EventHandler(this.btnOverrideLowerLevels_Click);
            // 
            // chkMultiLevel
            // 
            this.chkMultiLevel.AutoSize = true;
            this.chkMultiLevel.Enabled = false;
            this.chkMultiLevel.Location = new System.Drawing.Point(17, 19);
            this.chkMultiLevel.Name = "chkMultiLevel";
            this.chkMultiLevel.Size = new System.Drawing.Size(77, 17);
            this.chkMultiLevel.TabIndex = 20;
            this.chkMultiLevel.Text = "Multi-Level";
            this.chkMultiLevel.UseVisualStyleBackColor = true;
            this.chkMultiLevel.CheckedChanged += new System.EventHandler(this.chkMultiLevel_CheckedChanged);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(14, 39);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 16);
            this.label2.TabIndex = 6;
            this.label2.Text = "From Level:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(14, 66);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(72, 16);
            this.label3.TabIndex = 2;
            this.label3.Text = "To Level:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // grpBasis
            // 
            this.grpBasis.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpBasis.Controls.Add(this.grdBasis);
            this.grpBasis.Location = new System.Drawing.Point(8, 259);
            this.grpBasis.Name = "grpBasis";
            this.grpBasis.Size = new System.Drawing.Size(664, 179);
            this.grpBasis.TabIndex = 11;
            this.grpBasis.TabStop = false;
            this.grpBasis.Text = "Basis";
            // 
            // grdBasis
            // 
            this.grdBasis.AllowDrop = true;
            this.grdBasis.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grdBasis.ContextMenu = this.mnuBasisGrid;
            this.grdBasis.Location = new System.Drawing.Point(16, 16);
            this.grdBasis.Name = "grdBasis";
            this.grdBasis.Size = new System.Drawing.Size(632, 151);
            this.grdBasis.TabIndex = 0;
            this.grdBasis.AfterCellUpdate += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdBasis_AfterCellUpdate);
            this.grdBasis.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.grdBasis_InitializeLayout);
            this.grdBasis.InitializeRow += new Infragistics.Win.UltraWinGrid.InitializeRowEventHandler(this.grdBasis_InitializeRow);
            this.grdBasis.AfterRowsDeleted += new System.EventHandler(this.grdBasis_AfterRowsDeleted);
            this.grdBasis.AfterRowInsert += new Infragistics.Win.UltraWinGrid.RowEventHandler(this.grdBasis_AfterRowInsert);
            this.grdBasis.CellChange += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdBasis_CellChange);
            this.grdBasis.ClickCellButton += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdBasis_ClickCellButton);
            this.grdBasis.AfterCellListCloseUp += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdBasis_AfterCellListCloseUp);
            this.grdBasis.MouseEnterElement += new Infragistics.Win.UIElementEventHandler(this.grdBasis_MouseEnterElement);
            this.grdBasis.DragDrop += new System.Windows.Forms.DragEventHandler(this.grdBasis_DragDrop);
            this.grdBasis.DragEnter += new System.Windows.Forms.DragEventHandler(this.grdBasis_DragEnter);
            this.grdBasis.DragOver += new System.Windows.Forms.DragEventHandler(this.grdBasis_DragOver);
            // 
            // grpOptions
            // 
            this.grpOptions.Controls.Add(this.cboVariable);
            this.grpOptions.Controls.Add(this.cbxAttributeSet);
            this.grpOptions.Controls.Add(this.cboFilter);
            this.grpOptions.Controls.Add(this.chxCopyPreInitValues);
            this.grpOptions.Controls.Add(this.lblAttribute);
            this.grpOptions.Controls.Add(this.cboStoreAttribute);
            this.grpOptions.Controls.Add(this.lblSet);
            this.grpOptions.Controls.Add(this.lblVariable);
            this.grpOptions.Controls.Add(this.lblFilter);
            this.grpOptions.Location = new System.Drawing.Point(8, 136);
            this.grpOptions.Name = "grpOptions";
            this.grpOptions.Size = new System.Drawing.Size(664, 117);
            this.grpOptions.TabIndex = 10;
            this.grpOptions.TabStop = false;
            this.grpOptions.Text = "Options";
            // 
            // cboVariable
            // 
            this.cboVariable.AutoAdjust = true;
            this.cboVariable.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboVariable.DataSource = null;
            this.cboVariable.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboVariable.DropDownWidth = 223;
            this.cboVariable.Location = new System.Drawing.Point(424, 56);
            this.cboVariable.Margin = new System.Windows.Forms.Padding(0);
            this.cboVariable.Name = "cboVariable";
            this.cboVariable.Size = new System.Drawing.Size(223, 21);
            this.cboVariable.TabIndex = 8;
            this.cboVariable.Tag = null;
            // 
            // cbxAttributeSet
            // 
            this.cbxAttributeSet.AutoAdjust = true;
            this.cbxAttributeSet.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cbxAttributeSet.DataSource = null;
            this.cbxAttributeSet.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxAttributeSet.DropDownWidth = 223;
            this.cbxAttributeSet.Location = new System.Drawing.Point(424, 24);
            this.cbxAttributeSet.Margin = new System.Windows.Forms.Padding(0);
            this.cbxAttributeSet.Name = "cbxAttributeSet";
            this.cbxAttributeSet.Size = new System.Drawing.Size(223, 21);
            this.cbxAttributeSet.TabIndex = 47;
            this.cbxAttributeSet.Tag = null;
            this.cbxAttributeSet.SelectionChangeCommitted += new System.EventHandler(this.cbxAttributeSet_SelectionChangeCommitted);
            //Begin TT#316 - MD - DOConnell - Replace all Windows Combobox controls with new enhanced control
            this.cbxAttributeSet.MIDComboBoxPropertiesChangedEvent += new MIDComboBoxPropertiesChangedEventHandler(cbxAttributeSet_MIDComboBoxPropertiesChangedEvent);
            //End TT#316 - MD - DOConnell - Replace all Windows Combobox controls with new enhanced control
            // 
            // cboFilter
            // 
            this.cboFilter.AutoAdjust = true;
            this.cboFilter.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboFilter.DataSource = null;
            this.cboFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboFilter.DropDownWidth = 224;
            this.cboFilter.Location = new System.Drawing.Point(97, 56);
            this.cboFilter.Margin = new System.Windows.Forms.Padding(0);
            this.cboFilter.Name = "cboFilter";
            this.cboFilter.Size = new System.Drawing.Size(224, 21);
            this.cboFilter.TabIndex = 17;
            this.cboFilter.Tag = null;
            this.cboFilter.SelectionChangeCommitted += new System.EventHandler(this.cboFilter_SelectionChangeCommitted);
            //Begin TT#316 - MD - DOConnell - Replace all Windows Combobox controls with new enhanced control
            this.cboFilter.MIDComboBoxPropertiesChangedEvent += new MIDComboBoxPropertiesChangedEventHandler(cboFilter_MIDComboBoxPropertiesChangedEvent);
            //End TT#316 - MD - DOConnell - Replace all Windows Combobox controls with new enhanced control
            this.cboFilter.DragDrop += new System.Windows.Forms.DragEventHandler(this.cboFilter_DragDrop);
            this.cboFilter.DragEnter += new System.Windows.Forms.DragEventHandler(this.cboFilter_DragEnter);
            this.cboFilter.DragOver += new System.Windows.Forms.DragEventHandler(this.cboFilter_DragOver);
            // 
            // chxCopyPreInitValues
            // 
            this.chxCopyPreInitValues.AutoSize = true;
            this.chxCopyPreInitValues.Location = new System.Drawing.Point(17, 91);
            this.chxCopyPreInitValues.Name = "chxCopyPreInitValues";
            this.chxCopyPreInitValues.Size = new System.Drawing.Size(121, 17);
            this.chxCopyPreInitValues.TabIndex = 50;
            this.chxCopyPreInitValues.Text = "Copy Pre-Init Values";
            this.chxCopyPreInitValues.UseVisualStyleBackColor = true;
            this.chxCopyPreInitValues.CheckedChanged += new System.EventHandler(this.chxCopyPreInitValues_CheckedChanged);
            // 
            // lblAttribute
            // 
            this.lblAttribute.Location = new System.Drawing.Point(14, 24);
            this.lblAttribute.Name = "lblAttribute";
            this.lblAttribute.Size = new System.Drawing.Size(56, 24);
            this.lblAttribute.TabIndex = 48;
            this.lblAttribute.Text = "Attribute";
            this.lblAttribute.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cboStoreAttribute
            // 
            this.cboStoreAttribute.AllowDrop = true;
            this.cboStoreAttribute.AllowUserAttributes = false;
            this.cboStoreAttribute.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboStoreAttribute.Location = new System.Drawing.Point(97, 24);
            this.cboStoreAttribute.Name = "cboStoreAttribute";
            this.cboStoreAttribute.Size = new System.Drawing.Size(224, 21);
            this.cboStoreAttribute.TabIndex = 49;
            this.cboStoreAttribute.SelectionChangeCommitted += new System.EventHandler(this.cboStoreAttribute_SelectionChangeCommitted);
            //Begin TT#316 - MD - DOConnell - Replace all Windows Combobox controls with new enhanced control
            this.cboStoreAttribute.MIDComboBoxPropertiesChangedEvent += new MIDComboBoxPropertiesChangedEventHandler(cboStoreAttribute_MIDComboBoxPropertiesChangedEvent);
            //End TT#316 - MD - DOConnell - Replace all Windows Combobox controls with new enhanced control
            this.cboStoreAttribute.DragDrop += new System.Windows.Forms.DragEventHandler(this.cboStoreAttribute_DragDrop);
            this.cboStoreAttribute.DragEnter += new System.Windows.Forms.DragEventHandler(this.cboStoreAttribute_DragEnter);
            this.cboStoreAttribute.DragOver += new System.Windows.Forms.DragEventHandler(this.cboStoreAttribute_DragOver);
            // 
            // lblSet
            // 
            this.lblSet.Location = new System.Drawing.Point(344, 24);
            this.lblSet.Name = "lblSet";
            this.lblSet.Size = new System.Drawing.Size(72, 17);
            this.lblSet.TabIndex = 46;
            this.lblSet.Text = "Attribute Set";
            this.lblSet.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblVariable
            // 
            this.lblVariable.Location = new System.Drawing.Point(344, 57);
            this.lblVariable.Name = "lblVariable";
            this.lblVariable.Size = new System.Drawing.Size(56, 17);
            this.lblVariable.TabIndex = 7;
            this.lblVariable.Text = "Variable ";
            this.lblVariable.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblVariable.Visible = false;
            // 
            // lblFilter
            // 
            this.lblFilter.Location = new System.Drawing.Point(14, 56);
            this.lblFilter.Name = "lblFilter";
            this.lblFilter.Size = new System.Drawing.Size(40, 16);
            this.lblFilter.TabIndex = 16;
            this.lblFilter.Text = "Filter";
            this.lblFilter.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // grpCopyTo
            // 
            this.grpCopyTo.Controls.Add(this.cboCopyToVersion);
            this.grpCopyTo.Controls.Add(this.mdsPlanDateRange);
            this.grpCopyTo.Controls.Add(this.lblMerchandise);
            this.grpCopyTo.Controls.Add(this.txtCopyToNode);
            this.grpCopyTo.Controls.Add(this.lblVersion);
            this.grpCopyTo.Controls.Add(this.lblTimePeriod);
            this.grpCopyTo.Location = new System.Drawing.Point(8, 8);
            this.grpCopyTo.Name = "grpCopyTo";
            this.grpCopyTo.Size = new System.Drawing.Size(332, 119);
            this.grpCopyTo.TabIndex = 4;
            this.grpCopyTo.TabStop = false;
            this.grpCopyTo.Text = "Copy To";
            // 
            // cboCopyToVersion
            // 
            this.cboCopyToVersion.AutoAdjust = true;
            this.cboCopyToVersion.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboCopyToVersion.DataSource = null;
            this.cboCopyToVersion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboCopyToVersion.DropDownWidth = 223;
            this.cboCopyToVersion.Location = new System.Drawing.Point(97, 51);
            this.cboCopyToVersion.Margin = new System.Windows.Forms.Padding(0);
            this.cboCopyToVersion.Name = "cboCopyToVersion";
            this.cboCopyToVersion.Size = new System.Drawing.Size(223, 21);
            this.cboCopyToVersion.TabIndex = 7;
            this.cboCopyToVersion.Tag = null;
            this.cboCopyToVersion.SelectionChangeCommitted += new System.EventHandler(this.cboCopyToVersion_SelectionChangeCommitted);
            //Begin TT#316 - MD - DOConnell - Replace all Windows Combobox controls with new enhanced control
            this.cboCopyToVersion.MIDComboBoxPropertiesChangedEvent += new MIDComboBoxPropertiesChangedEventHandler(cboCopyToVersion_MIDComboBoxPropertiesChangedEvent);
            //End TT#316 - MD - DOConnell - Replace all Windows Combobox controls with new enhanced control
            // 
            // mdsPlanDateRange
            // 
            this.mdsPlanDateRange.DateRangeForm = null;
            this.mdsPlanDateRange.DateRangeRID = 0;
            this.mdsPlanDateRange.Enabled = false;
            this.mdsPlanDateRange.Location = new System.Drawing.Point(97, 87);
            this.mdsPlanDateRange.Name = "mdsPlanDateRange";
            this.mdsPlanDateRange.Size = new System.Drawing.Size(224, 24);
            this.mdsPlanDateRange.TabIndex = 19;
            this.mdsPlanDateRange.OnSelection += new MIDRetail.Windows.Controls.MIDDateRangeSelector.SelectionEventHandler(this.mdsPlanDateRange_OnSelection);
            this.mdsPlanDateRange.Click += new System.EventHandler(this.mdsPlanDateRange_Click);
            // 
            // lblMerchandise
            // 
            this.lblMerchandise.Location = new System.Drawing.Point(14, 21);
            this.lblMerchandise.Name = "lblMerchandise";
            this.lblMerchandise.Size = new System.Drawing.Size(72, 16);
            this.lblMerchandise.TabIndex = 18;
            this.lblMerchandise.Text = "Merchandise";
            this.lblMerchandise.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtCopyToNode
            // 
            this.txtCopyToNode.AllowDrop = true;
            this.txtCopyToNode.Location = new System.Drawing.Point(97, 16);
            this.txtCopyToNode.Name = "txtCopyToNode";
            this.txtCopyToNode.Size = new System.Drawing.Size(224, 20);
            this.txtCopyToNode.TabIndex = 1;
            this.txtCopyToNode.TextChanged += new System.EventHandler(this.txtCopyToNode_TextChanged);
            this.txtCopyToNode.DragDrop += new System.Windows.Forms.DragEventHandler(this.txtCopyToNode_DragDrop);
            this.txtCopyToNode.DragEnter += new System.Windows.Forms.DragEventHandler(this.txtCopyToNode_DragEnter);
            this.txtCopyToNode.DragOver += new System.Windows.Forms.DragEventHandler(this.txtCopyToNode_DragOver);
            this.txtCopyToNode.Validating += new System.ComponentModel.CancelEventHandler(this.txtCopyToNode_Validating);
            this.txtCopyToNode.Validated += new System.EventHandler(this.txtCopyToNode_Validated);
            // 
            // lblVersion
            // 
            this.lblVersion.Location = new System.Drawing.Point(14, 56);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(48, 16);
            this.lblVersion.TabIndex = 6;
            this.lblVersion.Text = "Version:";
            this.lblVersion.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblTimePeriod
            // 
            this.lblTimePeriod.Location = new System.Drawing.Point(14, 92);
            this.lblTimePeriod.Name = "lblTimePeriod";
            this.lblTimePeriod.Size = new System.Drawing.Size(72, 16);
            this.lblTimePeriod.TabIndex = 2;
            this.lblTimePeriod.Text = "Time Period:";
            this.lblTimePeriod.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
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
            this.ugWorkflows.Location = new System.Drawing.Point(16, 52);
            this.ugWorkflows.Name = "ugWorkflows";
            this.ugWorkflows.Size = new System.Drawing.Size(648, 344);
            this.ugWorkflows.TabIndex = 1;
            // 
            // Icons
            // 
            this.Icons.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.Icons.ImageSize = new System.Drawing.Size(16, 16);
            this.Icons.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // frmCopyForecastMethod
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(718, 572);
            this.Controls.Add(this.tabCopyMethod);
            this.Name = "frmCopyForecastMethod";
            this.Text = "Copy Store Forecast";
            this.Load += new System.EventHandler(this.frmCopyForecastMethod_Load);
            this.Controls.SetChildIndex(this.pnlGlobalUser, 0);
            this.Controls.SetChildIndex(this.lblName, 0);
            this.Controls.SetChildIndex(this.txtName, 0);
            this.Controls.SetChildIndex(this.txtDesc, 0);
            this.Controls.SetChildIndex(this.tabCopyMethod, 0);
            this.Controls.SetChildIndex(this.btnSave, 0);
            this.Controls.SetChildIndex(this.btnClose, 0);
            this.Controls.SetChildIndex(this.btnProcess, 0);
            this.pnlGlobalUser.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.utmMain)).EndInit();
            this.tabCopyMethod.ResumeLayout(false);
            this.tabMethod.ResumeLayout(false);
            this.gbxMultiLevel.ResumeLayout(false);
            this.gbxMultiLevel.PerformLayout();
            this.grpBasis.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdBasis)).EndInit();
            this.grpOptions.ResumeLayout(false);
            this.grpOptions.PerformLayout();
            this.grpCopyTo.ResumeLayout(false);
            this.grpCopyTo.PerformLayout();
            this.tabProperties.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ugWorkflows)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		/// <summary>
		/// Create a new Copy Forecast Method
		/// </summary>
		override public void NewWorkflowMethod(MIDWorkflowMethodTreeNode aParentNode)
		{
			try
			{
				//Begin TT#523 - JScott - Duplicate folder when new folder added
				//_OTSForecastCopyMethod = new OTSForecastCopyMethod(SAB, Include.NoRID, _methodType);
				_OTSForecastCopyMethod = new OTSForecastCopyMethod(SAB, Include.NoRID, _methodType, _profileType);
				//End TT#523 - JScott - Duplicate folder when new folder added
				ABM = _OTSForecastCopyMethod;
				base.NewWorkflowMethod(aParentNode, eSecurityFunctions.ForecastMethodsUserOTSPlan, eSecurityFunctions.ForecastMethodsGlobalOTSPlan);

				if (_methodType == eMethodType.CopyChainForecast)
					_OTSForecastCopyMethod.SG_RID = Include.AllStoreGroupRID;

				Common_Load();

				if (cboVariable.Enabled)
				{
					cboVariable.SelectedIndex = 0;
				}
			}
			catch (Exception ex)
			{
				HandleException(ex, "NewOTSForecastCopyMethod");
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
				//Begin TT#523 - JScott - Duplicate folder when new folder added
				//_OTSForecastCopyMethod = new OTSForecastCopyMethod(SAB, aMethodRID, _methodType);
				_OTSForecastCopyMethod = new OTSForecastCopyMethod(SAB, aMethodRID, _methodType, _profileType);
				//End TT#523 - JScott - Duplicate folder when new folder added
				base.UpdateWorkflowMethod(aLockStatus, eSecurityFunctions.ForecastMethodsUserOTSPlan, eSecurityFunctions.ForecastMethodsGlobalOTSPlan);

				Common_Load();

			}
			catch (Exception ex)
			{
				HandleException(ex, "InitializeOTSForecastCopyMethod");
				FormLoadError = true;
			}
		}

		/// <summary>
		/// Deletes a Copy Forecast Method.
		/// </summary>
		/// <param name="aMethodRID">The record ID of the method</param>
		override public bool DeleteWorkflowMethod(int aMethodRID)
		{
			try
			{
				//Begin TT#523 - JScott - Duplicate folder when new folder added
				//_OTSForecastCopyMethod = new OTSForecastCopyMethod(SAB, aMethodRID, _methodType);
				_OTSForecastCopyMethod = new OTSForecastCopyMethod(SAB, aMethodRID, _methodType, _profileType);
				//End TT#523 - JScott - Duplicate folder when new folder added
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
		/// Renames an Copy Forecast Method.
		/// </summary>
		/// <param name="aMethodRID">The record ID of the method</param>
		/// <param name="aNewName">The new name of the workflow or method</param>
		override public bool RenameWorkflowMethod(int aMethodRID, string aNewName)
		{
			try
			{
				//Begin TT#523 - JScott - Duplicate folder when new folder added
				//_OTSForecastCopyMethod = new OTSForecastCopyMethod(SAB, aMethodRID, _methodType);
				_OTSForecastCopyMethod = new OTSForecastCopyMethod(SAB, aMethodRID, _methodType, _profileType);
				//End TT#523 - JScott - Duplicate folder when new folder added
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
				this.grpCopyTo.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_CopyTo);
				this.lblMerchandise.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Merchandise) + ":";
				this.lblVersion.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Version) + ":";
				this.lblTimePeriod.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_PlanTimePeriod) + ":";
				this.lblAttribute.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Attribute) + ":";
				this.lblSet.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_AttributeSet) + ":";
				this.lblFilter.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Filter) + ":";
				this.grpOptions.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Options);
				this.grpBasis.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Basis);
				this.tabMethod.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Method);
				this.tabProperties.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Properties);
				this.btnOverrideLowerLevels.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_OverrideLowVersion);
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
				FormLoaded = false;
				Icon = MIDGraphics.GetIcon(MIDGraphics.CopyIcon);
				_storeFilterDL = new FilterData();
				// REMOVED Issue 4858
				//_versionProfList = SAB.ClientServerSession.GetUserForecastVersions();
				_variables = SAB.ApplicationServerSession.DefaultPlanComputations.PlanVariables.VariableProfileList;

				SetText();



				_picInclude = new Bitmap(GraphicsDirectory + "\\include.gif");
				_picExclude = new Bitmap(GraphicsDirectory + "\\exclude.gif");
				_dynamicToPlanImage = Image.FromFile(GraphicsDirectory + "\\" + MIDGraphics.DynamicToPlanImage);
				_dynamicToCurrentImage = Image.FromFile(GraphicsDirectory + "\\" + MIDGraphics.DynamicToCurrentImage);

				//Populate the form.

				// Load Filters

				FunctionSecurityProfile filterUserSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ToolsFiltersStoreUser);
				FunctionSecurityProfile filterGlobalSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ToolsFiltersStoreGlobal);

				_userRIDList = new ArrayList();
				_userRIDList.Add(-1);

				if (filterUserSecurity.AllowUpdate || filterUserSecurity.AllowView)
				{
					_userRIDList.Add(SAB.ClientServerSession.UserRID);
				}

				if (filterGlobalSecurity.AllowUpdate || filterGlobalSecurity.AllowView)
				{
					_userRIDList.Add(Include.GlobalUserRID);  // Issue 3806
				}

				BindVersionComboBoxes();
				StoreAttributes_Populate();
				if (_methodType == eMethodType.CopyStoreForecast)
				{
					BindFilterComboBox();
				}

                LoadOverrideModelComboBox(cboOverride.ComboBox, _OTSForecastCopyMethod.OverrideLowLevelRid, _OTSForecastCopyMethod.CustomOLL_RID);// TT#7 - RBeck - Dynamic dropdowns  

				BindVariableComboBox();
				CreateBasisComboLists();

				LoadMethodData();

				// Begin issue 3716 - stodd 02/15/06
				LoadWorkflows();
				// End issue 3716
				FormLoaded = true;

                //BEGIN TT#110-MD-VStuart - In Use Tool
                tabCopyMethod.Controls.Remove(tabProperties);
                //END TT#110-MD-VStuart - In Use Tool
            }

			catch (Exception exception)
			{
				HandleException(exception);
			}
		}

		private void BindFilterComboBox()
		{
			DataTable dtFilter;

			try
			{
				cboFilter.Items.Clear();
                // Begin TT#2669 - JSmith - Unable to remove a filter from a wokflow
                //cboFilter.Items.Add(new FilterNameCombo(-1, Include.GlobalUserRID, "(None)"));  // Issue 3806
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


		private void cboFilter_SelectionChangeCommitted(object sender, System.EventArgs e)
		{
			if (FormLoaded)
            // Begin Track #6347 - JSmith - Copy Store Forecasting seems to be running extremely long
                //ChangePending = true;
            {
                ChangePending = true;
                SetPreInitOption();
            }
            // End Track #6347

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

        private void cboFilter_DragEnter(object sender, System.Windows.Forms.DragEventArgs e)
        {
        }

        private void cboFilter_DragDrop(object sender, System.Windows.Forms.DragEventArgs e)
        {
            //Begin Track #5858 - Kjohnson - Validating store security only
            try
            {
                bool isSuccessfull = ((MIDComboBoxTag)(((ComboBox)sender).Tag)).ComboBox_DragDrop(sender, e);

                if (isSuccessfull)
                {
                    ChangePending = true;
                    ((MIDComboBoxEnh)((ComboBox)sender).Parent).FirePropertyChangeEvent();
                    // Begin Track #6347 - JSmith - Copy Store Forecasting seems to be running extremely long
                    SetPreInitOption();
                    // Begin Track #6347
                }
            }
            catch (Exception exc)
            {
                HandleException(exc);
            }
            //End Track #5858
        }

		private void BindVersionComboBoxes()
		{
			ProfileList VersionProfList = null;
			try
			{
				//				_dtForecastVersions = new DataTable("Versions");
				//				_dtForecastVersions.Columns.Add("Description", typeof(string));
				//				_dtForecastVersions.Columns.Add("Key", typeof(int));
				//
				//				_dtForecastVersions.Rows.Add(new object[] {string.Empty, Include.NoRID});

				if (_methodType == eMethodType.CopyStoreForecast)
				{
					VersionProfList = base.GetForecastVersionList(eSecuritySelectType.Update, eSecurityTypes.Store, false, _OTSForecastCopyMethod.VersionRID, true);	// Track #5871

					//					foreach (VersionProfile verProf in _versionProfList)
					//					{
					//						// Begin Issue 4562 - stodd - 8.6.07
					//						if (verProf.Key == Include.FV_ActualRID ||
					//							verProf.StoreSecurity.AccessDenied ||
					//							// If Blended AND the forecast version isn't equal to itself.
					//							(verProf.IsBlendedVersion && verProf.ForecastVersionRID != verProf.Key))
					//						{
					//							// Do not include this version
					//						}
					//						else
					//						{
					//							_dtForecastVersions.Rows.Add(new object[] {verProf.Description, verProf.Key});
					//						}
					//						// End Issue 4562
					//					}
				}
				else
				{
					VersionProfList = base.GetForecastVersionList(eSecuritySelectType.Update, eSecurityTypes.Chain, false, _OTSForecastCopyMethod.VersionRID, true);	// Track #5871

					//					foreach (VersionProfile verProf in _versionProfList)
					//					{
					//						// Begin Issue 4562 - stodd - 8.6.07
					//						if (verProf.Key == Include.FV_ActualRID ||
					//							verProf.ChainSecurity.AccessDenied ||
					//							// If Blended AND the forecast version isn't equal to itself.
					//							(verProf.IsBlendedVersion && verProf.ForecastVersionRID != verProf.Key))
					//						{
					//							// Do not include this version
					//						}
					//						else
					//						{
					//							_dtForecastVersions.Rows.Add(new object[] {verProf.Description, verProf.Key});
					//						}
					//						// End Issue 4562
					//					}
				}

				//this.cboCopyToVersion.DataSource = _dtForecastVersions;
				this.cboCopyToVersion.DisplayMember = "Description";
				this.cboCopyToVersion.ValueMember = "Key";
				this.cboCopyToVersion.DataSource = VersionProfList.ArrayList;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		#region Attribute and Attribute Set Combo Boxes
		private void StoreAttributes_Populate()
		{
            try
			{
                // Begin Track #4872 - JSmith - Global/User Attributes
                //ProfileList profileList = SAB.StoreServerSession.GetStoreGroupListViewList();
                BuildAttributeList();

                //this.cboStoreAttribute.ValueMember = "Key";
                //this.cboStoreAttribute.DisplayMember = "Name";
                //this.cboStoreAttribute.DataSource = profileList.ArrayList;
                // End Track #4872

                AdjustTextWidthComboBox_DropDown(cboStoreAttribute);    // TT#7 - RBeck - Dynamic dropdowns
			}
			catch (Exception ex)
			{
				HandleException(ex);
				FormLoadError = true;
			}
		}

		private void cboStoreAttribute_SelectionChangeCommitted(object sender, System.EventArgs e)
		{
			int idx;
			try
			{
				if (FormLoaded)
				{
					if (_attributeReset)
					{
						_attributeReset = false;
						return;
					}
					idx = this.cboStoreAttribute.SelectedIndex;
					if (!DeleteWarningOK(this.lblAttribute.Text))
					{
						_attributeReset = true;
						cboStoreAttribute.SelectedValue = _prevAttributeValue;
						return;
					}
					else
					{
						this.cboStoreAttribute.SelectedIndex = idx;
					}
					_dtGroupLevel.Clear();
					_dtBasis.Clear();
					_attributeChanged = true;
					ChangePending = true;
                    // Begin Track #4872 - JSmith - Global/User Attributes
                    ErrorProvider.SetError(cboStoreAttribute, string.Empty);
                    // End Track #4872
				}
				if (this.cboStoreAttribute.SelectedValue != null)
				{
					PopulateStoreAttributeSet(this.cboStoreAttribute.SelectedValue.ToString());
					_prevAttributeValue = Convert.ToInt32(this.cboStoreAttribute.SelectedValue, CultureInfo.CurrentUICulture);
				}
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

		private bool DeleteWarningOK(string aChangedItem)
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
						aChangedItem);
					errorMessage += Environment.NewLine + MIDText.GetTextOnly((int)eMIDTextCode.msg_ContinueQuestion);
					diagResult = MessageBox.Show(errorMessage, Text,
						MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
					if (diagResult == System.Windows.Forms.DialogResult.No)
						continueProcess = false;
					else
						continueProcess = true;
				}
			}
			catch (Exception ex)
			{
				HandleException(ex);
				continueProcess = false;
			}
			return continueProcess;
		}

		/// <summary>
		/// Populate all values of the Store_Group_Levels (Attribute Sets)
		/// based on the key parameter.
		/// </summary>
		/// <param name="key">SGL_RID</param>
		private void PopulateStoreAttributeSet(string key)
		{
			try
			{
                ProfileList pl = StoreMgmt.StoreGroup_GetLevelListViewList(Convert.ToInt32(key, CultureInfo.CurrentUICulture)); //SAB.StoreServerSession.GetStoreGroupLevelListViewList(Convert.ToInt32(key, CultureInfo.CurrentUICulture));

				this.cbxAttributeSet.ValueMember = "Key";
				this.cbxAttributeSet.DisplayMember = "Name";
				this.cbxAttributeSet.DataSource = pl.ArrayList;

				if (this.cbxAttributeSet.Items.Count > 0)
				{
					this.cbxAttributeSet.SelectedIndex = 0;
					_prevSetValue = Convert.ToInt32(cbxAttributeSet.SelectedValue, CultureInfo.CurrentUICulture);
				}
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}

		private void cbxAttributeSet_SelectionChangeCommitted(object sender, System.EventArgs e)
		{
			if (FormLoaded)
			{
				if (_setReset)
				{
					_setReset = false;
					return;
				}
				grdBasis.UpdateData();
				if (!_attributeChanged &&
                    !ValidBasisGrid())
				{
					string text = MIDText.GetTextOnly(eMIDTextCode.msg_ErrorsFoundReviewCorrect);
					MessageBox.Show(text);
					_setReset = true;
					cbxAttributeSet.SelectedValue = _prevSetValue;
				}
				else
				{
					if (_attributeChanged)
						_attributeChanged = false;
					else
						SaveBasisForSetValues(_prevSetValue);

					LoadBasisForSetValues((int)cbxAttributeSet.SelectedValue);
					_prevSetValue = Convert.ToInt32(cbxAttributeSet.SelectedValue, CultureInfo.CurrentUICulture);
				}
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
				//Begin TT#523 - JScott - Duplicate folder when new folder added
				//_OTSForecastCopyMethod = new OTSForecastCopyMethod(SAB, aMethodRID, _methodType);
				_OTSForecastCopyMethod = new OTSForecastCopyMethod(SAB, aMethodRID, _methodType, _profileType);
				//End TT#523 - JScott - Duplicate folder when new folder added
				ProcessAction(_methodType, true);
			}
			catch (Exception ex)
			{
				HandleException(ex);
				FormLoadError = true;
			}
		}
		// End Issue 4323 - stodd 2.21.07 - fix process from workflow explorer

		private void SaveBasisForSetValues(int aSetValue)
		{
			DataRow setRow = null;
			foreach (DataRow row in _dtGroupLevel.Rows)
			{
				if ((int)row["SGL_RID"] == aSetValue)
				{
					setRow = row;
					break;
				}
			}
			if (setRow == null)
			{
				if (_dtBasis.DefaultView.Count > 0)
				{
					setRow = _dtGroupLevel.NewRow();
					_dtGroupLevel.Rows.Add(setRow);
					setRow["SGL_RID"] = aSetValue;
					_dtGroupLevel.AcceptChanges();
				}
			}
			else if (_dtBasis.DefaultView.Count == 0)
			{
				_dtGroupLevel.Rows.Remove(setRow);
				_dtGroupLevel.AcceptChanges();
			}
		}

		private void LoadBasisForSetValues(int aSetValue)
		{

			_dtBasis.DefaultView.RowFilter = "SGL_RID = " + aSetValue.ToString();
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
		#endregion Attribute and Attribute Set Combo Boxes
		/// <summary>
		/// Creates a list for use on the "Version" column, which is a dropdown.
		/// </summary>

		private void CreateBasisComboLists()
		{
			int i;
			Infragistics.Win.ValueList vl;
			Infragistics.Win.ValueListItem vli;
			ProfileList VersionProfList = null;
			try
			{
				vl = grdBasis.DisplayLayout.ValueLists.Add("Version");
				if (_planType == ePlanType.Store)
				{
					VersionProfList = base.GetForecastVersionList(eSecuritySelectType.View | eSecuritySelectType.Update, eSecurityTypes.Store, false, _OTSForecastCopyMethod.VersionRID);	// Track #5871
				}
				else
				{
					VersionProfList = base.GetForecastVersionList(eSecuritySelectType.View | eSecuritySelectType.Update, eSecurityTypes.Chain, false, _OTSForecastCopyMethod.VersionRID);	// Track #5871
				}

				for (i = 0; i < VersionProfList.Count; i++)
				{
					vli = new Infragistics.Win.ValueListItem();
					vli.DataValue = VersionProfList[i].Key;
					vli.DisplayText = ((VersionProfile)VersionProfList[i]).Description;
					vl.ValueListItems.Add(vli);
				}

                // Begin Track #5934 - JSmith - version description not showing
                if (_OTSForecastCopyMethod.DSForecastCopy != null)
                {
                    DataTable dtBasis = _OTSForecastCopyMethod.DSForecastCopy.Tables["Basis"];
                    foreach (DataRow dr in dtBasis.Rows)
                    {
                        int fvRid = Convert.ToInt32(dr["FV_RID"], CultureInfo.CurrentUICulture);
                        if (fvRid != Include.NoRID &&
                            !VersionProfList.Contains(fvRid))
                        {
                            ForecastVersionProfileBuilder fvpb = new ForecastVersionProfileBuilder();
                            VersionProfile versionProf = fvpb.Build(fvRid);
                            vli = new Infragistics.Win.ValueListItem();
                            vli.DataValue = versionProf.Key;
                            vli.DisplayText = versionProf.Description;
                            vl.ValueListItems.Add(vli);
                        }
                    }
                }
                // End Track #5934
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		private void BindVariableComboBox()
		{
			try
			{
				cboVariable.Items.Clear();
				foreach (VariableProfile vp in _variables)
				{
					if (vp.AllowForecastBalance)
					{
						cboVariable.Items.Add(new VariableCombo(vp.Key, vp.VariableName));
					}
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
				_dsForecastCopy = _OTSForecastCopyMethod.DSForecastCopy;

				mdsPlanDateRange.DateRangeRID = Include.UndefinedCalendarDateRange;
				mdsPlanDateRange.SetImage(null);

				//Begin Track #5858 - KJohnson - Validating store security only
				if (_OTSForecastCopyMethod.PlanType == ePlanType.Chain)
				{
					txtCopyToNode.Tag = new MIDMerchandiseTextBoxTag(SAB, txtCopyToNode, eMIDControlCode.form_CopyChainForecast, eSecurityTypes.Chain, eSecuritySelectType.View | eSecuritySelectType.Update);
				}
				else
				{
					txtCopyToNode.Tag = new MIDMerchandiseTextBoxTag(SAB, txtCopyToNode, eMIDControlCode.form_CopyStoreForecast, eSecurityTypes.Store, eSecuritySelectType.View | eSecuritySelectType.Update);
                    // Begin TT#44 - JSmith - Drag/Drop User Attributes or Filters in to Global Methods does not react consistantly
                    //cboStoreAttribute.Tag = new MIDStoreAttributeComboBoxTag(SAB, cboStoreAttribute);
                    //cboFilter.Tag = new MIDStoreFilterComboBoxTag(SAB, cboFilter, eSecurityTypes.Store, eSecuritySelectType.View | eSecuritySelectType.Update);
                    cboStoreAttribute.Tag = new MIDStoreAttributeComboBoxTag(SAB, cboStoreAttribute, FunctionSecurity, _OTSForecastCopyMethod.GlobalUserType == eGlobalUserType.User);
                    cboFilter.Tag = new MIDStoreFilterComboBoxTag(SAB, cboFilter.ComboBox, eSecurityTypes.Store, eSecuritySelectType.View | eSecuritySelectType.Update, FunctionSecurity, _OTSForecastCopyMethod.GlobalUserType == eGlobalUserType.User);// TT#7 - RBeck - Dynamic dropdowns  
                    // End TT#44
				}
				//End Track #5858

				if (_OTSForecastCopyMethod.Method_Change_Type != eChangeType.add)
				{

					this.txtName.Text = _OTSForecastCopyMethod.Name;
					this.txtDesc.Text = _OTSForecastCopyMethod.Method_Description;

					if (_OTSForecastCopyMethod.User_RID == Include.GetGlobalUserRID())
						radGlobal.Checked = true;
					else
						radUser.Checked = true;

					LoadWorkflows();
				}

				if (_OTSForecastCopyMethod.HierNodeRID > 0)
				{
					//Begin Track #5378 - color and size not qualified
					//					HierarchyNodeProfile hnp = SAB.HierarchyServerSession.GetNodeData(_OTSForecastCopyMethod.HierNodeRID);
					HierarchyNodeProfile hnp = SAB.HierarchyServerSession.GetNodeData(_OTSForecastCopyMethod.HierNodeRID, true, true);
					//End Track #5378

                    //Begin Track #5858 - KJohnson - Validating store security only
                    txtCopyToNode.Text = hnp.Text;
                    ((MIDTag)(txtCopyToNode.Tag)).MIDTagData = hnp;
                    //End Track #5858
					chkMultiLevel.Checked = _OTSForecastCopyMethod.MultiLevelInd;
					if (_OTSForecastCopyMethod.MultiLevelInd)
					{
                        // BEGIN Track #6107 – John Smith - Cannot view departments in multi-level
                        //foreach (MIDListBoxItem item in cboFromLevel.Items)
                        //{
                        //    if (item.Key == _OTSForecastCopyMethod.FromLevel)
                        //    {
                        //        cboFromLevel.SelectedItem = item;
                        //        break;
                        //    }
                        //}
                        
                        cboFromLevel.SelectedIndex = cboFromLevel.Items.IndexOf(new FromLevelCombo(_OTSForecastCopyMethod.FromLevelType, _OTSForecastCopyMethod.FromLevelOffset, _OTSForecastCopyMethod.FromLevelSequence, ""));
                        
                        //foreach (MIDListBoxItem item in cboToLevel.Items)
                        //{
                        //    if (item.Key == _OTSForecastCopyMethod.ToLevel)
                        //    {
                        //        cboToLevel.SelectedItem = item;
                        //        break;
                        //    }
                        //}
                        cboToLevel.SelectedIndex = cboToLevel.Items.IndexOf(new ToLevelCombo(_OTSForecastCopyMethod.ToLevelType, _OTSForecastCopyMethod.ToLevelOffset, _OTSForecastCopyMethod.ToLevelSequence, ""));
                        // END Track #6107
					}
					else
					{
						cboToLevel.Enabled = false;
						cboFromLevel.Enabled = false;
						btnOverrideLowerLevels.Enabled = false;
						cboOverride.Enabled = false;
					}
				}
				else
				{
					chkMultiLevel.Enabled = false;
					cboToLevel.Enabled = false;
					cboFromLevel.Enabled = false;
					btnOverrideLowerLevels.Enabled = false;
					cboOverride.Enabled = false;
				}

				if (_OTSForecastCopyMethod.VersionRID > 0)
					cboCopyToVersion.SelectedValue = _OTSForecastCopyMethod.VersionRID;

				if (_OTSForecastCopyMethod.DateRangeRID > 0 && _OTSForecastCopyMethod.DateRangeRID != Include.UndefinedCalendarDateRange)
				{
					DateRangeProfile drp = SAB.ClientServerSession.Calendar.GetDateRange(_OTSForecastCopyMethod.DateRangeRID);
					LoadDateRangeSelector(mdsPlanDateRange, drp);
				}

                // Begin Track #4872 - JSmith - Global/User Attributes
                //if (_OTSForecastCopyMethod.SG_RID > 0)
                //    cboStoreAttribute.SelectedValue = _OTSForecastCopyMethod.SG_RID;
                if (_OTSForecastCopyMethod.SG_RID > 0)
                {
                    cboStoreAttribute.SelectedValue = _OTSForecastCopyMethod.SG_RID;
                    if (cboStoreAttribute.ContinueReadOnly)
                    {
                        SetMethodReadOnly();
                    }
                }
                // End Track #4872

				if (_OTSForecastCopyMethod.StoreFilterRID > 0 && _OTSForecastCopyMethod.StoreFilterRID != Include.UndefinedStoreFilter)
					cboFilter.SelectedIndex = cboFilter.Items.IndexOf(new FilterNameCombo(_OTSForecastCopyMethod.StoreFilterRID, -1, ""));

				//				cboVariable.SelectedIndex = cboVariable.Items.IndexOf(new VariableCombo(_OTSForecastCopyMethod.VariableNumber, ""));
				//				if (cboVariable.Items.Count > 0 &&
				//					cboVariable.SelectedIndex == -1)
				//				{
				//					cboVariable.SelectedIndex = 0;
				//				}

                // Begin Track #6347 - JSmith - Copy Store Forecasting seems to be running extremely long
                //if (_methodType == eMethodType.CopyChainForecast)
                //    grpOptions.Enabled = false;
                // End Track #6347

				LoadBasis();

                // Begin Track #6347 - JSmith - Copy Store Forecasting seems to be running extremely long
                chxCopyPreInitValues.Checked = _OTSForecastCopyMethod.CopyPreInitValues;
                SetPreInitOption();
                // End Track #6347
			}
			catch (Exception ex)
			{
				HandleException(ex, "LoadMethodData");
			}
		}

        // Begin Track #6347 - JSmith - Copy Store Forecasting seems to be running extremely long
        private void SetPreInitOption()
        {
            try
            {
                if (CheckAllowPreInitValues())
                {
                    if (FunctionSecurity.AllowUpdate)
                    {
                        chxCopyPreInitValues.Enabled = true;
                    }
                }
                else
                {
                    if (chxCopyPreInitValues.Checked)
                    {
                        chxCopyPreInitValues.Checked = false;
                    }
                    chxCopyPreInitValues.Enabled = false;
                }
            }
            catch
            {
                throw;
            }
        }

        private bool CheckAllowPreInitValues()
        {
            int hn_RID, fv_RID, sgl_RID, detail_Seq;
            DateRangeProfile basisdrp, plandrp;
            ProfileList basisWeekRange, planWeekRange;
            Hashtable setKeys = new Hashtable();

            if (_OTSForecastCopyMethod.StoreFilterRID != Include.UndefinedStoreFilter ||
                cboFilter.SelectedIndex > 0)
            {
                return false;
            }

            // Begin TT#79 - JSmith - Copy PreInit option disabled on new method
            //foreach (DataRow row in _dtBasis.Rows)
            //{
            //    sgl_RID = 0;
            //    detail_Seq = 0;
            //    hn_RID = 0;
            //    fv_RID = 0;
            //    basisWeekRange = null;
            //    planWeekRange = null;
            //    if (row["SGL_RID"] != DBNull.Value)
            //    {
            //        sgl_RID = (int)row["SGL_RID"];
            //    }
            //    if (row["DETAIL_SEQ"] != DBNull.Value)
            //    {
            //        detail_Seq = (int)row["DETAIL_SEQ"];
            //    }
            //    if (row["HN_RID"] != DBNull.Value)
            //    {
            //        hn_RID = (int)row["HN_RID"];
            //    }
            //    if (row["FV_RID"] != DBNull.Value)
            //    {
            //        fv_RID = (int)row["FV_RID"];
            //    }
            //    if (row["WEIGHT"] != DBNull.Value &&
            //        Convert.ToDouble(row["WEIGHT"]) != 1)
            //    {
            //        return false;
            //    }
            //    if (setKeys.ContainsKey(sgl_RID))
            //    {
            //        return false;
            //    }
            //    else
            //    {
            //        setKeys.Add(sgl_RID, null);
            //    }

            //    if (row["CDR_RID"] != DBNull.Value &&
            //        Convert.ToInt32(row["CDR_RID"]) > 1)
            //    {
            //        basisdrp = SAB.ClientServerSession.Calendar.GetDateRange(Convert.ToInt32(row["CDR_RID"], CultureInfo.CurrentUICulture));
            //        basisWeekRange = SAB.ApplicationServerSession.Calendar.GetWeekRange(basisdrp, null);
            //    }
            //    if (mdsPlanDateRange.DateRangeRID > 0)
            //    {
            //        plandrp = SAB.ClientServerSession.Calendar.GetDateRange(mdsPlanDateRange.DateRangeRID);
            //        planWeekRange = SAB.ApplicationServerSession.Calendar.GetWeekRange(plandrp, null);
            //    }

            //    if (fv_RID == Include.FV_ActualRID ||
            //        _OTSForecastCopyMethod.VersionRID == Include.FV_ActualRID)
            //    {
            //        return false;
            //    }
            //    else if (_OTSForecastCopyMethod.HierNodeRID == hn_RID &&
            //        fv_RID != _OTSForecastCopyMethod.VersionRID &&
            //        basisWeekRange != null &&
            //        planWeekRange != null &&
            //        ((WeekProfile)basisWeekRange[0]).Key == ((WeekProfile)planWeekRange[0]).Key &&
            //        ((WeekProfile)basisWeekRange[basisWeekRange.Count - 1]).Key == ((WeekProfile)planWeekRange[planWeekRange.Count - 1]).Key)
            //    {
            //        return true;
            //    }
            //}
            foreach (UltraGridRow row in grdBasis.Rows)
            {
                sgl_RID = 0;
                detail_Seq = 0;
                hn_RID = 0;
                fv_RID = 0;
                basisWeekRange = null;
                planWeekRange = null;
                if (row.Cells["SGL_RID"].Value != DBNull.Value)
                {
                    sgl_RID = (int)row.Cells["SGL_RID"].Value;
                }
                if (row.Cells["DETAIL_SEQ"].Value != DBNull.Value)
                {
                    detail_Seq = (int)row.Cells["DETAIL_SEQ"].Value;
                }
                if (row.Cells["HN_RID"].Value != DBNull.Value)
                {
                    hn_RID = (int)row.Cells["HN_RID"].Value;
                }
                if (row.Cells["FV_RID"].Value != DBNull.Value)
                {
                    fv_RID = (int)row.Cells["FV_RID"].Value;
                }
                if (row.Cells["WEIGHT"].Value != DBNull.Value &&
                    Convert.ToDouble(row.Cells["WEIGHT"].Value) != 1)
                {
                    return false;
                }
                if (setKeys.ContainsKey(sgl_RID))
                {
                    return false;
                }
                else
                {
                    setKeys.Add(sgl_RID, null);
                }

                if (row.Cells["CDR_RID"].Value != DBNull.Value &&
                    Convert.ToInt32(row.Cells["CDR_RID"].Value) > 1)
                {
                    basisdrp = SAB.ClientServerSession.Calendar.GetDateRange(Convert.ToInt32(row.Cells["CDR_RID"].Value, CultureInfo.CurrentUICulture));
                    basisWeekRange = SAB.ApplicationServerSession.Calendar.GetWeekRange(basisdrp, null);
                }
				//Begin TT#159 - JScott - Copy chain plan method -> get 7-severe message error if time period is not entered before adding basis (give warning not severe error)
				if (mdsPlanDateRange.DateRangeRID == Include.UndefinedCalendarDateRange)
				{
					return false;
				}
				else
				//End TT#159 - JScott - Copy chain plan method -> get 7-severe message error if time period is not entered before adding basis (give warning not severe error)
                if (mdsPlanDateRange.DateRangeRID > 0)
                {
                    plandrp = SAB.ClientServerSession.Calendar.GetDateRange(mdsPlanDateRange.DateRangeRID);
                    planWeekRange = SAB.ApplicationServerSession.Calendar.GetWeekRange(plandrp, null);
                }

                if (fv_RID == Include.FV_ActualRID ||
                    _OTSForecastCopyMethod.VersionRID == Include.FV_ActualRID)
                {
                    return false;
                }
                else if (_OTSForecastCopyMethod.HierNodeRID == hn_RID &&
                    fv_RID != _OTSForecastCopyMethod.VersionRID &&
                    basisWeekRange != null &&
                    planWeekRange != null &&
                    ((WeekProfile)basisWeekRange[0]).Key == ((WeekProfile)planWeekRange[0]).Key &&
                    ((WeekProfile)basisWeekRange[basisWeekRange.Count - 1]).Key == ((WeekProfile)planWeekRange[planWeekRange.Count - 1]).Key)
                {
                    return true;
                }
            }
            // End TT#79

            return false;
        }
        // End Track #6347

		private void LoadBasis()
		{
			try
			{
				_dtGroupLevel = MIDEnvironment.CreateDataTable("GroupLevel");
				_dtGroupLevel.Columns.Add("SGL_RID", System.Type.GetType("System.Int32"));

				_dtBasis = MIDEnvironment.CreateDataTable("Basis");

				_dtBasis.Columns.Add("SGL_RID", System.Type.GetType("System.Int32")); //this column will be hidden.
				_dtBasis.Columns.Add("DETAIL_SEQ", System.Type.GetType("System.Int32")); //this column will be hidden.
				_dtBasis.Columns.Add("Merchandise", System.Type.GetType("System.String"));
				_dtBasis.Columns.Add("HN_RID", System.Type.GetType("System.Int32")); //this column will be hidden.
				_dtBasis.Columns.Add("FV_RID", System.Type.GetType("System.Int32")); //this column will be hidden.
				_dtBasis.Columns.Add("DateRange", System.Type.GetType("System.String"));
				_dtBasis.Columns.Add("CDR_RID", System.Type.GetType("System.Int32")); //this column will be hidden.
				_dtBasis.Columns.Add("WEIGHT", System.Type.GetType("System.Decimal"));
				_dtBasis.Columns.Add("INCLUDE_EXCLUDE", System.Type.GetType("System.Int32")); //this column will be hidden. We'll use the buttons column for display.
				_dtBasis.Columns.Add("IncludeButton", System.Type.GetType("System.String")); //button column for include/exclude

				if (_dsForecastCopy != null)
				{
					_dtGroupLevel = _dsForecastCopy.Tables["GroupLevel"];
					_dtBasis = _dsForecastCopy.Tables["Basis"];
				}
				else
				{
					_dsForecastCopy = MIDEnvironment.CreateDataSet();
					_dsForecastCopy.Tables.Add(_dtGroupLevel);
					_dsForecastCopy.Tables.Add(_dtBasis);
				}

				_dtBasis.Columns["WEIGHT"].DefaultValue = 1;

				foreach (DataRow dr in _dtBasis.Rows)
				{
					// Begin Issue 4010 stodd
					if (!_OTSForecastCopyMethod.MultiLevelInd)
					{
						if (dr["HN_RID"] != DBNull.Value)
						{
							int hnRid = Convert.ToInt32(dr["HN_RID"], CultureInfo.CurrentUICulture);
							if (hnRid != Include.NoRID)
							{
								//Begin Track #5378 - color and size not qualified
								//							HierarchyNodeProfile hnp = SAB.HierarchyServerSession.GetNodeData(hnRid);
								HierarchyNodeProfile hnp = SAB.HierarchyServerSession.GetNodeData(hnRid, true, true);
								//End Track #5378
								dr["Merchandise"] = hnp.Text;
							}
						}
					}
					else
					{
						dr["HN_RID"] = _OTSForecastCopyMethod.HierNodeRID;
                        dr["Merchandise"] = SAB.HierarchyServerSession.GetNodeData(_OTSForecastCopyMethod.HierNodeRID, true, true).Text;
					}
					// End Issue 4010 stodd
					DateRangeProfile drp = SAB.ClientServerSession.Calendar.GetDateRange(Convert.ToInt32(dr["CDR_RID"], CultureInfo.CurrentUICulture));
					dr["DateRange"] = drp.DisplayDate;
				}

				grdBasis.DataSource = _dtBasis.DefaultView;
				//Begin Track #5518 - JScott - Tried to do a Chain Copy (not using the Multi level function)as the original function and received a Null Reference message.
				//LoadBasisForSetValues((int)cbxAttributeSet.SelectedValue);
				if (_OTSForecastCopyMethod.PlanType == ePlanType.Chain)
				{
					LoadBasisForSetValues(Include.AllStoreGroupRID);
				}
				else
				{
					LoadBasisForSetValues((int)cbxAttributeSet.SelectedValue);
				}
				//End Track #5518 - JScott - Tried to do a Chain Copy (not using the Multi level function)as the original function and received a Null Reference message.
			}
			catch (Exception ex)
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
                // Begin TT#1164 - JSmith - When going in to some methods or Admin features the columns are appearing too wide.
                //ugld.ApplyDefaults((Infragistics.Win.UltraWinGrid.UltraGrid)sender, e, true, 2, true);
                ugld.ApplyDefaults((Infragistics.Win.UltraWinGrid.UltraGrid)sender, e, true, 2, false);
                // End TT#1164
                //End TT#169

				// BEGIN MID Track #3792 - replace obsolete method 
				//e.Layout.AutoFitColumns = true;
				e.Layout.AutoFitStyle = AutoFitStyle.ResizeAllColumns;
				// END MID Track #3792

				//hide the key columns.
				e.Layout.Bands[0].Columns["SGL_RID"].Hidden = true;
				e.Layout.Bands[0].Columns["DETAIL_SEQ"].Hidden = true;
				e.Layout.Bands[0].Columns["HN_RID"].Hidden = true;
				e.Layout.Bands[0].Columns["CDR_RID"].Hidden = true;
				e.Layout.Bands[0].Columns["INCLUDE_EXCLUDE"].Hidden = true;

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

				if (_OTSForecastCopyMethod.MultiLevelInd)
					grdBasis.DisplayLayout.Bands[0].Columns["Merchandise"].CellActivation = Activation.Disabled;

				//Set the header captions.
				grdBasis.DisplayLayout.Bands[0].Columns["Merchandise"].Header.VisiblePosition = 1;
				grdBasis.DisplayLayout.Bands[0].Columns["Merchandise"].Header.Caption = "Merchandise";
				grdBasis.DisplayLayout.Bands[0].Columns["FV_RID"].Header.VisiblePosition = 2;
				grdBasis.DisplayLayout.Bands[0].Columns["FV_RID"].Header.Caption = "Version";
				grdBasis.DisplayLayout.Bands[0].Columns["DateRange"].Header.VisiblePosition = 3;
				grdBasis.DisplayLayout.Bands[0].Columns["DateRange"].Header.Caption = "Date Range";
				grdBasis.DisplayLayout.Bands[0].Columns["WEIGHT"].Header.VisiblePosition = 5;
				grdBasis.DisplayLayout.Bands[0].Columns["WEIGHT"].Header.Caption = "Weight";
				grdBasis.DisplayLayout.Bands[0].Columns["IncludeButton"].Header.VisiblePosition = 6;
				grdBasis.DisplayLayout.Bands[0].Columns["IncludeButton"].Header.Caption = " ";

				//make the "Version" column a drop down list.
				grdBasis.DisplayLayout.Bands[0].Columns["FV_RID"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
				grdBasis.DisplayLayout.Bands[0].Columns["FV_RID"].ValueList = grdBasis.DisplayLayout.ValueLists["Version"];

				grdBasis.DisplayLayout.Bands[0].Columns["IncludeButton"].CellAppearance.ImageHAlign = Infragistics.Win.HAlign.Center;
				grdBasis.DisplayLayout.Bands[0].Columns["IncludeButton"].CellAppearance.ImageVAlign = Infragistics.Win.VAlign.Middle;
				grdBasis.DisplayLayout.Bands[0].Columns["IncludeButton"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Button;
				grdBasis.DisplayLayout.Bands[0].Columns["IncludeButton"].Width = 20;
				grdBasis.DisplayLayout.Bands[0].Columns["IncludeButton"].CellActivation = Activation.NoEdit;


				//Make the "INCLUDE_EXCLUDE" column a checkbox column.
				grdBasis.DisplayLayout.Bands[0].Columns["INCLUDE_EXCLUDE"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox;

				//Set the width of the "DateRange" column so that the DateRangeSelector can fit.
				grdBasis.DisplayLayout.Bands[0].Columns["DateRange"].Width = 160;
				// BEGIN Issue 4640 stodd 09.21.2007
				if (FunctionSecurity.AllowUpdate)
				{
					grdBasis.DisplayLayout.Bands[0].Columns["DateRange"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.EditButton;
					grdBasis.DisplayLayout.Bands[0].Columns["DateRange"].CellActivation = Activation.NoEdit;
				}
				// END Issue 4640 stodd 09.21.2007

				grdBasis.DisplayLayout.Bands[0].AddButtonCaption = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_AddBasisDetails);
				grdBasis.DisplayLayout.Bands[0].AddButtonToolTipText = MIDText.GetTextOnly(eMIDTextCode.tt_Button_AddBasisDetails);
				grdBasis.DisplayLayout.AddNewBox.Hidden = false;
				grdBasis.DisplayLayout.AddNewBox.Style = AddNewBoxStyle.Compact;
			}
			catch (Exception err)
			{
				HandleException(err);
			}
		}

		private void grdBasis_InitializeRow(object sender, Infragistics.Win.UltraWinGrid.InitializeRowEventArgs e)
		{
			try
			{
				if (e.Row.Cells["INCLUDE_EXCLUDE"].Value != DBNull.Value)
				{
					if (Convert.ToInt32(e.Row.Cells["INCLUDE_EXCLUDE"].Value, CultureInfo.CurrentUICulture) == (int)eBasisIncludeExclude.Include)
					{
						e.Row.Cells["IncludeButton"].Appearance.Image = _picInclude;
					}
					else
					{
						e.Row.Cells["INCLUDE_EXCLUDE"].Value = (int)eBasisIncludeExclude.Exclude;
						e.Row.Cells["IncludeButton"].Appearance.Image = _picExclude;
					}
				}
			}
			catch (Exception exc)
			{
				HandleException(exc);
			}
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
			string errorMessage = string.Empty, productID;
			try
			{
				if (_skipAfterCellUpdate)
				{
					_skipAfterCellUpdate = false;
					return;
				}

				switch (e.Cell.Column.Key)
				{
					case "Merchandise":
						if (e.Cell.Value.ToString().Trim().Length == 0)
						{
							e.Cell.Row.Cells["HN_RID"].Value = Include.NoRID;
						}
						else
						{
							productID = e.Cell.Value.ToString().Trim();
							//							HierarchyNodeProfile hnp = SAB.HierarchyServerSession.GetNodeData(productID);
							HierarchyMaintenance hm = new HierarchyMaintenance(SAB);
							EditMsgs em = new EditMsgs();
							HierarchyNodeProfile hnp = hm.NodeLookup(ref em, productID.Split('[')[0].Trim(), false);
							if (hnp.Key == Include.NoRID)
							{
								errorMessage = string.Format(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_InvalidTreeNode),
									productID);
								errorFound = true;
								MessageBox.Show(errorMessage, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
							}
							else
							{
								e.Cell.Row.Cells["HN_RID"].Value = hnp.Key;
								_skipAfterCellUpdate = true;
								e.Cell.Value = hnp.Text;
							}
						}
						break;
					default:
						break;
				}
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
                    // Begin Track #6347 - JSmith - Copy Store Forecasting seems to be running extremely long
                    SetPreInitOption();
                    // End Track #6347
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
					//Begin Track #5518 - JScott - Tried to do a Chain Copy (not using the Multi level function)as the original function and received a Null Reference message.
					//e.Row.Cells["SGL_RID"].Value = Convert.ToInt32(cbxAttributeSet.SelectedValue, CultureInfo.CurrentUICulture);
					if (_OTSForecastCopyMethod.PlanType == ePlanType.Chain)
					{
						e.Row.Cells["SGL_RID"].Value = Include.AllStoreGroupRID;
					}
					else
					{
						e.Row.Cells["SGL_RID"].Value = Convert.ToInt32(cbxAttributeSet.SelectedValue, CultureInfo.CurrentUICulture);
					}
					//End Track #5518 - JScott - Tried to do a Chain Copy (not using the Multi level function)as the original function and received a Null Reference message.
					e.Row.Cells["HN_RID"].Value = Include.NoRID;
					e.Row.Cells["CDR_RID"].Value = Include.UndefinedCalendarDateRange;
					e.Row.Cells["INCLUDE_EXCLUDE"].Value = (int)eBasisIncludeExclude.Include;
					e.Row.Cells["IncludeButton"].Appearance.Image = _picInclude;
					int rowCount = _dtBasis.Rows.Count;
					e.Row.Cells["DETAIL_SEQ"].Value = rowCount++;

					if (_OTSForecastCopyMethod.MultiLevelInd)
						// Begin TT#76 - stodd - invalid node
                        e.Row.Cells["MERCHANDISE"].Value = SAB.HierarchyServerSession.GetNodeData(_OTSForecastCopyMethod.HierNodeRID, true, true).NodeID;
						// End TT#76 - stodd - invalid node
				}

				if (FormLoaded)
				{
					ChangePending = true;
                    // Begin Track #6347 - JSmith - Copy Store Forecasting seems to be running extremely long
                    SetPreInitOption();
                    // End Track #6347
				}
			}
			catch (Exception exc)
			{
				HandleException(exc);
			}
		}

        // Begin Track #6347 - JSmith - Copy Store Forecasting seems to be running extremely long
        private void grdBasis_AfterRowsDeleted(object sender, EventArgs e)
        {
            if (FormLoaded)
            {
                ChangePending = true;
                SetPreInitOption();
            }
        }
        // End Track #6347

		private void grdBasis_DragEnter(object sender, DragEventArgs e)
		{
			Image_DragEnter(sender, e);
		}
		private void grdBasis_DragOver(object sender, System.Windows.Forms.DragEventArgs e)
		{
			try
			{
				Image_DragOver(sender, e);
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

                // Begin TT#95 - JSmith - not allow drop in multi level
                //if (aCell == aRow.Cells["Merchandise"])
                if (aCell == aRow.Cells["Merchandise"] &&
                    !chkMultiLevel.Checked)
                // End TT#95
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
			HierarchyNodeProfile hnp;
            TreeNodeClipboardList cbList;
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
                        //// Create a new instance of the DataObject interface.
                        //IDataObject data = Clipboard.GetDataObject();

                        ////If the data is ClipboardProfile, then retrieve the data
                        //ClipboardProfile cbp;
                        //HierarchyClipboardData MIDTreeNode_cbd;
                        ////object cellValue = null;	
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

									aRow.Cells["HN_RID"].Value = hnp.Key;
									_skipAfterCellUpdate = true;
									aRow.Cells["Merchandise"].Value = hnp.Text;
									aRow.Cells["Merchandise"].Appearance.Image = null;
									aRow.Cells["Merchandise"].Tag = null;
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

						frmCalDtSelector = (CalendarDateSelector)CreateControl(typeof(CalendarDateSelector), new object[] { SAB });

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
						frmCalDtSelector.AllowDynamicToPlan = false;

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

					case "IncludeButton":

						if (Convert.ToInt32(e.Cell.Row.Cells["INCLUDE_EXCLUDE"].Value, CultureInfo.CurrentUICulture) == (int)eBasisIncludeExclude.Include)
						{
							e.Cell.Row.Cells["INCLUDE_EXCLUDE"].Value = (int)eBasisIncludeExclude.Exclude;
							e.Cell.Appearance.Image = _picExclude;
						}
						else
						{
							e.Cell.Row.Cells["INCLUDE_EXCLUDE"].Value = (int)eBasisIncludeExclude.Include;
							e.Cell.Appearance.Image = _picInclude;
						}

						e.Cell.CancelUpdate();
						grdBasis.PerformAction(UltraGridAction.DeactivateCell);

						break;
				}
			}
			catch (Exception exception)
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
			catch (Exception ex)
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
			catch (Exception ex)
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

		private void InsertBasis(bool InsertBeforeRow)
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
				//Begin Track #5518 - JScott - Tried to do a Chain Copy (not using the Multi level function)as the original function and received a Null Reference message.
				//addedRow["SGL_RID"] = Convert.ToInt32(cbxAttributeSet.SelectedValue, CultureInfo.CurrentUICulture);
				if (_OTSForecastCopyMethod.PlanType == ePlanType.Chain)
				{
					addedRow["SGL_RID"] = Include.AllStoreGroupRID;
				}
				else
				{
					addedRow["SGL_RID"] = Convert.ToInt32(cbxAttributeSet.SelectedValue, CultureInfo.CurrentUICulture);
				}
				//End Track #5518 - JScott - Tried to do a Chain Copy (not using the Multi level function)as the original function and received a Null Reference message.
				addedRow["DETAIL_SEQ"] = rowPosition;
				addedRow["HN_RID"] = Include.NoRID;
				addedRow["CDR_RID"] = Include.UndefinedCalendarDateRange;
				addedRow["INCLUDE_EXCLUDE"] = (int)eBasisIncludeExclude.Include;

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
			int storeFilterRID;
			try
			{
                //Begin Track #5858 - KJohnson - Validating store security only
                HierarchyNodeProfile hnp = (HierarchyNodeProfile)((MIDTag)txtCopyToNode.Tag).MIDTagData;
                _OTSForecastCopyMethod.HierNodeRID = hnp.Key;
                //End Track #5858
				_OTSForecastCopyMethod.VersionRID = Convert.ToInt32(cboCopyToVersion.SelectedValue, CultureInfo.CurrentUICulture);
				_OTSForecastCopyMethod.DateRangeRID = mdsPlanDateRange.DateRangeRID;
				_OTSForecastCopyMethod.SG_RID = Convert.ToInt32(cboStoreAttribute.SelectedValue, CultureInfo.CurrentUICulture);
				if (cboFilter.SelectedIndex != -1)
				{
					if (((FilterNameCombo)cboFilter.SelectedItem).FilterRID == -1)
					{
						cboFilter.SelectedIndex = -1;
					}
				}
				if (cboFilter.SelectedIndex != -1)
					storeFilterRID = ((FilterNameCombo)cboFilter.SelectedItem).FilterRID;
				else
					storeFilterRID = Include.UndefinedStoreFilter;

				_OTSForecastCopyMethod.StoreFilterRID = storeFilterRID;
				grdBasis.UpdateData();
				//Begin Track #5518 - JScott - Tried to do a Chain Copy (not using the Multi level function)as the original function and received a Null Reference message.
				//SaveBasisForSetValues(Convert.ToInt32(cbxAttributeSet.SelectedValue, CultureInfo.CurrentUICulture));
				if (_OTSForecastCopyMethod.PlanType == ePlanType.Chain)
				{
					SaveBasisForSetValues(Include.AllStoreGroupRID);
				}
				else
				{
					SaveBasisForSetValues(Convert.ToInt32(cbxAttributeSet.SelectedValue, CultureInfo.CurrentUICulture));
				}
				//End Track #5518 - JScott - Tried to do a Chain Copy (not using the Multi level function)as the original function and received a Null Reference message.
				_OTSForecastCopyMethod.DSForecastCopy = _dsForecastCopy;
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
				if (txtCopyToNode.Text.Trim().Length == 0)
				{
					methodFieldsValid = false;
					ErrorProvider.SetError(txtCopyToNode, SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_FieldIsRequired));
				}
				else
				{
					ErrorProvider.SetError(txtCopyToNode, string.Empty);
				}

                //if ((Convert.ToInt32(cboCopyToVersion.SelectedValue, CultureInfo.CurrentUICulture) == Include.NoRID)) //TT#741 - MD - ComboBox NullReference Exception red icon not displayed - RBeck 
                if ((cboCopyToVersion.SelectedValue == null) || Convert.ToInt32(cboCopyToVersion.SelectedValue, CultureInfo.CurrentUICulture) == Include.NoRID)
				{
					methodFieldsValid = false;
					ErrorProvider.SetError(cboCopyToVersion, SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_FieldIsRequired));
				}
				else
				{
					ErrorProvider.SetError(cboCopyToVersion, string.Empty);
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

                // Begin MID Track #5699 - KJohnson - Null Error
                if (_btnProcessClick || _btnSaveClick)
                {
                    if (_dtBasis.DataSet.Tables[1].Rows.Count == 0)
                    {
                        string msg = MIDText.GetText(eMIDTextCode.msg_pl_BasisRequired);
                        SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Warning, msg, this.ToString());
                        methodFieldsValid = false;
                        ErrorProvider.SetError(grdBasis, SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_pl_BasisRequired));
                    }
                }
                // End MID Track #5699

				if (methodFieldsValid)
				{
					if (!ValidBasisGrid())
					{
						methodFieldsValid = false;
						this.tabCopyMethod.SelectedTab = this.tabMethod;
					}
				}

				// BEGIN Issue 5882
				ePlanSelectType planSelectType = ePlanSelectType.Store;
				if (_planType == ePlanType.Chain)
					planSelectType = ePlanSelectType.Chain;
				// END Issue 5882
				// BEGIN Issue 4858
                //if (!base.ValidatePlanVersionSecurity(cboCopyToVersion, planSelectType))
                //{
                //    methodFieldsValid = false;
                //}
                ////Begin Track #5858 - JSmith - Validating store security only
                ////if (!base.ValidatePlanNodeSecurity(txtCopyToNode))
                //if (!base.ValidateStorePlanNodeSecurity(txtCopyToNode))
                ////End Track #5858
                //{
                //    methodFieldsValid = false;
                //}
                //// END Issue 4858

				if (chkMultiLevel.Checked)
				{
					if (cboFromLevel.SelectedIndex < 0)
					{
						methodFieldsValid = false;
						ErrorProvider.SetError(cboFromLevel, SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_FieldIsRequired));
					}
					else if (cboToLevel.SelectedIndex < 0)
					{
						methodFieldsValid = false;
						ErrorProvider.SetError(cboToLevel, SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_FieldIsRequired));
					}
					else
					{
                        // BEGIN Track #6107 – John Smith - Cannot view departments in multi-level
                        //MIDListBoxItem fromItem = (MIDListBoxItem)cboFromLevel.SelectedItem;
                        //MIDListBoxItem toItem = (MIDListBoxItem)cboToLevel.SelectedItem;

                        //if (fromItem.Key > toItem.Key)
                        //{
                        //    methodFieldsValid = false;
                        //    ErrorProvider.SetError(cboFromLevel, "From level must be equal to or lower then the to level.");
                        //}
                        //else
                        //{
                        //    ErrorProvider.SetError(cboFromLevel, String.Empty);
                        //    ErrorProvider.SetError(cboToLevel, String.Empty);
                        //}
                        FromLevelCombo fromItem = (FromLevelCombo)cboFromLevel.SelectedItem;
                        ToLevelCombo toItem = (ToLevelCombo)cboToLevel.SelectedItem;

                        if (fromItem.FromLevelOffset > toItem.ToLevelOffset)
                        {
                            methodFieldsValid = false;
                            ErrorProvider.SetError(cboFromLevel, "From level must be equal to or lower then the to level.");
                        }
                        else
                        {
                            ErrorProvider.SetError(cboFromLevel, String.Empty);
                            ErrorProvider.SetError(cboToLevel, String.Empty);
                        }
                        // END Track #6107
					}
				}

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
				ABM = _OTSForecastCopyMethod;
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

		#region Validate Basis Grid
		private bool ValidBasisGrid()
		{
			bool errorFound = false;
            string errorMessage = string.Empty;
			ErrorProvider.SetError(grdBasis, string.Empty);
			try
			{
				// BEGIN Issue 5787 stodd - removed following code that was forcing a basis.
				//// BEGIN Issue 5173
				//if (grdBasis.Rows.Count == 0)
				//{
				//    errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_pl_BasisRequired);
				//    grdBasis.Tag = errorMessage;
				//    ErrorProvider.SetError(grdBasis, errorMessage);
				//    errorFound = true;
				//}
				//// END Issue 5173
				// END Issue 5787 

				if (grdBasis.Rows.Count > 0)
				{
					foreach (UltraGridRow gridRow in grdBasis.Rows)
					{
						if (!ValidMerchandise(gridRow.Cells["Merchandise"]))
						{
							errorFound = true;
						}
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
					}
				}

				if (errorFound)
					return false;
				else
					return true;
			}
			catch (Exception ex)
			{
				HandleException(ex);
				return false;
			}
		}
		private bool ValidMerchandise(UltraGridCell gridCell)
		{
			bool errorFound = false;
			string errorMessage = string.Empty;
			//int key, rowseq = -1;
			try
			{
				string productID = gridCell.Value.ToString().Trim();
				if (productID == string.Empty && _basisNodeRequired)
				{
					errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_FieldIsRequired);
					errorFound = true;
				}
				//				else
				//				{
				//					foreach (Infragistics.Win.ValueListItem vli in ugBasisNodeVersion.DisplayLayout.ValueLists["Merchandise"].ValueListItems)
				//					{
				//						if ( Convert.ToInt32(vli.DataValue, CultureInfo.CurrentUICulture) == Convert.ToInt32(gridCell.Value, CultureInfo.CurrentUICulture))
				//						{
				//							rowseq = Convert.ToInt32(vli.DataValue, CultureInfo.CurrentUICulture);
				//							break;
				//						}
				//
				//					}	
				//					if (rowseq != -1)
				//					{
				//						DataRow row = _merchDataTable2.Rows[rowseq];
				//						if (row != null)
				//							return true;
				//					}
				//					key = GetNodeText(ref productID);
				//					if (key == Include.NoRID)
				//					{
				//						errorMessage = string.Format(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_InvalidTreeNode),
				//							productID );	
				//						errorFound = true;
				//					}
				//				}
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
				gridCell.Appearance.Image = null;
				gridCell.Tag = null;
				return true;
			}
		}
		private bool ValidVersion(UltraGridCell gridCell)
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
					// Begin Track #5810 - JSmith - Weighting value must be greater than 1.0 in basis for copy store forecast
//					if (dblValue < 1)
//					{
//						errorMessage = string.Format
//							(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_al_MinimumValueExceeded),dblValue, "1");
//						errorFound = true;
//					}
//					else if (dblValue > 9999)
//					{
//						errorMessage = string.Format
//							(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_al_MaximumValueExceeded),dblValue, "9999");
//						errorFound = true;
//					}
					if (dblValue > 9999)
					{
						errorMessage = string.Format
							(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_al_MaximumValueExceeded),dblValue, "9999");
						errorFound = true;
					}
					// End Track #5810
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
		//				if (_methodType == eMethodType.CopyChainForecast)
		//					ProcessAction(eMethodType.CopyChainForecast);
		//				else
		//					ProcessAction(eMethodType.CopyStoreForecast);
		//				// as part of the  processing we saved the info, so it should be changed to update.
		//				if (!ErrorFound)
		//				{
		//					_OTSForecastCopyMethod.Method_Change_Type = eChangeType.update;
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
		//					_OTSForecastCopyMethod.Method_Change_Type = eChangeType.update;
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
                // Begin MID Track #5699 - KJohnson - Null Error
                _btnProcessClick = true;
                // End MID Track #5699
				_basisNodeRequired = true;
				if (_methodType == eMethodType.CopyChainForecast)
					ProcessAction(eMethodType.CopyChainForecast);
				else
					ProcessAction(eMethodType.CopyStoreForecast);
				// as part of the  processing we saved the info, so it should be changed to update.
				if (!ErrorFound)
				{
					_OTSForecastCopyMethod.Method_Change_Type = eChangeType.update;
					btnSave.Text = "&Update";
				}
                // Begin MID Track #5699 - KJohnson - Null Error
                _btnProcessClick = false;
                // End MID Track #5699
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
                // Begin MID Track #5699 - KJohnson - Null Error
                _btnSaveClick = true;
                // End MID Track #5699
				_basisNodeRequired = false;
				base.btnSave_Click();
                // Begin MID Track #5699 - KJohnson - Null Error
                _btnSaveClick = false;
                // End MID Track #5699
			}
			catch (Exception exception)
			{
				HandleException(exception);
			}
		}
		// End MID Track 4858
		#endregion Button Events

		private void tabCopyMethod_SelectedIndexChanged(object sender, System.EventArgs e)
		{

		}

		#region TextBox Events

		private void txtCopyToNode_TextChanged(object sender, System.EventArgs e)
		{
			if (FormLoaded)
			{
				ChangePending = true;
			}
            ApplySecurity();

            //// BEGIN Issue 4858 stodd 11.2.2007
            //bool canUpdate = ABM.AuthorizedToUpdate(this.SAB.ClientServerSession, this.SAB.ClientServerSession.UserRID);
            //base.ApplyCanUpdate(canUpdate);
            ////Begin Track #5858 - JSmith - Validating store security only
            ////base.ValidatePlanNodeSecurity(txtCopyToNode);
            //base.ValidateStorePlanNodeSecurity(txtCopyToNode);
            ////End Track #5858
            //// END Issue 4858
		}
		
		private void txtCopyToNode_DragEnter(object sender, System.Windows.Forms.DragEventArgs e)
		{
            Image_DragEnter(sender, e);
		}

		private void txtCopyToNode_DragOver(object sender, DragEventArgs e)
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
                return;
            }          
        //TT#695  - End - MD - RBeck - Drag and drop of size merchandise causes error
            //Image_DragOver(sender, e);
		}

		private void txtCopyToNode_DragDrop(object sender, System.Windows.Forms.DragEventArgs e)
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

                    _OTSForecastCopyMethod.HierNodeRID = hnp.Key;

                    ChangePending = true;
                    // Begin Track #6347 - JSmith - Copy Store Forecasting seems to be running extremely long
                    SetPreInitOption();
                    // End Track #6347
                    ApplySecurity();
                    chkMultiLevel.Enabled = true;
                    if (chkMultiLevel.Checked)
                    // BEGIN Track #6107 – John Smith - Cannot view departments in multi-level
                    {
                        //LoadLevelCombos();
                    //Begin TT#3057 - MD - Copy chain multi level settings not holding - RBeck
                        //PopulateFromToLevels(hnp, cboFromLevel.ComboBox, 0);// TT#7 - RBeck - Dynamic dropdowns   
                        //PopulateFromToLevels(hnp, cboToLevel.ComboBox, 0);// TT#7 - RBeck - Dynamic dropdowns    
                        PopulateFromToLevels(hnp, cboFromLevel, 0);    
                        PopulateFromToLevels(hnp, cboToLevel, 0);
                    //End  TT#3057 - MD - Copy chain multi level settings not holding - RBeck
                    }
                    // END Track #6107
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

//                            //Begin Track #5858 - KJohnson - Validating store security only
//                            ((TextBox)sender).Text = hnp.Text;
//                            ((MIDTag)(((TextBox)sender).Tag)).MIDTagData = hnp;
//                            //End Track #5858

//                            _OTSForecastCopyMethod.HierNodeRID = cbp.Key;

//                            ApplySecurity();

//                            //// BEGIN Issue 4858 stodd 11.2.2007
//                            //bool canUpdate = ABM.AuthorizedToUpdate(this.SAB.ClientServerSession, this.SAB.ClientServerSession.UserRID);
//                            //base.ApplyCanUpdate(canUpdate);
//                            ////Begin Track #5858 - JSmith - Validating store security only
//                            ////base.ValidatePlanNodeSecurity(txtCopyToNode);
//                            //if (this._OTSForecastCopyMethod.PlanType == ePlanType.Chain)
//                            //{
//                            //    base.ValidateChainPlanNodeSecurity(txtCopyToNode);
//                            //}
//                            //else
//                            //{
//                            //    base.ValidateStorePlanNodeSecurity(txtCopyToNode);
//                            //}
//                            ////End Track #5858
//                            //// END Issue 4858
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

		//private void txtCopyToNode_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		//{
		//	e.Handled = true;
		//}

		private void txtCopyToNode_Validating(object sender, System.ComponentModel.CancelEventArgs e)
		{
            //string productID; 
            //try
            //{
            //    if (txtCopyToNode.Modified)
            //    {
            //        if (txtCopyToNode.Text.Trim().Length > 0)
            //        {
            //            productID = txtCopyToNode.Text.Trim();
            //            _nodeRID = GetNodeText(ref productID);
            //            if (_nodeRID == Include.NoRID)
            //                MessageBox.Show(productID + " is not valid; please enter or drag and drop a node from the tree");
            //            else 
            //            {
            //                // BEGIN Issue 4858 stodd
            //                _OTSForecastCopyMethod.HierNodeRID = _nodeRID; 
            //                // END Issue 4858

            //                //Begin Track #5858 - KJohnson - Validating store security only
            //                txtCopyToNode.Text = productID;
            //                ((MIDTag)(txtCopyToNode.Tag)).MIDTagData = _nodeRID;
            //                //End Track #5858

            //                //ApplySecurity();
            //                // BEGIN Issue 4858 stodd 11.2.2007
            //                bool canUpdate = ABM.AuthorizedToUpdate(this.SAB.ClientServerSession, this.SAB.ClientServerSession.UserRID);
            //                base.ApplyCanUpdate(canUpdate);
            //                //Begin Track #5858 - JSmith - Validating store security only
            //                //base.ValidatePlanNodeSecurity(txtCopyToNode);
            //                base.ValidateStorePlanNodeSecurity(txtCopyToNode);
            //                //End Track #5858
            //                // END Issue 4858
            //            }
            //        }
            //        else
            //        {
            //            //Begin Track #5858 - KJohnson - Validating store security only
            //            ((MIDTag)(txtCopyToNode.Tag)).MIDTagData = null;
            //            //End Track #5858
            //        }
            //    }
            //}
            //catch
            //{
            //    throw;
            //}
		}

        private void txtCopyToNode_Validated(object sender, EventArgs e)
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

                    // BEGIN Issue 4858 stodd
                    _OTSForecastCopyMethod.HierNodeRID = _nodeRID;
                    // END Issue 4858

                    ChangePending = true;
                    // Begin Track #6347 - JSmith - Copy Store Forecasting seems to be running extremely long
                    SetPreInitOption();
                    // End Track #6347
                    ApplySecurity();

                    chkMultiLevel.Enabled = true;
                    if (chkMultiLevel.Checked)
                    // BEGIN Track #6107 – John Smith - Cannot view departments in multi-level
                    {
                    //Begin TT#3057 - MD - Copy chain multi level settings not holding - RBeck
                        //PopulateFromToLevels(hnp, cboFromLevel.ComboBox, 0);// TT#7 - RBeck - Dynamic dropdowns   
                        //PopulateFromToLevels(hnp, cboToLevel.ComboBox, 0);// TT#7 - RBeck - Dynamic dropdowns    
                        PopulateFromToLevels(hnp, cboFromLevel, 0);
                        PopulateFromToLevels(hnp, cboToLevel, 0);
                    //End  TT#3057 - MD - Copy chain multi level settings not holding - RBeck
                    }
                    // END Track #6107


                    //// BEGIN Issue 4858 stodd 11.2.2007
                    //bool canUpdate = ABM.AuthorizedToUpdate(this.SAB.ClientServerSession, this.SAB.ClientServerSession.UserRID);
                    //base.ApplyCanUpdate(canUpdate);
                    ////Begin Track #5858 - JSmith - Validating store security only
                    ////base.ValidatePlanNodeSecurity(txtCopyToNode);
                    //base.ValidateStorePlanNodeSecurity(txtCopyToNode);
                    ////End Track #5858
                    //// END Issue 4858
                }
                //End Track #5858
            }
            catch (Exception)
            {
                throw;
            }
        }
		
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

		private void cboCopyToVersion_SelectionChangeCommitted(object sender, System.EventArgs e)
		{
			if (FormLoaded)
			{
				ChangePending = true;
				// BEGIN Issue 4858 stodd
				_OTSForecastCopyMethod.VersionRID = (int)this.cboCopyToVersion.SelectedValue;
				//bool canUpdate = ABM.AuthorizedToUpdate(this.SAB.ClientServerSession, this.SAB.ClientServerSession.UserRID);
				//base.ApplyCanUpdate(canUpdate);
				// END Issue 4858 stodd

                // Begin Track #6347 - JSmith - Copy Store Forecasting seems to be running extremely long
                SetPreInitOption();
                // End Track #6347
			}
			// BEGIN Issue 4858 jsmith - set version to default value
			else if (_OTSForecastCopyMethod.Method_Change_Type == eChangeType.add)
			{
				_OTSForecastCopyMethod.VersionRID = (int)this.cboCopyToVersion.SelectedValue;
			}
			// END Issue 4858 
            ApplySecurity();

            //// BEGIN Issue 5882
            //ePlanSelectType planSelectType = ePlanSelectType.Store;
            //if (_planType == ePlanType.Chain)
            //    planSelectType = ePlanSelectType.Chain;
            //// END Issue 5882
            //base.ValidatePlanVersionSecurity(cboCopyToVersion, planSelectType);
		}

        //Begin TT#316 - MD - DOConnell - Replace all Windows Combobox controls with new enhanced control
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

        void cbxAttributeSet_MIDComboBoxPropertiesChangedEvent(object source, MIDComboBoxPropertiesChangedEventArgs args)
        {
            this.cbxAttributeSet_SelectionChangeCommitted(source, new EventArgs());
        }

        void cboFilter_MIDComboBoxPropertiesChangedEvent(object source, MIDComboBoxPropertiesChangedEventArgs args)
        {
            this.cboFilter_SelectionChangeCommitted(source, new EventArgs());
        }

        void cboStoreAttribute_MIDComboBoxPropertiesChangedEvent(object source, MIDComboBoxPropertiesChangedEventArgs args)
        {
            this.cboStoreAttribute_SelectionChangeCommitted(source, new EventArgs());
        }

        void cboCopyToVersion_MIDComboBoxPropertiesChangedEvent(object source, MIDComboBoxPropertiesChangedEventArgs args)
        {
            this.cboCopyToVersion_SelectionChangeCommitted(source, new EventArgs());
        }
        //End TT#316 - MD - DOConnell - Replace all Windows Combobox controls with new enhanced control

        // Begin Track #4872 - JSmith - Global/User Attributes
        override protected void BuildAttributeList()
        {
            ProfileList profileList;
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
                profileList = GetStoreGroupList(_OTSForecastCopyMethod.Method_Change_Type, _OTSForecastCopyMethod.GlobalUserType, false);
                cboStoreAttribute.Initialize(SAB, FunctionSecurity, profileList.ArrayList, _OTSForecastCopyMethod.GlobalUserType == eGlobalUserType.User);
                if (currValue != Include.NoRID)
                {
                    cboStoreAttribute.SelectedValue = currValue;
                }
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
			if (FormLoaded)
			{
				//Begin Track #5858 - JSmith - Validating store security only
				string errorMessage = string.Empty;
				string item = string.Empty;
				//End Track #5858

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

				////Begin Track #5858 - JSmith - Validating store security only
				////if (_nodeRID != Include.NoRID)
				////{
				////    _hierNodeSecurity = SAB.ClientServerSession.GetMyUserNodeSecurityAssignment(_nodeRID, (int)eSecurityTypes.Store);
				////    if (!_hierNodeSecurity.AllowUpdate)
				////    {
				////        btnProcess.Enabled = false;
				////    }
				////}
				////ErrorProvider.SetError(txtCopyToNode, string.Empty);
				//if (_OTSForecastCopyMethod.HierNodeRID != Include.NoRID)
				//{
				//    if (_OTSForecastCopyMethod.PlanType == ePlanType.Chain)
				//    {
				//        _hierNodeSecurity = SAB.ClientServerSession.GetMyUserNodeSecurityAssignment(_OTSForecastCopyMethod.HierNodeRID, (int)eSecurityTypes.Chain);
				//    }
				//    else
				//    {
				//        _hierNodeSecurity = SAB.ClientServerSession.GetMyUserNodeSecurityAssignment(_OTSForecastCopyMethod.HierNodeRID, (int)eSecurityTypes.Store);
				//    }
				//    if (!_hierNodeSecurity.AllowUpdate)
				//    {
				//        btnProcess.Enabled = false;
				//        btnSave.Enabled = false;
				//        securityOk = false;
				//        errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_NotAuthorizedToPlan);
				//        item = MIDText.GetTextOnly(eMIDTextCode.lbl_Merchandise);
				//        errorMessage = errorMessage + item + ".";
				//        ErrorProvider.SetError(txtCopyToNode, errorMessage);
				//    }
				//}
				////End Track #5858

				if (_OTSForecastCopyMethod.PlanType == ePlanType.Chain)
				{
                    securityOk = base.ValidateChainPlanVersionSecurity(cboCopyToVersion.ComboBox, true);// TT#7 - RBeck - Dynamic dropdowns  
					//Begin Track #5858 stodd
					if (securityOk)
					{
						//if (txtCopyToNode.Tag != null)
							securityOk = (((MIDControlTag)(txtCopyToNode.Tag)).IsAuthorized(eSecurityTypes.Chain, eSecuritySelectType.Update));
					}
					// End Track #5858 stodd
				}
				else
				{
                    securityOk = base.ValidateStorePlanVersionSecurity(cboCopyToVersion.ComboBox, true);// TT#7 - RBeck - Dynamic dropdowns  
					//Begin Track #5858 stodd
					if (securityOk)
					{
						//if (txtCopyToNode.Tag != null)
							securityOk = (((MIDControlTag)(txtCopyToNode.Tag)).IsAuthorized(eSecurityTypes.Store, eSecuritySelectType.Update));
					}
					//End Track #5858 stodd
				}

				bool canUpdate = ABM.AuthorizedToUpdate(this.SAB.ClientServerSession, this.SAB.ClientServerSession.UserRID);
				base.ApplyCanUpdate(canUpdate);
				if (!canUpdate)
				{
					if (FunctionSecurity.IsReadOnly
						|| txtCopyToNode.Text.Trim().Length == 0
						|| (cboCopyToVersion.SelectedValue == null
						|| Convert.ToInt32(cboCopyToVersion.SelectedValue, CultureInfo.CurrentUICulture) == Include.NoRID))
					{
						// Skip
					}
					else
					{
						securityOk = false;
					}
				}

			}
			return securityOk;	// track 5871 stodd
		}

		#endregion
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
                    // Begin Track #6347 - JSmith - Copy Store Forecasting seems to be running extremely long
                    SetPreInitOption();
                    // End Track #6347
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
                GetOTSPLANWorkflows(_OTSForecastCopyMethod.Key, ugWorkflows);
            }
            catch (Exception ex)
            {
                HandleException(ex, "LoadWorkflows");
            }
		}
		#endregion

		private void chkMultiLevel_CheckedChanged(object sender, EventArgs e)
        {
            cboToLevel.Enabled = chkMultiLevel.Checked;
            cboFromLevel.Enabled = chkMultiLevel.Checked;
			btnOverrideLowerLevels.Enabled = chkMultiLevel.Checked;
			cboOverride.Enabled = chkMultiLevel.Checked;

            if (!chkMultiLevel.Checked)
            {
                cboToLevel.SelectedIndex = -1;
                cboFromLevel.SelectedIndex = -1;
            }

            _OTSForecastCopyMethod.MultiLevelInd = chkMultiLevel.Checked;

            if (chkMultiLevel.Checked && cboFromLevel.Items.Count == 0)
            {
                // BEGIN Track #6107 – John Smith - Cannot view departments in multi-level
                //LoadLevelCombos();
                HierarchyNodeProfile hnp = SAB.HierarchyServerSession.GetNodeData(_OTSForecastCopyMethod.HierNodeRID);
            //Begin TT#3057 - MD - Copy chain multi level settings not holding - RBeck
                //PopulateFromToLevels(hnp, cboFromLevel.ComboBox, 0);// TT#7 - RBeck - Dynamic dropdowns   
                //PopulateFromToLevels(hnp, cboToLevel.ComboBox, 0);// TT#7 - RBeck - Dynamic dropdowns    
                PopulateFromToLevels(hnp, cboFromLevel, 0);
                PopulateFromToLevels(hnp, cboToLevel, 0);
            //End  TT#3057 - MD - Copy chain multi level settings not holding - RBeck
                // END Track #6107
            }

            if (FormLoaded)
            {
                if (chkMultiLevel.Checked)
                {
                    grdBasis.DisplayLayout.Bands[0].Columns["Merchandise"].CellActivation = Activation.Disabled;

                    for (int row = 0; row < grdBasis.Rows.Count; row++)
                        grdBasis.Rows[row].Cells["Merchandise"].Value = txtCopyToNode.Text;
                }
                else
                    grdBasis.DisplayLayout.Bands[0].Columns["Merchandise"].CellActivation = Activation.AllowEdit;
            }
        }

        private void cboFromLevel_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (cboFromLevel.SelectedIndex == -1) return;
            // BEGIN Track #6107 – John Smith - Cannot view departments in multi-level
            //MIDListBoxItem selectedItem = (MIDListBoxItem)cboFromLevel.SelectedItem;
            //_OTSForecastCopyMethod.FromLevel = selectedItem.Key;

            if (FormLoaded)
            {
                ChangePending = true;

                if (txtCopyToNode.Tag != null)
                {
                    HierarchyNodeProfile hnp = SAB.HierarchyServerSession.GetNodeData(_OTSForecastCopyMethod.HierNodeRID);
                    //PopulateFromToLevels(hnp, cboToLevel.ComboBox, cboFromLevel.SelectedIndex);// TT#7 - RBeck - Dynamic dropdowns
                    PopulateFromToLevels(hnp, cboToLevel, cboFromLevel.SelectedIndex);  //TT#3057 - MD - Copy chain multi level settings not holding - RBeck
                    ApplySecurity();
                }

                FromLevelCombo selectedItem = (FromLevelCombo)cboFromLevel.SelectedItem;
                _OTSForecastCopyMethod.FromLevelType = selectedItem.FromLevelType;
                _OTSForecastCopyMethod.FromLevelOffset = selectedItem.FromLevelOffset;
                _OTSForecastCopyMethod.FromLevelSequence = selectedItem.FromLevelSequence;
            }
            // END Track #6107
        }

        private void cboToLevel_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (cboToLevel.SelectedIndex == -1) return;
            // BEGIN Track #6107 – John Smith - Cannot view departments in multi-level
            //MIDListBoxItem selectedItem = (MIDListBoxItem)cboToLevel.SelectedItem;
            //_OTSForecastCopyMethod.ToLevel = selectedItem.Key;
            if (FormLoaded)
            {
                ToLevelCombo selectedItem = (ToLevelCombo)cboToLevel.SelectedItem;
                _OTSForecastCopyMethod.ToLevelType = selectedItem.ToLevelType;
                _OTSForecastCopyMethod.ToLevelOffset = selectedItem.ToLevelOffset;
                _OTSForecastCopyMethod.ToLevelSequence = selectedItem.ToLevelSequence;
            }
            // END Track #6107
        }

        // BEGIN Track #6107 – John Smith - Cannot view departments in multi-level
        //private void LoadLevelCombos()
        //{
        //    cboFromLevel.Items.Clear();
        //    cboToLevel.Items.Clear();

        //    HierarchyProfile hier = SAB.HierarchyServerSession.GetMainHierarchyData();
        //    HierarchyNodeProfile hnp = SAB.HierarchyServerSession.GetNodeData((int)txtCopyToNode.Tag, true, true);

        //    for (int i = hnp.HomeHierarchyLevel; i < hier.HierarchyLevels.Count; i++)
        //    {
        //        if (i != 0)
        //        {
        //            HierarchyLevelProfile hlp = (HierarchyLevelProfile)hier.HierarchyLevels[i];
        //            cboFromLevel.Items.Add(new MIDListBoxItem(hlp.Level, hlp.LevelID));
        //            cboToLevel.Items.Add(new MIDListBoxItem(hlp.Level, hlp.LevelID));
        //        }
        //        else
        //        {
        //            cboFromLevel.Items.Add(new MIDListBoxItem(0, txtCopyToNode.Text));
        //            cboToLevel.Items.Add(new MIDListBoxItem(0, txtCopyToNode.Text));
        //        }
        //    }
        //}
        //// END Track #6107

//Begin TT#3057 - MD - Copy chain multi level settings not holding - RBeck
		// private void PopulateFromToLevels(HierarchyNodeProfile aHierarchyNodeProfile, ComboBox aComboBox, int toOffset)
        private void PopulateFromToLevels(HierarchyNodeProfile aHierarchyNodeProfile, MIDComboBoxEnh aComboBox, int toOffset)    // T E S T 
        {
            try
            {
                HierarchyProfile hierProf;
				
			//	  object oldSelectedItem = aComboBox.SelectedItem;
            //    aComboBox.Items.Clear();				
                object oldSelectedItem = aComboBox.ComboBox.SelectedItem;    // TT#3057
                aComboBox.ComboBox.Items.Clear();    // TT#3057

                int offset = 0;
                int fromLimit = 0;
				//if (aComboBox.Name == "cboFromLevel")
                if (aComboBox.ComboBox.Name == "cboFromLevel")    // TT#3057
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
							//  if (aComboBox.Name == "cboFromLevel")
                                if (aComboBox.ComboBox.Name == "cboFromLevel")   // TT#3057
                                {
									//aComboBox.Items.Add(
                                    aComboBox.ComboBox.Items.Add(    // TT#3057
                                        new FromLevelCombo(eFromLevelsType.HierarchyLevel,
                                        0,
                                        0,
                                        hierProf.HierarchyID));
                                }
                                else
                                {
                                   //if (cboFromLevel.Items.Count > 0) 
									if (cboFromLevel.ComboBox.Items.Count > 0)   // TT#3057 
                                    {
                                        //aComboBox.Items.Add(
										aComboBox.ComboBox.Items.Add(    // TT#3057
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
                                   // if (aComboBox.Name == "cboFromLevel") 
									if (aComboBox.ComboBox.Name == "cboFromLevel")   // TT#3057
                                    {
                                       //aComboBox.Items.Add(
									    aComboBox.ComboBox.Items.Add(    // TT#3057 
                                            new FromLevelCombo(eFromLevelsType.HierarchyLevel,
                                            0,
                                            hlp.Key,
                                            hlp.LevelID));
                                    }
                                    else
                                    {
                                       //if (cboFromLevel.Items.Count > 0) 
										if (cboFromLevel.ComboBox.Items.Count > 0)   // TT#3057 
                                        {
                                          // aComboBox.Items.Add(
										    aComboBox.ComboBox.Items.Add(    // TT#3057
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

                        // add offsets to comboBox
                        
                        //BEGIN TT#4689 - DOConnell - OTS Forecast - Multi-Level Low Levels not being populated correctly
                        //int longestBranchCount = SAB.HierarchyServerSession.GetLongestBranch(aHierarchyNodeProfile.Key);
                        DataTable hierarchyLevels = SAB.HierarchyServerSession.GetHierarchyDescendantLevels(aHierarchyNodeProfile.Key);
                        int longestBranchCount = hierarchyLevels.Rows.Count - 1;
                        //END TT#4689 - DOConnell - OTS Forecast - Multi-Level Low Levels not being populated correctly

                        //if (aComboBox.Name == "cboFromLevel")
						if (aComboBox.ComboBox.Name == "cboFromLevel")    // TT#3057
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
                            if (aComboBox.ComboBox.Name == "cboFromLevel")    // TT#3057
                            {
                                //aComboBox.Items.Add(
								aComboBox.ComboBox.Items.Add(    // TT#3057 
                                new FromLevelCombo(eFromLevelsType.LevelOffset,
                                offset,
                                0,
                                null));
                            }
                            else
                            {
                                //aComboBox.Items.Add(
								aComboBox.ComboBox.Items.Add(    // TT#3057
                                new ToLevelCombo(eToLevelsType.LevelOffset,
                                offset,
                                0,
                                null));
                            }
                        }
                    }

                    //if (aComboBox.Items.Count > 0)
					if (aComboBox.ComboBox.Items.Count > 0)     // TT#3057
                    {
                        if (toOffset > 0)
                        {
                            //int count = aComboBox.Items.Count;
							int count = aComboBox.ComboBox.Items.Count;     // TT#3057
                            for (int i = 0; i < toOffset; i++)
                            {
                                //aComboBox.Items.RemoveAt(0);
								aComboBox.ComboBox.Items.RemoveAt(0);    // TT#3057
                            }

                            // if (oldSelectedItem != null && aComboBox.Items.IndexOf(oldSelectedItem) > -1)
							if (oldSelectedItem != null && aComboBox.ComboBox.Items.IndexOf(oldSelectedItem) > -1)    // TT#3057 
                            {
                                //aComboBox.SelectedIndex = aComboBox.Items.IndexOf(oldSelectedItem);
								aComboBox.SelectedIndex = aComboBox.ComboBox.Items.IndexOf(oldSelectedItem);    // TT#3057 
                            }
                            else
                            {
                                aComboBox.SelectedIndex = 0;    // TT#3057
                            }
                        }
                        else
                        {
                            //if (oldSelectedItem != null && aComboBox.Items.IndexOf(oldSelectedItem) > -1 && aComboBox.Name == "cboToLevel")
							if (oldSelectedItem != null && aComboBox.ComboBox.Items.IndexOf(oldSelectedItem) > -1 && aComboBox.Name == "cboToLevel")    // TT#3057
                            {
                                //aComboBox.SelectedIndex = aComboBox.Items.IndexOf(oldSelectedItem);
								aComboBox.SelectedIndex = aComboBox.ComboBox.Items.IndexOf(oldSelectedItem);    // TT#3057
                            }
                            else
                            {                              
                                aComboBox.SelectedIndex = 0;
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
//End TT#3057 - MD - Copy chain multi level settings not holding - RBeck

		// BEGIN Override Low Level enhancment
		private void btnOverrideLowerLevels_Click(object sender, System.EventArgs e)
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
					args = new object[] { SAB, _OTSForecastCopyMethod.OverrideLowLevelRid, _OTSForecastCopyMethod.HierNodeRID, _OTSForecastCopyMethod.VersionRID, lowLevelText, _OTSForecastCopyMethod.CustomOLL_RID, methodSecurity };
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
					if (_OTSForecastCopyMethod.OverrideLowLevelRid != e.aOllRid)
						ChangePending = true;
					_OTSForecastCopyMethod.OverrideLowLevelRid = e.aOllRid;
					if (_OTSForecastCopyMethod.CustomOLL_RID != e.aCustomOllRid)
					{
						_OTSForecastCopyMethod.CustomOLL_RID = e.aCustomOllRid;
						UpdateMethodCustomOLLRid(_OTSForecastCopyMethod.Key, _OTSForecastCopyMethod.CustomOLL_RID);
					}

                    //Begin tt#700 - APicchetti - forecast methods show the method has changed when user copens Override Low Level model and then clicks close
                    if (_overrideLowLevelfrm.DialogResult != DialogResult.Cancel)
                    {
                        LoadOverrideModelComboBox(cboOverride.ComboBox, e.aOllRid, _OTSForecastCopyMethod.CustomOLL_RID);// TT#7 - RBeck - Dynamic dropdowns  
                    }

                    _overrideLowLevelfrm = null;
                    // End tt#700


					//LoadOverrideModelComboBox(cboOverride, e.aOllRid, _OTSForecastCopyMethod.CustomOLL_RID);
				}
			}
			catch
			{
				throw;
			}

		}

        private void cboOverride_SelectionChangeCommitted(object sender, System.EventArgs e)
		{
            if (FormLoaded)
            {
                _OTSForecastCopyMethod.OverrideLowLevelRid = ((ComboObject)cboOverride.SelectedItem).Key;
                ChangePending = true;
            }
		}
        // END Override Low Level enhancment

        // Begin Track #4872 - JSmith - Global/User Attributes
        private void frmCopyForecastMethod_Load(object sender, EventArgs e)
        {
            if (cboStoreAttribute.ReplaceAttribute)
            {
                ChangePending = true;
            }

            // Begin TT#253-MD - JSmith - Attribute Set not holding and extra drop down appears on the Method
            cboVariable.Visible = false;
            // End TT#253-MD - JSmith - Attribute Set not holding and extra drop down appears on the Method

			ApplySecurity();
        }

        private void cboStoreAttribute_DragEnter(object sender, DragEventArgs e)
        {
            Image_DragEnter(sender, e);
        }

        private void cboStoreAttribute_DragOver(object sender, DragEventArgs e)
        {
            Image_DragOver(sender, e);
        }

        private void cboFilter_DragOver(object sender, DragEventArgs e)
        {
            Image_DragOver(sender, e);
        }

        // End Track #4872

        // Begin Track #6347 - JSmith - Copy Store Forecasting seems to be running extremely long
        private void chxCopyPreInitValues_CheckedChanged(object sender, EventArgs e)
        {
            if (FormLoaded)
            {
                _OTSForecastCopyMethod.CopyPreInitValues = chxCopyPreInitValues.Checked;
                if (chxCopyPreInitValues.Checked)
                {
                    cbxAttributeSet.SelectedIndex = 0;
                }
                ChangePending = true;
            }
            if (_methodType == eMethodType.CopyStoreForecast)
            {
                if (chxCopyPreInitValues.Checked)
                {
                    cboStoreAttribute.Enabled = false;
                    cbxAttributeSet.Enabled = false;
                    cboFilter.Enabled = false;
                }
                else
                {
                    cboStoreAttribute.Enabled = true;
                    cbxAttributeSet.Enabled = true;
                    cboFilter.Enabled = true;
                }
            }
        }

        private void grdBasis_AfterCellListCloseUp(object sender, CellEventArgs e)
        {
            if (FormLoaded)
            {
                // Begin TT#208 - JSmith - Copy PreInit Values checkbox remains disabled
                grdBasis.UpdateData();
                //SetPreInitOption();
                // End 208
            }
        }
        // End Track #6347
	}
}
