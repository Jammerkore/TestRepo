--dv =============================================
--dv Modified date: 3/26/2014
--dv Notes:	
--dv History: 
--dv ============================================= 
create table [dbo].[SIZE_CONSTRAINT_MINMAX] ( 
	"SIZE_CONSTRAINT_RID" int not null,
	"SGL_RID" int not null,
	"COLOR_CODE_RID" int not null,
	"SIZES_RID" int not null,
	"DIMENSIONS_RID" int not null,
	"ROW_TYPE_ID" int not null,
	"SIZE_CODE_RID" int not null,
	"SIZE_MIN" int null,
	"SIZE_MAX" int null,
	"SIZE_MULT" int null)  

go
