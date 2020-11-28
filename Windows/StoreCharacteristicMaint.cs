//using System;
//using System.Drawing;
//using System.Collections;
//using System.ComponentModel;
//using System.Text.RegularExpressions;
//using System.Windows.Forms;
//using System.Globalization;
//using System.Data;
//using System.Diagnostics;

//using Infragistics.Win;
//using Infragistics.Win.UltraWinGrid;

//using MIDRetail.Business;
//using MIDRetail.DataCommon;
//using MIDRetail.Common;
//using MIDRetail.Data;


//namespace MIDRetail.Windows
//{
//    /// <summary>
//    /// Summary description for StoreCharacteristicMaint.
//    /// </summary>
//    public class StoreCharacteristicMaint : MIDFormBase
//    {
//        private SessionAddressBlock _SAB;
//        private ExplorerAddressBlock _EAB;
//        private Infragistics.Win.UltraWinGrid.UltraGrid charGrid;
//        private System.Windows.Forms.Button btnSave;
//        private System.Windows.Forms.Button btnClose;
//        /// <summary>
//        /// Required designer variable.
//        /// </summary>
//        private System.ComponentModel.Container components = null;
//        private StoreServerSession _storeSession;
//        //Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
//        private FolderDataLayer _dlFolder;
//        //End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
//        private DataSet _storeCharDataSet;
//        private Hashtable _htGridState;
//        private Point _point;
//        private int _dummyKey;
//        private UltraGridCell _invalidCell;
//        private ArrayList _deletedStoreCharGroupIDs;
//        private ArrayList _deleteStoreCharGroupKeys;
//        private ArrayList _deletedStoreCharValueKeys;
//        //private bool _ignoreEnterEdit = false;

//        //Begin TT#724 - JScott - The "Available Stores" attribute set can be renamed in the Store Group Explorer and could cause processing problems
//        //private string _availableStoresText = MIDText.GetTextOnly(eMIDTextCode.lbl_AvailableStores);

//        //End TT#724 - JScott - The "Available Stores" attribute set can be renamed in the Store Group Explorer and could cause processing problems
//        // add event to update explorer when new group is added
//        //Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
//        //public delegate void StoreGroupChangeEventHandler(object source, StoreGroupChangeEventArgs e);
//        //public event StoreGroupChangeEventHandler OnStoreGroupPropertyChangeHandler;
//        //End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders

//        Infragistics.Shared.ResourceCustomizer _rc;
//        //================================================================================
//        // Due to the nature of how the characteristic groups are stored and searched, 
//        // these words by themselves cannot be used as a store char group name.
//        //================================================================================
//        // Begin Issue 4106 - stodd
//        private string [] RESERVED_WORDS = { "BETWEEN","GREATER","LESS","THAN","EQUALS","IN","AND","OR","=","(",")","<",">",
//        "ST_RID","ST_ID","STORE_NAME","STORE_DESC","ACTIVE_IND","CITY","STATE","SELLING_SQ_FT","SELLING_OPEN_DT","SELLING_CLOSE_DATE",
//        "STOCK_OPEN_DATE","STOCK_CLOSE_DATE","LEAD_TIME","SHIP_ON_MONDAY","SHIP_ON_TUESDAY","SHIP_ON_WEDNESDAY",
//        "SHIP_ON_THURSDAY","SHIP_ON_FRIDAY","SHIP_ON_SATURDAY","SHIP_ON_SUNDAY","SIMILAR_STORE_MODEL" };
//        private System.Windows.Forms.ContextMenu menuChar;
//        private System.Windows.Forms.MenuItem menuInsert;
//        private System.Windows.Forms.MenuItem menuDelete;
//        // End issue 4106
//        private ArrayList _reserverWords;

//        //BEGIN TT#110-MD-VStuart - In Use Tool
//        private bool _display;
//        private MenuItem menuInUseInfo;
//        private ArrayList _ridList;
//        //END TT#110-MD-VStuart - In Use Tool
//        //BEGIN TT#667-MD-VStuart-In Use Error in Store Characteristics upon aborted delete of Child Record
//        private bool _skipInUse = false;
//        //END   TT#667-MD-VStuart-In Use Error in Store Characteristics upon aborted delete of Child Record
//        //BEGIN TT#643-MD-VStuart-Need Queries for Store Characteristic Values
//        private eProfileType etype;  
//        private bool inQuiry = true;
//        //END   TT#643-MD-VStuart-Need Queries for Store Characteristic Values


//        public StoreCharacteristicMaint(SessionAddressBlock aSAB, ExplorerAddressBlock aEAB) : base (aSAB)
//        {
//            _SAB = aSAB;
//            _EAB = aEAB;
//            _reserverWords = new ArrayList(RESERVED_WORDS);
//            InitializeComponent();
//            _storeSession = _SAB.StoreServerSession;
//            _storeCharDataSet = _storeSession.GetAllStoreCharacteristicsData().Copy();
//            //Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
//            _dlFolder = new FolderDataLayer();
//            //End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders

//            charGrid.SetDataBinding(_storeCharDataSet, "");
//            DisableFields();

//            _dummyKey = -1;
//            _invalidCell = null;
//            _deletedStoreCharGroupIDs = new ArrayList();
//            _deletedStoreCharValueKeys = new ArrayList();
//            _deleteStoreCharGroupKeys = new ArrayList();

//            _rc = Infragistics.Win.UltraWinGrid.Resources.Customizer;
//            _rc.SetCustomizedString("DataErrorRowUpdateUnableToUpdateRow", 
//                "Unable to Update Row. Please press OK to continue.");

//            FunctionSecurity = _SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AdminStoresCharacteristics);
//        }

//        /// <summary>
//        /// Clean up any resources being used.
//        /// </summary>
//        protected override void Dispose( bool disposing )
//        {
//            if( disposing )
//            {
//                if(components != null)
//                {
//                    components.Dispose();
//                }

//                this.charGrid.Click -= new System.EventHandler(this.charGrid_Click);
//                this.charGrid.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.charGrid_MouseDown);
//                this.charGrid.MouseUp -= new System.Windows.Forms.MouseEventHandler(this.charGrid_MouseUp);
//                this.charGrid.CellChange -= new Infragistics.Win.UltraWinGrid.CellEventHandler(this.charGrid_CellChange);
//                this.charGrid.BeforeExitEditMode -= new Infragistics.Win.UltraWinGrid.BeforeExitEditModeEventHandler(this.charGrid_BeforeExitEditMode);
//                this.charGrid.AfterRowsDeleted -= new System.EventHandler(this.charGrid_AfterRowsDeleted);
//                this.charGrid.AfterRowInsert -= new Infragistics.Win.UltraWinGrid.RowEventHandler(this.charGrid_AfterRowInsert);
//                this.charGrid.BeforeRowsDeleted -= new Infragistics.Win.UltraWinGrid.BeforeRowsDeletedEventHandler(this.charGrid_BeforeRowsDeleted);
//                this.charGrid.BeforeRowInsert -= new Infragistics.Win.UltraWinGrid.BeforeRowInsertEventHandler(this.charGrid_BeforeRowInsert);
//                this.charGrid.KeyDown -= new System.Windows.Forms.KeyEventHandler(this.charGrid_KeyDown);
//                this.charGrid.BeforeRowExpanded -= new Infragistics.Win.UltraWinGrid.CancelableRowEventHandler(this.charGrid_BeforeRowExpanded);
//                this.charGrid.KeyUp -= new System.Windows.Forms.KeyEventHandler(this.charGrid_KeyUp);
//                this.charGrid.InitializeLayout -= new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.charGrid_InitializeLayout);
//                //Begin TT#169 - JSmith - enter max of 1000 and receive 'must be integer'
//                MIDRetail.Windows.Controls.UltraGridLayoutDefaults ugld = new MIDRetail.Windows.Controls.UltraGridLayoutDefaults(ErrorImage);
//                ugld.DetachGridEventHandlers(charGrid);
//                //End TT#169
//                this.charGrid.BeforeCellDeactivate -= new System.ComponentModel.CancelEventHandler(this.charGrid_BeforeCellDeactivate);
//                this.charGrid.BeforeEnterEditMode -= new System.ComponentModel.CancelEventHandler(this.charGrid_BeforeEnterEditMode);
//                this.btnSave.Click -= new System.EventHandler(this.btnSave_Click);
//                this.btnClose.Click -= new System.EventHandler(this.btnClose_Click);
//                this.Closing -= new System.ComponentModel.CancelEventHandler(this.StoreCharacteristicMaint_Closing);
//                this.Load -= new System.EventHandler(this.StoreCharacteristicMaint_Load);
//            }
//            base.Dispose( disposing );
//        }

//        private void StoreCharacteristicMaint_Load(object sender, System.EventArgs e)
//        {
//            if (FunctionSecurity.AllowUpdate)
//            {

//                Format_Title(eDataState.Updatable, eMIDTextCode.frm_Administration, MIDText.GetTextOnly(eMIDTextCode.frm_StoreCharacteristicMaint));
//            }
//            else
//            {

//                Format_Title(eDataState.ReadOnly, eMIDTextCode.frm_Administration, MIDText.GetTextOnly(eMIDTextCode.frm_StoreCharacteristicMaint));
//            }

//            SetReadOnly(FunctionSecurity.AllowUpdate);  //Security changes - 1/24/2005 vg

//            // Begin Track #5497 - JSmith - unhandled exception if read only
//            if (!FunctionSecurity.AllowUpdate)
//            {
//                menuInsert.Visible = false;
//                menuDelete.Visible = false;
//            }
//            // End Track #5497

////			SetGridDeleteAccess();

//        }

////		private void SetGridDeleteAccess()
////		{
////			foreach(UltraGridBand ugb in charGrid.DisplayLayout.Bands)
////			{
////				if (!FunctionSecurity.AllowUpdate)
////				{
////					ugb.Override.AllowDelete = DefaultableBoolean.False;
////				}
////				else
////				{
////					ugb.Override.AllowDelete = DefaultableBoolean.True;
////				}
////			}
////		}


//        private void PopulateTypeValueList()
//        {
//            ValueList objValueList = this.charGrid.DisplayLayout.ValueLists.Add(("Types"));
//            objValueList.ValueListItems.Add(0, "Text");
//            objValueList.ValueListItems.Add(1, "Date");
//            objValueList.ValueListItems.Add(2, "Number");
//            objValueList.ValueListItems.Add(3, "Dollar");
//        }

//        private void PopulateHasListValueList()
//        {
//            ValueList objValueList = this.charGrid.DisplayLayout.ValueLists.Add(("HasList"));
//            objValueList.ValueListItems.Add("0", "No");
//            objValueList.ValueListItems.Add("1", "Yes");
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
//            this.charGrid = new Infragistics.Win.UltraWinGrid.UltraGrid();
//            this.menuChar = new System.Windows.Forms.ContextMenu();
//            this.menuInsert = new System.Windows.Forms.MenuItem();
//            this.menuDelete = new System.Windows.Forms.MenuItem();
//            this.menuInUseInfo = new System.Windows.Forms.MenuItem();
//            this.btnSave = new System.Windows.Forms.Button();
//            this.btnClose = new System.Windows.Forms.Button();
//            ((System.ComponentModel.ISupportInitialize)(this.utmMain)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.charGrid)).BeginInit();
//            this.SuspendLayout();
//            // 
//            // utmMain
//            // 
//            this.utmMain.MenuSettings.ForceSerialization = true;
//            this.utmMain.ToolbarSettings.ForceSerialization = true;
//            // 
//            // charGrid
//            // 
//            this.charGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
//            | System.Windows.Forms.AnchorStyles.Left) 
//            | System.Windows.Forms.AnchorStyles.Right)));
//            this.charGrid.ContextMenu = this.menuChar;
//            this.charGrid.DisplayLayout.AddNewBox.Hidden = false;
//            appearance1.BackColor = System.Drawing.Color.White;
//            appearance1.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
//            appearance1.BackGradientStyle = Infragistics.Win.GradientStyle.ForwardDiagonal;
//            this.charGrid.DisplayLayout.Appearance = appearance1;
//            this.charGrid.DisplayLayout.InterBandSpacing = 10;
//            appearance2.BackColor = System.Drawing.Color.Transparent;
//            this.charGrid.DisplayLayout.Override.CardAreaAppearance = appearance2;
//            appearance3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
//            appearance3.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
//            appearance3.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
//            appearance3.ForeColor = System.Drawing.Color.Black;
//            appearance3.TextHAlignAsString = "Left";
//            appearance3.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
//            this.charGrid.DisplayLayout.Override.HeaderAppearance = appearance3;
//            appearance4.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
//            this.charGrid.DisplayLayout.Override.RowAppearance = appearance4;
//            appearance5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
//            appearance5.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
//            appearance5.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
//            this.charGrid.DisplayLayout.Override.RowSelectorAppearance = appearance5;
//            this.charGrid.DisplayLayout.Override.RowSelectorWidth = 12;
//            this.charGrid.DisplayLayout.Override.RowSpacingBefore = 2;
//            appearance6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
//            appearance6.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
//            appearance6.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
//            appearance6.ForeColor = System.Drawing.Color.Black;
//            this.charGrid.DisplayLayout.Override.SelectedRowAppearance = appearance6;
//            this.charGrid.DisplayLayout.RowConnectorColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
//            this.charGrid.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.Solid;
//            this.charGrid.Location = new System.Drawing.Point(16, 6);
//            this.charGrid.Name = "charGrid";
//            this.charGrid.Size = new System.Drawing.Size(476, 315);
//            this.charGrid.TabIndex = 0;
//            this.charGrid.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.charGrid_InitializeLayout);
//            this.charGrid.AfterRowsDeleted += new System.EventHandler(this.charGrid_AfterRowsDeleted);
//            this.charGrid.AfterRowInsert += new Infragistics.Win.UltraWinGrid.RowEventHandler(this.charGrid_AfterRowInsert);
//            this.charGrid.BeforeRowExpanded += new Infragistics.Win.UltraWinGrid.CancelableRowEventHandler(this.charGrid_BeforeRowExpanded);
//            this.charGrid.CellChange += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.charGrid_CellChange);
//            this.charGrid.BeforeCellDeactivate += new System.ComponentModel.CancelEventHandler(this.charGrid_BeforeCellDeactivate);
//            this.charGrid.BeforeEnterEditMode += new System.ComponentModel.CancelEventHandler(this.charGrid_BeforeEnterEditMode);
//            this.charGrid.BeforeExitEditMode += new Infragistics.Win.UltraWinGrid.BeforeExitEditModeEventHandler(this.charGrid_BeforeExitEditMode);
//            this.charGrid.BeforeRowInsert += new Infragistics.Win.UltraWinGrid.BeforeRowInsertEventHandler(this.charGrid_BeforeRowInsert);
//            this.charGrid.BeforeRowsDeleted += new Infragistics.Win.UltraWinGrid.BeforeRowsDeletedEventHandler(this.charGrid_BeforeRowsDeleted);
//            this.charGrid.Click += new System.EventHandler(this.charGrid_Click);
//            this.charGrid.KeyDown += new System.Windows.Forms.KeyEventHandler(this.charGrid_KeyDown);
//            this.charGrid.KeyUp += new System.Windows.Forms.KeyEventHandler(this.charGrid_KeyUp);
//            this.charGrid.MouseDown += new System.Windows.Forms.MouseEventHandler(this.charGrid_MouseDown);
//            this.charGrid.MouseUp += new System.Windows.Forms.MouseEventHandler(this.charGrid_MouseUp);
//            // 
//            // menuChar
//            // 
//            this.menuChar.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
//            this.menuInsert,
//            this.menuDelete,
//            this.menuInUseInfo});
//            // 
//            // menuInsert
//            // 
//            this.menuInsert.Index = 0;
//            this.menuInsert.Text = "Insert";
//            this.menuInsert.Click += new System.EventHandler(this.menuInsert_Click);
//            // 
//            // menuDelete
//            // 
//            this.menuDelete.Index = 1;
//            this.menuDelete.Text = "Delete";
//            this.menuDelete.Click += new System.EventHandler(this.menuDelete_Click);
//            // 
//            // menuInUseInfo
//            // 
//            this.menuInUseInfo.Index = 2;
//            this.menuInUseInfo.Text = "In Use";
//            this.menuInUseInfo.Click += new System.EventHandler(this.menuInUseInfo_Click);
//            // 
//            // btnSave
//            // 
//            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
//            this.btnSave.Location = new System.Drawing.Point(326, 331);
//            this.btnSave.Name = "btnSave";
//            this.btnSave.Size = new System.Drawing.Size(75, 23);
//            this.btnSave.TabIndex = 6;
//            this.btnSave.Text = "Save";
//            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
//            // 
//            // btnClose
//            // 
//            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
//            this.btnClose.Location = new System.Drawing.Point(414, 331);
//            this.btnClose.Name = "btnClose";
//            this.btnClose.Size = new System.Drawing.Size(75, 23);
//            this.btnClose.TabIndex = 7;
//            this.btnClose.Text = "Close";
//            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
//            // 
//            // StoreCharacteristicMaint
//            // 
//            this.AllowDragDrop = true;
//            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
//            this.ClientSize = new System.Drawing.Size(497, 361);
//            this.Controls.Add(this.btnClose);
//            this.Controls.Add(this.btnSave);
//            this.Controls.Add(this.charGrid);
//            this.Name = "StoreCharacteristicMaint";
//            this.Text = "Store Profile Dynamic Characteristics";
//            this.Closing += new System.ComponentModel.CancelEventHandler(this.StoreCharacteristicMaint_Closing);
//            this.Load += new System.EventHandler(this.StoreCharacteristicMaint_Load);
//            this.Controls.SetChildIndex(this.charGrid, 0);
//            this.Controls.SetChildIndex(this.btnSave, 0);
//            this.Controls.SetChildIndex(this.btnClose, 0);
//            ((System.ComponentModel.ISupportInitialize)(this.utmMain)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.charGrid)).EndInit();
//            this.ResumeLayout(false);

//        }

//        #endregion


//        private void charGrid_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
//        {
//            //Begin TT#169 - JSmith - enter max of 1000 and receive 'must be integer'
//            //MIDRetail.Windows.Controls.UltraGridLayoutDefaults ugld = new MIDRetail.Windows.Controls.UltraGridLayoutDefaults();
//            //ugld.ApplyDefaults(e);
//            MIDRetail.Windows.Controls.UltraGridLayoutDefaults ugld = new MIDRetail.Windows.Controls.UltraGridLayoutDefaults(ErrorImage);
//            // Begin TT#1164 - JSmith - When going in to some methods or Admin features the columns are appearing too wide.
//            //ugld.ApplyDefaults((Infragistics.Win.UltraWinGrid.UltraGrid)sender, e, true, 2, true);
//            ugld.ApplyDefaults((Infragistics.Win.UltraWinGrid.UltraGrid)sender, e, true, 2, false);
//            // End TT#1164
//            //End TT#169
//            charGrid.DisplayLayout.GroupByBox.Hidden = true;

//            // Column always hidden
//            charGrid.DisplayLayout.Bands[0].Columns[0].Hidden = true; // RID
//            charGrid.DisplayLayout.Bands[1].Columns[0].Hidden = true; // RID
//            charGrid.DisplayLayout.Bands[1].Columns[1].Hidden = true; // RID
//            //temp
//            //			charGrid.DisplayLayout.Bands[1].Columns[3].Hidden = true;
//            //			charGrid.DisplayLayout.Bands[1].Columns[4].Hidden = true;
//            //			charGrid.DisplayLayout.Bands[1].Columns[5].Hidden = true;
//            this.charGrid.DisplayLayout.Bands[1].Columns["text_value"].Header.Caption = "Text Values";
//            this.charGrid.DisplayLayout.Bands[1].Columns["date_value"].Header.Caption = "Date Values";
//            this.charGrid.DisplayLayout.Bands[1].Columns["number_value"].Header.Caption = "Numeric Values";
//            this.charGrid.DisplayLayout.Bands[1].Columns["dollar_value"].Header.Caption = "Dollar Values";

//            this.charGrid.DisplayLayout.Bands[0].Columns["SCG_ID"].Header.Caption = "Characteristics";
//            this.charGrid.DisplayLayout.Bands[0].Columns["SCG_ID"].Width = 150;
//            this.charGrid.DisplayLayout.Bands[0].Columns["scg_type"].Header.Caption = "Value Type";
//            this.charGrid.DisplayLayout.Bands[0].Columns["scg_type"].Width = 70;
//            this.charGrid.DisplayLayout.Bands[0].Columns["scg_list_ind"].Hidden = true;

//            this.charGrid.DisplayLayout.Bands[0].AddButtonCaption = "Characteristic";
//            this.charGrid.DisplayLayout.Bands[1].AddButtonCaption = "Characteristic Value";
			
//            PopulateTypeValueList();
//            //PopulateHasListValueList();

//            this.charGrid.DisplayLayout.Bands[0].Columns["scg_type"].ValueList = this.charGrid.DisplayLayout.ValueLists["Types"];            
//            this.charGrid.DisplayLayout.Bands[0].Columns["scg_type"].AutoEdit = true;
//            this.charGrid.DisplayLayout.Bands[0].Columns["scg_type"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;

//            //this.charGrid.DisplayLayout.Bands[0].Columns["scg_list_ind"].ValueList = this.charGrid.DisplayLayout.ValueLists["HasList"]; 
//        }

//        private void charGrid_InitializeRow(object sender, InitializeRowEventArgs e)
//        {
		
//        }

//        private void charGrid_Click(object sender, System.EventArgs e)
//        {
			
//        }

//        private void SaveGridState()
//        {
//            if (_htGridState == null)
//                _htGridState = new Hashtable();
//            else
//                _htGridState.Clear();

//            foreach(UltraGridRow row in charGrid.Rows)
//            {
//                int scr_rid = Convert.ToInt32(row.Cells["SCG_RID"].Value, CultureInfo.CurrentUICulture);

//                _htGridState.Add( scr_rid, row.Expanded);
//            }
//        }

//        private void RestoreGridState()
//        {
//            object expanded = null;
//            foreach(UltraGridRow row in charGrid.Rows)
//            {
//                expanded = _htGridState[Convert.ToInt32(row.Cells["SCG_RID"].Value, CultureInfo.CurrentUICulture)];
//                if (expanded == null)
//                    row.Expanded = true;
//                else if ((bool)expanded)
//                    row.Expanded = true;
//            }
//        }


//        private void btnClose_Click(object sender, System.EventArgs e)
//        {
//            //BEGIN TT#496-MD-VStuart-System error when deleting a Store Characteristic
//            //IClose();
//            this.Close();
//            //END  TT#496-MD-VStuart-System error when deleting a Store Characteristic
//        }

//        private void btnSave_Click(object sender, System.EventArgs e)
//        {
//            ISave();
//        }

//        protected override bool SaveChanges()
//        {
//            try
//            {
//                Cursor.Current = Cursors.WaitCursor;
//                SaveGridState();

//                ArrayList newGroupRidList = new ArrayList();

//                //=====================
//                // Do Child deletes
//                //========================================================
//                // Delete individual char values deleted from char groups
//                //=========================================================
//                if (_deletedStoreCharValueKeys.Count > 0)
//                {
//                    _storeSession.DeleteStoreCharValues(_deletedStoreCharValueKeys);
//                    _deletedStoreCharValueKeys.Clear();
//                }
//                //================================================
//                // Delete char values of char groups deleted
//                //================================================
//                foreach (int scgRid in _deleteStoreCharGroupKeys)
//                {
//                    _storeSession.DeleteAllCharValuesFromCharGroup(scgRid);
//                }
////				DataTable xDataTable = _storeCharDataSet.Tables["STORE_CHAR"].GetChanges(DataRowState.Deleted);
////				if (xDataTable != null)
////				{
////					xDataTable = _storeSession.UpdateStoreChar(xDataTable);
////				}

//                //=========================
//                // Do all of parent table
//                //=========================
//                DataTable xDataTable = _storeCharDataSet.Tables["STORE_CHAR_GROUP"].GetChanges();
//                if (xDataTable != null)
//                    xDataTable = _storeSession.UpdateStoreCharGroup(xDataTable);
//                // The xDataTable now contains the NEW Store Char Group RID from the DB for any new rows.
//                // So we search the REAL dataTable until we find a match, and update it's RID.
//                // this update then cascades down to it's children.
//                if (xDataTable != null)
//                {
//                    SaveChangesParentRows(xDataTable, newGroupRidList);
//                }

//                // delete store char column from store profile list and Data Table in session
//                foreach(string name in _deletedStoreCharGroupIDs)
//                {
//                    _storeSession.DeleteStoreCharGroup(name);
//                }
//                _deletedStoreCharGroupIDs.Clear();

//                //====================
//                // Do Child Inserts
//                //====================
//                xDataTable = _storeCharDataSet.Tables["STORE_CHAR"].GetChanges(DataRowState.Added);
//                if (xDataTable != null)
//                {
//                    SaveChangesChildInserts(xDataTable);
//                }
//                //===================
//                // Do Child Updates
//                //===================
//                xDataTable = _storeCharDataSet.Tables["STORE_CHAR"].GetChanges(DataRowState.Modified);
//                if (xDataTable != null)
//                {
//                    SaveChangesChildUpdates(xDataTable);
//                }
//                _storeCharDataSet.AcceptChanges();

//                //===========================
//                // Refresh data in Dataset
//                //===========================
//                charGrid.DataSource = null;
//                charGrid.ResetDisplayLayout();
//                ApplyAppearance(charGrid);
//                charGrid.DisplayLayout.AddNewBox.Hidden = false;
//                charGrid.SetDataBinding(_storeCharDataSet, "");
//                RestoreGridState();

//                if (xDataTable != null)
//                    xDataTable.Dispose();

//                DisableFields();

//                //========================
//                // clear out work areas
//                //========================
//                if (_deletedStoreCharGroupIDs != null)
//                    _deletedStoreCharGroupIDs.Clear();
//                if (_deleteStoreCharGroupKeys != null)
//                    _deleteStoreCharGroupKeys.Clear();
//                if (_deletedStoreCharValueKeys != null)
//                    _deletedStoreCharValueKeys.Clear();

//                //******************************************************************************
//                // Throws event to add new group to Store Explorer for each new Characteristic
//                //******************************************************************************
//                //Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
//                //foreach (int newGroupRid in newGroupRidList)
//                //{
//                //    StoreGroupProfile sg = _storeSession.GetStoreGroup(newGroupRid);
//                //    StoreGroupChangeEventArgs ea = new StoreGroupChangeEventArgs(sg);
//                //    if (OnStoreGroupPropertyChangeHandler != null)
//                //    {
//                //        OnStoreGroupPropertyChangeHandler(this, ea);
//                //    }
//                //    else
//                //    {
//                //        _EAB.StoreGroupExplorer.OnStoreGroupPropertiesChange(sg);

//                //    }
//                //}
//                foreach (StoreGroupNodeDescription newGroupNode in newGroupRidList)
//                {
//                    StoreGroupProfile sg = _storeSession.GetStoreGroup(newGroupNode.StoreGroupRID);

//                    //_EAB.StoreGroupExplorer.AddStoreGroup(sg, newGroupNode.FolderRID);  //TODO -Attribute Set Filter

//                }
//                //End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders

//                ChangePending = false;

//                Cursor.Current = Cursors.Default;
//            }
//            catch (Exception ex)
//            {
//                Cursor.Current = Cursors.Default;
//                this.HandleException(ex);
//            }
//            return true;
//        }

//        private void SaveChangesParentRows(DataTable xDataTable, ArrayList newGroupRidList)
//        {
//            //Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
//            // Begin TT# 166 - stodd
//            //DataTable dtFolders;
//            // Begin TT#166 - JSmith - store char auto add
//            //FolderProfile folderProf = null;
//            int mainGlobalFolderKey = 0;
//            // End TT#166
//            // End TT# 166
//            //End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
//            try
//            {
//                foreach(DataRow xrow in xDataTable.Rows)
//                {
//                    if (xrow.RowState != DataRowState.Deleted)
//                    {
//                        string name = (string)xrow["SCG_ID"];
//                        int charType = Convert.ToInt32( xrow["SCG_TYPE"], CultureInfo.CurrentUICulture );

//                        foreach(DataRow row in _storeCharDataSet.Tables["STORE_CHAR_GROUP"].Rows)
//                        {
//                            if (row.RowState != DataRowState.Deleted)
//                            {
//                                int oldKey = Convert.ToInt32 ( row["SCG_RID"], CultureInfo.CurrentUICulture );
//                                int newKey = Convert.ToInt32 ( xrow["SCG_RID"], CultureInfo.CurrentUICulture );
//                                bool hasList = ((string)xrow["SCG_LIST_IND"] == "1")? true: false;
//                                if (name == (string)row["SCG_ID"] && oldKey < 0) //Add
//                                {
//                                    //==============================================================================
//                                    // Why is this try/catch here?   10/11/2005 stodd
//                                    // For some reason Infragistics occasionally throws an
//                                    // ArgumentOutOfRangeException. Sometimes Infragistics swallows it, but
//                                    // many times he doesn't. It's swallowed here and it doesn't seem to adversly 
//                                    // affect processing.
//                                    //==============================================================================
//                                    try
//                                    {
//                                        row["SCG_RID"] = newKey;
//                                    }
//                                    catch
//                                    {
//                                    }
//                                    _storeSession.AddStoreCharGroup(name, newKey, (eStoreCharType)charType, hasList);

//                                    //****************************************************************************
//                                    // Builds a dynamic group (attr) for the newly created characteristic group
//                                    //****************************************************************************
//                                    // Add Group (attr) for new Characteristic if name doesn't already exist.
//                                    // BEGIN Issue 4410 stodd 09.24.2007
//                                    // Begin Track #4872 - JSmith - Global/User Attributes
//                                    //if (!_storeSession.DoesGroupNameExist(name))
//                                    if (!_storeSession.DoesGroupNameExist(name, Include.GlobalUserRID))
//                                    // End Track #4872
//                                    {
//                                        // Begin TT#166 - JSmith - store char auto add
//                                        //int newGroupRID = _storeSession.AddDynamicGroup(name, newKey, Include.GlobalUserRID, ref folderProf);
//                                        int newGroupRID = _storeSession.AddDynamicGroup(name, newKey, Include.GlobalUserRID, -1, ref mainGlobalFolderKey); //TODO -Attribute Set Filter - make filter
//                                        // End TT#166
//                                        //_storeSession.OpenUpdateConnection();
//                                        //// Begin Track #4872 - JSmith - Global/User Attributes
//                                        ////int newGroupRID = _storeSession.AddGroup(name, true);
//                                        //int newGroupRID = _storeSession.AddGroup(name, true, Include.GlobalUserRID);
//                                        //// End Track #4872
//                                        ////Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
//                                        ////newGroupRidList.Add(newGroupRID);
//                                        ////End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
//                                        //int newGroupLevelRID = _storeSession.AddGroupLevel(newGroupRID, _availableStoresText);
//                                        //// Add dynamic desc for group
//                                        //_storeSession.AddStoreDynamicGroupDesc(newGroupRID, newKey, 1);
//                                        //_storeSession.CommitData();
//                                        //_storeSession.CloseUpdateConnection();
//                                        ////Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders

//                                        //dtFolders = _dlFolder.Folder_Read(Include.GlobalUserRID, eProfileType.StoreGroupMainGlobalFolder);

//                                        //folderProf = new FolderProfile(dtFolders.Rows[0]);

//                                        //_dlFolder.OpenUpdateConnection();

//                                        //try
//                                        //{
//                                        //    _dlFolder.Folder_Item_Insert(folderProf.Key, newGroupRID, eProfileType.StoreGroup);
//                                        //    _dlFolder.CommitData();
//                                        //}
//                                        //catch (Exception exc)
//                                        //{
//                                        //    string message = exc.ToString();
//                                        //    throw;
//                                        //}
//                                        //finally
//                                        //{
//                                        //    _dlFolder.CloseUpdateConnection();
//                                        //}
//                                        // Begin TT#166 - JSmith - store char auto add
//                                        //newGroupRidList.Add(new StoreGroupNodeDescription(newGroupRID, folderProf.Key));
//                                        newGroupRidList.Add(new StoreGroupNodeDescription(newGroupRID, mainGlobalFolderKey));
//                                        // End TT#166
//                                        //End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
//                                    }
//                                    // END Issue 4410 stodd 09.24.2007
//                                }
//                                else if (name == (string)row["SCG_ID"] && oldKey > 0) //update
//                                {
//                                    _storeSession.UpdateStoreCharGroup(name, newKey, hasList);
//                                }
//                            }
//                        }
//                    }
//                }
//                _storeCharDataSet.Tables["STORE_CHAR_GROUP"].AcceptChanges();

//            }
//            catch
//            {
//                throw;
//            }

//        }


//        private void SaveChangesChildInserts(DataTable xDataTable)
//        {
//            try
//            {
//                //updates DB
//                xDataTable = _storeSession.UpdateStoreChar(xDataTable);
//                // The xDataTable now contains the NEW Store Char RID from the DB for any new rows.
//                foreach(DataRow xrow in xDataTable.Rows)
//                {

//                    int scRid = Convert.ToInt32 ( xrow["SC_RID"], CultureInfo.CurrentUICulture );
//                    int scgRid = Convert.ToInt32 ( xrow["SCG_RID"], CultureInfo.CurrentUICulture );

//                    DataRow pRow = _storeCharDataSet.Tables["STORE_CHAR_GROUP"].Rows.Find(scgRid);
//                    eStoreCharType charType = (eStoreCharType)Convert.ToInt32(pRow["SCG_TYPE"], CultureInfo.CurrentUICulture);

//                    object aValue = null;
//                    switch ( charType )
//                    {
//                        case eStoreCharType.text:
//                            aValue = xrow["TEXT_VALUE"];
//                            break;
//                        case eStoreCharType.number:
//                            aValue = xrow["NUMBER_VALUE"];
//                            break;
//                        case eStoreCharType.date:
//                            aValue = xrow["DATE_VALUE"];
//                            break;
//                        case eStoreCharType.dollar:
//                            aValue = xrow["DOLLAR_VALUE"];
//                            break;
//                        default:
//                            aValue = null;
//                            break;
//                    }
//                    // UPdates new store char key in datatable
//                    DataRow [] rows = _storeCharDataSet.Tables["STORE_CHAR_GROUP"].Select("SCG_RID = " + scgRid.ToString(CultureInfo.CurrentUICulture));
//                    DataRow aRow = rows[0];
//                    DataRow [] childRows = aRow.GetChildRows("CharacteristicValue");
//                    bool matchFound = false;
//                    foreach(DataRow childRow in childRows)
//                    {
//                        object scValue = null;
//                        switch ( charType )
//                        {
//                            case eStoreCharType.text:
//                                scValue = childRow["TEXT_VALUE"];
//                                break;
//                            case eStoreCharType.number:
//                                scValue = childRow["NUMBER_VALUE"];
//                                break;
//                            case eStoreCharType.date:
//                                scValue = childRow["DATE_VALUE"];
//                                break;
//                            case eStoreCharType.dollar:
//                                scValue = childRow["DOLLAR_VALUE"];
//                                break;
//                            default:
//                                scValue = null;
//                                break;
//                        }

//                        switch ( charType )
//                        {
//                            case eStoreCharType.text:
//                                string string1 = aValue.ToString();
//                                string string2 = scValue.ToString();
//                                if (string1 == string2)
//                                {
//                                    childRow["SC_RID"] = scRid;
//                                    matchFound = true;
//                                }
//                                break;
//                            case eStoreCharType.number:
//                            case eStoreCharType.dollar:
//                                decimal dec1 = Convert.ToDecimal(aValue, CultureInfo.CurrentUICulture);
//                                decimal dec2 = Convert.ToDecimal(scValue, CultureInfo.CurrentUICulture);
//                                if (dec1 == dec2)
//                                {
//                                    childRow["SC_RID"] = scRid;
//                                    matchFound = true;
//                                }
//                                break;
//                            case eStoreCharType.date:
//                                DateTime date1 = Convert.ToDateTime(aValue, CultureInfo.CurrentUICulture);
//                                DateTime date2 = Convert.ToDateTime(scValue, CultureInfo.CurrentUICulture);
//                                if (date1 == date2)
//                                {
//                                    childRow["SC_RID"] = scRid;
//                                    matchFound = true;
//                                }
//                                break;
//                            default:
//                                break;
//                        }
//                    }

//                    if (!matchFound)
//                    {
//                        MessageBox.Show("Error durring update. No value match found.");
//                    }

//                    // updates session
//                    _storeSession.AddStoreChar(scgRid, scRid, aValue);
//                }
//            }
//            catch
//            {
//                throw;
//            }
//        }

//        private void SaveChangesChildUpdates(DataTable xDataTable)
//        {
//            try
//            {
//                // update DB
//                xDataTable = _storeSession.UpdateStoreChar(xDataTable);
//                foreach(DataRow xrow in xDataTable.Rows)
//                {
//                    int scRid = Convert.ToInt32 ( xrow["SC_RID"], CultureInfo.CurrentUICulture );
//                    int scgRid = Convert.ToInt32 ( xrow["SCG_RID"], CultureInfo.CurrentUICulture );

//                    DataRow pRow = _storeCharDataSet.Tables["STORE_CHAR_GROUP"].Rows.Find(scgRid);
//                    eStoreCharType charType = (eStoreCharType)Convert.ToInt32(pRow["SCG_TYPE"], CultureInfo.CurrentUICulture);

//                    object aValue = null;
//                    switch ( charType )
//                    {
//                        case eStoreCharType.text:
//                            aValue = xrow["TEXT_VALUE"];
//                            break;
//                        case eStoreCharType.number:
//                            aValue = xrow["NUMBER_VALUE"];
//                            break;
//                        case eStoreCharType.date:
//                            aValue = xrow["DATE_VALUE"];
//                            break;
//                        case eStoreCharType.dollar:
//                            aValue = xrow["DOLLAR_VALUE"];
//                            break;
//                        default:
//                            aValue = null;
//                            break;
//                    }
//                    // updates session
//                    _storeSession.UpdateStoreChar(scRid, aValue);
//                }
//            }
//            catch
//            {
//                throw;
//            }
//        }

//        private void DisableFields()
//        {
//            foreach(UltraGridRow row in charGrid.Rows)
//            {
//                eStoreCharType aCharType = (eStoreCharType)Convert.ToInt32( row.Cells["SCG_TYPE"].Value, CultureInfo.CurrentUICulture );

//                UltraGridCell aCell = row.Cells["SCG_TYPE"];
//                aCell.Activation = Activation.NoEdit;

//                if (row.HasChild())
//                {
//                    foreach(UltraGridRow childRow in row.ChildBands[0].Rows)
//                    {
//                        DisableFieldsInRow(childRow,aCharType);
//                    }
//                }
				
//            }
//        }

//        private void DisableFieldsInRow(UltraGridRow childRow, eStoreCharType aCharType)
//        {
//            UltraGridCell aCell = null;

//            switch (aCharType)
//            {
//                case eStoreCharType.text:
//                    aCell = childRow.Cells["DATE_VALUE"];
//                    aCell.Activation = Activation.Disabled;
//                    aCell = childRow.Cells["NUMBER_VALUE"];
//                    aCell.Activation = Activation.Disabled;
//                    aCell = childRow.Cells["DOLLAR_VALUE"];
//                    aCell.Activation = Activation.Disabled;
//                    aCell = childRow.Cells["TEXT_VALUE"];
//                    aCell.Activation = Activation.AllowEdit;
//                    break;
//                case eStoreCharType.date:
//                    aCell = childRow.Cells["TEXT_VALUE"];
//                    aCell.Activation = Activation.Disabled;
//                    aCell = childRow.Cells["NUMBER_VALUE"];
//                    aCell.Activation = Activation.Disabled;
//                    aCell = childRow.Cells["DOLLAR_VALUE"];
//                    aCell.Activation = Activation.Disabled;
//                    aCell = childRow.Cells["DATE_VALUE"];
//                    aCell.Activation = Activation.AllowEdit;
//                    break;
//                case eStoreCharType.number:
//                    aCell = childRow.Cells["TEXT_VALUE"];
//                    aCell.Activation = Activation.Disabled;
//                    aCell = childRow.Cells["DATE_VALUE"];
//                    aCell.Activation = Activation.Disabled;
//                    aCell = childRow.Cells["DOLLAR_VALUE"];
//                    aCell.Activation = Activation.Disabled;
//                    aCell = childRow.Cells["NUMBER_VALUE"];
//                    aCell.Activation = Activation.AllowEdit;

//                    break;
//                case eStoreCharType.dollar:
//                    aCell = childRow.Cells["TEXT_VALUE"];
//                    aCell.Activation = Activation.Disabled;
//                    aCell = childRow.Cells["NUMBER_VALUE"];
//                    aCell.Activation = Activation.Disabled;
//                    aCell = childRow.Cells["DATE_VALUE"];
//                    aCell.Activation = Activation.Disabled;
//                    aCell = childRow.Cells["DOLLAR_VALUE"];
//                    aCell.Activation = Activation.AllowEdit;

//                    break;
//            }
//        }

//        private void charGrid_CellChange(object sender, Infragistics.Win.UltraWinGrid.CellEventArgs e)
//        {
//            try
//            {
//                if (e.Cell.Column.Key == "SCG_TYPE")
//                {
//                    eStoreCharType aCharType = (eStoreCharType)Convert.ToInt32( e.Cell.EditorResolved.Value, CultureInfo.CurrentUICulture );
//                    UltraGridRow row = e.Cell.Row;
//                    if (row.HasChild())
//                    {
//                        foreach(UltraGridRow childRow in row.ChildBands[0].Rows)
//                        {
//                            DisableFieldsInRow(childRow,aCharType);
//                        }
//                    }
//                }
//            }
//            catch
//            {
//                throw;
//            }
//        }

//        #region Validation
//        private bool ValidateCharacteristics()
//        {
//            bool isOk = true;
	
//            foreach(UltraGridRow row in charGrid.Rows)
//            {
//                isOk = ValidateEachCharGroup(row);
//                if (!isOk) break;  // Found error.  Stop processing.
//                int charType = Convert.ToInt32(row.Cells["SCG_TYPE"].Value, CultureInfo.CurrentUICulture);

//                string charName = (string)row.Cells["SCG_ID"].Value;
//                if (row.HasChild())
//                {
//                    foreach (UltraGridRow childRow in row.ChildBands[0].Rows)
//                    {
//                        isOk = ValidateEachCharValue(row, childRow, (eStoreCharType)charType, charName);
//                        if (!isOk) break;  // Found error.  Stop processing.
//                    }
//                }
//                if (!isOk) break;  // Found error.  Stop processing.	
//            }
//            return isOk;
//        }

//        private bool ValidateEachCharGroup(UltraGridRow row)
//        {
//            bool isOk = true;
//            isOk = ValidName(row, row.Cells["SCG_ID"].Value, row.Cells["SCG_ID"].OriginalValue);
//            if (!isOk)
//            {
//                charGrid.ActiveRow = row;
//                row.Expanded = true;
//                charGrid.ActiveCell = row.Cells["SCG_ID"];
//                charGrid.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.EnterEditMode, false, false);
//            }
//            else
//            {
//                if (row.Cells["SCG_TYPE"].Value == System.DBNull.Value)
//                {
//                    charGrid.ActiveRow = row;
//                    row.Expanded = true;
//                    charGrid.ActiveCell = row.Cells["SCG_TYPE"];
//                    charGrid.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.EnterEditMode, false, false);
//                    string errorMessage = _SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ValueRequired);
//                    errorMessage = errorMessage.Replace("{0}","Characterstic Type");
//                    MessageBox.Show (errorMessage,  this.Text, 	MessageBoxButtons.OK, MessageBoxIcon.Error);
//                    isOk = false;
//                }
//            }

//            return isOk; 
//        }

//        private bool ValidateEachCharValue(UltraGridRow parentRow, UltraGridRow row, eStoreCharType aCharType, string charName)
//        {
//            bool isOk = true;

//            switch (aCharType)
//            {
//                case eStoreCharType.date:
//                    isOk = ValidDateValue(row, row.Cells["DATE_VALUE"].Value, charName);
//                    if (isOk)
//                    {
//                        int rowRid = Convert.ToInt32( row.Cells["SC_RID"].Value, CultureInfo.CurrentUICulture );
//                        DateTime rowValue = Convert.ToDateTime( row.Cells["DATE_VALUE"].Value, CultureInfo.CurrentUICulture );
//                        foreach(UltraGridRow childRow in parentRow.ChildBands[0].Rows)
//                        {
//                            int childRowRid = Convert.ToInt32( childRow.Cells["SC_RID"].Value, CultureInfo.CurrentUICulture );
//                            DateTime childRowValue = Convert.ToDateTime( childRow.Cells["DATE_VALUE"].Value, CultureInfo.CurrentUICulture );
//                            if (rowValue == childRowValue)
//                            {
//                                if (rowRid != childRowRid)
//                                {
//                                    string errorMessage = _SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_DuplicateCharValue);
//                                    MessageBox.Show (errorMessage,  this.Text, 	MessageBoxButtons.OK, MessageBoxIcon.Error);
//                                    isOk = false;
//                                    break;
//                                }
//                            }
//                        }
//                    }
//                    if (!isOk)
//                    {
//                        charGrid.ActiveRow = row;
//                        row.Expanded = true;
//                        charGrid.ActiveCell = row.Cells["DATE_VALUE"];
//                        charGrid.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.EnterEditMode, false, false);
//                    }
//                    break;

//                case eStoreCharType.text:
//                    isOk = ValidTextValue(row, row.Cells["TEXT_VALUE"].Value, charName);
//                    if (isOk)
//                    {
//                        int rowRid = Convert.ToInt32( row.Cells["SC_RID"].Value, CultureInfo.CurrentUICulture );
//                        string rowValue = row.Cells["TEXT_VALUE"].Value.ToString();
//                        foreach(UltraGridRow childRow in parentRow.ChildBands[0].Rows)
//                        {
//                            int childRowRid = Convert.ToInt32( childRow.Cells["SC_RID"].Value, CultureInfo.CurrentUICulture );
//                            string childRowValue = childRow.Cells["TEXT_VALUE"].Value.ToString();
//                            if (rowValue == childRowValue)
//                            {
//                                if (rowRid != childRowRid)
//                                {
//                                    string errorMessage = _SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_DuplicateCharValue);
//                                    MessageBox.Show (errorMessage,  this.Text, 	MessageBoxButtons.OK, MessageBoxIcon.Error);
//                                    isOk = false;
//                                    break;
//                                }
//                            }
//                        }
//                    }
//                    if (!isOk)
//                    {
//                        charGrid.ActiveRow = row;
//                        row.Expanded = true;
//                        charGrid.ActiveCell = row.Cells["TEXT_VALUE"];
//                        charGrid.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.EnterEditMode, false, false);
//                    }
//                    break;

//                case eStoreCharType.number:
//                    isOk = ValidNumberValue(row, row.Cells["NUMBER_VALUE"].Value, charName);
//                    if (isOk)
//                    {
//                        int rowRid = Convert.ToInt32( row.Cells["SC_RID"].Value, CultureInfo.CurrentUICulture );
//                        decimal rowValue = Convert.ToDecimal( row.Cells["NUMBER_VALUE"].Value, CultureInfo.CurrentUICulture );
//                        foreach(UltraGridRow childRow in parentRow.ChildBands[0].Rows)
//                        {
//                            int childRowRid = Convert.ToInt32( childRow.Cells["SC_RID"].Value, CultureInfo.CurrentUICulture );
//                            decimal childRowValue = Convert.ToDecimal( childRow.Cells["NUMBER_VALUE"].Value, CultureInfo.CurrentUICulture );
//                            if (rowValue == childRowValue)
//                            {
//                                if (rowRid != childRowRid)
//                                {
//                                    string errorMessage = _SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_DuplicateCharValue);
//                                    MessageBox.Show (errorMessage,  this.Text, 	MessageBoxButtons.OK, MessageBoxIcon.Error);
//                                    isOk = false;
//                                    break;
//                                }
//                            }
//                        }
//                    }
//                    if (!isOk)
//                    {
//                        charGrid.ActiveRow = row;
//                        row.Expanded = true;
//                        charGrid.ActiveCell = row.Cells["NUMBER_VALUE"];
//                        charGrid.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.EnterEditMode, false, false);
//                    }
//                    break;

//                case eStoreCharType.dollar:
//                    isOk = ValidDollarValue(row, row.Cells["DOLLAR_VALUE"].Value, charName);
//                    if (isOk)
//                    {
//                        int rowRid = Convert.ToInt32( row.Cells["SC_RID"].Value, CultureInfo.CurrentUICulture );
//                        decimal rowValue = Convert.ToDecimal( row.Cells["DOLLAR_VALUE"].Value, CultureInfo.CurrentUICulture );
//                        foreach(UltraGridRow childRow in parentRow.ChildBands[0].Rows)
//                        {
//                            int childRowRid = Convert.ToInt32( childRow.Cells["SC_RID"].Value, CultureInfo.CurrentUICulture );
//                            decimal childRowValue = Convert.ToDecimal( childRow.Cells["DOLLAR_VALUE"].Value, CultureInfo.CurrentUICulture );
//                            if (rowValue == childRowValue)
//                            {
//                                if (rowRid != childRowRid)
//                                {
//                                    string errorMessage = _SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_DuplicateCharValue);
//                                    MessageBox.Show (errorMessage,  this.Text, 	MessageBoxButtons.OK, MessageBoxIcon.Error);
//                                    isOk = false;
//                                    break;
//                                }
//                            }
//                        }
//                    }
//                    if (!isOk)
//                    {
//                        charGrid.ActiveRow = row;
//                        row.Expanded = true;
//                        charGrid.ActiveCell = row.Cells["DOLLAR_VALUE"];
//                        charGrid.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.EnterEditMode, false, false);
//                    }
//                    break;
//            }
//            return isOk; 
//        }

//        private bool ValidName(UltraGridRow row, object cellValue, object previousCellValue)
//        {
//            bool valid = true;

//            if (cellValue == System.DBNull.Value)
//            {
//                string errorMessage = _SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ValueRequired);
//                errorMessage = errorMessage.Replace("{0}","Characterstic Name");
//                MessageBox.Show (errorMessage,  this.Text, 	MessageBoxButtons.OK, MessageBoxIcon.Error);
//                valid = false;
//            }
//            else if ((string)cellValue == "")
//            {
//                string errorMessage = _SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ValueRequired);
//                errorMessage = errorMessage.Replace("{0}","Characterstic Name");
//                MessageBox.Show (errorMessage,  this.Text, 	MessageBoxButtons.OK, MessageBoxIcon.Error);
//                valid = false;
//            }

//            if (valid)
//            {
//                if (_reserverWords.Contains(((string)cellValue).ToUpper(CultureInfo.CurrentUICulture)))
//                {
					
//                    string errorMessage = _SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MatchesReserveWord);
//                    errorMessage = errorMessage.Replace("{0}", cellValue.ToString());
//                    //string errorMessage = _SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_DuplicateCharGroupName);
//                    MessageBox.Show (errorMessage,  this.Text, 	MessageBoxButtons.OK, MessageBoxIcon.Error);
//                    valid = false;
//                }
//            }

//            //============================================================================
//            // tests to see if char group name matches any other char group name in list.
//            // saved and unsaved.  Loops through backwards.
//            //============================================================================
//            // Begin TT#471 - stodd - duplicate char name error
//            int gridCharGroupRid = Include.NoRID;
//            string gridCharGroupName = string.Empty;
//            int charGroupRid = Convert.ToInt32(row.Cells["SCG_RID"].Value, CultureInfo.CurrentUICulture);
//            string charGroupName = (string)row.Cells["SCG_ID"].Value;
//            int count = charGrid.Rows.Count;	
//            // End TT#471 - stodd - duplicate char name error
//            if (valid)
//            {
//                for (int i=0;i<count;i++)
//                {
//                    UltraGridRow gridRow =  charGrid.Rows[i];
//                    gridCharGroupRid = Convert.ToInt32( gridRow.Cells["SCG_RID"].Value, CultureInfo.CurrentUICulture );
//                    gridCharGroupName = (string) gridRow.Cells["SCG_ID"].Value ;
//                    if (gridCharGroupName.ToUpper() == charGroupName.ToUpper())
//                    {
//                        if (gridCharGroupRid != charGroupRid)
//                        {
//                            string errorMessage = _SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_DuplicateCharGroupName);
//                            MessageBox.Show (errorMessage,  this.Text, 	MessageBoxButtons.OK, MessageBoxIcon.Error);
//                            valid = false;
//                            break;
//                        }
//                    }
//                }
//            }

//            if (valid)
//            {
//                // Begin TT#471 - stodd - duplicate char name error
//                // On the off chance there's something in the store session that's not in the grid,
//                // we check the store service store char group list. If a mismatch is found, the rid
//                // is checked to see if it's already in the grid. If it's not in the grid, then it's a real mismatch.
//                int matchGroupRid = _storeSession.StoreCharGroupExists((string)cellValue);

//                if (matchGroupRid > 0 && matchGroupRid != charGroupRid)
//                {
//                    bool matchFound = false;
//                    for (int i = 0; i < count; i++)
//                    {
//                        UltraGridRow gridRow = charGrid.Rows[i];
//                        gridCharGroupRid = Convert.ToInt32(gridRow.Cells["SCG_RID"].Value, CultureInfo.CurrentUICulture);
//                        // Find the match group rid in the current grid.
//                        if (gridCharGroupRid == matchGroupRid)
//                        {
//                            matchFound = true;
//                            break;
//                        }
//                    }
//                    if (!matchFound)
//                    {
//                        string errorMessage = _SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_DuplicateCharGroupName);
//                        MessageBox.Show(errorMessage, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
//                        valid = false;
//                    }
//                }
//                // End TT#471 - stodd - duplicate char name error
//            }

//            // since when this Store Characteristic is saved it will become a dynamic attribute of the same name,
//            // we check to be sure it's a valid Store Group/attribute name

//            if (valid)
//            {
//                if (row.Cells["SCG_ID"].DataChanged && (string)cellValue != (string)row.Cells["SCG_ID"].OriginalValue)
//                {
//                    // Begin Track #4872 - JSmith - Global/User Attributes
//                    //bool groupExists = _storeSession.DoesGroupNameExist((String)cellValue);
//                    bool groupExists = _storeSession.DoesGroupNameExist((String)cellValue, Include.GlobalUserRID);
//                    // End Track #4872
//                    if (groupExists)
//                    {
//                        string errorMessage = _SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_CharNameAttrNameConflict);
//                        MessageBox.Show (errorMessage,  this.Text, 	MessageBoxButtons.OK, MessageBoxIcon.Error);
//                        valid = false;
//                    }
//                }
//            }


//            return valid;
//        }

//        private bool ValidCharType(UltraGridRow row, object cellValue)
//        {
//            bool valid = true;

//            if (cellValue == System.DBNull.Value )
//            {
//                string errorMessage = _SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ValueRequired);
//                errorMessage = errorMessage.Replace("{0}","Characteristic Type");
//                MessageBox.Show (errorMessage,  this.Text, 	MessageBoxButtons.OK, MessageBoxIcon.Error);
//                valid = false;
//            }
//            return valid;
//        }

//        private bool ValidDateValue(UltraGridRow row, object cellValue, string charName)
//        {
//            bool valid = true;

//            if (cellValue == System.DBNull.Value )
//            {
//                string errorMessage = _SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ValueRequired);
//                errorMessage = errorMessage.Replace("{0}","Date Value");
//                MessageBox.Show (errorMessage,  this.Text, 	MessageBoxButtons.OK, MessageBoxIcon.Error);
//                valid = false;
//            }

//            if (valid)
//            {
//                //				int dYear = Convert.ToInt32( cellValue.ToString().Substring(6,4) );
//                //				int dDay = Convert.ToInt32( cellValue.ToString().Substring(3,2) );
//                //				int dMonth = Convert.ToInt32( cellValue.ToString().Substring(0,2) );
//                //				DateTime aDate = new DateTime(dYear, dMonth, dDay);
//                int charGroupRid = _storeSession.StoreCharGroupExists(charName);
//                // if the charGroupRid = 0, then this is a new char group, so the 
//                // char value can't exist
//                if (charGroupRid > 0)  
//                {
//                    int rowRid = Convert.ToInt32( row.Cells["SC_RID"].Value, CultureInfo.CurrentUICulture );
//                    int rid = _storeSession.StoreCharExists(charGroupRid, cellValue);

//                    if (rid > 0 && rowRid != rid)
//                    {
//                        string errorMessage = _SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_DuplicateCharValue);
//                        MessageBox.Show (errorMessage,  this.Text, 	MessageBoxButtons.OK, MessageBoxIcon.Error);
//                        valid = false;
//                    }
//                }
//            }
//            return valid;
//        }

//        private bool ValidTextValue(UltraGridRow row, object cellValue, string charName)
//        {
//            bool valid = true;

//            if (cellValue == System.DBNull.Value )
//            {
//                string errorMessage = _SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ValueRequired);
//                errorMessage = errorMessage.Replace("{0}","Text Value");
//                MessageBox.Show (errorMessage,  this.Text, 	MessageBoxButtons.OK, MessageBoxIcon.Error);
//                valid = false;
//            }

//            if (valid)
//            {
//                int charGroupRid = _storeSession.StoreCharGroupExists(charName);
//                // if the charGroupRid = 0, then this is a new char group, so the 
//                // char value can't exist
//                if (charGroupRid > 0)  
//                {
//                    int rowRid = Convert.ToInt32( row.Cells["SC_RID"].Value, CultureInfo.CurrentUICulture );
//                    int rid = _storeSession.StoreCharExists(charGroupRid, cellValue);

//                    if (rid > 0 && rowRid != rid)
//                    {
//                        string errorMessage = _SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_DuplicateCharValue);
//                        MessageBox.Show (errorMessage,  this.Text, 	MessageBoxButtons.OK, MessageBoxIcon.Error);
//                        valid = false;
//                    }
//                }
//            }
//            return valid;
//        }

//        private bool ValidNumberValue(UltraGridRow row, object cellValue, string charName)
//        {
//            bool valid = true;

//            if (cellValue == System.DBNull.Value )
//            {
//                string errorMessage = _SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ValueRequired);
//                errorMessage = errorMessage.Replace("{0}","Number Value");
//                MessageBox.Show (errorMessage,  this.Text, 	MessageBoxButtons.OK, MessageBoxIcon.Error);
//                valid = false;
//            }

//            if (valid)
//            {
//                try 
//                {
//                    Convert.ToDouble(cellValue);
//                }
//                catch (FormatException) 
//                {
//                    string errorMessage = _SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_NumericValuesOnly);
//                    MessageBox.Show (errorMessage,  this.Text, 	MessageBoxButtons.OK, MessageBoxIcon.Error);
//                    valid = false;
//                }
//            }

//            if (valid)
//            {
//                int charGroupRid = _storeSession.StoreCharGroupExists(charName);
//                // if the charGroupRid = 0, then this is a new char group, so the 
//                // char value can't exist
//                if (charGroupRid > 0)  
//                {
//                    decimal aCellValue = Convert.ToDecimal(cellValue, CultureInfo.CurrentUICulture);
//                    int rowRid = Convert.ToInt32( row.Cells["SC_RID"].Value, CultureInfo.CurrentUICulture );
//                    int rid = _storeSession.StoreCharExists(charGroupRid, (object)aCellValue);

//                    if (rid > 0 && rowRid != rid)
//                    {
//                        string errorMessage = _SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_DuplicateCharValue);
//                        MessageBox.Show (errorMessage,  this.Text, 	MessageBoxButtons.OK, MessageBoxIcon.Error);
//                        valid = false;
//                    }
//                }
//            }
//            return valid;
//        }

//        private bool ValidDollarValue(UltraGridRow row, object cellValue, string charName)
//        {
//            bool valid = true;

//            if (cellValue == System.DBNull.Value )
//            {
//                string errorMessage = _SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ValueRequired);
//                errorMessage = errorMessage.Replace("{0}","Dollar Value");
//                MessageBox.Show (errorMessage,  this.Text, 	MessageBoxButtons.OK, MessageBoxIcon.Error);
//                valid = false;
//            }

//            if (valid)
//            {
//                try 
//                {
//                    Convert.ToDouble(cellValue);
//                }
//                catch (FormatException) 
//                {
//                    string errorMessage = _SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_NumericValuesOnly);
//                    MessageBox.Show (errorMessage,  this.Text, 	MessageBoxButtons.OK, MessageBoxIcon.Error);
//                    valid = false;
//                }
//            }

//            if (valid)
//            {
//                int charGroupRid = _storeSession.StoreCharGroupExists(charName);
//                // if the charGroupRid = 0, then this is a new char group, so the 
//                // char value can't exist
//                if (charGroupRid > 0)  
//                {
//                    decimal aCellValue = Convert.ToDecimal(cellValue, CultureInfo.CurrentUICulture);
//                    int rowRid = Convert.ToInt32( row.Cells["SC_RID"].Value, CultureInfo.CurrentUICulture);
//                    int rid = _storeSession.StoreCharExists(charGroupRid, (object)aCellValue);

//                    if (rid > 0 && rowRid != rid)
//                    {
//                        string errorMessage = _SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_DuplicateCharValue);
//                        MessageBox.Show (errorMessage,  this.Text, 	MessageBoxButtons.OK, MessageBoxIcon.Error);
//                        valid = false;
//                    }
//                }
//            }
//            return valid;
//        }
//        #endregion

//        private void charGrid_AfterRowInsert(object sender, Infragistics.Win.UltraWinGrid.RowEventArgs e)
//        {
//            try
//            {
//                if (e.Row.Band.Index == 1)
//                {
//                    charGrid.BeginUpdate();
//                    charGrid.SuspendRowSynchronization();

//                    // set dummy key in new row
//                    e.Row.Cells["SC_RID"].Value = _dummyKey--;

//                    // I discovered that just updating the GRID with the corect value wasn't enough for the value to
//                    // make it to the DB in some cases.
//                    // So per Infragistics documentation, I search for the correct row in the datatable and update it
//                    // instead.
//                    UltraGridRow parentGridRow = e.Row.ParentRow;
//                    //object [] parentKey = new object[1];
//                    //object parentKey = parentGridRow.Cells["SCG_RID"].Value;
//                    //DataRow parentDBRow = _storeCharDataSet.Tables["STORE_CHAR_GROUP"].Rows.Find(parentKey);
//                    //if (parentDBRow != null)
//                    //{
//                    //    parentDBRow["SCG_LIST_IND"] = "1";
//                    //}
//                    // set dummy key in new row
//                    //e.Row.Cells["SC_RID"].Value = _dummyKey--;

//                    if (parentGridRow != null)
//                    {
//                        parentGridRow.Cells["SCG_LIST_IND"].Value = "1";
//                    }

//                    int charType = Convert.ToInt32( parentGridRow.Cells["SCG_TYPE"].Value, CultureInfo.CurrentUICulture );
//                    switch ((eStoreCharType)charType)
//                    {
//                        case eStoreCharType.date:
//                            DisableFieldsInRow(e.Row, eStoreCharType.date);
//                            charGrid.ActiveCell = e.Row.Cells["DATE_VALUE"];
//                            charGrid.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.EnterEditMode, false, false);
//                            break;
//                        case eStoreCharType.text:
//                            DisableFieldsInRow(e.Row, eStoreCharType.text);
//                            charGrid.ActiveCell = e.Row.Cells["TEXT_VALUE"];
//                            charGrid.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.EnterEditMode, false, false);
//                            break;
//                        case eStoreCharType.number:
//                            DisableFieldsInRow(e.Row, eStoreCharType.number);
//                            charGrid.ActiveCell = e.Row.Cells["NUMBER_VALUE"];
//                            charGrid.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.EnterEditMode, false, false);
//                            break;
//                        case eStoreCharType.dollar:
//                            DisableFieldsInRow(e.Row, eStoreCharType.dollar);
//                            charGrid.ActiveCell = e.Row.Cells["DOLLAR_VALUE"];
//                            charGrid.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.EnterEditMode, false, false);
//                            break;
//                    }

//                    charGrid.ResumeRowSynchronization();
//                    charGrid.EndUpdate();
//                }
//                else
//                {
//                    e.Row.Cells["SCG_RID"].Value = _dummyKey--;
//                    e.Row.Cells["SCG_LIST_IND"].Value = "0";
//                }
//                ChangePending = true;
//            }
//            catch 
//            {
//                throw;
//            }
			
//        }

//        private void charGrid_BeforeRowInsert(object sender, Infragistics.Win.UltraWinGrid.BeforeRowInsertEventArgs e)
//        {
//            try
//            {
//                UltraGridRow parentRow = e.ParentRow;
			
//                if (parentRow != null)
//                {

//                    if (parentRow.Cells["SCG_TYPE"].Value == DBNull.Value || parentRow.Cells["SCG_ID"].Value == DBNull.Value)
//                    {
//                        e.Cancel = true; 
//                        MessageBox.Show(_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_IncompleteCharacteristicGroup),
//                            this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
//                    }

//                }
//            }
//            catch
//            {
//                throw;
//            }
//        }

//        private void charGrid_BeforeEnterEditMode(object sender, System.ComponentModel.CancelEventArgs e)
//        {

//        }

//        private void charGrid_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
//        {
//            try
//            {
//                _point = new Point(e.X, e.Y);
//            }
//            catch
//            {
//                throw;
//            }
//        }

//        private void charGrid_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
//        {
//            try
//            {
//                _point.X = 0;
//                _point.Y = 0;
//            }
//            catch
//            {
//                throw;
//            }
//        }

//        private void StoreCharacteristicMaint_Closing(object sender, System.ComponentModel.CancelEventArgs e)
//        {
		    
//        }

//        private void charGrid_BeforeRowExpanded(object sender, Infragistics.Win.UltraWinGrid.CancelableRowEventArgs e)
//        {
//            try
//            {
//                charGrid.ActiveRow = e.Row;
//            }
//            catch
//            {
//                throw;
//            }
//        }

//        private void charGrid_BeforeExitEditMode(object sender, Infragistics.Win.UltraWinGrid.BeforeExitEditModeEventArgs e)
//        {
//            try
//            {
//                string charName;
//                switch (this.charGrid.ActiveCell.Column.Key)
//                {
//                    case "TEXT_VALUE":
//                        charName = (string)charGrid.ActiveRow.ParentRow.Cells["SCG_ID"].Value;
//                        if (!ValidTextValue(charGrid.ActiveRow, charGrid.ActiveCell.Text, charName))
//                        {
//                            e.Cancel = true;
//                            _invalidCell = charGrid.ActiveCell;
//                        }
//                        break;
//                    case "DATE_VALUE":
//                        charName = (string)charGrid.ActiveRow.ParentRow.Cells["SCG_ID"].Value;
//                        //BEGIN TT#496-MD-VStuart-System error when deleting a Store Characteristic
//                        object aValue;
//                        if (charGrid.ActiveCell.Text == "__/__/____" || charGrid.ActiveCell.Text == string.Empty)
//                        {
//                            aValue = null;
//                        }
//                        else
//                        {
//                            int dYear = Convert.ToInt32(charGrid.ActiveCell.Text.Substring(6, 4), CultureInfo.CurrentUICulture);
//                            int dDay = Convert.ToInt32(charGrid.ActiveCell.Text.Substring(3, 2), CultureInfo.CurrentUICulture);
//                            int dMonth = Convert.ToInt32(charGrid.ActiveCell.Text.Substring(0, 2), CultureInfo.CurrentUICulture);
//                            DateTime aDate = new DateTime(dYear, dMonth, dDay);
//                            aValue = aDate;
//                        }
//                        //END  TT#496-MD-VStuart-System error when deleting a Store Characteristic
//                        if (!ValidDateValue(charGrid.ActiveRow, aValue, charName))
//                        {
//                            e.Cancel = true;
//                            _invalidCell = charGrid.ActiveCell;
//                        }
//                        break;
//                    case "NUMBER_VALUE":
//                        charName = (string)charGrid.ActiveRow.ParentRow.Cells["SCG_ID"].Value;
//                        if (!ValidNumberValue(charGrid.ActiveRow, charGrid.ActiveCell.Text, charName))
//                        {
//                            e.Cancel = true;
//                            _invalidCell = charGrid.ActiveCell;
//                        }
//                        break;
//                    case "DOLLAR_VALUE":
//                        charName = (string)charGrid.ActiveRow.ParentRow.Cells["SCG_ID"].Value;
//                        if (!ValidDollarValue(charGrid.ActiveRow, charGrid.ActiveCell.Text, charName))
//                        {
//                            e.Cancel = true;
//                            _invalidCell = charGrid.ActiveCell;
//                        }
//                        break;
//                }
//                ChangePending = true;
//            }
//            catch
//            {
//                throw;
//            }
//        }

//        private void charGrid_BeforeCellDeactivate(object sender, System.ComponentModel.CancelEventArgs e)
//        {
//            try
//            {
//                if (_invalidCell != null)
//                {
//                    e.Cancel = true;
//                    charGrid.ActiveCell = _invalidCell;
//                    charGrid.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.EnterEditMode, false, false);
//                    _invalidCell = null;
//                }
//            }
//            catch
//            {
//                throw;
//            }
//        }

//        private void charGrid_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
//        {
//            try
//            {
//                if (_invalidCell != null)
//                {
//                    e.Handled = true;
//                    charGrid.ActiveCell = _invalidCell;
//                    charGrid.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.EnterEditMode, false, false);
//                    _invalidCell = null;
//                }
//            }
//            catch
//            {
//                throw;
//            }
//        }

//        private void charGrid_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
//        {
//            try
//            {
//                if (_invalidCell != null)
//                {
//                    charGrid.ActiveCell = _invalidCell;
//                    charGrid.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.EnterEditMode, false, false);
//                    _invalidCell = null;
//                }
//            }
//            catch
//            {
//                throw;
//            }
//        }

//        private void charGrid_AfterRowsDeleted(object sender, System.EventArgs e)
//        {
//            try
//            {
//                ChangePending = true;
//                // This catches if all child rows are deleted from a parent, the list ind needs to be
//                // set back to "0".
//                foreach (DataRow row in _storeCharDataSet.Tables["STORE_CHAR_GROUP"].Rows)
//                {
//                    if (row.RowState != DataRowState.Deleted) // don't check parent rows we are deleting
//                    {
//                        DataRow [] childRows = row.GetChildRows("CharacteristicValue");
//                        if (childRows.Length == 0)
//                        {
//                            if ((string)row["SCG_LIST_IND"] != "0")
//                                row["SCG_LIST_IND"] = "0";
//                        }
//                    }
//                }	
//            }
//            catch
//            {
//                throw;
//            }
//        }

//        private void charGrid_BeforeRowsDeleted(object sender, Infragistics.Win.UltraWinGrid.BeforeRowsDeletedEventArgs e)
//        {
//            try
//            {
//                foreach(UltraGridRow row in e.Rows)
//                {
//                    if (row.Band == charGrid.DisplayLayout.Bands[0])  // Parent Row
//                    {
//                        if (!FunctionSecurity.AllowDelete)
//                        {
//                            e.Cancel = true;
//                            return;
//                        }
//                    }
//                    else if (row.Band == charGrid.DisplayLayout.Bands[1])  // child Row
//                    {
//                        if (!FunctionSecurity.AllowUpdate)
//                        {
//                            e.Cancel = true;
//                            return;
//                        }
//                    }
//                }

//                if (_deletedStoreCharGroupIDs == null)
//                    _deletedStoreCharGroupIDs = new ArrayList();
//                if (_deleteStoreCharGroupKeys == null)
//                    _deleteStoreCharGroupKeys = new ArrayList();
//                if (_deletedStoreCharValueKeys == null)
//                    _deletedStoreCharValueKeys = new ArrayList();

//                //BEGIN TT#110-MD-VStuart - In Use Tool
//                //Changes the delete prompt to occur before the InUse check.
//                e.DisplayPromptMsg = false;
//                string message = string.Format(MIDText.GetTextOnly(eMIDTextCode.msg_DeleteRows), e.Rows.Length);
//                message += Environment.NewLine + MIDText.GetTextOnly((int)eMIDTextCode.msg_ContinueQuestion);
//                string lblDeleteRow = MIDText.GetTextOnly(eMIDTextCode.lbl_DeleteRow);
//                DialogResult diagResult = MessageBox.Show(message, lblDeleteRow, System.Windows.Forms.MessageBoxButtons.YesNo,
//                    System.Windows.Forms.MessageBoxIcon.Question);

//                if (diagResult == System.Windows.Forms.DialogResult.No)
//                {
//                    e.Cancel = true;
//                    return;
//                }
//                _ridList = new ArrayList();
//                //END TT#110-MD-VStuart - In Use Tool

//                //BEGIN TT#3813-VStuart-Object Reference Error when saving Store Profile-MID
//                foreach (UltraGridRow row in e.Rows)
//                {
//                    if (row.Band == charGrid.DisplayLayout.Bands[0]) // Parent Row
//                    {
//                        int scg_rid = Convert.ToInt32(row.Cells["SCG_RID"].Value, CultureInfo.CurrentUICulture);
//                        if (scg_rid > 0)
//                        {
//                            _ridList.Add(scg_rid);
//                        }

//                       if (!_skipInUse)
//                        {
//                            inQuiry = false;
//                            menuInUseInfo_Click(sender, eProfileType.StoreCharacteristics, scg_rid);
//                            inQuiry = true;
//                          if (_display)
//                            {
//                                _deletedStoreCharValueKeys.Clear();
//                                e.Cancel = true;
//                            }
//                          else
//                            {
//                                if (scg_rid > 0)
//                                {
//                                    _deleteStoreCharGroupKeys.Add(scg_rid);
//                                }

//                                if (row.Cells["SCG_ID"].Value != DBNull.Value)
//                                {
//                                    string scg_id = (string)(row.Cells["SCG_ID"].Value);
//                                    _deletedStoreCharGroupIDs.Add(scg_id);
//                                }
//                            }
//                        }
//                        _skipInUse = false;
//                    }
//                    else if (row.Band == charGrid.DisplayLayout.Bands[1]) // child Row
//                    {
//                        int sc_rid = Convert.ToInt32(row.Cells["SC_RID"].Value, CultureInfo.CurrentUICulture);
//                        if (sc_rid > 0)
//                        {
//                            _ridList.Add(sc_rid);
//                        }
//                        if (!_skipInUse)
//                        {
//                            inQuiry = false;
//                            menuInUseInfo_Click(sender, eProfileType.StoreCharacteristicValues, sc_rid);
//                            inQuiry = true;

//                            if (_display)
//                            {
//                                _deletedStoreCharValueKeys.Clear();
//                                e.Cancel = true;
//                            }
//                            else
//                            {
//                                if (sc_rid > 0)
//                                {
//                                    _deletedStoreCharValueKeys.Add(sc_rid);
//                                }
//                            }
//                        }
//                        _skipInUse = false;
//                    }
//                }
//                //END TT#3813-VStuart-Object Reference Error when saving Store Profile-MID

//                //string msgText = string.Empty;
//                //foreach(UltraGridRow row in e.Rows)
//                //{
//                //    if (row.Band == charGrid.DisplayLayout.Bands[0])  // Parent Row
//                //    {
//                //        int scg_rid = Convert.ToInt32(row.Cells["SCG_RID"].Value, CultureInfo.CurrentUICulture);
//                //        if (scg_rid > 0)
//                //        {
//                //            //BEGIN TT#110-MD-VStuart - In Use Tool
//                //            _ridList.Add(scg_rid);
//                //            //END TT#110-MD-VStuart - In Use Tool

//                //    //        string scgName = _storeSession.GetStoreCharacteristicGroupID(scg_rid);

//                //    //        //string[] captions = new string[] { "Attributes", "Attribute Sets" };
//                //    //        InUseInfo inUseInfo = new InUseInfo("Attributes,Attribute Sets");
//                //    //        //// Begin TT#110-MD - RMatelic - In Use Tool >>>>> this may be a temporary change
//                //    //        //inUseInfo.ItemRID = scg_rid;
//                //    //        inUseInfo.ItemName = scgName;
//                //    //        //inUseInfo.ItemProfileType = eProfileType.StoreCharacteristics;
//                //    //        // End TT#tt#110-MD
//                //    //        if (_storeSession.IsStoreCharGroupUsedAnywhere(scg_rid, ref inUseInfo))
//                //    //        {
//                //    //            charGrid.ActiveRow = row;
//                //    //            //msgText = _SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_StoreCharacteristicInUse);
//                //    //            //msgText = msgText.Replace("{0}", scgName);

//                //    //            // Begin TT#110-MD - RMatelic - In Use Tool >>>>> this may be a temporary change
//                //    //            //InUseDialog frm = new InUseDialog("Characteristic cannot be removed.", msgText, inUseInfo);
//                //    //            //InUseDialog frm = new InUseDialog(inUseInfo);
//                //    //            // End TT#110-MD

//                //    //            //BEGIN TT#496-MD-VStuart-System error when deleting a Store Characteristic
//                //    //            //frm.ShowDialog();
//                //    //            //if (frm.ShowDialog() == DialogResult.OK)
//                //    //            //{
//                //    //            //    e.Cell.Row.Cells["Notes"].Value = frm.TextValue;
//                //    //            //    ap.AllocationNotes = frm.TextValue;
//                //    //            //    //ChangePending = true;
//                //    //            //}
//                //    //            //frm.Dispose();
//                //    //            //END  TT#496-MD-VStuart-System error when deleting a Store Characteristic

//                //    //            //e.Cancel = true;
//                //    //            //break;    //TT#496-MD-VStuart-System error when deleting a Store Characteristic
//                //    //        }
//                //    //        if (row.ChildBands[0].Rows.Count > 0)
//                //    //        {
//                //    //            if (_storeSession.DoesStoreCharGroupHaveStoreValuesAssigned(scg_rid))
//                //    //            {
//                //    //                //msgText = _SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ValuesExisitForStoreChar);
//                //    //                //msgText = msgText.Replace("{0}", scgName);
//                //    //                //BEGIN TT#496-MD-VStuart-System error when deleting a Store Characteristic
//                //    //                //if (MessageBox.Show(msgText, this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question)
//                //    //                //    == DialogResult.Yes)
//                //    //                //{
//                //    //                //    //foreach (UltraGridRow childRow in row.ChildBands[0].Rows)
//                //    //                //    //{
//                //    //                //    //    int sc_rid = Convert.ToInt32(childRow.Cells["SC_RID"].Value, CultureInfo.CurrentUICulture);
//                //    //                //    //    if (sc_rid > 0)
//                //    //                //    //        _deletedStoreCharValueKeys.Add(sc_rid);
//                //    //                //    //}
//                //    //                //}
//                //    //                //else
//                //    //                //{
//                //    //                //    e.Cancel = true;
//                //    //                //    break;
//                //    //                //}
//                //    //                //END  TT#496-MD-VStuart-System error when deleting a Store Characteristic
//                //    //            }
//                //    //        }
//                //    //        else
//                //    //        {
//                //    //            //============================================================================================
//                //    //            // If the Char Group has values assigned to stores, let the user know this and give them a
//                //    //            // chance to abort the delete.  Otherwise we add it to a preDelete list.
//                //    //            // The adapter will take care of child rows in 'lists'.  But these are not in a list and 
//                //    //            // need special processing.
//                //    //            //============================================================================================
//                //    //            if (_storeSession.DoesStoreCharGroupHaveStoreValuesAssigned(scg_rid))
//                //    //            {
//                //    //                //msgText = _SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ValuesExisitForStoreChar);
//                //    //                //msgText = msgText.Replace("{0}", scgName);
//                //    //                ////BEGIN TT#496-MD-VStuart-System error when deleting a Store Characteristic
//                //    //                //if (MessageBox.Show(msgText, this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question)
//                //    //                //    == DialogResult.Yes)
//                //    //                //{
//                //    //                //    //if (scg_rid > 0)
//                //    //                //    //    _deleteStoreCharGroupKeys.Add(scg_rid);
//                //    //                //}
//                //    //                //else
//                //    //                //{
//                //    //                //    e.Cancel = true;
//                //    //                //    break;
//                //    //                //}
//                //    //                //END  TT#496-MD-VStuart-System error when deleting a Store Characteristic
//                //    //            }
//                //    //        }

//                //    //        //if (scg_rid > 0)
//                //    //        ////BEGIN TT#496-MD-VStuart-System error when deleting a Store Characteristic
//                //    //        //{
//                //    //        //    //_deleteStoreCharGroupKeys.Add(scg_rid);
//                //    //        //    _ridList.Add(scg_rid);
//                //    //        //}
//                //    //        //END  TT#496-MD-VStuart-System error when deleting a Store Characteristic

//                //    //        //if (row.Cells["SCG_ID"].Value != DBNull.Value)
//                //    //        //{
//                //    //        //    string scg_id = (string)(row.Cells["SCG_ID"].Value);
//                //    //        //    _deletedStoreCharGroupIDs.Add(scg_id);
//                //    //        //}
//                //        }

//                //    }
//                //    else if (row.Band == charGrid.DisplayLayout.Bands[1])  // child Row
//                //    {
//                //        int sc_rid = Convert.ToInt32(row.Cells["SC_RID"].Value, CultureInfo.CurrentUICulture);
//                //        if (sc_rid > 0)
//                //        {
//                //            //if (_storeSession.IsStoreCharUsedAnywhere(sc_rid))
//                //            //{
//                //                //object aValue = _storeSession.GetStoreCharacteristicValue(sc_rid);
//                //                //charGrid.ActiveRow = row;
//                //                //msgText = _SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ValuesExistForDelete);
//                //                //msgText = msgText.Replace("{0}", aValue.ToString());
//                //                //if (MessageBox.Show(msgText, this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question)
//                //                //    == DialogResult.Yes) 
//                //                //{
//                //                    if (sc_rid > 0)
//                //                    //BEGIN TT#496-MD-VStuart-System error when deleting a Store Characteristic
//                //                    {
//                //                        //_deletedStoreCharValueKeys.Add(sc_rid);
//                //                        _ridList.Add(sc_rid);
//                //                    }
//                //                    //END TT#496-MD-VStuart-System error when deleting a Store Characteristic
//                //            //    }
//                //            //    else
//                //            //    {
//                //            //        //BEGIN TT#667-MD-VStuart-In Use Error in Store Characteristics upon aborted delete of Child Record
//                //            //        _skipInUse = true;
//                //            //        //END   TT#667-MD-VStuart-In Use Error in Store Characteristics upon aborted delete of Child Record
//                //            //        e.Cancel = true;
//                //            //        break;
//                //            //    }
//                //            //}
//                //            //else
//                //            //{
//                //                if (sc_rid > 0)
//                //                //BEGIN TT#496-MD-VStuart-System error when deleting a Store Characteristic
//                //                {
//                //                    _deletedStoreCharValueKeys.Add(sc_rid);
//                //                    _ridList.Add(sc_rid);
//                //                }
//                //                //END TT#496-MD-VStuart-System error when deleting a Store Characteristic
//                //            //}
//                //        }
//                //    }

//                //}
//                //////BEGIN TT#667-MD-VStuart-In Use Error in Store Characteristics upon aborted delete of Child Record
//                //if (!_skipInUse)
//                //{
//                ////    //BEGIN TT#643-MD-VStuart-Need Queries for Store Characteristic Values
//                //    inQuiry = false;
//                //    menuInUseInfo_Click(sender,  e);
//                //    inQuiry = true;

//                ////    ////BEGIN TT#110-MD-VStuart - In Use Tool
//                ////    //const eProfileType etype = eProfileType.StoreCharacteristics;
//                ////    //string inUseTitle = Regex.Replace(etype.ToString(), "((?<=[a-z])[A-Z]|[A-Z](?=[a-z]))", " $1").Trim();
//                ////    //bool display = false;
//                ////    //bool inQuiry = false;
//                ////    //DisplayInUseForm(_ridList, eProfileType.StoreCharacteristics, inUseTitle, ref display, inQuiry);
//                ////    //If the InUseForm has data cancel the delete process.
//                ////    //_display = display;
//                //    if (_display)
//                //    {
//                //        _deletedStoreCharValueKeys.Clear();
//                //        e.Cancel = true;
//                //    }
//                ////    //END TT#110-MD-VStuart - In Use Tool
//                ////    //END   TT#643-MD-VStuart-Need Queries for Store Characteristic Values
//                //}
//                //_skipInUse = false;
//                //////END   TT#667-MD-VStuart-In Use Error in Store Characteristics upon aborted delete of Child Record
//            }
//            catch
//            {
//                throw;
//            }
//        }

//        private void DebugGrid()
//        {
//            Debug.WriteLine("    ");
//            Debug.WriteLine("--GRID--");
//            foreach(UltraGridRow row in charGrid.Rows)
//            {
//                string charRid = row.Cells["SCG_RID"].Value.ToString();
//                string charName = row.Cells["SCG_ID"].Value.ToString() ;
//                Debug.WriteLine(charRid + " " + charName);
//                if (row.HasChild())
//                {
//                    foreach(UltraGridRow childRow in row.ChildBands[0].Rows)
//                    {
//                        string charRid2 = childRow.Cells["SCG_RID"].Value.ToString();
//                        string charValRid =  childRow.Cells["SC_RID"].Value.ToString();
//                        string textValue =  childRow.Cells["TEXT_VALUE"].Value.ToString() ;
//                        string dateValue =  childRow.Cells["DATE_VALUE"].Value.ToString() ;
//                        string numberValue =  childRow.Cells["NUMBER_VALUE"].Value.ToString() ;
//                        string dollarValue =  childRow.Cells["DOLLAR_VALUE"].Value.ToString() ;

//                        Debug.WriteLine("   " + charRid2 + " " + charValRid + " " + textValue + " " +
//                            dateValue + " " +
//                            numberValue + " " +
//                            dollarValue);
//                    }
//                }
//            }

//            Debug.WriteLine("--DataSet--");
//            foreach(DataRow row in _storeCharDataSet.Tables[0].Rows)
//            {
//                string charRid = row["SCG_RID"].ToString();
//                string charName = row["SCG_ID"].ToString() ;
//                Debug.WriteLine(charRid + " " + charName);

//                DataRow [] childRows = row.GetChildRows("CharacteristicValue");
//                foreach (DataRow cRow in childRows)
//                {
//                        string charRid2 = cRow["SCG_RID"].ToString();
//                        string charValRid =  cRow["SC_RID"].ToString();
//                        string textValue =  cRow["TEXT_VALUE"].ToString() ;
//                        string dateValue =  cRow["DATE_VALUE"].ToString() ;
//                        string numberValue =  cRow["NUMBER_VALUE"].ToString() ;
//                        string dollarValue =  cRow["DOLLAR_VALUE"].ToString() ;

//                        Debug.WriteLine("   " + charRid2 + " " + charValRid + " " + textValue + " " +
//                            dateValue + " " +
//                            numberValue + " " +
//                            dollarValue);
//                }
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

//        //		override public void IClose()
//        //		{
//        //			try
//        //			{
//        //				if (_storeCharDataSet.HasChanges())
//        //				{
//        //
//        //					if (MessageBox.Show (_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_SavePendingChanges),  "Save Changes",
//        //						MessageBoxButtons.YesNo, MessageBoxIcon.Question)
//        //						== DialogResult.Yes) 
//        //					{
//        //						SaveChanges();
//        //					}
//        //				}
//        //
//        //				this.Close();
//        //
//        //			}		
//        //			catch(Exception ex)
//        //			{
//        //				MessageBox.Show(ex.Message);
//        //			}
//        //			
//        //		}

//        override public void ISave()
//        {
//            try
//            {
//                if (ValidateCharacteristics())
//                {
//                    if (_storeCharDataSet.HasChanges())
//                    {
//                        SaveChanges();
//                    }
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

//        private void menuInsert_Click(object sender, System.EventArgs e)
//        {
//            charGrid.DisplayLayout.Bands[0].AddNew();
//        }

//        private void menuDelete_Click(object sender, System.EventArgs e)
//        {
//            charGrid.DeleteSelectedRows(true);
//        }

//        //Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
//        private class StoreGroupNodeDescription
//        {
//            public int StoreGroupRID;
//            public int FolderRID;

//            public StoreGroupNodeDescription(int aStoreGroupRID, int aFolderRID)
//            {
//                StoreGroupRID = aStoreGroupRID;
//                FolderRID = aFolderRID;
//            }
//        }
//        //End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders

//        //BEGIN TT#110-MD-VStuart - In Use Tool
//        private void menuInUseInfo_Click(object sender, EventArgs e)
//        {
//            _ridList = new ArrayList();
//            //BEGIN TT#496-MD-VStuart-System error when deleting a Store Characteristic
//            //Band[1] is for the Child Rows.
//            UltraGridColumn scRidColumn = charGrid.DisplayLayout.Bands[1].Columns["SC_RID"];
//            foreach (UltraGridRow row in charGrid.Selected.Rows)
//            {
//                if (row.Band.Key == "CharacteristicValue")
//                {
//                    etype = eProfileType.StoreCharacteristicValues;  //TT#643-MD-VStuart-Need Queries for Store Characteristic Values
//                    int scRid = Convert.ToInt32(row.GetCellValue(scRidColumn), CultureInfo.CurrentUICulture);
//                    if (row.Band != charGrid.DisplayLayout.Bands[1]) continue;
//                    if (scRid > 0)
//                    {
//                        _ridList.Add(scRid);
//                    }
//                }
//            }
//            //Band[0] is for the Parent Rows.
//            UltraGridColumn scgRidColumn = charGrid.DisplayLayout.Bands[0].Columns["SCG_RID"];
//            foreach (UltraGridRow row in charGrid.Selected.Rows)
//            {
//                if (row.Band.Key == "STORE_CHAR_GROUP")
//                {
//                    //BEGIN TT#643-MD-VStuart-Need Queries for Store Characteristic Values
//                    etype = eProfileType.StoreCharacteristics;
//                    int scgRid = Convert.ToInt32(row.GetCellValue(scgRidColumn), CultureInfo.CurrentUICulture);
//                    if (row.Band != charGrid.DisplayLayout.Bands[0]) continue;
//                    if (scgRid > 0)
//                    {
//                        _ridList.Add(scgRid);
//                    }
//                }
//            }
//            //END  TT#496-MD-VStuart-System error when deleting a Store Characteristic
//            //If the no RID is selected do nothing.
//            if (_ridList.Count > 0)
//            {
//                //const eProfileType etype = eProfileType.StoreCharacteristics;
//                //string inUseTitle = Regex.Replace(etype.ToString(), "((?<=[a-z])[A-Z]|[A-Z](?=[a-z]))", " $1").Trim(); //TT#1532-MD -jsobek -Add In Use for Header Characteristics
//                string inUseTitle = InUseUtility.GetInUseTitleFromProfileType(etype); //TT#1532-MD -jsobek -Add In Use for Header Characteristics
//                bool display = false;
//                //bool inQuiry = true; 
//                DisplayInUseForm(_ridList, etype, inUseTitle, ref display, inQuiry);
//                _display = display;
//                //END   TT#643-MD-VStuart-Need Queries for Store Characteristic Values
//            }
//        }

//        private bool menuInUseInfo_Click(object sender, eProfileType type, int aRID)
//        {
//            _ridList = new ArrayList();
//            ////BEGIN TT#496-MD-VStuart-System error when deleting a Store Characteristic
//            ////Band[1] is for the Child Rows.
//            //UltraGridColumn scRidColumn = charGrid.DisplayLayout.Bands[1].Columns["SC_RID"];
//            //foreach (UltraGridRow row in charGrid.Selected.Rows)
//            //{
//            //    if (row.Band.Key == "CharacteristicValue")
//            //    {
//            //        etype = eProfileType.StoreCharacteristicValues;  //TT#643-MD-VStuart-Need Queries for Store Characteristic Values
//            //        int scRid = Convert.ToInt32(row.GetCellValue(scRidColumn), CultureInfo.CurrentUICulture);
//            //        if (row.Band != charGrid.DisplayLayout.Bands[1]) continue;
//            //        if (scRid > 0)
//            //        {
//            //            _ridList.Add(scRid);
//            //        }
//            //    }
//            //}
//            ////Band[0] is for the Parent Rows.
//            //UltraGridColumn scgRidColumn = charGrid.DisplayLayout.Bands[0].Columns["SCG_RID"];
//            //foreach (UltraGridRow row in charGrid.Selected.Rows)
//            //{
//            //    if (row.Band.Key == "STORE_CHAR_GROUP")
//            //    {
//            //        //BEGIN TT#643-MD-VStuart-Need Queries for Store Characteristic Values
//            //        etype = eProfileType.StoreCharacteristics;
//            //        int scgRid = Convert.ToInt32(row.GetCellValue(scgRidColumn), CultureInfo.CurrentUICulture);
//            //        if (row.Band != charGrid.DisplayLayout.Bands[0]) continue;
//            //        if (scgRid > 0)
//            //        {
//            //            _ridList.Add(scgRid);
//            //        }
//            //    }
//            //}
//            _ridList.Add(aRID);
//            //END  TT#496-MD-VStuart-System error when deleting a Store Characteristic
//            //If the no RID is selected do nothing.
//            if (_ridList.Count > 0)
//            {
//                //const eProfileType etype = eProfileType.StoreCharacteristics;
//                //string inUseTitle = Regex.Replace(etype.ToString(), "((?<=[a-z])[A-Z]|[A-Z](?=[a-z]))", " $1").Trim(); //TT#1532-MD -jsobek -Add In Use for Header Characteristics
//                string inUseTitle = InUseUtility.GetInUseTitleFromProfileType(etype); //TT#1532-MD -jsobek -Add In Use for Header Characteristics
//                bool display = false;
//                //bool inQuiry = true; 
//                DisplayInUseForm(_ridList, type, inUseTitle, ref display, inQuiry);
//                _display = display;
//                //END   TT#643-MD-VStuart-Need Queries for Store Characteristic Values
//            }
//            return _display;
//        }
//        //END TT#110-MD-VStuart - In Use Tool
//    }
//}
