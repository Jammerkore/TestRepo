using System;
using System.Collections;


namespace MIDRetail.Business
{

	/// <summary>
	/// Need is defined as the inventory required to achieve a planned, future on-hand level.
	/// Percent need is a measurement of need relative to plan.
	/// </summary>
	public class NeedAndPercentNeed: MIDAlgorithm
	{
		int _beginningStockQuantity;
		int _unachievedSales;
		int _inTransit;
		int _unitsAllocated;
		int _desiredEndingStock;
		int _need;
		double _percentNeed;

        // Begin TT#1243 - JSmith - Audit Performance
        //public NeedAndPercentNeed()
        //{
        //}
        public NeedAndPercentNeed(MIDRetail.Common.Audit aAudit) :
            base(aAudit)
        {
        }
        // End TT#1243

		#region Properties
		public int BeginningStockQuantity
		{
			get { return _beginningStockQuantity; }
			set { _beginningStockQuantity = value; }
		}
		public int UnachievedSales
		{
			get { return _unachievedSales; }
			set { _unachievedSales = value; }
		}
		public int InTransit
		{
			get { return _inTransit; }
			set { _inTransit = value; }
		}
		public int UnitsAllocated
		{
			get { return _unitsAllocated; }
			set { _unitsAllocated = value; }
		}
		public int DesiredEndingStock
		{
			get { return _desiredEndingStock; }
			set { _desiredEndingStock = value; }
		}
		public int Need
		{
			get { return _need; }
			set { _need = value; }
		}
		public double PercentNeed
		{
			get { return _percentNeed; }
			set { _percentNeed = value; }
		}
		#endregion

		public override void Calculate()
		{

			// if beginning stock is less than zero, treat as zero
			_beginningStockQuantity = Math.Max(_beginningStockQuantity, 0);

			// calculate plan, need and percent need
			int plan = _unachievedSales + _desiredEndingStock;
			_need = plan - (_beginningStockQuantity + _inTransit + _unitsAllocated);

			if (plan == 0)
			{
				_percentNeed = 0.0;
			}
			else
			{
				_percentNeed = (double)(_need * 100) / (double)plan;
			}
		}
	}
	

}
