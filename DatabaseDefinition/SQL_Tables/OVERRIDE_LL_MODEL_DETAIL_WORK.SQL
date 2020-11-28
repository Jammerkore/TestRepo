--dv =============================================
--dv Modified date: 3/26/2014
--dv Notes:	
--dv History: 
--dv ============================================= 
create table [dbo].[OVERRIDE_LL_MODEL_DETAIL_WORK] ( 
	"OLL_RID" bigint not null,
	"HN_RID" int not null,
	"VERSION_RID" int null,
	"EXCLUDE_IND" char(1) null)
go
