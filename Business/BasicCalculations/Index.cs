using System;
using System.Collections;



namespace MIDRetail.Business
{
	/// <summary>
	/// An index is a percentage comparison of an observed value to the average.  
	/// So the average itself has an index of 100.
	/// </summary>
	/// <remarks>
	/// This calculation is critical in the statistical analysis and understanding of data.  
	/// It indicates the magnitude of an observed value relative to the average.  It is used 
	/// to grade observations based on their relationship to the average.  Frequently, it is 
	/// used to identify exceptions to the “norm” or average.  It gives mathematical meaning 
	/// to the phrases “below average”, “average” and “above average”.
	/// </remarks>
	/// <seealso cref="AverageQuantity"/>
	public class Index: MIDAlgorithm
	{
		int _eligibleSummands;
		double _eligibleTotal;
		double _ineligibleTotal;
		double _averageQuantity;
		ArrayList _summandList;

       // Begin TT#1243 - JSmith - Audit Performance
        //public Index()
        //{
        //}
        public Index(MIDRetail.Common.Audit aAudit) :
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
		/// A result of the calculate method:
		/// EligibleTotal / EligibleSummands
		/// </summary>
		/// <seealso cref="AverageQuantity"/>
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
		/// First the AverageQuantity of the Summands is calculated.
		/// Then the index of each summand is calculated and placed in the Summand.Result
		/// <seealso cref="AverageQuantity"/>
		/// <seealso cref="Summand"/>
		/// </summary>
		public override void Calculate()
		{
			// first calculate the average quantity
            // Begin TT#1243 - JSmith - Audit Performance
            //AverageQuantity AQ = new AverageQuantity();
            AverageQuantity AQ = new AverageQuantity(Warnings);
            // End TT#1243
			AQ.SummandList = _summandList;
			AQ.Calculate();

			// set some results
			_eligibleSummands = AQ.EligibleSummands;
			_eligibleTotal = AQ.EligibleTotal;
			_ineligibleTotal = AQ.IneligibleTotal;
			_averageQuantity = AQ.AvgQuantity;
			_warnings = AQ.Warnings;

			// calculate index
			foreach (Summand S in _summandList)
				if (S.Eligible && (_averageQuantity > 0.0))
				{
					S.Result = (S.Quantity * 100) / _averageQuantity; 
				}
				else 
					S.Result = 0.0; 
		}

	}
	
}
