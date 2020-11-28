using System;
using System.Collections;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Globalization;

using MIDRetail.DataCommon;

namespace MIDRetail.Data
{
	public class ComputationsNotLoadedException : Exception
	{
	}

	public partial class ComputationsDLLData : DataLayer
	{
		public ComputationsDLLData()
			: base()
		{

		}

		public ComputationsDLLData(string aConnectionString)
			: base(aConnectionString)
		{

		}

		public bool ComputationsDLL_Exists()
		{
			try
			{
                int rowCount = StoredProcedures.MID_COMPUTATIONS_DLL_READ_COUNT.ReadRecordCount(_dba);
                return (rowCount > 0);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		public bool ComputationsDLL_Exists(string aVersion)
		{
			try
			{
                int rowCount = StoredProcedures.MID_COMPUTATIONS_DLL_READ_COUNT_FROM_VERSION.ReadRecordCount(_dba, VERSION: aVersion);
                return (rowCount > 0);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		public string ComputationsDLL_ReadLatestVersion()
		{
			DataTable dt;

			try
			{
                dt = StoredProcedures.MID_COMPUTATIONS_DLL_READ_LATEST_VERSION.Read(_dba);
				if (dt.Rows.Count > 0)
				{
					return Convert.ToString(dt.Rows[0]["VERSION"]);
				}
				else
				{
					throw new ComputationsNotLoadedException();
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		public byte[] ComputationsDLL_ReadLatestDLL()
		{
			DataTable dt;

			try
			{
                dt = StoredProcedures.MID_COMPUTATIONS_DLL_READ_LATEST_IMAGE.Read(_dba);
				if (dt.Rows.Count > 0)
				{
					return (byte[])dt.Rows[0]["DLL_IMAGE"];
				}
				else
				{
					throw new ComputationsNotLoadedException();
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		public byte[] ComputationsDLL_ReadDLL(string aVersion)
		{
			DataTable dt;

			try
			{
                dt = StoredProcedures.MID_COMPUTATIONS_DLL_READ_IMAGE_FROM_VERSION.Read(_dba, VERSION: aVersion);

				if (dt.Rows.Count == 1)
				{
					return (byte[])dt.Rows[0]["DLL_IMAGE"];
				}
				else
				{
					throw new ComputationsNotLoadedException();
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}



		public void ComputationsDLL_Insert(string aVersion, byte[] aComputationsDLL)
		{
			try
			{
                StoredProcedures.MID_COMPUTATIONS_DLL_INSERT.Insert(_dba,
                                                                    VERSION: aVersion,
                                                                    DLL_IMAGE: aComputationsDLL,
                                                                    CREATION_DATE: DateTime.Now
                                                                    );
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		public DataTable ComputationsDLL_GetTables(string aSQLCommand)
		{
			
			try
			{
				return _dba.ExecuteSQLQuery(aSQLCommand, "Tables");
			}
			catch 
			{
				throw;
			}
		}

		public DataTable ComputationsDLL_ReadTableDefn(string aTableName)
		{
			string SQLCommand;
			
			try
			{
				SQLCommand = "SELECT TOP 1 * FROM " + aTableName;

				return _dba.ExecuteSQLQuery(SQLCommand, "TableDefn");
			}
			catch 
			{
				throw;
			}
		}

		public void ComputationsDLL_AddColumn(string aCommand)
		{
			try
			{
				_dba.ExecuteNonQuery(aCommand);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

        
		public string ComputationsDLL_GetDefaultConstraintName(string aTableName, string aColumnName)
		{
			DataTable dt;
			DataRow dr;
			string constraintName = null;
			
			try
			{   // MID Track #5632 Added (NOLOCK) to SELECT text - RonM - 6-18-2008 
                dt = StoredProcedures.MID_COMPUTATIONS_DLL_READ_DEFAULT_CONSTRAINT.Read(_dba,
                                                                                          TABLE_NAME: aTableName,
                                                                                          COLUMN_NAME: aColumnName
                                                                                          );
				if (dt.Rows.Count > 0)
				{
					dr = dt.Rows[0];
					constraintName = Convert.ToString(dr["CONSTRAINT_NAME"]);
				}

				return constraintName;
			}
			catch 
			{
				throw;
			}
		}
		// End Track #4637
	}
}
