using System;
using System.Data;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;

using MIDRetail.DataCommon;

namespace MIDRetail.Data
{
	/// <summary>
	/// Inherits MethodBase containing all properties for a Method.
	/// Adds properties for Create Master Headers
	/// </summary>
	public class CreateMasterHeadersMethodData: MethodBaseData
	{
		private int _methodRid;
		private bool _useSelectedHeaders;
        private DataTable _dtMerchandise;
        private DataTable _dtOverride;
      
		public int MethodRid
		{
			get{return _methodRid;}
			set{_methodRid = value;	}
		}

        public bool UseSelectedHeaders
		{
			get{return _useSelectedHeaders;}
			set{_useSelectedHeaders = value;	}
		}

        public DataTable dtMerchandise
		{
			get{return _dtMerchandise;}
			set{_dtMerchandise = value;	}
		}

        public DataTable dtOverride
		{
			get{return _dtOverride;}
			set{_dtOverride = value;	}
		}

     	/// <summary>
        /// Creates an instance of the CreateMasterHeadersMethodData class
		/// </summary>
		public CreateMasterHeadersMethodData()
		{
		}

		/// <summary>
        /// Creates an instance of the CreateMasterHeadersMethodData class
		/// </summary>
		/// <param name="aMethodRID">The record ID of the method</param>
        public CreateMasterHeadersMethodData(int aMethodRID, eChangeType changeType)
		{
            switch (changeType)
            {
                case eChangeType.populate:
                    PopulateCreateMasterHeaders(aMethodRID);
                    break;
            } 
		}

		/// <summary>
        /// Creates an instance of the CreateMasterHeadersMethodData class
		/// </summary>
		/// <param name="td">An instance of the TransactionData class containing the database connection.</param>
		/// <param name="aMethodRID">The record ID of the method</param>
		public CreateMasterHeadersMethodData(TransactionData td, int aMethodRID)
		{
			_dba = td.DBA;
			_methodRid = aMethodRID;
		}
        public CreateMasterHeadersMethodData(TransactionData td)
		{
			_dba = td.DBA;
			_methodRid = Include.NoRID;
		}
		
		public bool PopulateCreateMasterHeaders(int method_RID)
		{
			try
			{
                if (PopulateMethod(method_RID))
                {
                    _methodRid = method_RID;
                    DataSet dsCreateMasterHeaders = StoredProcedures.MID_METHOD_CREATE_MASTER_HEADERS_READ.ReadAsDataSet(_dba, METHOD_RID: method_RID);
                    if (dsCreateMasterHeaders.Tables.Count != 0)
                    {
                        DataRow dr = dsCreateMasterHeaders.Tables[0].Rows[0];
                        if (dr["USE_SELECTED_HEADERS"] != System.DBNull.Value)
                        {
                            _useSelectedHeaders = Include.ConvertCharToBool(Convert.ToChar(dr["USE_SELECTED_HEADERS"].ToString(), CultureInfo.CurrentUICulture));
                        }
                        SetDataTables(dsCreateMasterHeaders);
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    // Read database to build data tables
                    DataSet dsCreateMasterHeaders = StoredProcedures.MID_METHOD_CREATE_MASTER_HEADERS_READ.ReadAsDataSet(_dba, METHOD_RID: method_RID);
                    _useSelectedHeaders = false;
                    SetDataTables(dsCreateMasterHeaders);
                    
                    return false;
                }
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

        private void SetDataTables(DataSet dsCreateMasterHeaders)
        {
            _dtMerchandise = dsCreateMasterHeaders.Tables[1];
            _dtMerchandise.TableName = "Merchandise";
            _dtOverride = dsCreateMasterHeaders.Tables[2];
            _dtOverride.TableName = "Override";
        }

		public bool InsertMethod(int method_RID, TransactionData td)
		{
			bool InsertSuccessful = true;

            try
            {
                StoredProcedures.MID_METHOD_CREATE_MASTER_HEADERS_UPSERT.Insert(_dba,
                                                                           METHOD_RID: method_RID,
                                                                           USE_SELECTED_HEADERS: Include.ConvertBoolToChar(UseSelectedHeaders),
                                                                           MERCHANDISE_TABLE: dtMerchandise,
                                                                           OVERRIDE_TABLE: dtOverride
                                                                           );
            }
            catch (Exception e)
            {
                string exceptionMessage = e.Message;
                InsertSuccessful = false;
                throw;
            }
			return InsertSuccessful;
		}
       
		public bool UpdateMethod(int method_RID, TransactionData td)
		{
			bool UpdateSuccessful = true;
			_dba = td.DBA;
			try
			{
				if (UpdateMethod(method_RID))
					UpdateSuccessful = true;
			}
			catch
			{
				UpdateSuccessful = false;
				throw;
			}
			finally
			{
			}
			return UpdateSuccessful;
		}

		public bool UpdateMethod(int method_RID)
		{
			bool UpdateSuccessful = true;
			try
			{
                StoredProcedures.MID_METHOD_CREATE_MASTER_HEADERS_UPSERT.Insert(_dba,
                                                                           METHOD_RID: method_RID,
                                                                           USE_SELECTED_HEADERS: Include.ConvertBoolToChar(UseSelectedHeaders),
                                                                           MERCHANDISE_TABLE: dtMerchandise,
                                                                           OVERRIDE_TABLE: dtOverride
                                                                           );
           		UpdateSuccessful = true;
			}
			catch
			{
				UpdateSuccessful = false;
				throw;
			}
			finally
			{
			}
			return UpdateSuccessful;
		}

		public bool DeleteMethod(int method_RID, TransactionData td)
		{
			bool DeleteSuccessfull = true;
			_dba = td.DBA;
			try
			{
                StoredProcedures.MID_METHOD_CREATE_MASTER_HEADERS_DELETE.Delete(_dba, METHOD_RID: method_RID);
                DeleteSuccessfull = true;
			}
			catch
			{
				DeleteSuccessfull = false;
				throw;
			}
			finally
			{
			}
			return DeleteSuccessfull;
		}
	}
}

