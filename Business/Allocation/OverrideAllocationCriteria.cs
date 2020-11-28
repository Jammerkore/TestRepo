using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using MIDRetail.Common;
using MIDRetail.Data;
using MIDRetail.DataCommon;


namespace MIDRetail.Business.Allocation
{

    
    /// <summary>
	/// Defines the overrides to general criteria to drive the allocation process.
	/// </summary>
    [Serializable]
	public class OverrideAllocationCriteria
	{
		//=======
		// FIELDS
		//=======
        private Session _session;
		private ApplicationSessionTransaction _appSessionTrans;
        private SessionAddressBlock _sab;
        private Audit _audit;
        private bool _processingAssortment;
		
		//=============
		// CONSTRUCTORS
		//=============
		/// <summary>
		/// Creates an instance of the AllocationOverrideMethod.
		/// </summary>
        /// <param name="aSession">Client Session</param>
        /// <param name="aApplicationTransaction">Application Session Transaction</param>
		public OverrideAllocationCriteria(Session aSession, ApplicationSessionTransaction aApplicationTransaction)
		{
            _session = aSession;
            _appSessionTrans = aApplicationTransaction;
            _sab = _appSessionTrans.SAB;
            _audit = _sab.ApplicationServerSession.Audit;
            _processingAssortment = false;
		}

		//========
		// METHODS
		//========

		/// <summary>
		/// Applies Allocation Criteria to an Assortment Profile.
		/// </summary>
        /// <param name="aWorkFlowStep">Allocation Workflow Step that instantiated the process</param>
        /// <param name="aAssortmentProfile">Assortment Profile to which to apply Criteria</param>
        /// <param name="aAllocationCriteria">Allocation Criteria to apply</param>
        /// <param name="aStatusMsg">Status Message area to provide information when failure occurs</param>
        /// <returns>True: when process action is successful; False: when process action fails (aStatusMsg will contain information about the failure)</returns>
		public bool ProcessAllocationCriteria(
            AllocationWorkFlowStep aWorkFlowStep, 
            AssortmentProfile aAssortmentProfile,
            AllocationCriteria aAllocationCriteria,
            out MIDException aStatusMsg)
		{
            bool actionSuccess;
            try
            {
                aStatusMsg = null;
                //aAssortmentProfile.ResetTempLocks(false); // Turn temp locks off  // TT#1016 - MD - Jellis - Action Failed
                _processingAssortment = true;
                actionSuccess = SetShippingHorizon(
                    aWorkFlowStep,
                    aAssortmentProfile,
                    aAllocationCriteria.BeginCdrRID,
                    aAllocationCriteria.ShipToCdrRID,  // TT#1105 - MD - Jellis - Null Reference in Group Allocation Method
                    out aStatusMsg);                   // TT#1105 - MD - Jellis - Null Reference in Group Allocation Method
                if (actionSuccess)
                {
                    aAssortmentProfile.ResetTempLocks(false); // TT#1016 - MD - Jellis - Action Failed
                    actionSuccess =
                        ProcessAllocationCriteria(
                            aWorkFlowStep,
                            (AllocationProfile)aAssortmentProfile,
                            aAllocationCriteria,
                            out aStatusMsg);
                    // begin TT#946 - MD - Jellis - Group Allocation Not Working
                    if (actionSuccess)
                    {
                        foreach (AllocationProfile ap in aAssortmentProfile.AssortmentMembers)
                        {
                            if (!ProcessAllocationCriteria(
                                aWorkFlowStep,
                                ap,
                                aAllocationCriteria,
                                out aStatusMsg))
                            {
                                actionSuccess = false;
                                break;
                            }
                        }
                        // begin TT#1597 - Jellis - MD - GA overallocates inventory maximum
                        if (actionSuccess)
                        {
                            actionSuccess = WriteHeader(aWorkFlowStep, aAssortmentProfile);
                        }
                        // end TT#1597 - Jellis - MD - GA overallocates inventory maximum
                    }
                    // end TT#946 - MD - Jellis - Group Allocation Not Working
                 }
            }
            catch (MIDException e)
            {
                aStatusMsg = e;
                actionSuccess = false;
            }
            catch (Exception e)
            {
                aStatusMsg = new MIDException(eErrorLevel.fatal, (int)eMIDTextCode.systemError, e.Message);
                actionSuccess = false;
            }
            finally
            {
                aAssortmentProfile.ResetTempLocks(true); // turn temp locks back on.
                // BEGIN TT#486 - md - stodd - manual merge
                aAssortmentProfile.SetStoreCapacityNotLoaded(); // TT#3145 - JSmith - Exceed capacity run manually versus run in a workflow give different results.
                // END TT#486 - md - stodd - manual merge
                _processingAssortment = false;
            }
            return actionSuccess;
        }

        /// <summary>
        /// Applies Allocation Criteria to an Allocation Profile
        /// </summary>
        /// <param name="aWorkFlowStep">Work Flow Step that originated this process</param>
        /// <param name="aAllocationProfile">Allocation Profile to which to apply the process</param>
        /// <param name="aAllocationCriteria">Allocation Criteria to apply</param>
        /// <param name="aStatusMsg">Status Message describing any failure</param>
        /// <returns>True: Successful application of Allocation Criteria; False: Application of criteria failes (in this case aStatusMsg will contain a message giving a reason for the failure.</returns>
        public bool ProcessAllocationCriteria(
            AllocationWorkFlowStep aWorkFlowStep, 
            AllocationProfile aAllocationProfile,
            AllocationCriteria aAllocationCriteria,
            out MIDException aStatusMsg)
        {
            bool actionSuccess = true;
            aStatusMsg = null;
            string auditMsg;
            if (aAllocationProfile == null)
            {
                auditMsg = MIDText.GetText(eMIDTextCode.msg_NotAllocationProfile);
                _audit.Add_Msg(
                    eMIDMessageLevel.Severe,
                    eMIDTextCode.msg_NotAllocationProfile,
                    auditMsg,
                    this.GetType().Name);
                aStatusMsg = new MIDException(eErrorLevel.severe,
                    (int)(eMIDTextCode.msg_NotAllocationProfile),
                    auditMsg);
                return false;
            }
            try
			{
                if (!_processingAssortment)
                {
                    aAllocationProfile.ResetTempLocks(false); // turn temp locks off.
                }
                // Begin TT#4988 - BVaughan - Performance
                #if DEBUG
                if ((aAllocationProfile is AssortmentProfile && !aAllocationProfile.isAssortmentProfile) || (!(aAllocationProfile is AssortmentProfile) && aAllocationProfile.isAssortmentProfile))
                {
                    throw new Exception("Object does not match AssortmentProfile in ProcessAllocationCriteria()");
                }
                #endif	
                // End TT#4988 - BVaughan - Performance
                if (aAllocationProfile.HeaderAllocationStatus == eHeaderAllocationStatus.ReceivedOutOfBalance)
				{
					string msg = string.Format(
                        _audit.GetText(eMIDTextCode.msg_HeaderStatusDisallowsAction, false), MIDText.GetTextOnly((int)aAllocationProfile.HeaderAllocationStatus));
                    auditMsg = 
                        (((AllocationBaseMethod)aWorkFlowStep.Method).Name + " " + aAllocationProfile.HeaderID + " " + msg);
					_audit.Add_Msg(
						eMIDMessageLevel.Warning,eMIDTextCode.msg_HeaderStatusDisallowsAction,
                        auditMsg,  
						this.GetType().Name);
                    _appSessionTrans.SetAllocationActionStatus(aAllocationProfile.Key, eAllocationActionStatus.NoActionPerformed);
                    aStatusMsg = new MIDException(eErrorLevel.warning,
                        (int)(eMIDTextCode.msg_HeaderStatusDisallowsAction),
                        auditMsg);
                    actionSuccess = false;
				}
				else if (aAllocationCriteria.OTSPlanRID == Include.NoRID
					&& aAllocationCriteria.OTSPlanPHL != Include.NoRID
					&& _appSessionTrans.GetColorLevelSequence() == aAllocationCriteria.OTSPlanPHLSeq
					&& aAllocationProfile.PlanLevelStartHnRID == aAllocationProfile.StyleHnRID) // Color level specified and more than one color
				{
                    auditMsg =
                        ((AllocationBaseMethod)aWorkFlowStep.Method).Name + " " + MIDText.GetTextOnly(eMIDTextCode.msg_al_CannotDetermineOTSColorWhenMultipleColors) + ": " + aAllocationProfile.HeaderID;
					_audit.Add_Msg(
						eMIDMessageLevel.Warning,
						eMIDTextCode.msg_al_CannotDetermineOTSColorWhenMultipleColors,
						auditMsg,
						this.GetType().Name);
					_appSessionTrans.SetAllocationActionStatus(aAllocationProfile.Key, eAllocationActionStatus.NoActionPerformed);
                    aStatusMsg = new MIDException(eErrorLevel.warning,
                        (int)(eMIDTextCode.msg_al_CannotDetermineOTSColorWhenMultipleColors),
                        auditMsg);
                    actionSuccess = false;
				}
				else if (aAllocationCriteria.OTSOnHandRID == Include.NoRID
					&& aAllocationCriteria.OTSOnHandPHL != Include.NoRID
                    && _appSessionTrans.GetColorLevelSequence() == aAllocationCriteria.OTSOnHandPHLSeq
					&& aAllocationProfile.PlanLevelStartHnRID == aAllocationProfile.StyleHnRID) // Color level specified and more than one color
				{
                    auditMsg =
                        ((AllocationBaseMethod)aWorkFlowStep.Method).Name + " " + MIDText.GetTextOnly(eMIDTextCode.msg_al_CannotDetermineOnHandColorWhenMultipleColors) + ": " + aAllocationProfile.HeaderID;
					_audit.Add_Msg(
						eMIDMessageLevel.Warning,
						eMIDTextCode.msg_al_CannotDetermineOnHandColorWhenMultipleColors,
						auditMsg,
						this.GetType().Name);
					_appSessionTrans.SetAllocationActionStatus(aAllocationProfile.Key, eAllocationActionStatus.NoActionPerformed);
                    aStatusMsg = new MIDException(eErrorLevel.warning,
                        (int)(eMIDTextCode.msg_al_CannotDetermineOTSColorWhenMultipleColors),
                        auditMsg);
                    actionSuccess = false;
				}
                // Begin TT#4988 - BVaughan - Performance
                //else if ((aAllocationProfile is AssortmentProfile)                 // TT#1140 - MD - Jellis - Group Allocation Header Inventory Min Max Options
                    //&& aAllocationCriteria.Inventory_MERCH_HN_RID == Include.NoRID // TT#1140 - MD - Jellis - Group Allocation Header Inventory Min Max Options
                    //&& aAllocationCriteria.Inventory_MERCH_PH_RID != Include.NoRID
                    //&& _appSessionTrans.GetColorLevelSequence() == aAllocationCriteria.Inventory_MERCH_PHL_SEQ
                    //&& aAllocationProfile.PlanLevelStartHnRID == aAllocationProfile.StyleHnRID)
                else if ((aAllocationProfile.isAssortmentProfile)                 // TT#1140 - MD - Jellis - Group Allocation Header Inventory Min Max Options
                    && aAllocationCriteria.Inventory_MERCH_HN_RID == Include.NoRID // TT#1140 - MD - Jellis - Group Allocation Header Inventory Min Max Options
                    && aAllocationCriteria.Inventory_MERCH_PH_RID != Include.NoRID
                    && _appSessionTrans.GetColorLevelSequence() == aAllocationCriteria.Inventory_MERCH_PHL_SEQ
                    && aAllocationProfile.PlanLevelStartHnRID == aAllocationProfile.StyleHnRID)
                // End TT#4988 - BVaughan - Performance
                {
                    auditMsg = string.Format(
                        _audit.GetText(
                        eMIDTextCode.msg_al_DynamicColorBasisInvalid, false),
                        aAllocationProfile.HeaderID,
                        MIDText.GetTextOnly(eMIDTextCode.lbl_InventoryBasis),
                        MIDText.GetTextOnly((int)((AllocationBaseMethod)aWorkFlowStep.Method).MethodType),
                        MIDText.GetTextOnly((int)eHierarchyLevelType.Color));
                    _audit.Add_Msg(
                        MIDText.GetMessageLevel((int)eMIDTextCode.msg_al_DynamicColorBasisInvalid),
                        eMIDTextCode.msg_al_DynamicColorBasisInvalid,
                        auditMsg,
                        GetType().Name);
                    _appSessionTrans.SetAllocationActionStatus(aAllocationProfile.Key, eAllocationActionStatus.NoActionPerformed);
                    aStatusMsg = new MIDException(eErrorLevel.warning,
                        (int)(eMIDTextCode.msg_al_CannotDetermineOTSColorWhenMultipleColors),
                        auditMsg);
                    actionSuccess = false;
                }
                    // begin TT#1140 - MD - Jellis - Group Allocation Header Inventory Min Max Options
                // Begin TT#4988 - BVaughan - Performance
                //else if (!(aAllocationProfile is AssortmentProfile)                 
                //    && aAllocationCriteria.HdrInventory_MERCH_HN_RID == Include.NoRID 
                //    && aAllocationCriteria.HdrInventory_MERCH_PH_RID != Include.NoRID
                //    && _appSessionTrans.GetColorLevelSequence() == aAllocationCriteria.HdrInventory_MERCH_PHL_SEQ
                //    && aAllocationProfile.PlanLevelStartHnRID == aAllocationProfile.StyleHnRID)
                else if (!(aAllocationProfile.isAssortmentProfile)
                    && aAllocationCriteria.HdrInventory_MERCH_HN_RID == Include.NoRID
                    && aAllocationCriteria.HdrInventory_MERCH_PH_RID != Include.NoRID
                    && _appSessionTrans.GetColorLevelSequence() == aAllocationCriteria.HdrInventory_MERCH_PHL_SEQ
                    && aAllocationProfile.PlanLevelStartHnRID == aAllocationProfile.StyleHnRID)
                // End TT#4988 - BVaughan - Performance
                {
                    auditMsg = string.Format(
                        _audit.GetText(
                        eMIDTextCode.msg_al_DynamicColorBasisInvalid, false),
                        aAllocationProfile.HeaderID,
                        MIDText.GetTextOnly(eMIDTextCode.lbl_InventoryBasis),
                        MIDText.GetTextOnly((int)((AllocationBaseMethod)aWorkFlowStep.Method).MethodType),
                        MIDText.GetTextOnly((int)eHierarchyLevelType.Color));
                    _audit.Add_Msg(
                        MIDText.GetMessageLevel((int)eMIDTextCode.msg_al_DynamicColorBasisInvalid),
                        eMIDTextCode.msg_al_DynamicColorBasisInvalid,
                        auditMsg,
                        GetType().Name);
                    _appSessionTrans.SetAllocationActionStatus(aAllocationProfile.Key, eAllocationActionStatus.NoActionPerformed);
                    aStatusMsg = new MIDException(eErrorLevel.warning,
                        (int)(eMIDTextCode.msg_al_CannotDetermineOTSColorWhenMultipleColors),
                        auditMsg);
                    actionSuccess = false;
                }
                    // end TT#1140 - MD - Jellis - Group Allocation Header Inventory Min Max Options
				else
				{
                    if (!aAllocationProfile.StoresLoaded)
                    {
                        aAllocationProfile.LoadStores();  
                    }
					aAllocationProfile.PercentNeedLimit = aAllocationCriteria.PercentNeedLimit;
   
					Index_RID storeIdxRID;
					bool exceedCapacity;
					bool calculateCapacityMaximums = false;
					double exceedCapacityPercent;
                    if (aAllocationCriteria.CapacityStoreGroupRID != Include.NoRID)
                    {
                        ProfileList capacityStoreGroupList = StoreMgmt.StoreGroup_GetLevelListFilled(aAllocationCriteria.CapacityStoreGroupRID); //_sab.StoreServerSession.GetStoreGroupLevelList(aAllocationCriteria.CapacityStoreGroupRID);
                        if (capacityStoreGroupList != null)
                        {
                            foreach (StoreGroupLevelProfile sglp in capacityStoreGroupList)
                            {
                                if (aAllocationCriteria.CapacityStrGroupLvlExists(sglp.Key))
                                {
                                    exceedCapacity = false;
                                    if (aAllocationCriteria.ExceedCapacity)
                                    {
                                        exceedCapacity = aAllocationCriteria.ExceedCapacity;
                                        exceedCapacityPercent = double.MaxValue;
                                    }
                                    else
                                    {
                                        exceedCapacity = aAllocationCriteria.GetStrGroupLvlExceedCapacity(sglp.Key);
                                        if (exceedCapacity)
                                        {
                                            exceedCapacityPercent = aAllocationCriteria.GetStrGroupLvlExceedCapacityByPct(sglp.Key);
                                            if (exceedCapacityPercent <= 0)
                                            {
                                                exceedCapacityPercent = double.MaxValue;
                                            }
                                        }
                                        else
                                        {
                                            exceedCapacityPercent = 0;
                                        }
                                    }
                                    foreach (StoreProfile sp in sglp.Stores)
                                    {
                                        storeIdxRID = aAllocationProfile.StoreIndex(sp.Key);
                                        if (exceedCapacity != aAllocationProfile.GetStoreMayExceedCapacity(storeIdxRID)
                                            || exceedCapacityPercent != aAllocationProfile.GetStoreCapacityExceedByPct(storeIdxRID))
                                        {
                                            calculateCapacityMaximums = true;
                                        }
                                        aAllocationProfile.SetStoreMayExceedCapacity(storeIdxRID, exceedCapacity);
                                        aAllocationProfile.SetStoreCapacityExceedByPct(storeIdxRID, exceedCapacityPercent);
                                    }
                                }
                            }
                            if (calculateCapacityMaximums)
                            {
                                aAllocationProfile.CalculateCapacityMaximum();
                            }
                        }
                    }
        
                    foreach (Index_RID strIdxRID in _appSessionTrans.StoreIndexRIDArray())
                    {
                        aAllocationProfile.SetStoreMayExceedMax(strIdxRID, aAllocationCriteria.ExceedMaximums);
                    }
                    // begin TT#1140 - MD - Jellis - Group Allocation - Header Inventory Min Max Options
                    //int inventoryBasisHnRID = Include.NoRID;
                    //if (aAllocationCriteria.InventoryMinMax) // TT#946 - MD - Jellis - Group Allocation Not Working
                    //{                                        // TT#946 - MD - Jellis - Group Allocation Not Working
                    //    if (aAllocationCriteria.Inventory_MERCH_HN_RID == Include.NoRID)
                    //    {
                    //        if (aAllocationCriteria.Inventory_MERCH_PH_RID == Include.NoRID)
                    //        {
                    //            HierarchyNodeProfile hnp =
                    //                aAllocationProfile.AppSessionTransaction.GetPlanLevelData(aAllocationProfile.PlanLevelStartHnRID);
                    //            if (hnp == null)
                    //            {
                    //                throw new MIDException(eErrorLevel.severe,
                    //                    (int)(eMIDTextCode.msg_al_PlanLevelUndetermined),
                    //                    MIDText.GetText(eMIDTextCode.msg_al_PlanLevelUndetermined));
                    //            }
                    //            else
                    //            {
                    //                inventoryBasisHnRID = hnp.Key;
                    //            }
                    //        }
                    //        else
                    //        {
                    //            inventoryBasisHnRID =
                    //                aAllocationProfile.AppSessionTransaction.GetAncestorDataByLevel
                    //                    (aAllocationCriteria.Inventory_MERCH_PH_RID,
                    //                     aAllocationProfile.PlanLevelStartHnRID,
                    //                     aAllocationCriteria.Inventory_MERCH_PHL_SEQ).Key;
                    //        }
                    //    }
                    //    else
                    //    {
                    //        inventoryBasisHnRID = aAllocationCriteria.Inventory_MERCH_HN_RID;
                    //    }
                    //}         // TT#946 - MD - Jellis - Group Allocation Not Working
                    //if (!aAllocationProfile.SetGradeMinimumMaximumType(aAllocationCriteria.InventoryMinMax, inventoryBasisHnRID, out aStatusMsg))
                    //{
                    //    return false;
                    //}
                    // Begin TT#4988 - BVaughan - Performance
                    #if DEBUG
                    if ((aAllocationProfile is AssortmentProfile && !aAllocationProfile.isAssortmentProfile) || (!(aAllocationProfile is AssortmentProfile) && aAllocationProfile.isAssortmentProfile))
                    {
                        throw new Exception("Object does not match AssortmentProfile in ProcessAllocationCriteria()");
                    }
                    #endif
                    //if (aAllocationProfile is AssortmentProfile)
                    if (aAllocationProfile.isAssortmentProfile)
                    // End TT#4988 - BVaughan - Performance
                    {
                        if (!aAllocationProfile.SetGradeMinimumMaximumType(
                            aAllocationCriteria.InventoryMinMax,
                            aAllocationCriteria.Inventory_MERCH_HN_RID,
                            aAllocationCriteria.Inventory_MERCH_PH_RID,
                            aAllocationCriteria.Inventory_MERCH_PHL_SEQ,
                            out aStatusMsg))
                        {
                            return false;
                        }
                    }
                    else
                    {
                        if (!aAllocationProfile.SetGradeMinimumMaximumType(
                            aAllocationCriteria.HdrInventoryMinMax,
                            aAllocationCriteria.HdrInventory_MERCH_HN_RID,
                            aAllocationCriteria.HdrInventory_MERCH_PH_RID,
                            aAllocationCriteria.HdrInventory_MERCH_PHL_SEQ,
                            out aStatusMsg))
                        {
                            return false;
                        }
                    }
                    // end TT#1140 - MD - Jellis - Group Allocation - Header Inventory Min Max Opetions
                    // begin TT#1114 - MD - Jellis - Need Analysis Not showing
                    // Begin TT#4988 - BVaughan - Performance
                    #if DEBUG
                    if ((aAllocationProfile is AssortmentProfile && !aAllocationProfile.isAssortmentProfile) || (!(aAllocationProfile is AssortmentProfile) && aAllocationProfile.isAssortmentProfile))
                    {
                        throw new Exception("Object does not match AssortmentProfile in ProcessAllocationCriteria()");
                    }
                    #endif	
                    //if ((aAllocationProfile is AssortmentProfile)
                    //    || (!_processingAssortment
                    //        && aAllocationProfile.AsrtRID == Include.NoRID))
                    if ((aAllocationProfile.isAssortmentProfile)
                        || (!_processingAssortment
                            && aAllocationProfile.AsrtRID == Include.NoRID))
                    // End TT#4988 - BVaughan - Performance
                    {
                        // end TT#1114 - MD - Jellis - Need Analysis Not showing
                        if (aAllocationCriteria.GradeWeekCount > 0)
                        {
                            aAllocationProfile.GradeWeekCount = aAllocationCriteria.GradeWeekCount;
                        }
                        if (aAllocationCriteria.OTSOnHandRID == Include.NoRID)
                        {
                            if (aAllocationCriteria.OTSOnHandPHL == Include.NoRID)
                            {
                                if (aAllocationCriteria.OTSOnHandPHLSeq == 0 && !aAllocationCriteria.OnHandUnspecified)
                                {
                                    HierarchyNodeProfile hnp = _appSessionTrans.GetPlanLevelData(aAllocationProfile.PlanLevelStartHnRID);
                                    if (hnp == null)
                                    {
                                        throw new MIDException(eErrorLevel.severe,
                                            (int)(eMIDTextCode.msg_al_PlanLevelUndetermined),
                                            MIDText.GetText(eMIDTextCode.msg_al_PlanLevelUndetermined));
                                    }
                                    else
                                    {
                                        aAllocationProfile.OnHandHnRID = hnp.Key;
                                    }
                                }
                            }
                            else
                            {
                                aAllocationProfile.OnHandHnRID =
                                    _appSessionTrans.GetAncestorDataByLevel(
                                        aAllocationCriteria.OTSOnHandPHL,
                                        aAllocationProfile.PlanLevelStartHnRID,
                                        aAllocationCriteria.OTSOnHandPHLSeq).Key;
                            }
                        }
                        else
                        {
                            aAllocationProfile.OnHandHnRID = aAllocationCriteria.OTSOnHandRID;
                        }
                        if (aAllocationCriteria.OTSPlanFactorPercent > 0)
                        {
                            aAllocationProfile.PlanFactor = aAllocationCriteria.OTSPlanFactorPercent;
                        }

                        if (aAllocationCriteria.OTSPlanRID == Include.NoRID)
                        {
                            if (aAllocationCriteria.OTSPlanPHL == Include.NoRID)
                            {
                                if (aAllocationCriteria.OTSPlanPHLSeq == 0 && !aAllocationCriteria.MerchUnspecified)
                                {
                                    HierarchyNodeProfile hnp = _appSessionTrans.GetPlanLevelData(aAllocationProfile.PlanLevelStartHnRID);
                                    if (hnp == null)
                                    {
                                        throw new MIDException(eErrorLevel.severe,
                                            (int)(eMIDTextCode.msg_al_PlanLevelUndetermined),
                                            MIDText.GetText(eMIDTextCode.msg_al_PlanLevelUndetermined));
                                    }
                                    else
                                        aAllocationProfile.PlanHnRID = hnp.Key;
                                }
                            }
                            else
                            {
                                aAllocationProfile.PlanHnRID =
                                    _appSessionTrans.GetAncestorDataByLevel(
                                        aAllocationCriteria.OTSPlanPHL,
                                        aAllocationProfile.PlanLevelStartHnRID, aAllocationCriteria.OTSPlanPHLSeq).Key;
                            }
                        }
                        else
                        {
                            aAllocationProfile.PlanHnRID = aAllocationCriteria.OTSPlanRID;
                        }
                    }  // TT#1114 - MD - Jellis - Need Analysis Not showing
                    foreach (HdrColorBin c in aAllocationProfile.BulkColors.Values)
					{
                        if (aAllocationCriteria.ColorMinMaxExists(c.ColorCodeRID))
						{
                            aAllocationProfile.SetColorMaximum(c.ColorCodeRID, aAllocationCriteria.GetColorMax(c.ColorCodeRID));
                            aAllocationProfile.SetColorMinimum(c.ColorCodeRID, aAllocationCriteria.GetColorMin(c.ColorCodeRID));
						}
						else
						{
                            aAllocationProfile.SetColorMaximum(c.ColorCodeRID, aAllocationCriteria.AllColorMaximum);
                            aAllocationProfile.SetColorMinimum(c.ColorCodeRID, aAllocationCriteria.AllColorMinimum);
						}
                        aAllocationProfile.SetColorMultiple(c.ColorCodeRID, aAllocationCriteria.AllColorMultiple);
						foreach(HdrSizeBin s in c.ColorSizes.Values)
						{

                            aAllocationProfile.SetSizeMaximum(c.ColorCodeRID, s.SizeCodeRID, aAllocationCriteria.GetColorSizeMax(c.ColorCodeRID, s.SizeCodeRID)); 
                            aAllocationProfile.SetSizeMinimum(c.ColorCodeRID, s.SizeCodeRID, aAllocationCriteria.GetColorSizeMin(c.ColorCodeRID, s.SizeCodeRID)); 
                            aAllocationProfile.SetSizeMultiple(c.ColorCodeRID, s.SizeCodeRID, aAllocationCriteria.AllSizeMultiple); 
						}
					}

                    aAllocationProfile.PackRoundingOverrideList = aAllocationCriteria.PackRoundingOverrideList;

                    
                    
                    if (!aAllocationProfile.AllocationStarted ||                                 
                        (aAllocationProfile.AllocationStarted && aAllocationProfile.AllUnitsInReserve))    
                    {
						if (aAllocationCriteria.ReserveQty != Include.UndefinedReserve)
						{
                            AllocateReserveSpecification ars =
                                 new AllocateReserveSpecification(
                                     aAllocationCriteria.ReserveIsPercent, 
                                     aAllocationCriteria.ReserveQty, 
                                     aAllocationCriteria.ReserveAsPacks, 
                                     aAllocationCriteria.ReserveAsBulk);
                            aAllocationProfile.AllocateReserve(ars);
						}

                        if (aAllocationCriteria.StoreVSWOverride != null)
                        {
                            if (aAllocationProfile.BeginDay == Include.UndefinedDate)
                            {
                                aAllocationProfile.SetStoreImoCriteria(aAllocationCriteria.StoreVSWOverride, true);  
                            }
                        }
                    }

                    bool gradesEqual = true;
					if (aAllocationCriteria.GradeCount > 0)				
					{
                        int boundaryCount = 0;
                        Dictionary<int, Dictionary<double, GroupAllocationGradeBin>> allocationGrades =
                            aAllocationCriteria.GetGroupAllocationGrades();
						Dictionary<double, GroupAllocationGradeBin> gradeBoundarys = null;	
                        foreach (Dictionary<double, GroupAllocationGradeBin> boundaryHT in allocationGrades.Values)
                        {
                            boundaryCount += boundaryHT.Values.Count;
							if (gradeBoundarys != null)
							{
							    if (gradeBoundarys.Count != boundaryHT.Count)
								{
								     gradesEqual = false;
								}
								else
								{	 
						    	    foreach (double boundary in boundaryHT.Keys)
                                    {
							    	    if (!gradeBoundarys.ContainsKey(boundary))
						    		    {
						    		        gradesEqual = false;
						    		    }
								    }
                                }
							}
							gradeBoundarys = boundaryHT;
                        }
						if (!gradesEqual)
						{
						    //throw new MIDException.....
						}	
                        // begin TT#1009 - MD - Jellis - Override Grades not honored
                        //if (!aAllocationProfile.AllocationStarted ||                                 
                        //    (aAllocationProfile.AllocationStarted && aAllocationProfile.AllUnitsInReserve))   
                        if (aAllocationProfile.AllocationStarted && !aAllocationProfile.AllUnitsInReserve)
                            // end TT#1009 - MD - Jellis - Override Grades not honored
                        {
                            ArrayList gradeList = aAllocationProfile.GradeList;
                            Dictionary<int, int> gradeCounts = new Dictionary<int,int>();
                            int gradeCount;
                            foreach (AllocationGradeBin grade in gradeList)
                            {
                                if (!gradeBoundarys.ContainsKey(grade.LowBoundary))
                                {
                                    gradesEqual = false;
                                    break;
                                }
                                if (!gradeCounts.TryGetValue(grade.GradeSglRID, out gradeCount))
                                {
                                    gradeCount = 0;
                                    //gradeCounts.Add(grade.GradeSglRID, gradeCount); // TT#946 - MD - Jellis - Group Allocation Not Working
                                }
                                else                                       // TT#946 - MD - Jellis - Group Allocation Not Working
                                {                                          // TT#946 - MD - Jellis - Group Allocation Not Working
                                    gradeCounts.Remove(grade.GradeSglRID); // TT#946 - MD - Jellis - Group Allocation Not Working
                                }                                          // TT#946 - MD - Jellis - Group Allocation Not Working
                                gradeCount++;                              // TT#946 - MD - Jellis - Group Allocation Not Working
                                gradeCounts.Add(grade.GradeSglRID, gradeCount); // TT#946 - MD - Jellis - Group Allocation Not Working
                            }
                       
                            foreach (int gradeCountA in gradeCounts.Values)
                            {
                                if (gradeCountA != gradeBoundarys.Count)
                                {
                                    gradesEqual = false;
                                }
                            }
                        }
                        if (gradesEqual)
                        {
                            aAllocationProfile.GradeSG_RID = aAllocationCriteria.GradeStoreGroupRID;
                            AllocationGradeBin gradeBin;
                            ArrayList gradeList = new ArrayList();
                            AssortmentProfile ap = aAllocationProfile as AssortmentProfile;
                            if (ap != null)
                            {
                                foreach (Dictionary<double, GroupAllocationGradeBin> boundaryHT in allocationGrades.Values)
                                {
                                    foreach (GroupAllocationGradeBin agb in boundaryHT.Values)
                                    {
                                        gradeBin = new AllocationGradeBin();
                                        gradeBin.SetGradeAttributeSet(agb.GradeSglRID);
                                        gradeBin.SetGrade(agb.Grade);
                                        gradeBin.SetLowBoundary(agb.LowBoundary);
                                        gradeBin.SetGradeAdMinimum(0);
                                        gradeBin.SetGradeColorMaximum(int.MaxValue);
                                        gradeBin.SetGradeColorMinimum(0);
                                        gradeBin.SetGradeMaximum(agb.GroupGradeMaximum);
                                        gradeBin.SetGradeMinimum(agb.GroupGradeMinimum);
                                        gradeBin.SetGradeOriginalAdMinimum(0);
                                        gradeBin.SetGradeOriginalMaximum(agb.GroupGradeOriginalMaximum);
                                        gradeBin.SetGradeOriginalMinimum(agb.GroupGradeOriginalMinimum);
                                        gradeBin.SetGradeShipUpTo(agb.GradeShipUpTo);
                                        gradeList.Add(gradeBin);
                                    }
                                }
                            }
                            else
                            {
                                // begin TT#951 - MD - Jellis - Line Item Minimum not working
                                if (aAllocationCriteria.LineItemMinOverrideInd == true)
                                {
                                    foreach (Dictionary<double, GroupAllocationGradeBin> boundaryHT in allocationGrades.Values)
                                    {
                                        foreach (GroupAllocationGradeBin agb in boundaryHT.Values)
                                        {
                                            gradeBin = new AllocationGradeBin();
                                            gradeBin.SetGradeAttributeSet(agb.GradeSglRID);
                                            gradeBin.SetGrade(agb.Grade);
                                            gradeBin.SetLowBoundary(agb.LowBoundary);
                                            gradeBin.SetGradeAdMinimum(agb.GradeAdMinimum);
                                            gradeBin.SetGradeColorMaximum(agb.GradeColorMaximum);
                                            gradeBin.SetGradeColorMinimum(agb.GradeColorMinimum);
                                            gradeBin.SetGradeMaximum(int.MaxValue);                       // NOTE:  Line Item Minimum overrides individual header maximums as well as minimums
                                            gradeBin.SetGradeMinimum(aAllocationCriteria.LineItemMinOverride);
                                            gradeBin.SetGradeOriginalAdMinimum(agb.GradeOriginalAdMinimum);
                                            gradeBin.SetGradeOriginalMaximum(agb.GradeOriginalMaximum);
                                            gradeBin.SetGradeOriginalMinimum(agb.GradeOriginalMinimum);
                                            gradeBin.SetGradeShipUpTo(agb.GradeShipUpTo);
                                            gradeList.Add(gradeBin);
                                        }
                                    }
                                }
                                else
                                {
                                    // end TT#951 - MD - Jellis - Line Item Minimum not working
                                    foreach (Dictionary<double, GroupAllocationGradeBin> boundaryHT in allocationGrades.Values)
                                    {
                                        foreach (GroupAllocationGradeBin agb in boundaryHT.Values)
                                        {
                                            gradeBin = new AllocationGradeBin();
                                            gradeBin.SetGradeAttributeSet(agb.GradeSglRID);
                                            gradeBin.SetGrade(agb.Grade);
                                            gradeBin.SetLowBoundary(agb.LowBoundary);
                                            gradeBin.SetGradeAdMinimum(agb.GradeAdMinimum);
                                            gradeBin.SetGradeColorMaximum(agb.GradeColorMaximum);
                                            gradeBin.SetGradeColorMinimum(agb.GradeColorMinimum);
                                            gradeBin.SetGradeMaximum(agb.GradeMaximum);
                                            gradeBin.SetGradeMinimum(agb.GradeMinimum);
                                            gradeBin.SetGradeOriginalAdMinimum(agb.GradeOriginalAdMinimum);
                                            gradeBin.SetGradeOriginalMaximum(agb.GradeOriginalMaximum);
                                            gradeBin.SetGradeOriginalMinimum(agb.GradeOriginalMinimum);
                                            gradeBin.SetGradeShipUpTo(agb.GradeShipUpTo);
                                            gradeList.Add(gradeBin);
                                        }
                                    }
                                }  // TT#951 - MD - Jellis - Line Item Minimum not working
                            }
                            aAllocationProfile.GradeList = gradeList;
                        }
					}
                    if (actionSuccess) 
                    {
                        // begin TT#1597 - Jellis - MD - GA Over allocates Inventory Maximum
                        if (!_processingAssortment)
                        {
                            actionSuccess = WriteHeader(aWorkFlowStep, aAllocationProfile);
                        }
                        // end TT#1597 - Jellis - MD - GA Over allocates Inventory Maximum
                        //actionSuccess = aAllocationProfile.WriteHeader();
                        //if (actionSuccess)
                        //{
                        //    _appSessionTrans.SetAllocationActionStatus(aAllocationProfile.Key, eAllocationActionStatus.ActionCompletedSuccessfully);
                        //    _appSessionTrans.WriteAllocationAuditInfo
                        //        (aAllocationProfile.Key,
                        //        0,
                        //        ((AllocationBaseMethod)aWorkFlowStep.Method).MethodType,
                        //        ((AllocationBaseMethod)aWorkFlowStep.Method).Key,
                        //        ((AllocationBaseMethod)aWorkFlowStep.Method).Name,
                        //        aWorkFlowStep.Component.ComponentType,
                        //        null,
                        //        null,
                        //        0,
                        //        0
                        //        );
                        //}
                        //else
                        //{
                        //    _appSessionTrans.SetAllocationActionStatus(aAllocationProfile.Key, eAllocationActionStatus.ActionFailed);
                        //}
                    } 
                    else 
                    {
                        _appSessionTrans.SetAllocationActionStatus(aAllocationProfile.Key, eAllocationActionStatus.ActionFailed);  
                    } 
				} 
			}
			catch (Exception error)
			{
                _appSessionTrans.SetAllocationActionStatus(aAllocationProfile.Key, eAllocationActionStatus.ActionFailed);
				string message = error.ToString();
				throw;
			}
			finally
			{
                if (!_processingAssortment)
                {
                    aAllocationProfile.ResetTempLocks(true);
                    // BEGIN TT#486 - md - stodd - manual merge
                    aAllocationProfile.SetStoreCapacityNotLoaded(); // TT#3145 - JSmith - Exceed capacity run manually versus run in a workflow give different results.
                    // END TT#486 - md - stodd - manual merge
                }
                // begin TT#1146 - MD - Jellis - "f" grade stores not outted correctly and get an allocation
                _appSessionTrans.ClearAllocationCubeGroup();
                _appSessionTrans.ResetFilter(eProfileType.Store);
                // end TT#1146 - MD _ Jellis - "f" grade stores not outted correctly and get an allocation

			}
            return actionSuccess;
		}
        // begin TT#1597 - Jellis - MD - GA overallocates inventory maximum
        private bool WriteHeader(AllocationWorkFlowStep aWorkFlowStep, AllocationProfile aAllocationProfile)
        {
            bool actionSuccess = aAllocationProfile.WriteHeader();
            if (actionSuccess)
            {
                _appSessionTrans.SetAllocationActionStatus(aAllocationProfile.Key, eAllocationActionStatus.ActionCompletedSuccessfully);
                _appSessionTrans.WriteAllocationAuditInfo
                    (aAllocationProfile.Key,
                    0,
                    ((AllocationBaseMethod)aWorkFlowStep.Method).MethodType,
                    ((AllocationBaseMethod)aWorkFlowStep.Method).Key,
                    ((AllocationBaseMethod)aWorkFlowStep.Method).Name,
                    aWorkFlowStep.Component.ComponentType,
                    null,
                    null,
                    0,
                    0
                    );
            }
            else
            {
                _appSessionTrans.SetAllocationActionStatus(aAllocationProfile.Key, eAllocationActionStatus.ActionFailed);
            }
            return actionSuccess;
        }
        // end TT#1597 - Jellis - MD - GA overallocates inventory maximum

        /// <summary>
        /// Sets Shipping Horizon when assortment has not started allocation
        /// </summary>
        /// <param name="aAssortmentProfile">AssortmentProfile</param>
        /// <param name="aBeginCdrRID">Shipping Horizon Start Calendar Date Range RID</param>
        /// <param name="aShipCdrRID">Shipping Horizon Ship To Day Calendar Date Range RID</param>
        /// <returns></returns>
        private bool SetShippingHorizon(AllocationWorkFlowStep aWorkFlowStep, AssortmentProfile aAssortmentProfile, int aBeginCdrRID, int aShipCdrRID, out MIDException aStatusMsg) // TT#1105 - MD - Jellis - Null Reference in Group Allocation Method
        {
            if (!aAssortmentProfile.AllocationStarted ||
				(aAssortmentProfile.AllocationStarted && aAssortmentProfile.AllUnitsInReserve))
			{
                aStatusMsg = null; // TT#1105 - MD - Jellis - Null Reference in Group Allocation Method
				aAssortmentProfile.Action(eAllocationMethodType.BackoutAllocation, new GeneralComponent(eGeneralComponentType.Total), double.MaxValue, Include.AllStoreFilterRID, false);
				if (aShipCdrRID != Include.UndefinedCalendarDateRange)
				{
					aAssortmentProfile.ShipToDay = ((DayProfile)(_appSessionTrans.SAB.ApplicationServerSession.Calendar.GetFirstDayOfRange(aShipCdrRID))).Date;
				}
				if (aBeginCdrRID != Include.UndefinedCalendarDateRange)
				{
					aAssortmentProfile.BeginDay = ((DayProfile)(_appSessionTrans.SAB.ApplicationServerSession.Calendar.GetFirstDayOfRange(aBeginCdrRID))).Date;
				}
                return true;
            }
            else
            {
                // begin TT#1105 - MD - Jellis - Null Reference in Group Allocation Method
                //_audit.Add_Msg(
                //    eMIDMessageLevel.Warning,
                //    eMIDTextCode.msg_MethodIgnored,
                //    string.Format(
                //    MIDText.GetText(eMIDTextCode.msg_MethodIgnored),
                //    aAssortmentProfile.HeaderID, 
                //    MIDText.GetTextOnly(eMIDTextCode.frm_GeneralAllocationMethod),
                //    MIDText.GetTextOnly((int)(aWorkFlowStep.Method.MethodType)) + " " + ((AllocationBaseMethod)aWorkFlowStep.Method).Name),
                //    this.GetType().Name);
                string msg =
                    string.Format(
                    MIDText.GetText(eMIDTextCode.msg_MethodIgnored),
                    aAssortmentProfile.HeaderID,
                    MIDText.GetTextOnly(eMIDTextCode.frm_GroupAllocationMethod),
                    MIDText.GetTextOnly((int)(aWorkFlowStep.Method.MethodType)) + " " + ((AllocationBaseMethod)aWorkFlowStep.Method).Name);
                aStatusMsg =
                    new MIDException(eErrorLevel.warning,
                        (int)eMIDTextCode.msg_MethodIgnored,
                        msg);
                _audit.Add_Msg(
                    eMIDMessageLevel.Warning,
                    eMIDTextCode.msg_MethodIgnored,
                    msg,
                    this.GetType().Name);
                // end  TT#1105 - MD - Jellis - Null Reference in Group Allocation Method
				_appSessionTrans.SetAllocationActionStatus(aAssortmentProfile.Key, eAllocationActionStatus.NoActionPerformed);
                return false;
            }
        }
	}
}
