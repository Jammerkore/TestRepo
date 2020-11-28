--dv =============================================
--dv Modified date: 3/26/2014
--dv Notes:	
--dv History: TT#1185 -JEllis -Verify ENQ before update
--dv ============================================= 
create table [dbo].[MID_ENQUEUE] ( 
	"ENQUEUE_TYPE" int not null,
	"USER_RID" int not null,
	"OWNING_THREADID" int null,
	"PH_RID" int null,
	"HN_RID" int null,
	"FV_RID" int null,
	"START_WEEK" int null,
	"END_WEEK" int null,
	"HDR_RID" int null,
	"RID" int null,                 
	"OWNING_TRANID" bigint null,    
	"ENQUEUE_TIMESTAMP" datetime default getdate() not null) 

go
