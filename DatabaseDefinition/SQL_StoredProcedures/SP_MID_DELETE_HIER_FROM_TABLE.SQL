-- =============================================
-- Author:		Geoffrey Taylor
-- Create date: 2/6/2012
-- Description:	this stored procedure will remove
--				rows from a table based on
--				HN_RID and ST_RID
-- =============================================
CREATE PROCEDURE [dbo].[SP_MID_DELETE_HIER_FROM_TABLE] 
	@HN_RID INT,
	@ST_RID INT,
	@TABLENAME VARCHAR(MAX),
	@ReturnCode INT OUTPUT
	WITH RECOMPILE
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @SQL NVARCHAR(MAX)

	SET @SQL = N'DELETE FROM ' + @TABLENAME + ' WHERE HN_RID = ' + CONVERT(VARCHAR, @HN_RID) + ' AND ST_RID = ' + CONVERT(VARCHAR, @ST_RID)

	EXECUTE sp_executesql @SQL

	SET @ReturnCode = @@ERROR

	RETURN @ReturnCode
END
GO


