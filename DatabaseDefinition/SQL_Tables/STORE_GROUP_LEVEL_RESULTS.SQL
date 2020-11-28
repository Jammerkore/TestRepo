--dv =============================================
--dv Created date: 2/19/2015
--dv Notes:	Ties group levels to stores based on group level rid and group level version
--dv History: 2015-02-19 TT#1414-MD -jsobek -Attribute Set Filter
--dv =============================================  
create table [dbo].[STORE_GROUP_LEVEL_RESULTS] ( 
	"RESULT_RID" int identity not null,
	"SGL_RID" int not null,
	"SGL_VERSION" int not null,
	"ST_RID" int not null
	)  

go
