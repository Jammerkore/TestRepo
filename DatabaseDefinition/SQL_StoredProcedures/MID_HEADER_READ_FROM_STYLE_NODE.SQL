--dv =============================================
--dv Create date:	10/15/2013
--dv Description:	Returns header based on style hierarchy node RID passed in
--dv =============================================
CREATE PROCEDURE [dbo].[MID_HEADER_READ_FROM_STYLE_NODE]
	@STYLE_HNRID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		HDR_RID, 
		HDR_ID, 
		ALLOCATION_STATUS_FLAGS, 
		SHIPPING_STATUS_FLAGS 
	FROM HEADER
    WHERE STYLE_HNRID = @STYLE_HNRID
    ORDER BY EARLIEST_SHIP_DAY, RELEASE_DATETIME, HDR_ID

END
GO
