--dv =============================================
--dv Create date: 2/7/2014
--dv Description: Reads a count of the application text from the code
--dv =============================================
CREATE PROCEDURE [dbo].[MID_APPLICATION_TEXT_READ_COUNT_FROM_CODE]
    @TEXT_CODE int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		COUNT(*) AS MyCount 
	FROM APPLICATION_TEXT 
	WHERE TEXT_CODE = @TEXT_CODE

END
GO