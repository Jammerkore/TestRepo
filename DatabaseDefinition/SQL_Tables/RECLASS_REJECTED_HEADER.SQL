--dv =============================================
--dv Modified date: 3/26/2014
--dv Notes:	
--dv History: TT#349-MD -Jsmith -Expand HDR_ID in SP_MID_ENQUEUE_HEADERS stored procedure from 32 to 100
--dv =============================================     
create table [dbo].[RECLASS_REJECTED_HEADER] ( 
	"PROCESS_RID" int not null,
	"HDR_RID" int not null,
	"HDR_ID" varchar(100) not null,       
	"HDR_STATUS" varchar(50) not null) 

go
