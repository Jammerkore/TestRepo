--dv =============================================
--dv Modified date: 3/26/2014
--dv Notes:	
--dv History: 
--dv =============================================   
create table [dbo].[HEADER_CHAR_GROUP] ( 
	"HCG_RID" int identity not null,
	"HCG_ID" varchar(50) not null,
	"HCG_TYPE" int not null,
	"HCG_LIST_IND" char(1) not null,
	"HCG_PROTECT_IND" char(1) default '0' not null)  

go
