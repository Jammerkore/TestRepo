using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;

using MIDRetail.Common;
using MIDRetail.Data;
using MIDRetail.DataCommon;
using MIDRetail.Business.Allocation;

namespace MIDRetail.Business
{
	//====================================================================
	// ISSUE 4827 - this entire class was created because of this issue
	//====================================================================
	/// <summary>
	/// Summary description for BaseBusinessRoutines.
	/// </summary>
	abstract public class BaseBusinessRoutines
	{
		private SessionAddressBlock _SAB;
		private ApplicationSessionTransaction _transaction;
		// BEGIN TT#2225 - stodd - VSW ANF Enhancement (IMO)
		private PlanCube _storePlanCube;
		// END TT#2225 - stodd - VSW ANF Enhancement (IMO)
		//BEGIN TT#583-MD - stodd -  Add IMO Audit info
		private Audit _audit;
		//END TT#583-MD - stodd -  Add IMO Audit info

		public SessionAddressBlock SAB
		{
			get	{ return _SAB; }
		}

		public ApplicationSessionTransaction Transaction
		{
			get	{ return _transaction; }
		}

		public BaseBusinessRoutines(SessionAddressBlock sab, ApplicationSessionTransaction aTransaction)
		{
			_SAB = sab;
			_transaction = aTransaction;
			// BEGIN TT#2225 - stodd - VSW ANF Enhancement (IMO)
			_storePlanCube = null;
			// END TT#2225 - stodd - VSW ANF Enhancement (IMO)
            // Begin TT#1157 - MD - JSmith - Store Eligibility Load ANF
            //_audit = _SAB.ApplicationServerSession.Audit;
            if (_SAB != null)
            {
                _audit = _SAB.ApplicationServerSession.Audit;
            }
            // ENd TT#1157 - MD - JSmith - Store Eligibility Load ANF
		}

		#region A&F Base Methods
		//================================
		// A&F Base method definitions
		//================================
		virtual public bool IsPresentationMinDefined()
		{
			return false;
		}

		virtual public bool ApplyPresentationMinPlusSales(int storeIndex)
		{
			return false;
		}

		virtual public VariableProfile GetPresentationMinVariable()
		{
			return null;
		}

		//Begin TT#875 - JScott - Add Base Code to Support A&F Custom Features

		virtual public string GetForecastMonitorWOSModLabel()
		{
			return "WOSMod";
		}

		virtual public string GetForecastMonitorApplyPreMinLabel()
		{
			return "Apply-Pre Min";
		}
		//End TT#875 - JScott - Add Base Code to Support A&F Custom Features
		// END A&F Base method definitions

		#endregion

		// BEGIN TT#2225 - stodd - VSW ANF Enhancement (IMO)
		#region VSW Methods
		virtual public IMOProfileList GetVSW_Max(
            ProfileList allStoreList, 
            int planLevelStartHnRID, 
            StoreShipDay[] shipDayList, 
            int planLevelHnRid,
            int eligibilityHnRID)
		{
			//BEGIN TT#583-MD - stodd -  Add IMO Audit info
			if (_audit.LoggingLevel == eMIDMessageLevel.Debug)
			{
				HierarchyNodeProfile startHnp = _SAB.HierarchyServerSession.GetNodeData(planLevelStartHnRID, false, true);
				HierarchyNodeProfile hnp = _SAB.HierarchyServerSession.GetNodeData(planLevelHnRid, false, true);
				string message = "Item Max Calculation Starting. Plan Level Start Hierarchy Node: " + startHnp.Text + " Plan Level Hierarchy Node: " + hnp.Text;
				this._audit.Add_Msg(eMIDMessageLevel.Debug,
									message,
									this.GetType().Name);
			}
			//END TT#583-MD - stodd -  Add IMO Audit info
            
			IMOProfileList ipl = this.SAB.HierarchyServerSession.GetNodeIMOList(allStoreList, planLevelStartHnRID);
			for (int i=0; i<ipl.Count;i++)
			{
				IMOProfile imop = (IMOProfile)ipl[i];
                int IMOOriginalValue = 0;
                int IMOFWOSOriginalValue = 0;  // TT#3804 - AGallagher - Item/FWOS Max option in Global Settings
                // BEGIN TT#933-MD - AGallagher - Item Max vs. FWOS Max
                if (SAB.ClientServerSession.GlobalOptions.VSWItemFWOSMax != eVSWItemFWOSMax.Default)
                {
                    IMOOriginalValue = imop.IMOMaxValue;
                }
                // END TT#933-MD - AGallagher - Item Max vs. FWOS Max
				if ((imop.IMOFWOS_Max > 0 && imop.IMOFWOS_Max != int.MaxValue) || imop.IMOFWOS_MaxType == eModifierType.Model)
				{
					if (imop.IMOStoreRID == shipDayList[i].Store.Key)
					{
						imop.IMOMaxValue = GetIMOMaxValue(
                            imop, 
                            planLevelHnRid, 
                            eligibilityHnRID, 
                            shipDayList[i], 
                            (StoreProfile)allStoreList.FindKey(imop.IMOStoreRID));
					}
					else // this is to catch if the IMO Profile list is out of sync with the shipDayList
					{
						StoreShipDay storeShipDay = new StoreShipDay(); //Include.UndefinedStoreRID, Include.UndefinedDate);
						foreach (StoreShipDay ssd in shipDayList)
						{
							if (ssd.Store.Key == imop.IMOStoreRID)
							{
								storeShipDay = ssd;
								break;
							}
						}

						imop.IMOMaxValue = GetIMOMaxValue(
                            imop, 
                            planLevelHnRid, 
                            eligibilityHnRID,
                            storeShipDay, 
                            (StoreProfile)allStoreList.FindKey(imop.IMOStoreRID));
					}	
				}
				// BEGIN TT#2352 - stodd - zero is not holding when applied to FWOS Max field
				else if (imop.IMOFWOS_Max == 0)
				{
					imop.IMOMaxValue = 0;
				}
				// END TT#2352 - stodd - zero is not holding when applied to FWOS Max field
                // BEGIN TT#933-MD - AGallagher - Item Max vs. FWOS Max
                if (imop.IMOMaxValue != int.MaxValue)
                {
                    IMOFWOSOriginalValue = imop.IMOMaxValue;  // TT#3804 - AGallagher - Item/FWOS Max option in Global Settings
                    if (SAB.ClientServerSession.GlobalOptions.VSWItemFWOSMax == eVSWItemFWOSMax.Highest)
                    {
                        if (IMOOriginalValue != int.MaxValue)   // TT#3804 - AGallagher - Item/FWOS Max option in Global Settings 
                        {                                       // TT#3804 - AGallagher - Item/FWOS Max option in Global Settings 
                            if (IMOOriginalValue > imop.IMOMaxValue)
                            {
                                imop.IMOMaxValue = IMOOriginalValue;
                            }
                        }
                    }                                           // TT#3804 - AGallagher - Item/FWOS Max option in Global Settings 
                    if (SAB.ClientServerSession.GlobalOptions.VSWItemFWOSMax == eVSWItemFWOSMax.Lowest)
                    {
                        if (IMOOriginalValue < imop.IMOMaxValue)
                        {
                            imop.IMOMaxValue = IMOOriginalValue;
                        }
                    }
                }
                else
                    // When Item FWOS calculates to max int (cannot calculte a FWOS value) them use IMO value.
                {
                    if (SAB.ClientServerSession.GlobalOptions.VSWItemFWOSMax == eVSWItemFWOSMax.Highest)
                    {
                        imop.IMOMaxValue = IMOOriginalValue;
                    }
                }
                // BEGIN TT#3804 - AGallagher - Item/FWOS Max option in Global Settings 
                if (_audit.LoggingLevel == eMIDMessageLevel.Debug)
                {
                    string message = "Store RID = " + imop.IMOStoreRID + " Item Max value = " + IMOOriginalValue + " FWOS computed value = " + IMOFWOSOriginalValue + " Item/FWOS Max Ind = " + SAB.ClientServerSession.GlobalOptions.VSWItemFWOSMax + " Selected value = " + imop.IMOMaxValue;
                    this._audit.Add_Msg(eMIDMessageLevel.Debug,
                                        message,
                                        this.GetType().Name);
                }
                // BEGIN TT#3804 - AGallagher - Item/FWOS Max option in Global Settings 
                // END TT#933-MD - AGallagher - Item Max vs. FWOS Max
			}
			//BEGIN TT#583-MD - stodd -  Add IMO Audit info
			if (_audit.LoggingLevel == eMIDMessageLevel.Debug)
			{
				string message = "Item Max Calculation Completed.";
				this._audit.Add_Msg(eMIDMessageLevel.Debug,
									message,
									this.GetType().Name);
			}
			//END TT#583-MD - stodd -  Add IMO Audit info
			return ipl;
		}

		/// <summary>
		/// Gets the IMO Max value as calulated from the IMOFWOS_Max.
		/// </summary>
		/// <param name="imop"></param>
		/// <param name="hnRid"></param>
		/// <param name="storeShipDay"></param>
		/// <returns></returns>
		private int GetIMOMaxValue(
            IMOProfile imop, 
            int hnRid,
            int eligibilityHnRID, 
            StoreShipDay storeShipDay, 
            StoreProfile aStore)
		{
            // Begin TT#2837 - JSmith - Invalid Calendar Data
            if (storeShipDay.ShipDay == Include.UndefinedDate)
            {
                return int.MaxValue;
            }
            // End TT#2837 - JSmith - Invalid Calendar Data
			int imoMaxValue = 0;
            int weekKey = 0;
            double imoFWOSMaxValue = 0;
			if (_storePlanCube == null)
			{
				_storePlanCube = this.Transaction.GetAllocationPlanCube
					(hnRid, eligibilityHnRID, Include.UndefinedDate, storeShipDay.ShipDay);
			}

			DayProfile startDay = SAB.ApplicationServerSession.Calendar.GetDay(storeShipDay.ShipDay);
            //BEGIN TT#108 - MD - DOConnell - FWOS Max Model
            if (imop.IMOFWOS_MaxType == eModifierType.Model)
            {
                // Begin TT#1386-MD - stodd - manual merge
                //ProfileList storeList = _SAB.ApplicationServerSession.GetProfileList(eProfileType.Store);
                ProfileList storeList = StoreMgmt.StoreProfiles_GetActiveStoresList();
                // End TT#1386-MD - stodd - manual merge
                FWOSMaxModelProfile smmp = SAB.HierarchyServerSession.GetFWOSMaxModelData(imop.IMOFWOS_MaxModelRID);

                if (smmp.ContainsReoccurringDates)
                {
                    weekKey = startDay.Week.WeekInYear;
                }
                else
                {
                    weekKey = startDay.Week.Key;
                }

                //BEGIN TT#3828 - DOConnell - FWOS Max Model should allow dates dynamic to Store Open dates.
                if (smmp.ContainsStoreDynamicDates)
                {
                    Hashtable ModelDateEntries = (Hashtable)smmp.ModelDateEntries.Clone();
                    Hashtable StoreDateEntries = (Hashtable)ModelDateEntries[aStore.Key];
                    if (StoreDateEntries.ContainsKey(weekKey))
                    {
                        imoFWOSMaxValue = Convert.ToDouble(StoreDateEntries[weekKey]);
                    }
                    else
                    {
                        imoFWOSMaxValue = Convert.ToDouble(smmp.FWOSMaxModelDefault);
                    }
                }
                else
                {
                    if (smmp.ModelDateEntries.ContainsKey(weekKey))
                    {
                        imoFWOSMaxValue = Convert.ToDouble(smmp.ModelDateEntries[weekKey]);
                    }
                    else
                    {
                        imoFWOSMaxValue = Convert.ToDouble(smmp.FWOSMaxModelDefault);
                    }
                }
                //if (smmp.ModelDateEntries.ContainsKey(weekKey))
                //{
                //    imoFWOSMaxValue = Convert.ToDouble(smmp.ModelDateEntries[weekKey]);
                //}
                //else
                //{
                //    imoFWOSMaxValue = Convert.ToDouble(smmp.FWOSMaxModelDefault);
                //}
                //END TT#3828 - DOConnell - FWOS Max Model should allow dates dynamic to Store Open dates.
            }
            else
            {
                imoFWOSMaxValue = imop.IMOFWOS_Max;
            }
            //END TT#108 - MD - DOConnell - FWOS Max Model
			// BEGIN TT#2225 - stodd - VSW ANF Enhancement (partial days fix)
            double FWOSDays = imoFWOSMaxValue * 7;
			int fullDays = (int)FWOSDays;
			double partialDay = FWOSDays - fullDays;

			DateTime endDate = storeShipDay.ShipDay.AddDays(fullDays);
			DayProfile endDay = SAB.ApplicationServerSession.Calendar.GetDay(endDate);

			imoMaxValue = _transaction.GetStoreOTSSalesPlan(
                imop.IMOStoreRID, 
                hnRid, 
                eligibilityHnRID, 
                startDay, 
                endDay, 
                100);
			int fullDayValue = imoMaxValue;	// TT#583-MD - stodd -  Add IMO Audit info

			double partialDayValue = 0;
			if (partialDay > 0)
			{
				// BEGIN TT#2401 - stodd - wrong date sent to ModifyIMOMaxValue
				DayProfile partialStartDay = endDay;
				DayProfile partialEndDay = SAB.ApplicationServerSession.Calendar.Add(endDay, 1);
				partialDayValue = _transaction.GetStoreOTSSalesPlan(
                    imop.IMOStoreRID, 
                    hnRid, 
                    eligibilityHnRID, 
                    partialStartDay, 
                    partialEndDay, 
                    100);
				// END TT#2401 - stodd - wrong date sent to ModifyIMOMaxValue
				partialDayValue = partialDayValue * partialDay;
			}

			imoMaxValue = imoMaxValue + (int)(partialDayValue + 0.5d);
			// END TT#2225 - 
			
			// BEGIN TT#2225 - stodd - VSW ANF Enhancement (IMO)
			if (imoMaxValue < 0)
				imoMaxValue = 0;
			// END TT#2225 - stodd - VSW ANF Enhancement (IMO)

			int tempIMOvalue = imoMaxValue;		// TT#583-MD - stodd -  Add IMO Audit info
			double modifiedIMOMaxValue = ModifyIMOMaxValue(aStore, hnRid, startDay, imoMaxValue);

			imoMaxValue = (int)modifiedIMOMaxValue;
            //Begin TT#324 - MD - DOConnell - Using a "blank" FWOS Max Model causes entire alloc to go to VSW
            if (imoMaxValue == 0)
            {
                //treat the max value as if the field is blank in Node Properties
                imoMaxValue = int.MaxValue;
            }
            //End TT#324 - MD - DOConnell - Using a "blank" FWOS Max Model causes entire alloc to go to VSW
			//BEGIN TT#583-MD - stodd -  Add IMO Audit info
			if (_audit.LoggingLevel == eMIDMessageLevel.Debug)
			{
				int IMOCustomValue = imoMaxValue - tempIMOvalue;
				DayProfile displayEndDay = endDay;
				if (partialDay == 0)
				{
					displayEndDay = SAB.ApplicationServerSession.Calendar.Add(endDay, -1);
				}
				string message = "Item Max for Store: " + aStore.StoreId + " Ship Day: " + storeShipDay.ShipDay.ToString("MM/dd/yyyy") + " Ship Day of Week: " + startDay.DayInWeek + " FWOS Max: " + imoFWOSMaxValue +
					" No of full Days: " + fullDays + " Partial Day %: " + partialDay + " End Day: " + displayEndDay.Date.ToString("MM/dd/yyyy") +
					" First " + startDay.Week.Text() + " End " + displayEndDay.Week.Text() +
					" Full Day Amount: " + fullDayValue + " Partial Day Amount: " + (int)(partialDayValue + 0.5d) + " Custom Item Max Adj: " + IMOCustomValue + " Item Max Value: " + imoMaxValue;
				this._audit.Add_Msg(eMIDMessageLevel.Debug,
									message,
									this.GetType().Name);
			}
			//END TT#583-MD - stodd -  Add IMO Audit info

			return imoMaxValue;
		}

		/// <summary>
		/// Allows for the imoMaxValue to be overriden with custom calculations. In base, no modifications take place;
		/// </summary>
		/// <param name="imoMaxValue"></param>
		/// <returns></returns>
		virtual public double ModifyIMOMaxValue(StoreProfile aStore, int hnRid, DayProfile startDay, int imoMaxValue)
		{
			return imoMaxValue;
		}
		#endregion 
		// END TT#2225 - stodd - VSW ANF Enhancement (IMO)

        // Begin TT#1595-MD - stodd - Batch Comp
        #region BatchComp

        virtual public eReturnCode ProcessBatchComp(string aComputation)
        {
            eReturnCode rc = eReturnCode.editErrors;

            if (string.IsNullOrEmpty(aComputation) || aComputation.ToUpper().Trim() == "ALL")
            {
                rc = ProcessAllBatchComps();
            }
            else
            {
                rc = ProcessThisBatchComp(aComputation);
            }

            return rc;
        }

        virtual public eReturnCode ProcessAllBatchComps()
        {
            eReturnCode rc = eReturnCode.severe;
            string errorMessage = MIDText.GetTextOnly(eMIDTextCode.msg_NoBatchCompsDefined);
            SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Error, errorMessage, GetType().Name);
            return rc;
        }

        virtual public eReturnCode ProcessThisBatchComp(string aComputation)
        {
            eReturnCode rc = eReturnCode.severe;
            string errorMessage = MIDText.GetTextOnly(eMIDTextCode.msg_bc_BatchCalcNotFound);
            errorMessage = errorMessage.Replace("{0}", aComputation);
            SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Error, errorMessage, GetType().Name);
            return rc;
        }

        virtual public eReturnCode ProcessThisBatchComp(eBatchComp aComputation)
        {
            eReturnCode rc = eReturnCode.severe;
            string errorMessage = MIDText.GetTextOnly(eMIDTextCode.msg_bc_BatchCalcNotFound);
            errorMessage = errorMessage.Replace("{0}", "");
            SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Error, errorMessage, GetType().Name);
            return rc;
        }


        #endregion
        // End TT#1595-MD - stodd - Batch Comp
	}
}
