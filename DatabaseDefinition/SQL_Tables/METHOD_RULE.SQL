--dv =============================================
--dv Modified date: 3/26/2014
--dv Notes:	
--dv History: 
--dv =============================================   
create table [dbo].[METHOD_RULE] ( 
	"METHOD_RID" int not null,
	"STORE_FILTER_RID" int null,
	"HDR_RID" int null,
	"STORE_ORDER" int null,
	"HEADER_COMPONENT" int null,
	"HDR_PACK_RID" int null,
	"HDR_BC_RID" int null,
	"INCLUDED_STORES" int null,
	"INCLUDED_QUANTITY" int null,
	"EXCLUDED_STORES" int null,
	"EXCLUDED_QUANTITY" int null,
	"SGL_RID" int null,
	"IS_HEADER_MASTER" char(1) null,  
	"INCLUDE_RESERVE_IND" char(1) default '0' null)
go
