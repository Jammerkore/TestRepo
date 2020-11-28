--dv =============================================
--dv Modified date: 3/26/2014
--dv Notes:	
--dv History: 
--dv =============================================  
create table [dbo].[SIZE_CODE] ( 
	"SIZE_CODE_RID" int identity not null,
	"SIZE_CODE_ID" varchar(50) not null,
	"SIZE_CODE_PRIMARY" varchar(100) null,
	"SIZE_CODE_SECONDARY" varchar(100) null,
	"SIZE_CODE_PRODUCT_CATEGORY" varchar(100) null)  

go
