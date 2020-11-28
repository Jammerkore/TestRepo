--dv =============================================
--dv Create date: 2/4/2014
--dv Description: Deletes hierarchy security user node actions, based on user and node passed in
--dv =============================================
CREATE PROCEDURE [dbo].[MID_SECURITY_USER_HIERARCHY_NODE_DELETE_FROM_USER_AND_NODE]
	@USER_RID int, 
	@HN_RID int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE
	FROM SECURITY_USER_HIERARCHY_NODE 
	WHERE USER_RID = @USER_RID
		  AND HN_RID = @HN_RID

	SELECT @@ROWCOUNT;

END
GO

