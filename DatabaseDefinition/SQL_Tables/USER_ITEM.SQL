--dv =============================================
--dv Modified date: 3/26/2014
--dv Notes:	
--dv History: 
--dv =============================================  
create table [dbo].[USER_ITEM] ( 
	"USER_RID" int not null,
	"ITEM_TYPE" int not null,
	"ITEM_RID" int not null,
	"OWNER_USER_RID" int null)  

go
