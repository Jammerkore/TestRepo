/*RELEASE 8.6.0.05 12-21-2016 TT#5393 - JSmith - Store Intransit Rolled to Alternate Hierarchy Nodes */
set nocount on
declare @Nodes table (HN_RID int)

insert into @Nodes
select distinct(si.HN_RID)
from STORE_INTRANSIT si with (nolock)
join HIERARCHY_NODE hn with (nolock) on hn.HN_RID = si.HN_RID
where hn.HOME_PH_RID <> (select PH_RID from PRODUCT_HIERARCHY where PH_TYPE = 800000)

delete from STORE_INTRANSIT where HN_RID in (select HN_RID from @Nodes)

delete from @Nodes

insert into @Nodes
select distinct(si.HN_RID)
from STORE_EXTERNAL_INTRANSIT si with (nolock)
join HIERARCHY_NODE hn with (nolock) on hn.HN_RID = si.HN_RID
where hn.HOME_PH_RID <> (select PH_RID from PRODUCT_HIERARCHY where PH_TYPE = 800000)

delete from STORE_EXTERNAL_INTRANSIT where HN_RID in (select HN_RID from @Nodes)

GO
