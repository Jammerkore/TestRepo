// Begin TT#3822 - JSmith - Add Stop Time to Purge
// Too many changes to mark.  Use difference tool for comparison.
// Also removed unnecessary comments for readability
// End TT#3822 - JSmith - Add Stop Time to Purge
using System;
using System.IO;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Lifetime;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Xml;
using System.Xml.Serialization;
using System.Threading;


using MIDRetail.Business;
using MIDRetail.Business.Allocation;
using MIDRetail.Common;
using MIDRetail.DataCommon;
using MIDRetail.Data;

namespace MIDRetail.Purge
{
	/// <summary>
	/// Entry point for size codes load.
	/// </summary>
	class Purge
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static int Main(string[] args)
		{
			PurgeWorker purge = new PurgeWorker();
			return purge.Purge(args);
		}
	}

	public class PurgeWorker
	{
		string sourceModule = "Purge.cs";
		string eventLogID = "MIDPurge";
		SessionAddressBlock _SAB;
		SessionSponsor _sponsor;
		IMessageCallback _messageCallback;
		int _storeDailyHistoryRecordsDeleted = 0;
		int _chainWeeklyHistoryRecordsDeleted = 0;
		int _storeWeeklyHistoryRecordsDeleted = 0;
		int _chainOTSPlanRecordsDeleted = 0;
		int _storeOTSPlanRecordsDeleted = 0;
		int _headerRecordsDeleted = 0;
		int _intransitRecordsDeleted = 0;
		int _intransitReviewRecordsDeleted = 0;
		int _dailyPercentagesRecordsDeleted = 0;
        int _usersDeleted = 0;
		int _groupsDeleted = 0;
        int _auditsDeleted = 0;
        int _emptyStoreSetsDeleted = 0;
        int _IMORevRecsDeleted = 0; //TT#1576-MD -jsobek -Data Layer Request - Add VSW Revision Records to Purge
		int concurrentProcesses = 5;
		eMIDMessageLevel highestMessage;
		
		string message = null;
		bool errorFound = false;
		int commitLimit = 100000;
		bool purgeRowsWithAllZeros = false;
		bool purgeRowsWithAllUnlocked = true;
        int auditDays = -1;
		int scheduleDays = -1;
		bool purgeUsers = false;
		int daysToKeepDeletedUsers = 30;
		bool purgeGroups = false;
		int daysToKeepDeletedGroups = 30;
		System.Runtime.Remoting.Channels.IChannel channel;
		int _processId = Include.NoRID;
        int weeksToKeepDailySizeOnhand = 2;
        bool deleteEmptyStoreSets = true;
		
        DateTime _shutdownTime = DateTime.MaxValue; 
        bool _StopTimeExceededMessageWritten = false;
        DateTime _startTime;
		
		public int Purge(string[] args)
		{

			try
			{
				_messageCallback = new BatchMessageCallback();
				_sponsor = new SessionSponsor();
				_SAB = new SessionAddressBlock(_messageCallback, _sponsor);

				if (!EventLog.SourceExists(eventLogID))
				{
					EventLog.CreateEventSource(eventLogID, null);
				}
			
				// Register callback channel

				try
				{
					channel = _SAB.OpenCallbackChannel();
				}
				catch (Exception exception)
				{
					errorFound = true;
					EventLog.WriteEntry(eventLogID, "Error opening port #0 - " + exception.Message, EventLogEntryType.Error);
					return Convert.ToInt32(eMIDMessageLevel.Severe,CultureInfo.CurrentUICulture);
				}

				// Create Sessions

				try
				{
                    _SAB.CreateSessions((int)eServerType.Client | (int)eServerType.Hierarchy | (int)eServerType.Store | (int)eServerType.Application | (int)eServerType.Scheduler);
				}
				catch (Exception exception)
				{
					errorFound = true;
					Exception innerE = exception;
					while (innerE.InnerException != null) 
					{
						innerE = innerE.InnerException;
					}
					EventLog.WriteEntry(eventLogID, "Error creating sessions - " + innerE.Message, EventLogEntryType.Error);
					return Convert.ToInt32(eMIDMessageLevel.Severe,CultureInfo.CurrentUICulture);
				}

				eSecurityAuthenticate authentication = _SAB.ClientServerSession.UserLogin(MIDConfigurationManager.AppSettings["User"], 
					MIDConfigurationManager.AppSettings["Password"], eProcesses.purge);

                //BEGIN TT#1663-VSuart-Header Load and Header Reconcile should return Severe-MID
                //BEGIN TT#1644-VSuart-Process Control-MID
                if (authentication == eSecurityAuthenticate.Unavailable)
                {
                    //return Convert.ToInt32(eMIDMessageLevel.ProcessUnavailable);
                    errorFound = true; //TT#1665 - MD - DOConnell - Completion Status is not being logged in the Audit correctly.
                    return Convert.ToInt32(eMIDMessageLevel.Severe);
                }
                //END TT#1644-VSuart-Process Control-MID
                //END TT#1663-VSuart-Header Load and Header Reconcile should return Severe-MID

				if (authentication != eSecurityAuthenticate.UserAuthenticated)
				{
					errorFound = true;
					EventLog.WriteEntry(eventLogID, "Unable to log in with user:" + MIDConfigurationManager.AppSettings["User"] + " password:" + MIDConfigurationManager.AppSettings["Password"], EventLogEntryType.Error);
					System.Console.Write("Unable to log in with user:" + MIDConfigurationManager.AppSettings["User"] + " password:" + MIDConfigurationManager.AppSettings["Password"]);
					return Convert.ToInt32(eMIDMessageLevel.Severe,CultureInfo.CurrentUICulture);
				}

				if (args.Length > 0)
				{
					if (args[0] == Include.SchedulerID)
					{
						_processId = Convert.ToInt32(args[1]);
					}
				}

				string strCommitLimit = MIDConfigurationManager.AppSettings["CommitLimit"];
				if (strCommitLimit != null)
				{
					if (strCommitLimit.ToUpper(CultureInfo.CurrentCulture) == "UNLIMITED")
					{
						commitLimit = int.MaxValue;
					}
					else
					{
						try
						{
							commitLimit = Convert.ToInt32(strCommitLimit);
						}
						catch
						{
						}
					}
				}

				string strAuditDays = MIDConfigurationManager.AppSettings["DaysToKeepAudit"];
				if (strAuditDays != null)
				{
					try
					{
						auditDays = Convert.ToInt32(strAuditDays);
					}
					catch
					{
					}
				}

				string strScheduleDays = MIDConfigurationManager.AppSettings["DaysToKeepCompletedSchedules"];
				if (strScheduleDays != null)
				{
					try
					{
						scheduleDays = Convert.ToInt32(strScheduleDays);
					}
					catch
					{
					}
				}

				string strParm = MIDConfigurationManager.AppSettings["ConcurrentProcesses"];
				if (strParm != null)
				{
					try
					{
						concurrentProcesses = Convert.ToInt32(strParm);
					}
					catch
					{
					}
				}

				strParm = MIDConfigurationManager.AppSettings["DeleteUsers"];
				if (strParm != null)
				{
					try
					{
						purgeUsers = Convert.ToBoolean(strParm);
					}
					catch
					{
					}
				}

				strParm = MIDConfigurationManager.AppSettings["DaysToKeepDeletedUsers"];
				if (strParm != null)
				{
					try
					{
						daysToKeepDeletedUsers = Convert.ToInt32(strParm);
					}
					catch
					{
					}
				}

				strParm = MIDConfigurationManager.AppSettings["DeleteGroups"];
				if (strParm != null)
				{
					try
					{
						purgeGroups = Convert.ToBoolean(strParm);
					}
					catch
					{
					}
				}

				strParm = MIDConfigurationManager.AppSettings["DaysToKeepDeletedGroups"];
				if (strParm != null)
				{
					try
					{
						daysToKeepDeletedGroups = Convert.ToInt32(strParm);
					}
					catch
					{
					}
				}

                strParm = MIDConfigurationManager.AppSettings["PurgeRowsWithAllZeros"];
				if (strParm != null)
				{
					try
					{
						purgeRowsWithAllZeros = Convert.ToBoolean(strParm);
					}
					catch
					{
					}
				}

                strParm = MIDConfigurationManager.AppSettings["PurgeRowsWithAllUnlocked"];
                if (strParm != null)
                {
                    try
                    {
                        purgeRowsWithAllUnlocked = Convert.ToBoolean(strParm);
                    }
                    catch
                    {
                    }
                }

                strParm = MIDConfigurationManager.AppSettings["WeeksToKeepDailySizeOnhand"];
                if (strParm != null)
                {
                    try
                    {
                        weeksToKeepDailySizeOnhand = Convert.ToInt32(strParm);
                    }
                    catch
                    {
                    }
                }
                
				strParm = MIDConfigurationManager.AppSettings["DeleteEmptyStoreSets"];
				if (strParm != null)
				{
					try
					{
						deleteEmptyStoreSets = Convert.ToBoolean(strParm);
					}
					catch
					{
					}
				}

                
				if (_processId != Include.NoRID)
				{
					_SAB.ClientServerSession.Initialize(_processId);
				}
				else
				{
					_SAB.ClientServerSession.Initialize();
				}

                //_SAB.HierarchyServerSession.Initialize();  // TT#1905 - JSmith - Versioning_Test Interfaced after interface in a new store to a dynamic set when process the alloc override receve severe error.

				_SAB.ApplicationServerSession.Initialize();

				if (_SAB.SchedulerServerSession != null)
				{
					_SAB.SchedulerServerSession.Initialize();
				}
				else
				{
					_SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Warning, "Scheduler Service not found -- Schedules will not be purged", sourceModule);
				}

                _SAB.StoreServerSession.Initialize();

                // Begin TT#1905 - JSmith - Versioning_Test Interfaced after interface in a new store to a dynamic set when process the alloc override receve severe error.
                // StoreServerSession must be initialized before HierarchyServerSession 
                _SAB.HierarchyServerSession.Initialize();
                // End TT#1905 - JSmith - Versioning_Test Interfaced after interface in a new store to a dynamic set when process the alloc override receve severe error.

                strParm = MIDConfigurationManager.AppSettings["PurgeCutoffTime"];
                // Begin TT#1385-MD - JSmith - Purge Cutoff Time
                //if (strParm != null)
                if (strParm != null &&
                    strParm.Trim().Length > 0)
                // Begin TT#1385-MD - JSmith - Purge Cutoff Time
                {
                    try
                    {
                        _shutdownTime = Convert.ToDateTime(DateTime.Now.ToShortDateString() + " " + strParm);
                        if (_shutdownTime != DateTime.MaxValue)
                        {
                            message = MIDText.GetText(eMIDTextCode.msg_CutoffTime, _shutdownTime.TimeOfDay);
                            _SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, message, sourceModule, true);
                        }
                    }
                    catch
                    {
                        message = MIDText.GetText(eMIDTextCode.msg_CutoffTimeInvalid, strParm);
                        _SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Warning, message, sourceModule);
                    }
                }

                

				if (!errorFound)
				{
                    StoreMgmt.LoadInitialStoresAndGroups(_SAB, _SAB.ClientServerSession, true);  // TT#1957-MD - JSmith - Purge fails deleting users with personal attributes
                    errorFound = PurgeData(ref errorFound, commitLimit, auditDays, scheduleDays, concurrentProcesses, purgeUsers, daysToKeepDeletedUsers, purgeGroups, daysToKeepDeletedGroups, purgeRowsWithAllZeros, purgeRowsWithAllUnlocked, weeksToKeepDailySizeOnhand, deleteEmptyStoreSets, _shutdownTime);
                }
			}

			catch ( Exception exception )
			{
				errorFound = true;
				message = "";
				while(exception != null)
				{
					message += " -- " + exception.Message;
					exception = exception.InnerException;
				}
				_SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, message, sourceModule);
				_SAB.ClientServerSession.Audit.UpdateHeader(eProcessCompletionStatus.Failed, eMIDTextCode.sum_Failed, "", _SAB.GetHighestAuditMessageLevel());
			}
			finally
			{ 
				if (!errorFound)
				{
					if (_SAB.ClientServerSession != null && _SAB.ClientServerSession.Audit != null)
					{
						_SAB.ClientServerSession.Audit.UpdateHeader(eProcessCompletionStatus.Successful, eMIDTextCode.sum_Successful, "", _SAB.GetHighestAuditMessageLevel());
					}
				}
				else
				{
					if (_SAB.ClientServerSession != null && _SAB.ClientServerSession.Audit != null)
					{
						_SAB.ClientServerSession.Audit.UpdateHeader(eProcessCompletionStatus.Failed, eMIDTextCode.sum_Failed, "", _SAB.GetHighestAuditMessageLevel());
					}
				}

				highestMessage = _SAB.CloseSessions();
			}

			return Convert.ToInt32(highestMessage, CultureInfo.CurrentUICulture);
		}

        public bool PurgeData(ref bool errorFound, int aCommitLimit, int aAuditDays, int aScheduleDays, int aConcurrentProcesses,
            bool aDeleteUsers, int aDaysToKeepDeletedUsers, bool aDeleteGroups, int aDaysToKeepDeletedGroups, bool aPurgeRowsWithAllZeros,
		    bool aPurgeRowsWithAllUnlocked, int aWeeksToKeepDailySizeOnhand, bool aDeleteEmptyStoreSets, DateTime aShutdownTime)
		{
			Stack purgeStack;
			// use ArrayList to maintain reference until errors are counted
			ArrayList purgeArrayList = new ArrayList();
			int _totalErrors = 0;

			PurgeData pd = new PurgeData();
            try
            {
                _SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, eMIDTextCode.msg_PurgeStarting, sourceModule);

                if (StopTimeExceeded(aShutdownTime)) return errorFound;

                StartTimer();
                _SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, eMIDTextCode.msg_PurgeDeterminingDates, sourceModule);

                // use the hierarchy service to build all purge dates for each node
                // process return bool identifying if error is found
                if (_SAB.HierarchyServerSession.BuildPurgeDates(aWeeksToKeepDailySizeOnhand))
                {
                    _SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Error, eMIDTextCode.msg_ErrorDeterminingPurgeDates, sourceModule);
                    return true;
                }

                _SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, eMIDTextCode.msg_PurgeDeterminingDatesCompleted, "Duration:" + GetDuration(), sourceModule);

                if (StopTimeExceeded(aShutdownTime)) return errorFound;

                if (_SAB.ClientServerSession.GlobalOptions.PerformOnetimePurge)
                {
                    pd.Perform_Onetime_Purge();
                }

                if (StopTimeExceeded(aShutdownTime)) return errorFound;

                StartTimer();
                _SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, eMIDTextCode.msg_PurgeRemovingHistoryForecasts, sourceModule);

                purgeStack = BuildPurgeStack(_SAB.ClientServerSession, purgeArrayList, aCommitLimit, aPurgeRowsWithAllZeros, aPurgeRowsWithAllUnlocked, aShutdownTime);

                if (aConcurrentProcesses > 1)
                {
                    ConcurrentProcessManager cpm = new ConcurrentProcessManager(_SAB.ClientServerSession.Audit, purgeStack, aConcurrentProcesses, 5000);
                    cpm.ProcessCommands();
                }
                else
                {
                    while (purgeStack.Count > 0)
                    {
                        ConcurrentProcess cp = (ConcurrentProcess)purgeStack.Pop();
                        if (!StopTimeExceeded(aShutdownTime))
                        {
                            cp.ExecuteProcess();
                        }
                        _totalErrors += cp.NumberOfErrors;
                    }
                }

                foreach (PurgeProcess pp in purgeArrayList)
                {
                    _totalErrors += pp.NumberOfErrors;
                    if (pp.AutomaticShutdown)
                    {
                        WriteStopTimeExceededMessage();
                    }
                    switch (pp.VariableDataType)
                    {
                        case eVariableDataType.storeDailyHistory:
                            _storeDailyHistoryRecordsDeleted += pp.RecordsDeleted;
                            break;
                        case eVariableDataType.storeWeeklyHistory:
                            _storeWeeklyHistoryRecordsDeleted += pp.RecordsDeleted;
                            break;
                        case eVariableDataType.storeWeeklyForecast:
                            _storeOTSPlanRecordsDeleted += pp.RecordsDeleted;
                            break;
                        case eVariableDataType.chainWeeklyHistory:
                            _chainWeeklyHistoryRecordsDeleted += pp.RecordsDeleted;
                            break;
                        case eVariableDataType.chainWeeklyForecast:
                            _chainOTSPlanRecordsDeleted += pp.RecordsDeleted;
                            break;
                        case eVariableDataType.storeExternalIntransit:
                            _intransitRecordsDeleted += pp.RecordsDeleted;
                            break;
                        case eVariableDataType.storeIntransitReview:
                            _intransitReviewRecordsDeleted += pp.RecordsDeleted;
                            break;
                        // Begin TT#4352 - JSmith - VSW Review records not getting purged
                        case eVariableDataType.storeIMOReview:
                            _IMORevRecsDeleted += pp.RecordsDeleted;
                            break;
                        // End TT#4352 - JSmith - VSW Review records not getting purged
                    }

                    if (pp is StoreDailyPercentagesPurgeProcess)
                    {
                        _dailyPercentagesRecordsDeleted += pp.RecordsDeleted;
                        if (_dailyPercentagesRecordsDeleted > 0)
                        {
                            // create dummy change profile to clear daily percentages cache
                            NodeChangeProfile changeProfile;
                            changeProfile = new NodeChangeProfile(Include.NoRID, _SAB.ClientServerSession.UserRID, DateTime.Now, new Dictionary<int, Dictionary<long, object>>());
                            ChangeProfileDailyPct dpChange = new ChangeProfileDailyPct(
                                Include.NoRID, // storeRID
                                string.Empty, // storeID
                                "1", // new value
                                "0", // old value
                                string.Empty,
                                0
                            );
                            KeyValuePair<long, object> kvp = new KeyValuePair<long, object>(Include.NoRID, dpChange);
                            changeProfile.Add(eProfileType.DailyPercentages, kvp);
                            _SAB.HierarchyServerSession.ClearCache(changeProfile.NodeChanges);
                        }
                    }
                }

                if (!_StopTimeExceededMessageWritten)
                {
                    _SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, eMIDTextCode.msg_PurgeRemovingHistoryForecastsCompleted, "Duration:" + GetDuration(), sourceModule);
                }

                if (StopTimeExceeded(aShutdownTime)) return errorFound;

                _SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, eMIDTextCode.msg_PurgeRemovingHeaders, sourceModule);

                // purge headers
                StartTimer();
                try
                {
                    PurgeHeaders(aCommitLimit, aShutdownTime);
                }
                catch
                {
                    errorFound = true;
                }

                _SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, eMIDTextCode.msg_PurgeRemovingHeadersComplete, "Duration:" + GetDuration(), sourceModule);

                if (StopTimeExceeded(aShutdownTime)) return errorFound;

                _SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, eMIDTextCode.msg_PurgeRemovingAudits, sourceModule);

                StartTimer();
                if (aAuditDays > -1)
                {
                    PurgeAudit(aAuditDays, aCommitLimit, aShutdownTime);
                }

                _SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, eMIDTextCode.msg_PurgeRemovingAuditsComplete, "Duration:" + GetDuration(), sourceModule);

                if (StopTimeExceeded(aShutdownTime)) return errorFound;

                _SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, eMIDTextCode.msg_PurgeRemovingSchedules, sourceModule);

                StartTimer();
                if (_SAB.SchedulerServerSession != null && aScheduleDays > -1)
                {
                    PurgeSchedule(aScheduleDays, aShutdownTime);
                }

                _SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, eMIDTextCode.msg_PurgeRemovingSchedulesComplete, "Duration:" + GetDuration(), sourceModule);

                // purge computation groups
                StartTimer();
                try
                {
                    PurgeComputationGroups(aShutdownTime);
                }
                catch
                {
                    errorFound = true;
                }

                if (StopTimeExceeded(aShutdownTime)) return errorFound;


                if (aDeleteUsers)
                {
                    _SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, eMIDTextCode.msg_PurgeRemovingDeletedUsers, sourceModule);
                    StartTimer();
                    DeleteUsers(aDaysToKeepDeletedUsers, aShutdownTime);
                    _SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, eMIDTextCode.msg_PurgeRemovingDeletedUsersComplete, "Duration:" + GetDuration(), sourceModule);
                }

                if (StopTimeExceeded(aShutdownTime)) return errorFound;

                if (aDeleteGroups)
                {
                    _SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, eMIDTextCode.msg_PurgeRemovingDeletedGroups, sourceModule);
                    StartTimer();
                    DeleteGroups(aDaysToKeepDeletedGroups, aShutdownTime);
                    _SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, eMIDTextCode.msg_PurgeRemovingDeletedUGroupsComplete, "Duration:" + GetDuration(), sourceModule);
                }

                if (StopTimeExceeded(aShutdownTime)) return errorFound;

                // Begin RO-3962 - JSmith - Purge is not deleting dynamic empty set attribute sets
                //if (aDeleteEmptyStoreSets)
                //{
                //    _SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, eMIDTextCode.msg_PurgeRemovingDeletedEmptyStoreSets, sourceModule);
                //    StartTimer();
                //    DeleteEmptyStoreSets(aShutdownTime);
                //    _SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, eMIDTextCode.msg_PurgeRemovingDeletedEmptyStoreSetsComplete, "Duration:" + GetDuration(), sourceModule);
                //}
                // End RO-3962 - JSmith - Purge is not deleting dynamic empty set attribute sets

                if (StopTimeExceeded(aShutdownTime)) return errorFound;

                StartTimer();
                _SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, eMIDTextCode.msg_PurgeRemovingDeletedHierarchyNodes, sourceModule);
                DeleteHierarchyNodes(aShutdownTime);
                _SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, eMIDTextCode.msg_PurgeRemovingDeletedHierarchyNodesComplete, "Duration:" + GetDuration(), sourceModule);

                _SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, eMIDTextCode.msg_PurgeComplete, sourceModule);
            }

            catch (Exception exception)
            {
                errorFound = true;
                _SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, exception.Message, sourceModule);

                throw;
            }
            finally
            {
                // Begin TT#4352 - JSmith - VSW Review records not getting purged
                //_SAB.ClientServerSession.Audit.PurgeAuditInfo_Add(_storeDailyHistoryRecordsDeleted, _chainWeeklyHistoryRecordsDeleted,
                //    _storeWeeklyHistoryRecordsDeleted, _chainOTSPlanRecordsDeleted,
                //    _storeOTSPlanRecordsDeleted, _headerRecordsDeleted, _intransitRecordsDeleted, _intransitReviewRecordsDeleted, _usersDeleted, _groupsDeleted, _auditsDeleted,
                //    _dailyPercentagesRecordsDeleted, _emptyStoreSetsDeleted);
                _SAB.ClientServerSession.Audit.PurgeAuditInfo_Add(_storeDailyHistoryRecordsDeleted, _chainWeeklyHistoryRecordsDeleted,
                   _storeWeeklyHistoryRecordsDeleted, _chainOTSPlanRecordsDeleted,
                   _storeOTSPlanRecordsDeleted, _headerRecordsDeleted, _intransitRecordsDeleted, _intransitReviewRecordsDeleted, _usersDeleted, _groupsDeleted, _auditsDeleted,
                   _dailyPercentagesRecordsDeleted, _emptyStoreSetsDeleted, _IMORevRecsDeleted);
                // End TT#4352 - JSmith - VSW Review records not getting purged
            }
			return false;
		}

        private bool StopTimeExceeded(DateTime aShutdownTime)
        {
            if (aShutdownTime != DateTime.MaxValue &&
                DateTime.Now.TimeOfDay > aShutdownTime.TimeOfDay)
            {
                WriteStopTimeExceededMessage();
                return true;
            }
            return false;
        }

        private void WriteStopTimeExceededMessage()
        {
            if (!_StopTimeExceededMessageWritten)
            {
                _SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Warning, eMIDTextCode.msg_CutoffTimeExceeded, sourceModule);
                _StopTimeExceededMessageWritten = true;
            }
        }
        
        private Stack BuildPurgeStack(Session aSession, ArrayList aPurgeArrayList, int aCommitLimit, bool aPurgeRowsWithAllZeros, bool aPurgeRowsWithAllUnlocked, DateTime aShutdownTime)
		{
			try
			{
				ConcurrentProcess purgeProcess = null;
				Stack purgeStack = new Stack();

				for (int i=0; i<=aSession.GlobalOptions.NumberOfStoreDataTables-1; i++)
				{
					purgeProcess = new StoreDailyHistoryPurgeProcess(_SAB, aSession.Audit, i, aCommitLimit, aPurgeRowsWithAllZeros, aShutdownTime);
					purgeStack.Push(purgeProcess);
					aPurgeArrayList.Add(purgeProcess);
				}
        
                for (int i=0; i<=aSession.GlobalOptions.NumberOfStoreDataTables-1; i++)
				{
					purgeProcess = new StoreWeeklyHistoryPurgeProcess(_SAB, aSession.Audit, i, aCommitLimit, aPurgeRowsWithAllZeros, aShutdownTime); 
					purgeStack.Push(purgeProcess);
					aPurgeArrayList.Add(purgeProcess);
				}
                
                for (int i=0; i<=aSession.GlobalOptions.NumberOfStoreDataTables-1; i++)
				{
					purgeProcess = new StoreWeeklyForecastPurgeProcess(_SAB, aSession.Audit, i, aCommitLimit, aPurgeRowsWithAllZeros, aShutdownTime); 
					purgeStack.Push(purgeProcess);
					aPurgeArrayList.Add(purgeProcess);
				}

                purgeProcess = new StoreWeeklyForecastLockPurgeProcess(_SAB, aSession.Audit, aCommitLimit, aPurgeRowsWithAllZeros, aPurgeRowsWithAllUnlocked, aShutdownTime);
				purgeStack.Push(purgeProcess);
				aPurgeArrayList.Add(purgeProcess);

				purgeProcess = new StoreIntransitPurgeProcess(_SAB, aSession.Audit, aCommitLimit, aPurgeRowsWithAllZeros, aShutdownTime); 
				purgeStack.Push(purgeProcess);
				aPurgeArrayList.Add(purgeProcess);

				purgeProcess = new StoreIntransitReviewPurgeProcess(_SAB, aSession.Audit, aCommitLimit, aPurgeRowsWithAllZeros, aShutdownTime);
				purgeStack.Push(purgeProcess);
				aPurgeArrayList.Add(purgeProcess);

                // Begin TT#4352 - JSmith - VSW Review records not getting purged
                purgeProcess = new StoreIMOReviewPurgeProcess(_SAB, aSession.Audit, aCommitLimit, aPurgeRowsWithAllZeros, aShutdownTime);
                purgeStack.Push(purgeProcess);
                aPurgeArrayList.Add(purgeProcess);
                // End TT#4352 - JSmith - VSW Review records not getting purged

				purgeProcess = new ChainWeeklyHistoryPurgeProcess(_SAB, aSession.Audit, aCommitLimit, aPurgeRowsWithAllZeros, aShutdownTime); 
				purgeStack.Push(purgeProcess);
				aPurgeArrayList.Add(purgeProcess);

				purgeProcess = new ChainWeeklyForecastPurgeProcess(_SAB, aSession.Audit, aCommitLimit, aPurgeRowsWithAllZeros, aShutdownTime); 
				purgeStack.Push(purgeProcess);
				aPurgeArrayList.Add(purgeProcess);

                purgeProcess = new ChainWeeklyForecastLockPurgeProcess(_SAB, aSession.Audit, aCommitLimit, aPurgeRowsWithAllZeros, aPurgeRowsWithAllUnlocked, aShutdownTime);
				purgeStack.Push(purgeProcess);
				aPurgeArrayList.Add(purgeProcess);

                purgeProcess = new AuditForecastPurgeProcess(_SAB, aSession.Audit, aCommitLimit, aPurgeRowsWithAllZeros, aShutdownTime);
				purgeStack.Push(purgeProcess);
				aPurgeArrayList.Add(purgeProcess);

                purgeProcess = new AuditModifiedSalesPurgeProcess(_SAB, aSession.Audit, aCommitLimit, aPurgeRowsWithAllZeros, aShutdownTime);
				purgeStack.Push(purgeProcess);
				aPurgeArrayList.Add(purgeProcess);

                purgeProcess = new StoreDailyPercentagesPurgeProcess(_SAB, aSession.Audit, aCommitLimit, aPurgeRowsWithAllZeros, aShutdownTime);
				purgeStack.Push(purgeProcess);
				aPurgeArrayList.Add(purgeProcess);
                
				return purgeStack;
			}
			catch (Exception exc)
			{
				aSession.Audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in BuildPurgeStack", "TaskListProcessor");
				aSession.Audit.Log_Exception(exc, GetType().Name);
	
				throw;
			}
		}

		public void PurgeHeaders(int aCommitLimit, DateTime aShutdownTime)
		{
			Header headerData = new Header();
			try
			{
                PurgeMultiHeaders(aShutdownTime);
                
                if (StopTimeExceeded(aShutdownTime)) return;
				
                PurgeGroupAllocationHeaders(aShutdownTime);        //TT#1091-MD - STodd - Group Allocation Purge
				
				if (StopTimeExceeded(aShutdownTime)) return;

				string headerID;
				int headerRID;
				AllocationProfile ap = null;
				DataTable headers = headerData.GetHeadersToPurge();

				foreach(DataRow headerRow in headers.Rows)
				{
                    if (StopTimeExceeded(aShutdownTime)) return;

					headerID = Convert.ToString(headerRow["HDR_ID"], CultureInfo.CurrentUICulture);
					headerRID = Convert.ToInt32(headerRow["HDR_RID"], CultureInfo.CurrentUICulture);
					try
					{
                        // Begin TT#1966-MD -  JSmith - DC Fulfillment
                        //ap = new AllocationProfile(_SAB,headerID,headerRID,_SAB.ClientServerSession);
                        ApplicationSessionTransaction aTrans = _SAB.ApplicationServerSession.CreateTransaction();
                        ap = new AllocationProfile(aTrans, headerID, headerRID, _SAB.ApplicationServerSession, false);
                        // End TT#1966-MD -  JSmith - DC Fulfillment
						// skip all headers that participate in multis
						if (ap.InUseByMulti ||
							ap.HeaderGroupRID > Include.DefaultHeaderRID ||
							ap.HeaderType == eHeaderType.MultiHeader)
						{
							continue;
						}

						if (ap.Released)
						{
							if (ap.ShippingComplete)
							{
									try
									{
										ap.Purge(false);
                                        message = _SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_DeleteSuccessfulWithValue, false, headerID);
                                        _SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, message, sourceModule);
										++_headerRecordsDeleted;
									}
									catch (MIDException MIDexception)
									{
										_SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Error, MIDexception.ErrorMessage, sourceModule);
									}
									catch (Exception exception)
									{
										string message = _SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ErrorDeleting, false);
										message = message.Replace("{0}", headerID);
										_SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Error, message, sourceModule);
										_SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Error, exception.Message, sourceModule);
									}
							}
							else
							{
								string message = _SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_UnableToDeleteHeaderNotCompletelyShipped, false);
								message = message.Replace("{0}", headerID);
								_SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Error, message, sourceModule);
							}
						}
					}
					catch (MIDException MIDexception)
					{
						_SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Error, MIDexception.ErrorMessage, sourceModule);
					}
					catch (Exception exception)
					{
						string message = "AllocationProfile failed for " + headerID;
						_SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Error, message, sourceModule);
						_SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Error, exception.Message, sourceModule);
					}

				}
			}
			catch ( Exception exception )
			{
				_SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, exception.Message, sourceModule);
				throw;
			}
			finally
			{ 
			}
		}

        private void PurgeMultiHeaders(DateTime aShutdownTime)
        {
            Header headerData = new Header();
            try
            {
                string headerID;
                int headerRID;
                int headersDeleted = 0;

                DataTable headers = headerData.GetMultiHeadersToPurge();
                headerData.OpenUpdateConnection();

                foreach (DataRow headerRow in headers.Rows)
                {
                    if (StopTimeExceeded(aShutdownTime)) return;

                    headerID = Convert.ToString(headerRow["HDR_ID"], CultureInfo.CurrentUICulture);
                    headerRID = Convert.ToInt32(headerRow["HDR_RID"], CultureInfo.CurrentUICulture);

                    try
                    {
                        headersDeleted += headerData.DeleteMultiHeaders(headerRID);
                        message = _SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_DeleteSuccessfulWithValue, false, headerID);
                        _SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, message, sourceModule);
                        if (headersDeleted >= commitLimit)
                        {
                            headerData.CommitData();
                            _headerRecordsDeleted += headersDeleted;
                            headersDeleted = 0;
                        }
                    }
                    catch
                    {
                        string message = _SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ErrorDeleting, true, headerID);
                    }
                }

                headerData.CommitData();
                _headerRecordsDeleted += headersDeleted;
            }
            catch (Exception exception)
            {
                _SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, exception.Message, sourceModule);
                throw;
            }
            finally
            {
                if (headerData != null &&
                    headerData.ConnectionIsOpen)
                {
                    headerData.CloseUpdateConnection();
                }
            }
        }

        //Begin TT#1091-MD - STodd - Add Header Purge Criteria group allocation
        private void PurgeGroupAllocationHeaders(DateTime aShutdownTime)
        {
            Header headerData = new Header();
            try
            {
                string headerID;
                int headerRID;
                int headersDeleted = 0;

                DataTable headers = headerData.GetGroupAllocationHeadersToPurge();
                headerData.OpenUpdateConnection();

                foreach (DataRow headerRow in headers.Rows)
                {
                    headerID = Convert.ToString(headerRow["HDR_ID"], CultureInfo.CurrentUICulture);
                    headerRID = Convert.ToInt32(headerRow["HDR_RID"], CultureInfo.CurrentUICulture);

                    try
                    {
                        headersDeleted += headerData.DeleteGroupAllocation(headerRID);
                        message = _SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_DeleteSuccessfulWithValue, false, headerID);
                        _SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, message, sourceModule);
                        if (headersDeleted >= commitLimit)
                        {
                            headerData.CommitData();
                            _headerRecordsDeleted += headersDeleted;
                            headersDeleted = 0;
                        }
                    }
                    catch
                    {
                        string message = _SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ErrorDeleting, true, headerID);
                    }
                }

                headerData.CommitData();
                _headerRecordsDeleted += headersDeleted;
            }
            catch (Exception exception)
            {
                _SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, exception.Message, sourceModule);
                throw;
            }
            finally
            {
                if (headerData != null &&
                    headerData.ConnectionIsOpen)
                {
                    headerData.CloseUpdateConnection();
                }
            }
        }
        //End TT#1091-MD - STodd - Add Header Purge Criteria group allocation

        public void PurgeAudit(int aAuditDays, int aCommitLimit, DateTime aShutdownTime)
		{
			PurgeData pd = new PurgeData();
            bool aAutomaticStopExceeded = false;
			try
			{
				pd.OpenUpdateConnection();
                _auditsDeleted = pd.Purge_Audit(aAuditDays, aCommitLimit, aShutdownTime, out aAutomaticStopExceeded);
				pd.CommitData();

                if (aAutomaticStopExceeded)
                {
                    _SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Warning, eMIDTextCode.msg_CutoffTimeExceeded, sourceModule);
                }
			}
			catch ( Exception exception )
			{
				_SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, exception.Message, sourceModule);
				throw;
			}
			finally
			{ 
				pd.CloseUpdateConnection();
			}
		}

        public void PurgeSchedule(int aScheduleDays, DateTime aShutdownTime)
		{
			ScheduleData dlSchedule = new ScheduleData();
			DateTime purgeDate;
			DataTable dtSchedules;
			DataTable dtOrphanedJobs;
			DataTable dtOrphanedTasklists;

			try
			{
				dlSchedule.OpenUpdateConnection();

				try
				{
					purgeDate = DateTime.Now.AddDays(aScheduleDays * -1);

					dtSchedules = dlSchedule.ReadCompletedJobsOlderThanDate(purgeDate);
					_SAB.SchedulerServerSession.DeleteSchedulesFromList(dtSchedules);

					dtOrphanedJobs = dlSchedule.ReadOrphanedSystemJobs();
					dlSchedule.JobTaskListJoin_DeleteSystemFromList(dtOrphanedJobs);
					dlSchedule.Job_DeleteSystemFromList(dtOrphanedJobs);

					dtOrphanedTasklists = dlSchedule.ReadOrphanedSystemTaskLists();
					dlSchedule.TaskList_DeleteSystemFromList(dtOrphanedTasklists);

					dlSchedule.CommitData();
				}
				catch ( Exception exception )
				{
					_SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, exception.Message, sourceModule);
					throw;
				}
				finally
				{ 
					dlSchedule.CloseUpdateConnection();
				}
			}
			catch ( Exception exception )
			{
				_SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, exception.Message, sourceModule);
				throw;
			}
		}

        public void PurgeComputationGroups(DateTime aShutdownTime)
		{
			ComputationData cd = new ComputationData();
			DataTable dtComputationGroups;
			int computationGroupRID;

			try
			{
				dtComputationGroups = cd.ComputationGroups_Read();
				cd.OpenUpdateConnection();

				try
				{
					foreach (DataRow dr in dtComputationGroups.Rows)
					{
                        if (StopTimeExceeded(aShutdownTime)) break;

						computationGroupRID = Convert.ToInt32(dr["CG_RID"], CultureInfo.CurrentUICulture);
						if (cd.GetItemCount(computationGroupRID) == 0)
						{
							cd.DeleteComputationGroup(computationGroupRID);
						}
					}
					
					cd.CommitData();
				}
				catch ( Exception exception )
				{
					_SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, exception.Message, sourceModule);
					throw;
				}
				finally
				{ 
					cd.CloseUpdateConnection();
				}
			}
			catch ( Exception exception )
			{
				_SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, exception.Message, sourceModule);
				throw;
			}
		}

        private void DeleteUsers(int aDaysToKeepDeletedUsers, DateTime aShutdownTime)
		{
			SecurityAdmin securityData = new SecurityAdmin();
			DataTable dtDeletedUsers;
            DataTable dtHierarchies;   //TT#5245-Purge error number 2-BonTon
            MerchandiseHierarchyData dlHierarchy;   //TT#5245-Purge error number 2-BonTon
			int userRID;
			string userName = null, userFullName, message;
			DateTime deleteUserDate, userDeletedDate;
            bool deleteUser = true;
            bool aAutomaticStopExceeded = false;
            int userCommitLimit = 50;

			try
			{
				deleteUserDate = DateTime.Now.AddDays(-aDaysToKeepDeletedUsers);
				dtDeletedUsers = securityData.GetDeletedUsers();

				try
				{
					foreach (DataRow dr in dtDeletedUsers.Rows)
					{
                        if (StopTimeExceeded(aShutdownTime)) break;

                        deleteUser = true;
						try
						{
							userRID = Convert.ToInt32(dr["USER_RID"], CultureInfo.CurrentUICulture);
							userName = Convert.ToString(dr["USER_NAME"], CultureInfo.CurrentUICulture);
							userFullName = Convert.ToString(dr["USER_FULLNAME"], CultureInfo.CurrentUICulture);
							userDeletedDate = Convert.ToDateTime(dr["USER_DELETE_DATETIME"], CultureInfo.CurrentUICulture);
		
                            //BEGIN TT#5245-Purge error number 2-BonTon
                            dlHierarchy = new MerchandiseHierarchyData();
                            //dtHierarchies = dlHierarchy.MyHierarchies_Read(userRID);
                            dtHierarchies = dlHierarchy.MyHierarchies_InUse_Read(userRID);

                            //BEGIN TT#5245-Purge error number 2-BonTon
                            if (dtHierarchies.Rows.Count > 0)
                            {   //We have a user marked for deletion that has Hierarchies in use.
                                message = _SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_HierarchyInUseByUser, false);
                                message = message.Replace("{0}", userName);
                                _SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Warning, message, sourceModule);
                                message = _SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_HierarchyInUseByUserItem, false);
                                foreach (DataRow inUseRow in dtHierarchies.Rows)
                                {
                                    string inUseMessage = string.Format(
                                        (string)message.Clone(),
                                        Convert.ToString(inUseRow["NODE_ID"]),
                                        Convert.ToString(inUseRow["ITEM_TYPE"]),
                                        Convert.ToString(inUseRow["ITEM_NAME"]),
                                        Convert.ToString(inUseRow["ITEM_OWNER"])
                                        );
                                    _SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Warning, inUseMessage, sourceModule);
                                }
                            }
                            //END TT#5245-Purge error number 2-BonTon

                            if (dtHierarchies.Rows.Count == 0)
                            {
							if (userDeletedDate > deleteUserDate)
							{
								continue;
							}

							if (CheckForRunningJobs(userRID))
							{
								message = _SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_CannotDeleteUserItemsRunning);
								message = message.Replace("{0}", userName);
								_SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Warning, message, sourceModule);
								continue;
							}

							securityData.OpenUpdateConnection();
                            securityData.DeleteUserFilters(userRID, userCommitLimit, aShutdownTime, out aAutomaticStopExceeded);
                            if (aAutomaticStopExceeded)
                            {
                                securityData.CommitData();
                                _SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Warning, eMIDTextCode.msg_CutoffTimeExceeded, sourceModule);
                                break;
                            }
                            securityData.DeleteUserMethods(userRID, userCommitLimit, aShutdownTime, out aAutomaticStopExceeded);
                            if (aAutomaticStopExceeded)
                            {
                                securityData.CommitData();
                                _SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Warning, eMIDTextCode.msg_CutoffTimeExceeded, sourceModule);
                                break;
                            }

                            securityData.DeleteUserWorkflows(userRID, userCommitLimit, aShutdownTime, out aAutomaticStopExceeded);
                            if (aAutomaticStopExceeded)
                            {
                                securityData.CommitData();
                                _SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Warning, eMIDTextCode.msg_CutoffTimeExceeded, sourceModule);
                                break;
                            }
                            securityData.DeleteUserTasklists(userRID, userCommitLimit, aShutdownTime, out aAutomaticStopExceeded);
                            if (aAutomaticStopExceeded)
                            {
                                securityData.CommitData();
                                _SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Warning, eMIDTextCode.msg_CutoffTimeExceeded, sourceModule);
                                break;
                            }
							securityData.DeleteAllUserSession(userRID);
							securityData.CommitData();

                            if (StopTimeExceeded(aShutdownTime)) break;

                            if (!DeleteUserHierarchies(userRID, userName, aShutdownTime))
							{
                                deleteUser = false;
							}

                            if (StopTimeExceeded(aShutdownTime)) break;

                            if (!DeleteUserGroups(userRID, aShutdownTime))
                            {
                                deleteUser = false;
                            }

                            if (StopTimeExceeded(aShutdownTime)) break;

                            if (!deleteUser)
                            {
                                message = _SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_AllUserItemsNotDeleted, false);
                                message = message.Replace("{0}", userName);
                                _SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Warning, message, sourceModule);
                            }
                            else
                            {
                                //Begin TT#1313-MD -jsobek -Header Filters
                                //Delete filters and their corresponding stored procedures for the user
                                FilterData filterData = new FilterData();
                                DataTable dtStoreFilters = filterData.FilterReadAllForUser(filterTypes.StoreFilter, userRID);
                                DataTable dtHeaderFilters = filterData.FilterReadAllForUser(filterTypes.HeaderFilter, userRID);
                                DataTable dtAssortmentFilters = filterData.FilterReadAllForUser(filterTypes.AssortmentFilter, userRID);
                              
                                //Delete store filters
                                filterData.OpenUpdateConnection();
                                foreach (DataRow drFilter in dtStoreFilters.Rows)
                                {
                                    int filterRID = (int)drFilter["FILTER_RID"];
                                    filterData.FilterDelete(filterRID);
                                }
                                filterData.CommitData();
                                filterData.CloseUpdateConnection();

                                //Delete the procedures
                                foreach (DataRow drFilter in dtHeaderFilters.Rows)
                                {
                                    int filterRID = (int)drFilter["FILTER_RID"];
                                    filterData.RemoveFilterProcedure(filterRID, filterTypes.HeaderFilter);
                                }
                                foreach (DataRow drFilter in dtAssortmentFilters.Rows)
                                {
                                    int filterRID = (int)drFilter["FILTER_RID"];
                                    filterData.RemoveFilterProcedure(filterRID, filterTypes.AssortmentFilter);
                                }

                              

                                //Delete header and assortment filters
                                filterData.OpenUpdateConnection();

                                filterData.WorkspaceCurrentFilter_Delete(userRID, eWorkspaceType.AllocationWorkspace);
                                filterData.WorkspaceCurrentFilter_Delete(userRID, eWorkspaceType.AssortmentWorkspace);
                                filterData.CommitData();

                                foreach (DataRow drFilter in dtHeaderFilters.Rows)
                                {
                                    int filterRID = (int)drFilter["FILTER_RID"];
                                    filterData.FilterDelete(filterRID);
                                }
                                foreach (DataRow drFilter in dtAssortmentFilters.Rows)
                                {
                                    int filterRID = (int)drFilter["FILTER_RID"];
                                    filterData.FilterDelete(filterRID);
                                }
                                filterData.CommitData();
                                filterData.CloseUpdateConnection();
                                //End TT#1313-MD -jsobek -Header Filters


                                securityData.OpenUpdateConnection();
                                securityData.DeleteUser(userRID);
                                securityData.CommitData();
                                ++_usersDeleted;
                                message = _SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_DeleteSuccessfulWithValue, false);
                                message = message.Replace("{0}", userName);
                                _SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, message, sourceModule);
                                }
                                //END TT#5245-Purge error number 2-BonTon
                            }
						}
						catch
						{
							throw;
						}
						finally
						{ 
							securityData.CloseUpdateConnection();
						}
					}
				}
				catch
				{
					throw;
				}
			}
			catch ( Exception exception )
			{
                // Begin TT#5245 - JSmith - Purge Error number 2
                if (userName != null)
                {
                    message = _SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_AllUserItemsNotDeleted, false);
                    message = message.Replace("{0}", userName);
                    _SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Warning, message, sourceModule);
                }
                // End TT#5245 - JSmith - Purge Error number 2
				_SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, exception.Message, sourceModule);
				throw;
			}
		}

		private bool CheckForRunningJobs(int aUserRID)
		{
			ScheduleData dlSchedule;
			DataTable dtJobs;
			DataRow[] runningJobs;
			bool invalidJobStatusFound;

			try
			{
				if (_SAB.SchedulerServerSession == null)
				{
					_SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, eMIDTextCode.msg_ScheduleSessionNotAvailable, sourceModule);
					return false;
				}

				dlSchedule = new ScheduleData();
				dtJobs = dlSchedule.Job_ReadByUser(aUserRID);

				if (dtJobs.Rows.Count > 0)
				{
					runningJobs = dtJobs.Select("EXECUTION_STATUS = " + (int)eProcessExecutionStatus.Running + " OR EXECUTION_STATUS = " + (int)eProcessExecutionStatus.Queued);

					if (runningJobs.Length > 0)
					{
						_SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, eMIDTextCode.msg_CannotDeleteRunningJob, sourceModule); 
						return true;
					}

					runningJobs = dtJobs.Select (
						"EXECUTION_STATUS = " + (int)eProcessExecutionStatus.Executed + " OR EXECUTION_STATUS = " + (int)eProcessExecutionStatus.OnHold +
						" OR EXECUTION_STATUS = " + (int)eProcessExecutionStatus.Waiting);

					if (runningJobs.Length > 0)
					{
						return true;
					}
				}

				try
				{
					if (dtJobs.Rows.Count > 0)
					{
						invalidJobStatusFound = _SAB.SchedulerServerSession.DeleteSchedulesFromList(dtJobs);
					}
					else
					{
						invalidJobStatusFound = false;
					}

					if (invalidJobStatusFound)
					{
						_SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, eMIDTextCode.msg_DeleteFailedDataInUse, sourceModule);
						return true;
					}
				}
				catch (Exception error)
				{
					string message = error.ToString();
					throw;
				}
			}
			catch (Exception error)
			{
				string message = error.ToString();
				throw;
			}
			

			return false;
		}

        private bool DeleteUserHierarchies(int aUserRID, string aUserName, DateTime aShutdownTime)
		{
			MerchandiseHierarchyData dlHierarchy;
			HierarchyNodeProfile hnp;
			HierarchyProfile hp;
			DataTable dtHierarchies, dtNodes;
			int phRID, hnRID;
			string message;
            bool blAllHierarchiesDeleted = true;

			try
			{
				dlHierarchy = new MerchandiseHierarchyData();
				dtHierarchies = dlHierarchy.MyHierarchies_Read(aUserRID);

				if (dtHierarchies.Rows.Count > 0)
				{
					if (_SAB.HierarchyServerSession == null)
					{
						_SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, eMIDTextCode.msg_ScheduleSessionNotAvailable, sourceModule);
						return false;
					}

					foreach (DataRow dr in dtHierarchies.Rows)
					{
                        if (StopTimeExceeded(aShutdownTime)) break;

						phRID = Convert.ToInt32(dr["PH_RID"], CultureInfo.CurrentUICulture);
						try
						{
                            if (dlHierarchy.HierarchyInUseByHeader(phRID))
                            {
                                message = _SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_HierarchyInUseByHeader, false);
                                message = message.Replace("{0}", aUserName);
                                _SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Warning, message, sourceModule);
                                blAllHierarchiesDeleted = false;
                                continue;
                            }

							hp = _SAB.HierarchyServerSession.GetHierarchyDataForUpdate(phRID, false);
							if (hp.HierarchyLockStatus == eLockStatus.ReadOnly ||
								hp.HierarchyLockStatus == eLockStatus.Cancel)
							{

                                string[] errParms = new string[3];
                                errParms.SetValue("Delete Node", 0);
                                errParms.SetValue(hp.HierarchyID.ToString().Trim(), 1);
                                errParms.SetValue(hp.InUseUserID.ToString().Trim(), 2);
								message = MIDText.GetText(eMIDTextCode.msg_StandardInUseMsg, errParms);

								_SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Warning, message, sourceModule);
										
								return false;
							}
							dtNodes = dlHierarchy.Hierarchy_NodeRID_Read_For_Purge(phRID);
							foreach (DataRow nodesdr in dtNodes.Rows)
							{
								hnRID = Convert.ToInt32(nodesdr["HN_RID"], CultureInfo.CurrentUICulture);
								try
								{
									hnp = _SAB.HierarchyServerSession.GetNodeDataForUpdate(hnRID, false);
									if (hnp.NodeLockStatus == eLockStatus.ReadOnly ||
										hnp.NodeLockStatus == eLockStatus.Cancel)
									{
                                        string[] errParms = new string[3];
                                        errParms.SetValue("Delete Node", 0);
                                        errParms.SetValue(hnp.Text.ToString().Trim(), 1);
                                        errParms.SetValue(hnp.InUseUserID.ToString().Trim(), 2);
                                        message = MIDText.GetText(eMIDTextCode.msg_StandardInUseMsg, errParms);

                                        _SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Warning, message, sourceModule);
										
										return false;
									}
									if (hnp.HomeHierarchyLevel > 0)
									{
										try
										{
											_SAB.HierarchyServerSession.OpenUpdateConnection();
											hnp.NodeChangeType = eChangeType.delete;
											_SAB.HierarchyServerSession.NodeUpdateProfileInfo(hnp);
											_SAB.HierarchyServerSession.CommitData();
										}
										catch
										{
											throw;
										}
										finally
										{
											_SAB.HierarchyServerSession.CloseUpdateConnection();
										}
									}
								}
								catch
								{
									throw;
								}
								finally
								{
									_SAB.HierarchyServerSession.DequeueNode(hnRID);
								}
							}
							// delete the hierarchy
							try
							{
								_SAB.HierarchyServerSession.OpenUpdateConnection();
								hp.HierarchyChangeType = eChangeType.delete;
								_SAB.HierarchyServerSession.HierarchyUpdate(hp);
								_SAB.HierarchyServerSession.CommitData();
							}
							catch
							{
								throw;
							}
							finally
							{
								_SAB.HierarchyServerSession.CloseUpdateConnection();
							}
						}
						catch
						{
							throw;
						}
						finally
						{
							_SAB.HierarchyServerSession.DequeueHierarchy(phRID);
						}
					}
				}
			}
			catch
			{
				throw;
			}	

            return blAllHierarchiesDeleted;
		}

        private bool DeleteUserGroups(int aUserRID, DateTime aShutdownTime)
        {
            ProfileList storeGroupList;
            GenericEnqueue objEnqueue;
            string message;
            StoreGroupProfile currStoreGroupProfile = null;

            try
            {
                storeGroupList = StoreMgmt.StoreGroup_GetListViewList(aUserRID); //_SAB.StoreServerSession.GetStoreGroupListViewList(aUserRID);
                foreach (StoreGroupProfile groupProf in storeGroupList)
                {
                    currStoreGroupProfile = groupProf;
                    if (StopTimeExceeded(aShutdownTime)) break;

                    if (groupProf.OwnerUserRID == aUserRID)
                    {
                        objEnqueue = new GenericEnqueue(eLockType.StoreGroup, groupProf.Key, _SAB.ClientServerSession.UserRID, _SAB.ClientServerSession.ThreadID);

                        try
                        {
                            objEnqueue.EnqueueGeneric();
                            // Begin TT#1957-MD - JSmith - Purge fails deleting users with personal attributes
                            //StoreMgmt.StoreGroup_SetInactive(groupProf.Key); //_SAB.StoreServerSession.DeleteGroup(groupProf.Key);
                            StoreMgmt.StoreGroup_Delete(groupProf.Key);
                            // End TT#1957-MD - JSmith - Purge fails deleting users with personal attributes
                        }
                        catch (GenericConflictException)
                        {
                            string[] errParms = new string[3];
                            errParms.SetValue("Delete User Group", 0);
                            errParms.SetValue(groupProf.Name.ToString().Trim(), 1);
                            errParms.SetValue(((GenericConflict)objEnqueue.ConflictList[0]).UserName, 2);
                            message = MIDText.GetText(eMIDTextCode.msg_StandardInUseMsg, errParms);
                            
                            _SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Warning, message, sourceModule);

                            return false;
                        }
                        finally
                        {
                            objEnqueue.DequeueGeneric();
                        }
                    }
                }

            }
            // Begin TT#1957-MD - JSmith - Purge fails deleting users with personal attributes
            catch (DatabaseForeignKeyViolation)
            {
                string[] errParms = new string[3];
                errParms.SetValue("Delete User Group", 0);
                errParms.SetValue(currStoreGroupProfile.Name.ToString().Trim(), 1);
                errParms.SetValue("Global", 2);
                message = MIDText.GetText(eMIDTextCode.msg_StandardInUseMsg, errParms);

                _SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Warning, message, sourceModule);

                return false;
            }
            // End TT#1957-MD - JSmith - Purge fails deleting users with personal attributes
            catch (Exception exception)
            {
                _SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, exception.Message, sourceModule);
                throw;
            } 

            return true;
        }

        private void DeleteGroups(int aDaysToKeepDeletedGroups, DateTime aShutdownTime)
		{
			SecurityAdmin securityData = new SecurityAdmin();
			DataTable dtDeletedGroups;
			int groupRID;
			string groupName, message;
			DateTime deleteGroupDate, groupDeletedDate;

			try
			{
				deleteGroupDate = DateTime.Now.AddDays(-aDaysToKeepDeletedGroups);
				dtDeletedGroups = securityData.GetDeletedGroups();

				try
				{
					foreach (DataRow dr in dtDeletedGroups.Rows)
					{
						try
						{
							groupRID = Convert.ToInt32(dr["GROUP_RID"], CultureInfo.CurrentUICulture);
							groupName = Convert.ToString(dr["GROUP_NAME"], CultureInfo.CurrentUICulture);
							groupDeletedDate = Convert.ToDateTime(dr["GROUP_DELETE_DATETIME"], CultureInfo.CurrentUICulture);
		
							if (groupDeletedDate > deleteGroupDate)
							{
								continue;
							}

							securityData.OpenUpdateConnection();
							securityData.DeleteGroup(groupRID);
							securityData.CommitData();
							++_groupsDeleted;
							message = _SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_DeleteSuccessfulWithValue, false);
							message = message.Replace("{0}", groupName);
							_SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, message, sourceModule);
						}
						catch
						{
							throw;
						}
						finally
						{ 
							securityData.CloseUpdateConnection();
						}
					}
				}
				catch
				{
					throw;
				}
			}
			catch ( Exception exception )
			{
				_SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, exception.Message, sourceModule);
				throw;
			}
		}

        private void DeleteEmptyStoreSets(DateTime aShutdownTime)
		{
			//StoreData storeData = new StoreData();
			
			try
			{
				int numGroupsRemoved = 0;
                bool setsInUse = false; // StoreMgmt.DeleteUnusedGroupLevels(ref numGroupsRemoved); //_SAB.StoreServerSession.DeleteUnusedGroupLevels(ref numGroupsRemoved);
				if (setsInUse)
				{
					_SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, eMIDTextCode.msg_PurgeEmptyStoreSetsInUse, sourceModule);
				}
				_emptyStoreSetsDeleted = numGroupsRemoved;
				message = _SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_EmptyStoreSetsDeleted, false);
				message = message.Replace("{0}", _emptyStoreSetsDeleted.ToString());
				_SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, message, sourceModule);
			}
			catch (Exception exception)
			{
				_SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, exception.Message, sourceModule);
				throw;
			}
		}

        private void PurgeOldCalendarDateRanges()
        {
            CalendarData calendarData = new CalendarData();

            try
            {
                calendarData.OpenUpdateConnection();
                calendarData.PurgeOldCalendarDateRanges();
                calendarData.CommitData();
            }
            catch (Exception exception)
            {
                _SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, exception.Message, sourceModule);
                throw;
            }
            finally
            {
                calendarData.CloseUpdateConnection();
            }
        }

        private void DeleteHierarchyNodes(DateTime aShutdownTime)
        {
            MerchandiseHierarchyData mhd = new MerchandiseHierarchyData();
            HierarchyMaintenance hm = new HierarchyMaintenance(_SAB, _SAB.ClientServerSession);
            HierarchyNodeProfile hnp;
            EditMsgs em;
            eMIDMessageLevel messageLevel = eMIDMessageLevel.Information;
            string module = string.Empty;
            string deleteConfirmed = MIDText.GetText(eMIDTextCode.msg_PurgeDeleteConfirmed);
            string deleteError = MIDText.GetText(eMIDTextCode.msg_ErrorDeleting);

            try
            {
                DataTable dt = mhd.Hierarchy_Node_GetDeletedNodes();   
                foreach (DataRow dr in dt.Rows)
                {
                    em = new EditMsgs();
                    hnp = _SAB.HierarchyServerSession.GetNodeData(Convert.ToInt32(dr["HN_RID"]));
                    if (hnp.HomeHierarchyLevel == 0)   // delete hierarchy
                    {
                        HierarchyProfile hp = new HierarchyProfile(hnp.HomeHierarchyRID);
                        hp.HierarchyChangeType = eChangeType.delete;
                        hm.ProcessHierarchyData(ref em, hp);
                    }
                    else
                    {
                        hnp.NodeChangeType = eChangeType.delete;
                        hm.ProcessNodeProfileInfo(ref em, hnp);
                    }

                    // strip off Delete tag
                    int StartIndex;
                    int EndIndex;
                    string text;
                    StartIndex = hnp.Text.IndexOf("#Del");
                    if (StartIndex > -1)
                    {
                        EndIndex = hnp.Text.IndexOf("#", StartIndex + 1);
                        text = hnp.Text.Substring(0, StartIndex - 1) + hnp.Text.Substring(EndIndex + 1, hnp.Text.Length - EndIndex - 1);
                    }
                    else
                    {
                        text = hnp.Text;
                    }
                    messageLevel = eMIDMessageLevel.Information;
                    if (!em.ErrorFound)
                    {
                        message = (string)deleteConfirmed.Clone();
                        message = message.Replace("{0}", text);
                    }
                    else
                    {
                        message = (string)deleteError.Clone();
                        message = message.Replace("{0}", text);
                        message += Environment.NewLine + _SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_FollowingErrors);
                        for (int i = 0; i < em.EditMessages.Count; i++)
                        {
                            EditMsgs.Message emm = (EditMsgs.Message)em.EditMessages[i];
                            message += Environment.NewLine + "     ";
                            if (emm.messageByCode &&
                                emm.code != eMIDTextCode.Unassigned)
                            {
                                message += _SAB.ClientServerSession.Audit.GetText(emm.code);
                            }
                            else
                            {
                                message += emm.msg;
                            }

                            if (emm.messageLevel > messageLevel)
                            {
                                messageLevel = emm.messageLevel;
                            }

                            if (module == string.Empty)
                            {
                                module = emm.module;
                            }
                        }
                    }
                    _SAB.ClientServerSession.Audit.Add_Msg(messageLevel, message, this.GetType().Name);
                }
            }
            catch (Exception exception)
            {
                _SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, exception.Message, sourceModule);
                throw;
            }
        }

        protected void StartTimer()
        {
            _startTime = DateTime.Now;
        }

        protected string GetDuration()
        {
            TimeSpan duration = DateTime.Now.Subtract(_startTime);
            return Convert.ToString(duration, CultureInfo.CurrentUICulture);
        }
	}

	abstract public class PurgeProcess : ConcurrentProcess
	{
		//=======
		// FIELDS
		//=======
		private PurgeData _pd = null;
		private VariablesData _vd = null;
		private Intransit _it = null;
        private IMO_Data _imo = null;   // TT#4352 - JSmith - VSW Review records not getting purged
		private SessionAddressBlock _SAB;
		private int _recordsDeleted;
		private eVariableDataType _variableDataType;
		private int _commitLimit;
		private bool _purgeRowsWithAllZeros = false;
        private DateTime _shutdownTime;
        private bool _automaticShutdown = false;
        private DateTime _startTime;
		
		//=============
		// CONSTRUCTORS
		//=============

		public PurgeProcess(SessionAddressBlock aSAB, Audit aAudit, eVariableDataType aVariableDataType,
            int aCommitLimit, bool aPurgeRowsWithAllZeros, DateTime aShutdownTime)
			: base(aAudit)
		{
			try
			{
				_SAB = aSAB;
				_recordsDeleted = 0;
				_variableDataType = aVariableDataType;
				_commitLimit = aCommitLimit;
				_purgeRowsWithAllZeros = aPurgeRowsWithAllZeros;
                _shutdownTime = aShutdownTime;
                ExitMessageLevel = eMIDMessageLevel.Information;
                StartTimer();
			}
			catch (Exception exc)
			{
				ExitMessageLevel = eMIDMessageLevel.Severe;
				Audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in Constructor", "CommandThreadProcessor");
				Audit.Log_Exception(exc, GetType().Name);
				throw;
			}
		}

		//===========
		// PROPERTIES
		//===========

		public PurgeData PurgeData 
		{
			get 
			{ 
				if (_pd == null)
				{
					_pd = new PurgeData();
				}
				return _pd ; 
			}
		}

		public VariablesData VarData 
		{
			get 
			{ 
				if (_vd == null)
				{
                    // Begin TT#5124 - JSmith - Performance
                    //_vd = new VariablesData();
                    _vd = new VariablesData(_SAB.ApplicationServerSession.GlobalOptions.NumberOfStoreDataTables);
                    // End TT#5124 - JSmith - Performance
				}
				return _vd ; 
			}
		}

		public Intransit IntransitData 
		{
			get 
			{ 
				if (_it == null)
				{
					_it = new Intransit();
				}
				return _it ; 
			}
		}

        // Begin TT#4352 - JSmith - VSW Review records not getting purged
        public IMO_Data IMOData
        {
            get
            {
                if (_imo == null)
                {
                    _imo = new IMO_Data();
                }
                return _imo;
            }
        }
        // End TT#4352 - JSmith - VSW Review records not getting purged

		public SessionAddressBlock SAB 
		{
			get { return _SAB ; }
		}

		public eVariableDataType VariableDataType 
		{
			get { return _variableDataType ; }
		}

		public int RecordsDeleted 
		{
			get { return _recordsDeleted ; }
			set { _recordsDeleted = value ; }

		}

		public int CommitLimit 
		{
			get { return _commitLimit ; }
			set { _commitLimit = value ; }

		}

		public bool PurgeRowsWithAllZeros 
		{
			get { return _purgeRowsWithAllZeros ; }

		}

        public int NumberOfTables
        {
            get { return _SAB.ClientServerSession.GlobalOptions.NumberOfStoreDataTables; }

        }

        public bool AutomaticShutdown
        {
            get { return _automaticShutdown; }

        }

	
		//========
		// METHODS
		//========
        protected bool StopTimeExceeded()
        {
            if (_shutdownTime != DateTime.MaxValue &&  
                DateTime.Now.TimeOfDay > _shutdownTime.TimeOfDay)
            {
                _automaticShutdown = true;
                return true;
            }
            return false;
        }

        protected void StartTimer()
        {
            _startTime = DateTime.Now;
        }

        protected string GetDuration()
        {
            TimeSpan duration = DateTime.Now.Subtract(_startTime);
            return Convert.ToString(duration, CultureInfo.CurrentUICulture);
        }
	}


	public class StoreDailyHistoryPurgeProcess : PurgeProcess
	{
		//=======
		// FIELDS
		//=======
		private int _tableNumber;
		
		//=============
		// CONSTRUCTORS
		//=============

        public StoreDailyHistoryPurgeProcess(SessionAddressBlock aSAB, Audit aAudit, int aTableNumber, int aCommitLimit, bool aPurgeRowsWithAllZeros, DateTime aShutdownTime)
            : base(aSAB, aAudit, eVariableDataType.storeDailyHistory, aCommitLimit, aPurgeRowsWithAllZeros, aShutdownTime)
		{
			try
			{
				_tableNumber = aTableNumber;
			}
			catch (Exception exc)
			{
				ExitMessageLevel = eMIDMessageLevel.Severe;
				Audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in Constructor", "CommandThreadProcessor");
				Audit.Log_Exception(exc, GetType().Name);
				throw;
			}
		}

		//===========
		// PROPERTIES
		//===========

		public int TableNumber 
		{
			get { return _tableNumber ; }
		}
		
		//========
		// METHODS
		//========

		override public void ExecuteProcess()
		{
			eMIDMessageLevel messageLevel = eMIDMessageLevel.Information;
			int recordsDeleted = CommitLimit + 1;  // initialize to non-zero start loop

            string message = "Executing purge for store Daily History - table " + TableNumber.ToString();
            StartTimer();
			try
			{
				IsRunning = true;

				Audit.Add_Msg(eMIDMessageLevel.Information, message, "PurgeProcess");

				try
				{
					try
					{
						DataTable dt = PurgeData.PurgeDates_ReadDailyHistoryDates();
                        // Begin TT#5210 - JSmith - Purge Performance
                        if (dt.Rows.Count == 0)	// nothing to do
                        {
                            return;
                        }
                        // End TT#5210 - JSmith - Purge Performance
						VarData.OpenUpdateConnection();
						switch (dt.Rows.Count)
						{
							case 0:	// nothing to do
								break;
							case 1:
								DataRow dr = dt.Rows[0];
								int purgeDate = (dr["PURGE_DAILY_HISTORY"] == System.DBNull.Value) ? Include.Undefined : Convert.ToInt32(dr["PURGE_DAILY_HISTORY"], CultureInfo.CurrentUICulture);
								if (purgeDate > Include.Undefined)
								{
									while (recordsDeleted >= CommitLimit)
									{
                                        if (StopTimeExceeded()) break;

                                        recordsDeleted = VarData.StoreDailyHistory_DeleteLessThanDate(TableNumber, purgeDate, CommitLimit, NumberOfTables);
										RecordsDeleted += recordsDeleted;
										VarData.CommitData();
									}
								}
								break;
							default:
								while (recordsDeleted >= CommitLimit)
								{
                                    if (StopTimeExceeded()) break;

                                    recordsDeleted = VarData.StoreDailyHistory_DeleteLessThanDate(TableNumber, CommitLimit, NumberOfTables);
									RecordsDeleted += recordsDeleted;
									VarData.CommitData();
								}
								break;
						}
						VarData.CommitData();
					}
					catch ( Exception ex )
					{
						++NumberOfErrors;
						Audit.Log_Exception(ex, GetType().Name);
					}
					finally
					{
						VarData.CloseUpdateConnection();
					}
				}
				catch ( Exception ex )
				{
					Audit.Log_Exception(ex, GetType().Name);
				}


			}
			catch (ThreadAbortException)
			{
				try
				{
					Audit.Add_Msg(eMIDMessageLevel.Warning, "Cancelled by User", "CommandThreadProcessor");

					messageLevel = eMIDMessageLevel.Severe;
				}
				catch (InvalidOperationException)
				{
					messageLevel = eMIDMessageLevel.Severe;
				}
			}
			catch (Exception exc)
			{
				Audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in ExecuteProcess", "CommandThreadProcessor");
				Audit.Log_Exception(exc, GetType().Name);

				messageLevel = eMIDMessageLevel.Severe;
			}
			finally
			{
				IsRunning = false;
				CompletionDateTime = DateTime.Now;
				ExitMessageLevel = messageLevel;

                message = message.Replace("Executing purge", "Completed purge");
                Audit.Add_Msg(eMIDMessageLevel.Information, message + " with Message Level " + MIDText.GetTextOnly((int)ExitMessageLevel) + ". Duration: " + GetDuration(), "CommandThreadProcessor");
			}
		}
	}

	public class StoreIntransitPurgeProcess : PurgeProcess
	{
		//=======
		// FIELDS
		//=======
		
		//=============
		// CONSTRUCTORS
		//=============

        public StoreIntransitPurgeProcess(SessionAddressBlock aSAB, Audit aAudit, int aCommitLimit, bool aPurgeRowsWithAllZeros, DateTime aShutdownTime)
            : base(aSAB, aAudit, eVariableDataType.storeExternalIntransit, aCommitLimit, aPurgeRowsWithAllZeros, aShutdownTime)
		{
			try
			{
			}
			catch (Exception exc)
			{
				ExitMessageLevel = eMIDMessageLevel.Severe;
				Audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in Constructor", "CommandThreadProcessor");
				Audit.Log_Exception(exc, GetType().Name);
				throw;
			}
		}

		//===========
		// PROPERTIES
		//===========

		
		//========
		// METHODS
		//========

		override public void ExecuteProcess()
		{
			eMIDMessageLevel messageLevel = eMIDMessageLevel.Information;
			int recordsDeleted = CommitLimit + 1;  // initialize to non-zero start loop

			string message = "Executing purge for store intransit" ;
            StartTimer();
			try
			{
				IsRunning = true;

				Audit.Add_Msg(eMIDMessageLevel.Information, message, "PurgeProcess");

				try
				{
					try
					{
						IntransitData.OpenUpdateConnection();
						while (recordsDeleted >= CommitLimit)
						{
                            if (StopTimeExceeded()) break;

							recordsDeleted = IntransitData.Purge_External_Intransit(CommitLimit);
							RecordsDeleted += recordsDeleted;
							IntransitData.CommitData();
						}
						IntransitData.CommitData();
					}
					catch ( Exception ex )
					{
						++NumberOfErrors;
						Audit.Log_Exception(ex, GetType().Name);
					}
					finally
					{
						IntransitData.CloseUpdateConnection();
					}
				}
				catch ( Exception ex )
				{
					Audit.Log_Exception(ex, GetType().Name);
				}


			}
			catch (ThreadAbortException)
			{
				try
				{
					Audit.Add_Msg(eMIDMessageLevel.Warning, "Cancelled by User", "CommandThreadProcessor");

					messageLevel = eMIDMessageLevel.Severe;
				}
				catch (InvalidOperationException)
				{
					messageLevel = eMIDMessageLevel.Severe;
				}
			}
			catch (Exception exc)
			{
				Audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in ExecuteProcess", "CommandThreadProcessor");
				Audit.Log_Exception(exc, GetType().Name);

				messageLevel = eMIDMessageLevel.Severe;
			}
			finally
			{
				IsRunning = false;
				CompletionDateTime = DateTime.Now;
				ExitMessageLevel = messageLevel;

                message = message.Replace("Executing purge", "Completed purge");
                Audit.Add_Msg(eMIDMessageLevel.Information, message + " with Message Level " + MIDText.GetTextOnly((int)ExitMessageLevel) + ". Duration: " + GetDuration(), "CommandThreadProcessor");
			}
		}

	}

	public class StoreIntransitReviewPurgeProcess : PurgeProcess
	{
		//=======
		// FIELDS
		//=======

		//=============
		// CONSTRUCTORS
		//=============

        public StoreIntransitReviewPurgeProcess(SessionAddressBlock aSAB, Audit aAudit, int aCommitLimit, bool aPurgeRowsWithAllZeros, DateTime aShutdownTime)
			: base(aSAB, aAudit, eVariableDataType.storeIntransitReview, aCommitLimit, aPurgeRowsWithAllZeros, aShutdownTime)
		{
			try
			{
			}
			catch (Exception exc)
			{
				ExitMessageLevel = eMIDMessageLevel.Severe;
				Audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in Constructor", "CommandThreadProcessor");
				Audit.Log_Exception(exc, GetType().Name);
				throw;
			}
		}

		//===========
		// PROPERTIES
		//===========


		//========
		// METHODS
		//========

		override public void ExecuteProcess()
		{
			eMIDMessageLevel messageLevel = eMIDMessageLevel.Information;
			int recordsDeleted = CommitLimit + 1;  // initialize to non-zero start loop

			string message = "Executing purge for store intransit revision records";
            StartTimer();
			try
			{
				IsRunning = true;

				Audit.Add_Msg(eMIDMessageLevel.Information, message, "PurgeProcess");

				try
				{
					try
					{
						IntransitData.OpenUpdateConnection();
						while (recordsDeleted >= CommitLimit)
						{
                            if (StopTimeExceeded()) break;

							recordsDeleted = IntransitData.Purge_Intransit_Review(CommitLimit);
							RecordsDeleted += recordsDeleted;
							IntransitData.CommitData();
						}
						IntransitData.CommitData();
					}
					catch (Exception ex)
					{
						++NumberOfErrors;
						Audit.Log_Exception(ex, GetType().Name);
					}
					finally
					{
						IntransitData.CloseUpdateConnection();
					}
				}
				catch (Exception ex)
				{
					Audit.Log_Exception(ex, GetType().Name);
				}


			}
			catch (ThreadAbortException)
			{
				try
				{
					Audit.Add_Msg(eMIDMessageLevel.Warning, "Cancelled by User", "CommandThreadProcessor");

					messageLevel = eMIDMessageLevel.Severe;
				}
				catch (InvalidOperationException)
				{
					messageLevel = eMIDMessageLevel.Severe;
				}
			}
			catch (Exception exc)
			{
				Audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in ExecuteProcess", "CommandThreadProcessor");
				Audit.Log_Exception(exc, GetType().Name);

				messageLevel = eMIDMessageLevel.Severe;
			}
			finally
			{
				IsRunning = false;
				CompletionDateTime = DateTime.Now;
				ExitMessageLevel = messageLevel;

                message = message.Replace("Executing purge", "Completed purge");
                Audit.Add_Msg(eMIDMessageLevel.Information, message + " with Message Level " + MIDText.GetTextOnly((int)ExitMessageLevel) + ". Duration: " + GetDuration(), "CommandThreadProcessor");
			}
		}

	}

    // Begin TT#4352 - JSmith - VSW Review records not getting purged
    public class StoreIMOReviewPurgeProcess : PurgeProcess
    {
        //=======
        // FIELDS
        //=======

        //=============
        // CONSTRUCTORS
        //=============

        public StoreIMOReviewPurgeProcess(SessionAddressBlock aSAB, Audit aAudit, int aCommitLimit, bool aPurgeRowsWithAllZeros, DateTime aShutdownTime)
            : base(aSAB, aAudit, eVariableDataType.storeIMOReview, aCommitLimit, aPurgeRowsWithAllZeros, aShutdownTime)
        {
            try
            {
            }
            catch (Exception exc)
            {
                ExitMessageLevel = eMIDMessageLevel.Severe;
                Audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in Constructor", "CommandThreadProcessor");
                Audit.Log_Exception(exc, GetType().Name);
                throw;
            }
        }

        //===========
        // PROPERTIES
        //===========


        //========
        // METHODS
        //========

        override public void ExecuteProcess()
        {
            eMIDMessageLevel messageLevel = eMIDMessageLevel.Information;
            int recordsDeleted = CommitLimit + 1;  // initialize to non-zero start loop

            string message = "Executing purge for store VSW revision records";
            StartTimer();
            try
            {
                IsRunning = true;

                Audit.Add_Msg(eMIDMessageLevel.Information, message, "PurgeProcess");

                try
                {
                    try
                    {
                        IMOData.OpenUpdateConnection();
                        while (recordsDeleted >= CommitLimit)
                        {
                            if (StopTimeExceeded()) break;

                            recordsDeleted = IMOData.Purge_IMO_Review(CommitLimit);
                            RecordsDeleted += recordsDeleted;
                            IMOData.CommitData();
                        }
                        IMOData.CommitData();
                    }
                    catch (Exception ex)
                    {
                        ++NumberOfErrors;
                        Audit.Log_Exception(ex, GetType().Name);
                    }
                    finally
                    {
                        IMOData.CloseUpdateConnection();
                    }
                }
                catch (Exception ex)
                {
                    Audit.Log_Exception(ex, GetType().Name);
                }


            }
            catch (ThreadAbortException)
            {
                try
                {
                    Audit.Add_Msg(eMIDMessageLevel.Warning, "Cancelled by User", "CommandThreadProcessor");

                    messageLevel = eMIDMessageLevel.Severe;
                }
                catch (InvalidOperationException)
                {
                    messageLevel = eMIDMessageLevel.Severe;
                }
            }
            catch (Exception exc)
            {
                Audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in ExecuteProcess", "CommandThreadProcessor");
                Audit.Log_Exception(exc, GetType().Name);

                messageLevel = eMIDMessageLevel.Severe;
            }
            finally
            {
                IsRunning = false;
                CompletionDateTime = DateTime.Now;
                ExitMessageLevel = messageLevel;

                message = message.Replace("Executing purge", "Completed purge");
                Audit.Add_Msg(eMIDMessageLevel.Information, message + " with Message Level " + MIDText.GetTextOnly((int)ExitMessageLevel) + ". Duration: " + GetDuration(), "CommandThreadProcessor");
            }
        }

    }
    // End TT#4352 - JSmith - VSW Review records not getting purged

	public class StoreWeeklyHistoryPurgeProcess : PurgeProcess
	{
		//=======
		// FIELDS
		//=======
		private int _tableNumber;
		
		//=============
		// CONSTRUCTORS
		//=============

        public StoreWeeklyHistoryPurgeProcess(SessionAddressBlock aSAB, Audit aAudit, int aTableNumber, int aCommitLimit, bool aPurgeRowsWithAllZeros, DateTime aShutdownTime)
            : base(aSAB, aAudit, eVariableDataType.storeWeeklyHistory, aCommitLimit, aPurgeRowsWithAllZeros, aShutdownTime)
		{
			try
			{
				_tableNumber = aTableNumber;
			}
			catch (Exception exc)
			{
				ExitMessageLevel = eMIDMessageLevel.Severe;
				Audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in Constructor", "CommandThreadProcessor");
				Audit.Log_Exception(exc, GetType().Name);
				throw;
			}
		}

		//===========
		// PROPERTIES
		//===========

		public int TableNumber 
		{
			get { return _tableNumber ; }
		}
		
		//========
		// METHODS
		//========

		override public void ExecuteProcess()
		{
			eMIDMessageLevel messageLevel = eMIDMessageLevel.Information;
			int recordsDeleted = CommitLimit + 1;  // initialize to non-zero start loop

			string message = "Executing purge for store weekly history - table " + TableNumber.ToString();
            StartTimer();
			try
			{
				IsRunning = true;

				Audit.Add_Msg(eMIDMessageLevel.Information, message, "PurgeProcess");

				try
				{
					try
					{
						DataTable dt = PurgeData.PurgeDates_ReadWeeklyHistoryDates();
                        // Begin TT#5210 - JSmith - Purge Performance
                        if (dt.Rows.Count == 0)	// nothing to do
                        {
                            return;
                        }
                        // End TT#5210 - JSmith - Purge Performance
						VarData.OpenUpdateConnection();
						switch (dt.Rows.Count)
						{
							case 0:	// nothing to do
								break;
							case 1:
								DataRow dr = dt.Rows[0];
								int purgeDate = (dr["PURGE_WEEKLY_HISTORY"] == System.DBNull.Value) ? Include.Undefined : Convert.ToInt32(dr["PURGE_WEEKLY_HISTORY"], CultureInfo.CurrentUICulture);
								if (purgeDate > Include.Undefined)
								{
									while (recordsDeleted >= CommitLimit)
									{
                                        if (StopTimeExceeded()) break;

                                        recordsDeleted = VarData.StoreWeeklyHistory_DeleteLessThanDate(TableNumber, purgeDate, CommitLimit, NumberOfTables);
										RecordsDeleted += recordsDeleted;
										VarData.CommitData();
									}
                                    // Begin TT#2131-MD - JSmith - Halo Integration
                                    if (SAB.ROExtractEnabled)
                                    {
                                        recordsDeleted = CommitLimit + 1;  // initialize to non-zero start loop
                                        while (recordsDeleted >= CommitLimit)
                                        {
                                            if (StopTimeExceeded()) break;

                                            recordsDeleted = VarData.EXTRACT_PLANNING_CONTROL_StoreHistoryDeleteLessThanDate(purgeDate, CommitLimit);
                                            VarData.CommitData();
                                        }
                                    }
                                    // End TT#2131-MD - JSmith - Halo Integration
                                }
                                break;
							default:
								while (recordsDeleted >= CommitLimit)
								{
                                    if (StopTimeExceeded()) break;

                                    recordsDeleted = VarData.StoreWeeklyHistory_DeleteLessThanDate(TableNumber, CommitLimit, NumberOfTables);
									RecordsDeleted += recordsDeleted;
									VarData.CommitData();
								}
                                // Begin TT#2131-MD - JSmith - Halo Integration
                                if (SAB.ROExtractEnabled)
                                {
                                    recordsDeleted = CommitLimit + 1;  // initialize to non-zero start loop
                                    while (recordsDeleted >= CommitLimit)
                                    {
                                        if (StopTimeExceeded()) break;

                                        recordsDeleted = VarData.EXTRACT_PLANNING_CONTROL_StoreHistoryDeleteLessThanDate(CommitLimit);
                                        VarData.CommitData();
                                    }
                                }
                                // End TT#2131-MD - JSmith - Halo Integration
                                break;
						}
						VarData.CommitData();
					}
					catch ( Exception ex )
					{
						++NumberOfErrors;
						Audit.Log_Exception(ex, GetType().Name);
					}
					finally
					{
						VarData.CloseUpdateConnection();
					}
				}
				catch ( Exception ex )
				{
					Audit.Log_Exception(ex, GetType().Name);
				}


			}
			catch (ThreadAbortException)
			{
				try
				{
					Audit.Add_Msg(eMIDMessageLevel.Warning, "Cancelled by User", "CommandThreadProcessor");

					messageLevel = eMIDMessageLevel.Severe;
				}
				catch (InvalidOperationException)
				{
					messageLevel = eMIDMessageLevel.Severe;
				}
			}
			catch (Exception exc)
			{
				Audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in ExecuteProcess", "CommandThreadProcessor");
				Audit.Log_Exception(exc, GetType().Name);

				messageLevel = eMIDMessageLevel.Severe;
			}
			finally
			{
				IsRunning = false;
				CompletionDateTime = DateTime.Now;
				ExitMessageLevel = messageLevel;

                message = message.Replace("Executing purge", "Completed purge");
                Audit.Add_Msg(eMIDMessageLevel.Information, message + " with Message Level " + MIDText.GetTextOnly((int)ExitMessageLevel) + ". Duration: " + GetDuration(), "CommandThreadProcessor");
			}
		}
	}

	public class StoreWeeklyForecastPurgeProcess : PurgeProcess
	{
		//=======
		// FIELDS
		//=======
		private int _tableNumber;
		
		//=============
		// CONSTRUCTORS
		//=============

        public StoreWeeklyForecastPurgeProcess(SessionAddressBlock aSAB, Audit aAudit, int aTableNumber, int aCommitLimit, bool aPurgeRowsWithAllZeros, DateTime aShutdownTime)
            : base(aSAB, aAudit, eVariableDataType.storeWeeklyForecast, aCommitLimit, aPurgeRowsWithAllZeros, aShutdownTime)
		{
			try
			{
				_tableNumber = aTableNumber;
			}
			catch (Exception exc)
			{
				ExitMessageLevel = eMIDMessageLevel.Severe;
				Audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in Constructor", "CommandThreadProcessor");
				Audit.Log_Exception(exc, GetType().Name);
				throw;
			}
		}

		//===========
		// PROPERTIES
		//===========

		public int TableNumber 
		{
			get { return _tableNumber ; }
		}
		
		//========
		// METHODS
		//========

		override public void ExecuteProcess()
		{
			eMIDMessageLevel messageLevel = eMIDMessageLevel.Information;
			int recordsDeleted = CommitLimit + 1;  // initialize to non-zero start loop

			string message = "Executing purge for store weekly forecasts - table " + TableNumber.ToString();
            StartTimer();
			try
			{
				IsRunning = true;

				Audit.Add_Msg(eMIDMessageLevel.Information, message, "PurgeProcess");

				try
				{
					try
					{
						DataTable dt = PurgeData.PurgeDates_ReadPlanDates();
                        // Begin TT#5210 - JSmith - Purge Performance
                        if (dt.Rows.Count == 0)	// nothing to do
                        {
                            return;
                        }
                        // End TT#5210 - JSmith - Purge Performance
						VarData.OpenUpdateConnection();
						switch (dt.Rows.Count)
						{
							case 0:	// nothing to do
								break;
							case 1:
								DataRow dr = dt.Rows[0];
								int purgeDate = (dr["PURGE_PLANS"] == System.DBNull.Value) ? Include.Undefined : Convert.ToInt32(dr["PURGE_PLANS"], CultureInfo.CurrentUICulture);
								if (purgeDate > Include.Undefined)
								{
									while (recordsDeleted >= CommitLimit)
									{
                                        if (StopTimeExceeded()) break;

                                        recordsDeleted = VarData.StorePlans_DeleteLessThanDate(TableNumber, purgeDate, CommitLimit, NumberOfTables);
										RecordsDeleted += recordsDeleted;
										VarData.CommitData();
									}
                                    // Begin TT#2131-MD - JSmith - Halo Integration
                                    if (SAB.ROExtractEnabled)
                                    {
                                        recordsDeleted = CommitLimit + 1;  // initialize to non-zero start loop
                                        while (recordsDeleted >= CommitLimit)
                                        {
                                            if (StopTimeExceeded()) break;

                                            recordsDeleted = VarData.EXTRACT_PLANNING_CONTROL_StoreForecastDeleteLessThanDate(purgeDate, CommitLimit);
                                            VarData.CommitData();
                                        }
                                    }
                                    // End TT#2131-MD - JSmith - Halo Integration
                                }
                                break;
							default:
								while (recordsDeleted >= CommitLimit)
								{
                                    if (StopTimeExceeded()) break;

                                    recordsDeleted = VarData.StorePlans_DeleteLessThanDate(TableNumber, CommitLimit, NumberOfTables);
									RecordsDeleted += recordsDeleted;
									VarData.CommitData();
								}
                                // Begin TT#2131-MD - JSmith - Halo Integration
                                if (SAB.ROExtractEnabled)
                                {
                                    recordsDeleted = CommitLimit + 1;  // initialize to non-zero start loop
                                    while (recordsDeleted >= CommitLimit)
                                    {
                                        if (StopTimeExceeded()) break;

                                        recordsDeleted = VarData.EXTRACT_PLANNING_CONTROL_StoreForecastDeleteLessThanDate(CommitLimit);
                                        VarData.CommitData();
                                    }
                                }
                                // End TT#2131-MD - JSmith - Halo Integration
                                break;
						}
						VarData.CommitData();

                        if (PurgeRowsWithAllZeros)
                        {
                            recordsDeleted = CommitLimit + 1;
                            while (recordsDeleted >= CommitLimit)
                            {
                                if (StopTimeExceeded()) break;

                                recordsDeleted = VarData.StorePlans_ZeroRows(TableNumber, CommitLimit);
                                VarData.CommitData();
                            }
                            VarData.CommitData();
                        }
					}
					catch ( Exception ex )
					{
						++NumberOfErrors;
						Audit.Log_Exception(ex, GetType().Name);
					}
					finally
					{
						VarData.CloseUpdateConnection();
					}
				}
				catch ( Exception ex )
				{
					Audit.Log_Exception(ex, GetType().Name);
				}


			}
			catch (ThreadAbortException)
			{
				try
				{
					Audit.Add_Msg(eMIDMessageLevel.Warning, "Cancelled by User", "CommandThreadProcessor");

					messageLevel = eMIDMessageLevel.Severe;
				}
				catch (InvalidOperationException)
				{
					messageLevel = eMIDMessageLevel.Severe;
				}
			}
			catch (Exception exc)
			{
				Audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in ExecuteProcess", "CommandThreadProcessor");
				Audit.Log_Exception(exc, GetType().Name);

				messageLevel = eMIDMessageLevel.Severe;
			}
			finally
			{
				IsRunning = false;
				CompletionDateTime = DateTime.Now;
				ExitMessageLevel = messageLevel;

                message = message.Replace("Executing purge", "Completed purge");
                Audit.Add_Msg(eMIDMessageLevel.Information, message + " with Message Level " + MIDText.GetTextOnly((int)ExitMessageLevel) + ". Duration: " + GetDuration(), "CommandThreadProcessor");
			}
		}
	}

	public class StoreWeeklyForecastLockPurgeProcess : PurgeProcess
	{
		//=======
		// FIELDS
		//=======
        private bool _purgeRowsWithAllUnlocked = true;

		
		//=============
		// CONSTRUCTORS
		//=============

        public StoreWeeklyForecastLockPurgeProcess(SessionAddressBlock aSAB, Audit aAudit, int aCommitLimit, bool aPurgeRowsWithAllZeros, bool aPurgeRowsWithAllUnlocked, DateTime aShutdownTime)
            : base(aSAB, aAudit, eVariableDataType.storeWeeklyForecast, aCommitLimit, aPurgeRowsWithAllZeros, aShutdownTime)
		{
            try
            {
                _purgeRowsWithAllUnlocked = aPurgeRowsWithAllUnlocked;
            }
            catch (Exception exc)
            {
                ExitMessageLevel = eMIDMessageLevel.Severe;
                Audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in Constructor", "CommandThreadProcessor");
                Audit.Log_Exception(exc, GetType().Name);
                throw;
            }
		}

		//===========
		// PROPERTIES
		//===========

		//========
		// METHODS
		//========

		override public void ExecuteProcess()
		{
			eMIDMessageLevel messageLevel = eMIDMessageLevel.Information;
			int recordsDeleted = CommitLimit + 1;  // initialize to non-zero start loop

			string message = "Executing purge for store weekly forecasts locks";
            StartTimer();
			try
			{
				IsRunning = true;

				Audit.Add_Msg(eMIDMessageLevel.Information, message, "PurgeProcess");

				try
				{
					try
					{
						DataTable dt = PurgeData.PurgeDates_ReadPlanDates();
                        // Begin TT#5210 - JSmith - Purge Performance
                        if (dt.Rows.Count == 0)	// nothing to do
                        {
                            return;
                        }
                        // End TT#5210 - JSmith - Purge Performance
						VarData.OpenUpdateConnection();
						switch (dt.Rows.Count)
						{
							case 0:	// nothing to do
								break;
							case 1:
								DataRow dr = dt.Rows[0];
								int purgeDate = (dr["PURGE_PLANS"] == System.DBNull.Value) ? Include.Undefined : Convert.ToInt32(dr["PURGE_PLANS"], CultureInfo.CurrentUICulture);
								if (purgeDate > Include.Undefined)
								{
									while (recordsDeleted >= CommitLimit)
									{
                                        if (StopTimeExceeded()) break;

										recordsDeleted = VarData.StorePlanLocks_DeleteLessThanDate(purgeDate, CommitLimit);
										RecordsDeleted += recordsDeleted;
										VarData.CommitData();
									}
								}
								break;
							default:
								while (recordsDeleted >= CommitLimit)
								{
                                    if (StopTimeExceeded()) break;

									recordsDeleted = VarData.StorePlanLocks_DeleteLessThanDate(CommitLimit);
									RecordsDeleted += recordsDeleted;
									VarData.CommitData();
								}
								break;
						}

                        if (!StopTimeExceeded()) 
                        {
                            if (_purgeRowsWithAllUnlocked)
                            {
                                recordsDeleted = CommitLimit + 1;
                                while (recordsDeleted >= CommitLimit)
                                {
                                    if (StopTimeExceeded()) break;

                                    recordsDeleted = VarData.StorePlans_DeleteAllUnlocked(CommitLimit);
                                    VarData.CommitData();
                                }
                            }
                        }

						VarData.CommitData();
					}
					catch ( Exception ex )
					{
						++NumberOfErrors;
						Audit.Log_Exception(ex, GetType().Name);
					}
					finally
					{
						VarData.CloseUpdateConnection();
					}
				}
				catch ( Exception ex )
				{
					Audit.Log_Exception(ex, GetType().Name);
				}


			}
			catch (ThreadAbortException)
			{
				try
				{
					Audit.Add_Msg(eMIDMessageLevel.Warning, "Cancelled by User", "CommandThreadProcessor");

					messageLevel = eMIDMessageLevel.Severe;
				}
				catch (InvalidOperationException)
				{
					messageLevel = eMIDMessageLevel.Severe;
				}
			}
			catch (Exception exc)
			{
				Audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in ExecuteProcess", "CommandThreadProcessor");
				Audit.Log_Exception(exc, GetType().Name);

				messageLevel = eMIDMessageLevel.Severe;
			}
			finally
			{
				IsRunning = false;
				CompletionDateTime = DateTime.Now;
				ExitMessageLevel = messageLevel;

                message = message.Replace("Executing purge", "Completed purge");
                Audit.Add_Msg(eMIDMessageLevel.Information, message + " with Message Level " + MIDText.GetTextOnly((int)ExitMessageLevel) + ". Duration: " + GetDuration(), "CommandThreadProcessor");
			}
		}

	}

	public class ChainWeeklyHistoryPurgeProcess : PurgeProcess
	{
		//=======
		// FIELDS
		//=======
		
		//=============
		// CONSTRUCTORS
		//=============

        public ChainWeeklyHistoryPurgeProcess(SessionAddressBlock aSAB, Audit aAudit, int aCommitLimit, bool aPurgeRowsWithAllZeros, DateTime aShutdownTime)
            : base(aSAB, aAudit, eVariableDataType.chainWeeklyHistory, aCommitLimit, aPurgeRowsWithAllZeros, aShutdownTime)
		{
			try
			{
			}
			catch (Exception exc)
			{
				ExitMessageLevel = eMIDMessageLevel.Severe;
				Audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in Constructor", "CommandThreadProcessor");
				Audit.Log_Exception(exc, GetType().Name);
				throw;
			}
		}

		//===========
		// PROPERTIES
		//===========

		
		//========
		// METHODS
		//========

		override public void ExecuteProcess()
		{
			eMIDMessageLevel messageLevel = eMIDMessageLevel.Information;
			int recordsDeleted = CommitLimit + 1;  // initialize to non-zero start loop

			string message = "Executing purge for chain weekly history" ;
            StartTimer();
			try
			{
				IsRunning = true;

				Audit.Add_Msg(eMIDMessageLevel.Information, message, "PurgeProcess");

				try
				{
					try
					{
						DataTable dt = PurgeData.PurgeDates_ReadWeeklyHistoryDates();
                        // Begin TT#5210 - JSmith - Purge Performance
                        if (dt.Rows.Count == 0)	// nothing to do
                        {
                            return;
                        }
                        // End TT#5210 - JSmith - Purge Performance
						VarData.OpenUpdateConnection();
						switch (dt.Rows.Count)
						{
							case 0:	// nothing to do
								break;
							case 1:
								DataRow dr = dt.Rows[0];
								int purgeDate = (dr["PURGE_WEEKLY_HISTORY"] == System.DBNull.Value) ? Include.Undefined : Convert.ToInt32(dr["PURGE_WEEKLY_HISTORY"], CultureInfo.CurrentUICulture);
								if (purgeDate > Include.Undefined)
								{
									while (recordsDeleted >= CommitLimit)
									{
                                        if (StopTimeExceeded()) break;

										recordsDeleted = VarData.ChainWeeklyHistory_DeleteLessThanDate(purgeDate, CommitLimit);
										RecordsDeleted += recordsDeleted;
										VarData.CommitData();
									}
                                    // Begin TT#2131-MD - JSmith - Halo Integration
                                    if (SAB.ROExtractEnabled)
                                    {
                                        recordsDeleted = CommitLimit + 1;  // initialize to non-zero start loop
                                        while (recordsDeleted >= CommitLimit)
                                        {
                                            if (StopTimeExceeded()) break;

                                            recordsDeleted = VarData.EXTRACT_PLANNING_CONTROL_ChainHistoryDeleteLessThanDate(purgeDate, CommitLimit);
                                            VarData.CommitData();
                                        }
                                    }
                                    // End TT#2131-MD - JSmith - Halo Integration
                                }
                                break;
							default:
								while (recordsDeleted >= CommitLimit)
								{
                                    if (StopTimeExceeded()) break;

									recordsDeleted = VarData.ChainWeeklyHistory_DeleteLessThanDate(CommitLimit);
									RecordsDeleted += recordsDeleted;
									VarData.CommitData();
								}
                                // Begin TT#2131-MD - JSmith - Halo Integration
                                if (SAB.ROExtractEnabled)
                                {
                                    recordsDeleted = CommitLimit + 1;  // initialize to non-zero start loop
                                    while (recordsDeleted >= CommitLimit)
                                    {
                                        if (StopTimeExceeded()) break;

                                        recordsDeleted = VarData.EXTRACT_PLANNING_CONTROL_ChainHistoryDeleteLessThanDate(CommitLimit);
                                        VarData.CommitData();
                                    }
                                }
                                // End TT#2131-MD - JSmith - Halo Integration
                                break;
						}
						VarData.CommitData();
					}
					catch ( Exception ex )
					{
						++NumberOfErrors;
						Audit.Log_Exception(ex, GetType().Name);
					}
					finally
					{
						VarData.CloseUpdateConnection();
					}
				}
				catch ( Exception ex )
				{
					Audit.Log_Exception(ex, GetType().Name);
				}


			}
			catch (ThreadAbortException)
			{
				try
				{
					Audit.Add_Msg(eMIDMessageLevel.Warning, "Cancelled by User", "CommandThreadProcessor");

					messageLevel = eMIDMessageLevel.Severe;
				}
				catch (InvalidOperationException)
				{
					messageLevel = eMIDMessageLevel.Severe;
				}
			}
			catch (Exception exc)
			{
				Audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in ExecuteProcess", "CommandThreadProcessor");
				Audit.Log_Exception(exc, GetType().Name);

				messageLevel = eMIDMessageLevel.Severe;
			}
			finally
			{
				IsRunning = false;
				CompletionDateTime = DateTime.Now;
				ExitMessageLevel = messageLevel;

                message = message.Replace("Executing purge", "Completed purge");
                Audit.Add_Msg(eMIDMessageLevel.Information, message + " with Message Level " + MIDText.GetTextOnly((int)ExitMessageLevel) + ". Duration: " + GetDuration(), "CommandThreadProcessor");
			}
		}

	}

	public class ChainWeeklyForecastPurgeProcess : PurgeProcess
	{
		//=======
		// FIELDS
		//=======

		//=============
		// CONSTRUCTORS
		//=============

        public ChainWeeklyForecastPurgeProcess(SessionAddressBlock aSAB, Audit aAudit, int aCommitLimit, bool aPurgeRowsWithAllZeros, DateTime aShutdownTime)
            : base(aSAB, aAudit, eVariableDataType.chainWeeklyForecast, aCommitLimit, aPurgeRowsWithAllZeros, aShutdownTime)
		{
            try
            {
            }
            catch (Exception exc)
            {
                ExitMessageLevel = eMIDMessageLevel.Severe;
                Audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in Constructor", "CommandThreadProcessor");
                Audit.Log_Exception(exc, GetType().Name);
                throw;
            }
		}

		//===========
		// PROPERTIES
		//===========

		
		//========
		// METHODS
		//========

		override public void ExecuteProcess()
		{
			eMIDMessageLevel messageLevel = eMIDMessageLevel.Information;
			int recordsDeleted = CommitLimit + 1;  // initialize to non-zero start loop

			string message = "Executing purge for chain weekly forecast" ;
            StartTimer();
			try
			{
				IsRunning = true;

				Audit.Add_Msg(eMIDMessageLevel.Information, message, "PurgeProcess");

				try
				{
					try
					{
						DataTable dt = PurgeData.PurgeDates_ReadPlanDates();
                        // Begin TT#5210 - JSmith - Purge Performance
                        if (dt.Rows.Count == 0)	// nothing to do
                        {
                            return;
                        }
                        // End TT#5210 - JSmith - Purge Performance
						VarData.OpenUpdateConnection();
						switch (dt.Rows.Count)
						{
							case 0:	// nothing to do
								break;
							case 1:
								DataRow dr = dt.Rows[0];
								int purgeDate = (dr["PURGE_PLANS"] == System.DBNull.Value) ? Include.Undefined : Convert.ToInt32(dr["PURGE_PLANS"], CultureInfo.CurrentUICulture);
								if (purgeDate > Include.Undefined)
								{
									while (recordsDeleted >= CommitLimit)
									{
                                        if (StopTimeExceeded()) break;

										recordsDeleted = VarData.ChainPlans_DeleteLessThanDate(purgeDate, CommitLimit);
										RecordsDeleted += recordsDeleted;
										VarData.CommitData();
									}
                                    // Begin TT#2131-MD - JSmith - Halo Integration
                                    if (SAB.ROExtractEnabled)
                                    {
                                        recordsDeleted = CommitLimit + 1;  // initialize to non-zero start loop
                                        while (recordsDeleted >= CommitLimit)
                                        {
                                            if (StopTimeExceeded()) break;

                                            recordsDeleted = VarData.EXTRACT_PLANNING_CONTROL_ChainForecastDeleteLessThanDate(purgeDate, CommitLimit);
                                            VarData.CommitData();
                                        }
                                    }
                                    // End TT#2131-MD - JSmith - Halo Integration
                                }
                                break;
							default:
								while (recordsDeleted >= CommitLimit)
								{
                                    if (StopTimeExceeded()) break;

									recordsDeleted = VarData.ChainPlans_DeleteLessThanDate(CommitLimit);
									RecordsDeleted += recordsDeleted;
									VarData.CommitData();
								}
                                // Begin TT#2131-MD - JSmith - Halo Integration
                                if (SAB.ROExtractEnabled)
                                {
                                    recordsDeleted = CommitLimit + 1;  // initialize to non-zero start loop
                                    while (recordsDeleted >= CommitLimit)
                                    {
                                        if (StopTimeExceeded()) break;

                                        recordsDeleted = VarData.EXTRACT_PLANNING_CONTROL_ChainForecastDeleteLessThanDate(CommitLimit);
                                        VarData.CommitData();
                                    }
                                }
                                // End TT#2131-MD - JSmith - Halo Integration
                                break;
						}
						VarData.CommitData();
					}
					catch ( Exception ex )
					{
						++NumberOfErrors;
						Audit.Log_Exception(ex, GetType().Name);
					}
					finally
					{
						VarData.CloseUpdateConnection();
					}
				}
				catch ( Exception ex )
				{
					Audit.Log_Exception(ex, GetType().Name);
				}


			}
			catch (ThreadAbortException)
			{
				try
				{
					Audit.Add_Msg(eMIDMessageLevel.Warning, "Cancelled by User", "CommandThreadProcessor");

					messageLevel = eMIDMessageLevel.Severe;
				}
				catch (InvalidOperationException)
				{
					messageLevel = eMIDMessageLevel.Severe;
				}
			}
			catch (Exception exc)
			{
				Audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in ExecuteProcess", "CommandThreadProcessor");
				Audit.Log_Exception(exc, GetType().Name);

				messageLevel = eMIDMessageLevel.Severe;
			}
			finally
			{
				IsRunning = false;
				CompletionDateTime = DateTime.Now;
				ExitMessageLevel = messageLevel;

                message = message.Replace("Executing purge", "Completed purge");
                Audit.Add_Msg(eMIDMessageLevel.Information, message + " with Message Level " + MIDText.GetTextOnly((int)ExitMessageLevel) + ". Duration: " + GetDuration(), "CommandThreadProcessor");
			}
		}

	}

	public class ChainWeeklyForecastLockPurgeProcess : PurgeProcess
	{
		//=======
		// FIELDS
		//=======
        bool _purgeRowsWithAllUnlocked;

		
		//=============
		// CONSTRUCTORS
		//=============

        public ChainWeeklyForecastLockPurgeProcess(SessionAddressBlock aSAB, Audit aAudit, int aCommitLimit, bool aPurgeRowsWithAllZeros, bool aPurgeRowsWithAllUnlocked, DateTime aShutdownTime)
            : base(aSAB, aAudit, eVariableDataType.chainWeeklyForecast, aCommitLimit, aPurgeRowsWithAllZeros, aShutdownTime)
		{
			try
			{
                _purgeRowsWithAllUnlocked = aPurgeRowsWithAllUnlocked;

			}
			catch (Exception exc)
			{
				ExitMessageLevel = eMIDMessageLevel.Severe;
				Audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in Constructor", "CommandThreadProcessor");
				Audit.Log_Exception(exc, GetType().Name);
				throw;
			}
		}

		//===========
		// PROPERTIES
		//===========

		
		//========
		// METHODS
		//========

		override public void ExecuteProcess()
		{
			eMIDMessageLevel messageLevel = eMIDMessageLevel.Information;
			int recordsDeleted = CommitLimit + 1;  // initialize to non-zero start loop

			string message = "Executing purge for chain weekly forecast locks" ;
            StartTimer();
			try
			{
				IsRunning = true;

				Audit.Add_Msg(eMIDMessageLevel.Information, message, "PurgeProcess");

				try
				{
					try
					{
						DataTable dt = PurgeData.PurgeDates_ReadPlanDates();
                        // Begin TT#5210 - JSmith - Purge Performance
                        if (dt.Rows.Count == 0)	// nothing to do
                        {
                            return;
                        }
                        // End TT#5210 - JSmith - Purge Performance
						VarData.OpenUpdateConnection();
						switch (dt.Rows.Count)
						{
							case 0:	// nothing to do
								break;
							case 1:
								DataRow dr = dt.Rows[0];
								int purgeDate = (dr["PURGE_PLANS"] == System.DBNull.Value) ? Include.Undefined : Convert.ToInt32(dr["PURGE_PLANS"], CultureInfo.CurrentUICulture);
								if (purgeDate > Include.Undefined)
								{
									while (recordsDeleted >= CommitLimit)
									{
                                        if (StopTimeExceeded()) break;

										recordsDeleted = VarData.ChainPlanLocks_DeleteLessThanDate(purgeDate, CommitLimit);
										RecordsDeleted += recordsDeleted;
										VarData.CommitData();
									}
								}
								break;
							default:
								while (recordsDeleted >= CommitLimit)
								{
                                    if (StopTimeExceeded()) break;

									recordsDeleted = VarData.ChainPlanLocks_DeleteLessThanDate(CommitLimit);
									RecordsDeleted += recordsDeleted;
									VarData.CommitData();
								}
								break;
						}
                        if (!StopTimeExceeded())
                        {
                            if (PurgeRowsWithAllZeros)
                            {
                                recordsDeleted = CommitLimit + 1;
                                while (recordsDeleted >= CommitLimit)
                                {
                                    if (StopTimeExceeded()) break;

                                    recordsDeleted = VarData.ChainPlans_ZeroRows(CommitLimit);
                                    VarData.CommitData();
                                }
                            }
                        }

                        if (!StopTimeExceeded())
                        {
                            if (_purgeRowsWithAllUnlocked)
                            {
                                recordsDeleted = CommitLimit + 1;
                                while (recordsDeleted >= CommitLimit)
                                {
                                    if (StopTimeExceeded()) break;

                                    recordsDeleted = VarData.ChainPlans_DeleteAllUnlocked(CommitLimit);
                                    VarData.CommitData();
                                }
                            }
                        }

						VarData.CommitData();
					}
					catch ( Exception ex )
					{
						++NumberOfErrors;
						Audit.Log_Exception(ex, GetType().Name);
					}
					finally
					{
						VarData.CloseUpdateConnection();
					}
				}
				catch ( Exception ex )
				{
					Audit.Log_Exception(ex, GetType().Name);
				}


			}
			catch (ThreadAbortException)
			{
				try
				{
					Audit.Add_Msg(eMIDMessageLevel.Warning, "Cancelled by User", "CommandThreadProcessor");

					messageLevel = eMIDMessageLevel.Severe;
				}
				catch (InvalidOperationException)
				{
					messageLevel = eMIDMessageLevel.Severe;
				}
			}
			catch (Exception exc)
			{
				Audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in ExecuteProcess", "CommandThreadProcessor");
				Audit.Log_Exception(exc, GetType().Name);

				messageLevel = eMIDMessageLevel.Severe;
			}
			finally
			{
				IsRunning = false;
				CompletionDateTime = DateTime.Now;
				ExitMessageLevel = messageLevel;

                message = message.Replace("Executing purge", "Completed purge");
                Audit.Add_Msg(eMIDMessageLevel.Information, message + " with Message Level " + MIDText.GetTextOnly((int)ExitMessageLevel) + ". Duration: " + GetDuration(), "CommandThreadProcessor");
			}
		}

	}

	public class AuditForecastPurgeProcess : PurgeProcess
	{
		//=======
		// FIELDS
		//=======
		
		//=============
		// CONSTRUCTORS
		//=============

        public AuditForecastPurgeProcess(SessionAddressBlock aSAB, Audit aAudit, int aCommitLimit, bool aPurgeRowsWithAllZeros, DateTime aShutdownTime)
            : base(aSAB, aAudit, eVariableDataType.none, aCommitLimit, aPurgeRowsWithAllZeros, aShutdownTime)
		{
			try
			{
			}
			catch (Exception exc)
			{
				ExitMessageLevel = eMIDMessageLevel.Severe;
				Audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in Constructor", "CommandThreadProcessor");
				Audit.Log_Exception(exc, GetType().Name);
				throw;
			}
		}

		//===========
		// PROPERTIES
		//===========

		
		//========
		// METHODS
		//========

		override public void ExecuteProcess()
		{
			AuditData auditData;
			eMIDMessageLevel messageLevel = eMIDMessageLevel.Information;
			int recordsDeleted = 0;
			int commitCount = 0;

			string message = "Executing purge for forecast audit information" ;
            StartTimer();
			try
			{
				IsRunning = true;

				Audit.Add_Msg(eMIDMessageLevel.Information, message, "PurgeProcess");
				auditData = new AuditData();

				try
				{
					try
					{
						DataTable dt = PurgeData.PurgeDates_ReadAuditForecasts();
						auditData.OpenUpdateConnection();
						foreach (DataRow dr in dt.Rows)
						{
                            if (StopTimeExceeded()) break;

							auditData.ForecastAuditForecast_Delete(Convert.ToInt32(dr["AUDIT_FORECAST_RID"], CultureInfo.CurrentUICulture));
							++recordsDeleted;
							++commitCount;
							if (commitCount > CommitLimit)
							{
								auditData.CommitData();
								commitCount = 0;
							}
						}

						auditData.CommitData();
					}
					catch ( Exception ex )
					{
						++NumberOfErrors;
						Audit.Log_Exception(ex, GetType().Name);
					}
					finally
					{
						auditData.CloseUpdateConnection();
					}
				}
				catch ( Exception ex )
				{
					Audit.Log_Exception(ex, GetType().Name);
				}


			}
			catch (ThreadAbortException)
			{
				try
				{
					Audit.Add_Msg(eMIDMessageLevel.Warning, "Cancelled by User", "CommandThreadProcessor");

					messageLevel = eMIDMessageLevel.Severe;
				}
				catch (InvalidOperationException)
				{
					messageLevel = eMIDMessageLevel.Severe;
				}
			}
			catch (Exception exc)
			{
				Audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in ExecuteProcess", "CommandThreadProcessor");
				Audit.Log_Exception(exc, GetType().Name);

				messageLevel = eMIDMessageLevel.Severe;
			}
			finally
			{
				IsRunning = false;
				CompletionDateTime = DateTime.Now;
				ExitMessageLevel = messageLevel;

                message = message.Replace("Executing purge", "Completed purge");
                Audit.Add_Msg(eMIDMessageLevel.Information, message + " with Message Level " + MIDText.GetTextOnly((int)ExitMessageLevel) + ". Duration: " + GetDuration(), "CommandThreadProcessor");
			}
		}

	}

	public class AuditModifiedSalesPurgeProcess : PurgeProcess
	{
		//=======
		// FIELDS
		//=======
		
		//=============
		// CONSTRUCTORS
		//=============

        public AuditModifiedSalesPurgeProcess(SessionAddressBlock aSAB, Audit aAudit, int aCommitLimit, bool aPurgeRowsWithAllZeros, DateTime aShutdownTime)
            : base(aSAB, aAudit, eVariableDataType.none, aCommitLimit, aPurgeRowsWithAllZeros, aShutdownTime)
		{
			try
			{
			}
			catch (Exception exc)
			{
				ExitMessageLevel = eMIDMessageLevel.Severe;
				Audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in Constructor", "CommandThreadProcessor");
				Audit.Log_Exception(exc, GetType().Name);
				throw;
			}
		}

		//===========
		// PROPERTIES
		//===========

		
		//========
		// METHODS
		//========

		override public void ExecuteProcess()
		{
			AuditData auditData;
			eMIDMessageLevel messageLevel = eMIDMessageLevel.Information;
			int recordsDeleted = 0;
			int commitCount = 0;

			string message = "Executing purge for modified sales audit information" ;
            StartTimer();
			try
			{
				IsRunning = true;

				Audit.Add_Msg(eMIDMessageLevel.Information, message, "PurgeProcess");
				auditData = new AuditData();

				try
				{
					try
					{
						DataTable dt = PurgeData.PurgeDates_ReadAuditModifiedSales();
						auditData.OpenUpdateConnection();
						foreach (DataRow dr in dt.Rows)
						{
                            if (StopTimeExceeded()) break;

							auditData.ForecastAuditForecast_Delete(Convert.ToInt32(dr["AUDIT_FORECAST_RID"], CultureInfo.CurrentUICulture));
							++recordsDeleted;
							++commitCount;
							if (commitCount > CommitLimit)
							{
								auditData.CommitData();
								commitCount = 0;
							}
						}

						auditData.CommitData();
					}
					catch ( Exception ex )
					{
						++NumberOfErrors;
						Audit.Log_Exception(ex, GetType().Name);
					}
					finally
					{
						auditData.CloseUpdateConnection();
					}
				}
				catch ( Exception ex )
				{
					Audit.Log_Exception(ex, GetType().Name);
				}


			}
			catch (ThreadAbortException)
			{
				try
				{
					Audit.Add_Msg(eMIDMessageLevel.Warning, "Cancelled by User", "CommandThreadProcessor");

					messageLevel = eMIDMessageLevel.Severe;
				}
				catch (InvalidOperationException)
				{
					messageLevel = eMIDMessageLevel.Severe;
				}
			}
			catch (Exception exc)
			{
				Audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in ExecuteProcess", "CommandThreadProcessor");
				Audit.Log_Exception(exc, GetType().Name);

				messageLevel = eMIDMessageLevel.Severe;
			}
			finally
			{
				IsRunning = false;
				CompletionDateTime = DateTime.Now;
				ExitMessageLevel = messageLevel;

                message = message.Replace("Executing purge", "Completed purge");
                Audit.Add_Msg(eMIDMessageLevel.Information, message + " with Message Level " + MIDText.GetTextOnly((int)ExitMessageLevel) + ". Duration: " + GetDuration(), "CommandThreadProcessor");
			}
		}
	}

    public class StoreDailyPercentagesPurgeProcess : PurgeProcess
    {
        //=======
        // FIELDS
        //=======

        //=============
        // CONSTRUCTORS
        //=============

        public StoreDailyPercentagesPurgeProcess(SessionAddressBlock aSAB, Audit aAudit, int aCommitLimit, bool aPurgeRowsWithAllZeros, DateTime aShutdownTime)
            : base(aSAB, aAudit, eVariableDataType.none, aCommitLimit, aPurgeRowsWithAllZeros, aShutdownTime)
        {
            try
            {
            }
            catch (Exception exc)
            {
                ExitMessageLevel = eMIDMessageLevel.Severe;
                Audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in Constructor", "CommandThreadProcessor");
                Audit.Log_Exception(exc, GetType().Name);
                throw;
            }
        }

        //===========
        // PROPERTIES
        //===========


        //========
        // METHODS
        //========

        override public void ExecuteProcess()
        {
            DailyPercentagesCriteriaData dailyPercentagesData;
            eMIDMessageLevel messageLevel = eMIDMessageLevel.Information;
            int recordsDeleted = CommitLimit + 1;
            string message = "Executing purge for store daily percentages information";
            StartTimer();
            try
            {
                IsRunning = true;

                Audit.Add_Msg(eMIDMessageLevel.Information, message, "PurgeProcess");
                dailyPercentagesData = new DailyPercentagesCriteriaData();
                
                try
                {
                    try
                    {
                        dailyPercentagesData.OpenUpdateConnection();
                        recordsDeleted = dailyPercentagesData.DailyPercentagesCriteria_DeleteSP(CommitLimit);
                        RecordsDeleted += recordsDeleted;
                        dailyPercentagesData.CommitData();
                    }
                    catch (Exception ex)
                    {
                        ++NumberOfErrors;
                        Audit.Log_Exception(ex, GetType().Name);
                    }
                    finally
                    {
                        dailyPercentagesData.CloseUpdateConnection();
                    }
                }
                catch (Exception ex)
                {
                    Audit.Log_Exception(ex, GetType().Name);
                }
            }
            catch (ThreadAbortException)
            {
                try
                {
                    Audit.Add_Msg(eMIDMessageLevel.Warning, "Cancelled by User", "CommandThreadProcessor");

                    messageLevel = eMIDMessageLevel.Severe;
                }
                catch (InvalidOperationException)
                {
                    messageLevel = eMIDMessageLevel.Severe;
                }
            }
            catch (Exception exc)
            {
                Audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in ExecuteProcess", "CommandThreadProcessor");
                Audit.Log_Exception(exc, GetType().Name);

                messageLevel = eMIDMessageLevel.Severe;
            }
            finally
            {
                IsRunning = false;
                CompletionDateTime = DateTime.Now;
                ExitMessageLevel = messageLevel;

                message = message.Replace("Executing purge", "Completed purge");
                Audit.Add_Msg(eMIDMessageLevel.Information, message + " with Message Level " + MIDText.GetTextOnly((int)ExitMessageLevel) + ". Duration: " + GetDuration(), "CommandThreadProcessor");
            }
        }
    }
}
