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
            int? defaultOtsSgRid, 
            int? defaultAllocSgRid,
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
            int? shippingHorizonWeeks, 
            char? productLevelDelimiter, 
            int? headerLinkCharacteristic,
            string sizeCurveCharMask, 
            string sizeGroupCharMask, 
            string sizeAlternateCharMask,
            string sizeConstraintCharMask, 
            bool aNormalizeSizeCurves, 
            int? aFillSizesToType, 
            bool aAllowRlseIfAllInReserve,  
            int? numberOfWeeksWithZeroSales,
            int? maximumChainWOS,
            bool prorateChainStock, 
            int? aGenericSizeCurveNameType,
            eGenerateSizeCurveUsing aGenerateSizeCurveUsing, 
            bool aPackToleranceNoMaxStep,
            bool aPackToleranceStepped,
            bool aRIExpandInd,  
            bool allowStoreMaxValueModification,
            int? aVSWSizeConstraints, 
            int? aMyActivityMessageUpperLimit,
            GlobalOptions_SMTP_BL SMTP_Options,
            bool storeDeleteInProgress,
            bool enableVelocityGradeOptions,
            bool forceSingleClientInstance,
            bool forceSingleUserInstance,
            bool useActiveDirectoryAuthentication,
            bool useActiveDirectoryAuthenticationWithDomain,
            bool useBatchOnlyMode,
            bool controlServiceDefaultBatchOnlyModeOn,
            int? aVSWItemFWOSMax,   // TT#933-MD - AGallagher - Item Max vs. FWOS Max
			bool priorHeaderIncludeReserveInd,   // TT#1609-MD - stodd - data layer change - prior header
			int? cartonRoundingSgRid,		// TT#1652-MD - stodd - DC Carton Rounding
            int? split_option,               // TT#1966-MD - AGallagher - DC Fulfillment
            char? apply_minimums_ind,        // TT#1966-MD - AGallagher - DC Fulfillment
            char? prioritize_type,           // TT#1966-MD - AGallagher - DC Fulfillment
            int? header_field,               // TT#1966-MD - AGallagher - DC Fulfillment
            int? hcg_rid,                    // TT#1966-MD - AGallagher - DC Fulfillment
            int? header_order,              // TT#1966-MD - AGallagher - DC Fulfillment
            int? store_order,                // TT#1966-MD - AGallagher - DC Fulfillment
            int? split_by_option,          // TT#1966-MD - AGallagher - DC Fulfillment
            int? split_by_reserve,               // TT#1966-MD - AGallagher - DC Fulfillment
            int? apply_by,                      // TT#1966-MD - AGallagher - DC Fulfillment
            int? within_dc,                     // TT#1966-MD - AGallagher - DC Fulfillment
            bool useExternalEligibilityAllocation,
            bool useExternalEligibilityPlanning,
            int? externalEligibilityProductIdentifier,
            int? externalEligibilityChannelIdentifier,
            string externalEligibilityURL
            )
        {
            try
            {
                int DO_UPDATE_RESERVE_STORE = 0;
                int reserveStoreRidParsed = 0;
                int? RESERVE_ST_RID = null;
                if (int.TryParse(reserveStoreRid, NumberStyles.Integer, CultureInfo.CurrentUICulture, out reserveStoreRidParsed))
                {
                    DO_UPDATE_RESERVE_STORE = 1;
                    RESERVE_ST_RID = reserveStoreRidParsed;
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

                int newStorePeriodBeginParsed = 0;
                int? NEW_STORE_TIMEFRAME_BEGIN = null;
                if (int.TryParse(newStorePeriodBegin, NumberStyles.Integer, CultureInfo.CurrentUICulture, out newStorePeriodBeginParsed))
                {
                    NEW_STORE_TIMEFRAME_BEGIN = newStorePeriodBeginParsed;
                }

                int newStorePeriodEndParsed = 0;
                int? NEW_STORE_TIMEFRAME_END = null;
                if (int.TryParse(newStorePeriodEnd, NumberStyles.Integer, CultureInfo.CurrentUICulture, out newStorePeriodEndParsed))
                {
                    NEW_STORE_TIMEFRAME_END = newStorePeriodEndParsed;
                }

                int nonCompStorePeriodBeginParsed = 0;
                int? NON_COMP_STORE_TIMEFRAME_BEGIN = null;
                if (int.TryParse(nonCompStorePeriodBegin, NumberStyles.Integer, CultureInfo.CurrentUICulture, out nonCompStorePeriodBeginParsed))
                {
                    NON_COMP_STORE_TIMEFRAME_BEGIN = nonCompStorePeriodBeginParsed;
                }

                int nonCompStorePeriodEndParsed = 0;
                int? NON_COMP_STORE_TIMEFRAME_END = null;
                if (int.TryParse(nonCompStorePeriodEnd, NumberStyles.Integer, CultureInfo.CurrentUICulture, out nonCompStorePeriodEndParsed))
                {
                    NON_COMP_STORE_TIMEFRAME_END = nonCompStorePeriodEndParsed;
                }

                double defaultPercentNeedLimitParsed = 0;
                double? DEFAULT_PCT_NEED_LIMIT = null;
                if (double.TryParse(defaultPercentNeedLimit, NumberStyles.Number, CultureInfo.CurrentUICulture, out defaultPercentNeedLimitParsed))
                {
                    DEFAULT_PCT_NEED_LIMIT = defaultPercentNeedLimitParsed;
                }

                double defaultBalanceToleranceParsed = 0;
                double? DEFAULT_BALANCE_TOLERANCE = null;
                if (double.TryParse(defaultBalanceTolerance, NumberStyles.Number, CultureInfo.CurrentUICulture, out defaultBalanceToleranceParsed))
                {
                    DEFAULT_BALANCE_TOLERANCE = defaultBalanceToleranceParsed;
                }

                double defaultPackSizeErrorPercentParsed = 0;
                double? DEFAULT_PACK_SIZE_ERROR_PCT = null;
                if (double.TryParse(defaultPackSizeErrorPercent, NumberStyles.Number, CultureInfo.CurrentUICulture, out defaultPackSizeErrorPercentParsed))
                {
                    DEFAULT_PACK_SIZE_ERROR_PCT = defaultPackSizeErrorPercentParsed;
                }

                double defaultMaxSizeErrorPercentParsed = 0;
                double? DEFAULT_MAX_SIZE_ERROR_PCT = null;
                if (double.TryParse(defaultMaxSizeErrorPercent, NumberStyles.Number, CultureInfo.CurrentUICulture, out defaultMaxSizeErrorPercentParsed))
                {
                    DEFAULT_MAX_SIZE_ERROR_PCT = defaultMaxSizeErrorPercentParsed;
                }

                double defaultFillSizeHolesPercentParsed = 0;
                double? DEFAULT_FILL_SIZE_HOLES_PCT = null;
                if (double.TryParse(defaultFillSizeHolesPercent, NumberStyles.Number, CultureInfo.CurrentUICulture, out defaultFillSizeHolesPercentParsed))
                {
                    DEFAULT_FILL_SIZE_HOLES_PCT = defaultFillSizeHolesPercentParsed;
                }

                double genericPackRounding1stPackPctParsed = 0;
                double? GENERIC_PACK_ROUNDING_1ST_PACK_PCT = null;
                if (double.TryParse(genericPackRounding1stPackPct, NumberStyles.Number, CultureInfo.CurrentUICulture, out genericPackRounding1stPackPctParsed))
                {
                    GENERIC_PACK_ROUNDING_1ST_PACK_PCT = genericPackRounding1stPackPctParsed;
                }

                double genericPackRoundingNthPackPctParsed = 0;
                double? GENERIC_PACK_ROUNDING_NTH_PACK_PCT = null;
                if (double.TryParse(genericPackRoundingNthPackPct, NumberStyles.Number, CultureInfo.CurrentUICulture, out genericPackRoundingNthPackPctParsed))
                {
                    GENERIC_PACK_ROUNDING_NTH_PACK_PCT = genericPackRoundingNthPackPctParsed;
                }

                string SIZE_CURVE_CHARMASK = Include.NullForStringValue;  //TT#1310-MD -jsobek -Error when adding a new Store
                if (!String.IsNullOrWhiteSpace(sizeCurveCharMask))
                {
                    SIZE_CURVE_CHARMASK = sizeCurveCharMask.Trim();
                }

                string SIZE_GROUP_CHARMASK = Include.NullForStringValue;  //TT#1310-MD -jsobek -Error when adding a new Store
                if (!String.IsNullOrWhiteSpace(sizeGroupCharMask))
                {
                    SIZE_GROUP_CHARMASK = sizeGroupCharMask.Trim();
                }

                string SIZE_ALTERNATE_CHARMASK = Include.NullForStringValue;  //TT#1310-MD -jsobek -Error when adding a new Store
                if (!String.IsNullOrWhiteSpace(sizeAlternateCharMask))
                {
                    SIZE_ALTERNATE_CHARMASK = sizeAlternateCharMask.Trim();
                }

                string SIZE_CONSTRAINT_CHARMASK = Include.NullForStringValue;  //TT#1310-MD -jsobek -Error when adding a new Store
                if (!String.IsNullOrWhiteSpace(sizeConstraintCharMask))
                {
                    SIZE_CONSTRAINT_CHARMASK = sizeConstraintCharMask.Trim();
                }

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
                if (cartonRoundingSgRid != null && cartonRoundingSgRid > 0)
                {
                    DC_CARTON_ROUNDING_SG_RID = cartonRoundingSgRid;
                }
                // End TT#1689-MD - stodd - DC Carton Rounding Default Attribute should be blank if not selected

                // BEGIN TT#1966-MD - AGallagher - DC Fulfillment
                char? APPLY_MINIMUMS_IND = apply_minimums_ind;
                char? PRIORITIZE_TYPE = prioritize_type;
                int? HEADER_FIELD = header_field;
                int? HCG_RID = hcg_rid;

                // checking if int is null then converting to int values
                int? HEADERS_ORDER = header_order != null ? (int?)(header_order.GetHashCode()) : null;
                int? STORES_ORDER = store_order != null ? (int?)(store_order.GetHashCode()) : null;
                int? SPLIT_BY_OPTION = split_by_option != null ? (int?)(split_by_option.GetHashCode()) : null;
                int? SPLIT_BY_RESERVE = split_by_reserve != null ? (int?)(split_by_reserve.GetHashCode()) : null;
                int? APPLY_BY = apply_by != null ? (int?)(apply_by.GetHashCode()) : null;
                int? WITHIN_DC = within_dc != null ? (int?)(within_dc.GetHashCode()) : null;
                // END TT#1966-MD - AGallagher - DC Fulfillment

                char USE_EXTERNAL_ELIGIBILITY_ALLOCATION = useExternalEligibilityAllocation ? '1' : '0';
                char USE_EXTERNAL_ELIGIBILITY_PLANNING = useExternalEligibilityPlanning ? '1' : '0';

                int? EXTERNAL_ELIGIBILITY_PRODUCT_IDENTIFIER = externalEligibilityProductIdentifier != null ? 
                    (int?)(externalEligibilityProductIdentifier.GetHashCode()) : null;
                int? EXTERNAL_ELIGIBILITY_CHANNEL_IDENTIFIER = externalEligibilityChannelIdentifier != null ? 
                    (int?)(externalEligibilityChannelIdentifier.GetHashCode()) : null;

                string EXTERNAL_ELIGIBILITY_URL = Include.NullForStringValue;
                if (!String.IsNullOrWhiteSpace(externalEligibilityURL))
                {
                    EXTERNAL_ELIGIBILITY_URL = externalEligibilityURL.Trim();
                }

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
                                                                  FILL_SIZES_TO_TYPE: aFillSizesToType,
                                                                  GENERIC_PACK_ROUNDING_1ST_PACK_PCT: GENERIC_PACK_ROUNDING_1ST_PACK_PCT,
                                                                  GENERIC_PACK_ROUNDING_NTH_PACK_PCT: GENERIC_PACK_ROUNDING_NTH_PACK_PCT,
                                                                  ACTIVITY_MESSAGE_UPPER_LIMIT: aMyActivityMessageUpperLimit,
                                                                  SHIPPING_HORIZON_WEEKS: shippingHorizonWeeks,
                                                                  HEADER_LINK_CHARACTERISTIC: headerLinkCharacteristic,
                                                                  SIZE_CURVE_CHARMASK: SIZE_CURVE_CHARMASK,
                                                                  SIZE_GROUP_CHARMASK: SIZE_GROUP_CHARMASK,
                                                                  SIZE_ALTERNATE_CHARMASK: SIZE_ALTERNATE_CHARMASK,
                                                                  SIZE_CONSTRAINT_CHARMASK: SIZE_CONSTRAINT_CHARMASK,
                                                                  ALLOW_RLSE_IF_ALL_IN_RSRV_IND: Include.ConvertBoolToChar(aAllowRlseIfAllInReserve),
                                                                  GENERIC_SIZE_CURVE_NAME_TYPE: aGenericSizeCurveNameType != null ? (int?)(aGenericSizeCurveNameType.GetHashCode()) : null,
                                                                  NUMBER_OF_WEEKS_WITH_ZERO_SALES: numberOfWeeksWithZeroSales,
                                                                  MAXIMUM_CHAIN_WOS: maximumChainWOS,
                                                                  PRORATE_CHAIN_STOCK: PRORATE_CHAIN_STOCK,
                                                                  GEN_SIZE_CURVE_USING: aGenerateSizeCurveUsing.GetHashCode(),
                                                                  PACK_TOLERANCE_NO_MAX_STEP_IND: Include.ConvertBoolToChar(aPackToleranceNoMaxStep),
                                                                  PACK_TOLERANCE_STEPPED_IND: Include.ConvertBoolToChar(aPackToleranceStepped),
                                                                  RI_EXPAND_IND: Include.ConvertBoolToChar(aRIExpandInd),
                                                                  ALLOW_STORE_MAX_VALUE_MODIFICATION: ALLOW_STORE_MAX_VALUE_MODIFICATION,
                                                                  VSW_SIZE_CONSTRAINTS: aVSWSizeConstraints,
                                                                  ENABLE_VELOCITY_GRADE_OPTIONS: ENABLE_VELOCITY_GRADE_OPTIONS,
                                                                  FORCE_SINGLE_CLIENT_INSTANCE: FORCE_SINGLE_CLIENT_INSTANCE,
                                                                  FORCE_SINGLE_USER_INSTANCE: FORCE_SINGLE_USER_INSTANCE,
                                                                  USE_ACTIVE_DIRECTORY_AUTHENTICATION: USE_ACTIVE_DIRECTORY_AUTHENTICATION,
                                                                  USE_ACTIVE_DIRECTORY_AUTHENTICATION_WITH_DOMAIN: USE_ACTIVE_DIRECTORY_AUTHENTICATION_WITH_DOMAIN,
                                                                  USE_BATCH_ONLY_MODE: USE_BATCH_ONLY_MODE,
                                                                  CONTROL_SERVICE_DEFAULT_BATCH_ONLY_MODE_ON: CONTROL_SERVICE_DEFAULT_BATCH_ONLY_MODE_ON,
                                                                  VSW_ITEM_FWOS_MAX_IND: aVSWItemFWOSMax,
                                                                  PRIOR_HEADER_INCLUDE_RESERVE_IND: PRIOR_HEADER_INCLUDE_RESERVE_IND,  // TT#1609-MD - stodd - data layer change - prior header
                                                                  DC_CARTON_ROUNDING_SG_RID: DC_CARTON_ROUNDING_SG_RID,	// TT#1652-MD - stodd - DC Carton Rounding
                                                                  SPLIT_OPTION: split_option,                           // TT#1966-MD - AGallagher - DC Fulfillment
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
