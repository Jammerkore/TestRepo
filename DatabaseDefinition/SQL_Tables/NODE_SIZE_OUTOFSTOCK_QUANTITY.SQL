--dv =============================================
--dv Modified date: 3/26/2014
--dv Notes:	
--dv History: 
--dv =============================================   
create table [dbo].[NODE_SIZE_OUTOFSTOCK_QUANTITY] ( 
	"HN_RID" int not null,
	"SGL_RID" int not null,
	"COLOR_CODE_RID" int not null,
	"SIZES_RID" int not null,
	"DIMENSIONS_RID" int not null,
	"ROW_TYPE_ID" int not null,
	"SIZE_CODE_RID" int not null,
	"OOS_QUANTITY" int null)  

go
