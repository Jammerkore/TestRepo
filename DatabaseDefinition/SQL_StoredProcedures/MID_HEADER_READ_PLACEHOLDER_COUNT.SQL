--dv =============================================
--dv Create date:	8/13/2014
--dv Description:	Returns the count of place holder headers in the assortment (or group allocation). 
--dv =============================================
CREATE PROCEDURE [dbo].[MID_HEADER_READ_PLACEHOLDER_COUNT]
	@PLACEHOLDER_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		COUNT(*) MyCount 
	FROM HEADER h 
	WHERE h.PLACEHOLDER_RID = @PLACEHOLDER_RID and h.PLACEHOLDER_RID <> h.HDR_RID and DISPLAY_TYPE not in (800739, 800740)

END
GO
