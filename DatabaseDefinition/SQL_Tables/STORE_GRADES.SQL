--dv =============================================
--dv Modified date: 3/26/2014
--dv Notes:	
--dv History: 
--dv =============================================   
create table [dbo].[STORE_GRADES] ( 
	"HN_RID" int not null,
	"BOUNDARY" int not null,
	"GRADE_CODE" varchar(20) null,
	"WOS_INDEX" float null,
	"MINIMUM_STOCK" int null,
	"MAXIMUM_STOCK" int null,
	"MINIMUM_AD" int null,
	"MINIMUM_COLOR" int null,
	"MAXIMUM_COLOR" int null,
	"SHIP_UP_TO" int null)   

go
