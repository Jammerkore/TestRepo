--dv =============================================
--dv Create date:	02/07/2014
--dv Description:	Returns a count of all Size Codes
--dv =============================================
CREATE PROCEDURE [dbo].[MID_SIZE_CODE_READ_COUNT]
	@SIZE_CODE_ID varchar(50)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		COUNT(*) 'MyCount'
    FROM SIZE_CODE
    WHERE SIZE_CODE_ID = @SIZE_CODE_ID

END
GO

