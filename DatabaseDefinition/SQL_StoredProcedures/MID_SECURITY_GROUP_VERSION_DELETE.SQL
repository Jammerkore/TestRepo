--dv =============================================
--dv Create date: 2/4/2014
--dv Description: Deletes a security group version
--dv =============================================
CREATE PROCEDURE [dbo].[MID_SECURITY_GROUP_VERSION_DELETE]
	@GROUP_RID int, 
	@FV_RID int, 
	@ACTION_ID int, 
	@SEC_TYPE int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE 
	FROM SECURITY_GROUP_VERSION 
	WHERE GROUP_RID = @GROUP_RID 
		  AND FV_RID= @FV_RID
		  AND ACTION_ID= @ACTION_ID
		  AND SEC_TYPE= @SEC_TYPE

	SELECT @@ROWCOUNT;

END
GO
