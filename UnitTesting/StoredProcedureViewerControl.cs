using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MIDRetail.Windows;

namespace UnitTesting
{
    public partial class StoredProcedureViewerControl : UserControl
    {
        public StoredProcedureViewerControl()
        {
            InitializeComponent();
        }
        private DataSet dsStoredProcedures;
        public void BindGrid(DataSet aDataSet)
        {
            dsStoredProcedures = aDataSet;
            ultraGrid1.DataSource = null;

            BindingSource bs = new BindingSource(aDataSet, aDataSet.Tables[0].TableName);
            this.ultraGrid1.DataSource = bs;
            this.ultraGrid1.DisplayLayout.Bands[0].PerformAutoResizeColumns(false, Infragistics.Win.UltraWinGrid.PerformAutoSizeType.AllRowsInBand);
            this.ultraGrid1.Rows[0].Selected = true;
        }
        private void TestManagerControl_Load(object sender, EventArgs e)
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

            string layoutFile = System.IO.Path.GetTempPath() + "\\GridProcedureViewer.xml";
            if (System.IO.File.Exists(layoutFile))
            {
                this.ultraGrid1.DisplayLayout.LoadFromXml(layoutFile);
            }

         this.unitTestGridControl1.SelectedUnitTestChangedEvent += new UnitTestGridControl.SelectedUnitTestChangedEventHandler(Handle_UnitTestChanged);
        }
        private void ultraGrid1_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            //e.Layout.Bands[0].Columns["AUDIT_DATETIME"].Format = "MM/dd/yyyy HH:mm:ss";
           // e.Layout.Bands[0].Columns["testName"].Header.Caption = "Test";
           // e.Layout.Bands[0].Columns["procedureName"].Hidden = true;
            e.Layout.Bands[0].Columns["moduleName"].Header.Caption = "Module";
            e.Layout.Bands[0].Columns["procedureName"].Header.Caption = "Procedure";
            e.Layout.Bands[0].Columns["procedureType"].Header.Caption = "Type";
            e.Layout.Bands[0].Columns["tableNames"].Header.Caption = "Table";
            e.Layout.Bands[0].Columns["testCount"].Header.Caption = "Test Count";
            e.Layout.Bands[0].Columns["expectedResultCount"].Header.Caption = "Expected Result Count";
            e.Layout.Bands[0].Columns["hasDefaults"].Hidden = true;
            e.Layout.Bands[0].Columns["hasNoLock"].Header.Caption = "Has NoLock";
            e.Layout.Bands[0].Columns["hasRowLock"].Header.Caption = "Has RowLock";
            e.Layout.Bands[0].Columns["hasOrderBy"].Header.Caption = "Has Order By";
            e.Layout.Bands[0].Columns["canRestoreState"].Header.Caption = "Can Reset State";
            e.Layout.Bands[0].Columns["canRestoreState"].Hidden = true;
            e.Layout.Bands[0].Columns["validation"].Header.Caption = "Validation Msg";
          
            //e.Layout.Bands[0].Columns["moduleName"].AutoSizeMode = Infragistics.Win.UltraWinGrid.ColumnAutoSizeMode.AllRowsInBand;
            //e.Layout.Bands[0].Columns["procedureName"].AutoSizeMode = Infragistics.Win.UltraWinGrid.ColumnAutoSizeMode.AllRowsInBand;
            //e.Layout.Bands[0].Columns["testCount"].AutoSizeMode = Infragistics.Win.UltraWinGrid.ColumnAutoSizeMode.AllRowsInBand;
        }
        private void ultraToolbarsManager1_ToolClick(object sender, Infragistics.Win.UltraWinToolbars.ToolClickEventArgs e)
        {
            switch (e.Tool.Key)
            {
                case "btnRefresh":
                    UnitTests.RefreshStoredProcedures();
                    break;

                case "btnAdd":

                    if (this.ultraGrid1.Selected.Rows.Count > 0)
                    {
                        Infragistics.Win.UltraWinGrid.UltraGridRow urFirst = this.ultraGrid1.Selected.Rows[0];
                        if (urFirst.ListObject != null)
                        {
                            DataRow drFirstProcedureSelected = ((DataRowView)urFirst.ListObject).Row;
                            UnitTest_AddForm frmAddTest = new UnitTest_AddForm();
                            string procedureName = (string)drFirstProcedureSelected["procedureName"];
                            frmAddTest.Text = "Add Unit Test for " + procedureName;
                            //GenerateTestNameFromProcedure(environmentName, procedureName, "-RowCount");
                            frmAddTest.unitTest_AddControl1.SetProcedureName(procedureName);
                            frmAddTest.unitTest_AddControl1.GenerateTestName();
                            frmAddTest.unitTest_AddControl1.FillParametersFromProcedure(procedureName);
                            frmAddTest.unitTest_AddControl1.SetEditMode(UnitTest_AddControl.EditModes.Add);
                           
                            frmAddTest.ShowDialog();
                            UpdateSelectedRows();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Please select a stored procedure.");
                    }
                  
                    break;
                //case "btnAutoGenerate":
                //    if (this.ultraGrid1.Selected.Rows.Count > 0)
                //    {
                //        SelectEnvironmentForm frm = new SelectEnvironmentForm();
                //        frm.ShowDialog();
                //        if (frm.selectedEnvironment != string.Empty)
                //        {
                //            string environmentName = frm.selectedEnvironment;
                //            UnitTests.AutoGenerateTests(environmentName, GetSelectedStoredProcedures());
                //            //GetSelectedStoredProcedures
                //            RaiseShowUnitTestTab_Event();
                //        }
                //    }
                //    else
                //    {
                //        MessageBox.Show("Please select one or more stored procedures.");
                //    }
                //    break;
                case "btnEditStoredProcedure":
                    if (this.ultraGrid1.Selected.Rows.Count > 0)
                    {
                        EditStoredProcedures(GetSelectedStoredProcedures());
                    }
                    else
                    {
                        MessageBox.Show("Please select one or more stored procedures.");
                    }
                    break;
                case "btnTop100":
                    if (this.ultraGrid1.Selected.Rows.Count > 0)
                    {
                        ShowTop100(GetSelectedStoredProcedures());
                    }
                    else
                    {
                        MessageBox.Show("Please select one or more stored procedures.");
                    }
                    break;
                case "btnProceduresInProjectNotReferenced":
                    Raise_MakeReport_Event(UnitTests.ReportTypes.ProceduresNotReferenced);
                    break;
                case "btnParameterListing":
                    Raise_MakeReport_Event(UnitTests.ReportTypes.ParameterListing);
                    break;
                case "btnDatabaseComparison":
                    Raise_MakeReport_Event(UnitTests.ReportTypes.DatabaseComparison);
                    break;
                case "btnObjectDBOCheck":
                    Raise_MakeReport_Event(UnitTests.ReportTypes.ObjectDBOCheck);
                    break;
                case "btnManageEnvironments":
                    PopUpEnvironmentControl();
                    break;

                #region "Grid Tools"

                case "gridSearchFindButton":
                    MIDRetail.Windows.Controls.SharedControlRoutines.SearchGrid(ultraGrid1, (((Infragistics.Win.UltraWinToolbars.TextBoxTool)this.ultraToolbarsManager1.Toolbars["Grid Search Toolbar"].Tools["gridSearchText"]).Text));
                    break;
                case "gridSearchClearButton":
                    Infragistics.Win.UltraWinToolbars.TextBoxTool t = (Infragistics.Win.UltraWinToolbars.TextBoxTool)this.ultraToolbarsManager1.Toolbars["Grid Search Toolbar"].Tools["gridSearchText"];
                    t.Text = "";
                    MIDRetail.Windows.Controls.SharedControlRoutines.ClearGridSearchResults(ultraGrid1);
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
                    //UltraGridExcelExportWrapper exporter = new UltraGridExcelExportWrapper(this.ultraGrid1);
                    //exporter.ExportSelectedRowsToExcel();
                    SharedRoutines.GridExport.ExportSelectedRowsToExcel(this.ultraGrid1);
                    break;

                case "gridExportAll":
                    //UltraGridExcelExportWrapper exporter2 = new UltraGridExcelExportWrapper(this.ultraGrid1);
                    //exporter2.ExportAllRowsToExcel();
                    SharedRoutines.GridExport.ExportAllRowsToExcel(this.ultraGrid1);
                    break;

                case "gridEmailSelectedRows":
                    //UltraGridExcelExportWrapper exporter3 = new UltraGridExcelExportWrapper(this.ultraGrid1);
                    //ShowEmailForm(exporter3.ExportSelectedRowsToExcelAsAttachment());

                    SharedRoutines.GridExport.EmailSelectedRows("Stored Procedures", "StoredProcedures.xls", this.ultraGrid1);
                    break;

                case "gridEmailAllRows":
                    //UltraGridExcelExportWrapper exporter4 = new UltraGridExcelExportWrapper(this.ultraGrid1);
                    //ShowEmailForm(exporter4.ExportAllRowsToExcelAsAttachment());
                    SharedRoutines.GridExport.EmailAllRows("Stored Procedures", "StoredProcedures.xls", this.ultraGrid1);
                    break;

                

                #endregion

            }
        }
        private void PopUpEnvironmentControl()
        {
            EnvironmentForm ef = new EnvironmentForm();
            ef.ShowDialog();

        }
        private void EditStoredProcedures(DataRow[] drProcedures)
        {
            foreach (DataRow dr in drProcedures)
            {
           
                string procedureName = (string)dr["procedureName"];
                string sPath = Shared_UtilityFunctions.GetCurrentProjectPath() + "DatabaseDefinition\\SQL_StoredProcedures\\" + procedureName + ".SQL";

                if (System.IO.File.Exists(sPath) == true)
                {
                    System.Diagnostics.Process.Start(sPath);
                }
            }
        }
        private void ShowTop100(DataRow[] drProcedures)
        {
            foreach (DataRow dr in drProcedures)
            {
                string procedureName = (string)dr["procedureName"];
                MIDRetail.Data.baseStoredProcedure bp = Shared_BaseStoredProcedures.GetStoredProcedure(procedureName);
                if (bp.tableNames.Count > 0)
                {
                    string top100fileName = procedureName + "_top100.SQL";
                    string fullpath = string.Empty;

                    DataRow drEnv = UnitTests.GetEnvironmentFromName(UnitTests.defaultEnvironment);
                    string sbody = "USE [" + (string)drEnv["databaseName"] + "]" + System.Environment.NewLine + System.Environment.NewLine;
                    sbody += "SELECT TOP 100 * FROM " + bp.tableNames[0];

                    UnitTests.SaveTempSQLFile(sbody, top100fileName, out fullpath);
                    if (System.IO.File.Exists(fullpath) == true)
                    {
                        System.Diagnostics.Process.Start(fullpath);
                    }
                }
            }
        }
      
        private void ultraGrid1_AfterSelectChange(object sender, Infragistics.Win.UltraWinGrid.AfterSelectChangeEventArgs e)
        {
            UpdateSelectedRows();
        }
        private void UpdateSelectedRows()
        {
            if (this.ultraGrid1.Selected.Rows.Count > 0)
            {
                Infragistics.Win.UltraWinGrid.UltraGridRow urFirst = this.ultraGrid1.Selected.Rows[0];
                if (urFirst.ListObject != null)
                {
                    DataRow drFirstSelected = ((DataRowView)urFirst.ListObject).Row;
                    string procedureName = (string)drFirstSelected["procedureName"];
                    this.txtProcedureName.Text = procedureName;
                    this.unitTestGridControl1.BindGrid(UnitTests.GetTestsForProcedure(procedureName));
            
                }
            }
        }
        private void Handle_UnitTestChanged(object sender, UnitTestGridControl.SelectedUnitTestChangedEventArgs e)
        {
            if (this.ultraGrid1.Selected.Rows.Count > 0)
            {
                Infragistics.Win.UltraWinGrid.UltraGridRow urFirst = this.ultraGrid1.Selected.Rows[0];
                if (urFirst.ListObject != null)
                {
                    DataRow drFirstSelected = ((DataRowView)urFirst.ListObject).Row;
                    string procedureName = (string)drFirstSelected["procedureName"];
                    string testName = (string)e.drUnitTest["testName"];
                    this.unitTestExpectedResultControl1.BindGrid(testName, procedureName);
                }
            }
        }
        public DataRow[] GetSelectedStoredProcedures()
        {
            List<DataRow> drList = new List<DataRow>();
            foreach (Infragistics.Win.UltraWinGrid.UltraGridRow ur in this.ultraGrid1.Selected.Rows)
            {
                if (ur.ListObject != null)
                {
                    DataRow dr = ((DataRowView)ur.ListObject).Row;
                    drList.Add(dr);
                }
            }
            return drList.ToArray<DataRow>();
        }

        public event ShowUnitTestTab_EventHandler ShowUnitTestTab_Event;
        public virtual void RaiseShowUnitTestTab_Event()
        {

            if (ShowUnitTestTab_Event != null)
                ShowUnitTestTab_Event(this, new ShowUnitTestTab_EventArgs());
        }
        public class ShowUnitTestTab_EventArgs
        {
            public ShowUnitTestTab_EventArgs() { }
        }
        public delegate void ShowUnitTestTab_EventHandler(object sender, ShowUnitTestTab_EventArgs e);



        public event MakeReport_EventHandler MakeReport_Event;
        public virtual void Raise_MakeReport_Event(UnitTests.ReportTypes ReportType)
        {

            if (MakeReport_Event != null)
                MakeReport_Event(this, new MakeReport_EventArgs(ReportType));
        }
        public class MakeReport_EventArgs
        {
            public MakeReport_EventArgs(UnitTests.ReportTypes ReportType) { this.ReportType = ReportType; }
            public UnitTests.ReportTypes ReportType { get; private set; }
        }
        public delegate void MakeReport_EventHandler(object sender, MakeReport_EventArgs e);

        private void TestManagerControl_Leave(object sender, EventArgs e)
        {
            string layoutFile = System.IO.Path.GetTempPath() +"\\GridProcedureViewer.xml";
            this.ultraGrid1.DisplayLayout.SaveAsXml(layoutFile);

        }
    }
}
