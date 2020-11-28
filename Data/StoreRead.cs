using System;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Globalization;

using MIDRetail.DataCommon;


namespace MIDRetail.Data
{

	public partial class StoreRead : DataLayer
	{	


		public StoreRead() : base()
		{

		}

        public StoreRead(string aConnectionString)
			: base(aConnectionString)
		{

		}

	

	}
}
