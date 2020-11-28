--dv ============================================= 
--dv Create date: 8/8/2013 
--dv Modified:	8/8/2013 
--dv Description: Inserts store removal analysis rows into STORE_REMOVAL_ANALYSIS for each table in the STORE_REMOVAL_TABLE_LIST
--dv				with STORE_SET_IND = 0  (Not a Store Set delete type table)
--dv ============================================= 
CREATE PROCEDURE [dbo].[MID_STORE_REMOVAL_DO_ANALYSIS] 
	@MINIMUM_DELETE_ROW_THRESHOLD int,
	@MAXIMUM_DELETE_ROW_THRESHOLD int,
	@DELETION_PROCESS_PERCENTAGE_THRESHOLD float
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM STORE_REMOVAL_ANALYSIS

	declare @analysis_sql varchar(max)
	

	declare @tableName varchar(4000)
	declare @tableListID int

	declare @C1 Cursor
	set @C1 = Cursor for select TABLE_NAME, TABLE_LIST_ID from STORE_REMOVAL_TABLE_LIST where STORE_SET_IND = 0
    
	open @C1
    fetch next from @C1 Into @tableName, @tableListID
    while ( @@FETCH_STATUS = 0 )
    begin
		set @analysis_sql = 'INSERT INTO STORE_REMOVAL_ANALYSIS (TABLE_LIST_ID, TOTAL_ROW_COUNT, ROWS_TO_DELETE_COUNT, ANALYSIS_DATE) VALUES (' + CAST(@tableListID AS varchar) + ', (SELECT SUM(p.rows) as [RowCount] FROM sys.schemas s LEFT JOIN sys.tables t ON s.schema_id = t.schema_id LEFT JOIN sys.partitions p ON t.object_id = p.object_id LEFT JOIN  sys.allocation_units a ON  p.partition_id = a.container_id WHERE    p.index_id  in(0,1) AND p.rows is not null AND a.type = 1 AND t.[name]=''' + @tableName + '''), (select count(*) FROM ' + @tableName + ' WITH (NOLOCK) WHERE ST_RID IN (SELECT ST_RID FROM STORES WITH (NOLOCK) WHERE STORE_DELETE_IND=1)), GETDATE())' 
		
		print @analysis_sql
		exec (@analysis_sql)
		fetch next from @C1 into @tableName, @tableListID
	end
	close @C1 
	deallocate @C1;

	/* store set analysis where the set is based upon store id */
	--exec [dbo].[MID_STORE_REMOVAL_DO_ANALYSIS_SGL] @MINIMUM_DELETE_ROW_THRESHOLD, @DELETION_PROCESS_PERCENTAGE_THRESHOLD

	/* Decision process */
	exec [dbo].[MID_STORE_REMOVAL_DECIDE_PROCESS] @MINIMUM_DELETE_ROW_THRESHOLD, @MAXIMUM_DELETE_ROW_THRESHOLD, @DELETION_PROCESS_PERCENTAGE_THRESHOLD

END
GO
