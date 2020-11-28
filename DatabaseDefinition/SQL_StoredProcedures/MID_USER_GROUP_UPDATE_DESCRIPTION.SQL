--dv =============================================
--dv Create date: 1/16/2014
--dv Description: Updates the description for the user group
--dv =============================================
CREATE PROCEDURE [dbo].[MID_USER_GROUP_UPDATE_DESCRIPTION]
	@GROUP_RID int,
	@GROUP_DESCRIPTION varchar(250)
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE USER_GROUP 
	SET
		GROUP_DESCRIPTION = @GROUP_DESCRIPTION
	WHERE GROUP_RID = @GROUP_RID

	SELECT @@ROWCOUNT;
END
GO