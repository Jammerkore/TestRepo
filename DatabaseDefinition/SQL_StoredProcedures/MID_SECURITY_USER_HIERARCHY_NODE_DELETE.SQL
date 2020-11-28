--dv =============================================
--dv Create date: 2/4/2014
--dv Description: Deletes a hierarchy security user node action
--dv =============================================
CREATE PROCEDURE [dbo].[MID_SECURITY_USER_HIERARCHY_NODE_DELETE]
	@USER_RID int, 
	@HN_RID int, 
	@ACTION_ID int, 
	@SEC_TYPE int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE 
	FROM SECURITY_USER_HIERARCHY_NODE 
	WHERE USER_RID=@USER_RID
		  AND HN_RID=@HN_RID
		  AND ACTION_ID=@ACTION_ID
		  AND SEC_TYPE = @SEC_TYPE

	SELECT @@ROWCOUNT;

END
GO

