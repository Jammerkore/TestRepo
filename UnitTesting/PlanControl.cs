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
    public partial class PlanControl : UserControl
    {
        public PlanControl()
        {
            InitializeComponent();

        }
        private void ultraGrid1_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            //e.Layout.Bands[0].Columns["AUDIT_DATETIME"].Format = "MM/dd/yyyy HH:mm:ss";
           // e.Layout.Bands[0].Columns["environmentName"].Header.Caption = "DB";
            e.Layout.Bands[0].Columns["testName"].Header.Caption = "Test";
            e.Layout.Bands[0].Columns["procedureName"].Header.Caption = "Procedure";
            e.Layout.Bands[0].Columns["procedureType"].Header.Caption = "Type";
            e.Layout.Bands[0].Columns["executedSequence"].Header.Caption = "Executed Sequence";
            e.Layout.Bands[0].Columns["sequence"].Header.Caption = "Sequence";
            e.Layout.Bands[0].Columns["isSuspended"].Header.Caption = "Is Suspended";
            e.Layout.Bands[0].Columns["resultKind"].Header.Caption = "Result Kind";
            e.Layout.Bands[0].Columns["fieldName"].Header.Caption = "Field";
            e.Layout.Bands[0].Columns["expectedValue"].Header.Caption = "Expected Value";
            e.Layout.Bands[0].Columns["actualValue"].Header.Caption = "Actual Value";
            e.Layout.Bands[0].Columns["passFail"].Header.Caption = "Pass/Fail";
            e.Layout.Bands[0].Columns["failureMessage"].Header.Caption = "Failure Msg";

        }
        private void ultraToolbarsManager1_ToolClick(object sender, Infragistics.Win.UltraWinToolbars.ToolClickEventArgs e)
        {
            switch (e.Tool.Key)
            {
                case "btnSaveTestPlan":
                    if (AreOptionsValid())
                    {
                        SaveTestPlan(true);
                       
                    }
                   
                    break;
                case "btnLoadTestPlan":
                    LoadTestPlan();
                    break;
                case "btnExecuteTestPlan":
                    if (AreOptionsValid())
                    {
                        try
                        {
                            this.ultraToolbarsManager1.Tools["btnExecuteTestPlan"].SharedProps.Enabled = false;
                            Cursor.Current = Cursors.WaitCursor;

                            SaveTestPlan();
                            ExecuteTestPlan();
                        }
                        finally
                        {
                            this.ultraToolbarsManager1.Tools["btnExecuteTestPlan"].SharedProps.Enabled = true;
                            Cursor.Current = Cursors.Default;
                        }

                    }
                    break;
                case "btnEditTest":

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
                    string testName = (string)drFirstSelected["testName"];
                    string procedureName = (string)drFirstSelected["procedureName"];
                    this.txtProcedureName.Text = procedureName;
                    this.unitTestGridControl1.BindGrid(UnitTests.GetTestsForProcedure(procedureName));
                    this.unitTestExpectedResultControl1.BindGrid(testName, procedureName);
                }
            }
        }
        public void ExportAllRowsToFile(string fullPathWithFileName)
        {
            MIDRetail.Windows.SharedRoutines.GridExport.ExportAllRowsToExcel(this.ultraGrid1, fullPathWithFileName);
        }

        private DataSet dsResults = new DataSet();
        public void LoadTestsToRun(DataRow[] drTestsToRun, string planName)
        {
            //dsResults.Tables.Add("Results");
            //dsResults.Tables[0].Columns.Add("environmentName", typeof(string));
            //dsResults.Tables[0].Columns.Add("testName", typeof(string));
            //dsResults.Tables[0].Columns.Add("procedureName", typeof(string));

            //dsResults.Tables[0].Columns.Add("passFail", typeof(string));
            //dsResults.Tables[0].Columns.Add("failureMessage", typeof(string));
            dsResults = Shared_GenericExecution.MakeUnitTestResultDataSet();

            //foreach (DataRow dr in drTestsToRun)
            //{
            //    DataRow drResult = dsResults.Tables[0].NewRow();

            //    //drResult["environmentName"] = dr["environmentName"];
            //    drResult["testName"] = dr["testName"];
            //    drResult["procedureName"] = dr["procedureName"];


            //    dsResults.Tables[0].Rows.Add(drResult);
            //}
            foreach (DataRow drTest in drTestsToRun)
            {
                string testName = (string)drTest["testName"];
                string procedureName = (string)drTest["procedureName"];

                DataRow[] drExpected = UnitTests.dsTestExpectedResults.Tables[0].Select("testName='" + testName + "' AND procedureName='" + procedureName + "'");
                foreach (DataRow dr in drExpected)
                {
                    DataRow drResult = dsResults.Tables[0].NewRow();
                    //drResult["environmentName"] = drExpectedResult["environmentName"];
                    drResult["testName"] = testName;
                    drResult["procedureName"] = procedureName;
                    baseStoredProcedure sp = Shared_BaseStoredProcedures.GetStoredProcedure(procedureName);
                    drResult["procedureType"] = sp.procedureType;
                    if (drTest["sequence"] == DBNull.Value)
                    {
                        drResult["sequence"] = string.Empty;
                    }
                    else
                    {
                        drResult["sequence"] = (string)drTest["sequence"];
                    }
                    drResult["isSuspended"] = drTest["isSuspended"];
                    drResult["resultKind"] = dr["resultKind"];
                    drResult["fieldName"] = dr["fieldName"];
                    drResult["expectedValue"] = dr["expectedValue"];
                    drResult["actualValue"] = string.Empty;
                    drResult["passFail"] = string.Empty;
                    drResult["failureMessage"] = string.Empty;
                    dsResults.Tables[0].Rows.Add(drResult);
                }
            }

            BindGrid(dsResults);

            this.planOptions1.SetPlanName(planName);

            LoadTestPlan();
        }
        public void LoadTestsFromExistingPlan(string planName)
        {
            this.planOptions1.SetPlanName(planName);

            LoadTestPlan();


            dsResults = Shared_GenericExecution.MakeUnitTestResultDataSet();
            DataSet dsExistingTests = new DataSet();
            dsExistingTests.ReadXml(this.planOptions1.txtFolderPath.Text + "unitTests.xml");

            foreach (DataRow drTest in dsExistingTests.Tables[0].Rows)
            {
                string testName = (string)drTest["testName"];
                string procedureName = (string)drTest["procedureName"];

                DataRow[] drExpected = UnitTests.dsTestExpectedResults.Tables[0].Select("testName='" + testName + "' AND procedureName='" + procedureName + "'");
                foreach (DataRow dr in drExpected)
                {
                    DataRow drResult = dsResults.Tables[0].NewRow();
                    //drResult["environmentName"] = drExpectedResult["environmentName"];
                    drResult["testName"] = testName;
                    drResult["procedureName"] = procedureName;
                    baseStoredProcedure sp = Shared_BaseStoredProcedures.GetStoredProcedure(procedureName);
                    drResult["procedureType"] = sp.procedureType;
                    if (drTest["sequence"] == DBNull.Value)
                    {
                        drResult["sequence"] = string.Empty;
                    }
                    else
                    {
                        drResult["sequence"] = (string)drTest["sequence"];
                    }
                    drResult["isSuspended"] = drTest["isSuspended"];
                    drResult["resultKind"] = dr["resultKind"];
                    drResult["fieldName"] = dr["fieldName"];
                    drResult["expectedValue"] = dr["expectedValue"];
                    drResult["actualValue"] = string.Empty;
                    drResult["passFail"] = string.Empty;
                    drResult["failureMessage"] = string.Empty;
                    dsResults.Tables[0].Rows.Add(drResult);
                }
            }
            BindGrid(dsResults);
        }
        public void LoadTestPlan()
        {
            //Load Options if they exist
            string optionPath = this.planOptions1.txtFolderPath.Text + "PlanOptions.xml";

            if (System.IO.File.Exists(optionPath))
            {
                Plan.PlanOptions options = new Plan.PlanOptions();
                options.ReadOptionsFromFile(optionPath);

                this.planOptions1.SetPlanName(options.planName);

                //this.planOptionDB1.txtServer.Text = options.environmentServer;
                this.planOptionDB1.txtConnectionString.Text = options.environmentConnectionString;
                //this.planOptionDB1.txtDatabaseName.Text = options.environmentDatabase;
                this.planOptionDB1.txtBAKFile.Text = options.environmentBAKFilePath;
                this.planOptionDB1.cboEnvironment.Text = options.environmentName; //set environment last
                
                this.planOptions1.chkValidateStoredProcedures.Checked = options.doValidateStoredProceduresOption;

                this.planOptions1.chkCreateNewDatabase.Checked = options.doCreateNewDatabaseOption;
                this.planOptions1.chkRemoveNewDBOnFailure.Checked = options.doRemoveNewDatabaseOnFailureOption;
                this.planOptions1.chkRemoveNewDBOnSuccess.Checked = options.doRemoveNewDatabaseOnSuccessOption;

                this.planOptions1.chkUpgradeStandardDatabase.Checked = options.doUpgradeStandardDatabaseOption;
                this.planOptions1.chkUseUpgradedDatabaseForUnitTests.Checked = options.doUseUpgradedDatabaseForUnitTestsOption;
                this.planOptions1.chkRemoveUpgradeDBOnFailure.Checked = options.doRemoveUpgradeDatabaseOnFailureOption;
                this.planOptions1.chkRemoveUpgradeDBOnSuccess.Checked = options.doRemoveUpgradeDatabaseOnSuccessOption;
                
                this.planOptions1.chkRunUnitTests.Checked = options.doRunUnitTestsOption;
                this.planOptions1.chkStopOnFirstUnitTestFailure.Checked = options.doStopOnFirstUnitTestFailure;
                this.planOptions1.chkDeletePriorResultFolders.Checked = options.doDeletePriorResultFolders;     

                this.planEmailControl1.chkEmailResultsOnSuccess.Checked = options.doEmailResultsOnSuccessOption;
                this.planEmailControl1.txtSuccessTo.Text = options.emailSuccessTo;
                this.planEmailControl1.txtSuccessCc.Text = options.emailSuccessCc;

                this.planEmailControl1.chkEmailResultsOnFailure.Checked = options.doEmailResultsOnFailureOption;
                this.planEmailControl1.txtFailureTo.Text = options.emailFailureTo;
                this.planEmailControl1.txtFailureCc.Text = options.emailFailureCc;

            }
        }
        public void BindGrid(DataSet aDataSet)
        {

            ultraGrid1.DataSource = null;

            BindingSource bs = new BindingSource(aDataSet, aDataSet.Tables[0].TableName);
            this.ultraGrid1.DataSource = bs;
            this.ultraGrid1.DisplayLayout.Bands[0].PerformAutoResizeColumns(false, Infragistics.Win.UltraWinGrid.PerformAutoSizeType.AllRowsInBand);

            
           // this.ultraGrid1.Rows[0].Selected = true;

        }

        private bool AreOptionsValid()
        {
            string planFullPath = this.planOptions1.txtFolderPath.Text;
            if (planFullPath == string.Empty)
            {
                MessageBox.Show("Please provide a plan path.");
                return false;
            }
            if (planFullPath.EndsWith("\\") == false)
            {
                MessageBox.Show("Plan path must have ending backslash.");
                return false;
            }

            return true;
        }
        private void ExecuteTestPlan()
        {
            Plan.SetProgressBar(lblStatus, prgInstall);
            Plan.Run(testPlanPath + "\\" + testPlanFolder + "\\");
            ultraGrid1.DataSource = null;
            DataSet dsResultsForAllTestsInPlan = Plan.ReadAllResultsAsDataset();
            if (dsResultsForAllTestsInPlan != null)
            {
                BindGrid(dsResultsForAllTestsInPlan);
            }
            txtSummary.Text = Plan.ReadResultSummary();
        }

        private string testPlanPath;
        private string testPlanFolder;

        /// <summary>
        /// Returns true if sucessful
        /// </summary>
        /// <returns></returns>
        private void SaveTestPlan(bool showSavedMessage = false)
        {
            //string environmentName = this.ultraCombo1.Text;
            //if (UnitTests.DoesEnvironmentExist(environmentName) == false)
            //{
            //    MessageBox.Show("Please select a valid DB.");
            //    return;
            //}
            //UnitTests.defaultEnvironment = environmentName;
            //DatabaseAccess dba = new DatabaseAccess(UnitTests.GetConnectionForEnvironment(environmentName));
            //DataRow[] tests = dsResults.Tables[0].Select("environmentName='Any' OR environmentName='" + environmentName + "'");
            //UnitTests.RunUnitTests(ref dsResults, tests, dba, environmentName);





            UnitTests.GetPathAndFolderFromFullPath(this.planOptions1.txtFolderPath.Text, out testPlanPath, out testPlanFolder);

            if (System.IO.Directory.Exists(testPlanPath) == false)
            {
                MessageBox.Show("Can not access the path before the folder.");
                return;
            }
            if (System.IO.Directory.Exists(testPlanPath + "\\" + testPlanFolder) == false)
            {
                System.IO.Directory.CreateDirectory(testPlanPath + "\\" + testPlanFolder);
            }

            //copy tests
            UnitTests.SaveTestFilesForPlan(dsResults, testPlanPath + "\\" + testPlanFolder + "\\");

            //save options
            Plan.PlanOptions runOptions = new Plan.PlanOptions();

            runOptions.environmentName = this.planOptionDB1.cboEnvironment.Text;
            runOptions.environmentConnectionString = this.planOptionDB1.txtConnectionString.Text;
            runOptions.environmentServer = runOptions.GetServerNameFromConnectionString(runOptions.environmentConnectionString); //this.planOptionDB1.txtServer.Text;
            //runOptions.environmentDatabase = this.planOptionDB1.txtDatabaseName.Text;
            runOptions.environmentBAKFilePath = this.planOptionDB1.txtBAKFile.Text;

             
            runOptions.doValidateStoredProceduresOption = this.planOptions1.chkValidateStoredProcedures.Checked;
            
            runOptions.doCreateNewDatabaseOption = this.planOptions1.chkCreateNewDatabase.Checked;
            runOptions.doRemoveNewDatabaseOnFailureOption = this.planOptions1.chkRemoveNewDBOnFailure.Checked;
            runOptions.doRemoveNewDatabaseOnSuccessOption = this.planOptions1.chkRemoveNewDBOnSuccess.Checked;      

            runOptions.doUpgradeStandardDatabaseOption = this.planOptions1.chkUpgradeStandardDatabase.Checked;
            if (runOptions.doUpgradeStandardDatabaseOption)
            {
                runOptions.doUseUpgradedDatabaseForUnitTestsOption = this.planOptions1.chkUseUpgradedDatabaseForUnitTests.Checked;
            }
            else
            {
                runOptions.doUseUpgradedDatabaseForUnitTestsOption = false;
            }
            runOptions.doRemoveUpgradeDatabaseOnFailureOption = this.planOptions1.chkRemoveUpgradeDBOnFailure.Checked;
            runOptions.doRemoveUpgradeDatabaseOnSuccessOption = this.planOptions1.chkRemoveUpgradeDBOnSuccess.Checked;

            runOptions.doRunUnitTestsOption = this.planOptions1.chkRunUnitTests.Checked;
            runOptions.doStopOnFirstUnitTestFailure = this.planOptions1.chkStopOnFirstUnitTestFailure.Checked;
            runOptions.doDeletePriorResultFolders = this.planOptions1.chkDeletePriorResultFolders.Checked;

            runOptions.doEmailResultsOnSuccessOption = this.planEmailControl1.chkEmailResultsOnSuccess.Checked;
            runOptions.doEmailResultsOnFailureOption = this.planEmailControl1.chkEmailResultsOnFailure.Checked;
            runOptions.emailSuccessTo = this.planEmailControl1.txtSuccessTo.Text;
            runOptions.emailSuccessCc = this.planEmailControl1.txtSuccessCc.Text;
            runOptions.emailFailureTo = this.planEmailControl1.txtFailureTo.Text;
            runOptions.emailFailureCc = this.planEmailControl1.txtFailureCc.Text;

            runOptions.planName = this.planOptions1.txtPlan.Text;   

            runOptions.SaveOptionsToFile(testPlanPath + "\\" + testPlanFolder + "\\PlanOptions.xml");

            if (showSavedMessage)
            {
                MessageBox.Show("Test Plan " + testPlanFolder + " has been saved here: " + testPlanPath + "\\" + testPlanFolder);
            }
        }
    }
}
