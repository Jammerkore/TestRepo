--dv =============================================
--dv Create date:	10/16/2013
--dv Description:	Gets headers from the on hand hierarchy node passed in
--dv =============================================
CREATE PROCEDURE [dbo].[MID_HEADER_READ_FROM_ON_HAND_NODE]
	@ON_HAND_HNRID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		HDR_RID, 
		HDR_ID, 
		STYLE_HNRID, 
		ALLOCATION_STATUS_FLAGS, 
		SHIPPING_STATUS_FLAGS
	FROM HEADER
    WHERE ON_HAND_HNRID = @ON_HAND_HNRID

END
GO
