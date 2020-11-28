--dv =============================================
--dv Create date: 1/16/2014
--dv Description: Removes a user from a group
--dv =============================================
CREATE PROCEDURE [dbo].[MID_USER_GROUP_JOIN_DELETE]
	@GROUP_RID int,
	@USER_RID int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE 
	FROM USER_GROUP_JOIN 
	WHERE GROUP_RID= @GROUP_RID
		   AND USER_RID = @USER_RID

	SELECT @@ROWCOUNT;
END
GO