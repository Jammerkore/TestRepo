--dv =============================================
--dv Create date: 1/16/2014
--dv Description: Updates the name for the user group
--dv =============================================
CREATE PROCEDURE [dbo].[MID_USER_GROUP_UPDATE_NAME]
	@GROUP_RID int,
	@GROUP_NAME varchar(30)
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE USER_GROUP 
	SET
		GROUP_NAME = @GROUP_NAME
	WHERE GROUP_RID = @GROUP_RID

	SELECT @@ROWCOUNT;
END
GO