DECLARE @RecoveryModel VARCHAR(10)
DECLARE @ConstraintTable TABLE (ForeignKey VARCHAR(100), TableName VARCHAR(100), ColumnName VARCHAR(100), ReferenceTableName VARCHAR(100), ReferenceColumnName VARCHAR(100))
DECLARE @TableName VARCHAR(100)
DECLARE @ColName VARCHAR(100)
DECLARE @Constraint VARCHAR(100)
DECLARE @Command NVARCHAR(200)
DECLARE @RowsToDelete INT
DECLARE @Count int

-- Store current Recover Mode and set Recovery Mode to "SIMPLE"

SELECT @RecoveryModel = recovery_model_desc
	FROM sys.databases
	WHERE name = db_name()

SET @Command = 'ALTER DATABASE ' +  db_name() + ' SET RECOVERY SIMPLE'
EXEC (@Command)

-- Remove orphaned SIZE_CURVE records

SELECT DEFAULT_SIZE_CURVE_RID INTO #DEFSZCRVTEMP FROM SIZE_CURVE_GROUP
	WHERE DEFAULT_SIZE_CURVE_RID IS NOT NULL

SELECT SC.SIZE_CURVE_RID INTO #SZCRVTEMP FROM SIZE_CURVE SC
	LEFT OUTER JOIN SIZE_CURVE_GROUP_JOIN SCGJ ON SCGJ.SIZE_CURVE_RID = SC.SIZE_CURVE_RID
	WHERE SCGJ.SIZE_CURVE_GROUP_RID is NULL AND
		SC.SIZE_CURVE_RID NOT IN (SELECT DEFAULT_SIZE_CURVE_RID FROM #DEFSZCRVTEMP)

INSERT INTO @ConstraintTable
    SELECT f.name AS ForeignKey,
           OBJECT_NAME(f.parent_object_id) AS TableName,
           COL_NAME(fc.parent_object_id, fc.parent_column_id) AS ColumnName,
           OBJECT_NAME (f.referenced_object_id) AS ReferenceTableName,
           COL_NAME(fc.referenced_object_id, fc.referenced_column_id) AS ReferenceColumnName
        FROM sys.foreign_keys AS f
        INNER JOIN sys.foreign_key_columns AS fc ON f.OBJECT_ID = fc.constraint_object_id
        WHERE OBJECT_NAME (f.referenced_object_id) = 'SIZE_CURVE'
            and COL_NAME(fc.referenced_object_id, fc.referenced_column_id) = 'SIZE_CURVE_RID'
        ORDER BY TableName, ColumnName

DECLARE C2 CURSOR FORWARD_ONLY FOR
    SELECT ForeignKey, TableName, ColumnName
    FROM @ConstraintTable

-- Disable constraint checking on all foreign key tables

OPEN C2

FETCH NEXT FROM C2 INTO @Constraint, @TableName, @ColName

WHILE @@FETCH_STATUS = 0
BEGIN
    SET @Command = 'ALTER TABLE ' + @TableName + ' NOCHECK CONSTRAINT ' + @Constraint
    EXEC sp_executesql @Command
    FETCH NEXT FROM C2 INTO @Constraint, @TableName, @ColName
END

CLOSE C2

-- Remove unreferenced SIZE_CURVE rows

CREATE TABLE #SZCRVRIDTEMP (SIZE_CURVE_RID int)

INSERT #SZCRVRIDTEMP
	SELECT TOP 10000 SIZE_CURVE_RID FROM #SZCRVTEMP

SET @RowsToDelete = @@rowcount
SET @Count = 0

WHILE @RowsToDelete > 0
BEGIN
	BEGIN TRANSACTION
		DELETE FROM SIZE_CURVE_JOIN
			WHERE SIZE_CURVE_RID IN (SELECT SIZE_CURVE_RID FROM #SZCRVRIDTEMP)
		DELETE FROM SIZE_CURVE
			WHERE SIZE_CURVE_RID IN (SELECT SIZE_CURVE_RID FROM #SZCRVRIDTEMP)
		DELETE FROM #SZCRVTEMP
			WHERE SIZE_CURVE_RID IN (SELECT SIZE_CURVE_RID FROM #SZCRVRIDTEMP)
	COMMIT

	TRUNCATE TABLE #SZCRVRIDTEMP

	INSERT #SZCRVRIDTEMP
		SELECT TOP 10000 SIZE_CURVE_RID FROM #SZCRVTEMP

	SET @RowsToDelete = @@rowcount
END

-- Enable constraint checking on all foreign key tables

OPEN C2

FETCH NEXT FROM C2 INTO @Constraint, @TableName, @ColName

WHILE @@FETCH_STATUS = 0
BEGIN
    SET @Command = 'ALTER TABLE ' + @TableName + ' CHECK CONSTRAINT ' + @Constraint
    EXEC sp_executesql @Command
    FETCH NEXT FROM C2 INTO @Constraint, @TableName, @ColName
END

CLOSE C2
DEALLOCATE C2

-- Restore Recover Mode to original

SET @Command = 'ALTER DATABASE ' +  db_name() + ' SET RECOVERY ' + @RecoveryModel
EXEC (@Command)

DROP TABLE #DEFSZCRVTEMP
DROP TABLE #SZCRVTEMP
DROP TABLE #SZCRVRIDTEMP

go