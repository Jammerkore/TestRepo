using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Forms;
using System.Data;
using System.Text.RegularExpressions;
using System.Linq;
using System.Collections.Generic;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using MIDRetail.Business;
using MIDRetail.Common;
using MIDRetail.DataCommon;
using MIDRetail.Data;
using MIDRetail.Windows.Controls;
using Logility.ROUI;

namespace MIDRetail.Windows
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class frmGlobalOptions : MIDFormBase
	{
		private SessionAddressBlock _SAB;
		private int _cboAllocStoreAttrValue;
		private int _cboPlanStoreAttrValue;
		private int _cboStoreValue;
		private int _cboProductValue;
		private string _cboStateValue;
		private int _cboHeaderLinkCharValue;
		private FunctionSecurityProfile _securityCompanyInfo;
		private FunctionSecurityProfile _securityDisplayOptions;
		private FunctionSecurityProfile _securityOTSPlanVersions;
		private FunctionSecurityProfile _securityAllocationDefaults;
//Begin Track #3784 - JScott - Add security for Header Gloabl Options
		private FunctionSecurityProfile _securityAllocationHeaders;
//End Track #3784 - JScott - Add security for Header Gloabl Options
		//Begin Track #6240 - stodd - Add security for basis labels and ots defaults
		private FunctionSecurityProfile _securityBasisLabels;
		private FunctionSecurityProfile _securityOTSDefaults;
		//End Track #6240 - stodd - Add security for basis labels and ots defaults
		private eGenerateSizeCurveUsing _generateSizeCurveUsing;
        private eVSWSizeConstraints _vswSizeConstraints;  // TT#246-MD - AGallagher - VSW Size - Add In-store Size Constraint options
        private eVSWItemFWOSMax _vswItemFWOSMax;   // TT#933-MD - AGallagher - Item Max vs. FWOS Max
        private int _cboDCCartonRoundDfltAttrValue;  // TT#1652-MD - RMatelic - DC Carton Rounding
		private DataTable _dtFV;
        private bool _versionsHaveChanged;
		private bool _servicesNeedRestarted = false;
		private bool _linkCharIndexIsReset = false;
		private bool _replaceCharacter = false;
		private char _keyedChar;
		private TabPage _currentTabPage = null;
        private bool _maxItemOverride;    // TT#1401 - AGallagher - Reservation Stores
        private bool _priorHeaderIncludeReserve; // TT#1608-MD - srisch - Working 5.4 - Prior Header 05/27/2015 
        //BEGIN TT#46-MD -jsobek -Develop My Activity Log
        private int _myActivityMessageUpperLimit = 100000;
        //END TT#46-MD -jsobek -Develop My Activity Log
        // BEGIN TT#1966-MD - AGallagher - DC Fulfillment
        DataTable dtdcf;
        private bool _DCFChangePending = false;
        private eDCFulfillmentSplitOption _split_Option;
        private char _apply_Minimums_Ind;
        private char _prioritize_Type;
        private int _header_Field;
        private int _hcg_Rid;
        private eDCFulfillmentHeadersOrder _header_Order;
        private eDCFulfillmentStoresOrder _store_Order;
        private eDCFulfillmentSplitByOption _split_By_Option;
        private eDCFulfillmentReserve _split_By_Reserve;
        private eDCFulfillmentMinimums _apply_By;
        private eDCFulfillmentWithinDC _within_Dc;
        // END TT#1966-MD - AGallagher - DC Fulfillment
        private bool _useExternalEligibilityAllocation;
        private bool _useExternalEligibilityPlanning;
        private eExternalEligibilityProductIdentifier _externalEligibilityProductIdentifier;
        private eExternalEligibilityChannelIdentifier _externalEligibilityChannelIdentifier;
        private string _externalEligibilityURL;


        private GlobalOptions_SMTP_BL SMTP_Options = new GlobalOptions_SMTP_BL(); //TT#506-MD -jsobek -Add infrastructure to allow Email to be sent from the application

		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage tabCompInfo;
		private MIDRetail.Windows.Controls.MIDComboBoxEnh cboState;
		private System.Windows.Forms.TextBox txtEmail;
		private MIDRetail.Windows.MaskedEdit txtFax;
		private MIDRetail.Windows.MaskedEdit txtPhone;
		private System.Windows.Forms.Label lblEmail;
		private System.Windows.Forms.Label lblFax;
		private System.Windows.Forms.Label lblPhone;
		private System.Windows.Forms.Label lblCompany;
		private System.Windows.Forms.TextBox txtCompany;
		private MIDRetail.Windows.MaskedEdit txtZip;
		private System.Windows.Forms.Label lblStreet;
		private System.Windows.Forms.Label lblState;
		private System.Windows.Forms.TextBox txtCity;
		private System.Windows.Forms.Label lblCity;
		private System.Windows.Forms.Label lblZip;
        private System.Windows.Forms.TextBox txtStreet;
		private System.Windows.Forms.TabPage tabStores;
		private System.Windows.Forms.Label lblNonCompPeriodWeeks;
		private System.Windows.Forms.Label lblNewStorePeriodWeeks;
		private System.Windows.Forms.Label lblNonCompPeriodEnd;
		private System.Windows.Forms.TextBox txtNonCompPeriodEnd;
		private System.Windows.Forms.Label lblNewStorePeriodEnd;
		private System.Windows.Forms.TextBox txtNewStorePeriodEnd;
		private System.Windows.Forms.Label lblNonCompPeriodBegin;
		private System.Windows.Forms.Label lblNewStorePeriodBegin;
		private System.Windows.Forms.TextBox txtNonCompPeriodBegin;
		private System.Windows.Forms.TextBox txtNewStorePeriodBegin;
        // Begin Track #4872 - JSmith - Global/User Attributes
        private MIDAttributeComboBox cboAllocStoreAttr;
        private MIDAttributeComboBox cboPlanStoreAttr;
        // End Track #4872
		private MIDRetail.Windows.Controls.MIDComboBoxEnh cboStore;
		private MIDRetail.Windows.Controls.MIDComboBoxEnh cboProduct;
		private System.Windows.Forms.Label lblNonCompPeriod;
		private System.Windows.Forms.Label lblNewStorePeriod;
		private System.Windows.Forms.Label lblAllocStoreAttr;
		private System.Windows.Forms.Label lblPlanStoreAttr;
		private System.Windows.Forms.Label lblStore;
		private System.Windows.Forms.Label lblProduct;
		private System.Windows.Forms.TabPage tabOTS;
		private Infragistics.Win.UltraWinGrid.UltraGrid uGridVersions;
		private System.Windows.Forms.TabPage tabAllocDefault;
		private System.Windows.Forms.Label lblShippingHorizon;
		private System.Windows.Forms.Label lblShippingHorizonWeeks;
		private System.Windows.Forms.TextBox txtShippingHorizonWeeks;
		private System.Windows.Forms.GroupBox gboSizeOptions;
		private System.Windows.Forms.Label lblFillSizeHoles;
		private System.Windows.Forms.Label lblFillSizeHolesPct;
		private System.Windows.Forms.TextBox txtFillSizeHoles;
		private System.Windows.Forms.Label lblPackDevTolerancePct;
		private System.Windows.Forms.TextBox txtPackDevTolerance;
		private System.Windows.Forms.Label lblPackDevTolerance;
		private System.Windows.Forms.Label lblPackNeedTolerance;
		private System.Windows.Forms.TextBox txtPackNeedTolerance;
		private System.Windows.Forms.Label lblSGPeriodWeeks;
		private System.Windows.Forms.Label lblPctBalTolerance;
		private System.Windows.Forms.Label lblPctNeedLimitPct;
		private System.Windows.Forms.Label lblSGPeriod;
		private System.Windows.Forms.TextBox txtSGPeriod;
		private System.Windows.Forms.Label lblBalTolerance;
		private System.Windows.Forms.TextBox txtPctBalTolerance;
		private System.Windows.Forms.Label lblNeedLimit;
		private System.Windows.Forms.TextBox txtPctNeedLimit;
		private System.Windows.Forms.ToolTip toolTip1;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.CheckedListBox clbHeaderTypeRelease;
		private System.Windows.Forms.TabPage tabAllocHeaders;
		private System.Windows.Forms.Label lblProductLevelDelimiter;
		private System.Windows.Forms.TextBox txtProductLevelDelimiter;
		private System.Windows.Forms.CheckBox cboProtectInterfacedHeaders;
		private System.Windows.Forms.Label lblHeaderLinkChar;
		private MIDRetail.Windows.Controls.MIDComboBoxEnh cboHeaderLinkChar;
		private System.Windows.Forms.GroupBox gbReqSizeFiters;
		private System.Windows.Forms.CheckBox cboSizeCurve;
		private System.Windows.Forms.CheckBox cboSizeGroup;
		private System.Windows.Forms.CheckBox cboSizeAlternates;
		private System.Windows.Forms.CheckBox cboSizeConstraints;
		private System.Windows.Forms.Label lblCharMask1;
		private System.Windows.Forms.Label lblCharMask2;
		private System.Windows.Forms.TextBox txtSizeConstraints;
		private System.Windows.Forms.TextBox txtSizeCurve;
		private System.Windows.Forms.TextBox txtSizeGroup;
		private System.Windows.Forms.TextBox txtSizeAlternates;
		private System.Windows.Forms.Label lblNormalizeSizeCurves;
		private System.Windows.Forms.RadioButton radNormalizeSizeCurves_Yes;
		private System.Windows.Forms.RadioButton radNormalizeSizeCurves_No;
		private System.Windows.Forms.GroupBox gboNormalizeSizeCurves;
		private System.Windows.Forms.GroupBox gboFillSizesTo;
		private System.Windows.Forms.Label lblFillSizesTo;
		private System.Windows.Forms.RadioButton radFillSizesTo_Holes;
		private System.Windows.Forms.RadioButton radFillSizesTo_SizePlan;
		private System.Windows.Forms.CheckBox cboDoNotReleaseIfAllInReserve;
        private TabPage tabBasisLabels;
		private System.ComponentModel.IContainer components;
        private RowColChooserOrderPanel _basisLabelVarChooser;
        private ArrayList _selectableVariableList;
        private GroupBox FRBLgroupBox;
        private ArrayList _basisLabelSelectableVariableList;
        private TabPage tabOTSDefaults;
        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private CheckBox ckbProrateChainStock;
        private Label lblNumberOfWeeksWithZeroSales;
        private TextBox txtNumberOfWeeksWithZeroSales;
        private Label lblMaximumChainWOS;
		private TextBox txtMaximumChainWOS;
        private GroupBox gboGenericSizeCurveName;
        private RadioButton radGenericSizeCurveName_HeaderCharacteristics;
        private RadioButton radGenericSizeCurveName_NodePropertiesName;
        private Label lblGenericSizeCurveNameUsing;
		private MIDRetail.Windows.Controls.MIDComboBoxEnh cboGenerateSizeCurveUsing;
		private Label lblGenSizeCurveUsing;
		private GroupBox gbxGenericPackRounding;
        private Label lblPctNthPackRoundUpFrom;
        private Label lblPct1stPackRoundUpFrom;
        private TextBox txtPctNthPackRoundUpFrom;
        private TextBox txtPct1stPackRoundUpFrom;
        private Label lblPCtNthPackRoundUpFromPct;
        private Label lblPct1stPackRoundUpFromPct;
        private CheckBox cbxNoMaxStep;
        private CheckBox cbxStepped;
        private GroupBox gboItemMaxOverride;
        private RadioButton radItemMaxOverrideNo;
        private RadioButton radItemMaxOverrideYes;
        private Label lbl_VSWSizwConstraints;
        private MIDWindowsComboBox cboVSWSizeContraints;
        private Label lblMyActivityMessageUpperLimit;
        private TextBox txtMyActivityMessageUpperLimit;
        private GlobalOptionsSMTP SMTP_Control;
        private TabPage tabSystem;
        private Panel panel1;
        private GroupBox gbSystem;
        private Label lblBatchModeLastChanged;
        private Label lblLastChangedBy;
        private Label lblMessageForClients;
        private TextBox txtSendMsg;
        private Button btnSendMsg;
        private Button btnBatchModeTurnOff;
        private Button btnBatchModeTurnOn;
        private Label lblBatchOnlyMode;
        private GroupBox gbLoginOptions;
        private CheckBox cbxForceSingleClientInstance;
        private CheckBox cbxForceSingleUserInstance;
        private GroupBox gbCurrentUsers;
        private CheckBox cbxControlServiceDefaultBatchOnlyModeOn;
        private CheckBox cbxEnableRemoteSystemOptions;
        private Panel pnlCurrentUsers;
        private Infragistics.Win.Misc.UltraPanel pnlCurrentUsers_Fill_Panel;
        private UltraGrid ultraGrid1;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _pnlCurrentUsers_Toolbars_Dock_Area_Left;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsManager ultraToolbarsManager1;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _pnlCurrentUsers_Toolbars_Dock_Area_Right;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _pnlCurrentUsers_Toolbars_Dock_Area_Bottom;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _pnlCurrentUsers_Toolbars_Dock_Area_Top;
        private ProfileList _varProfList;
        private RadioButton radFillSizesTo_SizePlanWithSizeMins;
        private CheckBox cbxEnableVelocityGradeOptions;
        private GroupBox gboItemFWOS;
        private RadioButton radItemFWOSLowest;
        private RadioButton radItemFWOSHighest;
        private RadioButton radItemFWOSDefault;
        //BEGIN TT#3509-VStuart-Cannot delete OTS Plan Version-ANFUser
        private ContextMenuStrip contextMenuStrip1;
        private ToolStripMenuItem toolStripMenuDelete;
        private ToolStripMenuItem toolStripMenuInUse;
        private ArrayList _ridList;
        private eProfileType etype;
        private Button btnInUse;
        private RadioButton radActiveDirectoryWithDomain;
        private RadioButton radActiveDirectoryAuthentication;
        private RadioButton radWindowsAuthentication;
        private RadioButton radStandardAuthentication;
        private CheckBox chkPriorHeaderIncludeReserve;
        private GroupBox gboDCCartonAttribute;
        private MIDAttributeComboBox cboDCCartonRoundDfltAttribute;
        private GroupBox groupBox3;
        private GroupBox gbxMasterSplitOptions;
        private CheckBox cbxApplyMinimums;
        private GroupBox groupBox4;
        private RadioButton rbProportional;
        private RadioButton rbDCFulfillment;
        private GroupBox gboAppplyOverageTo;
        private RadioButton rbHeadersDescending;
        private RadioButton rbHeadersAscending;
        private MIDAttributeComboBox cboPrioritizeHeadersBy;
        private Label lblPrioritizeHeadersBy;
        private GroupBox gbxOrderStoresBy;
        private GroupBox gbxStoreOrder;
        private RadioButton rbStoresDescending;
        private ContextMenu mnuGrids;
        private RadioButton rbStoresAscending;
        private UltraGrid ugOrderStoresBy;
        private GroupBox gbxDCFulfillmentSplitBy;
        private GroupBox gbxMinimums;
        private RadioButton radApplyAllStores;
        private Label lblMinimums;
        private RadioButton radApplyAllocQty;
        private GroupBox gbxReserve;
        private RadioButton radPostSplit;
        private RadioButton radPreSplit;
        private Label lblReserve;
        private GroupBox gbxSplitBy;
        private RadioButton radSplitDCStore;
        private RadioButton radSplitStoreDC;
        private Label lblSplit;
        private GroupBox gbxWithinDC;
        private Label label2;
        private RadioButton radWithinDCProportional;
        private RadioButton radWithinDCFill;
        private Label lblWithinDC;
        private bool inQuiry = true;
        //END  TT#3509-VStuart-Cannot delete OTS Plan Version-ANFUser

		public frmGlobalOptions(SessionAddressBlock aSAB) : base (aSAB)
		{
			//
			// Required for Windows Form Designer support
			//
			_SAB = aSAB;

			InitializeComponent();
            // begin TT#1365 - JEllis - FL Detail Pack Size Need Enhancement
            cbxNoMaxStep.CheckedChanged += new EventHandler(cbxNoMaxStep_CheckedChanged);
            cbxStepped.CheckedChanged += new EventHandler(cbxStepped_CheckedChanged);
            // end TT#1365 - JEllis - FL Detail Pack Size Need Enhancement

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
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

                if (_basisLabelVarChooser != null)
                {
                    _basisLabelVarChooser.Dispose();
                }

				this.tabControl1.SelectedIndexChanged -= new System.EventHandler(this.tabControl1_SelectedIndexChanged);
				this.cboState.TextChanged -= new System.EventHandler(this.comboBox_TextChanged);
				this.cboState.SelectionChangeCommitted -= new System.EventHandler(this.cboState_SelectionChangeCommitted);
				this.txtEmail.TextChanged -= new System.EventHandler(this.textBox_TextChanged);
				this.txtFax.TextChanged -= new System.EventHandler(this.textBox_TextChanged);
				this.txtPhone.TextChanged -= new System.EventHandler(this.textBox_TextChanged);
				this.txtCompany.Validating -= new System.ComponentModel.CancelEventHandler(this.txtCompanyName_Validating);
				this.txtCompany.TextChanged -= new System.EventHandler(this.textBox_TextChanged);
				this.txtZip.TextChanged -= new System.EventHandler(this.textBox_TextChanged);
				this.txtCity.TextChanged -= new System.EventHandler(this.textBox_TextChanged);
				this.txtStreet.TextChanged -= new System.EventHandler(this.textBox_TextChanged);
				this.txtNonCompPeriodEnd.Validating -= new System.ComponentModel.CancelEventHandler(this.txtNewStore_Validating);
				this.txtNonCompPeriodEnd.TextChanged -= new System.EventHandler(this.txtNonCompPeriodEnd_TextChanged);
				this.txtNewStorePeriodEnd.Validating -= new System.ComponentModel.CancelEventHandler(this.txtNewStore_Validating);
				this.txtNewStorePeriodEnd.TextChanged -= new System.EventHandler(this.txtNewStorePeriodEnd_TextChanged);
				this.txtNonCompPeriodBegin.Validating -= new System.ComponentModel.CancelEventHandler(this.txtNewStore_Validating);
				this.txtNonCompPeriodBegin.TextChanged -= new System.EventHandler(this.txtNonCompPeriodBegin_TextChanged);
				this.txtNewStorePeriodBegin.Validating -= new System.ComponentModel.CancelEventHandler(this.txtNewStore_Validating);
				this.txtNewStorePeriodBegin.TextChanged -= new System.EventHandler(this.txtNewStorePeriodBegin_TextChanged);
				this.cboAllocStoreAttr.SelectionChangeCommitted -= new System.EventHandler(this.cboAllocStoreAttr_SelectionChangeCommitted);
				this.cboPlanStoreAttr.SelectionChangeCommitted -= new System.EventHandler(this.cboPlanStoreAttr_SelectionChangeCommitted);
				this.cboStore.SelectionChangeCommitted -= new System.EventHandler(this.cboStore_SelectionChangeCommitted);
				this.cboProduct.SelectionChangeCommitted -= new System.EventHandler(this.cboProduct_SelectionChangeCommitted);
                //Begin TT#316 -jsobek -Replace all Windows Combobox controls with new enhanced control
                this.cboState.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler(cboState_MIDComboBoxPropertiesChangedEvent);
                this.cboAllocStoreAttr.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler(cboAllocStoreAttr_MIDComboBoxPropertiesChangedEvent);
                this.cboPlanStoreAttr.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler(cboPlanStoreAttr_MIDComboBoxPropertiesChangedEvent);
                this.cboStore.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler(cboStore_MIDComboBoxPropertiesChangedEvent);
                this.cboProduct.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler(cboProduct_MIDComboBoxPropertiesChangedEvent);
                this.cboGenerateSizeCurveUsing.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler(cboGenerateSizeCurveUsing_MIDComboBoxPropertiesChangedEvent);
                this.cboHeaderLinkChar.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler(cboHeaderLinkChar_MIDComboBoxPropertiesChangedEvent);
                //End TT#316
				this.uGridVersions.MouseEnterElement -= new Infragistics.Win.UIElementEventHandler(this.uGridVersions_MouseEnterElement);
				this.uGridVersions.AfterRowInsert -= new Infragistics.Win.UltraWinGrid.RowEventHandler(this.uGridVersions_AfterRowInsert);
				this.uGridVersions.BeforeRowUpdate -= new Infragistics.Win.UltraWinGrid.CancelableRowEventHandler(this.uGridVersions_BeforeRowUpdate);
				this.uGridVersions.BeforeRowsDeleted -= new Infragistics.Win.UltraWinGrid.BeforeRowsDeletedEventHandler(this.uGridVersions_BeforeRowsDeleted);
				this.uGridVersions.InitializeLayout -= new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.uGridVersions_InitializeLayout);
                //Begin TT#169 - JSmith - enter max of 1000 and receive 'must be integer'
                MIDRetail.Windows.Controls.UltraGridLayoutDefaults ugld = new MIDRetail.Windows.Controls.UltraGridLayoutDefaults(ErrorImage);
                ugld.DetachGridEventHandlers(uGridVersions);
                //End TT#169
				this.uGridVersions.CellChange -= new Infragistics.Win.UltraWinGrid.CellEventHandler(this.uGridVersions_CellChange);
				this.txtShippingHorizonWeeks.Validating -= new System.ComponentModel.CancelEventHandler(this.txtPositiveInteger_Validating);
				this.txtShippingHorizonWeeks.TextChanged -= new System.EventHandler(this.textBox_TextChanged);
				this.txtFillSizeHoles.Validating -= new System.ComponentModel.CancelEventHandler(this.txtZeroToOneHundredPercent_Validating);
				this.txtFillSizeHoles.TextChanged -= new System.EventHandler(this.textBox_TextChanged);
                // BEGIN TT#616 - AGallagher - Allocation - Pack Rounding (#67)
                this.txtPct1stPackRoundUpFrom.Validating -= new System.ComponentModel.CancelEventHandler(this.txtZeroToOneHundredPercent_Validating);
                this.txtPct1stPackRoundUpFrom.TextChanged -= new System.EventHandler(this.textBox_TextChanged);
                this.txtPctNthPackRoundUpFrom.Validating -= new System.ComponentModel.CancelEventHandler(this.txtZeroToOneHundredPercent_Validating);
                this.txtPctNthPackRoundUpFrom.TextChanged -= new System.EventHandler(this.textBox_TextChanged);
                // END TT#616 - AGallagher - Allocation - Pack Rounding (#67)
				this.txtPackDevTolerance.Validating -= new System.ComponentModel.CancelEventHandler(this.txtPositivePercent_Validating);
				this.txtPackDevTolerance.TextChanged -= new System.EventHandler(this.textBox_TextChanged);
				this.txtPackNeedTolerance.Validating -= new System.ComponentModel.CancelEventHandler(this.txtPositivePercent_Validating);
				this.txtPackNeedTolerance.TextChanged -= new System.EventHandler(this.textBox_TextChanged);
				this.txtSGPeriod.Validating -= new System.ComponentModel.CancelEventHandler(this.txtPositiveInteger_Validating);
				this.txtSGPeriod.TextChanged -= new System.EventHandler(this.textBox_TextChanged);
				this.txtPctBalTolerance.Validating -= new System.ComponentModel.CancelEventHandler(this.txtPositivePercent_Validating);
				this.txtPctBalTolerance.TextChanged -= new System.EventHandler(this.textBox_TextChanged);
				this.txtPctNeedLimit.Validating -= new System.ComponentModel.CancelEventHandler(this.txtPercent_Validating);
				this.txtPctNeedLimit.TextChanged -= new System.EventHandler(this.textBox_TextChanged);
				// BEGIN MID Track #4357 - security for interfaced and non-interfaced headers
				this.cboProtectInterfacedHeaders.CheckedChanged -= new System.EventHandler(this.cboProtectInterfacedHeaders_CheckedChanged);
				// END MID Track #4357
                this.txtNumberOfWeeksWithZeroSales.Validating -= new System.ComponentModel.CancelEventHandler(this.txtPositiveInteger_Validating);
                this.txtMaximumChainWOS.Validating -= new System.ComponentModel.CancelEventHandler(this.txtPositiveInteger_Validating);
                this.ckbProrateChainStock.CheckedChanged -= new System.EventHandler(this.checkbox_CheckedChanged);
				// BEGIN ANF Generic Size Constraints
				this.txtSizeConstraints.KeyPress -= new System.Windows.Forms.KeyPressEventHandler(this.txtCharMask_KeyPress);
				this.txtSizeConstraints.MouseHover -= new System.EventHandler(this.maskTextBox_MouseHover);
				this.txtSizeConstraints.TextChanged -= new System.EventHandler(this.maskTextBox_TextChanged);
				this.txtSizeAlternates.KeyPress -= new System.Windows.Forms.KeyPressEventHandler(this.txtCharMask_KeyPress);
				this.txtSizeAlternates.MouseHover -= new System.EventHandler(this.maskTextBox_MouseHover);
				this.txtSizeAlternates.TextChanged -= new System.EventHandler(this.maskTextBox_TextChanged);
				this.txtSizeGroup.KeyPress -= new System.Windows.Forms.KeyPressEventHandler(this.txtCharMask_KeyPress);
				this.txtSizeGroup.MouseHover -= new System.EventHandler(this.maskTextBox_MouseHover);
				this.txtSizeGroup.TextChanged -= new System.EventHandler(this.maskTextBox_TextChanged);
				this.txtSizeCurve.KeyPress -= new System.Windows.Forms.KeyPressEventHandler(this.txtCharMask_KeyPress);
				this.txtSizeCurve.MouseHover -= new System.EventHandler(this.maskTextBox_MouseHover);
				this.txtSizeCurve.TextChanged -= new System.EventHandler(this.maskTextBox_TextChanged);
				// END ANF Generic Size Constraints
				this.btnOK.Click -= new System.EventHandler(this.btnOK_Click);
				this.btnCancel.Click -= new System.EventHandler(this.btnCancel_Click);
				// BEGIN MID Track #4921 - JSmith - A&F 666 - Size Modification
				this.radFillSizesTo_Holes.CheckedChanged -= new System.EventHandler(this.radFillSizesTo_Holes_CheckedChanged);
				this.radFillSizesTo_SizePlan.CheckedChanged -= new System.EventHandler(this.radFillSizesTo_SizePlan_CheckedChanged);
				this.radNormalizeSizeCurves_No.CheckedChanged -= new System.EventHandler(this.radNormalizeSizeCurves_No_CheckedChanged);
				this.radNormalizeSizeCurves_Yes.CheckedChanged -= new System.EventHandler(this.radNormalizeSizeCurves_Yes_CheckedChanged);
                // End MID Track #4921
                // BEGIN TT#1401 - AGallagher - Reservation Stores
                this.radItemMaxOverrideNo.CheckedChanged -= new System.EventHandler(this.radItemMaxOverrideNo_CheckedChanged);
                this.radItemMaxOverrideYes.CheckedChanged -= new System.EventHandler(this.radItemMaxOverrideYes_CheckedChanged);
                // END TT#1401 - AGallagher - Reservation Stores
                // begin TT#1365 - JEllis - FL Detail Pack Size Need Enhancement
                cbxNoMaxStep.CheckedChanged -= new EventHandler(cbxNoMaxStep_CheckedChanged);
                cbxStepped.CheckedChanged -= new EventHandler(cbxStepped_CheckedChanged);
                // end TT#1365 - JEllis - FL Detail Pack Size Need Enhancement
                // Begin TT#3899 - JSmith - Remove the “Expand Headers When Intransit is Relieved” option on Headers Tab.
                //this.cboRIExpand.CheckedChanged -= new EventHandler(CheckBox_CheckedChanged);  // TT#628-MD - JSmith -  Add Option To Not Expand Allocation Records Once Relieved
                // End TT#3899 - JSmith - Remove the “Expand Headers When Intransit is Relieved” option on Headers Tab.
                // BEGIN TT#933-MD - AGallagher - Item Max vs. FWOS Max
                this.radItemFWOSDefault.CheckedChanged -= new System.EventHandler(this.radItemFWOSDefault_CheckedChanged);
                this.radItemFWOSHighest.CheckedChanged -= new System.EventHandler(this.radItemFWOSHighest_CheckedChanged);
                this.radItemFWOSLowest.CheckedChanged -= new System.EventHandler(this.radItemFWOSLowest_CheckedChanged);
                // END TT#933-MD - AGallagher - Item Max vs. FWOS Max
                this.chkPriorHeaderIncludeReserve.CheckedChanged -= new System.EventHandler(this.chkPriorHeaderIncludeReserve_CheckedChanged); //TT#1608-MD - srisch - Working 5.4 - Prior Header 05/27/2015 
                // Begin TT#1652-MD - RMatelic - DC Carton Rounding
                this.cboDCCartonRoundDfltAttribute.MIDComboBoxPropertiesChangedEvent -= new MIDRetail.Windows.Controls.MIDComboBoxPropertiesChangedEventHandler(this.cboDCCartonRoundDfltAttribute_MIDComboBoxPropertiesChangedEvent);
                this.cboDCCartonRoundDfltAttribute.SelectionChangeCommitted -= new System.EventHandler(this.cboDCCartonRoundDfltAttribute_SelectionChangeCommitted);
                //End TT#1652-MD 
                // BEGIN TT#1966-MD - AGallagher - DC Fulfillment
                this.rbStoresDescending.CheckedChanged -= new System.EventHandler(this.rbStoresDescending_CheckedChanged);
                this.rbStoresAscending.CheckedChanged -= new System.EventHandler(this.rbStoresAscending_CheckedChanged);
                this.ugOrderStoresBy.InitializeLayout -= new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.ugOrderStoresBy_InitializeLayout);
                this.ugOrderStoresBy.AfterRowInsert -= new Infragistics.Win.UltraWinGrid.RowEventHandler(this.ugOrderStoresBy_AfterRowInsert);
                this.ugOrderStoresBy.MouseEnterElement -= new Infragistics.Win.UIElementEventHandler(this.ugOrderStoresBy_MouseEnterElement);
                this.cbxApplyMinimums.CheckedChanged -= new System.EventHandler(this.cbxApplyMinimums_CheckedChanged);
                this.rbProportional.CheckedChanged -= new System.EventHandler(this.rbProportional_CheckedChanged);
                this.rbDCFulfillment.CheckedChanged -= new System.EventHandler(this.rbDCFulfillment_CheckedChanged);
                this.rbHeadersDescending.CheckedChanged -= new System.EventHandler(this.rbHeadersDescending_CheckedChanged);
                this.rbHeadersAscending.CheckedChanged -= new System.EventHandler(this.rbHeadersAscending_CheckedChanged);
                this.cboPrioritizeHeadersBy.MIDComboBoxPropertiesChangedEvent -= new MIDRetail.Windows.Controls.MIDComboBoxPropertiesChangedEventHandler(this.cboPrioritizeHeadersBy_MIDComboBoxPropertiesChangedEvent);
                this.cboPrioritizeHeadersBy.SelectionChangeCommitted -= new System.EventHandler(this.cboPrioritizeHeadersBy_SelectionChangeCommitted);
                this.radSplitStoreDC.CheckedChanged -= new System.EventHandler(this.radSplitStoreDC_CheckedChanged);
                this.radSplitDCStore.CheckedChanged -= new System.EventHandler(this.radSplitDCStore_CheckedChanged);
                this.radPreSplit.CheckedChanged -= new System.EventHandler(this.radPreSplit_CheckedChanged);
                this.radPostSplit.CheckedChanged -= new System.EventHandler(this.radPostSplit_CheckedChanged);
                this.radApplyAllocQty.CheckedChanged -= new System.EventHandler(this.radApplyAllocQty_CheckedChanged);
                this.radApplyAllStores.CheckedChanged -= new System.EventHandler(this.radApplyAllStores_CheckedChanged);
                this.radWithinDCFill.CheckedChanged -= new System.EventHandler(this.radWithinDCFill_CheckedChanged);
                this.radWithinDCProportional.CheckedChanged -= new System.EventHandler(this.radWithinDCProportional_CheckedChanged);
                this.ugOrderStoresBy.AfterCellListCloseUp -= new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ugOrderStoresBy_AfterCellListCloseUp);
                // END TT#1966-MD - AGallagher - DC Fulfillment
                this.Load -= new System.EventHandler(this.frmGlobalOptions_Load);

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
            this.components = new System.ComponentModel.Container();
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand1 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance11 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance12 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridLayout ultraGridLayout1 = new Infragistics.Win.UltraWinGrid.UltraGridLayout();
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
            Infragistics.Win.UltraWinToolbars.UltraToolbar ultraToolbar1 = new Infragistics.Win.UltraWinToolbars.UltraToolbar("CurrentUsers");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool1 = new Infragistics.Win.UltraWinToolbars.ButtonTool("btnShowCurrentUsers");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool3 = new Infragistics.Win.UltraWinToolbars.ButtonTool("btnLogOutSelected");
            Infragistics.Win.UltraWinToolbars.UltraToolbar ultraToolbar2 = new Infragistics.Win.UltraWinToolbars.UltraToolbar("ActiveUserCountToolbar");
            Infragistics.Win.UltraWinToolbars.LabelTool labelTool3 = new Infragistics.Win.UltraWinToolbars.LabelTool("lblCurrentUserCount");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool2 = new Infragistics.Win.UltraWinToolbars.ButtonTool("btnShowCurrentUsers");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool4 = new Infragistics.Win.UltraWinToolbars.ButtonTool("btnLogOutSelected");
            Infragistics.Win.UltraWinToolbars.LabelTool labelTool2 = new Infragistics.Win.UltraWinToolbars.LabelTool("lblCurrentUserCount");
            Infragistics.Win.Appearance appearance25 = new Infragistics.Win.Appearance();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabCompInfo = new System.Windows.Forms.TabPage();
            this.SMTP_Control = new MIDRetail.Windows.GlobalOptionsSMTP();
            this.txtProductLevelDelimiter = new System.Windows.Forms.TextBox();
            this.lblProductLevelDelimiter = new System.Windows.Forms.Label();
            this.cboState = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this.txtEmail = new System.Windows.Forms.TextBox();
            this.txtFax = new MIDRetail.Windows.MaskedEdit();
            this.txtPhone = new MIDRetail.Windows.MaskedEdit();
            this.lblEmail = new System.Windows.Forms.Label();
            this.lblFax = new System.Windows.Forms.Label();
            this.lblPhone = new System.Windows.Forms.Label();
            this.txtCompany = new System.Windows.Forms.TextBox();
            this.txtZip = new MIDRetail.Windows.MaskedEdit();
            this.lblStreet = new System.Windows.Forms.Label();
            this.lblState = new System.Windows.Forms.Label();
            this.txtCity = new System.Windows.Forms.TextBox();
            this.lblCity = new System.Windows.Forms.Label();
            this.lblZip = new System.Windows.Forms.Label();
            this.txtStreet = new System.Windows.Forms.TextBox();
            this.lblCompany = new System.Windows.Forms.Label();
            this.tabStores = new System.Windows.Forms.TabPage();
            this.lblMyActivityMessageUpperLimit = new System.Windows.Forms.Label();
            this.txtMyActivityMessageUpperLimit = new System.Windows.Forms.TextBox();
            this.gbReqSizeFiters = new System.Windows.Forms.GroupBox();
            this.txtSizeConstraints = new System.Windows.Forms.TextBox();
            this.txtSizeAlternates = new System.Windows.Forms.TextBox();
            this.txtSizeGroup = new System.Windows.Forms.TextBox();
            this.txtSizeCurve = new System.Windows.Forms.TextBox();
            this.lblCharMask2 = new System.Windows.Forms.Label();
            this.lblCharMask1 = new System.Windows.Forms.Label();
            this.cboSizeConstraints = new System.Windows.Forms.CheckBox();
            this.cboSizeAlternates = new System.Windows.Forms.CheckBox();
            this.cboSizeGroup = new System.Windows.Forms.CheckBox();
            this.cboSizeCurve = new System.Windows.Forms.CheckBox();
            this.lblNonCompPeriodWeeks = new System.Windows.Forms.Label();
            this.lblNewStorePeriodWeeks = new System.Windows.Forms.Label();
            this.lblNonCompPeriodEnd = new System.Windows.Forms.Label();
            this.txtNonCompPeriodEnd = new System.Windows.Forms.TextBox();
            this.lblNewStorePeriodEnd = new System.Windows.Forms.Label();
            this.txtNewStorePeriodEnd = new System.Windows.Forms.TextBox();
            this.lblNonCompPeriodBegin = new System.Windows.Forms.Label();
            this.lblNewStorePeriodBegin = new System.Windows.Forms.Label();
            this.txtNonCompPeriodBegin = new System.Windows.Forms.TextBox();
            this.txtNewStorePeriodBegin = new System.Windows.Forms.TextBox();
            this.cboAllocStoreAttr = new MIDRetail.Windows.Controls.MIDAttributeComboBox();
            this.cboPlanStoreAttr = new MIDRetail.Windows.Controls.MIDAttributeComboBox();
            this.cboStore = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this.cboProduct = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this.lblNonCompPeriod = new System.Windows.Forms.Label();
            this.lblNewStorePeriod = new System.Windows.Forms.Label();
            this.lblAllocStoreAttr = new System.Windows.Forms.Label();
            this.lblPlanStoreAttr = new System.Windows.Forms.Label();
            this.lblStore = new System.Windows.Forms.Label();
            this.lblProduct = new System.Windows.Forms.Label();
            this.tabAllocDefault = new System.Windows.Forms.TabPage();
            this.chkPriorHeaderIncludeReserve = new System.Windows.Forms.CheckBox();
            this.gboDCCartonAttribute = new System.Windows.Forms.GroupBox();
            this.cboDCCartonRoundDfltAttribute = new MIDRetail.Windows.Controls.MIDAttributeComboBox();
            this.gboItemFWOS = new System.Windows.Forms.GroupBox();
            this.radItemFWOSLowest = new System.Windows.Forms.RadioButton();
            this.radItemFWOSHighest = new System.Windows.Forms.RadioButton();
            this.radItemFWOSDefault = new System.Windows.Forms.RadioButton();
            this.cbxEnableVelocityGradeOptions = new System.Windows.Forms.CheckBox();
            this.gboItemMaxOverride = new System.Windows.Forms.GroupBox();
            this.radItemMaxOverrideNo = new System.Windows.Forms.RadioButton();
            this.radItemMaxOverrideYes = new System.Windows.Forms.RadioButton();
            this.gbxGenericPackRounding = new System.Windows.Forms.GroupBox();
            this.lblPCtNthPackRoundUpFromPct = new System.Windows.Forms.Label();
            this.lblPct1stPackRoundUpFromPct = new System.Windows.Forms.Label();
            this.txtPctNthPackRoundUpFrom = new System.Windows.Forms.TextBox();
            this.txtPct1stPackRoundUpFrom = new System.Windows.Forms.TextBox();
            this.lblPctNthPackRoundUpFrom = new System.Windows.Forms.Label();
            this.lblPct1stPackRoundUpFrom = new System.Windows.Forms.Label();
            this.lblShippingHorizon = new System.Windows.Forms.Label();
            this.lblShippingHorizonWeeks = new System.Windows.Forms.Label();
            this.txtShippingHorizonWeeks = new System.Windows.Forms.TextBox();
            this.gboSizeOptions = new System.Windows.Forms.GroupBox();
            this.cboVSWSizeContraints = new MIDRetail.Windows.Controls.MIDWindowsComboBox();
            this.lbl_VSWSizwConstraints = new System.Windows.Forms.Label();
            this.cbxNoMaxStep = new System.Windows.Forms.CheckBox();
            this.cbxStepped = new System.Windows.Forms.CheckBox();
            this.cboGenerateSizeCurveUsing = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this.lblGenSizeCurveUsing = new System.Windows.Forms.Label();
            this.gboGenericSizeCurveName = new System.Windows.Forms.GroupBox();
            this.radGenericSizeCurveName_HeaderCharacteristics = new System.Windows.Forms.RadioButton();
            this.radGenericSizeCurveName_NodePropertiesName = new System.Windows.Forms.RadioButton();
            this.lblGenericSizeCurveNameUsing = new System.Windows.Forms.Label();
            this.gboFillSizesTo = new System.Windows.Forms.GroupBox();
            this.radFillSizesTo_SizePlanWithSizeMins = new System.Windows.Forms.RadioButton();
            this.radFillSizesTo_SizePlan = new System.Windows.Forms.RadioButton();
            this.radFillSizesTo_Holes = new System.Windows.Forms.RadioButton();
            this.lblFillSizesTo = new System.Windows.Forms.Label();
            this.gboNormalizeSizeCurves = new System.Windows.Forms.GroupBox();
            this.radNormalizeSizeCurves_No = new System.Windows.Forms.RadioButton();
            this.radNormalizeSizeCurves_Yes = new System.Windows.Forms.RadioButton();
            this.lblNormalizeSizeCurves = new System.Windows.Forms.Label();
            this.lblFillSizeHoles = new System.Windows.Forms.Label();
            this.lblFillSizeHolesPct = new System.Windows.Forms.Label();
            this.txtFillSizeHoles = new System.Windows.Forms.TextBox();
            this.lblPackDevTolerancePct = new System.Windows.Forms.Label();
            this.txtPackDevTolerance = new System.Windows.Forms.TextBox();
            this.lblPackDevTolerance = new System.Windows.Forms.Label();
            this.lblPackNeedTolerance = new System.Windows.Forms.Label();
            this.txtPackNeedTolerance = new System.Windows.Forms.TextBox();
            this.lblSGPeriodWeeks = new System.Windows.Forms.Label();
            this.lblPctBalTolerance = new System.Windows.Forms.Label();
            this.lblPctNeedLimitPct = new System.Windows.Forms.Label();
            this.lblSGPeriod = new System.Windows.Forms.Label();
            this.txtSGPeriod = new System.Windows.Forms.TextBox();
            this.lblBalTolerance = new System.Windows.Forms.Label();
            this.txtPctBalTolerance = new System.Windows.Forms.TextBox();
            this.lblNeedLimit = new System.Windows.Forms.Label();
            this.txtPctNeedLimit = new System.Windows.Forms.TextBox();
            this.tabOTS = new System.Windows.Forms.TabPage();
            this.uGridVersions = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuInUse = new System.Windows.Forms.ToolStripMenuItem();
            this.tabAllocHeaders = new System.Windows.Forms.TabPage();
            this.gbxDCFulfillmentSplitBy = new System.Windows.Forms.GroupBox();
            this.gbxWithinDC = new System.Windows.Forms.GroupBox();
            this.radWithinDCProportional = new System.Windows.Forms.RadioButton();
            this.radWithinDCFill = new System.Windows.Forms.RadioButton();
            this.lblWithinDC = new System.Windows.Forms.Label();
            this.gbxMinimums = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.radApplyAllStores = new System.Windows.Forms.RadioButton();
            this.lblMinimums = new System.Windows.Forms.Label();
            this.radApplyAllocQty = new System.Windows.Forms.RadioButton();
            this.gbxReserve = new System.Windows.Forms.GroupBox();
            this.radPostSplit = new System.Windows.Forms.RadioButton();
            this.radPreSplit = new System.Windows.Forms.RadioButton();
            this.lblReserve = new System.Windows.Forms.Label();
            this.gbxSplitBy = new System.Windows.Forms.GroupBox();
            this.radSplitDCStore = new System.Windows.Forms.RadioButton();
            this.radSplitStoreDC = new System.Windows.Forms.RadioButton();
            this.lblSplit = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.gbxOrderStoresBy = new System.Windows.Forms.GroupBox();
            this.gbxStoreOrder = new System.Windows.Forms.GroupBox();
            this.rbStoresDescending = new System.Windows.Forms.RadioButton();
            this.rbStoresAscending = new System.Windows.Forms.RadioButton();
            this.ugOrderStoresBy = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.mnuGrids = new System.Windows.Forms.ContextMenu();
            this.gbxMasterSplitOptions = new System.Windows.Forms.GroupBox();
            this.cbxApplyMinimums = new System.Windows.Forms.CheckBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.rbProportional = new System.Windows.Forms.RadioButton();
            this.rbDCFulfillment = new System.Windows.Forms.RadioButton();
            this.gboAppplyOverageTo = new System.Windows.Forms.GroupBox();
            this.rbHeadersDescending = new System.Windows.Forms.RadioButton();
            this.rbHeadersAscending = new System.Windows.Forms.RadioButton();
            this.cboPrioritizeHeadersBy = new MIDRetail.Windows.Controls.MIDAttributeComboBox();
            this.lblPrioritizeHeadersBy = new System.Windows.Forms.Label();
            this.cboDoNotReleaseIfAllInReserve = new System.Windows.Forms.CheckBox();
            this.cboProtectInterfacedHeaders = new System.Windows.Forms.CheckBox();
            this.cboHeaderLinkChar = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this.lblHeaderLinkChar = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.clbHeaderTypeRelease = new System.Windows.Forms.CheckedListBox();
            this.tabBasisLabels = new System.Windows.Forms.TabPage();
            this.FRBLgroupBox = new System.Windows.Forms.GroupBox();
            this.tabOTSDefaults = new System.Windows.Forms.TabPage();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.ckbProrateChainStock = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblMaximumChainWOS = new System.Windows.Forms.Label();
            this.txtMaximumChainWOS = new System.Windows.Forms.TextBox();
            this.lblNumberOfWeeksWithZeroSales = new System.Windows.Forms.Label();
            this.txtNumberOfWeeksWithZeroSales = new System.Windows.Forms.TextBox();
            this.tabSystem = new System.Windows.Forms.TabPage();
            this.gbCurrentUsers = new System.Windows.Forms.GroupBox();
            this.pnlCurrentUsers = new System.Windows.Forms.Panel();
            this.pnlCurrentUsers_Fill_Panel = new Infragistics.Win.Misc.UltraPanel();
            this.ultraGrid1 = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this._pnlCurrentUsers_Toolbars_Dock_Area_Left = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this.ultraToolbarsManager1 = new Infragistics.Win.UltraWinToolbars.UltraToolbarsManager(this.components);
            this._pnlCurrentUsers_Toolbars_Dock_Area_Right = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this._pnlCurrentUsers_Toolbars_Dock_Area_Bottom = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this._pnlCurrentUsers_Toolbars_Dock_Area_Top = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this.panel1 = new System.Windows.Forms.Panel();
            this.gbSystem = new System.Windows.Forms.GroupBox();
            this.lblBatchModeLastChanged = new System.Windows.Forms.Label();
            this.lblLastChangedBy = new System.Windows.Forms.Label();
            this.lblMessageForClients = new System.Windows.Forms.Label();
            this.txtSendMsg = new System.Windows.Forms.TextBox();
            this.btnSendMsg = new System.Windows.Forms.Button();
            this.btnBatchModeTurnOff = new System.Windows.Forms.Button();
            this.btnBatchModeTurnOn = new System.Windows.Forms.Button();
            this.lblBatchOnlyMode = new System.Windows.Forms.Label();
            this.gbLoginOptions = new System.Windows.Forms.GroupBox();
            this.radActiveDirectoryWithDomain = new System.Windows.Forms.RadioButton();
            this.radActiveDirectoryAuthentication = new System.Windows.Forms.RadioButton();
            this.radWindowsAuthentication = new System.Windows.Forms.RadioButton();
            this.radStandardAuthentication = new System.Windows.Forms.RadioButton();
            this.cbxControlServiceDefaultBatchOnlyModeOn = new System.Windows.Forms.CheckBox();
            this.cbxEnableRemoteSystemOptions = new System.Windows.Forms.CheckBox();
            this.cbxForceSingleClientInstance = new System.Windows.Forms.CheckBox();
            this.cbxForceSingleUserInstance = new System.Windows.Forms.CheckBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnInUse = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.utmMain)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabCompInfo.SuspendLayout();
            this.tabStores.SuspendLayout();
            this.gbReqSizeFiters.SuspendLayout();
            this.tabAllocDefault.SuspendLayout();
            this.gboDCCartonAttribute.SuspendLayout();
            this.gboItemFWOS.SuspendLayout();
            this.gboItemMaxOverride.SuspendLayout();
            this.gbxGenericPackRounding.SuspendLayout();
            this.gboSizeOptions.SuspendLayout();
            this.gboGenericSizeCurveName.SuspendLayout();
            this.gboFillSizesTo.SuspendLayout();
            this.gboNormalizeSizeCurves.SuspendLayout();
            this.tabOTS.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uGridVersions)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.tabAllocHeaders.SuspendLayout();
            this.gbxDCFulfillmentSplitBy.SuspendLayout();
            this.gbxWithinDC.SuspendLayout();
            this.gbxMinimums.SuspendLayout();
            this.gbxReserve.SuspendLayout();
            this.gbxSplitBy.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.gbxOrderStoresBy.SuspendLayout();
            this.gbxStoreOrder.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ugOrderStoresBy)).BeginInit();
            this.gbxMasterSplitOptions.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.gboAppplyOverageTo.SuspendLayout();
            this.tabBasisLabels.SuspendLayout();
            this.tabOTSDefaults.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabSystem.SuspendLayout();
            this.gbCurrentUsers.SuspendLayout();
            this.pnlCurrentUsers.SuspendLayout();
            this.pnlCurrentUsers_Fill_Panel.ClientArea.SuspendLayout();
            this.pnlCurrentUsers_Fill_Panel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGrid1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraToolbarsManager1)).BeginInit();
            this.panel1.SuspendLayout();
            this.gbSystem.SuspendLayout();
            this.gbLoginOptions.SuspendLayout();
            this.SuspendLayout();
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
            this.tabControl1.Controls.Add(this.tabCompInfo);
            this.tabControl1.Controls.Add(this.tabStores);
            this.tabControl1.Controls.Add(this.tabAllocDefault);
            this.tabControl1.Controls.Add(this.tabOTS);
            this.tabControl1.Controls.Add(this.tabAllocHeaders);
            this.tabControl1.Controls.Add(this.tabBasisLabels);
            this.tabControl1.Controls.Add(this.tabOTSDefaults);
            this.tabControl1.Controls.Add(this.tabSystem);
            this.tabControl1.ItemSize = new System.Drawing.Size(111, 18);
            this.tabControl1.Location = new System.Drawing.Point(8, 16);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(655, 442);
            this.tabControl1.TabIndex = 5;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // tabCompInfo
            // 
            this.tabCompInfo.Controls.Add(this.SMTP_Control);
            this.tabCompInfo.Controls.Add(this.txtProductLevelDelimiter);
            this.tabCompInfo.Controls.Add(this.lblProductLevelDelimiter);
            this.tabCompInfo.Controls.Add(this.cboState);
            this.tabCompInfo.Controls.Add(this.txtEmail);
            this.tabCompInfo.Controls.Add(this.txtFax);
            this.tabCompInfo.Controls.Add(this.txtPhone);
            this.tabCompInfo.Controls.Add(this.lblEmail);
            this.tabCompInfo.Controls.Add(this.lblFax);
            this.tabCompInfo.Controls.Add(this.lblPhone);
            this.tabCompInfo.Controls.Add(this.txtCompany);
            this.tabCompInfo.Controls.Add(this.txtZip);
            this.tabCompInfo.Controls.Add(this.lblStreet);
            this.tabCompInfo.Controls.Add(this.lblState);
            this.tabCompInfo.Controls.Add(this.txtCity);
            this.tabCompInfo.Controls.Add(this.lblCity);
            this.tabCompInfo.Controls.Add(this.lblZip);
            this.tabCompInfo.Controls.Add(this.txtStreet);
            this.tabCompInfo.Controls.Add(this.lblCompany);
            this.tabCompInfo.Location = new System.Drawing.Point(4, 22);
            this.tabCompInfo.Name = "tabCompInfo";
            this.tabCompInfo.Size = new System.Drawing.Size(647, 416);
            this.tabCompInfo.TabIndex = 0;
            this.tabCompInfo.Text = "Company Information";
            this.tabCompInfo.UseVisualStyleBackColor = true;
            // 
            // SMTP_Control
            // 
            this.SMTP_Control.Location = new System.Drawing.Point(30, 162);
            this.SMTP_Control.Name = "SMTP_Control";
            this.SMTP_Control.Size = new System.Drawing.Size(579, 187);
            this.SMTP_Control.TabIndex = 15;
            // 
            // txtProductLevelDelimiter
            // 
            this.txtProductLevelDelimiter.Location = new System.Drawing.Point(472, 100);
            this.txtProductLevelDelimiter.Name = "txtProductLevelDelimiter";
            this.txtProductLevelDelimiter.Size = new System.Drawing.Size(24, 20);
            this.txtProductLevelDelimiter.TabIndex = 9;
            this.txtProductLevelDelimiter.Text = "\\";
            this.txtProductLevelDelimiter.TextChanged += new System.EventHandler(this.txtProductLevelDelimiter_TextChanged);
            // 
            // lblProductLevelDelimiter
            // 
            this.lblProductLevelDelimiter.Location = new System.Drawing.Point(353, 98);
            this.lblProductLevelDelimiter.Name = "lblProductLevelDelimiter";
            this.lblProductLevelDelimiter.Size = new System.Drawing.Size(128, 23);
            this.lblProductLevelDelimiter.TabIndex = 21;
            this.lblProductLevelDelimiter.Text = "Product Level Delimiter:";
            this.lblProductLevelDelimiter.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cboState
            // 
            this.cboState.AutoAdjust = true;
            this.cboState.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboState.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboState.DataSource = null;
            this.cboState.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboState.DropDownWidth = 64;
            this.cboState.FormattingEnabled = false;
            this.cboState.IgnoreFocusLost = false;
            this.cboState.ItemHeight = 13;
            this.cboState.Location = new System.Drawing.Point(108, 100);
            this.cboState.Margin = new System.Windows.Forms.Padding(0);
            this.cboState.MaxDropDownItems = 25;
            this.cboState.Name = "cboState";
            this.cboState.SetToolTip = "";
            this.cboState.Size = new System.Drawing.Size(64, 21);
            this.cboState.TabIndex = 3;
            this.cboState.Tag = null;
            this.cboState.MIDComboBoxPropertiesChangedEvent += new MIDRetail.Windows.Controls.MIDComboBoxPropertiesChangedEventHandler(this.cboState_MIDComboBoxPropertiesChangedEvent);
            this.cboState.SelectionChangeCommitted += new System.EventHandler(this.cboState_SelectionChangeCommitted);
            this.cboState.TextChanged += new System.EventHandler(this.comboBox_TextChanged);
            // 
            // txtEmail
            // 
            this.txtEmail.Location = new System.Drawing.Point(400, 72);
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Size = new System.Drawing.Size(224, 20);
            this.txtEmail.TabIndex = 7;
            this.txtEmail.TextChanged += new System.EventHandler(this.textBox_TextChanged);
            // 
            // txtFax
            // 
            this.txtFax.ErrorInvalid = false;
            this.txtFax.InputChar = ' ';
            this.txtFax.InputMask = "(000)000-0000";
            this.txtFax.Location = new System.Drawing.Point(400, 46);
            this.txtFax.MaxLength = 13;
            this.txtFax.Name = "txtFax";
            this.txtFax.Size = new System.Drawing.Size(100, 20);
            this.txtFax.StdInputMask = MIDRetail.Windows.MaskedEdit.InputMaskType.Custom;
            this.txtFax.TabIndex = 6;
            this.txtFax.TextChanged += new System.EventHandler(this.textBox_TextChanged);
            // 
            // txtPhone
            // 
            this.txtPhone.ErrorInvalid = false;
            this.txtPhone.InputChar = ' ';
            this.txtPhone.InputMask = "(000)000-0000";
            this.txtPhone.Location = new System.Drawing.Point(400, 20);
            this.txtPhone.MaxLength = 13;
            this.txtPhone.Name = "txtPhone";
            this.txtPhone.Size = new System.Drawing.Size(100, 20);
            this.txtPhone.StdInputMask = MIDRetail.Windows.MaskedEdit.InputMaskType.Custom;
            this.txtPhone.TabIndex = 5;
            this.txtPhone.TextChanged += new System.EventHandler(this.textBox_TextChanged);
            // 
            // lblEmail
            // 
            this.lblEmail.AutoSize = true;
            this.lblEmail.Location = new System.Drawing.Point(365, 75);
            this.lblEmail.Name = "lblEmail";
            this.lblEmail.Size = new System.Drawing.Size(32, 13);
            this.lblEmail.TabIndex = 5;
            this.lblEmail.Text = "Email";
            this.lblEmail.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblFax
            // 
            this.lblFax.AutoSize = true;
            this.lblFax.Location = new System.Drawing.Point(370, 49);
            this.lblFax.Name = "lblFax";
            this.lblFax.Size = new System.Drawing.Size(27, 13);
            this.lblFax.TabIndex = 4;
            this.lblFax.Text = "FAX";
            this.lblFax.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblPhone
            // 
            this.lblPhone.AutoSize = true;
            this.lblPhone.Location = new System.Drawing.Point(359, 24);
            this.lblPhone.Name = "lblPhone";
            this.lblPhone.Size = new System.Drawing.Size(38, 13);
            this.lblPhone.TabIndex = 3;
            this.lblPhone.Text = "Phone";
            this.lblPhone.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtCompany
            // 
            this.txtCompany.Location = new System.Drawing.Point(108, 20);
            this.txtCompany.Name = "txtCompany";
            this.txtCompany.Size = new System.Drawing.Size(232, 20);
            this.txtCompany.TabIndex = 0;
            this.txtCompany.TextChanged += new System.EventHandler(this.textBox_TextChanged);
            this.txtCompany.Validating += new System.ComponentModel.CancelEventHandler(this.txtCompanyName_Validating);
            // 
            // txtZip
            // 
            this.txtZip.ErrorInvalid = false;
            this.txtZip.InputChar = ' ';
            this.txtZip.InputMask = "00000-9999";
            this.txtZip.Location = new System.Drawing.Point(244, 100);
            this.txtZip.MaxLength = 10;
            this.txtZip.Name = "txtZip";
            this.txtZip.Size = new System.Drawing.Size(96, 20);
            this.txtZip.StdInputMask = MIDRetail.Windows.MaskedEdit.InputMaskType.Zip;
            this.txtZip.TabIndex = 4;
            this.txtZip.TextChanged += new System.EventHandler(this.textBox_TextChanged);
            // 
            // lblStreet
            // 
            this.lblStreet.AutoSize = true;
            this.lblStreet.Location = new System.Drawing.Point(73, 50);
            this.lblStreet.Name = "lblStreet";
            this.lblStreet.Size = new System.Drawing.Size(35, 13);
            this.lblStreet.TabIndex = 0;
            this.lblStreet.Text = "Street";
            this.lblStreet.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblState
            // 
            this.lblState.AutoSize = true;
            this.lblState.Location = new System.Drawing.Point(76, 103);
            this.lblState.Name = "lblState";
            this.lblState.Size = new System.Drawing.Size(32, 13);
            this.lblState.TabIndex = 4;
            this.lblState.Text = "State";
            this.lblState.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtCity
            // 
            this.txtCity.Location = new System.Drawing.Point(108, 72);
            this.txtCity.Name = "txtCity";
            this.txtCity.Size = new System.Drawing.Size(232, 20);
            this.txtCity.TabIndex = 2;
            this.txtCity.TextChanged += new System.EventHandler(this.textBox_TextChanged);
            // 
            // lblCity
            // 
            this.lblCity.AutoSize = true;
            this.lblCity.Location = new System.Drawing.Point(84, 75);
            this.lblCity.Name = "lblCity";
            this.lblCity.Size = new System.Drawing.Size(24, 13);
            this.lblCity.TabIndex = 2;
            this.lblCity.Text = "City";
            this.lblCity.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblZip
            // 
            this.lblZip.Location = new System.Drawing.Point(214, 98);
            this.lblZip.Name = "lblZip";
            this.lblZip.Size = new System.Drawing.Size(24, 23);
            this.lblZip.TabIndex = 6;
            this.lblZip.Text = "Zip";
            this.lblZip.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtStreet
            // 
            this.txtStreet.Location = new System.Drawing.Point(108, 46);
            this.txtStreet.Name = "txtStreet";
            this.txtStreet.Size = new System.Drawing.Size(232, 20);
            this.txtStreet.TabIndex = 1;
            this.txtStreet.TextChanged += new System.EventHandler(this.textBox_TextChanged);
            // 
            // lblCompany
            // 
            this.lblCompany.Location = new System.Drawing.Point(6, 24);
            this.lblCompany.Name = "lblCompany";
            this.lblCompany.Size = new System.Drawing.Size(100, 16);
            this.lblCompany.TabIndex = 1;
            this.lblCompany.Text = "Company Name";
            this.lblCompany.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // tabStores
            // 
            this.tabStores.Controls.Add(this.lblMyActivityMessageUpperLimit);
            this.tabStores.Controls.Add(this.txtMyActivityMessageUpperLimit);
            this.tabStores.Controls.Add(this.gbReqSizeFiters);
            this.tabStores.Controls.Add(this.lblNonCompPeriodWeeks);
            this.tabStores.Controls.Add(this.lblNewStorePeriodWeeks);
            this.tabStores.Controls.Add(this.lblNonCompPeriodEnd);
            this.tabStores.Controls.Add(this.txtNonCompPeriodEnd);
            this.tabStores.Controls.Add(this.lblNewStorePeriodEnd);
            this.tabStores.Controls.Add(this.txtNewStorePeriodEnd);
            this.tabStores.Controls.Add(this.lblNonCompPeriodBegin);
            this.tabStores.Controls.Add(this.lblNewStorePeriodBegin);
            this.tabStores.Controls.Add(this.txtNonCompPeriodBegin);
            this.tabStores.Controls.Add(this.txtNewStorePeriodBegin);
            this.tabStores.Controls.Add(this.cboAllocStoreAttr);
            this.tabStores.Controls.Add(this.cboPlanStoreAttr);
            this.tabStores.Controls.Add(this.cboStore);
            this.tabStores.Controls.Add(this.cboProduct);
            this.tabStores.Controls.Add(this.lblNonCompPeriod);
            this.tabStores.Controls.Add(this.lblNewStorePeriod);
            this.tabStores.Controls.Add(this.lblAllocStoreAttr);
            this.tabStores.Controls.Add(this.lblPlanStoreAttr);
            this.tabStores.Controls.Add(this.lblStore);
            this.tabStores.Controls.Add(this.lblProduct);
            this.tabStores.Location = new System.Drawing.Point(4, 22);
            this.tabStores.Name = "tabStores";
            this.tabStores.Size = new System.Drawing.Size(647, 416);
            this.tabStores.TabIndex = 2;
            this.tabStores.Text = "Display Options";
            this.tabStores.UseVisualStyleBackColor = true;
            this.tabStores.Visible = false;
            // 
            // lblMyActivityMessageUpperLimit
            // 
            this.lblMyActivityMessageUpperLimit.AutoSize = true;
            this.lblMyActivityMessageUpperLimit.Location = new System.Drawing.Point(391, 116);
            this.lblMyActivityMessageUpperLimit.Name = "lblMyActivityMessageUpperLimit";
            this.lblMyActivityMessageUpperLimit.Size = new System.Drawing.Size(143, 13);
            this.lblMyActivityMessageUpperLimit.TabIndex = 22;
            this.lblMyActivityMessageUpperLimit.Text = "Activity Message Upper Limit";
            this.lblMyActivityMessageUpperLimit.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtMyActivityMessageUpperLimit
            // 
            this.txtMyActivityMessageUpperLimit.Location = new System.Drawing.Point(538, 112);
            this.txtMyActivityMessageUpperLimit.Name = "txtMyActivityMessageUpperLimit";
            this.txtMyActivityMessageUpperLimit.Size = new System.Drawing.Size(70, 20);
            this.txtMyActivityMessageUpperLimit.TabIndex = 5;
            this.txtMyActivityMessageUpperLimit.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtMyActivityMessageUpperLimit.Validating += new System.ComponentModel.CancelEventHandler(this.txtMyActivityMessageUpperLimit_Validating);
            // 
            // gbReqSizeFiters
            // 
            this.gbReqSizeFiters.Controls.Add(this.txtSizeConstraints);
            this.gbReqSizeFiters.Controls.Add(this.txtSizeAlternates);
            this.gbReqSizeFiters.Controls.Add(this.txtSizeGroup);
            this.gbReqSizeFiters.Controls.Add(this.txtSizeCurve);
            this.gbReqSizeFiters.Controls.Add(this.lblCharMask2);
            this.gbReqSizeFiters.Controls.Add(this.lblCharMask1);
            this.gbReqSizeFiters.Controls.Add(this.cboSizeConstraints);
            this.gbReqSizeFiters.Controls.Add(this.cboSizeAlternates);
            this.gbReqSizeFiters.Controls.Add(this.cboSizeGroup);
            this.gbReqSizeFiters.Controls.Add(this.cboSizeCurve);
            this.gbReqSizeFiters.Location = new System.Drawing.Point(24, 216);
            this.gbReqSizeFiters.Name = "gbReqSizeFiters";
            this.gbReqSizeFiters.Size = new System.Drawing.Size(519, 112);
            this.gbReqSizeFiters.TabIndex = 20;
            this.gbReqSizeFiters.TabStop = false;
            this.gbReqSizeFiters.Text = "Require Size Drop Down Filters";
            // 
            // txtSizeConstraints
            // 
            this.txtSizeConstraints.Location = new System.Drawing.Point(354, 72);
            this.txtSizeConstraints.Name = "txtSizeConstraints";
            this.txtSizeConstraints.Size = new System.Drawing.Size(90, 20);
            this.txtSizeConstraints.TabIndex = 9;
            this.txtSizeConstraints.TextChanged += new System.EventHandler(this.maskTextBox_TextChanged);
            this.txtSizeConstraints.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtCharMask_KeyPress);
            this.txtSizeConstraints.MouseHover += new System.EventHandler(this.maskTextBox_MouseHover);
            // 
            // txtSizeAlternates
            // 
            this.txtSizeAlternates.Location = new System.Drawing.Point(354, 36);
            this.txtSizeAlternates.Name = "txtSizeAlternates";
            this.txtSizeAlternates.Size = new System.Drawing.Size(90, 20);
            this.txtSizeAlternates.TabIndex = 8;
            this.txtSizeAlternates.TextChanged += new System.EventHandler(this.maskTextBox_TextChanged);
            this.txtSizeAlternates.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtCharMask_KeyPress);
            this.txtSizeAlternates.MouseHover += new System.EventHandler(this.maskTextBox_MouseHover);
            // 
            // txtSizeGroup
            // 
            this.txtSizeGroup.Location = new System.Drawing.Point(121, 72);
            this.txtSizeGroup.Name = "txtSizeGroup";
            this.txtSizeGroup.Size = new System.Drawing.Size(90, 20);
            this.txtSizeGroup.TabIndex = 7;
            this.txtSizeGroup.TextChanged += new System.EventHandler(this.maskTextBox_TextChanged);
            this.txtSizeGroup.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtCharMask_KeyPress);
            this.txtSizeGroup.MouseHover += new System.EventHandler(this.maskTextBox_MouseHover);
            // 
            // txtSizeCurve
            // 
            this.txtSizeCurve.Location = new System.Drawing.Point(121, 36);
            this.txtSizeCurve.Name = "txtSizeCurve";
            this.txtSizeCurve.Size = new System.Drawing.Size(90, 20);
            this.txtSizeCurve.TabIndex = 6;
            this.txtSizeCurve.TextChanged += new System.EventHandler(this.maskTextBox_TextChanged);
            this.txtSizeCurve.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtCharMask_KeyPress);
            this.txtSizeCurve.MouseHover += new System.EventHandler(this.maskTextBox_MouseHover);
            // 
            // lblCharMask2
            // 
            this.lblCharMask2.Location = new System.Drawing.Point(356, 20);
            this.lblCharMask2.Name = "lblCharMask2";
            this.lblCharMask2.Size = new System.Drawing.Size(88, 16);
            this.lblCharMask2.TabIndex = 5;
            this.lblCharMask2.Text = "Character Mask";
            // 
            // lblCharMask1
            // 
            this.lblCharMask1.Location = new System.Drawing.Point(123, 20);
            this.lblCharMask1.Name = "lblCharMask1";
            this.lblCharMask1.Size = new System.Drawing.Size(88, 16);
            this.lblCharMask1.TabIndex = 4;
            this.lblCharMask1.Text = "Character Mask";
            // 
            // cboSizeConstraints
            // 
            this.cboSizeConstraints.AutoSize = true;
            this.cboSizeConstraints.Location = new System.Drawing.Point(232, 74);
            this.cboSizeConstraints.Name = "cboSizeConstraints";
            this.cboSizeConstraints.Size = new System.Drawing.Size(101, 17);
            this.cboSizeConstraints.TabIndex = 3;
            this.cboSizeConstraints.Text = "Size Constraints";
            this.cboSizeConstraints.CheckedChanged += new System.EventHandler(this.sizeFilterCheckBox_CheckedChanged);
            // 
            // cboSizeAlternates
            // 
            this.cboSizeAlternates.AutoSize = true;
            this.cboSizeAlternates.Location = new System.Drawing.Point(232, 38);
            this.cboSizeAlternates.Name = "cboSizeAlternates";
            this.cboSizeAlternates.Size = new System.Drawing.Size(96, 17);
            this.cboSizeAlternates.TabIndex = 2;
            this.cboSizeAlternates.Text = "Size Alternates";
            this.cboSizeAlternates.CheckedChanged += new System.EventHandler(this.sizeFilterCheckBox_CheckedChanged);
            // 
            // cboSizeGroup
            // 
            this.cboSizeGroup.AutoSize = true;
            this.cboSizeGroup.Location = new System.Drawing.Point(16, 74);
            this.cboSizeGroup.Name = "cboSizeGroup";
            this.cboSizeGroup.Size = new System.Drawing.Size(78, 17);
            this.cboSizeGroup.TabIndex = 1;
            this.cboSizeGroup.Text = "Size Group";
            this.cboSizeGroup.CheckedChanged += new System.EventHandler(this.sizeFilterCheckBox_CheckedChanged);
            // 
            // cboSizeCurve
            // 
            this.cboSizeCurve.AutoSize = true;
            this.cboSizeCurve.Location = new System.Drawing.Point(16, 38);
            this.cboSizeCurve.Name = "cboSizeCurve";
            this.cboSizeCurve.Size = new System.Drawing.Size(77, 17);
            this.cboSizeCurve.TabIndex = 0;
            this.cboSizeCurve.Text = "Size Curve";
            this.cboSizeCurve.CheckedChanged += new System.EventHandler(this.sizeFilterCheckBox_CheckedChanged);
            // 
            // lblNonCompPeriodWeeks
            // 
            this.lblNonCompPeriodWeeks.AutoSize = true;
            this.lblNonCompPeriodWeeks.Location = new System.Drawing.Point(343, 183);
            this.lblNonCompPeriodWeeks.Name = "lblNonCompPeriodWeeks";
            this.lblNonCompPeriodWeeks.Size = new System.Drawing.Size(38, 13);
            this.lblNonCompPeriodWeeks.TabIndex = 19;
            this.lblNonCompPeriodWeeks.Text = "weeks";
            this.lblNonCompPeriodWeeks.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblNewStorePeriodWeeks
            // 
            this.lblNewStorePeriodWeeks.AutoSize = true;
            this.lblNewStorePeriodWeeks.Location = new System.Drawing.Point(343, 152);
            this.lblNewStorePeriodWeeks.Name = "lblNewStorePeriodWeeks";
            this.lblNewStorePeriodWeeks.Size = new System.Drawing.Size(38, 13);
            this.lblNewStorePeriodWeeks.TabIndex = 18;
            this.lblNewStorePeriodWeeks.Text = "weeks";
            this.lblNewStorePeriodWeeks.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblNonCompPeriodEnd
            // 
            this.lblNonCompPeriodEnd.AutoSize = true;
            this.lblNonCompPeriodEnd.Location = new System.Drawing.Point(264, 183);
            this.lblNonCompPeriodEnd.Name = "lblNonCompPeriodEnd";
            this.lblNonCompPeriodEnd.Size = new System.Drawing.Size(26, 13);
            this.lblNonCompPeriodEnd.TabIndex = 17;
            this.lblNonCompPeriodEnd.Text = "End";
            this.lblNonCompPeriodEnd.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtNonCompPeriodEnd
            // 
            this.txtNonCompPeriodEnd.Location = new System.Drawing.Point(296, 179);
            this.txtNonCompPeriodEnd.MaxLength = 3;
            this.txtNonCompPeriodEnd.Name = "txtNonCompPeriodEnd";
            this.txtNonCompPeriodEnd.Size = new System.Drawing.Size(40, 20);
            this.txtNonCompPeriodEnd.TabIndex = 8;
            this.txtNonCompPeriodEnd.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtNonCompPeriodEnd.TextChanged += new System.EventHandler(this.txtNonCompPeriodEnd_TextChanged);
            this.txtNonCompPeriodEnd.Validating += new System.ComponentModel.CancelEventHandler(this.txtNewStore_Validating);
            // 
            // lblNewStorePeriodEnd
            // 
            this.lblNewStorePeriodEnd.AutoSize = true;
            this.lblNewStorePeriodEnd.Location = new System.Drawing.Point(264, 152);
            this.lblNewStorePeriodEnd.Name = "lblNewStorePeriodEnd";
            this.lblNewStorePeriodEnd.Size = new System.Drawing.Size(26, 13);
            this.lblNewStorePeriodEnd.TabIndex = 15;
            this.lblNewStorePeriodEnd.Text = "End";
            this.lblNewStorePeriodEnd.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtNewStorePeriodEnd
            // 
            this.txtNewStorePeriodEnd.Location = new System.Drawing.Point(296, 148);
            this.txtNewStorePeriodEnd.MaxLength = 3;
            this.txtNewStorePeriodEnd.Name = "txtNewStorePeriodEnd";
            this.txtNewStorePeriodEnd.Size = new System.Drawing.Size(40, 20);
            this.txtNewStorePeriodEnd.TabIndex = 6;
            this.txtNewStorePeriodEnd.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtNewStorePeriodEnd.TextChanged += new System.EventHandler(this.txtNewStorePeriodEnd_TextChanged);
            this.txtNewStorePeriodEnd.Validating += new System.ComponentModel.CancelEventHandler(this.txtNewStore_Validating);
            // 
            // lblNonCompPeriodBegin
            // 
            this.lblNonCompPeriodBegin.AutoSize = true;
            this.lblNonCompPeriodBegin.Location = new System.Drawing.Point(168, 183);
            this.lblNonCompPeriodBegin.Name = "lblNonCompPeriodBegin";
            this.lblNonCompPeriodBegin.Size = new System.Drawing.Size(34, 13);
            this.lblNonCompPeriodBegin.TabIndex = 13;
            this.lblNonCompPeriodBegin.Text = "Begin";
            this.lblNonCompPeriodBegin.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblNewStorePeriodBegin
            // 
            this.lblNewStorePeriodBegin.AutoSize = true;
            this.lblNewStorePeriodBegin.Location = new System.Drawing.Point(168, 152);
            this.lblNewStorePeriodBegin.Name = "lblNewStorePeriodBegin";
            this.lblNewStorePeriodBegin.Size = new System.Drawing.Size(34, 13);
            this.lblNewStorePeriodBegin.TabIndex = 12;
            this.lblNewStorePeriodBegin.Text = "Begin";
            this.lblNewStorePeriodBegin.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtNonCompPeriodBegin
            // 
            this.txtNonCompPeriodBegin.Location = new System.Drawing.Point(208, 179);
            this.txtNonCompPeriodBegin.MaxLength = 3;
            this.txtNonCompPeriodBegin.Name = "txtNonCompPeriodBegin";
            this.txtNonCompPeriodBegin.Size = new System.Drawing.Size(40, 20);
            this.txtNonCompPeriodBegin.TabIndex = 7;
            this.txtNonCompPeriodBegin.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtNonCompPeriodBegin.TextChanged += new System.EventHandler(this.txtNonCompPeriodBegin_TextChanged);
            this.txtNonCompPeriodBegin.Validating += new System.ComponentModel.CancelEventHandler(this.txtNewStore_Validating);
            // 
            // txtNewStorePeriodBegin
            // 
            this.txtNewStorePeriodBegin.Location = new System.Drawing.Point(208, 148);
            this.txtNewStorePeriodBegin.MaxLength = 3;
            this.txtNewStorePeriodBegin.Name = "txtNewStorePeriodBegin";
            this.txtNewStorePeriodBegin.Size = new System.Drawing.Size(40, 20);
            this.txtNewStorePeriodBegin.TabIndex = 5;
            this.txtNewStorePeriodBegin.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtNewStorePeriodBegin.TextChanged += new System.EventHandler(this.txtNewStorePeriodBegin_TextChanged);
            this.txtNewStorePeriodBegin.Validating += new System.ComponentModel.CancelEventHandler(this.txtNewStore_Validating);
            // 
            // cboAllocStoreAttr
            // 
            this.cboAllocStoreAttr.AllowDrop = true;
            this.cboAllocStoreAttr.AllowUserAttributes = false;
            this.cboAllocStoreAttr.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboAllocStoreAttr.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboAllocStoreAttr.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboAllocStoreAttr.ItemHeight = 13;
            this.cboAllocStoreAttr.Location = new System.Drawing.Point(168, 112);
            this.cboAllocStoreAttr.Name = "cboAllocStoreAttr";
            this.cboAllocStoreAttr.Size = new System.Drawing.Size(176, 21);
            this.cboAllocStoreAttr.TabIndex = 4;
            this.cboAllocStoreAttr.MIDComboBoxPropertiesChangedEvent += new MIDRetail.Windows.Controls.MIDComboBoxPropertiesChangedEventHandler(this.cboAllocStoreAttr_MIDComboBoxPropertiesChangedEvent);
            this.cboAllocStoreAttr.SelectionChangeCommitted += new System.EventHandler(this.cboAllocStoreAttr_SelectionChangeCommitted);
            this.cboAllocStoreAttr.DragEnter += new System.Windows.Forms.DragEventHandler(this.cboStoreAttr_DragEnter);
            this.cboAllocStoreAttr.DragOver += new System.Windows.Forms.DragEventHandler(this.cboStoreAttr_DragOver);
            // 
            // cboPlanStoreAttr
            // 
            this.cboPlanStoreAttr.AllowDrop = true;
            this.cboPlanStoreAttr.AllowUserAttributes = false;
            this.cboPlanStoreAttr.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboPlanStoreAttr.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboPlanStoreAttr.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboPlanStoreAttr.ItemHeight = 13;
            this.cboPlanStoreAttr.Location = new System.Drawing.Point(168, 80);
            this.cboPlanStoreAttr.Name = "cboPlanStoreAttr";
            this.cboPlanStoreAttr.Size = new System.Drawing.Size(176, 21);
            this.cboPlanStoreAttr.TabIndex = 3;
            this.cboPlanStoreAttr.MIDComboBoxPropertiesChangedEvent += new MIDRetail.Windows.Controls.MIDComboBoxPropertiesChangedEventHandler(this.cboPlanStoreAttr_MIDComboBoxPropertiesChangedEvent);
            this.cboPlanStoreAttr.SelectionChangeCommitted += new System.EventHandler(this.cboPlanStoreAttr_SelectionChangeCommitted);
            this.cboPlanStoreAttr.DragEnter += new System.Windows.Forms.DragEventHandler(this.cboStoreAttr_DragEnter);
            this.cboPlanStoreAttr.DragOver += new System.Windows.Forms.DragEventHandler(this.cboStoreAttr_DragOver);
            // 
            // cboStore
            // 
            this.cboStore.AutoAdjust = true;
            this.cboStore.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboStore.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboStore.DataSource = null;
            this.cboStore.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboStore.DropDownWidth = 176;
            this.cboStore.FormattingEnabled = false;
            this.cboStore.IgnoreFocusLost = false;
            this.cboStore.ItemHeight = 13;
            this.cboStore.Location = new System.Drawing.Point(168, 48);
            this.cboStore.Margin = new System.Windows.Forms.Padding(0);
            this.cboStore.MaxDropDownItems = 25;
            this.cboStore.Name = "cboStore";
            this.cboStore.SetToolTip = "";
            this.cboStore.Size = new System.Drawing.Size(176, 21);
            this.cboStore.TabIndex = 2;
            this.cboStore.Tag = null;
            this.cboStore.MIDComboBoxPropertiesChangedEvent += new MIDRetail.Windows.Controls.MIDComboBoxPropertiesChangedEventHandler(this.cboStore_MIDComboBoxPropertiesChangedEvent);
            this.cboStore.SelectionChangeCommitted += new System.EventHandler(this.cboStore_SelectionChangeCommitted);
            // 
            // cboProduct
            // 
            this.cboProduct.AutoAdjust = true;
            this.cboProduct.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboProduct.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboProduct.DataSource = null;
            this.cboProduct.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboProduct.DropDownWidth = 176;
            this.cboProduct.FormattingEnabled = false;
            this.cboProduct.IgnoreFocusLost = false;
            this.cboProduct.ItemHeight = 13;
            this.cboProduct.Location = new System.Drawing.Point(168, 16);
            this.cboProduct.Margin = new System.Windows.Forms.Padding(0);
            this.cboProduct.MaxDropDownItems = 25;
            this.cboProduct.Name = "cboProduct";
            this.cboProduct.SetToolTip = "";
            this.cboProduct.Size = new System.Drawing.Size(176, 21);
            this.cboProduct.TabIndex = 1;
            this.cboProduct.Tag = null;
            this.cboProduct.MIDComboBoxPropertiesChangedEvent += new MIDRetail.Windows.Controls.MIDComboBoxPropertiesChangedEventHandler(this.cboProduct_MIDComboBoxPropertiesChangedEvent);
            this.cboProduct.SelectionChangeCommitted += new System.EventHandler(this.cboProduct_SelectionChangeCommitted);
            // 
            // lblNonCompPeriod
            // 
            this.lblNonCompPeriod.AutoSize = true;
            this.lblNonCompPeriod.Location = new System.Drawing.Point(32, 183);
            this.lblNonCompPeriod.Name = "lblNonCompPeriod";
            this.lblNonCompPeriod.Size = new System.Drawing.Size(118, 13);
            this.lblNonCompPeriod.TabIndex = 5;
            this.lblNonCompPeriod.Text = "Non-Comp Store Period";
            this.lblNonCompPeriod.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblNewStorePeriod
            // 
            this.lblNewStorePeriod.AutoSize = true;
            this.lblNewStorePeriod.Location = new System.Drawing.Point(32, 152);
            this.lblNewStorePeriod.Name = "lblNewStorePeriod";
            this.lblNewStorePeriod.Size = new System.Drawing.Size(90, 13);
            this.lblNewStorePeriod.TabIndex = 4;
            this.lblNewStorePeriod.Text = "New Store Period";
            this.lblNewStorePeriod.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblAllocStoreAttr
            // 
            this.lblAllocStoreAttr.AutoSize = true;
            this.lblAllocStoreAttr.Location = new System.Drawing.Point(42, 116);
            this.lblAllocStoreAttr.Name = "lblAllocStoreAttr";
            this.lblAllocStoreAttr.Size = new System.Drawing.Size(123, 13);
            this.lblAllocStoreAttr.TabIndex = 3;
            this.lblAllocStoreAttr.Text = "Allocation Store Attribute";
            this.lblAllocStoreAttr.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblPlanStoreAttr
            // 
            this.lblPlanStoreAttr.AutoSize = true;
            this.lblPlanStoreAttr.Location = new System.Drawing.Point(66, 84);
            this.lblPlanStoreAttr.Name = "lblPlanStoreAttr";
            this.lblPlanStoreAttr.Size = new System.Drawing.Size(99, 13);
            this.lblPlanStoreAttr.TabIndex = 2;
            this.lblPlanStoreAttr.Text = "OTS Store Attribute";
            this.lblPlanStoreAttr.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblStore
            // 
            this.lblStore.AutoSize = true;
            this.lblStore.Location = new System.Drawing.Point(96, 52);
            this.lblStore.Name = "lblStore";
            this.lblStore.Size = new System.Drawing.Size(69, 13);
            this.lblStore.TabIndex = 1;
            this.lblStore.Text = "Store Display";
            this.lblStore.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblProduct
            // 
            this.lblProduct.Location = new System.Drawing.Point(65, 15);
            this.lblProduct.Name = "lblProduct";
            this.lblProduct.Size = new System.Drawing.Size(100, 23);
            this.lblProduct.TabIndex = 0;
            this.lblProduct.Text = "Product Display";
            this.lblProduct.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tabAllocDefault
            // 
            this.tabAllocDefault.Controls.Add(this.chkPriorHeaderIncludeReserve);
            this.tabAllocDefault.Controls.Add(this.gboDCCartonAttribute);
            this.tabAllocDefault.Controls.Add(this.gboItemFWOS);
            this.tabAllocDefault.Controls.Add(this.cbxEnableVelocityGradeOptions);
            this.tabAllocDefault.Controls.Add(this.gboItemMaxOverride);
            this.tabAllocDefault.Controls.Add(this.gbxGenericPackRounding);
            this.tabAllocDefault.Controls.Add(this.lblShippingHorizon);
            this.tabAllocDefault.Controls.Add(this.lblShippingHorizonWeeks);
            this.tabAllocDefault.Controls.Add(this.txtShippingHorizonWeeks);
            this.tabAllocDefault.Controls.Add(this.gboSizeOptions);
            this.tabAllocDefault.Controls.Add(this.lblSGPeriodWeeks);
            this.tabAllocDefault.Controls.Add(this.lblPctBalTolerance);
            this.tabAllocDefault.Controls.Add(this.lblPctNeedLimitPct);
            this.tabAllocDefault.Controls.Add(this.lblSGPeriod);
            this.tabAllocDefault.Controls.Add(this.txtSGPeriod);
            this.tabAllocDefault.Controls.Add(this.lblBalTolerance);
            this.tabAllocDefault.Controls.Add(this.txtPctBalTolerance);
            this.tabAllocDefault.Controls.Add(this.lblNeedLimit);
            this.tabAllocDefault.Controls.Add(this.txtPctNeedLimit);
            this.tabAllocDefault.Location = new System.Drawing.Point(4, 22);
            this.tabAllocDefault.Name = "tabAllocDefault";
            this.tabAllocDefault.Size = new System.Drawing.Size(647, 416);
            this.tabAllocDefault.TabIndex = 4;
            this.tabAllocDefault.Text = "Allocation Defaults";
            this.tabAllocDefault.UseVisualStyleBackColor = true;
            this.tabAllocDefault.Visible = false;
            // 
            // chkPriorHeaderIncludeReserve
            // 
            this.chkPriorHeaderIncludeReserve.AutoSize = true;
            this.chkPriorHeaderIncludeReserve.Location = new System.Drawing.Point(233, 343);
            this.chkPriorHeaderIncludeReserve.Name = "chkPriorHeaderIncludeReserve";
            this.chkPriorHeaderIncludeReserve.Size = new System.Drawing.Size(172, 17);
            this.chkPriorHeaderIncludeReserve.TabIndex = 41;
            this.chkPriorHeaderIncludeReserve.Text = "Prior Header - Include Reserve";
            this.chkPriorHeaderIncludeReserve.UseVisualStyleBackColor = true;
            this.chkPriorHeaderIncludeReserve.CheckedChanged += new System.EventHandler(this.chkPriorHeaderIncludeReserve_CheckedChanged);
            // 
            // gboDCCartonAttribute
            // 
            this.gboDCCartonAttribute.Controls.Add(this.cboDCCartonRoundDfltAttribute);
            this.gboDCCartonAttribute.Location = new System.Drawing.Point(427, 94);
            this.gboDCCartonAttribute.Name = "gboDCCartonAttribute";
            this.gboDCCartonAttribute.Size = new System.Drawing.Size(204, 50);
            this.gboDCCartonAttribute.TabIndex = 42;
            this.gboDCCartonAttribute.TabStop = false;
            this.gboDCCartonAttribute.Text = "DC Carton Rounding Default Attribute";
            // 
            // cboDCCartonRoundDfltAttribute
            // 
            this.cboDCCartonRoundDfltAttribute.AllowDrop = true;
            this.cboDCCartonRoundDfltAttribute.AllowUserAttributes = false;
            this.cboDCCartonRoundDfltAttribute.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboDCCartonRoundDfltAttribute.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboDCCartonRoundDfltAttribute.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboDCCartonRoundDfltAttribute.ItemHeight = 13;
            this.cboDCCartonRoundDfltAttribute.Location = new System.Drawing.Point(14, 18);
            this.cboDCCartonRoundDfltAttribute.Name = "cboDCCartonRoundDfltAttribute";
            this.cboDCCartonRoundDfltAttribute.Size = new System.Drawing.Size(176, 21);
            this.cboDCCartonRoundDfltAttribute.TabIndex = 5;
            this.cboDCCartonRoundDfltAttribute.MIDComboBoxPropertiesChangedEvent += new MIDRetail.Windows.Controls.MIDComboBoxPropertiesChangedEventHandler(this.cboDCCartonRoundDfltAttribute_MIDComboBoxPropertiesChangedEvent);
            this.cboDCCartonRoundDfltAttribute.SelectionChangeCommitted += new System.EventHandler(this.cboDCCartonRoundDfltAttribute_SelectionChangeCommitted);
            // 
            // gboItemFWOS
            // 
            this.gboItemFWOS.Controls.Add(this.radItemFWOSLowest);
            this.gboItemFWOS.Controls.Add(this.radItemFWOSHighest);
            this.gboItemFWOS.Controls.Add(this.radItemFWOSDefault);
            this.gboItemFWOS.Location = new System.Drawing.Point(441, 48);
            this.gboItemFWOS.Name = "gboItemFWOS";
            this.gboItemFWOS.Size = new System.Drawing.Size(184, 37);
            this.gboItemFWOS.TabIndex = 40;
            this.gboItemFWOS.TabStop = false;
            this.gboItemFWOS.Text = "Item/FWOS Max:";
            // 
            // radItemFWOSLowest
            // 
            this.radItemFWOSLowest.AutoSize = true;
            this.radItemFWOSLowest.Location = new System.Drawing.Point(123, 14);
            this.radItemFWOSLowest.Name = "radItemFWOSLowest";
            this.radItemFWOSLowest.Size = new System.Drawing.Size(62, 17);
            this.radItemFWOSLowest.TabIndex = 2;
            this.radItemFWOSLowest.TabStop = true;
            this.radItemFWOSLowest.Text = "Lowest:";
            this.radItemFWOSLowest.UseVisualStyleBackColor = true;
            this.radItemFWOSLowest.CheckedChanged += new System.EventHandler(this.radItemFWOSLowest_CheckedChanged);
            // 
            // radItemFWOSHighest
            // 
            this.radItemFWOSHighest.AutoSize = true;
            this.radItemFWOSHighest.Location = new System.Drawing.Point(64, 14);
            this.radItemFWOSHighest.Name = "radItemFWOSHighest";
            this.radItemFWOSHighest.Size = new System.Drawing.Size(64, 17);
            this.radItemFWOSHighest.TabIndex = 1;
            this.radItemFWOSHighest.TabStop = true;
            this.radItemFWOSHighest.Text = "Highest:";
            this.radItemFWOSHighest.UseVisualStyleBackColor = true;
            this.radItemFWOSHighest.CheckedChanged += new System.EventHandler(this.radItemFWOSHighest_CheckedChanged);
            // 
            // radItemFWOSDefault
            // 
            this.radItemFWOSDefault.AutoSize = true;
            this.radItemFWOSDefault.Location = new System.Drawing.Point(6, 14);
            this.radItemFWOSDefault.Name = "radItemFWOSDefault";
            this.radItemFWOSDefault.Size = new System.Drawing.Size(62, 17);
            this.radItemFWOSDefault.TabIndex = 0;
            this.radItemFWOSDefault.TabStop = true;
            this.radItemFWOSDefault.Text = "Default:";
            this.radItemFWOSDefault.UseVisualStyleBackColor = true;
            this.radItemFWOSDefault.CheckedChanged += new System.EventHandler(this.radItemFWOSDefault_CheckedChanged);
            // 
            // cbxEnableVelocityGradeOptions
            // 
            this.cbxEnableVelocityGradeOptions.AutoSize = true;
            this.cbxEnableVelocityGradeOptions.Location = new System.Drawing.Point(18, 344);
            this.cbxEnableVelocityGradeOptions.Name = "cbxEnableVelocityGradeOptions";
            this.cbxEnableVelocityGradeOptions.Size = new System.Drawing.Size(170, 17);
            this.cbxEnableVelocityGradeOptions.TabIndex = 39;
            this.cbxEnableVelocityGradeOptions.Text = "Enable Velocity Grade Options";
            this.cbxEnableVelocityGradeOptions.UseVisualStyleBackColor = true;
            // 
            // gboItemMaxOverride
            // 
            this.gboItemMaxOverride.Controls.Add(this.radItemMaxOverrideNo);
            this.gboItemMaxOverride.Controls.Add(this.radItemMaxOverrideYes);
            this.gboItemMaxOverride.Location = new System.Drawing.Point(441, 3);
            this.gboItemMaxOverride.Name = "gboItemMaxOverride";
            this.gboItemMaxOverride.Size = new System.Drawing.Size(110, 37);
            this.gboItemMaxOverride.TabIndex = 30;
            this.gboItemMaxOverride.TabStop = false;
            this.gboItemMaxOverride.Text = "Item Max Override:";
            this.gboItemMaxOverride.Visible = false;
            // 
            // radItemMaxOverrideNo
            // 
            this.radItemMaxOverrideNo.AutoSize = true;
            this.radItemMaxOverrideNo.Location = new System.Drawing.Point(62, 14);
            this.radItemMaxOverrideNo.Name = "radItemMaxOverrideNo";
            this.radItemMaxOverrideNo.Size = new System.Drawing.Size(42, 17);
            this.radItemMaxOverrideNo.TabIndex = 1;
            this.radItemMaxOverrideNo.TabStop = true;
            this.radItemMaxOverrideNo.Text = "No:";
            this.radItemMaxOverrideNo.UseVisualStyleBackColor = true;
            this.radItemMaxOverrideNo.CheckedChanged += new System.EventHandler(this.radItemMaxOverrideNo_CheckedChanged);
            // 
            // radItemMaxOverrideYes
            // 
            this.radItemMaxOverrideYes.AutoSize = true;
            this.radItemMaxOverrideYes.Location = new System.Drawing.Point(6, 14);
            this.radItemMaxOverrideYes.Name = "radItemMaxOverrideYes";
            this.radItemMaxOverrideYes.Size = new System.Drawing.Size(46, 17);
            this.radItemMaxOverrideYes.TabIndex = 0;
            this.radItemMaxOverrideYes.TabStop = true;
            this.radItemMaxOverrideYes.Text = "Yes:";
            this.radItemMaxOverrideYes.UseVisualStyleBackColor = true;
            this.radItemMaxOverrideYes.CheckedChanged += new System.EventHandler(this.radItemMaxOverrideYes_CheckedChanged);
            // 
            // gbxGenericPackRounding
            // 
            this.gbxGenericPackRounding.Controls.Add(this.lblPCtNthPackRoundUpFromPct);
            this.gbxGenericPackRounding.Controls.Add(this.lblPct1stPackRoundUpFromPct);
            this.gbxGenericPackRounding.Controls.Add(this.txtPctNthPackRoundUpFrom);
            this.gbxGenericPackRounding.Controls.Add(this.txtPct1stPackRoundUpFrom);
            this.gbxGenericPackRounding.Controls.Add(this.lblPctNthPackRoundUpFrom);
            this.gbxGenericPackRounding.Controls.Add(this.lblPct1stPackRoundUpFrom);
            this.gbxGenericPackRounding.Location = new System.Drawing.Point(6, 94);
            this.gbxGenericPackRounding.Name = "gbxGenericPackRounding";
            this.gbxGenericPackRounding.Size = new System.Drawing.Size(418, 50);
            this.gbxGenericPackRounding.TabIndex = 29;
            this.gbxGenericPackRounding.TabStop = false;
            this.gbxGenericPackRounding.Text = "Generic Pack Rounding:";
            // 
            // lblPCtNthPackRoundUpFromPct
            // 
            this.lblPCtNthPackRoundUpFromPct.Location = new System.Drawing.Point(395, 18);
            this.lblPCtNthPackRoundUpFromPct.Name = "lblPCtNthPackRoundUpFromPct";
            this.lblPCtNthPackRoundUpFromPct.Size = new System.Drawing.Size(10, 23);
            this.lblPCtNthPackRoundUpFromPct.TabIndex = 0;
            this.lblPCtNthPackRoundUpFromPct.Text = "%";
            this.lblPCtNthPackRoundUpFromPct.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblPct1stPackRoundUpFromPct
            // 
            this.lblPct1stPackRoundUpFromPct.Location = new System.Drawing.Point(188, 18);
            this.lblPct1stPackRoundUpFromPct.Name = "lblPct1stPackRoundUpFromPct";
            this.lblPct1stPackRoundUpFromPct.Size = new System.Drawing.Size(10, 23);
            this.lblPct1stPackRoundUpFromPct.TabIndex = 0;
            this.lblPct1stPackRoundUpFromPct.Text = "%";
            this.lblPct1stPackRoundUpFromPct.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblPct1stPackRoundUpFromPct.Click += new System.EventHandler(this.lblPct1stPackRoundUpFromPct_Click);
            // 
            // txtPctNthPackRoundUpFrom
            // 
            this.txtPctNthPackRoundUpFrom.Location = new System.Drawing.Point(341, 18);
            this.txtPctNthPackRoundUpFrom.Name = "txtPctNthPackRoundUpFrom";
            this.txtPctNthPackRoundUpFrom.Size = new System.Drawing.Size(48, 20);
            this.txtPctNthPackRoundUpFrom.TabIndex = 0;
            this.txtPctNthPackRoundUpFrom.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtPctNthPackRoundUpFrom.TextChanged += new System.EventHandler(this.textBox_TextChanged);
            this.txtPctNthPackRoundUpFrom.Validating += new System.ComponentModel.CancelEventHandler(this.txtZeroToOneHundredPercent_Validating);
            // 
            // txtPct1stPackRoundUpFrom
            // 
            this.txtPct1stPackRoundUpFrom.Location = new System.Drawing.Point(136, 21);
            this.txtPct1stPackRoundUpFrom.Name = "txtPct1stPackRoundUpFrom";
            this.txtPct1stPackRoundUpFrom.Size = new System.Drawing.Size(48, 20);
            this.txtPct1stPackRoundUpFrom.TabIndex = 0;
            this.txtPct1stPackRoundUpFrom.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.toolTip1.SetToolTip(this.txtPct1stPackRoundUpFrom, "Percent to round up from 1st Pack");
            this.txtPct1stPackRoundUpFrom.TextChanged += new System.EventHandler(this.textBox_TextChanged);
            this.txtPct1stPackRoundUpFrom.Validating += new System.ComponentModel.CancelEventHandler(this.txtZeroToOneHundredPercent_Validating);
            // 
            // lblPctNthPackRoundUpFrom
            // 
            this.lblPctNthPackRoundUpFrom.AutoSize = true;
            this.lblPctNthPackRoundUpFrom.Location = new System.Drawing.Point(206, 23);
            this.lblPctNthPackRoundUpFrom.Name = "lblPctNthPackRoundUpFrom";
            this.lblPctNthPackRoundUpFrom.Size = new System.Drawing.Size(134, 13);
            this.lblPctNthPackRoundUpFrom.TabIndex = 0;
            this.lblPctNthPackRoundUpFrom.Text = "Nth Pack - Round up from:";
            this.lblPctNthPackRoundUpFrom.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblPctNthPackRoundUpFrom.Click += new System.EventHandler(this.lblPctNthPackRoundUpFrom_Click);
            // 
            // lblPct1stPackRoundUpFrom
            // 
            this.lblPct1stPackRoundUpFrom.AutoSize = true;
            this.lblPct1stPackRoundUpFrom.Location = new System.Drawing.Point(4, 25);
            this.lblPct1stPackRoundUpFrom.Name = "lblPct1stPackRoundUpFrom";
            this.lblPct1stPackRoundUpFrom.Size = new System.Drawing.Size(131, 13);
            this.lblPct1stPackRoundUpFrom.TabIndex = 0;
            this.lblPct1stPackRoundUpFrom.Text = "1st Pack - Round up from:";
            this.lblPct1stPackRoundUpFrom.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblPct1stPackRoundUpFrom.Click += new System.EventHandler(this.lblPct1stPackRoundUpFrom_Click);
            // 
            // lblShippingHorizon
            // 
            this.lblShippingHorizon.AutoSize = true;
            this.lblShippingHorizon.Location = new System.Drawing.Point(248, 56);
            this.lblShippingHorizon.Name = "lblShippingHorizon";
            this.lblShippingHorizon.Size = new System.Drawing.Size(87, 13);
            this.lblShippingHorizon.TabIndex = 28;
            this.lblShippingHorizon.Text = "Shipping Horizon";
            this.lblShippingHorizon.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblShippingHorizonWeeks
            // 
            this.lblShippingHorizonWeeks.AutoSize = true;
            this.lblShippingHorizonWeeks.Location = new System.Drawing.Point(394, 56);
            this.lblShippingHorizonWeeks.Name = "lblShippingHorizonWeeks";
            this.lblShippingHorizonWeeks.Size = new System.Drawing.Size(38, 13);
            this.lblShippingHorizonWeeks.TabIndex = 27;
            this.lblShippingHorizonWeeks.Text = "weeks";
            this.lblShippingHorizonWeeks.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtShippingHorizonWeeks
            // 
            this.txtShippingHorizonWeeks.Location = new System.Drawing.Point(338, 52);
            this.txtShippingHorizonWeeks.MaxLength = 3;
            this.txtShippingHorizonWeeks.Name = "txtShippingHorizonWeeks";
            this.txtShippingHorizonWeeks.Size = new System.Drawing.Size(48, 20);
            this.txtShippingHorizonWeeks.TabIndex = 3;
            this.txtShippingHorizonWeeks.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtShippingHorizonWeeks.TextChanged += new System.EventHandler(this.textBox_TextChanged);
            this.txtShippingHorizonWeeks.Validating += new System.ComponentModel.CancelEventHandler(this.txtPositiveInteger_Validating);
            // 
            // gboSizeOptions
            // 
            this.gboSizeOptions.Controls.Add(this.cboVSWSizeContraints);
            this.gboSizeOptions.Controls.Add(this.lbl_VSWSizwConstraints);
            this.gboSizeOptions.Controls.Add(this.cbxNoMaxStep);
            this.gboSizeOptions.Controls.Add(this.cbxStepped);
            this.gboSizeOptions.Controls.Add(this.cboGenerateSizeCurveUsing);
            this.gboSizeOptions.Controls.Add(this.lblGenSizeCurveUsing);
            this.gboSizeOptions.Controls.Add(this.gboGenericSizeCurveName);
            this.gboSizeOptions.Controls.Add(this.gboFillSizesTo);
            this.gboSizeOptions.Controls.Add(this.gboNormalizeSizeCurves);
            this.gboSizeOptions.Controls.Add(this.lblFillSizeHoles);
            this.gboSizeOptions.Controls.Add(this.lblFillSizeHolesPct);
            this.gboSizeOptions.Controls.Add(this.txtFillSizeHoles);
            this.gboSizeOptions.Controls.Add(this.lblPackDevTolerancePct);
            this.gboSizeOptions.Controls.Add(this.txtPackDevTolerance);
            this.gboSizeOptions.Controls.Add(this.lblPackDevTolerance);
            this.gboSizeOptions.Controls.Add(this.lblPackNeedTolerance);
            this.gboSizeOptions.Controls.Add(this.txtPackNeedTolerance);
            this.gboSizeOptions.Location = new System.Drawing.Point(6, 149);
            this.gboSizeOptions.Name = "gboSizeOptions";
            this.gboSizeOptions.Size = new System.Drawing.Size(625, 190);
            this.gboSizeOptions.TabIndex = 4;
            this.gboSizeOptions.TabStop = false;
            this.gboSizeOptions.Text = "Size Options";
            this.gboSizeOptions.Enter += new System.EventHandler(this.gboSizeOptions_Enter);
            // 
            // cboVSWSizeContraints
            // 
            this.cboVSWSizeContraints.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboVSWSizeContraints.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboVSWSizeContraints.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboVSWSizeContraints.FormattingEnabled = true;
            this.cboVSWSizeContraints.Location = new System.Drawing.Point(440, 162);
            this.cboVSWSizeContraints.Name = "cboVSWSizeContraints";
            this.cboVSWSizeContraints.Size = new System.Drawing.Size(167, 21);
            this.cboVSWSizeContraints.TabIndex = 40;
            // 
            // lbl_VSWSizwConstraints
            // 
            this.lbl_VSWSizwConstraints.AutoSize = true;
            this.lbl_VSWSizwConstraints.Location = new System.Drawing.Point(326, 166);
            this.lbl_VSWSizwConstraints.Name = "lbl_VSWSizwConstraints";
            this.lbl_VSWSizwConstraints.Size = new System.Drawing.Size(113, 13);
            this.lbl_VSWSizwConstraints.TabIndex = 39;
            this.lbl_VSWSizwConstraints.Text = "VSW Size Constraints:";
            this.lbl_VSWSizwConstraints.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbxNoMaxStep
            // 
            this.cbxNoMaxStep.AutoSize = true;
            this.cbxNoMaxStep.Location = new System.Drawing.Point(434, 48);
            this.cbxNoMaxStep.Name = "cbxNoMaxStep";
            this.cbxNoMaxStep.Size = new System.Drawing.Size(88, 17);
            this.cbxNoMaxStep.TabIndex = 38;
            this.cbxNoMaxStep.Text = "No-Max Step";
            this.cbxNoMaxStep.UseVisualStyleBackColor = true;
            // 
            // cbxStepped
            // 
            this.cbxStepped.AutoSize = true;
            this.cbxStepped.Location = new System.Drawing.Point(356, 48);
            this.cbxStepped.Name = "cbxStepped";
            this.cbxStepped.Size = new System.Drawing.Size(66, 17);
            this.cbxStepped.TabIndex = 37;
            this.cbxStepped.Text = "Stepped";
            this.cbxStepped.UseVisualStyleBackColor = true;
            // 
            // cboGenerateSizeCurveUsing
            // 
            this.cboGenerateSizeCurveUsing.AutoAdjust = true;
            this.cboGenerateSizeCurveUsing.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboGenerateSizeCurveUsing.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboGenerateSizeCurveUsing.DataSource = null;
            this.cboGenerateSizeCurveUsing.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboGenerateSizeCurveUsing.DropDownWidth = 155;
            this.cboGenerateSizeCurveUsing.FormattingEnabled = true;
            this.cboGenerateSizeCurveUsing.IgnoreFocusLost = false;
            this.cboGenerateSizeCurveUsing.ItemHeight = 13;
            this.cboGenerateSizeCurveUsing.Location = new System.Drawing.Point(162, 162);
            this.cboGenerateSizeCurveUsing.Margin = new System.Windows.Forms.Padding(0);
            this.cboGenerateSizeCurveUsing.MaxDropDownItems = 25;
            this.cboGenerateSizeCurveUsing.Name = "cboGenerateSizeCurveUsing";
            this.cboGenerateSizeCurveUsing.SetToolTip = "";
            this.cboGenerateSizeCurveUsing.Size = new System.Drawing.Size(155, 21);
            this.cboGenerateSizeCurveUsing.TabIndex = 28;
            this.cboGenerateSizeCurveUsing.Tag = null;
            this.cboGenerateSizeCurveUsing.MIDComboBoxPropertiesChangedEvent += new MIDRetail.Windows.Controls.MIDComboBoxPropertiesChangedEventHandler(this.cboGenerateSizeCurveUsing_MIDComboBoxPropertiesChangedEvent);
            this.cboGenerateSizeCurveUsing.SelectionChangeCommitted += new System.EventHandler(this.cboGenerateSizeCurveUsing_SelectionChangeCommitted);
            // 
            // lblGenSizeCurveUsing
            // 
            this.lblGenSizeCurveUsing.AutoSize = true;
            this.lblGenSizeCurveUsing.Location = new System.Drawing.Point(16, 166);
            this.lblGenSizeCurveUsing.Name = "lblGenSizeCurveUsing";
            this.lblGenSizeCurveUsing.Size = new System.Drawing.Size(140, 13);
            this.lblGenSizeCurveUsing.TabIndex = 27;
            this.lblGenSizeCurveUsing.Text = "Generate Size Curves Using";
            this.lblGenSizeCurveUsing.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // gboGenericSizeCurveName
            // 
            this.gboGenericSizeCurveName.Controls.Add(this.radGenericSizeCurveName_HeaderCharacteristics);
            this.gboGenericSizeCurveName.Controls.Add(this.radGenericSizeCurveName_NodePropertiesName);
            this.gboGenericSizeCurveName.Controls.Add(this.lblGenericSizeCurveNameUsing);
            this.gboGenericSizeCurveName.Location = new System.Drawing.Point(8, 111);
            this.gboGenericSizeCurveName.Name = "gboGenericSizeCurveName";
            this.gboGenericSizeCurveName.Size = new System.Drawing.Size(520, 40);
            this.gboGenericSizeCurveName.TabIndex = 26;
            this.gboGenericSizeCurveName.TabStop = false;
            this.gboGenericSizeCurveName.Text = "Generic Size Curve Name";
            // 
            // radGenericSizeCurveName_HeaderCharacteristics
            // 
            this.radGenericSizeCurveName_HeaderCharacteristics.AutoSize = true;
            this.radGenericSizeCurveName_HeaderCharacteristics.Location = new System.Drawing.Point(207, 17);
            this.radGenericSizeCurveName_HeaderCharacteristics.Name = "radGenericSizeCurveName_HeaderCharacteristics";
            this.radGenericSizeCurveName_HeaderCharacteristics.Size = new System.Drawing.Size(132, 17);
            this.radGenericSizeCurveName_HeaderCharacteristics.TabIndex = 2;
            this.radGenericSizeCurveName_HeaderCharacteristics.TabStop = true;
            this.radGenericSizeCurveName_HeaderCharacteristics.Text = "Header Characteristics";
            this.radGenericSizeCurveName_HeaderCharacteristics.UseVisualStyleBackColor = true;
            // 
            // radGenericSizeCurveName_NodePropertiesName
            // 
            this.radGenericSizeCurveName_NodePropertiesName.AutoSize = true;
            this.radGenericSizeCurveName_NodePropertiesName.Checked = true;
            this.radGenericSizeCurveName_NodePropertiesName.Location = new System.Drawing.Point(60, 17);
            this.radGenericSizeCurveName_NodePropertiesName.Name = "radGenericSizeCurveName_NodePropertiesName";
            this.radGenericSizeCurveName_NodePropertiesName.Size = new System.Drawing.Size(132, 17);
            this.radGenericSizeCurveName_NodePropertiesName.TabIndex = 1;
            this.radGenericSizeCurveName_NodePropertiesName.TabStop = true;
            this.radGenericSizeCurveName_NodePropertiesName.Text = "Node Properties Name";
            this.radGenericSizeCurveName_NodePropertiesName.UseVisualStyleBackColor = true;
            // 
            // lblGenericSizeCurveNameUsing
            // 
            this.lblGenericSizeCurveNameUsing.AutoSize = true;
            this.lblGenericSizeCurveNameUsing.Location = new System.Drawing.Point(25, 19);
            this.lblGenericSizeCurveNameUsing.Name = "lblGenericSizeCurveNameUsing";
            this.lblGenericSizeCurveNameUsing.Size = new System.Drawing.Size(29, 13);
            this.lblGenericSizeCurveNameUsing.TabIndex = 0;
            this.lblGenericSizeCurveNameUsing.Text = "Use:";
            // 
            // gboFillSizesTo
            // 
            this.gboFillSizesTo.Controls.Add(this.radFillSizesTo_SizePlanWithSizeMins);
            this.gboFillSizesTo.Controls.Add(this.radFillSizesTo_SizePlan);
            this.gboFillSizesTo.Controls.Add(this.radFillSizesTo_Holes);
            this.gboFillSizesTo.Controls.Add(this.lblFillSizesTo);
            this.gboFillSizesTo.Location = new System.Drawing.Point(11, 64);
            this.gboFillSizesTo.Name = "gboFillSizesTo";
            this.gboFillSizesTo.Size = new System.Drawing.Size(323, 40);
            this.gboFillSizesTo.TabIndex = 25;
            this.gboFillSizesTo.TabStop = false;
            // 
            // radFillSizesTo_SizePlanWithSizeMins
            // 
            this.radFillSizesTo_SizePlanWithSizeMins.Location = new System.Drawing.Point(185, 16);
            this.radFillSizesTo_SizePlanWithSizeMins.Name = "radFillSizesTo_SizePlanWithSizeMins";
            this.radFillSizesTo_SizePlanWithSizeMins.Size = new System.Drawing.Size(132, 16);
            this.radFillSizesTo_SizePlanWithSizeMins.TabIndex = 3;
            this.radFillSizesTo_SizePlanWithSizeMins.Text = "Size Plan + Size Mins";
            this.radFillSizesTo_SizePlanWithSizeMins.CheckedChanged += new System.EventHandler(this.radFillSizesTo_SizePlanWithSizeMins_CheckedChanged);
            // 
            // radFillSizesTo_SizePlan
            // 
            this.radFillSizesTo_SizePlan.Location = new System.Drawing.Point(112, 16);
            this.radFillSizesTo_SizePlan.Name = "radFillSizesTo_SizePlan";
            this.radFillSizesTo_SizePlan.Size = new System.Drawing.Size(72, 16);
            this.radFillSizesTo_SizePlan.TabIndex = 2;
            this.radFillSizesTo_SizePlan.Text = "Size Plan";
            this.radFillSizesTo_SizePlan.CheckedChanged += new System.EventHandler(this.radFillSizesTo_SizePlan_CheckedChanged);
            // 
            // radFillSizesTo_Holes
            // 
            this.radFillSizesTo_Holes.Location = new System.Drawing.Point(52, 16);
            this.radFillSizesTo_Holes.Name = "radFillSizesTo_Holes";
            this.radFillSizesTo_Holes.Size = new System.Drawing.Size(56, 16);
            this.radFillSizesTo_Holes.TabIndex = 1;
            this.radFillSizesTo_Holes.Text = "Holes";
            this.radFillSizesTo_Holes.CheckedChanged += new System.EventHandler(this.radFillSizesTo_Holes_CheckedChanged);
            // 
            // lblFillSizesTo
            // 
            this.lblFillSizesTo.Location = new System.Drawing.Point(3, 16);
            this.lblFillSizesTo.Name = "lblFillSizesTo";
            this.lblFillSizesTo.Size = new System.Drawing.Size(48, 16);
            this.lblFillSizesTo.TabIndex = 0;
            this.lblFillSizesTo.Text = "Fill To:";
            this.lblFillSizesTo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // gboNormalizeSizeCurves
            // 
            this.gboNormalizeSizeCurves.Controls.Add(this.radNormalizeSizeCurves_No);
            this.gboNormalizeSizeCurves.Controls.Add(this.radNormalizeSizeCurves_Yes);
            this.gboNormalizeSizeCurves.Controls.Add(this.lblNormalizeSizeCurves);
            this.gboNormalizeSizeCurves.Location = new System.Drawing.Point(340, 64);
            this.gboNormalizeSizeCurves.Name = "gboNormalizeSizeCurves";
            this.gboNormalizeSizeCurves.Size = new System.Drawing.Size(256, 40);
            this.gboNormalizeSizeCurves.TabIndex = 24;
            this.gboNormalizeSizeCurves.TabStop = false;
            // 
            // radNormalizeSizeCurves_No
            // 
            this.radNormalizeSizeCurves_No.Location = new System.Drawing.Point(200, 12);
            this.radNormalizeSizeCurves_No.Name = "radNormalizeSizeCurves_No";
            this.radNormalizeSizeCurves_No.Size = new System.Drawing.Size(48, 24);
            this.radNormalizeSizeCurves_No.TabIndex = 23;
            this.radNormalizeSizeCurves_No.Text = "No";
            this.radNormalizeSizeCurves_No.CheckedChanged += new System.EventHandler(this.radNormalizeSizeCurves_No_CheckedChanged);
            // 
            // radNormalizeSizeCurves_Yes
            // 
            this.radNormalizeSizeCurves_Yes.Location = new System.Drawing.Point(144, 12);
            this.radNormalizeSizeCurves_Yes.Name = "radNormalizeSizeCurves_Yes";
            this.radNormalizeSizeCurves_Yes.Size = new System.Drawing.Size(48, 24);
            this.radNormalizeSizeCurves_Yes.TabIndex = 22;
            this.radNormalizeSizeCurves_Yes.Text = "Yes";
            this.radNormalizeSizeCurves_Yes.CheckedChanged += new System.EventHandler(this.radNormalizeSizeCurves_Yes_CheckedChanged);
            // 
            // lblNormalizeSizeCurves
            // 
            this.lblNormalizeSizeCurves.AutoSize = true;
            this.lblNormalizeSizeCurves.Location = new System.Drawing.Point(22, 18);
            this.lblNormalizeSizeCurves.Name = "lblNormalizeSizeCurves";
            this.lblNormalizeSizeCurves.Size = new System.Drawing.Size(115, 13);
            this.lblNormalizeSizeCurves.TabIndex = 21;
            this.lblNormalizeSizeCurves.Text = "Normalize Size Curves:";
            this.lblNormalizeSizeCurves.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblFillSizeHoles
            // 
            this.lblFillSizeHoles.AutoSize = true;
            this.lblFillSizeHoles.Location = new System.Drawing.Point(12, 24);
            this.lblFillSizeHoles.Name = "lblFillSizeHoles";
            this.lblFillSizeHoles.Size = new System.Drawing.Size(72, 13);
            this.lblFillSizeHoles.TabIndex = 9;
            this.lblFillSizeHoles.Text = "Fill Size Holes";
            this.lblFillSizeHoles.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // lblFillSizeHolesPct
            // 
            this.lblFillSizeHolesPct.Location = new System.Drawing.Point(69, 44);
            this.lblFillSizeHolesPct.Name = "lblFillSizeHolesPct";
            this.lblFillSizeHolesPct.Size = new System.Drawing.Size(32, 23);
            this.lblFillSizeHolesPct.TabIndex = 19;
            this.lblFillSizeHolesPct.Text = "%";
            this.lblFillSizeHolesPct.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtFillSizeHoles
            // 
            this.txtFillSizeHoles.Location = new System.Drawing.Point(15, 43);
            this.txtFillSizeHoles.MaxLength = 7;
            this.txtFillSizeHoles.Name = "txtFillSizeHoles";
            this.txtFillSizeHoles.Size = new System.Drawing.Size(48, 20);
            this.txtFillSizeHoles.TabIndex = 4;
            this.txtFillSizeHoles.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.toolTip1.SetToolTip(this.txtFillSizeHoles, "Percent of available units for use in Fill Size Holes");
            this.txtFillSizeHoles.TextChanged += new System.EventHandler(this.textBox_TextChanged);
            this.txtFillSizeHoles.Validating += new System.ComponentModel.CancelEventHandler(this.txtZeroToOneHundredPercent_Validating);
            // 
            // lblPackDevTolerancePct
            // 
            this.lblPackDevTolerancePct.Location = new System.Drawing.Point(168, 44);
            this.lblPackDevTolerancePct.Name = "lblPackDevTolerancePct";
            this.lblPackDevTolerancePct.Size = new System.Drawing.Size(32, 23);
            this.lblPackDevTolerancePct.TabIndex = 17;
            this.lblPackDevTolerancePct.Text = "%";
            this.lblPackDevTolerancePct.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtPackDevTolerance
            // 
            this.txtPackDevTolerance.Location = new System.Drawing.Point(114, 43);
            this.txtPackDevTolerance.MaxLength = 7;
            this.txtPackDevTolerance.Name = "txtPackDevTolerance";
            this.txtPackDevTolerance.Size = new System.Drawing.Size(48, 20);
            this.txtPackDevTolerance.TabIndex = 5;
            this.txtPackDevTolerance.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.toolTip1.SetToolTip(this.txtPackDevTolerance, "Percent of the total pack allowed to be in size error.");
            this.txtPackDevTolerance.TextChanged += new System.EventHandler(this.textBox_TextChanged);
            this.txtPackDevTolerance.Validating += new System.ComponentModel.CancelEventHandler(this.txtPositivePercent_Validating);
            // 
            // lblPackDevTolerance
            // 
            this.lblPackDevTolerance.AutoSize = true;
            this.lblPackDevTolerance.Location = new System.Drawing.Point(108, 24);
            this.lblPackDevTolerance.Name = "lblPackDevTolerance";
            this.lblPackDevTolerance.Size = new System.Drawing.Size(174, 13);
            this.lblPackDevTolerance.TabIndex = 5;
            this.lblPackDevTolerance.Text = "Average Pack Deviation Tolerance";
            this.lblPackDevTolerance.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // lblPackNeedTolerance
            // 
            this.lblPackNeedTolerance.AutoSize = true;
            this.lblPackNeedTolerance.Location = new System.Drawing.Point(298, 24);
            this.lblPackNeedTolerance.Name = "lblPackNeedTolerance";
            this.lblPackNeedTolerance.Size = new System.Drawing.Size(135, 13);
            this.lblPackNeedTolerance.TabIndex = 7;
            this.lblPackNeedTolerance.Text = "Max Pack Need Tolerance";
            this.lblPackNeedTolerance.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // txtPackNeedTolerance
            // 
            this.txtPackNeedTolerance.Location = new System.Drawing.Point(302, 44);
            this.txtPackNeedTolerance.MaxLength = 7;
            this.txtPackNeedTolerance.Name = "txtPackNeedTolerance";
            this.txtPackNeedTolerance.Size = new System.Drawing.Size(48, 20);
            this.txtPackNeedTolerance.TabIndex = 6;
            this.txtPackNeedTolerance.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.toolTip1.SetToolTip(this.txtPackNeedTolerance, "Specifies the tolerance for the Maximum Pack Allocation Need error");
            this.txtPackNeedTolerance.TextChanged += new System.EventHandler(this.textBox_TextChanged);
            this.txtPackNeedTolerance.Validating += new System.ComponentModel.CancelEventHandler(this.txtPositivePercent_Validating);
            // 
            // lblSGPeriodWeeks
            // 
            this.lblSGPeriodWeeks.AutoSize = true;
            this.lblSGPeriodWeeks.Location = new System.Drawing.Point(394, 30);
            this.lblSGPeriodWeeks.Name = "lblSGPeriodWeeks";
            this.lblSGPeriodWeeks.Size = new System.Drawing.Size(38, 13);
            this.lblSGPeriodWeeks.TabIndex = 20;
            this.lblSGPeriodWeeks.Text = "weeks";
            this.lblSGPeriodWeeks.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblPctBalTolerance
            // 
            this.lblPctBalTolerance.Location = new System.Drawing.Point(184, 48);
            this.lblPctBalTolerance.Name = "lblPctBalTolerance";
            this.lblPctBalTolerance.Size = new System.Drawing.Size(32, 23);
            this.lblPctBalTolerance.TabIndex = 16;
            this.lblPctBalTolerance.Text = "%";
            this.lblPctBalTolerance.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblPctNeedLimitPct
            // 
            this.lblPctNeedLimitPct.Location = new System.Drawing.Point(184, 22);
            this.lblPctNeedLimitPct.Name = "lblPctNeedLimitPct";
            this.lblPctNeedLimitPct.Size = new System.Drawing.Size(32, 23);
            this.lblPctNeedLimitPct.TabIndex = 15;
            this.lblPctNeedLimitPct.Text = "%";
            this.lblPctNeedLimitPct.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblSGPeriod
            // 
            this.lblSGPeriod.AutoSize = true;
            this.lblSGPeriod.Location = new System.Drawing.Point(219, 30);
            this.lblSGPeriod.Name = "lblSGPeriod";
            this.lblSGPeriod.Size = new System.Drawing.Size(116, 13);
            this.lblSGPeriod.TabIndex = 11;
            this.lblSGPeriod.Text = "Store Grade Timeframe";
            this.lblSGPeriod.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtSGPeriod
            // 
            this.txtSGPeriod.Location = new System.Drawing.Point(338, 26);
            this.txtSGPeriod.MaxLength = 3;
            this.txtSGPeriod.Name = "txtSGPeriod";
            this.txtSGPeriod.Size = new System.Drawing.Size(48, 20);
            this.txtSGPeriod.TabIndex = 1;
            this.txtSGPeriod.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtSGPeriod.TextChanged += new System.EventHandler(this.textBox_TextChanged);
            this.txtSGPeriod.Validating += new System.ComponentModel.CancelEventHandler(this.txtPositiveInteger_Validating);
            // 
            // lblBalTolerance
            // 
            this.lblBalTolerance.AutoSize = true;
            this.lblBalTolerance.Location = new System.Drawing.Point(24, 55);
            this.lblBalTolerance.Name = "lblBalTolerance";
            this.lblBalTolerance.Size = new System.Drawing.Size(97, 13);
            this.lblBalTolerance.TabIndex = 3;
            this.lblBalTolerance.Text = "Balance Tolerance";
            this.lblBalTolerance.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtPctBalTolerance
            // 
            this.txtPctBalTolerance.Location = new System.Drawing.Point(128, 51);
            this.txtPctBalTolerance.MaxLength = 7;
            this.txtPctBalTolerance.Name = "txtPctBalTolerance";
            this.txtPctBalTolerance.Size = new System.Drawing.Size(48, 20);
            this.txtPctBalTolerance.TabIndex = 2;
            this.txtPctBalTolerance.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtPctBalTolerance.TextChanged += new System.EventHandler(this.textBox_TextChanged);
            this.txtPctBalTolerance.Validating += new System.ComponentModel.CancelEventHandler(this.txtPositivePercent_Validating);
            // 
            // lblNeedLimit
            // 
            this.lblNeedLimit.AutoSize = true;
            this.lblNeedLimit.Location = new System.Drawing.Point(64, 29);
            this.lblNeedLimit.Name = "lblNeedLimit";
            this.lblNeedLimit.Size = new System.Drawing.Size(57, 13);
            this.lblNeedLimit.TabIndex = 1;
            this.lblNeedLimit.Text = "Need Limit";
            this.lblNeedLimit.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtPctNeedLimit
            // 
            this.txtPctNeedLimit.Location = new System.Drawing.Point(128, 25);
            this.txtPctNeedLimit.MaxLength = 7;
            this.txtPctNeedLimit.Name = "txtPctNeedLimit";
            this.txtPctNeedLimit.Size = new System.Drawing.Size(48, 20);
            this.txtPctNeedLimit.TabIndex = 0;
            this.txtPctNeedLimit.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtPctNeedLimit.TextChanged += new System.EventHandler(this.textBox_TextChanged);
            this.txtPctNeedLimit.Validating += new System.ComponentModel.CancelEventHandler(this.txtPercent_Validating);
            // 
            // tabOTS
            // 
            this.tabOTS.Controls.Add(this.uGridVersions);
            this.tabOTS.Location = new System.Drawing.Point(4, 22);
            this.tabOTS.Name = "tabOTS";
            this.tabOTS.Size = new System.Drawing.Size(647, 416);
            this.tabOTS.TabIndex = 3;
            this.tabOTS.Text = "OTS Plan Versions";
            this.tabOTS.UseVisualStyleBackColor = true;
            this.tabOTS.Visible = false;
            // 
            // uGridVersions
            // 
            this.uGridVersions.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.uGridVersions.ContextMenuStrip = this.contextMenuStrip1;
            appearance1.BackColor = System.Drawing.Color.White;
            appearance1.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance1.BackGradientStyle = Infragistics.Win.GradientStyle.ForwardDiagonal;
            this.uGridVersions.DisplayLayout.Appearance = appearance1;
            this.uGridVersions.DisplayLayout.InterBandSpacing = 10;
            appearance2.BackColor = System.Drawing.Color.Transparent;
            this.uGridVersions.DisplayLayout.Override.CardAreaAppearance = appearance2;
            appearance3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance3.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance3.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance3.ForeColor = System.Drawing.Color.Black;
            appearance3.TextHAlignAsString = "Left";
            appearance3.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            this.uGridVersions.DisplayLayout.Override.HeaderAppearance = appearance3;
            appearance4.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.uGridVersions.DisplayLayout.Override.RowAppearance = appearance4;
            appearance5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance5.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance5.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            this.uGridVersions.DisplayLayout.Override.RowSelectorAppearance = appearance5;
            this.uGridVersions.DisplayLayout.Override.RowSelectorWidth = 12;
            this.uGridVersions.DisplayLayout.Override.RowSpacingBefore = 2;
            appearance6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance6.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance6.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance6.ForeColor = System.Drawing.Color.Black;
            this.uGridVersions.DisplayLayout.Override.SelectedRowAppearance = appearance6;
            this.uGridVersions.DisplayLayout.RowConnectorColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.uGridVersions.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.Solid;
            this.uGridVersions.Location = new System.Drawing.Point(16, 16);
            this.uGridVersions.Name = "uGridVersions";
            this.uGridVersions.Size = new System.Drawing.Size(615, 351);
            this.uGridVersions.TabIndex = 1;
            this.uGridVersions.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.uGridVersions_InitializeLayout);
            this.uGridVersions.AfterRowInsert += new Infragistics.Win.UltraWinGrid.RowEventHandler(this.uGridVersions_AfterRowInsert);
            this.uGridVersions.BeforeRowUpdate += new Infragistics.Win.UltraWinGrid.CancelableRowEventHandler(this.uGridVersions_BeforeRowUpdate);
            this.uGridVersions.CellChange += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.uGridVersions_CellChange);
            this.uGridVersions.BeforeRowsDeleted += new Infragistics.Win.UltraWinGrid.BeforeRowsDeletedEventHandler(this.uGridVersions_BeforeRowsDeleted);
            this.uGridVersions.MouseEnterElement += new Infragistics.Win.UIElementEventHandler(this.uGridVersions_MouseEnterElement);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuDelete,
            this.toolStripMenuInUse});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(108, 48);
            // 
            // toolStripMenuDelete
            // 
            this.toolStripMenuDelete.Name = "toolStripMenuDelete";
            this.toolStripMenuDelete.Size = new System.Drawing.Size(107, 22);
            this.toolStripMenuDelete.Text = "Delete";
            this.toolStripMenuDelete.ToolTipText = "Delete this item.";
            this.toolStripMenuDelete.Click += new System.EventHandler(this.toolStripMenuDelete_Click);
            // 
            // toolStripMenuInUse
            // 
            this.toolStripMenuInUse.Name = "toolStripMenuInUse";
            this.toolStripMenuInUse.Size = new System.Drawing.Size(107, 22);
            this.toolStripMenuInUse.Text = "In Use";
            this.toolStripMenuInUse.ToolTipText = "Check to see if this row is In Use.";
            this.toolStripMenuInUse.Click += new System.EventHandler(this.toolStripMenuInUse_Click);
            // 
            // tabAllocHeaders
            // 
            this.tabAllocHeaders.Controls.Add(this.gbxDCFulfillmentSplitBy);
            this.tabAllocHeaders.Controls.Add(this.groupBox3);
            this.tabAllocHeaders.Controls.Add(this.cboDoNotReleaseIfAllInReserve);
            this.tabAllocHeaders.Controls.Add(this.cboProtectInterfacedHeaders);
            this.tabAllocHeaders.Controls.Add(this.cboHeaderLinkChar);
            this.tabAllocHeaders.Controls.Add(this.lblHeaderLinkChar);
            this.tabAllocHeaders.Controls.Add(this.label1);
            this.tabAllocHeaders.Controls.Add(this.clbHeaderTypeRelease);
            this.tabAllocHeaders.Location = new System.Drawing.Point(4, 22);
            this.tabAllocHeaders.Name = "tabAllocHeaders";
            this.tabAllocHeaders.Size = new System.Drawing.Size(647, 416);
            this.tabAllocHeaders.TabIndex = 5;
            this.tabAllocHeaders.Text = "Headers";
            this.tabAllocHeaders.UseVisualStyleBackColor = true;
            // 
            // gbxDCFulfillmentSplitBy
            // 
            this.gbxDCFulfillmentSplitBy.Controls.Add(this.gbxWithinDC);
            this.gbxDCFulfillmentSplitBy.Controls.Add(this.gbxMinimums);
            this.gbxDCFulfillmentSplitBy.Controls.Add(this.gbxReserve);
            this.gbxDCFulfillmentSplitBy.Controls.Add(this.gbxSplitBy);
            this.gbxDCFulfillmentSplitBy.Location = new System.Drawing.Point(16, 288);
            this.gbxDCFulfillmentSplitBy.Name = "gbxDCFulfillmentSplitBy";
            this.gbxDCFulfillmentSplitBy.Size = new System.Drawing.Size(267, 125);
            this.gbxDCFulfillmentSplitBy.TabIndex = 6;
            this.gbxDCFulfillmentSplitBy.TabStop = false;
            this.gbxDCFulfillmentSplitBy.Text = "DC Fulfillment Options";
            this.gbxDCFulfillmentSplitBy.Enter += new System.EventHandler(this.groupBox5_Enter);
            // 
            // gbxWithinDC
            // 
            this.gbxWithinDC.Controls.Add(this.radWithinDCProportional);
            this.gbxWithinDC.Controls.Add(this.radWithinDCFill);
            this.gbxWithinDC.Controls.Add(this.lblWithinDC);
            this.gbxWithinDC.Location = new System.Drawing.Point(8, 69);
            this.gbxWithinDC.Name = "gbxWithinDC";
            this.gbxWithinDC.Size = new System.Drawing.Size(251, 27);
            this.gbxWithinDC.TabIndex = 3;
            this.gbxWithinDC.TabStop = false;
            // 
            // radWithinDCProportional
            // 
            this.radWithinDCProportional.AutoSize = true;
            this.radWithinDCProportional.Location = new System.Drawing.Point(164, 5);
            this.radWithinDCProportional.Name = "radWithinDCProportional";
            this.radWithinDCProportional.Size = new System.Drawing.Size(84, 17);
            this.radWithinDCProportional.TabIndex = 3;
            this.radWithinDCProportional.TabStop = true;
            this.radWithinDCProportional.Text = "Proportional:";
            this.radWithinDCProportional.UseVisualStyleBackColor = true;
            this.radWithinDCProportional.CheckedChanged += new System.EventHandler(this.radWithinDCProportional_CheckedChanged);
            // 
            // radWithinDCFill
            // 
            this.radWithinDCFill.AutoSize = true;
            this.radWithinDCFill.Location = new System.Drawing.Point(60, 5);
            this.radWithinDCFill.Name = "radWithinDCFill";
            this.radWithinDCFill.Size = new System.Drawing.Size(40, 17);
            this.radWithinDCFill.TabIndex = 2;
            this.radWithinDCFill.TabStop = true;
            this.radWithinDCFill.Text = "Fill:";
            this.radWithinDCFill.UseVisualStyleBackColor = true;
            this.radWithinDCFill.CheckedChanged += new System.EventHandler(this.radWithinDCFill_CheckedChanged);
            // 
            // lblWithinDC
            // 
            this.lblWithinDC.AutoSize = true;
            this.lblWithinDC.Location = new System.Drawing.Point(3, 7);
            this.lblWithinDC.Name = "lblWithinDC";
            this.lblWithinDC.Size = new System.Drawing.Size(58, 13);
            this.lblWithinDC.TabIndex = 1;
            this.lblWithinDC.Text = "Within DC:";
            // 
            // gbxMinimums
            // 
            this.gbxMinimums.Controls.Add(this.label2);
            this.gbxMinimums.Controls.Add(this.radApplyAllStores);
            this.gbxMinimums.Controls.Add(this.lblMinimums);
            this.gbxMinimums.Controls.Add(this.radApplyAllocQty);
            this.gbxMinimums.Location = new System.Drawing.Point(7, 93);
            this.gbxMinimums.Name = "gbxMinimums";
            this.gbxMinimums.Size = new System.Drawing.Size(254, 26);
            this.gbxMinimums.TabIndex = 2;
            this.gbxMinimums.TabStop = false;
            this.gbxMinimums.Visible = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 31);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "label2";
            // 
            // radApplyAllStores
            // 
            this.radApplyAllStores.AutoSize = true;
            this.radApplyAllStores.Location = new System.Drawing.Point(62, 8);
            this.radApplyAllStores.Name = "radApplyAllStores";
            this.radApplyAllStores.Size = new System.Drawing.Size(65, 17);
            this.radApplyAllStores.TabIndex = 2;
            this.radApplyAllStores.TabStop = true;
            this.radApplyAllStores.Text = "Pre-split:";
            this.radApplyAllStores.UseVisualStyleBackColor = true;
            this.radApplyAllStores.Visible = false;
            this.radApplyAllStores.CheckedChanged += new System.EventHandler(this.radApplyAllStores_CheckedChanged);
            // 
            // lblMinimums
            // 
            this.lblMinimums.AutoSize = true;
            this.lblMinimums.Location = new System.Drawing.Point(6, 10);
            this.lblMinimums.Name = "lblMinimums";
            this.lblMinimums.Size = new System.Drawing.Size(56, 13);
            this.lblMinimums.TabIndex = 1;
            this.lblMinimums.Text = "Minimums:";
            // 
            // radApplyAllocQty
            // 
            this.radApplyAllocQty.AutoSize = true;
            this.radApplyAllocQty.Location = new System.Drawing.Point(166, 6);
            this.radApplyAllocQty.Name = "radApplyAllocQty";
            this.radApplyAllocQty.Size = new System.Drawing.Size(70, 17);
            this.radApplyAllocQty.TabIndex = 0;
            this.radApplyAllocQty.TabStop = true;
            this.radApplyAllocQty.Text = "Post-split:";
            this.radApplyAllocQty.UseVisualStyleBackColor = true;
            this.radApplyAllocQty.Visible = false;
            this.radApplyAllocQty.CheckedChanged += new System.EventHandler(this.radApplyAllocQty_CheckedChanged);
            // 
            // gbxReserve
            // 
            this.gbxReserve.Controls.Add(this.radPostSplit);
            this.gbxReserve.Controls.Add(this.radPreSplit);
            this.gbxReserve.Controls.Add(this.lblReserve);
            this.gbxReserve.Location = new System.Drawing.Point(7, 44);
            this.gbxReserve.Name = "gbxReserve";
            this.gbxReserve.Size = new System.Drawing.Size(254, 25);
            this.gbxReserve.TabIndex = 1;
            this.gbxReserve.TabStop = false;
            // 
            // radPostSplit
            // 
            this.radPostSplit.AutoSize = true;
            this.radPostSplit.Location = new System.Drawing.Point(166, 7);
            this.radPostSplit.Name = "radPostSplit";
            this.radPostSplit.Size = new System.Drawing.Size(70, 17);
            this.radPostSplit.TabIndex = 2;
            this.radPostSplit.TabStop = true;
            this.radPostSplit.Text = "Post-split:";
            this.radPostSplit.UseVisualStyleBackColor = true;
            this.radPostSplit.CheckedChanged += new System.EventHandler(this.radPostSplit_CheckedChanged);
            // 
            // radPreSplit
            // 
            this.radPreSplit.AutoSize = true;
            this.radPreSplit.Location = new System.Drawing.Point(62, 6);
            this.radPreSplit.Name = "radPreSplit";
            this.radPreSplit.Size = new System.Drawing.Size(65, 17);
            this.radPreSplit.TabIndex = 1;
            this.radPreSplit.TabStop = true;
            this.radPreSplit.Text = "Pre-split:";
            this.radPreSplit.UseVisualStyleBackColor = true;
            this.radPreSplit.CheckedChanged += new System.EventHandler(this.radPreSplit_CheckedChanged);
            // 
            // lblReserve
            // 
            this.lblReserve.AutoSize = true;
            this.lblReserve.Location = new System.Drawing.Point(4, 7);
            this.lblReserve.Name = "lblReserve";
            this.lblReserve.Size = new System.Drawing.Size(50, 13);
            this.lblReserve.TabIndex = 0;
            this.lblReserve.Text = "Reserve:";
            // 
            // gbxSplitBy
            // 
            this.gbxSplitBy.Controls.Add(this.radSplitDCStore);
            this.gbxSplitBy.Controls.Add(this.radSplitStoreDC);
            this.gbxSplitBy.Controls.Add(this.lblSplit);
            this.gbxSplitBy.Location = new System.Drawing.Point(7, 20);
            this.gbxSplitBy.Name = "gbxSplitBy";
            this.gbxSplitBy.Size = new System.Drawing.Size(254, 23);
            this.gbxSplitBy.TabIndex = 0;
            this.gbxSplitBy.TabStop = false;
            // 
            // radSplitDCStore
            // 
            this.radSplitDCStore.AutoSize = true;
            this.radSplitDCStore.Location = new System.Drawing.Point(166, 5);
            this.radSplitDCStore.Name = "radSplitDCStore";
            this.radSplitDCStore.Size = new System.Drawing.Size(95, 17);
            this.radSplitDCStore.TabIndex = 2;
            this.radSplitDCStore.TabStop = true;
            this.radSplitDCStore.Text = "DC then Store:";
            this.radSplitDCStore.UseVisualStyleBackColor = true;
            this.radSplitDCStore.CheckedChanged += new System.EventHandler(this.radSplitDCStore_CheckedChanged);
            // 
            // radSplitStoreDC
            // 
            this.radSplitStoreDC.AutoSize = true;
            this.radSplitStoreDC.Location = new System.Drawing.Point(62, 6);
            this.radSplitStoreDC.Name = "radSplitStoreDC";
            this.radSplitStoreDC.Size = new System.Drawing.Size(95, 17);
            this.radSplitStoreDC.TabIndex = 1;
            this.radSplitStoreDC.TabStop = true;
            this.radSplitStoreDC.Text = "Store then DC:";
            this.radSplitStoreDC.UseVisualStyleBackColor = true;
            this.radSplitStoreDC.CheckedChanged += new System.EventHandler(this.radSplitStoreDC_CheckedChanged);
            // 
            // lblSplit
            // 
            this.lblSplit.AutoSize = true;
            this.lblSplit.Location = new System.Drawing.Point(4, 9);
            this.lblSplit.Name = "lblSplit";
            this.lblSplit.Size = new System.Drawing.Size(44, 13);
            this.lblSplit.TabIndex = 0;
            this.lblSplit.Text = "Split by:";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.gbxOrderStoresBy);
            this.groupBox3.Controls.Add(this.gbxMasterSplitOptions);
            this.groupBox3.Location = new System.Drawing.Point(289, 9);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(349, 384);
            this.groupBox3.TabIndex = 5;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "DC Fulfillment Defaults";
            // 
            // gbxOrderStoresBy
            // 
            this.gbxOrderStoresBy.Controls.Add(this.gbxStoreOrder);
            this.gbxOrderStoresBy.Controls.Add(this.ugOrderStoresBy);
            this.gbxOrderStoresBy.Location = new System.Drawing.Point(13, 154);
            this.gbxOrderStoresBy.Name = "gbxOrderStoresBy";
            this.gbxOrderStoresBy.Size = new System.Drawing.Size(344, 224);
            this.gbxOrderStoresBy.TabIndex = 9;
            this.gbxOrderStoresBy.TabStop = false;
            // 
            // gbxStoreOrder
            // 
            this.gbxStoreOrder.Controls.Add(this.rbStoresDescending);
            this.gbxStoreOrder.Controls.Add(this.rbStoresAscending);
            this.gbxStoreOrder.Location = new System.Drawing.Point(148, 10);
            this.gbxStoreOrder.Name = "gbxStoreOrder";
            this.gbxStoreOrder.Size = new System.Drawing.Size(186, 29);
            this.gbxStoreOrder.TabIndex = 10;
            this.gbxStoreOrder.TabStop = false;
            // 
            // rbStoresDescending
            // 
            this.rbStoresDescending.AutoSize = true;
            this.rbStoresDescending.Location = new System.Drawing.Point(105, 9);
            this.rbStoresDescending.Name = "rbStoresDescending";
            this.rbStoresDescending.Size = new System.Drawing.Size(82, 17);
            this.rbStoresDescending.TabIndex = 1;
            this.rbStoresDescending.Text = "Descending";
            this.rbStoresDescending.UseVisualStyleBackColor = true;
            this.rbStoresDescending.CheckedChanged += new System.EventHandler(this.rbStoresDescending_CheckedChanged);
            // 
            // rbStoresAscending
            // 
            this.rbStoresAscending.AutoSize = true;
            this.rbStoresAscending.Location = new System.Drawing.Point(19, 9);
            this.rbStoresAscending.Name = "rbStoresAscending";
            this.rbStoresAscending.Size = new System.Drawing.Size(75, 17);
            this.rbStoresAscending.TabIndex = 0;
            this.rbStoresAscending.Text = "Ascending";
            this.rbStoresAscending.UseVisualStyleBackColor = true;
            this.rbStoresAscending.CheckedChanged += new System.EventHandler(this.rbStoresAscending_CheckedChanged);
            // 
            // ugOrderStoresBy
            // 
            this.ugOrderStoresBy.AllowDrop = true;
            this.ugOrderStoresBy.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ugOrderStoresBy.ContextMenu = this.mnuGrids;
            this.ugOrderStoresBy.DisplayLayout.AddNewBox.Hidden = false;
            this.ugOrderStoresBy.DisplayLayout.AddNewBox.Prompt = " Add ...";
            appearance7.BackColor = System.Drawing.Color.White;
            appearance7.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance7.BackGradientStyle = Infragistics.Win.GradientStyle.ForwardDiagonal;
            this.ugOrderStoresBy.DisplayLayout.Appearance = appearance7;
            ultraGridBand1.AddButtonCaption = " Action";
            this.ugOrderStoresBy.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
            this.ugOrderStoresBy.DisplayLayout.InterBandSpacing = 10;
            appearance8.BackColor = System.Drawing.Color.Transparent;
            this.ugOrderStoresBy.DisplayLayout.Override.CardAreaAppearance = appearance8;
            appearance9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance9.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance9.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance9.ForeColor = System.Drawing.Color.Black;
            appearance9.TextHAlignAsString = "Left";
            appearance9.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            this.ugOrderStoresBy.DisplayLayout.Override.HeaderAppearance = appearance9;
            appearance10.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.ugOrderStoresBy.DisplayLayout.Override.RowAppearance = appearance10;
            appearance11.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance11.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance11.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            this.ugOrderStoresBy.DisplayLayout.Override.RowSelectorAppearance = appearance11;
            this.ugOrderStoresBy.DisplayLayout.Override.RowSelectorWidth = 12;
            this.ugOrderStoresBy.DisplayLayout.Override.RowSpacingBefore = 2;
            appearance12.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance12.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance12.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance12.ForeColor = System.Drawing.Color.Black;
            this.ugOrderStoresBy.DisplayLayout.Override.SelectedRowAppearance = appearance12;
            this.ugOrderStoresBy.DisplayLayout.RowConnectorColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.ugOrderStoresBy.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.Solid;
            ultraGridLayout1.AddNewBox.Hidden = false;
            this.ugOrderStoresBy.Layouts.Add(ultraGridLayout1);
            this.ugOrderStoresBy.Location = new System.Drawing.Point(0, 46);
            this.ugOrderStoresBy.Name = "ugOrderStoresBy";
            this.ugOrderStoresBy.Size = new System.Drawing.Size(334, 160);
            this.ugOrderStoresBy.TabIndex = 7;
            this.ugOrderStoresBy.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.ugOrderStoresBy_InitializeLayout);
            this.ugOrderStoresBy.AfterRowInsert += new Infragistics.Win.UltraWinGrid.RowEventHandler(this.ugOrderStoresBy_AfterRowInsert);
            this.ugOrderStoresBy.AfterCellListCloseUp += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ugOrderStoresBy_AfterCellListCloseUp);
            this.ugOrderStoresBy.MouseEnterElement += new Infragistics.Win.UIElementEventHandler(this.ugOrderStoresBy_MouseEnterElement);
            // 
            // gbxMasterSplitOptions
            // 
            this.gbxMasterSplitOptions.Controls.Add(this.cbxApplyMinimums);
            this.gbxMasterSplitOptions.Controls.Add(this.groupBox4);
            this.gbxMasterSplitOptions.Controls.Add(this.gboAppplyOverageTo);
            this.gbxMasterSplitOptions.Controls.Add(this.cboPrioritizeHeadersBy);
            this.gbxMasterSplitOptions.Controls.Add(this.lblPrioritizeHeadersBy);
            this.gbxMasterSplitOptions.Location = new System.Drawing.Point(13, 17);
            this.gbxMasterSplitOptions.Name = "gbxMasterSplitOptions";
            this.gbxMasterSplitOptions.Size = new System.Drawing.Size(330, 131);
            this.gbxMasterSplitOptions.TabIndex = 6;
            this.gbxMasterSplitOptions.TabStop = false;
            this.gbxMasterSplitOptions.Text = "Master Split Options";
            // 
            // cbxApplyMinimums
            // 
            this.cbxApplyMinimums.AutoSize = true;
            this.cbxApplyMinimums.Location = new System.Drawing.Point(168, 22);
            this.cbxApplyMinimums.Name = "cbxApplyMinimums";
            this.cbxApplyMinimums.Size = new System.Drawing.Size(101, 17);
            this.cbxApplyMinimums.TabIndex = 5;
            this.cbxApplyMinimums.Text = "Apply Minimums";
            this.cbxApplyMinimums.UseVisualStyleBackColor = true;
            this.cbxApplyMinimums.CheckedChanged += new System.EventHandler(this.cbxApplyMinimums_CheckedChanged);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.rbProportional);
            this.groupBox4.Controls.Add(this.rbDCFulfillment);
            this.groupBox4.Location = new System.Drawing.Point(6, 11);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(149, 51);
            this.groupBox4.TabIndex = 4;
            this.groupBox4.TabStop = false;
            // 
            // rbProportional
            // 
            this.rbProportional.AutoSize = true;
            this.rbProportional.Location = new System.Drawing.Point(16, 28);
            this.rbProportional.Name = "rbProportional";
            this.rbProportional.Size = new System.Drawing.Size(81, 17);
            this.rbProportional.TabIndex = 1;
            this.rbProportional.Text = "Proportional";
            this.rbProportional.UseVisualStyleBackColor = true;
            this.rbProportional.CheckedChanged += new System.EventHandler(this.rbProportional_CheckedChanged);
            // 
            // rbDCFulfillment
            // 
            this.rbDCFulfillment.AutoSize = true;
            this.rbDCFulfillment.Location = new System.Drawing.Point(16, 10);
            this.rbDCFulfillment.Name = "rbDCFulfillment";
            this.rbDCFulfillment.Size = new System.Drawing.Size(89, 17);
            this.rbDCFulfillment.TabIndex = 0;
            this.rbDCFulfillment.Text = "DC Fulfillment";
            this.rbDCFulfillment.UseVisualStyleBackColor = true;
            this.rbDCFulfillment.CheckedChanged += new System.EventHandler(this.rbDCFulfillment_CheckedChanged);
            // 
            // gboAppplyOverageTo
            // 
            this.gboAppplyOverageTo.Controls.Add(this.rbHeadersDescending);
            this.gboAppplyOverageTo.Controls.Add(this.rbHeadersAscending);
            this.gboAppplyOverageTo.Location = new System.Drawing.Point(6, 91);
            this.gboAppplyOverageTo.Name = "gboAppplyOverageTo";
            this.gboAppplyOverageTo.Size = new System.Drawing.Size(209, 34);
            this.gboAppplyOverageTo.TabIndex = 3;
            this.gboAppplyOverageTo.TabStop = false;
            // 
            // rbHeadersDescending
            // 
            this.rbHeadersDescending.AutoSize = true;
            this.rbHeadersDescending.Location = new System.Drawing.Point(105, 13);
            this.rbHeadersDescending.Name = "rbHeadersDescending";
            this.rbHeadersDescending.Size = new System.Drawing.Size(82, 17);
            this.rbHeadersDescending.TabIndex = 1;
            this.rbHeadersDescending.Text = "Descending";
            this.rbHeadersDescending.UseVisualStyleBackColor = true;
            this.rbHeadersDescending.CheckedChanged += new System.EventHandler(this.rbHeadersDescending_CheckedChanged);
            // 
            // rbHeadersAscending
            // 
            this.rbHeadersAscending.AutoSize = true;
            this.rbHeadersAscending.Location = new System.Drawing.Point(16, 13);
            this.rbHeadersAscending.Name = "rbHeadersAscending";
            this.rbHeadersAscending.Size = new System.Drawing.Size(75, 17);
            this.rbHeadersAscending.TabIndex = 0;
            this.rbHeadersAscending.Text = "Ascending";
            this.rbHeadersAscending.UseVisualStyleBackColor = true;
            this.rbHeadersAscending.CheckedChanged += new System.EventHandler(this.rbHeadersAscending_CheckedChanged);
            // 
            // cboPrioritizeHeadersBy
            // 
            this.cboPrioritizeHeadersBy.AllowDrop = true;
            this.cboPrioritizeHeadersBy.AllowUserAttributes = false;
            this.cboPrioritizeHeadersBy.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboPrioritizeHeadersBy.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboPrioritizeHeadersBy.Cursor = System.Windows.Forms.Cursors.Default;
            this.cboPrioritizeHeadersBy.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboPrioritizeHeadersBy.FormattingEnabled = true;
            this.cboPrioritizeHeadersBy.Location = new System.Drawing.Point(126, 64);
            this.cboPrioritizeHeadersBy.Name = "cboPrioritizeHeadersBy";
            this.cboPrioritizeHeadersBy.Size = new System.Drawing.Size(164, 21);
            this.cboPrioritizeHeadersBy.TabIndex = 0;
            this.cboPrioritizeHeadersBy.MIDComboBoxPropertiesChangedEvent += new MIDRetail.Windows.Controls.MIDComboBoxPropertiesChangedEventHandler(this.cboPrioritizeHeadersBy_MIDComboBoxPropertiesChangedEvent);
            this.cboPrioritizeHeadersBy.SelectionChangeCommitted += new System.EventHandler(this.cboPrioritizeHeadersBy_SelectionChangeCommitted);
            // 
            // lblPrioritizeHeadersBy
            // 
            this.lblPrioritizeHeadersBy.AutoSize = true;
            this.lblPrioritizeHeadersBy.Location = new System.Drawing.Point(8, 67);
            this.lblPrioritizeHeadersBy.Name = "lblPrioritizeHeadersBy";
            this.lblPrioritizeHeadersBy.Size = new System.Drawing.Size(104, 13);
            this.lblPrioritizeHeadersBy.TabIndex = 1;
            this.lblPrioritizeHeadersBy.Text = "Prioritize Headers By";
            // 
            // cboDoNotReleaseIfAllInReserve
            // 
            this.cboDoNotReleaseIfAllInReserve.Location = new System.Drawing.Point(21, 219);
            this.cboDoNotReleaseIfAllInReserve.Name = "cboDoNotReleaseIfAllInReserve";
            this.cboDoNotReleaseIfAllInReserve.Size = new System.Drawing.Size(232, 24);
            this.cboDoNotReleaseIfAllInReserve.TabIndex = 4;
            this.cboDoNotReleaseIfAllInReserve.Text = "Do not Release if all units in reserve";
            this.cboDoNotReleaseIfAllInReserve.CheckedChanged += new System.EventHandler(this.cboDoNotReleaseIfAllInReserve_CheckedChanged);
            // 
            // cboProtectInterfacedHeaders
            // 
            this.cboProtectInterfacedHeaders.Location = new System.Drawing.Point(21, 195);
            this.cboProtectInterfacedHeaders.Name = "cboProtectInterfacedHeaders";
            this.cboProtectInterfacedHeaders.Size = new System.Drawing.Size(232, 24);
            this.cboProtectInterfacedHeaders.TabIndex = 2;
            this.cboProtectInterfacedHeaders.Text = "Protect Interfaced Headers";
            this.cboProtectInterfacedHeaders.CheckedChanged += new System.EventHandler(this.cboProtectInterfacedHeaders_CheckedChanged);
            // 
            // cboHeaderLinkChar
            // 
            this.cboHeaderLinkChar.AutoAdjust = true;
            this.cboHeaderLinkChar.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboHeaderLinkChar.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboHeaderLinkChar.DataSource = null;
            this.cboHeaderLinkChar.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboHeaderLinkChar.DropDownWidth = 168;
            this.cboHeaderLinkChar.FormattingEnabled = false;
            this.cboHeaderLinkChar.IgnoreFocusLost = false;
            this.cboHeaderLinkChar.ItemHeight = 13;
            this.cboHeaderLinkChar.Location = new System.Drawing.Point(48, 264);
            this.cboHeaderLinkChar.Margin = new System.Windows.Forms.Padding(0);
            this.cboHeaderLinkChar.MaxDropDownItems = 25;
            this.cboHeaderLinkChar.Name = "cboHeaderLinkChar";
            this.cboHeaderLinkChar.SetToolTip = "";
            this.cboHeaderLinkChar.Size = new System.Drawing.Size(168, 21);
            this.cboHeaderLinkChar.TabIndex = 3;
            this.cboHeaderLinkChar.Tag = null;
            this.cboHeaderLinkChar.MIDComboBoxPropertiesChangedEvent += new MIDRetail.Windows.Controls.MIDComboBoxPropertiesChangedEventHandler(this.cboHeaderLinkChar_MIDComboBoxPropertiesChangedEvent);
            this.cboHeaderLinkChar.SelectionChangeCommitted += new System.EventHandler(this.cboHeaderLinkChar_SelectionChangeCommitted);
            // 
            // lblHeaderLinkChar
            // 
            this.lblHeaderLinkChar.Location = new System.Drawing.Point(20, 245);
            this.lblHeaderLinkChar.Name = "lblHeaderLinkChar";
            this.lblHeaderLinkChar.Size = new System.Drawing.Size(200, 23);
            this.lblHeaderLinkChar.TabIndex = 2;
            this.lblHeaderLinkChar.Text = "Link Headers with Characteristic:";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(19, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(264, 23);
            this.label1.TabIndex = 1;
            this.label1.Text = "Select the types of headers that can be released";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // clbHeaderTypeRelease
            // 
            this.clbHeaderTypeRelease.CheckOnClick = true;
            this.clbHeaderTypeRelease.Location = new System.Drawing.Point(16, 32);
            this.clbHeaderTypeRelease.Name = "clbHeaderTypeRelease";
            this.clbHeaderTypeRelease.Size = new System.Drawing.Size(240, 154);
            this.clbHeaderTypeRelease.TabIndex = 0;
            this.clbHeaderTypeRelease.SelectedValueChanged += new System.EventHandler(this.clbHeaderTypeRelease_SelectedValueChanged);
            // 
            // tabBasisLabels
            // 
            this.tabBasisLabels.Controls.Add(this.FRBLgroupBox);
            this.tabBasisLabels.Location = new System.Drawing.Point(4, 22);
            this.tabBasisLabels.Name = "tabBasisLabels";
            this.tabBasisLabels.Size = new System.Drawing.Size(647, 416);
            this.tabBasisLabels.TabIndex = 6;
            this.tabBasisLabels.Text = "Basis Labels";
            this.tabBasisLabels.UseVisualStyleBackColor = true;
            // 
            // FRBLgroupBox
            // 
            this.FRBLgroupBox.Location = new System.Drawing.Point(20, 14);
            this.FRBLgroupBox.Name = "FRBLgroupBox";
            this.FRBLgroupBox.Padding = new System.Windows.Forms.Padding(18, 9, 18, 13);
            this.FRBLgroupBox.Size = new System.Drawing.Size(601, 310);
            this.FRBLgroupBox.TabIndex = 21;
            this.FRBLgroupBox.TabStop = false;
            this.FRBLgroupBox.Text = "Forecast Review Basis Labels";
            // 
            // tabOTSDefaults
            // 
            this.tabOTSDefaults.Controls.Add(this.groupBox2);
            this.tabOTSDefaults.Controls.Add(this.groupBox1);
            this.tabOTSDefaults.Location = new System.Drawing.Point(4, 22);
            this.tabOTSDefaults.Name = "tabOTSDefaults";
            this.tabOTSDefaults.Padding = new System.Windows.Forms.Padding(3);
            this.tabOTSDefaults.Size = new System.Drawing.Size(647, 416);
            this.tabOTSDefaults.TabIndex = 7;
            this.tabOTSDefaults.Text = "OTS Defaults";
            this.tabOTSDefaults.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.ckbProrateChainStock);
            this.groupBox2.Location = new System.Drawing.Point(20, 127);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(18, 9, 18, 13);
            this.groupBox2.Size = new System.Drawing.Size(600, 89);
            this.groupBox2.TabIndex = 23;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Processing Options";
            // 
            // ckbProrateChainStock
            // 
            this.ckbProrateChainStock.Location = new System.Drawing.Point(30, 36);
            this.ckbProrateChainStock.Name = "ckbProrateChainStock";
            this.ckbProrateChainStock.Size = new System.Drawing.Size(232, 18);
            this.ckbProrateChainStock.TabIndex = 3;
            this.ckbProrateChainStock.Text = "Prorate Chain Stock when zero Chain Sales:";
            this.ckbProrateChainStock.CheckedChanged += new System.EventHandler(this.checkbox_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lblMaximumChainWOS);
            this.groupBox1.Controls.Add(this.txtMaximumChainWOS);
            this.groupBox1.Controls.Add(this.lblNumberOfWeeksWithZeroSales);
            this.groupBox1.Controls.Add(this.txtNumberOfWeeksWithZeroSales);
            this.groupBox1.Location = new System.Drawing.Point(20, 16);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(18, 9, 18, 13);
            this.groupBox1.Size = new System.Drawing.Size(600, 89);
            this.groupBox1.TabIndex = 22;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Chain WOS Calculation Options";
            // 
            // lblMaximumChainWOS
            // 
            this.lblMaximumChainWOS.AutoSize = true;
            this.lblMaximumChainWOS.Location = new System.Drawing.Point(355, 44);
            this.lblMaximumChainWOS.Name = "lblMaximumChainWOS";
            this.lblMaximumChainWOS.Size = new System.Drawing.Size(113, 13);
            this.lblMaximumChainWOS.TabIndex = 11;
            this.lblMaximumChainWOS.Text = "Maximum Chain WOS:";
            this.lblMaximumChainWOS.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtMaximumChainWOS
            // 
            this.txtMaximumChainWOS.Location = new System.Drawing.Point(471, 40);
            this.txtMaximumChainWOS.MaxLength = 7;
            this.txtMaximumChainWOS.Name = "txtMaximumChainWOS";
            this.txtMaximumChainWOS.Size = new System.Drawing.Size(48, 20);
            this.txtMaximumChainWOS.TabIndex = 10;
            this.txtMaximumChainWOS.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.toolTip1.SetToolTip(this.txtMaximumChainWOS, "Specifies the maximum chain WOS value that will be used in forecasting store stoc" +
        "k plans.");
            this.txtMaximumChainWOS.Validating += new System.ComponentModel.CancelEventHandler(this.txtPositiveInteger_Validating);
            // 
            // lblNumberOfWeeksWithZeroSales
            // 
            this.lblNumberOfWeeksWithZeroSales.AutoSize = true;
            this.lblNumberOfWeeksWithZeroSales.Location = new System.Drawing.Point(34, 44);
            this.lblNumberOfWeeksWithZeroSales.Name = "lblNumberOfWeeksWithZeroSales";
            this.lblNumberOfWeeksWithZeroSales.Size = new System.Drawing.Size(145, 13);
            this.lblNumberOfWeeksWithZeroSales.TabIndex = 9;
            this.lblNumberOfWeeksWithZeroSales.Text = "No. of weeks with zero sales:";
            this.lblNumberOfWeeksWithZeroSales.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtNumberOfWeeksWithZeroSales
            // 
            this.txtNumberOfWeeksWithZeroSales.Location = new System.Drawing.Point(182, 40);
            this.txtNumberOfWeeksWithZeroSales.MaxLength = 7;
            this.txtNumberOfWeeksWithZeroSales.Name = "txtNumberOfWeeksWithZeroSales";
            this.txtNumberOfWeeksWithZeroSales.Size = new System.Drawing.Size(48, 20);
            this.txtNumberOfWeeksWithZeroSales.TabIndex = 8;
            this.txtNumberOfWeeksWithZeroSales.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.toolTip1.SetToolTip(this.txtNumberOfWeeksWithZeroSales, "Specifies how many weeks of zero sales is acceptable before it quits looking forw" +
        "ard in order to calculate the chain WOS.\r\n");
            this.txtNumberOfWeeksWithZeroSales.Validating += new System.ComponentModel.CancelEventHandler(this.txtPositiveInteger_Validating);
            // 
            // tabSystem
            // 
            this.tabSystem.Controls.Add(this.gbCurrentUsers);
            this.tabSystem.Controls.Add(this.panel1);
            this.tabSystem.Location = new System.Drawing.Point(4, 22);
            this.tabSystem.Name = "tabSystem";
            this.tabSystem.Size = new System.Drawing.Size(647, 416);
            this.tabSystem.TabIndex = 8;
            this.tabSystem.Text = "System";
            this.tabSystem.UseVisualStyleBackColor = true;
            // 
            // gbCurrentUsers
            // 
            this.gbCurrentUsers.Controls.Add(this.pnlCurrentUsers);
            this.gbCurrentUsers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbCurrentUsers.Location = new System.Drawing.Point(0, 182);
            this.gbCurrentUsers.Name = "gbCurrentUsers";
            this.gbCurrentUsers.Size = new System.Drawing.Size(647, 234);
            this.gbCurrentUsers.TabIndex = 24;
            this.gbCurrentUsers.TabStop = false;
            this.gbCurrentUsers.Text = "Current Users";
            // 
            // pnlCurrentUsers
            // 
            this.pnlCurrentUsers.Controls.Add(this.pnlCurrentUsers_Fill_Panel);
            this.pnlCurrentUsers.Controls.Add(this._pnlCurrentUsers_Toolbars_Dock_Area_Left);
            this.pnlCurrentUsers.Controls.Add(this._pnlCurrentUsers_Toolbars_Dock_Area_Right);
            this.pnlCurrentUsers.Controls.Add(this._pnlCurrentUsers_Toolbars_Dock_Area_Bottom);
            this.pnlCurrentUsers.Controls.Add(this._pnlCurrentUsers_Toolbars_Dock_Area_Top);
            this.pnlCurrentUsers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlCurrentUsers.Location = new System.Drawing.Point(3, 16);
            this.pnlCurrentUsers.Name = "pnlCurrentUsers";
            this.pnlCurrentUsers.Size = new System.Drawing.Size(641, 215);
            this.pnlCurrentUsers.TabIndex = 0;
            // 
            // pnlCurrentUsers_Fill_Panel
            // 
            // 
            // pnlCurrentUsers_Fill_Panel.ClientArea
            // 
            this.pnlCurrentUsers_Fill_Panel.ClientArea.Controls.Add(this.ultraGrid1);
            this.pnlCurrentUsers_Fill_Panel.Cursor = System.Windows.Forms.Cursors.Default;
            this.pnlCurrentUsers_Fill_Panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlCurrentUsers_Fill_Panel.Location = new System.Drawing.Point(0, 46);
            this.pnlCurrentUsers_Fill_Panel.Name = "pnlCurrentUsers_Fill_Panel";
            this.pnlCurrentUsers_Fill_Panel.Size = new System.Drawing.Size(641, 169);
            this.pnlCurrentUsers_Fill_Panel.TabIndex = 0;
            // 
            // ultraGrid1
            // 
            appearance13.BackColor = System.Drawing.SystemColors.Window;
            appearance13.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.ultraGrid1.DisplayLayout.Appearance = appearance13;
            this.ultraGrid1.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns;
            this.ultraGrid1.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.ultraGrid1.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance14.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance14.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance14.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance14.BorderColor = System.Drawing.SystemColors.Window;
            this.ultraGrid1.DisplayLayout.GroupByBox.Appearance = appearance14;
            appearance15.ForeColor = System.Drawing.SystemColors.GrayText;
            this.ultraGrid1.DisplayLayout.GroupByBox.BandLabelAppearance = appearance15;
            this.ultraGrid1.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.ultraGrid1.DisplayLayout.GroupByBox.Hidden = true;
            appearance16.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance16.BackColor2 = System.Drawing.SystemColors.Control;
            appearance16.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance16.ForeColor = System.Drawing.SystemColors.GrayText;
            this.ultraGrid1.DisplayLayout.GroupByBox.PromptAppearance = appearance16;
            this.ultraGrid1.DisplayLayout.MaxColScrollRegions = 1;
            this.ultraGrid1.DisplayLayout.MaxRowScrollRegions = 1;
            appearance17.BackColor = System.Drawing.SystemColors.Window;
            appearance17.ForeColor = System.Drawing.SystemColors.ControlText;
            this.ultraGrid1.DisplayLayout.Override.ActiveCellAppearance = appearance17;
            appearance18.BackColor = System.Drawing.SystemColors.Highlight;
            appearance18.ForeColor = System.Drawing.Color.Black;
            this.ultraGrid1.DisplayLayout.Override.ActiveRowAppearance = appearance18;
            this.ultraGrid1.DisplayLayout.Override.AllowAddNew = Infragistics.Win.UltraWinGrid.AllowAddNew.No;
            this.ultraGrid1.DisplayLayout.Override.AllowColMoving = Infragistics.Win.UltraWinGrid.AllowColMoving.NotAllowed;
            this.ultraGrid1.DisplayLayout.Override.AllowColSwapping = Infragistics.Win.UltraWinGrid.AllowColSwapping.NotAllowed;
            this.ultraGrid1.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            this.ultraGrid1.DisplayLayout.Override.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.True;
            this.ultraGrid1.DisplayLayout.Override.AllowRowSummaries = Infragistics.Win.UltraWinGrid.AllowRowSummaries.False;
            this.ultraGrid1.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.ultraGrid1.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance19.BackColor = System.Drawing.SystemColors.Window;
            this.ultraGrid1.DisplayLayout.Override.CardAreaAppearance = appearance19;
            appearance20.BorderColor = System.Drawing.Color.Silver;
            appearance20.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.ultraGrid1.DisplayLayout.Override.CellAppearance = appearance20;
            this.ultraGrid1.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;
            this.ultraGrid1.DisplayLayout.Override.CellPadding = 0;
            appearance21.BackColor = System.Drawing.SystemColors.Control;
            appearance21.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance21.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance21.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance21.BorderColor = System.Drawing.SystemColors.Window;
            this.ultraGrid1.DisplayLayout.Override.GroupByRowAppearance = appearance21;
            appearance22.TextHAlignAsString = "Left";
            this.ultraGrid1.DisplayLayout.Override.HeaderAppearance = appearance22;
            this.ultraGrid1.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.ultraGrid1.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance23.BackColor = System.Drawing.SystemColors.Window;
            appearance23.BorderColor = System.Drawing.Color.Silver;
            this.ultraGrid1.DisplayLayout.Override.RowAppearance = appearance23;
            this.ultraGrid1.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;
            this.ultraGrid1.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.None;
            this.ultraGrid1.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Extended;
            this.ultraGrid1.DisplayLayout.Override.SummaryDisplayArea = Infragistics.Win.UltraWinGrid.SummaryDisplayAreas.TopFixed;
            appearance24.BackColor = System.Drawing.SystemColors.ControlLight;
            this.ultraGrid1.DisplayLayout.Override.TemplateAddRowAppearance = appearance24;
            this.ultraGrid1.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.ultraGrid1.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.ultraGrid1.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.ultraGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraGrid1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraGrid1.Location = new System.Drawing.Point(0, 0);
            this.ultraGrid1.Name = "ultraGrid1";
            this.ultraGrid1.Size = new System.Drawing.Size(641, 169);
            this.ultraGrid1.TabIndex = 0;
            this.ultraGrid1.Text = "ultraGrid1";
            this.ultraGrid1.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.ultraGrid1_InitializeLayout);
            // 
            // _pnlCurrentUsers_Toolbars_Dock_Area_Left
            // 
            this._pnlCurrentUsers_Toolbars_Dock_Area_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._pnlCurrentUsers_Toolbars_Dock_Area_Left.BackColor = System.Drawing.Color.Transparent;
            this._pnlCurrentUsers_Toolbars_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Left;
            this._pnlCurrentUsers_Toolbars_Dock_Area_Left.ForeColor = System.Drawing.SystemColors.ControlText;
            this._pnlCurrentUsers_Toolbars_Dock_Area_Left.Location = new System.Drawing.Point(0, 46);
            this._pnlCurrentUsers_Toolbars_Dock_Area_Left.Name = "_pnlCurrentUsers_Toolbars_Dock_Area_Left";
            this._pnlCurrentUsers_Toolbars_Dock_Area_Left.Size = new System.Drawing.Size(0, 169);
            this._pnlCurrentUsers_Toolbars_Dock_Area_Left.ToolbarsManager = this.ultraToolbarsManager1;
            // 
            // ultraToolbarsManager1
            // 
            this.ultraToolbarsManager1.DesignerFlags = 1;
            this.ultraToolbarsManager1.DockWithinContainer = this.pnlCurrentUsers;
            this.ultraToolbarsManager1.ShowFullMenusDelay = 500;
            ultraToolbar1.DockedColumn = 0;
            ultraToolbar1.DockedRow = 0;
            ultraToolbar1.NonInheritedTools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            buttonTool1,
            buttonTool3});
            ultraToolbar1.Settings.AllowCustomize = Infragistics.Win.DefaultableBoolean.False;
            ultraToolbar1.Settings.AllowDockBottom = Infragistics.Win.DefaultableBoolean.False;
            ultraToolbar1.Settings.AllowDockLeft = Infragistics.Win.DefaultableBoolean.False;
            ultraToolbar1.Settings.AllowDockRight = Infragistics.Win.DefaultableBoolean.False;
            ultraToolbar1.Settings.AllowFloating = Infragistics.Win.DefaultableBoolean.False;
            ultraToolbar1.Settings.AllowHiding = Infragistics.Win.DefaultableBoolean.False;
            ultraToolbar1.Settings.FillEntireRow = Infragistics.Win.DefaultableBoolean.True;
            ultraToolbar1.Settings.GrabHandleStyle = Infragistics.Win.UltraWinToolbars.GrabHandleStyle.None;
            ultraToolbar1.Text = "CurrentUsers";
            ultraToolbar2.DockedColumn = 0;
            ultraToolbar2.DockedRow = 1;
            ultraToolbar2.NonInheritedTools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            labelTool3});
            ultraToolbar2.Settings.AllowCustomize = Infragistics.Win.DefaultableBoolean.False;
            ultraToolbar2.Settings.AllowDockBottom = Infragistics.Win.DefaultableBoolean.False;
            ultraToolbar2.Settings.AllowDockLeft = Infragistics.Win.DefaultableBoolean.False;
            ultraToolbar2.Settings.AllowDockRight = Infragistics.Win.DefaultableBoolean.False;
            ultraToolbar2.Settings.AllowFloating = Infragistics.Win.DefaultableBoolean.False;
            ultraToolbar2.Settings.AllowHiding = Infragistics.Win.DefaultableBoolean.False;
            ultraToolbar2.Settings.GrabHandleStyle = Infragistics.Win.UltraWinToolbars.GrabHandleStyle.None;
            ultraToolbar2.Text = "ActiveUserCountToolbar";
            this.ultraToolbarsManager1.Toolbars.AddRange(new Infragistics.Win.UltraWinToolbars.UltraToolbar[] {
            ultraToolbar1,
            ultraToolbar2});
            buttonTool2.SharedPropsInternal.Caption = "Show Current Users";
            buttonTool2.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
            buttonTool4.SharedPropsInternal.Caption = "Log Out Selected Users";
            buttonTool4.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
            appearance25.TextHAlignAsString = "Left";
            labelTool2.SharedPropsInternal.AppearancesSmall.Appearance = appearance25;
            this.ultraToolbarsManager1.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            buttonTool2,
            buttonTool4,
            labelTool2});
            this.ultraToolbarsManager1.ToolClick += new Infragistics.Win.UltraWinToolbars.ToolClickEventHandler(this.ultraToolbarsManager1_ToolClick);
            // 
            // _pnlCurrentUsers_Toolbars_Dock_Area_Right
            // 
            this._pnlCurrentUsers_Toolbars_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._pnlCurrentUsers_Toolbars_Dock_Area_Right.BackColor = System.Drawing.Color.Transparent;
            this._pnlCurrentUsers_Toolbars_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Right;
            this._pnlCurrentUsers_Toolbars_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
            this._pnlCurrentUsers_Toolbars_Dock_Area_Right.Location = new System.Drawing.Point(641, 46);
            this._pnlCurrentUsers_Toolbars_Dock_Area_Right.Name = "_pnlCurrentUsers_Toolbars_Dock_Area_Right";
            this._pnlCurrentUsers_Toolbars_Dock_Area_Right.Size = new System.Drawing.Size(0, 169);
            this._pnlCurrentUsers_Toolbars_Dock_Area_Right.ToolbarsManager = this.ultraToolbarsManager1;
            // 
            // _pnlCurrentUsers_Toolbars_Dock_Area_Bottom
            // 
            this._pnlCurrentUsers_Toolbars_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._pnlCurrentUsers_Toolbars_Dock_Area_Bottom.BackColor = System.Drawing.Color.Transparent;
            this._pnlCurrentUsers_Toolbars_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Bottom;
            this._pnlCurrentUsers_Toolbars_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
            this._pnlCurrentUsers_Toolbars_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 215);
            this._pnlCurrentUsers_Toolbars_Dock_Area_Bottom.Name = "_pnlCurrentUsers_Toolbars_Dock_Area_Bottom";
            this._pnlCurrentUsers_Toolbars_Dock_Area_Bottom.Size = new System.Drawing.Size(641, 0);
            this._pnlCurrentUsers_Toolbars_Dock_Area_Bottom.ToolbarsManager = this.ultraToolbarsManager1;
            // 
            // _pnlCurrentUsers_Toolbars_Dock_Area_Top
            // 
            this._pnlCurrentUsers_Toolbars_Dock_Area_Top.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._pnlCurrentUsers_Toolbars_Dock_Area_Top.BackColor = System.Drawing.Color.Transparent;
            this._pnlCurrentUsers_Toolbars_Dock_Area_Top.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Top;
            this._pnlCurrentUsers_Toolbars_Dock_Area_Top.ForeColor = System.Drawing.SystemColors.ControlText;
            this._pnlCurrentUsers_Toolbars_Dock_Area_Top.Location = new System.Drawing.Point(0, 0);
            this._pnlCurrentUsers_Toolbars_Dock_Area_Top.Name = "_pnlCurrentUsers_Toolbars_Dock_Area_Top";
            this._pnlCurrentUsers_Toolbars_Dock_Area_Top.Size = new System.Drawing.Size(641, 46);
            this._pnlCurrentUsers_Toolbars_Dock_Area_Top.ToolbarsManager = this.ultraToolbarsManager1;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.gbSystem);
            this.panel1.Controls.Add(this.gbLoginOptions);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(647, 182);
            this.panel1.TabIndex = 22;
            // 
            // gbSystem
            // 
            this.gbSystem.Controls.Add(this.lblBatchModeLastChanged);
            this.gbSystem.Controls.Add(this.lblLastChangedBy);
            this.gbSystem.Controls.Add(this.lblMessageForClients);
            this.gbSystem.Controls.Add(this.txtSendMsg);
            this.gbSystem.Controls.Add(this.btnSendMsg);
            this.gbSystem.Controls.Add(this.btnBatchModeTurnOff);
            this.gbSystem.Controls.Add(this.btnBatchModeTurnOn);
            this.gbSystem.Controls.Add(this.lblBatchOnlyMode);
            this.gbSystem.Location = new System.Drawing.Point(213, 9);
            this.gbSystem.Name = "gbSystem";
            this.gbSystem.Size = new System.Drawing.Size(431, 134);
            this.gbSystem.TabIndex = 22;
            this.gbSystem.TabStop = false;
            this.gbSystem.Text = "Batch Only Mode";
            // 
            // lblBatchModeLastChanged
            // 
            this.lblBatchModeLastChanged.AutoSize = true;
            this.lblBatchModeLastChanged.Location = new System.Drawing.Point(84, 80);
            this.lblBatchModeLastChanged.Name = "lblBatchModeLastChanged";
            this.lblBatchModeLastChanged.Size = new System.Drawing.Size(0, 13);
            this.lblBatchModeLastChanged.TabIndex = 16;
            // 
            // lblLastChangedBy
            // 
            this.lblLastChangedBy.AutoSize = true;
            this.lblLastChangedBy.Location = new System.Drawing.Point(6, 80);
            this.lblLastChangedBy.Name = "lblLastChangedBy";
            this.lblLastChangedBy.Size = new System.Drawing.Size(76, 13);
            this.lblLastChangedBy.TabIndex = 15;
            this.lblLastChangedBy.Text = "Last Changed:";
            // 
            // lblMessageForClients
            // 
            this.lblMessageForClients.AutoSize = true;
            this.lblMessageForClients.Location = new System.Drawing.Point(6, 48);
            this.lblMessageForClients.Name = "lblMessageForClients";
            this.lblMessageForClients.Size = new System.Drawing.Size(101, 13);
            this.lblMessageForClients.TabIndex = 14;
            this.lblMessageForClients.Text = "Message for clients:";
            // 
            // txtSendMsg
            // 
            this.txtSendMsg.Location = new System.Drawing.Point(113, 44);
            this.txtSendMsg.Name = "txtSendMsg";
            this.txtSendMsg.Size = new System.Drawing.Size(230, 20);
            this.txtSendMsg.TabIndex = 13;
            // 
            // btnSendMsg
            // 
            this.btnSendMsg.Location = new System.Drawing.Point(350, 43);
            this.btnSendMsg.Name = "btnSendMsg";
            this.btnSendMsg.Size = new System.Drawing.Size(75, 23);
            this.btnSendMsg.TabIndex = 12;
            this.btnSendMsg.Text = "Send Msg";
            this.btnSendMsg.UseVisualStyleBackColor = true;
            this.btnSendMsg.Click += new System.EventHandler(this.btnSendMsg_Click);
            // 
            // btnBatchModeTurnOff
            // 
            this.btnBatchModeTurnOff.Location = new System.Drawing.Point(350, 13);
            this.btnBatchModeTurnOff.Name = "btnBatchModeTurnOff";
            this.btnBatchModeTurnOff.Size = new System.Drawing.Size(75, 23);
            this.btnBatchModeTurnOff.TabIndex = 11;
            this.btnBatchModeTurnOff.Text = "Turn Off";
            this.toolTip1.SetToolTip(this.btnBatchModeTurnOff, "Turns off Batch Only Mode and allows\r\nusers to log in.");
            this.btnBatchModeTurnOff.UseVisualStyleBackColor = true;
            this.btnBatchModeTurnOff.Click += new System.EventHandler(this.btnBatchModeTurnOff_Click);
            // 
            // btnBatchModeTurnOn
            // 
            this.btnBatchModeTurnOn.Location = new System.Drawing.Point(269, 13);
            this.btnBatchModeTurnOn.Name = "btnBatchModeTurnOn";
            this.btnBatchModeTurnOn.Size = new System.Drawing.Size(75, 23);
            this.btnBatchModeTurnOn.TabIndex = 10;
            this.btnBatchModeTurnOn.Text = "Turn On";
            this.toolTip1.SetToolTip(this.btnBatchModeTurnOn, "Puts the MID Retail system in Batch Only Mode\r\nand will shutdown all client insta" +
        "nces.");
            this.btnBatchModeTurnOn.UseVisualStyleBackColor = true;
            this.btnBatchModeTurnOn.Click += new System.EventHandler(this.btnBatchModeTurnOn_Click);
            // 
            // lblBatchOnlyMode
            // 
            this.lblBatchOnlyMode.Location = new System.Drawing.Point(53, 18);
            this.lblBatchOnlyMode.Name = "lblBatchOnlyMode";
            this.lblBatchOnlyMode.Size = new System.Drawing.Size(210, 18);
            this.lblBatchOnlyMode.TabIndex = 4;
            this.lblBatchOnlyMode.Text = "Batch Only Mode";
            this.lblBatchOnlyMode.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // gbLoginOptions
            // 
            this.gbLoginOptions.Controls.Add(this.radActiveDirectoryWithDomain);
            this.gbLoginOptions.Controls.Add(this.radActiveDirectoryAuthentication);
            this.gbLoginOptions.Controls.Add(this.radWindowsAuthentication);
            this.gbLoginOptions.Controls.Add(this.radStandardAuthentication);
            this.gbLoginOptions.Controls.Add(this.cbxControlServiceDefaultBatchOnlyModeOn);
            this.gbLoginOptions.Controls.Add(this.cbxEnableRemoteSystemOptions);
            this.gbLoginOptions.Controls.Add(this.cbxForceSingleClientInstance);
            this.gbLoginOptions.Controls.Add(this.cbxForceSingleUserInstance);
            this.gbLoginOptions.Location = new System.Drawing.Point(3, 9);
            this.gbLoginOptions.Name = "gbLoginOptions";
            this.gbLoginOptions.Size = new System.Drawing.Size(204, 167);
            this.gbLoginOptions.TabIndex = 23;
            this.gbLoginOptions.TabStop = false;
            this.gbLoginOptions.Text = "Login Options";
            // 
            // radActiveDirectoryWithDomain
            // 
            this.radActiveDirectoryWithDomain.AutoSize = true;
            this.radActiveDirectoryWithDomain.Location = new System.Drawing.Point(6, 70);
            this.radActiveDirectoryWithDomain.Name = "radActiveDirectoryWithDomain";
            this.radActiveDirectoryWithDomain.Size = new System.Drawing.Size(194, 17);
            this.radActiveDirectoryWithDomain.TabIndex = 32;
            this.radActiveDirectoryWithDomain.Text = "Use A.D. Authentication w/ Domain";
            this.radActiveDirectoryWithDomain.UseVisualStyleBackColor = true;
            // 
            // radActiveDirectoryAuthentication
            // 
            this.radActiveDirectoryAuthentication.AutoSize = true;
            this.radActiveDirectoryAuthentication.Location = new System.Drawing.Point(6, 52);
            this.radActiveDirectoryAuthentication.Name = "radActiveDirectoryAuthentication";
            this.radActiveDirectoryAuthentication.Size = new System.Drawing.Size(193, 17);
            this.radActiveDirectoryAuthentication.TabIndex = 31;
            this.radActiveDirectoryAuthentication.Text = "Use Active Directory Authentication";
            this.radActiveDirectoryAuthentication.UseVisualStyleBackColor = true;
            // 
            // radWindowsAuthentication
            // 
            this.radWindowsAuthentication.AutoSize = true;
            this.radWindowsAuthentication.Location = new System.Drawing.Point(6, 34);
            this.radWindowsAuthentication.Name = "radWindowsAuthentication";
            this.radWindowsAuthentication.Size = new System.Drawing.Size(162, 17);
            this.radWindowsAuthentication.TabIndex = 30;
            this.radWindowsAuthentication.Text = "Use Windows Authentication";
            this.radWindowsAuthentication.UseVisualStyleBackColor = true;
            // 
            // radStandardAuthentication
            // 
            this.radStandardAuthentication.AutoSize = true;
            this.radStandardAuthentication.Checked = true;
            this.radStandardAuthentication.Location = new System.Drawing.Point(6, 16);
            this.radStandardAuthentication.Name = "radStandardAuthentication";
            this.radStandardAuthentication.Size = new System.Drawing.Size(161, 17);
            this.radStandardAuthentication.TabIndex = 29;
            this.radStandardAuthentication.TabStop = true;
            this.radStandardAuthentication.Text = "Use Standard Authentication";
            this.radStandardAuthentication.UseVisualStyleBackColor = true;
            // 
            // cbxControlServiceDefaultBatchOnlyModeOn
            // 
            this.cbxControlServiceDefaultBatchOnlyModeOn.Location = new System.Drawing.Point(6, 148);
            this.cbxControlServiceDefaultBatchOnlyModeOn.Name = "cbxControlServiceDefaultBatchOnlyModeOn";
            this.cbxControlServiceDefaultBatchOnlyModeOn.Size = new System.Drawing.Size(189, 17);
            this.cbxControlServiceDefaultBatchOnlyModeOn.TabIndex = 28;
            this.cbxControlServiceDefaultBatchOnlyModeOn.Text = "Start with Batch Only Mode On";
            this.cbxControlServiceDefaultBatchOnlyModeOn.CheckedChanged += new System.EventHandler(this.checkbox_CheckedChanged);
            // 
            // cbxEnableRemoteSystemOptions
            // 
            this.cbxEnableRemoteSystemOptions.Location = new System.Drawing.Point(6, 129);
            this.cbxEnableRemoteSystemOptions.Name = "cbxEnableRemoteSystemOptions";
            this.cbxEnableRemoteSystemOptions.Size = new System.Drawing.Size(189, 17);
            this.cbxEnableRemoteSystemOptions.TabIndex = 27;
            this.cbxEnableRemoteSystemOptions.Text = "Enable Remote System Options";
            this.cbxEnableRemoteSystemOptions.CheckedChanged += new System.EventHandler(this.checkbox_CheckedChanged);
            // 
            // cbxForceSingleClientInstance
            // 
            this.cbxForceSingleClientInstance.Location = new System.Drawing.Point(6, 91);
            this.cbxForceSingleClientInstance.Name = "cbxForceSingleClientInstance";
            this.cbxForceSingleClientInstance.Size = new System.Drawing.Size(189, 17);
            this.cbxForceSingleClientInstance.TabIndex = 25;
            this.cbxForceSingleClientInstance.Text = "Enforce Single Instance";
            this.cbxForceSingleClientInstance.CheckedChanged += new System.EventHandler(this.checkbox_CheckedChanged);
            // 
            // cbxForceSingleUserInstance
            // 
            this.cbxForceSingleUserInstance.Location = new System.Drawing.Point(6, 110);
            this.cbxForceSingleUserInstance.Name = "cbxForceSingleUserInstance";
            this.cbxForceSingleUserInstance.Size = new System.Drawing.Size(189, 17);
            this.cbxForceSingleUserInstance.TabIndex = 26;
            this.cbxForceSingleUserInstance.Text = "Enforce Single User Login";
            this.cbxForceSingleUserInstance.CheckedChanged += new System.EventHandler(this.checkbox_CheckedChanged);
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(487, 474);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 17;
            this.btnOK.Text = "OK";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(575, 474);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 18;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnInUse
            // 
            this.btnInUse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnInUse.Location = new System.Drawing.Point(28, 474);
            this.btnInUse.Name = "btnInUse";
            this.btnInUse.Size = new System.Drawing.Size(75, 23);
            this.btnInUse.TabIndex = 19;
            this.btnInUse.Text = "In Use";
            this.btnInUse.UseVisualStyleBackColor = true;
            this.btnInUse.Click += new System.EventHandler(this.btnInUse_Click);
            // 
            // frmGlobalOptions
            // 
            this.AllowDragDrop = true;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(671, 512);
            this.Controls.Add(this.btnInUse);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.tabControl1);
            this.Name = "frmGlobalOptions";
            this.Text = "Global Options";
            this.Load += new System.EventHandler(this.frmGlobalOptions_Load);
            this.Controls.SetChildIndex(this.tabControl1, 0);
            this.Controls.SetChildIndex(this.btnCancel, 0);
            this.Controls.SetChildIndex(this.btnOK, 0);
            this.Controls.SetChildIndex(this.btnInUse, 0);
            ((System.ComponentModel.ISupportInitialize)(this.utmMain)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabCompInfo.ResumeLayout(false);
            this.tabCompInfo.PerformLayout();
            this.tabStores.ResumeLayout(false);
            this.tabStores.PerformLayout();
            this.gbReqSizeFiters.ResumeLayout(false);
            this.gbReqSizeFiters.PerformLayout();
            this.tabAllocDefault.ResumeLayout(false);
            this.tabAllocDefault.PerformLayout();
            this.gboDCCartonAttribute.ResumeLayout(false);
            this.gboItemFWOS.ResumeLayout(false);
            this.gboItemFWOS.PerformLayout();
            this.gboItemMaxOverride.ResumeLayout(false);
            this.gboItemMaxOverride.PerformLayout();
            this.gbxGenericPackRounding.ResumeLayout(false);
            this.gbxGenericPackRounding.PerformLayout();
            this.gboSizeOptions.ResumeLayout(false);
            this.gboSizeOptions.PerformLayout();
            this.gboGenericSizeCurveName.ResumeLayout(false);
            this.gboGenericSizeCurveName.PerformLayout();
            this.gboFillSizesTo.ResumeLayout(false);
            this.gboNormalizeSizeCurves.ResumeLayout(false);
            this.gboNormalizeSizeCurves.PerformLayout();
            this.tabOTS.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uGridVersions)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.tabAllocHeaders.ResumeLayout(false);
            this.gbxDCFulfillmentSplitBy.ResumeLayout(false);
            this.gbxWithinDC.ResumeLayout(false);
            this.gbxWithinDC.PerformLayout();
            this.gbxMinimums.ResumeLayout(false);
            this.gbxMinimums.PerformLayout();
            this.gbxReserve.ResumeLayout(false);
            this.gbxReserve.PerformLayout();
            this.gbxSplitBy.ResumeLayout(false);
            this.gbxSplitBy.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.gbxOrderStoresBy.ResumeLayout(false);
            this.gbxStoreOrder.ResumeLayout(false);
            this.gbxStoreOrder.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ugOrderStoresBy)).EndInit();
            this.gbxMasterSplitOptions.ResumeLayout(false);
            this.gbxMasterSplitOptions.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.gboAppplyOverageTo.ResumeLayout(false);
            this.gboAppplyOverageTo.PerformLayout();
            this.tabBasisLabels.ResumeLayout(false);
            this.tabOTSDefaults.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabSystem.ResumeLayout(false);
            this.gbCurrentUsers.ResumeLayout(false);
            this.pnlCurrentUsers.ResumeLayout(false);
            this.pnlCurrentUsers_Fill_Panel.ClientArea.ResumeLayout(false);
            this.pnlCurrentUsers_Fill_Panel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ultraGrid1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraToolbarsManager1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.gbSystem.ResumeLayout(false);
            this.gbSystem.PerformLayout();
            this.gbLoginOptions.ResumeLayout(false);
            this.gbLoginOptions.PerformLayout();
            this.ResumeLayout(false);

		}

		#endregion


		//		public string HandleNull(string obj, string defObj)
		//		{
		//			if (obj != DBNull.Value.ToString())
		//				return obj;
		//			else
		//				return defObj;
		//		}

		private void frmGlobalOptions_Load(object sender, System.EventArgs e)
		{
            // Begin Track #4872 - JSmith - Global/User Attributes
            ProfileList alProfileList;
            ProfileList plProfileList;
            ProfileList dcProfileList;  //TT#5126-VStuart-Allocation Store Attribute in Global Options will not update-MID
            // End Track #4872
			try
			{
				FunctionSecurity = _SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AdminGlobalOptions);

				_securityCompanyInfo = _SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AdminGlobalOptionsCompanyInfo);
				_securityDisplayOptions = _SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AdminGlobalOptionsDisplay);
				_securityOTSPlanVersions = _SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AdminGlobalOptionsOTSVersions);
				_securityAllocationDefaults = _SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AdminGlobalOptionsAlDefaults);
				//Begin Track #3784 - JScott - Add security for Header Gloabl Options
				_securityAllocationHeaders = _SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AdminGlobalOptionsAlHeaders);
				//End Track #3784 - JScott - Add security for Header Gloabl Options
				//Begin Track #6240 - stodd - Add security for basis labels and ots forecast
				_securityBasisLabels = _SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AdminGlobalOptionsBasisLabels);
				_securityOTSDefaults = _SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AdminGlobalOptionsOTSDefaults);
				//End Track #6240 - stodd - Add security for basis labels and ots forecast

                //Begin TT#901-MD -jsobek -Batch Only Mode


                //Set Soft Text
                gbLoginOptions.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_RemoteSystemOptions_gbLoginOptions);
                //ckbUseWindowsLogin.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_RemoteSystemOptions_ckbUseWindowsLogin);
                cbxForceSingleClientInstance.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_RemoteSystemOptions_cbxForceSingleClientInstance);
                cbxForceSingleUserInstance.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_RemoteSystemOptions_cbxForceSingleUserInstance);
                cbxEnableRemoteSystemOptions.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_RemoteSystemOptions_cbxEnableRemoteSystemOptions);
                cbxControlServiceDefaultBatchOnlyModeOn.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_RemoteSystemOptions_cbxControlServiceDefaultBatchOnlyModeOn);

                btnBatchModeTurnOn.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_RemoteSystemOptions_btnBatchModeTurnOn);
                btnBatchModeTurnOff.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_RemoteSystemOptions_btnBatchModeTurnOff);
                btnSendMsg.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_RemoteSystemOptions_btnSendMsg);
                lblMessageForClients.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_RemoteSystemOptions_lblMessageForClients);
                lblLastChangedBy.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_RemoteSystemOptions_lblLastChangedBy);

                gbCurrentUsers.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_RemoteSystemOptions_gbCurrentUsers);
                this.ultraToolbarsManager1.Tools["btnShowCurrentUsers"].SharedProps.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_RemoteSystemOptions_btnShowCurrentUsers);
                this.ultraToolbarsManager1.Tools["btnLogOutSelected"].SharedProps.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_RemoteSystemOptions_btnLogOutSelected);

                bool isUserBatchOnlyAdmin;
                FunctionSecurityProfile batchOnlyModeSecurity = _SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AdminBatchOnlyMode);

                if (batchOnlyModeSecurity.AllowUpdate)
                {
                    isUserBatchOnlyAdmin = true;
                }
                else
                {
                    isUserBatchOnlyAdmin = false;
                }

                if (isUserBatchOnlyAdmin == false ) 
                {
                    //Hide the system tab
                    //this.tabSystem.Hide();
                    this.tabControl1.TabPages.Remove(this.tabSystem);  //Hide does not work - must remove the control
                }
                else
                {
                    if (_SAB.IsRemoteServices() == false)
                    {
                        //Disable the system tab batch only mode options when running local
                        this.gbSystem.Enabled = false;
                        this.gbSystem.Text = MIDText.ReplaceControlChars(MIDText.GetTextOnly(eMIDTextCode.msg_RemoteSystemOptions_NotAvailableRunningLocal)); //"Running Local - No system functions are currently available.";
                        this.gbCurrentUsers.Enabled = false;
                        this.btnBatchModeTurnOn.Enabled = false;
                        this.btnBatchModeTurnOff.Enabled = false;
                        this.txtSendMsg.Enabled = false;
                        this.btnSendMsg.Enabled = false;
                    }
                    else
                    {
                        if (_SAB.clientSocketManager != null)
                        {
                            //populate the batch only mode group box
                            SetBatchModeOptions();
                        }
                        else
                        {
                            //Disable the system tab batch only mode options when not connected
                            this.gbSystem.Enabled = false;
                            this.gbSystem.Text = MIDText.ReplaceControlChars(MIDText.GetTextOnly(eMIDTextCode.msg_RemoteSystemOptions_NotAvailableNotConnected)); //"Not connected to control service - No system functions are currently available.";
                            this.gbCurrentUsers.Enabled = false;
                            this.btnBatchModeTurnOn.Enabled = false;
                            this.btnBatchModeTurnOff.Enabled = false;
                            this.txtSendMsg.Enabled = false;
                            this.btnSendMsg.Enabled = false;
                        }
                    }
                }
                //End TT#901-MD -jsobek -Batch Only Mode

				// Begin - MID Track #2479 - 1/24/05 vg	  Only display Size Options when Size Need indicator is turned on.		
				if (!_SAB.ClientServerSession.GlobalOptions.AppConfig.SizeInstalled)
				{
					this.gboSizeOptions.Visible = false;
					this.lblFillSizeHoles.Visible = false;
					this.lblFillSizeHolesPct.Visible = false;
					this.txtFillSizeHoles.Visible = false;
					this.lblPackDevTolerancePct.Visible = false;
					this.txtPackDevTolerance.Visible = false;
					this.lblPackDevTolerance.Visible = false;
					this.lblPackNeedTolerance.Visible = false;
					this.txtPackNeedTolerance.Visible = false;
					// BEGIN MID Track #4921 - JSmith - A&F 666 - Size Modification
					this.gboFillSizesTo.Visible = false;
					this.gboNormalizeSizeCurves.Visible = false;
					// END MID Track #4921
				}
				// End - MID Track #2479 - 1/24/05 vg
				SetText();

				if (!_securityCompanyInfo.AllowUpdate && _securityCompanyInfo.AllowView)
				{
					_currentTabPage = this.tabCompInfo;
				}
				else if (!_securityDisplayOptions.AllowUpdate && _securityDisplayOptions.AllowView)
				{
					_currentTabPage = this.tabStores;
				}
				else if (!_securityOTSPlanVersions.AllowUpdate && _securityOTSPlanVersions.AllowView)
				{
					_currentTabPage = this.tabOTS;
				}
				else if (!_securityAllocationDefaults.AllowUpdate && _securityAllocationDefaults.AllowView)
				{
					_currentTabPage = this.tabAllocDefault;
				}
					//Begin Track #3784 - JScott - Add security for Header Gloabl Options
				else if (!_securityAllocationHeaders.AllowUpdate && _securityAllocationHeaders.AllowView)
				{
					_currentTabPage = this.tabAllocHeaders;
				}
					//End Track #3784 - JScott - Add security for Header Gloabl Options
				//Begin Track #6240 - stodd - Add security for basis labels and ots defaults
				else if (!_securityBasisLabels.AllowUpdate && _securityBasisLabels.AllowView)
				{
					_currentTabPage = this.tabBasisLabels;
				}
				else if (!_securityOTSDefaults.AllowUpdate && _securityOTSDefaults.AllowView)
				{
					_currentTabPage = this.tabOTSDefaults;
				}
				//End Track #6240 - stodd - Add security for basis labels and ots defaults
				else
				{
					_currentTabPage = this.tabCompInfo;
				}

				_versionsHaveChanged = false;
				FormLoaded = false;

				GlobalOptions opts = new GlobalOptions();
				DataTable dt = opts.GetGlobalOptions();

				if (dt.Rows.Count != 1)
				{
					MessageBox.Show("Error loading system options");
					this.Close();
					return;
				}
				DataRow dr = dt.Rows[0];

				// company tab
				txtCompany.Text = dr["COMPANY_NAME"].ToString();
				txtStreet.Text = dr["COMPANY_STREET"].ToString();
				txtCity.Text = dr["COMPANY_CITY"].ToString();
				DataTable dtStates = opts.GetStateAbbreviations();
				cboState.DataSource = dtStates;
				cboState.DisplayMember = "SP_ABBREVIATION";
				cboState.ValueMember = "SP_ABBREVIATION";
				if (dr["COMPANY_SP_ABBREVIATION"] != DBNull.Value)
				{
					_cboStateValue = Convert.ToString(dr["COMPANY_SP_ABBREVIATION"], CultureInfo.CurrentCulture);
					cboState.SelectedValue = _cboStateValue;
				}

				txtZip.Value = dr["COMPANY_POSTAL_CODE"].ToString();
				txtPhone.Value = dr["COMPANY_TELEPHONE"].ToString();
				txtFax.Value = dr["COMPANY_FAX"].ToString();
				txtEmail.Text = dr["COMPANY_EMAIL"].ToString();
				if (dr["PRODUCT_LEVEL_DELIMITER"] != DBNull.Value)
				{
					txtProductLevelDelimiter.Text = Convert.ToString(dr["PRODUCT_LEVEL_DELIMITER"], CultureInfo.CurrentCulture);
				}
				else
				{
					txtProductLevelDelimiter.Text = @"\";
				}

				// purge tab
				//			txtDaily.Text = dr["PURGE_DAILY_HISTORY"].ToString();
				//			if (dr["PURGE_DAILY_HIST_WK_DAY_IND"].ToString() == "1")
				//				rdbWeeks.Checked = true;
				//			else
				//				rdbDays.Checked = true;
				//			txtWeekly.Text = dr["PURGE_WEEKLY_HISTORY"].ToString();
				//			txtPlans.Text = dr["PURGE_PLANS"].ToString();
				//			txtAlloc.Text = dr["PURGE_ALLOCATIONS"].ToString();

				// product & stores tab
				DataTable dtPrDisplayOpts = MIDText.GetTextType(eMIDTextType.eHierarchyDisplayOptions, eMIDTextOrderBy.TextCode);
                // Begin TT#249 - JSmith - Invalid options
                DataRow[] doNotDisplayRow = dtPrDisplayOpts.Select("TEXT_CODE = " + Convert.ToInt32(eHierarchyDisplayOptions.DoNotDisplay).ToString());
                doNotDisplayRow[0].Delete();
                // End TT#249
				cboProduct.DataSource = dtPrDisplayOpts;
				cboProduct.DisplayMember = "TEXT_VALUE";
				cboProduct.ValueMember = "TEXT_CODE";
				_cboProductValue = Convert.ToInt32(dr["PRODUCT_LEVEL_DISPLAY_ID"], CultureInfo.CurrentUICulture);
				cboProduct.SelectedValue = _cboProductValue;
				DataTable dtStDisplayOpts = MIDText.GetTextType(eMIDTextType.eStoreDisplayOptions, eMIDTextOrderBy.TextCode);
				cboStore.DataSource = dtStDisplayOpts;
				cboStore.DisplayMember = "TEXT_VALUE";
				cboStore.ValueMember = "TEXT_CODE";
				_cboStoreValue = Convert.ToInt32(dr["STORE_DISPLAY_OPTION_ID"], CultureInfo.CurrentUICulture);
				cboStore.SelectedValue = _cboStoreValue;

				StoreData storeData = new StoreData();
                // Begin Track #4872 - JSmith - Global/User Attributes
                //DataTable dtPlanStGroup = storeData.StoreGroup_Read(eDataOrderBy.ID);
                //cboPlanStoreAttr.DataSource = dtPlanStGroup;
                //cboPlanStoreAttr.DisplayMember = "SG_ID";
                //cboPlanStoreAttr.ValueMember = "SG_RID";
                plProfileList = StoreMgmt.StoreGroup_GetListViewList(eStoreGroupSelectType.GlobalOnly, false); //SAB.StoreServerSession.GetStoreGroupListViewList(eStoreGroupSelectType.GlobalOnly, false);
                cboPlanStoreAttr.Initialize(SAB, _securityDisplayOptions, plProfileList.ArrayList, false);
                // End Track #4872
				_cboPlanStoreAttrValue = Convert.ToInt32(dr["DEFAULT_OTS_SG_RID"], CultureInfo.CurrentUICulture);
				cboPlanStoreAttr.SelectedValue = _cboPlanStoreAttrValue;
                // Begin Track #4872 - JSmith - Global/User Attributes
                //DataTable dtAllocStGroup = storeData.StoreGroup_Read(eDataOrderBy.ID);
                //cboAllocStoreAttr.DataSource = dtAllocStGroup;
                //cboAllocStoreAttr.DisplayMember = "SG_ID";
                //cboAllocStoreAttr.ValueMember = "SG_RID";
                alProfileList = StoreMgmt.StoreGroup_GetListViewList(eStoreGroupSelectType.GlobalOnly, false); //SAB.StoreServerSession.GetStoreGroupListViewList(eStoreGroupSelectType.GlobalOnly, false);
                cboAllocStoreAttr.Initialize(SAB, _securityDisplayOptions, alProfileList.ArrayList, false);
                // End Track #4872
				_cboAllocStoreAttrValue = Convert.ToInt32(dr["DEFAULT_ALLOC_SG_RID"], CultureInfo.CurrentUICulture);
				cboAllocStoreAttr.SelectedValue = _cboAllocStoreAttrValue;

                //BEGIN TT#5126-VStuart-Allocation Store Attribute in Global Options will not update-MID
                dcProfileList = StoreMgmt.StoreGroup_GetListViewList(eStoreGroupSelectType.GlobalOnly, false);
                //END TT#5126-VStuart-Allocation Store Attribute in Global Options will not update-MID

                // Begin TT#1652-MD - RMatelic - DC Carton Rounding
				// Begin TT#1689-MD - stodd - DC Carton Rounding Default Attribute should be blank if not selected 
                StoreGroupListViewProfile emptyStore = new StoreGroupListViewProfile(0);
                emptyStore.GroupId = "";
                emptyStore.Name  = "";
                //BEGIN TT#5126-VStuart-Allocation Store Attribute in Global Options will not update-MID
                //alProfileList.Insert(0, emptyStore);
                dcProfileList.Insert(0, emptyStore);
                // End TT#1689-MD - stodd - DC Carton Rounding Default Attribute should be blank if not selected 

                //cboDCCartonRoundDfltAttribute.Initialize(SAB, _securityAllocationDefaults, alProfileList.ArrayList, false);
                cboDCCartonRoundDfltAttribute.Initialize(SAB, _securityAllocationDefaults, dcProfileList.ArrayList, false);
                //END TT#5126-VStuart-Allocation Store Attribute in Global Options will not update-MID
                _cboDCCartonRoundDfltAttrValue = Convert.ToInt32(dr["DC_CARTON_ROUNDING_SG_RID"], CultureInfo.CurrentUICulture);
                cboDCCartonRoundDfltAttribute.SelectedValue = _cboDCCartonRoundDfltAttrValue;
                // End TT#1652-MD 

                // BEGIN TT#1966-MD - AGallagher - DC Fulfillment
                BuildHeaderCharList();

                _split_Option = (eDCFulfillmentSplitOption)(Convert.ToInt32(dr["SPLIT_OPTION"]));
                if (_split_Option == eDCFulfillmentSplitOption.DCFulfillment)
                { this.rbDCFulfillment.Checked = true; }
                if (_split_Option == eDCFulfillmentSplitOption.Proportional)
                { this.rbProportional.Checked = true; }

                _apply_Minimums_Ind = Convert.ToChar(dr["APPLY_MINIMUMS_IND"], CultureInfo.CurrentUICulture);
                if (_apply_Minimums_Ind == '1')
                { this.cbxApplyMinimums.Checked = true; }
                else
                { this.cbxApplyMinimums.Checked = false; }

                if (dr["PRIORITIZE_TYPE"] == DBNull.Value)
                    { _prioritize_Type = 'H'; }
                else
                    { _prioritize_Type = Convert.ToChar(dr["PRIORITIZE_TYPE"], CultureInfo.CurrentUICulture); }

                if (dr["HEADER_FIELD"] == DBNull.Value)
                { _header_Field = -7; }
                else
                    { _header_Field = Convert.ToInt32(dr["HEADER_FIELD"], CultureInfo.CurrentUICulture); }

                if (dr["HCG_RID"] == DBNull.Value)
                     { _hcg_Rid = -1; } 
                else
                    { _hcg_Rid = Convert.ToInt32(dr["HCG_RID"], CultureInfo.CurrentUICulture); } 

                _header_Order = (eDCFulfillmentHeadersOrder)(Convert.ToInt32(dr["HEADERS_ORDER"]));
                if (_header_Order == eDCFulfillmentHeadersOrder.Ascending)
                    {
                        this.rbHeadersAscending.Checked = true;
                    }
                if (_header_Order == eDCFulfillmentHeadersOrder.Descending)
                {
                    this.rbHeadersDescending.Checked = true;
                }
                _store_Order = (eDCFulfillmentStoresOrder)(Convert.ToInt32(dr["STORES_ORDER"]));
                if (_store_Order == eDCFulfillmentStoresOrder.Ascending)
                {
                    this.rbStoresAscending.Checked = true;
                }
                if (_store_Order == eDCFulfillmentStoresOrder.Descending)
                {
                    this.rbStoresDescending.Checked = true;
                }
                if (_prioritize_Type == 'C')
                { cboPrioritizeHeadersBy.SelectedValue = _hcg_Rid;} 
                else
                { cboPrioritizeHeadersBy.SelectedValue = _header_Field; }

                _split_By_Option = (eDCFulfillmentSplitByOption)(Convert.ToInt32(dr["SPLIT_BY_OPTION"]));
                if (_split_By_Option == eDCFulfillmentSplitByOption.SplitByDC)
                { this.radSplitDCStore.Checked = true; }
                if (_split_By_Option == eDCFulfillmentSplitByOption.SplitByStore)
                { this.radSplitStoreDC.Checked = true; }

                _split_By_Reserve = (eDCFulfillmentReserve)(Convert.ToInt32(dr["SPLIT_BY_RESERVE"]));
                if (_split_By_Reserve == eDCFulfillmentReserve.ReservePostSplit)
                { this.radPostSplit.Checked = true; }
                if (_split_By_Reserve == eDCFulfillmentReserve.ReservePreSplit)
                { this.radPreSplit.Checked = true; }

                _apply_By = (eDCFulfillmentMinimums)(Convert.ToInt32(dr["APPLY_BY"]));
                if (_apply_By == eDCFulfillmentMinimums.ApplyByQty)
                { this.radApplyAllocQty.Checked = true; }
                if (_apply_By == eDCFulfillmentMinimums.ApplyFirst)
                { this.radApplyAllStores.Checked = true; }

                _within_Dc = (eDCFulfillmentWithinDC)(Convert.ToInt32(dr["WITHIN_DC"]));
                if (_within_Dc == eDCFulfillmentWithinDC.Fill)
                { this.radWithinDCFill.Checked = true; }
                if (_within_Dc == eDCFulfillmentWithinDC.Proportional)
                { this.radWithinDCProportional.Checked = true; }

                char useExternalEligibilityAllocation = Convert.ToChar(dr["USE_EXTERNAL_ELIGIBILITY_ALLOCATION"], CultureInfo.CurrentUICulture);
                _useExternalEligibilityAllocation = Include.ConvertCharToBool(useExternalEligibilityAllocation);

                char useExternalEligibilityPlanning = Convert.ToChar(dr["USE_EXTERNAL_ELIGIBILITY_PLANNING"], CultureInfo.CurrentUICulture);
                _useExternalEligibilityPlanning = Include.ConvertCharToBool(useExternalEligibilityPlanning);

                int identifierOption = (Convert.ToInt32(dr["EXTERNAL_ELIGIBILITY_PRODUCT_IDENTIFIER"]));
                _externalEligibilityProductIdentifier = (eExternalEligibilityProductIdentifier)identifierOption;

                identifierOption = (Convert.ToInt32(dr["EXTERNAL_ELIGIBILITY_CHANNEL_IDENTIFIER"]));
                _externalEligibilityChannelIdentifier = (eExternalEligibilityChannelIdentifier)identifierOption;

                if (dr["EXTERNAL_ELIGIBILITY_URL"] == DBNull.Value)
                { _externalEligibilityURL = null; }
                else
                { _externalEligibilityURL = Convert.ToString(dr["EXTERNAL_ELIGIBILITY_URL"], CultureInfo.CurrentUICulture); }

                //Load DCF Store Order -----------------------
                dtdcf = opts.GetDCFStoreOrderInfo(Convert.ToInt32(dr["SYSTEM_OPTION_RID"], CultureInfo.CurrentUICulture));
                ugOrderStoresBy.DataSource = dtdcf;
                // END TT#1966-MD - AGallagher - DC Fulfillment

                //BEGIN TT#46-MD -jsobek -Develop My Activity Log
                if (dr["ACTIVITY_MESSAGE_UPPER_LIMIT"] != DBNull.Value)
                {
                    _myActivityMessageUpperLimit = (int)dr["ACTIVITY_MESSAGE_UPPER_LIMIT"];
                }
                this.txtMyActivityMessageUpperLimit.Text = _myActivityMessageUpperLimit.ToString();
                //END TT#46-MD -jsobek -Develop My Activity Log

				if(dr["NEW_STORE_TIMEFRAME_BEGIN"] != DBNull.Value)
					txtNewStorePeriodBegin.Text = dr["NEW_STORE_TIMEFRAME_BEGIN"].ToString();
				if(dr["NEW_STORE_TIMEFRAME_END"] != DBNull.Value)
					txtNewStorePeriodEnd.Text = dr["NEW_STORE_TIMEFRAME_END"].ToString();
				if(dr["NON_COMP_STORE_TIMEFRAME_BEGIN"] != DBNull.Value)
					txtNonCompPeriodBegin.Text = dr["NON_COMP_STORE_TIMEFRAME_BEGIN"].ToString();
				if(dr["NON_COMP_STORE_TIMEFRAME_END"] != DBNull.Value)
					txtNonCompPeriodEnd.Text = dr["NON_COMP_STORE_TIMEFRAME_END"].ToString();

				// Header Link Characteristic
				HeaderCharacteristicsData headerCharData = new HeaderCharacteristicsData();
				DataTable dtHeaderLinkChar = headerCharData.HeaderCharGroup_Read();
				// add row for none
				DataRow row = dtHeaderLinkChar.NewRow();
				row["HCG_RID"] = Include.NoRID;
				row["HCG_ID"] = MIDText.GetTextOnly(eMIDTextCode.lbl_None);
				dtHeaderLinkChar.Rows.InsertAt(row, 0);
				dtHeaderLinkChar.AcceptChanges();
				cboHeaderLinkChar.DataSource = dtHeaderLinkChar;
				cboHeaderLinkChar.DisplayMember = "HCG_ID";
				cboHeaderLinkChar.ValueMember = "HCG_RID";
				_cboHeaderLinkCharValue = (dr["HEADER_LINK_CHARACTERISTIC"] == DBNull.Value) ? 
					Include.NoRID : Convert.ToInt32(dr["HEADER_LINK_CHARACTERISTIC"], CultureInfo.CurrentUICulture);
				// setting to the new <None> row by value fails so set by index in that case
				if (_cboHeaderLinkCharValue > Include.NoRID)
				{
					// setting to the only real row by value fails so set by index in that case
					if (dtHeaderLinkChar.Rows.Count == 2)
					{
						cboHeaderLinkChar.SelectedIndex = 1;
					}
					else
					{
						cboHeaderLinkChar.SelectedValue = _cboHeaderLinkCharValue;
					}
				}
				else
				{
					cboHeaderLinkChar.SelectedIndex = 0;
				}

				// Plan Versions
				ForecastVersion forVersion = new ForecastVersion();
				_dtFV = forVersion.GetForecastVersions(true);

				//make DESCRIPTION column the primary key
				DataColumn[] PrimaryKeyColumn = new DataColumn[1];
				PrimaryKeyColumn[0] = _dtFV.Columns["DESCRIPTION"];
				_dtFV.PrimaryKey = PrimaryKeyColumn;

				AddCheckboxColumns();
				UpdateCheckboxColumns();
				//Begin Track #4457 - JSmith - Add forecast versions
				AddValueLists();
				//End Track #4457

				uGridVersions.DataSource = _dtFV;

				uGridVersions.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortSingle;

				uGridVersions.DisplayLayout.AddNewBox.Hidden = false;
				uGridVersions.DisplayLayout.AddNewBox.Prompt = "Add new";
				uGridVersions.DisplayLayout.Bands[0].Columns["FV_RID"].Hidden = true;
				uGridVersions.DisplayLayout.Bands[0].Columns["FV_ID"].Hidden = true;
				uGridVersions.DisplayLayout.Bands[0].Columns["PROTECT_HISTORY_IND"].Hidden = true;
				uGridVersions.DisplayLayout.Bands[0].Columns["ACTIVE_IND"].Hidden = true;
				//Begin Track #4457 - JSmith - Add forecast versions
				uGridVersions.DisplayLayout.Bands[0].Columns["CURRENT_BLEND_IND"].Hidden = true;
				//Begin Track #4547 - JSmith - Add similar stores by forecast versions
				uGridVersions.DisplayLayout.Bands[0].Columns["SIMILAR_STORE_IND"].Hidden = true;
				//End Track #4547
	
//				uGridVersions.DisplayLayout.Bands[0].Columns["DESCRIPTION"].Header.Caption = "Description";
				uGridVersions.DisplayLayout.Bands[0].Columns["DESCRIPTION"].Header.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_FV_Description);
				uGridVersions.DisplayLayout.Bands[0].Columns["DESCRIPTION"].Width = 150;
//				uGridVersions.DisplayLayout.Bands[0].Columns["PROTECT_HISTORY_IND_B"].Header.Caption = "Protect History";
				uGridVersions.DisplayLayout.Bands[0].Columns["PROTECT_HISTORY_IND_B"].Header.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_FV_ProtectHistory);
//				uGridVersions.DisplayLayout.Bands[0].Columns["ACTIVE_IND_B"].Header.Caption = "Active";
				uGridVersions.DisplayLayout.Bands[0].Columns["ACTIVE_IND_B"].Header.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_FV_Active);
				uGridVersions.DisplayLayout.Bands[0].Columns["ACTIVE_IND_B"].Width = 80;
				uGridVersions.DisplayLayout.Bands[0].Columns["BLEND_TYPE"].Header.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_FV_Combine);
				uGridVersions.DisplayLayout.Bands[0].Columns["ACTUAL_FV_RID"].Header.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_FV_CombineActual);
				uGridVersions.DisplayLayout.Bands[0].Columns["FORECAST_FV_RID"].Header.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_FV_CombineForecast);
				uGridVersions.DisplayLayout.Bands[0].Columns["CURRENT_BLEND_IND_B"].Header.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_FV_CombineCurrentMonth);
				//Begin Track #4547 - JSmith - Add similar stores by forecast versions
				uGridVersions.DisplayLayout.Bands[0].Columns["SIMILAR_STORE_IND_B"].Header.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_FV_SimilarStore);
				//End Track #4547

				int col = uGridVersions.DisplayLayout.Bands[0].Columns["ACTIVE_IND_B"].Header.VisiblePosition;
				++col;
				//Begin Track #4547 - JSmith - Add similar stores by forecast versions
				uGridVersions.DisplayLayout.Bands[0].Columns["SIMILAR_STORE_IND_B"].Header.VisiblePosition = col;
				++col;
				//End Track #4547
				uGridVersions.DisplayLayout.Bands[0].Columns["BLEND_TYPE"].Header.VisiblePosition = col;
				++col;
				uGridVersions.DisplayLayout.Bands[0].Columns["ACTUAL_FV_RID"].Header.VisiblePosition = col; 
				++col;
				uGridVersions.DisplayLayout.Bands[0].Columns["FORECAST_FV_RID"].Header.VisiblePosition = col; 
				++col;
				uGridVersions.DisplayLayout.Bands[0].Columns["CURRENT_BLEND_IND_B"].Header.VisiblePosition = col; 

				uGridVersions.DisplayLayout.Bands[0].Columns["BLEND_TYPE"].AutoEdit = true;
				uGridVersions.DisplayLayout.Bands[0].Columns["BLEND_TYPE"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
				uGridVersions.DisplayLayout.Bands[0].Columns["BLEND_TYPE"].ValueList = uGridVersions.DisplayLayout.ValueLists["BLEND_TYPE"];

				uGridVersions.DisplayLayout.Bands[0].Columns["ACTUAL_FV_RID"].AutoEdit = true;
				uGridVersions.DisplayLayout.Bands[0].Columns["ACTUAL_FV_RID"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
				uGridVersions.DisplayLayout.Bands[0].Columns["ACTUAL_FV_RID"].ValueList = uGridVersions.DisplayLayout.ValueLists["ACTUAL_FV"];

				uGridVersions.DisplayLayout.Bands[0].Columns["FORECAST_FV_RID"].AutoEdit = true;
				uGridVersions.DisplayLayout.Bands[0].Columns["FORECAST_FV_RID"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
				uGridVersions.DisplayLayout.Bands[0].Columns["FORECAST_FV_RID"].ValueList = uGridVersions.DisplayLayout.ValueLists["FORECAST_FV"];

				SetVersionRowCharacteristics();
				//End Track #4457


				// Allocation Defaults
				txtPctNeedLimit.Text = dr["DEFAULT_PCT_NEED_LIMIT"].ToString();
				txtPctBalTolerance.Text = dr["DEFAULT_BALANCE_TOLERANCE"].ToString();
				txtPackDevTolerance.Text = dr["DEFAULT_PACK_SIZE_ERROR_PCT"].ToString();
				txtPackNeedTolerance.Text = dr["DEFAULT_MAX_SIZE_ERROR_PCT"].ToString();
				txtFillSizeHoles.Text = dr["DEFAULT_FILL_SIZE_HOLES_PCT"].ToString();
				txtSGPeriod.Text = dr["STORE_GRADE_TIMEFRAME"].ToString();
				//			ckbSizeBreakout.Checked = (dr["SIZE_BREAKOUT_IND"].ToString() == "1");
				//			ckbSizeNeed.Checked = (dr["SIZE_NEED_IND"].ToString() == "1");
				//			ckbProtectHeaders.Checked = (dr["PROTECT_IF_HDRS_IND"].ToString() == "1");
				//			txtAlloc.Text = dr["PURGE_ALLOCATIONS"].ToString();
				//			txtReserveStore.Text = dr["RESERVE_ST_RID"].ToString();
				//ckbUseWindowsLogin.Checked = (dr["USE_WINDOWS_LOGIN"].ToString() == "1");
				txtShippingHorizonWeeks.Text = dr["SHIPPING_HORIZON_WEEKS"].ToString();
                // BEGIN TT#616 - AGallagher - Allocation - Pack Rounding (#67)
                txtPct1stPackRoundUpFrom.Text = dr["GENERIC_PACK_ROUNDING_1ST_PACK_PCT"].ToString();
                txtPctNthPackRoundUpFrom.Text = dr["GENERIC_PACK_ROUNDING_NTH_PACK_PCT"].ToString();
                // END TT#616 - AGallagher - Allocation - Pack Rounding (#67)
				// Begin track #6074 stodd velocity changes
				// Begin TT # 91 - stodd
				//cbxDefaultGradesByBasis.Checked = Include.ConvertCharToBool(Convert.ToChar(dr["DEFAULT_GRADES_BY_BASIS_IND"]));
				// End TT # 91 - stodd
				// End track #6074 stodd
				// BEGIN MID Track #4826 - JSmith - Normalize Size Curves
				if (dr["NORMALIZE_SIZE_CURVES_IND"].ToString() == "1")
				{
					radNormalizeSizeCurves_Yes.Checked = true;
				}
				else
				{
					radNormalizeSizeCurves_No.Checked = true;
				}
				// END MID Track #4826
                // BEGIN TT#1401 - AGallagher - Reservation Stores
                bool _maxItemOverride;
                _maxItemOverride = Convert.ToBoolean(dr["ALLOW_STORE_MAX_VALUE_MODIFICATION"], CultureInfo.CurrentUICulture);
                if (_maxItemOverride == true)
                {
                    radItemMaxOverrideYes.Checked = true;
                }
                else
                {
                    radItemMaxOverrideNo.Checked = true;
                }
                // END TT#1401 - AGallagher - Reservation Stores
                // BEGIN  TT#1608-MD - srisch - Working 5.4 - Prior Header 05/27/2015 Needs Change
                bool _PriorHeader_IncludeReserve;
                _PriorHeader_IncludeReserve = Convert.ToBoolean(Convert.ToInt64( dr["PRIOR_HEADER_INCLUDE_RESERVE_IND"]), CultureInfo.CurrentUICulture);
                if (_PriorHeader_IncludeReserve == true)
                {
                    chkPriorHeaderIncludeReserve.Checked = true;
                }
                else
                { 
                    chkPriorHeaderIncludeReserve.Checked = false;
                }
                // END  TT#1608-MD - srisch - Working 5.4 - Prior Header 05/27/2015 
				// BEGIN MID Track #4921 - JSmith - A&F 666 - Size Modification
				eFillSizesToType fillSizesToType = (eFillSizesToType)dr["FILL_SIZES_TO_TYPE"];
				if (fillSizesToType == eFillSizesToType.SizePlan)
				{
					this.radFillSizesTo_SizePlan.Checked = true;
				}
                else if (fillSizesToType == eFillSizesToType.SizePlanWithMins) //TT#848-MD -jsobek -Fill to Size Plan Presentation
                {
                    this.radFillSizesTo_SizePlanWithSizeMins.Checked = true;
                }
				else
				{
					this.radFillSizesTo_Holes.Checked = true;
				}
				

				// begin MID Track 6335 Option to not Release Hdr with all units in reserve
				if (dr["ALLOW_RLSE_IF_ALL_IN_RSRV_IND"].ToString() == "1")
				{
					this.cboDoNotReleaseIfAllInReserve.Checked = false;
				}
				else
				{
					this.cboDoNotReleaseIfAllInReserve.Checked = true;
				}
				// end MID Track 6335 Option to not Release Hdr with all units in reserve

				// END MID Track #4921

                // Begin TT#3899 - JSmith - Remove the “Expand Headers When Intransit is Relieved” option on Headers Tab.
                //// Begin TT#628-MD - JSmith -  Add Option To Not Expand Allocation Records Once Relieved
                //if (dr["RI_EXPAND_IND"].ToString() == "1")
                //{
                //    this.cboRIExpand.Checked = true;
                //}
                //else
                //{
                //    this.cboRIExpand.Checked = false;
                //}
                //// End TT#628-MD - JSmith -  Add Option To Not Expand Allocation Records Once Relieved
                // End TT#3899 - JSmith - Remove the “Expand Headers When Intransit is Relieved” option on Headers Tab.

                //Begin TT#894-MD -jsobek -Single Client Instance System Option
                if (dr["FORCE_SINGLE_CLIENT_INSTANCE"].ToString() == "1")
                {
                    this.cbxForceSingleClientInstance.Checked = true;
                }
                else
                {
                    this.cbxForceSingleClientInstance.Checked = false;
                }
                //End TT#894-MD -jsobek -Single Client Instance System Option
                //Begin TT#898-MD -jsobek -Single User Instance System Option
                if (dr["FORCE_SINGLE_USER_INSTANCE"].ToString() == "1")
                {
                    this.cbxForceSingleUserInstance.Checked = true;
                }
                else
                {
                    this.cbxForceSingleUserInstance.Checked = false;
                }
                //End TT#898-MD -jsobek -Single User Instance System Option

                //Begin TT#1521-MD -jsobek -Active Directory Authentication
                if (dr["USE_WINDOWS_LOGIN"].ToString() == "1")
                {
                    this.radWindowsAuthentication.Checked = true;
                }
                else
                {
                    if (dr["USE_ACTIVE_DIRECTORY_AUTHENTICATION"].ToString() == "1")
                    {
                        this.radActiveDirectoryAuthentication.Checked = true;
                    }
                    else
                    {
                        if (dr["USE_ACTIVE_DIRECTORY_AUTHENTICATION_WITH_DOMAIN"].ToString() == "1")
                        {
                            this.radActiveDirectoryWithDomain.Checked = true;
                        }
                        else
                        {
                            this.radStandardAuthentication.Checked = true;
                        }
                    }
                }
                //End TT#1521-MD -jsobek -Active Directory Authentication


                //Begin TT#901-MD -jsobek -Batch Only Mode
                if (dr["USE_BATCH_ONLY_MODE"].ToString() == "1")
                {
                    this.cbxEnableRemoteSystemOptions.Checked = true;
                }
                else
                {
                    this.cbxEnableRemoteSystemOptions.Checked = false;
                }
                if (dr["CONTROL_SERVICE_DEFAULT_BATCH_ONLY_MODE_ON"].ToString() == "1")
                {
                    this.cbxControlServiceDefaultBatchOnlyModeOn.Checked = true;
                }
                else
                {
                    this.cbxControlServiceDefaultBatchOnlyModeOn.Checked = false;
                }
                //End TT#901-MD -jsobek -Batch Only Mode
                                
//				if (dr["SIZE_CURVE_CHARMASK"] 
//				txtSizeCurve.Text = dr["SIZE_CURVE_CHARMASK"].ToString();
//				txtSizeGroup.Text = dr["SIZE_GROUP_CHARMASK"].ToString();
//				txtSizeAlternates.Text = dr["SIZE_ALTERNATE_CHARMASK"].ToString();
//				txtSizeConstraints.Text = dr["SIZE_CONSTRAINT_CHARMASK"].ToString();
				SetSizeFilterControls(txtSizeCurve, cboSizeCurve, dr["SIZE_CURVE_CHARMASK"]);
				SetSizeFilterControls(txtSizeGroup, cboSizeGroup, dr["SIZE_GROUP_CHARMASK"]);
				SetSizeFilterControls(txtSizeAlternates, cboSizeAlternates, dr["SIZE_ALTERNATE_CHARMASK"]);
				SetSizeFilterControls(txtSizeConstraints, cboSizeConstraints, dr["SIZE_CONSTRAINT_CHARMASK"]);

				// Begin TT#391 - stodd -
				DataTable dtGenSizeCurveUsing = MIDText.GetTextType(eMIDTextType.eGenerateSizeCurveUsing, eMIDTextOrderBy.TextValue);
				cboGenerateSizeCurveUsing.DataSource = dtGenSizeCurveUsing;
				cboGenerateSizeCurveUsing.DisplayMember = "TEXT_VALUE";
				cboGenerateSizeCurveUsing.ValueMember = "TEXT_CODE";
				_generateSizeCurveUsing = (eGenerateSizeCurveUsing)(Convert.ToInt32(dr["GEN_SIZE_CURVE_USING"]));
				cboGenerateSizeCurveUsing.SelectedValue = _generateSizeCurveUsing;
				// End TT#391

                // BEGIN TT#246-MD - AGallagher - VSW Size - Add In-store Size Constraint options 
                DataTable dtVSWSizeConstrains = MIDText.GetTextType(eMIDTextType.eVSWSizeConstraints, eMIDTextOrderBy.TextCode);
                cboVSWSizeContraints.DataSource = dtVSWSizeConstrains;
                cboVSWSizeContraints.DisplayMember = "TEXT_VALUE";
                cboVSWSizeContraints.ValueMember = "TEXT_CODE";
                _vswSizeConstraints = (eVSWSizeConstraints)(Convert.ToInt32(dr["VSW_SIZE_CONSTRAINTS"]));
                cboVSWSizeContraints.SelectedValue = _vswSizeConstraints;
                // END TT#246-MD - AGallagher - VSW Size - Add In-store Size Constraint options 

                // BEGIN TT#933-MD - AGallagher - Item Max vs. FWOS Max
                _vswItemFWOSMax = (eVSWItemFWOSMax)(Convert.ToInt32(dr["VSW_ITEM_FWOS_MAX_IND"]));
                if (_vswItemFWOSMax == eVSWItemFWOSMax.Default)
                { this.radItemFWOSDefault.Checked = true; }
                if (_vswItemFWOSMax == eVSWItemFWOSMax.Highest)
                { this.radItemFWOSHighest.Checked = true; }
                if (_vswItemFWOSMax == eVSWItemFWOSMax.Lowest)
                { this.radItemFWOSLowest.Checked = true; }
                // END TT#933-MD - AGallagher - Item Max vs. FWOS Max

                // BEGIN TT#933-MD - AGallagher - Item Max vs. FWOS Max
                _vswItemFWOSMax = (eVSWItemFWOSMax)(Convert.ToInt32(dr["VSW_ITEM_FWOS_MAX_IND"]));
                if (_vswItemFWOSMax == eVSWItemFWOSMax.Default)
                { this.radItemFWOSDefault.Checked = true; }
                if (_vswItemFWOSMax == eVSWItemFWOSMax.Highest)
                { this.radItemFWOSHighest.Checked = true; }
                if (_vswItemFWOSMax == eVSWItemFWOSMax.Lowest)
                { this.radItemFWOSLowest.Checked = true; }
                // END TT#933-MD - AGallagher - Item Max vs. FWOS Max

                // begin TT#1365 - JEllis - FL Detail Pack Size Need Enhancement
                this.cbxNoMaxStep.Checked = (dr["PACK_TOLERANCE_NO_MAX_STEP_IND"].ToString() == "1");
                this.cbxStepped.Checked = (dr["PACK_TOLERANCE_STEPPED_IND"].ToString() == "1");
                // end TT#1365 - JEllis - FL Detail Pack Size Need Enhancement

                //Load Basis Labels-----------------------
                BasisLabelTypeProfile viewVarProf;
                ArrayList basisLabelTypeProfileList = new ArrayList();

                _varProfList = GetBasisLabelProfList(Convert.ToInt32(dr["SYSTEM_OPTION_RID"], CultureInfo.CurrentUICulture));
                dt = opts.GetBasisLabelInfo(Convert.ToInt32(dr["SYSTEM_OPTION_RID"], CultureInfo.CurrentUICulture));
                foreach (DataRow releaseRow in dt.Rows)
                {
                    int basisLabelType = Convert.ToInt32(releaseRow["LABEL_TYPE"], CultureInfo.CurrentUICulture);
                    BasisLabelVariableEntry blve = new BasisLabelVariableEntry(releaseRow);
                    viewVarProf = (BasisLabelTypeProfile)_varProfList.FindKey(basisLabelType);
                    blve.LabelText = Convert.ToString(viewVarProf.BasisLabelName);
                    basisLabelTypeProfileList.Add(blve);
                }

                _basisLabelSelectableVariableList = new ArrayList();

                LoadSelectableVariableList(basisLabelTypeProfileList);

                foreach (RowColProfileHeader rowColHdr in _selectableVariableList)
                {
                    _basisLabelSelectableVariableList.Add(rowColHdr.Copy());
                }

                _basisLabelVarChooser = new RowColChooserOrderPanel(_basisLabelSelectableVariableList, true, null);

                FRBLgroupBox.Controls.Add(_basisLabelVarChooser);
                FRBLgroupBox.Controls.SetChildIndex(_basisLabelVarChooser, 0);
                _basisLabelVarChooser.Dock = DockStyle.Fill;
                _basisLabelVarChooser.FillControl();

                _basisLabelVarChooser.Visible = true;

                // Default to All Stores
                _basisLabelVarChooser.UpdateData();


                // BEGIN MID Track #6043 - KJohnson - Forcast Chain WOS Calculations
                //---Load OTS Default-------------------------
                txtNumberOfWeeksWithZeroSales.Text = dr["NUMBER_OF_WEEKS_WITH_ZERO_SALES"].ToString();
                txtMaximumChainWOS.Text = dr["MAXIMUM_CHAIN_WOS"].ToString();
                ckbProrateChainStock.Checked = (dr["PRORATE_CHAIN_STOCK"].ToString() == "1");
                // END MID Track #6043 - KJohnson

                //--------------------------------------------

			// BEGIN MID Track #4357 - security for interfaced and non-interfaced headers
			if (_SAB.ClientServerSession.GlobalOptions.ProtectInterfacedHeadersInd)
			{
				cboProtectInterfacedHeaders.Checked = true;
			}
			else
			{
				cboProtectInterfacedHeaders.Checked = false;
			}
			// END MID Track #4357

            this.cbxEnableVelocityGradeOptions.Checked = (dr["ENABLE_VELOCITY_GRADE_OPTIONS"].ToString() == "1"); //TT#855-MD -jsobek -Velocity Enhancements

            // Begin TT#413 - JSmith - Add new Generic Size Curve names to correspond with the new Size Curves being generated within Node Properties settings.
            switch (_SAB.ClientServerSession.GlobalOptions.GenericSizeCurveNameType)
            {
                case eGenericSizeCurveNameType.HeaderCharacteristic:
                    radGenericSizeCurveName_HeaderCharacteristics.Checked = true;
                    break;
                default:
                    radGenericSizeCurveName_NodePropertiesName.Checked = true;
                    break;
            }
            // End TT#413

				if (_securityCompanyInfo.IsReadOnly &&
					_securityDisplayOptions.IsReadOnly &&
					_securityOTSPlanVersions.IsReadOnly &&
					//Begin Track #3784 - JScott - Add security for Header Gloabl Options
					//				_securityAllocationDefaults.IsReadOnly)
					_securityAllocationDefaults.IsReadOnly &&
					_securityAllocationHeaders.IsReadOnly &&
					// Begin Track #6240 stodd
					_securityBasisLabels.IsReadOnly && _securityOTSDefaults.IsReadOnly)
					//End Track #3784 - JScott - Add security for Header Gloabl Options
					// End Track #6240 stodd
				{
					Format_Title(eDataState.ReadOnly, eMIDTextCode.frm_Administration, MIDText.GetTextOnly(eMIDTextCode.frm_GlobalOptions));
					btnOK.Visible = false;
					SetReadOnly(false);  //Security changes - 1/24/2005 vg
                    

                    //Begin TT#506-MD -jsobek -Add infrastructure to allow Email to be sent from the application
                    this.SMTP_Options.LoadFromDataRow(dr);
                    this.SMTP_Control.Load_UI_From_BL(this.SMTP_Options);
                    this.SMTP_Control.SetReadOnly();
                    //End TT#506-MD -jsobek -Add infrastructure to allow Email to be sent from the application
				}
				else
				{
					SetReadOnly(true);
					Format_Title(eDataState.Updatable, eMIDTextCode.frm_Administration, MIDText.GetTextOnly(eMIDTextCode.frm_GlobalOptions));
					btnOK.Visible = true;

                    //Begin TT#506-MD -jsobek -Add infrastructure to allow Email to be sent from the application
                    //Ensure certain SMTP controls stay disabled by default since SetReadOnly function blindly ignores default settings
                    this.SMTP_Control.DoEnable();
                    this.SMTP_Options.LoadFromDataRow(dr);
                    this.SMTP_Control._SAB = _SAB;
                    this.SMTP_Control.Load_UI_From_BL(this.SMTP_Options);
                    //End TT#506-MD -jsobek -Add infrastructure to allow Email to be sent from the application

                    //BEGIN TT#3509-VStuart-Cannot delete OTS Plan Version-ANFUser
				    this.btnInUse.Visible = false;
                    //END  TT#3509-VStuart-Cannot delete OTS Plan Version-ANFUser

					if (_securityCompanyInfo.IsReadOnly)
					{
						this.tabCompInfo.Text += " [Read Only]";
						SetControlReadOnly(this.tabCompInfo, !_securityCompanyInfo.AllowUpdate);
                        //Begin TT#506-MD -jsobek -Add infrastructure to allow Email to be sent from the application
                        this.SMTP_Options.LoadFromDataRow(dr);
                        this.SMTP_Control.Load_UI_From_BL(this.SMTP_Options);
                        this.SMTP_Control.SetReadOnly();
                        //End TT#506-MD -jsobek -Add infrastructure to allow Email to be sent from the application
					}
					else if (_securityCompanyInfo.AccessDenied)
					{
						this.tabControl1.Controls.Remove(this.tabCompInfo);
					}

					if (_securityDisplayOptions.IsReadOnly)
					{
						this.tabStores.Text += " [Read Only]";
						SetControlReadOnly(this.tabStores, !_securityDisplayOptions.AllowUpdate);
					}
					else if (_securityDisplayOptions.AccessDenied)
					{
						this.tabControl1.Controls.Remove(this.tabStores);
					}

					if (_securityOTSPlanVersions.IsReadOnly)
					{
						this.tabOTS.Text += " [Read Only]";
						SetControlReadOnly(this.tabOTS, !_securityOTSPlanVersions.AllowUpdate);
					}
					else if (_securityOTSPlanVersions.AccessDenied)
					{
						this.tabControl1.Controls.Remove(this.tabOTS);
					}

					//Begin Track #3784 - JScott - Add security for Header Gloabl Options
					//				if (_securityAllocationDefaults.IsReadOnly)
					//				{
					//					this.tabAllocDefault.Text += " [Read Only]";
					//					SetControlReadOnly(this.tabAllocDefault, !_securityAllocationDefaults.AllowUpdate);
					//					this.tabAllocHeaders.Text += " [Read Only]";
					//					SetControlReadOnly(this.tabAllocHeaders, !_securityAllocationDefaults.AllowUpdate);
					//				}
					//				else if (_securityAllocationDefaults.AccessDenied)
					//				{
					//					this.tabControl1.Controls.Remove(this.tabAllocDefault);
					//					this.tabControl1.Controls.Remove(this.tabAllocHeaders);
					//				}
					if (_securityAllocationDefaults.IsReadOnly)
					{
						this.tabAllocDefault.Text += " [Read Only]";
						SetControlReadOnly(this.tabAllocDefault, !_securityAllocationDefaults.AllowUpdate);
					}
					else if (_securityAllocationDefaults.AccessDenied)
					{
						this.tabControl1.Controls.Remove(this.tabAllocDefault);
					}

					if (_securityAllocationHeaders.IsReadOnly)
					{
						this.tabAllocHeaders.Text += " [Read Only]";
						SetControlReadOnly(this.tabAllocHeaders, !_securityAllocationHeaders.AllowUpdate);
					}
					else if (_securityAllocationHeaders.AccessDenied)
					{
						this.tabControl1.Controls.Remove(this.tabAllocHeaders);
					}
					//End Track #3784 - JScott - Add security for Header Gloabl Options

					// Begin Track #6240 stodd
					if (_securityBasisLabels.IsReadOnly)
					{
						Debug.WriteLine("BASIS LABELS");
						this.tabBasisLabels.Text += " [Read Only]";
						SetControlReadOnly(this.tabBasisLabels, !_securityBasisLabels.AllowUpdate);
						this._basisLabelVarChooser.Enabled = false;
					}
					else if (_securityBasisLabels.AccessDenied)
					{
						this.tabControl1.Controls.Remove(this.tabBasisLabels);
					}

					if (_securityOTSDefaults.IsReadOnly)
					{
						this.tabOTSDefaults.Text += " [Read Only]";
						SetControlReadOnly(this.tabOTSDefaults, !_securityOTSDefaults.AllowUpdate);
					}
					else if (_securityOTSDefaults.AccessDenied)
					{
						this.tabControl1.Controls.Remove(this.tabOTSDefaults);
					}
					// End track #6240 stodd
				}

				LoadHeaderReleaseFlags();

				FormLoaded = true;
			}
			catch 
			{
				throw;
			}
		}

        private void LoadSelectableVariableList(ArrayList dtBasisLabelVariables)
        {
            Hashtable varKeyHash;
            BasisLabelTypeProfile viewVarProf;
            BasisLabelVariableEntry varEntry;

            try
            {
                varKeyHash = new Hashtable();

                foreach (BasisLabelVariableEntry viewVarEntry in dtBasisLabelVariables)
                {
                    viewVarProf = (BasisLabelTypeProfile)_varProfList.FindKey(viewVarEntry.LabelType);

                    if (viewVarProf != null)
                    {
                        varKeyHash.Add(viewVarProf.Key, viewVarEntry);
                    }
                }

                _selectableVariableList = new ArrayList();

                foreach (BasisLabelTypeProfile varProf in _varProfList)
                {
                    varEntry = (BasisLabelVariableEntry)varKeyHash[varProf.Key];

                    if (varEntry != null)
                    {
                        _selectableVariableList.Add(new RowColProfileHeader(varProf.BasisLabelName, true, Convert.ToInt32(varEntry.LabelSequence), varProf));
                    }
                    else
                    {
                        _selectableVariableList.Add(new RowColProfileHeader(varProf.BasisLabelName, false, -1, varProf));
                    }
                }
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

        private ProfileList GetBasisLabelProfList(int systemOptionRID)
        {
            ProfileList basisLabelList = new ProfileList(eProfileType.BasisLabelType);

            Array values;
            string[] names;

            values = System.Enum.GetValues(typeof(eBasisLabelType));
            names = System.Enum.GetNames(typeof(eBasisLabelType));

            for (int i = 0; i < names.Length; i++)
            {
                BasisLabelTypeProfile bltp = new BasisLabelTypeProfile(i);
                bltp.BasisLabelSystemOptionRID = systemOptionRID;
                bltp.BasisLabelName = names[i].Replace("_", " ");
                bltp.BasisLabelType = i;
                bltp.BasisLabelSequence = -1;

                basisLabelList.Add(bltp);
            }
            return basisLabelList;
        }

		private void SetSizeFilterTextBox(TextBox aTextBox, object aTextValue)
		{
			try
			{
				aTextBox.Enabled = (aTextValue == DBNull.Value || aTextValue.ToString()  == string.Empty) ? false : true; 
			}
			catch 
			{
				throw;
			}
		}	

		private void SetSizeFilterControls(TextBox aTextBox, CheckBox aCheckBox, object aTextValue)
		{
			try
			{
				aTextBox.Text =  aTextValue.ToString();
				aTextBox.Enabled = (aTextValue == DBNull.Value || aTextValue.ToString()  == string.Empty) ? false : true; 
				aCheckBox.Checked = (aTextBox.Text == null || aTextBox.Text == string.Empty) ? false : true; 
			}
			catch 
			{
				throw;
			}
		}	
	
		private void AddCheckboxColumns()
		{
			DataColumn newColumn = new DataColumn();
			newColumn.AllowDBNull = false; 
			newColumn.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_FV_ProtectHistory); 
			newColumn.ColumnName = "PROTECT_HISTORY_IND_B"; 
			newColumn.DefaultValue = true;
			newColumn.ReadOnly = false;
			newColumn.DataType = System.Type.GetType("System.Boolean"); 	
			_dtFV.Columns.Add(newColumn); 

			newColumn = new DataColumn();
			newColumn.AllowDBNull = false; 
			newColumn.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_FV_Active); 
			newColumn.ColumnName = "ACTIVE_IND_B"; 
			newColumn.DefaultValue = true;
			newColumn.ReadOnly = false;
			newColumn.DataType = System.Type.GetType("System.Boolean"); 	
			_dtFV.Columns.Add(newColumn); 

//Begin Track #4547 - JSmith - Add similar stores by forecast versions
			newColumn = new DataColumn();
			newColumn.AllowDBNull = false; 
			newColumn.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_FV_SimilarStore); 
			newColumn.ColumnName = "SIMILAR_STORE_IND_B"; 
			newColumn.DefaultValue = true;
			newColumn.ReadOnly = false;
			newColumn.DataType = System.Type.GetType("System.Boolean"); 	
			_dtFV.Columns.Add(newColumn); 
//End Track #4547

//Begin Track #4457 - JSmith - Add forecast versions
			newColumn = new DataColumn();
			newColumn.AllowDBNull = false; 
			newColumn.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_FV_CombineCurrentMonth); 
			newColumn.ColumnName = "CURRENT_BLEND_IND_B"; 
			newColumn.DefaultValue = false;
			newColumn.ReadOnly = false;
			newColumn.DataType = System.Type.GetType("System.Boolean"); 	
			_dtFV.Columns.Add(newColumn); 
//End Track #4457
		}


		private void UpdateCheckboxColumns()
		{
			foreach(DataRow dr in _dtFV.Rows)
			{
				if ((string)dr["PROTECT_HISTORY_IND"] == "1")
					dr["PROTECT_HISTORY_IND_B"] = true;
				else
					dr["PROTECT_HISTORY_IND_B"] = false;
				if ((string)dr["ACTIVE_IND"] == "1")
					dr["ACTIVE_IND_B"] = true;
				else
					dr["ACTIVE_IND_B"] = false;
				//Begin Track #4547 - JSmith - Add similar stores by forecast versions
				if ((string)dr["SIMILAR_STORE_IND"] == "1")
					dr["SIMILAR_STORE_IND_B"] = true;
				else
					dr["SIMILAR_STORE_IND_B"] = false;
				//End Track #4547
				//Begin Track #4457 - JSmith - Add forecast versions
				if ((string)dr["CURRENT_BLEND_IND"] == "1")
					dr["CURRENT_BLEND_IND_B"] = true;
				else
					dr["CURRENT_BLEND_IND_B"] = false;
				//End Track #4457
			}
			_dtFV.AcceptChanges();
		}

		//Begin Track #4457 - JSmith - Add forecast versions
		private void AddValueLists()
		{
			uGridVersions.DisplayLayout.ValueLists.Clear();
			uGridVersions.DisplayLayout.ValueLists.Add("BLEND_TYPE");
			uGridVersions.DisplayLayout.ValueLists.Add("ACTUAL_FV");
			uGridVersions.DisplayLayout.ValueLists.Add("FORECAST_FV");

			DataTable dt = MIDText.GetTextType(eMIDTextType.eForecastBlendType, eMIDTextOrderBy.TextCode);
			foreach (DataRow dr in dt.Rows)
			{
				uGridVersions.DisplayLayout.ValueLists["BLEND_TYPE"].ValueListItems.Add(Convert.ToInt32(dr["TEXT_CODE"], CultureInfo.CurrentUICulture), dr["TEXT_VALUE"].ToString());
			}

			foreach (DataRow dr in _dtFV.Rows)
			{
				if (Convert.ToInt32(dr["FV_RID"]) == Include.FV_ActualRID ||
					Convert.ToInt32(dr["FV_RID"]) == Include.FV_ModifiedRID)
				{
					uGridVersions.DisplayLayout.ValueLists["ACTUAL_FV"].ValueListItems.Add(Convert.ToInt32(dr["FV_RID"], CultureInfo.CurrentUICulture), dr["DESCRIPTION"].ToString());
				}
				else
				{
					uGridVersions.DisplayLayout.ValueLists["FORECAST_FV"].ValueListItems.Add(Convert.ToInt32(dr["FV_RID"], CultureInfo.CurrentUICulture), dr["DESCRIPTION"].ToString());
				}
			}
		}
		//End Track #4457

		private void LoadHeaderReleaseFlags()
		{
			try
			{
				bool addType;
				clbHeaderTypeRelease.Items.Clear();
                // Begin TT#1043 – JSmith - Assortment and Placeholder show as header types when should not.
                //DataTable headerTypes = MIDText.GetTextType(eMIDTextType.eHeaderType, eMIDTextOrderBy.TextValue);
                // Begin TT#1966-MD - JSmith - DC Fulfillment
                //DataTable headerTypes = MIDText.GetTextType(eMIDTextType.eHeaderType, eMIDTextOrderBy.TextValue, Convert.ToInt32(eHeaderType.Assortment), Convert.ToInt32(eHeaderType.Placeholder));
                DataTable headerTypes = MIDText.GetTextType(eMIDTextType.eHeaderType, eMIDTextOrderBy.TextValue, Convert.ToInt32(eHeaderType.Assortment), Convert.ToInt32(eHeaderType.Placeholder), Convert.ToInt32(eHeaderType.Master));
                // End TT#1966-MD - JSmith - DC Fulfillment
                // End TT#1043
				foreach (DataRow dr in headerTypes.Rows)
				{
					addType = true;
					int headerType = (Convert.ToInt32(dr["TEXT_CODE"], CultureInfo.CurrentUICulture));
					string headerTypeName = (Convert.ToString(dr["TEXT_VALUE"], CultureInfo.CurrentUICulture));
					// begin MID Track 3891 Remove WorkUpSizeBuy (now part of WorkUpTotalBuy)
					//if (!_SAB.ClientServerSession.GlobalOptions.AppConfig.SizeInstalled)
					//{
					//	eSizeHeaderType sizeHeaderType = (eSizeHeaderType)headerType;
					//	if (Enum.IsDefined(typeof(eSizeHeaderType),sizeHeaderType))
					//	{
					//		addType = false;
					//	}
					//}

					// remove until ready
					//eHeaderType tmpHeaderType = (eHeaderType)headerType;
					//if (tmpHeaderType == eHeaderType.MultiHeader ||
					//	tmpHeaderType == eHeaderType.WorkupSizeBuy)
					//{
					//	addType = false;
					//}
					// end MID Track 3891 Remove WorkUpSizeBuy (now part of WorkUpTotalBuy)

					if (addType)
					{
						HeaderTypeProfile hrp = (HeaderTypeProfile)_SAB.ClientServerSession.GlobalOptions.HeaderTypeProfileList.FindKey(headerType);
						if (hrp == null)
						{
							hrp = new HeaderTypeProfile(headerType);
							hrp.HeaderTypeName = headerTypeName;
							hrp.ReleaseHeaderType = true;
						}
		
						CheckState checkState = CheckState.Unchecked;
						if (hrp.ReleaseHeaderType)
						{
							checkState = CheckState.Checked;
						}
						clbHeaderTypeRelease.Items.Add(hrp, checkState);
					}
				}
			}
			catch
			{
				throw;
			}
		}

		private void btnOK_Click(object sender, System.EventArgs e)
		{
			SaveChanges();
			if (!ErrorFound)
			{
				_SAB.ClientServerSession.RefreshGlobalOptions();
                _SAB.ApplicationServerSession.RefreshGlobalOptions();   // TT#78 - Ron Matelic - Header Char delete issue

				this.Close();
			}
		}

		override protected bool SaveChanges()
		{
			bool globalOptionsValid = true;
			ErrorFound = false;
			// BEGIN MID Track #4921 - JSmith - A&F 666 - Size Modification
			eFillSizesToType fillSizesToType;
			if (this.radFillSizesTo_SizePlan.Checked)
			{
				fillSizesToType = eFillSizesToType.SizePlan;
			}
            else if (this.radFillSizesTo_SizePlanWithSizeMins.Checked) //TT#848-MD -jsobek -Fill to Size Plan Presentation
            {
                fillSizesToType = eFillSizesToType.SizePlanWithMins;
            }
			else
			{
				fillSizesToType = eFillSizesToType.Holes;
			}
			// End MID Track #4921

            // Begin TT#413 - JSmith - Add new Generic Size Curve names to correspond with the new Size Curves being generated within Node Properties settings.
            eGenericSizeCurveNameType genericSizeCurveNameType;
			if (this.radGenericSizeCurveName_NodePropertiesName.Checked)
			{
				genericSizeCurveNameType = eGenericSizeCurveNameType.NodePropertiesName;
			}
			else
			{
				genericSizeCurveNameType = eGenericSizeCurveNameType.HeaderCharacteristic;
			}
            // End TT#413
            // BEGIN TT#1401 - AGallagher - Reservation Stores
            if (radItemMaxOverrideYes.Checked == true)
            {
                _priorHeaderIncludeReserve = true;
            }
            else
            {
                _priorHeaderIncludeReserve = false;
            }
            // END TT#1401 - AGallagher - Reservation Stores

            // BEGIN TT#1608-MD - srisch - Working 5.4 - Prior Header 05/27/2015 
            if (chkPriorHeaderIncludeReserve.Checked == true)
            {
                _priorHeaderIncludeReserve = true;
            }
            else
            {
                _priorHeaderIncludeReserve = false;
            }
            // END TT#1608-MD - srisch - Working 5.4 - Prior Header 05/27/2015 
			//validate current tab before saving.  Other tabs were validated before allowing the tab to change
			switch (_currentTabPage.Name)
			{
				case "tabCompInfo":
					globalOptionsValid = ValidateTabCompInfo();
					break;
				case "tabStores":
					globalOptionsValid = ValidateTabDisplayOptions();
					break;
				case "tabOTS":
					globalOptionsValid = ValidateTabOTS();
					break;
				case "tabAllocDefault":
					globalOptionsValid = ValidateTabAllocDefault();
					break;
                case "tabBasisLabels":
                    globalOptionsValid = ValidateTabBasisLabels();
                    break;
            }
			
			if (globalOptionsValid)
			{
				GlobalOptions opts = new GlobalOptions();
				try
				{
                    this.SMTP_Control.Save_UI_To_BL(this.SMTP_Options);  //TT#506-MD -jsobek -Add infrastructure to allow Email to be sent from the application


		//			int shippingHorizonWeeks = 0;  
					opts.OpenUpdateConnection();
                    // BEGIN TT#1966-MD - AGallagher - DC Fulfillment
                    if (_DCFChangePending)
                    {
                        _DCFChangePending = false;
                        opts.GetDCFStoreOrderInfo_Delete_All(1);

                        int _SEQ;
                        string _DIST_CENTER;
                        int _SCG_RID;

                        foreach (DataRow drdcf in dtdcf.Rows)
                        {
                            if (drdcf.RowState != DataRowState.Deleted &&
                                drdcf.RowState != DataRowState.Detached)
                            {
                                _SEQ = Convert.ToInt32(drdcf["SEQ"], CultureInfo.CurrentUICulture);
                                _DIST_CENTER = Convert.ToString(drdcf["DIST_CENTER"], CultureInfo.CurrentUICulture);
                                _SCG_RID = Convert.ToInt32(drdcf["SCG_RID"], CultureInfo.CurrentUICulture);

                                opts.GetDCFStoreOrderInfo_Insert(1, _SEQ, _DIST_CENTER, _SCG_RID);
                            }
                        } 
                        
                        opts.OpenUpdateConnection();
                    }

                    // converting cboVSWSizeContraints.SelectedValue to eVSWSizeConstraints value
                    eVSWSizeConstraints selectedVSWSizeConstraints = eVSWSizeConstraints.None;
                    int selectedIntVSWSizeConstraints;
                    if(cboVSWSizeContraints != null && cboVSWSizeContraints.SelectedValue != null &&
                        int.TryParse(cboVSWSizeContraints.SelectedValue.ToString(), out selectedIntVSWSizeConstraints))
                    {
                        if (Enum.IsDefined(typeof(eVSWSizeConstraints), selectedIntVSWSizeConstraints))
                        {
                            selectedVSWSizeConstraints = (eVSWSizeConstraints)selectedIntVSWSizeConstraints;
                        }
                    }

                    // END TT#1966-MD - AGallagher - DC Fulfillment
                    opts.UpdateGlobalOptions(
						txtCompany.Text,
                        txtStreet.Text,
                        txtCity.Text,
                        _cboStateValue,
						txtZip.Value,
                        txtPhone.Value,
                        txtFax.Value,
                        txtEmail.Text,
						0,
						_cboStoreValue,
                        _cboPlanStoreAttrValue,
                        _cboAllocStoreAttrValue,
						txtNewStorePeriodBegin.Text.Trim(), 
                        txtNewStorePeriodEnd.Text.Trim(), 
                        txtNonCompPeriodBegin.Text.Trim(), 
                        txtNonCompPeriodEnd.Text.Trim(), 
						_cboProductValue,
						txtPctNeedLimit.Text.Trim(),
                        txtPctBalTolerance.Text.Trim(),
                        txtPackDevTolerance.Text.Trim(),
						txtPackNeedTolerance.Text.Trim(),
                        txtFillSizeHoles.Text.Trim(),
                        // BEGIN TT#616 - AGallagher - Allocation - Pack Rounding (#67)
                        txtPct1stPackRoundUpFrom.Text.Trim(),
                        txtPctNthPackRoundUpFrom.Text.Trim(),
                        // END TT#616 - AGallagher - Allocation - Pack Rounding (#67)
						//true, true, false, RonM keep the db settings for non modifiable bools 
						_SAB.ClientServerSession.GlobalOptions.SizeBreakoutInd,
						_SAB.ClientServerSession.GlobalOptions.AppConfig.SizeInstalled,
						_SAB.ClientServerSession.GlobalOptions.BulkIsDetail,
						Convert.ToInt32(txtSGPeriod.Text, CultureInfo.CurrentUICulture), 
						//true,
						_SAB.ClientServerSession.GlobalOptions.ProtectInterfacedHeadersInd,
						string.Empty,
						this.radWindowsAuthentication.Checked, //ckbUseWindowsLogin.Checked, 
				        Convert.ToInt32(txtShippingHorizonWeeks.Text, CultureInfo.CurrentUICulture),
						Convert.ToChar(txtProductLevelDelimiter.Text, CultureInfo.CurrentUICulture),
						_cboHeaderLinkCharValue, 
						txtSizeCurve.Text.Trim(),
					    txtSizeGroup.Text.Trim(),
		                txtSizeAlternates.Text.Trim(),
						// BEGIN MID Track #4826 - JSmith - Normalize Size Curves
//						txtSizeConstraints.Text.Trim() 
						txtSizeConstraints.Text.Trim(),
						// BEGIN MID Track #4921 - JSmith - A&F 666 - Size Modification
						this.radNormalizeSizeCurves_Yes.Checked,
						// BEGIN MID Track #6335 Option to not Release Hdr with all units in reserve
						Convert.ToInt32(fillSizesToType),
                      	!this.cboDoNotReleaseIfAllInReserve.Checked,
						// END MID Track #6335 Option to not Release Hdr with all units in reserve
						// End MID Track #4921
						// END MID Track #4826
				//	shippingHorizonWeeks
                        // BEGIN MID Track #6043 - KJohnson - Forcast Chain WOS Calculations
                        Convert.ToInt32(txtNumberOfWeeksWithZeroSales.Text, CultureInfo.CurrentUICulture),
                        Convert.ToInt32(txtMaximumChainWOS.Text, CultureInfo.CurrentUICulture),
                        // Begin TT#413 - JSmith - Add new Generic Size Curve names to correspond with the new Size Curves being generated within Node Properties settings.
                        ckbProrateChainStock.Checked,
						// END MID Track #6043 - KJohnson
						// Begin Track #6074 stodd velocity changes
						// Begin TT # 91 - stodd
						//cbxDefaultGradesByBasis.Checked
						// End TT # 91 - stodd
						// End Track #6074
                        genericSizeCurveNameType.GetHashCode(),
                        // End TT#413
   						_generateSizeCurveUsing, // TT#1365 - JEllis - FL Detail Pack Size Need Enhancement
                        cbxNoMaxStep.Checked,    // TT#1365 - JEllis - FL Detail Pack Size Need Enhancement
                        cbxStepped.Checked,       // TT#1365 - JEllis - FL Detail Pack Size Need Enhancement
                        // Begin TT#3899 - JSmith - Remove the “Expand Headers When Intransit is Relieved” option on Headers Tab.
                        //cboRIExpand.Checked,  // TT#628-MD - JSmith -  Add Option To Not Expand Allocation Records Once Relieved
                        true,  // default value since field is no longer used
                        // End TT#3899 - JSmith - Remove the “Expand Headers When Intransit is Relieved” option on Headers Tab.
                        // BEGIN TT#1401 - AGallagher - Reservation Stores
                        _maxItemOverride,
                        // END TT#1401 - AGallagher - Reservation Stores
                        selectedVSWSizeConstraints.GetHashCode(),  // TT#246-MD - AGallagher - VSW Size - Add In-store Size Constraint options 
                        //BEGIN TT#46-MD -jsobek -Develop My Activity Log
                        _myActivityMessageUpperLimit,
                        //END TT#46-MD -jsobek -Develop My Activity Log
                        //Begin TT#506-MD -jsobek -Add infrastructure to allow Email to be sent from the application
                        this.SMTP_Options,
                        //End TT#506-MD -jsobek -Add infrastructure to allow Email to be sent from the application
						_SAB.ClientServerSession.GlobalOptions.IsStoreDeleteInProgress,	// TT#739-MD - STodd - delete stores
                        this.cbxEnableVelocityGradeOptions.Checked, //TT#855-MD -jsobek -Velocity Enhancements
                        this.cbxForceSingleClientInstance.Checked, //TT#894-MD -jsobek -Single Client Instance System Option
                        this.cbxForceSingleUserInstance.Checked, //TT#898-MD -jsobek -Single User Instance System Option
                        this.radActiveDirectoryAuthentication.Checked, //this.ckbUseActiveDirectoryAuthentication.Checked, 
                        this.radActiveDirectoryWithDomain.Checked, 
                        this.cbxEnableRemoteSystemOptions.Checked, //TT#901-MD -jsobek -Batch Only Mode
                        this.cbxControlServiceDefaultBatchOnlyModeOn.Checked, //TT#901-MD -jsobek -Batch Only Mode
                        _vswItemFWOSMax.GetHashCode(),  // TT#933-MD - AGallagher - Item Max vs. FWOS Max
                        this.chkPriorHeaderIncludeReserve.Checked,
						_cboDCCartonRoundDfltAttrValue,  // TT#1652-MD - RMatelic - DC Carton Rounding
                        _split_Option.GetHashCode(),
                        _apply_Minimums_Ind,
                        _prioritize_Type,
                        _header_Field,
                        _hcg_Rid,
                        _header_Order.GetHashCode(),
                        _store_Order.GetHashCode(),
                        _split_By_Option.GetHashCode(),
                        _split_By_Reserve.GetHashCode(),
                        _apply_By.GetHashCode(),
                        _within_Dc.GetHashCode(),
                        _useExternalEligibilityAllocation,
                        _useExternalEligibilityPlanning,
                        _externalEligibilityProductIdentifier.GetHashCode(),
                        _externalEligibilityChannelIdentifier.GetHashCode(),
                        _externalEligibilityURL
                        );

                   	opts.DeleteHeaderReleaseTypes();
					for (int i = 0; i < this.clbHeaderTypeRelease.Items.Count; i++)
					{
						HeaderTypeProfile hrp = (HeaderTypeProfile)clbHeaderTypeRelease.Items[i];
						bool release = false;
						if (clbHeaderTypeRelease.GetItemCheckState(i) == CheckState.Checked)
						{
							release = true;
						}
						opts.AddHeaderReleaseTypes((eHeaderType)hrp.Key, release);
					}

					opts.CommitData();
				}
				catch (Exception error)
				{
					ErrorFound = true;
					MessageBox.Show(error.Message);
					return true;
				}
				finally
				{
					opts.CloseUpdateConnection();
				}
                              
                // update basis labels
                _basisLabelVarChooser.UpdateData();
                _selectableVariableList = (ArrayList)_basisLabelSelectableVariableList.Clone();
                foreach (RowColProfileHeader rowColHdr in _selectableVariableList)
                    {
                        opts.GetBasisLabelInfo_Delete(((BasisLabelTypeProfile)rowColHdr.Profile).BasisLabelSystemOptionRID, ((BasisLabelTypeProfile)rowColHdr.Profile).BasisLabelType);
                        if (rowColHdr.IsDisplayed == true)
                        {
                            opts.GetBasisLabelInfo_Insert(((BasisLabelTypeProfile)rowColHdr.Profile).BasisLabelSystemOptionRID, ((BasisLabelTypeProfile)rowColHdr.Profile).BasisLabelType, rowColHdr.Sequence);
                        }
                }

				if (! _versionsHaveChanged)
				{
					ChangePending = false;
					return true;
				}

				// update forecast versions
				ForecastVersion fv = new ForecastVersion();
                DataTable dtFvOrig = fv.GetForecastVersions_ReadAllSortedByVersion(); // fv.GetForecastVersions(true, "ORDER BY FV_RID");
				int actualVersionRID = Include.NoRID;
				int forecastVersionRID = Include.NoRID;
				eForecastBlendType blendType = eForecastBlendType.None;

				try
				{
					fv.OpenUpdateConnection();

					foreach(DataRow dr in dtFvOrig.Rows)
					{
						bool rowFound = false;
						int rowFvRid = Convert.ToInt32(dr["FV_RID"], CultureInfo.CurrentUICulture);
						foreach (Infragistics.Win.UltraWinGrid.UltraGridRow udr in uGridVersions.Rows)
						{
							if (udr.Cells["FV_RID"].Value != DBNull.Value)
							{
								if (Convert.ToInt32(udr.Cells["FV_RID"].Value, CultureInfo.CurrentUICulture) == rowFvRid)
								{
									blendType = (eForecastBlendType)udr.Cells["BLEND_TYPE"].Value;
									if (blendType == eForecastBlendType.None)
									{
										actualVersionRID = Include.NoRID;
										forecastVersionRID = Include.NoRID;
									}
									else
									{
										actualVersionRID = (int)udr.Cells["ACTUAL_FV_RID"].Value;
										forecastVersionRID = (int)udr.Cells["FORECAST_FV_RID"].Value;
									}
									fv.UpdateVersion(rowFvRid, 
										(string)udr.Cells["FV_ID"].Value,
										(string)udr.Cells["DESCRIPTION"].Value,
										(bool)udr.Cells["PROTECT_HISTORY_IND_B"].Value,
//Begin Track #4457 - JSmith - Add forecast versions
//										(bool)udr.Cells["ACTIVE_IND_B"].Value);
										(bool)udr.Cells["ACTIVE_IND_B"].Value,
										blendType,
										actualVersionRID,
										forecastVersionRID,
//Begin Track #4547 - JSmith - Add similar stores by forecast versions
//									(bool)udr.Cells["CURRENT_BLEND_IND_B"].Value);
									(bool)udr.Cells["CURRENT_BLEND_IND_B"].Value,
									(bool)udr.Cells["SIMILAR_STORE_IND_B"].Value);
//End Track #4547
//End Track #4457

									rowFound = true;
									break;
								}
							}
						}
						if (! rowFound)
						{
							try
							{
								fv.CommitData();  // commit here just in case this delete fails
								fv.DeleteVersion(rowFvRid);
							}
							catch
							{
								MessageBox.Show(dr["DESCRIPTION"] + " cannot be deleted, but will be set to inactive");
								fv.OpenUpdateConnection();
								blendType = (eForecastBlendType)dr["BLEND_TYPE"];
								if (blendType == eForecastBlendType.None)
								{
									actualVersionRID = Include.NoRID;
									forecastVersionRID = Include.NoRID;
								}
								else
								{
									actualVersionRID = (int)dr["ACTUAL_FV_RID"];
									forecastVersionRID = (int)dr["FORECAST_FV_RID"];
								}
//Begin Track #5316 - JSmith - Delete failed
//								fv.UpdateVersion(rowFvRid, 
//									(string)dr["FV_ID"],
//									(string)dr["DESCRIPTION"],
//									((string)dr["PROTECT_HISTORY_IND"] == "1"),
////Begin Track #4457 - JSmith - Add forecast versions
////									false);
//									false,
//									blendType,
//									actualVersionRID,
//									forecastVersionRID,
////Begin Track #4547 - JSmith - Add similar stores by forecast versions
////									((string)dr["CURRENT_BLEND_IND_B"] == "1"));
//									((string)dr["CURRENT_BLEND_IND_B"] == "1"),
//									((string)dr["SIMILAR_STORE_IND_B"] == "1"));
////End Track #4547
////End Track #4457
								fv.UpdateVersion(rowFvRid, 
									(string)dr["FV_ID"],
									(string)dr["DESCRIPTION"],
									((string)dr["PROTECT_HISTORY_IND"] == "1"),
									false,
									blendType,
									actualVersionRID,
									forecastVersionRID,
									((string)dr["CURRENT_BLEND_IND"] == "1"),
									((string)dr["SIMILAR_STORE_IND"] == "1"));
//End Track #5316
							}

						}
					}
					foreach (Infragistics.Win.UltraWinGrid.UltraGridRow dr in uGridVersions.Rows)
					{
						if (dr.Cells["FV_RID"].Value == DBNull.Value)
						{
							try
							{
								fv.CommitData();  // commit here just in case this create fails
								if (dr.Cells["BLEND_TYPE"].Value != DBNull.Value &&
									(eForecastBlendType)dr.Cells["BLEND_TYPE"].Value != eForecastBlendType.None)
								{
									blendType = (eForecastBlendType)dr.Cells["BLEND_TYPE"].Value;
									actualVersionRID = (int)dr.Cells["ACTUAL_FV_RID"].Value;
									forecastVersionRID = (int)dr.Cells["FORECAST_FV_RID"].Value;
								}
								else
								{
									blendType = eForecastBlendType.None;
									actualVersionRID = Include.NoRID;
									forecastVersionRID = Include.NoRID;
								}
								
								fv.CreateVersion(
									"Z",
									(string)dr.Cells["DESCRIPTION"].Value,
									(bool)dr.Cells["PROTECT_HISTORY_IND_B"].Value,
//Begin Track #4457 - JSmith - Add forecast versions
//									(bool)dr.Cells["ACTIVE_IND_B"].Value);
									(bool)dr.Cells["ACTIVE_IND_B"].Value,
									blendType,
									actualVersionRID,
									forecastVersionRID,
//Begin Track #4547 - JSmith - Add similar stores by forecast versions
//									(bool)dr.Cells["CURRENT_BLEND_IND_B"].Value);
									(bool)dr.Cells["CURRENT_BLEND_IND_B"].Value,
									(bool)dr.Cells["SIMILAR_STORE_IND_B"].Value);
//End Track #4547
//End Track #4457
							}
							catch
							{
								MessageBox.Show((string)dr.Cells["DESCRIPTION"].Value + " already exists");
								fv.OpenUpdateConnection();
							}
						}
					}

					fv.CommitData();
				}
				catch (Exception error)
				{
					ErrorFound = true;
					MessageBox.Show(error.Message);
					return true;
				}
				finally
				{
					fv.CloseUpdateConnection();
				}

				ChangePending = false;
			}
			else
			{
				ErrorFound = true;
				string text = MIDText.GetTextOnly(eMIDTextCode.msg_ErrorsFoundReviewCorrect);
				MessageBox.Show(text);
			}

			return true;
		}

		private bool ValidateTabCompInfo()
		{
			bool compInfoValid = true;
//			string errorMessage = null;
			try
			{
				ErrorProvider.SetError (txtProductLevelDelimiter,"");
				if (txtProductLevelDelimiter.Text.Trim().Length != 1)
				{
					compInfoValid = false;
					ErrorProvider.SetError (txtProductLevelDelimiter,MIDText.GetText(eMIDTextCode.msg_ProductLevelDelimiterEdit));
				}
                //Begin TT#506-MD -jsobek -Add infrastructure to allow Email to be sent from the application
                if (this.SMTP_Control.IsValid(this.SMTP_Options) == false )
                {
                    compInfoValid = false;
                    
                }
                //Require Company Name if SMTP is enabled
                if (this.SMTP_Options.SMTP_Enabled.Value == true && txtCompany.Text == string.Empty) 
                {
                    ErrorProvider.SetError(txtCompany, "Please enter your company name");
                    compInfoValid = false;

                }
                //End TT#506-MD -jsobek -Add infrastructure to allow Email to be sent from the application
			}
			catch( Exception exception )
			{
				HandleException(exception);
			}
			return compInfoValid;
		}

		private bool ValidateTabDisplayOptions()
		{
			bool displayOptionsValid = true;
			string errorMessage = null;
			int newStorePeriodBegin = 0;
			int newStorePeriodEnd = 0;
			int nonCompStorePeriodBegin = 0;
			int nonCompStorePeriodEnd = 0;
			bool newStorePeriodBeginEntered = false;
			bool newStorePeriodEndEntered = false;
			bool nonCompStorePeriodBeginEntered = false;
			bool nonCompStorePeriodEndEntered = false;
			try
			{
				ErrorFound = false;
				#region LOAD NUMERIC VALUES
				//Make sure new store values are numeric
				if (txtNewStorePeriodBegin.Text.Length > 0)
				{
					newStorePeriodBeginEntered = true;
					try
					{
						ErrorProvider.SetError (this.txtNewStorePeriodBegin,"");
						newStorePeriodBegin = Convert.ToInt32(txtNewStorePeriodBegin.Text, CultureInfo.CurrentUICulture);
					}
					catch
					{
						errorMessage = _SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MustBeNumeric);
						displayOptionsValid = false;
						ErrorProvider.SetError (this.txtNewStorePeriodBegin,errorMessage);
					}
				}
				if (txtNewStorePeriodEnd.Text.Length > 0)
				{
					newStorePeriodEndEntered = true;
					try
					{
						ErrorProvider.SetError (this.txtNewStorePeriodEnd,"");
						newStorePeriodEnd = Convert.ToInt32(txtNewStorePeriodEnd.Text, CultureInfo.CurrentUICulture);
					}
					catch
					{
						errorMessage = _SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MustBeNumeric);
						displayOptionsValid = false;
						ErrorProvider.SetError (this.txtNewStorePeriodEnd,errorMessage);
					}
				}

//				if (newStorePeriodBeginEntered && newStorePeriodEndEntered)
//				{
//					if (newStorePeriodEnd < newStorePeriodBegin)
//					{
//						errorMessage = _SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_EndNotGTBegin);
//						displayOptionsValid = false;
//						ErrorProvider.SetError (this.txtNewStorePeriodEnd,errorMessage);
//					}
//					else
//					{
//						ErrorProvider.SetError (this.txtNewStorePeriodEnd,"");
//					}
//				}

				//Make sure noncomp store values are numeric
				if (txtNonCompPeriodBegin.Text.Length > 0)
				{
					nonCompStorePeriodBeginEntered = true;
					try
					{
						ErrorProvider.SetError (this.txtNonCompPeriodBegin,"");
						nonCompStorePeriodBegin = Convert.ToInt32(txtNonCompPeriodBegin.Text, CultureInfo.CurrentUICulture);
					}
					catch
					{
						errorMessage = _SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MustBeNumeric);
						displayOptionsValid = false;
						ErrorProvider.SetError (this.txtNonCompPeriodBegin,errorMessage);
					}
				}
				if (txtNonCompPeriodEnd.Text.Length > 0)
				{
					nonCompStorePeriodEndEntered = true;
					try
					{
						ErrorProvider.SetError (this.txtNonCompPeriodEnd,"");
						nonCompStorePeriodEnd = Convert.ToInt32(txtNonCompPeriodEnd.Text, CultureInfo.CurrentUICulture);
					}
					catch
					{
						errorMessage = _SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MustBeNumeric);
						displayOptionsValid = false;
						ErrorProvider.SetError (this.txtNonCompPeriodEnd,errorMessage);
					}
				}
				#endregion

				// begin validation
				if(newStorePeriodBeginEntered) {
					if(!newStorePeriodEndEntered && !(nonCompStorePeriodBeginEntered || newStorePeriodBegin < 0)) {
						errorMessage = _SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_CannontBeBlank);
						displayOptionsValid = false;
						ErrorProvider.SetError (this.txtNewStorePeriodBegin, errorMessage);
					}
					else if(newStorePeriodEnd <= newStorePeriodBegin) {							
						errorMessage = _SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_EndNotGTBegin);
						displayOptionsValid = false;
						ErrorProvider.SetError (this.txtNewStorePeriodBegin, errorMessage);
					}
					else {
						ErrorProvider.SetError (this.txtNewStorePeriodBegin, "");
					}
				}
				else {
					if(nonCompStorePeriodEndEntered && newStorePeriodEndEntered && !nonCompStorePeriodBeginEntered && !newStorePeriodBeginEntered) {
						if(nonCompStorePeriodEnd < 0) {
							errorMessage = _SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MustBePositiveInteger);
							displayOptionsValid = false;
							ErrorProvider.SetError (this.txtNonCompPeriodEnd, errorMessage);
						}
						else {
							ErrorProvider.SetError (this.txtNonCompPeriodEnd, "");
						}
						if(newStorePeriodEnd < 0) {
							errorMessage = _SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MustBePositiveInteger);
							displayOptionsValid = false;
							ErrorProvider.SetError (this.txtNewStorePeriodEnd, errorMessage);
						}
						else {
							ErrorProvider.SetError (this.txtNewStorePeriodEnd, "");
						}
					}
					else if(nonCompStorePeriodEndEntered && !newStorePeriodEndEntered && !nonCompStorePeriodBeginEntered && !newStorePeriodBeginEntered) {
						if(nonCompStorePeriodEnd < 0) {
							errorMessage = _SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MustBePositiveInteger);
							displayOptionsValid = false;
							ErrorProvider.SetError (this.txtNonCompPeriodEnd, errorMessage);
						}
						else {
							ErrorProvider.SetError (this.txtNonCompPeriodEnd, "");
						}
					}
					else if(newStorePeriodEndEntered) {
						errorMessage = _SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_BlankBeginMeansBlankEnd);
						displayOptionsValid = false;
						ErrorProvider.SetError (this.txtNewStorePeriodEnd, errorMessage);
					}
					else {
						ErrorProvider.SetError (this.txtNewStorePeriodEnd, "");
					}
				}

				if(nonCompStorePeriodBeginEntered) {
					if(!(nonCompStorePeriodEndEntered && nonCompStorePeriodEnd > nonCompStorePeriodBegin)){
						errorMessage = _SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_EndNotGTBegin);
						displayOptionsValid = false;
						ErrorProvider.SetError (this.txtNonCompPeriodEnd, errorMessage);
					}
					else {
						ErrorProvider.SetError (this.txtNonCompPeriodEnd, "");
					}
				}

				if(displayOptionsValid) {
					// make sure everything's bigger than the next
					if(newStorePeriodBeginEntered) {
						if(
							(newStorePeriodEndEntered && newStorePeriodBegin >= newStorePeriodEnd) || 
							(nonCompStorePeriodBeginEntered && newStorePeriodBegin >= nonCompStorePeriodBegin) ||
							(nonCompStorePeriodEndEntered && newStorePeriodBegin >= nonCompStorePeriodEnd)
							) {
							errorMessage = _SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_SmallerValuesFound);
							displayOptionsValid = false;
							ErrorProvider.SetError (this.txtNewStorePeriodBegin, errorMessage);
						}
						else {
							ErrorProvider.SetError (this.txtNewStorePeriodBegin, "");
						}
					}
					if(newStorePeriodEndEntered) {
						if(
							(nonCompStorePeriodBeginEntered && newStorePeriodEnd > nonCompStorePeriodBegin) ||
							(nonCompStorePeriodEndEntered && newStorePeriodEnd >= nonCompStorePeriodEnd)
							) {
							errorMessage = _SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_SmallerValuesFound);
							displayOptionsValid = false;
							ErrorProvider.SetError (this.txtNewStorePeriodEnd, errorMessage);
						}
						else {
							ErrorProvider.SetError (this.txtNewStorePeriodEnd, "");
						}
					}
					if(nonCompStorePeriodBeginEntered) {
						if(
							(nonCompStorePeriodEndEntered && nonCompStorePeriodBegin >= nonCompStorePeriodEnd)
							) {
							errorMessage = _SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_SmallerValuesFound);
							displayOptionsValid = false;
							ErrorProvider.SetError (this.txtNonCompPeriodBegin, errorMessage);
						}
						else {
							ErrorProvider.SetError (this.txtNonCompPeriodBegin, "");
						}
					}
				}
				// BEGIN ANF Generic Size Constraints
				if(displayOptionsValid)
				{
					displayOptionsValid = ValidateSizeFilters();
				}
				// END ANF Generic Size Constraints
			}
			catch( Exception exception )
			{
				HandleException(exception);
			}
			return displayOptionsValid;
		}
		
		// BEGIN ANF Generic Size Constraints
		private bool ValidateSizeFilters()
		{
			bool displayOptionsValid = true;
			try
			{
				if (!ValidSizeFilter(cboSizeCurve, txtSizeCurve))
				{
					displayOptionsValid = false;
				}
				if (!ValidSizeFilter(cboSizeGroup, txtSizeGroup))
				{
					displayOptionsValid = false;
				}
				if (!ValidSizeFilter(cboSizeAlternates, txtSizeAlternates))
				{
					displayOptionsValid = false;
				}
				if (!ValidSizeFilter(cboSizeConstraints, txtSizeConstraints))
				{
					displayOptionsValid = false;
				}
			}	
			catch( Exception exception )
			{
				HandleException(exception);
			}
			return displayOptionsValid;
		}

		private bool ValidSizeFilter(CheckBox aCheckBox, TextBox aTextBox)
		{
			bool filterValid = true;
			string errorMessage = null;
			try
			{
				if (aCheckBox.Checked)
				{
					if (aTextBox.Text.Trim() == string.Empty)
					{
						errorMessage = _SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_FieldIsRequired);
						filterValid = false;
						ErrorProvider.SetError (aTextBox, errorMessage);
					}
					else 
					{
						ErrorProvider.SetError (aTextBox, string.Empty);
					}
				}
				else 
				{
					ErrorProvider.SetError (aTextBox, string.Empty);
				}
			}	
			catch( Exception exception )
			{
				HandleException(exception);
			}
			return filterValid;
		}
		// END ANF Generic Size Constraints

		private bool ValidateTabOTS()
		{
			bool versionsValid = true;
			string errorMessage = null;
			//Begin Track #4457 - JSmith - Add forecast versions
			eForecastBlendType blendType;
			//End Track #4457
			try
			{
				if (_versionsHaveChanged)
				{
					foreach (Infragistics.Win.UltraWinGrid.UltraGridRow dr in uGridVersions.Rows)
					{
						//						int rowFvRid = Convert.ToInt32(dr.Cells["FV_RID"].Value, CultureInfo.CurrentUICulture);
						//						string fvID = Convert.ToString(dr.Cells["FV_ID"].Value, CultureInfo.CurrentUICulture);
						string fvDescription = Convert.ToString(dr.Cells["DESCRIPTION"].Value, CultureInfo.CurrentUICulture);
						bool fvProtectHistoryInd = Convert.ToBoolean(dr.Cells["PROTECT_HISTORY_IND_B"].Value, CultureInfo.CurrentUICulture);
						bool fvActiveInd = Convert.ToBoolean(dr.Cells["ACTIVE_IND_B"].Value, CultureInfo.CurrentUICulture);

						if (fvDescription.Length == 0)
						{
							errorMessage = _SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_FieldIsRequired);
							versionsValid = false;
							dr.Cells["DESCRIPTION"].Appearance.Image = ErrorImage;
//							dr.Cells["DESCRIPTION"].Tag = errorMessage;
							((GridCellTag)(dr.Cells["DESCRIPTION"].Tag)).Message = errorMessage;
						}
						else
						{
							dr.Cells["DESCRIPTION"].Appearance.Image = null;
//							dr.Cells["DESCRIPTION"].Tag = null;
							((GridCellTag)(dr.Cells["DESCRIPTION"].Tag)).Message = null;
						}

						//Begin Track #4457 - JSmith - Add forecast versions
						blendType = (eForecastBlendType)dr.Cells["BLEND_TYPE"].Value;
						if (blendType != eForecastBlendType.None)
						{
							if (dr.Cells["ACTUAL_FV_RID"].Value == DBNull.Value)
							{
								errorMessage = _SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_FieldIsRequired);
								versionsValid = false;
								dr.Cells["ACTUAL_FV_RID"].Appearance.Image = ErrorImage;
//								dr.Cells["ACTUAL_FV_RID"].Tag = errorMessage;
								((GridCellTag)(dr.Cells["ACTUAL_FV_RID"].Tag)).Message = errorMessage;
							}
							else
							{
								dr.Cells["ACTUAL_FV_RID"].Appearance.Image = null;
//								dr.Cells["ACTUAL_FV_RID"].Tag = null;
								((GridCellTag)(dr.Cells["ACTUAL_FV_RID"].Tag)).Message = null;
							}

							if (dr.Cells["FORECAST_FV_RID"].Value == DBNull.Value)
							{
								errorMessage = _SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_FieldIsRequired);
								versionsValid = false;
								dr.Cells["FORECAST_FV_RID"].Appearance.Image = ErrorImage;
//								dr.Cells["FORECAST_FV_RID"].Tag = errorMessage;
								((GridCellTag)(dr.Cells["FORECAST_FV_RID"].Tag)).Message = errorMessage;
							}
							else
							{
								dr.Cells["FORECAST_FV_RID"].Appearance.Image = null;
//								dr.Cells["FORECAST_FV_RID"].Tag = null;
								((GridCellTag)(dr.Cells["FORECAST_FV_RID"].Tag)).Message = null;
							}

							// make sure forecast version is not a combined version
							if (dr.Cells["FORECAST_FV_RID"].Value != DBNull.Value)
							{
								int versionRID = Include.NoRID;
								if (dr.Cells["FV_RID"].Value != DBNull.Value)
								{
									versionRID = Convert.ToInt32(dr.Cells["FV_RID"].Value);
								}
								int forecastVersionRID = Convert.ToInt32(dr.Cells["FORECAST_FV_RID"].Value);
								// allow a version to be combined with itself
								if (versionRID != forecastVersionRID)
								{
									foreach (Infragistics.Win.UltraWinGrid.UltraGridRow dr2 in uGridVersions.Rows)
									{
										if (dr2.Cells["FV_RID"].Value != DBNull.Value)
										{
											if (Convert.ToInt32(dr2.Cells["FV_RID"].Value) == forecastVersionRID)
											{
												blendType = (eForecastBlendType)dr2.Cells["BLEND_TYPE"].Value;
												if (blendType != eForecastBlendType.None)
												{
													errorMessage = _SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_CannotUseCombinedVersion);
													versionsValid = false;
													dr.Cells["FORECAST_FV_RID"].Appearance.Image = ErrorImage;
//													dr.Cells["FORECAST_FV_RID"].Tag = errorMessage;
													((GridCellTag)(dr.Cells["FORECAST_FV_RID"].Tag)).Message = errorMessage;
												}
											}
										}
									}
								}
							}
						}
						//End Track #4457
					}
				}
				
			}
			catch( Exception exception )
			{
				HandleException(exception);
			}
			return versionsValid;
		}

		private bool ValidateTabAllocDefault()
		{
			bool allocDefaultValid = true;
			string errorMessage = null;
			try
			{
				try
				{
					ErrorProvider.SetError (this.txtPctNeedLimit,"");
					string strPctNeedLimit = this.txtPctNeedLimit.Text.Trim();
					if (strPctNeedLimit.Length > 0)
					{
						double pctNeedLimit = Convert.ToDouble(strPctNeedLimit, CultureInfo.CurrentUICulture);
						//if (pctNeedLimit < -100 ||
						//	pctNeedLimit > 100)
						if (pctNeedLimit > 100)
						{
							errorMessage = _SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ValueCannotExceed100);
							allocDefaultValid = false;
							ErrorProvider.SetError (this.txtPctNeedLimit,errorMessage);
						}
						else
						{
							pctNeedLimit = Math.Round(pctNeedLimit,2);
							this.txtPctNeedLimit.Text = pctNeedLimit.ToString(CultureInfo.CurrentUICulture);
						}
					}
				}
				catch
				{
					errorMessage = _SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MustBeNumeric);
					allocDefaultValid = false;
					ErrorProvider.SetError (this.txtPctNeedLimit,errorMessage);
				}

				try
				{
					ErrorProvider.SetError (this.txtPctBalTolerance,"");
					string strPctBalanceTolerance = this.txtPctBalTolerance.Text.Trim();
					if (strPctBalanceTolerance.Length > 0)
					{
						double pctBalanceTolerance = Convert.ToDouble(strPctBalanceTolerance, CultureInfo.CurrentUICulture);
						if (pctBalanceTolerance < 0)
						{
							errorMessage = _SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MustBeNonNegative);
							allocDefaultValid = false;
							ErrorProvider.SetError (this.txtPctBalTolerance,errorMessage);
						}
//						else
//							if (pctBalanceTolerance < 0 ||
//							pctBalanceTolerance > 100)
//						{
//							errorMessage = _SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MustBeBetween0And100);
//							allocDefaultValid = false;
//							ErrorProvider.SetError (this.txtPctBalTolerance,errorMessage);
//						}
					}
				}
				catch
				{
					errorMessage = _SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MustBeNumeric);
					allocDefaultValid = false;
					ErrorProvider.SetError (this.txtPctBalTolerance,errorMessage);
				}
                // begin TT#1365 - JEllis - Detail Pack Size Need Enhancement
                try
                {
                    ErrorProvider.SetError(this.txtPackNeedTolerance, "");
                    if (cbxStepped.Checked)
                    {
                        string strPackNeedTolerance = txtPackNeedTolerance.Text.Trim();
                        if (strPackNeedTolerance.Length == 0
                            || Convert.ToDouble(strPackNeedTolerance, CultureInfo.CurrentUICulture) > Include.MaxPackNeedTolerance)
                        {
                            allocDefaultValid = false;
                            errorMessage = string.Format(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_al_ValueTooLargeWhenSteppedActive), Include.MaxPackNeedTolerance, cbxStepped.Text);
                            ErrorProvider.SetError(txtPackNeedTolerance, errorMessage);
                        }
                    }
                }
                catch (Exception)
                {
                    throw;
                }
                // end TT#1365 - JEllis - Detail Pack Size Need Enhancement
			}
			catch( Exception exception )
			{
				HandleException(exception);
			}
			return allocDefaultValid;
		}

        private bool ValidateTabBasisLabels()
		{
            bool globalOptionsValid = true;
			string errorMessage = null;
			try
			{
                ErrorProvider.SetError(_basisLabelVarChooser, string.Empty);

                if (!_basisLabelVarChooser.ValidateData())
                {
                    //errorMessage = _SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MustBeNonNegative);
                    //ErrorProvider.SetError(this.txtPctBalTolerance, errorMessage);
                    globalOptionsValid = false;
                }
			}
			catch( Exception exception )
			{
				HandleException(exception);
			}
            return globalOptionsValid;
		}

		//		private void txtPurge_Leave(object sender, System.EventArgs e)
		//		{
		//			try
		//			{
		//				string inStr = ((TextBox)sender).Text.ToString();
		//				int outVal = (int)(System.Math.Abs((Convert.ToDouble(inStr)) + 0.5));
		//				
		//				string outStr = outVal.ToString();
		//				if (inStr != outStr)
		//				{
		//					MessageBox.Show("Converting to positive integer");
		//				}
		//				((TextBox)sender).Text = outStr;
		//			}
		//			catch
		//			{
		//				MessageBox.Show("Please enter a positive integer");
		//				((TextBox)sender).Text = "6";
		//			}
		//		}




		private void btnCancel_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void uGridVersions_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
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
			if (!_securityOTSPlanVersions.AllowDelete)
			{
				foreach (UltraGridBand ugb in uGridVersions.DisplayLayout.Bands)
				{
					ugb.Override.AllowDelete = DefaultableBoolean.False;
				}
			}
			else
			{
				foreach (UltraGridBand ugb in uGridVersions.DisplayLayout.Bands)
				{
					ugb.Override.AllowDelete = DefaultableBoolean.True;
				}
			}
		}

		//Begin Track #4457 - JSmith - Add forecast versions
		private void uGridVersions_AfterRowInsert(object sender, Infragistics.Win.UltraWinGrid.RowEventArgs e)
		{
			e.Row.Cells["BLEND_TYPE"].Value = eForecastBlendType.None;
			SetVersionRowCharacteristics(e.Row, eForecastBlendType.None);
		}
		//End Track #4457

		private void uGridVersions_BeforeRowsDeleted(object sender, Infragistics.Win.UltraWinGrid.BeforeRowsDeletedEventArgs e)
		{
			_versionsHaveChanged = true;
			ChangePending = true;

			foreach (Infragistics.Win.UltraWinGrid.UltraGridRow dr in e.Rows)
			{
				if ((dr.Cells["FV_RID"].Value != DBNull.Value) && (Convert.ToInt32(dr.Cells["FV_RID"].Value, CultureInfo.CurrentUICulture) <= 4))
				{
//					MessageBox.Show("Cannot delete a standard version");
					MessageBox.Show(_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_CannotDeleteStandardVersion), this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
					e.Cancel = true;
					break;
				}
				//Begin Track #4457 - JSmith - Add forecast versions
				else
				{
					foreach (Infragistics.Win.UltraWinGrid.UltraGridRow row in uGridVersions.Rows)
					{
						if (row.Cells["ACTUAL_FV_RID"].Value != DBNull.Value)
						{
							if (Convert.ToInt32(dr.Cells["FV_RID"].Value, CultureInfo.CurrentUICulture) == Convert.ToInt32(row.Cells["ACTUAL_FV_RID"].Value, CultureInfo.CurrentUICulture))
							{
								MessageBox.Show(_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_CannotDeleteCombinedVersion), this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
								e.Cancel = true;
								break;
							}
						}
						if (row.Cells["FORECAST_FV_RID"].Value != DBNull.Value)
						{
							if (Convert.ToInt32(dr.Cells["FV_RID"].Value, CultureInfo.CurrentUICulture) == Convert.ToInt32(row.Cells["FORECAST_FV_RID"].Value, CultureInfo.CurrentUICulture))
							{
								MessageBox.Show(_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_CannotDeleteCombinedVersion), this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
								e.Cancel = true;
								break;
							}
						}
					}
                    //BEGIN TT#3509-VStuart-Cannot delete OTS Plan Version-ANFUser
                    if (!e.Cancel)
                    {
                        if (CheckInUse(eProfileType.Version, Convert.ToInt32(dr.Cells["FV_RID"].Value), false))
                        {
                            e.Cancel = true;
                        }
                    }
                    //END TT#3509-VStuart-Cannot delete OTS Plan Version-ANFUser
				}
				//End Track #4457
			}
		}

		private void uGridVersions_BeforeRowUpdate(object sender, Infragistics.Win.UltraWinGrid.CancelableRowEventArgs e)
		{
			_versionsHaveChanged = true;
			ChangePending = true;

			if (e.Row.Cells["FV_RID"].Value != DBNull.Value)
			{
				if ((Convert.ToInt32(e.Row.Cells["FV_RID"].Value, CultureInfo.CurrentUICulture) == 1) && 
					(Convert.ToBoolean(e.Row.Cells["PROTECT_HISTORY_IND_B"].Value, CultureInfo.CurrentUICulture) == false))
				{
					MessageBox.Show("Cannot unprotect this version");
					e.Cancel = true;
				}
				if ((Convert.ToInt32(e.Row.Cells["FV_RID"].Value, CultureInfo.CurrentUICulture) <= 3) && 
					(Convert.ToBoolean(e.Row.Cells["ACTIVE_IND_B"].Value, CultureInfo.CurrentUICulture) == false))
				{
					MessageBox.Show("This version must be active");
					e.Cancel = true;
				}
			}
			if (e.Row.Cells["DESCRIPTION"].Value == DBNull.Value)
				e.Cancel = true;
			else
				// Don't allow leading or trailing spaces in description
				e.Row.Cells["DESCRIPTION"].Value = ((string)e.Row.Cells["DESCRIPTION"].Value).Trim();
		}

		//Begin Track #4457 - JSmith - Add forecast versions
		private void uGridVersions_CellChange(object sender, Infragistics.Win.UltraWinGrid.CellEventArgs e)
		{
			try
			{
				switch (uGridVersions.ActiveCell.Column.Key)
				{
					case "BLEND_TYPE":
						eForecastBlendType blendType = (eForecastBlendType)uGridVersions.ActiveCell.ValueListResolved.GetValue(uGridVersions.ActiveCell.ValueListResolved.SelectedItemIndex);
						
						SetVersionRowCharacteristics(uGridVersions.ActiveCell.Row, blendType);

						break;
					case "ACTUAL_FV_RID":
						uGridVersions.ActiveCell.Row.Cells["ACTUAL_FV_RID"].Appearance.Image = null;
//						uGridVersions.ActiveCell.Row.Cells["ACTUAL_FV_RID"].Tag = null;
						((GridCellTag)(uGridVersions.ActiveCell.Row.Cells["ACTUAL_FV_RID"].Tag)).Message = null;
						break;
					case "FORECAST_FV_RID":
						uGridVersions.ActiveCell.Row.Cells["FORECAST_FV_RID"].Appearance.Image = null;
//						uGridVersions.ActiveCell.Row.Cells["FORECAST_FV_RID"].Tag = null;
						((GridCellTag)(uGridVersions.ActiveCell.Row.Cells["FORECAST_FV_RID"].Tag)).Message = null;
						break;
				}

				if (FormLoaded)
				{
					ChangePending = true;
					_versionsHaveChanged = true;
				}
			}
			catch( Exception ex )
			{
				HandleException(ex);
			}
		}

		private void SetVersionRowCharacteristics()
		{
			try
			{
				foreach(  UltraGridRow gridRow in this.uGridVersions.Rows )
				{
					eForecastBlendType blendType = (eForecastBlendType)Convert.ToInt32(gridRow.Cells["BLEND_TYPE"].Value, CultureInfo.CurrentUICulture);
					
					SetVersionRowCharacteristics(gridRow, blendType);
				}
			}
			catch( Exception ex )
			{
				HandleException(ex);
			}
		}

		private void SetVersionRowCharacteristics(UltraGridRow aGridRow, eForecastBlendType aBlendType)
		{
			try
			{
				GridCellTag cellTag;

				if (aGridRow.Cells["FV_RID"].Value != DBNull.Value)
				{
					if (Convert.ToInt32(aGridRow.Cells["FV_RID"].Value) == Include.FV_ActualRID ||
						Convert.ToInt32(aGridRow.Cells["FV_RID"].Value) == Include.FV_ActionRID ||
						Convert.ToInt32(aGridRow.Cells["FV_RID"].Value) == Include.FV_BaselineRID ||
						Convert.ToInt32(aGridRow.Cells["FV_RID"].Value) == Include.FV_ModifiedRID)
					{
						aGridRow.Cells["BLEND_TYPE"].Activation = Infragistics.Win.UltraWinGrid.Activation.Disabled;
						aGridRow.Cells["ACTUAL_FV_RID"].Activation = Infragistics.Win.UltraWinGrid.Activation.Disabled;
						aGridRow.Cells["FORECAST_FV_RID"].Activation = Infragistics.Win.UltraWinGrid.Activation.Disabled;
						aGridRow.Cells["CURRENT_BLEND_IND_B"].Activation = Infragistics.Win.UltraWinGrid.Activation.Disabled;
					}
				}

				if (aBlendType == eForecastBlendType.None)
				{
					aGridRow.Cells["ACTUAL_FV_RID"].Activation = Infragistics.Win.UltraWinGrid.Activation.Disabled;
					aGridRow.Cells["FORECAST_FV_RID"].Activation = Infragistics.Win.UltraWinGrid.Activation.Disabled;
					aGridRow.Cells["CURRENT_BLEND_IND_B"].Activation = Infragistics.Win.UltraWinGrid.Activation.Disabled;
					aGridRow.Cells["PROTECT_HISTORY_IND_B"].Activation = Infragistics.Win.UltraWinGrid.Activation.AllowEdit;
					aGridRow.Cells["ACTUAL_FV_RID"].Value = DBNull.Value;
					aGridRow.Cells["FORECAST_FV_RID"].Value = DBNull.Value;
				}
				else
				{
					aGridRow.Cells["ACTUAL_FV_RID"].Activation = Infragistics.Win.UltraWinGrid.Activation.AllowEdit;
					aGridRow.Cells["FORECAST_FV_RID"].Activation = Infragistics.Win.UltraWinGrid.Activation.AllowEdit;
					aGridRow.Cells["CURRENT_BLEND_IND_B"].Activation = Infragistics.Win.UltraWinGrid.Activation.AllowEdit;
					aGridRow.Cells["PROTECT_HISTORY_IND_B"].Activation = Infragistics.Win.UltraWinGrid.Activation.Disabled;
					aGridRow.Cells["PROTECT_HISTORY_IND_B"].Value = true;
				}

				// add tooltips
				cellTag = new GridCellTag(); 
				cellTag.HelpText = MIDText.GetTextOnly(eMIDTextCode.tt_FV_Description);
				aGridRow.Cells["DESCRIPTION"].Tag = cellTag;

				cellTag = new GridCellTag(); 
				cellTag.HelpText = MIDText.GetTextOnly(eMIDTextCode.tt_FV_Active);
				aGridRow.Cells["ACTIVE_IND_B"].Tag = cellTag;

				cellTag = new GridCellTag(); 
				cellTag.HelpText = MIDText.GetTextOnly(eMIDTextCode.tt_FV_BlendType);
				aGridRow.Cells["BLEND_TYPE"].Tag = cellTag;

				cellTag = new GridCellTag(); 
				cellTag.HelpText = MIDText.GetTextOnly(eMIDTextCode.tt_FV_ActualVersion);
				aGridRow.Cells["ACTUAL_FV_RID"].Tag = cellTag;

				cellTag = new GridCellTag(); 
				cellTag.HelpText = MIDText.GetTextOnly(eMIDTextCode.tt_FV_ForecastVersion);;
				aGridRow.Cells["FORECAST_FV_RID"].Tag = cellTag;

				cellTag = new GridCellTag(); 
				cellTag.HelpText = MIDText.GetTextOnly(eMIDTextCode.tt_FV_PeriodHistory);
				aGridRow.Cells["CURRENT_BLEND_IND_B"].Tag = cellTag;

				cellTag = new GridCellTag(); 
				cellTag.HelpText = MIDText.GetTextOnly(eMIDTextCode.tt_FV_ProtectHistory);
				aGridRow.Cells["PROTECT_HISTORY_IND_B"].Tag = cellTag;

				cellTag = new GridCellTag(); 
				cellTag.HelpText = MIDText.GetTextOnly(eMIDTextCode.tt_FV_SimilarStores);
				aGridRow.Cells["SIMILAR_STORE_IND_B"].Tag = cellTag;

			}
			catch( Exception ex )
			{
				HandleException(ex);
			}
		}
		//End Track #4457

		private void txtPositiveInteger_Validating(object sender, System.ComponentModel.CancelEventArgs e)
		{
			try
			{
				ErrorProvider.SetError ((TextBox)sender,"");
				((TextBox)sender).Text = (((TextBox)sender).Text.ToString(CultureInfo.CurrentUICulture)).Trim();
				string inStr = ((TextBox)sender).Text.ToString(CultureInfo.CurrentUICulture);
				string outStr = System.Math.Abs(Convert.ToInt32(inStr, CultureInfo.CurrentUICulture)).ToString(CultureInfo.CurrentUICulture);
				
				if (inStr != outStr)
				{
					throw new Exception();
				}
			}
			catch
			{
				ErrorProvider.SetError ((TextBox)sender,"Please enter a positive integer");
				MessageBox.Show("Please enter a positive integer");
				((TextBox)sender).Text = "";
				((TextBox)sender).Focus();
			}

		}

		private void txtNewStore_Validating(object sender, System.ComponentModel.CancelEventArgs e)
		{
			try
			{
				ErrorProvider.SetError ((TextBox)sender,"");
				string inStr = ((TextBox)sender).Text.ToString(CultureInfo.CurrentUICulture);
				if (inStr == null || inStr.Length == 0)
					return;

				string outStr = Convert.ToInt32(inStr, CultureInfo.CurrentUICulture).ToString(CultureInfo.CurrentUICulture);
		
				if (inStr != outStr)
				{
					throw new Exception();
				}
			}
			catch
			{
				ErrorProvider.SetError ((TextBox)sender,"Please enter an integer, or leave empty");
				MessageBox.Show("Please enter an integer, or leave empty");
				((TextBox)sender).Text = string.Empty;
				((TextBox)sender).Focus();
			}

		}

		private void txtPositivePercent_Validating(object sender, System.ComponentModel.CancelEventArgs e)
		{
			try
			{
				ErrorProvider.SetError ((TextBox)sender,"");
				((TextBox)sender).Text = (((TextBox)sender).Text.ToString(CultureInfo.CurrentUICulture)).Trim();
				string inStr = ((TextBox)sender).Text.ToString(CultureInfo.CurrentUICulture);
				if (inStr == null || inStr.Length == 0) 
					return;

				string outStr = System.Math.Abs(Convert.ToDouble(inStr, CultureInfo.CurrentUICulture)).ToString(CultureInfo.CurrentUICulture);
				
				if (inStr != outStr)
				{
					throw new Exception();
				}
			}
			catch
			{
				ErrorProvider.SetError ((TextBox)sender,"Please enter a positive number");
				MessageBox.Show("Please enter a positive number");
				((TextBox)sender).Text = string.Empty;
				((TextBox)sender).Focus();
			}
		}



		private void txtZeroToOneHundredPercent_Validating(object sender, System.ComponentModel.CancelEventArgs e)
		{
			try
			{
				ErrorProvider.SetError ((TextBox)sender,"");
				((TextBox)sender).Text = (((TextBox)sender).Text.ToString(CultureInfo.CurrentUICulture)).Trim();
				string inStr = ((TextBox)sender).Text.ToString(CultureInfo.CurrentUICulture);
				if (inStr == null || inStr.Length == 0) 
					return;

				double outVal = System.Math.Abs(Convert.ToDouble(inStr, CultureInfo.CurrentUICulture));
								
				if (outVal < 0.0 || outVal > 100.0)
				{
					throw new Exception();
				}
			}
			catch
			{
				ErrorProvider.SetError ((TextBox)sender,"Please enter 0.0 to 100.0 Percent");
				MessageBox.Show("Please enter 0.0 to 100.0 Percent");
				((TextBox)sender).Text = string.Empty;
				((TextBox)sender).Focus();
			}
		}

		private void txtPercent_Validating(object sender, System.ComponentModel.CancelEventArgs e)
		{
			try
			{
				ErrorProvider.SetError ((TextBox)sender,"");
				((TextBox)sender).Text = (((TextBox)sender).Text.ToString(CultureInfo.CurrentUICulture)).Trim();
//				string inStr = ((TextBox)sender).Text.ToString(CultureInfo.CurrentUICulture);
//				if (inStr == null || inStr.Length == 0) 
//					return;
//
//				((TextBox)sender).Text = Convert.ToDouble(inStr, CultureInfo.CurrentUICulture).ToString(CultureInfo.CurrentUICulture);
			}
			catch
			{
				ErrorProvider.SetError ((TextBox)sender,"Please enter a number");
				MessageBox.Show("Please enter a number");
				((TextBox)sender).Text = string.Empty;
				((TextBox)sender).Focus();
			}
		}

		/// <summary>
		/// validate company name
		///
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void txtCompanyName_Validating(object sender, System.ComponentModel.CancelEventArgs e)
		{
			ErrorProvider.SetError (this.txtCompany,"");
			if (txtCompany.Text.Length < 1)
			{
				ErrorProvider.SetError (this.txtCompany,"Please enter your company name");
				MessageBox.Show("Please enter your company name");
				((TextBox)sender).Text = string.Empty ;
				((TextBox)sender).Focus();
			}
		}
		private void SetText()
		{
			try
			{
				this.btnOK.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_OK);
				this.btnCancel.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_Cancel);
				this.label1.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_SelectHeaderRelease);
				// BEGIN MID Track #4357 - security for interfaced and non-interfaced headers
				this.cboProtectInterfacedHeaders.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_ProtectInterfacedHeaders);
				// END MID Track #4357
				lblHeaderLinkChar.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_HeaderLinkCharacteristic) + ":";
				// BEGIN MID Track #4826 - JSmith - Normalize Size Curves
				this.lblNormalizeSizeCurves.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_NormalizeSizeCurves) + ":";
				this.radNormalizeSizeCurves_Yes.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Yes);
				this.radNormalizeSizeCurves_No.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_No);
				// END MID Track #4826
				// BEGIN MID Track #4921 - JSmith - A&F 666 - Size Modification
				this.lblFillSizesTo.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Fill_SizesTo) + ":";
				this.radFillSizesTo_Holes.Text = MIDText.GetTextOnly((int)eFillSizesToType.Holes);
				this.radFillSizesTo_SizePlan.Text = MIDText.GetTextOnly((int)eFillSizesToType.SizePlan);
                this.radFillSizesTo_SizePlanWithSizeMins.Text = MIDText.GetTextOnly((int)eFillSizesToType.SizePlanWithMins); //TT#848-MD -jsobek -Fill to Size Plan Presentation
				// END MID Track #4921
                // BEGIN TT#616 - AGallagher - Allocation - Pack Rounding (#67)
                this.gbxGenericPackRounding.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Generic_Pack_Rounding);
                this.lblPct1stPackRoundUpFrom.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Generic_Pack_1st_Pack_Rounding_Up_From);
                this.lblPctNthPackRoundUpFrom.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Generic_Pack_Nth_Pack_Rounding_Up_From);
                // END TT#616 - AGallagher - Allocation - Pack Rounding (#67)
				// BEGIN MID Track #6074 - stodd - velocity changes
				// Begin TT # 91 - stodd
				//this.cbxDefaultGradesByBasis.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_DefaultStoreGradesByBasis);
				// End TT # 91 - stodd
				// END MID Track #6074 - stodd - velocity changes

                // Begin TT#413 - JSmith - Add new Generic Size Curve names to correspond with the new Size Curves being generated within Node Properties settings.
                gboGenericSizeCurveName.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_GenericSizeCurveName);
                lblGenericSizeCurveNameUsing.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Use) + ":";
                radGenericSizeCurveName_NodePropertiesName.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_NodePropertiesName);
                radGenericSizeCurveName_HeaderCharacteristics.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_HeaderCharacteristics);
                // End TT#413
				//Begin TT#391 - stodd - 
                this.lblGenericSizeCurveNameUsing.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Use);
				//End TT#391 - stodd - 
                // BEGIN TT#1401 - AGallagher - Reservation Stores
                this.gboItemMaxOverride.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_ItemMaxOverride);
                radItemMaxOverrideYes.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Yes);
                radItemMaxOverrideNo.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_No);
                chkPriorHeaderIncludeReserve.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_PriorHeaderIncludeReserve); //TT#1608-MD - srisch - Working 5.4 - Prior Header 05/27/2015 Needs Changes
                // END TT#1401 - AGallagher - Reservation Stores
                // Begin TT#3899 - JSmith - Remove the “Expand Headers When Intransit is Relieved” option on Headers Tab.
                //cboRIExpand.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_RIExpandInd); // TT#628-MD - JSmith -  Add Option To Not Expand Allocation Records Once Relieved
                // End TT#3899 - JSmith - Remove the “Expand Headers When Intransit is Relieved” option on Headers Tab.
                this.lbl_VSWSizwConstraints.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_VSWSizeConstraints);  // TT#246-MD - AGallagher - VSW Size
                // BEGIN TT#933-MD - AGallagher - Item Max vs. FWOS Max
                gboItemFWOS.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_ItemFWOSMax);
                radItemFWOSDefault.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_ItemFWOSMaxDefault);
                radItemFWOSHighest.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_ItemFWOSMaxHighest);
                radItemFWOSLowest.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_ItemFWOSMaxLowest); 
                // END TT#933-MD - AGallagher - Item Max vs. FWOS Max
                // BEGIN TT#933-MD - AGallagher - Item Max vs. FWOS Max
                gboItemFWOS.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_ItemFWOSMax);
                radItemFWOSDefault.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_ItemFWOSMaxDefault);
                radItemFWOSHighest.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_ItemFWOSMaxHighest);
                radItemFWOSLowest.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_ItemFWOSMaxLowest);
                // END TT#933-MD - AGallagher - Item Max vs. FWOS Max
                // Begin TT#1652-MD - RMatelic - DC Carton Rounding
                gboDCCartonAttribute.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_DCCartonRoundDfltAttribute);
                // End TT#1652-MD 
                // BEGIN TT#1966-MD - AGallagher - DC Fulfillment
                this.gbxMasterSplitOptions.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_DCFulfillmentMasterSplitOptions);
                this.rbDCFulfillment.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_DCFulfillment);
                this.rbProportional.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_DCFulfillmentProportional);
                this.cbxApplyMinimums.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_DCFulfillmentApplyMinimums);
                this.lblPrioritizeHeadersBy.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_DCFulfillmentPrioritizeHeadersBy);
                this.rbHeadersAscending.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_DCFulfillmentHeadersAscending);
                this.rbHeadersDescending.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_DCFulfillmentHeadersDescending);
                this.rbStoresAscending.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_DCFulfillmentStoresAscending);
                this.rbStoresDescending.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_DCFulfillmentStoresDescending);
                this.gbxDCFulfillmentSplitBy.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_DCFulfillmentOptions);
                this.lblSplit.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_DCFulfillmentSplitBy);
                this.radSplitStoreDC.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_DCFulfillmentSplitByStore);
                this.radSplitDCStore.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_DCFulfillmentSplitByDC);
                this.lblReserve.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_DCFulfillmentReserve);
                this.radPreSplit.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_DCFulfillmentReservePreSplit);
                this.radPostSplit.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_DCFulfillmentReservePostSplit);
                this.lblMinimums.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_DCFulfillmentMinimumsApply);
                this.radApplyAllocQty.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_DCFulfillmentMinimumsApplyByQuantity);
                //this.radApplyAllStores.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_DCFulfillmentMinimumsApplyFirst);
                //this.radApplyAllocQty.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_DCFulfillmentMinimumsApplyByQuantity);
                this.radApplyAllStores.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_DCFulfillmentReservePreSplit);
                this.radApplyAllocQty.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_DCFulfillmentReservePostSplit);
                this.lblWithinDC.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_DCFulfillmentWithinDC);
                this.radWithinDCFill.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_DCFulfillmentWithinDCFill);
                this.radWithinDCProportional.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_DCFulfillmentWithinDCProportional);
                // END TT#1966-MD - AGallagher - DC Fulfillment
			}
			catch( Exception exception )
			{
				HandleException(exception);
			}
		}

        // BEGIN TT#1966-MD - AGallagher - DC Fulfillment
        private void BuildHeaderCharList()
        {
            try
            {
                HeaderCharList hcl = new HeaderCharList();
                cboPrioritizeHeadersBy.DataSource = hcl.BuildHeaderCharList();
                cboPrioritizeHeadersBy.DisplayMember = "Text";
                cboPrioritizeHeadersBy.ValueMember = "Key";
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }
        // END TT#1966-MD - AGallagher - DC Fulfillment
		private void tabControl1_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			try
			{
				this.Cursor = Cursors.WaitCursor;
				if (this.tabControl1.SelectedTab.Name != _currentTabPage.Name)
				{
					switch (_currentTabPage.Name)
					{
						case "tabCompInfo":
							if (!ValidateTabCompInfo())
							{
								ErrorFound = true;
								this.tabControl1.SelectedTab = this.tabCompInfo;
							}
							break;
						case "tabStores":
							if (!ValidateTabDisplayOptions())
							{
								ErrorFound = true;
								this.tabControl1.SelectedTab = this.tabStores;
							}
							break;
						case "tabOTS":
							if (!ValidateTabOTS())
							{
								ErrorFound = true;
								this.tabControl1.SelectedTab = this.tabOTS;
							}
							break;
						case "tabAllocDefault":
							if (!ValidateTabAllocDefault())
							{
								ErrorFound = true;
								this.tabControl1.SelectedTab = this.tabAllocDefault;
							}
							break;
					}
					if (ErrorFound)
					{
						ErrorFound = false;
						string text = MIDText.GetTextOnly(eMIDTextCode.msg_ErrorsFoundReviewCorrect);
						MessageBox.Show(text);
					}
					else
					{
						_currentTabPage = this.tabControl1.SelectedTab;
					}
				}

                //BEGIN TT#3509-VStuart-Cannot delete OTS Plan Version-ANFUser
                if (this.tabControl1.SelectedTab.Name == "tabOTS")
                {
                    btnInUse.Visible = true;
                }
                else
                {
                    btnInUse.Visible = false;
                }
                //END  TT#3509-VStuart-Cannot delete OTS Plan Version-ANFUser
            }
			catch( Exception exception )
			{
				HandleException(exception);
			}
			finally
			{
				this.Cursor = Cursors.Default;
			}
		}

		private void uGridVersions_MouseEnterElement(object sender, Infragistics.Win.UIElementEventArgs e)
		{
			try
			{
				ShowUltraGridToolTip(uGridVersions, e);
			}
			catch( Exception exception )
			{
				HandleException(exception);
			}
		}

		private void textBox_TextChanged(object sender, System.EventArgs e)
		{
			if (FormLoaded)
			{
				ChangePending = true;
			}
		}

        // Begin TT#628-MD - JSmith -  Add Option To Not Expand Allocation Records Once Relieved
        void CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (FormLoaded)
            {
                ChangePending = true;
            }
        }
        // End TT#628-MD - JSmith -  Add Option To Not Expand Allocation Records Once Relieved

		// BEGIN ANF Generic Size Constraint
		private void txtCharMask_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			// 120 = 'X', 42 = '*', and 8 = Backspace - the only allowed characters
			_replaceCharacter = false;
			switch (e.KeyChar)
			{
				case (char)88:		// = X
				case (char)8:		// = Backspace
					break;

				case (char)42:		// = *
					TextBox textBox = (TextBox)sender;
					if (textBox.Text.EndsWith("*"))		
					{
						e.Handled = true;			// disallow consecutive asterisks
					}
					break;
	
				default:
					_keyedChar = e.KeyChar;
					_replaceCharacter = true;
					break;
			}
		}

		private void maskTextBox_TextChanged(object sender, System.EventArgs e)
		{
			if (FormLoaded)
			{
				ChangePending = true;
				if (_replaceCharacter)
				{
					int index = ((TextBox)sender).Text.LastIndexOf(_keyedChar);
					((TextBox)sender).Text = ((TextBox)sender).Text.Replace(_keyedChar, 'X');	// replace any other character
					((TextBox)sender).Select(index + 1, 0);
				}
				if (((TextBox)sender).Text != null &&
					((TextBox)sender).Text.Trim().Length > 0 &&
					!((TextBox)sender).Text.EndsWith("*"))
				{
					((TextBox)sender).Text += "*";
				}
			}
		}

		private void maskTextBox_MouseHover(object sender, System.EventArgs e)
		{
			try
			{
				string message = MIDText.GetTextOnly((int)eMIDTextCode.tt_SizeFilterMaskValues);
				ToolTip.Active = true; 
				ToolTip.SetToolTip((System.Windows.Forms.Control)sender, message);
			}
			catch( Exception exception )
			{
				HandleException(exception);
			}
		}
		// END ANF Generic Size Constraint

		private void txtProductLevelDelimiter_TextChanged(object sender, System.EventArgs e)
		{
			if (FormLoaded)
			{
				ChangePending = true;
				_servicesNeedRestarted = true;
			}
		}

		private void cboState_SelectionChangeCommitted(object sender, System.EventArgs e)
		{
			if (FormLoaded)
			{
				ChangePending = true;
				_cboStateValue = Convert.ToString(cboState.SelectedValue, CultureInfo.CurrentCulture);
			}
		}
        //Begin TT#316 -jsobek -Replace all Windows Combobox controls with new enhanced control
        void cboState_MIDComboBoxPropertiesChangedEvent(object source, MIDComboBoxPropertiesChangedEventArgs args)
        {
            this.cboState_SelectionChangeCommitted(source, new EventArgs());
        }
        //End TT#316

		private void comboBox_TextChanged(object sender, System.EventArgs e)
		{
			if (FormLoaded)
			{
				ChangePending = true;
			}
		}

		private void checkbox_CheckedChanged(object sender, System.EventArgs e)
		{
			if (FormLoaded)
			{
				ChangePending = true;
			}
		}

		private void txtNewStorePeriodBegin_TextChanged(object sender, System.EventArgs e)
		{
			if (FormLoaded)
			{
				ChangePending = true;
				_servicesNeedRestarted = true;
			}
		}

		private void txtNewStorePeriodEnd_TextChanged(object sender, System.EventArgs e)
		{
			if (FormLoaded)
			{
				ChangePending = true;
				_servicesNeedRestarted = true;
			}
		}

		private void txtNonCompPeriodBegin_TextChanged(object sender, System.EventArgs e)
		{
			if (FormLoaded)
			{
				ChangePending = true;
				_servicesNeedRestarted = true;
			}
		}

		private void txtNonCompPeriodEnd_TextChanged(object sender, System.EventArgs e)
		{
			if (FormLoaded)
			{
				ChangePending = true;
				_servicesNeedRestarted = true;
			}
		}

		private void cboProduct_SelectionChangeCommitted(object sender, System.EventArgs e)
		{
			if (FormLoaded)
			{
				ChangePending = true;
				_cboProductValue = Convert.ToInt32(cboProduct.SelectedValue, CultureInfo.CurrentCulture);
				_servicesNeedRestarted = true;
			}
		}
        //Begin TT#316 -jsobek -Replace all Windows Combobox controls with new enhanced control
        void cboProduct_MIDComboBoxPropertiesChangedEvent(object source, MIDComboBoxPropertiesChangedEventArgs args)
        {
            this.cboProduct_SelectionChangeCommitted(source, new EventArgs());
        }
        //End TT#316

		private void cboStore_SelectionChangeCommitted(object sender, System.EventArgs e)
		{
			if (FormLoaded)
			{
				ChangePending = true;
				_cboStoreValue = Convert.ToInt32(cboStore.SelectedValue, CultureInfo.CurrentCulture);
				_servicesNeedRestarted = true;
			}
		}
        //Begin TT#316 -jsobek -Replace all Windows Combobox controls with new enhanced control
        void cboStore_MIDComboBoxPropertiesChangedEvent(object source, MIDComboBoxPropertiesChangedEventArgs args)
        {
            this.cboStore_SelectionChangeCommitted(source, new EventArgs());
        }
        //End TT#316

		private void cboPlanStoreAttr_SelectionChangeCommitted(object sender, System.EventArgs e)
		{
			if (FormLoaded)
			{
				ChangePending = true;
				_cboPlanStoreAttrValue = Convert.ToInt32(cboPlanStoreAttr.SelectedValue, CultureInfo.CurrentCulture);
			}
		}
        //Begin TT#316 -jsobek -Replace all Windows Combobox controls with new enhanced control
        void cboPlanStoreAttr_MIDComboBoxPropertiesChangedEvent(object source, MIDComboBoxPropertiesChangedEventArgs args)
        {
            this.cboPlanStoreAttr_SelectionChangeCommitted(source, new EventArgs());
        }
        //End TT#316

		private void cboAllocStoreAttr_SelectionChangeCommitted(object sender, System.EventArgs e)
		{
			if (FormLoaded)
			{
				ChangePending = true;
				_cboAllocStoreAttrValue = Convert.ToInt32(cboAllocStoreAttr.SelectedValue, CultureInfo.CurrentCulture);
			}
		}
        //Begin TT#316 -jsobek -Replace all Windows Combobox controls with new enhanced control
        void cboAllocStoreAttr_MIDComboBoxPropertiesChangedEvent(object source, MIDComboBoxPropertiesChangedEventArgs args)
        {
            this.cboAllocStoreAttr_SelectionChangeCommitted(source, new EventArgs());
        }
        //End TT#316

        // Begin TT#1652-MD - RMatelic - DC Carton Rounding
        private void cboDCCartonRoundDfltAttribute_MIDComboBoxPropertiesChangedEvent(object source, MIDComboBoxPropertiesChangedEventArgs args)
        {
            this.cboDCCartonRoundDfltAttribute_SelectionChangeCommitted(source, new EventArgs());
        }

        private void cboDCCartonRoundDfltAttribute_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (FormLoaded)
            {
                ChangePending = true;
                _cboDCCartonRoundDfltAttrValue = Convert.ToInt32(cboDCCartonRoundDfltAttribute.SelectedValue, CultureInfo.CurrentCulture);
            }
        }
        // End TT#1652-MD

		private void clbHeaderTypeRelease_SelectedValueChanged(object sender, System.EventArgs e)
		{
			if (FormLoaded)
			{
				ChangePending = true;
			}
		}

		private void cboHeaderLinkChar_SelectionChangeCommitted(object sender, System.EventArgs e)
		{
			try
			{
				if (FormLoaded &&
					!FormIsClosing &&
					!_linkCharIndexIsReset)
				{
					int selectedValue = Convert.ToInt32(cboHeaderLinkChar.SelectedValue, CultureInfo.CurrentCulture);
					if (selectedValue > Include.NoRID &&
						MultiHeadersHaveMismatchedValues(selectedValue))
					{
						_linkCharIndexIsReset = true;
						// setting to the new <None> row by value fails so set by index in that case
						if (_cboHeaderLinkCharValue > Include.NoRID)
						{
							cboHeaderLinkChar.SelectedValue = _cboHeaderLinkCharValue;
						}
						else
						{
							cboHeaderLinkChar.SelectedIndex = 0;
						}
						MessageBox.Show (_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_al_LinkedHeaderCharMultiMismatch), this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
					}
					else
					{
						ChangePending = true;
						_cboHeaderLinkCharValue = selectedValue;
					}
				}
				_linkCharIndexIsReset = false;
			}
			catch (Exception ex)
			{
				HandleException(ex, "cboHeaderLinkChar_SelectionChangeCommitted");
			}
		}
        //Begin TT#316 -jsobek -Replace all Windows Combobox controls with new enhanced control
        void cboHeaderLinkChar_MIDComboBoxPropertiesChangedEvent(object source, MIDComboBoxPropertiesChangedEventArgs args)
        {
            this.cboHeaderLinkChar_SelectionChangeCommitted(source, new EventArgs());
        }
        //End TT#316

		private bool MultiHeadersHaveMismatchedValues(int aLinkCharacteristicValue)
		{
			try
			{
				int hdrRID;
				string hdrID;
				int hdrGroupRID;
				int hcRID;
				int multiHcRID;
				object hashValue;
				Hashtable multiHeaders = new Hashtable();
				Header headerData = new Header();
				DataTable dt = headerData.GetMultiHeadersWithLinkCharacteristic(aLinkCharacteristicValue);
				foreach (DataRow dr in dt.Rows)
				{
					hdrRID = Convert.ToInt32(dr["HDR_RID"], CultureInfo.CurrentCulture); 
					hdrID = Convert.ToString(dr["HDR_ID"], CultureInfo.CurrentCulture); 
					hdrGroupRID = Convert.ToInt32(dr["HDR_GROUP_RID"], CultureInfo.CurrentCulture);
					hcRID = Convert.ToInt32(dr["HC_RID"], CultureInfo.CurrentCulture);
					if (hdrGroupRID == Include.UndefinedHeaderGroupRID)
					{
						multiHeaders.Add(hdrRID, hcRID);
					}
					else 
					{
						hashValue = multiHeaders[hdrGroupRID];
						if (hashValue != null)
						{
							multiHcRID = Convert.ToInt32(hashValue, CultureInfo.CurrentCulture);
							if (hcRID != multiHcRID)
							{
								return true;
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				HandleException(ex, "MultiHeadersHaveMismatchedValues");
			}
			return false;
		}

		// BEGIN MID Track #4357 - security for interfaced and non-interfaced headers
		private void cboProtectInterfacedHeaders_CheckedChanged(object sender, System.EventArgs e)
		{
			if (FormLoaded)
			{
				ChangePending = true;
				if (cboProtectInterfacedHeaders.Checked)
				{
					_SAB.ClientServerSession.GlobalOptions.ProtectInterfacedHeadersInd = true;
				}
				else
				{
					_SAB.ClientServerSession.GlobalOptions.ProtectInterfacedHeadersInd = false;
				}
			}
		}
		// END MID Track #4357

		private void sizeFilterCheckBox_CheckedChanged(object sender, System.EventArgs e)
		{
			try
			{
				TextBox textBox = new TextBox();
				CheckBox checkBox = (CheckBox)sender;  
				switch (checkBox.Name)
				{
					case "cboSizeCurve":
						textBox = txtSizeCurve;
						break;
					case "cboSizeGroup":
						textBox = txtSizeGroup;
						break;
					case "cboSizeAlternates":
						textBox = txtSizeAlternates;
						break;
					case "cboSizeConstraints":
						textBox = txtSizeConstraints;
						break;
				}
				if (!checkBox.Checked)
				{
					textBox.Text = null;
				}
				textBox.Enabled =  checkBox.Checked;
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}

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
        //Begin TT#848-MD -jsobek -Fill to Size Plan Presentation
        private void radFillSizesTo_SizePlanWithSizeMins_CheckedChanged(object sender, EventArgs e)
        {
            if (FormLoaded)
            {
                ChangePending = true;
            }
        }
        //End TT#848-MD -jsobek -Fill to Size Plan Presentation

		private void radNormalizeSizeCurves_Yes_CheckedChanged(object sender, System.EventArgs e)
		{
			if (FormLoaded)
			{
				ChangePending = true;
			}
		}

		private void radNormalizeSizeCurves_No_CheckedChanged(object sender, System.EventArgs e)
		{
			if (FormLoaded)
			{
				ChangePending = true;
			}
		}
		// End MID Track #4921

        // BEGIN TT#1401 - AGallagher - Reservation Stores
        private void radItemMaxOverrideYes_CheckedChanged(object sender, System.EventArgs e)
        {
            if (FormLoaded)
            {
                ChangePending = true;
            }
        }

        private void radItemMaxOverrideNo_CheckedChanged(object sender, System.EventArgs e)
        {
            if (FormLoaded)
            {
                ChangePending = true;
            }
        }
        // END TT#1401 - AGallagher - Reservation Stores


        // BEGIN TT#933-MD - AGallagher - Item Max vs. FWOS Max
        private void radItemFWOSDefault_CheckedChanged(object sender, System.EventArgs e)
        {
            if (FormLoaded)
            {
                ChangePending = true;
                _vswItemFWOSMax = eVSWItemFWOSMax.Default;
                _SAB.ClientServerSession.GlobalOptions.VSWItemFWOSMax = _vswItemFWOSMax;
            }
        }
        private void radItemFWOSHighest_CheckedChanged(object sender, System.EventArgs e)
        {
            if (FormLoaded)
            {
                ChangePending = true;
                _vswItemFWOSMax = eVSWItemFWOSMax.Highest;
                _SAB.ClientServerSession.GlobalOptions.VSWItemFWOSMax = _vswItemFWOSMax;
            }
        }
        private void radItemFWOSLowest_CheckedChanged(object sender, System.EventArgs e)
        {
            if (FormLoaded)
            {
                ChangePending = true;
                _vswItemFWOSMax = eVSWItemFWOSMax.Lowest;
                _SAB.ClientServerSession.GlobalOptions.VSWItemFWOSMax = _vswItemFWOSMax;
            }
        }
        // END TT#933-MD - AGallagher - Item Max vs. FWOS Max


        
		override protected void AfterClosing()
		{
			try
			{
				if (_servicesNeedRestarted)
				{
					MessageBox.Show (_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MustRestartServices), this.Text,
						MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				}
			}
			catch (Exception ex)
			{
				HandleException(ex, "AfterClosing");
			}
		}

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

		// BEGIN MID Track 6335 Option to not Release Hdr with all units in reserve
		private void cboDoNotReleaseIfAllInReserve_CheckedChanged(object sender, System.EventArgs e)
		{
			if (FormLoaded)
			{
				ChangePending = true;
				if (cboDoNotReleaseIfAllInReserve.Checked)
				{
					_SAB.ClientServerSession.GlobalOptions.AllowReleaseIfAllUnitsInReserve = false;
				}
				else
				{
					_SAB.ClientServerSession.GlobalOptions.AllowReleaseIfAllUnitsInReserve = true;
				}
			}
		}
        // END MID Track 6335 Option to not Release Hdr with all units in reserve

        // begin TT#1365 - JEllis - FL Detail Packs Size Need Enhancement
        private void cbxStepped_CheckedChanged(object sender, System.EventArgs e)
        {
            if (FormLoaded)
            {
                ChangePending = true;
                if (cbxStepped.Checked)
                {
                    _SAB.ClientServerSession.GlobalOptions.PackToleranceStepped = false;
                }
                else
                {
                    _SAB.ClientServerSession.GlobalOptions.PackToleranceStepped = true;
                }
            }
        }
        private void cbxNoMaxStep_CheckedChanged(object sender, System.EventArgs e)
        {
            if (FormLoaded)
            {
                ChangePending = true;
                if (cbxNoMaxStep.Checked)
                {
                    _SAB.ClientServerSession.GlobalOptions.PackToleranceNoMaxStep = false;
                }
                else
                {
                    _SAB.ClientServerSession.GlobalOptions.PackToleranceNoMaxStep = true;
                }
            }
        }
        // end TT#1365 - JEllis - FL Detail Packs Size Need Enhancement

        private void cboStoreAttr_DragEnter(object sender, DragEventArgs e)
        {
            Image_DragEnter(sender, e);
        }

        private void cboStoreAttr_DragOver(object sender, DragEventArgs e)
        {
            Image_DragOver(sender, e);
        }

		// Begin TT#391 - stodd
		private void cboGenerateSizeCurveUsing_SelectionChangeCommitted(object sender, EventArgs e)
		{
			try
			{
				if (FormLoaded)
				{
					int selectedValue = Convert.ToInt32(cboGenerateSizeCurveUsing.SelectedValue, CultureInfo.CurrentCulture);
					_generateSizeCurveUsing = (eGenerateSizeCurveUsing)selectedValue;
				}
			}
			catch (Exception ex)
			{
				HandleException(ex, "cboGenerateSizeCurveUsing_SelectionChangeCommitted");
			}
		}
        //Begin TT#316 -jsobek -Replace all Windows Combobox controls with new enhanced control
        void cboGenerateSizeCurveUsing_MIDComboBoxPropertiesChangedEvent(object source, MIDComboBoxPropertiesChangedEventArgs args)
        {
            this.cboGenerateSizeCurveUsing_SelectionChangeCommitted(source, new EventArgs());
        }
        //End TT#316

        private void gboSizeOptions_Enter(object sender, EventArgs e)
        {

        }

        private void gbxGenericPackRounding_Enter(object sender, EventArgs e)
        {

        }

        private void lblPct1stPackRoundUpFrom_Click(object sender, EventArgs e)
        {

        }

        private void lblPct1stPackRoundUpFromPct_Click(object sender, EventArgs e)
        {

        }

        private void lblPctNthPackRoundUpFrom_Click(object sender, EventArgs e)
        {

        }
        // End TT#391 - stodd

        //BEGIN TT#46-MD -jsobek -Develop My Activity Log
        private void txtMyActivityMessageUpperLimit_Validating(object sender, CancelEventArgs e)
        {

            int tempLimit;
            if (int.TryParse(txtMyActivityMessageUpperLimit.Text, out tempLimit) == false)
            {
                String sMsg = MIDText.GetTextOnly(eMIDTextCode.msg_MyActivity_MessageLimitInvalid); 

              
                    e.Cancel = true;
               
                MessageBox.Show(sMsg, MIDText.GetTextOnly(eMIDTextCode.msg_MyActivity_MessageLimitInvalidTitle));
                return;
            }
            if (tempLimit < 50)
            {
                String sMsg = MIDText.GetTextOnly(eMIDTextCode.msg_MyActivity_MessageLimitLow);
                
                    e.Cancel = true;

                    MessageBox.Show(sMsg, MIDText.GetTextOnly(eMIDTextCode.msg_MyActivity_MessageLimitInvalidTitle));
                return;
            }
            if (tempLimit > 100000)
            {
                String sMsg = MIDText.GetTextOnly(eMIDTextCode.msg_MyActivity_MessageLimitHigh).Replace("{0}", "100,000") ;
                
                    e.Cancel = true;

                    MessageBox.Show(sMsg, MIDText.GetTextOnly(eMIDTextCode.msg_MyActivity_MessageLimitInvalidTitle));
                return;
            }
            _myActivityMessageUpperLimit = tempLimit;
            
        }
        //END TT#46-MD -jsobek -Develop My Activity Log
		
		//BEGIN TT#3509-VStuart-Cannot delete OTS Plan Version-ANFUser
        private void toolStripMenuDelete_Click(object sender, EventArgs e)
        {
            // This is where the Delete goes.
            uGridVersions.DeleteSelectedRows();
        }

	    private void toolStripMenuInUse_Click(object sender, EventArgs e)
        {
            //"This where the In Use code goes."
            _ridList = new ArrayList();
            //Band[0] is for the Parent Rows.
            UltraGridColumn fvRidColumn = uGridVersions.DisplayLayout.Bands[0].Columns["FV_RID"];
            foreach (UltraGridRow row in uGridVersions.Selected.Rows)
            {
                if (row.Band.Key == "Plan Version")
                {
                    etype = eProfileType.Version;
                    int fvRid = Convert.ToInt32(row.GetCellValue(fvRidColumn), CultureInfo.CurrentUICulture);
                    if (row.Band != uGridVersions.DisplayLayout.Bands[0]) continue;
                    if (fvRid > 0)
                    {
                        _ridList.Add(fvRid);
                    }
                }
            }
            //If the no RID is selected do nothing.
            if (_ridList.Count > 0)
            {
                //const eProfileType etype = eProfileType.Version;
                //string inUseTitle = Regex.Replace(etype.ToString(), "((?<=[a-z])[A-Z]|[A-Z](?=[a-z]))", " $1").Trim(); //TT#1532-MD -jsobek -Add In Use for Header Characteristics
                string inUseTitle = InUseUtility.GetInUseTitleFromProfileType(etype); //TT#1532-MD -jsobek -Add In Use for Header Characteristics
                bool display = false;
                //bool inQuiry = true; 
                DisplayInUseForm(_ridList, etype, inUseTitle, ref display, inQuiry);
            }

        }

        private void btnInUse_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("You have clicked the In Use button.");
            toolStripMenuInUse_Click(sender, e);
         }

        //END  TT#3509-VStuart-Cannot delete OTS Plan Version-ANFUser

        // BEGIN TT#1966-MD - AGallagher - DC Fulfillment


        private void rbStoresDescending_CheckedChanged(object sender, EventArgs e)
        {
            if (FormLoaded && rbStoresDescending.Checked)
            {
                ChangePending = true;
                _store_Order = eDCFulfillmentStoresOrder.Descending;
            }
        }
        private void rbStoresAscending_CheckedChanged(object sender, EventArgs e)
        {
            if (FormLoaded && rbStoresAscending.Checked)
            {
                ChangePending = true;
                _store_Order = eDCFulfillmentStoresOrder.Ascending;
            }
        }
        private void radSplitStoreDC_CheckedChanged(object sender, EventArgs e)
        {
            if (FormLoaded && radSplitStoreDC.Checked)
            {
                ChangePending = true;
                _split_By_Option = eDCFulfillmentSplitByOption.SplitByStore;
            }
        }
        private void radSplitDCStore_CheckedChanged(object sender, EventArgs e)
        {
            if (FormLoaded && radSplitDCStore.Checked)
            {
                ChangePending = true;
                _split_By_Option = eDCFulfillmentSplitByOption.SplitByDC;
            }
        }
        private void radPreSplit_CheckedChanged(object sender, EventArgs e)
        {
            if (FormLoaded && radPreSplit.Checked)
            {
                ChangePending = true;
                _split_By_Reserve = eDCFulfillmentReserve.ReservePreSplit;
            }
        }
        private void radPostSplit_CheckedChanged(object sender, EventArgs e)
        {
            if (FormLoaded && radPostSplit.Checked)
            {
                ChangePending = true;
                _split_By_Reserve = eDCFulfillmentReserve.ReservePostSplit;
            }
        }
        private void radApplyAllocQty_CheckedChanged(object sender, EventArgs e)
        {
            if (FormLoaded && radApplyAllocQty.Checked)
            {
                ChangePending = true;
                _apply_By = eDCFulfillmentMinimums.ApplyByQty;
            }
        }
        private void radApplyAllStores_CheckedChanged(object sender, EventArgs e)
        {
            if (FormLoaded && radApplyAllStores.Checked)
            {
                ChangePending = true;
                _apply_By = eDCFulfillmentMinimums.ApplyFirst;
            }
        }
        private void radWithinDCFill_CheckedChanged(object sender, EventArgs e)
        {
            if (FormLoaded && radWithinDCFill.Checked)
            {
                ChangePending = true;
                _within_Dc = eDCFulfillmentWithinDC.Fill;
            }
        }
        private void radWithinDCProportional_CheckedChanged(object sender, EventArgs e)
        {
            if (FormLoaded && radWithinDCProportional.Checked)
            {
                ChangePending = true;
                _within_Dc = eDCFulfillmentWithinDC.Proportional;
            }
        }
        private void ugOrderStoresBy_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            AddOrderStoresByValueLists();
            OrderStoresByGridLayout();
            PopulateOrderStoresByValueLists();
            MIDRetail.Windows.Controls.UltraGridLayoutDefaults ugld = new MIDRetail.Windows.Controls.UltraGridLayoutDefaults(ErrorImage);
            ugld.ApplyDefaults((Infragistics.Win.UltraWinGrid.UltraGrid)sender, e, true, 2, false);
            BuildGridContextMenu();
        }
        private void AddOrderStoresByValueLists()
        {
            try
            {
                
                this.ugOrderStoresBy.DisplayLayout.ValueLists.Clear();
                this.ugOrderStoresBy.DisplayLayout.ValueLists.Add("StoreChars");
                this.ugOrderStoresBy.DisplayLayout.ValueLists.Add("DistCenter");
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void PopulateOrderStoresByValueLists()
        {
            try
            {
                Infragistics.Win.ValueListItem valListItem;

                this.ugOrderStoresBy.DisplayLayout.ValueLists["StoreChars"].ValueListItems.Clear();

                StoreData storeCharMaint = new StoreData();
                DataTable dt = storeCharMaint.StoreCharGroup_Read();
                foreach (DataRow row in dt.Rows)
                {
                    if ((eStoreCharType)Convert.ToInt32(row["SCG_TYPE"]) == eStoreCharType.number)
                    {
                        valListItem = new Infragistics.Win.ValueListItem();
                        valListItem.DataValue = Convert.ToInt32(row["SCG_RID"]);
                        valListItem.DisplayText = Convert.ToString(row["SCG_ID"]);
                        this.ugOrderStoresBy.DisplayLayout.ValueLists["StoreChars"].ValueListItems.Add(valListItem);
                    }
                }

                this.ugOrderStoresBy.DisplayLayout.ValueLists["DistCenter"].ValueListItems.Clear();

                Header headerData = new Header();
                dt = headerData.GetDistCenters();
                foreach (DataRow row in dt.Rows)
                {
                    valListItem = new Infragistics.Win.ValueListItem();
                    valListItem.DataValue = Convert.ToString(row["DIST_CENTER"]);
                    valListItem.DisplayText = Convert.ToString(row["DIST_CENTER"]);
                    this.ugOrderStoresBy.DisplayLayout.ValueLists["DistCenter"].ValueListItems.Add(valListItem);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void OrderStoresByGridLayout()
        {
            try
            {
                this.ugOrderStoresBy.DisplayLayout.AddNewBox.Hidden = false;
                this.ugOrderStoresBy.DisplayLayout.GroupByBox.Hidden = true;
                this.ugOrderStoresBy.DisplayLayout.GroupByBox.Prompt = string.Empty;
                this.ugOrderStoresBy.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
                this.ugOrderStoresBy.DisplayLayout.Bands[0].AddButtonCaption = MIDText.GetTextOnly(eMIDTextCode.lbl_DCFulfillmentDC);
                this.ugOrderStoresBy.DisplayLayout.Bands[0].Override.RowSelectors = DefaultableBoolean.True;

                this.ugOrderStoresBy.DisplayLayout.Bands[0].Columns["SYSTEM_OPTION_RID"].Hidden = true;
                this.ugOrderStoresBy.DisplayLayout.Bands[0].Columns["SEQ"].Hidden = true;
                this.ugOrderStoresBy.DisplayLayout.Bands[0].Columns["DIST_CENTER"].Width = 160;
                this.ugOrderStoresBy.DisplayLayout.Bands[0].Columns["DIST_CENTER"].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right;
                this.ugOrderStoresBy.DisplayLayout.Bands[0].Columns["DIST_CENTER"].AutoCompleteMode = Infragistics.Win.AutoCompleteMode.SuggestAppend;
                this.ugOrderStoresBy.DisplayLayout.Bands[0].Columns["DIST_CENTER"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDown;
                this.ugOrderStoresBy.DisplayLayout.Bands[0].Columns["DIST_CENTER"].ValueList = ugOrderStoresBy.DisplayLayout.ValueLists["DistCenter"];
                this.ugOrderStoresBy.DisplayLayout.Bands[0].Columns["SCG_RID"].Width = 160;
                this.ugOrderStoresBy.DisplayLayout.Bands[0].Columns["SCG_RID"].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right;
                this.ugOrderStoresBy.DisplayLayout.Bands[0].Columns["SCG_RID"].AutoCompleteMode = Infragistics.Win.AutoCompleteMode.SuggestAppend;
                this.ugOrderStoresBy.DisplayLayout.Bands[0].Columns["SCG_RID"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                this.ugOrderStoresBy.DisplayLayout.Bands[0].Columns["SCG_RID"].ValueList = ugOrderStoresBy.DisplayLayout.ValueLists["StoreChars"];

                this.ugOrderStoresBy.DisplayLayout.CaptionVisible = DefaultableBoolean.True;
                this.ugOrderStoresBy.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_DCFulfillmentOrderStoresByGrid);
                this.ugOrderStoresBy.DisplayLayout.Bands[0].Columns["SCG_RID"].Header.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_DCFulfillmentStoreCharacteristic);
                this.ugOrderStoresBy.DisplayLayout.Bands[0].Columns["DIST_CENTER"].Header.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_DCFulfillmentDC);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }
        private void cbxApplyMinimums_CheckedChanged(object sender, EventArgs e)
        {
            if (FormLoaded)
            {
                ChangePending = true;
                if (cbxApplyMinimums.Checked == true)
                { _apply_Minimums_Ind = '1'; }
                else
                { _apply_Minimums_Ind = '0'; }
            }
        }
        private void ugOrderStoresBy_AfterRowInsert(object sender, RowEventArgs e)
        {
            ChangePending = true;
            _DCFChangePending = true;
            e.Row.Cells["SYSTEM_OPTION_RID"].Value = 1;
            e.Row.Cells["SEQ"].Value = ugOrderStoresBy.Rows.Count;
        }
        private void ugOrderStoresBy_MouseEnterElement(object sender, UIElementEventArgs e)
        {
            ShowUltraGridToolTip(ugOrderStoresBy, e);
        }
        
        private void rbProportional_CheckedChanged(object sender, EventArgs e)
        {
            if (FormLoaded && rbProportional.Checked)
            {
                ChangePending = true;
                _split_Option = eDCFulfillmentSplitOption.Proportional;
            }
        }
        private void rbDCFulfillment_CheckedChanged(object sender, EventArgs e)
        {
            if (FormLoaded && rbDCFulfillment.Checked)
            {
                ChangePending = true;
                _split_Option = eDCFulfillmentSplitOption.DCFulfillment;
            }
        }
        private void rbHeadersDescending_CheckedChanged(object sender, EventArgs e)
        {
            if (FormLoaded && rbHeadersDescending.Checked)
            {
                ChangePending = true;
                _header_Order = eDCFulfillmentHeadersOrder.Descending;
            }
        }
        private void rbHeadersAscending_CheckedChanged(object sender, EventArgs e)
        {
            if (FormLoaded && rbHeadersAscending.Checked)
            {
                ChangePending = true;
                _header_Order = eDCFulfillmentHeadersOrder.Ascending;
            }
        }
        private void cboPrioritizeHeadersBy_MIDComboBoxPropertiesChangedEvent(object source, MIDComboBoxPropertiesChangedEventArgs args)
        {
            this.cboPrioritizeHeadersBy_SelectionChangeCommitted(source, new EventArgs());
        }
        private void cboPrioritizeHeadersBy_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (FormLoaded)
            {
                ChangePending = true;
                try
                {
                    if (cboPrioritizeHeadersBy.SelectedIndex != -1)
                    {
                        HeaderFieldCharEntry hfce = (HeaderFieldCharEntry)cboPrioritizeHeadersBy.SelectedItem;
                        _prioritize_Type = hfce.Type;
                        if (hfce.Type == 'C')
                        {
                            _hcg_Rid = hfce.Key;
                            _header_Field = Include.NoRID;
                        }
                        else
                        {
                            _header_Field = hfce.Key;
                            _hcg_Rid = Include.NoRID;
                        }
                    }
                }
                catch (Exception ex)
                {
                    HandleException(ex);
                }
            }
        }
        
        // END TT#1966-MD - AGallagher - DC Fulfillment

        //Begin TT#901-MD -jsobek -Batch Only Mode
        private void btnSendMsg_Click(object sender, EventArgs e)
        {
            if (txtSendMsg.Text == string.Empty)
            {
                MessageBox.Show(((int)eMIDTextCode.msg_RemoteSystemOptions_ProvideMessageToSend).ToString() + ": " + MIDText.ReplaceControlChars(MIDText.GetTextOnly(eMIDTextCode.msg_RemoteSystemOptions_ProvideMessageToSend))); //"Please type a message to send."
            }
            else
            {
                string msg = "From: " + _SAB.ClientServerSession.GetUserName(_SAB.ClientServerSession.UserRID) + " on " + _SAB.ClientServerSession.GetMachineName() + System.Environment.NewLine + this.txtSendMsg.Text;
                _SAB.clientSocketManager.SendCommandToServer(SocketSharedRoutines.SocketServerCommands.IssueShowMessage, msg);
                MessageBox.Show(MIDText.GetTextOnly(eMIDTextCode.msg_RemoteSystemOptions_MessageSent)); //"Message has been sent to clients."
            }
        }

        private void btnBatchModeTurnOn_Click(object sender, EventArgs e)
        {
            if (_SAB.IsApplicationInBatchOnlyMode())
            {
                //batch mode already on - let this user know
                MessageBox.Show(MIDText.GetTextOnly(eMIDTextCode.msg_RemoteSystemOptions_BatchOnlyModeAlreadyOnPrefix) + _SAB.GetBatchModeLastChangedBy()); //"Batch Only Mode has already been turned ON by: "
            }
            else
            {
                _SAB.clientSocketManager.SendCommandToServer(SocketSharedRoutines.SocketServerCommands.SetBatchOnlyModeOn, MakeCurrentUserAndTimeTag());
                MessageBox.Show(MIDText.GetTextOnly(eMIDTextCode.msg_RemoteSystemOptions_BatchOnlyModeWillTurnOn)); //"Batch Mode will be turned ON.  Please wait while client applications shut down."
            }
            SetBatchModeOptions();
        }
        private string MakeCurrentUserAndTimeTag()
        {
            return _SAB.ClientServerSession.GetUserName(_SAB.ClientServerSession.UserRID) + " on " + _SAB.ClientServerSession.GetMachineName() + System.Environment.NewLine + " at " + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString();
        }
        private void btnBatchModeTurnOff_Click(object sender, EventArgs e)
        {
            if (_SAB.IsApplicationInBatchOnlyMode() == false)
            {
                //batch mode already off - let this user know
                MessageBox.Show(MIDText.GetTextOnly(eMIDTextCode.msg_RemoteSystemOptions_BatchOnlyModeAlreadyOffPrefix) + _SAB.GetBatchModeLastChangedBy()); //"Batch Only Mode has already been turned OFF by: "
            }
            else
            {
                _SAB.clientSocketManager.SendCommandToServer(SocketSharedRoutines.SocketServerCommands.SetBatchOnlyModeOff, MakeCurrentUserAndTimeTag());
                MessageBox.Show(MIDText.GetTextOnly(eMIDTextCode.msg_RemoteSystemOptions_BatchOnlyModeWillTurnOff)); //"Batch Mode will be turned OFF."
            }
            SetBatchModeOptions();
        }
        private void SetBatchModeOptions()
        {
            this.gbSystem.Text = _SAB.clientSocketManager.connectedMessage;
            this.lblBatchModeLastChanged.Text = _SAB.GetBatchModeLastChangedBy();
            if (_SAB.IsApplicationInBatchOnlyMode())
            {
                this.btnBatchModeTurnOff.Enabled = true;
                this.btnBatchModeTurnOn.Enabled = false;
                lblBatchOnlyMode.Text = MIDText.GetTextOnly(eMIDTextCode.msg_RemoteSystemOptions_BatchOnlyModeTurnedOn); //"Batch Only Mode is ON";
            }
            else
            {
                this.btnBatchModeTurnOff.Enabled = false;
                this.btnBatchModeTurnOn.Enabled = true;
                lblBatchOnlyMode.Text = MIDText.GetTextOnly(eMIDTextCode.msg_RemoteSystemOptions_BatchOnlyModeTurnedOff); //"Batch Only Mode is OFF";
            }
        }

        private void ultraToolbarsManager1_ToolClick(object sender, Infragistics.Win.UltraWinToolbars.ToolClickEventArgs e)
        {
            switch (e.Tool.Key)
            {
                case "btnShowCurrentUsers":
                    ShowCurrentUsers();
                    break;

                case "btnLogOutSelected":
                    LogOutSelectedUsers();
                    break;
            }
        }
       
        //private DateTime? _ShowCurrentUsersStartTime = null;
        private void ShowCurrentUsers()
        {
            //bool doSendCommand = true;
            //if (_ShowCurrentUsersStartTime != null)
            //{
            //    //make users wait 15 seconds between clicks
            //    TimeSpan ts = DateTime.Now - (DateTime)_ShowCurrentUsersStartTime;
            //    if (ts.TotalSeconds < 15)
            //    {
            //        MessageBox.Show("Please wait while the command is processing.");
            //    }
            //}

            //if (doSendCommand)
            //{
            try
            {
                //this.ultraToolbarsManager1.Tools["btnShowCurrentUsers"].SharedProps.Enabled = false;
                //Cursor.Current = Cursors.WaitCursor;

                _SAB.globalOptionsUpdateCurrentUserCount = new SessionAddressBlock.UpdateCurrentUserCountDelegate(UpdateCurrentUserCountAndReenableButton);
                //_ShowCurrentUsersStartTime = DateTime.Now;
                ClearCurrentUserCountAndDisableButton();
                _SAB.InitializeCurrentUserDataSet();

                BindingSource bs = new BindingSource(_SAB.dsCurrentUsers, _SAB.dsCurrentUsers.Tables[0].TableName);
                this.ultraGrid1.DataSource = bs;
                this.ultraGrid1.DisplayLayout.Bands[0].PerformAutoResizeColumns(false, Infragistics.Win.UltraWinGrid.PerformAutoSizeType.AllRowsInBand);

                _SAB.clientSocketManager.SendCommandToServer(SocketSharedRoutines.SocketServerCommands.GetCurrentUsers, "");
            }
            finally
            {
                //this.ultraToolbarsManager1.Tools["btnShowCurrentUsers"].SharedProps.Enabled = true;
                //Cursor.Current = Cursors.Default;
            }
            //}
        }
        private void LogOutSelectedUsers()
        {
            DataRow[] drSelectedUsers = GetSelectedUsers();
            if (drSelectedUsers.Length > 0)
            {
                string taginfo = string.Empty;
                taginfo = SocketSharedRoutines.Tags.issuedByStart + _SAB.ClientServerSession.GetUserName(_SAB.ClientServerSession.UserRID) + " on " + _SAB.ClientServerSession.GetMachineName() + System.Environment.NewLine + " at " + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + SocketSharedRoutines.Tags.issuedByEnd;
                foreach (DataRow dr in drSelectedUsers)
                {
                    taginfo += SocketSharedRoutines.Tags.rowStart;
                    taginfo += SocketSharedRoutines.MakeUserNameForTagInfo((string)dr["USER_NAME"]);
                    taginfo += SocketSharedRoutines.MakeClientIPForTagInfo((string)dr["CLIENT_IP"]);
                    string sPort = (string)dr["CLIENT_PORT"];
                    int iPort;
                    int.TryParse(sPort, out iPort);
                    taginfo += SocketSharedRoutines.MakeClientPortForTagInfo(iPort);
                    taginfo += SocketSharedRoutines.Tags.rowEnd;
                }
                _SAB.clientSocketManager.SendCommandToServer(SocketSharedRoutines.SocketServerCommands.IssueShutDownForSelectedUsers, taginfo);
                MessageBox.Show(MIDText.GetTextOnly(eMIDTextCode.msg_RemoteSystemOptions_LogOutCommandSent)); //"Log Out command has been sent to selected users."
            }
            else
            {
                MessageBox.Show(((int)eMIDTextCode.msg_RemoteSystemOptions_SelectOneOrMoreUsers).ToString() + ": " + MIDText.ReplaceControlChars(MIDText.GetTextOnly(eMIDTextCode.msg_RemoteSystemOptions_SelectOneOrMoreUsers))); //"Please select one or more users to log out."
            }
        }

        private void ultraGrid1_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            e.Layout.Bands[0].Columns["CLIENT_PORT"].Hidden = true;
            e.Layout.Bands[0].Columns["APP_STATUS"].Hidden = true;

            e.Layout.Bands[0].Columns["USER_NAME"].Header.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_RemoteSystemOptions_ShowCurrentUserGrid_UserColumn); // "User";
            e.Layout.Bands[0].Columns["CLIENT_TYPE"].Header.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_RemoteSystemOptions_ShowCurrentUserGrid_ClientType); // "Client Type";
            e.Layout.Bands[0].Columns["MACHINE_NAME"].Header.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_RemoteSystemOptions_ShowCurrentUserGrid_MachineColumn); //"Machine";
            e.Layout.Bands[0].Columns["CLIENT_IP"].Header.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_RemoteSystemOptions_ShowCurrentUserGrid_IPAddressColumn); //"IP Address";

        }
        public void UpdateCurrentUserCountAndReenableButton(int currentUserCount)
        {
            this.ultraToolbarsManager1.Tools["lblCurrentUserCount"].SharedProps.Caption = currentUserCount.ToString() + MIDText.GetTextOnly(eMIDTextCode.msg_RemoteSystemOptions_UsersLoggedInSuffix); //" user(s) logged in"
            this.ultraToolbarsManager1.Tools["btnShowCurrentUsers"].SharedProps.Enabled = true;
            //Cursor.Current = Cursors.Default;
            //this.UseWaitCursor = false;
            Application.UseWaitCursor = false;
            Application.DoEvents();
        }
        private void ClearCurrentUserCountAndDisableButton()
        {
            this.ultraToolbarsManager1.Tools["lblCurrentUserCount"].SharedProps.Caption = string.Empty;
          
            this.ultraToolbarsManager1.Tools["btnShowCurrentUsers"].SharedProps.Enabled = false;
            //Cursor.Current = Cursors.WaitCursor;
            //this.UseWaitCursor = true;
            Application.UseWaitCursor = true;
            Application.DoEvents();
        }
        public DataRow[] GetSelectedUsers()
        {
            List<DataRow> drList = new List<DataRow>();
            foreach (Infragistics.Win.UltraWinGrid.UltraGridRow ur in this.ultraGrid1.Selected.Rows)
            {
                if (ur.ListObject != null)
                {
                    DataRow dr = ((DataRowView)ur.ListObject).Row;
                    drList.Add(dr);
                }
            }
            return drList.ToArray<DataRow>();
        }

        // BEGIN TT#1608-MD - srisch - Working 5.4 - Prior Header 05/27/2015 
        private void chkPriorHeaderIncludeReserve_CheckedChanged(object sender, EventArgs e)
        {
            {
                if (FormLoaded)
                {
                    ChangePending = true;
                }
            }
        }
        //END TT#1608-MD - srisch - Working 5.4 - Prior Header 05/27/2015 
      
        //End TT#901-MD -jsobek -Batch Only Mode

        // BEGIN TT#1966-MD - AGallagher - DC Fulfillment

        
        private void ugOrderStoresBy_AfterCellListCloseUp(object sender, Infragistics.Win.UltraWinGrid.CellEventArgs e)
        {
            _DCFChangePending = true;
        }
        private void BuildGridContextMenu()
        {
            MenuItem mnuItemInsert = new MenuItem(MIDText.GetTextOnly(eMIDTextCode.lbl_Insert));
            MenuItem mnuItemInsertBefore = new MenuItem(MIDText.GetTextOnly(eMIDTextCode.lbl_Insert_Before));
            MenuItem mnuItemInsertAfter = new MenuItem(MIDText.GetTextOnly(eMIDTextCode.lbl_Insert_after));
            MenuItem mnuItemDelete = new MenuItem(MIDText.GetTextOnly(eMIDTextCode.lbl_Delete));
            MenuItem mnuItemDeleteAll = new MenuItem(MIDText.GetTextOnly(eMIDTextCode.lbl_DeleteAll));

            mnuGrids.MenuItems.Add(mnuItemInsert);
            mnuItemInsert.MenuItems.Add(mnuItemInsertBefore);
            mnuItemInsert.MenuItems.Add(mnuItemInsertAfter);
            mnuGrids.MenuItems.Add(mnuItemDelete);
            mnuGrids.MenuItems.Add(mnuItemDeleteAll);

            mnuItemInsert.Click += new System.EventHandler(this.mnuGridsItemInsert_Click);
            mnuItemInsertBefore.Click += new System.EventHandler(this.mnuGridsItemInsertBefore_Click);
            mnuItemInsertAfter.Click += new System.EventHandler(this.mnuGridsItemInsertAfter_Click);
            mnuItemDelete.Click += new System.EventHandler(this.mnuGridsItemDelete_Click);
            mnuItemDeleteAll.Click += new System.EventHandler(this.mnuGridsItemDeleteAll_Click);

            ugOrderStoresBy.ContextMenu = mnuGrids;
        }

        private void mnuGridsItemInsert_Click(object sender, System.EventArgs e)
        {
        }

        private void mnuGridsItemInsertBefore_Click(object sender, System.EventArgs e)
        {
            try
            {
                _DCFChangePending = true;
                int rowPosition = 0;
                if (ugOrderStoresBy.Rows.Count > 0)
                {
                    if (this.ugOrderStoresBy.ActiveRow == null) return;
                    rowPosition = Convert.ToInt32(this.ugOrderStoresBy.ActiveRow.Cells["SEQ"].Value, CultureInfo.CurrentUICulture);
                    // increment the position of the active row to end of grid
                    foreach (UltraGridRow gridRow in ugOrderStoresBy.Rows)
                    {
                        if (Convert.ToInt32(gridRow.Cells["SEQ"].Value, CultureInfo.CurrentUICulture) >= rowPosition)
                        {
                            gridRow.Cells["SEQ"].Value = Convert.ToInt32(gridRow.Cells["SEQ"].Value, CultureInfo.CurrentUICulture) + 1;
                        }
                    }
                }
                UltraGridRow addedRow = this.ugOrderStoresBy.DisplayLayout.Bands[0].AddNew();
      
                addedRow.Cells["SEQ"].Value = rowPosition;
                this.ugOrderStoresBy.DisplayLayout.Bands[0].SortedColumns.Clear();
                this.ugOrderStoresBy.DisplayLayout.Bands[0].SortedColumns.Add("SEQ", false);
            }
            catch (Exception exception)
            {
                HandleException(exception);
            }
        }

        private void mnuGridsItemInsertAfter_Click(object sender, System.EventArgs e)
        {
            try
            {
                _DCFChangePending = true;
                int rowPosition = 0;
                if (ugOrderStoresBy.Rows.Count > 0)
                {
                    if (this.ugOrderStoresBy.ActiveRow == null) return;
                    rowPosition = Convert.ToInt32(this.ugOrderStoresBy.ActiveRow.Cells["SEQ"].Value, CultureInfo.CurrentUICulture);
                    // increment the position of the active row to end of grid
                    foreach (UltraGridRow gridRow in ugOrderStoresBy.Rows)
                    {
                        if (Convert.ToInt32(gridRow.Cells["SEQ"].Value, CultureInfo.CurrentUICulture) > rowPosition)
                        {
                            gridRow.Cells["SEQ"].Value = Convert.ToInt32(gridRow.Cells["SEQ"].Value, CultureInfo.CurrentUICulture) + 1;
                        }
                    }
                }
  
                UltraGridRow addedRow = this.ugOrderStoresBy.DisplayLayout.Bands[0].AddNew();
  
                addedRow.Cells["SEQ"].Value = rowPosition + 1;
                this.ugOrderStoresBy.DisplayLayout.Bands[0].SortedColumns.Clear();
                this.ugOrderStoresBy.DisplayLayout.Bands[0].SortedColumns.Add("SEQ", false);
             }
            catch (Exception exception)
            {
                HandleException(exception);
            }
        }

        private void mnuGridsItemDelete_Click(object sender, System.EventArgs e)
        {
            try
            {
                _DCFChangePending = true;
                ugOrderStoresBy.DeleteSelectedRows();
            }
            catch (Exception exc)
            {
                HandleException(exc);
            }
        }

        private void mnuGridsItemDeleteAll_Click(object sender, System.EventArgs e)
        {
            try
            {
                _DCFChangePending = true;
                dtdcf.Clear();
                dtdcf.AcceptChanges();
                ChangePending = true;
            }
            catch (Exception exc)
            {
                HandleException(exc);
            }
        }
 
        private void groupBox5_Enter(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
        // END TT#1966-MD - AGallagher - DC Fulfillment
		
	
		#region MIDFormBase Overrides

		#endregion MIDFormBase Overrides
	}

    //public class BasisLabelVariableEntry
    //{
    //    //=======
    //    // FIELDS
    //    //=======

    //    private int _systemOptionRID;
    //    private int _labelType;
    //    private string _labelText;
    //    private int _labelSequence;

    //    //=============
    //    // CONSTRUCTORS
    //    //=============

    //    public BasisLabelVariableEntry(DataRow aDataRow)
    //    {
    //        try
    //        {
    //            LoadFromDataRow(aDataRow);
    //        }
    //        catch (Exception exc)
    //        {
    //            string message = exc.ToString();
    //            throw;
    //        }
    //    }

    //    public BasisLabelVariableEntry(int aSystemOptionRID, int aLabelType, string aLabelText, int aLabelSequence)
    //    {
    //        try
    //        {
    //            _systemOptionRID = aSystemOptionRID;
    //            _labelType = aLabelType;
    //            _labelText = aLabelText;
    //            _labelSequence = aLabelSequence;
    //        }
    //        catch (Exception exc)
    //        {
    //            string message = exc.ToString();
    //            throw;
    //        }
    //    }

    //    //===========
    //    // PROPERTIES
    //    //===========

    //    public int SystemOptionRID
    //    {
    //        get
    //        {
    //            return _systemOptionRID;
    //        }
    //        set
    //        {
    //            _systemOptionRID = value;
    //        }
    //    }

    //    public int LabelType
    //    {
    //        get
    //        {
    //            return _labelType;
    //        }
    //        set
    //        {
    //            _labelType = value;
    //        }
    //    }

    //    public string LabelText
    //    {
    //        get
    //        {
    //            return _labelText;
    //        }
    //        set
    //        {
    //            _labelText = value;
    //        }
    //    }

    //    public int LabelSequence
    //    {
    //        get
    //        {
    //            return _labelSequence;
    //        }
    //        set
    //        {
    //            _labelSequence = value;
    //        }
    //    }

    //    //========
    //    // METHODS
    //    //========

    //    private void LoadFromDataRow(DataRow aRow)
    //    {
    //        try
    //        {
    //            _systemOptionRID = Convert.ToInt32(aRow["SYSTEM_OPTION_RID"], CultureInfo.CurrentUICulture);
    //            _labelType = Convert.ToInt32(aRow["LABEL_TYPE"], CultureInfo.CurrentCulture);
    //            _labelText = "";
    //            _labelSequence = Convert.ToInt32(aRow["LABEL_SEQ"], CultureInfo.CurrentUICulture);
    //        }
    //        catch (Exception exc)
    //        {
    //            string message = exc.ToString();
    //            throw;
    //        }
    //    }

    //    public DataRow UnloadToDataRow(DataRow aRow)
    //    {
    //        try
    //        {
    //            aRow["SYSTEM_OPTION_RID"] = _systemOptionRID;
    //            aRow["LABEL_TYPE"] = _labelType;
    //            aRow["LABEL_TEXT"] = _labelType;
    //            aRow["LABEL_SEQ"] = _labelSequence;

    //            return aRow;
    //        }
    //        catch (Exception exc)
    //        {
    //            string message = exc.ToString();
    //            throw;
    //        }
    //    }
    //}

    //public class BasisLabelTypeProfile : Profile
    //{
    //    //=======
    //    // FIELDS
    //    //=======

    //    private int _key;
    //    private int _systemOptionRID;
    //    private string _name;
    //    private int _type;
    //    private int _sequence;

    //    //=============
    //    // CONSTRUCTORS
    //    //=============

    //    /// <summary>
    //    /// Creates a new instance of BasisLabelTypeProfile.
    //    /// </summary>
    //    /// <param name="aKey">
    //    /// The integer that identifies the logical RID of this coordinate.
    //    /// </param>

    //    /// <summary>
    //    /// Returns the eProfileType of this profile.
    //    /// </summary>

    //    public BasisLabelTypeProfile(int aKey)
    //        : base(aKey)
    //    {
    //        _key = aKey;
    //    }

    //    override public eProfileType ProfileType
    //    {
    //        get
    //        {
    //            return eProfileType.BasisLabelType;

    //        }
    //    }

    //    public int BasisLabelSystemOptionRID
    //    {
    //        get
    //        {
    //            return _systemOptionRID;
    //        }
    //        set
    //        {
    //            _systemOptionRID = value;
    //        }
    //    }

    //    public int BasisLabelType
    //    {
    //        get
    //        {
    //            return _type;
    //        }
    //        set
    //        {
    //            _type = value;
    //        }
    //    }

    //    public string BasisLabelName
    //    {
    //        get
    //        {
    //            return _name;
    //        }
    //        set
    //        {
    //            _name = value;
    //        }
    //    }

    //    public int BasisLabelSequence
    //    {
    //        get
    //        {
    //            return _sequence;
    //        }
    //        set
    //        {
    //            _sequence = value;
    //        }
    //    }
    //    // BEGIN TT#1966-MD - AGallagher - DC Fulfillment
    //    public int DCFulfillmentSystemOptionRID
    //    {
    //        get
    //        {
    //            return _systemOptionRID;
    //        }
    //        set
    //        {
    //            _systemOptionRID = value;
    //        }
    //    }
    //    // END TT#1966-MD - AGallagher - DC Fulfillment

    //    //===========
    //    // PROPERTIES
    //    //===========

    //    /// <summary>
    //    /// Gets the integer key of this coordinate.
    //    /// </summary>

    //    public int Key
    //    {
    //        get
    //        {
    //            return _key;
    //        }
    //        set
    //        {
    //            _key = value;
    //        }
    //    }

    //    //========
    //    // METHODS
    //    //========

    //    /// <summary>
    //    /// Creates a copy of this BasisLabelTypeProfile.
    //    /// </summary>
    //    /// <returns>
    //    /// A new instance of BasisLabelTypeProfile with a copy of this objects information.
    //    /// </returns>

    //    public BasisLabelTypeProfile Copy()
    //    {
    //        try
    //        {
    //            return new BasisLabelTypeProfile(_key);
    //        }
    //        catch (Exception exc)
    //        {
    //            string message = exc.ToString();
    //            throw;
    //        }
    //    }
    //}

}

