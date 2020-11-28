--dv =============================================
--dv Create date: 5/8/2017
--dv Notes:	
--dv History: TT#1966-MD - JSmith - DC Fulfillment
--dv =============================================  
create table [dbo].[HEADER_PACK_ASSOCIATION] ( 
	"HDR_PACK_RID" int not null,
	"SEQ" int not null,
	"HDR_RID" int not null,
	"ASSOCIATED_PACK_RID" int null) ON 'ALLOCATION'  

go
