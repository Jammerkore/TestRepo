--dv =============================================
--dv Create date:	11/21/2013
--dv Description:	Reads basis rows from the trend caps table based on method and store group level
--dv =============================================
CREATE PROCEDURE [dbo].[MID_TREND_CAPS_READ_FROM_STORE_GROUP_LEVEL] 
	@METHOD_RID int,
	@SGL_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		METHOD_RID, 
		SGL_RID, 
		TREND_CAP_ID, 
		TOL_PCT, 
		COALESCE(HIGH_LIMIT,-1) HIGH_LIMIT, 
		COALESCE(LOW_LIMIT,-1) LOW_LIMIT 
	FROM TREND_CAPS
	WHERE METHOD_RID = @METHOD_RID
		  AND SGL_RID = @SGL_RID

END
GO