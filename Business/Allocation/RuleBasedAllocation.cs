
// TT#575 - MD - Jellis - Assorment - Restructure Rule Method Logic
// This code originally resided in RuleMethod.cs
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Text;
using MIDRetail.Common;
using MIDRetail.Data;
using MIDRetail.DataCommon;

namespace MIDRetail.Business.Allocation
{
	public class RuleBasedAllocation
	{
        private SessionAddressBlock _sab;
        private ApplicationSessionTransaction _appSessionTran;
        public RuleBasedAllocation(ApplicationSessionTransaction aAppSessionTransaction)
        {
            _appSessionTran = aAppSessionTransaction;
            _sab = aAppSessionTransaction.SAB;
        }

        /// <summary>
        /// Calculates a Rule Allocation for the given list of stores
        /// </summary>
        /// <param name="aStoreList">List of Index_RIDs that identify the stores</param>
        /// <param name="aRuleMethodType">Rule Method Type</param>
        /// <param name="aRuleQuantity">For "absolute quantity" the rule quantity; this is ignored for all other rules</param>
        /// <param name="aBasisAllocationProfile">Basis Allocation Profile for "Fill", "Exact" and "Proportional" allocations; ignored for all other rules</param>
        /// <param name="aBasisComponent">Basis General Component describing the component on the Basis Allocation Profile to use as the basis of a "Fill", "Exact" or "Proportional" allocation</param>
        /// <param name="aTargetAllocationProfile">Target Allocation Profile where the calculated allocation will eventually be saved.</param>
        /// <param name="aTargetComponent">Target General Component describing the component on the Target Allocation Profile where the calculated allocation will eventually be saved.</param>
        /// <param name="aBasisSortDirection">Basis sort direction only used on a "Fill" allocation</param>
        /// <param name="aRuleAllocationProfile">Rule Allocation Profile to which the calculated allocation is to be saved (the calculated allocation is not saved on the target allocation profile by this calculation module).</param>
        /// <param name="aStatusMsg">Status Message that gives a reason for a failed calculation.</param>
        /// <returns>True: Calculation was successful and result is in Rule Allocation Profile; False: Calculation failed, reason for failure is returned via aStatusMsg.</returns>
        public bool  CalculateRuleAllocation
            (
            List<Index_RID> aStoreList,
            eRuleMethod aRuleMethodType,
            int aRuleQuantity,
            AllocationProfile aBasisAllocationProfile,
            GeneralComponent aBasisComponent,
            AllocationProfile aTargetAllocationProfile,
            GeneralComponent aTargetComponent,
            eSortDirection aBasisSortDirection,
            ref RuleAllocationProfile aRuleAllocationProfile,
            out MIDException aStatusMsg
            )
		{
            switch (aRuleMethodType)
            {
                case (eRuleMethod.Exact):
                {
                    return CalculateExactAllocation
                        (
                        aStoreList,
                        aBasisAllocationProfile,
                        aBasisComponent,
                        aTargetAllocationProfile,
                        aTargetComponent,
                        ref aRuleAllocationProfile,
                        out aStatusMsg
                        );
                }
                case (eRuleMethod.Fill):
                {
                    return CalculateFillAllocation(
                            aStoreList,
                            aBasisAllocationProfile,
                            aBasisComponent,
                            aTargetAllocationProfile,
                            aTargetComponent,
                            aBasisSortDirection,
                            ref aRuleAllocationProfile,
                            out aStatusMsg);
                }
                case (eRuleMethod.Out):
                {
                    return ExcludeStores(
                        aStoreList,
                        ref aRuleAllocationProfile,
                        out aStatusMsg);
                }
                case (eRuleMethod.ColorMaximum):
                {
                    return CalculateColorMaximumAllocation(
                        aStoreList,
                        aTargetAllocationProfile,
                        aTargetComponent,
                        ref aRuleAllocationProfile,
                        out aStatusMsg);
                }
				case (eRuleMethod.ColorMinimum):
				{
					return CalculateColorMinimumAllocation(
                        aStoreList,
                        aTargetAllocationProfile,
                        aTargetComponent,
                        ref aRuleAllocationProfile,
                        out aStatusMsg);
	    		}
				case (eRuleMethod.Quantity):
				{
					return this.CalculateAbsoluteQtyAllocation(
                        aStoreList,
                        aTargetAllocationProfile,
                        aTargetComponent,
                        aRuleQuantity,
                        ref aRuleAllocationProfile,
                        out aStatusMsg);
				}
    			case (eRuleMethod.None):
	    		{
                    return CalculateNoRuleAllocation(
                        aStoreList,
                        aTargetAllocationProfile,
                        aTargetComponent,
                        ref aRuleAllocationProfile,
                        out aStatusMsg);
				}
				case (eRuleMethod.Proportional):
				{	
					return CalculateProportionalAllocation(
                        aStoreList,
                        aBasisAllocationProfile,
                        aBasisComponent,
                        aTargetAllocationProfile,
                        aTargetComponent,
                        ref aRuleAllocationProfile,
                        out aStatusMsg);
				}
				case (eRuleMethod.AdMinimum):
				{
					return CalculateGradeAdMinimumAllocation(
                        aStoreList,
                        aTargetAllocationProfile,
                        aTargetComponent,
                        ref aRuleAllocationProfile,
                        out aStatusMsg);
                }
				case (eRuleMethod.StockMaximum): 
					{
						return CalculateGradeMaximumAllocation                            (
                            aStoreList,
                            aTargetAllocationProfile,
                            aTargetComponent,
                            ref aRuleAllocationProfile,
                            out aStatusMsg);
					}
				case (eRuleMethod.StockMinimum):  // same as Allocation Min
				{
					return this.CalculateGradeMinimumAllocation(
                            aStoreList,
                            aTargetAllocationProfile,
                            aTargetComponent,
                            ref aRuleAllocationProfile,
                            out aStatusMsg);
				}
            }
			string errorMessage = 	string.Format
				(MIDText.GetText(eMIDTextCode.msg_InvalidRule),
				aRuleMethodType.ToString());

            errorMessage =
				"Header ["
                + aTargetAllocationProfile.HeaderID
				+ "] "
				+ errorMessage;
            _sab.ApplicationServerSession.Audit.Add_Msg(
				eMIDMessageLevel.Severe,
				eMIDTextCode.msg_InvalidRule,
				errorMessage,
				this.GetType().Name);
            aStatusMsg = 
				new MIDException(
				eErrorLevel.severe,
				(int)eMIDTextCode.msg_InvalidRule,
				errorMessage);
            return false;
        }

        /// <summary>
        /// Creates OUT rule for each store in the list
        /// </summary>
        /// <param name="aStoreList">List of Index_RID objects that identify the stores</param>
        /// <param name="aRuleAllocationProfile">The RuleAllocationProfile where the store rules are to be saved</param>
        /// <param name="aStatusMsg">Error message indicating why ExcludeStores failed (null when ExcludeStores succeeds)</param>
        /// <returns>True: when Exclude Stores succeeds; False: when Exclude Stores fails, in this case aStatusMsg indicates the reason for failure.</returns>
        public bool ExcludeStores(
            List<Index_RID> aStoreList,
            ref RuleAllocationProfile aRuleAllocationProfile,
            out MIDException aStatusMsg
            )
        {
            aStatusMsg = null;
            try
            {
                foreach (Index_RID storeIdxRID in aStoreList)
			    {
				    aRuleAllocationProfile.SetStoreRuleAllocation(storeIdxRID, eRuleType.Exclude, 0);
			    }
            }
            catch (Exception e)
            {
                aStatusMsg = new MIDException(eErrorLevel.severe, (int)eMIDTextCode.systemError, e.Message);
                return false;
            }
            finally
            {
            }

            return true;
        }

        /// <summary>
        /// Calculates an Exact Rule Allocation
        /// </summary>
        /// <param name="aStoreList">List of Index_RID objects that identify the stores</param>
        /// <param name="aBasisAllocationProfile">Allocation Profile to use as the basis of the exact allocation.</param>
        /// <param name="aBasisComponent">Component on the Basis Allocation Profile to use as the basis of the exact allocation</param>
        /// <param name="aTargetAllocationProfile">Allocation Profile where the "Exact" rule allocation is to be calculated</param>
        /// <param name="aTargetComponent">Component on the Target Allocation Profile where the "Exact" rule allocation is to be calculated</param>
        /// <param name="aRuleAllocationProfile">The Rule Allocation Profile where the store rules are to be saved</param>
        /// <param name="aStatusMsg">Error message indicating why CalculateExactAllocation failed (null when CalculateExactAllocation succeeds)</param>
        /// <returns>True: when CalculateExactAllocation succeeds; False: when CalculateExactAllocation fails, in this case aStatusMsg indicates the reason for failure.</returns>
        public bool CalculateExactAllocation(
            List<Index_RID> aStoreList,
            AllocationProfile aBasisAllocationProfile,
            GeneralComponent aBasisComponent,
            AllocationProfile aTargetAllocationProfile,
            GeneralComponent aTargetComponent,
            ref RuleAllocationProfile aRuleAllocationProfile,
            out MIDException aStatusMsg)
         {
            AllocationWorkMultiple priorHeaderWorkMultiple = GetQtyPerPack(aBasisComponent, aBasisAllocationProfile);
            AllocationWorkMultiple currHeaderWorkMultiple = GetQtyPerPack(aTargetComponent, aTargetAllocationProfile);

            aStatusMsg = null;
            int qty;
            int priorQty;
            int remainder;
            int max;
            try
            {
			    foreach (Index_RID storeIdxRID in aStoreList)
			    {
                    priorQty = aBasisAllocationProfile.GetStoreQtyAllocated(aBasisComponent, storeIdxRID);
                    if (aBasisComponent.ComponentType == eComponentType.SpecificPack)
                    {
					    priorQty = priorQty * priorHeaderWorkMultiple.Multiple;
                    }
				    remainder = priorQty % currHeaderWorkMultiple.Multiple;
				    if (remainder > 0)
				    {
					    aStatusMsg = new MIDException(eErrorLevel.severe,
						    (int)(eMIDTextCode.msg_al_PacksNotCompatibleForExactRule),
						    MIDText.GetText(eMIDTextCode.msg_al_PacksNotCompatibleForExactRule));
                        return false;
				    }
				    else
				    {
                        if (aTargetComponent.ComponentType == eComponentType.SpecificPack)
						    qty = priorQty / currHeaderWorkMultiple.Multiple;
					    else
						    qty = priorQty;
				    }

                    //max = aTargetAllocationProfile.GetStoreCapacityMaximum(storeIdxRID);                    // TT#1074 - MD - Jellis - Group Allocation - Inventory Min Max broken
                    max = aTargetAllocationProfile.GetStoreCapacityMaximum(aTargetComponent, storeIdxRID, true);  // TT#1074 - MD - JEllis - Group Allocation - Inventory Min Max broken
				    if (qty > max)  // Test for greater than MAX
				    {
					    // TODO 
					    // put some kind of message to the log
					    // IF (and that’ a big “if”) we put any message out, it should go to the audit log;  
					    // in that case, there should only be one message that basically says exact 
					    // rejected because of MAX constraint (if we identify the stores, 
					    // they should be in a list within this one message).
				    }
				    else
                    {
					    aRuleAllocationProfile.SetStoreRuleAllocation(storeIdxRID, eRuleType.Exact, qty);
                    }
			    }
            }
            catch (Exception e)
            {
                aStatusMsg = new MIDException(eErrorLevel.severe, (int)eMIDTextCode.systemError, e.Message);
                return false;
            }
            finally
            {
            }
            return true;
        }
        /// <summary>
        /// Calculates a Proportional Rule Allocation
        /// </summary>
        /// <param name="aStoreList">List of Index_RID objects that identify the stores</param>
        /// <param name="aBasisAllocationProfile">Allocation Profile to use as the basis of the proportional allocation.</param>
        /// <param name="aBasisComponent">Component on the Basis Allocation Profile to use as the basis of the proportional allocation</param>
        /// <param name="aTargetAllocationProfile">Allocation Profile where the "Proportional" rule allocation is to be calculated</param>
        /// <param name="aTargetComponent">Component on the Target Allocation Profile where the "Exact" rule allocation is to be calculated</param>
        /// <param name="aRuleAllocationProfile">The Rule Allocation Profile where the store rules are to be saved</param>
        /// <param name="aStatusMsg">Error message indicating why CalculateProportionalAllocation failed (null when CalculateProportionalAllocation succeeds)</param>
        /// <returns>True: when CalculateProportionalAllocationn succeeds; False: when CalculateProportionalAllocation fails, in this case aStatusMsg indicates the reason for failure.</returns> 
        public bool CalculateProportionalAllocation(
                List<Index_RID> aStoreList,
                AllocationProfile aBasisAllocationProfile,
                GeneralComponent aBasisComponent,
                AllocationProfile aTargetAllocationProfile,
                GeneralComponent aTargetComponent,
                ref RuleAllocationProfile aRuleAllocationProfile,
                out MIDException aStatusMsg
            )
            // begin TT#586 - MD - Add overloads for proportional and fill when placeholder is target
        {
            return CalculateProportionalAllocation(
                aStoreList,
                aBasisAllocationProfile,
                aBasisComponent,
                aTargetAllocationProfile.GetRuleUnitsToAllocate(aTargetComponent),
                aTargetAllocationProfile,
                aTargetComponent,
                ref aRuleAllocationProfile,
                out aStatusMsg);
        }
        /// <summary>
        /// Calculates a Proportional Rule Allocation
        /// </summary>
        /// <param name="aStoreList">List of Index_RID objects that identify the stores</param>
        /// <param name="aBasisAllocationProfile">Allocation Profile to use as the basis of the proportional allocation.</param>
        /// <param name="aBasisComponent">Component on the Basis Allocation Profile to use as the basis of the proportional allocation</param>
        /// <param name="aTargetTotalQuantity">Total Quantity to spread proportionally</param>
        /// <param name="aTargetAllocationProfile">Allocation Profile where the "Proportional" rule allocation is to be calculated</param>
        /// <param name="aTargetComponent">Component on the Target Allocation Profile where the "Exact" rule allocation is to be calculated</param>
        /// <param name="aRuleAllocationProfile">The Rule Allocation Profile where the store rules are to be saved</param>
        /// <param name="aStatusMsg">Error message indicating why CalculateProportionalAllocation failed (null when CalculateProportionalAllocation succeeds)</param>
        /// <returns>True: when CalculateProportionalAllocationn succeeds; False: when CalculateProportionalAllocation fails, in this case aStatusMsg indicates the reason for failure.</returns> 
        public bool CalculateProportionalAllocation(
                List<Index_RID> aStoreList,
                AllocationProfile aBasisAllocationProfile,
                GeneralComponent aBasisComponent,
                int aTargetTotalQuantity,
                AllocationProfile aTargetAllocationProfile,
                GeneralComponent aTargetComponent,
                ref RuleAllocationProfile aRuleAllocationProfile,
                out MIDException aStatusMsg
            )
            // end TT#586 - MD - Jellis - Add overloads for propportional and fill when placeholder is target
        {
            AllocationWorkMultiple currHeaderWorkMultiple = GetQtyPerPack(aTargetComponent, aTargetAllocationProfile);
            int qty;
            aStatusMsg = null;
            try
            {
			    ArrayList summandList = new ArrayList();
                foreach (Index_RID storeIdxRID in aStoreList)
			    {
				    Summand aSummand = new Summand();
                    aSummand.Eligible = aTargetAllocationProfile.GetStoreIsEligible(storeIdxRID);
				    aSummand.Item = storeIdxRID.RID;
				    aSummand.ItemIdx = storeIdxRID.Index;
                    qty = aBasisAllocationProfile.GetStoreQtyAllocated(aBasisComponent, storeIdxRID);
				    aSummand.Quantity = (double)qty;
                    aSummand.Min = 0;
                    aSummand.Max = aTargetAllocationProfile.GetStorePrimaryMaximum(aTargetComponent, storeIdxRID); 

				    summandList.Add(aSummand);
			    }

                ProportionalSpread aSpread = new ProportionalSpread(_sab);
			    aSpread.SummandList = summandList;
                //aSpread.RequestedTotal = aTargetAllocationProfile.GetRuleUnitsToAllocate(aTargetComponent); // TT#586 - MD - Jellis - Add overloads for proportional and fill when placehold is target
                aSpread.RequestedTotal = aTargetTotalQuantity;                                                // TT#586 - MD - Jellis - Add overloads for proportional and fill when placehold is target
			    aSpread.Precision = 0;

			    // If the current componenet is a pack, then the Requested Total is already in packs (not units)
			    // so the multiple becomes 1.
                if (aTargetComponent.ComponentType != eComponentType.SpecificPack)
                {
                    aSpread.Multiple = currHeaderWorkMultiple.Multiple;
                }
			    aSpread.Calculate();
					
			    foreach (Summand aSummand  in summandList)
			    {
				    aRuleAllocationProfile.SetStoreRuleAllocation(new Index_RID(aSummand.ItemIdx, aSummand.Item), eRuleType.ProportionalAllocated, (int)aSummand.Result);
			    }
            }
            catch (Exception e)
            {
                aStatusMsg = new MIDException(eErrorLevel.severe, (int)eMIDTextCode.systemError, e.Message);
                return false;
            }
            finally
            {
            }
            return true;
        }

        /// <summary>
        /// Calculates a Fill Rule Allocation
        /// </summary>
        /// <param name="aStoreList">List of Index_RID objects that identify the stores</param>
        /// <param name="aBasisAllocationProfile">Allocation Profile to use as the basis of the exact allocation.</param>
        /// <param name="aBasisComponent">Component on the Basis Allocation Profile to use as the basis of the exact allocation</param>
        /// <param name="aTargetAllocationProfile">Allocation Profile where the "Exact" rule allocation is to be calculated</param>
        /// <param name="aTargetComponent">Component on the Target Allocation Profile where the "Exact" rule allocation is to be calculated</param>
        /// <param name="aRuleAllocationProfile">The Rule Allocation Profile where the store rules are to be saved</param>
        /// <param name="aStatusMsg">Error message indicating why CalculateFillAllocation failed (null when CalculateFillAllocation succeeds)</param>
        /// <returns>True: when CalculateFillAllocation succeeds; False: when CalculateFillAllocation fails, in this case aStatusMsg indicates the reason for failure.</returns>  
        public bool CalculateFillAllocation(
                List<Index_RID> aStoreList,
                AllocationProfile aBasisAllocationProfile,
                GeneralComponent aBasisComponent,
                AllocationProfile aTargetAllocationProfile,
                GeneralComponent aTargetComponent,
                eSortDirection aBasisSortDirection,
                ref RuleAllocationProfile aRuleAllocationProfile,
                out MIDException aStatusMsg)
        // begin TT#586 - MD - Jellis - Add overloads for proportional and fill when placehold is target
        {
            return CalculateFillAllocation(
                aStoreList,
                aBasisAllocationProfile,
                aBasisComponent,
                aTargetAllocationProfile.GetRuleUnitsToAllocate(aTargetComponent),
                aTargetAllocationProfile,
                aTargetComponent,
                aBasisSortDirection,
                ref aRuleAllocationProfile,
                out aStatusMsg);
        }
        public bool CalculateFillAllocation(
                List<Index_RID> aStoreList,
                AllocationProfile aBasisAllocationProfile,
                GeneralComponent aBasisComponent,
                int aTargetTotalFillQuantity,
                AllocationProfile aTargetAllocationProfile,
                GeneralComponent aTargetComponent,
                eSortDirection aBasisSortDirection,
                ref RuleAllocationProfile aRuleAllocationProfile,
                out MIDException aStatusMsg)
        // end TT#586 - MD - Jellis - Add overloads for proportional and fill when placehold is target
        {
            AllocationWorkMultiple priorHeaderWorkMultiple = GetQtyPerPack(aBasisComponent, aBasisAllocationProfile);
            AllocationWorkMultiple currHeaderWorkMultiple = GetQtyPerPack(aTargetComponent, aTargetAllocationProfile);
            int qty;
            int priorQty;
            int remainder;
            int max;
            aStatusMsg = null;
			//**************
			// Sort stores
			//**************
            try
            {
			    ArrayList summandList = new ArrayList();
                foreach (Index_RID storeIdxRID in aStoreList)
			    {
				    Summand aSummand = new Summand();
                    aSummand.Eligible = aTargetAllocationProfile.GetStoreIsEligible(storeIdxRID);
				    aSummand.Item = storeIdxRID.RID;
				    aSummand.ItemIdx = storeIdxRID.Index;

                    priorQty = aBasisAllocationProfile.GetStoreQtyAllocated(aBasisComponent, storeIdxRID);
                    if (aBasisComponent.ComponentType == eComponentType.SpecificPack)
					    priorQty = priorQty * priorHeaderWorkMultiple.Multiple;

				    remainder = priorQty % currHeaderWorkMultiple.Multiple;
				    if (remainder > 0)
				    {
					    aStatusMsg = new MIDException(eErrorLevel.severe,
						    (int)(eMIDTextCode.msg_al_PacksNotCompatibleForFillRule),
						    MIDText.GetText(eMIDTextCode.msg_al_PacksNotCompatibleForFillRule));
                        return false;
				    }
				    else
				    {
                        if (aTargetComponent.ComponentType == eComponentType.SpecificPack)
                        {
						    qty = priorQty / currHeaderWorkMultiple.Multiple;
                        }
					    else
                        {
						    qty = priorQty;
                        }
				    }

                    //max = aTargetAllocationProfile.GetStoreCapacityMaximum(storeIdxRID);                   // TT#1074 - MD - Jellis - Group Allocation - Inventory Min Max Broken
                    max = aTargetAllocationProfile.GetStoreCapacityMaximum(aTargetComponent, storeIdxRID, true); // TT#1074 - MD - Jellis - Group Allocation - Inventory Min Max Broken
 				    if (qty > max)
                    {
					    aSummand.Quantity = (double)max;
                    }
				    else
                    {
					    aSummand.Quantity = (double)qty;
                    }
				    summandList.Add(aSummand);
			    }
                if (aBasisSortDirection == eSortDirection.Descending)
                {
				    summandList.Sort(new SummandDescendingComparer());
                }
			    else
                {
				    summandList.Sort(new SummandAscendingComparer());
                }

			    //*****************************
			    // Get total units to allocate
			    //*****************************
                //int totalUnits = aTargetAllocationProfile.GetRuleUnitsToAllocate(aTargetComponent); // TT#586 - MD - Jellis - add overload for proportional and fill when target is placeholder
                int totalUnits = aTargetTotalFillQuantity;                                            // TT#586 - MD - Jellis - add overload for proportional and fill when target is placeholder   
			    int remainingUnits = totalUnits;

			    //************************
			    // Allocate to each store
			    //************************
			    foreach (Summand aSummand  in summandList)
			    {
				    if (aSummand.Quantity < remainingUnits)
				    {
					    aRuleAllocationProfile.SetStoreRuleAllocation(new Index_RID(aSummand.ItemIdx, aSummand.Item), eRuleType.Fill, (int)aSummand.Quantity);
					    remainingUnits -= (int)aSummand.Quantity;
				    }
				    else
				    {
					    aRuleAllocationProfile.SetStoreRuleAllocation(new Index_RID(aSummand.ItemIdx, aSummand.Item), eRuleType.Fill, remainingUnits);
					    remainingUnits = 0;
				    }
			    }
            }
            catch (Exception e)
            {
                aStatusMsg = new MIDException(eErrorLevel.severe, (int)eMIDTextCode.systemError, e.Message);
                return false;
            }
            finally
            {
            }
            return true;
        }

        /// <summary>
        /// Calculates a Grade Minimum Rule Allocation
        /// </summary>
        /// <param name="aStoreList">List of Index_RID objects that identify the stores</param>
        /// <param name="aTargetAllocationProfile">Allocation Profile where the "Exact" rule allocation is to be calculated</param>
        /// <param name="aTargetComponent">Component on the Target Allocation Profile where the "Exact" rule allocation is to be calculated</param>
        /// <param name="aRuleAllocationProfile">The Rule Allocation Profile where the store rules are to be saved</param>
        /// <param name="aStatusMsg">Error message indicating why CalculateGradeMinimumAllocation failed (null when CalculateGradeMinimumAllocation succeeds)</param>
        /// <returns>True: when CalculateGradeMinimumAllocation succeeds; False: when CalculateGradeMinimumAllocation fails, in this case aStatusMsg indicates the reason for failure.</returns>  
        public bool CalculateGradeMinimumAllocation(
                List<Index_RID> aStoreList,
                AllocationProfile aTargetAllocationProfile,
                GeneralComponent aTargetComponent,
                ref RuleAllocationProfile aRuleAllocationProfile,
                out MIDException aStatusMsg
            )
        {
            int min;
            aStatusMsg = null;
            eMIDTextCode statusReasonCode; // TT#1176- MD - Jellis - Group Allocation - Size need not observing inv min max
            try
            {
                foreach (Index_RID storeIdxRID in aStoreList)
			    {
                    // begin TT#1176 - MD - Jellis - Group Allocation Size Need not observing inv min max
                    //min = aTargetAllocationProfile.GetStoreMinimum(aTargetComponent, storeIdxRID, true);    // TT#1074 - MD - Jellis - Group Allocation - Inventory Min Max broken
                    if (!aTargetAllocationProfile.TryGetStoreMinimum(aTargetComponent, storeIdxRID, true, out min, out statusReasonCode))
                    {
                        aStatusMsg = 
                            new MIDException(eErrorLevel.severe,
                                (int)statusReasonCode,
                                MIDText.GetTextOnly(statusReasonCode)
                                + " : Header ID [" +   aTargetAllocationProfile.HeaderID + "]" 
                                + " : Source/Method [" + GetType().Name + " / CalculateGradeMinimumAllocation]");
                        return false;
                    }
                    //if (min <= aTargetAllocationProfile.GetStoreCapacityMaximum(storeIdxRID)                    // TT#1074 - MD - Jellis - Group Allocation - Inventory Min Max broken
                    if (min <= aTargetAllocationProfile.GetStoreCapacityMaximum(aTargetComponent, storeIdxRID, true)    // TT#1074 - MD - Jellis - Group Allocation - Inventory Min Max broken
                        && min <= aTargetAllocationProfile.GetStorePrimaryMaximum(aTargetComponent, storeIdxRID))
                    {
					    aRuleAllocationProfile.SetStoreRuleAllocation(storeIdxRID, eRuleType.Minimum, 0);
				    }
			    }
                }
            catch (Exception e)
            {
                aStatusMsg = new MIDException(eErrorLevel.severe, (int)eMIDTextCode.systemError, e.Message);
                return false;
            }
            finally
            {
            }
            return true;
        }

        /// <summary>
        /// Calculates a Grade Ad Minimum Rule Allocation
        /// </summary>
        /// <param name="aStoreList">List of Index_RID objects that identify the stores</param>
        /// <param name="aTargetAllocationProfile">Allocation Profile where the "Exact" rule allocation is to be calculated</param>
        /// <param name="aTargetComponent">Component on the Target Allocation Profile where the "Exact" rule allocation is to be calculated</param>
        /// <param name="aRuleAllocationProfile">The Rule Allocation Profile where the store rules are to be saved</param>
        /// <param name="aStatusMsg">Error message indicating why CalculateGradeAdMinimumAllocation failed (null when CalculateGradeAdMinimumAllocation succeeds)</param>
        /// <returns>True: when CalculateGradeAdMinimumAllocation succeeds; False: when CalculateGradeAdMinimumAllocation fails, in this case aStatusMsg indicates the reason for failure.</returns>   
        public bool CalculateGradeAdMinimumAllocation(
                List<Index_RID> aStoreList,
                AllocationProfile aTargetAllocationProfile,
                GeneralComponent aTargetComponent,
                ref RuleAllocationProfile aRuleAllocationProfile,
                out MIDException aStatusMsg
            )
        {
            int min;
            aStatusMsg = null;
            try 
            {
                int capacityMaximum; // TT#1074 - MD - Jellis - Group Allocation - Inventory min max broken
                foreach (Index_RID storeIdxRID in aStoreList)
			    {
                    min = aTargetAllocationProfile.GetGradeAdMinimum(aTargetAllocationProfile.GetStoreGradeIdx(storeIdxRID));   
                    if (aTargetAllocationProfile.GradeInventoryMinimumMaximum) 
                    {    
                        min = Math.Max(0, min - aTargetAllocationProfile.GetStoreInventoryBasis(aTargetAllocationProfile.GradeInventoryBasisHnRID, storeIdxRID, true)); // TT#1074 - MD - Jellis - Group Allocation - Inventory Min Max Broken 
                    }
                    //if (min <= aTargetAllocationProfile.GetStoreCapacityMaximum(storeIdxRID)                   // TT#1074 - MD - Jellis - Group Allocation - Inventory Min Max Broken
                    if (min <= aTargetAllocationProfile.GetStoreCapacityMaximum(aTargetComponent, storeIdxRID, true) // TT#1074 - MD - Jellis- Group Allocation - Inventory Min Max Broken
                        && min <= aTargetAllocationProfile.GetStorePrimaryMaximum(aTargetComponent, storeIdxRID))
                    {
                        aRuleAllocationProfile.SetStoreRuleAllocation(storeIdxRID, eRuleType.AdMinimum, 0);
                    }
			    }
            }
            catch (Exception e)
            {
                aStatusMsg = new MIDException(eErrorLevel.severe, (int)eMIDTextCode.systemError, e.Message);
                return false;
            }
            finally
            {
            }
            return true;
        }

        /// <summary>
        /// Calculates a Color Minimum Rule Allocation
        /// </summary>
        /// <param name="aStoreList">List of Index_RID objects that identify the stores</param>
        /// <param name="aTargetAllocationProfile">Allocation Profile where the "Exact" rule allocation is to be calculated</param>
        /// <param name="aTargetComponent">Component on the Target Allocation Profile where the "Exact" rule allocation is to be calculated</param>
        /// <param name="aRuleAllocationProfile">The Rule Allocation Profile where the store rules are to be saved</param>
        /// <param name="aStatusMsg">Error message indicating why CalculateColorMinimumAllocation failed (null when CalculateColorMinimumAllocation succeeds)</param>
        /// <returns>True: when CalculateColorMinimumAllocation succeeds; False: when CalculateColorMinimumAllocation fails, in this case aStatusMsg indicates the reason for failure.</returns>   
        public bool CalculateColorMinimumAllocation(
                List<Index_RID> aStoreList,
                AllocationProfile aTargetAllocationProfile,
                GeneralComponent aTargetComponent,
                ref RuleAllocationProfile aRuleAllocationProfile,
                out MIDException aStatusMsg)
        {

            int min;
            aStatusMsg = null;
            try
            {
			    GeneralComponent colorComponent;
                switch (aTargetComponent.ComponentType)
			    {
				    case (eComponentType.ColorAndSize):
				    {
					    colorComponent = ((AllocationColorSizeComponent)aTargetComponent).ColorComponent;
					    break;
				    }
				    case (eComponentType.SpecificColor):
				    {
					    colorComponent = (AllocationColorOrSizeComponent)aTargetComponent;
					    break;
				    }
				    default:
				    {
					    colorComponent = null;
					    break;
				    }
			    }
			    if (colorComponent != null
				    && colorComponent.ComponentType == eComponentType.SpecificColor)
			    {
				    foreach (Index_RID storeIdxRID in aStoreList)
				    {
                        min = aTargetAllocationProfile.GetStoreColorMinimum(((AllocationColorOrSizeComponent)colorComponent).ColorRID, storeIdxRID);
                        //if (min <= aTargetAllocationProfile.GetStoreCapacityMaximum(storeIdxRID)                   // TT#1074 - MD - Jellis - Group Allocation Inventory min max broken
                        if (min <= aTargetAllocationProfile.GetStoreCapacityMaximum(aTargetComponent, storeIdxRID, true) // TT#1074 - MD - Jellis - Group ALlocation Inventory Min max broken
                            && min <= aTargetAllocationProfile.GetStorePrimaryMaximum(colorComponent, storeIdxRID))
                        {
                            aRuleAllocationProfile.SetStoreRuleAllocation(storeIdxRID, eRuleType.ColorMinimum, 0);
                        }
				    }
			    }
			    else
			    {
				    string colorMinimum = MIDText.GetTextOnly((int)eRuleType.ColorMinimum);
                    string ruleComponent = MIDText.GetTextOnly((int)aTargetComponent.ComponentType);
				    string specificColor = MIDText.GetTextOnly((int)eComponentType.SpecificColor);
				    string message = string.Format
                        (_sab.ApplicationServerSession.Audit.GetText(eMIDTextCode.msg_al_CannotDetermineColorMinMaxUsingSpecifiedComponent, false),
                        aTargetAllocationProfile.HeaderID,
					    GetType().Name + "/" + "CalculateColorMinimum",
					    colorMinimum,
					    ruleComponent,
					    specificColor);
                    _sab.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Warning, message, this.GetType().Name);
				    aStatusMsg = 
                        new MIDException(
                            eErrorLevel.warning,
                            (int)(eMIDTextCode.msg_al_CannotDetermineColorMinMaxUsingSpecifiedComponent),
					        message);
                    return false;
			    }
            }
            catch (Exception e)
            {
                aStatusMsg = new MIDException(eErrorLevel.severe, (int)eMIDTextCode.systemError, e.Message);
                return false;
            }
            finally
            {
            }
            return true;
        }

        /// <summary>
        /// Calculates Grade Maximum Allocation
        /// </summary>
        /// <param name="aStoreList">List of Index_RID objects that identify the stores</param>
        /// <param name="aTargetAllocationProfile">Allocation Profile where the "Exact" rule allocation is to be calculated</param>
        /// <param name="aTargetComponent">Component on the Target Allocation Profile where the "Exact" rule allocation is to be calculated</param>
        /// <param name="aRuleAllocationProfile">The Rule Allocation Profile where the store rules are to be saved</param>
        /// <param name="aStatusMsg">Error message indicating why CalculateGradeMaximumAllocation failed (null when CalculateGradeMaximumAllocation succeeds)</param>
        /// <returns>True: when CalculateGradeMaximumAllocation succeeds; False: when CalculateGradeMaximumAllocation fails, in this case aStatusMsg indicates the reason for failure.</returns>   
        public bool CalculateGradeMaximumAllocation(
                List<Index_RID> aStoreList,
                AllocationProfile aTargetAllocationProfile,
                GeneralComponent aTargetComponent,
                ref RuleAllocationProfile aRuleAllocationProfile,
                out MIDException aStatusMsg)
        {
            int max;
            aStatusMsg = null;
            eMIDTextCode statusReasonCode; // TT#1176 - MD - Jellis - Group Allocation - Size need not observing inv min max
            try
            {
                //int capacityMax; // TT#1074 - MD - Jellis - Group Allocation - Inventory Min max broken  // TT#1176 - MD - Jellis - Group Allocation - Size need not observing inv min max
                foreach (Index_RID storeIdxRID in aStoreList)
			    {
                    // begin TT#1176 - MD -Jellis - Group Allocation - Size Need not observing inv min max
                    //max = aTargetAllocationProfile.GetStoreMaximum(aTargetComponent, storeIdxRID, true); // TT#1074 - MD - Jellis - Group Allocation - Inventory MIn Max Broken
                    if (!aTargetAllocationProfile.TryGetStoreMaximum(aTargetComponent, storeIdxRID, true, out max, out statusReasonCode))
                    {
                        aStatusMsg =
                            new MIDException(eErrorLevel.severe,
                                (int)statusReasonCode,
                                MIDText.GetTextOnly(statusReasonCode)
                                + " : Header ID [" + aTargetAllocationProfile.HeaderID + "]"
                                + " : Source/Method [" + GetType().Name + " / CalculateGradeMaximumAllocation]");
                        return false;
                    }
                    // end TT#1176 - MD - Jellis - Group Allocation - Size Need not observing inv min max
                    //if (max <= aTargetAllocationProfile.GetStoreCapacityMaximum(storeIdxRID)                   // TT#1074 - MD - Jellis - Group Allocation - Inventory Min Max Broken
                    if (max <= aTargetAllocationProfile.GetStoreCapacityMaximum(aTargetComponent, storeIdxRID, true) // TT#1074 - MD - Jellis - Group Allocation - Inventory Min Max Broken 
                        && max <= aTargetAllocationProfile.GetStorePrimaryMaximum(aTargetComponent, storeIdxRID))
                    {
                        aRuleAllocationProfile.SetStoreRuleAllocation(storeIdxRID, eRuleType.Maximum, 0);
                    }
			    }
            }
            catch (Exception e)
            {
                aStatusMsg = new MIDException(eErrorLevel.severe, (int)eMIDTextCode.systemError, e.Message);
                return false;
            }
            finally
            {
            }
            return true;
        }

        /// <summary>
        /// Calculates a Color Maximum Rule Allocation
        /// </summary>
        /// <param name="aStoreList">List of Index_RID objects that identify the stores</param>
        /// <param name="aTargetAllocationProfile">Allocation Profile where the "Exact" rule allocation is to be calculated</param>
        /// <param name="aTargetComponent">Component on the Target Allocation Profile where the "Exact" rule allocation is to be calculated</param>
        /// <param name="aRuleAllocationProfile">The Rule Allocation Profile where the store rules are to be saved</param>
        /// <param name="aStatusMsg">Error message indicating why CalculateColorMaximumAllocation failed (null when CalculateColorMaximumAllocation succeeds)</param>
        /// <returns>True: when CalculateColorMaximumAllocation succeeds; False: when CalculateColorMaximumAllocation fails, in this case aStatusMsg indicates the reason for failure.</returns>   
         public bool CalculateColorMaximumAllocation(
                List<Index_RID> aStoreList,
                AllocationProfile aTargetAllocationProfile,
                GeneralComponent aTargetComponent,
                ref RuleAllocationProfile aRuleAllocationProfile,
                out MIDException aStatusMsg)
        {
            int max;
            aStatusMsg = null;
            try
            {
			    GeneralComponent colorComponent;
                switch (aTargetComponent.ComponentType)
			    {
				    case (eComponentType.ColorAndSize):
				    {
                        colorComponent = ((AllocationColorSizeComponent)aTargetComponent).ColorComponent;
					    break;
				    }
				    case (eComponentType.SpecificColor):
				    {
                        colorComponent = (AllocationColorOrSizeComponent)aTargetComponent;
					    break;
				    }
				    default:
				    {
					    colorComponent = null;
					    break;
				    }
			    }
			    if (colorComponent != null
				    && colorComponent.ComponentType == eComponentType.SpecificColor)
			    {
                    foreach (Index_RID storeIdxRID in aStoreList)
				    {
                        max = aTargetAllocationProfile.GetStoreColorMaximum(((AllocationColorOrSizeComponent)colorComponent).ColorRID, storeIdxRID);
                        //if (max <= aTargetAllocationProfile.GetStoreCapacityMaximum(storeIdxRID)                   // TT#1074 - MD - Jellis- Group Allocation - inventory Min Max Broken
                        if (max <= aTargetAllocationProfile.GetStoreCapacityMaximum(aTargetComponent, storeIdxRID, true) // TT#1074 - MD - Jellis - Group Allocation - Inventory Min Max Broken
                            && max <= aTargetAllocationProfile.GetStorePrimaryMaximum(colorComponent, storeIdxRID))
					    {
						    aRuleAllocationProfile.SetStoreRuleAllocation(storeIdxRID, eRuleType.ColorMaximum, 0);
					    }

				    }
			    }
			    else
			    {
				    string colorMaximum = MIDText.GetTextOnly((int)eRuleType.ColorMaximum);
                    string ruleComponent = MIDText.GetTextOnly((int)aTargetComponent.ComponentType);
				    string specificColor = MIDText.GetTextOnly((int)eComponentType.SpecificColor);
				    string message = string.Format
                        (_sab.ApplicationServerSession.Audit.GetText(eMIDTextCode.msg_al_CannotDetermineColorMinMaxUsingSpecifiedComponent, false),
                        aTargetAllocationProfile.HeaderID,
					    GetType().Name + "/" + "CalculateColorMaximumAllocation",
					    colorMaximum,
					    ruleComponent,
					    specificColor);
                    _sab.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Warning, message, this.GetType().Name);
				    aStatusMsg =
                        new MIDException(
                            eErrorLevel.warning,
                            (int)(eMIDTextCode.msg_al_CannotDetermineColorMinMaxUsingSpecifiedComponent),
				    	    message);
                    return false;
			    }
            }
            catch (Exception e)
            {
                aStatusMsg = new MIDException(eErrorLevel.severe, (int)eMIDTextCode.systemError, e.Message);
                return false;
            }
            finally
            {
            }
            return true;
        }

        /// <summary>
        /// Calculates an Absolute Quantity Rule Allocation
        /// </summary>
        /// <param name="aStoreList">List of Index_RID objects that identify the stores</param>
        /// <param name="aTargetAllocationProfile">Allocation Profile where the "Absolute Quantity" rule allocation is to be calculated</param>
        /// <param name="aTargetComponent">Component on the Target Allocation Profile where the "Exact" rule allocation is to be calculated.</param>
        /// <param name="aRuleQuantity">Absolute Quantity to be allocated to each store.</param>
        /// <param name="aRuleAllocationProfile">The Rule Allocation Profile where the store rules are to be saved</param>
         /// <param name="aStatusMsg">Error message indicating why CalculateAbsoluteQtyAllocation failed (null when CalculateAbsoluteQtyAllocation succeeds)</param>
        /// <returns>True: when CalculateColorMaximumAllocation succeeds; False: when CalculateColorMaximumAllocation fails, in this case aStatusMsg indicates the reason for failure.</returns>   
        public bool CalculateAbsoluteQtyAllocation(
                List<Index_RID> aStoreList,
                AllocationProfile aTargetAllocationProfile,
                GeneralComponent aTargetComponent,
                int aRuleQuantity,
                ref RuleAllocationProfile aRuleAllocationProfile,
                out MIDException aStatusMsg)
        {
            int qty;
            aStatusMsg = null;
            try
            {
                foreach (Index_RID storeIdxRID in aStoreList)
			    {
				    qty = (int)aRuleQuantity;
                    //if (qty <= aTargetAllocationProfile.GetStoreCapacityMaximum(storeIdxRID)                   // TT#1074 - MD - Jellis-  Group Allocation - Inventory Min Max Broken 
                    if (qty <= aTargetAllocationProfile.GetStoreCapacityMaximum(aTargetComponent, storeIdxRID, true) // TT#1074 - MD - Jellis - Group Allocation - Inventory Min Max Broken
                        && qty <= aTargetAllocationProfile.GetStorePrimaryMaximum(aTargetComponent, storeIdxRID))
				    {
					    aRuleAllocationProfile.SetStoreRuleAllocation(storeIdxRID, eRuleType.AbsoluteQuantity, aRuleQuantity);
				    }
			    }
            }
            catch (Exception e)
            {
                aStatusMsg = new MIDException(eErrorLevel.severe, (int)eMIDTextCode.systemError, e.Message);
                return false;
            }
            finally
            {
            }
            return true;
        }

        /// <summary>
        /// "No" rule Allocation
        /// </summary>
        /// <param name="aStoreList">List of Index_RID objects that identify the stores</param>
        /// <param name="aTargetAllocationProfile">Allocation Profile where the "Absolute Quantity" rule allocation is to be calculated</param>
        /// <param name="aTargetComponent">Component on the Target Allocation Profile where the "Exact" rule allocation is to be calculated.</param>
        /// <param name="aRuleAllocationProfile">The Rule Allocation Profile where the store rules are to be saved</param>
        /// <param name="aStatusMsg">Error message indicating why CalculateNoRuleAllocation failed (null when CalculateNoRuleAllocation succeeds)</param>
        /// <returns>True: when CalculateNoRuleAllocation succeeds; False: when CalculateNoRuleAllocation fails, in this case aStatusMsg indicates the reason for failure.</returns>  
        public bool CalculateNoRuleAllocation(
                List<Index_RID> aStoreList,
                AllocationProfile aTargetAllocationProfile,
                GeneralComponent aTargetComponent,
                ref RuleAllocationProfile aRuleAllocationProfile,
                out MIDException aStatusMsg)
        {
            aStatusMsg = null;
			try
			{

				// Nothing occurs in this case!

			}
			catch (Exception e)
			{
                aStatusMsg = new MIDException(eErrorLevel.severe, (int)eMIDTextCode.systemError, e.Message);
                return false;
			}
            finally
            {
            }
            return true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="aComponent"></param>
        /// <param name="aAllocProfile"></param>
        /// <returns></returns>
        private AllocationWorkMultiple GetQtyPerPack(GeneralComponent aComponent, AllocationProfile aAllocProfile)
        {
            AllocationWorkMultiple awm = new AllocationWorkMultiple(1, 1);

            if (aComponent.ComponentType == eComponentType.SpecificPack)
            {
                AllocationPackComponent apc = (AllocationPackComponent)aComponent;
                try
                {
                    if (aAllocProfile.PackIsOnHeader(apc.PackName))
                    {
                        awm.Multiple = aAllocProfile.GetPackMultiple(apc.PackName);
                        awm.Minimum = awm.Multiple;
                    }
                }
                catch
                {

                }
            }
            else if (aComponent.ComponentType == eComponentType.Total
                || aComponent.ComponentType == eComponentType.Bulk
                || aComponent.ComponentType == eComponentType.DetailType)
            {
                switch (aComponent.ComponentType)
                {
                    case (eComponentType.Total):
                        awm = aAllocProfile.GetAllocationWorkMultiple(eAllocationSummaryNode.Total);
                        break;
                    case (eComponentType.Bulk):
                        awm = aAllocProfile.GetAllocationWorkMultiple(eAllocationSummaryNode.Bulk);
                        break;
                    case (eComponentType.DetailType):
                        awm = aAllocProfile.GetAllocationWorkMultiple(eAllocationSummaryNode.DetailType);
                        break;
                }
            }

            return awm;
        }

        // begin TT#586 - MD - JEllis - Assortment - Add 2-dimensional Spread
        /// <summary>
        /// Spreads each row total in a list of rows proportionally by column using a list of column totals as the basis.
        /// </summary>
        /// <param name="aRowTotalList">List of Row Totals that are to be proportionally spread.</param>
        /// <param name="aColTotalList">List of Column Totals that are used as a basis during the spread and to which each column is to accumulate after the spread is complete</param>
        /// <param name="aConstraint">Constraints on the spread (including: current cell values that may or may not be locked, minimums, maximums and whether a particular columns is out(excluded) for a given row.</param>
        /// <param name="aSpreadResult">When the spread is successful, this object contains the results of the spread</param>
        /// <param name="aUnitsWereSpread">True: at least one Row had "new" units spread to at least one column.</param>
        /// <param name="aStatusMsg">If the spread fails, this message indicates what went wrong.</param>
        /// <returns>True: Spread was successful; False: Spread was not successful, aSpreadMsg will provide information on what went wrong</returns>
        /// <remarks>If there are any constraints on the spread, the spread may report that it succeeded BUT there may be some rows that did not spread completely due the specified constraints (this is not considered a failure because the spread did what it could without violating any constraint.).</remarks>
        public bool Do_2_DimensionalSpread(
            List<int> aRowTotalList,
            List<int> aColTotalList,
            ConstraintBin[,] aConstraint,
            out int[,] aSpreadResult,
            out bool aUnitsWereSpread,
            out MIDException aStatusMsg,
            bool bSpreadRemainingUnits = false  // TT#2049-MD - JSmith - Attached Detail PPK header with Qty less than PH the PH values are not the difference between the Orig PH value and the Header Allocation.
            )
        {
            aStatusMsg = null;
            aSpreadResult = null;
            bool status = true;
            aUnitsWereSpread = false;
            try
            {
                if (aRowTotalList.Count < 1)
                {
                    aStatusMsg = 
                        new MIDException(eErrorLevel.severe,
                            (int)eMIDTextCode.msg_al_SpreadFailed_NoRowsToProcess,
                            string.Format(MIDText.GetText(eMIDTextCode.msg_al_SpreadFailed_NoRowsToProcess),GetType().Name,"Do_2_DimensionalSpread"));
                    return false;
                }
                if (aColTotalList.Count < 1)
                {
                    aStatusMsg =
                        new MIDException(eErrorLevel.severe,
                            (int)eMIDTextCode.msg_al_SpreadFailed_NoColsToProcess,
                            string.Format(MIDText.GetText(eMIDTextCode.msg_al_SpreadFailed_NoColsToProcess),GetType().Name,"Do_2_DimensionalSpread"));
                    return false;
                }
                ConstraintBin[,] constraint = aConstraint;
                if (constraint == null)
                {
                    constraint = new ConstraintBin[aRowTotalList.Count, aColTotalList.Count];
					for (int r=0;r<aRowTotalList.Count;r++)
					{
						for (int c = 0; c < aColTotalList.Count; c++)
						{
							constraint[r, c] = new ConstraintBin();
						}
					}
                }
                if (constraint.GetLength(0) != aRowTotalList.Count        // TT#586 - MD - Jellis - Assortment add 2 dimensional spread
                    || constraint.GetLength(1) != aColTotalList.Count)   // TT#586 - MD - Jellis - Assortment add 2 dimensional spread
                {
                    string message =
                        string.Format(
                        MIDText.GetText(eMIDTextCode.msg_al_RowOrColVectorsIncapatilbeWithMatrix),
                        "aRowTotalList",
                        aRowTotalList.Count.ToString(), 
                        "aColTotalList",
                        aColTotalList.Count.ToString(),
                        "aConstraint",
                        constraint.GetLength(0).ToString(),    // TT#586 - MD - Jellis - Assortment add 2 dimensional spread                       
                        constraint.GetLength(1).ToString(),    // TT#586 - MD - Jellis - Assortment add 2 dimensional spread
                        GetType().Name,
                        "Do_2_DimensionalSpread");
                    aStatusMsg = 
                        new MIDException(eErrorLevel.severe,
                            (int)eMIDTextCode.msg_al_RowOrColVectorsIncapatilbeWithMatrix,
                            message);
                    return false;
                }

                aSpreadResult = new int[aRowTotalList.Count, aColTotalList.Count];

				MIDGenericSortItem[] sortedRow = new MIDGenericSortItem[aRowTotalList.Count];
				int rowTotal = 0;
                int[] rowUnitsToBreakOut = new int[aRowTotalList.Count];
                int[] colTotalAllocated = new int[aColTotalList.Count];
                colTotalAllocated.Initialize();
				for (int r=0; r<aRowTotalList.Count; r++)
				{
                    rowUnitsToBreakOut[r] = aRowTotalList[r];
                    for (int c=0; c<aColTotalList.Count; c++)
                    {
                        colTotalAllocated[c] += constraint[r, c].PriorAllocated; // TT#586 - MD - Jellis - Assortment add 2 dimensional spread
                        rowUnitsToBreakOut[r] -= constraint[r,c].PriorAllocated;
                        if (rowUnitsToBreakOut[r] < 0)
                        {
                            rowUnitsToBreakOut[r] = 0;
                        }
                    }
    				sortedRow[r].Item = r;
					sortedRow[r].SortKey = new double[2];
                    //sortedRow[r].SortKey[0] = (double) aRowTotalList[r];  // TT#994 - MD - Jellis - Size Balance Goes Negative 
                    sortedRow[r].SortKey[0] = (double)rowUnitsToBreakOut[r]; // TT#994 - MD - Jellis - Size Balance Goes Negative 
					sortedRow[r].SortKey[1] = _appSessionTran.GetRandomDouble();
					rowTotal += rowUnitsToBreakOut[r];
				}
                Array.Sort(sortedRow, new MIDGenericSortAscendingComparer());  // TT#1143 - MD - Jellis - Group Allocation Min Broken

                int colTotal = 0;
				MIDGenericSortItem[] sortedCol = new MIDGenericSortItem[aColTotalList.Count];
                MIDGenericSortAscendingComparer sortedAscendingComparer = new MIDGenericSortAscendingComparer();  // TT#1143 - MD- Jellis - Group Allocatio Min Broken
                int[] adjColUnitsToBreakout = new int[aColTotalList.Count];
                for (int c=0; c<aColTotalList.Count; c++)
                {
                    adjColUnitsToBreakout[c] = aColTotalList[c];
                    if (adjColUnitsToBreakout[c] < 0)
                    {
                        adjColUnitsToBreakout[c] = 0;
                    }
                    sortedCol[c].Item = c;
                    sortedCol[c].SortKey = new double[2];
                    sortedCol[c].SortKey[0] = (double)adjColUnitsToBreakout[c];
                    sortedCol[c].SortKey[1] = _appSessionTran.GetRandomDouble();
                    colTotal += adjColUnitsToBreakout[c];
                }
                if (rowTotal > colTotal) 
                    // There are more units in the row to spread by col than there are units in the col
                    // i.e. during the spread we'll run out of "col units".
                    // So, Increase col totals proportionally to equal row total
                {
                    int j;
                    Array.Sort(sortedCol, sortedAscendingComparer);
                    int newTotal = rowTotal;
                    int oldTotal = colTotal;
                    int wrkTotal;
                    foreach (MIDGenericSortItem mgsiSize in sortedCol)
                    {
                        j = mgsiSize.Item;
                        if (oldTotal > 0)
                        {
                            wrkTotal = oldTotal;
                            oldTotal -= adjColUnitsToBreakout[j];
                            adjColUnitsToBreakout[j] =
                                (int)((((double)adjColUnitsToBreakout[j]
                                        * (double)newTotal
                                        / (double)wrkTotal)
                                        + .5d));
                        }
                        newTotal -= adjColUnitsToBreakout[j];
                        if (newTotal < 0)
                        {
                            newTotal = 0;
                        }
                    }
                }

                int[] colUnitsToBreakout = new int[aColTotalList.Count];
                foreach (MIDGenericSortItem mgsiRow in sortedRow)
                {
                    colTotal = 0;
                    // Set up columns for this row.
                    for (int c = 0; c < aColTotalList.Count; c++)
                    {
                        colUnitsToBreakout[c] =
                            adjColUnitsToBreakout[c]
                            - colTotalAllocated[c];  // TT#586 - MD - Jellis Assortment add 2 dimensional spread
                            //- constraint[mgsiRow.Item, c].PriorAllocated;  // TT#586 - MD - Jellis Assortment add 2 dimensional spread
                        if (colUnitsToBreakout[c] < 0)
                        {
                            colUnitsToBreakout[c] = 0;
                        }

                        if (constraint[mgsiRow.Item, c].PriorAllocated > constraint[mgsiRow.Item, c].Maximum)
                        {
                            colUnitsToBreakout[c] = 0;
                        }
                        if (constraint[mgsiRow.Item, c].IsLocked
                            || constraint[mgsiRow.Item, c].IsOut)
                        {
                            colUnitsToBreakout[c] = 0;
                        }
                        sortedCol[c].Item = c;
                        sortedCol[c].SortKey = new double[2];
                        sortedCol[c].SortKey[0] = (double)colUnitsToBreakout[c];
                        sortedCol[c].SortKey[1] = _appSessionTran.GetRandomDouble();
                        colTotal += colUnitsToBreakout[c];
                    }
                    Array.Sort(sortedCol, new MIDGenericSortAscendingComparer()); // TT#1143 - MD - Jellis - Group  Allocation Min Broken

                    int allocated;
                    int cellAllocated;
                    int cellCurrentAllocated;
                    int newTotal = (int)mgsiRow.SortKey[0];
                    foreach (MIDGenericSortItem mgsiCol in sortedCol)
                    {
                        if (colTotal > 0)
                        {
                            allocated =
                                (int)((mgsiCol.SortKey[0] * (double)newTotal / (double)colTotal) + .5d);
                            allocated = (int)(((double)allocated / (double)constraint[mgsiRow.Item, mgsiCol.Item].Multiple) + .5d);
                            allocated = allocated * constraint[mgsiRow.Item, mgsiCol.Item].Multiple;
                            if (allocated > newTotal)
                            {
                                allocated = newTotal;
                                allocated = (int)(((double)allocated / (double)constraint[mgsiRow.Item, mgsiCol.Item].Multiple));
                                allocated = allocated * constraint[mgsiRow.Item, mgsiCol.Item].Multiple;
                            }
                            cellCurrentAllocated = constraint[mgsiRow.Item, mgsiCol.Item].PriorAllocated;
                            cellAllocated = cellCurrentAllocated + allocated;
                            if (cellAllocated < constraint[mgsiRow.Item, mgsiCol.Item].Minimum)
                            {
                                cellAllocated = cellCurrentAllocated;
                                allocated = 0;
                            }
                            else if (cellAllocated > constraint[mgsiRow.Item, mgsiCol.Item].Maximum) 
                            {
                                if (cellCurrentAllocated < constraint[mgsiRow.Item, mgsiCol.Item].Maximum)
                                {
                                    cellAllocated = constraint[mgsiRow.Item, mgsiCol.Item].Maximum;
                                    allocated =
                                        constraint[mgsiRow.Item, mgsiCol.Item].Maximum
                                        - cellCurrentAllocated;
                                }
                                else
                                {
                                    cellAllocated = cellCurrentAllocated;
                                    allocated = 0;
                                }
                            }

                            aSpreadResult[mgsiRow.Item, mgsiCol.Item] = cellAllocated;
                            if (allocated > 0)
                            {
                                colTotalAllocated[mgsiCol.Item] += allocated; // TT#586 - MD - Jellis Assortment add 2 dimensional spread
                                aUnitsWereSpread = true;
                            }
                        }
                        else
                        {
                            aSpreadResult[mgsiRow.Item, mgsiCol.Item] = constraint[mgsiRow.Item, mgsiCol.Item].PriorAllocated; // TT#1064 - MD - Jellis - Cannot  Release Group Allocation
                            allocated = 0;
                        }
                        newTotal -= allocated;
                        colTotal -= (int)mgsiCol.SortKey[0];
                        if (newTotal < 0)
                        {
                            newTotal = 0;
                        }
                        if (colTotal < 0)
                        {
                            colTotal = 0;
                        }
                    }
                }
                // Begin TT#2049-MD - JSmith - Attached Detail PPK header with Qty less than PH the PH values are not the difference between the Orig PH value and the Header Allocation.
                if (bSpreadRemainingUnits)
                {
                    SpreadRemainingUnits(aRowTotalList, aColTotalList, aConstraint, ref aSpreadResult, colTotalAllocated);
                }
                // End TT#2049-MD - JSmith - Attached Detail PPK header with Qty less than PH the PH values are not the difference between the Orig PH value and the Header Allocation.
                status = true;
            }
            catch (Exception e)
            {
                status = false;
                aStatusMsg = new MIDException(eErrorLevel.severe, (int)eMIDTextCode.systemError, e); // TT#586 - MD - Jellis - Assortment add 2 dimensional spread
            }
            finally
            {
            }
            return status;
        }
        // end TT#586 - MD - Jellis - Assortment - Add 2-dimensional Spread

        // Begin TT#2049-MD - JSmith - Attached Detail PPK header with Qty less than PH the PH values are not the difference between the Orig PH value and the Header Allocation.
        private bool SpreadRemainingUnits(
            List<int> aRowTotalList,
            List<int> aColTotalList,
            ConstraintBin[,] aConstraint,
            ref int[,] aSpreadResult,
            int[] colTotalAllocated
            )
        {
            bool unitsRemaining = false;
            bool unitsAllocated = true;
            int cellAllocated;

            while (unitsAllocated)
            {
                unitsAllocated = false;
                for (int c = 0; c < aColTotalList.Count; c++)
                {
                    if (colTotalAllocated[c] < aColTotalList[c])
                    {
                        unitsRemaining = true;
                        break;
                    }
                }

                if (unitsRemaining)
                {
                    // Give remaining units to stores with lowest total first
                    ConstraintBin[,] constraint = aConstraint;
                    MIDGenericSortItem[] sortedRow = new MIDGenericSortItem[aRowTotalList.Count];
                    int rowTotal = 0;
                    int[] rowUnitsToBreakOut = new int[aRowTotalList.Count];
                    colTotalAllocated.Initialize();
                    for (int r = 0; r < aRowTotalList.Count; r++)
                    {
                        rowUnitsToBreakOut[r] = aRowTotalList[r];
                        for (int c = 0; c < aColTotalList.Count; c++)
                        {
                            rowUnitsToBreakOut[r] -= aSpreadResult[r, c];
                            if (rowUnitsToBreakOut[r] < 0)
                            {
                                rowUnitsToBreakOut[r] = 0;
                            }
                        }
                        sortedRow[r].Item = r;
                        sortedRow[r].SortKey = new double[2];
                        sortedRow[r].SortKey[0] = (double)(aRowTotalList[r] - rowUnitsToBreakOut[r]);
                        sortedRow[r].SortKey[1] = _appSessionTran.GetRandomDouble();
                        rowTotal += rowUnitsToBreakOut[r];
                    }
                    Array.Sort(sortedRow, new MIDGenericSortAscendingComparer());
                    foreach (MIDGenericSortItem mgsiRow in sortedRow)
                    {
                        if (rowUnitsToBreakOut[mgsiRow.Item] > 0)
                        {
                            for (int c = 0; c < aColTotalList.Count; c++)
                            {
                                if (colTotalAllocated[c] < aColTotalList[c])
                                {
                                    if (rowUnitsToBreakOut[mgsiRow.Item] >= constraint[mgsiRow.Item, c].Multiple)
                                    {
                                        cellAllocated = aSpreadResult[mgsiRow.Item, c] + constraint[mgsiRow.Item, c].Multiple;
                                        if (cellAllocated <= constraint[mgsiRow.Item, c].Maximum)
                                        {
                                            aSpreadResult[mgsiRow.Item, c] = cellAllocated;
                                            rowUnitsToBreakOut[mgsiRow.Item] -= constraint[mgsiRow.Item, c].Multiple;
                                            colTotalAllocated[c] += constraint[mgsiRow.Item, c].Multiple;
                                            unitsAllocated = true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return true;
        }
        // End TT#2049-MD - JSmith - Attached Detail PPK header with Qty less than PH the PH values are not the difference between the Orig PH value and the Header Allocation.
	}
}
