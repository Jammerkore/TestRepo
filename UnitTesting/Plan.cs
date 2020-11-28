using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Net.Mail;
using System.Collections;

using MIDRetail.Data;

namespace UnitTesting
{
    static class Plan
    {
        private static string results = string.Empty;
        private static bool isSuccessful = true;
        private static string currentProcess = string.Empty;
        private static PlanOptions runOptions = new PlanOptions();
        private static string resultsFolder = string.Empty;
        private static string resultsDateTimeString = string.Empty;
        private static string planFilePath = string.Empty;
        private static string upgradedDatabaseName = string.Empty;
        private static string upgradedDatabaseConnectionString = string.Empty;
        private static string unitTestFailedResultsFileName;
        private static string unitTestAllResultsFileName;

        private static System.Windows.Forms.ToolStripStatusLabel lblStatus = null;
        private static System.Windows.Forms.ToolStripProgressBar prgInstall = null;

        /// <summary>
        /// Runs processes for unit testing
        /// </summary>
        /// <param name="planPath"></param>
        public static void Run(string planPath)
        {
            planFilePath = planPath;
            results = string.Empty;
            isSuccessful = true;

            Plan_ValidatePlanFolder();

            Plan_ReadOptions();

            Plan_DeletePriorResultsFolders();

            Plan_MakeResultsFolder();
     
            Plan_ValidateStoredProcedures();
         
            Plan_CreateNewDatabase();
           
            Plan_UpgradeStandardDatabase();
            
            Plan_RunUnitTests();

            Plan_DropDBAfterRunUnitTests();

            Plan_WriteResultSummary();

            Plan_EmailResults();
                         
        }

        /// <summary>
        /// Ensure the folder passed in actually exists
        /// </summary>
        public static void Plan_ValidatePlanFolder()
        {
            currentProcess = "Validate Plan Folder";
            if (System.IO.Directory.Exists(planFilePath) == false)
            {
                isSuccessful = false;
                Console.WriteLine("Plan folder does not exist: " + planFilePath);
            }
        }

        /// <summary>
        /// Sets the result date time string, and attempts to make the result folder for this run.
        /// </summary>
        public static void Plan_MakeResultsFolder()
        {
            if (isSuccessful)
            {
                currentProcess = "Make Results Folder";
                DateTime resultTime = DateTime.Now;
                try
                {
                    resultsDateTimeString = resultTime.Year.ToString() + "_" + resultTime.Month.ToString().PadLeft(2, '0') + "_" + resultTime.Day.ToString().PadLeft(2, '0') + "_" + resultTime.Hour.ToString().PadLeft(2, '0') + "_" + resultTime.Minute.ToString().PadLeft(2, '0') + "_" + resultTime.Second.ToString().PadLeft(2, '0');
                    resultsFolder = planFilePath + "results_" + resultsDateTimeString;
                    System.IO.Directory.CreateDirectory(resultsFolder);
                }
                catch (Exception ex)
                {
                    isSuccessful = false;
                    Console.WriteLine("Enable to create results folder under:  " + planFilePath + " Error: " + ex.ToString());
                }
            }
        }

        public static void Plan_DeletePriorResultsFolders()
        {
            if (isSuccessful && runOptions.doDeletePriorResultFolders)
            {
                currentProcess = "Delete Prior Result Folders";
                try
                {
                    System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(planFilePath);
                    foreach(System.IO.DirectoryInfo dinfo in di.GetDirectories("results_*"))
                    {
                        dinfo.Delete(true);
                    }
                }
                catch (Exception ex)
                {
                    isSuccessful = false;
                    Console.WriteLine("Enable to create results folder under:  " + planFilePath + " Error: " + ex.ToString());
                }
            }
            else
            {
                AddSkippedProcessToResults();
            }
        }

        //public static void Plan_GlobalErrorHandling_Add()
        //{
        //    // Add the event handler for handling non-UI thread exceptions
        //    AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(HandleExceptionsNonUI);
        //    // Add the event handler for handling UI thread exceptions to the event.
        //    System.Windows.Forms.Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(HandleExceptionsUI); 
        //}
        //public static void Plan_GlobalErrorHandling_Remove()
        //{
        //    //Remove the event handler for handling non-UI thread exceptions 
        //    AppDomain.CurrentDomain.UnhandledException -= new UnhandledExceptionEventHandler(HandleExceptionsNonUI);
        //}
        //public static void HandleExceptionsNonUI(object sender, UnhandledExceptionEventArgs e)
        //{
        //    Exception ex = (Exception)e.ExceptionObject;
        //    isSuccessful = false;
        //    AddFailureMessageToResults(ex.ToString());
        //}

        //public static void HandleExceptionsUI(object sender, System.Threading.ThreadExceptionEventArgs t)
        //{
        //    isSuccessful = false;
        //    AddFailureMessageToResults(t.Exception.ToString());
        //}

        private static void AddSkippedProcessToResults()
        {
            results += currentProcess + ": Skipped " + System.Environment.NewLine;
        }
        private static void AddFailureMessageToResults(string failureMsg)
        {
            results += currentProcess + ": Failed " + failureMsg + System.Environment.NewLine;
        }
        private static void AddSuccessMessageToResults(string successMsg = "")
        {
            if (isSuccessful)
            {
                results += currentProcess + ": Success " + successMsg + System.Environment.NewLine;
            }
        }
        private static void AddMessageToResults(string msg)
        {
            results += currentProcess + ": " + msg + System.Environment.NewLine;
            if (lblStatus != null)
            {
                lblStatus.Text = msg;
                System.Windows.Forms.Application.DoEvents();
            }
        }

        /// <summary>
        /// Ensures the option file exists, and if so, reads the plan options
        /// </summary>
        private static void Plan_ReadOptions()
        {
            if (isSuccessful)
            {
                currentProcess = "Read Options";
                string optionPath = planFilePath + "\\PlanOptions.xml";

                if (System.IO.File.Exists(optionPath) == false)
                {
                    isSuccessful = false;
                    AddFailureMessageToResults("Option file does not exist: " + optionPath);
                }
                else
                {
                    runOptions.ReadOptionsFromFile(optionPath);
                    AddSuccessMessageToResults();
                }
            }
        }

        /// <summary>
        /// Runs a series of validation checks against stored procedures in the project
        /// </summary>
        private static void Plan_ValidateStoredProcedures()
        {
            currentProcess = "Validate Stored Procedures";
            if (isSuccessful && runOptions.doValidateStoredProceduresOption)
            {
                Shared_BaseStoredProcedures.PopulateStoredProcedureListFromAssembly();

                DataSet dsInvalid = new DataSet();
                dsInvalid.Tables.Add("Invalid Procedures");
                dsInvalid.Tables[0].Columns.Add("procedureName");
                dsInvalid.Tables[0].Columns.Add("validation");
                foreach (baseStoredProcedure bp in Shared_BaseStoredProcedures.storedProcedureList)
                {
                    bool hasNoLock = false;
                    bool hasRowLock = false;
                    bool hasOrderBy = false;
                    string validationMsg = Shared_ValidateStoredProcedures.ValidateStoredProcedure(bp.procedureName, out hasNoLock, out hasRowLock, out hasOrderBy);
                    if (validationMsg != "OK")
                    {
                        DataRow drInvalid = dsInvalid.Tables[0].NewRow();
                        drInvalid["procedureName"] = bp.procedureName;
                        drInvalid["validation"] = validationMsg;
                        dsInvalid.Tables[0].Rows.Add(drInvalid);
                    }
                }

                if (dsInvalid.Tables[0].Rows.Count > 0)
                {
                    isSuccessful = false;
                    string failureMsg = dsInvalid.Tables[0].Rows.Count.ToString() + " stored procedures are invalid:" + System.Environment.NewLine;
                    foreach (DataRow dr in dsInvalid.Tables[0].Rows)
                    {
                        string procedureName = (string)dr["procedureName"];
                        string validationMsg = (string)dr["validation"];
                        failureMsg += procedureName + ":" + validationMsg;
                    }
                    AddFailureMessageToResults(failureMsg);
                }
                else
                {
                    ReportViewerControl rc = new ReportViewerControl();
                    //now validate there are no extra non-defined stored procedures in the project
                    rc.MakeProceduresNotReferencedDataSet();
                    foreach (DataRow dr in rc.dsReportData.Tables[0].Rows)
                    {
                            isSuccessful = false;
                            string fileName = (string)dr["File"];
                            string procedureName = (string)dr["Procedure"];
                            string failureMsg = procedureName + " in file " + fileName + " is not referenced in the project." + System.Environment.NewLine;
                            AddFailureMessageToResults(failureMsg);
                        
                    }

                    //assert that all sql objects are created under the dbo schema
                    rc.MakeObjectDBOCheckDataSet();
                    foreach (DataRow dr in rc.dsReportData.Tables[0].Rows)
                    {
                        if ((string)dr["Has DBO"] == "N")
                        {
                            isSuccessful = false;
                            string objectName = (string)dr["Name"];
                            string objectType = (string)dr["Type"];
                            string failureMsg = objectType + " " + objectName + " was not created under the dbo schema." + System.Environment.NewLine;
                            AddFailureMessageToResults(failureMsg);
                        }
                    }
                }
                AddSuccessMessageToResults();
            }
            else
            {
                AddSkippedProcessToResults();
            }
        }

        public static void SetProgressBar(System.Windows.Forms.ToolStripStatusLabel alblStatus, System.Windows.Forms.ToolStripProgressBar aprgInstall)
        {
            lblStatus = alblStatus;
            prgInstall = aprgInstall;
        }

        /// <summary>
        /// Builds a new database from scratch to ensure the database creation process still works
        /// </summary>
        private static void Plan_CreateNewDatabase()
        {
            currentProcess = "Create New Database";
            if (isSuccessful && runOptions.doCreateNewDatabaseOption)
            {
                //Create new database from scratch
                string newDBName;
                string connection = CreateDatabase(out newDBName);
                if (isSuccessful)
                {
                    AddMessageToResults("Database created: " + newDBName);
                    Queue messageQueue = new Queue();
                    Queue processedQueue = new Queue();
                    MIDRetail.DatabaseUpdate.UpdateRoutines.SetProgressBar(lblStatus, prgInstall);
                    bool isValidConfiguration;
                    MIDRetail.DatabaseUpdate.UpdateRoutines.ProcessDatabase(messageQueue, processedQueue, true, false, false, false, false, connection, newDBName, new MIDRetail.DatabaseUpdate.UpdateRoutines.SetMessageDelegate(AddMessageToResults), new MIDRetail.DatabaseUpdate.UpdateRoutines.SetMessageToInstallerLog(SetLogMessage), out isSuccessful, out isValidConfiguration);
                    while (messageQueue.Count > 0)
                    {
                        AddMessageToResults(messageQueue.Dequeue().ToString());
                    }
                }
                if (isSuccessful)
                {
                    if (runOptions.doRemoveNewDatabaseOnSuccessOption)
                    {
                        //Remove the newly created database
                        DropDatabase(newDBName);
                    }
                }
                else
                {
                    if (runOptions.doRemoveNewDatabaseOnFailureOption)
                    {
                        //Remove the newly created database
                        DropDatabase(newDBName);
                    }
                }
                if (isSuccessful)
                {
                    AddSuccessMessageToResults();
                }
                else
                {
                    AddFailureMessageToResults("Database creation failed.");
                }
             
            }
            else
            {
                AddSkippedProcessToResults();
            }
        }

        private static void SetLogMessage(string message)
        {
        }

        private static string newDBNameForUpgrade;
        /// <summary>
        /// Creates a copy of the specified database, then attempts an upgrade to the latest version
        /// </summary>
        private static void Plan_UpgradeStandardDatabase()
        {
            currentProcess = "Upgrade Standard Database";
            if (isSuccessful && runOptions.doUpgradeStandardDatabaseOption)
            {
                prgInstall.Value = 0;
                upgradedDatabaseConnectionString = CreateDatabaseFromBackupFileForUpgrade(out newDBNameForUpgrade);  //restore database from bak file
                if (isSuccessful)   //upgrade the database created from the bak file
                {
                    AddMessageToResults("Database to upgrade: " + newDBNameForUpgrade);
                    Queue messageQueue = new Queue();
                    Queue processedQueue = new Queue();
                    MIDRetail.DatabaseUpdate.UpdateRoutines.SetProgressBar(lblStatus, prgInstall);
                    bool isValidConfiguration;
                    MIDRetail.DatabaseUpdate.UpdateRoutines.ProcessDatabase(messageQueue, processedQueue, false, true, false, false, false, upgradedDatabaseConnectionString, newDBNameForUpgrade, new MIDRetail.DatabaseUpdate.UpdateRoutines.SetMessageDelegate(AddMessageToResults), new MIDRetail.DatabaseUpdate.UpdateRoutines.SetMessageToInstallerLog(SetLogMessage), out isSuccessful, out isValidConfiguration);
                    while (messageQueue.Count > 0)
                    {
                        AddMessageToResults(messageQueue.Dequeue().ToString());
                    }
                }
                if (runOptions.doUseUpgradedDatabaseForUnitTestsOption == false)
                {  
                    if (isSuccessful)
                    {
                        if (runOptions.doRemoveUpgradeDatabaseOnSuccessOption)
                        {
                            //Remove the database created from the bak file
                            DropDatabase(newDBNameForUpgrade);
                        }
                    }
                    else
                    {
                        if (runOptions.doRemoveUpgradeDatabaseOnFailureOption)
                        {
                            //Remove the database created from the bak file
                            DropDatabase(newDBNameForUpgrade);
                        }
                    }
                }
                if (isSuccessful)
                {
                    AddSuccessMessageToResults();
                }
                else
                {
                    AddFailureMessageToResults("Upgrade Standard Database failed.");
                }
            }
            else
            {
                AddSkippedProcessToResults();
            }
        }


        private static DataSet GetParametersForUnitTestForPlan(DataSet dsAllParameters, string testName, string procedureName)
        {
            DataSet dsParms = dsAllParameters.Clone();


            DataRow[] drParms = dsAllParameters.Tables[0].Select("testName='" + testName + "' AND procedureName='" + procedureName + "'");
            foreach (DataRow dr in drParms)
            {
                DataRow drParam = dsParms.Tables[0].NewRow();
                Shared_UtilityFunctions.DataRowCopy(dr, drParam);
                dsParms.Tables[0].Rows.Add(drParam);
            }
            return dsParms;
        }

        private static string GetSequenceFromTest(string testName, string procedureName)
        {
            DataRow[] drTest = dsTestsForPlan.Tables[0].Select("testName='" + testName + "' AND procedureName='" + procedureName + "'");
            if (drTest.Length > 0 && dsTestsForPlan.Tables[0].Columns.Contains("sequence"))
            {
                if (drTest[0]["sequence"] == DBNull.Value)
                {
                    return string.Empty;
                }
                else
                {
                    return (string)drTest[0]["sequence"];
                }
            }
            else
            {
                return string.Empty;
            }
        }
        private static string GetIsSuspendedFromTest(string testName, string procedureName)
        {
            DataRow[] drTest = dsTestsForPlan.Tables[0].Select("testName='" + testName + "' AND procedureName='" + procedureName + "'");
            if (drTest.Length > 0 && dsTestsForPlan.Tables[0].Columns.Contains("isSuspended"))
            {
                if (drTest[0]["isSuspended"] == DBNull.Value)
                {
                    return string.Empty;
                }
                else
                {
                    return (string)drTest[0]["isSuspended"];
                }
            }
            else
            {
                return string.Empty;
            }
        }


        private static void Plan_DropDBAfterRunUnitTests()
        {
            try
            {
                currentProcess = "Drop Upgraded Database After Running Unit Tests";
                if (runOptions.doUseUpgradedDatabaseForUnitTestsOption == true)
                {
                    if (isSuccessful)
                    {
                        if (runOptions.doRemoveUpgradeDatabaseOnSuccessOption)
                        {
                            //Remove the database created from the bak file
                            DropDatabase(newDBNameForUpgrade);
                        }
                    }
                    else
                    {
                        if (runOptions.doRemoveUpgradeDatabaseOnFailureOption)
                        {
                            //Remove the database created from the bak file
                            DropDatabase(newDBNameForUpgrade);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                isSuccessful = false;
                AddFailureMessageToResults(ex.ToString());
            }
        }


        private static DataSet dsResultsForAllTests = null;
        private static DataSet dsSortedResultsForAllTests;
        private static DataSet dsTestsForPlan;
        private static DataSet dsParametersForPlan;
        private static bool haltUnitTestExecution;

        /// <summary>
        /// Runs unit tests in the folder
        /// </summary>
        private static void Plan_RunUnitTests()
        {
            currentProcess = "Run Unit Tests";
            if (isSuccessful && runOptions.doRunUnitTestsOption)
            {
                executedSeq = 1;

                string connectionString;
                if (runOptions.doUseUpgradedDatabaseForUnitTestsOption)
                {
                    connectionString = upgradedDatabaseConnectionString;
                }
                else
                {
                    //use the environment connection string
                    connectionString = runOptions.environmentConnectionString;
                }

                AddMessageToResults("Database for running tests: " + connectionString);
        


                DatabaseAccess dba = new DatabaseAccess(connectionString);

                dsTestsForPlan = new DataSet();
                dsParametersForPlan = new DataSet();
                DataSet dsExpectedResultsForPlan = new DataSet();

                dsTestsForPlan.ReadXml(planFilePath + "unitTests.xml");
                dsParametersForPlan.ReadXml(planFilePath + "unitTestParameters.xml");
                dsExpectedResultsForPlan.ReadXml(planFilePath + "unitTestExpectedResults.xml");

                Shared_BaseStoredProcedures.PopulateStoredProcedureListFromAssembly();

                //Populate a result dataset
                dsResultsForAllTests = Shared_GenericExecution.MakeUnitTestResultDataSet();
            
                foreach (DataRow drExpectedResult in dsExpectedResultsForPlan.Tables[0].Rows)
                {
                    DataRow drResult = dsResultsForAllTests.Tables[0].NewRow();
                    //drResult["environmentName"] = drExpectedResult["environmentName"];

                    string testName = (string)drExpectedResult["testName"];
                    string procedureName = (string)drExpectedResult["procedureName"];

                    drResult["testName"] = testName;
                    drResult["procedureName"] = procedureName;
                    baseStoredProcedure sp = Shared_BaseStoredProcedures.GetStoredProcedure(procedureName);
                    drResult["procedureType"] = sp.procedureType;
                    drResult["sequence"] = GetSequenceFromTest(testName, procedureName);
                    drResult["isSuspended"] = GetIsSuspendedFromTest(testName, procedureName);
                    drResult["resultKind"] = drExpectedResult["resultKind"];
                    drResult["fieldName"] = drExpectedResult["fieldName"];
                    drResult["expectedValue"] = drExpectedResult["expectedValue"];
                    drResult["actualValue"] = string.Empty;
                    drResult["passFail"] = string.Empty;
                    drResult["failureMessage"] = string.Empty;
                    drResult["executedSequence"] = string.Empty;
                    dsResultsForAllTests.Tables[0].Rows.Add(drResult);
                }

                prgInstall.Value = 0;
                prgInstall.Maximum = dsResultsForAllTests.Tables[0].Rows.Count;

                haltUnitTestExecution = false;

                //Execute all the read tests first
                ExecuteUnitTestSet(connectionString, dba, new List<storedProcedureTypes>() {storedProcedureTypes.Read, storedProcedureTypes.ReadAsDataset});
                ExecuteUnitTestSet(connectionString, dba, new List<storedProcedureTypes>() {storedProcedureTypes.RecordCount, storedProcedureTypes.ScalarValue, storedProcedureTypes.OutputOnly });
                ExecuteUnitTestSet(connectionString, dba, new List<storedProcedureTypes>() {storedProcedureTypes.InsertAndReturnRID});
                ExecuteUnitTestSet(connectionString, dba, new List<storedProcedureTypes>() {storedProcedureTypes.Insert});
                ExecuteUnitTestSet(connectionString, dba, new List<storedProcedureTypes>() { storedProcedureTypes.Update, storedProcedureTypes.UpdateWithReturnCode });
                ExecuteUnitTestSet(connectionString, dba, new List<storedProcedureTypes>() {storedProcedureTypes.Delete });

                dsSortedResultsForAllTests = dsResultsForAllTests.Clone();
                DataRow[] drSortedAllTests = dsResultsForAllTests.Tables[0].Select("", "executedSequence");
                foreach (DataRow dr in drSortedAllTests)
                {
                    DataRow drSorted = dsSortedResultsForAllTests.Tables[0].NewRow();
                    Shared_UtilityFunctions.DataRowCopy(dr, drSorted);
                    dsSortedResultsForAllTests.Tables[0].Rows.Add(drSorted);
                }
                //dsSortedResultsForAllTests.Tables[0].Columns.Remove("executedSequence");


                //Make grid and bind results
                PlanControl ucResults = new PlanControl();
                ucResults.BindGrid(dsSortedResultsForAllTests);

                //Export results
                unitTestAllResultsFileName = "unit_test_all_results_for_" + runOptions.environmentName + "_" + resultsDateTimeString + ".xls";
                ucResults.ExportAllRowsToFile(resultsFolder + "\\" + unitTestAllResultsFileName);

                //Make dataset for just failed tests
                DataSet dsFailedTests = Shared_GenericExecution.MakeUnitTestResultDataSet();
                DataRow[] drFailedTests = dsSortedResultsForAllTests.Tables[0].Select("passFail='Fail'");
                DataRow[] drPassedTests = dsSortedResultsForAllTests.Tables[0].Select("passFail='Pass'");

                AddMessageToResults("Tests Passed: " + drPassedTests.Length.ToString() + "  Tests Failed: " + drFailedTests.Length.ToString());
                prgInstall.Value = prgInstall.Maximum;

                if (drFailedTests.Length > 0)
                {
                    foreach(DataRow dr in drFailedTests)
                    {
                        DataRow drFailed = dsFailedTests.Tables[0].NewRow();
                        Shared_UtilityFunctions.DataRowCopy(dr, drFailed);
                        dsFailedTests.Tables[0].Rows.Add(drFailed);
                    }

                    //Make grid and bind results
                    PlanControl ucFailedResults = new PlanControl();
                    ucFailedResults.BindGrid(dsFailedTests);

                    //Export results
                    unitTestFailedResultsFileName = "unit_test_failed_results_for_" + runOptions.environmentName + "_" + resultsDateTimeString + ".xls";
                    ucFailedResults.ExportAllRowsToFile(resultsFolder + "\\" + unitTestFailedResultsFileName);

                    AddFailureMessageToResults("Not all unit tests passed.");
                    isSuccessful = false;
                }
                else
                {
                    AddSuccessMessageToResults();
                }
            }
            else
            {
                AddSkippedProcessToResults();
            }
        }

        private static bool IsBaseProcedureInAllowedTypes(baseStoredProcedure sp, List<storedProcedureTypes> procedureTypesToExecute)
        {
    
            bool isInList = false;
            foreach (storedProcedureTypes procedureType in procedureTypesToExecute)
            {
                if (sp.procedureType == procedureType)
                {
                    isInList = true;
                }
            }

            return isInList;
        }

        private static int executedSeq;
        private static void SetExecutedSequenceOnResult(DataRow drResult)
        {
            drResult["executedSequence"] = executedSeq.ToString().PadLeft(5, '0');
        }
        private static void ExecuteUnitTestSet(string connectionString, DatabaseAccess dba, List<storedProcedureTypes> procedureTypesToExecute)
        {
            try
            {
                DataRow[] drSorted = dsTestsForPlan.Tables[0].Select("", "sequence");
                foreach (DataRow drTest in drSorted)
                {
                    string testName = (string)drTest["testName"];
                    string procedureName = (string)drTest["procedureName"];


                    baseStoredProcedure sp = Shared_BaseStoredProcedures.GetStoredProcedure(procedureName);
                    if (sp != null && IsBaseProcedureInAllowedTypes(sp, procedureTypesToExecute))
                    {
                       
                        string isSuspended = "N";
                        if (drTest["isSuspended"] != DBNull.Value)
                        {
                            isSuspended = (string)drTest["isSuspended"];
                        }
                        if (isSuspended == "Y")
                        {
                            AddMessageToResults("Running test: (Suspended) " + testName);
                            prgInstall.Value += 1;
                            SetResultsToPassForTest(ref dsResultsForAllTests, testName, procedureName);
                        }
                        else if (haltUnitTestExecution)
                        {
                            AddMessageToResults("Running test: (Skipped - Stop on First Failure) " + testName);
                            prgInstall.Value += 1;
                            SetResultsToPassForTest(ref dsResultsForAllTests, testName, procedureName);
                        }
                        else
                        {
                            AddMessageToResults("Running test: " + testName);
                            prgInstall.Value += 1;

                            DataSet dsOutputParameters = null;
                            Shared_SetParameter.SetParametersForProcedure(sp, GetParametersForUnitTestForPlan(dsParametersForPlan, testName, procedureName), connectionString);

                            DataTable dtActualResults = null;
                            int actualRowCount = -1;
                            string failureMessage = string.Empty;
                            object scalarValue = null;
                            GetActualResults(sp, dba, ref actualRowCount, ref dtActualResults, ref failureMessage, ref dsOutputParameters, ref scalarValue);
                            if (failureMessage == string.Empty)
                            {
                                CompareActualResultsToExpectedResults(ref dsResultsForAllTests, testName, procedureName, actualRowCount, dtActualResults, dsOutputParameters, scalarValue);
                            }
                            else
                            {
                                CheckForStopOnFirstFailure();
                                SetResultsToFailForTest(ref dsResultsForAllTests, testName, procedureName, failureMessage);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                isSuccessful = false;
                AddFailureMessageToResults(ex.ToString());
            }
        }

        private static void GetActualResults(baseStoredProcedure sp, DatabaseAccess dba, ref int actualRowCount, ref DataTable dtActualResults, ref string failureMessage, ref DataSet dsOutputParameters, ref object scalarValue)
        {
            try
            {
                bool hasError = false;
                int rowsInserted = -1;
                int rowsUpdated = -1;
                int rowsDeleted = -1;
                int rowCount = -1;
                actualRowCount = -1;
                dtActualResults = null;
                scalarValue = null;
                if (sp.procedureType == storedProcedureTypes.Read || sp.procedureType == storedProcedureTypes.ReadAsDataset)
                {
                    Shared_GenericExecution.DoRead(sp, dba, out hasError, out failureMessage, out dtActualResults, out dsOutputParameters);
                }
                if (sp.procedureType == storedProcedureTypes.RecordCount)
                {
                    Shared_GenericExecution.DoReadAsRecordCount(sp, dba, out hasError, out failureMessage, out rowCount);
                }
                if (sp.procedureType == storedProcedureTypes.ScalarValue)
                {
                    Shared_GenericExecution.DoReadScalar(sp, dba, out hasError, out failureMessage, out scalarValue);
                }
                if (sp.procedureType == storedProcedureTypes.Insert)
                {
                    Shared_GenericExecution.DoInsert(sp, dba, out hasError, out failureMessage, out rowsInserted, out dsOutputParameters);
                }
                if (sp.procedureType == storedProcedureTypes.InsertAndReturnRID) //uses outputParameters to actual results to expected results
                {
                    int newRID;
                    Shared_GenericExecution.DoInsertAndReturnRID(sp, dba, out hasError, out failureMessage, out newRID, out dsOutputParameters);
                }
                if (sp.procedureType == storedProcedureTypes.Update || sp.procedureType == storedProcedureTypes.UpdateWithReturnCode)
                {
                    Shared_GenericExecution.DoUpdate(sp, dba, out hasError, out failureMessage, out rowsUpdated, out dsOutputParameters);
                }
                if (sp.procedureType == storedProcedureTypes.Delete)
                {
                    Shared_GenericExecution.DoDelete(sp, dba, out hasError, out failureMessage, out rowsDeleted, out dsOutputParameters);
                }

                if (!hasError)
                {
                    if (sp.procedureType == storedProcedureTypes.Read)
                    {
                        if (dtActualResults != null)
                        {
                            actualRowCount = dtActualResults.Rows.Count;
                        }
                    }
                    if (sp.procedureType == storedProcedureTypes.RecordCount)
                    {
                        actualRowCount = rowCount;
                    }
                    if (sp.procedureType == storedProcedureTypes.Insert)
                    {
                        actualRowCount = rowsInserted;
                    }
                    if (sp.procedureType == storedProcedureTypes.Update)
                    {
                        actualRowCount = rowsUpdated;
                    }
                    if (sp.procedureType == storedProcedureTypes.Delete)
                    {
                        actualRowCount = rowsDeleted;
                    }
                }
            }
            catch (Exception ex)
            {
                failureMessage = ex.ToString();
            }
        }

       
        private static void CompareActualResultsToExpectedResults(ref DataSet dsAllResults, string testName, string procedureName, int actualRowCount, DataTable dtActualResults, DataSet dsOutputParameters, object scalarValue)
        {
            //use actualRowCount if not equal to negative one.
            if (actualRowCount == -1)
            {
                //use the row count of dtActual Results
                if (dtActualResults != null)
                {
                    actualRowCount = dtActualResults.Rows.Count;
                }
            }

         
            DataRow[] drResultsForThisTest = dsAllResults.Tables[0].Select("testName='" + testName + "' AND procedureName='" + procedureName + "'");
            //testPassed = true;
            foreach (DataRow drResult in drResultsForThisTest)
            {
                SetExecutedSequenceOnResult(drResult);
                string resultKind = (string)drResult["resultKind"];
                if (resultKind == UnitTests.ExpectedResultKinds.RowCountEqualsX.Name || resultKind == UnitTests.ExpectedResultKinds.RowCountEqualsOne.Name)
                {
                    drResult["actualValue"] = actualRowCount.ToString();

                    int expectedCount;
                    int.TryParse((string)drResult["expectedValue"], out expectedCount);


                    if (expectedCount != actualRowCount)
                    {
                        CheckForStopOnFirstFailure();
                        drResult["passFail"] = "Fail";
                        drResult["failureMessage"] = "Row Count does not equal " + expectedCount.ToString() + ".";
                    }
                    else
                    {
                        drResult["passFail"] = "Pass";
                        drResult["failureMessage"] = string.Empty;
                    }
                }
             
                else if (resultKind == UnitTests.ExpectedResultKinds.RowCountGreaterThanZero.Name)
                {
                    drResult["actualValue"] = actualRowCount.ToString();
                    if (actualRowCount <= 0)
                    {
                        CheckForStopOnFirstFailure();
                        drResult["passFail"] = "Fail";
                        drResult["failureMessage"] = "Row Count is not greater than zero.";
                    }
                    else
                    {
                        drResult["passFail"] = "Pass";
                        drResult["failureMessage"] = string.Empty;
                    }
                }
                else if (resultKind == UnitTests.ExpectedResultKinds.CompareScalarValue.Name)
                {
                    string expectedResult = (string)drResult["expectedValue"];

                    string actualResult = string.Empty;
                    if (scalarValue != null)
                    {
                        actualResult = scalarValue.ToString();
                    }
                    drResult["actualValue"] = actualResult;
                    if (actualResult != expectedResult)
                    {
                        CheckForStopOnFirstFailure();
                        drResult["passFail"] = "Fail";
                        drResult["failureMessage"] = "Actual result: " + actualResult + " does not does not equal expected result: " + expectedResult;
                    }
                    else
                    {
                        drResult["passFail"] = "Pass";
                        drResult["failureMessage"] = string.Empty;
                    }
                }
                else if (resultKind == UnitTests.ExpectedResultKinds.OutputParameterEquals.Name)
                {
                    //Ensure there is output parameters
                    if (dsOutputParameters == null || dsOutputParameters.Tables.Count == 0 || dsOutputParameters.Tables[0].Rows.Count == 0)
                    {
                        CheckForStopOnFirstFailure();
                        drResult["passFail"] = "Fail";
                        drResult["failureMessage"] = "No output parameters returned. Cannot evaluate output parameters.";
                    }
                    else
                    {
                        string expectedParameterName = (string)drResult["fieldName"];
                        string expectedResult = (string)drResult["expectedValue"];
                        bool foundParameter = false;
                        foreach (DataRow dr in dsOutputParameters.Tables[0].Rows)
                        {
                            string outputParameterName = (string)dr["parameterName"];
                            if (expectedParameterName == outputParameterName)
                            {
                                foundParameter = true;

                                string actualResult = (string)dr["parameterValue"];
                                if (actualResult.ToString() != expectedResult)
                                {
                                    CheckForStopOnFirstFailure();
                                    drResult["passFail"] = "Fail";
                                    drResult["failureMessage"] = "Output Parameter: " + outputParameterName + " Actual result: " + actualResult.ToString() + " does not does not equal expected result: " + expectedResult;
                                }
                                else
                                {
                                    drResult["passFail"] = "Pass";
                                    drResult["failureMessage"] = string.Empty;
                                }
                            }
                        }
                        if (foundParameter == false)
                        {
                            CheckForStopOnFirstFailure();
                            drResult["passFail"] = "Fail";
                            drResult["failureMessage"] = "Output Parameter " + expectedParameterName + " does not exist in the result set.";
                        }
                    }
                }
                else if (resultKind == UnitTests.ExpectedResultKinds.FieldEquals.Name)
                {
                    //Ensure there is at least one row
                    if (dtActualResults.Rows.Count == 0)
                    {
                        CheckForStopOnFirstFailure();
                        drResult["passFail"] = "Fail";
                        drResult["failureMessage"] = "Row Count is zero. Cannot evaluate fields.";
                    }
                    else
                    {
                        //Ensure the field exists in the first row
                        string fieldName = (string)drResult["fieldName"];
                        if (dtActualResults.Columns.Contains(fieldName) == false)
                        {
                            CheckForStopOnFirstFailure();
                            drResult["passFail"] = "Fail";
                            drResult["failureMessage"] = "Field " + fieldName + " does not exist in the result set.";
                        }
                        else
                        {
                            //Ensure the field value matches the expected value
                            object actualResult = dtActualResults.Rows[0][fieldName];
                            object expectedResult = drResult["expectedValue"];
                            if (actualResult.ToString() != expectedResult.ToString())
                            {
                                CheckForStopOnFirstFailure();
                                drResult["passFail"] = "Fail";
                                drResult["failureMessage"] = "Field: " + fieldName + " Actual Result " + actualResult.ToString() + " does not does not equal expected result: " + expectedResult.ToString();
                            }
                            else
                            {
                                drResult["passFail"] = "Pass";
                                drResult["failureMessage"] = string.Empty;
                            }
                        }
                    }
                }
                else
                {
                    CheckForStopOnFirstFailure();
                    drResult["passFail"] = "Fail";
                    drResult["failureMessage"] = "Unknown Result Kind:" + resultKind;
                }
               
                
            } //end foreach actual result
            executedSeq++;
            //if there are no ExpectedResults defined - fail the test
            //if (drResultsForThisTest.Length == 0)
            //{
            //    testPassed = false;
            //    //failureMessage = "No expected results were defined for this test.";

            //}
           
        }
        private static void CheckForStopOnFirstFailure()
        {
            if (runOptions.doStopOnFirstUnitTestFailure)
            {
                haltUnitTestExecution = true;
            }
        }

        private static void SetResultsToFailForTest(ref DataSet dsAllResults, string testName, string procedureName, string failureMessage)
        {
            DataRow[] drResultsForThisTest = dsAllResults.Tables[0].Select("testName='" + testName + "' AND procedureName='" + procedureName + "'");
        
            foreach (DataRow drResult in drResultsForThisTest)
            {
                drResult["passFail"] = "Fail";
                drResult["failureMessage"] = failureMessage;
                SetExecutedSequenceOnResult(drResult);
            }
            executedSeq++;
        }
        private static void SetResultsToPassForTest(ref DataSet dsAllResults, string testName, string procedureName)
        {
            DataRow[] drResultsForThisTest = dsAllResults.Tables[0].Select("testName='" + testName + "' AND procedureName='" + procedureName + "'");

            foreach (DataRow drResult in drResultsForThisTest)
            {
                drResult["passFail"] = "Pass";
                drResult["failureMessage"] = string.Empty;
                SetExecutedSequenceOnResult(drResult);
            }
            executedSeq++;
        }

        private static void GetActualResultsForUpdate(baseStoredProcedure sp, DatabaseAccess dba, ref int actualRowCount, ref DataTable dtActualResults, ref string failureMessage, ref DataSet dsOutputParameters)
        {
            try
            {
                bool hasError;
                int rowsUpdated;

                Shared_GenericExecution.DoUpdate(sp, dba, out hasError, out failureMessage, out rowsUpdated, out dsOutputParameters);
                if (!hasError)
                {
                    actualRowCount = rowsUpdated;
                }
            }
            catch (Exception ex)
            {
                failureMessage = ex.ToString();
            }
        }

        
        public static DataSet ReadAllResultsAsDataset()
        {
            return dsSortedResultsForAllTests;
        }
        public static string ReadResultSummary()
        {
            return results;
        }

  

        /// <summary>
        /// Writes the result summary in a text file
        /// </summary>
        /// <param name="resultsFolder"></param>
        private static void Plan_WriteResultSummary()
        {
            try
            {
                currentProcess = "Write Result Summary";
                if (isSuccessful)
                {
                    results += "Plan Overall Result: Success" + System.Environment.NewLine;
                }
                else
                {
                    results += "Plan Overall Result: Failure" + System.Environment.NewLine;
                }

                System.IO.StreamWriter sw = new System.IO.StreamWriter(resultsFolder + @"\resultSummary.txt");
                sw.Write(results);
                sw.Flush();
                sw.Close();
            }
            catch (Exception ex)
            {
                isSuccessful = false;
                AddFailureMessageToResults(ex.ToString());
            }
        }

        /// <summary>
        /// Emails results
        /// </summary>
        private static void Plan_EmailResults()
        {
            try
            {
                currentProcess = "Email Results";
                if (isSuccessful && runOptions.doEmailResultsOnSuccessOption)
                {
                    SendEmailMessage(runOptions.emailSuccessTo, runOptions.emailSuccessCc, true, false);

                }
                else if (isSuccessful == false && runOptions.doEmailResultsOnFailureOption)
                {
                    SendEmailMessage(runOptions.emailFailureTo, runOptions.emailFailureCc, true, true);
                }
            }
            catch (Exception ex)
            {
                isSuccessful = false;
                AddFailureMessageToResults(ex.ToString());
            }
        }




        /// <summary>
        /// Creates an email message for this run and then emails the message
        /// </summary>
        /// <param name="emailFrom"></param>
        /// <param name="emailTo"></param>
        /// <param name="emailCC"></param>
        /// <param name="emailSubject"></param>
        /// <param name="emailBody"></param>
        /// <returns></returns>
        private static void SendEmailMessage(string emailTo, string emailCC, bool attachAllResults, bool attachFailedResults)
        {
            try
            {
            MailMessage email = new MailMessage();
            email.IsBodyHtml = true;


            //use default system error message From
            email.From = new MailAddress("app@midretail.com");

          
            //email.To.Add(emailTo);
            if (emailTo != null && emailTo != String.Empty)
            {
                string[] to_addresses = emailTo.Split(';');
                foreach (string emailAddress in to_addresses)
                {
                    if (emailAddress != string.Empty)
                    {
                        if (emailAddress.Contains("@"))
                        {
                            email.To.Add(emailAddress);
                        }
                    }
                }
            }

            if (emailCC != null && emailCC != String.Empty)
            {
                string[] cc_addresses = emailCC.Split(';');
                foreach (string emailAddress in cc_addresses)
                {
                    if (emailAddress != string.Empty)
                    {
                        if (emailAddress.Contains("@"))
                        {
                            email.CC.Add(emailAddress);
                        }
                    }
                }
            }
            //if (emailBCC != string.Empty)
            //{
            //    email.Bcc.Add(emailBCC);
            //}
   
            email.Subject = "Unit Testing - Results for plan " + runOptions.planName + " - " + resultsDateTimeString;
           
      
            //create message body
            email.Body = "Unit Testing - Results for plan " + runOptions.planName + " - " + resultsDateTimeString + "<br />" + results.Replace(System.Environment.NewLine, "<br />");

            

            //string environmentInfoToAttach = MIDRetail.Data.EnvironmentInfo.MIDInfo.GetAllEnvironmentInfo(Environment.NewLine);

            //System.Net.Mail.Attachment envAttachment;
            //byte[] byteArray = Encoding.ASCII.GetBytes(environmentInfoToAttach);
            //System.IO.MemoryStream streamForEnvAttachment = new System.IO.MemoryStream(byteArray);
            //envAttachment = new System.Net.Mail.Attachment(streamForEnvAttachment, "MIDEnvironmentInfo.txt");
            //email.Attachments.Add(envAttachment);

            if (attachAllResults && resultsFolder != string.Empty && System.IO.File.Exists(resultsFolder + "\\" + unitTestAllResultsFileName))
            {
                System.Net.Mail.Attachment attachment;
                attachment = new System.Net.Mail.Attachment(resultsFolder + "\\" + unitTestAllResultsFileName);
                email.Attachments.Add(attachment);
            }
            if (attachFailedResults && resultsFolder != string.Empty && System.IO.File.Exists(resultsFolder + "\\" + unitTestAllResultsFileName))
            {
                System.Net.Mail.Attachment attachment;
                attachment = new System.Net.Mail.Attachment(resultsFolder + "\\" + unitTestFailedResultsFileName);
                email.Attachments.Add(attachment);
            }

            //return email;
            SmtpClient client = new SmtpClient("smtp.midretail.com");
            client.Port = 25;
            client.UseDefaultCredentials = true;
            client.Send(email);
            }
            catch (Exception ex)
            {
                isSuccessful = false;
                AddFailureMessageToResults(ex.ToString());
            }
        }

       

        /// <summary>
        /// Creates a new database, and returns the connection string to that database
        /// </summary>
        /// <returns></returns>
        private static string CreateDatabase(out string dbName)
        {
            bool isConnectionOpen = false;
            System.Data.SqlClient.SqlCommand masterSqlCmd = null;
            dbName = "TechQA_521_SQL_UT_CREATE_TEST_" + resultsDateTimeString;
            try
            {
                string masterConnection = "server=" + runOptions.environmentServer + ";database=master;uid=sa;pwd=Midsa1;";

                masterSqlCmd = new System.Data.SqlClient.SqlCommand();
                masterSqlCmd.Connection = new System.Data.SqlClient.SqlConnection(masterConnection);
                masterSqlCmd.Connection.Open();
                isConnectionOpen = true;

              


                string cmd = string.Empty;
                cmd += @"CREATE DATABASE [TechQA]" + System.Environment.NewLine;
                cmd += @" CONTAINMENT = NONE" + System.Environment.NewLine;
                cmd += @" ON  PRIMARY " + System.Environment.NewLine;
                cmd += @"( NAME = N'TechQA', FILENAME = N'C:\Database1\TechQA.mdf' , SIZE = 4096KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )" + System.Environment.NewLine;
                cmd += @" LOG ON " + System.Environment.NewLine;
                cmd += @"( NAME = N'TechQA_log', FILENAME = N'C:\Database1\TechQA_log.ldf' , SIZE = 1024KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)" + System.Environment.NewLine;
                cmd = cmd.Replace("TechQA", dbName);
                masterSqlCmd.CommandText = cmd;
                masterSqlCmd.CommandTimeout = 0;
                masterSqlCmd.ExecuteNonQuery();

                cmd = "ALTER DATABASE [TechQA] SET COMPATIBILITY_LEVEL = 110";
                cmd = cmd.Replace("TechQA", dbName);
                masterSqlCmd.CommandText = cmd;
                masterSqlCmd.ExecuteNonQuery();

                cmd = "IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))" + System.Environment.NewLine;
                cmd += "begin" + System.Environment.NewLine;
                cmd += "EXEC [TechQA].[dbo].[sp_fulltext_database] @action = 'enable'" + System.Environment.NewLine;
                cmd += "end" + System.Environment.NewLine;
                cmd = cmd.Replace("TechQA", dbName);
                masterSqlCmd.CommandText = cmd;
                masterSqlCmd.ExecuteNonQuery();

                cmd = "ALTER DATABASE [TechQA] SET ANSI_NULL_DEFAULT OFF";
                cmd = cmd.Replace("TechQA", dbName);
                masterSqlCmd.CommandText = cmd;
                masterSqlCmd.ExecuteNonQuery();

                cmd = "ALTER DATABASE [TechQA] SET ANSI_NULLS OFF";
                cmd = cmd.Replace("TechQA", dbName);
                masterSqlCmd.CommandText = cmd;
                masterSqlCmd.ExecuteNonQuery();

                cmd = "ALTER DATABASE [TechQA] SET ANSI_PADDING OFF";
                cmd = cmd.Replace("TechQA", dbName);
                masterSqlCmd.CommandText = cmd;
                masterSqlCmd.ExecuteNonQuery();

                cmd = "ALTER DATABASE [TechQA] SET ANSI_WARNINGS OFF";
                cmd = cmd.Replace("TechQA", dbName);
                masterSqlCmd.CommandText = cmd;
                masterSqlCmd.ExecuteNonQuery();

                cmd = "ALTER DATABASE [TechQA] SET ARITHABORT OFF";
                cmd = cmd.Replace("TechQA", dbName);
                masterSqlCmd.CommandText = cmd;
                masterSqlCmd.ExecuteNonQuery();

                cmd = "ALTER DATABASE [TechQA] SET AUTO_CLOSE OFF";
                cmd = cmd.Replace("TechQA", dbName);
                masterSqlCmd.CommandText = cmd;
                masterSqlCmd.ExecuteNonQuery();

                cmd = "ALTER DATABASE [TechQA] SET AUTO_CREATE_STATISTICS ON";
                cmd = cmd.Replace("TechQA", dbName);
                masterSqlCmd.CommandText = cmd;
                masterSqlCmd.ExecuteNonQuery();

                cmd = "ALTER DATABASE [TechQA] SET AUTO_SHRINK OFF";
                cmd = cmd.Replace("TechQA", dbName);
                masterSqlCmd.CommandText = cmd;
                masterSqlCmd.ExecuteNonQuery();

                cmd = "ALTER DATABASE [TechQA] SET AUTO_UPDATE_STATISTICS ON";
                cmd = cmd.Replace("TechQA", dbName);
                masterSqlCmd.CommandText = cmd;
                masterSqlCmd.ExecuteNonQuery();

                cmd = "ALTER DATABASE [TechQA] SET CURSOR_CLOSE_ON_COMMIT OFF";
                cmd = cmd.Replace("TechQA", dbName);
                masterSqlCmd.CommandText = cmd;
                masterSqlCmd.ExecuteNonQuery();

                cmd = "ALTER DATABASE [TechQA] SET CURSOR_DEFAULT  GLOBAL";
                cmd = cmd.Replace("TechQA", dbName);
                masterSqlCmd.CommandText = cmd;
                masterSqlCmd.ExecuteNonQuery();

                cmd = "ALTER DATABASE [TechQA] SET CONCAT_NULL_YIELDS_NULL OFF";
                cmd = cmd.Replace("TechQA", dbName);
                masterSqlCmd.CommandText = cmd;
                masterSqlCmd.ExecuteNonQuery();

                cmd = "ALTER DATABASE [TechQA] SET NUMERIC_ROUNDABORT OFF";
                cmd = cmd.Replace("TechQA", dbName);
                masterSqlCmd.CommandText = cmd;
                masterSqlCmd.ExecuteNonQuery();

                cmd = "ALTER DATABASE [TechQA] SET QUOTED_IDENTIFIER OFF";
                cmd = cmd.Replace("TechQA", dbName);
                masterSqlCmd.CommandText = cmd;
                masterSqlCmd.ExecuteNonQuery();

                cmd = "ALTER DATABASE [TechQA] SET RECURSIVE_TRIGGERS OFF";
                cmd = cmd.Replace("TechQA", dbName);
                masterSqlCmd.CommandText = cmd;
                masterSqlCmd.ExecuteNonQuery();

                cmd = "ALTER DATABASE [TechQA] SET  DISABLE_BROKER";
                cmd = cmd.Replace("TechQA", dbName);
                masterSqlCmd.CommandText = cmd;
                masterSqlCmd.ExecuteNonQuery();

                cmd = "ALTER DATABASE [TechQA] SET AUTO_UPDATE_STATISTICS_ASYNC OFF";
                cmd = cmd.Replace("TechQA", dbName);
                masterSqlCmd.CommandText = cmd;
                masterSqlCmd.ExecuteNonQuery();

                cmd = "ALTER DATABASE [TechQA] SET DATE_CORRELATION_OPTIMIZATION OFF";
                cmd = cmd.Replace("TechQA", dbName);
                masterSqlCmd.CommandText = cmd;
                masterSqlCmd.ExecuteNonQuery();

                cmd = "ALTER DATABASE [TechQA] SET TRUSTWORTHY OFF";
                cmd = cmd.Replace("TechQA", dbName);
                masterSqlCmd.CommandText = cmd;
                masterSqlCmd.ExecuteNonQuery();

                cmd = "ALTER DATABASE [TechQA] SET ALLOW_SNAPSHOT_ISOLATION OFF";
                cmd = cmd.Replace("TechQA", dbName);
                masterSqlCmd.CommandText = cmd;
                masterSqlCmd.ExecuteNonQuery();

                cmd = "ALTER DATABASE [TechQA] SET PARAMETERIZATION SIMPLE";
                cmd = cmd.Replace("TechQA", dbName);
                masterSqlCmd.CommandText = cmd;
                masterSqlCmd.ExecuteNonQuery();

                cmd = "ALTER DATABASE [TechQA] SET READ_COMMITTED_SNAPSHOT OFF";
                cmd = cmd.Replace("TechQA", dbName);
                masterSqlCmd.CommandText = cmd;
                masterSqlCmd.ExecuteNonQuery();

                cmd = "ALTER DATABASE [TechQA] SET HONOR_BROKER_PRIORITY OFF";
                cmd = cmd.Replace("TechQA", dbName);
                masterSqlCmd.CommandText = cmd;
                masterSqlCmd.ExecuteNonQuery();

                cmd = "ALTER DATABASE [TechQA] SET RECOVERY SIMPLE";
                cmd = cmd.Replace("TechQA", dbName);
                masterSqlCmd.CommandText = cmd;
                masterSqlCmd.ExecuteNonQuery();

                cmd = "ALTER DATABASE [TechQA] SET  MULTI_USER";
                cmd = cmd.Replace("TechQA", dbName);
                masterSqlCmd.CommandText = cmd;
                masterSqlCmd.ExecuteNonQuery();

                cmd = "ALTER DATABASE [TechQA] SET PAGE_VERIFY CHECKSUM ";
                cmd = cmd.Replace("TechQA", dbName);
                masterSqlCmd.CommandText = cmd;
                masterSqlCmd.ExecuteNonQuery();

                cmd = "ALTER DATABASE [TechQA] SET DB_CHAINING OFF ";
                cmd = cmd.Replace("TechQA", dbName);
                masterSqlCmd.CommandText = cmd;
                masterSqlCmd.ExecuteNonQuery();

                cmd = "ALTER DATABASE [TechQA] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) ";
                cmd = cmd.Replace("TechQA", dbName);
                masterSqlCmd.CommandText = cmd;
                masterSqlCmd.ExecuteNonQuery();

                cmd = "ALTER DATABASE [TechQA] SET TARGET_RECOVERY_TIME = 0 SECONDS ";
                cmd = cmd.Replace("TechQA", dbName);
                masterSqlCmd.CommandText = cmd;
                masterSqlCmd.ExecuteNonQuery();

                cmd = "ALTER DATABASE [TechQA] SET  READ_WRITE ";
                cmd = cmd.Replace("TechQA", dbName);
                masterSqlCmd.CommandText = cmd;
                masterSqlCmd.ExecuteNonQuery();


                isSuccessful = true;
                return "server=" + runOptions.environmentServer + ";database=" + dbName + ";uid=sa;pwd=Midsa1;";
            }
            catch (Exception ex)
            {
                isSuccessful = false;
                AddFailureMessageToResults(ex.ToString());
                return string.Empty;
            }
            finally
            {
                if (masterSqlCmd != null && isConnectionOpen)
                {
                    masterSqlCmd.Connection.Close();
                }
            }
        }

        /// <summary>
        /// Creates a new database from a bak file for updating, and returns the connection string to that database
        /// </summary>
        /// <returns></returns>
        private static string CreateDatabaseFromBackupFileForUpgrade(out string dbName)
        {
            bool isConnectionOpen = false;
            System.Data.SqlClient.SqlCommand masterSqlCmd = null;
            dbName = "TechQA_521_SQL_UT_UPGRADE_TEST_" + resultsDateTimeString;
            try
            {
                string bakFilePath = runOptions.environmentBAKFilePath;
                AddMessageToResults("Upgrading from backup path=" + bakFilePath);
                string masterConnection = "server=" + runOptions.environmentServer + ";database=master;uid=sa;pwd=Midsa1;";

                masterSqlCmd = new System.Data.SqlClient.SqlCommand();
                masterSqlCmd.Connection = new System.Data.SqlClient.SqlConnection(masterConnection);
                masterSqlCmd.Connection.Open();
                isConnectionOpen = true;

                string cmd = string.Empty;
                cmd += @"RESTORE DATABASE " + dbName + System.Environment.NewLine;
                cmd += @" FROM DISK = '" + bakFilePath + "'" + System.Environment.NewLine;
                cmd += @" WITH REPLACE," + System.Environment.NewLine;
                cmd += @" MOVE 'middemo_Data' TO 'C:\Backup1\" + dbName + ".mdf'," + System.Environment.NewLine;
                cmd += @" MOVE 'middemo_Log' TO 'C:\Backup1\" + dbName + ".ldf'" + System.Environment.NewLine;
                masterSqlCmd.CommandText = cmd;
                masterSqlCmd.CommandTimeout = 0;
                masterSqlCmd.ExecuteNonQuery();

                isSuccessful = true;
                return "server=" + runOptions.environmentServer + ";database=" + dbName + ";uid=sa;pwd=Midsa1;";
            }
            catch (Exception ex)
            {
                isSuccessful = false;
                AddFailureMessageToResults(ex.ToString());
                return string.Empty;
            }
            finally
            {
                if (masterSqlCmd != null && isConnectionOpen)
                {
                    masterSqlCmd.Connection.Close();
                }
            }
        }

        private static void DropDatabase(string dbName)
        {
            bool isConnectionOpen = false;
            System.Data.SqlClient.SqlCommand masterSqlCmd = null;
            try
            {
                AddMessageToResults("Dropping test database: " + dbName);
                string masterConnection = "server=" + runOptions.environmentServer + ";database=master;uid=sa;pwd=Midsa1;";

                masterSqlCmd = new System.Data.SqlClient.SqlCommand();
                masterSqlCmd.Connection = new System.Data.SqlClient.SqlConnection(masterConnection);
                masterSqlCmd.Connection.Open();
                isConnectionOpen = true;

                string cmd = "ALTER DATABASE [" + dbName + "] SET SINGLE_USER WITH ROLLBACK IMMEDIATE";
                masterSqlCmd.CommandText = cmd;
                masterSqlCmd.ExecuteNonQuery();
                


                cmd = "DROP DATABASE [" + dbName + "]";
                masterSqlCmd.CommandText = cmd;
                masterSqlCmd.ExecuteNonQuery();

               

                isSuccessful = true;
            }
            catch (Exception ex)
            {
                isSuccessful = false;
                AddFailureMessageToResults(ex.ToString());
            }
            finally
            {
                if (masterSqlCmd != null && isConnectionOpen)
                {
                    masterSqlCmd.Connection.Close();
                }
            }
        }

        public class PlanOptions
        {
            public string environmentName = string.Empty;
            public string environmentServer = string.Empty;
            //public string environmentDatabase = string.Empty;
            public string environmentConnectionString = string.Empty;
            public string environmentBAKFilePath = string.Empty;

            public bool doValidateStoredProceduresOption = false;

            public bool doCreateNewDatabaseOption = false;
            public bool doRemoveNewDatabaseOnFailureOption = false;
            public bool doRemoveNewDatabaseOnSuccessOption = false;

            public bool doUpgradeStandardDatabaseOption = false;
            public bool doUseUpgradedDatabaseForUnitTestsOption = false;
            public bool doRemoveUpgradeDatabaseOnFailureOption = false;
            public bool doRemoveUpgradeDatabaseOnSuccessOption = false;

            public bool doRunUnitTestsOption = false;
            public bool doStopOnFirstUnitTestFailure = false;
            public bool doDeletePriorResultFolders = false;

            public bool doEmailResultsOnSuccessOption = false;
            public bool doEmailResultsOnFailureOption = false;

            
            public string emailSuccessTo = string.Empty;
            public string emailSuccessCc = string.Empty;
        
            public string emailFailureTo = string.Empty;
            public string emailFailureCc = string.Empty;

            public string planName = string.Empty;

            private DataSet MakePlanOptionDataSet()
            {
                DataSet dsPlanOptions = new DataSet();
                dsPlanOptions.Tables.Add("PlanOptions");
                dsPlanOptions.Tables[0].Columns.Add("environmentName", typeof(string));
                dsPlanOptions.Tables[0].Columns.Add("environmentServer", typeof(string));
                //dsPlanOptions.Tables[0].Columns.Add("environmentDatabase", typeof(string));
                dsPlanOptions.Tables[0].Columns.Add("environmentConnectionString", typeof(string));
                dsPlanOptions.Tables[0].Columns.Add("environmentBAKFilePath", typeof(string));
                dsPlanOptions.Tables[0].Columns.Add("doValidateStoredProceduresOption", typeof(bool));

                dsPlanOptions.Tables[0].Columns.Add("doCreateNewDatabaseOption", typeof(bool));
                dsPlanOptions.Tables[0].Columns.Add("doRemoveNewDatabaseOnFailureOption", typeof(bool));
                dsPlanOptions.Tables[0].Columns.Add("doRemoveNewDatabaseOnSuccessOption", typeof(bool));

                dsPlanOptions.Tables[0].Columns.Add("doUpgradeStandardDatabaseOption", typeof(bool));
                dsPlanOptions.Tables[0].Columns.Add("doUseUpgradedDatabaseForUnitTestsOption", typeof(bool));
                dsPlanOptions.Tables[0].Columns.Add("doRemoveUpgradeDatabaseOnFailureOption", typeof(bool));
                dsPlanOptions.Tables[0].Columns.Add("doRemoveUpgradeDatabaseOnSuccessOption", typeof(bool));


                dsPlanOptions.Tables[0].Columns.Add("doRunUnitTestsOption", typeof(bool));
                dsPlanOptions.Tables[0].Columns.Add("doStopOnFirstUnitTestFailure", typeof(bool));
                dsPlanOptions.Tables[0].Columns.Add("doDeletePriorResultFolders", typeof(bool));
             

                dsPlanOptions.Tables[0].Columns.Add("doEmailResultsOnSuccessOption", typeof(bool));
                dsPlanOptions.Tables[0].Columns.Add("emailSuccessTo", typeof(string));
                dsPlanOptions.Tables[0].Columns.Add("emailSuccessCc", typeof(string));
                dsPlanOptions.Tables[0].Columns.Add("doEmailResultsOnFailureOption", typeof(bool));
                dsPlanOptions.Tables[0].Columns.Add("emailFailureTo", typeof(string));
                dsPlanOptions.Tables[0].Columns.Add("emailFailureCc", typeof(string));

                dsPlanOptions.Tables[0].Columns.Add("planName", typeof(string));

                return dsPlanOptions;
            }

            public void ReadOptionsFromFile(string planOptionPath)
            {
                DataSet dsPlanOptions = MakePlanOptionDataSet();
                dsPlanOptions.ReadXml(planOptionPath);
                DataRow drPlanOptions = dsPlanOptions.Tables[0].Rows[0];
                environmentName = (string)drPlanOptions["environmentName"];
                environmentServer = (string)drPlanOptions["environmentServer"];
                //environmentDatabase = (string)drPlanOptions["environmentDatabase"];
                environmentConnectionString = (string)drPlanOptions["environmentConnectionString"];
                environmentBAKFilePath = (string)drPlanOptions["environmentBAKFilePath"];
                doValidateStoredProceduresOption = (bool)drPlanOptions["doValidateStoredProceduresOption"];

                doCreateNewDatabaseOption = (bool)drPlanOptions["doCreateNewDatabaseOption"];
                doRemoveNewDatabaseOnFailureOption = (bool)drPlanOptions["doRemoveNewDatabaseOnFailureOption"];
                doRemoveNewDatabaseOnSuccessOption = (bool)drPlanOptions["doRemoveNewDatabaseOnSuccessOption"];

                doUpgradeStandardDatabaseOption = (bool)drPlanOptions["doUpgradeStandardDatabaseOption"];
                doUseUpgradedDatabaseForUnitTestsOption = (bool)drPlanOptions["doUseUpgradedDatabaseForUnitTestsOption"];
                doRemoveUpgradeDatabaseOnFailureOption = (bool)drPlanOptions["doRemoveUpgradeDatabaseOnFailureOption"];
                doRemoveUpgradeDatabaseOnSuccessOption = (bool)drPlanOptions["doRemoveUpgradeDatabaseOnSuccessOption"];


                doRunUnitTestsOption = (bool)drPlanOptions["doRunUnitTestsOption"];
                doStopOnFirstUnitTestFailure = (bool)drPlanOptions["doStopOnFirstUnitTestFailure"];
                doDeletePriorResultFolders = (bool)drPlanOptions["doDeletePriorResultFolders"];
              
                doEmailResultsOnSuccessOption = (bool)drPlanOptions["doEmailResultsOnSuccessOption"];
                emailSuccessTo = (string)drPlanOptions["emailSuccessTo"];
                emailSuccessCc = (string)drPlanOptions["emailSuccessCc"];
                doEmailResultsOnFailureOption = (bool)drPlanOptions["doEmailResultsOnFailureOption"];
                emailFailureTo = (string)drPlanOptions["emailFailureTo"];
                emailFailureCc = (string)drPlanOptions["emailFailureCc"];

                if (drPlanOptions["planName"] != DBNull.Value)
                {
                    planName = (string)drPlanOptions["planName"];
                }
            }

           
            public void SaveOptionsToFile(string planOptionPath)
            {
                DataSet dsPlanOptions = MakePlanOptionDataSet();
                DataRow drPlanOptions = dsPlanOptions.Tables[0].NewRow();
                UpdateDataRow(drPlanOptions);
                dsPlanOptions.Tables[0].Rows.Add(drPlanOptions);
                dsPlanOptions.WriteXml(planOptionPath);
            }
            private void UpdateDataRow(DataRow drPlanOptions)
            {
                drPlanOptions["environmentName"] = environmentName;
                drPlanOptions["environmentServer"] = environmentServer;
                //drPlanOptions["environmentDatabase"] = environmentDatabase;
                drPlanOptions["environmentConnectionString"] = environmentConnectionString;
                drPlanOptions["environmentBAKFilePath"] = environmentBAKFilePath;
                drPlanOptions["doValidateStoredProceduresOption"] = doValidateStoredProceduresOption;

                drPlanOptions["doCreateNewDatabaseOption"] = doCreateNewDatabaseOption;
                drPlanOptions["doRemoveNewDatabaseOnFailureOption"] = doRemoveNewDatabaseOnFailureOption;
                drPlanOptions["doRemoveNewDatabaseOnSuccessOption"] = doRemoveNewDatabaseOnSuccessOption;

                drPlanOptions["doUpgradeStandardDatabaseOption"] = doUpgradeStandardDatabaseOption;
                drPlanOptions["doUseUpgradedDatabaseForUnitTestsOption"] = doUseUpgradedDatabaseForUnitTestsOption;
                drPlanOptions["doRemoveUpgradeDatabaseOnFailureOption"] = doRemoveUpgradeDatabaseOnFailureOption;
                drPlanOptions["doRemoveUpgradeDatabaseOnSuccessOption"] = doRemoveUpgradeDatabaseOnSuccessOption;

                drPlanOptions["doRunUnitTestsOption"] = doRunUnitTestsOption;
                drPlanOptions["doStopOnFirstUnitTestFailure"] = doStopOnFirstUnitTestFailure;
                drPlanOptions["doDeletePriorResultFolders"] = doDeletePriorResultFolders;
              
                drPlanOptions["doEmailResultsOnSuccessOption"] = doEmailResultsOnSuccessOption;
                drPlanOptions["emailSuccessTo"] = emailSuccessTo;
                drPlanOptions["emailSuccessCc"] = emailSuccessCc;
                drPlanOptions["doEmailResultsOnFailureOption"] = doEmailResultsOnFailureOption;
                drPlanOptions["emailFailureTo"] = emailFailureTo;
                drPlanOptions["emailFailureCc"] = emailFailureCc;

                drPlanOptions["planName"] = planName;
            }
            public string GetServerNameFromConnectionString(string conn)
            {
                int serverStart = conn.IndexOf("server=");
                conn = conn.Substring(serverStart);
                int semiStart = conn.IndexOf(";");
                return conn.Substring(7, semiStart - 7);
            }
        }


      
    }
}
