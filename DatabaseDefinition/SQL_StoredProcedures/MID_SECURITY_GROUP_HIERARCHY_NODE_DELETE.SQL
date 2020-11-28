--dv =============================================
--dv Create date: 2/4/2014
--dv Description: Deletes a hierarchy node security gorup
--dv =============================================
CREATE PROCEDURE [dbo].[MID_SECURITY_GROUP_HIERARCHY_NODE_DELETE]
	@GROUP_RID int, 
	@HN_RID int, 
	@ACTION_ID int, 
	@SEC_TYPE int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE 
	FROM SECURITY_GROUP_HIERARCHY_NODE 
	WHERE GROUP_RID = @GROUP_RID 
		  AND HN_RID = @HN_RID
		  AND ACTION_ID = @ACTION_ID
		  AND SEC_TYPE = @SEC_TYPE

	SELECT @@ROWCOUNT;

END
GO
