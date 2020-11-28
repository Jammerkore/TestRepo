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
    public partial class UnitTestGridControl : UserControl
    {
        public UnitTestGridControl()
        {
            InitializeComponent();
        }
        public void BindGrid(DataSet aDataSet)
        {

            ultraGrid1.DataSource = null;

            BindingSource bs = new BindingSource(aDataSet, aDataSet.Tables[0].TableName);
            this.ultraGrid1.DataSource = bs;
            this.ultraGrid1.DisplayLayout.Bands[0].PerformAutoResizeColumns(false, Infragistics.Win.UltraWinGrid.PerformAutoSizeType.AllRowsInBand);


            SelectFirstRow();
        }
        private void SelectFirstRow()
        {
            if (this.ultraGrid1.Rows.Count > 0)
            {
                this.ultraGrid1.Rows[0].Selected = true;
                UpdateSelectedRows();
            }
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

            string layoutFile = System.IO.Path.GetTempPath() + "\\GridUnitTestViewer.xml";
            if (System.IO.File.Exists(layoutFile))
            {
                this.ultraGrid1.DisplayLayout.LoadFromXml(layoutFile);
            }
        }
        private void TestManagerControl_Leave(object sender, EventArgs e)
        {
            string layoutFile = System.IO.Path.GetTempPath() + "\\GridUnitTestViewer.xml";
            this.ultraGrid1.DisplayLayout.SaveAsXml(layoutFile);

        }
        private void ultraGrid1_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            //e.Layout.Bands[0].Columns["AUDIT_DATETIME"].Format = "MM/dd/yyyy HH:mm:ss";
            e.Layout.Bands[0].Columns["environmentName"].Header.Caption = "Env";
            e.Layout.Bands[0].Columns["testName"].Header.Caption = "Test";
            e.Layout.Bands[0].Columns["procedureName"].Header.Caption = "Procedure";
            e.Layout.Bands[0].Columns["procedureType"].Header.Caption = "Type";
            e.Layout.Bands[0].Columns["sequence"].Header.Caption = "Sequence";
            e.Layout.Bands[0].Columns["isSuspended"].Header.Caption = "Is Suspended";
            e.Layout.Bands[0].Columns["expectedResultCount"].Header.Caption = "Expected Result Count";
            e.Layout.Bands[0].Columns["validationMsg"].Header.Caption = "Validation Msg";
            //e.Layout.Bands[0].Columns["restoreState"].Hidden = true;
        }
        private void ultraToolbarsManager1_ToolClick(object sender, Infragistics.Win.UltraWinToolbars.ToolClickEventArgs e)
        {
            switch (e.Tool.Key)
            {
                case "btnEdit":
                    EditUnitTest();
                   
                    break;
                case "btnCopy":
                    CopyUnitTest();

                    break;
                case "btnDelete":
                    DeleteUnitTest();
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

                    SharedRoutines.GridExport.EmailSelectedRows("Unit Tests", "Unit Tests.xls", this.ultraGrid1);
                    break;

                case "gridEmailAllRows":
                    //UltraGridExcelExportWrapper exporter4 = new UltraGridExcelExportWrapper(this.ultraGrid1);
                    //ShowEmailForm(exporter4.ExportAllRowsToExcelAsAttachment());
                    SharedRoutines.GridExport.EmailAllRows("Unit Tests", "Unit Tests.xls", this.ultraGrid1);
                    break;
                case "btnResetLayout":
                    //UltraGridExcelExportWrapper exporter4 = new UltraGridExcelExportWrapper(this.ultraGrid1);
                    //ShowEmailForm(exporter4.ExportAllRowsToExcelAsAttachment());

                    ultraGrid1.DisplayLayout.ResetBands();
                    ultraGrid1_InitializeLayout(this, new Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs(ultraGrid1.DisplayLayout));
                    break;



                #endregion

            }
        }
        private void EditUnitTest()
        {
            if (this.ultraGrid1.Selected.Rows.Count == 0)
            {
                MessageBox.Show("Please first select a test to edit.");
                return;
            }
            Infragistics.Win.UltraWinGrid.UltraGridRow urFirst = this.ultraGrid1.Selected.Rows[0];
            if (urFirst.ListObject != null)
            {
                DataRow drFirstSelected = ((DataRowView)urFirst.ListObject).Row;
                string testName = (string)drFirstSelected["testName"];
                string procedureName = (string)drFirstSelected["procedureName"];

                UnitTest_AddForm frmAddTest = new UnitTest_AddForm();
                frmAddTest.Text = "Edit Unit Test";
                frmAddTest.unitTest_AddControl1.SetProcedureName(procedureName);
                //frmAddTest.unitTest_AddControl1.LoadParametersForProcedure(procedureName);
                frmAddTest.unitTest_AddControl1.SetEditMode(UnitTest_AddControl.EditModes.Edit);
                frmAddTest.unitTest_AddControl1.LoadTest(testName, procedureName);
                frmAddTest.ShowDialog();
                RaiseSelectedUnitTestChangedEvent(drFirstSelected);  //force a rebind of parameter control and expected results control
            }
        }
        private void CopyUnitTest()
        {
            if (this.ultraGrid1.Selected.Rows.Count == 0)
            {
                MessageBox.Show("Please first select a test to copy.");
                return;
            }
            Infragistics.Win.UltraWinGrid.UltraGridRow urFirst = this.ultraGrid1.Selected.Rows[0];
            if (urFirst.ListObject != null)
            {
                DataRow drFirstSelected = ((DataRowView)urFirst.ListObject).Row;
                string testName = (string)drFirstSelected["testName"];
                string procedureName = (string)drFirstSelected["procedureName"];

                UnitTest_AddForm frmAddTest = new UnitTest_AddForm();
                frmAddTest.Text = "Add copy of Unit Test";
                frmAddTest.unitTest_AddControl1.SetProcedureName(procedureName);
                //frmAddTest.unitTest_AddControl1.FillParametersFromProcedure(procedureName);
                frmAddTest.unitTest_AddControl1.SetEditMode(UnitTest_AddControl.EditModes.Copy);
                frmAddTest.unitTest_AddControl1.LoadTest(testName, procedureName);
                frmAddTest.ShowDialog();
                RaiseSelectedUnitTestChangedEvent(drFirstSelected);  //force a rebind of parameter control and expected results control
            }
        }
        private void DeleteUnitTest()
        {
            if (this.ultraGrid1.Selected.Rows.Count > 0)
            {
                if (UnitTests.promptOnDelete == true)
                {
                    if (MessageBox.Show("Delete these " + this.ultraGrid1.Selected.Rows.Count.ToString() + " test(s)?", "Confirm:", MessageBoxButtons.YesNo) != DialogResult.Yes)
                    {
                        return;
                    }
                }
                UnitTests.DeleteSelectedTests(GetSelectedUnitTests());
                SelectFirstRow();
            }
            else
            {
                MessageBox.Show("Please select one or more unit tests to delete.");
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
                    DataRow drFirstUnitTestSelected = ((DataRowView)urFirst.ListObject).Row;
                    RaiseSelectedUnitTestChangedEvent(drFirstUnitTestSelected);
                }
            }
        }
        public DataRow[] GetSelectedUnitTests()
        {
            //List<int> headerRidList = new List<int>();
            List<DataRow> drList = new List<DataRow>();
            foreach (Infragistics.Win.UltraWinGrid.UltraGridRow ur in this.ultraGrid1.Selected.Rows)
            {
                if (ur.ListObject != null)
                {
                    DataRow dr = ((DataRowView)ur.ListObject).Row;
                    drList.Add(dr);
                }
            }

            //return headerRidList;
            return drList.ToArray<DataRow>();
        }
        public DataRow[] GetAllUnitTests()
        {
            //List<int> headerRidList = new List<int>();
            List<DataRow> drList = new List<DataRow>();
            foreach (Infragistics.Win.UltraWinGrid.UltraGridRow ur in this.ultraGrid1.Rows)
            {
                if (ur.ListObject != null)
                {
                    DataRow dr = ((DataRowView)ur.ListObject).Row;
                    drList.Add(dr);
                }
            }

            //return headerRidList;
            return drList.ToArray<DataRow>();
        }
        public event SelectedUnitTestChangedEventHandler SelectedUnitTestChangedEvent;
        public virtual void RaiseSelectedUnitTestChangedEvent(DataRow drUnitTest)
        {

            if (SelectedUnitTestChangedEvent != null)
                SelectedUnitTestChangedEvent(this, new SelectedUnitTestChangedEventArgs(drUnitTest));
        }
        public class SelectedUnitTestChangedEventArgs
        {
            public SelectedUnitTestChangedEventArgs(DataRow drUnitTest) { this.drUnitTest = drUnitTest; }
            public DataRow drUnitTest { get; private set; } // readonly

        }
        public delegate void SelectedUnitTestChangedEventHandler(object sender, SelectedUnitTestChangedEventArgs e);

       
    }
}
