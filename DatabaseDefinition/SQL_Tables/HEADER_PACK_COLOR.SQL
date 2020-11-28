--dv =============================================
--dv Modified date: 3/26/2014
--dv Notes:	
--dv History: 
--dv =============================================   
create table [dbo].[HEADER_PACK_COLOR] ( 
    "HDR_PACK_RID" int not null, 
    "HDR_PC_RID" int identity(100,1) not null,
	"COLOR_CODE_RID" int not null,
	"UNITS" int not null,
	"SEQ" int null,
	"NAME" varchar(50) null,
	"DESCRIPTION"  varchar(100) null,
    "LAST_PCSZ_KEY_USED" int default 0) ON 'ALLOCATION'

go
