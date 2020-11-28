--dv =============================================
--dv Create date:	02/07/2014
--dv Description:	Returns a count of all Size Codes
--dv =============================================
CREATE PROCEDURE [dbo].[MID_SIZES_READ_COUNT]
	@SIZE_CODE_PRIMARY varchar(100)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		COUNT(*) 'MyCount'
    FROM SIZES
    WHERE SIZE_CODE_PRIMARY = @SIZE_CODE_PRIMARY

END
GO