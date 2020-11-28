--dv =============================================
--dv Modified date: 3/26/2014
--dv Notes:	
--dv History: 
--dv =============================================   
create table [dbo].[USER_PLAN_BASIS_DETAILS] ( 
	"USER_RID" int not null,
	"BASIS_RID" int not null,
	"SEQ_ID" int not null,
	"HN_RID" int null,
	"FV_RID" int null,
	"CDR_RID" int null,
	"WEIGHT" float null,
	"IS_INCLUDED_IND" char(1) null)  

go
