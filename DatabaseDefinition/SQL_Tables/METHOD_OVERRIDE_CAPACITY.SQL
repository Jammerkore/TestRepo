--dv =============================================
--dv Modified date: 3/26/2014
--dv Notes:	
--dv History: 
--dv =============================================   
create table [dbo].[METHOD_OVERRIDE_CAPACITY] ( 
	"METHOD_RID" int not null,
	"SGL_RID" int not null,
	"EXCEED_CAPACITY" char(1) null,
	"EXCEED_BY" float null)  

go
