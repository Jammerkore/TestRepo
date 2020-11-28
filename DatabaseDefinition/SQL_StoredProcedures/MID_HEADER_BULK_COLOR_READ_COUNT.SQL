--dv =============================================
--dv Create date:	10/16/2013
--dv Description:	Gets header bulk color count based on the header rid passed in
--dv =============================================
CREATE PROCEDURE [dbo].[MID_HEADER_BULK_COLOR_READ_COUNT]
	@HDR_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		COUNT(*) AS MyCount 
	FROM HEADER_BULK_COLOR
    WHERE HDR_RID = @HDR_RID

END
GO
