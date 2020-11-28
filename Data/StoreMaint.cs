using System;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Globalization;
using System.Collections.Generic;

using MIDRetail.DataCommon;


namespace MIDRetail.Data
{

	public partial class StoreMaint : DataLayer
	{	


		public StoreMaint() : base()
		{

		}

        public StoreMaint(string aConnectionString)
			: base(aConnectionString)
		{

		}

        public DataTable ReadStoresForMaint()
        {
            try
            {
                return StoredProcedures.MID_STORES_READ_FOR_MAINT.Read(_dba);
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }
        public DataSet ReadStoresFieldsForMaint(int storeRID)
        {
            try
            {
                return StoredProcedures.MID_STORES_READ_FIELDS_FOR_MAINT.ReadAsDataSet(_dba, storeRID);
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }
        public DataSet ReadStoreFieldsForMaintByCol(List<int> storeCharGroups)
        {

            DataTable dtStoreList = new DataTable();
            dtStoreList.Columns.Add("CHAR_GROUP_RID", typeof(int));
            for (int i = 0; i < storeCharGroups.Count; i++)
            {
                //ensure rids are distinct, and only added to the datatable one time
                if (dtStoreList.Select("CHAR_GROUP_RID=" + storeCharGroups[i].ToString()).Length == 0)
                {
                    DataRow dr = dtStoreList.NewRow();
                    dr["CHAR_GROUP_RID"] = storeCharGroups[i];
                    dtStoreList.Rows.Add(dr);
                }
            }

            return StoredProcedures.MID_STORES_READ_FIELDS_FOR_MAINT_BY_COL.ReadAsDataSet(_dba, INCLUDE_INACTIVE_STORES: 0, CHAR_GROUP_RID_LIST: dtStoreList);
        }
        public DataSet ReadStoresFieldsForMaintNewStore()
        {
            try
            {
                return StoredProcedures.MID_STORES_READ_FIELDS_FOR_MAINT_ADD.ReadAsDataSet(_dba);
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }
        public DataSet ReadCharacteristicListValuesForMaint()
        {
            try
            {
                return StoredProcedures.MID_STORE_CHAR_GROUP_READ_FOR_MAINT.ReadAsDataSet(_dba);
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

        public bool DoesStoreIdAlreadyExist(string proposedId, int storeEditRID)
        {
            DataTable dt = StoredProcedures.MID_STORES_READ_ID_FOR_DUPLICATE.Read(_dba,
                                                                                    PROPOSED_STORE_ID: proposedId,
                                                                                    ST_EDIT_RID: storeEditRID);
            int matchCount = (int)dt.Rows[0]["MYCOUNT"];
            if (matchCount > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public int StoreProfile_Insert(StoreProfile store)
        {
            try
            {
 
                return StoredProcedures.SP_MID_STORE_INSERT.InsertAndReturnRID(_dba,
                                                                     ST_ID: Include.ConvertObjectToNullableString(store.StoreId),
                                                                     STORE_NAME: Include.ConvertObjectToNullableString(store.StoreName),
                                                                     STORE_DESC: Include.ConvertObjectToNullableString(store.StoreDescription),
                                                                     ACTIVE_IND: Include.ConvertObjectToNullableChar(Convert.ToInt32(store.ActiveInd, CultureInfo.CurrentUICulture)),
                                                                     CITY: Include.ConvertObjectToNullableString(store.City),
                                                                     STATE: Include.ConvertObjectToNullableString(store.State),
                                                                     SELLING_SQ_FT: Include.ConvertObjectToNullableInt(store.SellingSqFt),
                                                                     SELLING_OPEN_DATE: Include.ConvertObjectToNullableDateTime(checkDateNull(store.SellingOpenDt)),
                                                                     SELLING_CLOSE_DATE: Include.ConvertObjectToNullableDateTime(checkDateNull(store.SellingCloseDt)),
                                                                     STOCK_OPEN_DATE: Include.ConvertObjectToNullableDateTime(checkDateNull(store.StockOpenDt)),
                                                                     STOCK_CLOSE_DATE: Include.ConvertObjectToNullableDateTime(checkDateNull(store.StockCloseDt)),
                                                                     LEAD_TIME: Include.ConvertObjectToNullableInt(store.LeadTime),
                                                                     SHIP_ON_MONDAY: Include.ConvertObjectToNullableChar(Convert.ToInt32(store.ShipOnMonday, CultureInfo.CurrentUICulture)),
                                                                     // Begin TT#1949-MD - JSmith - Add new Store Select Ship on Days Mon, Wed, Fri.  After Update ALL ship on days are selected for the new store added.
                                                                     //SHIP_ON_TUESDAY: Include.ConvertObjectToNullableChar(Convert.ToInt32(store.ShipOnMonday, CultureInfo.CurrentUICulture)),
                                                                     //SHIP_ON_WEDNESDAY: Include.ConvertObjectToNullableChar(Convert.ToInt32(store.ShipOnMonday, CultureInfo.CurrentUICulture)),
                                                                     //SHIP_ON_THURSDAY: Include.ConvertObjectToNullableChar(Convert.ToInt32(store.ShipOnMonday, CultureInfo.CurrentUICulture)),
                                                                     //SHIP_ON_FRIDAY: Include.ConvertObjectToNullableChar(Convert.ToInt32(store.ShipOnMonday, CultureInfo.CurrentUICulture)),
                                                                     //SHIP_ON_SATURDAY: Include.ConvertObjectToNullableChar(Convert.ToInt32(store.ShipOnMonday, CultureInfo.CurrentUICulture)),
                                                                     //SHIP_ON_SUNDAY: Include.ConvertObjectToNullableChar(Convert.ToInt32(store.ShipOnMonday, CultureInfo.CurrentUICulture)),
                                                                     SHIP_ON_TUESDAY: Include.ConvertObjectToNullableChar(Convert.ToInt32(store.ShipOnTuesday, CultureInfo.CurrentUICulture)),
                                                                     SHIP_ON_WEDNESDAY: Include.ConvertObjectToNullableChar(Convert.ToInt32(store.ShipOnWednesday, CultureInfo.CurrentUICulture)),
                                                                     SHIP_ON_THURSDAY: Include.ConvertObjectToNullableChar(Convert.ToInt32(store.ShipOnThursday, CultureInfo.CurrentUICulture)),
                                                                     SHIP_ON_FRIDAY: Include.ConvertObjectToNullableChar(Convert.ToInt32(store.ShipOnFriday, CultureInfo.CurrentUICulture)),
                                                                     SHIP_ON_SATURDAY: Include.ConvertObjectToNullableChar(Convert.ToInt32(store.ShipOnSaturday, CultureInfo.CurrentUICulture)),
                                                                     SHIP_ON_SUNDAY: Include.ConvertObjectToNullableChar(Convert.ToInt32(store.ShipOnSunday, CultureInfo.CurrentUICulture)),
                                                                     // End TT#1949-MD - JSmith - Add new Store Select Ship on Days Mon, Wed, Fri.  After Update ALL ship on days are selected for the new store added.
                                                                     SIMILAR_STORE_MODEL: Include.ConvertObjectToNullableChar(Convert.ToInt32(store.SimilarStoreModel, CultureInfo.CurrentUICulture)),
                                                                     // Begin TT#1947-MD - JSmith - Store Load API
                                                                     //IMO_ID: Include.ConvertObjectToNullableChar(store.IMO_ID),
                                                                     IMO_ID: Include.ConvertObjectToNullableString(store.IMO_ID),
                                                                     // End TT#1947-MD - JSmith - Store Load API
                                                                     STORE_DELETE_IND: '0'
                                                                     );

            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

        public void StoreProfile_Update(StoreProfile store)
        {
            try
            {
              
                StoredProcedures.SP_MID_STORE_UPDATE.Update(_dba,
                                                         ST_RID: store.Key, 
                                                         ST_ID: Include.ConvertObjectToNullableString(store.StoreId),
                                                         STORE_NAME: Include.ConvertObjectToNullableString(store.StoreName),
                                                         STORE_DESC: Include.ConvertObjectToNullableString(store.StoreDescription),
                                                         ACTIVE_IND: Include.ConvertObjectToNullableChar(Convert.ToInt32(store.ActiveInd, CultureInfo.CurrentUICulture)),
                                                         CITY: Include.ConvertObjectToNullableString(store.City),
                                                         STATE: Include.ConvertObjectToNullableString(store.State),
                                                         SELLING_SQ_FT: Include.ConvertObjectToNullableInt(store.SellingSqFt),
                                                         SELLING_OPEN_DATE: Include.ConvertObjectToNullableDateTime(checkDateNull(store.SellingOpenDt)),
                                                         SELLING_CLOSE_DATE: Include.ConvertObjectToNullableDateTime(checkDateNull(store.SellingCloseDt)),
                                                         STOCK_OPEN_DATE: Include.ConvertObjectToNullableDateTime(checkDateNull(store.StockOpenDt)),
                                                         STOCK_CLOSE_DATE: Include.ConvertObjectToNullableDateTime(checkDateNull(store.StockCloseDt)),
                                                         LEAD_TIME: Include.ConvertObjectToNullableInt(store.LeadTime),
                                                         SHIP_ON_MONDAY: Include.ConvertObjectToNullableChar(Convert.ToInt32(store.ShipOnMonday, CultureInfo.CurrentUICulture)),
                                                         // Begin TT#1810-MD - JSmith - Str Profile - Edit Stores - Deselect Ship on Day Tues- Select OK- All Ship on Days are not BLANK for that store.
                                                         //SHIP_ON_TUESDAY: Include.ConvertObjectToNullableChar(Convert.ToInt32(store.ShipOnMonday, CultureInfo.CurrentUICulture)),
                                                         //SHIP_ON_WEDNESDAY: Include.ConvertObjectToNullableChar(Convert.ToInt32(store.ShipOnMonday, CultureInfo.CurrentUICulture)),
                                                         //SHIP_ON_THURSDAY: Include.ConvertObjectToNullableChar(Convert.ToInt32(store.ShipOnMonday, CultureInfo.CurrentUICulture)),
                                                         //SHIP_ON_FRIDAY: Include.ConvertObjectToNullableChar(Convert.ToInt32(store.ShipOnMonday, CultureInfo.CurrentUICulture)),
                                                         //SHIP_ON_SATURDAY: Include.ConvertObjectToNullableChar(Convert.ToInt32(store.ShipOnMonday, CultureInfo.CurrentUICulture)),
                                                         //SHIP_ON_SUNDAY: Include.ConvertObjectToNullableChar(Convert.ToInt32(store.ShipOnMonday, CultureInfo.CurrentUICulture)),
                                                         SHIP_ON_TUESDAY: Include.ConvertObjectToNullableChar(Convert.ToInt32(store.ShipOnTuesday, CultureInfo.CurrentUICulture)),
                                                         SHIP_ON_WEDNESDAY: Include.ConvertObjectToNullableChar(Convert.ToInt32(store.ShipOnWednesday, CultureInfo.CurrentUICulture)),
                                                         SHIP_ON_THURSDAY: Include.ConvertObjectToNullableChar(Convert.ToInt32(store.ShipOnThursday, CultureInfo.CurrentUICulture)),
                                                         SHIP_ON_FRIDAY: Include.ConvertObjectToNullableChar(Convert.ToInt32(store.ShipOnFriday, CultureInfo.CurrentUICulture)),
                                                         SHIP_ON_SATURDAY: Include.ConvertObjectToNullableChar(Convert.ToInt32(store.ShipOnSaturday, CultureInfo.CurrentUICulture)),
                                                         SHIP_ON_SUNDAY: Include.ConvertObjectToNullableChar(Convert.ToInt32(store.ShipOnSunday, CultureInfo.CurrentUICulture)),
                                                         // End TT#1810-MD - JSmith - Str Profile - Edit Stores - Deselect Ship on Day Tues- Select OK- All Ship on Days are not BLANK for that store.
                                                         SIMILAR_STORE_MODEL: Include.ConvertObjectToNullableChar(Convert.ToInt32(store.SimilarStoreModel, CultureInfo.CurrentUICulture)),
                                                         IMO_ID: Include.ConvertObjectToNullableString(store.IMO_ID),
                                                         STORE_DELETE_IND: Include.ConvertObjectToNullableChar(Convert.ToInt32(store.DeleteStore))
                                                         );
                return;

            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }
        private object checkDateNull(DateTime tdate)
        {
            if (tdate.Year == 1)
            {
                return System.DBNull.Value;
            }
            else
            {
                return tdate;
            }
        }
	}
}
