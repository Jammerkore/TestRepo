--dv =============================================
--dv Modified date: 3/26/2014
--dv Notes:	
--dv History: 
--dv ============================================= 
create table [dbo].[HEADER_PACK_COLOR_SIZE] ( 
	"HDR_PACK_RID" int not null, 
	"HDR_PC_RID" int not null,
	"HDR_PCSZ_KEY"  int not null, 
	"SIZE_CODE_RID" int not null, 
	"SEQ" int not null,
	"UNITS" int not null) ON 'ALLOCATION'  

go
