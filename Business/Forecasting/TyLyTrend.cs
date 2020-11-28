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
	/// Summary description for TyLyTrend.
	/// </summary>
	public class TyLyTrend : ForecastSalesBase
	{
		private string _sourceModule;
		private ProfileList _planStoreList;
		private StoreWeekEligibilityList _storeEligibilty;
		private ArrayList _storePlanValues;
		private ArrayList _storeTYBasisValues;
		private ArrayList _storeLYBasisValues;
		private ArrayList _storeApplyToBasisValues;
		private ArrayList _basisTrendValues;
		private ArrayList _originalTrendValues;
		private double _chainValue;
		private WeekProfile _planWeek;
		private Hashtable _storeCellRefHash = new Hashtable();
		private ForecastMonitorStoreData _fmStoreData;
		private StoreWeekModifierList _storeSalesModifiers;
		private GroupLevelFunctionProfile _groupLevelFunction;
		private double _highLimit;
		private double _lowLimit;
		private double _tolerance;
		private eTrendCapID _trendCapId;
		private int _reserveStoreRid;
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
					_storePlanValues = null;
					_storeTYBasisValues = null;
					_storeLYBasisValues = null;
					_storeApplyToBasisValues = null;
					_basisTrendValues = null;
					_storeCellRefHash = null;
					_fmStoreData = null;
					_storeSalesModifiers = null;
					_groupLevelFunction = null;
					_originalTrendValues = null;
					disposed = true;
				}
			}
			catch (Exception)
			{
			}
		}
		// End MID Track #5210
		#endregion

		public TyLyTrend(SessionAddressBlock SAB, OTSPlanMethod otsPlanMethod)
			: base(SAB, otsPlanMethod)
		{
			_SAB = SAB;
			_OTSPlanMethod = otsPlanMethod;
			_sourceModule = "TyLyTrend.cs";
			_forecastMonitor = otsPlanMethod.ForecastMonitor;			
		}

		// BEGIN MID Track #4370 - John Smith - FWOS Models
//		public void ProcessSets(ArrayList setList)
		public void ProcessSets(ArrayList setList, ArrayList aWeeksToProcess)
		// END MID Track #4370
		{
			try
			{
				if (_OTSPlanMethod.MONITOR)
				{
					_forecastMonitor.MonitorType = eForecastMonitorType.TyLyTrend;
					_forecastMonitor.ClearSetDataOnly();
					_forecastMonitor.AddSets(setList);
					_forecastMonitor.WriteLine("Sales Variable: " + _OTSPlanMethod.CurrentVariable.VariableProfile.VariableName);
					// BEgin Track #5773 stodd
					if (_OTSPlanMethod.CurrentVariable.ForcastModelRID != Include.NoRID)
					{
						_forecastMonitor.WriteLine("Forecast Formula: " + ((eForecastFormulaType)_OTSPlanMethod.CurrentVariable.ForecastFormula));
						_forecastMonitor.WriteLine("Variable Selections from Forecast Model");
						_forecastMonitor.WriteLine("  " + "Sales Mod(" + _OTSPlanMethod.CurrentVariable.SalesModifier.ToString() + ") ");
					}
					// End Track #5773 stodd
				}

				// Since all sets in the list are done the same, the first set is good enough...
				_groupLevelFunction = (GroupLevelFunctionProfile)setList[0];
				int sgl = _groupLevelFunction.Key;

				//****************************************************************
				// Builds the plan store list using all of the store group levels
				//****************************************************************
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
				

				GetTrendCapInfo();
				if (_OTSPlanMethod.MONITOR)
				{
					if (_trendCapId == eTrendCapID.Tolerance)
					{
						_forecastMonitor.WriteLine("Tolerance Trend Cap. " + 
							"  Tolerance: " + _tolerance.ToString(CultureInfo.CurrentUICulture) + "%" );
					}
					else if (_trendCapId == eTrendCapID.Limits)
					{
						_forecastMonitor.WriteLine("Trend Limits. " +
							" Low Limit: " + _lowLimit.ToString("###.#####", CultureInfo.CurrentUICulture) + 
							" High Limit: " + _highLimit.ToString("###.#####", CultureInfo.CurrentUICulture));
					}
					else
					{
						_forecastMonitor.WriteLine("No Trend Caps designated.");
					}
				}

				// Begin Issue 3752 - stodd 
				_reserveStoreRid =_SAB.ApplicationServerSession.GlobalOptions.ReserveStoreRID;
				// End Issue 3752 - stodd 			
				// Begin Track # 6418 - stodd - spreading negatives.
				base.AllowChainNegatives = _OTSPlanMethod.CurrentVariable.AllowChainNegatives;
				// End Track # 6418 - stodd - spreading negatives.

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
					_planWeek = planWeek;
					weekInPlan++;

					//************************
					// get store eligibility
					//************************
					_storeEligibilty = _OTSPlanMethod.ApplicationTransaction.GetStoreEligibilityForSales(_OTSPlanMethod.Plan_HN_RID,planWeek.Key);

					//*************************************
					// Read store plan values for each store
					//*************************************
					_storePlanValues = _OTSPlanMethod.ReadStoreValues(planWeek, _OTSPlanMethod.SalesVariable);

					//*************************************
					// Read chain plan value
					//*************************************
					//_chainValue = _OTSPlanMethod.ReadChainSalesValue(planWeek);
					if (_OTSPlanMethod.MONITOR)
					{
						_forecastMonitor.ClearWeeklyDataOnly();
						_forecastMonitor.YearWeek = planWeek.YearWeek;
						_forecastMonitor.ChainValue = _chainValue;
						_forecastMonitor.WriteSetHeader();
					}

					if (_OTSPlanMethod.MONITOR)
						_OTSPlanMethod.DisplayPlanWeekAlignment();

					//*************************************
					// Read store basis value
					//*************************************
					_storeTYBasisValues = _OTSPlanMethod.ReadStoreBasisValues(weekInPlan, planWeek, eTyLyType.TyLy, _OTSPlanMethod.CurrentVariable);	
					_storeLYBasisValues = _OTSPlanMethod.ReadStoreBasisValues(weekInPlan, planWeek, eTyLyType.AlternateLy, _OTSPlanMethod.CurrentVariable);
                    //BEGIN TT#43 - MD - DOConnell - Projected Sales Enhancement
                    if (!_groupLevelFunction.Proj_Curr_Wk_Sales_IND)
                    {
                        _storeApplyToBasisValues = _OTSPlanMethod.ReadStoreBasisValues(weekInPlan, planWeek, eTyLyType.AlternateApplyTo, _OTSPlanMethod.CurrentVariable);
                    }
                    else
                    {
                        _storeApplyToBasisValues = _OTSPlanMethod.ReadStoreBasisValues(weekInPlan, planWeek, eTyLyType.ProjectCurrWkSales, _OTSPlanMethod.CurrentVariable);
                    }
                    //END TT#43 - MD - DOConnell - Projected Sales Enhancement
					// BEGIN Issue 5401 stodd 4.29.2008 
					MissingBasisCheck(sgl);
					// END Issue 5401
					
					//*************************************
					// Compute Trends
					//*************************************
					_basisTrendValues = ComputeBasisTrendValues();

					//****************************************
					// Get Sales Modifier for stores for week
					//****************************************
					_storeSalesModifiers = _OTSPlanMethod.ApplicationTransaction.GetStoreModifierForSales(_OTSPlanMethod.Plan_HN_RID, planWeek.YearWeek);

					ArrayList summandList = new ArrayList();
					GradeStoreBin [] gradeStoreBinList = new GradeStoreBin[ _OTSPlanMethod.AllStoreList.ArrayList.Count ];
					_storeCellRefHash.Clear();
					double basisTrendTotal = 0, ineligibleTrendTotal = 0;
					for (int s=0;s<_basisTrendValues.Count;s++)
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
						summand.Eligible = ((StoreWeekEligibilityProfile)_storeEligibilty.FindKey( storeProfile.Key )).StoreIsEligible;
						// Begin Issue 3752 - stodd 
						// The RESERVE STORE is ALWAYS ineligible
						if (summand.Item == _reserveStoreRid)
							summand.Eligible = false;
						// End Issue 3752 - stodd 
						summand.Set = (int)_OTSPlanMethod.StoreGroupLevelHash[storeProfile.Key];
						summand.ApplyToValue = (double)_storeApplyToBasisValues[s];
						// change NEG to ZEROS
						summand.ApplyToValue = Math.Max(summand.ApplyToValue, 0.0);

						// if cell is locked quantity becomes locked cell value, otherwise we use the
						// basis value.
						//if (cr.isCellLocked)
						//{
						//	summand.Locked = true;
						//	summand.Quantity = cr.CurrentCellValue;
						//	lockedSummandTotal += summand.Quantity; 
						//}
						//else
						//{
							summand.Quantity = (double)_basisTrendValues[s];
							summand.Locked = false;
						//}

						if (summand.Eligible)
							basisTrendTotal += summand.Quantity;
						else
							ineligibleTrendTotal += summand.Quantity;

						if (_OTSPlanMethod.MONITOR)
						{
							_fmStoreData.StoreName = ((StoreProfile)_OTSPlanMethod.AllStoreList[s]).StoreId;
							_fmStoreData.InitValue = cr.CurrentCellValue;
							_fmStoreData.IsEligible = summand.Eligible;
							_fmStoreData.IsLocked = summand.Locked;
							_fmStoreData.Set = summand.Set;
							_fmStoreData.TyBasisValue = (double)_storeTYBasisValues[s];
							_fmStoreData.LyBasisValue = (double)_storeLYBasisValues[s];
							_fmStoreData.ApplyToValue = (double)_storeApplyToBasisValues[s];
							_fmStoreData.Trend = (double)_basisTrendValues[s];
							_fmStoreData.OriginalTrend = (double)_originalTrendValues[s];
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


					if (_OTSPlanMethod.MONITOR)
					{
						_forecastMonitor.WriteLine("Basis Trend Total     : " + basisTrendTotal.ToString());
						_forecastMonitor.WriteLine("Ineligible Trend Total: " + ineligibleTrendTotal.ToString());
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
							string errorMsg = this._OTSPlanMethod.Name + " for week " + planWeek.YearWeek.ToString(CultureInfo.CurrentUICulture) + 
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



					if (_OTSPlanMethod.MONITOR && _OTSPlanMethod.MonitorMode == eForecastMonitorMode.Verbose)
					{
						_forecastMonitor.WriteAllStoreData(true);
					}

					//****************************************************************
					// Using the SmoothBy enum we determine how to spread the values
					//****************************************************************
					int precision = 4;
					// Begin Track # 6418 - stodd - spreading negatives.
					// Begin Track # 6271 stodd
					//base.AllowChainNegatives = _OTSPlanMethod.CurrentVariable.AllowChainNegatives;
					// End Track # 6271 stodd
					// End Track # 6418 - stodd - spreading negatives.

					switch((eGroupLevelSmoothBy)_groupLevelFunction.GLSB_ID)
					{
						case eGroupLevelSmoothBy.None:
							SmoothByNone(summandList, basisTrendTotal, precision);
							break;
						case eGroupLevelSmoothBy.StoreSet:
							SmoothBySet(summandList, basisTrendTotal, precision);
							break;
						case eGroupLevelSmoothBy.StoreGrade:
							SmoothByGrade(summandList, basisTrendTotal, precision);
							break;
						case eGroupLevelSmoothBy.Both:
							SmoothByBoth(summandList, basisTrendTotal, precision);
							break;
						default:  // smooth by none
							string errorMsg = "Invalid SmoothBy option.";
							_SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Error, errorMsg, _sourceModule);
							continue;					
					}

					// update Forecast Monitor with smoothed trends
					// move trend in Summand from Result to Trend property
					
					int storeKey1;
					ForecastMonitorStoreData fmStoreData1;
					for (int s=0;s<_OTSPlanMethod.AllStoreList.ArrayList.Count;s++)
					{
						if (_OTSPlanMethod.MONITOR)
						{
							storeKey1 = ((Summand)summandList[s]).Item;
							fmStoreData1 = _forecastMonitor.GetStoreData(storeKey1);
							fmStoreData1.Trend = ((Summand)summandList[s]).Result;
						}
						
						((Summand)summandList[s]).Trend = ((Summand)summandList[s]).Result;
					}
				


					//****************************************************************************
					// Take values in summandList and copy them to the corresponding PlanCellRef
					// for only the stores we are planning this time 
					//****************************************************************************
					StoreProfile tempStoreProfile = new StoreProfile(0);
					foreach(Summand summand in summandList)
					{
						tempStoreProfile.Key = summand.Item;
						
						// is the store in this set?
						// is it Eligible?
						if ( _planStoreList.ArrayList.Contains( tempStoreProfile ) && summand.Eligible && !summand.Locked) 
						{
							
							StoreWeekModifierProfile storeModifier = (StoreWeekModifierProfile)_storeSalesModifiers.FindKey(summand.Item);

							PlanCellReference cr = (PlanCellReference)_storeCellRefHash[summand.Item];
							if (!cr.isCellLocked) // if its NOT locked
							{

								summand.Result = Math.Round(summand.Trend * summand.ApplyToValue, 0);
								//**********************
								// apply Sales Modifier
								//**********************
								if (storeModifier != null)
									summand.Result = summand.Result * storeModifier.StoreModifier;

								//cr.SetCompCellValue(eSetCellMode.Computation, summand.Result);
								cr.SetEntryCellValue(summand.Result);

							}
							else
							{
								summand.Locked = true;
								summand.Result = cr.CurrentCellValue;
							}

							if (_OTSPlanMethod.MONITOR)
							{
								int storeKey = summand.Item;
								ForecastMonitorStoreData fmStoreData = _forecastMonitor.GetStoreData(summand.Item);
								if (storeModifier != null)
									fmStoreData.SalesModifier = storeModifier.StoreModifier; 
								fmStoreData.ResultValue = summand.Result;
							}
						}
					}


					if (_OTSPlanMethod.MONITOR)
					{
						_forecastMonitor.WriteAllStoreData();
						//_forecastMonitor.ClearSetDataOnly();
					}

				}

			}
			catch (Exception)
			{
				throw;
			}
		}

		// BEGIN iss ue 5401 stodd
		/// <summary>
		/// Checks to be sure the basis has store information
		/// </summary>
		private void MissingBasisCheck(int sgl)
		{
			if (_storeTYBasisValues.Count == 0)
			{
				string msg = MIDText.GetText(eMIDTextCode.msg_MissingBasis);
				msg = msg.Replace("{0}", _OTSPlanMethod.Name);
                //StoreGroupLevelProfile sglp = StoreMgmt.GetStoreGroupLevel(sgl); //_SAB.StoreServerSession.GetStoreGroupLevel(sgl);
                string levelName = StoreMgmt.StoreGroupLevel_GetName(sgl); //TT#1517-MD -jsobek -Store Service Optimization
                msg = msg.Replace("{1}", levelName);
				msg = msg.Replace("{2}", "TY");
				_SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Error, msg, _sourceModule);
				throw new MIDException(eErrorLevel.severe, (int)eMIDTextCode.msg_MissingBasis, msg);
			}

			if (_storeLYBasisValues.Count == 0)
			{
				string msg = MIDText.GetText(eMIDTextCode.msg_MissingBasis);
				msg = msg.Replace("{0}", _OTSPlanMethod.Name);
                //StoreGroupLevelProfile sglp = StoreMgmt.GetStoreGroupLevel(sgl); //_SAB.StoreServerSession.GetStoreGroupLevel(sgl);
                string levelName = StoreMgmt.StoreGroupLevel_GetName(sgl); //TT#1517-MD -jsobek -Store Service Optimization
                msg = msg.Replace("{1}", levelName);
				msg = msg.Replace("{2}", "LY");
				_SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Error, msg, _sourceModule);
				throw new MIDException(eErrorLevel.severe, (int)eMIDTextCode.msg_MissingBasis, msg);
			}

			if (_storeApplyToBasisValues.Count == 0)
			{
				string msg = MIDText.GetText(eMIDTextCode.msg_MissingBasis);
				msg = msg.Replace("{0}", _OTSPlanMethod.Name);
                //StoreGroupLevelProfile sglp = StoreMgmt.GetStoreGroupLevel(sgl); //_SAB.StoreServerSession.GetStoreGroupLevel(sgl);
                string levelName = StoreMgmt.StoreGroupLevel_GetName(sgl); //TT#1517-MD -jsobek -Store Service Optimization
                msg = msg.Replace("{1}", levelName);
				msg = msg.Replace("{2}", "Apply To");
				_SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Error, msg, _sourceModule);
				throw new MIDException(eErrorLevel.severe, (int)eMIDTextCode.msg_MissingBasis, msg);
			}
		}
		// END issue 5401

		/// <summary>
		/// gets the trend cap information for this group level function
		/// </summary>
		private void GetTrendCapInfo()
		{

			_highLimit = double.MaxValue;
			_lowLimit = 0.0;
			_tolerance = 0.0;

			TrendCaps tc = new TrendCaps();
			DataTable dt = tc.GetTrendCaps(_OTSPlanMethod.Key, _groupLevelFunction.Key);
			if (dt != null && dt.Rows.Count > 0)	// Issue 5313 stodd 4.7.2008
			{
				DataRow row = dt.Rows[0];
				_trendCapId = (eTrendCapID)Convert.ToInt32(row["TREND_CAP_ID"], CultureInfo.CurrentUICulture);
				
				switch (_trendCapId)
				{
					case eTrendCapID.None:
						_highLimit = double.MaxValue;
						_lowLimit = 0.0;
						break;
					case eTrendCapID.Limits:
						_highLimit = Convert.ToDouble(row["HIGH_LIMIT"], CultureInfo.CurrentUICulture);
						// Begin Issue 3777 - stodd
						if (_highLimit == Include.UndefinedDouble)
							_highLimit = double.MaxValue;
						// End Issue 3777
						_lowLimit = Convert.ToDouble(row["LOW_LIMIT"], CultureInfo.CurrentUICulture);
						// Begin Issue 3777 - stodd
						if (_lowLimit == Include.UndefinedDouble)
							_lowLimit = 0.0;
						// End Issue 3777
						break;
					case eTrendCapID.Tolerance:
						_tolerance = Convert.ToDouble(row["TOL_PCT"], CultureInfo.CurrentUICulture);
						break;
					default:  // smooth by none
						// BEGIN Issue 5401 stodd
						_trendCapId = eTrendCapID.None;
						_highLimit = double.MaxValue;
						_lowLimit = 0.0;
						string msg = MIDText.GetText(eMIDTextCode.msg_MissingTrendCaps);
						msg = msg.Replace("{0}", _OTSPlanMethod.Name);
						//StoreGroupLevelProfile sglp = StoreMgmt.GetStoreGroupLevel(_groupLevelFunction.Key); //_SAB.StoreServerSession.GetStoreGroupLevel(_groupLevelFunction.Key);
                        string levelName = StoreMgmt.StoreGroupLevel_GetName(_groupLevelFunction.Key); //TT#1517-MD -jsobek -Store Service Optimization
                        msg = msg.Replace("{1}", levelName);
						_SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, msg, _sourceModule);		
						// END Issue 5401
						break;	
				}

			}

		}

		private ArrayList ComputeBasisTrendValues()
		{
			try
			{
				ArrayList trendValues = new ArrayList();
				for (int s=0;s<_storeTYBasisValues.Count;s++)
				{

					double tyValue = (double)_storeTYBasisValues[s];
					double lyValue = (double)_storeLYBasisValues[s];
					// BEGIN Issue 5425 stodd 5.5.2008 negative trend values
					// IF a basis value is negative, it throws off the trend. So we set the trend to 0.
					if (tyValue < 0 || lyValue < 0)
					{
						trendValues.Add(0.0d);
					}
					else if (lyValue != 0.0d)
					// END Issue 5425
					{
						double newVal = tyValue / lyValue;
						trendValues.Add(newVal);
					}
					else
						trendValues.Add(0.0d);
				}
				
				_originalTrendValues = new ArrayList(trendValues.ToArray());

				ApplyTrendCaps(trendValues);

				return trendValues;
			}
			catch (Exception)
			{
				throw;
			}
			
		}

		/// <summary>
		/// Applies trend caps to the list of trend values
		/// </summary>
		/// <param name="trendValues"></param>
		private void ApplyTrendCaps(ArrayList trendValues)
		{
			switch (_trendCapId)
			{
				case eTrendCapID.None:
					break;
				case eTrendCapID.Limits:
					for (int s=0;s<trendValues.Count;s++)
					{
						if ((double)trendValues[s] > _highLimit)
							trendValues[s] = _highLimit;
						if ((double)trendValues[s] < _lowLimit)
							trendValues[s] = _lowLimit;
					}
					break;
				case eTrendCapID.Tolerance:
					ApplyTrendTolerance(trendValues);
					break;
				default:  // smooth by none
					// BEGIN Issue 5401 stodd
					_trendCapId = eTrendCapID.None;
					string msg = MIDText.GetText(eMIDTextCode.msg_MissingTrendCaps);
					msg = msg.Replace("{0}", _OTSPlanMethod.Name);
					//StoreGroupLevelProfile sglp = StoreMgmt.GetStoreGroupLevel(_groupLevelFunction.Key); //_SAB.StoreServerSession.GetStoreGroupLevel(_groupLevelFunction.Key);
                    string levelName = StoreMgmt.StoreGroupLevel_GetName(_groupLevelFunction.Key); //TT#1517-MD -jsobek -Store Service Optimization
                    msg = msg.Replace("{1}", levelName);
					_SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, msg, _sourceModule);		
					// END Issue 5401
					break;	
			}		
		}

		/// <summary>
		/// updates the values in trendValues by applying the computed
		/// tolerance limits
		/// </summary>
		/// <param name="trendValues"></param>
		private void ApplyTrendTolerance(ArrayList trendValues)
		{
			TrendTolerance tt = new TrendTolerance(_tolerance, trendValues, _storeEligibilty, _OTSPlanMethod.AllStoreList);
			if (!tt.Compute())
			{
				string errorMsg = MIDText.GetText(eMIDTextCode.msg_InvalidTrendPercentage);
				_SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Error, errorMsg, _sourceModule);
				if (_OTSPlanMethod.MONITOR)
				{
					_forecastMonitor.WriteLine("ERROR: " + errorMsg + " No Limits will be calculated.");
				}
			}

			if (_OTSPlanMethod.MONITOR)
			{
				_forecastMonitor.WriteLine("Computed Limits based upon Tolerance of " + _tolerance.ToString() +
					"  Low Limit: " + tt.LowLimit.ToString("###.#####", CultureInfo.CurrentUICulture) + 
					" High Limit: " + tt.HighLimit.ToString("###.#####", CultureInfo.CurrentUICulture));
				_forecastMonitor.WriteLine("Mean: " + tt.Mean.ToString("###.#####", CultureInfo.CurrentUICulture) + 
					"  Std Deviation: " + tt.StdDeviation.ToString("###.#####", CultureInfo.CurrentUICulture));
			}
			
		}

	}

	/// <summary>
	/// Used to apply a given trend tolerance to a list of values.
	/// </summary>
	public class TrendTolerance
	{
		private double [] _zList = { 1.040,1.150,1.280,1.440,1.645,1.700,1.750,
									  1.810,1.880,1.960,2.050,2.170,2.330,2.580,2.810,3.090,3.290 };
		private double [] _probList = {.3,.25,.2,.15,.1,.09,.08,.07,.06,.05,.04,.03,.02,.01,.005,.002,.001 };
		private int _max = 17;
	
		private ArrayList _trendValues;
		private double _tolerance;
		private double _inverseTolerance;
		private double _lowLimit;
		private double _highLimit;
		StoreWeekEligibilityList _storeEligibilty;
		ProfileList _storeList;
		private double _avg;
		private double _stdDev;

		public double LowLimit
		{
			get	{return _lowLimit;}
		}
		public double HighLimit
		{
			get	{return _highLimit;}
		}
		public double Mean
		{
			get	{return _avg;}
		}
		public double StdDeviation
		{
			get	{return _stdDev;}
		}


		public TrendTolerance(double tolerance, ArrayList trendValues, 
			StoreWeekEligibilityList storeEligibilty, ProfileList storeList)
		{
			_trendValues = trendValues;
			_tolerance = tolerance / 100;  // make it a percentage
			_inverseTolerance = 1.0 - _tolerance;
			_inverseTolerance = (double)(decimal)Math.Round(_inverseTolerance, 4);
			_storeEligibilty = storeEligibilty;
			_storeList = storeList;
		}

		public bool Compute()
		{

			ArrayList eligibleValues = RemoveIneligibleValues(_trendValues);

			_avg = MIDMath.GetAvg(eligibleValues);
			_stdDev = MIDMath.GetStandardDeviation(eligibleValues);
			double zVar = GetZVar(_inverseTolerance);
			if (zVar == 0.0)
				return false;

			_highLimit = (_stdDev * zVar) + _avg;
			double negZVal = 0.0 - zVar;	
			_lowLimit = (_stdDev * negZVal) + _avg;

			// if low limit is negative, low limit is ZERO
			_lowLimit = Math.Max(_lowLimit, 0.0);

			for (int s=0;s<_trendValues.Count;s++)
			{
				if ((double)_trendValues[s] > _highLimit)
					_trendValues[s] = _highLimit;
				if ((double)_trendValues[s] < _lowLimit)
					_trendValues[s] = _lowLimit;
			}

			return true;
		}

		private ArrayList RemoveIneligibleValues(ArrayList trendValues)
		{
			ArrayList newList = new ArrayList();

			for (int s=0;s<_trendValues.Count;s++)
			{
				StoreProfile storeProfile = (StoreProfile)_storeList[s];
				bool isEligible = ((StoreWeekEligibilityProfile)_storeEligibilty.FindKey( storeProfile.Key )).StoreIsEligible;
				if (isEligible)
				{
					newList.Add( (double)_trendValues[s] );
				}
			}

			return newList;

		}

		public double GetZVar(double tol)
		{
			if (tol > 0 && tol <= .3)
			{
			}
			else
			{
				// error - tolerance is outside of boundry that we can calculate
				return 0.0;
			}

			double pHigh = 0.0;
			double pLow = 0.0;
			double zHigh = 0.0;
			double zLow = 0.0;
			double zVar = 0.0;

			for (int i=0;i<_max;i++)
			{
				if (tol == _probList[i])
				{
					zVar = _zList[i];
					break;// we are done!
				}

				if (tol > _probList[i])
				{
					pLow = _probList[i];
					pHigh = _probList[i-1];
					zLow = _zList[i];
					zHigh = _zList[i-1];

					double pctChange = (tol - pLow) / (pHigh - pLow);

					zVar = zLow - (pctChange * (zLow - zHigh));
					break;
				}
			}

			return zVar;
		}
	}
}
