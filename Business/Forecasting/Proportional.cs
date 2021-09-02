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
	/// Summary description for Proportional.
	/// </summary>
	public class Proportional : ForecastSalesBase
	{

		private string _sourceModule;
		private ProfileList _planStoreList;
		private StoreWeekEligibilityList _storeEligibilty;
		private ArrayList _setList;
		private ArrayList _storePlanValues;
		private ArrayList _storeBasisValues;
		private double _chainValue;
		private WeekProfile _planWeek;
		private Hashtable _storeCellRefHash = new Hashtable();
		private ForecastMonitorStoreData _fmStoreData;
		private StoreWeekModifierList _storeSalesModifiers;
		private int _reserveStoreRid;

		// BENCHMARKING
		System.DateTime beginTime;
		System.DateTime endTime;
		System.DateTime beginTime2;
		System.DateTime endTime2;
		// \BENCHMARKING

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
					_planStoreList = null;
					_storeEligibilty = null;
					_setList = null;
					_storePlanValues = null;
					_storeBasisValues = null;
					_storeCellRefHash = null;
					_fmStoreData = null;
					_storeSalesModifiers = null;
					disposed = true;
				}
			}
			catch (Exception)
			{
			}
		}
		// End MID Track #5210
		#endregion

		public Proportional(SessionAddressBlock SAB, OTSPlanMethod otsPlanMethod)
			: base(SAB, otsPlanMethod)
		{
			_SAB = SAB;
			_OTSPlanMethod = otsPlanMethod;
			_sourceModule = "proportional.cs";
			_forecastMonitor = otsPlanMethod.ForecastMonitor;
		}

		/// <summary>
		/// Processes the current Group Level Function using the Proportional Spread algorithm.
		/// </summary>
		/// <param name="setList"></param>
		/// <param name="aWeeksToProcess">The list of weeks to process</param>
		// BEGIN MID Track #4370 - John Smith - FWOS Models
//		public void ProcessSets(ArrayList setList)
		// Begin Track #6187 stodd
		public void ProcessSets(ArrayList setList, ArrayList aWeeksToProcess, ModelVariableProfile assocVariable, bool usePlanAsBasis)
		// End Track #6187
		// END MID Track #4370
		{
			try
			{
				beginTime = System.DateTime.Now;
				beginTime2 = System.DateTime.Now;
				_setList = setList;
				if (_OTSPlanMethod.MONITOR)
				{
					_forecastMonitor.MonitorType = eForecastMonitorType.PercentContribution;
					_forecastMonitor.ClearSetDataOnly();
					_forecastMonitor.AddSets(setList);
					_forecastMonitor.WriteLine("Variable: " + _OTSPlanMethod.CurrentVariable.VariableProfile.VariableName);
					// BEgin Track #5773 stodd
					if (_OTSPlanMethod.CurrentVariable.ForcastModelRID != Include.NoRID)
					{
						_forecastMonitor.WriteLine("Forecast Formula: " + ((eForecastFormulaType)_OTSPlanMethod.CurrentVariable.ForecastFormula));
						_forecastMonitor.WriteLine("Variable Selections from Forecast Model");
						_forecastMonitor.WriteLine("  " + "Sales Mod(" + _OTSPlanMethod.CurrentVariable.SalesModifier.ToString() + ") ");
						// Begin Track #6187 stodd
						if (assocVariable != null)
							_forecastMonitor.WriteLine("  Associated Variable: " + assocVariable.VariableProfile.VariableName);
						_forecastMonitor.WriteLine("  Use Plan As Basis: " + usePlanAsBasis.ToString());
						// End Track #6187
					}
					// End Track #5773 stodd
				}

				// Since all sets in the list are done the same, the first set is good enough...
				GroupLevelFunctionProfile _groupLevelFunction = (GroupLevelFunctionProfile)setList[0];
				int sgl = _groupLevelFunction.Key;

				// Begin Issue 3752 - stodd 
				_reserveStoreRid =_SAB.ApplicationServerSession.GlobalOptions.ReserveStoreRID;
				// End Issue 3752 - stodd 			

				//****************************************************************
				// Builds the plan store list using all of the store group levels
				//****************************************************************
				Debug.WriteLine("Run Time: " + DateTime.Now.ToString(CultureInfo.CurrentUICulture));
				_planStoreList = new ProfileList(eProfileType.Store);
				foreach(GroupLevelFunctionProfile glf in setList)
				{
                    ProfileList storeList = StoreMgmt.StoreGroupLevel_GetStoreProfileList(_OTSPlanMethod.SG_RID, glf.Key); //_SAB.StoreServerSession.GetStoresInGroup(_OTSPlanMethod.SG_RID, glf.Key);

					foreach(StoreProfile sp in storeList.ArrayList)
					{
						if (_OTSPlanMethod.MONITOR)
						{
							_forecastMonitor.AddStore(sp.StoreId + "(" + sp.Key.ToString(CultureInfo.CurrentUICulture) + ")");
						}
					}
					_planStoreList.AddRange(storeList);
				}

				// Begin Track # 6418 - stodd - spreading negatives.
				base.AllowChainNegatives = _OTSPlanMethod.CurrentVariable.AllowChainNegatives;
				// End Track # 6418 - stodd - spreading negatives.

				// BENCHMARKING
				endTime2 = System.DateTime.Now;
				Debug.WriteLine("init & get basis data -- " + System.Convert.ToString(endTime2.Subtract(beginTime2), CultureInfo.CurrentUICulture));
				beginTime2 = System.DateTime.Now;
				// \BENCHMARKING

				//******************
				// Process each week
				//******************
				int weekInPlan = 0;
				// BEGIN MID Track #4370 - John Smith - FWOS Models
//				foreach(WeekProfile planWeek in _OTSPlanMethod.WeeksToPlan.ArrayList)
				foreach(WeekProfile planWeek in aWeeksToProcess)
				// END MID Track #4370
				{
					_OTSPlanMethod.WeekBeingPlanned = planWeek; // Track #6187

					// BENCHMARKING
					beginTime2 = System.DateTime.Now;
					// BENCHMARKING
					// Begin Issue 6280 - stodd
					if (_OTSPlanMethod.CurrentVariable.VariableProfile.VariableForecastType == eVariableForecastType.Stock)
					{
						ForecastVersion fv = new ForecastVersion();
						bool versionProtected = fv.GetIsVersionProtected(_OTSPlanMethod.Plan_FV_RID);
						WeekProfile currentWeek = _SAB.ApplicationServerSession.Calendar.CurrentWeek;
						if (planWeek == currentWeek && versionProtected)  // compares keys
						{
							string errorMsg = MIDText.GetText(eMIDTextCode.msg_VersionProtectedNoInventoryPlanned);
							errorMsg = errorMsg.Replace("{0}", planWeek.YearWeek.ToString(CultureInfo.CurrentUICulture));
							errorMsg = errorMsg.Replace("{1}", _OTSPlanMethod.CurrentVariable.VariableProfile.VariableName);
							errorMsg = errorMsg.Replace("{2}", "planned");
							_SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Warning, errorMsg, _sourceModule);
							if (_OTSPlanMethod.MONITOR)
							{
								_forecastMonitor.WriteLine("WARNING - " + errorMsg);
							}
							continue;
						}
					}
					// End Issue 6280
					_planWeek = planWeek;
					weekInPlan++;

					//************************
					// get store eligibility
					//************************
					//_storeEligibilty = _OTSPlanMethod.ApplicationTransaction.GetStoreEligibilityForSales(_OTSPlanMethod.Plan_HN_RID,((WeekProfile)_OTSPlanMethod.WeeksToPlan[0]).Key);
                    // Begin Track #6324 - JSmith - Eligibility settings for MD CXL, Mfg All, Add MKU, Grs Rec
                    //_storeEligibilty = _OTSPlanMethod.ApplicationTransaction.GetStoreEligibilityForSales(_OTSPlanMethod.Plan_HN_RID,planWeek.Key);
                    if (_OTSPlanMethod.CurrentVariable.VariableProfile.EligibilityType == eEligibilityType.Sales)
                    {
                        _storeEligibilty = _OTSPlanMethod.ApplicationTransaction.GetStoreEligibilityForSales(
                            eRequestingApplication.Forecast, 
                            _OTSPlanMethod.Plan_HN_RID, 
                            planWeek.Key
                            );
                    }
                    else if (_OTSPlanMethod.CurrentVariable.VariableProfile.EligibilityType == eEligibilityType.Stock)
                    {
                        _storeEligibilty = _OTSPlanMethod.ApplicationTransaction.GetStoreEligibilityForStock(
                            eRequestingApplication.Forecast, 
                            _OTSPlanMethod.Plan_HN_RID, 
                            planWeek.Key
                            );
                    }
                    else 
                    {
                        _storeEligibilty = null;
                    }
                    // End Track #6324

					//*************************************
					// Read store plan values for each store
					//*************************************
					_storePlanValues = _OTSPlanMethod.ReadStoreValues(planWeek, _OTSPlanMethod.CurrentVariable);

					//*************************************
					// Read chain plan value
					//*************************************
					_chainValue = _OTSPlanMethod.ReadChainValue(planWeek, _OTSPlanMethod.CurrentVariable);
					if (_OTSPlanMethod.MONITOR)
					{
						_forecastMonitor.ClearWeeklyDataOnly();
						_forecastMonitor.YearWeek = planWeek.YearWeek;
						_forecastMonitor.ChainValue = _chainValue;
						_forecastMonitor.WriteSetHeader();
					}

					if (_chainValue == 0)
					{
						// Begin Track # 6430 - stodd - because of the change below for Track #5211,
						//  The period was being shifted twice. This shift is no longer needed.
						// Begin Issue #4286 - Stodd 
						//_OTSPlanMethod.HandleBasisPeriodShift(weekInPlan, planWeek, eTyLyType.NonTyLy);
						// End Issue #4286 - Stodd 
						// End Issue #6430 - Stodd 

						string errorMsg = MIDText.GetText(eMIDTextCode.msg_ZeroChainTotalNoSpread);
						errorMsg = errorMsg.Replace("{0}", _planWeek.YearWeek.ToString(CultureInfo.CurrentUICulture));
						_SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, errorMsg, _sourceModule);
						if (_OTSPlanMethod.MONITOR)
						{
							_forecastMonitor.WriteLine("INFORMATIONAL - " + errorMsg);
						}
						//continue;		// Issue 5211
					}

					// Begin Track #6418 - stodd - neg spread
					if (_chainValue < 0 && !AllowChainNegatives)
					{
						_OTSPlanMethod.HandleBasisPeriodShift(weekInPlan, planWeek, eTyLyType.NonTyLy);

						string errorMsg = MIDText.GetText(eMIDTextCode.msg_NegativeChainTotalNoSpread);
						errorMsg = errorMsg.Replace("{0}", _planWeek.YearWeek.ToString(CultureInfo.CurrentUICulture));
						_SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, errorMsg, _sourceModule);
						if (_OTSPlanMethod.MONITOR)
						{
							_forecastMonitor.WriteLine("INFORMATIONAL - " + errorMsg);
						}
						continue;
					}
					// End Track #6418 - stodd - neg spread

					if (_OTSPlanMethod.MONITOR)
						_OTSPlanMethod.DisplayPlanWeekAlignment();

					//*************************************
					// Read store basis value
					//*************************************
					// BEGIN Track #5874 stodd 
					// When forecasting other variables, sometimes the Sales variable was done 
					// using TY/LY Trend, so no proportional basis existed.
					eTyLyType basisType = _OTSPlanMethod.GetBasisType();
					// Begin Track #6187 stodd
					ModelVariableProfile basisVariable = _OTSPlanMethod.CurrentVariable;
					if (assocVariable != null)
						basisVariable = assocVariable;
					if (usePlanAsBasis)
					{
						_storeBasisValues = new ArrayList();
						ArrayList valueList = _OTSPlanMethod.ReadStoreValues(planWeek, basisVariable);
						for (int i = 0; i < valueList.Count; i++)
						{
							PlanCellReference tPcr = (PlanCellReference)valueList[i];
							double storeValue = tPcr.HiddenCurrentCellValue;
							_storeBasisValues.Add(storeValue);
						}
					}
					else
						_storeBasisValues = _OTSPlanMethod.ReadStoreBasisValues(weekInPlan, planWeek, basisType, basisVariable);
					// End Track #6187 stodd
					// END Track #5874 stodd 

					//****************************************
					// Get Sales Modifier for stores for week
					//****************************************
					_storeSalesModifiers = _OTSPlanMethod.ApplicationTransaction.GetStoreModifierForSales(_OTSPlanMethod.Plan_HN_RID, planWeek.YearWeek);

					//*******************************************************
					// Load up summand array list getting ready to do spread
					// also build grade store list for store grade look up
					// Note: summand.Item IS the store RID
					//*******************************************************
					ArrayList summandList = new ArrayList();
					GradeStoreBin [] gradeStoreBinList = new GradeStoreBin[ _OTSPlanMethod.AllStoreList.ArrayList.Count ];
					_storeCellRefHash.Clear();
					double lockedSummandTotal = 0;
					double basisTotal = 0, ineligibleTotal = 0;
					for (int s=0;s<_storePlanValues.Count;s++)
					{
						StoreProfile storeProfile = (StoreProfile)_OTSPlanMethod.AllStoreList[s];
						if (_OTSPlanMethod.MONITOR)
						{
							_fmStoreData = _forecastMonitor.CreateStoreData(storeProfile.Key);
						}

						PlanCellReference cr = (PlanCellReference)_storePlanValues[s];
						// this Hash is used later (after sorting) to rematch stores with the Cell Reference
						_storeCellRefHash.Add(storeProfile.Key, cr);
						Summand summand = new Summand();
						summand.Item = _OTSPlanMethod.AllStoreList[s].Key;
                        // Begin Track #6324 - JSmith - Eligibility settings for MD CXL, Mfg All, Add MKU, Grs Rec
                        //summand.Eligible = ((StoreWeekEligibilityProfile)_storeEligibilty.FindKey(storeProfile.Key)).StoreIsEligible;
                        if (_storeEligibilty == null)
                        {
                            summand.Eligible = true;
                        }
                        else
                        {
                            summand.Eligible = ((StoreWeekEligibilityProfile)_storeEligibilty.FindKey(storeProfile.Key)).StoreIsEligible;
                        }
                        // End Track #6324
						// Begin Issue 3752 - stodd 
						// The RESERVE STORE is ALWAYS ineligible
						if (summand.Item == _reserveStoreRid)
							summand.Eligible = false;
						// End Issue 3752 - stodd 
						summand.Set = (int)_OTSPlanMethod.StoreGroupLevelHash[storeProfile.Key];

						// if cell is locked quantity becomes locked cell value, otherwise we use the
						// basis value.
						if (cr.isCellLocked)
						{
							summand.Locked = true;
							summand.Quantity = cr.CurrentCellValue;
							lockedSummandTotal += summand.Quantity; 
						}
						else
						{
							summand.Quantity = (double)_storeBasisValues[s];
							summand.Locked = false;
						}

						if (summand.Eligible)
							basisTotal += summand.Quantity;
						else
							ineligibleTotal += summand.Quantity;

						if (_OTSPlanMethod.MONITOR)
						{
							_fmStoreData.StoreName = ((StoreProfile)_OTSPlanMethod.AllStoreList[s]).StoreId;
							_fmStoreData.InitValue = summand.Quantity;
							_fmStoreData.IsEligible = summand.Eligible;
							_fmStoreData.IsLocked = summand.Locked;
							_fmStoreData.Set = summand.Set;
						}

						summand.Min = 0;				//no min
						summand.Max = double.MaxValue;  //no max
						summandList.Add(summand);


						// build Grade Store Bin list to use to find store grades...later
						GradeStoreBin gradeStoreBin = new GradeStoreBin();
						gradeStoreBin.StoreKey = summand.Item;
						gradeStoreBin.StoreGradeUnits = summand.Quantity;
						gradeStoreBin.StoreEligible = summand.Eligible;
						gradeStoreBinList[s] = gradeStoreBin;
					}

					// Begin Track #6187 stodd
					if (lockedSummandTotal != 0 && lockedSummandTotal > _chainValue)
					{
						string msg = MIDText.GetText(eMIDTextCode.msg_LockedStoreTotalGreaterThanChain);
						msg = msg.Replace("{0}", _OTSPlanMethod.Name);
						msg = msg.Replace("{1}", basisVariable.VariableProfile.VariableName);
						msg = msg.Replace("{2}", lockedSummandTotal.ToString());
						msg = msg.Replace("{3}", _chainValue.ToString());
						msg = msg.Replace("{4}", _planWeek.YearWeek.ToString());
						_SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Error, msg, _sourceModule);
						continue;
					}
					// End Track #6187

					if (_OTSPlanMethod.MONITOR)
					{
						_forecastMonitor.WriteLine("Basis total     : " + basisTotal.ToString());
						_forecastMonitor.WriteLine("Ineligible total: " + ineligibleTotal.ToString());
					}

					//******************
					// get store grades
					//******************
					StoreGradeList storeGradeList = _OTSPlanMethod.ApplicationTransaction.GetStoreGradeList(_OTSPlanMethod.Plan_HN_RID);
					if (storeGradeList.Count == 0)
					{
						// No Store Grade List found
						throw new MIDException(eErrorLevel.severe,
							(int)eMIDTextCode.msg_NoGradeDefinition,
							MIDText.GetText(eMIDTextCode.msg_NoGradeDefinition));
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
							string errorMsg = this._OTSPlanMethod.Name + " for week " + _planWeek.YearWeek.ToString(CultureInfo.CurrentUICulture) + 
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

					// update summand with store grade
					for (int s=0;s<_OTSPlanMethod.AllStoreList.ArrayList.Count;s++)
					{
						if (_OTSPlanMethod.MONITOR)
						{
							int storeKey = ((Summand)summandList[s]).Item;
							ForecastMonitorStoreData fmStoreData = _forecastMonitor.GetStoreData(storeKey);
							fmStoreData.Grade = storeGrades[s];
						}
						((Summand)summandList[s]).Grade = storeGrades[s];
					}

					// Using the SmoothBy enum we determine how to spread the values
					int precision = 0;
					// Begin Track # 6418 stodd
					// Begin Track # 6271 stodd
					//base.AllowChainNegatives = _OTSPlanMethod.CurrentVariable.AllowChainNegatives;
					// End Track # 6271 stodd
					// End Track # 6418 stodd
					//BEGIN TT#541 – MD - DOConnell - OTS Forecast receive a "nothing to spread to exception" when weeks are locked 
                    try
                    {
                        switch ((eGroupLevelSmoothBy)_groupLevelFunction.GLSB_ID)
                        {
                            case eGroupLevelSmoothBy.None:
                                SmoothByNone(summandList, _chainValue, precision);
                                break;
                            case eGroupLevelSmoothBy.StoreSet:
                                SmoothBySet(summandList, _chainValue, precision);
                                break;
                            case eGroupLevelSmoothBy.StoreGrade:
                                SmoothByGrade(summandList, _chainValue, precision);
                                break;
                            case eGroupLevelSmoothBy.Both:
                                SmoothByBoth(summandList, _chainValue, precision);
                                break;
                            default:  // smooth by none
                                string errorMsg = "Invalid SmoothBy option.";
                                _SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Error, errorMsg, _sourceModule);
                                continue;
                        }
                    }
                    catch (NothingToSpreadException)
                    {
                        HierarchyNodeProfile hnp = _SAB.HierarchyServerSession.GetNodeData(_OTSPlanMethod.Plan_HN_RID);
                        string message = MIDText.GetText(eMIDTextCode.msg_StoreWeekPctContributionSpreadFailed);
                        message = message.Replace("{0}", hnp.Text);
                        message = message.Replace("{1}", planWeek.YearWeek.ToString(CultureInfo.CurrentCulture));
                        _SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Warning, eMIDTextCode.msg_StoreWeekPctContributionSpreadFailed, message, this.ToString());
                    }
					//END TT#541 – MD - DOConnell - OTS Forecast receive a "nothing to spread to exception" when weeks are locked 
					

					//****************************************************************************
					// Take values in summandList and copy them to the corresponding PlanCellRef
					// for only the stores we are planning this time 
					//****************************************************************************
					StoreProfile tempStoreProfile = new StoreProfile(0);
					int sCount = summandList.Count;
					Summand storeSummand = null;
					PlanCellReference pcr = null;
					int ss = 0;
					for (ss=0;ss<sCount;ss++)
					//foreach(Summand summand in summandList)
					{
						storeSummand = (Summand)summandList[ss];
						tempStoreProfile.Key = storeSummand.Item;

						//**********************
						// apply Sales Modifier
						//**********************
						StoreWeekModifierProfile storeModifier = (StoreWeekModifierProfile)_storeSalesModifiers.FindKey(storeSummand.Item);
						if (storeModifier != null)
						{
							// Begin Track #6187 stodd
							// BEGIN MID Track #5773 - KJohnson - Planned FWOS Enhancement
							if (_OTSPlanMethod.CurrentVariable.SalesModifier)
							{
								storeSummand.Result = storeSummand.Result * storeModifier.StoreModifier;
							}
							// END MID Track #5773
							// End track #6187
						}

						// is the store in this set?
						// is it Eligible?
						if ( _planStoreList.ArrayList.Contains( tempStoreProfile ) && storeSummand.Eligible && !storeSummand.Locked) 
						{
							
							pcr = (PlanCellReference)_storeCellRefHash[storeSummand.Item];
							//pcr.SetCompCellValue(eSetCellMode.Computation, storeSummand.Result);
							pcr.SetEntryCellValue(storeSummand.Result);
							if (_OTSPlanMethod.MONITOR)
							{
								int storeKey = storeSummand.Item;
								ForecastMonitorStoreData fmStoreData = _forecastMonitor.GetStoreData(storeSummand.Item);
								if (storeModifier != null)
									fmStoreData.SalesModifier = storeModifier.StoreModifier; 
								fmStoreData.ResultValue = storeSummand.Result;
							}
						}
					}

					//BENCHMARK
					endTime2 = System.DateTime.Now;
					Debug.WriteLine("Completed Plan Week " + planWeek.YearWeek.ToString("000000", CultureInfo.CurrentUICulture) +
						" Attr Set " + _groupLevelFunction.Key + " -- " +
						System.Convert.ToString(endTime2.Subtract(beginTime2), CultureInfo.CurrentUICulture));
					//BENCHMARK
					if (_OTSPlanMethod.MONITOR)
					{
						_forecastMonitor.DumpToFile();
					}

				}

				//BENCHMARK
				endTime = System.DateTime.Now;
				Debug.WriteLine("Completed ALL WEEKS " +
					" Attr Set " + _groupLevelFunction.Key + " -- " +
					System.Convert.ToString(endTime.Subtract(beginTime), CultureInfo.CurrentUICulture));
				//BENCHMARK
			}

			catch( Exception)
			{
				//TEMPORARY
				//MessageBox.Show(ex.Message);
				throw;
			}

		}

		//**********
		// MISC
		//**********

		private void DebugSummandList(ArrayList summandList, string text)
		{

			// DEBUGGING
			for (int s=0;s<summandList.Count;s++)
			{
				string locked = " ";
				string inelig = " ";
				PlanCellReference cr = (PlanCellReference)_storeCellRefHash[((Summand)summandList[s]).Item];

				if (((Summand)summandList[s]).Locked)
					locked = "L";
				else
					locked = " ";
				if (((Summand)summandList[s]).Eligible)
					inelig = " ";
				else
					inelig = "E";

				Debug.WriteLine(text + " PLAN WEEK " + _planWeek.YearWeek.ToString("000000", CultureInfo.CurrentUICulture) +
					locked + inelig + 
					" STORE " + ((Summand)summandList[s]).Item.ToString(CultureInfo.CurrentUICulture) + "[" + s.ToString(CultureInfo.CurrentUICulture) + "] " +
					"Plan Value " + cr.CurrentCellValue.ToString("0000000000.00", CultureInfo.CurrentUICulture) +
					" SET " + ((Summand)summandList[s]).Set.ToString(CultureInfo.CurrentUICulture) +
					" GRADE " + ((Summand)summandList[s]).Grade.ToString(CultureInfo.CurrentUICulture) +
					" QUANTITY " + ((Summand)summandList[s]).Quantity.ToString(CultureInfo.CurrentUICulture)
					);
			}

			Debug.Flush();
		}




	}
}
