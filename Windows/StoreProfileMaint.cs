//using System;
//using System.IO;
//using System.Drawing;
//using System.Collections;
//using System.ComponentModel;
//using System.Windows.Forms;
//using System.Data;
//using System.Diagnostics;
//using System.Globalization;

//using Infragistics.Win.UltraWinGrid;
//using Infragistics.Shared;
//using Infragistics.Win;
//using Infragistics.Win.UltraWinListBar;
//using Infragistics.Win.UltraWinTree;
//using Infragistics.Win.UltraWinMaskedEdit;


//using MIDRetail.Business;
//using MIDRetail.Common;
//using MIDRetail.Data;
//using MIDRetail.DataCommon;


//namespace MIDRetail.Windows
//{
//    /// <summary>
//    /// Summary description for StoreProfileMaint.
//    /// </summary>
//    public class StoreProfileMaint : MIDFormBase
//    {
//        private SessionAddressBlock _SAB;
//        private ExplorerAddressBlock _EAB;
//        // add event to update explorer when new group is added
//        //Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
//        //public delegate void StoreGroupChangeEventHandler(object source, StoreGroupChangeEventArgs e);
//        //public event StoreGroupChangeEventHandler OnStoreGroupPropertyChangeHandler;
//        //End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders

//        private Infragistics.Win.UltraWinGrid.UltraGrid ultraGrid1;
//        private Infragistics.Win.UltraWinGrid.UltraDropDown stateDropDown;
//        /// <summary>
//        /// Required designer variable.
//        /// </summary>
//        private System.ComponentModel.Container components = null;
//        private DataTable _dtStoreProfile;
//        private DataSet _dsStoreProfile;

//        private int _tempRID = -1;

//        private GlobalOptions _go;

//        private frmUltraGridSearchReplace _frmUltraGridSearchReplace;
//        private UltraGridColumn _gridCol;

//        StoreServerSession _storeSession;
//        //Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
//        private FolderDataLayer _dlFolder;
//        //End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
//        private System.Windows.Forms.Button btnSaveToGroups;
//        private System.Windows.Forms.Button btnSaveView;
//        private System.Windows.Forms.Button btnRestoreView;
//        private System.Windows.Forms.Button btnSave;

//        private DataTable _dtHeaderText;
//        private System.Windows.Forms.ContextMenu mnuGridHeader;
//        private System.Windows.Forms.MenuItem menuSortAsc;
//        private System.Windows.Forms.MenuItem menuSortDesc;
//        private System.Windows.Forms.MenuItem menuSearch;

//        private ArrayList _insertedRows;
//        private const string _emptyDescription = "Null";

//        private Point _point;

//        private int _startAtStore;
//        //Begin TT#724 - JScott - The "Available Stores" attribute set can be renamed in the Store Group Explorer and could cause processing problems
//        //private string _availableStoresText = MIDText.GetTextOnly(eMIDTextCode.lbl_AvailableStores);
//        //End TT#724 - JScott - The "Available Stores" attribute set can be renamed in the Store Group Explorer and could cause processing problems

//        private DateTime _sellingOpenDt = Include.UndefinedDate;
//        private DateTime _sellingCloseDt = Include.UndefinedDate;
//        private DateTime _stockOpenDt = Include.UndefinedDate;
//        private DateTime _stockCloseDt = Include.UndefinedDate;
//        private UltraGridCell _invalidCell;
//        private DataTable _dtStoreStatus;
//        Infragistics.Shared.ResourceCustomizer rc;
//        private string _storeStatusText;
//        private string [] tempList = { "ACTIVE_IND_B","SHIP_ON_MONDAY_B","SHIP_ON_TUESDAY_B","SHIP_ON_WEDNESDAY_B",
//                                         "SHIP_ON_THURSDAY_B","SHIP_ON_FRIDAY_B","SHIP_ON_SATURDAY_B","SHIP_ON_SUNDAY_B" };
//        private ArrayList BOOL_COLUMNS = new ArrayList();
//        FunctionSecurityProfile _StoreAttributeSecurity;
//        // Begin Track #4872 - JSmith - Global/User Attributes
//        FunctionSecurityProfile _UserStoreAttributeSecurity;
//        // End Track #4872

//        bool _storeActiveChanged = false;

//        // Begin TT#3139 - JSmith - Store Profiles – read-only access
//        private bool _editEnabled = false;
//        FunctionSecurityProfile _ReadOnlyFunctionSecurity;
//        // End TT#3139 - JSmith - Store Profiles – read-only access


//        #region RowDescription structure
//        /// <summary>
//        /// Structure used only in the "Save Grid to Groups" method.
//        /// holds all of the Row Descriptions.
//        /// </summary>
//        private struct RowDescription
//        {
//            private int _level;
//            private string _description;

//            public int Level 
//            {
//                get{return _level;}
//                set{_level = value;}
//            }
//            public string Description
//            {
//                get{return _description;}
//                set{_description = value;}
//            }
//        }
//        #endregion

//        #region RowLevel structure
//        /// <summary>
//        /// Structure used only in the "Save Grid to Groups" method.
//        /// holds all of the Row Group-by information.
//        /// </summary>
//        private struct RowLevel
//        {
//            private string _name;
//            private string _columnName;
//            private string _sqlPlaceHolderName;
//            private DataCommon.eStoreCharType _dataType;
//            private bool _isCharacteristic;
//            private bool _isList;
//            // if this IS a characterist, the columnId will be the characteristic value Key.
//            // oftherwise the columnId will be the enum label for the STORES column.
//            private int	_columnId;

//            /// <summary>
//            /// for characteristics it will equal "*SCGRID__#" where # is the store char group rid.  This is
//            /// used in the SQL.
//            /// If not a characteristic, then same as ColumnName.
//            /// </summary>
//            public string SqlName 
//            {
//                get{return _name;}
//                set{_name = value;}
//            }
//            /// <summary>
//            /// Actual Column name or characteristic name
//            /// </summary>
//            public string ColumnName 
//            {
//                get{return _columnName;}
//                set{_columnName = value;}
//            }
//            public string SqlPlaceHolderName 
//            {
//                get{return _sqlPlaceHolderName;}
//                set{_sqlPlaceHolderName = value;}
//            }
//            public DataCommon.eStoreCharType DataType
//            {
//                get{return _dataType;}
//                set{_dataType = value;}
//            }
//            public bool IsCharacteristic 
//            {
//                get{return _isCharacteristic;}
//                set{_isCharacteristic = value;}
//            }
//            public bool IsList
//            {
//                get{return _isList;}
//                set{_isList = value;}
//            }
//            public int ColumnId 
//            {
//                get{return _columnId;}
//                set{_columnId = value;}
//            }
//        }
//        #endregion

//        public StoreProfileMaint(SessionAddressBlock aSAB, ExplorerAddressBlock aEAB)  : base (aSAB)
//        {
//            _SAB = aSAB;
//            _EAB = aEAB;
//            InitializeComponent();
//            BOOL_COLUMNS.AddRange(tempList);

//            _storeSession = _SAB.StoreServerSession;
//            _go = new GlobalOptions();
//            _insertedRows = new ArrayList();

//            FunctionSecurity = _SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AdminStoresProfiles);
//            // Begin Issue 3950 - stodd
//            // Begin Track #4872 - JSmith - Global/User Attributes
//            //_StoreAttributeSecurity = _SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AdminStoreAttributes);
//            _StoreAttributeSecurity = _SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AdminStoreAttributesGlobal);
//            // End Issue 3950
//            _UserStoreAttributeSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AdminStoreAttributesUser);
//            // End Track #4872
//            //Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
//            _dlFolder = new FolderDataLayer();
//            //End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
//        }

//        /// <summary>
//        /// Clean up any resources being used.
//        /// </summary>
//        protected override void Dispose( bool disposing )
//        {
//            rc = Infragistics.Win.UltraWinGrid.Resources.Customizer;
//            rc.ResetCustomizedString("DataErrorRowUpdateUnableToUpdateRow");

//            if( disposing )
//            {
//                if (components != null) 
//                {
//                    components.Dispose();
//                }
//                this.ultraGrid1.InitializeRow -= new Infragistics.Win.UltraWinGrid.InitializeRowEventHandler(this.ultraGrid1_InitializeRow);
//                this.ultraGrid1.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.ultraGrid1_MouseDown);
//                this.ultraGrid1.MouseUp -= new System.Windows.Forms.MouseEventHandler(this.ultraGrid1_MouseUp);
//                this.ultraGrid1.CellChange -= new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ultraGrid1_CellChange);
//                this.ultraGrid1.BeforeExitEditMode -= new Infragistics.Win.UltraWinGrid.BeforeExitEditModeEventHandler(this.ultraGrid1_BeforeExitEditMode);
//                this.ultraGrid1.AfterRowInsert -= new Infragistics.Win.UltraWinGrid.RowEventHandler(this.ultraGrid1_AfterRowInsert);
//                this.ultraGrid1.Validated -= new System.EventHandler(this.ultraGrid1_Validated);
//                this.ultraGrid1.BeforeCellUpdate -= new Infragistics.Win.UltraWinGrid.BeforeCellUpdateEventHandler(this.ultraGrid1_BeforeCellUpdate);
//                this.ultraGrid1.BeforeRowsDeleted -= new Infragistics.Win.UltraWinGrid.BeforeRowsDeletedEventHandler(this.ultraGrid1_BeforeRowsDeleted);
//                this.ultraGrid1.BeforeRowInsert -= new Infragistics.Win.UltraWinGrid.BeforeRowInsertEventHandler(this.ultraGrid1_BeforeRowInsert);
//                this.ultraGrid1.KeyDown -= new System.Windows.Forms.KeyEventHandler(this.ultraGrid1_KeyDown);
//                this.ultraGrid1.Validating -= new System.ComponentModel.CancelEventHandler(this.ultraGrid1_Validating);
//                this.ultraGrid1.KeyUp -= new System.Windows.Forms.KeyEventHandler(this.ultraGrid1_KeyUp);
//                this.ultraGrid1.AfterCellUpdate -= new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ultraGrid1_AfterCellUpdate);
//                this.ultraGrid1.InitializeLayout -= new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.ultraGrid1_InitializeLayout);
//                //Begin TT#169 - JSmith - enter max of 1000 and receive 'must be integer'
//                MIDRetail.Windows.Controls.UltraGridLayoutDefaults ugld = new MIDRetail.Windows.Controls.UltraGridLayoutDefaults(ErrorImage);
//                ugld.DetachGridEventHandlers(ultraGrid1);
//                //End TT#169
//                this.ultraGrid1.BeforeCellDeactivate -= new System.ComponentModel.CancelEventHandler(this.ultraGrid1_BeforeCellDeactivate);
//                this.ultraGrid1.InitializeGroupByRow -= new Infragistics.Win.UltraWinGrid.InitializeGroupByRowEventHandler(this.ultraGrid1_InitializeGroupByRow);
//                this.ultraGrid1.BeforeEnterEditMode -= new System.ComponentModel.CancelEventHandler(this.ultraGrid1_BeforeEnterEditMode);
//                this.btnSaveToGroups.Click -= new System.EventHandler(this.btnSaveToGroups_Click);
//                this.btnSaveView.Click -= new System.EventHandler(this.btnSaveView_Click);
//                this.btnRestoreView.Click -= new System.EventHandler(this.btnRestoreView_Click);
//                this.btnSave.Click -= new System.EventHandler(this.btnSave_Click);
//                this.menuSortAsc.Click -= new System.EventHandler(this.menuSortAsc_Click);
//                this.menuSortDesc.Click -= new System.EventHandler(this.menuSortDesc_Click);
//                this.menuSearch.Click -= new System.EventHandler(this.menuSearch_Click);
//                this.Leave -= new System.EventHandler(this.StoreProfileMaint_Leave);
//                this.Load -= new System.EventHandler(this.StoreProfileMaint_Load);
//                this.Enter -= new System.EventHandler(this.StoreProfileMaint_Enter);
//            }
//            base.Dispose( disposing );
//        }

//        #region Windows Form Designer generated code
//        /// <summary>
//        /// Required method for Designer support - do not modify
//        /// the contents of this method with the code editor.
//        /// </summary>
//        private void InitializeComponent()
//        {
//            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
//            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
//            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
//            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
//            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
//            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
//            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
//            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
//            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
//            Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
//            Infragistics.Win.Appearance appearance11 = new Infragistics.Win.Appearance();
//            Infragistics.Win.Appearance appearance12 = new Infragistics.Win.Appearance();
//            this.ultraGrid1 = new Infragistics.Win.UltraWinGrid.UltraGrid();
//            this.btnSaveToGroups = new System.Windows.Forms.Button();
//            this.stateDropDown = new Infragistics.Win.UltraWinGrid.UltraDropDown();
//            this.btnSaveView = new System.Windows.Forms.Button();
//            this.btnRestoreView = new System.Windows.Forms.Button();
//            this.btnSave = new System.Windows.Forms.Button();
//            this.mnuGridHeader = new System.Windows.Forms.ContextMenu();
//            this.menuSortAsc = new System.Windows.Forms.MenuItem();
//            this.menuSortDesc = new System.Windows.Forms.MenuItem();
//            this.menuSearch = new System.Windows.Forms.MenuItem();
//            ((System.ComponentModel.ISupportInitialize)(this.utmMain)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.ultraGrid1)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.stateDropDown)).BeginInit();
//            this.SuspendLayout();
//            // 
//            // utmMain
//            // 
//            this.utmMain.MenuSettings.ForceSerialization = true;
//            this.utmMain.ToolbarSettings.ForceSerialization = true;
//            // 
//            // ultraGrid1
//            // 
//            this.ultraGrid1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
//                        | System.Windows.Forms.AnchorStyles.Left)
//                        | System.Windows.Forms.AnchorStyles.Right)));
//            appearance1.BackColor = System.Drawing.Color.White;
//            appearance1.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
//            appearance1.BackGradientStyle = Infragistics.Win.GradientStyle.ForwardDiagonal;
//            this.ultraGrid1.DisplayLayout.Appearance = appearance1;
//            this.ultraGrid1.DisplayLayout.InterBandSpacing = 10;
//            appearance2.BackColor = System.Drawing.Color.Transparent;
//            this.ultraGrid1.DisplayLayout.Override.CardAreaAppearance = appearance2;
//            appearance3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
//            appearance3.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
//            appearance3.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
//            appearance3.ForeColor = System.Drawing.Color.Black;
//            appearance3.TextHAlignAsString = "Center";
//            appearance3.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
//            this.ultraGrid1.DisplayLayout.Override.HeaderAppearance = appearance3;
//            appearance4.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
//            this.ultraGrid1.DisplayLayout.Override.RowAppearance = appearance4;
//            appearance5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
//            appearance5.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
//            appearance5.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
//            this.ultraGrid1.DisplayLayout.Override.RowSelectorAppearance = appearance5;
//            this.ultraGrid1.DisplayLayout.Override.RowSelectorWidth = 12;
//            this.ultraGrid1.DisplayLayout.Override.RowSpacingBefore = 2;
//            appearance6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
//            appearance6.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
//            appearance6.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
//            appearance6.ForeColor = System.Drawing.Color.Black;
//            this.ultraGrid1.DisplayLayout.Override.SelectedRowAppearance = appearance6;
//            this.ultraGrid1.DisplayLayout.RowConnectorColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
//            this.ultraGrid1.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.Solid;
//            this.ultraGrid1.Location = new System.Drawing.Point(0, 0);
//            this.ultraGrid1.Name = "ultraGrid1";
//            this.ultraGrid1.Size = new System.Drawing.Size(656, 416);
//            this.ultraGrid1.TabIndex = 0;
//            this.ultraGrid1.BeforeRowInsert += new Infragistics.Win.UltraWinGrid.BeforeRowInsertEventHandler(this.ultraGrid1_BeforeRowInsert);
//            this.ultraGrid1.BeforeCellUpdate += new Infragistics.Win.UltraWinGrid.BeforeCellUpdateEventHandler(this.ultraGrid1_BeforeCellUpdate);
//            this.ultraGrid1.KeyUp += new System.Windows.Forms.KeyEventHandler(this.ultraGrid1_KeyUp);
//            this.ultraGrid1.BeforeEnterEditMode += new System.ComponentModel.CancelEventHandler(this.ultraGrid1_BeforeEnterEditMode);
//            this.ultraGrid1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ultraGrid1_MouseDown);
//            this.ultraGrid1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ultraGrid1_MouseUp);
//            this.ultraGrid1.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.ultraGrid1_InitializeLayout);
//            this.ultraGrid1.BeforeRowsDeleted += new Infragistics.Win.UltraWinGrid.BeforeRowsDeletedEventHandler(this.ultraGrid1_BeforeRowsDeleted);
//            this.ultraGrid1.InitializeGroupByRow += new Infragistics.Win.UltraWinGrid.InitializeGroupByRowEventHandler(this.ultraGrid1_InitializeGroupByRow);
//            this.ultraGrid1.BeforeExitEditMode += new Infragistics.Win.UltraWinGrid.BeforeExitEditModeEventHandler(this.ultraGrid1_BeforeExitEditMode);
//            this.ultraGrid1.AfterRowInsert += new Infragistics.Win.UltraWinGrid.RowEventHandler(this.ultraGrid1_AfterRowInsert);
//            this.ultraGrid1.BeforeCellDeactivate += new System.ComponentModel.CancelEventHandler(this.ultraGrid1_BeforeCellDeactivate);
//            this.ultraGrid1.AfterCellUpdate += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ultraGrid1_AfterCellUpdate);
//            this.ultraGrid1.InitializeRow += new Infragistics.Win.UltraWinGrid.InitializeRowEventHandler(this.ultraGrid1_InitializeRow);
//            this.ultraGrid1.CellChange += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ultraGrid1_CellChange);
//            this.ultraGrid1.Validating += new System.ComponentModel.CancelEventHandler(this.ultraGrid1_Validating);
//            this.ultraGrid1.Validated += new System.EventHandler(this.ultraGrid1_Validated);
//            this.ultraGrid1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ultraGrid1_KeyDown);
//            // 
//            // btnSaveToGroups
//            // 
//            this.btnSaveToGroups.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
//            this.btnSaveToGroups.Location = new System.Drawing.Point(8, 424);
//            this.btnSaveToGroups.Name = "btnSaveToGroups";
//            this.btnSaveToGroups.Size = new System.Drawing.Size(224, 23);
//            this.btnSaveToGroups.TabIndex = 2;
//            this.btnSaveToGroups.Text = "Save Current Layout to Store &Attributes";
//            this.btnSaveToGroups.Click += new System.EventHandler(this.btnSaveToGroups_Click);
//            // 
//            // stateDropDown
//            // 
//            appearance7.BackColor = System.Drawing.Color.White;
//            appearance7.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
//            appearance7.BackGradientStyle = Infragistics.Win.GradientStyle.ForwardDiagonal;
//            this.stateDropDown.DisplayLayout.Appearance = appearance7;
//            this.stateDropDown.DisplayLayout.InterBandSpacing = 10;
//            appearance8.BackColor = System.Drawing.Color.Transparent;
//            this.stateDropDown.DisplayLayout.Override.CardAreaAppearance = appearance8;
//            appearance9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
//            appearance9.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
//            appearance9.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
//            appearance9.ForeColor = System.Drawing.Color.Black;
//            appearance9.TextHAlignAsString = "Center";
//            appearance9.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
//            this.stateDropDown.DisplayLayout.Override.HeaderAppearance = appearance9;
//            appearance10.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
//            this.stateDropDown.DisplayLayout.Override.RowAppearance = appearance10;
//            appearance11.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
//            appearance11.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
//            appearance11.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
//            this.stateDropDown.DisplayLayout.Override.RowSelectorAppearance = appearance11;
//            this.stateDropDown.DisplayLayout.Override.RowSelectorWidth = 12;
//            this.stateDropDown.DisplayLayout.Override.RowSpacingBefore = 2;
//            appearance12.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
//            appearance12.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
//            appearance12.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
//            appearance12.ForeColor = System.Drawing.Color.Black;
//            this.stateDropDown.DisplayLayout.Override.SelectedRowAppearance = appearance12;
//            this.stateDropDown.DisplayLayout.RowConnectorColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
//            this.stateDropDown.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.Solid;
//            this.stateDropDown.Location = new System.Drawing.Point(280, 392);
//            this.stateDropDown.Name = "stateDropDown";
//            this.stateDropDown.Size = new System.Drawing.Size(75, 23);
//            this.stateDropDown.TabIndex = 3;
//            this.stateDropDown.Visible = false;
//            // 
//            // btnSaveView
//            // 
//            this.btnSaveView.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
//            this.btnSaveView.Location = new System.Drawing.Point(244, 424);
//            this.btnSaveView.Name = "btnSaveView";
//            this.btnSaveView.Size = new System.Drawing.Size(144, 23);
//            this.btnSaveView.TabIndex = 4;
//            this.btnSaveView.Text = "Save Store Profile &View";
//            this.btnSaveView.Click += new System.EventHandler(this.btnSaveView_Click);
//            // 
//            // btnRestoreView
//            // 
//            this.btnRestoreView.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
//            this.btnRestoreView.Location = new System.Drawing.Point(400, 424);
//            this.btnRestoreView.Name = "btnRestoreView";
//            this.btnRestoreView.Size = new System.Drawing.Size(144, 23);
//            this.btnRestoreView.TabIndex = 5;
//            this.btnRestoreView.Text = "Restore Store Profile &View";
//            this.btnRestoreView.Click += new System.EventHandler(this.btnRestoreView_Click);
//            // 
//            // btnSave
//            // 
//            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
//            this.btnSave.Location = new System.Drawing.Point(557, 424);
//            this.btnSave.Name = "btnSave";
//            this.btnSave.Size = new System.Drawing.Size(88, 23);
//            this.btnSave.TabIndex = 6;
//            this.btnSave.Text = "&Save Changes";
//            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
//            // 
//            // mnuGridHeader
//            // 
//            this.mnuGridHeader.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
//            this.menuSortAsc,
//            this.menuSortDesc,
//            this.menuSearch});
//            // 
//            // menuSortAsc
//            // 
//            this.menuSortAsc.Index = 0;
//            this.menuSortAsc.Text = "Sort Ascending";
//            this.menuSortAsc.Click += new System.EventHandler(this.menuSortAsc_Click);
//            // 
//            // menuSortDesc
//            // 
//            this.menuSortDesc.Index = 1;
//            this.menuSortDesc.Text = "Sort Descending";
//            this.menuSortDesc.Click += new System.EventHandler(this.menuSortDesc_Click);
//            // 
//            // menuSearch
//            // 
//            this.menuSearch.Index = 2;
//            this.menuSearch.Text = "Search";
//            this.menuSearch.Click += new System.EventHandler(this.menuSearch_Click);
//            // 
//            // StoreProfileMaint
//            // 
//            this.AllowDragDrop = true;
//            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
//            this.ClientSize = new System.Drawing.Size(656, 453);
//            this.Controls.Add(this.btnSave);
//            this.Controls.Add(this.btnSaveView);
//            this.Controls.Add(this.btnRestoreView);
//            this.Controls.Add(this.stateDropDown);
//            this.Controls.Add(this.btnSaveToGroups);
//            this.Controls.Add(this.ultraGrid1);
//            this.Name = "StoreProfileMaint";
//            this.Text = "Store Profile";
//            this.Load += new System.EventHandler(this.StoreProfileMaint_Load);
//            this.Enter += new System.EventHandler(this.StoreProfileMaint_Enter);
//            this.Leave += new System.EventHandler(this.StoreProfileMaint_Leave);
//            this.Closing += new System.ComponentModel.CancelEventHandler(this.StoreProfileMaint_Closing);
//            this.Controls.SetChildIndex(this.ultraGrid1, 0);
//            this.Controls.SetChildIndex(this.btnSaveToGroups, 0);
//            this.Controls.SetChildIndex(this.stateDropDown, 0);
//            this.Controls.SetChildIndex(this.btnRestoreView, 0);
//            this.Controls.SetChildIndex(this.btnSaveView, 0);
//            this.Controls.SetChildIndex(this.btnSave, 0);
//            ((System.ComponentModel.ISupportInitialize)(this.utmMain)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.ultraGrid1)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.stateDropDown)).EndInit();
//            this.ResumeLayout(false);

//        }
//        #endregion

//        // Begin TT#3139 - JSmith - Store Profiles – read-only access
//        //private void StoreProfileMaint_Load(object sender, System.EventArgs e)
//        //{
//        //    try
//        //    {
//        //        _dtStoreProfile = _storeSession.GetAllStores().Copy();
//        //        _dtStoreProfile.Columns["ST_ID"].Unique = true;

//        //        _dsStoreProfile = MIDEnvironment.CreateDataSet("stores DS");
//        //        _dsStoreProfile.Tables.Add(_dtStoreProfile);

//        //        _storeStatusText = _storeSession.StoreStatusText;

//        //        AddCheckboxColumns();
//        //        UpdateCheckboxColumns();
//        //        ultraGrid1.DataSource = _dtStoreProfile;
//        //        ultraGrid1.DisplayLayout.Bands[0].Columns[1].SortIndicator = SortIndicator.Ascending;
//        //        ultraGrid1.ActiveRow = ultraGrid1.Rows[0];
//        //        ultraGrid1.DisplayLayout.Bands[0].Columns[1].SortIndicator = SortIndicator.None;


//        //        if (_startAtStore != 0)
//        //        {
//        //            StoreProfile_StartAtStore();
//        //        }

//        //        rc = Infragistics.Win.UltraWinGrid.Resources.Customizer;
//        //        rc.SetCustomizedString("DataErrorRowUpdateUnableToUpdateRow",
//        //            "Store ID must be unique.  Enter a unique Store ID before continuing.");

//        //        //SetReadOnly(FunctionSecurity.AllowUpdate);  //Security changes - 1/24/2005 vg
//        //        ApplySecurity();
//        //        if (FunctionSecurity.AllowUpdate)
//        //        {
//        //            Format_Title(eDataState.Updatable, eMIDTextCode.frm_Administration, MIDText.GetTextOnly(eMIDTextCode.frm_StoreProfileMaint));
//        //        }
//        //        else
//        //        {
//        //            Format_Title(eDataState.ReadOnly, eMIDTextCode.frm_Administration, MIDText.GetTextOnly(eMIDTextCode.frm_StoreProfileMaint));
//        //        }


//        //        //// Begin Issue 3950 - stodd
//        //        //// these are always enabled
//        //        //this.btnSaveView.Enabled = true;
//        //        //this.btnRestoreView.Enabled = true;
//        //        //// This button is connected to the store attribute set security
//        //        //if (_StoreAttributeSecurity.AllowUpdate)
//        //        //    this.btnSaveToGroups.Enabled = true;
//        //        //else
//        //        //    this.btnSaveToGroups.Enabled = false;
//        //        //// End issue 3950
//        //    }
//        //    catch (Exception exception)
//        //    {
//        //        HandleException(exception);
//        //    }
//        //}

//        private void StoreProfileMaint_Load(object sender, System.EventArgs e)
//        {
//            try
//            {
//                _dtStoreProfile = _storeSession.GetAllStores().Copy();

//                _ReadOnlyFunctionSecurity = new FunctionSecurityProfile(Include.NoRID);
//                _ReadOnlyFunctionSecurity.SetReadOnly();

//                _storeStatusText = _storeSession.StoreStatusText;

//                BuildStoreGrid(_dtStoreProfile, _ReadOnlyFunctionSecurity);
				
//                if (_startAtStore != 0)
//                {
//                    StoreProfile_StartAtStore();
//                }

//                rc = Infragistics.Win.UltraWinGrid.Resources.Customizer;
//                rc.SetCustomizedString("DataErrorRowUpdateUnableToUpdateRow", 
//                    "Store ID must be unique.  Enter a unique Store ID before continuing.");

//                ApplyNoEdit();
//                Format_Title(eDataState.ReadOnly, eMIDTextCode.frm_Administration, MIDText.GetTextOnly(eMIDTextCode.frm_StoreProfileMaint));

//                if (FunctionSecurity.AllowUpdate)
//                {
//                    btnSave.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Edit);
//                    btnSave.Enabled = true;
//                }
 
//            }
//            catch ( Exception exception )
//            {
//                HandleException(exception);
//            }		
//        }

//        private void StoreProfileMaint_LoadForUpdate()
//        {
//            try
//            {
//                this.ultraGrid1.BeginUpdate();
//                this.ultraGrid1.SuspendRowSynchronization();

//                DataTable dt = _storeSession.GetStoreProfilesForUpdate();
//                if (dt == null)
//                {
//                    return;
//                }
//                _dtStoreProfile = dt;

//                ultraGrid1.DataSource = null;
//                ultraGrid1.DisplayLayout.Reset();
//                BuildStoreGrid(_dtStoreProfile, FunctionSecurity);
//                ApplyAppearanceCenterText(ultraGrid1);
//                SetColumnAlignment();

//                ApplyEdit();
//                Format_Title(eDataState.Updatable, eMIDTextCode.frm_Administration, MIDText.GetTextOnly(eMIDTextCode.frm_StoreProfileMaint));

//                _editEnabled = true;
//                btnSave.Text = "&Save Changes";
//            }
//            catch (Exception exception)
//            {
//                HandleException(exception);
//            }
//            finally
//            {
//                this.ultraGrid1.ResumeRowSynchronization();
//                this.ultraGrid1.EndUpdate();
//            }
//        }

//        private void BuildStoreGrid(DataTable dtStoreProfiles, FunctionSecurityProfile aFunctionSecurityProfile)
//        {
//            try
//            {
//                _dtStoreProfile = dtStoreProfiles;
//                _dtStoreProfile.Columns["ST_ID"].Unique = true;

//                _dsStoreProfile = MIDEnvironment.CreateDataSet("stores DS");
//                _dsStoreProfile.Tables.Add(_dtStoreProfile);

               

//                AddCheckboxColumns();
//                UpdateCheckboxColumns();
//                ultraGrid1.DataSource = _dtStoreProfile;
//                ultraGrid1.DisplayLayout.Bands[0].Columns[1].SortIndicator = SortIndicator.Ascending;
//                ultraGrid1.ActiveRow = ultraGrid1.Rows[0];
//                ultraGrid1.DisplayLayout.Bands[0].Columns[1].SortIndicator = SortIndicator.None;
                
//                if (_startAtStore != 0)
//                {
//                    StoreProfile_StartAtStore();
//                }

//                ApplySecurity(aFunctionSecurityProfile);
//            }
//            catch (Exception exception)
//            {
//                HandleException(exception);
//            }
//        }

//        private void ApplyNoEdit()
//        {
//            foreach (UltraGridBand band in ultraGrid1.DisplayLayout.Bands)
//            {
//                foreach (Infragistics.Win.UltraWinGrid.UltraGridColumn column in band.Columns)
//                {
//                    if (!column.Hidden)
//                    {
//                        column.CellActivation = Activation.NoEdit;
//                    }
//                }
//            }
//        }

//        private void ApplyEdit()
//        {
//            foreach (UltraGridBand band in ultraGrid1.DisplayLayout.Bands)
//            {
//                foreach (Infragistics.Win.UltraWinGrid.UltraGridColumn column in band.Columns)
//                {
//                    if (!column.Hidden)
//                    {
//                        // Begin TT#879 - MD - stodd - "Marked for Deletion" shoulod always be "noEdit"
//                        if (column.Key == "STORE_DELETE_IND_B")
//                        {
//                            column.CellActivation = Activation.NoEdit;
//                        }
//                        else if (column.Key == _storeStatusText)
//                        // End TT#879 - MD - stodd - "Marked for Deletion" shoulod always be "noEdit"
//                        {
//                            column.CellActivation = Activation.ActivateOnly;
//                        }
//                        else
//                        {
//                            column.CellActivation = Activation.AllowEdit;
//                        }
//                    }
//                }
//            }
//        }

//        private void ApplySecurity(FunctionSecurityProfile aFunctionSecurityProfile)
//        {
//            SetReadOnly(aFunctionSecurityProfile.AllowUpdate);
//            this.btnSaveView.Enabled = true;
//            this.btnRestoreView.Enabled = true;
//            if (_StoreAttributeSecurity.AllowUpdate ||
//                _UserStoreAttributeSecurity.AllowUpdate)
//                this.btnSaveToGroups.Enabled = true;
//            else
//                this.btnSaveToGroups.Enabled = false;
//        }

//        //private void ApplySecurity()
//        //{
//        //    SetReadOnly(FunctionSecurity.AllowUpdate);
//        //    this.btnSaveView.Enabled = true;
//        //    this.btnRestoreView.Enabled = true;
//        //    // This button is connected to the store attribute set security
//        //    // Begin Track #4872 - JSmith - Global/User Attributes
//        //    //if (_StoreAttributeSecurity.AllowUpdate)
//        //    if (_StoreAttributeSecurity.AllowUpdate ||
//        //        _UserStoreAttributeSecurity.AllowUpdate)
//        //        // Begin Track #4872
//        //        this.btnSaveToGroups.Enabled = true;
//        //    else
//        //        this.btnSaveToGroups.Enabled = false;
//        //}

//        override protected void AfterClosing()
//        {
//            try
//            {
                
//                if (FunctionSecurity.AllowUpdate &&
//                    _editEnabled)
//                {
//                    _storeSession.DequeueStoreProfiles();
//                }
//            }
//            catch (Exception exception)
//            {
//                HandleException(exception);
//            }
//        }

//        // End TT#3139 - JSmith - Store Profiles – read-only access
//        private void AddCheckboxColumns()
//        {
//            DataColumn newColumn = new DataColumn();
//            newColumn.AllowDBNull = false; 
//            newColumn.Caption = "ACTIVE_IND_B"; 
//            newColumn.ColumnName = "ACTIVE_IND_B"; 
//            newColumn.DefaultValue = true;
//            newColumn.ReadOnly = false;
//            newColumn.DataType = System.Type.GetType("System.Boolean"); 	
//            _dtStoreProfile.Columns.Add(newColumn); 

//            newColumn = new DataColumn();
//            newColumn.AllowDBNull = false; 
//            newColumn.Caption = "SHIP_ON_MONDAY_B"; 
//            newColumn.ColumnName = "SHIP_ON_MONDAY_B"; 
//            newColumn.DefaultValue = false;
//            newColumn.ReadOnly = false;
//            newColumn.DataType = System.Type.GetType("System.Boolean"); 	
//            _dtStoreProfile.Columns.Add(newColumn); 

//            newColumn = new DataColumn();
//            newColumn.AllowDBNull = false; 
//            newColumn.Caption = "SHIP_ON_TUESDAY_B"; 
//            newColumn.ColumnName = "SHIP_ON_TUESDAY_B"; 
//            newColumn.DefaultValue = false;
//            newColumn.ReadOnly = false;
//            newColumn.DataType = System.Type.GetType("System.Boolean"); 	
//            _dtStoreProfile.Columns.Add(newColumn); 

//            newColumn = new DataColumn();
//            newColumn.AllowDBNull = false; 
//            newColumn.Caption = "SHIP_ON_WEDNESDAY_B"; 
//            newColumn.ColumnName = "SHIP_ON_WEDNESDAY_B"; 
//            newColumn.DefaultValue = false;
//            newColumn.ReadOnly = false;
//            newColumn.DataType = System.Type.GetType("System.Boolean"); 	
//            _dtStoreProfile.Columns.Add(newColumn); 

//            newColumn = new DataColumn();
//            newColumn.AllowDBNull = false; 
//            newColumn.Caption = "SHIP_ON_THURSDAY_B"; 
//            newColumn.ColumnName = "SHIP_ON_THURSDAY_B"; 
//            newColumn.DefaultValue = false;
//            newColumn.ReadOnly = false;
//            newColumn.DataType = System.Type.GetType("System.Boolean"); 	
//            _dtStoreProfile.Columns.Add(newColumn); 

//            newColumn = new DataColumn();
//            newColumn.AllowDBNull = true; 
//            newColumn.Caption = "SHIP_ON_FRIDAY_B"; 
//            newColumn.ColumnName = "SHIP_ON_FRIDAY_B"; 
//            newColumn.DefaultValue = false;
//            newColumn.ReadOnly = false;
//            newColumn.DataType = System.Type.GetType("System.Boolean"); 	
//            _dtStoreProfile.Columns.Add(newColumn); 

//            newColumn = new DataColumn();
//            newColumn.AllowDBNull = false; 
//            newColumn.Caption = "SHIP_ON_SATURDAY_B"; 
//            newColumn.ColumnName = "SHIP_ON_SATURDAY_B"; 
//            newColumn.DefaultValue = false;
//            newColumn.ReadOnly = false;
//            newColumn.DataType = System.Type.GetType("System.Boolean"); 	
//            _dtStoreProfile.Columns.Add(newColumn); 

//            newColumn = new DataColumn();
//            newColumn.AllowDBNull = false; 
//            newColumn.Caption = "SHIP_ON_SUNDAY_B"; 
//            newColumn.ColumnName = "SHIP_ON_SUNDAY_B"; 
//            newColumn.DefaultValue = false;
//            newColumn.ReadOnly = false;
//            newColumn.DataType = System.Type.GetType("System.Boolean"); 	
//            _dtStoreProfile.Columns.Add(newColumn); 

//            // Begin Issue 3557 stodd
//            newColumn = new DataColumn();
//            newColumn.AllowDBNull = false; 
//            newColumn.Caption = "SIMILAR_STORE_MODEL_B"; 
//            newColumn.ColumnName = "SIMILAR_STORE_MODEL_B"; 
//            newColumn.DefaultValue = false;
//            newColumn.ReadOnly = false;
//            newColumn.DataType = System.Type.GetType("System.Boolean"); 	
//            _dtStoreProfile.Columns.Add(newColumn); 
//            // Begin Issue 3557 stodd

//            // BEGIN TT#739-MD - STodd - delete stores
//            newColumn = new DataColumn();
//            newColumn.AllowDBNull = false;
//            newColumn.Caption = "STORE_DELETE_IND_B";
//            newColumn.ColumnName = "STORE_DELETE_IND_B";
//            newColumn.DefaultValue = false;
//            newColumn.ReadOnly = false;
//            newColumn.DataType = System.Type.GetType("System.Boolean");
//            _dtStoreProfile.Columns.Add(newColumn);
//            // END TT#739-MD - STodd - delete stores
//        }
		
//        private void UpdateCheckboxColumns()
//        {
//            foreach(DataRow dr in _dtStoreProfile.Rows)
//            {
//                if ((string)dr["ACTIVE_IND"] == "1")
//                    dr["ACTIVE_IND_B"] = true;
//                else
//                    dr["ACTIVE_IND_B"] = false;
//                if ((string)dr["SHIP_ON_MONDAY"] == "1")
//                    dr["SHIP_ON_MONDAY_B"] = true;
//                else
//                    dr["SHIP_ON_MONDAY_B"] = false;
//                if ((string)dr["SHIP_ON_TUESDAY"] == "1")
//                    dr["SHIP_ON_TUESDAY_B"] = true;
//                else
//                    dr["SHIP_ON_TUESDAY_B"] = false;
//                if ((string)dr["SHIP_ON_WEDNESDAY"] == "1")
//                    dr["SHIP_ON_WEDNESDAY_B"] = true;
//                else
//                    dr["SHIP_ON_WEDNESDAY_B"] = false;
//                if ((string)dr["SHIP_ON_THURSDAY"] == "1")
//                    dr["SHIP_ON_THURSDAY_B"] = true;
//                else
//                    dr["SHIP_ON_THURSDAY_B"] = false;
//                if ((string)dr["SHIP_ON_FRIDAY"] == "1")
//                    dr["SHIP_ON_FRIDAY_B"] = true;
//                else
//                    dr["SHIP_ON_FRIDAY_B"] = false;
//                if ((string)dr["SHIP_ON_SATURDAY"] == "1")
//                    dr["SHIP_ON_SATURDAY_B"] = true;
//                else
//                    dr["SHIP_ON_SATURDAY_B"] = false;
//                if ((string)dr["SHIP_ON_SUNDAY"] == "1")
//                    dr["SHIP_ON_SUNDAY_B"] = true;
//                else
//                    dr["SHIP_ON_SUNDAY_B"] = false;
//                if ((string)dr["SIMILAR_STORE_MODEL"] == "1")
//                    dr["SIMILAR_STORE_MODEL_B"] = true;
//                else
//                    dr["SIMILAR_STORE_MODEL_B"] = false;
//                // BEGIN TT#739-MD - STodd - delete stores
//                if (dr["STORE_DELETE_IND"] != DBNull.Value && (string)dr["STORE_DELETE_IND"] == "1")
//                    dr["STORE_DELETE_IND_B"] = true;
//                else
//                    dr["STORE_DELETE_IND_B"] = false;
//                // END TT#739-MD - STodd - delete stores
//            }
//            _dtStoreProfile.AcceptChanges();
//        }

//        private void StoreProfileMaint_LoadDropDowns()
//        {
//            // States on STATE column

//            string [] states = _go.GetStateAbbreviationsArray();

//            ultraGrid1.DisplayLayout.ValueLists.Add("STATE");
//            ultraGrid1.DisplayLayout.ValueLists["STATE"].ValueListItems.Clear();
//            ultraGrid1.DisplayLayout.ValueLists["STATE"].ValueListItems.Add(string.Empty);
//            for (short s=0; s<states.Length; s++)
//                ultraGrid1.DisplayLayout.ValueLists["STATE"].ValueListItems.Add(states[s]);
							
//            ultraGrid1.DisplayLayout.Bands[0].Columns["STATE"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
//            ultraGrid1.DisplayLayout.Bands[0].Columns["STATE"].ValueList = ultraGrid1.DisplayLayout.ValueLists["STATE"];

//            //===============================
//            // Store Status value list
//            //===============================
//            ultraGrid1.DisplayLayout.ValueLists.Add("Store Status");
//            _dtStoreStatus = MIDText.GetLabels((int) eStoreStatus.New,(int) eStoreStatus.Preopen);
//            foreach (DataRow row in _dtStoreStatus.Rows)
//            {
//                int textCode = Convert.ToInt32(row["TEXT_CODE"], CultureInfo.CurrentUICulture);
//                ultraGrid1.DisplayLayout.ValueLists["Store Status"].ValueListItems.Add(textCode, row["TEXT_VALUE"].ToString());
//            }

//            // Handle the Store Status column
//            ultraGrid1.DisplayLayout.Bands[0].Columns["Store Status"].ValueList = ultraGrid1.DisplayLayout.ValueLists["Store Status"];
//            ultraGrid1.DisplayLayout.Bands[0].Columns["Store Status"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Edit;
//            ultraGrid1.DisplayLayout.Bands[0].Columns["Store Status"].CellActivation = Activation.ActivateOnly;

//            // Dynamic Characteristics
//            ArrayList characteristics = _storeSession.GetStoreCharacteristicList();

//            foreach(UltraGridColumn uc in this.ultraGrid1.DisplayLayout.Bands[0].Columns)
//            {
//                for (int c=0;c<characteristics.Count;c++)
//                {
//                    CharacteristicGroup cg = (CharacteristicGroup)characteristics[c];
//                    if ( uc.Header.Caption == cg.Name && cg.HasList)
//                    {
//                        ultraGrid1.DisplayLayout.ValueLists.Add(cg.Name);
//                        ultraGrid1.DisplayLayout.ValueLists[cg.Name].ValueListItems.Clear();

//                        ultraGrid1.DisplayLayout.ValueLists[cg.Name].ValueListItems.Add("<none>","<none>");
//                        foreach(StoreCharValue ddValue in cg.Values)
//                        {
//                            if (cg.DataType == eStoreCharType.date)
//                            {
//                                ultraGrid1.DisplayLayout.ValueLists[cg.Name].ValueListItems.Add((DateTime)ddValue.CharValue, ((DateTime)ddValue.CharValue).ToShortDateString());
//                            }
//                            else
//                            {
//                                ultraGrid1.DisplayLayout.ValueLists[cg.Name].ValueListItems.Add(ddValue.CharValue.ToString(), ddValue.CharValue.ToString());
//                            }
							
//                        }
						
//                        uc.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
//                        uc.ValueList = ultraGrid1.DisplayLayout.ValueLists[cg.Name];
//                    }
//                }
//            }
//        }
		
//        private void ultraGrid1_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
//        {
//            // check for saved layout
//            InfragisticsLayoutData layoutData = new InfragisticsLayoutData();
//            InfragisticsLayout layout = layoutData.InfragisticsLayout_Read(_SAB.ClientServerSession.UserRID, eLayoutID.storeMaintGrid);

//            if (layout.LayoutLength > 0)
//            {
//                ultraGrid1.DisplayLayout.Load(layout.LayoutStream);

//                // Begin MID Issue - stodd
//                // When layout was saved and new characteristics are added, we need to hide the
//                // 'key'; columns from the user.
//                foreach (UltraGridColumn cColumn in this.ultraGrid1.DisplayLayout.Bands[0].Columns)
//                {
//                    //===========================================
//                    // Hide store char group value 'key' columns
//                    //===========================================
//                    if (cColumn.Header.Caption.StartsWith("SCGRID__"))
//                        cColumn.Hidden = true;
//                }
//                // End MID Issue - stodd

//                // Begin MID Issue 4065 - stodd
//                ultraGrid1.DisplayLayout.ValueLists.Clear();
//                StoreProfileMaint_LoadDropDowns();
//                // End MID Issue 4065 - stodd

//            }
//            else
//            {	// DEFAULT grid layout
//                //Begin TT#169 - JSmith - enter max of 1000 and receive 'must be integer'
//                //MIDRetail.Windows.Controls.UltraGridLayoutDefaults ugld = new MIDRetail.Windows.Controls.UltraGridLayoutDefaults();
//                //ugld.ApplyDefaults(e);
//                MIDRetail.Windows.Controls.UltraGridLayoutDefaults ugld = new MIDRetail.Windows.Controls.UltraGridLayoutDefaults(ErrorImage);
//                ugld.ApplyDefaults((Infragistics.Win.UltraWinGrid.UltraGrid)sender, e, true, 2, false);
//                //End TT#169
//                DefaultGridLayout();
//            }

//            // Begin TT#11 - JSmith - The view of the calendar needs to pay attention to the culture info when displaying the dates.
//            this.ultraGrid1.DisplayLayout.Bands[0].Columns["SELLING_OPEN_DATE"].MaskInput = null;
//            this.ultraGrid1.DisplayLayout.Bands[0].Columns["SELLING_CLOSE_DATE"].MaskInput = null;
//            this.ultraGrid1.DisplayLayout.Bands[0].Columns["STOCK_OPEN_DATE"].MaskInput = null;
//            this.ultraGrid1.DisplayLayout.Bands[0].Columns["SELLING_CLOSE_DATE"].MaskInput = null;
//            this.ultraGrid1.DisplayLayout.Bands[0].Columns["SELLING_OPEN_DATE"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Date;
//            this.ultraGrid1.DisplayLayout.Bands[0].Columns["SELLING_CLOSE_DATE"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Date;
//            this.ultraGrid1.DisplayLayout.Bands[0].Columns["STOCK_OPEN_DATE"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Date;
//            this.ultraGrid1.DisplayLayout.Bands[0].Columns["SELLING_CLOSE_DATE"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Date;
//            // End TT#11 - JSmith - The view of the calendar needs to pay attention to the culture info when displaying the dates.
//        }

//        private void DefaultGridLayout()
//        {
//            this.ultraGrid1.DisplayLayout.AddNewBox.Hidden = false;
//            this.ultraGrid1.DisplayLayout.GroupByBox.Hidden = false;
//            this.ultraGrid1.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
					
//            ultraGrid1.DisplayLayout.Override.HeaderClickAction = HeaderClickAction.SortSingle;

//            ultraGrid1.DisplayLayout.Bands[0].Columns[0].Hidden = true;
//            ultraGrid1.DisplayLayout.Bands[0].Columns["ACTIVE_IND"].Hidden = true;
//            ultraGrid1.DisplayLayout.Bands[0].Columns["STORE_DELETE_IND"].Hidden = true;	// TT#739-MD - STodd - delete stores
//            ultraGrid1.DisplayLayout.Bands[0].Columns["SHIP_ON_MONDAY"].Hidden = true;
//            ultraGrid1.DisplayLayout.Bands[0].Columns["SHIP_ON_TUESDAY"].Hidden = true;
//            ultraGrid1.DisplayLayout.Bands[0].Columns["SHIP_ON_WEDNESDAY"].Hidden = true;
//            ultraGrid1.DisplayLayout.Bands[0].Columns["SHIP_ON_THURSDAY"].Hidden = true;
//            ultraGrid1.DisplayLayout.Bands[0].Columns["SHIP_ON_FRIDAY"].Hidden = true;
//            ultraGrid1.DisplayLayout.Bands[0].Columns["SHIP_ON_SATURDAY"].Hidden = true;
//            ultraGrid1.DisplayLayout.Bands[0].Columns["SHIP_ON_SUNDAY"].Hidden = true;
//            ultraGrid1.DisplayLayout.Bands[0].Columns["SIMILAR_STORE_MODEL"].Hidden = true;  // issue 3557 stodd

//            // Begin TT#11 - JSmith - The view of the calendar needs to pay attention to the culture info when displaying the dates.
//            //this.ultraGrid1.DisplayLayout.Bands[0].Columns["SELLING_OPEN_DATE"].MaskInput = "mm/dd/yyyy";
//            //this.ultraGrid1.DisplayLayout.Bands[0].Columns["SELLING_CLOSE_DATE"].MaskInput = "mm/dd/yyyy";
//            //this.ultraGrid1.DisplayLayout.Bands[0].Columns["STOCK_OPEN_DATE"].MaskInput = "mm/dd/yyyy";
//            //this.ultraGrid1.DisplayLayout.Bands[0].Columns["SELLING_CLOSE_DATE"].MaskInput = "mm/dd/yyyy";
//            // End TT#11 - JSmith - The view of the calendar needs to pay attention to the culture info when displaying the dates.

//            ApplyCharacteristicHeaderText();
//            ApplySoftHeaderText();

//            this.ultraGrid1.DisplayLayout.Bands[0].Columns["ACTIVE_IND_B"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox;
//            this.ultraGrid1.DisplayLayout.Bands[0].Columns["ACTIVE_IND_B"].Width = 50;
//            this.ultraGrid1.DisplayLayout.Bands[0].Columns["ACTIVE_IND_B"].Header.VisiblePosition = 3;

//            this.ultraGrid1.DisplayLayout.Bands[0].Columns["STORE_DELETE_IND_B"].CellActivation = Activation.NoEdit;
//            this.ultraGrid1.DisplayLayout.Bands[0].Columns["STORE_DELETE_IND_B"].Header.VisiblePosition = 4;
//            this.ultraGrid1.DisplayLayout.Bands[0].Columns["STORE_DELETE_IND_B"].Width = 110;
//            this.ultraGrid1.DisplayLayout.Bands[0].Columns["STORE_DELETE_IND_B"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox;

//            this.ultraGrid1.DisplayLayout.Bands[0].Columns["SIMILAR_STORE_MODEL_B"].Header.VisiblePosition = 5;
//            this.ultraGrid1.DisplayLayout.Bands[0].Columns["SIMILAR_STORE_MODEL_B"].Width = 110;
//            this.ultraGrid1.DisplayLayout.Bands[0].Columns["SIMILAR_STORE_MODEL_B"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox;

//            int shipOnPos = 15;
//            this.ultraGrid1.DisplayLayout.Bands[0].Columns["SHIP_ON_MONDAY_B"].Header.VisiblePosition = shipOnPos++;
//            this.ultraGrid1.DisplayLayout.Bands[0].Columns["SHIP_ON_MONDAY_B"].Width = 75;
//            this.ultraGrid1.DisplayLayout.Bands[0].Columns["SHIP_ON_MONDAY_B"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox;
//            this.ultraGrid1.DisplayLayout.Bands[0].Columns["SHIP_ON_TUESDAY_B"].Header.VisiblePosition = shipOnPos++;
//            this.ultraGrid1.DisplayLayout.Bands[0].Columns["SHIP_ON_TUESDAY_B"].Width = 75;
//            this.ultraGrid1.DisplayLayout.Bands[0].Columns["SHIP_ON_TUESDAY_B"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox;
//            this.ultraGrid1.DisplayLayout.Bands[0].Columns["SHIP_ON_WEDNESDAY_B"].Header.VisiblePosition = shipOnPos++;
//            this.ultraGrid1.DisplayLayout.Bands[0].Columns["SHIP_ON_WEDNESDAY_B"].Width = 75;
//            this.ultraGrid1.DisplayLayout.Bands[0].Columns["SHIP_ON_WEDNESDAY_B"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox;
//            this.ultraGrid1.DisplayLayout.Bands[0].Columns["SHIP_ON_THURSDAY_B"].Header.VisiblePosition = shipOnPos++;
//            this.ultraGrid1.DisplayLayout.Bands[0].Columns["SHIP_ON_THURSDAY_B"].Width = 75;
//            this.ultraGrid1.DisplayLayout.Bands[0].Columns["SHIP_ON_THURSDAY_B"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox;
//            this.ultraGrid1.DisplayLayout.Bands[0].Columns["SHIP_ON_FRIDAY_B"].Header.VisiblePosition = shipOnPos++;
//            this.ultraGrid1.DisplayLayout.Bands[0].Columns["SHIP_ON_FRIDAY_B"].Width = 75;
//            this.ultraGrid1.DisplayLayout.Bands[0].Columns["SHIP_ON_FRIDAY_B"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox;
//            this.ultraGrid1.DisplayLayout.Bands[0].Columns["SHIP_ON_SATURDAY_B"].Header.VisiblePosition = shipOnPos++;
//            this.ultraGrid1.DisplayLayout.Bands[0].Columns["SHIP_ON_SATURDAY_B"].Width = 75;
//            this.ultraGrid1.DisplayLayout.Bands[0].Columns["SHIP_ON_SATURDAY_B"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox;
//            this.ultraGrid1.DisplayLayout.Bands[0].Columns["SHIP_ON_SUNDAY_B"].Header.VisiblePosition = shipOnPos++;
//            this.ultraGrid1.DisplayLayout.Bands[0].Columns["SHIP_ON_SUNDAY_B"].Width = 75;
//            this.ultraGrid1.DisplayLayout.Bands[0].Columns["SHIP_ON_SUNDAY_B"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox;
//            this.ultraGrid1.DisplayLayout.Bands[0].Columns["STORE_NAME"].Width = 150;
//            this.ultraGrid1.DisplayLayout.Bands[0].Columns["STORE_DESC"].Width = 150;

//            this.ultraGrid1.DisplayLayout.Bands[0].Columns["LEAD_TIME"].Width = 75;
//            this.ultraGrid1.DisplayLayout.Bands[0].Columns["STATE"].Width = 50;
//            this.ultraGrid1.DisplayLayout.Bands[0].AddButtonCaption = "Store Profile";

//            this.ultraGrid1.DisplayLayout.MaxColScrollRegions = 2;
//            int colScrollWidth = this.ultraGrid1.DisplayLayout.Bands[0].Columns["STORE_NAME"].Width;
//            colScrollWidth += this.ultraGrid1.DisplayLayout.Bands[0].Columns["ST_ID"].Width;
//            this.ultraGrid1.DisplayLayout.ColScrollRegions[0].Width = colScrollWidth;
//            this.ultraGrid1.DisplayLayout.ColScrollRegions[0].Split (this.ultraGrid1.DisplayLayout.ColScrollRegions[0].Width);
//            this.ultraGrid1.DisplayLayout.Bands[0].Columns["ST_ID"].Header.ExclusiveColScrollRegion = this.ultraGrid1.DisplayLayout.ColScrollRegions[0];
//            this.ultraGrid1.DisplayLayout.Bands[0].Columns["STORE_NAME"].Header.ExclusiveColScrollRegion = this.ultraGrid1.DisplayLayout.ColScrollRegions[0];

//            // Begin TT#3139 - JSmith - Store Profiles – read-only access
//            SetColumnAlignment();
//            //foreach (UltraGridColumn cColumn in this.ultraGrid1.DisplayLayout.Bands[0].Columns)
//            //{
//            //    DataCommon.eStoreCharType cDataType;
//            //    cDataType = _storeSession.GetCharactersticDataType(cColumn.Header.Caption.ToString(CultureInfo.CurrentUICulture));
//            //    if (cDataType == DataCommon.eStoreCharType.dollar)
//            //    {
//            //        cColumn.CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right;
//            //        cColumn.Format = "$#,###,###,###.00";
//            //    }
//            //    else
//            //    {
//            //        cColumn.CellAppearance.TextVAlign = Infragistics.Win.VAlign.Middle;
//            //        cColumn.CellAppearance.TextHAlign = Infragistics.Win.HAlign.Center;
//            //    }

//            //    //===========================================
//            //    // Hide store char group value 'key' columns
//            //    //===========================================
//            //    if (cColumn.Header.Caption.StartsWith("SCGRID__"))
//            //        cColumn.Hidden = true;
//            //}
//            // End TT#3139 - JSmith - Store Profiles – read-only access

//            StoreProfileMaint_LoadDropDowns();
//        }

//        // Begin TT#3139 - JSmith - Store Profiles – read-only access
//        private void SetColumnAlignment()
//        {
//            foreach (UltraGridColumn cColumn in this.ultraGrid1.DisplayLayout.Bands[0].Columns)
//            {
//                DataCommon.eStoreCharType cDataType;
//                cDataType = _storeSession.GetCharactersticDataType(cColumn.Header.Caption.ToString(CultureInfo.CurrentUICulture));
//                if (cDataType == DataCommon.eStoreCharType.dollar)
//                {
//                    cColumn.CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right;
//                    cColumn.Format = "$#,###,###,###.00";
//                }
//                else
//                {
//                    cColumn.CellAppearance.TextVAlign = Infragistics.Win.VAlign.Middle;
//                    cColumn.CellAppearance.TextHAlign = Infragistics.Win.HAlign.Center;
//                }

//                //===========================================
//                // Hide store char group value 'key' columns
//                //===========================================
//                if (cColumn.Header.Caption.StartsWith("SCGRID__"))
//                    cColumn.Hidden = true;
//            }
//        }
//        // End TT#3139 - JSmith - Store Profiles – read-only access

//        private void ApplySoftHeaderText()
//        {
//            _dtHeaderText = MIDText.GetLabels((int)DataCommon.eStoreTableColumns.ST_ID, (int)DataCommon.eStoreTableColumns.STORE_DELETE_IND);
//            // BEGIN TT#1401 - stodd - add resevation stores (IMO)
//            DataTable dtAddlText = MIDText.GetLabels((int)DataCommon.eStoreTableColumns.IMO_ID, (int)DataCommon.eStoreTableColumns.IMO_ID);
//            foreach (DataRow row in dtAddlText.Rows)
//            {
//                DataRow nRow = _dtHeaderText.NewRow();
//                nRow["TEXT_CODE"] = row["TEXT_CODE"];
//                nRow["TEXT_VALUE"] = row["TEXT_VALUE"];
//                _dtHeaderText.Rows.Add(nRow);
//            }
//            // END TT#1401 - stodd - add resevation stores (IMO)


//            foreach(DataRow dr in _dtHeaderText.Rows)
//            {
//                eStoreTableColumns currCol = (eStoreTableColumns)Convert.ToInt32(dr["TEXT_CODE"], CultureInfo.CurrentUICulture);

//                switch(currCol)
//                {
//                    case DataCommon.eStoreTableColumns.ST_ID:
//                        ultraGrid1.DisplayLayout.Bands[0].Columns["ST_ID"].Header.Caption = (string)dr["TEXT_VALUE"];
//                        break;
//                    case DataCommon.eStoreTableColumns.STORE_NAME:
//                        ultraGrid1.DisplayLayout.Bands[0].Columns["STORE_NAME"].Header.Caption = (string)dr["TEXT_VALUE"];
//                        break;
//                    case DataCommon.eStoreTableColumns.STORE_DESC:
//                        ultraGrid1.DisplayLayout.Bands[0].Columns["STORE_DESC"].Header.Caption = (string)dr["TEXT_VALUE"];
//                        break;
//                    case DataCommon.eStoreTableColumns.ACTIVE_IND:
//                        ultraGrid1.DisplayLayout.Bands[0].Columns["ACTIVE_IND_B"].Header.Caption = (string)dr["TEXT_VALUE"];
//                        break;
//                    case DataCommon.eStoreTableColumns.CITY:
//                        ultraGrid1.DisplayLayout.Bands[0].Columns["CITY"].Header.Caption = (string)dr["TEXT_VALUE"];
//                        break;
//                    case DataCommon.eStoreTableColumns.STATE:
//                        ultraGrid1.DisplayLayout.Bands[0].Columns["STATE"].Header.Caption = (string)dr["TEXT_VALUE"];
//                        break;
//                    case DataCommon.eStoreTableColumns.SELLING_SQ_FT:
//                        ultraGrid1.DisplayLayout.Bands[0].Columns["SELLING_SQ_FT"].Header.Caption = (string)dr["TEXT_VALUE"];
//                        break;
//                    case DataCommon.eStoreTableColumns.SELLING_OPEN_DATE:
//                        ultraGrid1.DisplayLayout.Bands[0].Columns["SELLING_OPEN_DATE"].Header.Caption = (string)dr["TEXT_VALUE"];
//                        break;
//                    case DataCommon.eStoreTableColumns.SELLING_CLOSE_DATE:
//                        ultraGrid1.DisplayLayout.Bands[0].Columns["SELLING_CLOSE_DATE"].Header.Caption = (string)dr["TEXT_VALUE"];
//                        break;
//                    case DataCommon.eStoreTableColumns.STOCK_OPEN_DATE:
//                        ultraGrid1.DisplayLayout.Bands[0].Columns["STOCK_OPEN_DATE"].Header.Caption = (string)dr["TEXT_VALUE"];
//                        break;
//                    case DataCommon.eStoreTableColumns.STOCK_CLOSE_DATE:
//                        ultraGrid1.DisplayLayout.Bands[0].Columns["STOCK_CLOSE_DATE"].Header.Caption = (string)dr["TEXT_VALUE"];
//                        break;
//                    case DataCommon.eStoreTableColumns.LEAD_TIME:
//                        ultraGrid1.DisplayLayout.Bands[0].Columns["LEAD_TIME"].Header.Caption = (string)dr["TEXT_VALUE"];
//                        break;
//                    case DataCommon.eStoreTableColumns.SHIP_ON_MONDAY:
//                        ultraGrid1.DisplayLayout.Bands[0].Columns["SHIP_ON_MONDAY_B"].Header.Caption = (string)dr["TEXT_VALUE"];
//                        break;
//                    case DataCommon.eStoreTableColumns.SHIP_ON_TUESDAY:
//                        ultraGrid1.DisplayLayout.Bands[0].Columns["SHIP_ON_TUESDAY_B"].Header.Caption = (string)dr["TEXT_VALUE"];
//                        break;
//                    case DataCommon.eStoreTableColumns.SHIP_ON_WEDNESDAY:
//                        ultraGrid1.DisplayLayout.Bands[0].Columns["SHIP_ON_WEDNESDAY_B"].Header.Caption = (string)dr["TEXT_VALUE"];
//                        break;
//                    case DataCommon.eStoreTableColumns.SHIP_ON_THURSDAY:
//                        ultraGrid1.DisplayLayout.Bands[0].Columns["SHIP_ON_THURSDAY_B"].Header.Caption = (string)dr["TEXT_VALUE"];
//                        break;
//                    case DataCommon.eStoreTableColumns.SHIP_ON_FRIDAY:
//                        ultraGrid1.DisplayLayout.Bands[0].Columns["SHIP_ON_FRIDAY_B"].Header.Caption = (string)dr["TEXT_VALUE"];
//                        break;
//                    case DataCommon.eStoreTableColumns.SHIP_ON_SATURDAY:
//                        ultraGrid1.DisplayLayout.Bands[0].Columns["SHIP_ON_SATURDAY_B"].Header.Caption = (string)dr["TEXT_VALUE"];
//                        break;
//                    case DataCommon.eStoreTableColumns.SHIP_ON_SUNDAY:
//                        ultraGrid1.DisplayLayout.Bands[0].Columns["SHIP_ON_SUNDAY_B"].Header.Caption = (string)dr["TEXT_VALUE"];
//                        break;
//                    // BEGIN TT#1401 - stodd - add resevation stores (IMO)
//                    case DataCommon.eStoreTableColumns.IMO_ID:
//                        ultraGrid1.DisplayLayout.Bands[0].Columns["IMO_ID"].Header.Caption = (string)dr["TEXT_VALUE"];
//                        break;
//                    // END TT#1401 - stodd - add resevation stores (IMO)
//                    // BEGIN TT#739-MD - STodd - delete stores
//                    // similar store model was missing from the code, so I added it as well
//                    case DataCommon.eStoreTableColumns.SIMILAR_STORE_MODEL:
//                        ultraGrid1.DisplayLayout.Bands[0].Columns["SIMILAR_STORE_MODEL_B"].Header.Caption = (string)dr["TEXT_VALUE"];
//                        break;
//                    case DataCommon.eStoreTableColumns.STORE_DELETE_IND:
//                        ultraGrid1.DisplayLayout.Bands[0].Columns["STORE_DELETE_IND_B"].Header.Caption = (string)dr["TEXT_VALUE"];
//                        break;
//                    // END TT#739-MD - STodd - delete stores
//                }
//            }			
//        }



//        /// <summary>
//        /// Applies the column captions from the datatable and apply them to the captions
//        /// of the UltraGrid. 
//        /// </summary>
//        private void ApplyCharacteristicHeaderText()
//        {
//            foreach(DataColumn aColumn in _dtStoreProfile.Columns)
//            {
//                ultraGrid1.DisplayLayout.Bands[0].Columns[aColumn.ColumnName].Header.Caption = aColumn.Caption;
//            }
//        }


//        private void StoreProfile_StartAtStore()
//        {


//            foreach(UltraGridRow row in ultraGrid1.Rows)
//            {
//                LocateStartingStore(row);
//            }

//            //			UltraGridRow gridRow;
//            //			for(int intRow = 0;intRow < ultraGrid1.Rows.Count;intRow++)
//            //			{
//            //				gridRow = ultraGrid1.Rows[intRow];
//            //				if (gridRow.Band.Index == 0) 
//            //				{
//            //					if ( (int)gridRow.Cells["ST_RID"].Value == _startAtStore)
//            //					{
//            //						ultraGrid1.ActiveRow = gridRow;
//            //						gridRow.Expanded = true;
//            //						break;							
//            //					}
//            //				}										 
//            //
//            //			}							
//        }

//        public void ShowStore(int startingStore)
//        {

//            _startAtStore = startingStore;

//            foreach(UltraGridRow row in ultraGrid1.Rows)
//            {
//                LocateStartingStore(row);
//            }						
//        }

//        public void StoreProfile_InsertRow()
//        {
//            ultraGrid1.DisplayLayout.Bands[0].AddNew();					
//        }

//        private void btnSaveToGroups_Click(object sender, System.EventArgs e)
//        {
//            Cursor.Current = Cursors.WaitCursor;

//            int groupByCount;
//            UltraGridColumn uCol;
//            string groupName = null;
//            int newGroupRID = 0;
//            bool newGroup = false;
//            //Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
//            DataTable dtFolders;
//            FolderProfile folderProf = null;
//            //End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders

//            ArrayList descriptions = new ArrayList();
//            ArrayList levels = new ArrayList();
//            // we add a dummy entry to the level array so the column level number can be used
//            // as an index into the array
//            RowLevel rl = new RowLevel();
//            rl.SqlName = "0 level";
//            rl.DataType = DataCommon.eStoreCharType.text;
//            levels.Add(rl);
		
//            if (ultraGrid1.DisplayLayout.Bands[0].HasSortedColumns)
//            {
//                groupByCount = ultraGrid1.DisplayLayout.Bands[0].SortedColumns.Count;
//                // loop through the sorted (selected) columns and save some information
//                for (int i=0;i<groupByCount;i++)
//                {
//                    uCol = ultraGrid1.DisplayLayout.Bands[0].SortedColumns[i];
//                    RowLevel rLevel = new RowLevel();
//                    int rid = _SAB.StoreServerSession.GetStoreCharacteristicGroupRID(uCol.Key);
//                    if (rid != Include.NoRID)  //isChar
//                    {
//                        rLevel.SqlPlaceHolderName = "*SCGRID__" + rid.ToString(CultureInfo.CurrentUICulture);
//                        rLevel.ColumnName = uCol.Header.Caption;
//                        rLevel.SqlName = rLevel.SqlPlaceHolderName;
//                    }
//                    else
//                    {
//                        string dbColumn = uCol.Key;
//                        if (BOOL_COLUMNS.Contains(dbColumn))
//                        {
//                            char [] suffix = { '_','B' };
//                            dbColumn = dbColumn.TrimEnd(suffix);
//                        }
//                        rLevel.ColumnName = uCol.Header.Caption;
//                        rLevel.SqlName = "[" + dbColumn + "]";
//                    }
//                    if (uCol.ValueList != null)
//                        rLevel.IsList = true;
//                    else
//                        rLevel.IsList = false;
//                    if ( uCol.DataType == System.Type.GetType("System.String") )
//                        rLevel.DataType = DataCommon.eStoreCharType.text;
//                    else if ( uCol.DataType == System.Type.GetType("System.DateTime") )
//                        rLevel.DataType = DataCommon.eStoreCharType.date;
//                    else if ( uCol.DataType == System.Type.GetType("System.Boolean") )
//                        rLevel.DataType = DataCommon.eStoreCharType.text;
//                    else
//                        rLevel.DataType = DataCommon.eStoreCharType.number;
//                    GetColumnInfo(uCol, ref rLevel);
//                    levels.Add(rLevel);
					
//                    if (groupName == null)
//                        groupName = uCol.Header.Caption;
//                    else
//                        groupName += " : " + uCol.Header.Caption;
//                }

//                int lastGroupLevel = groupByCount;
//                int currLevel = 1;

//                //****************************************************************
//                // recusively moves throw all rows and their child rows
//                // populating two arrays: descriptions and levels.
//                // descriptions holds the level and description for each header.
//                // levels hold the level name and the level datatype.
//                //****************************************************************
//                foreach(UltraGridRow row in ultraGrid1.Rows)
//                {
//                    GetRowDescriptions(row, descriptions, currLevel, lastGroupLevel);
//                }

//                // Begin Track #4872 - JSmith - Global/User Attributes
//                eGlobalUserType globalUserType = eGlobalUserType.Global;
//                int ownerUserRID;
//                //bool nameOk = ValidStoreGroupName(ref groupName);
//                bool nameOk = ValidStoreGroupName(ref groupName, ref globalUserType);
//                if (globalUserType == eGlobalUserType.Global)
//                {
//                    ownerUserRID = Include.GlobalUserRID;
//                }
//                else
//                {
//                    ownerUserRID = SAB.ClientServerSession.UserRID;
//                }
//                // End Track #4872

//                //*************************************************
//                // Add group and group levels to DB and StoreInfo
//                //*************************************************
//                if (nameOk)
//                {
//                    Cursor.Current = Cursors.WaitCursor;
				
//                    newGroup = true;
//                    // Begin Track #4872 - JSmith - Global/User Attributes
//                    //newGroupRID = _storeSession.AddGroup(groupName, true);


//                    int newDynamicFilterRID = -1; //TT#1414-MD -jsobek -Attribute Set Filter -TODO


//                    newGroupRID = _storeSession.AddGroup(groupName, true, ownerUserRID, newDynamicFilterRID);
//                    // End Track #4872
//                    // Add dynamic desc for group
//                    _storeSession.OpenUpdateConnection();
//                    for (int i=1;i<levels.Count;i++)
//                    {
//                        RowLevel level = (RowLevel)levels[i];
//                        _storeSession.AddStoreDynamicGroupDesc(newGroupRID, level.ColumnId, i);
//                    }
//                    _storeSession.CommitData();
//                    _storeSession.CloseUpdateConnection();

//                    //Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
//                    if (ownerUserRID == Include.GlobalUserRID)
//                    {
//                        dtFolders = _dlFolder.Folder_Read(Include.GlobalUserRID, eProfileType.StoreGroupMainGlobalFolder);
//                    }
//                    else
//                    {
//                        dtFolders = _dlFolder.Folder_Read(ownerUserRID, eProfileType.StoreGroupMainUserFolder);

//                        if (dtFolders == null || dtFolders.Rows.Count == 0)
//                        {
//                            dtFolders = _dlFolder.Folder_Read(Include.GlobalUserRID, eProfileType.StoreGroupMainGlobalFolder);
//                        }
//                    }

//                    folderProf = new FolderProfile(dtFolders.Rows[0]);

//                    _dlFolder.OpenUpdateConnection();

//                    try
//                    {
//                        _dlFolder.Folder_Item_Insert(folderProf.Key, newGroupRID, eProfileType.StoreGroup);
//                        _dlFolder.CommitData();
//                    }
//                    catch (Exception exc)
//                    {
//                        string message = exc.ToString();
//                        throw;
//                    }
//                    finally
//                    {
//                        _dlFolder.CloseUpdateConnection();
//                    }

//                    //End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
//                    //Begin TT#724 - JScott - The "Available Stores" attribute set can be renamed in the Store Group Explorer and could cause processing problems
//                    //// Add _availableStoresText group level
//                    //int newGroupLevelRID = _storeSession.AddGroupLevel(newGroupRID, _availableStoresText);
//                    // Add Available Stores group level
//                    int newGroupLevelRID = _storeSession.AddGroupLevel(newGroupRID, Include.AvailableStoresGroupName);  
//                    //End TT#724 - JScott - The "Available Stores" attribute set can be renamed in the Store Group Explorer and could cause processing problems

//                    // Begin Track #3956 stodd
//                    _storeSession.GetStoresInGroup(newGroupRID, newGroupLevelRID);
//                    _storeSession.RefreshStoresInGroup(newGroupRID);

////                    string [] names = new string[100];
////                    string [] SQLs = new string[100];
////                    string colValue = null;
////                    string groupLevelName = null;
////                    // gets just the column stucture for the datatable, but no data.
////                    DataTable dtSGLStatement = _storeSession.GetEmptyStoreGroupLevelStatementDatatable();

////                    foreach(RowDescription rd in descriptions)
////                    {
////                        // init ColValue
////                        colValue = rd.Description;
////                        // we need to change Store Status to it's numeric (enum) equivalent
////                        if (((RowLevel)levels[rd.Level]).ColumnId == (int)eMIDTextCode.lbl_StoreStatus)
////                        {
////                            colValue = (GetStoreStatusCode(rd.Description)).ToString(CultureInfo.CurrentUICulture);
////                        }
////                        bool isList = ((RowLevel)levels[rd.Level]).IsList;
////                        if (rd.Level == 1)
////                        {
////                            names[rd.Level] = rd.Description;
////                            groupLevelName = rd.Description;
//////							if (((RowLevel)levels[rd.Level]).IsCharacteristic)  // want caption instead of column name
//////								colValue = ConvertBooleanColumn(((RowLevel)levels[rd.Level]).Name, colValue);
//////							else
////                                colValue = ConvertBooleanColumn(((RowLevel)levels[rd.Level]).ColumnName, colValue);
////                            if (colValue == _emptyDescription)
////                            {
////                                SQLs[rd.Level] = ((RowLevel)levels[rd.Level]).SqlName + " is ";
////                                SQLs[rd.Level] += ConvertDescToSql(colValue, ((RowLevel)levels[rd.Level]).DataType);
////                            }
////                            else
////                            {
////                                if (isList)
////                                {
////                                    SQLs[rd.Level] = ((RowLevel)levels[rd.Level]).SqlName + " IN (";
////                                    SQLs[rd.Level] += ConvertDescToSql(colValue, ((RowLevel)levels[rd.Level]).DataType);
////                                    SQLs[rd.Level] += ")";
////                                }
////                                else
////                                {
////                                    SQLs[rd.Level] = ((RowLevel)levels[rd.Level]).SqlName + " = ";
////                                    SQLs[rd.Level] += ConvertDescToSql(colValue, ((RowLevel)levels[rd.Level]).DataType);
////                                }
////                            }

////                            if (rd.Level == lastGroupLevel)
////                            {
////                                // Add Group Levels
////                                groupLevelName = ValidStoreGroupLevelName(groupLevelName);
////                                newGroupLevelRID = _storeSession.AddGroupLevel(newGroupRID, groupLevelName);	

////                                // Add Group Dyn Join recs
////                                if (colValue == _emptyDescription)
////                                {
////                                    SQLs[rd.Level] = ((RowLevel)levels[rd.Level]).SqlName + " is ";
////                                    SQLs[rd.Level] += ConvertDescToSql(colValue, ((RowLevel)levels[rd.Level]).DataType);
////                                }
////                                else
////                                {
////                                    if (isList)
////                                    {
////                                        SQLs[rd.Level] = ((RowLevel)levels[rd.Level]).SqlName + " IN (";
////                                        SQLs[rd.Level] += ConvertDescToSql(colValue, ((RowLevel)levels[rd.Level]).DataType);
////                                        SQLs[rd.Level] += ")";
////                                    }
////                                    else
////                                    {
////                                        SQLs[rd.Level] = ((RowLevel)levels[rd.Level]).SqlName + " = ";
////                                        SQLs[rd.Level] += ConvertDescToSql(colValue, ((RowLevel)levels[rd.Level]).DataType);
////                                    }
////                                }
////                                string [] dynJoinSql = new string[1];
////                                dynJoinSql[0] = SQLs[1];
////                                //_storeSession.AddGroupLevelStatement(dynJoinSql, newGroupLevelRID);
////                                BuildSGLStatementRow(newGroupLevelRID, names, levels, dynJoinSql, ref dtSGLStatement);
////                            }
////                        }
////                        else if (rd.Level == lastGroupLevel)
////                        {
////                            groupLevelName = "";
////                            for (int i=1;i<lastGroupLevel;i++)
////                            {
////                                groupLevelName += names[i] + " : ";
////                            }
////                            groupLevelName += rd.Description;
////                            // Add Group Levels
////                            groupLevelName = ValidStoreGroupLevelName(groupLevelName);
////                            newGroupLevelRID = _storeSession.AddGroupLevel(newGroupRID, groupLevelName);	

////                            names[rd.Level] = rd.Description;
////                            // Add Group Dyn Join recs
//////							if (((RowLevel)levels[rd.Level]).IsCharacteristic)  // want caption instead of column name
//////								colValue = ConvertBooleanColumn(((RowLevel)levels[rd.Level]).Name, colValue);
//////							else
////                                colValue = ConvertBooleanColumn(((RowLevel)levels[rd.Level]).ColumnName, colValue);
////                            if (colValue == _emptyDescription)
////                            {
////                                SQLs[rd.Level] = "AND " + ((RowLevel)levels[rd.Level]).SqlName + " is ";
////                                SQLs[rd.Level] += ConvertDescToSql(colValue, ((RowLevel)levels[rd.Level]).DataType);
////                            }
////                            else
////                            {
////                                if (isList)
////                                {
////                                    SQLs[rd.Level] = "AND " +  ((RowLevel)levels[rd.Level]).SqlName + " IN (";
////                                    SQLs[rd.Level] += ConvertDescToSql(colValue, ((RowLevel)levels[rd.Level]).DataType);
////                                    SQLs[rd.Level] += ")";
////                                }
////                                else
////                                {
////                                    SQLs[rd.Level] = "AND " +  ((RowLevel)levels[rd.Level]).SqlName + " = ";
////                                    SQLs[rd.Level] += ConvertDescToSql(colValue, ((RowLevel)levels[rd.Level]).DataType);
////                                }
////                            }
////                            string [] dynJoinSql = new string[lastGroupLevel];
////                            for (int i=1;i<=lastGroupLevel;i++)
////                            {
////                                dynJoinSql[i-1] = SQLs[i];
////                            }
////                            //_storeSession.AddGroupLevelStatement(dynJoinSql, newGroupLevelRID);
////                            BuildSGLStatementRow(newGroupLevelRID, names, levels, dynJoinSql, ref dtSGLStatement);
////                        }
////                        else // all other levels
////                        {
////                            names[rd.Level] = rd.Description;
//////							if (((RowLevel)levels[rd.Level]).IsCharacteristic)  // want caption instead of column name
//////								colValue = ConvertBooleanColumn(((RowLevel)levels[rd.Level]).Name, colValue);
//////							else
////                                colValue = ConvertBooleanColumn(((RowLevel)levels[rd.Level]).ColumnName, colValue);
////                            if (colValue == _emptyDescription)
////                            {
////                                SQLs[rd.Level] = "AND " + ((RowLevel)levels[rd.Level]).SqlName + " is ";
////                                SQLs[rd.Level] += ConvertDescToSql(colValue, ((RowLevel)levels[rd.Level]).DataType);
////                            }
////                            else
////                            {
////                                if (isList)
////                                {
////                                    SQLs[rd.Level] = "AND " +  ((RowLevel)levels[rd.Level]).SqlName + " IN (";
////                                    SQLs[rd.Level] += ConvertDescToSql(colValue, ((RowLevel)levels[rd.Level]).DataType);
////                                    SQLs[rd.Level] += ")";
////                                }
////                                else
////                                {
////                                    SQLs[rd.Level] = "AND " +  ((RowLevel)levels[rd.Level]).SqlName + " = ";
////                                    SQLs[rd.Level] += ConvertDescToSql(colValue, ((RowLevel)levels[rd.Level]).DataType);
////                                }
////                            }
////                        }
////                    }
////                    _storeSession.AddGroupLevelStatement(dtSGLStatement);
//                    // End Track #3956 stodd
//                }
//            }
//            else
//            {
//                MessageBox.Show("No store groups found.  First group the stores" +
//                    " by the desired characteristics, then try again.",
//                    "Store Group Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

//                groupName = null;
//            }

//            //***************************************************
//            // Throws event to add new group to Store Explorer
//            //***************************************************
//            if (newGroup)
//            {
//                StoreGroupProfile sg = _storeSession.GetStoreGroup(newGroupRID);
//                //Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
//                //StoreGroupChangeEventArgs ea = new StoreGroupChangeEventArgs(sg);
//                //if (OnStoreGroupPropertyChangeHandler != null)
//                //{
//                //    OnStoreGroupPropertyChangeHandler(this, ea);
//                //}
//                //else
//                //{
//                //    _EAB.StoreGroupExplorer.OnStoreGroupPropertiesChange(sg);

//                //}



//                //_EAB.StoreGroupExplorer.AddStoreGroup(sg, folderProf.Key);  //TODO -Attribute Set Filter


//                //End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
//            }

//            Cursor.Current = Cursors.Default;
//        }

//        private int GetStoreStatusCode(string storeStatusValue)
//        {
//            int textCode = Include.Undefined;

//            DataRow [] rows = _dtStoreStatus.Select("TEXT_VALUE = '" + storeStatusValue + "'");
//            if (rows.Length > 0)
//            {
//                DataRow row = rows[0];
//                textCode = Convert.ToInt32(row["TEXT_CODE"], CultureInfo.CurrentUICulture);
//            }

//            return textCode;
//        }


//        // BEGIN Track #3956 stodd
//        //private void BuildSGLStatementRow(int sgl_rid, string [] values, ArrayList levels, string [] dynJoinSql, ref DataTable dtSGLStatement)
//        //{
	
//        //    for(int i=0; i<dynJoinSql.Length; i++)
//        //    {
//        //        DataRow dr = dtSGLStatement.NewRow();
//        //        dr["SGL_RID"] = sgl_rid;
//        //        dr["SGLS_STATEMENT_SEQ"] = i + 1;
//        //        dr["SGLS_CHAR_IND"] = Include.ConvertBoolToChar( ((RowLevel)levels[i + 1]).IsCharacteristic );
//        //        string engSql = dynJoinSql[i];
//        //        if (engSql.IndexOf("*SCGRID__") != -1)
//        //        {
//        //            string placeHolder = "*SCGRID__" + ((RowLevel)levels[i + 1]).ColumnId.ToString(CultureInfo.CurrentUICulture);
//        //            engSql = engSql.Replace(placeHolder,((RowLevel)levels[i + 1]).ColumnName);
//        //        }
//        //        if ( ((RowLevel)levels[i + 1]).IsCharacteristic )
//        //        {
//        //            if (values[i + 1] == _emptyDescription) // a Null Characteristic
//        //            {
//        //                // Begin track #3956
//        //                string characteristicSql = dynJoinSql[i];
//        //                if (characteristicSql.Contains("is Null")) ;
//        //                {
//        //                    characteristicSql = characteristicSql.Replace("is Null", "= 0");
//        //                    characteristicSql = characteristicSql.Replace("*SCGRID__", "SCGRID__");
//        //                }
//        //                //dr["SGLS_STATEMENT"] = dynJoinSql[i];
//        //                dr["SGLS_STATEMENT"] = characteristicSql;
//        //                // End track #3956
//        //            }
//        //            else
//        //            {
//        //                int sc_rid = _storeSession.StoreCharExists(((RowLevel)levels[i + 1]).ColumnName, values[i + 1]);
//        //                if (sc_rid > 0) // found it
//        //                {
//        //                    if (i>0)
//        //                        dr["SGLS_STATEMENT"] = "AND ";
//        //                    dr["SGLS_STATEMENT"] += "SCGRID__" + ((RowLevel)levels[i + 1]).ColumnId.ToString(CultureInfo.CurrentUICulture) +
//        //                        " = " + sc_rid.ToString(CultureInfo.CurrentUICulture);
//        //                    dr["SGLS_VALUE"] = sc_rid.ToString(CultureInfo.CurrentUICulture);
//        //                }
//        //                else
//        //                {
//        //                    throw new MIDException(eErrorLevel.severe,0,"Could not find value for: " + dynJoinSql[i] );
//        //                }
//        //            }
//        //        }
//        //        else if (((RowLevel)levels[i + 1]).ColumnName == "Store Status")
//        //        {
//        //            // right now the dynJoinSql[i] looks something like 'Store Status = 804002'.
//        //            // we need to change the engSql to look like 'Store Status = NonComp'.
//        //            dr["SGLS_STATEMENT"] = dynJoinSql[i];
//        //            int storeStatusCode  = this.GetStoreStatusCode(values[i + 1]);
//        //            dr["SGLS_VALUE"] = storeStatusCode;
//        //            engSql = engSql.Replace("Store Status",_storeStatusText);
//        //            engSql = engSql.Replace(storeStatusCode.ToString(CultureInfo.CurrentUICulture), values[i + 1]);
//        //        }
//        //        else 
//        //        {
//        //            dr["SGLS_STATEMENT"] = dynJoinSql[i];
//        //            dr["SGLS_VALUE"] = values[i + 1];
//        //        }
//        //        dr["SGLS_CHAR_ID"] = ((RowLevel)levels[i + 1]).ColumnId;
//        //        engSql = engSql.Replace("=",MIDText.GetTextOnly(eMIDTextCode.lbl_Equals));
//        //        engSql = engSql.Replace("'0'","False"); // convert '0' to english FALSE
//        //        engSql = engSql.Replace("'1'","True");	// convert '0' to english FALSE
//        //        engSql = engSql.Replace("'","");
//        //        dr["SGLS_ENG_SQL"] = engSql;
//        //        if (((RowLevel)levels[i + 1]).IsList)
//        //        {
//        //            dr["SGLS_DT"] = eStoreCharType.list;
//        //            dr["SGLS_SQL_OPERATOR"] = DataCommon.eMIDTextCode.lbl_In;
//        //        }
//        //        else
//        //        {
//        //            dr["SGLS_DT"] = ((RowLevel)levels[i + 1]).DataType;
//        //            dr["SGLS_SQL_OPERATOR"] = DataCommon.eMIDTextCode.lbl_Equals;
//        //        }
//        //        dr["SGLS_PREFIX"] = "";
//        //        dr["SGLS_SUFFIX"] = "";
//        //        //((RowLevel)levels[rd.Level]).ColumnName + " = ";
//        //        dtSGLStatement.Rows.Add(dr);
//        //    }
//        //}
//        // End Track #3956 stodd

//        /// <summary>
//        /// this sets the RowLevel.IsCharacteristic & RowLevel.ColumnId
//        /// </summary>
//        /// <param name="rLevel"></param>
//        private void GetColumnInfo(UltraGridColumn col, ref RowLevel rLevel)
//        {

//            switch(col.Key)
//            {
//                case "ST_ID":
//                    rLevel.IsCharacteristic = false;
//                    rLevel.ColumnId = Convert.ToInt32(eMIDTextCode.lbl_ST_ID, CultureInfo.CurrentUICulture);
//                    break;
//                case "STORE_NAME":
//                    rLevel.IsCharacteristic = false;
//                    rLevel.ColumnId = Convert.ToInt32(eMIDTextCode.lbl_STORE_NAME, CultureInfo.CurrentUICulture);
//                    break;
//                case "STORE_DESC":
//                    rLevel.IsCharacteristic = false;
//                    rLevel.ColumnId = Convert.ToInt32(eMIDTextCode.lbl_STORE_DESC, CultureInfo.CurrentUICulture);
//                    break;
//                case "ACTIVE_IND_B":
//                    rLevel.IsCharacteristic = false;
//                    rLevel.ColumnId = Convert.ToInt32(eMIDTextCode.lbl_ACTIVE_IND, CultureInfo.CurrentUICulture);
//                    rLevel.ColumnName = "ACTIVE_IND";  // correct the name...
//                    break;
//                case "CITY":
//                    rLevel.IsCharacteristic = false;
//                    rLevel.ColumnId = Convert.ToInt32(eMIDTextCode.lbl_CITY, CultureInfo.CurrentUICulture);
//                    break;
//                case "STATE":
//                    rLevel.IsCharacteristic = false;
//                    rLevel.ColumnId = Convert.ToInt32(eMIDTextCode.lbl_STATE, CultureInfo.CurrentUICulture);
//                    break;
//                case "SELLING_SQ_FT":
//                    rLevel.IsCharacteristic = false;
//                    rLevel.ColumnId = Convert.ToInt32(eMIDTextCode.lbl_SELLING_SQ_FT, CultureInfo.CurrentUICulture);
//                    break;
//                case "SELLING_OPEN_DATE":
//                    rLevel.IsCharacteristic = false;
//                    rLevel.ColumnId = Convert.ToInt32(eMIDTextCode.lbl_SELLING_OPEN_DATE, CultureInfo.CurrentUICulture);
//                    break;
//                case "SELLING_CLOSE_DATE":
//                    rLevel.IsCharacteristic = false;
//                    rLevel.ColumnId = Convert.ToInt32(eMIDTextCode.lbl_SELLING_CLOSE_DATE, CultureInfo.CurrentUICulture);
//                    break;
//                case "STOCK_OPEN_DATE":
//                    rLevel.IsCharacteristic = false;
//                    rLevel.ColumnId = Convert.ToInt32(eMIDTextCode.lbl_STOCK_OPEN_DATE, CultureInfo.CurrentUICulture);
//                    break;
//                case "STOCK_CLOSE_DATE":
//                    rLevel.IsCharacteristic = false;
//                    rLevel.ColumnId = Convert.ToInt32(eMIDTextCode.lbl_STOCK_CLOSE_DATE, CultureInfo.CurrentUICulture);
//                    break;
//                case "LEAD_TIME":
//                    rLevel.IsCharacteristic = false;
//                    rLevel.ColumnId = Convert.ToInt32(eMIDTextCode.lbl_LEAD_TIME, CultureInfo.CurrentUICulture);
//                    break;
//                case "SHIP_ON_MONDAY_B":
//                    rLevel.IsCharacteristic = false;
//                    rLevel.ColumnId = Convert.ToInt32(eMIDTextCode.lbl_SHIP_ON_MONDAY, CultureInfo.CurrentUICulture);
//                    rLevel.ColumnName = "SHIP_ON_MONDAY";  // correct the name...
//                    break;
//                case "SHIP_ON_TUESDAY_B":
//                    rLevel.IsCharacteristic = false;
//                    rLevel.ColumnId = Convert.ToInt32(eMIDTextCode.lbl_SHIP_ON_TUESDAY, CultureInfo.CurrentUICulture);
//                    rLevel.ColumnName = "SHIP_ON_TUESDAY";  // correct the name...
//                    break;
//                case "SHIP_ON_WEDNESDAY_B":
//                    rLevel.IsCharacteristic = false;
//                    rLevel.ColumnId = Convert.ToInt32(eMIDTextCode.lbl_SHIP_ON_WEDNESDAY, CultureInfo.CurrentUICulture);
//                    rLevel.ColumnName = "SHIP_ON_WEDNESDAY";  // correct the name...
//                    break;
//                case "SHIP_ON_THURSDAY_B":
//                    rLevel.IsCharacteristic = false;
//                    rLevel.ColumnId = Convert.ToInt32(eMIDTextCode.lbl_SHIP_ON_THURSDAY, CultureInfo.CurrentUICulture);
//                    rLevel.ColumnName = "SHIP_ON_THURSDAY";  // correct the name...
//                    break;
//                case "SHIP_ON_FRIDAY_B":
//                    rLevel.IsCharacteristic = false;
//                    rLevel.ColumnId = Convert.ToInt32(eMIDTextCode.lbl_SHIP_ON_FRIDAY, CultureInfo.CurrentUICulture);
//                    rLevel.ColumnName = "SHIP_ON_FRIDAY";  // correct the name...
//                    break;
//                case "SHIP_ON_SATURDAY_B":
//                    rLevel.IsCharacteristic = false;
//                    rLevel.ColumnId = Convert.ToInt32(eMIDTextCode.lbl_SHIP_ON_SATURDAY, CultureInfo.CurrentUICulture);
//                    rLevel.ColumnName = "SHIP_ON_SATURDAY";  // correct the name...
//                    break;
//                case "SHIP_ON_SUNDAY_B":
//                    rLevel.IsCharacteristic = false;
//                    rLevel.ColumnId = Convert.ToInt32(eMIDTextCode.lbl_SHIP_ON_SUNDAY, CultureInfo.CurrentUICulture);
//                    rLevel.ColumnName = "SHIP_ON_SUNDAY";  // correct the name...
//                    break;
//                case "SIMILAR_STORE_MODEL_B":
//                    rLevel.IsCharacteristic = false;
//                    rLevel.ColumnId = Convert.ToInt32(eMIDTextCode.lbl_SimilarStoreModel, CultureInfo.CurrentUICulture);
//                    rLevel.ColumnName = "SIMILAR_STORE_MODEL";  // correct the name...
//                    break;
//                // BEGIN TT#1401 - stodd - add resevation stores (IMO)
//                case "IMO_ID":
//                    rLevel.IsCharacteristic = false;
//                    rLevel.ColumnId = Convert.ToInt32(eMIDTextCode.lbl_IMO_ID, CultureInfo.CurrentUICulture);
//                    break;
//                // END TT#1401 - stodd - add resevation stores (IMO)
//                // BEGIN TT#739-MD - STodd - delete stores
//                case "STORE_DELETE_IND":
//                    rLevel.IsCharacteristic = false;
//                    rLevel.ColumnId = Convert.ToInt32(eMIDTextCode.lbl_StoreDeleteInd);
//                    break;
//                // END TT#739-MD - STodd - delete stores
//                default:
//                    if (col.Key == "Store Status")
//                    {
//                        rLevel.IsCharacteristic = false;
//                        rLevel.ColumnId = Convert.ToInt32(eMIDTextCode.lbl_StoreStatus, CultureInfo.CurrentUICulture);
//                    }
//                    else
//                    {
//                        rLevel.IsCharacteristic = true;
//                        rLevel.ColumnId = _SAB.StoreServerSession.StoreCharGroupExists(col.Header.Caption);
//                    }
//                    break;
//            }

//        }

//        // Begin Track #3956 stodd
//        ///// <summary>
//        ///// These columns represent their values as True and False, but we need to
//        ///// be "1" and "0" for the Database SQL
//        ///// </summary>
//        ///// <param name="columnValue"></param>
//        ///// <param name="columnName"></param>
//        ///// <returns></returns>
//        //private string ConvertBooleanColumn(string columnName, string columnValue)
//        //{
//        //    string newColumnValue = columnValue;

//        //    if (columnName == "ACTIVE_IND" 
//        //        || columnName == "SHIP_ON_MONDAY"
//        //        || columnName == "SHIP_ON_TUESDAY"
//        //        || columnName == "SHIP_ON_WEDNESDAY"
//        //        || columnName == "SHIP_ON_THURSDAY"
//        //        || columnName == "SHIP_ON_FRIDAY"
//        //        || columnName == "SHIP_ON_SATURDAY"
//        //        || columnName == "SHIP_ON_SUNDAY"
//        //        || columnName == "SIMILAR_STORE_MODEL")
//        //    {
//        //        if (columnValue == "True")
//        //            newColumnValue = "1";
//        //        else
//        //            newColumnValue = "0";
//        //    }
			
//        //    return newColumnValue;

//        //}
		
//        ///// <summary>
//        ///// Using dataType, constructs a string holding the row level description
//        ///// as it would appear in a SQL statement.
//        ///// </summary>
//        ///// <param name="desc">Description.</param>
//        ///// <param name="dataType">Data Type of Description.</param>
//        //private string ConvertDescToSql(string desc, DataCommon.eStoreCharType dataType)
//        //{
//        //    string sql = "";
//        //    if (desc == _emptyDescription)
//        //        sql = "Null";
//        //    else if (dataType == DataCommon.eStoreCharType.date)
//        //        sql = "'" + desc + "'";
//        //    else if (dataType == DataCommon.eStoreCharType.text)
//        //    {
//        //        // Begin Issue 3949 - stodd
//        //        desc = desc.Replace("'","''");
//        //        // End
//        //        sql = "'" + desc + "'";
//        //    }
//        //    else 
//        //        sql = desc;
//        //    return sql;
//        //}
//        // End track #3956

//        private void GetRowDescriptions(UltraGridRow row, ArrayList descriptions, int currLevel, int lastGroupLevel)
//        {
//            char [] delim = {':'} ;
//            string desc = row.Description;
			
//            if (desc != null)
//            {
//                RowDescription rowDesc = new RowDescription();
//                // trims the description down to what we want
//                // starts out looking like:		State : AL (12 items)
//                // end up with just:			AL
//                string [] d1 = desc.Split(delim,2);
//                int len = d1[1].Length;
//                int start = d1[1].IndexOf("(",0,len);
//                int diff = len - start;
//                rowDesc.Description = d1[1].Remove(start,diff);
//                rowDesc.Description = rowDesc.Description.Trim();
//                if (rowDesc.Description == null || rowDesc.Description == "")
//                    rowDesc.Description = _emptyDescription;
//                rowDesc.Level = currLevel;

//                // this means there is only one 'level' to the group name and it is NULL
//                // so we don't bother adding it.
//                if (lastGroupLevel == 1 && rowDesc.Description == _emptyDescription)
//                {
//                }
//                else
//                {
//                    // for multi level definitions, we make sure that Multiple Nulls can't occur
////					if (rowDesc.Description == _emptyDescription)
////					{
////						foreach (RowDescription rd in descriptions)
////						{
////							if (rd.Level == rowDesc.Level && rd.Description == rowDesc.Description)
////							{
////								rowDesc.Description = rowDesc.Description + collisionCount.ToString();
////								collisionCount++;
////							}
////						}
////					}
//                    // Add description
//                    descriptions.Add(rowDesc);
//                }
//            }

//            if (row.HasChild(null))
//            {
//                currLevel++;
//                foreach (UltraGridChildBand cb in row.ChildBands)
//                {
//                    foreach (UltraGridRow childRow in cb.Rows)
//                    {
//                        GetRowDescriptions(childRow, descriptions, currLevel, lastGroupLevel);
//                    }
//                }
//            }		
//        }

//        private void LocateStartingStore(UltraGridRow row)
//        {
//            if (row.HasChild(null))
//            {
//                foreach (UltraGridChildBand cb in row.ChildBands)
//                {
//                    foreach (UltraGridRow childRow in cb.Rows)
//                    {
//                        LocateStartingStore(childRow);
//                    }
//                }
//            }
//            else
//            {
//                if ( Convert.ToInt32(row.Cells["ST_RID"].Value, CultureInfo.CurrentUICulture) == _startAtStore )
//                {
//                    ultraGrid1.ActiveRow = row;
//                    row.Expanded = true;						
//                }
//            }
//        }

//        private void doRow(UltraGridRow row, StreamWriter sw, string name, int printLevel, int currLevel, ArrayList columns, ArrayList SQLs)
//        {
//            char [] delim = {':'} ;
//            string desc = row.Description;
			
//            if (desc != null)
//            {
//                // trims the descriotion down to what we want
//                // starts out looking like:		State : AL (12 items)
//                // end up with just:			AL
//                string [] d1 = desc.Split(delim,2);
//                int len = d1[1].Length;
//                int start = d1[1].IndexOf("(",0,len);
//                int diff = len - start;
//                string newDesc = d1[1].Remove(start,diff);
//                newDesc = newDesc.Trim();
		
//                if (currLevel == 1)
//                {
//                    string sql = columns[currLevel] + " = " + newDesc;
//                    SQLs.Add(sql);
//                }
//                else
//                {
//                    SQLs.RemoveAt(currLevel);
//                    string sql = "AND " + columns[currLevel] + " = " + newDesc;
//                    SQLs.Add(sql);
//                }

//                name += newDesc;
//            }

//            if (printLevel == currLevel)
//            {
//                for (int i = 1;i<=printLevel;i++)
//                {
//                    string rec = name + "\t" + SQLs[i] + "\n";
//                    sw.Write(rec);
//                }
//                // remove last sql, we'll be replacing it soon.
//                //SQLs.RemoveAt(printLevel);
//            }

//            if (row.HasChild(null))
//            {
//                name += " : ";
//                currLevel++;

//                foreach (UltraGridChildBand cb in row.ChildBands)
//                {
//                    foreach (UltraGridRow r in cb.Rows)
//                    {
//                        doRow(r, sw, name, printLevel, currLevel, columns, SQLs);
//                    }
//                }
//            }		
//        }

//        private void GetNextRow(UltraGridRow aRow)
//        {
//            //retrieve reference to current ActiveRow
//            //UltraGridRow aRow = ultraGrid1.ActiveRow;

//            // look to see if this row has a child row
//            if (aRow.HasChild() == true)
//            {
//                aRow = aRow.GetChild(Infragistics.Win.UltraWinGrid.ChildRow.First);
//                ultraGrid1.ActiveRow = aRow;
//                return;
//            }

//            // look to see if this row has a next sibling
//            if (aRow.HasNextSibling() == true)
//            {
//                aRow = aRow.GetSibling(Infragistics.Win.UltraWinGrid.SiblingRow.Next);
//                ultraGrid1.ActiveRow = aRow;
//                return;
//            }

//            // look for next sibling of parent
//            aRow = aRow.ParentRow;
//            if (aRow.HasNextSibling() == true)
//            {	
//                aRow = aRow.GetSibling(Infragistics.Win.UltraWinGrid.SiblingRow.Next);
//                ultraGrid1.ActiveRow = aRow;
//                return;
//            }


//            // work back up the tree until finding a row with a sibling
//            aRow = aRow.ParentRow;
//            if (aRow == null) return; // last row of band 0 reached
//            while (aRow.HasNextSibling() == false)
//            {
//                aRow = aRow.ParentRow;
//                if (aRow == null) break;
//            }

//            ultraGrid1.ActiveRow = aRow;
//        }


//        private void btnSave_Click(object sender, System.EventArgs e)
//        {
//            // Begin TT#3139 - JSmith - Store Profiles – read-only access
//            if (!_editEnabled)
//            {
//                StoreProfileMaint_LoadForUpdate();
//                return;
//            }
//            // End TT#3139 - JSmith - Store Profiles – read-only access
//            this.ISave();
//        }

//        override protected bool SaveChanges()
//        {
//            try
//            {
//                Cursor.Current = Cursors.WaitCursor;
//                bool isOk = ValidStores();

//                if (_storeActiveChanged)
//                {
//                    if (MessageBox.Show (_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_StoreActiveChanged),  this.Text,
//                        MessageBoxButtons.YesNo, MessageBoxIcon.Question)
//                        == DialogResult.No)
//                    {
//                        return false;
//                    }
//                }

//                //***********************************************************************************************
//                // For some reason when the store session is Remote, the SaveStoreProfileChanges() method
//                // gets an 'Invalid cast'.  Trial and error showed this to be caused by the new alternative
//                // columns added to the grid for display purposes.  To get around the issue, the datatable is
//                // copied and the new columns are removed from the copied Datetable and then sent to session for
//                // processing.
//                //***********************************************************************************************
//                DataTable dtTempStoreProfile = _dtStoreProfile.Copy();
//                DataSet ds1 = MIDEnvironment.CreateDataSet("temp");
//                ds1.Tables.Add(dtTempStoreProfile);

//                dtTempStoreProfile.Columns.Remove("ACTIVE_IND_B");
//                dtTempStoreProfile.Columns.Remove("SHIP_ON_MONDAY_B");
//                dtTempStoreProfile.Columns.Remove("SHIP_ON_TUESDAY_B");
//                dtTempStoreProfile.Columns.Remove("SHIP_ON_WEDNESDAY_B");
//                dtTempStoreProfile.Columns.Remove("SHIP_ON_THURSDAY_B");
//                dtTempStoreProfile.Columns.Remove("SHIP_ON_FRIDAY_B");
//                dtTempStoreProfile.Columns.Remove("SHIP_ON_SATURDAY_B");
//                dtTempStoreProfile.Columns.Remove("SHIP_ON_SUNDAY_B");
//                dtTempStoreProfile.Columns.Remove("SIMILAR_STORE_MODEL_B");
//                dtTempStoreProfile.Columns.Remove("STORE_DELETE_IND_B");  // Begin TT#3267 - JSmith - Tried to add VSW store
			
//                DataTable dtChanges = dtTempStoreProfile.GetChanges();
				

//                if (dtChanges != null)
//                {
//                    if (isOk)
//                    {
//                        DataSet ds2 = MIDEnvironment.CreateDataSet("changes");
//                        ds2.Tables.Add(dtChanges);
//                        DataTable dtUpdtStoreProfile = _storeSession.SaveStoreProfileChanges(dtChanges);

//                        // We need to update the new row keys (ST_RID)
//                        foreach (DataRow aRow in _dtStoreProfile.Rows)
//                        {
//                            if (aRow.RowState == DataRowState.Added)
//                            {
//                                string aStoreId = (string)aRow["ST_ID"];
//                                foreach (DataRow aTempRow in dtUpdtStoreProfile.Rows)
//                                {
//                                    string aTempStoreId = (string)aTempRow["ST_ID"];

//                                    if (aTempStoreId == aStoreId)
//                                    {
//                                        aRow["ST_RID"] = aTempRow["ST_RID"];
//                                        break;
//                                    }
//                                }
//                            }
//                        }
//                        _dsStoreProfile.AcceptChanges();
//                        // Characteristic keys were added to hidden columns
//                        _dsStoreProfile.Merge(dtUpdtStoreProfile,false,MissingSchemaAction.Ignore);
//                        _dsStoreProfile.AcceptChanges();
//                        // we've processed the changes using dtTempStoreProfile, so we
//                        // need to accept those changes on the real datatable.
//                        _insertedRows.Clear();  // clear cache of new rows;
//                    }

//                    ProcessReserveStore();

//                    _storeSession.RefreshStoresInAllGroups();
//                }

//                _storeActiveChanged = false;
//                Cursor.Current = Cursors.Default;
//                return true;
//            }
//            catch
//            {
//                throw;
//            }
//        }


//        private void ProcessReserveStore()
//        {
//            foreach(UltraGridRow row in ultraGrid1.Rows)
//            {
//                LocateReserveStore(row);
//            }
//        }

//        private void LocateReserveStore(UltraGridRow row)
//        {
//            if (row.HasChild(null))
//            {
//                foreach (UltraGridChildBand cb in row.ChildBands)
//                {
//                    foreach (UltraGridRow childRow in cb.Rows)
//                    {
//                        LocateStartingStore(childRow);
//                    }
//                }
//            }
//            else
//            {
//                string st_id = (string)row.Cells["ST_ID"].Value;
//                if (st_id == "Reserve")
//                {
//                    SaveReserveStore(row);						
//                }
//            }
//        }

//        private void SaveReserveStore(UltraGridRow row)
//        {
//            bool activeInd = false;
//            int globalOptionsRid = _SAB.ClientServerSession.GlobalOptions.Key;
//            int reserveStoreRid = Convert.ToInt32(row.Cells["ST_RID"].Value, CultureInfo.CurrentUICulture);
//            activeInd = Convert.ToBoolean(row.Cells["ACTIVE_IND_B"].Value, CultureInfo.CurrentUICulture);

//            _go.OpenUpdateConnection();
//            if (activeInd)  // set active store
//                _go.UpdateReserveStore(globalOptionsRid,reserveStoreRid);
//            else			// unset active store
//                _go.UpdateReserveStore(globalOptionsRid,0);
//            _go.CommitData();
//            _go.CloseUpdateConnection();

//            // refresh session with cached Global Options
//            _SAB.ClientServerSession.RefreshGlobalOptions();
//            _SAB.ApplicationServerSession.RefreshGlobalOptions();
//        }

//        private void btnSaveView_Click(object sender, System.EventArgs e)
//        {
//            // Begin TT#2012 - JSmith - Layout corrupted if close before opens completely
//            //if (!ultraGrid1.IsDisposed)
//            if (FormLoaded && 
//                !ultraGrid1.IsDisposed)
//            // End TT#2012
//            {
//                InfragisticsLayoutData layoutData = new InfragisticsLayoutData();	
//                layoutData.InfragisticsLayout_Save(_SAB.ClientServerSession.UserRID, eLayoutID.storeMaintGrid, ultraGrid1);
//            }
//        }

//        private void btnRestoreView_Click(object sender, System.EventArgs e)
//        {
//            InfragisticsLayoutData layoutData = new InfragisticsLayoutData();	
//            layoutData.InfragisticsLayout_Delete(_SAB.ClientServerSession.UserRID, eLayoutID.storeMaintGrid);
////			FileStream FileLayout = new FileStream("StoreProfileMaintLayout.txt", FileMode.Truncate);
////			FileLayout.Close();
//            ultraGrid1.ResetDisplayLayout();
//            // Begin TT#3139 - JSmith - Store Profiles – read-only access
//            ApplyDefaults(ultraGrid1, true, 2, false);
//            DefaultGridLayout();
//            ApplyAppearanceCenterText(ultraGrid1);
//            SetColumnAlignment();
//            //Begin TT#169 - JSmith - enter max of 1000 and receive 'must be integer'
//            //ApplyDefaults(ultraGrid1);
//            //ApplyDefaults(ultraGrid1, true, 2, true);
//            ApplyDefaults(ultraGrid1, true, 2, true);
//            //End TT#169
            
//            //ApplySecurity();
//            if (_editEnabled)
//            {
//                ApplySecurity(FunctionSecurity);
//            }
//            else
//            {
//                ApplySecurity(_ReadOnlyFunctionSecurity);
//                ApplyNoEdit();
//                if (FunctionSecurity.AllowUpdate)
//                {
//                    btnSave.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Edit);
//                    btnSave.Enabled = true;
//                }
//            }
//            // End TT#3139 - JSmith - Store Profiles – read-only access
//        }

//        private void ultraGrid1_InitializeRow(object sender, Infragistics.Win.UltraWinGrid.InitializeRowEventArgs e)
//        {
//            if ((bool)e.Row.Cells["ACTIVE_IND_B"].Value == true)
//                e.Row.Cells["ACTIVE_IND"].Value = "1";
//            else
//                e.Row.Cells["ACTIVE_IND"].Value = "0";

//            if ((bool)e.Row.Cells["SHIP_ON_MONDAY_B"].Value == true)
//                e.Row.Cells["SHIP_ON_MONDAY"].Value = "1";
//            else
//                e.Row.Cells["SHIP_ON_MONDAY"].Value = "0";
//            if ((bool)e.Row.Cells["SHIP_ON_TUESDAY_B"].Value == true)
//                e.Row.Cells["SHIP_ON_TUESDAY"].Value = "1";
//            else
//                e.Row.Cells["SHIP_ON_TUESDAY"].Value = "0";
//            if ((bool)e.Row.Cells["SHIP_ON_WEDNESDAY_B"].Value == true)
//                e.Row.Cells["SHIP_ON_WEDNESDAY"].Value = "1";
//            else
//                e.Row.Cells["SHIP_ON_WEDNESDAY"].Value = "0";
//            if ((bool)e.Row.Cells["SHIP_ON_THURSDAY_B"].Value == true)
//                e.Row.Cells["SHIP_ON_THURSDAY"].Value = "1";
//            else
//                e.Row.Cells["SHIP_ON_THURSDAY"].Value = "0";
//            if ((bool)e.Row.Cells["SHIP_ON_FRIDAY_B"].Value == true)
//                e.Row.Cells["SHIP_ON_FRIDAY"].Value = "1";
//            else
//                e.Row.Cells["SHIP_ON_FRIDAY"].Value = "0";
//            if ((bool)e.Row.Cells["SHIP_ON_SATURDAY_B"].Value == true)
//                e.Row.Cells["SHIP_ON_SATURDAY"].Value = "1";
//            else
//                e.Row.Cells["SHIP_ON_SATURDAY"].Value = "0";
//            if ((bool)e.Row.Cells["SHIP_ON_SUNDAY_B"].Value == true)
//                e.Row.Cells["SHIP_ON_SUNDAY"].Value = "1";
//            else
//                e.Row.Cells["SHIP_ON_SUNDAY"].Value = "0";
//            if ((bool)e.Row.Cells["SIMILAR_STORE_MODEL_B"].Value == true)
//                e.Row.Cells["SIMILAR_STORE_MODEL"].Value = "1";
//            else
//                e.Row.Cells["SIMILAR_STORE_MODEL"].Value = "0";

//        }

//        private void ultraGrid1_BeforeRowInsert(object sender, Infragistics.Win.UltraWinGrid.BeforeRowInsertEventArgs e)
//        {
//            if (CheckInsertCondition(e) == false)
//            {
//                e.Cancel = true;
//                return;
//            }
//            else
//            {
//                ultraGrid1.BeginUpdate();
//                ultraGrid1.SuspendRowSynchronization();
//            }
//        }

//        private void ultraGrid1_AfterRowInsert(object sender, Infragistics.Win.UltraWinGrid.RowEventArgs e)
//        {
//            _insertedRows.Add(e.Row);
//            ultraGrid1.ActiveRow = e.Row;
//            //ultraGrid1.ActiveRowScrollRegion = ultraGrid1.DisplayLayout.RowScrollRegions[0];
//            ultraGrid1.ActiveColScrollRegion = ultraGrid1.DisplayLayout.ColScrollRegions[0];
//            ultraGrid1.ActiveCell = e.Row.Cells["ST_ID"];
//            e.Row.Cells["ST_RID"].Value = _tempRID--;
//            e.Row.Cells["Store Status"].Value = eStoreStatus.Comp;
//            // Begin MID Issue 3408 stodd
//            e.Row.Cells["SELLING_SQ_FT"].Value = 0;
//            e.Row.Cells["LEAD_TIME"].Value = 0;
//            // End MID Issue 3408 stodd
//            ultraGrid1.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.EnterEditMode, false, false);

//            ultraGrid1.ResumeRowSynchronization();
//            ultraGrid1.EndUpdate();

//        }

//        /// <summary>
//        /// checks to make sure that the previous row, if there is one, is completely
//        /// filled out. If any information is missing, return false to the calling
//        /// procedure to indicate that it should not proceed adding another row.
//        /// </summary>
//        /// <returns></returns>
//        private bool CheckInsertCondition(Infragistics.Win.UltraWinGrid.BeforeRowInsertEventArgs e)
//        {
//            //Check to see if there is already a Details row.
//            if (e.Band == ultraGrid1.DisplayLayout.Bands[0] && ultraGrid1.Rows.Count > 0)
//            {
//                //Find the last Details row and check its values. 			
//                UltraGridRow aRow = ultraGrid1.Rows[ultraGrid1.Rows.Count-1];

//                if (aRow.Cells["ST_ID"].Value.ToString() == "")
//                {
//                    MessageBox.Show("You must finish filling out the previous row.\r\nData Missing: Store ID.", "Error");
//                    return false;
//                }
//            }

//            //If we've gotten this far, there are no problems.
//            return true;
//        }

//        private void ultraGrid1_BeforeRowsDeleted(object sender, Infragistics.Win.UltraWinGrid.BeforeRowsDeletedEventArgs e)
//        {
//            bool allNewRows = false;

//            SelectedRowsCollection selectedRows = ultraGrid1.Selected.Rows;

//            foreach(UltraGridRow selRow in selectedRows)
//            {
//                allNewRows = false;
//                foreach(UltraGridRow newRow in _insertedRows)
//                {
//                    if(selRow == newRow)
//                    {
//                        allNewRows = true;
//                        break;
//                    }
//                }
//                //***********************************************************************
//                // if a row selected for deletion wasn't found in the new rows array,
//                // then at least one of the selected rows can't be deleted.
//                // if that's true, then NO stores are allowed to be deleted.
//                // users can ONLY delete newly inserted rows, that haven't been saved yet.
//                //***********************************************************************
//                if (allNewRows == false)
//                {
//                    break;
//                }

//            }
//            //**************************************************************
//            // If this is NOT a newly added row(s), then it cannot be deleted;
//            //**************************************************************
//            if (!allNewRows)
//            {
//                MessageBox.Show("Store(s) Cannot be deleted. \nInstead, make the store(s) inactive",
//                    "Deletion Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
//                e.Cancel = true;
//            }
//        }

//        private void menuSortAsc_Click(object sender, System.EventArgs e)
//        {
//            this.ultraGrid1.DisplayLayout.Bands[0].SortedColumns.Add(this._gridCol, false);
//        }

//        private void menuSortDesc_Click(object sender, System.EventArgs e)
//        {
//            this.ultraGrid1.DisplayLayout.Bands[0].SortedColumns.Add(this._gridCol, true);
//        }

//        private void menuSearch_Click(object sender, System.EventArgs e)
//        {
//            if ( this._gridCol == null ) { return; }

//            if ( this._frmUltraGridSearchReplace == null ) 
//            {
//                this._frmUltraGridSearchReplace = new frmUltraGridSearchReplace(_SAB, false);
//            }

//            this._frmUltraGridSearchReplace.ShowSearchReplace(ultraGrid1, _gridCol.Header.Caption);
//        }

//        private void ultraGrid1_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
//        {
//            try
//            {
//                Infragistics.Win.UIElement mouseUIElement;
//                Infragistics.Win.UIElement headerUIElement;
//                HeaderUIElement headerUI = null;
//                _point = new Point(e.X, e.Y);

//                if (e.Button == MouseButtons.Right)
//                {
//                    // retrieve the UIElement from the location of the mouse 
//                    mouseUIElement = ultraGrid1.DisplayLayout.UIElement.ElementFromPoint(new Point(e.X, e.Y));
//                    if ( mouseUIElement == null ) { return; }

//                    headerUIElement = mouseUIElement.GetAncestor(typeof(HeaderUIElement));
//                    if ( headerUIElement == null ) { return; }

//                    if ( headerUIElement.GetType() == typeof(HeaderUIElement) )
//                    {
//                        headerUI = (HeaderUIElement)headerUIElement;
//                        Infragistics.Win.UltraWinGrid.ColumnHeader colHeader = null;
//                        _gridCol = null;
//                        colHeader = (Infragistics.Win.UltraWinGrid.ColumnHeader)headerUI.SelectableItem;
//                        _gridCol = colHeader.Column;
//                        if ( _gridCol == null )
//                            return;

//                        this.mnuGridHeader.Show(this.ultraGrid1, _point);

//                    }
//                }
//            }
//            catch
//            {
//                throw;
//            }
//        }

//        private void ultraGrid1_BeforeEnterEditMode(object sender, System.ComponentModel.CancelEventArgs e)
//        {
//            Infragistics.Win.UIElement mouseUIElement;
//            Infragistics.Win.UltraWinGrid.UltraGridCell aCell;
	
//            // retrieve the UIElement from the location of the mouse 
//            mouseUIElement = ultraGrid1.DisplayLayout.UIElement.ElementFromPoint(_point);
//            if ( mouseUIElement == null ) 
//            { 
//                e.Cancel = true; 
//            }
//            else
//            {
//                aCell = (UltraGridCell)mouseUIElement.GetContext(typeof(Infragistics.Win.UltraWinGrid.UltraGridCell)); 
//                if ( aCell == null ) 
//                { 
//                    aCell = ultraGrid1.ActiveCell; 
//                }
				
//                //****************************************************************************
//                // You can NOT edit the Store Status column or the Reserve Store's Store ID
//                //****************************************************************************
//                if (aCell.Column.Key == _storeStatusText) 
//                { 
//                    e.Cancel = true; 
//                }

//                if (aCell.Column.Key == "ST_ID") 
//                {
//                    if (aCell.Value != DBNull.Value)
//                    {
//                        if ((string)aCell.Value == "Reserve") 
//                        { 
//                            MessageBox.Show("The Reserve Store's ID cannot be changed.",
//                                "Reserve Store ID", MessageBoxButtons.OK, MessageBoxIcon.Information);
//                            e.Cancel = true;
//                        }
//                    }
//                }
//            }
//        }

//        private void Row_Changed(object ob, DataRowChangeEventArgs e) 
//        { 
//            if (FormLoaded)
//            {
//                ChangePending = true;
//            }
//        }

//        private void ultraGrid1_CellChange(object sender, Infragistics.Win.UltraWinGrid.CellEventArgs e)
//        {
//            if (FormLoaded)
//            {
//                ChangePending = true;
//                if (e.Cell.Column.Key == "ACTIVE_IND_B")
//                {
//                    _storeActiveChanged = true;
//                }
//            }
//        }

//        #region Validation
//        private bool ValidStores()
//        {
//            bool isOk = true;
//            foreach(UltraGridRow row in ultraGrid1.Rows)
//            {
//                isOk = ValidateEachStoreRow(row);
//                if (!isOk) break;  // Found error.  Stop processing.
//            }
//            return isOk;
//        }

//        private bool ValidateEachStoreRow(UltraGridRow row)
//        {
//            bool isOk = true;



//            if ( row.Cells != null)
//            {
//                if (row.Cells["ST_ID"].Text.Length == 0)
//                {
//                    ultraGrid1.ActiveRow = row;
//                    row.Expanded = true;
//                    ultraGrid1.ActiveCell = row.Cells["ST_ID"];
//                    ultraGrid1.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.EnterEditMode, false, false);
//                    isOk = false;
//                    MessageBox.Show("Store ID is a required field.",this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);						
//                }
//                //				if (isOk)
//                //				{
//                //					if (!ValidSellingOpenDate(row, row.Cells["SELLING_OPEN_DATE"].Value))
//                //					{
//                //						ultraGrid1.ActiveRow = row;
//                //						row.Expanded = true;
//                //						ultraGrid1.ActiveCell = row.Cells["SELLING_OPEN_DATE"];
//                //						ultraGrid1.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.EnterEditMode, false, false);
//                //						isOk = false;
//                //						//MessageBox.Show("Selling Open Date is not prior to Selling Close Date.",this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);						
//                //					}
//                //				}
//                //				if (isOk)
//                //				{
//                //					if (!ValidStockOpenDate(row, row.Cells["STOCK_OPEN_DATE"].Value))
//                //					{
//                //						ultraGrid1.ActiveRow = row;
//                //						row.Expanded = true;
//                //						ultraGrid1.ActiveCell = row.Cells["STOCK_OPEN_DATE"];
//                //						ultraGrid1.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.EnterEditMode, false, false);
//                //						isOk = false;
//                //						//MessageBox.Show("Stock Open Date is not prior to Stock Close Date.",this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);						
//                //					}
//                //				}
//            }
			
//            if (row.HasChild(null))
//            {
//                foreach (UltraGridChildBand cb in row.ChildBands)
//                {
//                    foreach (UltraGridRow childRow in cb.Rows)
//                    {
//                        isOk = ValidateEachStoreRow(childRow);
//                        if (!isOk) break;  // Found error.  Stop processing.
//                    }
//                    if (!isOk) break; // Found error.  Stop processing.
//                }
//            }
//            return isOk;
//        }

//        // Begin TT#11 - JSmith - The view of the calendar needs to pay attention to the culture info when displaying the dates.
//        //// Begin MID Issue 2453
//        ///// <summary>
//        ///// Converts a cell value to DateTime.
//        ///// Used in Validation.
//        ///// </summary>
//        ///// <param name="cellValue"></param>
//        ///// <returns></returns>
//        //private DateTime ConvertCellValueToDateTime(object cellValue)
//        //{
//        //    char [] delim = { '/' };
//        //    string [] dates = cellValue.ToString().Split(delim,3);
//        //    dates[0] = dates[0].Replace("_", " ");
//        //    dates[1] = dates[1].Replace("_", " ");
//        //    dates[2] = dates[2].Replace("_", " ");

//        //    // Begin MID Issue 2453 stodd
//        //    int dYear = Convert.ToInt32( dates[2], CultureInfo.CurrentUICulture );
//        //    // now we must do some fancy footwork to get around the infragistics grid.
//        //    // After we do our editing, infragistics is kind enough to change a year of 4, 
//        //    // to 2004.  Unfortunately we've already determined that 4 is outside the valid years
//        //    // we can handle in the calendar...
//        //    DateTime date = DateTime.Now;
//        //    int century = (date.Year / 100) * 100;
//        //    if (dYear < 100)
//        //    {
//        //        if (dYear < 30)
//        //        {
//        //            dYear = century + dYear;
//        //        }
//        //        else
//        //        {
//        //            dYear = century - 100 + dYear;
//        //        }
//        //    }
//        //    // End MID Issue 2453 stodd

//        //    int dDay = Convert.ToInt32( dates[1], CultureInfo.CurrentUICulture );
//        //    int dMonth = Convert.ToInt32( dates[0], CultureInfo.CurrentUICulture );
//        //    DateTime aDate = new DateTime(dYear, dMonth, dDay);
//        //    return aDate;
//        //}
//        //// End MID Issue 2453
//        // End TT#11 - JSmith - The view of the calendar needs to pay attention to the culture info when displaying the dates.

//        private bool ValidSellingOpenDate(UltraGridRow row, object cellValue)
//        {
//            bool valid = true;

//            // Begin TT#11 - JSmith - The view of the calendar needs to pay attention to the culture info when displaying the dates.
//            //if (ultraGrid1.ActiveCell.EditorResolved.Value == System.DBNull.Value)
//            //    _sellingOpenDt = Include.UndefinedDate;
//            //else
//            //{
//            //    _sellingOpenDt = ConvertCellValueToDateTime(cellValue);
//            //}
//            if (!ultraGrid1.ActiveCell.EditorResolved.IsValid)
//            {
//                throw new FormatException();
//            }
//            else if (ultraGrid1.ActiveCell.EditorResolved.Value == System.DBNull.Value)
//                _sellingOpenDt = Include.UndefinedDate;
//            else
//            {
//                _sellingOpenDt = Convert.ToDateTime(ultraGrid1.ActiveCell.EditorResolved.Value);
//            }
//            // End TT#11 - JSmith - The view of the calendar needs to pay attention to the culture info when displaying the dates.
			
//            if (row.Cells["SELLING_CLOSE_DATE"].Value == DBNull.Value)
//                _sellingCloseDt = Include.UndefinedDate;
//            else
//                _sellingCloseDt	= (DateTime)row.Cells["SELLING_CLOSE_DATE"].Value;

//            if (_sellingOpenDt > _sellingCloseDt && _sellingCloseDt != Include.UndefinedDate)
//            {
//                string errorMessage = "Selling Open Date is after Selling Close Date";
//                MessageBox.Show (errorMessage,  this.Text, 	MessageBoxButtons.OK, MessageBoxIcon.Error);

//                valid = false;

//            }
//            return valid;
//        }

//        private bool ValidSellingCloseDate(UltraGridRow row, object cellValue)
//        {
//            bool valid = true;

//            if (row.Cells["SELLING_OPEN_DATE"].Value == DBNull.Value)
//                _sellingOpenDt = Include.UndefinedDate;
//            else
//                _sellingOpenDt	= (DateTime)row.Cells["SELLING_OPEN_DATE"].Value;

//            // Begin TT#11 - JSmith - The view of the calendar needs to pay attention to the culture info when displaying the dates.
//            //if (cellValue == System.DBNull.Value || (string)cellValue == "__/__/____")
//            //    _sellingCloseDt = Include.UndefinedDate;
//            //else
//            //{
//            //    _sellingCloseDt = ConvertCellValueToDateTime(cellValue);
//            //}
//            if (!ultraGrid1.ActiveCell.EditorResolved.IsValid)
//            {
//                throw new FormatException();
//            }
//            else if (ultraGrid1.ActiveCell.EditorResolved.Value == System.DBNull.Value)
//                _sellingCloseDt = Include.UndefinedDate;
//            else
//            {
//                _sellingCloseDt = Convert.ToDateTime(ultraGrid1.ActiveCell.EditorResolved.Value);
//            }
//            // End TT#11 - JSmith - The view of the calendar needs to pay attention to the culture info when displaying the dates.

//            if (_sellingOpenDt > _sellingCloseDt && _sellingCloseDt != Include.UndefinedDate)
//            {
//                string errorMessage = "Selling Close Date is prior to Selling Open Date";
//                MessageBox.Show (errorMessage,  this.Text, 	MessageBoxButtons.OK, MessageBoxIcon.Error);

//                valid = false;
//            }

//            return valid;
//        }

//        private bool ValidStockOpenDate(UltraGridRow row, object cellValue)
//        {
//            bool valid = true;

//            // Begin TT#11 - JSmith - The view of the calendar needs to pay attention to the culture info when displaying the dates.
//            //if (cellValue == System.DBNull.Value || (string)cellValue == "__/__/____")
//            //    _stockOpenDt = Include.UndefinedDate;
//            //else
//            //{
//            //    _stockOpenDt = ConvertCellValueToDateTime(cellValue);
//            //}
//            if (!ultraGrid1.ActiveCell.EditorResolved.IsValid)
//            {
//                throw new FormatException();
//            }
//            else if (ultraGrid1.ActiveCell.EditorResolved.Value == System.DBNull.Value)
//                _stockOpenDt = Include.UndefinedDate;
//            else
//            {
//                _stockOpenDt = Convert.ToDateTime(ultraGrid1.ActiveCell.EditorResolved.Value);
//            }
//            // End TT#11 - JSmith - The view of the calendar needs to pay attention to the culture info when displaying the dates.
			
//            if (row.Cells["STOCK_CLOSE_DATE"].Value == DBNull.Value)
//                _stockCloseDt = Include.UndefinedDate;
//            else
//                _stockCloseDt	= (DateTime)row.Cells["STOCK_CLOSE_DATE"].Value;

//            if (_stockOpenDt > _stockCloseDt && _stockCloseDt != Include.UndefinedDate)
//            {
//                string errorMessage = "Stock Open Date is after Stock Close Date";
//                MessageBox.Show (errorMessage,  this.Text, 	MessageBoxButtons.OK, MessageBoxIcon.Error);

//                valid = false;

//            }
//            return valid;
//        }

//        private bool ValidStockCloseDate(UltraGridRow row, object cellValue)
//        {
//            bool valid = true;

//            if (row.Cells["STOCK_OPEN_DATE"].Value == DBNull.Value)
//                _stockOpenDt = Include.UndefinedDate;
//            else
//                _stockOpenDt	= (DateTime)row.Cells["STOCK_OPEN_DATE"].Value;

//            // Begin TT#11 - JSmith - The view of the calendar needs to pay attention to the culture info when displaying the dates.
//            //if (cellValue == System.DBNull.Value || (string)cellValue == "__/__/____")
//            //    _stockCloseDt = Include.UndefinedDate;
//            //else
//            //{
//            //    _stockCloseDt = ConvertCellValueToDateTime(cellValue);
//            //}
//            if (!ultraGrid1.ActiveCell.EditorResolved.IsValid)
//            {
//                throw new FormatException();
//            }
//            else if (ultraGrid1.ActiveCell.EditorResolved.Value == System.DBNull.Value)
//                _stockCloseDt = Include.UndefinedDate;
//            else
//            {
//                _stockCloseDt = Convert.ToDateTime(ultraGrid1.ActiveCell.EditorResolved.Value);
//            }
//            // End TT#11 - JSmith - The view of the calendar needs to pay attention to the culture info when displaying the dates.

//            if (_stockOpenDt > _stockCloseDt && _stockCloseDt != Include.UndefinedDate)
//            {
//                string errorMessage = "Stock Close Date is prior to Stock Open Date";
//                MessageBox.Show (errorMessage,  this.Text, 	MessageBoxButtons.OK, MessageBoxIcon.Error);

//                valid = false;
//            }

//            return valid;
//        }

//        // Begin Track #3956 stodd
//        //private string ValidStoreGroupLevelName(string groupLevelName)
//        //{
//        //    bool nameOk = false;
//        //    NameDialog errorDialog = null;

//        //    while (!nameOk)
//        //    {
//        //        if (groupLevelName.Length > Include.StoreGroupLevelIDMaxSize)
//        //        {
//        //            if (errorDialog == null)
//        //            {
//        //                errorDialog = new NameDialog("Enter Store Attribute Set Name", "Store Attribute Set Name");
//        //            }
//        //            errorDialog.StartPosition = FormStartPosition.CenterScreen;
//        //            errorDialog.TextValue = groupLevelName;


//        //            MessageBox.Show("Attribute Set Name exceeds maximum of " + Include.StoreGroupLevelIDMaxSize +
//        //                " characters.  Please correct.",this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);	
//        //            errorDialog.TextValue = groupLevelName;
//        //            DialogResult theResult = errorDialog.ShowDialog();
//        //            if (theResult == DialogResult.OK)
//        //            {
//        //                groupLevelName = errorDialog.TextValue;
//        //                if (groupLevelName.Length <= Include.StoreGroupLevelIDMaxSize)
//        //                    nameOk = true;
//        //            }
//        //            else
//        //            {
//        //                groupLevelName = groupLevelName.Substring(0,50);
//        //                nameOk = true;
//        //            }
//        //        }
//        //        else
//        //            nameOk = true;
//        //    }
//        //    return groupLevelName;
//        //}
//        // End Track #3956

//        // Begin Track #4872 - JSmith - Global/User Attributes
//        //private bool ValidStoreGroupName(ref string groupName)
//        private bool ValidStoreGroupName(ref string groupName, ref eGlobalUserType aGlobalUserType)
//        // End Track #4872
//        {
//            NameDialog groupNameDialog = new NameDialog("Enter Store Attribute Name", "Store Attribute Name");
//            groupNameDialog.StartPosition = FormStartPosition.CenterScreen;
//            groupNameDialog.TextValue = groupName;
//            // Begin Track #4872 - JSmith - Global/User Attributes
//            if (_StoreAttributeSecurity.AllowUpdate &&
//                _UserStoreAttributeSecurity.AllowUpdate)
//            {
//                groupNameDialog.ShowUserGlobal = true;
//            }
//            // End Track #4872
//            bool nameOk = false;
//            bool cancelAction = false;
//            while (!(nameOk || cancelAction))
//            {
//                DialogResult theResult = groupNameDialog.ShowDialog();
//                if (theResult == DialogResult.OK)
//                {
//                    groupName = groupNameDialog.TextValue;
//                    // Begin Track #4872 - JSmith - Global/User Attributes
//                    if (_StoreAttributeSecurity.AllowUpdate &&
//                        _UserStoreAttributeSecurity.AllowUpdate)
//                    {
//                        if (groupNameDialog.isGlobalChecked)
//                        {
//                            aGlobalUserType = eGlobalUserType.Global;
//                        }
//                        else
//                        {
//                            aGlobalUserType = eGlobalUserType.User;
//                        }
//                    }
//                    else if (_StoreAttributeSecurity.AllowUpdate)
//                    {
//                        aGlobalUserType = eGlobalUserType.Global;
//                    }
//                    else if (_UserStoreAttributeSecurity.AllowUpdate)
//                    {
//                        aGlobalUserType = eGlobalUserType.User;
//                    }
//                    // End Track #4872
//                    if (groupName.Length > Include.StoreGroupIDMaxSize)
//                        MessageBox.Show("Attribute Name exceeds maximum of " + Include.StoreGroupIDMaxSize +
//                            " characters.  Please correct.",this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);						
//                    else
//                    {
//                        // Begin Track #4872 - JSmith - Global/User Attributes
//                        int userRID;
//                        if (aGlobalUserType == eGlobalUserType.Global)
//                        {
//                            userRID = Include.GlobalUserRID;
//                        }
//                        else
//                        {
//                            userRID = SAB.ClientServerSession.UserRID; ;
//                        }
//                        //bool groupExists = _storeSession.DoesGroupNameExist(groupName);
//                        bool groupExists = _storeSession.DoesGroupNameExist(groupName, userRID);
//                        // End Track #4872
//                        if (groupExists)
//                            MessageBox.Show("A store attribute already exists with the name - " + groupName,this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);						
//                        else
//                            nameOk = true;
//                    }
//                }
//                else
//                {
//                    cancelAction = true;
//                }
//            }

//            return nameOk;
//        }


//        #endregion

//        private void ultraGrid1_BeforeExitEditMode(object sender, Infragistics.Win.UltraWinGrid.BeforeExitEditModeEventArgs e)
//        {
//            try
//            {
//                switch (this.ultraGrid1.ActiveCell.Column.Key)
//                {
//                    case "SELLING_OPEN_DATE":
//                        if (!ValidSellingOpenDate(ultraGrid1.ActiveRow, ultraGrid1.ActiveCell.Text))
//                        {
//                            e.Cancel = true;
//                            _invalidCell = ultraGrid1.ActiveCell;
//                        }
//                        break;
//                    case "SELLING_CLOSE_DATE":
//                        if (!ValidSellingCloseDate(ultraGrid1.ActiveRow, ultraGrid1.ActiveCell.Text))
//                        {
//                            e.Cancel = true;
//                            _invalidCell = ultraGrid1.ActiveCell;
//                        }
//                        break;
//                    case "STOCK_OPEN_DATE":
//                        if (!ValidStockOpenDate(ultraGrid1.ActiveRow, ultraGrid1.ActiveCell.Text))
//                        {
//                            e.Cancel = true;
//                            _invalidCell = ultraGrid1.ActiveCell;
//                        }
//                        break;
//                    case "STOCK_CLOSE_DATE":
//                        if (!ValidStockCloseDate(ultraGrid1.ActiveRow, ultraGrid1.ActiveCell.Text))
//                        {
//                            e.Cancel = true;
//                            _invalidCell = ultraGrid1.ActiveCell;
//                        }
//                        break;
//                }

//                if (!e.Cancel)
//                {
//                    if ((ultraGrid1.ActiveCell.Column.Key == "SELLING_OPEN_DATE"
//                        || ultraGrid1.ActiveCell.Column.Key == "SELLING_CLOSE_DATE"))
//                    {
//                        WeekProfile currentWeek = _SAB.ClientServerSession.Calendar.CurrentWeek;

//                        eStoreStatus storeStatus = _SAB.StoreServerSession.GetStoreStatus(currentWeek, _sellingOpenDt, _sellingCloseDt);

//                        ultraGrid1.ActiveCell.Row.Cells["Store Status"].Value = storeStatus;
//                    }
//                }
//            }
//            catch ( FormatException )
//            {
//                string errorMessage = "Invalid Date Format";
//                MessageBox.Show (errorMessage,  this.Text, 	MessageBoxButtons.OK, MessageBoxIcon.Error);
//                e.Cancel = true;

//            }
//            catch ( Exception err )
//            {
//                // Begin MID Issue 2453
//                e.Cancel = true;
//                this.HandleException(err, false);
//                // End MID Issue 2453
//            }
//        }
//        private void ultraGrid1_BeforeCellUpdate(object sender, Infragistics.Win.UltraWinGrid.BeforeCellUpdateEventArgs e)
//        {
//            //BEGIN TT#858 - MD - DOConnell - Do not allow a store to be set to Inactive if it has allocation quantities or Intransit
//            if (FormLoaded)
//            {
//                if (e.Cell.Column.Key == "ACTIVE_IND_B")
//                {
//                    if (Convert.ToBoolean(e.Cell.Value) == true)
//                    {
//                        int stRID = Convert.ToInt32(e.Cell.Row.Cells["ST_RID"].Value);

//                        int allocationCount = _SAB.StoreServerSession.GetAllStoreAllocationCounts(stRID);
//                        //int intransitCount = _SAB.StoreServerSession.GetAllStoreAllocationCounts(stRID);
//                        int intransitCount = _SAB.StoreServerSession.GetAllStoreIntransitCounts(stRID);
//                        if (allocationCount > 0 || intransitCount > 0)
//                        {
//                            //BEGIN TT#858 - MD - DOConnell - Correct serialization error
//                            //string msgDetails = _SAB.StoreServerSession.Audit.GetText(eMIDTextCode.msg_DoNotAllowInactive, false);
//                            string msgDetails = MIDText.GetTextOnly(eMIDTextCode.msg_DoNotAllowInactive);
//                            //END TT#858 - MD - DOConnell - Correct serialization error

//                            //_editMsgs.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_CannotMarkReserveStoreInactive, msgDetails, _sourceModule);
//                            MessageBox.Show(msgDetails, "Store has Units", MessageBoxButtons.OK, MessageBoxIcon.Information);

//                            ChangePending = false;
//                            e.Cancel = true;
//                        }
//                    }
//                }
//            }
//            //END TT#858 - MD - DOConnell - Do not allow a store to be set to Inactive if it has allocation quantities or Intransit
//        }

//        private void ultraGrid1_BeforeCellDeactivate(object sender, System.ComponentModel.CancelEventArgs e)
//        {
//            try
//            {
//                if (_invalidCell != null)
//                {
//                    e.Cancel = true;
//                    ultraGrid1.ActiveCell = _invalidCell;
//                    ultraGrid1.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.EnterEditMode, false, false);
//                    _invalidCell = null;
//                }
//            }
//            catch
//            {
//                throw;
//            }
//        }

//        private void ultraGrid1_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
//        {
//            if (_invalidCell != null)
//            {
//                e.Handled = true;
//                ultraGrid1.ActiveCell = _invalidCell;
//                ultraGrid1.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.EnterEditMode, false, false);
//                _invalidCell = null;
//            }
//        }

//        private void ultraGrid1_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
//        {
//            if (_invalidCell != null)
//            {
//                ultraGrid1.ActiveCell = _invalidCell;
//                ultraGrid1.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.EnterEditMode, false, false);
//                _invalidCell = null;
//            }
//        }

//        private void ultraGrid1_AfterCellUpdate(object sender, Infragistics.Win.UltraWinGrid.CellEventArgs e)
//        {
			
			
//        }

//        private void ultraGrid1_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
//        {
//            _point.X = 0;
//            _point.Y = 0;

//            // BEGIN Issue 4959/4960 stodd 12.3.2007
//            if (e.KeyCode == Keys.H && (e.Control && e.Shift)) 
//            {
//                ultraGrid1.BeginUpdate();
//                foreach (UltraGridColumn cColumn in this.ultraGrid1.DisplayLayout.Bands[0].Columns)
//                {
//                    //=====================================================
//                    // Hide or reveal store char group value 'key' columns
//                    //=====================================================
//                    if (cColumn.Header.Caption.StartsWith("SCGRID__"))
//                        if (cColumn.Hidden)
//                            cColumn.Hidden = false;
//                        else
//                            cColumn.Hidden = true;
//                }
//                ultraGrid1.EndUpdate();
//            }
//            // END Issue 4959/4960 stodd 12.3.2007
//        }

//        private void ultraGrid1_InitializeGroupByRow(object sender, Infragistics.Win.UltraWinGrid.InitializeGroupByRowEventArgs e)
//        {
////			e.Row.Description = ((DateTime)e.Row.Value).ToString("d");

//            if (e.Row.HasChild())
//                e.Row.ExpandAll();
//        }

//        private void ultraGrid1_Validated(object sender, System.EventArgs e)
//        {
//            try
//            {


//            }
//            catch
//            {
//                throw;
//            }
//        }

//        private void ultraGrid1_Validating(object sender, System.ComponentModel.CancelEventArgs e)
//        {
//            try
//            {


//            }
//            catch
//            {
//                throw;
//            }
//        }

//        #region IFormBase Members
//        override public void ICut()
//        {
			
//        }

//        override public void ICopy()
//        {
			
//        }

//        override public void IPaste()
//        {
			
//        }	

////		override public void IClose()
////		{
////			try
////			{
////				this.Close();
////
////			}		
////			catch(Exception ex)
////			{
////				MessageBox.Show(ex.Message);
////			}
////			
////		}

//        override public void ISave()
//        {
//            try
//            {
//                if (SaveChanges())
//                {
//                    ChangePending = false;
//                }
//            }		
//            catch(Exception ex)
//            {
//                MessageBox.Show(ex.Message);
//            }
//        }

//        override public void ISaveAs()
//        {
			
//        }

//        override public void IDelete()
//        {
			
//        }

//        override public void IRefresh()
//        {
			
//        }
		
//        #endregion

//        private void StoreProfileMaint_Leave(object sender, System.EventArgs e)
//        {
//            rc = Infragistics.Win.UltraWinGrid.Resources.Customizer;
//            rc.ResetCustomizedString("DataErrorRowUpdateUnableToUpdateRow");
//        }

//        private void StoreProfileMaint_Enter(object sender, System.EventArgs e)
//        {
//            rc = Infragistics.Win.UltraWinGrid.Resources.Customizer;
//            rc.SetCustomizedString("DataErrorRowUpdateUnableToUpdateRow", 
//                "Store ID must be unique.  Enter a unique Store ID before continuing.");

//        }

//        private void StoreProfileMaint_Closing(object sender, System.ComponentModel.CancelEventArgs e)
//        {
//            _SAB.StoreServerSession.ClearAllStoreDataTable();
//        }
//    }

//    //Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
//    //public class StoreGroupChangeEventArgs : EventArgs
//    //{
//    //    StoreGroupProfile _storeGroup;		
//    //    public StoreGroupChangeEventArgs(StoreGroupProfile storeGroup)
//    //    {
//    //        _storeGroup = storeGroup;
//    //    }
//    //    public StoreGroupProfile StoreGroup 
//    //    {
//    //        get { return _storeGroup ; }
//    //        set { _storeGroup = value; }
//    //    }
//    //}

//    //End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
//    public class StoreChangeEventArgs : EventArgs
//    {
//        StoreProfile _storeProfile;
//        public StoreChangeEventArgs(StoreProfile store)
//        {
//            _storeProfile = store;
//        }
//        public StoreProfile StoreProfile 
//        {
//            get { return _storeProfile ; }
//            set { _storeProfile = value; }
//        }
//    }
//}
