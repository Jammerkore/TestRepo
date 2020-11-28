-- =============================================
-- Author:		Geoffrey Taylor
-- Create date: 2/13/2012
-- Description:	this stored procedure will
--				delete from tablename where columname = RID
-- =============================================
CREATE PROCEDURE [dbo].[SP_MID_DELETE_HIER_BY_COL] 
	@RID INT,
	@COLUMNAME VARCHAR(MAX),
	@TABLENAME VARCHAR(MAX),
	@ReturnCode INT OUTPUT
	WITH RECOMPILE
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @SQL NVARCHAR(MAX)

	SET @SQL = N'DELETE FROM ' + @TABLENAME + ' WHERE ' + @COLUMNAME + ' = ' + CONVERT(VARCHAR, @RID)

	EXECUTE sp_executesql @SQL

	SET @ReturnCode = @@ERROR

	RETURN @ReturnCode
END
GO


