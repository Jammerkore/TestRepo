--dv =============================================
--dv Create date:	8/13/2014
--dv Description:	Returns the count of actual headers in the assortment (or group allocation). Placeholders are not counted.
--dv =============================================
CREATE PROCEDURE [dbo].[MID_HEADER_READ_ASSORTMENT_COUNT]
	@ASRT_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		COUNT(*) MyCount 
	FROM HEADER h 
	WHERE h.ASRT_RID = @ASRT_RID and h.ASRT_RID <> h.HDR_RID and DISPLAY_TYPE not in (800739, 800740)

END
GO
