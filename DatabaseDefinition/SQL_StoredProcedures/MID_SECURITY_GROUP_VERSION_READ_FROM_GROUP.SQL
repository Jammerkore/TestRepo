--dv =============================================
--dv Create date: 1/16/2014
--dv Description: Reads security group version assignments from the provided group
--dv =============================================
CREATE PROCEDURE [dbo].[MID_SECURITY_GROUP_VERSION_READ_FROM_GROUP]
	@GROUP_RID int
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
	FROM SECURITY_GROUP_VERSION SGV, 
		 FORECAST_VERSION FV
	WHERE SGV.FV_RID = FV.FV_RID 
		  AND GROUP_RID = @GROUP_RID
	ORDER BY DESCRIPTION

END
GO