--dv =============================================
--dv Create date:	10/16/2013
--dv Description:	Gets headers based on the plan hierarchy node passed in
--dv =============================================
CREATE PROCEDURE [dbo].[MID_HEADER_READ_FROM_PLAN_NODE]
	@PLAN_HNRID int
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
    WHERE PLAN_HNRID = @PLAN_HNRID

END
GO
