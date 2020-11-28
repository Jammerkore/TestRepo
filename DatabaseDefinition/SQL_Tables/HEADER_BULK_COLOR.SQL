--dv =============================================
--dv Modified date: 3/26/2014
--dv Notes:	
--dv History: TT#246-MD -Jellis -AnF VSW In Store Minimums
--dv =============================================    
create table [dbo].[HEADER_BULK_COLOR] ( 
	"HDR_RID" int not null, 
    "HDR_BC_RID" int identity(100,1) not null,
	"COLOR_CODE_RID" int not null,
	"UNITS" int not null,
	"MULTIPLE" int not null,
	"MINIMUM" int not null,
	"MAXIMUM" int not null,
	"RESERVE_UNITS" int null,
	"SEQ" int null,
	"NAME" varchar(50) null,
	"DESCRIPTION" varchar(100) null,
    "ASRT_BC_RID" int null, 
    "LAST_BCSZ_KEY_USED" int default 0,  
	"COLOR_STATUS_FLAGS" int null      
	) ON 'ALLOCATION'                 

go
