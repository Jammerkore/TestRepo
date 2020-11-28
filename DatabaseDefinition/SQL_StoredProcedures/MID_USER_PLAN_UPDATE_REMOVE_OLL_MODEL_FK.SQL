--dv =============================================
--dv Create date:	11/30/2013
--dv Description:	Updates User Plan table removes FK for Header Override OLL Model
--dv =============================================
CREATE PROCEDURE [dbo].[MID_USER_PLAN_UPDATE_REMOVE_OLL_MODEL_FK]
	@OLL_RID int

AS
BEGIN
	SET NOCOUNT ON;

	UPDATE USER_PLAN 
	SET 
		OLL_RID = null
	WHERE OLL_RID = @OLL_RID

	SELECT @@ROWCOUNT;
END
GO