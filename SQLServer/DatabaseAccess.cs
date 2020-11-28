using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Globalization;

using MIDRetail.DataCommon;

 

namespace MIDRetail.Data
{
	//Begin TT#483 - JScott - Add Size Lost Sales criteria and processing
	internal enum SPReturnType
	{
		DataTable,
		DataSet
	}

	//End TT#483 - JScott - Add Size Lost Sales criteria and processing
	public delegate object DatabaseMethodDelegate(object[] args);
	/// <summary>
	/// Defines an update connection for a SQL Server database
	/// </summary>
	public class DBUpdateConnection : MIDConnectionString
	{
		private SqlTransaction _sqlTrans = null;
		private SqlCommand _sqlCommand = null;
        private DatabaseExceptionHandler _databaseExceptionHandler = new DatabaseExceptionHandler();  // TT#3056 - JSmith - Header VIRTUAL_LOCK issue
        private string _DBConnectionString = null;  // TT#2131-MD - JSmith - Halo Integration
//		private static string _sConnection  = "";
//		private string _lastCommand  = string.Empty;

//		private enum eDatabaseError
//		{
//			Timeout							= -2,
//			ForeignKeyViolation				= 547,
//			DeadLock						= 1205,
//			Blocking						= 1222,
//			UniqueIndexConstriantViolation	= 2601,
//			UniqueIndexConstriantViolation2	= 2627,
//			InvalidDatabase					= 4060,
//			LoginFailed						= 18456,
//		}

		/// <summary>
		/// Creates a new instance of DBUpdateConnection
		/// </summary>
		public DBUpdateConnection()
		{
			try
			{
// (CSMITH) - BEG MID Track #3369: DB Connection String hardcoded in config files
//				Initialize(MIDConfigurationManager.AppSettings["ConnectionString"]);
//				if (_sConnection == null || _sConnection.Trim().Length ==0)
//				{
//					MIDConnectionString MIDConnectionString = new MIDConnectionString();
//					_sConnection = MIDConnectionString.ReadConnectionString();
//				}
				Initialize(ConnectionString);
// (CSMITH) - END MID Track #3369
			}
			catch (Exception error)
			{
				string message = error.ToString();
				throw;
			}
		}

		/// <summary>
		/// Creates a new instance of DBUpdateConnection with the given ConnectionString
		/// </summary>
		/// <param name="aConnectionString">
		/// The ConnectionString to use to connect
		/// </param>
		public DBUpdateConnection(string aConnectionString)
		{
			try
			{
				Initialize(aConnectionString);
			}
			catch (Exception error)
			{
				string message = error.ToString();
				throw;
			}
		}
		
		/// <summary>
		/// Initialize the new instance
		/// </summary>
		/// <param name="aConnectionString">
		/// The ConnectionString to use to connect
		/// </param>
		public void Initialize(string aConnectionString)
		{
			string lastCommand = string.Empty;
            _DBConnectionString = aConnectionString;  // TT#2131-MD - JSmith - Halo Integration
			if (_DBConnectionString == null)  // TT#2131-MD - JSmith - Halo Integration
			{
				throw new Exception(Include.ErrorBadConfigFile + Include.GetConfigFilename());
			}
			try
			{
				_sqlCommand = new SqlCommand();
				lastCommand = "CreateConnection with " + _DBConnectionString;  // TT#2131-MD - JSmith - Halo Integration
				_sqlCommand.Connection = CreateConnection(_DBConnectionString);  // TT#2131-MD - JSmith - Halo Integration
				OpenConnection(_sqlCommand);

				lastCommand = "CreateConnection with " + _DBConnectionString;  // TT#2131-MD - JSmith - Halo Integration
				_sqlTrans = _sqlCommand.Connection.BeginTransaction();
				_sqlCommand.Transaction = _sqlTrans;
			}
			catch ( SqlException sql_error )
			{
                // Begin TT#3056 - JSmith - Header VIRTUAL_LOCK issue
                //HandleDatabaseException(sql_error, lastCommand);
                _databaseExceptionHandler.HandleDatabaseException(sql_error, lastCommand, _sqlCommand);
                // End TT#3056 - JSmith - Header VIRTUAL_LOCK issue
			}
			catch ( Exception error )
			{
				string message = error.ToString();
				throw;
			}
		}

//		/// <summary>
//		/// Gets the connection string used to connect to the database.
//		/// </summary>
//		public string ConnectionString
//		{
//			get
//			{
//				return _sConnection;
//			}
//		}

		/// <summary>
		/// Gets or sets the command to issue to the database.
		/// </summary>
		public SqlCommand SQLCommand 
		{
			get { return _sqlCommand ; }
			set { _sqlCommand = value; }
		}

		/// <summary>
		/// Gets or sets the transaction for the database.
		/// </summary>
		public SqlTransaction SQLTrans 
		{
			get { return _sqlTrans ; }
			set { _sqlTrans = value; }
		}

		private void OpenConnection(SqlCommand aSqlCommand)
		{
			string lastCommand = string.Empty;
			try
			{
				lastCommand = "DBUpdateConnection Open connection " + aSqlCommand;
				aSqlCommand.Connection.Open();

                // Begin Track #6395 - JSmith - Backposting rollup fails with database timeout messages
                aSqlCommand.Parameters.Clear();
                aSqlCommand.CommandType = CommandType.Text;
                aSqlCommand.CommandText = "set arithabort on";
                aSqlCommand.ExecuteNonQuery();
                // End Track #6395
			}
			catch ( SqlException sql_error )
			{
                // Begin TT#3056 - JSmith - Header VIRTUAL_LOCK issue
                //HandleDatabaseException(sql_error, lastCommand);
                _databaseExceptionHandler.HandleDatabaseException(sql_error, lastCommand, aSqlCommand);
                // End TT#3056 - JSmith - Header VIRTUAL_LOCK issue
			}
			catch 
			{
				throw;
			}
		}

		private SqlConnection CreateConnection(string aConnectionString)
		{
			string lastCommand = string.Empty;
			try
			{
				lastCommand = "DBUpdateConnection CreateConnection with " + _DBConnectionString;  // TT#2131-MD - JSmith - Halo Integration
				return new SqlConnection(aConnectionString );
			}
			catch ( SqlException sql_error )
			{
                // Begin TT#3056 - JSmith - Header VIRTUAL_LOCK issue
                //HandleDatabaseException(sql_error, lastCommand);
                _databaseExceptionHandler.HandleDatabaseException(sql_error, lastCommand, null);
                // End TT#3056 - JSmith - Header VIRTUAL_LOCK issue
			}
			catch 
			{
				throw;
			}
			throw new Exception("Application error in database access");  //  Exception to appease compiler;  code should never get here	
		}

		/// <summary>
		/// Commits the active data to the database.
		/// </summary>
		public void Commit()
		{
			string lastCommand = string.Empty;
			try
			{
				lastCommand = "DBUpdateConnection Commit";
				_sqlTrans.Commit();
			}
			catch ( SqlException sql_error )
			{
                // Begin TT#3056 - JSmith - Header VIRTUAL_LOCK issue
                //HandleDatabaseException(sql_error, lastCommand);
                // Begin TT#3302 - JSmith - Size Curves Failures
                //_databaseExceptionHandler.HandleDatabaseException(sql_error, lastCommand, null);
                _databaseExceptionHandler.HandleDatabaseException(sql_error, lastCommand, this.SQLCommand);
                // End TT#3302 - JSmith - Size Curves Failures
                // End TT#3056 - JSmith - Header VIRTUAL_LOCK issue
			}
			catch ( Exception error )
			{
				string message = error.ToString();
				throw;
			}
		}

		/// <summary>
		/// Issues a rollback of the active data in the database.
		/// </summary>
		public void RollBack()
		{
			try
			{
				if (_sqlTrans != null &&
					_sqlTrans.Connection.State == ConnectionState.Open)
				{
					_sqlTrans.Rollback();
				}
			}
			catch ( SqlException sql_error )
			{
                // Begin TT#3056 - JSmith - Header VIRTUAL_LOCK issue
                //HandleDatabaseException(sql_error, "DBUpdateConnection Rollback");
                // Begin TT#3302 - JSmith - Size Curves Failures
                //_databaseExceptionHandler.HandleDatabaseException(sql_error, "DBUpdateConnection Rollback", null);
                _databaseExceptionHandler.HandleDatabaseException(sql_error, "DBUpdateConnection Rollback", this.SQLCommand);
                // End TT#3302 - JSmith - Size Curves Failures
                // End TT#3056 - JSmith - Header VIRTUAL_LOCK issue
			}
			catch ( Exception error )
			{
				string message = error.ToString();
				throw;
			}
		}

		/// <summary>
		/// Closes the open connection to the database.
		/// </summary>
		public void Close()
		{
			try
			{
//Begin Track #3997 - JSmith - Operation exception closing connection
				if (_sqlCommand != null && _sqlCommand.Connection != null && _sqlCommand.Connection.State == ConnectionState.Open)
				{
					// swallow error if close fails because connection is already closed
					try
					{
                        // Begin Track #6395 - JSmith - Backposting rollup fails with database timeout messages
                        _sqlCommand.Parameters.Clear();
                        _sqlCommand.CommandType = CommandType.Text;
                        _sqlCommand.CommandText = "set arithabort off";
                        _sqlCommand.ExecuteNonQuery();
                        // End Track #6395
//End Track #3997
						_sqlCommand.Connection.Close();
						_sqlCommand.Connection.Dispose();
//Begin Track #3997 - JSmith - Operation exception closing connection
					}
					catch
					{
					}
				}
//End Track #3997
			}
			catch ( SqlException sql_error )
			{
                // Begin TT#3056 - JSmith - Header VIRTUAL_LOCK issue
                //HandleDatabaseException(sql_error, "DBUpdateConnection Close connection");
                _databaseExceptionHandler.HandleDatabaseException(sql_error, "DBUpdateConnection Close connection", _sqlCommand);
                // End TT#3056 - JSmith - Header VIRTUAL_LOCK issue
			}
			catch ( Exception error )
			{
				string message = error.ToString();
				throw;
			}
		}

        // Begin TT#3056 - JSmith - Header VIRTUAL_LOCK issue
//        internal void HandleDatabaseException(SqlException aSqlException, string aMIDCommand)
//        {
//            try
//            {
//                string sErrorMessage = "";				
//                for ( int i = 0; i < aSqlException.Errors.Count; i++ )
//                    sErrorMessage += aSqlException.Errors[i].Number.ToString() 
//                        + ":" + aSqlException.Errors[i].Message + "\n";

////				EventLog.WriteEntry("MIDRetail", "Database error; ConnectionString=" + ConnectionString + ";Command=" + _lastCommand + ";Error=" + sErrorMessage, EventLogEntryType.Error);
				
//                Exception err = null;
//                // uses SQLServer 2000 ErrorCodes 
//                switch (aSqlException.Number) 
//                { 
//                    case (int)eDatabaseError.Timeout: 
//                        // temporarily remove inner exception due to incompability issue between server 2003 and xp
////						err = new DatabaseRetryableViolation(Include.ErrorDatabase + sErrorMessage, aSqlException); 
//                        err = new DatabaseRetryableViolation(Include.ErrorDatabase + "Command=" + aMIDCommand + ";Error=" + sErrorMessage);  
//                        break;
//                    case (int)eDatabaseError.Blocking: 
//                        // temporarily remove inner exception due to incompability issue between server 2003 and xp
//                        err = new DatabaseRetryableViolation(Include.ErrorDatabase + "Command=" + aMIDCommand + ";Error=" + sErrorMessage);  
////						err = new DatabaseRetryableViolation(Include.ErrorDatabase + sErrorMessage, aSqlException); 
//                        break;
//                    case (int)eDatabaseError.ScanErrorWithNolock: 
//                        // temporarily remove inner exception due to incompability issue between server 2003 and xp
//                        err = new DatabaseRetryableViolation(Include.ErrorDatabase + "Command=" + aMIDCommand + ";Error=" + sErrorMessage);  
////						err = new DatabaseRetryableViolation(Include.ErrorDatabase + sErrorMessage, aSqlException); 
//                        break;
//                    case (int)eDatabaseError.ForeignKeyViolation: 
//                        EventLog.WriteEntry("MIDRetail", "ForeignKeyViolation Database error; ConnectionString=" + ConnectionString + ";Command=" + aMIDCommand + ";Error=" + sErrorMessage, EventLogEntryType.Error);
//                        // temporarily remove inner exception due to incompability issue between server 2003 and xp
//                        err = new DatabaseForeignKeyViolation(Include.ErrorDatabase + sErrorMessage); 
////						err = new DatabaseForeignKeyViolation(Include.ErrorDatabase + sErrorMessage, aSqlException); 
//                        break;
//                    case (int)eDatabaseError.DeadLock: 
//                        // temporarily remove inner exception due to incompability issue between server 2003 and xp
//                        err = new DatabaseRetryableViolation(Include.ErrorDatabase + "Command=" + aMIDCommand + ";Error=" + sErrorMessage);  
////						err = new DatabaseRetryableViolation(Include.ErrorDatabase + sErrorMessage, aSqlException); 
//                        break;
//                    case (int)eDatabaseError.DeadLock2: 
//                        // temporarily remove inner exception due to incompability issue between server 2003 and xp
//                        err = new DatabaseRetryableViolation(Include.ErrorDatabase + "Command=" + aMIDCommand + ";Error=" + sErrorMessage);  
////						err = new DatabaseRetryableViolation(Include.ErrorDatabase + sErrorMessage, aSqlException); 
//                        break;
//                    case (int)eDatabaseError.UniqueIndexConstriantViolation: 
//                        EventLog.WriteEntry("MIDRetail", "UniqueIndexConstriantViolation Database error; ConnectionString=" + ConnectionString + ";Command=" + aMIDCommand + ";Error=" + sErrorMessage, EventLogEntryType.Error);
//                        // temporarily remove inner exception due to incompability issue between server 2003 and xp
//                        err = new DatabaseUniqueIndexConstriantViolation(Include.ErrorDatabase + sErrorMessage); 
////						err = new DatabaseUniqueIndexConstriantViolation(Include.ErrorDatabase + sErrorMessage, aSqlException); 
//                        break; 
//                    case (int)eDatabaseError.UniqueIndexConstriantViolation2: 
//                        EventLog.WriteEntry("MIDRetail", "UniqueIndexConstriantViolation2 Database error; ConnectionString=" + ConnectionString + ";Command=" + aMIDCommand + ";Error=" + sErrorMessage, EventLogEntryType.Error);
//                        // temporarily remove inner exception due to incompability issue between server 2003 and xp
//                        err = new DatabaseUniqueIndexConstriantViolation2(Include.ErrorDatabase + sErrorMessage); 
////						err = new DatabaseUniqueIndexConstriantViolation2(Include.ErrorDatabase + sErrorMessage, aSqlException); 
//                        break; 
//                    case (int)eDatabaseError.InvalidDatabase: 
//                        EventLog.WriteEntry("MIDRetail", "InvalidDatabase Database error; ConnectionString=" + ConnectionString + ";Command=" + aMIDCommand + ";Error=" + sErrorMessage, EventLogEntryType.Error);
//                        // temporarily remove inner exception due to incompability issue between server 2003 and xp
//                        err = new MIDDatabaseUnavailableException(Include.ErrorDatabase + sErrorMessage); 
////						err = new MIDDatabaseUnavailableException(Include.ErrorDatabase + sErrorMessage, aSqlException); 
//                        break; 
//                    case (int)eDatabaseError.LoginFailed: 
//                        EventLog.WriteEntry("MIDRetail", "LoginFailed Database error; ConnectionString=" + ConnectionString + ";Command=" + aMIDCommand + ";Error=" + sErrorMessage, EventLogEntryType.Error);
//                        // temporarily remove inner exception due to incompability issue between server 2003 and xp
//                        err = new DatabaseLoginFailed(Include.ErrorDatabase + sErrorMessage); 
////						err = new DatabaseLoginFailed(Include.ErrorDatabase + sErrorMessage, aSqlException); 
//                        break; 
//                    case (int)eDatabaseError.GeneralNetworkError: 
//                        // temporarily remove inner exception due to incompability issue between server 2003 and xp
//                        err = new DatabaseRetryableViolation(Include.ErrorDatabase + "Command=" + aMIDCommand + ";Error=" + sErrorMessage);  
//                        //						err = new DatabaseRetryableViolation(Include.ErrorDatabase + sErrorMessage, aSqlException); 
//                        break;
//                    case (int)eDatabaseError.NotInCatalog: 
//                        // temporarily remove inner exception due to incompability issue between server 2003 and xp
//                        err = new DatabaseNotInCatalog(Include.ErrorDatabase + sErrorMessage);
////						err = new DatabaseNotInCatalog(Include.ErrorDatabase + sErrorMessage, aSqlException); 
//                        break;
//                    // Begin Track #6304 - JSmith - query processor could not start the necessary thread resources for parallel query 
//                    case (int)eDatabaseError.ParallelQueryThreadError: 
//                        // temporarily remove inner exception due to incompability issue between server 2003 and xp
//                        err = new ParallelQueryThreadError(Include.ErrorDatabase + sErrorMessage);
//                        break;
//                    // End Track #6304
//                    default: 
//                        EventLog.WriteEntry("MIDRetail", "Database error=" + aSqlException.Number.ToString() + "; ConnectionString=" + ConnectionString + ";Command=" + aMIDCommand + ";Error=" + sErrorMessage, EventLogEntryType.Error);
//                        // temporarily remove inner exception due to incompability issue between server 2003 and xp
//                        err = new Exception(Include.ErrorDatabase + sErrorMessage);
////						err = new Exception(Include.ErrorDatabase + sErrorMessage, aSqlException);
//                        break; 
//                } 

//                throw err;
//            }
//            catch ( Exception error )
//            {
//                string message = error.ToString();
//                throw;
//            }
//        }
        // End TT#3056 - JSmith - Header VIRTUAL_LOCK issue
	}

	/// <summary>
	/// SQL Server database specific code.
	/// </summary>
	public class DatabaseAccess : MIDConnectionString
	{
		private SqlCommand _readCommand;
		//private SqlConnection _readConnection;   // used by ReadOnlyStoredProcedure
		//private SqlDataReader _forwardReader;    // used by ReadOnlyStoredProcedure

		protected DBUpdateConnection _updateConnection = null;
		//		// Used with Store Characteristic Methods
		//		SqlDataAdapter _adStoreCharGroup = null;
		//		SqlDataAdapter _adStoreChar = null;
		//		DataSet _dsStoreChar = null;
//		public static string _sConnection  = "";
		private int _commandTimeout = 30;
		private bool _retryDatabaseCommand = true;
		private int _maximumRetryAttempts = 4;
		private int _retrySleepTime = 2000;
        private bool _allowRetryOnUpdateCommand = true; // TT#1185 Verify Enq before Update
		private ArrayList _updateCommands = new ArrayList();
		public DatabaseMethodDelegate _myDatabaseMethodDelegate;
//		private string _lastCommand  = string.Empty;
        private DatabaseExceptionHandler _databaseExceptionHandler = new DatabaseExceptionHandler();  // TT#3056 - JSmith - Header VIRTUAL_LOCK issue

        private string _altConnectionString = null;  // TT#2131-MD - JSmith - Halo Integration

		/// <summary>
		/// Creates an instance of DatabaseAccess
		/// </summary>
		public DatabaseAccess()
		{
			try
			{
// (CSMITH) - BEG MID Track #3369: DB Connection String hardcoded in config files
//				Initialize(MIDConfigurationManager.AppSettings["ConnectionString"]);
//				MIDConnectionString MIDConnectionString = new MIDConnectionString();
//				Initialize(MIDConnectionString.ReadConnectionString());
//				if (_sConnection == null || _sConnection.Trim().Length ==0)
//				{
//					MIDConnectionString MIDConnectionString = new MIDConnectionString();
//					_sConnection = MIDConnectionString.ReadConnectionString();
//				}
				Initialize(ConnectionString);
// (CSMITH) - END MID Track #3369
			}
			catch (Exception error)
			{
				string message = error.ToString();
				throw;
			}
		}

		/// <summary>
		/// Creates an instance of DatabaseAccess with the given ConnectionString
		/// </summary>
		/// <param name="aConnectionString">
		/// The ConnectionString to use to connect
		/// </param>
		public DatabaseAccess(string aConnectionString)
		{
			try
			{
                _altConnectionString = aConnectionString;  // TT#2131-MD - JSmith - Halo Integration
				Initialize(aConnectionString);
			}
			catch (Exception error)
			{
				string message = error.ToString();
				throw;
			}
		}

		/// <summary>
		/// Initialize the new instance
		/// </summary>
		/// <param name="aConnectionString">
		/// The ConnectionString to use to connect
		/// </param>
		private void Initialize(string aConnectionString)
		{
			try
			{
				object[] args = new object[]{aConnectionString};
				_myDatabaseMethodDelegate = new DatabaseMethodDelegate(myInitialize);
				ProcessReadCommand(new Command(_myDatabaseMethodDelegate, args, "DatabaseAccess Initialize " + aConnectionString));
			}
			catch
			{
				throw;
			}
		}

        // Begin TT#1243 - JSmith - Audit Performance
        public ConnectionState UpdateConnectionState 
		{
			get 
            {
                if (_updateConnection == null ||
                    _updateConnection.SQLCommand == null ||
                    _updateConnection.SQLCommand.Connection == null ||
                    _updateConnection.SQLTrans == null ||
                    _updateConnection.SQLTrans.Connection == null ||
                    _updateConnection.SQLCommand.Connection.State != ConnectionState.Open ||
                    _updateConnection.SQLTrans.Connection.State != ConnectionState.Open)
                {
                    return ConnectionState.Closed;
                }
                else
                {
                    return ConnectionState.Open;
                }
            }
		}
        // End TT#1243

        // Begin TT#2131-MD - JSmith - Halo Integration
        public string DBConnectionString
        {
            get
            {
                if (_altConnectionString != null)
                {
                    return _altConnectionString;
                }
                else
                {
                    return ConnectionString;
                }
            }
        }
        // End TT#2131-MD - JSmith - Halo Integration

		/// <summary>
		/// Initialize the new instance
		/// </summary>
		/// <param name="aArgs">
		/// The ConnectionString to use to connect
		/// </param>
		private object myInitialize(object[] aArgs)
		{
			try
			{
				string aConnectionString = Convert.ToString(aArgs[0], CultureInfo.CurrentCulture);
                // Begin TT#2131-MD - JSmith - Halo Integration
                //ConnectionString = aConnectionString;
                if (aConnectionString != ConnectionString)
                {
                    _altConnectionString = aConnectionString;
                }
                // End TT#2131-MD - JSmith - Halo Integration
				if (!EventLog.SourceExists("MIDRetail"))
				{
                    EventLog.CreateEventSource("MIDRetail", null);
				}
				if (ConnectionString == null)
				{
					EventLog.WriteEntry("MIDRetail", Include.ErrorBadConfigFile + Include.GetConfigFilename(), EventLogEntryType.Error);
					throw new Exception(Include.ErrorBadConfigFile + Include.GetConfigFilename());
				}
				string sCommandTimeout =  MIDConfigurationManager.AppSettings["DatabaseCommandTimeOut"];
				if (sCommandTimeout != null)
				{
					try
					{
						_commandTimeout = Convert.ToInt32(sCommandTimeout, CultureInfo.CurrentUICulture);
					}
					catch
					{
						EventLog.WriteEntry("MIDRetail", "Invalid value in DatabaseCommandTimeOut - defaulted to 30 seconds", EventLogEntryType.Error);
					}
				}
				string sParm =  MIDConfigurationManager.AppSettings["DatabaseRetryCount"];
				if (sParm != null)
				{
					_retryDatabaseCommand = true;
					try
					{
						_maximumRetryAttempts = Convert.ToInt32(sParm, CultureInfo.CurrentUICulture);
						if (_maximumRetryAttempts == 0)
						{
							_retryDatabaseCommand = false;
						}
					}
					catch
					{
						EventLog.WriteEntry("MIDRetail", "Invalid value in DatabaseRetryCount - defaulted to 4", EventLogEntryType.Error);
					}
				}
				
				sParm =  MIDConfigurationManager.AppSettings["DatabaseRetryInterval"];
				if (sParm != null)
				{
					try
					{
						_retrySleepTime = Convert.ToInt32(sParm, CultureInfo.CurrentUICulture);
					}
					catch
					{
						EventLog.WriteEntry("MIDRetail", "Invalid value in DatabaseRetryInterval - defaulted to 2000 (2 seconds)", EventLogEntryType.Error);
					}
				}
				return null;
			}
			catch
			{
				throw;
			}
		}

//		/// <summary>
//		/// Gets the connection string used to connect to the database.
//		/// </summary>
//		public string ConnectionString
//		{
//			get
//			{
//				return _sConnection;
//			}
//		}
        // begin TT#1185 - Verify ENQ before Update
        public bool UpdateConnectionOpen
        {
            get
            {
                if (_updateConnection != null)
                {
                    if (_updateConnection.SQLCommand.Connection.State == ConnectionState.Open)
                    {
                        return true;
                    }
                }
                return false;
            }
        }
		/// <summary>
		/// Open an update connection to the database.
		/// </summary>
		public void OpenUpdateConnection()
		{
            // begin TT#1185 - Verify ENQ on Update
            OpenUpdateConnection(true);
        }
        public void OpenUpdateConnection(bool aAllowRetryOnUpdate)
        {
            _allowRetryOnUpdateCommand = aAllowRetryOnUpdate;
            // end TT#1185 - Verify ENQ on Update
			try
			{
				_myDatabaseMethodDelegate = new DatabaseMethodDelegate(myOpenUpdateConnection);
				Command command = new Command(_myDatabaseMethodDelegate, null, "DatabaseAccess Open connection");
				_updateCommands.Clear();
				_updateCommands.Add(command);
				ProcessUpdateCommand(command);
			}
			catch
			{
				throw;
			}
		}
		/// <summary>
		/// Open an update connection to the database.
		/// </summary>
		private object myOpenUpdateConnection(object[] aArgs)
		{
			try
			{
				_updateConnection = new DBUpdateConnection(DBConnectionString);  // TT#2131-MD - JSmith - Halo Integration
				if (_commandTimeout != _updateConnection.SQLCommand.CommandTimeout)
				{
					_updateConnection.SQLCommand.CommandTimeout = _commandTimeout;
				}
                // Begin TT#3731 - JSmith - Error when Saving
                if (!UpdateConnectionOpen)
                {
                    throw new DatabaseRetryableViolation(Include.ErrorDatabase + "Command=Update Connection Open" + ";Error=Connection not available", Include.ErrorDatabase + "Command=Update Connection Open" + ";Error=Connection not available");
                }
                // End TT#3731 - JSmith - Error when Saving
			}
			catch ( SqlException sql_error )
			{
                // Begin TT#3056 - JSmith - Header VIRTUAL_LOCK issue
                CloseDatabaseUpdateConnection();
                //HandleDatabaseUpdateException(sql_error, "DatabaseAccess Open update connection");
                _databaseExceptionHandler.HandleDatabaseException(sql_error, "DatabaseAccess Open update connection", _updateConnection.SQLCommand);
                // End TT#3056 - JSmith - Header VIRTUAL_LOCK issue
			}
			catch 
			{
				throw;
			}
			return null;
		}

		/// <summary>
		/// Write active data be commited to the database.
		/// </summary>
		public void CommitData()
		{
			try
			{
				_myDatabaseMethodDelegate = new DatabaseMethodDelegate(myCommitData);
				Command command = new Command(_myDatabaseMethodDelegate, null, "DatabaseAccess Commit");
				_updateCommands.Add(command);
				ProcessUpdateCommand(command);
				_updateCommands.Clear();
			}
			catch
			{
				throw;
			}
		}
		/// <summary>
		/// Write active data be commited to the database.
		/// </summary>
		private object myCommitData(object[] aArgs)
		{
			string lastCommand = string.Empty;
			try
			{
				lastCommand = "DatabaseAccess Commit";
				_updateConnection.Commit();
//				_updateCommands.Clear();
				lastCommand = "DatabaseAccess CloseUpdateConnection";
//				CloseUpdateConnection();
				myCloseUpdateConnection(null);
				lastCommand = "DatabaseAccess OpenUpdateConnection";
//				OpenUpdateConnection();
				myOpenUpdateConnection(null);
			}
			catch ( SqlException sql_error )
			{
                // Begin TT#3056 - JSmith - Header VIRTUAL_LOCK issue
                CloseDatabaseUpdateConnection();
                //HandleDatabaseUpdateException(sql_error, lastCommand);
                _databaseExceptionHandler.HandleDatabaseException(sql_error, lastCommand, _updateConnection.SQLCommand);
                // End TT#3056 - JSmith - Header VIRTUAL_LOCK issue
			}
			catch 
			{
				throw;
			}
			return null;
		}

		/// <summary>
		/// Close the update connection.
		/// </summary>
		public void CloseUpdateConnection()
		{
			try
			{
				_myDatabaseMethodDelegate = new DatabaseMethodDelegate(myCloseUpdateConnection);
				Command command = new Command(_myDatabaseMethodDelegate, null, "DatabaseAccess Close connection");
				_updateCommands.Add(command);
				ProcessUpdateCommand(command);
				_updateCommands.Clear();
			}
			catch
			{
				throw;
			}
		}
		/// <summary>
		/// Close the update connection.
		/// </summary>
		private object myCloseUpdateConnection(object[] aArgs)
		{
			try
			{
				if (_updateConnection != null)
				{
                    // begin TT#1185 - Verify ENQ before Update
                    //CloseReader();
                    // end TT#1185 - Verify ENQ before Update
					if (_updateConnection.SQLCommand.Connection.State == ConnectionState.Open)
					{
						_updateConnection.Close();
					}
					_updateConnection = null;
				}
			}
			catch ( SqlException sql_error )
			{
                // Begin TT#3056 - JSmith - Header VIRTUAL_LOCK issue
                CloseDatabaseUpdateConnection();
                //HandleDatabaseUpdateException(sql_error, "DatabaseAccess Close connection");
                _databaseExceptionHandler.HandleDatabaseException(sql_error, "DatabaseAccess Close connection", _updateConnection.SQLCommand);
                // End TT#3056 - JSmith - Header VIRTUAL_LOCK issue
			}
			catch 
			{
				throw;
			}
			return null;
		}

		private void OpenConnection(SqlCommand aSqlCommand)
		{
			try
			{
				if (aSqlCommand.Connection.State != ConnectionState.Open)
				{
					aSqlCommand.Connection.Open();

                    // Begin Track #5968 - JSmith - ForeignKeyViolation Database Error
                    if (aSqlCommand.Connection.State != ConnectionState.Open)
                    {
                        throw new DatabaseRetryableViolation(Include.ErrorDatabase + "Command=Connection Open" + ";Error=Connection not available", Include.ErrorDatabase + "Command=Connection Open" + ";Error=Connection not available");
                    }
                    // Begin Track #5968

                    // Begin Track #6395 - JSmith - Backposting rollup fails with database timeout messages
                    string sveCommandText = aSqlCommand.CommandText;
                    aSqlCommand.Parameters.Clear();
                    aSqlCommand.CommandType = CommandType.Text;
                    aSqlCommand.CommandText = "set arithabort on";
                    aSqlCommand.ExecuteNonQuery();
                    aSqlCommand.CommandText = sveCommandText;
                    // End Track #6395
				}
			}
			catch ( SqlException sql_error )
			{
                // Begin TT#3056 - JSmith - Header VIRTUAL_LOCK issue
                //HandleDatabaseReadException(sql_error, "DatabaseAccess Open connection", aSqlCommand);
                _databaseExceptionHandler.HandleDatabaseException(sql_error, "DatabaseAccess Open connection", aSqlCommand);
                // End TT#3056 - JSmith - Header VIRTUAL_LOCK issue
			}
			catch 
			{
				throw;
			}
		}

		private void OpenConnection(SqlConnection aSqlConnection)
		{
            SqlCommand command = null;
			try
			{
				if (aSqlConnection.State != ConnectionState.Open)
				{
					aSqlConnection.Open();

                    // Begin Track #5968 - JSmith - ForeignKeyViolation Database Error
                    if (aSqlConnection.State != ConnectionState.Open)
                    {
                        throw new DatabaseRetryableViolation(Include.ErrorDatabase + "Command=Connection Open" + ";Error=Connection not available", Include.ErrorDatabase + "Command=Connection Open" + ";Error=Connection not available");
                    }
                    // Begin Track #5968

                    // Begin Track #6395 - JSmith - Backposting rollup fails with database timeout messages
                    //SqlCommand command = aSqlConnection.CreateCommand();
                    command = aSqlConnection.CreateCommand();
                    command.Parameters.Clear();
                    command.CommandType = CommandType.Text;
                    command.CommandText = "set arithabort on";
                    command.ExecuteNonQuery();
                    // End Track #6395
				}
			}
			catch ( SqlException sql_error )
			{
                // Begin TT#3056 - JSmith - Header VIRTUAL_LOCK issue
                //HandleDatabaseReadException(sql_error, "DatabaseAccess Open connection", command);
                _databaseExceptionHandler.HandleDatabaseException(sql_error, "DatabaseAccess Open connection", command);
                // End TT#3056 - JSmith - Header VIRTUAL_LOCK issue
			}
			catch 
			{
				throw;
			}
		}

		private SqlConnection CreateConnection(string aConnectionString)
		{
			string lastCommand = string.Empty;
            SqlCommand command = null;
			try
			{
				lastCommand = "CreateConnection with " + DBConnectionString;  // TT#2131-MD - JSmith - Halo Integration
				SqlConnection sc = new SqlConnection(aConnectionString );
				sc.Open();

                // Begin Track #5968 - JSmith - ForeignKeyViolation Database Error
                if (sc.State != ConnectionState.Open)
                {
                    throw new DatabaseRetryableViolation(Include.ErrorDatabase + "Command=Connection Open" + ";Error=Connection not available", Include.ErrorDatabase + "Command=Connection Open" + ";Error=Connection not available");
                }
                // Begin Track #5968

                // Begin Track #6395 - JSmith - Backposting rollup fails with database timeout messages
                //SqlCommand command = sc.CreateCommand();
                command = sc.CreateCommand();
                command.Parameters.Clear();
                command.CommandType = CommandType.Text;
                command.CommandText = "set arithabort on";
                command.ExecuteNonQuery();
                // End Track #6395

				return sc;
			}
			catch ( SqlException sql_error )
			{
                // Begin TT#3056 - JSmith - Header VIRTUAL_LOCK issue
                //HandleDatabaseReadException(sql_error, lastCommand, command);
                _databaseExceptionHandler.HandleDatabaseException(sql_error, lastCommand, command);
                // End TT#3056 - JSmith - Header VIRTUAL_LOCK issue
			}
			catch 
			{
				throw;
			}
			throw new Exception("Application error in database access");  //  Exception to appease compiler;  code should never get here	
		}

        //// begin TT#1185 - Verify ENQ before Update
        ///// <summary>
        ///// Opens a result reader associated with the "last open" SQL Command
        ///// </summary>
        ///// <param name="aForUpdateResults">True: Gets a reader to get results from an update command; False: Gets a reader to get results from a read command (command session must still be open)</param>
        ///// <returns>True: Reader opened successfully; False: Reader could not be opened</returns>
        //public bool OpenReader(bool aForUpdateResults)
        //{
        //    if (_forwardReader != null)
        //    {
        //        return false;
        //    }
        //    if (aForUpdateResults)
        //    {
        //        if (_updateConnection == null)
        //        {
        //            return false;
        //        }
        //        if (_updateConnection.SQLCommand == null)
        //        {
        //            return false;
        //        }
        //        _forwardReader = _updateConnection.SQLCommand.ExecuteReader(CommandBehavior.SingleResult);
        //    }
        //    else
        //    {
        //        if (_readCommand == null)
        //        {
        //            return false;
        //        }
        //        _forwardReader = _readCommand.ExecuteReader(CommandBehavior.SingleResult);
        //    }
        //    return true;
        //}
        ///// <summary>
        ///// Closes any reader associated with the "last open" SQL command
        ///// </summary>
        //public void CloseReader()
        //{
        //    if (_forwardReader != null)
        //    {
        //        if (!_forwardReader.IsClosed)
        //        {
        //            _forwardReader.Close();
        //        }
        //        _forwardReader.Dispose();
        //    }
        //    _forwardReader = null;
        //}
        //// end TT#1185 - Verify ENQ before Update

		/// <summary>
		/// Rolls back the data for the update transaction.
		/// </summary>
		public void RollBack()
		{
			try
			{
				// begin MID Track 4110 Get Null Reference on Cancel Intransit
				//if (_updateConnection != null && 
				//	_updateConnection.SQLTrans.Connection.State == ConnectionState.Open)
				if (_updateConnection != null
					&& _updateConnection.SQLTrans != null
					&& _updateConnection.SQLTrans.Connection != null
					&& _updateConnection.SQLTrans.Connection.State == ConnectionState.Open)
					// end MID Track 4110 Get Null Reference on Cancel Intransit
				{
					_updateConnection.RollBack();
				}
			}
			catch ( SqlException sql_error )
			{
                // Begin TT#3056 - JSmith - Header VIRTUAL_LOCK issue
                CloseDatabaseUpdateConnection();
                //HandleDatabaseUpdateException(sql_error, "DatabaseAccess Rollback");
                _databaseExceptionHandler.HandleDatabaseException(sql_error, "DatabaseAccess Rollback", _updateConnection.SQLCommand);
                // End TT#3056 - JSmith - Header VIRTUAL_LOCK issue
			}
			catch 
			{
				throw;
			}
		}

		/// <summary>
		/// Retrieves the name of the database where the application is connected.
		/// </summary>
		/// <returns></returns>
		public string GetDBName()
		{
			try
			{
				_myDatabaseMethodDelegate = new DatabaseMethodDelegate(myGetDBName);
				object returnObject = ProcessReadCommand(new Command(_myDatabaseMethodDelegate, null, "DatabaseAccess GetDBName"));
				return Convert.ToString(returnObject, CultureInfo.CurrentCulture);
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Retrieves the name of the database where the application is connected.
		/// </summary>
		/// <returns></returns>
		private object myGetDBName(object[] aArgs)
		{
			string lastCommand = string.Empty;
			SqlCommand cd = null;
			try
			{
				string serverName = null;
				string databaseName = null;
				SqlDataReader myReader;
				cd = new SqlCommand("select @@SERVERNAME as ServerName");
	
				lastCommand = "CreateConnection with " + DBConnectionString;  // TT#2131-MD - JSmith - Halo Integration
				cd.Connection = CreateConnection(DBConnectionString);  // TT#2131-MD - JSmith - Halo Integration
				OpenConnection(cd);

				lastCommand = "DatabaseAccess GetDBName";
				cd.CommandType = CommandType.Text;
								
				myReader = cd.ExecuteReader();
				if (myReader.Read())
				{
					serverName = (string) myReader["ServerName"];
				}
				myReader.Close();
				cd.Connection.Close();
				
				// need new connection for next command
//				cd.Connection.Open();
				OpenConnection(cd);
				cd.CommandText = "select DB_Name() as DBName";
								
				myReader = cd.ExecuteReader();
				if (myReader.Read())
				{
					databaseName = (string) myReader["DBName"];
				}
				myReader.Close();
				return serverName + ":" + databaseName;
			}
			catch ( SqlException sql_error )
			{
                // Begin TT#3056 - JSmith - Header VIRTUAL_LOCK issue
                //HandleDatabaseReadException(sql_error, lastCommand, cd);
                _databaseExceptionHandler.HandleDatabaseException(sql_error, lastCommand, cd);
                // End TT#3056 - JSmith - Header VIRTUAL_LOCK issue
			}
			catch ( Exception error )
			{
				string message = error.ToString();
                throw;
			}
			finally
			{
				if (cd != null && cd.Connection != null && cd.Connection.State == ConnectionState.Open)
				{
					// swallow error if close fails because connection is already closed
					try
					{
						cd.Connection.Close();
					}
					catch
					{
					}
                    cd.Dispose();
				}
			}
			throw new Exception("Application error in database access");  //  Exception to appease compiler;  code should never get here	
		}

        ///// <summary>
        ///// Execute the provided command to count the records identified in the command.
        ///// </summary>
        ///// <param name="SQLCommand"></param>
        ///// <returns></returns>
        //public int ExecuteRecordCount( string SQLCommand )
        //{
        //    try
        //    {
        //        object[] args = new object[]{SQLCommand};
        //        _myDatabaseMethodDelegate = new DatabaseMethodDelegate(myExecuteRecordCount);
        //        object returnObject = ProcessReadCommand(new Command(_myDatabaseMethodDelegate, args, SQLCommand));
        //        return Convert.ToInt32(returnObject, CultureInfo.CurrentCulture);
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}
        ///// <summary>
        ///// Execute the provided command to count the records identified in the command.
        ///// </summary>
        ///// <param name="SQLCommand"></param>
        ///// <returns></returns>
        //private object myExecuteRecordCount( object[] aArgs )
        //{
        //    string lastCommand = string.Empty;
        //    try
        //    {
        //        string SQLCommand = Convert.ToString(aArgs[0], CultureInfo.CurrentCulture);
        //        SqlCommand cd = null;
        //        try
        //        {
        //            int recCount = 0;
        //            SqlDataReader myReader;
        //            cd = new SqlCommand(SQLCommand);
        //            SQLMonitorList.SQLMonitorEntry se = null;
        //            SQLMonitorList.BeforeExecution(ref se, SQLCommand);

        //            lastCommand = "CreateConnection with " + ConnectionString;
        //            cd.Connection = CreateConnection(ConnectionString );
        //            OpenConnection(cd);

        //            lastCommand = SQLCommand;
        //            cd.CommandType = CommandType.Text;
        //            if (_commandTimeout != cd.CommandTimeout)
        //            {
        //                cd.CommandTimeout = _commandTimeout;
        //            }
                   			
        //            myReader = cd.ExecuteReader();
        //            if (myReader.Read())
        //            {
        //                recCount = (int) myReader["MyCount"];
        //            }
        //            myReader.Close();
        //            SQLMonitorList.AfterExecution(se);
        //            return recCount;
        //        }
        //        catch ( SqlException sql_error )
        //        {
        //            // Begin TT#3056 - JSmith - Header VIRTUAL_LOCK issue
        //            //HandleDatabaseReadException(sql_error, lastCommand, cd);
        //            _databaseExceptionHandler.HandleDatabaseException(sql_error, lastCommand, cd);
        //            // End TT#3056 - JSmith - Header VIRTUAL_LOCK issue
        //        }
        //        catch ( Exception error )
        //        {
        //            string message = error.ToString();
        //            throw;
        //        }
        //        finally
        //        {
        //            if (cd != null && cd.Connection != null && cd.Connection.State == ConnectionState.Open)
        //            {
        //                // swallow error if close fails because connection is already closed
        //                try
        //                {
        //                    cd.Connection.Close();
        //                }
        //                catch
        //                {
        //                }
        //                cd.Dispose();
        //            }
        //        }
        //        throw new Exception("Application error in database access");  //  Exception to appease compiler;  code should never get here	
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}

        ///// <summary>
        ///// Execute the provided command to count the records identified in the command.
        ///// </summary>
        ///// <param name="SQLCommand"></param>
        ///// <returns></returns>
        //public int ExecuteRecordCount( string SQLCommand, MIDDbParameter[] InputParameters )
        //{
        //    try
        //    {
        //        object[] args = new object[]{SQLCommand, InputParameters};
        //        _myDatabaseMethodDelegate = new DatabaseMethodDelegate(myExecuteRecordCount2);
        //        object returnObject = ProcessReadCommand(new Command(_myDatabaseMethodDelegate, args, SQLCommand));
        //        return Convert.ToInt32(returnObject, CultureInfo.CurrentCulture);
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}
        ///// <summary>
        ///// Execute the provided command to count the records identified in the command.
        ///// </summary>
        ///// <param name="aArgs"></param>
        ///// <returns></returns>
        //private object myExecuteRecordCount2( object[] aArgs )
        //{
        //    string lastCommand = string.Empty;
        //    try
        //    {
        //        string SQLCommand = Convert.ToString(aArgs[0], CultureInfo.CurrentCulture);
        //        MIDDbParameter[] InputParameters = (MIDDbParameter[])aArgs[1];
        //        SqlCommand cd = null;
        //        try
        //        {
        //            int recCount = 0;
        //            SqlDataReader myReader;
        //            cd = new SqlCommand(SQLCommand);
                    
        //            cd.Parameters.Clear();

        //            AddInputParametersToSQLCommand(InputParameters, ref cd);

        //            SQLMonitorList.SQLMonitorEntry se = null;
        //            SQLMonitorList.BeforeExecution(ref se, SQLCommand, InputParameters);

        //            lastCommand = "CreateConnection with " + ConnectionString;
        //            cd.Connection = CreateConnection(ConnectionString );
        //            OpenConnection(cd);
					
        //            lastCommand = SQLCommand;
        //            cd.CommandType = CommandType.Text;
        //            if (_commandTimeout != cd.CommandTimeout)
        //            {
        //                cd.CommandTimeout = _commandTimeout;
        //            }
                  		
        //            myReader = cd.ExecuteReader();
        //            if (myReader.Read())
        //            {
        //                recCount = (int) myReader["MyCount"];
        //            }
        //            myReader.Close();
        //            SQLMonitorList.AfterExecution(se);
        //            return recCount;
        //        }
        //        catch ( SqlException sql_error )
        //        {
        //            // Begin TT#3056 - JSmith - Header VIRTUAL_LOCK issue
        //            //HandleDatabaseReadException(sql_error, lastCommand, cd);
        //            _databaseExceptionHandler.HandleDatabaseException(sql_error, lastCommand, cd);
        //            // End TT#3056 - JSmith - Header VIRTUAL_LOCK issue
        //        }
        //        catch ( Exception error )
        //        {
        //            string message = error.ToString();
        //            throw;
        //        }
        //        finally
        //        {
        //            if (cd != null && cd.Connection != null && cd.Connection.State == ConnectionState.Open)
        //            {
        //                // swallow error if close fails because connection is already closed
        //                try
        //                {
        //                    cd.Connection.Close();
        //                }
        //                catch
        //                {
        //                }
        //                cd.Dispose();
        //            }
        //        }
        //        throw new Exception("Application error in database access");  //  Exception to appease compiler;  code should never get here	
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}

		
        ///// <summary>
        ///// Retrieves the application text identified in the command.
        ///// </summary>
        ///// <param name="SQLCommand"></param>
        ///// <returns></returns>
        //public string ExecuteGetText( string SQLCommand )
        //{
        //    try
        //    {
        //        object[] args = new object[]{SQLCommand};
        //        _myDatabaseMethodDelegate = new DatabaseMethodDelegate(myExecuteGetText);
        //        object returnObject = ProcessReadCommand(new Command(_myDatabaseMethodDelegate, args, SQLCommand));
        //        return Convert.ToString(returnObject, CultureInfo.CurrentCulture);
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}
        ///// <summary>
        ///// Retrieves the application text identified in the command.
        ///// </summary>
        ///// <param name="SQLCommand"></param>
        ///// <returns></returns>
        //private object myExecuteGetText( object[] aArgs )
        //{
        //    string lastCommand = string.Empty;
        //    try
        //    {
        //        string SQLCommand = Convert.ToString(aArgs[0], CultureInfo.CurrentCulture);
        //        SqlCommand cd = null;
        //        try
        //        {
        //            string text = null;
        //            SqlDataReader myReader;
        //            cd = new SqlCommand(SQLCommand);

        //            SQLMonitorList.SQLMonitorEntry se = null;
        //            if (SQLMonitorList.includeApplicationText)
        //            {
        //                SQLMonitorList.BeforeExecution(ref se, SQLCommand);
        //            }


	
        //            lastCommand = "CreateConnection with " + ConnectionString;
        //            cd.Connection = CreateConnection(ConnectionString );
        //            OpenConnection(cd);
					
					
        //            lastCommand = SQLCommand;
        //            cd.CommandType = CommandType.Text;
        //            if (_commandTimeout != cd.CommandTimeout)
        //            {
        //                cd.CommandTimeout = _commandTimeout;
        //            }

                
  
        //            myReader = cd.ExecuteReader();
        //            if (myReader.Read())
        //            {
        //                text = (string) myReader["TEXT_VALUE"];
        //            }
        //            myReader.Close();

        //            if (SQLMonitorList.includeApplicationText)
        //            {
        //                SQLMonitorList.AfterExecution(se);
        //            }

        //            return text;
        //        }
        //        catch ( SqlException sql_error )
        //        {
        //            // Begin TT#3056 - JSmith - Header VIRTUAL_LOCK issue
        //            //HandleDatabaseReadException(sql_error, lastCommand, cd);
        //            _databaseExceptionHandler.HandleDatabaseException(sql_error, lastCommand, cd);
        //            // End TT#3056 - JSmith - Header VIRTUAL_LOCK issue
        //        }
        //        catch ( Exception error )
        //        {
        //            string message = error.ToString();
        //            throw;
        //        }
        //        finally
        //        {
        //            if (cd != null && cd.Connection != null && cd.Connection.State == ConnectionState.Open)
        //            {
        //                // swallow error if close fails because connection is already closed
        //                try
        //                {
        //                    cd.Connection.Close();
        //                }
        //                catch
        //                {
        //                }
        //                cd.Dispose();
        //            }
        //        }
        //        throw new Exception("Application error in database access");  //  Exception to appease compiler;  code should never get here	
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}


        ///// <summary>
        ///// Executes a database maximum value request for the provided command.
        ///// </summary>
        ///// <param name="SQLCommand"></param>
        ///// <returns></returns>
        //public int ExecuteMaxValue( string SQLCommand )
        //{
        //    try
        //    {
        //        object[] args = new object[]{SQLCommand};
        //        _myDatabaseMethodDelegate = new DatabaseMethodDelegate(myExecuteMaxValue);
        //        object returnObject = ProcessReadCommand(new Command(_myDatabaseMethodDelegate, args, SQLCommand));
        //        return Convert.ToInt32(returnObject, CultureInfo.CurrentCulture);
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}
        ///// <summary>
        ///// Executes a database maximum value request for the provided command.
        ///// </summary>
        ///// <param name="aArgs"></param>
        ///// <returns></returns>
        //private object myExecuteMaxValue( object[] aArgs )
        //{
        //    string lastCommand = string.Empty;
        //    try
        //    {
        //        string SQLCommand = Convert.ToString(aArgs[0], CultureInfo.CurrentCulture);
        //        SqlCommand cd = null;
        //        try
        //        {
        //            int maxValue = 0;
        //            SqlDataReader myReader;
        //            cd = new SqlCommand(SQLCommand);
        //            SQLMonitorList.SQLMonitorEntry se = null;
        //            SQLMonitorList.BeforeExecution(ref se, SQLCommand);
                  
	
        //            lastCommand = "CreateConnection with " + ConnectionString;
        //            cd.Connection = CreateConnection(ConnectionString );
        //            OpenConnection(cd);

        //            lastCommand = SQLCommand;
        //            cd.CommandType = CommandType.Text;
        //            if (_commandTimeout != cd.CommandTimeout)
        //            {
        //                cd.CommandTimeout = _commandTimeout;
        //            }
                    
        //            myReader = cd.ExecuteReader();
        //            if (myReader.Read())
        //            {
        //                // Begin TT#3065 -JSmith - Purge process failed with timeout error
        //                //maxValue = (int) myReader["MyValue"];
        //                maxValue = myReader["MyValue"] == DBNull.Value ? 0 : (int)myReader["MyValue"];
        //                // End TT#3065 -JSmith - Purge process failed with timeout error
        //            }
        //            myReader.Close();
        //            SQLMonitorList.AfterExecution(se);
                  
        //            return maxValue;
        //        }
        //        catch ( SqlException sql_error )
        //        {
        //            // Begin TT#3056 - JSmith - Header VIRTUAL_LOCK issue
        //            //HandleDatabaseReadException(sql_error, lastCommand, cd);
        //            _databaseExceptionHandler.HandleDatabaseException(sql_error, lastCommand, cd);
        //            // End TT#3056 - JSmith - Header VIRTUAL_LOCK issue
        //        }
        //        catch ( Exception error )
        //        {
        //            string message = error.ToString();
        //            throw;
        //        }
        //        finally
        //        {
        //            if (cd != null && cd.Connection != null && cd.Connection.State == ConnectionState.Open)
        //            {
        //                // swallow error if close fails because connection is already closed
        //                try
        //                {
        //                    cd.Connection.Close();
        //                }
        //                catch
        //                {
        //                }
        //                cd.Dispose();
        //            }
        //        }
        //        throw new Exception("Application error in database access");  //  Exception to appease compiler;  code should never get here	
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}


        ///// <summary>
        ///// ExecuteScalar is a public method in class DatabaseAccess used to return a single value
        ///// </summary>
        ///// <remarks>
        ///// Rather than cache this single-row and column resultset in a DataSet object or open a SqlDataReader, use the ExecuteScalar method to return the value as an object. You can then convert it to the appropriate type.
        ///// </remarks>
        ///// <param name='name'>A string which contains a SQL statment that returns one value (one column of one row)</param>
        ///// <returns>Return value of type object.</returns>
        //public object ExecuteScalar( string SqlStatement )
        //{
        //    try
        //    {
        //        object[] args = new object[]{SqlStatement};
        //        _myDatabaseMethodDelegate = new DatabaseMethodDelegate(myExecuteScalar);
        //        object returnObject = ProcessReadCommand(new Command(_myDatabaseMethodDelegate, args, SqlStatement));
        //        return returnObject;
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}

        ///// <summary>
        ///// ExecuteScalar is a public method in class DatabaseAccess used to return a single value
        ///// </summary>
        ///// <remarks>
        ///// Rather than cache this single-row and column resultset in a DataSet object or open a SqlDataReader, use the ExecuteScalar method to return the value as an object. You can then convert it to the appropriate type.
        ///// </remarks>
        ///// <param name='name'>A string which contains a SQL statment that returns one value (one column of one row)</param>
        ///// <returns>Return value of type object.</returns>
        //private object myExecuteScalar(object[] aArgs)
        //{
        //    string lastCommand = string.Empty;
        //    SqlCommand cd = null;
        //    try
        //    {
        //        string SqlStatement = Convert.ToString(aArgs[0], CultureInfo.CurrentCulture);
        //        //SqlCommand cd = new SqlCommand();
        //        cd = new SqlCommand();

        //        try
        //        {
        //            SQLMonitorList.SQLMonitorEntry se = null;
        //            SQLMonitorList.BeforeExecution(ref se, SqlStatement);
        //            lastCommand = "CreateConnection with " + ConnectionString;
        //            cd.Connection = CreateConnection(ConnectionString);
        //            OpenConnection(cd);

        //            lastCommand = SqlStatement;
        //            cd.CommandType = CommandType.Text;
        //            cd.CommandText = SqlStatement;
        //            if (_commandTimeout != cd.CommandTimeout)
        //            {
        //                cd.CommandTimeout = _commandTimeout;
        //            }
                    
        //            object result = cd.ExecuteScalar();
        //            SQLMonitorList.AfterExecution(se);
        //            return result;
        //        }
        //        catch (SqlException sql_error)
        //        {
        //            // Begin TT#3056 - JSmith - Header VIRTUAL_LOCK issue
        //            //HandleDatabaseReadException(sql_error, lastCommand, cd);
        //            _databaseExceptionHandler.HandleDatabaseException(sql_error, lastCommand, cd);
        //            // End TT#3056 - JSmith - Header VIRTUAL_LOCK issue
        //        }
        //        catch (Exception error)
        //        {
        //            string message = error.ToString();
        //            throw;
        //        }
        //        finally
        //        {
        //            if (cd != null && cd.Connection != null && cd.Connection.State == ConnectionState.Open)
        //            {
        //                // swallow error if close fails because connection is already closed
        //                try
        //                {
        //                    cd.Connection.Close();
        //                }
        //                catch
        //                {
        //                }
        //                cd.Dispose();
        //            }
        //        }
        //        throw new Exception("Application error in database access");  //  Exception to appease compiler;  code should never get here	
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}

        ////Begin TT#1663 - DOConnell - Change Pack Name with quote issue 
        ///// <summary>
        ///// ExecuteScalar is a public method in class DatabaseAccess used to return a single value
        ///// </summary>
        ///// <remarks>
        ///// Rather than cache this single-row and column resultset in a DataSet object or open a SqlDataReader, use the ExecuteScalar method to return the value as an object. You can then convert it to the appropriate type.
        ///// </remarks>
        ///// <param name='name'>A string which contains a SQL statment that returns one value (one column of one row)</param>
        ///// <returns>Return value of type object.</returns>
        //public object ExecuteScalar(string SqlStatement, MIDDbParameter[] InputParameters )
        //{
        //    try
        //    {
        //        object[] args = new object[] { SqlStatement, InputParameters };
        //        _myDatabaseMethodDelegate = new DatabaseMethodDelegate(myExecuteScalar2);
        //        object returnObject = ProcessReadCommand(new Command(_myDatabaseMethodDelegate, args, SqlStatement));
        //        return returnObject;
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}

        ///// <summary>
        ///// ExecuteScalar is a public method in class DatabaseAccess used to return a single value
        ///// </summary>
        ///// <remarks>
        ///// Rather than cache this single-row and column resultset in a DataSet object or open a SqlDataReader, use the ExecuteScalar method to return the value as an object. You can then convert it to the appropriate type.
        ///// </remarks>
        ///// <param name='name'>A string which contains a SQL statment that returns one value (one column of one row)</param>
        ///// <returns>Return value of type object.</returns>
        //private object myExecuteScalar2( object[] aArgs )
        //{
        //    string lastCommand = string.Empty;
        //    SqlCommand cd = null;
        //    try
        //    {
        //        string SqlStatement = Convert.ToString(aArgs[0], CultureInfo.CurrentCulture);
        //        MIDDbParameter[] InputParameters = (MIDDbParameter[])aArgs[1];
        //        //SqlCommand cd = new SqlCommand();
        //        cd = new SqlCommand();
			
        //        try
        //        {
        //            cd.Parameters.Clear();

        //            AddInputParametersToSQLCommand(InputParameters, ref cd);

        //            lastCommand = "CreateConnection with " + ConnectionString;
        //            cd.Connection = CreateConnection(ConnectionString);


        //            SQLMonitorList.SQLMonitorEntry se = null;
        //            SQLMonitorList.BeforeExecution(ref se, SqlStatement, InputParameters);
        //            OpenConnection(cd);

        //            lastCommand = SqlStatement;
        //            cd.CommandType = CommandType.Text;
        //            cd.CommandText = SqlStatement;
        //            if (_commandTimeout != cd.CommandTimeout)
        //            {
        //                cd.CommandTimeout = _commandTimeout;
        //            }
        //            object result = cd.ExecuteScalar();
        //            SQLMonitorList.AfterExecution(se);
        //            return result;
        //        }
        //        catch ( SqlException sql_error )
        //        {
        //            // Begin TT#3056 - JSmith - Header VIRTUAL_LOCK issue
        //            //HandleDatabaseReadException(sql_error, lastCommand, cd);
        //            _databaseExceptionHandler.HandleDatabaseException(sql_error, lastCommand, cd);
        //            // End TT#3056 - JSmith - Header VIRTUAL_LOCK issue
        //        }
        //        catch ( Exception error )
        //        {
        //            string message = error.ToString();
        //            throw;
        //        }			
        //        finally
        //        {
        //            if (cd != null && cd.Connection != null && cd.Connection.State == ConnectionState.Open)
        //            {
        //                // swallow error if close fails because connection is already closed
        //                try
        //                {
        //                    cd.Connection.Close();
        //                }
        //                catch
        //                {
        //                }
        //                cd.Dispose();
        //            }
        //        }
        //        throw new Exception("Application error in database access");  //  Exception to appease compiler;  code should never get here	
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}
        ////End TT#1663 - DOConnell - Change Pack Name with quote issue

		/// <summary>
		/// Executes a provided database command.
		/// </summary>
		/// <param name="SQLCommand"></param>
		/// <param name="TableName"></param>
		/// <returns>DataTable</returns>
		public DataTable ExecuteSQLQuery( string SQLCommand, string TableName )
		{
			try
			{
				object[] args = new object[]{SQLCommand, TableName};
				_myDatabaseMethodDelegate = new DatabaseMethodDelegate(myExecuteSQLQuery);
				object returnObject = ProcessReadCommand(new Command(_myDatabaseMethodDelegate, args, SQLCommand));
				return (DataTable)returnObject;
			}
			catch
			{
				throw;
			}
		}

        public DataSet ExecuteSQLQueryForUpdateWithResults(string SQLStatement, int commandTimeout)
		{
			try
			{
                object[] args = new object[] { SQLStatement, commandTimeout };
                _myDatabaseMethodDelegate = new DatabaseMethodDelegate(myExecuteSQLQueryForUpdateWithResults);
                Command command = new Command(_myDatabaseMethodDelegate, args, SQLStatement);
                _updateCommands.Add(command);
                object returnObject = ProcessUpdateCommand(command);
                return (DataSet)returnObject;
			}
			catch
			{
				throw;
			}
		}

        //Begin TT#1430-MD -jsobek -Null reference after canceling a product search
        public DataTable ExecuteSQLQuery(string SQLCommand, string TableName, int commandTimeout)
        {
            try
            {
                object[] args = new object[] { SQLCommand, TableName, commandTimeout };
                _myDatabaseMethodDelegate = new DatabaseMethodDelegate(myExecuteSQLQueryWithTimeOut);
                object returnObject = ProcessReadCommand(new Command(_myDatabaseMethodDelegate, args, SQLCommand));
                return (DataTable)returnObject;
            }
            catch
            {
                throw;
            }
        }
        //End TT#1430-MD -jsobek -Null reference after canceling a product search


		/// <summary>
		/// Executes a provided database command.
		/// </summary>
		/// <param name="SQLCommand"></param>
		/// <param name="TableName"></param>
		/// <returns>DataTable</returns>
		private object myExecuteSQLQuery( object[] aArgs )
		{
			string lastCommand = string.Empty;
			try
			{
				string SQLCommand = Convert.ToString(aArgs[0], CultureInfo.CurrentCulture);
				string TableName = Convert.ToString(aArgs[1], CultureInfo.CurrentCulture);
				SqlCommand cd = null;
				try
				{
					cd = new SqlCommand(SQLCommand);
                    SQLMonitorList.SQLMonitorEntry se = null;
                    SQLMonitorList.BeforeExecution(ref se, SQLCommand);
					lastCommand = "CreateConnection with " + DBConnectionString;  // TT#2131-MD - JSmith - Halo Integration
					cd.Connection = CreateConnection(DBConnectionString);  // TT#2131-MD - JSmith - Halo Integration
					
					lastCommand = SQLCommand;
					cd.CommandType = CommandType.Text;
					if (_commandTimeout != cd.CommandTimeout)
					{
						cd.CommandTimeout = _commandTimeout;
					}
                    
					DataTable dt = MIDEnvironment.CreateDataTable( TableName );
					SqlDataAdapter sda = new SqlDataAdapter( cd );
					sda.Fill( dt );
                    SQLMonitorList.AfterExecution(se);
					return dt;
				}
				catch ( SqlException sql_error )
				{
                    // Begin TT#3056 - JSmith - Header VIRTUAL_LOCK issue
                    //HandleDatabaseReadException(sql_error, lastCommand, cd);
                    _databaseExceptionHandler.HandleDatabaseException(sql_error, lastCommand, cd);
                    // End TT#3056 - JSmith - Header VIRTUAL_LOCK issue
				}
				catch ( Exception error )
				{
					string message = error.ToString();
					throw;
				}
				finally
				{
					if (cd != null && cd.Connection != null && cd.Connection.State == ConnectionState.Open)
					{
						// swallow error if close fails because connection is already closed
						try
						{
							cd.Connection.Close();
						}
						catch
						{
						}
                        cd.Dispose();
					}
				}
				throw new Exception("Application error in database access");  //  Exception to appease compiler;  code should never get here	
			}
			catch
			{
				throw;
			}
		}


        private object myExecuteSQLQueryForUpdateWithResults(object[] aArgs)
		{
			string lastCommand = string.Empty;
			try
			{
                string SQLStatement = Convert.ToString(aArgs[0], CultureInfo.CurrentCulture);
				//string TableName = Convert.ToString(aArgs[1], CultureInfo.CurrentCulture);
                int commandTimeout = Convert.ToInt32(aArgs[1], CultureInfo.CurrentCulture);
				//SqlCommand cd = null;
				try
				{
					//cd = new SqlCommand(SQLCommand);
                    SQLMonitorList.SQLMonitorEntry se = null;
                    SQLMonitorList.BeforeExecution(ref se, SQLStatement);
					lastCommand = "CreateConnection with " + DBConnectionString;  // TT#2131-MD - JSmith - Halo Integration
					//cd.Connection = CreateConnection(ConnectionString );

                    lastCommand = SQLStatement;
					//cd.CommandType = CommandType.Text;
                    //if (_commandTimeout != cd.CommandTimeout)
                    //{
                    //    cd.CommandTimeout = _commandTimeout;
                    //}
                    //cd.CommandTimeout = timeout;



                    _updateConnection.SQLCommand.Parameters.Clear();
                    _updateConnection.SQLCommand.CommandType = CommandType.Text;
                    _updateConnection.SQLCommand.CommandText = SQLStatement;
                    if (commandTimeout == -1)
                    {
                        if (_commandTimeout != _updateConnection.SQLCommand.CommandTimeout)
                        {
                            _updateConnection.SQLCommand.CommandTimeout = _commandTimeout;
                        }
                    }
                    else
                    {
                        if (_updateConnection.SQLCommand.CommandTimeout != commandTimeout)
                        {
                            _updateConnection.SQLCommand.CommandTimeout = commandTimeout;
                        }
                    }

                    SqlCommand cd = _updateConnection.SQLCommand;



                    

                    DataSet ds = MIDEnvironment.CreateDataSet("resultSet");
                    SqlDataAdapter sda = new SqlDataAdapter(cd);
                    sda.Fill(ds);
                    SQLMonitorList.AfterExecution(se);
                    return ds;



                  
				}
				catch ( SqlException sql_error )
				{
                    // Begin TT#3056 - JSmith - Header VIRTUAL_LOCK issue
                    //HandleDatabaseReadException(sql_error, lastCommand, cd);
                    //_databaseExceptionHandler.HandleDatabaseException(sql_error, lastCommand, cd);
                    CloseDatabaseUpdateConnection();
                    _databaseExceptionHandler.HandleDatabaseException(sql_error, SQLStatement, _updateConnection.SQLCommand);
                    // End TT#3056 - JSmith - Header VIRTUAL_LOCK issue
				}
				catch ( Exception error )
				{
					string message = error.ToString();
					throw;
				}
                //finally
                //{
                //    if (cd != null && cd.Connection != null && cd.Connection.State == ConnectionState.Open)
                //    {
                //        // swallow error if close fails because connection is already closed
                //        try
                //        {
                //            cd.Connection.Close();
                //        }
                //        catch
                //        {
                //        }
                //        cd.Dispose();
                //    }
                //}
				throw new Exception("Application error in database access");  //  Exception to appease compiler;  code should never get here	
			}
			catch
			{
				throw;
			}
		}


        //Begin TT#1430-MD -jsobek -Null reference after canceling a product search
        private object myExecuteSQLQueryWithTimeOut(object[] aArgs)
        {
            string lastCommand = string.Empty;
            try
            {
                string SQLCommand = Convert.ToString(aArgs[0], CultureInfo.CurrentCulture);
                string TableName = Convert.ToString(aArgs[1], CultureInfo.CurrentCulture);
                int timeout = Convert.ToInt32(aArgs[2], CultureInfo.CurrentCulture);
                SqlCommand cd = null;
                try
                {
                    cd = new SqlCommand(SQLCommand);
                    SQLMonitorList.SQLMonitorEntry se = null;
                    SQLMonitorList.BeforeExecution(ref se, SQLCommand);
                    lastCommand = "CreateConnection with " + DBConnectionString;  // TT#2131-MD - JSmith - Halo Integration
                    cd.Connection = CreateConnection(DBConnectionString);  // TT#2131-MD - JSmith - Halo Integration

                    lastCommand = SQLCommand;
                    cd.CommandType = CommandType.Text;
                    //if (_commandTimeout != cd.CommandTimeout)
                    //{
                    //    cd.CommandTimeout = _commandTimeout;
                    //}
                    cd.CommandTimeout = timeout;

                    DataTable dt = MIDEnvironment.CreateDataTable(TableName);
                    SqlDataAdapter sda = new SqlDataAdapter(cd);
                    sda.Fill(dt);
                    SQLMonitorList.AfterExecution(se);
                    return dt;
                }
                catch (SqlException sql_error)
                {
                    // Begin TT#3056 - JSmith - Header VIRTUAL_LOCK issue
                    //HandleDatabaseReadException(sql_error, lastCommand, cd);
                    _databaseExceptionHandler.HandleDatabaseException(sql_error, lastCommand, cd);
                    // End TT#3056 - JSmith - Header VIRTUAL_LOCK issue
                }
                catch (Exception error)
                {
                    string message = error.ToString();
                    throw;
                }
                finally
                {
                    if (cd != null && cd.Connection != null && cd.Connection.State == ConnectionState.Open)
                    {
                        // swallow error if close fails because connection is already closed
                        try
                        {
                            cd.Connection.Close();
                        }
                        catch
                        {
                        }
                        cd.Dispose();
                    }
                }
                throw new Exception("Application error in database access");  //  Exception to appease compiler;  code should never get here	
            }
            catch
            {
                throw;
            }
        }
        //End TT#1430-MD -jsobek -Null reference after canceling a product search
	
        ///// <summary>
        ///// Executes a SQL query statement with input parameters.
        ///// </summary>
        ///// <param name="SQLStatement"></param>
        ///// <param name="TableName"></param>
        ///// <param name="InputParameters"></param>
        ///// <returns>DataTable</returns>
        //public DataTable ExecuteSQLQuery( string SQLStatement, string TableName, MIDDbParameter[] InputParameters )
        //{
        //    try
        //    {
        //        object[] args = new object[]{SQLStatement, TableName, InputParameters};
        //        _myDatabaseMethodDelegate = new DatabaseMethodDelegate(myExecuteSQLQuery2);
        //        object returnObject = ProcessReadCommand(new Command(_myDatabaseMethodDelegate, args, SQLStatement));
        //        return (DataTable)returnObject;
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}
        ///// <summary>
        ///// Executes a SQL query statement with input parameters.
        ///// </summary>
        ///// <param name="SQLStatement"></param>
        ///// <param name="TableName"></param>
        ///// <param name="InputParameters"></param>
        ///// <returns>DataTable</returns>
        //private object myExecuteSQLQuery2( object[] aArgs )
        //{
        //    string lastCommand = string.Empty;
        //    try
        //    {
        //        string SQLStatement = Convert.ToString(aArgs[0], CultureInfo.CurrentCulture);
        //        string TableName = Convert.ToString(aArgs[1], CultureInfo.CurrentCulture);
        //        MIDDbParameter[] InputParameters = (MIDDbParameter[])aArgs[2];
        //        SqlCommand cd = null;
        //        try
        //        {
        //            cd = new SqlCommand();
        //            cd.Parameters.Clear();

        //            AddInputParametersToSQLCommand(InputParameters, ref cd); //TT#827-MD -jsobek -Allocation Reviews Performance
                   
			
        //            lastCommand = "CreateConnection with " + ConnectionString;
        //            cd.Connection = CreateConnection(ConnectionString );
					
        //            lastCommand = SQLStatement;
        //            cd.CommandType = CommandType.Text;
        //            cd.CommandText = SQLStatement;
        //            if (_commandTimeout != cd.CommandTimeout)
        //            {
        //                cd.CommandTimeout = _commandTimeout;
        //            }
        //            SQLMonitorList.SQLMonitorEntry se = null;
        //            SQLMonitorList.BeforeExecution(ref se, SQLStatement, InputParameters);
				
        //            DataTable dt = MIDEnvironment.CreateDataTable( TableName );
        //            SqlDataAdapter sda = new SqlDataAdapter( cd );
        //            sda.Fill( dt );
        //            SQLMonitorList.AfterExecution(se);
        //            return dt;
        //        }
        //        catch ( SqlException sql_error )
        //        {
        //            // Begin TT#3056 - JSmith - Header VIRTUAL_LOCK issue
        //            //HandleDatabaseReadException(sql_error, lastCommand, cd);
        //            _databaseExceptionHandler.HandleDatabaseException(sql_error, lastCommand, cd);
        //            // End TT#3056 - JSmith - Header VIRTUAL_LOCK issue
        //        }
        //        catch ( Exception error )
        //        {
        //            string message = error.ToString();
        //            throw;
        //        }
        //        finally
        //        {
        //            if (cd != null && cd.Connection != null && cd.Connection.State == ConnectionState.Open)
        //            {
        //                // swallow error if close fails because connection is already closed
        //                try
        //                {
        //                    cd.Connection.Close();
        //                }
        //                catch
        //                {
        //                }
        //                cd.Dispose();
        //            }
        //        }
        //        throw new Exception("Application error in database access");  //  Exception to appease compiler;  code should never get here	
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}


        


        ////Begin TT#483 - JScott - Add Size Lost Sales criteria and processing
        ///// <summary>
        ///// Executes a stored procedure with only input parameters.
        ///// </summary>
        ///// <param name="ProcedureName"></param>
        ///// <param name="TableName"></param>
        ///// <param name="InputParameters"></param>
        ///// <returns>DataTable</returns>
        //public DataSet ExecuteDataSetQuery( string ProcedureName, string TableName, MIDDbParameter[] InputParameters)
        //{
        //    try
        //    {
        //        object[] args = new object[] { ProcedureName, TableName, InputParameters, SPReturnType.DataSet };
        //        _myDatabaseMethodDelegate = new DatabaseMethodDelegate(myExecuteQuery);
        //        object returnObject = ProcessReadCommand(new Command(_myDatabaseMethodDelegate, args, ProcedureName));
        //        return (DataSet)returnObject;
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}

		//End TT#483 - JScott - Add Size Lost Sales criteria and processing
		/// <summary>
		/// Executes a stored procedure with only input parameters.
		/// </summary>
		/// <param name="ProcedureName"></param>
		/// <param name="TableName"></param>
		/// <param name="InputParameters"></param>
		/// <returns>DataTable</returns>
		public DataTable ExecuteQuery( string ProcedureName, string TableName, MIDDbParameter[] InputParameters )
		{
			try
			{
				//Begin TT#483 - JScott - Add Size Lost Sales criteria and processing
				//object[] args = new object[] { ProcedureName, TableName, InputParameters };
				object[] args = new object[] { ProcedureName, TableName, InputParameters, SPReturnType.DataTable };
				//End TT#483 - JScott - Add Size Lost Sales criteria and processing
				_myDatabaseMethodDelegate = new DatabaseMethodDelegate(myExecuteQuery);
				object returnObject = ProcessReadCommand(new Command(_myDatabaseMethodDelegate, args, ProcedureName));
				return (DataTable)returnObject;
			}
			catch
			{
				throw;
			}
		}
		/// <summary>
		/// Executes a stored procedure with only input parameters.
		/// </summary>
		/// <param name="aArgs"></param>
		/// <returns>DataTable</returns>
		private object myExecuteQuery( object[] aArgs )
		{
			string lastCommand = string.Empty;
			try
			{
				string ProcedureName = Convert.ToString(aArgs[0], CultureInfo.CurrentCulture);
				string TableName = Convert.ToString(aArgs[1], CultureInfo.CurrentCulture);
				MIDDbParameter[] InputParameters = (MIDDbParameter[])aArgs[2];
				//Begin TT#483 - JScott - Add Size Lost Sales criteria and processing
				SPReturnType returnType = (SPReturnType)aArgs[3];
				//End TT#483 - JScott - Add Size Lost Sales criteria and processing
				SqlCommand cd = null;
				try
				{
					cd = new SqlCommand();

                    AddInputParametersToSQLCommand(InputParameters, ref cd);
			
					lastCommand = "CreateConnection with " + DBConnectionString;  // TT#2131-MD - JSmith - Halo Integration
					cd.Connection = CreateConnection(DBConnectionString);  // TT#2131-MD - JSmith - Halo Integration
					
					lastCommand = ProcedureName;
					cd.CommandType = CommandType.StoredProcedure;
					cd.CommandText = ProcedureName;
					if (_commandTimeout != cd.CommandTimeout)
					{
						cd.CommandTimeout = _commandTimeout;
					}
                    SQLMonitorList.SQLMonitorEntry se = null;
                    SQLMonitorList.BeforeExecution(ref se, ProcedureName, InputParameters);

					//Begin TT#483 - JScott - Add Size Lost Sales criteria and processing
					if (returnType == SPReturnType.DataTable)
					{
						DataTable dt = MIDEnvironment.CreateDataTable(TableName);
						SqlDataAdapter sda = new SqlDataAdapter(cd);
						sda.Fill(dt);
                        SQLMonitorList.AfterExecution(se);
						return dt;
					}
					else
					{
						DataSet ds = MIDEnvironment.CreateDataSet(TableName);
						SqlDataAdapter sda = new SqlDataAdapter(cd);
						sda.Fill(ds);
                        SQLMonitorList.AfterExecution(se);
						return ds;
					}
					//Begin TT#483 - JScott - Add Size Lost Sales criteria and processing
				}
				catch ( SqlException sql_error )
				{
                    // Begin TT#3056 - JSmith - Header VIRTUAL_LOCK issue
                    //HandleDatabaseReadException(sql_error, lastCommand, cd);
                    _databaseExceptionHandler.HandleDatabaseException(sql_error, lastCommand, cd);
                    // End TT#3056 - JSmith - Header VIRTUAL_LOCK issue
				}
				catch ( Exception error )
				{
					string message = error.ToString();
					throw;
				}
				finally
				{
					if (cd != null && cd.Connection != null && cd.Connection.State == ConnectionState.Open)
					{
						// swallow error if close fails because connection is already closed
						try
						{
							cd.Connection.Close();
						}
						catch
						{
						}
                        cd.Dispose();
					}
				}
				throw new Exception("Application error in database access");  //  Exception to appease compiler;  code should never get here	
			}
			catch
			{
				throw;
			}
		}

        //Begin TT#827-MD -jsobek -Allocation Reviews Performance - Unused function
        //public void AddParametersToSQLCommand(ref System.Data.SqlClient.SqlCommand cmd, MIDDbParameter[] InputParameters)
        //{
        //    if (InputParameters != null && InputParameters.Length > 0)
        //    {
        //        for (int i = 0; i < InputParameters.Length; i++)
        //        {
        //            SqlParameter InParam = new SqlParameter();
        //            InParam.Direction = ParameterDirection.Input;
        //            InParam.ParameterName = InputParameters[i].ParameterName;
        //            if (InputParameters[i].Value == null)
        //            {
        //                InParam.Value = DBNull.Value;
        //            }
        //            else
        //                if (InputParameters[i].DbType == eDbType.Char)
        //                {
        //                    InParam.Value = Convert.ToString(InputParameters[i].Value, CultureInfo.CurrentUICulture);
        //                    InParam.Size = 1;
        //                }
        //                else
        //                {
        //                    InParam.Value = InputParameters[i].Value;
        //                }
        //            switch (InputParameters[i].DbType)
        //            {
        //                case eDbType.Bit:
        //                    InParam.SqlDbType = SqlDbType.Bit;
        //                    break;
        //                case eDbType.Char:
        //                    InParam.SqlDbType = SqlDbType.Char;
        //                    break;
        //                case eDbType.DateTime:
        //                    InParam.SqlDbType = SqlDbType.DateTime;
        //                    if (InParam.Value != DBNull.Value)
        //                    {
        //                        if (Convert.ToDateTime(InParam.Value, CultureInfo.CurrentUICulture) == Include.UndefinedDate)
        //                        {
        //                            InParam.Value = DBNull.Value;
        //                        }
        //                    }
        //                    break;
        //                case eDbType.Float:
        //                    InParam.SqlDbType = SqlDbType.Float;
        //                    break;
        //                case eDbType.Int:
        //                    InParam.SqlDbType = SqlDbType.Int;
        //                    break;
        //                case eDbType.SmallDateTime:
        //                    InParam.SqlDbType = SqlDbType.SmallDateTime;
        //                    break;
        //                case eDbType.VarChar:
        //                    InParam.SqlDbType = SqlDbType.VarChar;
        //                    break;
        //                case eDbType.Image:
        //                    InParam.SqlDbType = SqlDbType.Image;
        //                    break;
        //                case eDbType.Text:
        //                    InParam.SqlDbType = SqlDbType.Text;
        //                    break;
        //                // begin TT#173  Provide database container for large data collections
        //                case eDbType.smallint:
        //                    InParam.SqlDbType = SqlDbType.SmallInt;
        //                    break;
        //                case eDbType.tinyint:
        //                    InParam.SqlDbType = SqlDbType.TinyInt;
        //                    break;
        //                case eDbType.VarBinary:
        //                    InParam.SqlDbType = SqlDbType.VarBinary;
        //                    InParam.Size = ((byte[])InputParameters[i].Value).Length;
        //                    break;
        //                // end TT#173  Provide database container for large data collections
        //                // begin TT#1185 - Verify ENQ before Update
        //                case eDbType.Int64:
        //                    InParam.SqlDbType = SqlDbType.BigInt;
        //                    break;
        //                // end TT#1185 - Verify ENQ before Update
        //                default:
        //                    break;
        //            }
        //            cmd.Parameters.Add(InParam);
        //        }
        //    }
        //}
        //End TT#827-MD -jsobek -Allocation Reviews Performance - Unused function

        ///// <summary>
        ///// Executes a stored procedure with no parameters.
        ///// </summary>
        ///// <param name="ProcedureName"></param>
        ///// <param name="TableName"></param>
        ///// <returns>DataTable</returns>
        //public DataTable ExecuteQuery( string ProcedureName, string TableName )
        //{
        //    try
        //    {
        //        object[] args = new object[]{ProcedureName, TableName};
        //        _myDatabaseMethodDelegate = new DatabaseMethodDelegate(myExecuteQuery2);
        //        object returnObject = ProcessReadCommand(new Command(_myDatabaseMethodDelegate, args, ProcedureName));
        //        return (DataTable)returnObject;
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}
        ///// <summary>
        ///// Executes a stored procedure with no parameters.
        ///// </summary>
        ///// <param name="aArgs"></param>
        ///// <returns>DataTable</returns>
        //private object myExecuteQuery2( object[] aArgs )
        //{
        //    string lastCommand = string.Empty;
        //    try
        //    {
        //        string ProcedureName = Convert.ToString(aArgs[0], CultureInfo.CurrentCulture);
        //        string TableName = Convert.ToString(aArgs[1], CultureInfo.CurrentCulture);
        //        SqlCommand cd = null;
        //        try
        //        {
        //            cd = new SqlCommand();
	
        //            lastCommand = "CreateConnection with " + ConnectionString;
        //            cd.Connection = CreateConnection(ConnectionString );
					
        //            lastCommand = ProcedureName;
        //            cd.CommandType = CommandType.StoredProcedure;
        //            cd.CommandText = ProcedureName;
        //            if (_commandTimeout != cd.CommandTimeout)
        //            {
        //                cd.CommandTimeout = _commandTimeout;
        //            }
        //            SQLMonitorList.SQLMonitorEntry se = null;
        //            SQLMonitorList.BeforeExecution(ref se, ProcedureName);

        //            DataTable dt = MIDEnvironment.CreateDataTable( TableName );
        //            SqlDataAdapter sda = new SqlDataAdapter( cd );
        //            sda.Fill( dt );
        //            SQLMonitorList.AfterExecution(se);
        //            return dt;
        //        }
        //        catch ( SqlException sql_error )
        //        {
        //            // Begin TT#3056 - JSmith - Header VIRTUAL_LOCK issue
        //            //HandleDatabaseReadException(sql_error, lastCommand, cd);
        //            _databaseExceptionHandler.HandleDatabaseException(sql_error, lastCommand, cd);
        //            // End TT#3056 - JSmith - Header VIRTUAL_LOCK issue
        //        }
        //        catch ( Exception error )
        //        {
        //            string message = error.ToString();
        //            throw;
        //        }
        //        finally
        //        {
        //            if (cd != null && cd.Connection != null && cd.Connection.State == ConnectionState.Open)
        //            {
        //                // swallow error if close fails because connection is already closed
        //                try
        //                {
        //                    cd.Connection.Close();
        //                }
        //                catch
        //                {
        //                }
        //                cd.Dispose();
        //            }
        //        }
        //        throw new Exception("Application error in database access");  //  Exception to appease compiler;  code should never get here	
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}

        ///// <summary>
        ///// Executes a simple SQL statement to read data.
        ///// </summary>
        ///// <param name="SqlStatement"></param>
        ///// <returns>DataTable</returns>
        //public DataTable ExecuteQuery( string SqlStatement )
        //{
        //    try
        //    {
        //        object[] args = new object[]{SqlStatement};
        //        _myDatabaseMethodDelegate = new DatabaseMethodDelegate(myExecuteQuery3);
        //        object returnObject = ProcessReadCommand(new Command(_myDatabaseMethodDelegate, args, SqlStatement));
        //        return (DataTable)returnObject;
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}
        ///// <summary>
        ///// Executes a simple SQL statement to read data.
        ///// </summary>
        ///// <param name="aArgs"></param>
        ///// <returns>DataTable</returns>
        //private object myExecuteQuery3( object[] aArgs )
        //{
        //    string lastCommand = string.Empty;
        //    try
        //    {
        //        string SqlStatement = Convert.ToString(aArgs[0], CultureInfo.CurrentCulture);
        //        SqlCommand cd = null;
        //        try
        //        {
        //            cd = new SqlCommand();
	
        //            lastCommand = "CreateConnection with " + ConnectionString;
        //            cd.Connection = CreateConnection(ConnectionString );
					
        //            lastCommand = SqlStatement;
        //            cd.CommandType = CommandType.Text;
        //            cd.CommandText = SqlStatement;
        //            if (_commandTimeout != cd.CommandTimeout)
        //            {
        //                cd.CommandTimeout = _commandTimeout;
        //            }
        //            SQLMonitorList.SQLMonitorEntry se = null;
        //            SQLMonitorList.BeforeExecution(ref se, SqlStatement);

        //            DataTable dt = MIDEnvironment.CreateDataTable( "Query Results" );
        //            SqlDataAdapter sda = new SqlDataAdapter( cd );
        //            sda.Fill( dt );
              
        //            SQLMonitorList.AfterExecution(se);
        //            return dt;
        //        }
        //        catch ( SqlException sql_error )
        //        {
        //            // Begin TT#3056 - JSmith - Header VIRTUAL_LOCK issue
        //            //HandleDatabaseReadException(sql_error, lastCommand, cd);
        //            _databaseExceptionHandler.HandleDatabaseException(sql_error, lastCommand, cd);
        //            // End TT#3056 - JSmith - Header VIRTUAL_LOCK issue
        //        }
        //        catch ( Exception error )
        //        {
        //            string message = error.ToString();
        //            throw;
        //        }
        //        finally
        //        {
        //            if (cd != null && cd.Connection != null && cd.Connection.State == ConnectionState.Open)
        //            {
        //                // swallow error if close fails because connection is already closed
        //                try
        //                {
        //                    cd.Connection.Close();
        //                }
        //                catch
        //                {
        //                }
        //                cd.Dispose();
        //            }
        //        }
        //        throw new Exception("Application error in database access");  //  Exception to appease compiler;  code should never get here	
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}

        ////Begin TT#1312 - JScott - Alternate Hierarchy Reclass
        ///// <summary>
        ///// Executes a stored procedure with only input parameters.
        ///// </summary>
        ///// <param name="ProcedureName"></param>
        ///// <param name="SetName"></param>
        ///// <param name="InputParameters"></param>
        ///// <returns>DataSet</returns>
        //public DataSet ExecuteQuery(string ProcedureName, MIDDbParameter[] InputParameters, string SetName)
        //{
        //    try
        //    {
        //        object[] args = new object[] { ProcedureName, SetName, InputParameters };
        //        _myDatabaseMethodDelegate = new DatabaseMethodDelegate(myExecuteQuery4);
        //        object returnObject = ProcessReadCommand(new Command(_myDatabaseMethodDelegate, args, ProcedureName));
        //        return (DataSet)returnObject;
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}
        ///// <summary>
        ///// Executes a stored procedure with only input parameters.
        ///// </summary>
        ///// <param name="aArgs"></param>
        ///// <returns>DataSet</returns>
        //private object myExecuteQuery4(object[] aArgs)
        //{
        //    string lastCommand = string.Empty;
        //    try
        //    {
        //        string ProcedureName = Convert.ToString(aArgs[0], CultureInfo.CurrentCulture);
        //        string SetName = Convert.ToString(aArgs[1], CultureInfo.CurrentCulture);
        //        MIDDbParameter[] InputParameters = (MIDDbParameter[])aArgs[2];
        //        SqlCommand cd = null;
        //        try
        //        {
        //            cd = new SqlCommand();

        //            AddInputParametersToSQLCommand(InputParameters, ref cd);

        //            lastCommand = "CreateConnection with " + ConnectionString;
        //            cd.Connection = CreateConnection(ConnectionString);

        //            lastCommand = ProcedureName;
        //            cd.CommandType = CommandType.StoredProcedure;
        //            cd.CommandText = ProcedureName;
        //            if (_commandTimeout != cd.CommandTimeout)
        //            {
        //                cd.CommandTimeout = _commandTimeout;
        //            }
        //            SQLMonitorList.SQLMonitorEntry se = null;
        //            SQLMonitorList.BeforeExecution(ref se, ProcedureName, InputParameters);

        //            DataSet ds = MIDEnvironment.CreateDataSet(SetName);
        //            SqlDataAdapter sda = new SqlDataAdapter(cd);
        //            sda.Fill(ds);
        //            SQLMonitorList.AfterExecution(se);
        //            return ds;
        //        }
        //        catch (SqlException sql_error)
        //        {
        //            // Begin TT#3056 - JSmith - Header VIRTUAL_LOCK issue
        //            //HandleDatabaseReadException(sql_error, lastCommand, cd);
        //            _databaseExceptionHandler.HandleDatabaseException(sql_error, lastCommand, cd);
        //            // End TT#3056 - JSmith - Header VIRTUAL_LOCK issue
        //        }
        //        catch (Exception error)
        //        {
        //            string message = error.ToString();
        //            throw;
        //        }
        //        finally
        //        {
        //            if (cd != null && cd.Connection != null && cd.Connection.State == ConnectionState.Open)
        //            {
        //                // swallow error if close fails because connection is already closed
        //                try
        //                {
        //                    cd.Connection.Close();
        //                }
        //                catch
        //                {
        //                }
        //                cd.Dispose();
        //            }
        //        }
        //        throw new Exception("Application error in database access");  //  Exception to appease compiler;  code should never get here	
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}

        //// Begin TT#110-MD - JSmith - In Use Tool
        ///// <summary>
        ///// Executes a stored procedure that returns a datatable with input and output parameters.
        ///// </summary>
        ///// <param name="ProcedureName"></param>
        ///// <param name="TableName"></param>
        ///// <param name="InputParameters"></param>
        ///// <param name="OutputParameters"></param>
        ///// <returns>DataTable</returns>
        //public DataTable ExecuteQuery(string ProcedureName, string TableName, MIDDbParameter[] InputParameters, MIDDbParameter[] OutputParameters)
        //{
        //    try
        //    {
        //        object[] args = new object[] { ProcedureName, TableName, InputParameters, OutputParameters, SPReturnType.DataTable };
        //        _myDatabaseMethodDelegate = new DatabaseMethodDelegate(myExecuteQuery5);
        //        object returnObject = ProcessReadCommand(new Command(_myDatabaseMethodDelegate, args, ProcedureName));
        //        return (DataTable)returnObject;
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}
        ///// <summary>
        ///// Executes a stored procedure with only input parameters.
        ///// </summary>
        ///// <param name="aArgs"></param>
        ///// <returns>DataTable</returns>
        //private object myExecuteQuery5(object[] aArgs)
        //{
        //    string lastCommand = string.Empty;
        //    try
        //    {
        //        string ProcedureName = Convert.ToString(aArgs[0], CultureInfo.CurrentCulture);
        //        string TableName = Convert.ToString(aArgs[1], CultureInfo.CurrentCulture);
        //        MIDDbParameter[] InputParameters = (MIDDbParameter[])aArgs[2];
        //        MIDDbParameter[] OutputParameters = (MIDDbParameter[])aArgs[3];
        //        SPReturnType returnType = (SPReturnType)aArgs[4];
        //        SqlCommand cd = null;
        //        try
        //        {
        //            cd = new SqlCommand();

        //            AddInputParametersToSQLCommand(InputParameters, ref cd);

        //            if (OutputParameters != null && OutputParameters.Length > 0)
        //            {
        //                for (int i = 0; i < OutputParameters.Length; i++)
        //                {
        //                    SqlParameter OutParam = new SqlParameter();
        //                    OutParam.Direction = ParameterDirection.Output;
        //                    OutParam.ParameterName = OutputParameters[i].ParameterName;
        //                    OutParam.Value = OutputParameters[i].Value;
        //                    switch (OutputParameters[i].DbType)
        //                    {
        //                        case eDbType.Bit:
        //                            OutParam.SqlDbType = SqlDbType.Bit;
        //                            break;
        //                        case eDbType.Char:
        //                            OutParam.SqlDbType = SqlDbType.Char;
        //                            break;
        //                        case eDbType.DateTime:
        //                            OutParam.SqlDbType = SqlDbType.DateTime;
        //                            break;
        //                        case eDbType.Float:
        //                            OutParam.SqlDbType = SqlDbType.Float;
        //                            break;
        //                        case eDbType.Decimal:
        //                            OutParam.SqlDbType = SqlDbType.Decimal;
        //                            break;
        //                        case eDbType.Int:
        //                            OutParam.SqlDbType = SqlDbType.Int;
        //                            break;
        //                        case eDbType.SmallDateTime:
        //                            OutParam.SqlDbType = SqlDbType.SmallDateTime;
        //                            break;
        //                        case eDbType.VarChar:
        //                            OutParam.SqlDbType = SqlDbType.VarChar;
        //                            break;
        //                        case eDbType.Image:
        //                            OutParam.SqlDbType = SqlDbType.Image;
        //                            break;
        //                        case eDbType.Text:
        //                            OutParam.SqlDbType = SqlDbType.Text;
        //                            break;
        //                        case eDbType.smallint:
        //                            OutParam.SqlDbType = SqlDbType.SmallInt;
        //                            break;
        //                        case eDbType.tinyint:
        //                            OutParam.SqlDbType = SqlDbType.TinyInt;
        //                            break;
        //                        case eDbType.VarBinary:
        //                            OutParam.SqlDbType = SqlDbType.VarBinary;
        //                            break;
        //                        case eDbType.Int64:
        //                            OutParam.SqlDbType = SqlDbType.BigInt;
        //                            break;
        //                        default:
        //                            break;
        //                    }
        //                    cd.Parameters.Add(OutParam);
        //                }
        //            }

        //            lastCommand = "CreateConnection with " + ConnectionString;
        //            cd.Connection = CreateConnection(ConnectionString);

        //            lastCommand = ProcedureName;
        //            cd.CommandType = CommandType.StoredProcedure;
        //            cd.CommandText = ProcedureName;
        //            if (_commandTimeout != cd.CommandTimeout)
        //            {
        //                cd.CommandTimeout = _commandTimeout;
        //            }
        //            SQLMonitorList.SQLMonitorEntry se = null;
        //            SQLMonitorList.BeforeExecution(ref se, ProcedureName, InputParameters);

        //            if (returnType == SPReturnType.DataTable)
        //            {
        //                DataTable dt = MIDEnvironment.CreateDataTable(TableName);
        //                SqlDataAdapter sda = new SqlDataAdapter(cd);
        //                sda.Fill(dt);

        //                if (OutputParameters != null && OutputParameters.Length > 0)
        //                {
        //                    for (int i = 0; i < OutputParameters.Length; i++)
        //                    {
        //                        OutputParameters[i].Value = OutputParmValue(cd, OutputParameters[i].ParameterName);
        //                    }
        //                }
        //                SQLMonitorList.AfterExecution(se);
        //                return dt;
        //            }
        //            else
        //            {
        //                DataSet ds = MIDEnvironment.CreateDataSet(TableName);
        //                SqlDataAdapter sda = new SqlDataAdapter(cd);
        //                sda.Fill(ds);

        //                if (OutputParameters != null && OutputParameters.Length > 0)
        //                {
        //                    for (int i = 0; i < OutputParameters.Length; i++)
        //                    {
        //                        OutputParameters[i].Value = OutputParmValue(cd, OutputParameters[i].ParameterName);
        //                    }
        //                }
        //                SQLMonitorList.AfterExecution(se);
        //                return ds;
        //            }
        //        }
        //        catch (SqlException sql_error)
        //        {
        //            // Begin TT#3056 - JSmith - Header VIRTUAL_LOCK issue
        //            //HandleDatabaseReadException(sql_error, lastCommand, cd);
        //            _databaseExceptionHandler.HandleDatabaseException(sql_error, lastCommand, cd);
        //            // End TT#3056 - JSmith - Header VIRTUAL_LOCK issue
        //        }
        //        catch (Exception error)
        //        {
        //            string message = error.ToString();
        //            throw;
        //        }
        //        finally
        //        {
        //            if (cd != null && cd.Connection != null && cd.Connection.State == ConnectionState.Open)
        //            {
        //                // swallow error if close fails because connection is already closed
        //                try
        //                {
        //                    cd.Connection.Close();
        //                }
        //                catch
        //                {
        //                }
        //                cd.Dispose();
        //            }
        //        }
        //        throw new Exception("Application error in database access");  //  Exception to appease compiler;  code should never get here	
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}

        // End TT#110-MD - JSmith - In Use Tool

		//End TT#1312 - JScott - Alternate Hierarchy Reclass
        //public DataSet FillDataSet(DataSet aDataSet, string aDataSetName, string aSqlStatement)
        //{
        //    try
        //    {
        //        object[] args = new object[]{aDataSet, aDataSetName, aSqlStatement};
        //        _myDatabaseMethodDelegate = new DatabaseMethodDelegate(myFillDataSet);
        //        object returnObject = ProcessReadCommand(new Command(_myDatabaseMethodDelegate, args, aSqlStatement));
        //        return (DataSet)returnObject;
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}

		/// <summary>
		/// Fills a dataset with the provided query.
		/// </summary>
		/// <param name="aArgs"></param>
		/// <returns>DataSet</returns>
        //private object myFillDataSet( object[] aArgs )
        //{
        //    string lastCommand = string.Empty;
        //    try
        //    {
        //        DataSet ds = (DataSet)aArgs[0];
        //        string dataSetName = Convert.ToString(aArgs[1], CultureInfo.CurrentCulture);
        //        string SqlStatement = Convert.ToString(aArgs[2], CultureInfo.CurrentCulture);
        //        SqlCommand cd = null;
        //        try
        //        {
        //            cd = new SqlCommand();
	
        //            lastCommand = "CreateConnection with " + ConnectionString;
        //            cd.Connection = CreateConnection(ConnectionString );
					
        //            lastCommand = SqlStatement;
        //            cd.CommandType = CommandType.Text;
        //            cd.CommandText = SqlStatement;
        //            if (_commandTimeout != cd.CommandTimeout)
        //            {
        //                cd.CommandTimeout = _commandTimeout;
        //            }
        //            SQLMonitorList.SQLMonitorEntry se = null;
        //            SQLMonitorList.BeforeExecution(ref se, SqlStatement);
        //            SqlDataAdapter sda = new SqlDataAdapter( cd  );
        //            sda.Fill( ds.Tables[dataSetName] );
        //            SQLMonitorList.AfterExecution(se);
        //            return ds;
        //        }
        //        catch ( SqlException sql_error )
        //        {
        //            // Begin TT#3056 - JSmith - Header VIRTUAL_LOCK issue
        //            //HandleDatabaseReadException(sql_error, lastCommand, cd);
        //            _databaseExceptionHandler.HandleDatabaseException(sql_error, lastCommand, cd);
        //            // End TT#3056 - JSmith - Header VIRTUAL_LOCK issue
        //        }
        //        catch ( Exception error )
        //        {
        //            string message = error.ToString();
        //            throw;
        //        }
        //        finally
        //        {
        //            if (cd != null && cd.Connection != null && cd.Connection.State == ConnectionState.Open)
        //            {
        //                // swallow error if close fails because connection is already closed
        //                try
        //                {
        //                    cd.Connection.Close();
        //                }
        //                catch
        //                {
        //                }
        //                cd.Dispose();
        //            }
        //        }
        //        throw new Exception("Application error in database access");  //  Exception to appease compiler;  code should never get here	
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}

		/// <summary>
		/// Executes a non-query SQL statement.
		/// </summary>
		/// <param name="SQLStatement"></param>
		/// <returns>The number of rows affected by the statement</returns>
		public int ExecuteNonQuery(string SQLStatement)
		{
			try
			{
				object[] args = new object[]{SQLStatement};
				_myDatabaseMethodDelegate = new DatabaseMethodDelegate(myExecuteNonQuery);
				Command command = new Command(_myDatabaseMethodDelegate, args, SQLStatement);
				_updateCommands.Add(command);
				object returnObject = ProcessUpdateCommand(command);
				return Convert.ToInt32(returnObject, CultureInfo.CurrentCulture);
			}
			catch
			{
				throw;
			}
		}
		/// <summary>
		/// Executes a non-query SQL statement.
		/// </summary>
		/// <param name="aArgs"></param>
		/// <returns>The number of rows affected by the statement</returns>
		private object myExecuteNonQuery(object[] aArgs)
		{
			try
			{
				string SQLStatement = Convert.ToString(aArgs[0], CultureInfo.CurrentCulture);
				try
				{
					_updateConnection.SQLCommand.Parameters.Clear();
					_updateConnection.SQLCommand.CommandType = CommandType.Text;
					_updateConnection.SQLCommand.CommandText = SQLStatement;
					if (_commandTimeout != _updateConnection.SQLCommand.CommandTimeout)
					{
						_updateConnection.SQLCommand.CommandTimeout = _commandTimeout;
					}

                    SQLMonitorList.SQLMonitorEntry se = null;
                    SQLMonitorList.BeforeExecution(ref se, SQLStatement);


                    int result = _updateConnection.SQLCommand.ExecuteNonQuery();
                    SQLMonitorList.AfterExecution(se);
                    return result;
				
				}
				catch ( SqlException sql_error )
				{
                    // Begin TT#3056 - JSmith - Header VIRTUAL_LOCK issue
                    CloseDatabaseUpdateConnection();
                    //HandleDatabaseUpdateException(sql_error, SQLStatement);
                    _databaseExceptionHandler.HandleDatabaseException(sql_error, SQLStatement, _updateConnection.SQLCommand);
                    // End TT#3056 - JSmith - Header VIRTUAL_LOCK issue
				}
				catch ( Exception error )
				{
					string message = error.ToString();
					throw;
				}
				throw new Exception("Application error in database access");  //  Exception to appease compiler;  code should never get here	
			}
			catch
			{
				throw;
			}
		}
        //Begin TT#846-MD -jsobek -New Stored Procedures for Performance
        ///// <summary>
        ///// Executes a non-query SQL statement.
        ///// </summary>
        ///// <param name="SQLStatement"></param>
        ///// <param name="commandTimeout">The wait time in seconds before terminating the command and generating an error</param>
        ///// <returns>The number of rows affected by the statement</returns>
        //public int ExecuteNonQuery(string SQLStatement, int commandTimeout)
        //{
        //    try
        //    {
        //        object[] args = new object[] { SQLStatement, commandTimeout };
        //        _myDatabaseMethodDelegate = new DatabaseMethodDelegate(myExecuteNonQuery2);
        //        Command command = new Command(_myDatabaseMethodDelegate, args, SQLStatement);
        //        _updateCommands.Add(command);
        //        object returnObject = ProcessUpdateCommand(command);
        //        return Convert.ToInt32(returnObject, CultureInfo.CurrentCulture);
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}
        /// <summary>
        /// Executes a non-query SQL statement.
        /// </summary>
        /// <param name="aArgs"></param>
        /// <returns>The number of rows affected by the statement</returns>
        //private object myExecuteNonQuery2(object[] aArgs)
        //{
        //    try
        //    {
        //        string SQLStatement = Convert.ToString(aArgs[0], CultureInfo.CurrentCulture);
        //        int commandTimeout = Convert.ToInt32(aArgs[1], CultureInfo.CurrentCulture);
        //        try
        //        {
        //            _updateConnection.SQLCommand.Parameters.Clear();
        //            _updateConnection.SQLCommand.CommandType = CommandType.Text;
        //            _updateConnection.SQLCommand.CommandText = SQLStatement;
        //            if (_updateConnection.SQLCommand.CommandTimeout != commandTimeout)
        //            {
        //                _updateConnection.SQLCommand.CommandTimeout = commandTimeout;
        //            }
        //            SQLMonitorList.SQLMonitorEntry se = null;
        //            SQLMonitorList.BeforeExecution(ref se, SQLStatement);

        //            int result = _updateConnection.SQLCommand.ExecuteNonQuery();
        //            SQLMonitorList.AfterExecution(se);
        //            return result;

        //        }
        //        catch (SqlException sql_error)
        //        {
        //            // Begin TT#3056 - JSmith - Header VIRTUAL_LOCK issue
        //            CloseDatabaseUpdateConnection();
        //            //HandleDatabaseUpdateException(sql_error, SQLStatement);
        //            _databaseExceptionHandler.HandleDatabaseException(sql_error, SQLStatement, _updateConnection.SQLCommand);
        //            // End TT#3056 - JSmith - Header VIRTUAL_LOCK issue
        //        }
        //        catch (Exception error)
        //        {
        //            string message = error.ToString();
        //            throw;
        //        }
        //        throw new Exception("Application error in database access");  //  Exception to appease compiler;  code should never get here	
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}
        //End TT#846-MD -jsobek -New Stored Procedures for Performance

        ///// <summary>
        ///// Executes a nonquery SQL statement that requires input parameters.
        ///// </summary>
        ///// <param name="SQLStatement"></param>
        ///// <param name="InputParameters"></param>
        ///// <returns>The number of rows affected by the statement</returns>
        //public int ExecuteNonQuery(string SQLStatement, MIDDbParameter[] InputParameters)
        //{
        //    try
        //    {
        //        object[] args = new object[]{SQLStatement, InputParameters};
        //        _myDatabaseMethodDelegate = new DatabaseMethodDelegate(myExecuteNonQuery3);
        //        Command command = new Command(_myDatabaseMethodDelegate, args, SQLStatement);
        //        _updateCommands.Add(command);
        //        object returnObject = ProcessUpdateCommand(command);
        //        return Convert.ToInt32(returnObject, CultureInfo.CurrentCulture);
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}
//        /// <summary>
//        /// Executes a nonquery SQL statement that requires input parameters.
//        /// </summary>
//        /// <param name="aArgs"></param>
//        /// <returns>The number of rows affected by the statement</returns>
//        private object myExecuteNonQuery3(object[] aArgs)
//        {
//            try
//            {
//                string SQLStatement = Convert.ToString(aArgs[0], CultureInfo.CurrentCulture);
//                MIDDbParameter[] InputParameters = (MIDDbParameter[])aArgs[1];
//                try
//                {
//                    _updateConnection.SQLCommand.Parameters.Clear();
//                    _updateConnection.SQLCommand.CommandType = CommandType.Text;
//                    _updateConnection.SQLCommand.CommandText = SQLStatement;
//                    if (_commandTimeout != _updateConnection.SQLCommand.CommandTimeout)
//                    {
//                        _updateConnection.SQLCommand.CommandTimeout = _commandTimeout;
//                    }

//                    SqlCommand cd = _updateConnection.SQLCommand;
//                    AddInputParametersToSQLCommand(InputParameters, ref cd);
					
//                    SQLMonitorList.SQLMonitorEntry se = null;
//                    SQLMonitorList.BeforeExecution(ref se, SQLStatement, InputParameters);
//                    int result = _updateConnection.SQLCommand.ExecuteNonQuery();
//                    SQLMonitorList.AfterExecution(se);
//                    return result;
				
//                }
//                catch ( SqlException sql_error )
//                {
//                    // Begin TT#3056 - JSmith - Header VIRTUAL_LOCK issue
////					EventLog.WriteEntry("MIDRetail", "Database error; ConnectionString=" + ConnectionString + ";Command=" + _updateConnection.SQLCommand.CommandText, EventLogEntryType.Error);
//                    CloseDatabaseUpdateConnection();
//                    //HandleDatabaseUpdateException(sql_error, SQLStatement);
//                    _databaseExceptionHandler.HandleDatabaseException(sql_error, SQLStatement, _updateConnection.SQLCommand);
//                    // End TT#3056 - JSmith - Header VIRTUAL_LOCK issue
//                }
//                catch ( Exception error )
//                {
//                    string message = error.ToString();
//                    throw;
//                }
//                throw new Exception("Application error in database access");  //  Exception to appease compiler;  code should never get here		
//            }
//            catch
//            {
//                throw;
//            }
//        }


        //Begin TT#846-MD -jsobek -New Stored Procedures for Performance

        public DataTable ExecuteStoredProcedureForRead(string ProcedureName, MIDDbParameter[] InputParameters = null, MIDDbParameter[] OutputParameters = null, int commandTimeout = -1)
        {
            try
            {
                object[] args = new object[] { ProcedureName, InputParameters, OutputParameters, commandTimeout };

                _myDatabaseMethodDelegate = new DatabaseMethodDelegate(myExecuteStoredProcedureForReadAsDataTable);
                object returnObject = ProcessReadCommand(new Command(_myDatabaseMethodDelegate, args, ProcedureName));
                return (DataTable)returnObject; 
            }
            catch
            {
                throw;
            }
        }
        
        /// <summary>
        /// Executes a stored procedure for reading data
        /// </summary>
        /// <param name="aArgs"></param>
        /// <returns>DataTable</returns>
        private DataTable myExecuteStoredProcedureForReadAsDataTable(object[] aArgs)
        {
            string lastCommand = string.Empty;
            try
            {
                string ProcedureName = Convert.ToString(aArgs[0], CultureInfo.CurrentCulture);
                MIDDbParameter[] InputParameters = (MIDDbParameter[])aArgs[1];
                MIDDbParameter[] OutputParameters = (MIDDbParameter[])aArgs[2];
                int commandTimeout = Convert.ToInt32(aArgs[3], CultureInfo.CurrentCulture);
                SqlCommand cd = null;
                try
                {
                    cd = new SqlCommand();

                    AddInputParametersToSQLCommand(InputParameters, ref cd);
                    AddOutputParametersToSQLCommand(OutputParameters, ref cd);

                    //lastCommand = "CreateConnection with " + ConnectionString;
                    cd.Connection = CreateConnection(DBConnectionString);  // TT#2131-MD - JSmith - Halo Integration

                    lastCommand = ProcedureName;
                    cd.CommandType = CommandType.StoredProcedure;
                    cd.CommandText = ProcedureName;

                    if (commandTimeout != -1)
                    {
                        //A specific time out was sent for this call
                        cd.CommandTimeout = commandTimeout;
                    }
                    else
                    {
                        if (_commandTimeout != cd.CommandTimeout)
                        {
                            cd.CommandTimeout = _commandTimeout;
                        }
                    }
                    SQLMonitorList.SQLMonitorEntry se = null;
                    SQLMonitorList.BeforeExecution(ref se, ProcedureName, InputParameters);

                    DataTable dt = MIDEnvironment.CreateDataTable();
                    SqlDataAdapter sda = new SqlDataAdapter(cd);
                    sda.Fill(dt);
                    SQLMonitorList.AfterExecution(se);
                    if (OutputParameters != null)
                    {
                        SetValuesOnOutputParamters(cd, ref OutputParameters);
                    }
                    return dt;

                }
                catch (SqlException sql_error)
                {
                    _databaseExceptionHandler.HandleDatabaseException(sql_error, lastCommand, cd);
                }
                catch (Exception error)
                {
                    string message = error.ToString();
                    throw;
                }
                finally
                {
                    if (cd != null && cd.Connection != null && cd.Connection.State == ConnectionState.Open)
                    {
                        // swallow error if close fails because connection is already closed
                        try
                        {
                            cd.Connection.Close();
                        }
                        catch
                        {
                        }
                        cd.Dispose();
                    }
                }
                throw new Exception("Application error in database access");  //  Exception to appease compiler;  code should never get here	
            }
            catch
            {
                throw;
            }
        }
        public DataSet ExecuteStoredProcedureForReadAsDataSet(string ProcedureName, MIDDbParameter[] InputParameters = null, MIDDbParameter[] OutputParameters = null, int commandTimeOut = -1)
        {
            try
            {
                object[] args = new object[] { ProcedureName, InputParameters, OutputParameters, commandTimeOut };

                _myDatabaseMethodDelegate = new DatabaseMethodDelegate(myExecuteStoredProcedureForReadAsDataSet);
                object returnObject = ProcessReadCommand(new Command(_myDatabaseMethodDelegate, args, ProcedureName));
                return (DataSet)returnObject;
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// Executes a stored procedure for reading data
        /// </summary>
        /// <param name="aArgs"></param>
        /// <returns>DataTable</returns>
        private DataSet myExecuteStoredProcedureForReadAsDataSet(object[] aArgs)
        {
            string lastCommand = string.Empty;
            try
            {
                string ProcedureName = Convert.ToString(aArgs[0], CultureInfo.CurrentCulture);
                MIDDbParameter[] InputParameters = (MIDDbParameter[])aArgs[1];
                MIDDbParameter[] OutputParameters = (MIDDbParameter[])aArgs[2];
                int commandTimeout = Convert.ToInt32(aArgs[3], CultureInfo.CurrentCulture);
                SqlCommand cd = null;
                try
                {
                    cd = new SqlCommand();

                    AddInputParametersToSQLCommand(InputParameters, ref cd);
                    AddOutputParametersToSQLCommand(OutputParameters, ref cd);

                    //lastCommand = "CreateConnection with " + ConnectionString;
                    cd.Connection = CreateConnection(DBConnectionString);  // TT#2131-MD - JSmith - Halo Integration

                    lastCommand = ProcedureName;
                    cd.CommandType = CommandType.StoredProcedure;
                    cd.CommandText = ProcedureName;
                    if (commandTimeout != -1)
                    {
                        //A specific time out was sent for this call
                        cd.CommandTimeout = commandTimeout;
                    }
                    else
                    {
                        if (_commandTimeout != cd.CommandTimeout)
                        {
                            cd.CommandTimeout = _commandTimeout;
                        }
                    }
                    SQLMonitorList.SQLMonitorEntry se = null;
                    SQLMonitorList.BeforeExecution(ref se, ProcedureName, InputParameters);


                    DataSet ds = MIDEnvironment.CreateDataSet();
                    SqlDataAdapter sda = new SqlDataAdapter(cd);
                    sda.Fill(ds);
                    SQLMonitorList.AfterExecution(se);
                    if (OutputParameters != null)
                    {
                        SetValuesOnOutputParamters(cd, ref OutputParameters);
                    }
                    return ds;
                }
                catch (SqlException sql_error)
                {
                    _databaseExceptionHandler.HandleDatabaseException(sql_error, lastCommand, cd);
                }
                catch (Exception error)
                {
                    string message = error.ToString();
                    throw;
                }
                finally
                {
                    if (cd != null && cd.Connection != null && cd.Connection.State == ConnectionState.Open)
                    {
                        // swallow error if close fails because connection is already closed
                        try
                        {
                            cd.Connection.Close();
                        }
                        catch
                        {
                        }
                        cd.Dispose();
                    }
                }
                throw new Exception("Application error in database access");  //  Exception to appease compiler;  code should never get here	
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Executes a stored procedure that updates data in table
        /// </summary>
        /// <param name="SQLStatement"></param>
        /// <param name="InputParameters"></param>
        /// <returns>The number of rows affected by the statement</returns>
        public int ExecuteStoredProcedureForUpdate(string SQLStatement, MIDDbParameter[] InputParameters, MIDDbParameter[] OutputParameters = null, int commandTimeout = -1)
        {
            try
            {
                object[] args = new object[] { SQLStatement, InputParameters, OutputParameters, commandTimeout };
                _myDatabaseMethodDelegate = new DatabaseMethodDelegate(myExecuteStoredProcedureForUpdate);
                Command command = new Command(_myDatabaseMethodDelegate, args, SQLStatement);
                _updateCommands.Add(command);
                object returnObject = ProcessUpdateCommand(command);
                return Convert.ToInt32(returnObject, CultureInfo.CurrentCulture);
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// Executes a stored procedure that updates data in table
        /// </summary>
        /// <param name="aArgs"></param>
        /// <returns>The number of rows affected by the statement</returns>
        private object myExecuteStoredProcedureForUpdate(object[] aArgs)
        {
            try
            {
                string SQLStatement = Convert.ToString(aArgs[0], CultureInfo.CurrentCulture);
                MIDDbParameter[] InputParameters = (MIDDbParameter[])aArgs[1];
                MIDDbParameter[] OutputParameters = (MIDDbParameter[])aArgs[2];
                int commandTimeout = Convert.ToInt32(aArgs[3], CultureInfo.CurrentCulture);
                try
                {
                    //if (_updateConnection == null)
                    //{
                    //    OpenUpdateConnection();
                    //}


                    _updateConnection.SQLCommand.Parameters.Clear();
                    _updateConnection.SQLCommand.CommandType = CommandType.StoredProcedure;
                    _updateConnection.SQLCommand.CommandText = SQLStatement;
                    if (commandTimeout == -1)
                    {
                        if (_commandTimeout != _updateConnection.SQLCommand.CommandTimeout)
                        {
                            _updateConnection.SQLCommand.CommandTimeout = _commandTimeout;
                        }
                    }
                    else
                    {
                        if (_updateConnection.SQLCommand.CommandTimeout != commandTimeout)
                        {
                            _updateConnection.SQLCommand.CommandTimeout = commandTimeout;
                        }
                    }

                    SqlCommand cd = _updateConnection.SQLCommand;
                    AddInputParametersToSQLCommand(InputParameters, ref cd);
                    AddOutputParametersToSQLCommand(OutputParameters, ref cd);

                    SQLMonitorList.SQLMonitorEntry se = null;
                    SQLMonitorList.BeforeExecution(ref se, SQLStatement, InputParameters);
                    int rowsUpdated = 0;
                    object obj = _updateConnection.SQLCommand.ExecuteScalar();
                    if (obj != null)
                    {
                        int.TryParse(obj.ToString(), out rowsUpdated); //TT#1309-MD -jsobek -Cancel  Allocation results in Action failed
                        //rowsUpdated = (int)obj; //TT#1309-MD -jsobek -Cancel  Allocation results in Action failed
                    }
                    SQLMonitorList.AfterExecution(se);
                    if (OutputParameters != null)
                    {
                        SetValuesOnOutputParamters(cd, ref OutputParameters);
                    }
                    return rowsUpdated;

                }
                catch (SqlException sql_error)
                {
                    CloseDatabaseUpdateConnection();
                    _databaseExceptionHandler.HandleDatabaseException(sql_error, SQLStatement, _updateConnection.SQLCommand);

                }
                catch (Exception error)
                {
                    string message = error.ToString();
                    throw;
                }
                throw new Exception("Application error in database access");  //  Exception to appease compiler;  code should never get here		
            }
            catch
            {
                throw;
            }
        }
        public DataTable ExecuteStoredProcedureForInsertAndRead(string SQLStatement, MIDDbParameter[] InputParameters, MIDDbParameter[] OutputParameters = null, int commandTimeout = -1)
        {
            try
            {
                object[] args = new object[] { SQLStatement, InputParameters, OutputParameters, commandTimeout };
                _myDatabaseMethodDelegate = new DatabaseMethodDelegate(myExecuteStoredProcedureForInsertAndRead);
                Command command = new Command(_myDatabaseMethodDelegate, args, SQLStatement);
                _updateCommands.Add(command);
                object returnObject = ProcessUpdateCommand(command);
                return (DataTable)returnObject; 
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// Executes a stored procedure that inserts data in table and returns a datatable
        /// </summary>
        /// <param name="aArgs"></param>
        /// <returns>The number of rows affected by the statement</returns>
        private object myExecuteStoredProcedureForInsertAndRead(object[] aArgs)
        {
            try
            {
                string SQLStatement = Convert.ToString(aArgs[0], CultureInfo.CurrentCulture);
                MIDDbParameter[] InputParameters = (MIDDbParameter[])aArgs[1];
                MIDDbParameter[] OutputParameters = (MIDDbParameter[])aArgs[2];
                int commandTimeout = Convert.ToInt32(aArgs[3], CultureInfo.CurrentCulture);
                try
                {
                    //if (_updateConnection == null)
                    //{
                    //    OpenUpdateConnection();
                    //}


                    _updateConnection.SQLCommand.Parameters.Clear();
                    _updateConnection.SQLCommand.CommandType = CommandType.StoredProcedure;
                    _updateConnection.SQLCommand.CommandText = SQLStatement;
                    if (commandTimeout == -1)
                    {
                        if (_commandTimeout != _updateConnection.SQLCommand.CommandTimeout)
                        {
                            _updateConnection.SQLCommand.CommandTimeout = _commandTimeout;
                        }
                    }
                    else
                    {
                        if (_updateConnection.SQLCommand.CommandTimeout != commandTimeout)
                        {
                            _updateConnection.SQLCommand.CommandTimeout = commandTimeout;
                        }
                    }

                    SqlCommand cd = _updateConnection.SQLCommand;
                    AddInputParametersToSQLCommand(InputParameters, ref cd);
                    AddOutputParametersToSQLCommand(OutputParameters, ref cd);

                    SQLMonitorList.SQLMonitorEntry se = null;
                    SQLMonitorList.BeforeExecution(ref se, SQLStatement, InputParameters);
                    
                    DataTable dt = MIDEnvironment.CreateDataTable();
                    SqlDataAdapter sda = new SqlDataAdapter(cd);
                    sda.Fill(dt);
                    SQLMonitorList.AfterExecution(se);
                    if (OutputParameters != null)
                    {
                        SetValuesOnOutputParamters(cd, ref OutputParameters);
                    }
                    return dt;

                }
                catch (SqlException sql_error)
                {
                    CloseDatabaseUpdateConnection();
                    _databaseExceptionHandler.HandleDatabaseException(sql_error, SQLStatement, _updateConnection.SQLCommand);

                }
                catch (Exception error)
                {
                    string message = error.ToString();
                    throw;
                }
                throw new Exception("Application error in database access");  //  Exception to appease compiler;  code should never get here		
            }
            catch
            {
                throw;
            }
        }
        ///// <summary>
        ///// Executes stores procedures for update that returns an integer returnCode
        ///// </summary>
        ///// <param name="ProcedureName"></param>
        ///// <param name="InputParameters"></param>
        ///// <param name="OutputParameters"></param>
        ///// <returns></returns>
        //public int ExecuteStoredProcedureForUpdateWithReturnCode(string ProcedureName, MIDDbParameter[] InputParameters, MIDDbParameter[] OutputParameters)
        //{
        //    try
        //    {
        //        object[] args = new object[] { ProcedureName, InputParameters, OutputParameters };
        //        _myDatabaseMethodDelegate = new DatabaseMethodDelegate(myExecuteStoredProcedureForUpdateWithReturnCode);
        //        Command command = new Command(_myDatabaseMethodDelegate, args, ProcedureName);
        //        _updateCommands.Add(command);
        //        object returnObject = ProcessUpdateCommand(command);
        //        return Convert.ToInt32(returnObject, CultureInfo.CurrentCulture);
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}
        ///// <summary>
        ///// Executes stores procedures for update that returns an integer returnCode
        ///// </summary>
        ///// <param name="aArgs"></param>
        ///// <returns>int</returns>
        //private object myExecuteStoredProcedureForUpdateWithReturnCode(object[] aArgs)
        //{
        //    try
        //    {
        //        string ProcedureName = Convert.ToString(aArgs[0], CultureInfo.CurrentCulture);
        //        MIDDbParameter[] InputParameters = (MIDDbParameter[])aArgs[1];
        //        MIDDbParameter[] OutputParameters = (MIDDbParameter[])aArgs[2];
        //        try
        //        {
        //            int returnCode = -1;
        //            _updateConnection.SQLCommand.Parameters.Clear();
        //            _updateConnection.SQLCommand.CommandType = CommandType.StoredProcedure;
        //            _updateConnection.SQLCommand.CommandText = ProcedureName;
        //            if (_commandTimeout != _updateConnection.SQLCommand.CommandTimeout)
        //            {
        //                _updateConnection.SQLCommand.CommandTimeout = _commandTimeout;
        //            }

        //            SqlCommand cd = _updateConnection.SQLCommand;
        //            AddInputParametersToSQLCommand(InputParameters, ref cd);
        //            AddOutputParametersToSQLCommand(OutputParameters, ref cd);
 

        //            SQLMonitorList.SQLMonitorEntry se = null;
        //            SQLMonitorList.BeforeExecution(ref se, ProcedureName, InputParameters);

        //            _updateConnection.SQLCommand.ExecuteNonQuery();
        //            SQLMonitorList.AfterExecution(se);
        //            foreach (SqlParameter param in _updateConnection.SQLCommand.Parameters)
        //                if (param.Direction == ParameterDirection.Output || param.Direction == ParameterDirection.InputOutput)
        //                {
        //                    returnCode = (int)param.Value;
        //                }

        //            return returnCode;
        //        }
        //        catch (SqlException sql_error)
        //        {
        //            // Begin TT#3056 - JSmith - Header VIRTUAL_LOCK issue
        //            CloseDatabaseUpdateConnection();
        //            //HandleDatabaseUpdateException(sql_error, ProcedureName);
        //            _databaseExceptionHandler.HandleDatabaseException(sql_error, ProcedureName, _updateConnection.SQLCommand);
        //            // End TT#3056 - JSmith - Header VIRTUAL_LOCK issue
        //        }
        //        catch (Exception error)
        //        {
        //            string message = error.ToString();
        //            throw;
        //        }
        //        throw new Exception("Application error in database access");  //  Exception to appease compiler;  code should never get here	
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}

        /// <summary>
        /// Executes a stored procedure that performs a command against the database
        /// </summary>
        /// <param name="SQLStatement"></param>
        /// <param name="InputParameters"></param>
        /// <returns>The number of rows affected by the statement</returns>
        public void ExecuteStoredProcedureForMaintenance(string SQLStatement, MIDDbParameter[] InputParameters = null, int commandTimeout = -1)
        {
            try
            {
                object[] args = new object[] { SQLStatement, InputParameters, commandTimeout };
                _myDatabaseMethodDelegate = new DatabaseMethodDelegate(myExecuteStoredProcedureForMaintenance);
                Command command = new Command(_myDatabaseMethodDelegate, args, SQLStatement);
                _updateCommands.Add(command);
                object returnObject = ProcessUpdateCommand(command);
                return;// Convert.ToInt32(returnObject, CultureInfo.CurrentCulture);
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// Executes a stored procedure that updates data in table
        /// </summary>
        /// <param name="aArgs"></param>
        /// <returns>The number of rows affected by the statement</returns>
        private object myExecuteStoredProcedureForMaintenance(object[] aArgs)
        {
            try
            {
                string SQLStatement = Convert.ToString(aArgs[0], CultureInfo.CurrentCulture);
                MIDDbParameter[] InputParameters = (MIDDbParameter[])aArgs[1];
                int commandTimeout = Convert.ToInt32(aArgs[2], CultureInfo.CurrentCulture);
                try
                {
                    //if (_updateConnection == null)
                    //{
                    //    OpenUpdateConnection();
                    //}


                    _updateConnection.SQLCommand.Parameters.Clear();
                    _updateConnection.SQLCommand.CommandType = CommandType.StoredProcedure;
                    _updateConnection.SQLCommand.CommandText = SQLStatement;
                    if (commandTimeout == -1)
                    {
                        if (_commandTimeout != _updateConnection.SQLCommand.CommandTimeout)
                        {
                            _updateConnection.SQLCommand.CommandTimeout = _commandTimeout;
                        }
                    }
                    else
                    {
                        if (_updateConnection.SQLCommand.CommandTimeout != commandTimeout)
                        {
                            _updateConnection.SQLCommand.CommandTimeout = commandTimeout;
                        }
                    }

                    SqlCommand cd = _updateConnection.SQLCommand;
                    AddInputParametersToSQLCommand(InputParameters, ref cd);

                    SQLMonitorList.SQLMonitorEntry se = null;
                    SQLMonitorList.BeforeExecution(ref se, SQLStatement, InputParameters);
                    int result = _updateConnection.SQLCommand.ExecuteNonQuery();
                    SQLMonitorList.AfterExecution(se);
                    return result;

                }
                catch (SqlException sql_error)
                {
                    CloseDatabaseUpdateConnection();
                    _databaseExceptionHandler.HandleDatabaseException(sql_error, SQLStatement, _updateConnection.SQLCommand);

                }
                catch (Exception error)
                {
                    string message = error.ToString();
                    throw;
                }
                throw new Exception("Application error in database access");  //  Exception to appease compiler;  code should never get here		
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Executes a stored procedure that inserts data in a table - returns number of rows inserted
        /// </summary>
        /// <param name="ProcedureName"></param>
        /// <param name="InputParameters"></param>
        /// <param name="OutputParameters"></param>
        /// <returns>int</returns>
        public int ExecuteStoredProcedureForInsert(string ProcedureName, MIDDbParameter[] InputParameters, MIDDbParameter[] OutputParameters = null)
        {
            try
            {
                object[] args = new object[] { ProcedureName, InputParameters, OutputParameters };
                _myDatabaseMethodDelegate = new DatabaseMethodDelegate(myExecuteStoredProcedureForInsert);
                Command command = new Command(_myDatabaseMethodDelegate, args, ProcedureName);
                _updateCommands.Add(command);
                object returnObject = ProcessUpdateCommand(command);
                return Convert.ToInt32(returnObject, CultureInfo.CurrentCulture);
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// Executes a stored procedure that inserts data in a table - returns number of rows inserted
        /// </summary>
        /// <param name="aArgs"></param>
        /// <returns>int</returns>
        private object myExecuteStoredProcedureForInsert(object[] aArgs)
        {
            try
            {
                string ProcedureName = Convert.ToString(aArgs[0], CultureInfo.CurrentCulture);
                MIDDbParameter[] InputParameters = (MIDDbParameter[])aArgs[1];
                MIDDbParameter[] OutputParameters = (MIDDbParameter[])aArgs[2];
                try
                {
                    
                    _updateConnection.SQLCommand.Parameters.Clear();
                    _updateConnection.SQLCommand.CommandType = CommandType.StoredProcedure;
                    _updateConnection.SQLCommand.CommandText = ProcedureName;
                    if (_commandTimeout != _updateConnection.SQLCommand.CommandTimeout)
                    {
                        _updateConnection.SQLCommand.CommandTimeout = _commandTimeout;
                    }

                    SqlCommand cd = _updateConnection.SQLCommand;
                    AddInputParametersToSQLCommand(InputParameters, ref cd);
                    AddOutputParametersToSQLCommand(OutputParameters, ref cd);


                    SQLMonitorList.SQLMonitorEntry se = null;
                    SQLMonitorList.BeforeExecution(ref se, ProcedureName, InputParameters);

                    int rowsInserted = 0;
                    object obj = _updateConnection.SQLCommand.ExecuteScalar();
                    if (obj != null)
                    {
                        rowsInserted = (int)obj;
                    }
                    SQLMonitorList.AfterExecution(se);
                    if (OutputParameters != null)
                    {
                        SetValuesOnOutputParamters(cd, ref OutputParameters);
                    }

                    return rowsInserted;
                }
                catch (SqlException sql_error)
                {
                    // Begin TT#3056 - JSmith - Header VIRTUAL_LOCK issue
                    CloseDatabaseUpdateConnection();
                    //HandleDatabaseUpdateException(sql_error, ProcedureName);
                    _databaseExceptionHandler.HandleDatabaseException(sql_error, ProcedureName, _updateConnection.SQLCommand);
                    // End TT#3056 - JSmith - Header VIRTUAL_LOCK issue
                }
                catch (Exception error)
                {
                    string message = error.ToString();
                    throw;
                }
                throw new Exception("Application error in database access");  //  Exception to appease compiler;  code should never get here	
            }
            catch
            {
                throw;
            }
        }

        

        /// <summary>
        /// Executes a stored procedure that inserts data in a table - returns RID from output parameter
        /// </summary>
        /// <param name="ProcedureName"></param>
        /// <param name="InputParameters"></param>
        /// <param name="OutputParameters"></param>
        /// <returns>int</returns>
        public int ExecuteStoredProcedureForInsertAndReturnRID(string ProcedureName, MIDDbParameter[] InputParameters, MIDDbParameter[] OutputParameters = null)
        {
            try
            {
                object[] args = new object[] { ProcedureName, InputParameters, OutputParameters };
                _myDatabaseMethodDelegate = new DatabaseMethodDelegate(myExecuteStoredProcedureForInsertAndReturnRID);
                Command command = new Command(_myDatabaseMethodDelegate, args, ProcedureName);
                _updateCommands.Add(command);
                object returnObject = ProcessUpdateCommand(command);
                return Convert.ToInt32(returnObject, CultureInfo.CurrentCulture);
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// Executes a stored procedure that inserts data in a table - returns RID from output parameter 
        /// </summary>
        /// <param name="aArgs"></param>
        /// <returns>int</returns>
        private object myExecuteStoredProcedureForInsertAndReturnRID(object[] aArgs)
        {
            try
            {
                string ProcedureName = Convert.ToString(aArgs[0], CultureInfo.CurrentCulture);
                MIDDbParameter[] InputParameters = (MIDDbParameter[])aArgs[1];
                MIDDbParameter[] OutputParameters = (MIDDbParameter[])aArgs[2];
                try
                {

                    _updateConnection.SQLCommand.Parameters.Clear();
                    _updateConnection.SQLCommand.CommandType = CommandType.StoredProcedure;
                    _updateConnection.SQLCommand.CommandText = ProcedureName;
                    if (_commandTimeout != _updateConnection.SQLCommand.CommandTimeout)
                    {
                        _updateConnection.SQLCommand.CommandTimeout = _commandTimeout;
                    }

                    SqlCommand cd = _updateConnection.SQLCommand;
                    AddInputParametersToSQLCommand(InputParameters, ref cd);
                    AddOutputParametersToSQLCommand(OutputParameters, ref cd);


                    SQLMonitorList.SQLMonitorEntry se = null;
                    SQLMonitorList.BeforeExecution(ref se, ProcedureName, InputParameters);

                    _updateConnection.SQLCommand.ExecuteNonQuery();
                    SQLMonitorList.AfterExecution(se);
                    int RID = -1;
                    if (OutputParameters != null)
                    {
                        foreach (SqlParameter param in _updateConnection.SQLCommand.Parameters)
                        {
                            if (param.Direction == ParameterDirection.Output || param.Direction == ParameterDirection.InputOutput)
                            {
                                RID = (int)param.Value;
                            }
                        }
                        SetValuesOnOutputParamters(cd, ref OutputParameters);
                    }
              
                    return RID;
                }
                catch (SqlException sql_error)
                {
                    // Begin TT#3056 - JSmith - Header VIRTUAL_LOCK issue
                    CloseDatabaseUpdateConnection();
                    //HandleDatabaseUpdateException(sql_error, ProcedureName);
                    _databaseExceptionHandler.HandleDatabaseException(sql_error, ProcedureName, _updateConnection.SQLCommand);
                    // End TT#3056 - JSmith - Header VIRTUAL_LOCK issue
                }
                catch (Exception error)
                {
                    string message = error.ToString();
                    throw;
                }
                throw new Exception("Application error in database access");  //  Exception to appease compiler;  code should never get here	
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Executes a stored procedure that deletes rows in a table.
        /// </summary>
        /// <param name="SQLStatement"></param>
        /// <returns>The number of rows affected by the statement</returns>
        public int ExecuteStoredProcedureForDelete(string ProcedureName, MIDDbParameter[] InputParameters, MIDDbParameter[] OutputParameters=null)
        {
            try
            {
                object[] args = new object[] { ProcedureName, InputParameters, OutputParameters };
                _myDatabaseMethodDelegate = new DatabaseMethodDelegate(myExecuteStoredProcedureForDelete);
                Command command = new Command(myExecuteStoredProcedureForDelete, args, ProcedureName);
                _updateCommands.Add(command);
                object returnObject = ProcessUpdateCommand(command);
                return Convert.ToInt32(returnObject, CultureInfo.CurrentCulture);
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// Executes a stored procedure that deletes rows in a table.
        /// </summary>
        /// <param name="aArgs"></param>
        /// <returns>The number of rows affected by the statement</returns>
        private object myExecuteStoredProcedureForDelete(object[] aArgs)
        {
            try
            {
                string ProcedureName = Convert.ToString(aArgs[0], CultureInfo.CurrentCulture);
                MIDDbParameter[] InputParameters = (MIDDbParameter[])aArgs[1];
                MIDDbParameter[] OutputParameters = (MIDDbParameter[])aArgs[2];
                try
                {
                    _updateConnection.SQLCommand.Parameters.Clear();
                    _updateConnection.SQLCommand.CommandType = CommandType.StoredProcedure;
                    _updateConnection.SQLCommand.CommandText = ProcedureName;
                    if (_commandTimeout != _updateConnection.SQLCommand.CommandTimeout)
                    {
                        _updateConnection.SQLCommand.CommandTimeout = _commandTimeout;
                    }

                    SqlCommand cd = _updateConnection.SQLCommand;
                    AddInputParametersToSQLCommand(InputParameters, ref cd);
                    AddOutputParametersToSQLCommand(OutputParameters, ref cd);

                    SQLMonitorList.SQLMonitorEntry se = null;
                    SQLMonitorList.BeforeExecution(ref se, ProcedureName, InputParameters);

                    int rowsDeleted = 0;
                    object obj = _updateConnection.SQLCommand.ExecuteScalar();
                    if (obj != null)
                    {
                        rowsDeleted = (int)obj;
                    }
                    SQLMonitorList.AfterExecution(se);
                    SetValuesOnOutputParamters(_updateConnection.SQLCommand, ref OutputParameters);
                  
                    return rowsDeleted;

                }
                catch (SqlException sql_error)
                {
                    CloseDatabaseUpdateConnection();
                    _databaseExceptionHandler.HandleDatabaseException(sql_error, ProcedureName, _updateConnection.SQLCommand);
                }
                catch (Exception error)
                {
                    string message = error.ToString();
                    throw;
                }
                throw new Exception("Application error in database access");  //  Exception to appease compiler;  code should never get here	
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Executes a stored procedure that returns a count of rows
        /// </summary>
        /// <param name="SQLCommand"></param>
        /// <returns></returns>
        public int ExecuteStoredProcedureForRecordCount(string SQLCommand, MIDDbParameter[] InputParameters)
        {
            try
            {
                object[] args = new object[] { SQLCommand, InputParameters };
                _myDatabaseMethodDelegate = new DatabaseMethodDelegate(myExecuteStoredProcedureForRecordCount);
                object returnObject = ProcessReadCommand(new Command(_myDatabaseMethodDelegate, args, SQLCommand));
                return Convert.ToInt32(returnObject, CultureInfo.CurrentCulture);
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// Executes a stored procedure that returns a count of rows
        /// </summary>
        /// <param name="aArgs"></param>
        /// <returns></returns>
        private object myExecuteStoredProcedureForRecordCount(object[] aArgs)
        {
            string lastCommand = string.Empty;
            try
            {
                string SQLCommand = Convert.ToString(aArgs[0], CultureInfo.CurrentCulture);
                MIDDbParameter[] InputParameters = (MIDDbParameter[])aArgs[1];
                SqlCommand cd = null;
                try
                {
                    int recCount = 0;
                    SqlDataReader myReader;
                    cd = new SqlCommand(SQLCommand);

                    cd.Parameters.Clear();

                    AddInputParametersToSQLCommand(InputParameters, ref cd);

                    SQLMonitorList.SQLMonitorEntry se = null;
                    SQLMonitorList.BeforeExecution(ref se, SQLCommand, InputParameters);

                    lastCommand = "CreateConnection with " + DBConnectionString;  // TT#2131-MD - JSmith - Halo Integration
                    cd.Connection = CreateConnection(DBConnectionString);  // TT#2131-MD - JSmith - Halo Integration
                    OpenConnection(cd);

                    lastCommand = SQLCommand;
                    cd.CommandType = CommandType.StoredProcedure;
                    if (_commandTimeout != cd.CommandTimeout)
                    {
                        cd.CommandTimeout = _commandTimeout;
                    }

                    myReader = cd.ExecuteReader();
                    if (myReader.Read())
                    {
                        recCount = (int)myReader["MyCount"];
                    }
                    myReader.Close();
                    SQLMonitorList.AfterExecution(se);
                    return recCount;
                }
                catch (SqlException sql_error)
                {
                    _databaseExceptionHandler.HandleDatabaseException(sql_error, lastCommand, cd);
                }
                catch (Exception error)
                {
                    string message = error.ToString();
                    throw;
                }
                finally
                {
                    if (cd != null && cd.Connection != null && cd.Connection.State == ConnectionState.Open)
                    {
                        // swallow error if close fails because connection is already closed
                        try
                        {
                            cd.Connection.Close();
                        }
                        catch
                        {
                        }
                        cd.Dispose();
                    }
                }
                throw new Exception("Application error in database access");  //  Exception to appease compiler;  code should never get here	
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="SqlStatement"></param>
        /// <param name="InputParameters"></param>
        /// <returns></returns>
        public object ExecuteStoredProcedureForScalarValue(string ProcedureName, MIDDbParameter[] InputParameters)
        {
            try
            {
                object[] args = new object[] { ProcedureName, InputParameters };
                _myDatabaseMethodDelegate = new DatabaseMethodDelegate(myExecuteStoredProcedureForScalarValue);
                object returnObject = ProcessReadCommand(new Command(_myDatabaseMethodDelegate, args, ProcedureName));
                return returnObject;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// ExecuteScalar is a public method in class DatabaseAccess used to return a single value
        /// </summary>
        /// <remarks>
        /// Rather than cache this single-row and column resultset in a DataSet object or open a SqlDataReader, use the ExecuteScalar method to return the value as an object. You can then convert it to the appropriate type.
        /// </remarks>
        /// <param name='name'>A string which contains a SQL statment that returns one value (one column of one row)</param>
        /// <returns>Return value of type object.</returns>
        private object myExecuteStoredProcedureForScalarValue(object[] aArgs)
        {
            string lastCommand = string.Empty;
            SqlCommand cd = null;
            try
            {
                string ProcedureName = Convert.ToString(aArgs[0], CultureInfo.CurrentCulture);
                MIDDbParameter[] InputParameters = (MIDDbParameter[])aArgs[1];
                //SqlCommand cd = new SqlCommand();
                cd = new SqlCommand();

                try
                {
                    cd.Parameters.Clear();

                    AddInputParametersToSQLCommand(InputParameters, ref cd);

                    lastCommand = "CreateConnection with " + DBConnectionString;  // TT#2131-MD - JSmith - Halo Integration
                    cd.Connection = CreateConnection(DBConnectionString);  // TT#2131-MD - JSmith - Halo Integration


                    SQLMonitorList.SQLMonitorEntry se = null;
                    SQLMonitorList.BeforeExecution(ref se, ProcedureName, InputParameters);
                    OpenConnection(cd);

                    lastCommand = ProcedureName;
                    cd.CommandType = CommandType.StoredProcedure;
                    cd.CommandText = ProcedureName;
                    if (_commandTimeout != cd.CommandTimeout)
                    {
                        cd.CommandTimeout = _commandTimeout;
                    }
                    object result = cd.ExecuteScalar();
                    SQLMonitorList.AfterExecution(se);
                    return result;
                }
                catch (SqlException sql_error)
                {
                    _databaseExceptionHandler.HandleDatabaseException(sql_error, lastCommand, cd);
                }
                catch (Exception error)
                {
                    string message = error.ToString();
                    throw;
                }
                finally
                {
                    if (cd != null && cd.Connection != null && cd.Connection.State == ConnectionState.Open)
                    {
                        // swallow error if close fails because connection is already closed
                        try
                        {
                            cd.Connection.Close();
                        }
                        catch
                        {
                        }
                        cd.Dispose();
                    }
                }
                throw new Exception("Application error in database access");  //  Exception to appease compiler;  code should never get here	
            }
            catch
            {
                throw;
            }
        }

        //End TT#846-MD -jsobek -New Stored Procedures for Performance

		/// <summary>
		/// Executes stores procedures requiring input parameters. 
		/// </summary>
		/// <param name="ProcedureName"></param>
		/// <param name="InputParameters"></param>
		/// <param name="OutputParameters"></param>
		/// <returns>int</returns>
		public int ExecuteStoredProcedure(string ProcedureName, MIDDbParameter[] InputParameters, MIDDbParameter[] OutputParameters)
		{
			try
			{
				object[] args = new object[]{ProcedureName, InputParameters, OutputParameters};
				_myDatabaseMethodDelegate = new DatabaseMethodDelegate(myExecuteStoredProcedure);
				Command command = new Command(_myDatabaseMethodDelegate, args, ProcedureName);
				_updateCommands.Add(command);
				object returnObject = ProcessUpdateCommand(command);
				return Convert.ToInt32(returnObject, CultureInfo.CurrentCulture);
			}
			catch
			{
				throw;
			}
		}
		/// <summary>
		/// Executes stores procedures requiring input parameters. 
		/// </summary>
		/// <param name="aArgs"></param>
		/// <returns>int</returns>
		private object myExecuteStoredProcedure(object[] aArgs)
		{
			try
			{
				string ProcedureName = Convert.ToString(aArgs[0], CultureInfo.CurrentCulture);
				MIDDbParameter[] InputParameters = (MIDDbParameter[])aArgs[1];
				MIDDbParameter[] OutputParameters = (MIDDbParameter[])aArgs[2];
				try
				{
					int RID = -1;
					_updateConnection.SQLCommand.Parameters.Clear();
					_updateConnection.SQLCommand.CommandType = CommandType.StoredProcedure;
					_updateConnection.SQLCommand.CommandText = ProcedureName;
					if (_commandTimeout != _updateConnection.SQLCommand.CommandTimeout)
					{
						_updateConnection.SQLCommand.CommandTimeout = _commandTimeout;
					}

                    SqlCommand cd = _updateConnection.SQLCommand;
                    AddInputParametersToSQLCommand(InputParameters, ref cd);
                    //Begin TT#846-MD -jsobek -New Stored Procedures for Performance
                    AddOutputParametersToSQLCommand(OutputParameters, ref cd);
                    //if ( OutputParameters != null && OutputParameters.Length > 0 )
                    //{
                    //    for( int i = 0; i < OutputParameters.Length; i++ )
                    //    {
                    //        SqlParameter OutParam  =  new SqlParameter();
                    //        OutParam.Direction = ParameterDirection.Output;
                    //        OutParam.ParameterName = OutputParameters[i].ParameterName;
                    //        OutParam.Value = OutputParameters[i].Value;
                    //        switch (OutputParameters[i].DbType)
                    //        {
                    //            case eDbType.Bit:
                    //                OutParam.SqlDbType = SqlDbType.Bit;
                    //                break;
                    //            case eDbType.Char:
                    //                OutParam.SqlDbType = SqlDbType.Char;
                    //                break;
                    //            case eDbType.DateTime:
                    //                OutParam.SqlDbType = SqlDbType.DateTime;
                    //                break;
                    //            case eDbType.Float:
                    //                OutParam.SqlDbType = SqlDbType.Float;
                    //                break;
                    //            case eDbType.Int:
                    //                OutParam.SqlDbType = SqlDbType.Int;
                    //                break;
                    //            case eDbType.SmallDateTime:
                    //                OutParam.SqlDbType = SqlDbType.SmallDateTime;
                    //                break;
                    //            case eDbType.VarChar:
                    //                OutParam.SqlDbType = SqlDbType.VarChar;
                    //                break;
                    //            case eDbType.Image:
                    //                OutParam.SqlDbType = SqlDbType.Image;
                    //                break;
                    //            case eDbType.Text:
                    //                OutParam.SqlDbType = SqlDbType.Text;
                    //                break;
                    //            // begin TT#173  Provide database container for large data collections
                    //            case eDbType.smallint:
                    //                OutParam.SqlDbType = SqlDbType.SmallInt;
                    //                break;
                    //            case eDbType.tinyint:
                    //                OutParam.SqlDbType = SqlDbType.TinyInt;
                    //                break;
                    //            case eDbType.VarBinary:
                    //                OutParam.SqlDbType = SqlDbType.VarBinary;
                    //                break;
                    //            // end TT#173  Provide database container for large data collections
                    //            // begin TT#1185 - Verify ENQ before Update
                    //            case eDbType.Int64:
                    //                OutParam.SqlDbType = SqlDbType.BigInt;
                    //                break;
                    //            // end TT#1185 - Verify ENQ before Update
                    //            default:
                    //                break;
                    //        }
                    //        _updateConnection.SQLCommand.Parameters.Add( OutParam );
                    //    }
                    //}
                    //End TT#846-MD -jsobek -New Stored Procedures for Performance

                    SQLMonitorList.SQLMonitorEntry se = null;
                    SQLMonitorList.BeforeExecution(ref se, ProcedureName, InputParameters);

					_updateConnection.SQLCommand.ExecuteNonQuery();
                    SQLMonitorList.AfterExecution(se);
					foreach( SqlParameter param in _updateConnection.SQLCommand.Parameters )
						if ( param.Direction == ParameterDirection.Output || param.Direction == ParameterDirection.InputOutput)
						{
							RID = (int)param.Value;
						}

					return RID;
				}
				catch ( SqlException sql_error )
				{
                    // Begin TT#3056 - JSmith - Header VIRTUAL_LOCK issue
                    CloseDatabaseUpdateConnection();
                    //HandleDatabaseUpdateException(sql_error, ProcedureName);
                    _databaseExceptionHandler.HandleDatabaseException(sql_error, ProcedureName, _updateConnection.SQLCommand);
                    // End TT#3056 - JSmith - Header VIRTUAL_LOCK issue
				}
				catch ( Exception error )
				{
					string message = error.ToString();
					throw;
				}
				throw new Exception("Application error in database access");  //  Exception to appease compiler;  code should never get here	
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Executes stored procedure requiring input parameters.
		/// </summary>
		/// <param name="ProcedureName"></param>
		/// <param name="InputParameters"></param>
		/// <returns>MIDDbParameter[]</returns>
		public MIDDbParameter[] ExecuteStoredProcedure(string ProcedureName, MIDDbParameter[] InputParameters )
		{
			try
			{
				object[] args = new object[]{ProcedureName, InputParameters};
				_myDatabaseMethodDelegate = new DatabaseMethodDelegate(myExecuteStoredProcedure2);
				Command command = new Command(_myDatabaseMethodDelegate, args, ProcedureName);
				_updateCommands.Add(command);
				object returnObject = ProcessUpdateCommand(command);
				return (MIDDbParameter[])returnObject;
			}
			catch
			{
				throw;
			}
		}
		/// <summary>
		/// Executes stored procedure requiring input parameters.
		/// </summary>
		/// <param name="aArgs"></param>
		/// <returns>MIDDbParameter[]</returns>
		private object myExecuteStoredProcedure2(object[] aArgs )
		{
			try
			{
				string ProcedureName = Convert.ToString(aArgs[0], CultureInfo.CurrentCulture);
				MIDDbParameter[] InputParameters = (MIDDbParameter[])aArgs[1];
				try
				{
					_updateConnection.SQLCommand.Parameters.Clear();
					_updateConnection.SQLCommand.CommandType = CommandType.StoredProcedure;
					_updateConnection.SQLCommand.CommandText = ProcedureName;
					if (_commandTimeout != _updateConnection.SQLCommand.CommandTimeout)
					{
						_updateConnection.SQLCommand.CommandTimeout = _commandTimeout;
					}

                    SqlCommand cd = _updateConnection.SQLCommand;
                    AddInputParametersToSQLCommand(InputParameters, ref cd);

                    SQLMonitorList.SQLMonitorEntry se = null;
                    SQLMonitorList.BeforeExecution(ref se, ProcedureName, InputParameters);
					_updateConnection.SQLCommand.ExecuteNonQuery();
                    SQLMonitorList.AfterExecution(se);
					return null;
				}
				catch ( SqlException sql_error )
				{
                    // Begin TT#3056 - JSmith - Header VIRTUAL_LOCK issue
                    CloseDatabaseUpdateConnection();
                    //HandleDatabaseUpdateException(sql_error, ProcedureName);
                    _databaseExceptionHandler.HandleDatabaseException(sql_error, ProcedureName, _updateConnection.SQLCommand);
                    // End TT#3056 - JSmith - Header VIRTUAL_LOCK issue
				}
				catch ( Exception error )
				{
					string message = error.ToString();
					throw;
				}
				throw new Exception("Application error in database access");  //  Exception to appease compiler;  code should never get here	
			}
			catch
			{
				throw;
			}
		}

        ///// <summary>
        ///// Executes stored procedure requiring no parameters.
        ///// </summary>
        ///// <param name="ProcedureName"></param>
        ///// <returns></returns>
        //public MIDDbParameter[] ExecuteStoredProcedure(string ProcedureName )
        //{
        //    try
        //    {
        //        object[] args = new object[]{ProcedureName};
        //        _myDatabaseMethodDelegate = new DatabaseMethodDelegate(myExecuteStoredProcedure3);
        //        Command command = new Command(_myDatabaseMethodDelegate, args, ProcedureName);
        //        _updateCommands.Add(command);
        //        object returnObject = ProcessUpdateCommand(command);
        //        return (MIDDbParameter[])returnObject;
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}
        ///// <summary>
        ///// Executes stored procedure requiring no parameters.
        ///// </summary>
        ///// <param name="ProcedureName"></param>
        ///// <returns></returns>
        //private object myExecuteStoredProcedure3(object[] aArgs )
        //{
        //    try
        //    {
        //        string ProcedureName = Convert.ToString(aArgs[0], CultureInfo.CurrentCulture);
        //        try
        //        {
        //            _updateConnection.SQLCommand.Parameters.Clear();
        //            _updateConnection.SQLCommand.CommandType = CommandType.StoredProcedure;
        //            _updateConnection.SQLCommand.CommandText = ProcedureName;
        //            if (_commandTimeout != _updateConnection.SQLCommand.CommandTimeout)
        //            {
        //                _updateConnection.SQLCommand.CommandTimeout = _commandTimeout;
        //            }
        //            SQLMonitorList.SQLMonitorEntry se = null;
        //            SQLMonitorList.BeforeExecution(ref se, ProcedureName);
        //            _updateConnection.SQLCommand.ExecuteNonQuery();
        //            SQLMonitorList.AfterExecution(se);
        //            return null;
        //        }
        //        catch ( SqlException sql_error )
        //        {
        //            // Begin TT#3056 - JSmith - Header VIRTUAL_LOCK issue
        //            CloseDatabaseUpdateConnection();
        //            //HandleDatabaseUpdateException(sql_error, ProcedureName);
        //            _databaseExceptionHandler.HandleDatabaseException(sql_error, ProcedureName, _updateConnection.SQLCommand);
        //            // End TT#3056 - JSmith - Header VIRTUAL_LOCK issue
        //        }
        //        catch ( Exception error )
        //        {
        //            string message = error.ToString();
        //            throw;
        //        }
        //        throw new Exception("Application error in database access");  //  Exception to appease compiler;  code should never get here	
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}

        ///// <summary>
        ///// Executes stored procedure requiring no parameters.
        ///// </summary>
        ///// <param name="ProcedureName"></param>
        ///// <param name="commandTimeout">The wait time in seconds before terminating the command and generating an error</param>
        ///// <returns></returns>
        //public MIDDbParameter[] ExecuteStoredProcedure(string ProcedureName, int commandTimeout )
        //{
        //    try
        //    {
        //        object[] args = new object[]{ProcedureName, commandTimeout};
        //        _myDatabaseMethodDelegate = new DatabaseMethodDelegate(myExecuteStoredProcedure4);
        //        Command command = new Command(_myDatabaseMethodDelegate, args, ProcedureName);
        //        _updateCommands.Add(command);
        //        object returnObject = ProcessUpdateCommand(command);
        //        return (MIDDbParameter[])returnObject;
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}
        ///// <summary>
        ///// Executes stored procedure requiring no parameters.
        ///// </summary>
        ///// <param name="aArgs"></param>
        ///// <returns></returns>
        //private object myExecuteStoredProcedure4(object[] aArgs )
        //{
        //    try
        //    {
        //        string ProcedureName = Convert.ToString(aArgs[0], CultureInfo.CurrentCulture);
        //        int commandTimeout = Convert.ToInt32(aArgs[1], CultureInfo.CurrentCulture);
        //        try
        //        {
        //            _updateConnection.SQLCommand.Parameters.Clear();
        //            _updateConnection.SQLCommand.CommandType = CommandType.StoredProcedure;
        //            _updateConnection.SQLCommand.CommandText = ProcedureName;
        //            if (_updateConnection.SQLCommand.CommandTimeout != commandTimeout)
        //            {
        //                _updateConnection.SQLCommand.CommandTimeout = commandTimeout;
        //            }
        //            SQLMonitorList.SQLMonitorEntry se = null;
        //            SQLMonitorList.BeforeExecution(ref se, ProcedureName);
        //            _updateConnection.SQLCommand.ExecuteNonQuery();
        //            SQLMonitorList.AfterExecution(se);
        //            return null;
        //        }
        //        catch ( SqlException sql_error )
        //        {
        //            // Begin TT#3056 - JSmith - Header VIRTUAL_LOCK issue
        //            CloseDatabaseUpdateConnection();
        //            //HandleDatabaseUpdateException(sql_error, ProcedureName);
        //            _databaseExceptionHandler.HandleDatabaseException(sql_error, ProcedureName, _updateConnection.SQLCommand);
        //            // End TT#3056 - JSmith - Header VIRTUAL_LOCK issue
        //        }
        //        catch ( Exception error )
        //        {
        //            string message = error.ToString();
        //            throw;
        //        }
        //        throw new Exception("Application error in database access");  //  Exception to appease compiler;  code should never get here	
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}

        //public void OpenReadConnection()
        //{
        //    try
        //    {
        //        _myDatabaseMethodDelegate = new DatabaseMethodDelegate(myOpenReadConnection);
        //        Command command = new Command(_myDatabaseMethodDelegate, null, "DatabaseAccess Open read connection");
        //        object returnObject = ProcessReadCommand(command);
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}
        //private object myOpenReadConnection(object[] aArgs)
        //{
        //    string lastCommand = string.Empty;
        //    try
        //    {
        //        lastCommand = "DatabaseAccess Open read CreateConnection with " + ConnectionString;
        //        _readConnection = CreateConnection(ConnectionString);
        //        OpenConnection(_readConnection);
        //    }
        //    catch ( SqlException sql_error )
        //    {
        //        // Begin TT#3056 - JSmith - Header VIRTUAL_LOCK issue
        //        //HandleDatabaseReadException(sql_error, lastCommand, null);
        //        // Begin TT#3302 - JSmith - Size Curves Failures
        //        //_databaseExceptionHandler.HandleDatabaseException(sql_error, lastCommand, null);
        //        _databaseExceptionHandler.HandleDatabaseException(sql_error, lastCommand, _readConnection.CreateCommand());
        //        // End TT#3302 - JSmith - Size Curves Failures
        //        // End TT#3056 - JSmith - Header VIRTUAL_LOCK issue
        //    }
        //    catch ( Exception error )
        //    {
        //        string message = error.ToString();
        //        throw;
        //    }
        //    return null;
        //}

        ///// <summary>
        ///// Executes Update stored procedure. 
        ///// </summary>
        ///// <param name="ProcedureName"></param>
        ///// <param name="InputParameters"></param>
        ///// <param name="OutputParameters"></param>
        ///// <returns>int</returns>
        //public int UpdateStoredProcedure(string ProcedureName, MIDDbParameter[] InputParameters, MIDDbParameter[] OutputParameters)
        //{
        //    try
        //    {
        //        object[] args = new object[]{ProcedureName, InputParameters, OutputParameters};
        //        _myDatabaseMethodDelegate = new DatabaseMethodDelegate(myUpdateStoredProcedure);
        //        Command command = new Command(_myDatabaseMethodDelegate, args, ProcedureName);
        //        _updateCommands.Add(command);
        //        object returnObject = ProcessUpdateCommand(command);
        //        return Convert.ToInt32(returnObject, CultureInfo.CurrentCulture);
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}
        ///// <summary>
        ///// Executes Update stored procedure. 
        ///// </summary>
        ///// <param name="aArgs"></param>
        ///// <returns>object</returns>
        //private object myUpdateStoredProcedure(object[] aArgs)
        //{
        //    try
        //    {
        //        string ProcedureName = Convert.ToString(aArgs[0], CultureInfo.CurrentCulture);
        //        MIDDbParameter[] InputParameters = (MIDDbParameter[])aArgs[1];
        //        MIDDbParameter[] OutputParameters = (MIDDbParameter[])aArgs[2];
        //        try
        //        {
        //            _updateConnection.SQLCommand.Parameters.Clear();
        //            _updateConnection.SQLCommand.CommandType = CommandType.StoredProcedure;
        //            _updateConnection.SQLCommand.CommandText = ProcedureName;
        //            if (_commandTimeout != _updateConnection.SQLCommand.CommandTimeout)
        //            {
        //                _updateConnection.SQLCommand.CommandTimeout = _commandTimeout;
        //            }

        //            if ( InputParameters != null && InputParameters.Length > 0 )
        //            {
        //                for( int i = 0; i < InputParameters.Length; i++ )
        //                {
        //                    SqlParameter InParam  =  new SqlParameter();
        //                    InParam.Direction = ParameterDirection.Input;
        //                    InParam.ParameterName = InputParameters[i].ParameterName;
        //                    switch (InputParameters[i].DbType)
        //                    {
        //                        case eDbType.Bit:
        //                            InParam.SqlDbType = SqlDbType.Bit;
        //                            break;
        //                        case eDbType.Char:
        //                            InParam.SqlDbType = SqlDbType.Char;
        //                            break;
        //                        case eDbType.DateTime:
        //                            InParam.SqlDbType = SqlDbType.DateTime;
        //                            if (InParam.Value != DBNull.Value)
        //                            {
        //                                if (Convert.ToDateTime(InParam.Value, CultureInfo.CurrentUICulture) == Include.UndefinedDate)
        //                                {
        //                                    InParam.Value = DBNull.Value;
        //                                }
        //                                InParam.Size = 17;
        //                            }
        //                            break;
        //                        case eDbType.Float:
        //                            InParam.SqlDbType = SqlDbType.Float;
        //                            break;
        //                        case eDbType.Decimal:
        //                            InParam.SqlDbType = SqlDbType.Decimal;
        //                            break;
        //                        case eDbType.Int:
        //                            InParam.SqlDbType = SqlDbType.Int;
        //                            break;
        //                        case eDbType.SmallDateTime:
        //                            InParam.SqlDbType = SqlDbType.SmallDateTime;
        //                            break;
        //                        case eDbType.VarChar:
        //                            InParam.SqlDbType = SqlDbType.VarChar;
        //                            break;
        //                        case eDbType.Image:
        //                            InParam.SqlDbType = SqlDbType.Image;
        //                            break;
        //                        case eDbType.Text:
        //                            InParam.SqlDbType = SqlDbType.Text;
        //                            break;
        //                            // begin TT#173  Provide database container for large data collections
        //                        case eDbType.smallint:
        //                            InParam.SqlDbType = SqlDbType.SmallInt;
        //                            break;
        //                        case eDbType.tinyint:
        //                            InParam.SqlDbType = SqlDbType.TinyInt;
        //                            break;
        //                        case eDbType.VarBinary:
        //                            InParam.SqlDbType = SqlDbType.VarBinary;
        //                            InParam.Size = ((byte[])InputParameters[i].Value).Length;
        //                            break;
        //                            // end TT#173  Provide database container for large data collections
        //                        // begin TT#1185 - Verify ENQ before Update
        //                        case eDbType.Int64:
        //                            InParam.SqlDbType = SqlDbType.BigInt;
        //                            break;
        //                        // end TT#1185 - Verify ENQ before Update
        //                        //Begin TT#827-MD -jsobek -Allocation Reviews Performance
        //                        case eDbType.Structured:
        //                            InParam.SqlDbType = SqlDbType.Structured;
        //                            InParam.TypeName = InputParameters[i].TypeName;
        //                            break;
        //                        //End TT#827-MD -jsobek -Allocation Reviews Performance
        //                        default:
        //                            break;
        //                    }
        //                    if (InputParameters[i].Value == null)
        //                    {
        //                        InParam.Value = DBNull.Value;
        //                    }
        //                    else if (InputParameters[i].DbType == eDbType.Char) 
        //                    {
        //                        InParam.Value = Convert.ToString(InputParameters[i].Value, CultureInfo.CurrentUICulture) ;
        //                        InParam.Size = 1;
        //                    }
        //                    else if (InputParameters[i].DbType == eDbType.Text)
        //                    {
        //                        InParam.Value = Convert.ToString(InputParameters[i].Value, CultureInfo.CurrentUICulture) ;
        //                        InParam.Size = ((System.Text.StringBuilder)InputParameters[i].Value).Length;
        //                    }
        //                    else
        //                    {
        //                        InParam.Value = InputParameters[i].Value;
        //                    }

        //                    _updateConnection.SQLCommand.Parameters.Add( InParam );
        //                }
        //            }
                 
        //            SqlParameter OutParam;
        //            if ( OutputParameters != null && OutputParameters.Length > 0 )
        //            {
        //                for( int i = 0; i < OutputParameters.Length; i++ )
        //                {
        //                    if (OutputParameters[i].ParameterName != "@ReturnCode")
        //                    {
        //                        OutParam  =  new SqlParameter();
        //                        switch (OutputParameters[i].Direction)
        //                        {
        //                            case (eParameterDirection.Output):
        //                            {
        //                                OutParam.Direction = ParameterDirection.Output;
        //                                break;
        //                            }
        //                            case (eParameterDirection.InputOutput):
        //                            {
        //                                OutParam.Direction = ParameterDirection.InputOutput;
        //                                break;
        //                            }
        //                            default:
        //                            {
        //                                OutParam.Direction = ParameterDirection.ReturnValue;
        //                                break;
        //                            }
        //                        }
        //                        OutParam.ParameterName = OutputParameters[i].ParameterName;

        //                        if (OutputParameters[i].Value == null)
        //                        {
        //                            OutParam.Value = DBNull.Value;
        //                        }
        //                        else if (OutputParameters[i].DbType == eDbType.Char) 
        //                        {
        //                            OutParam.Value = Convert.ToString(OutputParameters[i].Value, CultureInfo.CurrentUICulture) ;
        //                            OutParam.Size = 1;
        //                        }
        //                        else if (OutputParameters[i].DbType == eDbType.Text)
        //                        {
        //                            OutParam.Value = Convert.ToString(OutputParameters[i].Value, CultureInfo.CurrentUICulture) ;
        //                            OutParam.Size = ((System.Text.StringBuilder)OutputParameters[i].Value).Length;
        //                        }
        //                        else
        //                        {
        //                            OutParam.Value = OutputParameters[i].Value;
        //                        }
        //                        switch (OutputParameters[i].DbType)
        //                        {
        //                            case eDbType.Bit:
        //                                OutParam.SqlDbType = SqlDbType.Bit;
        //                                break;
        //                            case eDbType.Char:
        //                                OutParam.SqlDbType = SqlDbType.Char;
        //                                break;
        //                            case eDbType.DateTime:
        //                                OutParam.SqlDbType = SqlDbType.DateTime;
        //                                break;
        //                            case eDbType.Float:
        //                                OutParam.SqlDbType = SqlDbType.Float;
        //                                break;
        //                            case eDbType.Decimal:
        //                                OutParam.SqlDbType = SqlDbType.Decimal;
        //                                break;
        //                            case eDbType.Int:
        //                                OutParam.SqlDbType = SqlDbType.Int;
        //                                break;
        //                            case eDbType.SmallDateTime:
        //                                OutParam.SqlDbType = SqlDbType.SmallDateTime;
        //                                break;
        //                            case eDbType.VarChar:
        //                                OutParam.SqlDbType = SqlDbType.VarChar;
        //                                break;
        //                            case eDbType.Image:
        //                                OutParam.SqlDbType = SqlDbType.Image;
        //                                break;
        //                            case eDbType.Text:
        //                                OutParam.SqlDbType = SqlDbType.Text;
        //                                break;
        //                            // begin TT#173  Provide database container for large data collections
        //                            case eDbType.smallint:
        //                                OutParam.SqlDbType = SqlDbType.SmallInt;
        //                                break;
        //                            case eDbType.tinyint:
        //                                OutParam.SqlDbType = SqlDbType.TinyInt;
        //                                break;
        //                            case eDbType.VarBinary:
        //                                OutParam.SqlDbType = SqlDbType.VarBinary;
        //                                break;
        //                            // end TT#173  Provide database container for large data collections
        //                            // begin TT#1185 - Verify ENQ before Update
        //                            case eDbType.Int64:
        //                                OutParam.SqlDbType = SqlDbType.BigInt;
        //                                break;
        //                            // end TT#1185 - Verify ENQ before Update
        //                            default:
        //                                break;
        //                        }
        //                        _updateConnection.SQLCommand.Parameters.Add( OutParam );
        //                    }
        //                }
        //            }
        //            OutParam  =  new SqlParameter();
        //            OutParam.Direction = ParameterDirection.ReturnValue;
        //            OutParam.ParameterName = "@ReturnCode";
        //            OutParam.Value = 0;
        //            OutParam.SqlDbType = SqlDbType.Int;
        //            _updateConnection.SQLCommand.Parameters.Add(OutParam);

        //            SQLMonitorList.SQLMonitorEntry se = null;
        //            SQLMonitorList.BeforeExecution(ref se, ProcedureName);

        //            int rc = _updateConnection.SQLCommand.ExecuteNonQuery();
        //            SQLMonitorList.AfterExecution(se);
        //        }
        //        catch ( SqlException sql_error )
        //        {
        //            // Begin TT#3056 - JSmith - Header VIRTUAL_LOCK issue
        //            CloseDatabaseUpdateConnection();
        //            //HandleDatabaseUpdateException(sql_error, ProcedureName);
        //            _databaseExceptionHandler.HandleDatabaseException(sql_error, ProcedureName, _updateConnection.SQLCommand);
        //            // End TT#3056 - JSmith - Header VIRTUAL_LOCK issue
        //        }
        //        catch ( Exception error )
        //        {
        //            string message = error.ToString();
        //            throw;
        //        }
        //        finally
        //        {
        //        }
        //        object returnObj = OutputParmValue(_updateConnection.SQLCommand, "@ReturnCode");
        //        if (returnObj != null
        //            && returnObj != DBNull.Value)
        //        {
        //            return (int)returnObj;
        //        }
        //        return -1000;
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}
        ///// <summary>
        ///// Executes stores procedures requiring input parameters. 
        ///// </summary>
        ///// <param name="ProcedureName"></param>
        ///// <param name="InputParameters"></param>
        ///// <param name="OutputParameters"></param>
        ///// <returns>int</returns>
        //public void ReadOnlyStoredProcedure(string ProcedureName, MIDDbParameter[] InputParameters, MIDDbParameter[] OutputParameters)
        //{
        //    try
        //    {
        //        object[] args = new object[]{ProcedureName, InputParameters, OutputParameters};
        //        _myDatabaseMethodDelegate = new DatabaseMethodDelegate(myReadOnlyStoredProcedure);
        //        Command command = new Command(_myDatabaseMethodDelegate, args, ProcedureName);
        //        object returnObject = ProcessReadCommand(command);
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}
        ///// <summary>
        ///// Executes stores procedures requiring input parameters. 
        ///// </summary>
        ///// <param name="aArgs"></param>
        ///// <returns>int</returns>
        //private object myReadOnlyStoredProcedure(object[] aArgs)
        //{
        //    try
        //    {
        //        string ProcedureName = Convert.ToString(aArgs[0], CultureInfo.CurrentCulture);
        //        MIDDbParameter[] InputParameters = (MIDDbParameter[])aArgs[1];
        //        MIDDbParameter[] OutputParameters = (MIDDbParameter[])aArgs[2];
        //        try
        //        {
        //            _readCommand = _readConnection.CreateCommand();
	
        //            _readCommand.Parameters.Clear();
        //            _readCommand.CommandType = CommandType.StoredProcedure;
        //            _readCommand.CommandText = ProcedureName;
        //            if (_commandTimeout != _readCommand.CommandTimeout)
        //            {
        //                _readCommand.CommandTimeout = _commandTimeout;
        //            }

        //            //End TT#827-MD -jsobek -Allocation Reviews Performance
        //            //if (InputParameters != null && InputParameters.Length > 0)
        //            //{
        //            //    for (int i = 0; i < InputParameters.Length; i++)
        //            //    {
        //            //        SqlParameter InParam = new SqlParameter();
        //            //        InParam.Direction = ParameterDirection.Input;
        //            //        InParam.ParameterName = InputParameters[i].ParameterName;
        //            //        switch (InputParameters[i].DbType)
        //            //        {
        //            //            case eDbType.Bit:
        //            //                InParam.SqlDbType = SqlDbType.Bit;
        //            //                break;
        //            //            case eDbType.Char:
        //            //                InParam.SqlDbType = SqlDbType.Char;
        //            //                break;
        //            //            case eDbType.DateTime:
        //            //                InParam.SqlDbType = SqlDbType.DateTime;
        //            //                if (InParam.Value != DBNull.Value)
        //            //                {
        //            //                    if (Convert.ToDateTime(InParam.Value, CultureInfo.CurrentUICulture) == Include.UndefinedDate)
        //            //                    {
        //            //                        InParam.Value = DBNull.Value;
        //            //                    }
        //            //                    InParam.Size = 17;
        //            //                }
        //            //                break;
        //            //            case eDbType.Float:
        //            //                InParam.SqlDbType = SqlDbType.Float;
        //            //                break;
        //            //            case eDbType.Int:
        //            //                InParam.SqlDbType = SqlDbType.Int;
        //            //                break;
        //            //            case eDbType.SmallDateTime:
        //            //                InParam.SqlDbType = SqlDbType.SmallDateTime;
        //            //                break;
        //            //            case eDbType.VarChar:
        //            //                InParam.SqlDbType = SqlDbType.VarChar;
        //            //                break;
        //            //            case eDbType.Image:
        //            //                InParam.SqlDbType = SqlDbType.Image;
        //            //                break;
        //            //            case eDbType.Text:
        //            //                InParam.SqlDbType = SqlDbType.Text;
        //            //                break;
        //            //             begin TT#173  Provide database container for large data collections
        //            //            case eDbType.smallint:
        //            //                InParam.SqlDbType = SqlDbType.SmallInt;
        //            //                break;
        //            //            case eDbType.tinyint:
        //            //                InParam.SqlDbType = SqlDbType.TinyInt;
        //            //                break;
        //            //            case eDbType.VarBinary:
        //            //                InParam.SqlDbType = SqlDbType.VarBinary;
        //            //                InParam.Size = ((byte[])InputParameters[i].Value).Length;
        //            //                break;
        //            //             end TT#173  Provide database container for large data collections
        //            //             begin TT#1185 - Verify ENQ before Update
        //            //            case eDbType.Int64:
        //            //                InParam.SqlDbType = SqlDbType.BigInt;
        //            //                break;
        //            //             end TT#1185 - Verify ENQ before Update
        //            //            Begin TT#827-MD -jsobek -Allocation Reviews Performance
        //            //            case eDbType.Structured:
        //            //                InParam.SqlDbType = SqlDbType.Structured;
        //            //                InParam.TypeName = InputParameters[i].TypeName;
        //            //                break;
        //            //            End TT#827-MD -jsobek -Allocation Reviews Performance
        //            //            default:
        //            //                break;
        //            //        }
        //            //        if (InputParameters[i].Value == null)
        //            //        {
        //            //            InParam.Value = DBNull.Value;
        //            //        }
        //            //        else if (InputParameters[i].DbType == eDbType.Char)
        //            //        {
        //            //            InParam.Value = Convert.ToString(InputParameters[i].Value, CultureInfo.CurrentUICulture);
        //            //            InParam.Size = 1;
        //            //        }
        //            //        else if (InputParameters[i].DbType == eDbType.Text)
        //            //        {
        //            //            InParam.Value = Convert.ToString(InputParameters[i].Value, CultureInfo.CurrentUICulture);
        //            //            InParam.Size = ((System.Text.StringBuilder)InputParameters[i].Value).Length;
        //            //        }
        //            //        else
        //            //        {
        //            //            InParam.Value = InputParameters[i].Value;
        //            //        }

        //            //        _readCommand.Parameters.Add(InParam);
        //            //    }
        //            //}
        //            AddInputParametersToSQLCommand(InputParameters, ref _readCommand, true, true);
        //            //End TT#827-MD -jsobek -Allocation Reviews Performance

        //            if ( OutputParameters != null && OutputParameters.Length > 0 )
        //            {
        //                for( int i = 0; i < OutputParameters.Length; i++ )
        //                {
        //                    SqlParameter OutParam  =  new SqlParameter();
        //                    switch (OutputParameters[i].Direction)
        //                    {
        //                        case (eParameterDirection.Output):
        //                        {
        //                            OutParam.Direction = ParameterDirection.Output;
        //                            break;
        //                        }
        //                        case (eParameterDirection.InputOutput):
        //                        {
        //                            OutParam.Direction = ParameterDirection.InputOutput;
        //                            break;
        //                        }
        //                        default:
        //                        {
        //                            OutParam.Direction = ParameterDirection.ReturnValue;
        //                            break;
        //                        }
        //                    }
        //                    OutParam.ParameterName = OutputParameters[i].ParameterName;

        //                    if (OutputParameters[i].Value == null)
        //                    {
        //                        OutParam.Value = DBNull.Value;
        //                    }
        //                    else if (OutputParameters[i].DbType == eDbType.Char) 
        //                    {
        //                        OutParam.Value = Convert.ToString(OutputParameters[i].Value, CultureInfo.CurrentUICulture) ;
        //                        OutParam.Size = 1;
        //                    }
        //                    else if (OutputParameters[i].DbType == eDbType.Text)
        //                    {
        //                        OutParam.Value = Convert.ToString(OutputParameters[i].Value, CultureInfo.CurrentUICulture) ;
        //                        OutParam.Size = ((System.Text.StringBuilder)OutputParameters[i].Value).Length;
        //                    }
        //                    else
        //                    {
        //                        OutParam.Value = OutputParameters[i].Value;
        //                    }
        //                    switch (OutputParameters[i].DbType)
        //                    {
        //                        case eDbType.Bit:
        //                            OutParam.SqlDbType = SqlDbType.Bit;
        //                            break;
        //                        case eDbType.Char:
        //                            OutParam.SqlDbType = SqlDbType.Char;
        //                            break;
        //                        case eDbType.DateTime:
        //                            OutParam.SqlDbType = SqlDbType.DateTime;
        //                            break;
        //                        case eDbType.Float:
        //                            OutParam.SqlDbType = SqlDbType.Float;
        //                            break;
        //                        case eDbType.Decimal:
        //                            OutParam.SqlDbType = SqlDbType.Decimal;
        //                            break;
        //                        case eDbType.Int:
        //                            OutParam.SqlDbType = SqlDbType.Int;
        //                            break;
        //                        case eDbType.SmallDateTime:
        //                            OutParam.SqlDbType = SqlDbType.SmallDateTime;
        //                            break;
        //                        case eDbType.VarChar:
        //                            OutParam.SqlDbType = SqlDbType.VarChar;
        //                            break;
        //                        case eDbType.Image:
        //                            OutParam.SqlDbType = SqlDbType.Image;
        //                            break;
        //                        case eDbType.Text:
        //                            OutParam.SqlDbType = SqlDbType.Text;
        //                            break;
        //                        // begin TT#173  Provide database container for large data collections
        //                        case eDbType.smallint:
        //                            OutParam.SqlDbType = SqlDbType.SmallInt;
        //                            break;
        //                        case eDbType.tinyint:
        //                            OutParam.SqlDbType = SqlDbType.TinyInt;
        //                            break;
        //                        case eDbType.VarBinary:
        //                            OutParam.SqlDbType = SqlDbType.VarBinary;
        //                            break;
        //                        // end TT#173  Provide database container for large data collections
        //                        // begin TT#1185 - Verify ENQ before Update
        //                        case eDbType.Int64:
        //                            OutParam.SqlDbType = SqlDbType.BigInt;
        //                            break;
        //                        // end TT#1185 - Verify ENQ before Update
        //                        default:
        //                            break;
        //                    }
        //                    _readCommand.Parameters.Add( OutParam );
        //                }
        //            }

        //            SQLMonitorList.SQLMonitorEntry se = null;
        //            SQLMonitorList.BeforeExecution(ref se, ProcedureName, InputParameters);

        //            //_forwardReader = _readCommand.ExecuteReader(); // MID Track 4341 Performance Issues
        //            _forwardReader = _readCommand.ExecuteReader(CommandBehavior.SingleResult); // MID Track 4341 Performance Issues
        //            SQLMonitorList.AfterExecution(se);
        //            if ( OutputParameters != null && OutputParameters.Length > 0 )
        //            {
        //                for( int i = 0; i < OutputParameters.Length; i++ )
        //                {
        //                    OutputParameters[i].Value = _readCommand.Parameters[OutputParameters[i].ParameterName].Value;
        //                }
        //            }		
        //        }
        //        catch ( SqlException sql_error )
        //        {
        //            // Begin TT#3056 - JSmith - Header VIRTUAL_LOCK issue
        //            //HandleDatabaseReadException(sql_error, ProcedureName, _readCommand);
        //            _databaseExceptionHandler.HandleDatabaseException(sql_error, ProcedureName, _readCommand);
        //            // End TT#3056 - JSmith - Header VIRTUAL_LOCK issue
        //        }
        //        catch ( Exception error )
        //        {
        //            string message = error.ToString();
        //            throw;
        //        }
        //        return null;
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}

        ///// <summary>
        ///// Executes SqlBulkCopy to bulk insert rows from DataTable.. 
        ///// </summary>
        ///// <param name="TableName"></param>
        ///// <param name="DataTable"></param>
        //public bool SQLBulkCopy(string aTableName, DataTable aDataTable)
        //{
        //    try
        //    {
        //        object[] args = new object[] { aTableName, aDataTable };
        //        _myDatabaseMethodDelegate = new DatabaseMethodDelegate(mySQLBulkCopy);
        //        Command command = new Command(_myDatabaseMethodDelegate, args, "");
        //        _updateCommands.Add(command);
        //        object returnObject = ProcessUpdateCommand(command);
        //        return Convert.ToBoolean(returnObject, CultureInfo.CurrentCulture);
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}
        ///// <summary>
        ///// Executes SqlBulkCopy to bulk insert rows from DataTable. 
        ///// </summary>
        ///// <param name="aArgs"></param>
        ///// <returns>int</returns>
        //private object mySQLBulkCopy(object[] aArgs)
        //{
        //    try
        //    {
        //        string ProcedureName = "SqlBulkCopy " + (string)(aArgs[0]);
        //        string tableName = (string)aArgs[0];
        //        DataTable dataTable = (DataTable)aArgs[1];
        //        try
        //        {
        //            SqlBulkCopy bulkCopy = new SqlBulkCopy(_updateConnection.SQLCommand.Connection, SqlBulkCopyOptions.Default, _updateConnection.SQLTrans);
        //            if (_commandTimeout != bulkCopy.BulkCopyTimeout)
        //            {
        //                bulkCopy.BulkCopyTimeout = _commandTimeout;
        //            }

        //            // send no more than 5000 rows at a time.
        //            if (dataTable.Rows.Count > 5000)
        //            {
        //                bulkCopy.BatchSize = 5000;
        //            }
        //            else
        //            {
        //                bulkCopy.BatchSize = dataTable.Rows.Count;
        //            }
        //            bulkCopy.DestinationTableName = tableName;
        //            bulkCopy.WriteToServer(dataTable);
        //        }
        //        catch (SqlException sql_error)
        //        {
        //            // Begin TT#3056 - JSmith - Header VIRTUAL_LOCK issue
        //            //HandleDatabaseUpdateException(sql_error, ProcedureName);
        //            _databaseExceptionHandler.HandleDatabaseException(sql_error, ProcedureName, null);
        //            // End TT#3056 - JSmith - Header VIRTUAL_LOCK issue
        //            return false;
        //        }
        //        catch (Exception error)
        //        {
        //            string message = error.ToString();
        //            throw;
        //        }
        //        finally
        //        {
        //        }
        //        return true;
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}

        ///// <summary>
        ///// Get the columns defined to a table.
        ///// </summary>
        ///// <param name="aTableName"></param>
        ///// <returns>Returns an empty DataTable containing the columns of the table</returns>
        //public DataTable GetTableSchema(string aTableName)
        //{
        //    try
        //    {
        //        // Begin TT#3235 - JSmith - Cancel Allocation Action is not working
        //        //string SQLCommand = "select top 0 * from " + aTableName;
        //        string SQLCommand = "select top 0 * from " + aTableName + " with (nolock)";
        //        // Begin TT#3235 - JSmith - Cancel Allocation Action is not working
        //        return ExecuteSQLQuery(SQLCommand, aTableName);
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}

        //// Begin TT#3373 - JSmith - Save Store Forecast receive DBNull error
        ///// <summary>
        ///// Get the columns defined to a SQL Type.
        ///// </summary>
        ///// <param name="aTableType"></param>
        ///// <param name="aTableName"></param>
        ///// <returns>Returns an empty DataTable containing the columns of the type</returns>
        //public DataTable GetTableSchemaFromType(int aTableType, string aTableName)
        //{
        //    try
        //    {
        //        MIDDbParameter[] inParams = { new MIDDbParameter("@TABLE_TYPE", aTableType, eDbType.Int, eParameterDirection.Input) };
        //        return ExecuteQuery("SP_MID_GET_TABLE_FROM_TYPE", aTableName, inParams);
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}
        //// End TT#3373 - JSmith - Save Store Forecast receive DBNull error

        ///// <summary>
        ///// Creates a copy of a table.
        ///// </summary>
        ///// <param name="aTableName"></param>
        ///// <returns>Returns an empty DataTable containing the columns of the table</returns>
        //public void CloneTable(string aCopyFromTableName, string aCopyToTableName)
        //{
        //    try
        //    {
        //        string SQLCommand = "select top 0 * into " + aCopyToTableName + " from " + aCopyFromTableName;
        //        ExecuteNonQuery(SQLCommand);
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}
        
        // End TT#739-MD -JSmith - Delete Stores

//        internal void HandleDatabaseUpdateException(SqlException aSqlException, string aMIDCommand)
//        {
//            try
//            {
//                CloseDatabaseUpdateConnection();

//                string sErrorMessage = "";				
//                for ( int i = 0; i < aSqlException.Errors.Count; i++ )
//                    sErrorMessage += aSqlException.Errors[i].Number.ToString() 
//                        + ":" + aSqlException.Errors[i].Message + "\n";

////				EventLog.WriteEntry("MIDRetail", "Database error; ConnectionString=" + ConnectionString + ";Command=" + _lastCommand + ";Error=" + sErrorMessage, EventLogEntryType.Error);
				
//                Exception err = null;
//                // uses SQLServer 2000 ErrorCodes 
//                switch (aSqlException.Number) 
//                { 
//                    case (int)eDatabaseError.Timeout:
//                        // temporarily remove inner exception due to incompability issue between server 2003 and xp
//                        err = new DatabaseRetryableViolation(Include.ErrorDatabase + "Command=" + aMIDCommand + ";Error=" + sErrorMessage); 
////						err = new DatabaseRetryableViolation(Include.ErrorDatabase + sErrorMessage, aSqlException); 
//                        break;
//                    case (int)eDatabaseError.Blocking: 
//                        // temporarily remove inner exception due to incompability issue between server 2003 and xp
//                        err = new DatabaseRetryableViolation(Include.ErrorDatabase + "Command=" + aMIDCommand + ";Error=" + sErrorMessage);  
////						err = new DatabaseRetryableViolation(Include.ErrorDatabase + sErrorMessage, aSqlException); 
//                        break;
//                    case (int)eDatabaseError.ScanErrorWithNolock: 
//                        // temporarily remove inner exception due to incompability issue between server 2003 and xp
//                        err = new DatabaseRetryableViolation(Include.ErrorDatabase + "Command=" + aMIDCommand + ";Error=" + sErrorMessage);  
////						err = new DatabaseRetryableViolation(Include.ErrorDatabase + sErrorMessage, aSqlException); 
//                        break;
//                    case (int)eDatabaseError.ForeignKeyViolation: 
//                        // ForeignKey Violation 
//                        EventLog.WriteEntry("MIDRetail", "ForeignKeyViolation Database error; ConnectionString=" + ConnectionString + ";Command=" + aMIDCommand + ";Error=" + sErrorMessage, EventLogEntryType.Error);
//                        // temporarily remove inner exception due to incompability issue between server 2003 and xp
//                        err = new DatabaseForeignKeyViolation(Include.ErrorDatabase + sErrorMessage); 
////						err = new DatabaseForeignKeyViolation(Include.ErrorDatabase + sErrorMessage, aSqlException); 
//                        break; 
//                    case (int)eDatabaseError.DeadLock:
//                        // temporarily remove inner exception due to incompability issue between server 2003 and xp
//                        err = new DatabaseRetryableViolation(Include.ErrorDatabase + "Command=" + aMIDCommand + ";Error=" + sErrorMessage);  
////						err = new DatabaseRetryableViolation(Include.ErrorDatabase + sErrorMessage, aSqlException); 
//                        break;
//                    case (int)eDatabaseError.DeadLock2: 
//                        // temporarily remove inner exception due to incompability issue between server 2003 and xp
//                        err = new DatabaseRetryableViolation(Include.ErrorDatabase + "Command=" + aMIDCommand + ";Error=" + sErrorMessage);  
////						err = new DatabaseRetryableViolation(Include.ErrorDatabase + sErrorMessage, aSqlException); 
//                        break;
//                    case (int)eDatabaseError.UniqueIndexConstriantViolation: 
//                        EventLog.WriteEntry("MIDRetail", "UniqueIndexConstriantViolation Database error; ConnectionString=" + ConnectionString + ";Command=" + aMIDCommand + ";Error=" + sErrorMessage, EventLogEntryType.Error);
//                        // temporarily remove inner exception due to incompability issue between server 2003 and xp
//                        err = new DatabaseUniqueIndexConstriantViolation(Include.ErrorDatabase + sErrorMessage); 
////						err = new DatabaseUniqueIndexConstriantViolation(Include.ErrorDatabase + sErrorMessage, aSqlException); 
//                        break; 
//                    case (int)eDatabaseError.UniqueIndexConstriantViolation2:
//                        EventLog.WriteEntry("MIDRetail", "UniqueIndexConstriantViolation2 Database error; ConnectionString=" + ConnectionString + ";Command=" + aMIDCommand + ";Error=" + sErrorMessage, EventLogEntryType.Error);
//                        // temporarily remove inner exception due to incompability issue between server 2003 and xp
//                        err = new DatabaseUniqueIndexConstriantViolation2(Include.ErrorDatabase + sErrorMessage); 
////						err = new DatabaseUniqueIndexConstriantViolation2(Include.ErrorDatabase + sErrorMessage, aSqlException); 
//                        break; 
//                    case (int)eDatabaseError.InvalidDatabase: 
//                        EventLog.WriteEntry("MIDRetail", "InvalidDatabase Database error; ConnectionString=" + ConnectionString + ";Command=" + aMIDCommand + ";Error=" + sErrorMessage, EventLogEntryType.Error);
//                        // temporarily remove inner exception due to incompability issue between server 2003 and xp
//                        err = new MIDDatabaseUnavailableException(Include.ErrorDatabase + sErrorMessage); 
////						err = new MIDDatabaseUnavailableException(Include.ErrorDatabase + sErrorMessage, aSqlException); 
//                        break; 
//                    case (int)eDatabaseError.LoginFailed: 
//                        EventLog.WriteEntry("MIDRetail", "LoginFailed Database error; ConnectionString=" + ConnectionString + ";Command=" + aMIDCommand + ";Error=" + sErrorMessage, EventLogEntryType.Error);
//                        // temporarily remove inner exception due to incompability issue between server 2003 and xp
//                        err = new DatabaseLoginFailed(Include.ErrorDatabase + sErrorMessage); 
////						err = new DatabaseLoginFailed(Include.ErrorDatabase + sErrorMessage, aSqlException); 
//                        break; 
//                    case (int)eDatabaseError.GeneralNetworkError: 
//                        // temporarily remove inner exception due to incompability issue between server 2003 and xp
//                        err = new DatabaseRetryableViolation(Include.ErrorDatabase + "Command=" + aMIDCommand + ";Error=" + sErrorMessage);  
//                        //						err = new DatabaseRetryableViolation(Include.ErrorDatabase + sErrorMessage, aSqlException); 
//                        break;
//                    case (int)eDatabaseError.NotInCatalog: 
//                        // temporarily remove inner exception due to incompability issue between server 2003 and xp
//                        err = new DatabaseNotInCatalog(Include.ErrorDatabase + sErrorMessage);
//                        //						err = new DatabaseNotInCatalog(Include.ErrorDatabase + sErrorMessage, aSqlException); 
//                        break;
//                    // Begin Track #6304 - JSmith - query processor could not start the necessary thread resources for parallel query 
//                    case (int)eDatabaseError.ParallelQueryThreadError:
//                        // temporarily remove inner exception due to incompability issue between server 2003 and xp
//                        err = new ParallelQueryThreadError(Include.ErrorDatabase + sErrorMessage);
//                        break;
//                    // End Track #6304
//                    default: 
//                        EventLog.WriteEntry("MIDRetail", "Database error=" + aSqlException.Number.ToString() + "; ConnectionString=" + ConnectionString + ";Command=" + aMIDCommand + ";Error=" + sErrorMessage, EventLogEntryType.Error);
//                        // temporarily remove inner exception due to incompability issue between server 2003 and xp
//                        err = new Exception(Include.ErrorDatabase + sErrorMessage);
////						err = new Exception(Include.ErrorDatabase + sErrorMessage, aSqlException);
//                        break; 
//                } 

//                throw err;
//            }
//            catch ( Exception error )
//            {
//                string message = error.ToString();
//                throw;
//            }
//        }
        // End TT#3056 - JSmith - Header VIRTUAL_LOCK issue

		internal void CloseDatabaseUpdateConnection()
		{
			try
			{
				if (_updateConnection != null)
				{
					if (_updateConnection.SQLCommand.Connection != null)
					{
//Begin Track #3997 - JSmith - Operation exception closing connection
						if (_updateConnection.SQLCommand.Connection.State == ConnectionState.Open)
						{
							// swallow error if close fails because connection is already closed
							try
							{
//End Track #3997
								_updateConnection.SQLTrans.Rollback();
								_updateConnection.SQLCommand.Connection.Close();
//Begin Track #3997 - JSmith - Operation exception closing connection
							}
							catch
							{
							}
						}
//End Track #3997
					}
				}
			}
			catch
			{
				throw;
			}
		}

        // Begin TT#3056 - JSmith - Header VIRTUAL_LOCK issue
//        internal void HandleDatabaseReadException(SqlException aSqlException, string aMIDCommand)
//        {
//            try
//            {
//                string sErrorMessage = "";				
//                for ( int i = 0; i < aSqlException.Errors.Count; i++ )
//                    sErrorMessage += aSqlException.Errors[i].Number.ToString() 
//                        + ":" + aSqlException.Errors[i].Message + "\n";

////				EventLog.WriteEntry("MIDRetail", "Database error; ConnectionString=" + ConnectionString + ";Command=" + _lastCommand + ";Error=" + sErrorMessage, EventLogEntryType.Error);
				
//                Exception err = null;
//                // uses SQLServer 2000 ErrorCodes 
//                switch (aSqlException.Number) 
//                { 
//                    case (int)eDatabaseError.Timeout: 
//                        // temporarily remove inner exception due to incompability issue between server 2003 and xp
//                        err = new DatabaseRetryableViolation(Include.ErrorDatabase + "Command=" + aMIDCommand + ";Error=" + sErrorMessage);  
////						err = new DatabaseRetryableViolation(Include.ErrorDatabase + sErrorMessage, aSqlException); 
//                        break;
//                    case (int)eDatabaseError.Blocking: 
//                        // temporarily remove inner exception due to incompability issue between server 2003 and xp
//                        err = new DatabaseRetryableViolation(Include.ErrorDatabase + "Command=" + aMIDCommand + ";Error=" + sErrorMessage);  
////						err = new DatabaseRetryableViolation(Include.ErrorDatabase + sErrorMessage, aSqlException); 
//                        break;
//                    case (int)eDatabaseError.ScanErrorWithNolock: 
//                        // temporarily remove inner exception due to incompability issue between server 2003 and xp
//                        err = new DatabaseRetryableViolation(Include.ErrorDatabase + "Command=" + aMIDCommand + ";Error=" + sErrorMessage);  
////						err = new DatabaseRetryableViolation(Include.ErrorDatabase + sErrorMessage, aSqlException); 
//                        break;
//                    case (int)eDatabaseError.ForeignKeyViolation: 
//                        EventLog.WriteEntry("MIDRetail", "ForeignKeyViolation Database error; ConnectionString=" + ConnectionString + ";Command=" + aMIDCommand + ";Error=" + sErrorMessage, EventLogEntryType.Error);
//                        // temporarily remove inner exception due to incompability issue between server 2003 and xp
//                        err = new DatabaseForeignKeyViolation(Include.ErrorDatabase + sErrorMessage); 
////						err = new DatabaseForeignKeyViolation(Include.ErrorDatabase + sErrorMessage, aSqlException); 
//                        break; 
//                    case (int)eDatabaseError.DeadLock: 
//                        // temporarily remove inner exception due to incompability issue between server 2003 and xp
//                        err = new DatabaseRetryableViolation(Include.ErrorDatabase + "Command=" + aMIDCommand + ";Error=" + sErrorMessage);  
////						err = new DatabaseRetryableViolation(Include.ErrorDatabase + sErrorMessage, aSqlException); 
//                        break;
//                    case (int)eDatabaseError.DeadLock2: 
//                        // temporarily remove inner exception due to incompability issue between server 2003 and xp
//                        err = new DatabaseRetryableViolation(Include.ErrorDatabase + "Command=" + aMIDCommand + ";Error=" + sErrorMessage);  
////						err = new DatabaseRetryableViolation(Include.ErrorDatabase + sErrorMessage, aSqlException); 
//                        break;
//                    case (int)eDatabaseError.InvalidDatabase: 
//                        EventLog.WriteEntry("MIDRetail", "InvalidDatabase Database error; ConnectionString=" + ConnectionString + ";Command=" + aMIDCommand + ";Error=" + sErrorMessage, EventLogEntryType.Error);
//                        // temporarily remove inner exception due to incompability issue between server 2003 and xp
//                        err = new MIDDatabaseUnavailableException(Include.ErrorDatabase + sErrorMessage); 
////						err = new MIDDatabaseUnavailableException(Include.ErrorDatabase + sErrorMessage, aSqlException); 
//                        break;
//                    case (int)eDatabaseError.LoginFailed: 
//                        EventLog.WriteEntry("MIDRetail", "LoginFailed Database error; ConnectionString=" + ConnectionString + ";Command=" + aMIDCommand + ";Error=" + sErrorMessage, EventLogEntryType.Error);
//                        // temporarily remove inner exception due to incompability issue between server 2003 and xp
//                        err = new DatabaseLoginFailed(Include.ErrorDatabase + sErrorMessage); 
//                        //						err = new DatabaseLoginFailed(Include.ErrorDatabase + sErrorMessage, aSqlException); 
//                        break; 
//                    case (int)eDatabaseError.GeneralNetworkError: 
//                        // temporarily remove inner exception due to incompability issue between server 2003 and xp
//                        err = new DatabaseRetryableViolation(Include.ErrorDatabase + "Command=" + aMIDCommand + ";Error=" + sErrorMessage);  
////						err = new DatabaseRetryableViolation(Include.ErrorDatabase + sErrorMessage, aSqlException); 
//                        break;
//                    default: 
//                        EventLog.WriteEntry("MIDRetail", "Database error=" + aSqlException.Number.ToString() + "; ConnectionString=" + ConnectionString + ";Command=" + aMIDCommand + ";Error=" + sErrorMessage, EventLogEntryType.Error);
//                        // temporarily remove inner exception due to incompability issue between server 2003 and xp
//                        err = new Exception(Include.ErrorDatabase + sErrorMessage);
////						err = new Exception(Include.ErrorDatabase + sErrorMessage, aSqlException);
//                        break; 
//                } 

//                throw err;
//            }
//            catch ( Exception error )
//            {
//                string message = error.ToString();
//                throw;
//            }
//        }
        // End TT#3056 - JSmith - Header VIRTUAL_LOCK issue

		public object ReadOutputParmValue(string aOutputParmName)
		{
			return OutputParmValue(_readCommand, aOutputParmName);
		}

        //public object UpdateOutputParmValue(string aOutputParmName)
        //{
        //    return OutputParmValue(_updateConnection.SQLCommand, aOutputParmName);
        //}
		public object OutputParmValue(SqlCommand aSQLCommand, string aOutputParmName)
		{
			foreach( SqlParameter param in aSQLCommand.Parameters )
			{
				if ( param.Direction == ParameterDirection.Output 
					|| param.Direction == ParameterDirection.InputOutput
					|| param.Direction == ParameterDirection.ReturnValue)
				{
					if (param.ParameterName == aOutputParmName)
					{
						if (param.Value == DBNull.Value)
						{
							return null;
						}
						switch (param.SqlDbType)
						{
							case SqlDbType.Bit:
							{
								return Convert.ToBoolean(param.Value);
							}
							case SqlDbType.Char:
							{
								return Convert.ToChar(param.Value);
							}
							case SqlDbType.DateTime:
							{
								if (param.Value == DBNull.Value)
								{
									return Include.UndefinedDate;
								}
								return Convert.ToDateTime(param.Value);
							}
							case SqlDbType.Float:
							{
								return Convert.ToDouble(param.Value);
							}
							case SqlDbType.Int:
							{
								return Convert.ToInt32(param.Value);
							}
							case SqlDbType.SmallDateTime:
							{
								return Convert.ToDateTime(param.Value);
							}
							case SqlDbType.VarChar:
							{
								return Convert.ToString(param.Value);
							}
							case SqlDbType.Image:
							{
								return Convert.ToString(param.Value);
							}
							case SqlDbType.Text:
							{
								return Convert.ToString(param.Value);
							}
							default:
							{	
								break;
							}
						}
					}
				}
			}
			return null;
		}

  

	

		/// <summary>
		/// Creates new database adapter.
		/// </summary>
		/// <param name="tableName"></param>
		/// <returns></returns>
		public DBAdapter NewDBAdapter(string tableName)
		{
			try
			{
				object[] args = new object[]{tableName};
				_myDatabaseMethodDelegate = new DatabaseMethodDelegate(myNewDBAdapter);
				Command command = new Command(_myDatabaseMethodDelegate, args, "DatabaseAccess New DB adapter for " + tableName);
				object returnObject = ProcessReadCommand(command);
				return (DBAdapter)returnObject;
			}
			catch
			{
				throw;
			}
		}
		/// <summary>
		/// Creates new database adapter.
		/// </summary>
		/// <param name="aArgs"></param>
		/// <returns></returns>
		private object myNewDBAdapter(object[] aArgs)
		{
			try
			{
				string tableName = Convert.ToString(aArgs[0], CultureInfo.CurrentCulture);
				DBAdapter ad;
				SqlConnection cn = new SqlConnection(DBConnectionString);  // TT#2131-MD - JSmith - Halo Integration
				//cn.Open();

				ad = new DBAdapter(tableName, cn, _commandTimeout);	// Issue 5018 stodd 12.24.2007
				return ad;
			}
			catch
			{
				throw;
			}
		}
		
		private object ProcessReadCommand(Command aCommand)
		{
			try
			{
				object returnObject = null;
				int retryCount = 0;
				bool retry = true;
					
				while (retry)
				{
					try
					{
						if (aCommand.DatabaseMethodDelegate != null)
						{
							returnObject = aCommand.DatabaseMethodDelegate(aCommand.Args);
						}
						retry = false;
					}
					catch (DatabaseRetryableViolation ex)
					{
						++retryCount;
						if (_retryDatabaseCommand &&
							retryCount < _maximumRetryAttempts)
						{
                            // Begin TT#3325 - JSmith - Event Log String Too Long - Error Message
                            //EventLog.WriteEntry("MIDRetail", "Error during read.  Retry attempt=" + retryCount.ToString() + " of " + _maximumRetryAttempts.ToString() + " because of error:" + ex.ToString(), EventLogEntryType.Information);
                            if (ex.ToString().Length > 32700)
                            {
                                EventLog.WriteEntry("MIDRetail", "Error during read.  Retry attempt=" + retryCount.ToString() + " of " + _maximumRetryAttempts.ToString() + " because of error:" + ex.ToString().Substring(0, 32700), EventLogEntryType.Information);
                            }
                            else
                            {
                                EventLog.WriteEntry("MIDRetail", "Error during read.  Retry attempt=" + retryCount.ToString() + " of " + _maximumRetryAttempts.ToString() + " because of error:" + ex.ToString(), EventLogEntryType.Information);
                            }
                            if (retryCount == 1)
                            {
                                System.Diagnostics.StackTrace t = new System.Diagnostics.StackTrace();
                                if (t.ToString().Length > 32700)
                                {
                                    EventLog.WriteEntry("MIDRetail", "Call Stack" + Environment.NewLine + t.ToString().Substring(0, 32700), EventLogEntryType.Information);
                                }
                                else
                                {
                                    EventLog.WriteEntry("MIDRetail", "Call Stack" + Environment.NewLine + t.ToString(), EventLogEntryType.Information);
                                }
                            }
                            // End TT#3325 - JSmith - Event Log String Too Long - Error Message
							System.Threading.Thread.Sleep(_retrySleepTime);
							CloseDatabaseUpdateConnection();
						}
						else
						{
                            System.Diagnostics.StackTrace t = new System.Diagnostics.StackTrace();
                            // Begin TT#3325 - JSmith - Event Log String Too Long - Error Message
                            //EventLog.WriteEntry("MIDRetail", ex.ToString() + Environment.NewLine + Environment.NewLine + "Call Stack" + Environment.NewLine + t.ToString(), EventLogEntryType.Error);
                            if (ex.ToString().Length > 32700)
                            {
                                EventLog.WriteEntry("MIDRetail", ex.ToString().Substring(0, 32700), EventLogEntryType.Error);
                            }
                            else
                            {
                                EventLog.WriteEntry("MIDRetail", ex.ToString(), EventLogEntryType.Error);
                            }
                            if (t.ToString().Length > 32700)
                            {
                                EventLog.WriteEntry("MIDRetail", "Call Stack" + Environment.NewLine + t.ToString().Substring(0, 32700), EventLogEntryType.Error);
                            }
                            else
                            {
                                EventLog.WriteEntry("MIDRetail", "Call Stack" + Environment.NewLine + t.ToString(), EventLogEntryType.Error);
                            }
                            // End TT#3325 - JSmith - Event Log String Too Long - Error Message
							retry = false;
							CloseUpdateConnection();
                            throw new DatabaseRetryableViolation(ex.MidErrorMessage, ex.MidErrorMessage);                          
						}
					}
					catch
					{
						retry = false;
						CloseUpdateConnection();
						throw;
					}
				}
				return returnObject;
			}
			catch
			{
				throw;
			}
		}

		private object ProcessUpdateCommand(Command aCommand)
		{
			try
			{
				object returnObject = null;
			
				try
				{
					if (aCommand.DatabaseMethodDelegate != null)
					{
						returnObject = aCommand.DatabaseMethodDelegate(aCommand.Args);
					}
				}
				catch (DatabaseRetryableViolation ex)
				{
                    // begin TT#1185 - Verify ENQ before update
                    if (_allowRetryOnUpdateCommand)
                    {
                        //end TT#1185 - Verify ENQ before update
						System.Threading.Thread.Sleep(_retrySleepTime);
                        // Begin TT#3325 - JSmith - Event Log String Too Long - Error Message
                        //EventLog.WriteEntry("MIDRetail", "Error during write.  Retry attempt= 1 of " + _maximumRetryAttempts.ToString() + " because of error:" + ex.ToString(), EventLogEntryType.Information);
                        if (ex.ToString().Length > 32700)
                        {
                            EventLog.WriteEntry("MIDRetail", "Error during write.  Retry attempt= 1 of " + _maximumRetryAttempts.ToString() + " because of error:" + ex.ToString().Substring(0, 32700), EventLogEntryType.Information);
                        }
                        else
                        {
                            EventLog.WriteEntry("MIDRetail", "Error during write.  Retry attempt= 1 of " + _maximumRetryAttempts.ToString() + " because of error:" + ex.ToString(), EventLogEntryType.Information);
                        }
                        
                        System.Diagnostics.StackTrace t = new System.Diagnostics.StackTrace();
                        if (t.ToString().Length > 32700)
                        {
                            EventLog.WriteEntry("MIDRetail", "Call Stack" + Environment.NewLine + t.ToString().Substring(0, 32700), EventLogEntryType.Information);
                        }
                        else
                        {
                            EventLog.WriteEntry("MIDRetail", "Call Stack" + Environment.NewLine + t.ToString(), EventLogEntryType.Information);
                        }
                        // End TT#3325 - JSmith - Event Log String Too Long - Error Message

	                    //ReprocessUpdateCommands();              //  TT#929 Orphaned Intransit
	                    returnObject = ReprocessUpdateCommands(); // TT#929 Orphaned Intransit
                    }
                    // begin TT#1185 - Verify ENQ before update
                    else
                    {
                        // Begin Development TT#18 - JSmith - Charge intransit action fails when processed alone or in a workflow.
                        //EventLog.WriteEntry("MIDAllocation", "Error during write.  No retry attempts due to database synchronization issues. " + ex.ToString(), EventLogEntryType.Information);
                        EventLog.WriteEntry("MIDRetail", "Error during write.  No retry attempts due to database synchronization issues. " + ex.ToString(), EventLogEntryType.Information);
                        // End Development TT#18
                        throw;
                    }
                    // end TT#1185 - Verify ENQ before update
                }
				catch 
				{
					throw;
				}
				return returnObject;
			}
			catch
			{
				throw;
			}
		}

		private object ReprocessUpdateCommands()
		{
			try
			{
				object returnObject = null;
				int retryCount = 1;
				bool retry = true;
					
				while (retry)
				{
					try
					{
						foreach(Command command in _updateCommands)
						{
							returnObject = command.DatabaseMethodDelegate(command.Args);
						}
						retry = false;
					}
					catch (DatabaseRetryableViolation ex)
					{
						++retryCount;
						if (retryCount < _maximumRetryAttempts)
						{
							System.Threading.Thread.Sleep(_retrySleepTime);
							
                            // Begin TT#3325 - JSmith - Event Log String Too Long - Error Message
                            //EventLog.WriteEntry("MIDRetail", "Error during write.  Retry attempt=" + retryCount.ToString() + " of " + _maximumRetryAttempts.ToString() + " because of error:" + ex.ToString(), EventLogEntryType.Information);
                            if (ex.ToString().Length > 32700)
                            {
                                EventLog.WriteEntry("MIDRetail", "Error during write.  Retry attempt=" + retryCount.ToString() + " of " + _maximumRetryAttempts.ToString() + " because of error:" + ex.ToString().Substring(0, 32700), EventLogEntryType.Information);
                            }
                            else
                            {
                                EventLog.WriteEntry("MIDRetail", "Error during write.  Retry attempt=" + retryCount.ToString() + " of " + _maximumRetryAttempts.ToString() + " because of error:" + ex.ToString(), EventLogEntryType.Information);
                            }
                            // End TT#3325 - JSmith - Event Log String Too Long - Error Message
						}
						else
						{
                            // Begin TT#3325 - JSmith - Event Log String Too Long - Error Message
                            //EventLog.WriteEntry("MIDRetail", ex.ToString(), EventLogEntryType.Error);
                            if (ex.ToString().Length > 32700)
                            {
                                EventLog.WriteEntry("MIDRetail", ex.ToString().Substring(0, 32700), EventLogEntryType.Error);
                            }
                            else
                            {
                                EventLog.WriteEntry("MIDRetail", ex.ToString(), EventLogEntryType.Error);
                            }
                            System.Diagnostics.StackTrace t = new System.Diagnostics.StackTrace();
                            if (t.ToString().Length > 32700)
                            {
                                EventLog.WriteEntry("MIDRetail", "Call Stack" + Environment.NewLine + t.ToString().Substring(0, 32700), EventLogEntryType.Error);
                            }
                            else
                            {
                                EventLog.WriteEntry("MIDRetail", "Call Stack" + Environment.NewLine + t.ToString(), EventLogEntryType.Error);
                            }
                            // End TT#3325 - JSmith - Event Log String Too Long - Error Message
							retry = false;
							throw;
						}
					}
					catch
					{
						retry = false;
						throw;
					}
				}
				return returnObject;
			}
			catch
			{
				throw;
			}
		}

        // Begin TT#310 - JSmith - HeaderCharacteristic length error in workspace
        public int GetColumnSize(string aTableName, string aColumnName)
        {
            int size = 0;

            string SQLCommand = "select c.max_length as 'column length' FROM sys.columns c"
                           + " JOIN sys.types AS t ON c.user_type_id=t.user_type_id"
                           + " WHERE c.object_id = OBJECT_ID('dbo." + aTableName + "')"
                           + " and c.name = '" + aColumnName + "' ";

            DataTable dt = ExecuteSQLQuery(SQLCommand, "Column Size");
            if (dt.Rows.Count > 0)
            {
                size = Convert.ToInt32(dt.Rows[0]["column length"]);
            }


            return size;
        }
        // End TT#310

        private void SetValuesOnOutputParamters(SqlCommand cd, ref MIDDbParameter[] OutputParameters)
        {
            if (OutputParameters != null && OutputParameters.Length > 0)
            {
                for (int i = 0; i < OutputParameters.Length; i++)
                {
                    OutputParameters[i].Value = cd.Parameters[OutputParameters[i].ParameterName].Value;
                }
            }	
        }

        //Begin TT#827-MD -jsobek -Allocation Reviews Performance
        private void AddInputParametersToSQLCommand(MIDDbParameter[] InputParameters, ref SqlCommand cd, bool forceDateTimeSizeTo17 = false, bool setSizeForTextParameters=false)
        {
            if (InputParameters != null && InputParameters.Length > 0)
            {
                for (int i = 0; i < InputParameters.Length; i++)
                {
                    SqlParameter InParam = new SqlParameter();
                    InParam.Direction = ParameterDirection.Input;
                    InParam.ParameterName = InputParameters[i].ParameterName;
                    if (InputParameters[i].Value == null)
                    {
                        InParam.Value = DBNull.Value;
                    }
                    else if (InputParameters[i].DbType == eDbType.Char)
                    {
                            InParam.Value = Convert.ToString(InputParameters[i].Value, CultureInfo.CurrentUICulture);
                            InParam.Size = 1;
                    }
                    else if (setSizeForTextParameters && InputParameters[i].DbType == eDbType.Text)
                    {
                        InParam.Value = Convert.ToString(InputParameters[i].Value, CultureInfo.CurrentUICulture);
                        InParam.Size = ((System.Text.StringBuilder)InputParameters[i].Value).Length;
                    }
                    else
                    {
                        InParam.Value = InputParameters[i].Value;
                    }
                    switch (InputParameters[i].DbType)
                    {
                        case eDbType.Bit:
                            InParam.SqlDbType = SqlDbType.Bit;
                            break;
                        case eDbType.Char:
                            InParam.SqlDbType = SqlDbType.Char;
                            break;
                        case eDbType.DateTime:
                            InParam.SqlDbType = SqlDbType.DateTime;
                            if (InParam.Value != DBNull.Value)
                            {
                                if (Convert.ToDateTime(InParam.Value, CultureInfo.CurrentUICulture) == Include.UndefinedDate)
                                {
                                    InParam.Value = DBNull.Value;
                                }
                                if (forceDateTimeSizeTo17)
                                {
                                    InParam.Size = 17;
                                }
                            }
                            break;

                        case eDbType.Float:
                            InParam.SqlDbType = SqlDbType.Float;
                            break;
                        case eDbType.Decimal:
                            InParam.SqlDbType = SqlDbType.Decimal;
                            break;
                        case eDbType.Int:
                            InParam.SqlDbType = SqlDbType.Int;
                            break;
                        case eDbType.SmallDateTime:
                            InParam.SqlDbType = SqlDbType.SmallDateTime;
                            break;
                        case eDbType.VarChar:
                            InParam.SqlDbType = SqlDbType.VarChar;
                            break;
                        case eDbType.Image:
                            InParam.SqlDbType = SqlDbType.Image;
                            break;
                        case eDbType.Text:
                            InParam.SqlDbType = SqlDbType.Text;
                            break;
                        // begin TT#173  Provide database container for large data collections
                        case eDbType.smallint:
                            InParam.SqlDbType = SqlDbType.SmallInt;
                            break;
                        case eDbType.tinyint:
                            InParam.SqlDbType = SqlDbType.TinyInt;
                            break;
                        case eDbType.VarBinary:
                            InParam.SqlDbType = SqlDbType.VarBinary;
                            InParam.Size = ((byte[])InputParameters[i].Value).Length;
                            break;
                        // end TT#173  Provide database container for large data collections
                        // begin TT#1185 - Verify ENQ before Update
                        case eDbType.Int64:
                            InParam.SqlDbType = SqlDbType.BigInt;
                            break;
                        // end TT#1185 - Verify ENQ before Update
                        //Begin TT#827-MD -jsobek -Allocation Reviews Performance
                        case eDbType.Structured:
                            InParam.SqlDbType = SqlDbType.Structured;
                            InParam.TypeName = InputParameters[i].TypeName;
                            break;
                        //End TT#827-MD -jsobek -Allocation Reviews Performance
                        default:
                            break;
                    }
                    cd.Parameters.Add(InParam);
                }
            }
        }
        //End TT#827-MD -jsobek -Allocation Reviews Performance
        //Begin TT#846-MD -jsobek -New Stored Procedures for Performance
        private void AddOutputParametersToSQLCommand(MIDDbParameter[] OutputParameters, ref SqlCommand cd)
        {
            if (OutputParameters != null && OutputParameters.Length > 0)
            {
                for (int i = 0; i < OutputParameters.Length; i++)
                {
                    SqlParameter OutParam = new SqlParameter();
                    OutParam.Direction = ParameterDirection.Output;
                    OutParam.ParameterName = OutputParameters[i].ParameterName;
                    //OutParam.Value = OutputParameters[i].Value;
                    if (OutputParameters[i].Value == null)
                    {
                        OutParam.Value = DBNull.Value;
                    }
                    else if (OutputParameters[i].DbType == eDbType.Char)
                    {
                        OutParam.Value = Convert.ToString(OutputParameters[i].Value, CultureInfo.CurrentUICulture);
                        OutParam.Size = 1;
                    }
                    else if (OutputParameters[i].DbType == eDbType.Text)
                    {
                        OutParam.Value = Convert.ToString(OutputParameters[i].Value, CultureInfo.CurrentUICulture);
                        OutParam.Size = ((System.Text.StringBuilder)OutputParameters[i].Value).Length;
                    }
                    else
                    {
                        OutParam.Value = OutputParameters[i].Value;
                    }
                    switch (OutputParameters[i].DbType)
                    {
                        case eDbType.Bit:
                            OutParam.SqlDbType = SqlDbType.Bit;
                            break;
                        case eDbType.Char:
                            OutParam.SqlDbType = SqlDbType.Char;
                            break;
                        case eDbType.DateTime:
                            OutParam.SqlDbType = SqlDbType.DateTime;
                            break;
                        case eDbType.Float:
                            OutParam.SqlDbType = SqlDbType.Float;
                            break;
                        case eDbType.Decimal:
                            OutParam.SqlDbType = SqlDbType.Decimal;
                            break;
                        case eDbType.Int:
                            OutParam.SqlDbType = SqlDbType.Int;
                            break;
                        case eDbType.SmallDateTime:
                            OutParam.SqlDbType = SqlDbType.SmallDateTime;
                            break;
                        case eDbType.VarChar:
                            OutParam.SqlDbType = SqlDbType.VarChar;
                            break;
                        case eDbType.Image:
                            OutParam.SqlDbType = SqlDbType.Image;
                            break;
                        case eDbType.Text:
                            OutParam.SqlDbType = SqlDbType.Text;
                            break;
                        // begin TT#173  Provide database container for large data collections
                        case eDbType.smallint:
                            OutParam.SqlDbType = SqlDbType.SmallInt;
                            break;
                        case eDbType.tinyint:
                            OutParam.SqlDbType = SqlDbType.TinyInt;
                            break;
                        case eDbType.VarBinary:
                            OutParam.SqlDbType = SqlDbType.VarBinary;
                            break;
                        // end TT#173  Provide database container for large data collections
                        // begin TT#1185 - Verify ENQ before Update
                        case eDbType.Int64:
                            OutParam.SqlDbType = SqlDbType.BigInt;
                            break;
                        // end TT#1185 - Verify ENQ before Update
                        default:
                            break;
                    }
                    cd.Parameters.Add(OutParam);
                }
            }
        }
        //End TT#846-MD -jsobek -New Stored Procedures for Performance
	}

	public class Command
	{
		DatabaseMethodDelegate _databaseMethodDelegate;
		object[] _args;
		string _MIDCommandText;

		public Command(DatabaseMethodDelegate aDatabaseMethodDelegate, object[] aArgs, string aCommandText)
		{
			_databaseMethodDelegate = aDatabaseMethodDelegate;
			_args = aArgs;
			_MIDCommandText = aCommandText;
		}

		public DatabaseMethodDelegate DatabaseMethodDelegate
		{
            get 
			{ 
				return _databaseMethodDelegate; 
			}
		}

		public object[] Args
		{
			get 
			{ 
				return _args;
			}
		}

		public string MIDCommandText
		{
			get 
			{ 
				return _MIDCommandText;
			}
		}
	}

	enum eDatabaseError
	{
		Timeout							= -2,
		GeneralNetworkError				= 11,
        SQLNotExistOrAccessDenied       = 17,  // TT#3771 - JSmith - Add Database Retry for Network Timeout
        SQLNotExistOrAccessDenied2      = 53,  // TT#3771 - JSmith - Add Database Retry for Network Timeout
        TCPSemaphoreTimeout             = 121,  // TT#3771 - JSmith - Add Database Retry for Network Timeout
		ForeignKeyViolation				= 547,
		ScanErrorWithNolock				= 601,
		DeadLock						= 1204,
		DeadLock2						= 1205,
		Blocking						= 1222,
		UniqueIndexConstriantViolation	= 2601,
		UniqueIndexConstriantViolation2	= 2627,
		NotInCatalog					= 3701,
		InvalidDatabase					= 4060,
        // Begin Track #6304 - JSmith - query processor could not start the necessary thread resources for parallel query 
        ParallelQueryThreadError        = 8642,
        // End Track #6304
		LoginFailed						= 18456,

        // Begin TT#3771 - JSmith - Add Database Retry for Network Timeout
        // Error codes found at "http://support.microsoft.com/kb/109787"
        // Server-Side Communication Errors
        UnableToReadLoginPacket = 17832,
        UnableToCloseServerSideConnection = 17825,
        UnableToWriteToServerSideConnection = 17824,
        CannotSendAfterSocketShutdown = 10058,
        ConnectionResetByPeer = 10054,
        SoftwareCausedConnectionAbort = 10053,
        NetworkErrorWasEncounteredWhileSendingResults = 1608,
        ThePipeIsBeingClosed = 232,
        ThePipeHasBeenEnded = 109,
        // Client-Side Communication Errors
        BadTokenFromSQLServer = 10008,
        ReadFromSQLServerFailed = 10010,
        ErrorClosingNetworkConnection = 10018,
        WriteToSQLServerFailed = 10025,
        // End TT#3771 - JSmith - Add Database Retry for Network Timeout
	}
}
