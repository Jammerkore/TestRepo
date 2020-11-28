using System;
using System.Collections;
using System.Globalization;
using System.Diagnostics;

using MIDRetail.Business;
using MIDRetail.Data;
using MIDRetail.DataCommon;
using MIDRetail.Common;

namespace MIDRetail.Business.Allocation
{
	/// <summary>
	/// Summary description for SizeRuleAlgorithm.
	/// </summary>
	public class SizeRuleAlgorithm   // MID Track 3619 Remove Fringe
	{
		private ProfileList _sizeList;
		// private Hashtable _sizeRemainingHash;       // MID Track 3619 Remove Fringe
		// private eFringeOverrideSort _sortDirection; // MID Track 3619 Remove Fringe
		private int _colorRid;
		private AllocationProfile _allocProfile;
		private CollectionDecoder _constraintDecoder;
		private	CollectionDecoder _ruleDecoder;
        //private SizeNeedResults _sizeNeedResults; // TT#1068 - MD - Jellis - Pack allocations not considered in decisions for bulk size allocations
		private MIDHashtable _colorStoreRemainingHash;
        private MIDHashtable _initialColorStoreRemainingHash;  // TT#1858-MD - JSmith - 2 hdrs same st/color after size need- style allocation is on 1 header and the size allocation is on the other.  Expect the style and size allocation to be on the same header.
		private Hashtable _storeGroupLevelHash;

		private ProfileList _storeList;
		private ArrayList _storeSizeRuleList;   // MID Track 3619 Remove Fringe 
        private Hashtable _sizeAlternateHash;  // TT#1837-MD - JSmith - Fill Size with Size Min Rule and Size Alternate not looking at correct OH + IT when allocating
       

		public SizeRuleAlgorithm(ProfileList sizeList,  // MID Track 3619 Remove Fringe 
			// Hashtable sizeRemainingHash,        // MID Track 3619 Remove Fringe 
			// eFringeOverrideSort sortDirection,  // MID Track 3619 Remove Fringe
			ProfileList storeList,
			int colorRid,
			AllocationProfile allocProfile,
			CollectionDecoder constraintDecoder,
			CollectionDecoder ruleDecoder,
			Hashtable storeGroupLevelHash,
			SizeNeedResults sizeNeedResults,
            Hashtable sizeAlternateHash)  // TT#1837-MD - JSmith - Fill Size with Size Min Rule and Size Alternate not looking at correct OH + IT when allocating
		{
			_sizeList = sizeList;
			// _sizeRemainingHash = sizeRemainingHash; // MID Track 3619 Remove Fringe
			// _sortDirection = sortDirection;         // MID Track 3619 Remove Fringe
			_storeList = storeList;
			_colorRid = colorRid;
			_allocProfile = allocProfile;
			_constraintDecoder = constraintDecoder;
			_ruleDecoder = ruleDecoder;
			_storeGroupLevelHash = storeGroupLevelHash;
            //_sizeNeedResults = sizeNeedResults; // TT#1068 - MD - Jellis - Pack allocations not considered in decisions for bulk size allocations
            _sizeAlternateHash = sizeAlternateHash;  // TT#1837-MD - JSmith - Fill Size with Size Min Rule and Size Alternate not looking at correct OH + IT when allocating
		}

        // begin TT#1068 - MD - Jellis - Pack allocations not considered in decisions for bulk size allocations
        private SizeNeedResults SizeResults
        {
            get
            {
                return _allocProfile.AppSessionTransaction.GetSizeNeedResults(_allocProfile.HeaderRID, _colorRid);
            }
        }
        // end TT#1068 - MD - Jellis - Pack allocations not considered in decisions for bulk size allocations

		/// <summary>
		/// Main Size Rule processing
		/// </summary>
		public void Process()
		{
			try
			{
				// SortSizes();  // MID Track 3619 Remove Fringe
		
				BuildColorStoreRemaining(); // MID Track 3620 Change Rule to Upto type rule

				foreach (SizeCodeProfile sizeCode in _sizeList)
				{
					// build size rule class foreach store
					_storeSizeRuleList = new ArrayList();  // MID Track 3619 Remove Fringe
					for (int s=0; s<_storeList.Count; s++)
					{
						StoreProfile sp = (StoreProfile)_storeList[s];
						if (_allocProfile.GetStoreIsEligible(sp.Key)        // only eligible stores that are not out
                            && _allocProfile.GetIncludeStoreInAllocation(sp.Key) // TT#1401 - JEllis - Urban Reservation Store pt 11
							&& !_allocProfile.GetStoreOut(_colorRid, sp.Key)) // only eligible stores that are not out
						{
							_storeSizeRuleList.Add(new SizeRuleStore(sp.Key));  // MID Track 3619 Remove Fringe
						}
					}
				
					// calc and add need to size rule class for each store
					//FillStoreFringe(sizeCode.Key); // MID Track 3492 Size Need with constraints not allocating correctly
					// begin MID Track 3781 Size Curve not required
					if (_storeSizeRuleList.Count > 0)
					{
						FillStoreSizeRule(sizeCode.Key); // MID Track 3619 Remove Fringe // MID Track 3492 Size Need with constraints not allocating correctly // TT#1391 - TMW New Action
						SortStores();
						ProcessStores(sizeCode.Key);
					}
					// end MID Track 3781 Size Curve not required
				}
			}
			catch
			{
				throw;
			}
		}


		private void BuildColorStoreRemaining()  // MID Track 3620 Change Rules to Upto type rules
		{
			GeneralComponent allSizeComponent = new GeneralComponent(eGeneralComponentType.AllSizes);
			AllocationColorOrSizeComponent colorComponent = new AllocationColorOrSizeComponent(eSpecificBulkType.SpecificColor, _colorRid); // MID Track 3620 Change Rules to Upto type rules
			AllocationColorSizeComponent colorSizeComponent = new AllocationColorSizeComponent(colorComponent, allSizeComponent); // MID Track 3620 Change Rules to Upto type rules
			_colorStoreRemainingHash = new MIDHashtable();
            _initialColorStoreRemainingHash = new MIDHashtable();  // TT#1858-MD - JSmith - 2 hdrs same st/color after size need- style allocation is on 1 header and the size allocation is on the other.  Expect the style and size allocation to be on the same header.
			for (int s=0; s<_storeList.Count; s++)
			{
				StoreProfile sp = (StoreProfile)_storeList[s];
				//=========================================
				// get Color units remaining for the store
				//=========================================
				int colorReceipts = _allocProfile.GetStoreQtyAllocated(_colorRid, sp.Key);
				//int allocatedToColor = _allocProfile.GetStoreQtyAllocated(allSizeComponent, sp.Key); // MID Track 3620 Change Rules to Upto type rules
				int allocatedToColor = _allocProfile.GetStoreQtyAllocated(colorSizeComponent, sp.Key); // MID Track 3620 Change Rules to Upto type rules
				int colorStoreRemaining = colorReceipts - allocatedToColor;
				if (colorStoreRemaining < 0) 
				{
					colorStoreRemaining = 0;
				}
				_colorStoreRemainingHash.Add(sp.Key, colorStoreRemaining);
                _initialColorStoreRemainingHash.Add(sp.Key, colorStoreRemaining);  // TT#1858-MD - JSmith - 2 hdrs same st/color after size need- style allocation is on 1 header and the size allocation is on the other.  Expect the style and size allocation to be on the same header.
			}
		}

		/// <summary>
		/// Processes Size Rule for each store
		/// </summary>
		private void ProcessStores(int sizeRid)
		{
			//int colorSizeRemaining = 	(int)_sizeRemainingHash[sizeRid]; // MID Track 3620 Change Rule to Upto type and allow rule to overallocate allocation
            int colorSizeRemaining = 0; // MID Track 3620 Change Rule to Upto type and allow rule to overallocate allocation
            //HdrColorBin hcb = _allocProfile.GetHdrColorBin(_sizeNeedResults.HeaderColorRid);  // TT#246 - MD - Jellis - AnF VSW In Store Minimum pt 5 // TT#1068 - MD - Jellis - Pack allocations not considered in decisions for bulk size allocations
            HdrColorBin hcb = _allocProfile.GetHdrColorBin(_colorRid);                        // TT#1068 - MD - Jellis - Pack allocations not considered in decisions for bulk size allocations
            HdrSizeBin hsb = (HdrSizeBin)hcb.ColorSizes[sizeRid];                             // TT#246 - MD - Jellis - AnF VSW In Store Minimum pt 5
            // begin TT#4150 - MD - Jellis - Re_Do TT#4121 Fix_To Remove TT#1068 Conflict (Code moved to SizeNeedAlgorithm)
			//// Begin TT#4121 - JSmith - Size Need Method-> w Constraints and size curve-> Severe Null Reference error-Header with detailed packs and bulk
            //_sizeNeedResults.Color = hcb;
            //// End TT#4121 - JSmith - Size Need Method-> w Constraints and size curve-> Severe Null Reference error-Header with detailed packs and bulk
            // end TT#4150 - MD - Jellis - Re_Do TT#4121 Fix_To Remove TT#1068 Conflict (Code moved to SizeNeedAlgorithm)
			SizeNeedResults sizeResults = SizeResults;  // TT#1068 - MD - Jellis - Pack allocations not considered in decisions for bulk size allocations

            if (hsb != null  // TT#3099 - MD - Jellis - Size Review Variable totals not correct
            // Begin TT#1487-MD - stodd - ASST - Fill Size Holes does not work on a PH.
                // && (_allocProfile.WorkUpBulkSizeBuy || hsb.SizeUnitsToAllocate > 0 )) // TT#2962 - AnF - Jellis - Size Error - Size not on Color // TT#3099 - MD - JEllis - Size Review Variable Totals not correct
                 && (_allocProfile.WorkUpBulkSizeBuy || hsb.SizeUnitsToAllocate > 0 || _allocProfile.Placeholder))
            // End TT#1487-MD - stodd - ASST - Fill Size Holes does not work on a PH.
            {                 // TT#2962 - AnF - Jellis - Size Error - Size not on Color
                for (int s = 0; s < _storeSizeRuleList.Count; s++) // MID Track 3619 Remove Fringe
                {
                    SizeRuleStore aStore = (SizeRuleStore)_storeSizeRuleList[s];  // MID Track 3619 Remove Fringe
                    int ColorStoreRemaining = (int)_colorStoreRemainingHash[aStore.StoreRid];
                    int InitialColorStoreRemaining = (int)_initialColorStoreRemainingHash[aStore.StoreRid];  // TT#1858-MD - JSmith - 2 hdrs same st/color after size need- style allocation is on 1 header and the size allocation is on the other.  Expect the style and size allocation to be on the same header.

                    // Begin MID Issue 3212 stodd
                    // Removed this code to stop processing.
                    // any of the stores might have a rule quantity and we want to 
                    // be sure to go ahead and process it, even if there are no
                    // units remaining.
                    //				//==========================================
                    //				// No more units to allocate for this color
                    //				//==========================================
                    //				if (colorSizeRemaining < 1)
                    //					break;
                    //
                    //				//==========================================
                    //				// No more units to allocate for this store
                    //				//==========================================
                    //				if (ColorStoreRemaining < 1)
                    //					continue;
                    // End MID Issue 3212 stodd

                    // Begin TT#1827-MD - JSmith - Run GA method, Need and Size Need with Size constraint and Minimum Rule.  The Size Constraint is not being honored.
                    // Begin TT#5094 - JSmith - GA->Size Need with Constraints
                    //if (ColorStoreRemaining < 1 &&
                    //    (_allocProfile.AssortmentProfile != null
                    //     && _allocProfile.AssortmentProfile.AsrtType == (int)eAssortmentType.GroupAllocation) && 
                    //    (aStore.Rule == eSizeRuleType.SizeMinimum ||
                    //    aStore.Rule == eSizeRuleType.SizeMinimumPlus1 ||
                    //    aStore.Rule == eSizeRuleType.SizeMaximum ||
                    //    aStore.Rule == eSizeRuleType.AbsoluteQuantity)
                    //    )
                    //{
                    //    continue;
                    //}
                    // End TT#5094 - JSmith - GA->Size Need with Constraints
                    // End TT#1827-MD - JSmith - Run GA method, Need and Size Need with Size constraint and Minimum Rule.  The Size Constraint is not being honored.

                    // Begin TT#1858-MD - JSmith - 2 hdrs same st/color after size need- style allocation is on 1 header and the size allocation is on the other.  Expect the style and size allocation to be on the same header.
                    if (InitialColorStoreRemaining < 1 &&
                        (_allocProfile.AssortmentProfile != null
                         && _allocProfile.AssortmentProfile.AsrtType == (int)eAssortmentType.GroupAllocation) &&
                        (aStore.Rule == eSizeRuleType.SizeMinimum ||
                        aStore.Rule == eSizeRuleType.SizeMinimumPlus1 ||
                        aStore.Rule == eSizeRuleType.SizeMaximum ||
                        aStore.Rule == eSizeRuleType.AbsoluteQuantity)
                        )
                    {
                        continue;
                    }
                    // End TT#1858-MD - JSmith - 2 hdrs same st/color after size need- style allocation is on 1 header and the size allocation is on the other.  Expect the style and size allocation to be on the same header.

                    int ruleQty; // MID Track 3620 Change Rule to Upto Type rule
                    int onhandQty; // MID Track 3620 Change Rule to Upto Type rule
                    switch (aStore.Rule)
                    {
                        case (eSizeRuleType.AbsoluteQuantity):  // MID Track 3619 Remove Fringe
                            colorSizeRemaining = CheckAndApplyQuantity(aStore.RuleQuantity, aStore, colorSizeRemaining, aStore.Rule);
                            break;
                        case (eSizeRuleType.Exclude):           // MID Track 3619 Remove Fringe
                            break;
                        case (eSizeRuleType.SizeMaximum):       // MID Track 3619 Remove Fringe
                            // begin MID Track 3620 Change Rules to Upto Type Rule
                            {
                                //colorSizeRemaining = CheckAndApplyQuantity(aStore.Maximum, aStore, colorSizeRemaining, aStore.Rule); 
                                //onhandQty = _sizeNeedResults.GetOnhandUnits(aStore.StoreRid, sizeRid);  // TT#317 - MD - Jellis - Size Inventory Min max gets incorrect result for Size Need
                                //onhandQty = _sizeNeedResults.GetIbOnhandUnits(aStore.StoreRid, sizeRid);  // TT#317 - MD - Jellis - Size Inventory Min max gets incorrect result for Size Need // TT#1068 - MD - Jellis - Pack allocations not considered in decisions for bulk size allocations
                                onhandQty = sizeResults.GetIbOnhandUnits(aStore.StoreRid, sizeRid); // // TT#1068 TT#1068 - MD - Jellis - Pack ALlocation not used for Bulk Size
                                if (onhandQty < 0)
                                {
                                    onhandQty = 0;
                                }
                                if (aStore.Maximum < int.MaxValue)                       // MID Track 3620 Size Rules are up to rules 
                                {                                                        // MID Track 3620 Size Rules are up to rules
                                    // begin TT#1600 - JEllis - Size Need Algorithm Error
                                    //ruleQty = 
                                    //    aStore.Maximum 
                                    //    - _sizeNeedResults.GetPriorAllocatedUnits(aStore.StoreRid, sizeRid)
                                    //    - onhandQty
                                    //    - _sizeNeedResults.GetIntransitUnits(aStore.StoreRid, sizeRid);
                                    // begin TT#317 - MD - JEllis - Size Inventory Min max gets incorrect result for Size Need
                                    //ruleQty =
                                    //        aStore.Maximum
                                    //        - _sizeNeedResults.GetAllocatedUnits(aStore.StoreRid, sizeRid)
                                    //        - onhandQty
                                    //        - _sizeNeedResults.GetVswOnhandUnits(aStore.StoreRid, sizeRid) // TT#2413 - JEllis - AnF VSW - Size Need Allocation not using VSW OnHand
                                    //        - _sizeNeedResults.GetIntransitUnits(aStore.StoreRid, sizeRid);
                                    // end TT#1600 - JEllis - Size Need Algorithm Error
                                    // begin TT#1068 - MD - Jellis - Pack allocations not considered in decisions for bulk size allocations
                                    //  ruleQty =
                                    //        aStore.Maximum
                                    //        - _sizeNeedResults.GetAllocatedUnits(aStore.StoreRid, sizeRid)
                                    //        - onhandQty
                                    //- Math.Max(_sizeNeedResults.GetIbVswOnhandUnits(aStore.StoreRid, sizeRid), 0)  // TT#3369 - TMW - Jellis - Size Prop with constraint not observing constraint
                                    //- Math.Max(_sizeNeedResults.GetIbIntransitUnits(aStore.StoreRid, sizeRid), 0); // TT#3369 - TMW - Jellis - Size Prop with constraint not observing constraint
                                    ruleQty =
                                          aStore.Maximum
                                          - sizeResults.GetAllocatedUnits(aStore.StoreRid, sizeRid)
                                          - onhandQty
                                          - Math.Max(sizeResults.GetIbVswOnhandUnits(aStore.StoreRid, sizeRid), 0)  // TT#3369 - TMW - Jellis - Size Prop with constraint not observing constraint
                                          - Math.Max(sizeResults.GetIbIntransitUnits(aStore.StoreRid, sizeRid), 0);
                                    // end  TT#1068 - MD - Jellis - Pack allocations not considered in decisions for bulk size allocations
                                    // end TT#317  - MD - JEllis - Size Inventory Min max gets incorrect result for Size Need
                                    // TT#1837-MD - JSmith - Fill Size with Size Min Rule and Size Alternate not looking at correct OH + IT when allocating
                                    //ruleQty = sizeResults.GetStoreInventoryMin(aStore.StoreRid, sizeRid, true);         // TT#1068 - MD - Jellis - Pack allocations not considered in decisions for bulk size allocations // TT#1176 - MD - Jellis - Size Need not observing inv min max on group
                                    ArrayList alSizeAlternates = null;
                                    if (_sizeAlternateHash != null)
                                    {
                                        alSizeAlternates = (ArrayList)_sizeAlternateHash[sizeRid];
                                    }
                                    if (alSizeAlternates != null)
                                    {
                                        foreach (SizeCodeProfile scp in alSizeAlternates)
                                        {
                                            if (scp.Key != sizeRid)
                                            {
                                                ruleQty = ruleQty
                                                        - Math.Max(sizeResults.GetIbOnhandUnits(aStore.StoreRid, scp.Key), 0)
                                                        - Math.Max(sizeResults.GetIbVswOnhandUnits(aStore.StoreRid, scp.Key), 0) 
                                                        - Math.Max(sizeResults.GetIbIntransitUnits(aStore.StoreRid, scp.Key), 0);
                                            }
                                        }
                                    }
                                    // TT#1837-MD - JSmith - Fill Size with Size Min Rule and Size Alternate not looking at correct OH + IT when allocating
                                    if (ruleQty > 0)
                                    {
                                        CheckAndApplyQuantity(ruleQty, aStore, colorSizeRemaining, aStore.Rule);
                                    }
                                }                                                       // MID Track 3620 Size Rules are up to rules
                                break;
                            }
                        // end MID Track 3620 Change Rules to Upto Type Rule
                        case (eSizeRuleType.SizeMinimum):    // MID Track 3619 Remove Fringe
                            // begin MID Track 3620 Change Rules to Upto Type Rule
                            {
                                // begin TT#519 - MD - Jellis - VSW - Minimums not working
                                ////colorSizeRemaining = CheckAndApplyQuantity(aStore.Minimum, aStore, colorSizeRemaining, aStore.Rule);
                                ////onhandQty = _sizeNeedResults.GetOnhandUnits(aStore.StoreRid, sizeRid);  // TT#317 - MD - Jellis - Size Inventory Min max gets incorrect result for Size Need
                                //onhandQty = _sizeNeedResults.GetIbOnhandUnits(aStore.StoreRid, sizeRid);  // TT#317 - MD - Jellis - Size Inventory Min max gets incorrect result for Size Need
                                //if (onhandQty < 0)
                                //{
                                //    onhandQty = 0;
                                //}
                                //// begin TT#1600 - JEllis - Size Need Algorithm Error
                                ////ruleQty = 
                                ////    aStore.Minimum 
                                ////    - _sizeNeedResults.GetPriorAllocatedUnits(aStore.StoreRid, sizeRid)
                                ////    - onhandQty
                                ////    - _sizeNeedResults.GetIntransitUnits(aStore.StoreRid, sizeRid);
                                //// begin TT#317 - MD - JEllis - Size Inventory Min max gets incorrect result for Size Need
                                ////ruleQty =
                                ////    aStore.Minimum
                                ////    - _sizeNeedResults.GetAllocatedUnits(aStore.StoreRid, sizeRid)
                                ////    - onhandQty
                                ////    - _sizeNeedResults.GetVswOnhandUnits(aStore.StoreRid, sizeRid) // TT#2413 - JEllis - AnF VSW - Size Need Allocation not using VSW OnHand
                                ////    - _sizeNeedResults.GetIntransitUnits(aStore.StoreRid, sizeRid);
                                //// end TT1600 - JEllis - Size Need Algorithm Error
                                //// begin TT#246 - MD - Jellis - AnF VSW In Store Minimums
                                ////ruleQty =
                                ////   aStore.Minimum
                                ////   - _sizeNeedResults.GetAllocatedUnits(aStore.StoreRid, sizeRid)
                                ////   - onhandQty
                                ////   - _sizeNeedResults.GetIbVswOnhandUnits(aStore.StoreRid, sizeRid) 
                                ////   - _sizeNeedResults.GetIbIntransitUnits(aStore.StoreRid, sizeRid);
                                //// end TT#317  - MD - JEllis - Size Inventory Min max gets incorrect result for Size Need
                                //if (hsb != null)
                                //{
                                //    switch (_sizeNeedResults.VSWSizeConstraints)
                                //    {
                                //        case (eVSWSizeConstraints.CombinedIdealSizeMinimum):
                                //            {
                                //                ruleQty =
                                //                  aStore.Minimum
                                //                  - _sizeNeedResults.GetIbVswOnhandUnits(aStore.StoreRid, sizeRid);
                                //                ruleQty =
                                //                    Math.Max(ruleQty, _allocProfile.GetStoreItemIdealMinimum(hsb, aStore.StoreRid));
                                //                break;
                                //            }
                                //        case (eVSWSizeConstraints.ItemMaxIdealSize):
                                //            {
                                //                ruleQty = _allocProfile.GetStoreItemIdealMinimum(hsb, aStore.StoreRid);
                                //                break;
                                //            }
                                //        case (eVSWSizeConstraints.InStoreSizeMinimum):
                                //            {
                                //                ruleQty =
                                //                    aStore.Minimum
                                //                    - _sizeNeedResults.GetIbVswOnhandUnits(aStore.StoreRid, sizeRid);
                                //                break;
                                //            }
                                //        default:
                                //            {
                                //                ruleQty =
                                //                    aStore.Minimum
                                //                    - _sizeNeedResults.GetAllocatedUnits(aStore.StoreRid, sizeRid)
                                //                    - onhandQty
                                //                    - _sizeNeedResults.GetIbVswOnhandUnits(aStore.StoreRid, sizeRid)
                                //                    - _sizeNeedResults.GetIbIntransitUnits(aStore.StoreRid, sizeRid);
                                //                break;
                                //            }
                                //    }
                                //}
                                //else
                                //{
                                //    ruleQty =
                                //        aStore.Minimum
                                //        - _sizeNeedResults.GetAllocatedUnits(aStore.StoreRid, sizeRid)
                                //        - onhandQty
                                //        - _sizeNeedResults.GetIbVswOnhandUnits(aStore.StoreRid, sizeRid)
                                //        - _sizeNeedResults.GetIbIntransitUnits(aStore.StoreRid, sizeRid);
                                //}
                                //// end TT#246 - MD - JEllis - AnF VSW In Store Minimums
                                if (hsb != null)
                                {
                                    //if (_sizeNeedResults.VSWSizeConstraints == eVSWSizeConstraints.None // TT#1068 - MD - Jellis - Pack allocations not considered in decisions for bulk size allocations
                                    if (sizeResults.VSWSizeConstraints == eVSWSizeConstraints.None //TT#1068 - MD - Jellis - Pack allocations not considered in decisions for bulk size allocations
                                        || _allocProfile.GetStoreImoMaxValue(aStore.StoreRid) == int.MaxValue)  // TT#692 - MD - Jellis - Non-VSW stores not getting Minimum allocation.
                                    {
                                        //ruleQty = _sizeNeedResults.GetStoreInventoryMin(aStore.StoreRid, sizeRid);  // TT#1068 - MD - Jellis - Pack allocations not considered in decisions for bulk size allocations
										// TT#1837-MD - JSmith - Fill Size with Size Min Rule and Size Alternate not looking at correct OH + IT when allocating
                                        //ruleQty = sizeResults.GetStoreInventoryMin(aStore.StoreRid, sizeRid, true);         // TT#1068 - MD - Jellis - Pack allocations not considered in decisions for bulk size allocations // TT#1176 - MD - Jellis - Size Need not observing inv min max on group
                                        ArrayList alSizeAlternates = null;
                                        if (_sizeAlternateHash != null)
                                        {
                                            alSizeAlternates = (ArrayList)_sizeAlternateHash[sizeRid];
                                        }
                                        ruleQty = sizeResults.GetStoreInventoryMin(aStore.StoreRid, sizeRid, true, alSizeAlternates);
										// TT#1837-MD - JSmith - Fill Size with Size Min Rule and Size Alternate not looking at correct OH + IT when allocating
                                    }
                                    else
                                    {
                                        ruleQty = _allocProfile.GetStoreItemMinimum(hsb, _allocProfile.AppSessionTransaction.StoreIndexRID(aStore.StoreRid));
                                    }
                                }
                                else
                                {
                                    //ruleQty = _sizeNeedResults.GetStoreInventoryMin(aStore.StoreRid, sizeRid); // TT#1068 - MD - Jellis - Pack allocations not considered in decisions for bulk size allocations
									// TT#1837-MD - JSmith - Fill Size with Size Min Rule and Size Alternate not looking at correct OH + IT when allocating
                                    //ruleQty = sizeResults.GetStoreInventoryMin(aStore.StoreRid, sizeRid, true); // TT#1068 - MD - Jellis - Pack allocations not considered in decisions for bulk size allocations // TT#1176 - MD - Jellis - Size Need not observing inv min max on group
                                    ArrayList alSizeAlternates = null;
                                    if (_sizeAlternateHash != null)
                                    {
                                        alSizeAlternates = (ArrayList)_sizeAlternateHash[sizeRid];
                                    }
                                    ruleQty = sizeResults.GetStoreInventoryMin(aStore.StoreRid, sizeRid, true, alSizeAlternates);
									// TT#1837-MD - JSmith - Fill Size with Size Min Rule and Size Alternate not looking at correct OH + IT when allocating
                                }
                                // end TT#519 - MD - Jellis - VSW - Minimums not working
                                // begin TT#3101 - MD - Jellis -  Process Fill Size doubles Size Minimum Rules
                                //ruleQty -= _sizeNeedResults.GetAllocatedUnits(aStore.StoreRid, sizeRid); // TT#1068 - MD - Jellis - Pack allocations not considered in decisions for bulk size allocations
                                ruleQty -= sizeResults.GetAllocatedUnits(aStore.StoreRid, sizeRid); // TT#1068 - MD - Jellis - Pack allocations not considered in decisions for bulk size allocations
                                // end TT#3101 - MD - Jellis - Process Fill Size doubles Size Minimum Rules
                                if (ruleQty > 0)
                                {
                                    CheckAndApplyQuantity(ruleQty, aStore, colorSizeRemaining, aStore.Rule);
                                }
                                break;
                            }
                        // end MID Track 3620 Change Rules to Upto Type Rule
                        case (eSizeRuleType.SizeMinimumPlus1):  // MID Track 3619 Remove Fringe
                            // begin MID Track 3620 Change Rules to Upto Type Rule
                            {
                                // begin TT#519 - MD - Jellis - AnF VSW In Store Minimums
                                ////int priorUnits = _sizeNeedResults.GetPriorAllocatedUnits(aStore.StoreRid, sizeRid);
                                ////if (priorUnits > aStore.Minimum) 			
                                ////	colorSizeRemaining = CheckAndApplyQuantity(1, aStore, colorSizeRemaining, aStore.Rule);
                                ////else
                                ////	colorSizeRemaining = CheckAndApplyQuantity(aStore.Minimum, aStore, colorSizeRemaining, aStore.Rule);
                                ////onhandQty = _sizeNeedResults.GetOnhandUnits(aStore.StoreRid, sizeRid);  // TT#317 - MD - Jellis - Size Inventory Min max gets incorrect result for Size Need
                                //onhandQty = _sizeNeedResults.GetIbOnhandUnits(aStore.StoreRid, sizeRid);  // TT#317 - MD - Jellis - Size Inventory Min max gets incorrect result for Size Need
                                //if (onhandQty < 0)
                                //{
                                //    onhandQty = 0;
                                //}
                                //// begin TT#1600 - JEllis - Size Need Algorithm Error
                                ////ruleQty = 
                                ////    aStore.Minimum 
                                ////    - _sizeNeedResults.GetPriorAllocatedUnits(aStore.StoreRid, sizeRid)
                                ////    - onhandQty
                                ////    - _sizeNeedResults.GetIntransitUnits(aStore.StoreRid, sizeRid);
                                //// begin TT#317 - MD - JEllis - Size Inventory Min max gets incorrect result for Size Need
                                ////ruleQty =
                                ////    aStore.Minimum
                                ////    - _sizeNeedResults.GetAllocatedUnits(aStore.StoreRid, sizeRid)
                                ////    - onhandQty
                                ////    - _sizeNeedResults.GetVswOnhandUnits(aStore.StoreRid, sizeRid) // TT#2413 - JEllis - AnF VSW - Size Need Allocation not using VSW OnHand
                                ////    - _sizeNeedResults.GetIntransitUnits(aStore.StoreRid, sizeRid); 
                                //// end TT#1600 - JEllis - Size Need Algorithm Error
                                //// begin TT#246 - MD - Jellis - AnF VSW In Store Minimums
                                ////ruleQty =
                                ////    aStore.Minimum
                                ////    - _sizeNeedResults.GetAllocatedUnits(aStore.StoreRid, sizeRid)
                                ////    - onhandQty
                                ////    - _sizeNeedResults.GetIbVswOnhandUnits(aStore.StoreRid, sizeRid)
                                ////    - _sizeNeedResults.GetIbIntransitUnits(aStore.StoreRid, sizeRid); 
                                //// end TT#317  - MD - JEllis - Size Inventory Min max gets incorrect result for Size Need
                                ////if (ruleQty > 0)
                                ////{
                                ////    CheckAndApplyQuantity(ruleQty, aStore, colorSizeRemaining, aStore.Rule);
                                ////}
                                ////else
                                ////{
                                ////    CheckAndApplyQuantity(Math.Max(aStore.Mult, 1), aStore, colorSizeRemaining, aStore.Rule);
                                ////} 
                                //if (hsb != null)
                                //{
                                //    switch (_sizeNeedResults.VSWSizeConstraints)
                                //    {
                                //        case (eVSWSizeConstraints.CombinedIdealSizeMinimum):
                                //            {
                                //                ruleQty =
                                //                  aStore.Minimum
                                //                  - _sizeNeedResults.GetIbVswOnhandUnits(aStore.StoreRid, sizeRid);
                                //                if (ruleQty < 1)
                                //                {
                                //                    ruleQty = Math.Max(aStore.Mult, 1);
                                //                }
                                //                ruleQty =
                                //                    Math.Max(ruleQty, _allocProfile.GetStoreItemIdealMinimum(hsb, aStore.StoreRid));
                                //                break;
                                //            }
                                //        case (eVSWSizeConstraints.ItemMaxIdealSize):
                                //            {
                                //                ruleQty = _allocProfile.GetStoreItemIdealMinimum(hsb, aStore.StoreRid);
                                //                break;
                                //            }
                                //        case (eVSWSizeConstraints.InStoreSizeMinimum):
                                //            {
                                //                ruleQty =
                                //                    aStore.Minimum
                                //                    - _sizeNeedResults.GetIbVswOnhandUnits(aStore.StoreRid, sizeRid);
                                //                if (ruleQty < 1)
                                //                {
                                //                    ruleQty = Math.Max(aStore.Mult, 1);
                                //                }
                                //                break;
                                //            }
                                //        default:
                                //            {
                                //                ruleQty =
                                //                    aStore.Minimum
                                //                    - _sizeNeedResults.GetAllocatedUnits(aStore.StoreRid, sizeRid)
                                //                    - onhandQty
                                //                    - _sizeNeedResults.GetIbVswOnhandUnits(aStore.StoreRid, sizeRid)
                                //                    - _sizeNeedResults.GetIbIntransitUnits(aStore.StoreRid, sizeRid);
                                //                if (ruleQty < 1)
                                //                {
                                //                    ruleQty = Math.Max(aStore.Mult, 1);
                                //                }
                                //                break;
                                //            }
                                //    }
                                //}
                                //else
                                //{
                                //    ruleQty =
                                //        aStore.Minimum
                                //        - _sizeNeedResults.GetAllocatedUnits(aStore.StoreRid, sizeRid)
                                //        - onhandQty
                                //        - _sizeNeedResults.GetIbVswOnhandUnits(aStore.StoreRid, sizeRid)
                                //        - _sizeNeedResults.GetIbIntransitUnits(aStore.StoreRid, sizeRid);
                                //    if (ruleQty < 1)
                                //    {
                                //        ruleQty = Math.Max(aStore.Mult, 1);
                                //    }
                                //}
                                if (hsb != null)
                                {
                                    //if (_sizeNeedResults.VSWSizeConstraints == eVSWSizeConstraints.None         // TT#693 - MD - Jellis - VSW stores not holding Minimum allocation on Balance with constraints. // TT#1068 - MD - Jellis - Pack allocations not considered in decisions for bulk size allocations
                                    if (sizeResults.VSWSizeConstraints == eVSWSizeConstraints.None         // TT#693 - MD - Jellis - VSW stores not holding Minimum allocation on Balance with constraints. // TT#1068 - MD - Jellis - Pack allocations not considered in decisions for bulk size allocations
                                        || _allocProfile.GetStoreImoMaxValue(aStore.StoreRid) == int.MaxValue)  // TT#693 - MD - Jellis - VSW stores not holding Minimum allocation on Balance with constraints.
                                    {
                                        //ruleQty = _sizeNeedResults.GetStoreInventoryMin(aStore.StoreRid, sizeRid); // TT#1068 - MD - Jellis - Pack allocations not considered in decisions for bulk size allocations
                                        ruleQty = sizeResults.GetStoreInventoryMin(aStore.StoreRid, sizeRid, true, (ArrayList)_sizeAlternateHash[sizeRid]); // TT#1068 - MD - Jellis - Pack allocations not considered in decisions for bulk size allocations // TT#1176 - MD - Jellis - Size Need not observing inv min max on group
                                    }
                                    else
                                    {
                                        ruleQty = _allocProfile.GetStoreItemMinimum(hsb, _allocProfile.AppSessionTransaction.StoreIndexRID(aStore.StoreRid));
                                    }
                                }
                                else
                                {
                                    //ruleQty = _sizeNeedResults.GetStoreInventoryMin(aStore.StoreRid, sizeRid); // TT#1068 - MD - Jellis - Pack allocations not considered in decisions for bulk size allocations
                                    ruleQty = sizeResults.GetStoreInventoryMin(aStore.StoreRid, sizeRid, true, (ArrayList)_sizeAlternateHash[sizeRid]); // TT#1068 - MD - Jellis - Pack allocations not considered in decisions for bulk size allocations // TT#1176 - MD - Jellis - Size Need not observing inv min max on group
                                }
                                // begin TT#3101 - MD - Jellis -  Process Fill Size doubles Size Minimum Rules
                                //ruleQty -= _sizeNeedResults.GetAllocatedUnits(aStore.StoreRid, sizeRid); // TT#1068 - MD - Jellis - Pack allocations not considered in decisions for bulk size allocations
                                ruleQty -= sizeResults.GetAllocatedUnits(aStore.StoreRid, sizeRid); // TT#1068 - MD - Jellis - Pack allocations not considered in decisions for bulk size allocations
                                // end TT#3101 - MD - Jellis - Process Fill Size doubles Size Minimum Rules
                                if (ruleQty < 1)
                                {
                                    ruleQty = Math.Max(aStore.Mult, 1);
                                }
                                // end TT#519 - MD - Jellis - AnF VSW Minimums not working
                                CheckAndApplyQuantity(ruleQty, aStore, colorSizeRemaining, aStore.Rule);
                                // end TT#246 - MD - JEllis - AnF VSW In Store Minimums

                                break;
                            }
                        // end MID Track 3620 Change Rules to Upto Type Rule
                        default:
                            break;
                    }

                    sizeResults.AddAllocatedUnits(aStore.StoreRid, sizeRid, aStore.UnitsAllocated); // TT#1068 - MD - Jellis - Pack allocations not considered in decisions for bulk size allocations
                    //_sizeNeedResults.AddAllocatedUnits(aStore.StoreRid, sizeRid, aStore.UnitsAllocated); // TT#1068 - MD - Jellis - Pack allocations not considered in decisions for bulk size allocations
                    // begin TT#1600 - JEllis - Size Need Algorithm Error (Note: no reason to keep "allocated" separate from "prior")
                    ////==================================================================================================
                    //// We update prior Allocated because NEED is next and it needs to know what we allocated by Size Rule
                    ////==================================================================================================
                    //_sizeNeedResults.AddPriorAllocatedUnits(aStore.StoreRid, sizeRid, aStore.UnitsAllocated);
                    // end TT#1600 - Jellis - Size Need Algorithm Error

                    // begin MID Track 3781 size curve not required
                    if (aStore.Rule != eSizeRuleType.None)
                    {
                        //_sizeNeedResults.AddStoreSizeRule(aStore.StoreRid, sizeRid, aStore.Rule); // TT#1068 - MD - Jellis - Pack allocations not considered in decisions for bulk size allocations
                        sizeResults.AddStoreSizeRule(aStore.StoreRid, sizeRid, aStore.Rule); // TT#1068 - MD - Jellis - Pack allocations not considered in decisions for bulk size allocations
                    }
                    // end MID Track 3781 size curve not required
                }
			}  // TT#2962 - AnF - Jellis - Size not on Header
		}

		private int CheckAndApplyQuantity(int units, SizeRuleStore aStore, int colorSizeRemaining, eSizeRuleType aRule)  // MID Track 3619 Remove Fringe
		{
			int colorStoreRemaining = (int)_colorStoreRemainingHash[aStore.StoreRid];
			// Begin MID Issue 3212 stodd
			// We ignore limits when Quantity is specified, otherwise we watch them closely!
			// begin MID Track 3620 Change Rule to Upto Type rule--also allow rules to overallocate
			//if (aRule != eSizeFringeRuleType.AbsoluteQuantity)
			//{
			//	units = Math.Min(units, colorSizeRemaining);
			//	units = Math.Min(units, colorStoreRemaining);
			//}
			// end MID Track 3620 Change Rule to Upto Type rule -- also allow rules to overallocate
			// End MID Issue 3212 stodd
			
            // begin TT#1543 - Size Multiple Broken - Part 2 - Fill Holes Broken
            if (aStore.Mult > 1)
            {
                int quote = (int)(((double)units / (double)aStore.Mult) + .5d);
                units = quote * aStore.Mult;
            }
            //if (aStore.Mult > units)
            //{
            //    units = 0;
            //}
            //else if (aStore.Mult > 1)
            //{
            //    // give units in multiples
            //    int quote = units / aStore.Mult;
            //    units = aStore.Mult * quote;
            //}
            // end TT#1543 - Size Multiple Broken - Part 2 - Fill Holes Broken

			//Adjust totals
			if (units > 0)
			{
				colorSizeRemaining -= units;
				colorStoreRemaining -= units;
				// updates hash with new value for store
				_colorStoreRemainingHash.Add(aStore.StoreRid, colorStoreRemaining);
			}

			aStore.UnitsAllocated = units;
			return colorSizeRemaining;
		}

		// begin MID Track 3619 Remove Fringe
		//private void SortSizes()
		//{
		//	try
		//	{
		//		MIDGenericSortItem[] sortedSize = new MIDGenericSortItem[_sizeList.Count];
		//		for (int s=0; s<_sizeList.Count; s++)
		//		{
		//			SizeCodeProfile scp = (SizeCodeProfile)_sizeList[s];
		//			sortedSize[s].Item = s;
		//			sortedSize[s].SortKey = new double[2];
		//			if (_sizeRemainingHash.ContainsKey(scp.Key))
		//				sortedSize[s].SortKey[0] = Convert.ToDouble(_sizeRemainingHash[scp.Key], CultureInfo.CurrentUICulture);
		//			else
		//				sortedSize[s].SortKey[0] = 0;
		//			sortedSize[s].SortKey[1] = MIDMath.GetRandomDouble();
		//		}
        //
		//		if (_sortDirection == eFringeOverrideSort.Ascending)
		//			Array.Sort(sortedSize,new SortAscendingComparer());
		//		else
		//			Array.Sort(sortedSize,new MIDGenericSortDescendingComparer());
        //
		//		ProfileList sortedSizeList = new ProfileList(eProfileType.SizeCode);
		//		foreach (MIDGenericSortItem mgsiSize in sortedSize)
		//		{
		//			int index = mgsiSize.Item;
		//			sortedSizeList.Add(_sizeList[index]);
		//		}
        // 
		//		// overlay size list with sorted size list
		//		_sizeList = sortedSizeList;
		//	}
		//	catch
		//	{
		//		throw;
		//	}
		//}
        // end MID Track 3619 Remove Fringe

		private void SortStores()
		{
			try
			{ 
				//MIDGenericSortItem[] sortedStore = new MIDGenericSortItem[_storeList.Count];  // MID Track 4336 Invalid Operation Exception
				MIDGenericSortItem[] sortedStore = new MIDGenericSortItem[_storeSizeRuleList.Count]; // MID Track 4336 Invalid Operation Exception
				for (int s=0; s<_storeSizeRuleList.Count; s++)  // MID Track 3619 Remove Fringe
				{
					SizeRuleStore aStore = (SizeRuleStore)_storeSizeRuleList[s]; // MID Track 3619 Remove Fringe
					sortedStore[s].Item = s;
					sortedStore[s].SortKey = new double[4];
					sortedStore[s].SortKey[0] = aStore.NeedPercent;	
					sortedStore[s].SortKey[1] = aStore.Need;	
					sortedStore[s].SortKey[2] = Convert.ToDouble(aStore.ColorUnitsRemaining, CultureInfo.CurrentUICulture); 
					sortedStore[s].SortKey[3] = MIDMath.GetRandomDouble();
				}

				Array.Sort(sortedStore,new MIDGenericSortDescendingComparer());

				ArrayList sortedStoreList = new ArrayList();
				foreach (MIDGenericSortItem mgsiStore in sortedStore)
				{
					int index = mgsiStore.Item;
					sortedStoreList.Add(_storeSizeRuleList[index]);  // MID Track 3619 Remove Fringe
				}

				// overlay store size rule list with sorted store size rule list
				_storeSizeRuleList = sortedStoreList; // MID Track 3619 Remove Fringe
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Fills in the data for the size rule class: Need, size rule, min, max, mult...
		/// </summary>
		/// <param name="aSizeCodeProfile">Profile that describes the size</param>
		//private void FillStoreFringe(int sizeRid) // MID Track 3492 Size Need with constraints not allocating correctly
        //private void FillStoreSizeRule(SizeCodeProfile aSizeCodeProfile) // MID Track 3619 Remove Fringe  // TT#1391 - TMW New Action
        private void FillStoreSizeRule (int sizeRid) // TT#1391 - TMW New Action
		{
			try
			{
                //int sizeRid = aSizeCodeProfile.Key;  // TT#1391 - TMW New Action
                SizeNeedResults sizeResults = SizeResults; // TT#1068 - MD - Jellis - Pack allocations not considered in decisions for bulk size allocations
				for (int s=0; s<_storeSizeRuleList.Count; s++) // MID Track 3619 Remove Fringe
				{
					//============
					// Calc NEED
					//============
                    // begin TT#1068 - MD - Jellis - Pack allocations not considered in decisions for bulk size allocations
                    //SizeRuleStore aStore = (SizeRuleStore)_storeSizeRuleList[s];  // MID Track 3619 Remove Fringe
                    //aStore.Need =
                    //    Need.UnitNeed(_sizeNeedResults.GetSizeNeed_PlanUnits(aStore.StoreRid, sizeRid),  // MID Track 4921 AnF#666 Fill to Size Plan Enhancement
                    //    //_sizeNeedResults.GetOnhandUnits(aStore.StoreRid, sizeRid)                                                               // TT#2313 - JEllis - AnF VSW -- Size Need not using VSW Onhand 
                    //    _sizeNeedResults.GetOnhandUnits(aStore.StoreRid, sizeRid) + _sizeNeedResults.GetVswOnhandUnits(aStore.StoreRid, sizeRid), // TT#2313 - JEllis - AnF VSW -- Size Need not using VSW Onhand
                    //    _sizeNeedResults.GetIntransitUnits(aStore.StoreRid, sizeRid),
                    //    // begin TT#1600 - JEllis - Size Need Algorithm Error
                    //    //_sizeNeedResults.GetPriorAllocatedUnits(aStore.StoreRid, sizeRid));
                    //    _sizeNeedResults.GetAllocatedUnits(aStore.StoreRid, sizeRid));
                    //    // end TT#1600 - JEllis - Size Need Algorithm Error
                    //aStore.NeedPercent =
                    //    Need.PctUnitNeed(aStore.Need, _sizeNeedResults.GetSizeNeed_PlanUnits(aStore.StoreRid, sizeRid)); // MID Track 4921 AnF#666 Fill to Size Plan Enhancement
                    SizeRuleStore aStore = (SizeRuleStore)_storeSizeRuleList[s];  // MID Track 3619 Remove Fringe
                    aStore.Need =
                        Need.UnitNeed(sizeResults.GetSizeNeed_PlanUnits(aStore.StoreRid, sizeRid),
                        sizeResults.GetOnhandUnits(aStore.StoreRid, sizeRid) + sizeResults.GetVswOnhandUnits(aStore.StoreRid, sizeRid),
                        sizeResults.GetIntransitUnits(aStore.StoreRid, sizeRid),
                        sizeResults.GetAllocatedUnits(aStore.StoreRid, sizeRid));
                    // end TT#1600 - JEllis - Size Need Algorithm Error
                    aStore.NeedPercent =
                        Need.PctUnitNeed(aStore.Need, sizeResults.GetSizeNeed_PlanUnits(aStore.StoreRid, sizeRid));
                    // end TT#1068 - MD - Jellis - Pack allocations not considered in decisions for bulk size allocations


					// Begin MID Issue 3267 stodd
					int sglRid = (int)_storeGroupLevelHash[aStore.StoreRid];
					// End MID Issue 3267 stodd

					//=======================
					// Get MIN, MAX, MULT
					//=======================
					if (_constraintDecoder != null)
					{
						//MinMaxItemBase minMax = (MinMaxItemBase)_constraintDecoder.GetItem(sglRid, _colorRid, sizeRid); // MID Track 3492 Size Need with constraints not allocating correctly
                        //MinMaxItemBase minMax = (MinMaxItemBase)_constraintDecoder.GetItemForStore(aStore.StoreRid, _colorRid, aSizeCodeProfile); // MID Track 3492 Size Need with constraints not allocating correctly  // TT#1391 - TMW New Action
                        MinMaxItemBase minMax = (MinMaxItemBase)_constraintDecoder.GetItemForStore(aStore.StoreRid, _colorRid, sizeRid);  // TT#1391 - TMW New Action
						aStore.Minimum = minMax.Min;
						aStore.Maximum = minMax.Max;
						if (minMax.Mult > 0)
						{
							aStore.Mult = minMax.Mult;
						}
						else
						{
							aStore.Mult = 1;
						}
					}
					else
					{
						aStore.Minimum = 0;
						aStore.Maximum = int.MaxValue;
						aStore.Mult = 1;
					}

					//==================
					// get Rule
					//==================
					// Begin MID Issue 3212 stodd
					// RuleItemBase fringeRule = (RuleItemBase)_ruleDecoder.GetItem(sglRid, _colorRid, sizeRid); // MID Track 3492 Size Need with constraints not allocating correctly
                    //RuleItemBase sizeRule = (RuleItemBase)_ruleDecoder.GetItemForStore(aStore.StoreRid, _colorRid, aSizeCodeProfile); // MID Track 3619 Remove Fringe // MID Track 3492 Size Need constraints not allocating correctly // TT#1391 - TMW New Action
                    RuleItemBase sizeRule = (RuleItemBase)_ruleDecoder.GetItemForStore(aStore.StoreRid, _colorRid, sizeRid);          // TT#1391 - TMW New Action
					// End MID Issue 3212 stodd
					if (sizeRule.Qty < int.MaxValue)                // MID Track 3620 Size Rules are up to rules
					{                                               // MID Track 3620 Size Rules are up to rules
						if (sizeRule.Rule == Include.Undefined)
						{ 
							aStore.Rule = eSizeRuleType.None;
						}
						else
						{
							aStore.Rule = (eSizeRuleType)sizeRule.Rule; // MID Track 3619 Remove Fringe
						}
						aStore.RuleQuantity = sizeRule.Qty;
					}                                               // MID Track 3620 Size Rules are up to rules
				}
			}
			catch
			{
				throw;
			}
		}

		


	}

	public class SizeRuleStore      // MID Track 3619 Remove Fringe
	{
		private int _storeRid;
		private int _min;
		private int _max;
		private int _mult;
		private eSizeRuleType _rule;  // MID Track 3619 Remove Fringe
		private int _ruleQuantity; 
		private int _unitsAllocated;
		private int _unitsRemaining;
		private int _colorUnitsRemaining;
		private double _need;
		private double _needPct;

		/// <summary>
		/// The Store Record ID (RID).
		/// </summary>
		public int StoreRid
		{
			get {return _storeRid;}
			set {_storeRid = value;}
		}
		public int Minimum
		{
			get {return _min;}
			set {_min = value;}
		}
		public int Maximum
		{
			get {return _max;}
			set {_max = value;}
		}
		public int Mult
		{
			get {return _mult;}
			set {_mult = value;}
		}
		public eSizeRuleType Rule // MID Track 3619 Remove Fringe
		{
			get {return _rule;}
			set {_rule = value;}
		}
		/// <summary>
		/// Only used of the rule is Quantity.  This is the quantity to use.
		/// </summary>
		public int RuleQuantity
		{
			get {return _ruleQuantity;}
			set {_ruleQuantity = value;}
		}
		/// <summary>
		/// The quantity size rule allocated to this store.
		/// </summary>
		public int UnitsAllocated
		{
			get {return _unitsAllocated;}
			set {_unitsAllocated = value;}
		}
		/// <summary>
		/// The quantity remaing to be allocated to this store from the size.
		/// </summary>
		public int UnitsRemaining
		{
			get {return _unitsRemaining;}
			set {_unitsRemaining = value;}
		}

		/// <summary>
		/// Remaining units at the color level for this store
		/// </summary>
		public int ColorUnitsRemaining
		{
			get {return _colorUnitsRemaining;}
			set {_colorUnitsRemaining = value;}
		}

		public double Need
		{
			get {return _need;}
			set {_need = value;}
		}

		public double NeedPercent
		{
			get {return _needPct;}
			set {_needPct = value;}
		}
		

		public SizeRuleStore(int storeRid)   // MID Track 3619 Remove Fringe
		{
			_storeRid = storeRid;
			this._rule = eSizeRuleType.None; // MID Track 3781 Size Curve Not Required
		}



	}
}
