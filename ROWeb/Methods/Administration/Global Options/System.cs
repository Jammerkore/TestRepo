using System;
using System.Collections.Generic;
using System.Data; 
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MIDRetail.Common; 	// TT#1156-MD - CTeegarden - update system tab to use row handler.
using MIDRetail.Data; // TT#1156-MD - CTeegarden - fill in system tab
using MIDRetail.DataCommon; 


namespace Logility.ROWeb
{
    public partial class ROGlobalOptions
    {
        private DataTable GetSystem()
        {
            SystemRowHandler rowHandler = SystemRowHandler.GetInstance(_GlobalOptionsProfile);
            DataTable dt = BuildSystemDataTable(rowHandler);

            if (this._GlobalOptionsProfile.Key != Include.NoRID)
            {
                AddSystemData(rowHandler, dt);
            }

            return dt;
        }

        private DataTable BuildSystemDataTable(SystemRowHandler rowHandler)
        {
            DataTable dt = new DataTable("System");

            rowHandler.AddUITableColumns(dt);

            return dt;
        }

        private void AddSystemData(SystemRowHandler rowHandler, DataTable dt)
        {
            DataRow dr = dt.NewRow();

            rowHandler.FillUIRow(dr);
            dt.Rows.Add(dr);
            _bUseBatchOnlyMode = rowHandler.bUseBatchOnlyMode;
            _bStartWithBatchOnlyModeOn = rowHandler.bStartWithBatchOnlyModeOn;
        }

        private void UpdateSystemDefaults(DataTable dtSystem)
        {
            DataRow dr = dtSystem.Rows[0];

            SystemRowHandler rowHandler = SystemRowHandler.GetInstance(_GlobalOptionsProfile);

            rowHandler.ParseUIRow(dr);
            _bUseBatchOnlyMode = rowHandler.bUseBatchOnlyMode;
            _bStartWithBatchOnlyModeOn = rowHandler.bStartWithBatchOnlyModeOn;
        }
    }

    public class SystemRowHandler : RowHandler
    {
        private static SystemRowHandler _Instance;

        public static SystemRowHandler GetInstance(GlobalOptionsProfile GlobalOptionsProfile)
        {
            if (_Instance == null)
            {
                _Instance = new SystemRowHandler(GlobalOptionsProfile);
            }
            else
            {
                _Instance._GlobalOptionsProfile = GlobalOptionsProfile;
            }

            return _Instance;
        }

        public bool bUseBatchOnlyMode { get { return _bBatchOnlyMode; } }

        public bool bStartWithBatchOnlyModeOn { get { return _bStartWithBatchOnlyModeOn; } }

        private GlobalOptionsProfile _GlobalOptionsProfile;
        private bool _bBatchOnlyMode;
        private bool _bStartWithBatchOnlyModeOn;

        private TypedColumnHandler<bool> _StandardLoginColHandler;
        private TypedColumnHandler<bool> _WindowsLoginColHandler;
        private TypedColumnHandler<bool> _ActiveDirectoryAuthenticationColHandler;
        private TypedColumnHandler<bool> _ActiveDirectoryAuthenticationWithDomainColHandler;
        private TypedColumnHandler<bool> _SingleClientInstanceColHandler;
        private TypedColumnHandler<bool> _SingleUserInstanceColHandler;
        private TypedColumnHandler<bool> _BatchOnlyModeColHandler;
        private TypedColumnHandler<bool> _StartWithBatchOnlyModeOnColHandler;

        protected SystemRowHandler(GlobalOptionsProfile GlobalOptionsProfile)
        {
            _GlobalOptionsProfile = GlobalOptionsProfile;
            _StandardLoginColHandler = 
                new TypedColumnHandler<bool>("Use Standard Authentication", eMIDTextCode.Unassigned, false, true);
            _WindowsLoginColHandler = 
                new TypedColumnHandler<bool>("USE_WINDOWS_AUTH", eMIDTextCode.lbl_RemoteSystemOptions_ckbUseWindowsLogin, false, false);
            _ActiveDirectoryAuthenticationColHandler = 
                new TypedColumnHandler<bool>("Use Active Directory Authentication", eMIDTextCode.Unassigned, false, false);
            _ActiveDirectoryAuthenticationWithDomainColHandler =
                new TypedColumnHandler<bool>("Use AD Authentication w/ Domain", eMIDTextCode.Unassigned, false, false);
            _SingleClientInstanceColHandler =
                new TypedColumnHandler<bool>("ENFORCE_SINGLE_INSTANCE", eMIDTextCode.lbl_RemoteSystemOptions_cbxForceSingleClientInstance, false, false);
            _SingleUserInstanceColHandler =
                new TypedColumnHandler<bool>("ENFORCE_SINGLE_USER_LOGIN", eMIDTextCode.lbl_RemoteSystemOptions_cbxForceSingleUserInstance, false, false);
            _BatchOnlyModeColHandler =
                new TypedColumnHandler<bool>("ENABLE_REMOTE_SYSTEM_OPTIONS", eMIDTextCode.lbl_RemoteSystemOptions_cbxEnableRemoteSystemOptions, false, false);
            _StartWithBatchOnlyModeOnColHandler =
                new TypedColumnHandler<bool>("START_WITH_BATCH_ONLY_MODE_ON", eMIDTextCode.lbl_RemoteSystemOptions_cbxControlServiceDefaultBatchOnlyModeOn, false, false);

            _aColumnHandlers = new ColumnHandler[] { _StandardLoginColHandler, _WindowsLoginColHandler, 
                                                     _ActiveDirectoryAuthenticationColHandler, 
                                                     _ActiveDirectoryAuthenticationWithDomainColHandler,
                                                     _SingleClientInstanceColHandler, _SingleUserInstanceColHandler, 
                                                     _BatchOnlyModeColHandler, _StartWithBatchOnlyModeOnColHandler };
        }

        public override void ParseUIRow(DataRow dr)
        {
            _GlobalOptionsProfile.UseWindowsLogin = _WindowsLoginColHandler.ParseUIColumn(dr);
            _GlobalOptionsProfile.UseActiveDirectoryAuthentication = _ActiveDirectoryAuthenticationColHandler.ParseUIColumn(dr);
            _GlobalOptionsProfile.UseActiveDirectoryAuthenticationWithDomain = _ActiveDirectoryAuthenticationWithDomainColHandler.ParseUIColumn(dr);
            _GlobalOptionsProfile.ForceSingleClientInstance = _SingleClientInstanceColHandler.ParseUIColumn(dr);
            _GlobalOptionsProfile.ForceSingleUserInstance = _SingleUserInstanceColHandler.ParseUIColumn(dr);
            _bBatchOnlyMode = _BatchOnlyModeColHandler.ParseUIColumn(dr);
            _bStartWithBatchOnlyModeOn = _StartWithBatchOnlyModeOnColHandler.ParseUIColumn(dr);
        }

        public override void FillUIRow(DataRow dr)
        {
            bool useStandardAuthentication = !(_GlobalOptionsProfile.UseActiveDirectoryAuthentication ||
                                               _GlobalOptionsProfile.UseActiveDirectoryAuthenticationWithDomain ||
                                               _GlobalOptionsProfile.UseWindowsLogin);
            SecurityAdmin admin = new SecurityAdmin();

            admin.GetControlServiceStartOptions(out _bBatchOnlyMode, out _bStartWithBatchOnlyModeOn);

            _StandardLoginColHandler.SetUIColumn(dr, useStandardAuthentication);
            _WindowsLoginColHandler.SetUIColumn(dr, _GlobalOptionsProfile.UseWindowsLogin);
            _ActiveDirectoryAuthenticationColHandler.SetUIColumn(dr, _GlobalOptionsProfile.UseActiveDirectoryAuthentication);
            _ActiveDirectoryAuthenticationWithDomainColHandler.SetUIColumn(dr, _GlobalOptionsProfile.UseActiveDirectoryAuthenticationWithDomain);
            _SingleClientInstanceColHandler.SetUIColumn(dr, _GlobalOptionsProfile.ForceSingleClientInstance);
            _SingleUserInstanceColHandler.SetUIColumn(dr, _GlobalOptionsProfile.ForceSingleUserInstance);
            _BatchOnlyModeColHandler.SetUIColumn(dr, _bBatchOnlyMode);
            _StartWithBatchOnlyModeOnColHandler.SetUIColumn(dr, _bStartWithBatchOnlyModeOn);
        }
    }
}
