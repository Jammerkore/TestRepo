using System;
using System.Collections;
using System.Configuration;
using System.Data;
//using System.Data.OracleClient;
using Oracle.DataAccess.Client;
using Oracle.DataAccess.Types;
using System.Diagnostics;
using System.Globalization;

using MID.MRS.DataCommon;


namespace MID.MRS.Data
{
	/// <summary>
	/// Defines an update connection for an Oracle database
	/// </summary>
	public class DBUpdateConnection
	{
		private OracleTransaction _oracleTrans = null;
		private OracleCommand _oracleCommand = null;
		private string _sConnection  = "";

		/// <summary>
		/// Creates a new instance of DBUpdateConnection
		/// </summary>
		public DBUpdateConnection()
		{
			try
			{
				Initialize(ConfigurationSettings.AppSettings["ConnectionString"]);
			}
			catch (Exception error)
			{
				throw;
			}
		}

		/// <summary>
		/// Creates a new instance of DBUpdateConnection using the given ConnectionString
		/// </summary>
		/// <param name="aConnectionString">
		/// The ConnectionString to use to connect.
		/// </param>
		public DBUpdateConnection(string aConnectionString)
		{
			try
			{
				Initialize(aConnectionString);
			}
			catch (Exception error)
			{
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
			_sConnection =  aConnectionString;
			if (_sConnection == null)
			{
				throw new Exception(Include.ErrorBadConfigFile + Include.GetConfigFilename());
			}
			try
			{
				_oracleCommand = new OracleCommand();
				_oracleCommand.Connection = new OracleConnection(ConnectionString);
				_oracleCommand.Connection.Open();
				_oracleTrans = _oracleCommand.Connection.BeginTransaction();
//  Oracle starts an implicit transaction on a connection.  The Transaction property is read-only for Oracle.
//				_oracleCommand.Transaction = _oracleTrans;
			}
			catch ( OracleException oracle_error )
			{
				string sErrorMessage = "";				
				for ( int i = 0; i < oracle_error.Errors.Count; i++ )
					sErrorMessage += oracle_error.Errors[i].Message + "\n";

				EventLog.WriteEntry("MIDAllocation", "Unable to open update connection; ConnectionString=" + ConnectionString + ";Error=" + sErrorMessage, EventLogEntryType.Error);
				Exception err = new Exception(Include.ErrorDatabase + sErrorMessage, oracle_error);
				throw;
			}
			catch ( Exception error )
			{
				throw;
			}
		}

		/// <summary>
		/// Gets the connection string used to connect to the database.
		/// </summary>
		public string ConnectionString
		{
			get
			{
				return _sConnection;
			}
		}

		/// <summary>
		/// Gets or sets the command to issue to the database.
		/// </summary>
		public OracleCommand OracleCmd 
		{
			get { return _oracleCommand ; }
			set { _oracleCommand = value; }
		}

		/// <summary>
		/// Gets or sets the transaction for the database.
		/// </summary>
		public OracleTransaction OracleTrans 
		{
			get { return _oracleTrans ; }
			set { _oracleTrans = value; }
		}

		/// <summary>
		/// Commits the active data to the database.
		/// </summary>
		public void Commit()
		{
			try
			{
				_oracleTrans.Commit();
			}
			catch ( OracleException oracle_error )
			{
				string sErrorMessage = "";				
				for ( int i = 0; i < oracle_error.Errors.Count; i++ )
					sErrorMessage += oracle_error.Errors[i].Message + "\n";
				
				EventLog.WriteEntry("MIDAllocation", "Database error; ConnectionString=" + ConnectionString + ";Error=" + sErrorMessage, EventLogEntryType.Error);
				Exception err = new Exception(Include.ErrorDatabase + sErrorMessage, oracle_error);
				throw;
			}
			catch ( Exception error )
			{
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
				_oracleTrans.Rollback();
			}
			catch ( OracleException oracle_error )
			{
				string sErrorMessage = "";				
				for ( int i = 0; i < oracle_error.Errors.Count; i++ )
					sErrorMessage += oracle_error.Errors[i].Message + "\n";

				EventLog.WriteEntry("MIDAllocation", "Database error; ConnectionString=" + ConnectionString + ";Error=" + sErrorMessage, EventLogEntryType.Error);
				Exception err = new Exception(Include.ErrorDatabase + sErrorMessage, oracle_error);
				throw;
			}
			catch ( Exception error )
			{
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
				_oracleCommand.Connection.Close();
				_oracleCommand.Connection.Dispose();
			}
			catch ( OracleException oracle_error )
			{
				string sErrorMessage = "";				
				for ( int i = 0; i < oracle_error.Errors.Count; i++ )
					sErrorMessage += oracle_error.Errors[i].Message + "\n";
				
				EventLog.WriteEntry("MIDAllocation", "Database error; ConnectionString=" + ConnectionString + ";Error=" + sErrorMessage, EventLogEntryType.Error);
				Exception err = new Exception(Include.ErrorDatabase + sErrorMessage, oracle_error);
				throw;
			}
			catch ( Exception error )
			{
				throw;
			}
		}
	}

	/// <summary>
	/// Oracle database specific code.
	/// </summary>
	public class DatabaseAccess
	{
		protected DBUpdateConnection _updateConnection;

		private string _sConnection  = "";

		/// <summary>
		/// Creates an instance of DatabaseAccess
		/// </summary>
		public DatabaseAccess()
		{
			try
			{
				Initialize(ConfigurationSettings.AppSettings["ConnectionString"]);
			}
			catch (Exception error)
			{
				throw;
			}
		}

		/// <summary>
		/// Creates an instance of DatabaseAccess using the given ConnectionString
		/// </summary>
		/// <param name="aConnectionString">
		/// The ConnectionString to use to connect.
		/// </param>
		public DatabaseAccess(string aConnectionString)
		{
			try
			{
				Initialize(aConnectionString);
			}
			catch (Exception error)
			{
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
			_sConnection =  aConnectionString;
			if (!EventLog.SourceExists("MIDAllocation"))
			{
				EventLog.CreateEventSource("MIDAllocation", null);
			}
			if (_sConnection == null)
			{
				EventLog.WriteEntry("MIDAllocation", Include.ErrorBadConfigFile + Include.GetConfigFilename(), EventLogEntryType.Error);
				throw new Exception(Include.ErrorBadConfigFile + Include.GetConfigFilename());
			}
		}

		/// <summary>
		/// Gets the connection string used to connect to the database.
		/// </summary>
		public string ConnectionString
		{
			get
			{
				return _sConnection;
			}
		}

		/// <summary>
		/// Open an update connection to the database.
		/// </summary>
		public void OpenUpdateConnection()
		{
			_updateConnection = new DBUpdateConnection(_sConnection);
		}

		/// <summary>
		/// Write active data be commited to the database.
		/// </summary>
		public void CommitData()
		{
			_updateConnection.Commit();
			CloseUpdateConnection();
			OpenUpdateConnection();
		}

		/// <summary>
		/// Close the update connection.
		/// </summary>
		public void CloseUpdateConnection()
		{
			_updateConnection.Close();
		}

		/// <summary>
		/// Rolls back the data for the update transaction.
		/// </summary>
		public void RollBack()
		{
			_updateConnection.RollBack();
		}

		/// <summary>
		/// Execute the provided command to count the records identified in the command.
		/// </summary>
		/// <param name="OracleCmd"></param>
		/// <returns></returns>
		public int ExecuteRecordCount( string OracleCmd )
		{
			OracleCommand cd = null;
			try
			{
				int recCount = 0;
				OracleDataReader myReader;
				cd = new OracleCommand(OracleCmd.Replace("@", ":"));
	
				cd.Connection = new OracleConnection(ConnectionString );
				cd.Connection.Open();

				cd.CommandType = CommandType.Text;
								
				myReader = cd.ExecuteReader();
				if (myReader.Read())
				{
					recCount = (int)((decimal) myReader["MyCount"]);
				}
				myReader.Close();
				return recCount;
			}
			catch ( OracleException oracle_error )
			{
				string sErrorMessage = "";				
				for ( int i = 0; i < oracle_error.Errors.Count; i++ )
					sErrorMessage += oracle_error.Errors[i].Message + "\n";

				EventLog.WriteEntry("MIDAllocation", "Database error; ConnectionString=" + ConnectionString + ";Error=" + sErrorMessage, EventLogEntryType.Error);
				Exception err = new Exception(Include.ErrorDatabase + sErrorMessage, oracle_error);
				throw;
			}
			catch ( Exception error )
			{
				throw;
			}
			finally
			{
				if (cd.Connection != null)
				{
					cd.Connection.Close();
				}
			}
		}

		/// <summary>
		/// Retrieves the application text identified in the command.
		/// </summary>
		/// <param name="OracleCmd"></param>
		/// <returns></returns>
		public string ExecuteGetText( string OracleCmd )
		{
			OracleCommand cd = null;
			try
			{
				string text = null;
				OracleDataReader myReader;
				cd = new OracleCommand(OracleCmd.Replace("@", ":"));
	
				cd.Connection = new OracleConnection(ConnectionString );
				cd.Connection.Open();

				cd.CommandType = CommandType.Text;
								
				myReader = cd.ExecuteReader();
				if (myReader.Read())
				{
					text =  (string) myReader["TEXT_VALUE"];
				}
				myReader.Close();
				return text;
			}
			catch ( OracleException oracle_error )
			{
				string sErrorMessage = "";				
				for ( int i = 0; i < oracle_error.Errors.Count; i++ )
					sErrorMessage += oracle_error.Errors[i].Message + "\n";

				EventLog.WriteEntry("MIDAllocation", "Database error; ConnectionString=" + ConnectionString + ";Error=" + sErrorMessage, EventLogEntryType.Error);
				Exception err = new Exception(Include.ErrorDatabase + sErrorMessage, oracle_error);
				throw;
			}
			catch ( Exception error )
			{
				throw;
			}
			finally
			{
				if (cd.Connection != null)
				{
					cd.Connection.Close();
				}
			}
		}

		/// <summary>
		/// Executes a database maximum value request for the provided command.
		/// </summary>
		/// <param name="OracleCmd"></param>
		/// <returns></returns>
		public int ExecuteMaxValue( string OracleCmd )
		{
			OracleCommand cd = null;
			try
			{
				int maxValue = 0;
				OracleDataReader myReader;
				cd = new OracleCommand(OracleCmd.Replace("@", ":"));
	
				cd.Connection = new OracleConnection(ConnectionString );
				cd.Connection.Open();

				cd.CommandType = CommandType.Text;
								
				myReader = cd.ExecuteReader();
				if (myReader.Read())
				{
					maxValue = (int) ((decimal)myReader["MyValue"]);
				}
				myReader.Close();
				return maxValue;
			}
			catch ( OracleException oracle_error )
			{
				string sErrorMessage = "";				
				for ( int i = 0; i < oracle_error.Errors.Count; i++ )
					sErrorMessage += oracle_error.Errors[i].Message + "\n";

				EventLog.WriteEntry("MIDAllocation", "Database error; ConnectionString=" + ConnectionString + ";Error=" + sErrorMessage, EventLogEntryType.Error);
				Exception err = new Exception(Include.ErrorDatabase + sErrorMessage, oracle_error);
				throw;
			}
			catch ( Exception error )
			{
				throw;
			}
			finally
			{
				if (cd.Connection != null)
				{
					cd.Connection.Close();
				}
			}
		}

		/// <summary>
		/// ExecuteScalar is a public method in class DatabaseAccess used to return a single value
		/// </summary>
		/// <remarks>
		/// Rather than cache this single-row and column resultset in a DataSet object or open a SqlDataReader, use the ExecuteScalar method to return the value as an object. You can then convert it to the appropriate type.
		/// </remarks>
		/// <param name='SqlStatement'>A string which contains a SQL statment that returns one value (one column of one row)</param>
		/// <returns>Return value of type object.</returns>
		public object ExecuteScalar( string SqlStatement )
		{
			OracleCommand cd = new OracleCommand();
			
			try
			{
	
				cd.Connection = new OracleConnection(ConnectionString);
				cd.Connection.Open();
				cd.CommandType = CommandType.Text;
				cd.CommandText = SqlStatement;
	
				return cd.ExecuteScalar();
			}
			catch ( OracleException oracle_error )
			{
				string sErrorMessage = "";				
				for ( int i = 0; i < oracle_error.Errors.Count; i++ )
					sErrorMessage += oracle_error.Errors[i].Message + "\n";

				EventLog.WriteEntry("MIDAllocation", "Database error; ConnectionString=" + ConnectionString + ";Error=" + sErrorMessage, EventLogEntryType.Error);
				Exception err = new Exception(Include.ErrorDatabase + sErrorMessage, oracle_error);
				throw;
			}
			catch ( Exception error )
			{
				throw;
			}
			finally
			{
				if (cd.Connection != null)
				{
					cd.Connection.Close();
				}
			}
		}


		/// <summary>
		/// Executes a provided database command.
		/// </summary>
		/// <param name="OracleCmd"></param>
		/// <param name="TableName"></param>
		/// <returns>DataTable</returns>
		public DataTable ExecuteSQLQuery( string OracleCmd, string TableName )
		{
			try
			{
				OracleCommand cd = new OracleCommand(OracleCmd.Replace("@", ":"));
	
				cd.Connection = new OracleConnection(ConnectionString );
				cd.CommandType = CommandType.Text;

				DataTable dt = new DataTable( TableName );
				dt.Locale = CultureInfo.InvariantCulture;
				OracleDataAdapter sda = new OracleDataAdapter( cd );
				sda.Fill( dt );

				return dt;
			}
			catch ( OracleException oracle_error )
			{
				string sErrorMessage = "";				
				for ( int i = 0; i < oracle_error.Errors.Count; i++ )
					sErrorMessage += oracle_error.Errors[i].Message + "\n";
				
				EventLog.WriteEntry("MIDAllocation", "Database error; ConnectionString=" + ConnectionString + ";Error=" + sErrorMessage, EventLogEntryType.Error);
				Exception err = new Exception(Include.ErrorDatabase + sErrorMessage, oracle_error);
				throw;
			}
			catch ( Exception error )
			{
				throw;
			}			
		}

		/// <summary>
		/// Executes a SQL query statement with input parameters.
		/// </summary>
		/// <param name="SQLStatement"></param>
		/// <param name="TableName"></param>
		/// <param name="InputParameters"></param>
		/// <returns>DataTable</returns>
		public DataTable ExecuteSQLQuery( string OracleCmd, string TableName, DbParameter[] InputParameters )
		{
			try
			{
				OracleCommand cd = new OracleCommand(OracleCmd.Replace("@", ":"));

				if ( InputParameters != null && InputParameters.Length > 0 )
				{
					for( int i = 0; i < InputParameters.Length; i++ )
					{
						OracleParameter InParam  =  new OracleParameter();
						InParam.Direction = ParameterDirection.Input;
						InParam.ParameterName = InputParameters[i].ParameterName.Replace("@", "IN_");
						if (InputParameters[i].Value == null)
							InParam.Value = DBNull.Value;
						else
							InParam.Value = InputParameters[i].Value;
						switch (InputParameters[i].DbType)
						{
							case eDbType.Bit:
								InParam.OracleDbType = OracleDbType.Char;
								if (InputParameters[i].Value.GetType() == System.Type.GetType("System.Boolean.FalseString"))
								{
									InParam.Value = 0;
								}
								else
								{
									InParam.Value = 1;
								}
								break;
							case eDbType.Char:
								InParam.OracleDbType = OracleDbType.Char;
								break;
							case eDbType.DateTime:
								InParam.OracleDbType = OracleDbType.Date;
								break;
							case eDbType.Float:
								InParam.OracleDbType = OracleDbType.Double;
								break;
							case eDbType.Int:
								InParam.OracleDbType = OracleDbType.Int32;
								break;
							case eDbType.SmallDateTime:
								InParam.OracleDbType = OracleDbType.Date;
								break;
							case eDbType.VarChar:
								InParam.OracleDbType = OracleDbType.Varchar2;
								break;
							case eDbType.Image:
								InParam.OracleDbType = OracleDbType.LongRaw;
								break;
							default:
								break;
						}
						cd.Parameters.Add( InParam );
					}
				}
			
				cd.Connection = new OracleConnection(ConnectionString );
				cd.CommandType = CommandType.Text;
				cd.CommandText = OracleCmd;

//				if ( InputParameters != null && InputParameters.Length > 0 )
//					for( int i = 0; i < InputParameters.Length; i++ )
//						cd.Parameters.Add( InputParameters[i] );

				DataTable dt = new DataTable( TableName );
				dt.Locale = CultureInfo.InvariantCulture;
				OracleDataAdapter sda = new OracleDataAdapter( cd );
				sda.Fill( dt );

				return dt;
			}
			catch ( OracleException oracle_error )
			{
				string sErrorMessage = "";				
				for ( int i = 0; i < oracle_error.Errors.Count; i++ )
					sErrorMessage += oracle_error.Errors[i].Message + "\n";
				
				EventLog.WriteEntry("MIDAllocation", "Database error; ConnectionString=" + ConnectionString + ";Error=" + sErrorMessage, EventLogEntryType.Error);
				Exception err = new Exception(Include.ErrorDatabase + sErrorMessage, oracle_error);
				throw;
			}
			catch ( Exception error )
			{
				throw;
			}	
		}

		/// <summary>
		/// Executes a stored procedure with only input parameters.
		/// </summary>
		/// <param name="ProcedureName"></param>
		/// <param name="TableName"></param>
		/// <param name="InputParameters"></param>
		/// <returns>DataTable</returns>
		public DataTable ExecuteQuery( string ProcedureName, string TableName, DbParameter[] InputParameters )
		{
			try
			{
				OracleCommand cd = new OracleCommand();

				if ( InputParameters != null && InputParameters.Length > 0 )
				{
					for( int i = 0; i < InputParameters.Length; i++ )
					{
						OracleParameter InParam  =  new OracleParameter();
						InParam.Direction = ParameterDirection.Input;
						InParam.ParameterName = InputParameters[i].ParameterName.Replace("@", "IN_");
						if (InputParameters[i].Value == null)
							InParam.Value = DBNull.Value;
						else
							InParam.Value = InputParameters[i].Value;
						switch (InputParameters[i].DbType)
						{
							case eDbType.Bit:
								InParam.OracleDbType = OracleDbType.Char;
								if (InputParameters[i].Value.GetType() == System.Type.GetType("System.Boolean.FalseString"))
								{
									InParam.Value = 0;
								}
								else
								{
									InParam.Value = 1;
								}
								break;
							case eDbType.Char:
								InParam.OracleDbType = OracleDbType.Char;
								break;
							case eDbType.DateTime:
								InParam.OracleDbType = OracleDbType.Date;
								break;
							case eDbType.Float:
								InParam.OracleDbType = OracleDbType.Double;
								break;
							case eDbType.Int:
								InParam.OracleDbType = OracleDbType.Int32;
								break;
							case eDbType.SmallDateTime:
								InParam.OracleDbType = OracleDbType.Date;
								break;
							case eDbType.VarChar:
								InParam.OracleDbType = OracleDbType.Varchar2;
								break;
							case eDbType.Image:
								InParam.OracleDbType = OracleDbType.LongRaw;
								break;
							default:
								break;
						}
						cd.Parameters.Add( InParam );
					}
				}
			
				cd.Connection = new OracleConnection(ConnectionString );
				cd.CommandType = CommandType.StoredProcedure;
				cd.CommandText = ProcedureName;

//				if ( InputParameters != null && InputParameters.Length > 0 )
//					for( int i = 0; i < InputParameters.Length; i++ )
//						cd.Parameters.Add( InputParameters[i] );

				DataTable dt = new DataTable( TableName );
				dt.Locale = CultureInfo.InvariantCulture;
				OracleDataAdapter sda = new OracleDataAdapter( cd );
				sda.Fill( dt );

				return dt;
			}
			catch ( OracleException oracle_error )
			{
				string sErrorMessage = "";				
				for ( int i = 0; i < oracle_error.Errors.Count; i++ )
					sErrorMessage += oracle_error.Errors[i].Message + "\n";
				
				EventLog.WriteEntry("MIDAllocation", "Database error; ConnectionString=" + ConnectionString + ";Error=" + sErrorMessage, EventLogEntryType.Error);
				Exception err = new Exception(Include.ErrorDatabase + sErrorMessage, oracle_error);
				throw;
			}
			catch ( Exception error )
			{
				throw;
			}	
		}

		/// <summary>
		/// Executes a stored procedure with no parameters.
		/// </summary>
		/// <param name="ProcedureName"></param>
		/// <param name="TableName"></param>
		/// <returns>DataTable</returns>
		public DataTable ExecuteQuery( string ProcedureName, string TableName )
		{
			try
			{
				OracleCommand cd = new OracleCommand();
	
				cd.Connection = new OracleConnection(ConnectionString );
				cd.CommandType = CommandType.StoredProcedure;
				cd.CommandText = ProcedureName;

				DataTable dt = new DataTable( TableName );
				dt.Locale = CultureInfo.InvariantCulture;
				OracleDataAdapter sda = new OracleDataAdapter( cd );
				sda.Fill( dt );

				return dt;
			}
			catch ( OracleException oracle_error )
			{
				string sErrorMessage = "";				
				for ( int i = 0; i < oracle_error.Errors.Count; i++ )
					sErrorMessage += oracle_error.Errors[i].Message + "\n";
				
				EventLog.WriteEntry("MIDAllocation", "Database error; ConnectionString=" + ConnectionString + ";Error=" + sErrorMessage, EventLogEntryType.Error);
				Exception err = new Exception(Include.ErrorDatabase + sErrorMessage, oracle_error);
				throw;
			}
			catch ( Exception error )
			{
				throw;
			}			
		}

		/// <summary>
		/// Executes a simple SQL statement to read data.
		/// </summary>
		/// <param name="SqlStatement"></param>
		/// <returns>DataTable</returns>
		public DataTable ExecuteQuery( string SqlStatement )
		{
			try
			{
				OracleCommand cd = new OracleCommand();
	
				cd.Connection = new OracleConnection(ConnectionString);
				cd.CommandType = CommandType.Text;
				cd.CommandText = SqlStatement.Replace("@", ":");

				DataTable dt = new DataTable( "Query Results" );
				dt.Locale = CultureInfo.InvariantCulture;
				OracleDataAdapter sda = new OracleDataAdapter( cd );
				sda.Fill( dt );

				return dt;
			}
			catch ( OracleException oracle_error )
			{
				string sErrorMessage = "";				
				for ( int i = 0; i < oracle_error.Errors.Count; i++ )
					sErrorMessage += oracle_error.Errors[i].Message + "\n";
				
				EventLog.WriteEntry("MIDAllocation", "Database error; ConnectionString=" + ConnectionString + ";Error=" + sErrorMessage, EventLogEntryType.Error);
				Exception err = new Exception(Include.ErrorDatabase + sErrorMessage, oracle_error);
				throw;
			}
			catch ( Exception error )
			{
				throw;
			}			
		}

		//		public OracleParameter[] ExecuteNonQuery(string ProcedureName, OracleParameter[] InputParameters, OracleParameter[] OutputParameters)
		//		{
		//			try
		//			{
		////				cd = new OracleCommand();
		////				cd.Connection = new OracleConnection(ConnectionString);
		//				uc.OracleCmd.Parameters.Clear();
		//				uc.OracleCmd.CommandType = CommandType.StoredProcedure;
		//				uc.OracleCmd.CommandText = ProcedureName;
		//
		//				if ( InputParameters != null && InputParameters.Length > 0 )
		//					for( int i = 0; i < InputParameters.Length; i++ )
		//						uc.OracleCmd.Parameters.Add( InputParameters[i] );
		//
		//				if ( OutputParameters != null && OutputParameters.Length > 0 )
		//					for( int i = 0; i < OutputParameters.Length; i++ )
		//						uc.OracleCmd.Parameters.Add( OutputParameters[i] );
		//				
		////				cd.Connection.Open();
		////				sqlTrans = cd.Connection.BeginTransaction();
		////				cd.Transaction = sqlTrans;
		//				uc.OracleCmd.ExecuteNonQuery();
		////				sqlTrans.Commit();
		//				
		//				ArrayList spc = new ArrayList();
		//				
		//				foreach( OracleParameter param in uc.OracleCmd.Parameters )
		//					if ( param.Direction == ParameterDirection.Output || param.Direction == ParameterDirection.InputOutput)
		//						spc.Add( param );
		//
		//				return (OracleParameter[])spc.ToArray( typeof(OracleParameter) );
		//			}
		//			catch ( OracleException oracle_error )
		//			{
		//				uc.OracleTrans.Rollback();
		//				string sErrorMessage = "";				
		//				for ( int i = 0; i < oracle_error.Errors.Count; i++ )
		//					sErrorMessage += oracle_error.Errors[i].Message + "\n";
		//				
		//				Exception err = new Exception(Include.ErrorDatabase + sErrorMessage, oracle_error);
		//				throw;
		//			}
		//			catch ( Exception error )
		//			{
		//				throw;
		//			}
		//			finally
		//			{
		////				if (cd.Connection != null)
		////				{
		////					cd.Connection.Close();
		////				}
		//			}	
		//		}

		//		public OracleParameter[] ExecuteNonQuery(string ProcedureName, OracleParameter[] InputParameters )
		//		{
		////			OracleTransaction sqlTrans = null;
		////			OracleCommand cd = null;
		//			try
		//			{
		////				cd = new OracleCommand();
		////	
		////				cd.Connection = new OracleConnection(ConnectionString);
		//				uc.OracleCmd.Parameters.Clear();
		//				uc.OracleCmd.CommandType = CommandType.StoredProcedure;
		//				uc.OracleCmd.CommandText = ProcedureName;
		//
		//				if ( InputParameters != null && InputParameters.Length > 0 )
		//					for( int i = 0; i < InputParameters.Length; i++ )
		//						uc.OracleCmd.Parameters.Add( InputParameters[i] );
		//
		////				cd.Connection.Open();
		////				sqlTrans = cd.Connection.BeginTransaction();
		////				cd.Transaction = sqlTrans;
		//				uc.OracleCmd.ExecuteNonQuery();
		////				sqlTrans.Commit();
		//
		//				return null;
		//			}
		//			catch ( OracleException oracle_error )
		//			{
		//				uc.OracleTrans.Rollback();
		//				if (uc.OracleCmd.Connection != null)
		//				{
		//					uc.OracleCmd.Connection.Close();
		//				}
		//				string sErrorMessage = "";				
		//				for ( int i = 0; i < oracle_error.Errors.Count; i++ )
		//					sErrorMessage += oracle_error.Errors[i].Message + "\n";
		//				
		//				Exception err = new Exception(Include.ErrorDatabase + sErrorMessage, oracle_error);
		//				throw;
		//			}
		//			catch ( Exception error )
		//			{
		//				throw;
		//			}
		////			finally
		////			{
		////				if (cd.Connection != null)
		////				{
		////					cd.Connection.Close();
		////				}
		////			}
		//		}

		//		public OracleParameter[] ExecuteNonQuery(string ProcedureName )
		//		{
		////			OracleTransaction sqlTrans = null;
		////			OracleCommand cd = null;
		//			try
		//			{
		////				cd = new OracleCommand();
		////	
		////				cd.Connection = new OracleConnection(ConnectionString);
		//				uc.OracleCmd.Parameters.Clear();
		//				uc.OracleCmd.CommandType = CommandType.StoredProcedure;
		//				uc.OracleCmd.CommandText = ProcedureName;
		//
		////				cd.Connection.Open();
		////				sqlTrans = cd.Connection.BeginTransaction();
		////				cd.Transaction = sqlTrans;
		//				uc.OracleCmd.ExecuteNonQuery();
		////				sqlTrans.Commit();
		//
		//				return null;
		//			}
		//			catch ( OracleException oracle_error )
		//			{
		//				uc.OracleTrans.Rollback();
		//				if (uc.OracleCmd.Connection != null)
		//				{
		//					uc.OracleCmd.Connection.Close();
		//				}
		//				string sErrorMessage = "";				
		//				for ( int i = 0; i < oracle_error.Errors.Count; i++ )
		//					sErrorMessage += oracle_error.Errors[i].Message + "\n";
		//				
		//				Exception err = new Exception(Include.ErrorDatabase + sErrorMessage, oracle_error);
		//				throw;
		//			}
		//			catch ( Exception error )
		//			{
		//				throw;
		//			}
		////			finally
		////			{
		////				if (cd.Connection != null)
		////				{
		////					cd.Connection.Close();
		////				}
		////			}	
		//		}

		/// <summary>
		/// Inserts a record into the database.
		/// </summary>
		/// <param name="SQLStatement"></param>
		/// <param name="InputParameters"></param>
		/// <param name="OutputParameters"></param>
		/// <returns>Record ID of the inserted record</returns>
		public int ExecuteInsert(string SQLStatement, DbParameter[] InputParameters, DbParameter[] OutputParameters)
		{
			try
			{
				int RID = -1;

				_updateConnection.OracleCmd.Parameters.Clear();
				_updateConnection.OracleCmd.CommandType = CommandType.Text;
				_updateConnection.OracleCmd.CommandText = SQLStatement.Replace("@", ":");

				if ( InputParameters != null && InputParameters.Length > 0 )
				{
					for( int i = 0; i < InputParameters.Length; i++ )
					{
						OracleParameter InParam  =  new OracleParameter();
						InParam.Direction = ParameterDirection.Input;
						InParam.ParameterName = InputParameters[i].ParameterName.Replace("@", ":");
						if (InputParameters[i].Value == null)
							InParam.Value = DBNull.Value;
						else
							InParam.Value = InputParameters[i].Value;
						switch (InputParameters[i].DbType)
						{
							case eDbType.Bit:
								InParam.OracleDbType = OracleDbType.Char;
								if (InputParameters[i].Value.GetType() == System.Type.GetType("System.Boolean.FalseString"))
								{
									InParam.Value = 0;
								}
								else
								{
									InParam.Value = 1;
								}
								break;
							case eDbType.Char:
								InParam.OracleDbType = OracleDbType.Char;
								break;
							case eDbType.DateTime:
								InParam.OracleDbType = OracleDbType.Date;
								break;
							case eDbType.Float:
								InParam.OracleDbType = OracleDbType.Double;
								break;
							case eDbType.Int:
								InParam.OracleDbType = OracleDbType.Int32;
								break;
							case eDbType.SmallDateTime:
								InParam.OracleDbType = OracleDbType.Date;
								break;
							case eDbType.VarChar:
								InParam.OracleDbType = OracleDbType.Varchar2;
								break;
							case eDbType.Decimal:
								InParam.OracleDbType = OracleDbType.Decimal;
								break;
							case eDbType.Image:
								InParam.OracleDbType = OracleDbType.LongRaw;
								break;
							default:
								break;
						}
						_updateConnection.OracleCmd.Parameters.Add( InParam );
					}
				}

				if ( OutputParameters != null && OutputParameters.Length > 0 )
				{
					for( int i = 0; i < OutputParameters.Length; i++ )
					{
						OracleParameter OutParam  =  new OracleParameter();
						OutParam.Direction = ParameterDirection.Output;
						OutParam.ParameterName = OutputParameters[i].ParameterName.Replace("@", ":");
						OutParam.Value = OutputParameters[i].Value;
						switch (OutputParameters[i].DbType)
						{
							case eDbType.Bit:
								OutParam.OracleDbType = OracleDbType.Char;
								if (InputParameters[i].Value.GetType() == System.Type.GetType("System.Boolean.FalseString"))
								{
									OutParam.Value = 0;
								}
								else
								{
									OutParam.Value = 1;
								}
								break;
							case eDbType.Char:
								OutParam.OracleDbType = OracleDbType.Char;
								break;
							case eDbType.DateTime:
								OutParam.OracleDbType = OracleDbType.Date;
								break;
							case eDbType.Float:
								OutParam.OracleDbType = OracleDbType.Double;
								break;
							case eDbType.Int:
								OutParam.OracleDbType = OracleDbType.Int32;
								break;
							case eDbType.SmallDateTime:
								OutParam.OracleDbType = OracleDbType.Date;
								break;
							case eDbType.VarChar:
								OutParam.OracleDbType = OracleDbType.Varchar2;
								break;
							case eDbType.Image:
								OutParam.OracleDbType = OracleDbType.LongRaw;
								break;
							default:
								break;
						}
						_updateConnection.OracleCmd.Parameters.Add( OutParam );
					}
				}
				
				
				_updateConnection.OracleCmd.ExecuteNonQuery();
				
//				ArrayList spc = new ArrayList();
				
				foreach( OracleParameter param in _updateConnection.OracleCmd.Parameters )
				{
					if ( param.Direction == ParameterDirection.ReturnValue || param.Direction == ParameterDirection.Output || param.Direction == ParameterDirection.InputOutput)
						RID = (int)param.Value;
				}
				

				return RID;
			}
			catch ( OracleException oracle_error )
			{
				_updateConnection.OracleTrans.Rollback();
				string sErrorMessage = "";				
				for ( int i = 0; i < oracle_error.Errors.Count; i++ )
					sErrorMessage += oracle_error.Errors[i].Message + "\n";
				
				EventLog.WriteEntry("MIDAllocation", "Database error; ConnectionString=" + ConnectionString + ";Error=" + sErrorMessage, EventLogEntryType.Error);
				Exception err = new Exception(Include.ErrorDatabase + sErrorMessage, oracle_error);
				throw;
			}
			catch ( Exception error )
			{
				throw;
			}
//			finally
//			{
//
//			}	
		}

		/// <summary>
		/// Executes a non-query SQL statement.
		/// </summary>
		/// <param name="SQLStatement"></param>
		public int ExecuteNonQuery(string SQLStatement)
		{
			try
			{
				_updateConnection.OracleCmd.Parameters.Clear();
				_updateConnection.OracleCmd.CommandType = CommandType.Text;
				_updateConnection.OracleCmd.CommandText = SQLStatement.Replace("@", ":");

				return _updateConnection.OracleCmd.ExecuteNonQuery();
				
//				ArrayList spc = new ArrayList();
				
			}
			catch ( OracleException oracle_error )
			{
				_updateConnection.OracleTrans.Rollback();
				string sErrorMessage = "";				
				for ( int i = 0; i < oracle_error.Errors.Count; i++ )
					sErrorMessage += oracle_error.Errors[i].Message + "\n";
				
				EventLog.WriteEntry("MIDAllocation", "Database error; ConnectionString=" + ConnectionString + ";Error=" + sErrorMessage, EventLogEntryType.Error);
				throw new Exception(Include.ErrorDatabase + sErrorMessage, oracle_error);
			}
			catch ( Exception error )
			{
				throw;
			}
//			finally
//			{
//
//			}	
		}

		/// <summary>
		/// Executes a nonquery SQL statement that requires input parameters.
		/// </summary>
		/// <param name="SQLStatement"></param>
		/// <param name="InputParameters"></param>
		public int ExecuteNonQuery(string SQLStatement, DbParameter[] InputParameters)
		{
			try
			{
				_updateConnection.OracleCmd.Parameters.Clear();
				_updateConnection.OracleCmd.CommandType = CommandType.Text;
				_updateConnection.OracleCmd.CommandText = SQLStatement.Replace("@", ":");

				if ( InputParameters != null && InputParameters.Length > 0 )
				{
					for( int i = 0; i < InputParameters.Length; i++ )
					{
						OracleParameter InParam  =  new OracleParameter();
						InParam.Direction = ParameterDirection.Input;
						InParam.ParameterName = InputParameters[i].ParameterName.Replace("@", ":");
						if (InputParameters[i].Value == null)
							InParam.Value = DBNull.Value;
						else
							InParam.Value = InputParameters[i].Value;
						switch (InputParameters[i].DbType)
						{
							case eDbType.Bit:
								InParam.OracleDbType = OracleDbType.Char;
								if (InputParameters[i].Value.GetType() == System.Type.GetType("System.Boolean.FalseString"))
								{
									InParam.Value = 0;
								}
								else
								{
									InParam.Value = 1;
								}
								break;
							case eDbType.Char:
								InParam.OracleDbType = OracleDbType.Char;
								break;
							case eDbType.DateTime:
								InParam.OracleDbType = OracleDbType.Date;
								break;
							case eDbType.Float:
								InParam.OracleDbType = OracleDbType.Double;
								break;
							case eDbType.Int:
								InParam.OracleDbType = OracleDbType.Int32;
								break;
							case eDbType.SmallDateTime:
								InParam.OracleDbType = OracleDbType.Date;
								break;
							case eDbType.VarChar:
								InParam.OracleDbType = OracleDbType.Varchar2;
								break;
							case eDbType.Image:
								InParam.OracleDbType = OracleDbType.LongRaw;
								break;
							default:
								break;
						}
						_updateConnection.OracleCmd.Parameters.Add( InParam );
					}
				}

				return _updateConnection.OracleCmd.ExecuteNonQuery();
				
//				ArrayList spc = new ArrayList();
				
			}
			catch ( OracleException oracle_error )
			{
				_updateConnection.OracleTrans.Rollback();
				string sErrorMessage = "";				
				for ( int i = 0; i < oracle_error.Errors.Count; i++ )
					sErrorMessage += oracle_error.Errors[i].Message + "\n";
				
				EventLog.WriteEntry("MIDAllocation", "Database error; ConnectionString=" + ConnectionString + ";Error=" + sErrorMessage, EventLogEntryType.Error);
				Exception err = new Exception(Include.ErrorDatabase + sErrorMessage, oracle_error);
				throw;
			}
			catch ( Exception error )
			{
				throw;
			}
//			finally
//			{
//
//			}	
		}

		/// <summary>
		/// Executes stores procedures requiring input parameters. 
		/// </summary>
		/// <param name="ProcedureName"></param>
		/// <param name="InputParameters"></param>
		/// <param name="OutputParameters"></param>
		/// <returns>int</returns>
		public int ExecuteStoredProcedure(string ProcedureName, DbParameter[] InputParameters, DbParameter[] OutputParameters)
		{
			try
			{
				int RID = -1;
				_updateConnection.OracleCmd.Parameters.Clear();
				_updateConnection.OracleCmd.CommandType = CommandType.StoredProcedure;
				_updateConnection.OracleCmd.CommandText = ProcedureName;

				if ( InputParameters != null && InputParameters.Length > 0 )
				{
					for( int i = 0; i < InputParameters.Length; i++ )
					{
						OracleParameter InParam  =  new OracleParameter();
						InParam.Direction = ParameterDirection.Input;
						InParam.ParameterName = InputParameters[i].ParameterName.Replace("@", "IN_");
						if (InputParameters[i].Value == null)
							InParam.Value = DBNull.Value;
						else
							InParam.Value = InputParameters[i].Value;
						switch (InputParameters[i].DbType)
						{
							case eDbType.Bit:
								InParam.OracleDbType = OracleDbType.Char;
								if (InputParameters[i].Value.GetType() == System.Type.GetType("System.Boolean.FalseString"))
								{
									InParam.Value = 0;
								}
								else
								{
									InParam.Value = 1;
								}
								break;
							case eDbType.Char:
								InParam.OracleDbType = OracleDbType.Char;
								break;
							case eDbType.DateTime:
								InParam.OracleDbType = OracleDbType.Date;
								break;
							case eDbType.Float:
								InParam.OracleDbType = OracleDbType.Double;
								break;
							case eDbType.Int:
								InParam.OracleDbType = OracleDbType.Int32;
								break;
							case eDbType.SmallDateTime:
								InParam.OracleDbType = OracleDbType.Date;
								break;
							case eDbType.VarChar:
								InParam.OracleDbType = OracleDbType.Varchar2;
								break;
							case eDbType.Image:
								InParam.OracleDbType = OracleDbType.LongRaw;
								break;
							default:
								break;
						}
						_updateConnection.OracleCmd.Parameters.Add( InParam );
					}
				}

				if ( OutputParameters != null && OutputParameters.Length > 0 )
				{
					for( int i = 0; i < OutputParameters.Length; i++ )
					{
						OracleParameter OutParam  =  new OracleParameter();
						OutParam.Direction = ParameterDirection.Output;
						OutParam.ParameterName = OutputParameters[i].ParameterName.Replace("@", "OUT_");
						OutParam.Value = OutputParameters[i].Value;
						switch (OutputParameters[i].DbType)
						{
							case eDbType.Bit:
								OutParam.OracleDbType = OracleDbType.Char;
								if (InputParameters[i].Value.GetType() == System.Type.GetType("System.Boolean.FalseString"))
								{
									OutParam.Value = 0;
								}
								else
								{
									OutParam.Value = 1;
								}
								break;
							case eDbType.Char:
								OutParam.OracleDbType = OracleDbType.Char;
								break;
							case eDbType.DateTime:
								OutParam.OracleDbType = OracleDbType.Date;
								break;
							case eDbType.Float:
								OutParam.OracleDbType = OracleDbType.Double;
								break;
							case eDbType.Int:
								OutParam.OracleDbType = OracleDbType.Int32;
								break;
							case eDbType.SmallDateTime:
								OutParam.OracleDbType = OracleDbType.Date;
								break;
							case eDbType.VarChar:
								OutParam.OracleDbType = OracleDbType.Varchar2;
								break;
							case eDbType.Image:
								OutParam.OracleDbType = OracleDbType.LongRaw;
								break;
							default:
								break;
						}
						_updateConnection.OracleCmd.Parameters.Add( OutParam );
					}
				}
				
				_updateConnection.OracleCmd.ExecuteNonQuery();
				
//				ArrayList spc = new ArrayList();
				
				foreach( OracleParameter param in _updateConnection.OracleCmd.Parameters )
					if ( param.Direction == ParameterDirection.ReturnValue || param.Direction == ParameterDirection.Output || param.Direction == ParameterDirection.InputOutput)
					{
						RID = (int)param.Value;
					}

				return RID;
			}
			catch ( OracleException oracle_error )
			{
				_updateConnection.OracleTrans.Rollback();
				string sErrorMessage = "";				
				for ( int i = 0; i < oracle_error.Errors.Count; i++ )
					sErrorMessage += oracle_error.Errors[i].Message + "\n";
				
				EventLog.WriteEntry("MIDAllocation", "Database error; ConnectionString=" + ConnectionString + ";Error=" + sErrorMessage, EventLogEntryType.Error);
				Exception err = new Exception(Include.ErrorDatabase + sErrorMessage, oracle_error);
				throw;
			}
			catch ( Exception error )
			{
				throw;
			}
//			finally
//			{
//
//			}	
		}

		/// <summary>
		/// Executes stored procedure requiring input parameters.
		/// </summary>
		/// <param name="ProcedureName"></param>
		/// <param name="InputParameters"></param>
		/// <returns>DbParameter[]</returns>
		public DbParameter[] ExecuteStoredProcedure(string ProcedureName, DbParameter[] InputParameters )
		{
			try
			{
				_updateConnection.OracleCmd.Parameters.Clear();
				_updateConnection.OracleCmd.CommandType = CommandType.StoredProcedure;
				_updateConnection.OracleCmd.CommandText = ProcedureName;

				if ( InputParameters != null && InputParameters.Length > 0 )
				{
					for( int i = 0; i < InputParameters.Length; i++ )
					{
						OracleParameter InParam  =  new OracleParameter();
						InParam.Direction = ParameterDirection.Input;
						InParam.ParameterName = InputParameters[i].ParameterName.Replace("@", "IN_");
						if (InputParameters[i].Value == null)
							InParam.Value = DBNull.Value;
						else
							InParam.Value = InputParameters[i].Value;
						switch (InputParameters[i].DbType)
						{
							case eDbType.Bit:
								InParam.OracleDbType = OracleDbType.Char;
								if (InputParameters[i].Value.GetType() == System.Type.GetType("System.Boolean.FalseString"))
								{
									InParam.Value = 0;
								}
								else
								{
									InParam.Value = 1;
								}
								break;
							case eDbType.Char:
								InParam.OracleDbType = OracleDbType.Char;
								break;
							case eDbType.DateTime:
								InParam.OracleDbType = OracleDbType.Date;
								break;
							case eDbType.Float:
								InParam.OracleDbType = OracleDbType.Double;
								break;
							case eDbType.Int:
								InParam.OracleDbType = OracleDbType.Int32;
								break;
							case eDbType.SmallDateTime:
								InParam.OracleDbType = OracleDbType.Date;
								break;
							case eDbType.VarChar:
								InParam.OracleDbType = OracleDbType.Varchar2;
								break;
							case eDbType.Image:
								InParam.OracleDbType = OracleDbType.LongRaw;
								break;
							default:
								break;
						}
						_updateConnection.OracleCmd.Parameters.Add( InParam );
					}
				}

				_updateConnection.OracleCmd.ExecuteNonQuery();

				return null;
			}
			catch ( OracleException oracle_error )
			{
				_updateConnection.OracleTrans.Rollback();
				if (_updateConnection.OracleCmd.Connection != null)
				{
					_updateConnection.OracleCmd.Connection.Close();
				}
				string sErrorMessage = "";				
				for ( int i = 0; i < oracle_error.Errors.Count; i++ )
					sErrorMessage += oracle_error.Errors[i].Message + "\n";
				
				EventLog.WriteEntry("MIDAllocation", "Database error; ConnectionString=" + ConnectionString + ";Error=" + sErrorMessage, EventLogEntryType.Error);
				Exception err = new Exception(Include.ErrorDatabase + sErrorMessage, oracle_error);
				throw;
			}
			catch ( Exception error )
			{
				throw;
			}
		}

		/// <summary>
		/// Executes stored procedure requiring no parameters.
		/// </summary>
		/// <param name="ProcedureName"></param>
		/// <returns></returns>
		public DbParameter[] ExecuteStoredProcedure(string ProcedureName )
		{
			try
			{

				_updateConnection.OracleCmd.Parameters.Clear();
				_updateConnection.OracleCmd.CommandType = CommandType.StoredProcedure;
				_updateConnection.OracleCmd.CommandText = ProcedureName;

				_updateConnection.OracleCmd.ExecuteNonQuery();

				return null;
			}
			catch ( OracleException oracle_error )
			{
				_updateConnection.OracleTrans.Rollback();
				if (_updateConnection.OracleCmd.Connection != null)
				{
					_updateConnection.OracleCmd.Connection.Close();
				}
				string sErrorMessage = "";				
				for ( int i = 0; i < oracle_error.Errors.Count; i++ )
					sErrorMessage += oracle_error.Errors[i].Message + "\n";
				
				EventLog.WriteEntry("MIDAllocation", "Database error; ConnectionString=" + ConnectionString + ";Error=" + sErrorMessage, EventLogEntryType.Error);
				Exception err = new Exception(Include.ErrorDatabase + sErrorMessage, oracle_error);
				throw;
			}
			catch ( Exception error )
			{
				throw;
			}
		}
	
		/// <summary>
		/// Creates new database adapter.
		/// </summary>
		/// <param name="tableName"></param>
		/// <returns></returns>
		public DBAdapter NewDBAdapter(string tableName)
		{
			DBAdapter ad;
			OracleConnection cn = new OracleConnection(ConnectionString);
			//cn.Open();

			ad = new DBAdapter(tableName, cn);
			return ad;
		}
	}
}

