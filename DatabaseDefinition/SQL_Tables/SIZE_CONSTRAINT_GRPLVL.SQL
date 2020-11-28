--dv =============================================
--dv Modified date: 3/26/2014
--dv Notes:	
--dv History: 
--dv =============================================  
create table [dbo].[SIZE_CONSTRAINT_GRPLVL] ( 
	"SIZE_CONSTRAINT_RID" int not null,
	"SGL_RID" int not null,
	"SIZE_MIN" int null,
	"SIZE_MAX" int null,
	"SIZE_MULT" int null,
	"ROW_TYPE_ID" int not null)  

go
