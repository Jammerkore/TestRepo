using System;
using System.Collections;

using MIDRetail.DataCommon;
using MIDRetail.Data;

namespace MIDRetail.Business
{

	/// <summary>
	/// An “average” quantity is computed by dividing the total of the observed values by the 
	/// number of observations.
	/// </summary>
	/// <remarks>
	/// <para>
	/// An “average” quantity is computed by dividing the total of the observed values by the 
	/// number of observations.     
	/// </para><para>
	/// This calculation is critical in the statistical analysis and understanding of data.  
	/// It identifies the symmetrical center in a sample of observed values.  Mathematically, 
	/// it is the expected value of a randomly selected observation from the sample.  It is 
	/// frequently viewed as the typical or “normal” value.
	/// </para><para>
	/// Comparisons to an “average” are used to make decisions and/or statistical inferences 
	/// about the values in a sample.  A comparison measures the degree to which an observed 
	/// value differs from the “average” or expected, typical value.
	/// </para><para>
	/// Some observations or items may be excluded from the calculation of the average.  
	/// The exclusion may be due to a variety of reasons.  The typical but not the only reason 
	/// is that the observation is known to be flawed or inaccurate.    
	/// </para><para>
	/// When the sales history for groups of stores is analyzed, the observations for a store 
	/// will be excluded if the store is “ineligible” to sell the merchandise.  So, all 
	/// ineligible stores are excluded from the calculation of the average store sales regardless 
	/// of whether an ineligible store sold the merchandise or not.  In this case, the sales data 
	/// analysis is for “eligible” stores only; “ineligible” stores are irrelevant to the analysis.
	/// </para>
	/// </remarks>
	public class AverageQuantity: MIDAlgorithm
	{
		int _eligibleSummands;
		double _eligibleTotal;
		double _ineligibleTotal;
		double _averageQuantity;
		ArrayList _summandList;

        // Begin TT#1243 - JSmith - Audit Performance
        //public AverageQuantity()
        //{
        //}
        public AverageQuantity(MIDRetail.Common.Audit aAudit) :
            base(aAudit)
        {
        }
        // End TT#1243

		#region Properties
		/// <summary>
		/// List of observed values
		/// </summary>
		/// <seealso cref="Summand"/>
		public ArrayList SummandList
		{
			get { return _summandList; }
			set { _summandList = value; }
		}

		/// <summary>
		/// Number of eligible values encountered in list
		/// </summary>
		public int EligibleSummands
		{
			get { return _eligibleSummands; }
		}

		/// <summary>
		/// The result of the calculate method:
		/// EligibleTotal / EligibleSummands
		/// </summary>
		public double AvgQuantity
		{
			get { return _averageQuantity; }
		}

		/// <summary>
		/// Total of eligible values encountered in list
		/// </summary>
		public double EligibleTotal
		{
			get { return _eligibleTotal; }
		}

		/// <summary>
		/// Total of ineligible values encountered in list
		/// </summary>
		public double IneligibleTotal
		{
			get { return _ineligibleTotal; }
		}

		#endregion

		/// <summary>
		/// Called to calculate the average quantity,
		/// after setting the SummandList property
		/// </summary>
		/// <remarks>
		/// All negative summand quantities are set to zero.
		/// If there are no eligible summands, a NoEligibleItems()
		/// warning will be issued.
		/// </remarks>
		/// <seealso cref="Summand"/>
		public override void Calculate()
		{
			_eligibleSummands = 0;
			_eligibleTotal = 0.0;
			_ineligibleTotal = 0.0;
			foreach (Summand S in _summandList)
			{
				
				// make sure all summand quantities are positive, or set to zero
				S.Quantity = Math.Max(S.Quantity, 0.0);

				if (S.Eligible)
				{
					_eligibleSummands++;
					_eligibleTotal += S.Quantity; 
				}
				else 
					_ineligibleTotal += S.Quantity; 
			}

			if (_eligibleSummands != 0)
				_averageQuantity = _eligibleTotal / _eligibleSummands;
			else
			{
				_averageQuantity = 0.0;
				//_warnings.Add(new NoEligibleItems());
				_warnings.Add_Msg(eMIDMessageLevel.Warning, MIDText.GetText(eMIDTextCode.msg_NoEligibleItems), this.ToString());

			}
		}

	}
	
}
