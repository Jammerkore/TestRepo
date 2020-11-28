--dv =============================================
--dv Modified date: 3/26/2014
--dv Notes:	
--dv History: 
--dv =============================================   
create table [dbo].[COMPONENT_CRITERIA] ( 
	"COMP_CRIT_RID" int identity not null,
	"EXPRESSION" varchar(250) null,
	"SIZE_CODE_RID" int null,
	"COLOR_CODE_RID" int null,
	"PACK_NAME" varchar(100) null)  

go
