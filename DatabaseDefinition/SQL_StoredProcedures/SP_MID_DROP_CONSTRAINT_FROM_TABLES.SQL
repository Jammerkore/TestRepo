--dv =============================================
--dv Modified date: 2/18/2014
--dv Flags: REFERENCED_FROM_SQL_ONLY
--dv Description:	Utility script that has been used as part of the upgrade process when we need to drop an entire table
--dv =============================================
CREATE PROCEDURE [dbo].[SP_MID_DROP_CONSTRAINT_FROM_TABLES] 
	@CONSTRAINT_TYPE VARCHAR(30)
AS
BEGIN
	DECLARE @sql NVARCHAR(4000)

	WHILE 1 = 1
	BEGIN
		SET @sql = (
				SELECT TOP 1 'ALTER TABLE [' + TABLE_SCHEMA + '].[' + TABLE_NAME + '] DROP CONSTRAINT [' + CONSTRAINT_NAME + ']'
				-- SELECT * 
				FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS
				WHERE CONSTRAINT_TYPE = @CONSTRAINT_TYPE
				)

		IF @sql IS NULL
			BREAK

		EXEC (@sql)
	END
END
GO


