--dv =============================================
--dv Modified date: 3/26/2014
--dv Notes:	
--dv History: 
--dv ============================================= 
create table [dbo].[SIZE_CONSTRAINT_MODEL] ( 
	"SIZE_CONSTRAINT_RID" int identity not null,
	"SIZE_CONSTRAINT_NAME" char(50) not null,
	"SIZE_GROUP_RID" int null,
	"SIZE_CURVE_GROUP_RID" int null,
	"SG_RID" int not null)  

go
