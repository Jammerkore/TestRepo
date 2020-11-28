--dv =============================================
--dv Modified date: 3/26/2014
--dv Notes:	
--dv History: 
--dv ============================================= 
create table [dbo].[AUDIT_RECLASS] ( 
	"PROCESS_RID" int not null,
	"RECLASS_SEQ" int not null,
	"RECLASS_ACTION" varchar(50) null,
	"RECLASS_ITEM_TYPE" varchar(50) null,
	"RECLASS_ITEM" varchar(50) null,
	"RECLASS_COMMENT" varchar(250) null)  

go
