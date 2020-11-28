--dv =============================================
--dv Modified date: 8/13/2014
--dv Notes:	
--dv History:  RMatelic - Add CREATE table SQL
--dv ============================================= 
create table [dbo].[METHOD_GROUP_ALLOCATION_STORE_GRADES_VALUES] ( 
	"METHOD_RID" int not null,
	"SGL_RID" int not null,
	"BOUNDARY" int not null,
	"MINIMUM_GROUP" int null,
	"MAXIMUM_GROUP" int null,
	"MINIMUM_HEADER" int null,
	"MAXIMUM_HEADER" int null) 

go