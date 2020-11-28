using System;
using System.Collections;
//using System.Configuration;
using System.Data;
//using System.Data.OracleClient;
using Oracle.DataAccess.Client;
using Oracle.DataAccess.Types;
using MID.MRS.DataCommon;

namespace MID.MRS.Data
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
		OracleDataAdapter _ad;
		OracleConnection _cn;

		public DBAdapter(string tableName, OracleConnection cn)
		{
			_cn = cn;

			_ad = new OracleDataAdapter();
			_ad.TableMappings.Add("Table", tableName);
		}
		
		public void SelectCommand(string select)
		{
			_cn.Open();
			OracleCommand selectCommand = new OracleCommand(select,	_cn);
			selectCommand.CommandType = CommandType.Text;
			_ad.SelectCommand = selectCommand;
			_cn.Close();
		}

		public void InsertCommand(string storedProcedure, DbParameter[] InputParameters, DbParameter[] OutputParameters)
		{
			try
			{
				_cn.Open();

				OracleCommand insertCommand = new OracleCommand(storedProcedure, _cn);
				insertCommand.CommandType = CommandType.StoredProcedure;
				string paramReplaceChar = "IN_";
				if ( InputParameters != null && InputParameters.Length > 0 )
				{
					for( int i = 0; i < InputParameters.Length; i++ )
					{
						OracleParameter inParam = BuildParameter(InputParameters[i], 
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
						OracleParameter outParam = BuildParameter(OutputParameters[i],
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
				throw;
			}
		}

		public void InsertCommand(string sqlStatement, DbParameter[] InputParameters)
		{
			try
			{
				_cn.Open();

				OracleCommand insertCommand = new OracleCommand(sqlStatement.Replace("@", ":"), _cn);
				insertCommand.CommandType = CommandType.Text;
				string paramReplaceChar = ":";
				if ( InputParameters != null && InputParameters.Length > 0 )
				{
					for( int i = 0; i < InputParameters.Length; i++ )
					{
						OracleParameter inParam = BuildParameter(InputParameters[i], 
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
				throw;
			}
		}

		public void UpdateCommand(string sqlStatement, DbParameter[] InputParameters)
		{
			try
			{
				_cn.Open();

				OracleCommand updateCommand = new OracleCommand(sqlStatement.Replace("@", ":"), _cn);
				updateCommand.CommandType = CommandType.Text;

				string paramReplaceChar = ":";
				if ( InputParameters != null && InputParameters.Length > 0 )
				{
					for( int i = 0; i < InputParameters.Length; i++ )
					{
						OracleParameter inParam = BuildParameter(InputParameters[i],
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
				throw;
			}
		}

		public void DeleteCommand(string sqlStatement, DbParameter[] InputParameters)
		{
			try
			{
				_cn.Open();

				OracleCommand deleteCommand = new OracleCommand(sqlStatement.Replace("@", ":"), _cn);
				deleteCommand.CommandType = CommandType.Text;

				string paramReplaceChar = ":";
				if ( InputParameters != null && InputParameters.Length > 0 )
				{
					for( int i = 0; i < InputParameters.Length; i++ )
					{
						OracleParameter inParam = BuildParameter(InputParameters[i],
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
				throw;
			}
		}

		public OracleParameter BuildParameter(DbParameter DbParam, string replaceChar, eParameterDirection direction)
		{
			OracleParameter oraParam  =  new OracleParameter();
			if (direction == eParameterDirection.Input)
				oraParam.Direction = ParameterDirection.Input;
			else if (direction == eParameterDirection.Output)
				oraParam.Direction = ParameterDirection.Output;
			else 
				oraParam.Direction = ParameterDirection.InputOutput;
			oraParam.ParameterName = DbParam.ParameterName.Replace("@", replaceChar);
			oraParam.Size = DbParam.Size;
			oraParam.SourceColumn = DbParam.ParamFieldName;
			switch (DbParam.DbType)
			{
				case eDbType.Bit:
					oraParam.OracleDbType = OracleDbType.Char;
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
					oraParam.OracleDbType = OracleDbType.Char;
					break;
				case eDbType.DateTime:
					oraParam.OracleDbType = OracleDbType.Date;
					break;
				case eDbType.Float:
					oraParam.OracleDbType = OracleDbType.Single;
					break;
				case eDbType.Int:
					oraParam.OracleDbType = OracleDbType.Int32;
					break;
				case eDbType.SmallDateTime:
					oraParam.OracleDbType = OracleDbType.Date;
					break;
				case eDbType.VarChar:
					oraParam.OracleDbType = OracleDbType.Varchar2;
					break;
				case eDbType.Single:
					oraParam.OracleDbType = OracleDbType.Single;
					break;
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
			}
			catch (Exception err)
			{
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
				throw;
			}
		}
	}
}
