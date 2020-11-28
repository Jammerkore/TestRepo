using System;
using System.Collections.Generic;
using System.Data; 
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MIDRetail.Common;  
using MIDRetail.DataCommon; 


namespace Logility.ROWeb
{
    public partial class ROGlobalOptions
    {
        private DataTable GetOTSDefaults()
        {
            OTSDefaultsRowHandler rowHandler = OTSDefaultsRowHandler.GetInstance(_GlobalOptionsProfile);
            DataTable dt = BuildOTSDefaultsDataTable(rowHandler);

            if (_GlobalOptionsProfile.Key != Include.NoRID)
            {
                AddOTSDefaultsData(rowHandler, dt);
            }

            return dt;
        }

        private DataTable BuildOTSDefaultsDataTable(OTSDefaultsRowHandler rowHandler)
        {
            DataTable dt = new DataTable("OTS Defaults");

            rowHandler.AddUITableColumns(dt);

            return dt;
        }

        private void AddOTSDefaultsData(OTSDefaultsRowHandler rowHandler, DataTable dt)
        {
            DataRow dr = dt.NewRow();

            rowHandler.FillUIRow(dr);

            dt.Rows.Add(dr);
        }

        private void UpdateOTSDefaults(DataTable dtOTSDefaults)
        {
            DataRow dr = dtOTSDefaults.Rows[0];
            OTSDefaultsRowHandler rowHandler = OTSDefaultsRowHandler.GetInstance(_GlobalOptionsProfile);

            rowHandler.ParseUIRow(dr);
        }
    }

    public class OTSDefaultsRowHandler : RowHandler
    {
        private static OTSDefaultsRowHandler _Instance;

        public static OTSDefaultsRowHandler GetInstance(GlobalOptionsProfile GlobalOptionsProfile)
        {
            if (_Instance == null)
            {
                _Instance = new OTSDefaultsRowHandler(GlobalOptionsProfile);
            }
            else
            {
                _Instance._GlobalOptionsProfile = GlobalOptionsProfile;
            }

            return _Instance;
        }

        private GlobalOptionsProfile _GlobalOptionsProfile;

        private TypedColumnHandler<int> _NumberOfWeeksWithZeroSales = new TypedColumnHandler<int>("No. of Weeks with Zero Sales:", eMIDTextCode.Unassigned, false, 0);
        private TypedColumnHandler<int> _MaximumChainWOS = new TypedColumnHandler<int>("Maximum Chain WOS:", eMIDTextCode.Unassigned, false, 0);
        private TypedColumnHandler<bool> _ProrateChainStock = new TypedColumnHandler<bool>("Prorate Chain Stock when zero Chain Sales:", eMIDTextCode.Unassigned, false, false);

        protected OTSDefaultsRowHandler(GlobalOptionsProfile GlobalOptionsProfile)
        {
            _GlobalOptionsProfile = GlobalOptionsProfile;
            _aColumnHandlers = new ColumnHandler[] { _NumberOfWeeksWithZeroSales, _MaximumChainWOS, _ProrateChainStock };
        }

        public override void ParseUIRow(DataRow dr)
        {
            _GlobalOptionsProfile.NumberOfWeeksWithZeroSales = _NumberOfWeeksWithZeroSales.ParseUIColumn(dr);
            _GlobalOptionsProfile.MaximumChainWOS = _MaximumChainWOS.ParseUIColumn(dr);
            _GlobalOptionsProfile.ProrateChainStock = _ProrateChainStock.ParseUIColumn(dr);
        }

        public override void FillUIRow(DataRow dr)
        {
            _NumberOfWeeksWithZeroSales.SetUIColumn(dr, _GlobalOptionsProfile.NumberOfWeeksWithZeroSales);
            _MaximumChainWOS.SetUIColumn(dr, _GlobalOptionsProfile.MaximumChainWOS);
            _ProrateChainStock.SetUIColumn(dr, _GlobalOptionsProfile.ProrateChainStock);
        }
    }

}
