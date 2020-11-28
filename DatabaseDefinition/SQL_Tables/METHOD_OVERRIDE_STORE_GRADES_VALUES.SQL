--dv =============================================
--dv Modified date: 3/26/2014
--dv Notes:	
--dv History: 
--dv ============================================= 
create table [dbo].[METHOD_OVERRIDE_STORE_GRADES_VALUES] ( 
	"METHOD_RID" int not null,
	"SGL_RID" int not null,
	"BOUNDARY" int not null,
	"MINIMUM_STOCK" int null,
	"MAXIMUM_STOCK" int null,
	"MINIMUM_AD" int null,
	"MINIMUM_COLOR" int null,
	"MAXIMUM_COLOR" int null, 
	"SHIP_UP_TO" int null) 

go
