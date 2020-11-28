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
	/// Adds properties for DC Carton Rounding
	/// </summary>
	public class DCCartonRoundingMethodData: MethodBaseData
	{
		private int _methodRid;
		private eAllocateOverageTo _applyOverageTo;
      
		public int MethodRid
		{
			get{return _methodRid;}
			set{_methodRid = value;	}
		}

        public eAllocateOverageTo ApplyOverageTo
		{
			get{return _applyOverageTo;}
			set{_applyOverageTo = value;	}
		}

     	/// <summary>
        /// Creates an instance of the DCCartonRoundingMethodData class
		/// </summary>
		public DCCartonRoundingMethodData()
		{
		}

		/// <summary>
        /// Creates an instance of the DCCartonRoundingMethodData class
		/// </summary>
		/// <param name="aMethodRID">The record ID of the method</param>
        public DCCartonRoundingMethodData(int aMethodRID, eChangeType changeType)
		{
            switch (changeType)
            {
                case eChangeType.populate:
                    PopulateDCCartonRounding(aMethodRID);
                    break;
            } 
		}

		/// <summary>
        /// Creates an instance of the DCCartonRoundingMethodData class
		/// </summary>
		/// <param name="td">An instance of the TransactionData class containing the database connection.</param>
		/// <param name="aMethodRID">The record ID of the method</param>
		public DCCartonRoundingMethodData(TransactionData td, int aMethodRID)
		{
			_dba = td.DBA;
			_methodRid = aMethodRID;
		}
        public DCCartonRoundingMethodData(TransactionData td)
		{
			_dba = td.DBA;
			_methodRid = Include.NoRID;
		}
		
		public bool PopulateDCCartonRounding(int method_RID)
		{
			try
			{
                if (PopulateMethod(method_RID))
                {
                    _methodRid = method_RID;
                    DataTable dtDCCartonRounding = StoredProcedures.MID_METHOD_DC_CARTON_ROUNDING_READ.Read(_dba, METHOD_RID: method_RID);
                    if (dtDCCartonRounding.Rows.Count != 0)
                    {
                        DataRow dr = dtDCCartonRounding.Rows[0];
                        if (dr["APPLY_OVERAGE_TO"] != System.DBNull.Value)
                        {
                            _applyOverageTo = (eAllocateOverageTo)Convert.ToInt32(dr["APPLY_OVERAGE_TO"].ToString(), CultureInfo.CurrentUICulture);
                        }
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public bool InsertMethod(int method_RID, TransactionData td)
		{
			bool InsertSuccessful = true;
            int applyOverageTo = Convert.ToInt32(ApplyOverageTo, CultureInfo.CurrentUICulture);
            try
            {
                StoredProcedures.MID_METHOD_DC_CARTON_ROUNDING_INSERT.Insert(_dba,
                                                                           METHOD_RID: method_RID,
                                                                           APPLY_OVERAGE_TO: Convert.ToChar(applyOverageTo.ToString(), CultureInfo.CurrentUICulture)
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

        //    SQLCommand.Append("INSERT INTO METHOD_GROUP_ALLOCATION_STORE_GRADES_VALUES (");
       
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
            int applyOverageTo = Convert.ToInt32(ApplyOverageTo, CultureInfo.CurrentUICulture);
			try
			{  
                StoredProcedures.MID_METHOD_DC_CARTON_ROUNDING_UPDATE.Update(_dba,
                                                                           METHOD_RID: method_RID,
                                                                           APPLY_OVERAGE_TO:  Convert.ToChar(applyOverageTo.ToString(), CultureInfo.CurrentUICulture)
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
                StoredProcedures.MID_METHOD_DC_CARTON_ROUNDING_DELETE.Delete(_dba, METHOD_RID: method_RID);
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

