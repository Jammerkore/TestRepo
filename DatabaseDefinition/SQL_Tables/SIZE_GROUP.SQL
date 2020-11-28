--dv =============================================
--dv Modified date: 3/26/2014
--dv Notes:	
--dv History: 
--dv =============================================  
create table [dbo].[SIZE_GROUP] ( 
	"SIZE_GROUP_RID" int identity not null,
	"SIZE_GROUP_NAME" varchar(50) not null,
	"SIZE_GROUP_DESCRIPTION" varchar(250) null,
	"WIDTH_ACROSS_IND" char(1) null)  

go
