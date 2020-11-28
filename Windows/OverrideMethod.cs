using System;
using System.Drawing;
using System.Collections;
using System.Collections.Specialized;
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
	/// Summary description for OverrideMethod.
	/// </summary>
	public class frmOverrideMethod : WorkflowMethodFormBase
	{

		private TabPage _currentTabPage = null;
		private bool _colorIsPopulated = false;
		private bool _colorChangesMade = false;
		private bool _colorMinMaxesChangesMade = false;
		private bool _storeGradesIsPopulated = false;
		private bool _storeGradesChangesMade = false;
		private StoreGradeList _storeGradeList;
		private bool _minMaxesChangesMade = false;
		private bool _capacityChangesMade = false;
		private bool _capacityIsPopulated = false;
        private bool _packRoundingChangesMade = false;  // TT#616 - AGallagher - Allocation - Pack Rounding (#67)
        private bool _packRoundingIsPopulated = false;  // TT#616 - AGallagher - Allocation - Pack Rounding (#67)
        private bool _imoIsPopulated = false;   // TT#1401 - gtaylor - Reservation Stores
        private bool _imoChangesMade = false;   // TT#1401 - gtaylor - Reservation Stores
        private bool _itemMaxEntryError = false;    // TT#2083 - gtaylor - item max entry issue
        // BEGIN TT#1401 - gtaylor - Reservation Stores
        private ProfileList _imoGroupList;
        // Begin TT#2731 - JSmith - Unable to copy allocation override method from global
        //private ProfileList _imoGroupLevelList;
        // End TT#2731 - JSmith - Unable to copy allocation override method from global
        private bool _reservationGridBuilt = false;
        private DataSet _imoDataSet;
        // Begin TT#2731 - JSmith - Unable to copy allocation override method from global
        //private IMOProfileList _imoProfileList;
        //private IMOMethodOverrideProfileList _imoMethodOverrideProfileList;
        //private bool _applyVSW = true;
        // End TT#2731 - JSmith - Unable to copy allocation override method from global
        // END TT#1401 - gtaylor - Reservation Stores
        private DataSet _dsOverRide;
		private System.Data.DataTable _allColorDataTable;
		private System.Data.DataTable _capacityDataTable;
        private System.Data.DataTable _storegradesDataTable;  // TT#618 - AGallagher - Allocation Override - Add Attribute Sets (#35)
		private System.Data.DataTable _merchDataTable2;
		private System.Data.DataTable _colorList;
       	static private ColorData _cd = null;
		private UltraGridColumn _gridCol;
		private UltraGridBand _gridBand;
		private bool _setRowPosition = false;
		private AllocationOverrideMethod _allocationOverrideMethod;
		private bool _textChanged = false;
		private bool _priorError = false;
		private int _lastMerchIndex = -1;
		private System.Windows.Forms.TabControl tabRuleMethod;
		private System.Windows.Forms.TabPage tabMethod;
		private System.Windows.Forms.Label lblFilter;
		private System.Windows.Forms.GroupBox gbxBasis;
		private System.Windows.Forms.TabPage tabProperties;
		private System.Windows.Forms.Label lblStoreGradeTime;
		private System.Windows.Forms.TextBox txtStoreGradeTime;
		private System.Windows.Forms.CheckBox cbxExceedMax;
		private System.Windows.Forms.Label lblReserve;
		private System.Windows.Forms.TextBox txtReserve;
		private System.Windows.Forms.TextBox txtNeedLimit;
		private System.Windows.Forms.Label lblNeedLimit;
		private System.Windows.Forms.Label lblOtsPlan;
		private System.Windows.Forms.Label lblOnHand;
		private System.Windows.Forms.Label lblOHFactor;
		private System.Windows.Forms.TextBox txtOHFactor;
		private System.Windows.Forms.GroupBox gbxSettings;
		private System.Windows.Forms.GroupBox gbxConstraints;
		private System.Windows.Forms.TabControl tabConstraints;
		private System.Windows.Forms.TabPage tabColor;
		private System.Windows.Forms.TabPage tabStoreGrade;
		private System.Windows.Forms.TabPage tabCapacity;
		private Infragistics.Win.UltraWinGrid.UltraGrid ugStoreGrades;
		private System.Windows.Forms.Label lblColorMult;
		private System.Windows.Forms.TextBox txtColorMult;
		private System.Windows.Forms.Label lblSizeMult;
		private System.Windows.Forms.TextBox txtSizeMult;
		private Infragistics.Win.UltraWinGrid.UltraGrid ugColor;
		private System.Windows.Forms.Label lblAttribute;
		private Infragistics.Win.UltraWinGrid.UltraGrid ugCapacity;
		private System.Windows.Forms.RadioButton rbReserveUnits;
		private System.Windows.Forms.RadioButton rbReservePct;
		private MIDRetail.Windows.Controls.MIDComboBoxEnh cboOverrideFilter;
        // Begin Track #4872 - JSmith - Global/User Attributes
        //private MIDRetail.Windows.Controls.MIDComboBoxEnh cboStoreAttribute;
        private MIDAttributeComboBox cboStoreAttribute;
        // End Track #4872
        private System.Windows.Forms.ToolTip toolTip1;
		private System.Windows.Forms.Label lblWeeks;
		private System.Windows.Forms.CheckBox cbxExceedCapacity;
		private Infragistics.Win.UltraWinGrid.UltraGrid ugWorkflows;
		private System.Windows.Forms.ContextMenu mnuGrids;
       	private System.ComponentModel.IContainer components;
		private Infragistics.Win.UltraWinGrid.UltraDropDown uddColors;
		private Infragistics.Win.UltraWinGrid.UltraGrid ugAllColors;
		private Infragistics.Win.UltraWinGrid.UltraGrid RightClickedFrom;
		private System.Windows.Forms.ContextMenu mnuGridColHeader;
		private System.Windows.Forms.ContextMenu mnuStoreGrades;
        private System.Windows.Forms.ContextMenu mnuPackRounding;   // TT#616 - AGallagher - Allocation - Pack Rounding (#67)
        private System.Windows.Forms.ContextMenu mnuIMOGrid;    // TT#1401 - Reservation Stores - gtaylor
		private bool _dupColorFound = false;
		private bool _dupGradeFound = false;
		private bool _colorMinError = false;
		private string _errors = null;
		private string _storeGradeDefaultText;
		private string _needLimitDefaultText;
		private string _allColorsMinDefaultText;
		private string _allColorsMaxDefaultText;
		private int _allColorsMinValue = 0;
		private int _allColorsMaxValue;
		private int _styleKey;
        private int _curAttributeSet;   // Begin TT#939 - RMatelic - Allocation Override -> will not clear fields in grid on store grade tab, change attribute set and get System Object
        private bool _setReset;         // End TT#939 
        // Begin TT#939 - JSmith - Allocation Override -> will not clear fields in grid on store grade tab, change attribute set and get System Object
        private bool _SGStoreAttributeChanged = false;
        // End TT#939
        private string _styleText;
		private TabPage tabPackRounding;
        private UltraGrid ugPackRounding;
        private Label lblSGAttribute;
        private MIDAttributeComboBox cboSGStoreAttribute;
        private Label lblSGSet;
		private GroupBox gbWarehouse;
		private Label lblReserveAsPacks;
		private Label lblReserveAsBulk;
		private TextBox txtReserveAsPacks;
		private TextBox txtReserveAsBulk;
        // private bool _askQuestion = true; // TT#3146 - RMatelic - Allocation override mssg when updating the Basis information 80427. Would not expect to get the message>> remove question 
        private bool _resetStoreGroup = false;   // TT#618 - STodd - Allocation Override - Add Attribute Sets (#35)
		private bool _StoreGradeValueChange = false;   // TT#618 - STodd - Allocation Override - Add Attribute Sets (#35)
		private ArrayList _deletedGrades;    // TT#618 - STodd - Allocation Override - Add Attribute Sets (#35)
		private Hashtable _addedGrades;    // TT#618 - STodd - Allocation Override - Add Attribute Sets (#35)
		private Label lblPackRoundingInfo;
        private GroupBox gbxMinMaxOpt;
        private RadioButton radInventoryMinMax;
        private RadioButton radAllocationMinMax;
        private Label lblInventoryBasis;    // TT#618 - STodd - Allocation Override - Add Attribute Sets (#35)		
		private bool _newGradeAdded = false;   // TT#618 - STodd - Allocation Override - Add Attribute Sets (#35)
        // BEGIN TT#1287 - AGallagher - Inventory Min/Max
        DataTable MerchandiseDataTable3;
        private bool _textChangedIB = false;
        private bool _priorErrorIB = false;
        private int _lastMerchIndexIB = -1;
        private TabPage tabReservation;
        private UltraGrid ugReservation;
        private MIDAttributeComboBox midAttributeCbx;
        private Label lblRsrvStrAtt;
        private CheckBox cbxRsrvDoNotApplyVSW;
        private MIDComboBoxEnh cboOTSPlan;
        private MIDComboBoxEnh cboOnHand;
        private MIDComboBoxEnh cboInventoryBasis;
        private MIDComboBoxEnh cmbSGAttributeSet;
        private char _InventoryInd;
        // END TT#1287 - AGallagher - Inventory Min/Max
        private bool _fromMenuInsert;        // TT#3154 - RMatelic - Allocation override color min max - right click and insert before or after receive a null reference exception
		/// <summary>
		/// Gets or sets a boolean identifying if changes have been made to the color data.
		/// </summary>
		public bool ColorChangesMade
		{
			get	{return _colorChangesMade;}
			set	
			{
				_colorChangesMade = value;
				// if true, turn on master flag
				if (_colorChangesMade)
				{
					//if (FormLoaded && !_loadingWindow)
					if (FormLoaded )
					{
						ChangePending = true;
					}
				}
			}
		}


        // BEGIN TT#2083 - gtaylor
        /// <summary>
        /// Gets a boolean identifying if an error was encountered during Item Max Value entry
        /// </summary>
        public bool ItemMaxEntryError
        {
            get { return _itemMaxEntryError; }
            set { _itemMaxEntryError = value; }
        }
        // END TT#2083 - gtaylor

        // BEGIN TT#1401 - gtaylor - Reservation Stores
        /// <summary>
        /// Gets or sets a boolean identifying if changes have been made to the imo data.
        /// </summary>
        public bool IMOChangesMade
        {
            get { return _imoChangesMade; }
            set
            {
                _imoChangesMade = value;
                // if true, turn on master flag
                if (_imoChangesMade)
                {
                    //if (FormLoaded && !_loadingWindow)
                    if (FormLoaded)
                    {
                        ChangePending = true;
                    }
                }
            }
        }
        // END TT#1401 - gtaylor - Reservation Stores        
        
        /// <summary>
		/// Gets or sets a boolean identifying if changes have been made to the color tab min max data.
		/// </summary>
		public bool ColorMinMaxesChangesMade
		{
			get	{return _colorMinMaxesChangesMade;}
			set	
			{
				_colorMinMaxesChangesMade = value;
				// if true, turn on master flag
				if (_colorMinMaxesChangesMade)
				{
					//if (FormLoaded && !_loadingWindow)
					if (FormLoaded)
					{
						ChangePending = true;
					}
				}
			}
		}
		/// <summary>
		/// Gets or sets a boolean identifying if changes have been made to the store grade data.
		/// </summary>
		public bool StoreGradesChangesMade
		{
			get	{return _storeGradesChangesMade;}
			set	
			{
				_storeGradesChangesMade = value;
				// if true, turn on master flag
				if (_storeGradesChangesMade)
				{
					//if (FormLoaded && !_loadingWindow)
					if (FormLoaded )
					{
						ChangePending = true;
					}
				}
			}
		}

		/// <summary>
		/// Gets or sets a boolean identifying if changes have been made to the min max data.
		/// </summary>
		public bool MinMaxesChangesMade
		{
			get	{return _minMaxesChangesMade;}
			set	
			{
				_minMaxesChangesMade = value;
				// if true, turn on master flag
				if (_minMaxesChangesMade)
				{
					//if (FormLoaded && !_loadingWindow)
					if (FormLoaded)
					{
						ChangePending = true;
					}
				}
			}
		}
		public bool CapacityChangesMade
		{
			get	{return _capacityChangesMade;}
			set	
			{
				_capacityChangesMade = value;
				// if true, turn on master flag
				if (_capacityChangesMade)
				{
					//if (FormLoaded && !_loadingWindow)
					if (FormLoaded )
					{
						ChangePending = true;
					}
				}
			}
		}
        // BEGIN TT#616 - AGallagher - Allocation - Pack Rounding (#67)
        public bool PackRoundingChangesMade
        {
            get { return _packRoundingChangesMade; }
            set
            {
                _packRoundingChangesMade = value;
                
                if (_packRoundingChangesMade)
                {
                    if (FormLoaded)
                    {
                        ChangePending = true;
                    }
                }
            }
        }

		// Begin TT#4332 - stodd - Matrix randomly shows all zeros for a grade tier
        public AllocationOverrideMethod AllocationOVerrideMethod
        {
            get
            {
                return _allocationOverrideMethod;
            }
        }
		// End TT#4332 - stodd - Matrix randomly shows all zeros for a grade tier
		
        // END TT#616 - AGallagher - Allocation - Pack Rounding (#67)
		public frmOverrideMethod(SessionAddressBlock SAB, ExplorerAddressBlock aEAB) : base (SAB, aEAB, eMIDTextCode.frm_OverrideMethod, eWorkflowMethodType.Method)
		{
			try
			{
				//
				// Required for Windows Form Designer support
				//
				InitializeComponent();
				_currentTabPage = this.tabColor;

				UserSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AllocationMethodsUserAllocationOverride);
				GlobalSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AllocationMethodsGlobalAllocationOverride);

                _imoGroupList = (ProfileList)StoreMgmt.StoreGroup_GetListViewList(eStoreGroupSelectType.All, false); //(ProfileList)SAB.StoreServerSession.GetStoreGroupListViewList(eStoreGroupSelectType.All, false); //TT#1401 - Reservation Stores - gtaylor
			}
			catch(Exception ex)
			{
				string exceptionMessage = ex.Message;
				this.FormLoadError = true;
			}
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }

                this.tabConstraints.Click -= new System.EventHandler(this.tabConstraints_Click);
                this.tabConstraints.SelectedIndexChanged -= new System.EventHandler(this.tabConstraints_SelectedIndexChanged);
                this.ugAllColors.CellChange -= new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ugAllColors_CellChange);
                this.ugAllColors.MouseEnterElement -= new Infragistics.Win.UIElementEventHandler(this.ugAllColors_MouseEnterElement);
                this.ugAllColors.InitializeLayout -= new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.ugAllColors_InitializeLayout);
                //Begin TT#169 - JSmith - enter max of 1000 and receive 'must be integer'
                MIDRetail.Windows.Controls.UltraGridLayoutDefaults ugld = new MIDRetail.Windows.Controls.UltraGridLayoutDefaults(ErrorImage);
                ugld.DetachGridEventHandlers(ugAllColors);
                //End TT#169
                this.uddColors.InitializeLayout -= new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.uddColors_InitializeLayout);
                this.txtSizeMult.Validating -= new System.ComponentModel.CancelEventHandler(this.txtPositiveInteger_Validating);
                this.txtColorMult.Validating -= new System.ComponentModel.CancelEventHandler(this.txtPositiveInteger_Validating);
                this.ugColor.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.GridMouseDown);
                this.ugColor.CellChange -= new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ugColor_CellChange);
                this.ugColor.MouseEnterElement -= new Infragistics.Win.UIElementEventHandler(this.ugColor_MouseEnterElement);
                this.ugColor.BeforeRowUpdate -= new Infragistics.Win.UltraWinGrid.CancelableRowEventHandler(this.ugColor_BeforeRowUpdate);
                this.ugColor.InitializeLayout -= new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.ugColor_InitializeLayout);
                //Begin TT#169 - JSmith - enter max of 1000 and receive 'must be integer'
                ugld.DetachGridEventHandlers(ugColor);
                //End TT#169
                this.ugStoreGrades.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.ugStoreGrades_MouseDown);
                this.ugStoreGrades.AfterRowUpdate -= new Infragistics.Win.UltraWinGrid.RowEventHandler(this.ugStoreGrades_AfterRowUpdate);
                this.ugStoreGrades.CellChange -= new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ugStoreGrades_CellChange);
                this.ugStoreGrades.BeforeExitEditMode -= new Infragistics.Win.UltraWinGrid.BeforeExitEditModeEventHandler(this.ugStoreGrades_BeforeExitEditMode);
                this.ugStoreGrades.AfterRowsDeleted -= new System.EventHandler(this.ugStoreGrades_AfterRowsDeleted);
                this.ugStoreGrades.MouseEnterElement -= new Infragistics.Win.UIElementEventHandler(this.ugStoreGrades_MouseEnterElement);
                this.ugStoreGrades.DragDrop -= new System.Windows.Forms.DragEventHandler(this.ugStoreGrades_DragDrop);
                this.ugStoreGrades.AfterRowInsert -= new Infragistics.Win.UltraWinGrid.RowEventHandler(this.ugStoreGrades_AfterRowInsert);
                this.ugStoreGrades.DragEnter -= new System.Windows.Forms.DragEventHandler(this.ugStoreGrades_DragEnter);
                this.ugStoreGrades.BeforeRowUpdate -= new Infragistics.Win.UltraWinGrid.CancelableRowEventHandler(this.ugStoreGrades_BeforeRowUpdate);
                this.ugStoreGrades.InitializeLayout -= new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.ugStoreGrades_InitializeLayout);
                //Begin TT#169 - JSmith - enter max of 1000 and receive 'must be integer'
                ugld.DetachGridEventHandlers(ugStoreGrades);
                //End TT#169
                this.ugStoreGrades.AfterSortChange -= new Infragistics.Win.UltraWinGrid.BandEventHandler(this.ugStoreGrades_AfterSortChange);
                this.cboStoreAttribute.SelectionChangeCommitted -= new System.EventHandler(this.cboStoreAttribute_SelectionChangeCommitted);
                this.ugCapacity.CellChange -= new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ugCapacity_CellChange);
                this.ugCapacity.MouseEnterElement -= new Infragistics.Win.UIElementEventHandler(this.ugCapacity_MouseEnterElement);
                this.cbxExceedCapacity.CheckStateChanged -= new System.EventHandler(this.cbxExceedCapacity_CheckStateChanged);
                this.txtNeedLimit.Validating -= new System.ComponentModel.CancelEventHandler(this.txtNegToOneHundredPercent_Validating);
                this.rbReserveUnits.CheckedChanged -= new System.EventHandler(this.rbReserveUnits_CheckedChanged);
                this.rbReservePct.CheckedChanged -= new System.EventHandler(this.rbReservePct_CheckedChanged);
                this.txtReserve.Validating -= new System.ComponentModel.CancelEventHandler(this.txtReserve_Validating);
                this.txtReserve.TextChanged -= new System.EventHandler(this.txtReserve_TextChanged);
                this.txtStoreGradeTime.TextChanged -= new System.EventHandler(this.txtStoreGradeTime_TextChanged);
                this.cboOnHand.KeyDown -= new System.Windows.Forms.KeyEventHandler(this.cboOnHand_KeyDown);
                this.cboOnHand.Validating -= new System.ComponentModel.CancelEventHandler(this.cboOnHand_Validating);
                this.cboOnHand.Validated -= new System.EventHandler(this.cboOnHand_Validated);
                this.cboOnHand.DragDrop -= new System.Windows.Forms.DragEventHandler(this.cboOnHand_DragDrop);
                this.cboOnHand.DragEnter -= new System.Windows.Forms.DragEventHandler(this.cboOnHand_DragEnter);
                this.cboOnHand.DragOver -= new System.Windows.Forms.DragEventHandler(this.cboOnHand_DragOver);
                this.cboOTSPlan.KeyDown -= new System.Windows.Forms.KeyEventHandler(this.cboOTSPlan_KeyDown);
                this.cboOTSPlan.Validating -= new System.ComponentModel.CancelEventHandler(this.cboOTSPlan_Validating);
                this.cboOTSPlan.Validated -= new System.EventHandler(this.cboOTSPlan_Validated);
                this.cboOTSPlan.DragDrop -= new System.Windows.Forms.DragEventHandler(this.cboOTSPlan_DragDrop);
                this.cboOTSPlan.DragEnter -= new System.Windows.Forms.DragEventHandler(this.cboOTSPlan_DragEnter);
                this.cboOTSPlan.DragOver -= new System.Windows.Forms.DragEventHandler(this.cboOTSPlan_DragOver);
                this.txtOHFactor.Validating -= new System.ComponentModel.CancelEventHandler(this.txtOHFactor_Validating);
                this.txtOHFactor.TextChanged -= new System.EventHandler(this.txtOHFactor_TextChanged);
                // Begin MID Track 4858 - JSmith - Security changes
                //				this.btnClose.Click -= new System.EventHandler(this.btnClose_Click);
                //				this.btnSave.Click -= new System.EventHandler(this.btnSave_Click);
                //				this.btnProcess.Click -= new System.EventHandler(this.btnProcess_Click);
                // End MID Track 4858

                // Begin TT#709 - RMatelic - Override Method - need option to not set Forecast Level : unrelated to issue
                this.cboOTSPlan.SelectionChangeCommitted -= new System.EventHandler(this.cboOTSPlan_SelectionChangeCommitted);
                this.cboOnHand.SelectionChangeCommitted -= new System.EventHandler(this.cboOnHand_SelectionChangeCommitted);
                // End TT#709 
                this.mnuIMOGrid.Popup -= new System.EventHandler(this.mnuIMOGrid_Popup);    // TT#1401 - Reservation Stores - gtaylor
                // BEGIN TT#616 - AGallagher - Allocation - Pack Rounding (#67)
                this.ugPackRounding.AfterRowInsert -= new Infragistics.Win.UltraWinGrid.RowEventHandler(this.ugPackRounding_AfterRowInsert);
                this.ugPackRounding.AfterCellUpdate -= new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ugPackRounding_AfterCellUpdate);
                this.ugPackRounding.BeforeRowsDeleted -= new Infragistics.Win.UltraWinGrid.BeforeRowsDeletedEventHandler(this.ugPackRounding_BeforeRowsDeleted);
                // END TT#616 - AGallagher - Allocation - Pack Rounding (#67)
                // BEGIN TT#618 - AGallagher - Allocation Override - Add Attribute Sets (#35)
                this.cboSGStoreAttribute.SelectionChangeCommitted -= new System.EventHandler(this.cboSGStoreAttribute_SelectionChangeCommitted);
                this.cboSGStoreAttribute.DragDrop -= new System.Windows.Forms.DragEventHandler(this.cboSGStoreAttribute_DragOver);
                this.cboSGStoreAttribute.DragEnter -= new System.Windows.Forms.DragEventHandler(this.cboSGStoreAttribute_DragEnter);
                // Begin TT#1505 - RMatelic - Users Can Not View Global Variables >> change combo box prefix to 'cmb' to keep enabled when read only
                this.cmbSGAttributeSet.SelectionChangeCommitted -= new System.EventHandler(this.cmbSGAttributeSet_SelectionChangeCommitted);
                // End TT#1505
                // END TT#618 - AGallagher - Allocation Override - Add Attribute Sets (#35)
                // Begin TT#709 - RMatelic - Override Method - need option to not set Forecast Level : unrelated to issue
                this.cboOTSPlan.SelectionChangeCommitted -= new System.EventHandler(this.cboOTSPlan_SelectionChangeCommitted);
                this.cboOnHand.SelectionChangeCommitted -= new System.EventHandler(this.cboOnHand_SelectionChangeCommitted);
                // BEGIN TT#1287 - AGallagher - Inventory Min/Max
                this.radAllocationMinMax.CheckedChanged -= new System.EventHandler(this.radAllocationMinMax_CheckedChanged);
                this.radInventoryMinMax.CheckedChanged -= new System.EventHandler(this.radInventoryMinMax_CheckedChanged);
                this.cboInventoryBasis.KeyDown -= new System.Windows.Forms.KeyEventHandler(this.cboInventoryBasis_KeyDown);
                this.cboInventoryBasis.Validating -= new System.ComponentModel.CancelEventHandler(this.cboInventoryBasis_Validating);
                this.cboInventoryBasis.Validated -= new System.EventHandler(this.cboInventoryBasis_Validated);
                this.cboInventoryBasis.DragDrop -= new System.Windows.Forms.DragEventHandler(this.cboInventoryBasis_DragDrop);
                this.cboInventoryBasis.DragEnter -= new System.Windows.Forms.DragEventHandler(this.cboInventoryBasis_DragEnter);
                this.cboInventoryBasis.DragOver -= new System.Windows.Forms.DragEventHandler(this.cboInventoryBasis_DragOver);
                // END TT#1287 - AGallagher - Inventory Min/Max

                //Begin TT#1401 - GRT - Reservation Stores
                this.ugReservation.InitializeLayout -= new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.ugReservation_InitializeLayout);
                //End TT#1401 - GRT - Reservation Stores
                // End TT#709 
                this.midAttributeCbx.SelectionChangeCommitted -= new System.EventHandler(this.midAttributeCbx_SelectionChangeCommitted);


                this.cboOverrideFilter.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler(cboOverrideFilter_MIDComboBoxPropertiesChangedEvent);
                this.cmbSGAttributeSet.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler(cmbSGAttributeSet_MIDComboBoxPropertiesChangedEvent);
                this.cboInventoryBasis.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler(cboInventoryBasis_MIDComboBoxPropertiesChangedEvent);
                this.cboSGStoreAttribute.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler(cboSGStoreAttribute_MIDComboBoxPropertiesChangedEvent);
                this.cboStoreAttribute.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler(cboStoreAttribute_MIDComboBoxPropertiesChangedEvent);
                this.midAttributeCbx.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler(midAttributeCbx_MIDComboBoxPropertiesChangedEvent);
                this.cboOnHand.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler(cboOnHand_MIDComboBoxPropertiesChangedEvent);
                this.cboOTSPlan.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler(cboOTSPlan_MIDComboBoxPropertiesChangedEvent);



                this.Load -= new System.EventHandler(this.frmOverrideMethod_Load);
                this.cboOverrideFilter.SelectionChangeCommitted -= new System.EventHandler(this.cboOverrideFilter_SelectionChangeCommitted);
                this.cboOverrideFilter.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler(cboOverrideFilter_MIDComboBoxPropertiesChangedEvent);

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
            this.components = new System.ComponentModel.Container();
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand1 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
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
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand2 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.Appearance appearance14 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance15 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance16 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance17 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance18 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance19 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand3 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.Appearance appearance20 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance21 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance22 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance23 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance24 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance25 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand4 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.Appearance appearance26 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance27 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance28 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance29 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance30 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance31 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand5 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.Appearance appearance32 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance33 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance34 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance35 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance36 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance37 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance38 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance39 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance40 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance41 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance42 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance43 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance44 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance45 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance46 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance47 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance48 = new Infragistics.Win.Appearance();
            this.tabRuleMethod = new System.Windows.Forms.TabControl();
            this.tabMethod = new System.Windows.Forms.TabPage();
            this.gbWarehouse = new System.Windows.Forms.GroupBox();
            this.lblReserveAsPacks = new System.Windows.Forms.Label();
            this.lblReserveAsBulk = new System.Windows.Forms.Label();
            this.txtReserveAsPacks = new System.Windows.Forms.TextBox();
            this.cboOverrideFilter = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this.rbReserveUnits = new System.Windows.Forms.RadioButton();
            this.txtReserveAsBulk = new System.Windows.Forms.TextBox();
            this.rbReservePct = new System.Windows.Forms.RadioButton();
            this.lblReserve = new System.Windows.Forms.Label();
            this.txtReserve = new System.Windows.Forms.TextBox();
            this.gbxConstraints = new System.Windows.Forms.GroupBox();
            this.tabConstraints = new System.Windows.Forms.TabControl();
            this.tabColor = new System.Windows.Forms.TabPage();
            this.ugAllColors = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.uddColors = new Infragistics.Win.UltraWinGrid.UltraDropDown();
            this.txtSizeMult = new System.Windows.Forms.TextBox();
            this.lblSizeMult = new System.Windows.Forms.Label();
            this.txtColorMult = new System.Windows.Forms.TextBox();
            this.lblColorMult = new System.Windows.Forms.Label();
            this.ugColor = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.mnuGrids = new System.Windows.Forms.ContextMenu();
            this.tabStoreGrade = new System.Windows.Forms.TabPage();
            this.cmbSGAttributeSet = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this.gbxMinMaxOpt = new System.Windows.Forms.GroupBox();
            this.cboInventoryBasis = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this.lblInventoryBasis = new System.Windows.Forms.Label();
            this.radInventoryMinMax = new System.Windows.Forms.RadioButton();
            this.radAllocationMinMax = new System.Windows.Forms.RadioButton();
            this.lblSGSet = new System.Windows.Forms.Label();
            this.cboSGStoreAttribute = new MIDRetail.Windows.Controls.MIDAttributeComboBox();
            this.lblSGAttribute = new System.Windows.Forms.Label();
            this.ugStoreGrades = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.mnuStoreGrades = new System.Windows.Forms.ContextMenu();
            this.tabCapacity = new System.Windows.Forms.TabPage();
            this.cboStoreAttribute = new MIDRetail.Windows.Controls.MIDAttributeComboBox();
            this.ugCapacity = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.cbxExceedCapacity = new System.Windows.Forms.CheckBox();
            this.lblAttribute = new System.Windows.Forms.Label();
            this.tabPackRounding = new System.Windows.Forms.TabPage();
            this.lblPackRoundingInfo = new System.Windows.Forms.Label();
            this.ugPackRounding = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.mnuPackRounding = new System.Windows.Forms.ContextMenu();
            this.tabReservation = new System.Windows.Forms.TabPage();
            this.cbxRsrvDoNotApplyVSW = new System.Windows.Forms.CheckBox();
            this.ugReservation = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.midAttributeCbx = new MIDRetail.Windows.Controls.MIDAttributeComboBox();
            this.lblRsrvStrAtt = new System.Windows.Forms.Label();
            this.lblFilter = new System.Windows.Forms.Label();
            this.gbxSettings = new System.Windows.Forms.GroupBox();
            this.lblWeeks = new System.Windows.Forms.Label();
            this.lblNeedLimit = new System.Windows.Forms.Label();
            this.txtNeedLimit = new System.Windows.Forms.TextBox();
            this.cbxExceedMax = new System.Windows.Forms.CheckBox();
            this.txtStoreGradeTime = new System.Windows.Forms.TextBox();
            this.lblStoreGradeTime = new System.Windows.Forms.Label();
            this.gbxBasis = new System.Windows.Forms.GroupBox();
            this.cboOnHand = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this.cboOTSPlan = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this.txtOHFactor = new System.Windows.Forms.TextBox();
            this.lblOHFactor = new System.Windows.Forms.Label();
            this.lblOnHand = new System.Windows.Forms.Label();
            this.lblOtsPlan = new System.Windows.Forms.Label();
            this.tabProperties = new System.Windows.Forms.TabPage();
            this.ugWorkflows = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.mnuIMOGrid = new System.Windows.Forms.ContextMenu();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.mnuGridColHeader = new System.Windows.Forms.ContextMenu();
            this.pnlGlobalUser.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.utmMain)).BeginInit();
            this.tabRuleMethod.SuspendLayout();
            this.tabMethod.SuspendLayout();
            this.gbWarehouse.SuspendLayout();
            this.gbxConstraints.SuspendLayout();
            this.tabConstraints.SuspendLayout();
            this.tabColor.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ugAllColors)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uddColors)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ugColor)).BeginInit();
            this.tabStoreGrade.SuspendLayout();
            this.gbxMinMaxOpt.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ugStoreGrades)).BeginInit();
            this.tabCapacity.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ugCapacity)).BeginInit();
            this.tabPackRounding.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ugPackRounding)).BeginInit();
            this.tabReservation.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ugReservation)).BeginInit();
            this.gbxSettings.SuspendLayout();
            this.gbxBasis.SuspendLayout();
            this.tabProperties.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ugWorkflows)).BeginInit();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(632, 705);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(544, 705);
            // 
            // btnProcess
            // 
            this.btnProcess.Location = new System.Drawing.Point(16, 705);
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
            this.tabRuleMethod.Location = new System.Drawing.Point(8, 48);
            this.tabRuleMethod.Name = "tabRuleMethod";
            this.tabRuleMethod.SelectedIndex = 0;
            this.tabRuleMethod.Size = new System.Drawing.Size(702, 649);
            this.tabRuleMethod.TabIndex = 24;
            // 
            // tabMethod
            // 
            this.tabMethod.Controls.Add(this.gbWarehouse);
            this.tabMethod.Controls.Add(this.gbxConstraints);
            this.tabMethod.Controls.Add(this.lblFilter);
            this.tabMethod.Controls.Add(this.gbxSettings);
            this.tabMethod.Controls.Add(this.gbxBasis);
            this.tabMethod.Location = new System.Drawing.Point(4, 22);
            this.tabMethod.Name = "tabMethod";
            this.tabMethod.Size = new System.Drawing.Size(694, 623);
            this.tabMethod.TabIndex = 1;
            this.tabMethod.Text = "Method";
            // 
            // gbWarehouse
            // 
            this.gbWarehouse.Controls.Add(this.lblReserveAsPacks);
            this.gbWarehouse.Controls.Add(this.lblReserveAsBulk);
            this.gbWarehouse.Controls.Add(this.txtReserveAsPacks);
            this.gbWarehouse.Controls.Add(this.cboOverrideFilter);
            this.gbWarehouse.Controls.Add(this.rbReserveUnits);
            this.gbWarehouse.Controls.Add(this.txtReserveAsBulk);
            this.gbWarehouse.Controls.Add(this.rbReservePct);
            this.gbWarehouse.Controls.Add(this.lblReserve);
            this.gbWarehouse.Controls.Add(this.txtReserve);
            this.gbWarehouse.Location = new System.Drawing.Point(328, 7);
            this.gbWarehouse.Name = "gbWarehouse";
            this.gbWarehouse.Size = new System.Drawing.Size(328, 92);
            this.gbWarehouse.TabIndex = 7;
            this.gbWarehouse.TabStop = false;
            this.gbWarehouse.Text = "Warehouse";
            // 
            // lblReserveAsPacks
            // 
            this.lblReserveAsPacks.AutoSize = true;
            this.lblReserveAsPacks.Location = new System.Drawing.Point(6, 66);
            this.lblReserveAsPacks.Name = "lblReserveAsPacks";
            this.lblReserveAsPacks.Size = new System.Drawing.Size(95, 13);
            this.lblReserveAsPacks.TabIndex = 10;
            this.lblReserveAsPacks.Text = "Reserve As Packs";
            // 
            // lblReserveAsBulk
            // 
            this.lblReserveAsBulk.AutoSize = true;
            this.lblReserveAsBulk.Location = new System.Drawing.Point(6, 42);
            this.lblReserveAsBulk.Name = "lblReserveAsBulk";
            this.lblReserveAsBulk.Size = new System.Drawing.Size(86, 13);
            this.lblReserveAsBulk.TabIndex = 9;
            this.lblReserveAsBulk.Text = "Reserve As Bulk";
            // 
            // txtReserveAsPacks
            // 
            this.txtReserveAsPacks.Location = new System.Drawing.Point(113, 63);
            this.txtReserveAsPacks.Name = "txtReserveAsPacks";
            this.txtReserveAsPacks.Size = new System.Drawing.Size(80, 20);
            this.txtReserveAsPacks.TabIndex = 8;
            // 
            // cboOverrideFilter
            // 
            this.cboOverrideFilter.AutoAdjust = true;
            this.cboOverrideFilter.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboOverrideFilter.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboOverrideFilter.DataSource = null;
            this.cboOverrideFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboOverrideFilter.DropDownWidth = 121;
            this.cboOverrideFilter.FormattingEnabled = false;
            this.cboOverrideFilter.IgnoreFocusLost = false;
            this.cboOverrideFilter.ItemHeight = 13;
            this.cboOverrideFilter.Location = new System.Drawing.Point(264, 90);
            this.cboOverrideFilter.Margin = new System.Windows.Forms.Padding(0);
            this.cboOverrideFilter.MaxDropDownItems = 25;
            this.cboOverrideFilter.Name = "cboOverrideFilter";
            this.cboOverrideFilter.SetToolTip = "";
            this.cboOverrideFilter.Size = new System.Drawing.Size(121, 21);
            this.cboOverrideFilter.TabIndex = 1;
            this.cboOverrideFilter.Tag = null;
            this.toolTip1.SetToolTip(this.cboOverrideFilter, "Filters");
            this.cboOverrideFilter.Visible = false;
            this.cboOverrideFilter.MIDComboBoxPropertiesChangedEvent += new MIDRetail.Windows.Controls.MIDComboBoxPropertiesChangedEventHandler(this.cboOverrideFilter_MIDComboBoxPropertiesChangedEvent);
            this.cboOverrideFilter.SelectionChangeCommitted += new System.EventHandler(this.cboOverrideFilter_SelectionChangeCommitted);
            // 
            // rbReserveUnits
            // 
            this.rbReserveUnits.Location = new System.Drawing.Point(264, 19);
            this.rbReserveUnits.Name = "rbReserveUnits";
            this.rbReserveUnits.Size = new System.Drawing.Size(54, 24);
            this.rbReserveUnits.TabIndex = 6;
            this.rbReserveUnits.Text = "Units";
            this.rbReserveUnits.CheckedChanged += new System.EventHandler(this.rbReserveUnits_CheckedChanged);
            // 
            // txtReserveAsBulk
            // 
            this.txtReserveAsBulk.Location = new System.Drawing.Point(113, 41);
            this.txtReserveAsBulk.Name = "txtReserveAsBulk";
            this.txtReserveAsBulk.Size = new System.Drawing.Size(80, 20);
            this.txtReserveAsBulk.TabIndex = 7;
            // 
            // rbReservePct
            // 
            this.rbReservePct.Location = new System.Drawing.Point(199, 19);
            this.rbReservePct.Name = "rbReservePct";
            this.rbReservePct.Size = new System.Drawing.Size(104, 24);
            this.rbReservePct.TabIndex = 5;
            this.rbReservePct.Text = "Percent";
            this.rbReservePct.CheckedChanged += new System.EventHandler(this.rbReservePct_CheckedChanged);
            // 
            // lblReserve
            // 
            this.lblReserve.Location = new System.Drawing.Point(6, 19);
            this.lblReserve.Name = "lblReserve";
            this.lblReserve.Size = new System.Drawing.Size(102, 20);
            this.lblReserve.TabIndex = 3;
            this.lblReserve.Text = "Reserve";
            // 
            // txtReserve
            // 
            this.txtReserve.Location = new System.Drawing.Point(113, 19);
            this.txtReserve.MaxLength = 12;
            this.txtReserve.Name = "txtReserve";
            this.txtReserve.Size = new System.Drawing.Size(80, 20);
            this.txtReserve.TabIndex = 4;
            this.txtReserve.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtReserve.TextChanged += new System.EventHandler(this.txtReserve_TextChanged);
            this.txtReserve.Validating += new System.ComponentModel.CancelEventHandler(this.txtReserve_Validating);
            // 
            // gbxConstraints
            // 
            this.gbxConstraints.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbxConstraints.Controls.Add(this.tabConstraints);
            this.gbxConstraints.Location = new System.Drawing.Point(8, 202);
            this.gbxConstraints.Name = "gbxConstraints";
            this.gbxConstraints.Size = new System.Drawing.Size(648, 415);
            this.gbxConstraints.TabIndex = 4;
            this.gbxConstraints.TabStop = false;
            this.gbxConstraints.Text = "Constraints";
            // 
            // tabConstraints
            // 
            this.tabConstraints.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabConstraints.Controls.Add(this.tabColor);
            this.tabConstraints.Controls.Add(this.tabStoreGrade);
            this.tabConstraints.Controls.Add(this.tabCapacity);
            this.tabConstraints.Controls.Add(this.tabPackRounding);
            this.tabConstraints.Controls.Add(this.tabReservation);
            this.tabConstraints.Location = new System.Drawing.Point(16, 16);
            this.tabConstraints.Name = "tabConstraints";
            this.tabConstraints.SelectedIndex = 0;
            this.tabConstraints.Size = new System.Drawing.Size(624, 393);
            this.tabConstraints.TabIndex = 0;
            this.tabConstraints.SelectedIndexChanged += new System.EventHandler(this.tabConstraints_SelectedIndexChanged);
            this.tabConstraints.Click += new System.EventHandler(this.tabConstraints_Click);
            // 
            // tabColor
            // 
            this.tabColor.Controls.Add(this.ugAllColors);
            this.tabColor.Controls.Add(this.uddColors);
            this.tabColor.Controls.Add(this.txtSizeMult);
            this.tabColor.Controls.Add(this.lblSizeMult);
            this.tabColor.Controls.Add(this.txtColorMult);
            this.tabColor.Controls.Add(this.lblColorMult);
            this.tabColor.Controls.Add(this.ugColor);
            this.tabColor.Location = new System.Drawing.Point(4, 22);
            this.tabColor.Name = "tabColor";
            this.tabColor.Size = new System.Drawing.Size(616, 367);
            this.tabColor.TabIndex = 0;
            this.tabColor.Text = "Color";
            this.tabColor.UseVisualStyleBackColor = true;
            // 
            // ugAllColors
            // 
            this.ugAllColors.AllowDrop = true;
            this.ugAllColors.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            appearance1.BackColor = System.Drawing.Color.White;
            appearance1.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance1.BackGradientStyle = Infragistics.Win.GradientStyle.ForwardDiagonal;
            this.ugAllColors.DisplayLayout.Appearance = appearance1;
            ultraGridBand1.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.None;
            this.ugAllColors.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
            this.ugAllColors.DisplayLayout.InterBandSpacing = 10;
            appearance2.BackColor = System.Drawing.Color.Transparent;
            this.ugAllColors.DisplayLayout.Override.CardAreaAppearance = appearance2;
            appearance3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance3.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance3.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance3.ForeColor = System.Drawing.Color.Black;
            appearance3.TextHAlignAsString = "Left";
            appearance3.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            this.ugAllColors.DisplayLayout.Override.HeaderAppearance = appearance3;
            appearance4.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.ugAllColors.DisplayLayout.Override.RowAppearance = appearance4;
            appearance5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance5.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance5.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            this.ugAllColors.DisplayLayout.Override.RowSelectorAppearance = appearance5;
            this.ugAllColors.DisplayLayout.Override.RowSelectorWidth = 12;
            this.ugAllColors.DisplayLayout.Override.RowSpacingBefore = 2;
            appearance6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance6.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance6.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance6.ForeColor = System.Drawing.Color.Black;
            this.ugAllColors.DisplayLayout.Override.SelectedRowAppearance = appearance6;
            this.ugAllColors.DisplayLayout.RowConnectorColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.ugAllColors.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.Solid;
            this.ugAllColors.Location = new System.Drawing.Point(208, 8);
            this.ugAllColors.Name = "ugAllColors";
            this.ugAllColors.Size = new System.Drawing.Size(326, 42);
            this.ugAllColors.TabIndex = 3;
            this.ugAllColors.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.ugAllColors_InitializeLayout);
            this.ugAllColors.CellChange += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ugAllColors_CellChange);
            this.ugAllColors.MouseEnterElement += new Infragistics.Win.UIElementEventHandler(this.ugAllColors_MouseEnterElement);
            // 
            // uddColors
            // 
            appearance7.BackColor = System.Drawing.Color.White;
            appearance7.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance7.BackGradientStyle = Infragistics.Win.GradientStyle.ForwardDiagonal;
            this.uddColors.DisplayLayout.Appearance = appearance7;
            this.uddColors.DisplayLayout.InterBandSpacing = 10;
            appearance8.BackColor = System.Drawing.Color.Transparent;
            this.uddColors.DisplayLayout.Override.CardAreaAppearance = appearance8;
            appearance9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance9.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance9.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance9.ForeColor = System.Drawing.Color.Black;
            appearance9.TextHAlignAsString = "Left";
            appearance9.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            this.uddColors.DisplayLayout.Override.HeaderAppearance = appearance9;
            appearance10.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.uddColors.DisplayLayout.Override.RowAppearance = appearance10;
            appearance11.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance11.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance11.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            this.uddColors.DisplayLayout.Override.RowSelectorAppearance = appearance11;
            this.uddColors.DisplayLayout.Override.RowSelectorWidth = 12;
            this.uddColors.DisplayLayout.Override.RowSpacingBefore = 2;
            appearance12.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance12.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance12.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance12.ForeColor = System.Drawing.Color.Black;
            this.uddColors.DisplayLayout.Override.SelectedRowAppearance = appearance12;
            this.uddColors.DisplayLayout.RowConnectorColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.uddColors.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.Solid;
            this.uddColors.Location = new System.Drawing.Point(304, 64);
            this.uddColors.Name = "uddColors";
            this.uddColors.Size = new System.Drawing.Size(75, 23);
            this.uddColors.TabIndex = 6;
            this.uddColors.Visible = false;
            this.uddColors.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.uddColors_InitializeLayout);
            // 
            // txtSizeMult
            // 
            this.txtSizeMult.Location = new System.Drawing.Point(96, 56);
            this.txtSizeMult.Name = "txtSizeMult";
            this.txtSizeMult.Size = new System.Drawing.Size(56, 20);
            this.txtSizeMult.TabIndex = 2;
            this.txtSizeMult.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtSizeMult.Validating += new System.ComponentModel.CancelEventHandler(this.txtPositiveInteger_Validating);
            // 
            // lblSizeMult
            // 
            this.lblSizeMult.Location = new System.Drawing.Point(16, 56);
            this.lblSizeMult.Name = "lblSizeMult";
            this.lblSizeMult.Size = new System.Drawing.Size(100, 23);
            this.lblSizeMult.TabIndex = 2;
            this.lblSizeMult.Text = "Size Multiple";
            // 
            // txtColorMult
            // 
            this.txtColorMult.Location = new System.Drawing.Point(96, 24);
            this.txtColorMult.Name = "txtColorMult";
            this.txtColorMult.Size = new System.Drawing.Size(56, 20);
            this.txtColorMult.TabIndex = 1;
            this.txtColorMult.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtColorMult.Validating += new System.ComponentModel.CancelEventHandler(this.txtPositiveInteger_Validating);
            // 
            // lblColorMult
            // 
            this.lblColorMult.Location = new System.Drawing.Point(16, 24);
            this.lblColorMult.Name = "lblColorMult";
            this.lblColorMult.Size = new System.Drawing.Size(100, 23);
            this.lblColorMult.TabIndex = 0;
            this.lblColorMult.Text = "Color Multiple";
            // 
            // ugColor
            // 
            this.ugColor.AllowDrop = true;
            this.ugColor.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ugColor.ContextMenu = this.mnuGrids;
            this.ugColor.DisplayLayout.AddNewBox.Hidden = false;
            this.ugColor.DisplayLayout.AddNewBox.Prompt = " Add ...";
            appearance13.BackColor = System.Drawing.Color.White;
            appearance13.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance13.BackGradientStyle = Infragistics.Win.GradientStyle.ForwardDiagonal;
            this.ugColor.DisplayLayout.Appearance = appearance13;
            ultraGridBand2.AddButtonCaption = "Grade";
            ultraGridBand2.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Extended;
            this.ugColor.DisplayLayout.BandsSerializer.Add(ultraGridBand2);
            this.ugColor.DisplayLayout.InterBandSpacing = 10;
            appearance14.BackColor = System.Drawing.Color.Transparent;
            this.ugColor.DisplayLayout.Override.CardAreaAppearance = appearance14;
            appearance15.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance15.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance15.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance15.ForeColor = System.Drawing.Color.Black;
            appearance15.TextHAlignAsString = "Left";
            appearance15.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            this.ugColor.DisplayLayout.Override.HeaderAppearance = appearance15;
            appearance16.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.ugColor.DisplayLayout.Override.RowAppearance = appearance16;
            appearance17.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance17.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance17.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            this.ugColor.DisplayLayout.Override.RowSelectorAppearance = appearance17;
            this.ugColor.DisplayLayout.Override.RowSelectorWidth = 12;
            this.ugColor.DisplayLayout.Override.RowSpacingBefore = 2;
            appearance18.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance18.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance18.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance18.ForeColor = System.Drawing.Color.Black;
            this.ugColor.DisplayLayout.Override.SelectedRowAppearance = appearance18;
            this.ugColor.DisplayLayout.RowConnectorColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.ugColor.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.Solid;
            this.ugColor.Location = new System.Drawing.Point(208, 50);
            this.ugColor.Name = "ugColor";
            this.ugColor.Size = new System.Drawing.Size(326, 303);
            this.ugColor.TabIndex = 4;
            this.ugColor.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.ugColor_InitializeLayout);
            this.ugColor.AfterRowInsert += new Infragistics.Win.UltraWinGrid.RowEventHandler(this.ugColor_AfterRowInsert);
            this.ugColor.BeforeRowUpdate += new Infragistics.Win.UltraWinGrid.CancelableRowEventHandler(this.ugColor_BeforeRowUpdate);
            this.ugColor.CellChange += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ugColor_CellChange);
            this.ugColor.MouseEnterElement += new Infragistics.Win.UIElementEventHandler(this.ugColor_MouseEnterElement);
            this.ugColor.MouseDown += new System.Windows.Forms.MouseEventHandler(this.GridMouseDown);
            // 
            // tabStoreGrade
            // 
            this.tabStoreGrade.Controls.Add(this.cmbSGAttributeSet);
            this.tabStoreGrade.Controls.Add(this.gbxMinMaxOpt);
            this.tabStoreGrade.Controls.Add(this.lblSGSet);
            this.tabStoreGrade.Controls.Add(this.cboSGStoreAttribute);
            this.tabStoreGrade.Controls.Add(this.lblSGAttribute);
            this.tabStoreGrade.Controls.Add(this.ugStoreGrades);
            this.tabStoreGrade.Location = new System.Drawing.Point(4, 22);
            this.tabStoreGrade.Name = "tabStoreGrade";
            this.tabStoreGrade.Size = new System.Drawing.Size(616, 367);
            this.tabStoreGrade.TabIndex = 1;
            this.tabStoreGrade.Text = "Store Grade";
            this.tabStoreGrade.UseVisualStyleBackColor = true;
            // 
            // cmbSGAttributeSet
            // 
            this.cmbSGAttributeSet.AutoAdjust = true;
            this.cmbSGAttributeSet.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmbSGAttributeSet.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbSGAttributeSet.DataSource = null;
            this.cmbSGAttributeSet.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSGAttributeSet.DropDownWidth = 184;
            this.cmbSGAttributeSet.FormattingEnabled = false;
            this.cmbSGAttributeSet.IgnoreFocusLost = false;
            this.cmbSGAttributeSet.ItemHeight = 13;
            this.cmbSGAttributeSet.Location = new System.Drawing.Point(379, 67);
            this.cmbSGAttributeSet.Margin = new System.Windows.Forms.Padding(0);
            this.cmbSGAttributeSet.MaxDropDownItems = 25;
            this.cmbSGAttributeSet.Name = "cmbSGAttributeSet";
            this.cmbSGAttributeSet.SetToolTip = "";
            this.cmbSGAttributeSet.Size = new System.Drawing.Size(184, 21);
            this.cmbSGAttributeSet.TabIndex = 48;
            this.cmbSGAttributeSet.Tag = null;
            this.cmbSGAttributeSet.MIDComboBoxPropertiesChangedEvent += new MIDRetail.Windows.Controls.MIDComboBoxPropertiesChangedEventHandler(this.cmbSGAttributeSet_MIDComboBoxPropertiesChangedEvent);
            this.cmbSGAttributeSet.SelectionChangeCommitted += new System.EventHandler(this.cmbSGAttributeSet_SelectionChangeCommitted);
            // 
            // gbxMinMaxOpt
            // 
            this.gbxMinMaxOpt.Controls.Add(this.cboInventoryBasis);
            this.gbxMinMaxOpt.Controls.Add(this.lblInventoryBasis);
            this.gbxMinMaxOpt.Controls.Add(this.radInventoryMinMax);
            this.gbxMinMaxOpt.Controls.Add(this.radAllocationMinMax);
            this.gbxMinMaxOpt.Location = new System.Drawing.Point(8, 3);
            this.gbxMinMaxOpt.Name = "gbxMinMaxOpt";
            this.gbxMinMaxOpt.Size = new System.Drawing.Size(593, 58);
            this.gbxMinMaxOpt.TabIndex = 49;
            this.gbxMinMaxOpt.TabStop = false;
            this.gbxMinMaxOpt.Text = "Min/Max Options:";
            // 
            // cboInventoryBasis
            // 
            this.cboInventoryBasis.AllowDrop = true;
            this.cboInventoryBasis.AutoAdjust = true;
            this.cboInventoryBasis.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.cboInventoryBasis.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboInventoryBasis.DataSource = null;
            this.cboInventoryBasis.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown;
            this.cboInventoryBasis.DropDownWidth = 184;
            this.cboInventoryBasis.FormattingEnabled = false;
            this.cboInventoryBasis.IgnoreFocusLost = true;
            this.cboInventoryBasis.ItemHeight = 13;
            this.cboInventoryBasis.Location = new System.Drawing.Point(372, 32);
            this.cboInventoryBasis.Margin = new System.Windows.Forms.Padding(0);
            this.cboInventoryBasis.MaxDropDownItems = 25;
            this.cboInventoryBasis.Name = "cboInventoryBasis";
            this.cboInventoryBasis.SetToolTip = "";
            this.cboInventoryBasis.Size = new System.Drawing.Size(184, 21);
            this.cboInventoryBasis.TabIndex = 3;
            this.cboInventoryBasis.Tag = null;
            this.cboInventoryBasis.MIDComboBoxPropertiesChangedEvent += new MIDRetail.Windows.Controls.MIDComboBoxPropertiesChangedEventHandler(this.cboInventoryBasis_MIDComboBoxPropertiesChangedEvent);
            this.cboInventoryBasis.SelectionChangeCommitted += new System.EventHandler(this.cboInventoryBasis_SelectionChangeCommitted);
            this.cboInventoryBasis.DragDrop += new System.Windows.Forms.DragEventHandler(this.cboInventoryBasis_DragDrop);
            this.cboInventoryBasis.DragEnter += new System.Windows.Forms.DragEventHandler(this.cboInventoryBasis_DragEnter);
            this.cboInventoryBasis.DragOver += new System.Windows.Forms.DragEventHandler(this.cboInventoryBasis_DragOver);
            this.cboInventoryBasis.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cboInventoryBasis_KeyDown);
            this.cboInventoryBasis.Validating += new System.ComponentModel.CancelEventHandler(this.cboInventoryBasis_Validating);
            this.cboInventoryBasis.Validated += new System.EventHandler(this.cboInventoryBasis_Validated);
            // 
            // lblInventoryBasis
            // 
            this.lblInventoryBasis.AutoSize = true;
            this.lblInventoryBasis.Location = new System.Drawing.Point(289, 38);
            this.lblInventoryBasis.Name = "lblInventoryBasis";
            this.lblInventoryBasis.Size = new System.Drawing.Size(82, 13);
            this.lblInventoryBasis.TabIndex = 2;
            this.lblInventoryBasis.Text = "Inventory Basis:";
            // 
            // radInventoryMinMax
            // 
            this.radInventoryMinMax.AutoSize = true;
            this.radInventoryMinMax.Location = new System.Drawing.Point(79, 38);
            this.radInventoryMinMax.Name = "radInventoryMinMax";
            this.radInventoryMinMax.Size = new System.Drawing.Size(114, 17);
            this.radInventoryMinMax.TabIndex = 1;
            this.radInventoryMinMax.TabStop = true;
            this.radInventoryMinMax.Text = "Inventory Min/Max";
            this.radInventoryMinMax.UseVisualStyleBackColor = true;
            this.radInventoryMinMax.CheckedChanged += new System.EventHandler(this.radInventoryMinMax_CheckedChanged);
            // 
            // radAllocationMinMax
            // 
            this.radAllocationMinMax.AutoSize = true;
            this.radAllocationMinMax.Location = new System.Drawing.Point(79, 17);
            this.radAllocationMinMax.Name = "radAllocationMinMax";
            this.radAllocationMinMax.Size = new System.Drawing.Size(116, 17);
            this.radAllocationMinMax.TabIndex = 0;
            this.radAllocationMinMax.TabStop = true;
            this.radAllocationMinMax.Text = "Allocation Min/Max";
            this.radAllocationMinMax.UseVisualStyleBackColor = true;
            this.radAllocationMinMax.CheckedChanged += new System.EventHandler(this.radAllocationMinMax_CheckedChanged);
            // 
            // lblSGSet
            // 
            this.lblSGSet.Location = new System.Drawing.Point(301, 71);
            this.lblSGSet.Name = "lblSGSet";
            this.lblSGSet.Size = new System.Drawing.Size(72, 17);
            this.lblSGSet.TabIndex = 47;
            this.lblSGSet.Text = "Attribute Set";
            // 
            // cboSGStoreAttribute
            // 
            this.cboSGStoreAttribute.AllowDrop = true;
            this.cboSGStoreAttribute.AllowUserAttributes = false;
            this.cboSGStoreAttribute.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboSGStoreAttribute.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboSGStoreAttribute.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSGStoreAttribute.Location = new System.Drawing.Point(87, 67);
            this.cboSGStoreAttribute.Name = "cboSGStoreAttribute";
            this.cboSGStoreAttribute.Size = new System.Drawing.Size(184, 21);
            this.cboSGStoreAttribute.TabIndex = 46;
            this.cboSGStoreAttribute.MIDComboBoxPropertiesChangedEvent += new MIDRetail.Windows.Controls.MIDComboBoxPropertiesChangedEventHandler(this.cboSGStoreAttribute_MIDComboBoxPropertiesChangedEvent);
            this.cboSGStoreAttribute.SelectionChangeCommitted += new System.EventHandler(this.cboSGStoreAttribute_SelectionChangeCommitted);
            this.cboSGStoreAttribute.DragEnter += new System.Windows.Forms.DragEventHandler(this.cboSGStoreAttribute_DragEnter);
            this.cboSGStoreAttribute.DragOver += new System.Windows.Forms.DragEventHandler(this.cboSGStoreAttribute_DragOver);
            // 
            // lblSGAttribute
            // 
            this.lblSGAttribute.Location = new System.Drawing.Point(33, 70);
            this.lblSGAttribute.Name = "lblSGAttribute";
            this.lblSGAttribute.Size = new System.Drawing.Size(48, 16);
            this.lblSGAttribute.TabIndex = 45;
            this.lblSGAttribute.Text = "Attribute";
            // 
            // ugStoreGrades
            // 
            this.ugStoreGrades.AllowDrop = true;
            this.ugStoreGrades.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ugStoreGrades.ContextMenu = this.mnuStoreGrades;
            this.ugStoreGrades.DisplayLayout.AddNewBox.Hidden = false;
            appearance19.BackColor = System.Drawing.Color.White;
            appearance19.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance19.BackGradientStyle = Infragistics.Win.GradientStyle.ForwardDiagonal;
            this.ugStoreGrades.DisplayLayout.Appearance = appearance19;
            ultraGridBand3.AddButtonCaption = "Grade";
            ultraGridBand3.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Extended;
            this.ugStoreGrades.DisplayLayout.BandsSerializer.Add(ultraGridBand3);
            this.ugStoreGrades.DisplayLayout.InterBandSpacing = 10;
            appearance20.BackColor = System.Drawing.Color.Transparent;
            this.ugStoreGrades.DisplayLayout.Override.CardAreaAppearance = appearance20;
            appearance21.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance21.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance21.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance21.ForeColor = System.Drawing.Color.Black;
            appearance21.TextHAlignAsString = "Left";
            appearance21.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            this.ugStoreGrades.DisplayLayout.Override.HeaderAppearance = appearance21;
            appearance22.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.ugStoreGrades.DisplayLayout.Override.RowAppearance = appearance22;
            appearance23.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance23.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance23.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            this.ugStoreGrades.DisplayLayout.Override.RowSelectorAppearance = appearance23;
            this.ugStoreGrades.DisplayLayout.Override.RowSelectorWidth = 12;
            this.ugStoreGrades.DisplayLayout.Override.RowSpacingBefore = 2;
            appearance24.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance24.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance24.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance24.ForeColor = System.Drawing.Color.Black;
            this.ugStoreGrades.DisplayLayout.Override.SelectedRowAppearance = appearance24;
            this.ugStoreGrades.DisplayLayout.RowConnectorColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.ugStoreGrades.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.Solid;
            this.ugStoreGrades.Location = new System.Drawing.Point(8, 92);
            this.ugStoreGrades.Name = "ugStoreGrades";
            this.ugStoreGrades.Size = new System.Drawing.Size(594, 255);
            this.ugStoreGrades.TabIndex = 5;
            this.ugStoreGrades.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.ugStoreGrades_InitializeLayout);
            this.ugStoreGrades.InitializeRow += new Infragistics.Win.UltraWinGrid.InitializeRowEventHandler(this.ugStoreGrades_InitializeRow);
            this.ugStoreGrades.AfterRowsDeleted += new System.EventHandler(this.ugStoreGrades_AfterRowsDeleted);
            this.ugStoreGrades.AfterRowInsert += new Infragistics.Win.UltraWinGrid.RowEventHandler(this.ugStoreGrades_AfterRowInsert);
            this.ugStoreGrades.AfterRowUpdate += new Infragistics.Win.UltraWinGrid.RowEventHandler(this.ugStoreGrades_AfterRowUpdate);
            this.ugStoreGrades.BeforeRowUpdate += new Infragistics.Win.UltraWinGrid.CancelableRowEventHandler(this.ugStoreGrades_BeforeRowUpdate);
            this.ugStoreGrades.CellChange += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ugStoreGrades_CellChange);
            this.ugStoreGrades.BeforeExitEditMode += new Infragistics.Win.UltraWinGrid.BeforeExitEditModeEventHandler(this.ugStoreGrades_BeforeExitEditMode);
            this.ugStoreGrades.BeforeRowsDeleted += new Infragistics.Win.UltraWinGrid.BeforeRowsDeletedEventHandler(this.ugStoreGrades_BeforeRowsDeleted);
            this.ugStoreGrades.AfterSortChange += new Infragistics.Win.UltraWinGrid.BandEventHandler(this.ugStoreGrades_AfterSortChange);
            this.ugStoreGrades.MouseEnterElement += new Infragistics.Win.UIElementEventHandler(this.ugStoreGrades_MouseEnterElement);
            this.ugStoreGrades.DragDrop += new System.Windows.Forms.DragEventHandler(this.ugStoreGrades_DragDrop);
            this.ugStoreGrades.DragEnter += new System.Windows.Forms.DragEventHandler(this.ugStoreGrades_DragEnter);
            this.ugStoreGrades.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ugStoreGrades_MouseDown);
            // 
            // tabCapacity
            // 
            this.tabCapacity.Controls.Add(this.cboStoreAttribute);
            this.tabCapacity.Controls.Add(this.ugCapacity);
            this.tabCapacity.Controls.Add(this.cbxExceedCapacity);
            this.tabCapacity.Controls.Add(this.lblAttribute);
            this.tabCapacity.Location = new System.Drawing.Point(4, 22);
            this.tabCapacity.Name = "tabCapacity";
            this.tabCapacity.Size = new System.Drawing.Size(616, 367);
            this.tabCapacity.TabIndex = 2;
            this.tabCapacity.Text = "Capacity";
            this.tabCapacity.UseVisualStyleBackColor = true;
            // 
            // cboStoreAttribute
            // 
            this.cboStoreAttribute.AllowDrop = true;
            this.cboStoreAttribute.AllowUserAttributes = false;
            this.cboStoreAttribute.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboStoreAttribute.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboStoreAttribute.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboStoreAttribute.Location = new System.Drawing.Point(80, 12);
            this.cboStoreAttribute.Name = "cboStoreAttribute";
            this.cboStoreAttribute.Size = new System.Drawing.Size(168, 21);
            this.cboStoreAttribute.TabIndex = 1;
            this.toolTip1.SetToolTip(this.cboStoreAttribute, "Attributes");
            this.cboStoreAttribute.MIDComboBoxPropertiesChangedEvent += new MIDRetail.Windows.Controls.MIDComboBoxPropertiesChangedEventHandler(this.cboStoreAttribute_MIDComboBoxPropertiesChangedEvent);
            this.cboStoreAttribute.SelectionChangeCommitted += new System.EventHandler(this.cboStoreAttribute_SelectionChangeCommitted);
            this.cboStoreAttribute.DragEnter += new System.Windows.Forms.DragEventHandler(this.cboStoreAttribute_DragEnter);
            this.cboStoreAttribute.DragOver += new System.Windows.Forms.DragEventHandler(this.cboStoreAttribute_DragOver);
            // 
            // ugCapacity
            // 
            this.ugCapacity.AllowDrop = true;
            this.ugCapacity.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ugCapacity.DisplayLayout.AddNewBox.Prompt = " ";
            appearance25.BackColor = System.Drawing.Color.White;
            appearance25.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance25.BackGradientStyle = Infragistics.Win.GradientStyle.ForwardDiagonal;
            this.ugCapacity.DisplayLayout.Appearance = appearance25;
            ultraGridBand4.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Extended;
            this.ugCapacity.DisplayLayout.BandsSerializer.Add(ultraGridBand4);
            this.ugCapacity.DisplayLayout.InterBandSpacing = 10;
            appearance26.BackColor = System.Drawing.Color.Transparent;
            this.ugCapacity.DisplayLayout.Override.CardAreaAppearance = appearance26;
            appearance27.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance27.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance27.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance27.ForeColor = System.Drawing.Color.Black;
            appearance27.TextHAlignAsString = "Left";
            appearance27.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            this.ugCapacity.DisplayLayout.Override.HeaderAppearance = appearance27;
            appearance28.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.ugCapacity.DisplayLayout.Override.RowAppearance = appearance28;
            appearance29.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance29.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance29.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            this.ugCapacity.DisplayLayout.Override.RowSelectorAppearance = appearance29;
            this.ugCapacity.DisplayLayout.Override.RowSelectorWidth = 12;
            this.ugCapacity.DisplayLayout.Override.RowSpacingBefore = 2;
            appearance30.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance30.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance30.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance30.ForeColor = System.Drawing.Color.Black;
            this.ugCapacity.DisplayLayout.Override.SelectedRowAppearance = appearance30;
            this.ugCapacity.DisplayLayout.RowConnectorColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.ugCapacity.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.Solid;
            this.ugCapacity.Location = new System.Drawing.Point(32, 40);
            this.ugCapacity.Name = "ugCapacity";
            this.ugCapacity.Size = new System.Drawing.Size(408, 309);
            this.ugCapacity.TabIndex = 3;
            this.ugCapacity.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.ugCapacity_InitializeLayout);
            this.ugCapacity.CellChange += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ugCapacity_CellChange);
            this.ugCapacity.MouseEnterElement += new Infragistics.Win.UIElementEventHandler(this.ugCapacity_MouseEnterElement);
            // 
            // cbxExceedCapacity
            // 
            this.cbxExceedCapacity.Location = new System.Drawing.Point(280, 16);
            this.cbxExceedCapacity.Name = "cbxExceedCapacity";
            this.cbxExceedCapacity.Size = new System.Drawing.Size(120, 16);
            this.cbxExceedCapacity.TabIndex = 2;
            this.cbxExceedCapacity.Text = "Exceed Capacity";
            this.cbxExceedCapacity.CheckStateChanged += new System.EventHandler(this.cbxExceedCapacity_CheckStateChanged);
            // 
            // lblAttribute
            // 
            this.lblAttribute.Location = new System.Drawing.Point(32, 16);
            this.lblAttribute.Name = "lblAttribute";
            this.lblAttribute.Size = new System.Drawing.Size(48, 16);
            this.lblAttribute.TabIndex = 0;
            this.lblAttribute.Text = "Attribute";
            // 
            // tabPackRounding
            // 
            this.tabPackRounding.Controls.Add(this.lblPackRoundingInfo);
            this.tabPackRounding.Controls.Add(this.ugPackRounding);
            this.tabPackRounding.Location = new System.Drawing.Point(4, 22);
            this.tabPackRounding.Name = "tabPackRounding";
            this.tabPackRounding.Padding = new System.Windows.Forms.Padding(3);
            this.tabPackRounding.Size = new System.Drawing.Size(616, 367);
            this.tabPackRounding.TabIndex = 3;
            this.tabPackRounding.Text = "Pack Rounding:";
            this.tabPackRounding.UseVisualStyleBackColor = true;
            // 
            // lblPackRoundingInfo
            // 
            this.lblPackRoundingInfo.AutoSize = true;
            this.lblPackRoundingInfo.Location = new System.Drawing.Point(26, 3);
            this.lblPackRoundingInfo.Name = "lblPackRoundingInfo";
            this.lblPackRoundingInfo.Size = new System.Drawing.Size(219, 13);
            this.lblPackRoundingInfo.TabIndex = 7;
            this.lblPackRoundingInfo.Text = "Apply Pack Rounding to Generic Packs Only";
            this.lblPackRoundingInfo.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // ugPackRounding
            // 
            this.ugPackRounding.AllowDrop = true;
            this.ugPackRounding.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ugPackRounding.ContextMenu = this.mnuPackRounding;
            this.ugPackRounding.DisplayLayout.AddNewBox.Hidden = false;
            appearance31.BackColor = System.Drawing.Color.White;
            appearance31.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance31.BackGradientStyle = Infragistics.Win.GradientStyle.ForwardDiagonal;
            this.ugPackRounding.DisplayLayout.Appearance = appearance31;
            ultraGridBand5.AddButtonCaption = "Pack Multiple";
            ultraGridBand5.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Extended;
            this.ugPackRounding.DisplayLayout.BandsSerializer.Add(ultraGridBand5);
            this.ugPackRounding.DisplayLayout.InterBandSpacing = 10;
            appearance32.BackColor = System.Drawing.Color.Transparent;
            this.ugPackRounding.DisplayLayout.Override.CardAreaAppearance = appearance32;
            appearance33.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance33.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance33.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance33.ForeColor = System.Drawing.Color.Black;
            appearance33.TextHAlignAsString = "Left";
            appearance33.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            this.ugPackRounding.DisplayLayout.Override.HeaderAppearance = appearance33;
            appearance34.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.ugPackRounding.DisplayLayout.Override.RowAppearance = appearance34;
            appearance35.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance35.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance35.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            this.ugPackRounding.DisplayLayout.Override.RowSelectorAppearance = appearance35;
            this.ugPackRounding.DisplayLayout.Override.RowSelectorWidth = 12;
            this.ugPackRounding.DisplayLayout.Override.RowSpacingBefore = 2;
            appearance36.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance36.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance36.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance36.ForeColor = System.Drawing.Color.Black;
            this.ugPackRounding.DisplayLayout.Override.SelectedRowAppearance = appearance36;
            this.ugPackRounding.DisplayLayout.RowConnectorColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.ugPackRounding.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.Solid;
            this.ugPackRounding.Location = new System.Drawing.Point(6, 30);
            this.ugPackRounding.Name = "ugPackRounding";
            this.ugPackRounding.Size = new System.Drawing.Size(360, 331);
            this.ugPackRounding.TabIndex = 6;
            this.ugPackRounding.AfterCellUpdate += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ugPackRounding_AfterCellUpdate);
            this.ugPackRounding.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.ugPackRounding_InitializeLayout);
            this.ugPackRounding.AfterRowInsert += new Infragistics.Win.UltraWinGrid.RowEventHandler(this.ugPackRounding_AfterRowInsert);
            this.ugPackRounding.BeforeRowsDeleted += new Infragistics.Win.UltraWinGrid.BeforeRowsDeletedEventHandler(this.ugPackRounding_BeforeRowsDeleted);
            // 
            // tabReservation
            // 
            this.tabReservation.Controls.Add(this.cbxRsrvDoNotApplyVSW);
            this.tabReservation.Controls.Add(this.ugReservation);
            this.tabReservation.Controls.Add(this.midAttributeCbx);
            this.tabReservation.Controls.Add(this.lblRsrvStrAtt);
            this.tabReservation.Location = new System.Drawing.Point(4, 22);
            this.tabReservation.Name = "tabReservation";
            this.tabReservation.Size = new System.Drawing.Size(616, 367);
            this.tabReservation.TabIndex = 4;
            this.tabReservation.Text = "Reservation";
            this.tabReservation.UseVisualStyleBackColor = true;
            // 
            // cbxRsrvDoNotApplyVSW
            // 
            this.cbxRsrvDoNotApplyVSW.AutoSize = true;
            this.cbxRsrvDoNotApplyVSW.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cbxRsrvDoNotApplyVSW.Location = new System.Drawing.Point(481, 17);
            this.cbxRsrvDoNotApplyVSW.Name = "cbxRsrvDoNotApplyVSW";
            this.cbxRsrvDoNotApplyVSW.Size = new System.Drawing.Size(117, 17);
            this.cbxRsrvDoNotApplyVSW.TabIndex = 37;
            this.cbxRsrvDoNotApplyVSW.Text = "Do Not Apply VSW";
            this.cbxRsrvDoNotApplyVSW.UseVisualStyleBackColor = true;
            this.cbxRsrvDoNotApplyVSW.CheckedChanged += new System.EventHandler(this.cbxRsrvDoNotApplyVSW_CheckedChanged);
            // 
            // ugReservation
            // 
            this.ugReservation.AllowDrop = true;
            this.ugReservation.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            appearance37.BackColor = System.Drawing.Color.White;
            appearance37.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance37.BackGradientStyle = Infragistics.Win.GradientStyle.ForwardDiagonal;
            this.ugReservation.DisplayLayout.Appearance = appearance37;
            this.ugReservation.DisplayLayout.InterBandSpacing = 10;
            appearance38.BackColor = System.Drawing.Color.Transparent;
            this.ugReservation.DisplayLayout.Override.CardAreaAppearance = appearance38;
            appearance39.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance39.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance39.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance39.ForeColor = System.Drawing.Color.Black;
            appearance39.TextHAlignAsString = "Left";
            appearance39.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            this.ugReservation.DisplayLayout.Override.HeaderAppearance = appearance39;
            appearance40.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.ugReservation.DisplayLayout.Override.RowAppearance = appearance40;
            appearance41.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance41.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance41.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            this.ugReservation.DisplayLayout.Override.RowSelectorAppearance = appearance41;
            this.ugReservation.DisplayLayout.Override.RowSelectorWidth = 12;
            this.ugReservation.DisplayLayout.Override.RowSpacingBefore = 2;
            appearance42.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance42.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance42.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance42.ForeColor = System.Drawing.Color.Black;
            this.ugReservation.DisplayLayout.Override.SelectedRowAppearance = appearance42;
            this.ugReservation.DisplayLayout.RowConnectorColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.ugReservation.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.Solid;
            this.ugReservation.Location = new System.Drawing.Point(13, 38);
            this.ugReservation.Name = "ugReservation";
            this.ugReservation.Size = new System.Drawing.Size(590, 318);
            this.ugReservation.TabIndex = 36;
            this.ugReservation.AfterCellUpdate += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ugReservation_AfterCellUpdate);
            this.ugReservation.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.ugReservation_InitializeLayout);
            this.ugReservation.BeforeRowUpdate += new Infragistics.Win.UltraWinGrid.CancelableRowEventHandler(this.ugReservation_BeforeRowUpdate);
            this.ugReservation.CellChange += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ugReservation_CellChange);
            this.ugReservation.DragDrop += new System.Windows.Forms.DragEventHandler(this.ugReservation_DragDrop);
            this.ugReservation.DragEnter += new System.Windows.Forms.DragEventHandler(this.ugReservation_DragEnter);
            // 
            // midAttributeCbx
            // 
            this.midAttributeCbx.AllowDrop = true;
            this.midAttributeCbx.AllowUserAttributes = false;
            this.midAttributeCbx.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.midAttributeCbx.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.midAttributeCbx.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.midAttributeCbx.Location = new System.Drawing.Point(252, 11);
            this.midAttributeCbx.Name = "midAttributeCbx";
            this.midAttributeCbx.Size = new System.Drawing.Size(200, 21);
            this.midAttributeCbx.TabIndex = 34;
            this.midAttributeCbx.MIDComboBoxPropertiesChangedEvent += new MIDRetail.Windows.Controls.MIDComboBoxPropertiesChangedEventHandler(this.midAttributeCbx_MIDComboBoxPropertiesChangedEvent);
            this.midAttributeCbx.SelectionChangeCommitted += new System.EventHandler(this.midAttributeCbx_SelectionChangeCommitted);
            // 
            // lblRsrvStrAtt
            // 
            this.lblRsrvStrAtt.Location = new System.Drawing.Point(164, 11);
            this.lblRsrvStrAtt.Name = "lblRsrvStrAtt";
            this.lblRsrvStrAtt.Size = new System.Drawing.Size(81, 23);
            this.lblRsrvStrAtt.TabIndex = 35;
            this.lblRsrvStrAtt.Text = "Store Attribute:";
            this.lblRsrvStrAtt.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblFilter
            // 
            this.lblFilter.Location = new System.Drawing.Point(538, 100);
            this.lblFilter.Name = "lblFilter";
            this.lblFilter.Size = new System.Drawing.Size(40, 16);
            this.lblFilter.TabIndex = 5;
            this.lblFilter.Text = "Filter:";
            this.lblFilter.Visible = false;
            this.lblFilter.Click += new System.EventHandler(this.lblFilter_Click);
            // 
            // gbxSettings
            // 
            this.gbxSettings.Controls.Add(this.lblWeeks);
            this.gbxSettings.Controls.Add(this.lblNeedLimit);
            this.gbxSettings.Controls.Add(this.txtNeedLimit);
            this.gbxSettings.Controls.Add(this.cbxExceedMax);
            this.gbxSettings.Controls.Add(this.txtStoreGradeTime);
            this.gbxSettings.Controls.Add(this.lblStoreGradeTime);
            this.gbxSettings.Location = new System.Drawing.Point(8, 6);
            this.gbxSettings.Name = "gbxSettings";
            this.gbxSettings.Size = new System.Drawing.Size(314, 93);
            this.gbxSettings.TabIndex = 2;
            this.gbxSettings.TabStop = false;
            this.gbxSettings.Text = "Settings";
            this.gbxSettings.Enter += new System.EventHandler(this.gbxSettings_Enter);
            // 
            // lblWeeks
            // 
            this.lblWeeks.Location = new System.Drawing.Point(232, 18);
            this.lblWeeks.Name = "lblWeeks";
            this.lblWeeks.Size = new System.Drawing.Size(48, 12);
            this.lblWeeks.TabIndex = 9;
            this.lblWeeks.Text = "weeks";
            // 
            // lblNeedLimit
            // 
            this.lblNeedLimit.Location = new System.Drawing.Point(8, 42);
            this.lblNeedLimit.Name = "lblNeedLimit";
            this.lblNeedLimit.Size = new System.Drawing.Size(104, 24);
            this.lblNeedLimit.TabIndex = 8;
            this.lblNeedLimit.Text = "Percent Need Limit";
            this.lblNeedLimit.Click += new System.EventHandler(this.lblNeedLimit_Click);
            // 
            // txtNeedLimit
            // 
            this.txtNeedLimit.Location = new System.Drawing.Point(144, 41);
            this.txtNeedLimit.Name = "txtNeedLimit";
            this.txtNeedLimit.Size = new System.Drawing.Size(80, 20);
            this.txtNeedLimit.TabIndex = 3;
            this.txtNeedLimit.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtNeedLimit.Validating += new System.ComponentModel.CancelEventHandler(this.txtNegToOneHundredPercent_Validating);
            // 
            // cbxExceedMax
            // 
            this.cbxExceedMax.Location = new System.Drawing.Point(9, 62);
            this.cbxExceedMax.Name = "cbxExceedMax";
            this.cbxExceedMax.Size = new System.Drawing.Size(120, 24);
            this.cbxExceedMax.TabIndex = 7;
            this.cbxExceedMax.Text = "Exceed Maximums";
            this.cbxExceedMax.CheckedChanged += new System.EventHandler(this.cbxExceedMax_CheckedChanged);
            // 
            // txtStoreGradeTime
            // 
            this.txtStoreGradeTime.Location = new System.Drawing.Point(144, 14);
            this.txtStoreGradeTime.MaxLength = 2;
            this.txtStoreGradeTime.Name = "txtStoreGradeTime";
            this.txtStoreGradeTime.Size = new System.Drawing.Size(80, 20);
            this.txtStoreGradeTime.TabIndex = 2;
            this.txtStoreGradeTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtStoreGradeTime.TextChanged += new System.EventHandler(this.txtStoreGradeTime_TextChanged);
            // 
            // lblStoreGradeTime
            // 
            this.lblStoreGradeTime.Location = new System.Drawing.Point(8, 17);
            this.lblStoreGradeTime.Name = "lblStoreGradeTime";
            this.lblStoreGradeTime.Size = new System.Drawing.Size(136, 16);
            this.lblStoreGradeTime.TabIndex = 0;
            this.lblStoreGradeTime.Text = "Store Grade Time Period";
            // 
            // gbxBasis
            // 
            this.gbxBasis.Controls.Add(this.cboOnHand);
            this.gbxBasis.Controls.Add(this.cboOTSPlan);
            this.gbxBasis.Controls.Add(this.txtOHFactor);
            this.gbxBasis.Controls.Add(this.lblOHFactor);
            this.gbxBasis.Controls.Add(this.lblOnHand);
            this.gbxBasis.Controls.Add(this.lblOtsPlan);
            this.gbxBasis.Location = new System.Drawing.Point(8, 112);
            this.gbxBasis.Name = "gbxBasis";
            this.gbxBasis.Size = new System.Drawing.Size(648, 89);
            this.gbxBasis.TabIndex = 3;
            this.gbxBasis.TabStop = false;
            this.gbxBasis.Text = "Basis";
            // 
            // cboOnHand
            // 
            this.cboOnHand.AllowDrop = true;
            this.cboOnHand.AutoAdjust = true;
            this.cboOnHand.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.cboOnHand.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboOnHand.DataSource = null;
            this.cboOnHand.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown;
            this.cboOnHand.DropDownWidth = 224;
            this.cboOnHand.FormattingEnabled = false;
            this.cboOnHand.IgnoreFocusLost = true;
            this.cboOnHand.ItemHeight = 13;
            this.cboOnHand.Location = new System.Drawing.Point(90, 56);
            this.cboOnHand.Margin = new System.Windows.Forms.Padding(0);
            this.cboOnHand.MaxDropDownItems = 25;
            this.cboOnHand.Name = "cboOnHand";
            this.cboOnHand.SetToolTip = "";
            this.cboOnHand.Size = new System.Drawing.Size(224, 21);
            this.cboOnHand.TabIndex = 11;
            this.cboOnHand.Tag = null;
            this.cboOnHand.MIDComboBoxPropertiesChangedEvent += new MIDRetail.Windows.Controls.MIDComboBoxPropertiesChangedEventHandler(this.cboOnHand_MIDComboBoxPropertiesChangedEvent);
            this.cboOnHand.SelectionChangeCommitted += new System.EventHandler(this.cboOnHand_SelectionChangeCommitted);
            this.cboOnHand.DragDrop += new System.Windows.Forms.DragEventHandler(this.cboOnHand_DragDrop);
            this.cboOnHand.DragEnter += new System.Windows.Forms.DragEventHandler(this.cboOnHand_DragEnter);
            this.cboOnHand.DragOver += new System.Windows.Forms.DragEventHandler(this.cboOnHand_DragOver);
            this.cboOnHand.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cboOnHand_KeyDown);
            this.cboOnHand.Validating += new System.ComponentModel.CancelEventHandler(this.cboOnHand_Validating);
            this.cboOnHand.Validated += new System.EventHandler(this.cboOnHand_Validated);
            // 
            // cboOTSPlan
            // 
            this.cboOTSPlan.AllowDrop = true;
            this.cboOTSPlan.AutoAdjust = true;
            this.cboOTSPlan.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.cboOTSPlan.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboOTSPlan.DataSource = null;
            this.cboOTSPlan.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown;
            this.cboOTSPlan.DropDownWidth = 224;
            this.cboOTSPlan.FormattingEnabled = false;
            this.cboOTSPlan.IgnoreFocusLost = true;
            this.cboOTSPlan.ItemHeight = 13;
            this.cboOTSPlan.Location = new System.Drawing.Point(90, 20);
            this.cboOTSPlan.Margin = new System.Windows.Forms.Padding(0);
            this.cboOTSPlan.MaxDropDownItems = 25;
            this.cboOTSPlan.Name = "cboOTSPlan";
            this.cboOTSPlan.SetToolTip = "";
            this.cboOTSPlan.Size = new System.Drawing.Size(224, 21);
            this.cboOTSPlan.TabIndex = 8;
            this.cboOTSPlan.Tag = null;
            this.cboOTSPlan.MIDComboBoxPropertiesChangedEvent += new MIDRetail.Windows.Controls.MIDComboBoxPropertiesChangedEventHandler(this.cboOTSPlan_MIDComboBoxPropertiesChangedEvent);
            this.cboOTSPlan.SelectionChangeCommitted += new System.EventHandler(this.cboOTSPlan_SelectionChangeCommitted);
            this.cboOTSPlan.DragDrop += new System.Windows.Forms.DragEventHandler(this.cboOTSPlan_DragDrop);
            this.cboOTSPlan.DragEnter += new System.Windows.Forms.DragEventHandler(this.cboOTSPlan_DragEnter);
            this.cboOTSPlan.DragOver += new System.Windows.Forms.DragEventHandler(this.cboOTSPlan_DragOver);
            this.cboOTSPlan.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cboOTSPlan_KeyDown);
            this.cboOTSPlan.Validating += new System.ComponentModel.CancelEventHandler(this.cboOTSPlan_Validating);
            this.cboOTSPlan.Validated += new System.EventHandler(this.cboOTSPlan_Validated);
            // 
            // txtOHFactor
            // 
            this.txtOHFactor.Location = new System.Drawing.Point(386, 54);
            this.txtOHFactor.Name = "txtOHFactor";
            this.txtOHFactor.Size = new System.Drawing.Size(100, 20);
            this.txtOHFactor.TabIndex = 10;
            this.txtOHFactor.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtOHFactor.TextChanged += new System.EventHandler(this.txtOHFactor_TextChanged);
            this.txtOHFactor.Validating += new System.ComponentModel.CancelEventHandler(this.txtOHFactor_Validating);
            // 
            // lblOHFactor
            // 
            this.lblOHFactor.Location = new System.Drawing.Point(330, 56);
            this.lblOHFactor.Name = "lblOHFactor";
            this.lblOHFactor.Size = new System.Drawing.Size(100, 20);
            this.lblOHFactor.TabIndex = 4;
            this.lblOHFactor.Text = "Factor %";
            // 
            // lblOnHand
            // 
            this.lblOnHand.Location = new System.Drawing.Point(8, 56);
            this.lblOnHand.Name = "lblOnHand";
            this.lblOnHand.Size = new System.Drawing.Size(64, 20);
            this.lblOnHand.TabIndex = 2;
            this.lblOnHand.Text = "On-Hand";
            // 
            // lblOtsPlan
            // 
            this.lblOtsPlan.Location = new System.Drawing.Point(8, 24);
            this.lblOtsPlan.Name = "lblOtsPlan";
            this.lblOtsPlan.Size = new System.Drawing.Size(80, 20);
            this.lblOtsPlan.TabIndex = 0;
            this.lblOtsPlan.Text = "OTS Forecast";
            // 
            // tabProperties
            // 
            this.tabProperties.Controls.Add(this.ugWorkflows);
            this.tabProperties.Location = new System.Drawing.Point(4, 22);
            this.tabProperties.Name = "tabProperties";
            this.tabProperties.Size = new System.Drawing.Size(694, 623);
            this.tabProperties.TabIndex = 0;
            this.tabProperties.Text = "Properties";
            this.tabProperties.Visible = false;
            // 
            // ugWorkflows
            // 
            this.ugWorkflows.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            appearance43.BackColor = System.Drawing.Color.White;
            appearance43.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance43.BackGradientStyle = Infragistics.Win.GradientStyle.ForwardDiagonal;
            this.ugWorkflows.DisplayLayout.Appearance = appearance43;
            this.ugWorkflows.DisplayLayout.InterBandSpacing = 10;
            appearance44.BackColor = System.Drawing.Color.Transparent;
            this.ugWorkflows.DisplayLayout.Override.CardAreaAppearance = appearance44;
            appearance45.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance45.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance45.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance45.ForeColor = System.Drawing.Color.Black;
            appearance45.TextHAlignAsString = "Left";
            appearance45.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            this.ugWorkflows.DisplayLayout.Override.HeaderAppearance = appearance45;
            appearance46.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.ugWorkflows.DisplayLayout.Override.RowAppearance = appearance46;
            appearance47.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance47.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance47.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            this.ugWorkflows.DisplayLayout.Override.RowSelectorAppearance = appearance47;
            this.ugWorkflows.DisplayLayout.Override.RowSelectorWidth = 12;
            this.ugWorkflows.DisplayLayout.Override.RowSpacingBefore = 2;
            appearance48.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance48.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance48.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance48.ForeColor = System.Drawing.Color.Black;
            this.ugWorkflows.DisplayLayout.Override.SelectedRowAppearance = appearance48;
            this.ugWorkflows.DisplayLayout.RowConnectorColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.ugWorkflows.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.Solid;
            this.ugWorkflows.Location = new System.Drawing.Point(24, 16);
            this.ugWorkflows.Name = "ugWorkflows";
            this.ugWorkflows.Size = new System.Drawing.Size(616, 585);
            this.ugWorkflows.TabIndex = 1;
            // 
            // mnuIMOGrid
            // 
            this.mnuIMOGrid.Popup += new System.EventHandler(this.mnuIMOGrid_Popup);
            // 
            // frmOverrideMethod
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(720, 731);
            this.Controls.Add(this.tabRuleMethod);
            this.Name = "frmOverrideMethod";
            this.Text = "Override Method";
            this.Load += new System.EventHandler(this.frmOverrideMethod_Load);
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
            this.gbWarehouse.ResumeLayout(false);
            this.gbWarehouse.PerformLayout();
            this.gbxConstraints.ResumeLayout(false);
            this.tabConstraints.ResumeLayout(false);
            this.tabColor.ResumeLayout(false);
            this.tabColor.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ugAllColors)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uddColors)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ugColor)).EndInit();
            this.tabStoreGrade.ResumeLayout(false);
            this.gbxMinMaxOpt.ResumeLayout(false);
            this.gbxMinMaxOpt.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ugStoreGrades)).EndInit();
            this.tabCapacity.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ugCapacity)).EndInit();
            this.tabPackRounding.ResumeLayout(false);
            this.tabPackRounding.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ugPackRounding)).EndInit();
            this.tabReservation.ResumeLayout(false);
            this.tabReservation.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ugReservation)).EndInit();
            this.gbxSettings.ResumeLayout(false);
            this.gbxSettings.PerformLayout();
            this.gbxBasis.ResumeLayout(false);
            this.gbxBasis.PerformLayout();
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
				_allocationOverrideMethod = new AllocationOverrideMethod(SAB,Include.NoRID);
				ABM = _allocationOverrideMethod;
				base.NewWorkflowMethod(aParentNode, eSecurityFunctions.AllocationMethodsUserAllocationOverride, eSecurityFunctions.AllocationMethodsGlobalAllocationOverride);

				Common_Load(aParentNode.GlobalUserType);

                // Begin TT#4473 - JSmith - Allocation Override Method -> Capacity Tab-> new method opens with attribute and no sets, allows check of Exceed Capacity-> when process it ignores capacity
                PopulateStoreAttributeSet(this.cboStoreAttribute.SelectedValue.ToString());
                // End TT#4473 - JSmith - Allocation Override Method -> Capacity Tab-> new method opens with attribute and no sets, allows check of Exceed Capacity-> when process it ignores capacity
			}
			catch(Exception ex)
			{
				string exceptionMessage = ex.Message;
			}
		}

		/// <summary>
		/// Opens an existing Allocation Override Method. 
		/// </summary>
		/// <param name="aSecurityLevel">The security of the user</param>
		/// <param name="aMethodRID">method_RID</param>
		/// <param name="aNodeRID">The record ID of the node</param>
		/// <param name="aNode">The node from the explorer where the workflow if being added</param>
		/// <param name="aLockStatus">The lock status of the data to be displayed</param>
		// public void InitializefrmOverrideMethod(int method_RID, int node_RID)
		override public void UpdateWorkflowMethod(int aMethodRID, int aNodeRID, MIDWorkflowMethodTreeNode aNode, eLockStatus aLockStatus)
		{
			try
			{
				_allocationOverrideMethod = new AllocationOverrideMethod(SAB,aMethodRID);
				base.UpdateWorkflowMethod(aLockStatus, eSecurityFunctions.AllocationMethodsUserAllocationOverride, eSecurityFunctions.AllocationMethodsGlobalAllocationOverride);
				Common_Load(aNode.GlobalUserType);
				_StoreGradeValueChange = true;
			}
			catch(Exception ex)
			{
				string exceptionMessage = ex.Message;
				FormLoadError = true;
				throw;
			}
		}

		/// <summary>
		/// Deletes an Override Method.
		/// </summary>
		/// <param name="method_RID">The record ID of the method</param>
		override public bool DeleteWorkflowMethod(int method_RID)
		{
			try
			{  
				_allocationOverrideMethod = new AllocationOverrideMethod(SAB,method_RID);
				return Delete();
			}
			catch(DatabaseForeignKeyViolation keyVio)
			{
				throw keyVio;
			}
			catch (Exception err)
			{
				string exceptionMessage = err.Message;
 				HandleException(err);
			}

			return true;
		}

		/// <summary>
		/// Renames a Allocation Override Method.
		/// </summary>
		/// <param name="aMethodRID">The record ID of the method</param>
		/// <param name="aNewName">The new name of the workflow or method</param>
		override public bool RenameWorkflowMethod(int aMethodRID, string aNewName)
		{
			try
			{       
				_allocationOverrideMethod = new AllocationOverrideMethod(SAB,aMethodRID);
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
				_allocationOverrideMethod = new AllocationOverrideMethod(SAB,aMethodRID);
				ProcessAction(eMethodType.AllocationOverride, true);
			}
			catch (Exception ex)
			{
				HandleException(ex);
				FormLoadError = true;
			}
		}

		private void SetText()
		{
			if (_allocationOverrideMethod.Method_Change_Type == eChangeType.update)
				this.btnSave.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_Update);
			else
				this.btnSave.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_Save);

			this.lblFilter.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Filter);
			this.gbxSettings.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Settings);
			this.lblStoreGradeTime.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_StoreGradeTime);
			this.lblWeeks.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Weeks); 
			this.lblNeedLimit.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_PctNeedLimit);
			// BEGIN TT#667 - Stodd - Pre-allocate Reserve
			this.lblReserve.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Reserve);
			// END TT#667 - Stodd - Pre-allocate Reserve
			this.rbReservePct.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Percent);
			this.rbReserveUnits.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Units);
			this.cbxExceedMax.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_ExceedsMax);

			this.gbxBasis.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Basis);
			this.lblOtsPlan.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_OTSPlan);
			this.lblOnHand.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_OnHand);
			this.lblOHFactor.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_FactorPct);
			
			this.gbxConstraints.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Constraints);
			this.lblColorMult.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_ColorMultiple);
			this.lblSizeMult.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_SizeMultiple);

            // BEGIN TT#1287 - AGallagher - Inventory Min/Max
            gbxMinMaxOpt.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_MinMaxOptions);
            radAllocationMinMax.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_AllocMinMax);
            radInventoryMinMax.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_InventoryMinMax);
            lblInventoryBasis.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_InventoryBasis);
            // END TT#1287 - AGallagher - Inventory Min/Max

			this.lblAttribute.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Attribute);
			this.cbxExceedCapacity.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_ExceedCapacity);

			this.btnClose.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_Close);
			this.btnProcess.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_Process);
			// BEGIN TT#667 - Stodd - Pre-allocate Reserve
			this.lblReserveAsBulk.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_ReserveAsBulk);
			this.lblReserveAsPacks.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_ReserveAsPacks);
			// END TT#667 - Stodd - Pre-allocate Reserve
			this.tabPackRounding.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Override_Pack_Rounding);  // TT#616 - AGallagher - Allocation - Pack Rounding (#67)
 			this.lblPackRoundingInfo.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Override_Pack_Rounding_Info);  // TT#616 - AGallagher - Allocation - Pack Rounding (#67)
            this.lblSGAttribute.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Attribute);
            this.lblSGSet.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_AttributeSet);
            // BEGIN TT#1401 - GTaylor - Reservation Stores
            this.tabReservation.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_IMO_Tab);
            EnhancedToolTip.SetToolTip(cbxRsrvDoNotApplyVSW, MIDText.GetTextOnly(eMIDTextCode.lbl_ApplyVSWCheckBox_Popup));
            EnhancedToolTip.SetToolTipWhenDisabled(ugReservation, MIDText.GetTextOnly(eMIDTextCode.lbl_VSWGridDisabled));
            // END TT#1401 - GTaylor - Reservation Stores
		}

		private void Common_Load(eGlobalUserType aGlobalUserType)
		{
			SetText();
			Name = MIDText.GetTextOnly((int)eMethodType.AllocationOverride);

			if (_allocationOverrideMethod.Method_Change_Type == eChangeType.add)
			{
				Format_Title(eDataState.New, eMIDTextCode.frm_OverrideMethod, null);
			}
			else
				if (FunctionSecurity.AllowUpdate)
			{
				Format_Title(eDataState.Updatable, eMIDTextCode.frm_OverrideMethod, _allocationOverrideMethod.Name);
			}
			else
			{
				Format_Title(eDataState.ReadOnly, eMIDTextCode.frm_OverrideMethod, _allocationOverrideMethod.Name);
			}

			if (FunctionSecurity.AllowExecute)
			{
				btnProcess.Enabled = true;
			}
			else
			{
				btnProcess.Enabled = false;
			}

			BuildDataTables();
			BindMerchandiseCombos();
			BindStoreAttrComboBox();
            			
			BindFilterComboBox();

			GetDataSource();
			LoadAllColorsGrid();
			ColorTab_Load();
			StoreGradesTab_Load();
			CapacityTab_Load();
            ReservationTab_Load();  //  TT#1401 - GTaylor - Reservation Stores - VSW
            PackRoundingTab_Load();  // TT#616 - AGallagher - Allocation - Pack Rounding (#67)
            BuildPRGridContextMenu();  // TT#616 - AGallagher - Allocation - Pack Rounding (#67)
			BuildGridContextMenu();
			SetReadOnly(FunctionSecurity.AllowUpdate);  //Security changes - 1/24/2005 vg
			LoadValues();
            // BEGIN TT#1287 - AGallagher - Inventory Min/Max
            if (_InventoryInd == 'I')
            {
                LoadMerchandiseCombo();
                LoadGenAllocValues();
            }
            // END TT#1287 - AGallagher - Inventory Min/Max

			LoadToolTipDefaults();
			CheckReserveValue();
		}

		private void BindFilterComboBox()
		{
			try
			{
				// Load Filters
				//StoreFilterData storeFilterDL = new StoreFilterData();
                FilterData storeFilterDL = new FilterData();
				ArrayList userRIDList = new ArrayList();
				userRIDList.Add(Include.GlobalUserRID);	// Issue 3806
				userRIDList.Add(SAB.ClientServerSession.UserRID);
                DataTable dtFilter = storeFilterDL.FilterRead(filterTypes.StoreFilter, eProfileType.FilterStore, userRIDList);

				cboOverrideFilter.Items.Clear();
				cboOverrideFilter.Items.Add(new FilterNameCombo(-1, Include.GlobalUserRID, "(None)"));	// issue 3806

				foreach (DataRow row in dtFilter.Rows)
				{
					cboOverrideFilter.Items.Add(
						new FilterNameCombo(Convert.ToInt32(row["FILTER_RID"], CultureInfo.CurrentUICulture),
						Convert.ToInt32(row["USER_RID"], CultureInfo.CurrentUICulture),
						Convert.ToString(row["FILTER_NAME"], CultureInfo.CurrentUICulture)));
				}
			}
			catch 
			{
				throw;
			}
		}

		private void BindMerchandiseCombos()
		{
			try
			{
                // Begin TT#709 - RMatelic - Override Method - need option to not set Forecast Level
                DataRow dRow = MerchandiseDataTable.NewRow();
                dRow["seqno"] = -1;
                dRow["leveltypename"] = eMerchandiseType.Undefined;
                dRow["text"] = string.Empty;
                dRow["key"] = -2;
                MerchandiseDataTable.Rows.Add(dRow);
                MerchandiseDataTable.DefaultView.Sort = "seqno";	
                // End TT#709  

                cboOTSPlan.DataSource = MerchandiseDataTable;
                cboOTSPlan.DisplayMember = "text";
                cboOTSPlan.ValueMember = "seqno";

				_merchDataTable2 =  MerchandiseDataTable.Copy();
                MerchandiseDataTable3 = MerchandiseDataTable.Copy();
                // BEGIN TT#1606 - AGallagher - Blank Inventory Min/Max Basis allowed
                DataRow[] rows = MerchandiseDataTable3.Select("seqno = -1");
                if (rows.Length == 1)
                { MerchandiseDataTable3.Rows.Remove(rows[0]);
                MerchandiseDataTable3.AcceptChanges();
                }
                // END TT#1606 - AGallagher - Blank Inventory Min/Max Basis allowed
                // Begin TT#709 - RMatelic - Override Method - need option to not set Forecast Level
                _merchDataTable2.DefaultView.Sort = "seqno";	
                // End TT#709  

				cboOnHand.DataSource = _merchDataTable2;
				cboOnHand.DisplayMember = "text";
				cboOnHand.ValueMember = "seqno";
				HierarchyProfile hp = SAB.HierarchyServerSession.GetMainHierarchyData();
				bool styleFound = false; 
				foreach (DataRow row in _merchDataTable2.Rows)
				{
					int key = Convert.ToInt32(row["key"],CultureInfo.CurrentUICulture);
					for (int levelIndex = 1; levelIndex <= hp.HierarchyLevels.Count; levelIndex++)
					{
						HierarchyLevelProfile hlp = (HierarchyLevelProfile)hp.HierarchyLevels[levelIndex];
						if (hlp.Key == key && hlp.LevelType == eHierarchyLevelType.Style)
						{
							_styleKey = key;
							// BEGIN MID Track #3218 
							_styleText = Convert.ToString(row["text"],CultureInfo.CurrentUICulture);
							// END MID Track #3218 
							styleFound = true;
							break;
						}
					}
					if (styleFound)
						break;
				}
			}
			catch (Exception ex)
			{
				HandleException(ex);
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
                BuildAttributeList();
				
                //this.cboStoreAttribute.ValueMember = "Key";
                //this.cboStoreAttribute.DisplayMember = "Name";
                //this.cboStoreAttribute.DataSource = al.ArrayList;
                // End Track #4872

				this.cboStoreAttribute.SelectedValue = _allocationOverrideMethod.SG_RID;
                //this.cboSGStoreAttribute.SelectedValue = _allocationOverrideMethod.sGstoreGroupRID;    // TT#618 - AGallagher - Allocation Override - Add Attribute Sets (#35) // TT#488 - MD - Jellis - Group Allocation
                this.cboSGStoreAttribute.SelectedValue = _allocationOverrideMethod.GradeStoreGroupRID;   // TT#488 - MD- Jellis - Group Allocation

                // Begin TT#1530 - RMatelic - Record locking violations from Store Attribute change
                // Begin Track #4872 - JSmith - Global/User Attributes
                //if (cboStoreAttribute.ContinueReadOnly)
                //{
                //    SetMethodReadOnly();
                //}
                // End Track #4872
                // Begin TT#1505 - RMatelic - Users Can Not View Global Variables >> change combo box prefix to 'cmb' to keep enabled when read only
                //else
                //{
                //    this.cboSGStoreAttribute.Enabled = FunctionSecurity.AllowUpdate;
                //}
                // End TT#1505  
                if (FunctionSecurity.AllowUpdate)
                {
                    if (cboStoreAttribute.ContinueReadOnly)
                    {
                        SetMethodReadOnly();
                    }
                }
                else
                {
                    this.cboStoreAttribute.Enabled = FunctionSecurity.AllowUpdate;
                    this.cboSGStoreAttribute.Enabled = FunctionSecurity.AllowUpdate;
                }
                // End TT#1530

                AdjustTextWidthComboBox_DropDown(cboSGStoreAttribute); // TT#7 - MD  RBeck - Dynamic dropdowns
            }
			catch (Exception exc)
			{HandleException(exc);	}
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
		}
        // BEGIN TT#616 - AGallagher - Allocation - Pack Rounding (#67)
        private void BuildPRGridContextMenu()
        {
            MenuItem mnuItemInsert = new MenuItem(MIDText.GetTextOnly(eMIDTextCode.lbl_Insert));
            //MenuItem mnuItemInsertBefore = new MenuItem(MIDText.GetTextOnly(eMIDTextCode.lbl_Insert_Before));
            //MenuItem mnuItemInsertAfter = new MenuItem(MIDText.GetTextOnly(eMIDTextCode.lbl_Insert_after));
            MenuItem mnuItemDelete = new MenuItem(MIDText.GetTextOnly(eMIDTextCode.lbl_Delete));
            //MenuItem mnuItemDeleteAll = new MenuItem(MIDText.GetTextOnly(eMIDTextCode.lbl_DeleteAll));

            mnuPackRounding.MenuItems.Add(mnuItemInsert);
            //mnuItemInsert.MenuItems.Add(mnuItemInsertBefore);
            //mnuItemInsert.MenuItems.Add(mnuItemInsertAfter);
            mnuPackRounding.MenuItems.Add(mnuItemDelete);
            //mnuGrids.MenuItems.Add(mnuItemDeleteAll);

            mnuItemInsert.Click += new System.EventHandler(this.mnuPackRoundingItemInsert_Click);
            //mnuItemInsertBefore.Click += new System.EventHandler(this.mnuGridsItemInsertBefore_Click);
            // mnuItemInsertAfter.Click += new System.EventHandler(this.mnuGridsItemInsertAfter_Click);
            mnuItemDelete.Click += new System.EventHandler(this.mnuPackRoundingItemDelete_Click);
            //mnuItemDeleteAll.Click += new System.EventHandler(this.mnuGridsItemDeleteAll_Click);
        }
        // END TT#616 - AGallagher - Allocation - Pack Rounding (#67)
		private void GridMouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right)
			{
				RightClickedFrom = (UltraGrid)sender;
			}
		}
		private void mnuGridsItemInsert_Click(object sender, System.EventArgs e)
		{
        }
        // BEGIN TT#616 - AGallagher - Allocation - Pack Rounding (#67)
        private void mnuPackRoundingItemInsert_Click(object sender, System.EventArgs e)
        {
            UltraGridRow addedRow = this.ugPackRounding.DisplayLayout.Bands[0].AddNew();
        }
        // END TT#616 - AGallagher - Allocation - Pack Rounding (#67)
        private void mnuGridsItemInsertBefore_Click(object sender, System.EventArgs e)
		{
			try
			{
				_setRowPosition = false;
				int rowPosition  = 0;
				if (ugColor.Rows.Count > 0)
				{
					if (this.ugColor.ActiveRow == null) return;
					rowPosition = Convert.ToInt32(this.ugColor.ActiveRow.Cells["RowPosition"].Value, CultureInfo.CurrentUICulture);
					// increment the position of the active row to end of grid
					foreach(  UltraGridRow gridRow in ugColor.Rows )
					{
						if (Convert.ToInt32(gridRow.Cells["RowPosition"].Value, CultureInfo.CurrentUICulture) >= rowPosition)
						{
							gridRow.Cells["RowPosition"].Value = Convert.ToInt32(gridRow.Cells["RowPosition"].Value, CultureInfo.CurrentUICulture) + 1;
						}
					}
				}
                _fromMenuInsert = true;          // TT#3154 - RMatelic - Allocation override color min max - right click and insert before or after receive a null reference exception
				UltraGridRow addedRow = this.ugColor.DisplayLayout.Bands[0].AddNew();
                _fromMenuInsert = false;         // TT#3154 - RMatelic - Allocation override color min max - right click and insert before or after receive a null reference exception
				addedRow.Cells["RowPosition"].Value = rowPosition;
				this.ugColor.DisplayLayout.Bands[0].SortedColumns.Clear();
				this.ugColor.DisplayLayout.Bands[0].SortedColumns.Add("RowPosition", false);
				_setRowPosition = true;
			}
			catch( Exception exception )
			{
				HandleException(exception);
			}
		}

		private void mnuGridsItemInsertAfter_Click(object sender, System.EventArgs e)
		{
			try
			{
				_setRowPosition = false;
				int rowPosition  = 0;
				if (ugColor.Rows.Count > 0)
				{
					if (this.ugColor.ActiveRow == null) return;
					rowPosition = Convert.ToInt32(this.ugColor.ActiveRow.Cells["RowPosition"].Value, CultureInfo.CurrentUICulture);
					// increment the position of the active row to end of grid
					foreach(  UltraGridRow gridRow in ugColor.Rows )
					{
						if (Convert.ToInt32(gridRow.Cells["RowPosition"].Value, CultureInfo.CurrentUICulture) > rowPosition)
						{
							gridRow.Cells["RowPosition"].Value = Convert.ToInt32(gridRow.Cells["RowPosition"].Value, CultureInfo.CurrentUICulture) + 1;
						}
					}
				}
                _fromMenuInsert = true;          // TT#3154 - RMatelic - Allocation override color min max - right click and insert before or after receive a null reference exception
				UltraGridRow addedRow = this.ugColor.DisplayLayout.Bands[0].AddNew();
                _fromMenuInsert = false;         // TT#3154 - RMatelic - Allocation override color min max - right click and insert before or after receive a null reference exception   
				addedRow.Cells["RowPosition"].Value = rowPosition + 1;
				this.ugColor.DisplayLayout.Bands[0].SortedColumns.Clear();
				this.ugColor.DisplayLayout.Bands[0].SortedColumns.Add("RowPosition", false);
				_setRowPosition = true;
			}
			catch( Exception exception )
			{
				HandleException(exception);
			}
		}

		private void mnuGridsItemDelete_Click(object sender, System.EventArgs e)
		{
			try
			{
				RightClickedFrom.DeleteSelectedRows();
			}
			catch (Exception exc)
			{
				HandleException(exc);	
			}
		}
        // BEGIN TT#616 - AGallagher - Allocation - Pack Rounding (#67)
        private void mnuPackRoundingItemDelete_Click(object sender, System.EventArgs e)
        {
            try
            {
              if (this.ugPackRounding.Selected.Rows.Count > 0)
                  this.ugPackRounding.DeleteSelectedRows();
            }
            catch (Exception exc)
            {
                HandleException(exc);
            }
        }
        // END TT#616 - AGallagher - Allocation - Pack Rounding (#67)
		private void mnuGridsItemDeleteAll_Click(object sender, System.EventArgs e)
		{
			try
			{
				_dsOverRide.Tables["Colors"].Clear();
				_dsOverRide.Tables["Colors"].AcceptChanges();
				ColorChangesMade = true;
			}
			catch (Exception exc)
			{
				HandleException(exc);	
			}
		}
		private void LoadToolTipDefaults()
		{
			MinMaxAllocationBin allocBin;
			allocBin = new MinMaxAllocationBin();
			_allColorsMaxValue = allocBin.LargestMaximum;
			 
			_storeGradeDefaultText = lblStoreGradeTime.Text + " default = " + SAB.ApplicationServerSession.GlobalOptions.StoreGradePeriod;
			this.toolTip1.SetToolTip(this.txtStoreGradeTime, _storeGradeDefaultText);

			_needLimitDefaultText = lblNeedLimit.Text + " default = " + SAB.ApplicationServerSession.GlobalOptions.PercentNeedLimit;
			this.toolTip1.SetToolTip(this.txtNeedLimit, _needLimitDefaultText);

			_allColorsMinDefaultText = ugAllColors.Rows[0].Cells["Color"].Text + " Minimum default = " + _allColorsMinValue.ToString(); 
			_allColorsMaxDefaultText = ugAllColors.Rows[0].Cells["Color"].Text + " Maximum default = " + _allColorsMaxValue.ToString("#,##0",CultureInfo.CurrentUICulture);
			ugAllColors.Rows[0].Cells["Minimum"].Tag = _allColorsMinDefaultText;
			ugAllColors.Rows[0].Cells["Maximum"].Tag = _allColorsMaxDefaultText;
	
		}		
        private void LoadValues()
		{
			if (_allocationOverrideMethod.Method_Change_Type != eChangeType.add)
			{
														
				this.txtName.Text = _allocationOverrideMethod.Name;
				this.txtDesc.Text = _allocationOverrideMethod.Method_Description;
								
				if (_allocationOverrideMethod.User_RID == Include.GetGlobalUserRID())
					radGlobal.Checked = true;
				else
					radUser.Checked = true;
			}

            GetWorkflows(_allocationOverrideMethod.Key, ugWorkflows);
			
			if (!_allocationOverrideMethod.UseStoreGradeDefault)
			{
				txtStoreGradeTime.Text = _allocationOverrideMethod.GradeWeekCount.ToString();
			}
			
			cbxExceedMax.Checked = _allocationOverrideMethod.ExceedMaximums;
			
			if (!_allocationOverrideMethod.UsePctNeedDefault)
			{
				txtNeedLimit.Text = _allocationOverrideMethod.PercentNeedLimit.ToString();
			}
			// BEGIN TT#667 - Stodd - Pre-allocate Reserve
			if (_allocationOverrideMethod.ReserveQty != Include.UndefinedReserve)
			{
				if (_allocationOverrideMethod.ReserveQty == 0)
				{
					txtReserve.Text = string.Empty;
				}
				else
				{
					txtReserve.Text = _allocationOverrideMethod.ReserveQty.ToString();
				}
			}
			// END TT#667 - Stodd - Pre-allocate Reserve

			rbReservePct.Checked = _allocationOverrideMethod.ReserveIsPercent;
			rbReserveUnits.Checked = !_allocationOverrideMethod.ReserveIsPercent;
			if (!_allocationOverrideMethod.UseFactorPctDefault)
				txtOHFactor.Text = _allocationOverrideMethod.OTSPlanFactorPercent.ToString();

			// BEGIN TT#667 - Stodd - Pre-allocate Reserve
			if (_allocationOverrideMethod.ReserveAsBulk == 0)
			{
				txtReserveAsBulk.Text = string.Empty;
			}
			else
			{
				txtReserveAsBulk.Text = _allocationOverrideMethod.ReserveAsBulk.ToString();
			}
			if (_allocationOverrideMethod.ReserveAsPacks == 0)
			{
				txtReserveAsPacks.Text = string.Empty;
			}
			else
			{
				txtReserveAsPacks.Text = _allocationOverrideMethod.ReserveAsPacks.ToString();
			}
			// END TT#667 - Stodd - Pre-allocate Reserve
			
			txtColorMult.Text = _allocationOverrideMethod.AllColorMultiple.ToString();
			txtSizeMult.Text = _allocationOverrideMethod.AllSizeMultiple.ToString();
			if (!SAB.ClientServerSession.GlobalOptions.AppConfig.SizeInstalled)
			{
				lblSizeMult.Visible = false;
				txtSizeMult.Visible = false;
			}
			if (_allocationOverrideMethod.Method_Change_Type != eChangeType.add)
				SetMerchandiseComboValues();
			cbxExceedCapacity.Checked = _allocationOverrideMethod.ExceedCapacity;

			if (_allocationOverrideMethod.StoreFilterRID == Include.UndefinedStoreFilter)
				cboOverrideFilter.SelectedIndex = -1;
			else
			{
				cboOverrideFilter.SelectedIndex = cboOverrideFilter.Items.IndexOf(new FilterNameCombo(_allocationOverrideMethod.StoreFilterRID, -1, ""));
			}
            // BEGIN TT#1287 - AGallagher - Inventory Min/Max
            switch (_allocationOverrideMethod.InventoryInd)
            {
                case 'A':
                    this.radAllocationMinMax.Checked = true;
                    cboInventoryBasis.Enabled = false;
                    cboInventoryBasis.Text = null;
                    _InventoryInd = 'A';
                    break;
                case 'I':
                    this.radInventoryMinMax.Checked = true;
                    cboInventoryBasis.Enabled = true;
                    _InventoryInd = 'I';
                    break;
            }
            // END TT#1287 - AGallagher - Inventory Min/Max

            //BEGIN TT#110-MD-VStuart - In Use Tool
            tabRuleMethod.Controls.Remove(tabProperties);
            //END TT#110-MD-VStuart - In Use Tool
		}
        // BEGIN TT#1287 - AGallagher - Inventory Min/Max
        private void LoadMerchandiseCombo()
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
        private void LoadGenAllocValues()
        {
            try
            {
                //if (_allocationOverrideMethod.Method_Change_Type == eChangeType.update)
                if (_allocationOverrideMethod.Method_Change_Type == eChangeType.update || _allocationOverrideMethod.Method_Change_Type == eChangeType.add)
                {
                    //Load Merchandise Node or Level Text to combo box
                    HierarchyNodeProfile hnp;
                    if (_allocationOverrideMethod.MERCH_HN_RID != Include.NoRID)
                    {
                        hnp = SAB.HierarchyServerSession.GetNodeData(_allocationOverrideMethod.MERCH_HN_RID, true, true);
                        AddNodeToMerchandiseCombo3(hnp);
                    }
                    else
                    {
                        if (_allocationOverrideMethod.MERCH_PH_RID != Include.NoRID)
                            SetComboToLevel(_allocationOverrideMethod.MERCH_PHL_SEQ);
                        else
                            cboInventoryBasis.SelectedIndex = 0;
                    }
                }

            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }
        // END TT#1287 - AGallagher - Inventory Min/Max

		private void SetMerchandiseComboValues()
		{
			//Load Merchandise Node or Level Text to combo box
			HierarchyNodeProfile hnp;
            // Begin TT#709 - RMatelic - Override Method - need option to not set Forecast Level  
            if (_allocationOverrideMethod.MerchUnspecified)
            {
                cboOTSPlan.SelectedValue = -1;  // blank
            }
			else if (_allocationOverrideMethod.OTSPlanRID != Include.NoRID)
			{
				//Begin Track #5378 - color and size not qualified
//				hnp = SAB.HierarchyServerSession.GetNodeData(_allocationOverrideMethod.OTSPlanRID);
                hnp = SAB.HierarchyServerSession.GetNodeData(_allocationOverrideMethod.OTSPlanRID, true, true);
				//End Track #5378
				AddNodeToOTSPlanCombo ( hnp );
			}
			else
			{
                if (_allocationOverrideMethod.OTSPlanPHL != Include.NoRID)
                {
                    SetOTSComboToLevel(_allocationOverrideMethod.OTSPlanPHLSeq);
                }
                else
                {
                    //cboOnHand.SelectedIndex = 0;
                    cboOTSPlan.SelectedValue = 0;  // OTS Plan Level
                }
                // End TT#709
			}

            // Begin TT#709 - RMatelic - Override Method - need option to not set Forecast Level  
            if (_allocationOverrideMethod.OnHandUnspecified)
            {
                cboOnHand.SelectedValue = -1;  // blank
            }
			else if (_allocationOverrideMethod.OTSOnHandRID != Include.NoRID)
			{
				//Begin Track #5378 - color and size not qualified
//				hnp = SAB.HierarchyServerSession.GetNodeData(_allocationOverrideMethod.OTSOnHandRID);
                hnp = SAB.HierarchyServerSession.GetNodeData(_allocationOverrideMethod.OTSOnHandRID, true, true);
				//End Track #5378
				AddNodeToOnHandCombo ( hnp );
			}
			else
			{
                if (_allocationOverrideMethod.OTSOnHandPHL != Include.NoRID)
                {
                    SetOnHandComboToLevel(_allocationOverrideMethod.OTSOnHandPHLSeq);
                }
                else
                //	cboOnHandPlan.SelectedIndex = 0;
                {
                    cboOnHand.SelectedValue = 0;  // OTS Plan Level
                }
            }   // End TT#709
		}

		private void frmOverrideMethod_Load(object sender, System.EventArgs e)
		{
            // Begin TT#1162 - JSmith - Duplicating Store Grade Error
            FormLoaded = false;
            // End TT#1162

			if (_capacityDataTable.Rows.Count == 0)
				PopulateStoreAttributeSet(this.cboStoreAttribute.SelectedValue.ToString());
                AdjustTextWidthComboBox_DropDown(cboStoreAttribute); // TT#7 - MD - RBeck - Dynamic dropdowns
            // Begin Track #4872 - JSmith - Global/User Attributes
            if (cboStoreAttribute.ReplaceAttribute)
            {
                ChangePending = true;
            }
            // End Track #4872
            // BEGIN TT#618 - AGallagher - Allocation Override - Add Attribute Sets (#35)
            //if (_storegradesDataTable.Rows.Count ==0)
               PopulateSGStoreAttributeSet(this.cboSGStoreAttribute.SelectedValue.ToString());
			   _dsOverRide.Tables["StoreGrades"].DefaultView.RowFilter = "SGLRID = " + this.cmbSGAttributeSet.SelectedValue.ToString();
               AdjustTextWidthComboBox_DropDown(cboSGStoreAttribute); // TT#7 - MD - RBeck - Dynamic dropdowns

            // END TT#618 - AGallagher - Allocation Override - Add Attribute Sets (#35)

            // Begin TT#1162 - JSmith - Duplicating Store Grade Error
            FormLoaded = true;
            // End TT#1162o
		}
		private void tabConstraints_Click(object sender, System.EventArgs e)
		{

		}
		#region Color Tab

		private void ugColor_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
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

		private void GetDataSource()
		{
			//DataRelation drOverRide;

			try
			{
				_dsOverRide = _allocationOverrideMethod.DSOverRide;

				// Begin TT#618 - STodd - Allocation Override - Add Attribute Sets (#35)
				//===============================================================================
				// For existing methods we don't have a node to recover the grade list from.
				// So instead, the original grade list will be the default.
				// If a new node is dragged to the store grades grid, _storeGradeList will be
				//   repopulated.
				//===============================================================================
				if (_storeGradeList == null)
				{
					_storeGradeList = new StoreGradeList(eProfileType.StoreGrade);
					foreach (DataRow row in _dsOverRide.Tables["GradeBoundary"].Rows)
					{
						string gradeCode = row["GradeCode"].ToString();
						int boundary = int.Parse(row["Boundary"].ToString());
						StoreGradeProfile gradeProf = new StoreGradeProfile(boundary);
						gradeProf.StoreGrade = gradeCode;
						gradeProf.Boundary = boundary;
						_storeGradeList.Add(gradeProf);
					}
				}
				// End TT#618 - STodd - Allocation Override - Add Attribute Sets (#35)

				//Create a relationship between the two tables and add it to the dataset.
				//So when the grid is bound to the dataset, the parent-child relationship
				//automatically displays nicely in the grid.

				//drOverRide = new DataRelation("Basis Details", _dsBasis.Tables["Basis"].Columns["BasisID"], _dsBasis.Tables["BasisDetails"].Columns["BasisID"]);
				//_dsOverRide.Relations.Add(drOverRide);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
		private void ColorTab_Load()
		{
			try
			{
				this.ugColor.DataSource = _dsOverRide.Tables["Colors"];
				this.ugColor.DisplayLayout.Bands[0].Columns["RowPosition"].Hidden = true;
				
				this.ugColor.DisplayLayout.AddNewBox.Hidden = false;
				this.ugColor.DisplayLayout.GroupByBox.Hidden = true;
				this.ugColor.DisplayLayout.GroupByBox.Prompt = "";
				this.ugColor.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
				this.ugColor.DisplayLayout.Bands[0].AddButtonCaption = MIDText.GetTextOnly(eMIDTextCode.lbl_ColorMinMax);
				
				this.ugColor.DisplayLayout.Bands[0].ColHeadersVisible = false;
				this.ugColor.DisplayLayout.Bands[0].Columns["Color"].Header.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_Color);
				this.ugColor.DisplayLayout.Bands[0].Columns["Color"].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Left;
				this.ugColor.DisplayLayout.Bands[0].Columns["Color"].Width = 115;
				this.ugColor.DisplayLayout.Bands[0].Columns["Color"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;

				this.ugColor.DisplayLayout.Bands[0].Columns["Minimum"].Header.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_Minimum);
				this.ugColor.DisplayLayout.Bands[0].Columns["Minimum"].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right;
				this.ugColor.DisplayLayout.Bands[0].Columns["Minimum"].Width = 82;
				
				this.ugColor.DisplayLayout.Bands[0].Columns["Maximum"].Header.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_Maximum);
				this.ugColor.DisplayLayout.Bands[0].Columns["Maximum"].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right;
				this.ugColor.DisplayLayout.Bands[0].Columns["Maximum"].Width = 82;

                //Begin TT#169 - JSmith - enter max of 1000 and receive 'must be integer'
                //FormatColumns(this.ugColor);
                //End TT#169
				this.ugColor.DisplayLayout.Bands[0].Columns["Color"].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Left;
			 
				if (!_colorIsPopulated)
				{
					_cd = new ColorData(); 
					_colorList = _cd.Colors_Read();
					uddColors.DataSource = _colorList;
					uddColors.DisplayLayout.Bands[0].Columns["COLOR_CODE_RID"].Hidden = true;	
					uddColors.DisplayLayout.Bands[0].Columns["COLOR_CODE_GROUP"].Hidden = true;	 
                    // BEGIN MID Track #6167 - Change color column headings
                    uddColors.DisplayLayout.Bands[0].Columns["VIRTUAL_IND"].Hidden = true;
                    uddColors.DisplayLayout.Bands[0].Columns["PURPOSE"].Hidden = true;
                    uddColors.DisplayLayout.Bands[0].Columns["COLOR_CODE_ID"].Header.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_Color);
                    uddColors.DisplayLayout.Bands[0].Columns["COLOR_CODE_NAME"].Header.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_Description); ;
                    // END MID Track #6167
                    uddColors.ValueMember = "COLOR_CODE_RID";  
					uddColors.DisplayMember = "COLOR_CODE_NAME"; 
									
					this.ugColor.DisplayLayout.Bands[0].Columns["Color"].ValueList = uddColors;
					this.ugColor.DisplayLayout.Bands[0].Columns["Color"].AutoEdit = true;
				}

				if (!FunctionSecurity.AllowUpdate)
				{
					this.ugColor.ContextMenu = null;
				}

				foreach (UltraGridBand ugb in this.ugColor.DisplayLayout.Bands)
				{
					if (!FunctionSecurity.AllowUpdate)
					{
						ugb.Override.AllowAddNew = AllowAddNew.No;
						ugb.Override.AllowDelete = DefaultableBoolean.False;
					}
				}
				_colorIsPopulated = true;


			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
		private void LoadAllColorsGrid()
		{
			try
			{
                _allColorDataTable = MIDEnvironment.CreateDataTable("allColorDataTable");
				DataColumn dataColumn;
				DataRow dRow;

				dataColumn = new DataColumn();
				dataColumn.DataType = System.Type.GetType("System.String");
				dataColumn.ColumnName = "Color";
				dataColumn.Caption = "Color";
				dataColumn.ReadOnly = false;
				dataColumn.Unique = false;
				dataColumn.AllowDBNull = true;
				_allColorDataTable.Columns.Add(dataColumn);

				dataColumn = new DataColumn();
				dataColumn.DataType = System.Type.GetType("System.Int32");
				dataColumn.ColumnName = "Minimum";
				dataColumn.Caption = "Minimum";
				dataColumn.ReadOnly = false;
				dataColumn.Unique = false;
				dataColumn.AllowDBNull = true;
				_allColorDataTable.Columns.Add(dataColumn);

				dataColumn = new DataColumn();
				dataColumn.DataType = System.Type.GetType("System.Int32");
				dataColumn.ColumnName = "Maximum";
				dataColumn.Caption = "Maximum";
				dataColumn.ReadOnly = false;
				dataColumn.Unique = false;
				dataColumn.AllowDBNull = true;
				_allColorDataTable.Columns.Add(dataColumn);
				dRow = _allColorDataTable.NewRow();
				dRow["Color"] = "All Colors";
				if (!_allocationOverrideMethod.UseAllColorsMinDefault)
					dRow["Minimum"] = _allocationOverrideMethod.AllColorMinimum;
				if (!_allocationOverrideMethod.UseAllColorsMaxDefault)
					dRow["Maximum"] = _allocationOverrideMethod.AllColorMaximum;
				_allColorDataTable.Rows.Add(dRow);
			
				this.ugAllColors.DataSource = _allColorDataTable;
		
				this.ugAllColors.DisplayLayout.GroupByBox.Hidden = true;
				this.ugAllColors.DisplayLayout.GroupByBox.Prompt = "";
				this.ugAllColors.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
				//this.ugAllColors.DisplayLayout.Bands[0].Override.RowSelectors. = DefaultableBoolean.False;
				 
				this.ugAllColors.DisplayLayout.Bands[0].Columns["Color"].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Left;
				this.ugAllColors.DisplayLayout.Bands[0].Columns["Color"].Width = 115;
				this.ugAllColors.DisplayLayout.Bands[0].Columns["Minimum"].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right;
				this.ugAllColors.DisplayLayout.Bands[0].Columns["Minimum"].Width = 82;
				this.ugAllColors.DisplayLayout.Bands[0].Columns["Maximum"].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right;
				this.ugAllColors.DisplayLayout.Bands[0].Columns["Maximum"].Width = 82;

                //Begin TT#169 - JSmith - enter max of 1000 and receive 'must be integer'
                //FormatColumns(this.ugAllColors);
                //End TT#169
				this.ugAllColors.DisplayLayout.Bands[0].Columns["Color"].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Left;

				UltraGridRow row = this.ugAllColors.Rows[0];
				row.Cells["Color"].Activation = Activation.ActivateOnly;
			} 
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
	
		#endregion

		#region Store Grade Tab

		private void ugStoreGrades_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
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
		
		private void StoreGradesTab_Load()
		{
            _storegradesDataTable = _dsOverRide.Tables["StoreGrades"];    // TT#618 - AGallagher - Allocation Override - Add Attribute Sets (#35)
			// Begin TT#618 - STodd - Allocation Override - Add Attribute Sets (#35)
			if (cmbSGAttributeSet.SelectedValue != null)
			{
				_storegradesDataTable.DefaultView.RowFilter = "SGLRID = " + this.cmbSGAttributeSet.SelectedValue.ToString();
			}
			// End TT#618 - STodd - Allocation Override - Add Attribute Sets (#35)

			this.ugStoreGrades.DataSource = _dsOverRide.Tables["StoreGrades"];
			this.ugStoreGrades.DisplayLayout.AddNewBox.Hidden = false;
			this.ugStoreGrades.DisplayLayout.GroupByBox.Hidden = true;
			this.ugStoreGrades.DisplayLayout.GroupByBox.Prompt = "";
			this.ugStoreGrades.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
			this.ugStoreGrades.DisplayLayout.Bands[0].AddButtonCaption = MIDText.GetTextOnly(eMIDTextCode.lbl_StoreGradeSingular);
			this.ugStoreGrades.DisplayLayout.Bands[0].Columns["RowPosition"].Hidden = true;
			this.ugStoreGrades.DisplayLayout.Bands[0].Columns["SGLRID"].Hidden = true;  // TT#618 - AGallagher - Allocation Override - Add Attribute Sets (#35)

//			this.ugStoreGrades.DisplayLayout.Override.AllowColSizing = Infragistics.Win.UltraWinGrid.AllowColSizing.Default;
			this.ugStoreGrades.DisplayLayout.Bands[0].Columns["Grade"].CellAppearance.TextHAlign =  Infragistics.Win.HAlign.Left;
			this.ugStoreGrades.DisplayLayout.Bands[0].Columns["Grade"].Width = 55;
			this.ugStoreGrades.DisplayLayout.Bands[0].Columns["Boundary"].CellAppearance.TextHAlign =  Infragistics.Win.HAlign.Right;
 			this.ugStoreGrades.DisplayLayout.Bands[0].Columns["Boundary"].Width = 70;
			this.ugStoreGrades.DisplayLayout.Bands[0].Columns["Allocation Min"].CellAppearance.TextHAlign =  Infragistics.Win.HAlign.Right;
			this.ugStoreGrades.DisplayLayout.Bands[0].Columns["Allocation Min"].Width = 85;
			this.ugStoreGrades.DisplayLayout.Bands[0].Columns["Allocation Min"].Nullable = Infragistics.Win.UltraWinGrid.Nullable.Null;
			this.ugStoreGrades.DisplayLayout.Bands[0].Columns["Allocation Max"].CellAppearance.TextHAlign =  Infragistics.Win.HAlign.Right;
			this.ugStoreGrades.DisplayLayout.Bands[0].Columns["Allocation Max"].Width = 85;
			this.ugStoreGrades.DisplayLayout.Bands[0].Columns["Min Ad"].CellAppearance.TextHAlign =  Infragistics.Win.HAlign.Right;
			this.ugStoreGrades.DisplayLayout.Bands[0].Columns["Min Ad"].Width = 65;
			this.ugStoreGrades.DisplayLayout.Bands[0].Columns["Color Min"].CellAppearance.TextHAlign =  Infragistics.Win.HAlign.Right;
			this.ugStoreGrades.DisplayLayout.Bands[0].Columns["Color Min"].Width = 65;
			this.ugStoreGrades.DisplayLayout.Bands[0].Columns["Color Max"].CellAppearance.TextHAlign =  Infragistics.Win.HAlign.Right;
			this.ugStoreGrades.DisplayLayout.Bands[0].Columns["Color Max"].Width = 65;
            // BEGIN TT#617 - AGallagher - Allocation Override - Ship Up To Rule (#36, #39)
            this.ugStoreGrades.DisplayLayout.Bands[0].Columns["Ship Up To"].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right;
            this.ugStoreGrades.DisplayLayout.Bands[0].Columns["Ship Up To"].Width = 65;
            // END TT#617 - AGallagher - Allocation Override - Ship Up To Rule (#36, #39)
            this.ugStoreGrades.DisplayLayout.Bands[0].Columns["Ship Up To"].Hidden = true;  // TT#853 - AGallagher - Allocation Override - Ship Up To by Grade Criteria - Hide until redesign is finalized
			this.ugStoreGrades.DisplayLayout.Bands[0].Columns["Grade"].Header.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_Grade);
			this.ugStoreGrades.DisplayLayout.Bands[0].Columns["Boundary"].Header.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_Boundary);
			this.ugStoreGrades.DisplayLayout.Bands[0].Columns["Allocation Min"].Header.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_Min_Stock);
			this.ugStoreGrades.DisplayLayout.Bands[0].Columns["Allocation Max"].Header.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_Max_Stock);
			this.ugStoreGrades.DisplayLayout.Bands[0].Columns["Min Ad"].Header.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_Min_Ad);
			this.ugStoreGrades.DisplayLayout.Bands[0].Columns["Color Min"].Header.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_Color_Min);
			this.ugStoreGrades.DisplayLayout.Bands[0].Columns["Color Max"].Header.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_Color_Max);
            this.ugStoreGrades.DisplayLayout.Bands[0].Columns["Ship Up To"].Header.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_Override_Ship_Up_To);  // TT#617 - AGallagher - Allocation Override - Ship Up To Rule (#36, #39)
			int colScrollWidth = this.ugStoreGrades.DisplayLayout.Bands[0].Columns["Grade"].Width;
			colScrollWidth += this.ugStoreGrades.DisplayLayout.Bands[0].Columns["Boundary"].Width;

			this.ugStoreGrades.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;
			//this.ugStoreGrades.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
			BuildStoreGradesContextmenu();
            //Begin TT#169 - JSmith - enter max of 1000 and receive 'must be integer'
            //FormatColumns(this.ugStoreGrades);
            //End TT#169
			this.ugStoreGrades.DisplayLayout.Bands[0].SortedColumns.Add("SGLRID", true);
			// END TT#618 - STodd - Allocation Override - Add Attribute Sets (#35)
			this.ugStoreGrades.DisplayLayout.Bands[0].SortedColumns.Add("Boundary", true); 	

			if (!FunctionSecurity.AllowUpdate)
			{
				this.ugStoreGrades.ContextMenu = null;
			}

			foreach (UltraGridBand ugb in this.ugStoreGrades.DisplayLayout.Bands)
			{
				if (!FunctionSecurity.AllowUpdate)
				{
					ugb.Override.AllowAddNew = AllowAddNew.No;
					ugb.Override.AllowDelete = DefaultableBoolean.False;
				}
			}

			_storeGradesIsPopulated = true;
		}
		#endregion

		#region Capacity Tab

		private void ugCapacity_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
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
		private void DefaultCapacityGridLayout()
		{
		}
		private void CapacityTab_Load()
		{
			_capacityDataTable = _dsOverRide.Tables["Capacity"];
			DataColumn dataColumn;
			dataColumn = new DataColumn();
			dataColumn.DataType = System.Type.GetType("System.Boolean");
			dataColumn.ColumnName = "Exceed Capacity";
			//	dataColumn.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_Exceed);
			dataColumn.Caption = "Exceed";
			dataColumn.ReadOnly = false;
			dataColumn.Unique = false;
			dataColumn.AllowDBNull = true;
			_capacityDataTable.Columns.Add(dataColumn);
            dataColumn.SetOrdinal(2); // TT#4473 - JSmith - Allocation Override Method -> Capacity Tab-> new method opens with attribute and no sets, allows check of Exceed Capacity-> when process it ignores capacity

			foreach(DataRow dr in _capacityDataTable.Rows)
			{
				if (dr["ExceedChar"].ToString() == "1")
					dr["Exceed Capacity"] = true;
				else
					dr["Exceed Capacity"] = false;
			} 
			this.ugCapacity.DataSource = _capacityDataTable;

			this.ugCapacity.DisplayLayout.Bands[0].Columns["SglRID"].Hidden = true;
			this.ugCapacity.DisplayLayout.Bands[0].Columns["ExceedChar"].Hidden = true;
			this.ugCapacity.DisplayLayout.AddNewBox.Hidden = true;
			this.ugCapacity.DisplayLayout.GroupByBox.Hidden = true;
			this.ugCapacity.DisplayLayout.GroupByBox.Prompt = "";
			this.ugCapacity.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
			this.ugCapacity.DisplayLayout.Bands[0].AddButtonCaption = "";

			this.ugCapacity.DisplayLayout.Bands[0].Columns["Set"].Header.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_Set);
			this.ugCapacity.DisplayLayout.Bands[0].Columns["Set"].CellAppearance.TextHAlign =  Infragistics.Win.HAlign.Left;
			this.ugCapacity.DisplayLayout.Bands[0].Columns["Set"].Width = 160;
			this.ugCapacity.DisplayLayout.Bands[0].Columns["Set"].Header.VisiblePosition = 1;
			this.ugCapacity.DisplayLayout.Bands[0].Columns["Set"].CellActivation = Activation.NoEdit;
			
			this.ugCapacity.DisplayLayout.Bands[0].Columns["Exceed Capacity"].Header.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_ExceedCapacity);
			this.ugCapacity.DisplayLayout.Bands[0].Columns["Exceed Capacity"].Width = 100;
			this.ugCapacity.DisplayLayout.Bands[0].Columns["Exceed Capacity"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox;
			this.ugCapacity.DisplayLayout.Bands[0].Columns["Exceed Capacity"].Header.VisiblePosition = 2;
			
			this.ugCapacity.DisplayLayout.Bands[0].Columns["Exceed by %"].Header.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_ExceedByPct);
			this.ugCapacity.DisplayLayout.Bands[0].Columns["Exceed by %"].CellAppearance.TextHAlign =  Infragistics.Win.HAlign.Right;
			this.ugCapacity.DisplayLayout.Bands[0].Columns["Exceed by %"].Width = 78;
			this.ugCapacity.DisplayLayout.Bands[0].Columns["Exceed by %"].Header.VisiblePosition = 3;

            //Begin TT#169 - JSmith - enter max of 1000 and receive 'must be integer'
            //FormatColumns(this.ugCapacity);
            //End TT#169
			 
			if (_allocationOverrideMethod.ExceedCapacity)
			{
				this.ugCapacity.Enabled = false;
			}

			if (!FunctionSecurity.AllowUpdate)
			{
				this.ugCapacity.ContextMenu = null;
			}

			foreach (UltraGridBand ugb in this.ugCapacity.DisplayLayout.Bands)
			{
				if (!FunctionSecurity.AllowUpdate)
				{
					ugb.Override.AllowAddNew = AllowAddNew.No;
					ugb.Override.AllowDelete = DefaultableBoolean.False;
				}
			}

			_capacityIsPopulated = true;
		}
		
		#endregion

        // BEGIN TT#616 - AGallagher - Allocation - Pack Rounding (#67)
        #region PackRounding Tab
                
        private void PackRoundingTab_Load()
        {
            this.ugPackRounding.DataSource = _dsOverRide.Tables["PackRounding"];
            this.ugPackRounding.DisplayLayout.AddNewBox.Hidden = false;
            this.ugPackRounding.DisplayLayout.GroupByBox.Hidden = true;
            this.ugPackRounding.DisplayLayout.GroupByBox.Prompt = "";
            this.ugPackRounding.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            //this.ugPackRounding.DisplayLayout.Bands[0].AddButtonCaption = "Pack Multiple";
            this.ugPackRounding.DisplayLayout.Bands[0].AddButtonCaption = MIDText.GetTextOnly(eMIDTextCode.lbl_Override_Pack_Multiple);

            this.ugPackRounding.DisplayLayout.Bands[0].Columns["PackText"].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Left;
            this.ugPackRounding.DisplayLayout.Bands[0].Columns["PackText"].Width = 142;

            this.ugPackRounding.DisplayLayout.Bands[0].Columns["FstPack"].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right;
            this.ugPackRounding.DisplayLayout.Bands[0].Columns["FstPack"].Format = "##0.##";
            this.ugPackRounding.DisplayLayout.Bands[0].Columns["FstPack"].Width = 52;

            this.ugPackRounding.DisplayLayout.Bands[0].Columns["NthPack"].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right;
            this.ugPackRounding.DisplayLayout.Bands[0].Columns["NthPack"].Format = "##0.##";
            this.ugPackRounding.DisplayLayout.Bands[0].Columns["NthPack"].Width = 52;

            this.ugPackRounding.DisplayLayout.Bands[0].Columns["PackMultiple"].Hidden = true;

            //Begin TT#169 - JSmith - enter max of 1000 and receive 'must be integer'
            //FormatColumns(this.ugPackRounding);
            //End TT#169

            if (!FunctionSecurity.AllowUpdate)
            {
                this.ugPackRounding.ContextMenu = null;
            }

            foreach (UltraGridBand upr in this.ugPackRounding.DisplayLayout.Bands)
            {
                if (!FunctionSecurity.AllowUpdate)
                {
                    upr.Override.AllowAddNew = AllowAddNew.No;
                    upr.Override.AllowDelete = DefaultableBoolean.False;
                }
            }

            _packRoundingIsPopulated = true;
        }

        #endregion
        // END TT#616 - AGallagher - Allocation - Pack Rounding (#67)


		private void tabConstraints_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			EditMsgs em = new EditMsgs();
			if (this.tabConstraints.SelectedTab.Name != _currentTabPage.Name)
			{
				switch (_currentTabPage.Name)
				{
					case "tabColor":
						if (ColorChangesMade || ColorMinMaxesChangesMade)
						{
							if (!ValidTabColor(ref em))
							{
								this.tabConstraints.SelectedTab = this.tabColor;
							}
						}
						break;
					
					case "tabStoreGrade":
						if (StoreGradesChangesMade || MinMaxesChangesMade)
						{
							if (!ValidTabStoreGrades(ref em))
							{
								this.tabConstraints.SelectedTab = this.tabStoreGrade;	
							}
						}
						break;
					case "tabCapacity":
						if (CapacityChangesMade)
						{
							if (!ValidTabCapacity(ref em))
							{
								this.tabConstraints.SelectedTab = this.tabCapacity;	
							}
						}
						break;
                    // BEGIN TT#616 - AGallagher - Allocation - Pack Rounding (#67)
                    case "tabPackRounding":
                        if (PackRoundingChangesMade)
                        {
                            if (!ValidTabPackRounding(ref em))
                            {
                                this.tabConstraints.SelectedTab = this.tabPackRounding;
                            }
                        }
                        break;
                    // END TT#616 - AGallagher - Allocation - Pack Rounding (#67)
                    //Begin TT#1401 - Reservation Stores
                    case "tabReservation":
                        if (IMOChangesMade)
                        {
                            if (!ValidTabReservation(ref em))
                            {
                                this.tabConstraints.SelectedTab = this.tabReservation;
                            }
                        }
                        break;
                    //End TT#1401 - Reservation Stores
				}
			}
			if (em.EditMessages.Count > 0)
			{
				_errors = null;
				for (int i=0; i<em.EditMessages.Count; i++)
				{
					EditMsgs.Message emm = (EditMsgs.Message) em.EditMessages[i];
					AddErrorMessage(emm);
				}
				MessageBox.Show (_errors,  this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			else
			{
				_currentTabPage = this.tabConstraints.SelectedTab;
				switch (_currentTabPage.Name)
				{
					case "tabColor":
						if (!_colorIsPopulated)
						{
							ColorTab_Load();
						}
						break;
					case "tabStoreGrades":
						if (!_storeGradesIsPopulated)
						{
							StoreGradesTab_Load();
						}
						break;
					case "tabCapacity":
						if (!_capacityIsPopulated)
						{
							CapacityTab_Load();
						}
						this.ugCapacity.DisplayLayout.Bands[0].Columns["Set"].CellActivation = Activation.ActivateOnly;
						break;
                    // BEGIN TT#616 - AGallagher - Allocation - Pack Rounding (#67)
                    case "tabPackRounding":
                        if (!_packRoundingIsPopulated)
                        {
                            PackRoundingTab_Load();
                        }
                        break;
                    // END TT#616 - AGallagher - Allocation - Pack Rounding (#67)
                    //Begin TT#1401 - Reservation Stores
                    case "tabReservation":
                        if (!_imoIsPopulated)
                        {
                            ReservationTab_Load();
                        }
                        break;
                    //End TT#1401 - Reservation Stores
				}
			}
		}

		private string AddErrorMessage(eMIDTextCode textCode)
		{
			string error = null;
			try
			{
				error = SAB.ClientServerSession.Audit.GetText(textCode);
				_errors += Environment.NewLine + "     "  + error;
				return error;
			}
			catch( Exception exception )
			{
				error = exception.Message;
				HandleException(exception);
				return error;
			}
		}

		private string AddErrorMessage(EditMsgs.Message emm)
		{
			string error = null;
			try
			{
				if (emm.code != 0)
				{
					error = SAB.ClientServerSession.Audit.GetText(emm.code);
				}
				else
				{
					error = emm.msg;
				}
				_errors += Environment.NewLine + "     "  + error;
				return error;
			}
			catch( Exception exception )
			{
				HandleException(exception);
				error = exception.Message;
				return error;
			}
		}

        //Begin TT#169 - JSmith - enter max of 1000 and receive 'must be integer'
        //private void FormatColumns(Infragistics.Win.UltraWinGrid.UltraGrid ultragid)
        //{
        //    foreach ( Infragistics.Win.UltraWinGrid.UltraGridBand oBand in ultragid.DisplayLayout.Bands )
        //    {
        //        foreach ( Infragistics.Win.UltraWinGrid.UltraGridColumn oColumn in oBand.Columns )
        //        {
        //            switch (oColumn.DataType.ToString())
        //            {
        //                case "System.Int32":
        //                    oColumn.CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right;
        //                    oColumn.Format = "#,###,##0";
        //                    // begin MID Track 6255 Enter max "1,000" and get error "must be integer" 
        //                    oColumn.MaskInput = "9999999";
        //                    oColumn.PromptChar = ' ';
        //                    // end MID Track 6255 Enter max "1,000" and get error "must be integer" 
        //                    break;
        //                case "System.Double":
        //                    oColumn.CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right;
        //                    oColumn.Format = "#,###,###.00";
        //                    break;
        //            }
        //        }
        //    }
        //}
        //End TT#169

		// Begin MID Track 4858 - JSmith - Security changes
//		private void btnClose_Click(object sender, System.EventArgs e)
//		{
//			try
//			{
//				Cancel_Click();
//			}
//			catch (Exception ex)
//			{
//				HandleException(ex);
//			}
//		}

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
            // BEGIN TT#1287 - AGallagher - Inventory Min/Max
            _allocationOverrideMethod.InventoryInd = _InventoryInd;   
            //Merchandise Level
            if (_InventoryInd == 'I')
            {
                DataRow myDataRow2 = MerchandiseDataTable3.Rows[cboInventoryBasis.SelectedIndex];
                eMerchandiseType MerchandiseType2 = (eMerchandiseType)(Convert.ToInt32(myDataRow2["leveltypename"], CultureInfo.CurrentUICulture));
                _allocationOverrideMethod.MerchandiseType = MerchandiseType2;

                switch (MerchandiseType2)
                {
                    case eMerchandiseType.Node:
                        _allocationOverrideMethod.MERCH_HN_RID = Convert.ToInt32(myDataRow2["key"], CultureInfo.CurrentUICulture);
                        break;
                    case eMerchandiseType.HierarchyLevel:
                        _allocationOverrideMethod.MERCH_PHL_SEQ = Convert.ToInt32(myDataRow2["key"], CultureInfo.CurrentUICulture);
                        _allocationOverrideMethod.MERCH_PH_RID = HP.Key;
                        _allocationOverrideMethod.MERCH_HN_RID = Include.NoRID;
                        break;
                    case eMerchandiseType.OTSPlanLevel:
                        _allocationOverrideMethod.MERCH_HN_RID = Include.NoRID;
                        _allocationOverrideMethod.MERCH_PH_RID = Include.NoRID;
                        _allocationOverrideMethod.MERCH_PHL_SEQ = 0;
                        break;
                }
            }
            else
            {
                _allocationOverrideMethod.MERCH_HN_RID = Include.NoRID;
                _allocationOverrideMethod.MERCH_PH_RID = Include.NoRID;
                _allocationOverrideMethod.MERCH_PHL_SEQ = 0;
            }

            // END TT#1287 - AGallagher - Inventory Min/Max
            // Store grade timeframe
            if (txtStoreGradeTime.Text.Length > 0)
            {
                _allocationOverrideMethod.GradeWeekCount = Convert.ToInt32(txtStoreGradeTime.Text, CultureInfo.CurrentUICulture);
                _allocationOverrideMethod.UseStoreGradeDefault = false;
            }
            else
            {
                _allocationOverrideMethod.GradeWeekCount = SAB.ApplicationServerSession.GlobalOptions.StoreGradePeriod;
                _allocationOverrideMethod.UseStoreGradeDefault = true;
            }

            // Exceed maximum ind
            _allocationOverrideMethod.ExceedMaximums = cbxExceedMax.Checked;
            // Percent need limit amount
            if (txtNeedLimit.Text != null && txtNeedLimit.Text.Trim() != string.Empty)
            {
                _allocationOverrideMethod.PercentNeedLimit = Convert.ToDouble(txtNeedLimit.Text, CultureInfo.CurrentUICulture);
                _allocationOverrideMethod.UsePctNeedDefault = false;
            }
            else
            {
                _allocationOverrideMethod.PercentNeedLimit = SAB.ApplicationServerSession.GlobalOptions.PercentNeedLimit;
                _allocationOverrideMethod.UsePctNeedDefault = true;
            }
            // Reserve amount
            if (txtReserve.Text.Trim().Length > 0)
                _allocationOverrideMethod.ReserveQty = Convert.ToDouble(txtReserve.Text, CultureInfo.CurrentUICulture);
            else
                _allocationOverrideMethod.ReserveQty = Include.UndefinedReserve;

            // BEGIN TT#667 - Stodd - Pre-allocate Reserve
            // REserve as Bulk & Reserve As Packs
            if (txtReserveAsBulk.Text != null && txtReserveAsBulk.Text.Trim() != string.Empty)
            {
                _allocationOverrideMethod.ReserveAsBulk = double.Parse(txtReserveAsBulk.Text);
            }
            else
            {
                _allocationOverrideMethod.ReserveAsBulk = 0;
            }

            if (txtReserveAsPacks.Text != null && txtReserveAsPacks.Text.Trim() != string.Empty)
            {
                _allocationOverrideMethod.ReserveAsPacks = double.Parse(txtReserveAsPacks.Text);
            }
            else
            {
                _allocationOverrideMethod.ReserveAsPacks = 0;
            }
            // END TT#667 - Stodd - Pre-allocate Reserve

            //Percent Ind
            _allocationOverrideMethod.ReserveIsPercent = rbReservePct.Checked;
            // Factor %
            if (txtOHFactor.Text.Trim().Length > 0)
            {
                _allocationOverrideMethod.OTSPlanFactorPercent = Convert.ToDouble(txtOHFactor.Text, CultureInfo.CurrentUICulture);
                _allocationOverrideMethod.UseFactorPctDefault = false;
            }
            else
            {
                _allocationOverrideMethod.OTSPlanFactorPercent = Include.DefaultPlanFactorPercent;
                _allocationOverrideMethod.UseFactorPctDefault = true;
            }
            //OTS Plan Merchandise Level
            // Begin TT#709 - RMatelic - Override Method - need option to not set Forecast Level
            //DataRow myDataRow = MerchandiseDataTable.Rows[cboOTSPlan.SelectedIndex];
            DataRow myDataRow = null;
            for (int i = 0; i < MerchandiseDataTable.Rows.Count; i++)
            {
                myDataRow = MerchandiseDataTable.Rows[i];
                if (Convert.ToInt32(myDataRow["seqno"], CultureInfo.CurrentUICulture) == Convert.ToInt32(cboOTSPlan.SelectedValue, CultureInfo.CurrentUICulture))
                {
                    break;
                }
            }
            // End TT#709  
            eMerchandiseType MerchandiseType = (eMerchandiseType)(Convert.ToInt32(myDataRow["leveltypename"], CultureInfo.CurrentUICulture));

            switch (MerchandiseType)
            {
                case eMerchandiseType.Node:
                    _allocationOverrideMethod.OTSPlanRID = Convert.ToInt32(myDataRow["key"], CultureInfo.CurrentUICulture);
                    _allocationOverrideMethod.OTSPlanPHL = Include.NoRID;
                    _allocationOverrideMethod.OTSPlanPHLSeq = 0;
                    _allocationOverrideMethod.MerchUnspecified = false; // TT#709 - RMatelic - Override Method - need option to not set Forecast Level
                    break;
                case eMerchandiseType.HierarchyLevel:
                    _allocationOverrideMethod.OTSPlanRID = Include.NoRID;
                    _allocationOverrideMethod.OTSPlanPHL = HP.Key;
                    _allocationOverrideMethod.OTSPlanPHLSeq = Convert.ToInt32(myDataRow["key"], CultureInfo.CurrentUICulture);
                    _allocationOverrideMethod.MerchUnspecified = false; // TT#709 - RMatelic - Override Method - need option to not set Forecast Level
                    break;
                case eMerchandiseType.OTSPlanLevel:
                    _allocationOverrideMethod.OTSPlanRID = Include.NoRID;
                    _allocationOverrideMethod.OTSPlanPHL = Include.NoRID;
                    _allocationOverrideMethod.OTSPlanPHLSeq = 0;
                    _allocationOverrideMethod.MerchUnspecified = false; // TT#709 - RMatelic - Override Method - need option to not set Forecast Level
                    break;
                // Begin TT#709 - RMatelic - Override Method - need option to not set Forecast Level
                case eMerchandiseType.Undefined:
                    _allocationOverrideMethod.OTSPlanRID = Include.NoRID;
                    _allocationOverrideMethod.OTSPlanPHL = Include.NoRID;
                    _allocationOverrideMethod.OTSPlanPHLSeq = 0;
                    _allocationOverrideMethod.MerchUnspecified = true; // TT#709 - RMatelic - Override Method - need option to not set Forecast Level
                    break;
                // End TT#709  
            }

            //On Hand Merchandise Level
            // Begin TT#709 - RMatelic - Override Method - need option to not set Forecast Level
            //myDataRow = _merchDataTable2.Rows[cboOnHand.SelectedIndex];
            myDataRow = null;
            for (int i = 0; i < _merchDataTable2.Rows.Count; i++)
            {
                myDataRow = _merchDataTable2.Rows[i];
                if (Convert.ToInt32(myDataRow["seqno"], CultureInfo.CurrentUICulture) == Convert.ToInt32(cboOnHand.SelectedValue, CultureInfo.CurrentUICulture))
                {
                    break;
                }
            }
            // End TT#709  
            MerchandiseType = (eMerchandiseType)(Convert.ToInt32(myDataRow["leveltypename"], CultureInfo.CurrentUICulture));

            switch (MerchandiseType)
            {
                case eMerchandiseType.Node:
                    _allocationOverrideMethod.OTSOnHandRID = Convert.ToInt32(myDataRow["key"], CultureInfo.CurrentUICulture);
                    _allocationOverrideMethod.OTSOnHandPHL = Include.NoRID;
                    _allocationOverrideMethod.OTSOnHandPHLSeq = 0;
                    _allocationOverrideMethod.OnHandUnspecified = false; // TT#709 - RMatelic - Override Method - need option to not set Forecast Level
                    break;
                case eMerchandiseType.HierarchyLevel:
                    _allocationOverrideMethod.OTSOnHandRID = Include.NoRID;
                    _allocationOverrideMethod.OTSOnHandPHL = HP.Key;
                    _allocationOverrideMethod.OTSOnHandPHLSeq = Convert.ToInt32(myDataRow["key"], CultureInfo.CurrentUICulture);
                    _allocationOverrideMethod.OnHandUnspecified = false; // TT#709 - RMatelic - Override Method - need option to not set Forecast Level
                    break;
                case eMerchandiseType.OTSPlanLevel:
                    _allocationOverrideMethod.OTSOnHandRID = Include.NoRID;
                    _allocationOverrideMethod.OTSOnHandPHL = Include.NoRID;
                    _allocationOverrideMethod.OTSOnHandPHLSeq = 0;
                    _allocationOverrideMethod.OnHandUnspecified = false; // TT#709 - RMatelic - Override Method - need option to not set Forecast Level
                    break;
                // Begin TT#709 - RMatelic - Override Method - need option to not set Forecast Level
                case eMerchandiseType.Undefined:
                    _allocationOverrideMethod.OTSOnHandRID = Include.NoRID;
                    _allocationOverrideMethod.OTSOnHandPHL = Include.NoRID;
                    _allocationOverrideMethod.OTSOnHandPHLSeq = 0;
                    _allocationOverrideMethod.OnHandUnspecified = true; // TT#709 - RMatelic - Override Method - need option to not set Forecast Level
                    break;
                // End TT#709  
            }
            _allocationOverrideMethod.AllColorMultiple = Convert.ToInt32(txtColorMult.Text, CultureInfo.CurrentUICulture);
            _allocationOverrideMethod.AllSizeMultiple = Convert.ToInt32(txtSizeMult.Text, CultureInfo.CurrentUICulture);
            _allocationOverrideMethod.SG_RID = Convert.ToInt32(cboStoreAttribute.SelectedValue, CultureInfo.CurrentUICulture);
            // begin TT#488 - MD - Jellis - Group Allocation
            //_allocationOverrideMethod.sGstoreGroupRID = Convert.ToInt32(cboSGStoreAttribute.SelectedValue, CultureInfo.CurrentUICulture);   // TT#618 - AGallagher - Allocation Override - Add Attribute Sets (#35)
            //_allocationOverrideMethod.StoreGroupRID = Convert.ToInt32(cboStoreAttribute.SelectedValue, CultureInfo.CurrentUICulture);  // TT#1409 - Capacity Broken
            _allocationOverrideMethod.GradeStoreGroupRID = Convert.ToInt32(cboSGStoreAttribute.SelectedValue, CultureInfo.CurrentUICulture);   // TT#618 - AGallagher - Allocation Override - Add Attribute Sets (#35)
            _allocationOverrideMethod.CapacityStoreGroupRID = Convert.ToInt32(cboStoreAttribute.SelectedValue, CultureInfo.CurrentUICulture);  // TT#1409 - Capacity Broken
            // end TT#488 - MD - Jellis 
            _allocationOverrideMethod.ExceedCapacity = cbxExceedCapacity.Checked;
            if (this.ugAllColors.Rows[0].Cells["Maximum"].Text.Trim().Length > 0)
            {
                _allocationOverrideMethod.AllColorMaximum = Convert.ToInt32(this.ugAllColors.Rows[0].Cells["Maximum"].Value);
                _allocationOverrideMethod.UseAllColorsMaxDefault = false;
            }
            else
            {
                _allocationOverrideMethod.AllColorMaximum = _allColorsMaxValue;
                _allocationOverrideMethod.UseAllColorsMaxDefault = true;
            }
            if (this.ugAllColors.Rows[0].Cells["Minimum"].Text.Trim().Length > 0)
            {
                _allocationOverrideMethod.AllColorMinimum = Convert.ToInt32(this.ugAllColors.Rows[0].Cells["Minimum"].Value);
                _allocationOverrideMethod.UseAllColorsMinDefault = false;
            }
            else
            {
                _allocationOverrideMethod.AllColorMinimum = _allColorsMinValue;
                _allocationOverrideMethod.UseAllColorsMinDefault = true;
            }

            int filterRID;
            if (cboOverrideFilter.SelectedItem == null)
                _allocationOverrideMethod.StoreFilterRID = Include.UndefinedStoreFilter;
            else
            {
                filterRID = ((FilterNameCombo)cboOverrideFilter.SelectedItem).FilterRID;
                if (filterRID == Include.NoRID)
                    _allocationOverrideMethod.StoreFilterRID = Include.UndefinedStoreFilter;
                else
                    _allocationOverrideMethod.StoreFilterRID = ((FilterNameCombo)cboOverrideFilter.SelectedItem).FilterRID;
            }

            SetCapacity();
            _allocationOverrideMethod.DSOverRide = _dsOverRide;
            // BEGIN TT#1401 - GTaylor - Reservation Stores
            _allocationOverrideMethod.IMODataSet = _imoDataSet;
            // Begin TT#2731 - JSmith - Unable to copy allocation override method from global
            //_allocationOverrideMethod.ApplyVSW = _applyVSW;
            // End TT#2731 - JSmith - Unable to copy allocation override method from global
            //if (_applyVSW == true)
            //{
            //    _allocationOverrideMethod.IMODataSet = _imoDataSet;
            //}
            //else
            //{
            //    DataSet _tempIMO = _imoDataSet;
            //    _tempIMO.Tables["Stores"].Rows.Clear();
            //    _allocationOverrideMethod.IMODataSet = _tempIMO;
            //    _tempIMO.Dispose();
            //}
            // END TT#1401 - GTaylor - Reservation Stores
        }
	
		private void SetCapacity() 
		{
			foreach(DataRow dr in _capacityDataTable.Rows)
			{
				if ((bool)dr["Exceed Capacity"] == true)
					dr["ExceedChar"] = "1"; 
				else
					dr["ExceedChar"] = "0"; 
			 
			}
		}
		/// <summary>
		/// Use to validate the fields that are specific to this method type
		/// </summary>
		override protected bool ValidateSpecificFields()
		{	
			string inStr, errorMessage;
			int inInt = 0;
			bool methodFieldsValid = true;
			ErrorProvider.SetError (txtStoreGradeTime,string.Empty);
 			ErrorProvider.SetError (txtReserve,string.Empty);
			ErrorProvider.SetError (txtColorMult,string.Empty);
			ErrorProvider.SetError (txtSizeMult,string.Empty);
			ErrorProvider.SetError (txtOHFactor,string.Empty);
			ErrorProvider.SetError (rbReservePct,string.Empty);
			ErrorProvider.SetError (ugAllColors,string.Empty);
			ErrorProvider.SetError (ugColor,string.Empty);
			ErrorProvider.SetError (ugStoreGrades,string.Empty);
			this.toolTip1.SetToolTip(this.txtStoreGradeTime, _storeGradeDefaultText);

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

			if (txtStoreGradeTime.Text.Length > 0)
			{	try
				{
					inInt = Convert.ToInt32(txtStoreGradeTime.Text, CultureInfo.CurrentUICulture);
					if (inInt < 1)
					{	
						methodFieldsValid = false;
						errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MustBePositiveInteger);
						ErrorProvider.SetError (txtStoreGradeTime,errorMessage);
					}
					else if (inInt > Include.MaxStoreGradeWeeks)
					{
						methodFieldsValid = false;
						errorMessage = string.Format
							(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_StoreGradePeriodExceeded),Include.MaxStoreGradeWeeks.ToString(CultureInfo.CurrentUICulture));
						ErrorProvider.SetError (txtStoreGradeTime,errorMessage);
					} 
				}
				catch
				{
					methodFieldsValid = false;
					errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MustBeNumeric);
					ErrorProvider.SetError (txtStoreGradeTime,errorMessage);
				}
			}
			//else
			// 	txtStoreGradeTime.Text = "0";

			inStr = txtReserve.Text.ToString(CultureInfo.CurrentUICulture);
			double reserve = 0;		// TT#667 - Stodd - Pre-allocate Reserve

			if (inStr.Trim() != string.Empty)
			{
				// BEGIN TT#667 - Stodd - Pre-allocate Reserve
				try
				{
					reserve = double.Parse(txtReserve.Text);
				// End TT#667 - Stodd - Pre-allocate Reserve
					decimal outdec = Convert.ToDecimal(inStr, CultureInfo.CurrentUICulture);
					if (rbReservePct.Checked == true)
					{
						if (outdec > 100)
						{
							methodFieldsValid = false;
							ErrorProvider.SetError(txtReserve, SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MustBeBetween0And100));
						}
						else if (outdec < 0)
						{
							methodFieldsValid = false;
							errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MustBeNonNegative);
							ErrorProvider.SetError(txtReserve, errorMessage);
						}
					}
					else
					{
						if (rbReserveUnits.Checked == false)
						{
							methodFieldsValid = false;
							ErrorProvider.SetError(rbReservePct, SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_FieldIsRequired));
						}
						else if (outdec < 0)
						{
							methodFieldsValid = false;
							errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MustBeNonNegative);
							ErrorProvider.SetError(txtReserve, errorMessage);
						}
					}
				// BEGIN TT#667 - Stodd - Pre-allocate Reserve
				}
				catch
				{
					methodFieldsValid = false;
					ErrorProvider.SetError(txtReserve, SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MustBeNumeric));
				}
				// End TT#667 - Stodd - Pre-allocate Reserve
			}

			// BEGIN TT#667 - Stodd - Pre-allocate Reserve
			//=====================================
			// Reserve As Bulk & Reserve As Packs
			//=====================================
			double reserveAsBulk = 0;
			double reserveAsPacks = 0;
			bool reserveOk = true;
			try
			{
				if (txtReserveAsBulk.Text.Trim() != string.Empty)
				{
					reserveAsBulk = double.Parse(txtReserveAsBulk.Text);
				}
			}
			catch
			{
				reserveOk = false;
				methodFieldsValid = false;
				ErrorProvider.SetError(txtReserveAsBulk, SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MustBeNumeric));
			}
			try
			{
				if (txtReserveAsPacks.Text.Trim() != string.Empty)
				{

					reserveAsPacks = double.Parse(txtReserveAsPacks.Text);
				}
			}
			catch
			{
				reserveOk = false;
				methodFieldsValid = false;
				ErrorProvider.SetError(txtReserveAsPacks, SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MustBeNumeric));
			}

			if (reserveOk)
			{
				if (rbReservePct.Checked)
				{
					double totalPct = reserveAsBulk + reserveAsPacks;
					if (reserveAsBulk > 0 || reserveAsPacks > 0)
					{
						if (totalPct != 100)
						{
							methodFieldsValid = false;
							ErrorProvider.SetError(txtReserveAsBulk, SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MustEqual100));
							ErrorProvider.SetError(txtReserveAsPacks, SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MustEqual100));
						}
					}
				}
				if (rbReserveUnits.Checked)
				{
					double totalUnits = reserveAsBulk + reserveAsPacks;
					if (reserveAsBulk > 0 || reserveAsPacks > 0)
					{
						if (totalUnits != reserve)
						{
							methodFieldsValid = false;
							ErrorProvider.SetError(txtReserveAsBulk, SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_al_AsBulkAsPackNotEqualReserve));
							ErrorProvider.SetError(txtReserveAsPacks, SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_al_AsBulkAsPackNotEqualReserve));
						}
					}
					int iReserveAsBulk = (int)reserveAsBulk;
					if (iReserveAsBulk != reserveAsBulk)
					{
						methodFieldsValid = false;
						ErrorProvider.SetError(txtReserveAsBulk, SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_InvalidUnits));
					}
					int iReserveAsPacks = (int)reserveAsPacks;
					if (iReserveAsPacks != reserveAsPacks)
					{
						methodFieldsValid = false;
						ErrorProvider.SetError(txtReserveAsPacks, SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_InvalidUnits));
					}
				}
			}
			// End TT#667 - Stodd - Pre-allocate Reserve

			inStr = txtOHFactor.Text.ToString(CultureInfo.CurrentUICulture);
			if (inStr.Trim() != string.Empty)
			{
				decimal outdec = Convert.ToDecimal(inStr, CultureInfo.CurrentUICulture);
				if (outdec > 100)
				{
					methodFieldsValid = false;
					ErrorProvider.SetError (txtOHFactor,SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MustBeBetween0And100));
				}
				else if (outdec < 0)
				{
					methodFieldsValid = false;
					errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MustBeNonNegative);
					ErrorProvider.SetError (txtOHFactor,errorMessage);
				}
                // Begin TT#3146 - RMatelic - Allocation override mssg when updating the Basis information 80427. Would not expect to get the message
                //else	// BEGIN MID Track #5340 - On hand Factor not saving after question
                //{
                //    if (Convert.ToInt32(cboOnHand.SelectedValue, CultureInfo.CurrentUICulture) != _styleKey && _askQuestion)
                //    {
                //        errorMessage  = string.Format(MIDText.GetText(eMIDTextCode.msg_ChangeSelectionQuestion),
                //            this.lblOnHand.Text,_styleText);
                //        if (MessageBox.Show (errorMessage,  this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                //            == DialogResult.Yes) 
                //        {
                //            cboOnHand.SelectedValue = _styleKey;
                //        }
                //        _askQuestion = false;
                //    }
                //}		// END MID Track #5340
                else if (cboOnHand.Text.Trim() == string.Empty)  // require On-hand if there is a Factor %  
                {
                    methodFieldsValid = false;
                    errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_FieldIsRequired);
                    ErrorProvider.SetError(cboOnHand, errorMessage);
                }
                // End TT#3146 
			}
			if (txtColorMult.Text.Length > 0)
			{
				try
				{
					inInt = Convert.ToInt32(txtColorMult.Text, CultureInfo.CurrentUICulture);
					if (inInt < 1)
					{
						methodFieldsValid = false;
						errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MustBePositiveInteger);
						ErrorProvider.SetError (txtColorMult,errorMessage);
					}
				}
				catch
				{
					methodFieldsValid = false;
					errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MustBeNumeric);
					ErrorProvider.SetError (txtColorMult,errorMessage);
				}
			}
			else
				txtColorMult.Text = "1";

			if (txtSizeMult.Text.Length > 0)
			{
				try
				{
					inInt = Convert.ToInt32(txtSizeMult.Text, CultureInfo.CurrentUICulture);
					if (inInt < 1)
					{
						methodFieldsValid = false;
						errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MustBePositiveInteger);
						ErrorProvider.SetError (txtSizeMult,errorMessage);
					}
					else if (txtColorMult.Text.Length > 0)
					{
						int intColor = Convert.ToInt32(txtColorMult.Text, CultureInfo.CurrentUICulture);
						if (SqlInt32.Mod(intColor,inInt) > 0)
						{
							methodFieldsValid = false;
							errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_SizeMustDivideColorEvenly);
							ErrorProvider.SetError (txtSizeMult,errorMessage);
						}
					}
				}
				catch
				{
					methodFieldsValid = false;
					errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MustBeNumeric);
					ErrorProvider.SetError (txtSizeMult,errorMessage);
				}
			}
			else
				txtSizeMult.Text = "1";
			
			EditMsgs em = new EditMsgs();
			if (!ValidTabColor(ref em))
			{
				methodFieldsValid = false;
				this.tabConstraints.SelectedTab = this.tabColor;
			}

			if (methodFieldsValid) 
			{
				if (!ValidTabStoreGrades(ref em))
				{
					methodFieldsValid = false;
					this.tabConstraints.SelectedTab = this.tabStoreGrade;
				}
			}
			
			if (methodFieldsValid) 
			{
				if (!ValidTabCapacity(ref em))
				{
					methodFieldsValid = false;
					this.tabConstraints.SelectedTab = this.tabCapacity;
				}
			}
            // BEGIN TT#616 - AGallagher - Allocation - Pack Rounding (#67)
            if (methodFieldsValid)
            {
                if (!ValidTabPackRounding(ref em))
                {
                    methodFieldsValid = false;
                    this.tabConstraints.SelectedTab = this.tabPackRounding;
                }
            }
            // END TT#616 - AGallagher - Allocation - Pack Rounding (#67)
            // BEGIN TT#2083 - gtaylor
            if (methodFieldsValid)
            {
                if (!ValidTabReservation(ref em))
                {
                    methodFieldsValid = false;
                    this.tabConstraints.SelectedTab = this.tabReservation;
                }
            }
            // END TT#2083 - gtaylor
			return methodFieldsValid;
		}

		/// <summary>
		/// Use to set the errors to the screen
		/// </summary>
		override protected void HandleErrors()
		{
			ErrorProvider.SetError (txtName,string.Empty);
			ErrorProvider.SetError (txtDesc,string.Empty);
			ErrorProvider.SetError (pnlGlobalUser,string.Empty);
			if (!WorkflowMethodNameValid)
			{
				ErrorProvider.SetError (txtName,WorkflowMethodNameMessage);
			}
			if (!WorkflowMethodDescriptionValid)
			{
				ErrorProvider.SetError (txtDesc,WorkflowMethodDescriptionMessage);
			}
			if (!UserGlobalValid)
			{
				ErrorProvider.SetError (pnlGlobalUser,UserGlobalMessage);
			}
		}

		/// <summary>
		/// Use to set the specific method object before updating
		/// </summary>
		override protected void SetObject()
		{
			try
			{
				ABM = _allocationOverrideMethod;
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
		#region Combo Boxes Drag and Drop 
        private void cboOnHand_DragEnter(object sender, System.Windows.Forms.DragEventArgs e)
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
            //    HandleException(ex);
            //}
            // End TT#296-MD - JSmith - Methods and Workflows can be dragged and dropped into Inventory Basis drop down.
        }

        private void cboOnHand_DragOver(object sender, DragEventArgs e)
        {
            // Begin TT#296-MD - JSmith - Methods and Workflows can be dragged and dropped into Inventory Basis drop down.
            Merchandise_DragOver(sender, e);
            //Image_DragOver(sender, e);
            // End TT#296-MD - JSmith - Methods and Workflows can be dragged and dropped into Inventory Basis drop down.
        }

		private void cboOTSPlan_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
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
					_lastMerchIndex = cboOTSPlan.SelectedIndex;
				}
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}

		private void cboOTSPlan_Validating(object sender, System.ComponentModel.CancelEventArgs e)
		{
			string errorMessage;

			try
			{
				if (cboOTSPlan.Text == string.Empty)
				{
					cboOTSPlan.SelectedIndex = _lastMerchIndex;
					_priorError = false;
				}
				else
				{
					if (_textChanged)
					{
						_textChanged = false;

						HierarchyNodeProfile hnp = GetNodeProfile(cboOTSPlan.Text);
						if (hnp.Key == Include.NoRID)
						{
							_priorError = true;

							errorMessage = string.Format(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_InvalidTreeNode), cboOTSPlan.Text);
							ErrorProvider.SetError(cboOTSPlan, errorMessage);
							MessageBox.Show(errorMessage);

							e.Cancel = true;
						}
						else 
						{
							AddNodeToOTSPlanCombo(hnp);
							_priorError = false;
						}	
					}
					// JBolles - MID Track #5020 - Prevent errored textbox from resetting its value before error is corrected
//					else if (_priorError)
//					{
//						cboOTSPlan.SelectedIndex = _lastMerchIndex;
//					}
				}
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}

		private void cboOTSPlan_Validated(object sender, System.EventArgs e)
		{
			try
			{
				// JBolles - MID Track #5020 - Prevent errored textbox from resetting its value before error is corrected
				if(!_priorError)
				{
					ErrorProvider.SetError(cboOTSPlan, string.Empty);
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

        private void cboOTSPlan_DragEnter(object sender, System.Windows.Forms.DragEventArgs e)
        {
            // Begin TT#296 - JSmith - Methods and Workflows can be dragged and dropped into Inventory Basis drop down.
            Merchandise_DragEnter(sender, e);
            //try
            //{
            //    Image_DragEnter(sender, e);
            //    ObjectDragEnter(e);
            //}
            //catch (Exception ex)
            //{
            //    HandleException(ex);
            //}
            // End TT#296 - JSmith - Methods and Workflows can be dragged and dropped into Inventory Basis drop down.
        }

        private void cboOTSPlan_DragOver(object sender, DragEventArgs e)
        {
            // Begin TT#296 - JSmith - Methods and Workflows can be dragged and dropped into Inventory Basis drop down.
            Merchandise_DragOver(sender, e);
            //Image_DragOver(sender, e);
            // End TT#296 - JSmith - Methods and Workflows can be dragged and dropped into Inventory Basis drop down.
        }

		private void cboOTSPlan_DragDrop(object sender, System.Windows.Forms.DragEventArgs e)
		{
            TreeNodeClipboardList cbList = null;
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
                    AddNodeToOTSPlanCombo(hnp);
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
		private void  AddNodeToOTSPlanCombo (HierarchyNodeProfile hnp )
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

					cboOTSPlan.SelectedIndex = MerchandiseDataTable.Rows.Count - 1;
				}
				else
				{
					cboOTSPlan.SelectedIndex = levIndex;
				}
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}

		private void cboOnHand_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
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
					_lastMerchIndex = cboOnHand.SelectedIndex;
				}
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}

		private void cboOnHand_Validating(object sender, System.ComponentModel.CancelEventArgs e)
		{
			string errorMessage;

			try
			{
				if (cboOnHand.Text == string.Empty)
				{
					cboOnHand.SelectedIndex = _lastMerchIndex;
					_priorError = false;
				}
				else
				{
					if (_textChanged)
					{
						_textChanged = false;

						HierarchyNodeProfile hnp = GetNodeProfile(cboOnHand.Text);
						if (hnp.Key == Include.NoRID)
						{
							_priorError = true;

							errorMessage = string.Format(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_InvalidTreeNode), cboOnHand.Text);
							ErrorProvider.SetError(cboOnHand, errorMessage);
							MessageBox.Show(errorMessage);

							e.Cancel = true;
						}
						else 
						{
							AddNodeToOnHandCombo(hnp);
							_priorError = false;
						}	
					}
					// JBolles - MID Track #5020 - Prevent errored textbox from resetting its value before error is corrected
//					else if (_priorError)
//					{
//						cboOnHand.SelectedIndex = _lastMerchIndex;
//					}
				}
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}
		
		private void cboOnHand_Validated(object sender, System.EventArgs e)
		{
			try
			{
				// JBolles - MID Track #5020 - Prevent errored textbox from resetting its value before error is corrected
				if(!_priorError)
				{
					ErrorProvider.SetError(cboOnHand, string.Empty);
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

		private void cboOnHand_DragDrop(object sender, System.Windows.Forms.DragEventArgs e)
        {
            TreeNodeClipboardList cbList = null;
            HierarchyNodeClipboardList hnList = null;
            HierarchyNodeProfile hnp;
			try
			{
                //ClipboardProfile cbp = GetClipboardData(eClipboardDataType.HierarchyNode);
                if (e.Data.GetDataPresent(typeof(TreeNodeClipboardList)))
                {
                    cbList = (TreeNodeClipboardList)e.Data.GetData(typeof(TreeNodeClipboardList));
                    //Begin Track #5378 - color and size not qualified
                    //				HierarchyNodeProfile hnp = SAB.HierarchyServerSession.GetNodeData(cbp.Key);
                    hnp = SAB.HierarchyServerSession.GetNodeData(cbList.ClipboardProfile.Key, true, true);
                    //End Track #5378
                    AddNodeToOnHandCombo(hnp);
                }
                else if (e.Data.GetDataPresent(typeof(HierarchyNodeClipboardList)))
                {
                    hnList = (HierarchyNodeClipboardList)e.Data.GetData(typeof(HierarchyNodeClipboardList));
                    //Begin Track #5378 - color and size not qualified
                    //				HierarchyNodeProfile hnp = SAB.HierarchyServerSession.GetNodeData(cbp.Key);
                    hnp = SAB.HierarchyServerSession.GetNodeData(hnList.ClipboardProfile.Key, true, true);
                    //End Track #5378
                    AddNodeToOnHandCombo(hnp);
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

        // BEGIN TT#618 - AGallagher - Allocation Override - Add Attribute Sets (#35)
        private void cboSGStoreAttribute_DragEnter(object sender, DragEventArgs e)
        {
            Image_DragEnter(sender, e);
        }

        private void cboSGStoreAttribute_DragOver(object sender, DragEventArgs e)
        {
            Image_DragOver(sender, e);
        }
        
        // Begin TT#1505 - RMatelic - Users Can Not View Global Variables >> change combo box prefix to 'cmb' to keep enabled when read only
        private void cmbSGAttributeSet_SelectionChangeCommitted(object sender, System.EventArgs e)
        // End TT#1505
        {
            try
			{				
				// Begin TT#618 - STodd - Allocation Override - Add Attribute Sets (#35)
				//if (this.cboSGAttributeSet.SelectedValue != null)
				//{
				//    PopulateSGStoreAttributeSet(this.cboSGAttributeSet.SelectedValue.ToString());
				//}
                // Begin TT#939 - JSmith - Allocation Override -> will not clear fields in grid on store grade tab, change attribute set and get System Object
                //if (FormLoaded)
				if (FormLoaded &&
                    !_SGStoreAttributeChanged)
                // End TT#939
                {   // Begin TT#939 - RMatelic - Allocation Override -> will not clear fields in grid on store grade tab, change attribute set and get System Object
                    if (_setReset)
                    {
                        _setReset = false;
                        return;
                    }
					if (_newGradeAdded)
					{
						AdjustSetForNewGrades((int)this.cmbSGAttributeSet.SelectedValue);
						_newGradeAdded = false;
					}
                    EditMsgs em = new EditMsgs();
                    if (!ValidTabStoreGrades(ref em))
                    {
                        this.tabConstraints.SelectedTab = this.tabStoreGrade;
                        if (em.EditMessages.Count > 0)
                        {
                            _errors = null;
                            for (int i = 0; i < em.EditMessages.Count; i++)
                            {
                                EditMsgs.Message emm = (EditMsgs.Message)em.EditMessages[i];
                                AddErrorMessage(emm);
                            }
                            MessageBox.Show(_errors, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            _setReset = true;
                            cmbSGAttributeSet.SelectedValue = _curAttributeSet;
                        }
                    }
                    else
                    {
                        _curAttributeSet = Convert.ToInt32(cmbSGAttributeSet.SelectedValue, CultureInfo.CurrentUICulture);
                        _dsOverRide.Tables["StoreGrades"].DefaultView.RowFilter = "SGLRID = " + this.cmbSGAttributeSet.SelectedValue.ToString();
                    }
                }   // End TT#939  
				// End TT#618 - STodd - Allocation Override - Add Attribute Sets (#35)
            }
            catch (Exception exc)
            { HandleException(exc); }
        }
        // END TT#618 - AGallagher - Allocation Override - Add Attribute Sets (#35)

		// Begin TT#618 - STodd - Allocation Override - Add Attribute Sets (#35)
		private void AdjustSetForNewGrades(int sglRid)
		{
            // Begin TT#1162 - JSmith - Duplicating Store Grade Error
            string rowFilter = _dsOverRide.Tables["StoreGrades"].DefaultView.RowFilter;
            // End TT#1162

			_dsOverRide.Tables["StoreGrades"].DefaultView.RowFilter = null;
			Hashtable gradeHash = new Hashtable();
			ArrayList setList = new ArrayList();
			ArrayList newRowList = new ArrayList();
			DataTable dt = _dsOverRide.Tables["StoreGrades"];
			int boundary = -1;
			int prevBoundary = -1;
			int set = 0;
			string grade = string.Empty;
			int rowPos = ugStoreGrades.Rows.Count;
			//========================================
			// Get unique boundary hash and set list
			//========================================
			foreach (DataRow row in dt.Rows)
			{
				prevBoundary = boundary;
				boundary = Convert.ToInt32(row["Boundary"]);
				grade = row["Grade"].ToString();
				set = Convert.ToInt32(row["SGLRID"]);

				if (!setList.Contains(set))
				{
					setList.Add(set);
				}

				if (!gradeHash.ContainsKey(boundary))
				{
					gradeHash.Add(boundary, grade);
				}
			}

			//========================================================================
			// Foreach set we see if it is missing any of the newly added grades.
			// if so, we build a new row and add it to an arrayList.
			//========================================================================
			foreach (int setRid in setList)
			{
				IDictionaryEnumerator en = gradeHash.GetEnumerator();
				DataRow[] sRows = dt.Select("SGLRID = " + setRid.ToString());

				while (en.MoveNext())
				{
					bool match = false;
					int gBoundary = Convert.ToInt32(en.Key);
					string gGrade = en.Value.ToString();
					foreach (DataRow sRow in sRows)
					{
						boundary = Convert.ToInt32(sRow["Boundary"]);
						if (gBoundary == boundary)
						{
							match = true;
							break;
						}
					}

					if (!match)
					{
						DataRow newRow = dt.NewRow();
						newRow["Boundary"] = gBoundary;
						newRow["SGLRID"] = setRid;
						newRow["Grade"] = gGrade;
						newRow["RowPosition"] = rowPos++;
						newRowList.Add(newRow);
					}
				}
			}
			// End TT#618 - STodd - Allocation Override - Add Attribute Sets (#35)

			//==============================================
			// Add all the new grade rows to the datatable
			//==============================================
			foreach (DataRow aRow in newRowList)
			{
				dt.Rows.Add(aRow);
			}
			dt.AcceptChanges();

			//===============================================================
			// Now that the news rows have been added, they are positionally 
			// in the wrong place in the grid.
			// The rest of this mess is to get those new rows in the other
			// sets in the right position.
			//===============================================================
			// Sort the grid like we want it.
			this.ugStoreGrades.DisplayLayout.Bands[0].SortedColumns.Clear();
			this.ugStoreGrades.DisplayLayout.Bands[0].SortedColumns.Add("SGLRID", true);
			this.ugStoreGrades.DisplayLayout.Bands[0].SortedColumns.Add("Boundary", true); 	
			ugStoreGrades.DisplayLayout.Bands[0].SortedColumns.RefreshSort(true);
			// Update thr row position to be correct
			rowPos = 0;
			if (ugStoreGrades.Rows.Count > 0)
			{
				foreach (UltraGridRow gridRow in ugStoreGrades.Rows)
				{
					gridRow.Cells["RowPosition"].Value = rowPos++;
				}
			}

            // Begin TT#1162 - JSmith - Duplicating Store Grade Error
            _dsOverRide.Tables["StoreGrades"].DefaultView.RowFilter = rowFilter;
            // End TT#1162
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

		private void  AddNodeToOnHandCombo (HierarchyNodeProfile hnp )
		{
			try
			{
				DataRow myDataRow;
				bool nodeFound = false;
				int nodeRID = Include.NoRID;
				int levIndex;
				for (levIndex = 0;
					levIndex < _merchDataTable2.Rows.Count; levIndex++)
				{	
					myDataRow = _merchDataTable2.Rows[levIndex];
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
					myDataRow = _merchDataTable2.NewRow();
					myDataRow["seqno"] = _merchDataTable2.Rows.Count;
					myDataRow["leveltypename"] = eMerchandiseType.Node;
					myDataRow["text"] = hnp.Text;	
					myDataRow["key"] = hnp.Key;
					_merchDataTable2.Rows.Add(myDataRow);

					cboOnHand.SelectedIndex = _merchDataTable2.Rows.Count - 1;
				}
				else
				{
					cboOnHand.SelectedIndex = levIndex;
				}
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}

		private void SetOTSComboToLevel(int seq)
		{
			try
			{
                // Begin TT#709 - RMatelic - Override Method - need option to not set Forecast Level
                //DataRow myDataRow;
                //for (int levIndex = 0;
                //    levIndex < MerchandiseDataTable.Rows.Count; levIndex++)
                //{	
                //    myDataRow = MerchandiseDataTable.Rows[levIndex];
                //    if (Convert.ToInt32(myDataRow["seqno"], CultureInfo.CurrentUICulture) == seq)
                //    {
                //        cboOTSPlan.SelectedIndex = levIndex;
                //        break;
                //    }
                //}
                cboOTSPlan.SelectedValue = seq;
                // End TT#709 
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}

		}
		
		private void SetOnHandComboToLevel(int seq)
		{
			try
			{
                // Begin TT#709 - RMatelic - Override Method - need option to not set Forecast Level
                //DataRow myDataRow;
                //for (int levIndex = 0;
                //    levIndex < _merchDataTable2.Rows.Count; levIndex++)
                //{	
                //    myDataRow = _merchDataTable2.Rows[levIndex];
                //    if (Convert.ToInt32(myDataRow["seqno"], CultureInfo.CurrentUICulture) == seq)
                //    {
                //        cboOnHand.SelectedIndex = levIndex;
                //        break;
                //    }
                //}
                cboOnHand.SelectedValue = seq;
                // End TT#709
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}

		}
		
		#endregion Combo Boxes Drag and Drop 
        
        // Begin TT#1401 - Reservation Stores
        private void mnuIMOGrid_Popup(object sender, System.EventArgs e)
        {
            try
            {
                mnuIMOGrid.MenuItems.Clear();
                MenuItem mnuItemApplyAll = new MenuItem(MIDText.GetTextOnly(eMIDTextCode.lbl_ApplyAll));
                MenuItem mnuItemApplyColumn = new MenuItem(MIDText.GetTextOnly(eMIDTextCode.lbl_ApplyColumn));
                MenuItem mnuItemExpandAll = new MenuItem(MIDText.GetTextOnly(eMIDTextCode.lbl_ExpandAll));
                MenuItem mnuItemCollapseAll = new MenuItem(MIDText.GetTextOnly(eMIDTextCode.lbl_CollapseAll));
                MenuItem mnuItemClearAll = new MenuItem(MIDText.GetTextOnly(eMIDTextCode.lbl_ClearAll));
                MenuItem mnuItemClearColumn = new MenuItem(MIDText.GetTextOnly(eMIDTextCode.lbl_ClearColumn));
                MenuItem mnuItemReset = new MenuItem(MIDText.GetTextOnly(eMIDTextCode.lbl_Reset_Messages));

                if (this.ugReservation.ActiveRow == null ||
                    this.ugReservation.ActiveRow.Band.Key != "Sets")
                {
                    mnuItemApplyAll.Enabled = false;
                    mnuItemApplyColumn.Enabled = false;
                }
                else
                {
                    mnuItemApplyAll.Enabled = true;
                    mnuItemApplyColumn.Enabled = true;
                }

                if (!FunctionSecurity.AllowUpdate)
                {
                    mnuItemApplyAll.Enabled = false;
                    mnuItemApplyColumn.Enabled = false;
                    mnuItemClearAll.Enabled = false;
                    mnuItemClearColumn.Enabled = false;
                }

                mnuIMOGrid.MenuItems.Add(mnuItemApplyAll);
                mnuIMOGrid.MenuItems.Add(mnuItemApplyColumn);
                mnuIMOGrid.MenuItems.Add(mnuItemClearAll);
                mnuIMOGrid.MenuItems.Add(mnuItemClearColumn);
                mnuIMOGrid.MenuItems.Add("-");
                mnuIMOGrid.MenuItems.Add(mnuItemExpandAll);
                mnuIMOGrid.MenuItems.Add(mnuItemCollapseAll);
                mnuItemApplyAll.Click += new System.EventHandler(this.mnuIMOItemApplyAll_Click);
                mnuItemApplyColumn.Click += new System.EventHandler(mnuIMOItemApplyColumn_Click);
                mnuItemExpandAll.Click += new System.EventHandler(this.mnuIMOItemExpandAll_Click);
                mnuItemCollapseAll.Click += new System.EventHandler(this.mnuIMOItemCollapseAll_Click);
                mnuItemClearAll.Click += new System.EventHandler(this.mnuIMOItemClearAll_Click);
                mnuItemClearColumn.Click += new System.EventHandler(this.mnuIMOItemClearColumn_Click);
                mnuItemReset.Click += new System.EventHandler(this.mnuIMOItemReset_Click);
            }
            catch (Exception exception)
            {
                HandleException(exception);
            }
        }
        private void mnuIMOItemClearColumn_Click(object sender, System.EventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                bool updateMinShipQty = false;
                bool updatePctPackThreshold = false;
                bool updateItemMax = false;
                if (this.ugReservation.ActiveCell != null)
                {
                    switch (this.ugReservation.ActiveCell.Column.Key)
                    {
                        case "Min Ship Qty":
                            updateMinShipQty = true;
                            break;
                        case "Pct Pack Threshold":
                            updatePctPackThreshold = true;
                            break;
                        case "Item Max":
                            updateItemMax = true;
                            break;
                        default:
                            string text = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ColumnCanNotBeApplied);
                            text = text.Replace("{0}", this.ugReservation.ActiveCell.Column.Key);
                            MessageBox.Show(text, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            break;
                    }
                    IMOItemClear(updateMinShipQty, updatePctPackThreshold, updateItemMax);
                }
            }
            catch (Exception exception)
            {
                HandleException(exception);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }
        private void mnuIMOItemClearAll_Click(object sender, System.EventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                IMOItemClear(true, true, true);
            }
            catch (Exception exception)
            {
                HandleException(exception);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }
        private void mnuIMOItemApplyAll_Click(object sender, System.EventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                IMOItemApply(true, true, true);
            }
            catch (Exception exception)
            {
                HandleException(exception);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }
        private void mnuIMOItemApplyColumn_Click(object sender, System.EventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                bool updateMinShipQty = false;
                bool updatePctPackThreshold = false;
                bool updateItemMax = false;
                if (this.ugReservation.ActiveCell != null)
                {
                    switch (this.ugReservation.ActiveCell.Column.Key)
                    {
                        case "Min Ship Qty":
                            updateMinShipQty = true;
                            break;
                        case "Pct Pack Threshold":
                            updatePctPackThreshold = true;
                            break;
                        case "Item Max":
                            updateItemMax = true;
                            break;
                        default:
                            string text = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ColumnCanNotBeApplied);
                            text = text.Replace("{0}", this.ugReservation.ActiveCell.Column.Key);
                            MessageBox.Show(text, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            break;
                    }
                    IMOItemApply(updateMinShipQty, updatePctPackThreshold, updateItemMax);
                }
            }
            catch (Exception exception)
            {
                HandleException(exception);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }
        private void mnuIMOItemReset_Click(object sender, System.EventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                Infragistics.Win.UltraWinGrid.UltraGridBand activeRowBand = this.ugReservation.ActiveRow.Band;
                Infragistics.Win.UltraWinGrid.UltraGridRow activeRow = this.ugReservation.ActiveRow;
                Infragistics.Win.UltraWinGrid.UltraGridChildBand ultraGridStoreBand;
                if (activeRowBand.Key == "Sets")		// apply values to all stores in set
                {
                    ultraGridStoreBand = activeRow.ChildBands["Stores"];	// get "Stores" band
                    foreach (UltraGridRow storeRow in ultraGridStoreBand.Rows)
                    {
                        storeRow.Cells["Store RID"].Tag = null;
                        storeRow.Cells["Store RID"].Appearance.Image = null;
                        storeRow.Cells["Store ID"].Tag = null;
                        storeRow.Cells["Store ID"].Appearance.Image = null;
                        storeRow.Cells["Reservation Store"].Tag = null;
                        storeRow.Cells["Reservation Store"].Appearance.Image = null;
                        storeRow.Cells["Min Ship Qty"].Tag = null;
                        storeRow.Cells["Min Ship Qty"].Appearance.Image = null;
                        storeRow.Cells["Pct Pack Threshold"].Tag = null;
                        storeRow.Cells["Pct Pack Threshold"].Appearance.Image = null;
                        storeRow.Cells["Item Max"].Tag = null;
                        storeRow.Cells["Item Max"].Appearance.Image = null;
                    }
                }
                else
                    if (activeRowBand.Key == "Stores")		// clear store value
                    {
                        activeRow.Cells["Store RID"].Tag = null;
                        activeRow.Cells["Store RID"].Appearance.Image = null;
                        activeRow.Cells["Store ID"].Tag = null;
                        activeRow.Cells["Store ID"].Appearance.Image = null;
                        activeRow.Cells["Reservation Store"].Tag = null;
                        activeRow.Cells["Reservation Store"].Appearance.Image = null;
                        activeRow.Cells["Min Ship Qty"].Tag = null;
                        activeRow.Cells["Min Ship Qty"].Appearance.Image = null;
                        activeRow.Cells["Pct Pack Threshold"].Tag = null;
                        activeRow.Cells["Pct Pack Threshold"].Appearance.Image = null;
                        activeRow.Cells["Item Max"].Tag = null;
                        activeRow.Cells["Item Max"].Appearance.Image = null;
                    }
            }
            catch (Exception exception)
            {
                HandleException(exception);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }
        private void mnuIMOItemCollapseAll_Click(object sender, System.EventArgs e)
        {
            try
            {
                this.ugReservation.Rows.CollapseAll(true);
            }
            catch (Exception exception)
            {
                HandleException(exception);
            }
        }
        private void mnuIMOItemExpandAll_Click(object sender, System.EventArgs e)
        {
            try
            {
                this.ugReservation.Rows.ExpandAll(true);
            }
            catch (Exception exception)
            {
                HandleException(exception);
            }
        }
        private void IMOItemClear(bool updateMinShipQty, bool updatePctPackThreshold, bool updateItemMax)
        {
            try
            {
                Infragistics.Win.UltraWinGrid.UltraGridBand activeRowBand = this.ugReservation.ActiveRow.Band;
                Infragistics.Win.UltraWinGrid.UltraGridRow activeRow = this.ugReservation.ActiveRow;
                Infragistics.Win.UltraWinGrid.UltraGridChildBand ultraGridStoreBand;
                if (activeRowBand.Key == "Sets")		// apply values to all stores in set
                {
                    ultraGridStoreBand = activeRow.ChildBands["Stores"];	// get "Stores" band
                    foreach (UltraGridRow storeRow in ultraGridStoreBand.Rows)
                    {
                        if (storeRow.Cells["Reservation Store"].Value.ToString().Trim().Length > 0)
                        {
                            storeRow.Cells["Updated"].Value = true;
                            if (updateMinShipQty)
                            {
                                storeRow.Cells["Min Ship Qty"].Value = string.Empty;
                                storeRow.Cells["Min Ship Qty"].Appearance.Image = null;
                                storeRow.Cells["Min Ship Qty"].Tag = null;
                            }
                            if (updatePctPackThreshold)
                            {
                                storeRow.Cells["Pct Pack Threshold"].Value = string.Empty;
                                storeRow.Cells["Pct Pack Threshold"].Appearance.Image = null;
                                storeRow.Cells["Pct Pack Threshold"].Tag = null;
                            }
                            if (updateItemMax)
                            {
                                storeRow.Cells["Item Max"].Value = string.Empty;
                                storeRow.Cells["Item Max"].Appearance.Image = null;
                                storeRow.Cells["Item Max"].Tag = null;
                            }
                            IMOChangesMade = true;
                        }
                    }

                }

                if (activeRowBand.Key == "Stores")		// clear store value
                {
                    activeRow.Cells["Updated"].Value = true;
                    IMOChangesMade = true;
                }
                if (updateMinShipQty)
                {
                    activeRow.Cells["Min Ship Qty"].Value = string.Empty;
                    activeRow.Cells["Min Ship Qty"].Appearance.Image = null;
                    activeRow.Cells["Min Ship Qty"].Tag = null;
                }
                if (updatePctPackThreshold)
                {
                    activeRow.Cells["Pct Pack Threshold"].Value = string.Empty;
                    activeRow.Cells["Pct Pack Threshold"].Appearance.Image = null;
                    activeRow.Cells["Pct Pack Threshold"].Tag = null;
                }
                if (updateItemMax)
                {
                    activeRow.Cells["Item Max"].Value = string.Empty;
                    activeRow.Cells["Item Max"].Appearance.Image = null;
                    activeRow.Cells["Item Max"].Tag = null;
                }
            }
            catch (Exception exception)
            {
                HandleException(exception);
            }
        }
        private void IMOItemApply(bool updateMinShipQty, bool updatePctPackThreshold, bool updateItemMax)
        {
            string errorMessage = null;
            try
            {
                if (this.ugReservation.ActiveRow == null)
                {
                    this.Cursor = Cursors.Default;
                    MessageBox.Show(errorMessage, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    if (ugReservation.ActiveCell != null)
                    {
                        ugReservation.ActiveCell.Row.Update();
                    }

                    Infragistics.Win.UltraWinGrid.UltraGridBand activeRowBand = this.ugReservation.ActiveRow.Band;
                    Infragistics.Win.UltraWinGrid.UltraGridRow activeRow = this.ugReservation.ActiveRow;

                    Infragistics.Win.UltraWinGrid.UltraGridChildBand ultraGridStoreBand;
                    if (activeRowBand.Key == "Sets")		// apply values to all stores in set
                    {
                        ultraGridStoreBand = activeRow.ChildBands["Stores"];	// get "Stores" band
                        foreach (UltraGridRow storeRow in ultraGridStoreBand.Rows)
                        {
                            if (storeRow.Cells["Reservation Store"].Value.ToString().Trim().Length > 0)
                            {
                                storeRow.Cells["Updated"].Value = true;
                                if (updateMinShipQty)
                                {
                                    storeRow.Cells["Min Ship Qty"].Value = activeRow.Cells["Min Ship Qty"].Value;
                                    storeRow.Cells["Min Ship Qty"].Appearance.Image = null;
                                    storeRow.Cells["Min Ship Qty"].Tag = null;
                                }
                                if (updatePctPackThreshold)
                                {
                                    storeRow.Cells["Pct Pack Threshold"].Value = activeRow.Cells["Pct Pack Threshold"].Value;
                                    storeRow.Cells["Pct Pack Threshold"].Appearance.Image = null;
                                    storeRow.Cells["Pct Pack Threshold"].Tag = null;
                                }
                                if (updateItemMax)
                                {
                                    storeRow.Cells["Item Max"].Value = activeRow.Cells["Item Max"].Value;
                                    storeRow.Cells["Item Max"].Appearance.Image = null;
                                    storeRow.Cells["Item Max"].Tag = null;
                                }
                            }
                        }
                        _imoDataSet.AcceptChanges();
                        IMOChangesMade = true;
                    }
                }
            }
            catch (Exception exception)
            {
                HandleException(exception);
            }
        }

        private void ReservationTab_Load()
        {
            try
            {
                Reservation_Attributes_Populate();
                Reservation_Populate(Include.NoRID);

                this.ugReservation.ContextMenu = mnuIMOGrid;

                ugReservation.ActiveRow = ugReservation.GetRow(Infragistics.Win.UltraWinGrid.ChildRow.First);
                _imoIsPopulated = true;
            }
            catch (Exception exception)
            {
                HandleException(exception);
            }
        }

        private void Reservation_Populate(int nodeRID)
        {
            Reservation_Populate(nodeRID, false);
        }

        private void Reservation_Populate(int nodeRID, bool aAttributeChanged)
        {
            try
            {
                // Begin TT#2731 - JSmith - Unable to copy allocation override method from global
                //Reservation_Define();
                //ugReservation.DataSource = _imoDataSet;

                //IMOProfile imop;
                //IMOMethodOverrideProfile imomop;

                //if (!aAttributeChanged)
                //{
                //    _imoMethodOverrideProfileList = _allocationOverrideMethod.GetMethodOverrideIMO();

                //    _applyVSW = _allocationOverrideMethod.GetApplyVSW();


                //    if (nodeRID != Include.NoRID || _imoGroupLevelList == null)
                //    {
                //        //  if the list is empty, repopulate it
                //        ProfileList storeList = SAB.StoreServerSession.GetAllStoresList();
                //        _imoProfileList = SAB.HierarchyServerSession.GetNodeIMOList(storeList, nodeRID);
                //    }
                //}

                _allocationOverrideMethod.Reservation_Populate(nodeRID, aAttributeChanged);
                _imoDataSet = _allocationOverrideMethod.IMODataSet;

                //foreach (StoreGroupLevelListViewProfile sglp in _imoGroupLevelList)
                //{
                //    _imoDataSet.Tables["Sets"].Rows.Add(new object[] { sglp.Name, string.Empty, string.Empty, string.Empty, string.Empty });

                //    foreach (StoreProfile storeProfile in sglp.Stores)
                //    {
                //        if ((_imoProfileList != null) && (_imoProfileList.Contains(storeProfile.Key)))
                //        {
                //            //  the profile exists
                //            imop = (IMOProfile)_imoProfileList.FindKey(storeProfile.Key);
                //            imop.IMOIsDefault = false;
                //        }
                //        else
                //        {
                //            //  create a new profile with existing data
                //            //      if so, get it
                //            //      if not, make it up
                //            imop = new IMOProfile(storeProfile.Key);
                //            imop.IMOStoreRID = storeProfile.Key;
                //            imop.IMOMinShipQty = 0;
                //            imop.IMOPackQty = Include.PercentPackThresholdDefault;
                //            imop.IMOMaxValue = int.MaxValue;
                //            imop.IMOIsDefault = true;
                //        }

                //        //  is this store key in the method override data?
                //        if ((_imoMethodOverrideProfileList.Contains(storeProfile.Key)) && (nodeRID == Include.NoRID))
                //        {
                //            //  blend it
                //            imomop = (IMOMethodOverrideProfile)_imoMethodOverrideProfileList.FindKey(storeProfile.Key);

                //            imop.IMOMaxValue = ((imomop.IMOMaxValue != Include.NoRID && imomop.IMOMaxValue != int.MaxValue) ? imomop.IMOMaxValue : imop.IMOMaxValue);
                //            imop.IMOMinShipQty = ((imomop.IMOMinShipQty != Include.NoRID && imomop.IMOMinShipQty != 0) ? imomop.IMOMinShipQty : imop.IMOMinShipQty);
                //            imop.IMOPackQty = ((imomop.IMOPackQty != Include.NoRID && imomop.IMOPackQty != Include.PercentPackThresholdDefault) ? imomop.IMOPackQty : imop.IMOPackQty);
                //            imop.IMOIsDefault = false;

                //            //cbxRsrvDoNotApplyVSW.Checked = !imomop.IMO_Apply_VSW;
                //        }

                //        _imoDataSet.Tables["Stores"].Rows.Add(new object[] {sglp.Name, 
                //            ((nodeRID == Include.NoRID) ? false : true),  // if this is the initial load, the nodeRid will equal include.norid
                //            imop.IMOStoreRID, 
                //            storeProfile.Text,
                //            (storeProfile.IMO_ID ?? String.Empty),
                //            (((imop.IMOIsDefault == true) || (imop.IMOMinShipQty == 0)) ? String.Empty : imop.IMOMinShipQty.ToString()),
                //            (((imop.IMOIsDefault == true) || (imop.IMOPackQty == Include.PercentPackThresholdDefault)) ? String.Empty : (imop.IMOPackQty*100).ToString()),    
                //            (((imop.IMOIsDefault == true) || (imop.IMOMaxValue == int.MaxValue)) ? String.Empty : imop.IMOMaxValue.ToString())
                //        });

                //    }
                //}
                // End TT#2731 - JSmith - Unable to copy allocation override method from global

                // set the DO NOT APPLY VSW checkbox
                // Begin TT#2731 - JSmith - Unable to copy allocation override method from global
                //cbxRsrvDoNotApplyVSW.Checked = !_applyVSW;
                cbxRsrvDoNotApplyVSW.Checked = !_allocationOverrideMethod.ApplyVSW;
                // End TT#2731 - JSmith - Unable to copy allocation override method from global

                ugReservation.DataSource = _imoDataSet;
                ugReservation.ActiveRow = ugReservation.GetRow(Infragistics.Win.UltraWinGrid.ChildRow.First);
                FunctionSecurityProfile securityLevel = FunctionSecurity;
                SetControlReadOnly(this.tabReservation, !securityLevel.AllowUpdate);
                ugReservation.Rows.ExpandAll(true);
            }
            catch (Exception exception)
            {
                HandleException(exception);
            }
        }

        private void Reservation_Attributes_Populate()
        {
            //  use the store capacity list.  it is already loaded and the dropdown is identical.
            try
            {
                StoreGroupListViewProfile storeGroup = (StoreGroupListViewProfile)_imoGroupList.FindKey(SAB.ClientServerSession.GlobalOptions.OTSPlanStoreGroupRID);
                midAttributeCbx.Initialize(SAB, FunctionSecurity, _imoGroupList.ArrayList, true);
                this.midAttributeCbx.SelectedValue = storeGroup.Key;
                // Begin TT#2731 - JSmith - Unable to copy allocation override method from global
                //_imoGroupLevelList = SAB.StoreServerSession.GetStoreGroupLevelListViewList(storeGroup.Key, true);
                _allocationOverrideMethod.IMOGroupLevelList = StoreMgmt.StoreGroup_GetLevelListViewList(storeGroup.Key, true); //SAB.StoreServerSession.GetStoreGroupLevelListViewList(storeGroup.Key, true);
                // End TT#2731 - JSmith - Unable to copy allocation override method from global

                AdjustTextWidthComboBox_DropDown(midAttributeCbx); //TT#7 - MD - RBeck - Dynamic dropdowns
            }
            catch (Exception exception)
            {
                HandleException(exception);
            }
        }

        private void ReservationGridLayout()
        {
            this.ugReservation.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            this.ugReservation.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.ugReservation.DisplayLayout.Appearance.ImageBackgroundStyle = Infragistics.Win.ImageBackgroundStyle.Stretched;
            this.ugReservation.DisplayLayout.AddNewBox.Hidden = true;
            this.ugReservation.DisplayLayout.GroupByBox.Hidden = true;
            this.ugReservation.DisplayLayout.GroupByBox.Prompt = string.Empty;
            this.ugReservation.DisplayLayout.Bands["Sets"].Columns["SetID"].Header.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_Store_Set);
            this.ugReservation.DisplayLayout.Bands["Sets"].Columns["Reservation Store"].Header.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_IMO_ID);
            this.ugReservation.DisplayLayout.Bands["Sets"].Columns["Min Ship Qty"].Header.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_IMO_MIN_SHIP_QTY);
            this.ugReservation.DisplayLayout.Bands["Sets"].Columns["Pct Pack Threshold"].Header.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_IMO_PCT_PK_THRSHLD);
            this.ugReservation.DisplayLayout.Bands["Sets"].Columns["Item Max"].Header.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_IMO_MAX_VALUE);

            this.ugReservation.DisplayLayout.Bands["Stores"].Columns["SetID"].Hidden = true;
            this.ugReservation.DisplayLayout.Bands["Stores"].Columns["Updated"].Hidden = true;
            this.ugReservation.DisplayLayout.Bands["Stores"].Columns["Store RID"].Hidden = true;
            this.ugReservation.DisplayLayout.Bands["Stores"].Columns["Store ID"].Header.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_ST_ID);
            this.ugReservation.DisplayLayout.Bands["Stores"].Columns["Reservation Store"].Header.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_IMO_ID);
            this.ugReservation.DisplayLayout.Bands["Stores"].Columns["Min Ship Qty"].Header.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_IMO_MIN_SHIP_QTY);
            this.ugReservation.DisplayLayout.Bands["Stores"].Columns["Pct Pack Threshold"].Header.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_IMO_PCT_PK_THRSHLD);
            this.ugReservation.DisplayLayout.Bands["Stores"].Columns["Item Max"].Header.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_IMO_MAX_VALUE);

            this.ugReservation.DisplayLayout.UseFixedHeaders = true;
            this.ugReservation.DisplayLayout.Bands["Sets"].Columns["SetID"].Header.Fixed = true;
            this.ugReservation.DisplayLayout.Bands["Stores"].Columns["Store ID"].Header.Fixed = true;
        }

        // Begin TT#2731 - JSmith - Unable to copy allocation override method from global
        //private void Reservation_Define()
        //{
        //    try
        //    {
        //        _imoDataSet = MIDEnvironment.CreateDataSet("reservationDataSet");

        //        DataTable setTable = _imoDataSet.Tables.Add("Sets");

        //        DataColumn dataColumn;
        //        //Create Columns and rows for datatable

        //        dataColumn = new DataColumn();
        //        dataColumn.DataType = System.Type.GetType("System.String");
        //        dataColumn.ColumnName = "SetID";
        //        dataColumn.ReadOnly = true;
        //        dataColumn.Unique = false;
        //        setTable.Columns.Add(dataColumn);

        //        dataColumn = new DataColumn();
        //        dataColumn.DataType = System.Type.GetType("System.String");
        //        dataColumn.ColumnName = "Reservation Store";
        //        dataColumn.ReadOnly = true;
        //        dataColumn.Unique = false;
        //        setTable.Columns.Add(dataColumn);

        //        dataColumn = new DataColumn();
        //        dataColumn.DataType = System.Type.GetType("System.String");
        //        dataColumn.ColumnName = "Min Ship Qty";
        //        dataColumn.ReadOnly = false;
        //        dataColumn.Unique = false;
        //        setTable.Columns.Add(dataColumn);

        //        dataColumn = new DataColumn();
        //        dataColumn.DataType = System.Type.GetType("System.String");
        //        dataColumn.ColumnName = "Pct Pack Threshold";
        //        dataColumn.ReadOnly = false;
        //        dataColumn.Unique = false;
        //        setTable.Columns.Add(dataColumn);

        //        dataColumn = new DataColumn();
        //        dataColumn.DataType = System.Type.GetType("System.String");
        //        dataColumn.ColumnName = "Item Max";
        //        dataColumn.ReadOnly = false;
        //        dataColumn.Unique = false;
        //        setTable.Columns.Add(dataColumn);

        //        //make set ID the primary key
        //        DataColumn[] PrimaryKeyColumn = new DataColumn[1];
        //        PrimaryKeyColumn[0] = setTable.Columns["SetID"];
        //        setTable.PrimaryKey = PrimaryKeyColumn;

        //        DataTable storeTable = _imoDataSet.Tables.Add("Stores");

        //        dataColumn = new DataColumn();
        //        dataColumn.DataType = System.Type.GetType("System.String");
        //        dataColumn.ColumnName = "SetID";
        //        dataColumn.ReadOnly = true;
        //        dataColumn.Unique = false;
        //        storeTable.Columns.Add(dataColumn);

        //        dataColumn = new DataColumn();
        //        dataColumn.DataType = System.Type.GetType("System.Boolean");
        //        dataColumn.ColumnName = "Updated";
        //        dataColumn.ReadOnly = false;
        //        dataColumn.Unique = false;
        //        storeTable.Columns.Add(dataColumn);

        //        dataColumn = new DataColumn();
        //        dataColumn.DataType = System.Type.GetType("System.Int32");
        //        dataColumn.ColumnName = "Store RID";
        //        dataColumn.ReadOnly = true;
        //        dataColumn.Unique = true;
        //        storeTable.Columns.Add(dataColumn);

        //        dataColumn = new DataColumn();
        //        dataColumn.DataType = System.Type.GetType("System.String");
        //        dataColumn.ColumnName = "Store ID";
        //        dataColumn.ReadOnly = true;
        //        dataColumn.Unique = false;
        //        storeTable.Columns.Add(dataColumn);

        //        dataColumn = new DataColumn();
        //        dataColumn.DataType = System.Type.GetType("System.String");
        //        dataColumn.ColumnName = "Reservation Store";
        //        dataColumn.ReadOnly = true;
        //        dataColumn.Unique = false;
        //        storeTable.Columns.Add(dataColumn);

        //        dataColumn = new DataColumn();
        //        dataColumn.DataType = System.Type.GetType("System.String");
        //        dataColumn.ColumnName = "Min Ship Qty";
        //        dataColumn.ReadOnly = false;
        //        dataColumn.Unique = false;
        //        storeTable.Columns.Add(dataColumn);

        //        dataColumn = new DataColumn();
        //        dataColumn.DataType = System.Type.GetType("System.String");
        //        dataColumn.ColumnName = "Pct Pack Threshold";
        //        dataColumn.ReadOnly = false;
        //        dataColumn.Unique = false;
        //        storeTable.Columns.Add(dataColumn);

        //        dataColumn = new DataColumn();
        //        dataColumn.DataType = System.Type.GetType("System.String");
        //        dataColumn.ColumnName = "Item Max";
        //        dataColumn.ReadOnly = false;
        //        dataColumn.Unique = false;
        //        storeTable.Columns.Add(dataColumn);

        //        _imoDataSet.Relations.Add("Stores",
        //            _imoDataSet.Tables["Sets"].Columns["SetID"],
        //            _imoDataSet.Tables["Stores"].Columns["SetID"]);
        //    }
        //    catch (Exception exception)
        //    {
        //        HandleException(exception);
        //    }
        //}
        // End TT#2731 - JSmith - Unable to copy allocation override method from global

        private void ugReservation_CellChange(object sender, Infragistics.Win.UltraWinGrid.CellEventArgs e)
        {
            try
            {
                //  is the grid populated?
                if (_imoIsPopulated)
                {
                    if (e.Cell.Row.Band.Key == "Stores")
                        e.Cell.Row.Cells["Updated"].Value = true;
                    IMOChangesMade = true;
                }
                string cellError = "";
                if (
                    ((e.Cell.Row.Cells["Min Ship Qty"].Text.Length > 0) ||
                    (e.Cell.Row.Cells["Pct Pack Threshold"].Text.Length > 0)) &&
                    (e.Cell.Row.Cells["Item Max"].Text.Length == 0)
                    )
                {
                    cellError = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_al_ItemMaxRequired);
                    e.Cell.Row.Cells["Item Max"].Appearance.Image = ErrorImage;
                    e.Cell.Row.Cells["Item Max"].Tag = cellError;
                    e.Cell.Row.Cells["Item Max"].ToolTipText = cellError;
                    ItemMaxEntryError = true;
                    //e.Row.Cells["Item Max"].Refresh();
                }
                else
                {
                    e.Cell.Row.Cells["Item Max"].Appearance.Image = null;
                    e.Cell.Row.Cells["Item Max"].Tag = null;
                    e.Cell.Row.Cells["Item Max"].ToolTipText = null;
                    ItemMaxEntryError = false;
                }
            }
            catch
            {
                throw;
            }
        }

        private void ugReservation_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            try
            {
                if (!_reservationGridBuilt)
                {
                    // check for saved layout
                    InfragisticsLayoutData layoutData = new InfragisticsLayoutData();
                    // Begin TT#461-MD - JSmith - Node Properties VSW tab error
                    //InfragisticsLayout layout = layoutData.InfragisticsLayout_Read(SAB.ClientServerSession.UserRID, eLayoutID.capacityGrid);
                    //if (layout.LayoutLength > 0)
                    //{
                    //    ugReservation.DisplayLayout.Load(layout.LayoutStream);
                    //}
                    //else
                    //{
                    //    MIDRetail.Windows.Controls.UltraGridLayoutDefaults ugld = new MIDRetail.Windows.Controls.UltraGridLayoutDefaults(ErrorImage);
                    //    ugld.ApplyDefaults((Infragistics.Win.UltraWinGrid.UltraGrid)sender, e, true, 2, false);
                    //    ReservationGridLayout();
                    //}
                    MIDRetail.Windows.Controls.UltraGridLayoutDefaults ugld = new MIDRetail.Windows.Controls.UltraGridLayoutDefaults(ErrorImage);
                    ugld.ApplyDefaults((Infragistics.Win.UltraWinGrid.UltraGrid)sender, e, true, 2, false);
                    ReservationGridLayout();
                    // End TT#461-MD - JSmith - Node Properties VSW tab error
                    this.ugReservation.DisplayLayout.Bands["Sets"].Columns["SetID"].Header.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_Store_Set);
                    this.ugReservation.DisplayLayout.Bands["Sets"].Columns["Reservation Store"].Header.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_IMO_ID);
                    this.ugReservation.DisplayLayout.Bands["Sets"].Columns["Min Ship Qty"].Header.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_IMO_MIN_SHIP_QTY);
                    this.ugReservation.DisplayLayout.Bands["Sets"].Columns["Pct Pack Threshold"].Header.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_IMO_PCT_PK_THRSHLD);
                    this.ugReservation.DisplayLayout.Bands["Sets"].Columns["Item Max"].Header.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_IMO_MAX_VALUE);

                    //  this is an application text example
                    //this.ugReservation.DisplayLayout.Bands["Sets"].Columns["Ratio"].Header.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_Similar_Store_Ratio);

                    this.ugReservation.DisplayLayout.Bands["Stores"].Columns["Store ID"].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Left;
                    this.ugReservation.DisplayLayout.Bands["Stores"].Columns["Store ID"].Header.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_ST_ID);
                    this.ugReservation.DisplayLayout.Bands["Stores"].Columns["Reservation Store"].Header.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_IMO_ID);
                    this.ugReservation.DisplayLayout.Bands["Stores"].Columns["Min Ship Qty"].Header.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_IMO_MIN_SHIP_QTY);
                    this.ugReservation.DisplayLayout.Bands["Stores"].Columns["Pct Pack Threshold"].Header.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_IMO_PCT_PK_THRSHLD);
                    this.ugReservation.DisplayLayout.Bands["Stores"].Columns["Item Max"].Header.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_IMO_MAX_VALUE);

                    _reservationGridBuilt = true;
                }

                if (!this.FunctionSecurity.AllowUpdate)
                {
                    foreach (UltraGridBand ugb in this.ugReservation.DisplayLayout.Bands)
                    {
                        ugb.Override.AllowDelete = DefaultableBoolean.False;
                    }
                }
                else
                {
                    foreach (UltraGridBand ugb in this.ugReservation.DisplayLayout.Bands)
                    {
                        ugb.Override.AllowDelete = DefaultableBoolean.True;
                    }
                }

            }
            catch (Exception exception)
            {
                HandleException(exception);
            }
        }

        private bool ValidTabReservation(ref EditMsgs em)
        {
            try
            {
                //if (FormIsClosing && !SaveOnClose)
                //{
                //    return true;
                //}

                if (ItemMaxEntryError)
                {
                    em.AddMsg(eMIDMessageLevel.Edit, "Item Max Must Have A Value", this.ToString());
                }

                if (em.ErrorFound)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception exception)
            {
                em.AddMsg(eMIDMessageLevel.Error, exception.Message, this.ToString());
                HandleException(exception);
            }

            return false;
        }

        private void Reservation_SetRowProperties(FunctionSecurityProfile securityProfile)
        {
            try
            {
                Infragistics.Win.UltraWinGrid.UltraGridChildBand ultraGridStoreBand;
                foreach (UltraGridRow setRow in this.ugReservation.Rows)
                {
                    ultraGridStoreBand = setRow.ChildBands["Stores"];	// get "Stores" band
                    foreach (UltraGridRow storeRow in ultraGridStoreBand.Rows)
                    {
                        //  if there is no reservation store, the user shouldn't be able to edit the fields.
                        if (storeRow.Cells["Reservation Store"].Text == "")
                        {
                            storeRow.Cells["Min Ship Qty"].Activation = Activation.NoEdit;
                            storeRow.Cells["Pct Pack Threshold"].Activation = Activation.NoEdit;
                            storeRow.Cells["Item Max"].Activation = Activation.NoEdit;
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                HandleException(exception);
            }
        }

        private void ugReservation_DragDrop(object sender, DragEventArgs e)
        {
            TreeNodeClipboardList cbList = null;
            HierarchyNodeClipboardList hnList = null;
            try
            {
                //ClipboardProfile cbp = GetClipboardData(eClipboardDataType.HierarchyNode);
                if (e.Data.GetDataPresent(typeof(TreeNodeClipboardList)))
                {
                    cbList = (TreeNodeClipboardList)e.Data.GetData(typeof(TreeNodeClipboardList));
                    if (cbList.ClipboardDataType == eProfileType.HierarchyNode)
                    {
                        if (FunctionSecurity.IsReadOnly)
                        {
                            MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_NotAuthorized));
                        }
                        else
                        {
                            Reservation_Populate(cbList.ClipboardProfile.Key);
                            IMOChangesMade = true;
                        }
                    }
                }
                else if (e.Data.GetDataPresent(typeof(HierarchyNodeClipboardList)))
                {
                    hnList = (HierarchyNodeClipboardList)e.Data.GetData(typeof(HierarchyNodeClipboardList));
                    if (FunctionSecurity.IsReadOnly)
                    {
                        MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_NotAuthorized));
                    }
                    else
                    {
                        Reservation_Populate(hnList.ClipboardProfile.Key);
                        IMOChangesMade = true;
                    }
                }
            }
            catch (BadDataInClipboardException)
            {
                MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_BadDataInClipboard),
                    this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception exception)
            {
                HandleException(exception);
            }
        }

        // BEGIN TT#2083 - gtaylor - when item max value is not present, the row should not save other values
        void ugReservation_AfterCellUpdate(object sender, Infragistics.Win.UltraWinGrid.CellEventArgs e)
        {
            string cellError = "";
            if (
                ((e.Cell.Row.Cells["Min Ship Qty"].Text.Length > 0) ||
                (e.Cell.Row.Cells["Pct Pack Threshold"].Text.Length > 0)) &&
                (e.Cell.Row.Cells["Item Max"].Text.Length == 0)
                )
            {
                cellError = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_al_ItemMaxRequired);
                e.Cell.Row.Cells["Item Max"].Appearance.Image = ErrorImage;
                e.Cell.Row.Cells["Item Max"].Tag = cellError;
                e.Cell.Row.Cells["Item Max"].ToolTipText = cellError;
                ItemMaxEntryError = true;
                //e.Row.Cells["Item Max"].Refresh();
            }
            else
            {
                e.Cell.Row.Cells["Item Max"].Appearance.Image = null;
                e.Cell.Row.Cells["Item Max"].Tag = null;
                e.Cell.Row.Cells["Item Max"].ToolTipText = null;
                ItemMaxEntryError = false;
            }
        }

        private void ugReservation_BeforeRowUpdate(object sender, CancelableRowEventArgs e)
        {
            string cellError = "";
            if (
                ((e.Row.Cells["Min Ship Qty"].Text.Length > 0) ||
                (e.Row.Cells["Pct Pack Threshold"].Text.Length > 0)) &&
                (e.Row.Cells["Item Max"].Text.Length == 0)
                )
            {
                cellError = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_al_ItemMaxRequired);
                e.Row.Cells["Item Max"].Appearance.Image = ErrorImage;
                e.Row.Cells["Item Max"].Tag = cellError;
                e.Row.Cells["Item Max"].ToolTipText = cellError;
                ItemMaxEntryError = true;
                //e.Row.Cells["Item Max"].Refresh();
                //e.Cancel = false;
            }
            else
            {
                e.Row.Cells["Item Max"].Appearance.Image = null;
                e.Row.Cells["Item Max"].Tag = null;
                e.Row.Cells["Item Max"].ToolTipText = null;
                ItemMaxEntryError = false;
            }
        }
        // END TT#2083 - gtaylor - when item max value is not present, the row should not save other values

        private void ugReservation_DragEnter(object sender, DragEventArgs e)
        {
            try
            {
                ObjectDragEnter(e);
            }
            catch (Exception exception)
            {
                HandleException(exception);
            }
        }

        // End TT#1401 - Reservation Stores

		private void txtReserve_TextChanged(object sender, System.EventArgs e)
		{
			CheckReserveValue();
		}

		private void CheckReserveValue()
		{
			// BEGIN TT#667 - Stodd - Pre-allocate Reserve
			if (txtReserve.Text == null || txtReserve.Text.Trim() == string.Empty)
			{
				rbReservePct.Enabled = rbReserveUnits.Enabled = false;
				txtReserveAsBulk.Enabled = false;
				txtReserveAsPacks.Enabled = false;
				txtReserveAsBulk.Text = string.Empty;
				txtReserveAsPacks.Text = string.Empty;
				rbReservePct.Checked = false;
				rbReserveUnits.Checked = false;
				lblReserveAsBulk.Enabled = false;
				lblReserveAsPacks.Enabled = false;

			}
			else if (FunctionSecurity.AllowUpdate)
			{
				rbReservePct.Enabled = rbReserveUnits.Enabled = true;
				txtReserveAsBulk.Enabled = true;
				txtReserveAsPacks.Enabled = true;
				lblReserveAsBulk.Enabled = true;
				lblReserveAsPacks.Enabled = true;

			}
			// END TT#667 - Stodd - Pre-allocate Reserve
		}

		private void txtReserve_Validating(object sender, System.ComponentModel.CancelEventArgs e)
		{
			ErrorProvider.SetError (txtReserve,string.Empty);
			double dblValue;
			try
			{
				string inStr = ((TextBox)sender).Text.ToString(CultureInfo.CurrentUICulture);
				if (inStr == null || inStr.Trim() == string.Empty) 
					return;
				
				dblValue = Convert.ToDouble(inStr, CultureInfo.CurrentUICulture);
				
				if (rbReservePct.Checked)
				{
					dblValue = Math.Round(dblValue,2);
					txtReserve.Text = dblValue.ToString(CultureInfo.CurrentUICulture);
					//txtZeroToOneHundredPercent_Validating(sender, e);
				}
				else
				{
					dblValue = Math.Round(dblValue,0);
					txtReserve.Text = dblValue.ToString(CultureInfo.CurrentUICulture);
				}
			}
			catch
			{
				ErrorProvider.SetError (txtReserve, SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MustBeNumeric));
				MessageBox.Show( SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MustBeNumeric));
				((TextBox)sender).Text = string.Empty;
				((TextBox)sender).Focus();
			}
		}
		private void txtOHFactor_Validating(object sender, System.ComponentModel.CancelEventArgs e)
		{
			double dblValue;
			ErrorProvider.SetError (txtOHFactor,string.Empty);
			try
			{
				string inStr = ((TextBox)sender).Text.ToString(CultureInfo.CurrentUICulture);
				if (inStr == null || inStr.Trim() == string.Empty) 
					return;
				
				dblValue = Convert.ToDouble(inStr, CultureInfo.CurrentUICulture);
				dblValue = Math.Round(dblValue,2);
				txtOHFactor.Text = dblValue.ToString(CultureInfo.CurrentUICulture);
				//txtZeroToOneHundredPercent_Validating(sender, e);

                // BEGIN MID Track #5340 - On hand Factor not saving after question
                //						   Moved following check to ValidateSpecificFields()
                // BEGIN MID Track #3218 - V26 - OH flips to lower level of hier when keying in % Factor
                //if (inStr.Trim().Length > 0) 
                //{
                //    if (Convert.ToInt32(cboOnHand.SelectedValue, CultureInfo.CurrentUICulture) != _styleKey && _askQuestion)
                //    {
                //        string errorMessage  = string.Format(MIDText.GetText(eMIDTextCode.msg_ChangeSelectionQuestion),
                //            this.lblOnHand.Text,_styleText);
                //        if (MessageBox.Show (errorMessage,  this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                //            == DialogResult.Yes) 
                //        {
                //            cboOnHand.SelectedValue = _styleKey;
                //        }
                //        _askQuestion = false;
                //    }
                //}
				// END MID Track #3218
                // END MID Track #5240
			}
			catch
			{
				ErrorProvider.SetError (txtOHFactor, SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MustBeNumeric));
				MessageBox.Show( SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MustBeNumeric));
				((TextBox)sender).Text = string.Empty;
				((TextBox)sender).Focus();
			}
		}
		
		private void txtZeroToOneHundredPercent_Validating(object sender, System.ComponentModel.CancelEventArgs e)
		{
			ErrorProvider.SetError (((TextBox)sender),string.Empty);
			try
			{
				string inStr = ((TextBox)sender).Text.ToString(CultureInfo.CurrentUICulture);
				if (inStr == null || inStr.Trim() == string.Empty) 
					return;

				double outVal = Convert.ToDouble(inStr, CultureInfo.CurrentUICulture);
								
				if (outVal < 0.0 || outVal > 100.0)
				{
					throw new Exception();
				}
			}
			catch
			{
				ErrorProvider.SetError (((TextBox)sender),SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MustBeBetween0And100));
				MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MustBeBetween0And100));
				((TextBox)sender).Text = string.Empty;
				((TextBox)sender).Focus();
			}
		}
		private void txtNegToOneHundredPercent_Validating(object sender, System.ComponentModel.CancelEventArgs e)
		{
			ErrorProvider.SetError (((TextBox)sender),string.Empty);
			try
			{
				string inStr = ((TextBox)sender).Text.ToString(CultureInfo.CurrentUICulture);
				if (inStr == null || inStr.Trim() == string.Empty) 
					return;

				double outVal = Convert.ToDouble(inStr, CultureInfo.CurrentUICulture);			
				// if (outVal < -100.0 || outVal > 100.0)
				if (outVal > 100.0)
				{
					throw new Exception();
				}
				else
				{
					outVal = Math.Round(outVal,2);
					((TextBox)sender).Text = outVal.ToString(CultureInfo.CurrentUICulture);
				}
			}
			catch
			{
				string errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ValueCannotExceed100);
				ErrorProvider.SetError (((TextBox)sender),errorMessage);
				MessageBox.Show(errorMessage);
				((TextBox)sender).Text = string.Empty;
				((TextBox)sender).Focus();
			}
		}

		private void cboStoreAttribute_SelectionChangeCommitted(object sender, System.EventArgs e)
		{
			try
			{ 
				if (FormLoaded)
				{
					//_trans.AllocationStoreAttributeID = Convert.ToInt32(cboStoreAttribute.SelectedValue, CultureInfo.CurrentUICulture);
					ChangePending = true;
				}
				if (this.cboStoreAttribute.SelectedValue != null)

					PopulateStoreAttributeSet(this.cboStoreAttribute.SelectedValue.ToString()); 
			}
			catch (Exception exc)
			{HandleException(exc);	}
		}
        // BEGIN TT#618 - AGallagher - Allocation Override - Add Attribute Sets (#35)
        private void cboSGStoreAttribute_SelectionChangeCommitted(object sender, System.EventArgs e)
        {
			try
			{
				if (FormLoaded)
				{
					if (_resetStoreGroup)
					{
						_resetStoreGroup = false;
					}
					else
					{
						ChangePending = true;

						if (ShowStoreGradesWarningMessage())
						{
							DialogResult dr = DialogResult.Yes;
							string msg = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_al_StoreGradeChangeWarning);
							msg = msg.Replace("{0}", "current Attribute");
							dr = MessageBox.Show(msg, MIDText.GetTextOnly(eMIDTextCode.frm_OverrideMethod), MessageBoxButtons.YesNo);
							if (dr == DialogResult.No)
							{
								_resetStoreGroup = true;
                                //this.cboSGStoreAttribute.SelectedValue = this._allocationOverrideMethod.sGstoreGroupRID; // TT#488 - MD - Jellis - Group Allocation
                                this.cboSGStoreAttribute.SelectedValue = this._allocationOverrideMethod.GradeStoreGroupRID; // TT#488 - MD - Jellis - Group Allocation
							}
							else if (dr == DialogResult.Yes)
							{
							}
						}
					}
				}
                //if (this.cboSGStoreAttribute.SelectedValue != null && (int)cboSGStoreAttribute.SelectedValue != _allocationOverrideMethod.sGstoreGroupRID) // TT#488 - MD - Jellis - Group Allocation
                if (this.cboSGStoreAttribute.SelectedValue != null && (int)cboSGStoreAttribute.SelectedValue != _allocationOverrideMethod.GradeStoreGroupRID)   // TT#488 - MD - Jellis - Group Allocation
				{
                    // Begin TT#939 - JSmith - Allocation Override -> will not clear fields in grid on store grade tab, change attribute set and get System Object
                    _SGStoreAttributeChanged = true;
                    // End TT#939
					PopulateSGStoreAttributeSet(cboSGStoreAttribute.SelectedValue.ToString());
					if (FormLoaded)
					{
                        //_allocationOverrideMethod.sGstoreGroupRID = (int)cboSGStoreAttribute.SelectedValue; // TT#488 - MD - Jellis - Group Allocation
                        _allocationOverrideMethod.GradeStoreGroupRID = (int)cboSGStoreAttribute.SelectedValue;   // TT#488 - MD - Jellis - Group Allocation
						StoreGrades_InitialPopulate();
						_StoreGradeValueChange = false;
					}
                    // Begin TT#939 - JSmith - Allocation Override -> will not clear fields in grid on store grade tab, change attribute set and get System Object
                    _SGStoreAttributeChanged = false;
                    // End TT#939
				}

			}
			catch (Exception exc)
			{ HandleException(exc); }
        }
        // END TT#618 - AGallagher - Allocation Override - Add Attribute Sets (#35)

		// BEGIN TT#618 - STodd - Allocation Override - Add Attribute Sets (#35)
		private bool ShowStoreGradesWarningMessage()
		{
			bool showMessage = false;

			if (_StoreGradeValueChange)
			{
				showMessage = true;
			}
			//else
			//{
			//    DataTable dt = (DataTable)ugStoreGrades.DataSource;
			//    string filter = _storegradesDataTable.DefaultView.RowFilter;
			//    _storegradesDataTable.DefaultView.RowFilter = null;

			//    for (int i = 0; i < dt.Rows.Count; i++)
			//    {
			//        for (int j = 0; j < 6; j++)
			//        {
			//            if (ugStoreGrades.Rows[i].Cells[j + 4].Value != System.DBNull.Value)
			//            {
			//                showMessage = true;
			//                break;
			//            }
			//        }
			//    }
			//    _storegradesDataTable.DefaultView.RowFilter = filter;
			//}

			return showMessage;
		}
		// END TT#618 - STodd - Allocation Override - Add Attribute Sets (#35)

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
                // Begin TT#4473 - JSmith - Allocation Override Method -> Capacity Tab-> new method opens with attribute and no sets, allows check of Exceed Capacity-> when process it ignores capacity
                //if (FormLoaded)
                if (_capacityIsPopulated)
                // End TT#4473 - JSmith - Allocation Override Method -> Capacity Tab-> new method opens with attribute and no sets, allows check of Exceed Capacity-> when process it ignores capacity
				{
					_dsOverRide.Tables["Capacity"].Clear();
                    this.ugCapacity.ResetDisplayLayout();
                    ApplyAppearance(ugCapacity);
					
					_capacityDataTable.Clear();
					foreach (StoreGroupLevelListViewProfile sglProf in pl)
					{
						DataRow newRow = _capacityDataTable.NewRow();
						newRow["SglRID"] = sglProf.Key;
						newRow["Set"] =  sglProf.Name;
						newRow["ExceedChar"] = "0";
						newRow["Exceed Capacity"] = false;
						_capacityDataTable.Rows.Add(newRow);
					}
					this.ugCapacity.DataSource = null;
					this.ugCapacity.DataSource = _capacityDataTable;

					this.ugCapacity.DisplayLayout.Bands[0].Columns["SglRID"].Hidden = true;
					this.ugCapacity.DisplayLayout.Bands[0].Columns["ExceedChar"].Hidden = true;
					this.ugCapacity.DisplayLayout.Bands[0].Columns["Set"].CellActivation = Activation.ActivateOnly;
				}
			}
			catch (Exception exc)
			{HandleException(exc);	}
		}

        // BEGIN TT#618 - AGallagher - Allocation Override - Add Attribute Sets (#35)
        private void PopulateSGStoreAttributeSet(string key)
        {
            try
            {
                ProfileList pl = StoreMgmt.StoreGroup_GetLevelListViewList(Convert.ToInt32(key, CultureInfo.CurrentUICulture)); //SAB.StoreServerSession.GetStoreGroupLevelListViewList(Convert.ToInt32(key, CultureInfo.CurrentUICulture));
				this.cmbSGAttributeSet.ValueMember = "Key";
				this.cmbSGAttributeSet.DisplayMember = "Name";
				this.cmbSGAttributeSet.DataSource = pl.ArrayList;
                // Begin TT#939 - JSmith - Allocation Override -> will not clear fields in grid on store grade tab, change attribute set and get System Object
                if (cmbSGAttributeSet.SelectedValue == null)
                {
                    cmbSGAttributeSet.SelectedValue = ((StoreGroupLevelListViewProfile)(pl.ArrayList[0])).Key;
                }
                // End TT#939
                _curAttributeSet = Convert.ToInt32(cmbSGAttributeSet.SelectedValue, CultureInfo.CurrentUICulture);  // TT#939 - RMatelic - Allocation Override -> will not clear fields in grid on store grade tab, change attribute set and get System Object
				// temp
				//if (FormLoaded)
				//{
				//    StoreGrades_Populate(this._allocationOverrideMethod.OTSPlanRID);
				//}
            }
            catch (Exception exc)
            { HandleException(exc); }
        }	
        // END TT#618 - AGallagher - Allocation Override - Add Attribute Sets (#35)

		private void cbxExceedCapacity_CheckStateChanged(object sender, System.EventArgs e)
		{
			if (cbxExceedCapacity.Checked)
			{
				foreach(DataRow dr in _capacityDataTable.Rows)
				{
					dr["Exceed Capacity"] = false;
					dr["ExceedChar"] = Include.ConvertBoolToChar((bool)dr["Exceed Capacity"]);
					dr["Exceed by %"] = System.DBNull.Value;
				} 
			}

			this.ugCapacity.Enabled = !cbxExceedCapacity.Checked;
		}

		private void rbReservePct_CheckedChanged(object sender, System.EventArgs e)
		{
			if (rbReservePct.Checked == false) return;
			ErrorProvider.SetError(txtReserve, string.Empty); 
			try
			{
				string inStr = txtReserve.Text.ToString(CultureInfo.CurrentUICulture);
				if (inStr == "")
				{
					return;
				}	
				decimal outdec = Convert.ToDecimal(inStr, CultureInfo.CurrentUICulture);
			
				if (outdec > 100)
				{
					throw new Exception();
				}
			}
			catch
			{
				ErrorProvider.SetError(txtReserve, SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MustBeBetween0And100));
				MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MustBeBetween0And100));
				txtReserve.Focus();
				return;
			}
			 
		}

		private void rbReserveUnits_CheckedChanged(object sender, System.EventArgs e)
		{
			if (rbReserveUnits.Checked == false) return;
		 
			try
			{
				string inStr = txtReserve.Text.ToString(CultureInfo.CurrentUICulture);
				if (inStr == "") return;
					 	
				decimal outdec = Convert.ToDecimal(inStr, CultureInfo.CurrentUICulture);
				long outint = Convert.ToInt64(outdec, CultureInfo.CurrentUICulture);
				txtReserve.Text = Convert.ToString(outint, CultureInfo.CurrentUICulture); 
			}		 
			catch
			{
				MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MustBeNumeric));
				txtReserve.Focus();
				return;
			}
			 
		}

		private void txtPositiveInteger_Validating(object sender, System.ComponentModel.CancelEventArgs e)
		{
			string errorMessage;
			try
			{
				ErrorProvider.SetError ((TextBox)sender,string.Empty);
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
				errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MustBePositiveInteger);
				ErrorProvider.SetError ((TextBox)sender,errorMessage);
				MessageBox.Show(errorMessage);
				((TextBox)sender).Text = string.Empty;
				((TextBox)sender).Focus();
			}
		}
		#region Store Grades Tab

		private void DefaultStoreGradesGridLayout()
		{
			try
			{
				this.ugStoreGrades.DisplayLayout.Appearance.ImageBackgroundStyle = Infragistics.Win.ImageBackgroundStyle.Stretched;
				//this.ugStoreGrades.DisplayLayout.Override.AllowAddNew = Infragistics.Win.DefaultableBoolean.False;
				this.ugStoreGrades.DisplayLayout.Override.AllowColSizing = Infragistics.Win.UltraWinGrid.AllowColSizing.Default;
				this.ugStoreGrades.DisplayLayout.Bands[0].Columns["Grade"].CellAppearance.TextHAlign =  Infragistics.Win.HAlign.Left;
				this.ugStoreGrades.DisplayLayout.Bands[0].Columns["Grade"].Width = 40;
				this.ugStoreGrades.DisplayLayout.Bands[0].Columns["Boundary"].Width = 75;
				this.ugStoreGrades.DisplayLayout.Bands[0].Columns["Allocation Min"].CellAppearance.TextHAlign =  Infragistics.Win.HAlign.Right;
				this.ugStoreGrades.DisplayLayout.Bands[0].Columns["Allocation Min"].Width = 70;
				this.ugStoreGrades.DisplayLayout.Bands[0].Columns["Allocation Max"].CellAppearance.TextHAlign =  Infragistics.Win.HAlign.Right;
				this.ugStoreGrades.DisplayLayout.Bands[0].Columns["Allocation Max"].Width = 70;
				this.ugStoreGrades.DisplayLayout.Bands[0].Columns["Min Ad"].CellAppearance.TextHAlign =  Infragistics.Win.HAlign.Right;
				this.ugStoreGrades.DisplayLayout.Bands[0].Columns["Min Ad"].Width = 70;
				//			this.ugStoreGrades.DisplayLayout.Bands[0].Columns["Max Ad"].CellAppearance.TextHAlign =  Infragistics.Win.HAlign.Right;
				//			this.ugStoreGrades.DisplayLayout.Bands[0].Columns["Max Ad"].Width = 70;
				this.ugStoreGrades.DisplayLayout.Bands[0].Columns["Color Min"].CellAppearance.TextHAlign =  Infragistics.Win.HAlign.Right;
				this.ugStoreGrades.DisplayLayout.Bands[0].Columns["Color Min"].Width = 70;
				this.ugStoreGrades.DisplayLayout.Bands[0].Columns["Color Max"].CellAppearance.TextHAlign =  Infragistics.Win.HAlign.Right;
				this.ugStoreGrades.DisplayLayout.Bands[0].Columns["Color Max"].Width = 70;

				int colScrollWidth = this.ugStoreGrades.DisplayLayout.Bands[0].Columns["Grade"].Width;
				colScrollWidth += this.ugStoreGrades.DisplayLayout.Bands[0].Columns["Boundary"].Width;
				colScrollWidth += this.ugStoreGrades.DisplayLayout.Bands[0].Columns["WOS Index"].Width;

				this.ugStoreGrades.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
				this.ugStoreGrades.DisplayLayout.MaxColScrollRegions = 2;
				this.ugStoreGrades.DisplayLayout.ColScrollRegions[0].Width = colScrollWidth;
				this.ugStoreGrades.DisplayLayout.ColScrollRegions[0].Split (this.ugStoreGrades.DisplayLayout.ColScrollRegions[0].Width);
			}
			catch( Exception exception )
			{
				HandleException(exception);
			}
		}
		private void BuildStoreGradesContextmenu()
		{
			try
			{
				MenuItem mnuItemInsert = new MenuItem(MIDText.GetTextOnly(eMIDTextCode.lbl_Insert));
				MenuItem mnuItemInsertBefore = new MenuItem(MIDText.GetTextOnly(eMIDTextCode.lbl_Insert_Before));
				MenuItem mnuItemInsertAfter = new MenuItem(MIDText.GetTextOnly(eMIDTextCode.lbl_Insert_after));
				MenuItem mnuItemCut= new MenuItem(MIDText.GetTextOnly(eMIDTextCode.lbl_Cut));
				MenuItem mnuItemCopy = new MenuItem(MIDText.GetTextOnly(eMIDTextCode.lbl_Copy));
				MenuItem mnuItemPaste = new MenuItem(MIDText.GetTextOnly(eMIDTextCode.lbl_Paste));
				MenuItem mnuItemDelete = new MenuItem(MIDText.GetTextOnly(eMIDTextCode.lbl_Delete));
				MenuItem mnuItemDeleteAll = new MenuItem(MIDText.GetTextOnly(eMIDTextCode.lbl_DeleteAll));

				mnuStoreGrades.MenuItems.Add(mnuItemInsert);
				mnuItemInsert.MenuItems.Add(mnuItemInsertBefore);
				mnuItemInsert.MenuItems.Add(mnuItemInsertAfter);
				mnuStoreGrades.MenuItems.Add(mnuItemDelete);
				mnuStoreGrades.MenuItems.Add(mnuItemDeleteAll);
				mnuItemInsert.Click += new System.EventHandler(this.mnuSGItemInsert_Click);
				mnuItemInsertBefore.Click += new System.EventHandler(this.mnuSGItemInsertBefore_Click);
				mnuItemInsertAfter.Click += new System.EventHandler(this.mnuSGItemInsertAfter_Click);
				mnuItemCut.Click += new System.EventHandler(this.mnuSGItemCut_Cut);
				mnuItemCopy.Click += new System.EventHandler(this.mnuSGItemCopy_Click);
				mnuItemPaste.Click += new System.EventHandler(this.mnuSGItemPaste_Click);
				mnuItemDelete.Click += new System.EventHandler(this.mnuSGItemDelete_Click);
				//			mnuItemExpandAll.Click += new System.EventHandler(this.mnuSGItemExpandAll_Click);
				//			mnuItemCollapseAll.Click += new System.EventHandler(this.mnuSGItemCollapseAll_Click);
				mnuItemDeleteAll.Click += new System.EventHandler(this.mnuSGItemDeleteAll_Click);
			}
			catch( Exception exception )
			{
				HandleException(exception);
			}
		}

		private void mnuSGItemInsert_Click(object sender, System.EventArgs e)
		{
		}

		private void mnuSGItemInsertBefore_Click(object sender, System.EventArgs e)
		{
			try
			{
				_setRowPosition = false;
				int rowPosition  = 0, firstRowPos = 0;
				UltraGridRow firstRow = null;
				if (ugStoreGrades.Rows.Count > 0)
				{
					if (this.ugStoreGrades.ActiveRow == null) return;
					firstRow = this.ugStoreGrades.ActiveRowScrollRegion.FirstRow;
					firstRowPos = Convert.ToInt32(firstRow.Cells["RowPosition"].Value, CultureInfo.CurrentUICulture);
					rowPosition = Convert.ToInt32(this.ugStoreGrades.ActiveRow.Cells["RowPosition"].Value, CultureInfo.CurrentUICulture);
					// increment the position of the active row to end of grid
					foreach(  UltraGridRow gridRow in ugStoreGrades.Rows )
					{
						if (Convert.ToInt32(gridRow.Cells["RowPosition"].Value, CultureInfo.CurrentUICulture) >= rowPosition)
						{
							gridRow.Cells["RowPosition"].Value = Convert.ToInt32(gridRow.Cells["RowPosition"].Value, CultureInfo.CurrentUICulture) + 1;
						}
					}
				}
				UltraGridRow addedRow = this.ugStoreGrades.DisplayLayout.Bands[0].AddNew();
				addedRow.Cells["RowPosition"].Value = rowPosition;
				addedRow.Cells["Grade"].Value = " ";
				this.ugStoreGrades.UpdateData();
				this.ugStoreGrades.DisplayLayout.Bands[0].SortedColumns.Clear();
				this.ugStoreGrades.DisplayLayout.Bands[0].SortedColumns.Add("RowPosition", false);

				_setRowPosition = true;
			 
				this.ugStoreGrades.ActiveRowScrollRegion.ScrollRowIntoView(addedRow);
				if (firstRow != null && (firstRowPos < rowPosition))
				{
					this.ugStoreGrades.ActiveRowScrollRegion.FirstRow = firstRow;
				}
			}
			catch( Exception exception )
			{
				HandleException(exception);
			}
		}

		private void mnuSGItemInsertAfter_Click(object sender, System.EventArgs e)
		{
			try
			{
				_setRowPosition = false;
				int rowPosition  = 0, firstRowPos = 0;
				UltraGridRow firstRow = null;
				if (ugStoreGrades.Rows.Count > 0)
				{
					if (this.ugStoreGrades.ActiveRow == null) return;
					firstRow = this.ugStoreGrades.ActiveRowScrollRegion.FirstRow;
					firstRowPos = Convert.ToInt32(firstRow.Cells["RowPosition"].Value, CultureInfo.CurrentUICulture);
					rowPosition = Convert.ToInt32(this.ugStoreGrades.ActiveRow.Cells["RowPosition"].Value, CultureInfo.CurrentUICulture);
					// increment the position of the active row to end of grid
					foreach(  UltraGridRow gridRow in ugStoreGrades.Rows )
					{
						if (Convert.ToInt32(gridRow.Cells["RowPosition"].Value, CultureInfo.CurrentUICulture) > rowPosition)
						{
							gridRow.Cells["RowPosition"].Value = Convert.ToInt32(gridRow.Cells["RowPosition"].Value, CultureInfo.CurrentUICulture) + 1;
						}
					}
					rowPosition = rowPosition + 1;   
				}
				UltraGridRow addedRow = this.ugStoreGrades.DisplayLayout.Bands[0].AddNew();
				addedRow.Cells["RowPosition"].Value = rowPosition;
				addedRow.Cells["Grade"].Value = " ";
				this.ugStoreGrades.UpdateData(); 
				this.ugStoreGrades.DisplayLayout.Bands[0].SortedColumns.Clear();
				this.ugStoreGrades.DisplayLayout.Bands[0].SortedColumns.Add("RowPosition", false);
				_setRowPosition = true;
			
				this.ugStoreGrades.ActiveRowScrollRegion.ScrollRowIntoView(addedRow);
				if (firstRow != null && ( rowPosition - firstRowPos < 5 ))
				{
					this.ugStoreGrades.ActiveRowScrollRegion.FirstRow = firstRow;
				}
			}
			catch( Exception exception )
			{
				HandleException(exception);
			}
		}

		private void mnuSGItemCut_Cut(object sender, System.EventArgs e)
		{
		}

		private void mnuSGItemCopy_Click(object sender, System.EventArgs e)
		{
		}

		private void mnuSGItemPaste_Click(object sender, System.EventArgs e)
		{
		}

		private void mnuSGItemDelete_Click(object sender, System.EventArgs e)
		{
			try
			{
//				if (this.ugStoreGrades.ActiveRow != null)
//				{
//					this.ugStoreGrades.ActiveRow.Delete();
//				}
				if (this.ugStoreGrades.Selected.Rows.Count > 0)
					this.ugStoreGrades.DeleteSelectedRows();
			}
			catch( Exception exception )
			{
				HandleException(exception);
			}
		}

		private void mnuSGItemDeleteAll_Click(object sender, System.EventArgs e)
		{
			try
			{
				_dsOverRide.Tables["StoreGrades"].Clear();
				_dsOverRide.Tables["StoreGrades"].AcceptChanges();
				StoreGradesChangesMade = true;
			}
			catch( Exception exception )
			{
				HandleException(exception);
			}
		}

		private void mnuSGItemExpandAll_Click(object sender, System.EventArgs e)
		{
			try
			{
				this.ugStoreGrades.Rows.ExpandAll(true);
			}
			catch( Exception exception )
			{
				HandleException(exception);
			}
		}

		private void mnuSGItemCollapseAll_Click(object sender, System.EventArgs e)
		{
			try
			{
				this.ugStoreGrades.Rows.CollapseAll(true);
			}
			catch( Exception exception )
			{
				HandleException(exception);
			}
		}

		// Begin TT#618 - STodd - Allocation Override - Add Attribute Sets (#35)
		private void StoreGrades_InitialPopulate()
		{
			StoreGrades_InitialPopulate(Include.NoRID);
		}
		// End TT#618 - STodd - Allocation Override - Add Attribute Sets (#35)

		/// <summary>
		/// Populates store grade grid when the Merch Node changes or when 
		/// a new attribute set is chosen.
		/// </summary>
		/// <param name="nodeRID"></param>
		private void StoreGrades_InitialPopulate(int nodeRID)
		{
			try
			{
				//			string minStock, maxStock, minAd, maxAd, minColor, maxColor;
				int count = 0, col = 0;
				int minStock, maxStock, minAd, minColor, maxColor, shipUpTo;
				
				_dsOverRide.Tables["StoreGrades"].Clear();
				_dsOverRide.Tables["StoreGrades"].AcceptChanges();

				// Begin TT#618 - STodd - Allocation Override - Add Attribute Sets (#35)
				ArrayList sglList = (ArrayList)this.cmbSGAttributeSet.DataSource;
				if (nodeRID != Include.NoRID || _storeGradeList == null)
				{
					_storeGradeList = SAB.HierarchyServerSession.GetStoreGradeList(nodeRID, false, true);
				}
				int rowCnt = sglList.Count * _storeGradeList.Count;
				bool[,] cellIsNull = new Boolean[rowCnt, 6];
				// End TT#618 - STodd - Allocation Override - Add Attribute Sets (#35)

				// Begin TT#618 - STodd - Allocation Override - Add Attribute Sets (#35)
				foreach (StoreGroupLevelListViewProfile sgllvp in sglList)
				{
					foreach (StoreGradeProfile sgp in _storeGradeList)
					{
						col = 0;
						if (sgp.MinStock > Include.Undefined)
						{
							//minStock = sgp.MinStock.ToString(CultureInfo.CurrentUICulture);
							minStock = sgp.MinStock;
							cellIsNull[count, col] = false;
						}
						else
						{
							minStock = 0;
							cellIsNull[count, col] = true;
						}
						col++;
						if (sgp.MaxStock > Include.Undefined)
						{
							//maxStock = sgp.MaxStock.ToString(CultureInfo.CurrentUICulture);
							maxStock = sgp.MaxStock;
							cellIsNull[count, col] = false;
						}
						else
						{
							maxStock = 0;
							cellIsNull[count, col] = true;
						}
						col++;
						if (sgp.MinAd > Include.Undefined)
						{
							//minAd = sgp.MinAd.ToString(CultureInfo.CurrentUICulture);
							minAd = sgp.MinAd;
							cellIsNull[count, col] = false;
						}
						else
						{
							minAd = 0;
							cellIsNull[count, col] = true;
						}
						col++;
						if (sgp.MinColor > Include.Undefined)
						{
							//minColor = sgp.MinColor.ToString(CultureInfo.CurrentUICulture);
							minColor = sgp.MinColor;
							cellIsNull[count, col] = false;
						}
						else
						{
							minColor = 0;
							cellIsNull[count, col] = true;
						}
						col++;
						if (sgp.MaxColor > Include.Undefined)
						{
							//maxColor = sgp.MaxColor.ToString(CultureInfo.CurrentUICulture);
							maxColor = sgp.MaxColor;
							cellIsNull[count, col] = false;
						}
						else
						{
							maxColor = 0;
							cellIsNull[count, col] = true;
						}

						col++;
						if (sgp.ShipUpTo > Include.Undefined)
						{
							shipUpTo = sgp.ShipUpTo;
							cellIsNull[count, col] = false;
						}
						else
						{
							shipUpTo = 0;
							cellIsNull[count, col] = true;
						}


						_dsOverRide.Tables["StoreGrades"].Rows.Add(new object[] { count, sgllvp.Key,  sgp.Boundary, sgp.StoreGrade,   
																					  minStock, maxStock, minAd,  
																					  minColor, maxColor, shipUpTo });  
						++count;
					}
				}
				// End TT#618 - STodd - Allocation Override - Add Attribute Sets (#35)

				_dsOverRide.Tables["StoreGrades"].DefaultView.RowFilter = null;

				this.ugStoreGrades.DataSource = _dsOverRide.Tables["StoreGrades"];
				// MID Track 1860 - null out grid values bases on cellIsNull 
				// Begin TT#618 - STodd - Allocation Override - Add Attribute Sets (#35)
				for (int i = 0; i <  count; i++)
				{
					for (int j = 0; j < 6; j++)
					{
						if (cellIsNull[i,j])
						{
							ugStoreGrades.Rows[i].Cells[j+4].Value = System.DBNull.Value;
						}
					}
				}
				// End TT#618 - STodd - Allocation Override - Add Attribute Sets (#35)

				// Begin TT#618 - STodd - Allocation Override - Add Attribute Sets (#35)
				_dsOverRide.Tables["StoreGrades"].DefaultView.RowFilter = "SGLRID = " + this.cmbSGAttributeSet.SelectedValue.ToString();
				_StoreGradeValueChange = false;
				// End TT#618 - STodd - Allocation Override - Add Attribute Sets (#35)

				FunctionSecurityProfile securityLevel = FunctionSecurity;
				
				if (FunctionSecurity.AllowUpdate)
				{
					this.ugStoreGrades.DisplayLayout.AddNewBox.Hidden = false;
				}
				else
				{
					this.ugStoreGrades.DisplayLayout.AddNewBox.Hidden = true;
				}

                // Begin TT#939 - JSmith - Allocation Override -> will not clear fields in grid on store grade tab, change attribute set and get System Object
                ErrorProvider.SetError(ugStoreGrades, null);
                // End TT#939
			}
			catch( Exception exception )
			{
				HandleException(exception);
			}
		}

		private void ugStoreGrades_DragDrop(object sender, System.Windows.Forms.DragEventArgs e)
		{
            TreeNodeClipboardList cbList = null;
            HierarchyNodeClipboardList hnList = null;
            try
            {
                //ClipboardProfile cbp = GetClipboardData(eClipboardDataType.HierarchyNode);
                if (e.Data.GetDataPresent(typeof(TreeNodeClipboardList)))
                {
                    cbList = (TreeNodeClipboardList)e.Data.GetData(typeof(TreeNodeClipboardList));
                    if (cbList.ClipboardDataType == eProfileType.HierarchyNode)
                    {
                        if (FunctionSecurity.IsReadOnly)
                        {
                            MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_NotAuthorized));

                        }
                        else
                        {
							if (ShowStoreGradesWarningMessage())
							{
								DialogResult dr = DialogResult.Yes;
								string msg = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_al_StoreGradeChangeWarning);
								msg = msg.Replace("{0}", "grade boundaries");
								dr = MessageBox.Show(msg, MIDText.GetTextOnly(eMIDTextCode.frm_OverrideMethod), MessageBoxButtons.YesNo);
								if (dr == DialogResult.Yes)
								{
									StoreGrades_InitialPopulate(cbList.ClipboardProfile.Key);
									StoreGradesChangesMade = true;
									MinMaxesChangesMade = true;
								}
							}
							else
							{
								StoreGrades_InitialPopulate(cbList.ClipboardProfile.Key);
								StoreGradesChangesMade = true;
								MinMaxesChangesMade = true;
							}
                        }
                    }
                }
                else if (e.Data.GetDataPresent(typeof(HierarchyNodeClipboardList)))
                {
                    hnList = (HierarchyNodeClipboardList)e.Data.GetData(typeof(HierarchyNodeClipboardList));
                    if (FunctionSecurity.IsReadOnly)
                    {
                        MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_NotAuthorized));

                    }
                    else
                    {
						if (ShowStoreGradesWarningMessage())
						{
							DialogResult dr = DialogResult.Yes;
							string msg = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_al_StoreGradeChangeWarning);
							msg = msg.Replace("{0}", "grade boundaries");
							dr = MessageBox.Show(msg, MIDText.GetTextOnly(eMIDTextCode.frm_OverrideMethod), MessageBoxButtons.YesNo);
							if (dr == DialogResult.Yes)
							{
								StoreGrades_InitialPopulate(hnList.ClipboardProfile.Key);
								StoreGradesChangesMade = true;
								MinMaxesChangesMade = true;
							}
						}
						else
						{
							StoreGrades_InitialPopulate(hnList.ClipboardProfile.Key);
							StoreGradesChangesMade = true;
							MinMaxesChangesMade = true;
						}
                    }
                }
            }
            catch (BadDataInClipboardException)
            {
                MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_BadDataInClipboard),
                    this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception exception)
            {
                HandleException(exception);
            }
		}

		private void ugStoreGrades_DragEnter(object sender, System.Windows.Forms.DragEventArgs e)
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

		private void ugStoreGrades_CellChange(object sender, Infragistics.Win.UltraWinGrid.CellEventArgs e)
		{
			try
			{
				if (_storeGradesIsPopulated)
				{
					switch (this.ugStoreGrades.ActiveCell.Column.Key)
					{
						case "Grade":
						case "Boundary":
							StoreGradesChangesMade = true;
							//InheritanceProvider.SetError (this.lblStoreGradesInheritance,string.Empty);
							break;
						case "Allocation Min":
						case "Allocation Max":
						case "Min Ad":
						case "Color Min":
						case "Color Max":
							MinMaxesChangesMade = true;
							//InheritanceProvider.SetError (this.lblMinMaxesInheritance,string.Empty);
							break;
					}
					ChangePending = true;
					_StoreGradeValueChange = true;
				}
			}
			catch( Exception exception )
			{
				HandleException(exception);
			}
		}

        // BEGIN TT#1896 - Allocation Override Error - GTaylor
        void ugStoreGrades_InitializeRow(object sender, InitializeRowEventArgs e)
        {
            if (e.Row.Cells["Grade"].Text.Trim() != "")
            {
                if (e.Row.IsActiveRow == false)
                    e.Row.Cells["Grade"].Activation = Activation.Disabled;
            }
            else
            {
                e.Row.Cells["Grade"].Activation = Activation.AllowEdit;
            }
        }
        // END TT#1896 - Allocation Override Error - GTaylor

		private void ugStoreGrades_AfterRowInsert(object sender, Infragistics.Win.UltraWinGrid.RowEventArgs e)
		{
			try
			{ 
				e.Row.Cells["RowPosition"].Value = this.ugStoreGrades.Rows.Count;
				e.Row.Cells["Grade"].Value = " ";
				// BEGIN TT#618 - STodd - Allocation Override - Add Attribute Sets (#35)
				e.Row.Cells["SGLRID"].Value = int.Parse(cmbSGAttributeSet.SelectedValue.ToString()); ;
				// BEGIN TT#618 - STodd - Allocation Override - Add Attribute Sets (#35)
				this.ugStoreGrades.UpdateData();
				_newGradeAdded = true;
			}
			catch( Exception exception )
			{
				HandleException(exception);
			}
		}
        // BEGIN TT#616 - AGallagher - Allocation - Pack Rounding (#67)
        private void ugPackRounding_AfterRowInsert(object sender, Infragistics.Win.UltraWinGrid.RowEventArgs e)
        {
            try
            {
				// Begin TT#616 - stodd - pack rounding
				if (SAB.ApplicationServerSession.GlobalOptions.GenericPackRounding1stPackPct == Include.DefaultGenericPackRounding1stPackPct)
				{
					e.Row.Cells["FstPack"].Value = DBNull.Value;
				}
				else
				{
					e.Row.Cells["FstPack"].Value = SAB.ApplicationServerSession.GlobalOptions.GenericPackRounding1stPackPct;
				}
                // BEGIN TT#944 - AGallagher - Allocation Override Method pack rounding receive data error message when adding a pack 
				// if (SAB.ApplicationServerSession.GlobalOptions.GenericPackRounding1stPackPct == Include.DefaultGenericPackRoundingNthPackPct)
                if (SAB.ApplicationServerSession.GlobalOptions.GenericPackRoundingNthPackPct == Include.DefaultGenericPackRoundingNthPackPct)
                // END TT#944 - AGallagher - Allocation Override Method pack rounding receive data error message when adding a pack 
				{
					e.Row.Cells["NthPack"].Value = DBNull.Value;
				}
				else
				{
					e.Row.Cells["NthPack"].Value = SAB.ApplicationServerSession.GlobalOptions.GenericPackRoundingNthPackPct;
				}
				// End TT#616 - stodd - pack rounding
            }
            catch (Exception exception)
            {
                HandleException(exception);
            }
        }
        private void ugPackRounding_AfterCellUpdate(object sender, Infragistics.Win.UltraWinGrid.CellEventArgs e)
        {
            try
            {
                if (e.Cell.Column.Key == "PackText")
                { e.Cell.Row.Cells["PackMultiple"].Value = e.Cell.Row.Cells["PackText"].Value; }
            }
            catch (Exception exception)
            {
                HandleException(exception);
            }
        }
        private void ugPackRounding_BeforeRowsDeleted(object sender, Infragistics.Win.UltraWinGrid.BeforeRowsDeletedEventArgs e)
        {

            //// Stop the grid from displaying it's message box.
            //e.DisplayPromptMsg = false;

            //// Display our own custom message box.
            //System.Windows.Forms.DialogResult result =
            //    System.Windows.Forms.MessageBox.Show(
            //    "Deleting " + e.Rows.Length.ToString() + " row(s). Continue ?",
            //    "Delete rows?",
            //    System.Windows.Forms.MessageBoxButtons.YesNo,
            //    System.Windows.Forms.MessageBoxIcon.Question);

            //// If the user clicked No on the message box, cancel the deletion of rows.
            //if (System.Windows.Forms.DialogResult.No == result)
            //    e.Cancel = true;

            {
                foreach (UltraGridRow gridRow in ugPackRounding.Selected.Rows)
                {
                    // Begin TT#935 - stodd - pack rounding
                    try
                    {
                        if ((int)gridRow.Cells["PackMultiple"].Value == -1)
                        { e.Cancel = true; }
                    }
                    catch
                    {
                        // if the pack multiple is not numeric for this check, swallow it.
                    }
                    // End TT#935 - stodd - pack rounding
                }
            }

        }
        // END TT#616 - AGallagher - Allocation - Pack Rounding (#67)


		// BEGIN TT#618 - Stodd - Allocation Override - Add Attribute Sets (#35)
		private void ugStoreGrades_BeforeRowsDeleted(object sender, BeforeRowsDeletedEventArgs e)
		{
			if (_deletedGrades == null)
				_deletedGrades = new ArrayList();
			else
				_deletedGrades.Clear();

			foreach (UltraGridRow row in e.Rows)
			{
				string grade = row.Cells["Grade"].Value.ToString().Trim();
				if (!_deletedGrades.Contains(grade))
				{
					_deletedGrades.Add(grade);
				}
			}
		}
		// END TT#618 - Stodd - Allocation Override - Add Attribute Sets (#35)


		private void ugStoreGrades_AfterRowsDeleted(object sender, System.EventArgs e)
		{
			try
			{
				if (_storeGradesIsPopulated)
				{
					// BEGIN TT#618 - Stodd - Allocation Override - Add Attribute Sets (#35)
					DataTable dt = (DataTable)ugStoreGrades.DataSource;
					foreach (string grade in _deletedGrades)
					{
						DataRow[] rows = dt.Select("Grade = '" + grade + "'");
						foreach (DataRow row in rows)
						{
							dt.Rows.Remove(row);
						}
					}
					dt.AcceptChanges();
					// END TT#618 - Stodd - Allocation Override - Add Attribute Sets (#35)

					StoreGradesChangesMade = true;
					MinMaxesChangesMade = true;
					ugStoreGrades.UpdateData();
				}
			}
			catch( Exception exception )
			{
				HandleException(exception);
			}
		}

		/// <summary>
		/// Reassignes the RowPosition after a sort
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ugStoreGrades_AfterSortChange(object sender, Infragistics.Win.UltraWinGrid.BandEventArgs e)
		{
			try
			{
				int count = 0;
				if (_setRowPosition)
				{
					foreach(  UltraGridRow gridRow in ugStoreGrades.Rows )
					{
						gridRow.Cells["RowPosition"].Value = count;
						++count;
					}
				}
			}
			catch( Exception exception )
			{
				HandleException(exception);
			}
		}

		private void ugStoreGrades_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			try
			{
				Infragistics.Win.UIElement mouseUIElement;
				Infragistics.Win.UIElement headerUIElement;
				Infragistics.Win.UltraWinGrid.UltraGridCell mouseCell;
				HeaderUIElement headerUI = null;
				Point point = new Point(e.X, e.Y);

				if (e.Button == MouseButtons.Right)
				{
					// retrieve the UIElement from the location of the mouse 
					mouseUIElement = ugStoreGrades.DisplayLayout.UIElement.ElementFromPoint(new Point(e.X, e.Y));
					if ( mouseUIElement == null ) { return; }

					headerUIElement = mouseUIElement.GetAncestor(typeof(HeaderUIElement));
					if(null == headerUIElement)
					{
						// retrieve the Cell from the UIElement 
						mouseCell = (Infragistics.Win.UltraWinGrid.UltraGridCell)mouseUIElement.GetContext(typeof(Infragistics.Win.UltraWinGrid.UltraGridCell));

						// if there is a cell object reference, set to active cell and edit
						if (mouseCell != null)
						{
							ugStoreGrades.ActiveCell = mouseCell;	// set the cell under the mouse as active
							//						AdjustStoreCapacityGridMenu(mouseCell);
						}
					}
					else
						if ( headerUIElement.GetType() == typeof(HeaderUIElement) )
					{
						headerUI = (HeaderUIElement)headerUIElement;
						Infragistics.Win.UltraWinGrid.ColumnHeader colHeader = null;
						_gridCol = null;
						colHeader = (Infragistics.Win.UltraWinGrid.ColumnHeader)headerUI.SelectableItem;
						_gridCol = colHeader.Column;
						if ( _gridCol == null )
							return;

						_gridBand = colHeader.Band;

						if (FunctionSecurity.AllowUpdate)
						{
							this.mnuGridColHeader.Show(this.ugStoreGrades, point);
						}
					}
				}
			}
			catch( Exception exception )
			{
				HandleException(exception);
			}
		}

		private void ugStoreGrades_MouseEnterElement(object sender, Infragistics.Win.UIElementEventArgs e)
		{
			try
			{
				ShowUltraGridToolTip(ugStoreGrades, e);
			}
			catch( Exception exception )
			{
				HandleException(exception);
			}
		}

		private void ugStoreGrades_BeforeExitEditMode(object sender, Infragistics.Win.UltraWinGrid.BeforeExitEditModeEventArgs e)
		{
			try
			{
				string errorMessage = null;
				if (DoValueByValueEdits)
				{
					switch (this.ugStoreGrades.ActiveCell.Column.Key)
					{
						case "Grade":
							if (!StoreGradeValid(ugStoreGrades.ActiveCell, ref errorMessage))
							{
								if (errorMessage != string.Empty)
								{	
									MessageBox.Show (errorMessage,  this.Text, 	MessageBoxButtons.OK, MessageBoxIcon.Error);
									e.Cancel = true; 
								}
							}
							break;
						case "Boundary":
							if (!StoreBoundaryValid(ugStoreGrades.ActiveCell, ref errorMessage))
							{
								MessageBox.Show (errorMessage,  this.Text, 	MessageBoxButtons.OK, MessageBoxIcon.Error);
								e.Cancel = true;
							}
							break;
						case "Allocation Min":
							if (!MinStockValid(ugStoreGrades.ActiveCell, ref errorMessage))
							{
								MessageBox.Show (errorMessage,  this.Text, 	MessageBoxButtons.OK, MessageBoxIcon.Error);
								e.Cancel = true;
							}
							break;
						case "Alocation Max":
							if (!MaxStockValid(ugStoreGrades.ActiveCell, ref errorMessage))
							{
								MessageBox.Show (errorMessage,  this.Text, 	MessageBoxButtons.OK, MessageBoxIcon.Error);
								e.Cancel = true;
							}
							break;
						case "Min Ad":
							if (!MinAdValid(ugStoreGrades.ActiveCell, ref errorMessage))
							{
								MessageBox.Show (errorMessage,  this.Text, 	MessageBoxButtons.OK, MessageBoxIcon.Error);
								e.Cancel = true;
							}
							break;
						case "Color Min":
							if (!ColorMinValid(ugStoreGrades.ActiveCell, ref errorMessage))
							{
								MessageBox.Show (errorMessage,  this.Text, 	MessageBoxButtons.OK, MessageBoxIcon.Error);
								e.Cancel = true;
							}
							break;
						case "Color Max":
							if (!ColorMaxValid(ugStoreGrades.ActiveCell, ref errorMessage))
							{
								MessageBox.Show (errorMessage,  this.Text, 	MessageBoxButtons.OK, MessageBoxIcon.Error);
								e.Cancel = true;
							}
							break;
					}
                    // Begin TT#939 - RMatelic - Allocation Override -> will not clear fields in grid on store grade tab, change attribute set and get System Object
                    if (e.Cancel)
                    {
                        DoValueByValueEdits = false;
                    }
				}   // End TT#939
			}
			catch( Exception exception )
			{
				HandleException(exception);
			}
		}

		private void ugStoreGrades_BeforeRowUpdate(object sender, Infragistics.Win.UltraWinGrid.CancelableRowEventArgs e)
		{
			try
			{
				// if row is completely empty, cancel the update
				if (e.Row.Cells["Grade"].Text.Length == 0 &&
					e.Row.Cells["Boundary"].Text.Length == 0 &&
					e.Row.Cells["Allocation Min"].Text.Length == 0 &&
					e.Row.Cells["Allocation Max"].Text.Length == 0 &&
					e.Row.Cells["Min Ad"].Text.Length == 0 &&
					e.Row.Cells["Color Min"].Text.Length == 0 &&
					e.Row.Cells["Color Max"].Text.Length == 0)
				{
					e.Cancel = true;
				}
			}
			catch( Exception exception )
			{
				HandleException(exception);
			}
		}

		private void ugStoreGrades_AfterRowUpdate(object sender, Infragistics.Win.UltraWinGrid.RowEventArgs e)
		{
			try
			{
				// BEGIN MID Track #3900 - Grid auto scrolling when data entered
				// remove  following code  
				//this.ugStoreGrades.DisplayLayout.Bands[0].SortedColumns.Clear();
				//this.ugStoreGrades.DisplayLayout.Bands[0].SortedColumns.Add("Boundary", true); 
				// END MID Track #3900	
			}
			catch( Exception exception )
			{
				HandleException(exception);
			}
		}

		private bool StoreGradeValid(UltraGridCell gridCell, ref string errorMessage)
		{
			bool errorFound = false;
			try
			{
				if (gridCell.Text.Trim().Length == 0)	// cell is empty
				{
					errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_FieldIsRequired);
					errorFound = true;
				}
				else
				{
					// make sure grades are unique
					foreach(  UltraGridRow gridRow in this.ugStoreGrades.Rows )
					{
						//						if (!gridRow.IsActiveRow)
						if (gridCell.Row != gridRow)
						{
							if ((string)gridCell.Text == (string)gridRow.Cells["Grade"].Text)
							{
								if  (!_dupGradeFound)
								{
									errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_DuplicateGradeNameNotAllowed);
									_dupGradeFound = true;	
								}
								else
								{
									errorMessage = string.Empty;
								}
								errorFound = true;
								break;
							}
						}
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

		private bool StoreBoundaryValid(UltraGridCell gridCell, ref string errorMessage)
		{
			bool errorFound = false;
			try
			{
				if (gridCell.Text.Length == 0)	// cell is empty
				{
					errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_FieldIsRequired);
					errorFound = true;
				}
				else
				{
                    // begin MID Track 6255 Enter max "1,000" and get error "must be integer" 
					//int cellValue = Convert.ToInt32(gridCell.Text, CultureInfo.CurrentUICulture);
                    // Begin TT#939 - JSmith - Allocation Override -> will not clear fields in grid on store grade tab, change attribute set and get System Object
                    //int cellValue = (int)gridCell.Value;
                    // end MID Track 6255 Enter max "1,000" and get error "must be integer" 
                    int cellValue = Convert.ToInt32(gridCell.Text.Trim().Replace(",", null), CultureInfo.CurrentUICulture);
                    // End TT#939
                    if (cellValue < 0)
					{
						errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MustBeNonNegative);
						errorFound = true;
					}
					else
					{
						// make sure boundaries are unique
						foreach(  UltraGridRow gridRow in this.ugStoreGrades.Rows )
						{
							//							if (!gridRow.IsActiveRow)
							if (gridCell.Row != gridRow)
							{
								if (gridRow.Cells["Boundary"].Text.Trim().Length > 0 )
								{
                                    // begin MID Track 6255 Enter max "1,000" and get error "must be integer" 
									//if (Convert.ToInt32(gridCell.Text, CultureInfo.CurrentUICulture) == Convert.ToInt32(gridRow.Cells["Boundary"].Text, CultureInfo.CurrentUICulture))
									if (cellValue == (int)gridRow.Cells["Boundary"].Value)
                                    // end MID Track 6255 Enter max "1,000" and get error "must be integer" 
                                    {
										errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_GradeBoundariesNotUnique);
										errorFound = true;
										break;
									}
								}
							}
						}
					}
				}
			}
			catch( Exception error)
			{
				string exceptionMessage = error.Message;
				errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MustBeInteger);
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

		private bool MinStockValid(UltraGridCell gridCell, ref string errorMessage)
		{
			bool errorFound = false;
			try
			{
                // Begin TT#939 - RMatelic - Allocation Override -> will not clear fields in grid on store grade tab, change attribute set and get System Object
                //if (gridCell.Text.Length == 0)	// cell is empty
                //{
                //    errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_FieldIsRequired);
                //    errorFound = true;
                //}
                //else
                if (gridCell.Text.Trim().Length > 0)
				{
                    // begin MID Track 6255 Enter max "1,000" and get error "must be integer" 
					//int cellValue = Convert.ToInt32(gridCell.Text, CultureInfo.CurrentUICulture);
                    //int cellValue = (int)gridCell.Value;
                    // end MID Track 6255 Enter max "1,000" and get error "must be integer" 
                    // Begin TT#939 - JSmith - Allocation Override -> will not clear fields in grid on store grade tab, change attribute set and get System Object
                    //int cellValue = Convert.ToInt32(gridCell.Text.Trim(), CultureInfo.CurrentUICulture);
                    int cellValue = Convert.ToInt32(gridCell.Text.Trim().Replace(",", null), CultureInfo.CurrentUICulture);
                    // End TT#939
                // End TT#939
                    if (cellValue < 0)
					{
						errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MustBeNonNegative);
						errorFound = true;
					}
				}
			}
			catch( Exception error)
			{
				string exceptionMessage = error.Message;
				errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MustBeInteger);
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

		private bool MaxStockValid(UltraGridCell gridCell, ref string errorMessage)
		{
			bool errorFound = false;
			try
			{
                // Begin TT#939 - RMatelic - Allocation Override -> will not clear fields in grid on store grade tab, change attribute set and get System Object
                //if (gridCell.Text.Length == 0)	// cell is empty
                //{
                //    errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_FieldIsRequired);
                //    errorFound = true;
                //}
                //else
                if (gridCell.Text.Trim().Length > 0)
                {
                    // begin MID Track 6255 Enter max "1,000" and get error "must be integer" 
                    //int cellValue = Convert.ToInt32(gridCell.Text, CultureInfo.CurrentUICulture);
                    //int cellValue = (int)gridCell.Value;
                    // end MID Track 6255 Enter max "1,000" and get error "must be integer" 
                    // Begin TT#939 - JSmith - Allocation Override -> will not clear fields in grid on store grade tab, change attribute set and get System Object
                    //int cellValue = Convert.ToInt32(gridCell.Text.Trim(), CultureInfo.CurrentUICulture);
                    int cellValue = Convert.ToInt32(gridCell.Text.Trim().Replace(",", null), CultureInfo.CurrentUICulture);
                    // End TT#939
                    // End TT#939
                    if (cellValue < 0)
					{
						errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MustBeNonNegative);
						errorFound = true;
					}
					else
                    {
						// validate against min stock if entered
                        if (gridCell.Row.Cells["Allocation Min"].Text.Trim().Length != 0)
                        {
                            // begin MID Track 6255 Enter max "1,000" and get error "must be integer" 
                            //if (Convert.ToInt32(gridCell.Row.Cells["Allocation Min"].Text, CultureInfo.CurrentUICulture) > cellValue)
                            if ((int)gridCell.Row.Cells["Allocation Min"].Value > cellValue)
                            // end MID Track 6255 Enter max "1,000" and get error "must be integer" 
                            {
                                errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MinStockNotLessThanMax);
                                errorFound = true;
                                gridCell.Row.Cells["Allocation Min"].Appearance.Image = ErrorImage;
                                gridCell.Row.Cells["Allocation Min"].Tag = errorMessage;
                            }
                            else
                            {
                                gridCell.Row.Cells["Allocation Min"].Appearance.Image = null;
                                gridCell.Row.Cells["Allocation Min"].Tag = null;
                            }
                        }
					}
				}
			}
			catch( Exception error)
			{
				string exceptionMessage = error.Message;
				errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MustBeInteger);
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

		private bool MinAdValid(UltraGridCell gridCell, ref string errorMessage)
		{
			bool errorFound = false;
			try
			{
                // Begin TT#939 - RMatelic - Allocation Override -> will not clear fields in grid on store grade tab, change attribute set and get System Object
                //if (gridCell.Text.Length == 0)	// cell is empty
                //{
                //    errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_FieldIsRequired);
                //    errorFound = true;
                //}
                //else
                if (gridCell.Text.Trim().Length > 0)
                {
                    // begin MID Track 6255 Enter max "1,000" and get error "must be integer" 
                    //int cellValue = Convert.ToInt32(gridCell.Text, CultureInfo.CurrentUICulture);
                    //int cellValue = (int)gridCell.Value;
                    // end MID Track 6255 Enter max "1,000" and get error "must be integer" 
                    // Begin TT#939 - JSmith - Allocation Override -> will not clear fields in grid on store grade tab, change attribute set and get System Object
                    //int cellValue = Convert.ToInt32(gridCell.Text.Trim(), CultureInfo.CurrentUICulture);
                    int cellValue = Convert.ToInt32(gridCell.Text.Trim().Replace(",", null), CultureInfo.CurrentUICulture);
                    // End TT#939
                // End TT#939
                    if (cellValue < 0)
					{
						errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MustBeNonNegative);
						errorFound = true;
					}
				}
			}
			catch( Exception error)
			{
				string exceptionMessage = error.Message;
				errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MustBeInteger);
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

		private bool ColorMinValid(UltraGridCell gridCell, ref string errorMessage)
		{
			bool errorFound = false;
			try
			{
				_colorMinError = false;
                // Begin TT#939 - RMatelic - Allocation Override -> will not clear fields in grid on store grade tab, change attribute set and get System Object
                //if (gridCell.Text.Length == 0)	// cell is empty
                //{
                //    errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_FieldIsRequired);
                //    errorFound = true;
                //}
                //else
                if (gridCell.Text.Trim().Length > 0)
                {
                    // begin MID Track 6255 Enter max "1,000" and get error "must be integer" 
                    //int cellValue = Convert.ToInt32(gridCell.Text, CultureInfo.CurrentUICulture);
                    //int cellValue = (int)gridCell.Value;
                    // end MID Track 6255 Enter max "1,000" and get error "must be integer" 
                    // Begin TT#939 - JSmith - Allocation Override -> will not clear fields in grid on store grade tab, change attribute set and get System Object
                    //int cellValue = Convert.ToInt32(gridCell.Text.Trim(), CultureInfo.CurrentUICulture);
                    int cellValue = Convert.ToInt32(gridCell.Text.Trim().Replace(",", null), CultureInfo.CurrentUICulture);
                    // End TT#939
                 // End TT#939
                    if (cellValue < 0)
                    // end MID Track 6255 Enter max "1,000" and get error "must be integer" 
                    {
                        _colorMinError = true;
                        errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MustBeNonNegative);
                        errorFound = true;
                    }
                    else
                    {
                        // validate against min stock if entered
                        if (gridCell.Row.Cells["Allocation Min"].Text.Trim().Length > 0)
                        {
                            // begin MID Track 6255 Enter max "1,000" and get error "must be integer" 
                            //if (Convert.ToInt32(gridCell.Row.Cells["Allocation Min"].Text, CultureInfo.CurrentUICulture) < cellValue)
                            if ((int)gridCell.Row.Cells["Allocation Min"].Value < cellValue)
                            // end MID Track 6255 Enter max "1,000" and get error "must be integer" 
                            {
                                _colorMinError = true;
                                errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MinColorNotLessThanMinStock);
                                errorFound = true;
                                gridCell.Row.Cells["Allocation Min"].Appearance.Image = ErrorImage;
                                gridCell.Row.Cells["Allocation Min"].Tag = errorMessage;
                            }
                        }
                    }
				}
			}
			catch( Exception error)
			{
				string exceptionMessage = error.Message;
				errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MustBeInteger);
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

		private bool ColorMaxValid(UltraGridCell gridCell, ref string errorMessage)
		{
			bool errorFound = false;
			try
			{
                // Begin TT#939 - RMatelic - Allocation Override -> will not clear fields in grid on store grade tab, change attribute set and get System Object
                //if (gridCell.Text.Length == 0)	// cell is empty
                //{
                //    errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_FieldIsRequired);
                //    errorFound = true;
                //}
                //else
                if (gridCell.Text.Trim().Length > 0)
                {
                    // begin MID Track 6255 Enter max "1,000" and get error "must be integer" 
                    //int cellValue = Convert.ToInt32(gridCell.Text, CultureInfo.CurrentUICulture);
                    //int cellValue = (int)gridCell.Value;
                    // end MID Track 6255 Enter max "1,000" and get error "must be integer" 
                    // Begin TT#939 - JSmith - Allocation Override -> will not clear fields in grid on store grade tab, change attribute set and get System Object
                    //int cellValue = Convert.ToInt32(gridCell.Text.Trim(), CultureInfo.CurrentUICulture);
                    int cellValue = Convert.ToInt32(gridCell.Text.Trim().Replace(",", null), CultureInfo.CurrentUICulture);
                    // End TT#939
                // End TT#939
                    if (cellValue < 0)
					{
						errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MustBeNonNegative);
						errorFound = true;
					}
					else if (gridCell.Row.Cells["Color Min"].Text.Length != 0) 	// validate against color min if entered
					{
                        // begin MID Track 6255 Enter max "1,000" and get error "must be integer" 
						//if (Convert.ToInt32(gridCell.Row.Cells["Color Min"].Text, CultureInfo.CurrentUICulture) > cellValue)
						if ((int)gridCell.Row.Cells["Color Min"].Value > cellValue)
                        // end MID Track 6255 Enter max "1,000" and get error "must be integer" 
                        {
							errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MinColorNotLessThanMax);
							errorFound = true;
							gridCell.Row.Cells["Color Min"].Appearance.Image = ErrorImage;
							gridCell.Row.Cells["Color Min"].Tag = errorMessage;
						}
						else if (!_colorMinError)
						{
							gridCell.Row.Cells["Color Min"].Appearance.Image = null;
							gridCell.Row.Cells["Color Min"].Tag = null;
						}
					}
					if (!errorFound)
					{
						// validate against max stock if entered
						if (gridCell.Row.Cells["Allocation Max"].Text.Length > 0)
                            // begin MID Track 6255 Enter max "1,000" and get error "must be integer" 
                            //if (Convert.ToInt32(gridCell.Row.Cells["Allocation Max"].Text, CultureInfo.CurrentUICulture) < cellValue)
                            if ((int)gridCell.Row.Cells["Allocation Max"].Value < cellValue)
                            // end MID Track 6255 Enter max "1,000" and get error "must be integer" 
							{
								errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MaxColorNotLessThanMaxStock);
								errorFound = true;
								gridCell.Row.Cells["Allocation Max"].Appearance.Image = ErrorImage;
								gridCell.Row.Cells["Allocation Max"].Tag = errorMessage;
							}
					}
				}
			}
			catch( Exception error)
			{
				string exceptionMessage = error.Message;
				errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MustBeInteger);
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

		private bool ValidTabStoreGrades(ref EditMsgs em)
		{
			try
			{
				string errorMessage = null;
				int lastBoundary = 0;
				_dupGradeFound = false;
				// sort descending by boundary
				this.ugStoreGrades.DisplayLayout.Bands[0].SortedColumns.Clear();
				this.ugStoreGrades.DisplayLayout.Bands[0].SortedColumns.Add("Boundary", true);
				UltraGridRow lastRow = null;
				if (ugStoreGrades.Rows.Count == 1)
				{
					errorMessage = "At least 2 Store Grades lines must be included.";
					em.AddMsg(eMIDMessageLevel.Edit, errorMessage,  this.ToString());
					ErrorProvider.SetError(ugStoreGrades,errorMessage);
				}
				foreach(  UltraGridRow gridRow in ugStoreGrades.Rows )
				{
					lastRow = gridRow;
					if (!StoreGradeValid(gridRow.Cells["Grade"], ref errorMessage))
					{
						em.AddMsg(eMIDMessageLevel.Edit, errorMessage,  this.ToString());
					}
				
					if (!StoreBoundaryValid(gridRow.Cells["Boundary"], ref errorMessage))
					{
						em.AddMsg(eMIDMessageLevel.Edit, errorMessage,  this.ToString());
					}
					else
					{
						lastBoundary = Convert.ToInt32(gridRow.Cells["Boundary"].Value, CultureInfo.CurrentUICulture);
					}
									
					if (gridRow.Cells["Allocation Min"].Text != string.Empty)
					{
						if (!MinStockValid(gridRow.Cells["Allocation Min"], ref errorMessage))
						{
							em.AddMsg(eMIDMessageLevel.Edit, errorMessage,  this.ToString());
						}
					}

					if (gridRow.Cells["Allocation Max"].Text != string.Empty)
					{
						if (!MaxStockValid(gridRow.Cells["Allocation Max"], ref errorMessage))
						{
							em.AddMsg(eMIDMessageLevel.Edit, errorMessage,  this.ToString());
						}
					}

					if (gridRow.Cells["Min Ad"].Text != string.Empty)
					{
						if (!MinAdValid(gridRow.Cells["Min Ad"], ref errorMessage))
						{
							em.AddMsg(eMIDMessageLevel.Edit, errorMessage,  this.ToString());
						}
					}
				
					if (gridRow.Cells["Color Min"].Text != string.Empty)
					{
						if (!ColorMinValid(gridRow.Cells["Color Min"], ref errorMessage))
						{
							em.AddMsg(eMIDMessageLevel.Edit, errorMessage,  this.ToString());
						}
					}

					if (gridRow.Cells["Color Max"].Text != string.Empty)
					{
						if (!ColorMaxValid(gridRow.Cells["Color Max"], ref errorMessage))
						{
							em.AddMsg(eMIDMessageLevel.Edit, errorMessage,  this.ToString());
						}
					}

				}

				if (lastBoundary != 0)
				{
					errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_LastBoundaryNotZero);
					em.AddMsg(eMIDMessageLevel.Edit, errorMessage,  this.ToString());
					lastRow.Cells["Boundary"].Appearance.Image = ErrorImage;
					lastRow.Cells["Boundary"].Tag = errorMessage;
				}
			
				if (em.ErrorFound)
				{
					return false;
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

		#endregion Store Grades Tab	

		// Begin MID Track 4858 - JSmith - Security changes
//		private void btnProcess_Click(object sender, System.EventArgs e)
//		{
//			try
//			{
//				ProcessAction(eMethodType.AllocationOverride);
//	
//				// as part of the  processing we saved the info, so it should be changed to update.
//				if (!ErrorFound)
//				{
//					_allocationOverrideMethod.Method_Change_Type = eChangeType.update;
//					btnSave.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_Update);
//				}
//
//			}
//			catch(Exception ex)
//			{
//				HandleException(ex, "btnProcess_Click");
//			}
//		}

		protected override void Call_btnProcess_Click()
		{
			try
			{
				// BEGIN TT#696-MD - Stodd - add "active process"
				//// BEGIN TT#497-MD - stodd -  Methods will not process with Method open
				//SelectedHeaderList selectedHeaderList = null;
				////bool isProcessingInAssortment = false;
				//bool useAssortmentHeaders = false;

				//useAssortmentHeaders = UseAssortmentSelectedHeaders();
				//if (useAssortmentHeaders)
				//{
				//    selectedHeaderList = SAB.AssortmentSelectedHeaderEvent.GetSelectedHeaders(this, eMethodType.AllocationOverride);
				//}
				//else
				//{
				//    // BEGIN MID Track #6022 - DB error trying to process new unsaved header
				//    selectedHeaderList = (SelectedHeaderList)SAB.ClientServerSession.GetSelectedHeaderList();
				//}
				//// END TT#497-MD - stodd -  Methods will not process with Method open

				//if (selectedHeaderList.Count == 0)
				//{
				//    MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_NoHeaderSelectedOnWorkspace));
				//    return;
				//}
				//// END MID Track #6022

				//====================================================
				// Checks to be sure there are valid selected headers
				//====================================================
				if (!OkToProcess(this, eMethodType.AllocationOverride))
				{
					return;
				}
				// END TT#696-MD - Stodd - add "active process"

                // BEGIN TT#1560 - MD - DOConnell - Allocation Override method - Do not allow the forecast level to be Style or below if running against a placeholder with a placeholder style
                if (!OkToProcess_Assortment(this, eMethodType.AllocationOverride, Convert.ToInt32(cboOTSPlan.SelectedValue)))
                {
                    return;
                }
                // END TT#1560 - MD - DOConnell - Allocation Override method - Do not allow the forecast level to be Style or below if running against a placeholder with a placeholder style
                
				ProcessAction(eMethodType.AllocationOverride);
	
				// as part of the  processing we saved the info, so it should be changed to update.
				if (!ErrorFound)
				{
					_allocationOverrideMethod.Method_Change_Type = eChangeType.update;
					btnSave.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_Update);
				}

			}
			catch(Exception ex)
			{
				HandleException(ex, "Call_btnProcess_Click");
			}
		}
		// End MID Track 4858

		#region Color Tab
		private bool ValidTabColor(ref EditMsgs em)
		{
			try
			{
				_dupColorFound = false;
				string errorMessage = null;
				UltraGridRow lastRow = null;
				bool allMinError = false;
				foreach (UltraGridRow gridRow in ugAllColors.Rows)
				{
					if (!AllColorsMinValid(gridRow.Cells["Minimum"], ref errorMessage))
					{
						em.AddMsg(eMIDMessageLevel.Edit, errorMessage,  this.ToString());
						allMinError = true;
					}
					if (!AllColorsMaxValid(gridRow.Cells["Maximum"], ref errorMessage))
					{
						if (errorMessage != string.Empty)
							em.AddMsg(eMIDMessageLevel.Edit, errorMessage,  this.ToString());
					}
					else if (!allMinError)
					{
						if (!AllColorsValid(gridRow.Cells["Minimum"], ref errorMessage))
						{
							em.AddMsg(eMIDMessageLevel.Edit, errorMessage,  this.ToString());
						}
					}
				}	
								
				foreach(  UltraGridRow gridRow in ugColor.Rows )
				{
					lastRow = gridRow;
					
					if (!ColorValid(gridRow.Cells["Color"], ref errorMessage))
					{
						if (errorMessage != string.Empty)
							em.AddMsg(eMIDMessageLevel.Edit, errorMessage,  this.ToString());
					}
											
					if (gridRow.Cells["Minimum"].Text != string.Empty)
					{
						if (!MinimumValid(gridRow.Cells["Minimum"], ref errorMessage))
						{
							em.AddMsg(eMIDMessageLevel.Edit, errorMessage,  this.ToString());
						}
					}

					if (gridRow.Cells["Maximum"].Text != string.Empty)
					{
						if (!MaximumValid(gridRow.Cells["Maximum"], ref errorMessage))
						{
							em.AddMsg(eMIDMessageLevel.Edit, errorMessage,  this.ToString());
						}
					}
				}
	
				if (em.ErrorFound)
				{
					return false;
				}
				else
				{
					return true;
				}
			}
			catch(Exception ex)
			{
				HandleException(ex);
			}
			
			return false;
		}

		private bool AllColorsMinValid(UltraGridCell gridCell, ref string errorMessage)
		{
			bool errorFound = false;
			try
			{
				if (gridCell.Text.Length == 0)	// cell is empty - use default
				{
					//errorMessage = errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_FieldIsRequired);
					//errorFound = true;
				}
				else
				{
                    // begin MID Track 6255 Enter max "1,000" and get error "must be integer" 
					//int cellValue = Convert.ToInt32(gridCell.Text, CultureInfo.CurrentUICulture);
                    // Begin TT#939 - JSmith - Allocation Override -> will not clear fields in grid on store grade tab, change attribute set and get System Object
                    //int cellValue = (int)gridCell.Value;
                    int cellValue = Convert.ToInt32(gridCell.Text.Trim().Replace(",", null), CultureInfo.CurrentUICulture);
                    // End TT#939
                    // end MID Track 6255 Enter max "1,000" and get error "must be integer" 
					if (cellValue < 0)
					{
						errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MustBeNonNegative);
						errorFound = true;
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
			{
				gridCell.Appearance.Image = ErrorImage;
				gridCell.Tag = errorMessage;
				return false;
			}
			else
			{
			    gridCell.Appearance.Image = null;
				gridCell.Tag = null;
				gridCell.Tag = _allColorsMinDefaultText;
				return true;
			}
		}
		
		private bool AllColorsMaxValid(UltraGridCell gridCell, ref string errorMessage)
		{
			bool errorFound = false;
			try
			{
				if (gridCell.Text.Length == 0)	// cell is empty - use default
				{
					//errorMessage = errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_FieldIsRequired);
					//errorFound = true;
				}
				else
				{
                    // begin MID Track 6255 Enter max "1,000" and get error "must be integer" 
                    // Begin TT#939 - JSmith - Allocation Override -> will not clear fields in grid on store grade tab, change attribute set and get System Object
					//int cellValue = Convert.ToInt32(gridCell.Text, CultureInfo.CurrentUICulture);
                    //int cellValue = (int)gridCell.Value;
                    int cellValue = Convert.ToInt32(gridCell.Text.Trim().Replace(",", null), CultureInfo.CurrentUICulture);
                    // End TT#939
                    // end MID Track 6255 Enter max "1,000" and get error "must be integer" 
					if (cellValue < 0)
					{
						errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MustBeNonNegative);
						errorFound = true;
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
			{
				gridCell.Appearance.Image = ErrorImage;
				gridCell.Tag = errorMessage;
				return false;
			}
			else
			{
				gridCell.Appearance.Image = null;
				//gridCell.Tag = null;
				gridCell.Tag = _allColorsMaxDefaultText;
				return true;
			}
		}
		
		private bool AllColorsValid(UltraGridCell gridCell, ref string errorMessage)
		{
			bool errorFound = false;
			try
			{
				if (gridCell.Text.Trim().Length > 0 && gridCell.Row.Cells["Maximum"].Text.Trim().Length > 0)
				{
                    // begin MID Track 6255 Enter max "1,000" and get error "must be integer" 
					//if (Convert.ToInt32(gridCell.Text, CultureInfo.CurrentUICulture) > Convert.ToInt32(gridCell.Row.Cells["Maximum"].Text, CultureInfo.CurrentUICulture))
                    // Begin TT#939 - JSmith - Allocation Override -> will not clear fields in grid on store grade tab, change attribute set and get System Object
                    //if ((int)gridCell.Value > (int)gridCell.Row.Cells["Maximum"].Value)
                    if (Convert.ToInt32(gridCell.Text.Trim().Replace(",", null), CultureInfo.CurrentUICulture) > Convert.ToInt32(gridCell.Row.Cells["Maximum"].Text.Trim().Replace(",", null), CultureInfo.CurrentUICulture))
                    // End TT#939
                    // end MID Track 6255 Enter max "1,000" and get error "must be integer" 
                    {
						errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MinColorNotLessThanMax);
						errorFound = true;
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
			{
				gridCell.Appearance.Image = ErrorImage;
				gridCell.Tag = errorMessage;
				gridCell.Row.Cells["Maximum"].Appearance.Image = ErrorImage;
				gridCell.Row.Cells["Maximum"].Tag = errorMessage;
				return false;
			}
			else
			{
				gridCell.Appearance.Image = null;
				gridCell.Tag = _allColorsMinDefaultText;
				gridCell.Row.Cells["Maximum"].Appearance.Image = null;
				gridCell.Row.Cells["Maximum"].Tag = _allColorsMaxDefaultText;
				return true;
			}
		}
		
		private bool ColorValid(UltraGridCell gridCell, ref string errorMessage)
		{
			bool errorFound = false;
			try
			{
				if (gridCell.Text.Length == 0)	// cell is empty
				{
					errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_FieldIsRequired);
					errorFound = true;
				}
				else
				{
					// make sure colors are unique
					foreach(  UltraGridRow gridRow in this.ugColor.Rows )
					{
						if (gridCell.Row != gridRow)
						{
							if ((string)gridCell.Text == (string)gridRow.Cells["Color"].Text)
							{
								if  (!_dupColorFound)
								{
									errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_DuplicateColorNotAllowed);
									_dupColorFound = true;	
								}
								else
								{
									errorMessage = string.Empty;
								}
								errorFound = true;
								break;
							}
						}
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
	
		private bool MinimumValid(UltraGridCell gridCell, ref string errorMessage)
		{
			bool errorFound = false;
			try
			{
				if (gridCell.Text.Length > 0)
				{
                    // begin MID Track 6255 Enter max "1,000" and get error "must be integer" 
					//int cellValue = Convert.ToInt32(gridCell.Text, CultureInfo.CurrentUICulture);
                    // Begin TT#939 - JSmith - Allocation Override -> will not clear fields in grid on store grade tab, change attribute set and get System Object
                    //int cellValue = (int)gridCell.Value;
                    int cellValue = Convert.ToInt32(gridCell.Text.Trim().Replace(",", null), CultureInfo.CurrentUICulture);
                    // End TT#939
                    // end MID Track 6255 Enter max "1,000" and get error "must be integer" 
					if (cellValue < 0)
					{
						errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MustBeNonNegative);
						errorFound = true;
					}
					else if (gridCell.Row.Cells["Maximum"].Text.Length > 0)
					{
                        // begin MID Track 6255 Enter max "1,000" and get error "must be integer" 
						//if (Convert.ToInt32(gridCell.Row.Cells["Maximum"].Text, CultureInfo.CurrentUICulture) < cellValue)
						if ((int)gridCell.Row.Cells["Maximum"].Value < cellValue)
                        // end MID Track 6255 Enter max "1,000" and get error "must be integer" 
                        {
							//errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MinColorNotLessThanMax);
							errorFound = true;
						}
					}
				}
			}
			catch( Exception error)
			{
				string exceptionMessage = error.Message;
				errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MustBeInteger);
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

		private bool MaximumValid(UltraGridCell gridCell, ref string errorMessage)
		{
			bool errorFound = false;
			try
			{
				if (gridCell.Text.Length > 0)	// cell is not empty
				{
                    // begin MID Track 6255 Enter max "1,000" and get error "must be integer" 
					//int cellValue = Convert.ToInt32(gridCell.Text, CultureInfo.CurrentUICulture);
                    // Begin TT#939 - JSmith - Allocation Override -> will not clear fields in grid on store grade tab, change attribute set and get System Object
                    //int cellValue = (int)gridCell.Value;
                    int cellValue = Convert.ToInt32(gridCell.Text.Trim().Replace(",", null), CultureInfo.CurrentUICulture);
                    // End TT#939
                    // end MID Track 6255 Enter max "1,000" and get error "must be integer" 
					if (cellValue < 0)
					{
						errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MustBeNonNegative);
						errorFound = true;
					}
					else if (gridCell.Row.Cells["Minimum"].Text.Length != 0)
						// validate against color min if entered
					{
                        // begin MID Track 6255 Enter max "1,000" and get error "must be integer" 
						//if (Convert.ToInt32(gridCell.Row.Cells["Minimum"].Text, CultureInfo.CurrentUICulture) > cellValue)
						if ((int)gridCell.Row.Cells["Minimum"].Value > cellValue)
                        // end MID Track 6255 Enter max "1,000" and get error "must be integer" 
                        {
							errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MinColorNotLessThanMax);
							errorFound = true;
							gridCell.Row.Cells["Minimum"].Appearance.Image = ErrorImage;
							gridCell.Row.Cells["Minimum"].Tag = errorMessage;
						}
						else
						{
							gridCell.Row.Cells["Minimum"].Appearance.Image = null;
							gridCell.Row.Cells["Minimum"].Tag = null;
						}
					}
				}
			}
			catch( Exception error)
			{
				string exceptionMessage = error.Message;
				errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MustBeInteger);
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
       
        // Begin TT#3154 - RMatelic - Allocation override color min max - right click and insert before or after receive a null reference exception
        private void ugColor_AfterRowInsert(object sender, RowEventArgs e)
        {
            try
            {
                if (!_fromMenuInsert)
                {
                    e.Row.Cells["RowPosition"].Value = this.ugColor.Rows.Count + 1;
                }
            }
            catch (Exception exception)
            {
                HandleException(exception);
            }
        }
        // End TT#3154
 
		private void ugColor_CellChange(object sender, Infragistics.Win.UltraWinGrid.CellEventArgs e)
		{
			try
			{
				if (_colorIsPopulated)
				{
					switch (this.ugColor.ActiveCell.Column.Key)
					{
						case "Color":
							ColorChangesMade = true;
							break;
						case "Minimum":
						case "Maximum":
							ColorMinMaxesChangesMade = true;
							break;
					}
				}
			}
			catch( Exception exception )
			{
				HandleException(exception);
			}
		}
		
		private void ugColor_BeforeRowUpdate(object sender, Infragistics.Win.UltraWinGrid.CancelableRowEventArgs e)
		{	
			try
			{
				// if row is completely empty, cancel the update
				if (e.Row.Cells["Color"].Text.Length == 0 &&
					e.Row.Cells["Minimum"].Text.Length == 0 &&
					e.Row.Cells["Maximum"].Text.Length == 0)
				{
					e.Cancel = true;
				}
			}
			catch( Exception exception )
			{
				HandleException(exception);
			}
		
		}

		#endregion
		#region Capacity Tab
		private bool ValidTabCapacity(ref EditMsgs em)
		{
			try
			{
				string errorMessage = null;
				foreach(  UltraGridRow gridRow in ugCapacity.Rows )
				{
					if (!ExceedCapacityValid(gridRow.Cells["Exceed by %"], ref errorMessage))
					{
						if (errorMessage != string.Empty)
							em.AddMsg(eMIDMessageLevel.Edit, errorMessage,  this.ToString());
					}
				}
	
				if (em.ErrorFound)
				{
					return false;
				}
				else
				{
					return true;
				}
			}
			catch(Exception ex)
			{
				HandleException(ex);
			}
			
			return false;
		}

		private bool ExceedCapacityValid(UltraGridCell gridCell, ref string errorMessage)
		{
			bool errorFound = false;
			try
			{
				if (gridCell.Text != string.Empty)
				{	
					if (!Convert.ToBoolean(gridCell.Row.Cells["Exceed Capacity"].Value, CultureInfo.CurrentUICulture))
					{
						errorMessage = "Check box must be checked if Exceed by % present.";
						errorFound = true;
					}
					else
					{
						double outVal = Convert.ToDouble(gridCell.Text, CultureInfo.CurrentUICulture);
								
						if (outVal < -100.0 )
						{
							errorMessage = string.Format(MIDText.GetText(eMIDTextCode.msg_al_MinimumValueExceeded),
								gridCell.Text,"-100.00");
							errorFound = true;
						}
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
			{
				gridCell.Appearance.Image = ErrorImage;
				gridCell.Tag = errorMessage;
				//gridCell.Row.Cells["Exceed Capacity"].Appearance.Image = ErrorImage;
				gridCell.Row.Cells["Exceed Capacity"].Tag = errorMessage;
				return false;
			}
			else
			{
				gridCell.Appearance.Image = null;
				gridCell.Tag = null;
				//gridCell.Row.Cells["Exceed Capacity"].Appearance.Image  = null;
				gridCell.Row.Cells["Exceed Capacity"].Tag = null;
				return true;
			}
		}
	
		#endregion
        // BEGIN TT#616 - AGallagher - Allocation - Pack Rounding (#67)
        #region PackRounding Tab
        private bool ValidTabPackRounding(ref EditMsgs em)
        {
            try
            {
                string errorMessage = null;
                foreach (UltraGridRow gridRow in ugPackRounding.Rows)
                {
                    {
                        if (gridRow.Cells["PackText"].Text.Length > 0)
                            if (gridRow.Cells["PackMultiple"].Text.Length < 1)
                            { gridRow.Cells["PackMultiple"].Value = gridRow.Cells["PackText"].Value; }
                    }
					// Begin TT#616 - stodd - pack rounding
					//{
					//    if (gridRow.Cells["FstPack"].Text.Length < 1)
					//    { gridRow.Cells["FstPack"].Value = SAB.ApplicationServerSession.GlobalOptions.GenericPackRounding1stPackPct; }
					//}
					//{
					//    if (gridRow.Cells["NthPack"].Text.Length < 1)
					//    { gridRow.Cells["NthPack"].Value = SAB.ApplicationServerSession.GlobalOptions.GenericPackRoundingNthPackPct; }
					//}
					// End TT#616 - stodd - pack rounding
                    {
                        if (!ExceedPackRoundingFstValid(gridRow.Cells["FstPack"], ref errorMessage))
                        {
                            if (errorMessage != string.Empty)
                                em.AddMsg(eMIDMessageLevel.Edit, errorMessage, this.ToString());
                        }
                        if (!ExceedPackRoundingNthValid(gridRow.Cells["NthPack"], ref errorMessage))
                        {
                            if (errorMessage != string.Empty)
                                em.AddMsg(eMIDMessageLevel.Edit, errorMessage, this.ToString());
                        }
                    }
                }

                if (em.ErrorFound)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return false;
        }
        private bool ExceedPackRoundingFstValid(UltraGridCell gridCell, ref string errorMessage)
		{
			bool errorFound = false;
            try
            {
                string inStr = (gridCell.Row.Cells["FstPack"].Text.ToString(CultureInfo.CurrentUICulture));
                if (inStr == null || inStr.Length == 0)
                {
                }
                else
                {
                    gridCell.Appearance.Image = null;
                    gridCell.Tag = null;
                    gridCell.Row.Cells["FstPack"].Tag = null;
					// Begin TT#616 - stodd - pack rounding
                    double outVal = Convert.ToDouble(inStr, CultureInfo.CurrentUICulture);
					// End TT#616 - stodd - pack rounding

                    if (outVal < 0.0 || outVal > 100.0)
                    {
                        errorMessage = "Please enter 0.0 to 100.0 Percent";
                        errorFound = true;
                        gridCell.Appearance.Image = ErrorImage;
                        gridCell.Tag = errorMessage;
                        gridCell.Row.Cells["FstPack"].Tag = errorMessage;
                    }
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
				return false;
			}
			else
			{
				return true;
			}
		}
        private bool ExceedPackRoundingNthValid(UltraGridCell gridCell, ref string errorMessage)
        {
            bool errorFound = false;
            try
            {
                string inStr = (gridCell.Row.Cells["NthPack"].Text.ToString(CultureInfo.CurrentUICulture));
                if (inStr == null || inStr.Length == 0)
                {
                }
                else
                {
                    gridCell.Appearance.Image = null;
                    gridCell.Tag = null;
                    gridCell.Row.Cells["NthPack"].Tag = null;
                    double outVal = System.Math.Abs(Convert.ToDouble(inStr, CultureInfo.CurrentUICulture));

                    if (outVal < 0.0 || outVal > 100.0)
                    {
                        errorMessage = "Please enter 0.0 to 100.0 Percent";
                        errorFound = true;
                        gridCell.Appearance.Image = ErrorImage;
                        gridCell.Tag = errorMessage;
                        gridCell.Row.Cells["NthPack"].Tag = errorMessage;
                    }
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
                return false;
            }
            else
            {
                return true;
            }
        }

        #endregion
        // END TT#616 - AGallagher - Allocation - Pack Rounding (#67)
		private void txtStoreGradeTime_TextChanged(object sender, System.EventArgs e)
		{
			//if (txtStoreGradeTime.TextLength == 0)
			//	txtStoreGradeTime.Text = SAB.ApplicationServerSession.GlobalOptions.StoreGradePeriod.ToString();	
		}

		private void ugCapacity_CellChange(object sender, Infragistics.Win.UltraWinGrid.CellEventArgs e)
		{
			try
			{
				switch (this.ugCapacity.ActiveCell.Column.Key)
				{
					case "Exceed Capacity":
					case "Exceed by %":
						CapacityChangesMade = true;
						break;
				}
			}
			catch(Exception ex)
			{
				HandleException(ex);
			}
		
		}

		private void ugCapacity_MouseEnterElement(object sender, Infragistics.Win.UIElementEventArgs e)
		{
			try
			{
				ShowUltraGridToolTip(ugCapacity, e);
			}
			catch(Exception ex)
			{
				HandleException(ex);
			}
		}

		private void ugAllColors_MouseEnterElement(object sender, Infragistics.Win.UIElementEventArgs e)
		{
			try
			{
				ShowUltraGridToolTip(ugAllColors, e);
			}
			catch(Exception ex)
			{
				HandleException(ex);
			}
		}

		private void ugColor_MouseEnterElement(object sender, Infragistics.Win.UIElementEventArgs e)
		{
			try
			{
				ShowUltraGridToolTip(ugColor, e);
			}
			catch(Exception ex)
			{
				HandleException(ex);
			}
		}

		private void ugAllColors_CellChange(object sender, Infragistics.Win.UltraWinGrid.CellEventArgs e)
		{
			try
			{
				switch (this.ugAllColors.ActiveCell.Column.Key)
				{
					case "Minimum":
						ugAllColors.ActiveCell.Tag = _allColorsMinDefaultText;	
						ColorMinMaxesChangesMade = true;
						break;
					case "Maximum":
						ugAllColors.ActiveCell.Tag = _allColorsMaxDefaultText;	
						ColorMinMaxesChangesMade = true;
						break;
				}
			}
			catch(Exception ex)
			{
				HandleException(ex);
			}
		}
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
//			catch (Exception ex)
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

		private void txtOHFactor_TextChanged(object sender, System.EventArgs e)
		{
			// BEGIN MID Track #3218 - V26 - OH flips to lower level of hier when keying in % Factor
			// changed logic and moved to txtOHFactor_Vaidating event
			//string inStr = txtOHFactor.Text.ToString(CultureInfo.CurrentUICulture);
			//if (inStr.Trim().Length > 0) 
			//	 cboOnHand.SelectedValue = _styleKey;
			// END MID Track #3218
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
                al = GetStoreGroupList(_allocationOverrideMethod.Method_Change_Type, _allocationOverrideMethod.GlobalUserType, true);
                cboStoreAttribute.Initialize(SAB, FunctionSecurity, al.ArrayList, _allocationOverrideMethod.GlobalUserType == eGlobalUserType.User);
                if (currValue != Include.NoRID)
                {
                    cboStoreAttribute.SelectedValue = currValue;
                }
            }
            catch
            {
                throw;
            }
            // BEGIN TT#618 - AGallagher - Allocation Override - Add Attribute Sets (#35)
            ProfileList bl;
            int currValue2;
            try
            {
                if (cboSGStoreAttribute.SelectedValue != null &&
                    cboSGStoreAttribute.SelectedValue.GetType() == typeof(System.Int32))
                {
                    currValue2 = Convert.ToInt32(cboSGStoreAttribute.SelectedValue);
                }
                else
                {
                    currValue2 = Include.NoRID;
                }
                bl = GetStoreGroupList(_allocationOverrideMethod.Method_Change_Type, _allocationOverrideMethod.GlobalUserType, true);
                cboSGStoreAttribute.Initialize(SAB, FunctionSecurity, bl.ArrayList, _allocationOverrideMethod.GlobalUserType == eGlobalUserType.User);
                if (currValue2 != Include.NoRID)
                {
                    cboSGStoreAttribute.SelectedValue = currValue2;
                }
            }
            catch
            {
                throw;
            }
            // BEGIN TT#618 - AGallagher - Allocation Override - Add Attribute Sets (#35)
        }
        // End Track #4872

        override protected bool ApplySecurity()	// track 5871 stodd
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
			return securityOk;	// track 5871 stodd
		}

        void cboOverrideFilter_MIDComboBoxPropertiesChangedEvent(object source, MIDComboBoxPropertiesChangedEventArgs args)
        {
            this.cboOverrideFilter_SelectionChangeCommitted(source, new EventArgs());
        }

		private void cboOverrideFilter_SelectionChangeCommitted(object sender, System.EventArgs e)
		{
			try
			{
				int filterRID = ((FilterNameCombo)cboOverrideFilter.SelectedItem).FilterRID;
			 
				if (filterRID == Include.NoRID)
					_allocationOverrideMethod.StoreFilterRID = Include.UndefinedStoreFilter;
				else
					_allocationOverrideMethod.StoreFilterRID =	((FilterNameCombo)cboOverrideFilter.SelectedItem).FilterRID;
	
			}
			catch
			{
				throw;
			}
		
		}

		private void cboOnHand_SelectionChangeCommitted(object sender, System.EventArgs e)
		{
			// BEGIN MID Track #3218 - V26 - OH flips to lower level of hier when keying in % Factor
			if (FormLoaded &&
				!FormIsClosing)
			{
                //_askQuestion = false; // TT#3146 - RMatelic - Allocation override mssg when updating the Basis information 80427. Would not expect to get the message>> remove question 
				// END MID Track #3218 
				_lastMerchIndex = cboOnHand.SelectedIndex;
				ErrorProvider.SetError(cboOnHand, string.Empty);
				ChangePending = true;
			}
		}

        private void ugAllColors_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            //Begin TT#169 - JSmith - enter max of 1000 and receive 'must be integer'
            //MIDRetail.Windows.Controls.UltraGridLayoutDefaults ugld = new MIDRetail.Windows.Controls.UltraGridLayoutDefaults();
            //ugld.ApplyDefaults(e);
            MIDRetail.Windows.Controls.UltraGridLayoutDefaults ugld = new MIDRetail.Windows.Controls.UltraGridLayoutDefaults(ErrorImage);
            // Begin TT#1164 - JSmith - When going in to some methods or Admin features the columns are appearing too wide.
            //ugld.ApplyDefaults((Infragistics.Win.UltraWinGrid.UltraGrid)sender, e, true, 2, true);
            ugld.ApplyDefaults((Infragistics.Win.UltraWinGrid.UltraGrid)sender, e, true, 2, false);
            //End TT#169
        }

        private void uddColors_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            //Begin TT#169 - JSmith - enter max of 1000 and receive 'must be integer'
            //MIDRetail.Windows.Controls.UltraGridLayoutDefaults ugld = new MIDRetail.Windows.Controls.UltraGridLayoutDefaults();
            MIDRetail.Windows.Controls.UltraGridLayoutDefaults ugld = new MIDRetail.Windows.Controls.UltraGridLayoutDefaults(ErrorImage);
            //End TT#169
            // Begin TT#3144 - RMatelic - Alloc override - color tab - Add color min-max and drop down cuts off the color and description fields
            //ugld.ApplyDefaults(e, true);
            ugld.ApplyDefaults(e, false);
            // End TT#3144
        }

		private void cboOTSPlan_SelectionChangeCommitted(object sender, System.EventArgs e)
		{
			if (FormLoaded &&
				!FormIsClosing)
			{
				ErrorProvider.SetError(cboOTSPlan, string.Empty);
				_lastMerchIndex = cboOTSPlan.SelectedIndex;
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

        private void ugPackRounding_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {

        }

        private void gbxSettings_Enter(object sender, EventArgs e)
        {

        }

        private void lblNeedLimit_Click(object sender, EventArgs e)
        {

        }

        private void cbxExceedMax_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void lblFilter_Click(object sender, EventArgs e)
        {

        }

        // BEGIN TT#1287 - AGallagher - Inventory Min/Max
        private void radAllocationMinMax_CheckedChanged(object sender, System.EventArgs e)
        {
            if (FormLoaded && (radAllocationMinMax.Checked == true) && (radAllocationMinMax.Focused == false))
            {
                _InventoryInd = 'A';
                cboInventoryBasis.Enabled = false;
                cboInventoryBasis.Text = null;
            }
            if (FormLoaded && (radAllocationMinMax.Checked == true) && (radAllocationMinMax.Focused == true))
            {
                _InventoryInd = 'A';
                cboInventoryBasis.Enabled = false;
                cboInventoryBasis.Text = null;
            }
        }

        private void radInventoryMinMax_CheckedChanged(object sender, System.EventArgs e)
        {
            if (FormLoaded && (radInventoryMinMax.Checked == true) && (radInventoryMinMax.Focused == false))
            {
                _InventoryInd = 'I';
                cboInventoryBasis.Enabled = true;
                LoadMerchandiseCombo();
                LoadGenAllocValues();
            }
            if (FormLoaded && (radInventoryMinMax.Checked == true) && (radInventoryMinMax.Focused == true))
            {
                _InventoryInd = 'I';
                cboInventoryBasis.Enabled = true;
                LoadMerchandiseCombo();
                LoadGenAllocValues();
            }
        }
        // END TT#1287 - AGallagher - Inventory Min/Max

        // BEGIN TT#1287 - AGallagher - Inventory Min/Max
        private void cboInventoryBasis_SelectionChangeCommitted(object sender, System.EventArgs e)
        {
            if (FormLoaded &&
                !FormIsClosing)
            {
                ErrorProvider.SetError(cboInventoryBasis, string.Empty);
                _lastMerchIndexIB = cboInventoryBasis.SelectedIndex;
                ChangePending = true;
            }
        }
        #region "DragNDrop Node to cboInventoryBasis"
        private void cboInventoryBasis_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
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

        private void cboInventoryBasis_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            string errorMessage;

            try
            {
                if (cboInventoryBasis.Text == string.Empty)
                {
                    cboInventoryBasis.SelectedIndex = _lastMerchIndexIB;
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

        private void cboInventoryBasis_Validated(object sender, System.EventArgs e)
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
        private void cboInventoryBasis_DragDrop(object sender, System.Windows.Forms.DragEventArgs e)
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
                }
                else
                {
                    cboInventoryBasis.SelectedIndex = levIndex;
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void SetComboToLevel(int seq)
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
                        cboInventoryBasis.SelectedIndex = levIndex;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

        }

        private void cboInventoryBasis_DragEnter(object sender, System.Windows.Forms.DragEventArgs e)
        {
            // Begin TT#296-MD - JSmith - Methods and Workflows can be dragged and dropped into Inventory Basis drop down.
            Merchandise_DragEnter(sender, e);
            //try
            //{
            //    ObjectDragEnter(e);
            //    Image_DragEnter(sender, e);
            //}
            //catch (Exception ex)
            //{
            //    HandleException(ex);
            //}
            // End TT#296-MD - JSmith - Methods and Workflows can be dragged and dropped into Inventory Basis drop down.
        }

        private void cboInventoryBasis_DragOver(object sender, DragEventArgs e)
        {
            // Begin TT#296-MD - JSmith - Methods and Workflows can be dragged and dropped into Inventory Basis drop down.
            Merchandise_DragOver(sender, e);
            //Image_DragOver(sender, e);
            // End TT#296-MD - JSmith - Methods and Workflows can be dragged and dropped into Inventory Basis drop down.
        }

        #endregion
        // END TT#1287 - AGallagher - Inventory Min/Max

        // BEGIN TT#1401 - GTaylor - Reservation Stores
        private void cbxRsrvDoNotApplyVSW_CheckedChanged(object sender, EventArgs e)
        {
            // Begin TT#2731 - JSmith - Unable to copy allocation override method from global
            //this._applyVSW = !this.cbxRsrvDoNotApplyVSW.Checked;
            _allocationOverrideMethod.ApplyVSW = !this.cbxRsrvDoNotApplyVSW.Checked;
            // End TT#2731 - JSmith - Unable to copy allocation override method from global
            this.ugReservation.Enabled = !this.cbxRsrvDoNotApplyVSW.Checked;
        }

        private void midAttributeCbx_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (FormLoaded)
            {
                // Begin TT#2731 - JSmith - Unable to copy allocation override method from global
                //_imoGroupLevelList = SAB.StoreServerSession.GetStoreGroupLevelListViewList(Convert.ToInt32(midAttributeCbx.SelectedValue, CultureInfo.CurrentUICulture), true);
                _allocationOverrideMethod.IMOGroupLevelList = StoreMgmt.StoreGroup_GetLevelListViewList(Convert.ToInt32(midAttributeCbx.SelectedValue, CultureInfo.CurrentUICulture), true); //SAB.StoreServerSession.GetStoreGroupLevelListViewList(Convert.ToInt32(midAttributeCbx.SelectedValue, CultureInfo.CurrentUICulture), true);
                // End TT#2731 - JSmith - Unable to copy allocation override method from global
                Reservation_Populate(Include.NoRID, true);
            }
        }
        // BEGIN TT#1401 - GTaylor - Reservation Stores

        //private void ugPackRounding_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        //{

        //}

        private void cmbSGAttributeSet_MIDComboBoxPropertiesChangedEvent(object source, MIDComboBoxPropertiesChangedEventArgs args)
        {
            this.cmbSGAttributeSet_SelectionChangeCommitted(source, new EventArgs());
        }
        private void cboInventoryBasis_MIDComboBoxPropertiesChangedEvent(object source, MIDComboBoxPropertiesChangedEventArgs args)
        {
            this.cboInventoryBasis_SelectionChangeCommitted(source, new EventArgs());
        }
        private void cboSGStoreAttribute_MIDComboBoxPropertiesChangedEvent(object source, MIDComboBoxPropertiesChangedEventArgs args)
        {
            this.cboSGStoreAttribute_SelectionChangeCommitted(source, new EventArgs());
        }
        private void cboStoreAttribute_MIDComboBoxPropertiesChangedEvent(object source, MIDComboBoxPropertiesChangedEventArgs args)
        {
            this.cboStoreAttribute_SelectionChangeCommitted(source, new EventArgs());
        }
        private void midAttributeCbx_MIDComboBoxPropertiesChangedEvent(object source, MIDComboBoxPropertiesChangedEventArgs args)
        {
            this.midAttributeCbx_SelectionChangeCommitted(source, new EventArgs());
        }
        private void cboOnHand_MIDComboBoxPropertiesChangedEvent(object source, MIDComboBoxPropertiesChangedEventArgs args)
        {
            this.cboOnHand_SelectionChangeCommitted(source, new EventArgs());
        }
        private void cboOTSPlan_MIDComboBoxPropertiesChangedEvent(object source, MIDComboBoxPropertiesChangedEventArgs args)
        {
            this.cboOTSPlan_SelectionChangeCommitted(source, new EventArgs());
        }
	}
}

