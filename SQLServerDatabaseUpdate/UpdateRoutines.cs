using System;
using System.Collections;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Globalization;
using System.Data.SqlClient;
using System.Configuration;
using System.Xml;
using System.Xml.Serialization;
using System.Reflection;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Runtime.InteropServices;


using MIDRetail.DataCommon;
using MIDRetail.Data;
using MIDRetail.DatabaseUpdate;
using MIDRetail.Business;
using MIDRetail.ForecastComputations;

namespace MIDRetail.DatabaseUpdate
{

	public static class UpdateRoutines
	{
        private static int _maxDegreeOfParallelism = -1;
        private static string _recoveryModel = string.Empty;

        private static SqlCommand sqlCmdForDBMaint = null;
        private static string sqlConnectionString = null;
        private static Queue messageQueue;
        private static Queue processedQueue;
        private static bool upgradingDatabase;
        private static SetMessageDelegate msgDelegate;
        public delegate void SetMessageDelegate(string msg);
        private static SetMessageToInstallerLog installerLogDelegate = null;
        public delegate void SetMessageToInstallerLog(string msg);
        private static IPlanComputationVariables variables;
        private static bool aNewDatabase;

        private static string folderForWindowsDLL = string.Empty;
        private static string folderForDatabaseFiles = string.Empty;

        #region "Processing"

        /// <summary>
        /// Main entry point for either installing a new database or upgrading a database.
        /// </summary>
        public static void ProcessDatabase(Queue aMessageQueue, Queue aProcessedQueue, bool isNewDatabase, bool doUpgradeDatabase, bool doValidateConfiguration, bool IsOneClickUpgrade, bool doLoadLicenseKey, string aConnectionString, string strDatabase, SetMessageDelegate setMessageDelegate, SetMessageToInstallerLog setMessageToInstaller, out bool keepProcessing, out bool bValidConfiguration)
        {
            keepProcessing = true;

            messageQueue = aMessageQueue;
            processedQueue = aProcessedQueue;
            upgradingDatabase = doUpgradeDatabase;
            msgDelegate = setMessageDelegate;
            installerLogDelegate = setMessageToInstaller;
            sqlConnectionString = aConnectionString;
            aNewDatabase = isNewDatabase;

            try
            {
                SetFoldersForProcessing();
                SetRecoveryModelSimple(aMessageQueue, aConnectionString, strDatabase);
                DisableTextConstraints(aMessageQueue, aConnectionString);
                MIDConnectionString.ConnectionString = aConnectionString;

                bValidConfiguration = true;
                if (doValidateConfiguration)
                {
                    object[] customAttributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyConfigurationAttribute), true);
                    string assemblyConfiguration = string.IsNullOrEmpty(((AssemblyConfigurationAttribute)customAttributes[0]).Configuration) ? "Base" : ((AssemblyConfigurationAttribute)customAttributes[0]).Configuration;
                    string databaseConfiguration = UpdateRoutines.GetConfiguration(messageQueue, aConnectionString);
                    if (databaseConfiguration != null &&
                        databaseConfiguration != assemblyConfiguration)
                    {
                        string msg = "WARNING: Installer configuration of " + assemblyConfiguration + " does not match database configuration of " + databaseConfiguration + ".";
                        messageQueue.Enqueue(msg);
                        if (!IsOneClickUpgrade)
                        {
                            if (MessageBox.Show(msg + Environment.NewLine + "Do you want to continue with the upgrade?", "",
                                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                            {
                                bValidConfiguration = false;
                                messageQueue.Enqueue("User cancelled upgrade.");
                            }
                        }
                    }
                }


                if (isNewDatabase || upgradingDatabase)
                {
                    PerformDatabaseMaintenance(out keepProcessing);
                }

                if (keepProcessing)
                {
                    if (doLoadLicenseKey)
                    {
                        keepProcessing = LicenseKeyRoutines.LoadLicenseKey(messageQueue, aConnectionString);
                    }
                }

                // Begin TT#195 MD - JSmith - Add environment authentication
                if (keepProcessing)
                {
                        if (StampDatabase(messageQueue, aConnectionString, strDatabase))
                        {
                            messageQueue.Enqueue("Database stamping was successful");
                        }
                        else
                        {
                            keepProcessing = false;
                            messageQueue.Enqueue("ERROR: Database stamping was not successful");
                        }
                }
                // End TT#195 MD

                //display a nice final message
                if (isNewDatabase || upgradingDatabase)
                {
                    if (keepProcessing == false)
                    {
                        if (upgradingDatabase)
                        {
                            messageQueue.Enqueue("ERROR: Database upgrade failed");
                        }
                        else
                        {
                            messageQueue.Enqueue("ERROR: Database installation failed");
                        }
                    }
                    else
                    {
                        if (upgradingDatabase)
                        {
                            messageQueue.Enqueue("Database upgrade was successful");
                        }
                        else
                        {
                            messageQueue.Enqueue("Database installation was successful");
                        }
                    }
                }

                //bprocessDatabase = true;
            }
            catch (Exception exc)
            {
                // Begin TT#1468 - GRT - Reporting incorrect completion status when error occurs
                messageQueue.Enqueue("ERROR: " + exc.ToString());
                keepProcessing = false;
                throw;
                // End TT#1468 - GRT - Reporting incorrect completion status when error occurs
            }
            finally
            {
                RestoreRecoveryModel(messageQueue, aConnectionString, strDatabase);
                EnableTextConstraints(messageQueue, aConnectionString);
                if (keepProcessing)
                {
                    ProgressBarSetToMaximum();
                }
            }
        }

        /// <summary>
        /// Sends a message to both the status bar and the installer log through their delegates
        /// </summary>
        /// <param name="message">The message to send.</param>
        private static void sendMessage(string message)
        {
            if (msgDelegate != null)
            {
                msgDelegate(message);
            }
            if (installerLogDelegate != null)
            {
                installerLogDelegate(message);
            }
        }
        /// <summary>
        /// Sends a message to just the installer log through its delegate
        /// </summary>
        /// <param name="message"></param>
        private static void sendLogMessage(string message)
        {
            if (installerLogDelegate != null)
            {
                installerLogDelegate(message);
            }
        }

        /// <summary>
        /// Second routine that controls flow when installing or upgrading a database.
        /// </summary>
        private static void PerformDatabaseMaintenance(out bool keepProcessing)
        {
            bool connectionOpen = false;
            try
            {

                
                try  // open connection
                {
                    int _commandTimeout = 30;
                    // Begin MID Track #5151 - JSmith - Timeout during upgrade
                    string sCommandTimeout = MIDConfigurationManager.AppSettings["DatabaseCommandTimeOut"];
                    if (sCommandTimeout != null)
                    {
                        try
                        {
                            _commandTimeout = Convert.ToInt32(sCommandTimeout, CultureInfo.CurrentUICulture);
                        }
                        catch
                        {
                        }
                    }
                    // End MID Track #5151

                    sqlCmdForDBMaint = new SqlCommand();
                    sqlCmdForDBMaint.Connection = new SqlConnection(sqlConnectionString);
                    sqlCmdForDBMaint.Connection.Open();
                    if (_commandTimeout != sqlCmdForDBMaint.CommandTimeout)
                    {
                        sqlCmdForDBMaint.CommandTimeout = _commandTimeout;
                    }
                    connectionOpen = true;
                }
                catch (Exception ex)
                {
                    string message = ex.ToString();
                    messageQueue.Enqueue("UNEXPECTED EXCEPTION: Error encountered during open of database");
                    keepProcessing = false;
                    return;
                }

                

                GetFileGroups(out keepProcessing); // validate file groups

                if (keepProcessing == false)
                {
                    return;
                }


                if (upgradingDatabase)
                {
                    try
                    {
                        string currentVersion = GetCurrentVersion(sqlCmdForDBMaint);
                        processedQueue.Enqueue(currentVersion);
                    }
                    catch (Exception ex)
                    {
                        string message = ex.ToString();
                        messageQueue.Enqueue("UNEXPECTED EXCEPTION: Error reading current version information");
                        keepProcessing = false;
                    }
                }


                //PlanComputationsCollection compCollections = new PlanComputationsCollection();
                //variables = compCollections.GetDefaultComputations().PlanVariables;

                //Initialize the progress bar
                ProgressBarSetMinimum(0);
                int totalCountOfObjectsToDelete = GetCountOfObjectsToDelete();
                int totalCountOfObjectsToProcess = GetCountOfObjectsToProcess();
                int totalCountofGeneratedObjects = 600;  //it is just an estimate + 200 for padding
                ProgressBarSetMaximum(totalCountOfObjectsToDelete + totalCountOfObjectsToProcess + totalCountofGeneratedObjects);

                //always delete old objects, even when creating a new database, just in case the process failed halfway through
                keepProcessing = true;
                DeleteDatabaseObjects(out keepProcessing);



                if (keepProcessing)
                {
                    ProcessDatabaseObjects(out keepProcessing);
                }


         



                if (keepProcessing && upgradingDatabase)
                {
                    //            SetStatusMessage("Loading Computations");
                    if (!LoadRoutines.LoadComputations(messageQueue, sqlConnectionString))
                    {
                        keepProcessing = false;
                        messageQueue.Enqueue("ERROR: Load Computations Failed.");
                    }
                    else
                    {
                        messageQueue.Enqueue("Load Computations Succeeded.");
                    }
                    //            SetStatusMessage("Computations Executed.");
                }

                ProgressBarSetToMaximum();

            }

            catch (Exception ex)
            {
                messageQueue.Enqueue("UNEXPECTED EXCEPTION: " + ex.ToString());
                keepProcessing = false;
            }
            finally
            {
                if (connectionOpen)
                {
                    sqlCmdForDBMaint.Connection.Close();
                }
            }

        }
        private static int GetCountOfObjectsToDelete()
        {
            int countOfObjectsToDelete = 0;
          
            //countOfObjectsToDelete += GetCountOfObjectsForFolder(GetFolderForSQLObject(DatabaseObjectsSQLObjectType.Constraint, false));
            countOfObjectsToDelete += GetCountOfObjectsForFolder(GetFolderForSQLObject(DatabaseObjectsSQLObjectType.FunctionScalar, false));
            countOfObjectsToDelete += GetCountOfObjectsForFolder(GetFolderForSQLObject(DatabaseObjectsSQLObjectType.FunctionTable, false));
            //countOfObjectsToDelete += GetCountOfObjectsForFolder(GetFolderForSQLObject(DatabaseObjectsSQLObjectType.Index, false));
            countOfObjectsToDelete += GetCountOfObjectsForFolder(GetFolderForSQLObject(DatabaseObjectsSQLObjectType.StoredProcedure, false));
            //countOfObjectsToDelete += GetCountOfObjectsForFolder(GetFolderForSQLObject(DatabaseObjectsSQLObjectType.Table, false));
            //countOfObjectsToDelete += GetCountOfObjectsForFolder(GetFolderForSQLObject(DatabaseObjectsSQLObjectType.TableKey, false));
            countOfObjectsToDelete += GetCountOfObjectsForFolder(GetFolderForSQLObject(DatabaseObjectsSQLObjectType.Trigger, false));
            countOfObjectsToDelete += GetCountOfObjectsForFolder(GetFolderForSQLObject(DatabaseObjectsSQLObjectType.Type, false));
            //countOfObjectsToDelete += GetCountOfObjectsForFolder(GetFolderForSQLObject(DatabaseObjectsSQLObjectType.UpgradeVersion, false));
            countOfObjectsToDelete += GetCountOfObjectsForFolder(GetFolderForSQLObject(DatabaseObjectsSQLObjectType.View, false));

            return countOfObjectsToDelete;
        }
        private static int GetCountOfObjectsToProcess()
        {
            int countOfObjectsToProcess = 0;
            if (upgradingDatabase == false)
            {
                countOfObjectsToProcess += GetCountOfObjectsForFolder(GetFolderForSQLObject(DatabaseObjectsSQLObjectType.Constraint, false));
                countOfObjectsToProcess += GetCountOfObjectsForFolder(GetFolderForSQLObject(DatabaseObjectsSQLObjectType.TableKey, false));
                countOfObjectsToProcess += GetCountOfObjectsForFolder(GetFolderForSQLObject(DatabaseObjectsSQLObjectType.Index, false));
            }
            countOfObjectsToProcess += GetCountOfObjectsForFolder(GetFolderForSQLObject(DatabaseObjectsSQLObjectType.Table, false)); //we update tables when upgrading so they count either way
            countOfObjectsToProcess += GetCountOfObjectsForFolder(GetFolderForSQLObject(DatabaseObjectsSQLObjectType.FunctionScalar, false));
            countOfObjectsToProcess += GetCountOfObjectsForFolder(GetFolderForSQLObject(DatabaseObjectsSQLObjectType.FunctionTable, false));
            countOfObjectsToProcess += GetCountOfObjectsForFolder(GetFolderForSQLObject(DatabaseObjectsSQLObjectType.StoredProcedure, false));
            countOfObjectsToProcess += GetCountOfObjectsForFolder(GetFolderForSQLObject(DatabaseObjectsSQLObjectType.Trigger, false));
            countOfObjectsToProcess += GetCountOfObjectsForFolder(GetFolderForSQLObject(DatabaseObjectsSQLObjectType.Type, false));
            countOfObjectsToProcess += GetCountOfObjectsForFolder(GetFolderForSQLObject(DatabaseObjectsSQLObjectType.View, false));
            countOfObjectsToProcess += GetCountOfUpgradeVersionsForFolder(GetFolderForSQLObject(DatabaseObjectsSQLObjectType.UpgradeVersion, false));
            return countOfObjectsToProcess;
        }
        private static int GetCountOfObjectsForFolder(string folder)
        {
            System.IO.DirectoryInfo di = new DirectoryInfo(folder);
            return di.GetFiles("*.SQL").Length;
        }
        private static int GetCountOfUpgradeVersionsForFolder(string folder)
        {
            System.IO.DirectoryInfo di = new DirectoryInfo(folder);
            return di.GetDirectories().Length;
        }
        private static void ProcessDatabaseObjects(out bool keepProcessing)
        {
            try
            {
                keepProcessing = true;
                List<DatabaseObjectsSQLObject> orderedSqlObjectsToCreate = GetOrderedSqlObjectsToProcess();
                foreach (DatabaseObjectsSQLObject so in orderedSqlObjectsToCreate)
                {
                    if (keepProcessing)
                    {
                        if (so.Ignore != "Y")
                        {
                            if (upgradingDatabase && so.Process == DatabaseObjectsSQLObjectProcess.RemoveDefaultLayout)
                            {
                                ProcessRemoveDefaultLayout(out keepProcessing);
                            }
                            //Begin TT#1356-MD -SQL Upgrade - Custom Conversions
                            //else if (so.Process == DatabaseObjectsSQLObjectProcess.PopulateInUse) 
                            //{
                                //PopulateInUse(out keepProcessing);
                            //}
                            else if (so.Process == DatabaseObjectsSQLObjectProcess.ClearCustomConversions)
                            {
                                ClearCustomConversionQueue();
                            }
                            else if (so.Process == DatabaseObjectsSQLObjectProcess.ExecuteDeferredCustomConversions)
                            {
                                CheckCustomConversionQueue(executeDeferredFunctions: true, keepProcessing: out keepProcessing);
                            }
                            //End TT#1356-MD -SQL Upgrade - Custom Conversions
                            else if (so.Process == DatabaseObjectsSQLObjectProcess.LoadCalcsAndVariables)
                            {
                                LoadCalcsAndVariables(out keepProcessing);
                            }
                            else if (upgradingDatabase == false && so.Process == DatabaseObjectsSQLObjectProcess.GenerateTableFiles)
                            {
                                GenerateTableObjects(out keepProcessing);
                            }
                            else if (upgradingDatabase == true && so.Process == DatabaseObjectsSQLObjectProcess.UpdateVariableTables)
                            {
                                UpdateVariableTables(out keepProcessing);
                            }
                            else if (so.Process == DatabaseObjectsSQLObjectProcess.GenerateNonTableFiles)
                            {
                                GenerateNonTableObjects(out keepProcessing);
                            }
                            else if (so.Process == DatabaseObjectsSQLObjectProcess.Single) //Process a single ordered object
                            {
                                if (so.Type == DatabaseObjectsSQLObjectType.UpgradeVersion)
                                {
                                    ProcessUpgradeVersion(so.Name, out keepProcessing);
                                }
                                else if (so.Type == DatabaseObjectsSQLObjectType.Script)
                                {
                                    ProcessScript(so.Name, out keepProcessing);
                                }
                                else
                                {
                                    ProcessDatabaseObject(so.Type, so.Name, Include.ConvertStringToBool(so.IsGenerated), out keepProcessing);
                                }
                            }
                            else if (so.Process == DatabaseObjectsSQLObjectProcess.NonOrdered && Include.ConvertStringToBool(so.IsGenerated) == false)
                            {
                                DirectoryInfo di = new DirectoryInfo(GetFolderForSQLObject(so.Type, Include.ConvertStringToBool(so.IsGenerated)));
                                foreach (FileInfo fi in di.GetFiles("*.SQL"))
                                {
                                    if (keepProcessing)
                                    {
                                        string sqlObjectName = fi.Name.Replace(".SQL", string.Empty);

                                        //Do not process ordered objects
                                        if (orderedSqlObjectsToCreate.Exists(delegate(DatabaseObjectsSQLObject dso) { return dso.Name == sqlObjectName; }) == false)
                                        {
                                            if (so.Type == DatabaseObjectsSQLObjectType.UpgradeVersion)
                                            {
                                                ProcessUpgradeVersion(sqlObjectName, out keepProcessing);
                                            }
                                            else
                                            {
                                                ProcessDatabaseObject(so.Type, sqlObjectName, Include.ConvertStringToBool(so.IsGenerated), out keepProcessing);
                                            }
                                        }
                                    }
                                }
                            }
                            else if (so.Process == DatabaseObjectsSQLObjectProcess.NonOrdered && Include.ConvertStringToBool(so.IsGenerated) == true)
                            {
                                if (genTableList != null)
                                {
                                    foreach (genBase gb in genTableList)
                                    {
                                        if (keepProcessing && gb.tableType == ConvertSQLObjectTypeToGenTableType(so.Type))
                                        {
                                            string sqlObjectName = gb.name;
                                            //Do not process ordered objects
                                            if (orderedSqlObjectsToCreate.Exists(delegate(DatabaseObjectsSQLObject dso) { return dso.Name == sqlObjectName; }) == false)
                                            {
                                                ProcessDatabaseObject(so.Type, sqlObjectName, Include.ConvertStringToBool(so.IsGenerated), out keepProcessing);
                                            }
                                        }
                                    }
                                }
                                if (genNonTableList != null)
                                {
                                    foreach (genBase gb in genNonTableList)
                                    {
                                        if (keepProcessing && gb.genType == ConvertSQLObjectTypeToGenNonTableType(so.Type))
                                        {
                                            string sqlObjectName = gb.name;
                                            //Do not process ordered objects
                                            if (orderedSqlObjectsToCreate.Exists(delegate(DatabaseObjectsSQLObject dso) { return dso.Name == sqlObjectName; }) == false)
                                            {
                                                ProcessDatabaseObject(so.Type, sqlObjectName, Include.ConvertStringToBool(so.IsGenerated), out keepProcessing);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                messageQueue.Enqueue("UNEXPECTED EXCEPTION: " + ex.ToString());
                throw;
            }
        }

        private static void LoadCalcsAndVariables(out bool keepProcessing)
        {
            try
            {
                sendMessage("Loading calcs and variables.");

                PlanComputationsCollection compCollections = new PlanComputationsCollection();
                variables = compCollections.GetDefaultComputations().PlanVariables;

                keepProcessing = true;
                processedQueue.Enqueue("Loaded calcs and variables.");
            }
            catch (Exception ex)
            {
                keepProcessing = false;
                messageQueue.Enqueue("UNEXPECTED EXCEPTION: Loading calcs and variables.  Error: " + ex.ToString());
            }
        }

        private static void ProcessDatabaseObject(DatabaseObjectsSQLObjectType soType, string sqlObjectName, bool isGenerated, out bool keepProcessing)
        {
            try
            {
                string sPath = GetFolderForSQLObject(soType, isGenerated);
                if (upgradingDatabase && soType == DatabaseObjectsSQLObjectType.Table && isTableFound(sqlCmdForDBMaint, sqlObjectName))
                {
                    sendMessage("Updating table " + sqlObjectName);
                    UpdateTable(sqlObjectName, sPath + sqlObjectName + ".SQL", out keepProcessing);
                    ProgressBarIncrementValue(1);
                    if (keepProcessing) processedQueue.Enqueue("Updated table " + sqlObjectName);
                }
                else if (upgradingDatabase && soType == DatabaseObjectsSQLObjectType.TableKey && isPrimaryKeyFound(sqlCmdForDBMaint, sqlObjectName, sPath + sqlObjectName + ".SQL"))
                {
                    keepProcessing = true;
                    return;
                }
                else if (upgradingDatabase && soType == DatabaseObjectsSQLObjectType.Constraint && isConstraintFound(sqlCmdForDBMaint, sqlObjectName))
                {
                    keepProcessing = true;
                    return;
                }
                else if (upgradingDatabase && soType == DatabaseObjectsSQLObjectType.Index && isIndexFound(sqlCmdForDBMaint, sqlObjectName))
                {
                    keepProcessing = true;
                    return;
                }
                else
                {
                    sendMessage("Creating " + soType.ToString() + " " + sqlObjectName);
                    ProcessSQLFile(sPath + sqlObjectName + ".SQL", out keepProcessing);
                    ProgressBarIncrementValue(1);
                    if (keepProcessing) processedQueue.Enqueue("Created " + soType.ToString() + " " + sqlObjectName);
                }
            }
            catch (Exception ex)
            {
                keepProcessing = false;
                messageQueue.Enqueue("UNEXPECTED EXCEPTION: " + ex.ToString());
            }
        }

        private static void DeleteDatabaseObjects(out bool keepProcessing)
        {
            try
            {
                keepProcessing = true;
                List<DatabaseObjectsSQLObject> orderedSqlObjectsToDelete = GetOrderedSqlObjectsToDelete();
                if (upgradingDatabase == false)
                {
                    sendMessage("Checking for existing objects...");
                }
                foreach (DatabaseObjectsSQLObject so in orderedSqlObjectsToDelete)
                {
                    if (keepProcessing && so.Ignore != "Y")
                    {
                        if (so.Process == DatabaseObjectsSQLObjectProcess.Single) //Process a single ordered object
                        {
                            DeleteDatabaseObject(so.Type, so.Name, out keepProcessing);
                        }
                        else if (so.Process == DatabaseObjectsSQLObjectProcess.NonOrdered) //Process all the non-ordered objects
                        {
                            DirectoryInfo di = new DirectoryInfo(GetFolderForSQLObject(so.Type, Include.ConvertStringToBool(so.IsGenerated)));
                            foreach (FileInfo fi in di.GetFiles("*.SQL"))
                            {
                                if (keepProcessing)
                                {
                                    string sqlObjectName = fi.Name.Replace(".SQL", string.Empty);
                                    //Do not process ordered objects
                                    if (orderedSqlObjectsToDelete.Exists(delegate(DatabaseObjectsSQLObject dso) { return dso.Name == sqlObjectName; }) == false)
                                    {
                                        DeleteDatabaseObject(so.Type, sqlObjectName, out keepProcessing);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                messageQueue.Enqueue("UNEXPECTED EXCEPTION: " + ex.ToString());
                throw;
            }
        }
        private static void DeleteDatabaseObject(DatabaseObjectsSQLObjectType soType, string sqlObjectName, out bool keepProcessing)
        {
            try
            {
 
                sqlCmdForDBMaint.Transaction = sqlCmdForDBMaint.Connection.BeginTransaction();
                sqlCmdForDBMaint.CommandText = MakeDropCommandForSQLObject(soType, sqlObjectName);
                int dropCount = (int)sqlCmdForDBMaint.ExecuteScalar();
                sqlCmdForDBMaint.Transaction.Commit();
                keepProcessing = true;
                if (dropCount > 0)
                {
                    sendMessage("Deleting " + soType.ToString() + " " + sqlObjectName);
                    processedQueue.Enqueue("Deleted " + soType.ToString() + " " + sqlObjectName);
                }
                ProgressBarIncrementValue(1);
            }
            catch (Exception ex)
            {
                keepProcessing = false;
                messageQueue.Enqueue("UNEXPECTED EXCEPTION: Error deleting object. Type=" + soType + " Object=" + sqlObjectName + "  Error=" + ex.ToString());
            }
        }
       
        private static void ProcessUpgradeVersion(string upgradeVersion, out bool keepProcessing)
        {
            try
            {
                string sPath = GetFolderForSQLObject(DatabaseObjectsSQLObjectType.UpgradeVersion, false);
                int majorVersion;
                int minorVersion;
                int revision;
                int modification;
                ParseVersionFromUpgradeFile(upgradeVersion, out majorVersion, out minorVersion, out revision, out modification);
                if (!UpgradeApplied(majorVersion, minorVersion, revision, modification))
                {
                    sendMessage("Processing upgrade version " + upgradeVersion);
                    processedQueue.Enqueue("Processing upgrade version " + upgradeVersion);
                    ProcessSQLFile(sPath + upgradeVersion + ".SQL", out keepProcessing);
                    if (keepProcessing)
                    {
                        CheckCustomConversionQueue(executeDeferredFunctions: false, keepProcessing: out keepProcessing); //run any immedidate custom conversion code //TT#1356-MD -jsobek -SQL Upgrade - Custom Conversions
                        UpdateApplicationVersion(majorVersion, minorVersion, revision, modification);  //update the version applied
                        processedQueue.Enqueue("Processed upgrade version " + upgradeVersion);
                    }
                }
                else
                {
                    keepProcessing = true;
                }
            }
            catch (Exception ex)
            {
                keepProcessing = false;
                messageQueue.Enqueue("UNEXPECTED EXCEPTION: Error processing upgrade version. Version=" + upgradeVersion + " Error=" + ex.ToString());
            }
        }

        //Begin TT#1356-MD -jsobek -SQL Upgrade - Custom Conversions
        private static void ClearCustomConversionQueue()
        {
            string command = string.Empty;
            command += " DELETE FROM APPLICATION_DB_CONVERSION_QUEUE" + System.Environment.NewLine;
            
            sqlCmdForDBMaint.CommandText = command;
            try
            {
                sqlCmdForDBMaint.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                messageQueue.Enqueue("UNEXPECTED EXCEPTION: " + ex.ToString());
                throw;
            }
        }
        private static void CheckCustomConversionQueue(bool executeDeferredFunctions, out bool keepProcessing)
        {
            string customConversion = string.Empty;
            try
            {
                DataTable dtCustomConversionsToExecute;
                EnvironmentData envData = new EnvironmentData();
                if (executeDeferredFunctions)
                {
                    dtCustomConversionsToExecute = envData.ReadCustomConversionsForDeferredExecution();
                }
                else
                {
                    dtCustomConversionsToExecute = envData.ReadCustomConversionsForImmediateExecution();
                }
                foreach (DataRow dr in dtCustomConversionsToExecute.Rows)
                {
                    customConversion = (string)dr["CC_FUNCTION_NAME"];
                    sendMessage("Running " + customConversion);

                    switch (customConversion)
                    {
                        case "PopulateInUse": //Must match what is in the upgrade version SQL file
                            sendMessage("Populating In Use.");
                            PopulateInUse p = new PopulateInUse();
                            p.Execute(sqlConnectionString, messageQueue, processedQueue);
                            processedQueue.Enqueue("Populated In Use.");

                            break;
                        case "RebuildHeaderFilterSP": //Must match what is in the upgrade version SQL file
                            sendMessage("Rebuilding filter stored procedures.");
                            RebuildHeaderFilterSP rebuildSP = new RebuildHeaderFilterSP();
                            rebuildSP.Execute();
                            processedQueue.Enqueue("Rebuilt filter stored procedures.");

                            break;
                        case "InsertSampleFilters": //Must match what is in the upgrade version SQL file //TT#1486-MD -jsobek -Store Filters - Insert samples
                            sendMessage("Inserting sample filters.");
                            InsertSampleFilters insertSamples = new InsertSampleFilters();
                            insertSamples.Execute();
                            processedQueue.Enqueue("Inserted sample filters.");

                            break;
                        case "InsertAllStoresAttribute": //Must match what is in the upgrade version SQL file //TT#1517-MD -jsobek -Store Service Optimization
                             sendMessage("Inserting All Stores attribute.");
                             InsertAllStoresAttribute insertAllStoresAttribute = new InsertAllStoresAttribute();
                             insertAllStoresAttribute.Execute();
                            processedQueue.Enqueue("Inserted All Stores attribute.");

                            break;
                        case "RefreshAttributeFilters": //Must match what is in the upgrade version SQL file //TT#1517-MD -jsobek -Store Service Optimization
                            if (!aNewDatabase)
                            {
                                sendMessage("Refreshing Attribute Filters.");
                                RefreshAttributeFilters refreshAttributeFilters = new RefreshAttributeFilters();
                                refreshAttributeFilters.Execute();
                                processedQueue.Enqueue("Refreshed Attribute Filters.");
                            }
                            break;
                        default:
                            sendMessage("Unknown custom conversion: " + customConversion);
                            processedQueue.Enqueue("Unknown custom conversion: " + customConversion);
                            break;
                    }
  
                  
                  

                    envData.RemoveCustomConversionFromQueue(customConversion);
                }
                keepProcessing = true;               
            }
            catch (Exception ex)
            {
                keepProcessing = false;
                messageQueue.Enqueue("UNEXPECTED EXCEPTION: Error processing custom conversion code. Custom Conversion=" + customConversion + " Error=" + ex.ToString());
            }
        }
        //End TT#1356-MD -jsobek -SQL Upgrade - Custom Conversions

        private static void SetFoldersForProcessing()
        {
            string appStartupPath = Application.StartupPath;
            //string appExecutablePath = Application.ExecutablePath;

            #if (DEBUG)
            folderForWindowsDLL = Directory.GetParent(Directory.GetParent(Directory.GetParent(appStartupPath).ToString().Trim()).ToString().Trim()) + @"\Windows\bin\Debug\" + "MIDRetail.Windows.dll"; ;
            #else
                folderForWindowsDLL = Directory.GetParent(appStartupPath).ToString().Trim() + @"\Installer\Install Files\Client\MIDRetail.Windows.dll";
            #endif


            #if (DEBUG)
                folderForDatabaseFiles = Directory.GetParent(Directory.GetParent(Directory.GetParent(appStartupPath).ToString().Trim()).ToString().Trim()) + @"\DatabaseDefinition";
            #else
                folderForDatabaseFiles = Directory.GetParent(appStartupPath).ToString().Trim() + @"\Database";
            #endif

            
        }

        private static void ParseVersionFromUpgradeFile(string fileName, out int majorVersion, out int minorVersion, out int revision, out int modification)
        {
            try
            {
                string[] nodes = fileName.Replace("ver", string.Empty).Split('.');
                majorVersion = Convert.ToInt32(nodes[0]);
                minorVersion = Convert.ToInt32(nodes[1]);
                revision = Convert.ToInt32(nodes[2]);
                modification = Convert.ToInt32(nodes[3]);
            }
			catch (Exception ex)
			{
				string message = ex.Message;
				throw;
			}
        }
		private static void UpdateApplicationVersion(int aMajorVersion, int aMinorVersion, int aRevision, int aModification)
		{
		
				string command = string.Empty;
				command += "begin" + System.Environment.NewLine;
				command += " " + "DECLARE @myCount int, @majorVersion int, @minorVersion int, @revision int, @modification int " + System.Environment.NewLine;
				command += " " + "select @majorVersion = " + aMajorVersion.ToString() + System.Environment.NewLine;
				command += " " + "select @minorVersion = " + aMinorVersion.ToString() + System.Environment.NewLine;
				command += " " + "select @revision  = " + aRevision.ToString() + System.Environment.NewLine;
				command += " " + "select @modification  = " + aModification.ToString() + System.Environment.NewLine;
				command += " " + "select @myCount = count(*) from APPLICATION_VERSION " + System.Environment.NewLine;
				command += " " + "where MAJOR_VERSION = @majorVersion" + System.Environment.NewLine;
				command += " " + "and MINOR_VERSION = @minorVersion" + System.Environment.NewLine;
				command += " " + "and REVISION = @revision" + System.Environment.NewLine;
				command += " " + "and MODIFICATION = @modification" + System.Environment.NewLine;
				command += " " + "if @myCount = 0" + System.Environment.NewLine;
				command += " " + "begin" + System.Environment.NewLine;
				command += " " + "insert into APPLICATION_VERSION(MAJOR_VERSION,MINOR_VERSION,REVISION,MODIFICATION,WHEN_RUN) " + System.Environment.NewLine;
				command += " " + "values (@majorVersion,@minorVersion,@revision,@modification,GETDATE())" + System.Environment.NewLine;
				command += " " + "end" + System.Environment.NewLine;
				command += " " + "else" + System.Environment.NewLine;
				command += " " + "begin" + System.Environment.NewLine;
				command += " " + "update APPLICATION_VERSION" + System.Environment.NewLine;
				command += " " + "set WHEN_RUN = GETDATE()" + System.Environment.NewLine;
				command += " " + "where MAJOR_VERSION = @majorVersion" + System.Environment.NewLine;
				command += " " + "and MINOR_VERSION = @minorVersion" + System.Environment.NewLine;
				command += " " + "and REVISION = @revision" + System.Environment.NewLine;
				command += " " + "and MODIFICATION = @modification" + System.Environment.NewLine;
				command += " " + "end" + System.Environment.NewLine;
				command += " " + "end" + System.Environment.NewLine;

				sqlCmdForDBMaint.CommandText = command;
				try
				{
                    sqlCmdForDBMaint.ExecuteNonQuery();
				}
				catch (Exception ex)
				{
					messageQueue.Enqueue("UNEXPECTED EXCEPTION: " + ex.ToString());
                    throw;
				}
		
		}
		private static bool UpgradeApplied(int aMajorVersion, int aMinorVersion, int aRevision, int aModification)
		{
            //try
            //{
				bool upgradeApplied = false;
				string command = string.Empty;
				command += "begin" + System.Environment.NewLine;
				command += " " + "DECLARE @myCount int, @majorVersion int, @minorVersion int, @revision int, @modification int " + System.Environment.NewLine;
				command += " " + "select @majorVersion = " + aMajorVersion.ToString() + System.Environment.NewLine;
				command += " " + "select @minorVersion = " + aMinorVersion.ToString() + System.Environment.NewLine;
				command += " " + "select @revision  = " + aRevision.ToString() + System.Environment.NewLine;
				command += " " + "select @modification  = " + aModification.ToString() + System.Environment.NewLine;
				command += " " + "select count(*) as MyCount from APPLICATION_VERSION " + System.Environment.NewLine;
				command += " " + "where MAJOR_VERSION = @majorVersion" + System.Environment.NewLine;
				command += " " + "and MINOR_VERSION = @minorVersion" + System.Environment.NewLine;
				command += " " + "and REVISION = @revision" + System.Environment.NewLine;
				command += " " + "and MODIFICATION = @modification" + System.Environment.NewLine;
				command += "end" + System.Environment.NewLine;

				sqlCmdForDBMaint.CommandText = command;
                SqlDataReader myReader = null;
				try
				{
					myReader = sqlCmdForDBMaint.ExecuteReader();

					if (myReader.Read())
					{
						int recCount = (int) myReader["MyCount"];
						if (recCount > 0)
						{
							upgradeApplied = true;
						}
					}
	
					return upgradeApplied;
				}
				catch (Exception ex)
				{
					messageQueue.Enqueue("UNEXPECTED EXCEPTION: Error reading applied upgrade versions. Error=" + ex.ToString());
                    throw;
				}
                finally
                {
                    if (myReader != null)
                    {
                        myReader.Close();
                    }
                }
            //}
            //catch
            //{
            //    return true; // return true so command will not be applied
            //}
		}
        private static void ProcessScript(string scriptName, out bool keepProcessing)
        {
            try
            {
                string sPath = GetFolderForSQLObject(DatabaseObjectsSQLObjectType.Script, false);
                sendMessage("Processing script " + scriptName);
                ProcessSQLFile(sPath + scriptName + ".SQL", out keepProcessing);
                if (keepProcessing)
                {
                    processedQueue.Enqueue("Processed script " + scriptName);
                    ProgressBarIncrementValue(25);  //count each script as 25
                }

            }
            catch (Exception ex)
            {
                keepProcessing = false;
                messageQueue.Enqueue("UNEXPECTED EXCEPTION: Error processing script. Script=" + scriptName + " Error=" + ex.ToString());
            }
        }
        private static void ProcessSQLFile(string fullPath, out bool keepProcessing)
        {
            StreamReader reader = null;
            try
            {

                sqlCmdForDBMaint.Transaction = sqlCmdForDBMaint.Connection.BeginTransaction();
                //sqlCmdForDBMaint.CommandText = cmdText;
                //sqlCmdForDBMaint.ExecuteNonQuery();
                //process schema changes
                reader = new StreamReader(fullPath);
                int lineCount = 0;

                string lineWithWhitespace; //TT#730-MD -jsobek -Preserve horizontal tabs and whitespace during database upgrade

                string line;
                string command = string.Empty;
                // process script
                while ((line = reader.ReadLine()) != null)
                {
                    lineCount++;
                    lineWithWhitespace = line;
                    //Remove sql developer comments unless we are upgrading the database in debug mode
                    //developer comments should begin with --DV
#if (DEBUG)

#else
				        if (line.Trim().ToUpper().StartsWith("--DV") == true)
                        {
                            continue;
                        }
#endif
                    //End TT#730-MD -jsobek -Preserve horizontal tabs and whitespace during database upgrade
                    line = line.Trim();

                    //preserve blank lines
                    if (line.Length == 0)
                    {
                        command += System.Environment.NewLine;
                    }

                    if (line.Length == 0 ||
                        line.StartsWith("/*COMMENT") ||
                        line.ToUpper().StartsWith("/*RELEASE *.*.*") ||
                        line.ToUpper().StartsWith("/*RELEASE *.*.*.*") ||
                        line == "use master" ||
                        line.ToLower().StartsWith("create database") ||
                        line.ToLower().StartsWith(@"use "))
                    {
                        continue;
                    }
                    else if (line.ToUpper() == "GO")
                    {
                        FilterCommand(ref command);
                        sqlCmdForDBMaint.CommandText = command;
                        sqlCmdForDBMaint.ExecuteNonQuery();
                        command = string.Empty;
                    }
                    else
                    {
                        command += lineWithWhitespace + System.Environment.NewLine;
                    }

                }

                // process command if anything in buffer
                //if (processCommand)
                //{
                //    ProcessCommand(aMessageQueue, aProcessedQueue, ref currentHeader, aFileName, lineCount, aSqlCommand, command, sqlMajorVersion, sqlMinorVersion, sqlRevision, sqlModification, SettingNewInstallVersionNumbers, true, aIgnoreErrors);
                //    processCommand = false;
                //}
                if (command != string.Empty)
                {
                    FilterCommand(ref command);
                    sqlCmdForDBMaint.CommandText = command;
                    sqlCmdForDBMaint.ExecuteNonQuery();
                }



                sqlCmdForDBMaint.Transaction.Commit();
                keepProcessing = true;
            }
            catch (Exception ex)
            {
                keepProcessing = false;
                messageQueue.Enqueue("UNEXPECTED EXCEPTION: Error processing SQL File. Path=" + fullPath + " Error=" + ex.ToString());
                if (sqlCmdForDBMaint.Transaction != null)
                {
                    sqlCmdForDBMaint.Transaction.Rollback();
                }

            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
        }
        private static void FilterCommand(ref string cmd)
        {
            if (cmd.Contains("ON 'ALLOCATION'"))
            {
                cmd = cmd.Replace("ON 'ALLOCATION'", "ON '" + allocationFileGroup + "'");
            }
            if (cmd.Contains("ON [ALLOCATION]"))
            {
                cmd = cmd.Replace("ON [ALLOCATION]", "ON '" + allocationFileGroup + "'");
            }
            if (cmd.Contains("ON 'FORECAST'"))
            {
                cmd = cmd.Replace("ON 'FORECAST'", "ON '" + forecastFileGroup + "'");
            }
            if (cmd.Contains("ON 'HISTORY'"))
            {
                cmd = cmd.Replace("ON 'HISTORY'", "ON '" + historyFileGroup + "'");
            }
            if (cmd.Contains("ON 'DAILYHISTORY'"))
            {
                cmd = cmd.Replace("ON 'DAILYHISTORY'", "ON '" + dailyHistoryFileGroup + "'");
            }

            if (cmd.Contains("ON 'AUDIT'"))
            {
                cmd = cmd.Replace("ON 'AUDIT'", "ON '" + auditFileGroup + "'");
            }
            if (cmd.Contains("ON [AUDIT]"))
            {
                cmd = cmd.Replace("ON [AUDIT]", "ON '" + auditFileGroup + "'");
            }

            //string[] sSplit = cmd.Split(System.Environment.NewLine.ToCharArray());

            //string newCmd = string.Empty;

            //foreach(string s in sSplit)
            //{
            //    if (s.Trim().ToUpper() != "GO")
            //    {
            //        newCmd += s + System.Environment.NewLine;
            //    }
            //    if (s.Trim().ToUpper() != "ON 'ALLOCATION'")
            //    {
            //        newCmd += newCmd.Replace("ON 'ALLOCATION'", "ON '" + allocationFileGroup + "'") + System.Environment.NewLine;
            //    }
            //    if (s.Trim().ToUpper() != "ON [ALLOCATION]")
            //    {
            //        newCmd += newCmd.Replace("ON [ALLOCATION]", "ON [" + allocationFileGroup + "]") + System.Environment.NewLine;
            //    }
            //    //if (s.Trim().ToUpper() != "ON 'FORECAST'")
            //    //{
            //    //    newCmd += newCmd.Replace("ON 'FORECAST'", "ON '" + forecastFileGroup + "'") + System.Environment.NewLine;
            //    //}
            //    //if (s.Trim().ToUpper() != "ON 'HISTORY'")
            //    //{
            //    //    newCmd += newCmd.Replace("ON 'HISTORY'", "ON '" + historyFileGroup + "'") + System.Environment.NewLine;
            //    //}
            //    //if (s.Trim().ToUpper() != "ON 'DAILYHISTORY'")
            //    //{
            //    //    newCmd += newCmd.Replace("ON 'DAILYHISTORY'", "ON '" + dailyHistoryFileGroup + "'") + System.Environment.NewLine;
            //    //}
            //    if (s.Trim().ToUpper() != "ON 'AUDIT'")
            //    {
            //        newCmd += newCmd.Replace("ON 'AUDIT'", "ON '" + auditFileGroup + "'") + System.Environment.NewLine;
            //    }
            //    if (s.Trim().ToUpper() != "ON [AUDIT]")
            //    {
            //        newCmd += newCmd.Replace("ON [AUDIT]", "ON [" + auditFileGroup + "]") + System.Environment.NewLine;
            //    }
            //}

            //return newCmd;
        }
        private static List<DatabaseObjectsSQLObject> GetOrderedSqlObjectsToDelete()
        {
            List<DatabaseObjectsSQLObject> orderedSqlObjectsToDelete = new List<DatabaseObjectsSQLObject>();

            DatabaseObjects dosDeleteSeq;
            TextReader trDeleteSeq = null;
            XmlSerializer xmlSerializerDeleteSeq;
            string fileLocation_DeleteSeq;

            fileLocation_DeleteSeq = folderForDatabaseFiles + @"\SequenceForDeleting.xml"; // +System.Configuration.ConfigurationManager.AppSettings["DeleteSequenceFile"];
            xmlSerializerDeleteSeq = new XmlSerializer(typeof(DatabaseObjects)); // Create a Serializer
            trDeleteSeq = new StreamReader(fileLocation_DeleteSeq);					  // Load the Xml File
            dosDeleteSeq = (DatabaseObjects)xmlSerializerDeleteSeq.Deserialize(trDeleteSeq);

            foreach (DatabaseObjectsSQLObject dso in dosDeleteSeq.SQLObject)
            {
                orderedSqlObjectsToDelete.Add(dso);
            }

            return orderedSqlObjectsToDelete;
        }
        private static List<DatabaseObjectsSQLObject> GetOrderedSqlObjectsToProcess()
        {
            List<DatabaseObjectsSQLObject> orderedSqlObjectsToProcess = new List<DatabaseObjectsSQLObject>();

            DatabaseObjects dosLoadSeq;
            TextReader trLoadSeq = null;
            XmlSerializer xmlSerializerLoadSeq;
            string fileLocation_LoadSeq;

            if (upgradingDatabase)
                fileLocation_LoadSeq = folderForDatabaseFiles + @"\SequenceForUpgradeDB.xml"; //+System.Configuration.ConfigurationManager.AppSettings["LoadSequenceFile"];
            else
            {
                fileLocation_LoadSeq = folderForDatabaseFiles + @"\SequenceForNewDB.xml"; //+System.Configuration.ConfigurationManager.AppSettings["LoadSequenceFile"];
            }

            xmlSerializerLoadSeq = new XmlSerializer(typeof(DatabaseObjects)); // Create a Serializer
            trLoadSeq = new StreamReader(fileLocation_LoadSeq);					  // Load the Xml File
            dosLoadSeq = (DatabaseObjects)xmlSerializerLoadSeq.Deserialize(trLoadSeq);

            foreach (DatabaseObjectsSQLObject dso in dosLoadSeq.SQLObject)
            {
                orderedSqlObjectsToProcess.Add(dso);
            }

            return orderedSqlObjectsToProcess;
        }
     
        private static string GetFolderForSQLObject(DatabaseObjectsSQLObjectType sqlObjectType, bool isGenerated)
        {
            try
            {
                string rootFolder = folderForDatabaseFiles;

                if (isGenerated && (sqlObjectType == DatabaseObjectsSQLObjectType.Table || sqlObjectType == DatabaseObjectsSQLObjectType.TableKey || sqlObjectType == DatabaseObjectsSQLObjectType.Constraint || sqlObjectType == DatabaseObjectsSQLObjectType.Index || sqlObjectType == DatabaseObjectsSQLObjectType.Trigger))
                {
                    return rootFolder + @"\" + Include.SQL_FOLDER_GENERATED_TABLE_FILES + @"\";
                }
                else if (isGenerated && (sqlObjectType == DatabaseObjectsSQLObjectType.Type || sqlObjectType == DatabaseObjectsSQLObjectType.Drop || sqlObjectType == DatabaseObjectsSQLObjectType.FunctionScalar || sqlObjectType == DatabaseObjectsSQLObjectType.FunctionTable || sqlObjectType == DatabaseObjectsSQLObjectType.StoredProcedure || sqlObjectType == DatabaseObjectsSQLObjectType.View))
                {
                    return rootFolder + @"\" + Include.SQL_FOLDER_GENERATED_NONTABLE_FILES + @"\";
                }
                else if (sqlObjectType == DatabaseObjectsSQLObjectType.UpgradeVersion)
                {
                    return rootFolder + @"\" + Include.SQL_FOLDER_UPGRADE_VERSIONS + @"\";
                }
                else if (sqlObjectType == DatabaseObjectsSQLObjectType.Constraint)
                {
                    return rootFolder + @"\" + Include.SQL_FOLDER_CONSTRAINTS + @"\";
                }
                else if (sqlObjectType == DatabaseObjectsSQLObjectType.FunctionScalar)
                {
                    return rootFolder + @"\" + Include.SQL_FOLDER_SCALAR_FUNCTIONS + @"\";
                }
                else if (sqlObjectType == DatabaseObjectsSQLObjectType.FunctionTable)
                {
                    return rootFolder + @"\" + Include.SQL_FOLDER_TABLE_FUNCTIONS + @"\";
                }
                else if (sqlObjectType == DatabaseObjectsSQLObjectType.Index)
                {
                    return rootFolder + @"\" + Include.SQL_FOLDER_INDEXES + @"\";
                }
                else if (sqlObjectType == DatabaseObjectsSQLObjectType.Script)
                {
                    return rootFolder + @"\" + Include.SQL_FOLDER_SCRIPTS + @"\";
                }
                else if (sqlObjectType == DatabaseObjectsSQLObjectType.StoredProcedure)
                {
                    return rootFolder + @"\" + Include.SQL_FOLDER_STORED_PROCEDURES + @"\";
                }
                else if (sqlObjectType == DatabaseObjectsSQLObjectType.Table)
                {
                    return rootFolder + @"\" + Include.SQL_FOLDER_TABLES + @"\";
                }
                else if (sqlObjectType == DatabaseObjectsSQLObjectType.TableKey)
                {
                    return rootFolder + @"\" + Include.SQL_FOLDER_TABLE_KEYS + @"\";
                }
                else if (sqlObjectType == DatabaseObjectsSQLObjectType.Trigger)
                {
                    return rootFolder + @"\" + Include.SQL_FOLDER_TRIGGERS + @"\";
                }
                else if (sqlObjectType == DatabaseObjectsSQLObjectType.Type)
                {
                    return rootFolder + @"\" + Include.SQL_FOLDER_TYPES + @"\";
                }
                else if (sqlObjectType == DatabaseObjectsSQLObjectType.View)
                {
                    return rootFolder + @"\" + Include.SQL_FOLDER_VIEWS + @"\";
                }
                else
                {
                    return "Unknown folder";
                }


            }
            catch (Exception ex)
            {
                messageQueue.Enqueue("UNEXPECTED EXCEPTION: " + ex.ToString());
                throw;
            }
        }

        private static string MakeDropCommandForSQLObject(DatabaseObjectsSQLObjectType sqlObjectType, string objectName)
        {
            try
            {

                string cmdText = string.Empty;


                if (sqlObjectType == DatabaseObjectsSQLObjectType.Constraint)
                {
                    string errMsg = "UNEXPECTED EXCEPTION: Attempted to drop constraint.  Not supported.";
                    messageQueue.Enqueue(errMsg);
                    throw new Exception(errMsg);
                }
                else if (sqlObjectType == DatabaseObjectsSQLObjectType.FunctionScalar)
                {
                    cmdText = "IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[" + objectName + "]') AND OBJECTPROPERTY(id, N'IsScalarFunction') = 1) " + System.Environment.NewLine;
                    cmdText += "BEGIN" + System.Environment.NewLine;
                    cmdText += "   DROP FUNCTION [dbo].[" + objectName + "] " + System.Environment.NewLine;
                    cmdText += "   SELECT 1 AS DropCount " + System.Environment.NewLine;
                    cmdText += "END" + System.Environment.NewLine;
                    cmdText += "ELSE" + System.Environment.NewLine;
                    cmdText += "BEGIN" + System.Environment.NewLine;
                    cmdText += "   SELECT 0 AS DropCount " + System.Environment.NewLine;
                    cmdText += "END" + System.Environment.NewLine;
                }
                else if (sqlObjectType == DatabaseObjectsSQLObjectType.FunctionTable)
                {
                    cmdText = "IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[" + objectName + "]') AND OBJECTPROPERTY(id, N'IsTableFunction') = 1) " + System.Environment.NewLine;
                    cmdText += "BEGIN" + System.Environment.NewLine;
                    cmdText += "   DROP FUNCTION [dbo].[" + objectName + "] " + System.Environment.NewLine;
                    cmdText += "   SELECT 1 AS DropCount " + System.Environment.NewLine;
                    cmdText += "END" + System.Environment.NewLine;
                    cmdText += "ELSE" + System.Environment.NewLine;
                    cmdText += "BEGIN" + System.Environment.NewLine;
                    cmdText += "   SELECT 0 AS DropCount " + System.Environment.NewLine;
                    cmdText += "END" + System.Environment.NewLine;
                }
                else if (sqlObjectType == DatabaseObjectsSQLObjectType.Index)
                {
                    string errMsg = "UNEXPECTED EXCEPTION: Attempted to drop index.  Not supported.";
                    messageQueue.Enqueue(errMsg);
                    throw new Exception(errMsg);
                }
                else if (sqlObjectType == DatabaseObjectsSQLObjectType.StoredProcedure)
                {
                    cmdText = "IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[" + objectName + "]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1) " + System.Environment.NewLine;
                    cmdText += "BEGIN" + System.Environment.NewLine;
                    cmdText += "   DROP PROCEDURE [dbo].[" + objectName + "] " + System.Environment.NewLine;
                    cmdText += "   SELECT 1 AS DropCount " + System.Environment.NewLine;
                    cmdText += "END" + System.Environment.NewLine;
                    cmdText += "ELSE" + System.Environment.NewLine;
                    cmdText += "BEGIN" + System.Environment.NewLine;
                    cmdText += "   SELECT 0 AS DropCount " + System.Environment.NewLine;
                    cmdText += "END" + System.Environment.NewLine;
                }
                else if (sqlObjectType == DatabaseObjectsSQLObjectType.Table)
                {
                    cmdText = "IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[" + objectName + "]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1) " + System.Environment.NewLine;
                    cmdText += "BEGIN" + System.Environment.NewLine;
                    cmdText += "   DROP TABLE [dbo].[" + objectName + "] " + System.Environment.NewLine;
                    cmdText += "   SELECT 1 AS DropCount " + System.Environment.NewLine;
                    cmdText += "END" + System.Environment.NewLine;
                    cmdText += "ELSE" + System.Environment.NewLine;
                    cmdText += "BEGIN" + System.Environment.NewLine;
                    cmdText += "   SELECT 0 AS DropCount " + System.Environment.NewLine;
                    cmdText += "END" + System.Environment.NewLine;
                }
                else if (sqlObjectType == DatabaseObjectsSQLObjectType.Trigger)
                {
                    cmdText = "IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[" + objectName + "]') AND OBJECTPROPERTY(id, N'IsTrigger') = 1) " + System.Environment.NewLine;
                    cmdText += "BEGIN" + System.Environment.NewLine;
                    cmdText += "   DROP TRIGGER [dbo].[" + objectName + "] " + System.Environment.NewLine;
                    cmdText += "   SELECT 1 AS DropCount " + System.Environment.NewLine;
                    cmdText += "END" + System.Environment.NewLine;
                    cmdText += "ELSE" + System.Environment.NewLine;
                    cmdText += "BEGIN" + System.Environment.NewLine;
                    cmdText += "   SELECT 0 AS DropCount " + System.Environment.NewLine;
                    cmdText += "END" + System.Environment.NewLine;
                }
                else if (sqlObjectType == DatabaseObjectsSQLObjectType.Type)
                {
                    cmdText = "IF EXISTS (SELECT * FROM sys.types WHERE is_table_type = 1 AND name = '" + objectName + "') " + System.Environment.NewLine;
                    cmdText += "BEGIN" + System.Environment.NewLine;
                    cmdText += "   DROP TYPE [dbo].[" + objectName + "] " + System.Environment.NewLine;
                    cmdText += "   SELECT 1 AS DropCount " + System.Environment.NewLine;
                    cmdText += "END" + System.Environment.NewLine;
                    cmdText += "ELSE" + System.Environment.NewLine;
                    cmdText += "BEGIN" + System.Environment.NewLine;
                    cmdText += "   SELECT 0 AS DropCount "+ System.Environment.NewLine;
                    cmdText += "END" + System.Environment.NewLine;
                }
                else if (sqlObjectType == DatabaseObjectsSQLObjectType.View)
                {
                    cmdText = "IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[" + objectName + "]') AND OBJECTPROPERTY(id, N'IsView') = 1) " + System.Environment.NewLine;
                    cmdText += "BEGIN" + System.Environment.NewLine;
                    cmdText += "   DROP VIEW [dbo].[" + objectName + "] " + System.Environment.NewLine;
                    cmdText += "   SELECT 1 AS DropCount " + System.Environment.NewLine;
                    cmdText += "END" + System.Environment.NewLine;
                    cmdText += "ELSE" + System.Environment.NewLine;
                    cmdText += "BEGIN" + System.Environment.NewLine;
                    cmdText += "   SELECT 0 AS DropCount " + System.Environment.NewLine;
                    cmdText += "END" + System.Environment.NewLine;
                }
                else
                {
                    string errMsg = "UNEXPECTED EXCEPTION: Unknown Type when dropping database sql objects.";
                    messageQueue.Enqueue(errMsg);
                    throw new Exception(errMsg);
                }

                return cmdText;
            }
            catch (Exception ex)
            {
                messageQueue.Enqueue("UNEXPECTED EXCEPTION: " + ex.ToString());
                throw;
            }
        }


        private static void UpdateVariableTables(out bool keepProcessing)
        {
            keepProcessing = true;
            if (!LoadRoutines.UpdateTables(messageQueue, sqlConnectionString))
            {
                keepProcessing = false;
                string msg = "ERROR: Update variable tables failed.";
                messageQueue.Enqueue(msg);
                sendMessage(msg);
            }
            else
            {
                sendMessage("Update variable tables succeeded.");
            }
        }

        private static void ProcessRemoveDefaultLayout(out bool keepProcessing)
        {
            try
            {
                sendMessage("Deleting default layout.");

                sqlCmdForDBMaint.Transaction = sqlCmdForDBMaint.Connection.BeginTransaction();
                sqlCmdForDBMaint.CommandText = "delete from INFRAGISTICS_LAYOUTS where LAYOUT_ID = 1";
                sqlCmdForDBMaint.ExecuteNonQuery();
                sqlCmdForDBMaint.Transaction.Commit();

                keepProcessing = true;
                processedQueue.Enqueue("Deleted default layout.");
            }
            catch (Exception ex)
            {
                keepProcessing = false;
                messageQueue.Enqueue("UNEXPECTED EXCEPTION: Error deleting default layout.  Error: " + ex.ToString());
            }
        }
        //private static void PopulateInUse(out bool keepProcessing)
        //{
        //    try
        //    {
        //        sendMessage("Populating In Use.");

        //        PopulateInUse p = new PopulateInUse();
        //        p.Execute(sqlConnectionString, messageQueue, processedQueue);

        //        keepProcessing = true;
        //        processedQueue.Enqueue("Populated In Use.");
        //    }
        //    catch (Exception ex)
        //    {
        //        keepProcessing = false;
        //        messageQueue.Enqueue("UNEXPECTED EXCEPTION: Error populating In Use.  Error: " + ex.ToString());
        //    }
        //}
    

        private static string GetCurrentVersion(SqlCommand sqlCommand)
        {
            DateTime dbDate = DateTime.MinValue;
            DataTable dt;
            SqlDataAdapter sda;
            DataRow dr;
            int majorVersion = 0;
            int minorVersion = 0;
            int revision = 0;
            int modification = 0;
            sqlCommand.CommandType = CommandType.Text;
            sqlCommand.CommandText = "select * from APPLICATION_VERSION order by MAJOR_VERSION, MINOR_VERSION, REVISION, MODIFICATION";
            dt = MIDEnvironment.CreateDataTable("Query Results");
            sda = new SqlDataAdapter(sqlCommand);
            sda.Fill(dt);

            dr = dt.Rows[dt.Rows.Count - 1];
            majorVersion = Convert.ToInt32(dr["MAJOR_VERSION"], CultureInfo.CurrentUICulture);
            minorVersion = Convert.ToInt32(dr["MINOR_VERSION"], CultureInfo.CurrentUICulture);
            revision = Convert.ToInt32(dr["REVISION"], CultureInfo.CurrentUICulture);
            modification = Convert.ToInt32(dr["MODIFICATION"], CultureInfo.CurrentUICulture);
            dbDate = Convert.ToDateTime(dr["WHEN_RUN"], CultureInfo.CurrentUICulture);

            string currentVersion = "Current version=" + majorVersion.ToString() + "."
                + minorVersion.ToString() + "." + revision.ToString() + "."
                + modification.ToString() + " run on " + dbDate.ToString();

            return currentVersion;
        }

        
        private static string GetConfiguration(Queue aMessageQueue, string aConnString)
        {
            bool connectionOpen = false;
            DataTable dt;
            SqlDataAdapter sda;
            SqlCommand sqlCommand = null;
            string configuration = null;

            try
            {
                // open connection

                try
                {
                    //TT#1130 - Database Utility  does not always connect across the network - apicchetti - 2/10/2011
                    //string connectionString = "server=" + aServer + ";database=" + aDatabase + ";uid=" + aUser + ";pwd=" + aPassword + ";";
                    string connectionString = aConnString;
                    //TT#1130 - Database Utility  does not always connect across the network - apicchetti - 2/10/2011

                    sqlCommand = new SqlCommand();
                    sqlCommand.Connection = new SqlConnection(connectionString);
                    sqlCommand.Connection.Open();
                    connectionOpen = true;
                }
                catch (Exception ex)
                {
                    string message = ex.ToString();
                    aMessageQueue.Enqueue("UNEXPECTED EXCEPTION: Error encountered during open of database");
                    throw;
                }

                sqlCommand.CommandType = CommandType.Text;
                sqlCommand.CommandText = "if exists (select * from INFORMATION_SCHEMA.TABLES where TABLE_SCHEMA = 'dbo' and TABLE_NAME = 'APPLICATION_UPGRADE_HISTORY')"
                + " select top 1 UPGRADE_CONFIGURATION from APPLICATION_UPGRADE_HISTORY order by UPGRADE_RID desc";
                dt = MIDEnvironment.CreateDataTable("Configuration");
                sda = new SqlDataAdapter(sqlCommand);
                sda.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    configuration = Convert.ToString(dt.Rows[0]["UPGRADE_CONFIGURATION"]);
                }
            }
            catch (Exception ex)
            {
                aMessageQueue.Enqueue("UNEXPECTED EXCEPTION: " + ex.ToString());
            }
            finally
            {
                if (connectionOpen)
                {
                    sqlCommand.Connection.Close();
                }
            }

            return configuration;
        }

        /// <summary>
        /// Adds new columns to an existing table
        /// </summary>
        /// <param name="aTableName"></param>
        /// <param name="aFileName"></param>
        /// <param name="keepProcessing"></param>
        private static void UpdateTable(string aTableName, string aFileName, out bool keepProcessing)
        {
            DataTable dtTables;
            string SQLCommand = null;
            SqlDataAdapter sda;
            Hashtable htColumns = new Hashtable();
            int index;
            StreamReader reader = null;
            string line = null;
            string columnName;
            bool transactionCreated = false;
            bool columnAdded = false;


                try
                {
                    SQLCommand = "select sc.name, sc.column_id "
                                + " from sys.tables st"
                                + " join sys.columns sc on sc.object_id = st.object_id"
                                + " where st.name = '" + aTableName + "'"
                                + " order by sc.column_id";

                    sqlCmdForDBMaint.CommandType = CommandType.Text;
                    sqlCmdForDBMaint.CommandText = SQLCommand;
                    dtTables = MIDEnvironment.CreateDataTable("Table");
                    sda = new SqlDataAdapter(sqlCmdForDBMaint);
                    sda.Fill(dtTables);

                    foreach (DataRow dr in dtTables.Rows)
                    {
                        htColumns.Add(Convert.ToString(dr["Name"], CultureInfo.CurrentCulture).ToUpper(), null);
                    }

                    bool checkingColumns = true;

                    reader = new StreamReader(aFileName);
                    
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (line.Trim().StartsWith(@"/*") ||
                            line.Trim().StartsWith(@"--") ||
                            // Begin TT#4415 - JSmith - Database Upgrase Failure
                            //line.ToUpper().Contains("CREATE") ||
                            //line.ToUpper().Contains("GO") ||
                            (line.Trim().ToUpper().Contains("CREATE") && line.Trim().ToUpper().Contains("TABLE")) ||
                            line.Trim().ToUpper() == "GO" ||
                            // End TT#4415 - JSmith - Database Upgrase Failure
                            line.Trim().ToUpper() == "BEGIN" ||
                            line.Trim().ToUpper() == "END" ||
                            line.Trim().Length == 0)
                        {
                            continue;
                        }
                        else if (line.ToUpper().Contains(" PRIMARY KEY ") ||
                            line.ToUpper().Contains(" ON ") ||
                            line.Trim() == ")" || line.Trim() == "("
                            )
                        {
                            checkingColumns = false;
                        }
                        else if (checkingColumns)
                        {
                            line = line.Replace(@",", " ").Trim();
                            index = line.IndexOf(" ");
                            columnName = line.Substring(0, index).Replace(@"""", " ").Replace(@"[", " ").Replace(@"]", " ").Replace(@"\t", " ").Trim().ToUpper();
                            if (!htColumns.ContainsKey(columnName))
                            {
                                if (!transactionCreated)
                                {
                                    sqlCmdForDBMaint.Transaction = sqlCmdForDBMaint.Connection.BeginTransaction();
                                    transactionCreated = true;
                                }

                                index = line.IndexOf("/*");
                                if (index > 0)
                                {
                                    line = line.Substring(0, index - 1);
                                }
                                else 
                                {
                                    index = line.IndexOf("--");
                                    if (index > 0)
                                    {
                                        line = line.Substring(0, index - 1);
                                    }
                                }

                                //line = line.Substring(0, line.Length - 1);
                                line = line.Trim();
                                line = line.Replace(",", "");
                                line = line.Replace("))", ")");
                                line = line.Replace(" )", "");
                                line = line.ToUpper().Replace("NULL)", "null");
                                //AddColumn(aTableName, line);
                                try
                                {
                                    string command = null;

                                    command = "alter table " + aTableName + " add " + line;
                        
                                    if (command != null)
                                    {
                                        sqlCmdForDBMaint.CommandText = command;
                                        sqlCmdForDBMaint.ExecuteNonQuery();
                                    }
                                
                                }
                                catch
                                {
                                    throw;
                                }

                                columnAdded = true;
                            }
                        }
                    }

                    if (columnAdded)
                    {
                        sqlCmdForDBMaint.Transaction.Commit();
                    }

                    keepProcessing = true;
                }
                catch (Exception exc)
                {
                    keepProcessing = false;
                    messageQueue.Enqueue("UNEXPECTED EXCEPTION: Error encountered updating table " + aTableName + "  Error: " + exc.ToString());
                }
          
        }

        #endregion

        #region "Utility functions for processing database objects"

        private static bool isTableFound(SqlCommand aSqlCommand, string aName)
        {
            DataTable dt;
            SqlDataAdapter sda;

            aSqlCommand.CommandType = CommandType.Text;
            aSqlCommand.CommandText = "SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE='BASE TABLE' AND TABLE_NAME COLLATE SQL_Latin1_General_CP1_CI_AS ='" + aName + "'";
            dt = MIDEnvironment.CreateDataTable("Table");
            sda = new SqlDataAdapter(aSqlCommand);
            sda.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private static bool isIndexFound(SqlCommand aSqlCommand, string aName)
        {
            DataTable dt;
            SqlDataAdapter sda;

            if (aName == "METHOD_MOD_SALES_METH_SELL_IDX")
            {
                int h = 1;
            }

            aSqlCommand.CommandType = CommandType.Text;
            aSqlCommand.CommandText = "SELECT name FROM sysindexes WHERE name COLLATE SQL_Latin1_General_CP1_CI_AS = '" + aName + "'";
            dt = MIDEnvironment.CreateDataTable("Table");
            sda = new SqlDataAdapter(aSqlCommand);
            sda.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private static bool isConstraintFound(SqlCommand aSqlCommand, string aName)
        {
            DataTable dt;
            SqlDataAdapter sda;

            aSqlCommand.CommandType = CommandType.Text;
            aSqlCommand.CommandText = "SELECT  * FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS where CONSTRAINT_NAME COLLATE SQL_Latin1_General_CP1_CI_AS = '" + aName + "'";
            dt = MIDEnvironment.CreateDataTable("Constraint");
            sda = new SqlDataAdapter(aSqlCommand);
            sda.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                return true;
            }

            aSqlCommand.CommandText = "SELECT  * FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS where CONSTRAINT_NAME COLLATE SQL_Latin1_General_CP1_CI_AS = '" + aName + "'";
            dt = MIDEnvironment.CreateDataTable("Constraint");
            sda = new SqlDataAdapter(aSqlCommand);
            sda.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                return true;
            }

            aSqlCommand.CommandText = "select * from sys.default_constraints where name COLLATE SQL_Latin1_General_CP1_CI_AS = '" + aName + "'";
            dt = MIDEnvironment.CreateDataTable("Constraint");
            sda = new SqlDataAdapter(aSqlCommand);
            sda.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                return true;
            }
            
            return false;
        }

        private static bool isPrimaryKeyFound(SqlCommand aSqlCommand, string aName, string aFileName)
        {
            DataTable dt;
            SqlDataAdapter sda;

            if (isConstraintFound(aSqlCommand, aName))
            {
                return true;
            }

            // check if file name is table name
            string tableName = aName.Replace("_PK", "");
            aSqlCommand.CommandType = CommandType.Text;
            aSqlCommand.CommandText = "SELECT * FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS WHERE  CONSTRAINT_TYPE = 'PRIMARY KEY' AND TABLE_NAME COLLATE SQL_Latin1_General_CP1_CI_AS = '" + tableName + "' AND TABLE_SCHEMA ='dbo'";
            dt = MIDEnvironment.CreateDataTable("Constraint");
            sda = new SqlDataAdapter(aSqlCommand);
            sda.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                return true;
            }

            // get table name out of file and check
            //string cmdText = ReadFile(aFileName);
            StreamReader sr = new StreamReader(aFileName);
            string cmdText = sr.ReadToEnd();
            sr.Close();


            int index = cmdText.ToUpper().IndexOf("ADD ");
            tableName = cmdText.Substring(0, index - 1).Trim();
            index = tableName.ToUpper().IndexOf("ALTER");
            tableName = tableName.Substring(index + 5, tableName.Length - (index + 5) - 1).Trim();
            tableName = tableName.ToUpper().Replace("TABLE", "").Replace(@"""", "").Trim();
            aSqlCommand.CommandText = "SELECT * FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS WHERE  CONSTRAINT_TYPE = 'PRIMARY KEY' AND TABLE_NAME COLLATE SQL_Latin1_General_CP1_CI_AS = '" + tableName + "' AND TABLE_SCHEMA ='dbo'";
            dt = MIDEnvironment.CreateDataTable("Constraint");
            sda = new SqlDataAdapter(aSqlCommand);
            sda.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                return true;
            }

            return false;
        }

        private static bool isScalarFunctionFound(SqlCommand aSqlCommand, string aName)
        {
            DataTable dt;
            SqlDataAdapter sda;

            aSqlCommand.CommandType = CommandType.Text;
            aSqlCommand.CommandText = "SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[" + aName + "]') AND OBJECTPROPERTY(id, N'IsScalarFunction') = 1";
            dt = MIDEnvironment.CreateDataTable("Table");
            sda = new SqlDataAdapter(aSqlCommand);
            sda.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private static bool isTableFunctionFound(SqlCommand aSqlCommand, string aName)
        {
            DataTable dt;
            SqlDataAdapter sda;

            aSqlCommand.CommandType = CommandType.Text;
            aSqlCommand.CommandText = "SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[" + aName + "]') AND OBJECTPROPERTY(id, N'IsTableFunction') = 1";
            dt = MIDEnvironment.CreateDataTable("Table");
            sda = new SqlDataAdapter(aSqlCommand);
            sda.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private static bool isViewFound(SqlCommand aSqlCommand, string aName)
        {
            DataTable dt;
            SqlDataAdapter sda;

            aSqlCommand.CommandType = CommandType.Text;
            aSqlCommand.CommandText = "select * from sys.views where name COLLATE SQL_Latin1_General_CP1_CI_AS = '" + aName + "'";
            dt = MIDEnvironment.CreateDataTable("Table");
            sda = new SqlDataAdapter(aSqlCommand);
            sda.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private static bool isTypeFound(SqlCommand aSqlCommand, string aName)
        {
            DataTable dt;
            SqlDataAdapter sda;

            aSqlCommand.CommandType = CommandType.Text;
            aSqlCommand.CommandText = "SELECT * FROM sys.types WHERE is_table_type = 1 AND name COLLATE SQL_Latin1_General_CP1_CI_AS = '" + aName + "'";
            dt = MIDEnvironment.CreateDataTable("Table");
            sda = new SqlDataAdapter(aSqlCommand);
            sda.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private static bool isTriggerFound(SqlCommand aSqlCommand, string aName)
        {
            DataTable dt;
            SqlDataAdapter sda;

            aSqlCommand.CommandType = CommandType.Text;
            aSqlCommand.CommandText = "select * from sys.triggers where name COLLATE SQL_Latin1_General_CP1_CI_AS = '" + aName + "'";
            dt = MIDEnvironment.CreateDataTable("Table");
            sda = new SqlDataAdapter(aSqlCommand);
            sda.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private static bool isStoredProcedureFound(SqlCommand aSqlCommand, string aName)
        {
            DataTable dt;
            SqlDataAdapter sda;

            aSqlCommand.CommandType = CommandType.Text;
            aSqlCommand.CommandText = "select * from sys.procedures where name COLLATE SQL_Latin1_General_CP1_CI_AS = '" + aName + "'";
            dt = MIDEnvironment.CreateDataTable("Table");
            sda = new SqlDataAdapter(aSqlCommand);
            sda.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion

        #region "File Groups"

        private static string allocationFileGroup = "ALLOCATION";
        private static string forecastFileGroup = "FORECAST";
        private static string historyFileGroup = "HISTORY";
        private static int historyFileGroupNumber = 2;
        private static string dailyHistoryFileGroup = "DAILYHISTORY";
        private static int dailyHistoryFileGroupNumber = 2;
        private static string auditFileGroup = "AUDIT";
        private static void GetFileGroups(out bool keepProcessing)
        {
            DataTable dt;
            SqlDataAdapter sda;

            ArrayList fileGroups = new ArrayList();

            try
            {
                sqlCmdForDBMaint.CommandType = CommandType.Text;
                sqlCmdForDBMaint.CommandText = "select * from sysfilegroups order by groupname";
                dt = MIDEnvironment.CreateDataTable("File Groups");
                sda = new SqlDataAdapter(sqlCmdForDBMaint);
                sda.Fill(dt);

                foreach (DataRow dr in dt.Rows)
                {
                    fileGroups.Add(Convert.ToString(dr["groupname"]));
                }


                //string strValue = MIDConfigurationManager.AppSettings["NoDataTables"];
                //if (strValue != null)
                //{
                //    noDataTables = Convert.ToInt32(strValue);
                //}
                string strValue = MIDConfigurationManager.AppSettings["AllocationFileGroup"];
                if (strValue != null)
                {
                    allocationFileGroup = Convert.ToString(strValue);
                }
                strValue = MIDConfigurationManager.AppSettings["ForecastFileGroup"];
                if (strValue != null)
                {
                    forecastFileGroup = Convert.ToString(strValue);
                }
                strValue = MIDConfigurationManager.AppSettings["HistoryFileGroup"];
                if (strValue != null)
                {
                    historyFileGroup = Convert.ToString(strValue);
                }

                historyFileGroupNumber = 2;
                strValue = MIDConfigurationManager.AppSettings["NoHistoryFileGroup"];
                if (strValue != null)
                {
                    try
                    {
                        historyFileGroupNumber = Convert.ToInt32(strValue);
                    }
                    catch
                    {
                    }
                }
                strValue = MIDConfigurationManager.AppSettings["DailyHistoryFileGroup"];
                if (strValue != null)
                {
                    dailyHistoryFileGroup = Convert.ToString(strValue);
                }
                dailyHistoryFileGroupNumber = 2;
                strValue = MIDConfigurationManager.AppSettings["NoDailyHistoryFileGroup"];
                if (strValue != null)
                {
                    try
                    {
                        dailyHistoryFileGroupNumber = Convert.ToInt32(strValue);
                    }
                    catch
                    {
                    }
                }
                strValue = MIDConfigurationManager.AppSettings["AuditFileGroup"];
                if (strValue != null)
                {
                    auditFileGroup = Convert.ToString(strValue);
                }

                // begin TT#173 Provide Database Container for large data collections
                //strValue = MIDConfigurationManager.AppSettings["WeekArchiveFileGroup"];
                //if (strValue != null)
                //{
                //    weekArchiveFileGroup = Convert.ToString(strValue);
                //}
                //strValue = MIDConfigurationManager.AppSettings["DayArchiveFileGroup"];
                //if (strValue != null)
                //{
                //    dayArchiveFileGroup = Convert.ToString(strValue);
                //}
                // end TT#173 Provide Database Container for large data collections

                if (!fileGroups.Contains(allocationFileGroup))
                {
                    sendMessage("File Group " + allocationFileGroup + " does not exist.  Substituting PRIMARY");
                    allocationFileGroup = "PRIMARY";
                }
                if (!fileGroups.Contains(forecastFileGroup))
                {
                    sendMessage("File Group " + forecastFileGroup + " does not exist.  Substituting PRIMARY");
                    forecastFileGroup = "PRIMARY";
                }
                bool resetNoFileGroup = false;
                for (int i = 0; i < historyFileGroupNumber; i++)
                {
                    if (i == 0)
                    {
                        if (!fileGroups.Contains(historyFileGroup))
                        {
                            sendMessage("File Group " + historyFileGroup + " does not exist.  Substituting PRIMARY");

                            historyFileGroup = "PRIMARY";
                            resetNoFileGroup = true;
                        }
                    }
                    else if (!resetNoFileGroup)
                    {
                        string fg = historyFileGroup + (i + 1).ToString();
                        if (!fileGroups.Contains(fg))
                        {
                            resetNoFileGroup = true;
                            sendMessage("File Group " + fg + " does not exist.  Substituting " + historyFileGroup);
                        }
                    }
                }
                if (resetNoFileGroup &&
                    historyFileGroupNumber > 1)
                {
                    historyFileGroupNumber = 1;
                    sendMessage("Overriding the number of history file groups to 1");
                }
                resetNoFileGroup = false;
                for (int i = 0; i < dailyHistoryFileGroupNumber; i++)
                {
                    if (i == 0)
                    {
                        if (!fileGroups.Contains(dailyHistoryFileGroup))
                        {
                            sendMessage("File Group " + dailyHistoryFileGroup + " does not exist.  Substituting " + historyFileGroup);
                            dailyHistoryFileGroup = historyFileGroup;
                            resetNoFileGroup = true;
                        }
                    }
                    else if (!resetNoFileGroup)
                    {
                        string fg = dailyHistoryFileGroup + (i + 1).ToString();
                        if (!fileGroups.Contains(fg))
                        {
                            sendMessage("File Group " + fg + " does not exist.  Substituting " + dailyHistoryFileGroup);
                            resetNoFileGroup = true;
                        }
                    }
                }
                if (resetNoFileGroup &&
                    dailyHistoryFileGroupNumber > 1)
                {
                    dailyHistoryFileGroupNumber = 1;
                    sendMessage("Overriding the number of daily history file groups to 1");
                }
                if (!fileGroups.Contains(auditFileGroup))
                {
                    sendMessage("File Group " + auditFileGroup + " does not exist.  Substituting PRIMARY");
                    auditFileGroup = "PRIMARY";
                }

                //if (!fileGroups.Contains(weekArchiveFileGroup))
                //{
                //    aMessageQueue.Enqueue("File Group " + weekArchiveFileGroup + " does not exist. Substituting PRIMARY");
                //    weekArchiveFileGroup = "PRIMARY";
                //}
                //if (!fileGroups.Contains(dayArchiveFileGroup))
                //{
                //    aMessageQueue.Enqueue("File Group " + dayArchiveFileGroup + " does not exist. Substituting PRIMARY");
                //    dayArchiveFileGroup = "PRIMARY";
                //}
                keepProcessing = true;
            }
            catch (Exception ex)
            {
                messageQueue.Enqueue("UNEXPECTED EXCEPTION: Reading FileGroups. Error=" + ex.ToString());
                keepProcessing = false;
            }

        }
        #endregion

        #region "Stamping"

        private static bool StampDatabase(Queue aMessageQueue, string aConnString, string aDatabase)
        {
            // Begin TT#195 MD - JSmith - Add environment authentication
            bool successful = true;
            string assemblyName = string.Empty;
            string remoteComputerName = null;
            try
            {
                assemblyName = folderForWindowsDLL;
                System.Diagnostics.FileVersionInfo fvi = System.Diagnostics.FileVersionInfo.GetVersionInfo(assemblyName);

                if (System.Windows.Forms.SystemInformation.TerminalServerSession)
                {
                    remoteComputerName = GetTerminalServerClientNameWTSAPI();
                }



                SqlCommand sqlCommand = null;
                bool connectionOpen = false;
                try
                {
                    try
                    {
                        string connectionString = aConnString;

                        sqlCommand = new SqlCommand();
                        sqlCommand.Connection = new SqlConnection(connectionString);
                        sqlCommand.Connection.Open();
                        connectionOpen = true;
                    }
                    catch (Exception ex)
                    {
                        string message = ex.ToString();
                        aMessageQueue.Enqueue("UNEXPECTED EXCEPTION: Error encountered during open of database");
                        throw;
                    }

                    try
                    {

                        sqlCommand.CommandText = "INSERT INTO APPLICATION_UPGRADE_HISTORY (UPGRADE_VERSION,UPGRADE_DATETIME,UPGRADE_USER,UPGRADE_MACHINE,UPGRADE_REMOTE_MACHINE,UPGRADE_CONFIGURATION) "
                           + "     VALUES (@version, @dt, @user, @machine, @rmachine,@config)";

                        AddParam(sqlCommand, "@version", SqlDbType.VarChar, fvi.FileVersion);
                        AddParam(sqlCommand, "@dt", SqlDbType.DateTime, DateTime.Now);
                        AddParam(sqlCommand, "@user", SqlDbType.VarChar, System.Security.Principal.WindowsIdentity.GetCurrent().Name);
                        AddParam(sqlCommand, "@machine", SqlDbType.VarChar, System.Environment.MachineName);
                        AddParam(sqlCommand, "@rmachine", SqlDbType.VarChar, remoteComputerName);
                        object[] customAttributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyConfigurationAttribute), true);
                        AddParam(sqlCommand, "@config", SqlDbType.VarChar, string.IsNullOrEmpty(((AssemblyConfigurationAttribute)customAttributes[0]).Configuration) ? "Base" : ((AssemblyConfigurationAttribute)customAttributes[0]).Configuration);


                        if (!ExecuteCommand(aMessageQueue, sqlCommand))
                        {
                            successful = false;
                        }
                    }
                    catch (Exception ex)
                    {
                        aMessageQueue.Enqueue("UNEXPECTED EXCEPTION: " + ex.ToString());
                    }
                }
                catch (Exception ex)
                {
                    aMessageQueue.Enqueue("UNEXPECTED EXCEPTION: " + ex.ToString());
                }
                finally
                {
                    if (connectionOpen)
                    {
                        sqlCommand.Connection.Close();
                    }
                }
            }
            catch
            {
                messageQueue.Enqueue("UNEXPECTED EXCEPTION: Unable to stamp database using information from " + assemblyName);
            }


            return successful;
            // End TT#195 MD
        }

        [DllImport("Wtsapi32.dll")]
        static extern bool WTSQuerySessionInformation(
            System.IntPtr hServer, int sessionId, WTS_INFO_CLASS wtsInfoClass, out System.IntPtr ppBuffer, out uint pBytesReturned);

        public enum WTS_INFO_CLASS
        {
            WTSInitialProgram,
            WTSApplicationName,
            WTSWorkingDirectory,
            WTSOEMId,
            WTSSessionId,
            WTSUserName,
            WTSWinStationName,
            WTSDomainName,
            WTSConnectState,
            WTSClientBuildNumber,
            WTSClientName,
            WTSClientDirectory,
            WTSClientProductId,
            WTSClientHardwareId,
            WTSClientAddress,
            WTSClientDisplay,
            WTSClientProtocolType
        }

        private static string GetTerminalServerClientNameWTSAPI()
        {

            const int WTS_CURRENT_SERVER_HANDLE = -1;

            IntPtr buffer = IntPtr.Zero;
            uint bytesReturned;

            string strReturnValue = "";
            try
            {
                WTSQuerySessionInformation(IntPtr.Zero, WTS_CURRENT_SERVER_HANDLE, WTS_INFO_CLASS.WTSClientName, out buffer, out bytesReturned);
                strReturnValue = System.Runtime.InteropServices.Marshal.PtrToStringAnsi(buffer);
            }

            finally
            {
                buffer = IntPtr.Zero;
            }

            return strReturnValue;
        }

        #endregion

        #region "Collation"

        // Begin TT#1343 - JSmith - Validate collation
        public static bool IsDatabaseCaseSensitive(Queue aMessageQueue, string aConnString)
        {
            bool connectionOpen = false;
            DataTable dt;
            SqlDataAdapter sda;
            SqlCommand sqlCommand = null;

            try
            {
                // open connection

                try
                {
                    string connectionString = aConnString;

                    sqlCommand = new SqlCommand();
                    sqlCommand.Connection = new SqlConnection(connectionString);
                    sqlCommand.Connection.Open();
                    connectionOpen = true;
                }
                catch (Exception ex)
                {
                    string message = ex.ToString();
                    aMessageQueue.Enqueue("UNEXPECTED EXCEPTION: Error encountered during open of database");
                    throw;
                }

                sqlCommand.CommandType = CommandType.Text;
                sqlCommand.CommandText = "SELECT CASE WHEN 'A' = 'a' THEN 'NOT CASE SENSITIVE' ELSE 'CASE SENSITIVE' END as CI";
                dt = MIDEnvironment.CreateDataTable("CI_Test");
                sda = new SqlDataAdapter(sqlCommand);
                sda.Fill(dt);

                if (dt.Rows.Count == 0)
                {
                    return false;
                }
                else if (Convert.ToString(dt.Rows[0]["CI"]) == "CASE SENSITIVE")
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                aMessageQueue.Enqueue("UNEXPECTED EXCEPTION: " + ex.ToString());
            }
            finally
            {
                if (connectionOpen)
                {
                    sqlCommand.Connection.Close();
                }
            }
            return false;
        }

        public static string GetCollation(Queue aMessageQueue, string aConnString, string aDatabase)
        {
            bool connectionOpen = false;
            DataTable dt;
            SqlDataAdapter sda;
            SqlCommand sqlCommand = null;

            try
            {
                // open connection

                try
                {
                    string connectionString = aConnString;

                    sqlCommand = new SqlCommand();
                    sqlCommand.Connection = new SqlConnection(connectionString);
                    sqlCommand.Connection.Open();
                    connectionOpen = true;
                }
                catch (Exception ex)
                {
                    string message = ex.ToString();
                    aMessageQueue.Enqueue("UNEXPECTED EXCEPTION: Error encountered during open of database");
                    throw;
                }

                sqlCommand.CommandType = CommandType.Text;
                sqlCommand.CommandText = "SELECT DATABASEPROPERTYEX('" + aDatabase + "', 'Collation') SQLCollation";

                dt = MIDEnvironment.CreateDataTable("Collation");
                sda = new SqlDataAdapter(sqlCommand);
                sda.Fill(dt);

                if (dt.Rows.Count == 0)
                {
                    return String.Empty;
                }
                else
                {
                    return Convert.ToString(dt.Rows[0]["SQLCollation"]);
                }
            }
            catch (Exception ex)
            {
                aMessageQueue.Enqueue("UNEXPECTED EXCEPTION: " + ex.ToString());
            }
            finally
            {
                if (connectionOpen)
                {
                    sqlCommand.Connection.Close();
                }
            }
            return String.Empty;
        }
        // End TT#1342

        #endregion

        #region "Database Validation and Compatibility Level"
        // Begin TT#3497 - JSmith - Add Database Compatibility Level Check in Upgrade Process
        public enum CompatibilityLevel
        {
            Undefined = 0,
            SQL2005 = 90,
            SQL2008 = 100,
            SQL2012 = 110,
            SQL2014 = 120,   // TT#1795-MD - JSmith - Support 2014
        }
        // End TT#3497 - JSmith - Add Database Compatibility Level Check in Upgrade Process
        // Begin TT#3497 - JSmith - Add Database Compatibility Level Check in Upgrade Process
        public static CompatibilityLevel GetCompatibilityLevel(Queue aMessageQueue, string aConnString, string aDatabase)
        {
            bool connectionOpen = false;
            DataTable dt;
            SqlDataAdapter sda;
            SqlCommand sqlCommand = null;

            try
            {
                // open connection

                try
                {
                    string connectionString = aConnString;

                    sqlCommand = new SqlCommand();
                    sqlCommand.Connection = new SqlConnection(connectionString);
                    sqlCommand.Connection.Open();
                    connectionOpen = true;
                }
                catch (Exception ex)
                {
                    string message = ex.ToString();
                    aMessageQueue.Enqueue("UNEXPECTED EXCEPTION: Error encountered during open of database");
                    throw;
                }

                sqlCommand.CommandType = CommandType.Text;
                sqlCommand.CommandText = "SELECT compatibility_level FROM sys.databases WHERE name ='" + aDatabase + "'";

                dt = MIDEnvironment.CreateDataTable("Compatibility");
                sda = new SqlDataAdapter(sqlCommand);
                sda.Fill(dt);

                if (dt.Rows.Count == 0)
                {
                    return CompatibilityLevel.Undefined;
                }
                else if (dt.Rows[0]["compatibility_level"] == DBNull.Value)
                {
                    return CompatibilityLevel.Undefined;
                }
                else
                {
                    return (CompatibilityLevel)Convert.ToInt32(dt.Rows[0]["compatibility_level"]);
                }
            }
            catch (Exception ex)
            {
                aMessageQueue.Enqueue("UNEXPECTED EXCEPTION: " + ex.ToString());
            }
            finally
            {
                if (connectionOpen)
                {
                    sqlCommand.Connection.Close();
                }
            }
            return CompatibilityLevel.Undefined;
        }
        // End TT#3497 - JSmith - Add Database Compatibility Level Check in Upgrade Process

        static public bool IsMIDDatabase(Queue aMessageQueue, string aConnString)
        {
            bool connectionOpen = false;
            DataTable dt;
            SqlDataAdapter sda;
            SqlCommand sqlCommand = null;

            try
            {
                // open connection

                try
                {
                    //TT#1130 - Database Utility  does not always connect across the network - apicchetti - 2/10/2011
                    //string connectionString = "server=" + aServer + ";database=" + aDatabase + ";uid=" + aUser + ";pwd=" + aPassword + ";";
                    string connectionString = aConnString;
                    //TT#1130 - Database Utility  does not always connect across the network - apicchetti - 2/10/2011

                    sqlCommand = new SqlCommand();
                    sqlCommand.Connection = new SqlConnection(connectionString);
                    sqlCommand.Connection.Open();
                    connectionOpen = true;
                }
                // TT#1468 - GRT - added SQLException for trapping
                catch (SqlException sqlex)
                {
                    string message = sqlex.ToString();
                    aMessageQueue.Enqueue("UNEXPECTED EXCEPTION: Error encountered during open of database");
                    aMessageQueue.Enqueue(message);
                    return false;
                }
                catch (Exception ex)
                {
                    string message = ex.ToString();
                    aMessageQueue.Enqueue("UNEXPECTED EXCEPTION: " + message);
                    return false;
                }

                sqlCommand.CommandType = CommandType.Text;
                sqlCommand.CommandText = "select * from INFORMATION_SCHEMA.TABLES "
                    + " where TABLE_TYPE = 'BASE TABLE'"
                    + " and TABLE_NAME = 'SYSTEM_OPTIONS'";
                dt = MIDEnvironment.CreateDataTable("SYSTEM_OPTIONS");
                sda = new SqlDataAdapter(sqlCommand);
                sda.Fill(dt);

                if (dt.Rows.Count == 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (SqlException sqlex)
            {
                aMessageQueue.Enqueue("UNEXPECTED SQL EXCEPTION: " + sqlex.ToString());
                return false;
            }
            catch (Exception ex)
            {
                aMessageQueue.Enqueue("UNEXPECTED EXCEPTION: " + ex.ToString());
                return false;
            }
            finally
            {
                if (connectionOpen)
                {
                    sqlCommand.Connection.Close();
                }
            }
        }

        static public eDatabaseType GetDatabaseType(Queue aMessageQueue, string aConnString)
        {
            bool connectionOpen = false;
            DataTable dt;
            DataRow dr;
            SqlDataAdapter sda;
            SqlCommand sqlCommand = null;
            eDatabaseType databaseType = eDatabaseType.None;
            string version;

            try
            {
                // open connection

                try
                {
                    string connectionString = aConnString;

                    sqlCommand = new SqlCommand();
                    sqlCommand.Connection = new SqlConnection(connectionString);
                    sqlCommand.Connection.Open();
                    connectionOpen = true;
                }
                catch (SqlException sqlex)
                {
                    string message = sqlex.ToString();
                    aMessageQueue.Enqueue("UNEXPECTED EXCEPTION: Error encountered during open of database");
                    aMessageQueue.Enqueue(message);
                    return databaseType;
                }
                catch (Exception ex)
                {
                    string message = ex.ToString();
                    aMessageQueue.Enqueue("UNEXPECTED EXCEPTION: " + message);
                    return databaseType;
                }

                sqlCommand.CommandType = CommandType.Text;
                sqlCommand.CommandText = "SELECT  SERVERPROPERTY('productversion') as version";
                dt = MIDEnvironment.CreateDataTable("Version");
                sda = new SqlDataAdapter(sqlCommand);
                sda.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    dr = dt.Rows[0];
                    version = Convert.ToString(dr["version"]);
                    if (version.StartsWith("9."))
                    {
                        databaseType = eDatabaseType.SQLServer2005;
                    }
                    else if (version.StartsWith("10."))
                    {
                        databaseType = eDatabaseType.SQLServer2008;
                    }
                    else if (version.StartsWith("11."))
                    {
                        databaseType = eDatabaseType.SQLServer2012;
                    }
                    else
                    {
                        databaseType = eDatabaseType.SQLServer2000;
                    }
                }
                return databaseType;
            }
            //  TT#1468 - GRT - added SQLException for trapping
            catch (SqlException sqlex)
            {
                string message = "UNEXPECTED SQL EXCEPTION: " + sqlex.ToString();
                aMessageQueue.Enqueue(message);
                return databaseType;
            }
            catch (Exception ex)
            {
                aMessageQueue.Enqueue("UNEXPECTED EXCEPTION: " + ex.ToString());
                return databaseType;
            }
            finally
            {
                if (connectionOpen)
                {
                    sqlCommand.Connection.Close();
                }

            }
        }

        #endregion

        #region "Degree of Parallelism"
        // Begin TT#4318 - JSmith - Remove setting max degree of parallelism during database upgrade
        //public static void SetMaxDegreeOfParallelismToOne(Queue aMessageQueue, string aMasterConnString)
        //{
        //    DataTable dt;
        //    SqlDataAdapter sda;
        //    SqlCommand sqlCommand = null;
        //    bool connectionOpen = false;
        //    try
        //    {
        //        try
        //        {
        //            //TT#1130 - Database Utility  does not always connect across the network - apicchetti - 2/10/2011
        //            string connectionString = aMasterConnString;
        //            //string connectionString = "server=" + aServer + ";database=master;uid=" + aUser + ";pwd=" + aPassword + ";";
        //            //TT#1130 - Database Utility  does not always connect across the network - apicchetti - 2/10/2011

        //            sqlCommand = new SqlCommand();
        //            sqlCommand.Connection = new SqlConnection(connectionString);
        //            sqlCommand.Connection.Open();
        //            connectionOpen = true;
        //        }
        //        //  TT#1468 - GRT - Reporting incorrect completion status when error occurs
        //        //      added SQLException, returns instead of throws.
        //        catch (SqlException sqlex)
        //        {
        //            string message = sqlex.ToString();
        //            aMessageQueue.Enqueue("FATAL DB Error: Error encountered during open of database");
        //            return;
        //        }
        //        catch (Exception ex)
        //        {
        //            string message = "ERROR: " + ex.ToString();
        //            aMessageQueue.Enqueue(message);
        //            return;
        //        }

        //        sqlCommand.CommandType = CommandType.Text;
        //        sqlCommand.CommandText = "select value from sys.configurations where name = 'max degree of parallelism'";
        //        dt = MIDEnvironment.CreateDataTable("config");
        //        sda = new SqlDataAdapter(sqlCommand);
        //        sda.Fill(dt);

        //        if (dt.Rows.Count > 0)
        //        {
        //            _maxDegreeOfParallelism = Convert.ToInt32(dt.Rows[0]["value"]);
        //            //  TT#1468 - GRT - if the Max DOP call fails then we want to fail, if it succeeded, continue as normal
        //            if (!SetMaxDegreeOfParallelismToOne(aMessageQueue, sqlCommand))
        //            {
        //                return;
        //            }
        //        }
        //    }
        //    //  TT#1468 - GRT - added SQLException
        //    catch (SqlException sqlex)
        //    {
        //        string message = "UNEXPECTED SQL EXCEPTION: " + sqlex.ToString();
        //        aMessageQueue.Enqueue(message);
        //        sqlCommand.Transaction.Rollback();
        //        return;
        //    }
        //    catch (Exception ex)
        //    {
        //        string message = "UNEXPECTED EXCEPTION: " + ex.ToString();
        //        aMessageQueue.Enqueue(message);
        //        sqlCommand.Transaction.Rollback();
        //        return;
        //    }
        //    finally
        //    {
        //        if (connectionOpen)
        //        {
        //            sqlCommand.Connection.Close();
        //        }
        //    }
        //    return;
        //}

        //private static bool SetMaxDegreeOfParallelismToOne(Queue aMessageQueue, SqlCommand aSqlCommand)
        //{
        //    try
        //    {
        //        aSqlCommand.CommandText = "sp_configure 'show advanced options', 1 reconfigure;";
        //        ExecuteCommand(aMessageQueue, aSqlCommand);

        //        aSqlCommand.CommandText = "sp_configure 'max degree of parallelism', 1 reconfigure;";
        //        ExecuteCommand(aMessageQueue, aSqlCommand);

        //        aSqlCommand.CommandText = "sp_configure 'show advanced options', 0 reconfigure;";
        //        ExecuteCommand(aMessageQueue, aSqlCommand);
        //    }
        //    catch (Exception ex)
        //    {
        //        aMessageQueue.Enqueue("UNEXPECTED EXCEPTION: " + ex.ToString());
        //        return false;
        //    }
        //    return true;
        //}

        //public static void ResetMaxDegreeOfParallelism(Queue aMessageQueue, string aMasterConnString)
        //{
        //    SqlCommand sqlCommand = null;
        //    bool connectionOpen = false;
        //    try
        //    {
        //        try
        //        {
        //            //TT#1130 - Database Utility  does not always connect across the network - apicchetti - 2/10/2011
        //            //string connectionString = "server=" + aServer + ";database=master;uid=" + aUser + ";pwd=" + aPassword + ";";
        //            string connectionString = aMasterConnString;
        //            //TT#1130 - Database Utility  does not always connect across the network - apicchetti - 2/10/2011

        //            sqlCommand = new SqlCommand();
        //            sqlCommand.Connection = new SqlConnection(connectionString);
        //            sqlCommand.Connection.Open();
        //            connectionOpen = true;
        //        }
        //        catch (Exception ex)
        //        {
        //            string message = ex.ToString();
        //            aMessageQueue.Enqueue("FATAL DB Error: Error encountered during open of database");
        //            throw;
        //        }

        //        if (_maxDegreeOfParallelism > -1)
        //        {
        //            ResetMaxDegreeOfParallelism(aMessageQueue, sqlCommand);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        aMessageQueue.Enqueue("UNEXPECTED EXCEPTION: " + ex.ToString());
        //        sqlCommand.Transaction.Rollback();
        //    }
        //    finally
        //    {
        //        if (connectionOpen)
        //        {
        //            sqlCommand.Connection.Close();
        //        }
        //    }
        //}

        //private static void ResetMaxDegreeOfParallelism(Queue aMessageQueue, SqlCommand aSqlCommand)
        //{
        //    try
        //    {
        //        aSqlCommand.CommandText = "sp_configure 'show advanced options', 1 reconfigure;";
        //        ExecuteCommand(aMessageQueue, aSqlCommand);

        //        aSqlCommand.CommandText = "sp_configure 'max degree of parallelism', " + _maxDegreeOfParallelism + " reconfigure;";
        //        ExecuteCommand(aMessageQueue, aSqlCommand);

        //        aSqlCommand.CommandText = "sp_configure 'show advanced options', 0 reconfigure;";
        //        ExecuteCommand(aMessageQueue, aSqlCommand);
        //    }
        //    catch (Exception ex)
        //    {
        //        aMessageQueue.Enqueue("UNEXPECTED EXCEPTION: " + ex.ToString());
        //    }
        //}
        // End TT#4318 - JSmith - Remove setting max degree of parallelism during database upgrade
        #endregion

        #region "Recovery Model"
        
        private static void SetRecoveryModelSimple(Queue aMessageQueue, string aConnString, string aDatabase)
        {
            DataTable dt;
            SqlDataAdapter sda;
            SqlCommand sqlCommand = null;
            bool connectionOpen = false;
            try
            {
                try
                {
                    //TT#1130 - Database Utility  does not always connect across the network - apicchetti - 2/10/2011
                    //string connectionString = "server=" + aServer + ";database=" + aDatabase + ";uid=" + aUser + ";pwd=" + aPassword + ";";
                    string connectionString = aConnString;
                    //TT#1130 - Database Utility  does not always connect across the network - apicchetti - 2/10/2011

                    sqlCommand = new SqlCommand();
                    sqlCommand.Connection = new SqlConnection(connectionString);
                    sqlCommand.Connection.Open();
                    connectionOpen = true;
                }
                catch (Exception ex)
                {
                    string message = ex.ToString();
                    aMessageQueue.Enqueue("UNEXPECTED EXCEPTION: Error encountered during open of database");
                    throw;
                }

                sqlCommand.CommandType = CommandType.Text;
                sqlCommand.CommandText = "SELECT recovery_model_desc FROM sys.databases WHERE name = '" + aDatabase + "'";
                dt = MIDEnvironment.CreateDataTable("model");
                sda = new SqlDataAdapter(sqlCommand);
                sda.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    _recoveryModel = Convert.ToString(dt.Rows[0]["recovery_model_desc"]);
                    SetRecoveryModelSimple(aMessageQueue, sqlCommand, aDatabase);
                }
            }
            catch (Exception ex)
            {
                aMessageQueue.Enqueue("UNEXPECTED EXCEPTION: " + ex.ToString());
                sqlCommand.Transaction.Rollback();
            }
            finally
            {
                if (connectionOpen)
                {
                    sqlCommand.Connection.Close();
                }
            }
        }

        private static void SetRecoveryModelSimple(Queue aMessageQueue, SqlCommand aSqlCommand, string aDatabase)
        {
            try
            {
                aSqlCommand.CommandText = "ALTER DATABASE " + aDatabase + " SET RECOVERY SIMPLE";
                ExecuteCommand(aMessageQueue, aSqlCommand);
            }
            catch (Exception ex)
            {
                aMessageQueue.Enqueue("UNEXPECTED EXCEPTION: " + ex.ToString());
            }
        }

        private static void RestoreRecoveryModel(Queue aMessageQueue, string aConnString, string aDatabase)
        {
            SqlCommand sqlCommand = null;
            bool connectionOpen = false;
            try
            {
                try
                {
                    //TT#1130 - Database Utility  does not always connect across the network - apicchetti - 2/10/2011
                    //string connectionString = "server=" + aServer + ";database=" + aDatabase + ";uid=" + aUser + ";pwd=" + aPassword + ";";
                    string connectionString = aConnString;
                    //TT#1130 - Database Utility  does not always connect across the network - apicchetti - 2/10/2011

                    sqlCommand = new SqlCommand();
                    sqlCommand.Connection = new SqlConnection(connectionString);
                    sqlCommand.Connection.Open();
                    connectionOpen = true;
                }
                catch (Exception ex)
                {
                    string message = ex.ToString();
                    aMessageQueue.Enqueue("UNEXPECTED EXCEPTION: Error encountered during open of database");
                    throw;
                }

                if (!string.IsNullOrEmpty(_recoveryModel))
                {
                    RestoreRecoveryModel(aMessageQueue, sqlCommand, aDatabase);
                }
            }
            catch (Exception ex)
            {
                aMessageQueue.Enqueue("UNEXPECTED EXCEPTION: " + ex.ToString());
            }
            finally
            {
                if (connectionOpen)
                {
                    sqlCommand.Connection.Close();
                }
            }
        }

        private static void RestoreRecoveryModel(Queue aMessageQueue, SqlCommand aSqlCommand, string aDatabase)
        {
            try
            {
                aSqlCommand.CommandText = "ALTER DATABASE " + aDatabase + " SET RECOVERY " + _recoveryModel;
                ExecuteCommand(aMessageQueue, aSqlCommand);
            }
            catch (Exception ex)
            {
                aMessageQueue.Enqueue("UNEXPECTED EXCEPTION: " + ex.ToString());
            }
        }
        
        #endregion

        #region "Constraints"
        
        private static void DisableTextConstraints(Queue aMessageQueue, string aConnString)
        {
            SqlCommand sqlCommand = null;
            bool connectionOpen = false;
            DataTable dt;
            try
            {
                try
                {
                    //TT#1130 - Database Utility  does not always connect across the network - apicchetti - 2/10/2011
                    //string connectionString = "server=" + aServer + ";database=" + aDatabase + ";uid=" + aUser + ";pwd=" + aPassword + ";";
                    string connectionString = aConnString;
                    //TT#1130 - Database Utility  does not always connect across the network - apicchetti - 2/10/2011

                    sqlCommand = new SqlCommand();
                    sqlCommand.Connection = new SqlConnection(connectionString);
                    sqlCommand.Connection.Open();
                    connectionOpen = true;
                }
                catch (Exception ex)
                {
                    string message = ex.ToString();
                    aMessageQueue.Enqueue("UNEXPECTED EXCEPTION: Error encountered during open of database");
                    throw;
                }
                dt = GetTextConstraints(aMessageQueue, sqlCommand);
                if (dt != null)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        DisableConstraints(aMessageQueue, sqlCommand, Convert.ToString(dr["TableName"]), Convert.ToString(dr["ForeignKey"]));
                    }
                }
            }
            catch (Exception ex)
            {
                aMessageQueue.Enqueue("UNEXPECTED EXCEPTION: " + ex.ToString());
            }
            finally
            {
                if (connectionOpen)
                {
                    sqlCommand.Connection.Close();
                }
            }
        }

        private static void DisableConstraints(Queue aMessageQueue, SqlCommand aSqlCommand, string aTable, string aKey)
        {
            try
            {
                aSqlCommand.CommandText = " ALTER TABLE " + aTable + " NOCHECK CONSTRAINT " + aKey;

                ExecuteCommand(aMessageQueue, aSqlCommand);
            }
            catch (Exception ex)
            {
                aMessageQueue.Enqueue("UNEXPECTED EXCEPTION: " + ex.ToString());
            }
        }

        private static void EnableTextConstraints(Queue aMessageQueue, string aConnString)
        {
            SqlCommand sqlCommand = null;
            bool connectionOpen = false;
            DataTable dt;
            try
            {
                try
                {
                    //TT#1130 - Database Utility  does not always connect across the network - apicchetti - 2/10/2011
                    //string connectionString = "server=" + aServer + ";database=" + aDatabase + ";uid=" + aUser + ";pwd=" + aPassword + ";";
                    string connectionString = aConnString;
                    //TT#1130 - Database Utility  does not always connect across the network - apicchetti - 2/10/2011

                    sqlCommand = new SqlCommand();
                    sqlCommand.Connection = new SqlConnection(connectionString);
                    sqlCommand.Connection.Open();
                    connectionOpen = true;
                }
                catch (Exception ex)
                {
                    string message = ex.ToString();
                    aMessageQueue.Enqueue("UNEXPECTED EXCEPTION: Error encountered during open of database");
                    throw;
                }

                dt = GetTextConstraints(aMessageQueue, sqlCommand);
                if (dt != null)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        EnableConstraints(aMessageQueue, sqlCommand, Convert.ToString(dr["TableName"]), Convert.ToString(dr["ForeignKey"]));
                    }
                }
            }
            catch (Exception ex)
            {
                aMessageQueue.Enqueue("UNEXPECTED EXCEPTION: " + ex.ToString());
            }
            finally
            {
                if (connectionOpen)
                {
                    sqlCommand.Connection.Close();
                }
            }
        }

        private static void EnableConstraints(Queue aMessageQueue, SqlCommand aSqlCommand, string aTable, string aKey)
        {
            try
            {
                aSqlCommand.CommandText = " ALTER TABLE " + aTable + " CHECK CONSTRAINT " + aKey;

                ExecuteCommand(aMessageQueue, aSqlCommand);
            }
            catch (Exception ex)
            {
                aMessageQueue.Enqueue("UNEXPECTED EXCEPTION: " + ex.ToString());
            }
        }

        private static DataTable GetTextConstraints(Queue aMessageQueue, SqlCommand aSqlCommand)
        {
            DataTable dt;
            SqlDataAdapter sda;
            try
            {
                aSqlCommand.CommandType = CommandType.Text;
                aSqlCommand.CommandText = "  SELECT f.name AS ForeignKey,"
                                        + "     OBJECT_NAME(f.parent_object_id) AS TableName,"
                                        + "     COL_NAME(fc.parent_object_id,"
                                        + "     fc.parent_column_id) AS ColumnName,"
                                        + "     OBJECT_NAME (f.referenced_object_id) AS ReferenceTableName,"
                                        + "     COL_NAME(fc.referenced_object_id,"
                                        + "     fc.referenced_column_id) AS ReferenceColumnName"
                                        + "   FROM sys.foreign_keys AS f"
                                        + "     INNER JOIN sys.foreign_key_columns AS fc"
                                        + "        ON f.OBJECT_ID = fc.constraint_object_id"
                                        + "   where OBJECT_NAME (f.referenced_object_id) = 'APPLICATION_TEXT'"
                                        + "     and COL_NAME(fc.referenced_object_id, fc.referenced_column_id) = 'TEXT_CODE'"
                                        + "   order by TableName, ColumnName";
                dt = MIDEnvironment.CreateDataTable("model");
                sda = new SqlDataAdapter(aSqlCommand);
                sda.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                aMessageQueue.Enqueue("UNEXPECTED EXCEPTION: " + ex.ToString());
                return null;
            }
        }

        #endregion

        #region "UI Interface Methods"
        private static ToolStripStatusLabel lblStatus;
        private static ToolStripProgressBar prgInstall;
        public static void SetProgressBar(ToolStripStatusLabel alblStatus, ToolStripProgressBar aprgInstall)
        {
            lblStatus = alblStatus;
            prgInstall = aprgInstall;
        }
        private static void LbLStatusEnabled(bool Value)
        {
            if (prgInstall != null &&
                !prgInstall.IsDisposed &&
                prgInstall.ProgressBar != null)
            {
                lblStatus.Enabled = Value;
            }
        }

        private static void ProgressBarEnabled(bool Value)
        {
            if (prgInstall != null &&
                !prgInstall.IsDisposed &&
                prgInstall.ProgressBar != null)
            {
                prgInstall.Enabled = Value;
            }
        }

        private static void ProgressBarIncrementValue(int Value)
        {
            if (prgInstall != null &&
                !prgInstall.IsDisposed &&
                prgInstall.ProgressBar != null)
            {
                if ((prgInstall.Value) + Value < prgInstall.Maximum)
                {
                    prgInstall.Increment(Value);
                }
            }
        }

        private static void ProgressBarSetValue(int Value)
        {
            if (prgInstall != null &&
                !prgInstall.IsDisposed &&
                prgInstall.ProgressBar != null)
            {
                if (Value < prgInstall.Maximum)
                {
                    prgInstall.Value = Value;
                }
            }
        }

        private static void ProgressBarSetStep(int Value)
        {
            if (prgInstall != null &&
                !prgInstall.IsDisposed &&
                prgInstall.ProgressBar != null)
            {
                prgInstall.Step = Value;
            }
        }

        private static void ProgressBarPerformStep()
        {
            if (prgInstall != null &&
                !prgInstall.IsDisposed &&
                prgInstall.ProgressBar != null)
            {
                prgInstall.PerformStep();
            }
        }

        private static void ProgressBarSetMinimum(int Value)
        {
            if (prgInstall != null &&
                !prgInstall.IsDisposed &&
                prgInstall.ProgressBar != null)
            {
                prgInstall.Minimum = Value;
            }
        }

        private static void ProgressBarSetMaximum(int Value)
        {
            if (prgInstall != null &&
                !prgInstall.IsDisposed &&
                prgInstall.ProgressBar != null)
            {
                prgInstall.Maximum = Value;
            }
        }

        private static void ProgressBarSetToMaximum()
        {
            if (prgInstall != null &&
                !prgInstall.IsDisposed &&
                prgInstall.ProgressBar != null)
            {
                ProgressBarSetValue(prgInstall.Maximum - 1);
            }
        }
        #endregion

        #region "SQL Command Helper Methods"
        private static void AddParam(SqlCommand aSqlCommand, string aName, SqlDbType aType, object aValue)
        {
            SqlParameter parm = new SqlParameter();
            parm.Direction = ParameterDirection.Input;
            parm.ParameterName = aName;
            parm.SqlDbType = aType;
            if (aValue == null)
            {
                parm.Value = DBNull.Value;
            }
            else
            {
                parm.Value = aValue;
            }
            aSqlCommand.Parameters.Add(parm);
        }
        private static bool ExecuteCommand(Queue aMessageQueue, SqlCommand aSqlCommand)
        {
            bool successful = true;
            try
            {
                try
                {
                    aSqlCommand.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    aMessageQueue.Enqueue("UNEXPECTED EXCEPTION: " + ex.ToString());
                    successful = false;
                }
            }
            catch
            {
                successful = false;
            }
            return successful;
        }

#endregion

        #region "Generate options and base class"
        private class genOptions
        {
            public string name;
            public string table;
            public StreamWriter writer;
            public bool isLockTable;
            public bool allowNull;
            public string checkConstraint;
            public string fileGroup;
            public int counter;
        }
        private delegate void genDelegate(genOptions options);
        private class genBase
        {
            public string name;
            public string table;
            public genTableTypes tableType;
            public genNonTableTypes genType;
            public genDelegate genDlg;
            public string suffix;

            public bool isLockTable = false;
            public bool allowNull = false;
            public string checkConstraint;
            public string fileGroup;
            public int counter;
            //public generatedTableishBase(string path, DatabaseObjectsSQLObjectType objectType, string objectName)
            //{
            //    this.objectType = objectType;
            //    this.objectName = objectName;
            //    this.path = path;
            //}
            public void GenerateFileForTableTypes()
            {
                string sPath = GetFolderForSQLObject(DatabaseObjectsSQLObjectType.Table, true); //Get the generated file path
                genOptions options = new genOptions();
                options.table = this.table;
                if (tableType == genTableTypes.Table)
                {
                    name = table;
                }
                if (tableType == genTableTypes.TableKey)
                {
                    name = table + suffix;
                }
                if (tableType == genTableTypes.Index)
                {
                    name = table + suffix;
                }
                options.name = name;

                StreamWriter sw = new StreamWriter(sPath + this.name + ".SQL");
                options.writer = sw;
                options.isLockTable = this.isLockTable;
                options.allowNull = this.allowNull;
                options.checkConstraint = this.checkConstraint;
                options.fileGroup = this.fileGroup;
                options.counter = this.counter;
                genDlg.Invoke(options);
                sw.Flush();
                sw.Close();
            }
            public void GenerateFileForNonTableTypes()
            {
                string sPath = GetFolderForSQLObject(DatabaseObjectsSQLObjectType.StoredProcedure, true); //Get the generated file path
                genOptions options = new genOptions();
                options.table = this.table;

                options.name = name;

                StreamWriter sw = new StreamWriter(sPath + this.name + ".SQL");
                options.writer = sw;
                options.isLockTable = this.isLockTable;
                options.allowNull = this.allowNull;
                options.checkConstraint = this.checkConstraint;
                options.fileGroup = this.fileGroup;
                options.counter = this.counter;
                genDlg.Invoke(options);
                sw.Flush();
                sw.Close();
            }

        }
        private static string _indent5 = "     ";
        private static string _indent10 = "          ";
        //private static string _indent15 = "               ";
        private static string _blankLine = new string(' ', 100);

        private static bool doGenerateStoreWeeklyForecast()
        {
            ProfileList aDatabaseVariables = variables.GetStoreWeeklyForecastDatabaseVariableList();
            if (aDatabaseVariables == null || aDatabaseVariables.Count == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        private static bool doGenerateChainWeeklyForecast()
        {
            ProfileList aDatabaseVariables = variables.GetChainWeeklyForecastDatabaseVariableList();
            if (aDatabaseVariables == null || aDatabaseVariables.Count == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        private static bool doGenerateChainHistory()
        {
            ProfileList aDatabaseVariables = variables.GetChainWeeklyHistoryDatabaseVariableList();
            if (aDatabaseVariables == null || aDatabaseVariables.Count == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        private static bool doGenerateStoreDailyHistory()
        {
            ProfileList aDatabaseVariables = variables.GetStoreDailyHistoryDatabaseVariableList();
            if (aDatabaseVariables == null || aDatabaseVariables.Count == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        private static bool doGenerateStoreWeeklyHistory()
        {
            ProfileList aDatabaseVariables = variables.GetStoreWeeklyHistoryDatabaseVariableList();
            if (aDatabaseVariables == null || aDatabaseVariables.Count == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        private static bool doGenerateOtherRollupsHistory()
        {
            ProfileList storeWeeklyHistoryRollupVariables = new ProfileList(eProfileType.Variable);
            foreach (VariableProfile vp in variables.GetStoreWeeklyHistoryDatabaseVariableList())
            {
                if (vp.LevelRollType != eLevelRollType.None &&
                    vp.StoreWeeklyHistoryModelType != eVariableDatabaseModelType.None &&
                    (vp.VariableCategory == eVariableCategory.Store || vp.VariableCategory == eVariableCategory.Both))
                {
                    storeWeeklyHistoryRollupVariables.Add(vp);
                }
            }

            ProfileList chainWeeklyHistoryRollupVariables = new ProfileList(eProfileType.Variable);
            foreach (VariableProfile vp in variables.GetChainWeeklyHistoryDatabaseVariableList())
            {
                // Begin TT#3158 - JSmith - Planning History Rollup
                //if (vp.LevelRollType != eLevelRollType.None &&
                //                    vp.ChainHistoryModelType != eVariableDatabaseModelType.None)
                if (vp.LevelRollType != eLevelRollType.None &&
                                    vp.ChainHistoryModelType != eVariableDatabaseModelType.None &&
                    (vp.VariableCategory == eVariableCategory.Chain || vp.VariableCategory == eVariableCategory.Both))
                // End TT#3158 - JSmith - Planning History Rollup
                {
                    chainWeeklyHistoryRollupVariables.Add(vp);
                }
            }

            if (storeWeeklyHistoryRollupVariables.Count > 0 && chainWeeklyHistoryRollupVariables.Count > 0)
                return true;
            else
                return false;
        }
        private static bool doGenerateOtherRollupsForecast()
        {
            ProfileList storeWeeklyForecastRollupVariables = new ProfileList(eProfileType.Variable);
            foreach (VariableProfile vp in variables.GetStoreWeeklyForecastDatabaseVariableList())
            {
                if (vp.LevelRollType != eLevelRollType.None &&
                    vp.StoreForecastModelType != eVariableDatabaseModelType.None &&
                    (vp.VariableCategory == eVariableCategory.Store || vp.VariableCategory == eVariableCategory.Both))
                {
                    storeWeeklyForecastRollupVariables.Add(vp);
                }
            }

            ProfileList chainWeeklyForecastRollupVariables = new ProfileList(eProfileType.Variable);
            foreach (VariableProfile vp in variables.GetChainWeeklyForecastDatabaseVariableList())
            {
                if (vp.LevelRollType != eLevelRollType.None &&
                    vp.ChainForecastModelType != eVariableDatabaseModelType.None &&
                    (vp.VariableCategory == eVariableCategory.Chain || vp.VariableCategory == eVariableCategory.Both))
                {
                    chainWeeklyForecastRollupVariables.Add(vp);
                }
            }

            if (storeWeeklyForecastRollupVariables.Count > 0 && chainWeeklyForecastRollupVariables.Count > 0)
                return true;
            else
                return false;
        }
        #endregion

        #region "Generate Table Objects"

        private static List<genBase> genTableList;
        private static void GenerateTableObjects(out bool keepProcessing)
        {
            try
            {

                MakeGeneratedTableObjectList();


                //Delete any existing files in the generated folder
                string sPath = GetFolderForSQLObject(DatabaseObjectsSQLObjectType.Table, true); //Get the generated file path
                DirectoryInfo di = new DirectoryInfo(sPath);
                foreach (FileInfo fi in di.GetFiles("*.SQL"))
                {
                    fi.Delete();
                }

                keepProcessing = true;
                foreach (genBase gb in genTableList)
                {
                    if (keepProcessing)
                    {
                        try
                        {
                            sendMessage("Generating " + gb.tableType.ToString() + " " + gb.name);
                            gb.GenerateFileForTableTypes();
                            processedQueue.Enqueue("Generated " + gb.tableType.ToString() + " " + gb.name);
                        }
                        catch (Exception ex)
                        {
                            keepProcessing = false;
                            messageQueue.Enqueue("UNEXPECTED EXCEPTION: Error generating table object. Type=" + gb.tableType.ToString() + " Name=" + gb.name + "  Error: " + ex.ToString());
                        }
                    }
                }

                keepProcessing = true;

            }
            catch (Exception ex)
            {
                keepProcessing = false;
                messageQueue.Enqueue("UNEXPECTED EXCEPTION: Error generating table objects. Error: " + ex.ToString());
            }
        }
        private static void MakeGeneratedTableObjectList()
        {
            genTableList = new List<genBase>();
            int tableCount = 10;
            if (doGenerateStoreWeeklyForecast())
            {
                genTableList.Add(new genBase { table = Include.DBStoreWeeklyLockTable, tableType = genTableTypes.Table, isLockTable = true, allowNull = true, genDlg = new genDelegate(Generate_Store_Forecast_Table) });
                genTableList.Add(new genBase { table = Include.DBStoreWeeklyLockTable, tableType = genTableTypes.TableKey, suffix = "_PK", isLockTable = true, genDlg = new genDelegate(Generate_Store_Forecast_Key) });
                genTableList.Add(new genBase { table = Include.DBStoreWeeklyLockTable, name = "HIER_NODE_STR_FORE_WK_LOCK_FK1", tableType = genTableTypes.Constraint, genDlg = new genDelegate(Generate_Node_Constraint) });
                genTableList.Add(new genBase { table = Include.DBStoreWeeklyLockTable, name = "STRS_STR_FORE_WK_LOCK_FK1", tableType = genTableTypes.Constraint, genDlg = new genDelegate(Generate_Store_Constraint) });
                genTableList.Add(new genBase { table = Include.DBStoreWeeklyLockTable, name = "FORE_VER_STR_FORE_WK_LOCK_FK1", tableType = genTableTypes.Constraint, genDlg = new genDelegate(Generate_Version_Constraint) });
                genTableList.Add(new genBase { table = Include.DBStoreWeeklyLockTable, tableType = genTableTypes.Index, suffix = "_ST_IDX", genDlg = new genDelegate(Generate_Store_Index) });

                for (int i = 0; i < tableCount; i++)
                {
                    string tableNo = i.ToString(CultureInfo.CurrentCulture);
                    string tableName = Include.DBStoreWeeklyForecastTable.Replace(Include.DBTableCountReplaceString, tableNo);
                    genTableList.Add(new genBase { table = tableName, tableType = genTableTypes.Table, allowNull = true, genDlg = new genDelegate(Generate_Store_Forecast_Table) });
                    genTableList.Add(new genBase { table = tableName, tableType = genTableTypes.TableKey, suffix = "_PK", genDlg = new genDelegate(Generate_Store_Forecast_Key) });
                    genTableList.Add(new genBase { table = tableName, name = tableName + "_HN_MOD", tableType = genTableTypes.Constraint, checkConstraint = tableNo, genDlg = new genDelegate(Generate_Check_Constraint) });
                    genTableList.Add(new genBase { table = tableName, name = "HIER_NODE_STR_FOR_WK" + tableNo + "_FK1", tableType = genTableTypes.Constraint, genDlg = new genDelegate(Generate_Node_Constraint) });
                    genTableList.Add(new genBase { table = tableName, name = "STRS_STR_FOR_WK" + tableNo + "_FK1", tableType = genTableTypes.Constraint, genDlg = new genDelegate(Generate_Store_Constraint) });
                    genTableList.Add(new genBase { table = tableName, name = "FOR_VER_STR_FOR_WK" + tableNo + "_FK1", tableType = genTableTypes.Constraint, genDlg = new genDelegate(Generate_Version_Constraint) });
                    genTableList.Add(new genBase { table = tableName, tableType = genTableTypes.Index, suffix = "_ST_IDX", genDlg = new genDelegate(Generate_Store_Index) });
                }
            }
            if (doGenerateChainWeeklyForecast())
            {
                genTableList.Add(new genBase { table = Include.DBChainWeeklyLockTable, tableType = genTableTypes.Table, isLockTable = true, allowNull = true, genDlg = new genDelegate(Generate_Chain_Forecast_Table) });
                genTableList.Add(new genBase { table = Include.DBChainWeeklyLockTable, tableType = genTableTypes.TableKey, suffix = "_PK", genDlg = new genDelegate(Generate_Chain_Forecast_Key) });
                genTableList.Add(new genBase { table = Include.DBChainWeeklyLockTable, name = "HIER_NODE_CHN_FORE_WK_LCK_FK1", tableType = genTableTypes.Constraint, genDlg = new genDelegate(Generate_Node_Constraint) });
                genTableList.Add(new genBase { table = Include.DBChainWeeklyLockTable, name = "FORE_VER_CHAIN_FORE_WK_LCK_FK1", tableType = genTableTypes.Constraint, genDlg = new genDelegate(Generate_Version_Constraint) });

                genTableList.Add(new genBase { table = Include.DBChainWeeklyForecastTable, tableType = genTableTypes.Table, allowNull = true, genDlg = new genDelegate(Generate_Chain_Forecast_Table) });
                genTableList.Add(new genBase { table = Include.DBChainWeeklyForecastTable, tableType = genTableTypes.TableKey, suffix = "_PK", genDlg = new genDelegate(Generate_Chain_Forecast_Key) });
                genTableList.Add(new genBase { table = Include.DBChainWeeklyForecastTable, name = "HIER_NODE_CHN_FOR_WK_FK1", tableType = genTableTypes.Constraint, genDlg = new genDelegate(Generate_Node_Constraint) });
                genTableList.Add(new genBase { table = Include.DBChainWeeklyForecastTable, name = "FOR_VER_CHN_FOR_WK_FK1", tableType = genTableTypes.Constraint, genDlg = new genDelegate(Generate_Version_Constraint) });
            }
            if (doGenerateChainHistory())
            {
                genTableList.Add(new genBase { table = Include.DBChainWeeklyHistoryTable, tableType = genTableTypes.Table, allowNull = true, genDlg = new genDelegate(Generate_Chain_History_Table) });
                genTableList.Add(new genBase { table = Include.DBChainWeeklyHistoryTable, tableType = genTableTypes.TableKey, suffix = "_PK", genDlg = new genDelegate(Generate_Chain_History_Key) });
                genTableList.Add(new genBase { table = Include.DBChainWeeklyHistoryTable, name = "HIER_NODE_CHN_HIS_WK_FK1", tableType = genTableTypes.Constraint, genDlg = new genDelegate(Generate_Node_Constraint) });
            }
            if (doGenerateStoreDailyHistory())
            {
                for (int i = 0; i < tableCount; i++)
                {
                    string tableNo = i.ToString(CultureInfo.CurrentCulture);
                    string tableName = Include.DBStoreDailyHistoryTable.Replace(Include.DBTableCountReplaceString, tableNo);
                    int remainder = i % dailyHistoryFileGroupNumber;
                    string fileGroup;
                    if (remainder == 0)
                    {
                        fileGroup = dailyHistoryFileGroup;
                    }
                    else
                    {
                        fileGroup = dailyHistoryFileGroup + (remainder + 1).ToString();
                    }
                    genTableList.Add(new genBase { table = tableName, tableType = genTableTypes.Table, allowNull = true, fileGroup = fileGroup, genDlg = new genDelegate(Generate_Store_History_Table) });
                    genTableList.Add(new genBase { table = tableName, tableType = genTableTypes.TableKey, suffix = "_PK", genDlg = new genDelegate(Generate_Store_History_Key) });
                    genTableList.Add(new genBase { table = tableName, name = tableName + "_HN_MOD", tableType = genTableTypes.Constraint, checkConstraint = tableNo, genDlg = new genDelegate(Generate_Check_Constraint) });
                    genTableList.Add(new genBase { table = tableName, name = "HIER_NODE_STR_HIS_DAY" + tableNo + "_FK1", tableType = genTableTypes.Constraint, genDlg = new genDelegate(Generate_Node_Constraint) });
                    genTableList.Add(new genBase { table = tableName, name = "STRS_STR_HIS_DAY" + tableNo + "_FK1", tableType = genTableTypes.Constraint, genDlg = new genDelegate(Generate_Store_Constraint) });
                    genTableList.Add(new genBase { table = tableName, tableType = genTableTypes.Index, suffix = "_ST_IDX", genDlg = new genDelegate(Generate_Store_Index) });
                }
            }
            if (doGenerateStoreWeeklyHistory())
            {
                for (int i = 0; i < tableCount; i++)
                {
                    string tableNo = i.ToString(CultureInfo.CurrentCulture);
                    string tableName = Include.DBStoreWeeklyHistoryTable.Replace(Include.DBTableCountReplaceString, tableNo);
                    int remainder = i % historyFileGroupNumber;
                    string fileGroup;
                    if (remainder == 0)
                    {
                        fileGroup = historyFileGroup;
                    }
                    else
                    {
                        fileGroup = historyFileGroup + (remainder + 1).ToString();
                    }
                    genTableList.Add(new genBase { table = tableName, tableType = genTableTypes.Table, allowNull = true, fileGroup = fileGroup, genDlg = new genDelegate(Generate_Store_History_Table) });
                    genTableList.Add(new genBase { table = tableName, tableType = genTableTypes.TableKey, suffix = "_PK", genDlg = new genDelegate(Generate_Store_History_Key) });
                    genTableList.Add(new genBase { table = tableName, name = tableName + "_HN_MOD", tableType = genTableTypes.Constraint, checkConstraint = tableNo, genDlg = new genDelegate(Generate_Check_Constraint) });
                    genTableList.Add(new genBase { table = tableName, name = "HIER_NODE_STR_HIS_WK" + tableNo + "_FK1", tableType = genTableTypes.Constraint, genDlg = new genDelegate(Generate_Node_Constraint) });
                    genTableList.Add(new genBase { table = tableName, name = "STRS_STR_HIS_WK" + tableNo + "_FK1", tableType = genTableTypes.Constraint, genDlg = new genDelegate(Generate_Store_Constraint) });
                    genTableList.Add(new genBase { table = tableName, tableType = genTableTypes.Index, suffix = "_ST_IDX", genDlg = new genDelegate(Generate_Store_Index) });
                }
            }
        }

        private enum genTableTypes
        {
            Table,
            TableKey,
            Constraint,
            Index,
            NotSupported
        }
        private static genTableTypes ConvertSQLObjectTypeToGenTableType(DatabaseObjectsSQLObjectType objType)
        {
            if (objType == DatabaseObjectsSQLObjectType.Table)
            {
                return genTableTypes.Table;
            }
            if (objType == DatabaseObjectsSQLObjectType.TableKey)
            {
                return genTableTypes.TableKey;
            }
            if (objType == DatabaseObjectsSQLObjectType.Constraint)
            {
                return genTableTypes.Constraint;
            }
            if (objType == DatabaseObjectsSQLObjectType.Index)
            {
                return genTableTypes.Index;
            }

            return genTableTypes.NotSupported;
            //throw new Exception("Type not supported: " + objType.ToString());
        }

        private static void Generate_Store_Forecast_Table(genOptions options)
        {
            string aTableName = options.table;
            StreamWriter aWriter = options.writer;
            bool aIsLockTable = options.isLockTable;
            bool aAllowNull = options.allowNull;
            try
            {
                ProfileList aDatabaseVariables = variables.GetStoreWeeklyForecastDatabaseVariableList();
                if (aDatabaseVariables == null || aDatabaseVariables.Count == 0)
                {
                    return;
                }

                aWriter.WriteLine("create table " + aTableName + " ( ");
                if (!aIsLockTable)
                {
                    aWriter.WriteLine(_indent5 + "HN_MOD smallint not null,");
                }
                aWriter.WriteLine(_indent5 + "HN_RID int not null,");
                aWriter.WriteLine(_indent5 + "FV_RID int not null,");
                aWriter.WriteLine(_indent5 + "TIME_ID int not null,");
                aWriter.WriteLine(_indent5 + "ST_RID int not null,");
                int count = 0;
                foreach (VariableProfile vp in aDatabaseVariables)
                {
                    ++count;
                    string line = AddVariable(vp, aIsLockTable, aAllowNull, eVariableCategory.Store);
                    if (count < aDatabaseVariables.Count)
                    {
                        line += ",";
                    }
                    else
                    {
                        line += ") on '" + forecastFileGroup + "'";
                    }
                    aWriter.WriteLine(_indent5 + line);
                }

                aWriter.WriteLine(System.Environment.NewLine);
                aWriter.WriteLine("GO");
                aWriter.WriteLine(System.Environment.NewLine);
            }
            catch
            {
                throw;
            }
        }
        private static void Generate_Chain_Forecast_Table(genOptions options)
        {
            string aTableName = options.table;
            StreamWriter aWriter = options.writer;
            bool aIsLockTable = options.isLockTable;
            bool aAllowNull = options.allowNull;
            try
            {
                ProfileList aDatabaseVariables = variables.GetChainWeeklyForecastDatabaseVariableList();
                if (aDatabaseVariables == null ||
                    aDatabaseVariables.Count == 0)
                {
                    return;
                }

                aWriter.WriteLine("create table " + aTableName + " ( ");
                aWriter.WriteLine(_indent5 + "HN_RID int not null,");
                aWriter.WriteLine(_indent5 + "FV_RID int not null,");
                aWriter.WriteLine(_indent5 + "TIME_ID int not null,");
                int count = 0;
                foreach (VariableProfile vp in aDatabaseVariables)
                {
                    ++count;
                    string line = AddVariable(vp, aIsLockTable, aAllowNull, eVariableCategory.Chain);
                    if (count < aDatabaseVariables.Count)
                    {
                        line += ",";
                    }
                    else
                    {
                        line += ") on '" + forecastFileGroup + "'";
                    }
                    aWriter.WriteLine(_indent5 + line);
                }

                aWriter.WriteLine(System.Environment.NewLine);
                aWriter.WriteLine("GO");
                aWriter.WriteLine(System.Environment.NewLine);
            }
            catch
            {
                throw;
            }
        }
        private static void Generate_Chain_History_Table(genOptions options)
        {
            string aTableName = options.table;
            bool aAllowNull = options.allowNull;
            StreamWriter aWriter = options.writer;
            try
            {
                ProfileList aDatabaseVariables = variables.GetChainWeeklyHistoryDatabaseVariableList();
                if (aDatabaseVariables == null ||
                    aDatabaseVariables.Count == 0)
                {
                    return;
                }

                aWriter.WriteLine("create table " + aTableName + " ( ");
                aWriter.WriteLine(_indent5 + "HN_RID int not null,");
                aWriter.WriteLine(_indent5 + "TIME_ID int not null,");
                int count = 0;
                foreach (VariableProfile vp in aDatabaseVariables)
                {
                    ++count;
                    string line = AddVariable(vp, false, aAllowNull, eVariableCategory.Chain);
                    if (count < aDatabaseVariables.Count)
                    {
                        line += ",";
                    }
                    else
                    {
                        line += ") on '" + historyFileGroup + "'";
                    }
                    aWriter.WriteLine(_indent5 + line);
                }

                aWriter.WriteLine(System.Environment.NewLine);
                aWriter.WriteLine("GO");
                aWriter.WriteLine(System.Environment.NewLine);
            }
            catch
            {
                throw;
            }
        }

        private static void Generate_Store_History_Table(genOptions options)
        {
            string aTableName = options.table;
            StreamWriter aWriter = options.writer;
            bool aAllowNull = options.allowNull;
            string aFileGroup = options.fileGroup;
            try
            {
                ProfileList aDatabaseVariables = variables.GetStoreDailyHistoryDatabaseVariableList();
                if (aDatabaseVariables == null ||
                    aDatabaseVariables.Count == 0)
                {
                    return;
                }

                aWriter.WriteLine("create table " + aTableName + " ( ");
                aWriter.WriteLine(_indent5 + "HN_MOD smallint not null,");
                aWriter.WriteLine(_indent5 + "HN_RID int not null,");
                aWriter.WriteLine(_indent5 + "TIME_ID int not null,");
                aWriter.WriteLine(_indent5 + "ST_RID int not null,");
                int count = 0;
                foreach (VariableProfile vp in aDatabaseVariables)
                {
                    ++count;
                    string line = AddVariable(vp, false, aAllowNull, eVariableCategory.Store);
                    if (count < aDatabaseVariables.Count)
                    {
                        line += ",";
                    }
                    else
                    {
                        line += ") on '" + aFileGroup + "'";
                    }
                    aWriter.WriteLine(_indent5 + line);
                }

                aWriter.WriteLine(System.Environment.NewLine);
                aWriter.WriteLine("GO");
                aWriter.WriteLine(System.Environment.NewLine);
            }
            catch
            {
                throw;
            }
        }

        private static void Generate_Node_Constraint(genOptions options)
        {
            string aTableName = options.table;
            string aConstraintName = options.name;
            StreamWriter aWriter = options.writer;
            try
            {
                aWriter.WriteLine("alter table " + aTableName);
                aWriter.WriteLine("   add constraint " + aConstraintName + " foreign key (");
                aWriter.WriteLine("      HN_RID)");
                aWriter.WriteLine("   references HIERARCHY_NODE (");
                aWriter.WriteLine("      HN_RID) on update no action on delete no action");

                aWriter.WriteLine(System.Environment.NewLine);
                aWriter.WriteLine("GO");
                aWriter.WriteLine(System.Environment.NewLine);
            }
            catch
            {
                throw;
            }
        }
        private static void Generate_Chain_History_Key(genOptions options)
        {
            string aTableName = options.table;
            StreamWriter aWriter = options.writer;
            string keyName = options.name;
            try
            {
                aWriter.WriteLine("alter table " + aTableName);
                aWriter.WriteLine(_indent5 + "add constraint " + keyName + " primary key clustered (HN_RID, TIME_ID)");

                aWriter.WriteLine(System.Environment.NewLine);
                aWriter.WriteLine("GO");
                aWriter.WriteLine(System.Environment.NewLine);
            }
            catch
            {
                throw;
            }
        }
        private static void Generate_Store_Forecast_Key(genOptions options)
        {
            string aTableName = options.table;
            bool aAddMod = !options.isLockTable;
            StreamWriter aWriter = options.writer;
            string keyName = options.name;
            try
            {
                aWriter.WriteLine("alter table " + aTableName);
                if (aAddMod)
                {
                    aWriter.WriteLine(_indent5 + "add constraint " + keyName + " primary key clustered (HN_RID, FV_RID, TIME_ID, ST_RID, HN_MOD)   ");
                }
                else
                {
                    aWriter.WriteLine(_indent5 + "add constraint " + keyName + " primary key clustered (HN_RID, FV_RID, TIME_ID, ST_RID)   ");
                }

                aWriter.WriteLine(System.Environment.NewLine);
                aWriter.WriteLine("GO");
                aWriter.WriteLine(System.Environment.NewLine);
            }
            catch
            {
                throw;
            }
        }
        private static void Generate_Chain_Forecast_Key(genOptions options)
        {
            string aTableName = options.table;
            StreamWriter aWriter = options.writer;
            string keyName = options.name;
            try
            {
                aWriter.WriteLine("alter table " + aTableName);
                aWriter.WriteLine(_indent5 + "add constraint " + keyName + " primary key clustered (HN_RID, FV_RID, TIME_ID)");

                aWriter.WriteLine(System.Environment.NewLine);
                aWriter.WriteLine("GO");
                aWriter.WriteLine(System.Environment.NewLine);
            }
            catch
            {
                throw;
            }
        }
        private static void Generate_Store_History_Key(genOptions options)
        {
            string aTableName = options.table;
            StreamWriter aWriter = options.writer;
            string keyName = options.name;
            try
            {
                aWriter.WriteLine("alter table " + aTableName);
                aWriter.WriteLine(_indent5 + "add constraint " + keyName + " primary key clustered (HN_RID, TIME_ID, ST_RID, HN_MOD)   ");

                aWriter.WriteLine(System.Environment.NewLine);
                aWriter.WriteLine("GO");
                aWriter.WriteLine(System.Environment.NewLine);
            }
            catch
            {
                throw;
            }
        }
        private static void Generate_Store_Constraint(genOptions options)
        {
            string aTableName = options.table;
            string aConstraintName = options.name;
            StreamWriter aWriter = options.writer;
            try
            {
                aWriter.WriteLine("alter table " + aTableName);
                aWriter.WriteLine("   add constraint " + aConstraintName + " foreign key (");
                aWriter.WriteLine("      ST_RID)");
                aWriter.WriteLine("   references STORES (");
                // BEGIN TT#739-MD - STodd - delete stores
                aWriter.WriteLine("      ST_RID) on update no action on delete cascade");
                // END TT#739-MD - STodd - delete stores

                aWriter.WriteLine(System.Environment.NewLine);
                aWriter.WriteLine("GO");
                aWriter.WriteLine(System.Environment.NewLine);
            }
            catch
            {
                throw;
            }
        }
        private static void Generate_Version_Constraint(genOptions options)
        {
            string aTableName = options.table;
            string aConstraintName = options.name;
            StreamWriter aWriter = options.writer;
            try
            {
                aWriter.WriteLine("alter table " + aTableName);
                aWriter.WriteLine("   add constraint " + aConstraintName + " foreign key (");
                aWriter.WriteLine("      FV_RID)");
                aWriter.WriteLine("   references FORECAST_VERSION (");
                aWriter.WriteLine("      FV_RID) on update no action on delete no action");

                aWriter.WriteLine(System.Environment.NewLine);
                aWriter.WriteLine("GO");
                aWriter.WriteLine(System.Environment.NewLine);
            }
            catch
            {
                throw;
            }
        }
        private static void Generate_Check_Constraint(genOptions options)
        {
            string aTableName = options.table;
            string aConstraintName = options.name;
            StreamWriter aWriter = options.writer;
            string aCheckConstraint = options.checkConstraint;
            try
            {
                aWriter.WriteLine("alter table " + aTableName + " add constraint " + aConstraintName + " check ([HN_MOD] = " + aCheckConstraint + ")");

                aWriter.WriteLine(System.Environment.NewLine);
                aWriter.WriteLine("GO");
                aWriter.WriteLine(System.Environment.NewLine);
            }
            catch
            {
                throw;
            }
        }
        private static void Generate_Store_Index(genOptions options)
        {
            string aTableName = options.table;
            string indexName = options.name;
            StreamWriter aWriter = options.writer;
            try
            {
                // Begin TT#3599 - JSmith - Issue with Purge in MID Test
                //aWriter.WriteLine("create index " + indexName + " on " + aTableName + " (");
                //// Begin TT#3330 - jsmith - Creating New Database Fails
                //if (aTableName.Contains("_DAY"))
                //{
                //    aWriter.WriteLine(_indent5 + "ST_RID) WITH (FILLFACTOR = 65)");
                //}
                //else
                //{
                //    aWriter.WriteLine(_indent5 + "ST_RID) WITH (FILLFACTOR = 80)");
                //}
                aWriter.WriteLine("create unique nonclustered index " + indexName + " on " + aTableName + " (");
                if (aTableName.Contains("_DAY"))
                {
                    aWriter.WriteLine(_indent5 + "[ST_RID] ASC,");
                    aWriter.WriteLine(_indent5 + "[HN_RID] ASC,");
                    aWriter.WriteLine(_indent5 + "[TIME_ID] ASC");
                    aWriter.WriteLine(_indent5 + ") WITH (FILLFACTOR = 65) ");
                }
                else if (aTableName.Contains("_FORECAST"))
                {
                    aWriter.WriteLine(_indent5 + "[ST_RID] ASC,");
                    aWriter.WriteLine(_indent5 + "[HN_RID] ASC,");
                    aWriter.WriteLine(_indent5 + "[FV_RID] ASC,");
                    aWriter.WriteLine(_indent5 + "[TIME_ID] ASC");
                    aWriter.WriteLine(_indent5 + ") WITH (FILLFACTOR = 80) ");
                }
                else
                {
                    aWriter.WriteLine(_indent5 + "[ST_RID] ASC,");
                    aWriter.WriteLine(_indent5 + "[HN_RID] ASC,");
                    aWriter.WriteLine(_indent5 + "[TIME_ID] ASC");
                    aWriter.WriteLine(_indent5 + ") WITH (FILLFACTOR = 80) ");
                }
                // End TT#3599 - JSmith - Issue with Purge in MID Test

                aWriter.WriteLine(System.Environment.NewLine);
                aWriter.WriteLine("GO");
                aWriter.WriteLine(System.Environment.NewLine);
            }
            catch
            {
                throw;
            }
        }

        private static string AddVariable(VariableProfile aVariableProfile, bool aIsLock, bool aAllowNull, eVariableCategory aVariableCategory)
        {
            try
            {
                eVariableDatabaseType databaseVariableType;
                switch (aVariableCategory)
                {
                    case eVariableCategory.Chain:
                        databaseVariableType = aVariableProfile.ChainDatabaseVariableType;
                        break;
                    case eVariableCategory.Store:
                        databaseVariableType = aVariableProfile.StoreDatabaseVariableType;
                        break;
                    default:
                        databaseVariableType = eVariableDatabaseType.None;
                        break;
                }
                string command = null;
                if (aIsLock)
                {
                    if (aAllowNull)
                    {
                        command = aVariableProfile.DatabaseColumnName + Include.cLockExtension + " char(1) null default 0";
                    }
                    else
                    {
                        command = aVariableProfile.DatabaseColumnName + Include.cLockExtension + " char(1) not null default 0";
                    }
                }
                else
                {
                    switch (databaseVariableType)
                    {
                        case eVariableDatabaseType.Integer:
                            if (aAllowNull)
                            {
                                command = aVariableProfile.DatabaseColumnName + " int null";
                            }
                            else
                            {
                                command = aVariableProfile.DatabaseColumnName + " int not null";
                            }
                            break;
                        case eVariableDatabaseType.Real:
                            if (aAllowNull)
                            {
                                command = aVariableProfile.DatabaseColumnName + " real null";
                            }
                            else
                            {
                                command = aVariableProfile.DatabaseColumnName + " real not null";
                            }
                            break;
                        case eVariableDatabaseType.DateTime:
                            if (aAllowNull)
                            {
                                command = aVariableProfile.DatabaseColumnName + " datetime null";
                            }
                            else
                            {
                                command = aVariableProfile.DatabaseColumnName + " datetime not null";
                            }
                            break;
                        case eVariableDatabaseType.String:
                            if (aAllowNull)
                            {
                                command = aVariableProfile.DatabaseColumnName + " varchar(100) null";
                            }
                            else
                            {
                                command = aVariableProfile.DatabaseColumnName + " varchar(100) not null";
                            }
                            break;
                        case eVariableDatabaseType.Char:
                            if (aAllowNull)
                            {
                                command = aVariableProfile.DatabaseColumnName + " char(1) null";
                            }
                            else
                            {
                                command = aVariableProfile.DatabaseColumnName + " char(1) not null";
                            }
                            break;
                        case eVariableDatabaseType.Float:
                            if (aAllowNull)
                            {
                                command = aVariableProfile.DatabaseColumnName + " float null";
                            }
                            else
                            {
                                command = aVariableProfile.DatabaseColumnName + " float not null";
                            }
                            break;
                        case eVariableDatabaseType.BigInteger:
                            if (aAllowNull)
                            {
                                command = aVariableProfile.DatabaseColumnName + " bigint null";
                            }
                            else
                            {
                                command = aVariableProfile.DatabaseColumnName + " bigint not null";
                            }
                            break;
                    }
                }
                return command;
            }
            catch
            {
                throw;
            }

        }
        #endregion

        #region "Generate Non Table Objects"
        private static List<genBase> genNonTableList;
        private static void GenerateNonTableObjects(out bool keepProcessing)
        {
            try
            {

                MakeGeneratedNonTableObjectList();


                //Delete any existing files in the generated folder
                string sPath = GetFolderForSQLObject(DatabaseObjectsSQLObjectType.StoredProcedure, true); //Get the generated file path for non-table objects
                DirectoryInfo di = new DirectoryInfo(sPath);
                foreach (FileInfo fi in di.GetFiles("*.SQL"))
                {
                    fi.Delete();
                }

                keepProcessing = true;
                foreach (genBase gb in genNonTableList)
                {
                    if (keepProcessing)
                    {
                        try
                        {
                            sendMessage("Generating " + gb.genType.ToString() + " " + gb.name);
                            gb.GenerateFileForNonTableTypes();
                            processedQueue.Enqueue("Generated " + gb.genType.ToString() + " " + gb.name);
                        }
                        catch (Exception ex)
                        {
                            keepProcessing = false;
                            messageQueue.Enqueue("UNEXPECTED EXCEPTION: Error generating non table object. Type=" + gb.genType.ToString() + " Name=" + gb.name + "  Error: " + ex.ToString());
                        }
                    }
                }

                keepProcessing = true;

            }
            catch (Exception ex)
            {
                keepProcessing = false;
                messageQueue.Enqueue("UNEXPECTED EXCEPTION: Error generating non table objects. Error: " + ex.ToString());
            }
        }
        private static void MakeGeneratedNonTableObjectList()
        {
            genNonTableList = new List<genBase>();
            int tableCount = 10;



            string tableName;
            string dropName;
            for (int i = 0; i < tableCount; i++)
            {
                tableName = Include.DBStoreDailyHistoryWriteSP.Replace(Include.DBTableCountReplaceString, i.ToString());
                dropName = tableName + "_DROP";
                genNonTableList.Add(new genBase { table = tableName, name = dropName, genType = genNonTableTypes.Drop, genDlg = new genDelegate(Generate_Drop_Procedure) });

                tableName = Include.DBStoreWeeklyHistoryWriteSP.Replace(Include.DBTableCountReplaceString, i.ToString());
                dropName = tableName + "_DROP";
                genNonTableList.Add(new genBase { table = tableName, name = dropName, genType = genNonTableTypes.Drop, genDlg = new genDelegate(Generate_Drop_Procedure) });

                tableName = Include.DBStoreWeeklyForecastWriteSP.Replace(Include.DBTableCountReplaceString, i.ToString());
                dropName = tableName + "_DROP";
                genNonTableList.Add(new genBase { table = tableName, name = dropName, genType = genNonTableTypes.Drop, genDlg = new genDelegate(Generate_Drop_Procedure) });

                tableName = Include.DBStoreDailyHistoryReadSP.Replace(Include.DBTableCountReplaceString, i.ToString());
                dropName = tableName + "_DROP";
                genNonTableList.Add(new genBase { table = tableName, name = dropName, genType = genNonTableTypes.Drop, genDlg = new genDelegate(Generate_Drop_Procedure) });

                tableName = Include.DBStoreWeeklyHistoryReadSP.Replace(Include.DBTableCountReplaceString, i.ToString());
                dropName = tableName + "_DROP";
                genNonTableList.Add(new genBase { table = tableName, name = dropName, genType = genNonTableTypes.Drop, genDlg = new genDelegate(Generate_Drop_Procedure) });

                tableName = Include.DBStoreWeeklyModVerReadSP.Replace(Include.DBTableCountReplaceString, i.ToString());
                dropName = tableName + "_DROP";
                genNonTableList.Add(new genBase { table = tableName, name = dropName, genType = genNonTableTypes.Drop, genDlg = new genDelegate(Generate_Drop_Procedure) });

                tableName = Include.DBStoreWeeklyForecastReadSP.Replace(Include.DBTableCountReplaceString, i.ToString());
                dropName = tableName + "_DROP";
                genNonTableList.Add(new genBase { table = tableName, name = dropName, genType = genNonTableTypes.Drop, genDlg = new genDelegate(Generate_Drop_Procedure) });
            }

            tableName = Include.DBStoreWeeklyForecastLockWriteSP;
            dropName = tableName + "_DROP";
            genNonTableList.Add(new genBase { table = tableName, name = dropName, genType = genNonTableTypes.Drop, genDlg = new genDelegate(Generate_Drop_Procedure) });

            tableName = Include.DBChainWeeklyHistoryWriteSP;
            dropName = tableName + "_DROP";
            genNonTableList.Add(new genBase { table = tableName, name = dropName, genType = genNonTableTypes.Drop, genDlg = new genDelegate(Generate_Drop_Procedure) });

            tableName = Include.DBChainWeeklyForecastWriteSP;
            dropName = tableName + "_DROP";
            genNonTableList.Add(new genBase { table = tableName, name = dropName, genType = genNonTableTypes.Drop, genDlg = new genDelegate(Generate_Drop_Procedure) });



            //tableName = Include.DBGetTableFromType;
            //dropName = tableName + "_DROP";
            //genNonTableList.Add(new genBase { table = tableName, name = dropName, genType = genNonTableTypes.Drop, genDlg = new genDelegate(Generate_Drop_Procedure) });

            //Generate types that are dependent on variables
            genNonTableList.Add(new genBase { name = Include.DBStoreDailyHistoryType, genType = genNonTableTypes.Type, genDlg = new genDelegate(Generate_MID_ST_HIS_DAY_TYPE) });
            genNonTableList.Add(new genBase { name = Include.DBStoreWeeklyHistoryType, genType = genNonTableTypes.Type, genDlg = new genDelegate(Generate_MID_ST_HIS_WK_TYPE) });
            genNonTableList.Add(new genBase { name = Include.DBStoreWeeklyForecastType, genType = genNonTableTypes.Type, genDlg = new genDelegate(Generate_MID_ST_FOR_WK_TYPE) });

            genNonTableList.Add(new genBase { name = "MID_ST_READ_TYPES", genType = genNonTableTypes.Type, genDlg = new genDelegate(Generate_MID_ST_READ_TYPES) });

            //Drop Types that are used by generated procedures
            //These are marked as Ignore in the SequenceForDeleting.xml
            genNonTableList.Add(new genBase { name = "MID_TYPE_DROPS_FOR_GENERATED_PROCEDURES", genType = genNonTableTypes.Drop, genDlg = new genDelegate(Generate_TYPE_DROPS) });



            //Generate procedures that are dependent on variables
            string procedureName;
            for (int i = 0; i < tableCount; i++)
            {
                if (doGenerateStoreDailyHistory())
                {
                    tableName = Include.DBStoreDailyHistoryTable.Replace(Include.DBTableCountReplaceString, i.ToString());

                    procedureName = Include.DBStoreDailyHistoryWriteSP.Replace(Include.DBTableCountReplaceString, i.ToString());
                    genNonTableList.Add(new genBase { table = tableName, name = procedureName, genType = genNonTableTypes.StoredProcedure, genDlg = new genDelegate(Generate_MID_ST_HIS_DAY_WRITE) });

                    procedureName = Include.DBStoreDailyHistoryReadSP.Replace(Include.DBTableCountReplaceString, i.ToString());
                    genNonTableList.Add(new genBase { table = tableName, name = procedureName, genType = genNonTableTypes.StoredProcedure, genDlg = new genDelegate(Generate_MID_ST_HIS_DAY_READ) });

                    genNonTableList.Add(new genBase { name = "ST_HIS_DAY_ROLLUP" + i.ToString(), genType = genNonTableTypes.StoredProcedure, counter = i, genDlg = new genDelegate(Generate_MID_ST_HIS_DAY_ROLLUP) });
                }
                if (doGenerateStoreWeeklyHistory())
                {
                    tableName = Include.DBStoreWeeklyHistoryTable.Replace(Include.DBTableCountReplaceString, i.ToString());

                    procedureName = Include.DBStoreWeeklyHistoryWriteSP.Replace(Include.DBTableCountReplaceString, i.ToString());
                    genNonTableList.Add(new genBase { table = tableName, name = procedureName, genType = genNonTableTypes.StoredProcedure, genDlg = new genDelegate(Generate_MID_ST_HIS_WK_WRITE) });

                    procedureName = Include.DBStoreWeeklyHistoryReadSP.Replace(Include.DBTableCountReplaceString, i.ToString());
                    genNonTableList.Add(new genBase { table = tableName, name = procedureName, genType = genNonTableTypes.StoredProcedure, genDlg = new genDelegate(Generate_MID_ST_HIS_WK_READ) });

                    genNonTableList.Add(new genBase { name = "ST_HIS_WK_ROLLUP" + i.ToString(), genType = genNonTableTypes.StoredProcedure, counter = i, genDlg = new genDelegate(Generate_MID_ST_HIS_WK_ROLLUP) });
                }
                if (doGenerateStoreWeeklyForecast())
                {
                    tableName = Include.DBStoreWeeklyForecastTable.Replace(Include.DBTableCountReplaceString, i.ToString());

                    procedureName = Include.DBStoreWeeklyForecastDelZeroSP.Replace(Include.DBTableCountReplaceString, i.ToString());
                    genNonTableList.Add(new genBase { table = tableName, name = procedureName, genType = genNonTableTypes.StoredProcedure, genDlg = new genDelegate(Generate_MID_ST_FOR_WK_DEL_ZERO) });

                    procedureName = Include.DBStoreWeeklyForecastWriteSP.Replace(Include.DBTableCountReplaceString, i.ToString());
                    genNonTableList.Add(new genBase { table = tableName, name = procedureName, genType = genNonTableTypes.StoredProcedure, genDlg = new genDelegate(Generate_MID_ST_FOR_WK_WRITE) });

                    procedureName = Include.DBStoreWeeklyForecastReadSP.Replace(Include.DBTableCountReplaceString, i.ToString());
                    genNonTableList.Add(new genBase { table = tableName, name = procedureName, genType = genNonTableTypes.StoredProcedure, genDlg = new genDelegate(Generate_MID_ST_FOR_WK_READ) });

                    procedureName = Include.DBStoreWeeklyModVerReadSP.Replace(Include.DBTableCountReplaceString, i.ToString());
                    genNonTableList.Add(new genBase { table = tableName, name = procedureName, genType = genNonTableTypes.StoredProcedure, genDlg = new genDelegate(Generate_MID_ST_MOD_WK_READ) });

                    genNonTableList.Add(new genBase { name = "ST_FOR_WK_ROLLUP" + i.ToString(), genType = genNonTableTypes.StoredProcedure, counter = i, genDlg = new genDelegate(Generate_MID_ST_FOR_WK_ROLLUP) });
                }
            }

            if (doGenerateStoreWeeklyForecast())
            {
                genNonTableList.Add(new genBase { table = Include.DBStoreWeeklyLockTable, name = Include.DBStoreWeeklyForecastDelUnlockedSP, genType = genNonTableTypes.StoredProcedure, genDlg = new genDelegate(Generate_MID_ST_FOR_WK_DEL_UNLOCK) });
                genNonTableList.Add(new genBase { table = Include.DBStoreWeeklyLockTable, name = Include.DBStoreWeeklyForecastLockReadSP, genType = genNonTableTypes.StoredProcedure, genDlg = new genDelegate(Generate_MID_ST_FOR_WK_LOCK_READ) });
                genNonTableList.Add(new genBase { name = Include.DBStoreWeeklyForecastLockWriteSP, genType = genNonTableTypes.StoredProcedure, genDlg = new genDelegate(Generate_MID_ST_FOR_WK_LOCK_WRITE) });
            }

            if (doGenerateChainHistory())
            {
                genNonTableList.Add(new genBase { table = Include.DBChainWeeklyHistoryTable, name = Include.DBChainWeeklyHistoryWriteSP + "_TYPES", genType = genNonTableTypes.Type, genDlg = new genDelegate(Generate_MID_CHN_HIS_WK_WRITE_TYPES) });
                genNonTableList.Add(new genBase { table = Include.DBChainWeeklyHistoryTable, name = Include.DBChainWeeklyHistoryWriteSP, genType = genNonTableTypes.StoredProcedure, genDlg = new genDelegate(Generate_MID_CHN_HIS_WK_WRITE) });
                genNonTableList.Add(new genBase { table = Include.DBChainWeeklyHistoryTable, name = Include.DBChainWeeklyHistoryReadSP, genType = genNonTableTypes.StoredProcedure, genDlg = new genDelegate(Generate_MID_CHN_HIS_WK_READ) });
                genNonTableList.Add(new genBase { name = "CHN_HIS_WK_ROLLUP", genType = genNonTableTypes.StoredProcedure, genDlg = new genDelegate(Generate_MID_CHN_HIS_WK_ROLLUP) });
            }
            if (doGenerateChainWeeklyForecast())
            {
                genNonTableList.Add(new genBase { table = Include.DBChainWeeklyForecastTable, name = Include.DBChainWeeklyForecastWriteSP + "_TYPES", genType = genNonTableTypes.Type, genDlg = new genDelegate(Generate_MID_CHN_FOR_WK_WRITE_TYPES) });
                genNonTableList.Add(new genBase { table = Include.DBChainWeeklyForecastTable, name = Include.DBChainWeeklyForecastWriteSP, genType = genNonTableTypes.StoredProcedure, genDlg = new genDelegate(Generate_MID_CHN_FOR_WK_WRITE) });
                genNonTableList.Add(new genBase { table = Include.DBChainWeeklyForecastTable, name = Include.DBChainWeeklyForecastReadSP, genType = genNonTableTypes.StoredProcedure, genDlg = new genDelegate(Generate_MID_CHN_FOR_WK_READ) });
                genNonTableList.Add(new genBase { table = Include.DBChainWeeklyLockTable, name = Include.DBChainWeeklyForecastLockReadSP, genType = genNonTableTypes.StoredProcedure, genDlg = new genDelegate(Generate_MID_CHN_FOR_WK_LOCK_READ) });
                genNonTableList.Add(new genBase { name = Include.DBChainWeeklyModVerReadSP, genType = genNonTableTypes.StoredProcedure, genDlg = new genDelegate(Generate_MID_CHN_MOD_WK_READ) });
                genNonTableList.Add(new genBase { table = Include.DBChainWeeklyForecastTable, name = Include.DBChainWeeklyForecastDelZeroSP, genType = genNonTableTypes.StoredProcedure, genDlg = new genDelegate(Generate_MID_CHN_FOR_WK_DEL_ZERO) });
                genNonTableList.Add(new genBase { name = Include.DBChainWeeklyForecastDelUnlockedSP, genType = genNonTableTypes.StoredProcedure, genDlg = new genDelegate(Generate_MID_CHN_FOR_WK_DEL_UNLOCK) });
                genNonTableList.Add(new genBase { name = "CHN_FOR_WK_ROLLUP", genType = genNonTableTypes.StoredProcedure, genDlg = new genDelegate(Generate_MID_CHN_FOR_WK_ROLLUP) });
            }

            if (doGenerateOtherRollupsHistory())
            {
                genNonTableList.Add(new genBase { name = "HIS_ST_OTHER_ROLLUPS", genType = genNonTableTypes.StoredProcedure, genDlg = new genDelegate(Generate_MID_HIS_ST_OTHER_ROLLUPS) });
            }

            if (doGenerateOtherRollupsForecast())
            {
                genNonTableList.Add(new genBase { name = "FOR_ST_TO_CHN_ROLLUP", genType = genNonTableTypes.StoredProcedure, genDlg = new genDelegate(Generate_MID_FOR_ST_TO_CHN_ROLLUP) });
            }
        }

        private enum genNonTableTypes
        {
            Type,
            Drop,
            StoredProcedure,
            FunctionScalar,
            FunctionTable,
            View,
            NotSupported
        }
        private static genNonTableTypes ConvertSQLObjectTypeToGenNonTableType(DatabaseObjectsSQLObjectType objType)
        {
            if (objType == DatabaseObjectsSQLObjectType.Type)
            {
                return genNonTableTypes.Type;
            }
            if (objType == DatabaseObjectsSQLObjectType.Drop)
            {
                return genNonTableTypes.Drop;
            }
            if (objType == DatabaseObjectsSQLObjectType.StoredProcedure)
            {
                return genNonTableTypes.StoredProcedure;
            }
            if (objType == DatabaseObjectsSQLObjectType.FunctionScalar)
            {
                return genNonTableTypes.FunctionScalar;
            }
            if (objType == DatabaseObjectsSQLObjectType.FunctionTable)
            {
                return genNonTableTypes.FunctionTable;
            }
            if (objType == DatabaseObjectsSQLObjectType.View)
            {
                return genNonTableTypes.View;
            }
            //throw new Exception("Non Table Type not supported: " + objType.ToString());
            return genNonTableTypes.NotSupported;
        }

        private static void AddTypeDrop(string aTypeName, StreamWriter aWriter)
        {
            try
            {
                aWriter.WriteLine("if exists (select * from sys.types WHERE is_table_type = 1 AND name = '" + aTypeName + "')");
                aWriter.WriteLine("drop TYPE [dbo].[" + aTypeName + "]");
                aWriter.WriteLine("GO");
                aWriter.WriteLine(" ");
            }
            catch
            {
                throw;
            }
        }
        private static void AddStoredProcedureDrop(string aTableName, StreamWriter aWriter)
        {
            try
            {
                aWriter.WriteLine("if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[" + aTableName + "]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)");
                aWriter.WriteLine("drop procedure [dbo].[" + aTableName + "]");
                aWriter.WriteLine("GO");
                aWriter.WriteLine(" ");
            }
            catch
            {
                throw;
            }
        }
        private static void Generate_Drop_Procedure(genOptions options)
        {
            try
            {
          
                AddStoredProcedureDrop(options.table, options.writer);
            }
            catch
            {
                throw;
            }
        }

        private static void Generate_MID_ST_HIS_DAY_TYPE(genOptions options)
        {
            ProfileList aDatabaseVariables = variables.GetStoreDailyHistoryDatabaseVariableList();
            StreamWriter aWriter = options.writer;
            string typeName = options.name;

            int count = 0;
            string line;
            try
            {
                AddTypeDrop(typeName, aWriter);

                aWriter.WriteLine("CREATE TYPE [dbo].[" + typeName + "] AS TABLE(");
                aWriter.WriteLine("  [HN_MOD] [int],");
                aWriter.WriteLine("  [HN_RID] [int],");
                aWriter.WriteLine("  [TIME_ID] [int],");
                aWriter.WriteLine("  [ST_RID] [int],");
                foreach (VariableProfile vp in aDatabaseVariables)
                {
                    ++count;
                    line = "  [" + vp.DatabaseColumnName + "] [";
                    switch (vp.StoreDatabaseVariableType)
                    {
                        case eVariableDatabaseType.Integer:
                            line += "int]";
                            break;
                        case eVariableDatabaseType.Float:
                            line += "float]";
                            break;
                        case eVariableDatabaseType.DateTime:
                            line += "smalldatetime]";
                            break;
                        case eVariableDatabaseType.Real:
                            line += "real]";
                            break;
                        case eVariableDatabaseType.BigInteger:
                            line += "bigint]";
                            break;
                    }
                    if (count < aDatabaseVariables.Count)
                    {
                        line += ", ";
                    }

                    aWriter.WriteLine(line);
                }
                aWriter.WriteLine("PRIMARY KEY (HN_MOD, HN_RID, TIME_ID, ST_RID)");
                aWriter.WriteLine(")");

                aWriter.WriteLine(System.Environment.NewLine);
                aWriter.WriteLine("GO");
                aWriter.WriteLine(System.Environment.NewLine);
            }
            catch
            {
                throw;
            }
        }
        private static void Generate_MID_ST_HIS_WK_TYPE(genOptions options)
        {
            ProfileList aDatabaseVariables = variables.GetStoreWeeklyHistoryDatabaseVariableList();
            StreamWriter aWriter = options.writer;

            int count = 0;
            string line;
            try
            {
                AddTypeDrop(Include.DBStoreWeeklyHistoryType, aWriter);

                aWriter.WriteLine("CREATE TYPE [dbo].[" + Include.DBStoreWeeklyHistoryType + "] AS TABLE(");
                aWriter.WriteLine("  [HN_MOD] [int],");
                aWriter.WriteLine("  [HN_RID] [int],");
                aWriter.WriteLine("  [TIME_ID] [int],");
                aWriter.WriteLine("  [ST_RID] [int],");
                foreach (VariableProfile vp in aDatabaseVariables)
                {
                    ++count;
                    line = "  [" + vp.DatabaseColumnName + "] [";
                    switch (vp.ChainDatabaseVariableType)
                    {
                        case eVariableDatabaseType.Integer:
                            line += "int]";
                            break;
                        case eVariableDatabaseType.Float:
                            line += "float]";
                            break;
                        case eVariableDatabaseType.DateTime:
                            line += "smalldatetime]";
                            break;
                        case eVariableDatabaseType.Real:
                            line += "real]";
                            break;
                        case eVariableDatabaseType.BigInteger:
                            line += "bigint]";
                            break;
                    }
                    if (count < aDatabaseVariables.Count)
                    {
                        line += ", ";
                    }

                    aWriter.WriteLine(line);
                }
                aWriter.WriteLine("PRIMARY KEY (HN_MOD, HN_RID, TIME_ID, ST_RID)");
                aWriter.WriteLine(")");

                aWriter.WriteLine(System.Environment.NewLine);
                aWriter.WriteLine("GO");
                aWriter.WriteLine(System.Environment.NewLine);
            }
            catch
            {
                throw;
            }
        }
        private static void Generate_MID_ST_FOR_WK_TYPE(genOptions options)
        {
            ProfileList aDatabaseVariables = variables.GetStoreWeeklyForecastDatabaseVariableList();
            StreamWriter aWriter = options.writer;

            int count = 0;
            string line;
            try
            {
                AddTypeDrop(Include.DBStoreWeeklyForecastType, aWriter);
                AddTypeDrop(Include.DBStoreWeeklyForecastLockType, aWriter);

                aWriter.WriteLine("CREATE TYPE [dbo].[" + Include.DBStoreWeeklyForecastType + "] AS TABLE(");
                aWriter.WriteLine("  [HN_MOD] [int],");
                aWriter.WriteLine("  [HN_RID] [int],");
                aWriter.WriteLine("  [FV_RID] [int],");
                aWriter.WriteLine("  [TIME_ID] [int],");
                aWriter.WriteLine("  [ST_RID] [int],");
                foreach (VariableProfile vp in aDatabaseVariables)
                {
                    ++count;
                    line = "  [" + vp.DatabaseColumnName + "] [";
                    switch (vp.ChainDatabaseVariableType)
                    {
                        case eVariableDatabaseType.Integer:
                            line += "int]";
                            break;
                        case eVariableDatabaseType.Float:
                            line += "float]";
                            break;
                        case eVariableDatabaseType.DateTime:
                            line += "smalldatetime]";
                            break;
                        case eVariableDatabaseType.Real:
                            line += "real]";
                            break;
                        case eVariableDatabaseType.BigInteger:
                            line += "bigint]";
                            break;
                    }
                    if (count < aDatabaseVariables.Count)
                    {
                        line += ", ";
                    }

                    aWriter.WriteLine(line);
                }
                aWriter.WriteLine("PRIMARY KEY (HN_MOD, HN_RID, FV_RID, TIME_ID, ST_RID)");
                aWriter.WriteLine(")");

                aWriter.WriteLine(System.Environment.NewLine);
                aWriter.WriteLine("GO");
                aWriter.WriteLine(System.Environment.NewLine);

                count = 0;
                aWriter.WriteLine("CREATE TYPE [dbo].[" + Include.DBStoreWeeklyForecastLockType + "] AS TABLE(");
                aWriter.WriteLine("  [HN_RID] [int],");
                aWriter.WriteLine("  [FV_RID] [int],");
                aWriter.WriteLine("  [TIME_ID] [int],");
                aWriter.WriteLine("  [ST_RID] [int],");
                foreach (VariableProfile vp in aDatabaseVariables)
                {
                    ++count;
                    line = "  [" + vp.DatabaseColumnName + "_LOCK] [char]";
                    if (count < aDatabaseVariables.Count)
                    {
                        line += ", ";
                    }

                    aWriter.WriteLine(line);
                }
                aWriter.WriteLine("PRIMARY KEY (HN_RID, FV_RID, TIME_ID, ST_RID)");
                aWriter.WriteLine(")");

                aWriter.WriteLine(System.Environment.NewLine);
                aWriter.WriteLine("GO");
                aWriter.WriteLine(System.Environment.NewLine);
            }
            catch
            {
                throw;
            }
        }
        private static void Generate_MID_ST_READ_TYPES(genOptions options)
        {
            StreamWriter aWriter = options.writer;
            try
            {
                AddTypeDrop(Include.DBStoreWeeklyHistoryReadType, aWriter);
                AddTypeDrop(Include.DBStoreDailyHistoryReadType, aWriter);
                AddTypeDrop(Include.DBStoreWeeklyModifiedReadType, aWriter);
                AddTypeDrop(Include.DBStoreWeeklyForecastReadType, aWriter);

                aWriter.WriteLine("CREATE TYPE [dbo].[" + Include.DBStoreWeeklyHistoryReadType + "] AS TABLE(");
                aWriter.WriteLine("  [HN_MOD] [int],");
                aWriter.WriteLine("  [HN_RID] [int],");
                aWriter.WriteLine("  [TIME_ID] [int],");

                aWriter.WriteLine("PRIMARY KEY (HN_MOD, HN_RID, TIME_ID)");
                aWriter.WriteLine(")");

                aWriter.WriteLine(System.Environment.NewLine);
                aWriter.WriteLine("GO");
                aWriter.WriteLine(System.Environment.NewLine);

                aWriter.WriteLine("CREATE TYPE [dbo].[" + Include.DBStoreDailyHistoryReadType + "] AS TABLE(");
                aWriter.WriteLine("  [HN_MOD] [int],");
                aWriter.WriteLine("  [HN_RID] [int],");
                aWriter.WriteLine("  [TIME_ID] [int],");

                aWriter.WriteLine("PRIMARY KEY (HN_MOD, HN_RID, TIME_ID)");
                aWriter.WriteLine(")");

                aWriter.WriteLine(System.Environment.NewLine);
                aWriter.WriteLine("GO");
                aWriter.WriteLine(System.Environment.NewLine);

                aWriter.WriteLine("CREATE TYPE [dbo].[" + Include.DBStoreWeeklyModifiedReadType + "] AS TABLE(");
                aWriter.WriteLine("  [HN_MOD] [int],");
                aWriter.WriteLine("  [HN_RID] [int],");
                aWriter.WriteLine("  [FV_RID] [int],");
                aWriter.WriteLine("  [TIME_ID] [int],");

                aWriter.WriteLine("PRIMARY KEY (HN_MOD, HN_RID, FV_RID, TIME_ID)");
                aWriter.WriteLine(")");

                aWriter.WriteLine(System.Environment.NewLine);
                aWriter.WriteLine("GO");
                aWriter.WriteLine(System.Environment.NewLine);

                aWriter.WriteLine("CREATE TYPE [dbo].[" + Include.DBStoreWeeklyForecastReadType + "] AS TABLE(");
                aWriter.WriteLine("  [HN_MOD] [int],");
                aWriter.WriteLine("  [HN_RID] [int],");
                aWriter.WriteLine("  [FV_RID] [int],");
                aWriter.WriteLine("  [TIME_ID] [int],");

                aWriter.WriteLine("PRIMARY KEY (HN_MOD, HN_RID, FV_RID, TIME_ID)");
                aWriter.WriteLine(")");

                aWriter.WriteLine(System.Environment.NewLine);
                aWriter.WriteLine("GO");
                aWriter.WriteLine(System.Environment.NewLine);


            }
            catch
            {
                throw;
            }
        }
        private static void Generate_TYPE_DROPS(genOptions options)
        {
            StreamWriter aWriter = options.writer;
            try
            {
                AddTypeDrop("MID_CHN_FOR_WK_READ_LOCK_TYPE", aWriter);
                AddTypeDrop("MID_CHN_FOR_WK_READ_TYPE", aWriter);
                AddTypeDrop("MID_CHN_HIS_WK_READ_TYPE", aWriter);
                AddTypeDrop("MID_CHN_MOD_WK_READ_TYPE", aWriter);
                AddTypeDrop("MID_ST_FOR_WK_READ_LOCK_TYPE", aWriter);
                AddTypeDrop("MID_ST_FOR_WK_READ_TYPE", aWriter);
                AddTypeDrop("MID_ST_HIS_DAY_READ_TYPE", aWriter);
                AddTypeDrop("MID_ST_HIS_WK_READ_TYPE", aWriter);
                AddTypeDrop("MID_ST_MOD_WK_READ_TYPE", aWriter);
            }
            catch
            {
                throw;
            }
        }
        private static void Generate_MID_ST_HIS_DAY_WRITE(genOptions options)
        {
            string procedureName = options.name;
            string tableName = options.table;
            ProfileList aDatabaseVariables = variables.GetStoreDailyHistoryDatabaseVariableList();
            StreamWriter aWriter = options.writer;


            string line;
            int count = 0;
            try
            {
               

                if (aDatabaseVariables == null ||
                    aDatabaseVariables.Count == 0)
                {
                    return;
                }

                aWriter.WriteLine("CREATE PROCEDURE [dbo].[" + procedureName + "]");
                aWriter.WriteLine(" ");
                aWriter.WriteLine("@dt " + Include.DBStoreDailyHistoryType + " READONLY");
                aWriter.WriteLine("AS");
                aWriter.WriteLine("SET NOCOUNT ON");

                aWriter.WriteLine("MERGE INTO " + tableName + " with (ROWLOCK) AS TARGET");
                aWriter.WriteLine("USING @dt AS SOURCE");
                aWriter.WriteLine("on (SOURCE.HN_MOD = TARGET.HN_MOD and SOURCE.HN_RID = TARGET.HN_RID and SOURCE.TIME_ID = TARGET.TIME_ID and SOURCE.ST_RID = TARGET.ST_RID)");
                aWriter.WriteLine("WHEN MATCHED THEN");
                aWriter.WriteLine("UPDATE ");
                count = 0;
                line = _blankLine;
                foreach (VariableProfile vp in aDatabaseVariables)
                {
                    ++count;
                    if (count == 1)
                    {
                        line = line.Insert(9, "SET");
                    }
                    line = line.Insert(13, "TARGET." + vp.DatabaseColumnName);
                    line = line.TrimEnd();
                    line += " = COALESCE(SOURCE." + vp.DatabaseColumnName + ", ";
                    line += "TARGET." + vp.DatabaseColumnName + ")";
                    if (count < aDatabaseVariables.Count)
                    {
                        line += ",";
                    }
                    aWriter.WriteLine(line);
                    line = _blankLine;
                }

                aWriter.WriteLine("WHEN NOT MATCHED BY TARGET THEN");
                line = "INSERT (HN_MOD, HN_RID, TIME_ID, ST_RID, ";
                count = 0;
                foreach (VariableProfile vp in aDatabaseVariables)
                {
                    ++count;
                    line += vp.DatabaseColumnName;
                    if (count < aDatabaseVariables.Count)
                    {
                        line += ", ";
                    }
                    if (line.Length > 150)
                    {
                        aWriter.WriteLine(line);
                        line = _indent5;

                    }
                }
                line += ")";
                aWriter.WriteLine(line);

                count = 0;
                line = "VALUES ( SOURCE.HN_MOD, SOURCE.HN_RID, SOURCE.TIME_ID, SOURCE.ST_RID, ";
                foreach (VariableProfile vp in aDatabaseVariables)
                {
                    ++count;
                    line += " SOURCE." + vp.DatabaseColumnName;
                    if (count < aDatabaseVariables.Count)
                    {
                        line += ",";
                    }
                    if (line.Length > 110)
                    {
                        aWriter.WriteLine(line);
                        line = _indent5;

                    }
                }
                line += ");";
                if (line.Length > 0)
                {
                    aWriter.WriteLine(line);
                }

                aWriter.WriteLine(System.Environment.NewLine);
                aWriter.WriteLine("GO");
                aWriter.WriteLine(System.Environment.NewLine);
            }
            catch
            {
                throw;
            }
        }
        private static void Generate_MID_ST_HIS_DAY_READ(genOptions options)
        {
            try
            {
                string procedureName = options.name;
                string tableName = options.table;
                ProfileList aDatabaseVariables = variables.GetStoreDailyHistoryDatabaseVariableList(); 
                StreamWriter aWriter = options.writer;

                if (aDatabaseVariables == null ||
                    aDatabaseVariables.Count == 0)
                {
                    return;
                }
                string aTempTableName = Include.DBTempTableName;
                string tempTableName = "@dt";
                string aViewName = Include.DBStoreDailyHistoryView;

                aWriter.WriteLine("CREATE PROCEDURE [dbo].[" + procedureName + "]");
                aWriter.WriteLine("@dt " + Include.DBStoreDailyHistoryReadType + " READONLY,");
                aWriter.WriteLine("@Rollup CHAR(1) = NULL");
                aWriter.WriteLine("AS");
                aWriter.WriteLine("SET NOCOUNT ON");
                aWriter.WriteLine("DECLARE @Tables INT,");
                aWriter.WriteLine("        @Loop INT,");
                aWriter.WriteLine("        @HN_TYPE INT,");
                aWriter.WriteLine("        @HN_RID INT,");
                aWriter.WriteLine("        @HN_MOD INT,");
                aWriter.WriteLine("        @ROLL_OPTION INT,");
                aWriter.WriteLine("        @LoopCount INT,");
                aWriter.WriteLine("        @NextLoopCount INT");
                //aWriter.WriteLine("SELECT @Tables = " + aTableCountColumn + " FROM SYSTEM_OPTIONS");
                aWriter.WriteLine("SET @Tables = 10;");

                aWriter.WriteLine("SELECT DISTINCT @HN_TYPE = ph.PH_TYPE, @HN_RID = t.HN_RID, @HN_MOD = t.HN_RID % @Tables,");
                aWriter.WriteLine("                @ROLL_OPTION = COALESCE(ph.HISTORY_ROLL_OPTION, 800251)");
                aWriter.WriteLine("        FROM HIERARCHY_NODE hn (NOLOCK)");
                aWriter.WriteLine("        JOIN PRODUCT_HIERARCHY ph (NOLOCK) ON ph.PH_RID = hn.HOME_PH_RID");
                aWriter.WriteLine("        JOIN " + tempTableName + " t on t.HN_RID = hn.HN_RID");
                aWriter.WriteLine("IF @HN_TYPE = 800001 and @ROLL_OPTION = 800251 -- Realtime");
                aWriter.WriteLine("   BEGIN");
                aWriter.WriteLine("   SET @LoopCount = 0");
                aWriter.WriteLine("   CREATE TABLE #TREE (LOOPCOUNT INT NOT NULL, PARENT_HN_RID INT NOT NULL, HOME_PH_RID INT, PH_TYPE INT, CHILD_HN_RID INT NOT NULL, CHILD_HN_MOD INT NOT NULL)");
                aWriter.WriteLine("   -- insert the children of the node into the temp table");
                aWriter.WriteLine("   INSERT #TREE (LOOPCOUNT, PARENT_HN_RID, HOME_PH_RID, PH_TYPE, CHILD_HN_RID, CHILD_HN_MOD) ");
                aWriter.WriteLine("       select @LoopCount as LOOPCOUNT, @HN_RID as PARENT_HN_RID, hn.HOME_PH_RID, ph.PH_TYPE, hnj.HN_RID, hnj.HN_RID % @Tables");
                aWriter.WriteLine("         from HIER_NODE_JOIN hnj (NOLOCK)");
                aWriter.WriteLine("           JOIN HIERARCHY_NODE hn (NOLOCK) ON hn.HN_RID = hnj.HN_RID");
                aWriter.WriteLine("           JOIN PRODUCT_HIERARCHY ph (NOLOCK) ON ph.PH_RID = hn.HOME_PH_RID");
                aWriter.WriteLine("       where @HN_RID = hnj.PARENT_HN_RID");
                aWriter.WriteLine("   SET @Loop = @@ROWCOUNT");
                aWriter.WriteLine("   -- chase all paths until you get the main hierarchy (type 800000) or the lowest leaf");
                aWriter.WriteLine("   SET @NextLoopCount = @LoopCount + 1");
                aWriter.WriteLine("   WHILE @Loop > 0");
                aWriter.WriteLine("   BEGIN");
                aWriter.WriteLine("      INSERT #TREE");
                aWriter.WriteLine("        select @NextLoopCount as LOOPCOUNT, hnj.PARENT_HN_RID, hn.HOME_PH_RID, ph.PH_TYPE, hnj.HN_RID, hnj.HN_RID % @Tables");
                aWriter.WriteLine("          from HIER_NODE_JOIN hnj (NOLOCK)");
                aWriter.WriteLine("            JOIN HIERARCHY_NODE hn (NOLOCK) ON hn.HN_RID = hnj.HN_RID");
                aWriter.WriteLine("            JOIN PRODUCT_HIERARCHY ph (NOLOCK) ON ph.PH_RID = hn.HOME_PH_RID");
                aWriter.WriteLine("            JOIN #TREE t ON hnj.PARENT_HN_RID = t.CHILD_HN_RID");
                aWriter.WriteLine("        WHERE t.LOOPCOUNT =  @LoopCount AND t.PH_TYPE <> 800000");
                aWriter.WriteLine("      SET @Loop = @@ROWCOUNT");
                aWriter.WriteLine("      SET @LoopCount = @LoopCount + 1");
                aWriter.WriteLine("      SET @NextLoopCount = @LoopCount + 1");
                aWriter.WriteLine("   END");
                aWriter.WriteLine("   -- join with dates from xml selecting only nodes from the main hierarchy or lowest leaf alternates");
                aWriter.WriteLine("   SELECT * ");
                aWriter.WriteLine("     INTO " + aTempTableName + "2");
                aWriter.WriteLine("     FROM #TREE");
                aWriter.WriteLine("     CROSS JOIN " + tempTableName);
                aWriter.WriteLine("   where LOOPCOUNT = @LoopCount - 1");
                aWriter.WriteLine("	     or PH_TYPE = 800000");

                int count = 0;
                string line = "       SELECT @HN_RID AS HN_RID, 1 AS FV_RID, ST_RID, shd.TIME_ID, ";
                // add variables
                foreach (VariableProfile vp in aDatabaseVariables)
                {
                    ++count;
                    line += "COALESCE(SUM(" + vp.DatabaseColumnName + "), 0) " + vp.DatabaseColumnName + ", ";
                    if (line.Length > 100)
                    {
                        aWriter.WriteLine(line);
                        line = _indent10;

                    }

                }
                if (line.Length > 0)
                {
                    aWriter.WriteLine(line);

                }
                // add locks
                count = 0;
                line = _indent10;
                // add variables
                foreach (VariableProfile vp in aDatabaseVariables)
                {
                    ++count;
                    line += "0 AS " + vp.DatabaseColumnName + Include.cLockExtension;
                    if (count < aDatabaseVariables.Count)
                    {
                        line += ", ";
                    }
                    if (line.Length > 100)
                    {
                        aWriter.WriteLine(line);
                        line = _indent10;

                    }

                }
                if (line.Length > 0)
                {
                    aWriter.WriteLine(line);

                }
                aWriter.WriteLine("		FROM " + aTempTableName + "2 t");
                aWriter.WriteLine("		JOIN " + aViewName + " shd (NOLOCK) ON t.CHILD_HN_RID = shd.HN_RID AND shd.ST_RID > 0");
                aWriter.WriteLine("			AND shd.TIME_ID = t.TIME_ID AND t.CHILD_HN_MOD = shd.HN_MOD ");
                aWriter.WriteLine("		GROUP BY ST_RID, shd.TIME_ID");
                aWriter.WriteLine("		OPTION (MAXDOP 1)");
                aWriter.WriteLine("	RETURN 0");
                aWriter.WriteLine("	END");
                aWriter.WriteLine("-- Process variables");
                aWriter.WriteLine("-- GET ALL THE ROWS");
                aWriter.WriteLine("IF @Rollup = 'Y'");
                count = 0;
                line = "       SELECT shd.HN_RID, 1 AS FV_RID, shd.ST_RID, ";
                // add variables
                foreach (VariableProfile vp in aDatabaseVariables)
                {
                    ++count;
                    line += "COALESCE(SUM(shd." + vp.DatabaseColumnName + "), 0) " + vp.DatabaseColumnName + ", ";
                    if (line.Length > 90)
                    {
                        aWriter.WriteLine(line);
                        line = _indent10;

                    }

                }
                if (line.Length > 0)
                {
                    aWriter.WriteLine(line);

                }
                // add locks
                count = 0;
                line = _indent10;
                // add variables
                foreach (VariableProfile vp in aDatabaseVariables)
                {
                    ++count;
                    line += "0 AS " + vp.DatabaseColumnName + Include.cLockExtension;
                    if (count < aDatabaseVariables.Count)
                    {
                        line += ", ";
                    }
                    if (line.Length > 90)
                    {
                        aWriter.WriteLine(line);
                        line = _indent10;

                    }

                }
                if (line.Length > 0)
                {
                    aWriter.WriteLine(line);

                }
                aWriter.WriteLine("        FROM " + tempTableName + " xml");
                aWriter.WriteLine("	JOIN " + aViewName + " shd (NOLOCK) ON xml.HN_RID = shd.HN_RID");
                aWriter.WriteLine("		AND shd.TIME_ID = xml.TIME_ID");
                aWriter.WriteLine("		AND shd.HN_MOD = xml.HN_MOD");
                aWriter.WriteLine("	GROUP BY shd.HN_RID, shd.ST_RID");
                aWriter.WriteLine("	ORDER BY shd.ST_RID");
                aWriter.WriteLine("	OPTION (MAXDOP 1)");
                aWriter.WriteLine("ELSE");
                count = 0;
                line = "       SELECT shd.HN_RID, 1 AS FV_RID, shd.TIME_ID, shd.ST_RID, ";
                // add variables
                foreach (VariableProfile vp in aDatabaseVariables)
                {
                    ++count;
                    line += "COALESCE(shd." + vp.DatabaseColumnName + ", 0) " + vp.DatabaseColumnName + ", ";
                    if (line.Length > 90)
                    {
                        aWriter.WriteLine(line);
                        line = _indent10;

                    }

                }
                if (line.Length > 0)
                {
                    aWriter.WriteLine(line);

                }
                // add locks
                count = 0;
                line = _indent10;
                foreach (VariableProfile vp in aDatabaseVariables)
                {
                    ++count;
                    line += "0 AS " + vp.DatabaseColumnName + Include.cLockExtension;
                    if (count < aDatabaseVariables.Count)
                    {
                        line += ", ";
                    }
                    if (line.Length > 90)
                    {
                        aWriter.WriteLine(line);
                        line = _indent10;

                    }

                }
                if (line.Length > 0)
                {
                    aWriter.WriteLine(line);

                }
                aWriter.WriteLine("		FROM " + tempTableName + " xml");
                aWriter.WriteLine("	JOIN " + tableName + " shd (NOLOCK) ON xml.HN_RID = shd.HN_RID AND shd.HN_MOD = xml.HN_MOD");
                aWriter.WriteLine("		AND shd.TIME_ID = xml.TIME_ID");
                aWriter.WriteLine("		AND shd.HN_MOD = xml.HN_MOD");
                aWriter.WriteLine("	OPTION (MAXDOP 1)");

                aWriter.WriteLine(System.Environment.NewLine);
                aWriter.WriteLine("if (select object_id('tempdb.dbo." + aTempTableName + "2')) > 0 DROP TABLE " + aTempTableName + "2");
                aWriter.WriteLine(System.Environment.NewLine);

                aWriter.WriteLine(System.Environment.NewLine);
                aWriter.WriteLine("GO");
                aWriter.WriteLine(System.Environment.NewLine);
            }
            catch
            {
                throw;
            }
        }
        private static void Generate_MID_ST_HIS_DAY_ROLLUP(genOptions options)
        {
            StoreHistoryDayRollupProcess rollupProcess;
            StoreDayWeekRollupProcess dayToWeekrollupProcess;
            string procedureName;
            bool includeZeroInAverage = true;
            bool honorLocks = false;
            bool zeroParentsWithNoChildren = true;
            ArrayList databaseVariables;

            try
            {
                ProfileList aWeeklyDatabaseVariables = new ProfileList(eProfileType.Variable);               
                foreach (VariableProfile vp in variables.GetStoreWeeklyHistoryDatabaseVariableList())
                {
                    if (vp.LevelRollType != eLevelRollType.None &&
                        vp.StoreWeeklyHistoryModelType != eVariableDatabaseModelType.None &&
                        (vp.VariableCategory == eVariableCategory.Store || vp.VariableCategory == eVariableCategory.Both))
                    {
                        aWeeklyDatabaseVariables.Add(vp);
                    }
                }

                ProfileList aDailyDatabaseVariables = new ProfileList(eProfileType.Variable);
                foreach (VariableProfile vp in variables.GetStoreDailyHistoryDatabaseVariableList())
                {
                    if (vp.LevelRollType != eLevelRollType.None &&
                        vp.StoreDailyHistoryModelType != eVariableDatabaseModelType.None &&
                        (vp.VariableCategory == eVariableCategory.Store || vp.VariableCategory == eVariableCategory.Both))
                    {
                        aDailyDatabaseVariables.Add(vp);
                    }
                }



                StreamWriter aWriter = options.writer;
                int aTableNumber = options.counter;



                MIDRetail.Business.Rollup _rollup = new Rollup(null, 0, 0, false, false, false);


                procedureName = Include.DBStoreDailyHistoryRollupSP.Replace(Include.DBTableCountReplaceString, aTableNumber.ToString());
                AddStoredProcedureDrop(procedureName, aWriter);
                rollupProcess = new StoreHistoryDayRollupProcess(null, null, aDailyDatabaseVariables.ArrayList, 0, 0, 0, 0, aTableNumber, aTableNumber, includeZeroInAverage, honorLocks, zeroParentsWithNoChildren, true, null);
                aWriter.Write(rollupProcess.BuildStoredProcedure(procedureName));
                aWriter.WriteLine(System.Environment.NewLine);
                aWriter.WriteLine("GO");
                aWriter.WriteLine(System.Environment.NewLine);

                zeroParentsWithNoChildren = false;
                procedureName = Include.DBStoreDailyHistoryNoZeroRollupSP.Replace(Include.DBTableCountReplaceString, aTableNumber.ToString());
                AddStoredProcedureDrop(procedureName, aWriter);
                rollupProcess = new StoreHistoryDayRollupProcess(null, null, aDailyDatabaseVariables.ArrayList, 0, 0, 0, 0, aTableNumber, aTableNumber, includeZeroInAverage, honorLocks, zeroParentsWithNoChildren, true, null);
                aWriter.Write(rollupProcess.BuildStoredProcedure(procedureName));
                aWriter.WriteLine(System.Environment.NewLine);
                aWriter.WriteLine("GO");
                aWriter.WriteLine(System.Environment.NewLine);

                zeroParentsWithNoChildren = true;
                procedureName = Include.DBStoreDayToWeekHistoryRollupSP.Replace(Include.DBTableCountReplaceString, aTableNumber.ToString());
                AddStoredProcedureDrop(procedureName, aWriter);
                databaseVariables = new ArrayList();
                _rollup.BuildIntersectionOfVariableLists(aDailyDatabaseVariables.ArrayList, aWeeklyDatabaseVariables.ArrayList, databaseVariables, eRollType.storeDailyHistoryToWeeks);
                dayToWeekrollupProcess = new StoreDayWeekRollupProcess(null, null, databaseVariables, 0, 0, 0, 0, aTableNumber, aTableNumber, includeZeroInAverage, honorLocks, zeroParentsWithNoChildren, true, null);
                aWriter.Write(dayToWeekrollupProcess.BuildStoredProcedure(procedureName));
                aWriter.WriteLine(System.Environment.NewLine);
                aWriter.WriteLine("GO");
                aWriter.WriteLine(System.Environment.NewLine);
            }
            catch
            {
                throw;
            }
        }
        private static void Generate_MID_ST_HIS_WK_WRITE(genOptions options)
        {
            string line;
            int count = 0;
            try
            {

                ProfileList aDatabaseVariables = variables.GetStoreWeeklyHistoryDatabaseVariableList();
                StreamWriter aWriter = options.writer;
                string procedureName = options.name;
                string tableName = options.table;

                if (aDatabaseVariables == null ||
                    aDatabaseVariables.Count == 0)
                {
                    return;
                }

                aWriter.WriteLine("CREATE PROCEDURE [dbo].[" + procedureName + "]");
                aWriter.WriteLine(" ");
                aWriter.WriteLine("@dt " + Include.DBStoreWeeklyHistoryType + " READONLY");
                aWriter.WriteLine("AS");
                aWriter.WriteLine("SET NOCOUNT ON");

                aWriter.WriteLine("MERGE INTO " + tableName + " with (ROWLOCK) AS TARGET");
                aWriter.WriteLine("USING @dt AS SOURCE");
                aWriter.WriteLine("on (SOURCE.HN_MOD = TARGET.HN_MOD and SOURCE.HN_RID = TARGET.HN_RID and SOURCE.TIME_ID = TARGET.TIME_ID and SOURCE.ST_RID = TARGET.ST_RID)");
                aWriter.WriteLine("WHEN MATCHED THEN");
                aWriter.WriteLine("UPDATE ");
                count = 0;
                line = _blankLine;
                foreach (VariableProfile vp in aDatabaseVariables)
                {
                    ++count;
                    if (count == 1)
                    {
                        line = line.Insert(9, "SET");
                    }
                    line = line.Insert(13, "TARGET." + vp.DatabaseColumnName);
                    line = line.TrimEnd();
                    line += " = COALESCE(SOURCE." + vp.DatabaseColumnName + ", ";
                    line += "TARGET." + vp.DatabaseColumnName + ")";
                    if (count < aDatabaseVariables.Count)
                    {
                        line += ",";
                    }
                    aWriter.WriteLine(line);
                    line = _blankLine;
                }

                aWriter.WriteLine("WHEN NOT MATCHED BY TARGET THEN");
                line = "INSERT (HN_MOD, HN_RID, TIME_ID, ST_RID, ";
                count = 0;
                foreach (VariableProfile vp in aDatabaseVariables)
                {
                    ++count;
                    line += vp.DatabaseColumnName;
                    if (count < aDatabaseVariables.Count)
                    {
                        line += ", ";
                    }
                    if (line.Length > 150)
                    {
                        aWriter.WriteLine(line);
                        line = _indent5;

                    }
                }
                line += ")";
                aWriter.WriteLine(line);

                count = 0;
                line = "VALUES ( SOURCE.HN_MOD, SOURCE.HN_RID, SOURCE.TIME_ID, SOURCE.ST_RID, ";
                foreach (VariableProfile vp in aDatabaseVariables)
                {
                    ++count;
                    line += " SOURCE." + vp.DatabaseColumnName;
                    if (count < aDatabaseVariables.Count)
                    {
                        line += ",";
                    }
                    if (line.Length > 110)
                    {
                        aWriter.WriteLine(line);
                        line = _indent5;

                    }
                }
                line += ");";
                if (line.Length > 0)
                {
                    aWriter.WriteLine(line);
                }

                aWriter.WriteLine(System.Environment.NewLine);
                aWriter.WriteLine("GO");
                aWriter.WriteLine(System.Environment.NewLine);
            }
            catch
            {
                throw;
            }
        }
        private static void Generate_MID_ST_HIS_WK_READ(genOptions options)
        {
            try
            {
                int count = 0;
                string line = string.Empty;
                string procedureName = options.name;
                string tableName = options.table;


                string aViewName = Include.DBStoreWeeklyHistoryView;
                string aTempTableName = Include.DBTempTableName;
                ProfileList aDatabaseVariables = variables.GetStoreWeeklyHistoryDatabaseVariableList();
                StreamWriter aWriter = options.writer;


                if (aDatabaseVariables == null ||
                    aDatabaseVariables.Count == 0)
                {
                    return;
                }

                string tempTableName = "@dt";

                aWriter.WriteLine("CREATE PROCEDURE [dbo].[" + procedureName + "]");
                aWriter.WriteLine("@dt " + Include.DBStoreWeeklyHistoryReadType + " READONLY,");
                aWriter.WriteLine("@Rollup CHAR(1) = NULL");
                aWriter.WriteLine("AS");
                aWriter.WriteLine("SET NOCOUNT ON");
                aWriter.WriteLine("DECLARE @Tables INT,");
                aWriter.WriteLine("        @Loop INT,");
                aWriter.WriteLine("        @HN_TYPE INT,");
                aWriter.WriteLine("        @HN_RID INT,");
                aWriter.WriteLine("        @HN_MOD INT,");
                aWriter.WriteLine("        @ROLL_OPTION INT,");
                aWriter.WriteLine("        @LoopCount INT,");
                aWriter.WriteLine("        @NextLoopCount INT");
                //aWriter.WriteLine("SELECT @Tables = " + aTableCountColumn + " FROM SYSTEM_OPTIONS");
                aWriter.WriteLine("SET @Tables = 10");

                aWriter.WriteLine("SELECT DISTINCT @HN_TYPE = ph.PH_TYPE, @HN_RID = t.HN_RID, @HN_MOD = t.HN_RID % @Tables,");
                aWriter.WriteLine("                @ROLL_OPTION = COALESCE(ph.HISTORY_ROLL_OPTION, 800251)");
                aWriter.WriteLine("        FROM HIERARCHY_NODE hn (NOLOCK)");
                aWriter.WriteLine("        JOIN PRODUCT_HIERARCHY ph (NOLOCK) ON ph.PH_RID = hn.HOME_PH_RID");
                aWriter.WriteLine("        JOIN " + tempTableName + " t on t.HN_RID = hn.HN_RID");
                aWriter.WriteLine("IF @HN_TYPE = 800001 and @ROLL_OPTION = 800251 -- Realtime");
                aWriter.WriteLine("   BEGIN");
                aWriter.WriteLine("   SET @LoopCount = 0");
                aWriter.WriteLine("   CREATE TABLE #TREE (LOOPCOUNT INT NOT NULL, PARENT_HN_RID INT NOT NULL, HOME_PH_RID INT, PH_TYPE INT, CHILD_HN_RID INT NOT NULL, CHILD_HN_MOD INT NOT NULL)");
                aWriter.WriteLine("   -- insert the children of the node into the temp table");
                aWriter.WriteLine("   INSERT #TREE (LOOPCOUNT, PARENT_HN_RID, HOME_PH_RID, PH_TYPE, CHILD_HN_RID, CHILD_HN_MOD) ");
                aWriter.WriteLine("       select @LoopCount as LOOPCOUNT, @HN_RID as PARENT_HN_RID, hn.HOME_PH_RID, ph.PH_TYPE, hnj.HN_RID, hnj.HN_RID % @Tables");
                aWriter.WriteLine("         from HIER_NODE_JOIN hnj (NOLOCK)");
                aWriter.WriteLine("           JOIN HIERARCHY_NODE hn (NOLOCK) ON hn.HN_RID = hnj.HN_RID");
                aWriter.WriteLine("           JOIN PRODUCT_HIERARCHY ph (NOLOCK) ON ph.PH_RID = hn.HOME_PH_RID");
                aWriter.WriteLine("       where @HN_RID = hnj.PARENT_HN_RID");
                aWriter.WriteLine("   SET @Loop = @@ROWCOUNT");
                aWriter.WriteLine("   -- chase all paths until you get the main hierarchy (type 800000) or the lowest leaf");
                aWriter.WriteLine("   SET @NextLoopCount = @LoopCount + 1");
                aWriter.WriteLine("   WHILE @Loop > 0");
                aWriter.WriteLine("   BEGIN");
                aWriter.WriteLine("      INSERT #TREE");
                aWriter.WriteLine("        select @NextLoopCount as LOOPCOUNT, hnj.PARENT_HN_RID, hn.HOME_PH_RID, ph.PH_TYPE, hnj.HN_RID, hnj.HN_RID % @Tables");
                aWriter.WriteLine("          from HIER_NODE_JOIN hnj (NOLOCK)");
                aWriter.WriteLine("            JOIN HIERARCHY_NODE hn (NOLOCK) ON hn.HN_RID = hnj.HN_RID");
                aWriter.WriteLine("            JOIN PRODUCT_HIERARCHY ph (NOLOCK) ON ph.PH_RID = hn.HOME_PH_RID");
                aWriter.WriteLine("            JOIN #TREE t ON hnj.PARENT_HN_RID = t.CHILD_HN_RID");
                aWriter.WriteLine("        WHERE t.LOOPCOUNT =  @LoopCount AND t.PH_TYPE <> 800000");
                aWriter.WriteLine("      SET @Loop = @@ROWCOUNT");
                aWriter.WriteLine("      SET @LoopCount = @LoopCount + 1");
                aWriter.WriteLine("      SET @NextLoopCount = @LoopCount + 1");
                aWriter.WriteLine("   END");
                aWriter.WriteLine("   -- join with dates from xml selecting only nodes from the main hierarchy or lowest leaf alternates");
                aWriter.WriteLine("   SELECT * ");
                aWriter.WriteLine("     INTO " + aTempTableName + "2");
                aWriter.WriteLine("     FROM #TREE");
                aWriter.WriteLine("     CROSS JOIN " + tempTableName);
                aWriter.WriteLine("   where LOOPCOUNT = @LoopCount - 1");
                aWriter.WriteLine("	     or PH_TYPE = 800000");

                count = 0;
                line = "       SELECT @HN_RID AS HN_RID, 1 AS FV_RID, ST_RID, shw.TIME_ID, ";
                // add variables
                foreach (VariableProfile vp in aDatabaseVariables)
                {
                    ++count;
                    line += "COALESCE(SUM(" + vp.DatabaseColumnName + "), 0) " + vp.DatabaseColumnName + ", ";
                    if (line.Length > 100)
                    {
                        aWriter.WriteLine(line);
                        line = _indent10;

                    }

                }
                if (line.Length > 0)
                {
                    aWriter.WriteLine(line);

                }
                // add locks
                count = 0;
                line = _indent10;
                // add variables
                foreach (VariableProfile vp in aDatabaseVariables)
                {
                    ++count;
                    line += "0 AS " + vp.DatabaseColumnName + Include.cLockExtension;
                    if (count < aDatabaseVariables.Count)
                    {
                        line += ", ";
                    }
                    if (line.Length > 100)
                    {
                        aWriter.WriteLine(line);
                        line = _indent10;

                    }

                }
                if (line.Length > 0)
                {
                    aWriter.WriteLine(line);

                }
                aWriter.WriteLine("		FROM " + aTempTableName + "2 t");
                aWriter.WriteLine("		JOIN " + aViewName + " shw (NOLOCK) ON t.CHILD_HN_RID = shw.HN_RID AND shw.ST_RID > 0");
                aWriter.WriteLine("			AND shw.TIME_ID = t.TIME_ID AND t.CHILD_HN_MOD = shw.HN_MOD ");
                aWriter.WriteLine("		GROUP BY ST_RID, shw.TIME_ID");
                aWriter.WriteLine("		OPTION (MAXDOP 1)");
                aWriter.WriteLine("	RETURN 0");
                aWriter.WriteLine("	END");
                aWriter.WriteLine("-- Process variables");
                aWriter.WriteLine("-- GET ALL THE ROWS");
                aWriter.WriteLine("IF @Rollup = 'Y'");
                count = 0;
                line = "       SELECT shw.HN_RID, 1 AS FV_RID, shw.ST_RID, ";
                // add variables
                foreach (VariableProfile vp in aDatabaseVariables)
                {
                    ++count;
                    line += "COALESCE(SUM(shw." + vp.DatabaseColumnName + "), 0) " + vp.DatabaseColumnName + ", ";
                    if (line.Length > 90)
                    {
                        aWriter.WriteLine(line);
                        line = _indent10;

                    }

                }
                if (line.Length > 0)
                {
                    aWriter.WriteLine(line);

                }
                // add locks
                count = 0;
                line = _indent10;
                // add variables
                foreach (VariableProfile vp in aDatabaseVariables)
                {
                    ++count;
                    line += "0 AS " + vp.DatabaseColumnName + Include.cLockExtension;
                    if (count < aDatabaseVariables.Count)
                    {
                        line += ", ";
                    }
                    if (line.Length > 90)
                    {
                        aWriter.WriteLine(line);
                        line = _indent10;

                    }

                }
                if (line.Length > 0)
                {
                    aWriter.WriteLine(line);

                }
                aWriter.WriteLine("        FROM " + tempTableName + " xml");
                aWriter.WriteLine("	JOIN " + aViewName + " shw (NOLOCK) ON xml.HN_RID = shw.HN_RID");
                aWriter.WriteLine("		AND shw.TIME_ID = xml.TIME_ID");
                aWriter.WriteLine("		AND shw.HN_MOD = xml.HN_MOD");
                aWriter.WriteLine("	GROUP BY shw.HN_RID, shw.ST_RID");
                aWriter.WriteLine("	ORDER BY shw.ST_RID");
                aWriter.WriteLine("	OPTION (MAXDOP 1)");
                aWriter.WriteLine("ELSE");
                count = 0;
                line = "       SELECT shw.HN_RID, 1 AS FV_RID, shw.TIME_ID, shw.ST_RID, ";
                // add variables
                foreach (VariableProfile vp in aDatabaseVariables)
                {
                    ++count;
                    line += "COALESCE(shw." + vp.DatabaseColumnName + ", 0) " + vp.DatabaseColumnName + ", ";
                    if (line.Length > 90)
                    {
                        aWriter.WriteLine(line);
                        line = _indent10;

                    }

                }
                if (line.Length > 0)
                {
                    aWriter.WriteLine(line);

                }
                // add locks
                count = 0;
                line = _indent10;
                // add variables
                foreach (VariableProfile vp in aDatabaseVariables)
                {
                    ++count;
                    line += "0 AS " + vp.DatabaseColumnName + Include.cLockExtension;
                    if (count < aDatabaseVariables.Count)
                    {
                        line += ", ";
                    }
                    if (line.Length > 90)
                    {
                        aWriter.WriteLine(line);
                        line = _indent10;

                    }

                }
                if (line.Length > 0)
                {
                    aWriter.WriteLine(line);

                }
                aWriter.WriteLine("		FROM " + tempTableName + " xml");
                aWriter.WriteLine("	JOIN " + tableName + " shw (NOLOCK) ON xml.HN_RID = shw.HN_RID AND shw.HN_MOD = xml.HN_MOD");
                aWriter.WriteLine("		AND shw.TIME_ID = xml.TIME_ID");
                aWriter.WriteLine("		AND shw.HN_MOD = xml.HN_MOD");
                aWriter.WriteLine("	OPTION (MAXDOP 1)");

                aWriter.WriteLine(System.Environment.NewLine);
                aWriter.WriteLine("if (select object_id('tempdb.dbo." + aTempTableName + "2')) > 0 DROP TABLE " + aTempTableName + "2");
                aWriter.WriteLine(System.Environment.NewLine);

                aWriter.WriteLine(System.Environment.NewLine);
                aWriter.WriteLine("GO");
                aWriter.WriteLine(System.Environment.NewLine);
            }
            catch
            {
                throw;
            }
        }
        private static void Generate_MID_ST_HIS_WK_ROLLUP(genOptions options)
        {
            StoreHistoryWeekRollupProcess rollupProcess;
            string procedureName;
            bool includeZeroInAverage = true;
            bool honorLocks = false;
            bool zeroParentsWithNoChildren = true;

            try
            {
                ProfileList storeWeeklyHistoryRollupVariables = new ProfileList(eProfileType.Variable);

                foreach (VariableProfile vp in variables.GetStoreWeeklyHistoryDatabaseVariableList())
                {
                    if (vp.LevelRollType != eLevelRollType.None &&
                        vp.StoreWeeklyHistoryModelType != eVariableDatabaseModelType.None &&
                        (vp.VariableCategory == eVariableCategory.Store || vp.VariableCategory == eVariableCategory.Both))
                    {
                        storeWeeklyHistoryRollupVariables.Add(vp);
                    }
                }
                ProfileList aDatabaseVariables = storeWeeklyHistoryRollupVariables;
                StreamWriter aWriter = options.writer;
                int aTableNumber = options.counter;


                procedureName = Include.DBStoreWeeklyHistoryRollupSP.Replace(Include.DBTableCountReplaceString, aTableNumber.ToString());
                AddStoredProcedureDrop(procedureName, aWriter);
                rollupProcess = new StoreHistoryWeekRollupProcess(null, null, aDatabaseVariables.ArrayList, 0, 0, 0, 0, aTableNumber, aTableNumber, includeZeroInAverage, honorLocks, zeroParentsWithNoChildren, true, null);
                aWriter.Write(rollupProcess.BuildStoredProcedure(procedureName));
                aWriter.WriteLine(System.Environment.NewLine);
                aWriter.WriteLine("GO");
                aWriter.WriteLine(System.Environment.NewLine);

                zeroParentsWithNoChildren = false;
                procedureName = Include.DBStoreWeeklyHistoryNoZeroRollupSP.Replace(Include.DBTableCountReplaceString, aTableNumber.ToString());
                AddStoredProcedureDrop(procedureName, aWriter);
                rollupProcess = new StoreHistoryWeekRollupProcess(null, null, aDatabaseVariables.ArrayList, 0, 0, 0, 0, aTableNumber, aTableNumber, includeZeroInAverage, honorLocks, zeroParentsWithNoChildren, true, null);
                aWriter.Write(rollupProcess.BuildStoredProcedure(procedureName));
                aWriter.WriteLine(System.Environment.NewLine);
                aWriter.WriteLine("GO");
                aWriter.WriteLine(System.Environment.NewLine);
            }
            catch
            {
                throw;
            }
        }
        private static void Generate_MID_ST_FOR_WK_DEL_ZERO(genOptions options)
        {
            string line;
            int count;

            try
            {
                string procedureName = options.name;
                string tableName = options.table;
                //string aLockTableName = Include.DBStoreWeeklyLockTable;
                ProfileList aDatabaseVariables = variables.GetStoreWeeklyForecastDatabaseVariableList();
                StreamWriter aWriter = options.writer;
                //int aTableNumber


                AddStoredProcedureDrop(procedureName, aWriter);

                if (aDatabaseVariables == null ||
                    aDatabaseVariables.Count == 0)
                {
                    return;
                }

                aWriter.WriteLine("CREATE PROCEDURE [dbo].[" + procedureName + "]");
                aWriter.WriteLine("@COMMIT_LIMIT INT,");
                aWriter.WriteLine("@RECORDS_DELETED int output");
                aWriter.WriteLine("AS");
                aWriter.WriteLine(System.Environment.NewLine);

                aWriter.WriteLine("DELETE top (@COMMIT_LIMIT) from " + tableName);

                line = _indent10 + "WHERE ";
                count = 0;
                foreach (VariableProfile vp in aDatabaseVariables)
                {
                    ++count;
                    line += "COALESCE(" + vp.DatabaseColumnName + ", 0) = 0";
                    if (count < aDatabaseVariables.Count)
                    {
                        line += " AND ";
                    }
                    if (line.Length > 90)
                    {
                        aWriter.WriteLine(line);
                        line = _indent10;
                    }
                }

                if (line.Length > 0)
                {
                    aWriter.WriteLine(line);
                }

                aWriter.WriteLine("set @RECORDS_DELETED = @@rowcount");
                aWriter.WriteLine(System.Environment.NewLine);
                aWriter.WriteLine("GO");
                aWriter.WriteLine(System.Environment.NewLine);
            }
            catch
            {
                throw;
            }
        }
        private static void Generate_MID_ST_FOR_WK_WRITE(genOptions options)
        {
            string line;
            int count = 0;
            try
            {
                string procedureName = options.name;
                string tableName = options.table;
                //string aLockTableName = Include.DBStoreWeeklyLockTable;
                //string aTempTableName = Include.DBTempTableName;
                ProfileList aDatabaseVariables = variables.GetStoreWeeklyForecastDatabaseVariableList(); 
                StreamWriter aWriter = options.writer;

                //int aTableNumber, string aTableCountColumn

                if (aDatabaseVariables == null ||
                    aDatabaseVariables.Count == 0)
                {
                    return;
                }

                aWriter.WriteLine("CREATE PROCEDURE [dbo].[" + procedureName + "]");
                aWriter.WriteLine(" ");
                // Begin TT#3373 - JSmith - Save Store Forecast receive DBNull error
                //aWriter.WriteLine("@dt " + Include.DBStoreWeeklyForecastType + " READONLY,");
                //aWriter.WriteLine("@dtLock " + Include.DBStoreWeeklyForecastLockType + " READONLY,");
                //aWriter.WriteLine("@SaveLocks CHAR");
                aWriter.WriteLine("@dt " + Include.DBStoreWeeklyForecastType + " READONLY");
                // End TT#3373 - JSmith - Save Store Forecast receive DBNull error
                aWriter.WriteLine("AS");
                aWriter.WriteLine("SET NOCOUNT ON");
                aWriter.WriteLine("MERGE INTO " + tableName + " with (ROWLOCK) AS TARGET");
                aWriter.WriteLine("USING @dt AS SOURCE");
                aWriter.WriteLine("on (SOURCE.HN_MOD = TARGET.HN_MOD and SOURCE.HN_RID = TARGET.HN_RID and SOURCE.FV_RID = TARGET.FV_RID and SOURCE.TIME_ID = TARGET.TIME_ID and SOURCE.ST_RID = TARGET.ST_RID)");
                aWriter.WriteLine("WHEN MATCHED THEN");
                aWriter.WriteLine("UPDATE ");
                count = 0;
                line = _blankLine;
                foreach (VariableProfile vp in aDatabaseVariables)
                {
                    ++count;
                    if (count == 1)
                    {
                        line = line.Insert(9, "SET");
                    }
                    line = line.Insert(13, "TARGET." + vp.DatabaseColumnName);
                    line = line.TrimEnd();
                    line += " = COALESCE(SOURCE." + vp.DatabaseColumnName + ", ";
                    line += "TARGET." + vp.DatabaseColumnName + ")";
                    if (count < aDatabaseVariables.Count)
                    {
                        line += ",";
                    }
                    aWriter.WriteLine(line);
                    line = _blankLine;
                }

                aWriter.WriteLine("WHEN NOT MATCHED BY TARGET THEN");
                line = "INSERT (HN_MOD, HN_RID, FV_RID, TIME_ID, ST_RID, ";
                count = 0;
                foreach (VariableProfile vp in aDatabaseVariables)
                {
                    ++count;
                    line += vp.DatabaseColumnName;
                    if (count < aDatabaseVariables.Count)
                    {
                        line += ", ";
                    }
                    if (line.Length > 150)
                    {
                        aWriter.WriteLine(line);
                        line = _indent5;

                    }
                }
                line += ")";
                aWriter.WriteLine(line);

                count = 0;
                line = "VALUES ( SOURCE.HN_MOD, SOURCE.HN_RID, SOURCE.FV_RID, SOURCE.TIME_ID, SOURCE.ST_RID,";
                foreach (VariableProfile vp in aDatabaseVariables)
                {
                    ++count;
                    line += " SOURCE." + vp.DatabaseColumnName;
                    if (count < aDatabaseVariables.Count)
                    {
                        line += ",";
                    }
                    if (line.Length > 110)
                    {
                        aWriter.WriteLine(line);
                        line = _indent5;

                    }
                }
                line += ");";
                if (line.Length > 0)
                {
                    aWriter.WriteLine(line);
                }

                // Begin TT#3373 - JSmith - Save Store Forecast receive DBNull error
                //  process locks
                //aWriter.WriteLine("if @SaveLocks = '1'");
                //aWriter.WriteLine("begin");
                //aWriter.WriteLine("MERGE INTO " + aLockTableName + " with (ROWLOCK) AS TARGET");
                //aWriter.WriteLine("USING @dtLock AS SOURCE");
                //aWriter.WriteLine("on (SOURCE.HN_RID = TARGET.HN_RID and SOURCE.FV_RID = TARGET.FV_RID and SOURCE.TIME_ID = TARGET.TIME_ID and SOURCE.ST_RID = TARGET.ST_RID)");
                //aWriter.WriteLine("WHEN MATCHED THEN");
                //aWriter.WriteLine("UPDATE ");
                //count = 0;
                //line = _blankLine;
                //foreach (VariableProfile vp in aDatabaseVariables)
                //{
                //    ++count;
                //    if (count == 1)
                //    {
                //        line = line.Insert(9, "SET");
                //    }
                //    line = line.Insert(13, "TARGET." + vp.DatabaseColumnName + "_LOCK");
                //    line = line.TrimEnd();
                //    line += " = COALESCE(SOURCE." + vp.DatabaseColumnName + "_LOCK, ";
                //    line += "TARGET." + vp.DatabaseColumnName + "_LOCK)";
                //    if (count < aDatabaseVariables.Count)
                //    {
                //        line += ",";
                //    }
                //    aWriter.WriteLine(line);
                //    line = _blankLine;
                //}

                //aWriter.WriteLine("WHEN NOT MATCHED BY TARGET THEN");
                //line = "INSERT (HN_RID, FV_RID, TIME_ID, ST_RID, ";
                //count = 0;
                //foreach (VariableProfile vp in aDatabaseVariables)
                //{
                //    ++count;
                //    line += vp.DatabaseColumnName + "_LOCK";
                //    if (count < aDatabaseVariables.Count)
                //    {
                //        line += ", ";
                //    }
                //    if (line.Length > 150)
                //    {
                //        aWriter.WriteLine(line);
                //        line = _indent5;

                //    }
                //}
                //line += ")";
                //aWriter.WriteLine(line);

                //count = 0;
                //line = "VALUES ( SOURCE.HN_RID, SOURCE.FV_RID, SOURCE.TIME_ID, SOURCE.ST_RID,";
                //foreach (VariableProfile vp in aDatabaseVariables)
                //{
                //    ++count;
                //    line += " SOURCE." + vp.DatabaseColumnName + "_LOCK";
                //    if (count < aDatabaseVariables.Count)
                //    {
                //        line += ",";
                //    }
                //    if (line.Length > 110)
                //    {
                //        aWriter.WriteLine(line);
                //        line = _indent5;

                //    }
                //}
                //line += ");";
                //if (line.Length > 0)
                //{
                //    aWriter.WriteLine(line);
                //}
                //aWriter.WriteLine("end");
                // End TT#3373 - JSmith - Save Store Forecast receive DBNull error

                aWriter.WriteLine(System.Environment.NewLine);
                aWriter.WriteLine("GO");
                aWriter.WriteLine(System.Environment.NewLine);
            }
            catch
            {
                throw;
            }
        }
        private static void Generate_MID_ST_FOR_WK_READ(genOptions options)
        {
            try
            {
                int count = 0;
                string line = string.Empty;
                string procedureName = options.name;
                string tableName = options.table;



                string aViewName = Include.DBStoreWeeklyForecastView;
                //string aTempTableName = Include.DBTempTableName;

                 ProfileList aDatabaseVariables = variables.GetStoreWeeklyForecastDatabaseVariableList();
                 StreamWriter aWriter = options.writer;
                //int aTableNumber, 
                //string aTableCountColumn


                if (aDatabaseVariables == null ||
                    aDatabaseVariables.Count == 0)
                {
                    return;
                }

                string tempTableName = "@dt";


                aWriter.WriteLine("CREATE PROCEDURE [dbo].[" + procedureName + "]");
                aWriter.WriteLine("@dt " + Include.DBStoreWeeklyForecastReadType + " READONLY");
                aWriter.WriteLine("AS");
                aWriter.WriteLine("SET NOCOUNT ON");

                count = 0;
                line = "       SELECT sfw.HN_RID, sfw.FV_RID, sfw.TIME_ID, sfw.ST_RID, ";
                // add variables
                foreach (VariableProfile vp in aDatabaseVariables)
                {
                    ++count;
                    line += "COALESCE(sfw." + vp.DatabaseColumnName + ", 0) " + vp.DatabaseColumnName;
                    if (count < aDatabaseVariables.Count)
                    {
                        line += ", ";
                    }
                    if (line.Length > 90)
                    {
                        aWriter.WriteLine(line);
                        line = _indent10;

                    }

                }
                if (line.Length > 0)
                {
                    aWriter.WriteLine(line);

                }
                aWriter.WriteLine("		FROM " + tempTableName + " xml");
                aWriter.WriteLine("	JOIN " + aViewName + " sfw ON xml.HN_MOD = sfw.HN_MOD AND xml.HN_RID = sfw.HN_RID");
                aWriter.WriteLine("		AND sfw.FV_RID = xml.FV_RID");
                aWriter.WriteLine("		AND sfw.TIME_ID = xml.TIME_ID");
                aWriter.WriteLine("	OPTION (MAXDOP 1)");

                aWriter.WriteLine(System.Environment.NewLine);
                aWriter.WriteLine("GO");
                aWriter.WriteLine(System.Environment.NewLine);
            }
            catch
            {
                throw;
            }
        }
        private static void Generate_MID_ST_MOD_WK_READ(genOptions options)
        {
            try
            {
                string procedureName = options.name;
                int aTableNumber = options.counter;

                string forecastTableName = Include.DBStoreWeeklyForecastTable.Replace(Include.DBTableCountReplaceString, aTableNumber.ToString());
                string historyTableName = Include.DBStoreWeeklyHistoryTable.Replace(Include.DBTableCountReplaceString, aTableNumber.ToString());
                string aLockTableName = Include.DBStoreWeeklyLockTable;

                string aForecastViewName = Include.DBStoreWeeklyForecastView;
                string aHistoryViewName = Include.DBStoreWeeklyHistoryView;
                string aTempTableName = Include.DBTempTableName;

                ProfileList aDatabaseVariables = variables.GetStoreWeeklyForecastDatabaseVariableList();
                StreamWriter aWriter = options.writer;
            




                if (aDatabaseVariables == null ||
                    aDatabaseVariables.Count == 0)
                {
                    return;
                }
                string line = string.Empty;
                int count = 0;
                string tempTableName = "@dt";

                aWriter.WriteLine("CREATE PROCEDURE [dbo].[" + procedureName + "]");
                aWriter.WriteLine("@dt " + Include.DBStoreWeeklyModifiedReadType + " READONLY,");
                aWriter.WriteLine("@Rollup CHAR(1) = NULL");
                aWriter.WriteLine("AS");
                aWriter.WriteLine("SET NOCOUNT ON");
                aWriter.WriteLine("DECLARE @Tables INT,");
                aWriter.WriteLine("        @Loop INT,");
                aWriter.WriteLine("        @HN_TYPE INT,");
                aWriter.WriteLine("        @HN_RID INT,");
                aWriter.WriteLine("        @FV_RID INT,");
                aWriter.WriteLine("        @HN_MOD INT,");
                aWriter.WriteLine("        @ROLL_OPTION INT,");
                aWriter.WriteLine("        @LoopCount INT,");
                aWriter.WriteLine("        @NextLoopCount INT");
                //aWriter.WriteLine("SELECT @Tables = " + aTableCountColumn + " FROM SYSTEM_OPTIONS");
                aWriter.WriteLine("SET @Tables = 10");

                aWriter.WriteLine("SELECT DISTINCT @HN_TYPE = ph.PH_TYPE, @HN_RID = t.HN_RID, @HN_MOD = t.HN_RID % @Tables,");
                aWriter.WriteLine("                @ROLL_OPTION = COALESCE(ph.HISTORY_ROLL_OPTION, 800251), @FV_RID = t.FV_RID");
                aWriter.WriteLine("        FROM HIERARCHY_NODE hn (NOLOCK)");
                aWriter.WriteLine("        JOIN PRODUCT_HIERARCHY ph (NOLOCK) ON ph.PH_RID = hn.HOME_PH_RID");
                aWriter.WriteLine("        JOIN " + tempTableName + " t on t.HN_RID = hn.HN_RID");

                aWriter.WriteLine(" ");
                aWriter.WriteLine(" -- build temp table of values and locks for modified version");
                aWriter.WriteLine(" create table " + aTempTableName + "MOD ");
                aWriter.WriteLine(" (HN_RID  int not null,");
                aWriter.WriteLine("  FV_RID  int not null,");
                aWriter.WriteLine("  TIME_ID int not null,");
                aWriter.WriteLine("  ST_RID int not null,");
                count = 0;
                // add variables
                line = "  ";
                foreach (VariableProfile vp in aDatabaseVariables)
                {
                    line += "  " + vp.DatabaseColumnName;
                    switch (vp.StoreDatabaseVariableType)
                    {
                        case eVariableDatabaseType.Integer:
                            line += " int ";
                            break;
                        case eVariableDatabaseType.Real:
                            line += " real ";
                            break;
                        case eVariableDatabaseType.Float:
                            line += " float ";
                            break;
                        case eVariableDatabaseType.BigInteger:
                            line += " bigint ";
                            break;
                    }
                    line += "  null,";
                    aWriter.WriteLine(line);
                    line = "  ";
                }
                count = 0;
                // add locks
                line = "  ";
                foreach (VariableProfile vp in aDatabaseVariables)
                {
                    ++count;
                    line += "  " + vp.DatabaseColumnName + Include.cLockExtension + "  char(1) null";
                    if (count < aDatabaseVariables.Count)
                    {
                        line += ", ";
                    }
                    else
                    {
                        line += ")";
                    }
                    aWriter.WriteLine(line);
                    line = "  ";

                }
                if (line.Length > 0)
                {
                    aWriter.WriteLine(line);
                }

                aWriter.WriteLine(" ");
                aWriter.WriteLine(" insert into " + aTempTableName + "MOD");
                aWriter.WriteLine("  select sfw.HN_RID, sfw.FV_RID, sfw.TIME_ID, sfw.ST_RID,");
                foreach (VariableProfile vp in aDatabaseVariables)
                {
                    line += "  sfw." + vp.DatabaseColumnName + ", ";
                    if (line.Length > 80)
                    {
                        aWriter.WriteLine(line);
                        line = "  ";
                    }

                }
                if (line.Length > 0)
                {
                    aWriter.WriteLine(line);
                }
                count = 0;
                line = "  ";
                foreach (VariableProfile vp in aDatabaseVariables)
                {
                    ++count;
                    line += "  COALESCE(sfwl." + vp.DatabaseColumnName + Include.cLockExtension + ", 0) " + vp.DatabaseColumnName + Include.cLockExtension;
                    if (count < aDatabaseVariables.Count)
                    {
                        line += ", ";
                    }
                    aWriter.WriteLine(line);
                    line = "  ";

                }

                aWriter.WriteLine("   FROM " + tempTableName + " t");
                aWriter.WriteLine("   JOIN " + aForecastViewName + " sfw (NOLOCK)");
                aWriter.WriteLine("    on sfw.HN_RID = t.HN_RID");
                aWriter.WriteLine("    and sfw.FV_RID = t.FV_RID");
                aWriter.WriteLine("    and sfw.TIME_ID = t.TIME_ID");
                aWriter.WriteLine("   left outer join " + aLockTableName + " sfwl (NOLOCK)");
                aWriter.WriteLine("    on sfwl.HN_RID = sfw.HN_RID");
                aWriter.WriteLine("    and sfwl.FV_RID =sfw.FV_RID");
                aWriter.WriteLine("    and sfwl.TIME_ID = sfw.TIME_ID");
                aWriter.WriteLine("    and sfwl.ST_RID = sfw.ST_RID");
                aWriter.WriteLine("  union");
                aWriter.WriteLine("  select sfwl.HN_RID, sfwl.FV_RID, sfwl.TIME_ID, sfwl.ST_RID, ");
                foreach (VariableProfile vp in aDatabaseVariables)
                {
                    line += "  sfw." + vp.DatabaseColumnName + ", ";
                    if (line.Length > 80)
                    {
                        aWriter.WriteLine(line);
                        line = "  ";
                    }

                }
                if (line.Length > 0)
                {
                    aWriter.WriteLine(line);
                }
                count = 0;
                line = "  ";
                foreach (VariableProfile vp in aDatabaseVariables)
                {
                    ++count;
                    line += "  COALESCE(sfwl." + vp.DatabaseColumnName + Include.cLockExtension + ", 0) " + vp.DatabaseColumnName + Include.cLockExtension;
                    if (count < aDatabaseVariables.Count)
                    {
                        line += ", ";
                    }
                    aWriter.WriteLine(line);
                    line = "  ";

                }

                aWriter.WriteLine(" FROM " + tempTableName + " t");
                aWriter.WriteLine(" JOIN " + aLockTableName + " sfwl (NOLOCK)");
                aWriter.WriteLine("  on sfwl.HN_RID = t.HN_RID");
                aWriter.WriteLine("  and sfwl.FV_RID = t.FV_RID");
                aWriter.WriteLine("  and sfwl.TIME_ID = t.TIME_ID");
                aWriter.WriteLine(" left outer join " + aForecastViewName + " sfw (NOLOCK)");
                aWriter.WriteLine("  on sfw.HN_RID = sfwl.HN_RID");
                aWriter.WriteLine("  and sfw.FV_RID =sfwl.FV_RID");
                aWriter.WriteLine("  and sfw.TIME_ID = sfwl.TIME_ID");
                aWriter.WriteLine("  and sfw.ST_RID = sfwl.ST_RID");

                aWriter.WriteLine(" ");
                aWriter.WriteLine(" -- create temp table for history values");
                aWriter.WriteLine(" create table " + aTempTableName + "HISTORY ");
                aWriter.WriteLine(" (HN_RID  int not null,");
                aWriter.WriteLine("  FV_RID  int not null,");
                aWriter.WriteLine("  TIME_ID int not null,");
                aWriter.WriteLine("  ST_RID int not null,");
                count = 0;
                // add variables
                line = "  ";
                foreach (VariableProfile vp in aDatabaseVariables)
                {
                    ++count;
                    line += "  " + vp.DatabaseColumnName;
                    switch (vp.StoreDatabaseVariableType)
                    {
                        case eVariableDatabaseType.Integer:
                            line += " int ";
                            break;
                        case eVariableDatabaseType.Real:
                            line += " real ";
                            break;
                        case eVariableDatabaseType.DateTime:
                            line += " datetime ";
                            break;
                        case eVariableDatabaseType.String:
                            line += " varchar(100) ";
                            break;
                        case eVariableDatabaseType.Char:
                            line += " char(1) ";
                            break;
                        case eVariableDatabaseType.Float:
                            line += " float ";
                            break;
                        case eVariableDatabaseType.BigInteger:
                            line += " bigint ";
                            break;
                    }
                    if (count < aDatabaseVariables.Count)
                    {
                        line += "null, ";
                    }
                    else
                    {
                        line += "null)";
                    }
                    aWriter.WriteLine(line);
                    line = "  ";
                }
                aWriter.WriteLine(" ");

                aWriter.WriteLine(" -- alternate and real time roll");
                aWriter.WriteLine("IF @HN_TYPE = 800001 and @ROLL_OPTION = 800251");
                aWriter.WriteLine("   BEGIN");
                aWriter.WriteLine("   SET @LoopCount = 0");
                aWriter.WriteLine("   CREATE TABLE #TREE (LOOPCOUNT INT NOT NULL, PARENT_HN_RID INT NOT NULL, HOME_PH_RID INT, PH_TYPE INT, CHILD_HN_RID INT NOT NULL, CHILD_HN_MOD INT NOT NULL)");
                aWriter.WriteLine("   -- insert the children of the node into the temp table");
                aWriter.WriteLine("   INSERT #TREE (LOOPCOUNT, PARENT_HN_RID, HOME_PH_RID, PH_TYPE, CHILD_HN_RID, CHILD_HN_MOD) ");
                aWriter.WriteLine("       select @LoopCount as LOOPCOUNT, @HN_RID as PARENT_HN_RID, hn.HOME_PH_RID, ph.PH_TYPE, hnj.HN_RID, hnj.HN_RID % @Tables");
                aWriter.WriteLine("         from HIER_NODE_JOIN hnj (NOLOCK)");
                aWriter.WriteLine("           JOIN HIERARCHY_NODE hn (NOLOCK) ON hn.HN_RID = hnj.HN_RID");
                aWriter.WriteLine("           JOIN PRODUCT_HIERARCHY ph (NOLOCK) ON ph.PH_RID = hn.HOME_PH_RID");
                aWriter.WriteLine("       where @HN_RID = hnj.PARENT_HN_RID");
                aWriter.WriteLine("   SET @Loop = @@ROWCOUNT");
                aWriter.WriteLine("   -- chase all paths until you get the main hierarchy (type 800000) or the lowest leaf");
                aWriter.WriteLine("   SET @NextLoopCount = @LoopCount + 1");
                aWriter.WriteLine("   WHILE @Loop > 0");
                aWriter.WriteLine("   BEGIN");
                aWriter.WriteLine("      INSERT #TREE");
                aWriter.WriteLine("        select @NextLoopCount as LOOPCOUNT, hnj.PARENT_HN_RID, hn.HOME_PH_RID, ph.PH_TYPE, hnj.HN_RID, hnj.HN_RID % @Tables");
                aWriter.WriteLine("          from HIER_NODE_JOIN hnj (NOLOCK)");
                aWriter.WriteLine("            JOIN HIERARCHY_NODE hn (NOLOCK) ON hn.HN_RID = hnj.HN_RID");
                aWriter.WriteLine("            JOIN PRODUCT_HIERARCHY ph (NOLOCK) ON ph.PH_RID = hn.HOME_PH_RID");
                aWriter.WriteLine("            JOIN #TREE t ON hnj.PARENT_HN_RID = t.CHILD_HN_RID");
                aWriter.WriteLine("        WHERE t.LOOPCOUNT =  @LoopCount AND t.PH_TYPE <> 800000");
                aWriter.WriteLine("      SET @Loop = @@ROWCOUNT");
                aWriter.WriteLine("      SET @LoopCount = @LoopCount + 1");
                aWriter.WriteLine("      SET @NextLoopCount = @LoopCount + 1");
                aWriter.WriteLine("   END");
                aWriter.WriteLine("   -- join with dates from xml selecting only nodes from the main hierarchy or lowest leaf alternates");
                aWriter.WriteLine("   SELECT * ");
                aWriter.WriteLine("     INTO " + aTempTableName + "2");
                aWriter.WriteLine("     FROM #TREE");
                aWriter.WriteLine("     CROSS JOIN " + tempTableName);
                aWriter.WriteLine("   where LOOPCOUNT = @LoopCount - 1");
                aWriter.WriteLine("	     or PH_TYPE = 800000");

                count = 0;
                aWriter.WriteLine("       -- build temp table of summed history values ");
                aWriter.WriteLine("       insert into " + aTempTableName + "HISTORY ");
                aWriter.WriteLine("       select @HN_RID as HN_RID, @FV_RID as FV_RID, shw.TIME_ID, shw.ST_RID, ");
                // add variables
                line = _indent10;
                int variablesCount = 0;
                foreach (VariableProfile vp in aDatabaseVariables)
                {
                    if (vp.StoreDatabaseVariableType == eVariableDatabaseType.Integer ||
                        vp.StoreDatabaseVariableType == eVariableDatabaseType.Real ||
                        vp.StoreDatabaseVariableType == eVariableDatabaseType.Float ||
                        vp.StoreDatabaseVariableType == eVariableDatabaseType.BigInteger)
                    {
                        ++variablesCount;
                    }

                }
                foreach (VariableProfile vp in aDatabaseVariables)
                {
                    if (vp.StoreDatabaseVariableType == eVariableDatabaseType.Integer ||
                        vp.StoreDatabaseVariableType == eVariableDatabaseType.Real ||
                        vp.StoreDatabaseVariableType == eVariableDatabaseType.Float ||
                        vp.StoreDatabaseVariableType == eVariableDatabaseType.BigInteger)
                    {
                        ++count;
                        line += "SUM(shw." + vp.DatabaseColumnName + ") " + vp.DatabaseColumnName;
                        if (count < variablesCount)
                        {
                            line += ", ";
                        }
                        aWriter.WriteLine(line);
                        line = _indent10;
                    }

                }
                aWriter.WriteLine("		FROM " + aTempTableName + "2 t");
                aWriter.WriteLine("	    JOIN " + aHistoryViewName + " shw (NOLOCK) ON t.CHILD_HN_RID = shw.HN_RID");
                aWriter.WriteLine("		    AND shw.TIME_ID = t.TIME_ID");
                aWriter.WriteLine("	    GROUP BY shw.TIME_ID, ST_RID");
                aWriter.WriteLine(" END ");
                aWriter.WriteLine(" ELSE ");
                aWriter.WriteLine(" BEGIN ");
                aWriter.WriteLine("       -- build temp table of history values ");
                aWriter.WriteLine("       insert into " + aTempTableName + "HISTORY ");
                aWriter.WriteLine("       select shw.HN_RID,  t.FV_RID as FV_RID, shw.TIME_ID, shw.ST_RID, ");
                // add variables
                count = 0;
                line = _indent10;
                foreach (VariableProfile vp in aDatabaseVariables)
                {
                    ++count;
                    line += "shw." + vp.DatabaseColumnName + " as " + vp.DatabaseColumnName;
                    if (count < aDatabaseVariables.Count)
                    {
                        line += ", ";
                    }
                    aWriter.WriteLine(line);
                    line = _indent10;

                }
                aWriter.WriteLine("  FROM " + tempTableName + " t");
                aWriter.WriteLine("  JOIN " + aHistoryViewName + " shw (NOLOCK)");
                aWriter.WriteLine("    on shw.HN_RID = t.HN_RID");
                aWriter.WriteLine("   and shw.TIME_ID = t.TIME_ID");
                aWriter.WriteLine(" END ");
                aWriter.WriteLine(" ");

                aWriter.WriteLine("  --select * from " + aTempTableName + "MOD");
                aWriter.WriteLine("  --select * from " + aTempTableName + "HISTORY");

                aWriter.WriteLine("   -- combine modified values with history");
                aWriter.WriteLine(" select tmpmod.HN_RID, tmpmod.FV_RID, tmpmod.TIME_ID, tmpmod.ST_RID,");
                foreach (VariableProfile vp in aDatabaseVariables)
                {
                    line += "COALESCE(COALESCE(tmpmod." + vp.DatabaseColumnName + ", tmphis." + vp.DatabaseColumnName + "), 0) " + vp.DatabaseColumnName + ", ";
                    aWriter.WriteLine(line);
                    line = _indent10;

                }
                count = 0;
                foreach (VariableProfile vp in aDatabaseVariables)
                {
                    ++count;
                    line += "COALESCE(tmpmod." + vp.DatabaseColumnName + Include.cLockExtension + ", 0) " + vp.DatabaseColumnName + Include.cLockExtension;
                    if (count < aDatabaseVariables.Count)
                    {
                        line += ", ";
                    }
                    aWriter.WriteLine(line);
                    line = _indent10;

                }
                aWriter.WriteLine("    into  " + aTempTableName + "MOD2");
                aWriter.WriteLine(" FROM " + aTempTableName + "MOD tmpmod");
                aWriter.WriteLine(" left outer JOIN " + aTempTableName + "HISTORY tmphis  ON tmphis.HN_RID = tmpmod.HN_RID");
                aWriter.WriteLine(" AND tmphis.FV_RID = tmpmod.FV_RID");
                aWriter.WriteLine(" AND tmphis.TIME_ID = tmpmod.TIME_ID");
                aWriter.WriteLine(" AND tmphis.ST_RID = tmpmod.ST_RID");

                aWriter.WriteLine(" ");
                aWriter.WriteLine(" -- remove duplicate rows from history table");
                aWriter.WriteLine("  delete " + aTempTableName + "HISTORY ");
                aWriter.WriteLine("    from " + aTempTableName + "HISTORY, " + aTempTableName + "MOD");
                aWriter.WriteLine("    where " + aTempTableName + "HISTORY.HN_RID = " + aTempTableName + "MOD.HN_RID");
                aWriter.WriteLine("         and " + aTempTableName + "HISTORY.FV_RID = " + aTempTableName + "MOD.FV_RID");
                aWriter.WriteLine("         and " + aTempTableName + "HISTORY.TIME_ID = " + aTempTableName + "MOD.TIME_ID");
                aWriter.WriteLine("         and " + aTempTableName + "HISTORY.ST_RID = " + aTempTableName + "MOD.ST_RID");
                aWriter.WriteLine(" ");

                aWriter.WriteLine(" --select * from " + aTempTableName + "HISTORY");

                aWriter.WriteLine(" insert into " + aTempTableName + "MOD2");
                aWriter.WriteLine(" select tmphis.HN_RID, tmphis.FV_RID, tmphis.TIME_ID, tmphis.ST_RID,");
                foreach (VariableProfile vp in aDatabaseVariables)
                {
                    line += "COALESCE(tmphis." + vp.DatabaseColumnName + ", 0) " + vp.DatabaseColumnName + ", ";
                    aWriter.WriteLine(line);
                    line = _indent10;
                }
                count = 0;
                foreach (VariableProfile vp in aDatabaseVariables)
                {
                    ++count;
                    line += "0 as " + vp.DatabaseColumnName + Include.cLockExtension;
                    if (count < aDatabaseVariables.Count)
                    {
                        line += ", ";
                    }
                    aWriter.WriteLine(line);
                    line = _indent10;
                }

                aWriter.WriteLine(" from " + aTempTableName + "HISTORY tmphis");
                aWriter.WriteLine(" ");
                aWriter.WriteLine(" select * from " + aTempTableName + "MOD2 order by TIME_ID, ST_RID");
                aWriter.WriteLine(" ");

                aWriter.WriteLine(" OPTION (MAXDOP 1)");

                aWriter.WriteLine(System.Environment.NewLine);
                aWriter.WriteLine("if (select object_id('tempdb.dbo." + aTempTableName + "2')) > 0 DROP TABLE " + aTempTableName + "2");
                aWriter.WriteLine("if (select object_id('tempdb.dbo." + aTempTableName + "MOD')) > 0 DROP TABLE " + aTempTableName + "MOD");
                aWriter.WriteLine("if (select object_id('tempdb.dbo." + aTempTableName + "MOD2')) > 0 DROP TABLE " + aTempTableName + "MOD2");
                aWriter.WriteLine("if (select object_id('tempdb.dbo." + aTempTableName + "HISTORY')) > 0 DROP TABLE " + aTempTableName + "HISTORY");
                aWriter.WriteLine(System.Environment.NewLine);

                aWriter.WriteLine(System.Environment.NewLine);
                aWriter.WriteLine("GO");
                aWriter.WriteLine(System.Environment.NewLine);
            }
            catch
            {
                throw;
            }
        }
        private static void Generate_MID_ST_FOR_WK_ROLLUP(genOptions options)
        {
            StoreForecastWeekRollupProcess rollupProcess;
            string procedureName;
            bool includeZeroInAverage = true;
            bool honorLocks = false;
            bool zeroParentsWithNoChildren = true;

            try
            {
                ProfileList storeWeeklyForecastRollupVariables = new ProfileList(eProfileType.Variable);
                foreach (VariableProfile vp in variables.GetStoreWeeklyForecastDatabaseVariableList())
                {
                    if (vp.LevelRollType != eLevelRollType.None &&
                        vp.StoreForecastModelType != eVariableDatabaseModelType.None &&
                        (vp.VariableCategory == eVariableCategory.Store || vp.VariableCategory == eVariableCategory.Both))
                    {
                        storeWeeklyForecastRollupVariables.Add(vp);
                    }
                }

                ProfileList aDatabaseVariables = storeWeeklyForecastRollupVariables;
                StreamWriter aWriter = options.writer;
                int aTableNumber = options.counter;

                procedureName = Include.DBStoreWeeklyForecastRollupSP.Replace(Include.DBTableCountReplaceString, aTableNumber.ToString());
                AddStoredProcedureDrop(procedureName, aWriter);
                rollupProcess = new StoreForecastWeekRollupProcess(null, null, aDatabaseVariables.ArrayList, 0, 0, 0, 0, aTableNumber, aTableNumber, includeZeroInAverage, honorLocks, zeroParentsWithNoChildren, true, null);
                aWriter.Write(rollupProcess.BuildStoredProcedure(procedureName, honorLocks));
                aWriter.WriteLine(System.Environment.NewLine);
                aWriter.WriteLine("GO");
                aWriter.WriteLine(System.Environment.NewLine);

                zeroParentsWithNoChildren = false;
                procedureName = Include.DBStoreWeeklyForecastNoZeroRollupSP.Replace(Include.DBTableCountReplaceString, aTableNumber.ToString());
                AddStoredProcedureDrop(procedureName, aWriter);
                rollupProcess = new StoreForecastWeekRollupProcess(null, null, aDatabaseVariables.ArrayList, 0, 0, 0, 0, aTableNumber, aTableNumber, includeZeroInAverage, honorLocks, zeroParentsWithNoChildren, true, null);
                aWriter.Write(rollupProcess.BuildStoredProcedure(procedureName, honorLocks));
                aWriter.WriteLine(System.Environment.NewLine);
                aWriter.WriteLine("GO");
                aWriter.WriteLine(System.Environment.NewLine);

                zeroParentsWithNoChildren = true;
                honorLocks = true;
                procedureName = Include.DBStoreWeeklyForecastHonorLocksRollupSP.Replace(Include.DBTableCountReplaceString, aTableNumber.ToString());
                AddStoredProcedureDrop(procedureName, aWriter);
                rollupProcess = new StoreForecastWeekRollupProcess(null, null, aDatabaseVariables.ArrayList, 0, 0, 0, 0, aTableNumber, aTableNumber, includeZeroInAverage, honorLocks, zeroParentsWithNoChildren, true, null);
                aWriter.Write(rollupProcess.BuildStoredProcedure(procedureName, honorLocks));
                aWriter.WriteLine(System.Environment.NewLine);
                aWriter.WriteLine("GO");
                aWriter.WriteLine(System.Environment.NewLine);
            }
            catch
            {
                throw;
            }
        }
        private static void Generate_MID_ST_FOR_WK_DEL_UNLOCK(genOptions options)
        {
            string line;
            int count;

            try
            {
                string aProcedureName = options.name;
                string aTableName = options.table; 
                ProfileList aDatabaseVariables = variables.GetStoreWeeklyForecastDatabaseVariableList();
                StreamWriter aWriter = options.writer;



                AddStoredProcedureDrop(aProcedureName, aWriter);

                if (aDatabaseVariables == null ||
                    aDatabaseVariables.Count == 0)
                {
                    return;
                }

                aWriter.WriteLine("CREATE PROCEDURE [dbo].[" + aProcedureName + "]");
                aWriter.WriteLine("@COMMIT_LIMIT INT,");
                aWriter.WriteLine("@RECORDS_DELETED int output");
                aWriter.WriteLine("AS");
                aWriter.WriteLine(System.Environment.NewLine);

                aWriter.WriteLine("DELETE TOP (@COMMIT_LIMIT) " + aTableName);

                line = _indent10 + "WHERE ";
                count = 0;
                foreach (VariableProfile vp in aDatabaseVariables)
                {
                    ++count;
                    line += "COALESCE(" + vp.DatabaseColumnName + Include.cLockExtension + ", 0) = 0";
                    if (count < aDatabaseVariables.Count)
                    {
                        line += " AND ";
                    }
                    if (line.Length > 90)
                    {
                        aWriter.WriteLine(line);
                        line = _indent10;
                    }
                }

                if (line.Length > 0)
                {
                    aWriter.WriteLine(line);
                }

                aWriter.WriteLine("set @RECORDS_DELETED = @@rowcount");

                aWriter.WriteLine(System.Environment.NewLine);
                aWriter.WriteLine("GO");
                aWriter.WriteLine(System.Environment.NewLine);
            }
            catch
            {
                throw;
            }
        }
        private static void Generate_MID_ST_FOR_WK_LOCK_READ(genOptions options)
        {
            int count = 0;
            string line;
            try
            {
                string aProcedureName = options.name;
                string aTableName = options.table;
                //string aViewName = Include.DBStoreWeeklyForecastView;
                //string aTableCountColumn, 
                //string aTempTableName,
                ProfileList aDatabaseVariables = variables.GetStoreWeeklyForecastDatabaseVariableList();
                StreamWriter aWriter = options.writer;


                AddStoredProcedureDrop(aProcedureName, aWriter);
                //AddTypeDrop(Include.DBStoreWeeklyForecastReadLockType, aWriter);

                if (aDatabaseVariables == null ||
                    aDatabaseVariables.Count == 0)
                {
                    return;
                }

                string tempTableName = "@dt";

                //aWriter.WriteLine("CREATE TYPE [dbo].[" + Include.DBStoreWeeklyForecastReadLockType + "] AS TABLE(");
                //aWriter.WriteLine("  [HN_RID] [int],");
                //aWriter.WriteLine("  [FV_RID] [int],");
                //aWriter.WriteLine("  [TIME_ID] [int]");
                //aWriter.WriteLine("PRIMARY KEY (HN_RID, FV_RID, TIME_ID)");
                //aWriter.WriteLine(")");

                //aWriter.WriteLine(System.Environment.NewLine);
                //aWriter.WriteLine("GO");
                //aWriter.WriteLine(System.Environment.NewLine);

                aWriter.WriteLine("CREATE PROCEDURE [dbo].[" + aProcedureName + "]");
                aWriter.WriteLine("@dt " + Include.DBStoreWeeklyForecastReadLockType + " READONLY");
                aWriter.WriteLine("AS");
                aWriter.WriteLine("SET NOCOUNT ON");

                count = 0;
                line = "       SELECT sfwl.HN_RID, sfwl.FV_RID, sfwl.TIME_ID, sfwl.ST_RID, ";
                // add variables
                foreach (VariableProfile vp in aDatabaseVariables)
                {
                    ++count;
                    line += "COALESCE(sfwl." + vp.DatabaseColumnName + Include.cLockExtension + ", '0') " + vp.DatabaseColumnName + Include.cLockExtension;
                    if (count < aDatabaseVariables.Count)
                    {
                        line += ", ";
                    }
                    if (line.Length > 90)
                    {
                        aWriter.WriteLine(line);
                        line = _indent10;

                    }

                }
                if (line.Length > 0)
                {
                    aWriter.WriteLine(line);

                }
                aWriter.WriteLine("		FROM " + tempTableName + " t");
                // Begin TT#3731 - JSmith - Error when Saving
                //aWriter.WriteLine("	JOIN " + aTableName + " sfwl ON t.HN_RID = sfwl.HN_RID");
                aWriter.WriteLine("	JOIN " + aTableName + " sfwl with (nolock)  ON t.HN_RID = sfwl.HN_RID");
                // End TT#3731 - JSmith - Error when Saving
                aWriter.WriteLine("		AND sfwl.FV_RID = t.FV_RID");
                aWriter.WriteLine("		AND sfwl.TIME_ID = t.TIME_ID");
                aWriter.WriteLine("	OPTION (MAXDOP 1)");

                aWriter.WriteLine(System.Environment.NewLine);
                aWriter.WriteLine("GO");
                aWriter.WriteLine(System.Environment.NewLine);
            }
            catch
            {
                throw;
            }
        }
        private static void Generate_MID_ST_FOR_WK_LOCK_WRITE(genOptions options)
        {
            string line;
            int count = 0;
            try
            {

                //string aTableName = options.table;
                string aLockTableName = Include.DBStoreWeeklyLockTable; 
                //string aTempTableName, 
                ProfileList aDatabaseVariables = variables.GetStoreWeeklyForecastDatabaseVariableList();
                StreamWriter aWriter = options.writer;
                //string aTableCountColumn


                string procedureName = options.name;

                if (aDatabaseVariables == null ||
                    aDatabaseVariables.Count == 0)
                {
                    return;
                }

                aWriter.WriteLine("CREATE PROCEDURE [dbo].[" + procedureName + "]");
                aWriter.WriteLine(" ");
                aWriter.WriteLine("@dtLock " + Include.DBStoreWeeklyForecastLockType + " READONLY");
                aWriter.WriteLine("AS");
                aWriter.WriteLine("SET NOCOUNT ON");

                aWriter.WriteLine("MERGE INTO " + aLockTableName + " with (ROWLOCK) AS TARGET");
                aWriter.WriteLine("USING @dtLock AS SOURCE");
                aWriter.WriteLine("on (SOURCE.HN_RID = TARGET.HN_RID and SOURCE.FV_RID = TARGET.FV_RID and SOURCE.TIME_ID = TARGET.TIME_ID and SOURCE.ST_RID = TARGET.ST_RID)");
                aWriter.WriteLine("WHEN MATCHED THEN");
                aWriter.WriteLine("UPDATE ");
                count = 0;
                line = _blankLine;
                foreach (VariableProfile vp in aDatabaseVariables)
                {
                    ++count;
                    if (count == 1)
                    {
                        line = line.Insert(9, "SET");
                    }
                    line = line.Insert(13, "TARGET." + vp.DatabaseColumnName + "_LOCK");
                    line = line.TrimEnd();
                    line += " = COALESCE(SOURCE." + vp.DatabaseColumnName + "_LOCK, ";
                    line += "TARGET." + vp.DatabaseColumnName + "_LOCK)";
                    if (count < aDatabaseVariables.Count)
                    {
                        line += ",";
                    }
                    aWriter.WriteLine(line);
                    line = _blankLine;
                }

                aWriter.WriteLine("WHEN NOT MATCHED BY TARGET THEN");
                line = "INSERT (HN_RID, FV_RID, TIME_ID, ST_RID, ";
                count = 0;
                foreach (VariableProfile vp in aDatabaseVariables)
                {
                    ++count;
                    line += vp.DatabaseColumnName + "_LOCK";
                    if (count < aDatabaseVariables.Count)
                    {
                        line += ", ";
                    }
                    if (line.Length > 150)
                    {
                        aWriter.WriteLine(line);
                        line = _indent5;

                    }
                }
                line += ")";
                aWriter.WriteLine(line);

                count = 0;
                line = "VALUES ( SOURCE.HN_RID, SOURCE.FV_RID, SOURCE.TIME_ID, SOURCE.ST_RID,";
                foreach (VariableProfile vp in aDatabaseVariables)
                {
                    ++count;
                    line += " SOURCE." + vp.DatabaseColumnName + "_LOCK";
                    if (count < aDatabaseVariables.Count)
                    {
                        line += ",";
                    }
                    if (line.Length > 110)
                    {
                        aWriter.WriteLine(line);
                        line = _indent5;

                    }
                }
                line += ");";
                if (line.Length > 0)
                {
                    aWriter.WriteLine(line);
                }

                aWriter.WriteLine(System.Environment.NewLine);
                aWriter.WriteLine("GO");
                aWriter.WriteLine(System.Environment.NewLine);
            }
            catch
            {
                throw;
            }
        }
        private static void Generate_MID_CHN_HIS_WK_WRITE_TYPES(genOptions options)
        {
            string line;
            int count = 0;
            try
            {
                string aProcedureName = options.name;
                string aTableName = options.table;
                //string aTempTableName, string aTableCountColumn, 
                ProfileList aDatabaseVariables = variables.GetChainWeeklyHistoryDatabaseVariableList();
                StreamWriter aWriter = options.writer;


                //AddStoredProcedureDrop(aProcedureName, aWriter);


                AddTypeDrop(Include.DBChainWeeklyHistoryType, aWriter);

                if (aDatabaseVariables == null ||
                    aDatabaseVariables.Count == 0)
                {
                    return;
                }

                aWriter.WriteLine("CREATE TYPE [dbo].[" + Include.DBChainWeeklyHistoryType + "] AS TABLE(");
                aWriter.WriteLine("  [HN_RID] [int],");
                aWriter.WriteLine("  [TIME_ID] [int],");
                foreach (VariableProfile vp in aDatabaseVariables)
                {
                    ++count;
                    line = "  [" + vp.DatabaseColumnName + "] [";
                    switch (vp.ChainDatabaseVariableType)
                    {
                        case eVariableDatabaseType.Integer:
                            line += "int]";
                            break;
                        case eVariableDatabaseType.Float:
                            line += "float]";
                            break;
                        case eVariableDatabaseType.DateTime:
                            line += "smalldatetime]";
                            break;
                        case eVariableDatabaseType.Real:
                            line += "real]";
                            break;
                        case eVariableDatabaseType.BigInteger:
                            line += "bigint]";
                            break;
                    }
                    if (count < aDatabaseVariables.Count)
                    {
                        line += ", ";
                    }

                    aWriter.WriteLine(line);
                }
                aWriter.WriteLine("PRIMARY KEY (HN_RID, TIME_ID)");
                aWriter.WriteLine(")");

                aWriter.WriteLine(System.Environment.NewLine);
                aWriter.WriteLine("GO");
                aWriter.WriteLine(System.Environment.NewLine);

                //aWriter.WriteLine("CREATE PROCEDURE [dbo].[" + aProcedureName + "]");
                //aWriter.WriteLine(" ");
                //aWriter.WriteLine("@dt " + Include.DBChainWeeklyHistoryType + " READONLY");
                //aWriter.WriteLine("AS");
                //aWriter.WriteLine("SET NOCOUNT ON");

                //aWriter.WriteLine("MERGE INTO " + aTableName + " with (ROWLOCK) AS TARGET");
                //aWriter.WriteLine("USING @dt AS SOURCE");
                //aWriter.WriteLine("on (SOURCE.HN_RID = TARGET.HN_RID and SOURCE.TIME_ID = TARGET.TIME_ID)");
                //aWriter.WriteLine("WHEN MATCHED THEN");
                //aWriter.WriteLine("UPDATE ");
                //count = 0;
                //line = _blankLine;
                //foreach (VariableProfile vp in aDatabaseVariables)
                //{
                //    ++count;
                //    if (count == 1)
                //    {
                //        line = line.Insert(9, "SET");
                //    }
                //    line = line.Insert(13, "TARGET." + vp.DatabaseColumnName);
                //    line = line.TrimEnd();
                //    line += " = COALESCE(SOURCE." + vp.DatabaseColumnName + ", ";
                //    line += "TARGET." + vp.DatabaseColumnName + ")";
                //    if (count < aDatabaseVariables.Count)
                //    {
                //        line += ",";
                //    }
                //    aWriter.WriteLine(line);
                //    line = _blankLine;
                //}

                //aWriter.WriteLine("WHEN NOT MATCHED BY TARGET THEN");
                //line = "INSERT (HN_RID, TIME_ID, ";
                //count = 0;
                //foreach (VariableProfile vp in aDatabaseVariables)
                //{
                //    ++count;
                //    line += vp.DatabaseColumnName;
                //    if (count < aDatabaseVariables.Count)
                //    {
                //        line += ", ";
                //    }
                //    if (line.Length > 150)
                //    {
                //        aWriter.WriteLine(line);
                //        line = _indent5;

                //    }
                //}
                //line += ")";
                //aWriter.WriteLine(line);

                //count = 0;
                //line = "VALUES ( SOURCE.HN_RID, SOURCE.TIME_ID,";
                //foreach (VariableProfile vp in aDatabaseVariables)
                //{
                //    ++count;
                //    line += " SOURCE." + vp.DatabaseColumnName;
                //    if (count < aDatabaseVariables.Count)
                //    {
                //        line += ",";
                //    }
                //    if (line.Length > 110)
                //    {
                //        aWriter.WriteLine(line);
                //        line = _indent5;

                //    }
                //}
                //line += ");";
                //if (line.Length > 0)
                //{
                //    aWriter.WriteLine(line);
                //}

                //aWriter.WriteLine(System.Environment.NewLine);
                //aWriter.WriteLine("GO");
                //aWriter.WriteLine(System.Environment.NewLine);
            }
            catch
            {
                throw;
            }
        }
        private static void Generate_MID_CHN_HIS_WK_WRITE(genOptions options)
        {
            string line;
            int count = 0;
            try
            {
                string aProcedureName = options.name;
                string aTableName = options.table;
                //string aTempTableName, string aTableCountColumn, 
                ProfileList aDatabaseVariables = variables.GetChainWeeklyHistoryDatabaseVariableList();
                StreamWriter aWriter = options.writer;


                //AddStoredProcedureDrop(aProcedureName, aWriter);


                //AddTypeDrop(Include.DBChainWeeklyHistoryType, aWriter);

                if (aDatabaseVariables == null ||
                    aDatabaseVariables.Count == 0)
                {
                    return;
                }

                //aWriter.WriteLine("CREATE TYPE [dbo].[" + Include.DBChainWeeklyHistoryType + "] AS TABLE(");
                //aWriter.WriteLine("  [HN_RID] [int],");
                //aWriter.WriteLine("  [TIME_ID] [int],");
                //foreach (VariableProfile vp in aDatabaseVariables)
                //{
                //    ++count;
                //    line = "  [" + vp.DatabaseColumnName + "] [";
                //    switch (vp.ChainDatabaseVariableType)
                //    {
                //        case eVariableDatabaseType.Integer:
                //            line += "int]";
                //            break;
                //        case eVariableDatabaseType.Float:
                //            line += "float]";
                //            break;
                //        case eVariableDatabaseType.DateTime:
                //            line += "smalldatetime]";
                //            break;
                //        case eVariableDatabaseType.Real:
                //            line += "real]";
                //            break;
                //        case eVariableDatabaseType.BigInteger:
                //            line += "bigint]";
                //            break;
                //    }
                //    if (count < aDatabaseVariables.Count)
                //    {
                //        line += ", ";
                //    }

                //    aWriter.WriteLine(line);
                //}
                //aWriter.WriteLine("PRIMARY KEY (HN_RID, TIME_ID)");
                //aWriter.WriteLine(")");

                //aWriter.WriteLine(System.Environment.NewLine);
                //aWriter.WriteLine("GO");
                //aWriter.WriteLine(System.Environment.NewLine);

                aWriter.WriteLine("CREATE PROCEDURE [dbo].[" + aProcedureName + "]");
                aWriter.WriteLine(" ");
                aWriter.WriteLine("@dt " + Include.DBChainWeeklyHistoryType + " READONLY");
                aWriter.WriteLine("AS");
                aWriter.WriteLine("SET NOCOUNT ON");

                aWriter.WriteLine("MERGE INTO " + aTableName + " with (ROWLOCK) AS TARGET");
                aWriter.WriteLine("USING @dt AS SOURCE");
                aWriter.WriteLine("on (SOURCE.HN_RID = TARGET.HN_RID and SOURCE.TIME_ID = TARGET.TIME_ID)");
                aWriter.WriteLine("WHEN MATCHED THEN");
                aWriter.WriteLine("UPDATE ");
                count = 0;
                line = _blankLine;
                foreach (VariableProfile vp in aDatabaseVariables)
                {
                    ++count;
                    if (count == 1)
                    {
                        line = line.Insert(9, "SET");
                    }
                    line = line.Insert(13, "TARGET." + vp.DatabaseColumnName);
                    line = line.TrimEnd();
                    line += " = COALESCE(SOURCE." + vp.DatabaseColumnName + ", ";
                    line += "TARGET." + vp.DatabaseColumnName + ")";
                    if (count < aDatabaseVariables.Count)
                    {
                        line += ",";
                    }
                    aWriter.WriteLine(line);
                    line = _blankLine;
                }

                aWriter.WriteLine("WHEN NOT MATCHED BY TARGET THEN");
                line = "INSERT (HN_RID, TIME_ID, ";
                count = 0;
                foreach (VariableProfile vp in aDatabaseVariables)
                {
                    ++count;
                    line += vp.DatabaseColumnName;
                    if (count < aDatabaseVariables.Count)
                    {
                        line += ", ";
                    }
                    if (line.Length > 150)
                    {
                        aWriter.WriteLine(line);
                        line = _indent5;

                    }
                }
                line += ")";
                aWriter.WriteLine(line);

                count = 0;
                line = "VALUES ( SOURCE.HN_RID, SOURCE.TIME_ID,";
                foreach (VariableProfile vp in aDatabaseVariables)
                {
                    ++count;
                    line += " SOURCE." + vp.DatabaseColumnName;
                    if (count < aDatabaseVariables.Count)
                    {
                        line += ",";
                    }
                    if (line.Length > 110)
                    {
                        aWriter.WriteLine(line);
                        line = _indent5;

                    }
                }
                line += ");";
                if (line.Length > 0)
                {
                    aWriter.WriteLine(line);
                }

                aWriter.WriteLine(System.Environment.NewLine);
                aWriter.WriteLine("GO");
                aWriter.WriteLine(System.Environment.NewLine);
            }
            catch
            {
                throw;
            }
        }

        private static void Generate_MID_CHN_HIS_WK_READ(genOptions options)
        {
            try
            {
                string aProcedureName = options.name;
                string aTableName = options.table;
                string aViewName = Include.DBChainWeeklyHistoryView;
                //string aTableCountColumn, 
                string aTempTableName = Include.DBTempTableName;
                ProfileList aDatabaseVariables = variables.GetChainWeeklyHistoryDatabaseVariableList();
                StreamWriter aWriter = options.writer;

                AddStoredProcedureDrop(aProcedureName, aWriter);
                //AddTypeDrop(Include.DBChainWeeklyHistoryReadType, aWriter);

                if (aDatabaseVariables == null ||
                    aDatabaseVariables.Count == 0)
                {
                    return;
                }

                string tempTableName = "@dt";

                //aWriter.WriteLine("CREATE TYPE [dbo].[" + Include.DBChainWeeklyHistoryReadType + "] AS TABLE(");
                //aWriter.WriteLine("  [HN_RID] [int],");
                //aWriter.WriteLine("  [TIME_ID] [int]");
                //aWriter.WriteLine("PRIMARY KEY (HN_RID, TIME_ID)");
                //aWriter.WriteLine(")");

                //aWriter.WriteLine(System.Environment.NewLine);
                //aWriter.WriteLine("GO");
                //aWriter.WriteLine(System.Environment.NewLine);

                aWriter.WriteLine("CREATE PROCEDURE [dbo].[" + aProcedureName + "]");
                aWriter.WriteLine("@dt " + Include.DBChainWeeklyHistoryReadType + " READONLY,");
                aWriter.WriteLine("@Rollup CHAR(1) = NULL");
                aWriter.WriteLine("AS");
                aWriter.WriteLine("SET NOCOUNT ON");
                aWriter.WriteLine("DECLARE @idoc int,");
                aWriter.WriteLine("        @Tables INT,");
                aWriter.WriteLine("        @Loop INT,");
                aWriter.WriteLine("        @HN_TYPE INT,");
                aWriter.WriteLine("        @HN_RID INT,");
                aWriter.WriteLine("        @HN_MOD INT,");
                aWriter.WriteLine("        @ROLL_OPTION INT,");
                aWriter.WriteLine("        @LoopCount INT,");
                aWriter.WriteLine("        @NextLoopCount INT");
                //aWriter.WriteLine("SELECT @Tables = " + aTableCountColumn + " FROM SYSTEM_OPTIONS");
                aWriter.WriteLine("SET @Tables = 10");

                aWriter.WriteLine("SELECT DISTINCT @HN_TYPE = ph.PH_TYPE, @HN_RID = t.HN_RID, @HN_MOD = t.HN_RID % @Tables,");
                aWriter.WriteLine("                @ROLL_OPTION = COALESCE(ph.HISTORY_ROLL_OPTION, 800251)");
                aWriter.WriteLine("        FROM HIERARCHY_NODE hn (NOLOCK)");
                aWriter.WriteLine("        JOIN PRODUCT_HIERARCHY ph (NOLOCK) ON ph.PH_RID = hn.HOME_PH_RID");
                aWriter.WriteLine("        JOIN " + tempTableName + " t on t.HN_RID = hn.HN_RID");
                aWriter.WriteLine("IF @HN_TYPE = 800001 and @ROLL_OPTION = 800251 -- Realtime");
                aWriter.WriteLine("   BEGIN");
                aWriter.WriteLine("   SET @LoopCount = 0");
                aWriter.WriteLine("   CREATE TABLE #TREE (LOOPCOUNT INT NOT NULL, PARENT_HN_RID INT NOT NULL, HOME_PH_RID INT, PH_TYPE INT, CHILD_HN_RID INT NOT NULL, CHILD_HN_MOD INT NOT NULL)");
                aWriter.WriteLine("   -- insert the children of the node into the temp table");
                aWriter.WriteLine("   INSERT #TREE (LOOPCOUNT, PARENT_HN_RID, HOME_PH_RID, PH_TYPE, CHILD_HN_RID, CHILD_HN_MOD) ");
                aWriter.WriteLine("       select @LoopCount as LOOPCOUNT, @HN_RID as PARENT_HN_RID, hn.HOME_PH_RID, ph.PH_TYPE, hnj.HN_RID, hnj.HN_RID % @Tables");
                aWriter.WriteLine("         from HIER_NODE_JOIN hnj (NOLOCK)");
                aWriter.WriteLine("           JOIN HIERARCHY_NODE hn (NOLOCK) ON hn.HN_RID = hnj.HN_RID");
                aWriter.WriteLine("           JOIN PRODUCT_HIERARCHY ph (NOLOCK) ON ph.PH_RID = hn.HOME_PH_RID");
                aWriter.WriteLine("       where @HN_RID = hnj.PARENT_HN_RID");
                aWriter.WriteLine("   SET @Loop = @@ROWCOUNT");
                aWriter.WriteLine("   -- chase all paths until you get the main hierarchy (type 800000) or the lowest leaf");
                aWriter.WriteLine("   SET @NextLoopCount = @LoopCount + 1");
                aWriter.WriteLine("   WHILE @Loop > 0");
                aWriter.WriteLine("   BEGIN");
                aWriter.WriteLine("      INSERT #TREE");
                aWriter.WriteLine("        select @NextLoopCount as LOOPCOUNT, hnj.PARENT_HN_RID, hn.HOME_PH_RID, ph.PH_TYPE, hnj.HN_RID, hnj.HN_RID % @Tables");
                aWriter.WriteLine("          from HIER_NODE_JOIN hnj (NOLOCK)");
                aWriter.WriteLine("            JOIN HIERARCHY_NODE hn (NOLOCK) ON hn.HN_RID = hnj.HN_RID");
                aWriter.WriteLine("            JOIN PRODUCT_HIERARCHY ph (NOLOCK) ON ph.PH_RID = hn.HOME_PH_RID");
                aWriter.WriteLine("            JOIN #TREE t ON hnj.PARENT_HN_RID = t.CHILD_HN_RID");
                aWriter.WriteLine("        WHERE t.LOOPCOUNT =  @LoopCount AND t.PH_TYPE <> 800000");
                aWriter.WriteLine("      SET @Loop = @@ROWCOUNT");
                aWriter.WriteLine("      SET @LoopCount = @LoopCount + 1");
                aWriter.WriteLine("      SET @NextLoopCount = @LoopCount + 1");
                aWriter.WriteLine("   END");
                aWriter.WriteLine("   -- join with dates from xml selecting only nodes from the main hierarchy or lowest leaf alternates");
                aWriter.WriteLine("   SELECT * ");
                aWriter.WriteLine("     INTO " + aTempTableName + "2");
                aWriter.WriteLine("     FROM #TREE");
                aWriter.WriteLine("     CROSS JOIN " + tempTableName);
                aWriter.WriteLine("   where LOOPCOUNT = @LoopCount - 1");
                aWriter.WriteLine("	     or PH_TYPE = 800000");

                int count = 0;
                string line = "       SELECT @HN_RID AS HN_RID, 1 AS FV_RID, chw.TIME_ID, ";
                // add variables
                foreach (VariableProfile vp in aDatabaseVariables)
                {
                    ++count;
                    line += "COALESCE(SUM(" + vp.DatabaseColumnName + "), 0) " + vp.DatabaseColumnName + ", ";
                    if (line.Length > 100)
                    {
                        aWriter.WriteLine(line);
                        line = _indent10;

                    }

                }
                if (line.Length > 0)
                {
                    aWriter.WriteLine(line);

                }
                // add locks
                count = 0;
                line = _indent10;
                // add variables
                foreach (VariableProfile vp in aDatabaseVariables)
                {
                    ++count;
                    line += "0 AS " + vp.DatabaseColumnName + Include.cLockExtension;
                    if (count < aDatabaseVariables.Count)
                    {
                        line += ", ";
                    }
                    if (line.Length > 100)
                    {
                        aWriter.WriteLine(line);
                        line = _indent10;

                    }

                }
                if (line.Length > 0)
                {
                    aWriter.WriteLine(line);

                }
                aWriter.WriteLine("		FROM " + aTempTableName + "2 t");
                aWriter.WriteLine("		JOIN " + aViewName + " chw (NOLOCK) ON t.CHILD_HN_RID = chw.HN_RID");
                aWriter.WriteLine("			AND chw.TIME_ID = t.TIME_ID");
                aWriter.WriteLine("		GROUP BY chw.TIME_ID");
                aWriter.WriteLine("		OPTION (MAXDOP 1)");
                aWriter.WriteLine("	RETURN 0");
                aWriter.WriteLine("	END");
                aWriter.WriteLine("-- Process variables");
                aWriter.WriteLine("-- GET ALL THE ROWS");
                aWriter.WriteLine("IF @Rollup = 'Y'");
                count = 0;
                line = "       SELECT chw.HN_RID, 1 AS FV_RID, ";
                // add variables
                foreach (VariableProfile vp in aDatabaseVariables)
                {
                    ++count;
                    line += "COALESCE(SUM(chw." + vp.DatabaseColumnName + "), 0) " + vp.DatabaseColumnName + ", ";
                    if (line.Length > 90)
                    {
                        aWriter.WriteLine(line);
                        line = _indent10;

                    }

                }
                if (line.Length > 0)
                {
                    aWriter.WriteLine(line);

                }
                // add locks
                count = 0;
                line = _indent10;
                // add variables
                foreach (VariableProfile vp in aDatabaseVariables)
                {
                    ++count;
                    line += "0 AS " + vp.DatabaseColumnName + Include.cLockExtension;
                    if (count < aDatabaseVariables.Count)
                    {
                        line += ", ";
                    }
                    if (line.Length > 90)
                    {
                        aWriter.WriteLine(line);
                        line = _indent10;

                    }

                }
                if (line.Length > 0)
                {
                    aWriter.WriteLine(line);

                }
                aWriter.WriteLine("        FROM " + tempTableName + " xml");
                aWriter.WriteLine("	JOIN " + aViewName + " chw ON xml.HN_RID = chw.HN_RID");
                aWriter.WriteLine("		AND chw.TIME_ID = xml.TIME_ID");
                aWriter.WriteLine("	GROUP BY chw.HN_RID");
                aWriter.WriteLine("	OPTION (MAXDOP 1)");
                aWriter.WriteLine("ELSE");
                count = 0;
                line = "       SELECT chw.HN_RID, 1 AS FV_RID, chw.TIME_ID, ";
                // add variables
                foreach (VariableProfile vp in aDatabaseVariables)
                {
                    ++count;
                    line += "COALESCE(chw." + vp.DatabaseColumnName + ", 0) " + vp.DatabaseColumnName + ", ";
                    if (line.Length > 90)
                    {
                        aWriter.WriteLine(line);
                        line = _indent10;

                    }

                }
                if (line.Length > 0)
                {
                    aWriter.WriteLine(line);

                }
                // add locks
                count = 0;
                line = _indent10;
                // add variables
                foreach (VariableProfile vp in aDatabaseVariables)
                {
                    ++count;
                    line += "0 AS " + vp.DatabaseColumnName + Include.cLockExtension;
                    if (count < aDatabaseVariables.Count)
                    {
                        line += ", ";
                    }
                    if (line.Length > 90)
                    {
                        aWriter.WriteLine(line);
                        line = _indent10;
                    }

                }
                if (line.Length > 0)
                {
                    aWriter.WriteLine(line);

                }
                aWriter.WriteLine("		FROM " + tempTableName + " xml");
                aWriter.WriteLine("	JOIN " + aViewName + " chw ON xml.HN_RID = chw.HN_RID");
                aWriter.WriteLine("		AND chw.TIME_ID = xml.TIME_ID");
                aWriter.WriteLine("	OPTION (MAXDOP 1)");

                aWriter.WriteLine(System.Environment.NewLine);
                //aWriter.WriteLine("if (select object_id('tempdb.dbo." + aTempTableName + "')) > 0 DROP TABLE " + aTempTableName);
                aWriter.WriteLine("if (select object_id('tempdb.dbo." + aTempTableName + "2')) > 0 DROP TABLE " + aTempTableName + "2");
                aWriter.WriteLine(System.Environment.NewLine);

                aWriter.WriteLine(System.Environment.NewLine);
                aWriter.WriteLine("GO");
                aWriter.WriteLine(System.Environment.NewLine);
            }
            catch
            {
                throw;
            }
        }
        private static void Generate_MID_CHN_HIS_WK_ROLLUP(genOptions options)
        {
            ChainHistoryWeekRollupProcess rollupProcess;
            string procedureName;
            bool includeZeroInAverage = true;
            bool honorLocks = false;
            bool zeroParentsWithNoChildren = true;

            try
            {
                ProfileList chainWeeklyHistoryRollupVariables = new ProfileList(eProfileType.Variable);
                foreach (VariableProfile vp in variables.GetChainWeeklyHistoryDatabaseVariableList())
                {
                    // Begin TT#3158 - JSmith - Planning History Rollup
                    //if (vp.LevelRollType != eLevelRollType.None &&
                    //                    vp.ChainHistoryModelType != eVariableDatabaseModelType.None)
                    if (vp.LevelRollType != eLevelRollType.None &&
                                        vp.ChainHistoryModelType != eVariableDatabaseModelType.None &&
                        (vp.VariableCategory == eVariableCategory.Chain || vp.VariableCategory == eVariableCategory.Both))
                    // End TT#3158 - JSmith - Planning History Rollup
                    {
                        chainWeeklyHistoryRollupVariables.Add(vp);
                    }
                }



                ProfileList aDatabaseVariables = chainWeeklyHistoryRollupVariables;
                StreamWriter aWriter = options.writer;


                procedureName = Include.DBChainWeeklyHistoryRollupSP;
                AddStoredProcedureDrop(procedureName, aWriter);
                rollupProcess = new ChainHistoryWeekRollupProcess(null, null, aDatabaseVariables.ArrayList, 0, 0, 0, 0, 0, 0, includeZeroInAverage, honorLocks, zeroParentsWithNoChildren, true, null);
                aWriter.Write(rollupProcess.BuildStoredProcedure(procedureName));
                aWriter.WriteLine(System.Environment.NewLine);
                aWriter.WriteLine("GO");
                aWriter.WriteLine(System.Environment.NewLine);

                zeroParentsWithNoChildren = false;
                procedureName = Include.DBChainWeeklyHistoryNoZeroRollupSP;
                AddStoredProcedureDrop(procedureName, aWriter);
                rollupProcess = new ChainHistoryWeekRollupProcess(null, null, aDatabaseVariables.ArrayList, 0, 0, 0, 0, 0, 0, includeZeroInAverage, honorLocks, zeroParentsWithNoChildren, true, null);
                aWriter.Write(rollupProcess.BuildStoredProcedure(procedureName));
                aWriter.WriteLine(System.Environment.NewLine);
                aWriter.WriteLine("GO");
                aWriter.WriteLine(System.Environment.NewLine);
            }
            catch
            {
                throw;
            }
        }
        private static void Generate_MID_CHN_FOR_WK_WRITE_TYPES(genOptions options)
        {
            string line;
            int count = 0;
            try
            {
                string aProcedureName = options.name;
                string aTableName = options.table;
                string aLockTableName = Include.DBChainWeeklyLockTable;
                //string aTempTableName, 
                //string aTableCountColumn,
                ProfileList aDatabaseVariables = variables.GetChainWeeklyForecastDatabaseVariableList();
                StreamWriter aWriter = options.writer;



                //AddStoredProcedureDrop(aProcedureName, aWriter);

                AddTypeDrop(Include.DBChainWeeklyForecastType, aWriter);
                AddTypeDrop(Include.DBChainWeeklyForecastLockType, aWriter);

                if (aDatabaseVariables == null ||
                    aDatabaseVariables.Count == 0)
                {
                    return;
                }

                aWriter.WriteLine("CREATE TYPE [dbo].[" + Include.DBChainWeeklyForecastType + "] AS TABLE(");
                aWriter.WriteLine("  [HN_RID] [int],");
                aWriter.WriteLine("  [FV_RID] [int],");
                aWriter.WriteLine("  [TIME_ID] [int],");
                foreach (VariableProfile vp in aDatabaseVariables)
                {
                    ++count;
                    line = "  [" + vp.DatabaseColumnName + "] [";
                    switch (vp.ChainDatabaseVariableType)
                    {
                        case eVariableDatabaseType.Integer:
                            line += "int]";
                            break;
                        case eVariableDatabaseType.Float:
                            line += "float]";
                            break;
                        case eVariableDatabaseType.DateTime:
                            line += "smalldatetime]";
                            break;
                        case eVariableDatabaseType.Real:
                            line += "real]";
                            break;
                        case eVariableDatabaseType.BigInteger:
                            line += "bigint]";
                            break;
                    }
                    if (count < aDatabaseVariables.Count)
                    {
                        line += ", ";
                    }

                    aWriter.WriteLine(line);
                }
                aWriter.WriteLine("PRIMARY KEY (HN_RID, FV_RID, TIME_ID)");
                aWriter.WriteLine(")");

                //aWriter.WriteLine(System.Environment.NewLine);
                aWriter.WriteLine("GO");
                aWriter.WriteLine(System.Environment.NewLine);

                count = 0;
                aWriter.WriteLine("CREATE TYPE [dbo].[" + Include.DBChainWeeklyForecastLockType + "] AS TABLE(");
                aWriter.WriteLine("  [HN_RID] [int],");
                aWriter.WriteLine("  [FV_RID] [int],");
                aWriter.WriteLine("  [TIME_ID] [int],");
                foreach (VariableProfile vp in aDatabaseVariables)
                {
                    ++count;
                    line = "  [" + vp.DatabaseColumnName + "_LOCK] [char]";
                    if (count < aDatabaseVariables.Count)
                    {
                        line += ", ";
                    }

                    aWriter.WriteLine(line);
                }
                aWriter.WriteLine("PRIMARY KEY (HN_RID, FV_RID, TIME_ID)");
                aWriter.WriteLine(")");

                //aWriter.WriteLine(System.Environment.NewLine);
                aWriter.WriteLine("GO");
                aWriter.WriteLine(System.Environment.NewLine);

                //aWriter.WriteLine("CREATE PROCEDURE [dbo].[" + aProcedureName + "]");
                //aWriter.WriteLine(" ");
                //aWriter.WriteLine("@dt " + Include.DBChainWeeklyForecastType + " READONLY,");
                //aWriter.WriteLine("@dtLock " + Include.DBChainWeeklyForecastLockType + " READONLY,");
                //aWriter.WriteLine("@SaveLocks CHAR");
                //aWriter.WriteLine("AS");
                //aWriter.WriteLine("SET NOCOUNT ON");
                //aWriter.WriteLine("MERGE INTO " + aTableName + " with (ROWLOCK) AS TARGET");
                //aWriter.WriteLine("USING @dt AS SOURCE");
                //aWriter.WriteLine("on (SOURCE.HN_RID = TARGET.HN_RID and SOURCE.FV_RID = TARGET.FV_RID and SOURCE.TIME_ID = TARGET.TIME_ID)");
                //aWriter.WriteLine("WHEN MATCHED THEN");
                //aWriter.WriteLine("UPDATE ");
                //count = 0;
                //line = _blankLine;
                //foreach (VariableProfile vp in aDatabaseVariables)
                //{
                //    ++count;
                //    if (count == 1)
                //    {
                //        line = line.Insert(9, "SET");
                //    }
                //    line = line.Insert(13, "TARGET." + vp.DatabaseColumnName);
                //    line = line.TrimEnd();
                //    line += " = COALESCE(SOURCE." + vp.DatabaseColumnName + ", ";
                //    line += "TARGET." + vp.DatabaseColumnName + ")";
                //    if (count < aDatabaseVariables.Count)
                //    {
                //        line += ",";
                //    }
                //    aWriter.WriteLine(line);
                //    line = _blankLine;
                //}

                //aWriter.WriteLine("WHEN NOT MATCHED BY TARGET THEN");
                //line = "INSERT (HN_RID, FV_RID, TIME_ID, ";
                //count = 0;
                //foreach (VariableProfile vp in aDatabaseVariables)
                //{
                //    ++count;
                //    line += vp.DatabaseColumnName;
                //    if (count < aDatabaseVariables.Count)
                //    {
                //        line += ", ";
                //    }
                //    if (line.Length > 150)
                //    {
                //        aWriter.WriteLine(line);
                //        line = _indent5;

                //    }
                //}
                //line += ")";
                //aWriter.WriteLine(line);

                //count = 0;
                //line = "VALUES ( SOURCE.HN_RID, SOURCE.FV_RID, SOURCE.TIME_ID,";
                //foreach (VariableProfile vp in aDatabaseVariables)
                //{
                //    ++count;
                //    line += " SOURCE." + vp.DatabaseColumnName;
                //    if (count < aDatabaseVariables.Count)
                //    {
                //        line += ",";
                //    }
                //    if (line.Length > 110)
                //    {
                //        aWriter.WriteLine(line);
                //        line = _indent5;

                //    }
                //}
                //line += ");";
                //if (line.Length > 0)
                //{
                //    aWriter.WriteLine(line);
                //}

                ////  process locks
                //aWriter.WriteLine("if @SaveLocks = '1'");
                //aWriter.WriteLine("begin");
                //aWriter.WriteLine("MERGE INTO " + aLockTableName + " with (ROWLOCK) AS TARGET");
                //aWriter.WriteLine("USING @dtLock AS SOURCE");
                //aWriter.WriteLine("on (SOURCE.HN_RID = TARGET.HN_RID and SOURCE.FV_RID = TARGET.FV_RID and SOURCE.TIME_ID = TARGET.TIME_ID)");
                //aWriter.WriteLine("WHEN MATCHED THEN");
                //aWriter.WriteLine("UPDATE ");
                //count = 0;
                //line = _blankLine;
                //foreach (VariableProfile vp in aDatabaseVariables)
                //{
                //    ++count;
                //    if (count == 1)
                //    {
                //        line = line.Insert(9, "SET");
                //    }
                //    line = line.Insert(13, "TARGET." + vp.DatabaseColumnName + "_LOCK");
                //    line = line.TrimEnd();
                //    line += " = COALESCE(SOURCE." + vp.DatabaseColumnName + "_LOCK, ";
                //    line += "TARGET." + vp.DatabaseColumnName + "_LOCK)";
                //    if (count < aDatabaseVariables.Count)
                //    {
                //        line += ",";
                //    }
                //    aWriter.WriteLine(line);
                //    line = _blankLine;
                //}

                //aWriter.WriteLine("WHEN NOT MATCHED BY TARGET THEN");
                //line = "INSERT (HN_RID, FV_RID, TIME_ID, ";
                //count = 0;
                //foreach (VariableProfile vp in aDatabaseVariables)
                //{
                //    ++count;
                //    line += vp.DatabaseColumnName + "_LOCK";
                //    if (count < aDatabaseVariables.Count)
                //    {
                //        line += ", ";
                //    }
                //    if (line.Length > 150)
                //    {
                //        aWriter.WriteLine(line);
                //        line = _indent5;

                //    }
                //}
                //line += ")";
                //aWriter.WriteLine(line);

                //count = 0;
                //line = "VALUES ( SOURCE.HN_RID, SOURCE.FV_RID, SOURCE.TIME_ID,";
                //foreach (VariableProfile vp in aDatabaseVariables)
                //{
                //    ++count;
                //    line += " SOURCE." + vp.DatabaseColumnName + "_LOCK";
                //    if (count < aDatabaseVariables.Count)
                //    {
                //        line += ",";
                //    }
                //    if (line.Length > 110)
                //    {
                //        aWriter.WriteLine(line);
                //        line = _indent5;

                //    }
                //}
                //line += ");";
                //if (line.Length > 0)
                //{
                //    aWriter.WriteLine(line);
                //}
                //aWriter.WriteLine("end");

                //aWriter.WriteLine(System.Environment.NewLine);
                //aWriter.WriteLine("GO");
                //aWriter.WriteLine(System.Environment.NewLine);
            }
            catch
            {
                throw;
            }
        }
        private static void Generate_MID_CHN_FOR_WK_WRITE(genOptions options)
        {
            string line;
            int count = 0;
            try
            {
                string aProcedureName = options.name;
                string aTableName = options.table;
                string aLockTableName = Include.DBChainWeeklyLockTable;
                //string aTempTableName, 
                //string aTableCountColumn,
                ProfileList aDatabaseVariables = variables.GetChainWeeklyForecastDatabaseVariableList();
                StreamWriter aWriter = options.writer;



                //AddStoredProcedureDrop(aProcedureName, aWriter);

                //AddTypeDrop(Include.DBChainWeeklyForecastType, aWriter);
                //AddTypeDrop(Include.DBChainWeeklyForecastLockType, aWriter);

                if (aDatabaseVariables == null ||
                    aDatabaseVariables.Count == 0)
                {
                    return;
                }

                //aWriter.WriteLine("CREATE TYPE [dbo].[" + Include.DBChainWeeklyForecastType + "] AS TABLE(");
                //aWriter.WriteLine("  [HN_RID] [int],");
                //aWriter.WriteLine("  [FV_RID] [int],");
                //aWriter.WriteLine("  [TIME_ID] [int],");
                //foreach (VariableProfile vp in aDatabaseVariables)
                //{
                //    ++count;
                //    line = "  [" + vp.DatabaseColumnName + "] [";
                //    switch (vp.ChainDatabaseVariableType)
                //    {
                //        case eVariableDatabaseType.Integer:
                //            line += "int]";
                //            break;
                //        case eVariableDatabaseType.Float:
                //            line += "float]";
                //            break;
                //        case eVariableDatabaseType.DateTime:
                //            line += "smalldatetime]";
                //            break;
                //        case eVariableDatabaseType.Real:
                //            line += "real]";
                //            break;
                //        case eVariableDatabaseType.BigInteger:
                //            line += "bigint]";
                //            break;
                //    }
                //    if (count < aDatabaseVariables.Count)
                //    {
                //        line += ", ";
                //    }

                //    aWriter.WriteLine(line);
                //}
                //aWriter.WriteLine("PRIMARY KEY (HN_RID, FV_RID, TIME_ID)");
                //aWriter.WriteLine(")");

                //aWriter.WriteLine(System.Environment.NewLine);
                //aWriter.WriteLine("GO");
                //aWriter.WriteLine(System.Environment.NewLine);

                //count = 0;
                //aWriter.WriteLine("CREATE TYPE [dbo].[" + Include.DBChainWeeklyForecastLockType + "] AS TABLE(");
                //aWriter.WriteLine("  [HN_RID] [int],");
                //aWriter.WriteLine("  [FV_RID] [int],");
                //aWriter.WriteLine("  [TIME_ID] [int],");
                //foreach (VariableProfile vp in aDatabaseVariables)
                //{
                //    ++count;
                //    line = "  [" + vp.DatabaseColumnName + "_LOCK] [char]";
                //    if (count < aDatabaseVariables.Count)
                //    {
                //        line += ", ";
                //    }

                //    aWriter.WriteLine(line);
                //}
                //aWriter.WriteLine("PRIMARY KEY (HN_RID, FV_RID, TIME_ID)");
                //aWriter.WriteLine(")");

                //aWriter.WriteLine(System.Environment.NewLine);
                //aWriter.WriteLine("GO");
                //aWriter.WriteLine(System.Environment.NewLine);

                aWriter.WriteLine("CREATE PROCEDURE [dbo].[" + aProcedureName + "]");
                aWriter.WriteLine(" ");
                aWriter.WriteLine("@dt " + Include.DBChainWeeklyForecastType + " READONLY,");
                aWriter.WriteLine("@dtLock " + Include.DBChainWeeklyForecastLockType + " READONLY,");
                aWriter.WriteLine("@SaveLocks CHAR");
                aWriter.WriteLine("AS");
                aWriter.WriteLine("SET NOCOUNT ON");
                aWriter.WriteLine("MERGE INTO " + aTableName + " with (ROWLOCK) AS TARGET");
                aWriter.WriteLine("USING @dt AS SOURCE");
                aWriter.WriteLine("on (SOURCE.HN_RID = TARGET.HN_RID and SOURCE.FV_RID = TARGET.FV_RID and SOURCE.TIME_ID = TARGET.TIME_ID)");
                aWriter.WriteLine("WHEN MATCHED THEN");
                aWriter.WriteLine("UPDATE ");
                count = 0;
                line = _blankLine;
                foreach (VariableProfile vp in aDatabaseVariables)
                {
                    ++count;
                    if (count == 1)
                    {
                        line = line.Insert(9, "SET");
                    }
                    line = line.Insert(13, "TARGET." + vp.DatabaseColumnName);
                    line = line.TrimEnd();
                    line += " = COALESCE(SOURCE." + vp.DatabaseColumnName + ", ";
                    line += "TARGET." + vp.DatabaseColumnName + ")";
                    if (count < aDatabaseVariables.Count)
                    {
                        line += ",";
                    }
                    aWriter.WriteLine(line);
                    line = _blankLine;
                }

                aWriter.WriteLine("WHEN NOT MATCHED BY TARGET THEN");
                line = "INSERT (HN_RID, FV_RID, TIME_ID, ";
                count = 0;
                foreach (VariableProfile vp in aDatabaseVariables)
                {
                    ++count;
                    line += vp.DatabaseColumnName;
                    if (count < aDatabaseVariables.Count)
                    {
                        line += ", ";
                    }
                    if (line.Length > 150)
                    {
                        aWriter.WriteLine(line);
                        line = _indent5;

                    }
                }
                line += ")";
                aWriter.WriteLine(line);

                count = 0;
                line = "VALUES ( SOURCE.HN_RID, SOURCE.FV_RID, SOURCE.TIME_ID,";
                foreach (VariableProfile vp in aDatabaseVariables)
                {
                    ++count;
                    line += " SOURCE." + vp.DatabaseColumnName;
                    if (count < aDatabaseVariables.Count)
                    {
                        line += ",";
                    }
                    if (line.Length > 110)
                    {
                        aWriter.WriteLine(line);
                        line = _indent5;

                    }
                }
                line += ");";
                if (line.Length > 0)
                {
                    aWriter.WriteLine(line);
                }

                //  process locks
                aWriter.WriteLine("if @SaveLocks = '1'");
                aWriter.WriteLine("begin");
                aWriter.WriteLine("MERGE INTO " + aLockTableName + " with (ROWLOCK) AS TARGET");
                aWriter.WriteLine("USING @dtLock AS SOURCE");
                aWriter.WriteLine("on (SOURCE.HN_RID = TARGET.HN_RID and SOURCE.FV_RID = TARGET.FV_RID and SOURCE.TIME_ID = TARGET.TIME_ID)");
                aWriter.WriteLine("WHEN MATCHED THEN");
                aWriter.WriteLine("UPDATE ");
                count = 0;
                line = _blankLine;
                foreach (VariableProfile vp in aDatabaseVariables)
                {
                    ++count;
                    if (count == 1)
                    {
                        line = line.Insert(9, "SET");
                    }
                    line = line.Insert(13, "TARGET." + vp.DatabaseColumnName + "_LOCK");
                    line = line.TrimEnd();
                    line += " = COALESCE(SOURCE." + vp.DatabaseColumnName + "_LOCK, ";
                    line += "TARGET." + vp.DatabaseColumnName + "_LOCK)";
                    if (count < aDatabaseVariables.Count)
                    {
                        line += ",";
                    }
                    aWriter.WriteLine(line);
                    line = _blankLine;
                }

                aWriter.WriteLine("WHEN NOT MATCHED BY TARGET THEN");
                line = "INSERT (HN_RID, FV_RID, TIME_ID, ";
                count = 0;
                foreach (VariableProfile vp in aDatabaseVariables)
                {
                    ++count;
                    line += vp.DatabaseColumnName + "_LOCK";
                    if (count < aDatabaseVariables.Count)
                    {
                        line += ", ";
                    }
                    if (line.Length > 150)
                    {
                        aWriter.WriteLine(line);
                        line = _indent5;

                    }
                }
                line += ")";
                aWriter.WriteLine(line);

                count = 0;
                line = "VALUES ( SOURCE.HN_RID, SOURCE.FV_RID, SOURCE.TIME_ID,";
                foreach (VariableProfile vp in aDatabaseVariables)
                {
                    ++count;
                    line += " SOURCE." + vp.DatabaseColumnName + "_LOCK";
                    if (count < aDatabaseVariables.Count)
                    {
                        line += ",";
                    }
                    if (line.Length > 110)
                    {
                        aWriter.WriteLine(line);
                        line = _indent5;

                    }
                }
                line += ");";
                if (line.Length > 0)
                {
                    aWriter.WriteLine(line);
                }
                aWriter.WriteLine("end");

                aWriter.WriteLine(System.Environment.NewLine);
                aWriter.WriteLine("GO");
                aWriter.WriteLine(System.Environment.NewLine);
            }
            catch
            {
                throw;
            }
        }
        private static void Generate_MID_CHN_FOR_WK_READ(genOptions options)
        {
            int count = 0;
            string line;
            try
            {
                string aProcedureName = options.name;
                string aTableName = options.table;
                string aViewName = Include.DBChainWeeklyForecastView;
                //string aTableCountColumn, string aTempTableName,
                ProfileList aDatabaseVariables = variables.GetChainWeeklyForecastDatabaseVariableList();
                StreamWriter aWriter = options.writer;


                AddStoredProcedureDrop(aProcedureName, aWriter);
                //AddTypeDrop(Include.DBChainWeeklyForecastReadType, aWriter);

                if (aDatabaseVariables == null ||
                    aDatabaseVariables.Count == 0)
                {
                    return;
                }

                string tempTableName = "@dt";

                //aWriter.WriteLine("CREATE TYPE [dbo].[" + Include.DBChainWeeklyForecastReadType + "] AS TABLE(");
                //aWriter.WriteLine("  [HN_RID] [int],");
                //aWriter.WriteLine("  [FV_RID] [int],");
                //aWriter.WriteLine("  [TIME_ID] [int]");
                //aWriter.WriteLine("PRIMARY KEY (HN_RID, FV_RID, TIME_ID)");
                //aWriter.WriteLine(")");

                //aWriter.WriteLine(System.Environment.NewLine);
                //aWriter.WriteLine("GO");
                //aWriter.WriteLine(System.Environment.NewLine);

                aWriter.WriteLine("CREATE PROCEDURE [dbo].[" + aProcedureName + "]");
                aWriter.WriteLine("@dt " + Include.DBChainWeeklyForecastReadType + " READONLY");
                aWriter.WriteLine("AS");
                aWriter.WriteLine("SET NOCOUNT ON");

                count = 0;
                line = "       SELECT cfw.HN_RID, cfw.FV_RID, cfw.TIME_ID, ";
                // add variables
                foreach (VariableProfile vp in aDatabaseVariables)
                {
                    ++count;
                    line += "COALESCE(cfw." + vp.DatabaseColumnName + ", 0) " + vp.DatabaseColumnName;
                    if (count < aDatabaseVariables.Count)
                    {
                        line += ", ";
                    }
                    if (line.Length > 90)
                    {
                        aWriter.WriteLine(line);
                        line = _indent10;

                    }

                }
                if (line.Length > 0)
                {
                    aWriter.WriteLine(line);

                }
                aWriter.WriteLine("		FROM " + tempTableName + " xml");
                aWriter.WriteLine("	JOIN " + aViewName + " cfw ON xml.HN_RID = cfw.HN_RID");
                aWriter.WriteLine("		AND cfw.FV_RID = xml.FV_RID");
                aWriter.WriteLine("		AND cfw.TIME_ID = xml.TIME_ID");
                aWriter.WriteLine("	OPTION (MAXDOP 1)");

                aWriter.WriteLine(System.Environment.NewLine);
                aWriter.WriteLine("GO");
                aWriter.WriteLine(System.Environment.NewLine);
            }
            catch
            {
                throw;
            }
        }
        private static void Generate_MID_CHN_FOR_WK_LOCK_READ(genOptions options)
        {
            int count = 0;
            string line;
            try
            {

                string aProcedureName = options.name;
                string aTableName = options.table;
                //string aViewName, string aTableCountColumn, string aTempTableName,
                ProfileList aDatabaseVariables=variables.GetChainWeeklyForecastDatabaseVariableList();
                StreamWriter aWriter = options.writer;

                AddStoredProcedureDrop(aProcedureName, aWriter);
                //AddTypeDrop(Include.DBChainWeeklyForecastReadLockType, aWriter);

                if (aDatabaseVariables == null ||
                    aDatabaseVariables.Count == 0)
                {
                    return;
                }

                string tempTableName = "@dt";

                //aWriter.WriteLine("CREATE TYPE [dbo].[" + Include.DBChainWeeklyForecastReadLockType + "] AS TABLE(");
                //aWriter.WriteLine("  [HN_RID] [int],");
                //aWriter.WriteLine("  [FV_RID] [int],");
                //aWriter.WriteLine("  [TIME_ID] [int]");
                //aWriter.WriteLine("PRIMARY KEY (HN_RID, FV_RID, TIME_ID)");
                //aWriter.WriteLine(")");

                //aWriter.WriteLine(System.Environment.NewLine);
                //aWriter.WriteLine("GO");
                //aWriter.WriteLine(System.Environment.NewLine);

                aWriter.WriteLine("CREATE PROCEDURE [dbo].[" + aProcedureName + "]");
                aWriter.WriteLine("@dt " + Include.DBChainWeeklyForecastReadLockType + " READONLY");
                aWriter.WriteLine("AS");
                aWriter.WriteLine("SET NOCOUNT ON");

                count = 0;
                line = "       SELECT cfwl.HN_RID, cfwl.FV_RID, cfwl.TIME_ID, ";
                // add variables
                foreach (VariableProfile vp in aDatabaseVariables)
                {
                    ++count;
                    line += "COALESCE(cfwl." + vp.DatabaseColumnName + Include.cLockExtension + ", '0') " + vp.DatabaseColumnName + Include.cLockExtension;
                    if (count < aDatabaseVariables.Count)
                    {
                        line += ", ";
                    }
                    if (line.Length > 90)
                    {
                        aWriter.WriteLine(line);
                        line = _indent10;

                    }

                }
                if (line.Length > 0)
                {
                    aWriter.WriteLine(line);

                }
                aWriter.WriteLine("		FROM " + tempTableName + " t");
                // Begin TT#3731 - JSmith - Error when Saving
                //aWriter.WriteLine("	JOIN " + aTableName + " cfwl ON t.HN_RID = cfwl.HN_RID");
                aWriter.WriteLine("	JOIN " + aTableName + " cfwl with (nolock) ON t.HN_RID = cfwl.HN_RID");
                // End TT#3731 - JSmith - Error when Saving
                aWriter.WriteLine("		AND cfwl.FV_RID = t.FV_RID");
                aWriter.WriteLine("		AND cfwl.TIME_ID = t.TIME_ID");
                aWriter.WriteLine("	OPTION (MAXDOP 1)");

                aWriter.WriteLine(System.Environment.NewLine);
                aWriter.WriteLine("GO");
                aWriter.WriteLine(System.Environment.NewLine);
            }
            catch
            {
                throw;
            }
        }
       
        private static void Generate_MID_CHN_MOD_WK_READ(genOptions options)
        {
            try
            {
                string aProcedureName = options.name;
                string aForecastTableName = Include.DBChainWeeklyForecastTable;

                string aHistoryTableName= Include.DBChainWeeklyHistoryTable;
                string aLockTableName = Include.DBChainWeeklyLockTable;
                string aForecastViewName = Include.DBChainWeeklyForecastView;
                string aHistoryViewName = Include.DBChainWeeklyHistoryView;
                //string aTableCountColumn, 
                string aTempTableName = Include.DBTempTableName;
                ProfileList aDatabaseVariables = variables.GetChainWeeklyForecastDatabaseVariableList();
                StreamWriter aWriter = options.writer;

                AddStoredProcedureDrop(aProcedureName, aWriter);
                //AddTypeDrop(Include.DBChainWeeklyModifiedReadType, aWriter);


                if (aDatabaseVariables == null ||
                    aDatabaseVariables.Count == 0)
                {
                    return;
                }

                string tempTableName = "@dt";

                //aWriter.WriteLine("CREATE TYPE [dbo].[" + Include.DBChainWeeklyModifiedReadType + "] AS TABLE(");
                //aWriter.WriteLine("  [HN_RID] [int],");
                //aWriter.WriteLine("  [FV_RID] [int],");
                //aWriter.WriteLine("  [TIME_ID] [int]");
                //aWriter.WriteLine("PRIMARY KEY (HN_RID, FV_RID, TIME_ID)");
                //aWriter.WriteLine(")");

                //aWriter.WriteLine(System.Environment.NewLine);
                //aWriter.WriteLine("GO");
                //aWriter.WriteLine(System.Environment.NewLine);


                string line = string.Empty;
                int count = 0;

                aWriter.WriteLine("CREATE PROCEDURE [dbo].[" + aProcedureName + "]");
                aWriter.WriteLine("@dt " + Include.DBChainWeeklyModifiedReadType + " READONLY,");
                aWriter.WriteLine("@Rollup CHAR(1) = NULL");
                aWriter.WriteLine("AS");
                aWriter.WriteLine("SET NOCOUNT ON");
                aWriter.WriteLine("DECLARE @Tables INT,");
                aWriter.WriteLine("        @Loop INT,");
                aWriter.WriteLine("        @HN_TYPE INT,");
                aWriter.WriteLine("        @HN_RID INT,");
                aWriter.WriteLine("        @FV_RID INT,");
                aWriter.WriteLine("        @HN_MOD INT,");
                aWriter.WriteLine("        @ROLL_OPTION INT,");
                aWriter.WriteLine("        @LoopCount INT,");
                aWriter.WriteLine("        @NextLoopCount INT");

                //aWriter.WriteLine("SELECT @Tables = " + aTableCountColumn + " FROM SYSTEM_OPTIONS");
                aWriter.WriteLine("SET @Tables = 10");

                aWriter.WriteLine("SELECT DISTINCT @HN_TYPE = ph.PH_TYPE, @HN_RID = t.HN_RID, @HN_MOD = t.HN_RID % @Tables,");
                aWriter.WriteLine("                @ROLL_OPTION = COALESCE(ph.HISTORY_ROLL_OPTION, 800251), @FV_RID = t.FV_RID");
                aWriter.WriteLine("        FROM HIERARCHY_NODE hn (NOLOCK)");
                aWriter.WriteLine("        JOIN PRODUCT_HIERARCHY ph (NOLOCK) ON ph.PH_RID = hn.HOME_PH_RID");
                aWriter.WriteLine("        JOIN " + tempTableName + " t on t.HN_RID = hn.HN_RID");

                aWriter.WriteLine(" ");
                aWriter.WriteLine(" -- build temp table of values and locks for modified version");
                aWriter.WriteLine(" create table " + aTempTableName + "MOD ");
                aWriter.WriteLine(" (HN_RID  int not null,");
                aWriter.WriteLine("  FV_RID  int not null,");
                aWriter.WriteLine("  TIME_ID int not null,");
                count = 0;
                // add variables
                line = "  ";
                foreach (VariableProfile vp in aDatabaseVariables)
                {
                    line += "  " + vp.DatabaseColumnName;
                    switch (vp.ChainDatabaseVariableType)
                    {
                        case eVariableDatabaseType.Integer:
                            line += " int ";
                            break;
                        case eVariableDatabaseType.Real:
                            line += " real ";
                            break;
                        case eVariableDatabaseType.DateTime:
                            line += " datetime ";
                            break;
                        case eVariableDatabaseType.String:
                            line += " varchar(100) ";
                            break;
                        case eVariableDatabaseType.Char:
                            line += " char(1) ";
                            break;
                        case eVariableDatabaseType.Float:
                            line += " float ";
                            break;
                        case eVariableDatabaseType.BigInteger:
                            line += " bigint ";
                            break;
                    }
                    line += "  null,";
                    aWriter.WriteLine(line);
                    line = "  ";
                }
                count = 0;
                // add locks
                line = "  ";
                foreach (VariableProfile vp in aDatabaseVariables)
                {
                    ++count;
                    line += "  " + vp.DatabaseColumnName + Include.cLockExtension + "   char(1) null";
                    if (count < aDatabaseVariables.Count)
                    {
                        line += ", ";
                    }
                    else
                    {
                        line += ")";
                    }
                    aWriter.WriteLine(line);
                    line = "  ";

                }
                if (line.Length > 0)
                {
                    aWriter.WriteLine(line);
                }

                aWriter.WriteLine(" ");
                aWriter.WriteLine(" insert into " + aTempTableName + "MOD");
                aWriter.WriteLine("  select cfw.HN_RID, cfw.FV_RID, cfw.TIME_ID,");
                foreach (VariableProfile vp in aDatabaseVariables)
                {
                    line += "  cfw." + vp.DatabaseColumnName + ", ";
                    if (line.Length > 80)
                    {
                        aWriter.WriteLine(line);
                        line = "  ";
                    }

                }
                if (line.Length > 0)
                {
                    aWriter.WriteLine(line);
                }
                count = 0;
                line = "  ";
                foreach (VariableProfile vp in aDatabaseVariables)
                {
                    ++count;
                    line += "  COALESCE(cfwl." + vp.DatabaseColumnName + Include.cLockExtension + ", 0) " + vp.DatabaseColumnName + Include.cLockExtension;
                    if (count < aDatabaseVariables.Count)
                    {
                        line += ", ";
                    }
                    aWriter.WriteLine(line);
                    line = "  ";

                }

                aWriter.WriteLine("   FROM " + tempTableName + " t");
                aWriter.WriteLine("   JOIN " + aForecastViewName + " cfw (NOLOCK)");
                aWriter.WriteLine("    on cfw.HN_RID = t.HN_RID");
                aWriter.WriteLine("    and cfw.FV_RID = t.FV_RID");
                aWriter.WriteLine("    and cfw.TIME_ID = t.TIME_ID");
                aWriter.WriteLine("   left outer join " + aLockTableName + " cfwl (NOLOCK)");
                aWriter.WriteLine("    on cfwl.HN_RID = cfw.HN_RID");
                aWriter.WriteLine("    and cfwl.FV_RID =cfw.FV_RID");
                aWriter.WriteLine("    and cfwl.TIME_ID = cfw.TIME_ID");
                aWriter.WriteLine("  union");
                aWriter.WriteLine("  select cfwl.HN_RID,  cfwl.FV_RID,  cfwl.TIME_ID, ");
                foreach (VariableProfile vp in aDatabaseVariables)
                {
                    line += "  cfw." + vp.DatabaseColumnName + ", ";
                    if (line.Length > 80)
                    {
                        aWriter.WriteLine(line);
                        line = "  ";
                    }

                }
                if (line.Length > 0)
                {
                    aWriter.WriteLine(line);
                }
                count = 0;
                line = "  ";
                foreach (VariableProfile vp in aDatabaseVariables)
                {
                    ++count;
                    line += "  COALESCE(cfwl." + vp.DatabaseColumnName + Include.cLockExtension + ", 0) " + vp.DatabaseColumnName + Include.cLockExtension;
                    if (count < aDatabaseVariables.Count)
                    {
                        line += ", ";
                    }
                    aWriter.WriteLine(line);
                    line = "  ";

                }

                aWriter.WriteLine(" FROM " + tempTableName + " t");
                aWriter.WriteLine(" JOIN " + aLockTableName + " cfwl (NOLOCK)");
                aWriter.WriteLine("  on cfwl.HN_RID = t.HN_RID");
                aWriter.WriteLine("  and cfwl.FV_RID = t.FV_RID");
                aWriter.WriteLine("  and cfwl.TIME_ID = t.TIME_ID");
                aWriter.WriteLine(" left outer join " + aForecastViewName + " cfw (NOLOCK)");
                aWriter.WriteLine("  on cfw.HN_RID = cfwl.HN_RID");
                aWriter.WriteLine("  and cfw.FV_RID =cfwl.FV_RID");
                aWriter.WriteLine("  and cfw.TIME_ID = cfwl.TIME_ID");

                aWriter.WriteLine(" ");
                aWriter.WriteLine(" -- create temp table for history values");
                aWriter.WriteLine(" create table " + aTempTableName + "HISTORY ");
                aWriter.WriteLine(" (HN_RID  int not null,");
                aWriter.WriteLine("  FV_RID  int not null,");
                aWriter.WriteLine("  TIME_ID int not null,");
                count = 0;
                // add variables
                line = "  ";
                foreach (VariableProfile vp in aDatabaseVariables)
                {
                    ++count;
                    line += "  " + vp.DatabaseColumnName;
                    switch (vp.ChainDatabaseVariableType)
                    {
                        case eVariableDatabaseType.Integer:
                            line += " int ";
                            break;
                        case eVariableDatabaseType.Real:
                            line += " real ";
                            break;
                        case eVariableDatabaseType.DateTime:
                            line += " datetime ";
                            break;
                        case eVariableDatabaseType.String:
                            line += " varchar(100) ";
                            break;
                        case eVariableDatabaseType.Char:
                            line += " char(1) ";
                            break;
                        case eVariableDatabaseType.Float:
                            line += " float ";
                            break;
                        case eVariableDatabaseType.BigInteger:
                            line += " bigint ";
                            break;
                    }
                    if (count < aDatabaseVariables.Count)
                    {
                        line += "null, ";
                    }
                    else
                    {
                        line += "null)";
                    }
                    aWriter.WriteLine(line);
                    line = "  ";
                }
                aWriter.WriteLine(" ");

                aWriter.WriteLine(" -- alternate and real time roll");
                aWriter.WriteLine("IF @HN_TYPE = 800001 and @ROLL_OPTION = 800251");
                aWriter.WriteLine("   BEGIN");
                aWriter.WriteLine("   SET @LoopCount = 0");
                aWriter.WriteLine("   CREATE TABLE #TREE (LOOPCOUNT INT NOT NULL, PARENT_HN_RID INT NOT NULL, HOME_PH_RID INT, PH_TYPE INT, CHILD_HN_RID INT NOT NULL, CHILD_HN_MOD INT NOT NULL)");
                aWriter.WriteLine("   -- insert the children of the node into the temp table");
                aWriter.WriteLine("   INSERT #TREE (LOOPCOUNT, PARENT_HN_RID, HOME_PH_RID, PH_TYPE, CHILD_HN_RID, CHILD_HN_MOD) ");
                aWriter.WriteLine("       select @LoopCount as LOOPCOUNT, @HN_RID as PARENT_HN_RID, hn.HOME_PH_RID, ph.PH_TYPE, hnj.HN_RID, hnj.HN_RID % @Tables");
                aWriter.WriteLine("         from HIER_NODE_JOIN hnj (NOLOCK)");
                aWriter.WriteLine("           JOIN HIERARCHY_NODE hn (NOLOCK) ON hn.HN_RID = hnj.HN_RID");
                aWriter.WriteLine("           JOIN PRODUCT_HIERARCHY ph (NOLOCK) ON ph.PH_RID = hn.HOME_PH_RID");
                aWriter.WriteLine("       where @HN_RID = hnj.PARENT_HN_RID");
                aWriter.WriteLine("   SET @Loop = @@ROWCOUNT");
                aWriter.WriteLine("   -- chase all paths until you get the main hierarchy (type 800000) or the lowest leaf");
                aWriter.WriteLine("   SET @NextLoopCount = @LoopCount + 1");
                aWriter.WriteLine("   WHILE @Loop > 0");
                aWriter.WriteLine("   BEGIN");
                aWriter.WriteLine("      INSERT #TREE");
                aWriter.WriteLine("        select @NextLoopCount as LOOPCOUNT, hnj.PARENT_HN_RID, hn.HOME_PH_RID, ph.PH_TYPE, hnj.HN_RID, hnj.HN_RID % @Tables");
                aWriter.WriteLine("          from HIER_NODE_JOIN hnj (NOLOCK)");
                aWriter.WriteLine("            JOIN HIERARCHY_NODE hn (NOLOCK) ON hn.HN_RID = hnj.HN_RID");
                aWriter.WriteLine("            JOIN PRODUCT_HIERARCHY ph (NOLOCK) ON ph.PH_RID = hn.HOME_PH_RID");
                aWriter.WriteLine("            JOIN #TREE t ON hnj.PARENT_HN_RID = t.CHILD_HN_RID");
                aWriter.WriteLine("        WHERE t.LOOPCOUNT =  @LoopCount AND t.PH_TYPE <> 800000");
                aWriter.WriteLine("      SET @Loop = @@ROWCOUNT");
                aWriter.WriteLine("      SET @LoopCount = @LoopCount + 1");
                aWriter.WriteLine("      SET @NextLoopCount = @LoopCount + 1");
                aWriter.WriteLine("   END");
                aWriter.WriteLine("   -- join with dates from xml selecting only nodes from the main hierarchy or lowest leaf alternates");
                aWriter.WriteLine("   SELECT * ");
                aWriter.WriteLine("     INTO " + aTempTableName + "2");
                aWriter.WriteLine("     FROM #TREE");
                aWriter.WriteLine("     CROSS JOIN " + tempTableName);
                aWriter.WriteLine("   where LOOPCOUNT = @LoopCount - 1");
                aWriter.WriteLine("	     or PH_TYPE = 800000");

                count = 0;
                aWriter.WriteLine("       -- build temp table of summed history values ");
                aWriter.WriteLine("       insert into " + aTempTableName + "HISTORY ");
                aWriter.WriteLine("       select @HN_RID as HN_RID, @FV_RID as FV_RID, chw.TIME_ID, ");
                // add variables
                line = _indent10;
                int variablesCount = 0;
                foreach (VariableProfile vp in aDatabaseVariables)
                {
                    if (vp.ChainDatabaseVariableType == eVariableDatabaseType.Integer ||
                        vp.ChainDatabaseVariableType == eVariableDatabaseType.Real ||
                        vp.ChainDatabaseVariableType == eVariableDatabaseType.Float ||
                        vp.ChainDatabaseVariableType == eVariableDatabaseType.BigInteger)
                    {
                        ++variablesCount;
                    }

                }
                foreach (VariableProfile vp in aDatabaseVariables)
                {
                    if (vp.ChainDatabaseVariableType == eVariableDatabaseType.Integer ||
                        vp.ChainDatabaseVariableType == eVariableDatabaseType.Real ||
                        vp.ChainDatabaseVariableType == eVariableDatabaseType.Float ||
                        vp.ChainDatabaseVariableType == eVariableDatabaseType.BigInteger)
                    {
                        ++count;
                        line += "SUM(chw." + vp.DatabaseColumnName + ") " + vp.DatabaseColumnName;
                        if (count < variablesCount)
                        {
                            line += ", ";
                        }
                        aWriter.WriteLine(line);
                        line = _indent10;
                    }
                }
                aWriter.WriteLine("		FROM " + aTempTableName + "2 t");
                aWriter.WriteLine("	    JOIN " + aHistoryViewName + " chw (NOLOCK) ON t.CHILD_HN_RID = chw.HN_RID");
                aWriter.WriteLine("		    AND chw.TIME_ID = t.TIME_ID");
                aWriter.WriteLine("	    GROUP BY chw.TIME_ID");
                aWriter.WriteLine(" END ");
                aWriter.WriteLine(" ELSE ");
                aWriter.WriteLine(" BEGIN ");
                aWriter.WriteLine("       -- build temp table of history values ");
                aWriter.WriteLine("       insert into " + aTempTableName + "HISTORY ");
                aWriter.WriteLine("       select chw.HN_RID,  t.FV_RID as FV_RID, chw.TIME_ID, ");
                // add variables
                count = 0;
                line = _indent10;
                foreach (VariableProfile vp in aDatabaseVariables)
                {
                    ++count;
                    line += "chw." + vp.DatabaseColumnName + " as " + vp.DatabaseColumnName;
                    if (count < aDatabaseVariables.Count)
                    {
                        line += ", ";
                    }
                    aWriter.WriteLine(line);
                    line = _indent10;

                }
                aWriter.WriteLine("  FROM " + tempTableName + " t");
                aWriter.WriteLine("  JOIN " + aHistoryViewName + " chw (NOLOCK)");
                aWriter.WriteLine("    on chw.HN_RID = t.HN_RID");
                aWriter.WriteLine("   and chw.TIME_ID = t.TIME_ID");
                aWriter.WriteLine(" END ");
                aWriter.WriteLine(" ");

                aWriter.WriteLine("  --select * from " + aTempTableName + "MOD");
                aWriter.WriteLine("  --select * from " + aTempTableName + "HISTORY");

                aWriter.WriteLine("   -- combine modified values with history");
                aWriter.WriteLine(" select tmpmod.HN_RID, tmpmod.FV_RID, tmpmod.TIME_ID,");
                foreach (VariableProfile vp in aDatabaseVariables)
                {
                    line += "COALESCE(COALESCE(tmpmod." + vp.DatabaseColumnName + ", tmphis." + vp.DatabaseColumnName + "), 0) " + vp.DatabaseColumnName + ", ";
                    aWriter.WriteLine(line);
                    line = _indent10;

                }
                count = 0;
                foreach (VariableProfile vp in aDatabaseVariables)
                {
                    ++count;
                    line += "COALESCE(tmpmod." + vp.DatabaseColumnName + Include.cLockExtension + ", 0) " + vp.DatabaseColumnName + Include.cLockExtension;
                    if (count < aDatabaseVariables.Count)
                    {
                        line += ", ";
                    }
                    aWriter.WriteLine(line);
                    line = _indent10;

                }
                aWriter.WriteLine("    into  " + aTempTableName + "MOD2");
                aWriter.WriteLine(" FROM " + aTempTableName + "MOD tmpmod");
                aWriter.WriteLine(" left outer JOIN " + aTempTableName + "HISTORY tmphis  ON tmphis.HN_RID = tmpmod.HN_RID");
                aWriter.WriteLine(" AND tmphis.FV_RID = tmpmod.FV_RID");
                aWriter.WriteLine(" AND tmphis.TIME_ID = tmpmod.TIME_ID");

                aWriter.WriteLine(" ");
                aWriter.WriteLine(" -- remove duplicate rows from history table");
                aWriter.WriteLine("  delete " + aTempTableName + "HISTORY ");
                aWriter.WriteLine("    from " + aTempTableName + "HISTORY, " + aTempTableName + "MOD");
                aWriter.WriteLine("    where " + aTempTableName + "HISTORY.HN_RID = " + aTempTableName + "MOD.HN_RID");
                aWriter.WriteLine("         and " + aTempTableName + "HISTORY.FV_RID = " + aTempTableName + "MOD.FV_RID");
                aWriter.WriteLine("         and " + aTempTableName + "HISTORY.TIME_ID = " + aTempTableName + "MOD.TIME_ID");
                aWriter.WriteLine(" ");

                aWriter.WriteLine(" --select * from " + aTempTableName + "HISTORY");

                aWriter.WriteLine(" insert into " + aTempTableName + "MOD2");
                aWriter.WriteLine(" select tmphis.HN_RID, tmphis.FV_RID, tmphis.TIME_ID,");
                foreach (VariableProfile vp in aDatabaseVariables)
                {
                    line += "COALESCE(tmphis." + vp.DatabaseColumnName + ", 0) " + vp.DatabaseColumnName + ", ";
                    aWriter.WriteLine(line);
                    line = _indent10;
                }
                count = 0;
                foreach (VariableProfile vp in aDatabaseVariables)
                {
                    ++count;
                    line += "0 as " + vp.DatabaseColumnName + Include.cLockExtension;
                    if (count < aDatabaseVariables.Count)
                    {
                        line += ", ";
                    }
                    aWriter.WriteLine(line);
                    line = _indent10;
                }

                aWriter.WriteLine(" from " + aTempTableName + "HISTORY tmphis");
                aWriter.WriteLine(" ");
                aWriter.WriteLine(" select * from " + aTempTableName + "MOD2 order by TIME_ID");
                aWriter.WriteLine(" ");

                aWriter.WriteLine(" OPTION (MAXDOP 1)");

                aWriter.WriteLine(System.Environment.NewLine);
                //aWriter.WriteLine("if (select object_id('tempdb.dbo." + aTempTableName + "')) > 0 DROP TABLE " + aTempTableName);
                aWriter.WriteLine("if (select object_id('tempdb.dbo." + aTempTableName + "2')) > 0 DROP TABLE " + aTempTableName + "2");
                aWriter.WriteLine("if (select object_id('tempdb.dbo." + aTempTableName + "MOD')) > 0 DROP TABLE " + aTempTableName + "MOD");
                aWriter.WriteLine("if (select object_id('tempdb.dbo." + aTempTableName + "MOD2')) > 0 DROP TABLE " + aTempTableName + "MOD2");
                aWriter.WriteLine("if (select object_id('tempdb.dbo." + aTempTableName + "HISTORY')) > 0 DROP TABLE " + aTempTableName + "HISTORY");
                aWriter.WriteLine(System.Environment.NewLine);

                aWriter.WriteLine(System.Environment.NewLine);
                aWriter.WriteLine("GO");
                aWriter.WriteLine(System.Environment.NewLine);
            }
            catch
            {
                throw;
            }
        }
        private static void Generate_MID_CHN_FOR_WK_DEL_ZERO(genOptions options)
        {
            string line;
            int count;

            try
            {
                string aProcedureName = options.name;
                string aTableName = options.table;
                //string aLockTableName, 
                ProfileList aDatabaseVariables = variables.GetChainWeeklyForecastDatabaseVariableList();
                StreamWriter aWriter = options.writer;

                //AddStoredProcedureDrop("SP_MID_CHN_FOR_WK{#}_DEL_ZERO", aWriter);

                AddStoredProcedureDrop(aProcedureName, aWriter);

                if (aDatabaseVariables == null ||
                    aDatabaseVariables.Count == 0)
                {
                    return;
                }

                aWriter.WriteLine("CREATE PROCEDURE [dbo].[" + aProcedureName + "]");
                aWriter.WriteLine("@COMMIT_LIMIT INT,");
                aWriter.WriteLine("@RECORDS_DELETED int output");
                aWriter.WriteLine("AS");
                aWriter.WriteLine(System.Environment.NewLine);

                aWriter.WriteLine("DELETE top (@COMMIT_LIMIT) from " + aTableName);

                line = _indent10 + "WHERE ";
                count = 0;
                foreach (VariableProfile vp in aDatabaseVariables)
                {
                    ++count;
                    line += "COALESCE(" + vp.DatabaseColumnName + ", 0) = 0";
                    if (count < aDatabaseVariables.Count)
                    {
                        line += " AND ";
                    }
                    if (line.Length > 90)
                    {
                        aWriter.WriteLine(line);
                        line = _indent10;
                    }
                }

                if (line.Length > 0)
                {
                    aWriter.WriteLine(line);
                }

                aWriter.WriteLine("set @RECORDS_DELETED = @@rowcount");
                aWriter.WriteLine(System.Environment.NewLine);

                aWriter.WriteLine(System.Environment.NewLine);
                aWriter.WriteLine("GO");
                aWriter.WriteLine(System.Environment.NewLine);
            }
            catch
            {
                throw;
            }
        }
        private static void Generate_MID_CHN_FOR_WK_DEL_UNLOCK(genOptions options)
        {
            string line;
            int count;

            try
            {
                string aProcedureName = options.name;
                string aTableName = Include.DBChainWeeklyLockTable;
                ProfileList aDatabaseVariables = variables.GetChainWeeklyForecastDatabaseVariableList();
                StreamWriter aWriter = options.writer;

                AddStoredProcedureDrop(aProcedureName, aWriter);

                if (aDatabaseVariables == null ||
                    aDatabaseVariables.Count == 0)
                {
                    return;
                }

                aWriter.WriteLine("CREATE PROCEDURE [dbo].[" + aProcedureName + "]");
                aWriter.WriteLine("@COMMIT_LIMIT INT,");
                aWriter.WriteLine("@RECORDS_DELETED int output");
                aWriter.WriteLine("AS");
                aWriter.WriteLine(System.Environment.NewLine);

                aWriter.WriteLine("DELETE TOP (@COMMIT_LIMIT) " + aTableName);

                line = _indent10 + "WHERE ";
                count = 0;
                foreach (VariableProfile vp in aDatabaseVariables)
                {
                    ++count;
                    line += "COALESCE(" + vp.DatabaseColumnName + Include.cLockExtension + ", 0) = 0";
                    if (count < aDatabaseVariables.Count)
                    {
                        line += " AND ";
                    }
                    if (line.Length > 90)
                    {
                        aWriter.WriteLine(line);
                        line = _indent10;
                    }
                }

                if (line.Length > 0)
                {
                    aWriter.WriteLine(line);
                }

                aWriter.WriteLine("set @RECORDS_DELETED = @@rowcount");

                aWriter.WriteLine(System.Environment.NewLine);
                aWriter.WriteLine("GO");
                aWriter.WriteLine(System.Environment.NewLine);
            }
            catch
            {
                throw;
            }
        }
        private static void Generate_MID_CHN_FOR_WK_ROLLUP(genOptions options)
        {
            ChainForecastWeekRollupProcess rollupProcess;
            string procedureName;
            bool includeZeroInAverage = true;
            bool honorLocks = false;
            bool zeroParentsWithNoChildren = true;

            try
            {
                ProfileList chainWeeklyForecastRollupVariables = new ProfileList(eProfileType.Variable);
                
                foreach (VariableProfile vp in variables.GetChainWeeklyForecastDatabaseVariableList())
                {
                    if (vp.LevelRollType != eLevelRollType.None &&
                        vp.ChainForecastModelType != eVariableDatabaseModelType.None &&
                        (vp.VariableCategory == eVariableCategory.Chain || vp.VariableCategory == eVariableCategory.Both))
                    {
                        chainWeeklyForecastRollupVariables.Add(vp);
                    }
                }

                ProfileList aDatabaseVariables = chainWeeklyForecastRollupVariables;
                
                StreamWriter aWriter = options.writer;

                procedureName = Include.DBChainWeeklyForecastRollupSP;
                AddStoredProcedureDrop(procedureName, aWriter);
                rollupProcess = new ChainForecastWeekRollupProcess(null, null, aDatabaseVariables.ArrayList, 0, 0, 0, 0, 0, 0, includeZeroInAverage, honorLocks, zeroParentsWithNoChildren, true, null);
                aWriter.Write(rollupProcess.BuildStoredProcedure(procedureName, honorLocks));
                aWriter.WriteLine(System.Environment.NewLine);
                aWriter.WriteLine("GO");
                aWriter.WriteLine(System.Environment.NewLine);

                zeroParentsWithNoChildren = false;
                procedureName = Include.DBChainWeeklyForecastNoZeroRollupSP;
                AddStoredProcedureDrop(procedureName, aWriter);
                rollupProcess = new ChainForecastWeekRollupProcess(null, null, aDatabaseVariables.ArrayList, 0, 0, 0, 0, 0, 0, includeZeroInAverage, honorLocks, zeroParentsWithNoChildren, true, null);
                aWriter.Write(rollupProcess.BuildStoredProcedure(procedureName, honorLocks));
                aWriter.WriteLine(System.Environment.NewLine);
                aWriter.WriteLine("GO");
                aWriter.WriteLine(System.Environment.NewLine);

                zeroParentsWithNoChildren = true;
                honorLocks = true;
                procedureName = Include.DBChainWeeklyForecastHonorLocksRollupSP;
                AddStoredProcedureDrop(procedureName, aWriter);
                rollupProcess = new ChainForecastWeekRollupProcess(null, null, aDatabaseVariables.ArrayList, 0, 0, 0, 0, 0, 0, includeZeroInAverage, honorLocks, zeroParentsWithNoChildren, true, null);
                aWriter.Write(rollupProcess.BuildStoredProcedure(procedureName, honorLocks));
                aWriter.WriteLine(System.Environment.NewLine);
                aWriter.WriteLine("GO");
                aWriter.WriteLine(System.Environment.NewLine);
            }
            catch
            {
                throw;
            }
        }
        private static void Generate_MID_HIS_ST_OTHER_ROLLUPS(genOptions options)
        {
            StoreToChainHistoryRollupProcess rollupProcess;
            StoreIntransitRollupProcess intransitRollupProcess;
            StoreExternalIntransitRollupProcess extIntransitRollupProcess;
            ArrayList databaseVariables;
            string procedureName;
            bool includeZeroInAverage = true;
            bool honorLocks = false;
            bool zeroParentsWithNoChildren = true;

            try
            {
                ProfileList storeWeeklyHistoryRollupVariables = new ProfileList(eProfileType.Variable);
                foreach (VariableProfile vp in variables.GetStoreWeeklyHistoryDatabaseVariableList())
                {
                    if (vp.LevelRollType != eLevelRollType.None &&
                        vp.StoreWeeklyHistoryModelType != eVariableDatabaseModelType.None &&
                        (vp.VariableCategory == eVariableCategory.Store || vp.VariableCategory == eVariableCategory.Both))
                    {
                        storeWeeklyHistoryRollupVariables.Add(vp);
                    }
                }

                ProfileList chainWeeklyHistoryRollupVariables = new ProfileList(eProfileType.Variable);
                foreach (VariableProfile vp in variables.GetChainWeeklyHistoryDatabaseVariableList())
                {
                    // Begin TT#3158 - JSmith - Planning History Rollup
                    //if (vp.LevelRollType != eLevelRollType.None &&
                    //                    vp.ChainHistoryModelType != eVariableDatabaseModelType.None)
                    if (vp.LevelRollType != eLevelRollType.None &&
                                        vp.ChainHistoryModelType != eVariableDatabaseModelType.None &&
                        (vp.VariableCategory == eVariableCategory.Chain || vp.VariableCategory == eVariableCategory.Both))
                    // End TT#3158 - JSmith - Planning History Rollup
                    {
                        chainWeeklyHistoryRollupVariables.Add(vp);
                    }
                }
                ProfileList aStoreDatabaseVariables = storeWeeklyHistoryRollupVariables;
                ProfileList aChainDatabaseVariables = chainWeeklyHistoryRollupVariables;
                StreamWriter aWriter = options.writer;

                MIDRetail.Business.Rollup _rollup = new Rollup(null, 0, 0, false, false, false);

                procedureName = Include.DBStoreToChainHistoryRollupSP;
                databaseVariables = new ArrayList();
                _rollup.BuildIntersectionOfVariableLists(aStoreDatabaseVariables.ArrayList, aChainDatabaseVariables.ArrayList, databaseVariables, eRollType.storeToChain);
                AddStoredProcedureDrop(procedureName, aWriter);
                rollupProcess = new StoreToChainHistoryRollupProcess(null, null, databaseVariables, 0, 0, 0, 0, 0, 0, includeZeroInAverage, honorLocks, zeroParentsWithNoChildren, true, null);
                aWriter.Write(rollupProcess.BuildStoredProcedure(procedureName));
                aWriter.WriteLine(System.Environment.NewLine);
                aWriter.WriteLine("GO");
                aWriter.WriteLine(System.Environment.NewLine);

                zeroParentsWithNoChildren = false;
                procedureName = Include.DBStoreToChainHistoryNoZeroRollupSP;
                AddStoredProcedureDrop(procedureName, aWriter);
                rollupProcess = new StoreToChainHistoryRollupProcess(null, null, databaseVariables, 0, 0, 0, 0, 0, 0, includeZeroInAverage, honorLocks, zeroParentsWithNoChildren, true, null);
                aWriter.Write(rollupProcess.BuildStoredProcedure(procedureName));
                aWriter.WriteLine(System.Environment.NewLine);
                aWriter.WriteLine("GO");
                aWriter.WriteLine(System.Environment.NewLine);

                zeroParentsWithNoChildren = true;
                honorLocks = false;
                procedureName = Include.DBStoreIntransitRollupSP;
                AddStoredProcedureDrop(procedureName, aWriter);
                intransitRollupProcess = new StoreIntransitRollupProcess(null, null, databaseVariables, 0, 0, 0, 0, 0, 0, includeZeroInAverage, honorLocks, zeroParentsWithNoChildren, true, null);
                aWriter.Write(intransitRollupProcess.BuildStoredProcedure(procedureName));
                aWriter.WriteLine(System.Environment.NewLine);
                aWriter.WriteLine("GO");
                aWriter.WriteLine(System.Environment.NewLine);

                procedureName = Include.DBStoreExternalIntransitRollupSP;
                AddStoredProcedureDrop(procedureName, aWriter);
                extIntransitRollupProcess = new StoreExternalIntransitRollupProcess(null, null, databaseVariables, 0, 0, 0, 0, 0, 0, includeZeroInAverage, honorLocks, zeroParentsWithNoChildren, true, null);
                aWriter.Write(extIntransitRollupProcess.BuildStoredProcedure(procedureName));
                aWriter.WriteLine(System.Environment.NewLine);
                aWriter.WriteLine("GO");
                aWriter.WriteLine(System.Environment.NewLine);
            }
            catch
            {
                throw;
            }
        }
        private static void Generate_MID_FOR_ST_TO_CHN_ROLLUP(genOptions options)
        {
            StoreToChainForecastRollupProcess rollupProcess;
            ArrayList databaseVariables;
            string procedureName;
            bool includeZeroInAverage = true;
            bool honorLocks = false;
            bool zeroParentsWithNoChildren = true;

            try
            {
                ProfileList storeWeeklyForecastRollupVariables = new ProfileList(eProfileType.Variable);
                foreach (VariableProfile vp in variables.GetStoreWeeklyForecastDatabaseVariableList())
                {
                    if (vp.LevelRollType != eLevelRollType.None &&
                        vp.StoreForecastModelType != eVariableDatabaseModelType.None &&
                        (vp.VariableCategory == eVariableCategory.Store || vp.VariableCategory == eVariableCategory.Both))
                    {
                        storeWeeklyForecastRollupVariables.Add(vp);
                    }
                }

                ProfileList chainWeeklyForecastRollupVariables = new ProfileList(eProfileType.Variable);
                foreach (VariableProfile vp in variables.GetChainWeeklyForecastDatabaseVariableList())
                {
                    if (vp.LevelRollType != eLevelRollType.None &&
                        vp.ChainForecastModelType != eVariableDatabaseModelType.None &&
                        (vp.VariableCategory == eVariableCategory.Chain || vp.VariableCategory == eVariableCategory.Both))
                    {
                        chainWeeklyForecastRollupVariables.Add(vp);
                    }
                }

                ProfileList aStoreDatabaseVariables = storeWeeklyForecastRollupVariables;
                ProfileList aChainDatabaseVariables = chainWeeklyForecastRollupVariables;
                StreamWriter aWriter = options.writer;

                MIDRetail.Business.Rollup _rollup = new Rollup(null, 0, 0, false, false, false);

                procedureName = Include.DBStoreToChainForecastRollupSP;
                databaseVariables = new ArrayList();
                _rollup.BuildIntersectionOfVariableLists(aStoreDatabaseVariables.ArrayList, aChainDatabaseVariables.ArrayList, databaseVariables, eRollType.storeToChain);
                AddStoredProcedureDrop(procedureName, aWriter);
                rollupProcess = new StoreToChainForecastRollupProcess(null, null, databaseVariables, 0, 0, 0, 0, 0, 0, includeZeroInAverage, honorLocks, zeroParentsWithNoChildren, true, null);
                aWriter.Write(rollupProcess.BuildStoredProcedure(procedureName));
                aWriter.WriteLine(System.Environment.NewLine);
                aWriter.WriteLine("GO");
                aWriter.WriteLine(System.Environment.NewLine);

                honorLocks = true;
                procedureName = Include.DBStoreToChainForecastHonorLocksRollupSP;
                databaseVariables = new ArrayList();
                _rollup.BuildIntersectionOfVariableLists(aStoreDatabaseVariables.ArrayList, aChainDatabaseVariables.ArrayList, databaseVariables, eRollType.storeToChain);
                AddStoredProcedureDrop(procedureName, aWriter);
                rollupProcess = new StoreToChainForecastRollupProcess(null, null, databaseVariables, 0, 0, 0, 0, 0, 0, includeZeroInAverage, honorLocks, zeroParentsWithNoChildren, true, null);
                aWriter.Write(rollupProcess.BuildStoredProcedure(procedureName));
                aWriter.WriteLine(System.Environment.NewLine);
                aWriter.WriteLine("GO");
                aWriter.WriteLine(System.Environment.NewLine);

                zeroParentsWithNoChildren = false;
                honorLocks = false;
                procedureName = Include.DBStoreToChainForecastNoZeroRollupSP;
                AddStoredProcedureDrop(procedureName, aWriter);
                rollupProcess = new StoreToChainForecastRollupProcess(null, null, databaseVariables, 0, 0, 0, 0, 0, 0, includeZeroInAverage, honorLocks, zeroParentsWithNoChildren, true, null);
                aWriter.Write(rollupProcess.BuildStoredProcedure(procedureName));
                aWriter.WriteLine(System.Environment.NewLine);
                aWriter.WriteLine("GO");
                aWriter.WriteLine(System.Environment.NewLine);
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region "Older Code"
        //private const int cCutoffReleaseMajor = 2;
        //private const int cCutoffReleaseMinor = 1;
        //private const int cCutoffReleaseRevision = 5;

        // Begin MID Track #5151 - JSmith - Timeout during upgrade
        //private static int _commandTimeout = 30;
        // End MID Track #5151
        //private static string _connectionString;

        //static private frmDatabaseUpdate _frame;


        //static private int _processCount;
            //public enum FileProcessType
    //{
    //    VersioningUpdates = 0,
    //    ReleaseUpdates = 1,
    //    //Begin TT#808 - JScott - Create separate upgrade script for Stored Procedure
    //    //NewInstall = 2
    //    NewInstall = 2,
    //    ProcedureReload = 3,
    //    //End TT#808 - JScott - Create separate upgrade script for Stored Procedure
    //}

        //static public bool ProcessScripts(string aFileName, string aFileKey, Queue aMessageQueue,
        //    Queue aProcessedQueue, bool aUpgradeProcess, bool aProcedureReloadProcess,
        //    eDatabaseType aDatabaseType,
        //    string aConnString,
        //    bool aIgnoreErrors, int aNoDataTables, string aAllocationFileGroup,
        //    string aForecastFileGroup, string aHistoryFileGroup, int aNoHistoryFileGroup,
        //    string aDailyHistoryFileGroup, int aNoDailyHistoryFileGroup, string aAuditFileGroup,
        //    string aWeekArchiveFileGroup, string aDayArchiveFileGroup)
        //{
        //    string connectionString = aConnString;

        //    return ProcessScripts(aFileName, aFileKey, aMessageQueue,
        //        aProcessedQueue, aUpgradeProcess, aProcedureReloadProcess, connectionString, aDatabaseType,
        //        aIgnoreErrors, aNoDataTables, aAllocationFileGroup,
        //        aForecastFileGroup, aHistoryFileGroup, aNoHistoryFileGroup,
        //        aDailyHistoryFileGroup, aNoDailyHistoryFileGroup, aAuditFileGroup,
        //        aWeekArchiveFileGroup, aDayArchiveFileGroup);
        //}


        //static public bool ProcessScripts(string aFileName, string aFileKey, Queue aMessageQueue,
        //    Queue aProcessedQueue, bool aUpgradeProcess, bool aProcedureReloadProcess, string aConnectionString, eDatabaseType aDatabaseType,
        //    bool aIgnoreErrors, int aNoDataTables, string aAllocationFileGroup,
        //    string aForecastFileGroup, string aHistoryFileGroup, int aNoHistoryFileGroup,
        //    string aDailyHistoryFileGroup, int aNoDailyHistoryFileGroup, string aAuditFileGroup,
        //    string aWeekArchiveFileGroup, string aDayArchiveFileGroup)
        //{
        //    bool connectionOpen = false;
        //    //DataTable dt;
        //    //SqlDataAdapter sda;
        //    //DataRow dr;
        //    SqlCommand sqlCommand = null;
        //    int majorVersion = 0;
        //    int minorVersion = 0;
        //    int revision = 0;
        //    int modification = 0;
        //    DateTime dbDate = DateTime.MinValue;
        //    _processCount = 0;

        //    try
        //    {
        //        // open connection

        //        try
        //        {
        //            _connectionString = aConnectionString;
        //            sqlCommand = new SqlCommand();
        //            sqlCommand.Connection = new SqlConnection(_connectionString);
        //            sqlCommand.Connection.Open();
        //            if (_commandTimeout != sqlCommand.CommandTimeout)
        //            {
        //                sqlCommand.CommandTimeout = _commandTimeout;
        //            }
        //            connectionOpen = true;
        //        }
        //        catch (Exception ex)
        //        {
        //            string message = ex.ToString();
        //            aMessageQueue.Enqueue("FATAL DB Error: Error encountered during open of database");
        //            throw;
        //        }
        //        ProgressBarSetMinimum(0);

        //        if (aUpgradeProcess)
        //        {
        //            // process upgrade file, executing only version updates

        //            ProcessUpgradeFile(
        //                FileProcessType.VersioningUpdates,
        //                sqlCommand,
        //                aFileName,
        //                aFileKey,
        //                aMessageQueue,
        //                aProcessedQueue,
        //                aUpgradeProcess,
        //                aIgnoreErrors,
        //                majorVersion,
        //                minorVersion,
        //                revision,
        //                modification,
        //                dbDate,
        //                aDatabaseType,
        //                aNoDataTables,
        //                aAllocationFileGroup,
        //                aForecastFileGroup,
        //                aHistoryFileGroup,
        //                aNoHistoryFileGroup,
        //                aDailyHistoryFileGroup,
        //                aNoDailyHistoryFileGroup,
        //                aAuditFileGroup,
        //                aWeekArchiveFileGroup,
        //                aDayArchiveFileGroup,
        //                false);

        //            // get current version

        //            try
        //            {
        //                string currentVersion = GetCurrentVersion(sqlCommand);
        //                aProcessedQueue.Enqueue(currentVersion);
        //            }
        //            catch (Exception ex)
        //            {
        //                string message = ex.ToString();
        //                aMessageQueue.Enqueue("FATAL DB Error: Error reading current version information");
        //                throw;
        //            }

        //            // process upgrade file, executing release updates

        //            ProcessUpgradeFile(
        //                FileProcessType.ReleaseUpdates,
        //                sqlCommand,
        //                aFileName,
        //                aFileKey,
        //                aMessageQueue,
        //                aProcessedQueue,
        //                aUpgradeProcess,
        //                aIgnoreErrors,
        //                majorVersion,
        //                minorVersion,
        //                revision,
        //                modification,
        //                dbDate,
        //                aDatabaseType,
        //                aNoDataTables,
        //                aAllocationFileGroup,
        //                aForecastFileGroup,
        //                aHistoryFileGroup,
        //                aNoHistoryFileGroup,
        //                aDailyHistoryFileGroup,
        //                aNoDailyHistoryFileGroup,
        //                aAuditFileGroup,
        //                aWeekArchiveFileGroup,
        //                aDayArchiveFileGroup,
        //                true);
        //            ProgressBarSetMaximum(_processCount);

        //            ProgressBarSetValue(0);
        //            ProcessUpgradeFile(
        //                FileProcessType.ReleaseUpdates,
        //                sqlCommand,
        //                aFileName,
        //                aFileKey,
        //                aMessageQueue,
        //                aProcessedQueue,
        //                aUpgradeProcess,
        //                aIgnoreErrors,
        //                majorVersion,
        //                minorVersion,
        //                revision,
        //                modification,
        //                dbDate,
        //                aDatabaseType,
        //                aNoDataTables,
        //                aAllocationFileGroup,
        //                aForecastFileGroup,
        //                aHistoryFileGroup,
        //                aNoHistoryFileGroup,
        //                aDailyHistoryFileGroup,
        //                aNoDailyHistoryFileGroup,
        //                aAuditFileGroup,
        //                aWeekArchiveFileGroup,
        //                aDayArchiveFileGroup,
        //                false);

        //            if (_processCount == 0)
        //            {
        //                ProgressBarSetMaximum(1);
        //            }
        //            ProgressBarSetToMaximum();
        //        }
        //        else if (aProcedureReloadProcess)
        //        {
        //            ProcessUpgradeFile(
        //                FileProcessType.ProcedureReload,
        //                sqlCommand,
        //                aFileName,
        //                aFileKey,
        //                aMessageQueue,
        //                aProcessedQueue,
        //                aUpgradeProcess,
        //                aIgnoreErrors,
        //                majorVersion,
        //                minorVersion,
        //                revision,
        //                modification,
        //                dbDate,
        //                aDatabaseType,
        //                aNoDataTables,
        //                aAllocationFileGroup,
        //                aForecastFileGroup,
        //                aHistoryFileGroup,
        //                aNoHistoryFileGroup,
        //                aDailyHistoryFileGroup,
        //                aNoDailyHistoryFileGroup,
        //                aAuditFileGroup,
        //                aWeekArchiveFileGroup,
        //                aDayArchiveFileGroup,
        //                false);
        //        }
        //        else
        //        {
        //            // process new install

        //            // get database version information when setting modification number
        //            if (aFileKey == "UpgradeFile")
        //            {
        //                try
        //                {
        //                    majorVersion = cCutoffReleaseMajor;
        //                    minorVersion = cCutoffReleaseMinor;
        //                    revision = cCutoffReleaseRevision;
        //                    modification = int.MaxValue;
        //                }
        //                catch (Exception ex)
        //                {
        //                    string message = ex.ToString();
        //                    aMessageQueue.Enqueue("FATAL DB Error: Error reading current version information");
        //                    throw;
        //                }
        //            }

        //            ProcessUpgradeFile(
        //                FileProcessType.NewInstall,
        //                sqlCommand,
        //                aFileName,
        //                aFileKey,
        //                aMessageQueue,
        //                aProcessedQueue,
        //                aUpgradeProcess,
        //                aIgnoreErrors,
        //                majorVersion,
        //                minorVersion,
        //                revision,
        //                modification,
        //                dbDate,
        //                aDatabaseType,
        //                aNoDataTables,
        //                aAllocationFileGroup,
        //                aForecastFileGroup,
        //                aHistoryFileGroup,
        //                aNoHistoryFileGroup,
        //                aDailyHistoryFileGroup,
        //                aNoDailyHistoryFileGroup,
        //                aAuditFileGroup,
        //                aWeekArchiveFileGroup,
        //                aDayArchiveFileGroup,
        //                false);
        //            //_frame.ProgressBarSetToMaximum();
        //        }
        //    }

        //    catch (Exception ex)
        //    {
        //        aMessageQueue.Enqueue("UNEXPECTED EXCEPTION: " + ex.ToString());
        //        return false;
        //    }
        //    finally
        //    {
        //        if (connectionOpen)
        //        {
        //            sqlCommand.Connection.Close();
        //        }
        //    }

        //    if (aMessageQueue.Count > 0)
        //    {
        //        return false;
        //    }
        //    else
        //    {
        //        return true;
        //    }
        //}

        //        private static void DeleteSQLFunctionsScalar(SqlCommand aSqlCommand, Queue aMessageQueue)
//        {
//            try
//            {
//                string sPath;
//#if (DEBUG)
//                sPath = Directory.GetParent(Directory.GetParent(Directory.GetParent(Application.StartupPath).ToString().Trim()).ToString().Trim()) + @"\DatabaseDefinition\SQL_Functions_Scalar\";
//#else
//                sPath = Path.GetDirectoryName(Application.ExecutablePath) + @"\SQL_Functions_Scalar\";
//#endif
//                if (System.IO.Directory.Exists(sPath) == true)
//                {
//                    DirectoryInfo di = new DirectoryInfo(sPath);
//                    foreach (FileInfo fi in di.GetFiles("*.SQL"))
//                    {
//                        string functionName = fi.Name.Replace(".SQL", string.Empty);
//                        aSqlCommand.CommandText = "IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[" + functionName + "]') AND OBJECTPROPERTY(id, N'IsScalarFunction') = 1) ";
//                        aSqlCommand.CommandText += "DROP FUNCTION [dbo].[" + functionName + "] ";
//                        //aSqlCommand.CommandText += Environment.NewLine + "GO";

//                        aSqlCommand.ExecuteNonQuery();
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                aMessageQueue.Enqueue("UNEXPECTED EXCEPTION: " + ex.ToString());
//                throw;
//            }
//        }
//        private static void DeleteSQLFunctionsTable(SqlCommand aSqlCommand, Queue aMessageQueue)
//        {
//            try
//            {
//                string sPath;
//#if (DEBUG)
//                sPath = Directory.GetParent(Directory.GetParent(Directory.GetParent(Application.StartupPath).ToString().Trim()).ToString().Trim()) + @"\DatabaseDefinition\SQL_Functions_Table\";
//#else
//                sPath = Path.GetDirectoryName(Application.ExecutablePath) + @"\SQL_Functions_Table\";
//#endif
//                if (System.IO.Directory.Exists(sPath) == true)
//                {
//                    DirectoryInfo di = new DirectoryInfo(sPath);
//                    foreach (FileInfo fi in di.GetFiles("*.SQL"))
//                    {
//                        string functionName = fi.Name.Replace(".SQL", string.Empty);
//                        aSqlCommand.CommandText = "IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[" + functionName + "]') AND OBJECTPROPERTY(id, N'IsTableFunction') = 1) ";
//                        aSqlCommand.CommandText += "DROP FUNCTION [dbo].[" + functionName + "] ";
//                        //aSqlCommand.CommandText += Environment.NewLine + "GO";

//                        aSqlCommand.ExecuteNonQuery();
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                aMessageQueue.Enqueue("UNEXPECTED EXCEPTION: " + ex.ToString());
//                throw;
//            }
//        }
//        private static void DeleteSQLTypes(SqlCommand aSqlCommand, Queue aMessageQueue)
//        {
//            try
//            {
//                string sPath;
//#if (DEBUG)
//                sPath = Directory.GetParent(Directory.GetParent(Directory.GetParent(Application.StartupPath).ToString().Trim()).ToString().Trim()) + @"\DatabaseDefinition\SQL_Types\";
//#else
//                sPath = Path.GetDirectoryName(Application.ExecutablePath) + @"\SQL_Types\";
//#endif
//                if (System.IO.Directory.Exists(sPath) == true)
//                {
//                    DirectoryInfo di = new DirectoryInfo(sPath);
//                    foreach (FileInfo fi in di.GetFiles("*.SQL"))
//                    {
//                        string sqlTypeName = fi.Name.Replace(".SQL", string.Empty);
//                        aSqlCommand.CommandText = "IF EXISTS (SELECT * FROM sys.types WHERE is_table_type = 1 AND name = '" + sqlTypeName + "') DROP TYPE " + sqlTypeName + " ";
//                        //aSqlCommand.CommandText += Environment.NewLine + "GO";

//                        aSqlCommand.ExecuteNonQuery();
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                aMessageQueue.Enqueue("UNEXPECTED EXCEPTION: " + ex.ToString());
//                throw;
//            }
//        }

   
       

        //        private static void DeleteSQLViews(SqlCommand aSqlCommand, Queue aMessageQueue)
//        {
//            try
//            {
//                string sPath;
//#if (DEBUG)
//                sPath = Directory.GetParent(Directory.GetParent(Directory.GetParent(Application.StartupPath).ToString().Trim()).ToString().Trim()) + @"\DatabaseDefinition\SQL_Views\";
//#else
//                sPath = Path.GetDirectoryName(Application.ExecutablePath) + @"\SQL_Views\";
//#endif
//                if (System.IO.Directory.Exists(sPath) == true)
//                {
//                    DirectoryInfo di = new DirectoryInfo(sPath);
//                    foreach (FileInfo fi in di.GetFiles("*.SQL"))
//                    {
//                        string sqlViewName = fi.Name.Replace(".SQL", string.Empty);
//                        aSqlCommand.CommandText = "IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[" + sqlViewName + "]') AND OBJECTPROPERTY(id, N'IsView') = 1) ";
//                        aSqlCommand.CommandText += "DROP VIEW [dbo].[" + sqlViewName + "] ";
//                        //aSqlCommand.CommandText += Environment.NewLine + "GO";

//                        aSqlCommand.ExecuteNonQuery();
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                aMessageQueue.Enqueue("UNEXPECTED EXCEPTION: " + ex.ToString());
//                throw;
//            }
//        }
//        private static void DeleteSQLTriggers(SqlCommand aSqlCommand, Queue aMessageQueue)
//        {
//            try
//            {
//                string sPath;
//#if (DEBUG)
//                sPath = Directory.GetParent(Directory.GetParent(Directory.GetParent(Application.StartupPath).ToString().Trim()).ToString().Trim()) + @"\DatabaseDefinition\SQL_Triggers\";
//#else
//                sPath = Path.GetDirectoryName(Application.ExecutablePath) + @"\SQL_Triggers\";
//#endif
//                if (System.IO.Directory.Exists(sPath) == true)
//                {
//                    DirectoryInfo di = new DirectoryInfo(sPath);
//                    foreach (FileInfo fi in di.GetFiles("*.SQL"))
//                    {
//                        string sqlTriggerName = fi.Name.Replace(".SQL", string.Empty);
//                        aSqlCommand.CommandText = "IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[" + sqlTriggerName + "]') AND OBJECTPROPERTY(id, N'IsTrigger') = 1) ";
//                        aSqlCommand.CommandText += "DROP TRIGGER [dbo].[" + sqlTriggerName + "] ";
//                        //aSqlCommand.CommandText += Environment.NewLine + "GO";

//                        aSqlCommand.ExecuteNonQuery();
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                aMessageQueue.Enqueue("UNEXPECTED EXCEPTION: " + ex.ToString());
//                throw;
//            }
//        }
//        private static void CreateSQLFunctionsScalar(SqlCommand aSqlCommand, Queue aMessageQueue, Queue aProcessedQueue, bool aUpgradeProcess,
//            string aConnectionString, eDatabaseType aDatabaseType,
//            int majorVersion, int minorVersion, int revision, int modification, DateTime dbDate,
//            bool aIgnoreErrors, int aNoDataTables, string aAllocationFileGroup,
//            string aForecastFileGroup, string aHistoryFileGroup, int aNoHistoryFileGroup,
//            string aDailyHistoryFileGroup, int aNoDailyHistoryFileGroup, string aAuditFileGroup,
//            string aWeekArchiveFileGroup, string aDayArchiveFileGroup,
//            DatabaseObjects aDatabaseObjects, DatabaseObjectsSQLObjectPhase aPhase)
//        {
//            try
//            {
//                string sPath;
//#if (DEBUG)
//                sPath = Directory.GetParent(Directory.GetParent(Directory.GetParent(Application.StartupPath).ToString().Trim()).ToString().Trim()) + @"\DatabaseDefinition\SQL_Functions_Scalar\";
//#else
//                sPath = Path.GetDirectoryName(Application.ExecutablePath) + @"\SQL_Functions_Scalar\";
//#endif
//                if (System.IO.Directory.Exists(sPath) == true)
//                {
//                    if (aDatabaseObjects.SQLObject != null)
//                    {
//                        foreach (DatabaseObjectsSQLObject so in aDatabaseObjects.SQLObject)
//                        {
//                            if (so.Type == DatabaseObjectsSQLObjectType.FunctionScalar &&
//                                so.Phase == aPhase)
//                            {
//                                //if (aUpgradeProcess && isScalarFunctionFound(aSqlCommand, so.Name))
//                                //{
//                                //    continue;
//                                //}

//                                string prerequisite;
//                                if (so.Prerequisite != null &&
//                                    !VerifyPrerequisites(aSqlCommand, so, out prerequisite))
//                                {
//                                    aMessageQueue.Enqueue("Prerequisite " + prerequisite + " not found for " + so.Name);
//                                    throw new Exception("Prerequisite not found error");
//                                }

//                                ProcessUpgradeFile(
//                                FileProcessType.NewInstall,
//                                aSqlCommand,
//                                sPath + so.Name + ".SQL",
//                                "InstallFile",
//                                aMessageQueue,
//                                aProcessedQueue,
//                                false,
//                                aIgnoreErrors,
//                                majorVersion,
//                                minorVersion,
//                                revision,
//                                modification,
//                                dbDate,
//                                aDatabaseType,
//                                aNoDataTables,
//                                aAllocationFileGroup,
//                                aForecastFileGroup,
//                                aHistoryFileGroup,
//                                aNoHistoryFileGroup,
//                                aDailyHistoryFileGroup,
//                                aNoDailyHistoryFileGroup,
//                                aAuditFileGroup,
//                                aWeekArchiveFileGroup,
//                                aDayArchiveFileGroup,
//                                false);

//                                _htProcessed.Add(so.Name, null);
//                            }
//                        }
//                    }

//                    if (aPhase == DatabaseObjectsSQLObjectPhase.PreGenerate)
//                    {
//                        DirectoryInfo di = new DirectoryInfo(sPath);
//                        foreach (FileInfo fi in di.GetFiles("*.SQL"))
//                        {
//                            string functionName = fi.Name.Replace(".SQL", string.Empty);

//                            if (_htProcessed.ContainsKey(functionName) ||
//                                (aUpgradeProcess && isScalarFunctionFound(aSqlCommand, functionName)))
//                            {
//                                continue;
//                            }

//                            ProcessUpgradeFile(
//                                FileProcessType.NewInstall,
//                                aSqlCommand,
//                                fi.FullName,
//                                "InstallFile",
//                                aMessageQueue,
//                                aProcessedQueue,
//                                false,
//                                aIgnoreErrors,
//                                majorVersion,
//                                minorVersion,
//                                revision,
//                                modification,
//                                dbDate,
//                                aDatabaseType,
//                                aNoDataTables,
//                                aAllocationFileGroup,
//                                aForecastFileGroup,
//                                aHistoryFileGroup,
//                                aNoHistoryFileGroup,
//                                aDailyHistoryFileGroup,
//                                aNoDailyHistoryFileGroup,
//                                aAuditFileGroup,
//                                aWeekArchiveFileGroup,
//                                aDayArchiveFileGroup,
//                                false);
//                        }
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                aMessageQueue.Enqueue("UNEXPECTED EXCEPTION: " + ex.ToString());
//                throw;
//            }
//        }
//        private static void CreateSQLFunctionsTable(SqlCommand aSqlCommand, Queue aMessageQueue, Queue aProcessedQueue, bool aUpgradeProcess,
//            string aConnectionString, eDatabaseType aDatabaseType,
//            int majorVersion, int minorVersion, int revision, int modification, DateTime dbDate,
//            bool aIgnoreErrors, int aNoDataTables, string aAllocationFileGroup,
//            string aForecastFileGroup, string aHistoryFileGroup, int aNoHistoryFileGroup,
//            string aDailyHistoryFileGroup, int aNoDailyHistoryFileGroup, string aAuditFileGroup,
//            string aWeekArchiveFileGroup, string aDayArchiveFileGroup,
//            DatabaseObjects aDatabaseObjects, DatabaseObjectsSQLObjectPhase aPhase)
//        {
//            try
//            {
//                string sPath;
//#if (DEBUG)
//                sPath = Directory.GetParent(Directory.GetParent(Directory.GetParent(Application.StartupPath).ToString().Trim()).ToString().Trim()) + @"\DatabaseDefinition\SQL_Functions_Table\";
//#else
//                sPath = Path.GetDirectoryName(Application.ExecutablePath) + @"\SQL_Functions_Table\";
//#endif
//                if (System.IO.Directory.Exists(sPath) == true)
//                {
//                    if (aDatabaseObjects.SQLObject != null)
//                    {
//                        foreach (DatabaseObjectsSQLObject so in aDatabaseObjects.SQLObject)
//                        {
//                            if (so.Type == DatabaseObjectsSQLObjectType.FunctionTable &&
//                                so.Phase == aPhase)
//                            {
//                                //if (aUpgradeProcess && isTableFunctionFound(aSqlCommand, so.Name))
//                                //{
//                                //    continue;
//                                //}

//                                string prerequisite;
//                                if (so.Prerequisite != null &&
//                                    !VerifyPrerequisites(aSqlCommand, so, out prerequisite))
//                                {
//                                    aMessageQueue.Enqueue("Prerequisite " + prerequisite + " not found for " + so.Name);
//                                    throw new Exception("Prerequisite not found error");
//                                }

//                                ProcessUpgradeFile(
//                                FileProcessType.NewInstall,
//                                aSqlCommand,
//                                sPath + so.Name + ".SQL",
//                                "InstallFile",
//                                aMessageQueue,
//                                aProcessedQueue,
//                                false,
//                                aIgnoreErrors,
//                                majorVersion,
//                                minorVersion,
//                                revision,
//                                modification,
//                                dbDate,
//                                aDatabaseType,
//                                aNoDataTables,
//                                aAllocationFileGroup,
//                                aForecastFileGroup,
//                                aHistoryFileGroup,
//                                aNoHistoryFileGroup,
//                                aDailyHistoryFileGroup,
//                                aNoDailyHistoryFileGroup,
//                                aAuditFileGroup,
//                                aWeekArchiveFileGroup,
//                                aDayArchiveFileGroup,
//                                false);

//                                _htProcessed.Add(so.Name, null);
//                            }
//                        }
//                    }

//                    if (aPhase == DatabaseObjectsSQLObjectPhase.PreGenerate)
//                    {
//                        DirectoryInfo di = new DirectoryInfo(sPath);
//                        foreach (FileInfo fi in di.GetFiles("*.SQL"))
//                        {
//                            string functionName = fi.Name.Replace(".SQL", string.Empty);

//                            if (_htProcessed.ContainsKey(functionName) ||
//                                (aUpgradeProcess && isTableFunctionFound(aSqlCommand, functionName)))
//                            {
//                                continue;
//                            }

//                            ProcessUpgradeFile(
//                                FileProcessType.NewInstall,
//                                aSqlCommand,
//                                fi.FullName,
//                                "InstallFile",
//                                aMessageQueue,
//                                aProcessedQueue,
//                                false,
//                                aIgnoreErrors,
//                                majorVersion,
//                                minorVersion,
//                                revision,
//                                modification,
//                                dbDate,
//                                aDatabaseType,
//                                aNoDataTables,
//                                aAllocationFileGroup,
//                                aForecastFileGroup,
//                                aHistoryFileGroup,
//                                aNoHistoryFileGroup,
//                                aDailyHistoryFileGroup,
//                                aNoDailyHistoryFileGroup,
//                                aAuditFileGroup,
//                                aWeekArchiveFileGroup,
//                                aDayArchiveFileGroup,
//                                false);
//                        }
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                aMessageQueue.Enqueue("UNEXPECTED EXCEPTION: " + ex.ToString());
//                throw;
//            }
//        }
//        private static void CreateSQLTypes(SqlCommand aSqlCommand, Queue aMessageQueue, Queue aProcessedQueue, bool aUpgradeProcess,
//            string aConnectionString, eDatabaseType aDatabaseType,
//            int majorVersion, int minorVersion, int revision, int modification, DateTime dbDate,
//            bool aIgnoreErrors, int aNoDataTables, string aAllocationFileGroup,
//            string aForecastFileGroup, string aHistoryFileGroup, int aNoHistoryFileGroup,
//            string aDailyHistoryFileGroup, int aNoDailyHistoryFileGroup, string aAuditFileGroup,
//            string aWeekArchiveFileGroup, string aDayArchiveFileGroup,
//            DatabaseObjects aDatabaseObjects, DatabaseObjectsSQLObjectPhase aPhase)
//        {
//            try
//            {
//                string sPath;
//#if (DEBUG)
//                sPath = Directory.GetParent(Directory.GetParent(Directory.GetParent(Application.StartupPath).ToString().Trim()).ToString().Trim()) + @"\DatabaseDefinition\SQL_Types\";
//#else
//                sPath = Path.GetDirectoryName(Application.ExecutablePath) + @"\SQL_Types\";
//#endif
//                if (System.IO.Directory.Exists(sPath) == true)
//                {
//                    if (aDatabaseObjects.SQLObject != null)
//                    {
//                        foreach (DatabaseObjectsSQLObject so in aDatabaseObjects.SQLObject)
//                        {
//                            if (so.Type == DatabaseObjectsSQLObjectType.Type &&
//                                so.Phase == aPhase)
//                            {
//                                //if (aUpgradeProcess && isTypeFound(aSqlCommand, so.Name))
//                                //{
//                                //    continue;
//                                //}

//                                string prerequisite;
//                                if (so.Prerequisite != null &&
//                                    !VerifyPrerequisites(aSqlCommand, so, out prerequisite))
//                                {
//                                    aMessageQueue.Enqueue("Prerequisite " + prerequisite + " not found for " + so.Name);
//                                    throw new Exception("Prerequisite not found error");
//                                }

//                                ProcessUpgradeFile(
//                                FileProcessType.NewInstall,
//                                aSqlCommand,
//                                sPath + so.Name + ".SQL",
//                                "InstallFile",
//                                aMessageQueue,
//                                aProcessedQueue,
//                                false,
//                                aIgnoreErrors,
//                                majorVersion,
//                                minorVersion,
//                                revision,
//                                modification,
//                                dbDate,
//                                aDatabaseType,
//                                aNoDataTables,
//                                aAllocationFileGroup,
//                                aForecastFileGroup,
//                                aHistoryFileGroup,
//                                aNoHistoryFileGroup,
//                                aDailyHistoryFileGroup,
//                                aNoDailyHistoryFileGroup,
//                                aAuditFileGroup,
//                                aWeekArchiveFileGroup,
//                                aDayArchiveFileGroup,
//                                false);

//                                _htProcessed.Add(so.Name, null);
//                            }
//                        }
//                    }

//                    if (aPhase == DatabaseObjectsSQLObjectPhase.PreGenerate)
//                    {
//                        DirectoryInfo di = new DirectoryInfo(sPath);
//                        foreach (FileInfo fi in di.GetFiles("*.SQL"))
//                        {
//                            string sqlTypeName = fi.Name.Replace(".SQL", string.Empty);

//                            if (_htProcessed.ContainsKey(sqlTypeName) ||
//                                (aUpgradeProcess && isTypeFound(aSqlCommand, sqlTypeName)))
//                            {
//                                continue;
//                            }

//                            ProcessUpgradeFile(
//                                //FileProcessType.NewInstall,
//                                FileProcessType.NewInstall,
//                                aSqlCommand,
//                                fi.FullName,
//                                "InstallFile",
//                                aMessageQueue,
//                                aProcessedQueue,
//                                false,
//                                aIgnoreErrors,
//                                majorVersion,
//                                minorVersion,
//                                revision,
//                                modification,
//                                dbDate,
//                                aDatabaseType,
//                                aNoDataTables,
//                                aAllocationFileGroup,
//                                aForecastFileGroup,
//                                aHistoryFileGroup,
//                                aNoHistoryFileGroup,
//                                aDailyHistoryFileGroup,
//                                aNoDailyHistoryFileGroup,
//                                aAuditFileGroup,
//                                aWeekArchiveFileGroup,
//                                aDayArchiveFileGroup,
//                                false);
//                        }
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                aMessageQueue.Enqueue("UNEXPECTED EXCEPTION: " + ex.ToString());
//                throw;
//            }
//        }
//        private static void CreateSQLStoredProcedures(SqlCommand aSqlCommand, Queue aMessageQueue, Queue aProcessedQueue, bool aUpgradeProcess,
//            string aConnectionString, eDatabaseType aDatabaseType,
//            int majorVersion, int minorVersion, int revision, int modification, DateTime dbDate,
//            bool aIgnoreErrors, int aNoDataTables, string aAllocationFileGroup,
//            string aForecastFileGroup, string aHistoryFileGroup, int aNoHistoryFileGroup,
//            string aDailyHistoryFileGroup, int aNoDailyHistoryFileGroup, string aAuditFileGroup,
//            string aWeekArchiveFileGroup, string aDayArchiveFileGroup,
//            DatabaseObjects aDatabaseObjects, DatabaseObjectsSQLObjectPhase aPhase)
//        {
//            try
//            {
//                string sPath = Path.GetDirectoryName(Application.ExecutablePath) + @"\SQL_StoredProcedures\";
//#if (DEBUG)
//                sPath = Directory.GetParent(Directory.GetParent(Directory.GetParent(Application.StartupPath).ToString().Trim()).ToString().Trim()) + @"\DatabaseDefinition\SQL_StoredProcedures\";
//#else
//                sPath = Path.GetDirectoryName(Application.ExecutablePath) + @"\SQL_StoredProcedures\";
//#endif
//                if (System.IO.Directory.Exists(sPath) == true)
//                {
//                    if (aDatabaseObjects.SQLObject != null)
//                    {
//                        foreach (DatabaseObjectsSQLObject so in aDatabaseObjects.SQLObject)
//                        {
//                            if (so.Type == DatabaseObjectsSQLObjectType.StoredProcedure &&
//                                so.Phase == aPhase)
//                            {
//                                //if (aUpgradeProcess && isStoredProcedureFound(aSqlCommand, so.Name))
//                                //{
//                                //    continue;
//                                //}

//                                string prerequisite;
//                                if (so.Prerequisite != null &&
//                                    !VerifyPrerequisites(aSqlCommand, so, out prerequisite))
//                                {
//                                    aMessageQueue.Enqueue("Prerequisite " + prerequisite + " not found for " + so.Name);
//                                    throw new Exception("Prerequisite not found error");
//                                }

//                                ProcessUpgradeFile(
//                                FileProcessType.NewInstall,
//                                aSqlCommand,
//                                sPath + so.Name + ".SQL",
//                                "InstallFile",
//                                aMessageQueue,
//                                aProcessedQueue,
//                                false,
//                                aIgnoreErrors,
//                                majorVersion,
//                                minorVersion,
//                                revision,
//                                modification,
//                                dbDate,
//                                aDatabaseType,
//                                aNoDataTables,
//                                aAllocationFileGroup,
//                                aForecastFileGroup,
//                                aHistoryFileGroup,
//                                aNoHistoryFileGroup,
//                                aDailyHistoryFileGroup,
//                                aNoDailyHistoryFileGroup,
//                                aAuditFileGroup,
//                                aWeekArchiveFileGroup,
//                                aDayArchiveFileGroup,
//                                false);

//                                _htProcessed.Add(so.Name, null);
//                            }
//                        }
//                    }

//                    if (aPhase == DatabaseObjectsSQLObjectPhase.PreGenerate)
//                    {
//                        DirectoryInfo di = new DirectoryInfo(sPath);
//                        foreach (FileInfo fi in di.GetFiles("*.SQL"))
//                        {
//                            string procedureName = fi.Name.Replace(".SQL", string.Empty);

//                            if (_htProcessed.ContainsKey(procedureName) ||
//                                (aUpgradeProcess && isStoredProcedureFound(aSqlCommand, procedureName)))
//                            {
//                                continue;
//                            }

//                            ProcessUpgradeFile(
//                                FileProcessType.NewInstall,
//                                aSqlCommand,
//                                fi.FullName,
//                                "InstallFile",
//                                aMessageQueue,
//                                aProcessedQueue,
//                                false,
//                                aIgnoreErrors,
//                                majorVersion,
//                                minorVersion,
//                                revision,
//                                modification,
//                                dbDate,
//                                aDatabaseType,
//                                aNoDataTables,
//                                aAllocationFileGroup,
//                                aForecastFileGroup,
//                                aHistoryFileGroup,
//                                aNoHistoryFileGroup,
//                                aDailyHistoryFileGroup,
//                                aNoDailyHistoryFileGroup,
//                                aAuditFileGroup,
//                                aWeekArchiveFileGroup,
//                                aDayArchiveFileGroup,
//                                false);
//                        }
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                aMessageQueue.Enqueue("UNEXPECTED EXCEPTION: " + ex.ToString());
//                throw;
//            }
//        }
//        private static void CreateSQLViews(SqlCommand aSqlCommand, Queue aMessageQueue, Queue aProcessedQueue, bool aUpgradeProcess,
//            string aConnectionString, eDatabaseType aDatabaseType,
//            int majorVersion, int minorVersion, int revision, int modification, DateTime dbDate,
//            bool aIgnoreErrors, int aNoDataTables, string aAllocationFileGroup,
//            string aForecastFileGroup, string aHistoryFileGroup, int aNoHistoryFileGroup,
//            string aDailyHistoryFileGroup, int aNoDailyHistoryFileGroup, string aAuditFileGroup,
//            string aWeekArchiveFileGroup, string aDayArchiveFileGroup,
//            DatabaseObjects aDatabaseObjects, DatabaseObjectsSQLObjectPhase aPhase)
//        {
//            try
//            {
//                string sPath;
//#if (DEBUG)
//                sPath = Directory.GetParent(Directory.GetParent(Directory.GetParent(Application.StartupPath).ToString().Trim()).ToString().Trim()) + @"\DatabaseDefinition\SQL_Views\";
//#else
//                sPath = Path.GetDirectoryName(Application.ExecutablePath) + @"\SQL_Views\";
//#endif
//                if (System.IO.Directory.Exists(sPath) == true)
//                {
//                    if (aDatabaseObjects.SQLObject != null)
//                    {
//                        foreach (DatabaseObjectsSQLObject so in aDatabaseObjects.SQLObject)
//                        {
//                            if (so.Type == DatabaseObjectsSQLObjectType.View &&
//                                so.Phase == aPhase)
//                            {
//                                //if (aUpgradeProcess && isViewFound(aSqlCommand, so.Name))
//                                //{
//                                //    continue;
//                                //}

//                                string prerequisite;
//                                if (so.Prerequisite != null &&
//                                    !VerifyPrerequisites(aSqlCommand, so, out prerequisite))
//                                {
//                                    aMessageQueue.Enqueue("Prerequisite " + prerequisite + " not found for " + so.Name);
//                                    throw new Exception("Prerequisite not found error");
//                                }

//                                ProcessUpgradeFile(
//                                FileProcessType.NewInstall,
//                                aSqlCommand,
//                                sPath + so.Name + ".SQL",
//                                "InstallFile",
//                                aMessageQueue,
//                                aProcessedQueue,
//                                false,
//                                aIgnoreErrors,
//                                majorVersion,
//                                minorVersion,
//                                revision,
//                                modification,
//                                dbDate,
//                                aDatabaseType,
//                                aNoDataTables,
//                                aAllocationFileGroup,
//                                aForecastFileGroup,
//                                aHistoryFileGroup,
//                                aNoHistoryFileGroup,
//                                aDailyHistoryFileGroup,
//                                aNoDailyHistoryFileGroup,
//                                aAuditFileGroup,
//                                aWeekArchiveFileGroup,
//                                aDayArchiveFileGroup,
//                                false);

//                                _htProcessed.Add(so.Name, null);
//                            }
//                        }
//                    }

//                    if (aPhase == DatabaseObjectsSQLObjectPhase.PreGenerate)
//                    {
//                        DirectoryInfo di = new DirectoryInfo(sPath);
//                        foreach (FileInfo fi in di.GetFiles("*.SQL"))
//                        {
//                            string viewName = fi.Name.Replace(".SQL", string.Empty);

//                            if (_htProcessed.ContainsKey(viewName) ||
//                                (aUpgradeProcess && isViewFound(aSqlCommand, viewName)))
//                            {
//                                continue;
//                            }

//                            ProcessUpgradeFile(
//                                FileProcessType.NewInstall,
//                                aSqlCommand,
//                                fi.FullName,
//                                "InstallFile",
//                                aMessageQueue,
//                                aProcessedQueue,
//                                false,
//                                aIgnoreErrors,
//                                majorVersion,
//                                minorVersion,
//                                revision,
//                                modification,
//                                dbDate,
//                                aDatabaseType,
//                                aNoDataTables,
//                                aAllocationFileGroup,
//                                aForecastFileGroup,
//                                aHistoryFileGroup,
//                                aNoHistoryFileGroup,
//                                aDailyHistoryFileGroup,
//                                aNoDailyHistoryFileGroup,
//                                aAuditFileGroup,
//                                aWeekArchiveFileGroup,
//                                aDayArchiveFileGroup,
//                                false);
//                        }
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                aMessageQueue.Enqueue("UNEXPECTED EXCEPTION: " + ex.ToString());
//                throw;
//            }
//        }
//        private static void CreateSQLTriggers(SqlCommand aSqlCommand, Queue aMessageQueue, Queue aProcessedQueue, bool aUpgradeProcess,
//            string aConnectionString, eDatabaseType aDatabaseType,
//            int majorVersion, int minorVersion, int revision, int modification, DateTime dbDate,
//            bool aIgnoreErrors, int aNoDataTables, string aAllocationFileGroup,
//            string aForecastFileGroup, string aHistoryFileGroup, int aNoHistoryFileGroup,
//            string aDailyHistoryFileGroup, int aNoDailyHistoryFileGroup, string aAuditFileGroup,
//            string aWeekArchiveFileGroup, string aDayArchiveFileGroup,
//            DatabaseObjects aDatabaseObjects, DatabaseObjectsSQLObjectPhase aPhase)
//        {
//            try
//            {
//                string sPath;
//#if (DEBUG)
//                sPath = Directory.GetParent(Directory.GetParent(Directory.GetParent(Application.StartupPath).ToString().Trim()).ToString().Trim()) + @"\DatabaseDefinition\SQL_Triggers\";
//#else
//                sPath = Path.GetDirectoryName(Application.ExecutablePath) + @"\SQL_Triggers\";
//#endif
//                if (System.IO.Directory.Exists(sPath) == true)
//                {

//                    if (aDatabaseObjects.SQLObject != null)
//                    {
//                        foreach (DatabaseObjectsSQLObject so in aDatabaseObjects.SQLObject)
//                        {
//                            if (so.Type == DatabaseObjectsSQLObjectType.Trigger &&
//                                so.Phase == aPhase)
//                            {
//                                //if (aUpgradeProcess && isTriggerFound(aSqlCommand, so.Name))
//                                //{
//                                //    continue;
//                                //}

//                                string prerequisite;
//                                if (so.Prerequisite != null &&
//                                    !VerifyPrerequisites(aSqlCommand, so, out prerequisite))
//                                {
//                                    aMessageQueue.Enqueue("Prerequisite " + prerequisite + " not found for " + so.Name);
//                                    throw new Exception("Prerequisite not found error");
//                                }

//                                ProcessUpgradeFile(
//                                FileProcessType.NewInstall,
//                                aSqlCommand,
//                                sPath + so.Name + ".SQL",
//                                "InstallFile",
//                                aMessageQueue,
//                                aProcessedQueue,
//                                false,
//                                aIgnoreErrors,
//                                majorVersion,
//                                minorVersion,
//                                revision,
//                                modification,
//                                dbDate,
//                                aDatabaseType,
//                                aNoDataTables,
//                                aAllocationFileGroup,
//                                aForecastFileGroup,
//                                aHistoryFileGroup,
//                                aNoHistoryFileGroup,
//                                aDailyHistoryFileGroup,
//                                aNoDailyHistoryFileGroup,
//                                aAuditFileGroup,
//                                aWeekArchiveFileGroup,
//                                aDayArchiveFileGroup,
//                                false);

//                                _htProcessed.Add(so.Name, null);
//                            }
//                        }
//                    }

//                    if (aPhase == DatabaseObjectsSQLObjectPhase.PreGenerate)
//                    {
//                        DirectoryInfo di = new DirectoryInfo(sPath);
//                        foreach (FileInfo fi in di.GetFiles("*.SQL"))
//                        {
//                            string triggerName = fi.Name.Replace(".SQL", string.Empty);

//                            if (_htProcessed.ContainsKey(triggerName) ||
//                                (aUpgradeProcess && isTriggerFound(aSqlCommand, triggerName)))
//                            {
//                                continue;
//                            }

//                            ProcessUpgradeFile(
//                                FileProcessType.NewInstall,
//                                aSqlCommand,
//                                fi.FullName,
//                                "InstallFile",
//                                aMessageQueue,
//                                aProcessedQueue,
//                                false,
//                                aIgnoreErrors,
//                                majorVersion,
//                                minorVersion,
//                                revision,
//                                modification,
//                                dbDate,
//                                aDatabaseType,
//                                aNoDataTables,
//                                aAllocationFileGroup,
//                                aForecastFileGroup,
//                                aHistoryFileGroup,
//                                aNoHistoryFileGroup,
//                                aDailyHistoryFileGroup,
//                                aNoDailyHistoryFileGroup,
//                                aAuditFileGroup,
//                                aWeekArchiveFileGroup,
//                                aDayArchiveFileGroup,
//                                false);
//                        }
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                aMessageQueue.Enqueue("UNEXPECTED EXCEPTION: " + ex.ToString());
//                throw;
//            }
//        }

//        private static void CreateSQLTables(SqlCommand aSqlCommand, Queue aMessageQueue, Queue aProcessedQueue, bool aUpgradeProcess,
//            string aConnectionString, eDatabaseType aDatabaseType,
//            int majorVersion, int minorVersion, int revision, int modification, DateTime dbDate,
//            bool aIgnoreErrors, int aNoDataTables, string aAllocationFileGroup,
//            string aForecastFileGroup, string aHistoryFileGroup, int aNoHistoryFileGroup,
//            string aDailyHistoryFileGroup, int aNoDailyHistoryFileGroup, string aAuditFileGroup,
//            string aWeekArchiveFileGroup, string aDayArchiveFileGroup,
//           DatabaseObjects aDatabaseObjects, DatabaseObjectsSQLObjectPhase aPhase)
//        {
//            try
//            {
//                string sPath;
//#if (DEBUG)
//                sPath = Directory.GetParent(Directory.GetParent(Directory.GetParent(Application.StartupPath).ToString().Trim()).ToString().Trim()) + @"\DatabaseDefinition\SQL_Tables\";
//#else
//                sPath = Path.GetDirectoryName(Application.ExecutablePath) + @"\SQL_Tables\";
//#endif
//                if (System.IO.Directory.Exists(sPath) == true)
//                {

//                    if (aDatabaseObjects.SQLObject != null)
//                    {
//                        foreach (DatabaseObjectsSQLObject so in aDatabaseObjects.SQLObject)
//                        {
//                            if (so.Type == DatabaseObjectsSQLObjectType.Table &&
//                                so.Phase == aPhase)
//                            {
//                                string prerequisite;
//                                if (so.Prerequisite != null &&
//                                    !VerifyPrerequisites(aSqlCommand, so, out prerequisite))
//                                {
//                                    aMessageQueue.Enqueue("Prerequisite " + prerequisite + " not found for " + so.Name);
//                                    throw new Exception("Prerequisite not found error");
//                                }

//                                if (aUpgradeProcess && isTableFound(aSqlCommand, so.Name))
//                                {
//                                    UpdateTable(aSqlCommand, aMessageQueue, so.Name, sPath + so.Name + ".SQL");
//                                }
//                                else
//                                {
//                                    ProcessUpgradeFile(
//                                    FileProcessType.NewInstall,
//                                    aSqlCommand,
//                                    sPath + so.Name + ".SQL",
//                                    "InstallFile",
//                                    aMessageQueue,
//                                    aProcessedQueue,
//                                    false,
//                                    aIgnoreErrors,
//                                    majorVersion,
//                                    minorVersion,
//                                    revision,
//                                    modification,
//                                    dbDate,
//                                    aDatabaseType,
//                                    aNoDataTables,
//                                    aAllocationFileGroup,
//                                    aForecastFileGroup,
//                                    aHistoryFileGroup,
//                                    aNoHistoryFileGroup,
//                                    aDailyHistoryFileGroup,
//                                    aNoDailyHistoryFileGroup,
//                                    aAuditFileGroup,
//                                    aWeekArchiveFileGroup,
//                                    aDayArchiveFileGroup,
//                                    false);

//                                    _htProcessed.Add(so.Name, null);
//                                }
//                            }
//                        }
//                    }

//                    if (aPhase == DatabaseObjectsSQLObjectPhase.PreGenerate)
//                    {
//                        DirectoryInfo di = new DirectoryInfo(sPath);
//                        foreach (FileInfo fi in di.GetFiles("*.SQL"))
//                        {
//                            string tableName = fi.Name.Replace(".SQL", string.Empty);

//                            if (_htProcessed.ContainsKey(tableName))
//                            {
//                                continue;
//                            }

//                            if (aUpgradeProcess && isTableFound(aSqlCommand, tableName))
//                            {
//                                UpdateTable(aSqlCommand, aMessageQueue, tableName, fi.FullName);
//                            }
//                            else
//                            {
//                                ProcessUpgradeFile(
//                                    FileProcessType.NewInstall,
//                                    aSqlCommand,
//                                    fi.FullName,
//                                    "InstallFile",
//                                    aMessageQueue,
//                                    aProcessedQueue,
//                                    false,
//                                    aIgnoreErrors,
//                                    majorVersion,
//                                    minorVersion,
//                                    revision,
//                                    modification,
//                                    dbDate,
//                                    aDatabaseType,
//                                    aNoDataTables,
//                                    aAllocationFileGroup,
//                                    aForecastFileGroup,
//                                    aHistoryFileGroup,
//                                    aNoHistoryFileGroup,
//                                    aDailyHistoryFileGroup,
//                                    aNoDailyHistoryFileGroup,
//                                    aAuditFileGroup,
//                                    aWeekArchiveFileGroup,
//                                    aDayArchiveFileGroup,
//                                    false);
//                            }
//                        }
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                aMessageQueue.Enqueue("UNEXPECTED EXCEPTION: " + ex.ToString());
//                throw;
//            }
//        }

//        private static void CreateSQLTableKeys(SqlCommand aSqlCommand, Queue aMessageQueue, Queue aProcessedQueue, bool aUpgradeProcess,
//            string aConnectionString, eDatabaseType aDatabaseType,
//            int majorVersion, int minorVersion, int revision, int modification, DateTime dbDate,
//            bool aIgnoreErrors, int aNoDataTables, string aAllocationFileGroup,
//            string aForecastFileGroup, string aHistoryFileGroup, int aNoHistoryFileGroup,
//            string aDailyHistoryFileGroup, int aNoDailyHistoryFileGroup, string aAuditFileGroup,
//            string aWeekArchiveFileGroup, string aDayArchiveFileGroup,
//           DatabaseObjects aDatabaseObjects, DatabaseObjectsSQLObjectPhase aPhase)
//        {
//            try
//            {


//                string sPath;
//#if (DEBUG)
//                sPath = Directory.GetParent(Directory.GetParent(Directory.GetParent(Application.StartupPath).ToString().Trim()).ToString().Trim()) + @"\DatabaseDefinition\SQL_Constraints\";
//#else
//                sPath = Path.GetDirectoryName(Application.ExecutablePath) + @"\SQL_Constraints\";
//#endif
//                if (System.IO.Directory.Exists(sPath) == true)
//                {

//                    if (aDatabaseObjects.SQLObject != null)
//                    {
//                        foreach (DatabaseObjectsSQLObject so in aDatabaseObjects.SQLObject)
//                        {
//                            if (so.Type == DatabaseObjectsSQLObjectType.Constraint &&
//                                so.Phase == aPhase)
//                            {
//                                if (aUpgradeProcess && isConstraintFound(aSqlCommand, so.Name))
//                                {
//                                    continue;
//                                }

//                                string prerequisite;
//                                if (so.Prerequisite != null &&
//                                    !VerifyPrerequisites(aSqlCommand, so, out prerequisite))
//                                {
//                                    aMessageQueue.Enqueue("Prerequisite " + prerequisite + " not found for " + so.Name);
//                                    throw new Exception("Prerequisite not found error");
//                                }

//                                ProcessUpgradeFile(
//                                FileProcessType.NewInstall,
//                                aSqlCommand,
//                                sPath + so.Name + ".SQL",
//                                "InstallFile",
//                                aMessageQueue,
//                                aProcessedQueue,
//                                false,
//                                aIgnoreErrors,
//                                majorVersion,
//                                minorVersion,
//                                revision,
//                                modification,
//                                dbDate,
//                                aDatabaseType,
//                                aNoDataTables,
//                                aAllocationFileGroup,
//                                aForecastFileGroup,
//                                aHistoryFileGroup,
//                                aNoHistoryFileGroup,
//                                aDailyHistoryFileGroup,
//                                aNoDailyHistoryFileGroup,
//                                aAuditFileGroup,
//                                aWeekArchiveFileGroup,
//                                aDayArchiveFileGroup,
//                                false);

//                                _htProcessed.Add(so.Name, null);
//                            }
//                        }
//                    }

//                    if (aPhase == DatabaseObjectsSQLObjectPhase.PreGenerate)
//                    {
//                        DirectoryInfo di = new DirectoryInfo(sPath);
//                        foreach (FileInfo fi in di.GetFiles("*_PK.SQL"))
//                        {
//                            string constraintName = fi.Name.Replace(".SQL", string.Empty);

//                            if (_htProcessed.ContainsKey(constraintName) ||
//                                (aUpgradeProcess && isPrimaryKeyFound(aSqlCommand, constraintName, fi.FullName)))
//                            {
//                                continue;
//                            }

//                            ProcessUpgradeFile(
//                                FileProcessType.NewInstall,
//                                aSqlCommand,
//                                fi.FullName,
//                                "InstallFile",
//                                aMessageQueue,
//                                aProcessedQueue,
//                                false,
//                                aIgnoreErrors,
//                                majorVersion,
//                                minorVersion,
//                                revision,
//                                modification,
//                                dbDate,
//                                aDatabaseType,
//                                aNoDataTables,
//                                aAllocationFileGroup,
//                                aForecastFileGroup,
//                                aHistoryFileGroup,
//                                aNoHistoryFileGroup,
//                                aDailyHistoryFileGroup,
//                                aNoDailyHistoryFileGroup,
//                                aAuditFileGroup,
//                                aWeekArchiveFileGroup,
//                                aDayArchiveFileGroup,
//                                false);
//                        }
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                aMessageQueue.Enqueue("UNEXPECTED EXCEPTION: " + ex.ToString());
//                throw;
//            }
//        }

//        private static void CreateSQLIndexes(SqlCommand aSqlCommand, Queue aMessageQueue, Queue aProcessedQueue, bool aUpgradeProcess,
//            string aConnectionString, eDatabaseType aDatabaseType,
//            int majorVersion, int minorVersion, int revision, int modification, DateTime dbDate,
//            bool aIgnoreErrors, int aNoDataTables, string aAllocationFileGroup,
//            string aForecastFileGroup, string aHistoryFileGroup, int aNoHistoryFileGroup,
//            string aDailyHistoryFileGroup, int aNoDailyHistoryFileGroup, string aAuditFileGroup,
//            string aWeekArchiveFileGroup, string aDayArchiveFileGroup,
//            DatabaseObjects aDatabaseObjects, DatabaseObjectsSQLObjectPhase aPhase)
//        {
//            try
//            {
//                string sPath;
//#if (DEBUG)
//                sPath = Directory.GetParent(Directory.GetParent(Directory.GetParent(Application.StartupPath).ToString().Trim()).ToString().Trim()) + @"\DatabaseDefinition\SQL_Indexes\";
//#else
//                sPath = Path.GetDirectoryName(Application.ExecutablePath) + @"\SQL_Indexes\";
//#endif
//                if (System.IO.Directory.Exists(sPath) == true)
//                {

//                    if (aDatabaseObjects.SQLObject != null)
//                    {
//                        foreach (DatabaseObjectsSQLObject so in aDatabaseObjects.SQLObject)
//                        {
//                            if (so.Type == DatabaseObjectsSQLObjectType.Index &&
//                                so.Phase == aPhase)
//                            {
//                                if (aUpgradeProcess && isIndexFound(aSqlCommand, so.Name))
//                                {
//                                    continue;
//                                }

//                                string prerequisite;
//                                if (so.Prerequisite != null &&
//                                    !VerifyPrerequisites(aSqlCommand, so, out prerequisite))
//                                {
//                                    aMessageQueue.Enqueue("Prerequisite " + prerequisite + " not found for " + so.Name);
//                                    throw new Exception("Prerequisite not found error");
//                                }

//                                ProcessUpgradeFile(
//                                FileProcessType.NewInstall,
//                                aSqlCommand,
//                                sPath + so.Name + ".SQL",
//                                "InstallFile",
//                                aMessageQueue,
//                                aProcessedQueue,
//                                false,
//                                aIgnoreErrors,
//                                majorVersion,
//                                minorVersion,
//                                revision,
//                                modification,
//                                dbDate,
//                                aDatabaseType,
//                                aNoDataTables,
//                                aAllocationFileGroup,
//                                aForecastFileGroup,
//                                aHistoryFileGroup,
//                                aNoHistoryFileGroup,
//                                aDailyHistoryFileGroup,
//                                aNoDailyHistoryFileGroup,
//                                aAuditFileGroup,
//                                aWeekArchiveFileGroup,
//                                aDayArchiveFileGroup,
//                                false);

//                                _htProcessed.Add(so.Name, null);
//                            }
//                        }
//                    }

//                    if (aPhase == DatabaseObjectsSQLObjectPhase.PreGenerate)
//                    {
//                        DirectoryInfo di = new DirectoryInfo(sPath);
//                        foreach (FileInfo fi in di.GetFiles("*.SQL"))
//                        {
//                            string indexName = fi.Name.Replace(".SQL", string.Empty);

//                            if (_htProcessed.ContainsKey(indexName) ||
//                                (aUpgradeProcess && isIndexFound(aSqlCommand, indexName)))
//                            {
//                                continue;
//                            }

//                            ProcessUpgradeFile(
//                                FileProcessType.NewInstall,
//                                aSqlCommand,
//                                fi.FullName,
//                                "InstallFile",
//                                aMessageQueue,
//                                aProcessedQueue,
//                                false,
//                                aIgnoreErrors,
//                                majorVersion,
//                                minorVersion,
//                                revision,
//                                modification,
//                                dbDate,
//                                aDatabaseType,
//                                aNoDataTables,
//                                aAllocationFileGroup,
//                                aForecastFileGroup,
//                                aHistoryFileGroup,
//                                aNoHistoryFileGroup,
//                                aDailyHistoryFileGroup,
//                                aNoDailyHistoryFileGroup,
//                                aAuditFileGroup,
//                                aWeekArchiveFileGroup,
//                                aDayArchiveFileGroup,
//                                false);
//                        }
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                aMessageQueue.Enqueue("UNEXPECTED EXCEPTION: " + ex.ToString());
//                throw;
//            }
//        }
//        private static void CreateSQLConstraints(SqlCommand aSqlCommand, Queue aMessageQueue, Queue aProcessedQueue, bool aUpgradeProcess,
//            string aConnectionString, eDatabaseType aDatabaseType,
//            int majorVersion, int minorVersion, int revision, int modification, DateTime dbDate,
//            bool aIgnoreErrors, int aNoDataTables, string aAllocationFileGroup,
//            string aForecastFileGroup, string aHistoryFileGroup, int aNoHistoryFileGroup,
//            string aDailyHistoryFileGroup, int aNoDailyHistoryFileGroup, string aAuditFileGroup,
//            string aWeekArchiveFileGroup, string aDayArchiveFileGroup,
//            DatabaseObjects aDatabaseObjects, DatabaseObjectsSQLObjectPhase aPhase)
//        {
//            try
//            {
//                string sPath;
//#if (DEBUG)
//                sPath = Directory.GetParent(Directory.GetParent(Directory.GetParent(Application.StartupPath).ToString().Trim()).ToString().Trim()) + @"\DatabaseDefinition\SQL_Constraints\";
//#else
//                sPath = Path.GetDirectoryName(Application.ExecutablePath) + @"\SQL_Constraints\";
//#endif
//                if (System.IO.Directory.Exists(sPath) == true)
//                {
//                    if (aDatabaseObjects.SQLObject != null)
//                    {
//                        foreach (DatabaseObjectsSQLObject so in aDatabaseObjects.SQLObject)
//                        {
//                            if (so.Type == DatabaseObjectsSQLObjectType.Constraint &&
//                                so.Phase == aPhase)
//                            {
//                                if (aUpgradeProcess && isConstraintFound(aSqlCommand, so.Name))
//                                {
//                                    continue;
//                                }

//                                string prerequisite;
//                                if (so.Prerequisite != null &&
//                                    !VerifyPrerequisites(aSqlCommand, so, out prerequisite))
//                                {
//                                    aMessageQueue.Enqueue("Prerequisite " + prerequisite + " not found for " + so.Name);
//                                    throw new Exception("Prerequisite not found error");
//                                }

//                                ProcessUpgradeFile(
//                                FileProcessType.NewInstall,
//                                aSqlCommand,
//                                sPath + so.Name + ".SQL",
//                                "InstallFile",
//                                aMessageQueue,
//                                aProcessedQueue,
//                                false,
//                                aIgnoreErrors,
//                                majorVersion,
//                                minorVersion,
//                                revision,
//                                modification,
//                                dbDate,
//                                aDatabaseType,
//                                aNoDataTables,
//                                aAllocationFileGroup,
//                                aForecastFileGroup,
//                                aHistoryFileGroup,
//                                aNoHistoryFileGroup,
//                                aDailyHistoryFileGroup,
//                                aNoDailyHistoryFileGroup,
//                                aAuditFileGroup,
//                                aWeekArchiveFileGroup,
//                                aDayArchiveFileGroup,
//                                false);

//                                _htProcessed.Add(so.Name, null);
//                            }
//                        }
//                    }

//                    if (aPhase == DatabaseObjectsSQLObjectPhase.PreGenerate)
//                    {
//                        DirectoryInfo di = new DirectoryInfo(sPath);
//                        foreach (FileInfo fi in di.GetFiles("*.SQL"))
//                        {
//                            // skip primary keys
//                            if (fi.Name.Contains("_PK.SQL"))
//                            {
//                                continue;
//                            }
//                            string constraintName = fi.Name.Replace(".SQL", string.Empty);

//                            if (_htProcessed.ContainsKey(constraintName) ||
//                                (aUpgradeProcess && isConstraintFound(aSqlCommand, constraintName)))
//                            {
//                                continue;
//                            }

//                            ProcessUpgradeFile(
//                                FileProcessType.NewInstall,
//                                aSqlCommand,
//                                fi.FullName,
//                                "InstallFile",
//                                aMessageQueue,
//                                aProcessedQueue,
//                                false,
//                                aIgnoreErrors,
//                                majorVersion,
//                                minorVersion,
//                                revision,
//                                modification,
//                                dbDate,
//                                aDatabaseType,
//                                aNoDataTables,
//                                aAllocationFileGroup,
//                                aForecastFileGroup,
//                                aHistoryFileGroup,
//                                aNoHistoryFileGroup,
//                                aDailyHistoryFileGroup,
//                                aNoDailyHistoryFileGroup,
//                                aAuditFileGroup,
//                                aWeekArchiveFileGroup,
//                                aDayArchiveFileGroup,
//                                false);
//                        }
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                aMessageQueue.Enqueue("UNEXPECTED EXCEPTION: " + ex.ToString());
//                throw;
//            }
//        }

         //static private bool VerifyPrerequisites(SqlCommand aSqlCommand, DatabaseObjectsSQLObject so, out string aPrerequisite)
        //{
        //    bool prerequisiteFound = true;
        //    aPrerequisite = string.Empty;

        //    foreach (DatabaseObjectsSQLObjectPrerequisite prerequisite in so.Prerequisite)
        //    {
        //        switch (prerequisite.Type)
        //        {
        //            case DatabaseObjectsSQLObjectPrerequisiteType.Constraint:
        //                if (!isConstraintFound(aSqlCommand, prerequisite.Name))
        //                {
        //                    aPrerequisite += prerequisite.Name;
        //                    prerequisiteFound = false;
        //                }
        //                break;
        //            case DatabaseObjectsSQLObjectPrerequisiteType.FunctionScalar:
        //                if (!isScalarFunctionFound(aSqlCommand, prerequisite.Name))
        //                {
        //                    aPrerequisite += prerequisite.Name;
        //                    prerequisiteFound = false;
        //                }
        //                break;
        //            case DatabaseObjectsSQLObjectPrerequisiteType.FunctionTable:
        //                if (!isTableFunctionFound(aSqlCommand, prerequisite.Name))
        //                {
        //                    aPrerequisite += prerequisite.Name;
        //                    prerequisiteFound = false;
        //                }
        //                break;
        //            case DatabaseObjectsSQLObjectPrerequisiteType.Index:
        //                if (!isIndexFound(aSqlCommand, prerequisite.Name))
        //                {
        //                    aPrerequisite += prerequisite.Name;
        //                    prerequisiteFound = false;
        //                }
        //                break;
        //            case DatabaseObjectsSQLObjectPrerequisiteType.StoredProcedure:
        //                if (!isStoredProcedureFound(aSqlCommand, prerequisite.Name))
        //                {
        //                    aPrerequisite += prerequisite.Name;
        //                    prerequisiteFound = false;
        //                }
        //                break;
        //            case DatabaseObjectsSQLObjectPrerequisiteType.Table:
        //                if (!isTableFound(aSqlCommand, prerequisite.Name))
        //                {
        //                    aPrerequisite += prerequisite.Name;
        //                    prerequisiteFound = false;
        //                }
        //                break;
        //            case DatabaseObjectsSQLObjectPrerequisiteType.Trigger:
        //                if (!isTriggerFound(aSqlCommand, prerequisite.Name))
        //                {
        //                    aPrerequisite += prerequisite.Name;
        //                    prerequisiteFound = false;
        //                }
        //                break;
        //            case DatabaseObjectsSQLObjectPrerequisiteType.Type:
        //                if (!isTypeFound(aSqlCommand, prerequisite.Name))
        //                {
        //                    aPrerequisite += prerequisite.Name;
        //                    prerequisiteFound = false;
        //                }
        //                break;
        //            case DatabaseObjectsSQLObjectPrerequisiteType.View:
        //                if (!isViewFound(aSqlCommand, prerequisite.Name))
        //                {
        //                    aPrerequisite += prerequisite.Name;
        //                    prerequisiteFound = false;
        //                }
        //                break;
        //        }

        //        if (!prerequisiteFound)
        //        {
        //            aPrerequisite += ";";
        //        }
        //    }

        //    return prerequisiteFound;
        //}

         //static private void ProcessCommand(Queue aMessageQueue, Queue aProcessedQueue, ref string aCurrentHeader, string aFileName, int aLineCount, SqlCommand aSqlCommand, string aCommand,
        //    int aMajorVersion, int aMinorVersion, int aRevision, int aModification, bool aSettingNewInstallVersionNumbers, bool aFinalize, bool aIgnoreErrors)
        //{
        //    try
        //    {
        //        try
        //        {
        //            if (aCommand.Trim().Length > 0 &&
        //                !aSettingNewInstallVersionNumbers)
        //            {
        //                aSqlCommand.CommandText = aCommand;
        //                aSqlCommand.ExecuteNonQuery();
        //            }

        //            if (aCurrentHeader != string.Empty)
        //            {
        //                aProcessedQueue.Enqueue(aCurrentHeader);
        //                aCurrentHeader = string.Empty;
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            aMessageQueue.Enqueue("Error processing SQL command below in file name " + aFileName + " near line " + aLineCount + ": " + ex.ToString());
        //            aMessageQueue.Enqueue("Command causing error:");
        //            aMessageQueue.Enqueue(aCommand);
        //            throw;
        //        }

        //        try
        //        {
        //            if (aMajorVersion < int.MaxValue ||
        //                aSettingNewInstallVersionNumbers)
        //            {
        //                UpdateApplicationVersion(aMajorVersion, aMinorVersion, aRevision, aModification);
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            aMessageQueue.Enqueue("Error updating version in file name " + aFileName + " near line " + aLineCount + ": " + ex.ToString());
        //            throw;
        //        }

        //        try
        //        {
        //            if (aFinalize)
        //            {
        //                if (aMessageQueue.Count > 0 && !aIgnoreErrors)
        //                {
        //                    aSqlCommand.Transaction.Rollback();
        //                }
        //                else
        //                {
        //                    aSqlCommand.Transaction.Commit();
        //                }

        //                aSqlCommand.Transaction = aSqlCommand.Connection.BeginTransaction();
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            aMessageQueue.Enqueue("Error during commit/rollback after command below in file name " + aFileName + " near line " + aLineCount + ": " + ex.ToString());
        //            aMessageQueue.Enqueue("Command causing error:");
        //            aMessageQueue.Enqueue(aCommand);
        //            throw;
        //        }
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}

            //static private void ProcessUpgradeFile(
        //    FileProcessType aProcessType,
        //    SqlCommand aSqlCommand,
        //    string aFileName,
        //    string aFileKey,
        //    Queue aMessageQueue,
        //    Queue aProcessedQueue,
        //    bool aUpgradeProcess,
        //    bool aIgnoreErrors,
        //    int aDBMajorVersion,
        //    int aDBMinorVersion,
        //    int aDBRevision,
        //    int aDBModification,
        //    DateTime aDBDate,
        //    eDatabaseType aDatabaseType, 
        //    int aNoDataTables, 
        //    string aAllocationFileGroup, 
        //    string aForecastFileGroup, 
        //    string aHistoryFileGroup, 
        //    int aNoHistoryFileGroup, 
        //    string aDailyHistoryFileGroup, 
        //    int aNoDailyHistoryFileGroup,
        //    string aAuditFileGroup,        
        //    string aWeekArchiveFileGroup,  
        //    string aDayArchiveFileGroup,   
        //    bool bGettingProcessCount)
        //{
        //    bool processCommand = false;
        //    string command = string.Empty;
        //    StreamReader reader = null;
        //    string line = null;
        //    int lineCount;
        //    string version = string.Empty;
        //    string executeClass;
        //    //ICustomConversion custConv;
        //    DateTime sqlDate = DateTime.MinValue;
        //    int sqlMajorVersion = 0;
        //    int sqlMinorVersion = 0;
        //    int sqlRevision = 0;
        //    int sqlModification = 0;
        //    bool versionError;
        //    bool SettingNewInstallVersionNumbers = false;
        //    string[] fields;
        //    bool priorCutoffReleaseCommand = false;
        //    string currentHeader = string.Empty;
        //    bool saveAllUpgradeEntries = false;

        //    try
        //    {
        //        if (bGettingProcessCount)
        //        {
        //            //enable progress controls
        //            ProgressBarEnabled(false);
        //            LbLStatusEnabled(false);
        //        }
        //        else
        //        {
        //            //enable progress controls
        //            LbLStatusEnabled(true);
        //            LbLStatusEnabled(true);
        //        }
        //        // create transaction

        //        try
        //        {
        //            aSqlCommand.Transaction = aSqlCommand.Connection.BeginTransaction();
        //        }
        //        catch (Exception ex)
        //        {
        //            string message = ex.ToString();
        //            aMessageQueue.Enqueue("FATAL DB Error: Error creating transaction");
        //            throw;
        //        }

        //        if (!aUpgradeProcess)
        //        {
        //            sqlMajorVersion = int.MaxValue;
        //        }

        //        if (aFileName == null)
        //        {
        //            aMessageQueue.Enqueue("FATAL Configuration Error: " + aFileKey + " parameter not found in configuration file");
        //            return;
        //        }
        //        else if (!File.Exists(aFileName))
        //        {
        //            aMessageQueue.Enqueue("FATAL Configuration Error: " + aFileKey + " file not found");
        //            return;
        //        }

        //        if (aProcessType == FileProcessType.NewInstall &&
        //            aFileKey == "UpgradeFile")
        //        {
        //            SettingNewInstallVersionNumbers = true;
        //        }

        //        //process schema changes
        //        reader = new StreamReader(aFileName);
        //        lineCount = 0;

        //        string lineWithWhitespace; //TT#730-MD -jsobek -Preserve horizontal tabs and whitespace during database upgrade
					
        //        // process script
        //        while ((line = reader.ReadLine()) != null)
        //        {
        //            // process all command if not the upgrade process
        //            lineCount++;

        //            if (!aUpgradeProcess &&
        //               !SettingNewInstallVersionNumbers &&
        //               aProcessType != FileProcessType.ProcedureReload)
        //            {
        //                processCommand = true;
        //            }
        //            //Begin TT#730-MD -jsobek -Preserve horizontal tabs and whitespace during database upgrade
        //            lineWithWhitespace = line;
        //            //Remove sql developer comments unless we are upgrading the database in debug mode
        //            //sql developer comments should begin with --DV
        //            #if (DEBUG)
                        
        //            #else
        //                if (line.Trim().ToUpper().StartsWith("--DV") == true)
        //                {
        //                    continue;
        //                }
        //            #endif
        //            //End TT#730-MD -jsobek -Preserve horizontal tabs and whitespace during database upgrade
        //            line = line.Trim();
        //            if (line.Length == 0 ||
        //                line.StartsWith("/*COMMENT") ||
        //                line == "use master" ||
        //                line.ToLower().StartsWith("create database") ||
        //                line.ToLower().StartsWith(@"use "))
        //            {
        //                continue;
        //            }
        //            else if (line.ToUpper().StartsWith("/*RELEASE *.*.*") ||
        //                line.ToUpper().StartsWith("/*RELEASE *.*.*.*"))
        //            {
                        
        //                // process prior command
        //                if (processCommand)
        //                {
        //                    if (bGettingProcessCount)
        //                    {
        //                        ++_processCount;
        //                    }
        //                    else
        //                    {
        //                        msgDelegate.Invoke("Processing:" + currentHeader);
                               
        //                        ProcessCommand(aMessageQueue, aProcessedQueue, ref currentHeader, aFileName, lineCount, aSqlCommand, command, sqlMajorVersion, sqlMinorVersion, sqlRevision, sqlModification, SettingNewInstallVersionNumbers, true, aIgnoreErrors);
        //                        ProgressBarIncrementValue(1);
        //                    }
        //                    processCommand = false;
        //                }
        //                command = string.Empty;

        //                currentHeader = line;

        //                if (aProcessType == FileProcessType.ReleaseUpdates)
        //                {
        //                    sqlMajorVersion = int.MaxValue;
        //                    sqlMinorVersion = int.MaxValue;
        //                    sqlRevision = int.MaxValue;
        //                    sqlModification = int.MaxValue;
        //                    processCommand = true;
        //                }
        //            }
        //            else if (line.ToUpper().StartsWith("/*VERSIONING"))
        //            {
        //                // process prior command
        //                if (processCommand &&
        //                    !bGettingProcessCount)
        //                {
        //                    ProcessCommand(aMessageQueue, aProcessedQueue, ref currentHeader, aFileName, lineCount, aSqlCommand, command, sqlMajorVersion, sqlMinorVersion, sqlRevision, sqlModification, SettingNewInstallVersionNumbers, true, aIgnoreErrors);
        //                    processCommand = false;
        //                }
        //                command = string.Empty;

        //                currentHeader = line;

        //                if (aProcessType == FileProcessType.VersioningUpdates)
        //                {
        //                    sqlMajorVersion = int.MaxValue;
        //                    sqlMinorVersion = int.MaxValue;
        //                    sqlRevision = int.MaxValue;
        //                    sqlModification = int.MaxValue;
        //                    processCommand = true;
        //                }
        //            }
        //            else if (line.ToUpper().StartsWith("/*EXECUTE"))
        //            {
        //                // process prior command
        //                if (processCommand &&
        //                    !SettingNewInstallVersionNumbers &&
        //                    !bGettingProcessCount)
        //                {
        //                    ProcessCommand(aMessageQueue, aProcessedQueue, ref currentHeader, aFileName, lineCount, aSqlCommand, command, sqlMajorVersion, sqlMinorVersion, sqlRevision, sqlModification, SettingNewInstallVersionNumbers, true, aIgnoreErrors);
                            
        //                    fields = line.Split(' ');

        //                    if (fields.Length == 3)
        //                    {
        //                        executeClass = fields[1];

        //                        try
        //                        {
        //                            //custConv = (ICustomConversion)System.Activator.CreateInstance(System.Type.GetType("MIDRetail.DatabaseUpdate." + executeClass));
        //                            //if (custConv != null)
        //                            //{
        //                            //    try
        //                            //    {
        //                            //        custConv.Execute(_connectionString, aMessageQueue, aProcessedQueue,
        //                            //            aDatabaseType, aNoDataTables, aAllocationFileGroup, 
        //                            //            aForecastFileGroup, aHistoryFileGroup, aNoHistoryFileGroup, 
        //                            //            aDailyHistoryFileGroup, aNoDailyHistoryFileGroup,
        //                            //            aAuditFileGroup, aWeekArchiveFileGroup, aDayArchiveFileGroup);
        //                            //    }
        //                            //    catch (Exception exc)
        //                            //    {
        //                            //        aMessageQueue.Enqueue("Line " + lineCount + ": Error encountered during execution of " + executeClass + ".Execute(): " + exc.Message);
        //                            //    }
        //                            //}
        //                            //else
        //                            //{
        //                                aMessageQueue.Enqueue("Line " + lineCount + ": Could not create class " + executeClass);
        //                            //}
        //                        }
        //                        catch (Exception exc)
        //                        {
        //                            aMessageQueue.Enqueue("Line " + lineCount + ": Error encountered during creation of " + executeClass + ": " + exc.Message);
        //                        }
        //                    }
        //                    else if (fields.Length == 4 &&
        //                        fields[1].ToUpper() == "MODULE")
        //                    {
        //                        try
        //                        {
        //                            if (!ProcessModule(fields[2], string.Empty))
        //                            {
        //                                aMessageQueue.Enqueue("Line " + lineCount + ": Error encountered during execution of " + fields[2] + ".Execute(). Check for logs. ");
        //                            }
        //                        }
        //                        catch (Exception exc)
        //                        {
        //                            aMessageQueue.Enqueue("Line " + lineCount + ": Error encountered during execution of " + fields[2] + ".Execute(): " + exc.Message);
        //                        }
        //                    }
        //                    else
        //                    {
        //                        aMessageQueue.Enqueue("Line " + lineCount + ": Execute Header is not valid");
        //                    }
        //                }

        //                command = string.Empty;
        //            }
        //            else if (line.ToUpper().StartsWith("/*RELEASE"))
        //            {
        //                // process prior command
        //                if (processCommand)
        //                {
        //                    if (bGettingProcessCount)
        //                    {
        //                        ++_processCount;
        //                    }
        //                    else
        //                    {
        //                        msgDelegate.Invoke("Processing:" + line);
        //                        ProcessCommand(aMessageQueue, aProcessedQueue, ref currentHeader, aFileName, lineCount, aSqlCommand, command, sqlMajorVersion, sqlMinorVersion, sqlRevision, sqlModification, SettingNewInstallVersionNumbers, true, aIgnoreErrors);
        //                        ProgressBarIncrementValue(1);
        //                    }
        //                    processCommand = false;
        //                }
        //                command = string.Empty;

        //                currentHeader = line;

        //                if (aProcessType != FileProcessType.VersioningUpdates)
        //                {
        //                    versionError = false;
        //                    fields = line.Split(' ');

        //                    if (fields.Length >= 3)
        //                    {
        //                        versionError = ParseVersion(lineCount, aMessageQueue, fields[1], fields[2], out sqlMajorVersion, out sqlMinorVersion, out sqlRevision, out sqlModification, out sqlDate, out priorCutoffReleaseCommand, out saveAllUpgradeEntries);
        //                    }
        //                    else
        //                    {
        //                        aMessageQueue.Enqueue("Line " + lineCount + ": Release Header is not valid");
        //                        versionError = true;
        //                    }

        //                    if (!versionError)
        //                    {
        //                        // determine if command is to be processed
        //                        if (saveAllUpgradeEntries)
        //                        {
        //                            if (!UpgradeApplied(sqlMajorVersion, sqlMinorVersion, sqlRevision, sqlModification))
        //                            {
        //                                processCommand = true;
        //                            }
        //                        }
        //                        else
        //                        if (sqlMajorVersion > aDBMajorVersion)
        //                        {
        //                            processCommand = true;
        //                        }
        //                        else if (sqlMajorVersion == aDBMajorVersion)
        //                        {
        //                            if (sqlMinorVersion > aDBMinorVersion)
        //                            {
        //                                processCommand = true;
        //                            }
        //                            else if (sqlMinorVersion == aDBMinorVersion)
        //                            {
        //                                if (sqlRevision > aDBRevision)
        //                                {
        //                                    processCommand = true;
        //                                }
        //                                else if (sqlRevision == aDBRevision)
        //                                {
        //                                    if (priorCutoffReleaseCommand)
        //                                    {
        //                                        if (sqlDate > aDBDate)
        //                                        {
        //                                            processCommand = true;
        //                                        }
        //                                    }
        //                                    else
        //                                    {
        //                                        if (sqlModification > aDBModification)
        //                                        {
        //                                            processCommand = true;
        //                                        }
        //                                    }
        //                                }
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //            else if (line.ToUpper() == "GO")
        //            {
        //                if (processCommand &&
        //                    !bGettingProcessCount)
        //                {
        //                    ProcessCommand(aMessageQueue, aProcessedQueue, ref currentHeader, aFileName, lineCount, aSqlCommand, command, sqlMajorVersion, sqlMinorVersion, sqlRevision, sqlModification, SettingNewInstallVersionNumbers, false, aIgnoreErrors);
        //                }
        //                command = string.Empty;
        //            }
        //            else if (line.ToUpper() == "/*PROCEDURES/FUCTIONS/VIEWS/TRIGGERS SECTION*/")
        //            {
        //                if (aProcessType == FileProcessType.ProcedureReload)
        //                {
        //                    processCommand = true;
        //                }
        //            }
        //            else if (processCommand)
        //            {
        //                if (aProcessType == FileProcessType.NewInstall)
        //                {
        //                    if (line.IndexOf("ON 'ALLOCATION'") > -1)
        //                    {
        //                        lineWithWhitespace = lineWithWhitespace.Replace("ON 'ALLOCATION'", "ON '" + aAllocationFileGroup + "'");
        //                    }
        //                    else if (line.IndexOf("ON [ALLOCATION]") > -1)
        //                    {
        //                        lineWithWhitespace = lineWithWhitespace.Replace("ON [ALLOCATION]", "ON [" + aAllocationFileGroup + "]");
        //                    }
        //                    else if (line.IndexOf("exec SP_MID_ST_TABLE_COUNT_UPDT") > -1)
        //                    {
        //                        lineWithWhitespace = lineWithWhitespace.Replace("10", aNoDataTables.ToString());
        //                    }
        //                    else if (line.IndexOf("ON 'AUDIT'") > -1)
        //                    {
        //                        lineWithWhitespace = lineWithWhitespace.Replace("ON 'AUDIT'", "ON '" + aAuditFileGroup + "'");
        //                    }
        //                    else if (line.IndexOf("ON [AUDIT]") > -1)
        //                    {
        //                        lineWithWhitespace = lineWithWhitespace.Replace("ON [AUDIT]", "ON [" + aAuditFileGroup + "]");
        //                    }
        //                    else if (line.IndexOf("ON 'WEEKARCHIVE'") > -1)
        //                    {
        //                        lineWithWhitespace = lineWithWhitespace.Replace("ON 'WEEKARCHIVE'", "ON '" + aWeekArchiveFileGroup + "'");
        //                    }
        //                    else if (line.IndexOf("ON [WEEKARCHIVE]") > -1)
        //                    {
        //                        lineWithWhitespace = lineWithWhitespace.Replace("ON [WEEKARCHIVE]", "ON [" + aWeekArchiveFileGroup + "]");
        //                    }
        //                    else if (line.IndexOf("ON 'DAYARCHIVE'") > -1)
        //                    { 
        //                        lineWithWhitespace = lineWithWhitespace.Replace("ON 'DAYARCHIVE'", "ON '" + aDayArchiveFileGroup + "'");
        //                    }
        //                    else if (line.IndexOf("ON [DAYARCHIVE]") > -1)
        //                    {
        //                        lineWithWhitespace = lineWithWhitespace.Replace("ON [DAYARCHIVE]", "ON [" + aDayArchiveFileGroup + "]");
        //                    }
        //                }
        //                else
        //                {
        //                    if (line.IndexOf("ON 'ALLOCATION'") > -1)
        //                    {
        //                        lineWithWhitespace = lineWithWhitespace.Replace("ON 'ALLOCATION'", "ON '" + aAllocationFileGroup + "'");
        //                    }
        //                    else if (line.IndexOf("ON [ALLOCATION]") > -1)
        //                    {
        //                        lineWithWhitespace = lineWithWhitespace.Replace("ON [ALLOCATION]", "ON [" + aAllocationFileGroup + "]");
        //                    }
        //                    else if (line.IndexOf("ON 'AUDIT'") > -1)
        //                    {
        //                        lineWithWhitespace = lineWithWhitespace.Replace("ON 'AUDIT'", "ON '" + aAuditFileGroup + "'");
        //                    }
        //                    else if (line.IndexOf("ON [AUDIT]") > -1)
        //                    {
        //                        lineWithWhitespace = lineWithWhitespace.Replace("ON [AUDIT]", "ON [" + aAuditFileGroup + "]");
        //                    }
        //                    else if (line.IndexOf("ON 'WEEKARCHIVE'") > -1)
        //                    {
        //                        lineWithWhitespace = lineWithWhitespace.Replace("ON 'WEEKARCHIVE'", "ON '" + aWeekArchiveFileGroup + "'");
        //                    }
        //                    else if (line.IndexOf("ON [WEEKARCHIVE]") > -1)
        //                    {
        //                        lineWithWhitespace = lineWithWhitespace.Replace("ON [WEEKARCHIVE]", "ON [" + aWeekArchiveFileGroup + "]");
        //                    }
        //                    else if (line.IndexOf("ON 'DAYARCHIVE'") > -1)
        //                    {
        //                        lineWithWhitespace = lineWithWhitespace.Replace("ON 'DAYARCHIVE'", "ON '" + aDayArchiveFileGroup + "'");
        //                    }
        //                    else if (line.IndexOf("ON [DAYARCHIVE]") > -1)
        //                    {
        //                        lineWithWhitespace = lineWithWhitespace.Replace("ON [DAYARCHIVE]", "ON [" + aDayArchiveFileGroup + "]");
        //                    }
        //                }
        //                command += lineWithWhitespace + System.Environment.NewLine;
        //            }
        //        }
				
        //        // process command if anything in buffer
        //        if (processCommand &&
        //                    !bGettingProcessCount)
        //        {
        //            ProcessCommand(aMessageQueue, aProcessedQueue, ref currentHeader, aFileName, lineCount, aSqlCommand, command, sqlMajorVersion, sqlMinorVersion, sqlRevision, sqlModification, SettingNewInstallVersionNumbers, true, aIgnoreErrors);
        //            processCommand = false;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        string message = ex.Message;
        //        throw;
        //    }
        //    finally
        //    {
        //        if (aSqlCommand.Transaction != null)
        //        {
        //            if (aMessageQueue.Count > 0 && !aIgnoreErrors)
        //            {
        //                aSqlCommand.Transaction.Rollback();
        //            }
        //            else
        //            {
        //                aSqlCommand.Transaction.Commit();
        //            }
        //        }

        //        if (reader != null)
        //        {
        //            reader.Close();
        //        }
        //    }
        //}

//        static private bool ProcessModule(string aModule, string aArguments)
//        {
//            Process process = new Process();

//#if (DEBUG)
//            process.StartInfo.FileName = Directory.GetParent(Application.StartupPath).ToString().Trim() + @"\Database\" + aModule;
//#else
//            process.StartInfo.FileName = Directory.GetParent(Application.StartupPath).ToString().Trim() + @"\Database\" + aModule;
//#endif
//            process.StartInfo.Arguments = aArguments;
//            process.StartInfo.CreateNoWindow = true;
//            process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
//            process.Start();
//            process.WaitForExit();
//            if (process.ExitCode > 0)
//            {
//                return false;
//            }
//            return true;
//        }
        // End TT#449-MD - JSmith - Modify upgrade process to be able to call external program

        //static private bool ParseVersion(
        //    int aLineCount,
        //    Queue aMessageQueue,
        //    string aNodeField,
        //    string aDateField,
        //    out int aMajorRelease,
        //    out int aMinorRelease,
        //    out int aRevision,
        //    out int aModification,
        //    out DateTime aDate, 
        //    out bool aPriorCutoffReleaseCommand,
        //    out bool aSaveAllUpgradeEntries)
        //{
        //    string[] nodes;

        //    try
        //    {
        //        aMajorRelease = 0;
        //        aMinorRelease = 0;
        //        aRevision = 0;
        //        aModification = 0;
        //        aDate = DateTime.MaxValue;
        //        aPriorCutoffReleaseCommand = false;
        //        aSaveAllUpgradeEntries = false;

        //        nodes = aNodeField.Split('.');

        //        if (nodes.Length < 1 || nodes.Length > 4)
        //        {
        //            aMessageQueue.Enqueue("Line " + aLineCount + ": Version number " + aNodeField + " is not valid");
        //            return true;
        //        }

        //        try
        //        {
        //            aMajorRelease = Convert.ToInt32(nodes[0]);
        //        }
        //        catch
        //        {
        //            aMessageQueue.Enqueue("Line " + aLineCount + ": Major version " + nodes[0] + " is not valid");
        //            return true;
        //        }
	
        //        if (nodes.Length > 1)
        //        {
        //            try
        //            {
        //                aMinorRelease = Convert.ToInt32(nodes[1]);
        //            }
        //            catch
        //            {
        //                aMessageQueue.Enqueue("Line " + aLineCount + ": Minor version " + nodes[1] + " is not valid");
        //                return true;
        //            }
        //        }
        //        else
        //        {
        //            aMessageQueue.Enqueue("Line " + aLineCount + ": Minor version not specified");
        //            return true;
        //        }

        //        if (nodes.Length > 2)
        //        {
        //            try
        //            {
        //                aRevision = Convert.ToInt32(nodes[2]);
        //            }
        //            catch
        //            {
        //                aMessageQueue.Enqueue("Line " + aLineCount + ": Revision " + nodes[2] + " is not valid");
        //                return true;
        //            }
        //        }
        //        else
        //        {
        //            aMessageQueue.Enqueue("Line " + aLineCount + ": Release not specified");
        //            return true;
        //        }

        //        aPriorCutoffReleaseCommand = aMajorRelease < cCutoffReleaseMajor || (aMajorRelease == cCutoffReleaseMajor && aMinorRelease < cCutoffReleaseMinor) || (aMajorRelease == cCutoffReleaseMajor && aMinorRelease == cCutoffReleaseMinor && aRevision < cCutoffReleaseRevision);

        //        if (nodes.Length > 3)
        //        {
        //            if (!aPriorCutoffReleaseCommand)
        //            {
        //                try
        //                {
        //                    aModification = Convert.ToInt32(nodes[3]);
        //                }
        //                catch
        //                {
        //                    aMessageQueue.Enqueue("Line " + aLineCount + ": Modification " + nodes[3] + " is not valid");
        //                    return true;
        //                }
        //            }
        //            else
        //            {
        //                aMessageQueue.Enqueue("Line " + aLineCount + ": Modification is not valid for Release " + cCutoffReleaseMajor + "." + cCutoffReleaseMinor + "." + (cCutoffReleaseRevision - 1) + " and prior");
        //                return true;
        //            }
        //        }
        //        else
        //        {
        //            if (aPriorCutoffReleaseCommand)
        //            {
        //                aModification = 0;
        //            }
        //            else
        //            {
        //                aMessageQueue.Enqueue("Line " + aLineCount + ": Modification is required for Release " + cCutoffReleaseMajor + "." + cCutoffReleaseMinor + "." + cCutoffReleaseRevision +" and later");
        //                return true;
        //            }
        //        }

        //        //determine date
        //        try
        //        {
        //            aDate = Convert.ToDateTime(aDateField);
        //        }
        //        catch
        //        {
        //            aMessageQueue.Enqueue("Line " + aLineCount + ": Date " + aDateField + " is not valid");
        //            return true;
        //        }

        //        aSaveAllUpgradeEntries = true;

        //        return false;
        //    }
        //    catch (Exception ex)
        //    {
        //        string message = ex.Message;
        //        throw;
        //    }
        //}
        #endregion

	}
}
