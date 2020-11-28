using System;
using System.Configuration;
using System.Collections;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Collections.Generic;
using System.Data;

using MIDRetail.Common;
using MIDRetail.DataCommon;
using MIDRetail.Data;

namespace MIDRetail.Business
{
	/// <summary>
	/// Summary description for Global.
	/// Base class for server global classes.
	/// </summary>
	public class Global
	{
		//Begin Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.
		static private ArrayList _loadLock;
		static private bool _loaded;

		//End Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.
		static private MRSCalendar _calendar;
		static private DateTime _calendarRefreshDate;
		static private int _readerLockTimeOut;
		static private int _writerLockTimeOut;
		//Begin Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.
		//static private Audit _audit = null;
		//static private bool _failed = false;
		//static private ArrayList _audits = new ArrayList();
		//End Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.

		// memory tracking
		static private Process _firstProcess = null;
		static private long _firstTotalMemory;
		static private Process _lastProcess = null;
		static private long _lastTotalMemory;

        // Begin TT#2307 - JSmith - Incorrect Stock Values
        static private bool _executingLocal;
        static private eMIDMessageSenderRecepient _recepient;
        static private MessageProcessor _messageProcessor;
        // End TT#2307

        // Begin TT#195 MD - JSmith - Add environment authentication
        static private ServiceProfile _serviceProfile;
        static private UpgradeProfile _upgradeProfile;
        static private eProcesses _process = eProcesses.unknown;
        static private string _configuration;
        // End TT#195 MD

        static private string _ROExtractConnectionString = null;   // TT#2131-MD - JSmith - Halo Integration
        static private bool _ROExtractEnabled = false;    // TT#2131-MD - JSmith - Halo Integration
        static private GlobalOptionsProfile _gop = null;    // TT#2131-MD - JSmith - Halo Integration

		/// <summary>
		/// Gets the Calendar.
		/// </summary>
		static internal MRSCalendar Calendar
		{
			get { return _calendar; }
			set { _calendar = value; }
		}

		static internal DateTime CalendarRefreshDate
		{
			get { return _calendarRefreshDate; }
			set { _calendarRefreshDate = value; }
		}

		static internal int ReaderLockTimeOut
		{
			get { return _readerLockTimeOut; }
		}

		static internal int WriterLockTimeOut
		{
			get { return _writerLockTimeOut; }
		}

        // Begin TT#2307 - JSmith - Incorrect Stock Values
        static public MessageProcessor MessageProcessor
        {
            get { return _messageProcessor; }
        }

        static public bool isExecutingLocal
        {
            get { return _executingLocal; }
        }
        // End TT#2307

        // Begin TT#195 MD - JSmith - Add environment authentication
        static public ServiceProfile ServiceProfile
        {
            get { return _serviceProfile; }
        }

        static public UpgradeProfile UpgradeProfile
        {
            get { return _upgradeProfile; }
        }

        static public string Configuration
        {
            get { return _configuration; }
        }
        // End TT#195 MD

        // Begin TT#2131-MD - JSmith - Halo Integration
        static public string ROExtractConnectionString
        {
            get { return _ROExtractConnectionString; }
        }

        static public bool ROExtractEnabled
        {
            get { return _ROExtractEnabled; }
        }

        static public GlobalOptionsProfile GlobalOptions
        {
            get
            {
                if (_gop == null)
                {
                    _gop = new GlobalOptionsProfile(-1);
                    _gop.LoadOptions();
                }
                return _gop;
            }
        }
        // End TT#2131-MD - JSmith - Halo Integration

        static Global()
		{
			try
			{
				_loadLock = new ArrayList();
				_loaded = false;
			}
			catch (Exception err)
			{
				string message = err.ToString();
				throw;
			}
		}

        /// <summary>
		/// This is used to load all base global areas.
		/// </summary>

        // Begin TT#2307 - JSmith - Incorrect Stock Values
        //static protected void LoadBase()
        static protected void LoadBase(eMIDMessageSenderRecepient aRecepient, int aMessagingInterval, bool aExecutingLocal, eProcesses aServiceID)
        // End TT#2307
		{
			try
			{
				//Begin Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.
				//_calendar = new MRSCalendar();
				//SetTimeouts();
				//CreateAudit(aProcess);
				lock (_loadLock.SyncRoot)
				{
					if (!_loaded)
					{
                        // Begin TT#195 MD - JSmith - Add environment authentication
                        _process = aServiceID;
                        object[] customAttributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyConfigurationAttribute), true);
                        _configuration = string.IsNullOrEmpty(((AssemblyConfigurationAttribute)customAttributes[0]).Configuration) ? "Base" : ((AssemblyConfigurationAttribute)customAttributes[0]).Configuration;
                        _upgradeProfile = GetUpgradeInformation();
                        VerifyEnvironment(null);
                        // End TT#195 MD

						_calendar = new MRSCalendar();
						SetTimeouts();

                        // Begin TT#2307 - JSmith - Incorrect Stock Values
                        _executingLocal = aExecutingLocal;
                        _recepient = aRecepient;
                        _messageProcessor = new MessageProcessor(_recepient, aMessagingInterval);
                        // End TT#2307


                        //Load cache data here...
                        UserNameStorage.SetDelegate_GetUnknownUserName(new UserNameStorage.GetUnknownUserNameDelegate(GetUnknownUserName));
                        PopulateUserNameStorageCache(); //TT#827-MD -jsobek -Allocation Reviews Performance

                        // Begin TT#2131-MD - JSmith - Halo Integration
                        _ROExtractEnabled = false;
                        _ROExtractConnectionString = MIDConfigurationManager.AppSettings["ROExtractConnectionString"];
                        if (!string.IsNullOrWhiteSpace(_ROExtractConnectionString)
                            && GlobalOptions.AppConfig.AnalyticsInstalled)
                        {
                            try
                            {
                                ROExtractData ROExtractData = new ROExtractData(_ROExtractConnectionString);
                                if (ROExtractData.RO_Extract_Exists())
                                {
                                    _ROExtractEnabled = true;
                                }
                            }
                            catch
                            {
                                EventLog.WriteEntry("MIDRetail", "Not an Analytics database.  Extract will not be allowed.", EventLogEntryType.Error);
                            }
                        }

                        // End TT#2131-MD - JSmith - Halo Integration

						_loaded = true;
					}
				}
				//End Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.
			}
			catch
			{
				throw;
			}
		}

        // Begin TT#4483 - JSmith - Modify Control Service start to not update other running services as Unexpected Termination
        static protected void MarkRunningProcesses(eProcesses aServiceID)
        {
            AuditData auditData = new AuditData();
            try
            {
                auditData.OpenUpdateConnection();
                auditData.MarkAllRunningForProcessAsUnexpectedTermination(aServiceID);
                auditData.CommitData();
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("MIDService", ex.Message, EventLogEntryType.Error);
                throw new MIDException(eErrorLevel.severe, 0, "MIDService: MarkRunningProcesses encountered error - " + ex.Message);
            }
            finally
            {
                auditData.CloseUpdateConnection();
            }
        }
        // End TT#4483 - JSmith - Modify Control Service start to not update other running services as Unexpected Termination

        //Begin TT#827-MD -jsobek -Allocation Reviews Performance
        /// <summary>
        /// This should get called for all services and the remote client session
        /// </summary>
        private static void PopulateUserNameStorageCache()
        {
            lock (_loadLock.SyncRoot)
            {
                if (!_loaded)
                {
                    SecurityAdmin secAdmin = new SecurityAdmin();
                    DataTable dtUsers = secAdmin.GetUserNameStorageCache();
                    UserNameStorage.PopulateUserNameStorageCache(dtUsers);
                }
            }
        }
        private static string GetUnknownUserName(int userRID)
        {
            SecurityAdmin secAdmin = new SecurityAdmin();
            return secAdmin.GetUserNameFromRID(userRID);
        }
        //End TT#827-MD -jsobek -Allocation Reviews Performance


        // Begin TT#195 MD - JSmith - Add environment authentication
        static protected void LoadEnvironmentInformation(eProcesses aServiceID)
        // End TT#2307
        {
            try
            {
                lock (_loadLock.SyncRoot)
                {
                    if (!_loaded)
                    {
                        _process = aServiceID;
                        object[] customAttributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyConfigurationAttribute), true);
                        _configuration = string.IsNullOrEmpty(((AssemblyConfigurationAttribute)customAttributes[0]).Configuration) ? "Base" : ((AssemblyConfigurationAttribute)customAttributes[0]).Configuration;
                        _upgradeProfile = GetUpgradeInformation();
                        VerifyEnvironment(null);

                        _loaded = true;
                    }
                }
                //End Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.
            }
            catch
            {
                throw;
            }
        }
        // End TT#195 MD

        // Begin TT#2307 - JSmith - Incorrect Stock Values
        static protected void StartMessageListener()
        {
            try
            {
                _messageProcessor.StartMessageListener();
            }
            catch
            {
                throw;
            }
        }

        static protected void StopMessageListener()
        {
            try
            {
                _messageProcessor.StopMessageListener();
            }
            catch
            {
                throw;
            }
        }

        static protected bool RemoveMessage(int aMessageRID)
        {
            try
            {
                return _messageProcessor.RemoveMessage(aMessageRID);
            }
            catch
            {
                throw;
            }
        }
        // End TT#2307

        static protected void SetInitialMemoryCounts()
		{
			try
			{
				System.GC.Collect();
				_firstProcess = System.Diagnostics.Process.GetCurrentProcess();
				_firstTotalMemory = System.GC.GetTotalMemory(true);
				_lastProcess = System.Diagnostics.Process.GetCurrentProcess();
				_lastTotalMemory = System.GC.GetTotalMemory(true);
			}
			catch
			{
				throw;
			}
		}

		static public string ShowMemory()
		{
			try
			{
				System.GC.Collect();
				bool monitorMemory = false;
				string strMonitor = MIDConfigurationManager.AppSettings["MonitorMemory"];
				if (strMonitor != null)
				{
					try
					{
						monitorMemory = Convert.ToBoolean(strMonitor);
					}
					catch
					{
					}
				
				}
				string memory = string.Empty;
				long totalMemory = System.GC.GetTotalMemory(true);
				Process process = System.Diagnostics.Process.GetCurrentProcess();
				if (_lastProcess == null)
				{
					memory = "TotalMemorySize=" + totalMemory.ToString() + System.Environment.NewLine
						+ "ProcessName=" + process.ProcessName.ToString() + System.Environment.NewLine
						+ "VirtualMemorySize=" + process.VirtualMemorySize64.ToString() + System.Environment.NewLine
						+ "WorkingSet=" + process.WorkingSet64.ToString() + System.Environment.NewLine
						+ "PagedMemorySize=" + process.PagedMemorySize64.ToString() + System.Environment.NewLine
						+ "PagedSystemMemorySize=" + process.PagedSystemMemorySize64.ToString() + System.Environment.NewLine
						+ "PrivateMemorySize=" + process.PrivateMemorySize64.ToString() + System.Environment.NewLine
						+ "PeakWorkingSet=" + process.PeakWorkingSet64.ToString() + System.Environment.NewLine
						+ "PeakPagedMemorySize=" + process.PeakPagedMemorySize64.ToString() + System.Environment.NewLine
						+ "PeakVirtualMemorySize=" + process.PeakVirtualMemorySize64.ToString();
				}
				else
				{
					memory = "TotalMemorySize=" + totalMemory.ToString()  + "(" + ((long)(totalMemory - _lastTotalMemory)).ToString() + ")" + "(" + ((long)(totalMemory - _firstTotalMemory)).ToString() + ")" + System.Environment.NewLine
						+ "ProcessName=" + process.ProcessName.ToString() + System.Environment.NewLine
						+ "VirtualMemorySize=" + process.VirtualMemorySize64.ToString() + "(" + ((int)(process.VirtualMemorySize64 - _lastProcess.VirtualMemorySize64)).ToString() + ")" + "(" + ((int)(process.VirtualMemorySize64 - _firstProcess.VirtualMemorySize64)).ToString() + ")" + System.Environment.NewLine
						+ "WorkingSet=" + process.WorkingSet64.ToString() + "(" + ((int)(process.WorkingSet64 - _lastProcess.WorkingSet64)).ToString() + ")" + "(" + ((int)(process.WorkingSet64 - _firstProcess.WorkingSet64)).ToString() + ")" + System.Environment.NewLine
						+ "PagedMemorySize=" + process.PagedMemorySize64.ToString() + "(" + ((int)(process.PagedMemorySize64 - _lastProcess.PagedMemorySize64)).ToString() + ")" + "(" + ((int)(process.PagedMemorySize64 - _firstProcess.PagedMemorySize64)).ToString() + ")" + System.Environment.NewLine
						+ "PagedSystemMemorySize=" + process.PagedSystemMemorySize64.ToString() + "(" + ((int)(process.PagedSystemMemorySize64 - _lastProcess.PagedSystemMemorySize64)).ToString() + ")" + "(" + ((int)(process.PagedSystemMemorySize64 - _firstProcess.PagedSystemMemorySize64)).ToString() + ")" + System.Environment.NewLine
						+ "PrivateMemorySize=" + process.PrivateMemorySize64.ToString() + "(" + ((int)(process.PrivateMemorySize64 - _lastProcess.PrivateMemorySize64)).ToString() + ")" + "(" + ((int)(process.PrivateMemorySize64 - _firstProcess.PrivateMemorySize64)).ToString() + ")" + System.Environment.NewLine
						+ "PeakWorkingSet=" + process.PeakWorkingSet64.ToString() + "(" + ((int)(process.PeakWorkingSet64 - _lastProcess.PeakWorkingSet64)).ToString() + ")" + "(" + ((int)(process.PeakWorkingSet64 - _firstProcess.PeakWorkingSet64)).ToString() + ")" + System.Environment.NewLine
						+ "PeakPagedMemorySize=" + process.PeakPagedMemorySize64.ToString() + "(" + ((int)(process.PeakPagedMemorySize64 - _lastProcess.PeakPagedMemorySize64)).ToString() + ")" + "(" + ((int)(process.PeakPagedMemorySize64 - _firstProcess.PeakPagedMemorySize64)).ToString() + ")" + System.Environment.NewLine
						+ "PeakVirtualMemorySize=" + process.PeakVirtualMemorySize64.ToString() + "(" + ((int)(process.PeakVirtualMemorySize64 - _lastProcess.PeakVirtualMemorySize64)).ToString() + ")" + "(" + ((int)(process.PeakVirtualMemorySize64 - _firstProcess.PeakVirtualMemorySize64)).ToString() + ")";
				}
				if (monitorMemory)
				{
					EventLog.WriteEntry("MIDService", memory, EventLogEntryType.Information);
				}
				_lastProcess = process;
				_lastTotalMemory = totalMemory;
				return memory;
			}
			catch
			{
				throw;
			}
		}

		static private void SetTimeouts()
		{
			try
			{
				// set lock timeouts
				if (MIDConfigurationManager.AppSettings["ReaderLockTimeOut"] == null)
				{
					_readerLockTimeOut = 10;
				}
				else
				{
					try
					{
						_readerLockTimeOut = Convert.ToInt32(MIDConfigurationManager.AppSettings["ReaderLockTimeOut"], CultureInfo.CurrentUICulture);
						if (_readerLockTimeOut < -1)
						{
							_readerLockTimeOut = 10;
						}
					}
					catch
					{
						_readerLockTimeOut = 10;
					}
				}

				if (MIDConfigurationManager.AppSettings["WriterLockTimeOut"] == null)
				{
					_writerLockTimeOut = 100;
				}
				else
				{
					try
					{
						_writerLockTimeOut = Convert.ToInt32(MIDConfigurationManager.AppSettings["WriterLockTimeOut"], CultureInfo.CurrentUICulture);
						if (_writerLockTimeOut < -1)
						{
							_writerLockTimeOut = 10;
						}
					}
					catch
					{
						_writerLockTimeOut = 100;
					}
				}
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Method that routinely colllect memory.
		/// </summary>
		static public void GarbageCollect()
		{
			try
			{
				System.GC.Collect();
			}
			catch (Exception e)
			{
				EventLog.WriteEntry("MIDService", "Error encountered -- " + e.Message, EventLogEntryType.Error);
				return;
			}
		}

        // Begin TT#195 MD - JSmith - Add environment authentication
        /// <summary>
        /// Registers the service start to the database.
        /// </summary>
        static public void RegisterServiceStart()
        {
            EnvironmentData ed = null;
            try
            {
                _serviceProfile = new ServiceProfile((int)_process);
                System.Diagnostics.FileVersionInfo fvi = System.Diagnostics.FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetEntryAssembly().Location);

                _serviceProfile.StartDateTime = DateTime.Now;
                _serviceProfile.Version = fvi.FileVersion;
                _serviceProfile.FileMajorPart = fvi.FileMajorPart;
                _serviceProfile.FileMinorPart = fvi.FileMinorPart;
                _serviceProfile.FileBuildPart = fvi.FileBuildPart;
                _serviceProfile.FilePrivatePart = fvi.FilePrivatePart;

                ed = new EnvironmentData();
                ed.OpenUpdateConnection();
                ed.ServiceStart_Add((int)_process, _serviceProfile.StartDateTime, _serviceProfile.Version);

                ed.CommitData();
            }
            catch (Exception e)
            {
                EventLog.WriteEntry("MIDService", "Error encountered -- " + e.Message, EventLogEntryType.Error);
                return;
            }
            finally
            {
                if (ed != null
                    && ed.ConnectionIsOpen)
                {
                    ed.CloseUpdateConnection();
                }
            }
        }
        public static void LoadSocketServerManager(Audit _audit)
        {
            //Begin TT#901-MD -jsobek -Batch Only Mode
            //string controlServerName = System.Net.Dns.GetHostName();
            //if (base.IsLocal == false)
            //{
                EventLog.WriteEntry("MIDControlService", "Attempting to get control service start options...", EventLogEntryType.Information);

                bool useBatchOnlyMode = false;
                bool defaultBatchOnlyModeOn = true;
                SecurityAdmin securityAdmin = new SecurityAdmin();
                securityAdmin.GetControlServiceStartOptions(out useBatchOnlyMode, out defaultBatchOnlyModeOn);
                EventLog.WriteEntry("MIDControlService", "Read start options." + System.Environment.NewLine + "Use Batch Only Mode: " + useBatchOnlyMode.ToString() + System.Environment.NewLine + "Default Batch Only Mode On: " + defaultBatchOnlyModeOn.ToString(), EventLogEntryType.Information);
                if (useBatchOnlyMode)
                {
                    string controlServerName;
                    int controlServerPort;
                    double serverTimerIntervalInMilliseconds;
                    double tmpInterval;
                    GetSocketSettingsFromConfigFile(out controlServerName, out controlServerPort, out tmpInterval, out serverTimerIntervalInMilliseconds);

                    serverSocketManager = new SocketServerManager();
                    if (defaultBatchOnlyModeOn)
                    {
                        serverSocketManager.IsApplicationInBatchOnlyMode = true;
                    }
                    else
                    {
                        serverSocketManager.IsApplicationInBatchOnlyMode = false;
                    }
                    serverSocketManager.StartListening(controlServerName, controlServerPort, serverTimerIntervalInMilliseconds, _audit);
                }
            //}
            //End TT#901-MD -jsobek -Batch Only Mode
        }
        //Begin TT#901-MD -jsobek -Batch Only Mode
        public static void GetSocketSettingsFromConfigFile(out string controlServerName, out int controlServerPort, out double clientTimerIntervalInMilliseconds, out double serverTimerIntervalInMilliseconds)
        {
            controlServerName = MIDConfigurationManager.AppSettings["RemoteOptions_ServerName"];
            string tempPort = MIDConfigurationManager.AppSettings["RemoteOptions_ServerPort"];
            string tempIntervalClient = MIDConfigurationManager.AppSettings["RemoteOptions_ClientInterval"];
            string tempIntervalServer = MIDConfigurationManager.AppSettings["RemoteOptions_ServerInterval"];
            //string tempserverLogEventsToEventViewer = MIDConfigurationManager.AppSettings["ServerCommandsLogToEventViewer"];
            //if (tempserverLogEventsToEventViewer.ToLower() == "true")
            //{
            //    serverLogEventsToEventViewer = true;
            //}
            //else
            //{
            //    serverLogEventsToEventViewer = false;
            //}

            int.TryParse(tempPort, out controlServerPort);
            double.TryParse(tempIntervalClient, out clientTimerIntervalInMilliseconds);
            double.TryParse(tempIntervalServer, out serverTimerIntervalInMilliseconds);
        }



        private static SocketServerManager serverSocketManager;

        public static bool IsApplicationInBatchOnlyMode()
        {
            if (serverSocketManager != null)
            {
                return serverSocketManager.IsApplicationInBatchOnlyMode;
            }
            else
            {
                return false;
            }
        }
        public static string BatchModeLastChangedBy()
        {
            if (serverSocketManager != null)
            {
                return serverSocketManager.BatchModeLastChangedBy;
            }
            else
            {
                return string.Empty;
            }
        }

        public static void StopSocketServerManager()
        {
            if (serverSocketManager != null)
            {
                serverSocketManager.StopListening();
            }
        }
       
     



        //End TT#901-MD -jsobek -Batch Only Mode
        static public UpgradeProfile GetUpgradeInformation()
        {
            lock (_loadLock.SyncRoot)
            {
                EnvironmentData ed = new EnvironmentData();
                return ed.GetUpgradeInformation();
            }
        }

        static public void VerifyEnvironment(ClientProfile aClientProfile)
        {
            // Do not verify if in debug mode
#if (DEBUG)
            return;
#endif

            lock (_loadLock.SyncRoot)
            {
                // verify client and service are using the same configuration
                if (aClientProfile != null &&
                    aClientProfile.Configuration != Configuration)
                {
                    WriteConfigurationMismatchEventLogEntry(aClientProfile.Configuration, Configuration);
                    throw new MIDException(eErrorLevel.severe,
                        (int)eMIDTextCode.msg_ConfigurationMismatch,
                        MIDText.GetText(eMIDTextCode.msg_ConfigurationMismatch));
                }

                // verify client and service have compatible versions
                if (aClientProfile != null && _serviceProfile != null &&
                    (aClientProfile.FileMajorPart != _serviceProfile.FileMajorPart ||
                    aClientProfile.FileMinorPart != _serviceProfile.FileMinorPart ||
                    aClientProfile.FileBuildPart != _serviceProfile.FileBuildPart))
                {
                    WriteClientServiceMismatchEventLogEntry(aClientProfile);
                    throw new MIDException(eErrorLevel.severe,
                        (int)eMIDTextCode.msg_ClientServiceMismatch,
                        MIDText.GetText(eMIDTextCode.msg_ClientServiceMismatch));
                }

                SystemData systemData = new SystemData();
                string DBName = systemData.GetDatabaseName();

                // verify client and service are using the same database
                if (aClientProfile != null &&
                    aClientProfile.DBName != DBName)
                {
                    WriteDBMismatchEventLogEntry(aClientProfile.DBName, DBName);
                    throw new MIDException(eErrorLevel.severe,
                        (int)eMIDTextCode.msg_DatabaseMismatch,
                        MIDText.GetText(eMIDTextCode.msg_DatabaseMismatch));
                }

                EnvironmentData ed = new EnvironmentData();
                UpgradeProfile up = ed.GetUpgradeInformation();
                ServiceProfile sp = ed.GetServicesInformation(_process);

                // verify service and database have compatible versions
                if (!up.Equals(UpgradeProfile) ||
                    (sp != null && ServiceProfile != null &&
                    !sp.Equals(ServiceProfile)))
                {
                    // Begin TT#2848 - JSmith - Error creating sessions on StoreLoad.exe
                    //WriteEventLogEntry(up, UpgradeProfile);
                    if (!up.Equals(UpgradeProfile))
                    {
                        WriteEventLogEntry(up, UpgradeProfile);
                    }
                    else
                    {
                        WriteEventLogEntry(sp, ServiceProfile);
                    }
                    // End TT#2848 - JSmith - Error creating sessions on StoreLoad.exe
                    throw new MIDException(eErrorLevel.severe,
                            (int)eMIDTextCode.msg_ServicesMismatch,
                            MIDText.GetText(eMIDTextCode.msg_ServicesMismatch));
                }

                // verify client and database have compatible versions
				string[] versionParts = up.UpgradeVersion.Split('.');
                if (aClientProfile !=  null &&
                    (aClientProfile.FileMajorPart.ToString() != versionParts[0] ||
                    aClientProfile.FileMinorPart.ToString() != versionParts[1] ||
                    aClientProfile.FileBuildPart.ToString() != versionParts[2]))
                {
                    WriteClientDatabaseMismatchEventLogEntry(aClientProfile, up);
                    throw new MIDException(eErrorLevel.severe,
                        (int)eMIDTextCode.msg_ClientDatabaseMismatch,
                        MIDText.GetText(eMIDTextCode.msg_ClientDatabaseMismatch));
                }
            }
        }

        static private void WriteDBMismatchEventLogEntry(string aClientDB, string aServiceDB)
        {
            EventLog.WriteEntry("MIDRetail", MIDText.GetTextOnly((int)_process)
                + " Database Mismatch; Client Database:" + aClientDB
                + ";Service Database:" + aServiceDB
                , EventLogEntryType.Error);
        }

        static private void WriteConfigurationMismatchEventLogEntry(string aClientConfiguration, string aServiceConfiguration)
        {
            EventLog.WriteEntry("MIDRetail", MIDText.GetTextOnly((int)_process)
                + " Configuration Mismatch; Client Configuration:" + aClientConfiguration
                + ";Service Configuration:" + aServiceConfiguration
                , EventLogEntryType.Error);
        }

        private static void WriteClientServiceMismatchEventLogEntry(ClientProfile aClientProfile)
        {
            EventLog.WriteEntry("MIDRetail", MIDText.GetTextOnly((int)_process)
                + " Client Service Mismatch; Client Version:" + aClientProfile.Version
                + ";Service Version:" + _serviceProfile.Version
                , EventLogEntryType.Error);
        }

        private static void WriteClientDatabaseMismatchEventLogEntry(ClientProfile aClientProfile, UpgradeProfile up)
        {
            EventLog.WriteEntry("MIDRetail", MIDText.GetTextOnly((int)_process)
               + " Client Database Mismatch; Client Version:" + aClientProfile.Version
               + ";Database Version:" + up.UpgradeVersion
               , EventLogEntryType.Error);
        }

        static private void WriteEventLogEntry(UpgradeProfile aDatabaseProfile, UpgradeProfile aServiceProfile)
        {
            EventLog.WriteEntry("MIDRetail", MIDText.GetTextOnly((int)_process)
                + ";Service Date:" + aServiceProfile.UpgradeDateTime.ToString()
                + ";Database Date:" + aDatabaseProfile.UpgradeDateTime.ToString()
                + ";Service Version:" + aServiceProfile.UpgradeVersion.ToString()
                + ";Registered Version:" + aDatabaseProfile.UpgradeVersion.ToString()
                + ";Service User:" + aServiceProfile.UpgradeUser.ToString()
                + ";Registered User:" + aDatabaseProfile.UpgradeUser.ToString()
                + ";Service Machine:" + aServiceProfile.UpgradeMachine.ToString()
                + ";Registered Machine:" + aDatabaseProfile.UpgradeMachine.ToString()
                + ";Service Configuration:" + aServiceProfile.UpgradeConfiguration.ToString()
                + ";Registered Configuration:" + aDatabaseProfile.UpgradeConfiguration.ToString()
                , EventLogEntryType.Error);
        }

        static private void WriteEventLogEntry(ServiceProfile aClientServiceProfile, ServiceProfile aServiceProfile)
        {
            if (aClientServiceProfile == null ||
                aServiceProfile == null)
            {
                return;
            }
            EventLog.WriteEntry("MIDRetail", MIDText.GetTextOnly(aClientServiceProfile.Key)
                + ";Service Start:" + aServiceProfile.StartDateTime.ToString()
                + ";Registered Start:" + aClientServiceProfile.StartDateTime.ToString()
                + ";Service Version:" + aServiceProfile.Version.ToString()
                + ";Registered Version:" + aClientServiceProfile.Version.ToString()
                , EventLogEntryType.Error);
        }
        // End TT#195 MD
	}
}
