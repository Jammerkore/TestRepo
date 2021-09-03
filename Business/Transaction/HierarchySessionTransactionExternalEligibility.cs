using System;
using System.Globalization;
using MIDRetail.Data;
using MIDRetail.Common;
using MIDRetail.DataCommon;
using System.Collections;
using System.Collections.Generic;
using MIDRetail.Business.Allocation;

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

                sel = (StoreEligibilityList)StoreEligibilityHash[aNodeRID];

                if (sel == null)
                {
                    sel = SAB.HierarchyServerSession.GetStoreEligibilityList(storeList, aNodeRID, true, false);
                    StoreEligibilityHash.Add(aNodeRID, sel);
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
                                // End TT#2307
                                _currentSalesEligibilityModelRID = sep.EligModelRID;
                            }

                            // Begin TT#2307 - JSmith - Incorrect Stock Values
                            if (_salesEligibilityModifierModel.ContainsValuesByStore)
                            {
                                SalesEligibilityModelDateHash = (Hashtable)_salesEligibilityModifierModel.ModelDateHash[sep.Key];
                                if (SalesEligibilityModelDateHash == null)
                                {
                                    SalesEligibilityModelDateHash = new Hashtable();
                                }
                            }
                            // End TT#2307

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
		public System.Collections.BitArray GetStoreExternalStockEligibilityFlags(
            eRequestingApplication requestingApplication,
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

                sel = (StoreEligibilityList)StoreEligibilityHash[aNodeRID];

                if (sel == null)
                {
                    sel = SAB.HierarchyServerSession.GetStoreEligibilityList(storeList, aNodeRID, true, false);
                    StoreEligibilityHash.Add(aNodeRID, sel);
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
        /// <param name="colorName">The name of the color</param>
        /// <param name="colorNodeRID">The key of the node for the color in the hierarchy</param>
        /// <param name="aFirstDayOfWeek">The first day of year/week for which eligibility is to be determined</param>
        /// <returns>BitArray containing eligibility settings indexed by storeRID</returns>
        public System.Collections.BitArray GetExternalStoreSalesEligibilityFlags(
            eRequestingApplication requestingApplication,
            int colorCodeRID,
            string colorName,
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

                sel = (StoreEligibilityList)ColorStoreEligibilityHash[colorNodeRID];

                if (sel == null)
                {
                    sel = SAB.HierarchyServerSession.GetStoreEligibilityList(storeList, colorNodeRID, true, false);
                    ColorStoreEligibilityHash.Add(colorNodeRID, sel);
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
        /// <param name="colorName">The name of the color</param>
        /// <param name="colorNodeRID">The key of the node for the color in the hierarchy</param>
		/// <param name="aFirstDayOfWeek">The first day of year/week for which eligibility is to be determined</param>
		/// <returns>BitArray containing eligibility settings indexed by storeRID</returns>
		public System.Collections.BitArray GetExternalStoreStockEligibilityFlags(
            eRequestingApplication requestingApplication,
            int colorCodeRID,
            string colorName,
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

                sel = (StoreEligibilityList)ColorStoreEligibilityHash[colorNodeRID];

                if (sel == null)
                {
                    sel = SAB.HierarchyServerSession.GetStoreEligibilityList(storeList, colorNodeRID, true, false);
                    ColorStoreEligibilityHash.Add(colorNodeRID, sel);
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

                sel = (StoreEligibilityList)PackStoreEligibilityHash[packRID];

                if (sel == null)
                {
                    // TESTING - get values from node associated with header/pack
                    int nodeRID = SetNodeForPack(
                        headerRID: headerRID,
                        packRID: packRID,
                        packName: packName
                        );
                    sel = SAB.HierarchyServerSession.GetStoreEligibilityList(storeList, nodeRID, true, false);
                    PackStoreEligibilityHash.Add(packRID, sel);
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
                                // End TT#2307
                                _currentSalesEligibilityModelRID = sep.EligModelRID;
                            }

                            // Begin TT#2307 - JSmith - Incorrect Stock Values
                            if (_salesEligibilityModifierModel.ContainsValuesByStore)
                            {
                                SalesEligibilityModelDateHash = (Hashtable)_salesEligibilityModifierModel.ModelDateHash[sep.Key];
                                if (SalesEligibilityModelDateHash == null)
                                {
                                    SalesEligibilityModelDateHash = new Hashtable();
                                }
                            }
                            // End TT#2307

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

                sel = (StoreEligibilityList)PackStoreEligibilityHash[packRID];

                if (sel == null)
                {
                    // TESTING - get values from node associated with header/pack
                    int nodeRID = SetNodeForPack(
                        headerRID: headerRID,
                        packRID: packRID,
                        packName: packName
                        );
                    sel = SAB.HierarchyServerSession.GetStoreEligibilityList(storeList, nodeRID, true, false);
                    PackStoreEligibilityHash.Add(packRID, sel);
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

        // TESTING - get values from node associated with header/pack
        private int SetNodeForPack(
            int headerRID,
            int packRID,
            string packName)
        {

            //HDR_RID   HDR_ID
            //10009     01_PE_ELIG_1style - 1clr - 1pack

            if (headerRID == 10009)
            {
                return 102;
            }

            return 101;
        }

    }
}
