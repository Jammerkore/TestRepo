using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

using MIDRetail.DataCommon;
using MIDRetail.Data;
using MIDRetail.Business;

namespace MIDRetail.Windows.Controls
{
    public partial class MIDGridControl : UserControl
    {
        public MIDGridControl()
        {
            InitializeComponent();
        }


        public filterTypes filterType = null;
        public eLayoutID layoutID = eLayoutID.NotDefined; //TT#1443-MD -jsobek -Audit Filters
        public eLayoutID layoutMenuID = eLayoutID.NotDefined; //TT#1443-MD -jsobek -Audit Filters

        public SessionAddressBlock SAB = null;
        public MemoryStream customLayoutStream = null;
        public MemoryStream customLayoutMenuStream = null; //TT#1455-MD -jsobek -Search Results - Grid Layout issues -Save Find Menu
		private bool _settingValues = false;
        private DataSet _ds;
        private bool doLayouts = true;
        public bool doResize = true;
        public bool doIncludeHeaderWhenResizing = false;
   
        public bool ShowToolbar
        {
            get
            {
                return this.ultraToolbarsManager1.Toolbars[0].Visible;
            }
            set
            {
                this.ultraToolbarsManager1.Toolbars[0].Visible = value;
            }
        }

        public void BindGrid(DataSet aDataSet)
        {
            try
            {
                ultraGrid1.DataSource = null;
                if (aDataSet != null)
                {
                    this._ds = aDataSet;
                    BindingSource bs = new BindingSource(aDataSet, aDataSet.Tables[0].TableName);
                    this.ultraGrid1.DataSource = bs;
                    if (doResize)
                    {
                        this.ultraGrid1.BeginUpdate();
                        this.ultraGrid1.SuspendRowSynchronization();
                        if (_ds.Tables[0].Rows.Count < 500)
                        {
                            this.ultraGrid1.DisplayLayout.Bands[0].PerformAutoResizeColumns(false, Infragistics.Win.UltraWinGrid.PerformAutoSizeType.AllRowsInBand, doIncludeHeaderWhenResizing);
                        }
                        else
                        {
                            //delayedResizing = true;   
                            foreach (Infragistics.Win.UltraWinGrid.UltraGridColumn column in this.ultraGrid1.DisplayLayout.Bands[0].Columns)
                            {
                                column.PerformAutoResize(100);
                            }
                        }
                        this.ultraGrid1.ResumeRowSynchronization();
                        this.ultraGrid1.EndUpdate();
                    }
                    if (doLayouts)
                    {
                        SetLayoutForReset();
                        LoadCustomLayoutOnGrid();
                    }

                    SetLayoutForReset();
                    SelectFirstRow();

                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex);
            }
        }

        #region Layout

        private System.IO.MemoryStream gridManagerSettingsMemoryStreamForReset = new System.IO.MemoryStream();  //used to undo user settings, and restore the grid back to the designed settings
        private System.IO.MemoryStream menuManagerSettingsMemoryStreamForReset = new System.IO.MemoryStream();  //used to undo user settings, and restore the menu back to the designed settings //TT#1455-MD -jsobek -Search Results - Grid Layout issues -Save Find Menu

        private void LoadCustomLayoutOnGrid()
        {
            if (customLayoutStream != null && customLayoutStream.Length > 0)
            {
                customLayoutStream.Position = 0;
                this.ultraGrid1.DisplayLayout.Load(customLayoutStream);
            }
            //Begin TT#1455-MD -jsobek -Search Results - Grid Layout issues -Save Find Menu
            if (customLayoutMenuStream != null && customLayoutMenuStream.Length > 0)
            {
                customLayoutMenuStream.Position = 0;
                this.ultraToolbarsManager1.LoadFromBinary(customLayoutMenuStream);
            }
            //End TT#1455-MD -jsobek -Search Results - Grid Layout issues -Save Find Menu
            LoadMenuSettingsFromGrid();
        }

        private void SetLayoutForReset()
        {

            if (this.gridManagerSettingsMemoryStreamForReset.Length == 0)
            {
                this.ultraGrid1.DisplayLayout.Save(this.gridManagerSettingsMemoryStreamForReset);
            }
            //Begin TT#1455-MD -jsobek -Search Results - Grid Layout issues -Save Find Menu
            if (this.menuManagerSettingsMemoryStreamForReset.Length == 0)
            {
                this.ultraToolbarsManager1.SaveAsBinary(menuManagerSettingsMemoryStreamForReset, true);
            }
            //End TT#1455-MD -jsobek -Search Results - Grid Layout issues -Save Find Menu
        }


        private void LoadMenuSettingsFromGrid()
        {
            _settingValues = true;
            if (this.ultraGrid1.DisplayLayout.GroupByBox.Hidden)
            {
                ((Infragistics.Win.UltraWinToolbars.StateButtonTool)this.ultraToolbarsManager1.Tools["gridShowGroupArea"]).Checked = false;
            }
            else
            {
                ((Infragistics.Win.UltraWinToolbars.StateButtonTool)this.ultraToolbarsManager1.Tools["gridShowGroupArea"]).Checked = true;
            }
      
            if (this.ultraGrid1.DisplayLayout.Override.FilterUIType == Infragistics.Win.UltraWinGrid.FilterUIType.FilterRow)
            {
                    ((Infragistics.Win.UltraWinToolbars.StateButtonTool)this.ultraToolbarsManager1.Tools["gridShowFilterRow"]).Checked = true;
            }
            else
            {
                    ((Infragistics.Win.UltraWinToolbars.StateButtonTool)this.ultraToolbarsManager1.Tools["gridShowFilterRow"]).Checked = false;
            }
            _settingValues = false;
        }


        private void LayoutSave()
        {
            try
            {
                if (this.filterType != null)
                {
                    if (this.layoutMenuID != eLayoutID.NotDefined)
                    {
                        //Begin TT#1455-MD -jsobek -Search Results - Grid Layout issues -Save Find Menu
                        customLayoutMenuStream = new MemoryStream();
                        this.ultraToolbarsManager1.SaveAsBinary(customLayoutMenuStream, true);
                        InfragisticsLayoutData layoutData = new InfragisticsLayoutData();
                        layoutData.InfragisticsLayout_Save(SAB.ClientServerSession.UserRID, this.layoutMenuID, customLayoutMenuStream);
                        //End TT#1455-MD -jsobek -Search Results - Grid Layout issues -Save Find Menu
                    }

                    if (this.layoutID != eLayoutID.NotDefined)
                    {
                        customLayoutStream = new MemoryStream();
                        this.ultraGrid1.DisplayLayout.Save(customLayoutStream);
                        InfragisticsLayoutData layoutData = new InfragisticsLayoutData();
                        layoutData.InfragisticsLayout_Save(SAB.ClientServerSession.UserRID, this.layoutID, customLayoutStream);
                    }

                    if (this.layoutMenuID != eLayoutID.NotDefined || this.layoutID != eLayoutID.NotDefined)
                    {
                        RaiseSaveLayoutEvent(customLayoutStream, customLayoutMenuStream, this.layoutID, this.layoutMenuID);
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex);
            }
        }


        private void LayoutReset()
        {
            try
            {
                this.gridManagerSettingsMemoryStreamForReset.Position = 0;
                this.ultraGrid1.DisplayLayout.Load(gridManagerSettingsMemoryStreamForReset);

                //Begin TT#1455-MD -jsobek -Search Results - Grid Layout issues -Save Find Menu
                this.menuManagerSettingsMemoryStreamForReset.Position = 0;
                this.ultraToolbarsManager1.LoadFromBinary(menuManagerSettingsMemoryStreamForReset);
                //End TT#1455-MD -jsobek -Search Results - Grid Layout issues -Save Find Menu
                LoadMenuSettingsFromGrid();
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex);
            }
        }
        private void LayoutLoad()
        {
            try
            {
                if (this.layoutID != eLayoutID.NotDefined)
                {
                    InfragisticsLayoutData layoutData = new InfragisticsLayoutData();
                    InfragisticsLayout gridLayout = layoutData.InfragisticsLayout_Read(SAB.ClientServerSession.UserRID, this.layoutID);
                    customLayoutStream = gridLayout.LayoutStream;
                }

                if (this.layoutMenuID != eLayoutID.NotDefined)
                {
                    InfragisticsLayoutData layoutData = new InfragisticsLayoutData();
                    //Begin TT#1455-MD -jsobek -Search Results - Grid Layout issues -Save Find Menu          
                    InfragisticsLayout menuLayout = layoutData.InfragisticsLayout_Read(SAB.ClientServerSession.UserRID, this.layoutMenuID);
                    customLayoutMenuStream = menuLayout.LayoutStream;
                    //End TT#1455-MD -jsobek -Search Results - Grid Layout issues -Save Find Menu
                }


                if (this.layoutID != eLayoutID.NotDefined || this.layoutMenuID != eLayoutID.NotDefined)
                {
                    LoadCustomLayoutOnGrid();
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex);
            }
        }

        private void LayoutRemove()
        {
            try
            {
                this.gridManagerSettingsMemoryStreamForReset.Position = 0;
                this.ultraGrid1.DisplayLayout.Load(gridManagerSettingsMemoryStreamForReset);

                //Begin TT#1455-MD -jsobek -Search Results - Grid Layout issues -Save Find Menu
                this.menuManagerSettingsMemoryStreamForReset.Position = 0;
                this.ultraToolbarsManager1.LoadFromBinary(menuManagerSettingsMemoryStreamForReset);
                //End TT#1455-MD -jsobek -Search Results - Grid Layout issues -Save Find Menu

                LoadMenuSettingsFromGrid();
                if (this.filterType != null)
                {
                    if (this.layoutMenuID != eLayoutID.NotDefined)
                    {
                        InfragisticsLayoutData layoutData = new InfragisticsLayoutData();
                        layoutData.InfragisticsLayout_Delete(SAB.ClientServerSession.UserRID, this.layoutMenuID); //TT#1455-MD -jsobek -Search Results - Grid Layout issues -Save Find Menu
                 
                    }
                    if (this.layoutID != eLayoutID.NotDefined)
                    {
                        InfragisticsLayoutData layoutData = new InfragisticsLayoutData();
                        layoutData.InfragisticsLayout_Delete(SAB.ClientServerSession.UserRID, this.layoutID);
                    }

                    if (this.layoutID != eLayoutID.NotDefined || this.layoutMenuID != eLayoutID.NotDefined)
                    {
                        RaiseRemoveLayoutEvent(this.layoutID, this.layoutMenuID);
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex);
            }
        }

        #endregion

        private void SelectFirstRow()
        {
            if (this.ultraGrid1.Rows.Count > 0)
            {
                this.ultraGrid1.Rows[0].Selected = true;
                //SelectedRowChanged();
            }
        }
        private void ultraGrid1_AfterSelectChange(object sender, Infragistics.Win.UltraWinGrid.AfterSelectChangeEventArgs e)
        {
            SelectedRowChanged();
        }
        private void SelectedRowChanged()
        {
            DataRow drFirstSelected = GetFirstSelectedRow();
            if (drFirstSelected != null)
            {
                RaiseSelectedRowChangedEvent(drFirstSelected);
            }
        }
        public DataRow GetFirstSelectedRow()
        {
            if (this.ultraGrid1.Selected.Rows.Count > 0)
            {
                Infragistics.Win.UltraWinGrid.UltraGridRow urFirst = this.ultraGrid1.Selected.Rows[0];
                if (urFirst.ListObject != null)
                {
                    return ((DataRowView)urFirst.ListObject).Row;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
        public List<DataRow> GetSelectedRows()
        {
            List<DataRow> selectedFieldList = new List<DataRow>();

            foreach (Infragistics.Win.UltraWinGrid.UltraGridRow urSelected in this.ultraGrid1.Selected.Rows)
            {
                if (urSelected.ListObject != null)
                {
                    DataRow drSelected = ((DataRowView)urSelected.ListObject).Row;
                    selectedFieldList.Add(drSelected);
                }
            }

            return selectedFieldList;
        }
 

        public string exportObjectName = "object";

        public void HideLayoutOnMenu()
        {
            this.ultraToolbarsManager1.Tools["mnuLayout"].SharedProps.Visible = false;
            this.doLayouts = false;
        }
        public void HideGridMenu()
        {
            this.ultraToolbarsManager1.Tools["gridMenuPopup"].SharedProps.Visible = false;
        }
        public void SkipAutoSelect()
        {
            this.ultraGrid1.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.CellSelect;
        }
        public void ResizeColumns()
        {
            this.ultraGrid1.DisplayLayout.Bands[0].PerformAutoResizeColumns(false, Infragistics.Win.UltraWinGrid.PerformAutoSizeType.AllRowsInBand);
        }


        private void ultraToolbarsManager1_ToolClick(object sender, Infragistics.Win.UltraWinToolbars.ToolClickEventArgs e)
        {
            if (_settingValues == false)
            {
                string date = Convert.ToString(System.DateTime.Now);
                string subject = exportObjectName;
                string fileName = exportObjectName;
                if (filterType != null)
                {
                    subject = filterType.exportFileName;
                    fileName = filterType.exportFileName + " " + date + ".xls";
                }
                switch (e.Tool.Key)
                {
                    case "btnLocate":
                        Locate();
                        break;
                    case "btnCopy":
                        CopyNode();
                        break;

                    #region "Grid Tools"
                    case "btnSaveLayout":
                        LayoutSave();
                        break;
                    case "btnRemoveLayout":
                        LayoutRemove();
                        break;
                    case "btnLoadLayout":
                        LayoutLoad();
                        break;
                    case "btnResetLayout":
                        LayoutReset();
                        break;
                    case "gridSearchFindButton":
                        SharedControlRoutines.SearchGrid(ultraGrid1, (((Infragistics.Win.UltraWinToolbars.TextBoxTool)this.ultraToolbarsManager1.Toolbars["Grid Search Toolbar"].Tools["gridSearchText"]).Text));
                        break;
                    case "gridSearchClearButton":
                        Infragistics.Win.UltraWinToolbars.TextBoxTool t = (Infragistics.Win.UltraWinToolbars.TextBoxTool)this.ultraToolbarsManager1.Toolbars["Grid Search Toolbar"].Tools["gridSearchText"];
                        t.Text = "";
                        SharedControlRoutines.ClearGridSearchResults(ultraGrid1);
                        break;

                    case "gridShowSearchToolbar":
                        this.ultraToolbarsManager1.Toolbars["Grid Search Toolbar"].Visible = !this.ultraToolbarsManager1.Toolbars["Grid Search Toolbar"].Visible;
                        break;

                    case "gridShowGroupArea":
                        this.ultraGrid1.DisplayLayout.GroupByBox.Hidden = !this.ultraGrid1.DisplayLayout.GroupByBox.Hidden;
                        break;

                    case "gridShowFilterRow":
                        if (this.ultraGrid1.DisplayLayout.Override.FilterUIType == Infragistics.Win.UltraWinGrid.FilterUIType.FilterRow)
                        {
                            this.ultraGrid1.DisplayLayout.Override.FilterUIType = Infragistics.Win.UltraWinGrid.FilterUIType.HeaderIcons;
                        }
                        else
                        {
                            this.ultraGrid1.DisplayLayout.Override.FilterUIType = Infragistics.Win.UltraWinGrid.FilterUIType.FilterRow;
                        }
                        break;

                    case "gridExportSelected":
                        if (this.filterType == filterTypes.AuditFilter) //TT#4280 -jsobek -Exporting rows in Audit to Excel
                        {
                            SharedControlRoutines.exportHelper.ExportSelectedRowsToExcel(this.ultraGrid1, string.Empty, string.Empty, string.Empty, true);
                        }
                        else
                        {
                            SharedControlRoutines.exportHelper.ExportSelectedRowsToExcel(this.ultraGrid1);
                        }
                        break;

                    case "gridExportAll":
                        SharedControlRoutines.exportHelper.ExportAllRowsToExcel(this.ultraGrid1);
                        break;

                    case "gridEmailSelectedRows":
                        if (this.filterType == filterTypes.AuditFilter) //TT#4280 -jsobek -Exporting rows in Audit to Excel
                        {
                            SharedControlRoutines.exportHelper.EmailSelectedRows(subject, fileName, this.ultraGrid1, string.Empty, string.Empty, string.Empty, true); //TT#1280-MD -jsobek -Audit Grid does not export correctly
                        }
                        else 
                        {
                            SharedControlRoutines.exportHelper.EmailSelectedRows(subject, fileName, this.ultraGrid1);
                        }
                        break;

                    case "gridEmailAllRows":
                        SharedControlRoutines.exportHelper.EmailAllRows(subject, fileName, this.ultraGrid1); //TT#4280 -jsobek -Exporting rows in Audit to Excel
                        break;

                    case "gridChooseColumns":
                        this.ultraGrid1.ShowColumnChooser("Choose Columns");
                        break;

                    #endregion

                }
            }
        }

 

        private void ultraToolbarsManager1_ToolKeyPress(object sender, Infragistics.Win.UltraWinToolbars.ToolKeyPressEventArgs e)
        {
            switch (e.Tool.Key)
            {
                case "gridSearchText":
                    if (e.KeyChar == (char)Keys.Return)
                    {
                        MIDRetail.Windows.Controls.SharedControlRoutines.SearchGrid(this.ultraGrid1, ((Infragistics.Win.UltraWinToolbars.TextBoxTool)e.Tool).Text);
                    }
                    break;
          
            }
        }


		private bool _showColumnChooser = true;
        public bool ShowColumnChooser
        {
            get
            {
                return _showColumnChooser;
            }
            set
            {
                _showColumnChooser = value;
            }
        }
  
        private void MIDGrid_Load(object sender, EventArgs e)
        {
            Infragistics.Shared.ResourceCustomizer rc = Infragistics.Win.UltraWinGrid.Resources.Customizer;
            rc.SetCustomizedString("GroupByBoxDefaultPrompt", "Drag a column here to group by that column.");
            rc.SetCustomizedString("ColumnChooserButtonToolTip", "Click here to show/hide columns");
            rc.SetCustomizedString("ColumnChooserDialogCaption", "Choose Columns");
            if (_showColumnChooser)
            {
                this.ultraGrid1.DisplayLayout.Override.RowSelectorHeaderStyle = Infragistics.Win.UltraWinGrid.RowSelectorHeaderStyle.ColumnChooserButton;
            }
            else
            {
                this.ultraGrid1.DisplayLayout.Override.RowSelectorHeaderStyle = Infragistics.Win.UltraWinGrid.RowSelectorHeaderStyle.SeparateElement;
            }

            this.ultraGrid1.DisplayLayout.Override.AllowAddNew = Infragistics.Win.UltraWinGrid.AllowAddNew.No;
            this.ultraGrid1.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            this.ultraGrid1.DisplayLayout.Override.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.True;

            //if (_allowEdit)
            //{
            //    this.ultraGrid1.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.True;
            //}
            //else
            //{
            //    this.ultraGrid1.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.False;
            //}

        }

        public delegate void gridInitializeLayoutCallbackDelegate(ref Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e);
        public gridInitializeLayoutCallbackDelegate gridInitializeLayoutCallback = null;

        public delegate void gridAfterBindingDelegate();
        public gridAfterBindingDelegate gridAfterBinding = null;

        private void ultraGrid1_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            //e.Layout.Bands[0].Columns["AUDIT_DATETIME"].Format = "MM/dd/yyyy HH:mm:ss";
            foreach (string s in columnsToHide)
            {
                e.Layout.Bands[0].Columns[s].Hidden = true;
                //if (ExceptionHandler.InDebugMode)
                //{
                //    e.Layout.Bands[0].Columns[s].ExcludeFromColumnChooser = Infragistics.Win.UltraWinGrid.ExcludeFromColumnChooser.False;
                //}
                //else
                //{
                    e.Layout.Bands[0].Columns[s].ExcludeFromColumnChooser = Infragistics.Win.UltraWinGrid.ExcludeFromColumnChooser.True;
                //}
            }

            if (gridInitializeLayoutCallback != null)
            {
                gridInitializeLayoutCallback.Invoke(ref e);
            }


            //if (this.gridManagerSettingsMemoryStreamForReset.Length == 0)
            //{
            //    this.ultraGrid1.DisplayLayout.Save(this.gridManagerSettingsMemoryStreamForReset);
            //}
            ////Begin TT#1455-MD -jsobek -Search Results - Grid Layout issues -Save Find Menu
            //if (this.menuManagerSettingsMemoryStreamForReset.Length == 0)
            //{
            //    this.ultraToolbarsManager1.SaveAsBinary(menuManagerSettingsMemoryStreamForReset, true);
            //}
            ////End TT#1455-MD -jsobek -Search Results - Grid Layout issues -Save Find Menu
        }

        public delegate void gridBeforeRowExpandedCallbackDelegate(ref Infragistics.Win.UltraWinGrid.CancelableRowEventArgs e, DataSet ds);
        public gridBeforeRowExpandedCallbackDelegate gridBeforeRowExpandedCallback = null;
        private void ultraGrid1_BeforeRowExpanded(object sender, Infragistics.Win.UltraWinGrid.CancelableRowEventArgs e)
        {
            if (gridBeforeRowExpandedCallback != null)
            {
                gridBeforeRowExpandedCallback.Invoke(ref e, _ds);
            }
        }

        public delegate void gridBeforeExitEditModeCallbackDelegate(ref Infragistics.Win.UltraWinGrid.BeforeExitEditModeEventArgs e, Infragistics.Win.UltraWinGrid.UltraGridRow activeRow, Infragistics.Win.UltraWinGrid.UltraGridCell activeCell, DataSet ds);
        public gridBeforeExitEditModeCallbackDelegate gridBeforeExitEditModeCallback = null;
        private void ultraGrid1_BeforeExitEditMode(object sender, Infragistics.Win.UltraWinGrid.BeforeExitEditModeEventArgs e)
        {
            if (gridBeforeExitEditModeCallback != null)
            {
                gridBeforeExitEditModeCallback.Invoke(ref e, this.ultraGrid1.ActiveRow, this.ultraGrid1.ActiveCell, _ds);
            }
        }

        public delegate void gridInitializeRowCallbackDelegate(ref Infragistics.Win.UltraWinGrid.InitializeRowEventArgs e, DataSet ds);
        public gridInitializeRowCallbackDelegate gridInitializeRowCallback = null;
        private void ultraGrid1_InitializeRow(object sender, Infragistics.Win.UltraWinGrid.InitializeRowEventArgs e)
        {
            if (gridInitializeRowCallback != null)
            {
                gridInitializeRowCallback.Invoke(ref e, _ds);
            }
        }

        public delegate void gridBeforeEnterEditModeCallbackDelegate(Infragistics.Win.UltraWinGrid.UltraGridCell activeCell, ref System.ComponentModel.CancelEventArgs e);
        public gridBeforeEnterEditModeCallbackDelegate gridBeforeEnterEditModeCallback = null;
        private void ultraGrid1_BeforeEnterEditMode(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (gridBeforeEnterEditModeCallback != null)
            {
                gridBeforeEnterEditModeCallback.Invoke(this.ultraGrid1.ActiveCell, ref e);
            }
        }

        
        public bool ExitEditModeOnReturnKeyPress = false;
        private void ultraGrid1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == (char)Keys.Return)
            {
                if (ExitEditModeOnReturnKeyPress)
                {
                    this.ultraGrid1.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.ExitEditMode);
                }
            }
        }


        private List<string> columnsToHide = new List<string>();
        public void HideColumn(string dataFieldName)
        {
            columnsToHide.Add(dataFieldName);
        }

        public void ShowButton(string btnName)
        {
            this.ultraToolbarsManager1.Tools[btnName].SharedProps.Visible = true;
        }


        private void Locate()
        {
            RaiseClearSelectedNodeEvent();
            foreach (Infragistics.Win.UltraWinGrid.UltraGridRow urSelected in this.ultraGrid1.Selected.Rows)
            {
                if (urSelected.ListObject != null)
                {
                    DataRow drSelected = ((DataRowView)urSelected.ListObject).Row;
                    RaiseLocateEvent(drSelected);
                }         
            }
        }

        private void CopyNode()
        {
            try
            {
                List<int> drSelectedList = new List<int>();
                foreach (Infragistics.Win.UltraWinGrid.UltraGridRow urSelected in this.ultraGrid1.Selected.Rows)
                {
                    if (urSelected.ListObject != null)
                    {
                        DataRow drSelected = ((DataRowView)urSelected.ListObject).Row;
                        drSelectedList.Add((int)drSelected["HN_RID"]);
                    }
                }

                IDataObject ido = new DataObject();
                ido.SetData(typeof(List<int>), drSelectedList);
                Clipboard.SetDataObject(ido, true);

                RaiseCopyActionEvent();
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex);
            }
        }


        #region Events
        //Copy Action Event
        public delegate void CopyActionEventHandler(object sender, CopyActionEventArgs e);
        public event CopyActionEventHandler CopyActionEvent;
        public void RaiseCopyActionEvent()
        {
            if (CopyActionEvent != null)
                CopyActionEvent(new object(), new CopyActionEventArgs());
        }
        public class CopyActionEventArgs
        {
            public CopyActionEventArgs() { }
        }
        //Locate Event
        public delegate void LocateEventHandler(object sender, LocateEventArgs e);
        public event LocateEventHandler LocateEvent;
        public void RaiseLocateEvent(DataRow drSelected)
        {
            if (LocateEvent != null)
                LocateEvent(new object(), new LocateEventArgs(drSelected));
        }
        public class LocateEventArgs
        {
            public LocateEventArgs(DataRow drSelected) { this.drSelected = drSelected; }
            public DataRow drSelected { get; private set; }
        }
        //Clear Selected Node Event
        public delegate void ClearSelectedNodeEventHandler(object sender, ClearSelectedNodeEventArgs e);
        public event ClearSelectedNodeEventHandler ClearSelectedNodeEvent;
        public void RaiseClearSelectedNodeEvent()
        {
            if (ClearSelectedNodeEvent != null)
                ClearSelectedNodeEvent(new object(), new ClearSelectedNodeEventArgs());
        }
        public class ClearSelectedNodeEventArgs
        {
            public ClearSelectedNodeEventArgs() { }
        }
        //Selected Row Changed Event
        public delegate void SelectedRowChangedEventHandler(object sender, SelectedRowChangedEventArgs e);
        public event SelectedRowChangedEventHandler SelectedRowChangedEvent;
        public virtual void RaiseSelectedRowChangedEvent(DataRow drSelected)
        {

            if (SelectedRowChangedEvent != null)
                SelectedRowChangedEvent(this, new SelectedRowChangedEventArgs(drSelected));
        }
        public class SelectedRowChangedEventArgs
        {
            public SelectedRowChangedEventArgs(DataRow drSelected) { this.drSelected = drSelected; }
            public DataRow drSelected { get; private set; } // readonly
        }

        // Begin TT#1807-MD - JSmith - Store Profiles - Filtering - Field does not change to selected field when filtering. Custom drop down has a selection for ((DB Null))
        public delegate void AfterRowFilterChangedEventHandler(object sender, Infragistics.Win.UltraWinGrid.AfterRowFilterChangedEventArgs e);
        public AfterRowFilterChangedEventHandler AfterRowFilterChangedEvent = null;
        private void ultraGrid1_AfterRowFilterChanged(object sender, Infragistics.Win.UltraWinGrid.AfterRowFilterChangedEventArgs e)
        {
            if (AfterRowFilterChangedEvent != null)
            {
                AfterRowFilterChangedEvent.Invoke(this, e);
            }
        }
        // End TT#1807-MD - JSmith - Store Profiles - Filtering - Field does not change to selected field when filtering. Custom drop down has a selection for ((DB Null))

        //Remove Layout Event
        public delegate void RemoveLayoutEventHandler(object sender, RemoveLayoutEventArgs e);
        public event RemoveLayoutEventHandler RemoveLayoutEvent;
        public void RaiseRemoveLayoutEvent(eLayoutID layoutID, eLayoutID layoutMenuID)
        {
            if (RemoveLayoutEvent != null)
                RemoveLayoutEvent(new object(), new RemoveLayoutEventArgs(layoutID, layoutMenuID));
        }
        public class RemoveLayoutEventArgs
        {
            public RemoveLayoutEventArgs(eLayoutID layoutID, eLayoutID layoutMenuID) { this.layoutID = layoutID; this.layoutMenuID = layoutMenuID; }

            public eLayoutID layoutID { get; private set; } // readonly
            public eLayoutID layoutMenuID { get; private set; } // readonly
        }

        //Save Layout Event
        public delegate void SaveLayoutEventHandler(object sender, SaveLayoutEventArgs e);
        public event SaveLayoutEventHandler SaveLayoutEvent;
        public void RaiseSaveLayoutEvent(MemoryStream customLayout, MemoryStream customMenuLayout, eLayoutID layoutID, eLayoutID layoutMenuID)
        {
            if (SaveLayoutEvent != null)
                SaveLayoutEvent(new object(), new SaveLayoutEventArgs(customLayout, customMenuLayout, layoutID, layoutMenuID));
        }
        public class SaveLayoutEventArgs
        {
            public SaveLayoutEventArgs(MemoryStream customLayout, MemoryStream customMenuLayout, eLayoutID layoutID, eLayoutID layoutMenuID) { this.customLayout = customLayout; this.customMenuLayout = customMenuLayout; this.layoutID = layoutID; this.layoutMenuID = layoutMenuID; }
            public MemoryStream customLayout { get; private set; } // readonly
            public MemoryStream customMenuLayout { get; private set; } // readonly //TT#1455-MD -jsobek -Search Results - Grid Layout issues -Save Find Menu

            public eLayoutID layoutID { get; private set; } // readonly
            public eLayoutID layoutMenuID { get; private set; } // readonly
        }
        #endregion

   

  
    }
}
