--dv =============================================
--dv Create date:	10/16/2013
--dv Description:	Gets header bulk color size count based on the header rid passed in
--dv Purpose:		To determine if the bulk color size exists for this header
--dv =============================================
CREATE PROCEDURE [dbo].[MID_HEADER_BULK_COLOR_SIZE_EXISTS]
	@HDR_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		COUNT(*) AS MyCount 
	FROM HEADER_BULK_COLOR_SIZE
    WHERE HDR_RID = @HDR_RID

END
GO
