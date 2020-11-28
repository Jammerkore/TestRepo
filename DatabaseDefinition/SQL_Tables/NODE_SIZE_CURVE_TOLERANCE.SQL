--dv =============================================
--dv Modified date: 3/26/2014
--dv Notes:	
--dv History: TT#2079 -DOConnell -Size - Minimum % Tolerance Calculation Issue
--dv ============================================= 
create table [dbo].[NODE_SIZE_CURVE_TOLERANCE] ( 
	"HN_RID" int not null,
	"MINIMUM_AVERAGE" float null,
	"PH_OFFSET_IND" int null,
	"PH_RID" int null,
	"PHL_SEQUENCE" int null,
	"PHL_OFFSET" int null,
	"SALES_TOLERANCE" float null,
	"INDEX_UNITS_TYPE" int null,
	"MIN_TOLERANCE" float null,
	"MAX_TOLERANCE" float null,			
	"APPLY_MIN_TO_ZERO_TOLERANCE_IND" char null) 

go
