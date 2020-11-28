--dv =============================================
--dv Modified date: 5/2/20717
--dv Notes:	Do not put comments in the table definition, the columns are read by a parser, and comments may exclude your column
--dv History: TT#894-MD -jsobek -Single Client Instance System Option
--dv History: TT#933-MD - AGallagher - Item Max vs. FWOS Max
--dv History: TT#3600 -jsobek -Add a default email address on the global options screen...	
--dv History: TT#1521-MD -jsobek -Active Directory Authentication
--dv History: TT#1966-MD - AGallagher - DC Fulfillment	
--dv =============================================  
create table [dbo].[SYSTEM_OPTIONS] ( 
	"SYSTEM_OPTION_RID" int not null,
	"COMPANY_NAME" varchar(50) not null,
	"COMPANY_STREET" varchar(50) null,
	"COMPANY_CITY" varchar(30) null,
	"COMPANY_SP_ABBREVIATION" varchar(10) null,
	"COMPANY_POSTAL_CODE" varchar(30) null,
	"COMPANY_TELEPHONE" varchar(30) null,
	"COMPANY_FAX" varchar(30) null,
	"COMPANY_EMAIL" varchar(50) null,
	"PURGE_ALLOCATIONS" int not null,
	"STORE_DISPLAY_OPTION_ID" int not null,
	"DEFAULT_OTS_SG_RID" int null,
	"DEFAULT_ALLOC_SG_RID" int null,
	"NEW_STORE_TIMEFRAME_BEGIN" int null,
	"NEW_STORE_TIMEFRAME_END" int null,
	"NON_COMP_STORE_TIMEFRAME_BEGIN" int null,
	"NON_COMP_STORE_TIMEFRAME_END" int null,
	"PRODUCT_LEVEL_DISPLAY_ID" int not null,
	"DEFAULT_PCT_NEED_LIMIT" float null,
	"DEFAULT_BALANCE_TOLERANCE" float null,
	"DEFAULT_PACK_SIZE_ERROR_PCT" float null,
	"DEFAULT_MAX_SIZE_ERROR_PCT" float null,
	"DEFAULT_FILL_SIZE_HOLES_PCT" float null,
	"SIZE_BREAKOUT_IND" char(1) not null,
	"SIZE_NEED_IND" char(1) not null,
	"STORE_GRADE_TIMEFRAME" int not null,
	"PROTECT_IF_HDRS_IND" char(1) not null,
	"RESERVE_ST_RID" int null,
	"BULK_IS_DETAIL_IND" char(1) default '0' not null,
	"STORE_TABLE_COUNT" int default 1 null,
	"USE_WINDOWS_LOGIN" char(1) default '0' null,
	"SHIPPING_HORIZON_WEEKS" int default 0 null,
	"LICENSE_KEY" varchar(140) null,
	"PRODUCT_LEVEL_DELIMITER" char(1) default '\' null,
	"HEADER_LINK_CHARACTERISTIC" int null,
	"SIZE_CURVE_CHARMASK" varchar(30) null,
	"SIZE_GROUP_CHARMASK" varchar(30) null,
	"SIZE_ALTERNATE_CHARMASK" varchar(30) null,
	"SIZE_CONSTRAINT_CHARMASK" varchar(30) null,
	"NORMALIZE_SIZE_CURVES_IND" char(1) null,
	"FILL_SIZES_TO_TYPE" int null,
    "ALLOW_RLSE_IF_ALL_IN_RSRV_IND" char(1) null,   
	"NUMBER_OF_WEEKS_WITH_ZERO_SALES" int null,		
	"MAXIMUM_CHAIN_WOS" int null,					
	"PRORATE_CHAIN_STOCK" char(1) default '0' null, 
	"GENERIC_SIZE_CURVE_NAME_TYPE" int null,  
	"GEN_SIZE_CURVE_USING" int default 801720 not null, 
	"GENERIC_PACK_ROUNDING_1ST_PACK_PCT" float null,			
	"GENERIC_PACK_ROUNDING_NTH_PACK_PCT" float null,			
	"PACK_TOLERANCE_NO_MAX_STEP_IND" char(1) default '0' null,  
	"PACK_TOLERANCE_STEPPED_IND" char(1) default '0' null,      
	"PERFORM_ONETIME_SPECIAL_PURGE_IND" char default '1' null,  
	"ALLOW_STORE_MAX_VALUE_MODIFICATION" bit DEFAULT 1,			
	"VSW_SIZE_CONSTRAINTS" int null,			
	"ACTIVITY_MESSAGE_UPPER_LIMIT" int default 100000 null, 
	"RI_EXPAND_IND" char(1) default '0' not null,  
	"STORE_DELETE_IN_PROGRESS_IND" char(1) default '0' null,	 
	"CONSTRAINTS_DROPPED" char(1) default '0' null,	 
	"SMTP_ENABLED" char(1) default '0' null,
	"SMTP_SERVER" varchar(250)  null,
	"SMTP_PORT" int default 25 null,
	"SMTP_USE_SSL" char(1) default '0' null,
	"SMTP_USE_DEFAULT_CREDENTIALS" char(1) default '1' null,
	"SMTP_USERNAME" varchar(250)  null,
	"SMTP_PWD" varchar(50)  null,
	"SMTP_MESSAGE_FORMAT_IN_HTML" char(1) default '1' null,
	"SMTP_USE_OUTLOOK_CONTACTS" char(1) default '0' null,
	"ENABLE_VELOCITY_GRADE_OPTIONS" char(1) null,
	"FORCE_SINGLE_CLIENT_INSTANCE" char(1) null, 
	"FORCE_SINGLE_USER_INSTANCE" char(1) null, 
	"USE_ACTIVE_DIRECTORY_AUTHENTICATION" char(1) null, 
	"USE_ACTIVE_DIRECTORY_AUTHENTICATION_WITH_DOMAIN" char(1) null, 
	"USE_BATCH_ONLY_MODE" char(1) default '0' null, 
	"CONTROL_SERVICE_DEFAULT_BATCH_ONLY_MODE_ON" char(1) default '0' null, 
	"VSW_ITEM_FWOS_MAX_IND" int null,	
	"SMTP_FROM_ADDRESS" varchar(250) default 'app@midretail.com' not null, 
	"USE_NET_SALES_IN_SIZE_DAY_TO_WEEK_SUMMARY" char(1) default '0' null,
	"PRIOR_HEADER_INCLUDE_RESERVE_IND" char(1) default '0' null,
	"DC_CARTON_ROUNDING_SG_RID" int null,	/* TT#1652-MD - stodd - DC Carton Rounding */
	"SPLIT_OPTION" int null,				/* TT#1966-MD - AGallagher - DC Fulfillment */
	"APPLY_MINIMUMS_IND" char(1) null,		/* TT#1966-MD - AGallagher - DC Fulfillment */
	"PRIORITIZE_TYPE" char(1) null,			/* TT#1966-MD - AGallagher - DC Fulfillment */
	"HEADER_FIELD" int null,				/* TT#1966-MD - AGallagher - DC Fulfillment */
	"HCG_RID" int null,						/* TT#1966-MD - AGallagher - DC Fulfillment */
	"HEADERS_ORDER" int null,				/* TT#1966-MD - AGallagher - DC Fulfillment */
	"STORES_ORDER" int null, 				/* TT#1966-MD - AGallagher - DC Fulfillment */
	"SPLIT_BY_OPTION" int null,				/* TT#1966-MD - AGallagher - DC Fulfillment */
	"SPLIT_BY_RESERVE" int null,			/* TT#1966-MD - AGallagher - DC Fulfillment */
	"APPLY_BY" int null,					/* TT#1966-MD - AGallagher - DC Fulfillment */
	"WITHIN_DC" int null,					/* TT#1966-MD - AGallagher - DC Fulfillment */
	)