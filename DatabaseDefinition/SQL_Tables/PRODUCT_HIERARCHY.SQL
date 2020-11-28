--dv =============================================
--dv Modified date: 3/26/2014
--dv Notes:	
--dv Fields: PH_RID: RID is internal identifier
--dv Fields: PH_ID: ID is external identifier
--dv Fields: PH_TYPE: Type is either organizational or open
--dv History: 
--dv =============================================   
create table [dbo].[PRODUCT_HIERARCHY] ( 
	"PH_RID" int identity not null,
	"PH_ID" varchar(50) not null,
	"PH_TYPE" int not null,
	"PH_COLOR" varchar(30) not null,
	"PH_OWNER" int not null,
	"OTS_PLANLEVEL_TYPE" int null,
	"HISTORY_ROLL_OPTION" int null)  

go
