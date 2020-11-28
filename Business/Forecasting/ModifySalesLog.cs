using System;
using System.Collections;
using System.Diagnostics;
using System.Globalization;
using System.Data;
using System.IO;

using MIDRetail.Common;
using MIDRetail.DataCommon;
using MIDRetail.Data;

namespace MIDRetail.Business
{
	/// <summary>
	/// Summary description for ModifySalesLog.
	/// </summary>
	public class ModifySalesLog : MIDLog
	{
		private SessionAddressBlock _sab;
		private int LINE_LENGTH = 170;
		private Hashtable _ruleHash;
		private DataTable _dtRuleLabels;
        //Begin  TT#339 - MD - Modify Forecast audit message - RBeck
            //public ModifySalesLog(SessionAddressBlock aSab, string filePrefix, string filePath, int userRid, string methodName, int methodRid)
            //    : base (filePrefix, filePath, userRid, methodName, methodRid)
        public ModifySalesLog(SessionAddressBlock aSab, string filePrefix, string filePath, int userRid, string methodName, string qualifiedNodeID)
            : base(filePrefix, filePath, userRid, methodName, qualifiedNodeID)
        //End    TT#339 - MD - Modify Forecast audit message - RBeck
		{
			_sab = aSab;

        //TT#753-754 - MD - Log informational message added to audit - RBeck
            string msgText = MIDText.GetText(eMIDTextCode.msg_Modify_Sales_Method_LogInformation);
            msgText = msgText.Replace("{0}", methodName);
            msgText = msgText.Replace("{1}", LogLocation);
            msgText = msgText.Replace("{2}", userRid.ToString());
            msgText = msgText.Replace("{3}", UserName);
            _sab.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, msgText, this.ToString());
        //TT#753-754 - MD - Log informational message added to audit - RBeck

            _dtRuleLabels = MIDText.GetLabels((int) eModifySalesRuleType.None, (int)eModifySalesRuleType.StockToSalesMaximum);
			_ruleHash = new Hashtable();
			foreach (DataRow row in _dtRuleLabels.Rows)
			{
				int key = Convert.ToInt32(row["TEXT_CODE"], CultureInfo.CurrentUICulture);
				string text = row["TEXT_VALUE"].ToString();
				_ruleHash.Add(key, text);
			}

		}

		public void WriteCaptions()
		{
			try
			{
				String rec = new String(' ',LINE_LENGTH);
				//SW.WriteLine(rec);

				rec = rec.Insert(82, "ACTUALS (Basis)");
				rec = rec.Insert(135, "MODIFIED");
				rec = rec.TrimEnd();
				SW.WriteLine(rec);

				rec = new String(' ',LINE_LENGTH);
				rec = rec.Insert(0, "(* = ineligible store)");
				rec = rec.Insert(58, "-------------------------------------------------------------------");
				rec = rec.Insert(126, "----------------------------------------");
				rec = rec.TrimEnd();
				SW.WriteLine(rec);

				rec = new String(' ',LINE_LENGTH);
				rec = rec.Insert(0, "StoreName(RID)");
				rec = rec.Insert(18, "Grade");
				rec = rec.Insert(24, "SellT");
				rec = rec.Insert(31, "Rule");
				rec = rec.Insert(48, "RuleQty");
				rec = rec.Insert(58, "Total");
				rec = rec.Insert(68, "Reg");
				rec = rec.Insert(78, "Promo");
				rec = rec.Insert(88, "Mkdn");
				rec = rec.Insert(98, "Stock");
				rec = rec.Insert(108, "SSRatio");
				rec = rec.Insert(116, "Sell-Thru");
				rec = rec.Insert(126, "Total");
				rec = rec.Insert(136, "Reg");
				rec = rec.Insert(146, "Promo");
				rec = rec.Insert(156, "Mkdn");
				rec = rec.TrimEnd();
				//rec += "\r";
	
				SW.WriteLine(rec);
			}
			catch
			{
				throw;
			}
		}

		public void WriteDetail(
			string storeName,
			int storeRid,
			bool isEligible,
			int gradeBoundary,
			int sellThru,
			eModifySalesRuleType rule,
			double qty,
			double aTotal,
			double aReg,
			double aPromo,
			double aMkdn,
			double aStock,
			double aSSRatio,
			double aSellThru,
			double mTotal,
			double mReg,
			double mPromo,
			double mMkdn
			)
		{
			try
			{
				string elig = string.Empty;
				if (!isEligible)
					elig = "*";

				String ruleText = "Unknown";
				if (this._ruleHash.ContainsKey((int) rule))
				{
					ruleText = (string)_ruleHash[(int)rule];
				}

				string rec = new String(' ',LINE_LENGTH);
				rec = rec.Insert(0,storeName + "(" + storeRid.ToString(CultureInfo.CurrentUICulture) + ")" + elig);
				rec = rec.Insert(18, gradeBoundary.ToString("####0", CultureInfo.CurrentUICulture));
				rec = rec.Insert(24, sellThru.ToString("####0", CultureInfo.CurrentUICulture));
				rec = rec.Insert(31, ruleText);
				rec = rec.Insert(48, qty.ToString("######.##", CultureInfo.CurrentUICulture));
				rec = rec.Insert(58, aTotal.ToString(CultureInfo.CurrentUICulture));
				rec = rec.Insert(68, aReg.ToString(CultureInfo.CurrentUICulture));
				rec = rec.Insert(78, aPromo.ToString(CultureInfo.CurrentUICulture));
				rec = rec.Insert(88, aMkdn.ToString(CultureInfo.CurrentUICulture));
				rec = rec.Insert(98, aStock.ToString(CultureInfo.CurrentUICulture));
				rec = rec.Insert(108, aSSRatio.ToString("####.00", CultureInfo.CurrentUICulture));
				rec = rec.Insert(116, aSellThru.ToString(CultureInfo.CurrentUICulture));
				rec = rec.Insert(126, mTotal.ToString(CultureInfo.CurrentUICulture));
				rec = rec.Insert(136, mReg.ToString(CultureInfo.CurrentUICulture));
				rec = rec.Insert(146, mPromo.ToString(CultureInfo.CurrentUICulture));
				rec = rec.Insert(156, mMkdn.ToString(CultureInfo.CurrentUICulture));
				rec = rec.TrimEnd();
				//rec += "\r";
	
				SW.WriteLine(rec);
			}
			catch
			{
				throw;
			}
		}

		public void WriteFomulas()
		{
			try
			{
				SW.WriteLine("");
				SW.WriteLine("Formulas:");
				SW.WriteLine("•	Sales Modifier – a qty of 1.5 increases the sales by 50%.");
				SW.WriteLine("•	Sales Index – a qty of.80 applies 80% of the average store sales to each store.");
				SW.WriteLine("•	Plug Sales – a qty of 20 applies 20 sales units to each store.");
				SW.WriteLine("•	S/S – apply the specified Stock to Sales Ratio to each store within the cell to modify the Actual Sales History.");  
				SW.WriteLine("    The computation is ((Objective S/S) / (Actual S/S)) * Actual Sales = Modified Sales.");
				SW.WriteLine("•	S/S Index – apply the specified Index to the Average Store S/S to modify the Actual Sales History for each store within the cell.");  
				SW.WriteLine("    The computation is ((Average Store S/S * Index) / Actual S/S)) * Actual Sales = Modified Sales.");
				SW.WriteLine("•	S/S Min – if the Store S/S is less than S/S entered, apply S/S rule.");  
				SW.WriteLine("•	S/S Max – if the Store S/S is more than S/S entered, apply S/S rule.");  
			}
			catch
			{
				throw;
			}
		}

		public void WriteGradesAndSellThru( StoreGradeList gradeList, double [] sellThru)
		{
			try
			{
				SW.WriteLine("");
				int max = gradeList.Count;
				if (sellThru.Length > max)
					max = sellThru.Length;

				string rec = new String(' ',LINE_LENGTH);
				rec = rec.Insert(0, "Grades");
				rec = rec.Insert(40, "Sell Thru");
				rec = rec.TrimEnd();
				SW.WriteLine(rec);

				for (int i=0;i<max;i++)
				{
					rec = new String(' ',LINE_LENGTH);

					if (i < gradeList.Count)
					{
						StoreGradeProfile sgp = (StoreGradeProfile)gradeList[i];
						rec = rec.Insert(3,sgp.StoreGrade + " " + sgp.Boundary.ToString(CultureInfo.CurrentUICulture));
					}
					if (i < sellThru.Length)
					{
						rec = rec.Insert(42,sellThru[i].ToString(CultureInfo.CurrentUICulture));
						if (i==0)
						{
							rec = rec.Insert(52,"( > " + sellThru[i].ToString(CultureInfo.CurrentUICulture) + ")" );
						}
						else
						{
							rec = rec.Insert(52,"( > " + sellThru[i].ToString(CultureInfo.CurrentUICulture) + 
								"  &  <= " +  sellThru[i-1].ToString(CultureInfo.CurrentUICulture));
						}
					}
					rec = rec.TrimEnd();
					SW.WriteLine(rec);
				}
			}
			catch
			{
				throw;
			}
		}

		public void Test()
		{
			WriteDetail(
				"abcsdefghij",
				1234,
				false,
				12345,
				12345,
				eModifySalesRuleType.StockToSalesRatio,
				999999.99,
				999999999,
				999999999,
				999999999,
				999999999,
				999999999,
				1234.99,
				1234.99,
				999999999,
				999999999,
				999999999,
				999999999
				);	
		}


	}
}
