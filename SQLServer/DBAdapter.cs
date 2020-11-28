using System;
//using System;
using System.Collections;
//using System.Configuration;
using System.Data;
using MIDRetail.DataCommon;
using System.Data.SqlClient;

//using MIDRetail.Common;

namespace MIDRetail.Data
{
	/// <summary>
	/// DBAdapter.
	/// 
	/// How to use:
	/// Instantiate a new DBAdapter (thru the DataAccess Class).
	/// call the following methods
	///		SelectCommand
	///		InsertCommand
	///		DeleteCommand
	///		UpdateCommand
	///		Instantiate a new DataSet
	///		Send the DataSet to the DBAdapter.Fill() method
	///		
	///	Now the DataSet should be filled and ready to use.
	///	
	///	Once changes has been made to the data within the Dataset,
	///	send a DataTable with the changes to the DBAdapter.UpdateTable() method.
	///		
	/// </summary>
	public class DBAdapter
	{
		SqlDataAdapter _ad;
		SqlConnection _cn;
		int _commandTimeout;	// Issue 5018 stodd 12.24.2007

		public DBAdapter(string tableName, SqlConnection cn, int commandTimeout)	// Issue 5018 stodd 12.24.2007
		{
			_cn = cn;
			_commandTimeout = commandTimeout;	// Issue 5018 stodd 12.24.2007
			_ad = new SqlDataAdapter();
			_ad.TableMappings.Add("Table", tableName);
		}
		
		public void SelectCommand(string select)
		{
			_cn.Open();
			SqlCommand selectCommand = new SqlCommand(select,	_cn);
			selectCommand.CommandType = CommandType.Text;
			selectCommand.CommandTimeout = _commandTimeout;		// Issue 5018 stodd 12.24.2007
			_ad.SelectCommand = selectCommand;
			_cn.Close();
		}

		public void InsertCommand(string storedProcedure, MIDDbParameter[] InputParameters, MIDDbParameter[] OutputParameters)
		{
			try
			{
				_cn.Open();

				SqlCommand insertCommand = new SqlCommand(storedProcedure, _cn);
				insertCommand.CommandTimeout = _commandTimeout;		// Issue 5018 stodd 12.24.2007
				insertCommand.CommandType = CommandType.StoredProcedure;
				string paramReplaceChar = "IN_";
				if ( InputParameters != null && InputParameters.Length > 0 )
				{
					for( int i = 0; i < InputParameters.Length; i++ )
					{
						SqlParameter inParam = BuildParameter(InputParameters[i], 
																paramReplaceChar,
																eParameterDirection.Input);
						insertCommand.Parameters.Add( inParam );
					}
				}
			
				paramReplaceChar = "OUT_";
				if ( OutputParameters != null && OutputParameters.Length > 0 )
				{
					for( int i = 0; i < OutputParameters.Length; i++ )
					{
						SqlParameter outParam = BuildParameter(OutputParameters[i],
																paramReplaceChar,
																eParameterDirection.Output);
						insertCommand.Parameters.Add (outParam );
					}
				}
				_ad.InsertCommand = insertCommand;

				_cn.Close();
			}
			catch (Exception err)
			{
				string message = err.ToString();
				throw;
			}
		}

		public void InsertCommand(string sqlStatement, MIDDbParameter[] InputParameters)
		{
			try
			{
				_cn.Open();

				//SqlCommand insertCommand = new SqlCommand(sqlStatement.Replace("@", ":"), _cn);
				SqlCommand insertCommand = new SqlCommand(sqlStatement, _cn);
				insertCommand.CommandTimeout = _commandTimeout;		// Issue 5018 stodd 12.24.2007

				insertCommand.CommandType = CommandType.Text;
				string paramReplaceChar = ":";
				if ( InputParameters != null && InputParameters.Length > 0 )
				{
					for( int i = 0; i < InputParameters.Length; i++ )
					{
						SqlParameter inParam = BuildParameter(InputParameters[i], 
							paramReplaceChar,
							eParameterDirection.Input);
						insertCommand.Parameters.Add( inParam );
					}
				}
				_ad.InsertCommand = insertCommand;

				_cn.Close();
			}
			catch (Exception err)
			{
				string message = err.ToString();
				throw;
			}
		}

		public void UpdateCommand(string sqlStatement, MIDDbParameter[] InputParameters)
		{
			try
			{
				_cn.Open();

				//SqlCommand updateCommand = new SqlCommand(sqlStatement.Replace("@", ":"), _cn);
				SqlCommand updateCommand = new SqlCommand(sqlStatement, _cn);
				updateCommand.CommandTimeout = _commandTimeout;		// Issue 5018 stodd 12.24.2007

				updateCommand.CommandType = CommandType.Text;

				string paramReplaceChar = ":";
				if ( InputParameters != null && InputParameters.Length > 0 )
				{
					for( int i = 0; i < InputParameters.Length; i++ )
					{
						SqlParameter inParam = BuildParameter(InputParameters[i],
																paramReplaceChar,	
																eParameterDirection.Input);
						updateCommand.Parameters.Add( inParam );
					}
				}

				_ad.UpdateCommand = updateCommand;

				_cn.Close();
			}
			catch (Exception err)
			{
				string message = err.ToString();
				throw;
			}
		}

		public void DeleteCommand(string sqlStatement, MIDDbParameter[] InputParameters)
		{
			try
			{
				_cn.Open();

				//SqlCommand deleteCommand = new SqlCommand(sqlStatement.Replace("@", ":"), _cn);
				SqlCommand deleteCommand = new SqlCommand(sqlStatement, _cn);
				deleteCommand.CommandTimeout = _commandTimeout;		// Issue 5018 stodd 12.24.2007

				deleteCommand.CommandType = CommandType.Text;

				string paramReplaceChar = ":";
				if ( InputParameters != null && InputParameters.Length > 0 )
				{
					for( int i = 0; i < InputParameters.Length; i++ )
					{
						SqlParameter inParam = BuildParameter(InputParameters[i],
																paramReplaceChar,
																eParameterDirection.Input);
						deleteCommand.Parameters.Add( inParam );
					}
				}

				_ad.DeleteCommand = deleteCommand;

				_cn.Close();
			}
			catch (Exception err)
			{
				string message = err.ToString();
				throw;
			}
		}

		public SqlParameter BuildParameter(MIDDbParameter DbParam, string replaceChar, eParameterDirection direction)
		{
			SqlParameter oraParam  =  new SqlParameter();
			if (direction == eParameterDirection.Input)
				oraParam.Direction = ParameterDirection.Input;
			else if (direction == eParameterDirection.Output)
				oraParam.Direction = ParameterDirection.Output;
			else 
				oraParam.Direction = ParameterDirection.InputOutput;
//			oraParam.ParameterName = DbParam.ParameterName.Replace("@", replaceChar);  Do not replace in SQL Server
			oraParam.ParameterName = DbParam.ParameterName;
			oraParam.Size = DbParam.Size;
			oraParam.SourceColumn = DbParam.ParamFieldName;
			switch (DbParam.DbType)
			{
				case eDbType.Bit:
					oraParam.SqlDbType = SqlDbType.Char;
					if (DbParam.Value.GetType() == System.Type.GetType("System.Boolean.FalseString"))
					{
						oraParam.Value = 0;
					}
					else
					{
						oraParam.Value = 1;
					}
					break;
				case eDbType.Char:
					oraParam.SqlDbType = SqlDbType.Char;
					break;
				case eDbType.DateTime:
					oraParam.SqlDbType = SqlDbType.DateTime;
					break;
				case eDbType.Float:
					oraParam.SqlDbType = SqlDbType.Float;
					break;
				case eDbType.Int:
					oraParam.SqlDbType = SqlDbType.Int;
					break;
				case eDbType.SmallDateTime:
					oraParam.SqlDbType = SqlDbType.SmallDateTime;
					break;
				case eDbType.VarChar:
					oraParam.SqlDbType = SqlDbType.VarChar;
					break;
				case eDbType.Single:
					oraParam.SqlDbType = SqlDbType.Float;
					break;
                //Begin TT#827-MD -jsobek -Allocation Reviews Performance
                case eDbType.Structured:
                    oraParam.SqlDbType = SqlDbType.Structured;
                    oraParam.TypeName = DbParam.TypeName;
                    break;
                //End TT#827-MD -jsobek -Allocation Reviews Performance
				default:
					break;
			}

			return oraParam;
		}

		public void Fill(DataSet ds)
		{
			try
			{
				_ad.Fill(ds);
                foreach (DataTable dt in ds.Tables)
                {
                    MIDEnvironment.SetDataTableGlobalization(dt);
                }
			}
			catch (Exception err)
			{
				string message = err.ToString();
				throw;
			}
		}

		public void UpdateTable(DataTable xDataTable)
		{
			try
			{
				_ad.Update(xDataTable);
				
				return;
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}
	}
}
