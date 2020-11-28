using System;
using System.Collections;

using MIDRetail.DataCommon;
using MIDRetail.Data;

namespace MIDRetail.Business
{

	/// <summary>
	/// Houses sales info for <see cref="PercentSellThrough"/>
	/// </summary>
	public class Sale
	{
		int _salesQuantity;
		int _beginningStock;

		public int SalesQuantity
		{
			get { return _salesQuantity; }
			set { _salesQuantity = value; }
		}
		public int BeginningStock
		{
			get { return _beginningStock; }
			set { _beginningStock = value; }
		}
	}

	/// <summary>
	/// Percent Sell Through is average sales divided by average beginning inventory for a given 
	/// period of time.  It measures inventory utilization. It indicates how adequate 
	/// inventory levels were for the sales volume over the given period.
	/// </summary>
	public class PercentSellThrough: MIDAlgorithm
	{
		ArrayList _salesList;
		double _percentSellThrough;

		// Begin TT#1243 - JSmith - Audit Performance
        //public PercentSellThrough()
        //{
        //}
        public PercentSellThrough(MIDRetail.Common.Audit aAudit) :
            base(aAudit)
        {
        }
        // End TT#1243

		#region Properties
		public ArrayList SalesList
		{
			get { return _salesList; }
			set { _salesList = value; }
		}
		public double PercentSellThru
		{
			get { return _percentSellThrough; }
			set { _percentSellThrough = value; }
		}
		#endregion

		public override void Calculate()
		{
			//			_warnings = new ArrayList();		
			if ((_salesList == null) || (_salesList.Count == 0))
			{
				_percentSellThrough = 0.0;
				//_warnings.Add(new NoPeriodHasPositiveSalesOrBeginningStock());
				_warnings.Add_Msg(eMIDMessageLevel.Warning, MIDText.GetText(eMIDTextCode.msg_NoPerHasPosSalesOrBegStock), this.ToString());
				return;
			}

			// determine percent sell through
			int totalSales = 0;
			int totalBeginningStock = 0;
			foreach (Sale S in _salesList)
			{
				totalSales += Math.Max(S.SalesQuantity, 0);
				totalBeginningStock += Math.Max(S.BeginningStock, 0);
			}

			if (totalBeginningStock == 0)
			{
				if (totalSales == 0)
				{
					//_warnings.Add(new NoPeriodHasPositiveSalesOrBeginningStock());
					_warnings.Add_Msg(eMIDMessageLevel.Warning, MIDText.GetText(eMIDTextCode.msg_NoPerHasPosSalesOrBegStock), this.ToString());
				}
				else
				{
					//_warnings.Add(new AverageBeginningStockIsZero());
					_warnings.Add_Msg(eMIDMessageLevel.Warning, MIDText.GetText(eMIDTextCode.msg_AverageBeginningStockIsZero), this.ToString());
				}
				_percentSellThrough = 0.0;
			}
			else
			{
				_percentSellThrough = (double) totalSales / (double) totalBeginningStock;
			}
		}

	}

	
}
