using System;
using System.Collections.Generic;
using System.Data; 
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// BEGIN TT#1156-MD - CTeegarden - fill in headers tab
using MIDRetail.Business.Allocation;
using MIDRetail.Common;
// END TT#1156-MD - CTeegarden - fill in headers tab
using MIDRetail.Data;  // TT#1156-MD - CTeegarden - add call to update global defaults

using MIDRetail.DataCommon; 


namespace Logility.ROWeb
{
    public partial class ROGlobalOptions
    {
        private DataTable GetHeaderTypes()
        {
            DataTable dt = new DataTable("Header Types");
            HeaderTypesRowHandler rowHandler = HeaderTypesRowHandler.GetInstance(_GlobalOptionsProfile);

            rowHandler.AddUITableColumns(dt);

            foreach (DataRow drIn in _dtHeaderTypes.Rows) 
            {
                DataRow dr = dt.NewRow();
                rowHandler.TranslateDBRowToUI(drIn, dr);

                dt.Rows.Add(dr);
            }

            return dt;
        }

        private void UpdateHeaderTypes(GlobalOptions opts, DataTable dtHeaderTypes)
        {
            HeaderTypesRowHandler rowHandler = HeaderTypesRowHandler.GetInstance(_GlobalOptionsProfile);

            opts.DeleteHeaderReleaseTypes();
            foreach (DataRow dr in dtHeaderTypes.Rows)
            {
                rowHandler.ParseUIRow(dr);
                rowHandler.Create(opts);
            }
        }

        private DataTable GetHeaderCharacteristics()
        {
            DataTable dt = new DataTable("Header Characteristics");

            dt.Columns.Add("RID", typeof(int));
            dt.Columns.Add("Name", typeof(string));

            foreach (HeaderCharGroupProfile profile in _HeaderProfiles.ArrayList)
            {
                DataRow dr = dt.NewRow();

                dr["RID"] = profile.Key;
                dr["Name"] = profile.ID;

                dt.Rows.Add(dr);
            }

            return dt;
        }

        private DataTable GetHeaders()
        {
            HeadersRowHandler rowHandler = HeadersRowHandler.GetInstance(_GlobalOptionsProfile);
            DataTable dt = BuildHeadersDataTable(rowHandler);

            if (this._GlobalOptionsProfile.Key != Include.NoRID)
            {
                AddHeadersData(rowHandler, dt);
            }

            return dt;
        }

        private DataTable BuildHeadersDataTable(HeadersRowHandler rowHandler)
        {
            DataTable dt = new DataTable("Headers");

            rowHandler.AddUITableColumns(dt);

            return dt;
        }
        private void AddHeadersData(HeadersRowHandler rowHandler, DataTable dt)
        {
            DataRow dr = dt.NewRow();

            rowHandler.FillUIRow(dr);

            dt.Rows.Add(dr);

        }

        private void UpdateHeaders(DataTable dtHeaders)
        {
            HeadersRowHandler rowHandler = HeadersRowHandler.GetInstance(_GlobalOptionsProfile);
            DataRow dr = dtHeaders.Rows[0];

            rowHandler.ParseUIRow(dr);
        }
    }

    public class HeadersRowHandler : RowHandler
    {
        private static HeadersRowHandler _Instance;

        public static HeadersRowHandler GetInstance(GlobalOptionsProfile GlobalOptionsProfile)
        {
            if (_Instance == null)
            {
                _Instance = new HeadersRowHandler(GlobalOptionsProfile);
            }
            else
            {
                _Instance._GlobalOptionsProfile = GlobalOptionsProfile;
            }

            return _Instance;
        }

        private GlobalOptionsProfile _GlobalOptionsProfile;

        private TypedColumnHandler<bool> _ProtectInterfacedHeaders = new TypedColumnHandler<bool>("Protect Interfaced Headers", eMIDTextCode.lbl_ProtectInterfacedHeaders, false, false);
        private TypedColumnHandler<bool> _DoNotReleaseIfAllUnitsInReserve = new TypedColumnHandler<bool>("Do not Release if all units in reserve", eMIDTextCode.Unassigned, false, false);
        private TypedColumnHandler<int> _LinkHeadersWithCharacteristic = new TypedColumnHandler<int>("Link Headers with Characteristic", eMIDTextCode.Unassigned, false, 0);

        protected HeadersRowHandler(GlobalOptionsProfile GlobalOptionsProfile)
        {
            _GlobalOptionsProfile = GlobalOptionsProfile;

            _aColumnHandlers = new ColumnHandler[] { _ProtectInterfacedHeaders, _DoNotReleaseIfAllUnitsInReserve, _LinkHeadersWithCharacteristic };
        }

        public override void ParseUIRow(DataRow dr)
        {
            _GlobalOptionsProfile.ProtectInterfacedHeadersInd = _ProtectInterfacedHeaders.ParseUIColumn(dr);
            _GlobalOptionsProfile.AllowReleaseIfAllUnitsInReserve = !_DoNotReleaseIfAllUnitsInReserve.ParseUIColumn(dr);
            _GlobalOptionsProfile.HeaderLinkCharacteristicKey = _LinkHeadersWithCharacteristic.ParseUIColumn(dr);
        }

        public override void FillUIRow(DataRow dr)
        {
            _ProtectInterfacedHeaders.SetUIColumn(dr, _GlobalOptionsProfile.ProtectInterfacedHeadersInd);
            _DoNotReleaseIfAllUnitsInReserve.SetUIColumn(dr, !_GlobalOptionsProfile.AllowReleaseIfAllUnitsInReserve);
            _LinkHeadersWithCharacteristic.SetUIColumn(dr, _GlobalOptionsProfile.HeaderLinkCharacteristicKey);
        }
    }

    public class HeaderTypesRowHandler : DBRowHandler
    {
        private static HeaderTypesRowHandler _Instance;

        public static HeaderTypesRowHandler GetInstance(GlobalOptionsProfile globalOptionsProfile)
        {
            if (_Instance == null)
            {
                _Instance = new HeaderTypesRowHandler(globalOptionsProfile);
            }
            else
            {
                _Instance.Init(globalOptionsProfile);
            }

            return _Instance;
        }

        HeaderTypeProfileList _headerProfiles;

        private string _sName;
        private bool _bChecked;

        private TypedDBColumnHandler<string> _NameColumnHandler = new TypedDBColumnHandler<string>("TEXT_VALUE", "Name", eMIDTextCode.Unassigned, true, string.Empty);
        private TypedColumnHandler<bool> _CheckedColumnHandler = new TypedColumnHandler<bool>("Checked", eMIDTextCode.Unassigned, false, false);

        protected HeaderTypesRowHandler(GlobalOptionsProfile globalOptionsProfile)
            : base("TEXT_CODE", "ID", eMIDTextCode.Unassigned)
        {
            Init(globalOptionsProfile);
            _aColumnHandlers = new ColumnHandler[] { _RIDColumnHandler, _NameColumnHandler, _CheckedColumnHandler };
        }

        private void Init(GlobalOptionsProfile globalOptionsProfile)
        {
            _headerProfiles = globalOptionsProfile.HeaderTypeProfileList;
        }

        public override void TranslateDBRowToUI(DataRow drDB, DataRow drUI)
        {
            ParseDBRow(drDB);
            _RIDColumnHandler.SetUIColumn(drUI, iRID); 
            _NameColumnHandler.SetUIColumn(drUI, _sName);
            _CheckedColumnHandler.SetUIColumn(drUI, _bChecked);
        }

        public override void ParseUIRow(DataRow dr)
        {
            base.ParseDataRow(dr, false);
            _sName = _NameColumnHandler.ParseUIColumn(dr);
            _bChecked = _CheckedColumnHandler.ParseUIColumn(dr);
        }

        public override void ParseDBRow(DataRow dr)
        {
            base.ParseDataRow(dr, true);
            _sName = _NameColumnHandler.ParseColumn(dr, true);

            HeaderTypeProfile headerProfile = _headerProfiles.FindKey(_iRID) as HeaderTypeProfile;

            _bChecked = headerProfile.ReleaseHeaderType;
        }

        protected override void ParseDataRow(DataRow dr, bool bIsDBRow)
        {
            throw new NotImplementedException();
        }

        public void Create(DataLayer dataLayer) 
        {
            GlobalOptions opts = (GlobalOptions) dataLayer;

            opts.AddHeaderReleaseTypes((eHeaderType)_iRID, _bChecked);
        }
    }
}
