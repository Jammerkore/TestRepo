CREATE PROCEDURE [dbo].[MID_HIERARCHY_ADD_ORG_LEVEL]
	@Level_ID VARCHAR(50),
	@After_Level VARCHAR(50),
	@Return_Code int = 0 output
AS
BEGIN
SET NOCOUNT ON

 -- @Return_Code values
 --  1 : Organization hierarchy not defined
 --  2 : New level ID already defined
 --  3 : Add after level not found
 --  4 : Cannot add level below style level

set @Return_Code = 0
 
DECLARE  @main_PH_RID int,
   @level_seq int,
   @level_type int,
   @max_level int,
   @curr_level int,
   @table_schema varchar(250),
   @constraint_name varchar(250),
   @table_name varchar(250),
   @col_name varchar(250),
   @referenced_col_name varchar(250),
   @PH_RID_Name varchar(250),
   @PHL_SEQUENCE_Name varchar(250),
   @NewID varchar(50),
   @key int,
   @newKey int,
   @return_value int,
   @count int,
   @sql NVARCHAR(4000)

-- get key of organizational hierarchy
set @main_PH_RID = -1
SELECT @main_PH_RID = PH_RID FROM PRODUCT_HIERARCHY where PH_TYPE = 800000
if @main_PH_RID < 0
begin
    set @Return_Code = 1
    return @Return_Code
end

-- validate new level ID
set @level_seq = -1
SELECT @level_seq = PHL_SEQUENCE FROM PRODUCT_HIERARCHY_LEVELS where PH_RID = @main_PH_RID and PHL_ID = @Level_ID
if @level_seq > 0
begin
    set @Return_Code = 2
    return @Return_Code
end

-- validate add after level
set @level_seq = -1
SELECT @level_seq = PHL_SEQUENCE, @level_type = PHL_TYPE  FROM PRODUCT_HIERARCHY_LEVELS where PH_RID = @main_PH_RID and PHL_ID = @After_Level
if @level_seq < 0
begin
    -- check if hierarchy level
	SELECT * from PRODUCT_HIERARCHY where PH_RID = @main_PH_RID and PH_ID = @After_Level
	if @@rowcount > 0
	begin
	   set @level_seq = 0
	end
	else
	begin
       set @Return_Code = 3
       return @Return_Code
	end
end
else if @level_type <> 800200
begin
    set @Return_Code = 4
    return @Return_Code
end

SELECT @max_level = max(PHL_SEQUENCE)  FROM PRODUCT_HIERARCHY_LEVELS where PH_RID = @main_PH_RID

-- drop constraints
SELECT f.name AS ConstraintName,
OBJECT_NAME(f.parent_object_id) AS TableName,
COL_NAME(fc.parent_object_id,
fc.parent_column_id) AS ColumnName,
OBJECT_NAME (f.referenced_object_id) AS ReferenceTableName,
COL_NAME(fc.referenced_object_id,
fc.referenced_column_id) AS ReferenceColumnName,
s.TABLE_SCHEMA as TableSchema,
kcu.ORDINAL_POSITION as OrdinalPosition
into #constraints
FROM sys.foreign_keys AS f
INNER JOIN sys.foreign_key_columns AS fc
ON f.OBJECT_ID = fc.constraint_object_id
inner join INFORMATION_SCHEMA.TABLE_CONSTRAINTS s
on f.name = s.CONSTRAINT_NAME
inner join INFORMATION_SCHEMA.KEY_COLUMN_USAGE kcu
on f.name = kcu.CONSTRAINT_NAME and COL_NAME(fc.parent_object_id, fc.parent_column_id) = kcu.COLUMN_NAME
where OBJECT_NAME (f.referenced_object_id) = 'PRODUCT_HIERARCHY_LEVELS'

select distinct ConstraintName, TableName, TableSchema 
into #drop
from #constraints

WHILE 1=1
BEGIN
 SET @sql = (SELECT TOP 1 'ALTER TABLE [' + TableSchema + '].['+ TableName
 + '] NOCHECK CONSTRAINT [' + ConstraintName + ']' FROM #drop)
 IF @sql IS NULL BREAK
 --print @sql
 EXEC (@sql)
 delete top(1) from #drop
END

select distinct TableSchema, TableName, ColumnName 
     into #renumber
     from #constraints
     where ReferenceColumnName = 'PHL_SEQUENCE'

-- get columns without referential integrity
insert into #renumber
SELECT distinct s.TABLE_SCHEMA as TableSchema,
	         t.name AS TableName, 
             c.name AS ColumnName		
      FROM   sys.tables AS t
             INNER JOIN sys.columns c
               ON t.OBJECT_ID = c.OBJECT_ID
             INNER JOIN INFORMATION_SCHEMA.TABLES s
               ON t.name = s.TABLE_NAME
      WHERE  c.name LIKE '%PHL_SEQ%'
	     or  c.name LIKE '%LEVEL_SEQ%'

-- renumber level references
 set @curr_level = @max_level
 while @curr_level > @level_seq
 begin
    -- renumber hierarchy records
    update PRODUCT_HIERARCHY_LEVELS set PHL_SEQUENCE = @curr_level + 1 where PHL_SEQUENCE = @curr_level
    update HIERARCHY_NODE set HOME_LEVEL = @curr_level + 1 where HOME_PH_RID = @main_PH_RID and HOME_LEVEL = @curr_level
    -- renumber all references to the level using the constraint definitions
    select distinct TableSchema, TableName, ColumnName 
      into #renumber2
      from #renumber
 
    WHILE 1=1
    BEGIN
      SELECT TOP 1 @table_schema = TableSchema, @table_name = TableName, @col_name = ColumnName
            FROM #renumber2
      IF @@rowcount = 0 BREAK
      
      SET @sql = 'update [' + @table_schema + '].['+ @table_name
      + '] set ' + @col_name + ' = ' + CONVERT(nvarchar, (@curr_level + 1))  + ' where ' + @col_name + ' = ' + CONVERT(nvarchar, @curr_level) 
      
      --print @sql
      EXEC (@sql)
      delete from #renumber2
        where TableSchema = @table_schema
          and TableName = @table_name
          and ColumnName = @col_name 
     END
     drop table #renumber2
 
    set @curr_level = @curr_level - 1
 end
 
 --insert the new level in the hierarchy definition
 INSERT INTO PRODUCT_HIERARCHY_LEVELS(PH_RID, PHL_SEQUENCE, PHL_ID, PHL_TYPE, PHL_COLOR,
                                      LENGTH_TYPE, REQUIRED_SIZE, SIZE_RANGE_FROM, SIZE_RANGE_TO, OTS_PLANLEVEL_TYPE, PHL_DISPLAY_OPTION_ID, PHL_ID_FORMAT,
                                      PURGE_DAILY_HISTORY_TIMEFRAME, PURGE_DAILY_HISTORY, PURGE_WEEKLY_HISTORY_TIMEFRAME, PURGE_WEEKLY_HISTORY,
                                      PURGE_PLANS_TIMEFRAME, PURGE_PLANS) 
     VALUES (@main_PH_RID, @level_seq + 1, @Level_ID, 800200, 'default',
             800100, 0, 0, 0, 800600, 800703, 800900,
             800950, null, 800950, null,
             800950, null)
			
-- add dummy nodes in hierarchy at new level
select *
  into #parentNodes
  from HIERARCHY_NODE where HOME_PH_RID = @main_PH_RID and HOME_LEVEL = @level_seq

set @count = 0
WHILE 1=1
BEGIN
  SELECT TOP 1 @key = HN_RID
    FROM #parentNodes

  IF @@rowcount = 0 BREAK

  -- find ID
  WHILE 1=1
  BEGIN
      set @count = @count + 1
      set @NewID = @Level_ID + CONVERT(nvarchar, @count)
	  if not exists (select HN_RID from BASE_NODE where BN_ID = @NewID) BREAK
  END

  set @curr_level = @level_seq + 1
  -- set purpose to null so added nodes can be easily identified.
  EXEC [dbo].[SP_MID_HIERNODE_BASE_INSERT] @main_PH_RID, @key, @main_PH_RID, @curr_level, 800200, 800600,
              '0', null, null, null, null, null, null, null, '0', null, @NewID, @NewID, @NewID, 800300, @newKey output

  update HIER_NODE_JOIN set PARENT_HN_RID = @newKey where PARENT_HN_RID = @key and HN_RID <> @newKey

  delete from #parentNodes 
    where HN_RID = @key

END

-- put constraints back
set @PH_RID_Name = null
set @PHL_SEQUENCE_Name = null
WHILE 1=1
BEGIN
  SELECT TOP 1 @table_schema = TableSchema, @table_name = TableName, @constraint_name = ConstraintName,
       @col_name = ColumnName, @referenced_col_name = ReferenceColumnName
    FROM #constraints
    order by ConstraintName, OrdinalPosition

  IF @@rowcount = 0 BREAK
 
  if @referenced_col_name = 'PH_RID'
  begin
     set @PH_RID_Name = @col_name
  end
  else
  begin
     set @PHL_SEQUENCE_Name = @col_name
  end

  if @PH_RID_Name is not null
   and @PHL_SEQUENCE_Name is not null
  begin
    SET @sql = 'ALTER TABLE [' + @table_schema + '].['+ @table_name
    + '] CHECK CONSTRAINT [' + @constraint_name + ']'
     
    --print @sql
    EXEC (@sql)
    set @PH_RID_Name = null
    set @PHL_SEQUENCE_Name = null
  end

  delete from #constraints 
    where TableSchema = @table_schema
      and TableName = @table_name
      and ColumnName = @col_name 
     and ReferenceColumnName = @referenced_col_name

END

return @Return_Code

END
