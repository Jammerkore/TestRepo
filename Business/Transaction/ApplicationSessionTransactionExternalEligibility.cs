using System;
using System.IO;
using System.Data;
using System.Configuration;
using System.Globalization;
using System.Text;
//using System.Runtime.Remoting;
//using System.Runtime.Remoting.Lifetime;
using MIDRetail.Data;
using MIDRetail.Common;
using MIDRetail.DataCommon;
using System.Collections;
using System.Collections.Generic;
using MIDRetail.Business.Allocation;
using System.Diagnostics;

using Logility.ROWebSharedTypes;

namespace MIDRetail.Business
{
	/// <summary>
	/// The Transaction class defines the "local" workspace for a series of functions in the client application.
	/// </summary>
	/// <remarks>
	/// This class gives the user the ability to store information that is "local", or unique, to a series of screens or functions.  This allows a Client
	/// application to open multiple functions of the same type, yet each has its own copy of information contained in this class.
	/// </remarks>

	public partial class ApplicationSessionTransaction : Transaction
	{
        //=======
        // FIELDS
        //=======

        //===========
        // PROPERTIES
        //===========

        //===========
        // METHODS
        //===========


        /// <summary>
        /// Returns the stock eligibility of a single store for a given node and year/week
        /// </summary>
        /// <param name="aColor">
        /// The color
        /// </param>
        /// <param name="storeRID">
        /// The record ID of the store
        /// </param>
        /// <param name="yearWeek">
        /// The year/week for which eligibility is to be determined
        /// </param>
        /// <returns>
        /// A boolean identifying if the store is eligible
        /// </returns>
        public bool GetStoreEligibilityForStock(
            eRequestingApplication requestingApplication,
            int nodeRID, 
            HdrColorBin aColor, 
            int storeRID, 
            int yearWeek
            )
        {
            try
            {
                return DetermineStockEligibility(requestingApplication, aColor, storeRID, yearWeek);
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

        /// <summary>
        /// Returns the stock eligibility of a list of store and year/week combinations
        /// </summary>
        /// <param name="aColor">
        /// The color
        /// </param>
        /// <param name="swel">
        /// A ProfileList containing instances of the StoreWeekEligibilityProfile class
        /// </param>
        /// <returns>
        /// The StoreWeekEligibilityList object containing the eligibility settings.
        /// </returns>
        /// <remarks>
        /// This method will not set the store's priority shipping status.
        /// </remarks>
        public StoreWeekEligibilityList GetStoreEligibilityForStock(
            eRequestingApplication requestingApplication,
            int nodeRID, 
            HdrColorBin aColor, 
            StoreWeekEligibilityList swel
            )
        {
            try
            {
                return GetStoreEligibilityForStock(requestingApplication, nodeRID, aColor, swel, false);
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

        /// <summary>
        /// Returns the stock eligibility of a list of store and year/week combinations
        /// </summary>
        /// <param name="aColor">
        /// The color
        /// </param>
        /// <param name="swel">
        /// A ProfileList containing instances of the StoreWeekEligibilityProfile class
        /// </param>
        /// <param name="setPriorityShipping">
        /// A flag identifying that the priority shipping is to be determined
        /// </param>
        /// <returns>
        /// The StoreWeekEligibilityList object containing the eligibility settings.
        /// </returns>
        public StoreWeekEligibilityList GetStoreEligibilityForStock(
            eRequestingApplication requestingApplication,
            int nodeRID, 
            HdrColorBin aColor, 
            StoreWeekEligibilityList swel, 
            bool setPriorityShipping
            )
        {
            try
            {
                foreach (StoreWeekEligibilityProfile swep in swel)
                {
                    swep.StoreIsEligible = DetermineStockEligibility(requestingApplication, aColor, swep.Key, swep.YearWeek);
                    if (setPriorityShipping)
                    {
                        swep.StoreIsPriorityShipper = DeterminePriorityShipping(nodeRID, swep.Key, swep.YearWeek);
                    }
                }
                return swel;
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

        /// <summary>
        /// Returns the stock eligibility of all stores for a given node and year/week
        /// </summary>
        /// <param name="aColor">
        /// The color
        /// </param>
        /// <param name="yearWeek">
        /// The year/week for which eligibility is to be determined
        /// </param>
        /// <returns>
        /// The StoreWeekEligibilityList object containing the eligibility settings.
        /// </returns>
        /// <remarks>
        /// This method will not set the store's priority shipping status.
        /// </remarks>
        public StoreWeekEligibilityList GetStoreEligibilityForStock(
            eRequestingApplication requestingApplication,
            int nodeRID,
            HdrColorBin aColor,
            int yearWeek
            )
        {
            try
            {
                return GetStoreEligibilityForStock(requestingApplication, nodeRID, aColor, yearWeek, false);
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

        /// <summary>
        /// Returns the stock eligibility of all stores for a given node and year/week
        /// </summary>
        /// <param name="aColor">
        /// The color
        /// </param>
        /// <param name="yearWeek">
        /// The year/week for which eligibility is to be determined
        /// </param>
        /// <param name="setPriorityShipping">
        /// A flag identifying that the priority shipping is to be determined
        /// </param>
        /// <returns>
        /// The StoreWeekEligibilityList object containing the eligibility settings.
        /// </returns>
        public StoreWeekEligibilityList GetStoreEligibilityForStock(
            eRequestingApplication requestingApplication,
            int nodeRID, 
            HdrColorBin aColor, 
            int yearWeek, 
            bool setPriorityShipping
            )
        {
            try
            {
                StoreWeekEligibilityList swel = new StoreWeekEligibilityList(eProfileType.StoreEligibility);
                StoreWeekEligibilityProfile swep = null;
                ProfileList allStoreList = GetProfileList(eProfileType.Store);
                foreach (StoreProfile sp in allStoreList.ArrayList)
                {
                    swep = new StoreWeekEligibilityProfile(sp.Key);
                    swep.YearWeek = yearWeek;
                    swep.StoreIsEligible = DetermineStockEligibility(requestingApplication, aColor, swep.Key, swep.YearWeek);
                    if (setPriorityShipping)
                    {
                        swep.StoreIsPriorityShipper = DeterminePriorityShipping(nodeRID, swep.Key, swep.YearWeek);
                    }
                    swel.Add(swep);
                }
                return swel;
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

        /// <summary>
        /// Returns the stock eligibility a provided list of stores for a given node and year/week
        /// </summary>
        /// <param name="storeList">
        /// A ProfileList containing StoreProfiles for the store to determine eligibility
        /// </param>
        /// <param name="aColor">
        /// The color
        /// </param>
        /// <param name="yearWeek">
        /// The year/week for which eligibility is to be determined
        /// </param>
        /// <returns>
        /// The StoreWeekEligibilityList object containing the eligibility settings.
        /// </returns>
        /// <remarks>
        /// This method will not set the store's priority shipping status.
        /// </remarks>
        public StoreWeekEligibilityList GetStoreEligibilityForStock(
            eRequestingApplication requestingApplication,
            ProfileList storeList, 
            int nodeRID, 
            HdrColorBin aColor, 
            int yearWeek
            )
        {
            try
            {
                return GetStoreEligibilityForStock(requestingApplication, storeList, nodeRID, aColor, yearWeek, false);
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

        /// <summary>
        /// Returns the stock eligibility a provided list of stores for a given node and year/week
        /// </summary>
        /// <param name="storeList">
        /// A ProfileList containing StoreProfiles for the store to determine eligibility
        /// </param>
        /// <param name="aColor">
        /// The color
        /// </param>
        /// <param name="yearWeek">
        /// The year/week for which eligibility is to be determined
        /// </param>
        /// <param name="setPriorityShipping">
        /// A flag identifying that the priority shipping is to be determined
        /// </param>
        /// <returns>
        /// The StoreWeekEligibilityList object containing the eligibility settings.
        /// </returns>
        public StoreWeekEligibilityList GetStoreEligibilityForStock(
            eRequestingApplication requestingApplication,
            ProfileList storeList, 
            int nodeRID, 
            HdrColorBin aColor, 
            int yearWeek, 
            bool setPriorityShipping
            )
        {
            try
            {
                StoreWeekEligibilityList swel = new StoreWeekEligibilityList(eProfileType.StoreEligibility);
                StoreWeekEligibilityProfile swep = null;
                foreach (StoreProfile sp in storeList.ArrayList)
                {
                    swep = new StoreWeekEligibilityProfile(sp.Key);
                    swep.YearWeek = yearWeek;
                    swep.StoreIsEligible = DetermineStockEligibility(requestingApplication, aColor, swep.Key, swep.YearWeek);
                    if (setPriorityShipping)
                    {
                        swep.StoreIsPriorityShipper = DeterminePriorityShipping(nodeRID, swep.Key, swep.YearWeek);
                    }
                    swel.Add(swep);
                }
                return swel;
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

        /// <summary>
        /// Determines the stock eligibility of a store for a given node and year/week
        /// </summary>
        /// <param name="aColor">
        /// The color
        /// </param>
        /// <param name="storeRID">
        /// The record ID of the store
        /// </param>
        /// <param name="yearWeek">
        /// The year/week for which eligibility is to be determined
        /// </param>
        /// <returns>
        /// A boolean identifying if the store is eligible
        /// </returns>
        /// <remarks>This routine will accept a year/week as either YYYYWW or YYYYDDD</remarks>
        private bool DetermineStockEligibility(
            eRequestingApplication requestingApplication,
            HdrColorBin aColor, 
            int storeRID, 
            int yearWeek
            )
        {
            try
            {
                if (yearWeek < 1000000)  // YYYYWW format
                {
                    // convert to YYYYDDD
                    yearWeek = this.SAB.ApplicationServerSession.Calendar.GetWeekKey(yearWeek);
                }

                int colorNodeRID = aColor.ColorNodeRID;

                System.Collections.Hashtable colorStockEligibilityHashByNodeRID;
                // select appropriate cache based on settings
                if (requestingApplication == eRequestingApplication.Forecast)
                {
                    colorStockEligibilityHashByNodeRID = PlanningColorStockEligibilityHashByNodeRID;
                }
                else
                {
                    colorStockEligibilityHashByNodeRID = AllocationColorStockEligibilityHashByNodeRID;
                }

                if (colorNodeRID != _colorCurrentStockEligibilityNodeRID) // if not same node, get year/week Hashtable		
                {
                    _colorStockEligibilityHashByYearWeek = (System.Collections.Hashtable)colorStockEligibilityHashByNodeRID[colorNodeRID];
                    if (_colorStockEligibilityHashByYearWeek == null)
                    {
                        _colorStockEligibilityHashByYearWeek = new System.Collections.Hashtable();
                        colorStockEligibilityHashByNodeRID.Add(colorNodeRID, _colorStockEligibilityHashByYearWeek);
                    }
                    _colorCurrentStockEligibilityNodeRID = colorNodeRID;
                    _colorCurrentStockEligibilityYearWeek = -1;  // reset current yearWeek since new node
                }

                if (yearWeek != _colorCurrentStockEligibilityYearWeek)   // if not same week, get BitArray for year/week
                {
                    _colorStockEligibilityBitArray = (System.Collections.BitArray)_colorStockEligibilityHashByYearWeek[yearWeek];
                    if (_colorStockEligibilityBitArray == null)
                    {
                        _colorStockEligibilityBitArray = HierarchySessionTransaction.GetExternalStoreStockEligibilityFlags(
                            requestingApplication,
                            aColor.HdrRID,
                            aColor.HdrID,
                            aColor.ColorCodeRID,
                            aColor.ColorID,
                            colorNodeRID,
                            yearWeek
                            );
                        _colorStockEligibilityHashByYearWeek.Add(yearWeek, _colorStockEligibilityBitArray);
                    }
                    _colorCurrentStockEligibilityYearWeek = yearWeek;
                }

                return _colorStockEligibilityBitArray[storeRID];
            }
            catch (System.NullReferenceException)
            {
                return false;
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

        /// <summary>
        /// Returns the Sales eligibility of a single store for a given node and year/week
        /// </summary>
        /// <param name="aColor">
        /// The color
        /// </param>
        /// <param name="storeRID">
        /// The record ID of the store
        /// </param>
        /// <param name="yearWeek">
        /// The year/week for which eligibility is to be determined
        /// </param>
        /// <returns>
        /// A boolean identifying if the store is eligible
        /// </returns>
        public bool GetStoreEligibilityForSales(
            eRequestingApplication requestingApplication,
            int nodeRID, 
            HdrColorBin aColor, 
            int storeRID, 
            int yearWeek
            )
        {
            try
            {
                return DetermineSalesEligibility(requestingApplication, nodeRID, aColor, storeRID, yearWeek);
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

        /// <summary>
        /// Returns the Sales eligibility of a list of store and year/week combinations
        /// </summary>
        /// <param name="aColor">
        /// The color
        /// </param>
        /// <param name="swel">
        /// A ProfileList containing instances of the StoreWeekEligibilityProfile class
        /// </param>
        /// <returns>
        /// The StoreWeekEligibilityList object containing the eligibility settings.
        /// </returns>
        /// <remarks>
        /// This method will not set the store's priority shipping status.
        /// </remarks>
        public StoreWeekEligibilityList GetStoreEligibilityForSales(
            eRequestingApplication requestingApplication,
            int nodeRID, 
            HdrColorBin aColor, 
            StoreWeekEligibilityList swel
            )
        {
            try
            {
                return GetStoreEligibilityForSales(requestingApplication, nodeRID, aColor, swel, false);
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

        /// <summary>
        /// Returns the Sales eligibility of a list of store and year/week combinations
        /// </summary>
        /// <param name="aColor">
        /// The color
        /// </param>
        /// <param name="swel">
        /// A ProfileList containing instances of the StoreWeekEligibilityProfile class
        /// </param>
        /// <param name="setPriorityShipping">
        /// A flag identifying that the priority shipping is to be determined
        /// </param>
        /// <returns>
        /// The StoreWeekEligibilityList object containing the eligibility settings.
        /// </returns>
        public StoreWeekEligibilityList GetStoreEligibilityForSales(
            eRequestingApplication requestingApplication,
            int nodeRID, 
            HdrColorBin aColor, 
            StoreWeekEligibilityList swel, 
            bool setPriorityShipping
            )
        {
            try
            {
                foreach (StoreWeekEligibilityProfile swep in swel)
                {
                    swep.StoreIsEligible = DetermineSalesEligibility(requestingApplication, aColor.ColorNodeRID, aColor, swep.Key, swep.YearWeek);
                    if (setPriorityShipping)
                    {
                        swep.StoreIsPriorityShipper = DeterminePriorityShipping(nodeRID, swep.Key, swep.YearWeek);
                    }
                }
                return swel;
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

        /// <summary>
        /// Returns the Sales eligibility of all stores for a given node and year/week
        /// </summary>
        /// <param name="aColor">
        /// The color
        /// </param>
        /// <param name="yearWeek">
        /// The year/week for which eligibility is to be determined
        /// </param>
        /// <returns>
        /// The StoreWeekEligibilityList object containing the eligibility settings.
        /// </returns>
        /// <remarks>
        /// This method will not set the store's priority shipping status.
        /// </remarks>
        public StoreWeekEligibilityList GetStoreEligibilityForSales(
            eRequestingApplication requestingApplication,
            int nodeRID, 
            HdrColorBin aColor, 
            int yearWeek
            )
        {
            try
            {
                return GetStoreEligibilityForSales(requestingApplication, nodeRID, aColor, yearWeek, false);
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

        /// <summary>
        /// Returns the Sales eligibility of all stores for a given node and year/week
        /// </summary>
        /// <param name="aColor">
        /// The color
        /// </param>
        /// <param name="yearWeek">
        /// The year/week for which eligibility is to be determined
        /// </param>
        /// <param name="setPriorityShipping">
        /// A flag identifying that the priority shipping is to be determined
        /// </param>
        /// <returns>
        /// The StoreWeekEligibilityList object containing the eligibility settings.
        /// </returns>
        public StoreWeekEligibilityList GetStoreEligibilityForSales(
            eRequestingApplication requestingApplication,
            int nodeRID, 
            HdrColorBin aColor, 
            int yearWeek, 
            bool setPriorityShipping
            )
        {
            try
            {
                StoreWeekEligibilityList swel = new StoreWeekEligibilityList(eProfileType.StoreEligibility);
                StoreWeekEligibilityProfile swep = null;
                ProfileList allStoreList = GetProfileList(eProfileType.Store);
                foreach (StoreProfile sp in allStoreList.ArrayList)
                {
                    swep = new StoreWeekEligibilityProfile(sp.Key);
                    swep.YearWeek = yearWeek;
                    swep.StoreIsEligible = DetermineSalesEligibility(requestingApplication, aColor.ColorNodeRID, aColor, swep.Key, swep.YearWeek);
                    if (setPriorityShipping)
                    {
                        swep.StoreIsPriorityShipper = DeterminePriorityShipping(nodeRID, swep.Key, swep.YearWeek);
                    }
                    swel.Add(swep);
                }
                return swel;
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

        /// <summary>
        /// Returns the Sales eligibility a provided list of stores for a given node and year/week
        /// </summary>
        /// <param name="storeList">
        /// A ProfileList containing StoreProfiles for the store to determine eligibility
        /// </param>
        /// <param name="aColor">
        /// The color
        /// </param>
        /// <param name="yearWeek">
        /// The year/week for which eligibility is to be determined
        /// </param>
        /// <returns>
        /// The StoreWeekEligibilityList object containing the eligibility settings.
        /// </returns>
        /// <remarks>
        /// This method will not set the store's priority shipping status.
        /// </remarks>
        public StoreWeekEligibilityList GetStoreEligibilityForSales(
            eRequestingApplication requestingApplication,
            ProfileList storeList, 
            int nodeRID, 
            HdrColorBin aColor,
            int yearWeek
            )
        {
            try
            {
                return GetStoreEligibilityForSales(requestingApplication, storeList, nodeRID, aColor, yearWeek, false);
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

        /// <summary>
        /// Returns the Sales eligibility a provided list of stores for a given node and year/week
        /// </summary>
        /// <param name="storeList">
        /// A ProfileList containing StoreProfiles for the store to determine eligibility
        /// </param>
        /// <param name="aColor">
        /// The color
        /// </param>
        /// <param name="yearWeek">
        /// The year/week for which eligibility is to be determined
        /// </param>
        /// <param name="setPriorityShipping">
        /// A flag identifying that the priority shipping is to be determined
        /// </param>
        /// <returns>
        /// The StoreWeekEligibilityList object containing the eligibility settings.
        /// </returns>
        public StoreWeekEligibilityList GetStoreEligibilityForSales(
            eRequestingApplication requestingApplication,
            ProfileList storeList, 
            int nodeRID, 
            HdrColorBin aColor, 
            int yearWeek,
            bool setPriorityShipping
            )
        {
            try
            {
                StoreWeekEligibilityList swel = new StoreWeekEligibilityList(eProfileType.StoreEligibility);
                StoreWeekEligibilityProfile swep = null;
                foreach (StoreProfile sp in storeList.ArrayList)
                {
                    swep = new StoreWeekEligibilityProfile(sp.Key);
                    swep.YearWeek = yearWeek;
                    swep.StoreIsEligible = DetermineSalesEligibility(requestingApplication, aColor.ColorNodeRID, aColor, swep.Key, swep.YearWeek);
                    if (setPriorityShipping)
                    {
                        swep.StoreIsPriorityShipper = DeterminePriorityShipping(nodeRID, swep.Key, swep.YearWeek);
                    }
                    swel.Add(swep);
                }
                return swel;
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

        /// <summary>
        /// Determines the Sales eligibility of a store for a given node and year/week
        /// </summary>
        /// <param name="aColor">
        /// The color
        /// </param>
        /// <param name="storeRID">
        /// The record ID of the store
        /// </param>
        /// <param name="yearWeek">
        /// The year/week for which eligibility is to be determined
        /// </param>
        /// <returns>
        /// A boolean identifying if the store is eligible
        /// </returns>
        /// <remarks>This routine will accept a year/week as either YYYYWW or YYYYDDD</remarks>
        private bool DetermineSalesEligibility(
            eRequestingApplication requestingApplication,
            int nodeRID,
            HdrColorBin aColor, 
            int storeRID, 
            int yearWeek
            )
        {
            try
            {
                if (yearWeek < 1000000)  // YYYYWW format
                {
                    // convert to YYYYDDD
                    yearWeek = this.SAB.ApplicationServerSession.Calendar.GetWeekKey(yearWeek);
                }

                System.Collections.Hashtable colorSalesEligibilityHashByNodeRID;
                // select appropriate cache based on settings
                if (requestingApplication == eRequestingApplication.Forecast)
                {
                    colorSalesEligibilityHashByNodeRID = PlanningColorSalesEligibilityHashByNodeRID;
                }
                else
                {
                    colorSalesEligibilityHashByNodeRID = AllocationColorSalesEligibilityHashByNodeRID;
                }

                if (nodeRID != _colorCurrentSalesEligibilityNodeRID) // if not same node, get year/week Hashtable		
                {
                    _colorSalesEligibilityHashByYearWeek = (System.Collections.Hashtable)colorSalesEligibilityHashByNodeRID[nodeRID];
                    if (_colorSalesEligibilityHashByYearWeek == null)
                    {
                        _colorSalesEligibilityHashByYearWeek = new System.Collections.Hashtable();
                        colorSalesEligibilityHashByNodeRID.Add(nodeRID, _colorSalesEligibilityHashByYearWeek);
                    }
                    _colorCurrentSalesEligibilityNodeRID = nodeRID;
                    _colorCurrentSalesEligibilityYearWeek = -1;  // reset current yearWeek since new node
                }

                if (yearWeek != _colorCurrentSalesEligibilityYearWeek)   // if not same week, get BitArray for year/week
                {
                    _colorSalesEligibilityBitArray = (System.Collections.BitArray)_colorSalesEligibilityHashByYearWeek[yearWeek];
                    if (_colorSalesEligibilityBitArray == null)
                    {
                        _colorSalesEligibilityBitArray = HierarchySessionTransaction.GetExternalStoreSalesEligibilityFlags(
                                requestingApplication,
                                aColor.HdrRID,
                                aColor.HdrID,
                                aColor.ColorCodeRID,
                                aColor.ColorID,
                                aColor.ColorNodeRID,
                                yearWeek
                                );
                        _colorSalesEligibilityHashByYearWeek.Add(yearWeek, _salesEligibilityBitArray);
                    }
                    _colorCurrentSalesEligibilityYearWeek = yearWeek;
                }

                return _colorSalesEligibilityBitArray[storeRID];
            }
            catch (System.NullReferenceException)
            {
                return false;
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

        /// <summary>
        /// Returns the stock eligibility of a single store for a given node and year/week
        /// </summary>
        /// <param name="aPack">
        /// The pack
        /// </param>
        /// <param name="storeRID">
        /// The record ID of the store
        /// </param>
        /// <param name="yearWeek">
        /// The year/week for which eligibility is to be determined
        /// </param>
        /// <returns>
        /// A boolean identifying if the store is eligible
        /// </returns>
        public bool GetStoreEligibilityForStock(
            eRequestingApplication requestingApplication,
            int nodeRID, 
            PackHdr aPack, 
            int storeRID, 
            int yearWeek
            )
        {
            try
            {
                return DetermineStockEligibility(requestingApplication, nodeRID, aPack, storeRID, yearWeek);
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

        /// <summary>
        /// Returns the stock eligibility of a list of store and year/week combinations
        /// </summary>
        /// <param name="aPack">
        /// The pack
        /// </param>
        /// <param name="swel">
        /// A ProfileList containing instances of the StoreWeekEligibilityProfile class
        /// </param>
        /// <returns>
        /// The StoreWeekEligibilityList object containing the eligibility settings.
        /// </returns>
        /// <remarks>
        /// This method will not set the store's priority shipping status.
        /// </remarks>
        public StoreWeekEligibilityList GetStoreEligibilityForStock(
            eRequestingApplication requestingApplication,
            int nodeRID, 
            PackHdr aPack, 
            StoreWeekEligibilityList swel
            )
        {
            try
            {
                return GetStoreEligibilityForStock(requestingApplication, nodeRID, aPack, swel, false);
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

        /// <summary>
        /// Returns the stock eligibility of a list of store and year/week combinations
        /// </summary>
        /// <param name="aPack">
        /// The pack
        /// </param>
        /// <param name="swel">
        /// A ProfileList containing instances of the StoreWeekEligibilityProfile class
        /// </param>
        /// <param name="setPriorityShipping">
        /// A flag identifying that the priority shipping is to be determined
        /// </param>
        /// <returns>
        /// The StoreWeekEligibilityList object containing the eligibility settings.
        /// </returns>
        public StoreWeekEligibilityList GetStoreEligibilityForStock(
            eRequestingApplication requestingApplication,
            int nodeRID, 
            PackHdr aPack, 
            StoreWeekEligibilityList swel, 
            bool setPriorityShipping
            )
        {
            try
            {
                foreach (StoreWeekEligibilityProfile swep in swel)
                {
                    swep.StoreIsEligible = DetermineStockEligibility(requestingApplication, nodeRID, aPack, swep.Key, swep.YearWeek);
                    if (setPriorityShipping)
                    {
                        swep.StoreIsPriorityShipper = DeterminePriorityShipping(nodeRID, swep.Key, swep.YearWeek);
                    }
                }
                return swel;
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

        /// <summary>
        /// Returns the stock eligibility of all stores for a given node and year/week
        /// </summary>
        /// <param name="aPack">
        /// The pack
        /// </param>
        /// <param name="yearWeek">
        /// The year/week for which eligibility is to be determined
        /// </param>
        /// <returns>
        /// The StoreWeekEligibilityList object containing the eligibility settings.
        /// </returns>
        /// <remarks>
        /// This method will not set the store's priority shipping status.
        /// </remarks>
        public StoreWeekEligibilityList GetStoreEligibilityForStock(
            eRequestingApplication requestingApplication,
            int nodeRID, 
            PackHdr aPack, 
            int yearWeek
            )
        {
            try
            {
                return GetStoreEligibilityForStock(requestingApplication, nodeRID, aPack, yearWeek, false);
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

        /// <summary>
        /// Returns the stock eligibility of all stores for a given node and year/week
        /// </summary>
        /// <param name="aPack">
        /// The pack
        /// </param>
        /// <param name="yearWeek">
        /// The year/week for which eligibility is to be determined
        /// </param>
        /// <param name="setPriorityShipping">
        /// A flag identifying that the priority shipping is to be determined
        /// </param>
        /// <returns>
        /// The StoreWeekEligibilityList object containing the eligibility settings.
        /// </returns>
        public StoreWeekEligibilityList GetStoreEligibilityForStock(
            eRequestingApplication requestingApplication,
            int nodeRID, 
            PackHdr aPack, 
            int yearWeek, 
            bool setPriorityShipping
            )
        {
            try
            {
                StoreWeekEligibilityList swel = new StoreWeekEligibilityList(eProfileType.StoreEligibility);
                StoreWeekEligibilityProfile swep = null;
                ProfileList allStoreList = GetProfileList(eProfileType.Store);
                foreach (StoreProfile sp in allStoreList.ArrayList)
                {
                    swep = new StoreWeekEligibilityProfile(sp.Key);
                    swep.YearWeek = yearWeek;
                    swep.StoreIsEligible = DetermineStockEligibility(requestingApplication, nodeRID, aPack, swep.Key, swep.YearWeek);
                    if (setPriorityShipping)
                    {
                        swep.StoreIsPriorityShipper = DeterminePriorityShipping(nodeRID, swep.Key, swep.YearWeek);
                    }
                    swel.Add(swep);
                }
                return swel;
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

        /// <summary>
        /// Returns the stock eligibility a provided list of stores for a given node and year/week
        /// </summary>
        /// <param name="storeList">
        /// A ProfileList containing StoreProfiles for the store to determine eligibility
        /// </param>
        /// <param name="aPack">
        /// The pack
        /// </param>
        /// <param name="yearWeek">
        /// The year/week for which eligibility is to be determined
        /// </param>
        /// <returns>
        /// The StoreWeekEligibilityList object containing the eligibility settings.
        /// </returns>
        /// <remarks>
        /// This method will not set the store's priority shipping status.
        /// </remarks>
        public StoreWeekEligibilityList GetStoreEligibilityForStock(
            eRequestingApplication requestingApplication,
            ProfileList storeList, 
            int nodeRID, 
            PackHdr aPack, 
            int yearWeek
            )
        {
            try
            {
                return GetStoreEligibilityForStock(requestingApplication, storeList, nodeRID, aPack, yearWeek, false);
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

        /// <summary>
        /// Returns the stock eligibility a provided list of stores for a given node and year/week
        /// </summary>
        /// <param name="storeList">
        /// A ProfileList containing StoreProfiles for the store to determine eligibility
        /// </param>
        /// <param name="aPack">
        /// The pack
        /// </param>
        /// <param name="yearWeek">
        /// The year/week for which eligibility is to be determined
        /// </param>
        /// <param name="setPriorityShipping">
        /// A flag identifying that the priority shipping is to be determined
        /// </param>
        /// <returns>
        /// The StoreWeekEligibilityList object containing the eligibility settings.
        /// </returns>
        public StoreWeekEligibilityList GetStoreEligibilityForStock(
            eRequestingApplication requestingApplication,
            ProfileList storeList, 
            int nodeRID, 
            PackHdr aPack, 
            int yearWeek, 
            bool setPriorityShipping
            )
        {
            try
            {
                StoreWeekEligibilityList swel = new StoreWeekEligibilityList(eProfileType.StoreEligibility);
                StoreWeekEligibilityProfile swep = null;
                foreach (StoreProfile sp in storeList.ArrayList)
                {
                    swep = new StoreWeekEligibilityProfile(sp.Key);
                    swep.YearWeek = yearWeek;
                    swep.StoreIsEligible = DetermineStockEligibility(requestingApplication, nodeRID, aPack, swep.Key, swep.YearWeek);
                    if (setPriorityShipping)
                    {
                        swep.StoreIsPriorityShipper = DeterminePriorityShipping(nodeRID, swep.Key, swep.YearWeek);
                    }
                    swel.Add(swep);
                }
                return swel;
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

        /// <summary>
        /// Determines the stock eligibility of a store for a given node and year/week
        /// </summary>
        /// <param name="aPack">
        /// The pack
        /// </param>
        /// <param name="storeRID">
        /// The record ID of the store
        /// </param>
        /// <param name="yearWeek">
        /// The year/week for which eligibility is to be determined
        /// </param>
        /// <returns>
        /// A boolean identifying if the store is eligible
        /// </returns>
        /// <remarks>This routine will accept a year/week as either YYYYWW or YYYYDDD</remarks>
        private bool DetermineStockEligibility(
            eRequestingApplication requestingApplication,
            int nodeRID,
            PackHdr aPack, 
            int storeRID, 
            int yearWeek
            )
        {
            try
            {
                if (yearWeek < 1000000)  // YYYYWW format
                {
                    // convert to YYYYDDD
                    yearWeek = this.SAB.ApplicationServerSession.Calendar.GetWeekKey(yearWeek);
                }

                System.Collections.Hashtable packSalesEligibilityHashByNodeRID;
                // Planning does not have packs.  So, must be Allocation
                packSalesEligibilityHashByNodeRID = AllocationPackSalesEligibilityHashByNodeRID;

                if (aPack.PackRID != _packCurrentStockEligibilityNodeRID) // if not same node, get year/week Hashtable		
                {
                    _packStockEligibilityHashByYearWeek = (System.Collections.Hashtable)packSalesEligibilityHashByNodeRID[aPack.PackRID];
                    if (_packStockEligibilityHashByYearWeek == null)
                    {
                        _packStockEligibilityHashByYearWeek = new System.Collections.Hashtable();
                        packSalesEligibilityHashByNodeRID.Add(aPack.PackRID, _packStockEligibilityHashByYearWeek);
                    }
                    _packCurrentStockEligibilityNodeRID = aPack.PackRID;
                    _packCurrentStockEligibilityYearWeek = -1;  // reset current yearWeek since new node
                }

                if (yearWeek != _packCurrentStockEligibilityYearWeek)   // if not same week, get BitArray for year/week
                {
                    _packStockEligibilityBitArray = (System.Collections.BitArray)_packStockEligibilityHashByYearWeek[yearWeek];
                    if (_packStockEligibilityBitArray == null)
                    {
                        // use style/color for eligibility if pack has colors
                        foreach (PackColorSize aColor in aPack.PackColors.Values)
                        {
                            if (aColor.ColorCodeRID > 0)
                            {
                                nodeRID = GetColorHierarchyNodeRID(styleRID: nodeRID, colorRID: aColor.ColorCodeRID);
                                break;
                            }
                        }
                        _packStockEligibilityBitArray = HierarchySessionTransaction.GetExternalStoreStockEligibilityFlags(
                            requestingApplication,
                            aPack.HdrRID,
                            aPack.HdrID,
                            nodeRID,
                            aPack.PackRID, 
                            aPack.PackName,
                            yearWeek
                            );
                        _packStockEligibilityHashByYearWeek.Add(yearWeek, _packStockEligibilityBitArray);
                    }
                    _packCurrentStockEligibilityYearWeek = yearWeek;
                }

                return _packStockEligibilityBitArray[storeRID];
            }
            catch (System.NullReferenceException)
            {
                return false;
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

        /// <summary>
        /// Returns the Sales eligibility of a single store for a given node and year/week
        /// </summary>
        /// <param name="aPack">
        /// The pack
        /// </param>
        /// <param name="storeRID">
        /// The record ID of the store
        /// </param>
        /// <param name="yearWeek">
        /// The year/week for which eligibility is to be determined
        /// </param>
        /// <returns>
        /// A boolean identifying if the store is eligible
        /// </returns>
        public bool GetStoreEligibilityForSales(
            eRequestingApplication requestingApplication,
            int nodeRID, 
            PackHdr aPack, 
            int storeRID, 
            int yearWeek
            )
        {
            try
            {
                return DetermineSalesEligibility(
                    requestingApplication,
                    nodeRID,
                    aPack, 
                    storeRID, 
                    yearWeek
                    );
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

        /// <summary>
        /// Returns the Sales eligibility of a list of store and year/week combinations
        /// </summary>
        /// <param name="aPack">
        /// The pack
        /// </param>
        /// <param name="swel">
        /// A ProfileList containing instances of the StoreWeekEligibilityProfile class
        /// </param>
        /// <returns>
        /// The StoreWeekEligibilityList object containing the eligibility settings.
        /// </returns>
        /// <remarks>
        /// This method will not set the store's priority shipping status.
        /// </remarks>
        public StoreWeekEligibilityList GetStoreEligibilityForSales(
            eRequestingApplication requestingApplication,
            int nodeRID, 
            PackHdr aPack, 
            StoreWeekEligibilityList swel
            )
        {
            try
            {
                return GetStoreEligibilityForSales(requestingApplication, nodeRID, aPack, swel, false);
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

        /// <summary>
        /// Returns the Sales eligibility of a list of store and year/week combinations
        /// </summary>
        /// <param name="aPack">
        /// The pack
        /// </param>
        /// <param name="swel">
        /// A ProfileList containing instances of the StoreWeekEligibilityProfile class
        /// </param>
        /// <param name="setPriorityShipping">
        /// A flag identifying that the priority shipping is to be determined
        /// </param>
        /// <returns>
        /// The StoreWeekEligibilityList object containing the eligibility settings.
        /// </returns>
        public StoreWeekEligibilityList GetStoreEligibilityForSales(
            eRequestingApplication requestingApplication,
            int nodeRID, 
            PackHdr aPack, 
            StoreWeekEligibilityList swel, 
            bool setPriorityShipping
            )
        {
            try
            {
                foreach (StoreWeekEligibilityProfile swep in swel)
                {
                    swep.StoreIsEligible = DetermineSalesEligibility(
                        requestingApplication,
                        nodeRID,
                        aPack, 
                        swep.Key, 
                        swep.YearWeek
                        );
                    if (setPriorityShipping)
                    {
                        swep.StoreIsPriorityShipper = DeterminePriorityShipping(nodeRID, swep.Key, swep.YearWeek);
                    }
                }
                return swel;
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

        /// <summary>
        /// Returns the Sales eligibility of all stores for a given node and year/week
        /// </summary>
        /// <param name="aPack">
        /// The pack
        /// </param>
        /// <param name="yearWeek">
        /// The year/week for which eligibility is to be determined
        /// </param>
        /// <returns>
        /// The StoreWeekEligibilityList object containing the eligibility settings.
        /// </returns>
        /// <remarks>
        /// This method will not set the store's priority shipping status.
        /// </remarks>
        public StoreWeekEligibilityList GetStoreEligibilityForSales(
            eRequestingApplication requestingApplication,
            int nodeRID, 
            PackHdr aPack, 
            int yearWeek
            )
        {
            try
            {
                return GetStoreEligibilityForSales(requestingApplication, nodeRID, aPack, yearWeek, false);
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

        /// <summary>
        /// Returns the Sales eligibility of all stores for a given node and year/week
        /// </summary>
        /// <param name="aPack">
        /// The pack
        /// </param>
        /// <param name="yearWeek">
        /// The year/week for which eligibility is to be determined
        /// </param>
        /// <param name="setPriorityShipping">
        /// A flag identifying that the priority shipping is to be determined
        /// </param>
        /// <returns>
        /// The StoreWeekEligibilityList object containing the eligibility settings.
        /// </returns>
        public StoreWeekEligibilityList GetStoreEligibilityForSales(
            eRequestingApplication requestingApplication,
            int nodeRID, 
            PackHdr aPack, 
            int yearWeek, 
            bool setPriorityShipping
            )
        {
            try
            {
                StoreWeekEligibilityList swel = new StoreWeekEligibilityList(eProfileType.StoreEligibility);
                StoreWeekEligibilityProfile swep = null;
                ProfileList allStoreList = GetProfileList(eProfileType.Store);
                foreach (StoreProfile sp in allStoreList.ArrayList)
                {
                    swep = new StoreWeekEligibilityProfile(sp.Key);
                    swep.YearWeek = yearWeek;
                    swep.StoreIsEligible = DetermineSalesEligibility(
                        requestingApplication,
                        nodeRID,
                        aPack, 
                        swep.Key, 
                        swep.YearWeek
                        );
                    if (setPriorityShipping)
                    {
                        swep.StoreIsPriorityShipper = DeterminePriorityShipping(nodeRID, swep.Key, swep.YearWeek);
                    }
                    swel.Add(swep);
                }
                return swel;
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

        /// <summary>
        /// Returns the Sales eligibility a provided list of stores for a given node and year/week
        /// </summary>
        /// <param name="storeList">
        /// A ProfileList containing StoreProfiles for the store to determine eligibility
        /// </param>
        /// <param name="aPack">
        /// The pack
        /// </param>
        /// <param name="yearWeek">
        /// The year/week for which eligibility is to be determined
        /// </param>
        /// <returns>
        /// The StoreWeekEligibilityList object containing the eligibility settings.
        /// </returns>
        /// <remarks>
        /// This method will not set the store's priority shipping status.
        /// </remarks>
        public StoreWeekEligibilityList GetStoreEligibilityForSales(
            eRequestingApplication requestingApplication,
            ProfileList storeList, 
            int nodeRID, 
            PackHdr aPack, 
            int yearWeek
            )
        {
            try
            {
                return GetStoreEligibilityForSales(requestingApplication, storeList, nodeRID, aPack, yearWeek, false);
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

        /// <summary>
        /// Returns the Sales eligibility a provided list of stores for a given node and year/week
        /// </summary>
        /// <param name="storeList">
        /// A ProfileList containing StoreProfiles for the store to determine eligibility
        /// </param>
        /// <param name="aPack">
        /// The pack
        /// </param>
        /// <param name="yearWeek">
        /// The year/week for which eligibility is to be determined
        /// </param>
        /// <param name="setPriorityShipping">
        /// A flag identifying that the priority shipping is to be determined
        /// </param>
        /// <returns>
        /// The StoreWeekEligibilityList object containing the eligibility settings.
        /// </returns>
        public StoreWeekEligibilityList GetStoreEligibilityForSales(
            eRequestingApplication requestingApplication,
            ProfileList storeList, 
            int nodeRID, 
            PackHdr aPack, 
            int yearWeek, 
            bool setPriorityShipping
            )
        {
            try
            {
                StoreWeekEligibilityList swel = new StoreWeekEligibilityList(eProfileType.StoreEligibility);
                StoreWeekEligibilityProfile swep = null;
                foreach (StoreProfile sp in storeList.ArrayList)
                {
                    swep = new StoreWeekEligibilityProfile(sp.Key);
                    swep.YearWeek = yearWeek;
                    swep.StoreIsEligible = DetermineSalesEligibility(
                        requestingApplication,
                        nodeRID,
                        aPack, 
                        swep.Key, 
                        swep.YearWeek
                        );
                    if (setPriorityShipping)
                    {
                        swep.StoreIsPriorityShipper = DeterminePriorityShipping(nodeRID, swep.Key, swep.YearWeek);
                    }
                    swel.Add(swep);
                }
                return swel;
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

        /// <summary>
        /// Determines the Sales eligibility of a store for a given node and year/week
        /// </summary>
        /// <param name="aPack">
        /// The pack
        /// </param>
        /// <param name="storeRID">
        /// The record ID of the store
        /// </param>
        /// <param name="yearWeek">
        /// The year/week for which eligibility is to be determined
        /// </param>
        /// <returns>
        /// A boolean identifying if the store is eligible
        /// </returns>
        /// <remarks>This routine will accept a year/week as either YYYYWW or YYYYDDD</remarks>
        private bool DetermineSalesEligibility(
            eRequestingApplication requestingApplication,
            int nodeRID,
            PackHdr aPack, 
            int storeRID, 
            int yearWeek
            )
        {
            try
            {
                if (yearWeek < 1000000)  // YYYYWW format
                {
                    // convert to YYYYDDD
                    yearWeek = this.SAB.ApplicationServerSession.Calendar.GetWeekKey(yearWeek);
                }

                System.Collections.Hashtable packStockEligibilityHashByNodeRID;
                // Planning does not have packs.  So, must be Allocation
                packStockEligibilityHashByNodeRID = AllocationPackStockEligibilityHashByNodeRID;

                if (aPack.PackRID != _packCurrentSalesEligibilityNodeRID) // if not same node, get year/week Hashtable		
                {
                    _packSalesEligibilityHashByYearWeek = (System.Collections.Hashtable)packStockEligibilityHashByNodeRID[aPack.PackRID];
                    if (_packSalesEligibilityHashByYearWeek == null)
                    {
                        _packSalesEligibilityHashByYearWeek = new System.Collections.Hashtable();
                        packStockEligibilityHashByNodeRID.Add(aPack.PackRID, _salesEligibilityHashByYearWeek);
                    }
                    _packCurrentSalesEligibilityNodeRID = aPack.PackRID;
                    _packCurrentSalesEligibilityYearWeek = -1;  // reset current yearWeek since new node
                }

                if (yearWeek != _packCurrentSalesEligibilityYearWeek)   // if not same week, get BitArray for year/week
                {
                    _packSalesEligibilityBitArray = (System.Collections.BitArray)_salesEligibilityHashByYearWeek[yearWeek];
                    if (_packSalesEligibilityBitArray == null)
                    {
                        // use style/color for eligibility if pack has colors
                        foreach (PackColorSize aColor in aPack.PackColors.Values)
                        {
                            if (aColor.ColorCodeRID > 0)
                            {
                                nodeRID = GetColorHierarchyNodeRID(styleRID: nodeRID, colorRID: aColor.ColorCodeRID);
                                break;
                            }
                        }
                        _packSalesEligibilityBitArray = HierarchySessionTransaction.GetExternalStoreSalesEligibilityFlags(
                            requestingApplication,
                            aPack.HdrRID,
                            aPack.HdrID,
                            nodeRID,
                            aPack.PackRID,
                            aPack.PackName,
                            yearWeek
                            );
                        _packSalesEligibilityHashByYearWeek.Add(yearWeek, _salesEligibilityBitArray);
                    }
                    _packCurrentSalesEligibilityYearWeek = yearWeek;
                }

                return _packSalesEligibilityBitArray[storeRID];
            }
            catch (System.NullReferenceException)
            {
                return false;
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

        private int GetColorHierarchyNodeRID(int styleRID, int colorRID)
        {
            int colorHnRID = Include.NoRID;
            try
            {

                ColorCodeProfile ccp = SAB.HierarchyServerSession.GetColorCodeProfile(colorRID);
                HierarchyNodeProfile hnp_style = SAB.HierarchyServerSession.GetNodeData(styleRID);
                if (!SAB.HierarchyServerSession.ColorExistsForStyle(hnp_style.HomeHierarchyRID, hnp_style.Key, ccp.ColorCodeID, ref colorHnRID))
                { 
                    // if color node does not exist, use the style
                    colorHnRID = styleRID;
                }                             
            }
            catch (Exception)
            {
                throw;
            }

            return colorHnRID;
        }

    }
}
