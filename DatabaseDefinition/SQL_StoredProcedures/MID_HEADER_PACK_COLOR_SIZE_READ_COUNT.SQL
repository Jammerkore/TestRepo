--dv =============================================
--dv Create date:	10/16/2013
--dv Description:	Gets header pack color size count based on the header pack rid passed in
--dv =============================================
CREATE PROCEDURE [dbo].[MID_HEADER_PACK_COLOR_SIZE_READ_COUNT]
	@HDR_PACK_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
		COUNT(*) AS MyCount 
	FROM HEADER_PACK_COLOR_SIZE
    WHERE HDR_PACK_RID = @HDR_PACK_RID

END
GO
