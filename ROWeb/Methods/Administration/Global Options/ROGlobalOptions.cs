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
                    return UpdateGlobalDefaults((ROGlobalOptionsParms)Parms);
            }

            return new RONoDataOut(eROReturnCode.Failure, "Invalid Request", ROInstanceID);
        }

        /// <summary>
        /// Get the global defaults
        /// </summary>
        /// <returns>An instance of ROGlobalOptionsOut containing the global defaults</returns>
        private ROOut GetGlobalOptions()
        {
            Logility.ROWebSharedTypes.ROGlobalOptions globalOptions = new Logility.ROWebSharedTypes.ROGlobalOptions();
            FetchGlobalDefaultsData();

            globalOptions.CompanyName = _GlobalOptionsProfile.CompanyName;
            globalOptions.UseExternalEligibilityAllocation = _GlobalOptionsProfile.UseExternalEligibilityAllocation;
            globalOptions.UseExternalEligibilityPlanning = _GlobalOptionsProfile.UseExternalEligibilityPlanning;
            globalOptions.ExternalEligibilityProductIdentifier = _GlobalOptionsProfile.ExternalEligibilityProductIdentifier;
            globalOptions.ExternalEligibilityChannelIdentifier = _GlobalOptionsProfile.ExternalEligibilityChannelIdentifier;
            globalOptions.ExternalEligibilityURL = _GlobalOptionsProfile.ExternalEligibilityURL;

            foreach (eExternalEligibilityProductIdentifier identifier in Enum.GetValues(typeof(eExternalEligibilityProductIdentifier)))
            {
                globalOptions.ProductIdentifierList.Add(new KeyValuePair<int, string>(identifier.GetHashCode(), identifier.ToString()));
            }

            foreach (eExternalEligibilityChannelIdentifier identifier in Enum.GetValues(typeof(eExternalEligibilityChannelIdentifier)))
            {
                globalOptions.ChannelIdentifierList.Add(new KeyValuePair<int, string>(identifier.GetHashCode(), identifier.ToString()));
            }

            return new ROGlobalOptionsOut(eROReturnCode.Successful, null, ROInstanceID, globalOptions);
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
        /// <param name="globalOptions">the data object containing the global defaults tables
        /// </param>
        public ROOut UpdateGlobalDefaults(ROGlobalOptionsParms globalOptions)
        {
            GlobalOptions opts = new GlobalOptions();

            try
            {
                UpdateGlobalDefaults(opts, globalOptions.ROGlobalOptions);
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

        private void UpdateGlobalDefaults(GlobalOptions opts, Logility.ROWebSharedTypes.ROGlobalOptions globalOptions)
        {
            _GlobalOptionsProfile.CompanyName = globalOptions.CompanyName;
            _GlobalOptionsProfile.UseExternalEligibilityAllocation = globalOptions.UseExternalEligibilityAllocation;
            _GlobalOptionsProfile.UseExternalEligibilityPlanning = globalOptions.UseExternalEligibilityPlanning;
            _GlobalOptionsProfile.ExternalEligibilityProductIdentifier = globalOptions.ExternalEligibilityProductIdentifier;
            _GlobalOptionsProfile.ExternalEligibilityChannelIdentifier = globalOptions.ExternalEligibilityChannelIdentifier;
            _GlobalOptionsProfile.ExternalEligibilityURL = globalOptions.ExternalEligibilityURL;

            opts.OpenUpdateConnection();

            SaveGlobalOptionsToDB(opts);
            opts.CommitData();
        }

        private void SaveGlobalOptionsToDB(GlobalOptions opts)
        {
            // grabbing current Global options values from the database
            // using _GlobalOptionsProfile for these values can create issues due to
            // some of the defaults being values that you cannot properly compensate for
            DataTable dt = opts.GetGlobalOptions();
            DataRow dr = dt.Rows[0];

            string companyStreet = dr["COMPANY_STREET"] != DBNull.Value ? 
                dr["COMPANY_STREET"].ToString() : null;

            string companyCity = dr["COMPANY_CITY"] != DBNull.Value ? 
                dr["COMPANY_CITY"].ToString() : null;

            string companyState = dr["COMPANY_SP_ABBREVIATION"] != DBNull.Value ? 
                dr["COMPANY_SP_ABBREVIATION"].ToString() : null;

            string companyZip = dr["COMPANY_POSTAL_CODE"] != DBNull.Value ? 
                dr["COMPANY_POSTAL_CODE"].ToString() : null;

            string companyPhone = dr["COMPANY_TELEPHONE"] != DBNull.Value ? 
                dr["COMPANY_TELEPHONE"].ToString() : null;

            string companyFax = dr["COMPANY_FAX"] != DBNull.Value ? 
                dr["COMPANY_FAX"].ToString() : null;

            string companyEmail = dr["COMPANY_EMAIL"] != DBNull.Value ? 
                dr["COMPANY_EMAIL"].ToString() : null;

            int purgeAllocationsPeriod = Convert.ToInt32(dr["PURGE_ALLOCATIONS"], CultureInfo.CurrentUICulture);

            int storeDisplayOptionId = Convert.ToInt32(dr["STORE_DISPLAY_OPTION_ID"], CultureInfo.CurrentUICulture);

            int? defaultOtsSgRid = dr["DEFAULT_OTS_SG_RID"] != DBNull.Value ?
                (int?)Convert.ToInt32(dr["DEFAULT_OTS_SG_RID"], CultureInfo.CurrentUICulture) : null;

            int? defaultAllocSgRid = dr["DEFAULT_ALLOC_SG_RID"] != DBNull.Value ?
                (int?)Convert.ToInt32(dr["DEFAULT_ALLOC_SG_RID"], CultureInfo.CurrentUICulture) : null;

            string newStorePeriodBegin = dr["NEW_STORE_TIMEFRAME_BEGIN"] != DBNull.Value ?
                dr["NEW_STORE_TIMEFRAME_BEGIN"].ToString() : null;

            string newStorePeriodEnd = dr["NEW_STORE_TIMEFRAME_END"] != DBNull.Value ?
                dr["NEW_STORE_TIMEFRAME_END"].ToString() : null;

            string nonCompStorePeriodBegin = dr["NON_COMP_STORE_TIMEFRAME_BEGIN"] != DBNull.Value ?
                dr["NON_COMP_STORE_TIMEFRAME_BEGIN"].ToString() : null;

            string nonCompStorePeriodEnd = dr["NON_COMP_STORE_TIMEFRAME_END"] != DBNull.Value ?
                dr["NON_COMP_STORE_TIMEFRAME_END"].ToString() : null;

            int productLevelDisplayId = Convert.ToInt32(dr["PRODUCT_LEVEL_DISPLAY_ID"], CultureInfo.CurrentUICulture);

            string defaultPercentNeedLimit = dr["DEFAULT_PCT_NEED_LIMIT"] != DBNull.Value ?
                dr["DEFAULT_PCT_NEED_LIMIT"].ToString() : null;
    
            string defaultBalanceTolerance = dr["DEFAULT_BALANCE_TOLERANCE"] != DBNull.Value ?
                dr["DEFAULT_BALANCE_TOLERANCE"].ToString() : null;
    
            string defaultPackSizeErrorPercent = dr["DEFAULT_PACK_SIZE_ERROR_PCT"] != DBNull.Value ?
                dr["DEFAULT_PACK_SIZE_ERROR_PCT"].ToString() : null;
    
            string defaultMaxSizeErrorPercent = dr["DEFAULT_MAX_SIZE_ERROR_PCT"] != DBNull.Value ?
                dr["DEFAULT_MAX_SIZE_ERROR_PCT"].ToString() : null;
    
            string defaultFillSizeHolesPercent = dr["DEFAULT_FILL_SIZE_HOLES_PCT"] != DBNull.Value ?
                dr["DEFAULT_FILL_SIZE_HOLES_PCT"].ToString() : null;

            string genericPackRounding1stPackPct = dr["GENERIC_PACK_ROUNDING_1ST_PACK_PCT"] != DBNull.Value ?
                dr["GENERIC_PACK_ROUNDING_1ST_PACK_PCT"].ToString() : null;
    
            string genericPackRoundingNthPackPct = dr["GENERIC_PACK_ROUNDING_NTH_PACK_PCT"] != DBNull.Value ?
                dr["GENERIC_PACK_ROUNDING_NTH_PACK_PCT"].ToString() : null;

            bool sizeBreakoutInd = Convert.ToChar(dr["SIZE_BREAKOUT_IND"]) == '1';
    
            bool sizeNeedInd = Convert.ToChar(dr["SIZE_NEED_IND"]) == '1';
    
            bool bulkIsDetailInd = Convert.ToChar(dr["BULK_IS_DETAIL_IND"]) == '1';
    
            int storeGradePeriod = Convert.ToInt32(dr["STORE_GRADE_TIMEFRAME"], CultureInfo.CurrentUICulture);
    
            bool protectInterfaceHeadersInd = Convert.ToChar(dr["PROTECT_IF_HDRS_IND"]) == '1';

            string reserveStoreRid = dr["RESERVE_ST_RID"] != DBNull.Value ?
                dr["RESERVE_ST_RID"].ToString() : null;

            bool useWindowsLogin = dr["USE_WINDOWS_LOGIN"] != DBNull.Value ?
                Convert.ToChar(dr["USE_WINDOWS_LOGIN"]) == '1' : false;

            int? shippingHorizonWeeks = dr["SHIPPING_HORIZON_WEEKS"] != DBNull.Value ?
                (int?)Convert.ToInt32(dr["SHIPPING_HORIZON_WEEKS"], CultureInfo.CurrentUICulture) : null;

            char? productLevelDelimiter = dr["PRODUCT_LEVEL_DELIMITER"] != DBNull.Value ? 
                (char?)Convert.ToChar(dr["PRODUCT_LEVEL_DELIMITER"]) : null;

            int? headerLinkCharacteristic = dr["HEADER_LINK_CHARACTERISTIC"] != DBNull.Value ?
                (int?)Convert.ToInt32(dr["HEADER_LINK_CHARACTERISTIC"], CultureInfo.CurrentUICulture) : null;
    
            string sizeCurveCharMask = dr["SIZE_CURVE_CHARMASK"] != DBNull.Value ?
                dr["SIZE_CURVE_CHARMASK"].ToString() : null;
    
            string sizeGroupCharMask = dr["SIZE_GROUP_CHARMASK"] != DBNull.Value ?
                dr["SIZE_GROUP_CHARMASK"].ToString() : null;
    
            string sizeAlternateCharMask = dr["SIZE_ALTERNATE_CHARMASK"] != DBNull.Value ?
                dr["SIZE_ALTERNATE_CHARMASK"].ToString() : null;
    
            string sizeConstraintCharMask = dr["SIZE_CONSTRAINT_CHARMASK"] != DBNull.Value ?
                dr["SIZE_CONSTRAINT_CHARMASK"].ToString() : null;
    
            bool aNormalizeSizeCurves = dr["NORMALIZE_SIZE_CURVES_IND"] != DBNull.Value ? 
                Convert.ToChar(dr["NORMALIZE_SIZE_CURVES_IND"]) == '1' : false;

            int? aFillSizesToType = dr["FILL_SIZES_TO_TYPE"] != DBNull.Value ?
                (int?)Convert.ToInt32(dr["FILL_SIZES_TO_TYPE"], CultureInfo.CurrentUICulture) : null;

            bool aAllowRlseIfAllInReserve = dr["ALLOW_RLSE_IF_ALL_IN_RSRV_IND"] != DBNull.Value ? 
                Convert.ToChar(dr["ALLOW_RLSE_IF_ALL_IN_RSRV_IND"]) == '1' : false;

            int? numberOfWeeksWithZeroSales = dr["NUMBER_OF_WEEKS_WITH_ZERO_SALES"] != DBNull.Value ?
                (int?)Convert.ToInt32(dr["NUMBER_OF_WEEKS_WITH_ZERO_SALES"], CultureInfo.CurrentUICulture) : null;

            int? maximumChainWOS = dr["MAXIMUM_CHAIN_WOS"] != DBNull.Value ?
                (int?)Convert.ToInt32(dr["MAXIMUM_CHAIN_WOS"], CultureInfo.CurrentUICulture) : null;

            bool prorateChainStock = dr["PRORATE_CHAIN_STOCK"] != DBNull.Value ? 
                Convert.ToChar(dr["PRORATE_CHAIN_STOCK"]) == '1' :  false;

            int? aGenericSizeCurveNameType = dr["GENERIC_SIZE_CURVE_NAME_TYPE"] != DBNull.Value ?
                (int?)Convert.ToInt32(dr["GENERIC_SIZE_CURVE_NAME_TYPE"], CultureInfo.CurrentUICulture) : null;

            eGenerateSizeCurveUsing aGenerateSizeCurveUsing = (eGenerateSizeCurveUsing)Convert.ToInt32(dr["GEN_SIZE_CURVE_USING"], CultureInfo.CurrentUICulture);

            bool aPackToleranceNoMaxStep = dr["PACK_TOLERANCE_NO_MAX_STEP_IND"] != DBNull.Value ? 
                Convert.ToChar(dr["PACK_TOLERANCE_NO_MAX_STEP_IND"]) == '1' : false;

            bool aPackToleranceStepped = dr["PACK_TOLERANCE_STEPPED_IND"] != DBNull.Value ? 
                Convert.ToChar(dr["PACK_TOLERANCE_STEPPED_IND"]) == '1' : false;

            bool aRIExpandInd = Convert.ToChar(dr["RI_EXPAND_IND"]) == '1';            

            bool allowStoreMaxValueModification = dr["ALLOW_STORE_MAX_VALUE_MODIFICATION"] != DBNull.Value ?
                Convert.ToBoolean(dr["ALLOW_STORE_MAX_VALUE_MODIFICATION"], CultureInfo.CurrentUICulture) : false;

            int? aVSWSizeConstraints = dr["VSW_SIZE_CONSTRAINTS"] != DBNull.Value ?
                (int?)Convert.ToInt32(dr["VSW_SIZE_CONSTRAINTS"], CultureInfo.CurrentUICulture) : null;

            int? aMyActivityMessageUpperLimit = dr["ACTIVITY_MESSAGE_UPPER_LIMIT"] != DBNull.Value ?
                (int?)Convert.ToInt32(dr["ACTIVITY_MESSAGE_UPPER_LIMIT"], CultureInfo.CurrentUICulture) : null;

            bool storeDeleteInProgress = dr["STORE_DELETE_IN_PROGRESS_IND"] != DBNull.Value ?
                Include.ConvertCharToBool(Convert.ToChar(dr["STORE_DELETE_IN_PROGRESS_IND"])) : false;

            bool enableVelocityGradeOptions = dr["ENABLE_VELOCITY_GRADE_OPTIONS"] != DBNull.Value ? 
                Convert.ToChar(dr["ENABLE_VELOCITY_GRADE_OPTIONS"]) == '1' : false;

            bool forceSingleClientInstance = dr["FORCE_SINGLE_CLIENT_INSTANCE"] != DBNull.Value ? 
                Convert.ToChar(dr["FORCE_SINGLE_CLIENT_INSTANCE"]) == '1' : false;

            bool forceSingleUserInstance = dr["FORCE_SINGLE_USER_INSTANCE"] != DBNull.Value ? 
                Convert.ToChar(dr["FORCE_SINGLE_USER_INSTANCE"]) == '1' : false;

            bool useActiveDirectoryAuthentication = dr["USE_ACTIVE_DIRECTORY_AUTHENTICATION"] != DBNull.Value ? 
                Convert.ToChar(dr["USE_ACTIVE_DIRECTORY_AUTHENTICATION"]) == '1' : false;

            bool useActiveDirectoryAuthenticationWithDomain = dr["USE_ACTIVE_DIRECTORY_AUTHENTICATION_WITH_DOMAIN"] != DBNull.Value ? 
                Convert.ToChar(dr["USE_ACTIVE_DIRECTORY_AUTHENTICATION_WITH_DOMAIN"]) == '1' : false;

            bool useBatchOnlyMode = dr["USE_BATCH_ONLY_MODE"] != DBNull.Value ? 
                Convert.ToChar(dr["USE_BATCH_ONLY_MODE"]) == '1' : false;

            bool controlServiceDefaultBatchOnlyModeOn = dr["CONTROL_SERVICE_DEFAULT_BATCH_ONLY_MODE_ON"] != DBNull.Value ? 
                Convert.ToChar(dr["CONTROL_SERVICE_DEFAULT_BATCH_ONLY_MODE_ON"]) == '1' : false;

            int? aVSWItemFWOSMax = dr["VSW_ITEM_FWOS_MAX_IND"] != DBNull.Value ?
                (int?)Convert.ToInt32(dr["VSW_ITEM_FWOS_MAX_IND"], CultureInfo.CurrentUICulture) : null;

            bool priorHeaderIncludeReserveInd = dr["PRIOR_HEADER_INCLUDE_RESERVE_IND"] != DBNull.Value ? 
                Convert.ToChar(dr["PRIOR_HEADER_INCLUDE_RESERVE_IND"]) == '1' : false;

            int? cartonRoundingSgRid = dr["DC_CARTON_ROUNDING_SG_RID"] != DBNull.Value ?
                (int?)Convert.ToInt32(dr["DC_CARTON_ROUNDING_SG_RID"], CultureInfo.CurrentUICulture) : null;

            int? split_option = dr["SPLIT_OPTION"] != DBNull.Value ?
                (int?)Convert.ToInt32(dr["SPLIT_OPTION"], CultureInfo.CurrentUICulture) : null;

            char? apply_minimums_ind = dr["APPLY_MINIMUMS_IND"] != DBNull.Value ?
                (char?)Convert.ToChar(dr["APPLY_MINIMUMS_IND"]) : null;

            char? prioritize_type = dr["PRIORITIZE_TYPE"] != DBNull.Value ?
                (char?)Convert.ToChar(dr["PRIORITIZE_TYPE"]) : null;

            int? header_field = dr["HEADER_FIELD"] != DBNull.Value ?
                (int?)Convert.ToInt32(dr["HEADER_FIELD"], CultureInfo.CurrentUICulture) : null;

            int? hcg_rid = dr["HCG_RID"] != DBNull.Value ?
                (int?)Convert.ToInt32(dr["HCG_RID"], CultureInfo.CurrentUICulture) : null;

            int? header_order = dr["HEADERS_ORDER"] != DBNull.Value ?
                (int?)Convert.ToInt32(dr["HEADERS_ORDER"], CultureInfo.CurrentUICulture) : null;

            int? store_order = dr["STORES_ORDER"] != DBNull.Value ?
                (int?)Convert.ToInt32(dr["STORES_ORDER"], CultureInfo.CurrentUICulture) : null;

            int? split_by_option = dr["SPLIT_BY_OPTION"] != DBNull.Value ?
                (int?)Convert.ToInt32(dr["SPLIT_BY_OPTION"], CultureInfo.CurrentUICulture) : null;

            int? split_by_reserve = dr["SPLIT_BY_RESERVE"] != DBNull.Value ?
                (int?)Convert.ToInt32(dr["SPLIT_BY_RESERVE"], CultureInfo.CurrentUICulture) : null;

            int? apply_by = dr["APPLY_BY"] != DBNull.Value ?
                (int?)Convert.ToInt32(dr["APPLY_BY"], CultureInfo.CurrentUICulture) : null;

            int? within_dc = dr["WITHIN_DC"] != DBNull.Value ?
                (int?)Convert.ToInt32(dr["WITHIN_DC"], CultureInfo.CurrentUICulture) : null;

            opts.UpdateGlobalOptions(
                _GlobalOptionsProfile.CompanyName,
                companyStreet,
                companyCity,
                companyState,
                companyZip,
                companyPhone,
                companyFax,
                companyEmail,
                purgeAllocationsPeriod,
                storeDisplayOptionId,
                defaultOtsSgRid,
                defaultAllocSgRid,
                newStorePeriodBegin,
                newStorePeriodEnd,
                nonCompStorePeriodBegin,
                nonCompStorePeriodEnd,
                productLevelDisplayId,
                defaultPercentNeedLimit,
                defaultBalanceTolerance,
                defaultPackSizeErrorPercent,
                defaultMaxSizeErrorPercent,
                defaultFillSizeHolesPercent,
                genericPackRounding1stPackPct,
                genericPackRoundingNthPackPct,
                sizeBreakoutInd,
                sizeNeedInd,
                bulkIsDetailInd,
                storeGradePeriod,
                protectInterfaceHeadersInd,
                reserveStoreRid,
                useWindowsLogin,
                shippingHorizonWeeks,
                productLevelDelimiter,
                headerLinkCharacteristic,
                sizeCurveCharMask,
                sizeGroupCharMask,
                sizeAlternateCharMask,
                sizeConstraintCharMask,
                aNormalizeSizeCurves,
                aFillSizesToType,
                aAllowRlseIfAllInReserve,
                numberOfWeeksWithZeroSales,
                maximumChainWOS,
                prorateChainStock,
                aGenericSizeCurveNameType,
                aGenerateSizeCurveUsing,
                aPackToleranceNoMaxStep,
                aPackToleranceStepped,
                aRIExpandInd,
                allowStoreMaxValueModification,
                aVSWSizeConstraints,
                aMyActivityMessageUpperLimit,
                _SmtpOptions,
                storeDeleteInProgress,
                enableVelocityGradeOptions,
                forceSingleClientInstance,
                forceSingleUserInstance,
                useActiveDirectoryAuthentication,
                useActiveDirectoryAuthenticationWithDomain,
                useBatchOnlyMode,
                controlServiceDefaultBatchOnlyModeOn,
                aVSWItemFWOSMax,
                priorHeaderIncludeReserveInd,
                cartonRoundingSgRid,
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
                within_dc,
                _GlobalOptionsProfile.UseExternalEligibilityAllocation,
                _GlobalOptionsProfile.UseExternalEligibilityPlanning,
                _GlobalOptionsProfile.ExternalEligibilityProductIdentifier.GetHashCode(),
                _GlobalOptionsProfile.ExternalEligibilityChannelIdentifier.GetHashCode(),
                _GlobalOptionsProfile.ExternalEligibilityURL);
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
