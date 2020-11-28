--dv =============================================
--dv Modified date: 3/26/2014
--dv Notes:	
--dv History: 
--dv =============================================   
create table [dbo].[GROUP_LEVEL_BASIS] ( 
	"METHOD_RID" int not null,
	"SGL_RID" int not null,
	"BASIS_SEQ" int not null,
	"HN_RID" int not null,
	"FV_RID" int not null,
	"CDR_RID" int not null,
	"WEIGHT" float not null,
	"INC_EXC_IND" char(1) not null,
	"TYLY_TYPE_ID" int not null,
	"MERCH_TYPE" int null,
	"MERCH_PH_RID" int null,
	"MERCH_PHL_SEQUENCE" int null,
	"MERCH_OFFSET" int null)  

go
