using System;
using System.Collections;

using MIDRetail.DataCommon;
using MIDRetail.Data;

namespace MIDRetail.Business
{

	
	//
	//
	//
	public class SalesObservation
	{
		int _salesQuantity;

		public int SalesQuantity
		{
			get { return _salesQuantity; }
			set { _salesQuantity = value; }
		}
	}
	public class WeeksOfSupply: MIDAlgorithm
	{
		ArrayList _salesList;
		double _weeksOfSupply;
		double _weeksPerSalesObservation;
		int _beginningStockQuantity;

        // Begin TT#1243 - JSmith - Audit Performance
        //public WeeksOfSupply()
        //{
        //    _weeksPerSalesObservation = 1.0;
        //}
        public WeeksOfSupply(MIDRetail.Common.Audit aAudit) :
            base(aAudit)
        {
            _weeksPerSalesObservation = 1.0;
        }
        // End TT#1243

		#region Properties
		public ArrayList SalesList
		{
			get { return _salesList; }
			set { _salesList = value; }
		}
		public double Result
		{
			get { return _weeksOfSupply; }
			set { _weeksOfSupply = value; }
		}
		public double WeeksPerSalesObservation
		{
			get { return _weeksPerSalesObservation; }
			set { _weeksPerSalesObservation = value; }
		}
		public int BeginningStockQuantity
		{
			get { return _beginningStockQuantity; }
			set { _beginningStockQuantity = value; }
		}
		#endregion

		public override void Calculate()
		{
			//			_warnings = new ArrayList();		
			if ((_salesList == null) || (_salesList.Count == 0) || (_beginningStockQuantity <= 0.0))
			{
				_weeksOfSupply = 0.0;
				//_warnings.Add(new NoPeriodHasPositiveSalesOrBeginningStock());
				_warnings.Add_Msg(eMIDMessageLevel.Warning, MIDText.GetText(eMIDTextCode.msg_NoPerHasPosSalesOrBegStock), this.ToString());
				return;
			}

			// determine weeks of supply
			double periodsOfSupply = 0.0;
			int totalSales = 0;
			foreach (SalesObservation S in _salesList)
			{
				if (S.SalesQuantity > 0)
				{
					totalSales += S.SalesQuantity;
					_beginningStockQuantity -= S.SalesQuantity;
					if (_beginningStockQuantity > 0)
					{
						periodsOfSupply += 1.0;
					}
					else
					{
						periodsOfSupply += (double)(S.SalesQuantity + _beginningStockQuantity) / (double)S.SalesQuantity;
						break;
					}
				}
				else
				{
					periodsOfSupply += 1.0;
				}
			}
			if (_beginningStockQuantity > 0)
			{
				// have stock left over, so we need to use the average method
				if (totalSales == 0)
				{
					_weeksOfSupply = 0.0;
					//_warnings.Add(new NoPeriodHasPositiveSalesOrBeginningStock());
					_warnings.Add_Msg(eMIDMessageLevel.Warning, MIDText.GetText(eMIDTextCode.msg_NoPerHasPosSalesOrBegStock), this.ToString());
					return;
				}
				periodsOfSupply = (double)(_beginningStockQuantity * _salesList.Count) / (double)totalSales;
			}
			_weeksOfSupply = periodsOfSupply * _weeksPerSalesObservation; 
		}
	}



}
