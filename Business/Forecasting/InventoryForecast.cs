using System;
using System.Globalization;
using System.Collections;
using System.Diagnostics;
using System.Data;
using System.Windows.Forms;


using MIDRetail.Common;
using MIDRetail.DataCommon;
using MIDRetail.Data;

namespace MIDRetail.Business
{
	/// <summary>
	/// Summary description for InventoryForecast.
	/// </summary>
	public class InventoryForecast
	{

		private string _sourceModule;
		private StoreWeekEligibilityList _storeEligibilty;
		private StoreWeekModifierList _storeStockModifiers;
		// BEGIN MID Track #4370 - John Smith - FWOS Models
		private StoreWeekModifierList _storeFWOSModifiers;
		// END MID Track #4370
		// BEGIN Issue 4827 stodd 10.12.2007		
		private ArrayList _storePresentationMinValues;
		private CustomBusinessRoutines _businessRoutines;		
		// END Issue 4827
		private ArrayList _storeSalesValues;
		private ArrayList _storeStockValues;
		private int _chainInventoryValue;
		private int _chainSalesValue;
		private ProportionalSpread _spread;
		private Hashtable _storeCellRefHash = new Hashtable();
		private OTSPlanMethod _OTSPlanMethod;
		private SessionAddressBlock _SAB;
		private ForecastMonitor _forecastMonitor;
		private ForecastMonitorStoreData _fmStoreData;
		private int _reserveStoreRid;
		private bool _ignoreWOSWarning = false;
		private ForecastVersion _fv = new ForecastVersion();

		// Begin MID Track #5210 - JSmith - Out of memory
		private bool disposed = false;

		#region IDisposable Members

		public void Dispose()
		{
			Dispose(true);
			System.GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			try
			{
				if (disposing &&
					!disposed)
				{
					_storeEligibilty = null;
					_storeStockModifiers = null;
					_storeFWOSModifiers = null;
					_storePresentationMinValues = null;
					_businessRoutines = null;		
					_storeSalesValues = null;
					_storeStockValues = null;
					_spread = null;
					_storeCellRefHash = null;
					_OTSPlanMethod = null;
					_forecastMonitor = null;
					_fmStoreData = null;
					_fv = null;
					disposed = true;
				}
			}
			catch (Exception)
			{
			}
		}
		// End MID Track #5210
		#endregion

		public InventoryForecast(SessionAddressBlock SAB, OTSPlanMethod otsPlanMethod)
		{
			_SAB = SAB;
			_OTSPlanMethod = otsPlanMethod;
			_sourceModule = "InventoryForecast.cs";
			_forecastMonitor = otsPlanMethod.ForecastMonitor;
		}
		
		// BEGIN MID Track #4370 - John Smith - FWOS Models
		public void Process(ModelVariableProfile aVariable, ArrayList aWeeksToPlan)
		// END MID Track #4370
		{

			double targetWeeksOfSupply;
			double weeksUsed;
			double avgWeeklySales = 0;
            // BEGIN MID Track #6043 - KJohnson - Forcast Chain WOS Calculations
            int maximumChainWOS = 0;
            // END MID Track #6043 - KJohnson

			_spread = new ProportionalSpread(_SAB);

			if (_OTSPlanMethod.MONITOR)
			{
				_forecastMonitor.WriteLine(" ");
				_forecastMonitor.WriteLine("###############################################");
				_forecastMonitor.WriteLine("INVENTORY PROCESSING");
				_forecastMonitor.WriteLine("###############################################");
				_forecastMonitor.MonitorType = eForecastMonitorType.Inventory;
				_forecastMonitor.ClearSetDataOnly();
				_forecastMonitor.WriteLine("Inventory Variable: " + _OTSPlanMethod.CurrentVariable.VariableProfile.VariableName);
				// Begin Track #5773 stodd
				_forecastMonitor.WriteLine("Assoc. Sales Variable: " + _OTSPlanMethod.SalesVariable.VariableProfile.VariableName);
				if (_OTSPlanMethod.CurrentVariable.ForcastModelRID != Include.NoRID)
				{
					_forecastMonitor.WriteLine("Forecast Formula: " + ((eForecastFormulaType)_OTSPlanMethod.CurrentVariable.ForecastFormula));
					_forecastMonitor.WriteLine("Variable Selections from Forecast Model");
					string modelTxt = "Grade WOS Index(" + _OTSPlanMethod.CurrentVariable.GradeWOSIDX.ToString() + ") " +
						"Stock Mod(" + _OTSPlanMethod.CurrentVariable.StockModifier.ToString() + ") " +
						"FWOS Override(" + _OTSPlanMethod.CurrentVariable.FWOSOverride.ToString() + ") " +
						"Stock Min(" + _OTSPlanMethod.CurrentVariable.StockMin.ToString() + ") " +
						"Stock Max(" + _OTSPlanMethod.CurrentVariable.StockMax.ToString() + ") " +
						"MIN+Sales(" + _OTSPlanMethod.CurrentVariable.MinPlusSales.ToString() + ") " 
						;
					_forecastMonitor.WriteLine("  " + modelTxt);
				}
				// End Track #5773 stodd
			}

			int weekInPlan = 0;

			// Begin Issue 3752 - stodd 
			_reserveStoreRid =_SAB.ApplicationServerSession.GlobalOptions.ReserveStoreRID;
			// End Issue 3752 - stodd 			

			StoreGradeList storeGradeList = _OTSPlanMethod.ApplicationTransaction.GetStoreGradeList(_OTSPlanMethod.Plan_HN_RID);
			StoreEligibilityList storeEligList = _SAB.HierarchyServerSession.GetStoreEligibilityList(_OTSPlanMethod.AllStoreList, _OTSPlanMethod.Plan_HN_RID, true, false);
			bool versionProtected = _fv.GetIsVersionProtected(_OTSPlanMethod.Plan_FV_RID);
			WeekProfile currentWeek = _SAB.ApplicationServerSession.Calendar.CurrentWeek;
			// Begin Issue 4827 - stodd 10.23.2007
			_businessRoutines = new CustomBusinessRoutines(_SAB, _OTSPlanMethod.ApplicationTransaction, _OTSPlanMethod.AllStoreList,_OTSPlanMethod.Plan_HN_RID);
			// End Issue 4827 - stodd
			// BEGIN Issue 4827 stodd 10.12.2007 Presentation + Sales
			// This resolves the IsPresentationMinDefined() question
			_businessRoutines.IsPresentationMinDefined();
			if (_OTSPlanMethod.MONITOR)
			{
				_forecastMonitor.WriteLine("isPresentation+Sales Defined: " + 
					_businessRoutines.IsPresentationMinDefined().ToString());
			}
			// END Issue 4827

			//*****************************
			// Inventory Week LOOP
			//*****************************
			// BEGIN MID Track #4370 - John Smith - FWOS Models
//			foreach(WeekProfile inventoryWeek in _OTSPlanMethod.WeeksToPlan.ArrayList)
			foreach(WeekProfile inventoryWeek in aWeeksToPlan)
			// END MID Track #4370
			{
				_OTSPlanMethod.WeekBeingPlanned = inventoryWeek; // Track #6187

				// Begin Issue 4047 - stodd
				if (inventoryWeek == currentWeek && versionProtected)  // compares keys
				{
					string errorMsg = MIDText.GetText(eMIDTextCode.msg_VersionProtectedNoInventoryPlanned);
					errorMsg = errorMsg.Replace("{0}", inventoryWeek.YearWeek.ToString(CultureInfo.CurrentUICulture));
					// Begin Track #6280 stodd
					errorMsg = errorMsg.Replace("{1}", _OTSPlanMethod.CurrentVariable.VariableProfile.VariableName);
					errorMsg = errorMsg.Replace("{2}", "planned");
					// End Track #6280
					_SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Warning, errorMsg, _sourceModule);
					if (_OTSPlanMethod.MONITOR)
					{
						_forecastMonitor.WriteLine("WARNING - " + errorMsg);
					}
					continue;
				}
				// End Issue 4047

				int eligibleCount = 0;
				_storeCellRefHash.Clear();

				_chainInventoryValue = _OTSPlanMethod.ReadChainValue(inventoryWeek, _OTSPlanMethod.StockVariable);
				_storeEligibilty = _OTSPlanMethod.ApplicationTransaction.GetStoreEligibilityForStock(
                    eRequestingApplication.Forecast, 
                    _OTSPlanMethod.Plan_HN_RID,
                    inventoryWeek.YearWeek
                    );
				_storeStockModifiers = _OTSPlanMethod.ApplicationTransaction.GetStoreModifierForStock(_OTSPlanMethod.Plan_HN_RID, inventoryWeek.YearWeek);
				// BEGIN MID Track #4370 - John Smith - FWOS Models
				_storeFWOSModifiers = _OTSPlanMethod.ApplicationTransaction.GetStoreModifierForFWOS(_OTSPlanMethod.Plan_HN_RID, inventoryWeek.YearWeek);
				// END MID Track #4370

				// BEGIN Issue 4827 stodd 10.12.2007 Presentation + Sales
				if (_businessRoutines.IsPresentationMinDefined()) 
				{
					VariableProfile presentationMinVariable = _businessRoutines.GetPresentationMinVariable();
					if (presentationMinVariable == null)
					{
						string errorMsg = MIDText.GetText(eMIDTextCode.msg_NoVarCustomTypeOfPresentationMin);
						_SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Error, errorMsg, _sourceModule);
						if (_OTSPlanMethod.MONITOR)
						{
							_forecastMonitor.WriteLine("ERROR - " + errorMsg);
						}
						throw new EndProcessingException(errorMsg);
					}
					else
					{
						_storePresentationMinValues = _OTSPlanMethod.ReadStoreValues(inventoryWeek, presentationMinVariable);
					}
				}
				// END Issue 4827

				//==================================================================================
				// Need to determine what the highest FWOS override is.
				// This is then used to adjust the total number of weeks of sales we get, IF
				// the max we find is higher than the chain WOS.
				//==================================================================================
				double MaxFWOSMod = 0.0d;
				foreach (StoreWeekModifierProfile storeMod in _storeFWOSModifiers.ArrayList)
				{

					if (storeMod.StoreModifier > MaxFWOSMod)
						MaxFWOSMod = storeMod.StoreModifier;
				}


				if (_OTSPlanMethod.MONITOR)
				{
					_forecastMonitor.WriteLine("");
					_forecastMonitor.WriteLine("INVENTORY WEEK: " + inventoryWeek.YearWeek.ToString() +
						" Amount: " + _chainInventoryValue.ToString());
					_forecastMonitor.WriteLine("");
				}
				//*************************************
				// Calc Target Weeks Of Supply
				//*************************************
				// BEGIN TT#619 - STodd - OTS Forecast - Chain Plan not required (#46)
				if (_OTSPlanMethod.ApplyTrendOptionsInd == 'C')
				{
				// END TT#619 - STodd - OTS Forecast - Chain Plan not required (#46)
					if (_chainInventoryValue == 0)
					{
						string errorMsg = MIDText.GetText(eMIDTextCode.msg_ZeroChainTotalNoWos);
						errorMsg = errorMsg.Replace("{0}", inventoryWeek.YearWeek.ToString(CultureInfo.CurrentUICulture));
						_SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, errorMsg, _sourceModule);
						if (_OTSPlanMethod.MONITOR)
						{
							_forecastMonitor.WriteLine("INFORMATIONAL - " + errorMsg);
						}
						//==================================================================
						// BEGIN Issue 5211 
						// If the chain is zero, then we zero out all of the store values
						//==================================================================
						_storeStockValues = _OTSPlanMethod.ReadStoreValues(inventoryWeek, _OTSPlanMethod.StockVariable);

						for (int str = 0; str < _OTSPlanMethod.AllStoreList.Count; str++)
						{
							PlanCellReference storeStockRef;
							StoreProfile storeProfile = (StoreProfile)_OTSPlanMethod.AllStoreList[str];
							bool eligible = true;
							bool locked = false;
							try
							{
								eligible = ((StoreWeekEligibilityProfile)_storeEligibilty.FindKey(storeProfile.Key)).StoreIsEligible;
								if (storeProfile.Key == _reserveStoreRid)
									eligible = false;

								storeStockRef = (PlanCellReference)_storeStockValues[str];

								if (storeStockRef.isCellLocked)
								{
									locked = true;
								}

								if (eligible && !locked)
								{
									storeStockRef.SetEntryCellValue(0d);
								}
							}
							catch (CellNotAvailableException)
							{
								string msg = MIDText.GetText(eMIDTextCode.msg_InventoryCellIsProtected);
								msg = msg.Replace("{0}", StoreMgmt.GetStoreDisplayText(storeProfile.Key)); //_SAB.StoreServerSession.GetStoreDisplayText(storeProfile.Key));
								msg = msg.Replace("{1}", inventoryWeek.YearWeek.ToString());
								msg = msg.Replace("{2}", _fv.GetVersionText(_OTSPlanMethod.Plan_FV_RID));
								_SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Warning, msg, this.ToString());
								if (_OTSPlanMethod.MONITOR)
								{
									_forecastMonitor.WriteLine(msg);
								}
							}
							catch (Exception)
							{
								throw;
							}
						}

						// END Issue 5211
						continue;
					}
				}

				//***************************************
				// build Stock Min Max information
				//***************************************
                // Begin TT#3420 - JSmith - Store Forecast Stock Min-Max at Low Level not being observed.
                //_OTSPlanMethod.StockMinMax.Build(inventoryWeek, _OTSPlanMethod.Plan_HN_RID, _OTSPlanMethod.GLFProfileList.ArrayList);
                _OTSPlanMethod.StockMinMax.Build(inventoryWeek, _OTSPlanMethod.Plan_HN_RID, _OTSPlanMethod.GLFProfileList.ArrayList, _OTSPlanMethod.Orig_Plan_HN_RID);
                // End TT#3420 - JSmith - Store Forecast Stock Min-Max at Low Level not being observed.
				//_OTSPlanMethod.StockMinMax.DebugThis();

				targetWeeksOfSupply = GetWeeksOfSupply(inventoryWeek);

                // BEGIN MID Track #6043 - KJohnson - Forcast Chain WOS Calculations
                maximumChainWOS = _SAB.ApplicationServerSession.GlobalOptions.MaximumChainWOS;
                // END MID Track #6043 - KJohnson

				// Begin Issue 3922 - stodd
                if (targetWeeksOfSupply > maximumChainWOS)  // BEGIN MID Track #6043 - KJohnson - Forcast Chain WOS Calculations
				{
					string errorMsg = string.Empty;
                    // BEGIN MID Track #6043 - KJohnson - Forcast Chain WOS Calculations
                    //if (!_ignoreWOSWarning)
                    //{
                    //    errorMsg = MIDText.GetText(eMIDTextCode.msg_WosExceededBoundaryQuestion);
                    //    errorMsg = errorMsg.Replace("{1}", targetWeeksOfSupply.ToString(CultureInfo.CurrentUICulture));
                    //    errorMsg = errorMsg.Replace("{0}", inventoryWeek.YearWeek.ToString(CultureInfo.CurrentUICulture));

                    //    DialogResult result = _SAB.MessageCallback.HandleMessage(errorMsg, "Weeks of Supply Exceeded Boundary",
                    //        System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Question);

                    //    if (result == DialogResult.Yes || result == DialogResult.OK)
                    //    {
                    //        _ignoreWOSWarning = true;
                    //    }
                    //    else
                    //    {
                    //        //================================
                    //        // User cancelled the forecast
                    //        //================================
                    //        errorMsg = MIDText.GetText(eMIDTextCode.msg_UserCancelledWosExceeded);
                    //        errorMsg = errorMsg.Replace("{1}", targetWeeksOfSupply.ToString(CultureInfo.CurrentUICulture));
                    //        errorMsg = errorMsg.Replace("{0}", inventoryWeek.YearWeek.ToString(CultureInfo.CurrentUICulture));
                    //        _SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Warning, errorMsg, _sourceModule);
                    //        throw new UserCancelledException();
                    //    }
                    //}

					errorMsg = MIDText.GetText(eMIDTextCode.msg_WosExceededBoundary);
					errorMsg = errorMsg.Replace("{0}", targetWeeksOfSupply.ToString(CultureInfo.CurrentUICulture));
					errorMsg = errorMsg.Replace("{1}", inventoryWeek.YearWeek.ToString(CultureInfo.CurrentUICulture));
                    errorMsg = errorMsg.Replace("{2}", maximumChainWOS.ToString(CultureInfo.CurrentUICulture));
					_SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Warning, errorMsg, _sourceModule);

                    // if WOS is greater than _maximumChainWOS weeks, set it to _maximumChainWOS weeks.
                    targetWeeksOfSupply = maximumChainWOS;
                    // END MID Track #6043 - KJohnson

					if (_OTSPlanMethod.MONITOR)
					{
						_forecastMonitor.WriteLine("WARNING - " + errorMsg);
						_forecastMonitor.WriteLine("Week: " + inventoryWeek.YearWeek.ToString() +
							"  Adjusted WOS: " + targetWeeksOfSupply.ToString("00.0000") );
					}
				}
				// End issue 3922

				ArrayList storeSummandList = new ArrayList();
				ArrayList storeIneligibleSummandList = new ArrayList();

				//*********************************************************************
				// Stock/Inventory are all values for the current week for ALL Stores
				//*********************************************************************
				_storeStockValues = _OTSPlanMethod.ReadStoreValues(inventoryWeek, _OTSPlanMethod.StockVariable);

				//*****************************************
				// Calculate weeks for sales
				//*****************************************
				int salesWeeks = 0;
				if (MaxFWOSMod > targetWeeksOfSupply)
				{
					salesWeeks = (int)MaxFWOSMod;
				}
				else
				{
					salesWeeks = (int)targetWeeksOfSupply;
				}

				double remainder = targetWeeksOfSupply - salesWeeks;
				if (remainder > 0)
					salesWeeks++;
				//get weeks into an array list
				ArrayList salesWeekList = new ArrayList(); 
				WeekProfile currWeek = null;
				for (int w=0;w<salesWeeks;w++)
				{

					if (w==0) //first week should equal inventory week
					{
						salesWeekList.Add(inventoryWeek);
						currWeek = inventoryWeek;
					}
					else
					{
						currWeek = _SAB.ApplicationServerSession.Calendar.Add(currWeek, 1);
						salesWeekList.Add(currWeek);
					}
				}

				double maxWeekHold = 0;
				bool salesPlanDepleted = false;
				double totalSales = 0.0;
		
				//********************************************************
				// STORE LOOP
				//********************************************************
				for (int str=0;str<_OTSPlanMethod.AllStoreList.Count;str++)
				{

					Summand storeSummand = new Summand();

					PlanCellReference	storeSalesRef;
					PlanCellReference	storeStockRef;

					StoreProfile storeProfile = (StoreProfile)_OTSPlanMethod.AllStoreList[str];

					//int storeRid = storeProfile.Key;

					if (_OTSPlanMethod.MONITOR)
					{
						_fmStoreData = _forecastMonitor.CreateStoreData(storeProfile.Key);
						_fmStoreData.StoreName = storeProfile.StoreId;
					}

					storeSummand.Item = storeProfile.Key;
					storeSummand.Eligible = ((StoreWeekEligibilityProfile)_storeEligibilty.FindKey( storeProfile.Key )).StoreIsEligible;
					// Begin Issue 3752 - stodd 
					// The RESERVE STORE is ALWAYS ineligible
					if (storeSummand.Item == _reserveStoreRid)
						storeSummand.Eligible = false;
					// End Issue 3752 - stodd 

					if (storeSummand.Eligible)
						eligibleCount++;
					// BEGIN MID Track #4370 - John Smith - FWOS Models
					StoreWeekModifierProfile storeMod = (StoreWeekModifierProfile)_storeFWOSModifiers.FindKey(storeProfile.Key);
					if (storeMod != null &&
						storeMod.StoreModifier >= 0)
					{
						// BEGIN MID Track #5773 - KJohnson - Planned FWOS Enhancement
						if (_OTSPlanMethod.CurrentVariable.FWOSOverride ||
							_OTSPlanMethod.CurrentVariable.ForcastModelRID == Include.NoRID) 
						{
							storeSummand.WeeksOfSupply = storeMod.StoreModifier;
							storeSummand.WeeksOfSupplyWasOverridden = true;
						} 
						else 
						{
							storeSummand.WeeksOfSupply = targetWeeksOfSupply;
						}
						// END MID Track #5773
					}
					else
					{
						storeSummand.WeeksOfSupply = targetWeeksOfSupply;
					}
					if (_OTSPlanMethod.MONITOR)
					{
						_fmStoreData.WeeksOfSupply = storeSummand.WeeksOfSupply;
						_fmStoreData.WeeksOfSupplyWasOverridden = storeSummand.WeeksOfSupplyWasOverridden;
					}
					// END MID Track #4370

					//************************************************
					// Dump INELIGIBLE stores off to another list. 
					// do not process them.
					//************************************************
					// BEGIN TT#1570 - stodd - ineligibility not showing correctly on monitor log
					if (_OTSPlanMethod.MONITOR)
					{
						_fmStoreData.IsEligible = storeSummand.Eligible;
					}
					// End TT#1570 - stodd - ineligibility not showing correctly on monitor log
					if (!storeSummand.Eligible)
					{
						storeSummand.Inventory = 0d;
						storeIneligibleSummandList.Add(storeSummand);
						continue;
					}
					//*********************************************************************
					// Sales values are ALL weeks for current store
					//*********************************************************************
					_storeSalesValues = _OTSPlanMethod.GetStoreWeeklyValues(salesWeekList, storeProfile.Key, _OTSPlanMethod.SalesVariable);
					

					// STOCK
					storeStockRef = (PlanCellReference)_storeStockValues[str];
					_storeCellRefHash.Add(storeProfile.Key, storeStockRef);


					if (storeStockRef.isCellLocked)
					{
						storeSummand.Locked = true;
						storeSummand.Inventory = storeStockRef.CurrentCellValue;
					}

					if (_OTSPlanMethod.MONITOR)
					{
						_fmStoreData.IsLocked = storeSummand.Locked;
					}

					//**********************
					// accum sales
					//**********************
					double totalAccumSales = 0;
					weeksUsed = 0;
					int partOfWeek = 1; 
					storeSummand.SalesPlanDepleted = false;
					int wk=0;
					double remainingWOS = storeSummand.WeeksOfSupply;
					foreach (WeekProfile salesWeek in salesWeekList)
					{
						// count weeks, but only accept those sales weeks >= inventory week
						if (inventoryWeek > salesWeek)
							continue;
		
						storeSalesRef = (PlanCellReference)_storeSalesValues[wk];

						// BEGIN Issue 5284 stodd 
						//if (storeSalesRef.CurrentCellValue == 0)
						//	break;	// no sales
						if (remainingWOS < 1)
						// END Issue 5284
						{
							totalAccumSales = totalAccumSales + (storeSalesRef.CurrentCellValue * remainingWOS) / partOfWeek;
							weeksUsed = weeksUsed + (remainingWOS / partOfWeek);
							//storeSummand.SalesPlanDepleted = true;
							salesPlanDepleted = true;
							break;
						}
						else
						{
							weeksUsed += 1;
							totalAccumSales += storeSalesRef.CurrentCellValue;
							remainingWOS = remainingWOS-1;
						}

						wk++;
					}

					// BEGIN Issue 6290 stodd 2.5.2009
					//totalAccumSales = Round(totalAccumSales);
					// END Issue 6290
					totalSales += totalAccumSales;


					if (weeksUsed > maxWeekHold)
						maxWeekHold = weeksUsed;

					storeSummand.TotalSales = totalAccumSales;
					storeSummand.Quantity = totalAccumSales;
					storeSummand.WeeksUsed = weeksUsed;


					if (_OTSPlanMethod.MONITOR)
					{
						_fmStoreData.TotalSales = totalAccumSales;
					}


					storeSummandList.Add(storeSummand);

				} // end store loop

				double weeksToUse = 0;
				if (salesPlanDepleted)
					weeksToUse = targetWeeksOfSupply;
				else
					weeksToUse = maxWeekHold;


				//****************************************************
				// get store grades based upon accumulated sales plan
				//****************************************************
				GradeStoreBin [] gradeStoreBinList = new GradeStoreBin[ _OTSPlanMethod.AllStoreList.ArrayList.Count ];

				for (int s=0;s<storeSummandList.Count;s++)
				{					
					Summand ss = (Summand)storeSummandList[s];
						

					if (ss.Eligible == false || weeksToUse == 0)
						avgWeeklySales = 0;
					else if (ss.Eligible)
					{
						if (ss.WeeksOfSupplyWasOverridden)
						{
							avgWeeklySales = (ss.TotalSales / ss.WeeksOfSupply);
						}
						else
						{
							avgWeeklySales = (ss.TotalSales / weeksToUse);
						}
					}

					// BEGIN Issue 6290 stodd 2.5.2009
					//avgWeeklySales = Round(avgWeeklySales);
					// BEGIN Issue 6290 stodd 2.5.2009
					((Summand)storeSummandList[s]).AvgWeeklySales = avgWeeklySales;

					if (_OTSPlanMethod.MONITOR)
					{
						ForecastMonitorStoreData fmStoreData = _forecastMonitor.GetStoreData(ss.Item);
						fmStoreData.AvgSales = avgWeeklySales;
						fmStoreData.WeeksUsed = weeksToUse;
					}

					// build Grade Store Bin list to use to find store grades...later
					GradeStoreBin gradeStoreBin = new GradeStoreBin();
					gradeStoreBin.StoreKey = ss.Item;
					gradeStoreBin.StoreGradeUnits = ss.TotalSales;
					gradeStoreBin.StoreEligible = ss.Eligible;
					gradeStoreBinList[s] = gradeStoreBin;
				}

				StoreGrade sg = new StoreGrade();
				// Begin Track # 6401 - stodd - no eligible stores getting grades
				int [] storeGrades = null;
				try
				{
					storeGrades = StoreGrade.GetGradeProfileKey(storeGradeList, gradeStoreBinList);
				}
				catch (MIDException ex)
				{
					if (ex.ErrorNumber == (int)eMIDTextCode.msg_GradeCalcFail_NoEligibleStores)
					{
						HierarchyNodeProfile hnp = _SAB.HierarchyServerSession.GetNodeData(_OTSPlanMethod.Plan_HN_RID);
						string errorMsg = this._OTSPlanMethod.Name + " for week " + inventoryWeek.YearWeek.ToString(CultureInfo.CurrentUICulture) + 
							" and node " + hnp.Text + ". ";
						errorMsg +=  MIDText.GetText(eMIDTextCode.msg_GradeCalcFail_NoEligibleStores);
						_SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Warning, errorMsg, _sourceModule);
						if (_OTSPlanMethod.MONITOR)
						{
							_forecastMonitor.WriteLine(MIDText.GetText(eMIDTextCode.msg_GradeCalcFail_NoEligibleStores));
						}
						continue;
					}
					else
					{
						throw;
					}
				}
				// End Track # 6401 - stodd - no eligible stores getting grades
				// update summand (and forecastMonitor) with store grade
				for (int s=0;s<storeSummandList.Count;s++)
				{
					if (_OTSPlanMethod.MONITOR)
					{
						int storeKey = ((Summand)storeSummandList[s]).Item;
						ForecastMonitorStoreData fmStoreData = _forecastMonitor.GetStoreData(storeKey);
						fmStoreData.Grade = storeGrades[s];
					}
					((Summand)storeSummandList[s]).Grade = storeGrades[s];
				}

				// Sort stores in decending sequence by Total Accumed Sales
				storeSummandList.Sort(new SummandDescendingComparer());

				if (_OTSPlanMethod.MONITOR && _OTSPlanMethod.MonitorMode == eForecastMonitorMode.Verbose)
				{
					_forecastMonitor.StoreDataList.Sort(new MonitorDescendingSalesComparer());
					_forecastMonitor.WriteStoreDataLabels();
					_forecastMonitor.WriteLine("Total Sales and Average Sales Calculated (and Grades).");
					_forecastMonitor.WriteAllStoreData();
				}

				//*************************************
				// Calc Weeks Of Supply INDEX 
				//*************************************
				// We find the number of stores in each store grade and then find the median store in each grade
				//***********************************************************************************************
				ProfileList storeGradeIndexList = new ProfileList(eProfileType.StoreGradeInventory);
				for (int summ=0; summ< storeSummandList.Count;summ++)
				{
					Summand currSummand = (Summand)storeSummandList[summ];
					if (storeGradeIndexList.Contains(currSummand.Grade))
					{
						StoreGradeInventoryProfile sgip = (StoreGradeInventoryProfile)storeGradeIndexList.FindKey(currSummand.Grade);
						sgip.TotalCount++;
					}
					else
					{
						StoreGradeInventoryProfile sgip = new StoreGradeInventoryProfile(currSummand.Grade);
						if (storeGradeIndexList.Count == 0)
							sgip.First = true;
						sgip.StartIndex = summ;
						sgip.TotalCount = 1;
						foreach (StoreGradeProfile sgp in storeGradeList.ArrayList)
						{
							if (sgp.Boundary == currSummand.Grade)
							{
								sgip.WOSIndex = sgp.WosIndex;
								if (sgp.Key == storeGradeList[storeGradeList.Count-1].Key)
									sgip.Last = true;
								break;
							}
						}
						
						storeGradeIndexList.Add(sgip);
					}
				}

				//foreach (StoreGradeInventoryProfile sgip in storeGradeIndexList.ArrayList)
				for (int gradeIdx=0;gradeIdx<storeGradeIndexList.Count;gradeIdx++)
				{

					StoreGradeInventoryProfile sgip = (StoreGradeInventoryProfile)storeGradeIndexList[gradeIdx];

					if (sgip.TotalCount > 1)
					{
						int medianOffset = sgip.TotalCount / 2;
						sgip.MedianIndex = sgip.StartIndex + medianOffset;
					}
					else
						sgip.MedianIndex = sgip.StartIndex;
					Summand medianStore = (Summand)storeSummandList[sgip.MedianIndex];
					medianStore.WOSIndex = sgip.WOSIndex;

					if (gradeIdx == (storeGradeIndexList.Count - 1))
						sgip.Last = true;
				}

				//Debug.Flush();

				double highGradeSales = 0;
				double lowGradeSales = 0;
				double highGradeIndex = 0;
				double lowGradeIndex = 0;
				int gradeSub = 0;

				//************************************************************************
				// Assign WOS Indexes to Stores
				//************************************************************************
				for (int summ=0; summ< storeSummandList.Count;summ++)
				{
					Summand currStore = (Summand)storeSummandList[summ];
					StoreGradeInventoryProfile sgip = (StoreGradeInventoryProfile)storeGradeIndexList.FindKey(currStore.Grade);
					//********************************************************************
					// We need to not only know the grade for the current store, 
					// but also positionally where the grade lies.
					// later we may need to know the grade after it or previous to it.
					//********************************************************************
					if (sgip != null)
					{
						for (int i=0;i<storeGradeIndexList.Count;i++)
						{
							if (storeGradeIndexList[i].Key == sgip.Key)
							{
								gradeSub = i;
								break;
							}
						}
					}
		
					Summand medianStore = (Summand)storeSummandList[sgip.MedianIndex];
					//******************
					// Assign WOS Index
					//******************
					// BEGIN MID Track #4416 - John Smith - Do not assign index if Weeks Of Supply is overridden
					if (!currStore.WeeksOfSupplyWasOverridden)
					{
					// END MID Track #4416
						if (sgip.First && currStore.TotalSales >= medianStore.TotalSales)
							currStore.WOSIndex = medianStore.WOSIndex; 
						else if (sgip.Last && currStore.TotalSales <= medianStore.TotalSales)
							currStore.WOSIndex = medianStore.WOSIndex; 
						else
						{
							if (currStore.TotalSales < medianStore.TotalSales)
							{
								highGradeSales = medianStore.TotalSales;
								highGradeIndex = medianStore.WOSIndex;
								// get lower grade...
								gradeSub++;
								StoreGradeInventoryProfile nextGrade = (StoreGradeInventoryProfile) storeGradeIndexList[gradeSub];
								Summand lowStore = (Summand)storeSummandList[nextGrade.MedianIndex];
								lowGradeSales = lowStore.TotalSales;
								lowGradeIndex = lowStore.WOSIndex;

								double m = (highGradeIndex - lowGradeIndex) / (highGradeSales - lowGradeSales);
								double b = (highGradeIndex - (m * highGradeSales));
								double storeWOSIndex = (m * currStore.TotalSales) + b;
								currStore.WOSIndex = storeWOSIndex;
							}
							else if (currStore.TotalSales > medianStore.TotalSales)
							{

								lowGradeSales = medianStore.TotalSales;
								lowGradeIndex = medianStore.WOSIndex;
								// get higher grade...
								gradeSub--;
								StoreGradeInventoryProfile prevGrade = (StoreGradeInventoryProfile) storeGradeIndexList[gradeSub];
								Summand highStore = (Summand)storeSummandList[prevGrade.MedianIndex];
								highGradeSales = highStore.TotalSales;
								highGradeIndex = highStore.WOSIndex;

								double m = (highGradeIndex - lowGradeIndex) / (highGradeSales - lowGradeSales);
								double b = (highGradeIndex - (m * highGradeSales));
								double storeWOSIndex = (m * currStore.TotalSales) + b;
								currStore.WOSIndex = storeWOSIndex;
							}
							else
								currStore.WOSIndex = medianStore.WOSIndex;
						}
					// BEGIN MID Track #4416 - John Smith - Do not assign index if Weeks Of Supply is overridden
					}
					// END MID Track #4416

					if (_OTSPlanMethod.MONITOR)
					{
						ForecastMonitorStoreData fmStoreData = _forecastMonitor.GetStoreData(currStore.Item);
						fmStoreData.WOSIndex = currStore.WOSIndex;
					}
				}  // end of WOS Index loop

				if (_OTSPlanMethod.MONITOR && _OTSPlanMethod.MonitorMode == eForecastMonitorMode.Verbose)
				{
					_forecastMonitor.WriteLine(" ");
					_forecastMonitor.WriteLine("WOS Index Calculated.");
					_forecastMonitor.WriteAllStoreData();
				}

				//*************************************************************************
				// Calc Inventory Plan
				//*************************************************************************

				// Build Stock Min Max for this planWeek.
				// Begin TT#3420 - JSmith - Store Forecast Stock Min-Max at Low Level not being observed.
                //_OTSPlanMethod.StockMinMax.Build(inventoryWeek, _OTSPlanMethod.Plan_HN_RID, _OTSPlanMethod.GLFProfileList.ArrayList);
                _OTSPlanMethod.StockMinMax.Build(inventoryWeek, _OTSPlanMethod.Plan_HN_RID, _OTSPlanMethod.GLFProfileList.ArrayList, _OTSPlanMethod.Orig_Plan_HN_RID);
                // End TT#3420 - JSmith - Store Forecast Stock Min-Max at Low Level not being observed.
				// get the store/group level hash, if we don't already have one
				if	(_OTSPlanMethod.StoreGroupLevelHash == null)
                    _OTSPlanMethod.StoreGroupLevelHash = StoreMgmt.GetStoreGroupLevelHashTable(_OTSPlanMethod.SG_RID); //_SAB.StoreServerSession.GetStoreGroupLevelHashTable(_OTSPlanMethod.SG_RID);

				double lastCalculatedPlan = double.MaxValue;
				double finalPlan = 0;
				double unbalancedTotal = 0;
				for (int summ=0; summ< storeSummandList.Count;summ++)
				{
					Summand currStore = (Summand)storeSummandList[summ];

					if (currStore.Locked)
					{
						unbalancedTotal += currStore.Inventory;

						if (_OTSPlanMethod.MONITOR)
						{	
							int storeKey = currStore.Item;
							ForecastMonitorStoreData fmStoreData = _forecastMonitor.GetStoreData(currStore.Item);
							fmStoreData.Inventory = currStore.Inventory;
							fmStoreData.StockModifier = 0;
							fmStoreData.StockMin = 0;
							fmStoreData.StockMax = 0;
						}
					}
					else
					{
						StoreGradeProfile storeGradeProfile = (StoreGradeProfile)storeGradeList.FindKey(currStore.Grade);

						StoreWeekModifierProfile storeMod = (StoreWeekModifierProfile)_storeStockModifiers.FindKey(currStore.Item);

						// BEGIN MID Track #4416 - John Smith - Do not assign index if Weeks Of Supply is overridden
						double initialPlan = 0;
						if (currStore.WeeksOfSupplyWasOverridden)
						{
							// stodd
							initialPlan = currStore.TotalSales;
						}
						else
						{
							// BEGIN MID Track #5773 - KJohnson - Planned FWOS Enhancement
							if (_OTSPlanMethod.CurrentVariable.GradeWOSIDX ||
								_OTSPlanMethod.CurrentVariable.ForcastModelRID == Include.NoRID) 
							{
								initialPlan = (currStore.WOSIndex * currStore.AvgWeeklySales * currStore.WeeksOfSupply) / 100;
							} 
							else
							{
								initialPlan = (currStore.AvgWeeklySales * currStore.WeeksOfSupply);
							}
							// END MID Track #5773
						}
						// BEGIN MID Track #4416
						// BEGIN MID Track #4370 - John Smith - FWOS Models
						if (!currStore.WeeksOfSupplyWasOverridden)
						{
							if (lastCalculatedPlan < initialPlan)
							{
								initialPlan = lastCalculatedPlan;
							}
							lastCalculatedPlan = initialPlan;
						}
						// END MID Track #4370

						double secondaryPlan = 0;
						if (storeMod == null)  // no stock modifier
							secondaryPlan = initialPlan;
						// BEGIN MID Track #4370 - John Smith - FWOS Models
						else if (!currStore.WeeksOfSupplyWasOverridden)
						{
							// BEGIN MID Track #5773 - KJohnson - Planned FWOS Enhancement
							if (_OTSPlanMethod.CurrentVariable.StockModifier ||
								_OTSPlanMethod.CurrentVariable.ForcastModelRID == Include.NoRID) 
							{
								secondaryPlan = initialPlan * storeMod.StoreModifier;
							} 
							else 
							{
								secondaryPlan = initialPlan;
							}
							// END MID Track #5773
						}
						else
						{
							secondaryPlan = initialPlan;
						}
						// END MID Track #4370

						// Move in the final inventory value for store, then check it against MINs and MAXs
						// Added rounding to finalPlan value.
						// BEGIN Track #5773 stodd 10.24.2008
						finalPlan = Round(secondaryPlan, _OTSPlanMethod.CurrentVariable.VariableProfile.NumDecimals);
						// END Track #5773 stodd 10.24.2008

						//*****************************************************************
						// Get Stock Min and Max and apply them
						//*****************************************************************
						int sglRid = (int)_OTSPlanMethod.StoreGroupLevelHash[currStore.Item];
						// BEGIN MID Track #5773 - KJohnson - Planned FWOS Enhancement
						int minStock = Include.Undefined;
						if (_OTSPlanMethod.CurrentVariable.StockMin ||
							_OTSPlanMethod.CurrentVariable.ForcastModelRID == Include.NoRID)
						{
							minStock = _OTSPlanMethod.StockMinMax.GetMinimumForWeek(inventoryWeek, sglRid, currStore.Grade); 
						}
						int maxStock = Include.Undefined; 
						if (_OTSPlanMethod.CurrentVariable.StockMax ||
							_OTSPlanMethod.CurrentVariable.ForcastModelRID == Include.NoRID)
						{
							maxStock = _OTSPlanMethod.StockMinMax.GetMaximumForWeek(inventoryWeek, sglRid, currStore.Grade); 
						}
						// END MID Track #5773

						// Used in Audit Reporting...
						if (minStock != Include.Undefined || maxStock != Include.Undefined)
							_OTSPlanMethod.SetStockMinMaxTrue(sglRid, currStore.Item); 

						// BEGIN Issue 4827 stodd 10.12.2007 Presentation + Sales
						// If a variable has been defined as Presentation Minimum...
						// And if THIS store gets the special processing...
						if (_businessRoutines.IsPresentationMinDefined())
						{
							StoreProfile sp = (StoreProfile)_OTSPlanMethod.AllStoreList.FindKey(currStore.Item);
							int storeIdx = _OTSPlanMethod.AllStoreList.ArrayList.IndexOf(sp);

							PlanCellReference pcr = (PlanCellReference)_storePresentationMinValues[storeIdx];
							if (_OTSPlanMethod.MONITOR)
							{	
								ForecastMonitorStoreData fmStoreData = _forecastMonitor.GetStoreData(currStore.Item);
								fmStoreData.PresentationMin	= pcr.CurrentCellValue;
							}
							if (_businessRoutines.ApplyPresentationMinPlusSales(currStore.Item))
							{
								// BEGIN MID Track #5773 - KJohnson - Planned FWOS Enhancement
								if (_OTSPlanMethod.CurrentVariable.MinPlusSales ||
									_OTSPlanMethod.CurrentVariable.ForcastModelRID == Include.NoRID)
								{
									// Take the higher of Min Stock or Presentation Min
									double max = Math.Max(pcr.CurrentCellValue, minStock);
									finalPlan = finalPlan + max;
								}
								// END MID Track #5773
								if (_OTSPlanMethod.MONITOR)
								{	
									ForecastMonitorStoreData fmStoreData = _forecastMonitor.GetStoreData(currStore.Item);
									fmStoreData.ApplyPresentationMin = true;
								}
							}
							else
							{
								if (minStock != Include.Undefined)
								{
									if (finalPlan < minStock)
										finalPlan = minStock;
								}
							}
						}
						else
						{
							if (minStock != Include.Undefined)
							{
								if (finalPlan < minStock)
									finalPlan = minStock;
							}
						}
						// End Issue 4827 stodd 10.12.2007 Presentation + Sales

						if (maxStock != Include.Undefined)
							if (finalPlan > maxStock)
							// BEGIN MID Track #4370 - John Smith - FWOS Models
							{
								finalPlan = maxStock;
								currStore.MaxStockWasApplied = true;
							}
							// END MID Track #4370
						
						// BEGIN MID Track #4370 - John Smith - FWOS Models
						currStore.Min = minStock;
						currStore.Max = maxStock;
						// END MID Track #4370
						// BEGIN Track #5773 stodd 10.24.2008
						finalPlan = Round(finalPlan, _OTSPlanMethod.CurrentVariable.VariableProfile.NumDecimals);
						// END Track #5773 stodd 10.24.2008


						currStore.Inventory = finalPlan;
						unbalancedTotal += finalPlan;

						if (_OTSPlanMethod.MONITOR)
						{	
							int storeKey = currStore.Item;
							ForecastMonitorStoreData fmStoreData = _forecastMonitor.GetStoreData(currStore.Item);
							fmStoreData.Inventory = currStore.Inventory;
							fmStoreData.StockModifier = storeMod.StoreModifier;
							if (minStock != Include.Undefined)
								fmStoreData.StockMin = minStock;
							else
								fmStoreData.StockMin = 0;
							if (maxStock != Include.Undefined)
								fmStoreData.StockMax = maxStock;
							else
								fmStoreData.StockMax = 0;
						}
					}

				}

				//*******************************************************
				// Update Inventory Values
				//*******************************************************
				// BEGIN MID Track #4370 - John Smith - FWOS Models
				Hashtable summandHash = new Hashtable();
				// END MID Track #4370
				foreach(Summand summand in storeSummandList)
				{
					try
					{
						if (summand.Eligible && !summand.Locked) 
						{
							
							PlanCellReference cr = (PlanCellReference)_storeCellRefHash[summand.Item];
							//cr.SetCompCellValue(eSetCellMode.Computation, summand.Inventory);
							cr.SetEntryCellValue(summand.Inventory);
							// BEGIN MID Track #4370 - John Smith - FWOS Models
							if (summand.WeeksOfSupplyWasOverridden ||
								summand.MaxStockWasApplied) 
							{
								cr.SetCellLock(true);
							}
							// END MID Track #4370

							if (_OTSPlanMethod.MONITOR)
							{
								int storeKey = summand.Item;
								ForecastMonitorStoreData fmStoreData = _forecastMonitor.GetStoreData(summand.Item);
								fmStoreData.Inventory = summand.Inventory;
							}
						}
					}
					catch (CellNotAvailableException)
					{


						string msg = MIDText.GetText(eMIDTextCode.msg_InventoryCellIsProtected);
						msg = msg.Replace("{0}", StoreMgmt.GetStoreDisplayText(summand.Item)); //_SAB.StoreServerSession.GetStoreDisplayText(summand.Item));
						msg = msg.Replace("{1}", inventoryWeek.YearWeek.ToString());
						msg = msg.Replace("{2}", _fv.GetVersionText(_OTSPlanMethod.Plan_FV_RID));
						_SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Warning, msg, this.ToString());
						if (_OTSPlanMethod.MONITOR)
						{
							_forecastMonitor.WriteLine(msg);
						}
					}
					catch (Exception)
					{
						throw;
					}

					// BEGIN MID Track #4370 - John Smith - FWOS Models
					summandHash.Add(summand.Item, summand);
					// END MID Track #4370
				}

				// BEGIN MID Track #4370 - John Smith - FWOS Models
				_OTSPlanMethod.InventoryForecastSummandHash.Add(inventoryWeek.Key, summandHash);
				// END MID Track #4370

				if (_OTSPlanMethod.MONITOR)
				{
					_forecastMonitor.WriteLine(" ");
					_forecastMonitor.WriteLine("Inventory for " + inventoryWeek.YearWeek.ToString() + " COMPLETED.");
					_forecastMonitor.WriteAllStoreData();
				}

				//DebugSummandList(_OTSPlanMethod.InventoryForecastSummandList, "After Store Inventory Processing");

				if (_OTSPlanMethod.MONITOR)
				{
					_forecastMonitor.ClearWeeklyDataOnly();
				}

				weekInPlan++;

			} // end week loop
		}

		// BEGIN TT#619 - STodd - OTS Forecast - Chain Plan not required (#46)
		private double GetWeeksOfSupply(WeekProfile inventoryWeek)
		{
			double weeksOfSupply = 0;

			switch (_OTSPlanMethod.ApplyTrendOptionsInd)
			{
				case 'C':
					weeksOfSupply = GetWeeksOfSupplyUsingChainPlans(inventoryWeek);
					break;
				case 'W':
					weeksOfSupply = GetWeeksOfSupplyUsingChainWOS(inventoryWeek);
					break;
				case 'S':
					weeksOfSupply = GetWeeksOfSupplyUsingPlugChainWOS();
					break;
				default:
					break;
			}

			if (_OTSPlanMethod.MONITOR)
			{
				_forecastMonitor.WriteLine("Week: " + inventoryWeek.YearWeek.ToString() +
					"  WOS: " + weeksOfSupply.ToString("00.0000"));
			}

			return weeksOfSupply;
		}
		// END TT#619 - STodd - OTS Forecast - Chain Plan not required (#46)


		private double GetWeeksOfSupplyUsingChainPlans(WeekProfile inventoryWeek)
		{

			double weekCount = 0;
			double weeksOfSupply = 0;
			double salesUsed = 0;
			double salesQuantityUsed = 0;
			int dayCount = 0;
			WeekProfile salesWeek = null;
            // BEGIN MID Track #6043 - KJohnson - Forcast Chain WOS Calculations
            int numberOfWeeksWithZeroSales = 0;
            int numberOfWeeksWithZeroSalesCounter = 0;
            // END MID Track #6043 - KJohnson

			double totalInventory = _chainInventoryValue;

			if (_OTSPlanMethod.MONITOR)
			{
				_forecastMonitor.WriteLine("Weeks of Supply--Inventory Amount: " + 
					_chainInventoryValue.ToString());
			}
			salesWeek = inventoryWeek;

            // BEGIN MID Track #6043 - KJohnson - Forcast Chain WOS Calculations
            numberOfWeeksWithZeroSales = _SAB.ApplicationServerSession.GlobalOptions.NumberOfWeeksWithZeroSales;
            // END MID Track #6043 - KJohnson

			do 
			{
				_chainSalesValue = _OTSPlanMethod.ReadChainValue(salesWeek, _OTSPlanMethod.SalesVariable);

                // BEGIN MID Track #6043 - KJohnson - Forcast Chain WOS Calculations
				// if we don't find any more sales, we're done.
                if (_chainSalesValue == 0)
                {
                    numberOfWeeksWithZeroSalesCounter++;
                }
                // Begin Track #6299 - JSmith - OTS Default Tab "No. of weeks with zero sales" not calculating correctly
                // must be consecutive weeks
                else
                {
                    numberOfWeeksWithZeroSalesCounter = 0;
                }
                // End Track #6299

                // Begin Track #6299 - JSmith - OTS Default Tab "No. of weeks with zero sales" not calculating correctly
                //if (numberOfWeeksWithZeroSalesCounter > numberOfWeeksWithZeroSales) //<--BEGIN MID Track #6168 - KJohnson - Options not followed
                if (numberOfWeeksWithZeroSalesCounter >= numberOfWeeksWithZeroSales) //<--BEGIN MID Track #6168 - KJohnson - Options not followed
                // End Track #6299
                {
                    break;
                }
                // END MID Track #6043 - KJohnson

				if (_OTSPlanMethod.MONITOR)
				{
					_forecastMonitor.WriteLine("Week: " + salesWeek.YearWeek.ToString() +
						"  Sales: " + _chainSalesValue.ToString() );
				}

                if (_chainSalesValue != 0) //<--BEGIN MID Track #6168 - KJohnson - Options not followed
                {
                    if (totalInventory >= _chainSalesValue)
                    {
                        salesUsed += _chainSalesValue;
                        totalInventory -= _chainSalesValue;
                        weekCount += 1;
                    }
                    else
                    {
                        //*******************************************************************************
                        // since the week's sales is greater than the inventory left, we must calculate
                        // the daily sales (as 1/7 of sales)
                        //*******************************************************************************
                        ArrayList dayPctList = new ArrayList();


                        double dailySales = _chainSalesValue * .142857;  // 1/7th

                        for (int day = 0; day < 7; day++)
                        {
                            if (totalInventory >= dailySales)
                            {
                                salesUsed += dailySales;
                                totalInventory -= dailySales;
                                dayCount += 1;
                                if (dayCount == 7)
                                {
                                    weekCount += 1;
                                    dayCount = 0;
                                }
                            }
                            else
                            {
                                salesQuantityUsed = totalInventory;
                                salesUsed += salesQuantityUsed;
                                totalInventory = 0;

                                weekCount = weekCount + (salesQuantityUsed / _chainSalesValue);
                                break;
                            }

                        }
                    }
                }

				if (totalInventory == 0)
					break;

				// increment to next sales week
				salesWeek = _SAB.ApplicationServerSession.Calendar.Add(salesWeek, 1);

            // BEGIN MID Track #6043 - KJohnson - Forcast Chain WOS Calculations
            } while (numberOfWeeksWithZeroSalesCounter <= numberOfWeeksWithZeroSales);   //<--BEGIN MID Track #6168 - KJohnson - Options not followed
            // END MID Track #6043 - KJohnson

			weekCount = weekCount + (dayCount / 7d);

			//weekCount = Round2(weekCount);

			if (totalInventory == 0 || salesUsed == 0)
				weeksOfSupply = weekCount;
			else if (totalInventory > 0)
			{
				weeksOfSupply = (_chainInventoryValue * weekCount) / salesUsed;
			}

			return weeksOfSupply;
			//***************************************
			// End of Weeks of Supply calculation
			//***************************************

		}
		// BEGIN TT#619 - STodd - OTS Forecast - Chain Plan not required (#46)
		private double GetWeeksOfSupplyUsingChainWOS(WeekProfile inventoryWeek)
		{
			double weeksOfSupply = 0;
			ModelVariableProfile chainWOSVariable = _OTSPlanMethod.GetChainPlanWOSVariable();
			weeksOfSupply = _OTSPlanMethod.ReadChainValueDouble(inventoryWeek, chainWOSVariable);
			return weeksOfSupply;
		}

		private double GetWeeksOfSupplyUsingPlugChainWOS()
		{
			return _OTSPlanMethod.ApplyTrendOptionsWOSValue;
		}
		// END TT#619 - STodd - OTS Forecast - Chain Plan not required (#46)

		/// <summary>
		/// Rounds a double to 0 decimal places
		/// </summary>
		/// <param name="aValue"></param>
		/// <returns></returns>
		// BEGIN Track #5773 stodd
		private double Round(double aValue, int decPlaces)
		{
			return System.Math.Round(aValue, decPlaces);
		}
		// END Track #5773 stodd

		/// <summary>
		/// Rounds a double to 0 decimal places
		/// </summary>
		/// <param name="aValue"></param>
		/// <returns></returns>
		private double Round(double aValue)
		{
			//return (double)((int)(aValue + .5d)) ;
			return System.Math.Round(aValue, 0);
		}

		private void DebugSummandList(ArrayList summandList, string text)
		{

			// DEBUGGING
			for (int s=0;s<summandList.Count;s++)
			{
				string locked = " ";
				string inelig = " ";

				if (((Summand)summandList[s]).Locked)
					locked = "L";
				else
					locked = " ";
				if (((Summand)summandList[s]).Eligible)
					inelig = " ";
				else
					inelig = "E";

				Debug.WriteLine(text + 
					locked + inelig + 
					" STORE " + ((Summand)summandList[s]).Item.ToString(CultureInfo.CurrentUICulture) + "[" + s.ToString(CultureInfo.CurrentUICulture) + "] " +
					" SET " + ((Summand)summandList[s]).Set.ToString(CultureInfo.CurrentUICulture) +
					" GRADE " + ((Summand)summandList[s]).Grade.ToString(CultureInfo.CurrentUICulture) +
					" QUANTITY " + ((Summand)summandList[s]).Quantity.ToString(CultureInfo.CurrentUICulture)
					);
			}

			Debug.Flush();
		}

		private class StoreGradeInventoryProfile : Profile
		{
			private int _startIndex;
			private int _totalCount;
			private int _medianIndex;
			private int	_WOSIndex;
			private bool _first;
			private bool _last;

			public int StartIndex 
			{
				get{return _startIndex;}
				set{_startIndex = value;}
			}
			public int TotalCount 
			{
				get{return _totalCount;}
				set{_totalCount = value;}
			}
			public int MedianIndex 
			{
				get{return _medianIndex;}
				set{_medianIndex = value;}
			}
			public int WOSIndex 
			{
				get{return _WOSIndex;}
				set{_WOSIndex = value;}
			}
			public bool First 
			{
				get{return _first;}
				set{_first = value;}
			}
			public bool Last 
			{
				get{return _last;}
				set{_last = value;}
			}

			override public eProfileType ProfileType
			{
				get
				{
					return eProfileType.StoreGradeInventory;
				}
			}

			public StoreGradeInventoryProfile(int aKey)
				: base(aKey)
			{
				_startIndex = 0;
				_totalCount = 0;
				_medianIndex = 0;
				_WOSIndex = 0;
				_first = false;
				_last = false;
			}

		}


	}
}
