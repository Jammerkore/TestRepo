--dv =============================================
--dv Modified date: 3/26/2014
--dv Notes:	
--dv History: 
--dv =============================================    
create table [dbo].[FOLDER_SHORTCUT] ( 
	"PARENT_FOLDER_RID" int not null,
	"CHILD_SHORTCUT_RID" int not null,
	"CHILD_SHORTCUT_TYPE" int not null)  

go
