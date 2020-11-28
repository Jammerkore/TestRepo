// BEGIN MID Track #5170 - JSmith - Model enhancements
// Too many lines changed to mark.  Use SCM Compare for details.
//End Track #5170
using System;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Collections;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Diagnostics;
using C1.Win.C1FlexGrid;

using MIDRetail.DataCommon;
using MIDRetail.Data;
using MIDRetail.Common;
using MIDRetail.Business;
using MIDRetail.Windows.Controls;

namespace MIDRetail.Windows
{
    public class frmSizeGroup : MIDFormBase
    {

        private SessionAddressBlock _SAB;
//		private MIDRetail.Business.Allocation.SizeGroupProfile _sgp = null;
        private SizeGroupList _sgl = null;
        private bool _ignoreFlag = true;
        private bool _verifySuccess = false;
//		private bool _editHasOccurred = false;
        private int _groupRID = -1;
        private string _newText;
        private string _noSecondarySizeStr;
        private string _noSizeDimensionLbl;
        private DragState _dragState;
        private int _gTopMouseCol;
        private int _gTopMouseRow;
        private int _dragStartColumn;
        private int _gLeftMouseCol;
        private int _gLeftMouseRow;
        private int _dragStartRow;

        private SizeGroupProfile _currSizeGroupProfile = null; // BEGIN MID Track #4970 - add locking
        private bool _showSaveMsg = false;
        private int _modelRID = Include.NoRID;
        private string _currGroupName = string.Empty;
        private bool _modelLocked = false;
        private int _currModelIndex = -1; // END MID Track #4970

        private C1.Win.C1FlexGrid.C1FlexGrid gMiddle;
        private C1.Win.C1FlexGrid.C1FlexGrid gLeft;
        private System.Windows.Forms.Panel pnlParent;
        private System.Windows.Forms.Panel pnlLeft;
        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.Panel pnlTopSpacer1;
        private C1.Win.C1FlexGrid.C1FlexGrid gTop;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnVerify;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rdbLike;
        private System.Windows.Forms.RadioButton rdbBegins;
        private System.Windows.Forms.RadioButton rdbExact;
        private System.Windows.Forms.ContextMenu rowGridMenu;
        private System.Windows.Forms.ContextMenu colGridMenu;
        private System.Windows.Forms.Button btnClose;
        protected System.Windows.Forms.PictureBox picBoxGroup;
        private System.Windows.Forms.Label lblSizeGroupDesc;
        private System.Windows.Forms.Label lblSizeGroupName;
        private System.Windows.Forms.Label lblProductCategory;
        private System.Windows.Forms.MenuItem mnuItemRowInsert;
        private System.Windows.Forms.MenuItem mnuItemRowDelete;
        private System.Windows.Forms.MenuItem mnuItemColInsert;
        private System.Windows.Forms.MenuItem mnuItemColDelete;
        private Button btnSaveAs;
        private Button btnNew;
        private MIDComboBoxEnh cboGroupName;
        private MIDComboBoxEnh cboProductCat;
        private Button btnInUse;

        //BEGIN TT#110-MD-VStuart - In Use Tool
        private bool _display;
        private ArrayList _ridList;
        protected bool _isInQuiry = true;
        //END TT#110-MD-VStuart - In Use Tool


        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;



        /// <summary>
        /// Standard constructor
        /// </summary>
        public frmSizeGroup(SessionAddressBlock sab) : base(sab)
        {
            InitializeComponent();

            _SAB = sab;
            FunctionSecurity =
                _SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AdminSizeGroups);
            DisplayPictureBoxImages();
            SetPictureBoxTags();

            mnuItemRowInsert = new MenuItem(MIDText.GetTextOnly(eMIDTextCode.lbl_Insert));
            MenuItem mnuItemRowInsertBefore = new MenuItem(MIDText.GetTextOnly(eMIDTextCode.lbl_Insert_Before));
            MenuItem mnuItemRowInsertAfter = new MenuItem(MIDText.GetTextOnly(eMIDTextCode.lbl_Insert_after));
            mnuItemRowDelete = new MenuItem(MIDText.GetTextOnly(eMIDTextCode.lbl_Delete));
            rowGridMenu.MenuItems.Add(mnuItemRowInsert);
            mnuItemRowInsert.MenuItems.Add(mnuItemRowInsertBefore);
            mnuItemRowInsert.MenuItems.Add(mnuItemRowInsertAfter);
            rowGridMenu.MenuItems.Add(mnuItemRowDelete);
            mnuItemRowInsert.Click += new EventHandler(mnuItemRowInsert_Click);
            mnuItemRowInsertBefore.Click += new EventHandler(mnuItemRowInsertBefore_Click);
            mnuItemRowInsertAfter.Click += new EventHandler(mnuItemRowInsertAfter_Click);
            mnuItemRowDelete.Click += new EventHandler(mnuItemRowDelete_Click);

            mnuItemColInsert = new MenuItem(MIDText.GetTextOnly(eMIDTextCode.lbl_Insert));
            MenuItem mnuItemColInsertBefore = new MenuItem(MIDText.GetTextOnly(eMIDTextCode.lbl_Insert_Before));
            MenuItem mnuItemColInsertAfter = new MenuItem(MIDText.GetTextOnly(eMIDTextCode.lbl_Insert_after));
            mnuItemColDelete = new MenuItem(MIDText.GetTextOnly(eMIDTextCode.lbl_Delete));
            colGridMenu.MenuItems.Add(mnuItemColInsert);
            mnuItemColInsert.MenuItems.Add(mnuItemColInsertBefore);
            mnuItemColInsert.MenuItems.Add(mnuItemColInsertAfter);
            colGridMenu.MenuItems.Add(mnuItemColDelete);
            mnuItemColInsert.Click += new EventHandler(mnuItemColInsert_Click);
            mnuItemColInsertBefore.Click += new EventHandler(mnuItemColInsertBefore_Click);
            mnuItemColInsertAfter.Click += new EventHandler(mnuItemColInsertAfter_Click);
            mnuItemColDelete.Click += new EventHandler(mnuItemColDelete_Click);
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

                this.gMiddle.AfterScroll -= new C1.Win.C1FlexGrid.RangeEventHandler(this.gMiddle_AfterScroll);
                this.gMiddle.AfterEdit -= new C1.Win.C1FlexGrid.RowColEventHandler(this.gMiddle_AfterEdit);
                this.gLeft.DragOver -= new System.Windows.Forms.DragEventHandler(this.gLeft_DragOver);
                this.gLeft.DragEnter -= new System.Windows.Forms.DragEventHandler(this.gLeft_DragEnter);
                this.gLeft.MouseMove -= new System.Windows.Forms.MouseEventHandler(this.gLeft_MouseMove);
                this.gLeft.QueryContinueDrag -=
                    new System.Windows.Forms.QueryContinueDragEventHandler(this.gLeft_QueryContinueDrag);
                this.gLeft.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.gLeft_MouseDown);
                this.gLeft.MouseUp -= new System.Windows.Forms.MouseEventHandler(this.gLeft_MouseUp);
                this.gLeft.BeforeResizeRow -= new C1.Win.C1FlexGrid.RowColEventHandler(this.gLeft_BeforeResizeRow);
                this.gLeft.AfterScroll -= new C1.Win.C1FlexGrid.RangeEventHandler(this.gLeft_AfterScroll);
                this.gLeft.AfterEdit -= new C1.Win.C1FlexGrid.RowColEventHandler(this.gLeft_AfterEdit);
                this.gLeft.DragDrop -= new System.Windows.Forms.DragEventHandler(this.gLeft_DragDrop);
                this.gLeft.AfterResizeRow -= new C1.Win.C1FlexGrid.RowColEventHandler(this.gLeft_AfterResizeRow);
                this.gTop.DragOver -= new System.Windows.Forms.DragEventHandler(this.gTop_DragOver);
                this.gTop.DragEnter -= new System.Windows.Forms.DragEventHandler(this.gTop_DragEnter);
                this.gTop.MouseMove -= new System.Windows.Forms.MouseEventHandler(this.gTop_MouseMove);
                this.gTop.AfterResizeColumn -= new C1.Win.C1FlexGrid.RowColEventHandler(this.gTop_AfterResizeColumn);
                this.gTop.QueryContinueDrag -=
                    new System.Windows.Forms.QueryContinueDragEventHandler(this.gTop_QueryContinueDrag);
                this.gTop.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.gTop_MouseDown);
                this.gTop.BeforeResizeColumn -= new C1.Win.C1FlexGrid.RowColEventHandler(this.gTop_BeforeResizeColumn);
                this.gTop.MouseUp -= new System.Windows.Forms.MouseEventHandler(this.gTop_MouseUp);
                this.gTop.AfterScroll -= new C1.Win.C1FlexGrid.RangeEventHandler(this.gTop_AfterScroll);
                this.gTop.AfterEdit -= new C1.Win.C1FlexGrid.RowColEventHandler(this.gTop_AfterEdit);
                this.gTop.DragDrop -= new System.Windows.Forms.DragEventHandler(this.gTop_DragDrop);
                this.txtDescription.KeyPress -=
                    new System.Windows.Forms.KeyPressEventHandler(this.txtDescription_KeyPress);
                this.cboGroupName.SelectedIndexChanged -= new System.EventHandler(this.cboGroupName_SelectedIndexChanged);
                this.cboGroupName.Validating -=
                    new System.ComponentModel.CancelEventHandler(this.cboGroupName_Validating);
                this.btnVerify.Click -= new System.EventHandler(this.btnVerify_Click);
                this.btnClear.Click -= new System.EventHandler(this.btnClear_Click);
                this.btnDelete.Click -= new System.EventHandler(this.btnDelete_Click);
                this.btnClose.Click -= new System.EventHandler(this.btnClose_Click);
                this.btnSave.Click -= new System.EventHandler(this.btnSave_Click);
                this.Load -= new System.EventHandler(this.SizeGroup_Load);
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources =
                new System.ComponentModel.ComponentResourceManager(typeof (frmSizeGroup));
            this.pnlParent = new System.Windows.Forms.Panel();
            this.gMiddle = new C1.Win.C1FlexGrid.C1FlexGrid();
            this.pnlLeft = new System.Windows.Forms.Panel();
            this.gLeft = new C1.Win.C1FlexGrid.C1FlexGrid();
            this.rowGridMenu = new System.Windows.Forms.ContextMenu();
            this.pnlTop = new System.Windows.Forms.Panel();
            this.gTop = new C1.Win.C1FlexGrid.C1FlexGrid();
            this.colGridMenu = new System.Windows.Forms.ContextMenu();
            this.pnlTopSpacer1 = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.cboProductCat = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this.cboGroupName = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this.picBoxGroup = new System.Windows.Forms.PictureBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rdbExact = new System.Windows.Forms.RadioButton();
            this.rdbBegins = new System.Windows.Forms.RadioButton();
            this.rdbLike = new System.Windows.Forms.RadioButton();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.lblSizeGroupDesc = new System.Windows.Forms.Label();
            this.lblProductCategory = new System.Windows.Forms.Label();
            this.lblSizeGroupName = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnNew = new System.Windows.Forms.Button();
            this.btnSaveAs = new System.Windows.Forms.Button();
            this.btnVerify = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnInUse = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize) (this.utmMain)).BeginInit();
            this.pnlParent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) (this.gMiddle)).BeginInit();
            this.pnlLeft.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) (this.gLeft)).BeginInit();
            this.pnlTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) (this.gTop)).BeginInit();
            this.pnlTopSpacer1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) (this.picBoxGroup)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // utmMain
            // 
            this.utmMain.MenuSettings.ForceSerialization = true;
            this.utmMain.ToolbarSettings.ForceSerialization = true;
            // 
            // pnlParent
            // 
            this.pnlParent.Controls.Add(this.gMiddle);
            this.pnlParent.Controls.Add(this.pnlLeft);
            this.pnlParent.Controls.Add(this.pnlTop);
            this.pnlParent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlParent.Location = new System.Drawing.Point(0, 147);
            this.pnlParent.Name = "pnlParent";
            this.pnlParent.Size = new System.Drawing.Size(715, 288);
            this.pnlParent.TabIndex = 0;
            // 
            // gMiddle
            // 
            this.gMiddle.AllowDragging = C1.Win.C1FlexGrid.AllowDraggingEnum.Both;
            this.gMiddle.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None;
            this.gMiddle.Anchor =
                ((System.Windows.Forms.AnchorStyles)
                 ((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                    | System.Windows.Forms.AnchorStyles.Left)
                   | System.Windows.Forms.AnchorStyles.Right)));
            this.gMiddle.ColumnInfo = resources.GetString("gMiddle.ColumnInfo");
            this.gMiddle.DropMode = C1.Win.C1FlexGrid.DropModeEnum.Manual;
            this.gMiddle.HighLight = C1.Win.C1FlexGrid.HighLightEnum.WithFocus;
            this.gMiddle.KeyActionTab = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcross;
            this.gMiddle.Location = new System.Drawing.Point(128, 40);
            this.gMiddle.Name = "gMiddle";
            this.gMiddle.Rows.DefaultSize = 17;
            this.gMiddle.Rows.Fixed = 0;
            this.gMiddle.Size = new System.Drawing.Size(587, 247);
            this.gMiddle.StyleInfo = resources.GetString("gMiddle.StyleInfo");
            this.gMiddle.TabIndex = 6;
            this.gMiddle.AfterScroll += new C1.Win.C1FlexGrid.RangeEventHandler(this.gMiddle_AfterScroll);
            this.gMiddle.BeforeEdit += new C1.Win.C1FlexGrid.RowColEventHandler(this.gMiddle_BeforeEdit);
            this.gMiddle.AfterEdit += new C1.Win.C1FlexGrid.RowColEventHandler(this.gMiddle_AfterEdit);
            // 
            // pnlLeft
            // 
            this.pnlLeft.Controls.Add(this.gLeft);
            this.pnlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlLeft.Location = new System.Drawing.Point(0, 40);
            this.pnlLeft.Name = "pnlLeft";
            this.pnlLeft.Size = new System.Drawing.Size(128, 248);
            this.pnlLeft.TabIndex = 3;
            // 
            // gLeft
            // 
            this.gLeft.AllowDelete = true;
            this.gLeft.AllowDragging = C1.Win.C1FlexGrid.AllowDraggingEnum.Rows;
            this.gLeft.AllowResizing = C1.Win.C1FlexGrid.AllowResizingEnum.Rows;
            this.gLeft.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None;
            this.gLeft.Anchor =
                ((System.Windows.Forms.AnchorStyles)
                 (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                   | System.Windows.Forms.AnchorStyles.Left)));
            this.gLeft.ColumnInfo = resources.GetString("gLeft.ColumnInfo");
            this.gLeft.ContextMenu = this.rowGridMenu;
            this.gLeft.DropMode = C1.Win.C1FlexGrid.DropModeEnum.Manual;
            this.gLeft.ExtendLastCol = true;
            this.gLeft.HighLight = C1.Win.C1FlexGrid.HighLightEnum.WithFocus;
            this.gLeft.Location = new System.Drawing.Point(0, 0);
            this.gLeft.Name = "gLeft";
            this.gLeft.Rows.DefaultSize = 17;
            this.gLeft.Rows.Fixed = 0;
            this.gLeft.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.gLeft.Size = new System.Drawing.Size(128, 248);
            this.gLeft.StyleInfo = resources.GetString("gLeft.StyleInfo");
            this.gLeft.TabIndex = 5;
            this.gLeft.BeforeResizeRow += new C1.Win.C1FlexGrid.RowColEventHandler(this.gLeft_BeforeResizeRow);
            this.gLeft.AfterResizeRow += new C1.Win.C1FlexGrid.RowColEventHandler(this.gLeft_AfterResizeRow);
            this.gLeft.AfterScroll += new C1.Win.C1FlexGrid.RangeEventHandler(this.gLeft_AfterScroll);
            this.gLeft.BeforeEdit += new C1.Win.C1FlexGrid.RowColEventHandler(this.gLeft_BeforeEdit);
            this.gLeft.AfterEdit += new C1.Win.C1FlexGrid.RowColEventHandler(this.gLeft_AfterEdit);
            this.gLeft.BeforeAddRow += new C1.Win.C1FlexGrid.RowColEventHandler(this.gLeft_BeforeAddRow);
            this.gLeft.BeforeDeleteRow += new C1.Win.C1FlexGrid.RowColEventHandler(this.gLeft_BeforeDeleteRow);
            this.gLeft.DragDrop += new System.Windows.Forms.DragEventHandler(this.gLeft_DragDrop);
            this.gLeft.DragEnter += new System.Windows.Forms.DragEventHandler(this.gLeft_DragEnter);
            this.gLeft.DragOver += new System.Windows.Forms.DragEventHandler(this.gLeft_DragOver);
            this.gLeft.QueryContinueDrag +=
                new System.Windows.Forms.QueryContinueDragEventHandler(this.gLeft_QueryContinueDrag);
            this.gLeft.MouseDown += new System.Windows.Forms.MouseEventHandler(this.gLeft_MouseDown);
            this.gLeft.MouseMove += new System.Windows.Forms.MouseEventHandler(this.gLeft_MouseMove);
            this.gLeft.MouseUp += new System.Windows.Forms.MouseEventHandler(this.gLeft_MouseUp);
            // 
            // pnlTop
            // 
            this.pnlTop.Controls.Add(this.gTop);
            this.pnlTop.Controls.Add(this.pnlTopSpacer1);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Location = new System.Drawing.Point(0, 0);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Size = new System.Drawing.Size(715, 40);
            this.pnlTop.TabIndex = 2;
            // 
            // gTop
            // 
            this.gTop.AllowDelete = true;
            this.gTop.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None;
            this.gTop.ColumnInfo = resources.GetString("gTop.ColumnInfo");
            this.gTop.ContextMenu = this.colGridMenu;
            this.gTop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gTop.DropMode = C1.Win.C1FlexGrid.DropModeEnum.Manual;
            this.gTop.HighLight = C1.Win.C1FlexGrid.HighLightEnum.WithFocus;
            this.gTop.KeyActionTab = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcross;
            this.gTop.Location = new System.Drawing.Point(128, 0);
            this.gTop.Name = "gTop";
            this.gTop.Rows.DefaultSize = 17;
            this.gTop.Rows.Fixed = 0;
            this.gTop.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.gTop.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Column;
            this.gTop.Size = new System.Drawing.Size(587, 40);
            this.gTop.StyleInfo = resources.GetString("gTop.StyleInfo");
            this.gTop.TabIndex = 7;
            this.gTop.BeforeResizeColumn += new C1.Win.C1FlexGrid.RowColEventHandler(this.gTop_BeforeResizeColumn);
            this.gTop.AfterResizeColumn += new C1.Win.C1FlexGrid.RowColEventHandler(this.gTop_AfterResizeColumn);
            this.gTop.AfterScroll += new C1.Win.C1FlexGrid.RangeEventHandler(this.gTop_AfterScroll);
            this.gTop.BeforeEdit += new C1.Win.C1FlexGrid.RowColEventHandler(this.gTop_BeforeEdit);
            this.gTop.AfterEdit += new C1.Win.C1FlexGrid.RowColEventHandler(this.gTop_AfterEdit);
            this.gTop.DragDrop += new System.Windows.Forms.DragEventHandler(this.gTop_DragDrop);
            this.gTop.DragEnter += new System.Windows.Forms.DragEventHandler(this.gTop_DragEnter);
            this.gTop.DragOver += new System.Windows.Forms.DragEventHandler(this.gTop_DragOver);
            this.gTop.QueryContinueDrag +=
                new System.Windows.Forms.QueryContinueDragEventHandler(this.gTop_QueryContinueDrag);
            this.gTop.MouseDown += new System.Windows.Forms.MouseEventHandler(this.gTop_MouseDown);
            this.gTop.MouseMove += new System.Windows.Forms.MouseEventHandler(this.gTop_MouseMove);
            this.gTop.MouseUp += new System.Windows.Forms.MouseEventHandler(this.gTop_MouseUp);
            // 
            // pnlTopSpacer1
            // 
            this.pnlTopSpacer1.Controls.Add(this.label4);
            this.pnlTopSpacer1.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlTopSpacer1.Location = new System.Drawing.Point(0, 0);
            this.pnlTopSpacer1.Name = "pnlTopSpacer1";
            this.pnlTopSpacer1.Size = new System.Drawing.Size(128, 40);
            this.pnlTopSpacer1.TabIndex = 3;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(8, 16);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(88, 20);
            this.label4.TabIndex = 0;
            this.label4.Text = "Width \\ Size";
            this.label4.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.cboProductCat);
            this.panel1.Controls.Add(this.cboGroupName);
            this.panel1.Controls.Add(this.picBoxGroup);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Controls.Add(this.txtDescription);
            this.panel1.Controls.Add(this.lblSizeGroupDesc);
            this.panel1.Controls.Add(this.lblProductCategory);
            this.panel1.Controls.Add(this.lblSizeGroupName);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(715, 147);
            this.panel1.TabIndex = 1;
            // 
            // cboProductCat
            // 
            this.cboProductCat.AutoAdjust = true;
            this.cboProductCat.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboProductCat.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboProductCat.DataSource = null;
            this.cboProductCat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown;
            this.cboProductCat.DropDownWidth = 324;
            this.cboProductCat.FormattingEnabled = false;
            this.cboProductCat.IgnoreFocusLost = true;
            this.cboProductCat.ItemHeight = 13;
            this.cboProductCat.Location = new System.Drawing.Point(142, 57);
            this.cboProductCat.Margin = new System.Windows.Forms.Padding(0);
            this.cboProductCat.MaxDropDownItems = 25;
            this.cboProductCat.Name = "cboProductCat";
            this.cboProductCat.Size = new System.Drawing.Size(270, 21);
            this.cboProductCat.TabIndex = 1;
            this.cboProductCat.Tag = null;
            // 
            // cboGroupName
            // 
            this.cboGroupName.AutoAdjust = true;
            this.cboGroupName.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboGroupName.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboGroupName.DataSource = null;
            this.cboGroupName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown;
            this.cboGroupName.DropDownWidth = 172;
            this.cboGroupName.FormattingEnabled = false;
            this.cboGroupName.IgnoreFocusLost = true;   //TT#2856 - MD - Creating Size Group with codes - RBeck
            this.cboGroupName.ItemHeight = 13;
            this.cboGroupName.Location = new System.Drawing.Point(142, 22);
            this.cboGroupName.Margin = new System.Windows.Forms.Padding(0);
            this.cboGroupName.MaxDropDownItems = 25;
            this.cboGroupName.Name = "cboGroupName";
            this.cboGroupName.Size = new System.Drawing.Size(143, 20);
            this.cboGroupName.TabIndex = 0;
            this.cboGroupName.Tag = null;
            this.cboGroupName.SelectedIndexChanged += new System.EventHandler(this.cboGroupName_SelectedIndexChanged);
            this.cboGroupName.Validating += new System.ComponentModel.CancelEventHandler(this.cboGroupName_Validating);
            // 
            // picBoxGroup
            // 
            this.picBoxGroup.Location = new System.Drawing.Point(114, 22);
            this.picBoxGroup.Name = "picBoxGroup";
            this.picBoxGroup.Size = new System.Drawing.Size(20, 20);
            this.picBoxGroup.TabIndex = 56;
            this.picBoxGroup.TabStop = false;
            this.picBoxGroup.Click += new System.EventHandler(this.picBox_Click);
            this.picBoxGroup.MouseHover += new System.EventHandler(this.picBoxFilter_MouseHover);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rdbExact);
            this.groupBox1.Controls.Add(this.rdbBegins);
            this.groupBox1.Controls.Add(this.rdbLike);
            this.groupBox1.Location = new System.Drawing.Point(472, 8);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(96, 90);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Verify Criteria";
            // 
            // rdbExact
            // 
            this.rdbExact.Location = new System.Drawing.Point(16, 64);
            this.rdbExact.Name = "rdbExact";
            this.rdbExact.Size = new System.Drawing.Size(64, 24);
            this.rdbExact.TabIndex = 2;
            this.rdbExact.Text = "Exact";
            // 
            // rdbBegins
            // 
            this.rdbBegins.Location = new System.Drawing.Point(16, 40);
            this.rdbBegins.Name = "rdbBegins";
            this.rdbBegins.Size = new System.Drawing.Size(64, 24);
            this.rdbBegins.TabIndex = 1;
            this.rdbBegins.Text = "Start";
            // 
            // rdbLike
            // 
            this.rdbLike.Location = new System.Drawing.Point(16, 16);
            this.rdbLike.Name = "rdbLike";
            this.rdbLike.Size = new System.Drawing.Size(64, 24);
            this.rdbLike.TabIndex = 0;
            this.rdbLike.Text = "Like";
            // 
            // txtDescription
            // 
            this.txtDescription.Location = new System.Drawing.Point(142, 99);
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(426, 20);
            this.txtDescription.TabIndex = 5;
            this.txtDescription.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtDescription_KeyPress);
            // 
            // lblSizeGroupDesc
            // 
            this.lblSizeGroupDesc.Location = new System.Drawing.Point(8, 99);
            this.lblSizeGroupDesc.Name = "lblSizeGroupDesc";
            this.lblSizeGroupDesc.Size = new System.Drawing.Size(129, 22);
            this.lblSizeGroupDesc.TabIndex = 4;
            this.lblSizeGroupDesc.Text = "Size Group Desciption";
            // 
            // lblProductCategory
            // 
            this.lblProductCategory.Location = new System.Drawing.Point(8, 58);
            this.lblProductCategory.Name = "lblProductCategory";
            this.lblProductCategory.Size = new System.Drawing.Size(100, 23);
            this.lblProductCategory.TabIndex = 3;
            this.lblProductCategory.Text = "Product Category";
            // 
            // lblSizeGroupName
            // 
            this.lblSizeGroupName.Location = new System.Drawing.Point(8, 22);
            this.lblSizeGroupName.Name = "lblSizeGroupName";
            this.lblSizeGroupName.Size = new System.Drawing.Size(100, 20);
            this.lblSizeGroupName.TabIndex = 2;
            this.lblSizeGroupName.Text = "Size Group Name";
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel2.Controls.Add(this.btnInUse);
            this.panel2.Controls.Add(this.btnNew);
            this.panel2.Controls.Add(this.btnSaveAs);
            this.panel2.Controls.Add(this.btnVerify);
            this.panel2.Controls.Add(this.btnClear);
            this.panel2.Controls.Add(this.btnDelete);
            this.panel2.Controls.Add(this.btnClose);
            this.panel2.Controls.Add(this.btnSave);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 435);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(715, 48);
            this.panel2.TabIndex = 2;
            // 
            // btnNew
            // 
            this.btnNew.Anchor =
                ((System.Windows.Forms.AnchorStyles)
                 ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnNew.Location = new System.Drawing.Point(565, 12);
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(65, 23);
            this.btnNew.TabIndex = 21;
            this.btnNew.Text = "New";
            this.btnNew.EnabledChanged += new System.EventHandler(this.btnNew_EnabledChanged);
            this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
            // 
            // btnSaveAs
            // 
            this.btnSaveAs.Anchor =
                ((System.Windows.Forms.AnchorStyles)
                 ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSaveAs.Location = new System.Drawing.Point(423, 12);
            this.btnSaveAs.Name = "btnSaveAs";
            this.btnSaveAs.Size = new System.Drawing.Size(65, 23);
            this.btnSaveAs.TabIndex = 20;
            this.btnSaveAs.Text = "Save As";
            this.btnSaveAs.EnabledChanged += new System.EventHandler(this.btnSaveAs_EnabledChanged);
            this.btnSaveAs.Click += new System.EventHandler(this.btnSaveAs_Click);
            // 
            // btnVerify
            // 
            this.btnVerify.Anchor =
                ((System.Windows.Forms.AnchorStyles)
                 ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnVerify.Location = new System.Drawing.Point(219, 12);
            this.btnVerify.Name = "btnVerify";
            this.btnVerify.Size = new System.Drawing.Size(65, 23);
            this.btnVerify.TabIndex = 19;
            this.btnVerify.Text = "Verify";
            this.btnVerify.Click += new System.EventHandler(this.btnVerify_Click);
            // 
            // btnClear
            // 
            this.btnClear.Anchor =
                ((System.Windows.Forms.AnchorStyles)
                 ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClear.Location = new System.Drawing.Point(144, 12);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(65, 23);
            this.btnClear.TabIndex = 18;
            this.btnClear.Text = "Clear";
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor =
                ((System.Windows.Forms.AnchorStyles)
                 ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDelete.Location = new System.Drawing.Point(494, 12);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(65, 23);
            this.btnDelete.TabIndex = 17;
            this.btnDelete.Text = "Delete";
            this.btnDelete.EnabledChanged += new System.EventHandler(this.btnDelete_EnabledChanged);
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor =
                ((System.Windows.Forms.AnchorStyles)
                 ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(636, 12);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(65, 23);
            this.btnClose.TabIndex = 16;
            this.btnClose.Text = "Close";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor =
                ((System.Windows.Forms.AnchorStyles)
                 ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Location = new System.Drawing.Point(351, 12);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(65, 23);
            this.btnSave.TabIndex = 14;
            this.btnSave.Text = "Save";
            this.btnSave.EnabledChanged += new System.EventHandler(this.btnSave_EnabledChanged);
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnInUse
            // 
            this.btnInUse.Location = new System.Drawing.Point(22, 12);
            this.btnInUse.Name = "btnInUse";
            this.btnInUse.Size = new System.Drawing.Size(75, 23);
            this.btnInUse.TabIndex = 22;
            this.btnInUse.Text = "In Use";
            this.btnInUse.UseVisualStyleBackColor = true;
            this.btnInUse.Click += new System.EventHandler(this.btnInUse_Click);
            // 
            // frmSizeGroup
            // 
            this.AllowDragDrop = true;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(715, 483);
            this.Controls.Add(this.pnlParent);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "frmSizeGroup";
            this.Text = "Size Group";
            this.Load += new System.EventHandler(this.SizeGroup_Load);
            this.Controls.SetChildIndex(this.panel1, 0);
            this.Controls.SetChildIndex(this.panel2, 0);
            this.Controls.SetChildIndex(this.pnlParent, 0);
            ((System.ComponentModel.ISupportInitialize) (this.utmMain)).EndInit();
            this.pnlParent.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize) (this.gMiddle)).EndInit();
            this.pnlLeft.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize) (this.gLeft)).EndInit();
            this.pnlTop.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize) (this.gTop)).EndInit();
            this.pnlTopSpacer1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize) (this.picBoxGroup)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion


        private void HandleExceptions(Exception ex)
        {
            Debug.WriteLine(ex.ToString());
            MessageBox.Show(ex.ToString());
        }

        // BEGIN MID Track #4970 - modify to emulate other models 
        protected override void AfterClosing()
        {
            try
            {
                CheckForDequeue();
            }
            catch (Exception ex)
            {
                HandleException(ex, "SizeGroupMaint.AfterClosing");
            }
        }

        // END MID Track #4970  

        private void SizeGroup_Load(object sender, System.EventArgs e)
        {
            try
            {
                base.FormLoaded = false;
                AddMenuItem(eMIDMenuItem.FileNew);
                EnableMenuItem(this, eMIDMenuItem.FileNew);

                gTop.Rows[1].HeightDisplay = gTop.Height - gTop.Rows[0].HeightDisplay;
                    //Let the first non-header row take up the rest of the available screen.
                gLeft.Cols[0].Width = Include.SizeGroupWidthHeaderColumnWidth;
                    //"20" should be wide enough for this header column.
                gLeft.Cols[1].Width = gLeft.Width - gLeft.Cols[0].Width;
                    //Let the first non-header column take up the rest of the available screen.

                gTop.Rows.Count = 2;
                    //We need two rows for this grid. One is the "header" (so we can resize and drag/drop). One is for the data.
                gTop.Cols.Count = Include.SizeGroupGridInitialColumns;
                    //Initially we only give the user 3 cols (dave, it's your call). Additional columns will be added on the fly in gTop_AfterEdit event.

                gLeft.Cols.Count = 2;
                    //We only two columns for this grid. One is the "header" (so we can resize and drag/drop). One is for the data.
                gLeft.Rows.Count = Include.SizeGroupGridInitialRows;
                    //Initially we only give the user 5 rows (dave, it's your call). Additional rows will be added on the fly in gLeft_AfterEdit event.

                gMiddle.Rows.Count = Include.SizeGroupGridInitialRows;
                gMiddle.Cols.Count = Include.SizeGroupGridInitialColumns;

                gTop.Rows.Fixed = 1;
                    //the "fixed" property turns a row into something header-like, so that user can resize its width and drag/drop by using the mouse.
                gLeft.Cols.Fixed = 1;

                _newText = MIDText.GetTextOnly((int) eMIDTextCode.msg_NewSizeGroup);
                _noSecondarySizeStr = MIDText.GetTextOnly((int) eMIDTextCode.str_NoSecondarySize);
                _sgl = new SizeGroupList(eProfileType.SizeGroup);
                _sgl.LoadAll(false);
//				_newText = MIDText.GetTextOnly((int) eMIDTextCode.msg_NewSizeGroup);
//				// load group names
//				_ignoreFlag = true;
//				DataTable dtGroups = MIDEnvironment.CreateDataTable("Groups");
//				dtGroups.Columns.Add("Key");
//				dtGroups.Columns.Add("Name");		
//
//				_newText = MIDText.GetTextOnly((int) eMIDTextCode.msg_NewSizeGroup);
//				dtGroups.Rows.Add( new object[] { -1, _newText } ) ;
//				foreach(SizeGroupProfile sgp in _sgl.ArrayList)
//				{
//					dtGroups.Rows.Add(new object[] { sgp.Key, sgp.SizeGroupName });
//				}
//				
//				cboGroupName.DisplayMember = "Name";
//				cboGroupName.ValueMember = "Key";
//				cboGroupName.DataSource = dtGroups;
//				cboGroupName.SelectedValue = -1;
//				_ignoreFlag = false;
                BindGroupNameCombo();
                // END MID Track #4970

                // load product categories
                ArrayList pcAL = _SAB.HierarchyServerSession.GetSizeProductCategoryList();
                pcAL.Sort();
                cboProductCat.Items.Add(_newText);
                cboProductCat.Items.AddRange((string[]) pcAL.ToArray(typeof (string)));
                int index = cboProductCat.FindStringExact(_newText);
                cboProductCat.SelectedIndex = index;

                // BEGIN MID Track #4970
                index = cboGroupName.FindStringExact(_newText);
                cboGroupName.SelectedIndex = index;
                // END MID Track #4970

                // verify buttons
                rdbLike.Checked = true;

                SetReadOnly(FunctionSecurity.AllowUpdate);

                //if (!FunctionSecurity.AllowDelete)
                {
                    btnDelete.Enabled = false;
                    btnInUse.Enabled = false;    //TT#110-MD-VStuart - In Use Tool
                }

                cboGroupName.Enabled = true;
                SetAutoScrollMinSize = false;
                SetText();
                // BEGIN MID Track #3942  use soft text label  
                _noSizeDimensionLbl = MIDText.GetTextOnly((int) eMIDTextCode.lbl_NoSecondarySize);
                // END MID Track #3942

                SetMaskedComboBoxesEnabled();
                base.FormLoaded = true;
            }
            catch (Exception exc)
            {
                HandleExceptions(exc);
            }
        }

        // BEGIN MID Track #4970 - additional logic
        private void BindGroupNameCombo()
        {
            try
            {
                _ignoreFlag = true;
                cboGroupName.Items.Clear();
                cboGroupName.Items.Add(new ComboObject(-1, _newText));
                foreach (SizeGroupProfile sgp in _sgl.ArrayList)
                {
                    cboGroupName.Items.Add(new ComboObject(sgp.Key, sgp.SizeGroupName.Trim()));
                }
                _ignoreFlag = false;
            }
            catch (Exception ex)
            {
                HandleException(ex, "BindModelNameComboBox");
            }
        }

        private void SetText()
        {
            try
            {
                // BEGIN MID Track #3942  use soft text label  
                _noSizeDimensionLbl = MIDText.GetTextOnly((int) eMIDTextCode.lbl_NoSecondarySize);
                // END MID Track #3942
                this.lblSizeGroupName.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Size_Group) + " " +
                                             MIDText.GetTextOnly(eMIDTextCode.lbl_Name);
                this.lblSizeGroupDesc.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Size_Group) + " " +
                                             MIDText.GetTextOnly(eMIDTextCode.lbl_Description);
                this.lblProductCategory.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_SizeProductCategory);
                this.btnClear.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Clear);
                this.btnVerify.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Verify);
                this.btnDelete.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_Delete);
                this.btnSave.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_Save);
                this.btnClose.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_Close);
                this.btnInUse.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_In_Use);  //TT#110-MD-VStuart - In Use Tool
            }
            catch
            {
                throw;
            }
        }

        // END MID Track #4970  

        // BEGIN MID Track #4396 - Justin Bolles - Size Dropdown Filter
        private void BindSizeGroupComboBox(bool includeEmptySelection, string aFilterString, bool aCaseSensitive)
        {
            try
            {
                DataTable dtGroups = MIDEnvironment.CreateDataTable("Groups");
                // Originally tried RowFilter but received invalid rowfilter msg when multiple wild cards

                SizeGroup sizeGroup = new SizeGroup();
                aFilterString = aFilterString.Replace("*", "%");
                //aFilterString = aFilterString.Replace("'", "''"); // for string with single quote

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

                cboGroupName.Items.Clear();
                if (includeEmptySelection)
                {
                    cboGroupName.Items.Add(new ComboObject(-1, ""));
                }

                foreach (DataRow row in dtGroups.Rows)
                {
                    cboGroupName.Items.Add(new ComboObject(Convert.ToInt32(row["SIZE_GROUP_RID"]),
                                                           Convert.ToString(row["SIZE_GROUP_NAME"])));
                }
                cboGroupName.Enabled = true;
            }
            catch (Exception ex)
            {
                HandleException(ex, "BindSizeGroupComboBox");
            }
        }

        // END MID Track #4396


        #region Grid After Scroll events

        //Scrolling can occur by dragging scroll bars or using the arrow keys when inside the grids
        //After one grid scrolls, another grid needs to scroll with it, so we can keep the two grid positions synchronized.
        private void gTop_AfterScroll(object sender, C1.Win.C1FlexGrid.RangeEventArgs e)
        {
            try
            {
                if (gTop.ScrollPosition.X != gMiddle.ScrollPosition.X) //Horizontal position changed
                {
                    gMiddle.ScrollPosition = new Point(gTop.ScrollPosition.X, gMiddle.ScrollPosition.Y);
                }
            }
            catch (Exception exc)
            {
                HandleExceptions(exc);
            }
        }

        private void gLeft_AfterScroll(object sender, C1.Win.C1FlexGrid.RangeEventArgs e)
        {
            try
            {
                if (gLeft.ScrollPosition.Y != gMiddle.ScrollPosition.Y) //Vertical position changed
                {
                    gMiddle.ScrollPosition = new Point(gMiddle.ScrollPosition.X, gLeft.ScrollPosition.Y);
                }
            }
            catch (Exception ex)
            {
                HandleExceptions(ex);
            }
        }

        private void gMiddle_AfterScroll(object sender, C1.Win.C1FlexGrid.RangeEventArgs e)
        {
            try
            {
                if (gMiddle.ScrollPosition.X != gTop.ScrollPosition.X) //Horizontal position changed
                {
                    gTop.ScrollPosition = new Point(gMiddle.ScrollPosition.X, gTop.ScrollPosition.Y);
                    gMiddle.ScrollPosition = new Point(gTop.ScrollPosition.X, gMiddle.ScrollPosition.Y);
                }
                if (gMiddle.ScrollPosition.Y != gLeft.ScrollPosition.Y) //Vertical position changed
                {
                    gLeft.ScrollPosition = new Point(gLeft.ScrollPosition.X, gMiddle.ScrollPosition.Y);
                    gMiddle.ScrollPosition = new Point(gMiddle.ScrollPosition.X, gLeft.ScrollPosition.Y);
                }
            }
            catch (Exception ex)
            {
                HandleExceptions(ex);
            }
        }

        #endregion

        #region Grid After Edit events

        private void gTop_AfterEdit(object sender, C1.Win.C1FlexGrid.RowColEventArgs e)
        {
            ChangePending = true;
            if (e.Col == gTop.Cols.Count - 1)
            {
                gTop.Cols.Count ++; //add an extra column
                gMiddle.Cols.Count ++; //then do the same for gMiddle -- must keep the cols in sync!
            }
        }

        private void gLeft_AfterEdit(object sender, C1.Win.C1FlexGrid.RowColEventArgs e)
        {
            ChangePending = true;
            if (e.Row == gLeft.Rows.Count - 1)
            {
                gLeft.Rows.Count ++; // add an extra row
                gMiddle.Rows.Count ++; //then do the same for gMiddle -- must keep the rows in sync!
            }
        }

        private void gMiddle_AfterEdit(object sender, C1.Win.C1FlexGrid.RowColEventArgs e)
        {
            ChangePending = true;
            if (e.Col == gTop.Cols.Count - 1)
            {
                gTop.Cols.Count ++; //add an extra column
                gMiddle.Cols.Count ++; //then do the same for gMiddle -- must keep the cols in sync!
            }
            if (e.Row == gLeft.Rows.Count - 1)
            {
                gLeft.Rows.Count ++; // add an extra row
                gMiddle.Rows.Count ++; //then do the same for gMiddle -- must keep the rows in sync!
            }
        }

        #endregion

        private void btnVerify_Click(object sender, System.EventArgs e)
        {
            _verifySuccess = false;
            try
            {
                string productCatStr = cboProductCat.Text.Trim();
                if ((productCatStr == _newText) || (productCatStr == ""))
                {
                    MessageBox.Show(MIDText.GetText(eMIDTextCode.msg_SelectProductCategory));
                    return;
                }

                // based on Top and Left grids fillout/verify size codes
                bool[] colLocks = new bool[gTop.Cols.Count];
                bool[] rowLocks = new bool[gLeft.Rows.Count];
                for (int col = 0; col < gTop.Cols.Count; col++)
                {
                    string sizeStr = Convert.ToString(gTop[1, col], CultureInfo.CurrentUICulture).Trim();
                    for (int row = 0; row < gLeft.Rows.Count; row++)
                    {
                        string widthStr = Convert.ToString(gLeft[row, 1], CultureInfo.CurrentUICulture).Trim();

                        string sizeCodeStr = Convert.ToString(gMiddle[row, col], CultureInfo.CurrentUICulture).Trim();
                        if (sizeCodeStr != "")
                        {
                            SizeCodeProfile scp = _SAB.HierarchyServerSession.GetSizeCodeProfile(sizeCodeStr);
                            if (scp.Key == -1)
                            {
                                string text = MIDText.GetText(eMIDTextCode.msg_AddSizeCodeQuery);
                                text = text.Replace("{0}", sizeCodeStr);
                                DialogResult ret = MessageBox.Show(text, "",
                                                                   MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                                                                   MessageBoxDefaultButton.Button2);
                                if (ret == DialogResult.Yes)
                                {
                                    //									if ((sizeStr == "") || (widthStr == ""))
                                    if ((sizeStr == ""))
                                    {
                                        MessageBox.Show(MIDText.GetText(eMIDTextCode.msg_FillInSizeAndWidth));
                                        return;
                                    }
                                    // size doesn't exist, need to add it
                                    scp.SizeCodeID = sizeCodeStr;
                                    scp.SizeCodeName = sizeCodeStr;
                                    scp.SizeCodePrimary = sizeStr;
                                    // BEGIN MID Track #3942 - Secondary size of None required
                                    if (widthStr.Trim().ToUpper(CultureInfo.CurrentUICulture) ==
                                        _noSizeDimensionLbl.ToUpper(CultureInfo.CurrentUICulture))
                                    {
                                        widthStr = string.Empty;
                                    }
                                    // END MID Track #3942
                                    if (widthStr.Trim().Length == 0)
                                    {
                                        scp.SizeCodeSecondary = null;
                                    }
                                    else
                                    {
                                        scp.SizeCodeSecondary = widthStr;
                                    }
                                    scp.SizeCodeProductCategory = productCatStr;
                                    scp.SizeCodeChangeType = eChangeType.add;
                                    try
                                    {
                                        scp = _SAB.HierarchyServerSession.SizeCodeUpdate(scp);
                                    }
                                    catch (SizePrimaryRequiredException)
                                    {
                                        MessageBox.Show(MIDText.GetText(eMIDTextCode.msg_SizePrimaryRequired));
                                        return;
                                    }
                                    catch (SizeCatgPriSecNotUniqueException exc)
                                    {
                                        string message = MIDText.GetText(eMIDTextCode.msg_SizeCatgPriSecNotUnique);
                                        message = message.Replace("{0}", scp.SizeCodeProductCategory);
                                        message = message.Replace("{1}", scp.SizeCodePrimary);
                                        if (scp.SizeCodeSecondary != null &&
                                            scp.SizeCodeSecondary.Trim().Length > 1)
                                        {
                                            message = message.Replace("{2}", scp.SizeCodeSecondary);
                                        }
                                        else
                                        {
                                            message = message.Replace("{2}", _noSecondarySizeStr);
                                        }
                                        message = message.Replace("{3}", exc.Message);
                                        MessageBox.Show(message);
                                        return;
                                    }

                                    // see if product category is new
                                    if (cboProductCat.FindStringExact(productCatStr) == -1)
                                    {
                                        // load product categories
                                        ArrayList pcAL = _SAB.HierarchyServerSession.GetSizeProductCategoryList();
                                        pcAL.Sort();
                                        cboProductCat.Items.Clear();
                                        cboProductCat.Items.Add(_newText);
                                        cboProductCat.Items.AddRange((string[]) pcAL.ToArray(typeof (string)));
                                        int index = cboProductCat.FindStringExact(productCatStr);
                                        cboProductCat.SelectedIndex = index;
                                    }
                                }
                                else
                                {
                                    gMiddle.ShowCell(row, col);
                                    return;
                                }
                            }
                            else
                            {
                                // get the size code based on the product category, size and width
                                eSearchContent sizeSearchType;
                                if (colLocks[col] || rdbExact.Checked)
                                    sizeSearchType = eSearchContent.WholeField;
                                else if (rdbBegins.Checked)
                                    sizeSearchType = eSearchContent.StartOfField;
                                else
                                    sizeSearchType = eSearchContent.AnyPartOfField;

                                eSearchContent widthSearchType;
                                if (rowLocks[row] || rdbExact.Checked)
                                    widthSearchType = eSearchContent.WholeField;
                                else if (rdbBegins.Checked)
                                    widthSearchType = eSearchContent.StartOfField;
                                else
                                    widthSearchType = eSearchContent.AnyPartOfField;
                                // BEGIN MID Track #5482 Sizes not alwasys filled in
                                // BEGIN MID Track #3942 'None' secondary size is required
                                //if (widthStr == string.Empty)
                                //    widthStr = _noSizeDimensionLbl;
                                // END MID Track #3942
                                if (widthStr == string.Empty)
                                {
                                    if (scp.SizeCodeSecondary != null)
                                    {
                                        widthStr = scp.SizeCodeSecondary;
                                    }
                                    else
                                    {
                                        widthStr = _noSizeDimensionLbl;
                                    }
                                }
                                // END MID Track #5482
                                SizeCodeList scl = _SAB.HierarchyServerSession.GetSizeCodeList(
                                    productCatStr, eSearchContent.WholeField,
                                    sizeStr, sizeSearchType,
                                    widthStr, widthSearchType);

                                if (scl.FindKey(scp.Key) == null)
                                {
                                    string text = MIDText.GetText(eMIDTextCode.msg_CorrectThisCode);
                                    text = text.Replace("{0}", scp.SizeCodeID);
                                    text = text.Replace("{1}", scp.SizeCodePrimary);
                                    text = text.Replace("{2}", scp.SizeCodeSecondary);
                                    text = text.Replace("{3}", scp.SizeCodeProductCategory);
                                    DialogResult ret = MessageBox.Show(text,
                                                                       "", MessageBoxButtons.YesNo,
                                                                       MessageBoxIcon.Question,
                                                                       MessageBoxDefaultButton.Button1);
                                    gMiddle.ShowCell(row, col);
                                    if (ret == DialogResult.Yes)
                                    {
                                        if (scl.Count > 1) // Have the user choose the code
                                        {
                                            SelectSizeCode frmSelectSizeCode = new SelectSizeCode(scl, _SAB);
                                            frmSelectSizeCode.StartPosition = FormStartPosition.CenterScreen;

                                            text = MIDText.GetText(eMIDTextCode.msg_SelectSizeCode);
                                            text = text.Replace("{0}", sizeStr);
                                            text = text.Replace("{1}",
                                                                widthStr.Trim().Length > 0
                                                                    ? widthStr
                                                                    : _noSizeDimensionLbl);
                                            frmSelectSizeCode.LabelText = text;

                                            DialogResult theResult = frmSelectSizeCode.ShowDialog();
                                            if (theResult == DialogResult.OK)
                                            {
                                                sizeCodeStr = frmSelectSizeCode.SizeCodeID;
                                            }
                                            else
                                            {
                                                gMiddle.SetData(row, col, "");
                                                return;
                                            }

                                        }
                                        else if (scl.Count == 1) // No choice, only one code
                                        {
                                            scp = (SizeCodeProfile) scl.ArrayList[0];
                                            sizeCodeStr = scp.SizeCodeID;
                                        }
                                        else
                                        {
                                            gMiddle.SetData(row, col, "");
                                            return;
                                        }

                                    }
                                //Begin TT#2856 - MD - Creating Size Group with codes - RBeck
                                    //else
                                    //{
                                    //    return;
                                    //}                                
                                }
                                if (scp.SizeCodeProductCategory != productCatStr)
                                {
                                    return;
                                }
                                //End   TT#2856 - MD - Creating Size Group with codes - RBeck
                                scp = _SAB.HierarchyServerSession.GetSizeCodeProfile(sizeCodeStr);

                                gTop.SetData(1, col, scp.SizeCodePrimary);
                                sizeStr = scp.SizeCodePrimary;
                                colLocks[col] = true;
                                gLeft.SetData(row, 1, scp.SizeCodeSecondary);
                                widthStr = scp.SizeCodeSecondary;
                                rowLocks[row] = true;
                                gMiddle.SetData(row, col, scp.SizeCodeID);
                            }

                        }
                        else if ((sizeStr != "" && (widthStr != "" || row == 0)) ||
                                 // we have a blank cell, lets try to fill it in
                                 (sizeStr != "" && widthStr.Trim().Length == 0 && row == 0))
                            // if no secondary size, only allow one row	
                        {
                            string text = MIDText.GetText(eMIDTextCode.msg_WantToFillInBlankCode);
                            text = text.Replace("{0}", sizeStr);
                            text = text.Replace("{1}", widthStr.Trim().Length > 0 ? widthStr : _noSizeDimensionLbl);
                            if (MessageBox.Show(text, this.Text,
                                                MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                                == DialogResult.Yes)
                            {
                                //								if (widthStr == "")
                                //								{
                                //									widthStr = _noSecondarySizeStr;
                                //								}
                                // get the size code based on the product category, size and width
                                eSearchContent sizeSearchType;
                                if (colLocks[col] || rdbExact.Checked)
                                    sizeSearchType = eSearchContent.WholeField;
                                else if (rdbBegins.Checked)
                                    sizeSearchType = eSearchContent.StartOfField;
                                else
                                    sizeSearchType = eSearchContent.AnyPartOfField;

                                eSearchContent widthSearchType;
                                if (rowLocks[row] || rdbExact.Checked)
                                    widthSearchType = eSearchContent.WholeField;
                                else if (rdbBegins.Checked)
                                    widthSearchType = eSearchContent.StartOfField;
                                else
                                    widthSearchType = eSearchContent.AnyPartOfField;
                                // BEGIN MID Track #3942 'None' secondary size is required
                                if (widthStr == string.Empty)
                                    widthStr = _noSizeDimensionLbl;
                                // END MID Track #3942
                                SizeCodeList scl = _SAB.HierarchyServerSession.GetSizeCodeList(
                                    productCatStr, eSearchContent.WholeField,
                                    sizeStr, sizeSearchType,
                                    widthStr, widthSearchType);

                                if (scl.Count > 1) // Have the user choose the code
                                {
                                    SelectSizeCode frmSelectSizeCode = new SelectSizeCode(scl, _SAB);
                                    frmSelectSizeCode.StartPosition = FormStartPosition.CenterScreen;
                                    frmSelectSizeCode.LabelText = "Select correct size code for " + sizeStr + ", " +
                                                                  widthStr;

                                    DialogResult theResult = frmSelectSizeCode.ShowDialog();
                                    if (theResult == DialogResult.OK)
                                    {
                                        sizeCodeStr = frmSelectSizeCode.SizeCodeID;
                                    }
                                    else
                                    {
                                        return;
                                    }
                                }
                                else if (scl.Count == 1) // No choice, only one code
                                {
                                    SizeCodeProfile scp = (SizeCodeProfile) scl.ArrayList[0];
                                    sizeCodeStr = scp.SizeCodeID;
                                }
//Begin Track #3723 - JScott - Prevent blank size codes from being saved
                                else
                                {
                                    text = MIDText.GetText(eMIDTextCode.msg_NoSizeCodeFound);
                                    text = text.Replace("{0}", sizeStr);
                                    text = text.Replace("{1}", widthStr);
                                    text = text.Replace("{2}", productCatStr);
                                    MessageBox.Show(text, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    //return;   //End   TT#2856 - MD - Creating Size Group with codes - RBeck
                                }
//End Track #3723 - JScott - Prevent blank size codes from being saved

                                if (sizeCodeStr != "")
                                {
                                    SizeCodeProfile scp = _SAB.HierarchyServerSession.GetSizeCodeProfile(sizeCodeStr);

                                    gTop.SetData(1, col, scp.SizeCodePrimary);
                                    sizeStr = scp.SizeCodePrimary;
                                    colLocks[col] = true;
                                    gLeft.SetData(row, 1, scp.SizeCodeSecondary);
                                    widthStr = scp.SizeCodeSecondary;
                                    rowLocks[row] = true;
                                    gMiddle.SetData(row, col, scp.SizeCodeID);
                                }
                            }
//Begin Track #3723 - JScott - Prevent blank size codes from being saved
                            else
                            {
                                //return;   //End   TT#2856 - MD - Creating Size Group with codes - RBeck
                            }
//End Track #3723 - JScott - Prevent blank size codes from being saved
                        }
                    }
                }
                // check for duplicate columns or rows
                Hashtable dupHash = new Hashtable();
                for (int col = 0; col < gTop.Cols.Count; col++)
                {
                    string sizeStr = Convert.ToString(gTop[1, col], CultureInfo.CurrentUICulture).Trim();
                    if (sizeStr != "")
                    {
                        if (dupHash.ContainsKey(sizeStr))
                        {
                            string text = MIDText.GetText(eMIDTextCode.msg_DuplicateSize);
                            text = text.Replace("{0}", sizeStr);
                            MessageBox.Show(text);
                            return;
                        }
                        dupHash.Add(sizeStr, null);
                    }
                }
                dupHash.Clear();
                for (int row = 0; row < gLeft.Rows.Count; row++)
                {
                    string widthStr = Convert.ToString(gLeft[row, 1], CultureInfo.CurrentUICulture).Trim();
                    if (widthStr != "")
                    {
                        if (dupHash.ContainsKey(widthStr))
                        {
                            string text = MIDText.GetText(eMIDTextCode.msg_DuplicateWidth);
                            text = text.Replace("{0}", widthStr);
                            MessageBox.Show(text);
                            return;
                        }
                        dupHash.Add(widthStr, null);
                    }
                }

            }
            catch (Exception exc)
            {
                HandleExceptions(exc);
            }
            _verifySuccess = true;
        }

//		private void cboGroupName_TextChanged(object sender, System.EventArgs e)
//		{
//			if (_ignoreFlag)
//			{
//				return;
//			}
//					
//			if (ChangePending && (_groupRID > 0))
//			{
//				DialogResult ret = MessageBox.Show(MIDText.GetText(eMIDTextCode.msg_SavePendingChanges),"",
//					MessageBoxButtons.YesNo,MessageBoxIcon.Question,MessageBoxDefaultButton.Button2);
//				if (ret == DialogResult.Yes)
//				{
//					_ignoreFlag = true;
//					cboGroupName.SelectedValue = _groupRID;
//					_ignoreFlag = false;
//					return;
//				}
//				ChangePending = false;
//			}
//			_groupRID = Convert.ToInt32(cboGroupName.SelectedValue, CultureInfo.CurrentUICulture);
// 
//			//_currGroupName = cboGroupName.Text;
//			//ChangePending = true;
//		}

        private void cboGroupName_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            try
            {
                if (_ignoreFlag)
                {
                    return;
                }

//				if (ChangePending)
//				{
//					DialogResult ret = MessageBox.Show(MIDText.GetText(eMIDTextCode.msg_SavePendingChanges),"",
//						MessageBoxButtons.YesNo,MessageBoxIcon.Question,MessageBoxDefaultButton.Button2);
//					if (ret == DialogResult.Yes)
//					{
//						_ignoreFlag = true;
//						cboGroupName.SelectedValue = _groupRID;
//						_ignoreFlag = false;
//						return;
//					}
//					ChangePending = false;
//
//				}
//				SetSizeGroup(); 
                _ignoreFlag = true;
                if (FormLoaded)
                {
                    if (CheckForPendingChanges())
                    {
                        ComboObject comboObj = (ComboObject) cboGroupName.SelectedItem;

                        SetSizeGroup(comboObj.Key);

                        ChangePending = false;
                    }
                    // Begin TT#3143 - JSmith - Save As always creates a new Size Group instead of updating an existing one
                    //if (FunctionSecurity.AllowDelete) \\VicTest
                    {
                    //    btnDelete.Enabled = true;
                        btnInUse.Enabled = true;    //TT#110-MD-VStuart - In Use Tool
                    }
                    // End TT#3143 - JSmith - Save As always creates a new Size Group instead of updating an existing one
                }
                _ignoreFlag = false;
            }
            catch (Exception exc)
            {
                HandleExceptions(exc);
            }
        }

        // BEGIN MID Track #4970 - replace TextChanged event with Validating
        private void cboGroupName_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _currGroupName = cboGroupName.Text.Trim();
        }

        //END MID Track #4970

        private void SetSizeGroup(int aGroupRID)
        {
            try
            {
                // BEGIN MID Track #4970 - add locking
                bool allowUpdate = FunctionSecurity.AllowUpdate;
                eDataState dataState;
                CheckForDequeue();
                _groupRID = Convert.ToInt32(cboGroupName.SelectedValue, CultureInfo.CurrentUICulture);
                _groupRID = aGroupRID;
                //if (cboGroupName.SelectedIndex >= 0)
                if (_groupRID >= 0)
                {
                    //_groupRID = Convert.ToInt32(cboGroupName.SelectedValue, CultureInfo.CurrentUICulture);
                    if (allowUpdate)
                    {
                        _currSizeGroupProfile =
                            (SizeGroupProfile)
                            _SAB.HierarchyServerSession.GetModelDataForUpdate(eModelType.SizeGroup, _groupRID, true);
                        if (_currSizeGroupProfile.ModelLockStatus == eLockStatus.ReadOnly)
                        {
                            allowUpdate = false;
                        }
                        else if (_currSizeGroupProfile.ModelLockStatus == eLockStatus.Cancel)
                        {
                            this.cboGroupName.SelectedIndex = _currModelIndex;
                            return;
                        }
                    }
                    else
                    {
                        _currSizeGroupProfile = _SAB.HierarchyServerSession.GetSizeGroupData(_groupRID);
                    }
                    _currModelIndex = this.cboGroupName.SelectedIndex;
                    if (!allowUpdate)
                    {
                        _modelLocked = false;
                    }
                    else
                    {
                        _modelLocked = true;
                    }
                    _modelRID = _currSizeGroupProfile.Key;
                    _currGroupName = _currSizeGroupProfile.SizeGroupName;
                }

                //_groupRID = Convert.ToInt32(cboGroupName.SelectedValue, CultureInfo.CurrentUICulture);
                //SizeGroupProfile sgp = 
                //	(SizeGroupProfile)_sgl.FindKey(_groupRID);
                //if (sgp == null)
                //{
                //	// add new size group
                //}
                //else
                //{
                //if (cboGroupName.SelectedIndex >= 0)
                if (_groupRID >= 0)
                {
                    // END MID Track #4970
                    // clear the grid first
                    btnClear_Click((object) null, (System.EventArgs) null);

                    txtDescription.Text = _currSizeGroupProfile.SizeGroupDescription;

                    // load existing group
                    SizeCodeList scl = _currSizeGroupProfile.SizeCodeList;

                    System.Collections.SortedList sizeAL = new SortedList();
                    System.Collections.SortedList widthAL = new SortedList();
                    System.Collections.Hashtable bothHash = new Hashtable();
                    bool productCategoryIsSet = false;
                    foreach (SizeCodeProfile scp in scl.ArrayList)
                    {
                        if (scp.Key == -1)
                        {
                            throw new MIDException(eErrorLevel.severe,
                                                   (int) eMIDTextCode.msg_CantRetrieveSizeCode,
                                                   MIDText.GetText(eMIDTextCode.msg_CantRetrieveSizeCode));
                        }
                        if (! productCategoryIsSet)
                        {
                            // set the product category
                            int index = cboProductCat.FindStringExact(scp.SizeCodeProductCategory);
                            if (index == -1)
                            {
                                string text = MIDText.GetText(eMIDTextCode.msg_UnknownProductCategory);
                                text = text.Replace("{0}", scp.SizeCodeID);
                                text = text.Replace("{1}", scp.SizeCodeProductCategory);

                                throw new MIDException(eErrorLevel.severe,
                                                       (int) eMIDTextCode.msg_UnknownProductCategory,
                                                       text);
                            }
                            cboProductCat.SelectedIndex = index;
                            productCategoryIsSet = true;
                        }
                        if (! sizeAL.Contains(scp.PrimarySequence))
                            sizeAL.Add(scp.PrimarySequence, scp.SizeCodePrimary);
                        if (! widthAL.Contains(scp.SecondarySequence))
                            widthAL.Add(scp.SecondarySequence, scp.SizeCodeSecondary);
                        if (! bothHash.ContainsKey(scp.SizeCodePrimary + "~" + scp.SizeCodeSecondary))
                            bothHash.Add(scp.SizeCodePrimary + "~" + scp.SizeCodeSecondary, scp);
                        else
                            // this should not happen? 
                            throw new MIDException(eErrorLevel.severe,
                                                   (int) eMIDTextCode.msg_LabelsNotUnique,
                                                   MIDText.GetText(eMIDTextCode.msg_LabelsNotUnique));
                    }


                    // make sure grids are large enough to hold data
                    if (gTop.Cols.Count <= sizeAL.Count)
                    {
                        gTop.Cols.Count = sizeAL.Count + 1; //add an extra column
                        gMiddle.Cols.Count = sizeAL.Count + 1;
                            //then do the same for gMiddle -- must keep the cols in sync!
                    }
                    if (gLeft.Rows.Count <= widthAL.Count)
                    {
                        gLeft.Rows.Count = widthAL.Count + 1; //add an extra row
                        gMiddle.Rows.Count = widthAL.Count + 1;
                            //then do the same for gMiddle -- must keep the rows in sync!
                    }

                    // load gTop
                    int col = 0;
                    foreach (string size in sizeAL.Values)
                    {
                        gTop.SetData(1, col, size);
                        col++;
                    }

                    // load gLeft & middle
                    int row = 0;
                    foreach (string width in widthAL.Values)
                    {
                        gLeft.SetData(row, 1, width);
                        col = 0;
                        foreach (string size in sizeAL.Values)
                        {
                            if (bothHash.Contains(size + "~" + width))
                            {
                                gMiddle.SetData(row, col, ((SizeCodeProfile) bothHash[size + "~" + width]).SizeCodeID);
                            }
                            col++;
                        }
                        row++;
                    }
                }
                // BEGIN MID Track #4970 - add locaking
                if (allowUpdate)
                {
                    dataState = eDataState.Updatable;
                }
                else
                {
                    dataState = eDataState.ReadOnly;
                }
                Format_Title(dataState, eMIDTextCode.frm_SizeGroups, null);
                DetermineControlsEnabled(allowUpdate);
                // Begin Track #5495 - JSmith - magnifying glass is not enabled
                SetMaskedComboBoxesEnabled();
                // End Track #5495
            } // END MID Track #4970
            catch (Exception exc)
            {
                HandleExceptions(exc);
            }
        }

        // BEGIN MID Track #4970 - emulate other models 
        private void CheckForDequeue()
        {
            try
            {
                if (_modelLocked)
                {
                    _SAB.HierarchyServerSession.DequeueModel(eModelType.SizeGroup, _modelRID);
                    _modelLocked = false;
                }
            }
            catch (Exception exception)
            {
                HandleException(exception);
            }
        }

        private void DetermineControlsEnabled(bool allowUpdate)
        {
            try
            {
                SetReadOnly(allowUpdate);
                btnInUse.Enabled = true;    //TT#3185-M-VStuart-In Use does not work in all explorers-ANFUser

                gLeft.AllowEditing = allowUpdate;
                gTop.AllowEditing = allowUpdate;
                gMiddle.AllowEditing = allowUpdate;

                mnuItemRowInsert.Enabled = allowUpdate;
                mnuItemRowDelete.Enabled = allowUpdate;
                mnuItemColInsert.Enabled = allowUpdate;
                mnuItemColDelete.Enabled = allowUpdate;

                if (!allowUpdate)
                {
                    cboGroupName.Enabled = true;
                }
                else
                {
                    //BEGIN TT#3185-M-VStuart-In Use does not work in all explorers-ANFUser
                    if (FunctionSecurity.AllowDelete && cboGroupName.SelectedIndex > -1)
                    {
                        btnDelete.Enabled = true;
                        //btnInUse.Enabled = true;    //TT#110-MD-VStuart - In Use Tool
                    }
                    else
                    {
                        btnDelete.Enabled = false;
                        //btnInUse.Enabled = false;    //TT#110-MD-VStuart - In Use Tool
                    }
                    //btnInUse.Enabled = true;
                    //END TT#3185-M-VStuart-In Use does not work in all explorers-ANFUser
                }
            }
            catch
            {
                throw;
            }
        }

        // END MID Track #4970

        private void btnDelete_Click(object sender, System.EventArgs e)
        {
            IDelete();
        }

        private void btnSave_Click(object sender, System.EventArgs e)
        {
            try
            {
                _showSaveMsg = true;
                SaveChanges();
            }
            catch
            {
                throw;
            }
        }

        // Begin TT#2700 - JSmith - New Size Group in 5.0
        //private bool SaveAs()
        private bool SaveAs(bool aNewModel)
            // End TT#2700 - JSmith - New Size Group in 5.0
        {
            frmSaveAs formSaveAs;
            bool continueSave;
            bool saveCancelled;
            SizeGroupProfile sgp;

            try
            {
                // Begin TT#2700 - JSmith - New Size Group in 5.0
                //btnVerify_Click((object)null, (System.EventArgs)null);
                if (!aNewModel)
                {
                    btnVerify_Click((object) null, (System.EventArgs) null);
                }
                // End TT#2700 - JSmith - New Size Group in 5.0
                if (_verifySuccess)
                {
                    sgp = null;

                    formSaveAs = new frmSaveAs(SAB);
                    formSaveAs.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
                    formSaveAs.ShowUserGlobal = false;

                    // Begin TT#2700 - JSmith - New Size Group in 5.0
                    //formSaveAs.SaveAsName = cboGroupName.Text.Trim();
                    if (!aNewModel)
                    {
                        formSaveAs.SaveAsName = cboGroupName.Text.Trim();
                    }
                    // End TT#2700 - JSmith - New Size Group in 5.0

                    continueSave = false;
                    saveCancelled = false;

                    while (!continueSave && !saveCancelled)
                    {
                        formSaveAs.ShowDialog(this);
                        saveCancelled = formSaveAs.SaveCanceled;

                        if (!saveCancelled)
                        {
                            CheckForDequeue(); //TT#3443-M-VStuart-Save As Size Groups-Urban

                            sgp = _sgl.FindGroupName(formSaveAs.SaveAsName);
                            if (sgp == null ||
                                MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_DuplicateName),
                                                Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) ==
                                DialogResult.Yes)
                            {
                                continueSave = true;
                            }
                        }
                    }

                    if (!saveCancelled)
                    {
                        // Begin TT#2700 - JSmith - New Size Group in 5.0
                        if (aNewModel)
                        {
                            _ignoreFlag = true;
                            _currGroupName = formSaveAs.SaveAsName;
                            cboGroupName.Text = formSaveAs.SaveAsName;
                            _groupRID = Include.NoRID;
                            _ignoreFlag = false;
                            return true;
                        }
                        // End TT#2700 - JSmith - New Size Group in 5.0
                        if (sgp != null &&
                            sgp.Key != Include.NoRID &&
                            sgp.Key == _groupRID)
                        {
                            // Begin TT#3143 - JSmith - Save As always creates a new Size Group instead of updating an existing one
                            //sgp.DeleteSizeGroup();
                            //_sgl.Remove(sgp);
                            SaveChanges();
                            ChangePending = false;
                            return true;
                            // ENd TT#3143 - JSmith - Save As always creates a new Size Group instead of updating an existing one
                        }

                        _currGroupName = formSaveAs.SaveAsName;
                        // Begin TT#2700 - JSmith - New Size Group in 5.0
                        _ignoreFlag = true;
                        // End TT#2700 - JSmith - New Size Group in 5.0
                        cboGroupName.Text = formSaveAs.SaveAsName;
                        // Begin TT#2700 - JSmith - New Size Group in 5.0
                        _ignoreFlag = false;
                        // End TT#2700 - JSmith - New Size Group in 5.0
                        if (sgp != null && 
                            sgp.Key != Include.NoRID)
                        {
                            CheckForDequeue();
                            _groupRID = sgp.Key;
                        }
                        else
                        {
                            _groupRID = Include.NoRID;
                        }
                        SaveChanges();

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

        protected override bool SaveChanges()
        {
//Begin Track #3607 - JScott - Duplicate key during save of new Group
            int groupRID;
            SizeGroupProfile sgp;

//End Track #3607 - JScott - Duplicate key during save of new Group
            // do verify first

            // BEGIN MID Track #4970 - emulate ohter models
            if (!NameValid())
            {
                return false;
            }
            // END  MID Track #4970

            btnVerify_Click((object) null, (System.EventArgs) null);
            if (! _verifySuccess)
            {
                return false;
            }
            try
            {
                bool nameChanged = false; // MID Track #4970
                string oldName = string.Empty; // MID Track #4970

                string groupNameStr = cboGroupName.Text.Trim();
                if ((groupNameStr == _newText) || (groupNameStr == ""))
                {
                    // Begin TT#2700 - JSmith - New Size Group in 5.0
                    //MessageBox.Show(MIDText.GetText(eMIDTextCode.msg_SelectSizeGroup));
                    //return false;
                    if (!SaveAs(true))
                    {
                        return false;
                    }
                    // End TT#2700 - JSmith - New Size Group in 5.0
                }

                // Begin TT#2966 - JSmith - Size Groups will not appear in the application for use
                Cursor = Cursors.WaitCursor;
                // End TT#2966 - JSmith - Size Groups will not appear in the application for use

                // Now it is time to write it out
//Begin Track #3607 - JScott - Duplicate key during save of new Group
//				int groupRID = Convert.ToInt32(cboGroupName.SelectedValue, CultureInfo.CurrentUICulture);
//				SizeGroupProfile sgp = 
//					(SizeGroupProfile)_sgl.FindKey(groupRID);
//				if (sgp == null)
//				{
//					sgp = new SizeGroupProfile(groupRID);
//				}
                groupRID = _groupRID; // MID Track #4970
                //if (cboGroupName.SelectedValue != null)
                if (groupRID != -1)
                {
                    //groupRID = Convert.ToInt32(cboGroupName.SelectedValue, CultureInfo.CurrentUICulture);
                    sgp = (SizeGroupProfile) _sgl.FindKey(groupRID);
                    if (sgp == null)
                    {
                        sgp = new SizeGroupProfile(groupRID);
                    }
                }
                else
                {
                    //sgp = _sgl.FindGroupName(groupNameStr);	// MID TRack #4970
                    sgp = _sgl.FindGroupName(_currGroupName);
                    if (sgp != null)
                    {
                        string text = MIDText.GetText(eMIDTextCode.msg_SizeGroupExists);
                        text = text.Replace("{0}", groupNameStr);
                        DialogResult ret = MessageBox.Show(text, "",
                                                           MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                                                           MessageBoxDefaultButton.Button2);
                        if (ret == DialogResult.No)
                        {
                            return false;
                        }
                    }
                    else
                    {
                        sgp = new SizeGroupProfile(Include.NoRID);
                    }

                    groupRID = sgp.Key;
                }

//End Track #3607 - JScott - Duplicate key during save of new Group
                //sgp.SizeGroupName = groupNameStr;	// BEGIN MID Track #4970
                if (sgp.SizeGroupName != _currGroupName)
                {
                    oldName = sgp.SizeGroupName;
                    nameChanged = true;
                }

                sgp.SizeGroupName = _currGroupName; // END MID Track #4970
                sgp.SizeGroupDescription = txtDescription.Text.Trim();
                sgp.SizeCodeList.Clear();

                for (int col = 0; col < gTop.Cols.Count; col++)
                {
                    for (int row = 0; row < gLeft.Rows.Count; row++)
                    {
                        string sizeCodeStr = Convert.ToString(gMiddle[row, col], CultureInfo.CurrentUICulture).Trim();
                        if (sizeCodeStr != "")
                        {
                            SizeCodeProfile scp = _SAB.HierarchyServerSession.GetSizeCodeProfile(sizeCodeStr);
//Begin Track #3607 - JScott - Duplicate key during save of new Group
                            scp.PrimarySequence = col + 1;
                            scp.SecondarySequence = row + 1;
//End Track #3607 - JScott - Duplicate key during save of new Group
                            sgp.SizeCodeList.Add(scp);
                        }
                    }
                }
                if (sgp.SizeCodeList.Count == 0)
                {
                    MessageBox.Show(MIDText.GetText(eMIDTextCode.msg_MustHaveAtLeaseOneSizeCode));
                    return false;
                }
                sgp.WriteSizeGroup();
                if (groupRID < 1)
                {
                    _sgl.Add(sgp);

                    // re-load group names
                    _ignoreFlag = true;
//					DataTable dtGroups = MIDEnvironment.CreateDataTable("Groups");
//					dtGroups.Columns.Add("Key");
//					dtGroups.Columns.Add("Name");	
//	
//					dtGroups.Rows.Add( new object[] { -1, _newText } ) ;
//					foreach(SizeGroupProfile sgpTemp in _sgl.ArrayList)
//					{
//						dtGroups.Rows.Add(new object[] { sgpTemp.Key, sgpTemp.SizeGroupName });
//					}
//					
//					cboGroupName.DisplayMember = "Name";
//					cboGroupName.ValueMember = "Key";
//					cboGroupName.DataSource = dtGroups;
//					cboGroupName.SelectedValue = sgp.Key;
                    _groupRID = sgp.Key;
                    // BEGIN MID Track #4970 - modify to emulate other models
                    cboGroupName.Items.Add(new ComboObject(sgp.Key, sgp.SizeGroupName));
                    //this.cbGroupName.Enabled = true;

                    for (int i = 0; i < cboGroupName.Items.Count; i++)
                    {
                        ComboObject comboObj = (ComboObject) cboGroupName.Items[i];
                        if (comboObj.Key == sgp.Key)
                        {
                            cboGroupName.SelectedIndex = i;
                            break;
                        }
                    }
                    _ignoreFlag = false;

                }
                else if (nameChanged)
                {
                    int index = cboGroupName.FindStringExact(oldName);
                    //DataTable dtGroups = (DataTable)cboGroupName.DataSource;
                    //DataRow row =  dtGroups.Rows[index];
                    //row["Name"] = groupNameStr;
                    ComboObject comboObj = (ComboObject) cboGroupName.Items[index];
                    cboGroupName.Items.Remove(comboObj);
                    cboGroupName.Items.Add(new ComboObject(sgp.Key, _currGroupName));
                }

// (CSMITH) - BEG MID Track #2895: No message when click Save
                if (_showSaveMsg) // MID Track #4970 - emulate other models; qualify displaying Save message 
                {
                    MessageBox.Show("Save completed", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    _showSaveMsg = false;
                }
// (CSMITH) - END MID Track #2895
                // Begin TT#148 - JSmith - When making a Size Group receive unhandled exception
                // need to lock data for update
                //_currSizeGroupProfile = sgp;    // MID Track #5480 - null reference
                if (_currSizeGroupProfile == null ||
                    _currSizeGroupProfile.Key != _groupRID)
                {
                    _currSizeGroupProfile =
                        (SizeGroupProfile)
                        _SAB.HierarchyServerSession.GetModelDataForUpdate(eModelType.SizeGroup, _groupRID, true);
                    // Begin TT#163 - RMatelic - Size Group after created and saved go back to review and get a Model Conflict message
                    _modelRID = _currSizeGroupProfile.Key;
                    _modelLocked = true;
                    _currModelIndex = this.cboGroupName.SelectedIndex;
                    // End TT#163
                }
                // End TT#148
            }
            catch (Exception exc)
            {
                HandleExceptions(exc);
            }
            // Begin TT#2966 - JSmith - Size Groups will not appear in the application for use
            finally
            {
                Cursor = Cursors.Default;
            }
            // End TT#2966 - JSmith - Size Groups will not appear in the application for use
            ChangePending = false;
            return true;
        }

        // BEGIN MID Track #4970 - added name change logic
        private bool NameValid()
        {
            bool isValid = true;
            try
            {
                //if (_newModel)
                if (_groupRID == -1)
                {
                    return isValid;
                }
                int grpRID = 0;
                string grpName = null;


                if (_currGroupName != _currSizeGroupProfile.SizeGroupName.Trim())
                {
                    SizeGroup sizeGroup = new SizeGroup();
                    DataTable dt = sizeGroup.GetSizeGroup(_currGroupName);
                    if (dt.Rows.Count > 0)
                    {
                        DataRow aRow = dt.Rows[0];
                        grpRID = Convert.ToInt32(aRow["SIZE_GROUP_RID"], CultureInfo.CurrentUICulture);
                        grpName = Convert.ToString(aRow["SIZE_GROUP_NAME"], CultureInfo.CurrentUICulture);

                        if (grpName == _currGroupName && grpRID != _groupRID)
                        {
                            isValid = false;

                            if (MessageBox.Show(_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_DuplicateName),
                                                this.Text,
                                                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                            {
                                cboGroupName.Text = _currSizeGroupProfile.SizeGroupName.Trim();
                            }
                            else
                            {
                                cboGroupName.Text = _currGroupName;
                            }
                        }
                    }
                }
            }
            catch
            {
                isValid = false;
                throw;
            }
            return isValid;
        }

        // END MID Track #4970 

        private void btnClear_Click(object sender, System.EventArgs e)
        {
            gTop.Clear();
            gLeft.Clear();
            gMiddle.Clear();

            //the "Clear" methods above also clears heights and widths of the grids. So set those again:
            gTop.Rows[1].HeightDisplay = gTop.Height - gTop.Rows[0].HeightDisplay;
                //Let the first non-header row take up the rest of the available screen.
            gLeft.Cols[0].Width = Include.SizeGroupWidthHeaderColumnWidth;
                //"20" should be wide enough for this header column.
            gLeft.Cols[1].Width = gLeft.Width - gLeft.Cols[0].Width;
                //Let the first non-header column take up the rest of the available screen.

        }

        private void btnClose_Click(object sender, System.EventArgs e)
        {
//			if (ChangePending)
//			{
//				DialogResult ret = MessageBox.Show("Save changes first?","",
//					MessageBoxButtons.YesNo,MessageBoxIcon.Question,MessageBoxDefaultButton.Button2);
//				if (ret == DialogResult.Yes)
//				{
//					return;
//				}
//			}

            this.Close();
        }

        private void txtDescription_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            ChangePending = true;
        }

        #region Resizing rows and columns

        #region Column Resize Events

        private void gTop_AfterResizeColumn(object sender, C1.Win.C1FlexGrid.RowColEventArgs e)
        {
            try
            {
                //Readjust column sizes for gMiddle
                gMiddle.Cols[e.Col].Width = gTop.Cols[e.Col].Width;
                //				RefreshHorizontalScrollBars(3);
            }
            catch (Exception exc)
            {
                HandleExceptions(exc);
            }
        }

        private void gTop_BeforeResizeColumn(object sender, C1.Win.C1FlexGrid.RowColEventArgs e)
        {
            //Since we are resizing, not dragging, we need to set the _dragState
            //to "dragResize" so that gTop_MouseMove event doesn't process the
            //dragging actions.
            try
            {
                _dragState = DragState.dragResize;
            }
            catch (Exception exc)
            {
                HandleExceptions(exc);
            }
        }

        #endregion

        #region Row Resize Events

        private void gLeft_AfterResizeRow(object sender, C1.Win.C1FlexGrid.RowColEventArgs e)
        {
            try
            {
                //Readjust column sizes for gMiddle
                gMiddle.Rows[e.Row].Height = gLeft.Rows[e.Row].Height;
            }
            catch (Exception exc)
            {
                HandleExceptions(exc);
            }
        }

        private void gLeft_BeforeResizeRow(object sender, C1.Win.C1FlexGrid.RowColEventArgs e)
        {
            //Since we are resizing, not dragging, we need to set the _dragState
            //to "dragResize" so that gTop_MouseMove event doesn't process the
            //dragging actions.
            try
            {
                _dragState = DragState.dragResize;
            }
            catch (Exception exc)
            {
                HandleExceptions(exc);
            }
        }

        #endregion

        #endregion

        #region Drag and Drop

        #region Drag/Drop columns

        private void gTop_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            try
            {
                _gTopMouseCol = gTop.MouseCol;
                _gTopMouseRow = gTop.MouseRow;

                if (e.Button == MouseButtons.Left)
                {
                    //If left button is pressed, set _dragState to "dragReady" and store the 
                    //initial row and column range. The "ready" state indicates a drag
                    //operation is to begin if the mouse moves while the mouse is down.

                    //we want to enable drag-drop when the user clicks on the heading row (row 0).
                    if (_gTopMouseRow == 0 &&
                        (_dragState == DragState.dragNone || _dragState == DragState.dragReady))
                    {
                        _dragState = DragState.dragReady;
                        _dragStartColumn = gTop.MouseCol;
                    }
                }
            }
            catch (Exception exc)
            {
                HandleExceptions(exc);
            }
        }

        private void gTop_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            //if the _dragState is "dragReady", set the _dragState to "started" 
            //and begin the drag.
            try
            {
                if (_dragState == DragState.dragReady)
                {
                    _dragState = DragState.dragStarted;
                    gTop.DoDragDrop(sender, DragDropEffects.All);
                }
            }
            catch (Exception exc)
            {
                HandleExceptions(exc);
            }
        }

        private void gTop_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            //If the button is released, set the _dragState to "dragNone"
            try
            {
                _dragState = DragState.dragNone;
            }
            catch (Exception exc)
            {
                HandleExceptions(exc);
            }
        }

        private void gTop_DragEnter(object sender, System.Windows.Forms.DragEventArgs e)
        {
            //Check the keystate(state of mouse buttons, among other things). 
            //If the left mouse is down, we're okay. Else, set _dragState
            //to "none", which will cause the drag operation to be cancelled at the 
            //next QueryContinueDrag call. This situation occurs if the user
            //releases the mouse button outside of the grid.
            try
            {
                if ((e.KeyState & 0x01) == 1)
                {
                    e.Effect = DragDropEffects.All;
                }
                else
                {
                    _dragState = DragState.dragNone;
                }
            }
            catch (Exception exc)
            {
                HandleExceptions(exc);
            }
        }

        private void gTop_DragOver(object sender, System.Windows.Forms.DragEventArgs e)
        {
            //During drag over, if the mouse is dragged to the left- or right-most
            //part of the grid, scroll grid to show the columns.

            int mouseX;

            try
            {
                mouseX = e.X - (this.Left + ((this.Size.Width - this.ClientSize.Width)/2));
                if (mouseX > gTop.Right - 20 && mouseX < gTop.Right)
                {
                    gTop.LeftCol ++;
                }
                else if (mouseX > gTop.Left && mouseX < gTop.Left + 20)
                {
                    gTop.LeftCol --;
                }
            }
            catch (Exception exc)
            {
                HandleExceptions(exc);
            }
        }

        private void gTop_QueryContinueDrag(object sender, System.Windows.Forms.QueryContinueDragEventArgs e)
        {
            //Check to see if the drag should continue. 
            //Cancel if:
            //(1) the escape key is pressed
            //(2) the DragEnter event handler cancels the drag by setting the 
            //		_dragState to "none".
            //Otherwise, if the mouse is up, perform a drop, or continue if the 
            //mouse if down.

            try
            {
                if (e.EscapePressed)
                {
                    e.Action = System.Windows.Forms.DragAction.Cancel;
                }
                else if ((e.KeyState & 0x01) == 0)
                {
                    if (_dragState == DragState.dragNone)
                    {
                        e.Action = DragAction.Cancel;
                    }
                    else
                    {
                        e.Action = DragAction.Drop;
                    }
                }
                else
                {
                    e.Action = System.Windows.Forms.DragAction.Continue;
                }
            }
            catch (Exception exc)
            {
                HandleExceptions(exc);
            }
        }

        private void gTop_DragDrop(object sender, System.Windows.Forms.DragEventArgs e)
        {
            //This event gets fired once the user releases the mouse button during
            //a drag-drop action.
            //In this procedure, move the columns in both gTop and gMiddle.

            int _dragStopColumn = gTop.MouseCol; //which column did the user halt.

            try
            {
                if (_dragStartColumn < _dragStopColumn) //we are moving from left to right
                {
                    try
                    {
                        gTop.Cols.MoveRange(_dragStartColumn, 1, _dragStopColumn);
                        gMiddle.Cols.MoveRange(_dragStartColumn, 1, _dragStopColumn);
                    }
                    catch (System.ArgumentException)
                    {
                        //This exception occurs when the user is trying to drag a column into an empty space
                        //in the grid, thus producing a "-1" for the _dragStopColumn. When this happens, we
                        //just ignore the error and pretend that nothing happened.
                    }
                    catch (Exception exc)
                    {
                        HandleExceptions(exc);
                    }
                }

                if (_dragStartColumn > _dragStopColumn) //move right to left
                {
                    try
                    {
                        gTop.Cols.MoveRange(_dragStartColumn, 1, _dragStopColumn);
                        gMiddle.Cols.MoveRange(_dragStartColumn, 1, _dragStopColumn);
                    }
                    catch (System.ArgumentException)
                    {
                        //This exception occurs when the user is trying to drag a column into an empty space
                        //in the grid, thus producing a "-1" for the _dragStopColumn. When this happens, we
                        //just ignore the error and pretend that nothing happened.
                    }
                    catch (Exception exc)
                    {
                        HandleExceptions(exc);
                    }
                }

                //Finally, we want to clear the _dragState. 
                //This is an important clean-up step.
                _dragState = DragState.dragNone;
            }
            catch (Exception exc)
            {
                HandleExceptions(exc);
            }
        }

        #endregion

        #region Drag/Drop rows

        private void gLeft_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            try
            {
                _gLeftMouseCol = gLeft.MouseCol;
                _gLeftMouseRow = gLeft.MouseRow;

                if (e.Button == MouseButtons.Left)
                {
                    //If left button is pressed, set _dragState to "dragReady" and store the 
                    //initial row and column range. The "ready" state indicates a drag
                    //operation is to begin if the mouse moves while the mouse is down.

                    //we want to enable drag-drop when the user clicks on the heading row (row 0).
                    if (_gLeftMouseCol == 0 &&
                        (_dragState == DragState.dragNone || _dragState == DragState.dragReady))
                    {
                        _dragState = DragState.dragReady;
                        _dragStartRow = gLeft.MouseRow;
                    }
                }
                else
                {
//					gLeft.MouseRow
                }
            }
            catch (Exception exc)
            {
                HandleExceptions(exc);
            }
        }

        private void gLeft_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            //if the _dragState is "dragReady", set the _dragState to "started" 
            //and begin the drag.
            try
            {
                if (_dragState == DragState.dragReady)
                {
                    _dragState = DragState.dragStarted;
                    gLeft.DoDragDrop(sender, DragDropEffects.All);
                }
            }
            catch (Exception exc)
            {
                HandleExceptions(exc);
            }
        }

        private void gLeft_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            //If the button is released, set the _dragState to "dragNone"
            try
            {
                _dragState = DragState.dragNone;
            }
            catch (Exception exc)
            {
                HandleExceptions(exc);
            }
        }

        private void gLeft_DragEnter(object sender, System.Windows.Forms.DragEventArgs e)
        {
            //Check the keystate(state of mouse buttons, among other things). 
            //If the left mouse is down, we're okay. Else, set _dragState
            //to "none", which will cause the drag operation to be cancelled at the 
            //next QueryContinueDrag call. This situation occurs if the user
            //releases the mouse button outside of the grid.
            try
            {
                if ((e.KeyState & 0x01) == 1)
                {
                    e.Effect = DragDropEffects.All;
                }
                else
                {
                    _dragState = DragState.dragNone;
                }
            }
            catch (Exception exc)
            {
                HandleExceptions(exc);
            }
        }

        private void gLeft_DragOver(object sender, System.Windows.Forms.DragEventArgs e)
        {
            //During drag over, if the mouse is dragged to the left- or right-most
            //part of the grid, scroll grid to show the columns.

            int mouseY;

            try
            {
                mouseY = e.Y - (this.Top + ((this.Size.Height - this.ClientSize.Height)/2));
                if (mouseY > gLeft.Bottom - 20 && mouseY < gLeft.Bottom)
                {
                    gLeft.TopRow ++;
                }
                else if (mouseY > gLeft.Top && mouseY < gLeft.Top + 20)
                {
                    gLeft.TopRow --;
                }
            }
            catch (Exception exc)
            {
                HandleExceptions(exc);
            }
        }

        private void gLeft_QueryContinueDrag(object sender, System.Windows.Forms.QueryContinueDragEventArgs e)
        {
            //Check to see if the drag should continue. 
            //Cancel if:
            //(1) the escape key is pressed
            //(2) the DragEnter event handler cancels the drag by setting the 
            //		_dragState to "none".
            //Otherwise, if the mouse is up, perform a drop, or continue if the 
            //mouse if down.

            try
            {
                if (e.EscapePressed)
                {
                    e.Action = System.Windows.Forms.DragAction.Cancel;
                }
                else if ((e.KeyState & 0x01) == 0)
                {
                    if (_dragState == DragState.dragNone)
                    {
                        e.Action = DragAction.Cancel;
                    }
                    else
                    {
                        e.Action = DragAction.Drop;
                    }
                }
                else
                {
                    e.Action = System.Windows.Forms.DragAction.Continue;
                }
            }
            catch (Exception exc)
            {
                HandleExceptions(exc);
            }
        }

        private void gLeft_DragDrop(object sender, System.Windows.Forms.DragEventArgs e)
        {
            //This event gets fired once the user releases the mouse button during
            //a drag-drop action.
            //In this procedure, move the columns in both gTop and gMiddle.

            int _dragStopRow = gLeft.MouseRow; //which column did the user halt.

            try
            {
                if (_dragStartRow < _dragStopRow) //we are moving from left to right
                {
                    try
                    {
                        gLeft.Rows.MoveRange(_dragStartRow, 1, _dragStopRow);
                        gMiddle.Rows.MoveRange(_dragStartRow, 1, _dragStopRow);
                    }
                    catch (System.ArgumentException)
                    {
                        //This exception occurs when the user is trying to drag a column into an empty space
                        //in the grid, thus producing a "-1" for the _dragStopColumn. When this happens, we
                        //just ignore the error and pretend that nothing happened.
                    }
                    catch (Exception exc)
                    {
                        HandleExceptions(exc);
                    }
                }

                if (_dragStartRow > _dragStopRow) //move right to left
                {
                    try
                    {
                        gLeft.Rows.MoveRange(_dragStartRow, 1, _dragStopRow);
                        gMiddle.Rows.MoveRange(_dragStartRow, 1, _dragStopRow);
                    }
                    catch (System.ArgumentException)
                    {
                        //This exception occurs when the user is trying to drag a column into an empty space
                        //in the grid, thus producing a "-1" for the _dragStopColumn. When this happens, we
                        //just ignore the error and pretend that nothing happened.
                    }
                    catch (Exception exc)
                    {
                        HandleExceptions(exc);
                    }
                }

                //Finally, we want to clear the _dragState. 
                //This is an important clean-up step.
                _dragState = DragState.dragNone;
            }
            catch (Exception exc)
            {
                HandleExceptions(exc);
            }
        }

        #endregion

        #endregion

        private void gTop_BeforeEdit(object sender, C1.Win.C1FlexGrid.RowColEventArgs e)
        {
            if (FormLoaded && !FunctionSecurity.AllowUpdate)
            {
                e.Cancel = true;
            }
        }

        private void gLeft_BeforeEdit(object sender, C1.Win.C1FlexGrid.RowColEventArgs e)
        {
            if (FormLoaded && !FunctionSecurity.AllowUpdate)
            {
                e.Cancel = true;
            }
        }

        private void gMiddle_BeforeEdit(object sender, C1.Win.C1FlexGrid.RowColEventArgs e)
        {
            if (FormLoaded && !FunctionSecurity.AllowUpdate)
            {
                e.Cancel = true;
            }
        }

        private void mnuItemRowInsert_Click(object sender, System.EventArgs e)
        {
            try
            {

            }
            catch (Exception exc)
            {
                HandleExceptions(exc);
            }
        }

        private void mnuItemRowInsertBefore_Click(object sender, System.EventArgs e)
        {
            try
            {
                ChangePending = true;
                gLeft.Rows.Insert(_gLeftMouseRow);
                gMiddle.Rows.Insert(_gLeftMouseRow);
            }
            catch (Exception exc)
            {
                HandleExceptions(exc);
            }
        }

        private void mnuItemRowInsertAfter_Click(object sender, System.EventArgs e)
        {
            try
            {
                ChangePending = true;
                gLeft.Rows.Insert(_gLeftMouseRow + 1);
                gMiddle.Rows.Insert(_gLeftMouseRow + 1);
            }
            catch (Exception exc)
            {
                HandleExceptions(exc);
            }
        }

        private void mnuItemRowDelete_Click(object sender, System.EventArgs e)
        {
            try
            {
                ChangePending = true;
                gLeft.Rows.Remove(_gLeftMouseRow);
                gMiddle.Rows.Remove(_gLeftMouseRow);
            }
            catch (Exception exc)
            {
                HandleExceptions(exc);
            }
        }

        private void gLeft_BeforeDeleteRow(object sender, C1.Win.C1FlexGrid.RowColEventArgs e)
        {
            try
            {
                ChangePending = true;
                gMiddle.Rows.Remove(e.Row);
            }
            catch (Exception exc)
            {
                HandleExceptions(exc);
            }
        }

        private void gLeft_BeforeAddRow(object sender, C1.Win.C1FlexGrid.RowColEventArgs e)
        {
            try
            {
                ChangePending = true;
                gMiddle.Rows.Insert(e.Row);
            }
            catch (Exception exc)
            {
                HandleExceptions(exc);
            }
        }

        private void mnuItemColInsert_Click(object sender, System.EventArgs e)
        {
            try
            {

            }
            catch (Exception exc)
            {
                HandleExceptions(exc);
            }
        }

        private void mnuItemColInsertBefore_Click(object sender, System.EventArgs e)
        {
            try
            {
                ChangePending = true;
                gTop.Cols.Insert(_gTopMouseCol);
                gMiddle.Cols.Insert(_gTopMouseCol);
            }
            catch (Exception exc)
            {
                HandleExceptions(exc);
            }
        }

        private void mnuItemColInsertAfter_Click(object sender, System.EventArgs e)
        {
            try
            {
                ChangePending = true;
                gTop.Cols.Insert(_gTopMouseCol + 1);
                gMiddle.Cols.Insert(_gTopMouseCol + 1);
            }
            catch (Exception exc)
            {
                HandleExceptions(exc);
            }
        }

        private void mnuItemColDelete_Click(object sender, System.EventArgs e)
        {
            try
            {
                ChangePending = true;
                gTop.Cols.Remove(_gTopMouseCol);
                gMiddle.Cols.Remove(_gTopMouseCol);
            }
            catch (Exception exc)
            {
                HandleExceptions(exc);
            }
        }

        #region Size Dropdown Filter

        // BEGIN MID Track #4396 - Justin Bolles - Size Dropdown Filter
        private void picBoxFilter_MouseHover(object sender, System.EventArgs e)
        {
            try
            {
                string message = MIDText.GetTextOnly((int) eMIDTextCode.tt_ClickToFilterDropDown);
                ToolTip.Active = true;
                ToolTip.SetToolTip((System.Windows.Forms.Control) sender, message);
            }
            catch (Exception exception)
            {
                HandleException(exception);
            }
        }

        private void picBox_Click(object sender, System.EventArgs e)
        {
            try
            {
                string enteredMask = string.Empty;
                bool caseSensitive = false;
                PictureBox picBox = (PictureBox) sender;

                if (CharMaskFromDialogOK(picBox, ref enteredMask, ref caseSensitive))
                {
                    //MessageBox.Show("Filter selection process not yet available");
                    BindSizeGroupComboBox(true, enteredMask, caseSensitive);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        protected void DisplayPictureBoxImages()
        {
            DisplayPictureBoxImage(picBoxGroup);
        }

        protected void SetPictureBoxTags()
        {
            picBoxGroup.Tag = _SAB.ApplicationServerSession.GlobalOptions.SizeGroupCharMask;
        }

        protected void SetMaskedComboBoxesEnabled()
        {
            if (_SAB.ApplicationServerSession.GlobalOptions.SizeGroupCharMask != string.Empty)
            {
                this.cboGroupName.Enabled = false;
            }

            picBoxGroup.Enabled = true; // MID Track #5256 - If security View only, can't select Size Groups when mask 
            // exists becuse of above check, but picBox is disabled from SetReadOnly; 
            // this overrides SetReadOnly
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
                string dialogLabel = MIDText.GetTextOnly((int) eMIDTextCode.lbl_FilterSelection);
                string textLabel = MIDText.GetTextOnly((int) eMIDTextCode.lbl_FilterSelectionText);

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
                        maskOK = (globalMask == string.Empty) ? true : EnteredMaskOK(aPicBox, aEnteredMask, globalMask);

                        if (!maskOK)
                        {
                            errMessage =
                                _SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_al_FilterGlobalOptionMismatch);
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

            if (!aEnteredMask.EndsWith("*"))
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

        // END MID Track #4396 - Justin Bolles - Size Dropdown Filter

        #endregion

        private void btnNew_Click(object sender, EventArgs e)
        {
            INew();
        }

        public override void INew()
        {
            try
            {
                Cursor = Cursors.WaitCursor;

                if (CheckForPendingChanges())
                {
                    // find new selection
                    for (int i = 0; i < cboGroupName.Items.Count; i++)
                    {
                        ComboObject comboObj = (ComboObject) cboGroupName.Items[i];
                        if (comboObj.Value == this._newText)
                        {
                            cboGroupName.SelectedIndex = i;
                            break;
                        }
                    }
                    ChangePending = false;
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

        public override void ISave()
        {

            try
            {
                _showSaveMsg = true;
                SaveChanges();
            }
            catch
            {
                throw;
            }
        }

        public override void ISaveAs()
        {

            try
            {
                _showSaveMsg = true;
                // Begin TT#2700 - JSmith - New Size Group in 5.0
                //SaveAs();
                SaveAs(false);
                // End TT#2700 - JSmith - New Size Group in 5.0
            }
            catch
            {
                throw;
            }
        }

        public override void IDelete()
        {

            try
            {
                SizeGroupProfile sgp = (SizeGroupProfile) _sgl.FindKey(_groupRID);

                if (sgp == null)
                {
                    MessageBox.Show(MIDText.GetText(eMIDTextCode.msg_PleaseSelectGroupToDelete));
                    return;
                }

                string text = MIDText.GetTextOnly(eMIDTextCode.msg_DeleteSizeGroup);
                text = text.Replace("{0}", sgp.SizeGroupName);
                DialogResult ret = MessageBox.Show(text, "",
                                                   MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                                                   MessageBoxDefaultButton.Button2);
                if (ret == DialogResult.No)
                {
                    return;
                }
                //BEGIN TT#110-MD-VStuart - In Use Tool
                var allowDelete = false;
                var _sgpArrayList = new ArrayList();
                _sgpArrayList.Add(sgp.Key);
                //string inUseTitle = Regex.Replace(eProfileType.SizeGroup.ToString(), "((?<=[a-z])[A-Z]|[A-Z](?=[a-z]))", " $1").Trim();
                string inUseTitle = InUseUtility.GetInUseTitleFromProfileType(eProfileType.SizeGroup); //TT#4304 -jsobek -Store Characteristic In Use not being reported on Store Filters
                DisplayInUseForm(_sgpArrayList, eProfileType.SizeGroup, inUseTitle, false, out allowDelete);
                if (!allowDelete)
                {
                    sgp.DeleteSizeGroup();

                    // need to update group list and clear form
                    btnClear_Click((object) null, (System.EventArgs) null);

                    _sgl.Remove(sgp);

                    // re-load group names
                    _ignoreFlag = true;


                    ComboObject comboObj = (ComboObject) cboGroupName.SelectedItem;
                    cboGroupName.Items.Remove(comboObj);
                    int index = cboGroupName.FindStringExact(_newText);
                    cboGroupName.SelectedIndex = index;

                    rdbLike.Checked = true;
                    txtDescription.Text = "";
                    index = cboProductCat.FindStringExact(_newText);
                    cboProductCat.SelectedIndex = index;
                    _ignoreFlag = false;
                    _groupRID = Include.NoRID;
                    MessageBox.Show("Delete completed", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ChangePending = false;
                }
                //END TT#110-MD-VStuart - In Use Tool
            }
            catch (Exception exc)
            {
                HandleExceptions(exc);
            }
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

        private void btnSaveAs_Click(object sender, EventArgs e)
        {
            ISaveAs();
        }

        //BEGIN TT#110-MD-VStuart - In Use Tool
        public void ShowInUse()
        {
            var emp = new SizeGroupProfile(_modelRID);
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

