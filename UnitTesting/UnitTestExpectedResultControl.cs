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
    public partial class UnitTestExpectedResultControl : UserControl
    {
        public UnitTestExpectedResultControl()
        {
            InitializeComponent();
        }
        private string testName;
        private string procedureName;
        //private DataSet dsTempExpectedResults;
        public void BindGrid(string testName, string procedureName)
        {
            this.testName = testName;
            this.procedureName = procedureName;
            //this.dsTempExpectedResults = aDataSet;
            //ultraGrid1.DataSource = null;

           // BindingSource bs = new BindingSource(aDataSet, aDataSet.Tables[0].TableName);
            UnitTests.UpdateExpectedResultsForTest(testName, procedureName);
            this.ultraGrid1.DataSource = UnitTests.dsExpectedResultsForTest;
            PerformAutoResize();
            SelectFirstRow();
        }
        private void SelectFirstRow()
        {
            if (this.ultraGrid1.Rows.Count > 0)
            {
                this.ultraGrid1.Rows[0].Selected = true;
        
            }
        }
        private void PerformAutoResize()
        {
            this.ultraGrid1.DisplayLayout.Bands[0].PerformAutoResizeColumns(false, Infragistics.Win.UltraWinGrid.PerformAutoSizeType.AllRowsInBand);
        }
        private void TestManagerControl_Load(object sender, EventArgs e)
        {
            Infragistics.Shared.ResourceCustomizer rc = Infragistics.Win.UltraWinGrid.Resources.Customizer;
            rc.SetCustomizedString("GroupByBoxDefaultPrompt", "Drag a column here to group by that column.");
            //rc.SetCustomizedString("ColumnChooserButtonToolTip", "Click here to show/hide columns");
            //rc.SetCustomizedString("ColumnChooserDialogCaption", "Choose Columns");
            //this.ultraGrid1.DisplayLayout.Override.RowSelectorHeaderStyle = Infragistics.Win.UltraWinGrid.RowSelectorHeaderStyle.ColumnChooserButton;


            this.ultraGrid1.DisplayLayout.Override.AllowAddNew = Infragistics.Win.UltraWinGrid.AllowAddNew.No;
            this.ultraGrid1.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            this.ultraGrid1.DisplayLayout.Override.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.True;
            this.ultraGrid1.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.False;
        }
        private void ultraGrid1_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            //e.Layout.Bands[0].Columns["AUDIT_DATETIME"].Format = "MM/dd/yyyy HH:mm:ss";
            e.Layout.Bands[0].Columns["testName"].Hidden = true;
            e.Layout.Bands[0].Columns["procedureName"].Hidden = true;

            e.Layout.Bands[0].Columns["resultKind"].Header.Caption = "Result Kind";
            e.Layout.Bands[0].Columns["fieldName"].Header.Caption = "Field";
            e.Layout.Bands[0].Columns["expectedValue"].Header.Caption = "Expected Value";
            e.Layout.Bands[0].Columns["mainRowIndex"].Hidden = true;
           
        }
        public void HideEditButtons()
        {
            this.ultraToolbarsManager1.Tools["btnAdd"].SharedProps.Visible = false;
            this.ultraToolbarsManager1.Tools["btnEdit"].SharedProps.Visible = false;
            this.ultraToolbarsManager1.Tools["btnCopy"].SharedProps.Visible = false;
            this.ultraToolbarsManager1.Tools["btnDelete"].SharedProps.Visible = false;

        }
        private void ultraToolbarsManager1_ToolClick(object sender, Infragistics.Win.UltraWinToolbars.ToolClickEventArgs e)
        {
            switch (e.Tool.Key)
            {
                case "btnAddRowCountEqualsX":
                    AddRowCountEqualsX();
                    break;
                case "btnAddRowCountEqualsOne":
                    AddRowCountEqualsOne();
                    break;
                case "btnAddCompareValue":
                    AddCompareValue();
                    break;
                case "btnAddRowCountGreaterThanZero":
                    AddRowCountGreaterThanZero();
                    break;
                case "btnFieldEquals":
                    AddFieldEquals();
                    break;
                case "btnEdit":
                    EditExpectedResult();
                    break;
                case "btnDelete":
                    DeleteExpectedResult();
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

                    SharedRoutines.GridExport.EmailSelectedRows("Unit Test Expected Results", "Unit Test Expected Results.xls", this.ultraGrid1);
                    break;

                case "gridEmailAllRows":
                    //UltraGridExcelExportWrapper exporter4 = new UltraGridExcelExportWrapper(this.ultraGrid1);
                    //ShowEmailForm(exporter4.ExportAllRowsToExcelAsAttachment());
                    SharedRoutines.GridExport.EmailAllRows("Unit Test Expected Results", "Unit Test Expected Results.xls", this.ultraGrid1);
                    break;



                #endregion

            }
        }
        private void AddRowCountEqualsX()
        {
            ExpectedResults_AddForm frm = new ExpectedResults_AddForm();
            frm.expectedResult_AddControl1.SetEditMode(ExpectedResult_AddControl.EditModes.Add);
            frm.expectedResult_AddControl1.SetExpectedResultKind(testName, procedureName, UnitTests.ExpectedResultKinds.RowCountEqualsX);
            frm.ShowDialog();
            if (frm.OK)
            {
                int rowCount;
                int.TryParse(frm.expectedResult_AddControl1.expectedValue, out rowCount);
                UnitTests.ExpectedResults_AddRowCountEqualsX(testName, procedureName, rowCount);
                PerformAutoResize();
                SelectFirstRow();
            }
        }
        private void AddCompareValue()
        {
            ExpectedResults_AddForm frm = new ExpectedResults_AddForm();
            frm.expectedResult_AddControl1.SetEditMode(ExpectedResult_AddControl.EditModes.Add);
            frm.expectedResult_AddControl1.SetExpectedResultKind(testName, procedureName, UnitTests.ExpectedResultKinds.CompareScalarValue);
            frm.ShowDialog();
            if (frm.OK)
            {
                UnitTests.ExpectedResults_AddCompareValue(testName, procedureName, frm.expectedResult_AddControl1.expectedValue);
                PerformAutoResize();
                SelectFirstRow();
            }
        }
        private void AddRowCountEqualsOne()
        {
            UnitTests.ExpectedResults_AddRowCountEqualsOne(testName, procedureName);
            PerformAutoResize();
            SelectFirstRow();
        }
        private void AddRowCountGreaterThanZero()
        {
            UnitTests.ExpectedResults_AddRowCountGreaterThanZero(testName, procedureName);
            PerformAutoResize();
            SelectFirstRow();
        }
        private void AddFieldEquals()
        {
            ExpectedResults_AddForm frm = new ExpectedResults_AddForm();
            frm.expectedResult_AddControl1.SetEditMode(ExpectedResult_AddControl.EditModes.Add);
            frm.expectedResult_AddControl1.SetExpectedResultKind(testName, procedureName, UnitTests.ExpectedResultKinds.FieldEquals);
            frm.ShowDialog();
            if (frm.OK)
            {
                string fieldName = frm.expectedResult_AddControl1.expectedField;
                string fieldValue = frm.expectedResult_AddControl1.expectedValue;
                UnitTests.ExpectedResults_AddFieldEquals(testName, procedureName, fieldName, fieldValue);
                PerformAutoResize();
                SelectFirstRow();
            }
        }
        public DataRow[] GetSelectedRowsOnGrid()
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
        private void DeleteExpectedResult()
        {
            if (this.ultraGrid1.Selected.Rows.Count > 0)
            {
                if (UnitTests.promptOnDelete == true)
                {
                    if (MessageBox.Show("Delete expected result?", "Confirm:", MessageBoxButtons.YesNo) != DialogResult.Yes)
                    {
                        return;
                    }
                }
                UnitTests.ExpectedResults_Delete(procedureName, testName, GetSelectedRowsOnGrid());
                SelectFirstRow();
                //SelectFirstRow();
            }
            else
            {
                MessageBox.Show("Please select one or more expected results to delete.");
            }
        }
        private void EditExpectedResult()
        {
            if (this.ultraGrid1.Selected.Rows.Count == 0)
            {
                MessageBox.Show("Please first select an expected result to edit.");
                return;
            }
            Infragistics.Win.UltraWinGrid.UltraGridRow urFirst = this.ultraGrid1.Selected.Rows[0];
            if (urFirst.ListObject != null)
            {
                DataRow drFirstSelected = ((DataRowView)urFirst.ListObject).Row;
                string testName = (string)drFirstSelected["testName"];
                string procedureName = (string)drFirstSelected["procedureName"];

                //UnitTest_AddForm frmAddTest = new UnitTest_AddForm();
                //frmAddTest.Text = "Edit Unit Test";
                //frmAddTest.unitTest_AddControl1.SetProcedureName(procedureName);
                ////frmAddTest.unitTest_AddControl1.FillParametersFromProcedure(procedureName);
                //frmAddTest.unitTest_AddControl1.SetEditMode(UnitTest_AddControl.EditModes.Edit);
                //frmAddTest.unitTest_AddControl1.LoadTest(testName, procedureName);
                //frmAddTest.ShowDialog();

                string resultKind = (string)drFirstSelected["resultKind"];
                if (resultKind == UnitTests.ExpectedResultKinds.FieldEquals.Name)
                {
                    ExpectedResults_AddForm frm = new ExpectedResults_AddForm();
                    frm.expectedResult_AddControl1.SetEditMode(ExpectedResult_AddControl.EditModes.Edit);
                    frm.expectedResult_AddControl1.SetExpectedResultKind(testName, procedureName, UnitTests.ExpectedResultKinds.FieldEquals);
                    frm.expectedResult_AddControl1.LoadExpectedResult(drFirstSelected);
                    frm.ShowDialog();
                    if (frm.OK)
                    {
                        string fieldName = frm.expectedResult_AddControl1.expectedField;
                        string fieldValue = frm.expectedResult_AddControl1.expectedValue;
                        drFirstSelected["fieldName"] = fieldName;
                        drFirstSelected["expectedValue"] = fieldValue;
                        PerformAutoResize();
                        int mainRowIndex = (int)drFirstSelected["mainRowIndex"];
                        UnitTests.ExpectedResults_Update(procedureName, testName, mainRowIndex, drFirstSelected);
                    }
                    
                }
                if (resultKind == UnitTests.ExpectedResultKinds.OutputParameterEquals.Name)
                {
                    ExpectedResults_AddForm frm = new ExpectedResults_AddForm();
                    frm.expectedResult_AddControl1.SetEditMode(ExpectedResult_AddControl.EditModes.Edit);
                    frm.expectedResult_AddControl1.SetExpectedResultKind(testName, procedureName, UnitTests.ExpectedResultKinds.OutputParameterEquals);
                    frm.expectedResult_AddControl1.LoadExpectedResult(drFirstSelected);
                    frm.ShowDialog();
                    if (frm.OK)
                    {
                        string fieldName = frm.expectedResult_AddControl1.expectedField;
                        string fieldValue = frm.expectedResult_AddControl1.expectedValue;
                        drFirstSelected["fieldName"] = fieldName;
                        drFirstSelected["expectedValue"] = fieldValue;
                        PerformAutoResize();
                        int mainRowIndex = (int)drFirstSelected["mainRowIndex"];
                        UnitTests.ExpectedResults_Update(procedureName, testName, mainRowIndex, drFirstSelected);
                    }

                }
                if (resultKind == UnitTests.ExpectedResultKinds.RowCountEqualsOne.Name)
                {
                    ExpectedResults_AddForm frm = new ExpectedResults_AddForm();
                    frm.expectedResult_AddControl1.SetEditMode(ExpectedResult_AddControl.EditModes.Edit);
                    frm.expectedResult_AddControl1.SetExpectedResultKind(testName, procedureName, UnitTests.ExpectedResultKinds.RowCountEqualsOne);
                    frm.expectedResult_AddControl1.LoadExpectedResult(drFirstSelected);
                    frm.ShowDialog();
                    if (frm.OK)
                    {
                        int rowCount;
                        int.TryParse(frm.expectedResult_AddControl1.expectedValue, out rowCount);
                        drFirstSelected["expectedValue"] = rowCount;
                        PerformAutoResize();
                        int mainRowIndex = (int)drFirstSelected["mainRowIndex"];
                        UnitTests.ExpectedResults_Update(procedureName, testName, mainRowIndex, drFirstSelected);
                    }
                }
                if (resultKind == UnitTests.ExpectedResultKinds.RowCountGreaterThanZero.Name)
                {
                    ExpectedResults_AddForm frm = new ExpectedResults_AddForm();
                    frm.expectedResult_AddControl1.SetEditMode(ExpectedResult_AddControl.EditModes.Edit);
                    frm.expectedResult_AddControl1.SetExpectedResultKind(testName, procedureName, UnitTests.ExpectedResultKinds.RowCountGreaterThanZero);
                    frm.expectedResult_AddControl1.LoadExpectedResult(drFirstSelected);
                    frm.ShowDialog();
                    if (frm.OK)
                    {
                        int rowCount;
                        int.TryParse(frm.expectedResult_AddControl1.expectedValue, out rowCount);
                        drFirstSelected["expectedValue"] = rowCount;
                        PerformAutoResize();
                        int mainRowIndex = (int)drFirstSelected["mainRowIndex"];
                        UnitTests.ExpectedResults_Update(procedureName, testName, mainRowIndex, drFirstSelected);
                    }
                }
                if (resultKind == UnitTests.ExpectedResultKinds.RowCountEqualsX.Name)
                {
                    ExpectedResults_AddForm frm = new ExpectedResults_AddForm();
                    frm.expectedResult_AddControl1.SetEditMode(ExpectedResult_AddControl.EditModes.Edit);
                    frm.expectedResult_AddControl1.SetExpectedResultKind(testName, procedureName, UnitTests.ExpectedResultKinds.RowCountEqualsX);
                    frm.expectedResult_AddControl1.LoadExpectedResult(drFirstSelected);
                    frm.ShowDialog();
                    if (frm.OK)
                    {
                        int rowCount;
                        int.TryParse(frm.expectedResult_AddControl1.expectedValue, out rowCount);
                        drFirstSelected["expectedValue"] = rowCount;
                        PerformAutoResize();
                        int mainRowIndex = (int)drFirstSelected["mainRowIndex"];
                        UnitTests.ExpectedResults_Update(procedureName, testName, mainRowIndex, drFirstSelected);
                    }
                }
                if (resultKind == UnitTests.ExpectedResultKinds.CompareScalarValue.Name)
                {
                    ExpectedResults_AddForm frm = new ExpectedResults_AddForm();
                    frm.expectedResult_AddControl1.SetEditMode(ExpectedResult_AddControl.EditModes.Edit);
                    frm.expectedResult_AddControl1.SetExpectedResultKind(testName, procedureName, UnitTests.ExpectedResultKinds.CompareScalarValue);
                    frm.expectedResult_AddControl1.LoadExpectedResult(drFirstSelected);
                    frm.ShowDialog();
                    if (frm.OK)
                    {
                        drFirstSelected["expectedValue"] = frm.expectedResult_AddControl1.expectedValue;
                        PerformAutoResize();
                        int mainRowIndex = (int)drFirstSelected["mainRowIndex"];
                        UnitTests.ExpectedResults_Update(procedureName, testName, mainRowIndex, drFirstSelected);
                    }
                }
            }
        }
    }
}
