--dv =============================================
--dv Create date:	9/20/2013
--dv Modified date: 2/19/2014
--dv Flags: REFERENCED_FROM_SQL_ONLY
--dv Description: Utility stored procedure.  Intended for one-time use after 5.21 Post Conversion has been installed.
--dv			  Makes stored procedures and gets the database ready for store removal
--dv History:		10/7/2014 TT#1328-MD -jsobek -Error with SP_MID_STORE_REMOVAL_BUILD_CONSTRAINT_PROCS
--dv        		11/23/2015 TT#4828 - JSmith - Edit Header filter throws error after Store purge
--dv =============================================
CREATE PROCEDURE [dbo].[SP_MID_STORE_REMOVAL_BUILD_CONSTRAINT_PROCS] 
AS
BEGIN
	DECLARE @constraints_dropped BIT

	SELECT @constraints_dropped = CONSTRAINTS_DROPPED
	FROM SYSTEM_OPTIONS

	IF @constraints_dropped = '1'
	BEGIN
		SELECT 'WARNING: The system indicates that constraints have already been dropped. This procedure cannot be run until constraints are re-added.'

		RETURN
	END

	SET NOCOUNT ON

	IF EXISTS (
			SELECT *
			FROM dbo.sysobjects
			WHERE id = object_id(N'[dbo].[STORE_REMOVAL_REF_INTEGRITY]')
				AND OBJECTPROPERTY(id, N'IsUserTable') = 1
			)
		DROP TABLE [dbo].[STORE_REMOVAL_REF_INTEGRITY]

	CREATE TABLE STORE_REMOVAL_REF_INTEGRITY (
		RowId INT PRIMARY KEY IDENTITY(1, 1),
		ForeignKeyConstraintName NVARCHAR(200),
		ForeignKeyConstraintTableSchema NVARCHAR(200),
		ForeignKeyConstraintTableName NVARCHAR(200),
		ForeignKeyConstraintColumnName NVARCHAR(200),
		ForeignKeyConstraintColumnSeq INT,
		PrimaryKeyConstraintName NVARCHAR(200),
		PrimaryKeyConstraintTableSchema NVARCHAR(200),
		PrimaryKeyConstraintTableName NVARCHAR(200),
		PrimaryKeyConstraintColumnName NVARCHAR(200)
		)

	INSERT INTO STORE_REMOVAL_REF_INTEGRITY (
		ForeignKeyConstraintName,
		ForeignKeyConstraintTableSchema,
		ForeignKeyConstraintTableName,
		ForeignKeyConstraintColumnName,
		ForeignKeyConstraintColumnSeq
		)
	SELECT U.CONSTRAINT_NAME,
		U.TABLE_SCHEMA,
		U.TABLE_NAME,
		U.COLUMN_NAME,
		U.ORDINAL_POSITION
	FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE U
	INNER JOIN INFORMATION_SCHEMA.TABLE_CONSTRAINTS C ON U.CONSTRAINT_NAME = C.CONSTRAINT_NAME
	WHERE C.CONSTRAINT_TYPE = 'FOREIGN KEY'

	SELECT *
	FROM STORE_REMOVAL_REF_INTEGRITY
	WHERE PrimaryKeyConstraintTableName = 'ARCHIVE_BULK_SIZE_ALLOCATION'

	UPDATE STORE_REMOVAL_REF_INTEGRITY
	SET PrimaryKeyConstraintName = UNIQUE_CONSTRAINT_NAME
	FROM STORE_REMOVAL_REF_INTEGRITY T
	INNER JOIN INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS R ON T.ForeignKeyConstraintName = R.CONSTRAINT_NAME

	UPDATE STORE_REMOVAL_REF_INTEGRITY
	SET PrimaryKeyConstraintTableSchema = TABLE_SCHEMA,
		PrimaryKeyConstraintTableName = TABLE_NAME
	FROM STORE_REMOVAL_REF_INTEGRITY T
	INNER JOIN INFORMATION_SCHEMA.TABLE_CONSTRAINTS C ON T.PrimaryKeyConstraintName = C.CONSTRAINT_NAME

	UPDATE STORE_REMOVAL_REF_INTEGRITY
	SET PrimaryKeyConstraintColumnName = COLUMN_NAME
	FROM STORE_REMOVAL_REF_INTEGRITY T
	INNER JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE U ON T.PrimaryKeyConstraintName = U.CONSTRAINT_NAME
	WHERE T.ForeignKeyConstraintColumnSeq = U.ORDINAL_POSITION

	--SELECT * FROM STORE_REMOVAL_REF_INTEGRITY
	--where PrimaryKeyConstraintTableName = 'AUDIT_OTS_FORECAST_SET'
	DECLARE @strsql NVARCHAR(max)

	--------------------------------------------
	--BUILD DROP CONSTRAINT STORED PROCEDURE
	--------------------------------------------
	SET @strsql = 'IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(''[dbo].[SP_MID_STORE_REMOVAL_DROP_CONSTRAINTS]'') AND OBJECTPROPERTY(id, ''IsProcedure'') = 1)
DROP PROCEDURE [dbo].[SP_MID_STORE_REMOVAL_DROP_CONSTRAINTS]'

	EXEC (@strsql)

	--BEGIN TT#4884 - DOConnell - Need to have Explicit [DBO] in the create statement for stored procedures
	--SET @strsql = 'CREATE PROCEDURE SP_MID_STORE_REMOVAL_DROP_CONSTRAINTS' + CHAR(13) + CHAR(10) + 'as' + CHAR(13) + CHAR(10) + 'BEGIN' + CHAR(13) + CHAR(10)
	SET @strsql = 'CREATE PROCEDURE [dbo].[SP_MID_STORE_REMOVAL_DROP_CONSTRAINTS]' + CHAR(13) + CHAR(10) + 'as' + CHAR(13) + CHAR(10) + 'BEGIN' + CHAR(13) + CHAR(10)
	--END TT#4884 - DOConnell - Need to have Explicit [DBO] in the create statement for stored procedures

	SELECT @strsql = @strsql + '
IF EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N''' + QUOTENAME(ForeignKeyConstraintTableSchema) + '.' + QUOTENAME(ForeignKeyConstraintName) + ''') AND parent_object_id = OBJECT_ID(N''' + QUOTENAME(ForeignKeyConstraintTableSchema) + '.' + QUOTENAME(ForeignKeyConstraintTableName) + '''))' + CHAR(13) + CHAR(10) + 'ALTER TABLE [' + ForeignKeyConstraintTableSchema + '].[' + ForeignKeyConstraintTableName + ']
DROP CONSTRAINT ' + ForeignKeyConstraintName
	FROM STORE_REMOVAL_REF_INTEGRITY

	SET @strsql = CHAR(13) + CHAR(10) + @strsql + ' UPDATE SYSTEM_OPTIONS set CONSTRAINTS_DROPPED = ''1''' + CHAR(13) + CHAR(10)
	SET @strsql = @strsql + CHAR(13) + CHAR(10) + 'END'

	EXEC (@strsql)

	--------------------------------------------
	--BUILD ADD CONSTRAINT STORED PROCEDURE
	--------------------------------------------
	SET @strsql = 'IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(''[dbo].[SP_MID_STORE_REMOVAL_ADD_CONSTRAINTS]'') AND OBJECTPROPERTY(id, ''IsProcedure'') = 1)
DROP PROCEDURE [dbo].[SP_MID_STORE_REMOVAL_ADD_CONSTRAINTS]'

	EXEC (@strsql)

	DECLARE @ForeignKeyConstraintName NVARCHAR(200)
	DECLARE @ForeignKeyConstraintTableSchema NVARCHAR(200)
	DECLARE @ForeignKeyConstraintTableName NVARCHAR(200)
	DECLARE @ForeignKeyConstraintColumnName NVARCHAR(200)
	DECLARE @PrimaryKeyConstraintTableName NVARCHAR(200)
	DECLARE @PrimaryKeyConstraintTableSchema NVARCHAR(200)
	DECLARE @PrimaryKeyConstraintColumnName NVARCHAR(200)
	DECLARE @FKNames NVARCHAR(max)
	DECLARE @PrimNames NVARCHAR(max)
	DECLARE @Cmd NVARCHAR(max)
	DECLARE @Count INT
	DECLARE @delete_referential_action tinyint
	DECLARE @delete_referential_action_desc NVARCHAR(200)

	--BEGIN TT#4884 - DOConnell - Need to have Explicit [DBO] in the create statement for stored procedures
	--SET @strsql = 'CREATE PROCEDURE SP_MID_STORE_REMOVAL_ADD_CONSTRAINTS' + CHAR(13) + CHAR(10) + 'as' + CHAR(13) + CHAR(10) + 'BEGIN' + CHAR(13) + CHAR(10)
	SET @strsql = 'CREATE PROCEDURE [dbo].[SP_MID_STORE_REMOVAL_ADD_CONSTRAINTS]' + CHAR(13) + CHAR(10) + 'as' + CHAR(13) + CHAR(10) + 'BEGIN' + CHAR(13) + CHAR(10)
	--END TT#4884 - DOConnell - Need to have Explicit [DBO] in the create statement for stored procedures

	--Set @strsql = ''
	DECLARE @C1 CURSOR SET @C1 = CURSOR
	FOR
	SELECT DISTINCT ForeignKeyConstraintName,
		PrimaryKeyConstraintTableName
	FROM STORE_REMOVAL_REF_INTEGRITY

	OPEN @C1

	FETCH NEXT
	FROM @C1
	INTO @ForeignKeyConstraintName,
		@PrimaryKeyConstraintTableName

	WHILE (@@FETCH_STATUS = 0)
	BEGIN
		SELECT @ForeignKeyConstraintColumnName = ForeignKeyConstraintColumnName,
			@ForeignKeyConstraintTableSchema = ForeignKeyConstraintTableSchema,
			@ForeignKeyConstraintTableName = ForeignKeyConstraintTableName,
			@PrimaryKeyConstraintTableSchema = PrimaryKeyConstraintTableSchema,
			@PrimaryKeyConstraintColumnName = PrimaryKeyConstraintColumnName,
			@delete_referential_action = F.delete_referential_action,
			@delete_referential_action_desc = F.delete_referential_action_desc
		FROM STORE_REMOVAL_REF_INTEGRITY ri
		INNER JOIN sys.foreign_keys F on F.name = ri.ForeignKeyConstraintName
		WHERE ForeignKeyConstraintName = @ForeignKeyConstraintName

		SELECT @Count = count(*)
		FROM STORE_REMOVAL_REF_INTEGRITY
		WHERE PrimaryKeyConstraintTableName = @PrimaryKeyConstraintTableName
			AND ForeignKeyConstraintName = @ForeignKeyConstraintName

		IF @Count > 1
		BEGIN
			SET @FKNames = ''

			SELECT @FKNames = COALESCE(@FKNames + ', ', '') + '[' + ForeignKeyConstraintColumnName + ']'
			FROM STORE_REMOVAL_REF_INTEGRITY
			WHERE PrimaryKeyConstraintTableName = @PrimaryKeyConstraintTableName
				AND ForeignKeyConstraintName = @ForeignKeyConstraintName
			ORDER BY ForeignKeyConstraintColumnSeq

			SET @FKNames = SUBSTRING(@FKNames, 3, 1000)
			SET @PrimNames = ''

			SELECT @PrimNames = COALESCE(@PrimNames + ', ', '') + '[' + PrimaryKeyConstraintColumnName + ']'
			FROM STORE_REMOVAL_REF_INTEGRITY
			WHERE PrimaryKeyConstraintTableName = @PrimaryKeyConstraintTableName
				AND ForeignKeyConstraintName = @ForeignKeyConstraintName
			ORDER BY ForeignKeyConstraintColumnSeq

			SET @PrimNames = SUBSTRING(@PrimNames, 3, 1000)
		END
		ELSE
		BEGIN
			SET @FKNames = @ForeignKeyConstraintColumnName
			SET @PrimNames = @PrimaryKeyConstraintColumnName
		END

		SELECT @strsql = @strsql + 'ALTER TABLE [' + @ForeignKeyConstraintTableSchema + '].[' + @ForeignKeyConstraintTableName + ']
		  ADD CONSTRAINT ' + @ForeignKeyConstraintName + ' FOREIGN KEY(' + @FKNames + ') REFERENCES [' + @PrimaryKeyConstraintTableSchema + '].[' + @PrimaryKeyConstraintTableName + '](' + @PrimNames + ')' 
		if @delete_referential_action = 1 SELECT @strsql = @strsql + ' ON DELETE ' + @delete_referential_action_desc
		SELECT @strsql = @strsql + CHAR(13) + CHAR(10) + CHAR(13) + CHAR(10)

		IF @ForeignKeyConstraintName = 'ARCHIVE_BULK_SIZE_ALLOCATION_FK2'
			PRINT @ForeignKeyConstraintName

		FETCH NEXT
		FROM @C1
		INTO @ForeignKeyConstraintName,
			@PrimaryKeyConstraintTableName
	END

	CLOSE @C1

	DEALLOCATE @C1;

	SET @strsql = CHAR(13) + CHAR(10) + @strsql + ' UPDATE SYSTEM_OPTIONS set CONSTRAINTS_DROPPED = ''0''' + CHAR(13) + CHAR(10)
	SET @strsql = @strsql + CHAR(13) + CHAR(10) + 'END'

	EXEC (@strsql)
END
GO


