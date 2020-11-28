CREATE PROCEDURE [dbo].[MID_HIERARCHY_REMOVE_ALT_LEVEL]
	@PH_RID int,
	@Level int,
	@Return_Code int = 0 output
AS
BEGIN
SET NOCOUNT ON


set @Return_Code = 0
 
DECLARE  @HN_RID int,
   @level_seq int,
   @max_level int,
   @curr_level int,
   @table_schema varchar(250),
   @table_name varchar(250),
   @col_name varchar(250),
   @key int,
   @newKey int,
   @return_value int,
   @count int,
   @sql NVARCHAR(4000)

select @HN_RID = HN_RID from HIERARCHY_NODE where HOME_PH_RID = @PH_RID and HOME_LEVEL = 0

exec SP_MID_GET_BRANCH_SIZE @HN_RID, '0', @max_level output

SET @level_seq = @Level

-- delete nodes in hierarchy at level
select *
  into #nodesToDelete
  from HIERARCHY_NODE where HOME_PH_RID = @PH_RID and HOME_LEVEL = @level_seq

set @count = 0
WHILE 1=1
BEGIN
  SELECT TOP 1 @key = HN_RID
    FROM #nodesToDelete

  IF @@rowcount = 0 BREAK

  set @count = @count + 1
  
  select @newKey = PARENT_HN_RID from HIER_NODE_JOIN where HN_RID = @key and PH_RID = @PH_RID

  update HIER_NODE_JOIN set PARENT_HN_RID = @newKey where PARENT_HN_RID = @key and PH_RID = @PH_RID

  EXEC [dbo].[SP_MID_HIERNODE_DELETE] @key

  delete from #nodesToDelete 
    where HN_RID = @key

END

SELECT distinct s.TABLE_SCHEMA as TableSchema,
	         t.name AS TableName, 
             c.name AS ColumnName	
	  into #renumber		
      FROM   sys.tables AS t
             INNER JOIN sys.columns c
               ON t.OBJECT_ID = c.OBJECT_ID
             INNER JOIN INFORMATION_SCHEMA.TABLES s
               ON t.name = s.TABLE_NAME
      WHERE  c.name LIKE '%MERCH_OFFSET%'
	     or  c.name LIKE '%LEVEL_OFFSET%'
		 or  c.name LIKE '%PHL_OFFSET%'
		 or  c.name LIKE '%FROM_OFFSET%'
		 or  c.name LIKE '%TO_OFFSET%'

-- renumber level references  
select HN_RID
      into #nodes
      from HIERARCHY_NODE with (nolock) where HOME_PH_RID = @PH_RID

 set @curr_level = @level_seq + 1
 while @curr_level <= @max_level
 begin
    -- renumber hierarchy records
    update HIERARCHY_NODE set HOME_LEVEL = @curr_level - 1 where HOME_PH_RID = @PH_RID and HOME_LEVEL = @curr_level
    
	-- renumber all references to the level
    update [dbo].[GROUP_LEVEL_BASIS] set MERCH_OFFSET = @curr_level - 1 where MERCH_OFFSET = @curr_level and MERCH_PH_RID = @PH_RID
    update [dbo].[METHOD_COPY_FORECAST] set FROM_LEVEL_OFFSET = @curr_level - 1 where FROM_LEVEL_OFFSET = @curr_level and HN_RID in (select HN_RID from #nodes)
    update [dbo].[METHOD_COPY_FORECAST] set TO_LEVEL_OFFSET = @curr_level - 1 where TO_LEVEL_OFFSET = @curr_level and HN_RID in (select HN_RID from #nodes)
    update [dbo].[METHOD_EXPORT] set LOW_LEVEL_OFFSET = @curr_level - 1 where LOW_LEVEL_OFFSET = @curr_level and HN_RID in (select HN_RID from #nodes)
    update [dbo].[METHOD_GLOBAL_LOCK] set FROM_LEVEL_OFFSET = @curr_level - 1 where FROM_LEVEL_OFFSET = @curr_level and HN_RID in (select HN_RID from #nodes)
    update [dbo].[METHOD_GLOBAL_LOCK] set TO_LEVEL_OFFSET = @curr_level - 1 where TO_LEVEL_OFFSET = @curr_level and HN_RID in (select HN_RID from #nodes)
    update [dbo].[METHOD_GLOBAL_UNLOCK] set FROM_LEVEL_OFFSET = @curr_level - 1 where FROM_LEVEL_OFFSET = @curr_level and HN_RID in (select HN_RID from #nodes)
    update [dbo].[METHOD_GLOBAL_UNLOCK] set TO_LEVEL_OFFSET = @curr_level - 1 where TO_LEVEL_OFFSET = @curr_level and HN_RID in (select HN_RID from #nodes)
    update [dbo].[METHOD_MATRIX] set LOW_LEVEL_OFFSET = @curr_level - 1 where LOW_LEVEL_OFFSET = @curr_level and HN_RID in (select HN_RID from #nodes)
    update [dbo].[METHOD_ROLLUP_BASIS_DETAIL] set FROM_LEVEL_OFFSET = @curr_level - 1 where FROM_LEVEL_OFFSET = @curr_level and METHOD_RID in (select METHOD_RID from METHOD_ROLLUP where HN_RID in (select HN_RID from #nodes))
    update [dbo].[METHOD_ROLLUP_BASIS_DETAIL] set TO_LEVEL_OFFSET = @curr_level - 1 where TO_LEVEL_OFFSET = @curr_level and METHOD_RID in (select METHOD_RID from METHOD_ROLLUP where HN_RID in (select HN_RID from #nodes))
    update [dbo].[METHOD_SPREAD_FORECAST] set FROM_LEVEL_OFFSET = @curr_level - 1 where FROM_LEVEL_OFFSET = @curr_level and HN_RID in (select HN_RID from #nodes)
    update [dbo].[METHOD_SPREAD_FORECAST] set LOW_LEVEL_OFFSET = @curr_level - 1 where LOW_LEVEL_OFFSET = @curr_level and HN_RID in (select HN_RID from #nodes)
    update [dbo].[METHOD_SPREAD_FORECAST] set TO_LEVEL_OFFSET = @curr_level - 1 where TO_LEVEL_OFFSET = @curr_level and HN_RID in (select HN_RID from #nodes)
    update [dbo].[NODE_SIZE_CURVE_CRITERIA_DETAIL] set PHL_OFFSET = @curr_level - 1 where PHL_OFFSET = @curr_level and PH_RID = @PH_RID
    update [dbo].[NODE_SIZE_CURVE_TOLERANCE] set PHL_OFFSET = @curr_level - 1 where PHL_OFFSET = @curr_level and PH_RID = @PH_RID
    update [dbo].[OTS_PLAN] set LOW_LEVEL_OFFSET = @curr_level - 1 where LOW_LEVEL_OFFSET = @curr_level and PLAN_HN_RID in (select HN_RID from #nodes)
    update [dbo].[OVERRIDE_LL_MODEL_HEADER] set HIGH_LEVEL_OFFSET = @curr_level - 1 where HIGH_LEVEL_OFFSET = @curr_level and HN_RID in (select HN_RID from #nodes)
    update [dbo].[OVERRIDE_LL_MODEL_HEADER] set LOW_LEVEL_OFFSET = @curr_level - 1 where LOW_LEVEL_OFFSET = @curr_level and HN_RID in (select HN_RID from #nodes)
    update [dbo].[TASK_ROLLUP] set FROM_OFFSET = @curr_level - 1 where FROM_OFFSET = @curr_level and HN_RID in (select HN_RID from #nodes)
    update [dbo].[TASK_ROLLUP] set TO_OFFSET = @curr_level - 1 where TO_OFFSET = @curr_level and HN_RID in (select HN_RID from #nodes)
    update [dbo].[USER_PLAN] set LOW_LEVEL_OFFSET = @curr_level - 1 where LOW_LEVEL_OFFSET = @curr_level and CHAIN_HN_RID in (select HN_RID from #nodes)
	update [dbo].[USER_PLAN] set LOW_LEVEL_OFFSET = @curr_level - 1 where LOW_LEVEL_OFFSET = @curr_level and STORE_HN_RID in (select HN_RID from #nodes)
 
    set @curr_level = @curr_level + 1
 end

return @Return_Code

END
