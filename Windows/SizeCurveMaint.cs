// BEGIN MID Track #5170 - JSmith - Model enhancements
// Too many lines changed to mark.  Use SCM Compare for details.
//End Track #5170
using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Forms;

using MIDRetail.Business;
using MIDRetail.Common;
using MIDRetail.Data;
using MIDRetail.DataCommon;
using MIDRetail.Windows.Controls;

using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win.UltraWinMaskedEdit;

namespace MIDRetail.Windows
{
	/// <summary>
	/// Summary description for SizeCurve.
	/// </summary>
	public class frmSizeCurve : MIDFormBase
	{
        // Begin Track #4872 - JSmith - Global/User Attributes
        private MIDAttributeComboBox cboStoreAttribute;
        // End Track #4872


        private Infragistics.Win.UltraWinGrid.UltraGrid ulgSizeCurve;
		private Infragistics.Win.UltraWinGrid.UltraGrid ulgDefaultSizeCurve;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Splitter splitter1;
		private System.Windows.Forms.Button btnSave;
		private System.Windows.Forms.Button btnClose;
		private System.Windows.Forms.Button btnSaveAs;
		private System.Windows.Forms.Button btnNew;
		private System.Windows.Forms.Button btnDelete;
		private System.Windows.Forms.ContextMenu ctmSizeCurve;
		private System.Windows.Forms.MenuItem mniSetAllCurvesInSet;
		private System.Windows.Forms.MenuItem mniCopyCurveToDefault;
		private System.Windows.Forms.MenuItem mniBalanceTo100;
		private System.Windows.Forms.ContextMenu ctmDefaultSizeCurve;
		private System.Windows.Forms.MenuItem mniBalanceDefaultTo100;
		private System.Windows.Forms.MenuItem mniCollapse;
		private System.Windows.Forms.MenuItem mniExpand;
		private System.Windows.Forms.MenuItem mniBalanceAllTo100;
		private System.Windows.Forms.Label lblSizeCurve;
		private System.Windows.Forms.Label lblStoreAttribute;
		private System.Windows.Forms.Label lblSizeGroup;
		protected System.Windows.Forms.PictureBox picBoxCurve;
		protected System.Windows.Forms.PictureBox picBoxGroup;
        private MIDComboBoxEnh cboSizeCurveGroup;
        private MIDComboBoxEnh cboSizeGroup;

        //BEGIN TT#110-MD-VStuart - In Use Tool
        private bool _display;
        private ArrayList _ridList; //TT#641-MD-VStuart-Size Curve Delete Needs to use new In Use Tool
        //private Button btnInUse;
        protected Button btnInUse;
        protected bool _isInQuiry = true;
        //END TT#110-MD-VStuart - In Use Tool

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

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

				this.Load -= new System.EventHandler(this.frmSizeCurve_Load);
				this.btnDelete.Click -= new System.EventHandler(this.btnDelete_Click);
				this.btnNew.Click -= new System.EventHandler(this.btnNew_Click);
				this.btnSaveAs.Click -= new System.EventHandler(this.btnSaveAs_Click);
				this.mniBalanceDefaultTo100.Click -= new System.EventHandler(this.mniBalanceDefaultTo100_Click);
				this.ulgDefaultSizeCurve.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.ulgDefaultSizeCurve_MouseDown);
				this.ulgDefaultSizeCurve.AfterCellUpdate -= new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ulgDefaultSizeCurve_AfterCellUpdate);
				this.ulgDefaultSizeCurve.BeforeEnterEditMode -= new System.ComponentModel.CancelEventHandler(this.ulgDefaultSizeCurve_BeforeEnterEditMode);
                this.ulgDefaultSizeCurve.InitializeLayout -= new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.ulgDefaultSizeCurve_InitializeLayout);
                //Begin TT#169 - JSmith - enter max of 1000 and receive 'must be integer'
                MIDRetail.Windows.Controls.UltraGridLayoutDefaults ugld = new MIDRetail.Windows.Controls.UltraGridLayoutDefaults(ErrorImage);
                ugld.DetachGridEventHandlers(ulgDefaultSizeCurve);
                //End TT#169
				this.cboSizeGroup.SelectionChangeCommitted -= new System.EventHandler(this.cboSizeGroup_SelectionChangeCommitted);
				this.btnClose.Click -= new System.EventHandler(this.btnClose_Click);
				this.btnSave.Click -= new System.EventHandler(this.btnSave_Click);
				this.mniCopyCurveToDefault.Click -= new System.EventHandler(this.mniCopyCurveToDefault_Click);
				this.mniBalanceTo100.Click -= new System.EventHandler(this.mniBalanceTo100_Click);
				this.mniBalanceAllTo100.Click -= new System.EventHandler(this.mniBalanceAllTo100_Click);
				this.mniExpand.Click -= new System.EventHandler(this.mniExpand_Click);
				this.mniCollapse.Click -= new System.EventHandler(this.mniCollapse_Click);
				this.ctmSizeCurve.Popup -= new System.EventHandler(this.ctmSizeCurve_Popup);
				this.ulgSizeCurve.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.ulgSizeCurve_MouseDown);
				this.ulgSizeCurve.AfterExitEditMode -= new System.EventHandler(this.ulgSizeCurve_AfterExitEditMode);
				this.ulgSizeCurve.AfterCellListCloseUp -= new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ulgSizeCurve_AfterCellListCloseUp);
				this.ulgSizeCurve.BeforeCellListDropDown -= new Infragistics.Win.UltraWinGrid.CancelableCellEventHandler(this.ulgSizeCurve_BeforeCellListDropDown);
				this.ulgSizeCurve.AfterCellUpdate -= new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ulgSizeCurve_AfterCellUpdate);
				this.ulgSizeCurve.BeforeEnterEditMode -= new System.ComponentModel.CancelEventHandler(this.ulgSizeCurve_BeforeEnterEditMode);
                this.ulgSizeCurve.InitializeLayout -= new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.ulgSizeCurve_InitializeLayout);
                //Begin TT#169 - JSmith - enter max of 1000 and receive 'must be integer'
                ugld.DetachGridEventHandlers(ulgSizeCurve);
                //End TT#169
				this.cboStoreAttribute.SelectionChangeCommitted -= new System.EventHandler(this.cboStoreAttribute_SelectionChangeCommitted);
                this.cboStoreAttribute.DragDrop -= new System.Windows.Forms.DragEventHandler(this.cboStoreAttribute_DragDrop);
                this.cboStoreAttribute.DragEnter -= new System.Windows.Forms.DragEventHandler(this.cboStoreAttribute_DragEnter);
				this.cboSizeCurveGroup.SelectionChangeCommitted -= new System.EventHandler(this.cboSizeCurveGroup_SelectionChangeCommitted);

                this.cboStoreAttribute.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler(cboStoreAttribute_MIDComboBoxPropertiesChangedEvent);
                this.cboSizeCurveGroup.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler(cboSizeCurveGroup_MIDComboBoxPropertiesChangedEvent);
                this.cboSizeGroup.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler(cboSizeGroup_MIDComboBoxPropertiesChangedEvent);
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
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance11 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance12 = new Infragistics.Win.Appearance();
            this.lblSizeCurve = new System.Windows.Forms.Label();
            this.lblStoreAttribute = new System.Windows.Forms.Label();
            this.cboStoreAttribute = new MIDRetail.Windows.Controls.MIDAttributeComboBox();
            this.ulgSizeCurve = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.ctmSizeCurve = new System.Windows.Forms.ContextMenu();
            this.mniCollapse = new System.Windows.Forms.MenuItem();
            this.mniExpand = new System.Windows.Forms.MenuItem();
            this.mniBalanceAllTo100 = new System.Windows.Forms.MenuItem();
            this.mniSetAllCurvesInSet = new System.Windows.Forms.MenuItem();
            this.mniBalanceTo100 = new System.Windows.Forms.MenuItem();
            this.mniCopyCurveToDefault = new System.Windows.Forms.MenuItem();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.lblSizeGroup = new System.Windows.Forms.Label();
            this.ulgDefaultSizeCurve = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.ctmDefaultSizeCurve = new System.Windows.Forms.ContextMenu();
            this.mniBalanceDefaultTo100 = new System.Windows.Forms.MenuItem();
            this.panel1 = new System.Windows.Forms.Panel();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.btnSaveAs = new System.Windows.Forms.Button();
            this.btnNew = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.picBoxCurve = new System.Windows.Forms.PictureBox();
            this.picBoxGroup = new System.Windows.Forms.PictureBox();
            this.cboSizeCurveGroup = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this.cboSizeGroup = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this.btnInUse = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.utmMain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ulgSizeCurve)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ulgDefaultSizeCurve)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxCurve)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxGroup)).BeginInit();
            this.SuspendLayout();
            // 
            // utmMain
            // 
            this.utmMain.MenuSettings.ForceSerialization = true;
            this.utmMain.ToolbarSettings.ForceSerialization = true;
            // 
            // lblSizeCurve
            // 
            this.lblSizeCurve.Location = new System.Drawing.Point(0, 16);
            this.lblSizeCurve.Name = "lblSizeCurve";
            this.lblSizeCurve.Size = new System.Drawing.Size(64, 24);
            this.lblSizeCurve.TabIndex = 0;
            this.lblSizeCurve.Text = "Size Curve";
            this.lblSizeCurve.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblStoreAttribute
            // 
            this.lblStoreAttribute.Location = new System.Drawing.Point(272, 16);
            this.lblStoreAttribute.Name = "lblStoreAttribute";
            this.lblStoreAttribute.Size = new System.Drawing.Size(80, 24);
            this.lblStoreAttribute.TabIndex = 2;
            this.lblStoreAttribute.Text = "Store Attribute";
            this.lblStoreAttribute.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cboStoreAttribute
            // 
            this.cboStoreAttribute.AllowDrop = true;
            this.cboStoreAttribute.AllowUserAttributes = false;
            this.cboStoreAttribute.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboStoreAttribute.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboStoreAttribute.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboStoreAttribute.Location = new System.Drawing.Point(352, 16);
            this.cboStoreAttribute.Name = "cboStoreAttribute";
            this.cboStoreAttribute.Size = new System.Drawing.Size(168, 21);
            this.cboStoreAttribute.TabIndex = 3;
            this.cboStoreAttribute.MIDComboBoxPropertiesChangedEvent += new MIDRetail.Windows.Controls.MIDComboBoxPropertiesChangedEventHandler(this.cboStoreAttribute_MIDComboBoxPropertiesChangedEvent);
            this.cboStoreAttribute.SelectionChangeCommitted += new System.EventHandler(this.cboStoreAttribute_SelectionChangeCommitted);
            this.cboStoreAttribute.DragDrop += new System.Windows.Forms.DragEventHandler(this.cboStoreAttribute_DragDrop);
            this.cboStoreAttribute.DragEnter += new System.Windows.Forms.DragEventHandler(this.cboStoreAttribute_DragEnter);
            this.cboStoreAttribute.DragOver += new System.Windows.Forms.DragEventHandler(this.cboStoreAttribute_DragOver);
            // 
            // ulgSizeCurve
            // 
            appearance1.BackColor = System.Drawing.Color.White;
            appearance1.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance1.BackGradientStyle = Infragistics.Win.GradientStyle.ForwardDiagonal;
            this.ulgSizeCurve.DisplayLayout.Appearance = appearance1;
            this.ulgSizeCurve.DisplayLayout.InterBandSpacing = 10;
            appearance2.BackColor = System.Drawing.Color.Transparent;
            this.ulgSizeCurve.DisplayLayout.Override.CardAreaAppearance = appearance2;
            appearance3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance3.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance3.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance3.ForeColor = System.Drawing.Color.Black;
            appearance3.TextHAlignAsString = "Left";
            appearance3.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            this.ulgSizeCurve.DisplayLayout.Override.HeaderAppearance = appearance3;
            appearance4.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.ulgSizeCurve.DisplayLayout.Override.RowAppearance = appearance4;
            appearance5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance5.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance5.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            this.ulgSizeCurve.DisplayLayout.Override.RowSelectorAppearance = appearance5;
            this.ulgSizeCurve.DisplayLayout.Override.RowSelectorWidth = 12;
            this.ulgSizeCurve.DisplayLayout.Override.RowSpacingBefore = 2;
            appearance6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance6.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance6.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance6.ForeColor = System.Drawing.Color.Black;
            this.ulgSizeCurve.DisplayLayout.Override.SelectedRowAppearance = appearance6;
            this.ulgSizeCurve.DisplayLayout.RowConnectorColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.ulgSizeCurve.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.Solid;
            this.ulgSizeCurve.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ulgSizeCurve.Location = new System.Drawing.Point(0, 134);
            this.ulgSizeCurve.Name = "ulgSizeCurve";
            this.ulgSizeCurve.Size = new System.Drawing.Size(792, 362);
            this.ulgSizeCurve.TabIndex = 4;
            this.ulgSizeCurve.UpdateMode = Infragistics.Win.UltraWinGrid.UpdateMode.OnCellChangeOrLostFocus;
            this.ulgSizeCurve.AfterCellUpdate += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ulgSizeCurve_AfterCellUpdate);
            this.ulgSizeCurve.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.ulgSizeCurve_InitializeLayout);
            this.ulgSizeCurve.AfterExitEditMode += new System.EventHandler(this.ulgSizeCurve_AfterExitEditMode);
            this.ulgSizeCurve.AfterCellListCloseUp += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ulgSizeCurve_AfterCellListCloseUp);
            this.ulgSizeCurve.BeforeCellListDropDown += new Infragistics.Win.UltraWinGrid.CancelableCellEventHandler(this.ulgSizeCurve_BeforeCellListDropDown);
            this.ulgSizeCurve.BeforeEnterEditMode += new System.ComponentModel.CancelEventHandler(this.ulgSizeCurve_BeforeEnterEditMode);
            this.ulgSizeCurve.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ulgSizeCurve_MouseDown);
            // 
            // ctmSizeCurve
            // 
            this.ctmSizeCurve.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mniCollapse,
            this.mniExpand,
            this.mniBalanceAllTo100,
            this.mniSetAllCurvesInSet,
            this.mniBalanceTo100,
            this.mniCopyCurveToDefault});
            this.ctmSizeCurve.Popup += new System.EventHandler(this.ctmSizeCurve_Popup);
            // 
            // mniCollapse
            // 
            this.mniCollapse.Index = 0;
            this.mniCollapse.Text = "Collapse all Stores";
            this.mniCollapse.Click += new System.EventHandler(this.mniCollapse_Click);
            // 
            // mniExpand
            // 
            this.mniExpand.Index = 1;
            this.mniExpand.Text = "Expand all Stores";
            this.mniExpand.Click += new System.EventHandler(this.mniExpand_Click);
            // 
            // mniBalanceAllTo100
            // 
            this.mniBalanceAllTo100.Index = 2;
            this.mniBalanceAllTo100.Text = "Balance all Curves to 100%";
            this.mniBalanceAllTo100.Click += new System.EventHandler(this.mniBalanceAllTo100_Click);
            // 
            // mniSetAllCurvesInSet
            // 
            this.mniSetAllCurvesInSet.Index = 3;
            this.mniSetAllCurvesInSet.Text = "Set all Curves in Set to";
            // 
            // mniBalanceTo100
            // 
            this.mniBalanceTo100.Index = 4;
            this.mniBalanceTo100.Text = "Balance Curve to 100%";
            this.mniBalanceTo100.Click += new System.EventHandler(this.mniBalanceTo100_Click);
            // 
            // mniCopyCurveToDefault
            // 
            this.mniCopyCurveToDefault.Index = 5;
            this.mniCopyCurveToDefault.Text = "Copy Curve to Default";
            this.mniCopyCurveToDefault.Click += new System.EventHandler(this.mniCopyCurveToDefault_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Location = new System.Drawing.Point(368, 560);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(80, 24);
            this.btnSave.TabIndex = 5;
            this.btnSave.Text = "Save";
            this.btnSave.EnabledChanged += new System.EventHandler(this.btnSave_EnabledChanged);
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(720, 560);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(80, 24);
            this.btnClose.TabIndex = 6;
            this.btnClose.Text = "Close";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // lblSizeGroup
            // 
            this.lblSizeGroup.Location = new System.Drawing.Point(520, 16);
            this.lblSizeGroup.Name = "lblSizeGroup";
            this.lblSizeGroup.Size = new System.Drawing.Size(64, 24);
            this.lblSizeGroup.TabIndex = 7;
            this.lblSizeGroup.Text = "Size Group";
            this.lblSizeGroup.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ulgDefaultSizeCurve
            // 
            appearance7.BackColor = System.Drawing.Color.White;
            appearance7.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance7.BackGradientStyle = Infragistics.Win.GradientStyle.ForwardDiagonal;
            this.ulgDefaultSizeCurve.DisplayLayout.Appearance = appearance7;
            this.ulgDefaultSizeCurve.DisplayLayout.InterBandSpacing = 10;
            appearance8.BackColor = System.Drawing.Color.Transparent;
            this.ulgDefaultSizeCurve.DisplayLayout.Override.CardAreaAppearance = appearance8;
            appearance9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance9.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance9.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance9.ForeColor = System.Drawing.Color.Black;
            appearance9.TextHAlignAsString = "Left";
            appearance9.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            this.ulgDefaultSizeCurve.DisplayLayout.Override.HeaderAppearance = appearance9;
            appearance10.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.ulgDefaultSizeCurve.DisplayLayout.Override.RowAppearance = appearance10;
            appearance11.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance11.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance11.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            this.ulgDefaultSizeCurve.DisplayLayout.Override.RowSelectorAppearance = appearance11;
            this.ulgDefaultSizeCurve.DisplayLayout.Override.RowSelectorWidth = 12;
            this.ulgDefaultSizeCurve.DisplayLayout.Override.RowSpacingBefore = 2;
            appearance12.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance12.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance12.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance12.ForeColor = System.Drawing.Color.Black;
            this.ulgDefaultSizeCurve.DisplayLayout.Override.SelectedRowAppearance = appearance12;
            this.ulgDefaultSizeCurve.DisplayLayout.RowConnectorColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.ulgDefaultSizeCurve.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.Solid;
            this.ulgDefaultSizeCurve.Dock = System.Windows.Forms.DockStyle.Top;
            this.ulgDefaultSizeCurve.Location = new System.Drawing.Point(0, 0);
            this.ulgDefaultSizeCurve.Name = "ulgDefaultSizeCurve";
            this.ulgDefaultSizeCurve.Size = new System.Drawing.Size(792, 128);
            this.ulgDefaultSizeCurve.TabIndex = 9;
            this.ulgDefaultSizeCurve.UpdateMode = Infragistics.Win.UltraWinGrid.UpdateMode.OnCellChangeOrLostFocus;
            this.ulgDefaultSizeCurve.AfterCellUpdate += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ulgDefaultSizeCurve_AfterCellUpdate);
            this.ulgDefaultSizeCurve.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.ulgDefaultSizeCurve_InitializeLayout);
            this.ulgDefaultSizeCurve.BeforeEnterEditMode += new System.ComponentModel.CancelEventHandler(this.ulgDefaultSizeCurve_BeforeEnterEditMode);
            this.ulgDefaultSizeCurve.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ulgDefaultSizeCurve_MouseDown);
            // 
            // ctmDefaultSizeCurve
            // 
            this.ctmDefaultSizeCurve.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mniBalanceDefaultTo100});
            // 
            // mniBalanceDefaultTo100
            // 
            this.mniBalanceDefaultTo100.Index = 0;
            this.mniBalanceDefaultTo100.Text = "Balance Curve to 100%";
            this.mniBalanceDefaultTo100.Click += new System.EventHandler(this.mniBalanceDefaultTo100_Click);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.ulgSizeCurve);
            this.panel1.Controls.Add(this.splitter1);
            this.panel1.Controls.Add(this.ulgDefaultSizeCurve);
            this.panel1.Location = new System.Drawing.Point(8, 56);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(792, 496);
            this.panel1.TabIndex = 10;
            // 
            // splitter1
            // 
            this.splitter1.Cursor = System.Windows.Forms.Cursors.HSplit;
            this.splitter1.Dock = System.Windows.Forms.DockStyle.Top;
            this.splitter1.Location = new System.Drawing.Point(0, 128);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(792, 6);
            this.splitter1.TabIndex = 10;
            this.splitter1.TabStop = false;
            // 
            // btnSaveAs
            // 
            this.btnSaveAs.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSaveAs.Location = new System.Drawing.Point(456, 560);
            this.btnSaveAs.Name = "btnSaveAs";
            this.btnSaveAs.Size = new System.Drawing.Size(80, 24);
            this.btnSaveAs.TabIndex = 11;
            this.btnSaveAs.Text = "Save As";
            this.btnSaveAs.EnabledChanged += new System.EventHandler(this.btnSaveAs_EnabledChanged);
            this.btnSaveAs.Click += new System.EventHandler(this.btnSaveAs_Click);
            // 
            // btnNew
            // 
            this.btnNew.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnNew.Location = new System.Drawing.Point(632, 560);
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(80, 24);
            this.btnNew.TabIndex = 12;
            this.btnNew.Text = "New";
            this.btnNew.EnabledChanged += new System.EventHandler(this.btnNew_EnabledChanged);
            this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDelete.Location = new System.Drawing.Point(544, 560);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(80, 24);
            this.btnDelete.TabIndex = 13;
            this.btnDelete.Text = "Delete";
            this.btnDelete.EnabledChanged += new System.EventHandler(this.btnDelete_EnabledChanged);
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // picBoxCurve
            // 
            this.picBoxCurve.Location = new System.Drawing.Point(72, 16);
            this.picBoxCurve.Name = "picBoxCurve";
            this.picBoxCurve.Size = new System.Drawing.Size(20, 20);
            this.picBoxCurve.TabIndex = 56;
            this.picBoxCurve.TabStop = false;
            this.picBoxCurve.Click += new System.EventHandler(this.picBoxFilter_Click);
            this.picBoxCurve.MouseHover += new System.EventHandler(this.picBoxFilter_MouseHover);
            // 
            // picBoxGroup
            // 
            this.picBoxGroup.Location = new System.Drawing.Point(592, 16);
            this.picBoxGroup.Name = "picBoxGroup";
            this.picBoxGroup.Size = new System.Drawing.Size(20, 20);
            this.picBoxGroup.TabIndex = 57;
            this.picBoxGroup.TabStop = false;
            this.picBoxGroup.Click += new System.EventHandler(this.picBoxFilter_Click);
            this.picBoxGroup.MouseHover += new System.EventHandler(this.picBoxFilter_MouseHover);
            // 
            // cboSizeCurveGroup
            // 
            this.cboSizeCurveGroup.AutoAdjust = true;
            this.cboSizeCurveGroup.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboSizeCurveGroup.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboSizeCurveGroup.DataSource = null;
            this.cboSizeCurveGroup.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSizeCurveGroup.DropDownWidth = 168;
            this.cboSizeCurveGroup.FormattingEnabled = false;
            this.cboSizeCurveGroup.IgnoreFocusLost = false;
            this.cboSizeCurveGroup.ItemHeight = 13;
            this.cboSizeCurveGroup.Location = new System.Drawing.Point(96, 16);
            this.cboSizeCurveGroup.Margin = new System.Windows.Forms.Padding(0);
            this.cboSizeCurveGroup.MaxDropDownItems = 25;
            this.cboSizeCurveGroup.Name = "cboSizeCurveGroup";
            this.cboSizeCurveGroup.Size = new System.Drawing.Size(168, 21);
            this.cboSizeCurveGroup.TabIndex = 1;
            this.cboSizeCurveGroup.Tag = null;
            this.cboSizeCurveGroup.MIDComboBoxPropertiesChangedEvent += new MIDRetail.Windows.Controls.MIDComboBoxPropertiesChangedEventHandler(this.cboSizeCurveGroup_MIDComboBoxPropertiesChangedEvent);
            this.cboSizeCurveGroup.SelectionChangeCommitted += new System.EventHandler(this.cboSizeCurveGroup_SelectionChangeCommitted);
            // 
            // cboSizeGroup
            // 
            this.cboSizeGroup.AutoAdjust = true;
            this.cboSizeGroup.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboSizeGroup.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboSizeGroup.DataSource = null;
            this.cboSizeGroup.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSizeGroup.DropDownWidth = 168;
            this.cboSizeGroup.FormattingEnabled = false;
            this.cboSizeGroup.IgnoreFocusLost = false;
            this.cboSizeGroup.ItemHeight = 13;
            this.cboSizeGroup.Location = new System.Drawing.Point(616, 16);
            this.cboSizeGroup.Margin = new System.Windows.Forms.Padding(0);
            this.cboSizeGroup.MaxDropDownItems = 25;
            this.cboSizeGroup.Name = "cboSizeGroup";
            this.cboSizeGroup.Size = new System.Drawing.Size(168, 21);
            this.cboSizeGroup.TabIndex = 8;
            this.cboSizeGroup.Tag = null;
            this.cboSizeGroup.MIDComboBoxPropertiesChangedEvent += new MIDRetail.Windows.Controls.MIDComboBoxPropertiesChangedEventHandler(this.cboSizeGroup_MIDComboBoxPropertiesChangedEvent);
            this.cboSizeGroup.SelectionChangeCommitted += new System.EventHandler(this.cboSizeGroup_SelectionChangeCommitted);
            // 
            // btnInUse
            // 
            this.btnInUse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnInUse.Location = new System.Drawing.Point(8, 561);
            this.btnInUse.Name = "btnInUse";
            this.btnInUse.Size = new System.Drawing.Size(75, 23);
            this.btnInUse.TabIndex = 58;
            this.btnInUse.Text = "In Use";
            this.btnInUse.UseVisualStyleBackColor = true;
            this.btnInUse.Click += new System.EventHandler(this.btnInUse_Click);
            // 
            // frmSizeCurve
            // 
            this.AllowDragDrop = true;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(808, 590);
            this.Controls.Add(this.btnInUse);
            this.Controls.Add(this.cboSizeGroup);
            this.Controls.Add(this.cboSizeCurveGroup);
            this.Controls.Add(this.picBoxGroup);
            this.Controls.Add(this.picBoxCurve);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnNew);
            this.Controls.Add(this.btnSaveAs);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.lblSizeGroup);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.cboStoreAttribute);
            this.Controls.Add(this.lblStoreAttribute);
            this.Controls.Add(this.lblSizeCurve);
            this.Name = "frmSizeCurve";
            this.Text = "Store Size Curves";
            this.Load += new System.EventHandler(this.frmSizeCurve_Load);
            this.Controls.SetChildIndex(this.lblSizeCurve, 0);
            this.Controls.SetChildIndex(this.lblStoreAttribute, 0);
            this.Controls.SetChildIndex(this.cboStoreAttribute, 0);
            this.Controls.SetChildIndex(this.btnSave, 0);
            this.Controls.SetChildIndex(this.btnClose, 0);
            this.Controls.SetChildIndex(this.lblSizeGroup, 0);
            this.Controls.SetChildIndex(this.panel1, 0);
            this.Controls.SetChildIndex(this.btnSaveAs, 0);
            this.Controls.SetChildIndex(this.btnNew, 0);
            this.Controls.SetChildIndex(this.btnDelete, 0);
            this.Controls.SetChildIndex(this.picBoxCurve, 0);
            this.Controls.SetChildIndex(this.picBoxGroup, 0);
            this.Controls.SetChildIndex(this.cboSizeCurveGroup, 0);
            this.Controls.SetChildIndex(this.cboSizeGroup, 0);
            this.Controls.SetChildIndex(this.btnInUse, 0);
            ((System.ComponentModel.ISupportInitialize)(this.utmMain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ulgSizeCurve)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ulgDefaultSizeCurve)).EndInit();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picBoxCurve)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxGroup)).EndInit();
            this.ResumeLayout(false);

		}
		#endregion

		const string cTotalColName = "UniqueInternalTotalColumnName";
		const string cNewNamePrompt = "Enter New Name";
		const string cCopyCurrentLabel = "(Copy Current Curve)";
		const string cCreateNewLabel = "(Create New Curve)";
		const string cDefaultLabel = "(Default)";
		const int cCopyCurrentIndex = 1;
		const int cNewCurveIndex = 2;
		const int cDefaultIndex = 3;

		private bool _beginEdit;
		private bool _beginCopy;
		private bool _settingSizeCurveGroupCombo = false;
		private bool _settingSizeGroupCombo = false;
		private SortedList _sortedCurveList;
		private int _defaultStoreGroupIndex;
		private string _holdCurveName;
		private int _holdCurveKey;
		private ArrayList _sizeCurveMenuItemList;
		private SizeCurve _dlSizeCurve;
		private SizeGroup _dlSizeGroup;
		private DataTable _dtSets;
		private DataTable _dtStores;
		private DataTable _dtSizeCurve;
		private DataTable _dtDefaultSizeCurve;
		private DataTable _dtSizeCodeKeys;
		private DataSet _dsSizeCurve;
		private ProfileList _groupLevelList;
		private SortedList _primarySizeList;
		private SortedList _secondarySizeList;
		private bool _showSecondaryColumn;
		private int _newSizeCurveRID;
		private int _lastGoodSizeGroup;
		private ValueList _curveValList;
		private SizeCurveGroupProfile _currSizeCurveGrpProf;
//Begin Track #4041 - JScott - Long delay during cell update
		private bool _systemCellUpdate;
//End Track #4041 - JScott - Long delay during cell update
		private int _modelRID = Include.NoRID;				// BEGIN MID Track #4970 - add locking
        //private bool _newModel = false;
		private bool _modelLocked = false;
		private int _currModelIndex = -1;					// END MID Track #4970
        // Begin Track #4872 - JSmith - Global/User Attributes
        private FunctionSecurityProfile _storeUserAttrSecLvl;
        // End Track #4872

		public frmSizeCurve(SessionAddressBlock aSAB)
			: base(aSAB)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			FunctionSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AdminSizeCurves);
			DisplayPictureBoxImages();
			SetPictureBoxTags();
		}

		public void HandleExceptions(Exception exc)
		{
			Debug.WriteLine(exc.ToString());
			MessageBox.Show(exc.ToString());
		}

		override protected bool SaveChanges()
		{
			try
			{
				if (Save())
				{
					ErrorFound = false;
				}
				else
				{
					ErrorFound = true;
				}

				return true;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		override public void ISave()
		{
			try
			{
				Cursor = Cursors.WaitCursor;
				if (btnSave.Enabled)			// MID Track #4970 - add security check 
				{
					Save();
				}
			}		
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
			finally
			{
				Cursor = Cursors.Default;
			}
		}

		override public void ISaveAs()
		{
			try
			{
				Cursor = Cursors.WaitCursor;
				if (btnSaveAs.Enabled)			// MID Track #4970 - add security check 
				{
					SaveAs();
				}
			}		
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
			finally
			{
				Cursor = Cursors.Default;
			}
		}

		override public void IDelete()
		{
			try
			{
				Cursor = Cursors.WaitCursor;

				DeleteSizeCurveGroup();
			}		
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
			finally
			{
				Cursor = Cursors.Default;
			}
		}
		// BEGIN MID Track #4970 - modify to emulate other models 
		override protected void AfterClosing()
		{
			try
			{
				CheckForDequeue();
			}
			catch (Exception ex)
			{
				HandleException(ex, "SizeCurveMaint.AfterClosing");
			}
		}
		// END MID Track #4970  

		private void frmSizeCurve_Load(object sender, System.EventArgs e)
		{
			try
			{
                AddMenuItem(eMIDMenuItem.FileNew);
                EnableMenuItem(this, eMIDMenuItem.FileNew);
                EnableMenuItem(this, eMIDMenuItem.FileSave);
                EnableMenuItem(this, eMIDMenuItem.FileSaveAs);
                EnableMenuItem(this, eMIDMenuItem.EditDelete);

//				_secLvl = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AdminSizeCurves);
				_sizeCurveMenuItemList = new ArrayList();

				_beginEdit = false;
				_beginCopy = false;
				_newSizeCurveRID = -2;
				_lastGoodSizeGroup = -1;

//				if (_secLvl.AllowUpdate)
//				{
//					btnSave.Enabled = false;
//					btnSaveAs.Enabled = false;
//					btnDelete.Enabled = false;
//					btnNew.Enabled = false;
//				}

				cboSizeCurveGroup.Tag = "IgnoreMouseWheel";

				_dlSizeCurve = new SizeCurve();
				_dlSizeGroup = new SizeGroup();

				_sortedCurveList = new SortedList();

				_curveValList = ulgSizeCurve.DisplayLayout.ValueLists.Add("Curve Name");
				_curveValList.SortStyle = ValueListSortStyle.Ascending;

				LoadSizeCurveGroupComboBox();
				LoadStoreAttrComboBoxes();
				LoadSizeGroupComboBoxes();

				SetSizeCurveCombo();
                // Begin Track #4872 - JSmith - Global/User Attributes
                //cboStoreAttribute.SelectedIndex = _defaultStoreGroupIndex;
                //cboStoreAttribute.SelectedValue = Include.NoRID;
                cboStoreAttribute.SelectedValue = SAB.ClientServerSession.GlobalOptions.AllocationStoreGroupRID;
                // End Track #4872

//				FunctionSecurity = new FunctionSecurityProfile((int)eSecurityFunctions.AdminSizeCurves);
//				FunctionSecurity.SetFullControl();
//				AllowUpdate = true;  Security changes 1/24/05 vg
				
				// BEGIN MID Track #4970 - additional logic
				//SetReadOnly(FunctionSecurity.AllowUpdate);  //Security changes - 1/24/2005 vg
				//if (!FunctionSecurity.AllowDelete)
				//{
				//	btnDelete.Enabled = false;
				//}
				//cboStoreAttribute.Enabled = true;
				//cboSizeCurveGroup.Enabled = true;
				//cboSizeGroup.Enabled = true;

				SetText();
				// END MID Track #4970  

                //Begin Track #5858 - KJohnson - Validating store security only
                // Begin TT#44 - JSmith - Drag/Drop User Attributes or Filters in to Global Methods does not react consistantly
                //cboStoreAttribute.Tag = new MIDStoreAttributeComboBoxTag(SAB, cboStoreAttribute);
                cboStoreAttribute.Tag = new MIDStoreAttributeComboBoxTag(SAB, cboStoreAttribute, FunctionSecurity, false);
                // End TT#44
                //End Track #5858
				SetMaskedComboBoxesEnabled();

                AdjustTextWidthComboBox_DropDown(cboStoreAttribute);   //TT#7 - MD - RBeck _ Dynamic download 
                btnInUse.Enabled = false;   //TT#3185-M-VStuart-In Use does not work in all explorers-ANFUser
				ChangePending = false;
				FormLoaded = true;
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
		}

		// BEGIN MID Track #4970 - additional logic
		private void SetText()
		{
			try
			{
				this.lblSizeCurve.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_SizeCurve);
				this.lblStoreAttribute.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Store_Attribute);
				this.lblSizeGroup.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Size_Group);
				this.btnSave.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_Save);
				this.btnSaveAs.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_SaveAs);
				this.btnDelete.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_Delete);
				this.btnNew.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_New);
				this.btnClose.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_Close);
                this.btnInUse.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_In_Use);  //TT#110-MD-VStuart - In Use Tool
            }
			catch  
			{
				throw;
			}
		}
		// END MID Track #4970  

		private void cboSizeCurveGroup_SelectionChangeCommitted(object sender, System.EventArgs e)
		{
			try
			{
				if(base.FormLoaded)
				{
					Cursor = Cursors.WaitCursor;

					if (CheckForPendingChanges())
					{
						SetSizeCurveCombo();
						ChangePending = false;
					}
				}
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
			finally
			{
				Cursor = Cursors.Default;
			}
		}

		private void cboStoreAttribute_SelectionChangeCommitted(object sender, System.EventArgs e)
		{
			try
			{
				Cursor = Cursors.WaitCursor;

				SetStoreAttributeCombo();
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
			finally
			{
				Cursor = Cursors.Default;
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

		private void cboSizeGroup_SelectionChangeCommitted(object sender, System.EventArgs e)
		{
			try
			{
				if(base.FormLoaded)
				{
					Cursor = Cursors.WaitCursor;

					SetSizeGroupCombo();
				}
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
			finally
			{
				Cursor = Cursors.Default;
			}
		}

		private void btnClose_Click(object sender, System.EventArgs e)
		{
			try
			{
				this.Close();
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
		}

		private void btnNew_Click(object sender, System.EventArgs e)
		{
            INew();
		}

        override public void INew()
        {
            try
            {
                Cursor = Cursors.WaitCursor;

                if (CheckForPendingChanges())
                {
                    if (cboSizeCurveGroup.SelectedIndex != -1)
                    {
                        cboSizeCurveGroup.SelectedIndex = -1;
                    }
                    else
                    {
                        SetSizeCurveCombo();
                        ChangePending = false;
                    }
                }
            }
            catch (Exception exc)
            {
                HandleExceptions(exc);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

		private void btnSave_Click(object sender, System.EventArgs e)
		{
			try
			{
				Cursor = Cursors.WaitCursor;

				Save();
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
			finally
			{
				Cursor = Cursors.Default;
			}
		}

		private void btnSaveAs_Click(object sender, System.EventArgs e)
		{
			try
			{
				Cursor = Cursors.WaitCursor;

				SaveAs();
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
			finally
			{
				Cursor = Cursors.Default;
			}
		}

		private void btnDelete_Click(object sender, System.EventArgs e)
		{
			try
			{
				Cursor = Cursors.WaitCursor;

				DeleteSizeCurveGroup();
			}
			catch (DatabaseForeignKeyViolation)
			{
				MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_DeleteFailedDataInUse));
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
			finally
			{
				Cursor = Cursors.Default;
			}
		}

		private void ctmSizeCurve_Popup(object sender, System.EventArgs e)
		{
			try
			{
				mniExpand.Visible = false;
				mniCollapse.Visible = false;
				mniSetAllCurvesInSet.Visible = false;
				mniBalanceAllTo100.Visible = false;
				mniBalanceTo100.Visible = false;
				mniCopyCurveToDefault.Visible = false;

				switch (ulgSizeCurve.ActiveRow.Band.Index)
				{
					case 0 :

						if (_currSizeCurveGrpProf.SizeCurveList.Count > 0)
						{
							mniExpand.Visible = true;
							mniCollapse.Visible = true;
							mniSetAllCurvesInSet.Visible = true;
							mniBalanceAllTo100.Visible = true;
						}
						break;

					case 1 :

						if (ulgSizeCurve.ActiveRow.Cells["SizeCurveRID"].Value != System.DBNull.Value)
						{
							mniBalanceTo100.Visible = true;
							mniCopyCurveToDefault.Visible = true;
						}
						break;

					case 2 :

						mniBalanceTo100.Visible = true;
						mniCopyCurveToDefault.Visible = true;
						break;
				}
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
		}

		private void mniSizeCurve_Click(object sender, System.EventArgs e)
		{
			UltraGridRow currRow;
			int newKey;
			string newName;

			try
			{
				Cursor = Cursors.WaitCursor;

				if (ulgSizeCurve.ActiveRow.HasChild())
				{
					PreGridUpdate(ulgSizeCurve);
					newName = Convert.ToString(((MenuItem)sender).Text);
					newKey = Convert.ToInt32(_sortedCurveList[newName]);
					currRow = ulgSizeCurve.ActiveRow.GetChild(ChildRow.First);

					while (currRow != null)
					{
						switch (newKey)
						{
							case cDefaultIndex :

								StoreSizeCurveChanged(currRow.Cells["SizeCurveRID"], System.DBNull.Value, newName);
								break;

							default:

								StoreSizeCurveChanged(currRow.Cells["SizeCurveRID"], newKey, newName);
								break;
						}

						currRow = currRow.GetSibling(SiblingRow.Next, true, false);
					}

					PostGridUpdate(ulgSizeCurve, _dsSizeCurve);
					RecurseRowsAndHighlightTotals(ulgSizeCurve.ActiveRow.GetChild(ChildRow.First));

					ChangePending = true;
				}
			}
			catch (Exception exc)
			{
				ulgSizeCurve.ResumeRowSynchronization();
				ulgSizeCurve.EndUpdate();
				HandleExceptions(exc);
			}
			finally
			{
				Cursor = Cursors.Default;
			}
		}
		
		private void mniCollapse_Click(object sender, System.EventArgs e)
		{
			UltraGridRow currRow;

			try
			{
				Cursor = Cursors.WaitCursor;

				currRow = ulgSizeCurve.ActiveRow.GetChild(ChildRow.First);

				while (currRow != null)
				{
					currRow.CollapseAll();
					currRow = currRow.GetSibling(SiblingRow.Next, true, false);
				}
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
			finally
			{
				Cursor = Cursors.Default;
			}
		}

		private void mniExpand_Click(object sender, System.EventArgs e)
		{
			UltraGridRow currRow;

			try
			{
				Cursor = Cursors.WaitCursor;

				currRow = ulgSizeCurve.ActiveRow.GetChild(ChildRow.First);

				while (currRow != null)
				{
					currRow.ExpandAll();
					currRow = currRow.GetSibling(SiblingRow.Next, true, false);
				}
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
			finally
			{
				Cursor = Cursors.Default;
			}
		}

		private void mniBalanceAllTo100_Click(object sender, System.EventArgs e)
		{
			UltraGridRow currRow;

			try
			{
				Cursor = Cursors.WaitCursor;

				currRow = ulgSizeCurve.ActiveRow.GetChild(ChildRow.First);
				PreGridUpdate(ulgSizeCurve);
				
				while (currRow != null)
				{
//Begin Track #4041 - JScott - Long delay during cell update
//					grid_BalanceTo100(ulgSizeCurve, currRow, _dtSizeCurve);
					grid_BalanceTo100(ulgSizeCurve, currRow.ChildBands[0].Rows);
//End Track #4041 - JScott - Long delay during cell update
					currRow = currRow.GetSibling(SiblingRow.Next, true, false);
				}

				PostGridUpdate(ulgSizeCurve, _dtSizeCurve);
			}
			catch (Exception exc)
			{
				ulgSizeCurve.ResumeRowSynchronization();
				ulgSizeCurve.EndUpdate();
				HandleExceptions(exc);
			}
			finally
			{
				Cursor = Cursors.Default;
			}
		}

		private void mniCopyCurveToDefault_Click(object sender, System.EventArgs e)
		{
			SizeCurveProfile sizeCurveProf;

			try
			{
				Cursor = Cursors.WaitCursor;

				UnbindDefaultSizeCurveGrid();

				sizeCurveProf = (SizeCurveProfile)_currSizeCurveGrpProf.SizeCurveList.FindKey(Convert.ToInt32(ulgSizeCurve.ActiveRow.Cells["SizeCurveRID"].Value));
				_currSizeCurveGrpProf.DefaultSizeCurve.SizeCodeList.Clear();

				foreach (SizeCodeProfile sizeCodeProf in sizeCurveProf.SizeCodeList)
				{
					_currSizeCurveGrpProf.DefaultSizeCurve.SizeCodeList.Add((SizeCodeProfile)sizeCodeProf.Clone());
				}

				BuildDefaultSizeCurveTable();
				BindDefaultSizeCurveGrid();
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
			finally
			{
				Cursor = Cursors.Default;
			}
		}

		private void mniBalanceTo100_Click(object sender, System.EventArgs e)
		{
			try
			{
				Cursor = Cursors.WaitCursor;

				PreGridUpdate(ulgSizeCurve);
//Begin Track #4041 - JScott - Long delay during cell update
//				grid_BalanceTo100(ulgSizeCurve, ulgSizeCurve.ActiveRow, _dtSizeCurve);
				
				if (ulgSizeCurve.ActiveRow.Band.Index == 1)
				{
					grid_BalanceTo100(ulgSizeCurve, ulgSizeCurve.ActiveRow.ChildBands[0].Rows);
				}
				else
				{
					grid_BalanceTo100(ulgSizeCurve, ulgSizeCurve.ActiveRow.ParentCollection);
				}

//End Track #4041 - JScott - Long delay during cell update
				PostGridUpdate(ulgSizeCurve, _dtSizeCurve);
			}
			catch (Exception exc)
			{
				ulgSizeCurve.ResumeRowSynchronization();
				ulgSizeCurve.EndUpdate();
				HandleExceptions(exc);
			}
			finally
			{
				Cursor = Cursors.Default;
			}
		}

		private void mniBalanceDefaultTo100_Click(object sender, System.EventArgs e)
		{
			try
			{
				Cursor = Cursors.WaitCursor;

				PreGridUpdate(ulgDefaultSizeCurve);
//Begin Track #4041 - JScott - Long delay during cell update
//				grid_BalanceTo100(ulgDefaultSizeCurve, ulgDefaultSizeCurve.ActiveRow, _dtDefaultSizeCurve);
				grid_BalanceTo100(ulgDefaultSizeCurve, ulgDefaultSizeCurve.ActiveRow.ParentCollection);
//End Track #4041 - JScott - Long delay during cell update
				PostGridUpdate(ulgDefaultSizeCurve, _dtDefaultSizeCurve);
			}
			catch (Exception exc)
			{
				ulgDefaultSizeCurve.ResumeRowSynchronization();
				ulgDefaultSizeCurve.EndUpdate();
				HandleExceptions(exc);
			}
			finally
			{
				Cursor = Cursors.Default;
			}
		}
				
		private void ulgSizeCurve_BeforeCellListDropDown(object sender, Infragistics.Win.UltraWinGrid.CancelableCellEventArgs e)
		{
			try
			{
				if (e.Cell.Column.Key == "Curve Name")
				{
					if (cboSizeGroup.SelectedIndex == -1)
					{
						MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_SelectSizeGroupBeforeDefiningCurve), Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
						e.Cancel = true;
					}
				}

				if (e.Cell.Column.ValueList.SelectedItemIndex == -1)
				{
					if (Convert.ToString(e.Cell.Value) == cDefaultLabel)
					{
						_holdCurveKey = cDefaultIndex;
					}
					else
					{
						_holdCurveKey = Convert.ToInt32(e.Cell.Row.Cells["SizeCurveRID"].Value);
					}
				}
				else
				{
					_holdCurveKey = Convert.ToInt32(e.Cell.Column.ValueList.GetValue(e.Cell.Column.ValueList.SelectedItemIndex));
				}
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
		}

		private void ulgSizeCurve_AfterCellListCloseUp(object sender, Infragistics.Win.UltraWinGrid.CellEventArgs e)
		{
			int currKey;

			try
			{
				Cursor = Cursors.WaitCursor;

				if (ulgSizeCurve.ActiveCell != null)
				{
					if (e.Cell.Column.Key == "Curve Name")
					{
						if (e.Cell.Column.ValueList.SelectedItemIndex != -1)
						{
							currKey = Convert.ToInt32(e.Cell.Column.ValueList.GetValue(e.Cell.Column.ValueList.SelectedItemIndex));

							switch (currKey)
							{
								case cCopyCurrentIndex:

									_holdCurveName = Convert.ToString(e.Cell.Value);
									ulgSizeCurve.PerformAction(UltraGridAction.ExitEditMode);
									_beginCopy = true;
									e.Cell.Value = cNewNamePrompt;
									e.Cell.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Edit;
									ulgSizeCurve.PerformAction(UltraGridAction.EnterEditMode);
									break;

								case cNewCurveIndex :

									_holdCurveName = Convert.ToString(e.Cell.Value);
									ulgSizeCurve.PerformAction(UltraGridAction.ExitEditMode);
									_beginEdit = true;
									e.Cell.Value = cNewNamePrompt;
									e.Cell.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Edit;
									ulgSizeCurve.PerformAction(UltraGridAction.EnterEditMode);
									break;

								case cDefaultIndex :

									PreGridUpdate(ulgSizeCurve);
									StoreSizeCurveChanged(e.Cell, System.DBNull.Value, null);
									PostGridUpdate(ulgSizeCurve, _dsSizeCurve);
									RecurseRowsAndHighlightTotals(e.Cell.Row.GetChild(ChildRow.First));
									break;

								default:
								
									PreGridUpdate(ulgSizeCurve);
									StoreSizeCurveChanged(e.Cell, currKey, null);
									PostGridUpdate(ulgSizeCurve, _dsSizeCurve);
									RecurseRowsAndHighlightTotals(e.Cell.Row.GetChild(ChildRow.First));
									break;
							}

							ChangePending = true;
						}
					}
				}
			}
			catch (Exception exc)
			{
				ulgSizeCurve.ResumeRowSynchronization();
				ulgSizeCurve.EndUpdate();
				HandleExceptions(exc);
			}
			finally
			{
				Cursor = Cursors.Default;
			}
		}

		private void ulgSizeCurve_AfterExitEditMode(object sender, System.EventArgs e)
		{
			SizeCurveProfile newSizeCurveProf;

			try
			{
				Cursor = Cursors.WaitCursor;

				if (_beginEdit)
				{
					_beginEdit = false;

					ulgSizeCurve.ActiveCell.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;

					if (ulgSizeCurve.ActiveCell.Value != System.DBNull.Value &&
						Convert.ToString(ulgSizeCurve.ActiveCell.Value) != cNewNamePrompt &&
						Convert.ToString(ulgSizeCurve.ActiveCell.Value) != Convert.ToString(cCopyCurrentIndex) &&
						Convert.ToString(ulgSizeCurve.ActiveCell.Value) != Convert.ToString(cNewCurveIndex) &&
						Convert.ToString(ulgSizeCurve.ActiveCell.Value) != Convert.ToString(cDefaultIndex))
					{
						if (Convert.ToString(ulgSizeCurve.ActiveCell.Value) == ulgSizeCurve.ActiveCell.Text)
						{
							PreGridUpdate(ulgSizeCurve);

							CreateDefaultSecondaryRows(_dtSizeCurve, _newSizeCurveRID);

							newSizeCurveProf = new SizeCurveProfile(_newSizeCurveRID);
							newSizeCurveProf.SizeCurveName = Convert.ToString(ulgSizeCurve.ActiveCell.Text);
							_currSizeCurveGrpProf.SizeCurveList.Add(newSizeCurveProf);

							BuildCurveValueList();

							StoreSizeCurveChanged(ulgSizeCurve.ActiveCell, _newSizeCurveRID, null);
							PostGridUpdate(ulgSizeCurve, _dsSizeCurve);
							RecurseRowsAndHighlightTotals(ulgSizeCurve.ActiveRow.GetChild(ChildRow.First));

							_newSizeCurveRID--;
						}
						else
						{
							PreGridUpdate(ulgSizeCurve);
							StoreSizeCurveChanged(ulgSizeCurve.ActiveCell, Convert.ToInt32(ulgSizeCurve.ActiveCell.Value), null);
							PostGridUpdate(ulgSizeCurve, _dsSizeCurve);
							RecurseRowsAndHighlightTotals(ulgSizeCurve.ActiveRow.GetChild(ChildRow.First));
						}

						ChangePending = true;
					}
					else
					{
						PreGridUpdate(ulgSizeCurve);
						StoreSizeCurveChanged(ulgSizeCurve.ActiveCell, null, _holdCurveName);
						PostGridUpdate(ulgSizeCurve, _dsSizeCurve);
					}
				}
				else if (_beginCopy)
				{
					_beginCopy = false;

					ulgSizeCurve.ActiveCell.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;

					if (ulgSizeCurve.ActiveCell.Value != System.DBNull.Value &&
						Convert.ToString(ulgSizeCurve.ActiveCell.Value) != cNewNamePrompt &&
						Convert.ToString(ulgSizeCurve.ActiveCell.Value) != Convert.ToString(cCopyCurrentIndex) &&
						Convert.ToString(ulgSizeCurve.ActiveCell.Value) != Convert.ToString(cNewCurveIndex) &&
						Convert.ToString(ulgSizeCurve.ActiveCell.Value) != Convert.ToString(cDefaultIndex))
					{
						if (Convert.ToString(ulgSizeCurve.ActiveCell.Value) == ulgSizeCurve.ActiveCell.Text)
						{
							PreGridUpdate(ulgSizeCurve);

							switch (_holdCurveKey)
							{
								case cDefaultIndex :
									CopySecondaryRows(_dtSizeCurve, _newSizeCurveRID, _dtDefaultSizeCurve, _currSizeCurveGrpProf.DefaultSizeCurve.Key);
									newSizeCurveProf = (SizeCurveProfile)_currSizeCurveGrpProf.DefaultSizeCurve.Clone();
									break;

								default:
									CopySecondaryRows(_dtSizeCurve, _newSizeCurveRID, _dtSizeCurve, _holdCurveKey);
									newSizeCurveProf = (SizeCurveProfile)((SizeCurveProfile)_currSizeCurveGrpProf.SizeCurveList.FindKey(_holdCurveKey)).Clone();
									break;
							}

							newSizeCurveProf.Key = _newSizeCurveRID;
							newSizeCurveProf.SizeCurveName = Convert.ToString(ulgSizeCurve.ActiveCell.Text);
							_currSizeCurveGrpProf.SizeCurveList.Add(newSizeCurveProf);

							BuildCurveValueList();

							StoreSizeCurveChanged(ulgSizeCurve.ActiveCell, _newSizeCurveRID, null);
							PostGridUpdate(ulgSizeCurve, _dsSizeCurve);
							RecurseRowsAndHighlightTotals(ulgSizeCurve.ActiveRow.GetChild(ChildRow.First));

							_newSizeCurveRID--;
						}
						else
						{
							PreGridUpdate(ulgSizeCurve);
							StoreSizeCurveChanged(ulgSizeCurve.ActiveCell, Convert.ToInt32(ulgSizeCurve.ActiveCell.Value), null);
							PostGridUpdate(ulgSizeCurve, _dsSizeCurve);
							RecurseRowsAndHighlightTotals(ulgSizeCurve.ActiveRow.GetChild(ChildRow.First));
						}

						ChangePending = true;
					}
					else
					{
						PreGridUpdate(ulgSizeCurve);
						StoreSizeCurveChanged(ulgSizeCurve.ActiveCell, null, _holdCurveName);
						PostGridUpdate(ulgSizeCurve, _dsSizeCurve);
					}
				}
			}
			catch (Exception exc)
			{
				ulgSizeCurve.ResumeRowSynchronization();
				ulgSizeCurve.EndUpdate();
				HandleExceptions(exc);
			}
			finally
			{
				Cursor = Cursors.Default;
			}
		}
		
		private void ulgSizeCurve_BeforeEnterEditMode(object sender, System.ComponentModel.CancelEventArgs e)
		{
			try
			{
				if (((UltraGrid)sender).ActiveCell.Band.Index == 2)
				{
					grid_BeforeEnterEditMode(sender, e);
				}
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
		}

		private void ulgSizeCurve_AfterCellUpdate(object sender, Infragistics.Win.UltraWinGrid.CellEventArgs e)
		{
			try
			{
//Begin Track #4041 - JScott - Long delay during cell update
//				if (e.Cell.Band.Index == 2)
//				{
//					if (e.Cell.Value == DBNull.Value)
//					{
//						e.Cell.Value = 0;
//					}
//
//					grid_AfterCellUpdate((UltraGrid)sender, _dtSizeCurve, e);
//				}
				if (!_systemCellUpdate)
				{
					if (e.Cell.Band.Index == 2)
					{
						if (e.Cell.Value == DBNull.Value)
						{
							e.Cell.Value = 0;
						}

						grid_AfterCellUpdate((UltraGrid)sender, _dtSizeCurve, e);
					}
				}
//End Track #4041 - JScott - Long delay during cell update
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
		}

		private void ulgSizeCurve_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			try
			{
				grid_MouseDown(ulgSizeCurve, ctmSizeCurve, e);
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
		}
		
		private void ulgDefaultSizeCurve_BeforeEnterEditMode(object sender, System.ComponentModel.CancelEventArgs e)
		{
			try
			{
				grid_BeforeEnterEditMode(sender, e);
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
		}

		private void ulgDefaultSizeCurve_AfterCellUpdate(object sender, Infragistics.Win.UltraWinGrid.CellEventArgs e)
		{
			try
			{
//Begin Track #4041 - JScott - Long delay during cell update
//				if (e.Cell.Value == DBNull.Value)
//				{
//					e.Cell.Value = 0;
//				}
//
//				grid_AfterCellUpdate((UltraGrid)sender, _dtDefaultSizeCurve, e);
				if (!_systemCellUpdate)
				{
					if (e.Cell.Value == DBNull.Value)
					{
						e.Cell.Value = 0;
					}

					grid_AfterCellUpdate((UltraGrid)sender, _dtDefaultSizeCurve, e);
				}
//End Track #4041 - JScott - Long delay during cell update
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
		}

		private void ulgDefaultSizeCurve_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			try
			{
				grid_MouseDown(ulgDefaultSizeCurve, ctmDefaultSizeCurve, e);
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
		}

		private void grid_BeforeEnterEditMode(object sender, System.ComponentModel.CancelEventArgs e)
		{
			DataRow currRow;

			try
			{
				if (((UltraGrid)sender).ActiveCell.Column.Key != cTotalColName && !Convert.ToBoolean(((UltraGrid)sender).ActiveCell.Row.Cells["isTotalRow"].Value))
				{
					currRow = _dtSizeCodeKeys.Rows.Find(Convert.ToString(((UltraGrid)sender).ActiveCell.Row.Cells["Secondary"].Value));

					if (currRow == null || currRow[((UltraGrid)sender).ActiveCell.Column.Key] == System.DBNull.Value)
					{
						e.Cancel = true;
					}
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

//Begin Track #4041 - JScott - Long delay during cell update
//		private void grid_AfterCellUpdate(UltraGrid aGrid, DataTable aDataTable, Infragistics.Win.UltraWinGrid.CellEventArgs e)
//		{
//			try
//			{
//				PreGridUpdate(aGrid);
//				
//				if (e.Cell.Column.Key != cTotalColName && !Convert.ToBoolean(e.Cell.Row.Cells["isTotalRow"].Value))
//				{
//					UpdateCellValue(Convert.ToInt32(e.Cell.Row.Cells["SizeCurveRID"].Value), e.Cell.Column.Key, Convert.ToString(e.Cell.Row.Cells["Secondary"].Value), (double)(decimal)Convert.ToDouble(e.Cell.Value));
//				}
//				else if (e.Cell.Column.Key == cTotalColName && !Convert.ToBoolean(e.Cell.Row.Cells["isTotalRow"].Value))
//				{
//					SpreadTotalToPrimary(aDataTable, Convert.ToInt32(e.Cell.Row.Cells["SizeCurveRID"].Value), Convert.ToString(e.Cell.Row.Cells["Secondary"].Value), false, (double)(decimal)Convert.ToDouble(e.Cell.Value));
//				}
//				else if (e.Cell.Column.Key != cTotalColName && Convert.ToBoolean(e.Cell.Row.Cells["isTotalRow"].Value))
//				{
//					SpreadTotalToSecondary(aDataTable, Convert.ToInt32(e.Cell.Row.Cells["SizeCurveRID"].Value), e.Cell.Column.Key, (double)(decimal)Convert.ToDouble(e.Cell.Value));
//				}
//				else if (e.Cell.Column.Key == cTotalColName && Convert.ToBoolean(e.Cell.Row.Cells["isTotalRow"].Value))
//				{
//					SpreadTotalToAll(aDataTable, Convert.ToInt32(e.Cell.Row.Cells["SizeCurveRID"].Value), (double)(decimal)Convert.ToDouble(e.Cell.Value));
//				}
//
//				SumSizeCurvePercentages(aDataTable, Convert.ToInt32(e.Cell.Row.Cells["SizeCurveRID"].Value));
//				PostGridUpdate(aGrid, aDataTable);
//		
//				ChangePending = true;
//			}
//			catch (Exception exc)
//			{
//				string message = exc.ToString();
//				aGrid.ResumeRowSynchronization();
//				aGrid.EndUpdate();
//				throw;
//			}
//		}
//
//		private void grid_BalanceTo100(UltraGrid aGrid, UltraGridRow aGridRow, DataTable aDataTable)
//		{
//			DataRow currRow;
//			
//			try
//			{
//				if (aGridRow.Cells["SizeCurveRID"].Value != System.DBNull.Value)
//				{
//					if (_secondarySizeList.Count > 1)
//					{
//						currRow = aDataTable.Rows.Find(new object[] { Convert.ToInt32(aGridRow.Cells["SizeCurveRID"].Value), true, "Total" });
//					}
//					else
//					{
//						currRow = aDataTable.Rows.Find(new object[] { Convert.ToInt32(aGridRow.Cells["SizeCurveRID"].Value), false, ((SizeCodeProfile)_secondarySizeList.GetByIndex(0)).SizeCodeSecondary });
//					}
//
//					if ((decimal)Convert.ToDouble(currRow[cTotalColName]) != 100)
//					{
//						SpreadTotalToAll(aDataTable, Convert.ToInt32(currRow["SizeCurveRID"]), 100);
//						SumSizeCurvePercentages(aDataTable, Convert.ToInt32(currRow["SizeCurveRID"]));
//						ChangePending = true;
//					}
//				}
//			}
//			catch (Exception exc)
//			{
//				string message = exc.ToString();
//				throw;
//			}
//		}
		private void grid_AfterCellUpdate(UltraGrid aGrid, DataTable aDataTable, Infragistics.Win.UltraWinGrid.CellEventArgs e)
		{
			UltraGridRow activeRow;
			UltraGridCell activeCell;

			try
			{
				aGrid.BeginUpdate();

				if (e.Cell.Column.Key != cTotalColName && !Convert.ToBoolean(e.Cell.Row.Cells["isTotalRow"].Value))
				{
					UpdateCellValue(Convert.ToInt32(e.Cell.Row.Cells["SizeCurveRID"].Value), e.Cell.Column.Key, Convert.ToString(e.Cell.Row.Cells["Secondary"].Value), (double)(decimal)Convert.ToDouble(e.Cell.Value));
					SumCellSizeCurvePercentages(e.Cell);
				}
				else if (e.Cell.Column.Key == cTotalColName && !Convert.ToBoolean(e.Cell.Row.Cells["isTotalRow"].Value))
				{
					SpreadTotalToPrimary(e.Cell);
					SumRowSizeCurvePercentages(e.Cell);
				}
				else if (e.Cell.Column.Key != cTotalColName && Convert.ToBoolean(e.Cell.Row.Cells["isTotalRow"].Value))
				{
					SpreadTotalToSecondary(e.Cell);
					SumColumnSizeCurvePercentages(e.Cell);
				}
				else if (e.Cell.Column.Key == cTotalColName && Convert.ToBoolean(e.Cell.Row.Cells["isTotalRow"].Value))
				{
					SpreadTotalToAll(e.Cell);
					SumAllSizeCurvePercentages(e.Cell);
				}

				activeRow = aGrid.ActiveRow;
				activeCell = aGrid.ActiveCell;
				aGrid.ActiveRow = null;
				aGrid.ActiveCell = null;
				aGrid.ActiveRow = activeRow;
				aGrid.ActiveCell = activeCell;
				aGrid.EndUpdate();
				aGrid.UpdateData();

				ChangePending = true;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				aGrid.ResumeRowSynchronization();
				aGrid.EndUpdate();
				throw;
			}
		}

		private void grid_BalanceTo100(UltraGrid aGrid, RowsCollection aRows)
		{
			try
			{
				foreach (UltraGridRow row in aRows)
				{
					if (!Convert.ToBoolean(row.Cells["isTotalRow"].Value))
					{
						if ((decimal)Convert.ToDouble(row.Cells[cTotalColName].Value) != 100)
						{
							SetCellValue(row.Cells[cTotalColName], (double)100);
							SpreadTotalToAll(row.Cells[cTotalColName]);
							SumAllSizeCurvePercentages(row.Cells[cTotalColName]);
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
//End Track #4041 - JScott - Long delay during cell update

		private void grid_MouseDown(UltraGrid aGrid, ContextMenu aMenu, MouseEventArgs e)
		{
			Point point;
			UIElement mouseUIElement;
			UIElement rowUIElement;
			UltraGridRow mouseRow;

			try
			{
				point = new Point(e.X, e.Y);

				if (e.Button == MouseButtons.Right)
				{
					mouseUIElement = aGrid.DisplayLayout.UIElement.ElementFromPoint(new Point(e.X, e.Y));

					if (mouseUIElement != null)
					{
						rowUIElement = mouseUIElement.GetAncestor(typeof(RowUIElement));

						if (rowUIElement != null)
						{
							mouseRow = (UltraGridRow)rowUIElement.SelectableItem;
							aGrid.ActiveRow = mouseRow;
							aMenu.Show(aGrid, point);
						}
					}
				}
			}
			catch( Exception exception )
			{
				HandleException(exception);
			}
		}

		private void SetSizeCurveCombo()
		{
			SizeCurveGroupProfile sizeCurveGroupProf;
	
			try
			{
				_settingSizeCurveGroupCombo = true;
				
				try
				{
					UnbindSizeCurveGrid();
					UnbindDefaultSizeCurveGrid();

					// BEGIN MID Track #4970 - add locking
					bool allowUpdate = FunctionSecurity.AllowUpdate;	
					eDataState dataState;								
					CheckForDequeue();
					// END MID Track #4970
					
					if (cboSizeCurveGroup.SelectedIndex >= 0)
					{	// BEGIN MID Track #4970 - add locking 
						//sizeCurveGroupProf = new SizeCurveGroupProfile(((ComboObject)cboSizeCurveGroup.SelectedItem).Key);
						ComboObject comboObj_SC = (ComboObject)cboSizeCurveGroup.SelectedItem;	
						if (allowUpdate)
						{
							sizeCurveGroupProf = (SizeCurveGroupProfile)SAB.HierarchyServerSession.GetModelDataForUpdate(eModelType.SizeCurve, comboObj_SC.Key, true);
							if (sizeCurveGroupProf.ModelLockStatus == eLockStatus.ReadOnly)
							{
								allowUpdate = false;
							}
							else if (sizeCurveGroupProf.ModelLockStatus == eLockStatus.Cancel)
							{
								this.cboSizeCurveGroup.SelectedIndex = _currModelIndex;
								return;
							}
						}
						else
						{
							sizeCurveGroupProf = SAB.HierarchyServerSession.GetSizeCurveGrouplData(comboObj_SC.Key);
						}
						_currModelIndex = this.cboSizeCurveGroup.SelectedIndex;
						if (!allowUpdate)
						{
							_modelLocked = false;
						}
						else
						{
							_modelLocked = true;
						}
						_modelRID = sizeCurveGroupProf.Key;

						if (sizeCurveGroupProf.DefinedSizeGroupRID != Include.NoRID)
						{
							if (CheckForValidSizeGroup(sizeCurveGroupProf.DefinedSizeGroupRID, sizeCurveGroupProf))
							{
								foreach (ComboObject comboObj in cboSizeGroup.Items)
								{
									if (comboObj.Key == sizeCurveGroupProf.DefinedSizeGroupRID)
									{
										cboSizeGroup.SelectedItem = comboObj;
									}
								}
							}
							else
							{
								MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_RemovingInvalidSizeGroupFromCurve), Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
								sizeCurveGroupProf.DefinedSizeGroupRID = Include.NoRID;
								cboSizeGroup.SelectedIndex = -1;
							}
						}
						else
						{
							cboSizeGroup.SelectedIndex = -1;
						}
					}
					else
					{
						sizeCurveGroupProf = new SizeCurveGroupProfile(Include.NoRID);
						sizeCurveGroupProf.DefaultSizeCurve = new SizeCurveProfile(Include.NoRID);
						sizeCurveGroupProf.DefaultSizeCurve.SizeCurveName = Include.DefaultSizeCurveName;

						if (cboSizeGroup.SelectedIndex >= 0)
						{
							sizeCurveGroupProf.DefinedSizeGroupRID = ((ComboObject)cboSizeGroup.SelectedItem).Key;
						}
						else
						{
							sizeCurveGroupProf.DefinedSizeGroupRID = Include.NoRID;
						}
					}

					BuildSizeCurveTables(sizeCurveGroupProf);

					if (_groupLevelList != null)
					{
						BuildStoreAttributeTables(_groupLevelList);
						BindSizeCurveGrid();
						BindDefaultSizeCurveGrid();
					}
					if (allowUpdate)
					{
						dataState = eDataState.Updatable;
					}
					else
					{
						dataState = eDataState.ReadOnly;
					}
					Format_Title(dataState, eMIDTextCode.frm_SizeCurves, null); 
					DetermineControlsEnabled(allowUpdate);
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
				finally
				{
					_settingSizeCurveGroupCombo = false;
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
		// BEGIN MID Track #4970 - emulate other models 
		private void CheckForDequeue()
		{
			try
			{
				if (_modelLocked)
				{
					SAB.HierarchyServerSession.DequeueModel(eModelType.SizeCurve, _modelRID);
					_modelLocked = false;
				}
			}
			catch(Exception exception)
			{
				HandleException(exception);
			}
		}

		private void DetermineControlsEnabled(bool allowUpdate)
		{
			try
			{
				SetReadOnly(allowUpdate);
                btnInUse.Enabled = true;  //TT#3185-M-VStuart-In Use does not work in all explorers-ANFUser 
				 
				if (!allowUpdate)
				{
					if (FunctionSecurity.AllowUpdate)
					{
						btnNew.Enabled = true;			// row is enqueued, so New is okay
					} 
					cboSizeCurveGroup.Enabled = true;
					cboSizeGroup.Enabled = true;
					cboStoreAttribute.Enabled = true;
					mniBalanceDefaultTo100.Enabled = false;
					mniSetAllCurvesInSet.Enabled = false;
					mniBalanceAllTo100.Enabled = false;
					mniBalanceTo100.Enabled = false;
					mniCopyCurveToDefault.Enabled = false;
				}
				else 
				{
                    //BEGIN TT#3185-M-VStuart-In Use does not work in all explorers-ANFUser
                    if (FunctionSecurity.AllowDelete && cboSizeCurveGroup.SelectedIndex > -1)
					{
						btnDelete.Enabled = true;
                        //btnInUse.Enabled = true;    //TT#110-MD-VStuart - In Use Tool
					}
					else
					{
						btnDelete.Enabled = false;
                        //btnInUse.Enabled = false;    //TT#110-MD-VStuart - In Use Tool
					}
                    //END TT#3185-M-VStuart-In Use does not work in all explorers-ANFUser
                    mniBalanceDefaultTo100.Enabled = true;
					mniSetAllCurvesInSet.Enabled = true;
					mniBalanceAllTo100.Enabled = true;
					mniBalanceTo100.Enabled = true;
					mniCopyCurveToDefault.Enabled = true;
				}
			}
			catch
			{
				throw;
			}
		}	
		// END MID Track #4970

		private void SetStoreAttributeCombo()
		{
			ProfileList grpLevelList;

			try
			{
                // Begin Track #4872 - JSmith - Global/User Attributes
                //grpLevelList = SAB.StoreServerSession.GetStoreGroupLevelListViewList(((ComboObject)cboStoreAttribute.SelectedItem).Key, true);
                grpLevelList = StoreMgmt.StoreGroup_GetLevelListViewList(Convert.ToInt32(cboStoreAttribute.SelectedValue), true); //SAB.StoreServerSession.GetStoreGroupLevelListViewList(Convert.ToInt32(cboStoreAttribute.SelectedValue), true);
                // End Track #4872

				UnbindSizeCurveGrid();
				UnbindDefaultSizeCurveGrid();

				BuildStoreAttributeTables(grpLevelList);

				if (_currSizeCurveGrpProf != null)
				{
					BuildSizeCurveTables(_currSizeCurveGrpProf);
					BindSizeCurveGrid();
					BindDefaultSizeCurveGrid();
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		private void SetSizeGroupCombo()
		{
			try
			{
				if (!_settingSizeCurveGroupCombo && !_settingSizeGroupCombo)
				{
					_settingSizeGroupCombo = true;

					try
					{
						if (cboSizeGroup.SelectedIndex >= 0 && ((ComboObject)cboSizeGroup.SelectedItem).Key == -1)
						{
							cboSizeGroup.SelectedIndex = -1;
						}
					
						if (_currSizeCurveGrpProf != null && _groupLevelList != null)
						{
							if (cboSizeGroup.SelectedIndex >= 0)
							{
								if (CheckForValidSizeGroup(((ComboObject)cboSizeGroup.SelectedItem).Key, _currSizeCurveGrpProf))
								{
									_currSizeCurveGrpProf.DefinedSizeGroupRID = ((ComboObject)cboSizeGroup.SelectedItem).Key;
								}
								else
								{
									MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_CurveSizeCodeNotOnSizeGroup), Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
									cboSizeGroup.SelectedIndex = _lastGoodSizeGroup;
									return;
								}
							}
							else
							{
								_currSizeCurveGrpProf.DefinedSizeGroupRID = Include.NoRID;
							}

							UnbindSizeCurveGrid();
							UnbindDefaultSizeCurveGrid();
							BuildSizeCurveTables(_currSizeCurveGrpProf);
							BuildStoreAttributeTables(_groupLevelList);
							BindSizeCurveGrid();
							BindDefaultSizeCurveGrid();
							ChangePending = true;		// // MID Track #4970 - move from below
						}
					}
					catch (Exception exc)
					{
						string message = exc.ToString();
						throw;
					}
					finally
					{
						_settingSizeGroupCombo = false;
						//ChangePending = true;			// MID Track #4970 - move to above; shoudn't change if message displayed 
					}
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
			finally
			{
				_lastGoodSizeGroup = cboSizeGroup.SelectedIndex;
			}
		}

		private bool CheckForValidSizeGroup(int aSizeGroupRID, SizeCurveGroupProfile aSizeCurveGroupProf)
		{
			SizeGroupProfile sizeGroupProf;

			try
			{
				if (aSizeGroupRID != Include.NoRID)
				{
					sizeGroupProf = new SizeGroupProfile(aSizeGroupRID);

					foreach (SizeCurveProfile sizeCurveProf in aSizeCurveGroupProf.SizeCurveList)
					{
						foreach (SizeCodeProfile sizeCodeProf in sizeCurveProf.SizeCodeList)
						{
							if (!sizeGroupProf.SizeCodeList.Contains(sizeCodeProf.Key))
							{
								return false;
							}
						}
					}

					foreach (SizeCodeProfile sizeCodeProf in aSizeCurveGroupProf.DefaultSizeCurve.SizeCodeList)
					{
						if (!sizeGroupProf.SizeCodeList.Contains(sizeCodeProf.Key))
						{
							return false;
						}
					}
				}

				return true;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		private void LoadSizeCurveGroupComboBox()
		{
			DataTable dtSizeCurveGroups;

			try
			{
				dtSizeCurveGroups = _dlSizeCurve.GetSizeCurveGroups();
				cboSizeCurveGroup.Items.Clear();

				foreach (DataRow row in dtSizeCurveGroups.Rows)
				{
					cboSizeCurveGroup.Items.Add(new ComboObject(Convert.ToInt32(row["SIZE_CURVE_GROUP_RID"]), Convert.ToString(row["SIZE_CURVE_GROUP_NAME"])));
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		// BEGIN MID Track #4396 - Justin Bolles - Size Dropdown Filter
		private void BindSizeCurveComboBox(bool includeEmptySelection, string aFilterString, bool aCaseSensitive)
		{
			try
			{
				base.FormLoaded = false;
				DataTable dtSizeCurve = MIDEnvironment.CreateDataTable();
				SizeCurve objSizeCurve = new SizeCurve();
								
				// RowFilter didn't work with multiple wild cards 
				aFilterString = aFilterString.Replace("*","%");
                //aFilterString = aFilterString.Replace("'","''");	// for string with single quote
			
                //string whereClause = "SIZE_CURVE_GROUP_NAME LIKE " +  "'" + aFilterString + "'";
                //if (!aCaseSensitive)
                //{
                //    whereClause += Include.CaseInsensitiveCollation;
                //}
				
				// BEGIN MID Track #4630 - Size Curves not sorted after filtering 
				//whereClause += " ORDER BY SIZE_CURVE_GROUP_NAME";
				// END MID Track #4630
				
				//dtSizeCurve = objSizeCurve.GetFilteredSizeCurveGroups(whereClause);
                if (aCaseSensitive)
                {
                    dtSizeCurve = objSizeCurve.GetFilteredSizeCurveGroupsCaseSensitive(aFilterString);
                }
                else
                {
                    dtSizeCurve = objSizeCurve.GetFilteredSizeCurveGroupsCaseInsensitive(aFilterString);
                }

				DataView dvSizeCurve = new DataView(dtSizeCurve, "", "SIZE_CURVE_GROUP_NAME", DataViewRowState.CurrentRows);
				cboSizeCurveGroup.Items.Clear();

				foreach (DataRow row in dtSizeCurve.Rows)
				{
					cboSizeCurveGroup.Items.Add(new ComboObject(Convert.ToInt32(row["SIZE_CURVE_GROUP_RID"]), Convert.ToString(row["SIZE_CURVE_GROUP_NAME"])));
				}

				if (includeEmptySelection)
				{							
					dtSizeCurve.Rows.Add(new object[] { Include.NoRID, ""} );
				}

				cboSizeCurveGroup.Enabled = true;
				base.FormLoaded = true;
			}		
			catch (Exception ex)
			{
				HandleException(ex, "BindSizeCurveComboBox");
			}
		}
		// END MID Track #4396

		private void LoadStoreAttrComboBoxes()
		{
			ProfileList strGrpProfList;
			int index;
            // Begin Track #4872 - JSmith - Global/User Attributes
            eStoreGroupSelectType storeGroupSelectType;
            // End Track #4872

			try
			{
                // Begin Track #4872 - JSmith - Global/User Attributes
                //strGrpProfList = SAB.StoreServerSession.GetStoreGroupListViewList();
                _storeUserAttrSecLvl = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AdminStoreAttributesUser);
                if (_storeUserAttrSecLvl.AccessDenied)
                {
                    storeGroupSelectType = eStoreGroupSelectType.GlobalOnly;
                }
                else
                {
                    storeGroupSelectType = eStoreGroupSelectType.All;
                }
                strGrpProfList = StoreMgmt.StoreGroup_GetListViewList(storeGroupSelectType, storeGroupSelectType != eStoreGroupSelectType.GlobalOnly); //SAB.StoreServerSession.GetStoreGroupListViewList(storeGroupSelectType, storeGroupSelectType != eStoreGroupSelectType.GlobalOnly);
                
                //cboStoreAttribute.Items.Clear();

                //_defaultStoreGroupIndex = -1;

                //foreach (StoreGroupListViewProfile strGrpLstVwProf in strGrpProfList)
                //{
                //    index = cboStoreAttribute.Items.Add(new ComboObject(strGrpLstVwProf.Key, strGrpLstVwProf.Name));

                //    if (strGrpLstVwProf.Key == SAB.ClientServerSession.GlobalOptions.AllocationStoreGroupRID)
                //    {
                //        _defaultStoreGroupIndex = index;
                //    }
                //}
                cboStoreAttribute.Initialize(SAB, FunctionSecurity, strGrpProfList.ArrayList, true);
                // End Track #4872
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		private void LoadSizeGroupComboBoxes()
		{
			DataTable dtSizeGroups;

			try
			{
				dtSizeGroups = _dlSizeGroup.GetSizeGroups(false);
				cboSizeGroup.Items.Clear();

				cboSizeGroup.Items.Add(new ComboObject(-1, "(None)"));

				foreach (DataRow row in dtSizeGroups.Rows)
				{
					cboSizeGroup.Items.Add(new ComboObject(Convert.ToInt32(row["SIZE_GROUP_RID"]), Convert.ToString(row["SIZE_GROUP_NAME"])));
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		// BEGIN MID Track #4396 - Justin Bolles - Size Dropdown Filter
		private void BindSizeGroupComboBox(bool includeEmptySelection, string aFilterString, bool aCaseSensitive)
		{
			try
			{
				base.FormLoaded = false;
				DataTable dtGroups = MIDEnvironment.CreateDataTable("Groups");
				// Originally tried RowFilter but received invalid rowfilter msg when multiple wild cards
			
				SizeGroup sizeGroup = new SizeGroup();
				aFilterString = aFilterString.Replace("*","%");
				//aFilterString = aFilterString.Replace("'","''");	// for string with single quote
			
				//string whereClause = "SIZE_GROUP_NAME LIKE '" + aFilterString + "'";
                //if (!aCaseSensitive)
                //{
                //    whereClause += Include.CaseInsensitiveCollation;
                //}
                //dtGroups = sizeGroup.SizeGroup_FilterRead(whereClause);
                if (aCaseSensitive)
                {
                    dtGroups = sizeGroup.SizeGroup_FilterReadCaseSensitive(aFilterString);
                }
                else
                {
                    dtGroups = sizeGroup.SizeGroup_FilterReadCaseInsensitive(aFilterString);
                }

				dtGroups.DefaultView.Sort = "SIZE_GROUP_NAME ASC"; 
				dtGroups.AcceptChanges();
				
				cboSizeGroup.Items.Clear();

				cboSizeGroup.Items.Add(new ComboObject(-1, "(None)"));

				foreach (DataRow row in dtGroups.Rows)
				{
					cboSizeGroup.Items.Add(new ComboObject(Convert.ToInt32(row["SIZE_GROUP_RID"]), Convert.ToString(row["SIZE_GROUP_NAME"])));
				}

				cboSizeGroup.Enabled = true;
				base.FormLoaded = true;
			}
			catch (Exception ex)
			{
				HandleException(ex, "BindSizeGroupComboBox");
			}
		}

		// END MID Track #4396
		
		private void UnbindSizeCurveGrid()
		{
			try
			{
// (CSMITH) - BEG MID - Changes per JScott
//				ulgSizeCurve.DataSource = null;
				if (ulgSizeCurve.DataSource != null)
				{
					ulgSizeCurve.DisplayLayout.Bands[1].Columns["Curve Name"].ValueList = null;
					ulgSizeCurve.DataSource = null;
				}
// (CSMITH) - END MID - Changes per JScott

				if (_dsSizeCurve != null)
				{
					_dsSizeCurve.Relations.Clear();
					_dsSizeCurve.Tables.Clear();
					_dsSizeCurve.AcceptChanges();
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		private void UnbindDefaultSizeCurveGrid()
		{
			try
			{
				ulgDefaultSizeCurve.DataSource = null;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		private void BuildSizeCurveTables(SizeCurveGroupProfile aSizeCurveGroupProf)
		{
			SizeGroupProfile sizeGroupProf = null;

			try
			{
				_currSizeCurveGrpProf = aSizeCurveGroupProf;

				// Clear existing dataset and create new datatables

				_dtSizeCodeKeys = MIDEnvironment.CreateDataTable("SizeCodeKeys");
				_dtSizeCodeKeys.Columns.Add("Secondary", typeof(string));
				_dtSizeCodeKeys.PrimaryKey = new DataColumn[] { _dtSizeCodeKeys.Columns["Secondary"] };

				// Walk through size curves and find all primary and secondary sizes

				_primarySizeList = new SortedList();
				_secondarySizeList = new SortedList();
				_showSecondaryColumn = false;

				if (_currSizeCurveGrpProf.DefinedSizeGroupRID != Include.NoRID)
				{
					sizeGroupProf = new SizeGroupProfile(_currSizeCurveGrpProf.DefinedSizeGroupRID);
					AddSizeCodeToTable(sizeGroupProf.SizeCodeList);
				}
				else
				{
					_currSizeCurveGrpProf.Resequence();

					foreach (SizeCurveProfile szCrvProf in _currSizeCurveGrpProf.SizeCurveList)
					{
						AddSizeCodeToTable(szCrvProf.SizeCodeList);
					}

					if (_currSizeCurveGrpProf.DefaultSizeCurve != null)
					{
						AddSizeCodeToTable(_currSizeCurveGrpProf.DefaultSizeCurve.SizeCodeList);
					}
				}

				// Build detail tables

				BuildSizeCurveTable();
				BuildDefaultSizeCurveTable();
				BuildCurveValueList();
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		private void BuildSizeCurveTable()
		{
			IDictionaryEnumerator dictEnum;
			DataRow currRow;
			SizeCodeProfile sizeCodeProf;

			try
			{
				_dtSizeCurve = MIDEnvironment.CreateDataTable("SizeCurve");
				_dtSizeCurve.Columns.Add("SizeCurveRID", typeof(int));
				_dtSizeCurve.Columns.Add("isTotalRow", typeof(bool));
				_dtSizeCurve.Columns.Add("hasSecondarySizes", typeof(bool));
				_dtSizeCurve.Columns.Add("Secondary", typeof(string));
				_dtSizeCurve.PrimaryKey = new DataColumn[] { _dtSizeCurve.Columns["SizeCurveRID"], _dtSizeCurve.Columns["isTotalRow"], _dtSizeCurve.Columns["Secondary"] };

				// Create Primary Size Columns

				if (_primarySizeList.Count > 0)
				{
					_dtSizeCurve.Columns.Add(cTotalColName, typeof(double));

					dictEnum = _primarySizeList.GetEnumerator();

					while (dictEnum.MoveNext())
					{
						sizeCodeProf = (SizeCodeProfile)dictEnum.Value;
						_dtSizeCurve.Columns.Add(sizeCodeProf.SizeCodePrimaryKey, typeof(double));
					}
				}

				// Fill Size Curve Table

				foreach (SizeCurveProfile szCrvProf in _currSizeCurveGrpProf.SizeCurveList)
				{
					CreateDefaultSecondaryRows(_dtSizeCurve, szCrvProf.Key);

					foreach (SizeCodeProfile szCdPrf in szCrvProf.SizeCodeList)
					{
						if (szCdPrf.SizeCodePercent > 0)
						{
							currRow = _dtSizeCurve.Rows.Find(new object[] { szCrvProf.Key, false, szCdPrf.SizeCodeSecondary });
							currRow[szCdPrf.SizeCodePrimaryKey] = (double)(decimal)szCdPrf.SizeCodePercent;
						}
					}
				}

				foreach (SizeCurveProfile sizeCurveProf in _currSizeCurveGrpProf.SizeCurveList)
				{
					SumSizeCurvePercentages(_dtSizeCurve, sizeCurveProf.Key);
				}

				_dtSizeCurve.AcceptChanges();
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		private void BuildDefaultSizeCurveTable()
		{
			IDictionaryEnumerator dictEnum;
			DataRow currRow;
			SizeCodeProfile sizeCodeProf;

			try
			{
				_dtDefaultSizeCurve = MIDEnvironment.CreateDataTable("DefaultSizeCurve");
				_dtDefaultSizeCurve.Columns.Add("SizeCurveRID", typeof(int));
				_dtDefaultSizeCurve.Columns.Add("isTotalRow", typeof(bool));
				_dtDefaultSizeCurve.Columns.Add("hasSecondarySizes", typeof(bool));
				_dtDefaultSizeCurve.Columns.Add("Secondary", typeof(string));
				_dtDefaultSizeCurve.PrimaryKey = new DataColumn[] { _dtDefaultSizeCurve.Columns["SizeCurveRID"], _dtDefaultSizeCurve.Columns["isTotalRow"], _dtDefaultSizeCurve.Columns["Secondary"] };

				// Create Primary Size Columns

				if (_primarySizeList.Count > 0)
				{
					_dtDefaultSizeCurve.Columns.Add(cTotalColName, typeof(double));

					dictEnum = _primarySizeList.GetEnumerator();

					while (dictEnum.MoveNext())
					{
						sizeCodeProf = (SizeCodeProfile)dictEnum.Value;
						_dtDefaultSizeCurve.Columns.Add(sizeCodeProf.SizeCodePrimaryKey, typeof(double));
					}
				}
	
				// Fill Default Table

				if (_currSizeCurveGrpProf.DefaultSizeCurve != null)
				{
					CreateDefaultSecondaryRows(_dtDefaultSizeCurve, _currSizeCurveGrpProf.DefaultSizeCurve.Key);

					foreach (SizeCodeProfile szCdPrf in _currSizeCurveGrpProf.DefaultSizeCurve.SizeCodeList)
					{
						if (szCdPrf.SizeCodePercent > 0)
						{
							currRow = _dtDefaultSizeCurve.Rows.Find(new object[] { _currSizeCurveGrpProf.DefaultSizeCurve.Key, false, szCdPrf.SizeCodeSecondary });
							currRow[szCdPrf.SizeCodePrimaryKey] =(double)(decimal)szCdPrf.SizeCodePercent;
						}
					}
				}
				else
				{
					CreateDefaultSecondaryRows(_dtDefaultSizeCurve, Include.NoRID);
				}

				SumSizeCurvePercentages(_dtDefaultSizeCurve, _currSizeCurveGrpProf.DefaultSizeCurveRid);

				_dtDefaultSizeCurve.AcceptChanges();
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		private void BuildCurveValueList()
		{
			MenuItem curveItem;
			IDictionaryEnumerator dictEnum;
            // BEGIN MID Track #5248 - KJohnson - Able to create a Size Curve without putting in the %'s by size and receive and error.
            ProfileList szCrvProfRemoveList;
            // END MID Track #5248

			try
			{
				ClearCurveMenuItemEventHandlers();
				_sortedCurveList.Clear();
				_sortedCurveList.Add(cDefaultLabel, cDefaultIndex);

				_curveValList.ValueListItems.Clear();
				_curveValList.ValueListItems.Add(cCopyCurrentIndex, cCopyCurrentLabel);
				_curveValList.ValueListItems.Add(cNewCurveIndex, cCreateNewLabel);
				_curveValList.ValueListItems.Add(cDefaultIndex, cDefaultLabel);

                // BEGIN MID Track #5248 - KJohnson - Able to create a Size Curve without putting in the %'s by size and receive and error.
                szCrvProfRemoveList = new SizeCodeList(eProfileType.SizeCode);
				foreach (SizeCurveProfile szCrvProf in _currSizeCurveGrpProf.SizeCurveList)
				{
                    if (szCrvProf.SizeCurveName != null)
                    {
                        _sortedCurveList.Add(szCrvProf.SizeCurveName, szCrvProf.Key);
                        _curveValList.ValueListItems.Add(szCrvProf.Key, szCrvProf.SizeCurveName);
                    }
                    else
                    {
                        szCrvProfRemoveList.Add((SizeCurveProfile)szCrvProf);
                    }
				}

                //--Remove Unwanted Items-----
                foreach (SizeCurveProfile szCrvProf in szCrvProfRemoveList)
                {
                    _currSizeCurveGrpProf.SizeCurveList.Remove(szCrvProf);
                }
                // END MID Track #5248

				mniSetAllCurvesInSet.MenuItems.Clear();

				dictEnum = _sortedCurveList.GetEnumerator();

				while (dictEnum.MoveNext())
				{
					curveItem = new MenuItem();
					curveItem.Text = Convert.ToString(dictEnum.Key);
					curveItem.Click += new System.EventHandler(this.mniSizeCurve_Click);

					mniSetAllCurvesInSet.MenuItems.Add(curveItem);
					_sizeCurveMenuItemList.Add(curveItem);
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		private void BuildStoreAttributeTables(ProfileList aGroupLevelList)
		{
			IDictionaryEnumerator dictEnum;
			SizeCurveProfile sizeCurveProf;
			DataRow storeRow;

			try
			{
                _groupLevelList = aGroupLevelList;

                // Begin Track #4872 - JSmith - Global/User Attributes
                if (_currSizeCurveGrpProf == null)
                {
                    return;
                }
                // End Track #4872

				_dtSets = MIDEnvironment.CreateDataTable("Sets");
				_dtSets.Columns.Add("StoreGroupLevelRID", typeof(int));
				_dtSets.Columns.Add("Store Set", typeof(string));

				_dtStores = MIDEnvironment.CreateDataTable("Store");
				_dtStores.Columns.Add("StoreGroupLevelRID", typeof(int));
				_dtStores.Columns.Add("StoreRID", typeof(int));
				_dtStores.Columns.Add("Store ID", typeof(string));
				_dtStores.Columns.Add("SizeCurveRID", typeof(int));
				_dtStores.Columns.Add("Curve Name", typeof(string));
				_dtStores.PrimaryKey = new DataColumn[] { _dtStores.Columns["StoreRID"] };

				// Build store and attribute tables

				foreach (StoreGroupLevelListViewProfile lstViewProf in _groupLevelList)
				{
					_dtSets.Rows.Add(new object[] { lstViewProf.Key, lstViewProf.Name });

					foreach (StoreProfile storeProf in lstViewProf.Stores)
					{
						_dtStores.Rows.Add(new object[] { lstViewProf.Key, storeProf.Key, storeProf.Text, System.DBNull.Value, cDefaultLabel });
					}
				}

				dictEnum = _currSizeCurveGrpProf.StoreSizeCurveHash.GetEnumerator();

				while (dictEnum.MoveNext())
				{
					sizeCurveProf = (SizeCurveProfile)dictEnum.Value;
					storeRow = _dtStores.Rows.Find(new object[] { Convert.ToInt32(dictEnum.Key) });

					if (storeRow != null)
					{
						storeRow["SizeCurveRID"] = sizeCurveProf.Key;
						storeRow["Curve Name"] = sizeCurveProf.SizeCurveName;
					}
				}

				_dtSets.AcceptChanges();
				_dtStores.AcceptChanges();
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		private void BindSizeCurveGrid()
		{
			int i;
			IDictionaryEnumerator dictEnum;
			SizeCodeProfile sizeCodeProf;

			try
			{
				if (_dtSets != null && _dtStores != null && _dtSizeCurve != null)
				{
					_dsSizeCurve = MIDEnvironment.CreateDataSet("SizeCurve");
					_dsSizeCurve.Tables.Add(_dtSets);
					_dsSizeCurve.Tables.Add(_dtStores);
					_dsSizeCurve.Tables.Add(_dtSizeCurve);
					_dsSizeCurve.Relations.Add("StoreGroupLevelRIDRelation", _dtSets.Columns["StoreGroupLevelRID"], _dtStores.Columns["StoreGroupLevelRID"], false);
					_dsSizeCurve.Relations.Add("SizeCurveRIDRelation", _dtStores.Columns["SizeCurveRID"], _dtSizeCurve.Columns["SizeCurveRID"], false);

					_dsSizeCurve.AcceptChanges();

					ulgSizeCurve.DataSource = _dsSizeCurve;
					ulgSizeCurve.DisplayLayout.Override.SelectTypeRow = SelectType.Single;
					ulgSizeCurve.DisplayLayout.Override.AllowColSizing = AllowColSizing.Free;
					ulgSizeCurve.DisplayLayout.AddNewBox.Hidden = true;
					ulgSizeCurve.DisplayLayout.AddNewBox.Style = AddNewBoxStyle.Compact;

					ulgSizeCurve.DisplayLayout.Bands[0].Columns["StoreGroupLevelRID"].Hidden = true;
					ulgSizeCurve.DisplayLayout.Bands[0].Columns["Store Set"].CellActivation = Activation.NoEdit;
					ulgSizeCurve.DisplayLayout.Bands[0].Columns["Store Set"].Header.VisiblePosition = 0;

					ulgSizeCurve.DisplayLayout.Bands[1].Columns["StoreGroupLevelRID"].Hidden = true;
					ulgSizeCurve.DisplayLayout.Bands[1].Columns["StoreRID"].Hidden = true;
					ulgSizeCurve.DisplayLayout.Bands[1].Columns["SizeCurveRID"].Hidden = true;
					ulgSizeCurve.DisplayLayout.Bands[1].Columns["Store ID"].CellActivation = Activation.NoEdit;
					ulgSizeCurve.DisplayLayout.Bands[1].Columns["Store ID"].Header.VisiblePosition = 0;
					ulgSizeCurve.DisplayLayout.Bands[1].Columns["Store ID"].Width = 200;
					ulgSizeCurve.DisplayLayout.Bands[1].Columns["Curve Name"].Header.VisiblePosition = 1;
					ulgSizeCurve.DisplayLayout.Bands[1].Columns["Curve Name"].Width = 200;
					ulgSizeCurve.DisplayLayout.Bands[1].Columns["Curve Name"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
					ulgSizeCurve.DisplayLayout.Bands[1].Columns["Curve Name"].ValueList = _curveValList;

					ulgSizeCurve.DisplayLayout.Bands[2].Columns["SizeCurveRID"].Hidden = true;
					ulgSizeCurve.DisplayLayout.Bands[2].Columns["isTotalRow"].Hidden = true;
					ulgSizeCurve.DisplayLayout.Bands[2].Columns["hasSecondarySizes"].Hidden = true;

					if (_showSecondaryColumn)
					{
						ulgSizeCurve.DisplayLayout.Bands[2].Columns["Secondary"].CellActivation = Activation.NoEdit;
						ulgSizeCurve.DisplayLayout.Bands[2].Columns["Secondary"].Header.Caption = " ";
						ulgSizeCurve.DisplayLayout.Bands[2].Columns["Secondary"].Header.VisiblePosition = 0;
						ulgSizeCurve.DisplayLayout.Bands[2].Columns["Secondary"].CellAppearance.BackColor = System.Drawing.Color.LightGray;
					}
					else
					{
						ulgSizeCurve.DisplayLayout.Bands[2].Columns["Secondary"].Hidden = true;
					}

					if (_primarySizeList.Count > 0)
					{
						ulgSizeCurve.DisplayLayout.Bands[2].Columns[cTotalColName].Header.Caption = "Total";
						ulgSizeCurve.DisplayLayout.Bands[2].Columns[cTotalColName].CellAppearance.FontData.Bold = DefaultableBoolean.True;
						ulgSizeCurve.DisplayLayout.Bands[2].Columns[cTotalColName].Header.Appearance.FontData.Bold = DefaultableBoolean.True;
						ulgSizeCurve.DisplayLayout.Bands[2].Columns[cTotalColName].MaskDisplayMode = MaskMode.IncludePromptChars;
						ulgSizeCurve.DisplayLayout.Bands[2].Columns[cTotalColName].Header.VisiblePosition = 1;
						ulgSizeCurve.DisplayLayout.Bands[2].Columns[cTotalColName].Format = "##0.000";
						ulgSizeCurve.DisplayLayout.Bands[2].Columns[cTotalColName].MinWidth = 60;
						ulgSizeCurve.DisplayLayout.Bands[2].Columns[cTotalColName].PerformAutoResize(PerformAutoSizeType.AllRowsInBand);
						ulgSizeCurve.DisplayLayout.Bands[2].Columns[cTotalColName].CellAppearance.TextHAlign = HAlign.Right;

						dictEnum = _primarySizeList.GetEnumerator();
						i = 2;

						while (dictEnum.MoveNext())
						{
							sizeCodeProf = (SizeCodeProfile)dictEnum.Value;

							ulgSizeCurve.DisplayLayout.Bands[2].Columns[sizeCodeProf.SizeCodePrimaryKey].MaskDisplayMode = MaskMode.IncludePromptChars;
							ulgSizeCurve.DisplayLayout.Bands[2].Columns[sizeCodeProf.SizeCodePrimaryKey].Header.VisiblePosition = i++;
							ulgSizeCurve.DisplayLayout.Bands[2].Columns[sizeCodeProf.SizeCodePrimaryKey].Format = "##0.000";
							ulgSizeCurve.DisplayLayout.Bands[2].Columns[sizeCodeProf.SizeCodePrimaryKey].MinWidth = 60;
							ulgSizeCurve.DisplayLayout.Bands[2].Columns[sizeCodeProf.SizeCodePrimaryKey].PerformAutoResize(PerformAutoSizeType.AllRowsInBand);
							ulgSizeCurve.DisplayLayout.Bands[2].Columns[sizeCodeProf.SizeCodePrimaryKey].CellAppearance.TextHAlign = HAlign.Right;
						}
					}

					ulgSizeCurve.Rows.ExpandAll(true);

					RecurseRowsAndHighlightTotals(ulgSizeCurve.GetRow(ChildRow.First));
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		private void BindDefaultSizeCurveGrid()
		{
			int i;
			IDictionaryEnumerator dictEnum;
			SizeCodeProfile sizeCodeProf;

			try
			{
				if (_dtDefaultSizeCurve != null)
				{
					ulgDefaultSizeCurve.DataSource = _dtDefaultSizeCurve;
					ulgDefaultSizeCurve.DisplayLayout.Override.SelectTypeRow = SelectType.Single;
					ulgDefaultSizeCurve.DisplayLayout.AddNewBox.Hidden = true;
					ulgDefaultSizeCurve.DisplayLayout.AddNewBox.Style = AddNewBoxStyle.Compact;

					ulgDefaultSizeCurve.DisplayLayout.Bands[0].Columns["SizeCurveRID"].Hidden = true;
					ulgDefaultSizeCurve.DisplayLayout.Bands[0].Columns["isTotalRow"].Hidden = true;
					ulgDefaultSizeCurve.DisplayLayout.Bands[0].Columns["hasSecondarySizes"].Hidden = true;

					if (_showSecondaryColumn)
					{
						ulgDefaultSizeCurve.DisplayLayout.Bands[0].Columns["Secondary"].CellActivation = Activation.NoEdit;
						ulgDefaultSizeCurve.DisplayLayout.Bands[0].Columns["Secondary"].Header.Caption = " ";
						ulgDefaultSizeCurve.DisplayLayout.Bands[0].Columns["Secondary"].Header.VisiblePosition = 0;
						ulgDefaultSizeCurve.DisplayLayout.Bands[0].Columns["Secondary"].CellAppearance.BackColor = System.Drawing.Color.LightGray;
					}
					else
					{
						ulgDefaultSizeCurve.DisplayLayout.Bands[0].Columns["Secondary"].Hidden = true;
					}

					if (_primarySizeList.Count > 0)
					{
						ulgDefaultSizeCurve.DisplayLayout.Bands[0].Columns[cTotalColName].Header.Caption = "Total";
						ulgDefaultSizeCurve.DisplayLayout.Bands[0].Columns[cTotalColName].CellAppearance.FontData.Bold = DefaultableBoolean.True;
						ulgDefaultSizeCurve.DisplayLayout.Bands[0].Columns[cTotalColName].Header.Appearance.FontData.Bold = DefaultableBoolean.True;
						ulgDefaultSizeCurve.DisplayLayout.Bands[0].Columns[cTotalColName].MaskDisplayMode = MaskMode.IncludePromptChars;
						ulgDefaultSizeCurve.DisplayLayout.Bands[0].Columns[cTotalColName].Header.VisiblePosition = 1;
						ulgDefaultSizeCurve.DisplayLayout.Bands[0].Columns[cTotalColName].Format = "##0.000";
						ulgDefaultSizeCurve.DisplayLayout.Bands[0].Columns[cTotalColName].MinWidth = 60;
						ulgDefaultSizeCurve.DisplayLayout.Bands[0].Columns[cTotalColName].PerformAutoResize(PerformAutoSizeType.AllRowsInBand);
						ulgDefaultSizeCurve.DisplayLayout.Bands[0].Columns[cTotalColName].CellAppearance.TextHAlign = HAlign.Right;

						dictEnum = _primarySizeList.GetEnumerator();
						i = 2;

						while (dictEnum.MoveNext())
						{
							sizeCodeProf = (SizeCodeProfile)dictEnum.Value;

							ulgDefaultSizeCurve.DisplayLayout.Bands[0].Columns[sizeCodeProf.SizeCodePrimaryKey].MaskDisplayMode = MaskMode.IncludePromptChars;
							ulgDefaultSizeCurve.DisplayLayout.Bands[0].Columns[sizeCodeProf.SizeCodePrimaryKey].Header.VisiblePosition = i++;
							ulgDefaultSizeCurve.DisplayLayout.Bands[0].Columns[sizeCodeProf.SizeCodePrimaryKey].Format = "##0.000";
							ulgDefaultSizeCurve.DisplayLayout.Bands[0].Columns[sizeCodeProf.SizeCodePrimaryKey].MinWidth = 60;
							ulgDefaultSizeCurve.DisplayLayout.Bands[0].Columns[sizeCodeProf.SizeCodePrimaryKey].PerformAutoResize(PerformAutoSizeType.AllRowsInBand);
							ulgDefaultSizeCurve.DisplayLayout.Bands[0].Columns[sizeCodeProf.SizeCodePrimaryKey].CellAppearance.TextHAlign = HAlign.Right;
						}
					}

					ulgDefaultSizeCurve.Rows.ExpandAll(true);
			
					RecurseRowsAndHighlightTotals(ulgDefaultSizeCurve.GetRow(ChildRow.First));
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		private void AddSizeCodeToTable(SizeCodeList aSizeCodeList)
		{
			DataRow currRow;

			try
			{
				foreach (SizeCodeProfile sizeCodeProf in aSizeCodeList)
				{
					if (sizeCodeProf.SizeCodeSecondary.Trim().Length > 0)
					{
						_showSecondaryColumn = true;
					}

					if (!_primarySizeList.Contains(sizeCodeProf.PrimarySequence + " - " + sizeCodeProf.SizeCodePrimaryKey))
					{
						_primarySizeList.Add(sizeCodeProf.PrimarySequence + " - " + sizeCodeProf.SizeCodePrimaryKey, sizeCodeProf);
					}

					if (!_secondarySizeList.Contains(sizeCodeProf.SecondarySequence))
					{
						_secondarySizeList.Add(sizeCodeProf.SecondarySequence, sizeCodeProf);
					}

					if (!_dtSizeCodeKeys.Columns.Contains(sizeCodeProf.SizeCodePrimaryKey))
					{
						_dtSizeCodeKeys.Columns.Add(sizeCodeProf.SizeCodePrimaryKey, typeof(object));
					}

					currRow = _dtSizeCodeKeys.Rows.Find(sizeCodeProf.SizeCodeSecondary);

					if (currRow == null)
					{
						currRow = _dtSizeCodeKeys.NewRow();
						currRow["Secondary"] = sizeCodeProf.SizeCodeSecondary;
						_dtSizeCodeKeys.Rows.Add(currRow);
					}

					if (currRow[sizeCodeProf.SizeCodePrimaryKey] == System.DBNull.Value)
					{
						currRow[sizeCodeProf.SizeCodePrimaryKey] = sizeCodeProf;
					}
                    else if (((SizeCodeProfile)currRow[sizeCodeProf.SizeCodePrimaryKey]).Key != sizeCodeProf.Key)
                    {
                        throw new MIDException(eErrorLevel.severe, (int)eMIDTextCode.msg_InvalidSizeCodeEncountered, MIDText.GetText(eMIDTextCode.msg_InvalidSizeCodeEncountered));
                    }
                }
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		private void RecurseRowsAndHighlightTotals(UltraGridRow startRow)
		{
			UltraGridRow currRow;

			try
			{
				currRow = startRow;

				while (currRow != null)
				{
					if (currRow.Band.Key == "SizeCurveRIDRelation" || currRow.Band.Key == "DefaultSizeCurve") 
					{
						if (Convert.ToBoolean(currRow.Cells["isTotalRow"].Value))
						{
							currRow.CellAppearance.FontData.Bold = DefaultableBoolean.True;
						}
					}
					else
					{
						if (currRow.HasChild(false))
						{
							RecurseRowsAndHighlightTotals(currRow.GetChild(ChildRow.First));
						}
					}

					currRow = currRow.GetSibling(SiblingRow.Next, true, false);
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		private void StoreSizeCurveChanged(UltraGridCell aCell, object aNewCurveKey, object aNewCurveName)
		{
			try
			{
				if (aNewCurveName != null)
				{
					aCell.Row.Cells["Curve Name"].Value = aNewCurveName;
				}

				if (aNewCurveKey != null)
				{
					aCell.Row.Cells["SizeCurveRID"].Value = aNewCurveKey;

					if (aNewCurveKey != System.DBNull.Value)
					{
						_currSizeCurveGrpProf.SetStoreSizeCurveProfile(Convert.ToInt32(aCell.Row.Cells["StoreRID"].Value), Convert.ToInt32(aNewCurveKey));
					}
					else
					{
						_currSizeCurveGrpProf.DeleteStoreSizeCurveProfile(Convert.ToInt32(aCell.Row.Cells["StoreRID"].Value));
					}
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		private void CreateDefaultSecondaryRows(DataTable aTable, int aSizeCurveRID)
		{
			IDictionaryEnumerator dictEnum;
			SizeCodeProfile sizeCodeProf;

			try
			{
				if (_secondarySizeList.Count > 0)
				{
					bool hasSecondarySizes = false;
					if (_secondarySizeList.Count > 1)
					{
						hasSecondarySizes = true;
						CreateDefaultSecondaryRow(aTable, aSizeCurveRID, true, "Total", hasSecondarySizes);
					}

					dictEnum = _secondarySizeList.GetEnumerator();

					while (dictEnum.MoveNext())
					{
						sizeCodeProf = (SizeCodeProfile)dictEnum.Value;
						CreateDefaultSecondaryRow(aTable, aSizeCurveRID, false, sizeCodeProf.SizeCodeSecondary, hasSecondarySizes);
					}

					aTable.AcceptChanges();
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		private void CreateDefaultSecondaryRow(DataTable aTable, int aSizeCurveRID, bool aIsTotalRow, string aSecondaryKey, bool aHasSecondarySizes)
		{
			IDictionaryEnumerator dictEnum;
			DataRow newRow;
			SizeCodeProfile sizeCodeProf;

			try
			{
				newRow = aTable.NewRow();
				newRow["SizeCurveRID"] = aSizeCurveRID;
				newRow["isTotalRow"] = aIsTotalRow;
				newRow["hasSecondarySizes"] = aHasSecondarySizes;
				newRow["Secondary"] = aSecondaryKey;
				newRow[cTotalColName] = 0;

				dictEnum = _primarySizeList.GetEnumerator();

				while (dictEnum.MoveNext())
				{
					sizeCodeProf = (SizeCodeProfile)dictEnum.Value;
					newRow[sizeCodeProf.SizeCodePrimaryKey] = 0;
				}

				aTable.Rows.Add(newRow);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		private void CopySecondaryRows(DataTable aTable, int aSizeCurveRID, DataTable aCopyTable, int aCopySizeCurveRID)
		{
			DataRow[] selectRows;

			try
			{
				selectRows = aCopyTable.Select("SizeCurveRID = " + aCopySizeCurveRID + " AND isTotalRow = 'True'");
				CopySecondaryRow(aTable, aSizeCurveRID, selectRows);
				
				selectRows = aCopyTable.Select("SizeCurveRID = " + aCopySizeCurveRID + " AND isTotalRow = 'False'");
				CopySecondaryRow(aTable, aSizeCurveRID, selectRows);

				aTable.AcceptChanges();
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		private void CopySecondaryRow(DataTable aTable, int aSizeCurveRID, DataRow[] aSelectedRows)
		{
			DataRow newRow;

			try
			{
				foreach (DataRow row in aSelectedRows)
				{
					newRow = aTable.NewRow();
					newRow.ItemArray = row.ItemArray;
					newRow["SizeCurveRID"] = aSizeCurveRID;

					aTable.Rows.Add(newRow);
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		private void UpdateCellValue(int aSizeCurveRID, string aPrimary, string aSecondary, double aValue)
		{
			DataRow currRow;
			SizeCurveProfile sizeCurveProf;
			SizeCodeProfile sizeCodeProf;
			SizeCodeProfile curveSizeCodeProf = null;

			try
			{
				currRow = _dtSizeCodeKeys.Rows.Find(aSecondary);
				sizeCodeProf = (SizeCodeProfile)currRow[aPrimary];

				if (aSizeCurveRID != _currSizeCurveGrpProf.DefaultSizeCurveRid)
				{
					sizeCurveProf = (SizeCurveProfile)_currSizeCurveGrpProf.SizeCurveList.FindKey(aSizeCurveRID);
				}
				else
				{
					sizeCurveProf = _currSizeCurveGrpProf.DefaultSizeCurve;
				}

				curveSizeCodeProf = (SizeCodeProfile)sizeCurveProf.SizeCodeList.FindKey(sizeCodeProf.Key);

				if (curveSizeCodeProf == null)
				{
					curveSizeCodeProf = (SizeCodeProfile)sizeCodeProf.Clone();
					sizeCurveProf.SizeCodeList.Add(curveSizeCodeProf);
				}

				curveSizeCodeProf.SizeCodePercent = (float)(decimal)aValue;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		private void SumSizeCurvePercentages(DataTable aSizeCurveTable, int aSizeCurveRID)
		{
			IDictionaryEnumerator primaryEnum;
			IDictionaryEnumerator secondaryEnum;
			SizeCodeProfile priSizeCodeProf;
			SizeCodeProfile secSizeCodeProf;
			object sumCell;
			//Begin TT#1346 - JScott - Receiving "140020:Curve Percentage must sum to 100" error when saving Size Curve
			//double sum;
			decimal sum;
			//End TT#1346 - JScott - Receiving "140020:Curve Percentage must sum to 100" error when saving Size Curve

			try
			{
				primaryEnum = _primarySizeList.GetEnumerator();

				if (_secondarySizeList.Count > 1)
				{
					while (primaryEnum.MoveNext())
					{
						priSizeCodeProf = (SizeCodeProfile)primaryEnum.Value;
						secondaryEnum = _secondarySizeList.GetEnumerator();
						sum = 0;

						while (secondaryEnum.MoveNext())
						{
							secSizeCodeProf = (SizeCodeProfile)secondaryEnum.Value;
							sumCell = aSizeCurveTable.Rows.Find(new object[] { aSizeCurveRID, false, secSizeCodeProf.SizeCodeSecondary })[priSizeCodeProf.SizeCodePrimaryKey];

							if (sumCell != System.DBNull.Value)
							{
								//Begin TT#1346 - JScott - Receiving "140020:Curve Percentage must sum to 100" error when saving Size Curve
								//sum += (double)(decimal)Convert.ToDouble(sumCell);
								sum += (decimal)Convert.ToDouble(sumCell);
								//End TT#1346 - JScott - Receiving "140020:Curve Percentage must sum to 100" error when saving Size Curve
							}
						}

						//Begin TT#1346 - JScott - Receiving "140020:Curve Percentage must sum to 100" error when saving Size Curve
						//aSizeCurveTable.Rows.Find(new object[] { aSizeCurveRID, true, "Total" })[priSizeCodeProf.SizeCodePrimary] = sum;
						aSizeCurveTable.Rows.Find(new object[] { aSizeCurveRID, true, "Total" })[priSizeCodeProf.SizeCodePrimaryKey] = (double)sum;
						//End TT#1346 - JScott - Receiving "140020:Curve Percentage must sum to 100" error when saving Size Curve
					}
				}

				secondaryEnum = _secondarySizeList.GetEnumerator();

				while (secondaryEnum.MoveNext())
				{
					secSizeCodeProf = (SizeCodeProfile)secondaryEnum.Value;
					primaryEnum = _primarySizeList.GetEnumerator();
					sum = 0;

					while (primaryEnum.MoveNext())
					{
						priSizeCodeProf = (SizeCodeProfile)primaryEnum.Value;
						sumCell = aSizeCurveTable.Rows.Find(new object[] { aSizeCurveRID, false, secSizeCodeProf.SizeCodeSecondary })[priSizeCodeProf.SizeCodePrimaryKey];

						if (sumCell != System.DBNull.Value)
						{
							//Begin TT#1346 - JScott - Receiving "140020:Curve Percentage must sum to 100" error when saving Size Curve
							//sum += (double)(decimal)Convert.ToDouble(sumCell);
							sum += (decimal)Convert.ToDouble(sumCell);
							//End TT#1346 - JScott - Receiving "140020:Curve Percentage must sum to 100" error when saving Size Curve
						}
					}

					//Begin TT#1346 - JScott - Receiving "140020:Curve Percentage must sum to 100" error when saving Size Curve
					//aSizeCurveTable.Rows.Find(new object[] { aSizeCurveRID, false, secSizeCodeProf.SizeCodeSecondary })[cTotalColName] = sum;
					aSizeCurveTable.Rows.Find(new object[] { aSizeCurveRID, false, secSizeCodeProf.SizeCodeSecondary })[cTotalColName] = (double)sum;
					//End TT#1346 - JScott - Receiving "140020:Curve Percentage must sum to 100" error when saving Size Curve
				}

				if (_secondarySizeList.Count > 1)
				{
					if (_primarySizeList.Count > 0)
					{
						primaryEnum = _primarySizeList.GetEnumerator();
						sum = 0;

						while (primaryEnum.MoveNext())
						{
							priSizeCodeProf = (SizeCodeProfile)primaryEnum.Value;
							sumCell = aSizeCurveTable.Rows.Find(new object[] { aSizeCurveRID, true, "Total" })[priSizeCodeProf.SizeCodePrimaryKey];

							if (sumCell != System.DBNull.Value)
							{
								//Begin TT#1346 - JScott - Receiving "140020:Curve Percentage must sum to 100" error when saving Size Curve
								//sum += (double)(decimal)Convert.ToDouble(sumCell);
								sum += (decimal)Convert.ToDouble(sumCell);
								//End TT#1346 - JScott - Receiving "140020:Curve Percentage must sum to 100" error when saving Size Curve
							}
						}

						//Begin TT#1346 - JScott - Receiving "140020:Curve Percentage must sum to 100" error when saving Size Curve
						//aSizeCurveTable.Rows.Find(new object[] { aSizeCurveRID, true, "Total" })[cTotalColName] = sum;
						aSizeCurveTable.Rows.Find(new object[] { aSizeCurveRID, true, "Total" })[cTotalColName] = (double)sum;
						//End TT#1346 - JScott - Receiving "140020:Curve Percentage must sum to 100" error when saving Size Curve
					}
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

//Begin Track #4041 - JScott - Long delay during cell update
//		private void SpreadTotalToAll(DataTable aSizeCurveTable, int aSizeCurveRID, double aSpreadValue)
//		{
//			BasicSpread spreader;
//			ArrayList inValues;
//			ArrayList outValues;
//			IDictionaryEnumerator priEnum;
//			IDictionaryEnumerator secEnum;
//			SizeCodeProfile priSizeCodeProf;
//			SizeCodeProfile secSizeCodeProf;
//			DataRow currRow;
//			int i;
//			
//			try
//			{
//				spreader = new BasicSpread();
//				inValues = new ArrayList();
//
//				secEnum = _secondarySizeList.GetEnumerator();
//
//				while (secEnum.MoveNext())
//				{
//					secSizeCodeProf = (SizeCodeProfile)secEnum.Value;
//					currRow = aSizeCurveTable.Rows.Find(new object[] { aSizeCurveRID, false, secSizeCodeProf.SizeCodeSecondary });
//					priEnum = _primarySizeList.GetEnumerator();
//
//					while (priEnum.MoveNext())
//					{
//						priSizeCodeProf = (SizeCodeProfile)priEnum.Value;
//
//						if (currRow[priSizeCodeProf.SizeCodePrimary] != System.DBNull.Value && Convert.ToString(currRow[priSizeCodeProf.SizeCodePrimary]) != string.Empty)
//						{
//							inValues.Add((double)(decimal)Convert.ToDouble(currRow[priSizeCodeProf.SizeCodePrimary]));
//						}
//						else
//						{
//							inValues.Add(0);
//						}
//					}
//				}
//
//				spreader.ExecuteSimpleSpread(aSpreadValue, inValues, 3, out outValues);
//
//				secEnum = _secondarySizeList.GetEnumerator();
//				i = 0;
//
//				while (secEnum.MoveNext())
//				{
//					secSizeCodeProf = (SizeCodeProfile)secEnum.Value;
//					currRow = aSizeCurveTable.Rows.Find(new object[] { aSizeCurveRID, false, secSizeCodeProf.SizeCodeSecondary });
//					priEnum = _primarySizeList.GetEnumerator();
//
//					while (priEnum.MoveNext())
//					{
//						priSizeCodeProf = (SizeCodeProfile)priEnum.Value;
//
//						if ((double)(decimal)Convert.ToDouble(currRow[priSizeCodeProf.SizeCodePrimary]) != (double)(decimal)Convert.ToDouble(outValues[i]))
//						{
//							currRow[priSizeCodeProf.SizeCodePrimary] = (double)(decimal)Convert.ToDouble(outValues[i]);
//
//							if (priSizeCodeProf.SizeCodePrimary != cTotalColName && !Convert.ToBoolean(currRow["isTotalRow"]))
//							{
//								UpdateCellValue(aSizeCurveRID, priSizeCodeProf.SizeCodePrimary, secSizeCodeProf.SizeCodeSecondary, (double)(decimal)Convert.ToDouble(outValues[i]));
//							}
//						}
//
//						i++;
//					}
//				}
//			}
//			catch (Exception exc)
//			{
//				string message = exc.ToString();
//				throw;
//			}
//		}
//
//		private void SpreadTotalToPrimary(DataTable aSizeCurveTable, int aSizeCurveRID, string aSecondaryKey, bool aIsTotalRow, double aSpreadValue)
//		{
//			BasicSpread spreader;
//			ArrayList inValues;
//			ArrayList outValues;
//			IDictionaryEnumerator dictEnum;
//			SizeCodeProfile sizeCodeProf;
//			DataRow currRow;
//			int i;
//			
//			try
//			{
//				spreader = new BasicSpread();
//				inValues = new ArrayList();
//
//				dictEnum = _primarySizeList.GetEnumerator();
//
//				while (dictEnum.MoveNext())
//				{
//					sizeCodeProf = (SizeCodeProfile)dictEnum.Value;
//					currRow = aSizeCurveTable.Rows.Find(new object[] { aSizeCurveRID, aIsTotalRow, aSecondaryKey });
//
//					if (currRow[sizeCodeProf.SizeCodePrimary] != System.DBNull.Value && Convert.ToString(currRow[sizeCodeProf.SizeCodePrimary]) != string.Empty)
//					{
//						inValues.Add((double)(decimal)Convert.ToDouble(currRow[sizeCodeProf.SizeCodePrimary]));
//					}
//					else
//					{
//						inValues.Add(0);
//					}
//				}
//
//				spreader.ExecuteSimpleSpread(aSpreadValue, inValues, 3, out outValues);
//
//				dictEnum = _primarySizeList.GetEnumerator();
//				i = 0;
//
//				while (dictEnum.MoveNext())
//				{
//					sizeCodeProf = (SizeCodeProfile)dictEnum.Value;
//					currRow = aSizeCurveTable.Rows.Find(new object[] { aSizeCurveRID, aIsTotalRow, aSecondaryKey });
//
//					if ((double)(decimal)Convert.ToDouble(currRow[sizeCodeProf.SizeCodePrimary]) != (double)(decimal)Convert.ToDouble(outValues[i]))
//					{
//						currRow[sizeCodeProf.SizeCodePrimary] = (double)(decimal)Convert.ToDouble(outValues[i]);
//
//						if (sizeCodeProf.SizeCodePrimary != cTotalColName && !Convert.ToBoolean(currRow["isTotalRow"]))
//						{
//							UpdateCellValue(aSizeCurveRID, sizeCodeProf.SizeCodePrimary, aSecondaryKey, (double)(decimal)Convert.ToDouble(outValues[i]));
//						}
//					}
//
//					i++;
//				}
//			}
//			catch (Exception exc)
//			{
//				string message = exc.ToString();
//				throw;
//			}
//		}
//
//		private void SpreadTotalToSecondary(DataTable aSizeCurveTable, int aSizeCurveRID, string aPrimaryKey, double aSpreadValue)
//		{
//			BasicSpread spreader;
//			ArrayList inValues;
//			ArrayList outValues;
//			IDictionaryEnumerator dictEnum;
//			SizeCodeProfile sizeCodeProf;
//			DataRow currRow;
//			int i;
//			
//			try
//			{
//				spreader = new BasicSpread();
//				inValues = new ArrayList();
//
//				dictEnum = _secondarySizeList.GetEnumerator();
//
//				while (dictEnum.MoveNext())
//				{
//					sizeCodeProf = (SizeCodeProfile)dictEnum.Value;
//					currRow = aSizeCurveTable.Rows.Find(new object[] { aSizeCurveRID, false, sizeCodeProf.SizeCodeSecondary });
//
//					if (currRow[aPrimaryKey] != System.DBNull.Value && Convert.ToString(currRow[aPrimaryKey]) != string.Empty)
//					{
//						inValues.Add((double)(decimal)Convert.ToDouble(currRow[aPrimaryKey]));
//					}
//					else
//					{
//						inValues.Add(0);
//					}
//				}
//
//				spreader.ExecuteSimpleSpread(aSpreadValue, inValues, 3, out outValues);
//
//				dictEnum = _secondarySizeList.GetEnumerator();
//				i = 0;
//
//				while (dictEnum.MoveNext())
//				{
//					sizeCodeProf = (SizeCodeProfile)dictEnum.Value;
//					currRow = aSizeCurveTable.Rows.Find(new object[] { aSizeCurveRID, false, sizeCodeProf.SizeCodeSecondary });
//
//					if ((double)(decimal)Convert.ToDouble(currRow[aPrimaryKey]) != (double)(decimal)Convert.ToDouble(outValues[i]))
//					{
//						currRow[aPrimaryKey] = (double)(decimal)Convert.ToDouble(outValues[i]);
//
//						if (aPrimaryKey != cTotalColName && !Convert.ToBoolean(currRow["isTotalRow"]))
//						{
//							UpdateCellValue(aSizeCurveRID, aPrimaryKey, sizeCodeProf.SizeCodeSecondary, (double)(decimal)Convert.ToDouble(outValues[i]));
//						}
//					}
//
//					i++;
//				}
//			}
//			catch (Exception exc)
//			{
//				string message = exc.ToString();
//				throw;
//			}
//		}
		private void SumCellSizeCurvePercentages(UltraGridCell aCell)
		{
			UltraGridRow chgRow;

			try
			{
				chgRow = aCell.Row;

				// Recalculate the Column Totals

				if (chgRow.ParentCollection.Count > 1)
				{
					SumColumnSizeCurvePercentages(aCell, aCell.Column.Key);
				}

				// Recalculate the Row Totals

				SumRowSizeCurvePercentages(aCell, chgRow);

				// Recalculate the Grand Total

				if (chgRow.ParentCollection.Count > 1)
				{
					SumColumnSizeCurvePercentages(aCell, cTotalColName);
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		private void SumAllSizeCurvePercentages(UltraGridCell aCell)
		{
			UltraGridRow chgRow;
			IDictionaryEnumerator primaryEnum;
			SizeCodeProfile priSizeCodeProf;

			try
			{
				chgRow = aCell.Row;

				// Recalculate the Column Totals

				if (chgRow.ParentCollection.Count > 1)
				{
					primaryEnum = _primarySizeList.GetEnumerator();

					while (primaryEnum.MoveNext())
					{
						priSizeCodeProf = (SizeCodeProfile)primaryEnum.Value;
						SumColumnSizeCurvePercentages(aCell, priSizeCodeProf.SizeCodePrimaryKey);
					}
				}

				// Recalculate the Row Totals

				foreach (UltraGridRow row in chgRow.ParentCollection)
				{
					SumRowSizeCurvePercentages(aCell, row);
				}

				// Recalculate the Grand Total

				if (chgRow.ParentCollection.Count > 1)
				{
					SumColumnSizeCurvePercentages(aCell, cTotalColName);
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		private void SumColumnSizeCurvePercentages(UltraGridCell aCell)
		{
			UltraGridRow chgRow;

			try
			{
				chgRow = aCell.Row;

				// Recalculate the Column Totals

				if (chgRow.ParentCollection.Count > 1)
				{
					SumColumnSizeCurvePercentages(aCell, aCell.Column.Key);
				}

				// Recalculate the Row Totals

				foreach (UltraGridRow row in chgRow.ParentCollection)
				{
					SumRowSizeCurvePercentages(aCell, row);
				}

				// Recalculate the Grand Total

				if (chgRow.ParentCollection.Count > 1)
				{
					SumColumnSizeCurvePercentages(aCell, cTotalColName);
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		private void SumRowSizeCurvePercentages(UltraGridCell aCell)
		{
			UltraGridRow chgRow;
			IDictionaryEnumerator primaryEnum;
			SizeCodeProfile priSizeCodeProf;

			try
			{
				chgRow = aCell.Row;

				// Recalculate the Column Totals

				if (chgRow.ParentCollection.Count > 1)
				{
					primaryEnum = _primarySizeList.GetEnumerator();

					while (primaryEnum.MoveNext())
					{
						priSizeCodeProf = (SizeCodeProfile)primaryEnum.Value;
						SumColumnSizeCurvePercentages(aCell, priSizeCodeProf.SizeCodePrimaryKey);
					}
				}

				// Recalculate the Row Totals

				SumRowSizeCurvePercentages(aCell, chgRow);

				// Recalculate the Grand Total

				if (chgRow.ParentCollection.Count > 1)
				{
					SumColumnSizeCurvePercentages(aCell, cTotalColName);
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		private void SumColumnSizeCurvePercentages(UltraGridCell aCell, string aColumn)
		{
			UltraGridRow chgRow;
			UltraGridRow totRow;
			//Begin TT#1346 - JScott - Receiving "140020:Curve Percentage must sum to 100" error when saving Size Curve
			//double sum;
			decimal sum;
			//End TT#1346 - JScott - Receiving "140020:Curve Percentage must sum to 100" error when saving Size Curve

			try
			{
				chgRow = aCell.Row;
				totRow = null;
				sum = 0;
					 
				foreach (UltraGridRow row in chgRow.ParentCollection)
				{
					if (Convert.ToBoolean(row.Cells["isTotalRow"].Value))
					{
						totRow = row;
					}
					else
					{
						//Begin TT#1346 - JScott - Receiving "140020:Curve Percentage must sum to 100" error when saving Size Curve
						//sum += (double)(decimal)Convert.ToDouble(row.Cells[aColumn].Value);
						sum += (decimal)Convert.ToDouble(row.Cells[aColumn].Value);
						//End TT#1346 - JScott - Receiving "140020:Curve Percentage must sum to 100" error when saving Size Curve
					}
				}

				//Begin TT#1346 - JScott - Receiving "140020:Curve Percentage must sum to 100" error when saving Size Curve
				//SetCellValue(totRow.Cells[aColumn], sum);
				SetCellValue(totRow.Cells[aColumn], (double)sum);
				//End TT#1346 - JScott - Receiving "140020:Curve Percentage must sum to 100" error when saving Size Curve
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		private void SumRowSizeCurvePercentages(UltraGridCell aCell, UltraGridRow aRow)
		{
			IDictionaryEnumerator primaryEnum;
			//Begin TT#1346 - JScott - Receiving "140020:Curve Percentage must sum to 100" error when saving Size Curve
			//double sum;
			decimal sum;
			//End TT#1346 - JScott - Receiving "140020:Curve Percentage must sum to 100" error when saving Size Curve
			SizeCodeProfile priSizeCodeProf;

			try
			{
				primaryEnum = _primarySizeList.GetEnumerator();
				sum = 0;

				while (primaryEnum.MoveNext())
				{
					priSizeCodeProf = (SizeCodeProfile)primaryEnum.Value;
					//Begin TT#1346 - JScott - Receiving "140020:Curve Percentage must sum to 100" error when saving Size Curve
					//sum += (double)(decimal)Convert.ToDouble(aRow.Cells[priSizeCodeProf.SizeCodePrimary].Value);
					sum += (decimal)Convert.ToDouble(aRow.Cells[priSizeCodeProf.SizeCodePrimaryKey].Value);
					//End TT#1346 - JScott - Receiving "140020:Curve Percentage must sum to 100" error when saving Size Curve
				}

				//Begin TT#1346 - JScott - Receiving "140020:Curve Percentage must sum to 100" error when saving Size Curve
				//SetCellValue(aRow.Cells[cTotalColName], sum);
				SetCellValue(aRow.Cells[cTotalColName], (double)sum);
				//End TT#1346 - JScott - Receiving "140020:Curve Percentage must sum to 100" error when saving Size Curve
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		private void SpreadTotalToAll(UltraGridCell aCell)
		{
			BasicSpread spreader;
			ArrayList inValues;
			UltraGridRow chgRow;
			IDictionaryEnumerator primaryEnum;
			ArrayList outValues;
			SizeCodeProfile priSizeCodeProf;
			DataRow currRow;
			int i;
			
			try
			{
				spreader = new BasicSpread();
				inValues = new ArrayList();
				chgRow = aCell.Row;

				foreach (UltraGridRow row in chgRow.ParentCollection)
				{
					if (!Convert.ToBoolean(row.Cells["isTotalRow"].Value))
					{
						currRow = _dtSizeCodeKeys.Rows.Find(Convert.ToString(row.Cells["Secondary"].Value));
						primaryEnum = _primarySizeList.GetEnumerator();

						while (primaryEnum.MoveNext())
						{
							priSizeCodeProf = (SizeCodeProfile)primaryEnum.Value;
							if (currRow[priSizeCodeProf.SizeCodePrimaryKey] != System.DBNull.Value && Convert.ToString(currRow[priSizeCodeProf.SizeCodePrimaryKey]) != string.Empty)
							{
								inValues.Add((double)(decimal)Convert.ToDouble(row.Cells[priSizeCodeProf.SizeCodePrimaryKey].Value));
							}
						}
					}
				}

				spreader.ExecuteSimpleSpread((double)(decimal)Convert.ToDouble(aCell.Value), inValues, 3, out outValues);

				i = 0;

				foreach (UltraGridRow row in chgRow.ParentCollection)
				{
					if (!Convert.ToBoolean(row.Cells["isTotalRow"].Value))
					{
						currRow = _dtSizeCodeKeys.Rows.Find(Convert.ToString(row.Cells["Secondary"].Value));
						primaryEnum = _primarySizeList.GetEnumerator();

						while (primaryEnum.MoveNext())
						{
							priSizeCodeProf = (SizeCodeProfile)primaryEnum.Value;
							if (currRow[priSizeCodeProf.SizeCodePrimaryKey] != System.DBNull.Value && Convert.ToString(currRow[priSizeCodeProf.SizeCodePrimaryKey]) != string.Empty)
							{
								if ((double)(decimal)Convert.ToDouble(row.Cells[priSizeCodeProf.SizeCodePrimaryKey].Value) != (double)(decimal)Convert.ToDouble(outValues[i]))
								{
									SetCellValue(row.Cells[priSizeCodeProf.SizeCodePrimaryKey], (double)(decimal)Convert.ToDouble(outValues[i]));

									if (priSizeCodeProf.SizeCodePrimaryKey != cTotalColName && !Convert.ToBoolean(row.Cells["isTotalRow"].Value))
									{
										UpdateCellValue(Convert.ToInt32(chgRow.Cells["SizeCurveRID"].Value), priSizeCodeProf.SizeCodePrimaryKey, 
											Convert.ToString(row.Cells["Secondary"].Value), (double)(decimal)Convert.ToDouble(outValues[i]));
									}
								}

								i++;
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

		private void SpreadTotalToPrimary(UltraGridCell aCell)
		{
			BasicSpread spreader;
			ArrayList inValues;
			UltraGridRow chgRow;
			IDictionaryEnumerator primaryEnum;
			ArrayList outValues;
			SizeCodeProfile priSizeCodeProf;
			DataRow currRow;
			int i;
			
			try
			{
				spreader = new BasicSpread();
				inValues = new ArrayList();
				chgRow = aCell.Row;

				primaryEnum = _primarySizeList.GetEnumerator();
				currRow = _dtSizeCodeKeys.Rows.Find(Convert.ToString(chgRow.Cells["Secondary"].Value));

				while (primaryEnum.MoveNext())
				{
					priSizeCodeProf = (SizeCodeProfile)primaryEnum.Value;

					if (currRow[priSizeCodeProf.SizeCodePrimaryKey] != System.DBNull.Value && Convert.ToString(currRow[priSizeCodeProf.SizeCodePrimaryKey]) != string.Empty)
					{
						inValues.Add((double)(decimal)Convert.ToDouble(chgRow.Cells[priSizeCodeProf.SizeCodePrimaryKey].Value));
					}
				}

				spreader.ExecuteSimpleSpread((double)(decimal)Convert.ToDouble(aCell.Value), inValues, 3, out outValues);

				i = 0;

				primaryEnum = _primarySizeList.GetEnumerator();
				currRow = _dtSizeCodeKeys.Rows.Find(Convert.ToString(chgRow.Cells["Secondary"].Value));

				while (primaryEnum.MoveNext())
				{
					priSizeCodeProf = (SizeCodeProfile)primaryEnum.Value;
					if (currRow[priSizeCodeProf.SizeCodePrimaryKey] != System.DBNull.Value && Convert.ToString(currRow[priSizeCodeProf.SizeCodePrimaryKey]) != string.Empty)
					{
						if ((double)(decimal)Convert.ToDouble(chgRow.Cells[priSizeCodeProf.SizeCodePrimaryKey].Value) != (double)(decimal)Convert.ToDouble(outValues[i]))
						{
							SetCellValue(chgRow.Cells[priSizeCodeProf.SizeCodePrimaryKey], (double)(decimal)Convert.ToDouble(outValues[i]));

							if (priSizeCodeProf.SizeCodePrimaryKey != cTotalColName && !Convert.ToBoolean(chgRow.Cells["isTotalRow"].Value))
							{
								UpdateCellValue(Convert.ToInt32(chgRow.Cells["SizeCurveRID"].Value), priSizeCodeProf.SizeCodePrimaryKey, Convert.ToString(chgRow.Cells["Secondary"].Value), (double)(decimal)Convert.ToDouble(outValues[i]));
							}
						}

						i++;
					}
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		private void SpreadTotalToSecondary(UltraGridCell aCell)
		{
			BasicSpread spreader;
			ArrayList inValues;
			UltraGridRow chgRow;
			ArrayList outValues;
			DataRow currRow;
			int i;
			
			try
			{
				spreader = new BasicSpread();
				inValues = new ArrayList();
				chgRow = aCell.Row;

				foreach (UltraGridRow row in chgRow.ParentCollection)
				{
					if (!Convert.ToBoolean(row.Cells["isTotalRow"].Value))
					{
						currRow = _dtSizeCodeKeys.Rows.Find(Convert.ToString(row.Cells["Secondary"].Value));

						if (currRow[aCell.Column.Key] != System.DBNull.Value && Convert.ToString(currRow[aCell.Column.Key]) != string.Empty)
						{
							inValues.Add((double)(decimal)Convert.ToDouble(row.Cells[aCell.Column].Value));
						}
					}
				}

				spreader.ExecuteSimpleSpread((double)(decimal)Convert.ToDouble(aCell.Value), inValues, 3, out outValues);

				i = 0;

				foreach (UltraGridRow row in chgRow.ParentCollection)
				{
					if (!Convert.ToBoolean(row.Cells["isTotalRow"].Value))
					{
						currRow = _dtSizeCodeKeys.Rows.Find(Convert.ToString(row.Cells["Secondary"].Value));

						if (currRow[aCell.Column.Key] != System.DBNull.Value && Convert.ToString(currRow[aCell.Column.Key]) != string.Empty)
						{
							if ((double)(decimal)Convert.ToDouble(row.Cells[aCell.Column].Value) != (double)(decimal)Convert.ToDouble(outValues[i]))
							{
								SetCellValue(row.Cells[aCell.Column], (double)(decimal)Convert.ToDouble(outValues[i]));

								if (!Convert.ToBoolean(row.Cells["isTotalRow"].Value))
								{
									UpdateCellValue(Convert.ToInt32(chgRow.Cells["SizeCurveRID"].Value), aCell.Column.Key, Convert.ToString(row.Cells["Secondary"].Value), (double)(decimal)Convert.ToDouble(outValues[i]));
								}
							}

							i++;
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
//End Track #4041 - JScott - Long delay during cell update

		private void PreGridUpdate(UltraGrid aGrid)
		{
			try
			{
				aGrid.BeginUpdate();
//Begin Track #4041 - JScott - Long delay during cell update
//				aGrid.SuspendRowSynchronization();
//End Track #4041 - JScott - Long delay during cell update
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		private void PostGridUpdate(UltraGrid aGrid, DataTable aDataTable)
		{
			UltraGridRow activeRow;
			UltraGridCell activeCell;

			try
			{
				activeRow = aGrid.ActiveRow;
				activeCell = aGrid.ActiveCell;
				aDataTable.AcceptChanges();
				aGrid.ActiveRow = null;
				aGrid.ActiveCell = null;
				aGrid.ActiveRow = activeRow;
				aGrid.ActiveCell = activeCell;
//Begin Track #4041 - JScott - Long delay during cell update
//				aGrid.ResumeRowSynchronization();
//End Track #4041 - JScott - Long delay during cell update
				aGrid.EndUpdate();
				aGrid.UpdateData();
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		private void PostGridUpdate(UltraGrid aGrid, DataSet aDataSet)
		{
			UltraGridRow activeRow;
			UltraGridCell activeCell;

			try
			{
				activeRow = aGrid.ActiveRow;
				activeCell = aGrid.ActiveCell;
				aDataSet.AcceptChanges();
				aGrid.ActiveRow = null;
				aGrid.ActiveCell = null;
				aGrid.ActiveRow = activeRow;
				aGrid.ActiveCell = activeCell;
				aGrid.ResumeRowSynchronization();
				aGrid.EndUpdate();
				aGrid.UpdateData();
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		private bool Save()
		{
			try
			{
				if (cboSizeCurveGroup.SelectedIndex == -1 // MID Track 6438 - Size Percents Not Correct in Size Review
					|| this._currSizeCurveGrpProf.SizeCurveGroupName == null // MID Track 6438 - Size Percents Not Correct in Size Review
					|| this._currSizeCurveGrpProf.SizeCurveGroupName.Trim() == string.Empty) // MID Track 6438 - Size Percents Not Correct in Size Review
				{
					return SaveAs();
				}
				else
				{
					if (CheckValues())
					{
						SaveSizeCurveValues();
                        // BEGIN MID Track #5248 - KJohnson - Able to create a Size Curve without putting in the %'s by size and receive and error.
                        SetSizeCurveCombo();
                        // END MID Track #5248
                        ChangePending = false;
						return true;
					}
					else
					{
						return false;
					}
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		private bool SaveAs()
		{
			frmSaveAs formSaveAs;
			bool continueSave;
			bool saveCancelled;
            int curveGroupKey = Include.NoRID;
			SizeCurveGroupProfile toSizeCurveGrpProf;

			try
			{
				if (CheckValues())
				{
					formSaveAs = new frmSaveAs(SAB);
					formSaveAs.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
					formSaveAs.ShowUserGlobal = false;

					if (cboSizeCurveGroup.SelectedIndex != -1)
					{
						formSaveAs.SaveAsName = ((ComboObject)cboSizeCurveGroup.SelectedItem).Value;
					}

					continueSave = false;
					saveCancelled = false;

					while (!continueSave && !saveCancelled)
					{
						formSaveAs.ShowDialog(this);
						saveCancelled = formSaveAs.SaveCanceled;

						if (!saveCancelled)
						{
							curveGroupKey = _dlSizeCurve.GetSizeCurveGroupKey(formSaveAs.SaveAsName);

							if (curveGroupKey == Include.NoRID ||
								MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_DuplicateName), Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
							{
								continueSave = true;
							}
						}
					}

					if (!saveCancelled)
					{
						if (curveGroupKey != Include.NoRID &&
                             _currSizeCurveGrpProf.Key == curveGroupKey)
						{
                            // Begin TT#3142 - JSmith - Save As always creates a new Size Curve instead of updating an existing one
                            //toSizeCurveGrpProf = new SizeCurveGroupProfile(curveGroupKey);
                            //toSizeCurveGrpProf.DeleteSizeCurveGroup();

                            SaveSizeCurveValues();
                            SetSizeCurveCombo();
                            ChangePending = false;
                            return true;
                            // End TT#3142 - JSmith - Save As always creates a new Size Curve instead of updating an existing one
						}

                        if (curveGroupKey != Include.NoRID)
                        {
                            _currSizeCurveGrpProf.Key = curveGroupKey;
                        }
                        else
                        {
                            _currSizeCurveGrpProf.Key = Include.NoRID;
                        }
						_currSizeCurveGrpProf.SizeCurveGroupName = formSaveAs.SaveAsName;

						foreach (SizeCurveProfile sizeCurveProf in _currSizeCurveGrpProf.SizeCurveList)
						{
							sizeCurveProf.Key = Include.NoRID;
						}

						_currSizeCurveGrpProf.DefaultSizeCurve.Key = Include.NoRID;

						SaveSizeCurveValues();
						LoadSizeCurveGroupComboBox();
						cboSizeCurveGroup.SelectedIndex = cboSizeCurveGroup.Items.IndexOf(new ComboObject(_currSizeCurveGrpProf.Key, _currSizeCurveGrpProf.SizeCurveGroupName));
						SetSizeCurveCombo();

						ChangePending = false;

						return true;
					}
					else
					{
						return false;
					}
				}
				else
				{
					return false;
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		private void DeleteSizeCurveGroup()
		{
			string text;

			try
			{
				if (cboSizeCurveGroup.SelectedIndex != -1)
				{
					text = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ConfirmDelete);
					text = text.Replace("{0}", _currSizeCurveGrpProf.SizeCurveGroupName);

				    if (MessageBox.Show(text, Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    //BEGIN TT#641-MD-VStuart-Size Curve Delete Needs to use new In Use Tool
                    //{	// BEGIN MID Track #5240 - delete size curve grouop too slow
				        //if (_currSizeCurveGrpProf.SizeCurveGroupIsInUse())
				        //{
				        //    //BEGIN TT#110-MD-VStuart - In Use Tool
				        //    var allowDelete = false;
				        //    var _scgpArrayList = new ArrayList();
				        //    _scgpArrayList.Add(_currSizeCurveGrpProf.Key);
				        //    string inUseTitle =
				        //        Regex.Replace(eProfileType.SizeCurveGroup.ToString(), "((?<=[a-z])[A-Z]|[A-Z](?=[a-z]))", " $1").Trim();
				        //    DisplayInUseForm(_scgpArrayList, eProfileType.SizeCurveGroup, inUseTitle, false, out allowDelete);
				        //    if (!allowDelete)
				        //    {
				        //        //Do nothing.
				        //    }
				        //    //text = MIDText.GetText((int)eMIDTextCode.msg_DeleteFailedDataInUse);
				        //    //MessageBox.Show(text, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
				        ////END TT#110-MD-VStuart - In Use Tool						
				        //}
				        //else 
				        //// END MID Track #5240
				    {
                        //If the RID is InUse don't delete. If RID is NOT InUse go ahead and delete.
                        var emp = new SizeCurveGroupProfile(_modelRID);
                        eProfileType type = emp.ProfileType;
                        int rid = _modelRID;

                        if (!CheckInUse(type, rid, false))
                        //END   TT#641-MD-VStuart-Size Curve Delete Needs to use new In Use Tool
                        {
				            _currSizeCurveGrpProf.DeleteSizeCurveGroup();
				            ChangePending = false;
				            cboSizeCurveGroup.SelectedIndex = -1;
				            LoadSizeCurveGroupComboBox();
				        }
				    }
				    //}
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		private bool CheckValues()
		{
			try
			{
				if (!RecurseRowsAndCheckValues(ulgSizeCurve.Rows[0]))
				{
					return false;
				}

				if (ulgDefaultSizeCurve.Rows.Count > 0)
				{
					if (!RecurseRowsAndCheckValues(ulgDefaultSizeCurve.Rows[0]))
					{
						return false;
					}
				}
				else
				{
					MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_SelectSizeGroupBeforeDefiningCurve), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
					return false;
				}

				return true;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		private bool RecurseRowsAndCheckValues(UltraGridRow startRow)
		{
			UltraGridRow currRow;

			try
			{
				currRow = startRow;

				while (currRow != null)
				{
					if (currRow.Band.Key == "SizeCurveRIDRelation" || currRow.Band.Key == "DefaultSizeCurve") 
					{
						if (Convert.ToBoolean(currRow.Cells["isTotalRow"].Value))
						{
							if ((decimal)Convert.ToDouble(currRow.Cells[cTotalColName].Value) != 100)
							{
								MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_CurvePercentageMustEqual100), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
								return false;
							}
						}
						// There is only one row so no total row so check value
						else if (!Convert.ToBoolean(currRow.Cells["hasSecondarySizes"].Value))
						{
							decimal total = (decimal)Convert.ToDouble(currRow.Cells[cTotalColName].Value);
							if ((decimal)Convert.ToDouble(currRow.Cells[cTotalColName].Value) != 100)
							{
								MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_CurvePercentageMustEqual100), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
								return false;
							}
						}
					}
					else
					{
						if (currRow.HasChild(false))
						{
							if (!RecurseRowsAndCheckValues(currRow.GetChild(ChildRow.First)))
							{
								return false;
							}
						}
					}

					currRow = currRow.GetSibling(SiblingRow.Next, true, false);
				}

				return true;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		private void SaveSizeCurveValues()
		{
			try
			{
				_dlSizeCurve.OpenUpdateConnection();

				try
				{
					ulgSizeCurve.PerformAction(UltraGridAction.ExitEditMode);
					ulgDefaultSizeCurve.PerformAction(UltraGridAction.ExitEditMode);

					PreGridUpdate(ulgSizeCurve);
					PostGridUpdate(ulgSizeCurve, _dsSizeCurve);
					PreGridUpdate(ulgDefaultSizeCurve);
					PostGridUpdate(ulgDefaultSizeCurve, _dtDefaultSizeCurve);
					
					// BEGIN MID Track #5268 - Size Curve Add/Update slow
					//_currSizeCurveGrpProf.WriteSizeCurveGroup(SAB);  
					_currSizeCurveGrpProf.WriteSizeCurveGroup(SAB, false); 
					// END MID Track #5268
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
				finally
				{
					_dlSizeCurve.CloseUpdateConnection();
				}

				ChangePending = false;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

//Begin Track #4041 - JScott - Long delay during cell update
		private void SetCellValue(UltraGridCell aCell, object aValue)
		{
			try
			{
				_systemCellUpdate = true;
				aCell.Value = aValue;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
			finally
			{
				_systemCellUpdate = false;
			}
		}

//End Track #4041 - JScott - Long delay during cell update
		private void ClearCurveMenuItemEventHandlers()
		{
			try
			{
				foreach (MenuItem menuItem in _sizeCurveMenuItemList)
				{
					menuItem.Click -= new System.EventHandler(this.mniSizeCurve_Click);
				}

				_sizeCurveMenuItemList.Clear();
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

        private void ulgDefaultSizeCurve_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            //Begin TT#169 - JSmith - enter max of 1000 and receive 'must be integer'
            //MIDRetail.Windows.Controls.UltraGridLayoutDefaults ugld = new MIDRetail.Windows.Controls.UltraGridLayoutDefaults();
            //ugld.ApplyDefaults(e);
            MIDRetail.Windows.Controls.UltraGridLayoutDefaults ugld = new MIDRetail.Windows.Controls.UltraGridLayoutDefaults(ErrorImage);
            // Begin TT#1164 - JSmith - When going in to some methods or Admin features the columns are appearing too wide.
            //ugld.ApplyDefaults((Infragistics.Win.UltraWinGrid.UltraGrid)sender, e, true, 3, true);
            ugld.ApplyDefaults((Infragistics.Win.UltraWinGrid.UltraGrid)sender, e, true, 3, false);
            // End TT#1164
            //End TT#169
        }

        private void ulgSizeCurve_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            //Begin TT#169 - JSmith - enter max of 1000 and receive 'must be integer'
            //MIDRetail.Windows.Controls.UltraGridLayoutDefaults ugld = new MIDRetail.Windows.Controls.UltraGridLayoutDefaults();
            //ugld.ApplyDefaults(e);
            MIDRetail.Windows.Controls.UltraGridLayoutDefaults ugld = new MIDRetail.Windows.Controls.UltraGridLayoutDefaults(ErrorImage);
            // Begin TT#1164 - JSmith - When going in to some methods or Admin features the columns are appearing too wide.
            //ugld.ApplyDefaults((Infragistics.Win.UltraWinGrid.UltraGrid)sender, e, true, 3, true);
            ugld.ApplyDefaults((Infragistics.Win.UltraWinGrid.UltraGrid)sender, e, true, 3, false);
            // End TT#1164
            //End TT#169
        }

		#region Size Dropdown Filter
		// BEGIN MID Track #4396 - Justin Bolles - Size Dropdown Filter
		protected void DisplayPictureBoxImages()
		{
			DisplayPictureBoxImage(picBoxGroup);
			DisplayPictureBoxImage(picBoxCurve);
		}
		
		protected void SetPictureBoxTags()
		{
			picBoxGroup.Tag= SAB.ApplicationServerSession.GlobalOptions.SizeAlternateCharMask;
			picBoxCurve.Tag = SAB.ApplicationServerSession.GlobalOptions.SizeCurveCharMask;
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

		private void picBoxFilter_Click(object sender, System.EventArgs e)
		{
			try
			{	
				string enteredMask = string.Empty;
				bool caseSensitive = false;
				PictureBox picBox = (PictureBox)sender;

				if (CharMaskFromDialogOK(picBox, ref enteredMask, ref caseSensitive))
				{
					//MessageBox.Show("Filter selection process not yet available");
					switch (picBox.Name)
					{
						case "picBoxGroup":
							this.BindSizeGroupComboBox(true , enteredMask, caseSensitive);
							break;
						
						case "picBoxCurve":
							BindSizeCurveComboBox(true , enteredMask, caseSensitive);
							break;
					}
				}
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}

		private void picBoxFilter_MouseHover(object sender, System.EventArgs e)
		{
			try
			{
				string message = MIDText.GetTextOnly((int)eMIDTextCode.tt_ClickToFilterDropDown);
				ToolTip.Active = true; 
				ToolTip.SetToolTip((System.Windows.Forms.Control)sender, message);
			}
			catch( Exception exception )
			{
				HandleException(exception);
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
							errMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_al_FilterGlobalOptionMismatch);
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

		protected void SetMaskedComboBoxesEnabled()
		{
			if (SAB.ApplicationServerSession.GlobalOptions.SizeCurveCharMask != string.Empty)
			{
				this.cboSizeCurveGroup.Enabled = false;
			}

			if (SAB.ApplicationServerSession.GlobalOptions.SizeGroupCharMask!= string.Empty)
			{
				this.cboSizeGroup.Enabled = false;
			}

			picBoxCurve.Enabled = true;		// MID Track #5256 - If security View only, can't select Size Curves when mask 
											// exists becuse of above check, but picBox is disabled from SetReadOnly; 
											// this overrides SetReadOnly
		}

        private void btnSave_EnabledChanged(object sender, EventArgs e)
        {
            if (btnSave.Enabled)
            {
                EnableMenuItem(this, eMIDMenuItem.FileSave);
                EnableMenuItem(this, eMIDMenuItem.FileSaveAs);
            }
            else
            {
                DisableMenuItem(this, eMIDMenuItem.FileSave);
            }
        }

        private void btnSaveAs_EnabledChanged(object sender, EventArgs e)
        {
            if (btnSaveAs.Enabled)
            {
                EnableMenuItem(this, eMIDMenuItem.FileSaveAs);
            }
            else
            {
                DisableMenuItem(this, eMIDMenuItem.FileSaveAs);
            }
        }

        private void btnDelete_EnabledChanged(object sender, EventArgs e)
        {
            if (btnDelete.Enabled)
            {
                EnableMenuItem(this, eMIDMenuItem.EditDelete);
            }
            else
            {
                DisableMenuItem(this, eMIDMenuItem.EditDelete);
            }
        }

        private void btnNew_EnabledChanged(object sender, EventArgs e)
        {
            if (btnNew.Enabled)
            {
                EnableMenuItem(this, eMIDMenuItem.FileNew);
            }
            else
            {
                DisableMenuItem(this, eMIDMenuItem.FileNew);
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
		// END MID Track #4396
		#endregion

        private void cboStoreAttribute_MIDComboBoxPropertiesChangedEvent(object source, MIDComboBoxPropertiesChangedEventArgs args)
        {
            this.cboStoreAttribute_SelectionChangeCommitted(source, new EventArgs());
        }
        private void cboSizeCurveGroup_MIDComboBoxPropertiesChangedEvent(object source, MIDComboBoxPropertiesChangedEventArgs args)
        {
            this.cboSizeCurveGroup_SelectionChangeCommitted(source, new EventArgs());
        }
        private void cboSizeGroup_MIDComboBoxPropertiesChangedEvent(object source, MIDComboBoxPropertiesChangedEventArgs args)
        {
            this.cboSizeGroup_SelectionChangeCommitted(source, new EventArgs());
        }

        ////BEGIN TT#110-MD-VStuart - In Use Tool
        //public virtual void ShowInUse()
        //{
        //    throw new Exception("ShowInUse not overridden");
        //}
        ////END TT#110-MD-VStuart - In Use Tool

        //BEGIN TT#110-MD-VStuart - In Use Tool
        public void ShowInUse()
        {
            var emp = new SizeCurveGroupProfile(_modelRID);
            eProfileType type = emp.ProfileType;
            int rid = _modelRID;
            ShowInUse(type, rid, inQuiry2);
        }
        //END TT#110-MD-VStuart - In Use Tool

        //BEGIN TT#110-MD-VStuart - In Use Tool
        /// <summary>
        /// Shows the In Use dialog.
        /// </summary>
        /// <param name="type"></param> The model eProfileType in question.
        /// <param name="rid"></param> The model in question. 
        /// <param name="inQuiry2"></param> False if this is a mandatory request.
        internal void ShowInUse(eProfileType type, int rid, bool inQuiry2)
        {
            if (rid != Include.NoRID)
            {
                _ridList = new ArrayList();
                _ridList.Add(rid);
                //string inUseTitle = Regex.Replace(type.ToString(), "((?<=[a-z])[A-Z]|[A-Z](?=[a-z]))", " $1").Trim();
                string inUseTitle = InUseUtility.GetInUseTitleFromProfileType(type); //TT#4304 -jsobek -Store Characteristic In Use not being reported on Store Filters
                bool display = true;
                bool inQuiry = inQuiry2;
                DisplayInUseForm(_ridList, type, inUseTitle, ref display, inQuiry);
            }
        }
        ////END TT#110-MD-VStuart - In Use Tool

        //BEGIN TT#110-MD-VStuart - In Use Tool
        private void btnInUse_Click(object sender, EventArgs e)
        {
            inQuiry2 = true;
            ShowInUse();
        }

		public bool inQuiry2
        {
            get { return _isInQuiry; }
            set { _isInQuiry = value; }
        }
        //END TT#110-MD-VStuart - In Use Tool

	}
}
