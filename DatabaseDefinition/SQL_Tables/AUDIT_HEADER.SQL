--dv =============================================
--dv Modified date: 3/26/2014
--dv Notes:	
--dv History: TT#571-MD -jsobek -Allocation Audit needs to keep iterations of allocation
--dv ============================================= 
create table [dbo].[AUDIT_HEADER] ( 
	"AUDIT_HEADER_RID" int identity not null,
	"PROCESS_DATE_TIME" datetime null,
	"PROCESS_RID" int null,
	"USER_RID" int null,
	"HDR_RID" int null,
	"METHOD_RID" int null,
	"METHOD_HAS_BEEN_DELETED" char(1) null,
	"METHOD_NAME" varchar(50) null,
	"METHOD_TYPE" int null,
	"HEADER_COMPONENT_TYPE" int null,
	"ACTION_TYPE" int null,
	"PACK_OR_COLOR_NAME" varchar(250) null,
	"SIZE_NAME" varchar(250) null,
	"UNITS_ALLOCATED_BY_PROCESS" int null,
	"STORE_COUNT" int null,
	"LAST_ENTRY" bit null 
	) ON 'AUDIT'  

go
