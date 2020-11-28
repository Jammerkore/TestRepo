--dv =============================================
--dv Create date: 2/4/2014
--dv Description: Deletes hierarchy node security functions, from the group and node passed in
--dv =============================================
CREATE PROCEDURE [dbo].[MID_SECURITY_GROUP_HIERARCHY_NODE_DELETE_FROM_GROUP_AND_NODE]
	@GROUP_RID int, 
	@HN_RID int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE 
	FROM SECURITY_GROUP_HIERARCHY_NODE 
	WHERE GROUP_RID = @GROUP_RID
		  AND HN_RID = @HN_RID

	SELECT @@ROWCOUNT;

END
GO
