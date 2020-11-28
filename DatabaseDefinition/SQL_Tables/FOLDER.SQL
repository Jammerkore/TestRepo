--dv =============================================
--dv Modified date: 3/26/2014
--dv Notes:	
--dv History: 
--dv =============================================     
create table [dbo].[FOLDER] ( 
	"FOLDER_RID" int identity not null,
	"USER_RID" int not null,
	"FOLDER_ID" varchar(100) not null,
	"FOLDER_TYPE" int not null,
	"FOLDER_COLOR" varchar(30)null)  

go
