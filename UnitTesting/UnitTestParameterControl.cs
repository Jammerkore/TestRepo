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
    public partial class UnitTestParameterControl : UserControl
    {
        public UnitTestParameterControl()
        {
            InitializeComponent();
        }
        private string testName;
        private string procedureName;
        private DataSet dsParms;
        public void BindGrid(string testName, string procedureName, DataSet aDataSet)
        {
            this.testName = testName;
            this.procedureName = procedureName; 
            ultraGrid1.DataSource = null;
            dsParms = aDataSet;
            BindingSource bs = new BindingSource(dsParms, dsParms.Tables[0].TableName);
        
            //((Infragistics.Win.UltraWinToolbars.LabelTool)this.ultraToolbarsManager1.Tools["lblSpacer"]).SharedProps.Caption = "Procedure: " + procedureName; //(string)aDataSet.Tables[0].Rows[0]["procedureName"];
    
            this.ultraGrid1.DataSource = bs;
            this.ultraGrid1.DisplayLayout.Bands[0].PerformAutoResizeColumns(false, Infragistics.Win.UltraWinGrid.PerformAutoSizeType.AllRowsInBand);
     
        }
        public void HideSaveButton()
        {
            this.ultraToolbarsManager1.Tools["btnSave"].SharedProps.Visible = false;
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
            //this.ultraGrid1.DisplayLayout.Override.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.True;
            //this.ultraGrid1.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.False;
        }
        private void ultraGrid1_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            //e.Layout.Bands[0].Columns["AUDIT_DATETIME"].Format = "MM/dd/yyyy HH:mm:ss";
            //e.Layout.Bands[0].Columns["testName"].Header.Caption = "Test";
            e.Layout.Bands[0].Columns["testName"].Hidden = true;
            e.Layout.Bands[0].Columns["procedureName"].Hidden = true;
            e.Layout.Bands[0].Columns["parameterName"].Header.Caption = "Parameter";
            e.Layout.Bands[0].Columns["parameterName"].CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit;
            e.Layout.Bands[0].Columns["parameterName"].CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.CellSelect;
            e.Layout.Bands[0].Columns["parameterType"].Header.Caption = "Type";
            e.Layout.Bands[0].Columns["parameterType"].CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit;
            e.Layout.Bands[0].Columns["parameterType"].CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.CellSelect;
            e.Layout.Bands[0].Columns["parameterValue"].Header.Caption = "Value";
           // e.Layout.Bands[0].Columns["parameterValue"].AutoSizeMode = Infragistics.Win.UltraWinGrid.ColumnAutoSizeMode.None;
            e.Layout.Bands[0].Columns["parameterValue"].MinWidth = 200;
        }
        public void SaveParameters()
        {
            if (this.ultraGrid1.ActiveCell != null && this.ultraGrid1.ActiveCell.IsInEditMode)
            {
                ultraGrid1.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.ExitEditMode);
            }
            UnitTests.UpdateUnitTestParameters(testName, procedureName, this.dsParms);
        }
        private void ultraToolbarsManager1_ToolClick(object sender, Infragistics.Win.UltraWinToolbars.ToolClickEventArgs e)
        {
            switch (e.Tool.Key)
            {
                case "btnExecuteAndViewResults":
                    if (this.ultraGrid1.ActiveCell != null && this.ultraGrid1.ActiveCell.IsInEditMode)
                    {
                        ultraGrid1.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.ExitEditMode);
                    }

                    MIDRetail.Data.baseStoredProcedure sp = Shared_BaseStoredProcedures.GetStoredProcedure(procedureName);
                    if (sp.procedureType == MIDRetail.Data.storedProcedureTypes.Read || sp.procedureType == MIDRetail.Data.storedProcedureTypes.ReadAsDataset)
                    {
                        bool hasError = false;
                        string failureMsg = string.Empty;
                        DataSet dsOutputParameters;
                        DataTable dtResults = UnitTests.ExecuteRead(procedureName, dsParms, out hasError, out failureMsg, out dsOutputParameters);
                        if (hasError)
                        {
                            MessageBox.Show(failureMsg);
                        }
                        else
                        {
                            if (dtResults != null)
                            {
                                ViewExecutionForm frm = new ViewExecutionForm();
                                frm.BindGrid(testName, procedureName, dtResults);
                                frm.BindOutputParameters(testName, procedureName, dsOutputParameters);
                                frm.ShowDialog();
                                if (frm.didInsertFromResults)
                                {
                                    RaiseInsertFromResultsEvent();
                                }
                            }
                            else
                            {
                                failureMsg = "Execution did not produce a result set.";
                                MessageBox.Show(failureMsg);
                            }
                        }
                    }
                    else if (sp.procedureType == MIDRetail.Data.storedProcedureTypes.Insert)
                    {
                        bool hasError = false;
                        string failureMsg = string.Empty;
                        DataSet dsOutputParameters;
                        int rowsInserted = UnitTests.ExecuteInsert(procedureName, dsParms, out hasError, out failureMsg, out dsOutputParameters);
                        if (hasError)
                        {
                            MessageBox.Show(failureMsg);
                        }
                        else
                        {
                            //if (rowsInserted != 0)
                            //{
                                ViewExecutionRowCountForm frm = new ViewExecutionRowCountForm();
                                frm.SetMessage(testName, procedureName, rowsInserted.ToString() + " rows inserted.", rowsInserted);
                                frm.BindOutputParameters(testName, procedureName, dsOutputParameters);
                                frm.ShowDialog();
                                if (frm.didInsertFromResults)
                                {
                                    RaiseInsertFromResultsEvent();
                                }
                            //}
                            //else
                            //{
                            //    failureMsg = "No rows were inserted.";
                            //    MessageBox.Show(failureMsg);
                            //}
                        }
                    
                    }
                    else if (sp.procedureType == MIDRetail.Data.storedProcedureTypes.InsertAndReturnRID)
                    {
                        bool hasError = false;
                        string failureMsg = string.Empty;
                        DataSet dsOutputParameters;
                        int newRID = UnitTests.ExecuteInsertAndReturnRID(procedureName, dsParms, out hasError, out failureMsg, out dsOutputParameters);
                        if (hasError)
                        {
                            MessageBox.Show(failureMsg);
                        }
                        else
                        {
                            //if (rowsInserted != 0)
                            //{
                            ViewExecutionRowCountForm frm = new ViewExecutionRowCountForm();
                            frm.SetMessage(testName, procedureName, "New RID = " + newRID.ToString(), -1);
                            frm.HideButton_InsertRowCount();
                            frm.BindOutputParameters(testName, procedureName, dsOutputParameters);
                            frm.ShowDialog();
                            if (frm.didInsertFromResults)
                            {
                                RaiseInsertFromResultsEvent();
                            }
                            //}
                            //else
                            //{
                            //    failureMsg = "No rows were inserted.";
                            //    MessageBox.Show(failureMsg);
                            //}
                        }

                    }
                    else if (sp.procedureType == MIDRetail.Data.storedProcedureTypes.Update || sp.procedureType == MIDRetail.Data.storedProcedureTypes.UpdateWithReturnCode)
                    {   
                        bool hasError = false;
                        string failureMsg = string.Empty;
                        DataSet dsOutputParameters;
                        int rowsUpdated = UnitTests.ExecuteUpdate(procedureName, dsParms, out hasError, out failureMsg, out dsOutputParameters);
                        if (hasError)
                        {
                            MessageBox.Show(failureMsg);
                        }
                        else
                        {
                            //if (rowsUpdated != 0)
                            //{
                                ViewExecutionRowCountForm frm = new ViewExecutionRowCountForm();
                                frm.SetMessage(testName, procedureName, rowsUpdated.ToString() + " rows updated.", rowsUpdated);
                                frm.BindOutputParameters(testName, procedureName, dsOutputParameters);
                                frm.ShowDialog();
                                if (frm.didInsertFromResults)
                                {
                                    RaiseInsertFromResultsEvent();
                                }
                            //}
                            //else
                            //{
                            //    failureMsg = "No rows were updated.";
                            //    MessageBox.Show(failureMsg);
                            //}
                        }
                    }
                    else if (sp.procedureType == MIDRetail.Data.storedProcedureTypes.ScalarValue)
                    {
                        bool hasError = false;
                        string failureMsg = string.Empty;
                        object scalarValue = UnitTests.ExecuteReadScalar(procedureName, dsParms, out hasError, out failureMsg);
                        if (hasError)
                        {
                            MessageBox.Show(failureMsg);
                        }
                        else
                        {
                            ViewExecutionRowCountForm frm = new ViewExecutionRowCountForm();
                            if (scalarValue == null)
                            {
                                scalarValue = "null";
                            }
                            frm.SetMessage(testName, procedureName, "Scalar Value = " + scalarValue.ToString(), -1);
                            frm.HideButton_InsertRowCount();
                            frm.ShowButton_InsertCompareValue();
                            frm.SetScalarValue(scalarValue);
                            frm.HideOutputParameters();
                            frm.ShowDialog();
                            if (frm.didInsertFromResults)
                            {
                                RaiseInsertFromResultsEvent();
                            }
                        }
                    }
                    else if (sp.procedureType == MIDRetail.Data.storedProcedureTypes.Delete)
                    {
                        bool hasError = false;
                        string failureMsg = string.Empty;
                        DataSet dsOutputParameters;
                        int rowsDeleted = UnitTests.ExecuteDelete(procedureName, dsParms, out hasError, out failureMsg, out dsOutputParameters);
                        if (hasError)
                        {
                            MessageBox.Show(failureMsg);
                        }
                        else
                        {
                            //if (rowsDeleted != 0)
                            //{
                                ViewExecutionRowCountForm frm = new ViewExecutionRowCountForm();
                                frm.SetMessage(testName, procedureName, rowsDeleted.ToString() + " rows deleted.", rowsDeleted);
                                frm.BindOutputParameters(testName, procedureName, dsOutputParameters);
                                frm.ShowDialog();
                                if (frm.didInsertFromResults)
                                {
                                    RaiseInsertFromResultsEvent();
                                }
                            //}
                            //else
                            //{
                            //    failureMsg = "No rows were deleted.";
                            //    MessageBox.Show(failureMsg);
                            //}
                        }
                    }
                    else if (sp.procedureType == MIDRetail.Data.storedProcedureTypes.RecordCount)
                    {
                        bool hasError = false;
                        string failureMsg = string.Empty;
                        int rowCount = UnitTests.ExecuteReadAsCount(procedureName, dsParms, out hasError, out failureMsg);
                        if (hasError)
                        {
                            MessageBox.Show(failureMsg);
                        }
                        else
                        {
                         
                                ViewExecutionRowCountForm frm = new ViewExecutionRowCountForm();
                                frm.SetMessage(testName, procedureName, rowCount.ToString() + " rows.", rowCount);
                                frm.HideOutputParameters();
                                frm.ShowDialog();
                                if (frm.didInsertFromResults)
                                {
                                    RaiseInsertFromResultsEvent();
                                }
                           
                        }
                    }
                  
                    break;
                case "btnSave":
                    UnitTests.UpdateUnitTestParameters(testName, procedureName, this.dsParms);
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

                    SharedRoutines.GridExport.EmailSelectedRows("Unit Test Parameters", "Unit Test Parameters.xls", this.ultraGrid1);
                    break;

                case "gridEmailAllRows":
                    //UltraGridExcelExportWrapper exporter4 = new UltraGridExcelExportWrapper(this.ultraGrid1);
                    //ShowEmailForm(exporter4.ExportAllRowsToExcelAsAttachment());
                    SharedRoutines.GridExport.EmailAllRows("Unit Test Parameters", "Unit Test Parameters.xls", this.ultraGrid1);
                    break;



                #endregion

            }
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
