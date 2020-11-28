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
                    ROStringParms parms = (ROStringParms)Parms;
                    return GetStoreProfile(parms.ROString, SAB);
                case eRORequest.StoreComboFiltersData:
                    return GetStoreFilterComboData();
               
            }

            return new RONoDataOut(eROReturnCode.Failure, "Invalid Request", ROInstanceID);
        }

        #region "Method to Get Store Profile"
        /// <summary>
        /// Returns the node properties from the hierarchy
        /// </summary>
        /// <param name="sStoreId">The node description.</param>
        /// <param name="aSAB">The reference to the SessionAddressBlock object for the user and environment</param>
        /// <returns>A datatable</returns>
        private ROOut GetStoreProfile(string sStoreId, SessionAddressBlock aSAB)
        {
            try
            {

                DataTable dt = new DataTable();
                dt.TableName = "Store Profile";
                dt.Columns.Add("STORE_ID", typeof(int));
                dt.Columns.Add("NAME", typeof(string));
                dt.Columns.Add("ACTIVE", typeof(bool));
                dt.Columns.Add("MARKED_FOR_DELETION", typeof(bool));
                dt.Columns.Add("SIMILAR_STORE_MODEL", typeof(bool));
                dt.Columns.Add("STORE_DESCRIPTION", typeof(string));
                dt.Columns.Add("CITY", typeof(string));
                dt.Columns.Add("STATE", typeof(string));

                StoreProfile storeProfile = StoreMgmt.StoreProfile_Get(sStoreId);
                if (storeProfile.Key != Include.NoRID)
                {
                    DataRow dr = dt.NewRow();
                    dr["STORE_ID"] = storeProfile.StoreId;
                    dr["NAME"] = storeProfile.StoreName;
                    dr["ACTIVE"] = storeProfile.ActiveInd;
                    dr["MARKED_FOR_DELETION"] = storeProfile.DeleteStore;
                    dr["SIMILAR_STORE_MODEL"] = storeProfile.SimilarStoreModel;
                    dr["STORE_DESCRIPTION"] = storeProfile.StoreDescription;
                    dr["CITY"] = storeProfile.City;
                    dr["STATE"] = storeProfile.State;

                    dt.Rows.Add(dr);
                }

                return new RODataTableOut(eROReturnCode.Successful, null, ROInstanceID, dt);
                //return dt;
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
