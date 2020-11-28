using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.InteropServices;

using MIDRetail.Business;
using MIDRetail.DataCommon;
using MIDRetail.Windows.Controls; 

namespace MIDRetail.Windows
{
    public partial class SQLMonitorForm : Form
    {
        public SQLMonitorForm()
        {
            InitializeComponent();
        }

        private void SQLMonitorForm_Load(object sender, EventArgs e)
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

            
            //ProfileList profList = StoreMgmt.GetActiveStoresList();
            //DataTable dtProfSize = new DataTable();
            //dtProfSize.Columns.Add("Store");
            //dtProfSize.Columns.Add("Profile Size In Bytes");
            //foreach (StoreProfile prof in profList)
            //{
            //    DataRow drProfSize = dtProfSize.NewRow();
            //    drProfSize["Store"] = prof.Text;
            //    int sizeInBytes = prof.GetSize();
            //    drProfSize["Profile Size In Bytes"] = sizeInBytes.ToString();
            //    dtProfSize.Rows.Add(drProfSize);
            //}
            //this.ultraGrid1.DataSource = dtProfSize;


            this.ultraGrid1.DataSource = SQLMonitorList.dtSQLMonitor;
        }
        private void ultraGrid1_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            e.Layout.Bands[0].Columns["Time Started"].Format = "MM/dd/yyyy HH:mm:ss.fff";

        }

        private void ultraToolbarsManager1_ToolClick(object sender, Infragistics.Win.UltraWinToolbars.ToolClickEventArgs e)
        {
            switch (e.Tool.Key)
            {
                case "btnStartMonitor":
                    SQLMonitorList.doMonitor = true;
                    this.ultraToolbarsManager1.Tools["lblMonitorStatus"].SharedProps.Caption = "Monitor: ON";
                    break;
                case "btnStopMonitor":
                    SQLMonitorList.doMonitor = false;
                    this.ultraToolbarsManager1.Tools["lblMonitorStatus"].SharedProps.Caption = "Monitor: OFF";
                    break;
                case "btnClear":
                    SQLMonitorList.ClearSQLMonitorEntries();
                    break;
                case "btnIncludeApplicationText":
                    SQLMonitorList.includeApplicationText = ((Infragistics.Win.UltraWinToolbars.StateButtonTool)this.ultraToolbarsManager1.Tools["btnIncludeApplicationText"]).Checked;
   
                    break;
                case "btnIncludeStartandStopTimes":
                    SQLMonitorList.includeStartAndStopTimes = ((Infragistics.Win.UltraWinToolbars.StateButtonTool)this.ultraToolbarsManager1.Tools["btnIncludeStartandStopTimes"]).Checked;
                    break;
                case "btnIncludeStackTrace":
                    SQLMonitorList.includeStackTrace = ((Infragistics.Win.UltraWinToolbars.StateButtonTool)this.ultraToolbarsManager1.Tools["btnIncludeStackTrace"]).Checked;
                    break;
                case "btnIncludeMessageListening":
                    SQLMonitorList.includeMessageListening = ((Infragistics.Win.UltraWinToolbars.StateButtonTool)this.ultraToolbarsManager1.Tools["btnIncludeMessageListening"]).Checked;
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
