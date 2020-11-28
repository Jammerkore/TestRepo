//using System;
//using System.Drawing;
//using System.Collections;
//using System.ComponentModel;
//using System.Windows.Forms;
//using System.Globalization;
//using System.Data;
//using System.Diagnostics;

//using Infragistics.Win;
//using Infragistics.Win.UltraWinGrid;

//using MIDRetail.Business;
//using MIDRetail.Business.Allocation;
//using MIDRetail.DataCommon;
//using MIDRetail.Common;
//using MIDRetail.Data;

//namespace MIDRetail.Windows {
//    public class HeaderCharacteristicMaint : MIDFormBase {
//        #region CONSTANTS
//        private const int CHAR_GROUP_BAND = 0;
//        private const int CHAR_BAND = 1;
//        private const int NOT_FOUND = -1;
//        private const char CHAR_BOOL_TRUE = '1';
//        private const char CHAR_BOOL_FALSE = '0';
		
//        // constants for the table names
//        private const string TABLE_HCG = "HEADER_CHAR_GROUP";
//        private const string TABLE_HC = "HEADER_CHAR";

//        // BEGIN MID Track #3978 - error deleting characteristic  ; comment out the following
//        //private const string TABLE_HCJ = "HEADER_CHAR_JOIN";
//        // END MID Track #3978  

//        private const string TABLE_H = "CH_Header";

//        // column constants for the table header char join
//        private const string COLUMN_J_H_RID = "HDR_RID";
//        private const string COLUMN_J_C_RID = "HC_RID";

//        // columns constats for the table header_char_group
//        private const string COLUMN_G_RID = "HCG_RID";
//        private const string COLUMN_G_ID = "HCG_ID";
//        private const string COLUMN_G_TYPE = "HCG_TYPE";
//        private const string COLUMN_G_LIST_IND = "HCG_LIST_IND";
//        private const string COLUMN_G_PROTECT = "HCG_PROTECT_IND";
//        private const string CHECKBOX = "CheckBox";

//        // Column constants for table header_char
//        private const string COLUMN_C_RID = "HC_RID";
//        private const string COLUMN_C_G_RID = "HCG_RID";
//        private const string COLUMN_C_TXTV = "TEXT_VALUE";
//        private const string COLUMN_C_DATEV = "DATE_VALUE";
//        private const string COLUMN_C_NUMV = "NUMBER_VALUE";
//        private const string COLUMN_C_DOLLARV = "DOLLAR_VALUE";

//        // columns constants for the header table
//        private const string COLUMN_H_RID = "KeyH";
//        #endregion

//        private DataTable _DTbl_HGroup;
//        private DataTable _DTbl_HChar;
//        private DataSet _hCharDSet, _shadowHcharDSet;
//        private Hashtable _htGridState;
//        private SessionAddressBlock _SAB;
//        private StoreServerSession _storeSession;
//        private HeaderCharacteristicsData _hCharDAdapter;
//        private ArrayList _msgInsert, _msgDeleteCharGrp, _msgDeleteChar;

//        private System.Windows.Forms.Button btnSave;
//        private System.Windows.Forms.Button btnClose;
//        private Infragistics.Win.UltraWinGrid.UltraGrid charGrid;
//        // BEGIN MID Track #3639 - workspace error after header characteristics maintenance 
//        private bool _valueRowInserted = false;
//        // END MID Track #3639 

//        private bool _removeLinkGlobalOption = false; // TT#78 - Ron Matelic - Header Char delete issue

//        public delegate void HeaderMaintSaved();
//        public event HeaderMaintSaved OnSavedHandler;
//        // BEGIN MID Track #5743 - KJohnson - Allow creation of PO field in header charac, when already defined.
//        public delegate bool HeaderMaintNewColumn(string column);
//        public event HeaderMaintNewColumn OnNewColumnHandler;
//        // END MID Track #5743 - KJohnson

//        private System.Windows.Forms.ContextMenu menuChar; //TT#1532-MD -jsobek -Add In Use for Header Characteristics
//        private MenuItem menuInUseInfo; //TT#1532-MD -jsobek -Add In Use for Header Characteristics

//        private System.ComponentModel.Container components = null;

//        public HeaderCharacteristicMaint(SessionAddressBlock aSAB) : base(aSAB){
//            _SAB = aSAB;
//            _hCharDSet = MIDEnvironment.CreateDataSet();
//            _shadowHcharDSet = MIDEnvironment.CreateDataSet();
//            _hCharDAdapter = new HeaderCharacteristicsData();
//            _msgInsert = new ArrayList();
//            _msgDeleteCharGrp = new ArrayList();
//            _msgDeleteChar = new ArrayList();

//            InitializeComponent();

//            //Begin TT#1532-MD -jsobek -Add In Use for Header Characteristics
//            this.menuChar = new System.Windows.Forms.ContextMenu();
//            this.menuInUseInfo = new System.Windows.Forms.MenuItem();
//            this.menuInUseInfo.Index = 2;
//            this.menuInUseInfo.Text = "In Use";
//            this.menuInUseInfo.Click += new System.EventHandler(this.menuInUseInfo_Click);
//            this.menuChar.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {this.menuInUseInfo});
//            this.charGrid.ContextMenu = this.menuChar;
//            //End TT#1532-MD -jsobek -Add In Use for Header Characteristics

//            _storeSession = _SAB.StoreServerSession;
//            FunctionSecurity = _SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AdminHeadersCharacteristics);
//        }

//        protected override void Dispose( bool disposing ) 
//        {
//            if( disposing ) 
//            {
//                if (components != null) 
//                {
//                    components.Dispose();
//                }
//                this.charGrid.InitializeRow -= new Infragistics.Win.UltraWinGrid.InitializeRowEventHandler(this.charGrid_InitializeRow);
//                this.charGrid.CellChange -= new Infragistics.Win.UltraWinGrid.CellEventHandler(this.charGrid_CellChange);
//                this.charGrid.BeforeExitEditMode -= new Infragistics.Win.UltraWinGrid.BeforeExitEditModeEventHandler(this.charGrid_BeforeExitEditMode);
//                this.charGrid.AfterRowInsert -= new Infragistics.Win.UltraWinGrid.RowEventHandler(this.charGrid_AfterRowInsert);
//                this.charGrid.BeforeRowsDeleted -= new Infragistics.Win.UltraWinGrid.BeforeRowsDeletedEventHandler(this.charGrid_BeforeRowsDeleted);
//                this.charGrid.BeforeRowInsert -= new Infragistics.Win.UltraWinGrid.BeforeRowInsertEventHandler(this.charGrid_BeforeRowInsert);
//                this.charGrid.InitializeLayout -= new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.charGrid_InitializeLayout);
//                //Begin TT#169 - JSmith - enter max of 1000 and receive 'must be integer'
//                MIDRetail.Windows.Controls.UltraGridLayoutDefaults ugld = new MIDRetail.Windows.Controls.UltraGridLayoutDefaults(ErrorImage);
//                ugld.DetachGridEventHandlers(charGrid);
//                //End TT#169
//                this.btnSave.Click -= new System.EventHandler(this.btnSave_Click);
//                this.btnClose.Click -= new System.EventHandler(this.btnClose_Click);
//                this.Closing -= new System.ComponentModel.CancelEventHandler(this.HeaderCharacteristicMaint_Closing);
//                this.Load -= new System.EventHandler(this.HeaderCharacteristicMaint_Load);
//            }
//            base.Dispose( disposing );
//        }


//        private void HeaderCharacteristicMaint_Load(object sender, System.EventArgs e) {
//            try {
//                if (FunctionSecurity.AllowUpdate) {
//                    Format_Title(eDataState.Updatable, eMIDTextCode.frm_Administration, MIDText.GetTextOnly(eMIDTextCode.frm_HeaderCharacteristicMaint));
//                }
//                else {
//                    Format_Title(eDataState.ReadOnly, eMIDTextCode.frm_Administration, MIDText.GetTextOnly(eMIDTextCode.frm_HeaderCharacteristicMaint));
//                }
//                loadData();
//                SetReadOnly(FunctionSecurity.AllowUpdate);  //Security changes - 1/24/2005 vg
//                SetGridDeleteAccess();

//            }
//            catch (Exception ex) {
//                HandleException(ex);
//            }
//        }

//        private void SetGridDeleteAccess()
//        {
//            foreach(UltraGridBand ugb in charGrid.DisplayLayout.Bands)
//            {
//                if (!FunctionSecurity.AllowUpdate)
//                {
//                    ugb.Override.AllowDelete = DefaultableBoolean.False;
//                }
//                else
//                {
//                    ugb.Override.AllowDelete = DefaultableBoolean.True;
//                }
//            }
//        }

//        private void HeaderCharacteristicMaint_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
//            try {
//                if(charGrid.ActiveCell != null && charGrid.ActiveCell.IsInEditMode)
//                {
//                    // Begin TT#118 - RMatelic - Can't exit after inserting row without entering data
//                    //MessageBox.Show(_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ErrorCantExitWhileEditing));
//                    // End TT#118  
//                    e.Cancel = true;
//                    return;
//                }
//                charGrid.ResumeRowSynchronization();
//                charGrid.UpdateData();
//                if (_hCharDSet.HasChanges() || checkProtectedChanged()) {
//                    ChangePending = true;
//                    //if (MessageBox.Show (_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_SavePendingChanges),  "Save Changes",
//                    //    MessageBoxButtons.YesNo, MessageBoxIcon.Question)
//                    //    == DialogResult.Yes) {
//                    //    SaveChanges();
//                    //}
//                }

//            }
//            catch (Exception ex) {
//                HandleException(ex);
//            }
//        }


//        #region HELPER METHODS
//        private void loadData() {
//            _hCharDSet.Clear();
//            _shadowHcharDSet.Clear();
			
//            _msgInsert.Clear();
//            _msgDeleteCharGrp.Clear();
//            _msgDeleteChar.Clear();
				
//            // BEGIN MID Track #3978 - error deleting characteristic ; comment out the following
//            //DetermineHeaders(ref _shadowHcharDSet);
//            // END MID Track #3978  

//            _hCharDAdapter.HeaderCharGroup_ReadUsingAdapter(_hCharDSet);
//            _hCharDAdapter.HeaderChar_ReadUsingAdapter(_hCharDSet);

//            _DTbl_HGroup = _hCharDSet.Tables[TABLE_HCG];
//            _DTbl_HChar = _hCharDSet.Tables[TABLE_HC];

//            // load data into shadow datatable
//            _hCharDAdapter.HeaderCharGroup_ReadUsingAdapter(_shadowHcharDSet);
//            _hCharDAdapter.AllHeaderChar_ReadUsingAdapter(_shadowHcharDSet);
//            // BEGIN MID Track #3978 - error deleting characteristic  ; comment out the following
//            //_hCharDAdapter.HeaderJoin_ReadUsingAdapter(_shadowHcharDSet, null);
//            // END MID Track #3978  
//            DataColumn HCG_PK = _DTbl_HGroup.Columns[COLUMN_G_RID];
//            DataColumn HC_PK = _DTbl_HChar.Columns[COLUMN_C_RID];

//            HCG_PK.AutoIncrement = true;
//            HCG_PK.AutoIncrementSeed = -1;
//            HCG_PK.AutoIncrementStep = -1;

//            HC_PK.AutoIncrement = true;
//            HC_PK.AutoIncrementSeed = -1;
//            HC_PK.AutoIncrementStep = -1;

//            _DTbl_HGroup.PrimaryKey = new DataColumn[] {HCG_PK};
//            _DTbl_HChar.PrimaryKey = new DataColumn[] {HC_PK};

//            // set shadow dataset keys.
//            _shadowHcharDSet.Tables[TABLE_HCG].PrimaryKey = 
//                new DataColumn[] { _shadowHcharDSet.Tables[TABLE_HCG].Columns[COLUMN_G_RID] };
//            _shadowHcharDSet.Tables[TABLE_HC].PrimaryKey = 
//                new DataColumn[] { _shadowHcharDSet.Tables[TABLE_HC].Columns[COLUMN_C_RID] };

//            // BEGIN MID Track #3978 - error deleting characteristic ; comment out the following
//            //_shadowHcharDSet.Tables[TABLE_HCJ].PrimaryKey = 
//            //	new DataColumn[] { _shadowHcharDSet.Tables[TABLE_HCJ].Columns[COLUMN_J_H_RID], 
//            //						 _shadowHcharDSet.Tables[TABLE_HCJ].Columns[COLUMN_J_C_RID] };
//            // END MID Track #3978  

//            if(!_hCharDSet.Relations.Contains("VALUES")) {
//                DataRelation dr = new DataRelation(
//                    "VALUES", _DTbl_HGroup.Columns[COLUMN_G_RID], 
//                    _DTbl_HChar.Columns[COLUMN_C_G_RID]);
//                _hCharDSet.Relations.Add(dr);
//                dr.ChildKeyConstraint.DeleteRule = Rule.Cascade;
//                dr.ChildKeyConstraint.UpdateRule = Rule.Cascade;		
//            }

//            // showdataset's relationships.
//            if(!_shadowHcharDSet.Relations.Contains("VALUES")) {
//                DataRelation dr = new DataRelation(
//                    "VALUES", _shadowHcharDSet.Tables[TABLE_HCG].Columns[COLUMN_G_RID], 
//                    _shadowHcharDSet.Tables[TABLE_HC].Columns[COLUMN_C_G_RID]);
//                _shadowHcharDSet.Relations.Add(dr);
//                dr.ChildKeyConstraint.DeleteRule = Rule.Cascade;	
//            }

//            // BEGIN MID Track #3978 - error deleting characteristic ; comment out the following
//            //if(!_shadowHcharDSet.Relations.Contains("CHARS")) {
//            //	DataRelation dr = new DataRelation(
//            //		"CHARS", _shadowHcharDSet.Tables[TABLE_HC].Columns[COLUMN_C_RID], 
//            //		_shadowHcharDSet.Tables[TABLE_HCJ].Columns[COLUMN_J_C_RID]);
//            //	_shadowHcharDSet.Relations.Add(dr);
//            //	dr.ChildKeyConstraint.DeleteRule = Rule.Cascade;	
//            //}
//            // END MID Track #3978 

//            charGrid.DataSource = _DTbl_HGroup;
//            charGrid.SuspendRowSynchronization();
//        }
//        private bool checkProtectedChanged() {
//            bool retValue = false;
//            RowsCollection rows = charGrid.Rows;
//            int rCount = rows.Count;

//            for(int i = 0; i < rCount; i++) {
//                int rID = Convert.ToInt32( rows[i].Cells[COLUMN_G_RID].Value, CultureInfo.CurrentUICulture );
//                char isChecked = (Convert.ToBoolean(rows[i].Cells[CHECKBOX].Value)) ? CHAR_BOOL_TRUE : CHAR_BOOL_FALSE;

//                int foundCount = _DTbl_HGroup.Select(
//                    String.Format("{0} = {1} AND {2} <> '{3}'",
//                    COLUMN_G_RID, rID, COLUMN_G_PROTECT, isChecked)).Length;
//                if (foundCount > 0) {
//                    retValue = true;
//                    break;
//                }
//            }

//            return retValue;
//        }

//        private void PopulateTypeValueList() {
//            ValueList objValueList = this.charGrid.DisplayLayout.ValueLists.Add(("Types"));
//            objValueList.ValueListItems.Add(0, "Text");
//            objValueList.ValueListItems.Add(1, "Date");
//            objValueList.ValueListItems.Add(2, "Number");
//            objValueList.ValueListItems.Add(3, "Dollar");
//        }

//        private void DisableFieldsInRow(UltraGridRow childRow, eStoreCharType aCharType) {
//            UltraGridCell aCell = null;

//            switch (aCharType) {
//                case eStoreCharType.text:
//                    aCell = childRow.Cells[COLUMN_C_DATEV];
//                    aCell.Activation = Activation.Disabled;
//                    aCell = childRow.Cells[COLUMN_C_NUMV];
//                    aCell.Activation = Activation.Disabled;
//                    aCell = childRow.Cells[COLUMN_C_DOLLARV];
//                    aCell.Activation = Activation.Disabled;
//                    aCell = childRow.Cells[COLUMN_C_TXTV];
//                    aCell.Activation = Activation.AllowEdit;
//                    break;
//                case eStoreCharType.date:
//                    aCell = childRow.Cells[COLUMN_C_TXTV];
//                    aCell.Activation = Activation.Disabled;
//                    aCell = childRow.Cells[COLUMN_C_NUMV];
//                    aCell.Activation = Activation.Disabled;
//                    aCell = childRow.Cells[COLUMN_C_DOLLARV];
//                    aCell.Activation = Activation.Disabled;
//                    aCell = childRow.Cells[COLUMN_C_DATEV];
//                    aCell.Activation = Activation.AllowEdit;
//                    break;
//                case eStoreCharType.number:
//                    aCell = childRow.Cells[COLUMN_C_TXTV];
//                    aCell.Activation = Activation.Disabled;
//                    aCell = childRow.Cells[COLUMN_C_DATEV];
//                    aCell.Activation = Activation.Disabled;
//                    aCell = childRow.Cells[COLUMN_C_DOLLARV];
//                    aCell.Activation = Activation.Disabled;
//                    aCell = childRow.Cells[COLUMN_C_NUMV];
//                    aCell.Activation = Activation.AllowEdit;
//                    break;
//                case eStoreCharType.dollar:
//                    aCell = childRow.Cells[COLUMN_C_TXTV];
//                    aCell.Activation = Activation.Disabled;
//                    aCell = childRow.Cells[COLUMN_C_NUMV];
//                    aCell.Activation = Activation.Disabled;
//                    aCell = childRow.Cells[COLUMN_C_DATEV];
//                    aCell.Activation = Activation.Disabled;
//                    aCell = childRow.Cells[COLUMN_C_DOLLARV];
//                    aCell.Activation = Activation.AllowEdit;
//                    break;
//            }
//        }

//        private void SaveGridState() {
//            if (_htGridState == null)
//                _htGridState = new Hashtable();
//            else
//                _htGridState.Clear();

//            RowsCollection rows = charGrid.Rows;
//            int rCount = rows.Count;

//            for(int i = 0; i < rCount; i++) {
//                int scr_rid = Convert.ToInt32(rows[i].Cells[COLUMN_G_RID].Value, CultureInfo.CurrentUICulture);
//                _htGridState.Add( scr_rid, rows[i].Expanded);
//            }
//        }

//        private void RestoreGridState() {
//            object expanded = null;

//            RowsCollection rows = charGrid.Rows;
//            int rCount = rows.Count;

//            for(int i = 0; i < rCount; i++) {
//                expanded = _htGridState[Convert.ToInt32(rows[i].Cells[COLUMN_G_RID].Value, CultureInfo.CurrentUICulture)];
//                if (expanded == null)
//                    rows[i].Expanded = true;
//                else if ((bool)expanded)
//                    rows[i].Expanded = true;
//            }
//        }
//        #endregion

//        #region ULTRA WIN GRID EVENTS
//        private void charGrid_InitializeRow(object sender, Infragistics.Win.UltraWinGrid.InitializeRowEventArgs e) {
//            // Only set the checkbox if its the parent band and its not a newly insert row
//            if(e.Row.Band.Index == CHAR_GROUP_BAND && e.Row.Cells[COLUMN_G_PROTECT].Value != DBNull.Value) {
//                if(!e.ReInitialize)
//                    e.Row.Cells[CHECKBOX].Value = e.Row.Cells[COLUMN_G_PROTECT].Value.ToString()[0] == CHAR_BOOL_TRUE ? true : false;
				
//                int ridID = Convert.ToInt32(e.Row.Cells[COLUMN_G_RID].Value);
//                DataRow ddr = _shadowHcharDSet.Tables[TABLE_HCG].Rows.Find(ridID);
//                bool hasHiddenChildern = (ddr != null && ddr.GetChildRows("VALUES").Length > 0) ? true : false;

//                if(e.Row.HasChild() || hasHiddenChildern) {
//                    eStoreCharType aCharType = (eStoreCharType)Convert.ToInt32( e.Row.Cells[COLUMN_G_TYPE].Value, CultureInfo.CurrentUICulture );
//                    e.Row.Cells[COLUMN_G_TYPE].Activation = Activation.NoEdit;

////					RowsCollection rColl = e.Row.ChildBands[0].Rows;
////					int rCount = rColl.Count;
////
////					for(int i = 0; i < rCount; i++) {
////						DisableFieldsInRow(rColl[i], aCharType);
////					}
//                }
//            }
//            if(e.Row.Band.Index == CHAR_BAND && e.Row.ParentRow.Cells[COLUMN_G_TYPE].Value != DBNull.Value) {
//                eStoreCharType aCharType = (eStoreCharType)Convert.ToInt32( e.Row.ParentRow.Cells[COLUMN_G_TYPE].Value, CultureInfo.CurrentUICulture );
//                DisableFieldsInRow(e.Row, aCharType);
//            }

//        }

//        private void charGrid_CellChange(object sender, Infragistics.Win.UltraWinGrid.CellEventArgs e) {
//            if (e.Cell.Column.Key == COLUMN_G_TYPE && e.Cell.EditorResolved.IsValid) {
//                eStoreCharType aCharType = (eStoreCharType)Convert.ToInt32( e.Cell.EditorResolved.Value, CultureInfo.CurrentUICulture );
//                UltraGridRow row = e.Cell.Row;
//                if (row.HasChild()) {
//                    RowsCollection rows = row.ChildBands[0].Rows;
//                    int rCount = rows.Count;
//                    for(int i = 0; i < rCount; i++) {
//                        DisableFieldsInRow(rows[i] ,aCharType);
//                    }
//                }
//            }

//            if (FormLoaded)
//            {
//                ChangePending = true;
//            }
//        }

//        private void charGrid_AfterRowInsert(object sender, Infragistics.Win.UltraWinGrid.RowEventArgs e) {
//            // Handle all relevant things when the add button is pressed
//            // For parentband set the checkbox to true
//            if(e.Row.Band.Index == CHAR_GROUP_BAND) {
//                e.Row.Cells[CHECKBOX].Value = true;
//                e.Row.Cells[COLUMN_G_LIST_IND].Value = CHAR_BOOL_FALSE;
//                e.Row.Cells[COLUMN_G_PROTECT].Value = CHAR_BOOL_TRUE;
//            }

//            // For childband set disable all colums except for the type selected
//            if(e.Row.Band.Index == CHAR_BAND)
//            {
//                if(e.Row.ParentRow.Cells[COLUMN_G_TYPE].Value != DBNull.Value)
//                {
//                    eStoreCharType aCharType = (eStoreCharType) Convert.ToInt32( e.Row.ParentRow.Cells[COLUMN_G_TYPE].Value, CultureInfo.CurrentUICulture );
//                    e.Row.ParentRow.Cells[COLUMN_G_TYPE].Activation = Activation.NoEdit;
//                    DisableFieldsInRow(e.Row, aCharType);

//                    // Update the List Indicator value to show that we have subitems
//                    // but update the dataset directlyl
//                    UltraGridRow parentGridRow = e.Row.ParentRow;							
//                    parentGridRow.Cells[COLUMN_G_LIST_IND].Value = CHAR_BOOL_TRUE;

//                    DataRow parentDBRow = 
//                        _DTbl_HGroup.Rows.Find(parentGridRow.Cells[COLUMN_G_RID].Value);
//                    if (parentDBRow != null) {
//                        parentDBRow[COLUMN_G_LIST_IND] = CHAR_BOOL_TRUE;
//                    }
//                    // BEGIN MID Track #3639 - workspace error after header characteristics maintenance 
//                    _valueRowInserted = true;
//                    // Set the active cell accordingly
//                    switch (aCharType) {
//                        case eStoreCharType.date:
//                            charGrid.ActiveCell = e.Row.Cells[COLUMN_C_DATEV];
//                            charGrid.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.EnterEditMode, false, false);
//                            break;
//                        case eStoreCharType.text:
//                            charGrid.ActiveCell = e.Row.Cells[COLUMN_C_TXTV];
//                            charGrid.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.EnterEditMode, false, false);
//                            break;
//                        case eStoreCharType.number:
//                            charGrid.ActiveCell = e.Row.Cells[COLUMN_C_NUMV];
//                            charGrid.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.EnterEditMode, false, false);
//                            break;
//                        case eStoreCharType.dollar:
//                            charGrid.ActiveCell = e.Row.Cells[COLUMN_C_DOLLARV];
//                            charGrid.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.EnterEditMode, false, false);
//                            break;
//                    }
//                    // END MID Track #3639 
//                } 
//            } 
//        }

//        private void charGrid_BeforeRowInsert(object sender, Infragistics.Win.UltraWinGrid.BeforeRowInsertEventArgs e) {
//            if(e.Band.Index == CHAR_BAND) {
//                if (e.ParentRow.Cells[COLUMN_G_TYPE].Value == DBNull.Value || e.ParentRow.Cells[COLUMN_G_ID].Value == DBNull.Value) {
//                    e.Cancel = true; 
//                    MessageBox.Show(_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_IncompleteCharacteristicGroup),
//                        this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
//                    return;
//                }

//                int gRID = Convert.ToInt32(e.ParentRow.Cells[COLUMN_G_RID].Value);
//                // a new sub row is being inserted check if its parent row is already in the database
//                // also if we've already thrown an error otherwise check count and throw the message
//                if (gRID > 0 && !_msgInsert.Contains(gRID)) {
//                    DataRow[] values = _shadowHcharDSet.Tables[TABLE_HCG].Rows.Find(gRID).GetChildRows("VALUES");
//                    DataRow[] currentChildren = _hCharDSet.Tables[TABLE_HCG].Rows.Find(gRID).GetChildRows("VALUES");

//                    if(values.Length > 0 && currentChildren.Length == 0) {
//                        // if the person presses yes load the data otherwise cancel the event.
//                        if( MessageBox.Show (_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ValuesExisitForCharHeaders),  "Question",
//                            MessageBoxButtons.YesNo, MessageBoxIcon.Question)
//                            == DialogResult.Yes) {
//                            // The person pressed yes so copy all the child values into the current dataset
//                            for(int i = 0; i < values.Length; i++) {
//                                this._DTbl_HChar.ImportRow(values[i]);
//                            }
//                            _msgInsert.Add(gRID);
//                        }
//                        else {
//                            e.Cancel = true;
//                        }
//                    }
//                }
//            }

//            if (FormLoaded)
//            {
//                ChangePending = true;
//            }
//        }

//        private void charGrid_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e) {
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
//            charGrid.DisplayLayout.Bands[CHAR_GROUP_BAND].Columns[COLUMN_G_RID].Hidden = true; // RID HEADER_CHAR_GROUP.HCG_RID
//            charGrid.DisplayLayout.Bands[CHAR_GROUP_BAND].Columns[COLUMN_G_LIST_IND].Hidden = true; // HEADER_CHAR_GROUP.HCG_LIST_IND 
//            charGrid.DisplayLayout.Bands[CHAR_GROUP_BAND].Columns[COLUMN_G_PROTECT].Hidden = true; // HEADER_CHAR_GROUP.HCG_PROTECT_IND 
//            charGrid.DisplayLayout.Bands[CHAR_BAND].Columns[COLUMN_C_RID].Hidden = true; // RID HEADER_CHAR.HC_RID
//            charGrid.DisplayLayout.Bands[CHAR_BAND].Columns[COLUMN_C_G_RID].Hidden = true; // RID HEADER_CHAR.HCG_RID
			
//            // Add Protected Checkbox. 
//            charGrid.DisplayLayout.Bands[CHAR_GROUP_BAND].Columns.Add(CHECKBOX, "Protect");
//            charGrid.DisplayLayout.Bands[CHAR_GROUP_BAND].Columns[CHECKBOX].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox;
//            charGrid.DisplayLayout.Bands[CHAR_GROUP_BAND].Columns[CHECKBOX].DataType = typeof(System.Boolean);

//            charGrid.DisplayLayout.Bands[CHAR_BAND].Columns[COLUMN_C_TXTV].Header.Caption = "Text Values";
//            charGrid.DisplayLayout.Bands[CHAR_BAND].Columns[COLUMN_C_DATEV].Header.Caption = "Date Values";
//            charGrid.DisplayLayout.Bands[CHAR_BAND].Columns[COLUMN_C_NUMV].Header.Caption = "Numeric Values";
//            charGrid.DisplayLayout.Bands[CHAR_BAND].Columns[COLUMN_C_DOLLARV].Header.Caption = "Dollar Values";

//            charGrid.DisplayLayout.Bands[CHAR_GROUP_BAND].Columns[COLUMN_G_ID].Header.Caption = "Characteristics";
//            charGrid.DisplayLayout.Bands[CHAR_GROUP_BAND].Columns[COLUMN_G_ID].Width = 150;
//            charGrid.DisplayLayout.Bands[CHAR_GROUP_BAND].Columns[COLUMN_G_TYPE].Header.Caption = "Value Type";
//            charGrid.DisplayLayout.Bands[CHAR_GROUP_BAND].Columns[COLUMN_G_TYPE].Width = 70;

//            charGrid.DisplayLayout.Bands[CHAR_GROUP_BAND].AddButtonCaption = "Characteristic";
//            charGrid.DisplayLayout.Bands[CHAR_BAND].AddButtonCaption = "Characteristic Value";
			
//            PopulateTypeValueList();
			
//            charGrid.DisplayLayout.Bands[CHAR_GROUP_BAND].Columns[COLUMN_G_TYPE].ValueList = this.charGrid.DisplayLayout.ValueLists["Types"];
//            charGrid.DisplayLayout.Bands[CHAR_GROUP_BAND].Columns[COLUMN_G_TYPE].AutoEdit = true;
//            charGrid.DisplayLayout.Bands[CHAR_GROUP_BAND].Columns[COLUMN_G_TYPE].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;

//        }

//        private void charGrid_BeforeRowsDeleted(object sender, Infragistics.Win.UltraWinGrid.BeforeRowsDeletedEventArgs e) {
//            // Handle if were deleting empty rows
//            if(!e.DisplayPromptMsg) 
//            {
//                return;
//            }

//            e.Cancel = true;
//            e.DisplayPromptMsg = false;
//            int rowCount = e.Rows.Length;

//            if( MessageBox.Show( 
//                String.Format(_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ConfirmRemove) , "these values"), 
//                this.Text, 
//                MessageBoxButtons.OKCancel, 
//                MessageBoxIcon.Question) == DialogResult.Cancel)
//                return;

//            // for all selected rows
//            for(int rIndex = 0; rIndex < rowCount; rIndex++) 
//            {
//                bool deletedFromDataSet = false;
//                UltraGridRow row = e.Rows[rIndex];
//                if(row.Band.Index == CHAR_BAND && Convert.ToInt32(row.Cells[COLUMN_C_RID].Value) > 0)
//                {
//                    // BEGIN MID Track #3978 - error deleting characteristic 
//                    //object cRID = row.Cells[COLUMN_C_RID].Value;
//                    //DataRow dr = _shadowHcharDSet.Tables[TABLE_HC].Rows.Find(cRID);
//                    // bool hasJoins = (dr.GetChildRows("CHARS").Length > 0) ? true : false;
//                    int cRID = Convert.ToInt32(row.Cells[COLUMN_C_RID].Value, CultureInfo.CurrentUICulture);
//                    bool hasJoins = _hCharDAdapter.HeadersExistForChar(cRID);
//                    // END MID Track #3978

//                    bool isCharInUse = IsInUse(eProfileType.HeaderChar, cRID);   //TT#1532-MD -jsobek -Add In Use for Header Characteristics
//                    if (!isCharInUse)
//                    {


//                        if (hasJoins && !_msgDeleteChar.Contains(cRID))
//                        {
//                            e.DisplayPromptMsg = false;
//                            if (MessageBox.Show(_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ValuesDeleteForCharHeaders), this.Text,
//                                MessageBoxButtons.OKCancel, MessageBoxIcon.Question)
//                                == DialogResult.OK)
//                            {

//                                //							// Delete from DataSet
//                                //							_hCharDSet.Tables[TABLE_HC].Rows.Find(cRID).Delete();
//                                // Delete joins from 
//                                _hCharDAdapter.deleteAllJoinsByCharID((int)cRID);
//                                _msgDeleteChar.Add(cRID);
//                                row.Delete(false);
//                                deletedFromDataSet = true;
//                            }
//                            else
//                            {
//                                break;
//                            }
//                        }
//                        else
//                        {
//                            //						// Delete from DataSet
//                            //						_hCharDSet.Tables[TABLE_HC].Rows.Find(cRID).Delete();
//                            _msgDeleteChar.Add(cRID);
//                            deletedFromDataSet = true;
//                            row.Delete(false);
//                        }

//                        //Begin TT#1532-MD -jsobek -Add In Use for Header Characteristics
//                        if (FormLoaded)
//                        {
//                            ChangePending = true;
//                        }
//                        //End TT#1532-MD -jsobek -Add In Use for Header Characteristics

//                    }
//                }
//                else if(row.Band.Index == CHAR_GROUP_BAND && Convert.ToInt32(row.Cells[COLUMN_G_RID].Value) > 0)
//                {
//                    object gRID = row.Cells[COLUMN_G_RID].Value;

//                    // Begin TT#78 - Deleting the Header Char linked to the Global Option Linked Header Char causes problems
//                    if (!OkToContinueCharGroupDelete((int)gRID))
//                    {
//                        return;
//                    }
//                    // End TT#78

//                    DataRow dr = _shadowHcharDSet.Tables[TABLE_HCG].Rows.Find(gRID);
//                    DataRow[] charRows = dr.GetChildRows("VALUES");
//                    bool hasJoins = false;

//                    // See if any of my childern have values attached
//                    for(int cIndex = 0; cIndex < charRows.Length; cIndex++) 
//                    {
//                        // BEGIN MID Track #3978 - error deleting characteristic
//                        //if(charRows[cIndex].GetChildRows("CHARS").Length > 0)
//                        //{
//                        //	hasJoins = true;
//                        //	break;
//                        //}
//                        int cRID = Convert.ToInt32(charRows[cIndex]["HC_RID"], CultureInfo.CurrentUICulture); 
//                        if( _hCharDAdapter.HeadersExistForChar(cRID))
//                        {
//                            hasJoins = true;
//                            break;
//                        }
//                        // END MID Track #3978
//                    }

//                    int groupRid = Convert.ToInt32(gRID); //TT#1532-MD -jsobek -Add In Use for Header Characteristics
//                    bool isCharInUse = IsInUse(eProfileType.HeaderCharGroup, groupRid);   //TT#1532-MD -jsobek -Add In Use for Header Characteristics
//                    if (!isCharInUse)
//                    {
//                        if (hasJoins && !_msgDeleteCharGrp.Contains(gRID))
//                        {
//                            e.DisplayPromptMsg = false;
//                            if (MessageBox.Show(_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ValuesDeleteForCharGroups), this.Text,
//                                MessageBoxButtons.OKCancel, MessageBoxIcon.Question)
//                                == DialogResult.OK)
//                            {
//                                DataRow groupRow = _hCharDSet.Tables[TABLE_HCG].Rows.Find(gRID);

//                                // if the shadow dataset has values underneath it but the current one doesnt
//                                // then import it so the values also get deleted.
//                                if (groupRow.GetChildRows("VALUES").Length != charRows.Length)
//                                {
//                                    for (int gCIndex = 0; gCIndex < charRows.Length; gCIndex++)
//                                    {
//                                        try
//                                        {
//                                            if (!_msgDeleteChar.Contains(charRows[gCIndex][COLUMN_C_RID]))
//                                                _hCharDSet.Tables[TABLE_HC].ImportRow(charRows[gCIndex]);
//                                        }
//                                        catch (ConstraintException) { };
//                                    }
//                                }

//                                //							// Delete from DataSet
//                                //							_hCharDSet.Tables[TABLE_HCG].Rows.Find(gRID).Delete();
//                                // Delete joins from tables
//                                _hCharDAdapter.deleteAllJoinsByCharGroupID((int)gRID);
//                                row.Delete(false);
//                                _msgDeleteCharGrp.Add(gRID);
//                                deletedFromDataSet = true;
//                            }
//                            else
//                            {
//                                break;
//                            }
//                        }
//                        else
//                        {
//                            DataRow groupRow = _hCharDSet.Tables[TABLE_HCG].Rows.Find(gRID);
//                            // if the shadow dataset has values underneath it but the current one doesnt
//                            // then import it so the values also get deleted.
//                            if (groupRow.GetChildRows("VALUES").Length != charRows.Length)
//                            {
//                                for (int gCIndex = 0; gCIndex < charRows.Length; gCIndex++)
//                                {
//                                    try
//                                    {
//                                        if (!_msgDeleteChar.Contains(charRows[gCIndex][COLUMN_C_RID]))
//                                            _hCharDSet.Tables[TABLE_HC].ImportRow(charRows[gCIndex]);
//                                    }
//                                    catch (ConstraintException) { };
//                                }
//                            }

//                            // Delete from DataSet
//                            //						_hCharDSet.Tables[TABLE_HCG].Rows.Find(gRID).Delete();
//                            row.Delete(false);
//                            deletedFromDataSet = true;
//                        }

//                        //Begin TT#1532-MD -jsobek -Add In Use for Header Characteristics
//                        if (FormLoaded)
//                        {
//                            ChangePending = true;
//                        }
//                        //End TT#1532-MD -jsobek -Add In Use for Header Characteristics
//                    }
//                }
//                if(!deletedFromDataSet) 
//                {
//                    row.Delete(false);
//                    deletedFromDataSet = false;
//                }
//            }

//            //Begin TT#1532-MD -jsobek -Add In Use for Header Characteristics
//            //if (FormLoaded)
//            //{
//            //    ChangePending = true;
//            //}
//            //End TT#1532-MD -jsobek -Add In Use for Header Characteristics
//        }
//        // Begin TT#78 - Deleting the Header Char linked to the Global Option Linked Header Char causes problems
//        private bool OkToContinueCharGroupDelete(int aHdrCharGroupRID)
//        {
//            bool okToContinue = false;
//            string errorMessage;
//            try
//            {
//                _removeLinkGlobalOption = false;
//                if (aHdrCharGroupRID == SAB.ClientServerSession.GlobalOptions.HeaderLinkCharacteristicKey)
//                {
//                    string charText = SAB.ClientServerSession.GlobalOptions.HeaderLinkCharacteristicValue;
//                    FunctionSecurityProfile securityAllocationHeaders = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AdminGlobalOptionsAlHeaders);
//                    if (securityAllocationHeaders.AccessDenied || !securityAllocationHeaders.AllowUpdate)
//                    {
//                        errorMessage = String.Format(_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_al_CannotDeleteHeaderCharAsGlobalOption),
//                                         charText);
//                        MessageBox.Show(errorMessage, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
//                    }
//                    else
//                    {
//                        errorMessage = String.Format(_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_al_RemoveHeaderCharAsGlobalOption),
//                                        charText);
//                        errorMessage += Environment.NewLine + MIDText.GetTextOnly((int)eMIDTextCode.msg_ContinueQuestion);

//                        DialogResult diagResult = MessageBox.Show(errorMessage, this.Text, System.Windows.Forms.MessageBoxButtons.YesNo,
//                            System.Windows.Forms.MessageBoxIcon.Question);

//                        if (diagResult == System.Windows.Forms.DialogResult.Yes)
//                        {
//                            _removeLinkGlobalOption = true;
//                            okToContinue = true;
//                        }
//                    }
//                }
//                else
//                {
//                    okToContinue = true;
//                }
//                return okToContinue;
//            }
//            catch
//            {
//                throw;
//            }
//        }
//        // End TT#78
//        #endregion

//        #region VALIDATION
//        private void charGrid_BeforeExitEditMode(object sender, Infragistics.Win.UltraWinGrid.BeforeExitEditModeEventArgs e)
//        {
//            try
//            {   // Begin TT#118 - RMatelic - Can't exit after inserting row without entering data
//                //if (_valueRowInserted)
//                //{
//                //    _valueRowInserted = false;
//                //    e.Cancel = true;
//                //    return;
//                //}
//                //int charGID;
//                //switch (this.charGrid.ActiveCell.Column.Key)
//                //{   
//                //    case COLUMN_G_ID:
//                //        if (!ValidCharacteristicName(charGrid.ActiveRow, charGrid.ActiveCell.Text)) 
//                //        {
//                //            e.Cancel = true;
//                //        }
//                //        break;
//                //    case COLUMN_G_TYPE:
//                //        if (!ValidCharType(charGrid.ActiveRow, charGrid.ActiveCell.Text)) 
//                //        {
//                //            e.Cancel = true;
//                //        }
//                //        break;
//                //    case COLUMN_C_TXTV:
//                //        charGID = (int)charGrid.ActiveRow.ParentRow.Cells[COLUMN_G_RID].Value;
//                //        if (!ValidTextValue(charGrid.ActiveRow, charGrid.ActiveCell.Text, charGID))
//                //        {
//                //            e.Cancel = true;
//                //        }
//                //        break;
//                //    case COLUMN_C_DATEV:
//                //        charGID = (int)charGrid.ActiveRow.ParentRow.Cells[COLUMN_G_RID].Value;
//                //        if (!ValidDateValue(charGrid.ActiveRow, charGrid.ActiveCell.Text, charGID))
//                //        {
//                //            e.Cancel = true;
//                //        }
//                //        break;
//                //    case COLUMN_C_NUMV:
//                //        charGID = (int)charGrid.ActiveRow.ParentRow.Cells[COLUMN_G_RID].Value;
//                //        if (!ValidNumberValue(charGrid.ActiveRow, charGrid.ActiveCell.Text, charGID))
//                //        {
//                //            e.Cancel = true;
//                //        }
//                //        break;
//                //    case COLUMN_C_DOLLARV:
//                //        charGID = (int)charGrid.ActiveRow.ParentRow.Cells[COLUMN_G_RID].Value;
//                //        if (!ValidDollarValue(charGrid.ActiveRow, charGrid.ActiveCell.Text, charGID))
//                //        {
//                //            e.Cancel = true;
//                //        }
//                //        break;	
//                //}
//            }   // End TT#118
//            catch
//            {
//                throw;
//            }
//        }

//        private bool ValidateCharacteristics() {
//            bool isOk = true;
//            RowsCollection rows = charGrid.Rows;
//            int rCount = rows.Count;

//            for(int i = 0; i < rCount; i++)
//            {
//                UltraGridRow row = rows[i];
//                isOk = ValidateEachCharGroup(row);

//                if (!isOk) break;  // Found error.  Stop processing.
//                int charType = Convert.ToInt32( row.Cells[COLUMN_G_TYPE].Value, CultureInfo.CurrentUICulture );
//                int charGID = (int) row.Cells[COLUMN_G_RID].Value ;

//                if (row.HasChild())
//                {
//                    RowsCollection crows = row.ChildBands[0].Rows;
//                    int crCount = crows.Count;

//                    for(int cInd = 0; cInd < crCount; cInd++) 
//                    {
//                        isOk = ValidateEachCharValue(crows[cInd], (eStoreCharType)charType, charGID);
//                        if (!isOk) break;  // Found error.  Stop processing.
//                    }	
//                }
//                if (!isOk) break; 
//            }

//            return isOk;
//        }

//        private bool ValidateEachCharValue(UltraGridRow row, eStoreCharType aCharType, int charGID) {
//            bool isOk = true;	
//            switch (aCharType) {
//                case eStoreCharType.date:
//                    isOk = ValidDateValue(row, row.Cells[COLUMN_C_DATEV].Value, charGID);
//                    if (!isOk) {
//                        charGrid.ActiveRow = row;
//                        row.Expanded = true;
//                        charGrid.ActiveCell = row.Cells[COLUMN_C_DATEV];
//                        charGrid.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.EnterEditMode, false, false);
//                    }
//                    break;
//                case eStoreCharType.text:
//                    isOk = ValidTextValue(row, row.Cells[COLUMN_C_TXTV].Value, charGID);
//                    if (!isOk) {
//                        charGrid.ActiveRow = row;
//                        row.Expanded = true;
//                        charGrid.ActiveCell = row.Cells[COLUMN_C_TXTV];
//                        charGrid.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.EnterEditMode, false, false);
//                    }
//                    break;
//                case eStoreCharType.number:
//                    isOk = ValidNumberValue(row, row.Cells[COLUMN_C_NUMV].Value, charGID);
//                    if (!isOk) {
//                        charGrid.ActiveRow = row;
//                        row.Expanded = true;
//                        charGrid.ActiveCell = row.Cells[COLUMN_C_NUMV];
//                        charGrid.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.EnterEditMode, false, false);
//                    }
//                    break;
//                case eStoreCharType.dollar:
//                    isOk = ValidDollarValue(row, row.Cells[COLUMN_C_DOLLARV].Value, charGID);
//                    if (!isOk) {
//                        charGrid.ActiveRow = row;
//                        row.Expanded = true;
//                        charGrid.ActiveCell = row.Cells[COLUMN_C_DOLLARV];
//                        charGrid.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.EnterEditMode, false, false);
//                    }
//                    break;
//            }
	
//            return isOk; 
//        }

//        private bool ValidateEachCharGroup(UltraGridRow row) {
//            bool isOk = true;
//            isOk = ValidCharacteristicName(row, row.Cells[COLUMN_G_ID].Value);
//            if (!isOk) {
//                charGrid.ActiveRow = row;
//                row.Expanded = true;
//                charGrid.ActiveCell = row.Cells[COLUMN_G_ID];
//                charGrid.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.EnterEditMode, false, false);
//            }
//            // Begin TT#118 - RMatelic - Can't exit after inserting row without entering data - add 'else' statement
//            else
//            {
//                isOk = ValidCharType(row, row.Cells[COLUMN_G_TYPE].Value);
//                if (!isOk)
//                {
//                    charGrid.ActiveRow = row;
//                    row.Expanded = true;
//                    charGrid.ActiveCell = row.Cells[COLUMN_G_TYPE];
//                    charGrid.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.EnterEditMode, false, false);
//                    isOk = false;
//                }
//            }
//            // End TT#118  
//            return isOk; 
//        }


//        private bool ValidCharacteristicName(UltraGridRow row, object cellValue) {
//            bool valid = true;
//            if (cellValue == System.DBNull.Value || cellValue.ToString().Trim().Length == 0)   //TT#118 - RMatelic - add Trim()
//            {
//                string errorMessage = String.Format(
//                    _SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ValueRequired),
//                    "Characteristic Name");
//                MessageBox.Show (errorMessage,  this.Text, 	MessageBoxButtons.OK, MessageBoxIcon.Error);
//                valid = false;
//            }
//            if (valid) {
//                int rID = Convert.ToInt32( row.Cells[COLUMN_G_RID].Value, CultureInfo.CurrentUICulture );
//                int rCount = _DTbl_HGroup.Select(
//                    String.Format("{0} = '{1}' AND {2} <> {3}", 
//                    COLUMN_G_ID, cellValue, COLUMN_G_RID, rID)).Length;
//                if (rCount > 0) {
//                    string errorMessage = _SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_DuplicateCharGroupName);
//                    MessageBox.Show (errorMessage,  this.Text, 	MessageBoxButtons.OK, MessageBoxIcon.Error);
//                    valid = false;
//                }
//            }
//            // BEGIN MID Track #5743 - KJohnson - Allow creation of PO field in header charac, when already defined.
//            if (valid)
//            {
//                // RAISE EVENT FOR ALLOCATION EXPLORER SO HE CAN HANDLE THE NEW COLUMNS.
//                if (row.Cells["HCG_ID"].OriginalValue == System.DBNull.Value)
//                {
//                    if ((OnNewColumnHandler != null) && (OnNewColumnHandler(cellValue.ToString())))
//                    {
//                        string errorMessage = _SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_DupHeaderColumnNameDefined);
//                        errorMessage = errorMessage.Replace("{0}", cellValue.ToString());
//                        MessageBox.Show(errorMessage, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
//                        valid = false;
//                    }
//                }
//                else 
//                {
//                    //Begin TT# 1317 - DOConnell - Get duplicate Key when logging in
//                    //if (row.Cells["HCG_ID"].DataChanged && (string)cellValue != (string)row.Cells["HCG_ID"].OriginalValue)
//                    if ((string)cellValue != (string)row.Cells["HCG_ID"].OriginalValue)
//                    //End TT# 1317 - DOConnell - Get duplicate Key when logging in
//                    {
//                        if ((OnNewColumnHandler != null) && (OnNewColumnHandler(cellValue.ToString())))
//                        {
//                            string errorMessage = _SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_DupHeaderColumnNameDefined);
//                            errorMessage = errorMessage.Replace("{0}", cellValue.ToString());
//                            MessageBox.Show(errorMessage, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
//                            valid = false;
//                        }
//                    }
//                }
//            }
//            // END MID Track #5743 - KJohnson
//            return valid;
//        }

//        private bool ValidCharType(UltraGridRow row, object cellValue) {
//            bool valid = true;
//            if (cellValue == System.DBNull.Value ) {
//                string errorMessage = String.Format(
//                    _SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ValueRequired),
//                    "Characteristic Type");
//                MessageBox.Show (errorMessage,  this.Text, 	MessageBoxButtons.OK, MessageBoxIcon.Error);
//                valid = false;
//            }

//            return valid;
//        }

//        private bool ValidDateValue(UltraGridRow row, object cellValue, int charGID) {
//            bool valid = true;

//            if (cellValue == System.DBNull.Value ) {
//                string errorMessage = String.Format(
//                    _SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ValueRequired),
//                    "Date Value");
//                MessageBox.Show (errorMessage,  this.Text, 	MessageBoxButtons.OK, MessageBoxIcon.Error);
//                valid = false;
//            }
//            if (valid) {
//                // TRY TO CONVERT TO A DATE TO VALIDATE ITS A DATE
//                try {
//                    Convert.ToDateTime(cellValue);
//                }
//                catch (Exception) {
//                    string errorMessage = _SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_DateValuesOnly);	
//                    MessageBox.Show (errorMessage,  this.Text, 	MessageBoxButtons.OK, MessageBoxIcon.Error);
//                    valid = false;
//                    return valid;
//                }

//                int rID = Convert.ToInt32( row.Cells[COLUMN_C_RID].Value, CultureInfo.CurrentUICulture );
//                int rCount = _DTbl_HChar.Select(
//                    String.Format("{0} = {1} AND {2} = #{3}# AND {4} <> {5}",
//                    COLUMN_C_G_RID, charGID, COLUMN_C_DATEV, cellValue, COLUMN_C_RID, rID)).Length;
//                if (rCount > 0) {
//                    string errorMessage = _SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_DuplicateCharValue);
//                    MessageBox.Show (errorMessage,  this.Text, 	MessageBoxButtons.OK, MessageBoxIcon.Error);
//                    valid = false;
//                }
//            }
//            return valid;
//        }

//        private bool ValidTextValue(UltraGridRow row, object cellValue, int charGID) {
//            bool valid = true;	

//            if (cellValue == System.DBNull.Value || cellValue.ToString().Trim().Length == 0) {
//                string errorMessage = String.Format(
//                    _SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ValueRequired),
//                    "Text Value");
//                MessageBox.Show (errorMessage,  this.Text, 	MessageBoxButtons.OK, MessageBoxIcon.Error);
//                valid = false;
//            }

//            if (valid) {
//                int rID = Convert.ToInt32( row.Cells[COLUMN_C_RID].Value, CultureInfo.CurrentUICulture );
//                int rCount = _DTbl_HChar.Select(
//                    String.Format("{0} = {1} AND {2} = '{3}' AND {4} <> {5}" 
//                    ,COLUMN_C_G_RID, charGID, COLUMN_C_TXTV, cellValue, COLUMN_C_RID, rID)).Length;
//                if (rCount > 0) {
//                    string errorMessage = _SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_DuplicateCharValue);
//                    MessageBox.Show (errorMessage,  this.Text, 	MessageBoxButtons.OK, MessageBoxIcon.Error);
//                    valid = false;
//                }
//            }
//            return valid;
//        }

//        private bool ValidNumberValue(UltraGridRow row, object cellValue, int charGID) {
//            bool valid = true;
//            if (cellValue == System.DBNull.Value ) {
//                string errorMessage = String.Format(
//                    _SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ValueRequired),
//                    "Number Value");
//                MessageBox.Show (errorMessage,  this.Text, 	MessageBoxButtons.OK, MessageBoxIcon.Error);
//                valid = false;
//            }

//            if (valid) { 
//                // TRY TO CONVERT TO A DOUBLE TO VALIDATE ITS A NUMBER
//                try {
//                    Convert.ToDouble(cellValue);
//                }
//                catch (Exception) {
//                    string errorMessage = _SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_NumericValuesOnly);
//                    MessageBox.Show (errorMessage,  this.Text, 	MessageBoxButtons.OK, MessageBoxIcon.Error);
//                    valid = false;
//                    return valid;
//                }

//                int rID = Convert.ToInt32( row.Cells[COLUMN_C_RID].Value, CultureInfo.CurrentUICulture );
//                int rCount = _DTbl_HChar.Select(
//                    String.Format("{0} = {1} AND {2} = '{3}' AND {4} <> {5}" 
//                    , COLUMN_C_G_RID, charGID, COLUMN_C_NUMV, cellValue, COLUMN_C_RID, rID)).Length;
//                if (rCount > 0) {
//                    string errorMessage = _SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_DuplicateCharValue);
//                    MessageBox.Show (errorMessage,  this.Text, 	MessageBoxButtons.OK, MessageBoxIcon.Error);
//                    valid = false;
//                }
//            }
//            return valid;
//        }

//        private bool ValidDollarValue(UltraGridRow row, object cellValue, int charGID) {
//            bool valid = true;

//            if (cellValue == System.DBNull.Value ) {
//                string errorMessage = String.Format(
//                    _SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ValueRequired),
//                    "Dollar Value");
//                MessageBox.Show (errorMessage,  this.Text, 	MessageBoxButtons.OK, MessageBoxIcon.Error);
//                valid = false;
//            }

//            if (valid) {
//                // TRY TO CONVERT TO A DOUBLE TO VALIDATE ITS A DOLLAR AMOUNT
//                try {
//                    Convert.ToDouble(cellValue);
//                }
//                catch (Exception) {
//                    string errorMessage = _SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_NumericValuesOnly);
//                    MessageBox.Show (errorMessage,  this.Text, 	MessageBoxButtons.OK, MessageBoxIcon.Error);
//                    valid = false;
//                    return valid;
//                }

//                int rID = Convert.ToInt32( row.Cells[COLUMN_C_RID].Value, CultureInfo.CurrentUICulture );
//                int rCount = _DTbl_HChar.Select(
//                    String.Format("{0} = {1} AND {2} = '{3}' AND {4} <> {5}" 
//                    , COLUMN_C_G_RID, charGID, COLUMN_C_DOLLARV, cellValue, COLUMN_C_RID, rID)).Length;

//                if (rCount > 0) {
//                    string errorMessage = _SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_DuplicateCharValue);
//                    MessageBox.Show (errorMessage,  this.Text, 	MessageBoxButtons.OK, MessageBoxIcon.Error);
//                    valid = false;
//                }
//            }
//            return valid;
//        }
//        #endregion		
		
//        protected override bool SaveChanges()
//        {
//            // Begin TT#118 - RMatelic - Can't exit after inserting row without entering data 
//            if (!ValidateCharacteristics())
//            {
//                ErrorFound = true;
//                return false;
//            }
//            // End TT#118 

//            SaveGridState();

//            charGrid.ResumeRowSynchronization();

//            // update the datasource with all the data in the grid
//            charGrid.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.ExitEditMode);
//            charGrid.UpdateData();
			
//            RowsCollection rColl = charGrid.Rows;
//            int rCount = rColl.Count;
//            for(int i = 0; i < rCount; i++) {
//                UltraGridCell idCell = rColl[i].Cells[COLUMN_G_RID];

//                DataRow[] results = _DTbl_HGroup.Select(
//                    String.Format("{0} = {1}",COLUMN_G_RID, idCell.Value));
//                if(results.Length != 1) {
//                    MessageBox.Show(_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ErrorTryingToUpdateTable));
//                }

//                results[0][COLUMN_G_PROTECT] = 
//                    (Convert.ToBoolean(rColl[i].Cells[CHECKBOX].Value)) ? CHAR_BOOL_TRUE : CHAR_BOOL_FALSE;
//                results[0][COLUMN_G_LIST_IND] = 
//                    (rColl[i].HasChild()) ? CHAR_BOOL_TRUE : CHAR_BOOL_FALSE;
//            }

//            // Do Child deletes
//            DataTable xDataTable = _DTbl_HChar.GetChanges(DataRowState.Deleted);
//            if (xDataTable != null)
//                _hCharDAdapter.HeaderChar_UpdateUsingAdapter(xDataTable);

//            // Do all of parent table
//            xDataTable = _DTbl_HGroup.GetChanges();
//            if (xDataTable != null)
//                _hCharDAdapter.HeaderCharGroup_UpdateUsingAdapter(xDataTable);

//            // The xDataTable now contains the NEW Store Char Group RID from the DB for any new rows.
//            // So we search the REAL dataTable until we find a match, and update it's RID.
//            // this update then cascades down to it's children.
//            if (xDataTable != null) {
//                int nRCount = xDataTable.Rows.Count;
//                DataRowCollection nRc = xDataTable.Rows;

//                for(int newRow = 0; newRow < nRCount; newRow++) {
//                    string name = nRc[newRow][COLUMN_G_ID].ToString();

//                    DataRow[] rows = _DTbl_HGroup.Select(
//                        String.Format("{0} = '{1}' AND {2} < 0", COLUMN_G_ID, name, COLUMN_G_RID));

//                    if(rows.Length == 1) {
//                        rows[0][COLUMN_G_RID] = nRc[newRow][COLUMN_G_RID];
//                    }
//                    else if(rows.Length > 1) {
//                        MessageBox.Show(_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ErrorGettingMultipleRecords));
//                    }
//                }
//                _DTbl_HGroup.AcceptChanges();
//            }

//            // Do Child Inserts
//            xDataTable = _DTbl_HChar.GetChanges(DataRowState.Added);
//            if (xDataTable != null)
//                _hCharDAdapter.HeaderChar_UpdateUsingAdapter(xDataTable);
//            // Do Child Updates
//            xDataTable = _DTbl_HChar.GetChanges(DataRowState.Modified);
//            if (xDataTable != null)
//                _hCharDAdapter.HeaderChar_UpdateUsingAdapter(xDataTable);
			
//            charGrid.DataSource = null;
//            charGrid.ResetDisplayLayout();
//            ApplyAppearance(charGrid);
//            charGrid.DisplayLayout.AddNewBox.Hidden = false;

//            // Begin TT#78 - Ron Matelic - Header Characteristic delete issue when designated as Global Option
//            if (_removeLinkGlobalOption)
//            {
//                UpdateHeaderLinkCharGlobalOption();
//            }
//            // refresh session with cached Global Options
//            _SAB.ClientServerSession.RefreshGlobalOptions();
//            _SAB.ApplicationServerSession.RefreshGlobalOptions();
//            // End TT#78

//            // RAISE EVENT FOR ALLOCATION EXPLORER SO HE CAN HANDLE THE NEW COLUMNS.
//            if(OnSavedHandler != null)
//            {
//                OnSavedHandler();
//            }

//            loadData();
//            RestoreGridState();

//            if (xDataTable != null)
//                xDataTable.Dispose();

//            charGrid.SuspendRowSynchronization();

//            ChangePending = false;  // MID Track #5487 - message after Save
//            return true;
//        }

//        // Begin TT#78 - Ron Matelic - Header Characteristic delete issue when designated as Global Option
//        private void UpdateHeaderLinkCharGlobalOption()
//        {
//            try
//            { 
//                int globalOptionsRID = _SAB.ClientServerSession.GlobalOptions.Key;
//                GlobalOptions globalOptions = new GlobalOptions();

//                globalOptions.OpenUpdateConnection();
//                globalOptions.UpdateHeaderLinkCharacteristic(globalOptionsRID, Include.NoRID);
//                globalOptions.CommitData();
//                globalOptions.CloseUpdateConnection();
//            }
//            catch (Exception ex)
//            {
//                HandleException(ex);
//            }
//        }
//        // End TT#78  

//        private void btnClose_Click(object sender, System.EventArgs e) {
//            try {
//                //this.Close();
//                IClose();
//            }		
//            catch(Exception ex) {
//                HandleException(ex);
//            }
//        }

//        private void btnSave_Click(object sender, System.EventArgs e) {
//            try {
//                if (_hCharDSet.HasChanges() || checkProtectedChanged())
//                {   // Begin TT#118 - RMatelic - Can't exit after inserting row without entering data; comment out 'if...'
//                    //if(ValidateCharacteristics())
//                    //{
//                        SaveChanges();
//                    //}
//                }   // End TT#118
//            }	
//            catch(Exception ex) {
//                HandleException(ex);
//            }
//        }

//        /// <summary>
//        /// Determines the headers the user has access to change
//        /// </summary>
//        /// <param name="ds">The dataset where the available header keys are loaded</param>
//        // BEGIN MID Track #3978 - error deleting characteristic ; comment out the following
////		private void DetermineHeaders(ref DataSet ds)
////		{
////			try
////			{
////				hCharCreateTempKeyTable(ref ds);
////			 
////				AllocationProfileList apl = new AllocationProfileList(eProfileType.Allocation);
////				apl.LoadAll(_SAB, _SAB.ClientServerSession);
////			
////				foreach(AllocationProfile ap in apl) 
////				{
////					ds.Tables[TABLE_H].Rows.Add( new object[] { ap.Key } );
////				}
////				 
////			}	
////			catch(Exception ex) 
////			{
////				HandleException(ex);
////			}
////		}

//        /// <summary>
//        /// Creates a table to house the header id's so we can do a data relationship
//        /// with the other tables that deals with headers, because the header id's in the 
//        /// main table is a string
//        /// </summary>
//        /// <param name="ds"></param>
////		private void hCharCreateTempKeyTable(ref DataSet ds)
////		{
////			if(!ds.Tables.Contains(TABLE_H)) 
////			{
////				DataTable tempDT = MIDEnvironment.CreateDataTable(TABLE_H);
////				tempDT.Columns.Add("KeyH", typeof(System.Int32));
////				tempDT.PrimaryKey = 
////					new DataColumn[] {
////										 tempDT.Columns[COLUMN_H_RID]
////									 };
////				ds.Tables.Add(tempDT);
////			}
////		}
//        // END MID Track #3978 
	
//        #region Windows Form Designer generated code
//        /// <summary>
//        /// Required method for Designer support - do not modify
//        /// the contents of this method with the code editor.
//        /// </summary>
//        private void InitializeComponent() {
//            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
//            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
//            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
//            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
//            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
//            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
//            this.charGrid = new Infragistics.Win.UltraWinGrid.UltraGrid();
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
//                        | System.Windows.Forms.AnchorStyles.Left)
//                        | System.Windows.Forms.AnchorStyles.Right)));
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
//            this.charGrid.BeforeRowInsert += new Infragistics.Win.UltraWinGrid.BeforeRowInsertEventHandler(this.charGrid_BeforeRowInsert);
//            this.charGrid.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.charGrid_InitializeLayout);
//            this.charGrid.BeforeRowsDeleted += new Infragistics.Win.UltraWinGrid.BeforeRowsDeletedEventHandler(this.charGrid_BeforeRowsDeleted);
//            this.charGrid.BeforeExitEditMode += new Infragistics.Win.UltraWinGrid.BeforeExitEditModeEventHandler(this.charGrid_BeforeExitEditMode);
//            this.charGrid.AfterRowInsert += new Infragistics.Win.UltraWinGrid.RowEventHandler(this.charGrid_AfterRowInsert);
//            this.charGrid.InitializeRow += new Infragistics.Win.UltraWinGrid.InitializeRowEventHandler(this.charGrid_InitializeRow);
//            this.charGrid.CellChange += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.charGrid_CellChange);
//            // 
//            // btnSave
//            // 
//            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
//            this.btnSave.Location = new System.Drawing.Point(333, 336);
//            this.btnSave.Name = "btnSave";
//            this.btnSave.Size = new System.Drawing.Size(75, 23);
//            this.btnSave.TabIndex = 1;
//            this.btnSave.Text = "Save";
//            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
//            // 
//            // btnClose
//            // 
//            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
//            this.btnClose.Location = new System.Drawing.Point(418, 336);
//            this.btnClose.Name = "btnClose";
//            this.btnClose.Size = new System.Drawing.Size(75, 23);
//            this.btnClose.TabIndex = 2;
//            this.btnClose.Text = "Close";
//            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
//            // 
//            // HeaderCharacteristicMaint
//            // 
//            this.AllowDragDrop = true;
//            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
//            this.ClientSize = new System.Drawing.Size(497, 361);
//            this.Controls.Add(this.btnClose);
//            this.Controls.Add(this.btnSave);
//            this.Controls.Add(this.charGrid);
//            this.Name = "HeaderCharacteristicMaint";
//            this.Text = "Form1";
//            this.Load += new System.EventHandler(this.HeaderCharacteristicMaint_Load);
//            this.Closing += new System.ComponentModel.CancelEventHandler(this.HeaderCharacteristicMaint_Closing);
//            this.Controls.SetChildIndex(this.charGrid, 0);
//            this.Controls.SetChildIndex(this.btnSave, 0);
//            this.Controls.SetChildIndex(this.btnClose, 0);
//            ((System.ComponentModel.ISupportInitialize)(this.utmMain)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.charGrid)).EndInit();
//            this.ResumeLayout(false);

//        }
//        #endregion

//        //Begin TT#1532-MD -jsobek -Add In Use for Header Characteristics
//        private void menuInUseInfo_Click(object sender, EventArgs e)
//        {
//            eProfileType etype = eProfileType.HeaderChar; 
//            ArrayList _ridList = new ArrayList();
      
//            UltraGridColumn scRidColumn = charGrid.DisplayLayout.Bands[1].Columns["HC_RID"];
//            foreach (UltraGridRow row in charGrid.Selected.Rows)
//            {
//                if (row.Band.Key == "VALUES")
//                {
//                    int scRid = Convert.ToInt32(row.GetCellValue(scRidColumn), CultureInfo.CurrentUICulture);
//                    if (row.Band != charGrid.DisplayLayout.Bands[1]) continue;
//                    if (scRid > 0)
//                    {
//                        _ridList.Add(scRid);
//                    }
//                }
//            }
   
//            UltraGridColumn scgRidColumn = charGrid.DisplayLayout.Bands[0].Columns["HCG_RID"];
//            foreach (UltraGridRow row in charGrid.Selected.Rows)
//            {
//                if (row.Band.Key == "HEADER_CHAR_GROUP")
//                {

//                    etype = eProfileType.HeaderCharGroup;
//                    int scgRid = Convert.ToInt32(row.GetCellValue(scgRidColumn), CultureInfo.CurrentUICulture);
//                    if (row.Band != charGrid.DisplayLayout.Bands[0]) continue;
//                    if (scgRid > 0)
//                    {
//                        _ridList.Add(scgRid);
//                    }
//                }
//            }
   
//            if (_ridList.Count > 0)
//            {

//                string inUseTitle = InUseUtility.GetInUseTitleFromProfileType(etype); 
//                bool display = false;
//                DisplayInUseForm(_ridList, etype, inUseTitle, ref display, true);

//            }
//        }
//        private bool IsInUse(eProfileType etype, int aRID)
//        {
//            bool isInUse = false;
//            ArrayList ridList = new ArrayList();
//            ridList.Add(aRID);
//            //If no RID is selected do nothing.
//            if (ridList.Count > 0)
//            {
//                string inUseTitle = InUseUtility.GetInUseTitleFromProfileType(etype); 
//                DisplayInUseForm(ridList, etype, inUseTitle, false, out isInUse);
//            }
//            return isInUse;
//        }
//        //End TT#1532-MD -jsobek -Add In Use for Header Characteristics
//    }
//}