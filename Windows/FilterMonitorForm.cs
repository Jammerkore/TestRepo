using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;


using MIDRetail.DataCommon;
using MIDRetail.Windows.Controls; 

namespace MIDRetail.Windows
{
    public partial class FilterMonitorForm : Form
    {
        public FilterMonitorForm()
        {
            InitializeComponent();
        }

        private void FilterMonitorForm_Load(object sender, EventArgs e)
        {
            Infragistics.Shared.ResourceCustomizer rc = Infragistics.Win.UltraWinGrid.Resources.Customizer;
            rc.SetCustomizedString("GroupByBoxDefaultPrompt", "Drag a column here to group by that column.");
            rc.SetCustomizedString("ColumnChooserButtonToolTip", "Click here to show/hide columns");
            rc.SetCustomizedString("ColumnChooserDialogCaption", "Choose Columns");
            this.ultraGrid1.DisplayLayout.Override.RowSelectorHeaderStyle = Infragistics.Win.UltraWinGrid.RowSelectorHeaderStyle.ColumnChooserButton;


            this.ultraGrid1.DisplayLayout.Override.AllowAddNew = Infragistics.Win.UltraWinGrid.AllowAddNew.No;
            this.ultraGrid1.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            this.ultraGrid1.DisplayLayout.Override.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.True;
            this.ultraGrid1.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.False;
            this.ultraGrid1.DisplayLayout.Override.RowSizingAutoMaxLines = 50;
            this.ultraGrid1.DisplayLayout.Override.RowSizing = Infragistics.Win.UltraWinGrid.RowSizing.AutoFree;

            this.ultraGrid1.DataSource = FilterMonitor.dtMonitor;
            
        }
        private void ultraGrid1_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
           // e.Layout.Bands[0].Columns["Time Started"].Format = "MM/dd/yyyy HH:mm:ss.fff";

        }

        private void ultraToolbarsManager1_ToolClick(object sender, Infragistics.Win.UltraWinToolbars.ToolClickEventArgs e)
        {
            switch (e.Tool.Key)
            {
                case "btnStartMonitor":
                    FilterMonitor.doMonitor = true;
                    this.ultraToolbarsManager1.Tools["lblMonitorStatus"].SharedProps.Caption = "Monitor: ON";
                    break;
                case "btnStopMonitor":
                    FilterMonitor.doMonitor = false;
                    this.ultraToolbarsManager1.Tools["lblMonitorStatus"].SharedProps.Caption = "Monitor: OFF";
                    break;
                case "btnClear":
                    FilterMonitor.ClearMonitorEntries();
                    break;
                case "btnIncludeApplicationText":
                    //FilterMonitor.includeApplicationText = ((Infragistics.Win.UltraWinToolbars.StateButtonTool)this.ultraToolbarsManager1.Tools["btnIncludeApplicationText"]).Checked;
   
                    break;
                case "btnIncludeStartandStopTimes":
                    //FilterMonitor.includeStartAndStopTimes = ((Infragistics.Win.UltraWinToolbars.StateButtonTool)this.ultraToolbarsManager1.Tools["btnIncludeStartandStopTimes"]).Checked;
                    break;
                case "btnIncludeStackTrace":
                    //FilterMonitor.includeStackTrace = ((Infragistics.Win.UltraWinToolbars.StateButtonTool)this.ultraToolbarsManager1.Tools["btnIncludeStackTrace"]).Checked;
                    break;
                case "btnIncludeMessageListening":
                    //FilterMonitor.includeMessageListening = ((Infragistics.Win.UltraWinToolbars.StateButtonTool)this.ultraToolbarsManager1.Tools["btnIncludeMessageListening"]).Checked;
                    break;
                   

                #region "Grid Tools"

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
                    SharedControlRoutines.exportHelper.ExportSelectedRowsToExcel(this.ultraGrid1);
                    break;

                case "gridExportAll":
                    SharedControlRoutines.exportHelper.ExportAllRowsToExcel(this.ultraGrid1);
                    break;

                case "gridEmailSelectedRows":
                    SharedControlRoutines.exportHelper.EmailSelectedRows("Allocation Analysis", "Allocation Analysis.xls", this.ultraGrid1);
                    break;

                case "gridEmailAllRows":
                    SharedControlRoutines.exportHelper.EmailAllRows("Allocation Analysis", "Allocation Analysis.xls", this.ultraGrid1);
                    break;

                case "gridChooseColumns":
                    this.ultraGrid1.ShowColumnChooser("Choose Columns");
                    break;

                #endregion

   
            }
        }

        private void SQLMonitorForm_Closing(object sender, FormClosingEventArgs e)
        {
            SQLMonitorList.doMonitor = false;
        }

        private void ultraToolbarsManager_ToolKeyPress(object sender, Infragistics.Win.UltraWinToolbars.ToolKeyPressEventArgs e)
        {
            switch (e.Tool.Key)
            {
                case "gridSearchText":
                    if (e.KeyChar == (char)Keys.Return)
                    {
                        MIDRetail.Windows.Controls.SharedControlRoutines.SearchGrid(this.ultraGrid1, ((Infragistics.Win.UltraWinToolbars.TextBoxTool)e.Tool).Text);
                    }
                    break;
                case "messageLevelComboBox":
                    if (e.KeyChar == (char)Keys.Return)
                    {
                        ((Infragistics.Win.UltraWinToolbars.ComboBoxTool)e.Tool).IsInEditMode = false;
                    }
                    break;
            }
        }
    }
}
