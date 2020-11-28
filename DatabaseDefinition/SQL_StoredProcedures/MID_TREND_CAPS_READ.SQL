--dv =============================================
--dv Create date:	11/21/2013
--dv Description:	Reads basis rows from the trend caps table based on method
--dv =============================================
CREATE PROCEDURE [dbo].[MID_TREND_CAPS_READ] 
	@METHOD_RID int
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

END
GO