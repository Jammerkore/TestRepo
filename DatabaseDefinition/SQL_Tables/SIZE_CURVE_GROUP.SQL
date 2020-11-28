--dv =============================================
--dv Modified date: 3/26/2014
--dv Notes:	
--dv History: 
--dv =============================================  
create table [dbo].[SIZE_CURVE_GROUP] ( 
	"SIZE_CURVE_GROUP_RID" int identity not null,
	"SIZE_CURVE_GROUP_NAME" varchar(250) null,
	"DEFAULT_SIZE_CURVE_RID" int null,
	"DEFINED_SIZE_GROUP_RID" int null)  

go
