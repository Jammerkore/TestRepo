--dv =============================================
--dv Create date:	10/16/2013
--dv Description:	Gets header bulk color size count based on the header rid passed in
--dv =============================================
CREATE PROCEDURE [dbo].[MID_HEADER_BULK_COLOR_SIZE_READ_COUNT]
	@HDR_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		COUNT(DISTINCT SIZE_CODE_RID) AS MyCount 
	FROM HEADER_BULK_COLOR_SIZE
    WHERE HDR_RID = @HDR_RID
	      AND UNITS > 0

END
GO
