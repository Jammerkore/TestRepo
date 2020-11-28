--dv =============================================
--dv Modified date: 3/26/2014
--dv Notes:	
--dv History: 
--dv =============================================  
create table [dbo].[SIZE_ALTERNATE_MODEL] ( 
	"SIZE_ALTERNATE_RID" int identity not null,
	"SIZE_ALTERNATE_NAME" char(50) not null,
	"PRIMARY_SIZE_CURVE_RID" int not null,
	"ALTERNATE_SIZE_CURVE_RID" int not null)  

go
