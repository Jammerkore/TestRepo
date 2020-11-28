using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceProcess;
using System.Collections;
using System.Threading;
using System.Xml;
using System.Xml.Serialization;
using System.Windows.Forms;

using MIDRetail.Common;
using MIDRetail.Data;
using MIDRetail.DataCommon;

using log4net;
using log4net.Config;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]

namespace MIDRetail.HierarchyLevelMaintenance
{
    class HierarchyLevelMaintenance
    {
        static int Main(string[] args)
        {
            log4net.ILog log = log4net.LogManager.GetLogger("Activity");

            HierarchyLevelMaintenanceWorker hlm = new HierarchyLevelMaintenanceWorker(log);
            return Convert.ToInt32(hlm.Process(args));
        }

        public class HierarchyLevelMaintenanceWorker
        {
            log4net.ILog _log;
            int _recordsRead = 0;
            int _levelsAdded = 0;
            int _levelsRemoved = 0;
            int _recordsWithErrors = 0;
            bool _servicesAreDefined = true;

            public HierarchyLevelMaintenanceWorker(log4net.ILog log)
			{
				_log = log;
                MIDConnectionString.ConnectionString = MIDConfigurationManager.AppSettings["ConnectionString"];
			}

            public eMIDMessageLevel Process(string[] args)
            {
                eMIDMessageLevel rc = eMIDMessageLevel.None;

                if (ReadyForProcess())
                {
                    rc = ProcessTransactions(args);
                    LogInfo(" ");
                    LogInfo(MIDText.GetTextOnly(eMIDTextCode.msg_TransactionsProcessed) + " " + _recordsRead);
                    LogInfo("   " + MIDText.GetTextOnly(eMIDTextCode.msg_ItemsAdded, "Levels") + " " + _levelsAdded);
                    LogInfo("   " + MIDText.GetTextOnly(eMIDTextCode.msg_ItemsRemoved, "Levels") + " " + _levelsRemoved);
                    LogInfo("   " + MIDText.GetTextOnly(eMIDTextCode.msg_ItemsWithErrors) + " " + _recordsWithErrors);
                    LogInfo(" ");
                    if (rc == eMIDMessageLevel.None &&
                        _levelsAdded > 0)
                    {
                        ProcessHierarchyTransactions();
                    }
                    if (rc == eMIDMessageLevel.None)
                    {
                        LogInfo(MIDText.GetText(eMIDTextCode.msg_ProcessCompletedSuccessfully));
                    }
                    else
                    {
                        LogError(MIDText.GetText(eMIDTextCode.msg_ProcessCompletedWithErrors));
                    }
                }
                else
                {
                    LogError(MIDText.GetText(eMIDTextCode.msg_ProcessDidNotStart));
                    rc = eMIDMessageLevel.Error;
                }
                return rc;
            }

            private bool ReadyForProcess()
            {
                string msg = string.Empty;
                bool ready = true;
                
                try
                {
                    if (!AreServicesStopped())
                    {
                        if (_servicesAreDefined)
                        {
                            msg = MIDText.GetText(eMIDTextCode.msg_StoreDeleteServicesMustBeDown);
                            LogError(msg);
                        }
                        else
                        {
                            msg = MIDText.GetText(eMIDTextCode.msg_MustBeApplicationServer);
                            LogError(msg);
                        }
                        ready = false;
                    }
                }
                catch (Exception ex)
                {
                    ready = false;
                }
                
                return ready;
            }

            public eMIDMessageLevel ProcessTransactions(string[] args)
            {
                string fileLocation = null;
                char[] delimiter = { '~' };
                string message;

                try
                {
                    if (args.Length > 0)
                    {
                        fileLocation = args[0];
                        if (args.Length > 1)
                        {
                            delimiter = args[1].ToCharArray();
                        }
                        else
                        {
                            delimiter = MIDConfigurationManager.AppSettings["Delimiter"].ToCharArray();
                        }
                    }
                    else
                    {
                        fileLocation = MIDConfigurationManager.AppSettings["InputFile"];
                        string strDelimiter = MIDConfigurationManager.AppSettings["Delimiter"];
                        if (strDelimiter != null)
                        {
                            delimiter = strDelimiter.ToCharArray();
                        }
                    }

                    message = MIDText.GetText(eMIDTextCode.msg_BatchInputFile);

                    if (fileLocation == "" || fileLocation == null)
                    {
                        message = MIDText.GetText(eMIDTextCode.msg_InputFileNotSpecified, fileLocation) + System.Environment.NewLine;
                        message += MIDText.GetTextOnly(eMIDTextCode.msg_ProcessNotRun, "Hierarchy Level Maintenance");

                        LogError(message);

                        return eMIDMessageLevel.Error;
                    }
                    else
                    {
                        if (!File.Exists(fileLocation))
                        {
                            message = MIDText.GetText(eMIDTextCode.msg_FileNotFound, fileLocation) + System.Environment.NewLine;
                            message += MIDText.GetTextOnly(eMIDTextCode.msg_ProcessNotRun, "Hierarchy Level Maintenance");

                            LogError(message);

                            return eMIDMessageLevel.Error;
                        }
                        else
                        {
                            FileInfo txnFileInfo = new FileInfo(fileLocation);

                            if (txnFileInfo.Length == 0)
                            {
                                message = MIDText.GetText(eMIDTextCode.msg_EmptyFile, fileLocation) + System.Environment.NewLine;
                                message += MIDText.GetTextOnly(eMIDTextCode.msg_ProcessNotRun, "Hierarchy Level Maintenance");

                                LogError(message);

                                return eMIDMessageLevel.Error;
                            }
                            else
                            {
                                message = message.Replace("{0}", "[" + fileLocation + "]");

                                LogInfo(message);

                                MerchandiseHierarchyData mhd = new MerchandiseHierarchyData();
                                DataTable dtHierarchies = mhd.Hierarchy_Read();
                                int organizationalPhRID = mhd.Hierarchy_Read_Organizational_RID();
                                DataTable dtOrgHierarchyLevels = mhd.HierarchyLevels_Read(organizationalPhRID);

                                if (fileLocation.Substring(fileLocation.Length - 4).ToUpper() == ".XML")
                                {
                                    return LoadXMLTransFile(fileLocation, dtHierarchies, organizationalPhRID, dtOrgHierarchyLevels);
                                }
                                else
                                {
                                    return LoadDelimitedTransFile(fileLocation, delimiter, dtHierarchies, organizationalPhRID, dtOrgHierarchyLevels);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogError(ex.ToString());
                    return eMIDMessageLevel.Error;
                }

            }

            private bool AreServicesStopped()
            {
                try
                {
                    string appControlServer = MIDConfigurationManager.AppSettings["ControlServiceNames"];
                    string appStoreServer = MIDConfigurationManager.AppSettings["StoreServiceNames"];
                    string appHierarchyServer = MIDConfigurationManager.AppSettings["MerchandiseServiceNames"];
                    string appApplicationServer = MIDConfigurationManager.AppSettings["ApplicationServiceNames"];
                    string appSchedulerServer = MIDConfigurationManager.AppSettings["SchedulerServiceNames"];

                    bool controlServerStopped = IsServiceStopped(appControlServer);
                    bool storeServerStopped = IsServiceStopped(appStoreServer);
                    bool hierarchyServerStopped = IsServiceStopped(appHierarchyServer);
                    bool schedulerServerStopped = IsServiceStopped(appSchedulerServer);
                    bool applicationServerStopped = IsServiceStopped(appApplicationServer);

                    return (controlServerStopped && storeServerStopped && hierarchyServerStopped && schedulerServerStopped && applicationServerStopped);
                }
                catch (Exception exc)
                {
                    LogError(exc.ToString());
                    return false;
                }
            }

            private bool IsServiceStopped(string serviceName)
            {
                bool isStopped = false;
                // return is stopped if no name for service
                if (serviceName == null || serviceName.Trim().Length == 0)
                {
                    return true;
                }

                System.ServiceProcess.ServiceController sc = new System.ServiceProcess.ServiceController(serviceName);

                try
                {
                    if (sc.Status == ServiceControllerStatus.Stopped)
                    {
                        isStopped = true;
                    }
                }
                catch (System.InvalidOperationException ex)
                {
                    LogError(MIDText.GetText(eMIDTextCode.msg_ServiceNotFound, serviceName));
                    isStopped = false;
                    _servicesAreDefined = false;
                }
                catch (Exception exc)
                {
                    LogError(exc.ToString());
                }

                return isStopped;
            }

            public eMIDMessageLevel LoadXMLTransFile(string fileLocation, DataTable dtHierarchies, int organizationalPhRID, DataTable dtOrgHierarchyLevels)
            {
                HierarchyLevelMaint hlm = null;
                TextReader r = null;
                eMIDMessageLevel returnCode = eMIDMessageLevel.None;

                try
                {
                    XmlSerializer s = new XmlSerializer(typeof(HierarchyLevelMaint));	// Create a Serializer
                    r = new StreamReader(fileLocation);			// Load the Xml File
                    hlm = (HierarchyLevelMaint)s.Deserialize(r);						// Deserialize the Xml File to a strongly typed object
                }
                catch (Exception ex)
                {
                    LogError(MIDText.GetText(eMIDTextCode.msg_FileSerializationError, fileLocation));
                    LogError(ex.ToString());
                    return eMIDMessageLevel.Error;
                }

                finally
                {
                    if (r != null)
                        r.Close();
                }

                try
                {
                    // Validate all transactions before processing
                    returnCode = ProcessXMLTransFile(hlm, dtHierarchies, organizationalPhRID, dtOrgHierarchyLevels, true);

                    if (returnCode == eMIDMessageLevel.None)
                    {
                        returnCode = ProcessXMLTransFile(hlm, dtHierarchies, organizationalPhRID, dtOrgHierarchyLevels, false);
                    }
                }
                catch (Exception ex)
                {
                    LogError(MIDText.GetText(eMIDTextCode.msg_FileProcessingError, fileLocation));
                    LogError(ex.ToString());
                    returnCode = eMIDMessageLevel.Error;
                }

                return returnCode;
            }

            public eMIDMessageLevel ProcessXMLTransFile(HierarchyLevelMaint hlm, DataTable dtHierarchies, int organizationalPhRID, DataTable dtOrgHierarchyLevels, bool blValidationStage)
            {
                eMIDMessageLevel returnCode = eMIDMessageLevel.None;

                try
                {
                     foreach (HierarchyLevelMaintHierarchy hlmh in hlm.Hierarchy)
                    {
                        if (blValidationStage)
                        {
                            LogInfo(MIDText.GetText(eMIDTextCode.msg_Transaction, hlmh.ID + "," + hlmh.Action + "," + hlmh.LevelName + "," + hlmh.NewLevel));
                            ++_recordsRead;
                        }
                        if (hlmh.Action == HierarchyLevelMaintHierarchyAction.Add)
                        {
                            if (AddLevel(hlmh.ID, hlmh.LevelName, hlmh.NewLevel, dtHierarchies, organizationalPhRID, dtOrgHierarchyLevels, blValidationStage) == eMIDMessageLevel.None)
                            {
                                if (!blValidationStage)
                                {
                                    ++_levelsAdded;
                                }
                            }
                            else
                            {
                                ++_recordsWithErrors;
                                returnCode = eMIDMessageLevel.Error;
                            }
                        }
                        else if (hlmh.Action == HierarchyLevelMaintHierarchyAction.Remove)
                        {
                            if (RemoveLevel(hlmh.ID, hlmh.LevelName, dtHierarchies, organizationalPhRID, dtOrgHierarchyLevels, blValidationStage) == eMIDMessageLevel.None)
                            {
                                if (!blValidationStage)
                                {
                                    ++_levelsRemoved;
                                }
                            }
                            else
                            {
                                ++_recordsWithErrors;
                                returnCode = eMIDMessageLevel.Error;
                            }
                        }
                        else
                        {
                            ++_recordsWithErrors;
                        }
                    }
                }
                catch
                {
                    throw;
                }

                return returnCode;
            }

            public eMIDMessageLevel LoadDelimitedTransFile(string fileLocation, char[] delimiter, DataTable dtHierarchies, int organizationalPhRID, DataTable dtOrgHierarchyLevels)
            {
                // Validate all transactions before processing
                eMIDMessageLevel returnCode = ProcessDelimitedTransFile(fileLocation, delimiter, dtHierarchies, organizationalPhRID, dtOrgHierarchyLevels, true);

                if (returnCode == eMIDMessageLevel.None)
                {
                    returnCode = ProcessDelimitedTransFile(fileLocation, delimiter, dtHierarchies, organizationalPhRID, dtOrgHierarchyLevels, false);
                }

                return returnCode;
            }

            public eMIDMessageLevel ProcessDelimitedTransFile(string fileLocation, char[] delimiter, DataTable dtHierarchies, int organizationalPhRID, DataTable dtOrgHierarchyLevels, bool blValidationStage)
            {
                StreamReader reader = null;
                StreamWriter exceptionFile = null;
                string line = null;
                string message = null;
                eMIDMessageLevel returnCode = eMIDMessageLevel.None;

                try
                {
                    reader = new StreamReader(fileLocation);  //opens the file

                    while ((line = reader.ReadLine()) != null)
                    {
                        string[] fields = MIDstringTools.Split(line, delimiter[0], true);
                        if (fields.Length == 1 && (fields[0] == null || fields[0] == ""))  // skip blank line
                        {
                            continue;
                        }
                        if (blValidationStage)
                        {
                            LogInfo(MIDText.GetText(eMIDTextCode.msg_Transaction, line));
                            ++_recordsRead;
                        }

                        if (fields.Length < 3 || fields.Length > 4)
                        {
                            LogInfo(MIDText.GetText(eMIDTextCode.msg_DelimiterSetting, delimiter[0].ToString(CultureInfo.CurrentUICulture)));
                            ++_recordsWithErrors;
                            returnCode = eMIDMessageLevel.Error;
                            LogError(MIDText.GetText(eMIDTextCode.msg_MismatchedDelimiter));
                            continue;
                        }

                        if (fields[1] == null ||
                            fields[1].Trim().Length == 0)
                        {
                            LogError(MIDText.GetText(eMIDTextCode.msg_ValueRequired, "Action"));
                            returnCode = eMIDMessageLevel.Error;
                        }
                        else if (fields[1].ToUpper() == "ADD")
                        {
                            if (fields.Length != 4)
                            {
                                LogInfo(MIDText.GetText(eMIDTextCode.msg_LevelAddTransactionIncorrect));
                                ++_recordsWithErrors;
                                returnCode = eMIDMessageLevel.Error;
                                continue;
                            }

                            if (AddLevel(fields[0], fields[2], fields[3], dtHierarchies, organizationalPhRID, dtOrgHierarchyLevels, blValidationStage) == eMIDMessageLevel.None)
                            {
                                if (!blValidationStage)
                                {
                                    ++_levelsAdded;
                                }
                            }
                            else
                            {
                                ++_recordsWithErrors;
                                returnCode = eMIDMessageLevel.Error;
                            }
                        }
                        else if (fields[1].ToUpper() == "REMOVE")
                        {
                            if (RemoveLevel(fields[0], fields[2], dtHierarchies, organizationalPhRID, dtOrgHierarchyLevels, blValidationStage) == eMIDMessageLevel.None)
                            {
                                if (!blValidationStage)
                                {
                                    ++_levelsRemoved;
                                }
                            }
                            else
                            {
                                ++_recordsWithErrors;
                                returnCode = eMIDMessageLevel.Error;
                            }
                        }
                        else
                        {
                            LogError(MIDText.GetText(eMIDTextCode.msg_UnknownAction, fields[1]));
                            ++_recordsWithErrors;
                            returnCode = eMIDMessageLevel.Error;
                            continue;
                        }
                    }
                }

                catch (FileNotFoundException)
                {
                    LogError(MIDText.GetText(eMIDTextCode.msg_InputFileNotFound));
                    returnCode = eMIDMessageLevel.Error;
                }
                catch (Exception ex)
                {
                    LogError(ex.ToString());
                    returnCode = eMIDMessageLevel.Error;
                }
                finally
                {
                    if (reader != null)
                    {
                        reader.Close();
                    }
                    if (exceptionFile != null)
                    {
                        exceptionFile.Close();
                    }
                }

                return returnCode;
            }

            private eMIDMessageLevel AddLevel(string aHierarchy, string aLevel, string aNewLevel, DataTable dtHierarchies, int organizationalPhRID, DataTable dtOrgHierarchyLevels, bool blValidationStage)
            {
                eMIDMessageLevel returnCode = eMIDMessageLevel.None;
                MerchandiseHierarchyData mhd = new MerchandiseHierarchyData();
                bool isOrganizationalHierarchy;
                int hierarchyRID;

                returnCode = ValidateHierarchy(aHierarchy, dtHierarchies, out isOrganizationalHierarchy, out hierarchyRID);
                if (returnCode == eMIDMessageLevel.None)
                {
                    returnCode = ValidateLevel(aHierarchy, aLevel, dtOrgHierarchyLevels, isOrganizationalHierarchy, hierarchyRID, false);
                }
                if (returnCode == eMIDMessageLevel.None &&
                    isOrganizationalHierarchy)
                {
                    returnCode = ValidateNewLevel(aNewLevel, dtOrgHierarchyLevels, isOrganizationalHierarchy, hierarchyRID);
                }

                if (returnCode == eMIDMessageLevel.None &&
                    ! blValidationStage)
                {
                    try
                    {
                        mhd.OpenUpdateConnection();
                        if (isOrganizationalHierarchy)
                        {
                            mhd.AddOrganizationalHierarchyLevel(aNewLevel, aLevel);
                            LogInfo(MIDText.GetText(eMIDTextCode.msg_LevelSuccessfullyAdded, aNewLevel, aHierarchy, aLevel));
                        }
                        else
                        {
                            mhd.AddAlternateHierarchyLevel(hierarchyRID, Convert.ToInt32(aLevel));
                            LogInfo(MIDText.GetText(eMIDTextCode.msg_LevelSuccessfullyAdded, null, aHierarchy, aLevel));
                        }
                        mhd.CommitData();
                        
                    }
                    catch (Exception ex)
                    {
                        LogError(ex.ToString());
                    }
                    finally
                    {
                        if (mhd != null && mhd.ConnectionIsOpen)
                        {
                            mhd.CloseUpdateConnection();
                        }
                    }
                }

                return returnCode;
            }

            private eMIDMessageLevel RemoveLevel(string aHierarchy, string aLevel, DataTable dtHierarchies, int organizationalPhRID, DataTable dtOrgHierarchyLevels, bool blValidationStage)
            {
                eMIDMessageLevel returnCode = eMIDMessageLevel.None;

                MerchandiseHierarchyData mhd = new MerchandiseHierarchyData();
                bool isOrganizationalHierarchy;
                int hierarchyRID;

                returnCode = ValidateHierarchy(aHierarchy, dtHierarchies, out isOrganizationalHierarchy, out hierarchyRID);
                if (returnCode == eMIDMessageLevel.None)
                {
                    returnCode = ValidateLevel(aHierarchy, aLevel, dtOrgHierarchyLevels, isOrganizationalHierarchy, hierarchyRID, true);
                }


                if (returnCode == eMIDMessageLevel.None &&
                    !blValidationStage)
                {
                    try
                    {
                        mhd.OpenUpdateConnection();
                        if (isOrganizationalHierarchy)
                        {
                            mhd.RemoveOrganizationalHierarchyLevel(aLevel);
                        }
                        else
                        {
                            mhd.RemoveAlternateHierarchyLevel(hierarchyRID, Convert.ToInt32(aLevel));
                        }
                        mhd.CommitData();
                        LogInfo(MIDText.GetText(eMIDTextCode.msg_LevelSuccessfullyRemoved, aLevel, aHierarchy));
                    }
                    catch (Exception ex)
                    {
                        LogError(ex.ToString());
                    }
                    finally
                    {
                        if (mhd != null && mhd.ConnectionIsOpen)
                        {
                            mhd.CloseUpdateConnection();
                        }
                    }
                }

                return returnCode;
            }

            private eMIDMessageLevel ValidateHierarchy(string aHierarchy, DataTable dtHierarchies, out bool isOrganizationalHierarchy, out int hierarchyRID)
            {
                eMIDMessageLevel returnCode = eMIDMessageLevel.None;
                isOrganizationalHierarchy = false;
                hierarchyRID = Include.NoRID;

                try
                {
                    if (aHierarchy == null ||
                    aHierarchy.Trim().Length == 0)
                    {
                        LogError(MIDText.GetText(eMIDTextCode.msg_ValueRequired, "Hierarchy"));
                        returnCode = eMIDMessageLevel.Error;
                    }
                    else
                    {
                        DataRow[] rows = dtHierarchies.Select("PH_ID = '" + aHierarchy + "'");

                        if (rows.Length == 0)
                        {
                            LogError(MIDText.GetText(eMIDTextCode.msg_HierarchyNotFound));
                            returnCode = eMIDMessageLevel.Error;
                        }
                        else
                        {
                            hierarchyRID = Convert.ToInt32(rows[0]["PH_RID"]);
                            if ((eHierarchyType)(Convert.ToInt32(rows[0]["PH_Type"])) == eHierarchyType.organizational)
                            {
                                isOrganizationalHierarchy = true;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogError(ex.ToString());
                }

                return returnCode;
            }

            private eMIDMessageLevel ValidateLevel(string aHierarchy, string aHierarchyLevel, DataTable dtOrgHierarchyLevels, bool isOrganizationalHierarchy, int hierarchyRID, bool isRemoveAction)
            {
                eMIDMessageLevel returnCode = eMIDMessageLevel.None;
                MerchandiseHierarchyData mhd;

                if (aHierarchyLevel == null ||
                    aHierarchyLevel.Trim().Length == 0)
                {
                    LogError(MIDText.GetText(eMIDTextCode.msg_ValueRequired, "Level Name"));
                    returnCode = eMIDMessageLevel.Error;
                }
                else if (isOrganizationalHierarchy)
                {
                    DataRow[] rows = dtOrgHierarchyLevels.Select("PHL_ID = '" + aHierarchyLevel + "'");

                    if (rows.Length == 0)
                    {
                        LogError(MIDText.GetText(eMIDTextCode.msg_LevelNameInvalid));
                        returnCode = eMIDMessageLevel.Error;
                    }
                    else if ((eHierarchyLevelType)(Convert.ToInt32(rows[0]["PHL_Type"])) != eHierarchyLevelType.Undefined)
                    {
                        LogError(MIDText.GetText(eMIDTextCode.msg_CannotAddLevelAfterStyle));
                        returnCode = eMIDMessageLevel.Error;
                    }
                }
                else
                {
                    try
                    {
                        int level = Convert.ToInt32(aHierarchyLevel);
                        if ((isRemoveAction && level < 1) ||
                            (!isRemoveAction && level < 0)
                           )
                        {
                            LogError(MIDText.GetText(eMIDTextCode.msg_LevelNameInvalid));
                            returnCode = eMIDMessageLevel.Error;
                        }
                        else
                        {
                            mhd = new MerchandiseHierarchyData();
                            int branchSize = mhd.GetBranchSize(mhd.GetHierarchyRootNodeRID(hierarchyRID), true);
                            if (level > branchSize)
                            {
                                LogError(MIDText.GetText(eMIDTextCode.msg_LevelNameInvalid));
                                LogError(MIDText.GetText(eMIDTextCode.msg_HierarchyBranchSize, aHierarchy, branchSize));
                                returnCode = eMIDMessageLevel.Error;
                            }
                        }
                    }
                    catch
                    {
                        LogError(MIDText.GetText(eMIDTextCode.msg_LevelNameInvalid));
                        returnCode = eMIDMessageLevel.Error;
                    }
                }

                return returnCode;
            }

            private eMIDMessageLevel ValidateNewLevel(string aHierarchyLevel, DataTable dtOrgHierarchyLevels, bool isOrganizationalHierarchy, int hierarchyRID)
            {
                eMIDMessageLevel returnCode = eMIDMessageLevel.None;

                try
                {
                    if (isOrganizationalHierarchy)
                    {
                        if (aHierarchyLevel == null ||
                            aHierarchyLevel.Trim().Length == 0)
                        {
                            LogError(MIDText.GetText(eMIDTextCode.msg_ValueRequired, "New Level"));
                            returnCode = eMIDMessageLevel.Error;
                        }
                        else
                        {
                            DataRow[] rows = dtOrgHierarchyLevels.Select("PHL_ID = '" + aHierarchyLevel + "'");

                            if (rows.Length > 0)
                            {
                                LogError(MIDText.GetText(eMIDTextCode.msg_DuplicateLevelName));
                                returnCode = eMIDMessageLevel.Error;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogError(ex.ToString());
                    returnCode = eMIDMessageLevel.Error;
                }

                return returnCode;
            }

            private eMIDMessageLevel ProcessHierarchyTransactions()
            {
                eMIDMessageLevel returnCode = eMIDMessageLevel.None;

                returnCode = ProcessReclass();

                if (returnCode == eMIDMessageLevel.None)
                {
                    returnCode = ProcessReclassFiles();
                }

                if (returnCode == eMIDMessageLevel.None)
                {
                    returnCode = RemoveTemporaryNodes();
                }

                return returnCode;

            }

            private eMIDMessageLevel ProcessReclass()
            {
                eMIDMessageLevel returnCode = eMIDMessageLevel.None;

                LogInfo(MIDText.GetText(eMIDTextCode.msg_ProcessingStep, "Reclass"));
                try
                {
                    Process process = new Process();
#if (DEBUG)
                    process.StartInfo.FileName = Directory.GetParent(Directory.GetParent(Directory.GetParent(Application.StartupPath).ToString().Trim()).ToString().Trim()) + @"\HierarchyReclass\bin\Debug\" + "HierarchyReclass.exe";
#else
                    process.StartInfo.FileName = Directory.GetParent(Application.StartupPath).ToString().Trim() + @"\HierarchyReclass.exe";
#endif

                    process.StartInfo.Arguments = Include.ForceLocalID;

                    LogDebug(MIDText.GetText(eMIDTextCode.msg_ProcessLocation, "Hierarchy Reclass", process.StartInfo.FileName));
                    LogDebug(MIDText.GetText(eMIDTextCode.msg_ProcessArguments, "Hierarchy Reclass", process.StartInfo.Arguments));

                    process.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
                    process.Start();
                    process.WaitForExit();
                    if (process.ExitCode > eMIDMessageLevel.Information.GetHashCode())
                    {
                        LogError(MIDText.GetText(eMIDTextCode.msg_ProcessingErrorReturnCode, "Hierarchy Reclass", process.ExitCode));
                        returnCode = eMIDMessageLevel.Error;
                    }
                }
                catch (Exception ex)
                {
                    LogError(ex.ToString());
                    returnCode = eMIDMessageLevel.Error;
                }

                return returnCode;
            }

            private eMIDMessageLevel ProcessReclassFiles()
            {
                string configLocation;
                string reclassInputDirectory, reclassOutputDirectory, addTrigger, moveTrigger, deleteTrigger;
                eMIDMessageLevel returnCode = eMIDMessageLevel.None;
                XmlDocument doc;

                LogInfo(MIDText.GetText(eMIDTextCode.msg_ProcessingStep, "Reclass Files"));
                #if (DEBUG)
                configLocation = Directory.GetParent(Directory.GetParent(Directory.GetParent(Application.StartupPath).ToString().Trim()).ToString().Trim()) + @"\HierarchyReclass\bin\Debug\" + "HierarchyReclass.exe.config";
#else
                configLocation = Directory.GetParent(Application.StartupPath).ToString().Trim() + @"\HierarchyReclass.exe.config";
#endif
                LogInfo(MIDText.GetText(eMIDTextCode.msg_FileLocation, configLocation));
                doc = GetXmlDocument(configLocation);
                if (doc == null)
                {
                    LogError(MIDText.GetText(eMIDTextCode.msg_FileNotFound, configLocation));
                    return eMIDMessageLevel.Error;
                }
                reclassInputDirectory = GetValue(doc, "InputDirectory");
                LogInfo(MIDText.GetText(eMIDTextCode.msg_ConfigSettingAndValue, "InputDirectory", reclassInputDirectory));
                reclassOutputDirectory = GetValue(doc, "OutputDirectory");
                LogInfo(MIDText.GetText(eMIDTextCode.msg_ConfigSettingAndValue, "OutputDirectory", reclassOutputDirectory));
                if (reclassOutputDirectory == null)
                {
                    LogError(MIDText.GetText(eMIDTextCode.msg_ConfigSettingNotFound, "OutputDirectory", configLocation));
                    return eMIDMessageLevel.Error;
                }
                addTrigger = GetValue(doc, "TriggerSuffix");
                LogInfo(MIDText.GetText(eMIDTextCode.msg_ConfigSettingAndValue, "TriggerSuffix", addTrigger));
                if (addTrigger != null)
                {
                    returnCode = ProcessReclassOutputFiles(reclassOutputDirectory, addTrigger);
                }
                moveTrigger = GetValue(doc, "ReclassMoveTriggerSuffix");
                LogInfo(MIDText.GetText(eMIDTextCode.msg_ConfigSettingAndValue, "ReclassMoveTriggerSuffix", moveTrigger));
                if (returnCode == eMIDMessageLevel.None &&
                    moveTrigger != null)
                {
                    returnCode = ProcessReclassOutputFiles(reclassOutputDirectory, moveTrigger);
                }
                deleteTrigger = GetValue(doc, "ReclassDeleteTriggerSuffix");
                LogInfo(MIDText.GetText(eMIDTextCode.msg_ConfigSettingAndValue, "ReclassDeleteTriggerSuffix", deleteTrigger));
                if (returnCode == eMIDMessageLevel.None && 
                    deleteTrigger != null)
                {
                    returnCode = ProcessReclassOutputFiles(reclassOutputDirectory, deleteTrigger);
                }

                return returnCode;
            }

            private eMIDMessageLevel ProcessReclassOutputFiles(string aFileLocation, string aTriggerExtension)
            {
                string[] fileList;
                string inputFile = null;
                int lastIndex;
                eMIDMessageLevel returnCode = eMIDMessageLevel.None;
                LogInfo(MIDText.GetText(eMIDTextCode.msg_ProcessingFilesForTrigger, aFileLocation, aTriggerExtension));

                try
                {
                    fileList = Include.GetFiles(aFileLocation, FormatExtension(aTriggerExtension, true));
                    
                    aTriggerExtension = FormatExtension(aTriggerExtension, false);

                    if (fileList.Length == 0)
                    {
                        LogInfo(MIDText.GetText(eMIDTextCode.msg_NoFilesForTrigger, aFileLocation, aTriggerExtension));
                    }
                    else
                    {
                        foreach (string triggerFile in fileList)
                        {
                            lastIndex = triggerFile.LastIndexOf(aTriggerExtension, StringComparison.CurrentCultureIgnoreCase);

                            if (lastIndex >= 0)
                            {
                                inputFile = triggerFile.Remove(lastIndex, aTriggerExtension.Length);
                            }

                            returnCode = ProcessHierarchyLoad(inputFile);
                            if (returnCode == eMIDMessageLevel.None)
                            {
                                System.IO.File.Delete(triggerFile);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogError(ex.ToString());
                    returnCode = eMIDMessageLevel.Error;
                }

                return returnCode;
            }

            private string FormatExtension(string aExtension, bool aIncludeWildcard)
            {
                try
                {
                    if (aExtension[0] != '.')
                    {
                        aExtension = "." + aExtension;
                    }

                    if (aIncludeWildcard)
                    {
                        return "*" + aExtension;
                    }
                    else
                    {
                        return aExtension;
                    }
                }
                catch (Exception exc)
                {
                    string message = exc.ToString();
                    throw;
                }
            }

            private XmlDocument GetXmlDocument(string file)
            {
                if (!File.Exists(file))
                    return null;

                XmlDocument doc = new XmlDocument();
                doc.Load(file);
                return doc;
            }

            private string GetValue(XmlDocument xmlDoc, string strKey)
            {
                if (xmlDoc == null)
                {
                    return string.Empty;
                }

                XmlNode appSettingsNode = GetSettingNode(xmlDoc);
                foreach (XmlNode childNode in appSettingsNode)
                {
                    if (childNode.GetType() != typeof(System.Xml.XmlComment))
                    {
                        if (childNode.Attributes["key"].Value == strKey)
                        {
                            return childNode.Attributes["value"].Value;
                        }
                    }
                }
                return string.Empty;
            }

            private XmlNode GetSettingNode(XmlDocument xmlDoc)
            {
                XmlNode appSettingsNode;
                appSettingsNode = xmlDoc.SelectSingleNode("configuration/appSettings");
                if (appSettingsNode == null)
                {
                    appSettingsNode = xmlDoc.SelectSingleNode("appSettings");
                }
                return appSettingsNode;
            }

            private eMIDMessageLevel ProcessHierarchyLoad(string aFileName)
            {
                eMIDMessageLevel returnCode = eMIDMessageLevel.None;

                try
                {

                    Process process = new Process();
#if (DEBUG)
                    process.StartInfo.FileName = Directory.GetParent(Directory.GetParent(Directory.GetParent(Application.StartupPath).ToString().Trim()).ToString().Trim()) + @"\HierarchyLoad\bin\Debug\" + "HierarchyLoad.exe";
#else
                    process.StartInfo.FileName = Directory.GetParent(Application.StartupPath).ToString().Trim() + @"\HierarchyLoad.exe"; ;
#endif

                    process.StartInfo.Arguments = @"""" + Include.ForceLocalID + @""" """ + aFileName + @"""";

                    process.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
                    LogDebug(MIDText.GetText(eMIDTextCode.msg_ProcessLocation, "Hierarchy Load", process.StartInfo.FileName));
                    LogDebug(MIDText.GetText(eMIDTextCode.msg_ProcessArguments, "Hierarchy Load", process.StartInfo.Arguments));
                    LogInfo(MIDText.GetText(eMIDTextCode.msg_ProcessingStep, aFileName));
                    
                    process.Start();
                    process.WaitForExit();
                    if (process.ExitCode > eMIDMessageLevel.Information.GetHashCode())
                    {
                        LogError(MIDText.GetText(eMIDTextCode.msg_ProcessingErrorReturnCode, "Hierarchy Load", process.ExitCode));
                        returnCode = eMIDMessageLevel.Error;
                    }
                }
                catch (Exception ex)
                {
                    LogError(ex.ToString());
                    returnCode = eMIDMessageLevel.Error;
                }

                return returnCode;
            }

            private eMIDMessageLevel RemoveTemporaryNodes()
            {
                MerchandiseHierarchyData mhd = new MerchandiseHierarchyData();
                eMIDMessageLevel returnCode = eMIDMessageLevel.None;

                LogInfo(MIDText.GetText(eMIDTextCode.msg_ProcessingStep, "removing temporary nodes"));

                try
                {
                    mhd.OpenUpdateConnection();
                    mhd.RemoveTemporaryNodes();
                    mhd.CommitData();
                    LogInfo(MIDText.GetText(eMIDTextCode.msg_CompletedStep, "removing temporary nodes"));
                }
                catch (Exception ex)
                {
                    LogError(ex.ToString());
                    returnCode = eMIDMessageLevel.Error;
                }
                finally
                {
                    if (mhd != null && mhd.ConnectionIsOpen)
                    {
                        mhd.CloseUpdateConnection();
                    }
                }

                return returnCode;
            }

            private void LogInfo(string msg)
            {
                try
                {
                    if (_log.IsInfoEnabled)
                    {
                        _log.Info(msg);
                    }
                }
                catch
                {
                    throw;
                }
            }

            private void LogError(string msg)
            {
                try
                {
                    _log.Error(msg);
                }
                catch
                {
                    throw;
                }
            }

            private void LogDebug(string msg)
            {
                try
                {
                    if (_log.IsDebugEnabled)
                    {
                        _log.Debug(msg);
                    }
                }
                catch
                {
                    throw;
                }
            }
        }
    }
}
