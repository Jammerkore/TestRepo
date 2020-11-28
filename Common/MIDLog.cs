using System;
using System.Collections;
using System.Diagnostics;
using System.Globalization;
using System.Data;
using System.IO;

using MIDRetail.DataCommon;
using MIDRetail.Data;

namespace MIDRetail.Common
{
	/// <summary>
	/// Summary description for MIDLog.
	/// </summary>
	public class MIDLog
	{

		private int _userRid;
		private string _userName;
		private string _fileName;
		private string _filePath;
        private string _logLocation; //TT#753-754 - MD - Log informational message added to audit - RBeck
        
		FileStream _fs;
		StreamWriter _sw;
        //private eForecastMonitorType _monitorType;
		private string _homeDirectory;
		private string _methodName;

		/// <summary>
		/// FileStream
		/// </summary>
		public FileStream FS 
		{
			get { return _fs ; }
		}
		/// <summary>
		/// StreamWriter
		/// </summary>
		public StreamWriter SW 
		{
			get { return _sw ; }
		}
    //TT#753-754 - MD - Log informational message added to audit - RBeck
        public string LogLocation
        {
            get { return _logLocation; }
        }
        public string UserName
        {
            get { return _userName; }
        }
    //TT#753-754 - MD - Log informational message added to audit - RBeck
        //TT#339 - MD - Modify Forecast audit message - RBeck
        //public MIDLog(string filePrefix, string filePath, int userRid, string methodName, int methodRid)
        public MIDLog(string filePrefix, string filePath, int userRid, string methodName, string qualifiedNodeID )
		{
			_userRid = userRid;
			_methodName = methodName;
			_filePath = filePath;
			// Begin Track #5929 - JSmith - File in use error
			bool needToFileOpen;
			int attemptCount;
			// End Track #5929

			try
			{
				// Begin TT#503 - protect the Monitor logs from special characters
				filePrefix = MIDMath.ValidAndReplaceFileName(filePrefix);
				methodName = MIDMath.ValidAndReplaceFileName(methodName);
				// Begin TT#503 - protect the Monitor logs from special characters

				if (!EventLog.SourceExists("MIDForecasting"))
				{
					EventLog.CreateEventSource("MIDForecasting", null);
				}

				if (filePath != null)
				{
					_homeDirectory = filePath;
					if(!Directory.Exists(_homeDirectory))
						Directory.CreateDirectory(_homeDirectory);
				}
				else
					_homeDirectory = Directory.GetCurrentDirectory();


                //Begin TT#827-MD -jsobek -Allocation Reviews Performance
                //SecurityAdmin secAdmin = new SecurityAdmin();
                //_userName = secAdmin.GetUserName(userRid);
                _userName = UserNameStorage.GetUserName(userRid);
                //End TT#827-MD -jsobek -Allocation Reviews Performance

				int end = _userName.LastIndexOf("\\");
				_userName = _userName.Substring(++end);

				// Begin Track #5929 - JSmith - File in use error
//				_fileName = _homeDirectory + "\\" + filePrefix + methodRid.ToString(CultureInfo.CurrentUICulture) + 
//					"_" + _userName + ".txt";
//
//			
//				_fs = new FileStream(_fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
//				_sw = new StreamWriter(_fs);
//				_sw.Close();
//
//				_fs = new FileStream(_fileName, FileMode.Truncate);
//				_sw = new StreamWriter(_fs);

                qualifiedNodeID = qualifiedNodeID.Replace("\\", "_");   //TT#339 - MD - Modify Forecast audit message - RBeck

				needToFileOpen = true;
				attemptCount = 0;
				while (needToFileOpen)
				{
					try
					{
						++attemptCount;
						if (attemptCount == 1)
						{
                            //TT#339 - MD - Modify Forecast audit message - RBeck
                            //_fileName = _homeDirectory + "\\" + filePrefix + methodRid.ToString(CultureInfo.CurrentUICulture) +
                            _fileName = _homeDirectory + "\\" + filePrefix + "_" + methodName + "_" + qualifiedNodeID +
								"_" + _userName + ".txt";
						}
						else
						{
                            //TT#339 - MD - Modify Forecast audit message - RBeck
                            //_fileName = _homeDirectory + "\\" + filePrefix + methodRid.ToString(CultureInfo.CurrentUICulture) +
                            _fileName = _homeDirectory + "\\" + filePrefix + "_" + methodName + "_" + qualifiedNodeID + 
								"_" + attemptCount.ToString() + "_" + _userName + attemptCount.ToString() + ".txt";
						}

			
						_fs = new FileStream(_fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
						_sw = new StreamWriter(_fs);
						_sw.Close();

						_fs = new FileStream(_fileName, FileMode.Truncate);

                        _logLocation = _fs.Name;  //TT#753-754 - MD - Log informational message added to audit - RBeck

						_sw = new StreamWriter(_fs);
						needToFileOpen = false;
					}
					catch
					{
						if (attemptCount == 10)
						{
							needToFileOpen = false;
							throw;
						}
					}
				}
				// End Track #5929
                
				_sw.WriteLine("Name: " + _methodName + " Started: " + DateTime.Now.ToString(CultureInfo.CurrentUICulture));
				_sw.WriteLine("User: " + _userName + "(" + _userRid.ToString(CultureInfo.CurrentUICulture) + ")");
			}
			catch (Exception ex)
			{
				// BEGIN Track 5873 stodd
				//EventLog.WriteEntry("MIDForecasting", "LOG FOLDER: " + _homeDirectory, EventLogEntryType.Warning);
				//EventLog.WriteEntry("MIDForecasting", ex.ToString(), EventLogEntryType.Warning);
				throw new FormatInvalidException("Problem Creating Log File. PATH: " + _homeDirectory
					+ ". MSG: " + ex.ToString());
				// End Track 5873 stodd
			}
		}

		public void ReopenLogFile()
		{	
			_fs = new FileStream(_fileName, FileMode.Append);
			_sw = new StreamWriter(_fs);
		}

		public void CloseLogFile()
		{
			if (_sw != null)
			{
				_sw.Flush();
				_sw.Close();
			}
		}

		/// <summary>
		/// Writes message immediately to log file.
		/// Note: you need to manually Reopen the log file to use this method
		/// </summary>
		/// <param name="message"></param>
		public void WriteLine(string message)
		{
			SW.WriteLine(message);
		}

		public void Write(string message)
		{
			SW.Write(message);
		}
	}
}
