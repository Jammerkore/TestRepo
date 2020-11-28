--dv =============================================
--dv Create date:	2/26/2014
--dv Description:	Reads the schema from the table passed in
--dv =============================================
CREATE PROCEDURE [dbo].[MID_TABLE_READ_SCHEMA] 
	@TABLE_NAME varchar(500)
AS 
BEGIN 
	SET NOCOUNT ON;

	DECLARE @SQL VARCHAR(MAX);
	SET @SQL = 'SELECT TOP 0 * FROM ' + @TABLE_NAME + ' WITH (NOLOCK)';

	EXEC (@SQL)

END 
GO