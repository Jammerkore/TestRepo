-- =============================================
-- Author:		Geoffrey Taylor
-- Create date: 2/13/2012
-- Description:	this stored procedure will 
--				udpate tablename 
--				set columnname = value 
--				where hn_rid = hn_rid
-- =============================================
CREATE PROCEDURE [dbo].[SP_MID_UPDATE_COLUMN_BY_TABLE] 
	@HN_RID INT,
	@ST_RID INT,
	@COLUMNNAME VARCHAR(MAX),
	@TABLENAME VARCHAR(MAX),
	@COLVALUE INT,
	@ReturnCode INT OUTPUT
	WITH RECOMPILE
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @SQL NVARCHAR(MAX)

	IF (@COLVALUE = - 1)
	BEGIN
		IF (@ST_RID = - 1)
			SET @SQL = N'UPDATE ' + @TABLENAME + ' SET ' + @COLUMNNAME + ' = NULL WHERE HN_RID = ' + CONVERT(VARCHAR, @HN_RID)
		ELSE
			SET @SQL = N'UPDATE ' + @TABLENAME + ' SET ' + @COLUMNNAME + ' = NULL WHERE HN_RID = ' + CONVERT(VARCHAR, @HN_RID) + ' AND ST_RID = ' + CONVERT(VARCHAR, @ST_RID)
	END
	ELSE
	BEGIN
		IF (@ST_RID = - 1)
			SET @SQL = N'UPDATE ' + @TABLENAME + ' SET ' + @COLUMNNAME + ' = ' + CONVERT(VARCHAR, @COLVALUE) + ' WHERE HN_RID = ' + CONVERT(VARCHAR, @HN_RID)
		ELSE
			SET @SQL = N'UPDATE ' + @TABLENAME + ' SET ' + @COLUMNNAME + ' = ' + CONVERT(VARCHAR, @COLVALUE) + ' WHERE HN_RID = ' + CONVERT(VARCHAR, @HN_RID) + ' AND ST_RID = ' + CONVERT(VARCHAR, @ST_RID)
	END

	EXECUTE sp_executesql @SQL

	SET @ReturnCode = @@ERROR

	RETURN @ReturnCode
END
GO


