using System;
using System.Data;
using System.Collections;
using System.Globalization;
using System.Text;

using MIDRetail.DataCommon;

namespace MIDRetail.Data
{
	/// <summary>
	/// Summary description for SystemData.
	/// </summary>
	public partial class SystemData : DataLayer
	{	
		public SystemData() : base()
		{
		}

		// BEGIN TT#739-MD - STodd - delete stores
		public SystemData(string aConnectionString)
			: base(aConnectionString)
		{
		}
		// END TT#739-MD - STodd - delete stores

        //BEGIN TT#110-MD-VStuart - In Use Tool


        /// <summary>
        /// Executes a stored procedure, with input parameters, to get the data of an object In Use.
        /// </summary>
        /// <param name="aUserRid"></param>
        /// <param name="inUseType"></param>
        /// <returns>DataTable</returns>
        public DataTable GetInUseData(int aUserRid, int inUseType, out bool aAllowDelete)
        {
            try
            {
                aAllowDelete = true;
                DataTable dt = new DataTable();

                dt = StoredProcedures.SP_MID_DETAIL_ACCESS.Read(_dba,
                                                                    ref aAllowDelete,
                                                                    inUseType: inUseType,
                                                                    inUseRID: aUserRid
                                                               );
                //aAllowDelete = Include.ConvertIntToBool((int)StoredProcedures.SP_MID_DETAIL_ACCESS.outAllowDelete.Value);
                return dt;
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

        /// <summary>
        /// Executes a stored procedure, with an input parameter, to get the soft text codes for the column headings.
        /// </summary>
        /// <param name="inUseType"></param>
        /// <returns>DataTable</returns>
        public DataTable GetInUseHeaders(int inUseType)
        {
            try
            {
                return StoredProcedures.SP_MID_APPLICATION_LABEL_HEADINGS.Read(_dba, inUseType: inUseType);
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }
        //END TT#110-MD-VStuart - In Use Tool

		// END TT#739-MD - STodd - delete stores
		public DataTable GetDBTableRowCounts()
		{
			try
			{
                return StoredProcedures.MID_DB_READ_TABLE_ROW_COUNTS.Read(_dba);
			}
			catch (Exception err)
			{
				string message = err.ToString();
				throw;
			}
		}

        //public DataTable DatabaseConstraints_DisableAll()
        //{
        //    try
        //    {
        //        string SQLCommand = "EXEC sp_MSforeachtable \"ALTER TABLE ? NOCHECK CONSTRAINT all\"";

        //        return _dba.ExecuteSQLQuery(SQLCommand, "DatabaseConstraints_DisableAll");
        //    }
        //    catch (Exception err)
        //    {
        //        string message = err.ToString();
        //        throw;
        //    }
        //}

        //public DataTable DatabaseConstraints_EnableAll()
        //{
        //    try
        //    {
        //        string SQLCommand = "EXEC sp_MSforeachtable \"ALTER TABLE ? NOCHECK CONSTRAINT all\"";

        //        return _dba.ExecuteSQLQuery(SQLCommand, "DatabaseConstraints_EnableAll");
        //    }
        //    catch (Exception err)
        //    {
        //        string message = err.ToString();
        //        throw;
        //    }
        //}
		// END TT#739-MD - STodd - delete stores
		// Begin TT#1581-MD - stodd - API Header Reconcile
        /// <summary>
        /// Executes a stored procedure, with input parameters, to get the data of an object In Use.
        /// </summary>
        /// <param name="aUserRid"></param>
        /// <param name="inUseType"></param>
        /// <returns>DataTable</returns>
        public DataTable GetAPIProcessControlRules(int API_ID)
        {
            try
            {
                DataTable dt = new DataTable();

                return StoredProcedures.MID_API_PROCESS_CONTROL_RULES_READ.Read(_dba,
                                                                    API_ID: API_ID
                                                               );
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }
		// End TT#1581-MD - stodd - API Header Reconcile
    }
}
