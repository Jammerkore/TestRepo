--dv =============================================
--dv Modified date: 3/26/2014
--dv Notes:	
--dv History: 
--dv =============================================  
create table [dbo].[STORE_CHAR_GROUP] ( 
	"SCG_RID" int identity not null,
	"SCG_ID" varchar(50) not null,
	"SCG_TYPE" int not null,
	"SCG_LIST_IND" char(1) not null)  

go
