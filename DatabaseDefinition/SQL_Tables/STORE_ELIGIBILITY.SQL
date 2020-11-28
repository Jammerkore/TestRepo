--dv =============================================
--dv Modified date: 3/26/2014
--dv Notes:	
--dv History: TT#42 -stodd -projected dales
--dv =============================================  
create table [dbo].[STORE_ELIGIBILITY] ( 
	"HN_RID" int not null,
	"ST_RID" int not null,
	"EM_RID" int null,
	"INELIGIBLE" char(1) null,
	"STKMOD_TYPE" int not null,
	"STKMOD_RID" int null,
	"STKMOD_PCT" float null,
	"SLSMOD_TYPE" int not null,
	"SLSMOD_RID" int null,
	"SLSMOD_PCT" float null,
	"SIMILAR_STORE_TYPE" int not null,
	"SIMILAR_STORE_RATIO" float null,
	"UNTIL_DATE" int null,
	"USE_ELIGIBILITY" char(1) null,
	"FWOSMOD_TYPE" int null,
	"FWOSMOD_RID" int null,
	"FWOSMOD_PCT" float null,
	"PRESENTATION_PLUS_SALES_IND" char(1) null,
	"STOCK_LEAD_WEEKS" int null)

go
