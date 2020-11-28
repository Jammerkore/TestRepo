using System;
using System.IO;
using System.Configuration;
using System.Data;
using MIDRetail.DataCommon;
using MIDRetail.Data;
using System.Diagnostics;
using System.Globalization;
using System.Windows;
using System.Timers;


namespace MIDRetail.Common
{

    public delegate string GetEnvironmentBusinessInfoDelegate();  //TT#506-MD -jsobek -Add infrastructure to allow Email to be sent from the application
	/// <summary>
	/// Writes and reads audit information.
	/// </summary>
	/// <remarks>
	/// This class should be accessed through the ClientServerSession in the SessionAddressBlock.
	/// </remarks>
	public class Audit
	{
		private System.Collections.Queue _errorQueue;
		private int _processRID;
		private eProcesses _process;
		private int _userRID;
		private eMIDMessageLevel _highestMessageLevel;
        //BEGIN TT#554-MD -jsobek -User Log Level Report
        private const eMIDMessageLevel _initialLoggingLevel = eMIDMessageLevel.Edit;
        private eMIDMessageLevel _loggingLevel = _initialLoggingLevel;
        //END TT#554-MD -jsobek -User Log Level Report

        // Begin TT#1243 - JSmith - Audit Performance
        AuditData auditData = new AuditData();
        private int _commitLimit = 100;
        private int _writeCount = 0;
        private Timer _timer;
        private int _timerInterval = 30000;
        // End TT#1243

        // Begin TT#3444 - JSmith - Severe error during Purge
        private object _emailLock = new object();
        // End TT#3444 - JSmith - Severe error during Purge

        // Begin TT#1753 - JSmith - New transaction is not allowed because there are other threads running in the session
        private System.Collections.ArrayList _connectionLock = new System.Collections.ArrayList();
        bool _writingAudit = false;
        // End TT#1753

        // Begin TT#220 MD - JSmith - Modify audit to get column length once during initialization and not every time a message is added
        private int AuditReportMessageLength;
        // End TT#220 MD

        // Begin TT#46 MD - JSmith - User Dashboard
        // add event to update explorer when header is changed
        private LogUserDashboardEvent _LogUserDashboardEvent;
        private bool _logActivity = false;
 
        // add event to update User Dashboard 
        public LogUserDashboardEvent LogUserDashboardEvent
        {
            get
            {
                return _LogUserDashboardEvent;
            }
        }
        // End TT#46 MD

		// Constructors

		/// <summary>
		/// Creates a new instance of Audit
		/// </summary>
		public Audit()
		{
			try
			{
                // Begin TT#1243 - JSmith - Audit Performance
                SetUpConnectionManagement();
                // End TT#1243
				_process = eProcesses.unknown;
				_highestMessageLevel = eMIDMessageLevel.Debug;
				_userRID = 1;
				_errorQueue = new System.Collections.Queue();
				SetLoggingLevel();
                CommonProcessing(false);  // TT#46 MD - JSmith - User Dashboard
			}

			catch
			{
				throw;
			}
		}

        private GetEnvironmentBusinessInfoDelegate getEnvironmentBusinessInfo = null; //TT#506-MD -jsobek -Add infrastructure to allow Email to be sent from the application

		/// <summary>
		/// Creates a new instance of Audit for a process and user
		/// </summary>
		/// <param name="aProcess">The process for which the audit is created</param>
		/// <param name="userRID">The record id of the user associated with the audit</param>
		public Audit(eProcesses aProcess, int userRID
            , GetEnvironmentBusinessInfoDelegate getEnvironmentBusinessInfo = null //TT#506-MD -jsobek -Add infrastructure to allow Email to be sent from the application
            )
		{
			try
			{
                this.getEnvironmentBusinessInfo = getEnvironmentBusinessInfo; //TT#506-MD -jsobek -Add infrastructure to allow Email to be sent from the application
                // Begin TT#1243 - JSmith - Audit Performance
                SetUpConnectionManagement();
                // End TT#1243
				_highestMessageLevel = eMIDMessageLevel.Debug;
				_errorQueue = new System.Collections.Queue();
				_process = aProcess;
				_userRID = userRID;
				AddHeader();
				SetLoggingLevel();
                CommonProcessing(false);  // TT#46 MD - JSmith - User Dashboard
			}
			catch
			{
				throw;
			}
		}

        // Begin TT#46 MD - JSmith - User Dashboard
        /// <summary>
        /// Creates a new instance of Audit for a process and user
        /// </summary>
        /// <param name="aProcess">The process for which the audit is created</param>
        /// <param name="userRID">The record id of the user associated with the audit</param>
        public Audit(eProcesses aProcess, int userRID, bool aEnableActivityLog
            , GetEnvironmentBusinessInfoDelegate getEnvironmentBusinessInfo = null //TT#506-MD -jsobek -Add infrastructure to allow Email to be sent from the application
            )
        {
            try
            {
                this.getEnvironmentBusinessInfo = getEnvironmentBusinessInfo; //TT#506-MD -jsobek -Add infrastructure to allow Email to be sent from the application
                SetUpConnectionManagement();
                _highestMessageLevel = eMIDMessageLevel.Debug;
                _errorQueue = new System.Collections.Queue();
                _process = aProcess;
                _userRID = userRID;
                AddHeader();
                SetLoggingLevel();
                CommonProcessing(aEnableActivityLog);
            }
            catch
            {
                throw;
            }
        }
        // End TT#46 MD - JSmith - User Dashboard

		/// <summary>
		/// Creates a new instance of Audit for a process record id
		/// </summary>
		/// <param name="aProcessRID">The record id of the process for which the audit is being created</param>
		/// <remarks>
		/// This constructor should be used when the audit is being used to add additional messages
		/// to an existing audit report.
		/// </remarks>
        public Audit(int aProcessRID, int userRID, bool aEnableActivityLog
            , GetEnvironmentBusinessInfoDelegate getEnvironmentBusinessInfo = null //TT#506-MD -jsobek -Add infrastructure to allow Email to be sent from the application
            )
		{
			try
			{
                this.getEnvironmentBusinessInfo = getEnvironmentBusinessInfo; //TT#506-MD -jsobek -Add infrastructure to allow Email to be sent from the application
                SetUpConnectionManagement();
				_errorQueue = new System.Collections.Queue();
				_processRID = aProcessRID;
				SetLoggingLevel();
                CommonProcessing(aEnableActivityLog);
			}
			catch
			{
				throw;
			}
		}
        // End TT#46 MD - JSmith - User Dashboard

        /// <summary>
        /// Creates a new instance of Audit for a process record id
        /// </summary>
        /// <param name="aProcessRID">The record id of the process for which the audit is being created</param>
        /// <remarks>
        /// This constructor should be used when the audit is being used to add additional messages
        /// to an existing audit report.
        /// </remarks>
		public Audit(int aProcessRID, int userRID
            , GetEnvironmentBusinessInfoDelegate getEnvironmentBusinessInfo = null //TT#506-MD -jsobek -Add infrastructure to allow Email to be sent from the application
            )
		{
			try
			{
                this.getEnvironmentBusinessInfo = getEnvironmentBusinessInfo; //TT#506-MD -jsobek -Add infrastructure to allow Email to be sent from the application
                // Begin TT#1243 - JSmith - Audit Performance
                SetUpConnectionManagement();
                // End TT#1243
				_errorQueue = new System.Collections.Queue();
				_processRID = aProcessRID;
				SetLoggingLevel();
                CommonProcessing(false);
            }
            catch
            {
                throw;
            }
        }

        // Begin TT#46 MD - JSmith - User Dashboard
        private void CommonProcessing(bool aEnableActivityLog)
        {
            _logActivity = aEnableActivityLog;
            if (_logActivity)
            {
                _LogUserDashboardEvent = new LogUserDashboardEvent();
            }
        }
        // End TT#46 MD

        // Begin TT#1243 - JSmith - Audit Performance
        private void SetUpConnectionManagement()
        {
            // Begin TT#220 MD - JSmith - Modify audit to get column length once during initialization and not every time a message is added
            AuditReportMessageLength = auditData.GetColumnSize("PROC_RPT", "REPORT_MESSAGE");
            // End TT#220 MD
            object parm = null;

            parm = MIDConfigurationManager.AppSettings["AuditCommitLimit"];
            if (parm != null)
            {
                try
                {
                    _commitLimit = Convert.ToInt32(parm);
                }
                catch
                {
                }
            }
            parm = MIDConfigurationManager.AppSettings["AuditConnectionTimeOut"];
            if (parm != null)
            {
                try
                {
                    _timerInterval = Convert.ToInt32(parm) * 1000;
                }
                catch
                {
                }
            }

            _timer = new Timer(_timerInterval); // Set up the timer for 30 seconds
            _timer.Interval = _timerInterval;
            _timer.Elapsed += new ElapsedEventHandler(Timer_Elapsed);
            _timer.Enabled = true; // Enable it

        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            CloseUpdateConnection();
        }
        // End TT#1243

        //BEGIN TT#554-MD -jsobek -User Log Level Report
		private void SetLoggingLevel()
		{
			try
			{
                    _loggingLevel = GetDefaultLoggingLevel();
			}
			catch
			{
				throw;
			}
		}
        public eMIDMessageLevel GetDefaultLoggingLevel()
        {
            try
            {
                string loggingLevelSetting = MIDConfigurationManager.AppSettings["AuditLoggingLevel"];
                eMIDMessageLevel loggingLevel = _initialLoggingLevel;
                if (loggingLevelSetting != null)
                {
                    switch (loggingLevelSetting.Trim().ToUpper())
                    {
                        case "DEBUG":
                            loggingLevel = eMIDMessageLevel.Debug;
                            break;
                        case "INFORMATION":
                            loggingLevel = eMIDMessageLevel.Information;
                            break;
                        case "EDIT":
                            loggingLevel = eMIDMessageLevel.Edit;
                            break;
                        case "WARNING":
                            loggingLevel = eMIDMessageLevel.Warning;
                            break;
                        case "ERROR":
                            loggingLevel = eMIDMessageLevel.Error;
                            break;
                        case "SEVERE":
                            loggingLevel = eMIDMessageLevel.Severe;
                            break;
                        default:
                            loggingLevel = _initialLoggingLevel;
                            break;

                    }
                }
                return loggingLevel;
            }
            catch
            {
                throw;
            }
        }
        //END TT#554-MD -jsobek -User Log Level Report

		// Public properties

		/// <summary>
		/// The process id of the audit report.
		/// </summary>
		/// <remarks>
		/// This is the primary key to the audit report.
		/// </remarks>
		public int ProcessRID
		{
			get
			{
				return _processRID;
			}
		}

		/// <summary>
		/// The highest eMIDMessageLevel that has been encountered.
		/// </summary>
		/// <remarks>
		/// </remarks>
		public eMIDMessageLevel HighestMessageLevel
		{
			get
			{
				return _highestMessageLevel;
			}
		}

		public eMIDMessageLevel LoggingLevel
		{
			get { return _loggingLevel; }
			set { _loggingLevel = value; }
		}

		// Private methods

		/// <summary>
		/// Opens an update connection to the database to write audit information.
		/// </summary>
		private void OpenUpdateConnection(AuditData auditData)
		{
			try
			{
                lock (_connectionLock.SyncRoot)
                {
                    if (!auditData.ConnectionIsOpen)
                    {
                        auditData.OpenUpdateConnection();
                    }

                    if (_timer.Enabled)
                    {
                        _timer.Stop();
                    }
                    _timer.Start();

                }
			}
			catch
			{
				throw;
			}
		}

        // Begin TT#1243 - JSmith - Audit Performance
        /// <summary>
        /// Closes the update connection to the database.
        /// </summary>
        public void CloseUpdateConnection()
        {
            try
            {
                CloseUpdateConnection(auditData);
            }
            catch
            {
                throw;
            }
        }
        // End TT#1243

		/// <summary>
		/// Closes the update connection to the database.
		/// </summary>
		private void CloseUpdateConnection(AuditData auditData)
		{
			try
            {
                if (!_writingAudit)
                {
                    lock (_connectionLock.SyncRoot)
                    {
                        _timer.Stop();
                        if (auditData.ConnectionIsOpen)
                        {
                            if (_writeCount > 0)
                            {
                                CommitData(auditData, true, false);
                            }
                            auditData.CloseUpdateConnection();
                        }
                    }
                }
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Commits the data to the database.
		/// </summary>
        /// 
        private void CommitData(AuditData auditData)
        {
            CommitData(auditData, false);
        }

        private void CommitData(AuditData auditData, bool aForceCommit)
        {
            CommitData(auditData, aForceCommit, true);
        }

		private void CommitData(AuditData auditData, bool aForceCommit, bool aWithLock)
		{
            try
            {
                if (aWithLock)
                {
                    lock (_connectionLock.SyncRoot)
                    {
                        ++_writeCount;
                        _timer.Stop();
                        if (_writeCount >= _commitLimit ||
                           aForceCommit)
                        {
                            _writeCount = 0;
                            auditData.CommitData();
                        }
                        _timer.Start();
                    }
                }
                else
                {
                    ++_writeCount;
                    _timer.Stop();
                    if (_writeCount >= _commitLimit ||
                       aForceCommit)
                    {
                        _writeCount = 0;
                        auditData.CommitData();
                    }
                    _timer.Start();
                }
            }
            catch
            {
                throw;
            }
		}

		/// <summary>
		/// Writes an audit report header to the database.
		/// </summary>
		/// <remarks>
		/// An audit header must exist before any messages can be added to the audit report.
		/// The database call returns the record id of the process to use as the primary key
		/// for all references to the audit report information.  The record id is available using the
		/// ProcessRID property.
		/// </remarks>
		private void AddHeader()
		{
            // Begin TT#1243 - JSmith - Audit Performance
            //AuditData auditData = new AuditData();
            // End TT#1243
			try
			{
                _writingAudit = true;
				OpenUpdateConnection(auditData);
				_processRID = auditData.ProcessAuditHeader_Add(_process, eProcessExecutionStatus.Running, 
					DateTime.Now, _userRID, eMIDTextCode.sum_Running);
                // Begin TT#1243 - JSmith - Audit Performance
                // force commit to write identity
                _writeCount = _commitLimit;
                // End TT#1243
				CommitData(auditData);
			}
			catch ( Exception err )
			{
				string eventLogID = "MIDAudit";
				if (!EventLog.SourceExists(eventLogID))
				{
					EventLog.CreateEventSource(eventLogID, null);
				}
				Exception innerE = err;
				while (innerE.InnerException != null) 
				{
					innerE = innerE.InnerException;
				}
				EventLog.WriteEntry(eventLogID, "Error creating Audit - " + innerE.ToString(), EventLogEntryType.Error);
				throw;
			}
			finally
			{
                _writingAudit = false;
				CloseUpdateConnection(auditData);
			}
		}

		// Public methods

		/// <summary>
		/// Updates the audit report header information for an audit report.
		/// </summary>
		/// <param name="CompletionStatus">The completion status of the process.</param>
		/// <param name="processSummary">A short summary of the status of the process.</param>
		/// <param name="procDesc">The description of the process</param>
		/// <param name="aHighestMessageLevel">The message level to put to the audit</param>
		public void UpdateHeader(eProcessCompletionStatus CompletionStatus, eMIDTextCode processSummary, string procDesc, eMIDMessageLevel aHighestMessageLevel)
		{
            // Begin TT#1243 - JSmith - Audit Performance
            //AuditData auditData = new AuditData();
            // End TT#1243
			try
			{
                _writingAudit = true;
				OpenUpdateConnection(auditData);
				auditData.ProcessAuditHeader_Update(_processRID, CompletionStatus, DateTime.Now, processSummary, 
					aHighestMessageLevel, procDesc);
				CommitData(auditData, true);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
			finally
			{
                _writingAudit = false;
				CloseUpdateConnection(auditData);
			}
		}

		/// <summary>
		/// Updates the audit report header information for an audit report.
		/// </summary>
		/// <param name="ExecutionStatus">The current status of the process.</param>
		/// <param name="processSummary">A short summary of the status of the process.</param>
		/// <param name="procDesc">The description of the process</param>
		/// <param name="aHighestMessageLevel">The message level to put to the audit</param>
		public void UpdateHeader(eProcessExecutionStatus ExecutionStatus, eMIDTextCode processSummary, string procDesc, eMIDMessageLevel aHighestMessageLevel)
		{
            // Begin TT#1243 - JSmith - Audit Performance
            //AuditData auditData = new AuditData();
            // End TT#1243
			try
			{
                _writingAudit = true;
				OpenUpdateConnection(auditData);
				auditData.ProcessAuditHeader_Update(_processRID, ExecutionStatus, DateTime.Now, processSummary, 
					aHighestMessageLevel, procDesc);
				CommitData(auditData, true);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
            // Begin TT#1243 - JSmith - Audit Performance
            finally
            {
                _writingAudit = false;
                CloseUpdateConnection(auditData);
            }
            // End TT#1243
		}

		/// <summary>
		/// Adds a message to the audit report that contains both a message code and additional text.
		/// </summary>
		/// <param name="messageLevel">The level of the message.</param>
		/// <param name="messageCode">The code of the message.</param>
		/// <param name="reportMessage">The text of the message.</param>
		/// <param name="reportingModule">The module reporting the message.</param>
		/// <param name="aForceWrite">Message is written regardless of the logging level</param>
		public void Add_Msg(eMIDMessageLevel messageLevel, eMIDTextCode messageCode, 
            string reportMessage, string reportingModule, bool aForceWrite = false,
        [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
        [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
        [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0)
		{
			Audit_Add_Msg(messageLevel, messageCode, reportMessage, reportingModule, aForceWrite, memberName, sourceFilePath, sourceLineNumber);
		}

		/// <summary>
		/// Adds a message to the audit report that contains both a message code and additional text.
		/// </summary>
		/// <param name="messageLevel">The level of the message.</param>
		/// <param name="messageCode">The code of the message.</param>
		/// <param name="reportMessage">The text of the message.</param>
		/// <param name="reportingModule">The module reporting the message.</param>
		/// <param name="aForceWrite">Causes the message to always be written</param>
		private void Audit_Add_Msg(eMIDMessageLevel messageLevel, eMIDTextCode messageCode,
            string reportMessage, string reportingModule, bool aForceWrite, string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0)
		{
			try
			{
                reportingModule = FormatReportingModule(memberName, sourceFilePath);
                // Begin TT#1159 - JSmith - Improve Messaging
                eMIDMessageLevel dbMessageLevel = MIDText.GetMessageLevel((int)messageCode);
                if (dbMessageLevel != eMIDMessageLevel.None)
                {
                    messageLevel = dbMessageLevel;
                }
                // End TT#1159

                LogActivity(reportingModule, messageLevel, messageCode, reportMessage, aForceWrite);  // TT#46 MD - JSmith - User Dashboard
                //Begin TT#506-MD -jsobek -Add infrastructure to allow Email to be sent from the application
                EmailAuditMessage(reportingModule, messageLevel, messageCode, reportMessage);
                //End TT#506-MD -jsobek -Add infrastructure to allow Email to be sent from the application

				if (messageLevel < _loggingLevel &&
					!aForceWrite)
				{
					return;
				}
				if (reportMessage == null)
				{
					reportMessage = string.Empty;
				}
				if (reportingModule == null)
				{
					reportingModule = "unknown";
				}
                // Begin TT#1243 - JSmith - Audit Performance
                //AuditData auditData = new AuditData();
                // End TT#1243
                int lineNumber = sourceLineNumber;
				try
				{
                    // Begin TT#1395 - JSmith - Object reference error when > 1 ConcurrentProcesses
                    lock (_errorQueue.SyncRoot)
                    {
                    // End TT#1395
                        _writingAudit = true;
                        OpenUpdateConnection(auditData);
                        // line number is only available if during a debug compile 
//#if (DEBUG)
//                        lineNumber = new System.Diagnostics.StackFrame(1, true).GetFileLineNumber();
//#endif
                        // lookup module if not provided
                        if (reportingModule == null)
                        {
                            string stackFrameModule = new System.Diagnostics.StackFrame(1, true).GetFileName();
                            reportingModule = stackFrameModule;
                        }
                        // Begin TT#220 MD - JSmith - Modify audit to get column length once during initialization and not every time a message is added
                        //auditData.AuditReport_Add(_processRID, reportingModule, lineNumber,
                        //    messageLevel, messageCode, reportMessage);
                        auditData.AuditReport_Add(_processRID, reportingModule, lineNumber,
                            messageLevel, messageCode, reportMessage, AuditReportMessageLength);
                        // End TT#220 MD
                        SaveHighestMessageLevel(messageLevel);
                        CommitData(auditData);
                    // Begin TT#1395 - JSmith - Object reference error when > 1 ConcurrentProcesses
                    }
                    // End TT#1395
				}
				catch (DatabaseForeignKeyViolation keyVio)
				{
					string message = keyVio.ToString();
					if (!EventLog.SourceExists("MIDAudit"))
					{
						EventLog.CreateEventSource("MIDAudit", null);
					}
					EventLog.WriteEntry("MIDAudit", Include.ErrorBadTextCode + "; messageCode=" + (int)messageCode + "; message=" + reportMessage+ "; reportingModule=" + reportingModule, EventLogEntryType.Warning);
					try
					{
                        // Begin TT#1395 - JSmith - Object reference error when > 1 ConcurrentProcesses
                        lock (_errorQueue.SyncRoot)
                        {
                        // End TT#1395
                            OpenUpdateConnection(auditData);
                            // Begin TT#220 MD - JSmith - Modify audit to get column length once during initialization and not every time a message is added
                            //auditData.AuditReport_Add(_processRID, reportingModule, lineNumber,
                            //    eMIDMessageLevel.Warning, 0, Include.ErrorBadTextCode + "; messageCode=" + (int)messageCode);
                            auditData.AuditReport_Add(_processRID, reportingModule, lineNumber,
                                eMIDMessageLevel.Warning, 0, Include.ErrorBadTextCode + "; messageCode=" + (int)messageCode, AuditReportMessageLength);
                            // End TT#220 MD
                            CommitData(auditData);
                        // Begin TT#1395 - JSmith - Object reference error when > 1 ConcurrentProcesses
                        }
                        // End TT#1395
					}
					catch
					{
					}
				}
				catch (Exception err)
				{
					if (!EventLog.SourceExists("MIDAudit"))
					{
						EventLog.CreateEventSource("MIDAudit", null);
					}
					EventLog.WriteEntry("MIDAudit", "Error in audit Add_Msg; messageCode=" + (int)messageCode + "; message=" + reportMessage+ "; reportingModule=" + reportingModule, EventLogEntryType.Error);
					EventLog.WriteEntry("MIDAudit", err.ToString(), EventLogEntryType.Error);
					throw;
				}
                finally
                {
                    _writingAudit = false;
                }
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Adds a message to the audit report that contains both a message code and additional text where
		/// the calling module provides the module and line number where the message was generated.
		/// </summary>
		/// <param name="messageLevel">The level of the message.</param>
		/// <param name="messageCode">The code of the message.</param>
		/// <param name="reportMessage">The text of the message.</param>
		/// <param name="lineNumber">The line number in the module where the message was generated.</param>
		/// <param name="reportingModule">The module reporting the message.</param>
        /// <param name="aForceWrite">Message is written regardless of the logging level</param>
		public void Add_Msg(eMIDMessageLevel messageLevel, eMIDTextCode messageCode, 
            string reportMessage, int lineNumber, string reportingModule, bool aForceWrite = false,
        [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
        [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
        [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0)
		{
			Audit_Add_Msg(messageLevel, messageCode, reportMessage, lineNumber, reportingModule, aForceWrite, memberName, sourceFilePath, sourceLineNumber);
		}

		/// <summary>
		/// Adds a message to the audit report that contains both a message code and additional text where
		/// the calling module provides the module and line number where the message was generated.
		/// </summary>
		/// <param name="messageLevel">The level of the message.</param>
		/// <param name="messageCode">The code of the message.</param>
		/// <param name="reportMessage">The text of the message.</param>
		/// <param name="lineNumber">The line number in the module where the message was generated.</param>
		/// <param name="reportingModule">The module reporting the message.</param>
		/// <param name="aForceWrite">Causes the message to always be written</param>
        private void Audit_Add_Msg(eMIDMessageLevel messageLevel, eMIDTextCode messageCode,
            string reportMessage, int lineNumber, string reportingModule, bool aForceWrite, string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0)
		{
			try
			{
                reportingModule = FormatReportingModule(memberName, sourceFilePath);
                // Begin TT#1159 - JSmith - Improve Messaging
                eMIDMessageLevel dbMessageLevel = MIDText.GetMessageLevel((int)messageCode);
                if (dbMessageLevel != eMIDMessageLevel.None)
                {
                    messageLevel = dbMessageLevel;
                }
                // End TT#1159

                LogActivity(reportingModule, messageLevel, messageCode, reportMessage, aForceWrite);  // TT#46 MD - JSmith - User Dashboard
                //Begin TT#506-MD -jsobek -Add infrastructure to allow Email to be sent from the application
                EmailAuditMessage(reportingModule, messageLevel, messageCode, reportMessage);
                //End TT#506-MD -jsobek -Add infrastructure to allow Email to be sent from the application

				if (messageLevel < _loggingLevel &&
					!aForceWrite)
				{
					return;
				}
				if (reportMessage == null)
				{
					reportMessage = string.Empty;
				}
				if (reportingModule == null)
				{
					reportingModule = "unknown";
				}
                // Begin TT#1243 - JSmith - Audit Performance
                //AuditData auditData = new AuditData();
                // End TT#1243
				try
				{
                    // Begin TT#1395 - JSmith - Object reference error when > 1 ConcurrentProcesses
                    lock (_errorQueue.SyncRoot)
                    {
                    // End TT#1395
                        _writingAudit = true;
                        OpenUpdateConnection(auditData);
                        // Begin TT#220 MD - JSmith - Modify audit to get column length once during initialization and not every time a message is added
                        //auditData.AuditReport_Add(_processRID, reportingModule, lineNumber,
                        //    messageLevel, messageCode, reportMessage);
                        auditData.AuditReport_Add(_processRID, reportingModule, lineNumber,
                            messageLevel, messageCode, reportMessage, AuditReportMessageLength);
                        // End TT#220 MD
                        SaveHighestMessageLevel(messageLevel);
                        CommitData(auditData);
                    // Begin TT#1395 - JSmith - Object reference error when > 1 ConcurrentProcesses
                    }
                    // End TT#1395
				}
				catch
				{
					throw;
				}
                finally
                {
                    _writingAudit = false;
                }
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Adds a message to the audit report that contains only text.
		/// </summary>
		/// <param name="messageLevel">The level of the message.</param>
		/// <param name="reportMessage">The text of the message.</param>
		/// <param name="reportingModule">The module reporting the message.</param>
        /// <param name="aForceWrite">Message is written regardless of the logging level</param>
		public void Add_Msg(eMIDMessageLevel messageLevel, string reportMessage, string reportingModule,
            bool aForceWrite = false, int lineNumber = 0, bool overrideReportModule = true,
        [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
        [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
        [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0)
		{
            Audit_Add_Msg(messageLevel, reportMessage, reportingModule, aForceWrite, lineNumber, overrideReportModule, memberName, sourceFilePath, sourceLineNumber);
		}

		/// <summary>
		/// Adds a message to the audit report that contains only text.
		/// </summary>
		/// <param name="messageLevel">The level of the message.</param>
		/// <param name="reportMessage">The text of the message.</param>
		/// <param name="reportingModule">The module reporting the message.</param>
		/// <param name="aForceWrite">Causes the message to always be written</param>
        private void Audit_Add_Msg(eMIDMessageLevel messageLevel, string reportMessage, string reportingModule,
            bool aForceWrite, int lineNumber = 0, bool overrideReportModule = true, string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0)
		{
			try
			{
                if (overrideReportModule
                    || reportingModule == null
                    || reportingModule.Trim().Length == 0)
                {
                    reportingModule = FormatReportingModule(memberName, sourceFilePath);
                }
                LogActivity(reportingModule, messageLevel, reportMessage, aForceWrite);  // TT#46 MD - JSmith - User Dashboard
                //Begin TT#506-MD -jsobek -Add infrastructure to allow Email to be sent from the application
                EmailAuditMessage(reportingModule, messageLevel, eMIDTextCode.Unassigned, reportMessage);
                //End TT#506-MD -jsobek -Add infrastructure to allow Email to be sent from the application

                if (messageLevel < _loggingLevel &&
					!aForceWrite)
				{
					return;
				}
				if (reportMessage == null)
				{
					reportMessage = string.Empty;
				}
				if (reportingModule == null)
				{
					reportingModule = "unknown";
				}
                // Begin TT#1243 - JSmith - Audit Performance
                //AuditData auditData = new AuditData();
                // End TT#1243
				try
				{
                    // Begin TT#1395 - JSmith - Object reference error when > 1 ConcurrentProcesses
                    lock (_errorQueue.SyncRoot)
                    {
                    // End TT#1395
                        _writingAudit = true;
                        OpenUpdateConnection(auditData);
                        // line number is only available if during a debug compile 
                        if (overrideReportModule)
                        {
                            lineNumber = sourceLineNumber;
                        }
//#if (DEBUG)
//                        lineNumber = new System.Diagnostics.StackFrame(1, true).GetFileLineNumber();
//#endif
                        // lookup module if not provided
                        if (reportingModule == null)
                        {
                            string stackFrameModule = new System.Diagnostics.StackFrame(1, true).GetFileName();
                            reportingModule = stackFrameModule;
                        }
                        // Begin TT#220 MD - JSmith - Modify audit to get column length once during initialization and not every time a message is added
                        //auditData.AuditReport_Add(_processRID, reportingModule, lineNumber,
                        //    messageLevel, reportMessage);
                        auditData.AuditReport_Add(_processRID, reportingModule, lineNumber,
                            messageLevel, reportMessage, AuditReportMessageLength);
                        // End TT#220 MD
                        SaveHighestMessageLevel(messageLevel);
                        CommitData(auditData);
                    // Begin TT#1395 - JSmith - Object reference error when > 1 ConcurrentProcesses
                    }
                    // End TT#1395
				}
				catch
				{
					throw;
				}
                finally
                {
                    _writingAudit = false;
                }
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Adds a message to the audit report that contain only a message code.
		/// </summary>
		/// <param name="messageLevel">The level of the message.</param>
		/// <param name="messageCode">The code of the message.</param>
		/// <param name="reportingModule">The module reporting the message.</param>
        /// <param name="aForceWrite">Message is written regardless of the logging level</param>
		public void Add_Msg(eMIDMessageLevel messageLevel, eMIDTextCode messageCode, string reportingModule,
            bool aForceWrite = false,
        [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
        [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
        [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0)
		{
			Audit_Add_Msg(messageLevel, messageCode, reportingModule, aForceWrite, memberName, sourceFilePath, sourceLineNumber);
		}

		/// <summary>
		/// Adds a message to the audit report that contain only a message code.
		/// </summary>
		/// <param name="messageLevel">The level of the message.</param>
		/// <param name="messageCode">The code of the message.</param>
		/// <param name="reportingModule">The module reporting the message.</param>
		/// <param name="aForceWrite">Causes the message to always be written</param>
        private void Audit_Add_Msg(eMIDMessageLevel messageLevel, eMIDTextCode messageCode, string reportingModule,
            bool aForceWrite, string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0)
		{
			try
			{
                reportingModule = FormatReportingModule(memberName, sourceFilePath);
                // Begin TT#1159 - JSmith - Improve Messaging
                eMIDMessageLevel dbMessageLevel = MIDText.GetMessageLevel((int)messageCode);
                if (dbMessageLevel != eMIDMessageLevel.None)
                {
                    messageLevel = dbMessageLevel;
                }
                // End TT#1159

                LogActivity(reportingModule, messageLevel, messageCode, string.Empty, aForceWrite);  // TT#46 MD - JSmith - User Dashboard
                //Begin TT#506-MD -jsobek -Add infrastructure to allow Email to be sent from the application
                EmailAuditMessage(reportingModule, messageLevel, messageCode, string.Empty);
                //End TT#506-MD -jsobek -Add infrastructure to allow Email to be sent from the application

				if (messageLevel < _loggingLevel &&
					!aForceWrite)
				{
					return;
				}
				if (reportingModule == null)
				{
					reportingModule = "unknown";
				}
                // Begin TT#1243 - JSmith - Audit Performance
                //AuditData auditData = new AuditData();
                // End TT#1243
				try
				{
                    // Begin TT#1395 - JSmith - Object reference error when > 1 ConcurrentProcesses
                    lock (_errorQueue.SyncRoot)
                    {
                    // End TT#1395
                        _writingAudit = true;
                        OpenUpdateConnection(auditData);
                        // line number is only available if during a debug compile 
                        int lineNumber = sourceLineNumber;
//#if (DEBUG)
//                        lineNumber = new System.Diagnostics.StackFrame(1, true).GetFileLineNumber();
//#endif
                        // lookup module if not provided
                        if (reportingModule == null)
                        {
                            string stackFrameModule = new System.Diagnostics.StackFrame(1, true).GetFileName();
                            reportingModule = stackFrameModule;
                        }
                        auditData.AuditReport_Add(_processRID, reportingModule, lineNumber,
                            messageLevel, messageCode);
                        SaveHighestMessageLevel(messageLevel);
                        CommitData(auditData);
                    // Begin TT#1395 - JSmith - Object reference error when > 1 ConcurrentProcesses
                    }
                    // End TT#1395
				}
				catch
				{
					throw;
				}
                finally
                {
                    _writingAudit = false;
                }
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Adds a message to the audit report for a given message code where the caller provides the calling
		/// module and line number.
		/// </summary>
		/// <param name="messageLevel">The level of the message.</param>
		/// <param name="messageCode">The code of the message.</param>
		/// <param name="lineNumber">The line number in the module where the message was generated.</param>
		/// <param name="reportingModule">The module reporting the message.</param>
		/// <param name="aForceWrite">Causes the message to always be written</param>
		private void Add_Msg(eMIDMessageLevel messageLevel, eMIDTextCode messageCode, int lineNumber, 
            string reportingModule, bool aForceWrite, string memberName, string sourceFilePath, int sourceLineNumber)
		{
			try
			{
                // Begin TT#1159 - JSmith - Improve Messaging
                eMIDMessageLevel dbMessageLevel = MIDText.GetMessageLevel((int)messageCode);
                if (dbMessageLevel != eMIDMessageLevel.None)
                {
                    messageLevel = dbMessageLevel;
                }
                // End TT#1159
                LogActivity(reportingModule, messageLevel, messageCode, string.Empty, aForceWrite);  // TT#46 MD - JSmith - User Dashboard
                //Begin TT#506-MD -jsobek -Add infrastructure to allow Email to be sent from the application
                EmailAuditMessage(reportingModule, messageLevel, messageCode, string.Empty);
                //End TT#506-MD -jsobek -Add infrastructure to allow Email to be sent from the application

				if (messageLevel < _loggingLevel &&
					!aForceWrite)
				{
					return;
				}
				if (reportingModule == null)
				{
					reportingModule = "unknown";
				}
                // Begin TT#1243 - JSmith - Audit Performance
                //AuditData auditData = new AuditData();
                // End TT#1243
				try
				{
                    // Begin TT#1395 - JSmith - Object reference error when > 1 ConcurrentProcesses
                    lock (_errorQueue.SyncRoot)
                    {
                    // End TT#1395
                        _writingAudit = true;
                        OpenUpdateConnection(auditData);
                        auditData.AuditReport_Add(_processRID, reportingModule, lineNumber,
                            messageLevel, messageCode);
                        SaveHighestMessageLevel(messageLevel);
                        CommitData(auditData);
                    // Begin TT#1395 - JSmith - Object reference error when > 1 ConcurrentProcesses
                    }
                    // End TT#1395
				}
				catch
				{
					throw;
				}
                finally
                {
                    _writingAudit = false;
                }
			}
			catch
			{
				throw;
			}
		}

        private string FormatReportingModule(string memberName, string sourceFilePath)
        {
            string reportingModule = null;

            if (memberName != null
                && memberName.Trim().Length > 0)
            {
                reportingModule = memberName;
            }

            if (sourceFilePath != null
                && sourceFilePath.Trim().Length > 0)
            {
                string[] sections = sourceFilePath.Split('\\');
                reportingModule += " in " + sections[sections.Length - 1];
            }

            return reportingModule;
        }

		/// <summary>
		/// Adds an MIDException to the audit report.
		/// </summary>
		/// <param name="ex">The exception.</param>
		/// <remarks>This logs all inner exceptions</remarks>
		public void Log_MIDException(MIDException ex)
		{
			try
			{
                int sourceLineNumber = 0;
                string reportingModule = "";
                bool exceptionContainsReportingModule = !string.IsNullOrWhiteSpace(ex.MemberName);
                if (exceptionContainsReportingModule)
                {
                    reportingModule = FormatReportingModule(ex.MemberName, ex.SourceFilePath);
                    sourceLineNumber = ex.SourceLineNumber;
                }

                //string reportingModule;
				Exception innerE = (Exception)ex;
				while (innerE.InnerException != null) 
				{
                    if (!exceptionContainsReportingModule)
                    {
                        if (innerE.TargetSite == null)
                        {
                            reportingModule = "unknown";
                        }
                        else
                        {
                            reportingModule = innerE.TargetSite.DeclaringType.FullName + ":" + innerE.TargetSite.Name;
                        }
                    }
                    //Add_Msg(eMIDMessageLevel.Error, innerE.ToString(), reportingModule);
                    Audit_Add_Msg(eMIDMessageLevel.Error, innerE.ToString(), reportingModule, true, sourceLineNumber, false);
					innerE = innerE.InnerException;
				}
                if (!exceptionContainsReportingModule)
                {
                    if (innerE.TargetSite == null)
                    {
                        reportingModule = "unknown";
                    }
                    else
                    {
                        reportingModule = innerE.TargetSite.DeclaringType.FullName + ":" + innerE.TargetSite.Name;
                    }
                }
                //Add_Msg(eMIDMessageLevel.Error, innerE.ToString(), reportingModule);
                Audit_Add_Msg(eMIDMessageLevel.Error, innerE.ToString(), reportingModule, true, sourceLineNumber, false);
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Adds an MIDException to the audit report.
		/// </summary>
		/// <param name="ex">The exception.</param>
		/// <param name="reportingModule">The module reporting the message.</param>
		/// <remarks>This logs all inner exceptions</remarks>
		public void Log_MIDException(MIDException ex, string reportingModule)
		{
            try
            {
                int sourceLineNumber = 0;
                bool exceptionContainsReportingModule = !string.IsNullOrWhiteSpace(ex.MemberName);
                if (exceptionContainsReportingModule)
                {
                    reportingModule = FormatReportingModule(ex.MemberName, ex.SourceFilePath);
                    sourceLineNumber = ex.SourceLineNumber;
                }

                Exception innerE = ex;
                while (innerE.InnerException != null)
                {
                    if (!exceptionContainsReportingModule)
                    {
                        if (innerE.TargetSite == null)
                        {
                            reportingModule = "unknown";
                        }
                        else
                        {
                            reportingModule = innerE.TargetSite.DeclaringType.FullName + ":" + reportingModule + ":" + innerE.TargetSite.Name;
                        }
                    }
                    //Add_Msg(eMIDMessageLevel.Error, innerE.ToString(), reportingModule);
                    Audit_Add_Msg(eMIDMessageLevel.Error, innerE.ToString(), reportingModule, true, sourceLineNumber, false);
                    innerE = innerE.InnerException;
                }
                if (!exceptionContainsReportingModule)
                {
                    if (innerE.TargetSite == null)
                    {
                        reportingModule = "unknown";
                    }
                    else
                    {
                        reportingModule = innerE.TargetSite.DeclaringType.FullName + ":" + reportingModule + ":" + innerE.TargetSite.Name;
                    }
                }
                //Add_Msg(eMIDMessageLevel.Error, innerE.ToString(), reportingModule);
                Audit_Add_Msg(eMIDMessageLevel.Error, innerE.ToString(), reportingModule, true, sourceLineNumber, false);
            }
            catch
            {
                throw;
            }
		}

		/// <summary>
		/// Adds an MIDException to the audit report.
		/// </summary>
		/// <param name="ex">The exception.</param>
		/// <param name="reportingModule">The module reporting the message.</param>
		/// <param name="logOnlyInnerMostException">This enumeration identifies if only the inner most exception
		/// is to be logged or if all inner exceptions are to be logged.</param>
		public void Log_MIDException(MIDException ex, string reportingModule, eExceptionLogging logOnlyInnerMostException)
		{
			try
			{
                int sourceLineNumber = 0;
                bool exceptionContainsReportingModule = !string.IsNullOrWhiteSpace(ex.MemberName);
                if (exceptionContainsReportingModule)
                {
                    reportingModule = FormatReportingModule(ex.MemberName, ex.SourceFilePath);
                    sourceLineNumber = ex.SourceLineNumber;
                }

				Exception innerE = ex;
				while (innerE.InnerException != null) 
				{
					if (logOnlyInnerMostException == eExceptionLogging.logAllInnerExceptions)
					{
                        if (!exceptionContainsReportingModule)
                        {
                            if (innerE.TargetSite == null)
                            {
                                reportingModule = "unknown";
                            }
                            else
                            {
                                reportingModule = innerE.TargetSite.DeclaringType.FullName + ":" + reportingModule + ":" + innerE.TargetSite.Name;
                            }
                        }
						Add_Msg(eMIDMessageLevel.Error, innerE.ToString(), reportingModule);
					}
					innerE = innerE.InnerException;
				}
                if (!exceptionContainsReportingModule)
                {
                    if (innerE.TargetSite == null)
                    {
                        reportingModule = "unknown";
                    }
                    else
                    {
                        reportingModule = innerE.TargetSite.DeclaringType.FullName + ":" + reportingModule + ":" + innerE.TargetSite.Name;
                    }
                }
                //Add_Msg(eMIDMessageLevel.Error, innerE.ToString(), reportingModule);
                Audit_Add_Msg(eMIDMessageLevel.Error, innerE.ToString(), reportingModule, true, sourceLineNumber, false);
			}
			catch
			{
				throw;
			}
		}

        ///// <summary>
        ///// Adds an exception to the audit report.
        ///// </summary>
        ///// <param name="ex">The exception.</param>
        ///// <remarks>This logs all inner exceptions</remarks>
        //public void Log_Exception(Exception ex,
        //[System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
        //[System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
        //[System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0)
        //{
        //    try
        //    {
        //        string reportingModule = FormatReportingModule(memberName, sourceFilePath);
        //        Exception innerE = ex;
        //        while (innerE.InnerException != null) 
        //        {
        //            if (innerE.TargetSite == null
        //                && (reportingModule == null || reportingModule.Trim().Length == 0))
        //            {
        //                reportingModule = "unknown";
        //            }
        //            else
        //            {
        //                reportingModule = innerE.TargetSite.DeclaringType.FullName + ":" + innerE.TargetSite.Name;
        //            }
        //            Add_Msg(eMIDMessageLevel.Error, innerE.ToString(), reportingModule);
        //            innerE = innerE.InnerException;
        //        }
        //        if (innerE.TargetSite == null)
        //        {
        //            reportingModule = "unknown";
        //        }
        //        else
        //        {
        //            reportingModule = innerE.TargetSite.DeclaringType.FullName + ":" + innerE.TargetSite.Name;
        //        }
		//		  Add_Msg(eMIDMessageLevel.Error, innerE.ToString(), reportingModule);
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}

        public void Log_Exception(string err)
        {
            try
            {
                string reportingModule = "unknown";;


                Add_Msg(eMIDMessageLevel.Error, err, reportingModule);
            }
            catch
            {
                throw;
            }
        }

		/// <summary>
		/// Adds an exception to the audit report.
		/// </summary>
		/// <param name="ex">The exception.</param>
		/// <param name="reportingModule">The module reporting the message.</param>
		/// <remarks>This logs all inner exceptions</remarks>
        public void Log_Exception(Exception ex, string reportingModule = "",
        [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
        [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
        [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0)
		{
			try
			{
                bool exceptionContainsReportingModule = false;
                if (ex is MIDException)
                {

                    exceptionContainsReportingModule = !string.IsNullOrWhiteSpace(((MIDException)ex).MemberName);
                    if (exceptionContainsReportingModule)
                    {
                        reportingModule = FormatReportingModule(((MIDException)ex).MemberName, ((MIDException)ex).SourceFilePath);
                        sourceLineNumber = ((MIDException)ex).SourceLineNumber;
                    }
                }

				Exception innerE = ex;
				while (innerE.InnerException != null) 
				{
                    if (!exceptionContainsReportingModule)
                    {
                        if (innerE.TargetSite == null)
                        {
                            reportingModule = "unknown";
                        }
                        else
                        {
                            reportingModule = innerE.TargetSite.DeclaringType.FullName + ":" + reportingModule + ":" + innerE.TargetSite.Name;
                        }
                    }
                    //Add_Msg(eMIDMessageLevel.Error, innerE.ToString(), reportingModule);
                    Audit_Add_Msg(eMIDMessageLevel.Error, innerE.ToString(), reportingModule, true, sourceLineNumber, false);
					innerE = innerE.InnerException;
				}
                if (!exceptionContainsReportingModule)
                {
                    if (innerE.TargetSite == null)
                    {
                        reportingModule = "unknown";
                    }
                    else
                    {
                        reportingModule = innerE.TargetSite.DeclaringType.FullName + ":" + reportingModule + ":" + innerE.TargetSite.Name;
                    }
                }


                Audit_Add_Msg(eMIDMessageLevel.Error, innerE.ToString(), reportingModule, true, sourceLineNumber, false);
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Adds an exception to the audit report.
		/// </summary>
		/// <param name="ex">The exception.</param>
		/// <param name="reportingModule">The module reporting the message.</param>
		/// <param name="logOnlyInnerMostException">This enumeration identifies if only the inner most exception
		/// is to be logged or if all inner exceptions are to be logged.</param>
        public void Log_Exception(Exception ex, string reportingModule, eExceptionLogging logOnlyInnerMostException,
        [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
        [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
        [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0)
		{
			try
			{
                bool exceptionContainsReportingModule = false;
                if (ex is MIDException)
                {

                    exceptionContainsReportingModule = !string.IsNullOrWhiteSpace(((MIDException)ex).MemberName);
                    if (exceptionContainsReportingModule)
                    {
                        reportingModule = FormatReportingModule(((MIDException)ex).MemberName, ((MIDException)ex).SourceFilePath);
                        sourceLineNumber = ((MIDException)ex).SourceLineNumber;
                    }
                }

				Exception innerE = ex;
				while (innerE.InnerException != null) 
				{
					if (logOnlyInnerMostException == eExceptionLogging.logAllInnerExceptions)
					{
                        if (!exceptionContainsReportingModule)
                        {
                            if (innerE.TargetSite == null)
                            {
                                reportingModule = "unknown";
                            }
                            else
                            {
                                reportingModule = innerE.TargetSite.DeclaringType.FullName + ":" + reportingModule + ":" + innerE.TargetSite.Name;
                            }
                        }
                        //Add_Msg(eMIDMessageLevel.Error, innerE.ToString(), reportingModule);
                        Audit_Add_Msg(eMIDMessageLevel.Error, innerE.ToString(), reportingModule, true, sourceLineNumber, false);
					}
					innerE = innerE.InnerException;
				}
                if (!exceptionContainsReportingModule)
                {
                    if (innerE.TargetSite == null)
                    {
                        reportingModule = "unknown";
                    }
                    else
                    {
                        reportingModule = innerE.TargetSite.DeclaringType.FullName + ":" + reportingModule + ":" + innerE.TargetSite.Name;
                    }
                }

                Audit_Add_Msg(eMIDMessageLevel.Error, innerE.ToString(), reportingModule, true, sourceLineNumber, false);
			}
			catch
			{
				throw;
			}
		}

        // Begin TT#46 MD - JSmith - User Dashboard
        public void LogActivity(string aModule, eMIDMessageLevel aMIDMessageLevel, eMIDTextCode aMessageCode, string aMessageDetails, bool aForce)
        {
            if (_logActivity &&
                LogUserDashboardEvent != null)
            {
                LogUserDashboardEvent.LogActivity(this, DateTime.Now, aModule, aMessageCode, aMIDMessageLevel, aMessageDetails, aForce);
            }
        }

        public void LogActivity(string aModule, eMIDMessageLevel aMIDMessageLevel, string aMessageDetails, bool aForce)
        {
            if (_logActivity &&
                LogUserDashboardEvent != null)
            {
                LogUserDashboardEvent.LogActivity(this, DateTime.Now, aModule, eMIDTextCode.Unassigned, aMIDMessageLevel, aMessageDetails, aForce);
            }
        }
        // End TT#46 MD - JSmith - User Dashboard

        //Begin TT#506-MD -jsobek -Add infrastructure to allow Email to be sent from the application
        private static DataSet dsAuditEmailMessage = null;
        private static DataSet dsSystemEmailFilters = null;
        private static string emailEnvironmentalInfo = null;
        private static string emailEnvironmentalBusinessInfo = null;
        private static bool canEmailAuditMessages = false;
        private static bool readEmailOptions = false;
        private void EmailAuditMessage(string aModule, eMIDMessageLevel aMIDMessageLevel, eMIDTextCode aMessageCode, string aMessageDetails)
        {
            // Begin TT#3444 - JSmith - Severe error during Purge
            lock (_emailLock)
            {
            // End TT#3444 - JSmith - Severe error during Purge
                //check to ensure SMTP flag is enabled before attempting to email the audit message
                if (readEmailOptions == false)
                {
                    readEmailOptions = true;
                    GlobalOptions globalOptions = new GlobalOptions();
                    DataTable dtGlobalOptions = globalOptions.GetGlobalOptions();
                    if (dtGlobalOptions.Rows[0]["SMTP_ENABLED"] != DBNull.Value)
                    {
                        if ((string)dtGlobalOptions.Rows[0]["SMTP_ENABLED"] != "0")
                        {
                            canEmailAuditMessages = true;
                        }
                    }
                }

                if (canEmailAuditMessages == false)
                {
                    return;
                }

                if (dsAuditEmailMessage == null)
                {
                    dsAuditEmailMessage = new DataSet();
                    dsAuditEmailMessage.Tables.Add("TEMPMSG");
                    dsAuditEmailMessage.Tables[0].Columns.Add("Time", typeof(DateTime));
                    dsAuditEmailMessage.Tables[0].Columns.Add("Module", typeof(String));
                    dsAuditEmailMessage.Tables[0].Columns.Add("MessageLevel", typeof(int));
                    dsAuditEmailMessage.Tables[0].Columns.Add("MessageLevelText", typeof(String));
                    dsAuditEmailMessage.Tables[0].Columns.Add("MessageCode", typeof(int));
                    dsAuditEmailMessage.Tables[0].Columns.Add("MessageText", typeof(String));
                    dsAuditEmailMessage.Tables[0].Columns.Add("Details", typeof(String));
                    dsAuditEmailMessage.CaseSensitive = false;
                }
                dsAuditEmailMessage.Tables[0].Rows.Clear();
                DataRow dr = dsAuditEmailMessage.Tables[0].NewRow();

                DateTime messageTime = DateTime.Now;
                string messageModule = aModule;
                int messageLevel = Convert.ToInt32(aMIDMessageLevel);
                string messageLevelText = MIDText.GetTextOnly(Convert.ToInt32(aMIDMessageLevel)); ;
                int messageCode = Convert.ToInt32(aMessageCode);
                string messageText = MIDText.GetText(aMessageCode);
                string messageDetails = aMessageDetails;

                dr["Time"] = messageTime;
                dr["Module"] = messageModule;
                dr["MessageLevel"] = messageLevel;
                dr["MessageLevelText"] = messageLevelText;
                dr["MessageCode"] = messageCode;
                dr["MessageText"] = messageText;
                dr["Details"] = messageDetails;
                dsAuditEmailMessage.Tables[0].Rows.Add(dr);


                if (dsSystemEmailFilters == null)
                {
                    GlobalOptions globalOptions = new GlobalOptions();
                    dsSystemEmailFilters = new DataSet();
                    dsSystemEmailFilters.Tables.Add(globalOptions.GetSystemEmail());
                }

                DataRow[] drEmail;
                foreach (DataRow drSystemEmail in dsSystemEmailFilters.Tables[0].Rows)
                {
                    string emailFilter = (string)drSystemEmail["SYSTEM_EMAIL_FILTER"];
                    drEmail = dsAuditEmailMessage.Tables[0].Select(emailFilter);
                    if (drEmail.Length > 0)
                    {
                        //Email this message
                        string emailFrom = string.Empty; //MIDEmail.MIDEmailApplicationFromAddress; //TT#3600 -jsobek -Add a default email address on the global options screen...
                        if (drSystemEmail["SYSTEM_EMAIL_FROM"] != DBNull.Value)
                        {
                            string tempFrom = (string)drSystemEmail["SYSTEM_EMAIL_FROM"];
                            if (MIDEmail.IsAddressValid(tempFrom) == true)
                            {
                                emailFrom = tempFrom;
                            }
                        }
                        string emailTo = MIDEmail.MIDEmailSupportTOAddress;
                        if (drSystemEmail["SYSTEM_EMAIL_TO"] != DBNull.Value)
                        {
                            string tempTo = (string)drSystemEmail["SYSTEM_EMAIL_TO"];
                            if (MIDEmail.IsAddressValid(tempTo) == true)
                            {
                                emailTo = tempTo;
                            }
                        }
                        string emailCc = String.Empty;
                        if (drSystemEmail["SYSTEM_EMAIL_CC"] != DBNull.Value)
                        {
                            string tempCc = (string)drSystemEmail["SYSTEM_EMAIL_CC"];
                            if (MIDEmail.IsAddressValid(tempCc) == true)
                            {
                                emailCc = tempCc;
                            }
                        }
                        string emailBcc = String.Empty;
                        if (drSystemEmail["SYSTEM_EMAIL_BCC"] != DBNull.Value)
                        {
                            string tempBcc = (string)drSystemEmail["SYSTEM_EMAIL_BCC"];
                            if (MIDEmail.IsAddressValid(tempBcc) == true)
                            {
                                emailBcc = tempBcc;
                            }
                        }
                        string emailSubject = String.Empty;
                        if (drSystemEmail["SYSTEM_EMAIL_SUBJECT"] != DBNull.Value)
                        {
                            emailSubject = (string)drSystemEmail["SYSTEM_EMAIL_SUBJECT"];
                            emailSubject = EmailSubjectAndBodyParser(emailSubject, false, messageTime.ToString("yyyy-MM-dd") + " " + messageTime.ToLongTimeString(), messageModule, messageLevel.ToString(), messageLevelText, messageCode.ToString(), messageText, messageDetails);
                        }

                        string emailBody = String.Empty;
                        if (drSystemEmail["SYSTEM_EMAIL_BODY"] != DBNull.Value)
                        {
                            emailBody = (string)drSystemEmail["SYSTEM_EMAIL_BODY"];
                            emailBody = EmailSubjectAndBodyParser(emailBody, true, messageTime.ToString("yyyy-MM-dd") + " " + messageTime.ToLongTimeString(), messageModule, messageLevel.ToString(), messageLevelText, messageCode.ToString(), messageText, messageDetails);

                        }
                        string emailAttachmentFileName = String.Empty;
                        if (drSystemEmail["SYSTEM_EMAIL_ATTACHMENT_FILENAME"] != DBNull.Value)
                        {
                            emailAttachmentFileName = (string)drSystemEmail["SYSTEM_EMAIL_ATTACHMENT_FILENAME"];
                        }





                        string emailReturnMsg = String.Empty;
                        if (emailEnvironmentalInfo == null)  //only get the environment info one time
                        {
                            emailEnvironmentalInfo = EnvironmentInfo.MIDInfo.GetAllEnvironmentInfo(Environment.NewLine);
                        }
                        if (emailEnvironmentalBusinessInfo == null)  //only get the environment business info one time
                        {
                            //emailEnvironmentalBusinessInfo = String.Empty;
                            if (this.getEnvironmentBusinessInfo != null)
                            {
                                emailEnvironmentalBusinessInfo = this.getEnvironmentBusinessInfo.Invoke();
                            }
                        }



                        if (MIDEmail.SendEmail(out emailReturnMsg,
                                               MIDEmail.CreateEmailSystemMessage(messageTime,
                                                                                 messageModule,
                                                                                 messageLevel,
                                                                                 messageLevelText,
                                                                                 messageCode,
                                                                                 messageText,
                                                                                 messageDetails,
                                                                                 emailEnvironmentalInfo,
                                                                                 emailEnvironmentalBusinessInfo,
                                                                                 emailFrom,
                                                                                 emailTo,
                                                                                 emailCc,
                                                                                 emailBcc,
                                                                                 emailSubject,
                                                                                 emailBody,
                                                                                 emailAttachmentFileName)) == MIDEmail.emailReturnMessageTypes.Success)
                        {
                            //MessageBox.Show("Email sent.", MIDEmail.emailReturnMessages.Success);
                        }
                        else
                        {
                            //MessageBox.Show(emailReturnMsg);
                        }

                    }
                }
            // Begin TT#3444 - JSmith - Severe error during Purge
            }
            // End TT#3444 - JSmith - Severe error during Purge
        }

        /// <summary>
        /// Replaces message fields/abbreviations with their values in an email subject or email body
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        private string EmailSubjectAndBodyParser(string unparsedSubjectOrBody, bool isBody, string messageTime, string messageModule, string messageLevel, string messageLevelText, string messageCode, string messageText, string messageDetails)
        {
            string s = unparsedSubjectOrBody;
            s = s.Replace("<Time>", messageTime).Replace("<T>", messageTime);
            s = s.Replace("<Module>", messageModule).Replace("<M>", messageModule);
            s = s.Replace("<MessageLevel>", messageLevel).Replace("<ML>", messageLevel);
            s = s.Replace("<MessageLevelText>", messageLevelText).Replace("<MLT>", messageLevelText);
            s = s.Replace("<MessageCode>", messageCode).Replace("<MC>", messageCode);
            s = s.Replace("<MessageText>", messageText).Replace("<MT>", messageText);
            if (isBody)
            {
                s = s.Replace("<Details>", messageDetails).Replace("<D>", messageDetails);
            }
            return s;
        }
        //public void EmailScheduledTasks(string aModule, eMIDMessageLevel aMIDMessageLevel, eMIDTextCode aMessageCode, string aMessageDetails)
        //{
        //    if (_process == eProcesses.schedulerService)
        //    {
        //    }
        //}
        public void EmailScheduledTasks(DataRow taskRow, eMIDMessageLevel taskMsgLevel, string taskListName)
        {
            //try 
            //{
            bool wasSuccessful = true;
            eProcessCompletionStatus taskStatus = eProcessCompletionStatus.Successful;
            int maxMsgLevel = (int)taskRow["MAX_MESSAGE_LEVEL"]; 
            int taskListRID = (int)taskRow["TASKLIST_RID"];
            string taskTypeName = ((eTaskType)Convert.ToInt32(taskRow["TASK_TYPE"])).ToString();
			//BEGIN TT#4574 - DOConnell - Purge Task List does not have an email option
            if (taskTypeName == "computationDriver")
            {
                taskTypeName = "Chain Forecasting";
            }
			//END TT#4574 - DOConnell - Purge Task List does not have an email option
			
            if ((int)taskMsgLevel >= maxMsgLevel)
            {
                wasSuccessful = false;
                taskStatus = eProcessCompletionStatus.Failed;
            }

            bool shouldEmail = false;

            if (wasSuccessful)
            {
                if (taskRow["EMAIL_SUCCESS_TO"] != DBNull.Value && (string)taskRow["EMAIL_SUCCESS_TO"] != String.Empty)
                {
                    shouldEmail = true;
                }
            }
            else
            {
                if (taskRow["EMAIL_FAILURE_TO"] != DBNull.Value && (string)taskRow["EMAIL_FAILURE_TO"] != String.Empty)
                {
                    shouldEmail = true;
                }
            }


            if (shouldEmail)
            {
                //string emailFrom = "app@midretail.com"; //TT#3600 -jsobek -Add a default email address on the global options screen...
                string emailFrom = string.Empty; //TT#3600 -jsobek -Add a default email address on the global options screen...
                string emailTo = String.Empty;
                string emailCC = null;
                string emailBCC = null;
                string emailSubject = String.Empty;
                string emailBody = String.Empty;

                //Set the default subject and body
                emailSubject = "Task " + taskTypeName + " on Task List " + taskListName;
                if (wasSuccessful)
                {
                    emailSubject += " was successful";
                }
                else
                {
                    emailSubject += " has failed";
                }
                
                string newline = MIDEmail.GetNewline();
                emailBody += emailSubject + newline;
                emailBody += "Status: " + MIDText.GetTextOnly((int)taskStatus) + " (" + MIDText.GetTextOnly((int)taskMsgLevel) + ")" + newline + newline; 
                emailBody += MIDEmail.CreateBasicUserInfoTextBlockWithDateTime(MIDEmail.GetNewline());




                if (wasSuccessful)
                {
                    if (taskRow["EMAIL_SUCCESS_FROM"] != DBNull.Value && (string)taskRow["EMAIL_SUCCESS_FROM"] != String.Empty)
                    {
                        emailFrom = (string)taskRow["EMAIL_SUCCESS_FROM"];
                    }
                    emailTo = (string)taskRow["EMAIL_SUCCESS_TO"];
                    if (taskRow["EMAIL_SUCCESS_CC"] != DBNull.Value && (string)taskRow["EMAIL_SUCCESS_CC"] != String.Empty)
                    {
                        emailCC = (string)taskRow["EMAIL_SUCCESS_CC"];
                    }
                    if (taskRow["EMAIL_SUCCESS_BCC"] != DBNull.Value && (string)taskRow["EMAIL_SUCCESS_BCC"] != String.Empty)
                    {
                        emailBCC = (string)taskRow["EMAIL_SUCCESS_BCC"];
                    }
                    if (taskRow["EMAIL_SUCCESS_SUBJECT"] != DBNull.Value && (string)taskRow["EMAIL_SUCCESS_SUBJECT"] != String.Empty)
                    {
                        emailSubject = (string)taskRow["EMAIL_SUCCESS_SUBJECT"];
                    }
                    if (taskRow["EMAIL_SUCCESS_BODY"] != DBNull.Value && (string)taskRow["EMAIL_SUCCESS_BODY"] != String.Empty)
                    {
                        emailBody = (string)taskRow["EMAIL_SUCCESS_BODY"];
                    }
                }
                else
                {
                    if (taskRow["EMAIL_FAILURE_FROM"] != DBNull.Value && (string)taskRow["EMAIL_FAILURE_FROM"] != String.Empty)
                    {
                        emailFrom = (string)taskRow["EMAIL_FAILURE_FROM"];
                    }

                    emailTo = (string)taskRow["EMAIL_FAILURE_TO"];
                    if (taskRow["EMAIL_FAILURE_CC"] != DBNull.Value && (string)taskRow["EMAIL_FAILURE_CC"] != String.Empty)
                    {
                        emailCC = (string)taskRow["EMAIL_FAILURE_CC"];
                    }
                    if (taskRow["EMAIL_FAILURE_BCC"] != DBNull.Value && (string)taskRow["EMAIL_FAILURE_BCC"] != String.Empty)
                    {
                        emailBCC = (string)taskRow["EMAIL_FAILURE_BCC"];
                    }
                    if (taskRow["EMAIL_FAILURE_SUBJECT"] != DBNull.Value && (string)taskRow["EMAIL_FAILURE_SUBJECT"] != String.Empty)
                    {
                        emailSubject = (string)taskRow["EMAIL_FAILURE_SUBJECT"];
                    }
                    if (taskRow["EMAIL_FAILURE_BODY"] != DBNull.Value && (string)taskRow["EMAIL_FAILURE_BODY"] != String.Empty)
                    {
                        emailBody = (string)taskRow["EMAIL_FAILURE_BODY"];
                    }
                }



                
                string emailReturnMessage = String.Empty;
                MIDEmail.SendEmail(out emailReturnMessage, emailSubject, emailBody, emailFrom, emailTo, emailCC, emailBCC);
            }
                //if (_process == eProcesses.schedulerService)
                //{
                //}
            //}
            //catch 
            //{
            //}
        }
        //End TT#506-MD -jsobek -Add infrastructure to allow Email to be sent from the application

		private void SaveHighestMessageLevel(eMIDMessageLevel messageLevel)
		{
			if (messageLevel > _highestMessageLevel)
			{
				_highestMessageLevel = messageLevel;
			}
		}

        ///// <summary>
        ///// Retrieves text for a message code.
        ///// </summary>
        ///// <param name="messageCode">The code for the message to be retrieved.</param>
        ///// <returns></returns>
        ///// <remarks>
        ///// Adds the message to the audit report in addition to returning the text to the caller.
        ///// </remarks>
        //public string GetText(eMIDTextCode messageCode)
        //{
        //    try
        //    {
        //        return GetText(messageCode, true);
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}

        /// <summary>
        /// Retrieves text for a message code.
        /// </summary>
        /// <param name="messageCode">The code for the message to be retrieved.</param>
        /// <returns></returns>
        /// <remarks>
        /// Adds the message to the audit report in addition to returning the text to the caller.
        /// </remarks>
        public string GetText(eMIDTextCode messageCode, params object[] args)
        {
            try
            {
                return GetText(messageCode, true, args);
            }
            catch
            {
                throw;
            }
        }

		/// <summary>
		/// Retrieves text for a message code.
		/// </summary>
		/// <param name="messageCode">The code for the message to be retrieved.</param>
		/// <param name="addToAuditReport">Identifies if the text should be added to the audit report</param>
		/// <returns></returns>
		/// <remarks>
		/// Optionally adds the message to the audit report in addition to returning the text to the caller.
		/// </remarks>
		public string GetText(eMIDTextCode messageCode, bool addToAuditReport, params object[] args)
		{
			try
			{
				string msg = null;
                string textValue;
                eMIDMessageLevel messageLevel = eMIDMessageLevel.Error;
				// line number is only available if during a debug compile 
				int lineNumber = 0;
#if (DEBUG)
				lineNumber = new System.Diagnostics.StackFrame(1,true).GetFileLineNumber();
#endif
				string reportingModule = "Unknown";
				if (addToAuditReport)
				{
					reportingModule = new System.Diagnostics.StackFrame(1,true).GetFileName();
				}
                
                //DataTable dt = MIDText.GetMsg(messageCode);
                //if (dt.Rows.Count == 1)
                //{
                //    DataRow dr = dt.Rows[0];
                //    msg = ((int)messageCode).ToString(CultureInfo.CurrentUICulture) + ":" + (string) dr["TEXT_VALUE"];
                //    if (args != null && args.Length > 0)
                //    {
                //        msg = string.Format(msg, args);
                //    }
                //    msg = msg.Replace("{newline}", System.Environment.NewLine);
                //    msg = msg.Replace(@"\n", Environment.NewLine);
                //    messageLevel = (eMIDMessageLevel)(Convert.ToInt32(dr["TEXT_LEVEL"], CultureInfo.CurrentUICulture));
                //    if (addToAuditReport)
                //    {
                //        Add_Msg(messageLevel, messageCode, lineNumber, reportingModule, false);
                //    }
                //}
                MIDText.GetMsg(messageCode, out textValue, out messageLevel);
                if (textValue != null)
                {
                    msg = ((int)messageCode).ToString(CultureInfo.CurrentUICulture) + ":" + textValue;
                    if (args != null && args.Length > 0)
                    {
                        msg = string.Format(msg, args);
                    }
                    msg = msg.Replace("{newline}", System.Environment.NewLine);
                    msg = msg.Replace(@"\n", Environment.NewLine);
                    if (addToAuditReport)
                    {
                        Add_Msg(messageLevel, messageCode, lineNumber, reportingModule, false, string.Empty, string.Empty, 0);
                    }
                }
				else
				{
					msg = Include.ErrorBadTextCode + messageCode.ToString();
				}
					
				return msg;
			}
			catch
			{
				throw;
			}
		}

        // Begin TT#3435 - JSmith - Errors messages not complete with encounter database errors.
        /// <summary>
		/// Gets the message level associated with a message.
		/// </summary>
		/// <param name="messageCode">The code of the message.</param>
        public eMIDMessageLevel GetMessageLevel(eMIDTextCode messageCode)
		{
			try
			{
                return MIDText.GetMessageLevel((int)messageCode);
            }
            catch
            {
                throw;
            }
        }
        // End TT#3435 - JSmith - Errors messages not complete with encounter database errors.

		/// <summary>
		/// Adds and exception.
		/// </summary>
		/// <param name="aException"></param>
//		public void Add(MIDException aException)
//		{
//			int lineNum = new System.Diagnostics.StackFrame(1,true).GetFileLineNumber();
//			string fileName = new System.Diagnostics.StackFrame(1,true).GetFileName();
////			_errorQueue.Enqueue(aException);
//
//			//Log_MIDException(aException);
//			if (aException.ErrorLevel > eErrorLevel.warning)
//			{
//				throw aException;
//			}
//		}

//		public void Add(MIDException aException, string sourceModule )
//		{
//			int lineNum = new System.Diagnostics.StackFrame(1,true).GetFileLineNumber();
//			string fileName = new System.Diagnostics.StackFrame(1,true).GetFileName();
//			//			_errorQueue.Enqueue(aException);
//
//			//Log_MIDException(aException, sourceModule);
//			if (aException.ErrorLevel > eErrorLevel.warning)
//			{
//				throw aException;
//			}
//		}

		/// <summary>
		/// Retrieves the next error from the error queue.
		/// </summary>
		/// <returns></returns>
		public MIDException GetNextError()
		{
			return (MIDException)_errorQueue.Dequeue();
		}

		/// <summary>
		/// Add hierarchy record counts to the audit report.
		/// </summary>
		/// <param name="hierarchyRecs">The number of hierarchy records processed.</param>
		/// <param name="levelRecs">The number of level records processed.</param>
		/// <param name="productRecs">The number of product records processed.</param>
		/// <param name="recordsWithErrors">The number of records with errors.</param>
		/// <param name="aMoveRecs">The number of reclass move records.</param>
		/// <param name="aRenameRecs">The number of reclass rename records.</param>
		/// <param name="aDeleteRecs">The number of reclass delete records.</param>
        /// <param name="aProductsAdded">The number of productRecs that resulted in a product being added.</param>
        /// <param name="aProductsUpdated">The number of productRecs that resulted in a product being updated.</param>
        //Begin TT#106 MD - JSmith - Number of Nodes Added needs to be included in the Audit > Summary of the Hierarchy Load API
        //public void HierarchyLoadAuditInfo_Add(int hierarchyRecs, int levelRecs,
        //    int productRecs, int recordsWithErrors, int aMoveRecs, int aRenameRecs, int aDeleteRecs)
        public void HierarchyLoadAuditInfo_Add(int hierarchyRecs, int levelRecs,
            int productRecs, int recordsWithErrors, int aMoveRecs, int aRenameRecs, int aDeleteRecs,
            int aProductsAdded, int aProductsUpdated)
        //End TT#106 MD
		{
            // Begin TT#1243 - JSmith - Audit Performance
            //AuditData auditData = new AuditData();
            // End TT#1243
			try
			{
                _writingAudit = true;
				OpenUpdateConnection(auditData);
                //Begin TT#106 MD - JSmith - Number of Nodes Added needs to be included in the Audit > Summary of the Hierarchy Load API
                //auditData.HierarchyLoadAuditInfo_Add(_processRID, hierarchyRecs, levelRecs, productRecs, recordsWithErrors,
                //    aMoveRecs, aRenameRecs, aDeleteRecs);
                auditData.HierarchyLoadAuditInfo_Add(_processRID, hierarchyRecs, levelRecs, productRecs, recordsWithErrors,
                    aMoveRecs, aRenameRecs, aDeleteRecs,aProductsAdded, aProductsUpdated);
                //End TT#106 MD
				CommitData(auditData);
			}
			catch
			{
				throw;
			}
            finally
            {
                _writingAudit = false;
                CloseUpdateConnection(auditData);
            }
		}
//Begin TT228 - RBeck - Hierarchy Reclass message incorrectly displayed  
        /// <summary>
        /// Add hierarchy record counts to the audit report.
        /// </summary>
        /// <param name="hierarchyReClsRecs">The number of hierarchy reclass records processed.</param>
        /// <param name="addChgReClsRecs">The number of hierarchy reclass records added or changed.</param>
        /// <param name="deleteReClsRecs">The number of hierarchy reclass records deleted.</param>
        /// <param name="moveReClsRecs">The number of hierarchy reclass move records.</param>
        /// <param name="rejectReClsRecs">The number of hierarchy reclass move records.</param>
        /// <param name="success">The Processing completed successfully without Transaction errors.</param>
        public void HierarchyReclassAuditInfo_Add(  int hierarchyReClsRecs
                                                  , int addChgReClsRecs
                                                  , int deleteReClsRecs
                                                  , int moveReClsRecs
                                                  , int rejectReClsRecs
                                                )
        {

            try
            {
                _writingAudit = true;
                OpenUpdateConnection(auditData);
                auditData.HierarchyReclassAuditInfo_Add(  _processRID
                                                          , hierarchyReClsRecs
                                                          , addChgReClsRecs, deleteReClsRecs
                                                          , moveReClsRecs, rejectReClsRecs
                                                        );
               
                CommitData(auditData);
            }
            catch
            {
                throw;
            }
            finally
            {
                _writingAudit = false;
                CloseUpdateConnection(auditData);
            }
        }
//End   TT228 - RBeck - Hierarchy Reclass message incorrectly displayed  

		// Begin TT#1581-MD - stodd - API Header Reconcile
        public void HeaderReconcileAuditInfo_Add(int filesRead
                                            , int recsRead
                                            , int recsWritten
                                            , int filesWritten
                                            , int duplicateRecsFound
                                            , int skippedRecs
                                            , int removeRecsWritten
                                            , int removeFilesWritten

                                        )
        {

            try
            {
                _writingAudit = true;
                OpenUpdateConnection(auditData);
                auditData.HeaderReconcileAuditInfo_Add(_processRID
                                                          , filesRead
                                                          , recsRead
                                                          , recsWritten
                                                          , filesWritten
                                                          , duplicateRecsFound
                                                          , skippedRecs
                                                          , removeRecsWritten
                                                          , removeFilesWritten

                                                        );

                CommitData(auditData);
            }
            catch
            {
                throw;
            }
            finally
            {
                _writingAudit = false;
                CloseUpdateConnection(auditData);
            }
        }
		// End TT#1581-MD - stodd - API Header Reconcile

		/// <summary>
		/// Add plan/history record counts to the audit report.
		/// </summary>
		/// <param name="chainDailyHistoryRecs">The number of chain daily history records processed.</param>
		/// <param name="chainWeeklyHistoryRecs">The number of chain weekly history records processed.</param>
		/// <param name="chainWeeklyForecastRecs">The number of chain weekly forecast records processed.</param>
		/// <param name="storeDailyHistoryRecs">The number of store daily history records processed.</param>
		/// <param name="storeWeeklyHistoryRecs">The number of store weekly history records processed.</param>
		/// <param name="storeWeeklyForecastRecs">The number of store weekly forecast records processed.</param>
		/// <param name="intransitRecs">The number of intransit records processed.</param>
		/// <param name="recordsWithErrors">The number of records with errors.</param>
		/// <param name="aNodesAdded">The number of nodes added by the process</param>
		public void PostingAuditInfo_Add(int chainDailyHistoryRecs, int chainWeeklyHistoryRecs,
			int chainWeeklyForecastRecs, int storeDailyHistoryRecs, int storeWeeklyHistoryRecs, int storeWeeklyForecastRecs,
			int intransitRecs, int recordsWithErrors, int aNodesAdded)
		{
            // Begin TT#1243 - JSmith - Audit Performance
            //AuditData auditData = new AuditData();
            // End TT#1243
			try
			{
                _writingAudit = true;
				OpenUpdateConnection(auditData);
				auditData.PostingAuditInfo_Add(_processRID, chainDailyHistoryRecs, chainWeeklyHistoryRecs,
					chainWeeklyForecastRecs, storeDailyHistoryRecs, storeWeeklyHistoryRecs, storeWeeklyForecastRecs,
					intransitRecs, recordsWithErrors, aNodesAdded);
				CommitData(auditData);
			}
			catch
			{
				throw;
			}
            finally
            {
                _writingAudit = false;
                CloseUpdateConnection(auditData);
            }
		}

		/// <summary>
		/// Add store record counts to the audit report.
		/// </summary>
		/// <param name="storeRecs">The number of store records processed.</param>
		/// <param name="recordsWithErrors">The number of records with errors.</param>
		/// <param name="recordsAdded">The number of records added.</param>
		/// <param name="recordsUpdated">The number of records updated.</param>
		// Begin MID Track #4668 - add number added and modified
//		public void StoreLoadAuditInfo_Add(int storeRecs, int recordsWithErrors)
		public void StoreLoadAuditInfo_Add(int storeRecs, int recordsWithErrors, int recordsAdded, int recordsUpdated, int recordsDeleted, int recordsRecovered)	// TT#739-MD - STodd - delete stores
		// End MID Track #4668
		{
            // Begin TT#1243 - JSmith - Audit Performance
            //AuditData auditData = new AuditData();
            // End TT#1243
			try
			{
                _writingAudit = true;
				OpenUpdateConnection(auditData);
				// Begin MID Track #4668 - add number added and modified
//				auditData.StoreLoadAuditInfo_Add(_processRID, storeRecs, recordsWithErrors);
				auditData.StoreLoadAuditInfo_Add(_processRID, storeRecs, recordsWithErrors, recordsAdded, recordsUpdated, recordsDeleted, recordsRecovered);	// TT#739-MD - STodd - delete stores
				// End MID Track #4668
				CommitData(auditData);
			}
			catch
			{
				throw;
			}
            finally
            {
                _writingAudit = false;
                CloseUpdateConnection(auditData);
            }
		}

		// BEGIN Issue 5117 stodd 4.17.2008
		public void SpecialRequestAuditInfo_Add(int totalJobs, int jobsProcessed, int jobsWithErrors, int successfulJobs)
		{
            // Begin TT#1243 - JSmith - Audit Performance
            //AuditData auditData = new AuditData();
            // End TT#1243
			try
			{
                _writingAudit = true;
				OpenUpdateConnection(auditData);
				auditData.SpecialRequestAuditInfo_Add(_processRID, totalJobs, jobsProcessed, jobsWithErrors, successfulJobs);
				CommitData(auditData);
			}
			catch
			{
				throw;
			}
            finally
            {
                _writingAudit = false;
                CloseUpdateConnection(auditData);
            }
		}

		/// <summary>
		/// Add purge record counts to the audit report.
		/// </summary>
		/// <param name="storeDailyHistoryRecs">The number of store daily history records purged.</param>
		/// <param name="chainWeeklyHistoryRecs">The number of chain weekly history records purged.</param>
		/// <param name="storeWeeklyHistoryRecs">The number of store weekly history records purged.</param>
		/// <param name="chainWeeklyForecastRecs">The number of chain weekly forecast records purged.</param>
		/// <param name="storeWeeklyForecastRecs">The number of store weekly forecast records purged.</param>
		/// <param name="headerRecs">The number of header records purged.</param>
		public void PurgeAuditInfo_Add(int storeDailyHistoryRecs, 
                                       int chainWeeklyHistoryRecs,
			                           int storeWeeklyHistoryRecs, 
                                       int chainWeeklyForecastRecs, 
                                       int storeWeeklyForecastRecs,
			                           int headerRecs, 
                                       int aIntransitRecs, 
                                       int aIntransitReviewRecs, 
                                       int aUserRecs, 
                                       int aGroupRecs, 
                                       int aAuditRecs, 
                                       int aDailyPercentages, 
                                       int emptyStoreSets,	// TT#739-MD - STodd - delete stores
                                       int aImoRevRecs) //TT#1576-MD -jsobek -Data Layer Request - Add VSW Revision Records to Purge
            
		{
			try
			{
                _writingAudit = true;
				OpenUpdateConnection(auditData);
				auditData.PurgeAuditInfo_Add(_processRID, 
                    storeDailyHistoryRecs, 
                    chainWeeklyHistoryRecs,
					storeWeeklyHistoryRecs, 
                    chainWeeklyForecastRecs, 
                    storeWeeklyForecastRecs,
					headerRecs, 
                    aIntransitRecs, 
                    aIntransitReviewRecs, 
                    aUserRecs, 
                    aGroupRecs, 
                    aAuditRecs, 
                    aDailyPercentages, 
                    emptyStoreSets,
                    aImoRevRecs
                    );	
         

				CommitData(auditData);
			}
			catch
			{
				throw;
			}
            finally
            {
                _writingAudit = false;
                CloseUpdateConnection(auditData);
            }
		}

		// Begin TT#465 - stodd - size day to week summary
        //Begin TT#846-MD -jsobek -New Stored Procedures for Performance -unused function
        //public void SizeDayToWeekSummaryAuditInfo_Add(int totalStyles, int totalColors,
        //    int totalSizes, int totalValuesRead, int totalRecsRead, int totalRecsWritten,
        //    int totalErrors)
        //{
        //    // Begin TT#1243 - JSmith - Audit Performance
        //    //AuditData auditData = new AuditData();
        //    // End TT#1243
        //    try
        //    {
        //        _writingAudit = true;
        //        OpenUpdateConnection(auditData);
        //        auditData.SizeDayToWeekSummary_Add(_processRID, totalStyles, totalColors,
        //            totalSizes, totalValuesRead, totalRecsRead,
        //            totalRecsWritten, totalErrors);
        //        CommitData(auditData);
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //    finally
        //    {
        //        _writingAudit = false;
        //        CloseUpdateConnection(auditData);
        //    }
        //}
        //End TT#846-MD -jsobek -New Stored Procedures for Performance -unused function
		// End TT#465 - stodd - size day to week summary

		// BEGIN TT#1401 - stodd - add resevation stores (IMO)
		public void PushToBackStockAuditInfo_Add(int headersRead, int HeadersWithErrors,
			int HeadersProcessed, int HeadersSkipped)
		{
			try
			{
				OpenUpdateConnection(auditData);
				auditData.PushToBackStock_Add(_processRID, headersRead, HeadersWithErrors,
					HeadersProcessed, HeadersSkipped);
				CommitData(auditData);
			}
			catch
			{
				throw;
			}
		}
		// END TT#1401 - stodd - add resevation stores (IMO)

		/// <summary>
		/// Add header load record counts to the audit report.
		/// </summary>
		/// <param name="aRecsRead">The number of header records read.</param>
		/// <param name="aRecsWithErrors">The number of header records with errors.</param>
		/// <param name="aHdrsCreated">The number of headers created.</param>
		/// <param name="aHdrsModified">The number of headers modified.</param>
		/// <param name="aHdrsRemoved">The number of headers removed.</param>
		/// <param name="aHdrsReset">The number of headers reset.</param>
		public void HeaderLoadAuditInfo_Add(int aRecsRead, int aRecsWithErrors,
			int aHdrsCreated, int aHdrsModified, int aHdrsRemoved, int aHdrsReset)
		{
            // Begin TT#1243 - JSmith - Audit Performance
            //AuditData auditData = new AuditData();
            // End TT#1243
			try
			{
                _writingAudit = true;
				OpenUpdateConnection(auditData);
				auditData.HeaderLoadAuditInfo_Add(_processRID, aRecsRead, aRecsWithErrors,
					aHdrsCreated, aHdrsModified, aHdrsRemoved, aHdrsReset);
				CommitData(auditData);
			}
			catch
			{
				throw;
			}
            finally
            {
                _writingAudit = false;
                CloseUpdateConnection(auditData);
            }
		}

		//Begin MOD - JScott - Build Pack Criteria Load
		/// <summary>
		/// Add build pack criteria load record counts to the audit report.
		/// </summary>
		/// <param name="aRecsRead">The number of build pack criteria records read.</param>
		/// <param name="aRecsWithErrors">The number of build pack criteria records with errors.</param>
		/// <param name="aHdrsCreated">The number of build pack criterias added or updated.</param>
		public void BuildPackCriteriaLoadAuditInfo_Add(int aRecsRead, int aRecsWithErrors, int aCriteriaAddedUpdated)
		{
            // Begin TT#1243 - JSmith - Audit Performance
            //AuditData auditData = new AuditData();
            // End TT#1243
			try
			{
                _writingAudit = true;
				OpenUpdateConnection(auditData);
				auditData.BuildPackCriteriaLoadAuditInfo_Add(_processRID, aRecsRead, aRecsWithErrors, aCriteriaAddedUpdated);
				CommitData(auditData);
			}
			catch
			{
				throw;
			}
            finally
            {
                _writingAudit = false;
                CloseUpdateConnection(auditData);
            }
		}

        //Begin TT#1499 - DOConnell - Chain Plan - Set Percentages Phase 2
        /// <summary>
        /// Add Chain Set Percent criteria load record counts to the audit report.
        /// </summary>
        /// <param name="aRecsRead">The number of Chain Set Percent criteria records read.</param>
        /// <param name="aRecsWithErrors">The number of Chain Set Percent criteria records with errors.</param>
        /// <param name="aHdrsCreated">The number of Chain Set Percent criterias added or updated.</param>
        public void ChainSetPercentCriteriaLoadAuditInfo_Add(int aRecsRead, int aRecsWithErrors, int aCriteriaAddedUpdated)
        {
            try
            {
                OpenUpdateConnection(auditData);
                auditData.ChainSetPercentCriteriaLoadAuditInfo_Add(_processRID, aRecsRead, aRecsWithErrors, aCriteriaAddedUpdated);
                CommitData(auditData);
            }
            catch
            {
                throw;
            }
        }
        //End TT#1499 - DOConnell - Chain Plan - Set Percentages Phase 2

        //Begin TT#43 – MD – DOConnell – Projected Sales Enhancement
        /// <summary>
        /// Add Chain Set Percent criteria load record counts to the audit report.
        /// </summary>
        /// <param name="aRecsRead">The number of Chain Set Percent criteria records read.</param>
        /// <param name="aRecsWithErrors">The number of Chain Set Percent criteria records with errors.</param>
        /// <param name="aHdrsCreated">The number of Chain Set Percent criterias added or updated.</param>
        public void DailyPercentagesCriteriaLoadAuditInfo_Add(int aRecsRead, int aRecsWithErrors, int aCriteriaAddedUpdated)
        {
            try
            {
                OpenUpdateConnection(auditData);
                auditData.DailyPercentagesCriteriaLoadAuditInfo_Add(_processRID, aRecsRead, aRecsWithErrors, aCriteriaAddedUpdated);
                CommitData(auditData);
            }
            catch
            {
                throw;
            }
        }
        //End TT#43 – MD – DOConnell – Projected Sales Enhancement

        //Begin TT#816 - MD - DOConnell - Node Properties Maintenance Enhancement - Phase 1 - Store Eligibility Load API
        /// <summary>
        /// Add Store Eligibility criteria load record counts to the audit report.
        /// </summary>
        /// <param name="aRecsRead">The number of Chain Set Percent criteria records read.</param>
        /// <param name="aRecsWithErrors">The number of Chain Set Percent criteria records with errors.</param>
        /// <param name="aHdrsCreated">The number of Chain Set Percent criterias added or updated.</param>
        public void StoreEligibilityCriteriaLoadAuditInfo_Add(int aRecsRead, int aRecsWithErrors, int aCriteriaAddedUpdated)
        {
            try
            {
                OpenUpdateConnection(auditData);
                auditData.StoreEligibilityCriteriaLoadAuditInfo_Add(_processRID, aRecsRead, aRecsWithErrors, aCriteriaAddedUpdated);
                CommitData(auditData);
            }
            catch
            {
                throw;
            }
        }
        //End TT#816 - MD - DOConnell - Node Properties Maintenance Enhancement - Phase 1 - Store Eligibility Load API

        //Begin TT#817 - MD - DOConnell - Node Properties Maintenance Enhancement - Phase 1 - Store Eligibility Load API
        /// <summary>
        /// Add VSW criteria load record counts to the audit report.
        /// </summary>
        /// <param name="aRecsRead">The number of Chain Set Percent criteria records read.</param>
        /// <param name="aRecsWithErrors">The number of Chain Set Percent criteria records with errors.</param>
        /// <param name="aHdrsCreated">The number of Chain Set Percent criterias added or updated.</param>
        public void VSWCriteriaLoadAuditInfo_Add(int aRecsRead, int aRecsWithErrors, int aCriteriaAddedUpdated)
        {
            try
            {
                OpenUpdateConnection(auditData);
                auditData.VSWCriteriaLoadAuditInfo_Add(_processRID, aRecsRead, aRecsWithErrors, aCriteriaAddedUpdated);
                CommitData(auditData);
            }
            catch
            {
                throw;
            }
        }
        //End TT#817 - MD - DOConnell - Node Properties Maintenance Enhancement - Phase 1 - Store Eligibility Load API

		/// <summary>
		/// Add color code load record counts to the audit report.
		/// </summary>
		/// <param name="aRecsRead">The number of header records read.</param>
		/// <param name="aRecsWithErrors">The number of color records with errors.</param>
		/// <param name="aCodeAdded">The number of color codes added.</param>
		/// <param name="aCodeUpdated">The number of color codes updated.</param>
		public void ColorCodeLoadAuditInfo_Add(int aRecsRead, int aRecsWithErrors,
			int aCodeAdded, int aCodeUpdated)
		{
            // Begin TT#1243 - JSmith - Audit Performance
            //AuditData auditData = new AuditData();
            // End TT#1243
			try
			{
                _writingAudit = true;
				OpenUpdateConnection(auditData);
				auditData.ColorCodeLoadAuditInfo_Add(_processRID, aRecsRead, aRecsWithErrors,
					aCodeAdded, aCodeUpdated);
				CommitData(auditData);
			}
			catch
			{
				throw;
			}
            finally
            {
                _writingAudit = false;
                CloseUpdateConnection(auditData);
            }
		}

		/// <summary>
		/// Add color code load record counts to the audit report.
		/// </summary>
		/// <param name="aRecsRead">The number of header records read.</param>
		/// <param name="aRecsWithErrors">The number of size code records with errors.</param>
		/// <param name="aCodeAdded">The number of size codes added.</param>
		/// <param name="aCodeUpdated">The number of size codes updated.</param>
		public void SizeCodeLoadAuditInfo_Add(int aRecsRead, int aRecsWithErrors,
			int aCodeAdded, int aCodeUpdated)
		{
            // Begin TT#1243 - JSmith - Audit Performance
            //AuditData auditData = new AuditData();
            // End TT#1243
			try
			{
                _writingAudit = true;
				OpenUpdateConnection(auditData);
				auditData.SizeCodeLoadAuditInfo_Add(_processRID, aRecsRead, aRecsWithErrors,
					aCodeAdded, aCodeUpdated);
				CommitData(auditData);
			}
			catch
			{
				throw;
			}
            finally
            {
                _writingAudit = false;
                CloseUpdateConnection(auditData);
            }
		}

        //BEGIN TT#4183-VStuart-Size Curve Load Audit Summary should not be included in the Audit Details-MID
        /// <summary>
        /// Add size curve load record counts to the audit report.
        /// </summary>
        /// <param name="aCurvesRead">The number of size curve records read.</param>
        /// <param name="aCurvesWithErrors">The number of size curve records with errors.</param>
        /// <param name="aCurvesCreated">The number of size curves added.</param>
        /// <param name="aCurvesModified">The number of size curves updated.</param>
        /// <param name="aCurvesRemoved">The number of size curves removed.</param>
        /// <param name="aGroupsRead">The number of size curve group records read.</param>
        /// <param name="aGroupsWithErrors">The number of size curve group records with errors.</param>
        /// <param name="aGroupsCreated">The number of size curve groups added.</param>
        /// <param name="aGroupsModified">The number of size curve groups updated.</param>
        /// <param name="aGroupsRemoved">The number of size curve groups removed.</param>
        public void SizeCurveLoadAuditInfo_Add(int aCurvesRead, int aCurvesWithErrors, int aCurvesCreated, int aCurvesModified, int aCurvesRemoved, 
            int aGroupsRead, int aGroupsWithErrors, int aGroupsCreated, int aGroupsModified, int aGroupsRemoved)
        {
            try
            {
                _writingAudit = true;
                OpenUpdateConnection(auditData);
                auditData.SizeCurveLoadAuditInfo_Add(_processRID, aCurvesRead, aCurvesWithErrors, aCurvesCreated, aCurvesModified, aCurvesRemoved, 
                    aGroupsRead, aGroupsWithErrors, aGroupsCreated, aGroupsModified, aGroupsRemoved);
                CommitData(auditData);
            }
            catch
            {
                throw;
            }
            finally
            {
                _writingAudit = false;
                CloseUpdateConnection(auditData);
            }
        }
        //END TT#4183-VStuart-Size Curve Load Audit Summary should not be included in the Audit Details-MID

		/// <summary>
		/// Add color code load record counts to the audit report.
		/// </summary>
		/// <param name="aRecsRead">The number of header records read.</param>
		/// <param name="aRecsWithErrors">The number of size code records with errors.</param>
		/// <param name="aCodeAdded">The number of size codes added.</param>
		/// <param name="aCodeUpdated">The number of size codes updated.</param>
		public void SizeConstraintsLoadAuditInfo_Add(int aRecsRead, int aRecsWithErrors, 
													 int modelsCreated, int modelsModified, int ModelsRemoved)
		{
            // Begin TT#1243 - JSmith - Audit Performance
            //AuditData auditData = new AuditData();
            // End TT#1243
			try
			{
                _writingAudit = true;
				OpenUpdateConnection(auditData);
				auditData.SizeConstraintsLoadAuditInfo_Add(_processRID, aRecsRead, aRecsWithErrors,
					modelsCreated, modelsModified,ModelsRemoved);
				CommitData(auditData);
			}
			catch
			{
				throw;
			}
            finally
            {
                _writingAudit = false;
                CloseUpdateConnection(auditData);
            }
		}
		//Begin TT#707 - JScott - Size Curve process needs to multi-thread
		/// <summary>
		/// Add size curve generate record counts to the audit report.
		/// </summary>
		/// <param name="aRecsRead">The number of header records read.</param>
		/// <param name="aRecsWithErrors">The number of size code records with errors.</param>
		/// <param name="aCodeAdded">The number of size codes added.</param>
		/// <param name="aCodeUpdated">The number of size codes updated.</param>
		public void SizeCurveGenerateAuditInfo_Add(int aExecuted, int aSuccessful, int aFailed, int aNoAction)
		{
            // Begin TT#1243 - JSmith - Audit Performance
            //AuditData auditData = new AuditData();
            // End TT#1243
			try
			{
                _writingAudit = true;
				OpenUpdateConnection(auditData);
				auditData.SizeCurveGenerateAuditInfo_Add(_processRID, aExecuted, aSuccessful, aFailed, aNoAction);
				CommitData(auditData);
			}
			catch
			{
				throw;
			}
            finally
            {
                _writingAudit = false;
                CloseUpdateConnection(auditData);
            }
		}

		//End TT#707 - JScott - Size Curve process needs to multi-thread
		/// <summary>
		/// Add hierarchy record counts to the audit report.
		/// </summary>
		/// <param name="aTotalItems">The number of rollup items processed.</param>
		/// <param name="aBatchSize">The number if items in a rollup batches.</param>
		/// <param name="aConcurrentProcesses">The number of concurrent rollup processes.</param>
		/// <param name="aTotalBatches">The total number of rollup batches processed.</param>
		/// <param name="aRecordsWithErrors">The number of records with errors.</param>
		public void RollupAuditInfo_Add(int aTotalItems, int aBatchSize,
			int aConcurrentProcesses, int aTotalBatches, int aRecordsWithErrors)
		{
            // Begin TT#1243 - JSmith - Audit Performance
            //AuditData auditData = new AuditData();
            // End TT#1243
			try
			{
                _writingAudit = true;
				OpenUpdateConnection(auditData);
				auditData.RollupAuditInfo_Add(_processRID, aTotalItems, aBatchSize, aConcurrentProcesses, aTotalBatches, aRecordsWithErrors);
				CommitData(auditData);
			}
			catch
			{
				throw;
			}
            finally
            {
                _writingAudit = false;
                CloseUpdateConnection(auditData);
            }
		}

		/// <summary>
		/// Add hierarchy record counts to the audit report.
		/// </summary>
		/// <param name="aTotalItems">The number of rollup items processed.</param>
		/// <param name="aConcurrentProcesses">The number of concurrent rollup processes.</param>
		/// <param name="aRecordsWithErrors">The number of records with errors.</param>
		public void ComputationDriverAuditInfo_Add(int aTotalItems,
			int aConcurrentProcesses, int aRecordsWithErrors)
		{
            // Begin TT#1243 - JSmith - Audit Performance
            //AuditData auditData = new AuditData();
            // End TT#1243
			try
			{
                _writingAudit = true;
				OpenUpdateConnection(auditData);
				auditData.ComputationDriverAuditInfo_Add(_processRID, aTotalItems, aConcurrentProcesses, aRecordsWithErrors);
				CommitData(auditData);
			}
			catch
			{
				throw;
			}
            finally
            {
                _writingAudit = false;
                CloseUpdateConnection(auditData);
            }
		}

        // Begin Track #5100 - JSmith - Add counts to audit
        /// <summary>
        /// Add relieve intransit record counts to the audit report.
        /// </summary>
        /// <param name="aRecsRead">The number of records read.</param>
        /// <param name="aRecsAccepted">The number of records accepted.</param>
        /// <param name="aRecordsWithErrors">The number of records with errors.</param>
        public void RelieveIntransitAuditInfo_Add(int aRecsRead,
            int aRecsAccepted, int aRecordsWithErrors)
        {
            // Begin TT#1243 - JSmith - Audit Performance
            //AuditData auditData = new AuditData();
            // End TT#1243
            try
            {
                _writingAudit = true;
                OpenUpdateConnection(auditData);
                auditData.RelieveIntransitAuditInfo_Add(_processRID, aRecsRead, aRecsAccepted, aRecordsWithErrors);
                CommitData(auditData);
            }
            catch
            {
                throw;
            }
            finally
            {
                _writingAudit = false;
                CloseUpdateConnection(auditData);
            }
        }
        // End Track #5100

        // Begin TT#710 - JSmith - Generate relieve intransit
        /// <summary>
        /// Add generate relieve intransit record counts to the audit report.
        /// </summary>
        /// <param name="aHeadersToRelieve">The number of header to be relieved.</param>
        /// <param name="aFilesGenerated">The number of files generated.</param>
        /// <param name="aTotalErrors">The number of errors.</param>
        public void GenerateRelieveIntransitAuditInfo_Add(int aHeadersToRelieve, int aFilesGenerated,
			int aTotalErrors)
        {
            // Begin TT#1243 - JSmith - Audit Performance
            //AuditData auditData = new AuditData();
            // End TT#1243
            try
            {
                _writingAudit = true;
                OpenUpdateConnection(auditData);
                auditData.GenerateRelieveIntransitSummary_Add(_processRID, aHeadersToRelieve, aFilesGenerated, aTotalErrors);
                CommitData(auditData);
            }
            catch
            {
                throw;
            }
            finally
            {
                _writingAudit = false;
                CloseUpdateConnection(auditData);
            }
        }
        // End TT#710

        // Begin TT#988 - JSmith - Add Active Only indicator to Override Low Level Model
        /// <summary>
        /// Add determine node activity record counts to the audit report.
        /// </summary>
        /// <param name="aHeadersToRelieve">The number of header to be relieved.</param>
        /// <param name="aFilesGenerated">The number of files generated.</param>
        /// <param name="aTotalErrors">The number of errors.</param>
        public void DetermineHierarchyActivityAuditInfo_Add(int aTotalNodes, int aActiveNodes,
            int aInactiveNodes, int aTotalErrors)
        {
            AuditData auditData = new AuditData();
            try
            {
                _writingAudit = true;
                OpenUpdateConnection(auditData);
                auditData.DetermineHierarchyActivitySummary_Add(_processRID, aTotalNodes, aActiveNodes,
                        aInactiveNodes, aTotalErrors);
                CommitData(auditData);
            }
            catch
            {
                throw;
            }
            finally
            {
                _writingAudit = false;
                CloseUpdateConnection(auditData);
            }
        }
        // End TT#988

		/// <summary>
		/// Add item to the audit report.
		/// </summary>
		/// <param name="aReclassAction">The type of action processed by the reclass.</param>
		/// <param name="aReclassItemType">The type of data affected by the reclass.</param>
		/// <param name="aReclassItem">The name of the item affected by the reclass.</param>
		/// <param name="aReclassComment">The reclass comment</param>
		public void AddReclassAuditMsg(string aReclassAction, string aReclassItemType, string aReclassItem, string aReclassComment)
		{
            // Begin TT#1243 - JSmith - Audit Performance
            //AuditData auditData = new AuditData();
            // End TT#1243
			try
			{
                _writingAudit = true;
				OpenUpdateConnection(auditData);
				auditData.ReclassAudit_Add(_processRID, aReclassAction, aReclassItemType, aReclassItem, aReclassComment);
				CommitData(auditData);
			}
			catch
			{
				throw;
			}
            finally
            {
                _writingAudit = false;
                CloseUpdateConnection(auditData);
            }
		}

		/// <summary>
		/// Generate an audit report for a given process
		/// </summary>
		/// <param name="messageLevel">The highest level of messages to put to the report.</param>
		/// <param name="processRID">The record id of the process for which the report is to be generated.</param>
		/// <param name="fileName">The directory and file name where the audit report is to be written.</param>
		public void GenerateReport(eMIDMessageLevel messageLevel, int processRID, string fileName)
		{
            // Begin TT#1243 - JSmith - Audit Performance
            //AuditData auditData = new AuditData();
            // End TT#1243
			try
			{
				StreamWriter rptWriter = new StreamWriter(fileName);
				DataTable ah = auditData.ProcessAuditHeader_Read(processRID);
				foreach(DataRow dr in ah.Rows)
				{
					string line = dr["Process RID"].ToString();
					line += "," + dr["Process ID"].ToString();
					line += "," + dr["Process"].ToString();
					line += "," + dr["Status Code"].ToString();
					line += "," + dr["Status"].ToString();
					line += "," + dr["Start Time"].ToString();
					line += "," + dr["Stop Time"].ToString();
					line += "," + dr["User RID"].ToString();
					line += "," + dr["User Name"].ToString();
					line += "," + dr["Highest Message Level"].ToString();
					line += "," + dr["Summary Code"].ToString();
					line += "," + dr["Summary"].ToString();
					line += "," + dr["Description"].ToString();

					rptWriter.WriteLine(line);
				}
				
				DataTable ar = auditData.AuditReport_Read(processRID);
				foreach(DataRow dr in ar.Rows)
				{
					string message = string.Empty;
					int messageCode = -1;
					if (dr["MessageCode"] != System.DBNull.Value)
					{
						messageCode = Convert.ToInt32(dr["MessageCode"],CultureInfo.CurrentUICulture);
						message =  Convert.ToString(dr["MessageCode"],CultureInfo.CurrentUICulture) + ":" + Convert.ToString(dr["Message"],CultureInfo.CurrentUICulture);
					}
					
					string line = dr["ProcessRID"].ToString();
					line += "," + dr["Time"].ToString();
					line += "," + dr["Module"].ToString();
					line += "," + dr["MessageLevel"].ToString();
					line += "," + message;
					if (dr["ReportMessage"] != DBNull.Value)
					{
						line += "," + dr["ReportMessage"].ToString();
					}
					rptWriter.WriteLine(line);
				}
				rptWriter.Flush();
			}
			catch ( InvalidOperationException  err )
			{
				string msg = err.Message;
			}
		}
	}

    // Begin TT#46 MD - JSmith - User Dashboard
    #region LogUserDashboardEvent
    public class LogUserDashboardEvent
    {
        // add event to update menu
        public delegate void LogUserDashboardEventHandler(object source, LogUserDashboardEventArgs e);
        public event LogUserDashboardEventHandler OnLogUserDashboardHandler;

        public void LogActivity(object source, DateTime aTime, string aModule, eMIDTextCode aMessageCode, eMIDMessageLevel aMessageLevel, string aMessageDetails, bool aForce)
        {
            LogUserDashboardEventArgs ea;
            // fire the event if handler is defined
            if (OnLogUserDashboardHandler != null)
            {
                ea = new LogUserDashboardEventArgs(aTime, aModule, aMessageCode, aMessageLevel, aMessageDetails, aForce);
                OnLogUserDashboardHandler(source, ea);
            }
            return;
        }
    }

    public class LogUserDashboardEventArgs : EventArgs
    {
        private string _time;
        private string _module;
        eMIDTextCode _messageCode;
        private string _message = null;
        private string _messageDetails;
        private eMIDMessageLevel _messageLevel;
        private bool _force;

        public LogUserDashboardEventArgs(DateTime aTime, string aModule, eMIDTextCode aMessageCode, eMIDMessageLevel aMessageLevel, string aMessageDetails, bool aForce)
        {
            _messageCode = aMessageCode;
            _messageDetails = aMessageDetails;
            _module = aModule;
            _messageLevel = aMessageLevel;
            _time = aTime.ToString();
            _force = aForce;
        }

        public string Time
        {
            get { return _time; }
            set { _time = value; }
        }

        public string Module
        {
            get { return _module; }
            set { _module = value; }
        }

        public eMIDTextCode MessageCode
        {
            get { return _messageCode; }
            set { _messageCode = value; }
        }

        public string Message
        {
            get { return _message; }
            set { _message = value; }
        }

        public eMIDMessageLevel MessageLevel
        {
            get { return _messageLevel; }
            set { _messageLevel = value; }
        }

        public string MessageDetails
        {
            get { return _messageDetails; }
            set { _messageDetails = value; }
        }

        public bool Force
        {
            get { return _force; }
            set { _force = value; }
        }
    }

    #endregion LogUserDashboardEvent
    // End TT#46 MD - JSmith - User Dashboard
}
