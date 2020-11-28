--dv =============================================
--dv Create date: 1/15/2014
--dv Description: Unmarks the user group for deletion
--dv =============================================
CREATE PROCEDURE [dbo].[MID_USER_GROUP_UPDATE_FOR_RECOVERY]
	@GROUP_RID int
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE USER_GROUP
	SET
		GROUP_DELETE_IND = '0'
	WHERE GROUP_RID = @GROUP_RID

	SELECT @@ROWCOUNT;
END
GO
