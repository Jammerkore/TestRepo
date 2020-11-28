--dv =============================================
--dv Create date: 2/4/2014
--dv Description: Deletes security group actions, based on group and version passed in
--dv =============================================
CREATE PROCEDURE [dbo].[MID_SECURITY_GROUP_VERSION_DELETE_FROM_GROUP_AND_VERSION]
	@GROUP_RID int, 
	@FV_RID int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE 
	FROM SECURITY_GROUP_VERSION 
	WHERE GROUP_RID = @GROUP_RID
		  AND FV_RID = @FV_RID

	SELECT @@ROWCOUNT;

END
GO
