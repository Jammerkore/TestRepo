--dv =============================================
--dv Modified date: 4/18/2017
--dv Notes:	
--dv History: TT#1966-MD - JSmith- DC Fulfillment
--dv =============================================    
create table [dbo].[METHOD_DC_FULFILLMENT] ( 
	"METHOD_RID" int not null,
	"SPLIT_OPTION" int not null,
	"APPLY_MINIMUMS_IND" char(1) null,
	"PRIORITIZE_TYPE" char not null,
	"HEADER_FIELD" int null,
	"HCG_RID" int null,
	"HEADERS_ORDER" int not null,
	"STORES_ORDER" int not null,
	"FIELD_DATA_TYPE" int null,
	"SPLIT_BY_OPTION" int null,			
	"SPLIT_BY_RESERVE" int null,
	"APPLY_BY" int null,
	"WITHIN_DC" int null
	)	
   
go
