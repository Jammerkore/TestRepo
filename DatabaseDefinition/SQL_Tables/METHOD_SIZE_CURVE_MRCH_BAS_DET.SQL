--dv =============================================
--dv Modified date: 3/26/2014
--dv Notes:	
--dv History: 
--dv =============================================  
create table [dbo].[METHOD_SIZE_CURVE_MRCH_BAS_DET] ( 
	"METHOD_RID" int not null,
	"BASIS_SEQ" int not null,
	"HN_RID" int null,
	"FV_RID" int null,
	"CDR_RID" int not null,
	"WEIGHT" float not null,
	"MERCH_TYPE" int null,
	"OLL_RID" int null,
	"CUSTOM_OLL_RID" int null) 

go
