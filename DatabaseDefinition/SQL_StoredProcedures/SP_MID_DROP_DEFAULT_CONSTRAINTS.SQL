--dv =============================================
--dv Modified date: 2/18/2014
--dv Flags: REFERENCED_FROM_SQL_ONLY
--dv Description:	Utility script that has been used as part of the upgrade process when a column with a default is being deleted
--dv History: 4.2.0.5 07-09-2012 - RBeck - TT#1337 - Schemas Different Across Brands
--dv =============================================
CREATE PROCEDURE [dbo].[SP_MID_DROP_DEFAULT_CONSTRAINTS] 
	(
	@tableName VARCHAR(100),
	@columnName VARCHAR(100)
	)
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @dfName VARCHAR(100)

	SELECT @dfName = df.[name]
	FROM sys.columns c
	JOIN sys.default_constraints df ON df.parent_object_id = c.object_id
		AND df.parent_column_id = c.column_id
	WHERE c.object_id = OBJECT_ID(@tableName)
		AND c.[name] = @columnName

	IF @dfName IS NOT NULL
	BEGIN
		EXEC ('ALTER TABLE ' + @tableName + ' DROP CONSTRAINT ' + @dfName)

		PRINT 'Constraint [' + @dfName + '] dropped for ' + @tableName + '.' + @columnName
	END
	ELSE
		PRINT 'Constraint not found for ' + @tableName + '.' + @columnName
END
GO


