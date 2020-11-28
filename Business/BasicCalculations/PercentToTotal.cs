using System;
using System.Collections;

namespace MIDRetail.Business
{

	/// <summary>
	/// Percent to Total is a standard measurement that equates partitions of the 
	/// whole to comparable parts of a hundred.  
	/// </summary>
	public class PercentToTotal: MIDAlgorithm
	{
		ArrayList _summandList;

        // Begin TT#1243 - JSmith - Audit Performance
        //public PercentToTotal()
        //{
        //}
        public PercentToTotal(MIDRetail.Common.Audit aAudit) :
            base(aAudit)
        {
        }
        // End TT#1243

		#region Properties
		public ArrayList SummandList
		{
			get { return _summandList; }
			set { _summandList = value; }
		}
		#endregion

		public override void Calculate()
		{
			// make sure all quantities are positive, or set to zero
			double sum = 0;
			foreach (Summand S in _summandList)
			{
				S.Quantity = Math.Max(S.Quantity, 0.0);
				sum += S.Quantity;
			}

			// calcualte Percent to Total
			foreach (Summand S in _summandList)
				if (sum != 0.0)
					S.Result = (100.0 * S.Quantity) / sum; 
				else 
					S.Result = 0.0;
		}

	}


}
