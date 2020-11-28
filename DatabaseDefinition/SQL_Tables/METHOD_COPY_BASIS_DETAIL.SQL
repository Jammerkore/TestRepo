--dv =============================================
--dv Modified date: 3/26/2014
--dv Notes:	
--dv History: 
--dv =============================================  
create table [dbo].[METHOD_COPY_BASIS_DETAIL] ( 
	"METHOD_RID" int not null,
	"SGL_RID" int not null,
	"DETAIL_SEQ" int not null,
	"HN_RID" int null,
	"FV_RID" int not null,
	"CDR_RID" int not null,
	"WEIGHT" float not null,
	"INCLUDE_EXCLUDE" int not null)  

go
