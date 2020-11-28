--dv =============================================
--dv Modified date: 3/26/2014
--dv Notes:	
--dv History: 
--dv =============================================  
create table [dbo].[MASTER_HEADER] ( 
	"SUBORD_HDR_RID" int not null,
	"MASTER_HDR_RID" int not null,
	"MASTER_COMPONENT" int null,
	"MASTER_PACK_RID" int null,
	"MASTER_BC_RID" int null,
	"SUBORD_COMPONENT" int null,
	"SUBORD_PACK_RID" int null,
	"SUBORD_BC_RID" int null)  

go
