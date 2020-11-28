--dv =============================================
--dv Create date:	02/07/2014
--dv Description:	Returns a count of all Size Codes
--dv =============================================
CREATE PROCEDURE [dbo].[MID_DIMENSIONS_READ_COUNT]
	@SIZE_CODE_SECONDARY varchar(100)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		COUNT(*) 'MyCount'
    FROM DIMENSIONS
    WHERE SIZE_CODE_SECONDARY = @SIZE_CODE_SECONDARY

END
GO