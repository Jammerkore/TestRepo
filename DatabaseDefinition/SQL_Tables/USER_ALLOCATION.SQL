--dv =============================================
--dv Modified date: 3/26/2014
--dv Notes:	
--dv History: 
--dv =============================================   
create table [dbo].[USER_ALLOCATION] ( 
	"USER_RID" int not null,
	"SG_RID" int null,
	"GROUP_BY_ID" int null,
	"FILTER_RID" int null,
	"VIEW_RID" int null,
	"INCLUDE_INELIGIBLE_STORES" char(1) null,
	"BEGIN_CDR_RID" int null,
	"END_CDR_RID" int null,
	"HN_RID" int null,
	"VIEW_TYPE" int null)  

go
