--dv =============================================
--dv Modified date: 3/26/2014
--dv Notes:	
--dv History: 
--dv =============================================   
create table [dbo].[USER_ASSORTMENT] ( 
	"USER_RID" int not null,
	"SG_RID" int null,
	"GROUP_BY_ID" int null,
	"VIEW_RID" int null,
	"VARIABLE_TYPE" int null,
	"VARIABLE_NUMBER" int null,
	"INCL_ONHAND" char(1) null,
	"INCL_INTRANSIT" char(1) null,
	"INCL_SIMILAR_STORES" char(1) null,
	"INCL_COMMITTED" char(1) null,
	"AVERAGE_BY" int not null default 0,
	"GRADE_BOUNDARY_IND" int null) 

go
