using System;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Globalization;

using MIDRetail.DataCommon;


namespace MIDRetail.Data
{

	public partial class StoreGroupRead : DataLayer
	{	


		public StoreGroupRead() : base()
		{

		}

        public StoreGroupRead(string aConnectionString)
			: base(aConnectionString)
		{

		}

	

	}
}
