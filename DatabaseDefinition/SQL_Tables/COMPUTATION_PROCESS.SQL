--dv =============================================
--dv Modified date: 3/26/2014
--dv Notes:	
--dv History: 
--dv =============================================  
create table [dbo].[COMPUTATION_PROCESS] ( 
	"COMPUTATION_RID" int identity not null,
	"PROCESS_RID" int not null,
	"COMPUTATION_TYPE" int not null,
	"STATUS_CODE" int not null,
	"START_TIME" datetime null,
	"STOP_TIME" datetime null)  

go
