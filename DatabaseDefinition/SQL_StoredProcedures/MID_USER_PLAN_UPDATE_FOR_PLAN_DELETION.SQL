--dv =============================================
--dv Create date:	2/13/2014
--dv Description:	Clears a user plan for plan view deletion
--dv =============================================
CREATE PROCEDURE [dbo].[MID_USER_PLAN_UPDATE_FOR_PLAN_DELETION]
	@VIEW_RID int
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE USER_PLAN 
		SET VIEW_RID = NULL 
	WHERE VIEW_RID = @VIEW_RID

	SELECT @@ROWCOUNT;
END
GO