--dv =============================================
--dv Create date: 2/3/2014
--dv Description: Reads security functions for users
--dv =============================================
CREATE PROCEDURE [dbo].[MID_SECURITY_USER_FUNCTION_READ_FOR_VERSION]
	@USER_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		FV.FV_RID FV_RID, 
		FV_ID, 
		coalesce(DESCRIPTION, ' ') DESCRIPTION, 
		ACTION_ID, 
		SEC_TYPE, 
		SEC_LVL_ID
	FROM FORECAST_VERSION FV, 
		 SECURITY_USER_VERSION SUV
	WHERE FV.FV_RID = SUV.FV_RID 
		  AND USER_RID = @USER_RID
	ORDER BY DESCRIPTION

END
GO
