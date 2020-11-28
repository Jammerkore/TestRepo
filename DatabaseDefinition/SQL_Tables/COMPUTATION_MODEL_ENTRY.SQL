--dv =============================================
--dv Modified date: 3/26/2014
--dv Notes:	
--dv History: 
--dv =============================================  
create table [dbo].[COMPUTATION_MODEL_ENTRY] ( 
	"COMP_MODEL_RID" int not null,
	"COMP_MODEL_SEQUENCE" int not null,
	"COMP_TYPE" int not null,
	"FV_RID" int null,
	"CHANGE_VARIABLE" int not null,
	"PRODUCT_LEVEL" int null)  

go
