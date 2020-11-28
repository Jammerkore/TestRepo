//using System;
//using System.Data;
//using System.Collections;
//using System.Globalization;

//using MIDRetail.DataCommon;


//namespace MIDRetail.Data
//{
//    /// <summary>
//    /// Summary description for Intransit.
//    /// </summary>
//    public class OnHand : DataLayer
//    {
//        private int _numberOfStoreDataTables = 1;
//        public OnHand()
//        {
//            GlobalOptions opts = new GlobalOptions();
//            DataTable dt = opts.GetGlobalOptions();
//            DataRow dr = dt.Rows[0];
//            this._numberOfStoreDataTables = (dr["STORE_TABLE_COUNT"] == DBNull.Value) ? 
//                1 : Convert.ToInt32(dr["STORE_TABLE_COUNT"], CultureInfo.CurrentUICulture);
//        }

////		public DataTable GetOhByTimeByNode(int timeId, int HN_RID, eOnHandVariable onHandVariable)
////		{
////			//TO DO
////			string dbColumn;
////			int tableNumber = HN_RID%_numberOfStoreDataTables;
////			if (onHandVariable == eOnHandVariable.InventoryTotal) 
////			{
////				dbColumn = Include.InventoryTotalUnitsColumn;
////			}
////			else
////			{
////				dbColumn = Include.InventoryRegularUnitsColumn;
////			}
////
////			string SQLCommand = "select ST_RID, COALESCE(" + dbColumn + ", 0) UNITS from STORE_HISTORY_DAY" + tableNumber.ToString(CultureInfo.CurrentUICulture) + " " + 
////				" where TIME_ID =" + timeId.ToString(CultureInfo.CurrentUICulture) + " and HN_RID =" + HN_RID.ToString(CultureInfo.CurrentUICulture);
////			return _dba.ExecuteSQLQuery( SQLCommand, "GET_INTRANSIT" );
////		}
		
//    }
//}


