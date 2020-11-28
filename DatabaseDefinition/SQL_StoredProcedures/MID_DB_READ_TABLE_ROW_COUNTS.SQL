--dv =============================================
--dv Create date: 1/31/2014
--dv Description: Reads the rowcounts from the database tables
--dv =============================================
CREATE PROCEDURE [dbo].[MID_DB_READ_TABLE_ROW_COUNTS]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT * 
	FROM  dbo.UDF_DB_TABLE_ROW_COUNTS()

END
GO
