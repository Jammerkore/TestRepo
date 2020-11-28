using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MIDRetail.Data;

namespace UnitTesting
{
    public partial class ViewExecutionGridControl : UserControl
    {
        public ViewExecutionGridControl()
        {
            InitializeComponent();

            DataSet dsEnvironment = UnitTests.GetEnvironmentsForSelection();
            this.ultraCombo1.SetDataBinding(dsEnvironment, "Environments");
            this.ultraCombo1.Text = UnitTests.defaultEnvironment;
        }
        private void ultraGrid1_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            //e.Layout.Bands[0].Columns["AUDIT_DATETIME"].Format = "MM/dd/yyyy HH:mm:ss";
            //e.Layout.Bands[0].Columns["environmentName"].Header.Caption = "DB";
            //e.Layout.Bands[0].Columns["testName"].Header.Caption = "Test";
            //e.Layout.Bands[0].Columns["procedureName"].Header.Caption = "Procedure";
            //e.Layout.Bands[0].Columns["passFail"].Header.Caption = "Pass/Fail";
            //e.Layout.Bands[0].Columns["failureMessage"].Header.Caption = "Failure Msg";
        }
        private void ultraToolbarsManager1_ToolClick(object sender, Infragistics.Win.UltraWinToolbars.ToolClickEventArgs e)
        {
            switch (e.Tool.Key)
            {
                case "btnInsertFieldMatching":
                    InsertFieldMatching();
                    break;
                case "btnInsertRowCount":
                    InsertRowCount();
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
                    MIDRetail.Windows.SharedRoutines.GridExport.ExportSelectedRowsToExcel(this.ultraGrid1);
                    break;

                case "gridExportAll":
                    //UltraGridExcelExportWrapper exporter2 = new UltraGridExcelExportWrapper(this.ultraGrid1);
                    //exporter2.ExportAllRowsToExcel();
                    MIDRetail.Windows.SharedRoutines.GridExport.ExportAllRowsToExcel(this.ultraGrid1);
                    break;

                case "gridEmailSelectedRows":
                    //UltraGridExcelExportWrapper exporter3 = new UltraGridExcelExportWrapper(this.ultraGrid1);
                    //ShowEmailForm(exporter3.ExportSelectedRowsToExcelAsAttachment());

                    MIDRetail.Windows.SharedRoutines.GridExport.EmailSelectedRows("Unit Test Results", "Unit Test Results.xls", this.ultraGrid1);
                    break;

                case "gridEmailAllRows":
                    //UltraGridExcelExportWrapper exporter4 = new UltraGridExcelExportWrapper(this.ultraGrid1);
                    //ShowEmailForm(exporter4.ExportAllRowsToExcelAsAttachment());
                    MIDRetail.Windows.SharedRoutines.GridExport.EmailAllRows("Unit Test Results", "Unit Test Results.xls", this.ultraGrid1);
                    break;



                #endregion
            }
        }
        private DataTable dtResultsShown;
        //public void LoadTestsToRun(DataRow[] drTestsToRun)
        //{
        //    dsResults.Tables.Add("Results");
        //    dsResults.Tables[0].Columns.Add("environmentName", typeof(string));
        //    dsResults.Tables[0].Columns.Add("testName", typeof(string));
        //    dsResults.Tables[0].Columns.Add("procedureName", typeof(string));
        //    dsResults.Tables[0].Columns.Add("passFail", typeof(string));
        //    dsResults.Tables[0].Columns.Add("failureMessage", typeof(string));

        //    foreach (DataRow dr in drTestsToRun)
        //    {
        //        DataRow drResult = dsResults.Tables[0].NewRow();

        //        drResult["environmentName"] = dr["environmentName"];
        //        drResult["testName"] = dr["testName"];
        //        drResult["procedureName"] = dr["procedureName"];

        //        dsResults.Tables[0].Rows.Add(drResult);
        //    }

        //    BindGrid(dsResults);
        //}
        private string testName;
        private string procedureName;
        public void BindGrid(string testName, string procedureName, DataSet aDataSet)
        {
            this.testName = testName;
            this.procedureName = procedureName;
            ultraGrid1.DataSource = null;
            this.dtResultsShown = aDataSet.Tables[0];
            BindingSource bs = new BindingSource(aDataSet, aDataSet.Tables[0].TableName);
            this.ultraGrid1.DataSource = bs;
            this.ultraGrid1.DisplayLayout.Bands[0].PerformAutoResizeColumns(false, Infragistics.Win.UltraWinGrid.PerformAutoSizeType.AllRowsInBand);

            
           // this.ultraGrid1.Rows[0].Selected = true;

        }
        public void BindGrid(string testName, string procedureName, DataTable aDataTable)
        {
            this.testName = testName;
            this.procedureName = procedureName;
            ultraGrid1.DataSource = null;
            this.dtResultsShown = aDataTable;
            BindingSource bs = new BindingSource(aDataTable, aDataTable.TableName);
            this.ultraGrid1.DataSource = bs;
            this.ultraGrid1.DisplayLayout.Bands[0].PerformAutoResizeColumns(false, Infragistics.Win.UltraWinGrid.PerformAutoSizeType.AllRowsInBand);


            // this.ultraGrid1.Rows[0].Selected = true;

        }
        //private void ExecuteStoredProcedure()
        //{
        //    string environmentName = this.ultraCombo1.Text;
        //    if (StoredProcedureTesting.DoesEnvironmentExist(environmentName) == false)
        //    {
        //        MessageBox.Show("Please select a valid DB.");
        //        return;
        //    }
        //    StoredProcedureTesting.defaultEnvironment = environmentName;
        //    DatabaseAccess dba = new DatabaseAccess(StoredProcedureTesting.GetConnectionForEnvironment(environmentName));
        //    DataRow[] tests = dsResults.Tables[0].Select("environmentName='Any' OR environmentName='" + environmentName + "'");
        //    StoredProcedureTesting.RunUnitTests(ref dsResults, tests, dba, environmentName);

        //}

        private void InsertFieldMatching()
        {
            if (dtResultsShown.Rows.Count > 0)
            {
                UnitTests.ExpectedResults_AddFieldsMatching(testName, procedureName, dtResultsShown.Rows[0]);
                RaiseInsertFromResultsEvent();
            }
            else
            {
                MessageBox.Show("There must be at least one row in the results first.");
            }
        }
        private void InsertRowCount()
        {
            //if (dtResultsShown.Rows.Count > 0)
            //{
                UnitTests.ExpectedResults_AddRowCountEqualsX(testName, procedureName, dtResultsShown.Rows.Count);
                RaiseInsertFromResultsEvent();
            //}
            //else
            //{
            //    MessageBox.Show("There must be at least one row in the results first.");
            //}
        }

        public event InsertFromResultsEventHandler InsertFromResultsEvent;
        public virtual void RaiseInsertFromResultsEvent()
        {

            if (InsertFromResultsEvent != null)
                InsertFromResultsEvent(this, new InsertFromResultsEventArgs());
        }
        public class InsertFromResultsEventArgs
        {
            public InsertFromResultsEventArgs() { }
        }
        public delegate void InsertFromResultsEventHandler(object sender, InsertFromResultsEventArgs e);
    }
}
