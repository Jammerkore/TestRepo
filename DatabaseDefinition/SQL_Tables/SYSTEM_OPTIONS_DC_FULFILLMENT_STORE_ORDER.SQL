--dv =============================================
--dv Modified date: 5/8/2017
--dv Notes:	
--dv History: TT#1966-MD - AGallagher- DC Fulfillment
--dv =============================================    
create table [dbo].[SYSTEM_OPTIONS_DC_FULFILLMENT_STORE_ORDER] ( 
	"SYSTEM_OPTION_RID" int not null,
	"SEQ" int not null,
	"DIST_CENTER" varchar(50) not null,
	"SCG_RID" int not null)	
   
go
