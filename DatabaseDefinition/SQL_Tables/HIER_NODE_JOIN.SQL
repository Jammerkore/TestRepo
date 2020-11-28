--dv =============================================
--dv Modified date: 3/26/2014
--dv Notes:	
--dv History: 
--dv =============================================  
create table [dbo].[HIER_NODE_JOIN] ( 
	"PH_RID" int not null,
	"PARENT_HN_RID" int not null,
	--"HN_RID" int not null)  -- TT#3630 - JSmith - Delete My Hierarchy
	"HN_RID" int not null,  -- TT#3630 - JSmith - Delete My Hierarchy
	"VIRTUAL_IND" char(1) null,  -- TT#3630 - JSmith - Delete My Hierarchy
	"NODE_DELETE_IND" char(1) null)  -- TT#3630 - JSmith - Delete My Hierarchy  

go
