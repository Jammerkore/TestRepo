using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

using MIDRetail.Data;
using MIDRetail.Common;
using MIDRetail.DataCommon;

namespace MIDRetail.Business.Allocation
{
	/// <summary>
	/// Defines the general criteria that drives the allocation process.
	/// </summary>
	/// <remarks>
	/// </remarks>
    public class DCCartonRoundingMethod : AllocationBaseMethod
    {
        //=======
        // FIELDS
        //=======
        private DCCartonRoundingMethodData _methodData;
        private eAllocateOverageTo _APPLY_OVERAGE_TO;
        private Audit _audit;
        private List<DCInfo> _DCInfoList = new List<DCInfo>();	// TT#1698-MD - stodd - Recieve Warning Error (total not spread) when processing Carton Rounding on Headers within a Group 
        private bool _DCIncludesReserveOnly;					// TT#1698-MD - stodd - Recieve Warning Error (total not spread) when processing Carton Rounding on Headers within a Group
		// Begin TT#1708-MD - stodd - Allocation is out of balance after DC Carton Rounding to Reserve 
        private Dictionary<int, int> _storeOddUnitList = new Dictionary<int, int>();
        private StoreGroupProfile _sgp;
		// End TT#1708-MD - stodd - Allocation is out of balance after DC Carton Rounding to Reserve

        //=============
        // CONSTRUCTORS
        //=============
        public DCCartonRoundingMethod(SessionAddressBlock SAB, int aMethodRID)
            : base(SAB, aMethodRID, eMethodType.DCCartonRounding, eProfileType.MethodDCCartonRounding)
        {
            if (base.Filled)
            {
                _methodData = new DCCartonRoundingMethodData(base.Key, eChangeType.populate);
                _APPLY_OVERAGE_TO = _methodData.ApplyOverageTo;

            }
            else
            {
                SG_RID = SAB.ClientServerSession.GlobalOptions.DCCartonRoundingSGRid;
                _APPLY_OVERAGE_TO = eAllocateOverageTo.AllocatedStores;
            }
        }

        //============
        // PROPERTIES
        //============
        /// <summary>
        /// Gets the ProfileType of this profile.
        /// </summary>
        override public eProfileType ProfileType
        {
            get
            {
                return eProfileType.MethodDCCartonRounding;
            }
        }

        /// <summary>
        /// Gets or sets the Allocate Overage To.
        /// </summary>
        public eAllocateOverageTo APPLY_OVERAGE_TO
        {
            get { return _APPLY_OVERAGE_TO; }
            set { _APPLY_OVERAGE_TO = value; }
        }

        //========
        // METHODS
        //========
        public override void ProcessMethod(
            ApplicationSessionTransaction aApplicationTransaction,
            int aStoreFilter, Profile methodProfile)
        {
            aApplicationTransaction.ResetAllocationActionStatus();

            foreach (AllocationProfile ap in (AllocationProfileList)aApplicationTransaction.GetMasterProfileList(eProfileType.Allocation))
            {
                AllocationWorkFlowStep awfs =
                    new AllocationWorkFlowStep(
                    this,
                    new GeneralComponent(eGeneralComponentType.Total),
                    false,
                    true,
                    aApplicationTransaction.SAB.ApplicationServerSession.GlobalOptions.BalanceTolerancePercent,
                    aStoreFilter,
                    -1);
                this.ProcessAction(
                    aApplicationTransaction.SAB,
                    aApplicationTransaction,
                    awfs,
                    ap,
                    true,
                    Include.NoRID);
            }
        }

        /// <summary>
        /// Processes the action associated with this method.
        /// </summary>
        /// <param name="aSAB">Session Address Block</param>
        /// <param name="aApplicationTransaction">An instance of the Application Transaction object</param>
        /// <param name="aAllocationWorkFlowStep">Workflow Step that describes parameters associated with this action.</param>
        /// <param name="aAllocationProfile">Allocation Profile to which to apply this action</param>
        /// <param name="WriteToDB">True: write results of action to database; False: Do not write results of action to database.</param>
        public override void ProcessAction(
            SessionAddressBlock aSAB,
            ApplicationSessionTransaction aApplicationTransaction,
            ApplicationWorkFlowStep aAllocationWorkFlowStep,
            Profile aAllocationProfile,
            bool WriteToDB,
            int aStoreFilterRID)
        {
            bool actionSuccess = true;
            AllocationProfile ap = aAllocationProfile as AllocationProfile;
            _audit = aSAB.ApplicationServerSession.Audit;
            _DCInfoList.Clear(); // TT#1690-MD - stodd - Multiple Headers with DC Carton Rounding 
            _DCIncludesReserveOnly = false;		// TT#1698-MD - stodd - Recieve Warning Error (total not spread) when processing Carton Rounding on Headers within a Group
            _storeOddUnitList.Clear();		// TT#1708-MD - stodd - Allocation is out of balance after DC Carton Rounding to Reserve

            if (ap == null)
            {
                string auditMsg = MIDText.GetText(eMIDTextCode.msg_NotAllocationProfile);
                _audit.Add_Msg(
                    eMIDMessageLevel.Severe,
                    eMIDTextCode.msg_NotAllocationProfile,
                    auditMsg,
                    this.GetType().Name);
                throw new MIDException(eErrorLevel.severe,
                    (int)(eMIDTextCode.msg_NotAllocationProfile),
                    auditMsg);
            }

            try
            {
                ap.ResetTempLocks(false);
                if (!Enum.IsDefined(typeof(eAllocationMethodType), (int)aAllocationWorkFlowStep._method.MethodType))
                {
                    throw new MIDException(eErrorLevel.severe,
                        (int)(eMIDTextCode.msg_WorkflowTypeInvalid),
                        MIDText.GetText(eMIDTextCode.msg_WorkflowTypeInvalid));
                }

                // Is Header Valid for DC Carton Rounding?
                // Begin TT#4911 - JSmith - Auto Release Workflow Fail on Intransit Charge
                bool isOKToSkip;
                //if (isValidHeader(ap, aApplicationTransaction))
                if (isValidHeader(ap, aApplicationTransaction, aAllocationWorkFlowStep, out isOKToSkip))
                // End TT#4911 - JSmith - Auto Release Workflow Fail on Intransit Charge
                {
                    //==================================
                    // Finally Do DC Carton Rounding
                    //==================================
                    int workFlowRID = ap.API_WorkflowRID;
                    bool workFlowTrigger = ap.API_WorkflowTrigger;

                    //=============================================
                    // Determine DCs and units allocated per DC
                    //=============================================
                    actionSuccess = LoadDCInformation(ap);	// TT#1718-MD - stodd - Processing DC Carton Rounding changed status to Allocated Out of Balance 

                    if (actionSuccess && isValidDCInfo(ap, aApplicationTransaction))	// TT#1718-MD - stodd - Processing DC Carton Rounding changed status to Allocated Out of Balance 
                    {
                        // Determine leftover units
                        int leftoverUnits = GetCartonRoundingLeftoverUnits(ap);

                        // Adjust the Receiving DC by the leftover units and return new adjusted total
                        int adjTotalUnits = SubtractLeftoverUnits(leftoverUnits);

                        // Sort DCs by ascending total units
                        SortDCsAscending(aApplicationTransaction);

                        // Spread adjusted value using Units per Carton as the multiple
                        SpreadAdjustedTotal(adjTotalUnits, ap.TotalUnitsAllocated, ap.UnitsPerCarton);

                        // Add leftover units back to receiving DC
                        AddBackLeftoverUnits(leftoverUnits);

                        DebugDCList("Preliminary DC Info");		// TT#1708-MD - stodd - Allocation is out of balance after DC Carton Rounding to Reserve

                        // Final Spread to Transfer (non-receiving DC) DCs
                        SpreadTransferDCTotalsToStores(ap);

                        // Final Spread to receiving DC. This is where more of the real work is done.
                        SpreadToReceivingDC(ap);

                        ReloadDCAllocatedTotals(ap);    // TT#1708-MD - stodd - Allocation is out of balance after DC Carton Rounding to Reserve
                        DebugDCList("Final DC Info");			// TT#1708-MD - stodd - Allocation is out of balance after DC Carton Rounding to Reserve

                        // _audit.Add_Msg(eMIDMessageLevel.Information, eMIDTextCode.msg_al_DCCartonRoundingApplied, this.Name + " " + ap.HeaderID, this.GetType().Name);     // TT#1699-MD - stodd - DC Carton Rounding is missing an information message stating it was run against a specific header
                        // Begin TT#1699-MD - stodd - DC Carton Rounding is missing an information message stating it was run against a specific header
                        string infoMsg = string.Format(_audit.GetText(eMIDTextCode.msg_al_DCCartonRoundingApplied, false), this.Name, ap.HeaderID);
                        _audit.Add_Msg(eMIDMessageLevel.Information, eMIDTextCode.msg_al_DCCartonRoundingApplied, infoMsg, this.GetType().Name);
                        // End TT#1699-MD - stodd - DC Carton Rounding is missing an information message stating it was run against a specific header

                        // Begin TT#4957 - JSmith - Carton Rounding Method Triggering Auto Release
                        //if (ap.API_WorkflowRID != Include.UndefinedWorkflowRID)
                        //{
                        //    ap.WorkflowTrigger = true;
                        //}
                        // End TT#4957 - JSmith - Carton Rounding Method Triggering Auto Release
                        if (actionSuccess)
                        {
                            if (WriteToDB
                                || ap.WorkflowTrigger == true)
                            {
                                actionSuccess = ap.WriteHeader();
                            }
                            if (actionSuccess)
                            {
                                aApplicationTransaction.SetAllocationActionStatus(ap.Key, eAllocationActionStatus.ActionCompletedSuccessfully);

                                aApplicationTransaction.WriteAllocationAuditInfo
                                    (ap.Key,
                                    0,
                                    this.MethodType,
                                    this.Key,
                                    this.Name,
                                    eComponentType.Total,
                                    null,
                                    null,
                                    0,
                                    0
                                    );
                            }
                            else
                            {
                                aApplicationTransaction.SetAllocationActionStatus(ap.Key, eAllocationActionStatus.ActionFailed);
                            }
                        }
                        else
                        {
                            aApplicationTransaction.SetAllocationActionStatus(ap.Key, eAllocationActionStatus.ActionFailed);
                        }
                    }
                    else
                    {
                        aApplicationTransaction.SetAllocationActionStatus(ap.Key, eAllocationActionStatus.ActionFailed);
                    }
                }
                else
                {
				    // Begin TT#4911 - JSmith - Auto Release Workflow Fail on Intransit Charge
                    // Set ActionFailed 
                    //aApplicationTransaction.SetAllocationActionStatus(ap.Key, eAllocationActionStatus.ActionFailed);
                    if (aAllocationWorkFlowStep.Key == Include.NoRID ||
                        !isOKToSkip)
                    {
                        aApplicationTransaction.SetAllocationActionStatus(ap.Key, eAllocationActionStatus.ActionFailed);
                    }
                    else
                    {
                        aApplicationTransaction.SetAllocationActionStatus(ap.Key, eAllocationActionStatus.ActionCompletedSuccessfully);
                    }
                    // End TT#4911 - JSmith - Auto Release Workflow Fail on Intransit Charge
                }
            }
            catch (Exception error)
            {
                aApplicationTransaction.SetAllocationActionStatus(ap.Key, eAllocationActionStatus.ActionFailed);
                string message = error.ToString();
                throw;
            }
            finally
            {
                ap.ResetTempLocks(true); 
            }
        }

        /// <summary>
        /// Total Allocated Untis / Units Per Carton returns leftover Units.
        /// </summary>
        /// <param name="ap"></param>
        /// <returns></returns>
        private int GetCartonRoundingLeftoverUnits(AllocationProfile ap)
        {
            try
            {
                int leftoverUnits = 0;
                int wholeCartons = 0;
                if (ap.UnitsPerCarton != 0)
                {
                    wholeCartons = ap.TotalUnitsAllocated / ap.UnitsPerCarton;
                }
                int wholeCartonUnits = wholeCartons * ap.UnitsPerCarton;
                leftoverUnits = ap.TotalUnitsAllocated - wholeCartonUnits;
                return leftoverUnits;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Subtracts leftover units from Receiving DC.
        /// </summary>
        /// <param name="leftoverUnits"></param>
        private int SubtractLeftoverUnits(int leftoverUnits)
        {
            try
            {
                int adjTotalUnitsAllocated = 0;
                foreach (DCInfo aDCInfo in _DCInfoList)
                {
                    if (aDCInfo.IsRecievingDC)
                    {
                        aDCInfo.TotalUnitsAllocated -= leftoverUnits;
                    }

                    adjTotalUnitsAllocated += aDCInfo.TotalUnitsAllocated;
                }

                return adjTotalUnitsAllocated;
            }
            catch
            {
                throw;
            }
        }

        private void SortDCsAscending(ApplicationSessionTransaction aApplicationTransaction)
        {
            try
            {
                List<DCInfo> sortedDCInfoList = new List<DCInfo>();
                MIDGenericSortItem[] sortedDC = new MIDGenericSortItem[_DCInfoList.Count];
                MIDGenericSortAscendingComparer sortedAscendingComparer = new MIDGenericSortAscendingComparer();

                for (int i = 0; i < _DCInfoList.Count; i++)
                {
                    DCInfo dcInfo = _DCInfoList[i];
                    sortedDC[i].Item = i;
                    sortedDC[i].SortKey = new double[2];
                    sortedDC[i].SortKey[0] = dcInfo.TotalUnitsAllocated;
                    sortedDC[i].SortKey[1] = aApplicationTransaction.GetRandomDouble();
                }

                Array.Sort(sortedDC, sortedAscendingComparer);
                int j = 0;
                foreach (MIDGenericSortItem sortColor in sortedDC)
                {
                    j = sortColor.Item;
                    sortedDCInfoList.Add(_DCInfoList[j]);
                }

                _DCInfoList = sortedDCInfoList;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Sorts stores descending by allocated units
        /// </summary>
        /// <param name="ap"></param>
        /// <param name="storeList"></param>
        /// <param name="aApplicationTransaction"></param>
        /// <returns></returns>
        private List<StoreProfile> SortStoresDescending(AllocationProfile ap, ProfileList storeList, ApplicationSessionTransaction aApplicationTransaction)
        {
            try
            {

                List<StoreProfile> sortedStoreList = new List<StoreProfile>();
                MIDGenericSortItem[] sortedStore = new MIDGenericSortItem[storeList.Count];
                MIDGenericSortDescendingComparer sortedDescendingComparer = new MIDGenericSortDescendingComparer();

                for (int i = 0; i < storeList.Count; i++)
                {
                    StoreProfile aStore = (StoreProfile)storeList[i];
                    sortedStore[i].Item = i;
                    sortedStore[i].SortKey = new double[2];
                    sortedStore[i].SortKey[0] = ap.GetStoreQtyAllocated(eAllocationSummaryNode.Total, aStore.Key);
                    sortedStore[i].SortKey[1] = aApplicationTransaction.GetRandomDouble();
                }

                Array.Sort(sortedStore, sortedDescendingComparer);
                int j = 0;
                foreach (MIDGenericSortItem sortStore in sortedStore)
                {
                    j = sortStore.Item;
                    sortedStoreList.Add((StoreProfile)storeList[j]);
                }

                return sortedStoreList;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Sort stores by difference (loss) between original allocation and current allocation.
        /// </summary>
        /// <param name="ap"></param>
        /// <param name="storeList"></param>
        /// <param name="aApplicationTransaction"></param>
        /// <returns></returns>
        private List<StoreProfile> SortStoresDescendingByDifference(AllocationProfile ap, Dictionary<int, int> storeLossDic, ProfileList storeList, ApplicationSessionTransaction aApplicationTransaction)
        {
            try
            {

                List<StoreProfile> sortedStoreList = new List<StoreProfile>();
                MIDGenericSortItem[] sortedStore = new MIDGenericSortItem[storeList.Count];
                MIDGenericSortDescendingComparer sortedDescendingComparer = new MIDGenericSortDescendingComparer();

                for (int i = 0; i < storeList.Count; i++)
                {
                    StoreProfile aStore = (StoreProfile)storeList[i];
                    sortedStore[i].Item = i;
                    sortedStore[i].SortKey = new double[2];
                    sortedStore[i].SortKey[0] = storeLossDic[aStore.Key];
                    sortedStore[i].SortKey[1] = aApplicationTransaction.GetRandomDouble();
                }

                Array.Sort(sortedStore, sortedDescendingComparer);
                int j = 0;
                foreach (MIDGenericSortItem sortStore in sortedStore)
                {
                    j = sortStore.Item;
                    sortedStoreList.Add((StoreProfile)storeList[j]);
                }

                return sortedStoreList;
            }
            catch
            {
                throw;
            }
        }


        /// <summary>
        /// Spreads the adjusted total back to the DCs using a multiple of Units Per Carton.
        /// </summary>
        /// <param name="aNewDCTotal"></param>
        /// <param name="aoldDCTotal"></param>
        /// <param name="unitsPerCarton"></param>
        private void SpreadAdjustedTotal(double aNewDCTotal, double aoldDCTotal, int unitsPerCarton)
        {
            int DCAllocated = 0;
			// Begin TT#1698-MD - stodd - Recieve Warning Error (total not spread) when processing Carton Rounding on Headers within a Group
            DCInfo receivingDC = null;
            DCInfo reserveDC = null;
			// End TT#1698-MD - stodd - Recieve Warning Error (total not spread) when processing Carton Rounding on Headers within a Group

            try
            {
				// Begin TT#1698-MD - stodd - Recieve Warning Error (total not spread) when processing Carton Rounding on Headers within a Group
                // If a DC's only allocation is to the reserve store, special processing is needed.
                // The reserve total is added to the Receiving DC's total for spreading purposes.
                // After the spread, the reserve total will be subtracted from the Receiving DC's total.
                if (_DCIncludesReserveOnly)
                {
                    foreach (DCInfo aDC in _DCInfoList)
                    { 
                        if (aDC.IsRecievingDC)
                        {
                            receivingDC = aDC;
                        }
                        if (aDC.IncludesReserveOnly)
                        {
                            reserveDC = aDC;
                        }
                    }

                    if (receivingDC != null && reserveDC != null)
                    {
                        receivingDC.OrigUnitsAllocated += reserveDC.OrigUnitsAllocated;
                        receivingDC.TotalUnitsAllocated += reserveDC.OrigUnitsAllocated;
                    }
                }
				// End TT#1698-MD - stodd - Recieve Warning Error (total not spread) when processing Carton Rounding on Headers within a Group

                foreach (DCInfo aDC in _DCInfoList)
                {
					// Begin TT#1698-MD - stodd - Recieve Warning Error (total not spread) when processing Carton Rounding on Headers within a Group
                    // We skip this DC. It's total has been added to the receiving DC for the Spread.
                    if (aDC.IncludesReserveOnly)
                    {
                        continue;
                    }
					// End TT#1698-MD - stodd - Recieve Warning Error (total not spread) when processing Carton Rounding on Headers within a Group

                    if (aoldDCTotal > 0)
                    {
                        DCAllocated =
                            (int)(((double)aDC.OrigUnitsAllocated                   
                            * (double)aNewDCTotal
                            / (double)aoldDCTotal) + 0.5d);

                        DCAllocated =
                            (int)(((double)DCAllocated
                            / (double)unitsPerCarton) + 0.5d);

                        DCAllocated = (DCAllocated * unitsPerCarton);

                        while (DCAllocated > aNewDCTotal)
                        {
                            DCAllocated -= unitsPerCarton;
                        }
                    }
                    else
                    {
                        DCAllocated = 0;
                    }
                    aoldDCTotal -= aDC.OrigUnitsAllocated;
                    aNewDCTotal -= DCAllocated;
                    aDC.TotalUnitsAllocated = DCAllocated;
                    if (aNewDCTotal < 0)
                    {
                        aNewDCTotal = 0;
                    }
                }

				// Begin TT#1698-MD - stodd - Recieve Warning Error (total not spread) when processing Carton Rounding on Headers within a Group
                // Subtract back out the reserve units from the receiving DC.
                if (_DCIncludesReserveOnly)
                {
                    if (receivingDC != null && reserveDC != null)
                    {
                        receivingDC.OrigUnitsAllocated -= reserveDC.OrigUnitsAllocated;
                        receivingDC.TotalUnitsAllocated -= reserveDC.OrigUnitsAllocated;

                        reserveDC.TotalUnitsAllocated = reserveDC.OrigUnitsAllocated;

                    }
                }
				// End TT#1698-MD - stodd - Recieve Warning Error (total not spread) when processing Carton Rounding on Headers within a Group
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Adds the leftover units back to the Receiving DC.
        /// </summary>
        /// <param name="leftoverUnits"></param>
        private void AddBackLeftoverUnits(int leftoverUnits)
        {
            foreach (DCInfo aDC in _DCInfoList)
            {
                if (aDC.IsRecievingDC)
                {
                    aDC.TotalUnitsAllocated += leftoverUnits;
                }
            }
        }

        /// <summary>
        /// Returns Receiving DC information from DC list.
        /// </summary>
        /// <returns></returns>
        private DCInfo GetReceivingDC()
        {
            DCInfo recDC = null;
            foreach (DCInfo aDC in _DCInfoList)
            {
                if (aDC.IsRecievingDC)
                {
                    recDC = aDC;
                    break;
                }
            }
            return recDC;
        }

        /// <summary>
        /// Spreads the DC total for Transfer DCs (non-receiving DCs) to stores.
        /// </summary>
        /// <param name="ap"></param>
        private void SpreadTransferDCTotalsToStores(AllocationProfile ap)
        {
            try
            {
                foreach (DCInfo aDC in _DCInfoList)
                {
                    // ONLY spread to non-receiving DCs...that have values in stores other than the Reserve store
                    if (!aDC.IsRecievingDC && !aDC.IncludesReserveOnly)		// TT#1698-MD - stodd - Recieve Warning Error (total not spread) when processing Carton Rounding on Headers within a Group
                    {
                        if (ap.PackCount > 0)
                        {
                            ap.SpreadStoreListTotalQtyAllocated(new GeneralComponent(eComponentType.Total), aDC.Stores, aDC.TotalUnitsAllocated);
                        }
                        else if (ap.BulkColors.Count > 0)
                        {
                            ap.SpreadStoreListTotalQtyAllocated(new GeneralComponent(eComponentType.Bulk), aDC.Stores, aDC.TotalUnitsAllocated);
                        }
                        else
                        {
                            ap.SpreadStoreListTotalQtyAllocated(new GeneralComponent(eComponentType.Total), aDC.Stores, aDC.TotalUnitsAllocated);
                        }
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Processes spread for Receiving DC.
        /// </summary>
        /// <param name="ap"></param>
        private void SpreadToReceivingDC(AllocationProfile ap)
        {
            DCInfo recDC = GetReceivingDC();
            int UnitsAllocatedChange = recDC.TotalUnitsAllocated - recDC.OrigUnitsAllocated;

            string msg = "(DC Carton Rounding for header " + ap.HeaderID + ") Amount of change in allocated units from the original allocation to the allocation after DC Carton Rounding for Receiving DC: " + UnitsAllocatedChange;
            _audit.Add_Msg(eMIDMessageLevel.Debug, msg, this.GetType().Name);

            // PACK
            if (ap.PackCount > 0)
            {
                SpreadToReceivingDCWithPack(ap, recDC, UnitsAllocatedChange);
            }
            // BULK
            else if (ap.BulkColors.Count > 0)
            {
                ProcessReceivingDCWithBulkOrTotal(ap, recDC, UnitsAllocatedChange, true);	// TT#1702-MD - DC Carton Rounding goes out of balance with Odd units 
            }
            // TOTAL
            else
            {
                ProcessReceivingDCWithBulkOrTotal(ap, recDC, UnitsAllocatedChange, false);	// TT#1702-MD - DC Carton Rounding goes out of balance with Odd units 
            }
        }

        /// <summary>
        /// Spread to header with a pack.
        /// </summary>
        /// <param name="ap"></param>
        /// <param name="recDC"></param>
        /// <param name="UnitsAllocatedChange"></param>
        private void SpreadToReceivingDCWithPack(AllocationProfile ap, DCInfo recDC, int UnitsAllocatedChange)
        {
            try
            {
                string msg = string.Empty;
                // Allocation decreased after carton rounding
                if (UnitsAllocatedChange < 1)
                {
                    ap.SpreadStoreListTotalQtyAllocated(new GeneralComponent(eComponentType.Total), recDC.Stores, recDC.TotalUnitsAllocated);
                    msg = "(DC Carton Rounding for header " + ap.HeaderID + ") For Receiving DC: Spread new DC total allocated of " + recDC.TotalUnitsAllocated + " to stores.";
                    _audit.Add_Msg(eMIDMessageLevel.Debug, msg, this.GetType().Name);
                }
                else // Allocated units increased after carton rounding
                {
                    if (_APPLY_OVERAGE_TO == eAllocateOverageTo.AllocatedStores)
                    {
                        ap.SpreadStoreListTotalQtyAllocated(new GeneralComponent(eComponentType.Total), recDC.Stores, recDC.TotalUnitsAllocated);
                        msg = "(DC Carton Rounding for header " + ap.HeaderID + ") For Receiving DC: Spread new DC total allocated of " + recDC.TotalUnitsAllocated + " to stores.";
                        _audit.Add_Msg(eMIDMessageLevel.Debug, msg, this.GetType().Name);
                    }
                    else if (_APPLY_OVERAGE_TO == eAllocateOverageTo.Reserve)
                    {
                        ap.Action(eAllocationMethodType.BalanceToDC, new GeneralComponent(eComponentType.Total), int.MaxValue, Include.AllStoreFilterRID, false);
						// Begin TT#1708-MD - stodd - Allocation is out of balance after DC Carton Rounding to Reserve
                        msg = "(DC Carton Rounding for header " + ap.HeaderID + ") For Receiving DC: Balance to Reserve performed.";
                        _audit.Add_Msg(eMIDMessageLevel.Debug, msg, this.GetType().Name);
						// End TT#1708-MD - stodd - Allocation is out of balance after DC Carton Rounding to Reserve
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        ///  Spread to a header with bulk color or just total.
        /// </summary>
        /// <param name="ap"></param>
        /// <param name="recDC"></param>
        /// <param name="UnitsAllocatedChange"></param>
        private void ProcessReceivingDCWithBulkOrTotal(AllocationProfile ap, DCInfo recDC, int UnitsAllocatedChange, bool isBulk)	// TT#1702-MD - DC Carton Rounding goes out of balance with Odd units 
        {
            int multiple = 1;	// TT#1702-MD - DC Carton Rounding goes out of balance with Odd units 
            try
            {
				// Begin TT#1702-MD - DC Carton Rounding goes out of balance with Odd units 
                if (isBulk)
                {
                    multiple = ap.BulkMultiple;
                }
                else
                {
                    multiple = ap.AllocationMultiple;
                }

                int oddUnits = recDC.TotalUnitsAllocated % multiple;

                string msg = "(DC Carton Rounding for header " + ap.HeaderID + ") For Receiving DC: Odd units after dividing by multiple of " + multiple + " = " + oddUnits;
				// End TT#1702-MD - DC Carton Rounding goes out of balance with Odd units 
                _audit.Add_Msg(eMIDMessageLevel.Debug, msg, this.GetType().Name);

                // no "odd" units
                if (oddUnits == 0)
                {
                    SpreadToReceivingDCWithBulk(ap, recDC, UnitsAllocatedChange);
                }
                // Yes, there are "odd" units that don't fit the bulk multiple
                else
                {
                    //Dictionary<int, int> storeOddUnitList = new Dictionary<int, int>();	// TT#1708-MD - stodd - Allocation is out of balance after DC Carton Rounding to Reserve
                    bool foundStoreOddUnits = false;
                    int storeOddUnits = 0;
                    foreach (StoreProfile sp in recDC.Stores)
                    {
                        int storeAllocatedUnits = ap.GetStoreQtyAllocated(eAllocationSummaryNode.Total, sp.Key);
						// Begin TT#1702-MD - DC Carton Rounding goes out of balance with Odd units 
                        //storeOddUnits = storeAllocatedUnits % ap.BulkMultiple;
                        storeOddUnits = storeAllocatedUnits % multiple;
						// End TT#1702-MD - DC Carton Rounding goes out of balance with Odd units 
                        _storeOddUnitList.Add(sp.Key, storeOddUnits);	// TT#1708-MD - stodd - Allocation is out of balance after DC Carton Rounding to Reserve
                        if (storeOddUnits != 0)
                        {
                            foundStoreOddUnits = true;
                            ap.SetStoreQtyAllocated(eAllocationSummaryNode.Total, sp.Key, storeAllocatedUnits - storeOddUnits);

                            msg = "(DC Carton Rounding for header " + ap.HeaderID + ") For Receiving DC: Odd units for store " + sp.StoreId + " = " + storeOddUnits
                                + Environment.NewLine + "These units will be added back to store after Spread.";
                            _audit.Add_Msg(eMIDMessageLevel.Debug, msg, this.GetType().Name);

                        }
                    }

                    if (foundStoreOddUnits)
                    {
                        recDC.TotalUnitsAllocated -= oddUnits;
						// Begin TT#1708-MD - stodd - Allocation is out of balance after DC Carton Rounding to Reserve
                        bool performBalanceToReserve = SpreadToReceivingDCWithBulk(ap, recDC, UnitsAllocatedChange);

                        // If during the spread, we did NOT perform a balance to reserve, we want to add
                        // back the odd units to the stores we took them from originally.
                        if (!performBalanceToReserve)
                        {
                            foreach (StoreProfile sp in recDC.Stores)
                            {
                                storeOddUnits = _storeOddUnitList[sp.Key];
                                if (storeOddUnits != 0 && oddUnits > 0)     // TT#4803 - stodd - Carton Rounding Process Left Allocation Out of Balance
                                {
                                    // Begin TT#4803 - stodd - Carton Rounding Process Left Allocation Out of Balance
                                    // This tries to make sure we don't add back more units than found in oddUnits.
                                    if (storeOddUnits > oddUnits)
                                    {
                                        AddOddUnitsToStore(ap, sp.Key, oddUnits);
                                        oddUnits = 0;
                                    }
                                    else
                                    {
                                        AddOddUnitsToStore(ap, sp.Key, storeOddUnits);
                                        oddUnits -= storeOddUnits;
                                    }
                                    // End TT#4803 - stodd - Carton Rounding Process Left Allocation Out of Balance
                                }
                            }
                        }
						// End TT#1708-MD - stodd - Allocation is out of balance after DC Carton Rounding to Reserve
                    }
                    else
                    // If no stores had odd units, then find the store with the largest allocation and add the DC odd units to it.
                    {
                        msg = "(DC Carton Rounding for header " + ap.HeaderID + ") For Receiving DC: No odd units found in stores. This means odd units where in a Transfer DC and must be added into the receiving DC.";
                        _audit.Add_Msg(eMIDMessageLevel.Debug, msg, this.GetType().Name);

                        if (_APPLY_OVERAGE_TO == eAllocateOverageTo.AllocatedStores)
                        {
                            Dictionary<int, int> storeAllocatedUnitsOriginalList = new Dictionary<int, int>();
                            foreach (StoreProfile sp in recDC.Stores)
                            {
                                int storeAllocatedUnits = ap.GetStoreQtyAllocated(eAllocationSummaryNode.Total, sp.Key);
                                storeAllocatedUnitsOriginalList.Add(sp.Key, storeAllocatedUnits);
                            }

                            // Spread new Total after subtracting odd units
                            if (ap.BulkColors.Count > 0)
                            {
                                ap.SpreadStoreListTotalQtyAllocated(new GeneralComponent(eComponentType.Bulk), recDC.Stores, recDC.TotalUnitsAllocated - oddUnits);
                            }
                            else
                            {
                                ap.SpreadStoreListTotalQtyAllocated(new GeneralComponent(eComponentType.Total), recDC.Stores, recDC.TotalUnitsAllocated - oddUnits);
                            }

                            if (UnitsAllocatedChange < 1)
                            {
                                // Find the store with the largest descrease in units and add the odd units to it.
                                int storeAllocatedUnits = 0;
                                Dictionary<int, int> storeAllocatedUnitsAfterSpreadList = new Dictionary<int, int>();
                                Dictionary<int, int> storeAllocatedUnitsDiffList = new Dictionary<int, int>();
                                foreach (StoreProfile sp in recDC.Stores)
                                {
                                    storeAllocatedUnits = ap.GetStoreQtyAllocated(eAllocationSummaryNode.Total, sp.Key);
                                    storeAllocatedUnitsDiffList.Add(sp.Key, storeAllocatedUnitsOriginalList[sp.Key] - storeAllocatedUnits);
                                    Debug.WriteLine(" Store RID " + sp.Key + " ORIG " + storeAllocatedUnitsOriginalList[sp.Key]
                                        + " AFTER SPREAD " + storeAllocatedUnits
                                        + " DIFF " + storeAllocatedUnitsDiffList[sp.Key]);
                                }

                                List<StoreProfile> storeList = SortStoresDescendingByDifference(ap, storeAllocatedUnitsDiffList, recDC.Stores, ap.AppSessionTransaction);
                                StoreProfile largestAllocatedDecreaseStore = storeList[0];

                                AddOddUnitsToStore(ap, largestAllocatedDecreaseStore.Key, oddUnits);

                                msg = "(DC Carton Rounding for header " + ap.HeaderID + ") For Receiving DC: The total allocation decreased. Odd units will be added back to one of the stores that decreased by the most." + Environment.NewLine
                                    + "Store: " + largestAllocatedDecreaseStore.StoreId + ". Added odd units in the amount of " + oddUnits;
                                _audit.Add_Msg(eMIDMessageLevel.Debug, msg, this.GetType().Name);

                            }
                            else
                            {
                                // Find the store with the largest allocation and add the odd units to it
                                List<StoreProfile> storeList = SortStoresDescending(ap, recDC.Stores, ap.AppSessionTransaction);
                                StoreProfile highestAllocatedStore = storeList[0];

                                AddOddUnitsToStore(ap, highestAllocatedStore.Key, oddUnits);

                                msg = "(DC Carton Rounding for header " + ap.HeaderID + ") For Receiving DC: The total allocation increase. Odd units will be added back to one of the stores with the greatest allocation." + Environment.NewLine
                                    + "Store: " + highestAllocatedStore.StoreId + ". Added odd units in the amount of " + oddUnits;
                                _audit.Add_Msg(eMIDMessageLevel.Debug, msg, this.GetType().Name);

                            }
                        }
                        else if (_APPLY_OVERAGE_TO == eAllocateOverageTo.Reserve)
                        {
                            msg = "(DC Carton Rounding for header " + ap.HeaderID + ") Odd units in the amount of " + oddUnits + " was added to reserve";
                            _audit.Add_Msg(eMIDMessageLevel.Debug, msg, this.GetType().Name);

                            // Balance to Reserve action
                            if (ap.BulkColors.Count > 0)
                            {
                                ap.Action(eAllocationMethodType.BalanceToDC, new GeneralComponent(eComponentType.Bulk), int.MaxValue, Include.AllStoreFilterRID, false);
                            }
                            else
                            {
                                ap.Action(eAllocationMethodType.BalanceToDC, new GeneralComponent(eComponentType.Total), int.MaxValue, Include.AllStoreFilterRID, false);
                            }
                        }
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Adds the store odd units to the store.
        /// </summary>
        /// <param name="ap"></param>
        /// <param name="storeRid"></param>
        /// <param name="oddUnits"></param>
        private void AddOddUnitsToStore(AllocationProfile ap, int storeRid, int oddUnits)
        {
            try
            {
                int storeAllocatedUnits = 0;
                if (ap.BulkColors.Count > 0)
                {
                    storeAllocatedUnits = ap.GetStoreQtyAllocated(eAllocationSummaryNode.Bulk, storeRid);
                    ap.SetStoreQtyAllocated(eAllocationSummaryNode.Bulk, storeRid, storeAllocatedUnits + oddUnits);
                }
                else
                {
                    storeAllocatedUnits = ap.GetStoreQtyAllocated(eAllocationSummaryNode.Total, storeRid);
                    ap.SetStoreQtyAllocated(eAllocationSummaryNode.Total, storeRid, storeAllocatedUnits + oddUnits);
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// This actually will spread to Total or Bulk, if present on the header.
        /// </summary>
        /// <param name="ap"></param>
        /// <param name="recDC"></param>
        /// <param name="UnitsAllocatedChange"></param>
        /// <returns></returns>
        private bool SpreadToReceivingDCWithBulk(AllocationProfile ap, DCInfo recDC, int UnitsAllocatedChange)	// TT#1708-MD - stodd - Allocation is out of balance after DC Carton Rounding to Reserve
        {
			// Begin TT#1708-MD - stodd - Allocation is out of balance after DC Carton Rounding to Reserve
            bool spreadToReserve = false;
            string msg = string.Empty;
			// End TT#1708-MD - stodd - Allocation is out of balance after DC Carton Rounding to Reserve

            // Allocation decreased after carton rounding
            if (UnitsAllocatedChange < 1)
            {
                if (ap.BulkColors.Count > 0)
                {
                    ap.SpreadStoreListTotalQtyAllocated(new GeneralComponent(eComponentType.Bulk), recDC.Stores, recDC.TotalUnitsAllocated);
                    msg = "(DC Carton Rounding for header " + ap.HeaderID + ") For Receiving DC: Spread new DC total allocated of " + recDC.TotalUnitsAllocated + " to stores.";
                    _audit.Add_Msg(eMIDMessageLevel.Debug, msg, this.GetType().Name);
                }
                else
                {
                    ap.SpreadStoreListTotalQtyAllocated(new GeneralComponent(eComponentType.Total), recDC.Stores, recDC.TotalUnitsAllocated);
                    msg = "(DC Carton Rounding for header " + ap.HeaderID + ") For Receiving DC: Spread new DC total allocated of " + recDC.TotalUnitsAllocated + " to stores.";
                    _audit.Add_Msg(eMIDMessageLevel.Debug, msg, this.GetType().Name);
                }
            }
            else // Allocated units increased after carton rounding
            {
                if (_APPLY_OVERAGE_TO == eAllocateOverageTo.AllocatedStores)
                {
                    if (ap.BulkColors.Count > 0)
                    {
                        ap.SpreadStoreListTotalQtyAllocated(new GeneralComponent(eComponentType.Bulk), recDC.Stores, recDC.TotalUnitsAllocated);
                        msg = "(DC Carton Rounding for header " + ap.HeaderID + ") For Receiving DC: Spread new DC total allocated of " + recDC.TotalUnitsAllocated + " to stores.";
                        _audit.Add_Msg(eMIDMessageLevel.Debug, msg, this.GetType().Name);
                    }
                    else
                    {
                        ap.SpreadStoreListTotalQtyAllocated(new GeneralComponent(eComponentType.Total), recDC.Stores, recDC.TotalUnitsAllocated);
                        msg = "(DC Carton Rounding for header " + ap.HeaderID + ") For Receiving DC: Spread new DC total allocated of " + recDC.TotalUnitsAllocated + " to stores.";
                        _audit.Add_Msg(eMIDMessageLevel.Debug, msg, this.GetType().Name);
                    }
                }
                else if (_APPLY_OVERAGE_TO == eAllocateOverageTo.Reserve)
                {
					// Begin TT#1708-MD - stodd - Allocation is out of balance after DC Carton Rounding to Reserve
                    spreadToReserve = true;

                    // add back "odd units" to receiving DC so balance is just other DCs to reserve
                    // Begin TT1710-MD - stodd - key not found error
                    if (_storeOddUnitList.Count > 0)
                    {
                        foreach (StoreProfile sp in recDC.Stores)
                        {
                            int storeOddUnits = _storeOddUnitList[sp.Key];
                            if (storeOddUnits != 0)
                            {
                                AddOddUnitsToStore(ap, sp.Key, storeOddUnits);
                            }
                        }
                    }
                    // End TT1710-MD - stodd - key not found error
                    //msg = "(DC Carton Rounding for header " + ap.HeaderID + ") Odd units in the amount of " + oddUnits + " was added to reserve";
					// End TT#1708-MD - stodd - Allocation is out of balance after DC Carton Rounding to Reserve
                    // Balance to Reserve action
                    if (ap.BulkColors.Count > 0)
                    {
                        ap.Action(eAllocationMethodType.BalanceToDC, new GeneralComponent(eComponentType.Bulk), int.MaxValue, Include.AllStoreFilterRID, false);
                        msg = "(DC Carton Rounding for header " + ap.HeaderID + ") For Receiving DC: Balance to Reserve performed.";
                        _audit.Add_Msg(eMIDMessageLevel.Debug, msg, this.GetType().Name);
                    }
                    else
                    {
                        ap.Action(eAllocationMethodType.BalanceToDC, new GeneralComponent(eComponentType.Total), int.MaxValue, Include.AllStoreFilterRID, false);
                        msg = "(DC Carton Rounding for header " + ap.HeaderID + ") For Receiving DC: Balance to Reserve performed.";
                        _audit.Add_Msg(eMIDMessageLevel.Debug, msg, this.GetType().Name);
                    }
                }
            }

            return spreadToReserve;		// TT#1708-MD - stodd - Allocation is out of balance after DC Carton Rounding to Reserve
        }

        private void DebugDCList(string title)		// TT#1708-MD - stodd - Allocation is out of balance after DC Carton Rounding to Reserve
        {
            string msg = string.Empty;
            Debug.WriteLine("");
            foreach (DCInfo aDC in _DCInfoList)
            {
                msg += "DC Name: " + aDC.SglName +		// TT#1708-MD - stodd - Allocation is out of balance after DC Carton Rounding to Reserve
                    "  SGLRID: " + aDC.SGLRid +
                    "  ORIG ALLOC: " + aDC.OrigUnitsAllocated +
                    "  CURR ALLOC: " + aDC.TotalUnitsAllocated +
                    "  IS RECEIVING DC: " + aDC.IsRecievingDC +
                    "  INCLUDES RESERVE ONLY: " + aDC.IncludesReserveOnly + Environment.NewLine;	// TT#1698-MD - stodd - Recieve Warning Error (total not spread) when processing Carton Rounding on Headers within a Group
            }

			// Begin TT#1708-MD - stodd - Allocation is out of balance after DC Carton Rounding to Reserve
            if (string.IsNullOrEmpty(title))
            {
                msg = "DC Carton Rounding: DC List" + Environment.NewLine + msg;
            }
            else
            {
                msg = title + Environment.NewLine + msg;
            }
			// End TT#1708-MD - stodd - Allocation is out of balance after DC Carton Rounding to Reserve

            Debug.WriteLine(msg);
            _audit.Add_Msg(eMIDMessageLevel.Debug, msg, this.GetType().Name);

        }

        private void DebugDCStores(AllocationProfile ap)
        {
            Debug.WriteLine("");
            Debug.WriteLine("Stores");
            foreach (DCInfo aDC in _DCInfoList)
            {
                Debug.WriteLine("  SGLRID: " + aDC.SGLRid +
                       "  IS RECEIVING DC: " + aDC.IsRecievingDC + "  TOTAL ALLOC: " + aDC.TotalUnitsAllocated);
                foreach (StoreProfile sp in aDC.Stores.ArrayList)
                {
                    Index_RID storeIndex = ap.StoreIndex(sp.Key);
                    Debug.WriteLine("  STORE ALLOC: " + ap.GetStoreQtyAllocated(eAllocationSummaryNode.Total, storeIndex)
                        );
                }
            }
        }

        // Begin TT#4911 - JSmith - Auto Release Workflow Fail on Intransit Charge
        //private bool isValidHeader(AllocationProfile ap, ApplicationSessionTransaction aApplicationTransaction)
        private bool isValidHeader(AllocationProfile ap, ApplicationSessionTransaction aApplicationTransaction, ApplicationWorkFlowStep aAllocationWorkFlowStep, out bool isOKToSkip)
        // End TT#4911 - JSmith - Auto Release Workflow Fail on Intransit Charge
        {
            bool isValid = true;
            isOKToSkip = false;  // TT#4911 - JSmith - Auto Release Workflow Fail on Intransit Charge
            try
            {
				// Begin TT#1718-MD - stodd - Processing DC Carton Rounding changed status to Allocated Out of Balance 
                //===========================================================================
                // NOTE: One piece of validation is done in the LoadDCInformation() method.
                //       if the DC Carton Rounding method is to allocate odd units to receiving DC, 
                //          receiving DC must have units allocated.
                //===========================================================================
				// End TT#1718-MD - stodd - Processing DC Carton Rounding changed status to Allocated Out of Balance 
				
                //===================================
                // Header must be All in Balance
                //===================================
                if (ap.HeaderAllocationStatus != eHeaderAllocationStatus.AllInBalance)
                {
                    isValid = false;
                    string msg = string.Format(
                        _audit.GetText(eMIDTextCode.msg_HeaderStatusDisallowsAction, false), MIDText.GetTextOnly((int)ap.HeaderAllocationStatus));
                    _audit.Add_Msg(
                        eMIDMessageLevel.Error, eMIDTextCode.msg_HeaderStatusDisallowsAction,
                        "Method: " + this.Name + " Header: " + ap.HeaderID + "  " + msg,
                        this.GetType().Name);
                    aApplicationTransaction.SetAllocationActionStatus(ap.Key, eAllocationActionStatus.NoActionPerformed);
                }
                // Begin TT#4911 - JSmith - Auto Release Workflow Fail on Intransit Charge
				// Check intransit charged last since it is ok to skip method in a workflow
                ////===========================================
                //// Header cannot have Intransit charged
                ////===========================================
                //else if (ap.StyleIntransitUpdated ||
                //            ap.BulkColorIntransitUpdated ||
                //            ap.BulkSizeIntransitUpdated)
                //{
                //    isValid = false;
                //    string msg = MIDText.GetTextOnly(eMIDTextCode.msg_al_DCCartonRoundingIntransitCharged);
                //    _audit.Add_Msg(
                //        eMIDMessageLevel.Error, eMIDTextCode.msg_al_DCCartonRoundingIntransitCharged,
                //        "Method: " + this.Name + " Header: " + ap.HeaderID + "  " + msg,
                //        this.GetType().Name);
                //    aApplicationTransaction.SetAllocationActionStatus(ap.Key, eAllocationActionStatus.NoActionPerformed);
                //}
                // End TT#4911 - JSmith - Auto Release Workflow Fail on Intransit Charge

                //===================================================================
                // Unsupported header types: IMO/VSW, Assortment/Group Allocation
                //===================================================================
                else if (ap.HeaderType == eHeaderType.IMO || ap.HeaderType == eHeaderType.Assortment)
                {
                    isValid = false;
                    string msg = MIDText.GetTextOnly(eMIDTextCode.msg_al_DCCartonRoundingUnsupportedHeaderType);
                    _audit.Add_Msg(
                        eMIDMessageLevel.Error, eMIDTextCode.msg_al_DCCartonRoundingUnsupportedHeaderType,
                        "Method: " + this.Name + " Header: " + ap.HeaderID + "  " + msg,
                        this.GetType().Name);
                    aApplicationTransaction.SetAllocationActionStatus(ap.Key, eAllocationActionStatus.NoActionPerformed);
                }

                //=========================
                // Sizes not supported
                //=========================
                else if (ap.HasSizes)
                {
                    isValid = false;
                    string msg = MIDText.GetTextOnly(eMIDTextCode.msg_al_DCCartonRoundingSizesInvalid);
                    _audit.Add_Msg(
                        eMIDMessageLevel.Error, eMIDTextCode.msg_al_DCCartonRoundingSizesInvalid,
                        "Method: " + this.Name + " Header: " + ap.HeaderID + "  " + msg,
                        this.GetType().Name);
                    aApplicationTransaction.SetAllocationActionStatus(ap.Key, eAllocationActionStatus.NoActionPerformed);
                }

				// Begin TT#1692-MD - stodd - DC Carton Rounding - process should not run if Units Per Carton has not been assigned
                //=========================
                // Units Per Carton = 0
                //=========================
                else if (ap.UnitsPerCarton == 0)
                {
                    isValid = false;
                    string msg = MIDText.GetTextOnly(eMIDTextCode.msg_al_DCCartonRoundingNoUnitsPerCarton);
                    _audit.Add_Msg(
                        eMIDMessageLevel.Error, eMIDTextCode.msg_al_DCCartonRoundingNoUnitsPerCarton,
                        "Method: " + this.Name + " Header: " + ap.HeaderID + "  " + msg,
                        this.GetType().Name);
                    aApplicationTransaction.SetAllocationActionStatus(ap.Key, eAllocationActionStatus.NoActionPerformed);
                }
				// End TT#1692-MD - stodd - DC Carton Rounding - process should not run if Units Per Carton has not been assigned

                // Begin TT#1693-MD - stodd - DC Carton Rounding - add edit to stop processing if any stores have a VSW ID defined.
                //=========================
                // VSW Found on stores
                //=========================
                else if (StoresHaveVSW())
                {
                    isValid = false;
                    string msg = MIDText.GetTextOnly(eMIDTextCode.msg_al_DCCartonRoundingVSWOnStores);
                    _audit.Add_Msg(
                        eMIDMessageLevel.Error, eMIDTextCode.msg_al_DCCartonRoundingVSWOnStores,
                        "Method: " + this.Name + " Header: " + ap.HeaderID + "  " + msg,
                        this.GetType().Name);
                    aApplicationTransaction.SetAllocationActionStatus(ap.Key, eAllocationActionStatus.NoActionPerformed);
                }
                // End TT#1693-MD - stodd - DC Carton Rounding - add edit to stop processing if any stores have a VSW ID defined.
                // Begin TT#1966-MD - JSmith- DC Fulfillment
                else if (ap.IsMasterHeader && ap.DCFulfillmentProcessed)
                {
                    string errorMessage = string.Format(MIDText.GetTextOnly(eMIDTextCode.msg_al_DCFulfillmentProcessedActionNotAllowed), ap.HeaderID);
                    _audit.Add_Msg(
                        eMIDMessageLevel.Warning,
                        eMIDTextCode.msg_al_DCFulfillmentProcessedActionNotAllowed,
                        this.Name + " " + errorMessage,
                        this.GetType().Name);
                    aApplicationTransaction.SetAllocationActionStatus(ap.Key, eAllocationActionStatus.NoActionPerformed);
                }
                else if (ap.IsSubordinateHeader && !ap.DCFulfillmentProcessed)
                {
                    isValid = false;
                    string errorMessage = string.Format(MIDText.GetTextOnly(eMIDTextCode.msg_al_DCFulfillmentNotProcessedActionNotAllowed), ap.HeaderID);
                    _audit.Add_Msg(
                        eMIDMessageLevel.Warning,
                        eMIDTextCode.msg_al_DCFulfillmentNotProcessedActionNotAllowed,
                        this.Name + " " + errorMessage,
                        this.GetType().Name);
                    aApplicationTransaction.SetAllocationActionStatus(ap.Key, eAllocationActionStatus.NoActionPerformed);
                }
                // End TT#1966-MD - JSmith- DC Fulfillment
				
                double remainder = 0;
                if (isValid)
                {
                    //=====================================
                    // Cannot have more than one pack
                    //=====================================
                    if (ap.PackCount > 1)
                    {
                        isValid = false;
                        string msg = MIDText.GetTextOnly(eMIDTextCode.msg_al_DCCartonRoundingMoreThanOnePack);
                        _audit.Add_Msg(
                            eMIDMessageLevel.Error, eMIDTextCode.msg_al_DCCartonRoundingMoreThanOnePack,
                        "Method: " + this.Name + " Header: " + ap.HeaderID + "  " + msg,
                            this.GetType().Name);
                        aApplicationTransaction.SetAllocationActionStatus(ap.Key, eAllocationActionStatus.NoActionPerformed);
                    }

                    //=====================================
                    // Cannot have more than one color
                    //=====================================
                    else if (ap.BulkColors.Count > 1)
                    {
                        isValid = false;
                        string msg = MIDText.GetTextOnly(eMIDTextCode.msg_al_DCCartonRoundingMoreThanOneColor);
                        _audit.Add_Msg(
                            eMIDMessageLevel.Error, eMIDTextCode.msg_al_DCCartonRoundingMoreThanOneColor,
                        "Method: " + this.Name + " Header: " + ap.HeaderID + "  " + msg,
                            this.GetType().Name);
                        aApplicationTransaction.SetAllocationActionStatus(ap.Key, eAllocationActionStatus.NoActionPerformed);
                    }

                    //============================================
                    // Cannot have both packs and bulk colors
                    //============================================
                    else if (ap.PackCount > 0 && ap.BulkColorCount > 0)
                    {
                        isValid = false;
                        string msg = MIDText.GetTextOnly(eMIDTextCode.msg_al_DCCartonRoundingPacksAndBulk);
                        _audit.Add_Msg(
                            eMIDMessageLevel.Error, eMIDTextCode.msg_al_DCCartonRoundingPacksAndBulk,
                        "Method: " + this.Name + " Header: " + ap.HeaderID + "  " + msg,
                            this.GetType().Name);
                        aApplicationTransaction.SetAllocationActionStatus(ap.Key, eAllocationActionStatus.NoActionPerformed);
                    }

                    //==================================================================
                    // If one pack, pack multiple must divide evenly into total units
                    //==================================================================
                    else if (ap.PackCount == 1)
                    {
                        PackHdr aPack = null;
                        foreach (PackHdr p in ap.Packs.Values)
                        {
                            aPack = p;
                        }
                        remainder = ap.TotalUnitsAllocated % aPack.PackMultiple;
                        if (remainder > 0)
                        {
                            isValid = false;
                            string msg = MIDText.GetTextOnly(eMIDTextCode.msg_al_DCCartonRoundingTotalNotDivisibleByMultiple);
                            _audit.Add_Msg(
                                eMIDMessageLevel.Error, eMIDTextCode.msg_al_DCCartonRoundingTotalNotDivisibleByMultiple,
                                "Method: " + this.Name + " Header: " + ap.HeaderID + "  " + msg,
                                this.GetType().Name);
                            aApplicationTransaction.SetAllocationActionStatus(ap.Key, eAllocationActionStatus.NoActionPerformed);
                        }
                    }
                }

                if (isValid)
                {
                    //========================================================================
                    // units per carton must be evenly divisible by allocation multiple
                    //========================================================================
                    remainder = ap.UnitsPerCarton % ap.AllocationMultiple;
                    if (remainder > 0)
                    {
                        isValid = false;
                        string msg = MIDText.GetTextOnly(eMIDTextCode.msg_al_DCCartonRoundingUnitsPerCartonNotDivisibleByMultiple);
                        _audit.Add_Msg(
                            eMIDMessageLevel.Error, eMIDTextCode.msg_al_DCCartonRoundingUnitsPerCartonNotDivisibleByMultiple,
                            "Method: " + this.Name + " Header: " + ap.HeaderID + "  " + msg,
                            this.GetType().Name);
                        aApplicationTransaction.SetAllocationActionStatus(ap.Key, eAllocationActionStatus.NoActionPerformed);
                    }
                }

                // Begin TT#4911 - JSmith - Auto Release Workflow Fail on Intransit Charge
                // Test intransit last.  If in a workflow, allow workflow to continue and skip this method if no other errors
                if (isValid)
                {
                    //===========================================
                    // Check if Header has Intransit charged
                    //===========================================
                    if (ap.StyleIntransitUpdated ||
                              ap.BulkColorIntransitUpdated ||
                              ap.BulkSizeIntransitUpdated)
                    {
                        isValid = false;
                        if (aAllocationWorkFlowStep.Key == Include.NoRID)
                        {
                            string msg = MIDText.GetTextOnly(eMIDTextCode.msg_al_DCCartonRoundingIntransitCharged);
                            _audit.Add_Msg(
                                eMIDMessageLevel.Error, eMIDTextCode.msg_al_DCCartonRoundingIntransitCharged,
                                "Method: " + this.Name + " Header: " + ap.HeaderID + "  " + msg,
                                this.GetType().Name);
                        }
                        else
                        {
                            string msg = MIDText.GetTextOnly(eMIDTextCode.msg_al_DCCartonRoundingIntransitChargedMethodSkipped);
                            _audit.Add_Msg(
                                eMIDMessageLevel.Error, eMIDTextCode.msg_al_DCCartonRoundingIntransitChargedMethodSkipped,
                                "Method: " + this.Name + " Header: " + ap.HeaderID + "  " + msg,
                                this.GetType().Name);
                            isOKToSkip = true;
                        }
                        aApplicationTransaction.SetAllocationActionStatus(ap.Key, eAllocationActionStatus.NoActionPerformed);
                    }
                }
                // End TT#4911 - JSmith - Auto Release Workflow Fail on Intransit Charge

                return isValid;
            }
            catch
            {
                throw;
            }

        }

        private bool StoresHaveVSW()
        {
            bool hasVSW = false;

            ProfileList storeList = SAB.StoreServerSession.GetAllStoresList();
            foreach (StoreProfile sp in storeList.ArrayList)
            {
                if (!string.IsNullOrEmpty(sp.IMO_ID) && !string.IsNullOrWhiteSpace(sp.IMO_ID))  // TT#1690-MD - stodd - spacing out VSW ID causes a "hasVSW" to be true
                {
                    hasVSW = true;
                    break;
                }
            }

            return hasVSW;
        }

        private bool isValidDCInfo(AllocationProfile ap, ApplicationSessionTransaction aApplicationTransaction)
        {
            bool isValid = true;
            try
            {
                DCInfo recDc = GetReceivingDC();

                //============================================================================================
                // Receiving DC on header must match a store group level in DC Cartoning Rounding attribute
                //============================================================================================
                if (recDc == null)
                {
                    isValid = false;
                    string msg = MIDText.GetTextOnly(eMIDTextCode.msg_al_DCCartonRoundingInvalidReceivingDC);
                    _audit.Add_Msg(
                        eMIDMessageLevel.Error, eMIDTextCode.msg_al_DCCartonRoundingInvalidReceivingDC,
                        "Method: " + this.Name + " Header: " + ap.HeaderID + "  " + msg,
                        this.GetType().Name);
                    aApplicationTransaction.SetAllocationActionStatus(ap.Key, eAllocationActionStatus.NoActionPerformed);
                    return isValid;
                }

                //=============================================
                // Receiving DC must have stores assigned
                //=============================================
                if (recDc.Stores.Count == 0)
                {
                    isValid = false;
                    string msg = MIDText.GetTextOnly(eMIDTextCode.msg_al_DCCartonRoundingReceivingDCHasNoStores);
                    _audit.Add_Msg(
                        eMIDMessageLevel.Error, eMIDTextCode.msg_al_DCCartonRoundingReceivingDCHasNoStores,
                        "Method: " + this.Name + " Header: " + ap.HeaderID + "  " + msg,
                        this.GetType().Name);
                    aApplicationTransaction.SetAllocationActionStatus(ap.Key, eAllocationActionStatus.NoActionPerformed);
                    return isValid;
                }

                return isValid;
            }
            catch
            {
                throw;
            }

        }


        /// <summary>
        /// Gathers information about each DC (Attribute set in this case) and adds them to the _DCInfoList.
        /// </summary>
        /// <param name="ap"></param>
        private bool LoadDCInformation(AllocationProfile ap)	// TT#1718-MD - stodd - Processing DC Carton Rounding changed status to Allocated Out of Balance 
        {
            bool isSuccessful = true;	// TT#1718-MD - stodd - Processing DC Carton Rounding changed status to Allocated Out of Balance 
            bool recievingDCFound = false;
            int qtyAllocated = 0;

            try
            {
                //_sgp = SAB.StoreServerSession.GetStoreGroup(SG_RID);	// TT#1708-MD - stodd - Allocation is out of balance after DC Carton Rounding to Reserve
                _sgp = StoreMgmt.StoreGroup_Get(SG_RID);  // TT#1386-MD stodd - manual merge
                foreach (StoreGroupLevelProfile sglp in _sgp.GroupLevels.ArrayList)		// TT#1708-MD - stodd - Allocation is out of balance after DC Carton Rounding to Reserve
                {
                    // fill store list in ALL DCs
                    //ProfileList storeList = SAB.StoreServerSession.GetStoresInGroup(SG_RID, sglp.Key);
                    ProfileList storeList = StoreMgmt.StoreGroupLevel_GetStoreProfileList(SG_RID, sglp.Key);     // TT#1386-MD stodd - manual merge

                    if (sglp.Name == ap.DistributionCenter)
                    {
                        recievingDCFound = true;
                        qtyAllocated = ap.GetStoreListTotalItemQtyAllocated(eAllocationSummaryNode.Total, _sgp.Key, sglp.Key);	// TT#1708-MD - stodd - Allocation is out of balance after DC Carton Rounding to Reserve
                        DCInfo newDCInfo = new DCInfo(sglp.Key, qtyAllocated, qtyAllocated, true, sglp.Name);	// TT#1708-MD - stodd - Allocation is out of balance after DC Carton Rounding to Reserve
                        newDCInfo.Stores = storeList;
                        _DCInfoList.Add(newDCInfo);

						// Begin TT#1718-MD - stodd - Processing DC Carton Rounding changed status to Allocated Out of Balance 
                        // Validation - if method is to allocate odd units to receiving DC, receiving DC must haver units allocated.
                        if (_APPLY_OVERAGE_TO == eAllocateOverageTo.AllocatedStores && qtyAllocated == 0)
                        {
                            isSuccessful = false;
                            string msg = string.Format(
                                _audit.GetText(eMIDTextCode.msg_al_DCCartonRoundingReceivingDCHasNoAllocation, false));
                            _audit.Add_Msg(
                                eMIDMessageLevel.Error, eMIDTextCode.msg_al_DCCartonRoundingReceivingDCHasNoAllocation,
                                "Method: " + this.Name + " Header: " + ap.HeaderID + "  " + msg,
                                this.GetType().Name);
                            ap.AppSessionTransaction.SetAllocationActionStatus(ap.Key, eAllocationActionStatus.NoActionPerformed);
                            break;
                        }
						// End TT#1718-MD - stodd - Processing DC Carton Rounding changed status to Allocated Out of Balance 
                    }
                    else
                    {
                        qtyAllocated = ap.GetStoreListTotalItemQtyAllocated(eAllocationSummaryNode.Total, _sgp.Key, sglp.Key);	// TT#1708-MD - stodd - Allocation is out of balance after DC Carton Rounding to Reserve
                        DCInfo newDCInfo = new DCInfo(sglp.Key, qtyAllocated, qtyAllocated, false, sglp.Name);	// TT#1708-MD - stodd - Allocation is out of balance after DC Carton Rounding to Reserve
                        newDCInfo.Stores = storeList;
						// Begin TT#1698-MD - stodd - Recieve Warning Error (total not spread) when processing Carton Rounding on Headers within a Group
                        //===============================================================
                        // Need to know if the entire qty for this set is in reserve.
                        // effects later processing.
                        //===============================================================
                        int reserveQtyAllocated = ap.GetStoreQtyAllocated(eAllocationSummaryNode.Total, ap.AppSessionTransaction.ReserveStore);
                        if (qtyAllocated > 0 && qtyAllocated == reserveQtyAllocated)	// TT#4803 - stodd - Carton Rounding Process Left Allocation Out of Balance
                        {
                            newDCInfo.IncludesReserveOnly = true;
                            _DCIncludesReserveOnly = true;
                        }
						// End TT#1698-MD - stodd - Recieve Warning Error (total not spread) when processing Carton Rounding on Headers within a Group
                        _DCInfoList.Add(newDCInfo);
                    }

                }

                if (!recievingDCFound)
                {
                    // ERROR - no Recieving DC match within Attribute
                    // Handeled in validation code
                }

                return isSuccessful;	// TT#1718-MD - stodd - Processing DC Carton Rounding changed status to Allocated Out of Balance 
            }
            catch
            {
                throw;	// TT#1718-MD - stodd - Processing DC Carton Rounding changed status to Allocated Out of Balance 
            }

        }

		// Begin TT#1708-MD - stodd - Allocation is out of balance after DC Carton Rounding to Reserve
        /// <summary>
        /// Reads the allocated totals for each DC and updates the DCInfo class.
        /// </summary>
        /// <param name="ap"></param>
        private void ReloadDCAllocatedTotals(AllocationProfile ap)
        {
            try
            {
                foreach (DCInfo aDC in _DCInfoList)
                {
                    aDC.TotalUnitsAllocated = ap.GetStoreListTotalItemQtyAllocated(eAllocationSummaryNode.Total, _sgp.Key, aDC.SGLRid);
                }
            }
            catch
            {

            }

        }
		// End TT#1708-MD - stodd - Allocation is out of balance after DC Carton Rounding to Reserve

        /// <summary>
        /// There should only be ONE pack on the header...and this returns it.
        /// </summary>
        /// <param name="ap"></param>
        /// <returns></returns>
        private PackHdr GetPack(AllocationProfile ap)
        {
            PackHdr aPack = null;
            foreach (PackHdr p in ap.Packs.Values)
            {
                aPack = p;
            }
            return aPack;
        }

        /// <summary>
        /// There should only be ONE color on the header...and this returns it.
        /// </summary>
        /// <param name="ap"></param>
        /// <returns></returns>
        private HdrColorBin GetBulkColor(AllocationProfile ap)
        {
            HdrColorBin aColor = null;
            foreach (HdrColorBin c in ap.BulkColors.Values)
            {
                aColor = c;
            }
            return aColor;
        }

        /// <summary>
        /// Unloads MethodOTSPlanProfile in to field by field object array.
        /// </summary>
        /// <returns>Object array</returns>
        public object[] ItemArray()
        {
            object[] ar = new object[9];
            ar[0] = this.Key;
            ar[1] = this.SG_RID;
            ar[2] = this.APPLY_OVERAGE_TO;
            return ar;
        }

        override public void Update(TransactionData td)
        {
            if (_methodData == null || base.Key < 0 || base.Method_Change_Type == eChangeType.add)
            {
                _methodData = new DCCartonRoundingMethodData(td);
            }
            _methodData.SG_RID = SG_RID;
            _methodData.ApplyOverageTo = APPLY_OVERAGE_TO;

            try
            {

                switch (base.Method_Change_Type)
                {
                    case eChangeType.add:
                        base.Update(td);
                        _methodData.InsertMethod(base.Key, td);
                        break;
                    case eChangeType.update:
                        base.Update(td);
                        _methodData.UpdateMethod(base.Key, td);
                        break;
                    case eChangeType.delete:
                        _methodData.DeleteMethod(base.Key, td);
                        base.Update(td);
                        break;
                }
            }
            catch (Exception e)
            {
                string message = e.ToString();
                throw;
            }
            finally
            {
                //TO DO:  whatever has to be done after an update or exception.
            }
        }

        public override bool WithinTolerance(double aTolerancePercent)
        {
            return true;
        }

        /// <summary>
        /// Returns a copy of this object.
        /// </summary>
        /// <param name="aSession">
        /// The current session
        /// </param>
        /// <param name="aCloneDateRanges">
        /// A flag identifying if date ranges are to be cloned or use the original</param>
        /// <param name="aCloneCustomOverrideModels">
        /// A flag identifying if custom override models are to be cloned or use the original
        /// </param>
        /// <returns>
        /// A copy of the object.
        /// </returns>
        override public ApplicationBaseMethod Copy(Session aSession, bool aCloneDateRanges, bool aCloneCustomOverrideModels)
        // End Track #5912
        {
            DCCartonRoundingMethod newAllocationGeneralMethod = null;

            try
            {
                newAllocationGeneralMethod = (DCCartonRoundingMethod)this.MemberwiseClone();
                newAllocationGeneralMethod.SG_RID = SG_RID;
                newAllocationGeneralMethod.APPLY_OVERAGE_TO = APPLY_OVERAGE_TO;

                return newAllocationGeneralMethod;
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        override public bool AuthorizedToUpdate(Session aSession, int aUserRID)
        {
            return true;
        }

        /// Returns a flag identifying if the user can view the data on the method.
        /// </summary>
        /// <param name="aSession">
        /// The current session
        /// </param>
        /// <param name="aUserRID">
        /// The internal key of the user</param>
        /// <returns>
        /// A flag.
        /// </returns>
        override public bool AuthorizedToView(Session aSession, int aUserRID)
        { 
            return true;
        }
    }

    internal class DCInfo
    {
        int _sglRid;
        int _totalUnitsAllocated;
        int _origUnitsAllocated;
        ProfileList _stores;
        bool _isRecievingDC;
        bool _includesReserveOnly;	// TT#1698-MD - stodd - Recieve Warning Error (total not spread) when processing Carton Rounding on Headers within a Group
        string _sglName;	// TT#1708-MD - stodd - Allocation is out of balance after DC Carton Rounding to Reserve

        public int SGLRid
        {
            get { return _sglRid; }
        }
        public int TotalUnitsAllocated
        {
            get { return _totalUnitsAllocated; }
            set { _totalUnitsAllocated = value; }
        }

        public int OrigUnitsAllocated
        {
            get { return _origUnitsAllocated; }
            set { _origUnitsAllocated = value; }	// TT#1698-MD - stodd - Recieve Warning Error (total not spread) when processing Carton Rounding on Headers within a Group
        }

        public bool IsRecievingDC
        {
            get { return _isRecievingDC; }
        }

        public ProfileList Stores
        {
            get { return _stores; }
            set { _stores = value; }
        }

		// begin TT#1708-MD - stodd - Allocation is out of balance after DC Carton Rounding to Reserve
        public string SglName
        {
            get { return _sglName; }
            set { _sglName = value; }
        }
		// end TT#1708-MD - stodd - Allocation is out of balance after DC Carton Rounding to Reserve

		// Begin TT#1698-MD - stodd - Recieve Warning Error (total not spread) when processing Carton Rounding on Headers within a Group
        public bool IncludesReserveOnly
        {
            get { return _includesReserveOnly; }
            set { _includesReserveOnly = value; }
        }
		// End TT#1698-MD - stodd - Recieve Warning Error (total not spread) when processing Carton Rounding on Headers within a Group
		
        public DCInfo(int sglRid, int totalUnitsAllocated, int origUnitsAllocated, bool isRecievingDC, string sglName)	// TT#1708-MD - stodd - Allocation is out of balance after DC Carton Rounding to Reserve
        {
            _sglRid = sglRid;
            _sglName = sglName;		// TT#1708-MD - stodd - Allocation is out of balance after DC Carton Rounding to Reserve
            _totalUnitsAllocated = totalUnitsAllocated;
            _origUnitsAllocated = origUnitsAllocated;
            _isRecievingDC = isRecievingDC;
            _stores = new ProfileList(eProfileType.Store);
            _includesReserveOnly = false;	// TT#1698-MD - stodd - Recieve Warning Error (total not spread) when processing Carton Rounding on Headers within a Group
        }
    }
}

