using System;
using System.Data;
using System.Collections;
using System.Globalization;

using MIDRetail.DataCommon;


namespace MIDRetail.Data
{

	public partial class GlobalOptions : DataLayer
	{
		public GlobalOptions() : base()
		{

		}

		public GlobalOptions(string aConnectionString) : base(aConnectionString)
		{

		}

		public DataTable GetApplicationInfo()
		{
			try
			{
                return StoredProcedures.MID_APPLICATION_VERSION_READ_ALL.Read(_dba);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

        public DataTable GetBasisLabelInfo(int aSystemOptionRID)
		{
			try
			{
                // Load Variables
                return StoredProcedures.MID_SYSTEM_OPTIONS_BASIS_LABELS_READ.Read(_dba, SYSTEM_OPTION_RID: aSystemOptionRID);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

        // BEGIN TT#1966-MD - AGallagher - DC Fulfillment
        public DataTable GetDCFStoreOrderInfo(int aSystemOptionRID)
        {
            try
            {
                // Load Variables
                return StoredProcedures.MID_SYSTEM_OPTIONS_DC_FULFILLMENT_STORE_ORDER_READ.Read(_dba, SYSTEM_OPTION_RID: aSystemOptionRID);
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }
        // END TT#1966-MD - AGallagher - DC Fulfillment
        public void GetBasisLabelInfo_Insert(int systemOptionRID, int labelType, int labelSequence)
        {
            try
            {
                OpenUpdateConnection();

                StoredProcedures.MID_SYSTEM_OPTIONS_BASIS_LABELS_INSERT.Insert(_dba,
                                                                               SYSTEM_OPTION_RID: systemOptionRID,
                                                                               LABEL_TYPE: labelType,
                                                                               LABEL_SEQ: labelSequence
                                                                               );
                CommitData();
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
            finally
            {
                if (ConnectionIsOpen)
                {
                    _dba.CloseUpdateConnection();
                }
            }
        }

        // BEGIN TT#1966-MD - AGallagher - DC Fulfillment
        public void GetDCFStoreOrderInfo_Insert(int systemOptionRID, int seq, string distCenter, int scgRID)
        {
            try
            {
                OpenUpdateConnection();

                StoredProcedures.MID_SYSTEM_OPTIONS_DC_FULFILLMENT_STORE_ORDER_INSERT.Insert(_dba,
                                                                               SYSTEM_OPTION_RID: systemOptionRID,
                                                                               SEQ : seq,
                                                                               DIST_CENTER : distCenter,
                                                                               SCG_RID : scgRID
                                                                               );
                CommitData();
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
            finally
            {
                if (ConnectionIsOpen)
                {
                    _dba.CloseUpdateConnection();
                }
            }
        }
        // END TT#1966-MD - AGallagher - DC Fulfillment

        public void GetBasisLabelInfo_Delete(int systemOptionRID, int labelType)
        {
            try
            {
                OpenUpdateConnection();

                //---Delete Old SYSTEM_OPTION_RID Records-----

                StoredProcedures.MID_SYSTEM_OPTIONS_BASIS_LABELS_DELETE.Delete(_dba,
                                                                               SYSTEM_OPTION_RID: systemOptionRID,
                                                                               LABEL_TYPE: labelType
                                                                               );
                CommitData();
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
            finally
            {
                if (ConnectionIsOpen)
                {
                    _dba.CloseUpdateConnection();
                }
            }
        }

        // BEGIN TT#1966-MD - AGallagher - DC Fulfillment
        public void GetDCFStoreOrderInfo_Delete(int systemOptionRID, int seq)
        {
            try
            {
                OpenUpdateConnection();

                //---Delete Old SYSTEM_OPTION_RID Records-----

                StoredProcedures.MID_SYSTEM_OPTIONS_DC_FULFILLMENT_STORE_ORDER_DELETE.Delete(_dba,
                                                                               SYSTEM_OPTION_RID: systemOptionRID,
                                                                               SEQ: seq
                                                                               );
                CommitData();
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
            finally
            {
                if (ConnectionIsOpen)
                {
                    _dba.CloseUpdateConnection();
                }
            }
        }
        public void GetDCFStoreOrderInfo_Delete_All(int systemOptionRID)
        {
            try
            {
                OpenUpdateConnection();

                //---Delete all DCF SYSTEM_OPTION_RID Records-----

                StoredProcedures.MID_SYSTEM_OPTIONS_DC_FULFILLMENT_STORE_ORDER_DELETE_ALL.Delete(_dba
                                                                               );
  
                CommitData();
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
            finally
            {
                if (ConnectionIsOpen)
                {
                    _dba.CloseUpdateConnection();
                }
            }
        }
        // END TT#1966-MD - AGallagher - DC Fulfillment
		public DataTable GetGlobalOptions()
		{
			try
			{
                //MID Track # 2354 - removed nolock because it causes concurrency issues
                return StoredProcedures.MID_SYSTEM_OPTIONS_READ_ALL.Read(_dba);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

        //Begin TT#506-MD -jsobek -Add infrastructure to allow Email to be sent from the application
        public DataTable GetSystemEmail()
        {
            try
            {
                return StoredProcedures.MID_SYSTEM_EMAIL_READ_ALL.Read(_dba);
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }
        //End TT#506-MD -jsobek -Add infrastructure to allow Email to be sent from the application

     
        public bool UpdateGlobalOptions(
            string companyName, 
            string companyStreet, 
            string companyCity, 
            string companyState, 
            string companyZip,
            string companyPhone, 
            string companyFax, 
            string companyEmail,
            int purgeAllocationsPeriod, 
            int storeDisplayOptionId, 
            int defaultOtsSgRid, 
            int defaultAllocSgRid,
            string newStorePeriodBegin, 
            string newStorePeriodEnd, 
            string nonCompStorePeriodBegin, 
            string nonCompStorePeriodEnd, 
            int productLevelDisplayId,
            string defaultPercentNeedLimit, 
            string defaultBalanceTolerance, 
            string defaultPackSizeErrorPercent,
            string defaultMaxSizeErrorPercent, 
            string defaultFillSizeHolesPercent,
            string genericPackRounding1stPackPct, 
            string genericPackRoundingNthPackPct,  
            bool sizeBreakoutInd, 
            bool sizeNeedInd, 
            bool bulkIsDetailInd, 
            int storeGradePeriod, 
            bool protectInterfaceHeadersInd, 
            string reserveStoreRid,
            bool useWindowsLogin, 
            int shippingHorizonWeeks, 
            char productLevelDelimiter, 
            int headerLinkCharacteristic,
            string sizeCurveCharMask, 
            string sizeGroupCharMask, 
            string sizeAlternateCharMask,
            string sizeConstraintCharMask, 
            bool aNormalizeSizeCurves, 
            eFillSizesToType aFillSizesToType, 
            bool aAllowRlseIfAllInReserve,  
            int numberOfWeeksWithZeroSales,
            int maximumChainWOS,
            bool prorateChainStock, 
            eGenericSizeCurveNameType aGenericSizeCurveNameType,
            eGenerateSizeCurveUsing aGenerateSizeCurveUsing, 
            bool aPackToleranceNoMaxStep,
            bool aPackToleranceStepped,
            bool aRIExpandInd,  
            bool allowStoreMaxValueModification,
            eVSWSizeConstraints aVSWSizeConstraints, 
            int aMyActivityMessageUpperLimit,
            GlobalOptions_SMTP_BL SMTP_Options,
            bool storeDeleteInProgress,
            bool enableVelocityGradeOptions,
            bool forceSingleClientInstance,
            bool forceSingleUserInstance,
            bool useActiveDirectoryAuthentication,
            bool useActiveDirectoryAuthenticationWithDomain,
            bool useBatchOnlyMode,
            bool controlServiceDefaultBatchOnlyModeOn,
            eVSWItemFWOSMax aVSWItemFWOSMax,   // TT#933-MD - AGallagher - Item Max vs. FWOS Max
			bool priorHeaderIncludeReserveInd,   // TT#1609-MD - stodd - data layer change - prior header
			int cartonRoundingSgRid,		// TT#1652-MD - stodd - DC Carton Rounding
            eDCFulfillmentSplitOption split_option,               // TT#1966-MD - AGallagher - DC Fulfillment
            char apply_minimums_ind,        // TT#1966-MD - AGallagher - DC Fulfillment
            char prioritize_type,           // TT#1966-MD - AGallagher - DC Fulfillment
            int header_field,               // TT#1966-MD - AGallagher - DC Fulfillment
            int hcg_rid,                    // TT#1966-MD - AGallagher - DC Fulfillment
            eDCFulfillmentHeadersOrder header_order,              // TT#1966-MD - AGallagher - DC Fulfillment
            eDCFulfillmentStoresOrder store_order,                // TT#1966-MD - AGallagher - DC Fulfillment
            eDCFulfillmentSplitByOption split_by_option,          // TT#1966-MD - AGallagher - DC Fulfillment
            eDCFulfillmentReserve split_by_reserve,               // TT#1966-MD - AGallagher - DC Fulfillment
            eDCFulfillmentMinimums apply_by,                      // TT#1966-MD - AGallagher - DC Fulfillment
            eDCFulfillmentWithinDC within_dc,                     // TT#1966-MD - AGallagher - DC Fulfillment
            bool useExternalEligibilityAllocation,
            bool useExternalEligibilityPlanning,
            eExternalEligibilityProductIdentifier externalEligibilityProductIdentifier,
            eExternalEligibilityChannelIdentifier externalEligibilityChannelIdentifier,
            string externalEligibilityURL
            )
        {
            try
            {
                int DO_UPDATE_RESERVE_STORE = 0;
                int? RESERVE_ST_RID = null;
                if (reserveStoreRid.Length > 0)
                {
                    DO_UPDATE_RESERVE_STORE = 1;
                    RESERVE_ST_RID = Convert.ToInt32(reserveStoreRid, CultureInfo.CurrentUICulture);
                }

                char SIZE_BREAKOUT_IND;
                if (sizeBreakoutInd)
                    SIZE_BREAKOUT_IND = '1';
                else
                    SIZE_BREAKOUT_IND = '0';

                char SIZE_NEED_IND;
                if (sizeNeedInd)
                    SIZE_NEED_IND = '1';
                else
                    SIZE_NEED_IND = '0';

                char BULK_IS_DETAIL_IND;
                if (bulkIsDetailInd)
                    BULK_IS_DETAIL_IND = '1';
                else
                    BULK_IS_DETAIL_IND = '0';

                char PROTECT_IF_HDRS_IND;
                if (protectInterfaceHeadersInd)
                    PROTECT_IF_HDRS_IND = '1';
                else
                    PROTECT_IF_HDRS_IND = '0';

                char USE_WINDOWS_LOGIN;
                if (useWindowsLogin)
                    USE_WINDOWS_LOGIN = '1';
                else
                    USE_WINDOWS_LOGIN = '0';

                char PRORATE_CHAIN_STOCK;
                if (prorateChainStock)
                    PRORATE_CHAIN_STOCK = '1';
                else
                    PRORATE_CHAIN_STOCK = '0';

                int? NEW_STORE_TIMEFRAME_BEGIN = null;
                if (newStorePeriodBegin.Length != 0) NEW_STORE_TIMEFRAME_BEGIN = Convert.ToInt32(newStorePeriodBegin, CultureInfo.CurrentUICulture);

                int? NEW_STORE_TIMEFRAME_END = null;
                if (newStorePeriodEnd.Length != 0) NEW_STORE_TIMEFRAME_END = Convert.ToInt32(newStorePeriodEnd, CultureInfo.CurrentUICulture);

                int? NON_COMP_STORE_TIMEFRAME_BEGIN = null;
                if (nonCompStorePeriodBegin.Length != 0) NON_COMP_STORE_TIMEFRAME_BEGIN = Convert.ToInt32(nonCompStorePeriodBegin, CultureInfo.CurrentUICulture);

                int? NON_COMP_STORE_TIMEFRAME_END = null;
                if (nonCompStorePeriodEnd.Length != 0) NON_COMP_STORE_TIMEFRAME_END = Convert.ToInt32(nonCompStorePeriodEnd, CultureInfo.CurrentUICulture);

                double? DEFAULT_PCT_NEED_LIMIT = null;
                if (defaultPercentNeedLimit.Length != 0) DEFAULT_PCT_NEED_LIMIT = Convert.ToDouble(defaultPercentNeedLimit, CultureInfo.CurrentUICulture);

                double? DEFAULT_BALANCE_TOLERANCE = null;
                if (defaultBalanceTolerance.Length != 0) DEFAULT_BALANCE_TOLERANCE = Convert.ToDouble(defaultBalanceTolerance, CultureInfo.CurrentUICulture);

                double? DEFAULT_PACK_SIZE_ERROR_PCT = null;
                if (defaultPackSizeErrorPercent.Length != 0) DEFAULT_PACK_SIZE_ERROR_PCT = Convert.ToDouble(defaultPackSizeErrorPercent, CultureInfo.CurrentUICulture);

                double? DEFAULT_MAX_SIZE_ERROR_PCT = null;
                if (defaultMaxSizeErrorPercent.Length != 0) DEFAULT_MAX_SIZE_ERROR_PCT = Convert.ToDouble(defaultMaxSizeErrorPercent, CultureInfo.CurrentUICulture);

                double? DEFAULT_FILL_SIZE_HOLES_PCT = null;
                if (defaultFillSizeHolesPercent.Length != 0) DEFAULT_FILL_SIZE_HOLES_PCT = Convert.ToDouble(defaultFillSizeHolesPercent, CultureInfo.CurrentUICulture);

                double? GENERIC_PACK_ROUNDING_1ST_PACK_PCT = null;
                if (genericPackRounding1stPackPct.Length != 0) GENERIC_PACK_ROUNDING_1ST_PACK_PCT = Convert.ToDouble(genericPackRounding1stPackPct, CultureInfo.CurrentUICulture);

                double? GENERIC_PACK_ROUNDING_NTH_PACK_PCT = null;
                if (genericPackRoundingNthPackPct.Length != 0) GENERIC_PACK_ROUNDING_NTH_PACK_PCT = Convert.ToDouble(genericPackRoundingNthPackPct, CultureInfo.CurrentUICulture);

                string SIZE_CURVE_CHARMASK = Include.NullForStringValue;  //TT#1310-MD -jsobek -Error when adding a new Store
                if (sizeCurveCharMask.Trim() != string.Empty) SIZE_CURVE_CHARMASK = sizeCurveCharMask;

                string SIZE_GROUP_CHARMASK = Include.NullForStringValue;  //TT#1310-MD -jsobek -Error when adding a new Store
                if (sizeGroupCharMask.Trim() != string.Empty) SIZE_GROUP_CHARMASK = sizeGroupCharMask;

                string SIZE_ALTERNATE_CHARMASK = Include.NullForStringValue;  //TT#1310-MD -jsobek -Error when adding a new Store
                if (sizeAlternateCharMask.Trim() != string.Empty) SIZE_ALTERNATE_CHARMASK = sizeAlternateCharMask;

                string SIZE_CONSTRAINT_CHARMASK = Include.NullForStringValue;  //TT#1310-MD -jsobek -Error when adding a new Store
                if (sizeConstraintCharMask.Trim() != string.Empty) SIZE_CONSTRAINT_CHARMASK = sizeConstraintCharMask;

                int ALLOW_STORE_MAX_VALUE_MODIFICATION = 0;
                if (allowStoreMaxValueModification) ALLOW_STORE_MAX_VALUE_MODIFICATION = 1;

                //Begin TT#855-MD -jsobek -Velocity Enhancements
                char ENABLE_VELOCITY_GRADE_OPTIONS;
                if (enableVelocityGradeOptions)
                    ENABLE_VELOCITY_GRADE_OPTIONS = '1';
                else
                    ENABLE_VELOCITY_GRADE_OPTIONS = '0';
                //End TT#855-MD -jsobek -Velocity Enhancements

                char FORCE_SINGLE_CLIENT_INSTANCE;
                if (forceSingleClientInstance)
                    FORCE_SINGLE_CLIENT_INSTANCE = '1';
                else
                    FORCE_SINGLE_CLIENT_INSTANCE = '0';

                char FORCE_SINGLE_USER_INSTANCE;
                if (forceSingleUserInstance)
                    FORCE_SINGLE_USER_INSTANCE = '1';
                else
                    FORCE_SINGLE_USER_INSTANCE = '0';

                char USE_ACTIVE_DIRECTORY_AUTHENTICATION;
                if (useActiveDirectoryAuthentication)
                    USE_ACTIVE_DIRECTORY_AUTHENTICATION = '1';
                else
                    USE_ACTIVE_DIRECTORY_AUTHENTICATION = '0';

                char USE_ACTIVE_DIRECTORY_AUTHENTICATION_WITH_DOMAIN;
                if (useActiveDirectoryAuthenticationWithDomain)
                    USE_ACTIVE_DIRECTORY_AUTHENTICATION_WITH_DOMAIN = '1';
                else
                    USE_ACTIVE_DIRECTORY_AUTHENTICATION_WITH_DOMAIN = '0';

                char USE_BATCH_ONLY_MODE;
                if (useBatchOnlyMode)
                    USE_BATCH_ONLY_MODE = '1';
                else
                    USE_BATCH_ONLY_MODE = '0';

                char CONTROL_SERVICE_DEFAULT_BATCH_ONLY_MODE_ON;
                if (controlServiceDefaultBatchOnlyModeOn)
                    CONTROL_SERVICE_DEFAULT_BATCH_ONLY_MODE_ON = '1';
                else
                    CONTROL_SERVICE_DEFAULT_BATCH_ONLY_MODE_ON = '0';

                int VSW_ITEM_FWOS_MAX_IND = aVSWItemFWOSMax.GetHashCode(); //TT#933-MD - AGallagher - Item Max vs. FWOS Max

                // Begin TT#1609-MD - stodd - data layer change - prior header
                char PRIOR_HEADER_INCLUDE_RESERVE_IND;
                if (priorHeaderIncludeReserveInd)
                    PRIOR_HEADER_INCLUDE_RESERVE_IND = '1';
                else
                    PRIOR_HEADER_INCLUDE_RESERVE_IND = '0';
                // End TT#1609-MD - stodd - data layer change - prior header
				
				
                // Begin TT#1689-MD - stodd - DC Carton Rounding Default Attribute should be blank if not selected 
                //int DC_CARTON_ROUNDING_SG_RID = cartonRoundingSgRid;	// TT#1652-MD - stodd - DC Carton Rounding
                int? DC_CARTON_ROUNDING_SG_RID = null;
                if (cartonRoundingSgRid > 0)
                {
                    DC_CARTON_ROUNDING_SG_RID = cartonRoundingSgRid;
                }
                // End TT#1689-MD - stodd - DC Carton Rounding Default Attribute should be blank if not selected

                // BEGIN TT#1966-MD - AGallagher - DC Fulfillment
                int SPLIT_OPTION = split_option.GetHashCode();
                char? APPLY_MINIMUMS_IND = apply_minimums_ind;
                char? PRIORITIZE_TYPE = prioritize_type;
                int? HEADER_FIELD = header_field;
                int? HCG_RID = hcg_rid;
                int? HEADERS_ORDER = header_order.GetHashCode();
                int STORES_ORDER = store_order.GetHashCode();
                int SPLIT_BY_OPTION = split_by_option.GetHashCode();
                int SPLIT_BY_RESERVE = split_by_reserve.GetHashCode();
                int APPLY_BY = apply_by.GetHashCode();
                int WITHIN_DC = within_dc.GetHashCode();
                // END TT#1966-MD - AGallagher - DC Fulfillment

                char USE_EXTERNAL_ELIGIBILITY_ALLOCATION;
                if (useExternalEligibilityAllocation)
                    USE_EXTERNAL_ELIGIBILITY_ALLOCATION = '1';
                else
                    USE_EXTERNAL_ELIGIBILITY_ALLOCATION = '0';

                char USE_EXTERNAL_ELIGIBILITY_PLANNING;
                if (useExternalEligibilityPlanning)
                    USE_EXTERNAL_ELIGIBILITY_PLANNING = '1';
                else
                    USE_EXTERNAL_ELIGIBILITY_PLANNING = '0';

                int EXTERNAL_ELIGIBILITY_PRODUCT_IDENTIFIER = externalEligibilityProductIdentifier.GetHashCode();
                int EXTERNAL_ELIGIBILITY_CHANNEL_IDENTIFIER = externalEligibilityChannelIdentifier.GetHashCode();

                string EXTERNAL_ELIGIBILITY_URL = Include.NullForStringValue;  
                if (externalEligibilityURL.Trim() != string.Empty) EXTERNAL_ELIGIBILITY_URL = externalEligibilityURL;

                int rowsUpdated = StoredProcedures.MID_SYSTEM_OPTIONS_UPDATE.Update(_dba,
                                                                  DO_UPDATE_RESERVE_STORE: DO_UPDATE_RESERVE_STORE,
                                                                  RESERVE_ST_RID: RESERVE_ST_RID,
                                                                  COMPANY_NAME: companyName,
                                                                  COMPANY_STREET: companyStreet,
                                                                  COMPANY_CITY: companyCity,
                                                                  COMPANY_SP_ABBREVIATION: companyState,
                                                                  COMPANY_POSTAL_CODE: companyZip,
                                                                  COMPANY_TELEPHONE: companyPhone,
                                                                  COMPANY_FAX: companyFax,
                                                                  COMPANY_EMAIL: companyEmail,
                                                                  PRODUCT_LEVEL_DELIMITER: productLevelDelimiter,
                                                                  SMTP_ENABLED: Include.ConvertBoolToChar(SMTP_Options.SMTP_Enabled.Value),
                                                                  SMTP_SERVER: SMTP_Options.SMTP_Server.Value,
                                                                  SMTP_PORT: SMTP_Options.SMTP_Port.Value,
                                                                  SMTP_USE_SSL: Include.ConvertBoolToChar(SMTP_Options.SMTP_Use_SSL.Value),
                                                                  SMTP_USE_DEFAULT_CREDENTIALS: Include.ConvertBoolToChar(SMTP_Options.SMTP_Use_Default_Credentials.Value),
                                                                  SMTP_USERNAME: SMTP_Options.SMTP_UserName.Value,
                                                                  SMTP_PWD: SMTP_Options.SMTP_Pwd.Value,
                                                                  SMTP_MESSAGE_FORMAT_IN_HTML: Include.ConvertBoolToChar(SMTP_Options.SMTP_MessageFormatInHTML.Value),
                                                                  SMTP_USE_OUTLOOK_CONTACTS: Include.ConvertBoolToChar(SMTP_Options.SMTP_Use_Outlook_Contacts.Value),
                                                                  SMTP_FROM_ADDRESS: SMTP_Options.SMTP_From_Address.Value,  
                                                                  PURGE_ALLOCATIONS: purgeAllocationsPeriod,
                                                                  STORE_DISPLAY_OPTION_ID: storeDisplayOptionId,
                                                                  DEFAULT_OTS_SG_RID: defaultOtsSgRid,
                                                                  DEFAULT_ALLOC_SG_RID: defaultAllocSgRid,
                                                                  NEW_STORE_TIMEFRAME_BEGIN: NEW_STORE_TIMEFRAME_BEGIN,
                                                                  NEW_STORE_TIMEFRAME_END: NEW_STORE_TIMEFRAME_END,
                                                                  NON_COMP_STORE_TIMEFRAME_BEGIN: NON_COMP_STORE_TIMEFRAME_BEGIN,
                                                                  NON_COMP_STORE_TIMEFRAME_END: NON_COMP_STORE_TIMEFRAME_END,
                                                                  PRODUCT_LEVEL_DISPLAY_ID: productLevelDisplayId,
                                                                  DEFAULT_PCT_NEED_LIMIT: DEFAULT_PCT_NEED_LIMIT,
                                                                  DEFAULT_BALANCE_TOLERANCE: DEFAULT_BALANCE_TOLERANCE,
                                                                  DEFAULT_PACK_SIZE_ERROR_PCT: DEFAULT_PACK_SIZE_ERROR_PCT,
                                                                  DEFAULT_MAX_SIZE_ERROR_PCT: DEFAULT_MAX_SIZE_ERROR_PCT,
                                                                  DEFAULT_FILL_SIZE_HOLES_PCT: DEFAULT_FILL_SIZE_HOLES_PCT,
                                                                  SIZE_BREAKOUT_IND: SIZE_BREAKOUT_IND,
                                                                  SIZE_NEED_IND: SIZE_NEED_IND,
                                                                  BULK_IS_DETAIL_IND: BULK_IS_DETAIL_IND,
                                                                  PROTECT_IF_HDRS_IND: PROTECT_IF_HDRS_IND,
                                                                  USE_WINDOWS_LOGIN: USE_WINDOWS_LOGIN,
                                                                  STORE_GRADE_TIMEFRAME: storeGradePeriod,
                                                                  NORMALIZE_SIZE_CURVES_IND: Include.ConvertBoolToChar(aNormalizeSizeCurves),
                                                                  FILL_SIZES_TO_TYPE: Convert.ToInt32(aFillSizesToType),
                                                                  GENERIC_PACK_ROUNDING_1ST_PACK_PCT: GENERIC_PACK_ROUNDING_1ST_PACK_PCT,
                                                                  GENERIC_PACK_ROUNDING_NTH_PACK_PCT: GENERIC_PACK_ROUNDING_NTH_PACK_PCT,
                                                                  ACTIVITY_MESSAGE_UPPER_LIMIT: Convert.ToInt32(aMyActivityMessageUpperLimit),
                                                                  SHIPPING_HORIZON_WEEKS: shippingHorizonWeeks,
                                                                  HEADER_LINK_CHARACTERISTIC: headerLinkCharacteristic,
                                                                  SIZE_CURVE_CHARMASK: SIZE_CURVE_CHARMASK,
                                                                  SIZE_GROUP_CHARMASK: SIZE_GROUP_CHARMASK,
                                                                  SIZE_ALTERNATE_CHARMASK: SIZE_ALTERNATE_CHARMASK,
                                                                  SIZE_CONSTRAINT_CHARMASK: SIZE_CONSTRAINT_CHARMASK,
                                                                  ALLOW_RLSE_IF_ALL_IN_RSRV_IND: Include.ConvertBoolToChar(aAllowRlseIfAllInReserve),
                                                                  GENERIC_SIZE_CURVE_NAME_TYPE: aGenericSizeCurveNameType.GetHashCode(),
                                                                  NUMBER_OF_WEEKS_WITH_ZERO_SALES: numberOfWeeksWithZeroSales,
                                                                  MAXIMUM_CHAIN_WOS: maximumChainWOS,
                                                                  PRORATE_CHAIN_STOCK: PRORATE_CHAIN_STOCK,
                                                                  GEN_SIZE_CURVE_USING: aGenerateSizeCurveUsing.GetHashCode(),
                                                                  PACK_TOLERANCE_NO_MAX_STEP_IND: Include.ConvertBoolToChar(aPackToleranceNoMaxStep),
                                                                  PACK_TOLERANCE_STEPPED_IND: Include.ConvertBoolToChar(aPackToleranceStepped),
                                                                  RI_EXPAND_IND: Include.ConvertBoolToChar(aRIExpandInd),
                                                                  ALLOW_STORE_MAX_VALUE_MODIFICATION: ALLOW_STORE_MAX_VALUE_MODIFICATION,
                                                                  VSW_SIZE_CONSTRAINTS: aVSWSizeConstraints.GetHashCode(),
                                                                  ENABLE_VELOCITY_GRADE_OPTIONS: ENABLE_VELOCITY_GRADE_OPTIONS,
                                                                  FORCE_SINGLE_CLIENT_INSTANCE: FORCE_SINGLE_CLIENT_INSTANCE,
                                                                  FORCE_SINGLE_USER_INSTANCE: FORCE_SINGLE_USER_INSTANCE,
                                                                  USE_ACTIVE_DIRECTORY_AUTHENTICATION: USE_ACTIVE_DIRECTORY_AUTHENTICATION,
                                                                  USE_ACTIVE_DIRECTORY_AUTHENTICATION_WITH_DOMAIN: USE_ACTIVE_DIRECTORY_AUTHENTICATION_WITH_DOMAIN,
                                                                  USE_BATCH_ONLY_MODE: USE_BATCH_ONLY_MODE,
                                                                  CONTROL_SERVICE_DEFAULT_BATCH_ONLY_MODE_ON: CONTROL_SERVICE_DEFAULT_BATCH_ONLY_MODE_ON,
                                                                  VSW_ITEM_FWOS_MAX_IND: VSW_ITEM_FWOS_MAX_IND,
                                                                  PRIOR_HEADER_INCLUDE_RESERVE_IND: PRIOR_HEADER_INCLUDE_RESERVE_IND,  // TT#1609-MD - stodd - data layer change - prior header
                                                                  DC_CARTON_ROUNDING_SG_RID: DC_CARTON_ROUNDING_SG_RID,	// TT#1652-MD - stodd - DC Carton Rounding
                                                                  SPLIT_OPTION: SPLIT_OPTION,                           // TT#1966-MD - AGallagher - DC Fulfillment
                                                                  APPLY_MINIMUMS_IND: APPLY_MINIMUMS_IND,               // TT#1966-MD - AGallagher - DC Fulfillment
                                                                  PRIORITIZE_TYPE: PRIORITIZE_TYPE,                     // TT#1966-MD - AGallagher - DC Fulfillment
                                                                  HEADER_FIELD: HEADER_FIELD,                           // TT#1966-MD - AGallagher - DC Fulfillment
                                                                  HCG_RID: HCG_RID,                                     // TT#1966-MD - AGallagher - DC Fulfillment
                                                                  HEADERS_ORDER: HEADERS_ORDER,                         // TT#1966-MD - AGallagher - DC Fulfillment
                                                                  STORES_ORDER: STORES_ORDER,                           // TT#1966-MD - AGallagher - DC Fulfillment
                                                                  SPLIT_BY_OPTION: SPLIT_BY_OPTION,                     // TT#1966-MD - AGallagher - DC Fulfillment
                                                                  SPLIT_BY_RESERVE: SPLIT_BY_RESERVE,                   // TT#1966-MD - AGallagher - DC Fulfillment
                                                                  APPLY_BY: APPLY_BY,                                   // TT#1966-MD - AGallagher - DC Fulfillment
                                                                  WITHIN_DC: WITHIN_DC,                                 // TT#1966-MD - AGallagher - DC Fulfillment
                                                                  USE_EXTERNAL_ELIGIBILITY_ALLOCATION: USE_EXTERNAL_ELIGIBILITY_ALLOCATION,
                                                                  USE_EXTERNAL_ELIGIBILITY_PLANNING: USE_EXTERNAL_ELIGIBILITY_PLANNING,
                                                                  EXTERNAL_ELIGIBILITY_PRODUCT_IDENTIFIER: EXTERNAL_ELIGIBILITY_PRODUCT_IDENTIFIER,
                                                                  EXTERNAL_ELIGIBILITY_CHANNEL_IDENTIFIER: EXTERNAL_ELIGIBILITY_CHANNEL_IDENTIFIER,
                                                                  EXTERNAL_ELIGIBILITY_URL: EXTERNAL_ELIGIBILITY_URL
                                                                  );


                return (rowsUpdated > 0);
                
            }
            catch (Exception e)
            {
                throw (e);
            }
        }




		/// <summary>
		/// returns the contents of the STATE_PROVINCE table as an array.
		/// </summary>
		/// <returns></returns>
		public string[] GetStateAbbreviationsArray()
		{
			try
			{
				string [] stateAbbrev = null;

				DataTable dtStateAbbrev = GetStateAbbreviations();

				int idx = 0;
				stateAbbrev = new string[ dtStateAbbrev.Rows.Count ];
				foreach(DataRow row in dtStateAbbrev.Rows)
				{
					string abbrev = (string)row["SP_ABBREVIATION"];
					stateAbbrev[idx] = abbrev; 

					idx++;
				}
				
				return stateAbbrev;
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		/// <summary>
		/// reads the STATE_PROVINCE table and returns it's contents as a DataTable
		/// </summary>
		/// <returns></returns>
		public DataTable GetStateAbbreviations()
		{
			try
			{
                // MID Track # 2354 - removed nolock because it causes concurrency issues
                return StoredProcedures.MID_STATE_PROVINCE_READ_ALL.Read(_dba);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		/// <summary>
		/// Reserver store is one of only global option fields updated behind-the-scenes, so
		/// this special update was created;
		/// </summary>
		/// <param name="reserveStoreRid"></param>
		public void UpdateReserveStore(int systemOptionRid, int reserveStoreRid)
		{
			try
			{
                int? RESERVE_ST_RID_Nullable = null;
                if (reserveStoreRid > 0) RESERVE_ST_RID_Nullable = reserveStoreRid;
                StoredProcedures.MID_SYSTEM_OPTIONS_UPDATE_RESERVE_STORE.Update(_dba,
                                                                                SYSTEM_OPTION_RID: systemOptionRid,
                                                                                RESERVE_ST_RID: RESERVE_ST_RID_Nullable
                                                                                );

				return;
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public void UpdateStoreDeleteInProgress(int systemOptionRid, bool iStoreDeleteInProgress)
		{
			try
			{
				char inProgress = Include.ConvertBoolToChar(iStoreDeleteInProgress);
				
                StoredProcedures.MID_SYSTEM_OPTIONS_UPDATE_DELETE_STORE_IN_PROGRESS.Update(_dba,
                                                                                           SYSTEM_OPTION_RID: systemOptionRid,
                                                                                           STORE_DELETE_IN_PROGRESS_IND: inProgress
                                                                                           );

				return;
			}
			catch (Exception err)
			{
				string message = err.ToString();
				throw;
			}
		}

        // Begin TT#78 - Ron Matelic - Header Characteristic delete issue when designated as Global Option
        public void UpdateHeaderLinkCharacteristic(int systemOptionRID, int headerCharGroupRID)
        {
            try
            {
                StoredProcedures.MID_SYSTEM_OPTIONS_UPDATE_HEADER_LINK_CHARACTERISTIC.Update(_dba,
                                                                                             SYSTEM_OPTION_RID: systemOptionRID,
                                                                                             HEADER_LINK_CHARACTERISTIC: headerCharGroupRID
                                                                                             );

                return;
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }
        // End TT#78 

		public DataTable GetHeaderReleaseTypes()
		{
			try
			{
                return StoredProcedures.MID_HEADER_TYPE_RELEASE_READ_ALL.Read(_dba);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public void AddHeaderReleaseTypes(eHeaderType aHeaderType, bool aReleaseHeaderType)
		{
			try
			{
                StoredProcedures.MID_HEADER_TYPE_RELEASE_INSERT.Insert(_dba,
                                                                       HEADER_TYPE: Convert.ToInt32(aHeaderType, CultureInfo.CurrentCulture),
                                                                       RELEASE_HEADER_TYPE_IND: Include.ConvertBoolToChar(aReleaseHeaderType)
                                                                       );
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public void DeleteHeaderReleaseTypes()
		{
			try
			{
                StoredProcedures.MID_HEADER_TYPE_RELEASE_DELETE_ALL.Delete(_dba);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public void WriteLicenseKey(int systemOptionRid,string aLicenseKey)
		{
			try
			{
				OpenUpdateConnection();
                StoredProcedures.MID_SYSTEM_OPTIONS_UPDATE_LICENSE_KEY.Update(_dba,
                                                                              SYSTEM_OPTION_RID: systemOptionRid,
                                                                              LICENSE_KEY: aLicenseKey
                                                                              );
				CommitData();
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
			finally
			{
				if (ConnectionIsOpen)
				{
					_dba.CloseUpdateConnection();
				}
			}
		}

        // Begin TT#862 - MD - stodd - Assortment Upgrade Issues
        public string GetLicenseKey(int systemOptionRid)
        {
            try
            {
                OpenUpdateConnection();

                //MIDDbParameter[] InParams = { new MIDDbParameter("@SYSTEM_OPTIONS_RID", systemOptionRid, eDbType.Int)
                //                            };
                //return (string)_dba.ExecuteScalar("SELECT [dbo].[UDF_MID_GET_LICENSE_KEY] (@SYSTEM_OPTIONS_RID) AS LICENSE_KEY", InParams);

                return (string)StoredProcedures.MID_LICENSE_KEY_READ.ReadValue(_dba, SYSTEM_OPTIONS_RID: systemOptionRid);
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
            finally
            {
                if (ConnectionIsOpen)
                {
                    _dba.CloseUpdateConnection();
                }
            }
        }
        // End TT#862 - MD - stodd - Assortment Upgrade Issues

	}
}
