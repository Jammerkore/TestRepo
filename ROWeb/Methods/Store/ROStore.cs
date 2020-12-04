using Logility.ROWebCommon;
using Logility.ROWebSharedTypes;
using MIDRetail.Business;
using MIDRetail.Data;
using MIDRetail.DataCommon;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;

namespace Logility.ROWeb
{
    public partial class ROStore : ROWebFunction
    {
        //==========
        // FIELDS
        //==========
        private FilterData _storeFilterDL;
        private ArrayList _userRIDList;
        private SessionAddressBlock _sab;

        /// <summary>
        /// Creates an instance of the class 
        /// </summary>
        /// <param name="SAB">The SessionAddressBlock for this user and environment</param>
        /// <param name="ROWebTools">An instance of the ROWebTools</param>
        public ROStore(SessionAddressBlock SAB, ROWebTools ROWebTools)
            : base(SAB, ROWebTools)
        {
            _sab = SAB;
        }

        override public void CleanUp()
        {

        }

        override public ROOut ProcessRequest(ROParms Parms)
        {
            switch (Parms.RORequest)
            {
                case eRORequest.GetStoreProfile:
                    if (Parms is ROStringParms)
                    {
                        ROStringParms parms = (ROStringParms)Parms;
                        return GetStoreProfile(parms.ROString);
                    }
                    else
                    {
                        ROKeyParms parms = (ROKeyParms)Parms;
                        return GetStoreProfile(parms.Key);
                    }
                case eRORequest.StoreComboFiltersData:
                    return GetStoreFilterComboData();
               
            }

            return new RONoDataOut(eROReturnCode.Failure, "Invalid Request", ROInstanceID);
        }

        #region "Method to Get Store Profile"
        /// <summary>
        /// Retrieves store profile using the store ID
        /// </summary>
        /// <param name="storeId">The node description.</param>
        /// <returns>ROStoreProfileOut</returns>
        private ROOut GetStoreProfile(string storeId)
        {
            StoreProfile storeProfile = StoreMgmt.StoreProfile_Get(storeId);
            return GetStoreProfile(storeProfile: storeProfile);
        }

        /// <summary>
        /// Retrieves store profile using the store key
        /// </summary>
        /// <param name="storeKey">The node description.</param>
        /// <returns>ROStoreProfileOut</returns>
        private ROOut GetStoreProfile(int storeKey)
        {
            StoreProfile storeProfile = StoreMgmt.StoreProfile_Get(storeKey);
            return GetStoreProfile(storeProfile: storeProfile);
        }

        /// <summary>
        /// Retrieves the properties for the store
        /// </summary>
        /// <param name="storeProfile">The properties for the store</param>
        /// <returns>ROStoreProfileOut</returns>
        private ROOut GetStoreProfile(StoreProfile storeProfile)
        {
            eROReturnCode returnCode = eROReturnCode.Successful;
            string message = null;

            try
            {
                ROStoreProfile storeProperties = null;

                if (storeProfile.Key != Include.NoRID)
                {
                    storeProperties = new ROStoreProfile(
                        store: new KeyValuePair<int, string>(storeProfile.Key, storeProfile.StoreId),
                        name: storeProfile.StoreName,
                        description: storeProfile.StoreDescription,
                        isActive: storeProfile.ActiveInd,
                        isMarkedForDeletion: storeProfile.DeleteStore,
                        city: storeProfile.City == null ? string.Empty : storeProfile.City,
                        state: storeProfile.State == null ? string.Empty : storeProfile.State,
                        sellingSquareFootage: storeProfile.SellingSqFt,
                        sellingOpenDate: storeProfile.SellingOpenDt == Include.UndefinedDate ? string.Empty : storeProfile.SellingOpenDt.ToShortDateString(),
                        sellingCloseDate: storeProfile.SellingCloseDt == Include.UndefinedDate ? string.Empty : storeProfile.SellingCloseDt.ToShortDateString(),
                        stockOpenDate: storeProfile.StockOpenDt == Include.UndefinedDate ? string.Empty : storeProfile.StockOpenDt.ToShortDateString(),
                        stockCloseDate: storeProfile.StockCloseDt == Include.UndefinedDate ? string.Empty : storeProfile.StockCloseDt.ToShortDateString(),
                        leadTime: storeProfile.LeadTime,
                        shipOnMonday: storeProfile.ShipOnMonday,
                        shipOnTuesday: storeProfile.ShipOnTuesday,
                        shipOnWednesday: storeProfile.ShipOnWednesday,
                        shipOnThursday: storeProfile.ShipOnThursday,
                        shipOnFriday: storeProfile.ShipOnFriday,
                        shipOnSaturday: storeProfile.ShipOnSaturday,
                        shipOnSunday: storeProfile.ShipOnSunday,
                        text: storeProfile.Text,
                        storeStatus: GetName.GetStoreStatus(storeStatus: storeProfile.Status),
                        stockStatus: GetName.GetStoreStatus(storeStatus: storeProfile.StockStatus),
                        similarStoreModel: storeProfile.SimilarStoreModel,
                        virtualStoreWarehouse_ID: storeProfile.IMO_ID == null ? string.Empty : storeProfile.IMO_ID
                        );

                    StoreMaint storeMaintData = new StoreMaint();
                    DataSet dsValues = storeMaintData.ReadStoresFieldsForMaint(storeProfile.Key);
                    int characteristicGroupKey, characteristicValueKey;
                    string characteristicGroupName, characteristicValueName;

                    foreach (DataRow drChar in dsValues.Tables[1].Rows)
                    {
                        characteristicGroupKey = Convert.ToInt32(drChar["SCG_RID"]); ;
                        characteristicGroupName = Convert.ToString(drChar["SCG_ID"]);
                        characteristicValueKey = 0;
                        characteristicValueName = string.Empty;
                        if (drChar["SC_RID"] != System.DBNull.Value)
                        {
                            characteristicValueKey = Convert.ToInt32(drChar["SC_RID"]);
                            characteristicValueName = Convert.ToString(drChar["CHAR_VALUE"]);
                        }

                        ROCharacteristic characteristic = new ROCharacteristic(
                            characteristicGroupKey: characteristicGroupKey,
                            characteristicGroupName: characteristicGroupName,
                            characteristicValueKey: characteristicValueKey,
                            characteristicValueName: characteristicValueName
                            );

                        storeProperties.Characteristics.Add(characteristic);
                    }
                }
                else
                {
                    returnCode = eROReturnCode.Failure;
                    message = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_StoreNotFound);
                }

                return new ROStoreProfileOut(returnCode, message, ROInstanceID, storeProperties);
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region "Method to Get Store Filters for Filter Dropdown"
        internal ROOut GetStoreFilterComboData()
        {

            _storeFilterDL = new FilterData();
            DataTable dtFilter;

            dtFilter = _storeFilterDL.FilterRead(filterTypes.StoreFilter, eProfileType.FilterStore, GetUserIds());

            ROIntStringPairListOut rOIntStringPairListOut = new ROIntStringPairListOut(eROReturnCode.Successful, null, ROInstanceID, BuildPairListOut(dtFilter));



            return rOIntStringPairListOut;
        }

        #endregion
                  

        #region "Common Methods"
        internal List<KeyValuePair<int, string>> BuildPairListOut(DataTable dtFilter)
        {
            List<KeyValuePair<int, string>> filtersValuePairList = new List<KeyValuePair<int, string>>();

            DataView dv = new DataView(dtFilter);
            dv.Sort = "FILTER_NAME";

            foreach (DataRowView rowView in dv)
            {
                int filterRID = Convert.ToInt32(rowView.Row["FILTER_RID"]);
                string filterName = Convert.ToString(rowView.Row["FILTER_NAME"]);
                filtersValuePairList.Add(new KeyValuePair<int, string>(filterRID, filterName));
            }

            return filtersValuePairList;
        }

        internal ArrayList GetUserIds()
        {
            FunctionSecurityProfile viewUserSecurity = _sab.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ForecastViewsUser);
            FunctionSecurityProfile viewGlobalSecurity = _sab.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ForecastViewsGlobal);

            _userRIDList = new ArrayList();

            _userRIDList.Add(-1);

            if (viewUserSecurity.AllowView)
            {
                _userRIDList.Add(_sab.ClientServerSession.UserRID);
            }

            if (viewGlobalSecurity.AllowView)
            {
                _userRIDList.Add(Include.GlobalUserRID);
            }

            return _userRIDList;
        }

        #endregion
    }
}
