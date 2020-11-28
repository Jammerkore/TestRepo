using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Reflection;
using System.Windows.Forms;

using MIDRetail.Data;
using MIDRetail.DataCommon;

namespace UnitTesting
{
    public static class UnitTests
    {
        public static string unitTestFilePath; //= System.Configuration.ConfigurationManager.AppSettings["LocalUnitTestFilePath"];
        public static string globalPlanFilePath = System.Configuration.ConfigurationManager.AppSettings["GlobalPlanFilePath"];
        public static string globalTemplateFilePath = System.Configuration.ConfigurationManager.AppSettings["GlobalTemplateFilePath"];
        public static string defaultEnvironment = System.Configuration.ConfigurationManager.AppSettings["DefaultEnvironment"];

        static UnitTests()
        {
            unitTestFilePath = Shared_UtilityFunctions.GetCurrentProjectPath() + @"UnitTesting\Tests\";//System.Configuration.ConfigurationManager.AppSettings["LocalUnitTestFilePath"];
        }

        //public static string defaultTestName = "default";


        public static bool promptOnDelete = true;
        //public static string defaultFolder = "";

        public enum ReportTypes
        {
            ProceduresNotReferenced = 0,
            ParameterListing = 1,
            DatabaseComparison = 2,
            ObjectDBOCheck = 3
        }

        public static DataSet GetTestsForProcedure(string procedureName)
        {
            DataRow[] drFind = dsTests.Tables[0].Select("procedureName='" + procedureName + "'");

            DataSet dsTestsForProcedure = dsTests.Clone();
            foreach (DataRow dr in drFind)
            {
                DataRow drNew = dsTestsForProcedure.Tables[0].NewRow();
                for (int iColumnIndex = 0; iColumnIndex < drNew.Table.Columns.Count; iColumnIndex++)
                {
                    drNew[iColumnIndex] = dr[iColumnIndex];
                }

                dsTestsForProcedure.Tables[0].Rows.Add(drNew);
            }
            return dsTestsForProcedure;
        }
       
        public static string GenerateTestNameFromProcedure(string environmentName, string procedureName)
        {
            string testName = procedureName;
           

            testName = environmentName + "-" + procedureName; //+ "->" + testKindDescription;
            bool testNameIsValid = false;
            int iNameCounter = 2;
            string tempTestName = testName;
            while (testNameIsValid == false)
            {
                if (DoesTestAlreadyExistForProcedure(tempTestName, procedureName) == true)
                {
                    tempTestName = tempTestName + iNameCounter;
                    iNameCounter++;
                }
                else
                {
                    testName = tempTestName;
                    testNameIsValid = true;
                }
            }
            return testName;
        }
    

     

        
        public static DataSet GetParametersForUnitTest(string testName, string procedureName)
        {
            DataSet dsParms = dsTestParameters.Clone();
      

            DataRow[] drParms = dsTestParameters.Tables[0].Select("testName='" + testName + "' AND procedureName='" + procedureName + "'");
            foreach (DataRow dr in drParms)
            {
                DataRow drParam = dsParms.Tables[0].NewRow();
                Shared_UtilityFunctions.DataRowCopy(dr, drParam);
                dsParms.Tables[0].Rows.Add(drParam);
            }
            return dsParms;
        }
        public static DataSet GetExpectedResultsForUnitTest(string testName, string procedureName)
        {
            DataSet dsExpected = dsTestExpectedResults.Clone();
            DataRow[] drResultsForUnitTest = dsTestExpectedResults.Tables[0].Select("testName='" + testName + "' AND procedureName='" + procedureName + "'");
            foreach (DataRow dr in drResultsForUnitTest)
            {
                DataRow drExpected = dsExpected.Tables[0].NewRow();
                Shared_UtilityFunctions.DataRowCopy(dr, drExpected);
                dsExpected.Tables[0].Rows.Add(drExpected);
            }
            return dsExpected;
        }
        public static void UpdateUnitTestParameters(string testName, string procedureName, DataSet dsParms)
        {
             //Remove existing test parameters
             RemoveParametersForUnitTest(testName, procedureName);

             //Add new test paramters
             foreach (DataRow dr in dsParms.Tables[0].Rows)
             {
                 DataRow drNewParam = dsTestParameters.Tables[0].NewRow();
                 Shared_UtilityFunctions.DataRowCopy(dr, drNewParam);
                 dsTestParameters.Tables[0].Rows.Add(drNewParam);
             }
        }
        private static void RemoveParametersForUnitTest(string testName, string procedureName)
        {
            DataRow[] drParms = dsTestParameters.Tables[0].Select("testName='" + testName + "' AND procedureName='" + procedureName + "'");
            foreach (DataRow dr in drParms)
            {
                dsTestParameters.Tables[0].Rows.Remove(dr);
            }
        }
        private static void RemoveExpectedResultsForUnitTest(string testName, string procedureName, ExpectedResultKinds resultKind)
        {
            DataRow[] drExpectedResults = dsTestExpectedResults.Tables[0].Select("testName='" + testName + "' AND procedureName='" + procedureName + "' AND resultKind = '" + resultKind.Name + "'");
            foreach (DataRow dr in drExpectedResults)
            {
                dsTestExpectedResults.Tables[0].Rows.Remove(dr);
            }
        }
        private static void RemoveAllExpectedResultsForUnitTest(string testName, string procedureName)
        {
            DataRow[] drExpectedResults = dsTestExpectedResults.Tables[0].Select("testName='" + testName + "' AND procedureName='" + procedureName + "'");
            foreach (DataRow dr in drExpectedResults)
            {
                dsTestExpectedResults.Tables[0].Rows.Remove(dr);
            }
        }
        public static void DeleteSelectedTests(DataRow[] drSelectedTests)
        {
            foreach (DataRow dr in drSelectedTests)
            {
                //string environmentName = (string)dr["environmentName"];
                string testName = (string)dr["testName"];
                string procedureName = (string)dr["procedureName"];

                DataRow[] drTest = dsTests.Tables[0].Select("testName='" + testName + "' AND procedureName='" + procedureName + "'");
                if (drTest.Length > 0)
                {
                    RemoveParametersForUnitTest(testName, procedureName);
                    RemoveAllExpectedResultsForUnitTest(testName, procedureName);
                    dsTests.Tables[0].Rows.Remove(drTest[0]);
                }
            }
            AutoSave();
        }
        public static void AutoSave()
        {
            //if (defaultFolder != string.Empty)
            //{
                SaveTestFiles(false);
            //}
        }
        public static void ExpectedResults_Delete(string procedureName, string testName, DataRow[] drExpectedResults)
        {
            foreach (DataRow dr in drExpectedResults)
            {
                int mainRowIndex = (int)dr["mainRowIndex"];
                dsTestExpectedResults.Tables[0].Rows.RemoveAt(mainRowIndex);
            }

            UpdateExpectedResultsForTest(testName, procedureName);
            AutoSave();
        }
        public static void ExpectedResults_Update(string procedureName, string testName, int mainRowIndex, DataRow newExpectedResult)
        {

            dsTestExpectedResults.Tables[0].Rows.RemoveAt(mainRowIndex);

            DataRow dr = dsTestExpectedResults.Tables[0].NewRow();
            dr["testName"] = newExpectedResult["testName"];
            dr["procedureName"] = newExpectedResult["procedureName"];
            dr["resultKind"] = newExpectedResult["resultKind"];
            dr["fieldName"] = newExpectedResult["fieldName"];
            dr["expectedValue"] = newExpectedResult["expectedValue"];
            dsTestExpectedResults.Tables[0].Rows.Add(dr);
            UpdateExpectedResultsForTest(testName, procedureName);
            AutoSave();
        }
        private static DataSet _dsExpectedResultsForTest;
        public static DataSet dsExpectedResultsForTest
        {
            get
            {
                return _dsExpectedResultsForTest;
            }
            set
            {
                _dsExpectedResultsForTest = value;
            }

        }
        public static void UpdateExpectedResultsForTest(string testName, string procedureName)
        {
            if (dsExpectedResultsForTest == null)
            {
                dsExpectedResultsForTest = dsTestExpectedResults.Clone();
                dsExpectedResultsForTest.Tables[0].Columns.Add("mainRowIndex", typeof(int));
            }
            else
            {
                dsExpectedResultsForTest.Tables[0].Rows.Clear();
            }
            
            DataRow[] drExpectedResultsForThisTest = dsTestExpectedResults.Tables[0].Select("testName='" + testName + "' AND procedureName='" + procedureName + "'");
            foreach (DataRow dr in drExpectedResultsForThisTest)
            {
                DataRow drExpectedResults = dsExpectedResultsForTest.Tables[0].NewRow();
                drExpectedResults["testName"] = dr["testName"];
                drExpectedResults["procedureName"] = dr["procedureName"];
                drExpectedResults["resultKind"] = dr["resultKind"];
                drExpectedResults["fieldName"] = dr["fieldName"];
                drExpectedResults["expectedValue"] = dr["expectedValue"];
                drExpectedResults["mainRowIndex"] = dsTestExpectedResults.Tables[0].Rows.IndexOf(dr);

                dsExpectedResultsForTest.Tables[0].Rows.Add(drExpectedResults);
            }
            
        }
        public static DataSet GetTestParametersFromProcedure(string procedureName)
        {
            DataSet dsParms = dsTestParameters.Clone();


            baseStoredProcedure sp = Shared_BaseStoredProcedures.GetStoredProcedure(procedureName);

            if (sp != null)
            {
                //Set Parameters for testing
                foreach (MIDRetail.Data.baseParameter param in sp.inputParameterList)
                {
                    DataRow drParam = dsParms.Tables[0].NewRow();
                    drParam["testName"] = "";
                    drParam["procedureName"] = procedureName;
                    drParam["parameterName"] = param.parameterName;
                    drParam["parameterType"] = param.DBType.ToString();
                    drParam["parameterValue"] = Include.NullForStringValue;  //TT#1310-MD -jsobek -Error when adding a new Store
                    dsParms.Tables[0].Rows.Add(drParam);

                }
            }
            return dsParms;
        }
       
        public static bool DoesTestAlreadyExistForProcedure(string newTestName, string procedureName)
        {
            bool exists = false;
            DataRow[] drResults = dsTests.Tables[0].Select("testName='" + newTestName + "' AND procedureName='" + procedureName + "'");
            if (drResults.Length > 0)
            {
                exists = true;
            }
            return exists;
        }
 
        //public static bool DoesEnvironmentAlreadyExist(string environmentName)
        //{
        //    bool exists = false;
        //    DataRow[] drResults = dsEnvironments.Tables[0].Select("environmentName='" + environmentName + "'");
        //    if (drResults.Length > 0)
        //    {
        //        exists = true;
        //    }
        //    return exists;
        //}

        public static bool DoesEnvironmentExist(string environmentName)
        {
            bool exists = false;
            if (dsEnvironments == null)
                return true;

            DataRow[] drResults = dsEnvironments.Tables[0].Select("environmentName='" + environmentName + "'");
            if (drResults.Length > 0)
            {
                exists = true;
            }

            return exists;
        }

       // public static string sCurrentProjectDir;

        private static DataSet dsTests;
        private static DataSet dsTestParameters;
        public static DataSet dsTestExpectedResults;
        private static DataSet dsEnvironments;
        private static DataSet dsEnvironmentsForSelection;

        //private static DataSet dsSettings;
    
        public static void InitializeTestStructures()
        {


            dsTests = new DataSet();
            dsTests.Tables.Add("Tests");
            dsTests.Tables[0].Columns.Add("environmentName", typeof(string));
            dsTests.Tables[0].Columns.Add("testName", typeof(string));
            dsTests.Tables[0].Columns.Add("procedureName", typeof(string));
            dsTests.Tables[0].Columns.Add("procedureType", typeof(string));
            dsTests.Tables[0].Columns.Add("sequence", typeof(string));
            dsTests.Tables[0].Columns.Add("isSuspended", typeof(string));
            dsTests.Tables[0].Columns.Add("expectedResultCount", typeof(int));
            dsTests.Tables[0].Columns.Add("validationMsg", typeof(string));

            dsTestParameters = new DataSet();
            dsTestParameters.Tables.Add("TestParameters");
            dsTestParameters.Tables[0].Columns.Add("testName", typeof(string));
            dsTestParameters.Tables[0].Columns.Add("procedureName", typeof(string));
            dsTestParameters.Tables[0].Columns.Add("parameterName", typeof(string));
            dsTestParameters.Tables[0].Columns.Add("parameterType", typeof(string));
            dsTestParameters.Tables[0].Columns.Add("parameterValue", typeof(string));

            dsTestExpectedResults = new DataSet();
            dsTestExpectedResults.Tables.Add("TestExpectedResults");
            dsTestExpectedResults.Tables[0].Columns.Add("testName", typeof(string));
            dsTestExpectedResults.Tables[0].Columns.Add("procedureName", typeof(string));
            dsTestExpectedResults.Tables[0].Columns.Add("resultKind", typeof(string));
            dsTestExpectedResults.Tables[0].Columns.Add("fieldName", typeof(string));
            dsTestExpectedResults.Tables[0].Columns.Add("expectedValue", typeof(string));

            dsEnvironments = new DataSet();
            dsEnvironments.Tables.Add("Environments");
            dsEnvironments.Tables[0].Columns.Add("environmentName");
            dsEnvironments.Tables[0].Columns.Add("connectionString");
            dsEnvironments.Tables[0].Columns.Add("server");
            dsEnvironments.Tables[0].Columns.Add("databaseName");
            dsEnvironments.Tables[0].Columns.Add("bakFilePath");

            dsEnvironmentsForSelection = new DataSet();
            dsEnvironmentsForSelection.Tables.Add("EnvironmentsForSelection");
            dsEnvironmentsForSelection.Tables[0].Columns.Add("environmentName");
            dsEnvironmentsForSelection.Tables[0].Columns.Add("connectionString");
            dsEnvironmentsForSelection.Tables[0].Columns.Add("server");
            dsEnvironmentsForSelection.Tables[0].Columns.Add("databaseName");
            dsEnvironmentsForSelection.Tables[0].Columns.Add("bakFilePath");

           

            //dsSettings = new DataSet();
            //dsSettings.Tables.Add("Settings");
            //dsSettings.Tables[0].Columns.Add("defaultEnvironment", typeof(string));
            //dsSettings.Tables[0].Columns.Add("promptOnDelete", typeof(string));

        }

        //public static void ApplySettings()
        //{
        //    //Assumes settings have been loaded into dsSettings
        //    DataRow drSettings = dsSettings.Tables[0].Rows[0];

        //    defaultEnvironment = (string)drSettings["defaultEnvironment"];
        //    if ((string)drSettings["promptOnDelete"] == "Y")
        //    {
        //        promptOnDelete = true;
        //    }
        //    else
        //    {
        //        promptOnDelete = false;
        //    }

        //}
        //public static void StoreSettings()
        //{
        //    dsSettings.Tables[0].Rows.Clear();
        //    DataRow drSettings = dsSettings.Tables[0].NewRow();
        //    drSettings["defaultEnvironment"] = defaultEnvironment;
        //    if (promptOnDelete == true)
        //    {
        //        drSettings["promptOnDelete"] = "Y";
        //    }
        //    else
        //    {
        //         drSettings["promptOnDelete"] = "N";
        //    }

        //    dsSettings.Tables[0].Rows.Add(drSettings);
        //}
        public static void SaveTestFiles(bool showMessage = true)
        {
            if (System.IO.Directory.Exists(unitTestFilePath) == false)
            {
                System.IO.Directory.CreateDirectory(unitTestFilePath);
            }
            System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(unitTestFilePath);
            di.Attributes &= ~System.IO.FileAttributes.ReadOnly; //= System.IO.FileAttributes.Normal;
            foreach(System.IO.FileInfo fi in di.GetFiles("*.xml"))
            {
                fi.Attributes &= ~System.IO.FileAttributes.ReadOnly; //= System.IO.FileAttributes.Normal;
            }
            dsTests.WriteXml(unitTestFilePath + "\\unitTests.xml");
            dsTestParameters.WriteXml(unitTestFilePath + "\\unitTestParameters.xml");
            dsTestExpectedResults.WriteXml(unitTestFilePath + "\\unitTestExpectedResults.xml");
            dsEnvironments.WriteXml(unitTestFilePath + "\\unitTestEnvironments.xml");
            //StoreSettings();
            //dsSettings.WriteXml(unitTestFilePath + folder + "\\unitTestSettings.xml");
            if (showMessage)
            {
                MessageBox.Show("Test files have been saved here: " + UnitTests.unitTestFilePath);
            }
        }
        public static void SaveTestFilesForPlan(DataSet dsSelected, string planPath, bool showMessage = false)
        {
            DataSet dsSelectedTests = dsTests.Clone();
            DataSet dsSelectedTestParameters = dsTestParameters.Clone();
            DataSet dsSelectedTestExpectedResults = dsTestExpectedResults.Clone();

            foreach (DataRow dr in dsSelected.Tables[0].Rows)
            {
                DataRow drSelected = dsSelectedTests.Tables[0].NewRow();

                string testName = (string)dr["testName"];
                string procedureName = (string)dr["procedureName"];

                DataRow drTest =GetUnitTestFromName(testName, procedureName);
                Shared_UtilityFunctions.DataRowCopy(drTest, drSelected);
                dsSelectedTests.Tables[0].Rows.Add(drSelected);
            }
            foreach (DataRow dr in dsSelectedTests.Tables[0].Rows)
            {
                string testName = (string)dr["testName"];
                string procedureName = (string)dr["procedureName"];

                DataSet dsParameters = GetParametersForUnitTest(testName, procedureName);
                foreach (DataRow drParameter in dsParameters.Tables[0].Rows)
                {
                    DataRow drSelected = dsSelectedTestParameters.Tables[0].NewRow();
                    Shared_UtilityFunctions.DataRowCopy(drParameter, drSelected);
                    dsSelectedTestParameters.Tables[0].Rows.Add(drSelected);
                }

                DataSet dsExpectedResults = GetExpectedResultsForUnitTest(testName, procedureName);
                foreach (DataRow drExpectedResult in dsExpectedResults.Tables[0].Rows)
                {
                    DataRow drSelected = dsSelectedTestExpectedResults.Tables[0].NewRow();
                    Shared_UtilityFunctions.DataRowCopy(drExpectedResult, drSelected);
                    dsSelectedTestExpectedResults.Tables[0].Rows.Add(drSelected);
                }
            }

            dsSelectedTests.WriteXml(planPath + "\\unitTests.xml", XmlWriteMode.WriteSchema);
            dsSelectedTestParameters.WriteXml(planPath + "\\unitTestParameters.xml", XmlWriteMode.WriteSchema);
            dsSelectedTestExpectedResults.WriteXml(planPath + "\\unitTestExpectedResults.xml", XmlWriteMode.WriteSchema);
            dsEnvironments.WriteXml(planPath + "\\unitTestEnvironments.xml", XmlWriteMode.WriteSchema);
            //StoreSettings();
            //dsSettings.WriteXml(planPath + "\\unitTestSettings.xml", XmlWriteMode.WriteSchema);

            if (showMessage)
            {
                MessageBox.Show("Plan files have been saved here: " + planPath);
            }
        }
        public static void LoadTestFiles(string fullpath)
        {
            if (fullpath == string.Empty)
            {
                LoadDefaultData();
                return;
            }
            else
            {
                if (System.IO.Directory.Exists(fullpath) == false)
                {
                    System.Windows.Forms.MessageBox.Show("Unit Test folder " + fullpath + " does not exist.");
                    LoadDefaultData();
                    return;
                }
                dsTests.ReadXml(fullpath + "\\unitTests.xml");
                foreach (DataRow dr in dsTests.Tables[0].Rows)
                {
                    string procedureName = (string)dr["procedureName"];
                    dr["procedureType"] = GetProcedureTypeFromName(procedureName);
                }
                dsTestParameters.ReadXml(fullpath + "\\unitTestParameters.xml");
                dsTestExpectedResults.ReadXml(fullpath + "\\unitTestExpectedResults.xml");
                ValidateTests();
                dsEnvironments.ReadXml(fullpath + "\\unitTestEnvironments.xml");
                MakeEnvironmentsForSelection();
                //dsSettings.ReadXml(fullpath + "\\unitTestSettings.xml");
                //ApplySettings();
                DeleteAllTempSQLFiles();
            }
        }
        private static void ValidateTests()
        {
            //string sPathInsertOrder = @"C:\MIDRetail\UnitTesting\delete_order.csv";
            //System.IO.StreamReader sr = new System.IO.StreamReader(sPathInsertOrder);
            //string sInsertOrder = sr.ReadToEnd();
            //sr.Close();
            //string[] s = sInsertOrder.Split(System.Environment.NewLine.ToCharArray(),StringSplitOptions.None);
            //foreach(string sline in s)
            //{
            //    if (sline != string.Empty)
            //    {
            //        string[] sSplit = sline.Split(',');
            //        string sTest = sSplit[0];
            //        string sSeq = sSplit[1];
            //        DataRow[] drTestFind = dsTests.Tables[0].Select("procedureName='" + sTest + "' AND (procedureType='Delete')");
            //        drTestFind[0]["sequence"] = sSeq;
            //    }
            //}


            foreach (DataRow drTest in dsTests.Tables[0].Rows)
            {
                string procedureName = (string)drTest["procedureName"];
                string testName = (string)drTest["testName"];
                drTest["validationMsg"] = string.Empty;
                drTest["expectedResultCount"] = GetExpectedResultCountForTest(procedureName, testName);

                //ensure the procedure exists
                baseStoredProcedure sp = Shared_BaseStoredProcedures.GetStoredProcedure(procedureName);

                if (sp == null)
                {
                    drTest["validationMsg"] = "Procedure Missing: " + procedureName;
                }
                else
                {


         
                    DataSet latestParameters = GetTestParametersFromProcedure(procedureName);
                    DataSet testParameters = GetParametersForUnitTest(testName, procedureName);
                    //Compare the latest parameters to the parameters saved on the test
                    foreach (DataRow drLatestParameter in latestParameters.Tables[0].Rows)
                    {
                        string parameterName = (string)drLatestParameter["parameterName"];
                        string parameterType = (string)drLatestParameter["parameterType"];
                        bool parameterAlreadyExists = true;

                        DataRow[] drFound = testParameters.Tables[0].Select("parameterName='" + parameterName + "'");

                        if (drFound.Length == 0)
                        {
                            parameterAlreadyExists = false;
                        }

                        if (parameterAlreadyExists == false)
                        {
                            DataRow drNewParameter = dsTestParameters.Tables[0].NewRow();
                            drNewParameter["testName"] = testName;
                            drNewParameter["procedureName"] = procedureName;
                            drNewParameter["parameterName"] = parameterName;
                            drNewParameter["parameterType"] = parameterType;
                            drNewParameter["parameterValue"] = drLatestParameter["parameterValue"];
                            dsTestParameters.Tables[0].Rows.Add(drNewParameter);
                            if (drTest["validationMsg"] != DBNull.Value)
                            {
                                drTest["validationMsg"] = (string)drTest["validationMsg"] +  ", Added parameter: " + parameterName;
                            }
                            else
                            {
                                drTest["validationMsg"] = "Added parameter: " + parameterName;
                            }
                        }
                    }

                    foreach (DataRow drTestParameter in testParameters.Tables[0].Rows)
                    {
                        string parameterName = (string)drTestParameter["parameterName"];
                        string parameterType = (string)drTestParameter["parameterType"];
                        bool parameterStillExists = true;

                        DataRow[] drFound = latestParameters.Tables[0].Select("parameterName='" + parameterName + "'");

                        if (drFound.Length == 0)
                        {
                            parameterStillExists = false;
                        }

                        if (parameterStillExists == false)
                        {
                            DataRow[] drFindMain = dsTestParameters.Tables[0].Select("procedureName ='" + procedureName + "' AND testName='" + testName + "' AND parameterName='" + parameterName + "'");
                            dsTestParameters.Tables[0].Rows.Remove(drFindMain[0]);
                            if (drTest["validationMsg"] != DBNull.Value)
                            {
                                drTest["validationMsg"] = (string)drTest["validationMsg"] + ", Removed parameter: " + parameterName;
                            }
                            else
                            {
                                drTest["validationMsg"] = "Removed parameter: " + parameterName;
                            }
                        }
                    }
                }
            }
        }
        public static void SaveTempSQLFile(string sql, string fileName, out string fullpath)
        {
            string folder = @"\tempSQL";
            if (System.IO.Directory.Exists(System.IO.Path.GetTempPath() + folder) == false)
            {
                System.IO.Directory.CreateDirectory(System.IO.Path.GetTempPath() + folder);
            }
            fullpath = System.IO.Path.GetTempPath() + folder + "\\" + fileName;
            System.IO.StreamWriter sw = new System.IO.StreamWriter(fullpath);
            sw.WriteLine(sql);
            sw.Flush();
            sw.Close();

        }
        public static void DeleteAllTempSQLFiles()
        {
            string folder = @"\tempSQL";
            if (System.IO.Directory.Exists(System.IO.Path.GetTempPath() + folder) == true)
            {
                System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(System.IO.Path.GetTempPath() + folder);
                try
                {
                    di.Delete(true);
                }
                catch
                {
                    //ignore errors when attempting to delete
                }
            }
           

        }
        private static void LoadDefaultData()
        {
            AddEnvironment("TQA", "server=MIDretail27;database=TechQA_521_SQL;uid=sa;pwd=Midsa1;", "MIDretail27", "TechQA_521_SQL", "C:\\Backup1\\TechQA_521_SQL.bak");
            MakeEnvironmentsForSelection();
            //ApplySettings();
        }
        private static void MakeEnvironmentsForSelection()
        {
            dsEnvironmentsForSelection = dsEnvironments.Copy();

            //DataRow dr = dsEnvironmentsForSelection.Tables[0].NewRow();
            //dr["environmentName"] = "Any";
            //dr["connectionString"] = "N/A";
            //dsEnvironmentsForSelection.Tables[0].Rows.InsertAt(dr, 0);
        }
        public static void AddEnvironment(string environmentName, string connectionString, string server, string databaseName, string bakFilePath)
        {
            DataRow dr = dsEnvironments.Tables[0].NewRow();
            dr["environmentName"] = environmentName;
            dr["connectionString"] = connectionString;
            dr["server"] = server;
            dr["databaseName"] = databaseName;
            dr["bakFilePath"] = bakFilePath;
            dsEnvironments.Tables[0].Rows.Add(dr);

            DataRow dr2 = dsEnvironmentsForSelection.Tables[0].NewRow();
            dr2["environmentName"] = environmentName;
            dr2["connectionString"] = connectionString;
            dr2["server"] = server;
            dr2["databaseName"] = databaseName;
            dr2["bakFilePath"] = bakFilePath;
            dsEnvironmentsForSelection.Tables[0].Rows.Add(dr2);
        }
        public static DataRow GetEnvironmentFromName(string environmentName)
        {
            DataRow[] drFind = dsEnvironments.Tables[0].Select("environmentName='" + environmentName + "'");
            if (drFind.Length > 0)
            {
                return drFind[0];
            }
            else
            {
                return null;
            }
        }
        public static void EditEnvironment(string oldEnvironmentName, string newEnvironmentName, string connectionString, string server, string databaseName, string bakFilePath)
        {
            DataRow[] dr = dsEnvironments.Tables[0].Select("environmentName='" + oldEnvironmentName + "'");
            dr[0]["environmentName"] = newEnvironmentName;
            dr[0]["connectionString"] = connectionString;
            dr[0]["server"] = server;
            dr[0]["databaseName"] = databaseName;
            dr[0]["bakFilePath"] = bakFilePath;

            DataRow[] dr2 = dsEnvironmentsForSelection.Tables[0].Select("environmentName='" + oldEnvironmentName + "'");
            dr2[0]["environmentName"] = newEnvironmentName;
            dr2[0]["connectionString"] = connectionString;
            dr2[0]["server"] = server;
            dr2[0]["databaseName"] = databaseName;
            dr2[0]["bakFilePath"] = bakFilePath;

            RenameEnvironmentInTests(oldEnvironmentName, newEnvironmentName);
        }
        public static void DeleteEnvironment(string environmentName)
        {
            DataRow[] dr = dsEnvironments.Tables[0].Select("environmentName='" + environmentName + "'");
            dsEnvironments.Tables[0].Rows.Remove(dr[0]);

            DataRow[] dr2 = dsEnvironmentsForSelection.Tables[0].Select("environmentName='" + environmentName + "'");
            dsEnvironmentsForSelection.Tables[0].Rows.Remove(dr2[0]);

            DataRow[] drTest = dsTests.Tables[0].Select("environmentName='" + environmentName + "'");
            DeleteSelectedTests(drTest);
        }
        public static DataSet GetEnvironmentsForEditing()
        {
            return dsEnvironments;
        }
        public static DataSet GetEnvironmentsForSelection()
        {
            return dsEnvironmentsForSelection;
        }
        public static string GetConnectionForEnvironment(string environmentName)
        {
            DataRow[] env = dsEnvironments.Tables[0].Select("environmentName='" + environmentName + "'");
            return (string)env[0]["connectionString"];
        }

        public static void GetPathAndFolderFromFullPath(string fullpath, out string path, out string folder)
        {
            int folderStartIndex = fullpath.LastIndexOf("\\");
            int folderStartIndex2 = fullpath.LastIndexOf("\\", folderStartIndex - 1);
            folder = fullpath.Substring(folderStartIndex2 + 1, folderStartIndex - folderStartIndex2 - 1);
            path = fullpath.Substring(0, folderStartIndex2);
        }


        public sealed class ExpectedResultKinds 
        {
            public static readonly ExpectedResultKinds RowCountEqualsX = new ExpectedResultKinds("RowCountEqualsX");
            public static readonly ExpectedResultKinds RowCountEqualsOne = new ExpectedResultKinds("RowCountEqualsOne");
            public static readonly ExpectedResultKinds RowCountGreaterThanZero = new ExpectedResultKinds("RowCountGreaterThanZero");
            public static readonly ExpectedResultKinds FieldEquals = new ExpectedResultKinds("FieldEquals");
            public static readonly ExpectedResultKinds OutputParameterEquals = new ExpectedResultKinds("OutputParameterEquals");
            public static readonly ExpectedResultKinds CompareScalarValue = new ExpectedResultKinds("CompareScalarValue");
            

            private ExpectedResultKinds(string Name)
            {
                this.Name = Name;
        
            }
            public string Name { get; private set; }
            public static implicit operator string(ExpectedResultKinds op) { return op.Name; }
      
        }

        //public static void GridSetColumnHeaderCaptionFromLayout(Infragistics.Win.UltraWinGrid.UltraGridLayout layout, string field, string headerCaption)
        //{
        //    if (layout.Bands[0].Columns.)
        //    {
        //        layout.Bands[0].Columns[field].Header.Caption = headerCaption;
        //    }
            
        //}
        public static void ExpectedResults_AddRowCountEqualsX(string testName, string procedureName, int expectedRowCount)
        {
            DataRow drExpectedResults = dsTestExpectedResults.Tables[0].NewRow();
            drExpectedResults["testName"] = testName;
            drExpectedResults["procedureName"] = procedureName;
            drExpectedResults["resultKind"] = ExpectedResultKinds.RowCountEqualsX.Name;
            drExpectedResults["fieldName"] = "";
            drExpectedResults["expectedValue"] = expectedRowCount.ToString();
            dsTestExpectedResults.Tables[0].Rows.Add(drExpectedResults);
            UnitTests.UpdateExpectedResultsForTest(testName, procedureName);
        }
        public static void ExpectedResults_AddCompareValue(string testName, string procedureName, object scalarValue)
        {
            DataRow drExpectedResults = dsTestExpectedResults.Tables[0].NewRow();
            drExpectedResults["testName"] = testName;
            drExpectedResults["procedureName"] = procedureName;
            drExpectedResults["resultKind"] = ExpectedResultKinds.CompareScalarValue.Name;
            drExpectedResults["fieldName"] = "";
            drExpectedResults["expectedValue"] = scalarValue.ToString();
            dsTestExpectedResults.Tables[0].Rows.Add(drExpectedResults);
            UnitTests.UpdateExpectedResultsForTest(testName, procedureName);
        }
        public static void ExpectedResults_AddOutputParameterMatching(string testName, string procedureName, DataSet dsOutputParameters)
        {

            RemoveExpectedResultsForUnitTest(testName, procedureName, ExpectedResultKinds.OutputParameterEquals);
            foreach (DataRow dr in dsOutputParameters.Tables[0].Rows)
            {
                string parameterName = (string)dr["parameterName"];
                string parameterValue = (string)dr["parameterValue"];
                ExpectedResults_AddOutputParameterEquals(testName, procedureName, parameterName, parameterValue);
            }
            //UnitTests.UpdateExpectedResultsForTest(testName, procedureName);
        }
        public static void ExpectedResults_AddOutputParameterEquals(string testName, string procedureName, string parameterName, string parameterValue)
        {
            DataRow drExpectedResults = dsTestExpectedResults.Tables[0].NewRow();
            drExpectedResults["testName"] = testName;
            drExpectedResults["procedureName"] = procedureName;
            drExpectedResults["resultKind"] = ExpectedResultKinds.OutputParameterEquals.Name;
            drExpectedResults["fieldName"] = parameterName;
            drExpectedResults["expectedValue"] = parameterValue;
            dsTestExpectedResults.Tables[0].Rows.Add(drExpectedResults);
            UnitTests.UpdateExpectedResultsForTest(testName, procedureName);

        }
        public static void ExpectedResults_AddRowCountEqualsOne(string testName, string procedureName)
        {
            DataRow drExpectedResults = dsTestExpectedResults.Tables[0].NewRow();
            drExpectedResults["testName"] = testName;
            drExpectedResults["procedureName"] = procedureName;
            drExpectedResults["resultKind"] = ExpectedResultKinds.RowCountEqualsOne.Name;
            drExpectedResults["fieldName"] = "";
            drExpectedResults["expectedValue"] = "1";
            dsTestExpectedResults.Tables[0].Rows.Add(drExpectedResults);
            UnitTests.UpdateExpectedResultsForTest(testName, procedureName);
       
        }
        public static void ExpectedResults_AddRowCountGreaterThanZero(string testName, string procedureName)
        {
            DataRow drExpectedResults = dsTestExpectedResults.Tables[0].NewRow();
            drExpectedResults["testName"] = testName;
            drExpectedResults["procedureName"] = procedureName;
            drExpectedResults["resultKind"] = ExpectedResultKinds.RowCountGreaterThanZero.Name;
            drExpectedResults["fieldName"] = "";
            drExpectedResults["expectedValue"] = "0";
            dsTestExpectedResults.Tables[0].Rows.Add(drExpectedResults);
            UnitTests.UpdateExpectedResultsForTest(testName, procedureName);

        }
        public static void ExpectedResults_AddFieldEquals(string testName, string procedureName, string fieldName, object fieldValue)
        {
            DataRow drExpectedResults = dsTestExpectedResults.Tables[0].NewRow();
            drExpectedResults["testName"] = testName;
            drExpectedResults["procedureName"] = procedureName;
            drExpectedResults["resultKind"] = ExpectedResultKinds.FieldEquals.Name;
            drExpectedResults["fieldName"] = fieldName;
            drExpectedResults["expectedValue"] = ConvertObjectToString(fieldValue);
            dsTestExpectedResults.Tables[0].Rows.Add(drExpectedResults);
            UnitTests.UpdateExpectedResultsForTest(testName, procedureName);
     
        }
        public static void ExpectedResults_AddFieldsMatching(string testName, string procedureName, DataRow drFields)
        {
            RemoveExpectedResultsForUnitTest(testName, procedureName, ExpectedResultKinds.FieldEquals);
            foreach(DataColumn col in drFields.Table.Columns)
            {
                string fieldName = col.ColumnName;
                ExpectedResults_AddFieldEquals(testName, procedureName, fieldName, drFields[fieldName]);
            }
            UnitTests.UpdateExpectedResultsForTest(testName, procedureName);
        }
    
        public static DataSet GetUnitTests()
        {
            return dsTests;
        }
        public static DataRow GetUnitTestFromName(string testName, string procedureName)
        {
            DataRow[] drFind = dsTests.Tables[0].Select("testName='" + testName + "' AND procedureName='" + procedureName + "'");
            if (drFind.Length > 0)
            {
                return drFind[0];
            }
            else
            {
                return null;
            }
        }
        public static void AddUnitTest(string environmentName, string testName, string procedureName, string sequence, string isSuspended, DataSet dsParms)
        {
            DataRow drTest = dsTests.Tables[0].NewRow();
            drTest["environmentName"] = environmentName;
            drTest["testName"] = testName;
            drTest["procedureName"] = procedureName;
            drTest["procedureType"] = GetProcedureTypeFromName(procedureName);
            drTest["sequence"] = sequence;
            drTest["isSuspended"] = isSuspended;
            dsTests.Tables[0].Rows.Add(drTest);

            foreach (DataRow dr in dsParms.Tables[0].Rows)
            {
                DataRow drParam = dsTestParameters.Tables[0].NewRow();
                drParam["testName"] = testName;
                drParam["procedureName"] = procedureName;
                drParam["parameterName"] = dr["parameterName"];
                drParam["parameterType"] = dr["parameterType"];
                drParam["parameterValue"] = dr["parameterValue"];
                dsTestParameters.Tables[0].Rows.Add(drParam);
            }
        }
        private static string GetProcedureTypeFromName(string procedureName)
        {
            MIDRetail.Data.baseStoredProcedure sp = Shared_BaseStoredProcedures.GetStoredProcedure(procedureName);
            if (sp != null)
            {
                return sp.procedureType.ToString();
            }
            else
            {
                return string.Empty;
            }
        }
        public static void EditUnitTest(string procedureName, string oldTestName, string newTestName, string environmentName, string sequence, string isSuspended, DataSet dsParms)
        {
            DataRow[] drFind = dsTests.Tables[0].Select("procedureName='" + procedureName + "' AND testName='" + oldTestName + "'");
            drFind[0]["environmentName"] = environmentName;
            drFind[0]["testName"] = newTestName;
            drFind[0]["procedureName"] = procedureName;
            drFind[0]["procedureType"] = GetProcedureTypeFromName(procedureName);
            drFind[0]["sequence"] = sequence;
            drFind[0]["isSuspended"] = isSuspended;

            RemoveParametersForUnitTest(oldTestName, procedureName);

            


            foreach (DataRow dr in dsParms.Tables[0].Rows)
            {
                DataRow drParam = dsTestParameters.Tables[0].NewRow();
                drParam["testName"] = newTestName;
                drParam["procedureName"] = procedureName;
                drParam["parameterName"] = dr["parameterName"];
                drParam["parameterType"] = dr["parameterType"];
                drParam["parameterValue"] = dr["parameterValue"];
                dsTestParameters.Tables[0].Rows.Add(drParam);
            }
            RenameExpectedResults(oldTestName, newTestName);
        }
        private static void RenameExpectedResults(string oldTestName, string newTestName)
        {
            DataRow[] drFind = dsTestExpectedResults.Tables[0].Select("testName='" + oldTestName + "'");
            foreach (DataRow dr in drFind)
            {
                dr["testName"] = newTestName;
            }
        }
        private static void RenameEnvironmentInTests(string oldEnvironmentName, string newEnvironmentName)
        {
            DataRow[] drFind = dsTests.Tables[0].Select("environmentName='" + oldEnvironmentName + "'");
            foreach (DataRow dr in drFind)
            {
                dr["environmentName"] = newEnvironmentName;
            }
        }
        public static int CountTestsForEnvironment(string environmentName)
        {
            DataRow[] drFind = dsTests.Tables[0].Select("environmentName='" + environmentName + "'");
            return drFind.Length;
        }

        public static DataSet GetCurrentSQLColumns(string conn)
        {

            string cmdText = "SELECT o.name as tableName, c.name as columnName, max_length, precision, scale, is_nullable, is_identity "
                            +"FROM sys.columns c "
                            +"INNER JOIN dbo.sysobjects o on c.object_id=o.id "
                            +"WHERE OBJECTPROPERTY(id, N'IsUserTable') = 1 "
                            +"ORDER BY o.name, column_id ";
            System.Data.SqlClient.SqlConnection sqlConn = new System.Data.SqlClient.SqlConnection(conn);
            System.Data.SqlClient.SqlCommand sqlCmd = new System.Data.SqlClient.SqlCommand(cmdText, sqlConn);
            DataSet ds = new DataSet();
            System.Data.SqlClient.SqlDataAdapter sda = new System.Data.SqlClient.SqlDataAdapter(sqlCmd);
            sda.Fill(ds);
            return ds;
        }
      
        

       
   

        private static bool IsEnvironmentValidToRunNow()
        {
            if (defaultEnvironment == string.Empty)
            {
                System.Windows.Forms.MessageBox.Show("No default DB has been set.");
                return false;
            }
            if (defaultEnvironment == "Any")
            {
                System.Windows.Forms.MessageBox.Show("Default DB cannot be set to Any to view immediate results.");
                return false;
            }
            return true;
        }
        public static DataTable ExecuteRead(string procedureName, DataSet dsRunNowParmeters, out bool hasError, out string failureMsg, out DataSet dsOutputParameters)
        {
            if (IsEnvironmentValidToRunNow() == false)
            {
                hasError = true;
                failureMsg = "Environment not valid.";
                dsOutputParameters = null;
                return null;
            }

            MIDRetail.Data.baseStoredProcedure sp = Shared_BaseStoredProcedures.GetStoredProcedure(procedureName);

            Shared_SetParameter.SetParametersForProcedure(sp, dsRunNowParmeters, GetConnectionForEnvironment(defaultEnvironment));
       
            DataTable dt;
            DatabaseAccess dba = new DatabaseAccess(GetConnectionForEnvironment(defaultEnvironment));
            Shared_GenericExecution.DoRead(sp, dba, out hasError, out failureMsg, out dt, out dsOutputParameters);
            
            return dt;
        }
        public static int ExecuteReadAsCount(string procedureName, DataSet dsRunNowParmeters, out bool hasError, out string failureMsg)
        {
            if (IsEnvironmentValidToRunNow() == false)
            {
                hasError = true;
                failureMsg = "Environment not valid.";
                return -1;
            }

            MIDRetail.Data.baseStoredProcedure sp = Shared_BaseStoredProcedures.GetStoredProcedure(procedureName);

            Shared_SetParameter.SetParametersForProcedure(sp, dsRunNowParmeters, GetConnectionForEnvironment(defaultEnvironment));
            int count;
            DatabaseAccess dba = new DatabaseAccess(GetConnectionForEnvironment(defaultEnvironment));
            Shared_GenericExecution.DoReadAsRecordCount(sp, dba, out hasError, out failureMsg, out count);

            return count;
        }
        public static int ExecuteInsert(string procedureName, DataSet dsRunNowParmeters, out bool hasError, out string failureMsg, out DataSet dsOutputParameters)
        {
            if (IsEnvironmentValidToRunNow() == false)
            {
                hasError = true;
                failureMsg = "Environment not valid.";
                dsOutputParameters = null;
                return 0;
            }
            MIDRetail.Data.baseStoredProcedure sp = Shared_BaseStoredProcedures.GetStoredProcedure(procedureName);

            //string conn = GetConnectionForEnvironment(defaultEnvironment);
            //DataSet dsSQLColumns = GetCurrentSQLColumns(conn);

            Shared_SetParameter.SetParametersForProcedure(sp, dsRunNowParmeters, GetConnectionForEnvironment(defaultEnvironment));

            
            //int rowsCleared;
            int rowsInserted;
            //string selectCmd;

            //DoClearExistingBeforeInsert(sp, defaultEnvironment, dsRunNowParmeters, out selectCmd, out hasError, out failureMsg, out rowsCleared);
            //if (hasError)
            //{
            //    MessageBox.Show(failureMsg);
            //    return null;
            //}
            DatabaseAccess dba = new DatabaseAccess(GetConnectionForEnvironment(defaultEnvironment));
            Shared_GenericExecution.DoInsert(sp, dba, out hasError, out failureMsg, out rowsInserted, out dsOutputParameters);

            //if (hasError)
            //{
            //    MessageBox.Show(failureMsg);
            //    return null;
            //}
            ////DataRow[] drCols = dsSQLColumns.Tables[0].Select("tableName='" + tableName + "' AND is_identity=1");

            //DataTable dt = GetGenericDataTable(GetConnectionForEnvironment(defaultEnvironment), selectCmd, out hasError, out failureMsg);
            //if (hasError)
            //{
            //    MessageBox.Show(failureMsg);
            //    return null;
            //}
     
           
            //return dt;
            return rowsInserted;
        }
        public static int ExecuteInsertAndReturnRID(string procedureName, DataSet dsRunNowParmeters, out bool hasError, out string failureMsg, out DataSet dsOutputParameters)
        {
            if (IsEnvironmentValidToRunNow() == false)
            {
                hasError = true;
                failureMsg = "Environment not valid.";
                dsOutputParameters = null;
                return 0;
            }
            MIDRetail.Data.baseStoredProcedure sp = Shared_BaseStoredProcedures.GetStoredProcedure(procedureName);

            //string conn = GetConnectionForEnvironment(defaultEnvironment);
            //DataSet dsSQLColumns = GetCurrentSQLColumns(conn);

            Shared_SetParameter.SetParametersForProcedure(sp, dsRunNowParmeters, GetConnectionForEnvironment(defaultEnvironment));


            //int rowsCleared;
            int newRID;
            //string selectCmd;

            //DoClearExistingBeforeInsert(sp, defaultEnvironment, dsRunNowParmeters, out selectCmd, out hasError, out failureMsg, out rowsCleared);
            //if (hasError)
            //{
            //    MessageBox.Show(failureMsg);
            //    return null;
            //}
            DatabaseAccess dba = new DatabaseAccess(GetConnectionForEnvironment(defaultEnvironment));
            Shared_GenericExecution.DoInsertAndReturnRID(sp, dba, out hasError, out failureMsg, out newRID, out dsOutputParameters);

            //if (hasError)
            //{
            //    MessageBox.Show(failureMsg);
            //    return null;
            //}
            ////DataRow[] drCols = dsSQLColumns.Tables[0].Select("tableName='" + tableName + "' AND is_identity=1");

            //DataTable dt = GetGenericDataTable(GetConnectionForEnvironment(defaultEnvironment), selectCmd, out hasError, out failureMsg);
            //if (hasError)
            //{
            //    MessageBox.Show(failureMsg);
            //    return null;
            //}


            //return dt;
            return newRID;
        }
        public static object ExecuteReadScalar(string procedureName, DataSet dsRunNowParmeters, out bool hasError, out string failureMsg)
        {
            if (IsEnvironmentValidToRunNow() == false)
            {
                hasError = true;
                failureMsg = "Environment not valid.";
                return null;
            }
            MIDRetail.Data.baseStoredProcedure sp = Shared_BaseStoredProcedures.GetStoredProcedure(procedureName);
            Shared_SetParameter.SetParametersForProcedure(sp, dsRunNowParmeters, GetConnectionForEnvironment(defaultEnvironment));
            DatabaseAccess dba = new DatabaseAccess(GetConnectionForEnvironment(defaultEnvironment));
            object scalarValue = null;
            Shared_GenericExecution.DoReadScalar(sp, dba, out hasError, out failureMsg, out scalarValue);
            return scalarValue;
        }
        public static int ExecuteUpdate(string procedureName, DataSet dsRunNowParmeters, out bool hasError, out string failureMsg, out DataSet dsOutputParameters)
        {
            if (IsEnvironmentValidToRunNow() == false)
            {
                hasError = true;
                failureMsg = "Environment not valid.";
                dsOutputParameters = null;
                return 0;
            }
            MIDRetail.Data.baseStoredProcedure sp = Shared_BaseStoredProcedures.GetStoredProcedure(procedureName);

            //string conn = GetConnectionForEnvironment(defaultEnvironment);
            //DataSet dsSQLColumns = GetCurrentSQLColumns(conn);

            Shared_SetParameter.SetParametersForProcedure(sp, dsRunNowParmeters, GetConnectionForEnvironment(defaultEnvironment));



            int rowsUpdated;

            //string selectCmd = string.Empty;

            //DataRow[] drSisterSelect = dsDefaultAttributes.Tables[0].Select("procedureName='" + procedureName + "'");
            //if (drSisterSelect.Length > 0)
            //{
            //    selectCmd = (string)drSisterSelect[0]["selectStatement"];
            //    SetParametersOnSelectStatement(ref selectCmd, dsRunNowParmeters);
            //}

            //if (selectCmd == string.Empty)
            //{
            //    MessageBox.Show("Update command has no sister select statement.");
            //    return null;
            //}

            //DataTable dtOriginal = GetGenericDataTable(GetConnectionForEnvironment(defaultEnvironment), selectCmd, out hasError, out failureMsg);
            //if (hasError)
            //{
            //    MessageBox.Show(failureMsg);
            //    return null;
            //}
            DatabaseAccess dba = new DatabaseAccess(GetConnectionForEnvironment(defaultEnvironment));
            Shared_GenericExecution.DoUpdate(sp, dba, out hasError, out failureMsg, out rowsUpdated, out dsOutputParameters);
            //if (hasError)
            //{
            //    MessageBox.Show(failureMsg);
            //    return null;
            //}


            //DataTable dtUpdated = GetGenericDataTable(GetConnectionForEnvironment(defaultEnvironment), selectCmd, out hasError, out failureMsg);
            //if (hasError)
            //{
            //    MessageBox.Show(failureMsg);
            //    return null;
            //}

            //int rowsReset;
            //DoResetAfterUpdate(sp, defaultEnvironment, dtOriginal, out hasError, out failureMsg, out rowsReset);
            //if (hasError)
            //{
            //    MessageBox.Show(failureMsg);
            //    return null;
            //}


            //return dtUpdated;
            return rowsUpdated;
        }
        public static int ExecuteDelete(string procedureName, DataSet dsRunNowParmeters, out bool hasError, out string failureMsg, out DataSet dsOutputParameters)
        {
            if (IsEnvironmentValidToRunNow() == false)
            {
                hasError = true;
                failureMsg = "Environment not valid.";
                dsOutputParameters = null;
                return 0;
            }
            MIDRetail.Data.baseStoredProcedure sp = Shared_BaseStoredProcedures.GetStoredProcedure(procedureName);

            //string conn = GetConnectionForEnvironment(defaultEnvironment);
            //DataSet dsSQLColumns = GetCurrentSQLColumns(conn);

            Shared_SetParameter.SetParametersForProcedure(sp, dsRunNowParmeters, GetConnectionForEnvironment(defaultEnvironment));


      
            int rowsDeleted;

            //string selectCmd = string.Empty;

            //DataRow[] drSisterSelect = dsDefaultAttributes.Tables[0].Select("procedureName='" + procedureName + "'");
            //if (drSisterSelect.Length > 0)
            //{
            //    selectCmd = (string)drSisterSelect[0]["selectStatement"];
            //    SetParametersOnSelectStatement(ref selectCmd, dsRunNowParmeters);
            //}

            //if (selectCmd == string.Empty)
            //{
            //    MessageBox.Show("Delete command has no sister select statement.");
            //    return null;
            //}

            //DataTable dtOriginal = GetGenericDataTable(GetConnectionForEnvironment(defaultEnvironment), selectCmd, out hasError, out failureMsg);
            //if (hasError)
            //{
            //    MessageBox.Show(failureMsg);
            //    return null;
            //}
            DatabaseAccess dba = new DatabaseAccess(GetConnectionForEnvironment(defaultEnvironment));
            Shared_GenericExecution.DoDelete(sp, dba, out hasError, out failureMsg, out rowsDeleted, out dsOutputParameters);
            //if (hasError)
            //{
            //    MessageBox.Show(failureMsg);
            //    return null;
            //}


            //DataTable dtDeleted = GetGenericDataTable(GetConnectionForEnvironment(defaultEnvironment), selectCmd, out hasError, out failureMsg);
            //if (hasError)
            //{
            //    MessageBox.Show(failureMsg);
            //    return null;
            //}

            //int rowsReset;
            //DoResetAfterDelete(sp, defaultEnvironment, dtOriginal, out hasError, out failureMsg, out rowsReset);
            //if (hasError)
            //{
            //    MessageBox.Show(failureMsg);
            //    return null;
            //}


            //return dtDeleted;
            return rowsDeleted;
        }

  
        //private static void SetParametersOnSelectStatement(ref string selectStatement, DataSet dsParms)
        //{
        //    foreach (DataRow dr in dsParms.Tables[0].Rows)
        //    {
        //        string parameterName = (string)dr["parameterName"];
        //        string parameterValue = (string)dr["parameterValue"];

        //        int pos = selectStatement.IndexOf(parameterName);
        //        if (pos != -1)
        //        {
        //            selectStatement = selectStatement.Replace(parameterName, parameterValue);
        //        }
        //    }
        //}
     
        //private static void DoClearExistingBeforeInsert(baseStoredProcedure sp, string environmentName, DataSet dsParameters, out string selectCmd, out bool hasError, out string failureMsg, out int rowsCleared)
        //{
        //    hasError = false;
        //    failureMsg = string.Empty;
        //    rowsCleared = 0;
        //    selectCmd = string.Empty;

        //    if (dsParameters.Tables[0].Rows.Count == 0)
        //    {
        //        hasError = true;
        //        failureMsg = "No parameters defined when atttempting to clear row.";
        //        return;
        //    }


        //    string tableName = sp.tableNames[0];
        //    selectCmd = "SELECT * FROM " + tableName + " WHERE " + GetWhereClauseFromParameters(sp, dsParameters);
        //    string deleteCmd = "DELETE FROM " + tableName + " WHERE " + GetWhereClauseFromParameters(sp, dsParameters);
        //    DataTable dtClearExisting;
        //    try
        //    {
        //        dtClearExisting = GetGenericDataTable(GetConnectionForEnvironment(environmentName), selectCmd, out hasError, out failureMsg);
        //    }
        //    catch (Exception ex)
        //    {
        //        hasError = true;
        //        failureMsg = "Error selecting row to clear: " + System.Environment.NewLine + ex.ToString();
        //        return;
        //    }

        //    if (hasError == false && dtClearExisting.Rows.Count == 1)
        //    {
        //        //Clear the row if it exists
        //        System.Data.SqlClient.SqlConnection sqlConn = new System.Data.SqlClient.SqlConnection(GetConnectionForEnvironment(environmentName));
        //        System.Data.SqlClient.SqlCommand sqlCmd = new System.Data.SqlClient.SqlCommand(deleteCmd, sqlConn);

        //        try
        //        {
        //            sqlConn.Open();
        //            rowsCleared = sqlCmd.ExecuteNonQuery();
        //        }
        //        catch (Exception ex)
        //        {
        //            hasError = true;
        //            failureMsg = "Error attempting to clear existing row..." + System.Environment.NewLine + ex.ToString();
        //        }
        //        finally
        //        {
        //            sqlConn.Close();
        //        }
        //        if (rowsCleared == 0)
        //        {
        //            hasError = true;
        //            failureMsg = "Failed to clear existing row.";
        //        }
        //    }
        //}
        //private static void DoResetAfterUpdate(baseStoredProcedure sp, string environmentName, DataTable dtOriginal, out bool hasError, out string failureMsg, out int rowsReset)
        //{
        //    hasError = false;
        //    failureMsg = string.Empty;
        //    rowsReset = 0;

        //    //Reset the state for the affected rows
        //    //Assume first column is the PK
        //    foreach (DataRow drOrig in dtOriginal.Rows)
        //    {
        //        string resetUpdateCmd = "UPDATE " + sp.tableNames[0] + " SET ";

        //        bool firstCol = true;
        //        bool needComma = false;
        //        foreach (DataColumn col in dtOriginal.Columns)
        //        {
        //            if (firstCol == true)
        //            {
        //                firstCol = false;
        //            }
        //            else
        //            {
        //                resetUpdateCmd += col.ColumnName + "=" + ConvertObjectToString(drOrig[col]);
        //                if (needComma)
        //                {
        //                    resetUpdateCmd += ",";
        //                }
        //                needComma = true;
        //            }
        //        }

        //        string PKcol = dtOriginal.Columns[0].ColumnName;
        //        resetUpdateCmd += " WHERE " + PKcol + "=" + drOrig[dtOriginal.Columns[0]].ToString();
        //        DoGenericUpdateInsertDelete(GetConnectionForEnvironment(environmentName), resetUpdateCmd, out hasError, out failureMsg);
        //        if (hasError)
        //        {
        //            return;
        //        }
        //    }

        //}
        //private static void DoResetAfterDelete(baseStoredProcedure sp, string environmentName, DataTable dtOriginal, out bool hasError, out string failureMsg, out int rowsReset)
        //{
        //    hasError = false;
        //    failureMsg = string.Empty;
        //    rowsReset = 0;

        //    //Reset the state for the affected rows
        //    //Assume first column is the PK
        //    foreach (DataRow drOrig in dtOriginal.Rows)
        //    {
        //        string resetCmd = "INSERT INTO " + sp.tableNames[0] + " VALUES ( ";

          
        //        int iCount = 1;
        //        foreach (DataColumn col in dtOriginal.Columns)
        //        {
                      
        //                resetCmd += ConvertObjectToString(drOrig[col]);
        //                if (iCount != dtOriginal.Columns.Count)
        //                {
        //                    resetCmd += ",";
        //                }
        //                iCount++;
            
        //        }
        //        resetCmd += ")";
        //        string PKcol = dtOriginal.Columns[0].ColumnName;
        //        //resetCmd += " WHERE " + PKcol + "=" + drOrig[dtOriginal.Columns[0]].ToString();
        //        DoGenericUpdateInsertDelete(GetConnectionForEnvironment(environmentName), resetCmd, out hasError, out failureMsg);
        //        if (hasError)
        //        {
        //            return;
        //        }
        //    }

        //}

        private static string ConvertObjectToString(object o)
        {
            string val = o.ToString();
            if (val == "False")
            {
                val = "0";
            }
            if (val == "True")
            {
                val = "1";
            }
               return val;
        }
      
        //private static string GetWhereClauseFromParameters(MIDRetail.Data.baseStoredProcedure sp, DataSet drParmetersForTest)
        //{
        //    string whereClause = string.Empty;
        //    foreach (MIDRetail.Data.baseParameter param in sp.inputParameterList)
        //    {
        //        DataRow[] drParmValue = drParmetersForTest.Tables[0].Select("parameterName='" + param.parameterName + "'");
        //        if (drParmValue.Length > 0)
        //        {
        //            if (param.DBType == MIDRetail.DataCommon.eDbType.Int)
        //            {
        //                MIDRetail.Data.intParameter p = (MIDRetail.Data.intParameter)param;
        //                int? testValue = ConvertToIntNullable((string)drParmValue[0]["parameterValue"]);
        //                if (testValue != null)
        //                {
        //                    whereClause += " AND " + param.parameterName.Replace("@", "") + "=" + testValue.ToString();
        //                }
        //            }
        //            else if (param.DBType == MIDRetail.DataCommon.eDbType.Int64)
        //            {
        //                MIDRetail.Data.longParameter p = (MIDRetail.Data.longParameter)param;
        //                long? testValue = ConvertToLongNullable((string)drParmValue[0]["parameterValue"]);
        //                if (testValue != null)
        //                {
        //                    whereClause += " AND " + param.parameterName.Replace("@", "") + "=" + testValue.ToString();
        //                }
        //            }
        //            else if (param.DBType == MIDRetail.DataCommon.eDbType.Float)
        //            {
        //                MIDRetail.Data.floatParameter p = (MIDRetail.Data.floatParameter)param;
        //                double? testValue = ConvertToDoubleNullable((string)drParmValue[0]["parameterValue"]);
        //                if (testValue != null)
        //                {
        //                    whereClause += " AND " + param.parameterName.Replace("@", "") + "=" + testValue.ToString();
        //                }
        //            }
        //            else if (param.DBType == MIDRetail.DataCommon.eDbType.Decimal)
        //            {
        //                MIDRetail.Data.decimalParameter p = (MIDRetail.Data.decimalParameter)param;
        //                double? testValue = ConvertToDoubleNullable((string)drParmValue[0]["parameterValue"]);
        //                if (testValue != null)
        //                {
        //                    whereClause += " AND " + param.parameterName.Replace("@", "") + "=" + testValue.ToString();
        //                }
        //            }
        //            else if (param.DBType == MIDRetail.DataCommon.eDbType.Image)
        //            {
        //                //MIDRetail.Data.byteArrayParameter p = (MIDRetail.Data.byteArrayParameter)param;
        //                //byte[] testValue = (byte[])drParmValue[0]["parameterValue"];
        //                //if (testValue != null)
        //                //{
        //                //    whereClause += " AND " + param.parameterName.Replace("@", "") + "=" + testValue.ToString();
        //                //}
        //            }
        //            else if (param.DBType == MIDRetail.DataCommon.eDbType.VarChar)
        //            {
        //                MIDRetail.Data.stringParameter p = (MIDRetail.Data.stringParameter)param;
        //                string testValue = ConvertToStringNullable((string)drParmValue[0]["parameterValue"]);
        //                if (testValue != "null")
        //                {
        //                    whereClause += " AND " + param.parameterName.Replace("@", "") + "=" + testValue;
        //                }
        //            }
        //            else if (param.DBType == MIDRetail.DataCommon.eDbType.Text)
        //            {
        //                MIDRetail.Data.textParameter p = (MIDRetail.Data.textParameter)param;
        //                string testValue = ConvertToStringNullable((string)drParmValue[0]["parameterValue"]);
        //                if (testValue != "null")
        //                {
        //                    whereClause += " AND " + param.parameterName.Replace("@", "") + "=" + testValue;
        //                }
        //            }
        //            else if (param.DBType == MIDRetail.DataCommon.eDbType.Char)
        //            {
        //                MIDRetail.Data.charParameter p = (MIDRetail.Data.charParameter)param;
        //                char? testValue = ConvertToCharNullable((string)drParmValue[0]["parameterValue"]);
        //                if (testValue != null)
        //                {
        //                    whereClause += " AND " + param.parameterName.Replace("@", "") + "=" + testValue.ToString();
        //                }
        //            }
        //            else if (param.DBType == MIDRetail.DataCommon.eDbType.DateTime)
        //            {
        //                MIDRetail.Data.datetimeParameter p = (MIDRetail.Data.datetimeParameter)param;
        //                DateTime? testValue = ConvertToDateTimeNullable((string)drParmValue[0]["parameterValue"]);
        //                if (testValue != null)
        //                {
        //                    whereClause += " AND " + param.parameterName.Replace("@", "") + "=" + testValue.ToString();
        //                }
        //            }
        //        }
        //    }
        //    //Remove beginning 'AND'
        //    whereClause = whereClause.Substring(5);
        //    return whereClause;
        //}

     


        #region "Stored Procedures as DataSet"

        private static DataSet dsStoredProcedures;

        public static DataSet GetStoredProcedureListAsDataset()
        {
            if (dsStoredProcedures == null)
            {
                dsStoredProcedures = new DataSet();
                dsStoredProcedures.Tables.Add("StoredProcedures");
                dsStoredProcedures.Tables[0].Columns.Add("moduleName");
                dsStoredProcedures.Tables[0].Columns.Add("procedureName");
                dsStoredProcedures.Tables[0].Columns.Add("procedureType");
                dsStoredProcedures.Tables[0].Columns.Add("tableNames");
                dsStoredProcedures.Tables[0].Columns.Add("testCount", typeof(int));
                dsStoredProcedures.Tables[0].Columns.Add("expectedResultCount", typeof(int));
                dsStoredProcedures.Tables[0].Columns.Add("hasDefaults");
                dsStoredProcedures.Tables[0].Columns.Add("hasNoLock");
                dsStoredProcedures.Tables[0].Columns.Add("hasRowLock");
                dsStoredProcedures.Tables[0].Columns.Add("hasOrderBy");
                dsStoredProcedures.Tables[0].Columns.Add("canRestoreState");
                dsStoredProcedures.Tables[0].Columns.Add("validation");
                FillStoredProcedureDataSetFromList();
            }
            return dsStoredProcedures;
        }
        private static void FillStoredProcedureDataSetFromList()
        {
            dsStoredProcedures.Tables[0].Rows.Clear();
            //storedProcedureList.Sort();
            foreach (baseStoredProcedure sp in Shared_BaseStoredProcedures.storedProcedureList)
            {
                int firstPlusLen = sp.GetType().FullName.IndexOf('+');
                string moduleName = sp.GetType().FullName.Substring(0, firstPlusLen);

                Type declaringType = sp.GetType().DeclaringType;
                string declaringTypeName = declaringType.Name;
                moduleName = moduleName.Replace("MIDRetail.Data.", "");

                DataRow dr = dsStoredProcedures.Tables[0].NewRow();
                dr["testCount"] = GetTestCountForProcedure(sp.procedureName);
                dr["expectedResultCount"] = GetExpectedResultCountForProcedure(sp.procedureName);
                dr["procedureName"] = sp.procedureName;
                dr["procedureType"] = sp.procedureType.ToString();
                //string tableNames = string.Empty;
                //foreach (string tableName in sp.tableNames)
                //{
                //    tableNames += tableName + ",";
                //}
                //if (tableNames.Length > 0)
                //{
                //    tableNames = tableNames.Substring(0, tableNames.Length - 1);
                //}
                //dr["tableNames"] = tableNames;

                string primaryTable = string.Empty;
                if (sp.tableNames.Count > 0)
                {
                    primaryTable = sp.tableNames[0];
                }
                dr["tableNames"] = primaryTable;

                dr["moduleName"] = moduleName;
                bool hasDefaults = Shared_BaseStoredProcedures.HasDefaults(sp.procedureName);
                if (hasDefaults)
                {
                    dr["hasDefaults"] = "Y";
                }
                else
                {
                    dr["hasDefaults"] = "N";
                }
               
                bool hasNoLock = false;
                bool hasRowLock = false;
                bool hasOrderBy = false;
                dr["validation"] = Shared_ValidateStoredProcedures.ValidateStoredProcedure(sp.procedureName, out hasNoLock, out hasRowLock, out hasOrderBy);

                if (hasNoLock)
                {
                    dr["hasNoLock"] = "Y";
                }
                else
                {
                    dr["hasNoLock"] = "N";
                }
                if (hasRowLock)
                {
                    dr["hasRowLock"] = "Y";
                }
                else
                {
                    dr["hasRowLock"] = "N";
                }
                if (hasOrderBy)
                {
                    dr["hasOrderBy"] = "Y";
                }
                else
                {
                    dr["hasOrderBy"] = "N";
                }
                dsStoredProcedures.Tables[0].Rows.Add(dr);
            }
        }
        public static void RefreshStoredProcedures()
        {
            //Refresh the list
            Shared_BaseStoredProcedures.PopulateStoredProcedureListFromAssembly();
            //Repopulate the dataset from the list
            FillStoredProcedureDataSetFromList();
            Shared_BaseStoredProcedures.GetModuleListAsDataset();
        }
        private static int GetTestCountForProcedure(string procedureName)
        {
            DataRow[] drFind = dsTests.Tables[0].Select("procedureName='" + procedureName + "'");
            return drFind.Length;

        }
        private static int GetExpectedResultCountForProcedure(string procedureName)
        {

            DataRow[] drFind = dsTests.Tables[0].Select("procedureName='" + procedureName + "'");
            int expectedResultCount = 0;
            foreach (DataRow dr in drFind)
            {
                string testName = (string)dr["testName"];
                DataRow[] drFindExpectedResults = dsTestExpectedResults.Tables[0].Select("procedureName='" + procedureName + "' AND testName='" + testName + "'");
                expectedResultCount += drFindExpectedResults.Length;
            }
            return expectedResultCount;
        }
        private static int GetExpectedResultCountForTest(string procedureName, string testName)
        {
            DataRow[] drFindExpectedResults = dsTestExpectedResults.Tables[0].Select("procedureName='" + procedureName + "' AND testName='" + testName + "'");


            return drFindExpectedResults.Length;
        }
        public static bool DoesProcedureExist(string procedureName)
        {
            bool exists = false;
            DataRow[] drResults = dsStoredProcedures.Tables[0].Select("procedureName='" + procedureName + "'");
            if (drResults.Length > 0)
            {
                exists = true;
            }
            return exists;
        }
#endregion

        public static event SelectPlan_OK_EventHandler SelectPlan_OK_Event;
        public static void RaiseSelectPlan_OK_Event(object sender, string planName)
        {

            if (SelectPlan_OK_Event != null)
                SelectPlan_OK_Event(sender, new SelectPlan_OK_EventArgs(planName));
        }
        public class SelectPlan_OK_EventArgs
        {
            public SelectPlan_OK_EventArgs(string planName) { this.planName = planName; }
            public string planName { get; private set; }
        }
        public delegate void SelectPlan_OK_EventHandler(object sender, SelectPlan_OK_EventArgs e);
        public static event SelectPlan_Cancel_EventHandler SelectPlan_Cancel_Event;
        public static void RaiseSelectPlan_Cancel_Event(object sender)
        {

            if (SelectPlan_Cancel_Event != null)
                SelectPlan_Cancel_Event(sender, new SelectPlan_Cancel_EventArgs());
        }
        public class SelectPlan_Cancel_EventArgs
        {
            public SelectPlan_Cancel_EventArgs() { }
        }
        public delegate void SelectPlan_Cancel_EventHandler(object sender, SelectPlan_Cancel_EventArgs e);


        public static event EnvironmentChanged_EventHandler EnvironmentChanged_Event;
        public static void RaiseEnvironmentChanged_Event(object sender, string newEnvironmentName)
        {

            if (EnvironmentChanged_Event != null)
                EnvironmentChanged_Event(sender, new EnvironmentChanged_EventArgs(newEnvironmentName));
        }
        public class EnvironmentChanged_EventArgs
        {
            public EnvironmentChanged_EventArgs(string environmentName) { this.environmentName = environmentName; }
            public string environmentName { get; private set; }
        }
        public delegate void EnvironmentChanged_EventHandler(object sender, EnvironmentChanged_EventArgs e);
       
    }
}
