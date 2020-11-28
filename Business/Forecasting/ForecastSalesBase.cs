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
	/// Summary description for ForecastSalesBase.
	/// </summary>
	public class ForecastSalesBase : Spread			// Track 6418 - stodd
	{
		public OTSPlanMethod _OTSPlanMethod;
		public SessionAddressBlock _SAB;
		public ForecastMonitor _forecastMonitor;
		private ProportionalSpread _spread;
		private bool _allowChainNegatives;	// Track #6271 stodd
		private ArrayList _summandList;

		public bool AllowChainNegatives
		{
			get { return _allowChainNegatives; }
			set { _allowChainNegatives = value; }
		}

		public ForecastSalesBase(SessionAddressBlock SAB, OTSPlanMethod otsPlanMethod)
		{
			_SAB = SAB;
			_OTSPlanMethod = otsPlanMethod;
			_forecastMonitor = otsPlanMethod.ForecastMonitor;
			_spread = new ProportionalSpread(_SAB);
		}

		//*************
		// SMOOTHING
		//*************

		#region Smoothing
		// Really just a proportional spread
		public int SmoothByNone(ArrayList summandList, double total, int precision)
		{
			// Set up for the Spread
			// Begin Track 6418 - stodd - spread negatives
			double []  basisArray = new double [summandList.Count];
			double []  spreadToArray = new double [summandList.Count];
			int i = 0;
			for (i=0; i<summandList.Count; i++)
			{
				basisArray[i] = ((Summand)summandList[i]).Quantity;
			}

			//************************
			// Do Proportional spread
			//************************
			// Begin Track 6494 - stodd 
			_summandList = summandList;
			// End Track 6494 - stodd 
			ExecuteForecastSpread(total, basisArray, spreadToArray, precision);

			for (i = 0; i < spreadToArray.Length; i++)
			{
				((Summand)summandList[i]).Result = spreadToArray[i];
			}

			//_spread.SummandList = summandList;
			//_spread.RequestedTotal = total;
			//_spread.Precision = precision;
			//_spread.AllowChainNegatives = _allowChainNegatives;	// track # 6271 stodd
			////************************
			//// Do Proportional spread
			////************************
			//// Begin Track #6187 stodd
			//int rc = _spread.Calculate();
			//if (rc != 0)
			//{
			//    string msg = MIDText.GetText(eMIDTextCode.msg_MethodWarning);
			//    msg = msg.Replace("{0}", this._OTSPlanMethod.Name);
			//    msg = msg.Replace("{1}", this._OTSPlanMethod.CurrentVariable.VariableProfile.VariableName);	// Variable
			//    msg = msg.Replace("{2}", this._OTSPlanMethod.WeekBeingPlanned.ToString()); // week
			//    string warnText = MIDText.GetText((eMIDTextCode)rc);
			//    msg = msg.Replace("{3}", warnText);
			//    _SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Warning, msg, this.ToString());
			//}
			return 0;
			// End Track #6187 stodd
			// End Track 6418 - stodd - spread negatives
		}

		public void SmoothBySet(ArrayList summandList, double total, int precision)
		{
			Summand summand = null;
			Summand totalSummand;
			
			// Sort by Store Set
			summandList.Sort(new SummandSort(eGroupLevelSmoothBy.StoreSet));

			// Get the set totals
			Hashtable totalSummandHash = TotalBySet(summandList);

			IDictionaryEnumerator myEnumerator = totalSummandHash.GetEnumerator();

			// We now need to SPREAD the chain value using the SET totals
			// We copy the hash table to a summandList and SmoothByNone
			// which is really just a proportional spread.
			//myEnumerator = totalSummandHash.GetEnumerator();
			ArrayList totalList = new ArrayList();
			while ( myEnumerator.MoveNext() )
			{
				totalList.Add(myEnumerator.Value);
			}
			// spread...totalSummedHash ends up with the values
			this.SmoothByNone(totalList, total, precision);

			// SMOOTHING by SET	
			ArrayList summandStoreSetList = new ArrayList();
			int currSet = ((Summand)summandList[0]).Set;
			Summand prevSummand = null;

			for (int s=0;s<summandList.Count;s++)
			{
				prevSummand = summand;
				summand = (Summand)summandList[s];  // for Convenience 
				if (currSet == summand.Set)
				{
					if (summand.Eligible)
					{
						if (summand.Locked)
						{
							summandStoreSetList.Add(summand);
						}
						else
						{
							summand.Quantity = 1;
							summandStoreSetList.Add(summand);
						}
					}
				}
				else  // set break
				{
					totalSummand = (Summand)totalSummandHash[prevSummand.Set];
					SmoothByNone(summandStoreSetList, totalSummand.Result, precision);

					if (_OTSPlanMethod.MONITOR)
					{
						string miscMsg = "Set: " + prevSummand.Set + " No Stores: " +
							summandStoreSetList.Count.ToString(CultureInfo.CurrentUICulture) + " Set Total: " +
							totalSummand.Result;
						_forecastMonitor.AddMiscMessage(miscMsg);
					}

					//***********************************************************************
					// It appears as if we smooth the set and then clear the values without 
					// ever placing them somehwere.  Remember that the summands in the 
					// summandStoreSetList are just references to the summands in the
					// summandList sent to this method.  The ending values are kept there.
					//***********************************************************************
					summandStoreSetList.Clear();
					if (summand.Eligible)
					{
						if (summand.Locked)
						{
							summandStoreSetList.Add(summand);
						}
						else
						{
							summand.Quantity = 1;
							summandStoreSetList.Add(summand);
						}
					}
					currSet = summand.Set;
				}
			}

			if (summandStoreSetList.Count > 0)
			{
				totalSummand = (Summand)totalSummandHash[summand.Set];
				SmoothByNone(summandStoreSetList, totalSummand.Result, precision);
				if (_OTSPlanMethod.MONITOR)
				{
					string miscMsg = "Set: " + summand.Set + " No Stores: " +
						summandStoreSetList.Count.ToString(CultureInfo.CurrentUICulture) + " Set Total: " +
						totalSummand.Result;
					_forecastMonitor.AddMiscMessage(miscMsg);
				}
			}

		}

		
		public void SmoothByGrade(ArrayList summandList, double total, int precision)
		{
			Summand summand = null;
			Summand totalSummand;

			// Sort by store Grade
			summandList.Sort(new SummandSort(eGroupLevelSmoothBy.StoreGrade)); 

			// Get totals by grade and spread chain value to them
			Hashtable totalSummandHash = TotalByGrade(summandList);

			IDictionaryEnumerator myEnumerator = totalSummandHash.GetEnumerator();

			// We now need to SPREAD the chain value using the GRADE totals
			// We copy the hash table to a summandList and SmoothByNone
			// which is really just a proportional spread.
			//myEnumerator = totalSummandHash.GetEnumerator();
			ArrayList totalList = new ArrayList();
			while ( myEnumerator.MoveNext() )
			{
				totalList.Add(myEnumerator.Value);
			}
			// spread
			this.SmoothByNone(totalList, total, precision);

			// SMOOTHING by GRADE
			ArrayList summandStoreGradeList = new ArrayList();
			int currGrade = ((Summand)summandList[0]).Grade;
			Summand prevSummand = null;

			for (int s=0;s<summandList.Count;s++)
			{
				prevSummand = summand;
				summand = (Summand)summandList[s];  // for Convenience 
				if (currGrade == summand.Grade)
				{
					if (summand.Eligible)
					{
						if (summand.Locked)
						{
							summandStoreGradeList.Add(summand);
						}
						else
						{
							summand.Quantity = 1;
							summandStoreGradeList.Add(summand);
						}
					}
				}
				else  // Grade break
				{
					totalSummand = (Summand)totalSummandHash[prevSummand.Grade];
					SmoothByNone(summandStoreGradeList, totalSummand.Result, precision);
					if (_OTSPlanMethod.MONITOR)
					{
						string miscMsg = "Grade: " + prevSummand.Grade + " No Stores: " +
							summandStoreGradeList.Count.ToString(CultureInfo.CurrentUICulture) + " Set Total: " +
							totalSummand.Result;
						_forecastMonitor.AddMiscMessage(miscMsg);
					}
					//***********************************************************************
					// It appears as if we smooth the grade and then clear the values without 
					// ever placing them somehwere.  Remember that the summands in the 
					// summandStoreGradeList are just references to the summands in the
					// summandList sent to this method.  The ending values are kept there.
					//***********************************************************************
					summandStoreGradeList.Clear();
					if (summand.Eligible)
					{
						if (summand.Locked)
						{
							summandStoreGradeList.Add(summand);
						}
						else
						{
							summand.Quantity = 1;
							summandStoreGradeList.Add(summand);
						}
					}				
					currGrade = summand.Grade;
				}
			}
			if (summandStoreGradeList.Count > 0)
			{
				totalSummand = (Summand)totalSummandHash[summand.Grade];
				SmoothByNone(summandStoreGradeList, totalSummand.Result, precision);
				if (_OTSPlanMethod.MONITOR)
				{
					string miscMsg = "Grade: " + summand.Grade + " No Stores: " +
						summandStoreGradeList.Count.ToString(CultureInfo.CurrentUICulture) + " Set Total: " +
						totalSummand.Result;
					_forecastMonitor.AddMiscMessage(miscMsg);
				}
			}
		}

		public void SmoothByBoth(ArrayList summandList, double total, int precision)
		{
			Summand summand = null;
			Summand totalSummand;

			// Sort by Store Set & Grade
			summandList.Sort(new SummandSort(eGroupLevelSmoothBy.Both));

			// Get the right totaling and average stores to do the smoothing with
			Hashtable totalSummandHash = TotalByBoth(summandList);

			IDictionaryEnumerator myEnumerator = totalSummandHash.GetEnumerator();

			// We now need to SPREAD the chain value using the SET totals
			// We copy the hash table to a summandList and SmoothByNone
			// which is really just a proportional spread.
			//myEnumerator = totalSummandHash.GetEnumerator();
			ArrayList totalList = new ArrayList();
			while ( myEnumerator.MoveNext() )
			{
				totalList.Add(myEnumerator.Value);
			}
			// spread
			this.SmoothByNone(totalList, total, precision);

			// SMOOTH by BOTH (SET & GRADE)
			ArrayList summandStoreBothList = new ArrayList();
			int currSet = ((Summand)summandList[0]).Set;
			int currGrade = ((Summand)summandList[0]).Grade;
			Summand prevSummand = null;
			for (int s=0;s<summandList.Count;s++)
			{
				prevSummand = summand;
				summand = (Summand)summandList[s];  // for Convenience 
				if (currSet == summand.Set && currGrade == summand.Grade)
				{
					if (summand.Eligible)
					{
						if (summand.Locked)
						{
							summandStoreBothList.Add(summand);
						}
						else
						{
							summand.Quantity = 1;
							summandStoreBothList.Add(summand);
						}
					}
				}
				else  // set break
				{
					totalSummand = (Summand)totalSummandHash[prevSummand.GetSetGradeHash()];
					SmoothByNone(summandStoreBothList, totalSummand.Result, precision);
					if (_OTSPlanMethod.MONITOR)
					{
						string miscMsg = "Set/Grade: " + prevSummand.Set.ToString(CultureInfo.CurrentUICulture) + "/" +
							prevSummand.Grade.ToString(CultureInfo.CurrentUICulture) + " No Stores: " +
							summandStoreBothList.Count.ToString(CultureInfo.CurrentUICulture) + " Set Total: " +
							totalSummand.Result;
						_forecastMonitor.AddMiscMessage(miscMsg);
					}
					//***********************************************************************
					// It appears as if we smooth the grade/set and then clear the values without 
					// ever placing them somehwere.  Remember that the summands in the 
					// summandStoreBothList are just references to the summands in the
					// summandList sent to this method.  The ending values are kept there.
					//***********************************************************************
					summandStoreBothList.Clear();
					if (summand.Eligible)
					{
						if (summand.Locked)
						{
							summandStoreBothList.Add(summand);
						}
						else
						{
							summand.Quantity = 1;
							summandStoreBothList.Add(summand);
						}
					}				
					currSet = summand.Set;
					currGrade = summand.Grade;
				}
			}
			if (summandStoreBothList.Count > 0)
			{
				totalSummand = (Summand)totalSummandHash[summand.GetSetGradeHash()];
				SmoothByNone(summandStoreBothList, totalSummand.Result, precision);
				if (_OTSPlanMethod.MONITOR)
				{
					string miscMsg = "Set/Grade: " + prevSummand.Set.ToString(CultureInfo.CurrentUICulture) + "/" +
						prevSummand.Grade.ToString(CultureInfo.CurrentUICulture) + " No Stores: " +
						summandStoreBothList.Count.ToString(CultureInfo.CurrentUICulture) + " Set Total: " +
						totalSummand.Result;
					_forecastMonitor.AddMiscMessage(miscMsg);
				}
			}
		}
		#endregion

		//*************
		// TOTALS
		//*************

		#region Totals
		/// <summary>
		/// Sorts the summandList by Set, then gets a total and and avg store for each set and places it in
		/// a hashtable.
		/// </summary>
		/// <param name="summandList"></param>
		/// <returns></returns>
		private Hashtable TotalBySet(ArrayList summandList)
		{
			Hashtable totalSummandHash = new Hashtable();
			int storeCount = 0;
			double setTotal = 0;
			int currSet = ((Summand)summandList[0]).Set;
			Summand totalSummand = null;
			Summand summand = null;
			Summand prevSummand = null;

			for (int s=0;s<summandList.Count;s++)
			{
				prevSummand = summand;
				summand = (Summand)summandList[s];  // for Convenience 
				if (currSet == summand.Set)
				{
					if (summand.Eligible)
					{
						storeCount++;
						setTotal += summand.Quantity;
					}
				}
				else  // set break
				{
					totalSummand = prevSummand.Clone();
					totalSummand.NumberOfStores = storeCount;
					totalSummand.Quantity = setTotal;
					totalSummandHash.Add(totalSummand.Set, totalSummand);
					
					if (summand.Eligible)
					{
						storeCount = 1;
						setTotal = summand.Quantity;
					}
					currSet = summand.Set;
				}
			}
		
			// get last SETs total
			if (summandList.Count > 0)
			{
				totalSummand = summand.Clone();
				totalSummand.NumberOfStores = storeCount;
				totalSummand.Quantity = setTotal;
				totalSummandHash.Add(totalSummand.Set, totalSummand);
			}

			
			return totalSummandHash;
		}

		private Hashtable TotalByGrade(ArrayList summandList)
		{
			Hashtable totalSummandHash = new Hashtable();
			int storeCount = 0;
			double gradeTotal = 0;
			int currGrade = ((Summand)summandList[0]).Grade;
			Summand totalSummand = null;
			Summand summand = null;
			Summand prevSummand = null;

			for (int s=0;s<summandList.Count;s++)
			{
				prevSummand = summand;
				summand = (Summand)summandList[s];  // for Convenience 
				if (currGrade == summand.Grade)
				{
					if (summand.Eligible)
					{
						storeCount++;
						gradeTotal += summand.Quantity;
					}
				}
				else  // Grade break
				{
					totalSummand = prevSummand.Clone();
					totalSummand.NumberOfStores = storeCount;
					totalSummand.Quantity = gradeTotal;
					totalSummandHash.Add(totalSummand.Grade, totalSummand);
					if (summand.Eligible)
					{
						storeCount = 1;
						gradeTotal = summand.Quantity;
					}
					currGrade = summand.Grade;
				}
			}
		
			// get last GRADEs total
			if (summandList.Count > 0)
			{
				totalSummand = summand.Clone();
				totalSummand.NumberOfStores = storeCount;
				totalSummand.Quantity = gradeTotal;
				totalSummandHash.Add(totalSummand.Grade, totalSummand);
			}

			return totalSummandHash;
		}

		private Hashtable TotalByBoth(ArrayList summandList)
		{
			Hashtable totalSummandHash = new Hashtable();
			int storeCount = 0;
			double setGradeTotal = 0;
			int currSet = ((Summand)summandList[0]).Set;
			int currGrade = ((Summand)summandList[0]).Grade;

			Summand totalSummand = null;
			Summand summand = null;
			Summand prevSummand = null;

			for (int s=0;s<summandList.Count;s++)
			{
				prevSummand = summand;
				summand = (Summand)summandList[s];  // for Convenience 
				if (currSet == summand.Set && currGrade == summand.Grade)
				{
					if (summand.Eligible)
					{
						storeCount++;
						setGradeTotal += summand.Quantity;
					}
				}
				else  // set break
				{
					totalSummand = prevSummand.Clone();
					totalSummand.NumberOfStores = storeCount;
					totalSummand.Quantity = setGradeTotal;
					totalSummandHash.Add(totalSummand.GetSetGradeHash(), totalSummand);
					if (summand.Eligible)
					{
						storeCount = 1;
						setGradeTotal = summand.Quantity;
					}
					currSet = summand.Set;
					currGrade = summand.Grade;
				}
			}
		
			// get last SETs total
			if (summandList.Count > 0)
			{
				totalSummand = summand.Clone();
				totalSummand.NumberOfStores = storeCount;
				totalSummand.Quantity = setGradeTotal;
				totalSummandHash.Add(totalSummand.GetSetGradeHash(), totalSummand);
			}
			
			return totalSummandHash;
		}

		// Begin Track 6418 - stodd - spreading negatives
		override protected bool ExcludeValue(int aIndex)
		{
			// Begin Track 6494 - spread ignoring eligible status
			Summand aSummand = (Summand)_summandList[aIndex];
			if (!aSummand.Eligible)
				return true;
			else if (aSummand.Locked)
				return true;
			else
				return false;
			// End Track 6494 - spread ignoring eligible status
		}

		override protected bool ExcludeValue(int aIndex, bool ignoreLocks)
		{
			// Begin Track 6494 - spread ignoring eligible status
			Summand aSummand = (Summand)_summandList[aIndex];
			if (ignoreLocks)
			{
				if (!aSummand.Eligible)
					return true;
				else
				return false;
			}
			else
			{
				if (!aSummand.Eligible)
				return true;
				else if (aSummand.Locked)
					return true;
				else
					return false;
			}
			// End Track 6494 - spread ignoring eligible status
		}

		public void ExecuteForecastSpread(double aSpreadFromValue, double[] aBasisValueArray, double[] aSpreadToValueArray, int aDecimals)
		{
			ExecutePctContributionSpread(aSpreadFromValue, aBasisValueArray, aSpreadToValueArray, aDecimals);
		}
		// End Track 6418 - stodd - spreading negatives
	}
	#endregion
}
