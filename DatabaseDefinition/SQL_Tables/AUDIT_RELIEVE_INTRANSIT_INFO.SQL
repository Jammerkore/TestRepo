--dv =============================================
--dv Modified date: 3/26/2014
--dv Notes:	
--dv History: 
--dv ============================================= 
create table [dbo].[AUDIT_RELIEVE_INTRANSIT_INFO] ( 
	"PROCESS_RID" int not null,
	"RECS_READ" int not null,
	"RECS_ACCEPTED" int not null,
	"RECS_WITH_ERRORS" int not null) ON 'AUDIT'  

go
