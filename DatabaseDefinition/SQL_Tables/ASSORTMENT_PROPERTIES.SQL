--dv =============================================
--dv Modified date: 3/26/2014
--dv Notes:	
--dv History: 
--dv History:  TT#2066-MD - JSmith - Ship to Date validation.  Is this how it should be working
--dv =============================================   
create table [dbo].[ASSORTMENT_PROPERTIES] ( 
	"HDR_RID" int not null,
	"RESERVE" float null,
	"RESERVE_TYPE_IND" int null,
	"SG_RID" int not null,
	"VARIABLE_TYPE" int not null,
	"VARIABLE_NUMBER" int not null,
	"INCL_ONHAND" char(1) null,
	"INCL_INTRANSIT" char(1) null,
	"INCL_SIMILAR_STORES" char(1) null,
	"INCL_COMMITTED" char(1) not null default 0,		
	"AVERAGE_BY" int not null default 0,  
	"GRADE_BOUNDARY_IND" int null,
	"CDR_RID" int not null default 1,			
	"USER_RID" int not null,
	"ANCHOR_HN_RID" int default -1 not null,		
	"LAST_PROCESS_DATETIME" datetime not null,
	"BEGIN_DAY_CDR_RID" int not null default 1
	)  

go
