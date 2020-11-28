using System;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Globalization;

using MIDRetail.DataCommon;


namespace MIDRetail.Data
{

	public partial class StoreCharRead : DataLayer
	{	


		public StoreCharRead() : base()
		{

		}

        public StoreCharRead(string aConnectionString)
			: base(aConnectionString)
		{

		}

        // Begin TT#2131-MD - JSmith - Halo Integration
        public DataTable GetAllStoreCharacteristicValues()
        {
            try
            {

                return StoredProcedures.MID_STORE_CHAR_READ_ALL.Read(_dba
                                                         );

            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }
        // End TT#2131-MD - JSmith - Halo Integration

    }
}
