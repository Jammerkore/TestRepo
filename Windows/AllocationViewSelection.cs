// BEGIN MID Track #5170 - JSmith - Model enhancements
// Too many lines changed to mark.  Use SCM Compare for details.
//End Track #5170cboGroupBy
using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic; // TT#1185 - Verify ENQ before Update
using System.ComponentModel;
using System.Globalization;
using System.Windows.Forms;
using System.Diagnostics;
using System.Data;
using System.IO;
using System.Configuration;

using Infragistics.Win.UltraWinGrid;
using Infragistics.Shared;
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
	/// Summary description for AllocationViewSelection.
	/// </summary>
	public class AllocationViewSelection : MIDFormBase
	{
		#region Windows-Generated Stuff

        // Begin Track #4872 - JSmith - Global/User Attributes
        private MIDAttributeComboBox cboStoreAttribute;
        // End Track #4872
		private System.Windows.Forms.RadioButton rbStyleView;
		private System.Windows.Forms.RadioButton rbSizeView;
		private System.Windows.Forms.RadioButton rbSummaryView;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.TextBox txtBasis;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.ToolTip toolTip1;
		private System.Windows.Forms.ContextMenu mnuHeaderGrid;
		private System.Windows.Forms.ContextMenu mnuGridColHeader;
		private MIDRetail.Windows.Controls.MIDDateRangeSelector drsBegin;
		private MIDRetail.Windows.Controls.MIDDateRangeSelector drsEnd;
		private Infragistics.Win.UltraWinGrid.UltraGrid ugHeader;
		private System.Windows.Forms.CheckBox chkIneligibleStores;
		private System.Windows.Forms.Label lblAttribute;
		private System.Windows.Forms.Label lblFilter;
		private System.Windows.Forms.Label lblView;
		private System.Windows.Forms.Label lblGroupBy;
		private System.Windows.Forms.Label lblMerchandise;
		private System.Windows.Forms.GroupBox gbxDisplay;
		private System.Windows.Forms.GroupBox gbxNeedAnalysis;
		private System.Windows.Forms.Label lblTimeEnd;
		private System.Windows.Forms.Label lblTimeBegin;
		private System.Windows.Forms.GroupBox gbxAllocHeaders;
		private System.Windows.Forms.CheckBox cbxAnalysis;
        private System.Windows.Forms.Label lblSizeCurve;
		private System.Windows.Forms.Label lblSizeConstraint;
		private System.Windows.Forms.Label lblSizeAlternate;
		private MIDRetail.Windows.Controls.MIDComboBoxEnh cboConstraints;
		private MIDRetail.Windows.Controls.MIDComboBoxEnh cboAlternates;
		protected System.Windows.Forms.PictureBox picBoxCurve;
		protected System.Windows.Forms.PictureBox picBoxAlternate;
		protected System.Windows.Forms.PictureBox picBoxConstraint;
        private GroupBox gbxBasis;
        private UltraGrid ugBasisNodeVersion;
        private ContextMenuStrip cmsBasis;
        private ToolStripMenuItem cmsBasisItemInsert;
        private ToolStripMenuItem cmsBasisItemDelete;
        private ToolStripMenuItem cmsBasisItemDeleteAll;
        private ContextMenuStrip cmsBasisInsertMenu;
        private ToolStripMenuItem cmsBasisItemInsertBefore;
        private ToolStripMenuItem cmsBasisItemInsertAfter;
        private Label lblStore;
        private MIDComboBoxEnh cboFilter;
        private MIDComboBoxEnh cboGroupBy;
        private MIDComboBoxEnh cboView;
        private MIDComboBoxEnh cboStore;
        private MIDComboBoxEnh cboSizeCurve;
		private System.ComponentModel.IContainer components;

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
				//remove handlers
				this.cboFilter.SelectionChangeCommitted -= new System.EventHandler(this.cboFilter_SelectionChangeCommitted);
                //Begin TT#316 - MD - DOConnell - Replace all Windows Combobox controls with new enhanced control
                this.cboFilter.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler(cboFilter_MIDComboBoxPropertiesChangedEvent);
                //End TT#316 - MD - DOConnell - Replace all Windows Combobox controls with new enhanced control
                this.cboFilter.DragEnter -= new System.Windows.Forms.DragEventHandler(this.cboFilter_DragEnter);
                this.cboFilter.DragDrop -= new System.Windows.Forms.DragEventHandler(this.cboFilter_DragDrop);
                this.cboFilter.DragOver -= new System.Windows.Forms.DragEventHandler(this.cboFilter_DragOver);
                this.cboGroupBy.SelectionChangeCommitted -= new System.EventHandler(this.cboGroupBy_SelectionChangeCommitted);
				this.cboStoreAttribute.SelectionChangeCommitted -= new System.EventHandler(this.cboStoreAttribute_SelectionChangeCommitted);
                this.cboStoreAttribute.DragDrop -= new System.Windows.Forms.DragEventHandler(this.cboStoreAttribute_DragDrop);
                this.cboStoreAttribute.DragEnter -= new System.Windows.Forms.DragEventHandler(this.cboStoreAttribute_DragEnter);
                // Begin TT#3513 - JSmith - Clean Up Memory Leaks
                this.cboStoreAttribute.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler(cboStoreAttribute_MIDComboBoxPropertiesChangedEvent);
                this.cboStoreAttribute.DragOver -= new System.Windows.Forms.DragEventHandler(this.cboStoreAttribute_DragOver);
                this.cmsBasis.Opening += new System.ComponentModel.CancelEventHandler(this.cmsBasis_Opening);
                this.cmsBasisItemInsertBefore.Click -= new System.EventHandler(this.cmsBasisItemInsertBefore_Click);
                this.cmsBasisItemInsertAfter.Click -= new System.EventHandler(this.cmsBasisItemInsertAfter_Click);
                this.cmsBasisItemDelete.Click -= new System.EventHandler(this.cmsBasisItemDelete_Click);
                this.cmsBasisItemDeleteAll.Click -= new System.EventHandler(this.cmsBasisItemDeleteAll_Click);
                // End TT#3513 - JSmith - Clean Up Memory Leaks
                this.chkIneligibleStores.CheckedChanged -= new System.EventHandler(this.chkIneligibleStores_CheckedChanged);
				this.cbxAnalysis.CheckedChanged -= new System.EventHandler(this.cbxAnalysis_CheckedChanged);
				this.rbStyleView.CheckedChanged -= new System.EventHandler(this.rbStyleView_CheckedChanged);
				this.rbSizeView.CheckedChanged -= new System.EventHandler(this.rbSizeView_CheckedChanged);
				this.rbSummaryView.CheckedChanged -= new System.EventHandler(this.rbSummaryView_CheckedChanged);
				this.drsEnd.Click -= new System.EventHandler(this.drsEnd_Click);
				this.drsEnd.Load -= new System.EventHandler(this.drsEnd_Load);
				this.drsEnd.OnSelection -= new MIDRetail.Windows.Controls.MIDDateRangeSelector.SelectionEventHandler(this.drsEnd_OnSelection);
				this.drsBegin.Click -= new System.EventHandler(this.drsBegin_Click);
				this.drsBegin.Load -= new System.EventHandler(this.drsBegin_Load);
				this.drsBegin.OnSelection -= new MIDRetail.Windows.Controls.MIDDateRangeSelector.SelectionEventHandler(this.drsBegin_OnSelection);
				this.txtBasis.Validating -= new System.ComponentModel.CancelEventHandler(this.txtBasis_Validating);
                this.txtBasis.Validated -= new System.EventHandler(this.txtBasis_Validated);
                this.txtBasis.DragDrop -= new System.Windows.Forms.DragEventHandler(this.txtBasis_DragDrop);
				this.txtBasis.TextChanged -= new System.EventHandler(this.txtBasis_TextChanged);
				this.txtBasis.DragEnter -= new System.Windows.Forms.DragEventHandler(this.txtBasis_DragEnter);
                this.txtBasis.DragOver -= new System.Windows.Forms.DragEventHandler(this.txtBasis_DragOver);  // TT#3513 - JSmith - Clean Up Memory Leaks
				this.btnOK.Click -= new System.EventHandler(this.btnOK_Click);
				this.btnCancel.Click -= new System.EventHandler(this.btnCancel_Click);
				this.ugHeader.BeforeExitEditMode -= new Infragistics.Win.UltraWinGrid.BeforeExitEditModeEventHandler(this.ugHeader_BeforeExitEditMode);
				this.ugHeader.AfterRowsDeleted -= new System.EventHandler(this.ugHeader_AfterRowsDeleted);
				this.ugHeader.AfterRowInsert -= new Infragistics.Win.UltraWinGrid.RowEventHandler(this.ugHeader_AfterRowInsert);
				this.ugHeader.BeforeRowsDeleted -= new Infragistics.Win.UltraWinGrid.BeforeRowsDeletedEventHandler(this.ugHeader_BeforeRowsDeleted);
				this.ugHeader.KeyDown -= new System.Windows.Forms.KeyEventHandler(this.ugHeader_KeyDown);
				this.ugHeader.InitializeLayout -= new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.ugHeader_InitializeLayout);
                //Begin TT#169 - JSmith - enter max of 1000 and receive 'must be integer'
                MIDRetail.Windows.Controls.UltraGridLayoutDefaults ugld = new MIDRetail.Windows.Controls.UltraGridLayoutDefaults(ErrorImage);
                ugld.DetachGridEventHandlers(ugHeader);
                //End TT#169
				this.Load -= new System.EventHandler(this.AllocationViewSelection_Load);
				// BEGIN MID Track #4567 - John Smith - add filtering
				this.picBoxConstraint.Click -= new System.EventHandler(this.picBox_Click);
				this.picBoxAlternate.Click -= new System.EventHandler(this.picBox_Click);
				this.picBoxCurve.Click -= new System.EventHandler(this.picBox_Click);
				// End MID Track #4567
             
                // Begin TT#456 - RMatelic - Add Views to Size Review 
                this.cboView.SelectionChangeCommitted -= new System.EventHandler(this.cboView_SelectionChangeCommitted);
                //Begin TT#316 - MD - DOConnell - Replace all Windows Combobox controls with new enhanced control
                this.cboView.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler(this.cboView_MIDComboBoxPropertiesChangedEvent);
                //End TT#316 - MD - DOConnell - Replace all Windows Combobox controls with new enhanced control
                // End TT#456

                // Begin TT#952 - RMatelic - MIDFormBase.cs error on Allocation View Selection
                this.ugBasisNodeVersion.ClickCellButton -= new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ugBasisNodeVersion_ClickCellButton);
                this.ugBasisNodeVersion.InitializeLayout -= new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.ugBasisNodeVersion_InitializeLayout);
                this.ugBasisNodeVersion.DragOver -= new System.Windows.Forms.DragEventHandler(this.ugBasisNodeVersion_DragOver);
                this.ugBasisNodeVersion.AfterCellUpdate -= new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ugBasisNodeVersion_AfterCellUpdate);
                this.ugBasisNodeVersion.DragDrop -= new System.Windows.Forms.DragEventHandler(this.ugBasisNodeVersion_DragDrop);
                this.ugBasisNodeVersion.DragEnter -= new System.Windows.Forms.DragEventHandler(this.ugBasisNodeVersion_DragEnter);
                this.ugBasisNodeVersion.InitializeRow -= new Infragistics.Win.UltraWinGrid.InitializeRowEventHandler(this.ugBasisNodeVersion_InitializeRow);
                this.ugBasisNodeVersion.MouseEnterElement -= new Infragistics.Win.UIElementEventHandler(this.ugBasisNodeVersion_MouseEnterElement);
                // End TT#952  
                
				// remove data bindings
				cboGroupBy.DataSource = null;
				cboStoreAttribute.DataSource = null;
				ugHeader.DataSource = null;
                // Begin TT#3513 - JSmith - Clean Up Memory Leaks
                cboFilter.DataSource = null;
                if (_trans.VelocityWindow != null)
                {
                    ((MIDRetail.Windows.frmVelocityMethod)_trans.VelocityWindow).RemoveReferenceToAllocationViewSelection();
                }
                cboFilter.Tag = null;
                cboFilter = null;
                cboStoreAttribute.Tag = null;
                cboStoreAttribute = null;
                // Begin TT#3513 - JSmith - Clean Up Memory Leaks
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
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand2 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            this.gbxDisplay = new System.Windows.Forms.GroupBox();
            this.cboStore = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this.cboView = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this.cboGroupBy = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this.cboFilter = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this.lblStore = new System.Windows.Forms.Label();
            this.cboStoreAttribute = new MIDRetail.Windows.Controls.MIDAttributeComboBox();
            this.lblFilter = new System.Windows.Forms.Label();
            this.lblView = new System.Windows.Forms.Label();
            this.lblGroupBy = new System.Windows.Forms.Label();
            this.lblAttribute = new System.Windows.Forms.Label();
            this.chkIneligibleStores = new System.Windows.Forms.CheckBox();
            this.rbStyleView = new System.Windows.Forms.RadioButton();
            this.rbSizeView = new System.Windows.Forms.RadioButton();
            this.rbSummaryView = new System.Windows.Forms.RadioButton();
            this.gbxNeedAnalysis = new System.Windows.Forms.GroupBox();
            this.cboSizeCurve = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this.picBoxConstraint = new System.Windows.Forms.PictureBox();
            this.picBoxAlternate = new System.Windows.Forms.PictureBox();
            this.picBoxCurve = new System.Windows.Forms.PictureBox();
            this.lblSizeAlternate = new System.Windows.Forms.Label();
            this.lblSizeConstraint = new System.Windows.Forms.Label();
            this.lblSizeCurve = new System.Windows.Forms.Label();
            this.cboAlternates = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this.cboConstraints = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this.lblMerchandise = new System.Windows.Forms.Label();
            this.drsEnd = new MIDRetail.Windows.Controls.MIDDateRangeSelector();
            this.drsBegin = new MIDRetail.Windows.Controls.MIDDateRangeSelector();
            this.lblTimeEnd = new System.Windows.Forms.Label();
            this.txtBasis = new System.Windows.Forms.TextBox();
            this.lblTimeBegin = new System.Windows.Forms.Label();
            this.gbxBasis = new System.Windows.Forms.GroupBox();
            this.ugBasisNodeVersion = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.cmsBasis = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cmsBasisItemInsert = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsBasisInsertMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cmsBasisItemInsertBefore = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsBasisItemInsertAfter = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsBasisItemDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsBasisItemDeleteAll = new System.Windows.Forms.ToolStripMenuItem();
            this.label5 = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.mnuHeaderGrid = new System.Windows.Forms.ContextMenu();
            this.mnuGridColHeader = new System.Windows.Forms.ContextMenu();
            this.gbxAllocHeaders = new System.Windows.Forms.GroupBox();
            this.ugHeader = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.cbxAnalysis = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.utmMain)).BeginInit();
            this.gbxDisplay.SuspendLayout();
            this.gbxNeedAnalysis.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxConstraint)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxAlternate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxCurve)).BeginInit();
            this.gbxBasis.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ugBasisNodeVersion)).BeginInit();
            this.cmsBasis.SuspendLayout();
            this.cmsBasisInsertMenu.SuspendLayout();
            this.gbxAllocHeaders.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ugHeader)).BeginInit();
            this.SuspendLayout();
            // 
            // utmMain
            // 
            this.utmMain.MenuSettings.ForceSerialization = true;
            this.utmMain.ToolbarSettings.ForceSerialization = true;
            // 
            // gbxDisplay
            // 
            this.gbxDisplay.Controls.Add(this.cboStore);
            this.gbxDisplay.Controls.Add(this.cboView);
            this.gbxDisplay.Controls.Add(this.cboGroupBy);
            this.gbxDisplay.Controls.Add(this.cboFilter);
            this.gbxDisplay.Controls.Add(this.lblStore);
            this.gbxDisplay.Controls.Add(this.cboStoreAttribute);
            this.gbxDisplay.Controls.Add(this.lblFilter);
            this.gbxDisplay.Controls.Add(this.lblView);
            this.gbxDisplay.Controls.Add(this.lblGroupBy);
            this.gbxDisplay.Controls.Add(this.lblAttribute);
            this.gbxDisplay.Controls.Add(this.chkIneligibleStores);
            this.gbxDisplay.Location = new System.Drawing.Point(8, 32);
            this.gbxDisplay.Name = "gbxDisplay";
            this.gbxDisplay.Size = new System.Drawing.Size(602, 79);
            this.gbxDisplay.TabIndex = 2;
            this.gbxDisplay.TabStop = false;
            this.gbxDisplay.Text = "Display";
            // 
            // cboStore
            // 
            this.cboStore.AutoAdjust = true;
            this.cboStore.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboStore.DataSource = null;
            this.cboStore.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboStore.DropDownWidth = 133;
            this.cboStore.Location = new System.Drawing.Point(458, 50);
            this.cboStore.Margin = new System.Windows.Forms.Padding(0);
            this.cboStore.Name = "cboStore";
            this.cboStore.Size = new System.Drawing.Size(133, 21);
            this.cboStore.TabIndex = 13;
            this.cboStore.Tag = null;
            // 
            // cboView
            // 
            this.cboView.AutoAdjust = true;
            this.cboView.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboView.DataSource = null;
            this.cboView.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboView.DropDownWidth = 166;
            this.cboView.Location = new System.Drawing.Point(280, 40);
            this.cboView.Margin = new System.Windows.Forms.Padding(0);
            this.cboView.Name = "cboView";
            this.cboView.Size = new System.Drawing.Size(166, 21);
            this.cboView.TabIndex = 7;
            this.cboView.Tag = null;
            this.cboView.SelectionChangeCommitted += new System.EventHandler(this.cboView_SelectionChangeCommitted);
            //Begin TT#316 - MD - DOConnell - Replace all Windows Combobox controls with new enhanced control
            this.cboView.MIDComboBoxPropertiesChangedEvent += new MIDComboBoxPropertiesChangedEventHandler(cboView_MIDComboBoxPropertiesChangedEvent);
            //End TT#316 - MD - DOConnell - Replace all Windows Combobox controls with new enhanced control
            // 
            // cboGroupBy
            // 
            this.cboGroupBy.AutoAdjust = true;
            this.cboGroupBy.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboGroupBy.DataSource = null;
            this.cboGroupBy.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboGroupBy.DropDownWidth = 166;
            this.cboGroupBy.Location = new System.Drawing.Point(80, 40);
            this.cboGroupBy.Margin = new System.Windows.Forms.Padding(0);
            this.cboGroupBy.Name = "cboGroupBy";
            this.cboGroupBy.Size = new System.Drawing.Size(166, 21);
            this.cboGroupBy.TabIndex = 3;
            this.cboGroupBy.Tag = null;
            // 
            // cboFilter
            // 
            this.cboFilter.AllowDrop = true;
            this.cboFilter.AutoAdjust = true;
            this.cboFilter.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboFilter.DataSource = null;
            this.cboFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboFilter.DropDownWidth = 200;
            this.cboFilter.Location = new System.Drawing.Point(280, 16);
            this.cboFilter.Margin = new System.Windows.Forms.Padding(0);
            this.cboFilter.Name = "cboFilter";
            this.cboFilter.Size = new System.Drawing.Size(166, 21);
            this.cboFilter.TabIndex = 5;
            this.cboFilter.Tag = null;
            this.cboFilter.SelectionChangeCommitted += new System.EventHandler(this.cboFilter_SelectionChangeCommitted);
            //Begin TT#316 - MD - DOConnell - Replace all Windows Combobox controls with new enhanced control
            this.cboFilter.MIDComboBoxPropertiesChangedEvent += new MIDComboBoxPropertiesChangedEventHandler (cboFilter_MIDComboBoxPropertiesChangedEvent);
            //End TT#316 - MD - DOConnell - Replace all Windows Combobox controls with new enhanced control
            this.cboFilter.DragDrop += new System.Windows.Forms.DragEventHandler(this.cboFilter_DragDrop);
            this.cboFilter.DragEnter += new System.Windows.Forms.DragEventHandler(this.cboFilter_DragEnter);
            this.cboFilter.DragOver += new System.Windows.Forms.DragEventHandler(this.cboFilter_DragOver);
            // 
            // lblStore
            // 
            this.lblStore.AutoSize = true;
            this.lblStore.Location = new System.Drawing.Point(459, 35);
            this.lblStore.Name = "lblStore";
            this.lblStore.Size = new System.Drawing.Size(35, 13);
            this.lblStore.TabIndex = 14;
            this.lblStore.Text = "label1";
            // 
            // cboStoreAttribute
            // 
            this.cboStoreAttribute.AllowDrop = true;
            this.cboStoreAttribute.AllowUserAttributes = false;
            this.cboStoreAttribute.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboStoreAttribute.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboStoreAttribute.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboStoreAttribute.DropDownWidth = 200;
            this.cboStoreAttribute.Location = new System.Drawing.Point(80, 16);
            this.cboStoreAttribute.Name = "cboStoreAttribute";
            this.cboStoreAttribute.Size = new System.Drawing.Size(166, 21);
            this.cboStoreAttribute.TabIndex = 1;
            this.cboStoreAttribute.SelectionChangeCommitted += new System.EventHandler(this.cboStoreAttribute_SelectionChangeCommitted);
            //Begin TT#316 - MD - DOConnell - Replace all Windows Combobox controls with new enhanced control
            this.cboStoreAttribute.MIDComboBoxPropertiesChangedEvent += new MIDComboBoxPropertiesChangedEventHandler(cboStoreAttribute_MIDComboBoxPropertiesChangedEvent);
            //End TT#316 - MD - DOConnell - Replace all Windows Combobox controls with new enhanced control
            this.cboStoreAttribute.DragDrop += new System.Windows.Forms.DragEventHandler(this.cboStoreAttribute_DragDrop);
            this.cboStoreAttribute.DragEnter += new System.Windows.Forms.DragEventHandler(this.cboStoreAttribute_DragEnter);
            this.cboStoreAttribute.DragOver += new System.Windows.Forms.DragEventHandler(this.cboStoreAttribute_DragOver);
            // 
            // lblFilter
            // 
            this.lblFilter.Location = new System.Drawing.Point(250, 24);
            this.lblFilter.Name = "lblFilter";
            this.lblFilter.Size = new System.Drawing.Size(40, 16);
            this.lblFilter.TabIndex = 4;
            this.lblFilter.Text = "Filter:";
            // 
            // lblView
            // 
            this.lblView.Location = new System.Drawing.Point(250, 48);
            this.lblView.Name = "lblView";
            this.lblView.Size = new System.Drawing.Size(40, 16);
            this.lblView.TabIndex = 6;
            this.lblView.Text = "View:";
            // 
            // lblGroupBy
            // 
            this.lblGroupBy.Location = new System.Drawing.Point(8, 48);
            this.lblGroupBy.Name = "lblGroupBy";
            this.lblGroupBy.Size = new System.Drawing.Size(80, 16);
            this.lblGroupBy.TabIndex = 2;
            this.lblGroupBy.Text = "Group By:";
            // 
            // lblAttribute
            // 
            this.lblAttribute.Location = new System.Drawing.Point(8, 24);
            this.lblAttribute.Name = "lblAttribute";
            this.lblAttribute.Size = new System.Drawing.Size(80, 16);
            this.lblAttribute.TabIndex = 0;
            this.lblAttribute.Text = "Store Attibute:";
            // 
            // chkIneligibleStores
            // 
            this.chkIneligibleStores.Location = new System.Drawing.Point(458, 14);
            this.chkIneligibleStores.Name = "chkIneligibleStores";
            this.chkIneligibleStores.Size = new System.Drawing.Size(100, 18);
            this.chkIneligibleStores.TabIndex = 11;
            this.chkIneligibleStores.Text = "Ineligible Stores";
            this.chkIneligibleStores.CheckedChanged += new System.EventHandler(this.chkIneligibleStores_CheckedChanged);
            // 
            // rbStyleView
            // 
            this.rbStyleView.Checked = true;
            this.rbStyleView.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.rbStyleView.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbStyleView.Location = new System.Drawing.Point(48, 8);
            this.rbStyleView.Name = "rbStyleView";
            this.rbStyleView.Size = new System.Drawing.Size(56, 24);
            this.rbStyleView.TabIndex = 3;
            this.rbStyleView.TabStop = true;
            this.rbStyleView.Text = "Style";
            this.rbStyleView.CheckedChanged += new System.EventHandler(this.rbStyleView_CheckedChanged);
            // 
            // rbSizeView
            // 
            this.rbSizeView.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.rbSizeView.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbSizeView.Location = new System.Drawing.Point(120, 8);
            this.rbSizeView.Name = "rbSizeView";
            this.rbSizeView.Size = new System.Drawing.Size(56, 24);
            this.rbSizeView.TabIndex = 4;
            this.rbSizeView.Text = "Size";
            this.rbSizeView.CheckedChanged += new System.EventHandler(this.rbSizeView_CheckedChanged);
            // 
            // rbSummaryView
            // 
            this.rbSummaryView.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.rbSummaryView.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbSummaryView.Location = new System.Drawing.Point(176, 8);
            this.rbSummaryView.Name = "rbSummaryView";
            this.rbSummaryView.Size = new System.Drawing.Size(96, 24);
            this.rbSummaryView.TabIndex = 5;
            this.rbSummaryView.Text = "Summary";
            this.rbSummaryView.CheckedChanged += new System.EventHandler(this.rbSummaryView_CheckedChanged);
            // 
            // gbxNeedAnalysis
            // 
            this.gbxNeedAnalysis.Controls.Add(this.cboSizeCurve);
            this.gbxNeedAnalysis.Controls.Add(this.picBoxConstraint);
            this.gbxNeedAnalysis.Controls.Add(this.picBoxAlternate);
            this.gbxNeedAnalysis.Controls.Add(this.picBoxCurve);
            this.gbxNeedAnalysis.Controls.Add(this.lblSizeAlternate);
            this.gbxNeedAnalysis.Controls.Add(this.lblSizeConstraint);
            this.gbxNeedAnalysis.Controls.Add(this.lblSizeCurve);
            this.gbxNeedAnalysis.Controls.Add(this.cboAlternates);
            this.gbxNeedAnalysis.Controls.Add(this.cboConstraints);
            this.gbxNeedAnalysis.Controls.Add(this.lblMerchandise);
            this.gbxNeedAnalysis.Controls.Add(this.drsEnd);
            this.gbxNeedAnalysis.Controls.Add(this.drsBegin);
            this.gbxNeedAnalysis.Controls.Add(this.lblTimeEnd);
            this.gbxNeedAnalysis.Controls.Add(this.txtBasis);
            this.gbxNeedAnalysis.Controls.Add(this.lblTimeBegin);
            this.gbxNeedAnalysis.Location = new System.Drawing.Point(8, 110);
            this.gbxNeedAnalysis.Name = "gbxNeedAnalysis";
            this.gbxNeedAnalysis.Size = new System.Drawing.Size(602, 115);
            this.gbxNeedAnalysis.TabIndex = 6;
            this.gbxNeedAnalysis.TabStop = false;
            this.gbxNeedAnalysis.Text = "Need Analysis";
            // 
            // cboSizeCurve
            // 
            this.cboSizeCurve.AutoAdjust = true;
            this.cboSizeCurve.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboSizeCurve.DataSource = null;
            this.cboSizeCurve.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSizeCurve.DropDownWidth = 408;
            this.cboSizeCurve.Location = new System.Drawing.Point(160, 88);
            this.cboSizeCurve.Margin = new System.Windows.Forms.Padding(0);
            this.cboSizeCurve.Name = "cboSizeCurve";
            this.cboSizeCurve.Size = new System.Drawing.Size(408, 21);
            this.cboSizeCurve.TabIndex = 12;
            this.cboSizeCurve.Tag = null;
            // 
            // picBoxConstraint
            // 
            this.picBoxConstraint.Location = new System.Drawing.Point(132, 120);
            this.picBoxConstraint.Name = "picBoxConstraint";
            this.picBoxConstraint.Size = new System.Drawing.Size(19, 20);
            this.picBoxConstraint.TabIndex = 59;
            this.picBoxConstraint.TabStop = false;
            this.picBoxConstraint.Click += new System.EventHandler(this.picBox_Click);
            // 
            // picBoxAlternate
            // 
            this.picBoxAlternate.Location = new System.Drawing.Point(132, 144);
            this.picBoxAlternate.Name = "picBoxAlternate";
            this.picBoxAlternate.Size = new System.Drawing.Size(20, 20);
            this.picBoxAlternate.TabIndex = 58;
            this.picBoxAlternate.TabStop = false;
            this.picBoxAlternate.Click += new System.EventHandler(this.picBox_Click);
            // 
            // picBoxCurve
            // 
            this.picBoxCurve.Location = new System.Drawing.Point(132, 88);
            this.picBoxCurve.Name = "picBoxCurve";
            this.picBoxCurve.Size = new System.Drawing.Size(20, 20);
            this.picBoxCurve.TabIndex = 57;
            this.picBoxCurve.TabStop = false;
            this.picBoxCurve.Click += new System.EventHandler(this.picBox_Click);
            // 
            // lblSizeAlternate
            // 
            this.lblSizeAlternate.Location = new System.Drawing.Point(8, 150);
            this.lblSizeAlternate.Name = "lblSizeAlternate";
            this.lblSizeAlternate.Size = new System.Drawing.Size(122, 16);
            this.lblSizeAlternate.TabIndex = 17;
            this.lblSizeAlternate.Text = "Size Alternates Model";
            // 
            // lblSizeConstraint
            // 
            this.lblSizeConstraint.Location = new System.Drawing.Point(8, 126);
            this.lblSizeConstraint.Name = "lblSizeConstraint";
            this.lblSizeConstraint.Size = new System.Drawing.Size(122, 16);
            this.lblSizeConstraint.TabIndex = 16;
            this.lblSizeConstraint.Text = "Size Constraints Model";
            // 
            // lblSizeCurve
            // 
            this.lblSizeCurve.Location = new System.Drawing.Point(8, 96);
            this.lblSizeCurve.Name = "lblSizeCurve";
            this.lblSizeCurve.Size = new System.Drawing.Size(112, 16);
            this.lblSizeCurve.TabIndex = 15;
            this.lblSizeCurve.Text = "Size Curve";
            // 
            // cboAlternates
            // 
            this.cboAlternates.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cboAlternates.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboAlternates.Location = new System.Drawing.Point(160, 144);
            this.cboAlternates.Name = "cboAlternates";
            this.cboAlternates.Size = new System.Drawing.Size(408, 21);
            this.cboAlternates.TabIndex = 14;
            // 
            // cboConstraints
            // 
            this.cboConstraints.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cboConstraints.BackColor = System.Drawing.SystemColors.Window;
            this.cboConstraints.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboConstraints.Location = new System.Drawing.Point(160, 120);
            this.cboConstraints.Name = "cboConstraints";
            this.cboConstraints.Size = new System.Drawing.Size(408, 21);
            this.cboConstraints.TabIndex = 13;
            // 
            // lblMerchandise
            // 
            this.lblMerchandise.Location = new System.Drawing.Point(8, 72);
            this.lblMerchandise.Name = "lblMerchandise";
            this.lblMerchandise.Size = new System.Drawing.Size(112, 16);
            this.lblMerchandise.TabIndex = 11;
            this.lblMerchandise.Text = "Merchandise Basis";
            // 
            // drsEnd
            // 
            this.drsEnd.DateRangeForm = null;
            this.drsEnd.DateRangeRID = 0;
            this.drsEnd.Enabled = false;
            this.drsEnd.Location = new System.Drawing.Point(160, 40);
            this.drsEnd.Name = "drsEnd";
            this.drsEnd.Size = new System.Drawing.Size(236, 24);
            this.drsEnd.TabIndex = 10;
            this.drsEnd.OnSelection += new MIDRetail.Windows.Controls.MIDDateRangeSelector.SelectionEventHandler(this.drsEnd_OnSelection);
            this.drsEnd.Click += new System.EventHandler(this.drsEnd_Click);
            this.drsEnd.Load += new System.EventHandler(this.drsEnd_Load);
            // 
            // drsBegin
            // 
            this.drsBegin.DateRangeForm = null;
            this.drsBegin.DateRangeRID = 0;
            this.drsBegin.Enabled = false;
            this.drsBegin.Location = new System.Drawing.Point(160, 16);
            this.drsBegin.Name = "drsBegin";
            this.drsBegin.Size = new System.Drawing.Size(236, 24);
            this.drsBegin.TabIndex = 9;
            this.drsBegin.OnSelection += new MIDRetail.Windows.Controls.MIDDateRangeSelector.SelectionEventHandler(this.drsBegin_OnSelection);
            this.drsBegin.Click += new System.EventHandler(this.drsBegin_Click);
            this.drsBegin.Load += new System.EventHandler(this.drsBegin_Load);
            // 
            // lblTimeEnd
            // 
            this.lblTimeEnd.Location = new System.Drawing.Point(8, 48);
            this.lblTimeEnd.Name = "lblTimeEnd";
            this.lblTimeEnd.Size = new System.Drawing.Size(112, 16);
            this.lblTimeEnd.TabIndex = 8;
            this.lblTimeEnd.Text = "Time Period End";
            // 
            // txtBasis
            // 
            this.txtBasis.AllowDrop = true;
            this.txtBasis.Location = new System.Drawing.Point(160, 64);
            this.txtBasis.Name = "txtBasis";
            this.txtBasis.Size = new System.Drawing.Size(405, 20);
            this.txtBasis.TabIndex = 6;
            this.txtBasis.TextChanged += new System.EventHandler(this.txtBasis_TextChanged);
            this.txtBasis.DragDrop += new System.Windows.Forms.DragEventHandler(this.txtBasis_DragDrop);
            this.txtBasis.DragEnter += new System.Windows.Forms.DragEventHandler(this.txtBasis_DragEnter);
            this.txtBasis.DragOver += new System.Windows.Forms.DragEventHandler(this.txtBasis_DragOver);
            this.txtBasis.Validating += new System.ComponentModel.CancelEventHandler(this.txtBasis_Validating);
            this.txtBasis.Validated += new System.EventHandler(this.txtBasis_Validated);
            // 
            // lblTimeBegin
            // 
            this.lblTimeBegin.Location = new System.Drawing.Point(8, 24);
            this.lblTimeBegin.Name = "lblTimeBegin";
            this.lblTimeBegin.Size = new System.Drawing.Size(112, 16);
            this.lblTimeBegin.TabIndex = 2;
            this.lblTimeBegin.Text = "Time Period Begin";
            // 
            // gbxBasis
            // 
            this.gbxBasis.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbxBasis.Controls.Add(this.ugBasisNodeVersion);
            this.gbxBasis.Location = new System.Drawing.Point(8, 230);
            this.gbxBasis.Name = "gbxBasis";
            this.gbxBasis.Size = new System.Drawing.Size(602, 146);
            this.gbxBasis.TabIndex = 60;
            this.gbxBasis.TabStop = false;
            this.gbxBasis.Text = "Basis";
            // 
            // ugBasisNodeVersion
            // 
            this.ugBasisNodeVersion.AllowDrop = true;
            this.ugBasisNodeVersion.AlphaBlendMode = Infragistics.Win.AlphaBlendMode.Disabled;
            this.ugBasisNodeVersion.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ugBasisNodeVersion.ContextMenuStrip = this.cmsBasis;
            appearance1.BackColor = System.Drawing.Color.White;
            appearance1.BackColor2 = System.Drawing.Color.White;
            this.ugBasisNodeVersion.DisplayLayout.AddNewBox.ButtonAppearance = appearance1;
            this.ugBasisNodeVersion.DisplayLayout.AddNewBox.Prompt = "Add ...";
            ultraGridBand1.HeaderVisible = true;
            ultraGridBand1.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Extended;
            this.ugBasisNodeVersion.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
            this.ugBasisNodeVersion.DisplayLayout.GroupByBox.Prompt = " ";
            this.ugBasisNodeVersion.DisplayLayout.GroupByBox.ShowBandLabels = Infragistics.Win.UltraWinGrid.ShowBandLabels.None;
            this.ugBasisNodeVersion.DisplayLayout.InterBandSpacing = 2;
            this.ugBasisNodeVersion.DisplayLayout.MaxColScrollRegions = 1;
            this.ugBasisNodeVersion.DisplayLayout.MaxRowScrollRegions = 1;
            this.ugBasisNodeVersion.DisplayLayout.Override.AllowAddNew = Infragistics.Win.UltraWinGrid.AllowAddNew.Yes;
            this.ugBasisNodeVersion.DisplayLayout.Override.AllowColMoving = Infragistics.Win.UltraWinGrid.AllowColMoving.NotAllowed;
            this.ugBasisNodeVersion.DisplayLayout.Override.AllowColSwapping = Infragistics.Win.UltraWinGrid.AllowColSwapping.NotAllowed;
            this.ugBasisNodeVersion.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.True;
            this.ugBasisNodeVersion.DisplayLayout.Override.AllowGroupMoving = Infragistics.Win.UltraWinGrid.AllowGroupMoving.NotAllowed;
            this.ugBasisNodeVersion.DisplayLayout.Override.AllowGroupSwapping = Infragistics.Win.UltraWinGrid.AllowGroupSwapping.NotAllowed;
            this.ugBasisNodeVersion.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.True;
            this.ugBasisNodeVersion.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.Edit;
            this.ugBasisNodeVersion.DisplayLayout.Override.CellMultiLine = Infragistics.Win.DefaultableBoolean.False;
            this.ugBasisNodeVersion.DisplayLayout.Override.GroupByColumnsHidden = Infragistics.Win.DefaultableBoolean.False;
            this.ugBasisNodeVersion.DisplayLayout.Override.NullText = "";
            this.ugBasisNodeVersion.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;
            this.ugBasisNodeVersion.DisplayLayout.Override.RowSizing = Infragistics.Win.UltraWinGrid.RowSizing.Fixed;
            this.ugBasisNodeVersion.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.None;
            this.ugBasisNodeVersion.DisplayLayout.Override.SelectTypeGroupByRow = Infragistics.Win.UltraWinGrid.SelectType.None;
            this.ugBasisNodeVersion.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.ugBasisNodeVersion.DisplayLayout.Override.TipStyleCell = Infragistics.Win.UltraWinGrid.TipStyle.Show;
            this.ugBasisNodeVersion.DisplayLayout.Override.TipStyleRowConnector = Infragistics.Win.UltraWinGrid.TipStyle.Hide;
            this.ugBasisNodeVersion.DisplayLayout.Override.TipStyleScroll = Infragistics.Win.UltraWinGrid.TipStyle.Hide;
            this.ugBasisNodeVersion.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.ugBasisNodeVersion.DisplayLayout.ViewStyle = Infragistics.Win.UltraWinGrid.ViewStyle.SingleBand;
            this.ugBasisNodeVersion.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.Horizontal;
            this.ugBasisNodeVersion.Location = new System.Drawing.Point(0, 19);
            this.ugBasisNodeVersion.Name = "ugBasisNodeVersion";
            this.ugBasisNodeVersion.Size = new System.Drawing.Size(586, 121);
            this.ugBasisNodeVersion.TabIndex = 13;
            this.ugBasisNodeVersion.UpdateMode = Infragistics.Win.UltraWinGrid.UpdateMode.OnCellChange;
            this.ugBasisNodeVersion.AfterCellUpdate += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ugBasisNodeVersion_AfterCellUpdate);
            this.ugBasisNodeVersion.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.ugBasisNodeVersion_InitializeLayout);
            this.ugBasisNodeVersion.InitializeRow += new Infragistics.Win.UltraWinGrid.InitializeRowEventHandler(this.ugBasisNodeVersion_InitializeRow);
            this.ugBasisNodeVersion.ClickCellButton += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ugBasisNodeVersion_ClickCellButton);
            this.ugBasisNodeVersion.MouseEnterElement += new Infragistics.Win.UIElementEventHandler(this.ugBasisNodeVersion_MouseEnterElement);
            this.ugBasisNodeVersion.DragDrop += new System.Windows.Forms.DragEventHandler(this.ugBasisNodeVersion_DragDrop);
            this.ugBasisNodeVersion.DragEnter += new System.Windows.Forms.DragEventHandler(this.ugBasisNodeVersion_DragEnter);
            this.ugBasisNodeVersion.DragOver += new System.Windows.Forms.DragEventHandler(this.ugBasisNodeVersion_DragOver);
            // 
            // cmsBasis
            // 
            this.cmsBasis.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmsBasisItemInsert,
            this.cmsBasisItemDelete,
            this.cmsBasisItemDeleteAll});
            this.cmsBasis.Name = "cmsBasis";
            this.cmsBasis.ShowImageMargin = false;
            this.cmsBasis.Size = new System.Drawing.Size(100, 70);
            this.cmsBasis.Opening += new System.ComponentModel.CancelEventHandler(this.cmsBasis_Opening);
            // 
            // cmsBasisItemInsert
            // 
            this.cmsBasisItemInsert.DropDown = this.cmsBasisInsertMenu;
            this.cmsBasisItemInsert.Name = "cmsBasisItemInsert";
            this.cmsBasisItemInsert.Size = new System.Drawing.Size(99, 22);
            this.cmsBasisItemInsert.Text = "Insert";
            // 
            // cmsBasisInsertMenu
            // 
            this.cmsBasisInsertMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmsBasisItemInsertBefore,
            this.cmsBasisItemInsertAfter});
            this.cmsBasisInsertMenu.Name = "cmsBasisInsertMenu";
            this.cmsBasisInsertMenu.OwnerItem = this.cmsBasisItemInsert;
            this.cmsBasisInsertMenu.ShowImageMargin = false;
            this.cmsBasisInsertMenu.Size = new System.Drawing.Size(84, 48);
            // 
            // cmsBasisItemInsertBefore
            // 
            this.cmsBasisItemInsertBefore.Name = "cmsBasisItemInsertBefore";
            this.cmsBasisItemInsertBefore.Size = new System.Drawing.Size(83, 22);
            this.cmsBasisItemInsertBefore.Text = "Before";
            this.cmsBasisItemInsertBefore.Click += new System.EventHandler(this.cmsBasisItemInsertBefore_Click);
            // 
            // cmsBasisItemInsertAfter
            // 
            this.cmsBasisItemInsertAfter.Name = "cmsBasisItemInsertAfter";
            this.cmsBasisItemInsertAfter.Size = new System.Drawing.Size(83, 22);
            this.cmsBasisItemInsertAfter.Text = "After";
            this.cmsBasisItemInsertAfter.Click += new System.EventHandler(this.cmsBasisItemInsertAfter_Click);
            // 
            // cmsBasisItemDelete
            // 
            this.cmsBasisItemDelete.Name = "cmsBasisItemDelete";
            this.cmsBasisItemDelete.Size = new System.Drawing.Size(99, 22);
            this.cmsBasisItemDelete.Text = "Delete";
            this.cmsBasisItemDelete.Click += new System.EventHandler(this.cmsBasisItemDelete_Click);
            // 
            // cmsBasisItemDeleteAll
            // 
            this.cmsBasisItemDeleteAll.Name = "cmsBasisItemDeleteAll";
            this.cmsBasisItemDeleteAll.Size = new System.Drawing.Size(99, 22);
            this.cmsBasisItemDeleteAll.Text = "Delete All";
            this.cmsBasisItemDeleteAll.Click += new System.EventHandler(this.cmsBasisItemDeleteAll_Click);
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(0, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(100, 23);
            this.label5.TabIndex = 0;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnOK.Location = new System.Drawing.Point(454, 571);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 8;
            this.btnOK.Text = "OK";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnCancel.Location = new System.Drawing.Point(535, 571);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 10;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // gbxAllocHeaders
            // 
            this.gbxAllocHeaders.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbxAllocHeaders.Controls.Add(this.ugHeader);
            this.gbxAllocHeaders.Location = new System.Drawing.Point(8, 376);
            this.gbxAllocHeaders.Name = "gbxAllocHeaders";
            this.gbxAllocHeaders.Size = new System.Drawing.Size(602, 190);
            this.gbxAllocHeaders.TabIndex = 12;
            this.gbxAllocHeaders.TabStop = false;
            this.gbxAllocHeaders.Text = "Allocation Headers";
            // 
            // ugHeader
            // 
            this.ugHeader.AlphaBlendMode = Infragistics.Win.AlphaBlendMode.Disabled;
            this.ugHeader.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            appearance2.BackColor = System.Drawing.Color.White;
            appearance2.BackColor2 = System.Drawing.Color.White;
            this.ugHeader.DisplayLayout.AddNewBox.ButtonAppearance = appearance2;
            this.ugHeader.DisplayLayout.AddNewBox.Hidden = false;
            this.ugHeader.DisplayLayout.AddNewBox.Prompt = "Add ...";
            ultraGridBand2.HeaderVisible = true;
            ultraGridBand2.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Extended;
            this.ugHeader.DisplayLayout.BandsSerializer.Add(ultraGridBand2);
            this.ugHeader.DisplayLayout.GroupByBox.Prompt = " ";
            this.ugHeader.DisplayLayout.GroupByBox.ShowBandLabels = Infragistics.Win.UltraWinGrid.ShowBandLabels.None;
            this.ugHeader.DisplayLayout.InterBandSpacing = 2;
            this.ugHeader.DisplayLayout.MaxColScrollRegions = 1;
            this.ugHeader.DisplayLayout.MaxRowScrollRegions = 1;
            this.ugHeader.DisplayLayout.Override.AllowAddNew = Infragistics.Win.UltraWinGrid.AllowAddNew.Yes;
            this.ugHeader.DisplayLayout.Override.AllowColMoving = Infragistics.Win.UltraWinGrid.AllowColMoving.NotAllowed;
            this.ugHeader.DisplayLayout.Override.AllowColSwapping = Infragistics.Win.UltraWinGrid.AllowColSwapping.NotAllowed;
            this.ugHeader.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.True;
            this.ugHeader.DisplayLayout.Override.AllowGroupMoving = Infragistics.Win.UltraWinGrid.AllowGroupMoving.NotAllowed;
            this.ugHeader.DisplayLayout.Override.AllowGroupSwapping = Infragistics.Win.UltraWinGrid.AllowGroupSwapping.NotAllowed;
            this.ugHeader.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.True;
            this.ugHeader.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.Edit;
            this.ugHeader.DisplayLayout.Override.CellMultiLine = Infragistics.Win.DefaultableBoolean.False;
            this.ugHeader.DisplayLayout.Override.GroupByColumnsHidden = Infragistics.Win.DefaultableBoolean.False;
            this.ugHeader.DisplayLayout.Override.NullText = "";
            this.ugHeader.DisplayLayout.Override.RowSizing = Infragistics.Win.UltraWinGrid.RowSizing.Fixed;
            this.ugHeader.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.None;
            this.ugHeader.DisplayLayout.Override.SelectTypeGroupByRow = Infragistics.Win.UltraWinGrid.SelectType.None;
            this.ugHeader.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.ugHeader.DisplayLayout.Override.TipStyleCell = Infragistics.Win.UltraWinGrid.TipStyle.Show;
            this.ugHeader.DisplayLayout.Override.TipStyleRowConnector = Infragistics.Win.UltraWinGrid.TipStyle.Hide;
            this.ugHeader.DisplayLayout.Override.TipStyleScroll = Infragistics.Win.UltraWinGrid.TipStyle.Hide;
            this.ugHeader.DisplayLayout.ViewStyle = Infragistics.Win.UltraWinGrid.ViewStyle.SingleBand;
            this.ugHeader.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.Horizontal;
            this.ugHeader.Location = new System.Drawing.Point(16, 16);
            this.ugHeader.Name = "ugHeader";
            this.ugHeader.Size = new System.Drawing.Size(566, 156);
            this.ugHeader.TabIndex = 14;
            this.ugHeader.UpdateMode = Infragistics.Win.UltraWinGrid.UpdateMode.OnCellChange;
            this.ugHeader.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.ugHeader_InitializeLayout);
            this.ugHeader.AfterRowsDeleted += new System.EventHandler(this.ugHeader_AfterRowsDeleted);
            this.ugHeader.AfterRowInsert += new Infragistics.Win.UltraWinGrid.RowEventHandler(this.ugHeader_AfterRowInsert);
            this.ugHeader.BeforeExitEditMode += new Infragistics.Win.UltraWinGrid.BeforeExitEditModeEventHandler(this.ugHeader_BeforeExitEditMode);
            this.ugHeader.BeforeRowsDeleted += new Infragistics.Win.UltraWinGrid.BeforeRowsDeletedEventHandler(this.ugHeader_BeforeRowsDeleted);
            this.ugHeader.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ugHeader_KeyDown);
            // 
            // cbxAnalysis
            // 
            this.cbxAnalysis.Location = new System.Drawing.Point(278, 8);
            this.cbxAnalysis.Name = "cbxAnalysis";
            this.cbxAnalysis.Size = new System.Drawing.Size(96, 24);
            this.cbxAnalysis.TabIndex = 6;
            this.cbxAnalysis.Text = "Analysis only";
            this.cbxAnalysis.CheckedChanged += new System.EventHandler(this.cbxAnalysis_CheckedChanged);
            // 
            // AllocationViewSelection
            // 
            this.AllowDragDrop = true;
            this.AllowDrop = true;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(618, 597);
            this.Controls.Add(this.gbxBasis);
            this.Controls.Add(this.cbxAnalysis);
            this.Controls.Add(this.gbxAllocHeaders);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.gbxNeedAnalysis);
            this.Controls.Add(this.rbStyleView);
            this.Controls.Add(this.rbSummaryView);
            this.Controls.Add(this.rbSizeView);
            this.Controls.Add(this.gbxDisplay);
            this.Name = "AllocationViewSelection";
            this.Text = "AllocationViewSelection";
            this.Load += new System.EventHandler(this.AllocationViewSelection_Load);
            this.Controls.SetChildIndex(this.gbxDisplay, 0);
            this.Controls.SetChildIndex(this.rbSizeView, 0);
            this.Controls.SetChildIndex(this.rbSummaryView, 0);
            this.Controls.SetChildIndex(this.rbStyleView, 0);
            this.Controls.SetChildIndex(this.gbxNeedAnalysis, 0);
            this.Controls.SetChildIndex(this.btnOK, 0);
            this.Controls.SetChildIndex(this.btnCancel, 0);
            this.Controls.SetChildIndex(this.gbxAllocHeaders, 0);
            this.Controls.SetChildIndex(this.cbxAnalysis, 0);
            this.Controls.SetChildIndex(this.gbxBasis, 0);
            ((System.ComponentModel.ISupportInitialize)(this.utmMain)).EndInit();
            this.gbxDisplay.ResumeLayout(false);
            this.gbxDisplay.PerformLayout();
            this.gbxNeedAnalysis.ResumeLayout(false);
            this.gbxNeedAnalysis.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxConstraint)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxAlternate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxCurve)).EndInit();
            this.gbxBasis.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ugBasisNodeVersion)).EndInit();
            this.cmsBasis.ResumeLayout(false);
            this.cmsBasisInsertMenu.ResumeLayout(false);
            this.gbxAllocHeaders.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ugHeader)).EndInit();
            this.ResumeLayout(false);

		}
		#endregion
        #endregion
        #region My Declarations

        private SessionAddressBlock _SAB;
        private ExplorerAddressBlock _EAB;
		private ApplicationSessionTransaction _trans;
		private FunctionSecurityProfile _allocationReviewSecurity;
		private FunctionSecurityProfile _allocationReviewStyleSecurity;
		private FunctionSecurityProfile _allocationReviewSummarySecurity;
		private FunctionSecurityProfile _allocationReviewSizeSecurity;
		//		private CalendarDateSelector _frm;
		private HierarchyNodeProfile _hnp;
		private bool _loading;
//		private CalendarDateSelector _drsBeginForm;
//		private CalendarDateSelector _drsEndForm;
		eAllocationSelectionViewType _viewType;
		private bool _continueProcess;
		private bool _allHeadersReadOnly;
		private Hashtable _readOnlyList;
		private bool _bypassScreen = false;
		private bool _errorFound = false;
		//private StoreFilterData _storeFilterDL;
        private FilterData _storeFilterDL;
		private DataTable _dtFilter;
//		private bool _headerChangesMade = false;
//		private bool _headerGridBuilt = false;
//		private bool _headerIsPopulated = false;

//		private Image _errorImage = null;
        // Begin TT#638 - RMatelic - Style Review - Add Basis Variables
        //private bool _setRowPosition = true;
		private bool _setHeaderRowPosition = true;
        // End TT#638  
		private AllocationHeaderProfileList _headerList;
		private AllocationHeaderProfile _profileToDelete;
		private AllocationHeaderProfileList _deleteList;
		private DataTable _headerDataTable;
	
		private string _lbl_Header;  
		private string _lbl_Description;  
		private string _lbl_RowPosition;  
		private string _lbl_RID;  
		private int _lastValidBasisRID = Include.NoRID; // MID Track #4311

		// BEGION MID Track #2959 - add Size Need Analysis
		private SizeModelData _sizeModelData = null;
       
        // Begin TT#456 - RMatelic - Add Views to Size Review  
        private bool _bindingView = false;  
        private int _styleViewRID;
        private int _sizeViewRID;
        private eLayoutID _layoutID;
        //private ArrayList _userRIDList = null;
        private DataTable _dtStyleViews = null;
        private DataTable _dtSizeViews = null;
        private UserGridView _userGridView;
        // End TT#456  
       
        // Begin TT#638 - RMatelic - Style Review - Add Basis Variables
        private System.Data.DataTable _basisDataTable;
        private System.Data.DataTable _merchDataTable2;
        private HierarchyProfile _hp;
        private bool _basisNodeInList = false;
        //private bool _setBasisRowPosition = true;
        private bool _skipAfterCellUpdate = false;
        // End TT#638 
        // Begin TT#2093 - JSmith - Need Analysis Add basis and after window opens add columns and receive an error.
        private Infragistics.Win.ValueList _analysisValueList = null;
        // End TT#2093 - JSmith - Need Analysis Add basis and after window opens add columns and receive an error.
        private string _groupName;
        public string GroupName
        {
            get
            {
                return _groupName;
            }
            set
            {
                _groupName = value;
            }
        }
        private SelectedHeaderList _selectedHeaderList; //TT#4800 - RMatelic - Select Allocation- Review - Select from Main Tool Bar and receive a Null Reference Exception
		#endregion

		protected SizeModelData SizeModelData
		{
			get
			{
				if (_sizeModelData == null)
				{
					_sizeModelData = new SizeModelData();
				}

				return _sizeModelData;
			}
		}
		// END MID Track #2959 


		public AllocationViewSelection(ExplorerAddressBlock eab, ApplicationSessionTransaction Trans)
			:base(Trans.SAB)
		{
 			InitializeComponent();
			AllowDragDrop = true;
			_trans = Trans;
			_SAB = _trans.SAB;
            _EAB = eab;
			_lbl_Header = MIDText.GetTextOnly(eMIDTextCode.lbl_Header);
			_lbl_Description = MIDText.GetTextOnly(eMIDTextCode.lbl_Description);
			_lbl_RowPosition = "Row Position";
            _lbl_RID = "RID";
			_allocationReviewSecurity = _SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AllocationReview);
			_allocationReviewStyleSecurity = _SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AllocationReviewStyle);
			_allocationReviewSummarySecurity = _SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AllocationReviewSummary);
			_allocationReviewSizeSecurity = _SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AllocationReviewSize);
			// BEGIN MID Track #4567 - John Smith - add filtering
			DisplayPictureBoxImages();
			SetPictureBoxTags();
			// END MID Track #4567
		}

		#region Initial Load
		public void DetermineWindow(eAllocationSelectionViewType ViewType)
		{
			//if (!_SAB.ApplicationServerSession.GlobalOptions.AppConfig.SizeInstalled) // MID Change j.ellis Serialization error
			if (!_SAB.ClientServerSession.GlobalOptions.AppConfig.SizeInstalled) // MID Change j.ellis Serialization Error
			{	
				this.rbSizeView.Enabled = false;
                this.cboSizeCurve.Enabled = false;		// BEGIN MID Track #2959 - Add Size Need Analysis
				this.cboConstraints.Enabled = false;
				this.cboAlternates.Enabled = false;		// END MID Track #2959 
			}
				
			System.EventArgs args = new EventArgs();
			_viewType = ViewType;
			Cursor.Current = Cursors.WaitCursor;
			_trans.CreateAllocationViewSelectionCriteria();
			//if (_viewType != eAllocationSelectionViewType.Velocity)
			_trans.NewCriteriaHeaderList();
	
			_headerList =(AllocationHeaderProfileList)_trans.GetMasterProfileList(eProfileType.AllocationHeader);
            // Begin TT#4800 - RMatelic -Select Allocation- Review - Select from Main Tool Bar and receive a Null Reference Exception
            _selectedHeaderList = (SelectedHeaderList)SAB.ClientServerSession.GetSelectedHeaderList();
            // Begin TT#5249 - JSmith - Grp Alloc>Process Vel Inter>Open Str Detail>Get Error
            //if (_selectedHeaderList.Count == 0)
            // Begin TT#2051-MD - AGallagher - Velocity receive mssg No header selected or selected header requires Save.  Then when apply 2nd rule receive a Null Reference Exception when selecting Apply changes.
            if (_viewType == eAllocationSelectionViewType.Velocity)
                _trans.AllocationStoreAttributeID = _trans.VelocityStoreGroupRID; 
            else
            // End TT#2051-MD - AGallagher - Velocity receive mssg No header selected or selected header requires Save.  Then when apply 2nd rule receive a Null Reference Exception when selecting Apply changes.
                if (_selectedHeaderList.Count == 0 && !_trans.ContainsGroupAllocationHeaders())
                // End TT#5249 - JSmith - Grp Alloc>Process Vel Inter>Open Str Detail>Get Error
                {
                    MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_NoHeaderSelectedOnWorkspace), MIDText.GetTextOnly(eMIDTextCode.frm_AllocationViewSelection), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            // End TT#4800
			if (_viewType == eAllocationSelectionViewType.Velocity)
				_trans.AllocationStoreAttributeID = _trans.VelocityStoreGroupRID; 
			else if (_trans.AllocationStoreAttributeID == Include.NoRID)
				//_trans.AllocationStoreAttributeID = _SAB.ApplicationServerSession.GlobalOptions.AllocationStoreGroupRID; // MID Change j.ellis Serialization error
				_trans.AllocationStoreAttributeID = _SAB.ClientServerSession.GlobalOptions.AllocationStoreGroupRID; // MID Change j.ellis Serialization error
		
			if (    _viewType == eAllocationSelectionViewType.None 
				|| _headerList.Count == 0)
			{
				if (_viewType != eAllocationSelectionViewType.None)
					_trans.AllocationViewType = _viewType;
 
				this.Show();
			}
			else
			{
				// BEGIN MID Track #2551 - data security not working
				// this is actually function security that needed to be added
				bool okToContinue = true;
				switch (_viewType)
				{
					case eAllocationSelectionViewType.Style:
						if (_allocationReviewStyleSecurity.AccessDenied)
							okToContinue = false;
						break;		

					case eAllocationSelectionViewType.Summary:
						if (_allocationReviewSummarySecurity.AccessDenied)
							okToContinue = false;
						break;	
					
					case eAllocationSelectionViewType.Size:
						if (_allocationReviewSizeSecurity.AccessDenied)
							okToContinue = false;
						break;	
				}
				if (!okToContinue)
				{
					MessageBox.Show(_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_NotAuthorized));
					return;
				}
			
				// Begin TT#1037 - MD - stodd - read only security -
				// Begin TT#1154-MD - stodd - null reference when opening selection - 
                //if (_trans.ContainsGroupAllocationHeaders() && AssortmentActiveProcessToolbarHelper.ActiveProcess.screenID == Include.NoRID)
				// Begin TT#1194-MD - stodd - view ga header
                if (_viewType == eAllocationSelectionViewType.Velocity)
                {

                }
                else if (_trans.ContainsGroupAllocationHeaders())
				// ENd TT#1194-MD - stodd - view ga header
				// End TT#1154-MD - stodd - null reference when opening selection - 
                {
                    string message = MIDText.GetTextOnly(eMIDTextCode.msg_as_GroupAllocationReviewScreenReadOnly);
                    DialogResult diagResult = MessageBox.Show(message, MIDText.GetTextOnly(eMIDTextCode.msg_as_GroupAllocationHeadersReadOnly), System.Windows.Forms.MessageBoxButtons.YesNo,
                    System.Windows.Forms.MessageBoxIcon.Question);

                    if (diagResult == System.Windows.Forms.DialogResult.Yes)
                    {
                        _trans.DataState = eDataState.ReadOnly;
                    }
                    else
                    {
                        return;
                    }
                }
				// End TT#1037 - MD - stodd - read only security -
				// END MID Track #2551 
				_trans.AllocationViewType = _viewType;
				AllocationViewSelection_Load(this,args);
				// Clear the dates and merchandise columns
				//_trans.AllocationNeedAnalysisPeriodBeginRID = Include.UndefinedCalendarDateRange;
				//_trans.AllocationNeedAnalysisPeriodEndRID = Include.UndefinedCalendarDateRange;
				_trans.AllocationNeedAnalysisPeriodBeginRID = Include.NoRID;
				_trans.AllocationNeedAnalysisPeriodEndRID = Include.NoRID;
				_trans.AllocationNeedAnalysisHNID = Include.NoRID;
				_bypassScreen = true;
				btnOK_Click(this, args);
			}
			Cursor.Current = Cursors.Default;
		}
		private void AllocationViewSelection_Load(object sender, System.EventArgs e)
		{
			ArrayList userRIDList;
			try
			{
				_loading = true;
				
				// Load Filters
				//_storeFilterDL = new StoreFilterData();
                _storeFilterDL = new FilterData();
				userRIDList = new ArrayList();
				userRIDList.Add(Include.GlobalUserRID);	//  Issue 3806
				userRIDList.Add(_SAB.ClientServerSession.UserRID);
                _dtFilter = _storeFilterDL.FilterRead(filterTypes.StoreFilter, eProfileType.FilterStore, userRIDList);
				SetRbViewType();
				BindGroupByComboBox();
				BindStoreAttributeComboBox();
				BindFilterComboBox();

                //BEGIN TT#6-MD-VStuart - Single Store Select
                ProfileList storeProfileList = StoreMgmt.StoreProfiles_GetActiveStoresList(); //_SAB.StoreServerSession.GetActiveStoresList();
                BindStoreComboBox(cboStore.TabIndex, storeProfileList.ArrayList); //TT#622 - Error attempting to open in designer  .ComboBox. removed 03/11/2013
                cboStore_Enabled();
                //END TT#6-MD-VStuart - Single Store Select
               
                // Begin TT#456 - Add Views to Size Review  
                SetupViewData();
                BindViewComboBox();
                // End TT#456  

                // Begin TT#2208 - JSmith - Style Review Slow Performance
                //BindSizeCurveComboBox();
                //BindSizeConstraintsComboBox();
                //BindSizeAlternatesComboBox();
                // End TT#2208
				//cboStoreAttribute.Tag = "IgnoreMouseWheel";
                //cboFilter.Tag = "IgnoreMouseWheel";
                //Begin Track #5858 - KJohnson - Validating store security only
                // Begin TT#44 - JSmith - Drag/Drop User Attributes or Filters in to Global Methods does not react consistantly
                //cboStoreAttribute.Tag = new MIDStoreAttributeComboBoxTag(SAB, cboStoreAttribute, eMIDControlCode.form_AllocationViewSelection);
                //cboFilter.Tag = new MIDStoreFilterComboBoxTag(SAB, cboFilter, eMIDControlCode.form_AllocationViewSelection, eSecurityTypes.Store, eSecuritySelectType.View | eSecuritySelectType.Update, true);
                cboStoreAttribute.Tag = new MIDStoreAttributeComboBoxTag(SAB, cboStoreAttribute, eMIDControlCode.form_AllocationViewSelection, _allocationReviewSecurity, true);
                cboFilter.Tag = new MIDStoreFilterComboBoxTag(SAB, cboFilter.ComboBox, eMIDControlCode.form_AllocationViewSelection, eSecurityTypes.Store, eSecuritySelectType.View | eSecuritySelectType.Update, true, _allocationReviewSecurity, true);
                // End TT#44
                //End Track #5858
				cboView.Tag = "IgnoreMouseWheel";
                cboGroupBy.Tag = "IgnoreMouseWheel";
				SetChkIneligibleStore();
				SetDtpNeedAnalysisDates();
				SetTxtNeedAnalysisBasis();
				SetUpHeaderGrid();
				DefaultHeaderGridLayout();
				// make sure values are set for new users
				_trans.AllocationStoreAttributeID = Convert.ToInt32(cboStoreAttribute.SelectedValue, CultureInfo.CurrentUICulture);
                _trans.AllocationGroupBy = Convert.ToInt32(cboGroupBy.SelectedValue, CultureInfo.CurrentUICulture); //TT#6-MD-VStuart - Single Store Select
				SetText();
                
                // Begin TT#638 - RMatelic - Style Review - Add Basis Variables
                SetUpBasisGrid();
                // End TT#638 

                SetReadOnly(_allocationReviewSecurity.AllowUpdate);
				// BEGIN MID Track #2551 - data security not working
				// this is actually function security that needed to be added
				if (_allocationReviewStyleSecurity.AccessDenied)
				{
					rbStyleView.Enabled = false;
				}
				if (_allocationReviewSummarySecurity.AccessDenied)
				{
					rbSummaryView.Enabled = false;
				}
				if (_allocationReviewSizeSecurity.AccessDenied)
				{
					rbSizeView.Enabled = false;
				}
				if (rbStyleView.Enabled == false && rbSummaryView.Enabled == false && rbSizeView.Enabled == false)
					SetReadOnly(false);
				if(!_allocationReviewSecurity.AllowUpdate) 
					ugHeader.Enabled = false;

				// BEGIN MID Track #4567 - John Smith - add filtering
                // Begin TT#2208 - JSmith - Style Review Slow Performance
                cboSizeCurve.Enabled = false;
                picBoxCurve.Enabled = false;
                //SetMaskedComboBoxesEnabled();

                //if (SAB.ApplicationServerSession.GlobalOptions.SizeCurveCharMask == string.Empty)
                //{
                //    BindSizeCurveComboBox();
                //}
                //if (SAB.ApplicationServerSession.GlobalOptions.SizeConstraintCharMask == string.Empty)
                //{
                //    BindSizeConstraintsComboBox();
                //}
                //if (SAB.ApplicationServerSession.GlobalOptions.SizeAlternateCharMask == string.Empty)
                //{
                //    BindSizeAlternatesComboBox();
                //}

                if (rbSizeView.Checked)
                {
                    if (SAB.ApplicationServerSession.GlobalOptions.SizeCurveCharMask == string.Empty)
                    {
                        BindSizeCurveComboBox();
                    }
                    cboSizeCurve.Enabled = true;
                    picBoxCurve.Enabled = true;
                }
                // End TT#2208
				// END MID Track #4567


                if (!_SAB.ClientServerSession.GlobalOptions.AppConfig.SizeInstalled)
                {
                    this.rbSizeView.Enabled = false;
                    this.cboSizeCurve.Enabled = false;
                    // Begin TT#199 RMatelic-In Allocation Selection Screen, remove Size Constraints Model and Size Alternate Model windows as they have no purpose 
                    //this.cboConstraints.Enabled = false;
                    //this.cboAlternates.Enabled = false;
                    // End TT#199
                    this.rbSizeView.Visible = false;
                    this.cboSizeCurve.Visible = false;
                    this.lblSizeCurve.Visible = false;
                    // Begin TT#199 RMatelic-In Allocation Selection Screen, remove Size Constraints Model and Size Alternate Model windows as they have no purpose 
                    //this.cboConstraints.Visible = false;
                    //this.lblSizeConstraint.Visible = false;
                    //this.cboAlternates.Visible = false;
                    //this.lblSizeAlternate.Visible = false;
                    // End TT#199
                    // BEGIN MID Track #4567 - John Smith - add filtering
                    this.picBoxCurve.Visible = false;
                    // Begin TT#199 RMatelic-In Allocation Selection Screen, remove Size Constraints Model and Size Alternate Model windows as they have no purpose 
                    //this.picBoxConstraint.Visible = false;
                    //this.picBoxAlternate.Visible = false;
                    // End TT#199
                    // END MID Track #4567
                }
                // END MID Track #2551
               
                // Begin TT#199 RMatelic-In Allocation Selection Screen, remove Size Constraints Model and Size Alternate Model windows as they have no purpose 
                //  Instead of totally removing the Constraints and Alternate cotntrols, hide the controls and adjust the group box dimensions 
                this.cboConstraints.Visible = false;
                this.lblSizeConstraint.Visible = false;
                this.cboAlternates.Visible = false;
                this.lblSizeAlternate.Visible = false;
                this.picBoxConstraint.Visible = false;
                this.picBoxAlternate.Visible = false;
                // Begin TT#638 - RMatelic - Style Review - Add Basis Variables
                //this.gbxNeedAnalysis.Height = 122;
                //this.gbxAllocHeaders.Location = new System.Drawing.Point(8, this.gbxNeedAnalysis.Bottom + 10);
                //this.gbxAllocHeaders.Height = 298;
                // End TT#199
                // End TT#638 

                //Begin Track #5858 - KJohnson - Validating store security only
                txtBasis.Tag = new MIDMerchandiseTextBoxTag(SAB, txtBasis, eMIDControlCode.form_Forecast_Balance_Model, eSecurityTypes.Store, eSecuritySelectType.View | eSecuritySelectType.Update);

                // Begin TT#1985 - JSmith - Unhandled exception error (attached) when selecting the alternate hierarchy for use as the basis during size review
                ((MIDTag)(txtBasis.Tag)).MIDTagData = _hnp;
                // End TT#1985

				//Begin Track #5858 stodd- removed ApplySecurity(). It was placed here by mistake.
                //ApplySecurity();
				//End Track #5858
                //End Track #5858
            }
			catch ( Exception ex )
			{
				HandleException(ex);
			}
			finally 
			{
				_loading = false;
			}
				  
		}
		private void SetRbViewType()
		{
			if (_allocationReviewStyleSecurity.AccessDenied)
			{
				rbStyleView.Enabled = false;
			}
			if (_allocationReviewSummarySecurity.AccessDenied)
			{
				rbSummaryView.Enabled = false;
			}
			if (_allocationReviewSizeSecurity.AccessDenied)
			{
				rbSizeView.Enabled = false;
			}
			switch (_trans.AllocationViewType)
			{
				case eAllocationSelectionViewType.Style:
				case eAllocationSelectionViewType.Velocity:
					if (rbStyleView.Enabled)
					{
						rbStyleView.Checked = true;
					}
					else if (rbSummaryView.Enabled)
					{
						rbSummaryView.Checked = true;
					}
					else if (rbSizeView.Enabled)
					{
						rbSizeView.Checked = true;
					}
					break;
				case eAllocationSelectionViewType.Size:
					if (rbSizeView.Enabled)
					{
						rbSizeView.Checked = true;
					}
					else if (rbStyleView.Enabled)
					{
						rbStyleView.Checked = true;
					}
					else if (rbSummaryView.Enabled)
					{
						rbSummaryView.Checked = true;
					}
					break;
				case eAllocationSelectionViewType.Summary:
					if (rbSummaryView.Enabled == true)
					{
						rbSummaryView.Checked = true;
					}
					else if (rbStyleView.Enabled)
					{
						rbStyleView.Checked = true;
					}
					else if (rbSizeView.Enabled)
					{
						rbSizeView.Checked = true;
					}
					break;
				case eAllocationSelectionViewType.None:
					_trans.AllocationViewType = eAllocationSelectionViewType.Style;
					if (rbStyleView.Enabled == true)
					{
						rbStyleView.Checked = true;
					}
					break;
					//default:
					//	_trans.AllocationViewType = eAllocationSelectionViewType.Style;
					//	rbStyleView.Checked = true;
					//	break;
			}
		}

		private void BindGroupByComboBox()
		{

            cboGroupBy.DataSource = null;
			DataTable dtGroupBy = MIDEnvironment.CreateDataTable();

			//dtGroupBy = MIDText.GetTextType((eMIDTextType)_trans.AllocationViewType, eMIDTextOrderBy.TextCode);
			// Changed above statement to account for Velocity view
			eAllocationSelectionViewType viewType;
			if (_trans.AllocationViewType == eAllocationSelectionViewType.Velocity)
				viewType = eAllocationSelectionViewType.Style;
			else
				viewType = _trans.AllocationViewType;
			dtGroupBy = MIDText.GetTextType((eMIDTextType)viewType, eMIDTextOrderBy.TextCode);

            cboGroupBy.Items.Clear();
            cboGroupBy.DataSource = dtGroupBy;
            cboGroupBy.DisplayMember = "TEXT_VALUE";
            cboGroupBy.ValueMember = "TEXT_CODE";
			if (_trans.AllocationGroupBy == Include.UndefinedGroupByRID)
                cboGroupBy.SelectedIndex = 0;
			else
			{
                cboGroupBy.SelectedValue = _trans.AllocationGroupBy;
                if (cboGroupBy.SelectedValue == null)
                    cboGroupBy.SelectedIndex = 0;
			}
		}

		private void BindStoreAttributeComboBox()
		{
            // Begin Track #4872 - JSmith - Global/User Attributes
            FunctionSecurityProfile userAttrSecLvl;
            // End Track #4872
			try
			{
//Begin Track #3767 - JScott - Force client to use cached store group lists in application session
//				ProfileList a1 = _SAB.StoreServerSession.GetStoreGroupListViewList();
                // Begin Track #4872 - JSmith - Global/User Attributes
                userAttrSecLvl = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AdminStoreAttributesUser);
                //ProfileList a1 = _SAB.ApplicationServerSession.GetProfileList(eProfileType.StoreGroupListView);
				//Begin TT#1517-MD -jsobek -Store Service Optimization
                //ProfileList a1 = _SAB.ApplicationServerSession.GetProfileList(eProfileType.StoreGroupListView, !userAttrSecLvl.AccessDenied);
                // Begin TT#1801-MD - JSmith - Allocation > Review > Select - Str Attribute Drop Down is showing OTHER users MY Attributes
                //ProfileList a1 = StoreMgmt.StoreGroup_GetListViewList(eStoreGroupSelectType.All, !userAttrSecLvl.AccessDenied);
                ProfileList a1 = StoreMgmt.StoreGroup_GetListViewList(eStoreGroupSelectType.MyUserAndGlobal, !userAttrSecLvl.AccessDenied);
                // End TT#1801-MD - JSmith - Allocation > Review > Select - Str Attribute Drop Down is showing OTHER users MY Attributes
				//End TT#1517-MD -jsobek -Store Service Optimization
                // End Track #4872
//End Track #3767 - JScott - Force client to use cached store group lists in application session
                // Begin Track #4872 - JSmith - Global/User Attributes
                //cboStoreAttribute.ValueMember = "Key";
                //cboStoreAttribute.DisplayMember = "Name";
                //cboStoreAttribute.DataSource = a1.ArrayList;
                cboStoreAttribute.Initialize(SAB, _allocationReviewSecurity, a1.ArrayList, true);
                AdjustTextWidthComboBox_DropDown(cboStoreAttribute);     //TT#7 - MD - Custom Control - rbeck
                // End Track #4872
				if (_trans.AllocationStoreAttributeID == Include.NoRID)
				{
					//					StoreGroupProfile sgp = (StoreGroupProfile)a1[0];
					//					cboStoreAttribute.SelectedValue = sgp.Key;
					//cboStoreAttribute.SelectedValue = _SAB.ApplicationServerSession.GlobalOptions.AllocationStoreGroupRID; // MID Change j.ellis Serialization error
					cboStoreAttribute.SelectedValue = _SAB.ClientServerSession.GlobalOptions.AllocationStoreGroupRID; // MID Change j.ellis Serialization error
				}
				else
				{
					cboStoreAttribute.SelectedValue = _trans.AllocationStoreAttributeID;
				}
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}

		private void BindFilterComboBox()
		{
			try
			{
                cboFilter.Items.Clear();
                // Begin TT#2669 - JSmith - Unable to remove a filter from a wokflow
                //cboFilter.Items.Add(new FilterNameCombo(-1, Include.GlobalUserRID, "(None)"));  // Issue 3806
                cboFilter.Items.Add(GetRemoveFilterRow());
                // End TT#2669 - JSmith - Unable to remove a filter from a wokflow

				foreach (DataRow row in _dtFilter.Rows)
				{
                    cboFilter.Items.Add(
						new FilterNameCombo(Convert.ToInt32(row["FILTER_RID"], CultureInfo.CurrentUICulture),
						Convert.ToInt32(row["USER_RID"], CultureInfo.CurrentUICulture),
						Convert.ToString(row["FILTER_NAME"], CultureInfo.CurrentUICulture)));
				}
				if (_trans.AllocationFilterID == Include.NoRID)
                    cboFilter.SelectedIndex = -1;
				else
				{
                    cboFilter.SelectedIndex = cboFilter.Items.IndexOf(new FilterNameCombo(_trans.AllocationFilterID, -1, ""));
				}
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}

        //BEGIN TT#6-MD-VStuart - Single Store Select //This is my tweaked change on 20120319.
        private void BindStoreComboBox(int aComponentIdx, ArrayList aValueList)
        {
            try
            {
                cboStore.Items.Clear();
                cboStore.Items.Add("(None)");

                //This section gives the user a complete list by clicking the dropdown.
                foreach (object obj in aValueList)
                { cboStore.Items.Add(obj); }

                AdjustTextWidthComboBox_DropDown(cboStore.ComboBox);
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }
        //END TT#6-MD-VStuart - Single Store Select

        // Begin TT#456 - Add Views to Size Review
        private void SetupViewData()
        {
            try
            {
                _dtStyleViews = GetViewDataTable(eLayoutID.styleReviewGrid);
                _dtSizeViews = GetViewDataTable(eLayoutID.sizeReviewGrid);

                _userGridView = new UserGridView();
                _styleViewRID = _userGridView.UserGridView_Read(SAB.ClientServerSession.UserRID, eLayoutID.styleReviewGrid);
                _sizeViewRID = _userGridView.UserGridView_Read(SAB.ClientServerSession.UserRID, eLayoutID.sizeReviewGrid);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }
        
        private DataTable GetViewDataTable(eLayoutID aLayoutID)
        {
            try
            {
                FunctionSecurityProfile globalViewSecurity = null;
                FunctionSecurityProfile userViewSecurity = null;
                ArrayList userRIDList = new ArrayList();
                GridViewData gridViewData = new GridViewData();

                switch (aLayoutID)
                {
                    case eLayoutID.styleReviewGrid:
                        globalViewSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AllocationViewsGlobalStyleReview);
                        userViewSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AllocationViewsUserStyleReview);
                        break;

                    case eLayoutID.sizeReviewGrid:
                        globalViewSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AllocationViewsGlobalSizeReview);
                        userViewSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AllocationViewsUserSizeReview);
                        break;
                }
                if (globalViewSecurity.AllowView)
                {
                    userRIDList.Add(Include.GlobalUserRID);
                }
                if (userViewSecurity.AllowView)
                {
                    userRIDList.Add(SAB.ClientServerSession.UserRID);
                }

                // Begin TT#1117 - JSmith - Global & User Views w/ the same names do not have indicators
                //DataTable dtViews = gridViewData.GridView_Read((int)aLayoutID, userRIDList);
                DataTable dtViews = gridViewData.GridView_Read((int)aLayoutID, userRIDList, true);
                // End TT#1117
                dtViews.Rows.Add(new object[] { Include.NoRID, SAB.ClientServerSession.UserRID, _layoutID, string.Empty });
                dtViews.PrimaryKey = new DataColumn[] {dtViews.Columns["VIEW_RID"] };
                return dtViews;
            }
            catch
            {
                throw;
            }
        }

		private void BindViewComboBox()
		{
            try
            {
                cboView.ValueMember = "VIEW_RID";
                cboView.DisplayMember = "VIEW_ID";

                _bindingView = true;
                if (rbStyleView.Checked)
                {
                    cboView.DataSource = _dtStyleViews;
                    cboView.SelectedValue = _styleViewRID;
                }
                else if (rbSizeView.Checked)
                {
                    cboView.DataSource = _dtSizeViews;
                    cboView.SelectedValue = _sizeViewRID;
                }
                else
                {
                    cboView.DataSource = null;
                }
                SetGroupByFromView();
                _bindingView = false;
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
		}

        // Begin TT#456 - RMatelic - Add view to Size Review 
        private void SetGroupByFromView()
        {
            try
            {
                DataRow dRow = null;
                if (rbStyleView.Checked)
                {
                    dRow = _dtStyleViews.Rows.Find(_styleViewRID);
                }
                else if (rbSizeView.Checked)
                {
                    dRow = _dtSizeViews.Rows.Find(_sizeViewRID);
                }

                if (dRow != null && dRow["GROUP_BY"] != DBNull.Value)
                {
                    cboGroupBy.SelectedValue = Convert.ToInt32(dRow["GROUP_BY"], CultureInfo.CurrentUICulture);
                }
            }
            catch
            {
                throw;
            }
        }    
        // End TT#456

        private void cboView_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                if (!_bindingView)
                {
                    if (rbStyleView.Checked)
                    {
                        _styleViewRID = Convert.ToInt32(cboView.SelectedValue, CultureInfo.CurrentUICulture);
                    }
                    else if (rbSizeView.Checked)
                    {
                        _sizeViewRID = Convert.ToInt32(cboView.SelectedValue, CultureInfo.CurrentUICulture);
                    }
                    SetGroupByFromView();
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        // End TT#456  
        //Begin TT#316 - MD - DOConnell - Replace all Windows Combobox controls with new enhanced control
        void cboView_MIDComboBoxPropertiesChangedEvent(object source, MIDComboBoxPropertiesChangedEventArgs args)
        {
            this.cboView_SelectionChangeCommitted(source, new EventArgs());
        }
        //End TT#316 - MD - DOConnell - Replace all Windows Combobox controls with new enhanced control

		// BEGION MID Track #2959 - add Size Need Analysis
		/// <summary>
		/// Populates the size curve combo box.
		/// </summary>
		public void BindSizeCurveComboBox()
		{
			try
			{
				DataTable dtSizeCurve = MIDEnvironment.CreateDataTable();
				SizeCurve objSizeCurve = new SizeCurve();
				dtSizeCurve = objSizeCurve.GetSizeCurveGroups();
				DataRow emptyRow = dtSizeCurve.NewRow();
				emptyRow["SIZE_CURVE_GROUP_NAME"] = string.Empty;
				emptyRow["SIZE_CURVE_GROUP_RID"] = Include.NoRID;
				dtSizeCurve.Rows.Add(emptyRow);
				dtSizeCurve.DefaultView.Sort = "SIZE_CURVE_GROUP_NAME ASC"; 
				dtSizeCurve.AcceptChanges();
                cboSizeCurve.DataSource = dtSizeCurve;
                cboSizeCurve.DisplayMember = "SIZE_CURVE_GROUP_NAME";
                cboSizeCurve.ValueMember = "SIZE_CURVE_GROUP_RID";

                cboSizeCurve.SelectedIndex = 0;
			}		
			catch (Exception ex)
			{
				HandleException(ex, "BindSizeCurveComboBox");
			}
		}

		// BEGIN MID Track #4567 - John Smith - add filtering
		public void BindSizeCurveComboBox(bool includeEmptySelection, string aFilterString, bool aCaseSensitive)
		{
			try
			{
				DataTable dtSizeCurve = MIDEnvironment.CreateDataTable();
				SizeCurve objSizeCurve = new SizeCurve();
                object selectedValue = cboSizeCurve.SelectedValue;
								
				// RowFilter didn't work with multiple wild cards 
				aFilterString = aFilterString.Replace("*","%");
                //aFilterString = aFilterString.Replace("'","''");	// for string with single quote
			
                //string whereClause = "SIZE_CURVE_GROUP_NAME LIKE " +  "'" + aFilterString + "'";	
                //if (!aCaseSensitive)
                //{
                //    whereClause += Include.CaseInsensitiveCollation;
                //}
				
                //dtSizeCurve = objSizeCurve.GetFilteredSizeCurveGroups(whereClause);
                if (aCaseSensitive)
                {
                    dtSizeCurve = objSizeCurve.GetFilteredSizeCurveGroupsCaseSensitive(aFilterString);
                }
                else
                {
                    dtSizeCurve = objSizeCurve.GetFilteredSizeCurveGroupsCaseInsensitive(aFilterString);
                }

				if (includeEmptySelection)
				{							
					dtSizeCurve.Rows.Add(new object[] { Include.NoRID, ""} );
				}

				DataView dvSizeCurve = new DataView(dtSizeCurve, "", "SIZE_CURVE_GROUP_NAME", DataViewRowState.CurrentRows);
                cboSizeCurve.DataSource = dvSizeCurve;
                cboSizeCurve.DisplayMember = "SIZE_CURVE_GROUP_NAME";
                cboSizeCurve.ValueMember = "SIZE_CURVE_GROUP_RID";
				if (selectedValue != null)
				{
                    cboSizeCurve.SelectedValue = selectedValue;
				}
                cboSizeCurve.Enabled = true;
			}		
			catch (Exception ex)
			{
				HandleException(ex, "BindSizeCurveComboBox");
			}
		}
		// END MID Track #4567

        // Begin TT#2208 - JSmith - Style Review Slow Performance
        //public void BindSizeConstraintsComboBox()
        //{
        //    try
        //    {
        //        DataTable dtSizeModel = SizeModelData.SizeConstraintModel_Read();
        //        DataRow emptyRow = dtSizeModel.NewRow();
        //        emptyRow["SIZE_CONSTRAINT_NAME"] = "";
        //        emptyRow["SIZE_CONSTRAINT_RID"] = Include.NoRID;
        //        dtSizeModel.Rows.Add(emptyRow);
        //        dtSizeModel.DefaultView.Sort = "SIZE_CONSTRAINT_NAME ASC"; 
        //        dtSizeModel.AcceptChanges();

        //        cboConstraints.DataSource = dtSizeModel;
        //        cboConstraints.DisplayMember = "SIZE_CONSTRAINT_NAME";
        //        cboConstraints.ValueMember = "SIZE_CONSTRAINT_RID";

        //        cboConstraints.SelectedIndex = 0;
        //    }		
        //    catch (Exception ex)
        //    {
        //        HandleException(ex, "BindSizeConstraintsComboBox");
        //    }
        //}
        // End TT#2208

		// BEGIN MID Track #4567 - John Smith - add filtering
        // Begin TT#2208 - JSmith - Style Review Slow Performance
        //private void BindSizeConstraintsComboBox(bool includeEmptySelection, string aFilterString, bool aCaseSensitive)
        //{
        //    try
        //    {	
        //        // Originally tried RowFilter but received invalid rowfilter msg when multiple wild cards
        //        aFilterString = aFilterString.Replace("*","%");
        //        aFilterString = aFilterString.Replace("'","''");	// for string with single quote

        //        string whereClause = "SIZE_CONSTRAINT_NAME LIKE " +  "'" + aFilterString + "'";
        //        if (!aCaseSensitive)
        //        {
        //            whereClause += Include.CaseInsensitiveCollation;
        //        }
        //        DataTable dtSizeModel = SizeModelData.SizeConstraintModel_FilterRead(whereClause);

        //        if (includeEmptySelection)
        //        {
        //            DataRow emptyRow = dtSizeModel.NewRow();
        //            emptyRow["SIZE_CONSTRAINT_NAME"] = "";
        //            emptyRow["SIZE_CONSTRAINT_RID"] = Include.NoRID;
        //            dtSizeModel.Rows.Add(emptyRow);
        //        }
        //        dtSizeModel.DefaultView.Sort = "SIZE_CONSTRAINT_NAME ASC"; 
        //        dtSizeModel.AcceptChanges();

        //        cboConstraints.DataSource = dtSizeModel;
        //        cboConstraints.DisplayMember = "SIZE_CONSTRAINT_NAME";
        //        cboConstraints.ValueMember = "SIZE_CONSTRAINT_RID";
        //        cboConstraints.Enabled = true;
        //    }		
        //    catch (Exception ex)
        //    {
        //        HandleException(ex, "BindSizeConstraintsComboBox");
        //    }
        //}
        // ENd TT#2208
		// END MID Track #4567

        // Begin TT#2208 - JSmith - Style Review Slow Performance
        //public void BindSizeAlternatesComboBox()
        //{
        //    try
        //    {
        //        DataTable dtSizeModel = SizeModelData.SizeAlternateModel_Read();
        //        DataRow emptyRow = dtSizeModel.NewRow();
        //        emptyRow["SIZE_ALTERNATE_NAME"] = "";
        //        emptyRow["SIZE_ALTERNATE_RID"] = Include.NoRID;
        //        dtSizeModel.Rows.Add(emptyRow);
        //        dtSizeModel.DefaultView.Sort = "SIZE_ALTERNATE_NAME ASC"; 
        //        dtSizeModel.AcceptChanges();

        //        cboAlternates.DataSource = dtSizeModel;
        //        cboAlternates.DisplayMember = "SIZE_ALTERNATE_NAME";
        //        cboAlternates.ValueMember = "SIZE_ALTERNATE_RID";

        //        cboAlternates.SelectedIndex = 0;
        //    }		
        //    catch (Exception ex)
        //    {
        //        HandleException(ex, "BindSizeAlternatesComboBox");
        //    }
        //}

        //// BEGIN MID Track #4567 - John Smith - add filtering
        //public void BindSizeAlternatesComboBox(bool includeEmptySelection, string aFilterString, bool aCaseSensitive)
        //{
        //    try
        //    {	 
        //        // Originally tried RowFilter but received invalid rowfilter msg when multiple wild cards
        //        aFilterString = aFilterString.Replace("*","%");
        //        aFilterString = aFilterString.Replace("'","''");	// for string with single quote

        //        string whereClause = "SIZE_ALTERNATE_NAME LIKE " +  "'" + aFilterString + "'";
        //        if (!aCaseSensitive)
        //        {
        //            whereClause += Include.CaseInsensitiveCollation;
        //        }
        //        DataTable dtSizeModel = SizeModelData.SizeAlternateModel_FilterRead(whereClause);

        //        if (includeEmptySelection)
        //        {							
        //            DataRow emptyRow = dtSizeModel.NewRow();
        //            emptyRow["SIZE_ALTERNATE_NAME"] = "";
        //            emptyRow["SIZE_ALTERNATE_RID"] = Include.NoRID;
        //            dtSizeModel.Rows.Add(emptyRow);
        //        }
        //        dtSizeModel.DefaultView.Sort = "SIZE_ALTERNATE_NAME ASC"; 
        //        dtSizeModel.AcceptChanges();
		
        //        cboAlternates.DataSource = dtSizeModel;
        //        cboAlternates.DisplayMember = "SIZE_ALTERNATE_NAME";
        //        cboAlternates.ValueMember = "SIZE_ALTERNATE_RID";
        //        cboAlternates.Enabled = true;
        //    }		
        //    catch (Exception ex)
        //    {
        //        HandleException(ex, "BindSizeAlternatesComboBox");
        //    }
        //}
        //// END MID Track #4567
        // Begin TT#2208 - JSmith - Style Review Slow Performance

		private void SetChkIneligibleStore()
		{
			chkIneligibleStores.Checked = _trans.AllocationIncludeIneligibleStores;
		}
	
		private void SetDtpNeedAnalysisDates()
		{
			DateRangeProfile drp;
			
			drsBegin.DateRangeRID = _trans.AllocationNeedAnalysisPeriodBeginRID;
			drp = _SAB.ClientServerSession.Calendar.GetDateRange(_trans.AllocationNeedAnalysisPeriodBeginRID);
			if (drp != null)
			{
				drsBegin.Text = drp.DisplayDate;
			}

			drsEnd.DateRangeRID = _trans.AllocationNeedAnalysisPeriodEndRID;
			drp = _SAB.ClientServerSession.Calendar.GetDateRange(_trans.AllocationNeedAnalysisPeriodEndRID);
			if (drp != null) 
			{
				drsEnd.Text = drp.DisplayDate;
			}
		}

		private void SetTxtNeedAnalysisBasis()
		{
			if (_trans.AllocationNeedAnalysisHNID == Include.NoRID)
			{
				txtBasis.Clear();
			}
			else
			{
                // BEGIN MID Track #6102 - color & size node display node not fully qualified
                //_hnp = _trans.GetNodeData(_trans.AllocationNeedAnalysisHNID);
                _hnp = _SAB.HierarchyServerSession.GetNodeData(_trans.AllocationNeedAnalysisHNID, true, true);
                // END MID Track #6102
				txtBasis.Text = _hnp.Text;
				_lastValidBasisRID = _trans.AllocationNeedAnalysisHNID; // MID Track #4311
			}

		}
		private void SetText()
		{
			this.Text = MIDText.GetTextOnly(eMIDTextCode.frm_AllocationViewSelection);

			this.btnOK.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_OK); 
			this.btnCancel.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_Cancel); 

			this.rbStyleView.Text = MIDText.GetTextOnly(eMIDTextCode.menu_Allocation_Style);
			this.rbSummaryView.Text = MIDText.GetTextOnly(eMIDTextCode.menu_Allocation_Summary);
			this.rbSizeView.Text = MIDText.GetTextOnly(eMIDTextCode.menu_Allocation_Size);
			this.gbxDisplay.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Display_Option);
			this.lblAttribute.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Store_Attribute); 
			this.lblFilter.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Filter); 
			this.lblGroupBy.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_GroupBy); 
			this.lblView.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_View); 
			this.chkIneligibleStores.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Ineligible) + " " + 
				MIDText.GetTextOnly(eMIDTextCode.lbl_Stores); 

			this.gbxNeedAnalysis.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_NeedAnalysis); 
			this.lblTimeBegin.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_TimePeriodBegin); 
			this.lblTimeEnd.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_TimePeriodEnd); 
			this.lblMerchandise.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Merchandise) + " " + 
				MIDText.GetTextOnly(eMIDTextCode.lbl_Basis);
            this.lblStore.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Stores);  //TT#6-MD-VStuart - Single Store Select
		
			this.gbxAllocHeaders.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_AllocHeaders); 
			// BEGIN MID Track #2959 - Add Size Need Analysis
			this.lblSizeCurve.Text =  MIDText.GetTextOnly(eMIDTextCode.lbl_SizeCurve); 
			this.lblSizeConstraint.Text =  MIDText.GetTextOnly(eMIDTextCode.lbl_ConstraintsModel); 
			this.lblSizeAlternate.Text =  MIDText.GetTextOnly(eMIDTextCode.lbl_AlternatesModel); 
			// END MID Track #2959 

            // Begin TT#638 - RMatelic - Style Review - Add Basis Variables
            cmsBasisItemInsert.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Insert);
            cmsBasisItemInsertBefore.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Insert_Before);
            cmsBasisItemInsertAfter.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Insert_after);
            cmsBasisItemDelete.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Delete);
            cmsBasisItemDeleteAll.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_DeleteAll);
            // End TT#638 
		}	

		#endregion

		private void cboStoreAttribute_SelectionChangeCommitted(object sender, System.EventArgs e)
		{
			if (! _loading)
			{
				_trans.AllocationStoreAttributeID = Convert.ToInt32(cboStoreAttribute.SelectedValue, CultureInfo.CurrentUICulture);
			}
		}

        //Begin TT#316 - MD - DOConnell - Replace all Windows Combobox controls with new enhanced control
        void cboStoreAttribute_MIDComboBoxPropertiesChangedEvent(object source, MIDComboBoxPropertiesChangedEventArgs args)
        {
            this.cboStoreAttribute_SelectionChangeCommitted(source, new EventArgs());
        }
        //End TT#316 - MD - DOConnell - Replace all Windows Combobox controls with new enhanced control

        private void cboStoreAttribute_DragDrop(object sender, DragEventArgs e)
        {
            //Begin Track #5858 - Kjohnson - Validating store security only
            try
            {
                bool isSuccessfull = ((MIDComboBoxTag)(((ComboBox)sender).Tag)).ComboBox_DragDrop(sender, e);

                if (isSuccessfull)
                {
                    //ChangePending = true;
                }
            }
            catch (Exception exc)
            {
                HandleException(exc);
            }
            //End Track #5858
        }

		private void cboGroupBy_SelectionChangeCommitted(object sender, System.EventArgs e)
		{
			if (! _loading)
			{
                _trans.AllocationGroupBy = Convert.ToInt32(cboGroupBy.SelectedValue, CultureInfo.CurrentUICulture);
			}
		}

		private void cboFilter_SelectionChangeCommitted(object sender, System.EventArgs e)
		{
			try
			{
                if (cboFilter.SelectedIndex != -1)
				{
                    if (((FilterNameCombo)cboFilter.SelectedItem).FilterRID == -1)
					{
                        cboFilter.SelectedIndex = -1;
					}
				}
                if (cboFilter.SelectedIndex != -1)
                    _trans.AllocationFilterID = ((FilterNameCombo)cboFilter.SelectedItem).FilterRID;
				else
					_trans.AllocationFilterID = Include.NoRID;
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}

        //Begin TT#316 - MD - DOConnell - Replace all Windows Combobox controls with new enhanced control
        void cboFilter_MIDComboBoxPropertiesChangedEvent(object source, MIDComboBoxPropertiesChangedEventArgs args)
        {
            this.cboFilter_SelectionChangeCommitted(source, new EventArgs());
        }
        //End TT#316 - MD - DOConnell - Replace all Windows Combobox controls with new enhanced control

        private void cboFilter_DragEnter(object sender, System.Windows.Forms.DragEventArgs e)
        {
            Image_DragEnter(sender, e);
        }

        private void cboFilter_DragDrop(object sender, System.Windows.Forms.DragEventArgs e)
        {
            //Begin Track #5858 - Kjohnson - Validating store security only
            try
            {
                bool isSuccessfull = ((MIDComboBoxTag)(((ComboBox)sender).Tag)).ComboBox_DragDrop(sender, e);

                if (isSuccessfull)
                {
                    ((MIDComboBoxEnh)((ComboBox)sender).Parent).FirePropertyChangeEvent();
                    //ChangePending = true;
                }
            }
            catch (Exception exc)
            {
                HandleException(exc);
            }
            //End Track #5858
        }

		private void rbStyleView_CheckedChanged(object sender, System.EventArgs e)
		{
			if (rbStyleView.Checked) 
			{
				_trans.AllocationViewType = eAllocationSelectionViewType.Style;
				_trans.AllocationGroupBy = Convert.ToInt32(eAllocationStyleViewGroupBy.Header, CultureInfo.CurrentUICulture);
				BindGroupByComboBox();
                BindViewComboBox();     // TT#456 - RMatelic - Add Views to Size Review  

                // Begin TT#2208 - JSmith - Style Review Slow Performance
                cboSizeCurve.Enabled = false;
                picBoxCurve.Enabled = false;
                // End TT#2208
			}
            cboStore_Enabled();     //TT#6-MD-VStuart - Single Store Select
        }
		private void rbSizeView_CheckedChanged(object sender, System.EventArgs e)
		{
			if (!_loading)
			{
				if (rbSizeView.Checked) 
				{
					_trans.AllocationViewType = eAllocationSelectionViewType.Size;
					_trans.AllocationGroupBy = Convert.ToInt32(eAllocationSizeViewGroupBy.Header, CultureInfo.CurrentUICulture);
					BindGroupByComboBox();
                    BindViewComboBox();     // TT#456 - RMatelic - Add Views to Size Review  

                    // Begin TT#2208 - JSmith - Style Review Slow Performance
                    if (_SAB.ApplicationServerSession.GlobalOptions.SizeCurveCharMask == string.Empty &&
                        cboSizeCurve.DataSource == null)
                    {
                        try
                        {
                            Cursor.Current = Cursors.WaitCursor;
                            BindSizeCurveComboBox();
                        }
                        catch
                        {
                            throw;
                        }
                        finally
                        {
                            Cursor.Current = Cursors.Default;
                        }
                    }
                    cboSizeCurve.Enabled = true;
                    picBoxCurve.Enabled = true;
                    // End TT#2208
				}
			}
			 cboStore_Enabled();     //TT#6-MD-VStuart - Single Store Select
		}
		private void rbSummaryView_CheckedChanged(object sender, System.EventArgs e)
		{
			if (!_loading)
			{
				if ( rbSummaryView.Checked) 
				{
					_trans.AllocationViewType = eAllocationSelectionViewType.Summary;
					_trans.AllocationGroupBy = Convert.ToInt32(eAllocationSummaryViewGroupBy.Attribute, CultureInfo.CurrentUICulture);
					BindGroupByComboBox();
                    BindViewComboBox();     // TT#456 - RMatelic - Add Views to Size Review  

                    // Begin TT#2208 - JSmith - Style Review Slow Performance
                    cboSizeCurve.Enabled = false;
                    picBoxCurve.Enabled = false;
                    // End TT#2208
				}
			}
		}

        //BEGIN TT#6-MD-VStuart - Single Store Select
        private void cboStore_Enabled()
        {
                if (rbSizeView.Checked || rbStyleView.Checked)
                {
                    cboStore.Enabled = true;
                }
                else cboStore.Enabled = false;
        }
        //END TT#6-MD-VStuart - Single Store Select

		private void btnOK_Click(object sender, System.EventArgs e)
		{
			try
			{	
				//				System.Windows.Forms.Form frm = null;
				MIDFormBase frm = null;
				System.Windows.Forms.Form parentForm;
				System.ComponentModel.CancelEventArgs args = new CancelEventArgs();
				if (!ValidateSpecificFields())
				{
					string text = _SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ErrorsFoundReviewCorrect);
					MessageBox.Show(text);
					return;
				}

				_trans.AllocationFilterTable = _dtFilter.Copy();
				_trans.AnalysisOnly = cbxAnalysis.Checked;
				// BEGIN MID Track #2959 - Add Size Need Analysis
                if (cboSizeCurve.SelectedValue != null)
				{
                    _trans.SizeCurveRID = Convert.ToInt32(cboSizeCurve.SelectedValue, CultureInfo.CurrentUICulture);
				}
                // Begin TT#2208 - JSmith - Style Review Slow Performance
                //if (cboConstraints.SelectedValue != null)
                //{
                //    _trans.SizeConstraintRID = Convert.ToInt32(cboConstraints.SelectedValue, CultureInfo.CurrentUICulture);
				
                //}
                //if (cboAlternates.SelectedValue != null)
                //{
                //    _trans.SizeAlternateRID = Convert.ToInt32(cboAlternates.SelectedValue, CultureInfo.CurrentUICulture);
                //}
				// END MID Track #2959  
                // End TT#2208

				if (!_trans.VelocityCriteriaExists)
				{
					if (cbxAnalysis.Checked)
					{
						_headerList.Clear();
						AllocationHeaderProfile ahp = new AllocationHeaderProfile(Include.DefaultHeaderRID);
						_headerList.Add(ahp);
					}
					// begin MID Track 4227 Enq/Deq not working
					//_trans.SetCriteriaHeaderList(_headerList);
					//_trans.UpdateAllocationViewSelectionHeaders();
					// end MID Track 4227 Enq/Deq not working
				}
				
				if (_trans.DataState == eDataState.New || _trans.DataState == eDataState.Updatable)
				{
					if (!OKToProcess())
					{
						if (_bypassScreen)
						{
							this.Close();
							this.MdiParent = null;
							// call dispose since form is not displayed
                            //this.Dispose();   // V 3.0 Merge  
						}
						return;
					}
				}
				// begin MID Track 4227 Enq/Deq not working
				if (!_trans.VelocityCriteriaExists)
				{
					// begin MID Track 4210 Intransit incorrectly displayed
					AllocationSubtotalProfileList subtotalList = (AllocationSubtotalProfileList)_trans.GetMasterProfileList(eProfileType.AllocationSubtotal);
					if (subtotalList != null)
					{
						foreach (AllocationSubtotalProfile asp in subtotalList)
						{
							asp.RemoveAllSubtotalMembers();
						}
						_trans.RemoveAllocationSubtotalProfileList();
					}
					// end MID Track 4210 Intransit incorrectly displayed
					_trans.SetCriteriaHeaderList(_headerList);
					_trans.UpdateAllocationViewSelectionHeaders();
					//BEGIN MID TRack 4261 - error opening AnalysisOnly; 
					//     solution is to qualify executing method
					//_trans.NewCriteriaHeaderList();  // keep allocation header list in sync with allocation profiles
					if (!cbxAnalysis.Checked)
					{
						_trans.NewCriteriaHeaderList();  // keep allocation header list in sync with allocation profiles
					}
					//END MID TRack 4261
				}
				// end MID Track 4227 Enq/Deq not working

                //BEGIN TT#6-MD-VStuart - Single Store Select
                if (cboStore.SelectedIndex > 0)
                {
                    ProfileList selectedStores = new ProfileList(eProfileType.AllocationQuickFilter);
                    StoreProfile singleStore = (StoreProfile)cboStore.SelectedItem;
                    selectedStores.Add(singleStore);
                    //Now lets get the reserve store.
                    int rsRID = _trans.ReserveStore.RID;
                    StoreProfile reserveStore = StoreMgmt.StoreProfile_Get(rsRID); // _SAB.StoreServerSession.GetStoreProfile(rsRID);
                    if (singleStore != reserveStore)
                    {
                        selectedStores.Add(reserveStore);
                    }
                    _trans.RefreshProfileLists(eProfileType.AllocationQuickFilter);
                    _trans.SetMasterProfileList(selectedStores);
                }
                //END TT#6-MD-VStuart - Single Store Select

                SaveSelectionToDatabase();

				Cursor.Current = Cursors.WaitCursor;
				// Close this form
				parentForm = this.MdiParent;
				this.Close();
				this.MdiParent = null;
                //if (_bypassScreen)
                //{
                //    // call dispose since form is not displayed
                //    Dispose();    // V 3.0 Merge  
                //}
				try
				{
                    // Begin TT#3861 - stodd - Allocation - Select not allowed in GA
                    if (_trans.DataState != eDataState.ReadOnly && _trans.ContainsGroupAllocationHeaders())
                    {
                        // Begin TT#3912 - stodd - Velocity Store Detail is read only
                        if (!_trans.VelocityCriteriaExists)
                        // End TT#3912 - stodd - Velocity Store Detail is read only
                        {
                            string message = MIDText.GetTextOnly(eMIDTextCode.msg_as_GroupAllocationReviewScreenReadOnly);
                            DialogResult diagResult = MessageBox.Show(message, MIDText.GetTextOnly(eMIDTextCode.msg_as_GroupAllocationHeadersReadOnly), System.Windows.Forms.MessageBoxButtons.YesNo,
                            System.Windows.Forms.MessageBoxIcon.Question);

                            if (diagResult == System.Windows.Forms.DialogResult.Yes)
                            {
                                _trans.DataState = eDataState.ReadOnly;
                            }
                            else
                            {
                                _trans.DequeueHeaders();
                                Cursor.Current = Cursors.Default;
                                return;
                            }
                        }
                    }
                    // End TT#3861 - stodd - Allocation - Select not allowed in GA

					switch (_trans.AllocationViewType)
					{
						case eAllocationSelectionViewType.Style:
						case eAllocationSelectionViewType.Velocity:
							frm = new MIDRetail.Windows.StyleView(_EAB, _trans);
							break;
						case eAllocationSelectionViewType.Size:
							frm = new MIDRetail.Windows.SizeView(_EAB, _trans);
							break;
						case eAllocationSelectionViewType.Summary:
						//Begin Assortment Planning - JScott - Assortment Planning Changes
							frm = new MIDRetail.Windows.SummaryView(_EAB, _trans);
							break;
							// NOTE -- Add this code in order to use Assortment View as Summary
							//if (_SAB.ClientServerSession.GlobalOptions.AppConfig.AssortmentInstalled)
							//{
							//    frm = new MIDRetail.Windows.AssortmentView(_trans, eAssortmentWindowType.AllocationSummary);
							//    ((AssortmentView)frm).Initialize();
							//}
							//else
							//{
							//    frm = new MIDRetail.Windows.SummaryView(_trans);
							//}
							//break;
						case eAllocationSelectionViewType.Assortment:
							frm = new MIDRetail.Windows.AssortmentView(_EAB, _trans, eAssortmentWindowType.Assortment);
							((AssortmentView)frm).Initialize();
                            //((AssortmentView)frm).GroupName = _groupName;
							break;
						//End Assortment Planning - JScott - Assortment Planning Changes
					}
					frm.MdiParent = parentForm;

                    // Begin VS2010 WindowState Fix - RMatelic - Maximized window state incorrect when window first opened >>> move WindowState to after Show()
                    //frm.WindowState = FormWindowState.Maximized;
					frm.Show();
                    frm.WindowState = FormWindowState.Maximized;
                    // End VS2010 WindowState Fix

					if (frm.ExceptionCaught)
					{
						frm.Close();
						frm.MdiParent = null;
					}
					// BEGIN MID Track #4077 - vertical splitters not correct when window first opens. 
					// only happens in Windows Server 2003 SP1, so this is a workaround
					else if (_trans.AllocationViewType == eAllocationSelectionViewType.Style ||
						_trans.AllocationViewType == eAllocationSelectionViewType.Velocity)
					{
						((MIDRetail.Windows.StyleView)frm).ResetSplitters();
					}
					// END MID Track #4077
                    // Begin TT#199-MD - RMatelic - Column headers not moving with the cells while using arrow keys
                    else if (_trans.AllocationViewType == eAllocationSelectionViewType.Size)
                    {
                        ((MIDRetail.Windows.SizeView)frm).ResetSplitters();
                    }
                    // End TT#199-MD
					Cursor.Current = Cursors.Default;
				}
				catch (Exception ex)
				{
					frm.Close();
					frm.MdiParent = null;
					HandleException(ex,frm.Name);
				}
			}
			catch (Exception ex)
			{
				HandleException (ex);
			}
			//finally
			//{
			//	this.Close();
			//	this.MdiParent = null;
			//}
		}
		/// <summary>
		/// Use to validate the fields that are specific to this method type
		/// </summary>
		private bool ValidateSpecificFields()
		{	
			DateRangeProfile begDRP;
			DateRangeProfile endDRP;
			string  errorMessage;
			bool FieldsValid = true;
			ErrorProvider.SetError (drsBegin,string.Empty);
			ErrorProvider.SetError (drsEnd,string.Empty);
			ErrorProvider.SetError (txtBasis,string.Empty);
			ErrorProvider.SetError (ugHeader,string.Empty);


			if (cbxAnalysis.Checked)
			{
				if (txtBasis.Text.Trim() == string.Empty) 
				{
					FieldsValid = false;
					errorMessage = _SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_FieldIsRequired);
					ErrorProvider.SetError (txtBasis,errorMessage);
				}
				if (drsEnd.DateRangeRID == Include.NoRID ||
					drsEnd.DateRangeRID == Include.UndefinedCalendarDateRange)
				{
					FieldsValid = false;
					errorMessage = _SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_FieldIsRequired);
					ErrorProvider.SetError (drsEnd,errorMessage);
				}


                if (rbSizeView.Checked && Convert.ToInt32(cboSizeCurve.SelectedValue, CultureInfo.CurrentUICulture) == Include.NoRID)
				{
					FieldsValid = false;
					errorMessage = _SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_FieldIsRequired);
					ErrorProvider.SetError (cboSizeCurve,errorMessage);
				}
			}

			if  (drsBegin.DateRangeRID != Include.NoRID &&
				drsBegin.DateRangeRID != Include.UndefinedCalendarDateRange)
			{
				if (drsEnd.DateRangeRID == Include.NoRID ||
					drsEnd.DateRangeRID == Include.UndefinedCalendarDateRange)
				{
					FieldsValid = false;
					errorMessage = _SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_FieldIsRequired);
					ErrorProvider.SetError (drsEnd,errorMessage);
				}
			}
			if  (drsEnd.DateRangeRID != Include.NoRID &&
				drsEnd.DateRangeRID != Include.UndefinedCalendarDateRange)
			{
				if (txtBasis.Text.Trim() == string.Empty) 
				{
					FieldsValid = false;
					errorMessage = _SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_FieldIsRequired);
					ErrorProvider.SetError (txtBasis,errorMessage);
				}
			}

			if (FieldsValid)
			{
				if  (drsBegin.DateRangeRID != Include.NoRID
					&& drsEnd.DateRangeRID != Include.NoRID)
				{
					begDRP = _SAB.ClientServerSession.Calendar.GetDateRange(drsBegin.DateRangeRID);
					endDRP = _SAB.ClientServerSession.Calendar.GetDateRange(drsEnd.DateRangeRID);
					if (begDRP.DateRangeType != endDRP.DateRangeType) 
					{
						FieldsValid = false;
						errorMessage = _SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_DatesMustBeSameType);
						ErrorProvider.SetError (drsBegin,errorMessage);
						ErrorProvider.SetError (drsEnd,errorMessage);
					}
					else if ( begDRP.StartDateKey > endDRP.StartDateKey)
					{
						FieldsValid = false;
						errorMessage = _SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_TimePeriodCannotBeGreater);
						ErrorProvider.SetError (drsBegin,errorMessage);
					}	
				}
			}
            
            // Begin TT#952 - RMatelic - MIDFormBase.cs error on Allocation View Selection-VerifyFailed
            if (!ValidBasisGrid())
            {
                FieldsValid = false;
            }
            // End TT#952

			if (_headerList.Count == 0 && !cbxAnalysis.Checked)
			{
				FieldsValid = false;
				errorMessage = _SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_FieldIsRequired);
				ErrorProvider.SetError (ugHeader,errorMessage);
			}	
			return  FieldsValid;			
		}

        // Begin TT#952 - RMatelic - MIDFormBase.cs error on Allocation View Selection-VerifyFailed
        private bool ValidBasisGrid()
        {
            bool validBasis = true;
            try
            {
                foreach (UltraGridRow gridRow in ugBasisNodeVersion.Rows)
                {
                    if (!ValidMerchandise(gridRow.Cells["Merchandise"]))
                    {
                        validBasis = false;
                    }
                    if (!ValidVersion(gridRow.Cells["BasisFVRID"]))
                    {
                        validBasis = false;
                    }
                    if (!ValidDateRange(gridRow.Cells["DateRange"]))
                    {
                        validBasis = false;
                    }
                    if (!ValidWeight(gridRow.Cells["Weight"]))
                    {
                        validBasis = false;
                    }
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return validBasis;  
        }

        private bool ValidMerchandise(UltraGridCell gridCell)
        {
            bool errorFound = false;
            string errorMessage = string.Empty;
            int rowseq = -1;
            try
            {
                string productID = gridCell.Value.ToString().Trim();
                if (productID == string.Empty)
                {
                    errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_FieldIsRequired);
                    errorFound = true;
                }
                else
                {
                    foreach (Infragistics.Win.ValueListItem vli in ugBasisNodeVersion.DisplayLayout.ValueLists["Merchandise"].ValueListItems)
                    {
                        if (Convert.ToInt32(vli.DataValue, CultureInfo.CurrentUICulture) == Convert.ToInt32(gridCell.Value, CultureInfo.CurrentUICulture))
                        {
                            rowseq = Convert.ToInt32(vli.DataValue, CultureInfo.CurrentUICulture);
                            break;
                        }
                    }
                    if (rowseq != -1)
                    {
                        DataRow row = _merchDataTable2.Rows[rowseq];
                        if (row != null)
                        {
                            return true;
                        }
                    }
                    HierarchyNodeProfile hnp = GetNodeProfile(productID);
                    if (hnp.Key == Include.NoRID)
                    {
                        errorMessage = string.Format(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_InvalidTreeNode),
                            productID);
                        errorFound = true;
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

        private bool ValidDateRange(UltraGridCell gridCell)
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
                else if (Convert.ToInt32(gridCell.Row.Cells["CdrRID"].Value, CultureInfo.CurrentUICulture) == Include.UndefinedCalendarDateRange)
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
                }
                gridCell.Tag = null;
                return true;
            }
        }

        private bool ValidWeight(UltraGridCell gridCell)
        {
            bool errorFound = false;
            string errorMessage = string.Empty;
            double dblValue;
            try
            {
                if (gridCell.Text.Length == 0)	// cell is empty - use default
                {
                    errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_FieldIsRequired);
                    errorFound = true;
                }
                else
                {
                    dblValue = Convert.ToDouble(gridCell.Text, CultureInfo.CurrentUICulture);
                    dblValue = Math.Round(dblValue, 3);
                   
                    gridCell.Value = dblValue.ToString(CultureInfo.CurrentUICulture);
                                      
                    if (dblValue < .001)
                    {
                        errorMessage = string.Format
                            (SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_al_MinimumValueExceeded), dblValue, ".001");
                        errorFound = true;
                    }
                    else if (dblValue > 9999)
                    {
                        errorMessage = string.Format
                            (SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_al_MaximumValueExceeded), dblValue, "9999");
                        errorFound = true;
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
        // End TT#952 

		private bool OKToProcess()
		{	
			// BEGIN MID Track #2547 - Error asking for Analysis Only and Size Review
			//if (cbxAnalysis.Checked)
			//{
			//	if (_trans.AllocationViewType == eAllocationSelectionViewType.Size)
			//	{
			//		MessageBox.Show(_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_SizeReviewInvalidForAnalysis),
			//			this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
			//		return false;
			//	}				
			//	else
			//		return true;
			//}
			// END MID Track #2547

			// BEGIN MID Track # 3017 Null Errors when node does not have grades defined.
			if (_trans.AllocationNeedAnalysisHNID != Include.NoRID)
			{
				StoreGradeList sgl = _trans.GetStoreGradeList(_trans.AllocationNeedAnalysisHNID);
				if (sgl.Count <= 0)
				{
					string message = _SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_NoGradeDefinition);
					MessageBox.Show
						(message,
						this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
					this.ErrorProvider.SetError(this.txtBasis, message);
    				return false;					
				}
			}
			// END MID Track # 3017
			// BEGIN MID Track #2959 - Remove above #2547 edit disallowing Size Review for Need Analysis
			if (cbxAnalysis.Checked)
				return true;
			// END MID Track #2959
			
			_allHeadersReadOnly = false;

			if (_trans.AllocationViewType == eAllocationSelectionViewType.Size)
				if (!SizeViewIsValid())
					return false;
		
			if (!ProceedAfterStatusCheck())
				return false;
			
			if (!_allHeadersReadOnly)
			{
				_continueProcess = true;
				//CheckSecurityEnqueue();
                ApplySecurity();
				if (!_continueProcess) 
					return false;
			}
			return true;
		}

		private bool SizeViewIsValid()
		{
			bool sizesExist = false;
			Header header;
			header = new Header(); 
			foreach (AllocationHeaderProfile ahp in _headerList)
			{	
				if (ahp.AllocationTypeFlags.WorkUpBulkSizeBuy)
					sizesExist = true;
				else if (header.BulkColorSizesExist(ahp.Key))
					sizesExist = true;
				// RonM - temporarily take out allowing pack sizes 
				//else if (header.PackSizesExist(ahp.Key)) 
				//	sizesExist = true;
				
				if (sizesExist)
					break;
			}
			if (!sizesExist)
				MessageBox.Show(_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_SizeReviewInvalid),
					this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
			
			return sizesExist;

		}	
		private bool ProceedAfterStatusCheck()
		{
			System.Windows.Forms.DialogResult diagResult;
			_readOnlyList = new Hashtable();
		
			string hdrAndStatus = string.Empty;
			foreach (AllocationHeaderProfile ahp in _headerList)
			{	
				if (   ahp.HeaderAllocationStatus == eHeaderAllocationStatus.ReceivedOutOfBalance
					|| ahp.HeaderAllocationStatus == eHeaderAllocationStatus.ReleaseApproved
					|| ahp.HeaderAllocationStatus == eHeaderAllocationStatus.Released
					|| ahp.HeaderAllocationStatus == eHeaderAllocationStatus.InUseByMultiHeader)
				{
					int status = (int)ahp.HeaderAllocationStatus;
					hdrAndStatus  = "HeaderID: " + ahp.HeaderID + "   Status: " + MIDText.GetTextOnly(status);
					_readOnlyList.Add(ahp.HeaderID,hdrAndStatus);
					_allHeadersReadOnly = true;
				}
                // Begin TT#1966-MD - JSmith - DC Fulfillment
                else if (ahp.IsMasterHeader && ahp.DCFulfillmentProcessed                    )
                {
                    int status = (int)ahp.HeaderAllocationStatus;
                    hdrAndStatus = "HeaderID: " + ahp.HeaderID + "   Status: " + MIDText.GetTextOnly(eMIDTextCode.msg_al_DCFulfillmentProcessed);
                    _readOnlyList.Add(ahp.HeaderID, hdrAndStatus);
                    _allHeadersReadOnly = true;
                }
                else if (ahp.IsSubordinateHeader && !ahp.DCFulfillmentProcessed)
                {
                    int status = (int)ahp.HeaderAllocationStatus;
                    hdrAndStatus = "HeaderID: " + ahp.HeaderID + "   Status: " + MIDText.GetTextOnly(eMIDTextCode.msg_al_DCFulfillmentNotProcessed);
                    _readOnlyList.Add(ahp.HeaderID, hdrAndStatus);
                    _allHeadersReadOnly = true;
                }
                // End TT#1966-MD - JSmith - DC Fulfillment
			}
			 
			if (_allHeadersReadOnly)
			{
				_trans.DataState = eDataState.ReadOnly;

				string errMsg = "The status of the following headers prohibits them "  
					+ "from being updated in the requested view."
					+ System.Environment.NewLine;
									
				foreach (string statusValue in _readOnlyList.Values)
				{
					errMsg += System.Environment.NewLine + statusValue;
				}
				errMsg += System.Environment.NewLine + System.Environment.NewLine;
				errMsg += "Do you wish to continue with the selected view as read-only?";

				diagResult = MessageBox.Show(
					errMsg,
					this.Text,
					//System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Asterisk);
					System.Windows.Forms.MessageBoxButtons.YesNoCancel, System.Windows.Forms.MessageBoxIcon.Asterisk);

				if (diagResult == System.Windows.Forms.DialogResult.No ||
					diagResult == System.Windows.Forms.DialogResult.Cancel)
				{
					return false;
				}
			}
			return true;
		}
		private void CheckSecurityEnqueue()
		{	
			try
			{
                // begin TT#1185 - Verify ENQ before Update
                if (!_trans.VelocityCriteriaExists)
                {
                    // end TT#1185 - Verify ENQ before Update
                    if ((rbSummaryView.Checked && _allocationReviewSummarySecurity.AllowUpdate) ||
                        (rbSizeView.Checked && _allocationReviewSizeSecurity.AllowUpdate) ||
                        (rbStyleView.Checked && _allocationReviewStyleSecurity.AllowUpdate))
                    {
                        try
                        {
                            // begin TT#1185 - Verify ENQ before Update
                            //BypassSecurityEnqueueCheck = false;
                            //if (BypassSecurityEnqueueCheck == false) //TT#311 - Determine whether to check the enqueue or not - apicchetti
                            //{
                            // end TT#1185 - Verify ENQ before Updtae
                            // BEGIN MID Track #2551 - data security not working
                            bool OKToEnqueue = true;
                            FunctionSecurityProfile nodeFunctionSecurity;
                            eSecurityFunctions securityFunction;

                            if (rbStyleView.Checked)
                                securityFunction = eSecurityFunctions.AllocationReviewStyle;
                            else if (rbSummaryView.Checked)
                                securityFunction = eSecurityFunctions.AllocationReviewSummary;
                            else
                                securityFunction = eSecurityFunctions.AllocationReviewSize;

                            List<int> selectedHdrRIDs = new List<int>(); // TT#1185 - Verify ENQ before Update
                            foreach (AllocationHeaderProfile ahp in _headerList)
                            {
                                nodeFunctionSecurity = _SAB.ClientServerSession.GetMyUserNodeFunctionSecurityAssignment(ahp.StyleHnRID, securityFunction, (int)eSecurityTypes.Allocation);
                                if (!nodeFunctionSecurity.AllowUpdate)
                                {
                                    OKToEnqueue = false;
                                    break;
                                }
                                selectedHdrRIDs.Add(ahp.Key);  // TT#1185 - Verify ENQ before Update
                            }

                            if (OKToEnqueue)
                            {
                                // begin TT#1185 - Verify ENQ before Update
                                //_trans.EnqueueHeaders();  
                                //if (_trans.HeadersEnqueued)
                                string enqMsg;
                                if (_trans.EnqueueHeaders(_trans.GetHeadersToEnqueue(selectedHdrRIDs), out enqMsg))
                                    _trans.DataState = eDataState.Updatable;
                                else
                                {
                                    // Begin TT#4515 - stodd - enqueue message
                                    //enqMsg =
                                    //   MIDText.GetTextOnly(eMIDTextCode.msg_al_HeaderEnqFailed)
                                    //   + System.Environment.NewLine
                                    //   + enqMsg;
                                    // End TT#4515 - stodd - enqueue message

                                    enqMsg += "Do you wish to continue with the selected view as read-only?";

                                    DialogResult diagResult = SAB.MessageCallback.HandleMessage(
                                        enqMsg,
                                        "Header Lock Conflict",
                                        MessageBoxButtons.OKCancel, System.Windows.Forms.MessageBoxIcon.Asterisk);

                                    if (diagResult == DialogResult.Cancel)
                                    {
                                        throw new CancelProcessException();
                                    }
                                    _trans.DataState = eDataState.ReadOnly;
                                }
                            }
                            else
                                _trans.DataState = eDataState.ReadOnly;
                            // END MID Track #2551 
                            // begin TT#1185 - Verify ENQ before Update
                            //}
                            //else
                            //{
                            //    BypassSecurityEnqueueCheck = false;
                            //}
                            // end TT#1185 - Verify ENQ before Update
                        }
                        catch (CancelProcessException)
                        {
                            _continueProcess = false;
                        }
                    }
                    else
                        _trans.DataState = eDataState.ReadOnly;
                }  // TT#1195 = Verofy ENQ before Updae
			}
			catch (Exception ex)
			{
				HandleException (ex);
			}
		}
		private void SaveSelectionToDatabase()
		{
            // Begin TT#456 - RMatelic - Add Views to Size Review  
            SaveUserGridViews();
            if (cboView.DataSource != null)
            {
                _trans.AllocationViewRID = Convert.ToInt32(cboView.SelectedValue, CultureInfo.CurrentUICulture);
            }
            else
            {
                _trans.AllocationViewRID = Include.NoRID;
            }
            // End TT#456

            // Begin TT#638 - RMatelic - Style Review - Add Basis Variables
            int seq = 0;
            _basisDataTable.DefaultView.Sort = "BasisSequence";

            foreach (DataRowView row in _basisDataTable.DefaultView)
            {
                row["BasisSequence"] = seq;
                seq++;
                // Begin TT#952 - RMatelic - MIDFormBase.cs error on Allocation View Selection
                if (row["BasisFVRID"] == DBNull.Value)
                {
                    row["BasisFVRID"] = Include.NoRID;
                }
                // End TT#952
            }
            _basisDataTable.AcceptChanges();
            // End TT#638 

            _trans.SaveAllocationDefaults();
		}

        // Begin TT#456 - RMatelic - Add Views to Size Review  
        private void SaveUserGridViews()
        {
            try
            {
                _userGridView.UserGridView_Update(SAB.ClientServerSession.UserRID, eLayoutID.styleReviewGrid, _styleViewRID);
                _userGridView.UserGridView_Update(SAB.ClientServerSession.UserRID, eLayoutID.sizeReviewGrid, _sizeViewRID);

                DataRow dRow = null;
                int viewRID = 0;
                if (rbStyleView.Checked)
                {
                    dRow = _dtStyleViews.Rows.Find(_styleViewRID);
                    viewRID = _styleViewRID;
                }
                else if (rbSizeView.Checked)
                {
                    dRow = _dtSizeViews.Rows.Find(_sizeViewRID);
                    viewRID = _sizeViewRID;
                }

                if (dRow != null && viewRID > 0)
                { 
                    int groupBySecondary = (dRow["GROUP_BY_SECONDARY"] == DBNull.Value ? Include.NoRID : Convert.ToInt32(dRow["GROUP_BY_SECONDARY"], CultureInfo.CurrentUICulture));
                    bool isSequential = (dRow["IS_SEQUENTIAL"] == DBNull.Value ? false : Include.ConvertCharToBool(Convert.ToChar(dRow["IS_SEQUENTIAL"], CultureInfo.CurrentUICulture)));

                    GridViewData gridViewData = new GridViewData();
                    gridViewData.OpenUpdateConnection();
                    try
                    {
                        gridViewData.GridView_Update(viewRID, false, _trans.AllocationGroupBy, groupBySecondary, isSequential, Include.NoRID, false);
                        gridViewData.CommitData();
                    }
                    catch (Exception exc)
                    {
                        gridViewData.Rollback();
                        string message = exc.Message;
                        throw;
                    }
                    finally
                    {
                        gridViewData.CloseUpdateConnection();
                    }
                }              
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }
        // End TT#456   

		private void btnCancel_Click(object sender, System.EventArgs e)
		{
			this.Close();
			this.MdiParent = null;
		}
		#region Header Grid


//		private string _lbl_Header = MIDText.GetTextOnly(eMIDTextCode.lbl_Header);
//		private string _lbl_Description = MIDText.GetTextOnly(eMIDTextCode.lbl_Description);
//		private string _lbl_RowPosition = "Row Position";
//		private string _lbl_RID = "RID";

		private void ugHeader_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
		{
			// check for saved layout
			InfragisticsLayoutData layoutData = new InfragisticsLayoutData();
			InfragisticsLayout layout = layoutData.InfragisticsLayout_Read(_SAB.ClientServerSession.UserRID, eLayoutID.allocationViewSelectionGrid);
			if (layout.LayoutLength > 0)
			{
				ugHeader.DisplayLayout.Load(layout.LayoutStream);
			}
			else
			{	// DEFAULT grid layout
                //Begin TT#169 - JSmith - enter max of 1000 and receive 'must be integer'
                //MIDRetail.Windows.Controls.UltraGridLayoutDefaults ugld = new MIDRetail.Windows.Controls.UltraGridLayoutDefaults();
                //ugld.ApplyDefaults(e);
                MIDRetail.Windows.Controls.UltraGridLayoutDefaults ugld = new MIDRetail.Windows.Controls.UltraGridLayoutDefaults(ErrorImage);
                // Begin TT#1164 - JSmith - When going in to some methods or Admin features the columns are appearing too wide.
                //ugld.ApplyDefaults((Infragistics.Win.UltraWinGrid.UltraGrid)sender, e, true, 2, true);
                ugld.ApplyDefaults((Infragistics.Win.UltraWinGrid.UltraGrid)sender, e, true, 2, false);
                // End TT#1164
                //End TT#169
				DefaultHeaderGridLayout();
			}
			this.ugHeader.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;
			this.ugHeader.DisplayLayout.Bands[0].Columns[_lbl_RowPosition].Hidden = true;

//			_headerGridBuilt = true;
		}

        private void ugHeader_SaveLayout()
        {
            // Begin TT#2012 - JSmith - Layout corrupted if close before opens completely
            //InfragisticsLayoutData layoutData = new InfragisticsLayoutData();
            //layoutData.InfragisticsLayout_Save(_SAB.ClientServerSession.UserRID, eLayoutID.allocationViewSelectionGrid, ugHeader);
            if (FormLoaded)
            {
                InfragisticsLayoutData layoutData = new InfragisticsLayoutData();
                layoutData.InfragisticsLayout_Save(_SAB.ClientServerSession.UserRID, eLayoutID.allocationViewSelectionGrid, ugHeader);
            }
            // End TT#2012
        }

		private void DefaultHeaderGridLayout()
		{
         	this.ugHeader.DisplayLayout.Appearance.ImageBackgroundStyle = Infragistics.Win.ImageBackgroundStyle.Stretched;
			this.ugHeader.DisplayLayout.Override.AllowColSizing = Infragistics.Win.UltraWinGrid.AllowColSizing.Default;
			this.ugHeader.DisplayLayout.Bands[0].Columns[_lbl_RowPosition].Hidden = true;
			this.ugHeader.DisplayLayout.Bands[0].Columns[_lbl_RID].Hidden = true;
		}

		private void SetUpHeaderGrid()
		{
			HeaderGrid_Define();
			HeaderGrid_Populate();
			this.ugHeader.DataSource = _headerDataTable;
			this.ugHeader.Text = "";
			this.ugHeader.DisplayLayout.Bands[0].HeaderVisible = false;

			this.ugHeader.DisplayLayout.AddNewBox.Hidden = false;
			this.ugHeader.DisplayLayout.GroupByBox.Hidden = true;
			this.ugHeader.DisplayLayout.GroupByBox.Prompt = "";
			this.ugHeader.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
			this.ugHeader.DisplayLayout.Bands[0].AddButtonCaption = _lbl_Header;

			this.ugHeader.DisplayLayout.Bands[0].Override.TipStyleRowConnector = TipStyle.Show;
			this.ugHeader.DisplayLayout.Bands[0].Override.TipStyleScroll = TipStyle.Show;
			this.ugHeader.DisplayLayout.Bands[0].Override.TipStyleCell = TipStyle.Show;

			this.ugHeader.DisplayLayout.Bands[0].Columns[_lbl_Description].AutoEdit = false;

			BuildHeaderGridContextMenu();
			this.ugHeader.ContextMenu = mnuHeaderGrid;

            //Begin TT#169 - JSmith - enter max of 1000 and receive 'must be integer'
            //FormatColumns(this.ugHeader);
            //End TT#169
			this.ugHeader.DisplayLayout.Bands[0].Columns["Header"].Width = 250;
			this.ugHeader.DisplayLayout.Bands[0].Columns["Description"].Width = 250;
//			_headerIsPopulated = true;
		}


		private void HeaderGrid_Define()
		{
			_headerDataTable = MIDEnvironment.CreateDataTable("HeaderGridDataTable");
			
			DataColumn dataColumn;

			//Create Columns and rows for datatable
			dataColumn = new DataColumn();
			dataColumn.DataType = System.Type.GetType("System.String");
			dataColumn.ColumnName = _lbl_Header;
			dataColumn.Caption = _lbl_Header;
			dataColumn.ReadOnly = false;
			dataColumn.Unique = true;
			_headerDataTable.Columns.Add(dataColumn);

			dataColumn = new DataColumn();
			dataColumn.DataType = System.Type.GetType("System.Int32");
			dataColumn.ColumnName = _lbl_RowPosition;
			dataColumn.Caption = _lbl_RowPosition;
			dataColumn.ReadOnly = false;
			dataColumn.Unique = false;
			dataColumn.AllowDBNull = true;
			_headerDataTable.Columns.Add(dataColumn);

			dataColumn = new DataColumn();
			dataColumn.DataType = System.Type.GetType("System.String");
			dataColumn.ColumnName = _lbl_Description;
			dataColumn.Caption = _lbl_Description;
			dataColumn.ReadOnly = false;
			dataColumn.Unique = false;
			dataColumn.AllowDBNull = true;
			_headerDataTable.Columns.Add(dataColumn);

			dataColumn = new DataColumn();
			dataColumn.DataType = System.Type.GetType("System.Int32");
			dataColumn.ColumnName = _lbl_RID;
			dataColumn.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_Sell_Thru_Pct);
			dataColumn.ReadOnly = false;
			dataColumn.Unique = false;
			dataColumn.AllowDBNull = true;
			_headerDataTable.Columns.Add(dataColumn);

			//make RID column the primary key
			DataColumn[] PrimaryKeyColumn = new DataColumn[1];
			PrimaryKeyColumn[0] = _headerDataTable.Columns[_lbl_Header];
			_headerDataTable.PrimaryKey = PrimaryKeyColumn;

		}


		private void BuildHeaderGridContextMenu()
		{
			MenuItem mnuItemInsert = new MenuItem(MIDText.GetTextOnly(eMIDTextCode.lbl_Insert));
			MenuItem mnuItemInsertBefore = new MenuItem(MIDText.GetTextOnly(eMIDTextCode.lbl_Insert_Before));
			MenuItem mnuItemInsertAfter = new MenuItem(MIDText.GetTextOnly(eMIDTextCode.lbl_Insert_after));
			MenuItem mnuItemCut= new MenuItem(MIDText.GetTextOnly(eMIDTextCode.lbl_Cut));
			MenuItem mnuItemCopy = new MenuItem(MIDText.GetTextOnly(eMIDTextCode.lbl_Copy));
			MenuItem mnuItemPaste = new MenuItem(MIDText.GetTextOnly(eMIDTextCode.lbl_Paste));
			MenuItem mnuItemDelete = new MenuItem(MIDText.GetTextOnly(eMIDTextCode.lbl_Delete));
			//			mnuHeaderGrid.MenuItems.Add(mnuItemCut);
			//			mnuHeaderGrid.MenuItems.Add(mnuItemCopy);
			//			mnuHeaderGrid.MenuItems.Add(mnuItemPaste);
			//			mnuHeaderGrid.MenuItems.Add("-");
			mnuHeaderGrid.MenuItems.Add(mnuItemInsert);
			mnuItemInsert.MenuItems.Add(mnuItemInsertBefore);
			mnuItemInsert.MenuItems.Add(mnuItemInsertAfter);
			mnuHeaderGrid.MenuItems.Add(mnuItemDelete);
			mnuItemInsert.Click += new System.EventHandler(this.mnuHeaderItemInsert_Click);
			mnuItemInsertBefore.Click += new System.EventHandler(this.mnuHeaderItemInsertBefore_Click);
			mnuItemInsertAfter.Click += new System.EventHandler(this.mnuHeaderItemInsertAfter_Click);
			mnuItemCut.Click += new System.EventHandler(this.mnuHeaderItemCut_Cut);
			mnuItemCopy.Click += new System.EventHandler(this.mnuHeaderItemCopy_Click);
			mnuItemPaste.Click += new System.EventHandler(this.mnuHeaderItemPaste_Click);
			mnuItemDelete.Click += new System.EventHandler(this.mnuHeaderItemDelete_Click);
		}

		private void mnuHeaderItemInsert_Click(object sender, System.EventArgs e)
		{
		}

		private void mnuHeaderItemInsertBefore_Click(object sender, System.EventArgs e)
		{
			_setHeaderRowPosition = false;
			Infragistics.Win.UltraWinGrid.UltraGridRow activeRow = this.ugHeader.ActiveRow;
			int rowPosition = Convert.ToInt32(this.ugHeader.ActiveRow.Cells[_lbl_RowPosition].Value, CultureInfo.CurrentUICulture);
			// increment the position of the active row to end of grid
			foreach(  UltraGridRow gridRow in ugHeader.Rows )
			{
				if (Convert.ToInt32(gridRow.Cells[_lbl_RowPosition].Value, CultureInfo.CurrentUICulture) >= rowPosition)
				{
					gridRow.Cells[_lbl_RowPosition].Value = Convert.ToInt32(gridRow.Cells[_lbl_RowPosition].Value, CultureInfo.CurrentUICulture) + 1;
				}
			}
			UltraGridRow addedRow = this.ugHeader.DisplayLayout.Bands[0].AddNew();
			addedRow.Cells[_lbl_RowPosition].Value = rowPosition;
			this.ugHeader.DisplayLayout.Bands[0].SortedColumns.Clear();
			this.ugHeader.DisplayLayout.Bands[0].SortedColumns.Add(_lbl_RowPosition, false);
			_setHeaderRowPosition = true;
		}

		private void mnuHeaderItemInsertAfter_Click(object sender, System.EventArgs e)
		{
			_setHeaderRowPosition = false;
			//			DebugWriter.WriteLine("enter mnuHeaderItemInsertAfter_Click");
			int rowPosition = Convert.ToInt32(this.ugHeader.ActiveRow.Cells[_lbl_RowPosition].Value, CultureInfo.CurrentUICulture);
			// increment the position of the active row to end of grid
			foreach(  UltraGridRow gridRow in ugHeader.Rows )
			{
				if (Convert.ToInt32(gridRow.Cells[_lbl_RowPosition].Value, CultureInfo.CurrentUICulture) > rowPosition)
				{
					gridRow.Cells[_lbl_RowPosition].Value = Convert.ToInt32(gridRow.Cells[_lbl_RowPosition].Value, CultureInfo.CurrentUICulture) + 1;
					//					DebugWriter.WriteLine(gridRow.Cells["Grade"].Value.ToString() + "=" + gridRow.Cells[_lbl_RowPosition].Value.ToString());
				}
			}
			//			DebugWriter.WriteLine("before AddNew mnuHeaderItemInsertAfter_Click");
			UltraGridRow addedRow = this.ugHeader.DisplayLayout.Bands[0].AddNew();
			//			DebugWriter.WriteLine("before set RowPosition mnuHeaderItemInsertAfter_Click");
			addedRow.Cells[_lbl_RowPosition].Value = rowPosition + 1;
			//			DebugWriter.WriteLine(addedRow.Cells["Grade"].Value.ToString() + "=" + addedRow.Cells[_lbl_RowPosition].Value.ToString());
			//			DebugWriter.WriteLine("before Clear sort mnuHeaderItemInsertAfter_Click");
			this.ugHeader.DisplayLayout.Bands[0].SortedColumns.Clear();
			//			DebugWriter.WriteLine("before add sort mnuHeaderItemInsertAfter_Click");
			this.ugHeader.DisplayLayout.Bands[0].SortedColumns.Add(_lbl_RowPosition, false);
			_setHeaderRowPosition = true;
			//			DebugWriter.WriteLine("leave mnuHeaderItemInsertAfter_Click");
		}

		private void mnuHeaderItemCut_Cut(object sender, System.EventArgs e)
		{
		}

		private void mnuHeaderItemCopy_Click(object sender, System.EventArgs e)
		{
		}

		private void mnuHeaderItemPaste_Click(object sender, System.EventArgs e)
		{
		}

		private void mnuHeaderItemDelete_Click(object sender, System.EventArgs e)
		{
			_profileToDelete = null;
			_deleteList = new AllocationHeaderProfileList(eProfileType.AllocationHeader);

			if (this.ugHeader.Selected.Rows.Count > 0)
				this.ugHeader.DeleteSelectedRows();
		}

		private void mnuHeaderItemExpandAll_Click(object sender, System.EventArgs e)
		{
			this.ugHeader.Rows.ExpandAll(true);
		}

		private void mnuHeaderItemCollapseAll_Click(object sender, System.EventArgs e)
		{
			this.ugHeader.Rows.CollapseAll(true);
		}

		private void ugHeader_AfterSortChange(object sender, Infragistics.Win.UltraWinGrid.BandEventArgs e)
		{
			//			DebugWriter.WriteLine("enter ugHeader_AfterSortChange");
			int count = 0;
			if (_setHeaderRowPosition)
			{
				//				DebugWriter.WriteLine("_setRowPosition ugHeader_AfterSortChange");
				foreach(  UltraGridRow gridRow in ugHeader.Rows )
				{
					gridRow.Cells[_lbl_RowPosition].Value = count;
					//					DebugWriter.WriteLine(gridRow.Cells["Grade"].Value.ToString() + "=" + gridRow.Cells[_lbl_RowPosition].Value.ToString());
					++count;
				}
			}
			//			DebugWriter.WriteLine("leave ugHeader_AfterSortChange");
		}

		private void HeaderGrid_Populate()
		{
			int count = 0;
			_headerDataTable.Rows.Clear();
			foreach(AllocationHeaderProfile ap in _headerList)
			{
				_headerDataTable.Rows.Add(new object[] { ap.HeaderID, count, ap.HeaderDescription, ap.Key});
				++count;
			}
		}

		private void ugHeader_DragDrop(object sender, System.Windows.Forms.DragEventArgs e)
		{
			// Create a new instance of the DataObject interface.
            //IDataObject data = Clipboard.GetDataObject();
			// If the data is ClipboardProfile, then retrieve the data.
            TreeNodeClipboardList cbList = null;
            if (e.Data.GetDataPresent(typeof(TreeNodeClipboardList)))
			{
                cbList = (TreeNodeClipboardList)e.Data.GetData(typeof(TreeNodeClipboardList));
                //if (cbp.ClipboardDataType == eClipboardDataType.HierarchyNode)
                if (cbList.ClipboardDataType == eProfileType.HierarchyNode)
				{
					HeaderGrid_Populate();
//					_headerChangesMade = true;
				}
				else
				{
					MessageBox.Show(_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MustBeNodeToDrop),
						this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}
			else
			{
				MessageBox.Show(_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MustBeNodeToDrop),
					this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}


		private void ugHeader_MouseEnterElement(object sender, Infragistics.Win.UIElementEventArgs e)
		{
			if(this.toolTip1 != null && this.toolTip1.Active) 
			{
				this.toolTip1.Active = false; //turn it off 
			}

			UltraGridCell gridCell = (UltraGridCell)e.Element.GetContext(typeof(Infragistics.Win.UltraWinGrid.UltraGridCell));
			if (gridCell != null)
			{
				if (gridCell.Tag != null && gridCell.Tag.GetType() == typeof(System.String))
				{
					toolTip1.Active = true; 
					toolTip1.SetToolTip(this.ugHeader, (string)gridCell.Tag);
				}
			}
		}

		private void ugHeader_BeforeExitEditMode(object sender, Infragistics.Win.UltraWinGrid.BeforeExitEditModeEventArgs e)
		{
			string headerName;
			string errorMessage = string.Empty; 
			AllocationHeaderProfile ap;
            
			if (e.CancellingEditOperation)
				return;
			 
			if (ugHeader.ActiveCell.Column.Key == _lbl_Header)
			{
				try
				{
					_errorFound = false;
					headerName = Convert.ToString(ugHeader.ActiveCell.Text, CultureInfo.CurrentUICulture).Trim();
					if (headerName.Trim() == string.Empty)
					{
						ugHeader.ActiveCell.Tag = string.Empty;
					}
					else
					{
						int key = Convert.ToInt32(ugHeader.ActiveCell.Row.Cells[_lbl_RID].Value, CultureInfo.CurrentUICulture);
						if (key != -1)
						{
							ap = (AllocationHeaderProfile)_headerList.FindKey(key);
							if (ap != null)
								_headerList.Remove(ap);
						}
						ap = new AllocationHeaderProfile(headerName,-1);
						if (ap.Key != -1)
						{
							if (_headerList.Count > 0)
							{
								foreach (AllocationHeaderProfile ahp in _headerList)
								{	
									if (ahp.HeaderID == headerName)
									{
										_errorFound = true;
										errorMessage = _SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_DuplicateHeaderIdNotAllowed);
										MessageBox.Show(errorMessage,this.Text,MessageBoxButtons.OK,MessageBoxIcon.Error);
										ugHeader.ActiveCell.Tag = "Duplicate header found!";
										e.Cancel = true;
									}
								}
							}
							if (!e.Cancel)
							{		
								ugHeader.ActiveCell.Row.Cells[_lbl_Description].Value = ap.HeaderDescription;
								ugHeader.ActiveCell.Row.Cells[_lbl_RID].Value = ap.Key;
								ugHeader.ActiveCell.Row.Cells[_lbl_RowPosition].Value = 127;
								ugHeader.ActiveCell.Tag = "";
								_headerList.Add(ap);
							}
						}
						else 
						{
							_errorFound = true;
							errorMessage = _SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_HeaderNotFound);
							MessageBox.Show(errorMessage,this.Text,MessageBoxButtons.OK,MessageBoxIcon.Error);
							ugHeader.ActiveCell.Tag = "Allocation Header Not Found!";
							e.Cancel = true;

						}
					}
				}
				catch (Exception ex)
				{
					HandleException (ex);
				}
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

		private void ugHeader_BeforeRowsDeleted(object sender, Infragistics.Win.UltraWinGrid.BeforeRowsDeletedEventArgs e)
		{
			foreach (Infragistics.Win.UltraWinGrid.UltraGridRow row in e.Rows)
			{
				if (Convert.ToInt32(row.Cells[_lbl_RID].Value, CultureInfo.CurrentUICulture) != -1)
				{
					_profileToDelete = (AllocationHeaderProfile)_headerList.FindKey(Convert.ToInt32(row.Cells[_lbl_RID].Value, CultureInfo.CurrentUICulture));
					_deleteList.Add(_profileToDelete);
				}
			}
		}
		private void ugHeader_AfterRowsDeleted(object sender, System.EventArgs e)
		{
			if (_deleteList != null)
			{
				foreach (AllocationHeaderProfile _profileToDelete in _deleteList)
				{
					_headerList.Remove(_profileToDelete);
				}	
			}
			
		}

		private void ugHeader_AfterRowInsert(object sender, Infragistics.Win.UltraWinGrid.RowEventArgs e)
		{
			e.Row.Cells[_lbl_RowPosition].Value = this.ugHeader.Rows.Count;
			e.Row.Cells[_lbl_RID].Value = -1;
		}
		#endregion

        // Begin TT#638 - RMatelic - Style Review - Add Basis Variables
        #region Basis Grid
        private void SetUpBasisGrid()
        {
            try
            {
                _basisDataTable = _trans.DTUserAllocBasis;
                if (!_basisDataTable.Columns.Contains("Merchandise"))
                {
                    _basisDataTable.Columns.Add("Merchandise");
                    _basisDataTable.Columns.Add("DateRange");
                    _basisDataTable.Columns.Add("Picture");
                    _basisDataTable.Columns["Weight"].DefaultValue = 1;
                    _basisDataTable.AcceptChanges();
                }

                CreateComboLists();

                this.ugBasisNodeVersion.DataSource = _basisDataTable;
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void CreateComboLists()
        {
            try
            {
                //Add a list to the grid, and name it "Merchandise".
                ugBasisNodeVersion.DisplayLayout.ValueLists.Add("Merchandise");
                BuildMerchandiseDataTable();
                HierarchyNodeProfile hnp;
                foreach (DataRow row in _basisDataTable.Rows)
                {
                    if ((int)row["BasisHNRID"] != Include.NoRID)
                    {
                        hnp = SAB.HierarchyServerSession.GetNodeData((int)row["BasisHNRID"], true, true);
                        AddNodeToMerchandiseCombo2(hnp);
                    }
                }
                foreach (DataRow row in _merchDataTable2.Rows)
                {
                    Infragistics.Win.ValueListItem vli = new Infragistics.Win.ValueListItem();
                    vli.DataValue = row["seqno"];
                    vli.DisplayText = Convert.ToString(row["text"], CultureInfo.CurrentUICulture);
                    vli.Tag = Convert.ToString(row["key"], CultureInfo.CurrentUICulture);
                    ugBasisNodeVersion.DisplayLayout.ValueLists["Merchandise"].ValueListItems.Add(vli);
                }

                //Add a list to the grid, and name it "Version".
                ugBasisNodeVersion.DisplayLayout.ValueLists.Add("Version");

                //Get the versions from the database.
                ForecastVersion fv = new ForecastVersion();
                DataTable VersionSource;
                VersionSource = fv.GetForecastVersions();

                //Loop through the datatable and manually add value and text to the lists.
                for (int i = 0; i < VersionSource.Rows.Count; i++)
                {
                    Infragistics.Win.ValueListItem vli = new Infragistics.Win.ValueListItem();

                    vli.DataValue = VersionSource.Rows[i]["FV_RID"];
                    vli.DisplayText = Convert.ToString(VersionSource.Rows[i]["DESCRIPTION"], CultureInfo.CurrentUICulture);
                    ugBasisNodeVersion.DisplayLayout.ValueLists["Version"].ValueListItems.Add(vli);
                    vli.Dispose();
                }

                //no longer need this temporary datatable.
                VersionSource.Dispose();
            }
            catch
            {
                throw;
            }
        }

        private void BuildMerchandiseDataTable()
        {
            try
            {
                _merchDataTable2 = MIDEnvironment.CreateDataTable();
                DataColumn myDataColumn;
                DataRow myDataRow;
                // Create new DataColumn, set DataType, ColumnName and add to DataTable.  
                // Level sequence number
                myDataColumn = new DataColumn();
                myDataColumn.DataType = System.Type.GetType("System.Int32");
                myDataColumn.ColumnName = "seqno";
                myDataColumn.ReadOnly = false;
                myDataColumn.Unique = true;
                _merchDataTable2.Columns.Add(myDataColumn);

                // Create second column - enum name.
                //Create Merchandise types - eMerchandiseType
                myDataColumn = new DataColumn();
                myDataColumn.DataType = System.Type.GetType("System.Int32");
                myDataColumn.ColumnName = "leveltypename";
                myDataColumn.AutoIncrement = false;
                myDataColumn.ReadOnly = false;
                myDataColumn.Unique = false;
                _merchDataTable2.Columns.Add(myDataColumn);

                // Create third column - text
                myDataColumn = new DataColumn();
                myDataColumn.DataType = System.Type.GetType("System.String");
                myDataColumn.ColumnName = "text";
                myDataColumn.AutoIncrement = false;
                myDataColumn.ReadOnly = false;
                myDataColumn.Unique = false;
                _merchDataTable2.Columns.Add(myDataColumn);

                // Create fourth column - Key
                myDataColumn = new DataColumn();
                myDataColumn.DataType = System.Type.GetType("System.Int32");
                myDataColumn.ColumnName = "key";
                myDataColumn.AutoIncrement = false;
                myDataColumn.ReadOnly = false;
                myDataColumn.Unique = false;
                _merchDataTable2.Columns.Add(myDataColumn);

                _hp = SAB.HierarchyServerSession.GetMainHierarchyData();

                //Default Selection to OTSPlanLevel
                myDataRow = _merchDataTable2.NewRow();
                myDataRow["seqno"] = 0;
                myDataRow["leveltypename"] = eMerchandiseType.OTSPlanLevel;
                myDataRow["text"] = MIDText.GetTextOnly(eMIDTextCode.lbl_OTSPlanLevel);
                myDataRow["key"] = Include.Undefined;
                _merchDataTable2.Rows.Add(myDataRow);

                for (int levelIndex = 1; levelIndex <= _hp.HierarchyLevels.Count; levelIndex++)
                {
                    HierarchyLevelProfile hlp = (HierarchyLevelProfile)_hp.HierarchyLevels[levelIndex];
                    if (hlp.LevelType != eHierarchyLevelType.Size)
                    {
                        myDataRow = _merchDataTable2.NewRow();
                        myDataRow["seqno"] = hlp.Level;
                        myDataRow["leveltypename"] = eMerchandiseType.HierarchyLevel;
                        myDataRow["text"] = hlp.LevelID;
                        myDataRow["key"] = hlp.Key;
                        _merchDataTable2.Rows.Add(myDataRow);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in BuildDataTable");
                HandleException(ex);
            }
        }

        private void AddNodeToMerchandiseCombo2(HierarchyNodeProfile hnp)
        {
            try
            {
                DataRow row;
                _basisNodeInList = false;
                int nodeRID = Include.NoRID;
                for (int levIndex = 0;
                    levIndex < _merchDataTable2.Rows.Count; levIndex++)
                {
                    row = _merchDataTable2.Rows[levIndex];
                    if ((eMerchandiseType)(Convert.ToInt32(row["leveltypename"], CultureInfo.CurrentUICulture)) == eMerchandiseType.Node)
                    {
                        nodeRID = (Convert.ToInt32(row["key"], CultureInfo.CurrentUICulture));
                        if (hnp.Key == nodeRID)
                        {
                            _basisNodeInList = true;
                            break;
                        }
                    }
                }
                if (!_basisNodeInList)
                {
                    row = _merchDataTable2.NewRow();
                    row["seqno"] = _merchDataTable2.Rows.Count;
                    row["leveltypename"] = eMerchandiseType.Node;
                    row["text"] = hnp.Text;
                    row["key"] = hnp.Key;
                    _merchDataTable2.Rows.Add(row);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void SetGridComboToLevel(DataRow aRow, int seq)
        {
            try
            {
                DataRow myDataRow;
                for (int levIndex = 0;
                    levIndex < _merchDataTable2.Rows.Count; levIndex++)
                {
                    myDataRow = _merchDataTable2.Rows[levIndex];
                    if (Convert.ToInt32(myDataRow["seqno"], CultureInfo.CurrentUICulture) == seq)
                    {
                        aRow["Merchandise"] = myDataRow["seqno"];
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

        }

        private void ugBasisNodeVersion_InitializeLayout(object sender, InitializeLayoutEventArgs e)
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

                this.ugBasisNodeVersion.DisplayLayout.Bands[0].HeaderVisible = false;
                this.ugBasisNodeVersion.DisplayLayout.AddNewBox.Hidden = true;
                this.ugBasisNodeVersion.DisplayLayout.GroupByBox.Hidden = true;
                this.ugBasisNodeVersion.DisplayLayout.GroupByBox.Prompt = "";
                this.ugBasisNodeVersion.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;

                this.ugBasisNodeVersion.DisplayLayout.Bands[0].ColHeadersVisible = true;
                //Prevent the user from re-arranging columns.
                this.ugBasisNodeVersion.DisplayLayout.Override.AllowColMoving = AllowColMoving.NotAllowed;

                //hide the db columns.
                this.ugBasisNodeVersion.DisplayLayout.Bands[0].Columns["BasisSequence"].Hidden = true;
                this.ugBasisNodeVersion.DisplayLayout.Bands[0].Columns["BasisHNRID"].Hidden = true;
                this.ugBasisNodeVersion.DisplayLayout.Bands[0].Columns["BasisPHRID"].Hidden = true;
                this.ugBasisNodeVersion.DisplayLayout.Bands[0].Columns["BasisPHLSequence"].Hidden = true;

                //Set the header captions.
                this.ugBasisNodeVersion.DisplayLayout.Bands[0].Columns["Merchandise"].Header.VisiblePosition = 1;
                this.ugBasisNodeVersion.DisplayLayout.Bands[0].Columns["Merchandise"].Header.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_Merchandise);
                this.ugBasisNodeVersion.DisplayLayout.Bands[0].Columns["Merchandise"].Width = 200;
                this.ugBasisNodeVersion.DisplayLayout.Bands[0].Columns["BasisFVRID"].Header.VisiblePosition = 2;
                this.ugBasisNodeVersion.DisplayLayout.Bands[0].Columns["BasisFVRID"].Width = 120;
                this.ugBasisNodeVersion.DisplayLayout.Bands[0].Columns["BasisFVRID"].Header.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_Version);

                //make the "Merchandise" & "Version" columns drop down lists.
                this.ugBasisNodeVersion.DisplayLayout.Bands[0].Columns["Merchandise"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDown;
                this.ugBasisNodeVersion.DisplayLayout.Bands[0].Columns["Merchandise"].ValueList = ugBasisNodeVersion.DisplayLayout.ValueLists["Merchandise"];
                this.ugBasisNodeVersion.DisplayLayout.Bands[0].Columns["BasisFVRID"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                this.ugBasisNodeVersion.DisplayLayout.Bands[0].Columns["BasisFVRID"].ValueList = ugBasisNodeVersion.DisplayLayout.ValueLists["Version"];

                //Begin TT#169 - JSmith - enter max of 1000 and receive 'must be integer'
                //FormatColumns(this.ugBasisNodeVersion);
                //End TT#169
                this.ugBasisNodeVersion.DisplayLayout.Bands[0].Columns["BasisFVRID"].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Left;

                if (!_allocationReviewSecurity.AllowUpdate)
                {
                    foreach (UltraGridBand ugb in this.ugBasisNodeVersion.DisplayLayout.Bands)
                    {
                        ugb.Override.AllowDelete = DefaultableBoolean.False;
                    }
                }

                foreach (DataRow row in _basisDataTable.Rows)
                {
                    if ((int)row["BasisHNRID"] != Include.NoRID)
                    {
                        for (int i = 0; i < _merchDataTable2.Rows.Count; i++)
                        {
                            DataRow listRow = _merchDataTable2.Rows[i];
                            if ((int)listRow["key"] == (int)row["BasisHNRID"])
                            {
                                SetGridComboToLevel(row, (int)listRow["seqno"]);
                            }
                        }
                    }
                    else if ((int)row["BasisPHRID"] != Include.NoRID)
                    {
                        SetGridComboToLevel(row, (int)row["BasisPHLSequence"]);
                    }
                    else
                        SetGridComboToLevel(row, 0);
                }

                this.ugBasisNodeVersion.DisplayLayout.Bands[0].Columns["CdrRID"].Hidden = true;
                this.ugBasisNodeVersion.DisplayLayout.Bands[0].Columns["DateRange"].Header.VisiblePosition = 3;
                this.ugBasisNodeVersion.DisplayLayout.Bands[0].Columns["DateRange"].Header.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_Date_Range);
                this.ugBasisNodeVersion.DisplayLayout.Bands[0].Columns["DateRange"].Width = 180;
                this.ugBasisNodeVersion.DisplayLayout.Bands[0].Columns["DateRange"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.EditButton;
                this.ugBasisNodeVersion.DisplayLayout.Bands[0].Columns["DateRange"].CellActivation = Activation.ActivateOnly;

                this.ugBasisNodeVersion.DisplayLayout.Bands[0].Columns["Picture"].Hidden = true;

                this.ugBasisNodeVersion.DisplayLayout.Bands[0].Columns["Weight"].Header.VisiblePosition = 4;

                this.ugBasisNodeVersion.DisplayLayout.Bands[0].Columns["Weight"].Header.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_Weight);
                this.ugBasisNodeVersion.DisplayLayout.Bands[0].Columns["Weight"].Width = 50;

                foreach (UltraGridRow row in ugBasisNodeVersion.Rows)
                {
                    DateRangeProfile dr = SAB.ClientServerSession.Calendar.GetDateRange(Convert.ToInt32(row.Cells["cdrRID"].Value, CultureInfo.CurrentUICulture));
                    row.Cells["DateRange"].Value = dr.DisplayDate;
                    if (dr.DateRangeType == eCalendarRangeType.Dynamic)
                    {
                        switch (dr.RelativeTo)
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
                    // Begin TT#952 - RMatelic - MIDFormBase.cs error on Allocation View Selection
                    if (Convert.ToInt32(row.Cells["BasisFVRID"].Value,CultureInfo.CurrentUICulture) == Include.NoRID)
                    {
                        row.Cells["BasisFVRID"].Value = DBNull.Value;
                    }
                    // End TT#952 
                } 
            }
            catch
            {
                throw;
            }            
        }


        private void ugBasisNodeVersion_ClickCellButton(object sender, Infragistics.Win.UltraWinGrid.CellEventArgs e)
        {
            try
            {
                if (_allocationReviewSecurity.AllowUpdate)
                {
                    CalendarDateSelector frmCalDtSelector = (CalendarDateSelector)CreateControl(typeof(CalendarDateSelector), new object[] { SAB });
                    frmCalDtSelector.AllowReoccurring = false;
                    frmCalDtSelector.AllowDynamic = true;
                    frmCalDtSelector.AllowDynamicToStoreOpen = false;
                    frmCalDtSelector.AllowDynamicToPlan = false;

                    if (e.Cell.Row.Cells["DateRange"].Value != null &&
                        e.Cell.Row.Cells["DateRange"].Value != System.DBNull.Value &&
                        e.Cell.Row.Cells["DateRange"].Text.Length > 0)
                    {
                        frmCalDtSelector.DateRangeRID = Convert.ToInt32(e.Cell.Row.Cells["cdrRid"].Value, CultureInfo.CurrentUICulture);
                    }


                    frmCalDtSelector.AnchorDate = SAB.ClientServerSession.Calendar.CurrentDate;

                    DialogResult DateRangeResult = frmCalDtSelector.ShowDialog();
                    if (DateRangeResult == DialogResult.OK)
                    {

                        DateRangeProfile SelectedDateRange = (DateRangeProfile)frmCalDtSelector.Tag;
                        // Begin TT#952 - RMatelic - MIDFormBase.cs error on Allocation View Selection >> move CdrRid code before setting SelectedDateRange.DisplayDate
                        if (e.Cell.Row.Cells["CdrRid"].Value != System.DBNull.Value)
                        {
                            e.Cell.Row.Cells["CdrRid"].Value = System.DBNull.Value;
                        }
                        e.Cell.Row.Cells["CdrRid"].Value = SelectedDateRange.Key;
                        // End TT#952
                        e.Cell.Value = SelectedDateRange.DisplayDate;
                        //if (!_changedByCode)  // Issue 4393 stodd 10.17.2007
                        {
                            //				e.Cell.Tag = SelectedDateRange;
                            // for some reason have to clear the cell before it can be updated??
                            // Begin TT#952 - RMatelic - MIDFormBase.cs error on Allocation View Selection >> move CdrRid code before setting SelectedDateRange.DisplayDate
                            //if (e.Cell.Row.Cells["CdrRid"].Value != System.DBNull.Value)
                            //{
                            //    e.Cell.Row.Cells["CdrRid"].Value = System.DBNull.Value;
                            //}
                            //e.Cell.Row.Cells["CdrRid"].Value = SelectedDateRange.Key;
                            // End TT#952 
                            if (SelectedDateRange.DateRangeType == eCalendarRangeType.Dynamic)
                            {
                                if (SelectedDateRange.RelativeTo == eDateRangeRelativeTo.Plan)
                                    e.Cell.Appearance.Image = DynamicToPlanImage;
                                else
                                    e.Cell.Appearance.Image = DynamicToCurrentImage;
                            }
                            else
                            {
                                e.Cell.Appearance.Image = null;
                            }
                        }
                        //else
                        //{
                        //    ChangePending = true;
                        //    btnSave.Enabled = true;
                        //}
                        //_changedByCode = false;

                    }
                }
            }
            catch (Exception exception)
            {
                HandleException(exception);
            }
        }

        private void ugBasisNodeVersion_AfterCellUpdate(object sender, Infragistics.Win.UltraWinGrid.CellEventArgs e)
        {
            bool errorFound = false;
            int rowseq = -1;
            string errorMessage = string.Empty, productID;
            try
            {
                switch (e.Cell.Column.Key)
                {
                    case "Merchandise":

                        if (_skipAfterCellUpdate) return;
                        if (e.Cell.Value.ToString().Trim().Length == 0)
                            return;

                        try
                        {
                            foreach (Infragistics.Win.ValueListItem vli in ugBasisNodeVersion.DisplayLayout.ValueLists["Merchandise"].ValueListItems)
                            {
                                if (Convert.ToInt32(vli.DataValue, CultureInfo.CurrentUICulture) == Convert.ToInt32(e.Cell.Value, CultureInfo.CurrentUICulture))
                                {
                                    rowseq = Convert.ToInt32(vli.DataValue, CultureInfo.CurrentUICulture);
                                    break;
                                }
                            }
                        }
                        // catch if value is not integer so that it can be check to determine if it is a product
                        catch (System.FormatException)
                        {
                            rowseq = -1;
                        }

                        if (rowseq != -1)
                        {
                            DataRow row = _merchDataTable2.Rows[rowseq];
                            if (row != null)
                            {
                                eMerchandiseType MerchandiseType = (eMerchandiseType)(Convert.ToInt32(row["leveltypename"], CultureInfo.CurrentUICulture));

                                switch (MerchandiseType)
                                {
                                    case eMerchandiseType.Node:
                                        e.Cell.Row.Cells["BasisHNRID"].Value = Convert.ToInt32(row["key"], CultureInfo.CurrentUICulture);
                                        e.Cell.Row.Cells["BasisPHRID"].Value = Include.NoRID;
                                        e.Cell.Row.Cells["BasisPHLSequence"].Value = 0;
                                        break;
                                    case eMerchandiseType.HierarchyLevel:
                                        e.Cell.Row.Cells["BasisPHRID"].Value = _hp.Key;
                                        e.Cell.Row.Cells["BasisPHLSequence"].Value = Convert.ToInt32(row["key"], CultureInfo.CurrentUICulture);
                                        e.Cell.Row.Cells["BasisHNRID"].Value = Include.NoRID;
                                        break;
                                    case eMerchandiseType.OTSPlanLevel:
                                        e.Cell.Row.Cells["BasisPHRID"].Value = Include.NoRID;
                                        e.Cell.Row.Cells["BasisHNRID"].Value = Include.NoRID;
                                        e.Cell.Row.Cells["BasisPHLSequence"].Value = 0;
                                        break;
                                }
                            }
                        }
                        else
                        {
                            object cellValue = null;
                            productID = e.Cell.Value.ToString().Trim();
                            HierarchyNodeProfile hnp = GetNodeProfile(productID);
                            if (hnp.Key == Include.NoRID)
                            {
                                errorMessage = string.Format(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_InvalidTreeNode),
                                    productID);
                                errorFound = true;
                            }
                            else
                            {
                                e.Cell.Row.Cells["BasisHNRID"].Value = hnp.Key;
                                e.Cell.Row.Cells["BasisPHRID"].Value = Include.NoRID;
                                e.Cell.Row.Cells["BasisPHLSequence"].Value = 0;
                                AddNodeToMerchandiseCombo2(hnp);
                                if (!_basisNodeInList)
                                {
                                    Infragistics.Win.ValueListItem vli = new Infragistics.Win.ValueListItem();
                                    vli.DataValue = ugBasisNodeVersion.DisplayLayout.ValueLists["Merchandise"].ValueListItems.Count;
                                    vli.DisplayText = hnp.Text;
                                    vli.Tag = Convert.ToString(hnp.Key, CultureInfo.CurrentUICulture);
                                    ugBasisNodeVersion.DisplayLayout.ValueLists["Merchandise"].ValueListItems.Add(vli);
                                    // Begin TT#2093 - JSmith - Need Analysis Add basis and after window opens add columns and receive an error.
                                    if (_analysisValueList != null)
                                    {
                                        object DataValue = vli.DataValue;
                                        vli = new Infragistics.Win.ValueListItem();
                                        vli.DataValue = DataValue;
                                        vli.DisplayText = hnp.Text;
                                        vli.Tag = Convert.ToString(hnp.Key, CultureInfo.CurrentUICulture);
                                        _analysisValueList.ValueListItems.Add(vli);
                                    }
                                    // End TT#2093 - JSmith - Need Analysis Add basis and after window opens add columns and receive an error.
                                    cellValue = vli.DataValue;
                                }
                                else
                                {
                                    foreach (Infragistics.Win.ValueListItem vli in ugBasisNodeVersion.DisplayLayout.ValueLists["Merchandise"].ValueListItems)
                                    {
                                        if (vli.DisplayText == hnp.Text)
                                        {
                                            cellValue = vli.DataValue;
                                            break;
                                        }
                                    }
                                }
                                _skipAfterCellUpdate = true;
                                e.Cell.Value = cellValue;
                                _skipAfterCellUpdate = false;
                                ugBasisNodeVersion.UpdateData();
                            }
                        }
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
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private HierarchyNodeProfile GetNodeProfile(string aProductID)
        {
            string desc = string.Empty;
            try
            {
                string productID = aProductID;
                string[] pArray = productID.Split(new char[] { '[' });
                productID = pArray[0].Trim();
              
                HierarchyMaintenance hm = new HierarchyMaintenance(SAB);
                EditMsgs em = new EditMsgs();
                return hm.NodeLookup(ref em, productID, false);
            }
            catch (Exception ex)
            {
                HandleException(ex);
                return new HierarchyNodeProfile(Include.NoRID);
            }
        }

        private void ugBasisNodeVersion_DragEnter(object sender, DragEventArgs e)
        {
            Image_DragEnter(sender, e);
        }

        private void ugBasisNodeVersion_DragOver(object sender, System.Windows.Forms.DragEventArgs e)
        {
            try
            {
                Image_DragOver(sender, e);
                Infragistics.Win.UIElement aUIElement;
                aUIElement = ugBasisNodeVersion.DisplayLayout.UIElement.ElementFromPoint(ugBasisNodeVersion.PointToClient(new Point(e.X, e.Y)));

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

                if (aCell == aRow.Cells["Merchandise"])
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

        private void ugBasisNodeVersion_DragDrop(object sender, System.Windows.Forms.DragEventArgs e)
        {
            HierarchyNodeProfile hnp; 
            TreeNodeClipboardList cbList = null;
            try
            {
                Infragistics.Win.UIElement aUIElement;

                aUIElement = ugBasisNodeVersion.DisplayLayout.UIElement.ElementFromPoint(ugBasisNodeVersion.PointToClient(new Point(e.X, e.Y)));

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
                        object cellValue = null;
                        if (e.Data.GetDataPresent(typeof(TreeNodeClipboardList)))
                        {
                            cbList = (TreeNodeClipboardList)e.Data.GetData(typeof(TreeNodeClipboardList));
                            if (cbList.ClipboardDataType == eProfileType.HierarchyNode)
                            {
                                hnp = SAB.HierarchyServerSession.GetNodeData(cbList.ClipboardProfile.Key, true, true);

                                aRow.Cells["BasisHNRID"].Value = hnp.Key;
                                aRow.Cells["Merchandise"].Appearance.Image = null;
                                aRow.Cells["Merchandise"].Tag = null;
                                // Issue stodd 10.4.2007
                                // Used to tell if user nixed changing the merchandise node. 
                                //if (_changedByCode)
                                //{
                                //    _changedByCode = false;
                                //}
                                //else
                                {
                                    aRow.Cells["BasisPHRID"].Value = Include.NoRID;
                                    aRow.Cells["BasisPHLSequence"].Value = 0;
                                    AddNodeToMerchandiseCombo2(hnp);
                                    if (!_basisNodeInList)
                                    {
                                        Infragistics.Win.ValueListItem vli = new Infragistics.Win.ValueListItem();
                                        vli.DataValue = ugBasisNodeVersion.DisplayLayout.ValueLists["Merchandise"].ValueListItems.Count;
                                        vli.DisplayText = hnp.Text;
                                        vli.Tag = Convert.ToString(hnp.Key, CultureInfo.CurrentUICulture);
                                        ugBasisNodeVersion.DisplayLayout.ValueLists["Merchandise"].ValueListItems.Add(vli);
                                        // Begin TT#2093 - JSmith - Need Analysis Add basis and after window opens add columns and receive an error.
                                        if (_analysisValueList != null)
                                        {
                                            object DataValue = vli.DataValue;
                                            vli = new Infragistics.Win.ValueListItem();
                                            vli.DataValue = DataValue;
                                            vli.DisplayText = hnp.Text;
                                            vli.Tag = Convert.ToString(hnp.Key, CultureInfo.CurrentUICulture);
                                            _analysisValueList.ValueListItems.Add(vli);
                                        }
                                        // End TT#2093 - JSmith - Need Analysis Add basis and after window opens add columns and receive an error.
                                        cellValue = vli.DataValue;
                                    }
                                    else
                                    {
                                        foreach (Infragistics.Win.ValueListItem vli in ugBasisNodeVersion.DisplayLayout.ValueLists["Merchandise"].ValueListItems)
                                        {
                                            if (vli.DisplayText == hnp.Text)
                                            {
                                                cellValue = vli.DataValue;
                                                break;
                                            }
                                        }
                                    }
                                    _skipAfterCellUpdate = true;
                                    aCell.Value = cellValue;
                                    _skipAfterCellUpdate = false;
                                    //_changedByCode = false;
                                    ugBasisNodeVersion.UpdateData();
                                }
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

        // Begin TT#952 - RMatelic - MIDFormBase.cs error on Allocation View Selection
       private void ugBasisNodeVersion_InitializeRow(object sender, InitializeRowEventArgs e)
        {
            try
            {
                if (e.Row.Cells["CdrRID"].Value != DBNull.Value &&
                   Convert.ToInt32(e.Row.Cells["CdrRID"].Value, CultureInfo.CurrentUICulture) == Include.UndefinedCalendarDateRange)
                {
                    DateRangeProfile dr = SAB.ClientServerSession.Calendar.GetDateRange(Include.UndefinedCalendarDateRange);
                    e.Row.Cells["DateRange"].Value = dr.DisplayDate;
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }


       private void ugBasisNodeVersion_MouseEnterElement(object sender, UIElementEventArgs e)
       {
           try
           {
               ShowUltraGridToolTip(ugBasisNodeVersion, e);
           }
           catch (Exception ex)
           {
               HandleException(ex);
           }
       }
        // End TT#952 

       // Begin TT#2093 - JSmith - Need Analysis Add basis and after window opens add columns and receive an error.
       private Infragistics.Win.ValueList BuildAnalysisValuelist()
       {
           if (_analysisValueList == null)
           {
               _analysisValueList = new Infragistics.Win.ValueList();
               _analysisValueList.SortStyle = ValueListSortStyle.Ascending;

               int levelTypeName;
               foreach (DataRow row in _merchDataTable2.Rows)
               {
                   levelTypeName = (int)row["leveltypename"];
                   if (levelTypeName == (int)eMerchandiseType.Node)
                   {
                       Infragistics.Win.ValueListItem vli = new Infragistics.Win.ValueListItem();
                       vli.DataValue = row["seqno"];
                       vli.DisplayText = Convert.ToString(row["text"], CultureInfo.CurrentUICulture);
                       vli.Tag = Convert.ToString(row["key"], CultureInfo.CurrentUICulture);
                       _analysisValueList.ValueListItems.Add(vli);
                   }
               }
           }

           return _analysisValueList;
       }
       // End TT#2093 - JSmith - Need Analysis Add basis and after window opens add columns and receive an error.
        #endregion
        
        #region Basis Grid Context Menu
        private void cmsBasis_Opening(object sender, CancelEventArgs e)
        {
            try
            {
                if (!_allocationReviewSecurity.AllowUpdate)
                {
                    return;
                }

                if (this.ugBasisNodeVersion.Rows.Count > 0)
                {
                    cmsBasisItemDelete.Visible = true;
                    cmsBasisItemDeleteAll.Visible = true;
                }
                else
                {
                    cmsBasisItemDelete.Visible = false;
                    cmsBasisItemDeleteAll.Visible = false;
                }
                
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }


        private void cmsBasisItemInsertBefore_Click(object sender, EventArgs e)
        {
            InsertBasisNode(true);
            // Begin TT#2093 - JSmith - Need Analysis Add basis and after window opens add columns and receive an error.
            ugBasisDropdownUpdate();
            // End TT#2093 - JSmith - Need Analysis Add basis and after window opens add columns and receive an error.
        }


        private void cmsBasisItemInsertAfter_Click(object sender, EventArgs e)
        {
            InsertBasisNode(false);
            // Begin TT#2093 - JSmith - Need Analysis Add basis and after window opens add columns and receive an error.
            ugBasisDropdownUpdate();
            // End TT#2093 - JSmith - Need Analysis Add basis and after window opens add columns and receive an error.
        }

        private void cmsBasisItemDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (ugBasisNodeVersion.Selected.Rows.Count > 0)
                {
                    ugBasisNodeVersion.DeleteSelectedRows();
                    _basisDataTable.AcceptChanges();
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void cmsBasisItemDeleteAll_Click(object sender, EventArgs e)
        {
            if (ugBasisNodeVersion.Rows.Count > 0)
            {
                _basisDataTable.Rows.Clear();
                _basisDataTable.AcceptChanges();
            }
        }

        private void InsertBasisNode(bool InsertBeforeRow)
        {
            //_setBasisRowPosition = false;
            int rowPosition = 0;
            try
            {
                if (this.ugBasisNodeVersion.Rows.Count > 0)
                {
                    if (this.ugBasisNodeVersion.ActiveRow == null) return;
                    rowPosition = Convert.ToInt32(this.ugBasisNodeVersion.ActiveRow.Cells["BasisSequence"].Value, CultureInfo.CurrentUICulture);
                    int seq;
                    foreach (DataRow row in _basisDataTable.Rows)
                    {
                        seq = (int)row["BasisSequence"];
                        if (InsertBeforeRow)
                        {
                            if (seq >= rowPosition)
                                row["BasisSequence"] = seq + 1;
                        }
                        else
                        {
                            if (seq > rowPosition)
                                row["BasisSequence"] = seq + 1;
                        }
                    }
                }
                DataRow addedRow = _basisDataTable.NewRow();
                addedRow["BasisSequence"] = rowPosition;
                addedRow["BasisHNRID"] = Include.NoRID;
                addedRow["BasisPHRID"] = Include.NoRID;
                addedRow["BasisPHLSequence"] = 0;
                // Begin TT#952 - RMatelic - MIDFormBase.cs error on Allocation View Selection
                //addedRow["CdrRID"] = Include.NoRID;
                addedRow["CdrRID"] = Include.UndefinedCalendarDateRange;
                // End TT#952  
                addedRow["Weight"] = 1;
      
                // Copy OTS Plan Merchandise to 1st inserted row only 
                if (this.ugBasisNodeVersion.Rows.Count == 0)
                {
                    int rowseq = 0;
            
                    SetGridComboToLevel(addedRow, rowseq);
                    DataRow row = _merchDataTable2.Rows[rowseq];
                    if (row != null)
                    {
                        eMerchandiseType MerchandiseType = (eMerchandiseType)(Convert.ToInt32(row["leveltypename"], CultureInfo.CurrentUICulture));

                        switch (MerchandiseType)
                        {
                            case eMerchandiseType.Node:
                                addedRow["BasisHNRID"] = Convert.ToInt32(row["key"], CultureInfo.CurrentUICulture);
                                addedRow["BasisPHRID"] = Include.NoRID;
                                addedRow["BasisPHLSequence"] = 0;
                                break;
                            case eMerchandiseType.HierarchyLevel:
                                addedRow["BasisPHRID"] = _hp.Key;
                                addedRow["BasisPHLSequence"] = Convert.ToInt32(row["key"], CultureInfo.CurrentUICulture);
                                addedRow["BasisHNRID"] = Include.NoRID;
                                break;
                            case eMerchandiseType.OTSPlanLevel:
                                addedRow["BasisHNRID"] = Include.NoRID;
                                addedRow["BasisPHRID"] = Include.NoRID;
                                addedRow["BasisPHLSequence"] = 0;
                                break;
                        }
                    }
                }
                _basisDataTable.Rows.Add(addedRow);
                this.ugBasisNodeVersion.DisplayLayout.Bands[0].SortedColumns.Clear();
                this.ugBasisNodeVersion.DisplayLayout.Bands[0].SortedColumns.Add("BasisSequence", false);
                //_setBasisRowPosition = true;
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }	
        #endregion
        // End TT#638 

        #region Date pickers
        private void drsBegin_Load(object sender, System.EventArgs e)
		{
			CalendarDateSelector frm = (CalendarDateSelector)CreateControl(typeof(CalendarDateSelector), new object[]{_SAB});
			frm.RestrictToOnlyWeeks = true;
			frm.RestrictToSingleDate = true;
			frm.AllowDynamicToPlan = false;
			frm.AllowDynamicToStoreOpen = false;
			drsBegin.DateRangeForm = frm;
		}

		private void drsBegin_Click(object sender, System.EventArgs e)
		{
			drsBegin.ShowSelector();
		}

		private void drsBegin_OnSelection(object source, MIDRetail.Windows.Controls.DateRangeSelectorEventArgs e)
		{
			DateRangeProfile drp;
			drp = e.SelectedDateRange;
			if (drp != null)
			{
				if (drp.Key == Include.UndefinedCalendarDateRange) //Allocation process looks for NoRID rather than Undefined value
				{
					_trans.AllocationNeedAnalysisPeriodBeginRID = Include.NoRID;
					drsBegin.DateRangeRID = Include.NoRID;
				}
				else
				{
					_trans.AllocationNeedAnalysisPeriodBeginRID = drp.Key;
					drsBegin.DateRangeRID = drp.Key;
				}
			}
		}

		private void drsEnd_Load(object sender, System.EventArgs e)
		{
			CalendarDateSelector frm = (CalendarDateSelector)CreateControl(typeof(CalendarDateSelector), new object[]{_SAB});
			frm.RestrictToOnlyWeeks = true;
			frm.RestrictToSingleDate = true;
			frm.AllowDynamicToPlan = false;
			frm.AllowDynamicToStoreOpen = false;
			drsEnd.DateRangeForm = frm;
		}

		private void drsEnd_Click(object sender, System.EventArgs e)
		{
			drsEnd.ShowSelector();
		}

		private void drsEnd_OnSelection(object source, MIDRetail.Windows.Controls.DateRangeSelectorEventArgs e)
		{
			DateRangeProfile drp;
			drp = e.SelectedDateRange;
			if (drp != null)
			{
				if (drp.Key == Include.UndefinedCalendarDateRange) //Allocation process looks for NoRID rather than Undefined value
				{
					_trans.AllocationNeedAnalysisPeriodEndRID = Include.NoRID;
					drsEnd.DateRangeRID = Include.NoRID;
				}
				else
				{
					_trans.AllocationNeedAnalysisPeriodEndRID = drp.Key;
					drsEnd.DateRangeRID = drp.Key;
				}
			}
		}

		#endregion

		private void chkIneligibleStores_CheckedChanged(object sender, System.EventArgs e)
		{
			_trans.AllocationIncludeIneligibleStores = chkIneligibleStores.Checked;
		}

        private void txtBasis_DragEnter(object sender, System.Windows.Forms.DragEventArgs e)
        {
            Image_DragEnter(sender, e);
        }

        private void txtBasis_DragOver(object sender, DragEventArgs e)
        {
            Image_DragOver(sender, e);
        }

        private void txtBasis_DragDrop(object sender, System.Windows.Forms.DragEventArgs e)
        {
            try
            {

                //Begin Track #5858 - Kjohnson - Validating store security only
                bool isSuccessfull = ((MIDTextBoxTag)(((TextBox)sender).Tag)).TextBox_DragDrop(sender, e);

                if (isSuccessfull)
                {
                    HierarchyNodeProfile hnp = (HierarchyNodeProfile)((MIDTag)(((TextBox)sender).Tag)).MIDTagData;

                    // BEGIN MID Track #6102 - color & size node display node not fully qualified
                    hnp = _SAB.HierarchyServerSession.GetNodeData(hnp.Key, true, true);
                    // END MID Track #6102

                    _trans.AllocationNeedAnalysisHNID = hnp.Key;
                    txtBasis.Text = hnp.Text;
                    _lastValidBasisRID = _trans.AllocationNeedAnalysisHNID; // MID Track #4311 - - error in basis text box

                    //ChangePending = true;
                    // Begin TT#99 - RMatelic - Headers In Use issue - comment our follwing line
                    //ApplySecurity();
                    // End TT#99 
                }
                //End Track #5858


                //// Create a new instance of the DataObject interface.
                //IDataObject data = Clipboard.GetDataObject();

                ////If the data is ClipboardProfile, then retrieve the data
                //ClipboardProfile cbp;

                //if (data.GetDataPresent(ClipboardProfile.Format.Name))
                //{
                //    cbp = (ClipboardProfile)data.GetData(ClipboardProfile.Format.Name);
                //    if (cbp.ClipboardDataType == eClipboardDataType.HierarchyNode)
                //    {
                //        if (cbp.ClipboardData.GetType() == typeof(HierarchyClipboardData))
                //        {
                //            _trans.AllocationNeedAnalysisHNID = cbp.Key;
                //            _hnp = _trans.GetNodeData(_trans.AllocationNeedAnalysisHNID);
                //            txtBasis.Text = _hnp.Text;
                //            _lastValidBasisRID = _trans.AllocationNeedAnalysisHNID; // MID Track #4311 - - error in basis text box
                //        }
                //        else
                //        {
                //            MessageBox.Show(_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_BadDataInClipboard));
                //        }
                //    }
                //    else
                //    {
                //        MessageBox.Show(_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MustBeNodeToDrop));
                //    }
                //}
                //else
                //{
                //    MessageBox.Show(_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MustBeNodeToDrop));
                //}
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void txtBasis_TextChanged(object sender, System.EventArgs e)
        {
            //if (txtBasis.Text.Trim() == string.Empty)
            //    _trans.AllocationNeedAnalysisHNID = Include.NoRID;
        }

		private void txtBasis_Validating(object sender, System.ComponentModel.CancelEventArgs e)
		{
            //string productID, errorMessage; 
            //int key;
            //try
            //{
            //    ErrorProvider.SetError(txtBasis,string.Empty);
            //    if (txtBasis.Modified)
            //    {
            //        if (txtBasis.Text.Trim().Length > 0)
            //        {
            //            productID = txtBasis.Text.Trim();
            //            key = GetNodeText(ref productID);
            //            if (key == Include.NoRID)
            //            {
            //                errorMessage = string.Format(_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_InvalidTreeNode), productID );	
            //                ErrorProvider.SetError(txtBasis,errorMessage);
            //                MessageBox.Show( errorMessage);
            //                e.Cancel = true;
            //                // BEGIN MID Track #4311 - error in basis text box
            //                if (_lastValidBasisRID != Include.NoRID)
            //                {
            //                    errorMessage = _SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_al_PreviousValidNodeQuestion);
            //                    if (MessageBox.Show (errorMessage,  this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question)
            //                        == DialogResult.Yes) 
            //                    {
            //                        _trans.AllocationNeedAnalysisHNID = _lastValidBasisRID;
            //                        _hnp = this._trans.GetNodeData(_trans.AllocationNeedAnalysisHNID );
            //                        txtBasis.Text = _hnp.Text;
            //                        txtBasis.Modified = false;
            //                    }
            //                    else	// MID Track #4311 - additional 
            //                    {
            //                        txtBasis.Text = null;
            //                    }
            //                }	
            //                else		// MID Track #4311 - additional 
            //                {
            //                    txtBasis.Text = null;
            //                }
            //                ErrorProvider.SetError(txtBasis,string.Empty);
            //            }	// END MID Track #4311
            //            else 
            //            {
							
            //                txtBasis.Text = productID;
            //                _trans.AllocationNeedAnalysisHNID = key;
            //                _lastValidBasisRID = _trans.AllocationNeedAnalysisHNID;
            //            }
            //        }
            //    }
            //}
            //catch
            //{
            //    throw;
            //}
		}

        private void txtBasis_Validated(object sender, System.EventArgs e)
        {
            try
            {
                //Begin Track #5858 - KJohnson- Validating store security only
                if ((((TextBox)sender).Text.Trim() == string.Empty) && (((TextBox)sender).Tag != null))
                {
                    _trans.AllocationNeedAnalysisHNID = Include.NoRID;
                }
                else
                {
                    HierarchyNodeProfile hnp = (HierarchyNodeProfile)((MIDTag)((TextBox)sender).Tag).MIDTagData;

                    // BEGIN MID Track #6102 - color & size node display node not fully qualified
                    hnp = _SAB.HierarchyServerSession.GetNodeData(hnp.Key, true, true);
                    // END MID Track #6102

                    txtBasis.Text = hnp.Text;
                    _trans.AllocationNeedAnalysisHNID = hnp.Key;
                    _lastValidBasisRID = _trans.AllocationNeedAnalysisHNID;

                    //ChangePending = true;
                    // Begin TT#99 - RMatelic - Headers In Use issue - comment our follwing line
                    //ApplySecurity();
                    // End TT#99 
                }
                //End Track #5858
            }
			catch
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
				string[] pArray = productID.Split(new char[] {'['});
				productID = pArray[0].Trim(); 
//				HierarchyNodeProfile hnp = _SAB.HierarchyServerSession.GetNodeData(productID);
				HierarchyMaintenance hm = new HierarchyMaintenance(_SAB);
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

		private void ugHeader_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			try
			{	
				if (e.KeyData == Keys.Delete && this.ugHeader.Selected.Rows.Count > 0) 
				{
					_profileToDelete = null;
					_deleteList = new AllocationHeaderProfileList(eProfileType.AllocationHeader);
				}
			
			}
			catch(Exception ex)
			{
				HandleException(ex);
			}
		}

        protected bool ApplySecurity()	// Track 5871 stodd
        {
            bool securityOk = true; // track #5871 stodd

            CheckSecurityEnqueue();

            return securityOk;	// track 5871 stodd
        }

		override protected void BeforeClosing()
		{
			try
			{
				if (ugHeader.ActiveCell != null && _errorFound)
					ugHeader.ActiveCell.CancelUpdate();
			}
			catch(Exception exception)
			{
				HandleException(exception);
			}
		}

        // Begin TT#2093 - JSmith - Need Analysis Add basis and after window opens add columns and receive an error.
        private void ugBasisDropdownUpdate()
        {
            try
            {
                int BasisHNRID = Include.NoRID;
                int BasisPHRID = Include.NoRID;
                int BasisPHLSequence = 0;
                object value = 0;

                ugBasisNodeVersion.BeginUpdate();
                foreach (UltraGridRow row in this.ugBasisNodeVersion.Rows)
                {
                    BasisHNRID = Convert.ToInt32(row.Cells["BasisHNRID"].Value);
                    BasisPHRID = Convert.ToInt32(row.Cells["BasisPHRID"].Value);
                    BasisPHLSequence = Convert.ToInt32(row.Cells["BasisPHLSequence"].Value);
                    value =row.Cells["Merchandise"].Value;
                    if (cbxAnalysis.Checked)
                    {
                        if (BasisHNRID != Include.NoRID ||
                           row.Cells["Merchandise"].Value == DBNull.Value ||
                            String.IsNullOrEmpty(Convert.ToString(row.Cells["Merchandise"].Value)))
                        {
                            row.Activation = Activation.AllowEdit;
                            row.Cells["Merchandise"].ValueList = BuildAnalysisValuelist();
                            row.Cells["Merchandise"].Value = DBNull.Value;
                            if (value != DBNull.Value)
                            {
                                row.Cells["Merchandise"].Value = GetValueListRow((Infragistics.Win.ValueList)row.Cells["Merchandise"].ValueList, BasisHNRID, BasisPHRID, BasisPHLSequence);
                            }
                        }
                        else
                        {
                            row.Activation = Activation.Disabled;
                        }
                    }
                    else
                    {
                        row.Activation = Activation.AllowEdit;
                        row.Cells["Merchandise"].ValueList = null;
                        row.Cells["Merchandise"].Value = DBNull.Value;
                        if (value != DBNull.Value)
                        {
                            row.Cells["Merchandise"].Value = GetValueListRow(ugBasisNodeVersion.DisplayLayout.ValueLists["Merchandise"], BasisHNRID, BasisPHRID, BasisPHLSequence);
                        }
                        
                    }
                }
            }
            catch (Exception exception)
            {
                HandleException(exception);
            }
            finally
            {
                ugBasisNodeVersion.EndUpdate();
            }
        }

        private int GetValueListRow(Infragistics.Win.ValueList aValueList, int aBasisHNRID, int aBasisPHRID, int aBasisPHLSequence)
        {
            int value = -1;
            int rowseq = -1;
            if (aBasisHNRID != Include.NoRID)
            {
                value = aBasisHNRID;
            }
            else if (aBasisPHRID != Include.NoRID)
            {
                value = aBasisPHLSequence;
            }

            foreach (Infragistics.Win.ValueListItem vli in aValueList.ValueListItems)
            {
                if (vli.DataValue != DBNull.Value)
                {
                    if (Convert.ToInt32(vli.Tag) == Convert.ToInt32(value))
                    {
                        rowseq = Convert.ToInt32(vli.DataValue, CultureInfo.CurrentUICulture);
                        break;
                    }
                }
            }
            return rowseq;
        }
        // End TT#2093 - JSmith - Need Analysis Add basis and after window opens add columns and receive an error.

		private void cbxAnalysis_CheckedChanged(object sender, System.EventArgs e)
		{
                ugHeader.Enabled = !cbxAnalysis.Checked;
                ugBasisDropdownUpdate();     //TT#2093 - MD -  Analysis when Basis not specific will cause an error - RBeck
		}

		// BEGIN MID Track #4567 - John Smith - add filtering
		private void picBox_Click(object sender, System.EventArgs e)
		{
			try
			{	
				string enteredMask = string.Empty;
				bool caseSensitive = false;
				PictureBox picBox = (PictureBox)sender;

				if (CharMaskFromDialogOK(picBox, ref enteredMask, ref caseSensitive))
				{
					base.FormLoaded = false;
					switch (picBox.Name)
					{
						case "picBoxCurve":
							BindSizeCurveComboBox(true, enteredMask, caseSensitive);
							break;
                        // Begin TT#2208 - JSmith - Style Review Slow Performance
                        //case "picBoxConstraint":
                        //    BindSizeConstraintsComboBox(true, enteredMask, caseSensitive);
                        //    break;
                        //case "picBoxAlternate":
                        //    BindSizeAlternatesComboBox(true, enteredMask, caseSensitive);
                        //    break;
                        // End TT#2208
						
					}
					base.FormLoaded = true;
				}
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}

		protected void DisplayPictureBoxImages()
		{
			DisplayPictureBoxImage(picBoxConstraint);
			DisplayPictureBoxImage(picBoxAlternate);
			DisplayPictureBoxImage(picBoxCurve);
		}

		protected void SetPictureBoxTags()
		{
			picBoxConstraint.Tag = _SAB.ApplicationServerSession.GlobalOptions.SizeConstraintCharMask;
			picBoxAlternate.Tag = _SAB.ApplicationServerSession.GlobalOptions.SizeGroupCharMask;
			picBoxCurve.Tag = _SAB.ApplicationServerSession.GlobalOptions.SizeCurveCharMask;
		}

		protected void SetMaskedComboBoxesEnabled()
		{
			if (_SAB.ApplicationServerSession.GlobalOptions.SizeCurveCharMask != string.Empty)
			{
                this.cboSizeCurve.Enabled = false;
			}
			if (_SAB.ApplicationServerSession.GlobalOptions.SizeConstraintCharMask != string.Empty)
			{
				this.cboConstraints.Enabled = false;
			}
			if (_SAB.ApplicationServerSession.GlobalOptions.SizeAlternateCharMask != string.Empty)
			{
				this.cboAlternates.Enabled = false;
			}
		}

		private void DisplayPictureBoxImage(System.Windows.Forms.PictureBox aPicBox)
		{
			Image image;
			try
			{
				image = Image.FromFile(MIDGraphics.ImageDir + "\\" + MIDGraphics.MagnifyingGlassImage);
				SizeF sizef = new SizeF(aPicBox.Width, aPicBox.Height);
				Size size = Size.Ceiling(sizef);
				Bitmap bitmap = new Bitmap(image, size);
				aPicBox.Image = bitmap;
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
				throw;
			}
		}

		private bool CharMaskFromDialogOK(PictureBox aPicBox, ref string aEnteredMask, ref bool aCaseSensitive)
		{
			bool maskOK = false;
			string errMessage = string.Empty;
            
			try
			{
				bool cancelAction = false;
				string dialogLabel = MIDText.GetTextOnly((int)eMIDTextCode.lbl_FilterSelection);
				string textLabel = MIDText.GetTextOnly((int)eMIDTextCode.lbl_FilterSelectionText);
				 
				string globalMask = Convert.ToString(aPicBox.Tag, CultureInfo.CurrentUICulture);	
			
				NameDialog nameDialog = new NameDialog(dialogLabel, textLabel, globalMask);
				nameDialog.AllowCaseSensitive();
     
				while (!(maskOK || cancelAction))
				{
					nameDialog.StartPosition = FormStartPosition.CenterParent;
					nameDialog.TreatEmptyAsCancel = false;
					DialogResult dialogResult = nameDialog.ShowDialog();

					if (dialogResult == DialogResult.Cancel)
						cancelAction = true;
					else
					{
						maskOK = false;
						aEnteredMask = nameDialog.TextValue.Trim();
						aCaseSensitive = nameDialog.CaseSensitive;
						maskOK = (globalMask == string.Empty) ? true : EnteredMaskOK(aPicBox,  aEnteredMask, globalMask);
						
						if (!maskOK)
						{
							errMessage = _SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_al_FilterGlobalOptionMismatch);
							MessageBox.Show(errMessage, dialogLabel, MessageBoxButtons.OK, MessageBoxIcon.Error);
						}
					}
				}

				if (cancelAction)
				{
					maskOK = false;
				}
				else
				{
					nameDialog.Dispose();
				}
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
			
			if(!aEnteredMask.EndsWith("*"))
			{
				aEnteredMask += "*";
			}

			return maskOK;
		}

		private bool EnteredMaskOK(PictureBox aPicBox, string aEnteredMask, string aGlobalMask)
		{
			bool maskOK = true;
			try
			{
                //bool okToContinue = true;
				int gXCount = 0;
				int eXCount = 0;
				int gWCount = 0;
				int eWCount = 0;
				char wildCard = '*';
				
				char[] cGlobalArray = aGlobalMask.ToCharArray(0, aGlobalMask.Length);
				for (int i = 0; i < cGlobalArray.Length; i++)
				{
					if (cGlobalArray[i] == wildCard)
					{
						gWCount++;
					}
					else
					{
						gXCount++;
					}
				}
				char[] cEnteredArray = aEnteredMask.ToCharArray(0, aEnteredMask.Length);
				for (int i = 0; i < cEnteredArray.Length; i++)
				{
					if (cEnteredArray[i] == wildCard)
					{
						eWCount++;
					}
					else
					{
						eXCount++;
					}
				}

				if (eXCount < gXCount)
				{
					maskOK = false;
				}
				else if (eXCount > gXCount && gWCount == 0)
				{  
					maskOK = false;
				}
				else if (aEnteredMask.Length < aGlobalMask.Length && !aGlobalMask.EndsWith("*"))	
				{  
					maskOK = false;
				}
				string[] globalParts = aGlobalMask.Split(new char[] {'*'});
				string[] enteredParts = aEnteredMask.Split(new char[] {'*'});
				int gLastEntry = globalParts.Length - 1;
				int eLastEntry = enteredParts.Length - 1;
				if (enteredParts[0].Length < globalParts[0].Length)
				{
					maskOK = false;
				}
				else if (enteredParts[eLastEntry].Length < globalParts[gLastEntry].Length)
				{
					maskOK = false;
				}
			}
			catch
			{
				throw;
			}
			return maskOK;
		}
        // END MID Track #4567

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
        // END MID Track #4567

        // begin TT#1185 - Verify ENQ before Update
        ////begin TT#311 - Enqueue flag - apicchetti
        //bool _bypassSecurityEnqueueCheck = false;
        //public bool BypassSecurityEnqueueCheck
        //{
        //    get
        //    {
        //        return _bypassSecurityEnqueueCheck;
        //    }

        //    set
        //    {
        //        _bypassSecurityEnqueueCheck = value;
        //    }
        //}
        ////end TT#311 - Enqueue flag - apicchetti
        // end TT#1185 - Verify ENQ before Update
	}
}
