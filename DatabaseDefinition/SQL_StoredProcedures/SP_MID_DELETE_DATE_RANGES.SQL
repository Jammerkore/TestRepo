CREATE PROCEDURE [dbo].[SP_MID_DELETE_DATE_RANGES] 
AS
BEGIN
	DECLARE @t TABLE (
		ForeignKey VARCHAR(100),
		TableName VARCHAR(100),
		ColumnName VARCHAR(100),
		ReferenceTableName VARCHAR(100),
		ReferenceColumnName VARCHAR(100)
		)
	DECLARE @t2 TABLE (
		TableName VARCHAR(100),
		ColumnName VARCHAR(100)
		)

	CREATE TABLE #RIDS (RID INT)

	INSERT INTO #RIDS
	SELECT CDR_RID
	FROM dbo.CALENDAR_DATE_RANGE
	WHERE CDR_NAME IS NULL
		OR CDR_NAME = ''

	-- get tables with foreign key references
	INSERT INTO @t
	SELECT f.NAME AS ForeignKey,
		OBJECT_NAME(f.parent_object_id) AS TableName,
		COL_NAME(fc.parent_object_id, fc.parent_column_id) AS ColumnName,
		OBJECT_NAME(f.referenced_object_id) AS ReferenceTableName,
		COL_NAME(fc.referenced_object_id, fc.referenced_column_id) AS ReferenceColumnName
	FROM sys.foreign_keys AS f
	INNER JOIN sys.foreign_key_columns AS fc ON f.OBJECT_ID = fc.constraint_object_id
	WHERE OBJECT_NAME(f.referenced_object_id) = 'CALENDAR_DATE_RANGE'
		AND COL_NAME(fc.referenced_object_id, fc.referenced_column_id) = 'CDR_RID'
	ORDER BY TableName,
		ColumnName

	-- get tables with specific named columns	
	INSERT INTO @t
	SELECT ' ' AS ForeignKey,
		t.NAME AS TableName,
		c.NAME AS ColumnName,
		' ' AS ReferenceTableName,
		' ' AS ReferenceColumnName
	FROM sys.tables AS t
	INNER JOIN sys.columns c ON t.OBJECT_ID = c.OBJECT_ID
	WHERE c.NAME LIKE '%CDR_RID%'
		OR c.NAME LIKE '%CDR%'
	ORDER BY TableName,
		ColumnName;

	DECLARE @tableName VARCHAR(100)
	DECLARE @colName VARCHAR(100)
	DECLARE @cmd NVARCHAR(200)
	DECLARE @count INT
	DECLARE @parmDefinition NVARCHAR(500)
	DECLARE @prevTableName VARCHAR(100)
	DECLARE @prevColName VARCHAR(100)

	DECLARE C1 CURSOR FORWARD_ONLY
	FOR
	SELECT TableName,
		ColumnName
	FROM @t
	ORDER BY TableName,
		ColumnName

	OPEN C1

	SET @prevTableName = ' '
	SET @prevColName = ' '

	FETCH NEXT
	FROM C1
	INTO @tableName,
		@colName

	WHILE @@FETCH_STATUS = 0
	BEGIN
		IF @tableName <> @prevTableName
			OR @colName <> @prevColName
		BEGIN
			INSERT INTO @t2
			VALUES (
				@tableName,
				@colName
				)
		END

		SET @prevTableName = @tableName
		SET @prevColName = @colName

		FETCH NEXT
		FROM C1
		INTO @tableName,
			@colName
	END

	CLOSE C1

	DEALLOCATE C1

	DECLARE C2 CURSOR FORWARD_ONLY
	FOR
	SELECT TableName,
		ColumnName
	FROM @t2

	OPEN C2

	FETCH NEXT
	FROM C2
	INTO @tableName,
		@colName

	WHILE @@FETCH_STATUS = 0
	BEGIN
		IF @tableName != 'CALENDAR_DATE_RANGE'
		BEGIN
			SET @cmd = 'delete from #RIDS where RID in (select ' + @colName + ' from ' + @tableName + ' with (nolock))'

			EXEC (@cmd)
		END

		FETCH NEXT
		FROM C2
		INTO @tableName,
			@colName
	END

	CLOSE C2

	DEALLOCATE C2

	DELETE
	FROM CALENDAR_DATE_RANGE
	WHERE CDR_RID IN (
			SELECT RID
			FROM #RIDS
			)

	DROP TABLE #RIDS

	SELECT @@ROWCOUNT;
END
GO


