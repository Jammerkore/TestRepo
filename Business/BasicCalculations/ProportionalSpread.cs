using System;
using System.Collections;
using System.Diagnostics;

using MIDRetail.DataCommon;
using MIDRetail.Common;
using MIDRetail.Data;
// removed unecessary comments  // MID Track 5762 (5758) Null Reference 

namespace MIDRetail.Business
{
	/// <summary>
	/// Spread proportionally based on previous values.
	/// </summary>
	/// <remarks>
	/// <para>
	/// A proportional spread reconfigures an old total of quantities to a new total
	/// of new quantities so that each new quantity has the same ratio to the new total
	/// that the corresponding old quantity value had to the old total.  A
	/// proportional spread without locks will not preserve any of the existing
	/// quantity values.  A proportional spread with locks preserves the existing
	/// values of quantities in the old total.
	/// </para><para>
	/// Multiple, minimum and maximum constraints can be placed on a proportional
	/// spread that may cause the resulting ratio of the new quantity to the new
	/// total to be significantly different than the original ratio.  The effects of
	/// constraints on a proportional spread will be cumulative.  That is to say
	/// enforcing a multiple, minimum or maximum for a particular quantity will have
	/// an effect on the results of subsequent quantities.
	/// </para><para>
	/// Mathematically, the best reconfiguration is achieved when the Quantity
	/// Multiple is 1, the Quantity Minimums are all zero (or sufficiently small that
	/// they have no effect) and the Quantity Maximums are sufficiently large (an
	/// example of a sufficiently large value is any number greater than the new
	/// total).
	/// </para><para>
	/// When the Quantity Multiple is greater than 1, the round-off error caused by
	/// rounding each quantity to the nearest multiple of the Quantity Multiple may
	/// cause the new quantity to new total ratio to be significantly greater or
	/// smaller than the original ratio.
	/// </para><para>
	/// When Quantity Minimums are specified, they will be observed except when the
	/// sum of the Quantity Minimums is greater than the New Total.
	/// When Quantity Maximums are specified, they will be observed without
	/// exception.  Consequently, if the new total is greater than the sum of the
	/// Quantity Maximums, the sum of the new quantities will not equal the new total.
	/// </para><para>
	/// When Quantity Maximums are specified, they will be observed without exception.  
	/// Consequently, if the new total is greater than the sum of the Quantity Maximums, 
	/// the sum of the new quantities will not equal the new total.
	/// </para><para>
	/// A proportional spread with locks is a proportional spread that preserves the 
	/// current values of certain (the locked) summands in an old total.  When the sum
	/// of the “locked” summands exceeds the new total, no spread can occur since “locked” 
	/// summands cannot be changed.  When the sum of the “locked” summands does not exceed 
	/// the new total, the new total is reduced by the sum of the “locked” summands and a 
	/// proportional spread without locks is performed on the remaining summands using the 
	/// remains of the new total.  Obviously, if all summands are locked, the proportional 
	/// spread has no effect.
	/// </para>
	/// </remarks>
	public class ProportionalSpread
	{
		ArrayList _summandList;
		double _requestedTotal;
		int _precision;
		double _multiple;
		bool _sortAscending;
        SessionAddressBlock _SAB; // MID Track 5762 (5758) Null Reference When Processing Proportional Allocation Rule
		private bool _allowChainNegatives;	// Track #6271 stodd

        public ProportionalSpread(SessionAddressBlock aSAB) // MID Track 5762 (5758) Null Reference When Processing Proportional Allocation Rule
		{
			_precision = 0;
			_multiple = 1.0;
			_sortAscending = true;
			_SAB = aSAB;  // MID Track 5762 (5758) Null Reference When Processing Proportional Allocation Rule
		}

		#region Properties
		public ArrayList SummandList
		{
			get { return _summandList; }
			set { _summandList = value; }
		}

		public double RequestedTotal
		{
			set { _requestedTotal = value; }
		}
	
		public int Precision
		{
			set { _precision = value; }
		}

		public double Multiple
		{
			set { _multiple = value; }
		}

		public bool SortAscending
		{
			set { _sortAscending = value; }
		}
		// Begin Track #6271
		public bool AllowChainNegatives
		{
			set { _allowChainNegatives = value; }
		}
		// End Track #6271
		#endregion

		// Begin Track #6187 stodd
		public int Calculate()
		{
		// End track #6187
			if (_summandList == null)
			{
				throw new Exception("Error:  SummandList is empty");
			}

			double minTotal = 0.0;
			double maxTotal = 0.0;
			double unlockedSummandTotal = 0.0;
			double lockedSummandTotal = 0.0;
			bool negativeReqTotal = false;
			foreach (Summand S in _summandList)
			{
				if (S.Eligible)
				{
					// make sure all quantities are positive, or set to zero
					S.Quantity = Math.Max(S.Quantity, 0.0);

					// and round min and max to nearest quantity multiple
					S.Min = Math.Round(S.Min/_multiple) * _multiple;
					S.Max = Math.Round(S.Max/_multiple) * _multiple;

					// calcualte the totals
					minTotal += S.Min;
					if (maxTotal < int.MaxValue)
					{
						if (S.Max < int.MaxValue)
						{
							maxTotal += S.Max;
						}
						else
						{
							maxTotal = int.MaxValue;
						}
					}
					if (S.Locked)
						lockedSummandTotal += S.Quantity;
					else
						unlockedSummandTotal += S.Quantity;
				}
			}

			// All stores have zero values so we'll spread the amount evenly to all.
			// to do this, we update each stores Quantity to 1.
			if (unlockedSummandTotal <= 0.0)
			{
				foreach (Summand S in _summandList)
				{
					if (S.Eligible)
					{
						if (!S.Locked)
						{
							S.Quantity = 1;
							unlockedSummandTotal += S.Quantity;
						}
					}
				}
			}

			if (_sortAscending)
				_summandList.Sort(new SummandAscendingComparer());
			else
				_summandList.Sort(new SummandDescendingComparer());

			// finally calcualte the new quantities
			double oldTotal = unlockedSummandTotal;
			// Begin Track #6271
			double requestedTotal = 0.0;
			if (_allowChainNegatives)
			{
				if (_requestedTotal < 0)
				{
					// If locked values are greater than 0, there is no way to spread a negative value to the stores.
					if (lockedSummandTotal > 0)
					{
						requestedTotal = 0.0;
					}
					else
					{
						requestedTotal = _requestedTotal;
						negativeReqTotal = true;
					}
				}
				else
				{
					requestedTotal = Math.Max(_requestedTotal - lockedSummandTotal, 0.0);
				}
			}
			else
			{
				requestedTotal = Math.Max(_requestedTotal - lockedSummandTotal, 0.0);
			}
			// End Track #6271
			double newTotal = requestedTotal;
			double spreadTotal = 0.0;
			bool enforceMin = (minTotal <= requestedTotal);
            //Debug.WriteLine("Total to spread = " + newTotal + " with previous total of " + oldTotal + " and a multiple of " + _multiple);
			foreach (Summand S in _summandList)
			{
				if (S.Eligible)
				{
					if (S.Locked)
					{
						S.Result = S.Quantity;
					}
					else
					{
						if (oldTotal <= 0.0) 
						{
							S.Result = 0.0;
						}
						else
						{
                            S.Result = Math.Round(
								((S.Quantity * newTotal) / (oldTotal * _multiple)), _precision) * _multiple;

                            //Debug.WriteLine("Store RID:" + S.Item + "  Math.Round(((" + S.Quantity + "*" + newTotal + ") / (" + oldTotal + "*" + _multiple +")), " + _precision + ") * " + _multiple + " = " + S.Result);

							if (enforceMin)
								S.Result = Math.Max(S.Result, S.Min);
							if (negativeReqTotal)
							{
								S.Result = Math.Max(S.Result, Math.Min(S.Max, newTotal));
							}
							else
							{
								S.Result = Math.Min(S.Result, Math.Min(S.Max, newTotal));
							}
							S.Result = Math.Round(S.Result, _precision);
						}
						oldTotal -= S.Quantity;
						newTotal -= S.Result;
					}
					spreadTotal += S.Result;
				}
			}
			// any warnings to report?
			//			_warnings = new ArrayList();		// dispose of any previous warnings
			// Begin Track #6187 stodd
			if (maxTotal < _requestedTotal)
			{
                _SAB.ApplicationServerSession.Audit.Add_Msg( // MID Track 5762 (5758) Null Reference When Processing Proportional Allocation Rule
						eMIDMessageLevel.Warning,
						eMIDTextCode.msg_MaxLessThanRequestedTotal,
						MIDText.GetText(eMIDTextCode.msg_MaxLessThanRequestedTotal) + " Max Total= " + maxTotal.ToString() +
							"   Total To Spread= " + _requestedTotal.ToString(),
						this.GetType().Name);
				return (int)eMIDTextCode.msg_MaxLessThanRequestedTotal;
			}
			if (minTotal > _requestedTotal)
			{
                _SAB.ApplicationServerSession.Audit.Add_Msg // MID Track 5762 (5758) Null Reference When Processing Proportional Allocation Rule
						(eMIDMessageLevel.Warning,
						eMIDTextCode.msg_MinimumsBroken,
						MIDText.GetText(eMIDTextCode.msg_MinimumsBroken) + " Min Total= " + minTotal.ToString() +
							"   Total To Spread= " + _requestedTotal.ToString(),
						this.GetType().Name);
				return (int)eMIDTextCode.msg_MinimumsBroken;
			}
			if (Math.Round(spreadTotal - _requestedTotal, _precision) != 0.0)
			{
                _SAB.ApplicationServerSession.Audit.Add_Msg( // MID Track 5762 (5758) Null Reference When Processing Proportional Allocation Rule
						eMIDMessageLevel.Warning,
						eMIDTextCode.msg_SpreadTotNotEqualToReqTot,
						MIDText.GetText(eMIDTextCode.msg_SpreadTotNotEqualToReqTot),
						this.GetType().Name);
				return (int)eMIDTextCode.msg_SpreadTotNotEqualToReqTot;
			}

			return 0;
			// End Track #6187 stodd
		}
		
		public void CopyResultToQuantity()
		{
			if (_summandList == null)
			{
				throw new Exception("Error:  SummandList is empty");
			}
			foreach (Summand S in _summandList)
			{
				S.Quantity = S.Result;
			}
		}
	}
	
}
