using System;  //TT#1636-MD -jsobek -Pre-Pack Fill Size
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using MIDRetail.Data;
using MIDRetail.Common;
using MIDRetail.DataCommon;

namespace MIDRetail.Business.Allocation
{

    public class DetailPackOptions
    {
        public double maxPackNeedTolerance;
        public double avgPackDeviationTolerance;
        public bool overrideAvgPackDevTolerance;
        public bool overrideMaxPackNeedTolerance;
        public bool packToleranceNoMaxStep;
        public bool packToleranceStepped;

        public ProfileList _storeFilteredList;
        public int _storeFilterCount;
        public int _packCount;
        public int _sizeCount;
        // 2-dim arrays by pack and store used to allocate packs by need
        public int[,] _packSizeContent;
        public double[,] _packSizeCurvePct;

        // 1-dim array by pack used to allocate packs by need
        public int[] _packsToAllocate;
        public int[] _packMultiple;
        public AllocationProfile[] _packApHome;		// TT#1386-MD - stodd - Manual Merge 

        // 1-dim arrays by store used to allocate packs by need
        public Index_RID[] _storeIdxRID;
        public int[] _storeUnitsRemainingToBreakout;
        public int[] _storeTotalNeedError;
        public int[] _storeTotalPosUnitNeed;
        public int[] _storeCandidateCount;
        public int[] _storePrimaryPackIDX;
        public double[] _storeTotalPosPctNeed;
        public double[] _storeAvgPosUnitNeed;
        public double[] _storeAvgPosPctNeed;
        public double[] _storePackDeviationErrorDiff;
        public double[] _storePackNeedErrorDiff;
        public double[] _storePackGapDiff;
        public double[] _storeDesiredTotalUnits;


        // 2-dim arrays by store and pack used to allocate packs by need 
        public int[,] _storePacksAllocated;
        public bool[,] _storePackAvailable;
        public bool[,] _storePackCandidate;
        public double[,] _storePackDeviationError;
        public int[,] _storePackDeviationErrorCount;
        public double[,] _storePackAvgDeviationError;
        public double[,] _storePackNeedError;
        public double[,] _storePackGap;
        public double[,] _storeSizePlan;           // TT#2920 - Jellis - Detail Pack Algorithm Change
        public double[,] _storePackPctNeedMeasure; // TT#2920 -  Jellis - Detail Pack Algorithm Change

        // 2-dim arrays by store and size used to allocate packs by need
        public double[,] _storeSizePosNeed;
        public double[,] _storeSizePosPctNeed;
        public int[,] _storeSizeNeedError;
        public double[,] _storeDesiredAllocationCurve;
        public double[,] _storeDesiredSizeUnits;

        public bool isFillSizeHoles = false;
		// Begin TT#1386-MD - stodd - Manual Merge 
		public Dictionary<int, StoreVector> _headerStoreUnitsAllocated;  // TT#1568 - MD - Jellis - GA Size need not observing Min/Max on member headers
        public Dictionary<int, StoreVector> _inventoryBasisAllocation;
		// End TT#1386-MD - stodd - Manual Merge 
    }


    public static class PackProcessing
    {
        public static void AllocateNonGenericPacks(SizeNeedResults aSizeNeedResults, DetailPackOptions pOpt, AllocationProfile _allocationProfile, ApplicationSessionTransaction _transaction, out int totalPackUnitsAllocated)
        {
            //_storeFilteredList = aStoreFilteredList;
            pOpt._storeFilterCount = pOpt._storeFilteredList.Count;
            pOpt._packCount = _allocationProfile.NonGenericPackCount;
            pOpt._sizeCount = aSizeNeedResults.Sizes.Count;    //  TT#1068 - MD - Jellis - Pack allocations not considered in decisions for bulk size allocations
            //_sizeCount = _sizeNeedResults.Sizes.Count;  //  TT#1068 - MD - Jellis - Pack allocations not considered in decisions for bulk size allocations
            // end TT#702 Infinite Loop when begin date set

            //======================//
            //  Build Pack Curves   //
            //======================//
            pOpt._packSizeContent = new int[pOpt._packCount, pOpt._sizeCount];
            pOpt._packSizeContent.Initialize();
            pOpt._packSizeCurvePct = new double[pOpt._packCount, pOpt._sizeCount];
            pOpt._packSizeCurvePct.Initialize();
            pOpt._packsToAllocate = new int[pOpt._packCount];
            pOpt._packMultiple = new int[pOpt._packCount];
            pOpt._packApHome = new AllocationProfile[pOpt._packCount];	// TT#1386-MD - stodd - Manual Merge 

            int sizeIDX;
            int packIDX = 0;
            int sizeRID;
            foreach (PackHdr ph in _allocationProfile.NonGenericPacks.Values) // TT#702 Infinite Loop when begin date set
            {
                int totalSizeUnits = 0;
                pOpt._packMultiple[packIDX] = ph.PackMultiple;
                pOpt._packsToAllocate[packIDX] =
                    ph.PacksToAllocate - ph.PacksAllocated;
				// Begin TT#1386-MD - stodd - Manual Merge 
                // begin TT#1568 - MD - Jellis - GA Size need not observing Header min/max
                // Begin TT#4988 - BVaughan - Performance
                #if DEBUG
                if ((_allocationProfile is AssortmentProfile && !_allocationProfile.isAssortmentProfile) || (!(_allocationProfile is AssortmentProfile) && _allocationProfile.isAssortmentProfile))
                {
                    throw new Exception("Object does not match AssortmentProfile in AllocateNonGenericPacks()");
                }
                #endif
                //if (_allocationProfile is AssortmentProfile)
                if (_allocationProfile.isAssortmentProfile)
                // End TT#4988 - BVaughan - Performance
                {
                    pOpt._packApHome[packIDX] = ((AssortmentProfile)_allocationProfile).GetAssortmentPackHome(ph.PackRID);
                }
                else
                {
                    pOpt._packApHome[packIDX] = _allocationProfile;
                }
                // end TT#1568 - MD - Jellis - GA Size need not observing Header min/max
				// End TT#1386-MD - stodd - Manual Merge 
                if (pOpt._packsToAllocate[packIDX] < 0)
                {
                    pOpt._packsToAllocate[packIDX] = 0;
                }
                foreach (PackColorSize pcs in ph.PackColors.Values)
                {
                    for (sizeIDX = 0; sizeIDX < pOpt._sizeCount; sizeIDX++)
                    {
                        //sizeRID = (int)_sizeNeedResults.Sizes[sizeIDX]; // TT#702 Infinite Loop when begin date set //  TT#1068 - MD - Jellis - Pack allocations not considered in decisions for bulk size allocations
                        sizeRID = (int)aSizeNeedResults.Sizes[sizeIDX]; //  TT#1068 - MD - Jellis - Pack allocations not considered in decisions for bulk size allocations
                        if (pcs.SizeIsInColor(sizeRID))
                        {
                            pOpt._packSizeContent[packIDX, sizeIDX] += pcs.GetSizeBin(sizeRID).ContentUnits;
                            totalSizeUnits += pcs.GetSizeBin(sizeRID).ContentUnits;
                        }
                    }
                }
                // Note:  we are not doing the typical spread to 100 because
                // we just require a reasonable estimate to use as a comparison
                if (totalSizeUnits > 0)
                {
                    for (sizeIDX = 0; sizeIDX < pOpt._sizeCount; sizeIDX++)
                    {
                        pOpt._packSizeCurvePct[packIDX, sizeIDX] = pOpt._packSizeContent[packIDX, sizeIDX] * 100 / totalSizeUnits;
                    }
                }
                packIDX++;
            }

            pOpt._storeIdxRID = new Index_RID[pOpt._storeFilterCount];
            pOpt._storePrimaryPackIDX = new int[pOpt._storeFilterCount];
            pOpt._storeCandidateCount = new int[pOpt._storeFilterCount];
            pOpt._storeUnitsRemainingToBreakout = new int[pOpt._storeFilterCount];
            pOpt._storeUnitsRemainingToBreakout.Initialize();
            pOpt._storeTotalNeedError = new int[pOpt._storeFilterCount];
            pOpt._storeTotalNeedError.Initialize();
            pOpt._storeTotalPosUnitNeed = new int[pOpt._storeFilterCount];
            pOpt._storeTotalPosUnitNeed.Initialize();
            pOpt._storeTotalPosPctNeed = new double[pOpt._storeFilterCount];
            pOpt._storeTotalPosPctNeed.Initialize();
            pOpt._storeAvgPosUnitNeed = new double[pOpt._storeFilterCount];
            pOpt._storeAvgPosUnitNeed.Initialize();
            pOpt._storeAvgPosPctNeed = new double[pOpt._storeFilterCount];
            pOpt._storeAvgPosPctNeed.Initialize();
            pOpt._storePackDeviationErrorDiff = new double[pOpt._storeFilterCount];
            pOpt._storePackDeviationErrorDiff.Initialize();
            pOpt._storePackNeedErrorDiff = new double[pOpt._storeFilterCount];
            pOpt._storePackNeedErrorDiff.Initialize();
            pOpt._storePackGapDiff = new double[pOpt._storeFilterCount];
            pOpt._storePackGapDiff.Initialize();
            pOpt._storeDesiredTotalUnits = new double[pOpt._storeFilterCount];
            pOpt._storeDesiredTotalUnits.Initialize();

            pOpt._storePacksAllocated = new int[pOpt._storeFilterCount, pOpt._packCount];
            pOpt._storePacksAllocated.Initialize();
            pOpt._storePackAvailable = new bool[pOpt._storeFilterCount, pOpt._packCount];
            pOpt._storePackAvailable.Initialize();
            pOpt._storePackCandidate = new bool[pOpt._storeFilterCount, pOpt._packCount];
            pOpt._storePackCandidate.Initialize();
            pOpt._storePackDeviationError = new double[pOpt._storeFilterCount, pOpt._packCount];
            pOpt._storePackDeviationError.Initialize();
            pOpt._storePackDeviationErrorCount = new int[pOpt._storeFilterCount, pOpt._packCount];
            pOpt._storePackDeviationErrorCount.Initialize();
            pOpt._storePackAvgDeviationError = new double[pOpt._storeFilterCount, pOpt._packCount];
            pOpt._storePackAvgDeviationError.Initialize();
            pOpt._storePackNeedError = new double[pOpt._storeFilterCount, pOpt._packCount];
            pOpt._storePackNeedError.Initialize();
            pOpt._storePackGap = new double[pOpt._storeFilterCount, pOpt._packCount];
            pOpt._storePackGap.Initialize();
            pOpt._storeSizePlan = new double[pOpt._storeFilterCount, pOpt._sizeCount];  // TT#2920 -  Jellis - Detail Pack Algorithm Change
            pOpt._storePackPctNeedMeasure = new double[pOpt._storeFilterCount, pOpt._packCount]; // TT#2920 -  Jellis - Detail Pack Algorithm Change

            pOpt._storeSizePosNeed = new double[pOpt._storeFilterCount, pOpt._sizeCount];
            pOpt._storeSizePosPctNeed = new double[pOpt._storeFilterCount, pOpt._sizeCount];
            pOpt._storeSizeNeedError = new int[pOpt._storeFilterCount, pOpt._sizeCount];
            pOpt._storeSizeNeedError.Initialize();

            pOpt._storeDesiredSizeUnits = new double[pOpt._storeFilterCount, pOpt._sizeCount];
            pOpt._storeDesiredSizeUnits.Initialize();
            pOpt._storeDesiredAllocationCurve = new double[pOpt._storeFilterCount, pOpt._sizeCount];
            pOpt._storeDesiredAllocationCurve.Initialize();
						
            int storeFilterIDX;
            StoreProfile storeProfile;
			
			// Begin TT#1386-MD - stodd - Manual Merge 
			// begin TT#1568 - MD - Jellis - GA Size Need not observing member header min/max
            AssortmentProfile assrtProfile = _allocationProfile.AssortmentProfile;
            AllocationProfile[] apList = null;
            StoreVector sv;

            if (assrtProfile != null)
            {
                apList = assrtProfile.AssortmentMembers;
                pOpt._headerStoreUnitsAllocated = new Dictionary<int, StoreVector>();
                Dictionary<long, StoreVector> inventoryBasisAllocation = assrtProfile.InventoryBasisAllocation;  // Get copy of current inventory basis allocation
                int[] inventoryBasis;
                pOpt._inventoryBasisAllocation = new Dictionary<int,StoreVector>();
                foreach (KeyValuePair<long, StoreVector> keyValue in inventoryBasisAllocation)
                {
                    // NOTE:  First int of the key is size; Second in of the key is the inventory basis
                    //        If the size portion is 0, then the key is for the NON-Size Inventory basis
                    inventoryBasis = MIDMath.UnPackLong(keyValue.Key);
                    if (inventoryBasis[0] == Include.IntransitKeyTypeNoSize)  
                    {
                        // Begin TT#1851-MD - JSmith - Allocate 2 headers same style/color using Inv Max.  After Size need the stores are allocated over the Inv Max.
                        //pOpt._inventoryBasisAllocation.Add(inventoryBasis[1], keyValue.Value);
                        // clear the inventory vector.  Inventory basis values are accounted for when retrieveing the maximums
                        StoreVector invSV = new StoreVector();
                        for (storeFilterIDX = 0; storeFilterIDX < pOpt._storeFilterCount; storeFilterIDX++)
                        {
                            storeProfile = (StoreProfile)pOpt._storeFilteredList[storeFilterIDX];
                            invSV.SetStoreValue(
                                storeProfile.Key,
                                 0
                           );
                        }
                        pOpt._inventoryBasisAllocation.Add(inventoryBasis[1], invSV);
                        // End TT#1851-MD - JSmith - Allocate 2 headers same style/color using Inv Max.  After Size need the stores are allocated over the Inv Max.
                    }
                }
                foreach (AllocationProfile ap in apList)
                {
                    sv = new StoreVector();
                    pOpt._headerStoreUnitsAllocated.Add(ap.Key, sv);

                    //foreach (Index_RID storeIdxRID in _storeIdxRID)
                    for (storeFilterIDX = 0; storeFilterIDX < pOpt._storeFilterCount; storeFilterIDX++)
                    {
                        storeProfile = (StoreProfile)pOpt._storeFilteredList[storeFilterIDX];
                        pOpt._storeIdxRID[storeFilterIDX] = _transaction.StoreIndexRID(storeProfile.Key); // TT#702 Infinite Loop when begin date set

                        //sv.SetStoreValue(
                        //    storeFilterIDX,
                        //    ap.GetStoreQtyAllocated(eAllocationSummaryNode.Total, storeIdxRID)
                        //    - ap.GetStoreQtyAllocated(eAllocationSummaryNode.DetailType, storeIdxRID)
                        //    + ap.GetStoreQtyAllocated(eAllocationSummaryNode.DetailSubType, storeIdxRID));

                        sv.SetStoreValue(
                            pOpt._storeIdxRID[storeFilterIDX].RID,
                            ap.GetStoreQtyAllocated(eAllocationSummaryNode.Total, pOpt._storeIdxRID[storeFilterIDX].RID)
                            - ap.GetStoreQtyAllocated(eAllocationSummaryNode.DetailType, pOpt._storeIdxRID[storeFilterIDX].RID)
                            + ap.GetStoreQtyAllocated(eAllocationSummaryNode.DetailSubType, pOpt._storeIdxRID[storeFilterIDX].RID));
                    }
                }
            }
			// End TT#1568 - MD - Jellis - GA Size Need not observing member header min/max
			// End TT#1386-MD - stodd - Manual Merge 
			
            //set units remaining for Fill Size Holes based on the size need results
            if (pOpt.isFillSizeHoles)
            {
                for (storeFilterIDX = 0; storeFilterIDX < pOpt._storeFilterCount; storeFilterIDX++)
                {
                    storeProfile = (StoreProfile)pOpt._storeFilteredList[storeFilterIDX];
                    pOpt._storeIdxRID[storeFilterIDX] = _transaction.StoreIndexRID(storeProfile.Key); // TT#702 Infinite Loop when begin date set
                   
                    int totalForAllSizes = 0;
                    for (sizeIDX = 0; sizeIDX < pOpt._sizeCount; sizeIDX++)
                    {
                        sizeRID = (int)aSizeNeedResults.Sizes[sizeIDX]; 
                        totalForAllSizes += aSizeNeedResults.GetAllocatedUnits(storeProfile.Key, sizeRID);
                    }
                    pOpt._storeUnitsRemainingToBreakout[storeFilterIDX] = totalForAllSizes;

                        
                    //    _allocationProfile.GetStoreQtyAllocated(eAllocationSummaryNode.DetailType, pOpt._storeIdxRID[storeFilterIDX]) // TT#702 Infinite Loop when begin date set
                    //    - _allocationProfile.GetStoreQtyAllocated(eAllocationSummaryNode.DetailSubType, pOpt._storeIdxRID[storeFilterIDX]); // TT#702 Infinite Loop when begin date set
                    //if (pOpt._storeUnitsRemainingToBreakout[storeFilterIDX] < 0)
                    //{
                    //    pOpt._storeUnitsRemainingToBreakout[storeFilterIDX] = 0;
                    //}
                    //BuildStorePackCriteria(storeFilterIDX);  //  TT#1068 - MD - Jellis - Pack allocations not considered in decisions for bulk size allocations
                    BuildStorePackCriteria(aSizeNeedResults, storeFilterIDX, pOpt, _allocationProfile); //  TT#1068 - MD - Jellis - Pack allocations not considered in decisions for bulk size allocations
                }
            }
            else
            {
                for (storeFilterIDX = 0; storeFilterIDX < pOpt._storeFilterCount; storeFilterIDX++)
                {
                    storeProfile = (StoreProfile)pOpt._storeFilteredList[storeFilterIDX];
                    pOpt._storeIdxRID[storeFilterIDX] = _transaction.StoreIndexRID(storeProfile.Key); // TT#702 Infinite Loop when begin date set
                    pOpt._storeUnitsRemainingToBreakout[storeFilterIDX] =
                        _allocationProfile.GetStoreQtyAllocated(eAllocationSummaryNode.DetailType, pOpt._storeIdxRID[storeFilterIDX]) // TT#702 Infinite Loop when begin date set
                        - _allocationProfile.GetStoreQtyAllocated(eAllocationSummaryNode.DetailSubType, pOpt._storeIdxRID[storeFilterIDX]); // TT#702 Infinite Loop when begin date set
                    if (pOpt._storeUnitsRemainingToBreakout[storeFilterIDX] < 0)
                    {
                        pOpt._storeUnitsRemainingToBreakout[storeFilterIDX] = 0;
                    }
                    //BuildStorePackCriteria(storeFilterIDX);  //  TT#1068 - MD - Jellis - Pack allocations not considered in decisions for bulk size allocations
                    BuildStorePackCriteria(aSizeNeedResults, storeFilterIDX, pOpt, _allocationProfile); //  TT#1068 - MD - Jellis - Pack allocations not considered in decisions for bulk size allocations
                }
            }



            // begin TT#1365 - JEllis - FL Detail Pack Size Need Enhancement
            if (pOpt.packToleranceStepped
                && pOpt.maxPackNeedTolerance < Include.MaxPackNeedTolerance + 1) // TT#1410 (related to TT#1412 - FL Pack Allocation Exceeds tolerance)
            {
                int maxTolerance = (int)pOpt.maxPackNeedTolerance + 1;
                int maxPackNeedTolerance = 0;
                while (maxPackNeedTolerance < maxTolerance)
                {
                    FitPacksToStores(aSizeNeedResults, pOpt, _allocationProfile, _transaction); //  TT#1068 - MD - Jellis - Pack allocations not considered in decisions for bulk size allocations
                    maxPackNeedTolerance++;
                }
            }
            else
            {
                if (pOpt.maxPackNeedTolerance < int.MaxValue)
                {
                    FitPacksToStores(aSizeNeedResults, pOpt, _allocationProfile, _transaction); //  TT#1068 - MD - Jellis - Pack allocations not considered in decisions for bulk size allocations
                }                                          // begin TT#1365 part 2
                else
                {
                    pOpt.maxPackNeedTolerance = int.MaxValue;
                    FitPacksToStores(aSizeNeedResults, pOpt, _allocationProfile, _transaction); //  TT#1068 - MD - Jellis - Pack allocations not considered in decisions for bulk size allocations    
                }                                         // end TT#1365 part 2
            }
            if (pOpt.packToleranceNoMaxStep && pOpt.maxPackNeedTolerance < int.MaxValue) // TT#1365 part 2
            {
                pOpt.maxPackNeedTolerance = int.MaxValue;
                FitPacksToStores(aSizeNeedResults, pOpt, _allocationProfile, _transaction); // TT#1365 part 2 //  TT#1068 - MD - Jellis - Pack allocations not considered in decisions for bulk size allocations
            }




            eDistributeChange distributeOption = eDistributeChange.ToNone;
            if (pOpt.isFillSizeHoles)
            {
                distributeOption = eDistributeChange.ToParent;
            }

            totalPackUnitsAllocated = 0;
            for (storeFilterIDX = 0; storeFilterIDX < pOpt._storeFilterCount; storeFilterIDX++)
            {
                packIDX = 0;
                foreach (PackHdr ph in _allocationProfile.NonGenericPacks.Values)
                {
                    if (pOpt._storePacksAllocated[storeFilterIDX, packIDX] !=
                        _allocationProfile.GetStoreQtyAllocated(ph, pOpt._storeIdxRID[storeFilterIDX]))
                    {
                        _allocationProfile.SetStoreQtyAllocated
                            (ph,
                            pOpt._storeIdxRID[storeFilterIDX],
                            pOpt._storePacksAllocated[storeFilterIDX, packIDX],
                            distributeOption,
                            false);
                        _allocationProfile.SetStoreAllocationFromPackContentBreakOut(eAllocationSummaryNode.Total, pOpt._storeIdxRID[storeFilterIDX], true);
                        _allocationProfile.SetStoreAllocationFromPackContentBreakOut(eAllocationSummaryNode.DetailType, pOpt._storeIdxRID[storeFilterIDX], true);
                        _allocationProfile.SetStoreAllocationFromPackContentBreakOut(ph, pOpt._storeIdxRID[storeFilterIDX], true);
                    }
                    totalPackUnitsAllocated += pOpt._storePacksAllocated[storeFilterIDX, packIDX] * ph.PackMultiple;
                    packIDX++;
                
                }
            }
        }


        private static void BuildStorePackCriteria(SizeNeedResults aSizeNeedResults, int aStoreFilterIDX, DetailPackOptions pOpt, AllocationProfile _allocationProfile) //  TT#1068 - MD - Jellis - Pack allocations not considered in decisions for bulk size allocations
        {
            StoreProfile storeProfile = (StoreProfile)pOpt._storeFilteredList.ArrayList[aStoreFilterIDX];
            int sizeRID;
            int sizeIDX;
            int packIDX;
            for (sizeIDX = 0; sizeIDX < pOpt._sizeCount; sizeIDX++)
            {
                sizeRID = (int)aSizeNeedResults.Sizes[sizeIDX];  //  TT#1068 - MD - Jellis - Pack allocations not considered in decisions for bulk size allocations
                pOpt._storeDesiredSizeUnits[aStoreFilterIDX, sizeIDX] =
                    aSizeNeedResults.GetAllocatedUnits(storeProfile.Key, sizeRID);  //  TT#1068 - MD - Jellis - Pack allocations not considered in decisions for bulk size allocations
                pOpt._storeDesiredTotalUnits[aStoreFilterIDX] +=
                    (int)pOpt._storeDesiredSizeUnits[aStoreFilterIDX, sizeIDX];
                pOpt._storeSizePosNeed[aStoreFilterIDX, sizeIDX] =
                    aSizeNeedResults.GetSizeNeedUnits(storeProfile.Key, sizeRID, true); // TT#2332 - Jellis - Pack Fitting Algorithm Broken  //  TT#1068 - MD - Jellis - Pack allocations not considered in decisions for bulk size allocations
                pOpt._storeSizePlan[aStoreFilterIDX, sizeIDX] = // TT#2920 -  Jellis - Detail Pack Algorithm Change
                    aSizeNeedResults.GetSizeNeed_PlanUnits(storeProfile.Key, sizeRID); // TT#2920 -  Jellis - Detail Pack Algorithm Change  //  TT#1068 - MD - Jellis - Pack allocations not considered in decisions for bulk size allocations
            }
            packIDX = 0;
            foreach (PackHdr ph in _allocationProfile.NonGenericPacks.Values)
            {
                pOpt._storePacksAllocated[aStoreFilterIDX, packIDX] =
                    _allocationProfile.GetStoreQtyAllocated(ph, pOpt._storeIdxRID[aStoreFilterIDX]);

                pOpt._storePackCandidate[aStoreFilterIDX, packIDX] = false;

                // Begin TT#1767-MD - stodd - Fill Size Method not processing units for Packs when it's a small quantity
                //==============================================================================================
                // Added the storeUnitsRemainingToBreakout bool to help manage the 
                // large "if" statement after the bool is checked.
                // Different question needs to be asked depending upon whether it's Fill Size Holes or not.
                //==============================================================================================
                bool storeUnitsRemainingToBreakout = true;
                if (pOpt.isFillSizeHoles)
                {
                    if (pOpt._storeUnitsRemainingToBreakout[aStoreFilterIDX] <= 0)
                    {
                        storeUnitsRemainingToBreakout = false;
                    }
                }
                else
                {
                    if (ph.PackMultiple > pOpt._storeUnitsRemainingToBreakout[aStoreFilterIDX])
                    {
                        storeUnitsRemainingToBreakout = false;
                    }
                }
                // End TT#1767-MD - stodd - Fill Size Method not processing units for Packs when it's a small quantity

                if (pOpt._packsToAllocate[packIDX] <= 0
                    // Begin TT#1767-MD - stodd - Fill Size Method not processing units for Packs when it's a small quantity
                    //|| ph.PackMultiple > pOpt._storeUnitsRemainingToBreakout[aStoreFilterIDX]
                    || !storeUnitsRemainingToBreakout
                    // End TT#1767-MD - stodd - Fill Size Method not processing units for Packs when it's a small quantity
                    || _allocationProfile.GetStoreIsManuallyAllocated(ph, pOpt._storeIdxRID[aStoreFilterIDX])
                    || _allocationProfile.GetStoreOut(ph, pOpt._storeIdxRID[aStoreFilterIDX])
                    || _allocationProfile.GetStoreLocked(ph, pOpt._storeIdxRID[aStoreFilterIDX])
                    //|| _allocationProfile.GetStoreTempLock(ph, _storeIdxRID[aStoreFilterIDX])      // TT#421 Detail Packs fail to allocate due to temp locks
                    || !Enum.IsDefined(typeof(eChosenRuleAllowsMoreAllocation), (int)_allocationProfile.GetStoreChosenRuleType(ph, pOpt._storeIdxRID[aStoreFilterIDX]))
                    )
                {
                    pOpt._storePackAvailable[aStoreFilterIDX, packIDX] = false;
                }
                else
                {
                    pOpt._storePackAvailable[aStoreFilterIDX, packIDX] = true;
                }
                packIDX++;
            }
            //ReBuildStorePackCriteria(aStoreFilterIDX); // TT#1365 - JEllis - FL Detail Pack Size Need Enhancement  (moved as 1st thing to do in FitPacksToStores)
        }



        /// <summary>
        /// Determines "best fit" of packs to remaining store detail allocation
        /// </summary>
        /// <param name="aMaxPackNeedTolerance">Maximum Pack Need Tolerance to use for best fit</param>
        private static void FitPacksToStores(SizeNeedResults aSizeNeedResults, DetailPackOptions pOpt, AllocationProfile _allocationProfile, ApplicationSessionTransaction _transaction) //  TT#1068 - MD - Jellis - Pack allocations not considered in decisions for bulk size allocations
        {
            int storeFilterIDX;
            int packIDX;
            for (storeFilterIDX = 0; storeFilterIDX < pOpt._storeFilterCount; storeFilterIDX++)
            {
                ReBuildStorePackCriteria(aSizeNeedResults, storeFilterIDX, pOpt, _allocationProfile, _transaction); //  TT#1068 - MD - Jellis - Pack allocations not considered in decisions for bulk size allocations
            }
            bool candidateStores = false;
            bool packsAllocated;
            Index_RID storeIdxRID;

            ArrayList candidateStoreIDX = new ArrayList();
            for (int storeIDX = 0; storeIDX < pOpt._storeFilterCount; storeIDX++)
            {
                storeIdxRID = pOpt._storeIdxRID[storeIDX];
                if (pOpt._storeCandidateCount[storeIDX] > 0
                    && pOpt._storeUnitsRemainingToBreakout[storeIDX] > 0)
                {
                    candidateStores = true;
                    candidateStoreIDX.Add(storeIDX);
                }
            }
            if (candidateStores)
            {
                int sortIDX = 0;
                int startSortAtIDX = 0;
                int sortEntryCount;
                MIDGenericSortItem[] candidateStore = new MIDGenericSortItem[candidateStoreIDX.Count];
                foreach (int storeIDX in candidateStoreIDX)
                {
                    candidateStore[sortIDX] = BuildCandidateStore(storeIDX, pOpt, _transaction);
                    sortIDX++;
                }
                packsAllocated = true;
                MIDGenericSortAscendingComparer sac = new MIDGenericSortAscendingComparer();  // TT#1143 - MD- Jellis - Group Allocation - Min broken
                while
                    (startSortAtIDX < candidateStoreIDX.Count
                    && packsAllocated)
                {
                    packsAllocated = false;
                    sortEntryCount = candidateStoreIDX.Count - startSortAtIDX;
                    Array.Sort(candidateStore, startSortAtIDX, sortEntryCount, sac);
                    while
                        (startSortAtIDX < candidateStoreIDX.Count
                        &&
                        (pOpt._storeCandidateCount[candidateStore[startSortAtIDX].Item] <= 0
                        || pOpt._storeUnitsRemainingToBreakout[candidateStore[startSortAtIDX].Item] <= 0
                        || pOpt._storePrimaryPackIDX[candidateStore[startSortAtIDX].Item] < 0)
                        )
                    {
                        startSortAtIDX++;
                    }
                    if (startSortAtIDX < candidateStoreIDX.Count)
                    {
                        packsAllocated = true;
                        storeFilterIDX = candidateStore[startSortAtIDX].Item;
                        packIDX = pOpt._storePrimaryPackIDX[storeFilterIDX];
                        UpdateStorePackCriteria(
                            aSizeNeedResults, //  TT#1068 - MD - Jellis - Pack allocations not considered in decisions for bulk size allocations
                            storeFilterIDX,
                            packIDX,
                            1, pOpt, _allocationProfile, _transaction);
                        candidateStore[startSortAtIDX] = BuildCandidateStore(storeFilterIDX, pOpt, _transaction);
                        if (pOpt._packsToAllocate[packIDX] <= 0)
                        {
                            for (sortIDX = startSortAtIDX + 1; sortIDX < candidateStoreIDX.Count; sortIDX++)
                            {
                                storeFilterIDX = candidateStore[sortIDX].Item;
                                if (packIDX == pOpt._storePrimaryPackIDX[storeFilterIDX])
                                {
                                    ReBuildStorePackCriteria(aSizeNeedResults, storeFilterIDX, pOpt, _allocationProfile, _transaction); //  TT#1068 - MD - Jellis - Pack allocations not considered in decisions for bulk size allocations
                                    candidateStore[sortIDX] = BuildCandidateStore(storeFilterIDX, pOpt, _transaction);
                                }
                            }
                        }
                    }
                }
            }

        }

        /// <summary>
        /// Updates a store's pack criteria (typically after a store has been allocated a pack)
        /// </summary>
        /// <param name="aMaxPackNeedTolerance">Maximum Pack Need Tolerance (Ship Variance). This varies when stepped tolerances are active (ie. the tolerances are varied from 0 to maximum specified tolerance)</param>
        /// <param name="aStoreFilterIDX">Identifies the store</param>
        /// <param name="aPackIDX">Identifies the pack that was allocated</param>
        /// <param name="aPacksAllocated">Number of packs allocated</param>
        private static void UpdateStorePackCriteria(SizeNeedResults aSizeNeedResults, int aStoreFilterIDX, int aPackIDX, int aPacksAllocated, DetailPackOptions pOpt, AllocationProfile _allocationProfile, ApplicationSessionTransaction _transaction) //  TT#1068 - MD - Jellis - Pack allocations not considered in decisions for bulk size allocations
        // end TT#1365 - JEllis - FL Detail Pack Size Need ENhancement
        {
            // Begin TT#1386-MD - stodd - Manual Merge
            StoreProfile storeProfile = (StoreProfile)pOpt._storeFilteredList[aStoreFilterIDX];
            pOpt._storeIdxRID[aStoreFilterIDX] = _transaction.StoreIndexRID(storeProfile.Key); 
            // End TT#1386-MD - stodd - Manual Merge

            pOpt._packsToAllocate[aPackIDX] -= aPacksAllocated;
            pOpt._storePacksAllocated[aStoreFilterIDX, aPackIDX] += aPacksAllocated;
            pOpt._storeUnitsRemainingToBreakout[aStoreFilterIDX] -=
                aPacksAllocated
                * pOpt._packMultiple[aPackIDX];
			// Begin TT#1386-MD - stodd - Manual Merge 
            // begin TT#1568 - MD - Jellis - GA Size Need not observing min/max
            if (pOpt._packApHome[aPackIDX].Key != _allocationProfile.Key)
            {
                StoreVector sv;
                if (pOpt._headerStoreUnitsAllocated.TryGetValue(pOpt._packApHome[aPackIDX].Key, out sv))
                {

                    //sv.SetStoreValue(
                    //    _storeIdxRID[aStoreFilterIDX].RID,
                    //    sv.GetStoreValue(_storeIdxRID[aStoreFilterIDX].RID)
                    //    + aPacksAllocated
                    //      * pOpt._packMultiple[aPackIDX]);

                    sv.SetStoreValue(
                        pOpt._storeIdxRID[aStoreFilterIDX].RID,
                        sv.GetStoreValue(pOpt._storeIdxRID[aStoreFilterIDX].RID)
                        + aPacksAllocated
                          * pOpt._packMultiple[aPackIDX]);

                }
                //WHat other InventoryForecast does this Header update????
                PackHdr ph = (PackHdr)_allocationProfile.NonGenericPacks[_allocationProfile.NonGenericPacks.GetKey(aPackIDX)];
                int[] inventoryList;
                foreach (PackColorSize pcs in ph.PackColors.Values)
                {
                    inventoryList = ((AssortmentProfile)_allocationProfile).GetInventoryUpdateList(pOpt._packApHome[aPackIDX].StyleHnRID, pcs.ColorCodeRID, false);
                    foreach (int inventoryRID in inventoryList)
                    {
                        if (pOpt._inventoryBasisAllocation.TryGetValue(inventoryRID, out sv))
                        {
                            //sv.SetStoreValue(
                            //    _storeIdxRID[aStoreFilterIDX].RID,
                            //    sv.GetStoreValue(_storeIdxRID[aStoreFilterIDX].RID)
                            //    + aPacksAllocated
                            //      * pcs.ColorUnitsInPack);

                            sv.SetStoreValue(
                                pOpt._storeIdxRID[aStoreFilterIDX].RID,
                                sv.GetStoreValue(pOpt._storeIdxRID[aStoreFilterIDX].RID)
                                + aPacksAllocated
                                  * pcs.ColorUnitsInPack);
                        }
                    }
                }
            }
            // end TT#1568 - MD - jellis - GA Size need not observing min/max
			// End TT#1386-MD - stodd - Manual Merge 
			
            int sizeIDX;
            int packSizeUnitsAllocated; // TT#1410 - FL Packs Allocated Exceed Tolerance
            for (sizeIDX = 0; sizeIDX < pOpt._sizeCount; sizeIDX++)
            {
                // begin TT#1410 - FL Packs Allocated Exceed Tolerance
                packSizeUnitsAllocated =
                    pOpt._packSizeContent[aPackIDX, sizeIDX]
                    * aPacksAllocated;
                if (packSizeUnitsAllocated >
                    pOpt._storeDesiredSizeUnits[aStoreFilterIDX, sizeIDX])
                //if (_packSizeContent[aPackIDX, sizeIDX] > 
                //    _storeDesiredSizeUnits[aStoreFilterIDX, sizeIDX])
                // end TT#1410 - FL Packs Allocated Exceed Tolerance
                {
                    pOpt._storeDesiredTotalUnits[aStoreFilterIDX] -= pOpt._storeDesiredSizeUnits[aStoreFilterIDX, sizeIDX];
                    pOpt._storeDesiredSizeUnits[aStoreFilterIDX, sizeIDX] = 0;
                }
                else
                {
                    // begin TT#1410 - FL Packs Allocated Exceed Tolerance
                    pOpt._storeDesiredSizeUnits[aStoreFilterIDX, sizeIDX] -=
                        packSizeUnitsAllocated;
                    pOpt._storeDesiredTotalUnits[aStoreFilterIDX] -=
                        packSizeUnitsAllocated;
                }
                pOpt._storeSizePosNeed[aStoreFilterIDX, sizeIDX] -=
                    packSizeUnitsAllocated;
            }
            ReBuildStorePackCriteria(aSizeNeedResults, aStoreFilterIDX, pOpt, _allocationProfile, _transaction); // TT#1365 - JEllis - FL Detail Pack Size Need Enhancement //  TT#1068 - MD - Jellis - Pack allocations not considered in decisions for bulk size allocations
        }

        /// <summary>
        /// ReBuilds a given store's pack criteria based on the current Maximum Pack Need Tolerance 
        /// </summary>
        /// <param name="aMaxPackNeedTolerance">Max Pack Need Tolerance (Ship Variance).  This value varies from 0 upto the specified maximum when the tolerance is "stepped"; does not vary when "stepped" is off</param>
        /// <param name="aStoreFilterIDX">Identifies the store</param>
        private static void ReBuildStorePackCriteria(SizeNeedResults aSizeNeedResults, int aStoreFilterIDX, DetailPackOptions pOpt, AllocationProfile _allocationProfile, ApplicationSessionTransaction _transaction) //  TT#1068 - MD - Jellis - Pack allocations not considered in decisions for bulk size allocations
        {
            StoreProfile storeProfile = (StoreProfile)pOpt._storeFilteredList.ArrayList[aStoreFilterIDX];
            int sizeRID;
            int sizeIDX;
            int sizesWithPosNeed = 0;
            double planUnits;
            pOpt._storeTotalNeedError[aStoreFilterIDX] = 0;
            pOpt._storeTotalPosPctNeed[aStoreFilterIDX] = 0;
            pOpt._storeTotalPosUnitNeed[aStoreFilterIDX] = 0;
            pOpt._storeAvgPosUnitNeed[aStoreFilterIDX] = 0;
            pOpt._storeAvgPosPctNeed[aStoreFilterIDX] = 0;
            pOpt._storePrimaryPackIDX[aStoreFilterIDX] = Include.NoRID;
            int sizeNeedError;  // TT#1410 - OVER ALLOCATE MAX TOLERANCE
            for (sizeIDX = 0; sizeIDX < pOpt._sizeCount; sizeIDX++)
            {
                sizeRID = (int)aSizeNeedResults.Sizes[sizeIDX]; //  TT#1068 - MD - Jellis - Pack allocations not considered in decisions for bulk size allocations
                if (pOpt._storeSizePosNeed[aStoreFilterIDX, sizeIDX] > 0)
                {
                    sizeNeedError = 0; // TT#1410 - OVER ALLOCATE MAX TOLERANCE
                    sizesWithPosNeed++;
                    pOpt._storeTotalPosUnitNeed[aStoreFilterIDX] += (int)pOpt._storeSizePosNeed[aStoreFilterIDX, sizeIDX];
                    pOpt._storeSizeNeedError[aStoreFilterIDX, sizeIDX] = 0;
                    planUnits = aSizeNeedResults.GetSizeNeed_PlanUnits(storeProfile.Key, sizeRID);  // MID Track 4921 AnF#666 Fill to Size Plan Enhancement //  TT#1068 - MD - Jellis - Pack allocations not considered in decisions for bulk size allocations
                    if (planUnits > 0)
                    {
                        pOpt._storeSizePosPctNeed[aStoreFilterIDX, sizeIDX] =
                        (double)pOpt._storeSizePosNeed[aStoreFilterIDX, sizeIDX] * 100.0d
                            / planUnits;
                        pOpt._storeTotalPosPctNeed[aStoreFilterIDX] += pOpt._storeSizePosPctNeed[aStoreFilterIDX, sizeIDX];
                    }
                    else
                    {
                        pOpt._storeSizePosPctNeed[aStoreFilterIDX, sizeIDX] = 0;
                    }
                }
                else
                {
                    sizeNeedError = Math.Abs((int)pOpt._storeSizePosNeed[aStoreFilterIDX, sizeIDX]); // TT#1410 OVER ALLCOATE MAX TOLERANCE
                    //_storeSizeNeedError[aStoreFilterIDX, sizeIDX] =                           // TT#1410 OVER ALLCOATE MAX TOLERANCE 
                    //    Math.Abs((int)_storeSizePosNeed[aStoreFilterIDX, sizeIDX]);           // TT#1410 OVER ALLCOATE MAX TOLERANCE
                    pOpt._storeSizePosNeed[aStoreFilterIDX, sizeIDX] = 0;
                    pOpt._storeSizePosPctNeed[aStoreFilterIDX, sizeIDX] = 0;
                }
                // NOTE:  we just want a reasonable percent to total to use for comparison purposes
                // to find the best fit packs. 
                pOpt._storeSizeNeedError[aStoreFilterIDX, sizeIDX] += sizeNeedError;               // TT#1410 OVER ALLOCATE MAX TOLERANCE
                pOpt._storeTotalNeedError[aStoreFilterIDX] += pOpt._storeSizeNeedError[aStoreFilterIDX, sizeIDX];
                if (pOpt._storeDesiredTotalUnits[aStoreFilterIDX] > 0)
                {
                    pOpt._storeDesiredAllocationCurve[aStoreFilterIDX, sizeIDX] =
                        pOpt._storeDesiredSizeUnits[aStoreFilterIDX, sizeIDX]
                        * 100 / pOpt._storeDesiredTotalUnits[aStoreFilterIDX];
                }
                else
                {
                    pOpt._storeDesiredAllocationCurve[aStoreFilterIDX, sizeIDX] = 0;
                }
            }

            if (sizesWithPosNeed > 0)
            {
                pOpt._storeAvgPosUnitNeed[aStoreFilterIDX] =
                    (double)pOpt._storeTotalPosUnitNeed[aStoreFilterIDX]
                    / (double)sizesWithPosNeed;
                pOpt._storeAvgPosPctNeed[aStoreFilterIDX] =
                    (double)pOpt._storeTotalPosPctNeed[aStoreFilterIDX]
                    / (double)sizesWithPosNeed;
            }
            IdentifyCandidatePacks(aSizeNeedResults, aStoreFilterIDX, pOpt, _allocationProfile, _transaction); // TT#1365 - JEllis - FL Detail Pack Size Need Enhancement //  TT#1068 - MD - Jellis - Pack allocations not considered in decisions for bulk size allocations
            //IdentifyCandidatePacks(aStoreFilterIDX); // TT#1365 - JEllis - FL Detail Pack Size Need Enhancement
        }
        /// <summary>
        /// Identifies candidate packs
        /// </summary>
        /// <param name="aMaxPackNeedTolerance">Max Pack Need Tolerance (Ship Variance). For Stepped tolerances, this varies from 0 upto the specified maximum depending on which "step" the algorithm is on</param>
        /// <param name="aStoreFilterIDX">Identifies the store</param>
        private static void IdentifyCandidatePacks(SizeNeedResults aSizeNeedResults, int aStoreFilterIDX, DetailPackOptions pOpt, AllocationProfile _allocationProfile, ApplicationSessionTransaction _transaction) //  TT#1068 - MD - Jellis - Pack allocations not considered in decisions for bulk size allocations
        {
            int packIDX = 0;
            int sizeIDX;
            int sizeRID;
            int sortIDX;
            int potentialCandidateCount = 0;

			// Begin TT#1386-MD - stodd - Manual Merge 
            StoreVector sv;  // TT#1568 - MD - Jellis - GA Size Need not observing min/max
            int maximum; // TT#1568 - MD - Jellis - GA Size need not observing min/max
            int minimum; // TT#1568 - MD - Jellis - GA Size need not observing min/max
			// End TT#1386-MD - stodd - Manual Merge
			
            foreach (PackHdr ph in _allocationProfile.NonGenericPacks.Values)
            {
                pOpt._storePackCandidate[aStoreFilterIDX, packIDX] = false;

                // Begin TT#1767-MD - stodd - Fill Size Method not processing units for Packs when it's a small quantity
                //==============================================================================================
                // Added the storeUnitsRemainingToBreakout bool to help manage the 
                // large "if" statement after the bool is checked.
                // Different question needs to be asked depending upon whether it's Fill Size Holes or not.
                //==============================================================================================
                bool storeUnitsRemainingToBreakout = true;
                if (pOpt.isFillSizeHoles)
                {
                    if (pOpt._storeUnitsRemainingToBreakout[aStoreFilterIDX] <= 0)
                    {
                        storeUnitsRemainingToBreakout = false;
                    }
                }
                else
                {
                    if (ph.PackMultiple > pOpt._storeUnitsRemainingToBreakout[aStoreFilterIDX])
                    {
                        storeUnitsRemainingToBreakout = false;
                    }
                }
                // End TT#1767-MD - stodd - Fill Size Method not processing units for Packs when it's a small quantity

                // Begin TT#1386-MD - stodd - Manual Merge
                StoreProfile storeProfile = (StoreProfile)pOpt._storeFilteredList[aStoreFilterIDX];
                pOpt._storeIdxRID[aStoreFilterIDX] = _transaction.StoreIndexRID(storeProfile.Key);
                // End TT#1386-MD - stodd - Manual Merge


                if (pOpt._storePackAvailable[aStoreFilterIDX, packIDX])
                {
                    if (pOpt._packsToAllocate[packIDX] <= 0

                        // Begin TT#1767-MD - stodd - Fill Size Method not processing units for Packs when it's a small quantity
                        //|| ph.PackMultiple > pOpt._storeUnitsRemainingToBreakout[aStoreFilterIDX]
                         || !storeUnitsRemainingToBreakout
                        // End TT#1767-MD - stodd - Fill Size Method not processing units for Packs when it's a small quantity
                        )
                    {
                        pOpt._storePackAvailable[aStoreFilterIDX, packIDX] = false;
                    }
					// Begin TT#1386-MD - stodd - Manual Merge
                    else
                    {
                        if (pOpt._packApHome[packIDX] != _allocationProfile)
                        {
                            //maximum = pOpt._packApHome[packIDX].GetStoreMaximum(eAllocationSummaryNode.Total, _storeIdxRID[aStoreFilterIDX], false);
                            //minimum = pOpt._packApHome[packIDX].GetStoreMinimum(eAllocationSummaryNode.Total, _storeIdxRID[aStoreFilterIDX], false);
                            maximum = pOpt._packApHome[packIDX].GetStoreMaximum(eAllocationSummaryNode.Total, pOpt._storeIdxRID[aStoreFilterIDX].RID, false);
                            minimum = pOpt._packApHome[packIDX].GetStoreMinimum(eAllocationSummaryNode.Total, pOpt._storeIdxRID[aStoreFilterIDX].RID, false);
                            if (pOpt._packApHome[packIDX].GradeInventoryBasisHnRID != Include.NoRID)
                            {
                                if (pOpt._inventoryBasisAllocation.TryGetValue(pOpt._packApHome[packIDX].GradeInventoryBasisHnRID, out sv))
                                {
                                    //maximum =
                                    //    Math.Max(0,
                                    //             maximum
                                    //             - (int)sv.GetStoreValue(_storeIdxRID[aStoreFilterIDX].RID));
                                    maximum =
                                        Math.Max(0,
                                                 maximum
                                                 - (int)sv.GetStoreValue(pOpt._storeIdxRID[aStoreFilterIDX].RID));

                                    //minimum =
                                    //    Math.Max(0,
                                    //             minimum 
                                    //             - (int)sv.GetStoreValue(_storeIdxRID[aStoreFilterIDX].RID));
                                    minimum =
                                        Math.Max(0,
                                                 minimum
                                                 - (int)sv.GetStoreValue(pOpt._storeIdxRID[aStoreFilterIDX].RID));
                                }
                            }
                            else
                            {
                                if (pOpt._headerStoreUnitsAllocated.TryGetValue(pOpt._packApHome[packIDX].Key, out sv))
                                {
                                    //maximum =
                                    //    Math.Max(0,
                                    //             maximum
                                    //             - (int)sv.GetStoreValue(_storeIdxRID[aStoreFilterIDX].RID));
                                    maximum =
                                        Math.Max(0,
                                                 maximum
                                                 - (int)sv.GetStoreValue(pOpt._storeIdxRID[aStoreFilterIDX].RID));

                                    //minimum =
                                    //    Math.Max(0,
                                    //             minimum
                                    //             - (int)sv.GetStoreValue(_storeIdxRID[aStoreFilterIDX].RID));
                                    minimum =
                                        Math.Max(0,
                                                 minimum
                                                 - (int)sv.GetStoreValue(pOpt._storeIdxRID[aStoreFilterIDX].RID));
                                }
                            }
                        }
                        else
                        {
                            maximum = int.MaxValue;
                            minimum = 0;
                        }
                        if (maximum < ph.PackMultiple
                            || (ph.PackMultiple < minimum
                                //&& pOpt._packApHome[packIDX].GetStoreQtyAllocated(eAllocationSummaryNode.Total, _storeIdxRID[aStoreFilterIDX]) == 0))
                                && pOpt._packApHome[packIDX].GetStoreQtyAllocated(eAllocationSummaryNode.Total, pOpt._storeIdxRID[aStoreFilterIDX].RID) == 0))
                        {
                            pOpt._storePackAvailable[aStoreFilterIDX, packIDX] = false;
                        }
                        // end TT#1568 - MD - Jellis - GA Size need not observing min/max
						// End TT#1386-MD - stodd - Manual Merge
					    else
					    {
                        pOpt._storePackGap[aStoreFilterIDX, packIDX] = -1;
                        pOpt._storePackPctNeedMeasure[aStoreFilterIDX, packIDX] = 0;  // TT#2920 - Jellis - Detail Pack Algorithm Change
                        pOpt._storePackDeviationError[aStoreFilterIDX, packIDX] = 0;
                        pOpt._storePackDeviationErrorCount[aStoreFilterIDX, packIDX] = 0;
                        pOpt._storePackNeedError[aStoreFilterIDX, packIDX] = 0;
                        int storeNeedErrorInPackSizes = 0;  // TT#2920 - Jellis - Detail Pack Algorithm Change
                        for (sizeIDX = 0; sizeIDX < pOpt._sizeCount; sizeIDX++)
                        {
                            sizeRID = (int)aSizeNeedResults.Sizes[sizeIDX]; //  TT#1068 - MD - Jellis - Pack allocations not considered in decisions for bulk size allocations
                            if (pOpt._packSizeContent[packIDX, sizeIDX] >
                                pOpt._storeDesiredSizeUnits[aStoreFilterIDX, sizeIDX])
                            //							_sizeNeedResults.GetAllocatedUnits(_storeIdxRID[aStoreFilterIDX].RID, sizeRID))
                            {
                                pOpt._storePackDeviationError[aStoreFilterIDX, packIDX] +=
                                    pOpt._packSizeContent[packIDX, sizeIDX]
                                    - pOpt._storeDesiredSizeUnits[aStoreFilterIDX, sizeIDX];
                                //								- _sizeNeedResults.GetAllocatedUnits(_storeIdxRID[aStoreFilterIDX].RID, sizeRID);
                                pOpt._storePackDeviationErrorCount[aStoreFilterIDX, packIDX]++;
                            }
                            if (pOpt._packSizeContent[packIDX, sizeIDX] >
                                pOpt._storeSizePosNeed[aStoreFilterIDX, sizeIDX])
                            {
                                pOpt._storePackNeedError[aStoreFilterIDX, packIDX] +=
                                    pOpt._packSizeContent[packIDX, sizeIDX]
                                    - pOpt._storeSizePosNeed[aStoreFilterIDX, sizeIDX];
                            }
                            // begin TT#2920 - Jellis - Detail Pack Algorithm Change
                            if (pOpt._packSizeContent[packIDX, sizeIDX] > 0)
                            {
                                storeNeedErrorInPackSizes += pOpt._storeSizeNeedError[aStoreFilterIDX, sizeIDX];
                                //if (_storeSizePlan[aStoreFilterIDX, packIDX] > 0)  // TT#1106 - MD - Jellis - Size Need - Index Out of Range
                                if (pOpt._storeSizePlan[aStoreFilterIDX, sizeIDX] > 0)    // TT#1106 - MD - Jellis- Size Need - Index Out of Range
                                {
                                    pOpt._storePackPctNeedMeasure[aStoreFilterIDX, packIDX] +=
                                        Math.Abs(
                                            pOpt._storeSizePosPctNeed[aStoreFilterIDX, sizeIDX]
                                            - (100 * (pOpt._storeSizePosNeed[aStoreFilterIDX, sizeIDX] - pOpt._packSizeContent[packIDX, sizeIDX])
                                                / pOpt._storeSizePlan[aStoreFilterIDX, sizeIDX])
                                                );
                                }
                            }
                            // Begin TT#4109 - JSmith - Size Need Method-> Pack Tolerance= 0 ; results ignore the 12 pc pack
                            else
                            {
                                if (pOpt._storeSizePlan[aStoreFilterIDX, sizeIDX] > 0)    // TT#1106 - MD - Jellis- Size Need - Index Out of Range
                                {
                                    pOpt._storePackPctNeedMeasure[aStoreFilterIDX, packIDX] +=
                                            pOpt._storeSizePosPctNeed[aStoreFilterIDX, sizeIDX];
                                }
                            }
                            // End TT#4109 - JSmith - Size Need Method-> Pack Tolerance= 0 ; results ignore the 12 pc pack
                            // end TT#2920 - Jellis - Detail Pack Algorithm Change
                        }
                        if (pOpt._storePackDeviationErrorCount[aStoreFilterIDX, packIDX] > 0)
                        {
                            pOpt._storePackAvgDeviationError[aStoreFilterIDX, packIDX] =
                                (double)pOpt._storePackDeviationError[aStoreFilterIDX, packIDX]
                                / (double)pOpt._storePackDeviationErrorCount[aStoreFilterIDX, packIDX];
                        }
                        else
                        {
                            pOpt._storePackAvgDeviationError[aStoreFilterIDX, packIDX] = 0;
                        }
                        if (pOpt._storePackAvgDeviationError[aStoreFilterIDX, packIDX] <=
                            pOpt.avgPackDeviationTolerance
                            &&
                            (
                            (pOpt._storePackNeedError[aStoreFilterIDX, packIDX]
                            // begin TT#2920 - Jellis - Detail Pack Algorithm Change
                                + storeNeedErrorInPackSizes <=
                                pOpt.maxPackNeedTolerance
                            )))
                        // + _storeTotalNeedError[aStoreFilterIDX]) <=
                        //aMaxPackNeedTolerance       // TT#1365 - JEllis - FL Detail Pack Size Need Enhancement
                        ////this.MaxPackNeedTolerance // TT#1365 - JEllis - FL Detail Pack Size Need Enhancement
                        //)))
                        // end TT#2920 - Jellis - Detail Pack Algorithm Change
                        {
                            pOpt._storePackCandidate[aStoreFilterIDX, packIDX] = true;
                            potentialCandidateCount++;
                        }
						}
                    }
                }		// TT#1386-MD - stodd - Manual Merge
                packIDX++;
            }
            if (potentialCandidateCount > 0)
            {
                MIDGenericSortItem[] sortedPack = new MIDGenericSortItem[potentialCandidateCount];
                packIDX = 0;
                sortIDX = 0;
                for (packIDX = 0; packIDX < pOpt._packCount; packIDX++)
                {
                    if (pOpt._storePackCandidate[aStoreFilterIDX, packIDX])
                    {
                        sortedPack[sortIDX].Item = packIDX;
                        sortedPack[sortIDX].SortKey = new double[6];  // TT#2920 - Jellis - Detail Pack Algorithm Change
                        sortedPack[sortIDX].SortKey[0] = pOpt._storePackDeviationError[aStoreFilterIDX, packIDX];
                        sortedPack[sortIDX].SortKey[1] = pOpt._storePackNeedError[aStoreFilterIDX, packIDX];
                        sortedPack[sortIDX].SortKey[2] = pOpt._storePackPctNeedMeasure[aStoreFilterIDX, packIDX]; // TT#2920 - Jellis - Detail Pack Algorithm Change
                        sortedPack[sortIDX].SortKey[3] = -pOpt._packMultiple[packIDX];  // TT#2920 - Jellis - Detail Pack Algorithm Change
                        sortedPack[sortIDX].SortKey[4] = -pOpt._packsToAllocate[packIDX]; // TT#2920 - Jellis - Detail Pack Algorithm Change
                        sortedPack[sortIDX].SortKey[5] = _transaction.GetRandomDouble();
                        sortIDX++;
                    }
                    pOpt._storePackCandidate[aStoreFilterIDX, packIDX] = false;
                }
                Array.Sort(sortedPack, new MIDGenericSortAscendingComparer());  // TT#1143 - MD - Jellis - Group ALlocation Min Broken
                packIDX = sortedPack[0].Item;
                int targetPackNeedError = (int)pOpt._storePackNeedError[aStoreFilterIDX, packIDX];
                double targetPackAvgDeviationError = pOpt._storePackAvgDeviationError[aStoreFilterIDX, packIDX];
                pOpt._storeCandidateCount[aStoreFilterIDX] = 0;
                double totalCurveUnits; // TT#2920 - Jellis - Detail Pack Algorithm Change
                for (sortIDX = 0; sortIDX < potentialCandidateCount; sortIDX++)
                {
                    packIDX = sortedPack[sortIDX].Item;
                    if (pOpt._storePackNeedError[aStoreFilterIDX, packIDX] == targetPackNeedError
                        && pOpt._storePackAvgDeviationError[aStoreFilterIDX, packIDX] == targetPackAvgDeviationError)
                    {
                        pOpt._storePackCandidate[aStoreFilterIDX, packIDX] = true;
                        pOpt._storeCandidateCount[aStoreFilterIDX]++;
                    }
                    pOpt._storePackGap[aStoreFilterIDX, packIDX] = 0;
                    // begin TT#2920 -  Jellis - Detail Pack Algorithm Change 
                    totalCurveUnits =
                        pOpt._storeDesiredTotalUnits[aStoreFilterIDX]
                        - (double)pOpt._packMultiple[packIDX];
                    // END TT#2920 -  Jellis - Detail Pack Algorithm Change
                    for (sizeIDX = 0; sizeIDX < pOpt._sizeCount; sizeIDX++)
                    {
                        // begin TT#2920 -  Jellis - Detail Pack Algorithm Change
                        //_storePackGap[aStoreFilterIDX, packIDX] +=
                        //    Math.Abs
                        //    (
                        //    _storeDesiredAllocationCurve[aStoreFilterIDX, sizeIDX]
                        //    - _packSizeCurvePct[packIDX, sizeIDX]
                        //    );
                        if (pOpt._packSizeContent[packIDX, sizeIDX] > 0)
                        {
                            if (totalCurveUnits > 0)
                            {
                                pOpt._storePackGap[aStoreFilterIDX, packIDX] +=
                                    Math.Abs
                                    (
                                     ((pOpt._storeDesiredSizeUnits[aStoreFilterIDX, sizeIDX] - (double)pOpt._packSizeContent[packIDX, sizeIDX])
                                      / totalCurveUnits)
                                     );
                            }
                        }
                        // END TT#2920 -  Jellis - Detail Pack Algorithm Change

                    }
                }
                if (pOpt._storeCandidateCount[aStoreFilterIDX] == 1
                    && potentialCandidateCount > 1)
                {
                    packIDX = sortedPack[1].Item;
                    targetPackNeedError = (int)pOpt._storePackNeedError[aStoreFilterIDX, packIDX];
                    targetPackAvgDeviationError = pOpt._storePackAvgDeviationError[aStoreFilterIDX, packIDX];
                    for (sortIDX = 1; sortIDX < potentialCandidateCount; sortIDX++)
                    {
                        packIDX = sortedPack[sortIDX].Item;
                        if (pOpt._storePackNeedError[aStoreFilterIDX, packIDX] == targetPackNeedError
                            && pOpt._storePackAvgDeviationError[aStoreFilterIDX, packIDX] == targetPackAvgDeviationError)
                        {
                            pOpt._storePackCandidate[aStoreFilterIDX, packIDX] = true;
                            pOpt._storeCandidateCount[aStoreFilterIDX]++;
                        }
                    }
                }
                if (pOpt._storeCandidateCount[aStoreFilterIDX] > 0)
                {
                    MIDGenericSortItem[] candidatePack = new MIDGenericSortItem[pOpt._storeCandidateCount[aStoreFilterIDX]];
                    packIDX = 0;
                    sortIDX = 0;
                    for (packIDX = 0; packIDX < pOpt._packCount; packIDX++)
                    {
                        if (pOpt._storePackCandidate[aStoreFilterIDX, packIDX])
                        {
                            candidatePack[sortIDX].Item = packIDX;
                            candidatePack[sortIDX].SortKey = new double[7]; // TT#2920 - Jellis - Detail Pack Algorithm Change
                            candidatePack[sortIDX].SortKey[0] = pOpt._storePackDeviationError[aStoreFilterIDX, packIDX];
                            candidatePack[sortIDX].SortKey[1] = pOpt._storePackNeedError[aStoreFilterIDX, packIDX];
                            candidatePack[sortIDX].SortKey[2] = pOpt._storePackPctNeedMeasure[aStoreFilterIDX, packIDX]; // TT#2920 - Jellis - Detail Pack Algorithm Change
                            candidatePack[sortIDX].SortKey[3] = pOpt._storePackGap[aStoreFilterIDX, packIDX]; // TT#2920 - Jellis - Detail Pack Algorithm Change
                            candidatePack[sortIDX].SortKey[4] = -pOpt._packMultiple[packIDX];  // descending order // TT#2920 - Jellis - Detail Pack Algorithm Change
                            candidatePack[sortIDX].SortKey[5] = -pOpt._packsToAllocate[packIDX]; // TT#2920 - Jellis - Detail Pack Algorithm Change
                            candidatePack[sortIDX].SortKey[6] = _transaction.GetRandomDouble(); // TT#2920 - Jellis - Detail Pack Algorithm Change
                            sortIDX++;
                        }
                    }
                    Array.Sort(candidatePack, new MIDGenericSortAscendingComparer()); // TT#1143 - MD - Jellis - Group ALlocation Min Broken
                    int packIDX_1;
                    int packIDX_2;
                    if (pOpt._storeCandidateCount[aStoreFilterIDX] > 1)
                    {
                        packIDX_1 = candidatePack[0].Item;
                        pOpt._storePrimaryPackIDX[aStoreFilterIDX] = packIDX_1;
                        packIDX_2 = candidatePack[1].Item;
                        pOpt._storePackDeviationErrorDiff[aStoreFilterIDX] =
                            Math.Abs
                            (
                            pOpt._storePackAvgDeviationError[aStoreFilterIDX, packIDX_1]
                            - pOpt._storePackAvgDeviationError[aStoreFilterIDX, packIDX_2]
                            );
                        pOpt._storePackNeedErrorDiff[aStoreFilterIDX] =
                            Math.Abs
                            (
                            pOpt._storePackNeedError[aStoreFilterIDX, packIDX_1]
                            - pOpt._storePackNeedError[aStoreFilterIDX, packIDX_2]
                            );
                        pOpt._storePackGapDiff[aStoreFilterIDX] =
                            Math.Abs
                            (
                            pOpt._storePackGap[aStoreFilterIDX, packIDX_1]
                            - pOpt._storePackGap[aStoreFilterIDX, packIDX_2]
                            );
                    }
                    else
                    {
                        pOpt._storePrimaryPackIDX[aStoreFilterIDX] = candidatePack[0].Item;
                        pOpt._storePackDeviationErrorDiff[aStoreFilterIDX] = 0;
                        pOpt._storePackNeedErrorDiff[aStoreFilterIDX] = 0;
                        pOpt._storePackGapDiff[aStoreFilterIDX] = 0;
                    }
                }
            }
        }

          

        private static MIDGenericSortItem BuildCandidateStore(int aStoreIDX, DetailPackOptions pOpt, ApplicationSessionTransaction _transaction)
        {
            MIDGenericSortItem candidateStoreSortEntry = new MIDGenericSortItem();
            candidateStoreSortEntry.Item = aStoreIDX;
            candidateStoreSortEntry.SortKey = new double[13]; // TT#2920 -  Jellis - Detail Pack Algorithm Change
            candidateStoreSortEntry.SortKey[0] = -pOpt._storeAvgPosPctNeed[aStoreIDX];
            candidateStoreSortEntry.SortKey[01] = -pOpt._storeAvgPosUnitNeed[aStoreIDX];
            if (pOpt._storeCandidateCount[aStoreIDX] > 1)
            {
                candidateStoreSortEntry.SortKey[02] = 2;
            }
            else
            {
                candidateStoreSortEntry.SortKey[02] = pOpt._storeCandidateCount[aStoreIDX];
            }
            if (pOpt._storePrimaryPackIDX[aStoreIDX] < 0)
            {
                candidateStoreSortEntry.SortKey[03] = double.MinValue;
                candidateStoreSortEntry.SortKey[06] = double.MinValue;
                candidateStoreSortEntry.SortKey[07] = double.MinValue;
                candidateStoreSortEntry.SortKey[08] = double.MinValue;
                candidateStoreSortEntry.SortKey[09] = double.MinValue;
                // begin TT#2920 -  Jellis - Detail Pack Algorithm Change
                candidateStoreSortEntry.SortKey[10] = double.MinValue;
                // end TT#2920 -  Jellis - Detail Pack Algorithm Change
            }
            else
            {
                candidateStoreSortEntry.SortKey[03] = pOpt._storePackDeviationError[aStoreIDX, pOpt._storePrimaryPackIDX[aStoreIDX]];  //  TT#1410 - FL System Not Allocating ENOUGH packs within given variance - should be ascending sequence not descending
                //candidateStoreSortEntry.SortKey[03] = -_storePackDeviationError[aStoreIDX, _storePrimaryPackIDX[aStoreIDX]]; // TT#1410 - FL System Not Allocting ENOUGH packs within given variance - should be ascending sequence not descending
                candidateStoreSortEntry.SortKey[06] = pOpt._storePackAvgDeviationError[aStoreIDX, pOpt._storePrimaryPackIDX[aStoreIDX]];
                candidateStoreSortEntry.SortKey[07] = pOpt._storePackNeedError[aStoreIDX, pOpt._storePrimaryPackIDX[aStoreIDX]];
                candidateStoreSortEntry.SortKey[08] = pOpt._storePackPctNeedMeasure[aStoreIDX, pOpt._storePrimaryPackIDX[aStoreIDX]]; // TT#2920 -  Jellis - Detail Pack Algorithm Change
                candidateStoreSortEntry.SortKey[09] = pOpt._storePackGap[aStoreIDX, pOpt._storePrimaryPackIDX[aStoreIDX]];  // TT#2920 -  Jellis - Detail Pack Algorithm Change
                candidateStoreSortEntry.SortKey[10] = -pOpt._packMultiple[pOpt._storePrimaryPackIDX[aStoreIDX]];           // TT#2920 -  Jellis - Detail Pack Algorithm Change
            }
            candidateStoreSortEntry.SortKey[04] = -pOpt._storePackNeedErrorDiff[aStoreIDX];
            candidateStoreSortEntry.SortKey[05] = -pOpt._storePackGapDiff[aStoreIDX];
            // begin TT#2920 -  Jellis - Detail Pack Algorithm Change
            candidateStoreSortEntry.SortKey[11] = pOpt._storeCandidateCount[aStoreIDX];
            candidateStoreSortEntry.SortKey[12] = _transaction.GetRandomDouble();
            //candidateStoreSortEntry.SortKey[10] = _storeCandidateCount[aStoreIDX];
            //candidateStoreSortEntry.SortKey[11] = _transaction.GetRandomDouble();
            // end TT#2920 -  Jellis - Detail Pack Algorithm Change
            return candidateStoreSortEntry;
        }




        // begin TT#1018 - MD - JEllis - Group ALlocation Over Allocates Detail
        public static void AdjustDetailTotals(AllocationProfile _allocationProfile)
        {
            // Begin TT#4988 - BVaughan - Performance
            #if DEBUG
            if ((_allocationProfile is AssortmentProfile && !_allocationProfile.isAssortmentProfile) || (!(_allocationProfile is AssortmentProfile) && _allocationProfile.isAssortmentProfile))
            {
                throw new Exception("Object does not match AssortmentProfile in AdjustDetailTotals()");
            }
            #endif
            //if (_allocationProfile is AssortmentProfile)
            if (_allocationProfile.isAssortmentProfile)
            // End TT#4988 - BVaughan - Performance
            {
                AssortmentProfile assrtMent = _allocationProfile as AssortmentProfile;
                int detailDifference;
                int bulkDifference;
                int remainingNegativeGap;
                List<AllocationProfile> positiveProfile = new List<AllocationProfile>();
                List<AllocationProfile> negativeProfile = new List<AllocationProfile>();

                foreach (Index_RID storeIdxRID in _allocationProfile.AppSessionTransaction.StoreIndexRIDArray())
                {
                    // begin TT#1063 - MD - Jellis - Group Allocation non-size Results change after Size Need 
                    // Adjust Detail first
                    positiveProfile.Clear();
                    negativeProfile.Clear();

                    foreach (AllocationProfile ap in assrtMent.AssortmentMembers)
                    {
                        detailDifference =
                            ap.GetStoreQtyAllocated(eAllocationSummaryNode.DetailType, storeIdxRID)
                            - ap.GetStoreQtyAllocated(eAllocationSummaryNode.DetailSubType, storeIdxRID);
                        if (detailDifference < 0)
                        {
                            negativeProfile.Add(ap);
                        }
                        else if (detailDifference > 0)
                        {
                            positiveProfile.Add(ap);
                        }
                    }

                    foreach (AllocationProfile negAp in negativeProfile)
                    {
                        remainingNegativeGap =
                            negAp.GetStoreQtyAllocated(eAllocationSummaryNode.DetailSubType, storeIdxRID)
                            - negAp.GetStoreQtyAllocated(eAllocationSummaryNode.DetailType, storeIdxRID);
                        foreach (AllocationProfile posAp in positiveProfile)
                        {
                            detailDifference =
                                posAp.GetStoreQtyAllocated(eAllocationSummaryNode.DetailType, storeIdxRID)
                                - posAp.GetStoreQtyAllocated(eAllocationSummaryNode.DetailSubType, storeIdxRID);
                            if (detailDifference > 0)
                            {
                                if (remainingNegativeGap >
                                    detailDifference)
                                {
                                    posAp.SetStoreQtyAllocated(
                                        eAllocationSummaryNode.DetailType,
                                        storeIdxRID,
                                        posAp.GetStoreQtyAllocated(eAllocationSummaryNode.DetailSubType, storeIdxRID),
                                        eDistributeChange.ToParent,
                                        false,
                                        false,
                                        false);
                                    remainingNegativeGap -= detailDifference;
                                }
                                else
                                {
                                    posAp.SetStoreQtyAllocated(
                                        eAllocationSummaryNode.DetailType,
                                        storeIdxRID,
                                        posAp.GetStoreQtyAllocated(eAllocationSummaryNode.DetailSubType, storeIdxRID) + (detailDifference - remainingNegativeGap),
                                        eDistributeChange.ToParent,
                                        false,
                                        false,
                                        false);
                                    remainingNegativeGap = 0;
                                    break;
                                }
                            }
                            if (remainingNegativeGap == 0)
                            {
                                break;
                            }
                        }
                        negAp.SetStoreQtyAllocated(
                            eAllocationSummaryNode.DetailType,
                            storeIdxRID,
                            negAp.GetStoreQtyAllocated(eAllocationSummaryNode.DetailSubType, storeIdxRID)
                            - remainingNegativeGap,
                            eDistributeChange.ToParent,
                            false,
                            false,
                            false);
                    }


                    // adjust bulk LAST (It affects the detail totals)
                    // end TT#1063 - MD - Jellis - Group Allocation non-size Results change after Size Need
                    positiveProfile.Clear();
                    negativeProfile.Clear();
                    foreach (AllocationProfile ap in assrtMent.AssortmentMembers)
                    {
                        bulkDifference =
                            ap.GetStoreQtyAllocated(eAllocationSummaryNode.Bulk, storeIdxRID)
                            - ap.GetStoreQtyAllocated(eAllocationSummaryNode.BulkColorTotal, storeIdxRID);
                        if (bulkDifference < 0)
                        {
                            negativeProfile.Add(ap);
                        }
                        else if (bulkDifference > 0)
                        {
                            positiveProfile.Add(ap);
                        }
                    }
                    foreach (AllocationProfile negAp in negativeProfile)
                    {
                        remainingNegativeGap =
                            negAp.GetStoreQtyAllocated(eAllocationSummaryNode.BulkColorTotal, storeIdxRID)
                            - negAp.GetStoreQtyAllocated(eAllocationSummaryNode.Bulk, storeIdxRID);
                        foreach (AllocationProfile posAp in positiveProfile)
                        {
                            bulkDifference =
                                posAp.GetStoreQtyAllocated(eAllocationSummaryNode.Bulk, storeIdxRID)
                                - posAp.GetStoreQtyAllocated(eAllocationSummaryNode.BulkColorTotal, storeIdxRID);
                            if (bulkDifference > 0)
                            {
                                if (remainingNegativeGap >
                                    bulkDifference)
                                {
                                    posAp.SetStoreQtyAllocated(
                                        eAllocationSummaryNode.Bulk,
                                        storeIdxRID,
                                        posAp.GetStoreQtyAllocated(eAllocationSummaryNode.BulkColorTotal, storeIdxRID),
                                        eDistributeChange.ToParent,
                                        false,
                                        false,
                                        false);
                                    remainingNegativeGap -= bulkDifference;
                                }
                                else
                                {
                                    posAp.SetStoreQtyAllocated(
                                        eAllocationSummaryNode.Bulk,
                                        storeIdxRID,
                                        posAp.GetStoreQtyAllocated(eAllocationSummaryNode.BulkColorTotal, storeIdxRID) + (bulkDifference - remainingNegativeGap),
                                        eDistributeChange.ToParent,
                                        false,
                                        false,
                                        false);
                                    remainingNegativeGap = 0;
                                    break;
                                }
                            }
                            // begin TT#1063 - MD - Jellis - Group Allocation non-size Results change after Size Need
                            if (remainingNegativeGap == 0)
                            {
                                break;
                            }
                            // end TT#1063 - MD - Jellis - Group Allocation non-size Results change after Size Need
                        }
                        negAp.SetStoreQtyAllocated(
                            eAllocationSummaryNode.Bulk,
                            storeIdxRID,
                            negAp.GetStoreQtyAllocated(eAllocationSummaryNode.BulkColorTotal, storeIdxRID)
                            - remainingNegativeGap,
                            eDistributeChange.ToParent,
                            false,
                            false,
                            false);
                    }
                }
                // Begin TT#1857-MD - JSmith - After Size Need Style Allocation and Size Allocation are not in balance.
                // TT#4962 fixed during allocation in Size Need.  This causes TT#1857-MD and is no longer needed
                //// Begin TT#4962 - JSmith - Header min not held after size need processed on group
                //foreach (AllocationProfile ap in assrtMent.AssortmentMembers)
                //{
                //    AdjustStoresBelowMinimum(ap);
                //}
                //// End TT#4962 - JSmith - Header min not held after size need processed on group
                // End TT#1857-MD - JSmith - After Size Need Style Allocation and Size Allocation are not in balance.
                AdjustStoresBelowMinimum(_allocationProfile);  // TT#4976 - JSmith - Group min doesn't hold in size need if a separate header min/max is processed
            }
            // Begin TT#1857-MD - JSmith - After Size Need Style Allocation and Size Allocation are not in balance.
            // TT#4962 fixed during allocation in Size Need.  This causes TT#1857-MD and is no longer needed
            //// Begin TT#4962 - JSmith - Header min not held after size need processed on group
            //else
            //    if (_allocationProfile.AssortmentProfile != null)  // TT#5061 - JSmith - Allocation is wiped out when size need is processed if the style allocation was out of balance
            //{
            //    AdjustStoresBelowMinimum(_allocationProfile);
            //}
            //// End TT#4962 - JSmith - Header min not held after size need processed on group
            // End TT#1857-MD - JSmith - After Size Need Style Allocation and Size Allocation are not in balance.
        }
        // end TT#1018 - MD - Jellis - Group Allocation Over Allocates Detail

        // Begin TT#4962 - JSmith - Header min not held after size need processed on group
        /// <summary>
        /// Sets store values to zero if allocated below the minimum
        /// </summary>
        /// <param name="aAllocationProfile">Header Profile</param>
        private static void AdjustStoresBelowMinimum(AllocationProfile aAllocationProfile)
        {
            int minimum;
            int prevUnitsAllocated;
            int storeInventoryBasis = 0;
            foreach (Index_RID storeIdxRID in aAllocationProfile.AppSessionTransaction.StoreIndexRIDArray())
            {
                minimum = aAllocationProfile.GetStoreMinimum(eAllocationSummaryNode.Total, storeIdxRID, false);
                prevUnitsAllocated = aAllocationProfile.GetStoreQtyAllocated(eAllocationSummaryNode.Total, storeIdxRID);
                // Begin TT#1829-MD - JSmith - Color total zero after Size Need
                storeInventoryBasis = 0; 
                if (aAllocationProfile.isAssortmentProfile
                    && aAllocationProfile.GradeInventoryBasisHnLevelType == eHierarchyLevelType.Undefined)
                {
                    AssortmentProfile assrtMent = aAllocationProfile as AssortmentProfile;
                    foreach (AllocationProfile ap in assrtMent.AssortmentMembers)
                    {
                        if (ap.GradeInventoryBasisHnLevelType != eHierarchyLevelType.Undefined)
                        {
                            storeInventoryBasis += ap.GetStoreInventoryBasis(ap.GradeInventoryBasisHnRID, storeIdxRID, false);
                        }
                    }
                }
                //if (prevUnitsAllocated > 0 &&
                //    prevUnitsAllocated < minimum)
                if (prevUnitsAllocated > 0 &&
                    prevUnitsAllocated < minimum - storeInventoryBasis)
                // End TT#1829-MD - JSmith - Color total zero after Size Need
                {
                    aAllocationProfile.SetStoreQtyAllocated(
                        eAllocationSummaryNode.Total,
                        storeIdxRID,
                        0,
                        eDistributeChange.ToAll,
                        false,
                        false,
                        false);
                }
            }
        }
        // End TT#4962 - JSmith - Header min not held after size need processed on group

        /// <summary>
        /// Allocates bulk units after "sized" pack units have been allocated
        /// </summary>
        /// <param name="aStoreFilterList">Profile list of stores</param>
		// Begin TT#5269 - JSmith - Size Need not balancing to allocated units
		//public static void AllocateBulkAfterPacks(AllocationProfile _allocationProfile, ApplicationSessionTransaction _transaction, ProfileList aStoreFilterList)
        public static void AllocateBulkAfterPacks(AllocationProfile _allocationProfile, ApplicationSessionTransaction _transaction, ProfileList aStoreFilterList, bool aTryToBalance = true)
		// End TT#5269 - JSmith - Size Need not balancing to allocated units
        // end TT#702 Infinite Loop when begin date set
        {
            int unitsToAllocate;
            Index_RID storeIdxRID;
            int unitsRemainToAllocate;
            int unitsAllocated; // TT#1021 - MD - Jellis - Header Status Wrong
            foreach (StoreProfile sp in aStoreFilterList)
            {
                storeIdxRID = _transaction.StoreIndexRID(sp.Key); // TT#702 Infinite loop when begin date set
                unitsToAllocate =
                    _allocationProfile.GetStoreQtyAllocated(eAllocationSummaryNode.DetailType, storeIdxRID) // TT#702 Infinite loop when begin date set
                    - _allocationProfile.GetStoreQtyAllocated(eAllocationSummaryNode.DetailSubType, storeIdxRID); // TT#702 Infinite loop when begin date set
				// Begin TT#5269 - JSmith - Size Need not balancing to allocated units
				//unitsRemainToAllocate =  _allocationProfile.SpreadStoreBulkRuleAllocation(_allocationProfile, storeIdxRID, unitsToAllocate, true); // TT#702 Infinite loop when begin date set  // TT#488 - MD - Jellis - Group Allocation
                unitsRemainToAllocate = _allocationProfile.SpreadStoreBulkRuleAllocation(_allocationProfile, storeIdxRID, unitsToAllocate, aTryToBalance); // TT#702 Infinite loop when begin date set  // TT#488 - MD - Jellis - Group Allocation
				// End TT#5269 - JSmith - Size Need not balancing to allocated units
                // begin TT#1021 - MD - Jellis - Header Status Wrong
                //_allocationProfile.SetStoreQtyAllocated // TT#702 Infinite loop when begin date set
                //     (
                //     eAllocationSummaryNode.Bulk,
                //     storeIdxRID,
                //     unitsToAllocate - unitsRemainToAllocate + _allocationProfile.GetStoreQtyAllocated(eAllocationSummaryNode.Bulk, storeIdxRID), // TT#702 Infinite loop when begin date set
                //     eDistributeChange.ToNone,
                //     false
                //     );
                unitsAllocated = unitsToAllocate - unitsRemainToAllocate;
                if (unitsAllocated > 0)
                {
                    _allocationProfile.SetStoreQtyAllocated
                        (
                        eAllocationSummaryNode.Bulk,
                        storeIdxRID,
                        unitsAllocated
                        + _allocationProfile.GetStoreQtyAllocated(eAllocationSummaryNode.Bulk, storeIdxRID),
                        eDistributeChange.ToNone,
                        false
                        );
                    _allocationProfile.SetStoreAllocationFromPackContentBreakOut(eAllocationSummaryNode.Total, storeIdxRID, true);
                    _allocationProfile.SetStoreAllocationFromPackContentBreakOut(eAllocationSummaryNode.DetailType, storeIdxRID, true);
                    _allocationProfile.SetStoreAllocationFromPackContentBreakOut(eAllocationSummaryNode.Bulk, storeIdxRID, true);
                }
                // TT#1021 - MD - Jellis - Header Status Wrong
            }
        }
        // END MID Track 3122 Bulk not allocated when bulk and sized packs on same header
    }
}
