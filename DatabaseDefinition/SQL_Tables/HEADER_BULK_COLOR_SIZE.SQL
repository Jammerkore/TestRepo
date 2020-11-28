--dv =============================================
--dv Modified date: 3/26/2014
--dv Notes:	
--dv History: 
--dv ============================================= 
create table [dbo].[HEADER_BULK_COLOR_SIZE] ( 
	"HDR_RID" int not null,
	"HDR_BC_RID" int not null,
	"HDR_BCSZ_KEY" int not null,
	"SIZE_CODE_RID" int not null,
	"UNITS" int not null,
	"MULTIPLE" int not null,
	"MINIMUM" int not null,
	"MAXIMUM" int not null,
	"RESERVE_UNITS" int null,
	"SEQ" int null) ON 'ALLOCATION'

go
