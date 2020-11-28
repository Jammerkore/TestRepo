--dv =============================================
--dv Create date: 2/5/2014
--dv Description: Updates the override low level for the user plan
--dv =============================================
CREATE PROCEDURE [dbo].[MID_USER_PLAN_UPDATE_CUSTOM_OLL] 
	@USER_RID INT,
	@CUSTOM_OLL_RID INT
AS
BEGIN
	SET NOCOUNT ON;
	
	UPDATE USER_PLAN 
	SET 
		CUSTOM_OLL_RID = @CUSTOM_OLL_RID
	WHERE USER_RID = @USER_RID

	SELECT @@ROWCOUNT;

END
GO