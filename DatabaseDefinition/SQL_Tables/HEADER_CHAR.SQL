--dv =============================================
--dv Modified date: 3/26/2014
--dv Notes:	
--dv History: 
--dv ============================================= 
create table [dbo].[HEADER_CHAR] ( 
	"HC_RID" int identity not null,
	"HCG_RID" int not null,
	"TEXT_VALUE" varchar(50) null,
	"DATE_VALUE" smalldatetime null,
	"NUMBER_VALUE" float null,
	"DOLLAR_VALUE" float null)  

go
