--dv =============================================
--dv Modified date: 3/26/2014
--dv Notes:	
--dv History: 
--dv =============================================    
create table [dbo].[ROLLUP_PROCESS] ( 
	"ROLLUP_RID" int identity not null,
	"PROCESS_RID" int not null,
	"ROLLUP_TYPE" int not null,
	"STATUS_CODE" int not null,
	"BATCH_NUMBER" int not null,
	"START_TIME" datetime not null,
	"STOP_TIME" datetime null)  

go
