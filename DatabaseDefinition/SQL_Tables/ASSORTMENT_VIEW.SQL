--dv =============================================
--dv Modified date: 12/18/2014
--dv Notes:	
--dv History: 03-18-2014 - TT#1077-MD -stodd - Group Allocation  View
--dv History: 12-18-2014 - TT#4247-stodd - Add SG_RID
--dv =============================================  
create table [dbo].[ASSORTMENT_VIEW] ( 
	"VIEW_RID" int identity not null,
	"USER_RID" int not null,
	"VIEW_ID" varchar(250) not null,
	"GROUP_BY_ID" int not null default 803051,
	"SG_RID" int not null default 1,
	"ASSORTMENT_VIEW_TYPE" int not null default 1
	)  

go
