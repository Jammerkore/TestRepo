--dv =============================================
--dv Modified date: 2/18/2014
--dv Flags: REFERENCED_FROM_SQL_ONLY
--dv Description: Utility stored procedure that will update statistics on all tables
--dv =============================================
CREATE PROCEDURE [dbo].[SP_MID_UPDATE_STATISTICS] 
AS
DECLARE @tableName VARCHAR(50)
DECLARE @CMD VARCHAR(8000)

DECLARE c1 CURSOR
FOR
SELECT TABLE_NAME
FROM INFORMATION_SCHEMA.TABLES
WHERE TABLE_TYPE = 'BASE TABLE'

OPEN c1

FETCH NEXT
FROM c1
INTO @tableName

WHILE @@FETCH_STATUS = 0
BEGIN
	SET @CMD = 'update statistics ' + @tableName

	--PRINT @CMD
	EXEC (@CMD)

	FETCH NEXT
	FROM c1
	INTO @tableName
END

CLOSE c1

DEALLOCATE c1
GO


