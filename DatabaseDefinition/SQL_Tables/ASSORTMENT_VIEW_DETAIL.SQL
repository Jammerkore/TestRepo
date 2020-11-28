--dv =============================================
--dv Modified date: 3/26/2014
--dv Notes:	
--dv History: 
--dv ============================================= 
create table [dbo].[ASSORTMENT_VIEW_DETAIL] ( 
	"VIEW_RID" int not null,
	"AXIS" int not null,
	"AXIS_SEQUENCE" int not null,
	"PROFILE_TYPE" int not null,
	"PROFILE_KEY" int not null,
	"SUMMARIZED_IND" char(1) null)  

go
