using System;
using System.Globalization;
using MIDRetail.Data;
using MIDRetail.Common;
using MIDRetail.DataCommon;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using MIDRetail.Business.Allocation;

using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Polly;

namespace MIDRetail.Business
{

    public partial class HierarchySessionTransaction : Transaction
    {
        //=======
        // FIELDS
        //=======


        /// <summary>
        /// Requests the transaction get the store eligibility settings for all stores for a node and week.
        /// </summary>
        /// <param name="aNodeRID">The record id of the node</param>
        /// <param name="aFirstDayOfWeek">The first day of year/week for which eligibility is to be determined</param>
        /// <returns>BitArray containing eligibility settings indexed by storeRID</returns>
        public System.Collections.BitArray GetExternalStoreSalesEligibilityFlags(
            eRequestingApplication requestingApplication,
            int headerRID,
            string headerID,
            int aNodeRID,
            int aFirstDayOfWeek
            )
        {
            try
            {
                StoreEligibilityList sel = null;
                ProfileList storeList = GetProfileList(eProfileType.Store);
                StoreEligibilityProfile sep = null;
                System.Collections.BitArray eligibilityBitArray = new System.Collections.BitArray(_maxStoreRID + 1, true);  // default to eligible
                                                                                                                            // get date information to check against open and close dates
                WeekProfile weekProfile = this.SAB.HierarchyServerSession.Calendar.GetWeek(aFirstDayOfWeek);
                DayProfile dayProfile = (DayProfile)weekProfile.Days[0];
                DateTime firstDayOfWeek = dayProfile.Date;
                dayProfile = (DayProfile)weekProfile.Days[weekProfile.DaysInWeek - 1];
                DateTime lastDayOfWeek = dayProfile.Date;

                Hashtable storeEligibilityHash;
                // select appropriate cache based on settings
                if (requestingApplication == eRequestingApplication.Forecast)
                {
                    storeEligibilityHash = PlanningSalesStoreEligibilityHash;
                }
                else
                {
                    storeEligibilityHash = AllocationSalesStoreEligibilityHash;
                }

                sel = (StoreEligibilityList)storeEligibilityHash[aNodeRID];

                if (sel == null)
                {
                    sel = CallExternalEligibility(
                             requestingApplication: requestingApplication,
                             variable: cSalesVariable,
                             merchandiseKey: aNodeRID,
                             yearWeek: weekProfile.YearWeek,
                             storeList: storeList
                            );
                    storeEligibilityHash.Add(aNodeRID, sel);
                }

                foreach (StoreProfile storeProfile in storeList)
                {
                    eStoreStatus storeStatus = GetStoreSalesStatus(storeProfile.Key, aFirstDayOfWeek);
                    if (storeStatus == eStoreStatus.Closed ||
                        storeProfile.SellingOpenDt > lastDayOfWeek ||
                        (storeProfile.SellingCloseDt != Include.UndefinedDate &&
                        storeProfile.SellingCloseDt < firstDayOfWeek))
                    {
                        eligibilityBitArray[storeProfile.Key] = false;
                    }
                    else
                    {
                        sep = (StoreEligibilityProfile)sel.FindKey(storeProfile.Key);
                        if (sep == null)    // if eligibility not set for store
                        {
                            if (storeStatus == eStoreStatus.Preopen)
                            {
                                eligibilityBitArray[storeProfile.Key] = false;
                            }
                        }
                        else
                        if (sep.StoreIneligible)
                        {
                            eligibilityBitArray[storeProfile.Key] = false;
                        }
                        else
                        if (sep.EligModelRID != Include.NoRID)      // check against model dates
                        {
                            if (sep.EligModelRID != _currentSalesEligibilityModelRID)
                            {

                                if (!SalesEligibilityModelHash.ContainsKey(sep.EligModelRID))
                                {
                                    LoadEligibilityModel(sep.EligModelRID);
                                    _currentStockEligibilityModelRID = sep.EligModelRID;	// set stock model since both are loaded
                                    _currentPriorityShippingModelRID = sep.EligModelRID;    // set priority shipping since loaded
                                }

                                _salesEligibilityModifierModel = (ModifierModel)SalesEligibilityModelHash[sep.EligModelRID];

                                if (!_salesEligibilityModifierModel.ContainsValuesByStore)
                                {
                                    SalesEligibilityModelDateHash = _salesEligibilityModifierModel.ModelDateHash;
                                }

                                _currentSalesEligibilityModelRID = sep.EligModelRID;
                            }

                            if (_salesEligibilityModifierModel.ContainsValuesByStore)
                            {
                                SalesEligibilityModelDateHash = (Hashtable)_salesEligibilityModifierModel.ModelDateHash[sep.Key];
                                if (SalesEligibilityModelDateHash == null)
                                {
                                    SalesEligibilityModelDateHash = new Hashtable();
                                }
                            }

                            if (SalesEligibilityModelDateHash.Count == 0)       // eligible if no dates
                            {
                                if (storeStatus == eStoreStatus.Preopen)
                                {
                                    eligibilityBitArray[storeProfile.Key] = false;
                                }
                                else
                                {
                                    eligibilityBitArray[storeProfile.Key] = true;
                                }
                            }
                            else
                            {
                                if (SalesEligibilityModelDateHash.Contains(aFirstDayOfWeek))
                                {
                                    eligibilityBitArray[storeProfile.Key] = true;
                                }
                                else
                                {	// check for reoccurring
                                    int weekInYear = weekProfile.WeekInYear;
                                    if (SalesEligibilityModelDateHash.Contains(weekInYear))
                                    {
                                        eligibilityBitArray[storeProfile.Key] = true;
                                    }
                                    else
                                    {
                                        eligibilityBitArray[storeProfile.Key] = false;
                                    }
                                }
                            }
                        }
                    }
                }
                return eligibilityBitArray;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
		/// Requests the transaction get the store eligibility settings for all stores for a node and week.
		/// </summary>
		/// <param name="aNodeRID">The record id of the node</param>
		/// <param name="aFirstDayOfWeek">The first day of year/week for which eligibility is to be determined</param>
		/// <returns>BitArray containing eligibility settings indexed by storeRID</returns>
		public System.Collections.BitArray GetExternalStoreStockEligibilityFlags(
            eRequestingApplication requestingApplication,
            int headerRID,
            string headerID,
            int aNodeRID, 
            int aFirstDayOfWeek
            )
        {
            try
            {
                StoreEligibilityList sel = null;
                ProfileList storeList = GetProfileList(eProfileType.Store);
                StoreEligibilityProfile sep = null;
                System.Collections.BitArray eligibilityBitArray = new System.Collections.BitArray(_maxStoreRID + 1, true);  // default to eligible
                                                                                                                            // get date information to check against open and close dates
                WeekProfile weekProfile = this.SAB.HierarchyServerSession.Calendar.GetWeek(aFirstDayOfWeek);
                DayProfile dayProfile = (DayProfile)weekProfile.Days[0];
                DateTime firstDayOfWeek = dayProfile.Date;
                dayProfile = (DayProfile)weekProfile.Days[weekProfile.DaysInWeek - 1];
                DateTime lastDayOfWeek = dayProfile.Date;

                Hashtable storeEligibilityHash;
                // select appropriate cache based on settings
                if (requestingApplication == eRequestingApplication.Forecast)
                {
                    storeEligibilityHash = PlanningStockStoreEligibilityHash;
                }
                else
                {
                    storeEligibilityHash = AllocationStockStoreEligibilityHash;
                }

                sel = (StoreEligibilityList)storeEligibilityHash[aNodeRID];

                if (sel == null)
                {
                    sel = CallExternalEligibility(
                             requestingApplication: requestingApplication,
                             variable: cStockVariable,
                             merchandiseKey: aNodeRID,
                             yearWeek: weekProfile.YearWeek,
                             storeList: storeList
                            );
                    storeEligibilityHash.Add(aNodeRID, sel);
                }

                foreach (StoreProfile storeProfile in storeList)
                {
                    eStoreStatus storeStatus = GetStoreStockStatus(storeProfile.Key, aFirstDayOfWeek);
                    if (storeStatus == eStoreStatus.Closed ||
                        storeProfile.StockOpenDt > lastDayOfWeek ||
                        (storeProfile.StockCloseDt != Include.UndefinedDate &&
                        storeProfile.StockCloseDt < firstDayOfWeek))
                    {
                        eligibilityBitArray[storeProfile.Key] = false;
                    }
                    else
                    {
                        sep = (StoreEligibilityProfile)sel.FindKey(storeProfile.Key);
                        if (sep == null)    // if eligibility not defined for the store
                        {
                            if (storeStatus == eStoreStatus.Preopen)
                            {
                                eligibilityBitArray[storeProfile.Key] = false;
                            }
                        }
                        else
                            if (sep.StoreIneligible)
                        {
                            eligibilityBitArray[storeProfile.Key] = false;
                        }
                        else
                        {
                            if (sep.EligModelRID != Include.NoRID)      // check against model dates
                            {
                                if (sep.EligModelRID != _currentStockEligibilityModelRID)
                                {

                                    if (!StockEligibilityModelHash.ContainsKey(sep.EligModelRID))
                                    {
                                        LoadEligibilityModel(sep.EligModelRID);
                                        _currentSalesEligibilityModelRID = sep.EligModelRID;	// set sales model since both are loaded
                                        _currentPriorityShippingModelRID = sep.EligModelRID;    // set priority shipping since loaded
                                    }

                                    _stockEligibilityModifierModel = (ModifierModel)StockEligibilityModelHash[sep.EligModelRID];

                                    if (!_stockEligibilityModifierModel.ContainsValuesByStore)
                                    {
                                        StockEligibilityModelDateHash = _stockEligibilityModifierModel.ModelDateHash;
                                    }
                                    _currentStockEligibilityModelRID = sep.EligModelRID;
                                }

                                if (_stockEligibilityModifierModel.ContainsValuesByStore)
                                {
                                    StockEligibilityModelDateHash = (Hashtable)_stockEligibilityModifierModel.ModelDateHash[sep.Key];
                                    if (StockEligibilityModelDateHash == null)
                                    {
                                        StockEligibilityModelDateHash = new Hashtable();
                                    }
                                }

                                if (StockEligibilityModelDateHash.Count == 0)       // eligible if no dates
                                {
                                    if (storeStatus == eStoreStatus.Preopen)
                                    {
                                        eligibilityBitArray[storeProfile.Key] = false;
                                    }
                                    else
                                    {
                                        eligibilityBitArray[storeProfile.Key] = true;
                                    }
                                }
                                else
                                {
                                    if (StockEligibilityModelDateHash.Contains(aFirstDayOfWeek))
                                    {
                                        eligibilityBitArray[storeProfile.Key] = true;
                                    }
                                    else
                                    {	// check for reoccurring
                                        int weekInYear = weekProfile.WeekInYear;
                                        if (StockEligibilityModelDateHash.Contains(weekInYear))
                                        {
                                            eligibilityBitArray[storeProfile.Key] = true;
                                        }
                                        else
                                        {
                                            eligibilityBitArray[storeProfile.Key] = false;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                return eligibilityBitArray;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Requests the transaction get the store eligibility settings for all stores for a node and week.
        /// </summary>
        /// <param name="colorCodeRID">The key of the color</param>
        /// <param name="colorID">The name of the color</param>
        /// <param name="colorNodeRID">The key of the node for the color in the hierarchy</param>
        /// <param name="aFirstDayOfWeek">The first day of year/week for which eligibility is to be determined</param>
        /// <returns>BitArray containing eligibility settings indexed by storeRID</returns>
        public System.Collections.BitArray GetExternalStoreSalesEligibilityFlags(
            eRequestingApplication requestingApplication,
            int headerRID,
            string headerID,
            int colorCodeRID,
            string colorID,
            int colorNodeRID,
            int aFirstDayOfWeek
            )
        {
            try
            {
                StoreEligibilityList sel = null;
                ProfileList storeList = GetProfileList(eProfileType.Store);
                StoreEligibilityProfile sep = null;
                System.Collections.BitArray eligibilityBitArray = new System.Collections.BitArray(_maxStoreRID + 1, true);  // default to eligible
                                                                                                                            // get date information to check against open and close dates
                WeekProfile weekProfile = this.SAB.HierarchyServerSession.Calendar.GetWeek(aFirstDayOfWeek);
                DayProfile dayProfile = (DayProfile)weekProfile.Days[0];
                DateTime firstDayOfWeek = dayProfile.Date;
                dayProfile = (DayProfile)weekProfile.Days[weekProfile.DaysInWeek - 1];
                DateTime lastDayOfWeek = dayProfile.Date;

                Hashtable storeEligibilityHash;
                // select appropriate cache based on settings
                if (requestingApplication == eRequestingApplication.Forecast)
                {
                    storeEligibilityHash = PlanningSalesColorStoreEligibilityHash;
                }
                else
                {
                    storeEligibilityHash = AllocationSalesColorStoreEligibilityHash;
                }

                sel = (StoreEligibilityList)storeEligibilityHash[colorNodeRID];

                if (sel == null)
                {
                    sel = CallExternalEligibility(
                             requestingApplication: requestingApplication,
                             variable: cSalesVariable,
                             merchandiseKey: colorNodeRID,
                             yearWeek: weekProfile.YearWeek,
                             storeList: storeList
                            );
                    storeEligibilityHash.Add(colorNodeRID, sel);
                }

                foreach (StoreProfile storeProfile in storeList)
                {
                    eStoreStatus storeStatus = GetStoreSalesStatus(storeProfile.Key, aFirstDayOfWeek);
                    if (storeStatus == eStoreStatus.Closed ||
                        storeProfile.SellingOpenDt > lastDayOfWeek ||
                        (storeProfile.SellingCloseDt != Include.UndefinedDate &&
                        storeProfile.SellingCloseDt < firstDayOfWeek))
                    {
                        eligibilityBitArray[storeProfile.Key] = false;
                    }
                    else
                    {
                        sep = (StoreEligibilityProfile)sel.FindKey(storeProfile.Key);
                        if (sep == null)    // if eligibility not set for store
                        {
                            if (storeStatus == eStoreStatus.Preopen)
                            {
                                eligibilityBitArray[storeProfile.Key] = false;
                            }
                        }
                        else
                        if (sep.StoreIneligible)
                        {
                            eligibilityBitArray[storeProfile.Key] = false;
                        }
                        else
                        if (sep.EligModelRID != Include.NoRID)      // check against model dates
                        {
                            if (sep.EligModelRID != _currentSalesEligibilityModelRID)
                            {

                                if (!SalesEligibilityModelHash.ContainsKey(sep.EligModelRID))
                                {
                                    LoadEligibilityModel(sep.EligModelRID);
                                    _currentStockEligibilityModelRID = sep.EligModelRID;	// set stock model since both are loaded
                                    _currentPriorityShippingModelRID = sep.EligModelRID;    // set priority shipping since loaded
                                }

                                _salesEligibilityModifierModel = (ModifierModel)SalesEligibilityModelHash[sep.EligModelRID];

                                if (!_salesEligibilityModifierModel.ContainsValuesByStore)
                                {
                                    SalesEligibilityModelDateHash = _salesEligibilityModifierModel.ModelDateHash;
                                }
                                _currentSalesEligibilityModelRID = sep.EligModelRID;
                            }

                            if (_salesEligibilityModifierModel.ContainsValuesByStore)
                            {
                                SalesEligibilityModelDateHash = (Hashtable)_salesEligibilityModifierModel.ModelDateHash[sep.Key];
                                if (SalesEligibilityModelDateHash == null)
                                {
                                    SalesEligibilityModelDateHash = new Hashtable();
                                }

                                if (SalesEligibilityModelDateHash.Count == 0)       // eligible if no dates
                                {
                                    if (storeStatus == eStoreStatus.Preopen)
                                    {
                                        eligibilityBitArray[storeProfile.Key] = false;
                                    }
                                    else
                                    {
                                        eligibilityBitArray[storeProfile.Key] = true;
                                    }
                                }
                                else
                                {
                                    if (SalesEligibilityModelDateHash.Contains(aFirstDayOfWeek))
                                    {
                                        eligibilityBitArray[storeProfile.Key] = true;
                                    }
                                    else
                                    {   // check for reoccurring
                                        int weekInYear = weekProfile.WeekInYear;
                                        if (SalesEligibilityModelDateHash.Contains(weekInYear))
                                        {
                                            eligibilityBitArray[storeProfile.Key] = true;
                                        }
                                        else
                                        {
                                            eligibilityBitArray[storeProfile.Key] = false;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                return eligibilityBitArray;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
		/// Requests the transaction get the store eligibility settings for all stores for a node and week.
		/// </summary>
		/// <param name="colorCodeRID">The key of the color</param>
        /// <param name="colorID">The ID of the color</param>
        /// <param name="colorNodeRID">The key of the node for the color in the hierarchy</param>
		/// <param name="aFirstDayOfWeek">The first day of year/week for which eligibility is to be determined</param>
		/// <returns>BitArray containing eligibility settings indexed by storeRID</returns>
		public System.Collections.BitArray GetExternalStoreStockEligibilityFlags(
            eRequestingApplication requestingApplication,
            int headerRID,
            string headerID,
            int colorCodeRID,
            string colorID,
            int colorNodeRID,
            int aFirstDayOfWeek
            )
        {
            try
            {
                StoreEligibilityList sel = null;
                ProfileList storeList = GetProfileList(eProfileType.Store);
                StoreEligibilityProfile sep = null;
                System.Collections.BitArray eligibilityBitArray = new System.Collections.BitArray(_maxStoreRID + 1, true);  // default to eligible
                                                                                                                            // get date information to check against open and close dates
                WeekProfile weekProfile = this.SAB.HierarchyServerSession.Calendar.GetWeek(aFirstDayOfWeek);
                DayProfile dayProfile = (DayProfile)weekProfile.Days[0];
                DateTime firstDayOfWeek = dayProfile.Date;
                dayProfile = (DayProfile)weekProfile.Days[weekProfile.DaysInWeek - 1];
                DateTime lastDayOfWeek = dayProfile.Date;

                Hashtable storeEligibilityHash;
                // select appropriate cache based on settings
                if (requestingApplication == eRequestingApplication.Forecast)
                {
                    storeEligibilityHash = PlanningStockColorStoreEligibilityHash;
                }
                else
                {
                    storeEligibilityHash = AllocationStockColorStoreEligibilityHash;
                }

                sel = (StoreEligibilityList)storeEligibilityHash[colorNodeRID];

                if (sel == null)
                {
                    sel = CallExternalEligibility(
                             requestingApplication: requestingApplication,
                             variable: cStockVariable,
                             merchandiseKey: colorNodeRID,
                             yearWeek: weekProfile.YearWeek,
                             storeList: storeList
                            );
                    storeEligibilityHash.Add(colorNodeRID, sel);
                }

                foreach (StoreProfile storeProfile in storeList)
                {
                    eStoreStatus storeStatus = GetStoreStockStatus(storeProfile.Key, aFirstDayOfWeek);
                    if (storeStatus == eStoreStatus.Closed ||
                        storeProfile.StockOpenDt > lastDayOfWeek ||
                        (storeProfile.StockCloseDt != Include.UndefinedDate &&
                        storeProfile.StockCloseDt < firstDayOfWeek))
                    {
                        eligibilityBitArray[storeProfile.Key] = false;
                    }
                    else
                    {
                        sep = (StoreEligibilityProfile)sel.FindKey(storeProfile.Key);
                        if (sep == null)    // if eligibility not defined for the store
                        {
                            if (storeStatus == eStoreStatus.Preopen)
                            {
                                eligibilityBitArray[storeProfile.Key] = false;
                            }
                        }
                        else
                            if (sep.StoreIneligible)
                        {
                            eligibilityBitArray[storeProfile.Key] = false;
                        }
                        else
                        {
                            if (sep.EligModelRID != Include.NoRID)      // check against model dates
                            {
                                if (sep.EligModelRID != _currentStockEligibilityModelRID)
                                {

                                    if (!StockEligibilityModelHash.ContainsKey(sep.EligModelRID))
                                    {
                                        LoadEligibilityModel(sep.EligModelRID);
                                        _currentSalesEligibilityModelRID = sep.EligModelRID;	// set sales model since both are loaded
                                        _currentPriorityShippingModelRID = sep.EligModelRID;    // set priority shipping since loaded
                                    }

                                    _stockEligibilityModifierModel = (ModifierModel)StockEligibilityModelHash[sep.EligModelRID];

                                    if (!_stockEligibilityModifierModel.ContainsValuesByStore)
                                    {
                                        StockEligibilityModelDateHash = _stockEligibilityModifierModel.ModelDateHash;
                                    }
                                    _currentStockEligibilityModelRID = sep.EligModelRID;
                                }

                                if (_stockEligibilityModifierModel.ContainsValuesByStore)
                                {
                                    StockEligibilityModelDateHash = (Hashtable)_stockEligibilityModifierModel.ModelDateHash[sep.Key];
                                    if (StockEligibilityModelDateHash == null)
                                    {
                                        StockEligibilityModelDateHash = new Hashtable();
                                    }
                                }

                                if (StockEligibilityModelDateHash.Count == 0)       // eligible if no dates
                                {
                                    if (storeStatus == eStoreStatus.Preopen)
                                    {
                                        eligibilityBitArray[storeProfile.Key] = false;
                                    }
                                    else
                                    {
                                        eligibilityBitArray[storeProfile.Key] = true;
                                    }
                                }
                                else
                                {
                                    if (StockEligibilityModelDateHash.Contains(aFirstDayOfWeek))
                                    {
                                        eligibilityBitArray[storeProfile.Key] = true;
                                    }
                                    else
                                    {	// check for reoccurring
                                        int weekInYear = weekProfile.WeekInYear;
                                        if (StockEligibilityModelDateHash.Contains(weekInYear))
                                        {
                                            eligibilityBitArray[storeProfile.Key] = true;
                                        }
                                        else
                                        {
                                            eligibilityBitArray[storeProfile.Key] = false;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                return eligibilityBitArray;
            }
            catch
            {
                throw;
            }
        }


        /// <summary>
        /// Requests the transaction get the store eligibility settings for all stores for a node and week.
        /// </summary>
        /// <param name="packRID">The key of the pack</param>
        /// <param name="packName">The name of the pack</param>
        /// <param name="aFirstDayOfWeek">The first day of year/week for which eligibility is to be determined</param>
        /// <returns>BitArray containing eligibility settings indexed by storeRID</returns>
        public System.Collections.BitArray GetExternalStoreSalesEligibilityFlags(
            eRequestingApplication requestingApplication,
            int headerRID,
            string headerID,
            int aNodeRID,
            int packRID,
            string packName,
            int aFirstDayOfWeek
            )
        {
            try
            {
                StoreEligibilityList sel = null;
                ProfileList storeList = GetProfileList(eProfileType.Store);
                StoreEligibilityProfile sep = null;
                System.Collections.BitArray eligibilityBitArray = new System.Collections.BitArray(_maxStoreRID + 1, true);  // default to eligible
                                                                                                                            // get date information to check against open and close dates
                WeekProfile weekProfile = this.SAB.HierarchyServerSession.Calendar.GetWeek(aFirstDayOfWeek);
                DayProfile dayProfile = (DayProfile)weekProfile.Days[0];
                DateTime firstDayOfWeek = dayProfile.Date;
                dayProfile = (DayProfile)weekProfile.Days[weekProfile.DaysInWeek - 1];
                DateTime lastDayOfWeek = dayProfile.Date;

                Hashtable storeEligibilityHash;
                // select appropriate cache based on settings
                if (requestingApplication == eRequestingApplication.Forecast)
                {
                    storeEligibilityHash = PlanningSalesPackStoreEligibilityHash;
                }
                else
                {
                    storeEligibilityHash = AllocationSalesPackStoreEligibilityHash;
                }

                sel = (StoreEligibilityList)storeEligibilityHash[packRID];

                if (sel == null)
                {
                    sel = CallExternalEligibility(
                             requestingApplication: requestingApplication,
                             variable: cSalesVariable,
                             merchandiseKey: aNodeRID,
                             yearWeek: weekProfile.YearWeek,
                             storeList: storeList
                            );
                    storeEligibilityHash.Add(packRID, sel);
                }

                foreach (StoreProfile storeProfile in storeList)
                {
                    eStoreStatus storeStatus = GetStoreSalesStatus(storeProfile.Key, aFirstDayOfWeek);
                    if (storeStatus == eStoreStatus.Closed ||
                        storeProfile.SellingOpenDt > lastDayOfWeek ||
                        (storeProfile.SellingCloseDt != Include.UndefinedDate &&
                        storeProfile.SellingCloseDt < firstDayOfWeek))
                    {
                        eligibilityBitArray[storeProfile.Key] = false;
                    }
                    else
                    {
                        sep = (StoreEligibilityProfile)sel.FindKey(storeProfile.Key);
                        if (sep == null)    // if eligibility not set for store
                        {
                            if (storeStatus == eStoreStatus.Preopen)
                            {
                                eligibilityBitArray[storeProfile.Key] = false;
                            }
                        }
                        else
                        if (sep.StoreIneligible)
                        {
                            eligibilityBitArray[storeProfile.Key] = false;
                        }
                        else
                        if (sep.EligModelRID != Include.NoRID)      // check against model dates
                        {
                            if (sep.EligModelRID != _currentSalesEligibilityModelRID)
                            {

                                if (!SalesEligibilityModelHash.ContainsKey(sep.EligModelRID))
                                {
                                    LoadEligibilityModel(sep.EligModelRID);
                                    _currentStockEligibilityModelRID = sep.EligModelRID;	// set stock model since both are loaded
                                    _currentPriorityShippingModelRID = sep.EligModelRID;    // set priority shipping since loaded
                                }

                                _salesEligibilityModifierModel = (ModifierModel)SalesEligibilityModelHash[sep.EligModelRID];

                                if (!_salesEligibilityModifierModel.ContainsValuesByStore)
                                {
                                    SalesEligibilityModelDateHash = _salesEligibilityModifierModel.ModelDateHash;
                                }

                                _currentSalesEligibilityModelRID = sep.EligModelRID;
                            }

                            if (_salesEligibilityModifierModel.ContainsValuesByStore)
                            {
                                SalesEligibilityModelDateHash = (Hashtable)_salesEligibilityModifierModel.ModelDateHash[sep.Key];
                                if (SalesEligibilityModelDateHash == null)
                                {
                                    SalesEligibilityModelDateHash = new Hashtable();
                                }
                            }

                            if (SalesEligibilityModelDateHash.Count == 0)       // eligible if no dates
                            {
                                if (storeStatus == eStoreStatus.Preopen)
                                {
                                    eligibilityBitArray[storeProfile.Key] = false;
                                }
                                else
                                {
                                    eligibilityBitArray[storeProfile.Key] = true;
                                }
                            }
                            else
                            {
                                if (SalesEligibilityModelDateHash.Contains(aFirstDayOfWeek))
                                {
                                    eligibilityBitArray[storeProfile.Key] = true;
                                }
                                else
                                {	// check for reoccurring
                                    int weekInYear = weekProfile.WeekInYear;
                                    if (SalesEligibilityModelDateHash.Contains(weekInYear))
                                    {
                                        eligibilityBitArray[storeProfile.Key] = true;
                                    }
                                    else
                                    {
                                        eligibilityBitArray[storeProfile.Key] = false;
                                    }
                                }
                            }
                        }
                    }
                }
                return eligibilityBitArray;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
		/// Requests the transaction get the store eligibility settings for all stores for a node and week.
		/// </summary>
		/// <param name="packRID">The key of the pack</param>
        /// <param name="packName">The name of the pack</param>
		/// <param name="aFirstDayOfWeek">The first day of year/week for which eligibility is to be determined</param>
		/// <returns>BitArray containing eligibility settings indexed by storeRID</returns>
		public System.Collections.BitArray GetExternalStoreStockEligibilityFlags(
            eRequestingApplication requestingApplication,
            int headerRID,
            string headerID,
            int aNodeRID,
            int packRID,
            string packName,
            int aFirstDayOfWeek
            )
        {
            try
            {
                StoreEligibilityList sel = null;
                ProfileList storeList = GetProfileList(eProfileType.Store);
                StoreEligibilityProfile sep = null;
                System.Collections.BitArray eligibilityBitArray = new System.Collections.BitArray(_maxStoreRID + 1, true);  // default to eligible
                                                                                                                            // get date information to check against open and close dates
                WeekProfile weekProfile = this.SAB.HierarchyServerSession.Calendar.GetWeek(aFirstDayOfWeek);
                DayProfile dayProfile = (DayProfile)weekProfile.Days[0];
                DateTime firstDayOfWeek = dayProfile.Date;
                dayProfile = (DayProfile)weekProfile.Days[weekProfile.DaysInWeek - 1];
                DateTime lastDayOfWeek = dayProfile.Date;

                Hashtable storeEligibilityHash;
                // select appropriate cache based on settings
                if (requestingApplication == eRequestingApplication.Forecast)
                {
                    storeEligibilityHash = PlanningStockPackStoreEligibilityHash;
                }
                else
                {
                    storeEligibilityHash = AllocationStockPackStoreEligibilityHash;
                }

                sel = (StoreEligibilityList)storeEligibilityHash[packRID];

                if (sel == null)
                {
                    sel = CallExternalEligibility(
                             requestingApplication: requestingApplication,
                             variable: cSalesVariable,
                             merchandiseKey: aNodeRID,
                             yearWeek: weekProfile.YearWeek,
                             storeList: storeList
                            );
                    storeEligibilityHash.Add(packRID, sel);
                }

                foreach (StoreProfile storeProfile in storeList)
                {
                    eStoreStatus storeStatus = GetStoreStockStatus(storeProfile.Key, aFirstDayOfWeek);
                    if (storeStatus == eStoreStatus.Closed ||
                        storeProfile.StockOpenDt > lastDayOfWeek ||
                        (storeProfile.StockCloseDt != Include.UndefinedDate &&
                        storeProfile.StockCloseDt < firstDayOfWeek))
                    {
                        eligibilityBitArray[storeProfile.Key] = false;
                    }
                    else
                    {
                        sep = (StoreEligibilityProfile)sel.FindKey(storeProfile.Key);
                        if (sep == null)    // if eligibility not defined for the store
                        {
                            if (storeStatus == eStoreStatus.Preopen)
                            {
                                eligibilityBitArray[storeProfile.Key] = false;
                            }
                        }
                        else
                            if (sep.StoreIneligible)
                        {
                            eligibilityBitArray[storeProfile.Key] = false;
                        }
                        else
                        {
                            if (sep.EligModelRID != Include.NoRID)      // check against model dates
                            {
                                if (sep.EligModelRID != _currentStockEligibilityModelRID)
                                {

                                    if (!StockEligibilityModelHash.ContainsKey(sep.EligModelRID))
                                    {
                                        LoadEligibilityModel(sep.EligModelRID);
                                        _currentSalesEligibilityModelRID = sep.EligModelRID;	// set sales model since both are loaded
                                        _currentPriorityShippingModelRID = sep.EligModelRID;    // set priority shipping since loaded
                                    }

                                    _stockEligibilityModifierModel = (ModifierModel)StockEligibilityModelHash[sep.EligModelRID];

                                    if (!_stockEligibilityModifierModel.ContainsValuesByStore)
                                    {
                                        StockEligibilityModelDateHash = _stockEligibilityModifierModel.ModelDateHash;
                                    }
                                    _currentStockEligibilityModelRID = sep.EligModelRID;
                                }

                                if (_stockEligibilityModifierModel.ContainsValuesByStore)
                                {
                                    StockEligibilityModelDateHash = (Hashtable)_stockEligibilityModifierModel.ModelDateHash[sep.Key];
                                    if (StockEligibilityModelDateHash == null)
                                    {
                                        StockEligibilityModelDateHash = new Hashtable();
                                    }
                                }

                                if (StockEligibilityModelDateHash.Count == 0)       // eligible if no dates
                                {
                                    if (storeStatus == eStoreStatus.Preopen)
                                    {
                                        eligibilityBitArray[storeProfile.Key] = false;
                                    }
                                    else
                                    {
                                        eligibilityBitArray[storeProfile.Key] = true;
                                    }
                                }
                                else
                                {
                                    if (StockEligibilityModelDateHash.Contains(aFirstDayOfWeek))
                                    {
                                        eligibilityBitArray[storeProfile.Key] = true;
                                    }
                                    else
                                    {	// check for reoccurring
                                        int weekInYear = weekProfile.WeekInYear;
                                        if (StockEligibilityModelDateHash.Contains(weekInYear))
                                        {
                                            eligibilityBitArray[storeProfile.Key] = true;
                                        }
                                        else
                                        {
                                            eligibilityBitArray[storeProfile.Key] = false;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                return eligibilityBitArray;
            }
            catch
            {
                throw;
            }
        }

        private StoreEligibilityList CallExternalEligibility(
            eRequestingApplication requestingApplication,
            int variable,
            int merchandiseKey,
            int yearWeek,
            ProfileList storeList
            )
        {
            try
            {
                string color = null;
                // build request to call API
                ROEligibilityRequest eligibilityRequest = new ROEligibilityRequest();
                ROEligibilityStore eligibilityStore;
                StoreEligibilityList sel = new StoreEligibilityList(eProfileType.StoreEligibility);
                StoreEligibilityProfile sep;
                string channel = null;
                string merchandise = null;
                HierarchyNodeProfile hierarchyNodeProfile;

                hierarchyNodeProfile = GetHierarchyNodeProfile(merchandiseKey: merchandiseKey);
                if (hierarchyNodeProfile != null)
                {
                    switch (GlobalOptions.ExternalEligibilityProductIdentifier)
                    {
                        case eExternalEligibilityProductIdentifier.Name:
                            if (hierarchyNodeProfile.LevelType == eHierarchyLevelType.Color)
                            {
                                HierarchyNodeProfile styleHierarchyNodeProfile = GetHierarchyNodeProfile(
                                    merchandiseKey: hierarchyNodeProfile.HomeHierarchyParentRID);
                                merchandise = styleHierarchyNodeProfile.NodeName;
                                color = hierarchyNodeProfile.NodeName;
                            }
                            else
                            {
                                merchandise = hierarchyNodeProfile.NodeName;
                            }
                            break;
                        case eExternalEligibilityProductIdentifier.NameConcatColorName:
                            merchandise = hierarchyNodeProfile.NodeName;
                            break;
                        default:
                            if (hierarchyNodeProfile.LevelType == eHierarchyLevelType.Color)
                            {
                                merchandise = hierarchyNodeProfile.NodeID;
                                HierarchyNodeProfile styleHierarchyNodeProfile = GetHierarchyNodeProfile(
                                    merchandiseKey: hierarchyNodeProfile.HomeHierarchyParentRID);
                                merchandise = styleHierarchyNodeProfile.NodeID;
                                color = hierarchyNodeProfile.NodeID;
                            }
                            else
                            {
                                merchandise = hierarchyNodeProfile.NodeID;
                            }
                            break;
                    }
                }

                eligibilityRequest.RequestingApplication = requestingApplication.GetHashCode();
                eligibilityRequest.Merchandise = merchandise;
                if (!string.IsNullOrEmpty(color))
                {
                    eligibilityRequest.Color = color;
                }
                eligibilityRequest.Variable = variable;
                eligibilityRequest.YearWeek = yearWeek;

                bool reserveStoreFound = false;

                foreach (StoreProfile storeProfile in storeList)
                {
                    // do not include the reserve store
                    if (storeProfile.Key != GlobalOptions.ReserveStoreRID)
                    {
                        switch (GlobalOptions.ExternalEligibilityChannelIdentifier)
                        {
                            case eExternalEligibilityChannelIdentifier.Name:
                                channel = storeProfile.StoreName;
                                break;
                            default:
                                channel = storeProfile.StoreId;
                                break;
                        }

                        // create a store key lookup
                        if (!StoreHash.ContainsKey(channel))
                        {
                            StoreHash.Add(channel, storeProfile.Key);
                        }

                        eligibilityStore = new ROEligibilityStore(
                            channel: channel
                            );
                        eligibilityRequest.EligibilityStores.Add(eligibilityStore);
                    }
                    else
                    {
                        reserveStoreFound = true;
                    }
                }

                string message = "Calling External Eligibility at " + GlobalOptions.ExternalEligibilityURL
                    + " for merchandise " + eligibilityRequest.Merchandise;
                if (!string.IsNullOrEmpty(eligibilityRequest.Color))
                {
                    message += " and color " + eligibilityRequest.Color;
                }
                message += " week " + eligibilityRequest.YearWeek;

                DateTime startTime = DateTime.Now;
                SAB.HierarchyServerSession.Audit.Add_Msg(
                    eMIDMessageLevel.Information,
                    message,
                    GetType().Name
                    );

                ROEligibilityRequest eligibilityResponse = MakeTheCall(eligibilityRequest: eligibilityRequest).GetAwaiter().GetResult();

                TimeSpan duration = DateTime.Now.Subtract(startTime);
                string strDuration = Convert.ToString(duration, CultureInfo.CurrentUICulture);

                message = "External Eligibility responded in " + strDuration;
                SAB.HierarchyServerSession.Audit.Add_Msg(
                    eMIDMessageLevel.Information,
                    message,
                    GetType().Name
                    );

                // Add reserve store back to the list.
                if (reserveStoreFound)
                {
                    sep = new StoreEligibilityProfile(aKey: GlobalOptions.ReserveStoreRID);
                    sep.StoreIneligible = false;
                    sel.Add(sep);
                }

                int storeKey = Include.NoRID;

                foreach (ROEligibilityStore outputEligibilityStore in eligibilityResponse.EligibilityStores)
                {
                    if (StoreHash.TryGetValue(outputEligibilityStore.Channel, out storeKey))
                    {
                        sep = new StoreEligibilityProfile(aKey: storeKey);
                        if (outputEligibilityStore.IsEligible)
                        {
                            sep.StoreIneligible = false;
                        }
                        else
                        {
                            sep.StoreIneligible = true;
                        }

                        sel.Add(sep);
                    }
                    else
                    {
                        SAB.HierarchyServerSession.Audit.Add_Msg(
                            eMIDMessageLevel.Error,
                            "Unable to determine eligibility for store " + outputEligibilityStore.Channel,
                            GetType().Name
                            );
                    }
                }

                return sel;
            }
            catch (System.Net.Http.HttpRequestException ex)
            {
                SAB.HierarchyServerSession.Audit.Add_Msg(
                    eMIDMessageLevel.Error,
                    ex.Message + " " + ex.InnerException.Message,
                    GetType().Name
                    );
                throw;
            }
            catch (Exception ex)
            {
                SAB.HierarchyServerSession.Audit.Add_Msg(
                    eMIDMessageLevel.Error,
                    ex.Message,
                    GetType().Name
                    );
                throw;
            }
        }

        private async Task<ROEligibilityRequest> MakeTheCall(ROEligibilityRequest eligibilityRequest)
        {
            try
            {
                ROEligibilityRequest eligibilityRequestResponse = null;
                HttpClient client = new HttpClient();

                var maxRetryAttempts = 3;
                var pauseBetweenFailures = TimeSpan.FromSeconds(2);

                var retryPolicy = Policy
                    .Handle<HttpRequestException>()
                    .WaitAndRetryAsync(maxRetryAttempts, i => pauseBetweenFailures);

                var timeoutPolicy = Policy
                    .TimeoutAsync(20);

                var json = JsonConvert.SerializeObject(eligibilityRequest);
                var data = new StringContent(json, Encoding.UTF8, "application/json");

                eligibilityRequestResponse = await retryPolicy
                    .WrapAsync(timeoutPolicy)
                    .ExecuteAsync(async () => 
                {
                    using (HttpResponseMessage response = await client.PostAsync(GlobalOptions.ExternalEligibilityURL.Trim(), data)
                        .ConfigureAwait(false))
                    {
                        var responseString = await response.Content.ReadAsStringAsync();
                        return JsonConvert.DeserializeObject<ROEligibilityRequest>(responseString);
                    }
                }).ConfigureAwait(false);

                return eligibilityRequestResponse;
            }
            catch
            {
                throw;
            }
        }

        private HierarchyNodeProfile GetHierarchyNodeProfile(int merchandiseKey)
        {
            HierarchyNodeProfile hierarchyNodeProfile = null;

            if (!NodeHash.TryGetValue(merchandiseKey, out hierarchyNodeProfile))
            {
                hierarchyNodeProfile = SAB.HierarchyServerSession.GetNodeData(
                    aNodeRID: merchandiseKey,
                    aChaseHierarchy: true,
                    aBuildQualifiedID: true
                    );
                NodeHash.Add(merchandiseKey, hierarchyNodeProfile);
            }

            return hierarchyNodeProfile;
        }
    }

    public class ROEligibilityStore
    {
        private string _channel;
        private bool _isEligible;

        #region Public Properties
        /// <summary>
        /// Gets or sets the name of the channel.
        /// </summary>
        public string Channel
        {
            get { return _channel; }
            set { _channel = value; }
        }

        /// <summary>
        /// Gets or sets the flag identifying if the channel is eligible.
        /// </summary>
        public bool IsEligible
        {
            get { return _isEligible; }
            set { _isEligible = value; }
        }

        #endregion

        public ROEligibilityStore(
            string channel)
        {
            _channel = channel;
            _isEligible = true;
        }
    }

    public class ROEligibilityRequest
    {
        private int _requestingApplication;
        private string _merchandise;
        private string _color;
        private int _variable;
        private int _yearWeek;
        private List<ROEligibilityStore> _eligibilityStores;


        #region Public Properties
        /// <summary>
        /// Gets or sets the requesting application.
        /// </summary>
        /// <remarks>
        /// Valid values
        ///   0: Allocation
        ///   1: Planning
        /// </remarks>
        public int RequestingApplication
        {
            get { return _requestingApplication; }
            set { _requestingApplication = value; }
        }

        
        /// <summary>
        /// Gets or sets the merchandise for which eligibility is to be determined.
        /// </summary>
        public string Merchandise
        {
            get { return _merchandise; }
            set { _merchandise = value; }
        }

        /// <summary>
        /// Gets or sets the color for which eligibility is to be determined if color is included.
        /// </summary>
        public string Color
        {
            get { return _color; }
            set { _color = value; }
        }

        /// <summary>
        /// Gets or sets the variable for which eligibility is to be determined.
        /// </summary>
        /// <remarks>
        /// Valid values
        ///   0: Stock
        ///   1: Sales
        /// </remarks>
        public int Variable
        {
            get { return _variable; }
            set { _variable = value; }
        }

        /// <summary>
        /// Gets or sets the year and week for which eligibility is to be determined.
        /// </summary>
        public int YearWeek
        {
            get { return _yearWeek; }
            set { _yearWeek = value; }
        }

        /// <summary>
        /// Gets or sets the list of simlar stores identified for the store.
        /// </summary>
        public List<ROEligibilityStore> EligibilityStores
        {
            get { return _eligibilityStores; }
        }

        #endregion

        public ROEligibilityRequest()
        {
            _color = null;
            _eligibilityStores = new List<ROEligibilityStore>();
        }

        
    }
}
