using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MIDRetail.Windows.Controls
{
    public partial class PlanChainLadderToolbarControl : UserControl
    {
        public PlanChainLadderToolbarControl()
        {
            InitializeComponent();

            //set this to false if we do not want to include the ladder view chart
            this.ultraToolbarsManager1.Tools["showChart"].SharedProps.Visible = true;
        }

        public void LoadViewList(DataTable dtViewList, int initialViewRID)
        {
            Infragistics.Win.UltraWinToolbars.ComboBoxTool cbo = (Infragistics.Win.UltraWinToolbars.ComboBoxTool)this.ultraToolbarsManager1.Tools["viewComboBox"];
            cbo.ValueList.ValueListItems.Clear();
            foreach (DataRow dr in dtViewList.Rows)
            {
                int viewRID;
                string viewName;

                viewRID = (int)dr["VIEW_RID"];
                viewName = (string)dr["DISPLAY_ID"];

                cbo.ValueList.ValueListItems.Add(viewRID, viewName);
            }
            cbo.Value = initialViewRID;
        }
        public void LoadDollarScalingList(DataTable dtList, int initialValue)
        {
            Infragistics.Win.UltraWinToolbars.ComboBoxTool cbo = (Infragistics.Win.UltraWinToolbars.ComboBoxTool)this.ultraToolbarsManager1.Tools["scalingDollarComboBox"];
            cbo.ValueList.ValueListItems.Clear();
            foreach (DataRow dr in dtList.Rows)
            {
                int i;
                string name;

                i = (int)dr["TEXT_CODE"];
                name = (string)dr["TEXT_VALUE"];

                cbo.ValueList.ValueListItems.Add(i, name);
            }
            cbo.Value = initialValue;
        }
        public void LoadUnitsScalingList(DataTable dtList, int initialValue)
        {
            Infragistics.Win.UltraWinToolbars.ComboBoxTool cbo = (Infragistics.Win.UltraWinToolbars.ComboBoxTool)this.ultraToolbarsManager1.Tools["scalingUnitsComboBox"];
            cbo.ValueList.ValueListItems.Clear();
            foreach (DataRow dr in dtList.Rows)
            {
                int i;
                string name;

                i = (int)dr["TEXT_CODE"];
                name = (string)dr["TEXT_VALUE"];

                cbo.ValueList.ValueListItems.Add(i, name);
            }
            cbo.Value = initialValue;
        }
        public void LoadBasisList(List<string> basisList)
        {
            Infragistics.Win.UltraWinToolbars.PopupMenuTool pop = (Infragistics.Win.UltraWinToolbars.PopupMenuTool)this.ultraToolbarsManager1.Tools["basisShow"];
            pop.Tools.Clear();
            int icount = 0;
            foreach (string s in basisList)
            {
                string skey = s + icount.ToString();
                Infragistics.Win.UltraWinToolbars.StateButtonTool btn = new Infragistics.Win.UltraWinToolbars.StateButtonTool(skey);
                btn.SharedProps.Caption = s;
                btn.Checked = true;
                this.ultraToolbarsManager1.Tools.Add(btn);
               
                pop.Tools.AddTool(skey);
                icount++;
            }
            if (pop.Tools.Count == 0)
            {
                pop.SharedProps.Visible = false;
            }
        }
        private void CheckBasis(string basisKey)
        {
            Infragistics.Win.UltraWinToolbars.PopupMenuTool pop = (Infragistics.Win.UltraWinToolbars.PopupMenuTool)this.ultraToolbarsManager1.Tools["basisShow"];
            bool found = false;
            int i = 0;
            string caption = string.Empty;
            while (found == false && i<=pop.Tools.Count-1)
            {
                if (pop.Tools[i].Key == basisKey)
                {
                    caption = pop.Tools[i].SharedProps.Caption;
                    found = true;
                }
                else
                {
                    i++;
                }
            }
            if (found == true)
            {
                RaiseShowHideBasisEvent(caption, i, ((Infragistics.Win.UltraWinToolbars.StateButtonTool)pop.Tools[i]).Checked);
            }

        }
        private bool _settingPeriod = false;
        public void SetPeriods(bool showYears, bool showSeasons, bool showQuarters, bool showMonths, bool showWeeks)
        {
            _settingPeriod = true;

            ((Infragistics.Win.UltraWinToolbars.StateButtonTool)this.ultraToolbarsManager1.Tools["periodYears"]).Checked = showYears;
            ((Infragistics.Win.UltraWinToolbars.StateButtonTool)this.ultraToolbarsManager1.Tools["periodSeasons"]).Checked = showSeasons;
            ((Infragistics.Win.UltraWinToolbars.StateButtonTool)this.ultraToolbarsManager1.Tools["periodQuarters"]).Checked = showQuarters;
            ((Infragistics.Win.UltraWinToolbars.StateButtonTool)this.ultraToolbarsManager1.Tools["periodMonths"]).Checked = showMonths;
            ((Infragistics.Win.UltraWinToolbars.StateButtonTool)this.ultraToolbarsManager1.Tools["periodWeeks"]).Checked = showWeeks;
            _settingPeriod = false;

        }


        //private bool _updatingMenuForSelectedCell = false;
        //public void UpdateMenuForSelectedCell(bool isColumnFrozen)
        //{
           // _updatingMenuForSelectedCell = true;
            //((Infragistics.Win.UltraWinToolbars.StateButtonTool)this.ultraToolbarsManager1.Tools["lockCell"]).Checked = isCellLocked;
            //((Infragistics.Win.UltraWinToolbars.StateButtonTool)this.ultraToolbarsManager1.Tools["lockRow"]).Checked = isRowLocked;
            //((Infragistics.Win.UltraWinToolbars.StateButtonTool)this.ultraToolbarsManager1.Tools["lockColumn"]).Checked = isColumnLocked;
           // ((Infragistics.Win.UltraWinToolbars.StateButtonTool)this.ultraToolbarsManager1.Tools["freezeColumn"]).Checked = isColumnFrozen;
           // _updatingMenuForSelectedCell = false;
        //}




  
        private void ultraToolbarsManager1_ToolClick(object sender, Infragistics.Win.UltraWinToolbars.ToolClickEventArgs e)
        {
            CheckBasis(e.Tool.Key);

            switch (e.Tool.Key)
            {
                case "showChart":
                   RaiseShowChartEvent();
                   break;
              

                case "btnApply":
                   RaiseApplyEvent();
                    break;
                case "chooseVariables":
                    RaiseChooseVariablesEvent();
                    break;
                case "chooseQuantity":
                    RaiseChooseQuantityEvent();
                    break;
                case "expandAll":
                    RaiseExpandAllPeriodsEvent();
                    break;
                case "collapseAll":
                    RaiseCollapseAllPeriodsEvent();
                    break;
                case "freezeColumns":
                    //if (_updatingMenuForSelectedCell == false)
                    //{
                    //    bool blnFreeze = ((Infragistics.Win.UltraWinToolbars.StateButtonTool)this.ultraToolbarsManager1.Tools["freezeColumn"]).Checked;
                        RaiseFreezeColumnEvent(true);
                    //}
                    break;
                case "unfreezeColumns":
                    RaiseFreezeColumnEvent(false);
                    break;
                case "lockColumn":
                    //if (_updatingMenuForActiveCell == false)
                    //{
                        //bool blnLock = ((Infragistics.Win.UltraWinToolbars.StateButtonTool)this.ultraToolbarsManager1.Tools["lockColumn"]).Checked;
                        RaiseLockColumnEvent(true);
                    //}
                    break;
                case "lockColumnCascade":
                    //if (_updatingMenuForActiveCell == false)
                    //{

                        RaiseLockColumnCascadeEvent(true);
                    //}
                    break;
                case "lockRow":
                    //if (_updatingMenuForActiveCell == false)
                    //{
                        //bool blnLock = ((Infragistics.Win.UltraWinToolbars.StateButtonTool)this.ultraToolbarsManager1.Tools["lockRow"]).Checked;
                        RaiseLockRowEvent(true);
                    //}
                    break;
                case "lockRowCascade":
                    //if (_updatingMenuForActiveCell == false)
                    //{
                        RaiseLockRowCascadeEvent(true);
                    //}
                    break;
                case "lockCell":
                    //if (_updatingMenuForActiveCell == false)
                    //{
                        //bool blnLock = ((Infragistics.Win.UltraWinToolbars.StateButtonTool)this.ultraToolbarsManager1.Tools["lockCell"]).Checked;
                        RaiseLockCellEvent(true);
                    //}
                    break;
                case "lockCellCascade":
                    //if (_updatingMenuForActiveCell == false)
                    //{
                    //bool blnLock = ((Infragistics.Win.UltraWinToolbars.StateButtonTool)this.ultraToolbarsManager1.Tools["lockCell"]).Checked;
                    RaiseLockCellCascadeEvent(true);
                    //}
                    break;
                case "lockSheet":
                    //if (_updatingMenuForActiveCell == false)
                    //{
                    //bool blnLock = ((Infragistics.Win.UltraWinToolbars.StateButtonTool)this.ultraToolbarsManager1.Tools["lockRow"]).Checked;
                    RaiseLockSheetEvent(true);
                    //}
                    break;
                case "lockSheetCascade":
                    //if (_updatingMenuForActiveCell == false)
                    //{
                    RaiseLockSheetCascadeEvent(true);
                    //}
                    break;
                case "unlockColumn":
                    //if (_updatingMenuForActiveCell == false)
                    //{
                    //bool blnLock = ((Infragistics.Win.UltraWinToolbars.StateButtonTool)this.ultraToolbarsManager1.Tools["lockColumn"]).Checked;
                    RaiseLockColumnEvent(false);
                    //}
                    break;
                case "unlockColumnCascade":
                    //if (_updatingMenuForActiveCell == false)
                    //{

                    RaiseLockColumnCascadeEvent(false);
                    //}
                    break;
                case "unlockRow":
                    //if (_updatingMenuForActiveCell == false)
                    //{
                    //bool blnLock = ((Infragistics.Win.UltraWinToolbars.StateButtonTool)this.ultraToolbarsManager1.Tools["lockRow"]).Checked;
                    RaiseLockRowEvent(false);
                    //}
                    break;
                case "unlockRowCascade":
                    //if (_updatingMenuForActiveCell == false)
                    //{
                    RaiseLockRowCascadeEvent(false);
                    //}
                    break;
                case "unlockCell":
                    //if (_updatingMenuForActiveCell == false)
                    //{
                    //bool blnLock = ((Infragistics.Win.UltraWinToolbars.StateButtonTool)this.ultraToolbarsManager1.Tools["lockCell"]).Checked;
                    RaiseLockCellEvent(false);
                    //}
                    break;
                case "unlockCellCascade":
                    //if (_updatingMenuForActiveCell == false)
                    //{
                    //bool blnLock = ((Infragistics.Win.UltraWinToolbars.StateButtonTool)this.ultraToolbarsManager1.Tools["lockCell"]).Checked;
                    RaiseLockCellCascadeEvent(false);
                    //}
                    break;
                case "unlockSheet":
                    //if (_updatingMenuForActiveCell == false)
                    //{
                    //bool blnLock = ((Infragistics.Win.UltraWinToolbars.StateButtonTool)this.ultraToolbarsManager1.Tools["lockRow"]).Checked;
                    RaiseLockSheetEvent(false);
                    //}
                    break;
                case "unlockSheetCascade":
                    //if (_updatingMenuForActiveCell == false)
                    //{
                    RaiseLockSheetCascadeEvent(false);
                    //}
                    break;
                case "periodYears":
                    if (_settingPeriod == false)
                    {
                        _settingPeriod = true;
                        //if (((Infragistics.Win.UltraWinToolbars.StateButtonTool)this.ultraToolbarsManager1.Tools["periodYears"]).Checked == true)
                        //{
                        //    ((Infragistics.Win.UltraWinToolbars.StateButtonTool)this.ultraToolbarsManager1.Tools["periodSeasons"]).Checked = true;
                        //    ((Infragistics.Win.UltraWinToolbars.StateButtonTool)this.ultraToolbarsManager1.Tools["periodQuarters"]).Checked = true;
                        //    ((Infragistics.Win.UltraWinToolbars.StateButtonTool)this.ultraToolbarsManager1.Tools["periodMonths"]).Checked = true;
                        //}
                     
                        RaisePeriodChangedEvent();
                        _settingPeriod = false;
                    }
                    break;
                case "periodSeasons":
                    if (_settingPeriod == false)
                    {
                        _settingPeriod = true;
                        //if (((Infragistics.Win.UltraWinToolbars.StateButtonTool)this.ultraToolbarsManager1.Tools["periodSeasons"]).Checked == true)
                        //{
                        //    ((Infragistics.Win.UltraWinToolbars.StateButtonTool)this.ultraToolbarsManager1.Tools["periodYears"]).Checked = false;
                        //    ((Infragistics.Win.UltraWinToolbars.StateButtonTool)this.ultraToolbarsManager1.Tools["periodQuarters"]).Checked = true;
                        //    ((Infragistics.Win.UltraWinToolbars.StateButtonTool)this.ultraToolbarsManager1.Tools["periodMonths"]).Checked = true;
                        //}
                        //else
                        //{
                        //    ((Infragistics.Win.UltraWinToolbars.StateButtonTool)this.ultraToolbarsManager1.Tools["periodYears"]).Checked = false;
 
                        //}
                        RaisePeriodChangedEvent();
                        _settingPeriod = false;
                    }
                    break;
                case "periodQuarters":
                    if (_settingPeriod == false)
                    {
                        _settingPeriod = true;
                        //if (((Infragistics.Win.UltraWinToolbars.StateButtonTool)this.ultraToolbarsManager1.Tools["periodQuarters"]).Checked == true)
                        //{
                        //    ((Infragistics.Win.UltraWinToolbars.StateButtonTool)this.ultraToolbarsManager1.Tools["periodYears"]).Checked = false;
                        //    ((Infragistics.Win.UltraWinToolbars.StateButtonTool)this.ultraToolbarsManager1.Tools["periodSeasons"]).Checked = false;
                        //    ((Infragistics.Win.UltraWinToolbars.StateButtonTool)this.ultraToolbarsManager1.Tools["periodMonths"]).Checked = true;
                        //}
                        //else
                        //{
                        //    ((Infragistics.Win.UltraWinToolbars.StateButtonTool)this.ultraToolbarsManager1.Tools["periodYears"]).Checked = false;
                        //    ((Infragistics.Win.UltraWinToolbars.StateButtonTool)this.ultraToolbarsManager1.Tools["periodSeasons"]).Checked = false;
                        //}
                        RaisePeriodChangedEvent();
                        _settingPeriod = false;
                    }
                    break;
                case "periodMonths":
                    if (_settingPeriod == false)
                    {
                        _settingPeriod = true;
                        //if (((Infragistics.Win.UltraWinToolbars.StateButtonTool)this.ultraToolbarsManager1.Tools["periodMonths"]).Checked == false)
                        //{
                        //    ((Infragistics.Win.UltraWinToolbars.StateButtonTool)this.ultraToolbarsManager1.Tools["periodYears"]).Checked = false;
                        //    ((Infragistics.Win.UltraWinToolbars.StateButtonTool)this.ultraToolbarsManager1.Tools["periodSeasons"]).Checked = false;
                        //    ((Infragistics.Win.UltraWinToolbars.StateButtonTool)this.ultraToolbarsManager1.Tools["periodQuarters"]).Checked = false;
                        //}
                        RaisePeriodChangedEvent();
                        _settingPeriod = false;
                    }
                    break;
                case "periodWeeks":
                    if (_settingPeriod == false)
                    {
                        _settingPeriod = true;
                    
                        RaisePeriodChangedEvent();
                        _settingPeriod = false;
                    }
                    break;

       

                #region "Grid Tools"

                case "gridSearchFindButton":
                    //SearchGrid(ugUserActivityExplorer, (((Infragistics.Win.UltraWinToolbars.TextBoxTool)this.ultraToolbarsManager1.Toolbars["Grid Search Toolbar"].Tools["gridSearchText"]).Text));
                    break;
                case "gridSearchClearButton":
                    Infragistics.Win.UltraWinToolbars.TextBoxTool t = (Infragistics.Win.UltraWinToolbars.TextBoxTool)this.ultraToolbarsManager1.Toolbars["Grid Search Toolbar"].Tools["gridSearchText"];
                    t.Text = "";
                    //ClearGridSearchResults(ugUserActivityExplorer);
                    break;

                case "gridShowSearchToolbar":
                    //this.ultraToolbarsManager1.Toolbars["Grid Search Toolbar"].Visible = !this.ultraToolbarsManager1.Toolbars["Grid Search Toolbar"].Visible;
                    break;

                case "gridShowGroupArea":
                    //this.ugUserActivityExplorer.DisplayLayout.GroupByBox.Hidden = !this.ugUserActivityExplorer.DisplayLayout.GroupByBox.Hidden;
                    break;

                case "gridShowFilterRow":
                    //if (this.ugUserActivityExplorer.DisplayLayout.Override.FilterUIType == Infragistics.Win.UltraWinGrid.FilterUIType.FilterRow)
                    //{
                    //    this.ugUserActivityExplorer.DisplayLayout.Override.FilterUIType = Infragistics.Win.UltraWinGrid.FilterUIType.HeaderIcons;
                    //}
                    //else
                    //{
                    //    this.ugUserActivityExplorer.DisplayLayout.Override.FilterUIType = Infragistics.Win.UltraWinGrid.FilterUIType.FilterRow;
                    //}
                    break;

                //case "gridExportSelected":
                //    //UltraGridExcelExportWrapper exporter = new UltraGridExcelExportWrapper(this.ugUserActivityExplorer);
                //    //exporter.ExportSelectedRowsToExcel();
                //    RaiseGridExportSelectedRowsEvent();
                //    break;

                case "gridExportMenuPopup":
                    //UltraGridExcelExportWrapper exporter2 = new UltraGridExcelExportWrapper(this.ugUserActivityExplorer);
                    //exporter2.ExportAllRowsToExcel();
                    RaiseGridExportAllRowsEvent();
                    break;

                //case "gridEmailSelectedRows":
                //   // UltraGridExcelExportWrapper exporter3 = new UltraGridExcelExportWrapper(this.ugUserActivityExplorer);
                //    //ShowEmailForm(exporter3.ExportSelectedRowsToExcelAsAttachment());
                //    RaiseGridEmailSelectedRowsEvent();
                //    break;

                case "GridEmailPopupMenu":
                    //UltraGridExcelExportWrapper exporter4 = new UltraGridExcelExportWrapper(this.ugUserActivityExplorer);
                    //ShowEmailForm(exporter4.ExportAllRowsToExcelAsAttachment());
                    RaiseGridEmailAllRowsEvent();
                    break;

                case "gridChooseColumns":
                    //this.ugUserActivityExplorer.ShowColumnChooser("Choose Columns");
                    break;

                #endregion

           






            }
        }

        private void ultraToolbarManager1_ToolValueChanged(object sender, Infragistics.Win.UltraWinToolbars.ToolEventArgs e)
        {
            switch (e.Tool.Key)
            {
                case "viewComboBox":
                    _isViewComboDirty = true;
                    break;

            }
        }
        private void ultraToolbarsManager1_ToolKeyPress(object sender, Infragistics.Win.UltraWinToolbars.ToolKeyPressEventArgs e)
        {
            switch (e.Tool.Key)
            {
                case "gridSearchText":
                    if (e.KeyChar == (char)Keys.Return)
                    {
                        //SearchGrid(ugUserActivityExplorer, ((Infragistics.Win.UltraWinToolbars.TextBoxTool)e.Tool).Text);
                    }
                    break;
                case "viewComboBox":
                    if (e.KeyChar == (char)Keys.Return)
                    {
                       //((Infragistics.Win.UltraWinToolbars.ComboBoxTool)e.Tool).IsInEditMode = false;
                       if (! _isViewComboDroppedDown)
                       {
                           ProcessViewComboBox((Infragistics.Win.UltraWinToolbars.ComboBoxTool)e.Tool);
                       }
          
                    }
                    break;
                case "scalingDollarComboBox":
                    if (e.KeyChar == (char)Keys.Return)
                    {
                        ((Infragistics.Win.UltraWinToolbars.ComboBoxTool)e.Tool).IsInEditMode = false;
                    }
                    break;
                case "scalingUnitsComboBox":
                    if (e.KeyChar == (char)Keys.Return)
                    {
                        ((Infragistics.Win.UltraWinToolbars.ComboBoxTool)e.Tool).IsInEditMode = false;
                    }
                    break;
            }
        }
        private void ultraToolbarsManager1_TookKeyDown(object sender, Infragistics.Win.UltraWinToolbars.ToolKeyEventArgs e)
        {
            switch (e.Tool.Key)
            {
                case "gridSearchText":
                    if (e.KeyCode == Keys.Return)
                    {
                        //SearchGrid(ugUserActivityExplorer, ((Infragistics.Win.UltraWinToolbars.TextBoxTool)e.Tool).Text);
                    }
                    break;
                case "viewComboBox":
                    if (e.KeyCode == Keys.Return)
                    {
                        //((Infragistics.Win.UltraWinToolbars.ComboBoxTool)e.Tool).IsInEditMode = false;
                        if (_isViewComboDroppedDown)
                        {
                            //int viewRID = (int)cbo.Value;
                            //cbo.Value = initialViewRID;
                            //_isViewComboDirty = true;
                            ((Infragistics.Win.UltraWinToolbars.ComboBoxTool)e.Tool).IsInEditMode = false;
                            //ProcessViewComboBox((Infragistics.Win.UltraWinToolbars.ComboBoxTool)e.Tool);
                        }

                    }
                    break;
                case "scalingDollarComboBox":
                    if (e.KeyCode == Keys.Return)
                    {
                        ((Infragistics.Win.UltraWinToolbars.ComboBoxTool)e.Tool).IsInEditMode = false;
                    }
                    break;
                case "scalingUnitsComboBox":
                    if (e.KeyCode == Keys.Return)
                    {
                        ((Infragistics.Win.UltraWinToolbars.ComboBoxTool)e.Tool).IsInEditMode = false;
                    }
                    break;
            }
        }

        private void ultraToolbarsManager1_AfterToolExitEditMode(object sender, Infragistics.Win.UltraWinToolbars.AfterToolExitEditModeEventArgs e)
        {
            switch (e.Tool.Key)
            {
                case "viewComboBox":
                    if (_isViewComboDirty)
                    {
                        ProcessViewComboBox((Infragistics.Win.UltraWinToolbars.ComboBoxTool)e.Tool);
                    }
                    break;
                case "scalingDollarComboBox":
                    string scalingDollarValue = (string)((Infragistics.Win.UltraWinToolbars.ComboBoxTool)e.Tool).Text;
                    RaiseDollarScalingChangedEvent(scalingDollarValue);
                    break;
                case "scalingUnitsComboBox":
                    string scalingUnitsValue = (string)((Infragistics.Win.UltraWinToolbars.ComboBoxTool)e.Tool).Text;
                    RaiseUnitsScalingChangedEvent(scalingUnitsValue);
                    break;
            }
        }

        private bool _isViewComboDroppedDown = false;
        private bool _isViewComboDirty = false;
        private void ultraToolbarsManager1_AfterToolDropdown(object sender, Infragistics.Win.UltraWinToolbars.ToolDropdownEventArgs e)
        {
            switch (e.Tool.Key)
            {
                case "viewComboBox":
                    //int viewRID = (int)((Infragistics.Win.UltraWinToolbars.ComboBoxTool)e.Tool).Value;
                    //string viewName = ((Infragistics.Win.UltraWinToolbars.ComboBoxTool)e.Tool).Text;
                    //RaiseViewChangedEvent(viewRID, viewName);
                    _isViewComboDroppedDown = true;
                    break;

            }
        }
        private void ultraToolbarsManager1_AfterToolCloseup(object sender, Infragistics.Win.UltraWinToolbars.ToolDropdownEventArgs e)
        {
            switch (e.Tool.Key)
            {
                case "viewComboBox":
                    _isViewComboDroppedDown = false;
                    ProcessViewComboBox((Infragistics.Win.UltraWinToolbars.ComboBoxTool)e.Tool);
                    break;
              
            }
        }
        private void ProcessViewComboBox(Infragistics.Win.UltraWinToolbars.ComboBoxTool cbo)
        {
            int viewRID = (int)cbo.Value;
            string viewName = cbo.Text;
            _isViewComboDirty = false;
            RaiseViewChangedEvent(viewRID, viewName);
        }


        #region "Events"
        public class ApplyEventArgs
        {
            public ApplyEventArgs() { }
        }
        public delegate void ApplyEventHandler(object sender, ApplyEventArgs e);
        public event ApplyEventHandler ApplyEvent;
        protected virtual void RaiseApplyEvent()
        {
            if (ApplyEvent != null)
                ApplyEvent(this, new ApplyEventArgs());
        }

        public class ShowChartEventArgs
        {
            public ShowChartEventArgs() { }
        }
        public delegate void ShowChartEventHandler(object sender, ShowChartEventArgs e);
        public event ShowChartEventHandler ShowChartEvent;
        protected virtual void RaiseShowChartEvent()
        {
            if (ShowChartEvent != null)
                ShowChartEvent(this, new ShowChartEventArgs());
        }

        public class CollapseAllPeriodsEventArgs
        {
            public CollapseAllPeriodsEventArgs() { }
        }
        public delegate void CollapseAllPeriodsEventHandler(object sender, CollapseAllPeriodsEventArgs e);
        public event CollapseAllPeriodsEventHandler CollapseAllPeriodsEvent;
        protected virtual void RaiseCollapseAllPeriodsEvent()
        {
            if (CollapseAllPeriodsEvent != null)
                CollapseAllPeriodsEvent(this, new CollapseAllPeriodsEventArgs());
        }

        public class ExpandAllPeriodsEventArgs
        {
            public ExpandAllPeriodsEventArgs() { }
        }
        public delegate void ExpandAllPeriodsEventHandler(object sender, ExpandAllPeriodsEventArgs e);
        public event ExpandAllPeriodsEventHandler ExpandAllPeriodsEvent;
        protected virtual void RaiseExpandAllPeriodsEvent()
        {
            if (ExpandAllPeriodsEvent != null)
                ExpandAllPeriodsEvent(this, new ExpandAllPeriodsEventArgs());
        }

        public class ChooseVariablesEventArgs
        {
            public ChooseVariablesEventArgs() { }
        }
        public delegate void ChooseVariablesEventHandler(object sender, ChooseVariablesEventArgs e);
        public event ChooseVariablesEventHandler ChooseVariablesEvent;
        protected virtual void RaiseChooseVariablesEvent()
        {
            if (ChooseVariablesEvent != null)
                ChooseVariablesEvent(this, new ChooseVariablesEventArgs());
        }

        public class ChooseQuantityEventArgs
        {
            public ChooseQuantityEventArgs() { }
        }
        public delegate void ChooseQuantityEventHandler(object sender, ChooseQuantityEventArgs e);
        public event ChooseQuantityEventHandler ChooseQuantityEvent;
        protected virtual void RaiseChooseQuantityEvent()
        {
            if (ChooseQuantityEvent != null)
                ChooseQuantityEvent(this, new ChooseQuantityEventArgs());
        }

        public class FreezeColumnEventArgs
        {
            public FreezeColumnEventArgs(bool blnFreeze) { this.blnFreeze = blnFreeze; }
            public bool blnFreeze { get; private set; }
        }
        public delegate void FreezeColumnEventHandler(object sender, FreezeColumnEventArgs e);
        public event FreezeColumnEventHandler FreezeColumnEvent;
        protected virtual void RaiseFreezeColumnEvent(bool blnFreeze)
        {
            if (FreezeColumnEvent != null)
                FreezeColumnEvent(this, new FreezeColumnEventArgs(blnFreeze));
        }

        public class LockColumnEventArgs
        {
            public LockColumnEventArgs(bool blnLock) { this.blnLock = blnLock; }
            public bool blnLock { get; private set; }
        }
        public delegate void LockColumnEventHandler(object sender, LockColumnEventArgs e);
        public event LockColumnEventHandler LockColumnEvent;
        protected virtual void RaiseLockColumnEvent(bool blnLock)
        {
            if (LockColumnEvent != null)
                LockColumnEvent(this, new LockColumnEventArgs(blnLock));
        }
        public class LockColumnCascadeEventArgs
        {
            public LockColumnCascadeEventArgs(bool blnLock) { this.blnLock = blnLock; }
            public bool blnLock { get; private set; }
        }
        public delegate void LockColumnCascadeEventHandler(object sender, LockColumnCascadeEventArgs e);
        public event LockColumnCascadeEventHandler LockColumnCascadeEvent;
        protected virtual void RaiseLockColumnCascadeEvent(bool blnLock)
        {
            if (LockColumnCascadeEvent != null)
                LockColumnCascadeEvent(this, new LockColumnCascadeEventArgs(blnLock));
        }
        public class LockCellEventArgs
        {
            public LockCellEventArgs(bool blnLock) { this.blnLock = blnLock; }
            public bool blnLock { get; private set; }
        }
        public delegate void LockCellEventHandler(object sender, LockCellEventArgs e);
        public event LockCellEventHandler LockCellEvent;
        protected virtual void RaiseLockCellEvent(bool blnLock)
        {
            if (LockCellEvent != null)
                LockCellEvent(this, new LockCellEventArgs(blnLock));
        }

        public class LockCellCascadeEventArgs
        {
            public LockCellCascadeEventArgs(bool blnLock) { this.blnLock = blnLock; }
            public bool blnLock { get; private set; }
        }
        public delegate void LockCellCascadeEventHandler(object sender, LockCellCascadeEventArgs e);
        public event LockCellCascadeEventHandler LockCellCascadeEvent;
        protected virtual void RaiseLockCellCascadeEvent(bool blnLock)
        {
            if (LockCellCascadeEvent != null)
                LockCellCascadeEvent(this, new LockCellCascadeEventArgs(blnLock));
        }

        public class LockRowEventArgs
        {
            public LockRowEventArgs(bool blnLock) { this.blnLock = blnLock; }
            public bool blnLock { get; private set; }
        }
        public delegate void LockRowEventHandler(object sender, LockRowEventArgs e);
        public event LockRowEventHandler LockRowEvent;
        protected virtual void RaiseLockRowEvent(bool blnLock)
        {
            if (LockRowEvent != null)
                LockRowEvent(this, new LockRowEventArgs(blnLock));
        }
        public class LockRowCascadeEventArgs
        {
            public LockRowCascadeEventArgs(bool blnLock) { this.blnLock = blnLock; }
            public bool blnLock { get; private set; }
        }
        public delegate void LockRowCascadeEventHandler(object sender, LockRowCascadeEventArgs e);
        public event LockRowCascadeEventHandler LockRowCascadeEvent;
        protected virtual void RaiseLockRowCascadeEvent(bool blnLock)
        {
            if (LockRowCascadeEvent != null)
                LockRowCascadeEvent(this, new LockRowCascadeEventArgs(blnLock));
        }
        public class LockSheetEventArgs
        {
            public LockSheetEventArgs(bool blnLock) { this.blnLock = blnLock; }
            public bool blnLock { get; private set; }
        }
        public delegate void LockSheetEventHandler(object sender, LockSheetEventArgs e);
        public event LockSheetEventHandler LockSheetEvent;
        protected virtual void RaiseLockSheetEvent(bool blnLock)
        {
            if (LockSheetEvent != null)
                LockSheetEvent(this, new LockSheetEventArgs(blnLock));
        }
        public class LockSheetCascadeEventArgs
        {
            public LockSheetCascadeEventArgs(bool blnLock) { this.blnLock = blnLock; }
            public bool blnLock { get; private set; }
        }
        public delegate void LockSheetCascadeEventHandler(object sender, LockSheetCascadeEventArgs e);
        public event LockSheetCascadeEventHandler LockSheetCascadeEvent;
        protected virtual void RaiseLockSheetCascadeEvent(bool blnLock)
        {
            if (LockSheetCascadeEvent != null)
                LockSheetCascadeEvent(this, new LockSheetCascadeEventArgs(blnLock));
        }

        public class ViewChangedEventArgs
        {
            public ViewChangedEventArgs(int viewRID, string viewName) { this.viewRID = viewRID; this.viewName = viewName; }
            public int viewRID { get; private set; }
            public string viewName { get; private set; }
        }
        public delegate void ViewChangedEventHandler(object sender, ViewChangedEventArgs e);
        public event ViewChangedEventHandler ViewChangedEvent;
        protected virtual void RaiseViewChangedEvent(int viewRID, string viewName)
        {
            if (ViewChangedEvent != null)
                ViewChangedEvent(this, new ViewChangedEventArgs(viewRID, viewName));
        }

        public class ShowHideBasisEventArgs
        {
            public ShowHideBasisEventArgs(string basisKey, int basisSequence, bool doShow) { this.basisKey = basisKey; this.basisSequence = basisSequence; this.doShow = doShow; }
            public string basisKey { get; private set; }
            public int basisSequence { get; private set; }
            public bool doShow { get; private set; }
        }
        public delegate void ShowHideBasisEventHandler(object sender, ShowHideBasisEventArgs e);
        public event ShowHideBasisEventHandler ShowHideBasisEvent;
        protected virtual void RaiseShowHideBasisEvent(string basisKey, int basisSequence, bool doShow)
        {
            if (ShowHideBasisEvent != null)
                ShowHideBasisEvent(this, new ShowHideBasisEventArgs(basisKey, basisSequence, doShow));
        }

        public class PeriodChangedEventArgs
        {
            public PeriodChangedEventArgs(bool showYears, bool showSeasons, bool showQuarters, bool showMonths, bool showWeeks) { this.showYears = showYears; this.showSeasons = showSeasons; this.showQuarters = showQuarters; this.showMonths = showMonths; this.showWeeks = showWeeks; }
            public bool showYears { get; private set; }
            public bool showSeasons { get; private set; }
            public bool showQuarters { get; private set; }
            public bool showMonths { get; private set; }
            public bool showWeeks { get; private set; }
        }
        public delegate void PeriodChangedEventHandler(object sender, PeriodChangedEventArgs e);
        public event PeriodChangedEventHandler PeriodChangedEvent;
        protected virtual void RaisePeriodChangedEvent()
        {
            if (PeriodChangedEvent != null)
                PeriodChangedEvent(this, new PeriodChangedEventArgs(((Infragistics.Win.UltraWinToolbars.StateButtonTool)this.ultraToolbarsManager1.Tools["periodYears"]).Checked, 
                                                                    ((Infragistics.Win.UltraWinToolbars.StateButtonTool)this.ultraToolbarsManager1.Tools["periodSeasons"]).Checked, 
                                                                    ((Infragistics.Win.UltraWinToolbars.StateButtonTool)this.ultraToolbarsManager1.Tools["periodQuarters"]).Checked, 
                                                                    ((Infragistics.Win.UltraWinToolbars.StateButtonTool)this.ultraToolbarsManager1.Tools["periodMonths"]).Checked, 
                                                                    ((Infragistics.Win.UltraWinToolbars.StateButtonTool)this.ultraToolbarsManager1.Tools["periodWeeks"]).Checked
                                                                    ));
        }


        public class DollarScalingChangedEventArgs
        {
            public DollarScalingChangedEventArgs(string scalingValue) { this.scalingValue = scalingValue; }
            public string scalingValue { get; private set; }
        }
        public delegate void DollarScalingChangedEventHandler(object sender, DollarScalingChangedEventArgs e);
        public event DollarScalingChangedEventHandler DollarScalingChangedEvent;
        protected virtual void RaiseDollarScalingChangedEvent(string scalingValue)
        {
            if (DollarScalingChangedEvent != null)
                DollarScalingChangedEvent(this, new DollarScalingChangedEventArgs(scalingValue));
        }

        public class UnitsScalingChangedEventArgs
        {
            public UnitsScalingChangedEventArgs(string scalingValue) { this.scalingValue = scalingValue; }
            public string scalingValue { get; private set; }
        }
        public delegate void UnitsScalingChangedEventHandler(object sender, UnitsScalingChangedEventArgs e);
        public event UnitsScalingChangedEventHandler UnitsScalingChangedEvent;
        protected virtual void RaiseUnitsScalingChangedEvent(string scalingValue)
        {
            if (UnitsScalingChangedEvent != null)
                UnitsScalingChangedEvent(this, new UnitsScalingChangedEventArgs(scalingValue));
        }


        public class GridExportAllRowsEventArgs
        {
            public GridExportAllRowsEventArgs() { }
        }
        public delegate void GridExportAllRowsEventHandler(object sender, GridExportAllRowsEventArgs e);
        public event GridExportAllRowsEventHandler GridExportAllRowsEvent;
        protected virtual void RaiseGridExportAllRowsEvent()
        {
            if (GridExportAllRowsEvent != null)
                GridExportAllRowsEvent(this, new GridExportAllRowsEventArgs());
        }
        //public class GridExportSelectedRowsEventArgs
        //{
        //    public GridExportSelectedRowsEventArgs() { }
        //}
        //public delegate void GridExportSelectedRowsEventHandler(object sender, GridExportSelectedRowsEventArgs e);
        //public event GridExportSelectedRowsEventHandler GridExportSelectedRowsEvent;
        //protected virtual void RaiseGridExportSelectedRowsEvent()
        //{
        //    if (GridExportSelectedRowsEvent != null)
        //        GridExportSelectedRowsEvent(this, new GridExportSelectedRowsEventArgs());
        //}
        public class GridEmailAllRowsEventArgs
        {
            public GridEmailAllRowsEventArgs() { }
        }
        public delegate void GridEmailAllRowsEventHandler(object sender, GridEmailAllRowsEventArgs e);
        public event GridEmailAllRowsEventHandler GridEmailAllRowsEvent;
        protected virtual void RaiseGridEmailAllRowsEvent()
        {
            if (GridEmailAllRowsEvent != null)
                GridEmailAllRowsEvent(this, new GridEmailAllRowsEventArgs());
        }

       



    

        
        //public class GridEmailSelectedRowsEventArgs
        //{
        //    public GridEmailSelectedRowsEventArgs() { }
        //}
        //public delegate void GridEmailSelectedRowsEventHandler(object sender, GridEmailSelectedRowsEventArgs e);
        //public event GridEmailSelectedRowsEventHandler GridEmailSelectedRowsEvent;
        //protected virtual void RaiseGridEmailSelectedRowsEvent()
        //{
        //    if (GridEmailSelectedRowsEvent != null)
        //        GridEmailSelectedRowsEvent(this, new GridEmailSelectedRowsEventArgs());
        //}


        #endregion

    }

}
