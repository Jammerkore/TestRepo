using System;
using System.Collections;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace XMLExample
{
	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	class Class1
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static int Main(string[] args)
		{
			try
			{
				XMLWorker worker = new XMLWorker();
				return worker.GenFile(args);
			}
			catch
			{
				return 1;
			}
		}

		public class XMLWorker
		{
			public int GenFile(string[] args)
			{
				try
				{
					ProductAmounts prodAmts = new ProductAmounts();
					XmlSerializer serializer = 	new XmlSerializer(typeof(ProductAmounts));

					prodAmts.Options = new ProductAmountsOptions();
					prodAmts.Options.SalesEndingDate = "2005-03-25";

					ArrayList prodAmtList = new ArrayList();

					AddMerchandise(prodAmtList, "product");


					prodAmts.ProductAmount = new ProductAmountsProductAmount[prodAmtList.Count];
					prodAmtList.CopyTo(0,prodAmts.ProductAmount,0,prodAmtList.Count);

					TextWriter writer = new StreamWriter("C:\\sample.xml");
					serializer.Serialize(writer, prodAmts);
					writer.Close();
					return 0;
				}
				catch
				{
					throw;
				}
			}

			private void AddMerchandise(ArrayList prodAmtList, string aProduct)
			{
				try
				{
					ProductAmountsProductAmount prodAmount = new ProductAmountsProductAmount();
					prodAmount.Product = aProduct;
					prodAmount.Version = "actual";
					prodAmount.Period = ProductAmountsProductAmountPeriod.Week;
					prodAmount.Store = "12345";

					ArrayList varAmtArray = new ArrayList();

					ProductAmountsProductAmountVariableAmount varAmt;

					varAmt = new ProductAmountsProductAmountVariableAmount();
					varAmt.Variable = "Sales";
					varAmt.Amount = 1000;
					varAmtArray.Add(varAmt);

					varAmt = new ProductAmountsProductAmountVariableAmount();
					varAmt.Variable = "Stock";
					varAmt.Amount = 500;
					varAmtArray.Add(varAmt);

					prodAmount.VariableAmount = new ProductAmountsProductAmountVariableAmount[varAmtArray.Count];
					varAmtArray.CopyTo(0,prodAmount.VariableAmount,0,varAmtArray.Count);

					prodAmtList.Add(prodAmount);
				}
				catch
				{
					throw;
				}
			}
		}
	}
}
