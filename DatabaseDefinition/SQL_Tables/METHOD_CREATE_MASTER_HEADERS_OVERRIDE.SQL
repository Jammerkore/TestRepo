--dv =============================================
--dv Modified date: 4/18/2017
--dv Notes:	
--dv History: TT#1966-MD - JSmith- DC Fulfillment
--dv =============================================    
create table [dbo].[METHOD_CREATE_MASTER_HEADERS_OVERRIDE] ( 
	"METHOD_RID" int not null,
	"SEQ" int not null,
	"OVERRIDE_TYPE" char not null,
	"HEADER_FIELD" int null,
	"HCG_RID" int null,
	"OVERRIDE_VALUE" varchar(250) not null)	
   
go
