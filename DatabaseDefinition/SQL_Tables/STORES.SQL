--dv =============================================
--dv Modified date: 3/26/2014
--dv Notes:	
--dv History: TT#1401 -GTaylor -Reservation Stores 
--dv History: TT#739-MD -stodd -delete stores
--dv ============================================= 
create table [dbo].[STORES] ( 
	"ST_RID" int identity not null,
	"ST_ID" varchar(50) not null,
	"STORE_NAME" varchar(50) null,
	"STORE_DESC" varchar(50) null,
	"ACTIVE_IND" char(1) not null,
	"CITY" varchar(50) null,
	"STATE" varchar(50) null,
	"SELLING_SQ_FT" int default 0 null,
	"SELLING_OPEN_DATE" smalldatetime null,
	"SELLING_CLOSE_DATE" smalldatetime null,
	"STOCK_OPEN_DATE" smalldatetime null,
	"STOCK_CLOSE_DATE" smalldatetime null,
	"LEAD_TIME" int null,
	"SHIP_ON_MONDAY" char(1) null,
	"SHIP_ON_TUESDAY" char(1) null,
	"SHIP_ON_WEDNESDAY" char(1) null,
	"SHIP_ON_THURSDAY" char(1) null,
	"SHIP_ON_FRIDAY" char(1) null,
	"SHIP_ON_SATURDAY" char(1) null,
	"SHIP_ON_SUNDAY" char(1) null,
	"SIMILAR_STORE_MODEL" char(1) default '0' not null,
	"IMO_ID" VARCHAR(50) null,	
	"STORE_DELETE_IND" char(1) default '0' not null	
	)  

go
