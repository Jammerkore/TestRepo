CREATE PROCEDURE [dbo].[MID_HIERARCHY_TEMP_NODES_DELETE]
AS
BEGIN
SET NOCOUNT ON

declare @key int

select *
  into #tempNodes
  from HIERARCHY_NODE 
  where PURPOSE is null
    and HN_RID not in (select PARENT_HN_RID from HIER_NODE_JOIN)

WHILE 1=1
BEGIN
  SELECT TOP 1 @key = HN_RID
    FROM #tempNodes

  IF @@rowcount = 0 BREAK

  EXEC [dbo].[SP_MID_HIERNODE_DELETE] @key

  delete from #tempNodes 
    where HN_RID = @key

END

drop table #tempNodes

END