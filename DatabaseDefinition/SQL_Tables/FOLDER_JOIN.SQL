--dv =============================================
--dv Modified date: 3/26/2014
--dv Notes:	
--dv History: 
--dv =============================================     
create table [dbo].[FOLDER_JOIN] ( 
	"PARENT_FOLDER_RID" int not null,
	"CHILD_ITEM_RID" int not null,
	"CHILD_ITEM_TYPE" int not null)  

go
