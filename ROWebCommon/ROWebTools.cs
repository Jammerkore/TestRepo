using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using MIDRetail.DataCommon;

using log4net;
using log4net.Config;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]

namespace Logility.ROWebCommon
{
    public class ROWebTools
    {
        //=======
        // FIELDS
        //=======

        private log4net.ILog _log = null;
        private ArrayList _alLogLock;
        private string _sROUserID = null;
        private string _sROSessionID = null;
        private List<string> _userList = new List<string>();

        //=============
        // CONSTRUCTORS
        //=============

        public ROWebTools()
        {
            _alLogLock = new ArrayList { };

            string parmStr = MIDConfigurationManager.AppSettings["LogByUserList"];
            if (parmStr != null)
            {
                string[] entries = parmStr.Split('|');
                foreach (string entry in entries)
                {
                    if (entry.Trim().Length > 0)
                    {
                        _userList.Add(entry.Trim());
                    }
                }
            }
        }

        //===========
        // PROPERTIES
        //===========

        public string ROUserID { get { return _sROUserID; } set { _sROUserID = value; UpdateAppender(); } }

        public string ROSessionID { get { return _sROSessionID; } set { _sROSessionID = value; } }

        public bool LogDebugEnabled { get { return _log.IsDebugEnabled; } }


        //========
        // METHODS
        //========

        public void CreateLog()
        {
            lock (_alLogLock.SyncRoot)
            {
                _log = log4net.LogManager.GetLogger("Activity");
            }
        }

        private void UpdateAppender()
        {
            if (_sROUserID != null)
            {
                bool logByUser = false;
                foreach (string user in _userList)
                {
                    if (user == "*")
                    {
                        logByUser = true;
                        break;
                    }
                    else if (Regex.IsMatch(_sROUserID, user.Replace("*", ".*?")))  // check if matches the user
                    {
                        logByUser = true;
                        break;
                    }
                }
                if (!logByUser)
                {
                    return;
                }

                log4net.Appender.FileAppender appender = (log4net.Appender.FileAppender)LogManager.GetCurrentLoggers()[0].Logger.Repository.GetAppenders()[0];
                string[] fileName = appender.File.Split('.');
                appender.File = fileName[0] + "." + _sROUserID + "." + fileName[1];
                appender.ActivateOptions();
            }
        }

        /// <summary>
        /// Log message to activity file
        /// </summary>
        /// <param name="msgLevel"> The level of the message</param>
        /// <param name="sMessage">The message</param>
        /// <param name="sROUserID">The User if to be included on the message</param>
        /// <param name="sROSessionID">The Session if to be included on the message</param>
        public void LogMessage(eROMessageLevel msgLevel, string sMessage, string sROUserID = null, string sROSessionID = null)
        {
            lock (_alLogLock.SyncRoot)
            {
                if (_log == null)
                {
                    CreateLog();
                }

                sMessage = " Message: " + sMessage;
                if (sROSessionID != null)
                {
                    sMessage = " Session: " + sROSessionID + " " + sMessage;
                }
                else if (_sROSessionID != null)
                {
                    sMessage = " Session: " + _sROSessionID + " " + sMessage;
                }

                if (sROUserID != null)
                {
                    sMessage = " User: " + sROUserID + " " + sMessage;
                }
                else if (_sROUserID != null)
                {
                    sMessage = " User: " + _sROUserID + " " + sMessage;
                }

                switch (msgLevel)
                {
                    case eROMessageLevel.Debug:
                        LogDebugMsg(sMessage);
                        break;
                    case eROMessageLevel.Information:
                        LogInfoMsg(sMessage);
                        break;
                    case eROMessageLevel.Warning:
                        LogWarnMsg(sMessage);
                        break;
                    case eROMessageLevel.Error:
                        LogErrorMsg(sMessage);
                        break;
                    case eROMessageLevel.Severe:
                        LogErrorMsg(sMessage);
                        break;
                    case eROMessageLevel.Fatal:
                        LogFatalMsg(sMessage);
                        break;
                }
            }
        }

        /// <summary>
        /// Log debug level message
        /// </summary>
        /// <param name="sMessage">Message text</param>
        private void LogDebugMsg(string sMessage)
        {

            try
            {
                if (_log.IsDebugEnabled)
                {
                    _log.Debug(sMessage);
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Log information level message
        /// </summary>
        /// <param name="sMessage">Message text</param>
        private void LogInfoMsg(string sMessage)
        {
            try
            {
                if (_log.IsInfoEnabled)
                {
                    _log.Info(sMessage);
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Log warning level message
        /// </summary>
        /// <param name="sMessage">Message text</param>
        private void LogWarnMsg(string sMessage)
        {
            try
            {
                if (_log.IsWarnEnabled)
                {
                    _log.Warn(sMessage);
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Log error level message
        /// </summary>
        /// <param name="sMessage">Message text</param>
        private void LogErrorMsg(string sMessage)
        {
            try
            {
                if (_log.IsErrorEnabled)
                {
                    _log.Error(sMessage);
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Log fatal level message
        /// </summary>
        /// <param name="sMessage">Message text</param>
        private void LogFatalMsg(string sMessage)
        {
            try
            {
                if (_log.IsFatalEnabled)
                {
                    _log.Fatal(sMessage);
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Provides the base message for exceptions.
        /// </summary>
        /// <returns></returns>
        public string GetExceptionReason()
        {
            return "Review log file on " + Environment.MachineName;
        }

        /// <summary>
        /// Provides the code for exceptions
        /// </summary>
        /// <returns></returns>
        public string GetExceptionCode()
        {
            return "Unexpected error";
        }

        /// <summary>
        /// Log message in Windows Event Viewer
        /// </summary>
        /// <param name="sMessage">Message text</param>
        /// <param name="entryType">The log entry type of the message</param>
        public void WriteWindowsEventViewerEntry(string sMessage, EventLogEntryType entryType = EventLogEntryType.Information)
        {
            if (!EventLog.SourceExists("MIDJobService"))
            {
                EventLog.CreateEventSource("MIDJobService", null);
            }
            EventLog.WriteEntry("MIDJobService", sMessage, entryType);
        }
    }

    public enum eROMessageLevel
    {
        Debug,
        Information,
        Warning,
        Severe,
        Error,
        Fatal
    }
}
