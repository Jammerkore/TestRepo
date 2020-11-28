using System;
using System.Collections.Generic;
using System.Data; 
using System.Globalization; 

using MIDRetail.Business;
using MIDRetail.Business.Allocation; 
using MIDRetail.Common;
using MIDRetail.DataCommon;
using MIDRetail.Data;

using Logility.ROWebCommon;  
using Logility.ROWebSharedTypes;

namespace Logility.ROWeb
{

    public partial class ROGlobalOptions : ROWebFunction
    {
        private int _iSystemOptionsRID;
        private GlobalOptions_SMTP_BL _SmtpOptions;
        private int _iActivityMsgUpperLimit;
        private GlobalOptionsProfile _GlobalOptionsProfile;
        private ProfileList _StoreProfileList;
        private DataTable _dtHeaderTypes;
        private HeaderCharGroupProfileList _HeaderProfiles;
        private DataTable _dtBasisLabels;
        private DataTable _dtFV; 
        private bool _bUseBatchOnlyMode;
        private bool _bStartWithBatchOnlyModeOn;
        private DataTable _dtStates;  

        public ROGlobalOptions(SessionAddressBlock SAB, ROWebTools ROWebTools)
            : base(SAB, ROWebTools)
        {
        }

        /// <summary>
        /// Cleans up any resources needing to be before exiting.
        /// </summary>
        public override void CleanUp()
        {
            _SmtpOptions = null;
            _GlobalOptionsProfile = null;
            _StoreProfileList.Clear();
            _StoreProfileList = null;
            _HeaderProfiles.Clear();
            _HeaderProfiles = null;
            _dtBasisLabels.Clear();
            _dtBasisLabels = null;
            _dtFV.Clear();
            _dtFV = null;
            _dtStates.Clear();
            _dtStates = null;
        }

        override public ROOut ProcessRequest(ROParms Parms)
        {
            switch (Parms.RORequest)
            {
                case eRORequest.GetGlobalOptions:
                    return GetGlobalOptions();

                case eRORequest.UpdateGlobalOptions:
                    RODataSetParms parms = (RODataSetParms)Parms;
                    return UpdateGlobalDefaults(parms.dsValue);
            }

            return new RONoDataOut(eROReturnCode.Failure, "Invalid Request", ROInstanceID);
        }

        /// <summary>
        /// Get the global defaults
        /// </summary>
        /// <returns>A DataSet containing the global defaults tables</returns>
        private ROOut GetGlobalOptions()
        {
            DataSet dsDefaults = new DataSet("Global Defaults");

            FetchGlobalDefaultsData();
            dsDefaults.Tables.Add(GetStoreAttributesList());
            dsDefaults.Tables.Add(GetCompanyInfo());
            dsDefaults.Tables.Add(GetStatesList());  
            dsDefaults.Tables.Add(GetDisplayOptions());
            dsDefaults.Tables.Add(GetProductDisplayList());
            dsDefaults.Tables.Add(GetStoreDisplayList());
            dsDefaults.Tables.Add(GetAllocationDefaults());
            dsDefaults.Tables.Add(GetItemFWOSMaxValues());
            dsDefaults.Tables.Add(GetForecastVersions());
            dsDefaults.Tables.Add(GetOTSPlanList("OTS History Plans", true));
            dsDefaults.Tables.Add(GetOTSPlanList("OTS Forecast Plans", false));
            dsDefaults.Tables.Add(GetOTSPlanVersions());
            dsDefaults.Tables.Add(GetHeaderTypes());
            dsDefaults.Tables.Add(GetHeaderCharacteristics());
            dsDefaults.Tables.Add(GetHeaders());
            dsDefaults.Tables.Add(GetBasisLabels());
            dsDefaults.Tables.Add(GetBasisLabelOrder());
            dsDefaults.Tables.Add(GetOTSDefaults());
            dsDefaults.Tables.Add(GetSystem());

            LogGlobalDefaultTables(dsDefaults);

            return new RODataSetOut(eROReturnCode.Successful, null, ROInstanceID, dsDefaults);
        }

        private void LogGlobalDefaultTables(DataSet dsGlobalDefaults)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder("The list of global defaults tables:");

            foreach (DataTable dt in dsGlobalDefaults.Tables)
            {
                sb.Append("\n").AppendFormat("Table ({0,-25}), row count is {1}", dt.TableName, dt.Rows.Count);
            }
            sb.Append("\n");
            ROWebTools.LogMessage(eROMessageLevel.Debug, sb.ToString());
        }

        private void FetchGlobalDefaultsData()
        {
            SAB.ApplicationServerSession.Refresh(); 
            _GlobalOptionsProfile = SAB.ApplicationServerSession.GlobalOptions;
            _StoreProfileList = StoreMgmt.StoreGroup_GetListViewList(eStoreGroupSelectType.GlobalOnly, false);
            _HeaderProfiles = SAB.HeaderServerSession.GetHeaderCharGroups();
            _dtHeaderTypes = MIDText.GetTextType(eMIDTextType.eHeaderType, eMIDTextOrderBy.TextValue, 
                                                Convert.ToInt32(eHeaderType.Assortment), Convert.ToInt32(eHeaderType.Placeholder));

            ForecastVersion forVersion = new ForecastVersion(); 

            _dtFV = forVersion.GetForecastVersions(true); 

            GlobalOptions opts = new GlobalOptions();
            DataTable dtIn = opts.GetGlobalOptions();
            DataRow globalOptionsRow = dtIn.Rows[0];

            _SmtpOptions = new GlobalOptions_SMTP_BL();
            _SmtpOptions.LoadFromDataRow(globalOptionsRow);
            _iActivityMsgUpperLimit = Convert.ToInt32(globalOptionsRow["ACTIVITY_MESSAGE_UPPER_LIMIT"]);

            _iSystemOptionsRID = Convert.ToInt32(globalOptionsRow["SYSTEM_OPTION_RID"], CultureInfo.CurrentUICulture);

            _dtBasisLabels = opts.GetBasisLabelInfo(_iSystemOptionsRID);
            _dtStates = opts.GetStateAbbreviations();
        }

        /// Updates the global settings in the database.  Ancillary data tables are ignored.
        /// </summary>
        /// <param name="dsDefaults">the data set containing the global defaults tables</param>
        public ROOut UpdateGlobalDefaults(DataSet dsDefaults)
        {
            GlobalOptions opts = new GlobalOptions();

            try
            {
                UpdateGlobalDefaults(opts, dsDefaults);
            }
            catch (Exception ex)
            {
                //try
                //{
                opts.Rollback();
                //}
                //catch (Exception)
                //{
                //    throw;
                //}

                throw ex;
            }

            finally
            {
                opts.CloseUpdateConnection();
            }

            return new RONoDataOut(eROReturnCode.Successful, null, ROInstanceID);
        }

        private void UpdateGlobalDefaults(GlobalOptions opts, DataSet dsDefaults)
        {
            DataTable dtCompanyInformation = GetDataTableFromDataSet("Company Information", dsDefaults);
            DataTable dtDisplayOptions = GetDataTableFromDataSet("Display Options", dsDefaults);
            DataTable dtAllocationDefaults = GetDataTableFromDataSet("Allocation Defaults", dsDefaults);
            DataTable dtOTSPlanVersions = GetDataTableFromDataSet("OTS Plan Versions", dsDefaults);
            DataTable dtHeaderTypes = GetDataTableFromDataSet("Header Types", dsDefaults);
            DataTable dtHeaders = GetDataTableFromDataSet("Headers", dsDefaults);
            DataTable dtBasisLabelsOrder = GetDataTableFromDataSet("Basis Label Order", dsDefaults);
            DataTable dtOTSDefaults = GetDataTableFromDataSet("OTS Defaults", dsDefaults);
            DataTable dtSystem = GetDataTableFromDataSet("System", dsDefaults);

            ROWebTools.LogMessage(eROMessageLevel.Debug, "Saving Basis Labels Order data.");
            UpdateBasisLabels(opts, dtBasisLabelsOrder);
            ROWebTools.LogMessage(eROMessageLevel.Debug, "Saving OTS Plan Version data.");
            UpdateOTSPlanVersions(dtOTSPlanVersions);

            ROWebTools.LogMessage(eROMessageLevel.Debug, "Transferring Allocation Defaults data.");
            UpdateAllocationDefaults(dtAllocationDefaults);
            ROWebTools.LogMessage(eROMessageLevel.Debug, "Transferring Company data.");
            UpdateCompanyInformation(dtCompanyInformation);
            ROWebTools.LogMessage(eROMessageLevel.Debug, "Transferring Display Options data.");
            UpdateDisplayOptions(dtDisplayOptions);
            ROWebTools.LogMessage(eROMessageLevel.Debug, "Transferring Header data.");
            UpdateHeaders(dtHeaders);
            ROWebTools.LogMessage(eROMessageLevel.Debug, "Transferring OTS Defaults data.");
            UpdateOTSDefaults(dtOTSDefaults);
            ROWebTools.LogMessage(eROMessageLevel.Debug, "Transferring System Defaults data.");
            UpdateSystemDefaults(dtSystem);

            opts.OpenUpdateConnection();
            ROWebTools.LogMessage(eROMessageLevel.Debug, "Saving Header Types data.");
            UpdateHeaderTypes(opts, dtHeaderTypes);
            ROWebTools.LogMessage(eROMessageLevel.Debug, "Saving remaining global options data to DB.");

            SaveGlobalOptionsToDB(opts);
            opts.CommitData();
        }

        private void SaveGlobalOptionsToDB(GlobalOptions opts)
        {
            string sNewStorePeriodBegin = Convert.ToString(_GlobalOptionsProfile.NewStorePeriodBegin, CultureInfo.CurrentUICulture);
            string sNewStorePeriodEnd = Convert.ToString(_GlobalOptionsProfile.NewStorePeriodEnd, CultureInfo.CurrentUICulture);
            string sNonCompStorePeriodBegin = Convert.ToString(_GlobalOptionsProfile.NonCompStorePeriodBegin, CultureInfo.CurrentUICulture);
            string sNonCompStorePeriodEnd = Convert.ToString(_GlobalOptionsProfile.NonCompStorePeriodEnd, CultureInfo.CurrentUICulture);
            int iProductLevelDisplay = Convert.ToInt32(_GlobalOptionsProfile.ProductLevelDisplay, CultureInfo.CurrentUICulture);
            string sPercentNeedLimit = ConvertDoubleToString(_GlobalOptionsProfile.PercentNeedLimit);
            string sBalanceTolerancePercent = ConvertDoubleToString(_GlobalOptionsProfile.BalanceTolerancePercent);
            string sPackSizeErrorPercent = ConvertDoubleToString(_GlobalOptionsProfile.PackSizeErrorPercent);
            string sMaxSizeErrorPercent = ConvertDoubleToString(_GlobalOptionsProfile.MaxSizeErrorPercent);
            string sFillSizeHolesPercent = ConvertDoubleToString(_GlobalOptionsProfile.FillSizeHolesPercent);
            string sGenericPackRounding1stPackPct = ConvertDoubleToString(_GlobalOptionsProfile.GenericPackRounding1stPackPct);
            string sGenericPackRoundingNthPackPct = ConvertDoubleToString(_GlobalOptionsProfile.GenericPackRoundingNthPackPct);
            // BEGIN STUBBED VALUES - MUST FILL IN FROM UI
            eDCFulfillmentSplitOption split_option = eDCFulfillmentSplitOption.DCFulfillment;
            char apply_minimums_ind = '0';
            char prioritize_type = 'H';
            int header_field = Include.NoRID;
            int hcg_rid = Include.NoRID;
            eDCFulfillmentHeadersOrder header_order = eDCFulfillmentHeadersOrder.Ascending;
            eDCFulfillmentStoresOrder store_order = eDCFulfillmentStoresOrder.Ascending;
            eDCFulfillmentSplitByOption split_by_option = eDCFulfillmentSplitByOption.SplitByDC;
            eDCFulfillmentReserve split_by_reserve = eDCFulfillmentReserve.ReservePostSplit;
            eDCFulfillmentMinimums apply_by = eDCFulfillmentMinimums.ApplyByQty;
            eDCFulfillmentWithinDC within_dc = eDCFulfillmentWithinDC.Fill;
            // END STUBBED VALUES

            opts.UpdateGlobalOptions
                    (_GlobalOptionsProfile.CompanyName, 
                     _GlobalOptionsProfile.Street, _GlobalOptionsProfile.City, _GlobalOptionsProfile.State, _GlobalOptionsProfile.Zip, 
                     _GlobalOptionsProfile.Telephone, _GlobalOptionsProfile.Fax, _GlobalOptionsProfile.Email,
                     _GlobalOptionsProfile.PurgeAllocationsPeriod, Convert.ToInt32(_GlobalOptionsProfile.StoreDisplay), 
                     _GlobalOptionsProfile.OTSPlanStoreGroupRID, _GlobalOptionsProfile.AllocationStoreGroupRID,
                     sNewStorePeriodBegin,
                     sNewStorePeriodEnd,
                     sNonCompStorePeriodBegin,
                     sNonCompStorePeriodEnd,
                     iProductLevelDisplay,
                     sPercentNeedLimit,
                     sBalanceTolerancePercent,
                     sPackSizeErrorPercent,
                     sMaxSizeErrorPercent,
                     sFillSizeHolesPercent,
                     sGenericPackRounding1stPackPct,
                     sGenericPackRoundingNthPackPct,
                     _GlobalOptionsProfile.SizeBreakoutInd, _GlobalOptionsProfile.AppConfig.SizeInstalled,
                     _GlobalOptionsProfile.BulkIsDetail, _GlobalOptionsProfile.StoreGradePeriod, 
                     _GlobalOptionsProfile.ProtectInterfacedHeadersInd, _GlobalOptionsProfile.ReserveStoreRID.ToString(),
                     _GlobalOptionsProfile.UseWindowsLogin, _GlobalOptionsProfile.ShippingHorizonWeeks,
                     _GlobalOptionsProfile.ProductLevelDelimiter, _GlobalOptionsProfile.HeaderLinkCharacteristicKey, 
                     _GlobalOptionsProfile.SizeCurveCharMask, _GlobalOptionsProfile.SizeGroupCharMask, 
                     _GlobalOptionsProfile.SizeAlternateCharMask, _GlobalOptionsProfile.SizeConstraintCharMask,
                     _GlobalOptionsProfile.NormalizeSizeCurves,
                     _GlobalOptionsProfile.FillSizesToType, _GlobalOptionsProfile.AllowReleaseIfAllUnitsInReserve, 
                     _GlobalOptionsProfile.NumberOfWeeksWithZeroSales, _GlobalOptionsProfile.MaximumChainWOS, 
                     _GlobalOptionsProfile.ProrateChainStock, _GlobalOptionsProfile.GenericSizeCurveNameType,
                     _GlobalOptionsProfile.GenerateSizeCurveUsing, _GlobalOptionsProfile.PackToleranceNoMaxStep, 
                     _GlobalOptionsProfile.PackToleranceStepped,
                     true, _GlobalOptionsProfile.AllowStoreMaxValueModification, _GlobalOptionsProfile.VSWSizeConstraints,
                     _iActivityMsgUpperLimit, _SmtpOptions,
                     _GlobalOptionsProfile.IsStoreDeleteInProgress, _GlobalOptionsProfile.EnableVelocityGradeOptions,
                     _GlobalOptionsProfile.ForceSingleClientInstance, _GlobalOptionsProfile.ForceSingleUserInstance, 
                     _GlobalOptionsProfile.UseActiveDirectoryAuthentication, _GlobalOptionsProfile.UseActiveDirectoryAuthenticationWithDomain, 
                     _bUseBatchOnlyMode, _bStartWithBatchOnlyModeOn, _GlobalOptionsProfile.VSWItemFWOSMax,
                     _GlobalOptionsProfile.PriorHeaderIncludeReserve, _GlobalOptionsProfile.DCCartonRoundingSGRid,
                     split_option,
                     apply_minimums_ind,
                     prioritize_type,
                     header_field,
                     hcg_rid,
                     header_order,
                     store_order,
                     split_by_option,
                     split_by_reserve,
                     apply_by,
                     within_dc
                    );
        }

        private string ConvertDoubleToString(double val)
        {
            string sReturn = string.Empty;

            if ((val > double.MinValue) && (val < double.MaxValue)) // TT#1156-MD - CTeegarden - refactor code for RowHandler
            {
                sReturn = Convert.ToString(val, CultureInfo.CurrentUICulture);
            }

            return sReturn;
        }

        private DataTable GetDataTableFromDataSet(string sTableName, DataSet ds)
        {
            DataTable dt = ds.Tables[sTableName];

            if ((dt == null) || (dt.Rows.Count < 1))
            {
                string errorMsg = String.Format("Could not find {0}'s table data in update global defaults data set", sTableName);
                throw new Exception(errorMsg);
            }

            return dt;
        }

        private DataTable GetStoreAttributesList()
        {
            DataTable dt = new DataTable("Store Attributes");

            dt.Columns.Add("RID", typeof(int));
            dt.Columns.Add("NAME", typeof(string));

            foreach (Profile profile in _StoreProfileList)
            {
                StoreGroupListViewProfile storeAttrib = profile as StoreGroupListViewProfile;

                if (storeAttrib != null)
                {
                    DataRow dr = dt.NewRow();

                    dr["RID"] = storeAttrib.Key;
                    dr["NAME"] = storeAttrib.Name;

                    dt.Rows.Add(dr);
                }
            }

            return dt;
        }

        private DataTable GetStatesList()
        {
            DataTable dt = new DataTable("States");

            dt.Columns.Add("NAME", typeof(string));
            dt.Columns[0].Caption = "State";

            foreach (DataRow drIn in _dtStates.Rows)
            {
                DataRow dr = dt.NewRow();

                dr["NAME"] = drIn["SP_ABBREVIATION"];
                dt.Rows.Add(dr);
            }

            return dt;
        }

        private DataTable GetAllocationDefaults()
        {
            AllocationRowHandler rowHandler = AllocationRowHandler.GetInstance(_GlobalOptionsProfile);
            DataTable dt = BuildAllocationDefaultsDataTable(rowHandler);

            if (this._GlobalOptionsProfile.Key != Include.NoRID)
                AddAllocationDefaultsData(dt, rowHandler);

            return dt;
        }

        private DataTable GetItemFWOSMaxValues()
        {
            ROWebTools.LogMessage(eROMessageLevel.Debug, "BEGIN GetItemFWOSMaxValues()"); 
            
            string[] typeNames = Enum.GetNames(typeof(eVSWItemFWOSMax));
            Array typeValues = Enum.GetValues(typeof(eVSWItemFWOSMax));
            DataTable dt = new DataTable("Item FWOS Max Values");

            dt.Columns.Add("ID", typeof(int));
            dt.Columns.Add("Name", typeof(string));

            for (int itemIndex = 0;  itemIndex < typeNames.Length; ++ itemIndex)
            {
                DataRow dr = dt.NewRow();

                dr["ID"] = (int)typeValues.GetValue(itemIndex);
                dr["Name"] = typeNames[itemIndex];

                dt.Rows.Add(dr);
            }
            ROWebTools.LogMessage(eROMessageLevel.Debug, "END GetItemFWOSMaxValues()"); 

            return dt;
        }

        private DataTable BuildAllocationDefaultsDataTable(AllocationRowHandler rowHandler)
        {
            DataTable dt = new DataTable("Allocation Defaults");

            rowHandler.AddUITableColumns(dt);

            return dt;
        }

        private void AddAllocationDefaultsData(DataTable dt, AllocationRowHandler rowHandler)
        {
            DataRow dr = dt.NewRow();

            rowHandler.FillUIRow(dr);

            dt.Rows.Add(dr);
        }

        private void UpdateAllocationDefaults(DataTable dtAllocationDefaults)
        {
            DataRow dr = dtAllocationDefaults.Rows[0];

            AllocationRowHandler rowHandler = AllocationRowHandler.GetInstance(_GlobalOptionsProfile);

            rowHandler.ParseUIRow(dr);

        }
    }

    public class AllocationRowHandler : RowHandler
    {
        private static AllocationRowHandler _Instance;

        public static AllocationRowHandler GetInstance(GlobalOptionsProfile GlobalOptionsProfile)
        {
            if (_Instance == null)
            {
                _Instance = new AllocationRowHandler(GlobalOptionsProfile);
            }
            else
            {
                _Instance._GlobalOptionsProfile = GlobalOptionsProfile;
            }

            return _Instance;
        }

        private GlobalOptionsProfile _GlobalOptionsProfile;

        private TypedColumnHandler<double> _NeedLimit = new TypedColumnHandler<double>("Need Limit", eMIDTextCode.Unassigned, false, 0.0);
        private TypedColumnHandler<double> _BalanceTolerance = new TypedColumnHandler<double>("Balance Tolerance", eMIDTextCode.Unassigned, false, 0.0);
        private TypedColumnHandler<int> _StoreGradeTimeframe = new TypedColumnHandler<int>("Store Grade Timeframe", eMIDTextCode.Unassigned, false, 0);
        private TypedColumnHandler<int> _ShippingHorizon = new TypedColumnHandler<int>("Shipping Horizon", eMIDTextCode.Unassigned, false, 0);
        private TypedColumnHandler<int> _ItemFWOSMax = new TypedColumnHandler<int>("Item/FWOS Max", eMIDTextCode.Unassigned, false, (int) eVSWItemFWOSMax.Default);
        private TypedColumnHandler<double> _GenericPackRoundingFirst = new TypedColumnHandler<double>("1st Pack Round up from", eMIDTextCode.Unassigned, false, 0.0);
        private TypedColumnHandler<double> _GenericPackRoundingNth = new TypedColumnHandler<double>("Nth Pack Round up from", eMIDTextCode.Unassigned, false, 0.0);
        private TypedColumnHandler<int> _DCCartonRoundingSGRid = new TypedColumnHandler<int>("DC Carton Rounding Default Attribute", eMIDTextCode.Unassigned, false, 0);
        private TypedColumnHandler<double> _FillSizeHoles = new TypedColumnHandler<double>("Fill Size Holes", eMIDTextCode.Unassigned, false, (int) eFillSizesToType.Holes);
        private TypedColumnHandler<double> _AvgPackDeviationTolerance = new TypedColumnHandler<double>("Average Pack Deviation Tolerance", eMIDTextCode.Unassigned, false, 0.0);
        private TypedColumnHandler<double> _MaxPackNeedTolerance = new TypedColumnHandler<double>("Max Pack Need Tolerance", eMIDTextCode.Unassigned, false, 0.0);
        private TypedColumnHandler<bool> _Stepped = new TypedColumnHandler<bool>("Stepped", eMIDTextCode.Unassigned, false, false);
        private TypedColumnHandler<bool> _NoMaxStep = new TypedColumnHandler<bool>("No Max-Step", eMIDTextCode.Unassigned, false, false);
        private TypedColumnHandler<int> _FillTo = new TypedColumnHandler<int>("Fill To", eMIDTextCode.Unassigned, false, 0);
        private TypedColumnHandler<bool> _NormalizeSizeCurves = new TypedColumnHandler<bool>("Normalize Size Curves", eMIDTextCode.Unassigned, false, false);
        private TypedColumnHandler<int> _GenericSizeCurveName = new TypedColumnHandler<int>("Generic Size Curve Name", eMIDTextCode.Unassigned, false, (int) eGenericSizeCurveNameType.HeaderCharacteristic);
        private TypedColumnHandler<int> _GenerateSizeCurvesUsing = new TypedColumnHandler<int>("Generate Size Curves Using", eMIDTextCode.Unassigned, false, (int) eGenerateSizeCurveUsing.InStockSales);
        private TypedColumnHandler<int> _VSWSizeConstraints = new TypedColumnHandler<int>("VSW Size Constraints", eMIDTextCode.Unassigned, false, (int) eVSWSizeConstraints.None);
        private TypedColumnHandler<bool> _EnableVelocityGradeOptions = new TypedColumnHandler<bool>("Enable Velocity Grade Options", eMIDTextCode.Unassigned, false, false);
        private TypedColumnHandler<bool> _PriorHeaderIncludeReserve = new TypedColumnHandler<bool>("Prior Header Include Reserve", eMIDTextCode.Unassigned, false, false);

        protected AllocationRowHandler(GlobalOptionsProfile GlobalOptionsProfile)
        {
            _GlobalOptionsProfile = GlobalOptionsProfile;

            _aColumnHandlers = new ColumnHandler[] { _NeedLimit, _BalanceTolerance, _StoreGradeTimeframe, _ShippingHorizon,
                                                        _ItemFWOSMax, _GenericPackRoundingFirst, _GenericPackRoundingNth, _DCCartonRoundingSGRid, _FillSizeHoles,
                                                        _AvgPackDeviationTolerance, _MaxPackNeedTolerance, _Stepped, _NoMaxStep, _FillTo,
                                                        _NormalizeSizeCurves, _GenericSizeCurveName, _GenerateSizeCurvesUsing,
                                                        _VSWSizeConstraints, _EnableVelocityGradeOptions, _PriorHeaderIncludeReserve };
        }

        public override void ParseUIRow(DataRow dr)
        {
            _GlobalOptionsProfile.PercentNeedLimit = _NeedLimit.ParseUIColumn(dr);
            _GlobalOptionsProfile.BalanceTolerancePercent = _BalanceTolerance.ParseUIColumn(dr);
            _GlobalOptionsProfile.StoreGradePeriod = _StoreGradeTimeframe.ParseUIColumn(dr);
            _GlobalOptionsProfile.ShippingHorizonWeeks = _ShippingHorizon.ParseUIColumn(dr);
            _GlobalOptionsProfile.VSWItemFWOSMax = (eVSWItemFWOSMax)_ItemFWOSMax.ParseUIColumn(dr);
            _GlobalOptionsProfile.GenericPackRounding1stPackPct = _GenericPackRoundingFirst.ParseUIColumn(dr);
            _GlobalOptionsProfile.GenericPackRoundingNthPackPct = _GenericPackRoundingNth.ParseUIColumn(dr);
            _GlobalOptionsProfile.DCCartonRoundingSGRid = _DCCartonRoundingSGRid.ParseUIColumn(dr);
            _GlobalOptionsProfile.FillSizeHolesPercent = _FillSizeHoles.ParseUIColumn(dr);
            _GlobalOptionsProfile.PackSizeErrorPercent = _AvgPackDeviationTolerance.ParseUIColumn(dr);
            _GlobalOptionsProfile.MaxSizeErrorPercent = _MaxPackNeedTolerance.ParseUIColumn(dr);
            _GlobalOptionsProfile.PackToleranceStepped = _Stepped.ParseUIColumn(dr);
            _GlobalOptionsProfile.PackToleranceNoMaxStep = _NoMaxStep.ParseUIColumn(dr);
            _GlobalOptionsProfile.FillSizesToType = (eFillSizesToType)_FillTo.ParseUIColumn(dr);
            _GlobalOptionsProfile.NormalizeSizeCurves = _NormalizeSizeCurves.ParseUIColumn(dr);
            _GlobalOptionsProfile.GenericSizeCurveNameType = (eGenericSizeCurveNameType)_GenericSizeCurveName.ParseUIColumn(dr);
            _GlobalOptionsProfile.GenerateSizeCurveUsing = (eGenerateSizeCurveUsing)_GenerateSizeCurvesUsing.ParseUIColumn(dr);
            _GlobalOptionsProfile.VSWSizeConstraints = (eVSWSizeConstraints)_VSWSizeConstraints.ParseUIColumn(dr);
            _GlobalOptionsProfile.EnableVelocityGradeOptions = _EnableVelocityGradeOptions.ParseUIColumn(dr);
            _GlobalOptionsProfile.PriorHeaderIncludeReserve = _PriorHeaderIncludeReserve.ParseUIColumn(dr);
        }

        public override void FillUIRow(DataRow dr)
        {
            _NeedLimit.SetUIColumn(dr, _GlobalOptionsProfile.PercentNeedLimit);
            _BalanceTolerance.SetUIColumn(dr, _GlobalOptionsProfile.BalanceTolerancePercent);
            _StoreGradeTimeframe.SetUIColumn(dr, _GlobalOptionsProfile.StoreGradePeriod);
            _ShippingHorizon.SetUIColumn(dr, _GlobalOptionsProfile.ShippingHorizonWeeks);
            _ItemFWOSMax.SetUIColumn(dr, (int) _GlobalOptionsProfile.VSWItemFWOSMax);
            _GenericPackRoundingFirst.SetUIColumn(dr, _GlobalOptionsProfile.GenericPackRounding1stPackPct);
            _GenericPackRoundingNth.SetUIColumn(dr, _GlobalOptionsProfile.GenericPackRoundingNthPackPct);
            _DCCartonRoundingSGRid.SetUIColumn(dr, _GlobalOptionsProfile.DCCartonRoundingSGRid);
            _FillSizeHoles.SetUIColumn(dr, _GlobalOptionsProfile.FillSizeHolesPercent);
            _AvgPackDeviationTolerance.SetUIColumn(dr, _GlobalOptionsProfile.PackSizeErrorPercent);
            _MaxPackNeedTolerance.SetUIColumn(dr, _GlobalOptionsProfile.MaxSizeErrorPercent);
            _Stepped.SetUIColumn(dr, _GlobalOptionsProfile.PackToleranceStepped);
            _NoMaxStep.SetUIColumn(dr, _GlobalOptionsProfile.PackToleranceNoMaxStep);
            _FillTo.SetUIColumn(dr, (int) _GlobalOptionsProfile.FillSizesToType);
            _NormalizeSizeCurves.SetUIColumn(dr, _GlobalOptionsProfile.NormalizeSizeCurves);
            _GenericSizeCurveName.SetUIColumn(dr, (int) _GlobalOptionsProfile.GenericSizeCurveNameType);
            _GenerateSizeCurvesUsing.SetUIColumn(dr, (int) _GlobalOptionsProfile.GenerateSizeCurveUsing);
            _VSWSizeConstraints.SetUIColumn(dr, (int) _GlobalOptionsProfile.VSWSizeConstraints);
            _EnableVelocityGradeOptions.SetUIColumn(dr, _GlobalOptionsProfile.EnableVelocityGradeOptions);
            _PriorHeaderIncludeReserve.SetUIColumn(dr, _GlobalOptionsProfile.PriorHeaderIncludeReserve);
        }
    }

}
