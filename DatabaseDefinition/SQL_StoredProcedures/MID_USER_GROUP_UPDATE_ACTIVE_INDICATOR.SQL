--dv =============================================
--dv Create date: 1/15/2014
--dv Description: Updates the active indicator flag for the user group
--dv =============================================
CREATE PROCEDURE [dbo].[MID_USER_GROUP_UPDATE_ACTIVE_INDICATOR]
	@GROUP_RID int,
	@GROUP_ACTIVE_IND char(1)
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE USER_GROUP
	SET
		GROUP_ACTIVE_IND = @GROUP_ACTIVE_IND
	WHERE GROUP_RID = @GROUP_RID

	SELECT @@ROWCOUNT;
END
GO
